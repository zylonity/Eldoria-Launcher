using System;
using System.Net;
using CmlLib.Core;
using CmlLib.Core.Auth;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Modrinth;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.Downloader;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using EldoriaLauncher.Properties;
using EldoriaLauncher.MrPack;
using System.Text.Json;
using Modrinth.Models;

namespace EldoriaLauncher
{
    public partial class Form1 : Form
    {
        string offlineUsername = (string)Properties.Settings.Default["Username"];
        int ram = (int)Properties.Settings.Default["Ram"];
        string mcVer = "fabric-loader-" + (string)Properties.Settings.Default["FabricVer"] + "-" + (string)Properties.Settings.Default["MinecraftVer"];
        string mcPathStr = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";


        bool updating = false;
        public void PlayActive()
        {
            ver.Text = (string)Properties.Settings.Default["ModpackVer"];
            //Is Username Valid
            bool usernameValid = false;
            offlineUsername = (string)Properties.Settings.Default["Username"];


            if (offlineUsername.Length >= 4 && offlineUsername.Contains(" ") == false && offlineUsername != "")
            {
                usernameValid = true;
            }
            else
            {
                usernameValid = false;
            }


            if (usernameValid && updating == false)
            {
                pictureBox1.Enabled = true;
            }
            else
            {
                pictureBox1.Enabled = false;
            }

        }


        //Initialises everything
        public Form1()
        {
            InitializeComponent();
            checkFabricVer();
            pictureBox1_EnabledChanged();
            PlayActive();
        }

        private void Downloader_FileChanged(DownloadFileChangedEventArgs e)
        {
            textBox1.Text = e.FileName;
        }

        private void Downloader_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Console.WriteLine("{0}%", e.ProgressPercentage);
            RunBar.Value = e.ProgressPercentage;
        }

        void checkFabricVer()
        {
            var eldoriaIndex = JsonSerializer.Deserialize<ModIndex>(System.IO.File.ReadAllText(mcPathStr + "\\modrinth.index.json"));


            //Update mc version and fabric version properties
            Properties.Settings.Default["MinecraftVer"] = eldoriaIndex.dependencies.minecraft;
            Properties.Settings.Default["FabricVer"] = eldoriaIndex.dependencies.fabric_loader;
            Properties.Settings.Default["ModpackVer"] = eldoriaIndex.versionId;
            Properties.Settings.Default.Save();
        }

        //Pressing play in offline mode
        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.jugar_disabled;
            pictureBox1.Enabled = false;

            RunBar.Visible = true;
            Cursor.Current = Cursors.WaitCursor;

            System.Net.ServicePointManager.DefaultConnectionLimit = 256;

            ram = (int)Properties.Settings.Default["Ram"];

            var path = new MinecraftPath(mcPathStr);
            var launcher = new CMLauncher(path);

            launcher.FileChanged += Downloader_FileChanged;
            launcher.ProgressChanged += Downloader_ProgressChanged;

            // initialize fabric version loader
            var fabricVersionLoader = new FabricVersionLoader();
            var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();

            //get version
            mcVer = "fabric-loader-" + (string)Properties.Settings.Default["FabricVer"] + "-" + (string)Properties.Settings.Default["MinecraftVer"];


            
            //install
            var fabric = fabricVersions.GetVersionMetadata(mcVer);
            await fabric.SaveAsync(path);

            // update version list
            await launcher.GetAllVersionsAsync();

            var process = await launcher.CreateProcessAsync(mcVer, new MLaunchOption
            {

                MaximumRamMb = ram,
                Session = MSession.GetOfflineSession(offlineUsername),
            });

            if((bool)Properties.Settings.Default["Console"] == true)
            {
                Logging logger = new Logging();
                logger.Activate();
                logger.Show();
                this.Hide();
                await logger.StartMinecraft(process);
            }
            else
            {
                process.Start();
            }

            

            await process.WaitForExitAsync();
            this.Show();
            pictureBox1.Enabled = true;
            pictureBox1.Image = Properties.Resources.jugar1;
            RunBar.Visible = false;
        }



        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.jugar2;
            PlayActive();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.jugar1;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = Properties.Resources.jugar3;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = Properties.Resources.jugar2;
        }

        private void pictureBox1_EnabledChanged(object sender = null, EventArgs e = null)
        {
            if (pictureBox1.Enabled == true)
            {
                pictureBox1.Image = Properties.Resources.jugar1;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.jugar_disabled;
            }
        }


        //Close button
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //Minimize button
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        //Move window
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.menu2;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.menu1;
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox4.Image = Properties.Resources.menu3;
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox4.Image = Properties.Resources.menu2;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Location = this.Location;
            settings.Activate();
            settings.Show();
            this.Hide();
            settings.Update();
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.menu2;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.menu1;
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox5.Image = Properties.Resources.menu3;
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox5.Image = Properties.Resources.menu2;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Mods mods = new Mods();
            mods.Location = this.Location;
            mods.Activate();
            mods.Show();
            this.Hide();
            mods.Update();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

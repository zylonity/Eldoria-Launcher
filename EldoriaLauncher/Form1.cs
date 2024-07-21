using System;
using System.Net;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Modrinth;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using EldoriaLauncher.Properties;
using EldoriaLauncher.MrPack;
using System.Text.Json;
using Modrinth.Models;
using CmlLib.Core.ModLoaders.FabricMC;
using System.Reflection.Metadata.Ecma335;

namespace EldoriaLauncher
{
    public partial class Form1 : Form
    {
        //Initialises everything
        public Form1()
        {
            InitializeComponent();
            checkFabricVer();
            PlayActive();
            
        }

        //FORM 1 STUFF
        string offlineUsername = (string)Properties.Settings.Default["Username"];
        int ram = (int)Properties.Settings.Default["Ram"];
        string mcVer = "fabric-loader-" + (string)Properties.Settings.Default["FabricVer"] + "-" + (string)Properties.Settings.Default["MinecraftVer"];
        string mcPathStr = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";


        bool updating = false;
        public void PlayActive()
        {
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

            pictureBox1.Enabled = false;
            pictureBox4.Enabled = false;
            pictureBox5.Enabled = false;


            RunBar.Visible = true;
            textBox1.Visible = true;

            Cursor.Current = Cursors.WaitCursor;

            System.Net.ServicePointManager.DefaultConnectionLimit = 256;

            ram = (int)Properties.Settings.Default["Ram"];

            var path = new MinecraftPath(mcPathStr);
            var launcher = new MinecraftLauncher(path);

            launcher.FileProgressChanged += (sender, args) =>
            {
                textBox1.Text = args.Name;
            };
            launcher.ByteProgressChanged += (sender, args) =>
            {
                var percentage = (args.ProgressedBytes / args.TotalBytes) * 100;
                RunBar.Value = (int)percentage;
            };

            // initialize fabric version loader
            var httpCli = new HttpClient();
            var fabricInstaller = new FabricInstaller(httpCli);
            //var fabricLoaders = fabricInstaller.GetLoaders((string)Properties.Settings.Default["MinecraftVer"]);

            //var selectedLoader = fabricLoaders.Result.Find(loader => loader.Version == (string)Properties.Settings.Default["FabricVer"]);

            //get version
            mcVer = "fabric-loader-" + (string)Properties.Settings.Default["FabricVer"] + "-" + (string)Properties.Settings.Default["MinecraftVer"];



            //install
            await fabricInstaller.Install((string)Properties.Settings.Default["MinecraftVer"], (string)Properties.Settings.Default["FabricVer"], path);


            // update version list
            await launcher.GetAllVersionsAsync();

            var process = await launcher.CreateProcessAsync(mcVer, new MLaunchOption
            {

                MaximumRamMb = ram,
                Session = MSession.CreateOfflineSession(offlineUsername),
            });

            if ((bool)Properties.Settings.Default["Console"] == true)
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
            pictureBox4.Enabled = true;
            pictureBox5.Enabled = true;

            textBox1.Visible = false;
            RunBar.Visible = false;
        }



        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Enabled == true)
            {
                pictureBox1.Image = Properties.Resources.boton_jugar2;
            }

            PlayActive();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (pictureBox1.Enabled == true)
                pictureBox1.Image = Properties.Resources.boton_jugar;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Enabled == true)
                pictureBox1.Image = Properties.Resources.boton_jugar3;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Enabled == true)
                pictureBox1.Image = Properties.Resources.boton_jugar2;
        }

        private void pictureBox1_EnabledChanged(object sender, EventArgs e)
        {


            if (pictureBox1.Enabled == false)
            {
                pictureBox1.Image = Properties.Resources.boton_jugar_disabled;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.boton_jugar;
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
            pictureBox4.Image = Properties.Resources.boton_ajustes2;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.boton_ajustes;
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox4.Image = Properties.Resources.boton_ajustes3;
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox4.Image = Properties.Resources.boton_ajustes2;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            LoadSettings();
            MainPanel.Visible = false;
            SettingsPanel.Visible = true;
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.boton_mods2;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.boton_mods;
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox5.Image = Properties.Resources.boton_mods3;
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox5.Image = Properties.Resources.boton_mods2;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            MainPanel.Visible = false;
            SettingsPanel.Visible = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox5_EnabledChanged(object sender, EventArgs e)
        {
            if (pictureBox5.Enabled == false)
            {
                pictureBox5.Image = Properties.Resources.boton_mods_disabled;
            }
            else
            {
                pictureBox5.Image = Properties.Resources.boton_mods;
            }
        }

        private void pictureBox4_EnabledChanged(object sender, EventArgs e)
        {
            if (pictureBox4.Enabled == false)
            {
                pictureBox4.Image = Properties.Resources.boton_ajustes_disabled;
            }
            else
            {
                pictureBox4.Image = Properties.Resources.boton_ajustes;
            }
        }



        //SETTINGS FORM
        private void LoadSettings()
        {
            OfflineUsernameBox.Text = (string)Properties.Settings.Default["Username"];
            RamBox.SelectedIndex = (int)Properties.Settings.Default["RamIndex"];
            checkBox1.Checked = (bool)Properties.Settings.Default["Console"];
            ver.Text = (string)Properties.Settings.Default["AppVer"];
        }

        private void RamBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fid;
            bool parseOK = Int32.TryParse(RamBox.Items[RamBox.SelectedIndex].ToString(), out fid);
            Properties.Settings.Default["Ram"] = fid;
            Properties.Settings.Default["RamIndex"] = RamBox.SelectedIndex;
        }

        private void OfflineUsernameBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["Username"] = OfflineUsernameBox.Text;

            PlayActive();
        }

        //private void pictureBox1_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //    mainForm.Location = this.Location;
        //    mainForm.Show();
        //    mainForm.PlayActive();
        //}
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["Console"] = checkBox1.Checked;
        }

        private void SettingsBack_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            SettingsPanel.Visible = false;
            MainPanel.Visible = true;
        }
    }
}

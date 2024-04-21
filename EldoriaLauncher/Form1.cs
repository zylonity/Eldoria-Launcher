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


namespace EldoriaLauncher
{
    public partial class Form1 : Form
    {
        string offlineUsername = (string)Properties.Settings.Default["Username"];
        int ram = (int)Properties.Settings.Default["Ram"];
        string mcVer = "fabric-loader-0.15.10-1.20.1";

        MinecraftPath mcPath;
        string mcPathStr = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";

        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";

        CMLauncher launcher;
        

        bool validateFiles = false;
        bool updating = false;

        bool downloadFilesAsync = true;

        Dictionary<string, string> installModList = new Dictionary<string, string>();


        async Task InstallFabric(Downloading downloading)
        {
            mcPath = new MinecraftPath(mcPathStr);
            launcher = new CMLauncher(mcPath);

            downloading.DownloadBigLabel.Text = "Descargando Frabic";
            downloading.progressBar1.Visible = false;
            downloading.progressBar2.Visible = false;
            downloading.CurrentDownload.Visible = false;

            // initialize fabric version loader
            var fabricVersionLoader = new FabricVersionLoader();
            var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();

            // install
            var fabric = fabricVersions.GetVersionMetadata(mcVer);

            await fabric.SaveAsync(mcPath);
            

            await launcher.GetAllVersionsAsync();
            downloading.progressBar1.Visible = true;
            downloading.progressBar2.Visible = true;
            downloading.CurrentDownload.Visible = true;
        }

        async Task InstallMods(Downloading downloading)
        {
            //Use my key whilst the modrinth is private, update useragent when public
            var options = new ModrinthClientConfig
            {
                ModrinthToken = "mrp_p2f98ush9bEkhnAlCuDQCXP5GYj4IFdQGcsPXKn1top3lIgZRl13YicOCmuz",
                UserAgent = "Eldoria"
            };

            var client = new ModrinthClient(options);
            var project = await client.Project.GetAsync("Eldoria");
            var dp = await client.Project.GetDependenciesAsync(project.Slug);

            float itemsToDownload = dp.Projects.Length - 1;
            float itemsDownloaded = 0;

            downloading.DownloadBigLabel.Text = "Descargando Mods";

            for (int i = 0; i < dp.Projects.Length; i++)
            { 
                installModList.Add(dp.Projects[i].Title, dp.Versions[i].Files[0].Url);
            }

                //Download all the mods
            for (int i = 0; i < dp.Projects.Length; i++)
            {
                string filename = dp.Versions[i].Files[0].FileName;

                int x = i;
                if (i + 1 < dp.Projects.Length)
                    x = i + 1;

                string nextFileName = dp.Versions[x].Files[0].FileName;

                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += (s, e) =>
                {
                    downloading.progressBar2.Value = e.ProgressPercentage;
                    downloading.progressBar1.Value = (int)((itemsDownloaded / itemsToDownload) * 100);
                };
                webClient.DownloadFileCompleted += (s, e) =>
                {
                    itemsDownloaded++;
                    downloading.CurrentDownload.Text = nextFileName;

                    if ((int)itemsDownloaded == (dp.Projects.Length))
                    {
                        downloading.DownloadBigLabel.Text = "DONE";
                        downloading.Close();
                        updating = false;
                        PlayActive();
                    }
                };


                if(downloadFilesAsync == false)
                {
                    await webClient.DownloadFileTaskAsync(new Uri(dp.Versions[i].Files[0].Url), modsPath + "\\" + filename);
                }
                else
                {
                    webClient.DownloadFileAsync(new Uri(dp.Versions[i].Files[0].Url), modsPath + "\\" + filename);
                }




            }
        }


        //Currently only checks for the initial download, if files are missing then download everything from the repo
        async Task FirstDownload()
        {


            if (!Directory.Exists(mcPathStr))
            {
                updating = true;
                System.IO.Directory.CreateDirectory(modsPath);

                //Open installing window
                //Downloading downloading = new Downloading();
                //downloading.Activate();
                //downloading.Show();
                //downloading.TopMost = true;

                ////await InstallFabric(downloading);

                //await InstallMods(downloading);

                //if (downloadFilesAsync == false)
                //{
                //    downloading.Close();
                //    updating = false;
                //    PlayActive();
                //}

                Installer installer = new Installer();
                installer.Activate();
                installer.Show();
                installer.Location = this.Location;
                this.Hide();
                installer.Update();
            }


        }


        //Clones everything
        async Task InitialClone()
        {


        }

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







        //Initialises everything
        public Form1()
        {
            FirstDownload();
            InitializeComponent();
            pictureBox1_EnabledChanged();
            PlayActive();
        }



        //Pressing play in offline mode

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            System.Net.ServicePointManager.DefaultConnectionLimit = 256;

            ram = (int)Properties.Settings.Default["Ram"];


            // update version list
            await launcher.GetAllVersionsAsync();

            var process = await launcher.CreateProcessAsync(mcVer, new MLaunchOption
            {

                MaximumRamMb = ram,
                Session = MSession.GetOfflineSession(offlineUsername),
            });

            process.Start();

            this.WindowState = FormWindowState.Minimized;
            Cursor.Current = Cursors.Arrow;
            process.WaitForExit();
            pictureBox1.Enabled = true;
            this.WindowState = FormWindowState.Normal;
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

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Installer installer = new Installer();
            installer.Activate();
            installer.Show();
            installer.Location = this.Location;
            this.Hide();
            installer.Update();
            //Settings settings = new Settings();
            //settings.Activate();
            //settings.Show();
            //settings.Location = this.Location;
            //this.Hide();
            //settings.Update();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Net;
using System.IO;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.VersionLoader;
using LibGit2Sharp;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Modrinth;
using Modrinth.Exceptions;
using System.Net;
using System.Net.Http;
using Modrinth.Helpers;
using Modrinth.Extensions;
using Modrinth.Endpoints.Project;
using Modrinth.Models;
using System.Runtime.Intrinsics.Arm;
using static System.Windows.Forms.Design.AxImporter;
using CmlLib.Core.Version;

namespace EldoriaLauncher
{
    public partial class Form1 : Form
    {
        string offlineUsername = (string)Properties.Settings.Default["Username"];
        int ram = (int)Properties.Settings.Default["Ram"];
        string mcVer = "1.19.2-forge-43.2.0";
        string verPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string sRepo = "https://github.com/zylonity/Elixa-Modpack";

        bool validateFiles = false;
        bool updating = false;

        Downloading downloadWindow;



        //Currently only checks for the initial download, if files are missing then download everything from the repo
        async Task CheckUpdates()
        {
            var options = new ModrinthClientConfig
            {
                ModrinthToken = "mrp_p2f98ush9bEkhnAlCuDQCXP5GYj4IFdQGcsPXKn1top3lIgZRl13YicOCmuz",
                UserAgent = "Eldoria"
            };

            var client = new ModrinthClient(options);
            var project = await client.Project.GetAsync("Eldoria");
            var dp = await client.Project.GetDependenciesAsync(project.Slug);
            MessageBox.Show(dp.Versions[0].Files[0].Url);
            int progressBar = 0;
            int numberOfDownloads = dp.Projects.Length;

            //await Task.Run(async () =>
            //{
            //    for (int i = 1; i <= numberOfDownloads; i++)
            //    {

            //        using (WebClient wc = new WebClient())
            //        {

            //            // Move the declaration inside the loop
            //            int downloadNumber = i;

            //            var progress = new Progress<DownloadProgress>(dp => ReportProgress(dp.Percent, downloadNumber, dp.ExpectedSize, stopwatch.Elapsed, dp.TotalBytesRead));

            //            // Offload the download operation to a separate thread
            //            await wc.DownloadFileAsync(dp.Projects[i].Url, dp.Projects[i]., verPath, progress, downloadNumber);
            //        }
            //    }
            //});

            //WebClient wc = new WebClient();
            //await Task.Run(() => {
            //    wc.DownloadFileAsync(new Uri(dp.Projects[0].Url), project.Title);
            //    wc.DownloadProgressChanged += (s, e) =>
            //    {
            //        progressBar = e.ProgressPercentage;
            //    };
            //    label1.Text = progressBar.ToString();

            //});
            
            
            

            if (!Directory.Exists(verPath))
            {


            }
            try
            {

            }
            catch (ModrinthApiException e)
            {
                MessageBox.Show($"API call failed with status code {e.InnerException}");
            }
            catch (System.AggregateException f)
            {
                MessageBox.Show($"{f.InnerException}");
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

            CheckUpdates();
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

            var elixaPath = new MinecraftPath(Environment.GetEnvironmentVariable("appdata") + "\\.Elixa");

            var launcher = new CMLauncher(elixaPath);

            ram = (int)Properties.Settings.Default["Ram"];

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


        //Replaces relativePath thing that .net framwork doesnt have
        private static string GetRelativePath(string rootPath, string fullPath)
        {
            var rootUri = new Uri(rootPath.EndsWith("\\") ? rootPath : rootPath + "\\");
            var fullUri = new Uri(fullPath);
            var relativeUri = rootUri.MakeRelativeUri(fullUri);
            return Uri.UnescapeDataString(relativeUri.ToString());
        }

        //Looks through directories and compares the files.
        private static void TraverseTree(Tree tree, string currentPath, List<string> localDirectories, List<string> localFiles, ref List<string> missingDirectories, ref List<string> missingFiles)
        {
            foreach (var entry in tree)
            {
                if (entry.TargetType == TreeEntryTargetType.Blob)
                {
                    // Check for missing files
                    var fullPath = Path.Combine(currentPath, entry.Path);
                    if (!localFiles.Contains(entry.Path))
                    {
                        missingFiles.Add(entry.Path);
                    }
                }
                else if (entry.TargetType == TreeEntryTargetType.Tree)
                {
                    // Check for missing directories
                    var fullPath = Path.Combine(currentPath, entry.Path);
                    if (!localDirectories.Contains(entry.Path))
                    {
                        missingDirectories.Add(entry.Path);
                    }

                    // Recursively traverse the subdirectory
                    var subTree = (Tree)entry.Target;
                    TraverseTree(subTree, fullPath, localDirectories, localFiles, ref missingDirectories, ref missingFiles);
                }
            }
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

            Settings settings = new Settings();
            settings.Activate();
            settings.Show();
            this.Hide();
            settings.Update();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

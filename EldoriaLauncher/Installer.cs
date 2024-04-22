using Microsoft.VisualBasic.Devices;
using Modrinth;
using Modrinth.Endpoints.Project;
using Modrinth.Exceptions;
using Modrinth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static System.Windows.Forms.Design.AxImporter;

namespace EldoriaLauncher
{
    public partial class Installer : Form
    {

        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";

        bool downloadAsync = false;

        ModrinthClient client;
        ModrinthClientConfig options;
        Project project;
        Dependencies dp;

        Dictionary<string, Tuple<string, string>> installModList = new Dictionary<string, Tuple<string, string>>();

        
        public Installer()
        {
            InitializeComponent();
            GetMods();
        }

        async Task GetMods()
        {
            button1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            var userAgent = new UserAgent
            {
                ProjectName = "Eldoria-Launcher",
                ProjectVersion = "1.0.0",
                GitHubUsername = "zylonity",
                Contact = "kkhaleelkk505@gmail.com"
            };

            options = new ModrinthClientConfig
            {
                UserAgent = userAgent.ToString()
            };

            client = new ModrinthClient(options);
            

            try
            {
                project = await client.Project.GetAsync("Eldoria");
            }
            // Or you can catch the exception and handle all non-200 status codes
            catch (ModrinthApiException e)
            {
                MessageBox.Show("Error: " + e.InnerException);
            }

            dp = await client.Project.GetDependenciesAsync(project.Slug);

            //Set up dictionary with file name and url
            for (int i = 0; i < dp.Projects.Length; i++)
            {
                installModList.Add(dp.Projects[i].Title, new Tuple<string, string>(dp.Versions[i].Files[0].FileName, dp.Versions[i].Files[0].Url));
            }

            for (int i = 0; i < dp.Projects.Length; i++)
            {
                checkedListBox1.Items.Add(dp.Projects[i].Title, true);

            }
            button1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        async Task InstallMods()
        {
            Cursor.Current = Cursors.WaitCursor;
            System.IO.Directory.CreateDirectory(modsPath);

            //Use my key whilst the modrinth is private, update useragent when public
            var options = new ModrinthClientConfig
            {
                ModrinthToken = "mrp_p2f98ush9bEkhnAlCuDQCXP5GYj4IFdQGcsPXKn1top3lIgZRl13YicOCmuz",
                UserAgent = "Eldoria"
            };

            float itemsToDownload = checkedListBox1.CheckedItems.Count - 1;
            float itemsDownloaded = 0;

            downloadAsync = checkBox1.Checked;

            //Download all the mods
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {

                string filename = installModList[checkedListBox1.CheckedItems[i].ToString()].Item1;
                string downloadUrl = installModList[checkedListBox1.CheckedItems[i].ToString()].Item2;

                int x = i;
                if (i + 1 < dp.Projects.Length)
                    x = i + 1;

                string nextFileName = dp.Versions[x].Files[0].FileName;

                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += (s, e) =>
                {
                    progressBar2.Value = e.ProgressPercentage;
                    progressBar1.Value = (int)((itemsDownloaded / itemsToDownload) * 100);
                };
                webClient.DownloadFileCompleted += (s, e) =>
                {
                    itemsDownloaded++;
                    currentDownload.Text = nextFileName;

                    if (downloadAsync && (int)itemsDownloaded == (itemsToDownload + 1))
                    {
                        Application.Restart();
                    }
                };


                if (downloadAsync)
                {
                    webClient.DownloadFileAsync(new Uri(downloadUrl), modsPath + "\\" + filename);
                }
                else
                {
                    await webClient.DownloadFileTaskAsync(new Uri(downloadUrl), modsPath + "\\" + filename);
                }





            }

            if (!downloadAsync)
            {
                Application.Restart();
            }
                
        }

        //Move window
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Settings_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InstallMods();
            button1.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

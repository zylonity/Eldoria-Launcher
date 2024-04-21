using Modrinth;
using Modrinth.Endpoints.Project;
using Modrinth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Design.AxImporter;

namespace EldoriaLauncher
{
    public partial class Installer : Form
    {
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";

        ModrinthClient client;
        ModrinthClientConfig options;
        Project project;
        Dependencies dp;

        Dictionary<string, string> installModList = new Dictionary<string, string>();

        Form1 mainForm = Application.OpenForms.OfType<Form1>().Single();
        public Installer()
        {
            InitializeComponent();
            GetMods();
        }

        async Task GetMods()
        {
            options = new ModrinthClientConfig
            {
                ModrinthToken = "mrp_p2f98ush9bEkhnAlCuDQCXP5GYj4IFdQGcsPXKn1top3lIgZRl13YicOCmuz",
                UserAgent = "Eldoria"
            };

            client = new ModrinthClient(options);
            project = await client.Project.GetAsync("Eldoria");
            dp = await client.Project.GetDependenciesAsync(project.Slug);

            for (int i = 0; i < dp.Projects.Length; i++)
            {
                installModList.Add(dp.Projects[i].Title, dp.Versions[i].Files[0].Url);
            }

            for (int i = 0; i < dp.Projects.Length; i++)
            {
                checkedListBox1.Items.Add(dp.Projects[i].Title);
            }
        }

        async Task InstallMods(Downloading downloading)
        {
            //Use my key whilst the modrinth is private, update useragent when public
            var options = new ModrinthClientConfig
            {
                ModrinthToken = "mrp_p2f98ush9bEkhnAlCuDQCXP5GYj4IFdQGcsPXKn1top3lIgZRl13YicOCmuz",
                UserAgent = "Eldoria"
            };

            
            

            float itemsToDownload = dp.Projects.Length - 1;
            float itemsDownloaded = 0;

            downloading.DownloadBigLabel.Text = "Descargando Mods";

            

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
                    }
                };


                webClient.DownloadFileAsync(new Uri(dp.Versions[i].Files[0].Url), modsPath + "\\" + filename);




            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Location = this.Location;
            mainForm.Show();
            mainForm.PlayActive();

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

        }
    }
}

using Microsoft.VisualBasic.Devices;
using Modrinth;
using Modrinth.Exceptions;
using Modrinth.Models;
using System.Net;
using static System.Windows.Forms.Design.AxImporter;
using System.IO.Compression;
using EldoriaLauncher.MrPack;
using System.Text.Json;
using System.Xml.Linq;

namespace EldoriaLauncher
{
    public partial class Mods : Form
    {

        Form1 mainForm = Application.OpenForms.OfType<Form1>().Single();

        string eldoriaPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";

        bool downloadAsync = false;

        ModrinthClient client;
        UserAgent userAgent;
        ModrinthClientConfig options;
        Project project;
        Modrinth.Models.Version version;
        ModIndex eldoriaIndex;
        Dictionary<string, Tuple<string, string>> installModList = new Dictionary<string, Tuple<string, string>>();


        public Mods()
        {
            InitializeComponent();
            GetMods();
        }

        async Task GetMods()
        {

            listView1.CheckBoxes = true;
            button1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            userAgent = new UserAgent
            {
                ProjectName = "Eldoria-Launcher",
                ProjectVersion = "1.2.0",
                GitHubUsername = "zylonity",
            };

            options = new ModrinthClientConfig
            {
                UserAgent = userAgent.ToString()
            };

            client = new ModrinthClient(options);

            string[] files = Directory.GetFiles(modsPath);
            var fileNames = new List<string>();
            foreach (string file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }

            var modNames = new List<string>();

            foreach (string name in fileNames)
            {
                var search = await client.Project.SearchAsync(name);
                if (search.Hits.Count() > 0)
                {
                    modNames.Add(search.Hits[0].Title);
                    ListViewItem item1WithToolTip = new ListViewItem(search.Hits[0].Title);
                    item1WithToolTip.ToolTipText = search.Hits[0].Description;
                    listView1.Items.Add(item1WithToolTip);

                }
                else
                {
                    modNames.Add(name);
                    listView1.Items.Add(name);
                }

            }




            //Enable install UI
            checkedListBox1.Visible = true;
            progressBar2.Visible = true;
            currentDownload.Visible = true;
            button1.Visible = true;
            checkBox1.Visible = true;
            label1.Visible = true;
            label2.Visible = true;

            //Add all mods to the list
            for (int i = 0; i < modNames.Count; i++)
            {


            }

            button1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        async Task InstallMods()
        {
            Cursor.Current = Cursors.WaitCursor;
            System.IO.Directory.CreateDirectory(modsPath);

            float itemsToDownload = checkedListBox1.CheckedItems.Count - 1;
            float itemsDownloaded = 0;

            downloadAsync = checkBox1.Checked;

            //Download all the mods
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {

                string filename = installModList[checkedListBox1.CheckedItems[i].ToString()].Item1;
                string downloadUrl = installModList[checkedListBox1.CheckedItems[i].ToString()].Item2;


                int x = i;
                if (i + 1 < checkedListBox1.CheckedItems.Count)
                    x = i + 1;

                string nextFileName = checkedListBox1.CheckedItems[x].ToString();

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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Location = this.Location;
            mainForm.Show();
            mainForm.PlayActive();
        }
    }

}

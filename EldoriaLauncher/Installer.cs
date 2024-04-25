using Microsoft.VisualBasic.Devices;
using Modrinth;
using Modrinth.Endpoints.Project;
using Modrinth.Exceptions;
using Modrinth.Extensions;
using Modrinth.Helpers;
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
using System.IO.Compression;
using EldoriaLauncher.MrPack;
using System.Text.Json;

namespace EldoriaLauncher
{
    public partial class Installer : Form
    {
        string eldoriaPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";

        bool downloadAsync = false;

        ModrinthClient client;
        ModrinthClientConfig options;
        Project project;
        Modrinth.Models.Version version;
        Dependency[] verDependencies;
        Modrinth.Endpoints.Project.Dependencies projDependencies;
        ModIndex eldoriaIndex;
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
            System.IO.Directory.CreateDirectory(modsPath);

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


            version = await client.Version.GetAsync(project.Versions[project.Versions.Length - 1]);

            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += (s, e) =>
            {
                progressBar1.Value = e.ProgressPercentage;
            };
            webClient.DownloadFileCompleted += (s, e) =>
            {

            };

            //Download, extract, move files, and delete the zip file
            label3.Text = "Descargando y extrayendo info";
            await webClient.DownloadFileTaskAsync(new Uri(version.Files[0].Url), eldoriaPath + "\\" + version.Files[0].FileName + ".zip");

            ZipFile.ExtractToDirectory(eldoriaPath + "\\" + version.Files[0].FileName + ".zip", eldoriaPath);

            label3.Text = "Moviendo archivos";

            foreach (var file in new DirectoryInfo(eldoriaPath + "\\overrides").GetFiles())
            {
                file.MoveTo($@"{eldoriaPath}\{file.Name}");
            }

            foreach (var dir in new DirectoryInfo(eldoriaPath + "\\overrides").GetDirectories())
            {
                Directory.Move(dir.FullName, eldoriaPath + "\\" + dir.Name);
            }

            label3.Text = "Borrando archivos temporales";
            System.IO.File.Delete(eldoriaPath + "\\" + version.Files[0].FileName + ".zip");

            label3.Text = "Todo Hecho!";

            //Enable install UI
            checkedListBox1.Visible = true;
            progressBar2.Visible = true;
            currentDownload.Visible = true;
            button1.Visible = true;
            checkBox1.Visible = true;
            label1.Visible = true;
            label2.Visible = true;

            //Deserialize json file
            eldoriaIndex = JsonSerializer.Deserialize<ModIndex>(System.IO.File.ReadAllText(eldoriaPath + "\\modrinth.index.json"));

            //Update mc version and fabric version properties
            Properties.Settings.Default["MinecraftVer"] = eldoriaIndex.dependencies.minecraft;
            Properties.Settings.Default["FabricVer"] = eldoriaIndex.dependencies.fabric_loader;

            //Filter the path names to just file names
            for (int i = 0; i < eldoriaIndex.files.Length; i++)
            {
                eldoriaIndex.files[i].path = eldoriaIndex.files[i].path.Split("/")[1];
            }


            //Add all mods to the list
            for (int i = 0; i < eldoriaIndex.files.Length; i++)
            {

                installModList.Add(eldoriaIndex.files[i].path, new Tuple<string, string>(eldoriaIndex.files[i].path, eldoriaIndex.files[i].downloads[0]));
                checkedListBox1.Items.Add(eldoriaIndex.files[i].path, true);

            }

            button1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        async Task InstallMods()
        {
            Cursor.Current = Cursors.WaitCursor;
            System.IO.Directory.CreateDirectory(modsPath);

            var userAgent = new UserAgent
            {
                ProjectName = "Eldoria-Launcher",
                ProjectVersion = "1.1.0",
                GitHubUsername = "zylonity",
                Contact = "kkhaleelkk505@gmail.com"
            };

            options = new ModrinthClientConfig
            {
                UserAgent = userAgent.ToString()
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

    }

}

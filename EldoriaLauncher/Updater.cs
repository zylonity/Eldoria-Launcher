using Microsoft.VisualBasic.Devices;
using Modrinth;
using Modrinth.Exceptions;
using Modrinth.Models;
using System.Net;
using static System.Windows.Forms.Design.AxImporter;
using System.IO.Compression;
using EldoriaLauncher.MrPack;
using System.Text.Json;

namespace EldoriaLauncher
{
    public partial class Updater : Form
    {
        string eldoriaPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string tempUpdatePath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\Update";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";

        bool downloadAsync = false;

        ModrinthClient client;
        UserAgent userAgent;
        ModrinthClientConfig options;
        Project project;
        Modrinth.Models.Version version;
        ModIndex eldoriaIndex;
        Dictionary<string, Tuple<string, string>> installModList = new Dictionary<string, Tuple<string, string>>();


        public Updater()
        {
            InitializeComponent();
            GetNewMods();
        }

        void UpdateConfigs()
        {
            //Deal with files
            foreach (var sourceFilePath in Directory.GetFiles(tempUpdatePath + "\\updated_configs"))
            {
                var destFilePath = Path.Combine(eldoriaPath + "\\config", Path.GetFileName(sourceFilePath));

                if (System.IO.File.Exists(destFilePath))
                {
                    System.IO.File.Delete(destFilePath);
                }

                System.IO.File.Copy(sourceFilePath, destFilePath);
            }

            //Deal with folders
            foreach (var sourceSubDir in Directory.GetDirectories(tempUpdatePath + "\\updated_configs"))
            {
                var destSubDir = Path.Combine(eldoriaPath + "\\config", Path.GetFileName(sourceSubDir));

                if (Directory.Exists(destSubDir))
                {
                    Directory.Delete(destSubDir, true);
                }
                Directory.Move(sourceSubDir, destSubDir);
            }
        }

        async Task GetNewMods()
        {
            button1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            if (System.IO.Directory.Exists(tempUpdatePath))
            {
                System.IO.Directory.Delete(tempUpdatePath, true);
            }
            System.IO.Directory.CreateDirectory(tempUpdatePath);

            userAgent = new UserAgent
            {
                ProjectName = "Eldoria-Launcher",
                ProjectVersion = (string)Properties.Settings.Default["AppVer"],
                GitHubUsername = "zylonity",
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
            label3.Text = "Descargando info";
            await webClient.DownloadFileTaskAsync(new Uri(version.Files[0].Url), tempUpdatePath + "\\" + version.Files[0].FileName + ".zip");

            label3.Text = "Extrayendo info";
            ZipFile.ExtractToDirectory(tempUpdatePath + "\\" + version.Files[0].FileName + ".zip", tempUpdatePath, true);

            label3.Text = "Moviendo archivos";

            foreach (var file in new DirectoryInfo(tempUpdatePath + "\\overrides").GetFiles())
            {
                file.MoveTo($@"{tempUpdatePath}\{file.Name}");
            }

            foreach (var dir in new DirectoryInfo(tempUpdatePath + "\\overrides").GetDirectories())
            {
                Directory.Move(dir.FullName, tempUpdatePath + "\\" + dir.Name);
            }


            label3.Text = "Borrando traducciones";
            if(Directory.Exists(eldoriaPath + "\\translation_docs"))
            {
                Directory.Delete(eldoriaPath + "\\translation_docs", true);
            }

            label3.Text = "Actualizando traducciones";
            if (Directory.Exists(tempUpdatePath + "\\translation_docs"))
            {
                Directory.Move(tempUpdatePath + "\\translation_docs", eldoriaPath + "\\translation_docs");
            }
            else
            {
                MessageBox.Show("No se encuentra la carpeta con las traducciones! \n \nAprende ingles con Duolingo, la forma más popular para aprender inglés en línea. \nAprende inglés en solo 5 minutos diarios con nuestras divertidas lecciones. No importa si estás empezando con lo básico o quieres practicar tu lectura, escritura y conversación; está científicamente comprobado que Duolingo funciona.");
            }

            if (Directory.Exists(tempUpdatePath + "\\updated_configs"))
            {
                UpdateConfigs();
            }
            Directory.Delete(tempUpdatePath + "\\config", true);

            System.IO.File.Delete(tempUpdatePath + "\\" + version.Files[0].FileName + ".zip");

            ModIndex oldEldoriaIndex = JsonSerializer.Deserialize<ModIndex>(System.IO.File.ReadAllText(eldoriaPath + "\\modrinth.index.json"));
            ModIndex newEldoriaIndex = JsonSerializer.Deserialize<ModIndex>(System.IO.File.ReadAllText(tempUpdatePath + "\\modrinth.index.json"));

            var newFilesList = new List<MrPack.File>(newEldoriaIndex.files);

            //Find and delete old files
            bool found = false;

            int count = 0;
            string oldMods = "";
            foreach (var file in oldEldoriaIndex.files)
            {
                found = false;
                foreach (var nFile in newFilesList)
                {

                    if (file.path == nFile.path)
                    {
                        found = true;
                        //Detect new files by filtering old 
                        if (found && System.IO.File.Exists(eldoriaPath + "\\" + file.path))
                        {
                            newFilesList.Remove(nFile);
                        }
                        break;
                    }
                }

                if(!found && System.IO.File.Exists(eldoriaPath + "\\" + file.path))
                {
                    count++;
                    oldMods += file.path + "\n";
                    System.IO.File.Delete(eldoriaPath + "\\" + file.path);
                    
                }
            }
            MessageBox.Show(count.ToString() + " Mods Borrados \n \n" + oldMods);

            

            System.IO.File.Delete(eldoriaPath + "\\modrinth.index.json");
            System.IO.File.Move(tempUpdatePath + "\\modrinth.index.json", eldoriaPath + "\\modrinth.index.json");

            Directory.Delete(tempUpdatePath, true);

            label3.Text = "Todo Hecho!";

            //Enable install UI
            label4.Visible = false;
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
            Properties.Settings.Default["ModpackVer"] = eldoriaIndex.versionId;
            Properties.Settings.Default.Save();

            //Filter the path names to just file names
            //for (int i = 0; i < newFilesList.Count; i++)
            //{
            //    newFilesList[i].path = newFilesList[i].path.Split("/")[1];
            //}


            //Add all mods to the list
            for (int i = 0; i < newFilesList.Count; i++)
            {

                installModList.Add(newFilesList[i].path, new Tuple<string, string>(newFilesList[i].path, newFilesList[i].downloads[0]));
                checkedListBox1.Items.Add(newFilesList[i].path, true);

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
                    webClient.DownloadFileAsync(new Uri(downloadUrl), eldoriaPath + "\\" + filename);
                }
                else
                {
                    await webClient.DownloadFileTaskAsync(new Uri(downloadUrl), eldoriaPath + "\\" + filename);
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
    }

}

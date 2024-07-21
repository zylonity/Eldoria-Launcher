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
using BrightIdeasSoftware;

namespace EldoriaLauncher
{
    public partial class LauncherMain : Form
    {
        //Initialises everything
        public LauncherMain()
        {
            InitializeComponent();
            checkFabricVer();
            PlayActive();
            InitializeObjectListView();
            PopulateComboBox();

            if (!Directory.Exists(translationDocsPath))
            {
                MessageBox.Show("La carpeta translation_docs no existe. Por favor, reinstala el launcher.");
                Application.Exit();
            }

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

        //OLD FORM 1 STUFF
        #region main menu code

        string offlineUsername = (string)Properties.Settings.Default["Username"];
        int ram = (int)Properties.Settings.Default["Ram"];
        string mcVer = "fabric-loader-" + (string)Properties.Settings.Default["FabricVer"] + "-" + (string)Properties.Settings.Default["MinecraftVer"];
        string mcPathStr = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";
        string eldoriaPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string translationDocsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\translation_docs";


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
            ModsPanel.Visible = true;
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
        #endregion

        //OLD SETTINGS FORM STUFF
        #region settings code
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
            Properties.Settings.Default.Save();
        }

        private void OfflineUsernameBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["Username"] = OfflineUsernameBox.Text;
            Properties.Settings.Default.Save();
            PlayActive();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["Console"] = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void SettingsBack_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            SettingsPanel.Visible = false;
            MainPanel.Visible = true;
        }

        #endregion

        //MODS FORM
        #region mods code
        public class ListItem
        {
            public string Category { get; set; }
            public string ItemName { get; set; }
            public string toolTip { get; set; }
            public bool Checked { get; set; }
        }

        ModrinthClient client;
        UserAgent userAgent;
        ModrinthClientConfig options;
        Project project;
        Modrinth.Models.Version version;
        ModIndex eldoriaIndex;
        Dictionary<string, Tuple<string, string>> installModList = new Dictionary<string, Tuple<string, string>>();

        void LoadCheckboxes()
        {

            string docPath = eldoriaPath + "\\" + "downloadedOptionalMods.txt";
            if (System.IO.File.Exists(docPath))
            {
                Dictionary<string, string> mod_files = new Dictionary<string, string>();
                var lines = System.IO.File.ReadAllLines(docPath);

                foreach (var line in lines)
                {
                    var splitLine = line.Split(',');
                    if (splitLine.Length == 2)
                    {
                        mod_files.Add(splitLine[0], splitLine[1]);
                    }
                }

                foreach (OLVListItem listViewItem in objectListView1.Items)
                {
                    if (listViewItem.RowObject is ListItem listItem)
                    {
                        if (mod_files.Keys.Contains(listItem.ItemName))
                        {
                            listItem.Checked = true;
                        }
                    }
                }

                objectListView1.RefreshObjects(objectListView1.Items);
            }
        }



        private void PopulateComboBox()
        {
            string mainFolderPath = Path.Combine(eldoriaPath, "optional_packs");
            string customFolderPath = Path.Combine(mainFolderPath, "custom");

            if (!Directory.Exists(mainFolderPath))
            {
                MessageBox.Show("optional_packs no existe.");
                return;
            }

            string[] mainFileNames = Directory.GetFiles(mainFolderPath)
                                         .Select(file => Path.GetFileNameWithoutExtension(file))
                                         .ToArray();

            string[] customFileNames = Array.Empty<string>();
            if (Directory.Exists(customFolderPath))
            {
                customFileNames = Directory.GetFiles(customFolderPath)
                                      .Select(file => Path.GetFileNameWithoutExtension(file))
                                      .ToArray();
            }

            string[] allFileNames = mainFileNames.Concat(customFileNames).ToArray();

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(allFileNames);
        }

        async Task InstallMods()
        {
            button1.Enabled = false;
            pictureBox6.Enabled = false;
            pictureBox6.Visible = false;

            string docPath = eldoriaPath + "\\" + "downloadedOptionalMods.txt";
            string ogConfigPath = translationDocsPath + "\\" + "optionals.txt";
            string engOptionalsPath = translationDocsPath + "\\" + "ENG_Optionals.txt";

            var ogOptions = System.IO.File.ReadAllLines(ogConfigPath);
            var engOptionals = System.IO.File.ReadAllLines(engOptionalsPath);

            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += (s, e) =>
            {
                progressBar1.Value = e.ProgressPercentage;
            };
            webClient.DownloadFileCompleted += (s, e) =>
            {

            };

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


            if (System.IO.File.Exists(docPath))
            {
                Dictionary<string, string> mod_file = new Dictionary<string, string>();

                var lines = System.IO.File.ReadAllLines(docPath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');

                    if (parts.Length == 2)
                    {
                        mod_file.Add(parts[0], parts[1]);
                    }

                }

                string test = "";
                for (int i = 0; i < objectListView1.CheckedItems.Count; i++)
                {
                    //New installs
                    if (!mod_file.Keys.Contains(objectListView1.CheckedItems[i].Text))
                    {
                        int itemInList = 0;
                        for (int x = 0; x < engOptionals.Length; x++)
                        {
                            var parts = engOptionals[x].Split(',');

                            if (parts.Length == 2 && parts[1] == objectListView1.CheckedItems[i].Text)
                            {
                                itemInList = x;
                                break;
                            }
                        }
                        var project = await client.Project.GetAsync(ogOptions[itemInList].Split(',')[1]);
                        Modrinth.Models.Version version = null;
                        if (project.Versions.Length > 1)
                        {
                            for (int j = project.Versions.Length - 1; j > 0; j--)
                            {
                                version = await client.Version.GetAsync(project.Versions[j]);
                                if (version.Loaders.Contains("fabric") && version.GameVersions.Contains((string)Properties.Settings.Default["MinecraftVer"]))
                                {
                                    break;
                                }

                            }
                        }
                        else
                        {
                            version = await client.Version.GetAsync(project.Versions[project.Versions.Length - 1]);
                        }

                        test += objectListView1.CheckedItems[i].Text + ',' + version.Files[0].FileName + '\n';
                        await webClient.DownloadFileTaskAsync(new Uri(version.Files[0].Url), modsPath + "\\" + version.Files[0].FileName);
                    }
                    else
                    {
                        test += objectListView1.CheckedItems[i].Text + ',' + mod_file[objectListView1.CheckedItems[i].Text] + '\n';
                    }


                }

                string deleting = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    //To delete
                    if (!test.Contains(lines[i]))
                    {
                        var modParts = lines[i].Split(',');
                        System.IO.File.Delete(modsPath + "\\" + modParts[1]);
                        deleting += lines[i];
                    }

                }
                using (StreamWriter downloadedMods = new StreamWriter(docPath, false))
                {
                    downloadedMods.WriteLine(test);
                }
                MessageBox.Show("Actualizacion completa!");
            }
            else
            {

                string test = "";
                for (int i = 0; i < objectListView1.CheckedItems.Count; i++)
                {
                    int itemInList = 0;
                    for (int x = 0; x < engOptionals.Length; x++)
                    {
                        var parts = engOptionals[x].Split(',');

                        if (parts.Length == 2 && parts[1] == objectListView1.CheckedItems[i].Text)
                        {
                            itemInList = x;
                            break;
                        }
                    }
                    var project = await client.Project.GetAsync(ogOptions[itemInList].Split(',')[1]);
                    Modrinth.Models.Version version = null;
                    if (project.Versions.Length > 1)
                    {
                        for (int j = project.Versions.Length - 1; j > 0; j--)
                        {
                            version = await client.Version.GetAsync(project.Versions[j]);
                            if (version.Loaders.Contains("fabric") && version.GameVersions.Contains((string)Properties.Settings.Default["MinecraftVer"]))
                            {
                                break;
                            }

                        }
                    }
                    else
                    {
                        version = await client.Version.GetAsync(project.Versions[project.Versions.Length - 1]);
                    }



                    test += objectListView1.CheckedItems[i].Text + ',' + version.Files[0].FileName + '\n';
                    await webClient.DownloadFileTaskAsync(new Uri(version.Files[0].Url), modsPath + "\\" + version.Files[0].FileName);

                }

                using (StreamWriter downloadedMods = new StreamWriter(docPath, false))
                {
                    downloadedMods.WriteLine(test);
                }
            }

            button1.Enabled = true;
            pictureBox6.Enabled = true;
            pictureBox6.Visible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            InstallMods();

        }

        private async void InitializeObjectListView()
        {

            // Clear existing columns and groups
            objectListView1.Clear();
            objectListView1.Groups.Clear();

            objectListView1.CheckBoxes = true;

            // Create columns
            OLVColumn itemColumn = new OLVColumn("Item", "ItemName");
            itemColumn.Width = 185; // Adjust width if necessary
            itemColumn.IsVisible = false; // Make the column header invisible
            objectListView1.Columns.Add(itemColumn);

            // Set the data source
            var items = await LoadItemsFromTwoFiles(translationDocsPath + "\\" + "ENG_Optionals.txt", translationDocsPath + "\\" + "ES_Descriptions.txt");
            objectListView1.SetObjects(items);

            // CheckBox handling
            objectListView1.CellEditActivation = ObjectListView.CellEditActivateMode.SingleClick;
            objectListView1.CheckStateGetter = delegate (object rowObject)
            {
                return ((ListItem)rowObject).Checked ? CheckState.Checked : CheckState.Unchecked;
            };

            objectListView1.CheckStatePutter = delegate (object rowObject, CheckState value)
            {
                ((ListItem)rowObject).Checked = (value == CheckState.Checked);
                return value;
            };

            // Group by category
            itemColumn.GroupKeyGetter = delegate (object rowObject)
            {
                return ((ListItem)rowObject).Category;
            };

            // Hide the column headers
            objectListView1.HeaderStyle = ColumnHeaderStyle.None;

            objectListView1.CellToolTipGetter = delegate (OLVColumn column, object rowObject)
            {
                if (rowObject is ListItem item)
                {
                    return ((ListItem)rowObject).toolTip;
                }
                return null;
            };

            LoadCheckboxes();

            objectListView1.BuildList();


        }


        private async Task<List<ListItem>> LoadItemsFromTwoFiles(string filePath, string descPath)
        {

            var items = new List<ListItem>();

            if (System.IO.File.Exists(filePath))
            {
                var lines = System.IO.File.ReadAllLines(filePath);
                var descs = System.IO.File.ReadAllLines(descPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(',');
                    //Get project from url name

                    if (parts.Length == 2)
                    {
                        var item = new ListItem
                        {
                            Category = parts[0].Trim(),
                            ItemName = parts[1].Trim(),
                            toolTip = descs[i],
                            Checked = false
                        };
                        items.Add(item);
                    }
                }
            }
            else
            {
                MessageBox.Show($"File not found: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return items;
        }

        private async Task<List<ListItem>> LoadItemsFromFile(string filePath)
        {

            var items = new List<ListItem>();

            //Connect to modrinth API
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


            if (System.IO.File.Exists(filePath))
            {
                var lines = System.IO.File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    //Get project from url name

                    if (parts.Length == 2)
                    {
                        var project = await client.Project.GetAsync(parts[1].Trim());
                        var item = new ListItem
                        {
                            Category = parts[0].Trim(),
                            ItemName = project.Title,
                            toolTip = project.Description,
                            Checked = false
                        };
                        items.Add(item);
                    }
                }
            }
            else
            {
                MessageBox.Show($"File not found: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return items;
        }

        private async void Mods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10 && e.Shift)
            {
                await HandleShiftF10Async();
            }
        }

        private async Task HandleShiftF10Async()
        {
            string optionalsFilePath = Path.Combine(translationDocsPath, "optionals.txt");
            string esDescriptionsFilePath = Path.Combine(translationDocsPath, "ES_Descriptions.txt");
            string engOptionalsFilePath = Path.Combine(translationDocsPath, "ENG_Optionals.txt");
            string engDescriptionsFilePath = Path.Combine(translationDocsPath, "ENG_Descriptions.txt");

            var items = await LoadItemsFromFile(optionalsFilePath);

            // Create ENG_Optionals and ENG_Descriptions files
            using (StreamWriter engOptionalsWriter = new StreamWriter(engOptionalsFilePath, false))
            using (StreamWriter engDescriptionsWriter = new StreamWriter(engDescriptionsFilePath, false))
            {
                foreach (var item in items)
                {
                    // Write to ENG_Optionals
                    engOptionalsWriter.WriteLine($"{item.Category},{item.ItemName}");

                    // Write to ENG_Descriptions
                    string singleLineDescription = item.toolTip.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    engDescriptionsWriter.WriteLine(singleLineDescription);
                }
            }

            MessageBox.Show("Los archivos ENG_Optionals y ENG_Descriptions se crearon correctamente. Si no tienes ni puta idea que significa esto, ignoralo.");
        }


        private void UpdateObjectListViewCheckedState(string[] itemNames)
        {
            objectListView1.BeginUpdate();

            for (int x = 0; x < objectListView1.Items.Count; x++)
            {
                if (itemNames.Contains(objectListView1.Items[x].Text))
                {
                    objectListView1.GetItem(x).Checked = true;
                }
                else
                {
                    objectListView1.GetItem(x).Checked = false;
                }
            }

            objectListView1.EndUpdate();
            objectListView1.RefreshObjects(objectListView1.Items);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = comboBox1.SelectedItem.ToString();
            string mainFolderPath = Path.Combine(eldoriaPath, "optional_packs");
            string customFolderPath = Path.Combine(mainFolderPath, "custom");

            string mainFilePath = Path.Combine(mainFolderPath, selectedFile + ".txt");
            string customFilePath = Path.Combine(customFolderPath, selectedFile + ".txt");

            string filePath = null;

            if (System.IO.File.Exists(mainFilePath))
            {
                filePath = mainFilePath;
            }
            else if (System.IO.File.Exists(customFilePath))
            {
                filePath = customFilePath;
            }

            if (filePath == null)
            {
                MessageBox.Show("Archivo no encontrado: " + selectedFile + ".txt");
                return;
            }

            string[] fileContent = System.IO.File.ReadAllLines(filePath);

            UpdateObjectListViewCheckedState(fileContent);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string customPath = eldoriaPath + "\\optional_packs\\custom";

            if (!Directory.Exists(customPath))
            {
                Directory.CreateDirectory(customPath);
            }

            string[] existingFiles = Directory.GetFiles(customPath, "Preset *.txt");

            int maxNumber = 0;

            foreach (string file in existingFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string numberPart = fileName.Replace("Preset ", "");
                if (int.TryParse(numberPart, out int number))
                {
                    if (number > maxNumber)
                    {
                        maxNumber = number;
                    }
                }
            }

            int nextNumber = maxNumber + 1;
            string newFileName = $"Preset {nextNumber}.txt";
            string newFilePath = Path.Combine(customPath, newFileName);

            System.IO.File.Create(newFilePath).Dispose();

            PopulateComboBox();

            comboBox1.SelectedItem = Path.GetFileNameWithoutExtension(newFileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selectedPreset = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedPreset))
            {
                MessageBox.Show("No hay ningún preset seleccionado.");
                return;
            }

            string mainFolderPath = Path.Combine(eldoriaPath, "optional_packs");
            string customFolderPath = Path.Combine(mainFolderPath, "custom");

            string mainFilePath = Path.Combine(mainFolderPath, selectedPreset + ".txt");
            string customFilePath = Path.Combine(customFolderPath, selectedPreset + ".txt");

            string filePath = null;

            if (System.IO.File.Exists(mainFilePath))
            {
                filePath = mainFilePath;
            }
            else if (System.IO.File.Exists(customFilePath))
            {
                filePath = customFilePath;
            }

            if (filePath == null || !filePath.StartsWith(customFolderPath))
            {
                MessageBox.Show("No se puede guardar en el preset seleccionado. Solo se pueden guardar presets personalizados.");
                return;
            }

            string presetToSave = "";
            //List<string> checkedItemNames = new List<string>();
            for (int i = 0; i < objectListView1.Items.Count; i++)
            {
                if (objectListView1.GetItem(i).CheckState == CheckState.Checked)
                {
                    var listItem = (ListItem)objectListView1.GetModelObject(i);
                    presetToSave += listItem.ItemName + "\n";
                }
            }

            try
            {
                System.IO.File.WriteAllText(filePath, presetToSave);
                MessageBox.Show($"Preset guardado: {selectedPreset}.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el preset: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string selectedPreset = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedPreset))
            {
                MessageBox.Show("No hay ningún preset seleccionado.");
                return;
            }

            string mainFolderPath = Path.Combine(eldoriaPath, "optional_packs");
            string customFolderPath = Path.Combine(mainFolderPath, "custom");

            string customFilePath = Path.Combine(customFolderPath, selectedPreset + ".txt");

            if (System.IO.File.Exists(customFilePath))
            {
                try
                {
                    System.IO.File.Delete(customFilePath);
                    MessageBox.Show($"Preset eliminado: {selectedPreset}.txt");

                    // Refresh the ComboBox to reflect the deletion
                    PopulateComboBox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el preset: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Solo se pueden eliminar presets personalizados.");
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            MainPanel.Visible = true;
            ModsPanel.Visible = false;
        }
        #endregion


    }

}


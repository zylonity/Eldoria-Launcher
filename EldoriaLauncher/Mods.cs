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
using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices.Marshalling;
using static System.Windows.Forms.LinkLabel;
using CmlLib.Core.Files;
using System.IO;

namespace EldoriaLauncher
{


    public partial class Mods : Form
    {

        Form1 mainForm = Application.OpenForms.OfType<Form1>().Single();

        string eldoriaPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";
        string translationDocsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\translation_docs";

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
            InitializeObjectListView();
            PopulateComboBox();

            this.KeyPreview = true;

            if (!Directory.Exists(translationDocsPath))
            {
                MessageBox.Show("La carpeta translation_docs no existe. Por favor, reinstala el launcher.");
                Application.Exit();
            }
            //GetMods();
        }



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
            // Define the path to the main folder and the custom subfolder
            string mainFolderPath = Path.Combine(eldoriaPath, "optional_packs");
            string customFolderPath = Path.Combine(mainFolderPath, "custom");

            // Check if the main folder exists
            if (!Directory.Exists(mainFolderPath))
            {
                MessageBox.Show("The optional_packs folder does not exist.");
                return;
            }

            // Retrieve the names of the files in the main folder
            string[] mainFileNames = Directory.GetFiles(mainFolderPath)
                                         .Select(file => Path.GetFileNameWithoutExtension(file))
                                         .ToArray();

            // Retrieve the names of the files in the custom folder, if it exists
            string[] customFileNames = Array.Empty<string>();
            if (Directory.Exists(customFolderPath))
            {
                customFileNames = Directory.GetFiles(customFolderPath)
                                      .Select(file => Path.GetFileNameWithoutExtension(file))
                                      .ToArray();
            }

            // Combine the file names from both folders
            string[] allFileNames = mainFileNames.Concat(customFileNames).ToArray();

            // Populate the ComboBox with the combined file names
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(allFileNames);
        }

        async Task InstallMods()
        {
            button1.Enabled = false;
            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;

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
            pictureBox1.Enabled = true;
            pictureBox1.Visible = true;

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

        private void button1_Click(object sender, EventArgs e)
        {

            InstallMods();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Location = this.Location;
            mainForm.Show();
            mainForm.PlayActive();
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
    }

    public class ListItem
    {
        public string Category { get; set; }
        public string ItemName { get; set; }
        public string toolTip { get; set; }
        public bool Checked { get; set; }
    }

}

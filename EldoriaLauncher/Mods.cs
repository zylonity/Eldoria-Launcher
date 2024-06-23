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

namespace EldoriaLauncher
{


    public partial class Mods : Form
    {

        Form1 mainForm = Application.OpenForms.OfType<Form1>().Single();

        string eldoriaPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";
        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";
        string configPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\config";

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
            LoadCheckboxes();
            InitializeObjectListView();
            this.KeyPreview = true;
            //GetMods();
        }

        async Task GetMods()
        {

            //listView1.CheckBoxes = true;
            button1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            //userAgent = new UserAgent
            //{
            //    ProjectName = "Eldoria-Launcher",
            //    ProjectVersion = "1.2.0",
            //    GitHubUsername = "zylonity",
            //};

            //options = new ModrinthClientConfig
            //{
            //    UserAgent = userAgent.ToString()
            //};

            //client = new ModrinthClient(options);

            //string[] files = Directory.GetFiles(modsPath);
            //var fileNames = new List<string>();
            //foreach (string file in files)
            //{
            //    fileNames.Add(Path.GetFileName(file));
            //}

            //var modNames = new List<string>();

            //foreach (string name in fileNames)
            //{
            //    var search = await client.Project.SearchAsync(name);
            //    if (search.Hits.Count() > 0)
            //    {
            //        modNames.Add(search.Hits[0].Title);
            //        ListViewItem item1WithToolTip = new ListViewItem(search.Hits[0].Title);
            //        item1WithToolTip.ToolTipText = search.Hits[0].Description;
            //        listView1.Items.Add(item1WithToolTip);

            //    }
            //    else
            //    {
            //        modNames.Add(name);
            //        listView1.Items.Add(name);
            //    }

            //}




            //Enable install UI
            //checkedListBox1.Visible = true;
            progressBar2.Visible = true;
            currentDownload.Visible = true;
            button1.Visible = true;
            checkBox1.Visible = true;

            button1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        void LoadCheckboxes()
        {

            string docPath = eldoriaPath + "\\" + "downloadedOptionalMods.txt";
            if (System.IO.File.Exists(docPath))
            {
                var lines = System.IO.File.ReadAllLines(docPath);
                //var checkedMods = new HashSet<string>(lines.Select(line => line.Trim()));
                

                foreach (OLVListItem listViewItem in objectListView1.Items)
                {
                    if (listViewItem.RowObject is ListItem listItem)
                    {
                        if (lines.Contains(listItem.ItemName))
                        {
                            listItem.Checked = true;
                        }
                    }
                }

                //objectListView1.RefreshObjects(objectListView1.Objects.Cast<object>().ToList());
               objectListView1.Refresh();
            }
        }


        async Task InstallMods()
        {

            string docPath = eldoriaPath + "\\" + "downloadedOptionalMods.txt";

            string test = "";
            for (int i = 0; i < objectListView1.CheckedItems.Count; i++)
            {
                test += objectListView1.CheckedItems[i].Text + '\n';
            }
            MessageBox.Show(test);


            if (System.IO.File.Exists(docPath))
            {
                using (StreamWriter engOptionalsWriter = new StreamWriter(docPath, false))
                {

                }
            }
            else
            {
                using (StreamWriter downloadedMods = new StreamWriter(docPath, false))
                {
                    downloadedMods.WriteLine(test);
                }
            }

            //Cursor.Current = Cursors.WaitCursor;
            //System.IO.Directory.CreateDirectory(modsPath);

                //float itemsToDownload = checkedListBox1.CheckedItems.Count - 1;
                //float itemsDownloaded = 0;

                //downloadAsync = checkBox1.Checked;

                ////Download all the mods
                //for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                //{

                //    string filename = installModList[checkedListBox1.CheckedItems[i].ToString()].Item1;
                //    string downloadUrl = installModList[checkedListBox1.CheckedItems[i].ToString()].Item2;


                //    int x = i;
                //    if (i + 1 < checkedListBox1.CheckedItems.Count)
                //        x = i + 1;

                //    string nextFileName = checkedListBox1.CheckedItems[x].ToString();

                //    WebClient webClient = new WebClient();
                //    webClient.DownloadProgressChanged += (s, e) =>
                //    {
                //        progressBar2.Value = e.ProgressPercentage;
                //        progressBar1.Value = (int)((itemsDownloaded / itemsToDownload) * 100);
                //    };
                //    webClient.DownloadFileCompleted += (s, e) =>
                //    {
                //        itemsDownloaded++;
                //        currentDownload.Text = nextFileName;

                //        if (downloadAsync && (int)itemsDownloaded == (itemsToDownload + 1))
                //        {
                //            Application.Restart();
                //        }
                //    };


                //    if (downloadAsync)
                //    {
                //        webClient.DownloadFileAsync(new Uri(downloadUrl), modsPath + "\\" + filename);
                //    }
                //    else
                //    {
                //        await webClient.DownloadFileTaskAsync(new Uri(downloadUrl), modsPath + "\\" + filename);
                //    }





                //}

                //if (!downloadAsync)
                //{
                //    Application.Restart();
                //}

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
            //button1.Enabled = false;
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

            // Create columns
            OLVColumn itemColumn = new OLVColumn("Item", "ItemName");
            itemColumn.Width = 185; // Adjust width if necessary
            itemColumn.IsVisible = false; // Make the column header invisible
            objectListView1.Columns.Add(itemColumn);

            // Set the data source
            var items = await LoadItemsFromTwoFiles(configPath + "\\" + "ENG_Optionals.txt", configPath + "\\" + "ES_Descriptions.txt");
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

            objectListView1.BuildList();


        }


        private async Task<List<ListItem>> LoadItemsFromTwoFiles(string filePath, string descPath)
        {

            var items = new List<ListItem>();

            if (System.IO.File.Exists(filePath))
            {
                var lines = System.IO.File.ReadAllLines(filePath);
                var descs = System.IO.File.ReadAllLines(descPath);
                for(int i = 0; i < lines.Length; i++)
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
                ProjectVersion = "1.2.0",
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
            string optionalsFilePath = Path.Combine(configPath, "optionals.txt");
            string esDescriptionsFilePath = Path.Combine(configPath, "ES_Descriptions.txt");
            string engOptionalsFilePath = Path.Combine(configPath, "ENG_Optionals.txt");
            string engDescriptionsFilePath = Path.Combine(configPath, "ENG_Descriptions.txt");

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
    }

    public class ListItem
    {
        public string Category { get; set; }
        public string ItemName { get; set; }
        public string toolTip { get; set; }
        public bool Checked { get; set; }
    }

}

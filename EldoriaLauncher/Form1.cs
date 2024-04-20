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
            try
            {
                if (Directory.Exists(verPath))
                {
                    if (validateFiles)
                    {
                        using (var repo = new Repository(verPath))
                        {
                            // Fetch the remote branch and its tree
                            var remoteBranch = repo.Branches["main"];
                            var remoteCommit = repo.Lookup<Commit>(remoteBranch.Tip.Sha);
                            var remoteTree = remoteCommit.Tree;

                            // Get the directories and files in the local directory and its subdirectories
                            var localDirectories = Directory.GetDirectories(verPath, "*", SearchOption.AllDirectories)
                                .Select(path => GetRelativePath(verPath, path))
                                .ToList();
                            var localFiles = Directory.GetFiles(verPath, "*", SearchOption.AllDirectories)
                                .Select(path => GetRelativePath(verPath, path))
                                .ToList();

                            // Traverse the remote tree recursively, checking for missing directories and files
                            var missingDirectories = new List<string>();
                            var missingFiles = new List<string>();
                            TraverseTree(remoteTree, verPath, localDirectories, localFiles, ref missingDirectories, ref missingFiles);

                            // Print out the missing directories and files
                            foreach (var directory in missingDirectories)
                            {
                                Console.WriteLine($"Missing directory: {directory}");
                            }
                            foreach (var file in missingFiles)
                            {
                                Console.WriteLine($"Missing file: {file}");
                            }
                        }
                    }


                    string logMessage = "";
                    using (var repo = new Repository(verPath))
                    {
                        var remote = repo.Network.Remotes["origin"];
                        var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                        Commands.Fetch(repo, remote.Name, refSpecs, null, logMessage);

                        var updates = repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, repo.Branches[repo.Head.FriendlyName].TrackedBranch.Tip.Tree);

                        if (updates.Count > 0)
                        {
                            updating = true;

                            Downloading updateWindow = new Downloading();
                            updateWindow.Text = "Actualizando";
                            updateWindow.DownloadBigLabel.Text = "Actualizando...";
                            updateWindow.progressBar1.Hide();
                            updateWindow.Show();
                            updateWindow.Update();
                            updateWindow.TopMost = true;

                            foreach (var update in updates)
                            {
                                if (update.Status == ChangeKind.Added)
                                {
                                    if (System.IO.File.Exists(verPath + "\\" + update.Path))
                                    {
                                        System.IO.File.Delete(verPath + "\\" + update.Path);
                                    }

                                }
                            }


                            MergeResult mergeResult;
                            Signature merger = new Signature("Algun Player", "you@example.com", DateTimeOffset.Now);
                            try
                            {
                                mergeResult = repo.Merge(repo.Branches[repo.Head.FriendlyName].TrackedBranch, merger);


                                if (mergeResult.Status == MergeStatus.Conflicts)
                                {
                                    foreach (var conflict in repo.Index.Conflicts)
                                    {
                                        Commands.Checkout(repo, conflict.Theirs.Path, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force });
                                    }

                                    Commit mergeCommit = repo.Commit("Merge commit", merger, merger);
                                }

                                if (mergeResult.Status != MergeStatus.UpToDate)
                                {
                                    // Post UI updates back to the UI thread
                                    updateWindow.BeginInvoke((Action)(() =>
                                    {
                                        updateWindow.Close();
                                        updateWindow.Hide();

                                    }));

                                }


                            }
                            catch (LibGit2SharpException ex)
                            {
                                MessageBox.Show(ex.Message);
                                repo.Reset(ResetMode.Hard);
                                mergeResult = repo.Merge(repo.Branches[repo.Head.FriendlyName].TrackedBranch, merger);

                                if (mergeResult.Status == MergeStatus.Conflicts)
                                {
                                    foreach (var conflict in repo.Index.Conflicts)
                                    {
                                        Commands.Checkout(repo, conflict.Theirs.Path, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force });
                                    }

                                    Commit mergeCommit = repo.Commit("Merge commit", merger, merger);
                                }

                                if (mergeResult.Status != MergeStatus.UpToDate)
                                {
                                    // Post UI updates back to the UI thread
                                    updateWindow.BeginInvoke((Action)(() =>
                                    {
                                        updateWindow.Close();
                                        updateWindow.Hide();


                                    }));

                                }


                            }

                            if (mergeResult.Status == MergeStatus.Conflicts)
                            {
                                foreach (var conflict in repo.Index.Conflicts)
                                {
                                    Commands.Checkout(repo, conflict.Theirs.Path, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force });
                                }

                                Commit mergeCommit = repo.Commit("Merge commit", merger, merger);
                            }

                            updating = false;
                            updateWindow.BeginInvoke((Action)(() =>
                            {
                                PlayActive();
                                updateWindow.Hide();
                                updateWindow.Close();

                            }));
                        }

                    }
                    Console.WriteLine(logMessage);

                }
                else
                {
                    updating = true;
                    downloadWindow = new Downloading();
                    downloadWindow.Show();
                    downloadWindow.Update();
                    await InitialClone();
                    downloadWindow.Close();
                    updating = false;
                    PlayActive();

                }
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);

            }

        }

        //Clones everything
        async Task InitialClone()
        {
            downloadWindow.TopMost = true;
            float percentDone;
            float previousPercentDone = 0;

            var cOptions = new CloneOptions
            {
                //Parameters to check for progress
                //OnTransferProgress = progress =>
                //{
                //    percentDone = ((float)progress.IndexedObjects / (float)progress.TotalObjects * 100.0f);
                //    if ((int)percentDone > previousPercentDone)
                //    {
                //        previousPercentDone = (int)percentDone;

                //        // Update progress bar on UI thread
                //        downloadWindow.Invoke((Action)(() =>
                //        {
                //            downloadWindow.progressBar1.Value = (int)percentDone;
                //            downloadWindow.progressBar1.Update();
                //        }));
                //    }

                //    return true;
                //},
            };

            // Run clone in separate task
            await Task.Run(() => Repository.Clone(sRepo, verPath, cOptions));
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
            //settings.Activate();
            //settings.Show();
            this.Hide();
            //settings.Update();
        }

    }
}

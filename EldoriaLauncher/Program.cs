using EldoriaLauncher.MrPack;
using Modrinth;
using Modrinth.Exceptions;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EldoriaLauncher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void GetLauncherVersion()
        {
            string firstLine = "";
            string versionContent = Properties.Resources.AppVersion; 
            using (StringReader reader = new StringReader(versionContent))
            {
                firstLine = reader.ReadLine();
            }

            Properties.Settings.Default["AppVer"] = firstLine;
            Properties.Settings.Default.Save();
            
        }

        static private async Task<string> GetLatestVersionAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://raw.githubusercontent.com/zylonity/Eldoria-Launcher/master/EldoriaLauncher/Resources/AppVersion.txt";
                return await client.GetStringAsync(url);
            }
        }

        static private async Task UpdateApplicationAsync()
        {
            string updaterUrl = "https://github.com/zylonity/Eldoria-Launcher/raw/master/Updater.exe";
            string tempUpdaterPath = Path.Combine(Path.GetTempPath(), "Updater.exe");

            using (HttpClient client = new HttpClient())
            {
                byte[] data = await client.GetByteArrayAsync(updaterUrl);
                await System.IO.File.WriteAllBytesAsync(tempUpdaterPath, data);
            }

            string currentExePath = Application.ExecutablePath;

            var processInfo = new ProcessStartInfo(tempUpdaterPath, "\"" + currentExePath + "\"")
            {
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(processInfo);
                // Ensure the application exits
                Application.Exit();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("El proceso de actualización requiere privilegios de administrador. Por favor, inténtelo de nuevo. " + ex.Message);
            }
        }

        static private async Task CheckForUpdatesAsync()
        {
            string currentVersion = (string)Properties.Settings.Default["AppVer"];
            string latestVersion = await GetLatestVersionAsync();

            if (currentVersion != latestVersion)
            {
                DialogResult dialogResult = MessageBox.Show("Una nueva versión está disponible. " + latestVersion + " ¿Desea actualizar?", "Actualización disponible", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    await UpdateApplicationAsync();
                }
            }
            else
            {
                MessageBox.Show("Tienes la última versión. " + currentVersion);
            }
        }

        static async Task<string> GetModpackVersion()
        {
            var userAgent = new UserAgent
            {
                ProjectName = "Eldoria-Launcher",
                ProjectVersion = (string)Properties.Settings.Default["AppVer"],
                GitHubUsername = "zylonity",
            };

            var options = new ModrinthClientConfig
            {
                UserAgent = userAgent.ToString()
            };

            var client = new ModrinthClient(options);

            try
            {
                var project = await client.Project.GetAsync("Eldoria");
                var version = await client.Version.GetAsync(project.Versions[project.Versions.Length - 1]);
                return version.VersionNumber;
            }
            catch (ModrinthApiException)
            {
                MessageBox.Show("No se encuentra el proyecto de Eldoria en Modrinth. ¿Tienes internet?");
                return "1.0.0";
            }
        }

        public static int CompareVersions(string v1, string v2)
        {
            var v1Components = v1.Split('.').Select(int.Parse).ToArray();
            var v2Components = v2.Split('.').Select(int.Parse).ToArray();

            int maxLength = Math.Max(v1Components.Length, v2Components.Length);

            for (int i = 0; i < maxLength; i++)
            {
                int v1Component = i < v1Components.Length ? v1Components[i] : 0;
                int v2Component = i < v2Components.Length ? v2Components[i] : 0;

                if (v1Component < v2Component) return -1;
                if (v1Component > v2Component) return 1;
            }

            return 0;
        }

        static void Main()
        {
            string mcPathStr = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";

            GetLauncherVersion();

            ApplicationConfiguration.Initialize();

            if (Directory.Exists(mcPathStr))
            {
                ModIndex eldoriaIndex = JsonSerializer.Deserialize<ModIndex>(System.IO.File.ReadAllText(mcPathStr + "\\modrinth.index.json"));

                string currentVer = eldoriaIndex.versionId;
                string onlineVer = GetModpackVersion().Result;

                
                CheckForUpdatesAsync().Wait();

                int result = CompareVersions(currentVer, onlineVer);

                if (result < 0)
                    Application.Run(new Updater());
                else
                    Application.Run(new Form1());
            }
            else
            {
                Application.Run(new Installer());
            }
        }
    }
}

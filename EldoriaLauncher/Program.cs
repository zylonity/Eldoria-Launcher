using EldoriaLauncher.MrPack;
using Modrinth;
using Modrinth.Exceptions;
using System.Text.Json;
using static System.Windows.Forms.Design.AxImporter;

namespace EldoriaLauncher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]

        static async Task<string> GetVersion()
        {
            var userAgent = new UserAgent
            {
                ProjectName = "Eldoria-Launcher",
                ProjectVersion = "1.2.0",
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
            // Or you can catch the exception and handle all non-200 status codes
            catch (ModrinthApiException e)
            {
                MessageBox.Show("No se encuentra el proyecto de Eldoria en Modrinth.");
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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            string mcPathStr = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria";

            ApplicationConfiguration.Initialize();
            if (Directory.Exists(mcPathStr))
            {
                ModIndex eldoriaIndex = JsonSerializer.Deserialize<ModIndex>(System.IO.File.ReadAllText(mcPathStr + "\\modrinth.index.json"));

                string currentVer = eldoriaIndex.versionId;
                string onlineVer = GetVersion().Result;

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
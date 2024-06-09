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

                var result = currentVer.CompareTo(onlineVer);
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
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EldoriaUpdater
{
    class Program
    {
        private const string LatestReleaseApiUrl = "https://api.github.com/repos/zylonity/Eldoria-Launcher/releases/latest";

        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                MessageBox.Show("Argumentos inválidos.");
                return;
            }

            ApplicationConfiguration.Initialize();
            string oldExePath = args[0];

            try
            {
                string newExeUrl = await GetLatestReleaseUrlAsync();
                if (string.IsNullOrEmpty(newExeUrl))
                {
                    MessageBox.Show("No se pudo obtener la URL de la última versión.");
                    return;
                }

                string newExePath = Path.Combine(Path.GetDirectoryName(oldExePath), "EldoriaLauncher_New.exe");

                using (HttpClient client = new HttpClient())
                {
                    byte[] data = await client.GetByteArrayAsync(newExeUrl);
                    await File.WriteAllBytesAsync(newExePath, data);
                }

                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(oldExePath));
                foreach (Process process in processes)
                {
                    process.WaitForExit();
                }

                File.Delete(oldExePath);
                File.Move(newExePath, oldExePath);

                Process.Start(oldExePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("La actualización falló: " + ex.Message);
            }
        }

        private static async Task<string> GetLatestReleaseUrlAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; EldoriaUpdater/1.0)");

                try
                {
                    var response = await client.GetStringAsync(LatestReleaseApiUrl);
                    using (JsonDocument doc = JsonDocument.Parse(response))
                    {
                        var root = doc.RootElement;
                        var assets = root.GetProperty("assets");

                        foreach (var asset in assets.EnumerateArray())
                        {
                            if (asset.GetProperty("name").GetString() == "EldoriaLauncher.exe")
                            {
                                return asset.GetProperty("browser_download_url").GetString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener la URL de la última versión: " + ex.Message);
                }
            }

            return null;
        }
    }
}

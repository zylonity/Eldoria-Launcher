using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EldoriaUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                MessageBox.Show("Argumentos inválidos.");
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());

            string oldExePath = args[0];
            string newExeUrl = "https://github.com/zylonity/Eldoria-Launcher/raw/master/EldoriaLauncher.exe";
            string newExePath = Path.Combine(Path.GetDirectoryName(oldExePath), "EldoriaLauncher_New.exe");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] data = await client.GetByteArrayAsync(newExeUrl);
                    await File.WriteAllBytesAsync(newExePath, data);
                }

                // Esperar a que el ejecutable antiguo termine
                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(oldExePath));
                foreach (Process process in processes)
                {
                    process.WaitForExit();
                }

                // Reemplazar el ejecutable antiguo por el nuevo
                File.Delete(oldExePath);
                File.Move(newExePath, oldExePath);

                // Opcionalmente, iniciar el nuevo ejecutable
                Process.Start(oldExePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("La actualización falló: " + ex.Message);
            }
        }
    }
}

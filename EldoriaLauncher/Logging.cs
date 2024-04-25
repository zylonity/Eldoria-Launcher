using CmlLib.Core.Files;
using Microsoft.VisualBasic.Devices;
using Modrinth;
using Modrinth.Endpoints.Project;
using Modrinth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.LinkLabel;

namespace EldoriaLauncher
{
    public partial class Logging : Form
    {
        
        public Logging()
        {
            InitializeComponent();
        }

        public async Task StartMinecraft(Process process)
        {
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += (s, e) => WriteLog();
            process.OutputDataReceived += (s, e) => WriteLog();

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            this.Update();

            
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        string logFile = "";
        public void WriteLog()
        {
            
            string path = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\logs\\latest.log";

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                string[] lines = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                //logFile = sr.ReadToEnd();
               
                if(lines.Length > 0 && logFile != lines[lines.Length - 1])
                {
                    if (LogBox.InvokeRequired)
                    {
                        LogBox.Invoke(new Action(WriteLog), new object[] { logFile });
                    }
                    else
                    {
                        if (lines.Length > 0)
                        {
                            LogBox.SelectionStart = LogBox.TextLength;
                            LogBox.SelectionLength = 0;

                            LogBox.SelectionColor = Color.Black;
                            LogBox.AppendText(lines[lines.Length - 1] + "\n");
                            LogBox.SelectionColor = LogBox.ForeColor;
                        }
                    }
                }

                if (lines.Length > 0)
                    logFile = lines[lines.Length - 1];

            }

        }

    }
}

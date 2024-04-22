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
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static System.Windows.Forms.Design.AxImporter;

namespace EldoriaLauncher
{
    public partial class Logging : Form
    {

        string modsPath = Environment.GetEnvironmentVariable("appdata") + "\\.Eldoria\\mods";

        bool downloadAsync = false;

        ModrinthClient client;
        ModrinthClientConfig options;
        Project project;
        Dependencies dp;

        Dictionary<string, Tuple<string, string>> installModList = new Dictionary<string, Tuple<string, string>>();

        
        public Logging()
        {
            InitializeComponent();
        }

        public async Task StartMinecraft(Process process)
        {
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += (s, e) => WriteError(e.Data);
            process.OutputDataReceived += (s, e) => WriteLog(e.Data);

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
            Application.Exit();
        }

        public void WriteLog(string text)
        {
            if (LogBox.InvokeRequired)
            {
                LogBox.Invoke(new Action<string>(WriteLog), new object[] { text });
            }
            else
            {
                LogBox.SelectionStart = LogBox.TextLength;
                LogBox.SelectionLength = 0;

                LogBox.SelectionColor = Color.Black;
                LogBox.AppendText(text + '\n');
                LogBox.SelectionColor = LogBox.ForeColor;
            }
        }

        public void WriteError(string text)
        {
            if (LogBox.InvokeRequired)
            {
                LogBox.Invoke(new Action<string>(WriteError), new object[] { text });
            }
            else
            {
                LogBox.SelectionStart = LogBox.TextLength;
                LogBox.SelectionLength = 0;

                LogBox.SelectionColor = Color.DarkRed;
                LogBox.AppendText(text + '\n');
                LogBox.SelectionColor = LogBox.ForeColor;
            }
            
        }
    }
}

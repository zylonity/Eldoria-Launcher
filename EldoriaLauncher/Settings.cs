using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EldoriaLauncher
{
    public partial class Settings : Form
    {
        Form1 mainForm = Application.OpenForms.OfType<Form1>().Single();
        public Settings()
        {
            InitializeComponent();
            OfflineUsernameBox.Text = (string)Properties.Settings.Default["Username"];
            RamBox.SelectedIndex = (int)Properties.Settings.Default["RamIndex"];
        }

        private void RamBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fid;
            bool parseOK = Int32.TryParse(RamBox.Items[RamBox.SelectedIndex].ToString(), out fid);
            Properties.Settings.Default["Ram"] = fid;
            Properties.Settings.Default["RamIndex"] = RamBox.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void OfflineUsernameBox_TextChanged(object sender, EventArgs e)
        {
            string tempUs = OfflineUsernameBox.Text;
            Properties.Settings.Default["Username"] = tempUs;
            Properties.Settings.Default.Save();

            mainForm.PlayActive();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Location = this.Location;
            mainForm.Show();
            mainForm.PlayActive();

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["Console"] = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }
    }
}

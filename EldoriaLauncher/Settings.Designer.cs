namespace EldoriaLauncher
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            RamBox = new ComboBox();
            OfflineUsernameBox = new TextBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // RamBox
            // 
            RamBox.AccessibleRole = AccessibleRole.None;
            RamBox.BackColor = Color.FromArgb(110, 110, 110);
            RamBox.DisplayMember = "1";
            RamBox.FlatStyle = FlatStyle.Flat;
            RamBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            RamBox.ForeColor = Color.White;
            RamBox.FormattingEnabled = true;
            RamBox.Items.AddRange(new object[] { "2048 Mb", "3072 Mb", "4096 Mb", "5120 Mb", "6144 Mb", "7168 Mb", "8192 Mb", "9216 Mb", "10240 Mb", "11264 Mb", "12288 Mb", "13312 Mb", "14336 Mb", "15360 Mb", "16384 Mb", "17408 Mb", "18432 Mb", "19456 Mb", "20480 Mb" });
            RamBox.Location = new Point(262, 210);
            RamBox.Margin = new Padding(4, 3, 4, 3);
            RamBox.Name = "RamBox";
            RamBox.Size = new Size(289, 28);
            RamBox.TabIndex = 2;
            RamBox.Text = "2048 Mb";
            RamBox.SelectedIndexChanged += RamBox_SelectedIndexChanged;
            // 
            // OfflineUsernameBox
            // 
            OfflineUsernameBox.BackColor = Color.FromArgb(110, 110, 110);
            OfflineUsernameBox.BorderStyle = BorderStyle.None;
            OfflineUsernameBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            OfflineUsernameBox.ForeColor = Color.White;
            OfflineUsernameBox.Location = new Point(262, 107);
            OfflineUsernameBox.Margin = new Padding(4, 3, 4, 3);
            OfflineUsernameBox.Name = "OfflineUsernameBox";
            OfflineUsernameBox.Size = new Size(289, 19);
            OfflineUsernameBox.TabIndex = 4;
            OfflineUsernameBox.Text = "AAA";
            OfflineUsernameBox.TextChanged += OfflineUsernameBox_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = Properties.Resources.atrás;
            pictureBox1.Location = new Point(24, 15);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(23, 23);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.menu_gui;
            ClientSize = new Size(816, 489);
            Controls.Add(pictureBox1);
            Controls.Add(OfflineUsernameBox);
            Controls.Add(RamBox);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            MouseDown += Settings_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox RamBox;
        private System.Windows.Forms.TextBox OfflineUsernameBox;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
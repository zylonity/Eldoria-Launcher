﻿namespace EldoriaLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            RamBox = new ComboBox();
            OfflineUsernameBox = new TextBox();
            pictureBox1 = new PictureBox();
            checkBox1 = new CheckBox();
            ver = new Label();
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
            RamBox.Items.AddRange(new object[] { "4096", "5120", "6144", "7168", "8192", "9216", "10240", "11264", "12288", "13312", "14336", "15360", "16384", "17408", "18432", "19456", "20480" });
            RamBox.Location = new Point(72, 285);
            RamBox.Margin = new Padding(4, 3, 4, 3);
            RamBox.Name = "RamBox";
            RamBox.Size = new Size(289, 28);
            RamBox.TabIndex = 2;
            RamBox.Text = "4096";
            RamBox.SelectedIndexChanged += RamBox_SelectedIndexChanged;
            // 
            // OfflineUsernameBox
            // 
            OfflineUsernameBox.BackColor = Color.FromArgb(110, 110, 110);
            OfflineUsernameBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            OfflineUsernameBox.ForeColor = Color.White;
            OfflineUsernameBox.Location = new Point(72, 194);
            OfflineUsernameBox.Margin = new Padding(4, 3, 4, 3);
            OfflineUsernameBox.Name = "OfflineUsernameBox";
            OfflineUsernameBox.Size = new Size(289, 26);
            OfflineUsernameBox.TabIndex = 4;
            OfflineUsernameBox.Text = "AAA";
            OfflineUsernameBox.TextChanged += OfflineUsernameBox_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = Properties.Resources.atrás;
            pictureBox1.Location = new Point(13, 12);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(23, 23);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(339, 460);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(83, 20);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Consola";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // ver
            // 
            ver.AutoSize = true;
            ver.BackColor = Color.Transparent;
            ver.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            ver.ForeColor = Color.White;
            ver.Location = new Point(13, 460);
            ver.Name = "ver";
            ver.Size = new Size(60, 16);
            ver.TabIndex = 7;
            ver.Text = "Version";
            // 
            // Settings
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImage = Properties.Resources.fondo_ajustes;
            ClientSize = new Size(433, 489);
            Controls.Add(ver);
            Controls.Add(checkBox1);
            Controls.Add(pictureBox1);
            Controls.Add(OfflineUsernameBox);
            Controls.Add(RamBox);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings";
            StartPosition = FormStartPosition.Manual;
            Text = "Settings";
            Load += Settings_Load;
            MouseDown += Settings_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox RamBox;
        private System.Windows.Forms.TextBox OfflineUsernameBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private CheckBox checkBox1;
        private Label ver;
    }
}
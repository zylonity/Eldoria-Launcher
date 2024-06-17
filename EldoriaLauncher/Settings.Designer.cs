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
            checkBox1 = new CheckBox();
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
            RamBox.Location = new Point(85, 212);
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
            OfflineUsernameBox.BorderStyle = BorderStyle.None;
            OfflineUsernameBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            OfflineUsernameBox.ForeColor = Color.White;
            OfflineUsernameBox.Location = new Point(85, 110);
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
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Location = new Point(352, 458);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(69, 19);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Consola";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Settings
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImage = Properties.Resources.menu_gui;
            ClientSize = new Size(433, 489);
            Controls.Add(checkBox1);
            Controls.Add(pictureBox1);
            Controls.Add(OfflineUsernameBox);
            Controls.Add(RamBox);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings";
            StartPosition = FormStartPosition.Manual;
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
        private CheckBox checkBox1;
    }
}
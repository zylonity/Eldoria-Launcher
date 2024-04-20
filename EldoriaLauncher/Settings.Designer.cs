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
            this.RamBox = new System.Windows.Forms.ComboBox();
            this.OfflineUsernameBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RamBox
            // 
            this.RamBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.RamBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.RamBox.DisplayMember = "1";
            this.RamBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RamBox.Font = new System.Drawing.Font("Minecraft", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RamBox.ForeColor = System.Drawing.Color.White;
            this.RamBox.FormattingEnabled = true;
            this.RamBox.Items.AddRange(new object[] {
            "2048 Mb",
            "3072 Mb",
            "4096 Mb",
            "5120 Mb",
            "6144 Mb",
            "7168 Mb",
            "8192 Mb",
            "9216 Mb",
            "10240 Mb",
            "11264 Mb",
            "12288 Mb",
            "13312 Mb",
            "14336 Mb",
            "15360 Mb",
            "16384 Mb",
            "17408 Mb",
            "18432 Mb",
            "19456 Mb",
            "20480 Mb"});
            this.RamBox.Location = new System.Drawing.Point(267, 212);
            this.RamBox.Name = "RamBox";
            this.RamBox.Size = new System.Drawing.Size(281, 24);
            this.RamBox.TabIndex = 2;
            this.RamBox.Text = "2048 Mb";
            this.RamBox.SelectedIndexChanged += new System.EventHandler(this.RamBox_SelectedIndexChanged);
            // 
            // OfflineUsernameBox
            // 
            this.OfflineUsernameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.OfflineUsernameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OfflineUsernameBox.Font = new System.Drawing.Font("Minecraft", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OfflineUsernameBox.ForeColor = System.Drawing.Color.White;
            this.OfflineUsernameBox.Location = new System.Drawing.Point(267, 112);
            this.OfflineUsernameBox.Name = "OfflineUsernameBox";
            this.OfflineUsernameBox.Size = new System.Drawing.Size(281, 18);
            this.OfflineUsernameBox.TabIndex = 4;
            this.OfflineUsernameBox.Text = "AAA";
            this.OfflineUsernameBox.TextChanged += new System.EventHandler(this.OfflineUsernameBox_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::EldoriaLauncher.Properties.Resources.atrás;
            this.pictureBox1.Location = new System.Drawing.Point(25, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EldoriaLauncher.Properties.Resources.menu_gui;
            this.ClientSize = new System.Drawing.Size(816, 489);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.OfflineUsernameBox);
            this.Controls.Add(this.RamBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox RamBox;
        private System.Windows.Forms.TextBox OfflineUsernameBox;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
﻿namespace EldoriaLauncher
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            RunBar = new ProgressBar();
            pictureBox5 = new PictureBox();
            textBox1 = new TextBox();
            MainPanel = new Panel();
            SettingsPanel = new Panel();
            SettingsBack = new PictureBox();
            ver = new Label();
            checkBox1 = new CheckBox();
            OfflineUsernameBox = new TextBox();
            RamBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            MainPanel.SuspendLayout();
            SettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SettingsBack).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Enabled = false;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(81, 173);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(275, 71);
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            pictureBox1.EnabledChanged += pictureBox1_EnabledChanged;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseEnter += pictureBox1_MouseEnter;
            pictureBox1.MouseLeave += pictureBox1_MouseLeave;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = Properties.Resources.cerrar;
            pictureBox2.Location = new Point(403, 11);
            pictureBox2.Margin = new Padding(4, 3, 4, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(23, 23);
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Image = Properties.Resources.minimizar;
            pictureBox3.Location = new Point(369, 19);
            pictureBox3.Margin = new Padding(4, 3, 4, 3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(25, 11);
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox3.TabIndex = 15;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(236, 257);
            pictureBox4.Margin = new Padding(4, 3, 4, 3);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(176, 61);
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            pictureBox4.EnabledChanged += pictureBox4_EnabledChanged;
            pictureBox4.Click += pictureBox4_Click;
            pictureBox4.MouseDown += pictureBox4_MouseDown;
            pictureBox4.MouseEnter += pictureBox4_MouseEnter;
            pictureBox4.MouseLeave += pictureBox4_MouseLeave;
            pictureBox4.MouseUp += pictureBox4_MouseUp;
            // 
            // RunBar
            // 
            RunBar.Location = new Point(127, 427);
            RunBar.Name = "RunBar";
            RunBar.Size = new Size(183, 21);
            RunBar.TabIndex = 18;
            RunBar.Visible = false;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.Transparent;
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(24, 257);
            pictureBox5.Margin = new Padding(4, 3, 4, 3);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(176, 61);
            pictureBox5.TabIndex = 20;
            pictureBox5.TabStop = false;
            pictureBox5.EnabledChanged += pictureBox5_EnabledChanged;
            pictureBox5.Click += pictureBox5_Click;
            pictureBox5.MouseDown += pictureBox5_MouseDown;
            pictureBox5.MouseEnter += pictureBox5_MouseEnter;
            pictureBox5.MouseLeave += pictureBox5_MouseLeave;
            pictureBox5.MouseUp += pictureBox5_MouseUp;
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(12, 454);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(409, 23);
            textBox1.TabIndex = 22;
            textBox1.Visible = false;
            // 
            // MainPanel
            // 
            MainPanel.BackgroundImage = Properties.Resources.fondo_principal;
            MainPanel.Controls.Add(pictureBox1);
            MainPanel.Controls.Add(textBox1);
            MainPanel.Controls.Add(pictureBox2);
            MainPanel.Controls.Add(pictureBox3);
            MainPanel.Controls.Add(pictureBox5);
            MainPanel.Controls.Add(pictureBox4);
            MainPanel.Controls.Add(RunBar);
            MainPanel.Location = new Point(0, 0);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(433, 489);
            MainPanel.TabIndex = 23;
            // 
            // SettingsPanel
            // 
            SettingsPanel.BackgroundImage = Properties.Resources.fondo_ajustes;
            SettingsPanel.Controls.Add(SettingsBack);
            SettingsPanel.Controls.Add(ver);
            SettingsPanel.Controls.Add(checkBox1);
            SettingsPanel.Controls.Add(OfflineUsernameBox);
            SettingsPanel.Controls.Add(RamBox);
            SettingsPanel.Location = new Point(433, 0);
            SettingsPanel.Name = "SettingsPanel";
            SettingsPanel.Size = new Size(433, 489);
            SettingsPanel.TabIndex = 25;
            SettingsPanel.Visible = false;
            // 
            // SettingsBack
            // 
            SettingsBack.BackColor = Color.Transparent;
            SettingsBack.Cursor = Cursors.Hand;
            SettingsBack.Image = Properties.Resources.atrás;
            SettingsBack.Location = new Point(7, 11);
            SettingsBack.Margin = new Padding(4, 3, 4, 3);
            SettingsBack.Name = "SettingsBack";
            SettingsBack.Size = new Size(23, 23);
            SettingsBack.SizeMode = PictureBoxSizeMode.CenterImage;
            SettingsBack.TabIndex = 28;
            SettingsBack.TabStop = false;
            SettingsBack.Click += SettingsBack_Click;
            // 
            // ver
            // 
            ver.AutoSize = true;
            ver.BackColor = Color.Transparent;
            ver.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
            ver.ForeColor = Color.White;
            ver.Location = new Point(19, 461);
            ver.Name = "ver";
            ver.Size = new Size(60, 16);
            ver.TabIndex = 27;
            ver.Text = "Version";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(335, 457);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(83, 20);
            checkBox1.TabIndex = 26;
            checkBox1.Text = "Consola";
            checkBox1.UseVisualStyleBackColor = false;
            // 
            // OfflineUsernameBox
            // 
            OfflineUsernameBox.BackColor = Color.FromArgb(110, 110, 110);
            OfflineUsernameBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            OfflineUsernameBox.ForeColor = Color.White;
            OfflineUsernameBox.Location = new Point(72, 185);
            OfflineUsernameBox.Margin = new Padding(4, 3, 4, 3);
            OfflineUsernameBox.Name = "OfflineUsernameBox";
            OfflineUsernameBox.Size = new Size(289, 26);
            OfflineUsernameBox.TabIndex = 6;
            OfflineUsernameBox.Text = "AAA";
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
            RamBox.Location = new Point(72, 276);
            RamBox.Margin = new Padding(4, 3, 4, 3);
            RamBox.Name = "RamBox";
            RamBox.Size = new Size(289, 28);
            RamBox.TabIndex = 5;
            RamBox.Text = "4096";
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImage = Properties.Resources.Launcher_fondo_menus;
            ClientSize = new Size(1299, 489);
            Controls.Add(SettingsPanel);
            Controls.Add(MainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Eldoria Launcher";
            MouseDown += Form1_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            MainPanel.ResumeLayout(false);
            MainPanel.PerformLayout();
            SettingsPanel.ResumeLayout(false);
            SettingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SettingsBack).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private ProgressBar RunBar;
        private PictureBox pictureBox5;
        private TextBox textBox1;
        private Panel MainPanel;
        private Panel SettingsPanel;
        private TextBox OfflineUsernameBox;
        private ComboBox RamBox;
        private CheckBox checkBox1;
        private Label ver;
        private PictureBox SettingsBack;
    }
}


namespace EldoriaLauncher
{
    partial class Installer
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
            pictureBox1 = new PictureBox();
            checkedListBox1 = new CheckedListBox();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
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
            // checkedListBox1
            // 
            checkedListBox1.Anchor = AnchorStyles.Top;
            checkedListBox1.BackColor = Color.FromArgb(200, 200, 200);
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(86, 52);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(645, 310);
            checkedListBox1.TabIndex = 6;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(616, 397);
            button1.Name = "button1";
            button1.Size = new Size(124, 42);
            button1.TabIndex = 7;
            button1.Text = "Install";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Installer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.blank_gui;
            ClientSize = new Size(816, 489);
            Controls.Add(button1);
            Controls.Add(checkedListBox1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Installer";
            StartPosition = FormStartPosition.Manual;
            Text = "Settings";
            MouseDown += Settings_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private CheckedListBox checkedListBox1;
        private Button button1;
    }
}
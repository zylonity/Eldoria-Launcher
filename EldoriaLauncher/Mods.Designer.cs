namespace EldoriaLauncher
{
    partial class Mods
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
            button1 = new Button();
            currentDownload = new Label();
            progressBar1 = new ProgressBar();
            progressBar2 = new ProgressBar();
            checkBox1 = new CheckBox();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            objectListView1 = new BrightIdeasSoftware.ObjectListView();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)objectListView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(277, 435);
            button1.Name = "button1";
            button1.Size = new Size(124, 42);
            button1.TabIndex = 7;
            button1.Text = "Actualizar Mods";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // currentDownload
            // 
            currentDownload.AutoSize = true;
            currentDownload.BackColor = Color.Transparent;
            currentDownload.Location = new Point(12, 442);
            currentDownload.Name = "currentDownload";
            currentDownload.Size = new Size(38, 15);
            currentDownload.TabIndex = 8;
            currentDownload.Text = "label1";
            currentDownload.Visible = false;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(43, 381);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(215, 26);
            progressBar1.TabIndex = 9;
            // 
            // progressBar2
            // 
            progressBar2.Location = new Point(12, 460);
            progressBar2.Name = "progressBar2";
            progressBar2.Size = new Size(160, 19);
            progressBar2.TabIndex = 10;
            progressBar2.Visible = false;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Location = new Point(277, 381);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(136, 19);
            checkBox1.TabIndex = 11;
            checkBox1.Text = "Descargar con Async";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(43, 363);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 16;
            label3.Text = "label3";
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
            pictureBox1.TabIndex = 18;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // objectListView1
            // 
            objectListView1.BackColor = Color.White;
            objectListView1.CellEditUseWholeCell = false;
            objectListView1.CheckBoxes = true;
            objectListView1.Location = new Point(203, 74);
            objectListView1.Name = "objectListView1";
            objectListView1.Size = new Size(210, 254);
            objectListView1.TabIndex = 19;
            objectListView1.View = View.Details;
            // 
            // Mods
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Launcher_fondo_menus;
            ClientSize = new Size(433, 489);
            Controls.Add(objectListView1);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(checkBox1);
            Controls.Add(progressBar2);
            Controls.Add(progressBar1);
            Controls.Add(currentDownload);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Mods";
            StartPosition = FormStartPosition.Manual;
            Text = "Settings";
            KeyDown += Mods_KeyDown;
            MouseDown += Settings_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)objectListView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private Label currentDownload;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private CheckBox checkBox1;
        private Label label3;
        private PictureBox pictureBox1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
    }
}
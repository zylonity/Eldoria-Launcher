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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mods));
            button1 = new Button();
            progressBar1 = new ProgressBar();
            checkBox1 = new CheckBox();
            pictureBox1 = new PictureBox();
            objectListView1 = new BrightIdeasSoftware.ObjectListView();
            comboBox1 = new ComboBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)objectListView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(282, 410);
            button1.Name = "button1";
            button1.Size = new Size(124, 42);
            button1.TabIndex = 7;
            button1.Text = "Actualizar Mods";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(31, 418);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(215, 26);
            progressBar1.TabIndex = 9;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Location = new Point(275, 456);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(136, 19);
            checkBox1.TabIndex = 11;
            checkBox1.Text = "Descargar con Async";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.Visible = false;
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
            objectListView1.Location = new Point(193, 144);
            objectListView1.Name = "objectListView1";
            objectListView1.Size = new Size(210, 246);
            objectListView1.TabIndex = 19;
            objectListView1.View = View.Details;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(24, 186);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(163, 23);
            comboBox1.TabIndex = 20;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button2
            // 
            button2.Location = new Point(36, 244);
            button2.Name = "button2";
            button2.Size = new Size(137, 37);
            button2.TabIndex = 21;
            button2.Text = "Crear Preset";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(36, 287);
            button3.Name = "button3";
            button3.Size = new Size(137, 37);
            button3.TabIndex = 22;
            button3.Text = "Guardar Preset";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(36, 330);
            button4.Name = "button4";
            button4.Size = new Size(137, 37);
            button4.TabIndex = 23;
            button4.Text = "Borrar Preset";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Mods
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImage = Properties.Resources.fondo_modsl;
            ClientSize = new Size(433, 489);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(comboBox1);
            Controls.Add(objectListView1);
            Controls.Add(pictureBox1);
            Controls.Add(checkBox1);
            Controls.Add(progressBar1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Mods";
            StartPosition = FormStartPosition.Manual;
            Text = "Mods";
            KeyDown += Mods_KeyDown;
            MouseDown += Settings_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)objectListView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private ProgressBar progressBar1;
        private CheckBox checkBox1;
        private PictureBox pictureBox1;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private ComboBox comboBox1;
        private Button button2;
        private Button button3;
        private Button button4;
    }
}
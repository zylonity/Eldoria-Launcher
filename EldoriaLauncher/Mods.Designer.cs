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
            checkedListBox1 = new CheckedListBox();
            button1 = new Button();
            currentDownload = new Label();
            progressBar1 = new ProgressBar();
            progressBar2 = new ProgressBar();
            checkBox1 = new CheckBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            listView1 = new ListView();
            modsList = new ColumnHeader();
            columnHeader1 = new ColumnHeader();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // checkedListBox1
            // 
            checkedListBox1.Anchor = AnchorStyles.Top;
            checkedListBox1.BackColor = Color.FromArgb(200, 200, 200);
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(531, 37);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(200, 292);
            checkedListBox1.TabIndex = 6;
            checkedListBox1.Visible = false;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(607, 384);
            button1.Name = "button1";
            button1.Size = new Size(124, 42);
            button1.TabIndex = 7;
            button1.Text = "Actualizar Mods";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
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
            progressBar1.Location = new Point(96, 391);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(451, 26);
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
            checkBox1.Location = new Point(607, 431);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(136, 19);
            checkBox1.TabIndex = 11;
            checkBox1.Text = "Descargar con Async";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.Visible = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(611, 468);
            label1.Name = "label1";
            label1.Size = new Size(132, 15);
            label1.TabIndex = 12;
            label1.Text = "decente y buen internet";
            label1.Visible = false;
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(595, 453);
            label2.Name = "label2";
            label2.Size = new Size(164, 15);
            label2.TabIndex = 13;
            label2.Text = "Recomendado si tienes un PC";
            label2.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(96, 365);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 16;
            label3.Text = "label3";
            label3.Click += label3_Click;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { modsList, columnHeader1 });
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.Location = new Point(148, 36);
            listView1.Name = "listView1";
            listView1.ShowItemToolTips = true;
            listView1.Size = new Size(307, 278);
            listView1.TabIndex = 17;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // modsList
            // 
            modsList.Text = "ModsList";
            modsList.Width = 220;
            // 
            // columnHeader1
            // 
            columnHeader1.Width = 0;
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
            // Mods
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.blank_gui;
            ClientSize = new Size(816, 489);
            Controls.Add(pictureBox1);
            Controls.Add(listView1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(checkBox1);
            Controls.Add(progressBar2);
            Controls.Add(progressBar1);
            Controls.Add(currentDownload);
            Controls.Add(button1);
            Controls.Add(checkedListBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Mods";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            MouseDown += Settings_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private CheckedListBox checkedListBox1;
        private Button button1;
        private Label currentDownload;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private CheckBox checkBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private ListView listView1;
        private ColumnHeader modsList;
        private ColumnHeader columnHeader1;
        private PictureBox pictureBox1;
    }
}
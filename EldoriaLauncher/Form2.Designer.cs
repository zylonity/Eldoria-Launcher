namespace EldoriaLauncher
{
    partial class Downloading
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
            progressBar1 = new ProgressBar();
            DownloadBigLabel = new Label();
            progressBar2 = new ProgressBar();
            CurrentDownload = new Label();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(57, 83);
            progressBar1.Margin = new Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(394, 31);
            progressBar1.Step = 1;
            progressBar1.TabIndex = 0;
            // 
            // DownloadBigLabel
            // 
            DownloadBigLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            DownloadBigLabel.AutoSize = true;
            DownloadBigLabel.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DownloadBigLabel.Location = new Point(76, 42);
            DownloadBigLabel.Margin = new Padding(4, 0, 4, 0);
            DownloadBigLabel.Name = "DownloadBigLabel";
            DownloadBigLabel.Size = new Size(363, 25);
            DownloadBigLabel.TabIndex = 1;
            DownloadBigLabel.Text = "Descargando los archivos iniciales...";
            // 
            // progressBar2
            // 
            progressBar2.Location = new Point(57, 160);
            progressBar2.Name = "progressBar2";
            progressBar2.Size = new Size(394, 19);
            progressBar2.TabIndex = 2;
            // 
            // CurrentDownload
            // 
            CurrentDownload.AutoSize = true;
            CurrentDownload.Location = new Point(76, 142);
            CurrentDownload.Name = "CurrentDownload";
            CurrentDownload.Size = new Size(127, 15);
            CurrentDownload.TabIndex = 3;
            CurrentDownload.Text = "CurrentlyDownloading";
            CurrentDownload.Click += CurrentDownload_Click;
            // 
            // Downloading
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(515, 201);
            ControlBox = false;
            Controls.Add(CurrentDownload);
            Controls.Add(progressBar2);
            Controls.Add(DownloadBigLabel);
            Controls.Add(progressBar1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Downloading";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Descargando...";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public System.Windows.Forms.Label DownloadBigLabel;
        public System.Windows.Forms.ProgressBar progressBar1;
        public Label CurrentDownload;
        public ProgressBar progressBar2;
    }
}
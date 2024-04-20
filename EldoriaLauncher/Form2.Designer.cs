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
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(58, 128);
            progressBar1.Margin = new Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(467, 39);
            progressBar1.Step = 1;
            progressBar1.TabIndex = 0;
            progressBar1.Click += progressBar1_Click_1;
            // 
            // DownloadBigLabel
            // 
            DownloadBigLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            DownloadBigLabel.AutoSize = true;
            DownloadBigLabel.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DownloadBigLabel.Location = new Point(77, 59);
            DownloadBigLabel.Margin = new Padding(4, 0, 4, 0);
            DownloadBigLabel.Name = "DownloadBigLabel";
            DownloadBigLabel.Size = new Size(363, 25);
            DownloadBigLabel.TabIndex = 1;
            DownloadBigLabel.Text = "Descargando los archivos iniciales...";
            DownloadBigLabel.Click += DownloadBigLabel_Click_1;
            // 
            // Downloading
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(588, 209);
            ControlBox = false;
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
    }
}
using System;

namespace RumboSGA.Presentation
{
    partial class SFDetallesArticulos
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
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.myBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.dataDialog1 = new RumboSGA.UserControls.ArticulosTabControl();
            this.SuspendLayout();
            // 
            // dataDialog1
            // 
            this.dataDialog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataDialog1.Location = new System.Drawing.Point(0, 0);
            this.dataDialog1.Margin = new System.Windows.Forms.Padding(0);
            this.dataDialog1.Name = "dataDialog1";
            this.dataDialog1.Size = new System.Drawing.Size(1444, 420);
            this.dataDialog1.TabIndex = 0;
            // 
            // SFDetallesArticulos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1444, 420);
            this.Controls.Add(this.dataDialog1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SFDetallesArticulos";
            this.Text = "SFDetalles";
            this.ResumeLayout(false);

        }


        #endregion
        private System.ComponentModel.BackgroundWorker myBackgroundWorker;
        public RumboSGA.UserControls.ArticulosTabControl dataDialog1;
    }
}

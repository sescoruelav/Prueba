
namespace RumboSGA.Controles
{
    partial class ArchivoJira
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.radPictureBox1 = new Telerik.WinControls.UI.RadPictureBox();
            this.rumLabel1 = new RumboSGA.RumLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radPictureBox1
            // 
            this.radPictureBox1.ImageLayout = Telerik.WinControls.UI.RadImageLayout.FitIntoBounds;
            this.radPictureBox1.Location = new System.Drawing.Point(15, 29);
            this.radPictureBox1.Name = "radPictureBox1";
            this.radPictureBox1.Size = new System.Drawing.Size(115, 101);
            this.radPictureBox1.SvgImageXml = null;
            this.radPictureBox1.TabIndex = 0;
            // 
            // rumLabel1
            // 
            this.rumLabel1.Location = new System.Drawing.Point(45, 136);
            this.rumLabel1.Name = "rumLabel1";
            this.rumLabel1.Size = new System.Drawing.Size(57, 15);
            this.rumLabel1.TabIndex = 0;
            this.rumLabel1.Text = "rumLabel1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.pictureBox1.Location = new System.Drawing.Point(119, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // ArchivoJira
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.rumLabel1);
            this.Controls.Add(this.radPictureBox1);
            this.Name = "ArchivoJira";
            this.Size = new System.Drawing.Size(145, 154);
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadPictureBox radPictureBox1;
        private RumLabel rumLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

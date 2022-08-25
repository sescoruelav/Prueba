
using System;

namespace RumboSGA.Presentation
{
    partial class SFCierre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFCierre));
            this.rumLabelTitulo = new RumboSGA.RumLabel();
            this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            this.SuspendLayout();
            // 
            // rumLabelTitulo
            // 
            this.rumLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rumLabelTitulo.Location = new System.Drawing.Point(94, 51);
            this.rumLabelTitulo.Name = "rumLabelTitulo";
            this.rumLabelTitulo.Size = new System.Drawing.Size(196, 21);
            this.rumLabelTitulo.TabIndex = 6;
            this.rumLabelTitulo.Text = "Estas seguro que quieres salir";
            // 
            // rumButtonAceptar
            // 
            this.rumButtonAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rumButtonAceptar.BackColor = System.Drawing.Color.Transparent;
            this.rumButtonAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.rumButtonAceptar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.rumButtonAceptar.Location = new System.Drawing.Point(94, 120);
            this.rumButtonAceptar.Name = "rumButtonAceptar";
            this.rumButtonAceptar.Size = new System.Drawing.Size(74, 60);
            this.rumButtonAceptar.TabIndex = 3;
            this.rumButtonAceptar.Text = "Aceptar";
            this.rumButtonAceptar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rumButtonCancelar.BackColor = System.Drawing.Color.Transparent;
            this.rumButtonCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rumButtonCancelar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.rumButtonCancelar.Location = new System.Drawing.Point(202, 120);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(74, 60);
            this.rumButtonCancelar.TabIndex = 2;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // SFCierre
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = false;
            this.ClientSize = new System.Drawing.Size(400, 207);
            this.Controls.Add(this.rumLabelTitulo);
            this.Controls.Add(this.rumButtonAceptar);
            this.Controls.Add(this.rumButtonCancelar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SFCierre";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Salir";
            this.Load += new System.EventHandler(this.SFCierre_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private RumButton rumButtonCancelar;
        private RumButton rumButtonAceptar;
        private RumLabel rumLabelTitulo;

        #endregion
    }
}
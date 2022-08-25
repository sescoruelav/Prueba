
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace EstadoRecepciones
{
    partial class clasificacionEntradasForm
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
            this.SuspendLayout();
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            int prueba = MaximumSize.Width;
            this.ClientSize = new System.Drawing.Size(MaximumSize.Width, MaximumSize.Height);
            this.Text = "Preclasificacion Entradas";
            this.Size = new System.Drawing.Size(MaximumSize.Width, MaximumSize.Height);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.WindowState = FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        #endregion
    }
}
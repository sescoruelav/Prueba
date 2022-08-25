namespace RumboSGA.Presentation.FormulariosComunes
{
    partial class VisorComentarios
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Imagen = new System.Windows.Forms.PictureBox();
            this.rumGridComentarios = new RumboSGA.Controles.RumGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Imagen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumGridComentarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumGridComentarios.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.Imagen);
            this.panel1.Location = new System.Drawing.Point(0, 269);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1039, 497);
            this.panel1.TabIndex = 1;
            // 
            // Imagen
            // 
            this.Imagen.InitialImage = global::RumboSGA.Properties.Resources.Delete;
            this.Imagen.Location = new System.Drawing.Point(8, 8);
            this.Imagen.Name = "Imagen";
            this.Imagen.Size = new System.Drawing.Size(1033, 468);
            this.Imagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Imagen.TabIndex = 1;
            this.Imagen.TabStop = false;
            // 
            // rumGridComentarios
            // 
            this.rumGridComentarios.AutoSizeRows = true;
            this.rumGridComentarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.rumGridComentarios.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rumGridComentarios.MasterTemplate.AllowAddNewRow = false;
            this.rumGridComentarios.MasterTemplate.AllowColumnChooser = false;
            this.rumGridComentarios.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.rumGridComentarios.MasterTemplate.AllowColumnReorder = false;
            this.rumGridComentarios.MasterTemplate.AllowColumnResize = false;
            this.rumGridComentarios.MasterTemplate.AllowDeleteRow = false;
            this.rumGridComentarios.MasterTemplate.AllowEditRow = false;
            this.rumGridComentarios.MasterTemplate.AllowRowResize = false;
            this.rumGridComentarios.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.rumGridComentarios.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rumGridComentarios.Name = "rumGridComentarios";
            this.rumGridComentarios.Size = new System.Drawing.Size(1039, 263);
            this.rumGridComentarios.TabIndex = 0;
            this.rumGridComentarios.DoubleClick += new System.EventHandler(this.rumGridComentarios_DoubleClick);
            // 
            // VisorComentarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 768);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rumGridComentarios);
            this.Name = "VisorComentarios";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Visor Comentarios";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Imagen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumGridComentarios.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumGridComentarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controles.RumGridView rumGridComentarios;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox Imagen;
    }
}

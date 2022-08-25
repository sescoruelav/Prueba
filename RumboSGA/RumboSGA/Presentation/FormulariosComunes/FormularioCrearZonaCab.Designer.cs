namespace RumboSGA.Presentation.FormulariosComunes
{
    partial class FormularioCrearZonaCab
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormularioCrearZonaCab));
            this.radTextBoxDescripcion = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBoxTipoZona = new Telerik.WinControls.UI.RadTextBox();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumLabelTipoZona = new RumboSGA.RumLabel();
            this.rumLabelDescripcion = new RumboSGA.RumLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxDescripcion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxTipoZona)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTipoZona)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelDescripcion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxDescripcion
            // 
            this.radTextBoxDescripcion.Location = new System.Drawing.Point(118, 17);
            this.radTextBoxDescripcion.Name = "radTextBoxDescripcion";
            this.radTextBoxDescripcion.Size = new System.Drawing.Size(250, 20);
            this.radTextBoxDescripcion.TabIndex = 4;
            // 
            // radTextBoxTipoZona
            // 
            this.radTextBoxTipoZona.Location = new System.Drawing.Point(118, 52);
            this.radTextBoxTipoZona.Name = "radTextBoxTipoZona";
            this.radTextBoxTipoZona.Size = new System.Drawing.Size(57, 20);
            this.radTextBoxTipoZona.TabIndex = 5;
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(258, 83);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonCancelar.TabIndex = 3;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // rumButtonAceptar
            // 
            this.rumButtonAceptar.Location = new System.Drawing.Point(12, 83);
            this.rumButtonAceptar.Name = "rumButtonAceptar";
            this.rumButtonAceptar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonAceptar.TabIndex = 2;
            this.rumButtonAceptar.Text = "Crear zona";
            this.rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            // 
            // rumLabelTipoZona
            // 
            this.rumLabelTipoZona.AutoSize = false;
            this.rumLabelTipoZona.Location = new System.Drawing.Point(12, 47);
            this.rumLabelTipoZona.Name = "rumLabelTipoZona";
            this.rumLabelTipoZona.Size = new System.Drawing.Size(100, 29);
            this.rumLabelTipoZona.TabIndex = 1;
            this.rumLabelTipoZona.Text = "Tipo zona";
            // 
            // rumLabelDescripcion
            // 
            this.rumLabelDescripcion.AutoSize = false;
            this.rumLabelDescripcion.Location = new System.Drawing.Point(12, 12);
            this.rumLabelDescripcion.Name = "rumLabelDescripcion";
            this.rumLabelDescripcion.Size = new System.Drawing.Size(100, 29);
            this.rumLabelDescripcion.TabIndex = 0;
            this.rumLabelDescripcion.Text = "Descripcion zona ";
            // 
            // FormularioCrearZonaCab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 119);
            this.Controls.Add(this.radTextBoxTipoZona);
            this.Controls.Add(this.radTextBoxDescripcion);
            this.Controls.Add(this.rumButtonCancelar);
            this.Controls.Add(this.rumButtonAceptar);
            this.Controls.Add(this.rumLabelTipoZona);
            this.Controls.Add(this.rumLabelDescripcion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormularioCrearZonaCab";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Crear zona";
            this.Load += new System.EventHandler(this.FormularioCrearZonaCab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxDescripcion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxTipoZona)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTipoZona)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelDescripcion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RumLabel rumLabelDescripcion;
        private RumLabel rumLabelTipoZona;
        private RumButton rumButtonAceptar;
        private RumButton rumButtonCancelar;
        private Telerik.WinControls.UI.RadTextBox radTextBoxDescripcion;
        private Telerik.WinControls.UI.RadTextBox radTextBoxTipoZona;
    }
}

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class FormularioCrearLote
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
            this.rumLabelLote = new RumboSGA.RumLabel();
            this.rumLabelFechaCaducidad = new RumboSGA.RumLabel();
            this.radTextBoxLote = new Telerik.WinControls.UI.RadTextBox();
            this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.radDateTimeFechaCaducidad = new Telerik.WinControls.UI.RadDateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelLote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaCaducidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxLote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimeFechaCaducidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rumLabelLote
            // 
            this.rumLabelLote.AutoSize = false;
            this.rumLabelLote.ImageAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.rumLabelLote.Location = new System.Drawing.Point(12, 12);
            this.rumLabelLote.Name = "rumLabelLote";
            this.rumLabelLote.Size = new System.Drawing.Size(103, 18);
            this.rumLabelLote.TabIndex = 0;
            this.rumLabelLote.Text = "Lote:";
            this.rumLabelLote.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rumLabelFechaCaducidad
            // 
            this.rumLabelFechaCaducidad.AutoSize = false;
            this.rumLabelFechaCaducidad.ImageAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.rumLabelFechaCaducidad.Location = new System.Drawing.Point(12, 38);
            this.rumLabelFechaCaducidad.Name = "rumLabelFechaCaducidad";
            this.rumLabelFechaCaducidad.Size = new System.Drawing.Size(103, 18);
            this.rumLabelFechaCaducidad.TabIndex = 1;
            this.rumLabelFechaCaducidad.Text = "Fecha Caducidad:";
            this.rumLabelFechaCaducidad.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radTextBoxLote
            // 
            this.radTextBoxLote.Location = new System.Drawing.Point(121, 11);
            this.radTextBoxLote.Name = "radTextBoxLote";
            this.radTextBoxLote.Size = new System.Drawing.Size(190, 20);
            this.radTextBoxLote.TabIndex = 6;
            // 
            // rumButtonAceptar
            // 
            this.rumButtonAceptar.Location = new System.Drawing.Point(13, 204);
            this.rumButtonAceptar.Name = "rumButtonAceptar";
            this.rumButtonAceptar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonAceptar.TabIndex = 8;
            this.rumButtonAceptar.Text = "Aceptar";
            this.rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(220, 204);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonCancelar.TabIndex = 9;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // radDateTimeFechaCaducidad
            // 
            this.radDateTimeFechaCaducidad.Location = new System.Drawing.Point(121, 38);
            this.radDateTimeFechaCaducidad.Name = "radDateTimeFechaCaducidad";
            this.radDateTimeFechaCaducidad.Size = new System.Drawing.Size(190, 20);
            this.radDateTimeFechaCaducidad.TabIndex = 10;
            this.radDateTimeFechaCaducidad.TabStop = false;
            this.radDateTimeFechaCaducidad.Text = "miércoles, 17 de junio de 2020";
            this.radDateTimeFechaCaducidad.Value = new System.DateTime(2020, 6, 17, 16, 55, 23, 304);
            // 
            // FormularioCrearLote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 240);
            this.Controls.Add(this.radDateTimeFechaCaducidad);
            this.Controls.Add(this.rumButtonCancelar);
            this.Controls.Add(this.rumButtonAceptar);
            this.Controls.Add(this.radTextBoxLote);
            this.Controls.Add(this.rumLabelFechaCaducidad);
            this.Controls.Add(this.rumLabelLote);
            this.MaximumSize = new System.Drawing.Size(350, 270);
            this.MinimumSize = new System.Drawing.Size(350, 270);
            this.Name = "FormularioCrearLote";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(350, 270);
            this.Text = "FormularioCrearLote";
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelLote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaCaducidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxLote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimeFechaCaducidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RumLabel rumLabelLote;
        private RumLabel rumLabelFechaCaducidad;
        private Telerik.WinControls.UI.RadTextBox radTextBoxLote;
        private RumButton rumButtonAceptar;
        private RumButton rumButtonCancelar;
        private Telerik.WinControls.UI.RadDateTimePicker radDateTimeFechaCaducidad;
    }
}

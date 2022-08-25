namespace RumboSGA.Controles
{
    partial class VentanaDateTimePicker
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumDateTimePickerHasta = new RumboSGA.Controles.RumDateTimePicker();
            this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumLabelHasta = new RumboSGA.RumLabel();
            this.rumLabelDesde = new RumboSGA.RumLabel();
            this.rumDateTimePickerDesde = new RumboSGA.Controles.RumDateTimePicker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumDateTimePickerHasta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelHasta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelDesde)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumDateTimePickerDesde)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rumButtonCancelar);
            this.panel1.Controls.Add(this.rumDateTimePickerHasta);
            this.panel1.Controls.Add(this.rumButtonAceptar);
            this.panel1.Controls.Add(this.rumLabelHasta);
            this.panel1.Controls.Add(this.rumLabelDesde);
            this.panel1.Controls.Add(this.rumDateTimePickerDesde);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 193);
            this.panel1.TabIndex = 0;
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(155, 151);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonCancelar.TabIndex = 5;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // rumDateTimePickerHasta
            // 
            this.rumDateTimePickerHasta.CalendarSize = new System.Drawing.Size(290, 320);
            this.rumDateTimePickerHasta.Location = new System.Drawing.Point(3, 87);
            this.rumDateTimePickerHasta.Name = "rumDateTimePickerHasta";
            this.rumDateTimePickerHasta.Size = new System.Drawing.Size(254, 26);
            this.rumDateTimePickerHasta.TabIndex = 4;
            this.rumDateTimePickerHasta.TabStop = false;
            this.rumDateTimePickerHasta.Text = "martes, 16 de noviembre de 2021";
            this.rumDateTimePickerHasta.Value = new System.DateTime(2021, 11, 16, 13, 4, 17, 224);
            this.rumDateTimePickerHasta.ValueChanged += new System.EventHandler(this.RumDateTimePickerHasta_ValueChanged);
            // 
            // rumButtonAceptar
            // 
            this.rumButtonAceptar.Location = new System.Drawing.Point(3, 151);
            this.rumButtonAceptar.Name = "rumButtonAceptar";
            this.rumButtonAceptar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonAceptar.TabIndex = 3;
            this.rumButtonAceptar.Text = "Aceptar";
            this.rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            // 
            // rumLabelHasta
            // 
            this.rumLabelHasta.Location = new System.Drawing.Point(3, 61);
            this.rumLabelHasta.Name = "rumLabelHasta";
            this.rumLabelHasta.Size = new System.Drawing.Size(39, 20);
            this.rumLabelHasta.TabIndex = 2;
            this.rumLabelHasta.Text = "Hasta";
            // 
            // rumLabelDesde
            // 
            this.rumLabelDesde.Location = new System.Drawing.Point(3, 3);
            this.rumLabelDesde.Name = "rumLabelDesde";
            this.rumLabelDesde.Size = new System.Drawing.Size(43, 20);
            this.rumLabelDesde.TabIndex = 1;
            this.rumLabelDesde.Text = "Desde";
            // 
            // rumDateTimePickerDesde
            // 
            this.rumDateTimePickerDesde.CalendarSize = new System.Drawing.Size(290, 320);
            this.rumDateTimePickerDesde.Location = new System.Drawing.Point(3, 29);
            this.rumDateTimePickerDesde.Name = "rumDateTimePickerDesde";
            this.rumDateTimePickerDesde.ShowItemToolTips = false;
            this.rumDateTimePickerDesde.Size = new System.Drawing.Size(254, 26);
            this.rumDateTimePickerDesde.TabIndex = 0;
            this.rumDateTimePickerDesde.TabStop = false;
            this.rumDateTimePickerDesde.Text = "martes, 16 de noviembre de 2021";
            this.rumDateTimePickerDesde.Value = new System.DateTime(2021, 11, 16, 13, 4, 17, 224);
            this.rumDateTimePickerDesde.ValueChanged += new System.EventHandler(this.RumDateTimePickerDesde_ValueChanged);
            // 
            // VentanaDateTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 217);
            this.Controls.Add(this.panel1);
            this.Name = "VentanaDateTimePicker";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.Text = "Fecha";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumDateTimePickerHasta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelHasta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelDesde)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumDateTimePickerDesde)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private RumButton rumButtonCancelar;
        private RumDateTimePicker rumDateTimePickerHasta;
        private RumButton rumButtonAceptar;
        private RumLabel rumLabelHasta;
        private RumLabel rumLabelDesde;
        private RumDateTimePicker rumDateTimePickerDesde;
    }
}

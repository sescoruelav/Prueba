namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class FrmMostrarHistorialRecurso
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMostrarHistorialRecurso));
            this.radGridViewHistorial = new Telerik.WinControls.UI.RadGridView();
            this.rumLabelFechaInicio = new RumboSGA.RumLabel();
            this.radDateTimePickerFechaInicio = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radDateTimePickerFechaFinal = new Telerik.WinControls.UI.RadDateTimePicker();
            this.rumLabelFechaFinal = new RumboSGA.RumLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewHistorial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewHistorial.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaInicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerFechaInicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerFechaFinal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaFinal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridViewHistorial
            // 
            this.radGridViewHistorial.Location = new System.Drawing.Point(12, 53);
            // 
            // 
            // 
            this.radGridViewHistorial.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewHistorial.Name = "radGridViewHistorial";
            this.radGridViewHistorial.Size = new System.Drawing.Size(756, 380);
            this.radGridViewHistorial.TabIndex = 0;
            // 
            // rumLabelFechaInicio
            // 
            this.rumLabelFechaInicio.AutoSize = false;
            this.rumLabelFechaInicio.Location = new System.Drawing.Point(12, 12);
            this.rumLabelFechaInicio.Name = "rumLabelFechaInicio";
            this.rumLabelFechaInicio.Size = new System.Drawing.Size(93, 18);
            this.rumLabelFechaInicio.TabIndex = 1;
            this.rumLabelFechaInicio.Text = "Fecha inicio";
            this.rumLabelFechaInicio.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radDateTimePickerFechaInicio
            // 
            this.radDateTimePickerFechaInicio.Location = new System.Drawing.Point(112, 12);
            this.radDateTimePickerFechaInicio.Name = "radDateTimePickerFechaInicio";
            this.radDateTimePickerFechaInicio.Size = new System.Drawing.Size(193, 20);
            this.radDateTimePickerFechaInicio.TabIndex = 2;
            this.radDateTimePickerFechaInicio.TabStop = false;
            this.radDateTimePickerFechaInicio.Text = "lunes, 14 de septiembre de 2020";
            this.radDateTimePickerFechaInicio.Value = new System.DateTime(2020, 9, 14, 17, 6, 19, 995);
            // 
            // radDateTimePickerFechaFinal
            // 
            this.radDateTimePickerFechaFinal.Location = new System.Drawing.Point(575, 12);
            this.radDateTimePickerFechaFinal.Name = "radDateTimePickerFechaFinal";
            this.radDateTimePickerFechaFinal.Size = new System.Drawing.Size(193, 20);
            this.radDateTimePickerFechaFinal.TabIndex = 4;
            this.radDateTimePickerFechaFinal.TabStop = false;
            this.radDateTimePickerFechaFinal.Text = "lunes, 14 de septiembre de 2020";
            this.radDateTimePickerFechaFinal.Value = new System.DateTime(2020, 9, 14, 17, 6, 19, 995);
            // 
            // rumLabelFechaFinal
            // 
            this.rumLabelFechaFinal.AutoSize = false;
            this.rumLabelFechaFinal.Location = new System.Drawing.Point(475, 12);
            this.rumLabelFechaFinal.Name = "rumLabelFechaFinal";
            this.rumLabelFechaFinal.Size = new System.Drawing.Size(93, 18);
            this.rumLabelFechaFinal.TabIndex = 3;
            this.rumLabelFechaFinal.Text = "Fecha final";
            this.rumLabelFechaFinal.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmMostrarHistorialRecurso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 445);
            this.Controls.Add(this.radDateTimePickerFechaFinal);
            this.Controls.Add(this.rumLabelFechaFinal);
            this.Controls.Add(this.radDateTimePickerFechaInicio);
            this.Controls.Add(this.rumLabelFechaInicio);
            this.Controls.Add(this.radGridViewHistorial);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMostrarHistorialRecurso";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Historial Recurso";
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewHistorial.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewHistorial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaInicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerFechaInicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerFechaFinal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaFinal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView radGridViewHistorial;
        private RumLabel rumLabelFechaInicio;
        private Telerik.WinControls.UI.RadDateTimePicker radDateTimePickerFechaInicio;
        private Telerik.WinControls.UI.RadDateTimePicker radDateTimePickerFechaFinal;
        private RumLabel rumLabelFechaFinal;
    }
}

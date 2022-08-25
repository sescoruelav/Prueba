using RumboSGA.Controles;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class FrmSeleccionOpDate
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
            this.radComboBoxOperarioDesde = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.rumLabelOperarioDesde = new RumboSGA.RumLabel();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumDateTimePickerHasta = new RumboSGA.Controles.RumDateTimePicker();
            this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumLabelHasta = new RumboSGA.RumLabel();
            this.rumLabelDesde = new RumboSGA.RumLabel();
            this.rumDateTimePickerDesde = new RumboSGA.Controles.RumDateTimePicker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxOperarioDesde)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxOperarioDesde.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxOperarioDesde.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelOperarioDesde)).BeginInit();
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
            this.panel1.Controls.Add(this.rumLabelOperarioDesde);
            this.panel1.Controls.Add(this.radComboBoxOperarioDesde);
            this.panel1.Controls.Add(this.rumButtonCancelar);
            this.panel1.Controls.Add(this.rumDateTimePickerHasta);
            this.panel1.Controls.Add(this.rumButtonAceptar);
            this.panel1.Controls.Add(this.rumLabelHasta);
            this.panel1.Controls.Add(this.rumLabelDesde);
            this.panel1.Controls.Add(this.rumDateTimePickerDesde);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(504, 391);
            this.panel1.TabIndex = 0;
            // 
            // radComboBoxOperarioDesde
            // 
            this.radComboBoxOperarioDesde.AutoSizeDropDownHeight = true;
            this.radComboBoxOperarioDesde.AutoSizeDropDownToBestFit = true;
            // 
            // radComboBoxOperarioDesde.NestedRadGridView
            // 
            this.radComboBoxOperarioDesde.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radComboBoxOperarioDesde.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radComboBoxOperarioDesde.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radComboBoxOperarioDesde.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radComboBoxOperarioDesde.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radComboBoxOperarioDesde.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radComboBoxOperarioDesde.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radComboBoxOperarioDesde.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radComboBoxOperarioDesde.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radComboBoxOperarioDesde.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radComboBoxOperarioDesde.EditorControl.Name = "NestedRadGridView";
            this.radComboBoxOperarioDesde.EditorControl.ReadOnly = true;
            this.radComboBoxOperarioDesde.EditorControl.ShowGroupPanel = false;
            this.radComboBoxOperarioDesde.EditorControl.Size = new System.Drawing.Size(280, 150);
            this.radComboBoxOperarioDesde.EditorControl.TabIndex = 0;
            this.radComboBoxOperarioDesde.Location = new System.Drawing.Point(10, 27);
            this.radComboBoxOperarioDesde.Name = "radComboBoxOperarioDesde";
            this.radComboBoxOperarioDesde.Size = new System.Drawing.Size(339, 28);
            this.radComboBoxOperarioDesde.TabIndex = 29;
            this.radComboBoxOperarioDesde.TabStop = false;
            // 
            // rumLabelOperarioDesde
            // 
            this.rumLabelOperarioDesde.AutoSize = false;
            this.rumLabelOperarioDesde.Location = new System.Drawing.Point(10, 3);
            this.rumLabelOperarioDesde.Name = "rumLabelOperarioDesde";
            this.rumLabelOperarioDesde.Size = new System.Drawing.Size(99, 18);
            this.rumLabelOperarioDesde.TabIndex = 30;
            this.rumLabelOperarioDesde.Text = "Operario";
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(214, 341);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonCancelar.TabIndex = 5;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // rumDateTimePickerHasta
            // 
            this.rumDateTimePickerHasta.CalendarSize = new System.Drawing.Size(290, 320);
            this.rumDateTimePickerHasta.Location = new System.Drawing.Point(10, 202);
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
            this.rumButtonAceptar.Location = new System.Drawing.Point(59, 341);
            this.rumButtonAceptar.Name = "rumButtonAceptar";
            this.rumButtonAceptar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonAceptar.TabIndex = 3;
            this.rumButtonAceptar.Text = "Aceptar";
            this.rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            // 
            // rumLabelHasta
            // 
            this.rumLabelHasta.Location = new System.Drawing.Point(10, 176);
            this.rumLabelHasta.Name = "rumLabelHasta";
            this.rumLabelHasta.Size = new System.Drawing.Size(39, 20);
            this.rumLabelHasta.TabIndex = 2;
            this.rumLabelHasta.Text = "Hasta";
            // 
            // rumLabelDesde
            // 
            this.rumLabelDesde.Location = new System.Drawing.Point(10, 119);
            this.rumLabelDesde.Name = "rumLabelDesde";
            this.rumLabelDesde.Size = new System.Drawing.Size(43, 20);
            this.rumLabelDesde.TabIndex = 1;
            this.rumLabelDesde.Text = "Desde";
            // 
            // rumDateTimePickerDesde
            // 
            this.rumDateTimePickerDesde.CalendarSize = new System.Drawing.Size(290, 320);
            this.rumDateTimePickerDesde.Location = new System.Drawing.Point(10, 145);
            this.rumDateTimePickerDesde.Name = "rumDateTimePickerDesde";
            this.rumDateTimePickerDesde.ShowItemToolTips = false;
            this.rumDateTimePickerDesde.Size = new System.Drawing.Size(254, 26);
            this.rumDateTimePickerDesde.TabIndex = 0;
            this.rumDateTimePickerDesde.TabStop = false;
            this.rumDateTimePickerDesde.Text = "martes, 16 de noviembre de 2021";
            this.rumDateTimePickerDesde.Value = new System.DateTime(2021, 11, 16, 13, 4, 17, 224);
            this.rumDateTimePickerDesde.ValueChanged += new System.EventHandler(this.RumDateTimePickerDesde_ValueChanged);
            // 
            // FrmSeleccionOpDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 415);
            this.Controls.Add(this.panel1);
            this.Name = "FrmSeleccionOpDate";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.Text = "Fecha";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxOperarioDesde.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxOperarioDesde.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxOperarioDesde)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelOperarioDesde)).EndInit();
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
        private RumLabel rumLabelOperarioDesde;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radComboBoxOperarioDesde;
    }
}

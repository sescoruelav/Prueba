namespace RumboSGA.Presentation.FormularioRecepciones
{
    partial class FrmAltaDirectaPreaviso
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAltaDirectaPreaviso));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rumLabel2 = new RumboSGA.RumLabel();
            this.rmccmbEstado = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.rmccmbUbicacion = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.rumLabel1 = new RumboSGA.RumLabel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.btnCancelar = new RumboSGA.RumButton();
            this.rCheckBoxConReserva = new Telerik.WinControls.UI.RadCheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbEstado.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbEstado.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rCheckBoxConReserva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rCheckBoxConReserva, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.rumLabel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rmccmbEstado, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.rmccmbUbicacion, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rumLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 220);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rumLabel2
            // 
            this.rumLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rumLabel2.Location = new System.Drawing.Point(3, 65);
            this.rumLabel2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rumLabel2.Name = "rumLabel2";
            this.rumLabel2.Size = new System.Drawing.Size(91, 18);
            this.rumLabel2.TabIndex = 12;
            this.rumLabel2.Text = "Estado Existencia";
            // 
            // rmccmbEstado
            // 
            this.rmccmbEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // rmccmbEstado.NestedRadGridView
            // 
            this.rmccmbEstado.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.rmccmbEstado.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbEstado.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rmccmbEstado.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rmccmbEstado.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.rmccmbEstado.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.rmccmbEstado.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.rmccmbEstado.EditorControl.MasterTemplate.EnableGrouping = false;
            this.rmccmbEstado.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.rmccmbEstado.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rmccmbEstado.EditorControl.Name = "NestedRadGridView";
            this.rmccmbEstado.EditorControl.ReadOnly = true;
            this.rmccmbEstado.EditorControl.ShowGroupPanel = false;
            this.rmccmbEstado.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rmccmbEstado.EditorControl.TabIndex = 0;
            this.rmccmbEstado.Location = new System.Drawing.Point(166, 65);
            this.rmccmbEstado.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rmccmbEstado.Name = "rmccmbEstado";
            this.rmccmbEstado.Size = new System.Drawing.Size(158, 20);
            this.rmccmbEstado.TabIndex = 11;
            this.rmccmbEstado.TabStop = false;
            this.rmccmbEstado.Text = "comboEstado";
            // 
            // rmccmbUbicacion
            // 
            this.rmccmbUbicacion.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // rmccmbUbicacion.NestedRadGridView
            // 
            this.rmccmbUbicacion.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.rmccmbUbicacion.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbUbicacion.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rmccmbUbicacion.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rmccmbUbicacion.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.rmccmbUbicacion.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.rmccmbUbicacion.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.rmccmbUbicacion.EditorControl.MasterTemplate.EnableGrouping = false;
            this.rmccmbUbicacion.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.rmccmbUbicacion.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.rmccmbUbicacion.EditorControl.Name = "NestedRadGridView";
            this.rmccmbUbicacion.EditorControl.ReadOnly = true;
            this.rmccmbUbicacion.EditorControl.ShowGroupPanel = false;
            this.rmccmbUbicacion.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rmccmbUbicacion.EditorControl.TabIndex = 0;
            this.rmccmbUbicacion.Location = new System.Drawing.Point(166, 10);
            this.rmccmbUbicacion.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rmccmbUbicacion.Name = "rmccmbUbicacion";
            this.rmccmbUbicacion.Size = new System.Drawing.Size(158, 20);
            this.rmccmbUbicacion.TabIndex = 10;
            this.rmccmbUbicacion.TabStop = false;
            this.rmccmbUbicacion.Text = "comboUbicacion";
            // 
            // rumLabel1
            // 
            this.rumLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rumLabel1.Location = new System.Drawing.Point(3, 10);
            this.rumLabel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rumLabel1.Name = "rumLabel1";
            this.rumLabel1.Size = new System.Drawing.Size(55, 18);
            this.rumLabel1.TabIndex = 9;
            this.rumLabel1.Text = "Ubicación";
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAceptar.Location = new System.Drawing.Point(3, 168);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(110, 24);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = "Aceptar";
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCancelar.Location = new System.Drawing.Point(166, 168);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 24);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            // 
            // rCheckBoxConReserva
            // 
            this.rCheckBoxConReserva.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rCheckBoxConReserva.Location = new System.Drawing.Point(166, 113);
            this.rCheckBoxConReserva.Name = "rCheckBoxConReserva";
            this.rCheckBoxConReserva.Size = new System.Drawing.Size(104, 18);
            this.rCheckBoxConReserva.TabIndex = 56;
            this.rCheckBoxConReserva.Text = "Con Movimiento";
            this.rCheckBoxConReserva.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // FrmAltaDirectaPreaviso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(327, 220);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAltaDirectaPreaviso";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Alta directa preaviso";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbEstado.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbEstado.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rCheckBoxConReserva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
        private RumLabel rumLabel1;
        private RumLabel rumLabel2;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rmccmbEstado;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rmccmbUbicacion;
        private Telerik.WinControls.UI.RadCheckBox rCheckBoxConReserva;
    }
}

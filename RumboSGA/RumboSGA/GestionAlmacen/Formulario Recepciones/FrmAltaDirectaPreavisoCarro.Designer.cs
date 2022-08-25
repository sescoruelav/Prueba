namespace RumboSGA.Presentation.FormularioRecepciones
{
    partial class FrmAltaDirectaPreavisoCarro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAltaDirectaPreavisoCarro));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rmccmbUbicacion = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.rumLabel1 = new RumboSGA.RumLabel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.btnCancelar = new RumboSGA.RumButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rmccmbUbicacion, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rumLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 69.35484F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.64516F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(308, 124);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.rmccmbUbicacion.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rmccmbUbicacion.EditorControl.Name = "NestedRadGridView";
            this.rmccmbUbicacion.EditorControl.ReadOnly = true;
            this.rmccmbUbicacion.EditorControl.ShowGroupPanel = false;
            this.rmccmbUbicacion.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rmccmbUbicacion.EditorControl.TabIndex = 0;
            this.rmccmbUbicacion.Location = new System.Drawing.Point(157, 10);
            this.rmccmbUbicacion.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rmccmbUbicacion.Name = "rmccmbUbicacion";
            this.rmccmbUbicacion.Size = new System.Drawing.Size(148, 24);
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
            this.rumLabel1.Size = new System.Drawing.Size(103, 20);
            this.rumLabel1.TabIndex = 9;
            this.rumLabel1.Text = "Seleccione Carro";
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAceptar.Location = new System.Drawing.Point(3, 88);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(148, 24);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = "Aceptar";
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancelar.Location = new System.Drawing.Point(157, 88);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(148, 24);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FrmAltaDirectaPreavisoCarro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(308, 124);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAltaDirectaPreavisoCarro";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Alta directa preaviso carro";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbUbicacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
        private RumLabel rumLabel1;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rmccmbUbicacion;
    }
}

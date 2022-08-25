namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    partial class AbonarPackingMuelle
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSalir = new RumboSGA.RumButton();
            this.lblUbicacion = new RumboSGA.RumLabel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.radComboBoxUbicacion = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUbicacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxUbicacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxUbicacion.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxUbicacion.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.radComboBoxUbicacion, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSalir, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblUbicacion, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(493, 101);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnSalir
            // 
            this.btnSalir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSalir.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnSalir.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSalir.Location = new System.Drawing.Point(249, 53);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(241, 45);
            this.btnSalir.TabIndex = 4;
            this.btnSalir.Click += new System.EventHandler(this.salir_Event);
            // 
            // lblUbicacion
            // 
            this.lblUbicacion.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUbicacion.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblUbicacion.Location = new System.Drawing.Point(77, 10);
            this.lblUbicacion.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.lblUbicacion.Name = "lblUbicacion";
            // 
            // 
            // 
            this.lblUbicacion.RootElement.ControlBounds = new System.Drawing.Rectangle(166, 10, 100, 18);
            this.lblUbicacion.Size = new System.Drawing.Size(91, 20);
            this.lblUbicacion.TabIndex = 1;
            this.lblUbicacion.Text = "labelUbicacion";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(3, 53);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(240, 45);
            this.btnAceptar.TabIndex = 3;
            this.btnAceptar.Click += new System.EventHandler(this.aceptar_Event);
            // 
            // radComboBoxUbicacion
            // 
            this.radComboBoxUbicacion.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // radComboBoxUbicacion.NestedRadGridView
            // 
            this.radComboBoxUbicacion.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radComboBoxUbicacion.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radComboBoxUbicacion.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radComboBoxUbicacion.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radComboBoxUbicacion.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radComboBoxUbicacion.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radComboBoxUbicacion.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radComboBoxUbicacion.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radComboBoxUbicacion.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radComboBoxUbicacion.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radComboBoxUbicacion.EditorControl.Name = "NestedRadGridView";
            this.radComboBoxUbicacion.EditorControl.ReadOnly = true;
            this.radComboBoxUbicacion.EditorControl.ShowGroupPanel = false;
            this.radComboBoxUbicacion.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radComboBoxUbicacion.EditorControl.TabIndex = 0;
            this.radComboBoxUbicacion.Location = new System.Drawing.Point(249, 3);
            this.radComboBoxUbicacion.Name = "radComboBoxUbicacion";
            this.radComboBoxUbicacion.Size = new System.Drawing.Size(241, 24);
            this.radComboBoxUbicacion.TabIndex = 5;
            this.radComboBoxUbicacion.TabStop = false;
            // 
            // AbonarPackingMuelle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 101);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AbonarPackingMuelle";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Abonar Packing List";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUbicacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxUbicacion.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxUbicacion.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxUbicacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumLabel lblUbicacion;
        private RumButton btnAceptar;
        private RumButton btnSalir;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radComboBoxUbicacion;
    }
}
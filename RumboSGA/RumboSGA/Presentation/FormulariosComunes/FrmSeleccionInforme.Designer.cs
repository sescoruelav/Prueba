namespace RumboSGA.Presentation.FormulariosComunes
{
    partial class FrmSeleccionInforme
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSeleccionInforme));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radLabel1 = new RumboSGA.RumLabel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.btnCancelar = new RumboSGA.RumButton();
            this.rmccmbInforme = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbInforme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbInforme.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbInforme.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.74617F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.25383F));
            this.tableLayoutPanel1.Controls.Add(this.radLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.rmccmbInforme, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(558, 126);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radLabel1
            // 
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.Location = new System.Drawing.Point(3, 10);
            this.radLabel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(143, 59);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Informe";
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAceptar.Location = new System.Drawing.Point(3, 75);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(143, 48);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(152, 75);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(403, 48);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // rmccmbInforme
            // 
            this.rmccmbInforme.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // rmccmbInforme.NestedRadGridView
            // 
            this.rmccmbInforme.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.rmccmbInforme.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbInforme.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rmccmbInforme.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rmccmbInforme.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.rmccmbInforme.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.rmccmbInforme.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.rmccmbInforme.EditorControl.MasterTemplate.EnableGrouping = false;
            this.rmccmbInforme.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.rmccmbInforme.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rmccmbInforme.EditorControl.Name = "NestedRadGridView";
            this.rmccmbInforme.EditorControl.ReadOnly = true;
            this.rmccmbInforme.EditorControl.ShowGroupPanel = false;
            this.rmccmbInforme.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rmccmbInforme.EditorControl.TabIndex = 0;
            this.rmccmbInforme.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbInforme.Location = new System.Drawing.Point(152, 10);
            this.rmccmbInforme.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rmccmbInforme.Name = "rmccmbInforme";
            this.rmccmbInforme.NullText = "Seleccionar tipo";
            this.rmccmbInforme.Size = new System.Drawing.Size(403, 59);
            this.rmccmbInforme.TabIndex = 8;
            this.rmccmbInforme.TabStop = false;
            this.rmccmbInforme.Text = "Seleccionar informe";
            // 
            // FrmSeleccionInforme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(558, 126);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSeleccionInforme";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Selección Tipo";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbInforme.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbInforme.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbInforme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumLabel radLabel1;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rmccmbInforme;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
    }
}

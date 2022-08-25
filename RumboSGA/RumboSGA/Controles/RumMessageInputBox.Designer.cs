namespace RumboSGA.Controles
{
    partial class RumMessageInputBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RumMessageInputBox));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.btnCancelar = new RumboSGA.RumButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblInput = new RumboSGA.RumLabel();
            this.txtInput = new Telerik.WinControls.UI.RadTextBoxControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPathImagen = new RumboSGA.RumLabel();
            this.dbTableLayoutPanel1 = new RumboSGA.Controles.DBTableLayoutPanel(this.components);
            this.btnImagen = new RumboSGA.RumButton();
            this.rmccmbMotivo = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInput)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblPathImagen)).BeginInit();
            this.dbTableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnImagen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbMotivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbMotivo.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbMotivo.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.85075F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dbTableLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.27341F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.72659F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(603, 267);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnAceptar, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCancelar, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 220);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(597, 44);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(3, 3);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(292, 38);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(301, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(293, 38);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblInput
            // 
            this.lblInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInput.Location = new System.Drawing.Point(3, 3);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(74, 20);
            this.lblInput.TabIndex = 11;
            this.lblInput.Text = "Comentario";
            // 
            // txtInput
            // 
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInput.Location = new System.Drawing.Point(3, 70);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(591, 98);
            this.txtInput.TabIndex = 12;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.rmccmbMotivo, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtInput, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblInput, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(597, 171);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblPathImagen
            // 
            this.lblPathImagen.Location = new System.Drawing.Point(115, 3);
            this.lblPathImagen.Name = "lblPathImagen";
            this.lblPathImagen.Size = new System.Drawing.Size(2, 2);
            this.lblPathImagen.TabIndex = 1;
            // 
            // dbTableLayoutPanel1
            // 
            this.dbTableLayoutPanel1.ColumnCount = 2;
            this.dbTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.80342F));
            this.dbTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.19658F));
            this.dbTableLayoutPanel1.Controls.Add(this.btnImagen, 0, 0);
            this.dbTableLayoutPanel1.Controls.Add(this.lblPathImagen, 1, 0);
            this.dbTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbTableLayoutPanel1.Location = new System.Drawing.Point(3, 180);
            this.dbTableLayoutPanel1.Name = "dbTableLayoutPanel1";
            this.dbTableLayoutPanel1.RowCount = 1;
            this.dbTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dbTableLayoutPanel1.Size = new System.Drawing.Size(597, 34);
            this.dbTableLayoutPanel1.TabIndex = 2;
            // 
            // btnImagen
            // 
            this.btnImagen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImagen.Image = ((System.Drawing.Image)(resources.GetObject("btnImagen.Image")));
            this.btnImagen.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnImagen.Location = new System.Drawing.Point(3, 3);
            this.btnImagen.Name = "btnImagen";
            this.btnImagen.Size = new System.Drawing.Size(106, 28);
            this.btnImagen.TabIndex = 0;
            this.btnImagen.Click += new System.EventHandler(this.btnImagen_Click);
            // 
            // rmccmbMotivo
            // 
            this.rmccmbMotivo.Dock = System.Windows.Forms.DockStyle.Top;
            // 
            // rmccmbMotivo.NestedRadGridView
            // 
            this.rmccmbMotivo.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.rmccmbMotivo.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbMotivo.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rmccmbMotivo.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rmccmbMotivo.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.rmccmbMotivo.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.rmccmbMotivo.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.rmccmbMotivo.EditorControl.MasterTemplate.EnableGrouping = false;
            this.rmccmbMotivo.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.rmccmbMotivo.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.rmccmbMotivo.EditorControl.Name = "NestedRadGridView";
            this.rmccmbMotivo.EditorControl.ReadOnly = true;
            this.rmccmbMotivo.EditorControl.ShowGroupPanel = false;
            this.rmccmbMotivo.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rmccmbMotivo.EditorControl.TabIndex = 0;
            this.rmccmbMotivo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbMotivo.Location = new System.Drawing.Point(3, 35);
            this.rmccmbMotivo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rmccmbMotivo.Name = "rmccmbMotivo";
            this.rmccmbMotivo.NullText = "Seleccionar tipo";
            this.rmccmbMotivo.Size = new System.Drawing.Size(591, 24);
            this.rmccmbMotivo.TabIndex = 13;
            this.rmccmbMotivo.TabStop = false;
            this.rmccmbMotivo.Text = "Seleccionar motivo";
            // 
            // RumMessageInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 267);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RumMessageInputBox";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "RumMessageInputBox";
            this.Load += new System.EventHandler(this.RumMessageInputBox_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInput)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblPathImagen)).EndInit();
            this.dbTableLayoutPanel1.ResumeLayout(false);
            this.dbTableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnImagen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbMotivo.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbMotivo.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbMotivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Telerik.WinControls.UI.RadTextBoxControl txtInput;
        private RumLabel lblInput;
        private DBTableLayoutPanel dbTableLayoutPanel1;
        private RumButton btnImagen;
        private RumLabel lblPathImagen;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rmccmbMotivo;
    }
}

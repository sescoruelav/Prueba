using RumboSGA.Properties;

namespace RumboSGA.Herramientas.Stock
{
    partial class ElegirRecurso
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
            this.btnAceptar = new Telerik.WinControls.UI.RadButton();
            this.btnCancelar = new Telerik.WinControls.UI.RadButton();
            this.labelRecurso = new Telerik.WinControls.UI.RadLabel();
            this.comboBoxRecursos = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRecursos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRecursos.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRecursos.EditorControl.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelRecurso, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxRecursos, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(567, 72);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(3, 50);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(67, 19);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(76, 50);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(67, 19);
            this.btnCancelar.TabIndex = 1;
            // 
            // labelRecurso
            // 
            this.labelRecurso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRecurso.Location = new System.Drawing.Point(97, 25);
            this.labelRecurso.Name = "labelRecurso";
            this.labelRecurso.Size = new System.Drawing.Size(46, 18);
            this.labelRecurso.TabIndex = 5;
            this.labelRecurso.Text = "Recurso";
            // 
            // comboBoxRecursos
            // 
            this.comboBoxRecursos.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // comboBoxRecursos.NestedRadGridView
            // 
            this.comboBoxRecursos.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxRecursos.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxRecursos.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBoxRecursos.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.comboBoxRecursos.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.comboBoxRecursos.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.comboBoxRecursos.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.comboBoxRecursos.EditorControl.MasterTemplate.EnableGrouping = false;
            this.comboBoxRecursos.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.comboBoxRecursos.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.comboBoxRecursos.EditorControl.Name = "NestedRadGridView";
            this.comboBoxRecursos.EditorControl.ReadOnly = true;
            this.comboBoxRecursos.EditorControl.ShowGroupPanel = false;
            this.comboBoxRecursos.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.comboBoxRecursos.EditorControl.TabIndex = 0;
            this.comboBoxRecursos.Location = new System.Drawing.Point(149, 25);
            this.comboBoxRecursos.Name = "comboBoxRecursos";
            this.comboBoxRecursos.Size = new System.Drawing.Size(300, 20);
            this.comboBoxRecursos.TabIndex = 8;
            this.comboBoxRecursos.TabStop = false;
            // 
            // ElegirRecurso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 72);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ElegirRecurso";
            this.Text = "Asignar Recurso";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRecursos.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRecursos.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRecursos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btnAceptar;
        private Telerik.WinControls.UI.RadButton btnCancelar;
        private Telerik.WinControls.UI.RadLabel labelRecurso;
        private Telerik.WinControls.UI.RadMultiColumnComboBox comboBoxRecursos;
    }
}

using RumboSGA.Properties;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class ElegirMuelle
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
            this.rcbMuelle = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.btnAceptar = new Telerik.WinControls.UI.RadButton();
            this.btnCancelar = new Telerik.WinControls.UI.RadButton();
            this.labelMuelle = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rcbMuelle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcbMuelle.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcbMuelle.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelMuelle)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.30502F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.18147F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.24324F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Controls.Add(this.rcbMuelle, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelMuelle, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(502, 74);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rcbMuelle
            // 
            this.rcbMuelle.AutoSizeDropDownHeight = true;
            this.rcbMuelle.AutoSizeDropDownToBestFit = true;
            this.rcbMuelle.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // rcbMuelle.NestedRadGridView
            // 
            this.rcbMuelle.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.rcbMuelle.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rcbMuelle.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rcbMuelle.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rcbMuelle.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.rcbMuelle.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.rcbMuelle.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.rcbMuelle.EditorControl.MasterTemplate.EnableGrouping = false;
            this.rcbMuelle.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.rcbMuelle.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rcbMuelle.EditorControl.Name = "NestedRadGridView";
            this.rcbMuelle.EditorControl.ReadOnly = true;
            this.rcbMuelle.EditorControl.ShowGroupPanel = false;
            this.rcbMuelle.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rcbMuelle.EditorControl.TabIndex = 0;
            this.rcbMuelle.Location = new System.Drawing.Point(186, 17);
            this.rcbMuelle.Name = "rcbMuelle";
            this.rcbMuelle.Size = new System.Drawing.Size(211, 20);
            this.rcbMuelle.TabIndex = 14;
            this.rcbMuelle.TabStop = false;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(3, 53);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(91, 18);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(100, 53);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(80, 18);
            this.btnCancelar.TabIndex = 1;
            // 
            // labelMuelle
            // 
            this.labelMuelle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMuelle.Location = new System.Drawing.Point(140, 17);
            this.labelMuelle.Name = "labelMuelle";
            this.labelMuelle.Size = new System.Drawing.Size(40, 18);
            this.labelMuelle.TabIndex = 5;
            this.labelMuelle.Text = "Muelle";
            // 
            // ElegirMuelle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 74);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ElegirMuelle";
            this.Text = "Seleccion Muelle";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rcbMuelle.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcbMuelle.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcbMuelle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelMuelle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btnAceptar;
        private Telerik.WinControls.UI.RadButton btnCancelar;
        private Telerik.WinControls.UI.RadLabel labelMuelle;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rcbMuelle;
    }
}

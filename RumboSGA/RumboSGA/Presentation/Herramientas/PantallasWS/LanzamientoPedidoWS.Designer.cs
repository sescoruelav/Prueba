namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class LanzamientoPedidoWS
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAceptar = new Telerik.WinControls.UI.RadButton();
            this.btnCancelar = new Telerik.WinControls.UI.RadButton();
            this.labelMuelle = new Telerik.WinControls.UI.RadLabel();
            this.muelleList = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.recursoList = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.recursoLabel = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelMuelle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.muelleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recursoList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recursoLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.29623F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.438061F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelMuelle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.muelleList, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.recursoList, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.recursoLabel, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.615385F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.88461F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.46154F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.88461F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 136);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(3, 99);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(57, 34);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(66, 99);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(57, 34);
            this.btnCancelar.TabIndex = 1;
            // 
            // labelMuelle
            // 
            this.labelMuelle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMuelle.Location = new System.Drawing.Point(83, 16);
            this.labelMuelle.Name = "labelMuelle";
            this.labelMuelle.Size = new System.Drawing.Size(40, 18);
            this.labelMuelle.TabIndex = 5;
            this.labelMuelle.Text = "Muelle";
            // 
            // muelleList
            // 
            this.muelleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.muelleList.Location = new System.Drawing.Point(129, 16);
            this.muelleList.Name = "muelleList";
            this.muelleList.Size = new System.Drawing.Size(262, 20);
            this.muelleList.TabIndex = 6;
            // 
            // recursoList
            // 
            this.recursoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recursoList.Location = new System.Drawing.Point(129, 54);
            this.recursoList.Name = "recursoList";
            this.recursoList.Size = new System.Drawing.Size(262, 20);
            this.recursoList.TabIndex = 8;
            // 
            // recursoLabel
            // 
            this.recursoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recursoLabel.Location = new System.Drawing.Point(77, 54);
            this.recursoLabel.Name = "recursoLabel";
            this.recursoLabel.Size = new System.Drawing.Size(46, 18);
            this.recursoLabel.TabIndex = 7;
            this.recursoLabel.Text = "Recurso";
            // 
            // LanzamientoPedidoWS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 136);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(500, 175);
            this.MinimumSize = new System.Drawing.Size(500, 175);
            this.Name = "LanzamientoPedidoWS";
            this.Text = "";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelMuelle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.muelleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recursoList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recursoLabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btnAceptar;
        private Telerik.WinControls.UI.RadButton btnCancelar;
        private Telerik.WinControls.UI.RadLabel labelMuelle;
        private Telerik.WinControls.UI.RadLabel recursoLabel;
        private Telerik.WinControls.UI.RadMultiColumnComboBox muelleList;
        private Telerik.WinControls.UI.RadMultiColumnComboBox recursoList;
    }
}

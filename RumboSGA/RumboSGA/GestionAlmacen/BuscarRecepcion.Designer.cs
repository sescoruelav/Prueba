namespace RumboSGA.GestionAlmacen
{
    partial class BuscarRecepcion
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
            this.btnIniciarFiltro = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.comboFiltros = new Telerik.WinControls.UI.RadDropDownList();
            this.lblFiltro = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnIniciarFiltro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFiltros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFiltro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.06949F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.97281F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.73716F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.06949F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnIniciarFiltro, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.radGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboFiltros, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFiltro, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.743738F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.25626F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(662, 519);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnIniciarFiltro
            // 
            this.btnIniciarFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIniciarFiltro.Location = new System.Drawing.Point(567, 3);
            this.btnIniciarFiltro.Name = "btnIniciarFiltro";
            this.btnIniciarFiltro.Size = new System.Drawing.Size(92, 24);
            this.btnIniciarFiltro.TabIndex = 6;
            this.btnIniciarFiltro.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // radGridView1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.radGridView1, 7);
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(3, 38);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(656, 478);
            this.radGridView1.TabIndex = 7;
            // 
            // comboFiltros
            // 
            this.comboFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboFiltros.Location = new System.Drawing.Point(116, 3);
            this.comboFiltros.Name = "comboFiltros";
            this.comboFiltros.Size = new System.Drawing.Size(252, 20);
            this.comboFiltros.TabIndex = 8;
            this.comboFiltros.Text = "radDropDownList1";
            // 
            // lblFiltro
            // 
            this.lblFiltro.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFiltro.Location = new System.Drawing.Point(108, 3);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(2, 2);
            this.lblFiltro.TabIndex = 0;
            this.lblFiltro.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // BuscarRecepcion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 519);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BuscarRecepcion";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "BuscarArticulo";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnIniciarFiltro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFiltros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFiltro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel lblFiltro;
        private Telerik.WinControls.UI.RadButton btnIniciarFiltro;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadDropDownList comboFiltros;
    }
}

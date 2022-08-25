namespace RumboSGA.Presentation.Herramientas.Stock
{
    partial class FrmImportacionExportacion
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
            Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook1 = new Telerik.Windows.Documents.Spreadsheet.Model.Workbook();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rsSheetStock = new Telerik.WinControls.UI.RadSpreadsheet();
            this.rsSheetRibbonBar = new Telerik.WinControls.UI.RadSpreadsheetRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.rrBarGroupComparar = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rBtnComXSSCC = new RumboSGA.RumButtonElement();
            this.rBtnComXId = new RumboSGA.RumButtonElement();
            this.rBtnCompStockStatus = new RumboSGA.RumButtonElement();
            this.rrBarGroupExportar = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rbtnExportERP = new RumboSGA.RumButtonElement();
            this.rrBarGroupImportar = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rBtnCargaStockStatus = new RumboSGA.RumButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rsSheetStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsSheetRibbonBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 722);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(1110, 26);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rsSheetStock);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 168);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1110, 554);
            this.panel1.TabIndex = 2;
            // 
            // rsSheetStock
            // 
            this.rsSheetStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rsSheetStock.Location = new System.Drawing.Point(0, 0);
            this.rsSheetStock.Name = "rsSheetStock";
            this.rsSheetStock.Size = new System.Drawing.Size(1110, 554);
            this.rsSheetStock.TabIndex = 1;
            workbook1.ActiveTabIndex = -1;
            workbook1.Name = "Book1";
            workbook1.WorkbookContentChangedInterval = System.TimeSpan.Parse("00:00:00.0300000");
            this.rsSheetStock.Workbook = workbook1;
            // 
            // rsSheetRibbonBar
            // 
            this.rsSheetRibbonBar.ApplicationMenuStyle = Telerik.WinControls.UI.ApplicationMenuStyle.BackstageView;
            this.rsSheetRibbonBar.AssociatedSpreadsheet = this.rsSheetStock;
            this.rsSheetRibbonBar.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            // 
            // 
            // 
            this.rsSheetRibbonBar.ExitButton.Text = "Exit";
            this.rsSheetRibbonBar.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.rsSheetRibbonBar.Location = new System.Drawing.Point(0, 0);
            this.rsSheetRibbonBar.Name = "rsSheetRibbonBar";
            // 
            // 
            // 
            this.rsSheetRibbonBar.OptionsButton.Text = "Options";
            this.rsSheetRibbonBar.ShowLayoutModeButton = true;
            this.rsSheetRibbonBar.Size = new System.Drawing.Size(1110, 168);
            this.rsSheetRibbonBar.TabIndex = 0;
            this.rsSheetRibbonBar.Text = "Importacion/ Exportacion Stock";
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rrBarGroupComparar,
            this.rrBarGroupExportar,
            this.rrBarGroupImportar});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "SGA/WMS";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // rrBarGroupComparar
            // 
            this.rrBarGroupComparar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnComXSSCC,
            this.rBtnComXId,
            this.rBtnCompStockStatus});
            this.rrBarGroupComparar.Name = "rrBarGroupComparar";
            this.rrBarGroupComparar.Text = "Comparar";
            // 
            // rBtnComXSSCC
            // 
            this.rBtnComXSSCC.Image = global::RumboSGA.Properties.Resources.SSCC3;
            this.rBtnComXSSCC.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnComXSSCC.Name = "rBtnComXSSCC";
            this.rBtnComXSSCC.Text = "Stock SSCC";
            this.rBtnComXSSCC.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnComXSSCC.UseCompatibleTextRendering = false;
            this.rBtnComXSSCC.Click += new System.EventHandler(this.rBtnComXSSCC_Click);
            // 
            // rBtnComXId
            // 
            this.rBtnComXId.Image = global::RumboSGA.Properties.Resources.codigobar;
            this.rBtnComXId.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnComXId.Name = "rBtnComXId";
            this.rBtnComXId.Text = "stock rumboID";
            this.rBtnComXId.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnComXId.UseCompatibleTextRendering = false;
            this.rBtnComXId.Click += new System.EventHandler(this.rBtnComXId_Click);
            // 
            // rBtnCompStockStatus
            // 
            this.rBtnCompStockStatus.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.rBtnCompStockStatus.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCompStockStatus.Name = "rBtnCompStockStatus";
            this.rBtnCompStockStatus.Text = "stock Status";
            this.rBtnCompStockStatus.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCompStockStatus.UseCompatibleTextRendering = false;
            this.rBtnCompStockStatus.Click += new System.EventHandler(this.rBtnCompStockStatus_Click);
            // 
            // rrBarGroupExportar
            // 
            this.rrBarGroupExportar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rbtnExportERP});
            this.rrBarGroupExportar.Name = "rrBarGroupExportar";
            this.rrBarGroupExportar.Text = "Exportar";
            // 
            // rbtnExportERP
            // 
            this.rbtnExportERP.Image = global::RumboSGA.Properties.Resources.Export;
            this.rbtnExportERP.Name = "rbtnExportERP";
            this.rbtnExportERP.Text = "ERP";
            this.rbtnExportERP.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnExportERP.UseCompatibleTextRendering = false;
            this.rbtnExportERP.Click += new System.EventHandler(this.rbtnExportERP_Click);
            // 
            // rrBarGroupImportar
            // 
            this.rrBarGroupImportar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnCargaStockStatus});
            this.rrBarGroupImportar.Name = "rrBarGroupImportar";
            this.rrBarGroupImportar.Text = "Importar";
            // 
            // rBtnCargaStockStatus
            // 
            this.rBtnCargaStockStatus.Image = global::RumboSGA.Properties.Resources.Table;
            this.rBtnCargaStockStatus.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCargaStockStatus.Name = "rBtnCargaStockStatus";
            this.rBtnCargaStockStatus.Text = "Stock Status";
            this.rBtnCargaStockStatus.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCargaStockStatus.UseCompatibleTextRendering = false;
            this.rBtnCargaStockStatus.Click += new System.EventHandler(this.rBtnCargaStockStatus_Click);
            // 
            // FrmImportacionExportacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 748);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.rsSheetRibbonBar);
            this.MainMenuStrip = null;
            this.Name = "FrmImportacionExportacion";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Importacion/ Exportacion Stock";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rsSheetStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rsSheetRibbonBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadSpreadsheet rsSheetStock;
        private Telerik.WinControls.UI.RadSpreadsheetRibbonBar rsSheetRibbonBar;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private Telerik.WinControls.UI.RadRibbonBarGroup rrBarGroupComparar;
        private Telerik.WinControls.UI.RadRibbonBarGroup rrBarGroupExportar;
        private Telerik.WinControls.UI.RadRibbonBarGroup rrBarGroupImportar;
        private RumButtonElement rBtnComXSSCC;                        
        private RumButtonElement rBtnComXId;
        private RumButtonElement rBtnCompStockStatus;
        private RumButtonElement rbtnExportERP;
        private RumButtonElement rBtnCargaStockStatus;
    }
}

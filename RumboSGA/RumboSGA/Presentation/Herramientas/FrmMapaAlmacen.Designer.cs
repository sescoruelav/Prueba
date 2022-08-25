namespace RumboSGA.Presentation.Herramientas
{
    partial class FrmMapaAlmacen
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
            Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook2 = new Telerik.Windows.Documents.Spreadsheet.Model.Workbook();
            Telerik.Windows.Documents.Model.DocumentInfo documentInfo2 = new Telerik.Windows.Documents.Model.DocumentInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMapaAlmacen));
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.radSSheetHojaCalculo = new Telerik.WinControls.UI.RadSpreadsheet();
            this.radProgressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            this.radSSheetRBBarraOpciones = new Telerik.WinControls.UI.RadSpreadsheetRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnNuevaPlantilla = new RumboSGA.RumButtonElement();
            this.btnLlenado = new RumboSGA.RumButtonElement();
            this.btnReferenciaPicking = new RumboSGA.RumButtonElement();
            this.btnInfoCelda = new RumboSGA.RumButtonElement();
            this.btnFrecuenciaAcceso = new RumboSGA.RumButtonElement();
            this.btnZonas = new RumboSGA.RumButtonElement();
            this.btnUbicacion = new Telerik.WinControls.UI.RadButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSSheetHojaCalculo)).BeginInit();
            this.radSSheetHojaCalculo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSSheetRBBarraOpciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 522);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(1824, 24);
            this.radStatusStrip1.TabIndex = 0;
            // 
            // radSSheetHojaCalculo
            // 
            this.radSSheetHojaCalculo.Controls.Add(this.radProgressBar1);
            this.radSSheetHojaCalculo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radSSheetHojaCalculo.Location = new System.Drawing.Point(0, 173);
            this.radSSheetHojaCalculo.Name = "radSSheetHojaCalculo";
            this.radSSheetHojaCalculo.Size = new System.Drawing.Size(1824, 349);
            this.radSSheetHojaCalculo.TabIndex = 1;
            workbook2.ActiveTabIndex = -1;
            documentInfo2.Author = null;
            documentInfo2.Description = null;
            documentInfo2.Keywords = null;
            documentInfo2.Subject = null;
            documentInfo2.Title = null;
            workbook2.DocumentInfo = documentInfo2;
            workbook2.Name = "Book1";
            workbook2.WorkbookContentChangedInterval = System.TimeSpan.Parse("00:00:03.0300000");
            this.radSSheetHojaCalculo.Workbook = workbook2;
            // 
            // radProgressBar1
            // 
            this.radProgressBar1.AccessibleDescription = "progreso";
            this.radProgressBar1.Location = new System.Drawing.Point(551, 137);
            this.radProgressBar1.Name = "radProgressBar1";
            this.radProgressBar1.SeparatorColor2 = System.Drawing.Color.DarkRed;
            this.radProgressBar1.Size = new System.Drawing.Size(437, 64);
            this.radProgressBar1.Step = 1;
            this.radProgressBar1.TabIndex = 2;
            this.radProgressBar1.TabStop = false;
            this.radProgressBar1.Text = ">>>";
            this.radProgressBar1.Value1 = 50;
            // 
            // radSSheetRBBarraOpciones
            // 
            this.radSSheetRBBarraOpciones.ApplicationMenuStyle = Telerik.WinControls.UI.ApplicationMenuStyle.BackstageView;
            this.radSSheetRBBarraOpciones.AssociatedSpreadsheet = this.radSSheetHojaCalculo;
            this.radSSheetRBBarraOpciones.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            // 
            // 
            // 
            this.radSSheetRBBarraOpciones.ExitButton.Text = "Exit";
            this.radSSheetRBBarraOpciones.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.radSSheetRBBarraOpciones.Location = new System.Drawing.Point(0, 0);
            this.radSSheetRBBarraOpciones.Name = "radSSheetRBBarraOpciones";
            // 
            // 
            // 
            this.radSSheetRBBarraOpciones.OptionsButton.Text = "Options";
            this.radSSheetRBBarraOpciones.ShowLayoutModeButton = true;
            this.radSSheetRBBarraOpciones.SimplifiedHeight = 100;
            this.radSSheetRBBarraOpciones.Size = new System.Drawing.Size(1824, 173);
            this.radSSheetRBBarraOpciones.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radSSheetRBBarraOpciones.StartButtonImage")));
            this.radSSheetRBBarraOpciones.TabIndex = 0;
            this.radSSheetRBBarraOpciones.Text = "Mapa Almacén";
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup1});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "SGA/WMS";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnNuevaPlantilla,
            this.btnLlenado,
            this.btnReferenciaPicking,
            this.btnInfoCelda,
            this.btnFrecuenciaAcceso,
            this.btnZonas,
            this.btnUbicacion});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Acciones";
            // 
            // btnNuevaPlantilla
            // 
            this.btnNuevaPlantilla.Image = global::RumboSGA.Properties.Resources.varita_magica1;
            this.btnNuevaPlantilla.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNuevaPlantilla.Name = "btnNuevaPlantilla";
            this.btnNuevaPlantilla.Text = "Nueva plantilla";
            this.btnNuevaPlantilla.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnNuevaPlantilla.ToolTipText = "Nueva plantilla";
            this.btnNuevaPlantilla.Click += new System.EventHandler(this.btnNuevaPlantilla_Click);
            // 
            // btnLlenado
            // 
            this.btnLlenado.Image = global::RumboSGA.Properties.Resources.percentage_icon_icons_com_53125__1_;
            this.btnLlenado.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLlenado.Name = "btnLlenado";
            this.btnLlenado.Text = "% LLenado";
            this.btnLlenado.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLlenado.ToolTipText = "% LLenado";
            this.btnLlenado.Click += new System.EventHandler(this.btnLlenado_Click);
            // 
            // btnReferenciaPicking
            // 
            this.btnReferenciaPicking.AccessibleDescription = "Referencia Picking ";
            this.btnReferenciaPicking.AccessibleName = "referencia picking ";
            this.btnReferenciaPicking.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnReferenciaPicking.Image = global::RumboSGA.Properties.Resources.codigobar;
            this.btnReferenciaPicking.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnReferenciaPicking.Name = "btnReferenciaPicking";
            this.btnReferenciaPicking.Text = "Referencia Picking";
            this.btnReferenciaPicking.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnReferenciaPicking.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnReferenciaPicking.ToolTipText = "Referencia Picking";
            this.btnReferenciaPicking.Click += new System.EventHandler(this.btnReferenciaPicking_Click);
            // 
            // btnInfoCelda
            // 
            this.btnInfoCelda.AccessibleDescription = "Info celda";
            this.btnInfoCelda.AccessibleName = "Info celda";
            this.btnInfoCelda.Image = global::RumboSGA.Properties.Resources.Overdue;
            this.btnInfoCelda.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnInfoCelda.Name = "btnInfoCelda";
            this.btnInfoCelda.Text = "Info Celda";
            this.btnInfoCelda.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnInfoCelda.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnInfoCelda.ToolTipText = "Info Celda";
            this.btnInfoCelda.Click += new System.EventHandler(this.btnInfoCelda_Click);
            // 
            // btnFrecuenciaAcceso
            // 
            this.btnFrecuenciaAcceso.Image = global::RumboSGA.Properties.Resources.Close1;
            this.btnFrecuenciaAcceso.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFrecuenciaAcceso.Name = "btnFrecuenciaAcceso";
            this.btnFrecuenciaAcceso.Text = "Frecuencia Acceso";
            this.btnFrecuenciaAcceso.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFrecuenciaAcceso.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnFrecuenciaAcceso.ToolTipText = "Frecuencia Acceso";
            this.btnFrecuenciaAcceso.Click += new System.EventHandler(this.btnFrecuenciaAcceso_Click);
            // 
            // btnZonas
            // 
            this.btnZonas.Image = global::RumboSGA.Properties.Resources.colorButton;
            this.btnZonas.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnZonas.Name = "btnZonas";
            this.btnZonas.Text = "Zonas";
            this.btnZonas.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnZonas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnZonas.ToolTipText = "Zonas";
            this.btnZonas.Click += new System.EventHandler(this.btnZonas_Click);
            // 
            // btnUbicacion
            // 
            this.btnUbicacion.Image = global::RumboSGA.Properties.Resources.Form;
            this.btnUbicacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUbicacion.Name = "btnUbicacion";
            this.btnUbicacion.Text = "Form Ubicacion";
            this.btnUbicacion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUbicacion.Click += new System.EventHandler(this.btnUbicacion_Click);
            // 
            // FrmMapaAlmacen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1824, 546);
            this.Controls.Add(this.radSSheetHojaCalculo);
            this.Controls.Add(this.radSSheetRBBarraOpciones);
            this.Controls.Add(this.radStatusStrip1);
            this.MainMenuStrip = null;
            this.Name = "FrmMapaAlmacen";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Mapa Almacén";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSSheetHojaCalculo)).EndInit();
            this.radSSheetHojaCalculo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSSheetRBBarraOpciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private Telerik.WinControls.UI.RadSpreadsheetRibbonBar radSSheetRBBarraOpciones;
        private Telerik.WinControls.UI.RadSpreadsheet radSSheetHojaCalculo;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement btnNuevaPlantilla;
        private RumButtonElement btnLlenado;
        private Telerik.WinControls.UI.RadProgressBar radProgressBar1;
        private RumButtonElement btnReferenciaPicking;
        private RumButtonElement btnInfoCelda;
        private RumButtonElement btnFrecuenciaAcceso;
        private RumButtonElement btnZonas;
        private Telerik.WinControls.UI.RadButtonElement btnUbicacion;
    }
}

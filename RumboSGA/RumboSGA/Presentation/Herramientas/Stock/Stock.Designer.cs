using RumboSGA.Controles;
using RumboSGA.Properties;
using System.Windows.Forms;

namespace RumboSGA.Presentation.UserControls.Mantenimientos.Herramientas
{
    partial class Stock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stock));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.virtualGrid = new Telerik.WinControls.UI.RadVirtualGrid();
            this.radProgressBar1 = new Telerik.WinControls.UI.RadProgressBar();
            this.contenedor = new System.Windows.Forms.Panel();
            this.gridView = new RumboSGA.Controles.RumGridView();
            this.object_aa623162_f8de_48e1_9845_7cfd24651cf3 = new Telerik.WinControls.RootRadElement();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.grupoVer = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnStockMin = new RumboSGA.RumButtonElement();
            this.btnHistArticulo = new RumboSGA.RumButtonElement();
            this.btnAgruparStock = new RumboSGA.Controles.RumDropDownButtonElement();
            this.stockXArticuloItem = new RumboSGA.Controles.RumMenuItem();
            this.stockXEstadoItem = new RumboSGA.Controles.RumMenuItem();
            this.stockXUbicacionItem = new RumboSGA.Controles.RumMenuItem();
            this.stockXFamiliaItem = new RumboSGA.Controles.RumMenuItem();
            this.reiniciarItem = new RumboSGA.Controles.RumMenuItem();
            this.rBtnCargarInstalacion = new RumboSGA.RumButtonElement();
            this.grupoTabla = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnCambiarVista = new RumboSGA.RumButtonElement();
            this.btnExportar = new RumboSGA.RumButtonElement();
            this.btnFiltrar = new RumboSGA.RumButtonElement();
            this.btnLimpiarFiltros = new RumboSGA.RumButtonElement();
            this.btnRefrescar = new RumboSGA.RumButtonElement();
            this.grupoConf = new RumboSGA.Controles.RumRibbonBarGroup();
            this.radDropDownButtonElement1 = new RumboSGA.Controles.RumDropDownButtonElement();
            this.itemGuardarEstilo = new RumboSGA.Controles.RumMenuItem();
            this.itemCargarEstilo = new RumboSGA.Controles.RumMenuItem();
            this.itemColumnas = new RumboSGA.Controles.RumMenuItem();
            this.headerTamLetra = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuItemTamLetra = new Telerik.WinControls.UI.RadMenuComboItem();
            this.pagHeader = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuComboItem = new Telerik.WinControls.UI.RadMenuComboItem();
            this.grupoAcciones = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnCambiarEstadoExistencia = new RumboSGA.RumButtonElement();
            this.rbtnAltaPaletPicosRegularizacion = new RumboSGA.RumButtonElement();
            this.rbtnAltaPaletRegularizacion = new RumboSGA.RumButtonElement();
            this.rbtnAltaPaletMultiRegularizacion = new RumboSGA.RumButtonElement();
            this.rbtnBajaRegularizacion = new RumboSGA.RumButtonElement();
            this.rBtnImpresionSSCC = new RumboSGA.RumButtonElement();
            this.groupExportacion = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnImpExp = new RumboSGA.RumButtonElement();
            this.rBtnExpExccel = new RumboSGA.RumButtonElement();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).BeginInit();
            this.virtualGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).BeginInit();
            this.contenedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.32117F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.53011F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.14872F));
            this.tableLayoutPanel1.Controls.Add(this.virtualGrid, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(998, 385);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // virtualGrid
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.virtualGrid, 4);
            this.virtualGrid.Controls.Add(this.radProgressBar1);
            this.virtualGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualGrid.Location = new System.Drawing.Point(3, 3);
            this.virtualGrid.MasterViewInfo.IsWaiting = true;
            this.virtualGrid.Name = "virtualGrid";
            this.virtualGrid.Size = new System.Drawing.Size(992, 379);
            this.virtualGrid.TabIndex = 16;
            // 
            // radProgressBar1
            // 
            this.radProgressBar1.Location = new System.Drawing.Point(369, 92);
            this.radProgressBar1.Name = "radProgressBar1";
            this.radProgressBar1.Size = new System.Drawing.Size(130, 24);
            this.radProgressBar1.TabIndex = 0;
            this.radProgressBar1.Text = "radProgressBar1";
            this.radProgressBar1.Visible = false;
            // 
            // contenedor
            // 
            this.contenedor.AutoSize = true;
            this.contenedor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.contenedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contenedor.Controls.Add(this.tableLayoutPanel1);
            this.contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contenedor.Location = new System.Drawing.Point(0, 159);
            this.contenedor.Name = "contenedor";
            this.contenedor.Size = new System.Drawing.Size(1000, 387);
            this.contenedor.TabIndex = 0;
            // 
            // gridView
            // 
            this.gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView.Location = new System.Drawing.Point(4, 37);
            // 
            // 
            // 
            this.gridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridView.Name = "gridView";
            this.gridView.Size = new System.Drawing.Size(947, 640);
            this.gridView.TabIndex = 0;
            // 
            // object_aa623162_f8de_48e1_9845_7cfd24651cf3
            // 
            this.object_aa623162_f8de_48e1_9845_7cfd24651cf3.Name = "object_aa623162_f8de_48e1_9845_7cfd24651cf3";
            this.object_aa623162_f8de_48e1_9845_7cfd24651cf3.StretchHorizontally = true;
            this.object_aa623162_f8de_48e1_9845_7cfd24651cf3.StretchVertically = true;
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            // 
            // 
            // 
            this.radRibbonBar1.ExitButton.Text = "Exit";
            this.radRibbonBar1.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.OptionsButton.Text = "Options";
            this.radRibbonBar1.SimplifiedHeight = 100;
            this.radRibbonBar1.Size = new System.Drawing.Size(1000, 159);
            this.radRibbonBar1.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radRibbonBar1.StartButtonImage")));
            this.radRibbonBar1.TabIndex = 4;
            this.radRibbonBar1.Click += new System.EventHandler(this.radRibbonBar1_Click);
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.grupoVer,
            this.grupoTabla,
            this.grupoConf,
            this.grupoAcciones,
            this.groupExportacion});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // grupoVer
            // 
            this.grupoVer.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnStockMin,
            this.btnHistArticulo,
            this.btnAgruparStock,
            this.rBtnCargarInstalacion});
            this.grupoVer.Name = "grupoVer";
            this.grupoVer.Text = "";
            // 
            // btnStockMin
            // 
            this.btnStockMin.Image = global::RumboSGA.Properties.Resources.Form;
            this.btnStockMin.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStockMin.Name = "btnStockMin";
            this.btnStockMin.Text = "";
            this.btnStockMin.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnHistArticulo
            // 
            this.btnHistArticulo.Image = global::RumboSGA.Properties.Resources.History;
            this.btnHistArticulo.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnHistArticulo.Name = "btnHistArticulo";
            this.btnHistArticulo.Text = "";
            this.btnHistArticulo.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnAgruparStock
            // 
            this.btnAgruparStock.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.btnAgruparStock.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.btnAgruparStock.ExpandArrowButton = false;
            this.btnAgruparStock.Image = global::RumboSGA.Properties.Resources.Table;
            this.btnAgruparStock.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAgruparStock.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.stockXArticuloItem,
            this.stockXEstadoItem,
            this.stockXUbicacionItem,
            this.stockXFamiliaItem,
            this.reiniciarItem});
            this.btnAgruparStock.Name = "btnAgruparStock";
            this.btnAgruparStock.Text = "";
            this.btnAgruparStock.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // stockXArticuloItem
            // 
            this.stockXArticuloItem.Name = "stockXArticuloItem";
            this.stockXArticuloItem.Text = "radMenuItem1";
            // 
            // stockXEstadoItem
            // 
            this.stockXEstadoItem.Name = "stockXEstadoItem";
            this.stockXEstadoItem.Text = "radMenuItem2";
            this.stockXEstadoItem.UseCompatibleTextRendering = false;
            // 
            // stockXUbicacionItem
            // 
            this.stockXUbicacionItem.Name = "stockXUbicacionItem";
            this.stockXUbicacionItem.Text = "radMenuItem1";
            this.stockXUbicacionItem.UseCompatibleTextRendering = false;
            // 
            // stockXFamiliaItem
            // 
            this.stockXFamiliaItem.Name = "stockXFamiliaItem";
            this.stockXFamiliaItem.Text = "radMenuItem3";
            this.stockXFamiliaItem.UseCompatibleTextRendering = false;
            // 
            // reiniciarItem
            // 
            this.reiniciarItem.Name = "reiniciarItem";
            this.reiniciarItem.Text = "radMenuItem1";
            // 
            // rBtnCargarInstalacion
            // 
            this.rBtnCargarInstalacion.Enabled = false;
            this.rBtnCargarInstalacion.Image = global::RumboSGA.Properties.Resources.Add1;
            this.rBtnCargarInstalacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCargarInstalacion.Name = "rBtnCargarInstalacion";
            this.rBtnCargarInstalacion.Text = "Instalación";
            this.rBtnCargarInstalacion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCargarInstalacion.ToolTipText = "Instalación";
            // 
            // grupoTabla
            // 
            this.grupoTabla.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnCambiarVista,
            this.btnExportar,
            this.btnFiltrar,
            this.btnLimpiarFiltros,
            this.btnRefrescar});
            this.grupoTabla.Name = "grupoTabla";
            this.grupoTabla.Text = "Tabla";
            // 
            // btnCambiarVista
            // 
            this.btnCambiarVista.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.btnCambiarVista.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCambiarVista.Name = "btnCambiarVista";
            this.btnCambiarVista.Text = "";
            this.btnCambiarVista.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnExportar
            // 
            this.btnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.btnExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Text = "";
            this.btnExportar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Image = global::RumboSGA.Properties.Resources.Filter;
            this.btnFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Text = "";
            this.btnFiltrar.Click += new System.EventHandler(this.BtnFiltrar_Click);
            // 
            // btnLimpiarFiltros
            // 
            this.btnLimpiarFiltros.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnLimpiarFiltros.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            this.btnLimpiarFiltros.Text = "";
            this.btnLimpiarFiltros.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Text = "";
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // grupoConf
            // 
            this.grupoConf.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radDropDownButtonElement1});
            this.grupoConf.Name = "grupoConf";
            this.grupoConf.Text = "Configuracion";
            // 
            // radDropDownButtonElement1
            // 
            this.radDropDownButtonElement1.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.radDropDownButtonElement1.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.radDropDownButtonElement1.ExpandArrowButton = false;
            this.radDropDownButtonElement1.Image = global::RumboSGA.Properties.Resources.Administration;
            this.radDropDownButtonElement1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radDropDownButtonElement1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.itemGuardarEstilo,
            this.itemCargarEstilo,
            this.itemColumnas,
            this.headerTamLetra,
            this.menuItemTamLetra,
            this.pagHeader,
            this.menuComboItem});
            this.radDropDownButtonElement1.Name = "radDropDownButtonElement1";
            this.radDropDownButtonElement1.Text = "";
            this.radDropDownButtonElement1.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // itemGuardarEstilo
            // 
            this.itemGuardarEstilo.Name = "itemGuardarEstilo";
            this.itemGuardarEstilo.Text = "radMenuItem1";
            // 
            // itemCargarEstilo
            // 
            this.itemCargarEstilo.Name = "itemCargarEstilo";
            this.itemCargarEstilo.Text = "radMenuItem2";
            // 
            // itemColumnas
            // 
            this.itemColumnas.Name = "itemColumnas";
            this.itemColumnas.Text = "radMenuItem3";
            // 
            // headerTamLetra
            // 
            this.headerTamLetra.Name = "headerTamLetra";
            this.headerTamLetra.Text = "Tamaño Fuente Tabla";
            // 
            // menuItemTamLetra
            // 
            // 
            // 
            // 
            this.menuItemTamLetra.ComboBoxElement.AutoCompleteAppend = null;
            this.menuItemTamLetra.ComboBoxElement.AutoCompleteDataSource = null;
            this.menuItemTamLetra.ComboBoxElement.AutoCompleteSuggest = null;
            this.menuItemTamLetra.ComboBoxElement.DataMember = "";
            this.menuItemTamLetra.ComboBoxElement.DataSource = null;
            this.menuItemTamLetra.ComboBoxElement.DefaultValue = null;
            this.menuItemTamLetra.ComboBoxElement.DisplayMember = "";
            this.menuItemTamLetra.ComboBoxElement.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            this.menuItemTamLetra.ComboBoxElement.DropDownAnimationEnabled = true;
            this.menuItemTamLetra.ComboBoxElement.EditableElementText = "";
            this.menuItemTamLetra.ComboBoxElement.EditorElement = this.menuItemTamLetra.ComboBoxElement;
            this.menuItemTamLetra.ComboBoxElement.EditorManager = null;
            this.menuItemTamLetra.ComboBoxElement.Filter = null;
            this.menuItemTamLetra.ComboBoxElement.FilterExpression = "";
            this.menuItemTamLetra.ComboBoxElement.Focusable = true;
            this.menuItemTamLetra.ComboBoxElement.FormatString = "";
            this.menuItemTamLetra.ComboBoxElement.FormattingEnabled = true;
            this.menuItemTamLetra.ComboBoxElement.MaxDropDownItems = 0;
            this.menuItemTamLetra.ComboBoxElement.MaxLength = 32767;
            this.menuItemTamLetra.ComboBoxElement.MaxValue = null;
            this.menuItemTamLetra.ComboBoxElement.MinValue = null;
            this.menuItemTamLetra.ComboBoxElement.NullValue = null;
            this.menuItemTamLetra.ComboBoxElement.OwnerOffset = 0;
            this.menuItemTamLetra.ComboBoxElement.ShowImageInEditorArea = true;
            this.menuItemTamLetra.ComboBoxElement.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            this.menuItemTamLetra.ComboBoxElement.Value = null;
            this.menuItemTamLetra.ComboBoxElement.ValueMember = "";
            this.menuItemTamLetra.Name = "menuItemTamLetra";
            this.menuItemTamLetra.Text = "";
            // 
            // pagHeader
            // 
            this.pagHeader.Name = "pagHeader";
            this.pagHeader.Text = "Registros Paginacion";
            // 
            // menuComboItem
            // 
            // 
            // 
            // 
            this.menuComboItem.ComboBoxElement.AutoCompleteAppend = null;
            this.menuComboItem.ComboBoxElement.AutoCompleteDataSource = null;
            this.menuComboItem.ComboBoxElement.AutoCompleteSuggest = null;
            this.menuComboItem.ComboBoxElement.DataMember = "";
            this.menuComboItem.ComboBoxElement.DataSource = null;
            this.menuComboItem.ComboBoxElement.DefaultValue = null;
            this.menuComboItem.ComboBoxElement.DisplayMember = "";
            this.menuComboItem.ComboBoxElement.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            this.menuComboItem.ComboBoxElement.DropDownAnimationEnabled = true;
            this.menuComboItem.ComboBoxElement.EditableElementText = "";
            this.menuComboItem.ComboBoxElement.EditorElement = this.menuComboItem.ComboBoxElement;
            this.menuComboItem.ComboBoxElement.EditorManager = null;
            this.menuComboItem.ComboBoxElement.Filter = null;
            this.menuComboItem.ComboBoxElement.FilterExpression = "";
            this.menuComboItem.ComboBoxElement.Focusable = true;
            this.menuComboItem.ComboBoxElement.FormatString = "";
            this.menuComboItem.ComboBoxElement.FormattingEnabled = true;
            this.menuComboItem.ComboBoxElement.MaxDropDownItems = 0;
            this.menuComboItem.ComboBoxElement.MaxLength = 32767;
            this.menuComboItem.ComboBoxElement.MaxValue = null;
            this.menuComboItem.ComboBoxElement.MinValue = null;
            this.menuComboItem.ComboBoxElement.NullValue = null;
            this.menuComboItem.ComboBoxElement.OwnerOffset = 0;
            this.menuComboItem.ComboBoxElement.ShowImageInEditorArea = true;
            this.menuComboItem.ComboBoxElement.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            this.menuComboItem.ComboBoxElement.Value = null;
            this.menuComboItem.ComboBoxElement.ValueMember = "";
            this.menuComboItem.Name = "menuComboItem";
            this.menuComboItem.Text = "";
            // 
            // grupoAcciones
            // 
            this.grupoAcciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnCambiarEstadoExistencia,
            this.rbtnAltaPaletPicosRegularizacion,
            this.rbtnAltaPaletRegularizacion,
            this.rbtnAltaPaletMultiRegularizacion,
            this.rbtnBajaRegularizacion,
            this.rBtnImpresionSSCC});
            this.grupoAcciones.Name = "grupoAcciones";
            this.grupoAcciones.Text = "Acciones";
            // 
            // rBtnCambiarEstadoExistencia
            // 
            this.rBtnCambiarEstadoExistencia.Image = global::RumboSGA.Properties.Resources.EstadoMercanciaIcon1;
            this.rBtnCambiarEstadoExistencia.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCambiarEstadoExistencia.Name = "rBtnCambiarEstadoExistencia";
            this.rBtnCambiarEstadoExistencia.Text = "Estado Mercancía";
            this.rBtnCambiarEstadoExistencia.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCambiarEstadoExistencia.ToolTipText = "Estado Mercancía";
            this.rBtnCambiarEstadoExistencia.Click += new System.EventHandler(this.cambiarEstadoExistenciaMenuItem_Event);
            // 
            // rbtnAltaPaletPicosRegularizacion
            // 
            this.rbtnAltaPaletPicosRegularizacion.Image = global::RumboSGA.Properties.Resources.palletPicos;
            this.rbtnAltaPaletPicosRegularizacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnAltaPaletPicosRegularizacion.Name = "rbtnAltaPaletPicosRegularizacion";
            this.rbtnAltaPaletPicosRegularizacion.Text = "Alta Pico Reg";
            this.rbtnAltaPaletPicosRegularizacion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnAltaPaletPicosRegularizacion.ToolTipText = "Alta Pico Reg";
            this.rbtnAltaPaletPicosRegularizacion.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rbtnAltaPaletRegularizacion
            // 
            this.rbtnAltaPaletRegularizacion.Image = global::RumboSGA.Properties.Resources.pallet;
            this.rbtnAltaPaletRegularizacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnAltaPaletRegularizacion.Name = "rbtnAltaPaletRegularizacion";
            this.rbtnAltaPaletRegularizacion.Text = "Alta Palet Reg.";
            this.rbtnAltaPaletRegularizacion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnAltaPaletRegularizacion.ToolTipText = "Alta Palet Reg.";
            this.rbtnAltaPaletRegularizacion.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rbtnAltaPaletMultiRegularizacion
            // 
            this.rbtnAltaPaletMultiRegularizacion.Image = global::RumboSGA.Properties.Resources.palletMulti;
            this.rbtnAltaPaletMultiRegularizacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnAltaPaletMultiRegularizacion.Name = "rbtnAltaPaletMultiRegularizacion";
            this.rbtnAltaPaletMultiRegularizacion.Text = "Alta Multi Reg";
            this.rbtnAltaPaletMultiRegularizacion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnAltaPaletMultiRegularizacion.ToolTipText = "Alta Multi Reg";
            this.rbtnAltaPaletMultiRegularizacion.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rbtnBajaRegularizacion
            // 
            this.rbtnBajaRegularizacion.Image = global::RumboSGA.Properties.Resources.trash;
            this.rbtnBajaRegularizacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnBajaRegularizacion.Name = "rbtnBajaRegularizacion";
            this.rbtnBajaRegularizacion.Text = "Baja Reg";
            this.rbtnBajaRegularizacion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnBajaRegularizacion.ToolTipText = "Baja Reg";
            this.rbtnBajaRegularizacion.Click += new System.EventHandler(this.rBtnBajaRegularizacion_Click);
            // 
            // rBtnImpresionSSCC
            // 
            this.rBtnImpresionSSCC.Image = global::RumboSGA.Properties.Resources.codigobar;
            this.rBtnImpresionSSCC.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnImpresionSSCC.Name = "rBtnImpresionSSCC";
            this.rBtnImpresionSSCC.Text = "Imp Etiq SSCC";
            this.rBtnImpresionSSCC.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnImpresionSSCC.ToolTipText = "Imp Etiq SSCC";
            this.rBtnImpresionSSCC.Click += new System.EventHandler(this.rBtnImpresionSSCC_Click);
            // 
            // groupExportacion
            // 
            this.groupExportacion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnImpExp});
            this.groupExportacion.Name = "groupExportacion";
            this.groupExportacion.Text = "Exportacion";
            // 
            // rBtnImpExp
            // 
            this.rBtnImpExp.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rBtnImpExp.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnImpExp.Name = "rBtnImpExp";
            this.rBtnImpExp.Text = "I/O";
            this.rBtnImpExp.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnImpExp.ToolTipText = "I/O";
            this.rBtnImpExp.Click += new System.EventHandler(this.rBtnImpExp_Click);

            this.groupExportacion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnExpExccel});
            this.rBtnExpExccel.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rBtnExpExccel.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnExpExccel.Name = "rBtnExpExccel";
            this.rBtnExpExccel.Text = "Exportacion";
            this.rBtnExpExccel.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnExpExccel.ToolTipText = "Exportacion";
            this.rBtnExpExccel.Click += new System.EventHandler(this.btnExportacion_Click);
            
            
            // 
            // Stock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 546);
            this.ControlBox = false;
            this.Controls.Add(this.contenedor);
            this.Controls.Add(this.radRibbonBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Stock";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).EndInit();
            this.virtualGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radProgressBar1)).EndInit();
            this.contenedor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public RumGridView gridView;
        private System.Windows.Forms.Panel contenedor;
        private Telerik.WinControls.UI.RadVirtualGrid virtualGrid;
        private Telerik.WinControls.RootRadElement object_aa623162_f8de_48e1_9845_7cfd24651cf3;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup grupoVer;
        private RumButtonElement btnStockMin;
        private RumButtonElement btnHistArticulo;
        private RumRibbonBarGroup grupoTabla;
        private RumButtonElement btnCambiarVista;
        private RumButtonElement btnExportar;
        private RumButtonElement btnLimpiarFiltros;
        private RumRibbonBarGroup grupoConf;
        private RumDropDownButtonElement radDropDownButtonElement1;
        private RumDropDownButtonElement btnAgruparStock;
        private RumMenuItem stockXArticuloItem;
        private RumMenuItem stockXEstadoItem;
        private RumMenuItem stockXFamiliaItem;
        private RumMenuItem itemGuardarEstilo;
        private RumMenuItem itemCargarEstilo;
        private RumMenuItem itemColumnas;
        private RumMenuItem stockXUbicacionItem;
        private RumMenuItem reiniciarItem;
        private RumButtonElement btnRefrescar;
        private RumMenuHeaderItem headerTamLetra;
        private Telerik.WinControls.UI.RadMenuComboItem menuItemTamLetra;
        private RumMenuHeaderItem pagHeader;
        private Telerik.WinControls.UI.RadMenuComboItem menuComboItem;
        private RumButtonElement btnFiltrar;
        private RumButtonElement rbtnAltaPaletPicosRegularizacion;
        private RumButtonElement rbtnAltaPaletRegularizacion;
        private RumButtonElement rbtnAltaPaletMultiRegularizacion;
        private RumButtonElement rbtnBajaRegularizacion;
        private RumRibbonBarGroup grupoAcciones;
        private RumButtonElement rBtnImpresionSSCC;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private RumRibbonBarGroup groupExportacion;
        private RumButtonElement rBtnImpExp;
        private RumButtonElement rBtnExpExccel;
        private Telerik.WinControls.UI.RadProgressBar radProgressBar1;
        private RumButtonElement rBtnCargarInstalacion;
        private RumButtonElement rBtnCambiarEstadoExistencia;
    }
}
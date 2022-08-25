using RumboSGA.Controles;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas
{
    partial class FrmProductividad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProductividad));
            this.tlPanelPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.rgvOperario = new Telerik.WinControls.UI.RadGridView();
            this.rrbnBarraMenuProductividad = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnBorrarFiltro = new RumboSGA.RumButtonElement();
            this.rBtnExportar = new RumboSGA.RumButtonElement();
            this.rBtnRefrescar = new RumboSGA.RumButtonElement();
            this.rbtnGuardar = new RumboSGA.RumButtonElement();
            this.configBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rddBtnConfigurar = new RumboSGA.Controles.RumDropDownButtonElement();
            this.temasMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.guardarButton = new RumboSGA.Controles.RumMenuItem();
            this.cargarButton = new RumboSGA.Controles.RumMenuItem();
            this.editColumns = new RumboSGA.Controles.RumMenuItem();
            this.pagHeaderItem = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuComboItem = new Telerik.WinControls.UI.RadMenuComboItem();
            this.radRibbonBarButtonGroup1 = new Telerik.WinControls.UI.RadRibbonBarButtonGroup();
            this.checkNoAccion = new Telerik.WinControls.UI.RadCheckBoxElement();
            this.rChkOcultarDescartados = new Telerik.WinControls.UI.RadCheckBoxElement();
            this.grupoSeleccion = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rbtnOperAtras = new RumboSGA.RumButtonElement();
            this.rbtnoperAdelante = new RumboSGA.RumButtonElement();
            this.ddlOperario = new Telerik.WinControls.UI.RadDropDownListElement();
            this.rdtFechaDesde = new Telerik.WinControls.UI.RadDateTimePickerElement();
            this.grupoAciones = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnResolver = new RumboSGA.RumButtonElement();
            this.rbtnCalcular = new RumboSGA.RumButtonElement();
            this.rbtnCalcularAvanzado = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroupIncidencias = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rTxtEleIncidencias = new Telerik.WinControls.UI.RadTextBoxElement();
            this.tlPanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvOperario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvOperario.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rrbnBarraMenuProductividad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlOperario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tlPanelPrincipal
            // 
            this.tlPanelPrincipal.ColumnCount = 4;
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlPanelPrincipal.Controls.Add(this.rgvOperario, 0, 0);
            this.tlPanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlPanelPrincipal.Location = new System.Drawing.Point(0, 162);
            this.tlPanelPrincipal.Name = "tlPanelPrincipal";
            this.tlPanelPrincipal.RowCount = 1;
            this.tlPanelPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanelPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 396F));
            this.tlPanelPrincipal.Size = new System.Drawing.Size(1282, 380);
            this.tlPanelPrincipal.TabIndex = 2;
            // 
            // rgvOperario
            // 
            this.rgvOperario.BackgroundImage = global::RumboSGA.Properties.Resources._003_calculator;
            this.tlPanelPrincipal.SetColumnSpan(this.rgvOperario, 4);
            this.rgvOperario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvOperario.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvOperario.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvOperario.Name = "rgvOperario";
            this.rgvOperario.Size = new System.Drawing.Size(1276, 374);
            this.rgvOperario.TabIndex = 12;
            this.rgvOperario.CellBeginEdit += new Telerik.WinControls.UI.GridViewCellCancelEventHandler(this.RgvOperario_CellBeginEdit);
            this.rgvOperario.CellEndEdit += new Telerik.WinControls.UI.GridViewCellEventHandler(this.RgvOperario_CellEndEdit);
            this.rgvOperario.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.rgvOperario_ContextMenuOpening);
            // 
            // rrbnBarraMenuProductividad
            // 
            this.rrbnBarraMenuProductividad.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            // 
            // 
            // 
            this.rrbnBarraMenuProductividad.ExitButton.Text = "Exit";
            this.rrbnBarraMenuProductividad.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.rrbnBarraMenuProductividad.Location = new System.Drawing.Point(0, 0);
            this.rrbnBarraMenuProductividad.Name = "rrbnBarraMenuProductividad";
            // 
            // 
            // 
            this.rrbnBarraMenuProductividad.OptionsButton.Text = "Options";
            // 
            // 
            // 
            this.rrbnBarraMenuProductividad.RootElement.StretchVertically = true;
            this.rrbnBarraMenuProductividad.SimplifiedHeight = 100;
            this.rrbnBarraMenuProductividad.Size = new System.Drawing.Size(1282, 162);
            this.rrbnBarraMenuProductividad.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("rrbnBarraMenuProductividad.StartButtonImage")));
            this.rrbnBarraMenuProductividad.TabIndex = 1;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.rrbnBarraMenuProductividad.GetChildAt(0))).SimplifiedHeight = 100;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.rrbnBarraMenuProductividad.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.rrbnBarraMenuProductividad.GetChildAt(0).GetChildAt(5))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.rrbnBarraMenuProductividad.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.vistaBarGroup,
            this.configBarGroup,
            this.grupoSeleccion,
            this.grupoAciones,
            this.radRibbonBarGroupIncidencias});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // vistaBarGroup
            // 
            this.vistaBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnBorrarFiltro,
            this.rBtnExportar,
            this.rBtnRefrescar,
            this.rbtnGuardar});
            this.vistaBarGroup.Name = "vistaBarGroup";
            this.vistaBarGroup.Text = "";
            // 
            // rBtnBorrarFiltro
            // 
            this.rBtnBorrarFiltro.BorderHighlightColor = System.Drawing.Color.DarkRed;
            this.rBtnBorrarFiltro.ClickMode = Telerik.WinControls.ClickMode.Release;
            this.rBtnBorrarFiltro.EnableBorderHighlight = false;
            this.rBtnBorrarFiltro.EnableElementShadow = false;
            this.rBtnBorrarFiltro.EnableFocusBorder = false;
            this.rBtnBorrarFiltro.EnableFocusBorderAnimation = true;
            this.rBtnBorrarFiltro.EnableHighlight = false;
            this.rBtnBorrarFiltro.EnableRippleAnimation = false;
            this.rBtnBorrarFiltro.HighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnBorrarFiltro.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.rBtnBorrarFiltro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnBorrarFiltro.Name = "rBtnBorrarFiltro";
            this.rBtnBorrarFiltro.Text = "Limpiar Filtros";
            this.rBtnBorrarFiltro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnBorrarFiltro.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rBtnBorrarFiltro.ToolTipText = "Limpiar Filtros";
            this.rBtnBorrarFiltro.Click += new System.EventHandler(this.rBtnBorrarFiltro_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).GradientPercentage = 0.5F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).GradientPercentage2 = 0.666F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnBorrarFiltro.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // rBtnExportar
            // 
            this.rBtnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rBtnExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnExportar.Name = "rBtnExportar";
            this.rBtnExportar.Text = "Exportar Excel";
            this.rBtnExportar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnExportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rBtnExportar.ToolTipText = "Exportar Excel";
            this.rBtnExportar.Click += new System.EventHandler(this.rBtnExportacion_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).GradientPercentage = 0.5F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).GradientPercentage2 = 0.666F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnExportar.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // rBtnRefrescar
            // 
            this.rBtnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rBtnRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnRefrescar.Name = "rBtnRefrescar";
            this.rBtnRefrescar.Text = "Refrescar";
            this.rBtnRefrescar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnRefrescar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rBtnRefrescar.ToolTipText = "Refrescar";
            this.rBtnRefrescar.Click += new System.EventHandler(this.rBtnRefrescar_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).GradientPercentage = 0.5F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).GradientPercentage2 = 0.666F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnRefrescar.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // rbtnGuardar
            // 
            this.rbtnGuardar.Image = global::RumboSGA.Properties.Resources.Save;
            this.rbtnGuardar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnGuardar.Name = "rbtnGuardar";
            this.rbtnGuardar.Text = "Guardar";
            this.rbtnGuardar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rbtnGuardar.ToolTipText = "Guardar";
            this.rbtnGuardar.Click += new System.EventHandler(this.rbtnGuardar_Click);
            // 
            // configBarGroup
            // 
            this.configBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rddBtnConfigurar,
            this.radRibbonBarButtonGroup1});
            this.configBarGroup.Name = "configBarGroup";
            this.configBarGroup.Text = "";
            // 
            // rddBtnConfigurar
            // 
            this.rddBtnConfigurar.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rddBtnConfigurar.AutoSize = false;
            this.rddBtnConfigurar.Bounds = new System.Drawing.Rectangle(0, 0, 80, 62);
            this.rddBtnConfigurar.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rddBtnConfigurar.ExpandArrowButton = false;
            this.rddBtnConfigurar.Image = global::RumboSGA.Properties.Resources.Administration;
            this.rddBtnConfigurar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rddBtnConfigurar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.guardarButton,
            this.cargarButton,
            this.editColumns,
            this.pagHeaderItem,
            this.menuComboItem});
            this.rddBtnConfigurar.Name = "rddBtnConfigurar";
            this.rddBtnConfigurar.Text = "";
            // 
            // temasMenuItem
            // 
            this.temasMenuItem.Name = "temasMenuItem";
            this.temasMenuItem.Text = "";
            // 
            // guardarButton
            // 
            this.guardarButton.Name = "guardarButton";
            this.guardarButton.Text = "";
            this.guardarButton.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarButton
            // 
            this.cargarButton.Name = "cargarButton";
            this.cargarButton.Text = "";
            this.cargarButton.Click += new System.EventHandler(this.LoadItem_Click);
            // 
            // editColumns
            // 
            this.editColumns.Name = "editColumns";
            this.editColumns.Text = "";
            this.editColumns.Click += new System.EventHandler(this.ItemColumnas_Click);
            // 
            // pagHeaderItem
            // 
            this.pagHeaderItem.Name = "pagHeaderItem";
            this.pagHeaderItem.Text = "Registros Paginación";
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
            // radRibbonBarButtonGroup1
            // 
            this.radRibbonBarButtonGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.checkNoAccion,
            this.rChkOcultarDescartados});
            this.radRibbonBarButtonGroup1.Name = "radRibbonBarButtonGroup1";
            this.radRibbonBarButtonGroup1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.radRibbonBarButtonGroup1.Padding = new System.Windows.Forms.Padding(1);
            this.radRibbonBarButtonGroup1.ShowBorder = false;
            // 
            // checkNoAccion
            // 
            this.checkNoAccion.Checked = false;
            this.checkNoAccion.Name = "checkNoAccion";
            this.checkNoAccion.ReadOnly = false;
            this.checkNoAccion.ShowBorder = false;
            this.checkNoAccion.StretchVertically = false;
            this.checkNoAccion.Text = "Ver no acción";
            this.checkNoAccion.Click += new System.EventHandler(this.radCheckBoxElement1_Click);
            // 
            // rChkOcultarDescartados
            // 
            this.rChkOcultarDescartados.Checked = true;
            this.rChkOcultarDescartados.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rChkOcultarDescartados.Name = "rChkOcultarDescartados";
            this.rChkOcultarDescartados.ReadOnly = false;
            this.rChkOcultarDescartados.ShowBorder = false;
            this.rChkOcultarDescartados.Text = "Ocultar descartados";
            this.rChkOcultarDescartados.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.rChkOcultarDescartados.CheckStateChanged += new System.EventHandler(this.RChkOcultarDescartados_CheckStateChanged);
            // 
            // grupoSeleccion
            // 
            this.grupoSeleccion.Alignment = System.Drawing.ContentAlignment.BottomLeft;
            this.grupoSeleccion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rbtnOperAtras,
            this.rbtnoperAdelante,
            this.ddlOperario,
            this.rdtFechaDesde});
            this.grupoSeleccion.MaxSize = new System.Drawing.Size(450, 100);
            this.grupoSeleccion.Name = "grupoSeleccion";
            this.grupoSeleccion.StretchVertically = false;
            this.grupoSeleccion.Text = "Selección";
            // 
            // rbtnOperAtras
            // 
            this.rbtnOperAtras.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbtnOperAtras.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnOperAtras.Image = global::RumboSGA.Properties.Resources.izquierda;
            this.rbtnOperAtras.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.rbtnOperAtras.Name = "rbtnOperAtras";
            this.rbtnOperAtras.StretchVertically = false;
            this.rbtnOperAtras.Text = "";
            this.rbtnOperAtras.Click += new System.EventHandler(this.rbtnOperAtras_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).GradientPercentage = 0.5F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).GradientPercentage2 = 0.666F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnOperAtras.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // rbtnoperAdelante
            // 
            this.rbtnoperAdelante.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.rbtnoperAdelante.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnoperAdelante.Image = global::RumboSGA.Properties.Resources.derecha;
            this.rbtnoperAdelante.ImageAlignment = System.Drawing.ContentAlignment.BottomLeft;
            this.rbtnoperAdelante.Name = "rbtnoperAdelante";
            this.rbtnoperAdelante.ShouldPaint = false;
            this.rbtnoperAdelante.StretchVertically = false;
            this.rbtnoperAdelante.Text = "";
            this.rbtnoperAdelante.Click += new System.EventHandler(this.rbtnoperAdelante_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).GradientPercentage = 0.5F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).GradientPercentage2 = 0.666F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnoperAdelante.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // ddlOperario
            // 
            this.ddlOperario.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ddlOperario.ArrowButtonMinWidth = 17;
            this.ddlOperario.AutoCompleteDataSource = null;
            this.ddlOperario.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ddlOperario.DataMember = "";
            this.ddlOperario.DataSource = null;
            this.ddlOperario.DefaultValue = null;
            this.ddlOperario.DisplayMember = "";
            this.ddlOperario.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            this.ddlOperario.DropDownAnimationEnabled = true;
            this.ddlOperario.EditableElementText = "";
            this.ddlOperario.EditorElement = this.ddlOperario;
            this.ddlOperario.EditorManager = null;
            this.ddlOperario.Filter = null;
            this.ddlOperario.FilterExpression = "";
            this.ddlOperario.Focusable = true;
            this.ddlOperario.FormatString = "";
            this.ddlOperario.FormattingEnabled = true;
            this.ddlOperario.MaxDropDownItems = 0;
            this.ddlOperario.MaxLength = 32767;
            this.ddlOperario.MaxValue = null;
            this.ddlOperario.MinSize = new System.Drawing.Size(140, 0);
            this.ddlOperario.MinValue = null;
            this.ddlOperario.Name = "ddlOperario";
            this.ddlOperario.NullValue = null;
            this.ddlOperario.OwnerOffset = 0;
            this.ddlOperario.ShowImageInEditorArea = true;
            this.ddlOperario.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            this.ddlOperario.StretchVertically = false;
            this.ddlOperario.Value = null;
            this.ddlOperario.ValueMember = "";
            this.ddlOperario.SelectedValueChanged += new Telerik.WinControls.UI.Data.ValueChangedEventHandler(this.ddlOperario_SelectedValueChanged);
            // 
            // rdtFechaDesde
            // 
            this.rdtFechaDesde.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            this.rdtFechaDesde.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.rdtFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.rdtFechaDesde.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.rdtFechaDesde.MinDate = new System.DateTime(((long)(0)));
            this.rdtFechaDesde.MinSize = new System.Drawing.Size(230, 10);
            this.rdtFechaDesde.Name = "rdtFechaDesde";
            this.rdtFechaDesde.NullDate = new System.DateTime(((long)(0)));
            this.rdtFechaDesde.StretchVertically = false;
            this.rdtFechaDesde.ValueChanged += new System.EventHandler(this.rdtFechaDesde_ValueChanged);
            // 
            // grupoAciones
            // 
            this.grupoAciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnResolver,
            this.rbtnCalcular,
            this.rbtnCalcularAvanzado});
            this.grupoAciones.Name = "grupoAciones";
            this.grupoAciones.Text = "Acciones";
            this.grupoAciones.Click += new System.EventHandler(this.radRibbonBarGroup2_Click);
            // 
            // rBtnResolver
            // 
            this.rBtnResolver.Image = global::RumboSGA.Properties.Resources._001_repair_tools;
            this.rBtnResolver.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnResolver.Name = "rBtnResolver";
            this.rBtnResolver.Text = "Resolver";
            this.rBtnResolver.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnResolver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rBtnResolver.ToolTipText = "Resolver";
            this.rBtnResolver.Click += new System.EventHandler(this.rBtnResolver_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).GradientPercentage = 0.5F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).GradientPercentage2 = 0.666F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rBtnResolver.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // rbtnCalcular
            // 
            this.rbtnCalcular.Alignment = System.Drawing.ContentAlignment.TopRight;
            this.rbtnCalcular.Image = global::RumboSGA.Properties.Resources._003_calculator;
            this.rbtnCalcular.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnCalcular.Name = "rbtnCalcular";
            this.rbtnCalcular.SerializeChildren = false;
            this.rbtnCalcular.StretchVertically = true;
            this.rbtnCalcular.Text = "Calcular";
            this.rbtnCalcular.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnCalcular.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rbtnCalcular.ToolTipText = "Calcular";
            this.rbtnCalcular.Click += new System.EventHandler(this.rbtnCalcular_Click);
            // 
            // rbtnCalcularAvanzado
            // 
            this.rbtnCalcularAvanzado.Image = global::RumboSGA.Properties.Resources._003_calculator;
            this.rbtnCalcularAvanzado.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnCalcularAvanzado.Name = "rbtnCalcularAvanzado";
            this.rbtnCalcularAvanzado.Text = "Calcular Avanzado";
            this.rbtnCalcularAvanzado.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnCalcularAvanzado.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rbtnCalcularAvanzado.ToolTipText = "Calcular Avanzado";
            this.rbtnCalcularAvanzado.Click += new System.EventHandler(this.RbtnCalcularAvanzado_Click);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).GradientPercentage = 0.5F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).GradientPercentage2 = 0.666F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.rbtnCalcularAvanzado.GetChildAt(0))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            // 
            // radRibbonBarGroupIncidencias
            // 
            this.radRibbonBarGroupIncidencias.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rTxtEleIncidencias});
            this.radRibbonBarGroupIncidencias.Name = "radRibbonBarGroupIncidencias";
            this.radRibbonBarGroupIncidencias.Text = "Incidencias";
            // 
            // rTxtEleIncidencias
            // 
            this.rTxtEleIncidencias.EmbeddedLabelText = "";
            this.rTxtEleIncidencias.MinSize = new System.Drawing.Size(140, 0);
            this.rTxtEleIncidencias.Name = "rTxtEleIncidencias";
            this.rTxtEleIncidencias.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
            this.rTxtEleIncidencias.Text = "0";
            this.rTxtEleIncidencias.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // FrmProductividad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 542);
            this.Controls.Add(this.tlPanelPrincipal);
            this.Controls.Add(this.rrbnBarraMenuProductividad);
            this.MainMenuStrip = null;
            this.Name = "FrmProductividad";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tlPanelPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvOperario.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvOperario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rrbnBarraMenuProductividad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlOperario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Telerik.WinControls.UI.RadRibbonBar rrbnBarraMenuProductividad;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup vistaBarGroup;
        private RumButtonElement rBtnBorrarFiltro;
        private RumRibbonBarGroup configBarGroup;
        private RumDropDownButtonElement rddBtnConfigurar;
        private RumMenuItem guardarButton;
        private RumMenuItem cargarButton;
        private RumMenuItem editColumns;
        private System.Windows.Forms.TableLayoutPanel tlPanelPrincipal;
        private Telerik.WinControls.UI.RadGridView rgvOperario;
        private RumMenuItem temasMenuItem;
        private RumButtonElement rBtnExportar;
        private RumButtonElement rBtnRefrescar;
        private RumMenuHeaderItem pagHeaderItem;
        private Telerik.WinControls.UI.RadMenuComboItem menuComboItem;
        //private RumButtonElement rbtnFechaAdelante;
        //private RumButtonElement rbtnFechaAtras;
        private RumButtonElement rbtnoperAdelante;
        private RumButtonElement rbtnOperAtras;       
        private RumRibbonBarGroup grupoSeleccion;
        private RumRibbonBarGroup grupoAciones;
        private RumButtonElement rBtnResolver;
        private RumButtonElement rbtnCalcular;
        private Telerik.WinControls.UI.RadDropDownListElement ddlOperario;        
        private RumButtonElement rbtnGuardar;
        private Telerik.WinControls.UI.RadRibbonBarButtonGroup radRibbonBarButtonGroup1;
        private Telerik.WinControls.UI.RadCheckBoxElement checkNoAccion;
        private Telerik.WinControls.UI.RadCheckBoxElement rChkOcultarDescartados;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroupIncidencias;
        private Telerik.WinControls.UI.RadTextBoxElement rTxtEleIncidencias;
        private Telerik.WinControls.UI.RadDateTimePickerElement rdtFechaDesde;
        private RumButtonElement rbtnCalcularAvanzado;
    }
}

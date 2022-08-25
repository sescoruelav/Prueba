using RumboSGA.Controles;
using System.Windows.Forms;

namespace RumboSGA.Presentation
{
    partial class SFPrincipal
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
            Telerik.WinControls.UI.RadTreeNode radTreeNode1 = new Telerik.WinControls.UI.RadTreeNode();
            Telerik.WinControls.UI.RadTreeNode radTreeNode2 = new Telerik.WinControls.UI.RadTreeNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFPrincipal));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radCollapsiblePanel1 = new Telerik.WinControls.UI.RadCollapsiblePanel();
            this.rtvArbolMenu = new Telerik.WinControls.UI.RadTreeView();
            this.tcuadroControl = new RumboSGA.Controles.DBTableLayoutPanel(this.components);
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.mantenimientosTab = new RumboSGA.Controles.RumRibbonTab();
            this.modBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnNuevo = new RumboSGA.RumButtonElement();
            this.bntBorrar = new RumboSGA.RumButtonElement();
            this.btnClonar = new RumboSGA.RumButtonElement();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnCambiarVista = new RumboSGA.RumButtonElement();
            this.btnExportar = new RumboSGA.RumButtonElement();
            this.btnQuitarFiltros = new RumboSGA.RumButtonElement();
            this.btnRefrescar = new RumboSGA.RumButtonElement();
            this.btnBarraBusqueda = new RumboSGA.RumButtonElement();
            this.configBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnConf = new RumboSGA.Controles.RumDropDownButtonElement();
            this.menuItemColumnas = new RumboSGA.Controles.RumMenuItem();
            this.menuItemGuardar = new RumboSGA.Controles.RumMenuItem();
            this.menuItemCargar = new RumboSGA.Controles.RumMenuItem();
            this.menuItemTemas = new RumboSGA.Controles.RumMenuItem();
            this.headerTamLetra = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuItemTamLetra = new RumboSGA.Controles.RumMenuComboItem();
            this.headerPaginacion = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuComboItem = new RumboSGA.Controles.RumMenuComboItem();
            this.IncidenciaJira = new Telerik.WinControls.UI.RadMenuItem();
            this.menuManual = new Telerik.WinControls.UI.RadMenuItem();
            this.etiquetaCantBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnCrearRecepcion = new RumboSGA.RumButtonElement();
            this.btnCerrarRecepcion = new RumboSGA.RumButtonElement();
            this.btnBuscar = new RumboSGA.RumButtonElement();
            this.btnAsignarRecurso = new RumboSGA.RumButtonElement();
            this.btnActualizar = new RumboSGA.RumButtonElement();
            this.btnDeshacer = new RumboSGA.RumButtonElement();
            this.btnCambiarPorPedido = new RumboSGA.RumButtonElement();
            this.btnPedidos = new RumboSGA.RumButtonElement();
            this.btnOperarios = new RumboSGA.RumButtonElement();
            this.radDropDownButtonElement1 = new RumboSGA.Controles.RumDropDownButtonElement();
            this.menuItemColumnasControlTareas = new RumboSGA.Controles.RumMenuItem();
            this.menuItemGuardarControlTareas = new RumboSGA.Controles.RumMenuItem();
            this.menuItemCargarControlTareas = new RumboSGA.Controles.RumMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).BeginInit();
            this.radCollapsiblePanel1.PanelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rtvArbolMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.33334F));
            this.tableLayoutPanel1.Controls.Add(this.radCollapsiblePanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tcuadroControl, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 166);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1276, 569);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // radCollapsiblePanel1
            // 
            this.radCollapsiblePanel1.BackColor = System.Drawing.SystemColors.Control;
            this.radCollapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radCollapsiblePanel1.ExpandDirection = Telerik.WinControls.UI.RadDirection.Right;
            this.radCollapsiblePanel1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radCollapsiblePanel1.Location = new System.Drawing.Point(3, 3);
            this.radCollapsiblePanel1.Name = "radCollapsiblePanel1";
            this.radCollapsiblePanel1.OwnerBoundsCache = new System.Drawing.Rectangle(3, 85, 261, 735);
            // 
            // radCollapsiblePanel1.PanelContainer
            // 
            this.radCollapsiblePanel1.PanelContainer.Controls.Add(this.rtvArbolMenu);
            this.radCollapsiblePanel1.PanelContainer.Size = new System.Drawing.Size(181, 561);
            // 
            // 
            // 
            this.radCollapsiblePanel1.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 3, 187, 250);
            this.radCollapsiblePanel1.Size = new System.Drawing.Size(206, 563);
            this.radCollapsiblePanel1.TabIndex = 2;
            // 
            // rtvArbolMenu
            // 
            this.rtvArbolMenu.BackColor = System.Drawing.SystemColors.Control;
            this.rtvArbolMenu.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtvArbolMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtvArbolMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rtvArbolMenu.ForeColor = System.Drawing.Color.Black;
            this.rtvArbolMenu.ItemHeight = 28;
            this.rtvArbolMenu.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.rtvArbolMenu.LineStyle = Telerik.WinControls.UI.TreeLineStyle.Solid;
            this.rtvArbolMenu.Location = new System.Drawing.Point(0, 0);
            this.rtvArbolMenu.Name = "rtvArbolMenu";
            radTreeNode1.Expanded = true;
            radTreeNode1.Name = "Maestros";
            radTreeNode2.Name = "Proveedores";
            radTreeNode2.Text = "Proveedores";
            radTreeNode1.Nodes.AddRange(new Telerik.WinControls.UI.RadTreeNode[] {
            radTreeNode2});
            radTreeNode1.Text = "Maestros";
            this.rtvArbolMenu.Nodes.AddRange(new Telerik.WinControls.UI.RadTreeNode[] {
            radTreeNode1});
            this.rtvArbolMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.rtvArbolMenu.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 187, 312);
            this.rtvArbolMenu.ShowExpandCollapse = false;
            this.rtvArbolMenu.Size = new System.Drawing.Size(181, 561);
            this.rtvArbolMenu.SpacingBetweenNodes = -1;
            this.rtvArbolMenu.TabIndex = 1;
            // 
            // tcuadroControl
            // 
            this.tcuadroControl.ColumnCount = 5;
            this.tcuadroControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcuadroControl.Location = new System.Drawing.Point(215, 3);
            this.tcuadroControl.Name = "tcuadroControl";
            this.tcuadroControl.RowCount = 5;
            this.tcuadroControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tcuadroControl.Size = new System.Drawing.Size(1058, 563);
            this.tcuadroControl.TabIndex = 3;
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.mantenimientosTab});
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
            this.radRibbonBar1.Size = new System.Drawing.Size(1276, 166);
            this.radRibbonBar1.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radRibbonBar1.StartButtonImage")));
            this.radRibbonBar1.TabIndex = 2;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).SimplifiedHeight = 100;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // mantenimientosTab
            // 
            this.mantenimientosTab.IsSelected = true;
            this.mantenimientosTab.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.modBarGroup,
            this.vistaBarGroup,
            this.configBarGroup});
            this.mantenimientosTab.Name = "mantenimientosTab";
            this.mantenimientosTab.Text = "Acciones";
            this.mantenimientosTab.UseMnemonic = false;
            // 
            // modBarGroup
            // 
            this.modBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnNuevo,
            this.bntBorrar,
            this.btnClonar});
            this.modBarGroup.Name = "modBarGroup";
            this.modBarGroup.Text = "Modificaciones";
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = global::RumboSGA.Properties.Resources.Add;
            this.btnNuevo.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNuevo.ToolTipText = "Nuevo";
            this.btnNuevo.Click += new System.EventHandler(this.Nuevo_Click);
            // 
            // bntBorrar
            // 
            this.bntBorrar.Image = global::RumboSGA.Properties.Resources.Delete;
            this.bntBorrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.bntBorrar.Name = "bntBorrar";
            this.bntBorrar.Text = "Borrar";
            this.bntBorrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.bntBorrar.ToolTipText = "Borrar";
            this.bntBorrar.Click += new System.EventHandler(this.Borrar_Click);
            // 
            // btnClonar
            // 
            this.btnClonar.Image = global::RumboSGA.Properties.Resources.copy;
            this.btnClonar.Name = "btnClonar";
            this.btnClonar.Text = "Clonar";
            this.btnClonar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClonar.ToolTipText = "Clonar";
            this.btnClonar.Click += new System.EventHandler(this.Clonar_Click);
            // 
            // vistaBarGroup
            // 
            this.vistaBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnCambiarVista,
            this.btnExportar,
            this.btnQuitarFiltros,
            this.btnRefrescar,
            this.btnBarraBusqueda});
            this.vistaBarGroup.Name = "vistaBarGroup";
            this.vistaBarGroup.Text = "Ver";
            // 
            // btnCambiarVista
            // 
            this.btnCambiarVista.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.btnCambiarVista.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCambiarVista.Name = "btnCambiarVista";
            this.btnCambiarVista.Text = "Cambiar Vista";
            this.btnCambiarVista.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCambiarVista.ToolTipText = "Cambiar Vista";
            this.btnCambiarVista.Click += new System.EventHandler(this.CambiarVista_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Text = "";
            this.btnExportar.Click += new System.EventHandler(this.Exportar_Click);
            // 
            // btnQuitarFiltros
            // 
            this.btnQuitarFiltros.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnQuitarFiltros.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnQuitarFiltros.Name = "btnQuitarFiltros";
            this.btnQuitarFiltros.Text = "";
            this.btnQuitarFiltros.Click += new System.EventHandler(this.QuitarFiltros_Click);
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Text = "";
            this.btnRefrescar.Click += new System.EventHandler(this.Refrescar_Click);
            // 
            // btnBarraBusqueda
            // 
            this.btnBarraBusqueda.Image = global::RumboSGA.Properties.Resources.View;
            this.btnBarraBusqueda.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBarraBusqueda.Name = "btnBarraBusqueda";
            this.btnBarraBusqueda.Text = "";
            this.btnBarraBusqueda.Click += new System.EventHandler(this.BarraBusqueda_Click);
            // 
            // configBarGroup
            // 
            this.configBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnConf});
            this.configBarGroup.Name = "configBarGroup";
            this.configBarGroup.Text = "Configuracion";
            // 
            // btnConf
            // 
            this.btnConf.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConf.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.btnConf.AutoSize = false;
            this.btnConf.Bounds = new System.Drawing.Rectangle(0, 0, 74, 65);
            this.btnConf.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.btnConf.ExpandArrowButton = false;
            this.btnConf.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnConf.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConf.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuItemColumnas,
            this.menuItemGuardar,
            this.menuItemCargar,
            this.menuItemTemas,
            this.headerTamLetra,
            this.menuItemTamLetra,
            this.headerPaginacion,
            this.menuComboItem,
            this.IncidenciaJira,
            this.menuManual});
            this.btnConf.Name = "btnConf";
            this.btnConf.Text = "";
            // 
            // menuItemColumnas
            // 
            this.menuItemColumnas.Name = "menuItemColumnas";
            this.menuItemColumnas.Text = "radMenuItem1";
            // 
            // menuItemGuardar
            // 
            this.menuItemGuardar.Name = "menuItemGuardar";
            this.menuItemGuardar.Text = "radMenuItem1";
            // 
            // menuItemCargar
            // 
            this.menuItemCargar.Name = "menuItemCargar";
            this.menuItemCargar.Text = "radMenuItem1";
            // 
            // menuItemTemas
            // 
            this.menuItemTemas.Name = "menuItemTemas";
            this.menuItemTemas.Text = "Temas";
            // 
            // headerTamLetra
            // 
            this.headerTamLetra.Name = "headerTamLetra";
            this.headerTamLetra.Text = "Tamaño Letra";
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
            this.menuItemTamLetra.Text = "Tamaño Letra Tabla";
            // 
            // headerPaginacion
            // 
            this.headerPaginacion.Name = "headerPaginacion";
            this.headerPaginacion.Text = "Registros Paginación";
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
            // 
            // IncidenciaJira
            // 
            this.IncidenciaJira.Name = "IncidenciaJira";
            this.IncidenciaJira.Text = "Incidencia Jira";
            // 
            // menuManual
            // 
            this.menuManual.Name = "menuManual";
            this.menuManual.Text = "Manual";
            // 
            // etiquetaCantBarGroup
            // 
            this.etiquetaCantBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnCrearRecepcion,
            this.btnCerrarRecepcion,
            this.btnBuscar,
            this.btnAsignarRecurso,
            this.btnActualizar,
            this.btnDeshacer,
            this.btnCambiarPorPedido});
            this.etiquetaCantBarGroup.Name = "etiquetaCantBarGroup";
            this.etiquetaCantBarGroup.Text = "";
            // 
            // btnCrearRecepcion
            // 
            this.btnCrearRecepcion.Image = global::RumboSGA.Properties.Resources.Add;
            this.btnCrearRecepcion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCrearRecepcion.Name = "btnCrearRecepcion";
            this.btnCrearRecepcion.Text = "";
            this.btnCrearRecepcion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCerrarRecepcion
            // 
            this.btnCerrarRecepcion.Image = global::RumboSGA.Properties.Resources.Close;
            this.btnCerrarRecepcion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCerrarRecepcion.Name = "btnCerrarRecepcion";
            this.btnCerrarRecepcion.Text = "";
            this.btnCerrarRecepcion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Image = global::RumboSGA.Properties.Resources.Find;
            this.btnBuscar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Text = "";
            this.btnBuscar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnAsignarRecurso
            // 
            this.btnAsignarRecurso.Image = global::RumboSGA.Properties.Resources.NewResource;
            this.btnAsignarRecurso.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAsignarRecurso.Name = "btnAsignarRecurso";
            this.btnAsignarRecurso.Text = "";
            this.btnAsignarRecurso.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnActualizar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Text = "";
            // 
            // btnDeshacer
            // 
            this.btnDeshacer.Image = global::RumboSGA.Properties.Resources.Undo;
            this.btnDeshacer.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDeshacer.Name = "btnDeshacer";
            this.btnDeshacer.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCambiarPorPedido
            // 
            this.btnCambiarPorPedido.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.btnCambiarPorPedido.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCambiarPorPedido.Name = "btnCambiarPorPedido";
            this.btnCambiarPorPedido.Text = "";
            this.btnCambiarPorPedido.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnPedidos
            // 
            this.btnPedidos.Image = global::RumboSGA.Properties.Resources.Form;
            this.btnPedidos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPedidos.Name = "btnPedidos";
            this.btnPedidos.ShowBorder = false;
            this.btnPedidos.Text = "";
            this.btnPedidos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPedidos.Click += new System.EventHandler(this.pedidos_Click);
            // 
            // btnOperarios
            // 
            this.btnOperarios.Image = global::RumboSGA.Properties.Resources.edit;
            this.btnOperarios.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOperarios.Name = "btnOperarios";
            this.btnOperarios.ShowBorder = false;
            this.btnOperarios.Text = "";
            this.btnOperarios.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOperarios.Click += new System.EventHandler(this.operarios_Click);
            // 
            // radDropDownButtonElement1
            // 
            this.radDropDownButtonElement1.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.radDropDownButtonElement1.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.radDropDownButtonElement1.ExpandArrowButton = false;
            this.radDropDownButtonElement1.Image = global::RumboSGA.Properties.Resources.Administration;
            this.radDropDownButtonElement1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuItemColumnasControlTareas,
            this.menuItemGuardarControlTareas,
            this.menuItemCargarControlTareas});
            this.radDropDownButtonElement1.Name = "radDropDownButtonElement1";
            this.radDropDownButtonElement1.Text = "";
            // 
            // menuItemColumnasControlTareas
            // 
            this.menuItemColumnasControlTareas.Name = "menuItemColumnasControlTareas";
            this.menuItemColumnasControlTareas.Text = "";
            // 
            // menuItemGuardarControlTareas
            // 
            this.menuItemGuardarControlTareas.Name = "menuItemGuardarControlTareas";
            this.menuItemGuardarControlTareas.Text = "radMenuItem2";
            // 
            // menuItemCargarControlTareas
            // 
            this.menuItemCargarControlTareas.Name = "menuItemCargarControlTareas";
            this.menuItemCargarControlTareas.Text = "radMenuItem3";
            // 
            // SFPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1276, 735);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = null;
            this.Name = "SFPrincipal";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SFPrincipal_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.radCollapsiblePanel1.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtvArbolMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadCollapsiblePanel radCollapsiblePanel1;
        private Telerik.WinControls.UI.RadTreeView rtvArbolMenu;
        public Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private RumRibbonTab mantenimientosTab;
        private RumRibbonBarGroup modBarGroup;
        private RumButtonElement btnNuevo;
        private RumButtonElement bntBorrar;
        private RumButtonElement btnClonar;
        private RumRibbonBarGroup vistaBarGroup;
        private RumButtonElement btnCambiarVista;
        private RumButtonElement btnExportar;
        private RumButtonElement btnQuitarFiltros;
        private RumRibbonBarGroup configBarGroup;
        private RumDropDownButtonElement btnConf;
        private RumMenuItem menuItemColumnas;
        private RumMenuItem menuItemGuardar;
        private RumMenuItem menuItemCargar;
        private RumMenuItem menuItemTemas;
        private RumRibbonBarGroup etiquetaCantBarGroup;
        private RumButtonElement btnPedidos;
        private RumButtonElement btnOperarios;
        private RumDropDownButtonElement radDropDownButtonElement1;
        private RumMenuItem menuItemColumnasControlTareas;
        private RumMenuItem menuItemGuardarControlTareas;
        private RumMenuItem menuItemCargarControlTareas;
        private RumButtonElement btnCrearRecepcion;
        private RumButtonElement btnCerrarRecepcion;
        private RumButtonElement btnBuscar;
        private RumButtonElement btnDeshacer;
        private RumButtonElement btnAsignarRecurso;
        private RumButtonElement btnRefrescar;
        private RumMenuComboItem menuItemTamLetra;
        private RumMenuHeaderItem headerTamLetra;
        private RumButtonElement btnCambiarPorPedido;
        private RumButtonElement btnActualizar;
        private RumMenuHeaderItem headerPaginacion;
        private RumMenuComboItem menuComboItem;
        private RumButtonElement btnBarraBusqueda;
        private Telerik.WinControls.UI.RadMenuItem IncidenciaJira;
        private Telerik.WinControls.UI.RadMenuItem menuManual;
        private DBTableLayoutPanel tcuadroControl;
        // private cronus2020DataSet cronus2020DataSet;
    }
}

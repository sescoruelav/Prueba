using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;

namespace RumboSGA.Maestros
{
    partial class OrdenesRecogida
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Código generado por el Diseñador de componentes
        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.tableLayoutPanelCarga = new System.Windows.Forms.TableLayoutPanel();
            this.rgvGridPedidos = new Telerik.WinControls.UI.RadGridView();
            this.rdfFiltro = new Telerik.WinControls.UI.RadDataFilter();
            this.tableLayoutPanelBotones = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelDetalleCarga = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelDetalleDatosTotal = new System.Windows.Forms.TableLayoutPanel();
            this.tbDetalleCarga = new System.Windows.Forms.TabControl();
            this.tabPackingList = new System.Windows.Forms.TabPage();
            this.tabPedidos = new System.Windows.Forms.TabPage();
            this.tabSalidas = new System.Windows.Forms.TabPage();
            this.radRibbonBarAcciones = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTabAccionesPedidos = new RumboSGA.Controles.RumRibbonTab();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnVistaPedidosPedidos = new RumboSGA.RumButtonElement();
            this.rBtnVistaPedidosOrdenes = new RumboSGA.RumButtonElement();
            this.rBtnVistaPedidosCarga = new RumboSGA.RumButtonElement();
            this.rBtnOrganizador = new RumboSGA.RumButtonElement();
            this.verGrupoPrincipal = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnddPedidosfiltradoOpciones = new RumboSGA.Controles.RumDropDownButtonElement();
            this.ordenPendientesHoy = new RumboSGA.Controles.RumMenuItem();
            this.tareasHechasHoy = new RumboSGA.Controles.RumMenuItem();
            this.tareasPendientesMenu = new RumboSGA.Controles.RumMenuItem();
            this.bultosPendCarga = new RumboSGA.Controles.RumMenuItem();
            this.ordenCargaAbiertas = new RumboSGA.Controles.RumMenuItem();
            this.rBtnVistaPedidosExportar = new RumboSGA.RumButtonElement();
            this.rBtnVistaPedidosFiltrar = new RumboSGA.RumButtonElement();
            this.btnVistaPedidosQuitarFiltro = new RumboSGA.RumButtonElement();
            this.btnVistaPedidosRefrescar = new RumboSGA.RumButtonElement();
            this.prepBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnAccionLiberarPedido = new RumboSGA.RumButtonElement();
            this.btnAccionLanzamientoPedido = new RumboSGA.RumButtonElement();
            this.btnAccionLanzamientoAgrupado = new RumboSGA.RumButtonElement();
            this.btnAccionLanzammientoOla = new RumboSGA.RumButtonElement();
            this.rBtnCerrarPedido = new RumboSGA.RumButtonElement();
            this.chkRecursoManual = new Telerik.WinControls.UI.RadCheckBoxElement();
            this.rrbgVistaPedidosConfiguracion = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnVistaPedidosConfiguracion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.temasMenu = new RumboSGA.Controles.RumMenuItem();
            this.guardarMenu = new RumboSGA.Controles.RumMenuItem();
            this.cargarMenu = new RumboSGA.Controles.RumMenuItem();
            this.editColumns = new RumboSGA.Controles.RumMenuItem();
            this.ImprimirEtiquetasPackingList = new RumboSGA.Controles.RumMenuItem();
            this.ribbonTabAccionesOrdenesRecogida = new RumboSGA.Controles.RumRibbonTab();
            this.ribbonbarVistaRecogida = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnVistaOrdenesPedidos = new RumboSGA.RumButtonElement();
            this.btnVistaOrdenesOrdenes = new RumboSGA.RumButtonElement();
            this.btnVistaOrdenesCarga = new RumboSGA.RumButtonElement();
            this.verRecogidaGrupo = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnVistaOrdenesExportar = new RumboSGA.RumButtonElement();
            this.btnVistaOrdenesFiltrar = new RumboSGA.RumButtonElement();
            this.btnBorrarFiltroOrdenes = new RumboSGA.RumButtonElement();
            this.btnVistaOrdenesRefrescar = new RumboSGA.RumButtonElement();
            this.accionRecogBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnAccionReservar = new RumboSGA.RumButtonElement();
            this.btnAccionServir = new RumboSGA.RumButtonElement();
            this.rBtnServirSeleccionadas = new RumboSGA.RumButtonElement();
            this.btnAccionCancelarReservar = new RumboSGA.RumButtonElement();
            this.btnCancelarReservaSeleccionadas = new RumboSGA.RumButtonElement();
            this.btnModificarReserva = new RumboSGA.RumButtonElement();
            this.btnCambiarExistencia = new RumboSGA.RumButtonElement();
            this.btnAccionImprimirInforme = new RumboSGA.RumButtonElement();
            this.rBtnAsignarMuelle = new RumboSGA.RumButtonElement();
            this.rBtnCerrarOrden = new RumboSGA.RumButtonElement();
            this.configRecogidaGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnVistaOrdenesConfiguracion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.temasRecogidaMenu = new RumboSGA.Controles.RumMenuItem();
            this.guardarMenuRecogida = new RumboSGA.Controles.RumMenuItem();
            this.cargarMenuRecogida = new RumboSGA.Controles.RumMenuItem();
            this.columnRecogMenu = new RumboSGA.Controles.RumMenuItem();
            this.ribbonTabAccionesOrdenesCarga = new RumboSGA.Controles.RumRibbonTab();
            this.vistaCargaGrupo = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnVistaCargaPedidos = new RumboSGA.RumButtonElement();
            this.btnVistaCargaOrdenes = new RumboSGA.RumButtonElement();
            this.btnVistaCargaCarga = new RumboSGA.RumButtonElement();
            this.verCargaGrupo = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnVistaCargaExportar = new RumboSGA.RumButtonElement();
            this.btnVistaCargaFiltrar = new RumboSGA.RumButtonElement();
            this.btnVistaCargaBorrarFiltro = new RumboSGA.RumButtonElement();
            this.btnVistaCargaRefrescar = new RumboSGA.RumButtonElement();
            this.accionesCarga = new RumboSGA.Controles.RumRibbonBarGroup();
            this.bntAccionCrearCarga = new RumboSGA.RumButtonElement();
            this.btnAccionGenerarTareaCarga = new RumboSGA.RumButtonElement();
            this.btnAccionConfirmaCarga = new RumboSGA.RumButtonElement();
            this.btnAccionCerrarCarga = new RumboSGA.RumButtonElement();
            this.rBtnImprimirCarga = new RumboSGA.RumButtonElement();
            this.configCargaGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnVistaCargaConfiguracion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.temasCargaMenu = new RumboSGA.Controles.RumMenuItem();
            this.guardarMenuCarga = new RumboSGA.Controles.RumMenuItem();
            this.cargarMenuCarga = new RumboSGA.Controles.RumMenuItem();
            this.columnasCargaMenu = new RumboSGA.Controles.RumMenuItem();
            this.tableLayoutPanelCarga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridPedidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridPedidos.MasterTemplate)).BeginInit();
            this.rgvGridPedidos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdfFiltro)).BeginInit();
            this.rdfFiltro.SuspendLayout();
            this.tbDetalleCarga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBarAcciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelCarga
            // 
            this.tableLayoutPanelCarga.AutoSize = true;
            this.tableLayoutPanelCarga.ColumnCount = 3;
            this.tableLayoutPanelCarga.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelCarga.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanelCarga.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanelCarga.Controls.Add(this.rgvGridPedidos, 0, 0);
            this.tableLayoutPanelCarga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCarga.Location = new System.Drawing.Point(0, 163);
            this.tableLayoutPanelCarga.Name = "tableLayoutPanelCarga";
            this.tableLayoutPanelCarga.RowCount = 1;
            this.tableLayoutPanelCarga.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCarga.Size = new System.Drawing.Size(1152, 572);
            this.tableLayoutPanelCarga.TabIndex = 4;
            // 
            // rgvGridPedidos
            // 
            this.tableLayoutPanelCarga.SetColumnSpan(this.rgvGridPedidos, 3);
            this.rgvGridPedidos.Controls.Add(this.rdfFiltro);
            this.rgvGridPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvGridPedidos.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvGridPedidos.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvGridPedidos.Name = "rgvGridPedidos";
            this.rgvGridPedidos.Size = new System.Drawing.Size(1146, 566);
            this.rgvGridPedidos.TabIndex = 0;
            // 
            // rdfFiltro
            // 
            this.rdfFiltro.Controls.Add(this.tableLayoutPanelBotones);
            this.rdfFiltro.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.rdfFiltro.Location = new System.Drawing.Point(9, 3);
            this.rdfFiltro.Name = "rdfFiltro";
            this.rdfFiltro.Size = new System.Drawing.Size(636, 366);
            this.rdfFiltro.TabIndex = 1;
            // 
            // tableLayoutPanelBotones
            // 
            this.tableLayoutPanelBotones.ColumnCount = 1;
            this.tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBotones.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBotones.Name = "tableLayoutPanelBotones";
            this.tableLayoutPanelBotones.RowCount = 10;
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelBotones.Size = new System.Drawing.Size(636, 366);
            this.tableLayoutPanelBotones.TabIndex = 0;
            // 
            // tableLayoutPanelDetalleCarga
            // 
            this.tableLayoutPanelDetalleCarga.ColumnCount = 8;
            this.tableLayoutPanelDetalleCarga.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetalleCarga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDetalleCarga.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDetalleCarga.Name = "tableLayoutPanelDetalleCarga";
            this.tableLayoutPanelDetalleCarga.RowCount = 3;
            this.tableLayoutPanelDetalleCarga.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetalleCarga.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetalleCarga.Size = new System.Drawing.Size(949, 615);
            this.tableLayoutPanelDetalleCarga.TabIndex = 2;
            // 
            // tableLayoutPanelDetalleDatosTotal
            // 
            this.tableLayoutPanelDetalleDatosTotal.ColumnCount = 9;
            this.tableLayoutPanelDetalleDatosTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 500F));
            this.tableLayoutPanelDetalleDatosTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDetalleDatosTotal.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDetalleDatosTotal.Name = "tableLayoutPanelDetalleCarga";
            this.tableLayoutPanelDetalleDatosTotal.RowCount = 1;
            this.tableLayoutPanelDetalleDatosTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetalleDatosTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDetalleDatosTotal.Size = new System.Drawing.Size(949, 50);
            this.tableLayoutPanelDetalleDatosTotal.TabIndex = 2;
            // 
            // tbDetalleCarga
            // 
            this.tbDetalleCarga.Controls.Add(this.tabPackingList);
            this.tbDetalleCarga.Controls.Add(this.tabPedidos);
            this.tbDetalleCarga.Controls.Add(this.tabSalidas);
            this.tbDetalleCarga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDetalleCarga.Location = new System.Drawing.Point(0, 0);
            this.tbDetalleCarga.Name = "tbDetalleCarga";
            this.tbDetalleCarga.SelectedIndex = 0;
            this.tbDetalleCarga.Size = new System.Drawing.Size(949, 615);
            this.tbDetalleCarga.TabIndex = 2;
            // 
            // tabPackingList
            // 
            this.tabPackingList.Location = new System.Drawing.Point(4, 25);
            this.tabPackingList.Name = "tabPackingList";
            this.tabPackingList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPackingList.Size = new System.Drawing.Size(941, 586);
            this.tabPackingList.TabIndex = 0;
            this.tabPackingList.Text = "Packing List";
            this.tabPackingList.UseVisualStyleBackColor = true;
            // 
            // tabPedidos
            // 
            this.tabPedidos.Location = new System.Drawing.Point(4, 25);
            this.tabPedidos.Name = "tabPedidos";
            this.tabPedidos.Padding = new System.Windows.Forms.Padding(3);
            this.tabPedidos.Size = new System.Drawing.Size(941, 586);
            this.tabPedidos.TabIndex = 1;
            this.tabPedidos.Text = "Pedidos";
            this.tabPedidos.UseVisualStyleBackColor = true;
            // 
            // tabSalidas
            // 
            this.tabSalidas.Location = new System.Drawing.Point(4, 25);
            this.tabSalidas.Name = "tabSalidas";
            this.tabSalidas.Size = new System.Drawing.Size(941, 586);
            this.tabSalidas.TabIndex = 2;
            this.tabSalidas.Text = "Contenido";
            this.tabSalidas.UseVisualStyleBackColor = true;
            // 
            // radRibbonBarAcciones
            // 
            this.radRibbonBarAcciones.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTabAccionesPedidos,
            this.ribbonTabAccionesOrdenesRecogida,
            this.ribbonTabAccionesOrdenesCarga});
            this.radRibbonBarAcciones.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBarAcciones.Name = "radRibbonBarAcciones";
            this.radRibbonBarAcciones.SimplifiedHeight = 100;
            this.radRibbonBarAcciones.Size = new System.Drawing.Size(1152, 163);
            this.radRibbonBarAcciones.StartButtonImage = null;
            this.radRibbonBarAcciones.TabIndex = 5;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBarAcciones.GetChildAt(0))).SimplifiedHeight = 100;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBarAcciones.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBarAcciones.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTabAccionesPedidos
            // 
            this.ribbonTabAccionesPedidos.IsSelected = true;
            this.ribbonTabAccionesPedidos.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.vistaBarGroup,
            this.verGrupoPrincipal,
            this.prepBarGroup,
            this.rrbgVistaPedidosConfiguracion});
            this.ribbonTabAccionesPedidos.Name = "ribbonTabAccionesPedidos";
            this.ribbonTabAccionesPedidos.Text = "Acciones Pedidos";
            this.ribbonTabAccionesPedidos.UseMnemonic = false;
            // 
            // vistaBarGroup
            // 
            this.vistaBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnVistaPedidosPedidos,
            this.rBtnVistaPedidosOrdenes,
            this.rBtnVistaPedidosCarga,
            this.rBtnOrganizador});
            this.vistaBarGroup.Name = "vistaBarGroup";
            this.vistaBarGroup.Text = "Vistas";
            // 
            // rBtnVistaPedidosPedidos
            // 
            this.rBtnVistaPedidosPedidos.EnableFocusBorderAnimation = true;
            this.rBtnVistaPedidosPedidos.EnableRippleAnimation = true;
            this.rBtnVistaPedidosPedidos.Image = global::RumboSGA.Properties.Resources.Form;
            this.rBtnVistaPedidosPedidos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnVistaPedidosPedidos.Name = "rBtnVistaPedidosPedidos";
            this.rBtnVistaPedidosPedidos.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnVistaPedidosPedidos.Text = "Pedidos";
            this.rBtnVistaPedidosPedidos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnVistaPedidosPedidos.ToolTipText = "Pedidos";
            // 
            // rBtnVistaPedidosOrdenes
            // 
            this.rBtnVistaPedidosOrdenes.EnableFocusBorderAnimation = true;
            this.rBtnVistaPedidosOrdenes.EnableRippleAnimation = true;
            this.rBtnVistaPedidosOrdenes.Image = global::RumboSGA.Properties.Resources.EditList;
            this.rBtnVistaPedidosOrdenes.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnVistaPedidosOrdenes.Name = "rBtnVistaPedidosOrdenes";
            this.rBtnVistaPedidosOrdenes.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnVistaPedidosOrdenes.Text = "Ordenes de Recogida";
            this.rBtnVistaPedidosOrdenes.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnVistaPedidosOrdenes.ToolTipText = "Ordenes de Recogida";
            this.rBtnVistaPedidosOrdenes.Click += new System.EventHandler(this.btnVistaPedidosOrdenes_Click);
            // 
            // rBtnVistaPedidosCarga
            // 
            this.rBtnVistaPedidosCarga.EnableFocusBorderAnimation = true;
            this.rBtnVistaPedidosCarga.EnableRippleAnimation = true;
            this.rBtnVistaPedidosCarga.Image = global::RumboSGA.Properties.Resources.TransferReceipt;
            this.rBtnVistaPedidosCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnVistaPedidosCarga.Name = "rBtnVistaPedidosCarga";
            this.rBtnVistaPedidosCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnVistaPedidosCarga.Text = "Ordenes de Carga";
            this.rBtnVistaPedidosCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnVistaPedidosCarga.ToolTipText = "Ordenes de Carga";
            this.rBtnVistaPedidosCarga.Click += new System.EventHandler(this.btnVistaPedidosCarga_Click);
            // 
            // rBtnOrganizador
            // 
            this.rBtnOrganizador.EnableFocusBorderAnimation = true;
            this.rBtnOrganizador.EnableRippleAnimation = true;
            this.rBtnOrganizador.Image = global::RumboSGA.Properties.Resources.planificador;
            this.rBtnOrganizador.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnOrganizador.Name = "rBtnOrganizador";
            this.rBtnOrganizador.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnOrganizador.Text = "Organizador";
            this.rBtnOrganizador.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnOrganizador.ToolTipText = "Organizador";
            this.rBtnOrganizador.Click += new System.EventHandler(this.rBtnOrganizador_Click);
            // 
            // verGrupoPrincipal
            // 
            this.verGrupoPrincipal.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnddPedidosfiltradoOpciones,
            this.rBtnVistaPedidosExportar,
            this.rBtnVistaPedidosFiltrar,
            this.btnVistaPedidosQuitarFiltro,
            this.btnVistaPedidosRefrescar});
            this.verGrupoPrincipal.Name = "verGrupoPrincipal";
            this.verGrupoPrincipal.Text = "Ver";
            // 
            // rBtnddPedidosfiltradoOpciones
            // 
            this.rBtnddPedidosfiltradoOpciones.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rBtnddPedidosfiltradoOpciones.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rBtnddPedidosfiltradoOpciones.ExpandArrowButton = false;
            this.rBtnddPedidosfiltradoOpciones.Image = global::RumboSGA.Properties.Resources.Debug;
            this.rBtnddPedidosfiltradoOpciones.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnddPedidosfiltradoOpciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.ordenPendientesHoy,
            this.tareasHechasHoy,
            this.tareasPendientesMenu,
            this.bultosPendCarga,
            this.ordenCargaAbiertas});
            this.rBtnddPedidosfiltradoOpciones.Name = "rBtnddPedidosfiltradoOpciones";
            this.rBtnddPedidosfiltradoOpciones.Text = "Filtrado Por";
            this.rBtnddPedidosfiltradoOpciones.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnddPedidosfiltradoOpciones.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // ordenPendientesHoy
            // 
            this.ordenPendientesHoy.Name = "ordenPendientesHoy";
            this.ordenPendientesHoy.Text = "";
            // 
            // tareasHechasHoy
            // 
            this.tareasHechasHoy.Name = "tareasHechasHoy";
            this.tareasHechasHoy.Text = "";
            // 
            // tareasPendientesMenu
            // 
            this.tareasPendientesMenu.Name = "tareasPendientesMenu";
            this.tareasPendientesMenu.Text = "";
            // 
            // bultosPendCarga
            // 
            this.bultosPendCarga.Name = "bultosPendCarga";
            this.bultosPendCarga.Text = "";
            // 
            // ordenCargaAbiertas
            // 
            this.ordenCargaAbiertas.Name = "ordenCargaAbiertas";
            this.ordenCargaAbiertas.Text = "";
            // 
            // rBtnVistaPedidosExportar
            // 
            this.rBtnVistaPedidosExportar.EnableFocusBorderAnimation = true;
            this.rBtnVistaPedidosExportar.EnableRippleAnimation = true;
            this.rBtnVistaPedidosExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rBtnVistaPedidosExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnVistaPedidosExportar.Name = "rBtnVistaPedidosExportar";
            this.rBtnVistaPedidosExportar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnVistaPedidosExportar.Text = "";
            this.rBtnVistaPedidosExportar.Click += new System.EventHandler(this.btnVistaPedidosExportar_Click);
            // 
            // rBtnVistaPedidosFiltrar
            // 
            this.rBtnVistaPedidosFiltrar.EnableFocusBorderAnimation = true;
            this.rBtnVistaPedidosFiltrar.EnableRippleAnimation = true;
            this.rBtnVistaPedidosFiltrar.Image = global::RumboSGA.Properties.Resources.Filter;
            this.rBtnVistaPedidosFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnVistaPedidosFiltrar.Name = "rBtnVistaPedidosFiltrar";
            this.rBtnVistaPedidosFiltrar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnVistaPedidosFiltrar.Text = "";
            this.rBtnVistaPedidosFiltrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnVistaPedidosFiltrar.Click += new System.EventHandler(this.btnVistaPedidosFiltrar_Click);
            // 
            // btnVistaPedidosQuitarFiltro
            // 
            this.btnVistaPedidosQuitarFiltro.EnableFocusBorderAnimation = true;
            this.btnVistaPedidosQuitarFiltro.EnableRippleAnimation = true;
            this.btnVistaPedidosQuitarFiltro.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnVistaPedidosQuitarFiltro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaPedidosQuitarFiltro.Name = "btnVistaPedidosQuitarFiltro";
            this.btnVistaPedidosQuitarFiltro.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaPedidosQuitarFiltro.Text = "";
            this.btnVistaPedidosQuitarFiltro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaPedidosQuitarFiltro.Click += new System.EventHandler(this.btnVistaPedidosQuitarFiltro_Click);
            // 
            // btnVistaPedidosRefrescar
            // 
            this.btnVistaPedidosRefrescar.EnableFocusBorderAnimation = true;
            this.btnVistaPedidosRefrescar.EnableRippleAnimation = true;
            this.btnVistaPedidosRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnVistaPedidosRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaPedidosRefrescar.Name = "btnVistaPedidosRefrescar";
            this.btnVistaPedidosRefrescar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaPedidosRefrescar.Text = "";
            this.btnVistaPedidosRefrescar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaPedidosRefrescar.Click += new System.EventHandler(this.btnVistaPedidosRefrescar_Click);
            // 
            // prepBarGroup
            // 
            this.prepBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnAccionLiberarPedido,
            this.btnAccionLanzamientoPedido,
            this.btnAccionLanzamientoAgrupado,
            this.btnAccionLanzammientoOla,
            this.rBtnCerrarPedido,
            this.chkRecursoManual});
            this.prepBarGroup.Name = "prepBarGroup";
            this.prepBarGroup.Text = "Acciones";
            // 
            // btnAccionLiberarPedido
            // 
            this.btnAccionLiberarPedido.EnableFocusBorderAnimation = true;
            this.btnAccionLiberarPedido.EnableRippleAnimation = true;
            this.btnAccionLiberarPedido.Image = global::RumboSGA.Properties.Resources.GoTo;
            this.btnAccionLiberarPedido.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionLiberarPedido.Name = "btnAccionLiberarPedido";
            this.btnAccionLiberarPedido.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionLiberarPedido.Text = "Liberar";
            this.btnAccionLiberarPedido.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionLiberarPedido.ToolTipText = "Liberar";
            this.btnAccionLiberarPedido.Click += new System.EventHandler(this.rBtnAccionPedidosLiberar_Click);
            // 
            // btnAccionLanzamientoPedido
            // 
            this.btnAccionLanzamientoPedido.AutoSize = true;
            this.btnAccionLanzamientoPedido.EnableFocusBorderAnimation = true;
            this.btnAccionLanzamientoPedido.EnableRippleAnimation = true;
            this.btnAccionLanzamientoPedido.Image = global::RumboSGA.Properties.Resources.edit;
            this.btnAccionLanzamientoPedido.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionLanzamientoPedido.Name = "btnAccionLanzamientoPedido";
            this.btnAccionLanzamientoPedido.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionLanzamientoPedido.Text = "Lanzar";
            this.btnAccionLanzamientoPedido.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionLanzamientoPedido.ToolTipText = "Lanzar";
            this.btnAccionLanzamientoPedido.UseCompatibleTextRendering = false;
            this.btnAccionLanzamientoPedido.Click += new System.EventHandler(this.rBtnAccionLanzamientoPedido_Click);
            // 
            // btnAccionLanzamientoAgrupado
            // 
            this.btnAccionLanzamientoAgrupado.AutoSize = true;
            this.btnAccionLanzamientoAgrupado.EnableFocusBorderAnimation = true;
            this.btnAccionLanzamientoAgrupado.EnableRippleAnimation = true;
            this.btnAccionLanzamientoAgrupado.Image = global::RumboSGA.Properties.Resources.Table;
            this.btnAccionLanzamientoAgrupado.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionLanzamientoAgrupado.Name = "btnAccionLanzamientoAgrupado";
            this.btnAccionLanzamientoAgrupado.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionLanzamientoAgrupado.Text = "Agrupada";
            this.btnAccionLanzamientoAgrupado.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionLanzamientoAgrupado.ToolTipText = "Agrupada";
            this.btnAccionLanzamientoAgrupado.Click += new System.EventHandler(this.btnAccionLanzamientoAgrupado_Click);
            // 
            // btnAccionLanzammientoOla
            // 
            this.btnAccionLanzammientoOla.AutoSize = true;
            this.btnAccionLanzammientoOla.EnableFocusBorderAnimation = true;
            this.btnAccionLanzammientoOla.EnableRippleAnimation = true;
            this.btnAccionLanzammientoOla.Image = global::RumboSGA.Properties.Resources.SetupColumns;
            this.btnAccionLanzammientoOla.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionLanzammientoOla.Name = "btnAccionLanzammientoOla";
            this.btnAccionLanzammientoOla.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionLanzammientoOla.Text = "Por Ola";
            this.btnAccionLanzammientoOla.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionLanzammientoOla.ToolTipText = "Por Ola";
            // 
            // rBtnCerrarPedido
            // 
            this.rBtnCerrarPedido.EnableFocusBorderAnimation = true;
            this.rBtnCerrarPedido.EnableRippleAnimation = true;
            this.rBtnCerrarPedido.Image = global::RumboSGA.Properties.Resources.Close;
            this.rBtnCerrarPedido.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCerrarPedido.Name = "rBtnCerrarPedido";
            this.rBtnCerrarPedido.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnCerrarPedido.Text = "Cerrar Pedido";
            this.rBtnCerrarPedido.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCerrarPedido.ToolTipText = "Cerrar Pedido";
            this.rBtnCerrarPedido.Click += new System.EventHandler(this.rBtnCerrarPedido_Click);
            // 
            // chkRecursoManual
            // 
            this.chkRecursoManual.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRecursoManual.Checked = false;
            this.chkRecursoManual.Name = "chkRecursoManual";
            this.chkRecursoManual.ReadOnly = false;
            this.chkRecursoManual.StretchVertically = false;
            this.chkRecursoManual.Text = "Recurso Manual";
            this.chkRecursoManual.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rrbgVistaPedidosConfiguracion
            // 
            this.rrbgVistaPedidosConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnVistaPedidosConfiguracion});
            this.rrbgVistaPedidosConfiguracion.Name = "rrbgVistaPedidosConfiguracion";
            this.rrbgVistaPedidosConfiguracion.Text = "Configuración";
            // 
            // btnVistaPedidosConfiguracion
            // 
            this.btnVistaPedidosConfiguracion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.btnVistaPedidosConfiguracion.AutoSize = false;
            this.btnVistaPedidosConfiguracion.Bounds = new System.Drawing.Rectangle(0, 0, 62, 62);
            this.btnVistaPedidosConfiguracion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.btnVistaPedidosConfiguracion.ExpandArrowButton = false;
            this.btnVistaPedidosConfiguracion.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnVistaPedidosConfiguracion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaPedidosConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenu,
            this.guardarMenu,
            this.cargarMenu,
            this.editColumns,
            this.ImprimirEtiquetasPackingList});
            this.btnVistaPedidosConfiguracion.Name = "btnVistaPedidosConfiguracion";
            this.btnVistaPedidosConfiguracion.Text = "";
            this.btnVistaPedidosConfiguracion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaPedidosConfiguracion.Click += new System.EventHandler(this.btnVistaPedidosConfiguracion_Click);
            // 
            // temasMenu
            // 
            this.temasMenu.Name = "temasMenu";
            this.temasMenu.Text = "Temas";
            // 
            // guardarMenu
            // 
            this.guardarMenu.Name = "guardarMenu";
            this.guardarMenu.Text = "Guardar Estilo";
            this.guardarMenu.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarMenu
            // 
            this.cargarMenu.Name = "cargarMenu";
            this.cargarMenu.Text = "Cargar Estilo";
            this.cargarMenu.Click += new System.EventHandler(this.LoadItem_Click);
            // 
            // editColumns
            // 
            this.editColumns.Name = "editColumns";
            this.editColumns.Text = "Editar Columnas";
            this.editColumns.Click += new System.EventHandler(this.ItemColumnas_Click);
            // 
            // ImprimirEtiquetasPackingList
            // 
            this.ImprimirEtiquetasPackingList.Name = "ImprimirEtiquetasPackingList";
            this.ImprimirEtiquetasPackingList.Text = "Imprimir Etiquetas PackingList";
            this.ImprimirEtiquetasPackingList.Click += new System.EventHandler(this.ImprimirEtiquetasPackingList_Click);
            // 
            // ribbonTabAccionesOrdenesRecogida
            // 
            this.ribbonTabAccionesOrdenesRecogida.IsSelected = false;
            this.ribbonTabAccionesOrdenesRecogida.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonbarVistaRecogida,
            this.verRecogidaGrupo,
            this.accionRecogBarGroup,
            this.configRecogidaGroup});
            this.ribbonTabAccionesOrdenesRecogida.Name = "ribbonTabAccionesOrdenesRecogida";
            this.ribbonTabAccionesOrdenesRecogida.Text = "Acciones Recogida";
            this.ribbonTabAccionesOrdenesRecogida.UseMnemonic = false;
            // 
            // ribbonbarVistaRecogida
            // 
            this.ribbonbarVistaRecogida.CollapsingPriority = 1;
            this.ribbonbarVistaRecogida.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnVistaOrdenesPedidos,
            this.btnVistaOrdenesOrdenes,
            this.btnVistaOrdenesCarga});
            this.ribbonbarVistaRecogida.Name = "ribbonbarVistaRecogida";
            this.ribbonbarVistaRecogida.Text = "Vistas";
            // 
            // btnVistaOrdenesPedidos
            // 
            this.btnVistaOrdenesPedidos.EnableFocusBorderAnimation = true;
            this.btnVistaOrdenesPedidos.EnableRippleAnimation = true;
            this.btnVistaOrdenesPedidos.Image = global::RumboSGA.Properties.Resources.Form;
            this.btnVistaOrdenesPedidos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaOrdenesPedidos.Name = "btnVistaOrdenesPedidos";
            this.btnVistaOrdenesPedidos.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaOrdenesPedidos.Text = "Pedidos";
            this.btnVistaOrdenesPedidos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaOrdenesPedidos.ToolTipText = "Pedidos";
            this.btnVistaOrdenesPedidos.Click += new System.EventHandler(this.btnVistaOrdenesPedidos_Click);
            // 
            // btnVistaOrdenesOrdenes
            // 
            this.btnVistaOrdenesOrdenes.EnableFocusBorderAnimation = true;
            this.btnVistaOrdenesOrdenes.EnableRippleAnimation = true;
            this.btnVistaOrdenesOrdenes.Image = global::RumboSGA.Properties.Resources.EditList;
            this.btnVistaOrdenesOrdenes.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaOrdenesOrdenes.Name = "btnVistaOrdenesOrdenes";
            this.btnVistaOrdenesOrdenes.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaOrdenesOrdenes.Text = "Ordenes de Recogida";
            this.btnVistaOrdenesOrdenes.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaOrdenesOrdenes.ToolTipText = "Ordenes de Recogida";
            // 
            // btnVistaOrdenesCarga
            // 
            this.btnVistaOrdenesCarga.EnableFocusBorderAnimation = true;
            this.btnVistaOrdenesCarga.EnableRippleAnimation = true;
            this.btnVistaOrdenesCarga.Image = global::RumboSGA.Properties.Resources.TransferReceipt;
            this.btnVistaOrdenesCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaOrdenesCarga.Name = "btnVistaOrdenesCarga";
            this.btnVistaOrdenesCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaOrdenesCarga.Text = "Ordenes de Carga";
            this.btnVistaOrdenesCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaOrdenesCarga.ToolTipText = "Ordenes de Carga";
            this.btnVistaOrdenesCarga.Click += new System.EventHandler(this.btnVistaOrdenesOrdenesCarga_Click);
            // 
            // verRecogidaGrupo
            // 
            this.verRecogidaGrupo.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnVistaOrdenesExportar,
            this.btnVistaOrdenesFiltrar,
            this.btnBorrarFiltroOrdenes,
            this.btnVistaOrdenesRefrescar});
            this.verRecogidaGrupo.Name = "verRecogidaGrupo";
            this.verRecogidaGrupo.Text = "Ver";
            // 
            // btnVistaOrdenesExportar
            // 
            this.btnVistaOrdenesExportar.EnableFocusBorderAnimation = true;
            this.btnVistaOrdenesExportar.EnableRippleAnimation = true;
            this.btnVistaOrdenesExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.btnVistaOrdenesExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaOrdenesExportar.Name = "btnVistaOrdenesExportar";
            this.btnVistaOrdenesExportar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaOrdenesExportar.Text = "";
            this.btnVistaOrdenesExportar.Click += new System.EventHandler(this.btnVistaOrdenesExportar_Click);
            // 
            // btnVistaOrdenesFiltrar
            // 
            this.btnVistaOrdenesFiltrar.EnableFocusBorderAnimation = true;
            this.btnVistaOrdenesFiltrar.EnableRippleAnimation = true;
            this.btnVistaOrdenesFiltrar.Image = global::RumboSGA.Properties.Resources.Filter;
            this.btnVistaOrdenesFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaOrdenesFiltrar.Name = "btnVistaOrdenesFiltrar";
            this.btnVistaOrdenesFiltrar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaOrdenesFiltrar.Text = "";
            this.btnVistaOrdenesFiltrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaOrdenesFiltrar.Click += new System.EventHandler(this.btnVistaOrdenesFiltrar_Click);
            // 
            // btnBorrarFiltroOrdenes
            // 
            this.btnBorrarFiltroOrdenes.EnableFocusBorderAnimation = true;
            this.btnBorrarFiltroOrdenes.EnableRippleAnimation = true;
            this.btnBorrarFiltroOrdenes.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnBorrarFiltroOrdenes.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBorrarFiltroOrdenes.Name = "btnBorrarFiltroOrdenes";
            this.btnBorrarFiltroOrdenes.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnBorrarFiltroOrdenes.Text = "";
            this.btnBorrarFiltroOrdenes.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBorrarFiltroOrdenes.Click += new System.EventHandler(this.btnVistaOrdenesQuitarFiltro_Click);
            // 
            // btnVistaOrdenesRefrescar
            // 
            this.btnVistaOrdenesRefrescar.BackColor = System.Drawing.Color.Transparent;
            this.btnVistaOrdenesRefrescar.BorderHighlightColor = System.Drawing.Color.Transparent;
            this.btnVistaOrdenesRefrescar.EnableFocusBorderAnimation = true;
            this.btnVistaOrdenesRefrescar.EnableRippleAnimation = true;
            this.btnVistaOrdenesRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnVistaOrdenesRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaOrdenesRefrescar.Name = "btnVistaOrdenesRefrescar";
            this.btnVistaOrdenesRefrescar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaOrdenesRefrescar.Text = "";
            this.btnVistaOrdenesRefrescar.Click += new System.EventHandler(this.btnVistaOrdenesRefrescar_Click);
            // 
            // accionRecogBarGroup
            // 
            this.accionRecogBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnAccionReservar,
            this.btnAccionServir,
            this.rBtnServirSeleccionadas,
            this.btnAccionCancelarReservar,
            this.btnCancelarReservaSeleccionadas,
            this.btnModificarReserva,
            this.btnCambiarExistencia,
            this.btnAccionImprimirInforme,
            this.rBtnAsignarMuelle,
            this.rBtnCerrarOrden});
            this.accionRecogBarGroup.Name = "accionRecogBarGroup";
            this.accionRecogBarGroup.Text = "Acciones";
            // 
            // btnAccionReservar
            // 
            this.btnAccionReservar.EnableFocusBorderAnimation = true;
            this.btnAccionReservar.EnableRippleAnimation = true;
            this.btnAccionReservar.Image = global::RumboSGA.Properties.Resources.Task;
            this.btnAccionReservar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionReservar.Name = "btnAccionReservar";
            this.btnAccionReservar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionReservar.Text = "Reservar";
            this.btnAccionReservar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionReservar.ToolTipText = "Reservar";
            this.btnAccionReservar.Click += new System.EventHandler(this.btnAccionReservar_Click);
            // 
            // btnAccionServir
            // 
            this.btnAccionServir.EnableFocusBorderAnimation = true;
            this.btnAccionServir.EnableRippleAnimation = true;
            this.btnAccionServir.Image = global::RumboSGA.Properties.Resources.GoTo;
            this.btnAccionServir.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionServir.Name = "btnAccionServir";
            this.btnAccionServir.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionServir.Text = "Servir Reserva";
            this.btnAccionServir.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionServir.ToolTipText = "Servir Reserva";
            this.btnAccionServir.Click += new System.EventHandler(this.rBtnAccionServir_Click);
            // 
            // rBtnServirSeleccionadas
            // 
            this.rBtnServirSeleccionadas.EnableFocusBorderAnimation = true;
            this.rBtnServirSeleccionadas.EnableRippleAnimation = true;
            this.rBtnServirSeleccionadas.Image = global::RumboSGA.Properties.Resources.GoTo;
            this.rBtnServirSeleccionadas.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnServirSeleccionadas.Name = "rBtnServirSeleccionadas";
            this.rBtnServirSeleccionadas.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnServirSeleccionadas.Text = "Servir Seleccionadas";
            this.rBtnServirSeleccionadas.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnServirSeleccionadas.ToolTipText = "Servir Seleccionadas";
            this.rBtnServirSeleccionadas.Click += new System.EventHandler(this.rBtnServirSeleccionadas_Click);
            // 
            // btnAccionCancelarReservar
            // 
            this.btnAccionCancelarReservar.EnableFocusBorderAnimation = true;
            this.btnAccionCancelarReservar.EnableRippleAnimation = true;
            this.btnAccionCancelarReservar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnAccionCancelarReservar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionCancelarReservar.Name = "btnAccionCancelarReservar";
            this.btnAccionCancelarReservar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionCancelarReservar.Text = "Cancelar Reserva";
            this.btnAccionCancelarReservar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionCancelarReservar.ToolTipText = "Cancelar Reserva";
            this.btnAccionCancelarReservar.Click += new System.EventHandler(this.rBtnAccionCancelarReservar_Click);
            // 
            // btnCancelarReservaSeleccionadas
            // 
            this.btnCancelarReservaSeleccionadas.EnableFocusBorderAnimation = true;
            this.btnCancelarReservaSeleccionadas.EnableRippleAnimation = true;
            this.btnCancelarReservaSeleccionadas.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelarReservaSeleccionadas.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelarReservaSeleccionadas.Name = "btnCancelarReservaSeleccionadas";
            this.btnCancelarReservaSeleccionadas.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCancelarReservaSeleccionadas.Text = "Canc Res. Seleccionadas";
            this.btnCancelarReservaSeleccionadas.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancelarReservaSeleccionadas.ToolTipText = "Canc Res. Seleccionadas";
            this.btnCancelarReservaSeleccionadas.Click += new System.EventHandler(this.rBtnCancelarReservaSeleccionadas_Click);
            // 
            // btnModificarReserva
            // 
            this.btnModificarReserva.EnableFocusBorderAnimation = true;
            this.btnModificarReserva.EnableRippleAnimation = true;
            this.btnModificarReserva.Image = global::RumboSGA.Properties.Resources.edit;
            this.btnModificarReserva.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnModificarReserva.Name = "btnModificarReserva";
            this.btnModificarReserva.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnModificarReserva.Text = "Modificar Reserva";
            this.btnModificarReserva.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnModificarReserva.ToolTipText = "Modificar Reserva";
            this.btnModificarReserva.Click += new System.EventHandler(this.btnModificarReserva_Click);
            // 
            // btnCambiarExistencia
            // 
            this.btnCambiarExistencia.EnableFocusBorderAnimation = true;
            this.btnCambiarExistencia.EnableRippleAnimation = true;
            this.btnCambiarExistencia.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.btnCambiarExistencia.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCambiarExistencia.Name = "btnCambiarExistencia";
            this.btnCambiarExistencia.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCambiarExistencia.Text = "Cambiar Existencia";
            this.btnCambiarExistencia.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCambiarExistencia.ToolTipText = "Cambiar Existencia";
            this.btnCambiarExistencia.Click += new System.EventHandler(this.btnCambiarExistencia_Click);
            // 
            // btnAccionImprimirInforme
            // 
            this.btnAccionImprimirInforme.EnableFocusBorderAnimation = true;
            this.btnAccionImprimirInforme.EnableRippleAnimation = true;
            this.btnAccionImprimirInforme.Image = global::RumboSGA.Properties.Resources.printer;
            this.btnAccionImprimirInforme.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionImprimirInforme.Name = "btnAccionImprimirInforme";
            this.btnAccionImprimirInforme.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionImprimirInforme.Text = "Imprimir";
            this.btnAccionImprimirInforme.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionImprimirInforme.ToolTipText = "Imprimir";
            this.btnAccionImprimirInforme.Click += new System.EventHandler(this.rBtnAccionImprimirInforme_Click);
            // 
            // rBtnAsignarMuelle
            // 
            this.rBtnAsignarMuelle.EnableFocusBorderAnimation = true;
            this.rBtnAsignarMuelle.EnableRippleAnimation = true;
            this.rBtnAsignarMuelle.Image = global::RumboSGA.Properties.Resources.TransferReceipt;
            this.rBtnAsignarMuelle.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnAsignarMuelle.Name = "rBtnAsignarMuelle";
            this.rBtnAsignarMuelle.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnAsignarMuelle.Text = "Asignar Muelle";
            this.rBtnAsignarMuelle.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnAsignarMuelle.ToolTipText = "Asignar Muelle";
            this.rBtnAsignarMuelle.Click += new System.EventHandler(this.rBtnAsignarMuelle_Click);
            // 
            // rBtnCerrarOrden
            // 
            this.rBtnCerrarOrden.EnableFocusBorderAnimation = true;
            this.rBtnCerrarOrden.EnableRippleAnimation = true;
            this.rBtnCerrarOrden.Image = global::RumboSGA.Properties.Resources.Close;
            this.rBtnCerrarOrden.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCerrarOrden.Name = "rBtnCerrarOrden";
            this.rBtnCerrarOrden.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnCerrarOrden.Text = "Cerrar Orden";
            this.rBtnCerrarOrden.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCerrarOrden.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.rBtnCerrarOrden.ToolTipText = "Cerrar Orden";
            this.rBtnCerrarOrden.Click += new System.EventHandler(this.RBtnCerrarOrden_Click);
            // 
            // configRecogidaGroup
            // 
            this.configRecogidaGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnVistaOrdenesConfiguracion});
            this.configRecogidaGroup.Name = "configRecogidaGroup";
            this.configRecogidaGroup.Text = "Configuración";
            // 
            // btnVistaOrdenesConfiguracion
            // 
            this.btnVistaOrdenesConfiguracion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.btnVistaOrdenesConfiguracion.AutoSize = false;
            this.btnVistaOrdenesConfiguracion.Bounds = new System.Drawing.Rectangle(0, 0, 62, 62);
            this.btnVistaOrdenesConfiguracion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.btnVistaOrdenesConfiguracion.ExpandArrowButton = false;
            this.btnVistaOrdenesConfiguracion.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnVistaOrdenesConfiguracion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaOrdenesConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasRecogidaMenu,
            this.guardarMenuRecogida,
            this.cargarMenuRecogida,
            this.columnRecogMenu});
            this.btnVistaOrdenesConfiguracion.Name = "btnVistaOrdenesConfiguracion";
            this.btnVistaOrdenesConfiguracion.Text = "";
            // 
            // temasRecogidaMenu
            // 
            this.temasRecogidaMenu.Name = "temasRecogidaMenu";
            this.temasRecogidaMenu.Text = "Temas";
            // 
            // guardarMenuRecogida
            // 
            this.guardarMenuRecogida.Name = "guardarMenuRecogida";
            this.guardarMenuRecogida.Text = "Guardar Estilo";
            this.guardarMenuRecogida.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarMenuRecogida
            // 
            this.cargarMenuRecogida.Name = "cargarMenuRecogida";
            this.cargarMenuRecogida.Text = "Cargar Estilo";
            this.cargarMenuRecogida.Click += new System.EventHandler(this.LoadItem_Click);
            // 
            // columnRecogMenu
            // 
            this.columnRecogMenu.Name = "columnRecogMenu";
            this.columnRecogMenu.Text = "Columnas";
            this.columnRecogMenu.Click += new System.EventHandler(this.ItemColumnas_ClickRecogida);
            // 
            // ribbonTabAccionesOrdenesCarga
            // 
            this.ribbonTabAccionesOrdenesCarga.IsSelected = false;
            this.ribbonTabAccionesOrdenesCarga.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.vistaCargaGrupo,
            this.verCargaGrupo,
            this.accionesCarga,
            this.configCargaGroup});
            this.ribbonTabAccionesOrdenesCarga.Name = "ribbonTabAccionesOrdenesCarga";
            this.ribbonTabAccionesOrdenesCarga.Text = "Acciones Carga";
            this.ribbonTabAccionesOrdenesCarga.UseMnemonic = false;
            // 
            // vistaCargaGrupo
            // 
            this.vistaCargaGrupo.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnVistaCargaPedidos,
            this.btnVistaCargaOrdenes,
            this.btnVistaCargaCarga});
            this.vistaCargaGrupo.Name = "vistaCargaGrupo";
            this.vistaCargaGrupo.Text = "Vistas";
            // 
            // btnVistaCargaPedidos
            // 
            this.btnVistaCargaPedidos.EnableFocusBorderAnimation = true;
            this.btnVistaCargaPedidos.EnableRippleAnimation = true;
            this.btnVistaCargaPedidos.Image = global::RumboSGA.Properties.Resources.Form;
            this.btnVistaCargaPedidos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaPedidos.Name = "btnVistaCargaPedidos";
            this.btnVistaCargaPedidos.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaCargaPedidos.Text = "Pedidos";
            this.btnVistaCargaPedidos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaCargaPedidos.ToolTipText = "Pedidos";
            this.btnVistaCargaPedidos.Click += new System.EventHandler(this.btnVistaCargaPedidos_Click);
            // 
            // btnVistaCargaOrdenes
            // 
            this.btnVistaCargaOrdenes.EnableFocusBorderAnimation = true;
            this.btnVistaCargaOrdenes.EnableRippleAnimation = true;
            this.btnVistaCargaOrdenes.Image = global::RumboSGA.Properties.Resources.EditList;
            this.btnVistaCargaOrdenes.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaOrdenes.Name = "btnVistaCargaOrdenes";
            this.btnVistaCargaOrdenes.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaCargaOrdenes.Text = "Ordenes de Recogida";
            this.btnVistaCargaOrdenes.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaCargaOrdenes.ToolTipText = "Ordenes de Recogida";
            this.btnVistaCargaOrdenes.Click += new System.EventHandler(this.btnVistaCargaOrdenesRecogida_Click);
            // 
            // btnVistaCargaCarga
            // 
            this.btnVistaCargaCarga.EnableFocusBorderAnimation = true;
            this.btnVistaCargaCarga.EnableRippleAnimation = true;
            this.btnVistaCargaCarga.Image = global::RumboSGA.Properties.Resources.TransferReceipt;
            this.btnVistaCargaCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaCarga.Name = "btnVistaCargaCarga";
            this.btnVistaCargaCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaCargaCarga.Text = "Ordenes de Carga";
            this.btnVistaCargaCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaCargaCarga.ToolTipText = "Ordenes de Carga";
            this.btnVistaCargaCarga.Click += new System.EventHandler(this.btnVistaCargaCarga_Click);
            // 
            // verCargaGrupo
            // 
            this.verCargaGrupo.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnVistaCargaExportar,
            this.btnVistaCargaFiltrar,
            this.btnVistaCargaBorrarFiltro,
            this.btnVistaCargaRefrescar});
            this.verCargaGrupo.Name = "verCargaGrupo";
            this.verCargaGrupo.Text = "Ver";
            // 
            // btnVistaCargaExportar
            // 
            this.btnVistaCargaExportar.EnableFocusBorderAnimation = true;
            this.btnVistaCargaExportar.EnableRippleAnimation = true;
            this.btnVistaCargaExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.btnVistaCargaExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaExportar.Name = "btnVistaCargaExportar";
            this.btnVistaCargaExportar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaCargaExportar.Text = "";
            this.btnVistaCargaExportar.Click += new System.EventHandler(this.btnVistaCargaExportar_Click);
            // 
            // btnVistaCargaFiltrar
            // 
            this.btnVistaCargaFiltrar.EnableFocusBorderAnimation = true;
            this.btnVistaCargaFiltrar.EnableRippleAnimation = true;
            this.btnVistaCargaFiltrar.Image = global::RumboSGA.Properties.Resources.Filter;
            this.btnVistaCargaFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaFiltrar.Name = "btnVistaCargaFiltrar";
            this.btnVistaCargaFiltrar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaCargaFiltrar.Text = "";
            this.btnVistaCargaFiltrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaCargaFiltrar.Click += new System.EventHandler(this.btnVistaCargaFiltrar_Click);
            // 
            // btnVistaCargaBorrarFiltro
            // 
            this.btnVistaCargaBorrarFiltro.EnableFocusBorderAnimation = true;
            this.btnVistaCargaBorrarFiltro.EnableRippleAnimation = true;
            this.btnVistaCargaBorrarFiltro.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnVistaCargaBorrarFiltro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaBorrarFiltro.Name = "btnVistaCargaBorrarFiltro";
            this.btnVistaCargaBorrarFiltro.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaCargaBorrarFiltro.Text = "";
            this.btnVistaCargaBorrarFiltro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaCargaBorrarFiltro.Click += new System.EventHandler(this.btnVistaCargaQuitarFiltro_Click);
            // 
            // btnVistaCargaRefrescar
            // 
            this.btnVistaCargaRefrescar.EnableFocusBorderAnimation = true;
            this.btnVistaCargaRefrescar.EnableRippleAnimation = true;
            this.btnVistaCargaRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnVistaCargaRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaRefrescar.Name = "btnVistaCargaRefrescar";
            this.btnVistaCargaRefrescar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVistaCargaRefrescar.Text = "";
            this.btnVistaCargaRefrescar.Click += new System.EventHandler(this.btnVistaCargaRefrescar_Click);
            // 
            // accionesCarga
            // 
            this.accionesCarga.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.bntAccionCrearCarga,
            this.btnAccionGenerarTareaCarga,
            this.btnAccionConfirmaCarga,
            this.btnAccionCerrarCarga,
            this.rBtnImprimirCarga});
            this.accionesCarga.Name = "accionesCarga";
            this.accionesCarga.Text = "Acciones";
            // 
            // bntAccionCrearCarga
            // 
            this.bntAccionCrearCarga.EnableFocusBorderAnimation = true;
            this.bntAccionCrearCarga.EnableRippleAnimation = true;
            this.bntAccionCrearCarga.Image = global::RumboSGA.Properties.Resources.Add;
            this.bntAccionCrearCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.bntAccionCrearCarga.Name = "bntAccionCrearCarga";
            this.bntAccionCrearCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bntAccionCrearCarga.Text = "Crear Carga";
            this.bntAccionCrearCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.bntAccionCrearCarga.ToolTipText = "Crear Carga";
            this.bntAccionCrearCarga.Click += new System.EventHandler(this.btnAccionCrearCargaButton_Click);
            // 
            // btnAccionGenerarTareaCarga
            // 
            this.btnAccionGenerarTareaCarga.EnableFocusBorderAnimation = true;
            this.btnAccionGenerarTareaCarga.EnableRippleAnimation = true;
            this.btnAccionGenerarTareaCarga.Image = global::RumboSGA.Properties.Resources.CopyFromTask;
            this.btnAccionGenerarTareaCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionGenerarTareaCarga.Name = "btnAccionGenerarTareaCarga";
            this.btnAccionGenerarTareaCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionGenerarTareaCarga.Text = "Generar Tarea";
            this.btnAccionGenerarTareaCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionGenerarTareaCarga.ToolTipText = "Generar Tarea";
            this.btnAccionGenerarTareaCarga.UseCompatibleTextRendering = false;
            this.btnAccionGenerarTareaCarga.Click += new System.EventHandler(this.btnAccionGenerarTarea_Click);
            // 
            // btnAccionConfirmaCarga
            // 
            this.btnAccionConfirmaCarga.EnableFocusBorderAnimation = true;
            this.btnAccionConfirmaCarga.EnableRippleAnimation = true;
            this.btnAccionConfirmaCarga.Image = global::RumboSGA.Properties.Resources.truck;
            this.btnAccionConfirmaCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionConfirmaCarga.Name = "btnAccionConfirmaCarga";
            this.btnAccionConfirmaCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionConfirmaCarga.Text = "Confirmar Carga";
            this.btnAccionConfirmaCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionConfirmaCarga.ToolTipText = "Confirmar Carga";
            this.btnAccionConfirmaCarga.Click += new System.EventHandler(this.btnAccionConfirmaCarga_Click);
            // 
            // btnAccionCerrarCarga
            // 
            this.btnAccionCerrarCarga.EnableFocusBorderAnimation = true;
            this.btnAccionCerrarCarga.EnableRippleAnimation = true;
            this.btnAccionCerrarCarga.Image = global::RumboSGA.Properties.Resources.Close;
            this.btnAccionCerrarCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAccionCerrarCarga.Name = "btnAccionCerrarCarga";
            this.btnAccionCerrarCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAccionCerrarCarga.Text = "Cerrar Carga";
            this.btnAccionCerrarCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAccionCerrarCarga.ToolTipText = "Cerrar Carga";
            this.btnAccionCerrarCarga.Click += new System.EventHandler(this.btnAccionCerrarCarga_Click);
            // 
            // rBtnImprimirCarga
            // 
            this.rBtnImprimirCarga.EnableFocusBorderAnimation = true;
            this.rBtnImprimirCarga.EnableRippleAnimation = true;
            this.rBtnImprimirCarga.Image = global::RumboSGA.Properties.Resources.printer;
            this.rBtnImprimirCarga.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnImprimirCarga.Name = "rBtnImprimirCarga";
            this.rBtnImprimirCarga.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnImprimirCarga.Text = "Imprimir";
            this.rBtnImprimirCarga.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnImprimirCarga.ToolTipText = "Imprimir";
            this.rBtnImprimirCarga.Click += new System.EventHandler(this.rBtnImprimirCarga_Click);
            // 
            // configCargaGroup
            // 
            this.configCargaGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnVistaCargaConfiguracion});
            this.configCargaGroup.Name = "configCargaGroup";
            this.configCargaGroup.Text = "Configuración";
            // 
            // btnVistaCargaConfiguracion
            // 
            this.btnVistaCargaConfiguracion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.btnVistaCargaConfiguracion.AutoSize = false;
            this.btnVistaCargaConfiguracion.Bounds = new System.Drawing.Rectangle(0, 0, 62, 62);
            this.btnVistaCargaConfiguracion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.btnVistaCargaConfiguracion.ExpandArrowButton = false;
            this.btnVistaCargaConfiguracion.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnVistaCargaConfiguracion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaCargaConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasCargaMenu,
            this.guardarMenuCarga,
            this.cargarMenuCarga,
            this.columnasCargaMenu});
            this.btnVistaCargaConfiguracion.Name = "btnVistaCargaConfiguracion";
            this.btnVistaCargaConfiguracion.Text = "";
            // 
            // temasCargaMenu
            // 
            this.temasCargaMenu.Name = "temasCargaMenu";
            this.temasCargaMenu.Text = "Temas";
            // 
            // guardarMenuCarga
            // 
            this.guardarMenuCarga.Name = "guardarMenuCarga";
            this.guardarMenuCarga.Text = "Guardar";
            this.guardarMenuCarga.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarMenuCarga
            // 
            this.cargarMenuCarga.Name = "cargarMenuCarga";
            this.cargarMenuCarga.Text = "Cargar";
            this.cargarMenuCarga.Click += new System.EventHandler(this.LoadItem_Click);
            // 
            // columnasCargaMenu
            // 
            this.columnasCargaMenu.Name = "columnasCargaMenu";
            this.columnasCargaMenu.Text = "Columnas";
            this.columnasCargaMenu.Click += new System.EventHandler(this.ItemColumnas_ClickCarga);
            // 
            // OrdenesRecogida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 735);
            this.Controls.Add(this.tableLayoutPanelCarga);
            this.Controls.Add(this.radRibbonBarAcciones);
            this.MainMenuStrip = null;
            this.Name = "OrdenesRecogida";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanelCarga.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridPedidos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridPedidos)).EndInit();
            this.rgvGridPedidos.ResumeLayout(false);
            this.rgvGridPedidos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdfFiltro)).EndInit();
            this.rdfFiltro.ResumeLayout(false);
            this.tbDetalleCarga.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBarAcciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCarga;
        private Telerik.WinControls.UI.RadGridView rgvGridPedidos;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBarAcciones;
        private RumRibbonTab ribbonTabAccionesPedidos;
        private RumRibbonBarGroup prepBarGroup;
        private RumButtonElement btnAccionLanzamientoPedido;
        private RumButtonElement btnAccionLanzamientoAgrupado;
        private RumButtonElement btnAccionLanzammientoOla;
        private RumRibbonBarGroup vistaBarGroup;
        private RumButtonElement rBtnVistaPedidosPedidos;
        private RumButtonElement rBtnVistaPedidosOrdenes;
        private RumRibbonBarGroup rrbgVistaPedidosConfiguracion;
        private RumDropDownButtonElement rBtnddPedidosfiltradoOpciones;
        private RumMenuItem ordenPendientesHoy;
        private RumMenuItem tareasHechasHoy;
        private RumMenuItem tareasPendientesMenu;
        private RumMenuItem bultosPendCarga;
        private RumMenuItem ordenCargaAbiertas;
        private RumDropDownButtonElement btnVistaPedidosConfiguracion;
        private RumMenuItem temasMenu;
        private RumMenuItem guardarMenu;
        private RumMenuItem cargarMenu;
        private RumMenuItem editColumns;
        private RumMenuItem ImprimirEtiquetasPackingList;
        private RumButtonElement btnVistaPedidosRefrescar;
        private RumButtonElement rBtnVistaPedidosExportar;
        private RumButtonElement rBtnVistaPedidosFiltrar;
        private RumButtonElement btnVistaPedidosQuitarFiltro;
        private Telerik.WinControls.UI.RadCheckBoxElement chkRecursoManual;
        private RumButtonElement rBtnVistaPedidosCarga;
        private RumButtonElement btnAccionLiberarPedido;
        private RumRibbonTab ribbonTabAccionesOrdenesRecogida;
        private RumRibbonTab ribbonTabAccionesOrdenesCarga;
        private RumRibbonBarGroup ribbonbarVistaRecogida;
        private RumButtonElement btnVistaOrdenesPedidos;
        private RumButtonElement btnVistaOrdenesOrdenes;
        private RumButtonElement btnVistaOrdenesCarga;
        private RumButtonElement btnVistaOrdenesExportar;
        private RumButtonElement btnVistaOrdenesFiltrar;
        private RumButtonElement btnBorrarFiltroOrdenes;
        private RumRibbonBarGroup accionRecogBarGroup;
        private RumButtonElement btnAccionReservar;
        private RumButtonElement btnAccionServir;
        private RumButtonElement btnAccionCancelarReservar;
        private RumButtonElement btnAccionImprimirInforme;
        private RumRibbonBarGroup configRecogidaGroup;
        private RumDropDownButtonElement btnVistaOrdenesConfiguracion;
        private RumMenuItem temasRecogidaMenu;
        private RumMenuItem columnRecogMenu;
        private RumButtonElement btnVistaOrdenesRefrescar;
        private RumRibbonBarGroup vistaCargaGrupo;
        private RumButtonElement btnVistaCargaPedidos;
        private RumButtonElement btnVistaCargaOrdenes;
        private RumButtonElement btnVistaCargaCarga;
        private RumButtonElement btnVistaCargaExportar;
        private RumButtonElement btnVistaCargaFiltrar;
        private RumButtonElement btnVistaCargaBorrarFiltro;
        private RumRibbonBarGroup accionesCarga;
        private RumRibbonBarGroup configCargaGroup;
        private RumButtonElement bntAccionCrearCarga;
        private RumDropDownButtonElement btnVistaCargaConfiguracion;
        private RumMenuItem temasCargaMenu;
        private RumMenuItem columnasCargaMenu;
        private RumButtonElement btnVistaCargaRefrescar;
        private RumRibbonBarGroup verGrupoPrincipal;
        private RumRibbonBarGroup verRecogidaGrupo;
        private RumRibbonBarGroup verCargaGrupo;
        private RumMenuItem guardarMenuRecogida;
        private RumMenuItem cargarMenuRecogida;
        private RumMenuItem guardarMenuCarga;
        private RumMenuItem cargarMenuCarga;
        private Telerik.WinControls.UI.RadDataFilter rdfFiltro;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetalleCarga;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetalleDatosTotal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBotones;
        private System.Windows.Forms.TabControl tbDetalleCarga;
        private System.Windows.Forms.TabPage tabPackingList;
        private System.Windows.Forms.TabPage tabPedidos;
        private System.Windows.Forms.TabPage tabSalidas;
        private RumButtonElement btnAccionGenerarTareaCarga;
        private RumButtonElement btnAccionConfirmaCarga;
        private RumButtonElement btnAccionCerrarCarga;
        private RumButtonElement rBtnCerrarOrden;
        private RumButtonElement rBtnAsignarMuelle;
        private RumButtonElement btnCancelarReservaSeleccionadas;
        private RumButtonElement btnModificarReserva;
        private RumButtonElement btnCambiarExistencia;
        private RumButtonElement rBtnCerrarPedido;
        private RumButtonElement rBtnServirSeleccionadas;
        private RumButtonElement rBtnImprimirCarga;
        private RumButtonElement rBtnOrganizador;
    }
}

using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Presentation.Herramientas.Trazabilidad;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas
{
    public partial class Trazabilidad : Telerik.WinControls.UI.RadForm
    {
        public string name;

        private TableLayoutPanel tableLayoutPanelTabPedidos = new TableLayoutPanel();

        private RadDropDownList lista = new RadDropDownList();
        private RadDropDownList lista2 = new RadDropDownList();

        //TabControl tabControl1 = new System.Windows.Forms.TabControl();
        private RadTabbedFormControl tabControl1 = new RadTabbedFormControl();

        private RadGridView gridPedidos = new RadGridView();

        private RumButton btnVerSqlPedidos = new RumButton();
        private RumButton btnCerrarPedidos = new RumButton();
        private RumButton btnVerPacking = new RumButton();
        private RumButton btnImpContPedidoCli = new RumButton();
        private RumButton btnImpDiscrepanciasPedido = new RumButton();
        private RumButton btnImpPackingPedidoCli = new RumButton();
        private RumButton btnImpEtiqEnvio = new RumButton();
        private RumButton btnImpEtiqCarga = new RumButton();
        private RumButton btnQuitarReservas = new RumButton();
        private RumButton btnCerrarPedido = new RumButton();
        private RumButton btnCancelarPedido = new RumButton();

        private RumButton btnReiniciar = new RumButton();
        private RumButton btnSalir = new RumButton();
        private RumButton btnIniciar = new RumButton();

        private RadTreeNode filtro = new RadTreeNode(Lenguaje.traduce("Filtro"));
        private RadTreeNode subFiltro = new RadTreeNode(Lenguaje.traduce("Sub Filtro"));
        private RadTreeNode packing = new RadTreeNode(Lenguaje.traduce("Packing"));
        private RadTreeNode pedidosCli = new RadTreeNode(Lenguaje.traduce("Pedidos Cliente"));
        private RadTreeNode ordenesRecogida = new RadTreeNode(Lenguaje.traduce("Ordenes Recogida"));
        private RadTreeNode ordenesCarga = new RadTreeNode(Lenguaje.traduce("Ordenes Carga"));
        private RadTreeNode movSalida = new RadTreeNode(Lenguaje.traduce("Movimientos Salida"));
        private RadTreeNode matricula = new RadTreeNode(Lenguaje.traduce("Matricula"));
        private RadTreeNode articulos = new RadTreeNode(Lenguaje.traduce("Articulos"));
        private RadTreeNode pedidosProv = new RadTreeNode(Lenguaje.traduce("Pedidos Proveedor"));
        private RadTreeNode recepciones = new RadTreeNode(Lenguaje.traduce("Recepciones"));
        private RadTreeNode devolCli = new RadTreeNode(Lenguaje.traduce("Devoluciones Cliente"));
        private RadTreeNode devolProv = new RadTreeNode(Lenguaje.traduce("Devoluciones Proveedor"));
        private RadTreeNode ordenesFab = new RadTreeNode(Lenguaje.traduce("Ordenes Fabricacion"));
        private RadTreeNode regEntrada = new RadTreeNode(Lenguaje.traduce("Regularizaciones Entrada"));
        private RadTreeNode regSalida = new RadTreeNode(Lenguaje.traduce("Regularizaciones Salida"));
        private RadTreeNode preavisos = new RadTreeNode(Lenguaje.traduce("Preavisos"));
        private RadTreeNode maquinas = new RadTreeNode(Lenguaje.traduce("Maquinas"));
        private RadTreeNode tareasPend = new RadTreeNode(Lenguaje.traduce("Tareas Pendientes"));
        private RadTreeNode compStock = new RadTreeNode(Lenguaje.traduce(Constantes.COMPARATIVASTOCKMENU));

        //SUBFILTROS PACKING
        private RadTreeNode packingHoy = new RadTreeNode(Lenguaje.traduce("Packing List Hoy"));

        private RadTreeNode packingPendienteCarga = new RadTreeNode(Lenguaje.traduce("Pendientes de Carga"));
        private RadTreeNode packingPendienteCargaHoy = new RadTreeNode(Lenguaje.traduce("Pendiente Carga Hoy"));
        private RadTreeNode packingPendienteCargaHoySinOrden = new RadTreeNode(Lenguaje.traduce("PC Hoy Sin Orden de Carga"));
        private RadTreeNode packingPendienteCargaHoyConOrden = new RadTreeNode(Lenguaje.traduce("PC Hoy Con Orden de Carga"));
        private RadTreeNode packingXNumero = new RadTreeNode(Lenguaje.traduce("PL por Numero"));
        private RadTreeNode packingXFecha = new RadTreeNode(Lenguaje.traduce("PL Por Fecha Creacion"));
        private RadTreeNode packingXUbicacion = new RadTreeNode(Lenguaje.traduce("PL por Ubicacion Actual"));
        private RadTreeNode packingXEstados = new RadTreeNode(Lenguaje.traduce("PL por Estados"));

        //SUBFILTROS PEDIDOS
        private RadTreeNode pedidosHoy = new RadTreeNode(Lenguaje.traduce("Pedidos de Hoy"));

        private RadTreeNode pedidosPend = new RadTreeNode(Lenguaje.traduce("Pendientes de Carga"));
        private RadTreeNode pedidosXNum = new RadTreeNode(Lenguaje.traduce("Por Numero de Pedido"));
        private RadTreeNode pedidosXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha Prevista de Envio"));
        private RadTreeNode pedidosXCodigoRuta = new RadTreeNode(Lenguaje.traduce("Por Codigo de Ruta"));
        private RadTreeNode pedidosXCodigoCliente = new RadTreeNode(Lenguaje.traduce("Por Codigo de Cliente"));
        private RadTreeNode pedidosXEstado = new RadTreeNode(Lenguaje.traduce("Por Estado de Pedido"));

        //SUBFILTROS ORDENES DE RECOGIDA
        private RadTreeNode ordenesRecHoy = new RadTreeNode(Lenguaje.traduce("Ordenes de hoy"));

        private RadTreeNode ordenesRecPendCarga = new RadTreeNode(Lenguaje.traduce("Pendientes de Carga"));
        private RadTreeNode ordenesRecXNumOrden = new RadTreeNode(Lenguaje.traduce("Por Numero de Orden"));
        private RadTreeNode ordenesRecXEstado = new RadTreeNode(Lenguaje.traduce("Por Estado"));
        private RadTreeNode ordenesRecXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha"));

        //SUBFILTROS ORDENES DE CARGA
        private RadTreeNode ordenesCargaHoy = new RadTreeNode(Lenguaje.traduce("Ordenes de hoy"));

        private RadTreeNode ordenesCargaPendCarga = new RadTreeNode(Lenguaje.traduce("Ordenes Pendientes de Carga"));
        private RadTreeNode ordenesCargaXNumOrden = new RadTreeNode(Lenguaje.traduce("Por Nº Albaran Transportista"));
        private RadTreeNode ordenesCargaXEstado = new RadTreeNode(Lenguaje.traduce("Por Estado"));
        private RadTreeNode ordenesCargaXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha"));
        private RadTreeNode ordenesCargaXIdent = new RadTreeNode(Lenguaje.traduce("Por Identificador de Carga"));

        //SUBFILTROS MOVIMIENTOS SALIDA
        private RadTreeNode salidasHoy = new RadTreeNode(Lenguaje.traduce("Salidas de Hoy"));

        private RadTreeNode salidasXArticulo = new RadTreeNode(Lenguaje.traduce("Salidas por Articulo"));
        private RadTreeNode salidasXArticuloLote = new RadTreeNode(Lenguaje.traduce("Salidas por Artículo y Lote"));
        private RadTreeNode salidasXFechas = new RadTreeNode(Lenguaje.traduce("Salidas Entre Fechas"));

        //SUBFILTROS MATRICULA
        private RadTreeNode entradasMatricula = new RadTreeNode(Lenguaje.traduce("Entrada y Stock Nº Matricula"));

        private RadTreeNode salidasMatricula = new RadTreeNode(Lenguaje.traduce("Salidas Realizadas Nº M"));
        private RadTreeNode entradasMatrSerie = new RadTreeNode(Lenguaje.traduce("Entradas y Stock Nº Serie"));
        private RadTreeNode salidasMatrSerie = new RadTreeNode(Lenguaje.traduce("Salidas Realizadas Nº Serie"));

        //SUBFILTROS ARTICULOS
        private RadTreeNode articulosEntradas = new RadTreeNode(Lenguaje.traduce("Entrada y Stock Articulo y Lote"));

        private RadTreeNode articulosSalidas = new RadTreeNode(Lenguaje.traduce("Salidas Articulo y Lote"));
        private RadTreeNode entradasEntreFechas = new RadTreeNode(Lenguaje.traduce("Entrada y Stock entre Fechas"));
        private RadTreeNode salidasEntreFechas = new RadTreeNode(Lenguaje.traduce("Salidas Entre Fechas"));

        //SUBFILTROS PEDIDOS PROVEEDOR
        private RadTreeNode pedidosProvHoy = new RadTreeNode(Lenguaje.traduce("Pedidos de Hoy"));

        private RadTreeNode pedidosProvPendientes = new RadTreeNode(Lenguaje.traduce("Pedidos Pendientes"));
        private RadTreeNode pedidosProvXReferencia = new RadTreeNode(Lenguaje.traduce("Pedidos por Referencia y Serie"));
        private RadTreeNode pedidosProvXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha Prevista de Entrada"));
        private RadTreeNode pedidosProvXCodigo = new RadTreeNode(Lenguaje.traduce("Por Codigo de Proveedor"));

        //SUBFILTROS RECEPCIONES
        private RadTreeNode recepcionesHoy = new RadTreeNode(Lenguaje.traduce("Recepciones de Hoy"));

        private RadTreeNode recepcionesPend = new RadTreeNode(Lenguaje.traduce("Recepciones Pendientes"));
        private RadTreeNode recepcionesXNumRecepcion = new RadTreeNode(Lenguaje.traduce("Pedidos Numero de Recepcion"));
        private RadTreeNode recepcionesXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha Prevista de Entrada"));
        private RadTreeNode recepcionesXEstado = new RadTreeNode(Lenguaje.traduce("Por Estado"));

        //SUBFILTROS DEVOLUCIONES CLIENTES
        private RadTreeNode devolucionesCliHoy = new RadTreeNode(Lenguaje.traduce("Devoluciones de Hoy"));

        private RadTreeNode devolucionesCliPendientes = new RadTreeNode(Lenguaje.traduce("Devoluciones Pendientes"));
        private RadTreeNode devolucionesCliXNumero = new RadTreeNode(Lenguaje.traduce("Numero de Devolucion"));
        private RadTreeNode devolucionesCliXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha Prevista Devolucion"));
        private RadTreeNode devolucionesCliXEstado = new RadTreeNode(Lenguaje.traduce("Por Estado"));

        //SUBFILTROS DEVOLUCIONES PROVEEDOR
        private RadTreeNode devolucionesProvHoy = new RadTreeNode(Lenguaje.traduce("Devoluciones de Hoy"));

        private RadTreeNode devolucionesProvPendientes = new RadTreeNode(Lenguaje.traduce("Devoluciones Pendientes"));
        private RadTreeNode devolucionesProvXNumero = new RadTreeNode(Lenguaje.traduce("Numero de Devolucion"));
        private RadTreeNode devolucionesProvXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha Prevista Devolucion"));
        private RadTreeNode devolucionesProvXEstado = new RadTreeNode(Lenguaje.traduce("Por Estado"));

        //SUBFILTROS ORDENES FABRICACIÓN
        private RadTreeNode ordenesFabHoy = new RadTreeNode(Lenguaje.traduce("OF de Hoy"));

        private RadTreeNode ordenesFabPendientes = new RadTreeNode(Lenguaje.traduce("OF Pendientes"));
        private RadTreeNode ordenesFabSinAsignar = new RadTreeNode(Lenguaje.traduce("OF sin Asignar"));
        private RadTreeNode ordenesFabXNumOrden = new RadTreeNode(Lenguaje.traduce("OF por Numero de Orden"));
        private RadTreeNode ordenesFabXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha"));
        private RadTreeNode ordenesFabXFechaYArt = new RadTreeNode(Lenguaje.traduce("Por Fecha y Articulo Final"));

        //SUFILTROS REGULARIZACIONES ENTRADA
        private RadTreeNode entradasRegularizacionHoy = new RadTreeNode(Lenguaje.traduce("ER de Hoy"));

        private RadTreeNode entradasRegularizacionXFecha = new RadTreeNode(Lenguaje.traduce("ER por Fecha"));
        private RadTreeNode entradasRegularizacionXArticulo = new RadTreeNode(Lenguaje.traduce("ER por Articulo y Lote"));
        private RadTreeNode entradasRegularizacionXMotivo = new RadTreeNode(Lenguaje.traduce("ER por Motivo y Fechas"));

        //SUBFILTROS REGULARIZACIONES SALIDA
        private RadTreeNode salidasRegularizacionHoy = new RadTreeNode(Lenguaje.traduce("SR de Hoy"));

        private RadTreeNode salidasRegularizacionXFecha = new RadTreeNode(Lenguaje.traduce("SR por Fecha"));
        private RadTreeNode salidasRegularizacionXArticulo = new RadTreeNode(Lenguaje.traduce("SR por Articulo y Lote"));
        private RadTreeNode salidasRegularizacionXMotivo = new RadTreeNode(Lenguaje.traduce("SR por Motivo y Fechas"));

        //SUBFILTROS PREAVISOS
        private RadTreeNode preavisosHoy = new RadTreeNode(Lenguaje.traduce("Preavisos Hoy"));

        private RadTreeNode preavisosXNumPedido = new RadTreeNode(Lenguaje.traduce("Por Numero de Pedido"));
        private RadTreeNode preavisosXFecha = new RadTreeNode(Lenguaje.traduce("Por Fecha Creacion de Pedido"));
        private RadTreeNode preavisosSSCC = new RadTreeNode(Lenguaje.traduce("Por Num Etiqueta"));

        //SUBFILTROS MAQUINAS
        private RadTreeNode maquinasXNombre = new RadTreeNode(Lenguaje.traduce("Por Nombre"));

        private RadTreeNode maquinasXZona = new RadTreeNode(Lenguaje.traduce("Por Zona"));

        //SUBFILTROS TAREAS PENDIENTES
        private RadTreeNode tareasPendientes = new RadTreeNode(Lenguaje.traduce("Tareas Pendientes"));

        private RadTreeNode tareasPorFecha = new RadTreeNode(Lenguaje.traduce("Tareas por fecha"));
        private RadTreeNode tareasPorTipo = new RadTreeNode(Lenguaje.traduce("Tareas por tipo"));

        //SUBFILTROS COMPARATIVA STOCK
        private RadTreeNode stockComparativa = new RadTreeNode(Lenguaje.traduce(Constantes.DISCREPANCIAS));

        private RadTreeNode stockPorArticulo = new RadTreeNode(Lenguaje.traduce(Constantes.PORARTICULO));
        private RadTreeNode stockPorLote = new RadTreeNode(Lenguaje.traduce(Constantes.PORLOTE));

        public Trazabilidad()
        {
            InitializeComponent();
            this.Name = "Trazabilidad";
            this.Text = Lenguaje.traduce("Trazabilidad");
            this.Size = new Size(1000, 1000);
            this.FormBorderStyle = FormBorderStyle.None;

            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 70);

            radCollapsiblePanel1.EnableAnimation = false;
            radCollapsiblePanel1.Collapsed += RadCollapsiblePanel1_Collapsed;
            radCollapsiblePanel1.Expanded += RadCollapsiblePanel1_Expanded;
            //ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            //this.radTreeView1.SelectedNodeChanged += radTreeView1_SelectedNodeChanged;
            this.radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            this.WindowState = FormWindowState.Maximized;

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.radCollapsiblePanel1.CollapsiblePanelElement.HeaderElement.ShowHeaderLine = false;

            this.contenedor.AutoSize = true;
            this.contenedor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.contenedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contenedor.Location = new System.Drawing.Point(0, 57);
            this.contenedor.Name = "contenedor";
            this.contenedor.Size = new System.Drawing.Size(955, 734);
            this.contenedor.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            this.contenedor.TabIndex = 0;

            tabControl1.Dock = DockStyle.Fill;

            ////this.IsMdiContainer = true;
            //PanelTabs panelTabs = new PanelTabs();
            //panelTabs.Dock = DockStyle.Fill;
            ////panelTabs.MdiParent = this;

            //panelTabs.TopLevel = false;
            //panelTabs.Dock = DockStyle.Fill;
            //filtro.Text = "Filtro";
            //subFiltro.Text = "Sub Filtro";
            ////this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10));
            ////this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            //this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //tableLayoutPanel.Controls.Add(filtro, 0, 0);
            //tableLayoutPanel.Controls.Add(text, 1, 0);
            //tableLayoutPanel.Controls.Add(subFiltro, 2, 0);
            //tableLayoutPanel.Controls.Add(text2, 3, 0);
            ////tableLayoutPanel.Controls.Add(panelTabs, 0, 1);
            //panelTabs.Dock = DockStyle.Fill;
            //this.Controls.Add(tableLayoutPanel);
            //panel.Location = new System.Drawing.Point(0, 100);

            //panel.Controls.Add(panelTabs);
            ////panel.Dock = DockStyle.Fill;
            //this.Controls.Add(panel);
            ////this.Controls.Add(tableLayoutPanel);
            //panelTabs.Show();

            this.SuspendLayout();
            //
            // tabControl1
            //
            //this.tabControl1.Location = new System.Drawing.Point(8, 16);
            tabControl1.Name = "tabControl1";
            tabControl1.TabIndex = 0;
            //tabControl1.SelectedIndex = 0;
            //this.tabControl1.Size = new System.Drawing.Size(352, 248);

            tabControl1.TabIndex = 0;
            tabControl1.ShowNewTabButton = false;
            tabControl1.ShowItemToolTips = false;
            tabControl1.MaximizeButton = false;
            tabControl1.MinimizeButton = false;
            tabControl1.CloseButton = false;
            //
            // Form1
            //
            //this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            //this.ClientSize = new System.Drawing.Size(368, 273);
            //this.Controls.Add(this.tabControl1);
            this.ResumeLayout(false);
            //this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));

            //tableLayoutPanelPacking.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //tableLayoutPanelPacking.AutoSize = true;
            //tableLayoutPanelPacking.Dock = DockStyle.Fill;

            //this.tableLayoutPanelPacking.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //this.tableLayoutPanelPacking.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0));

            //tableLayoutPanel.Controls.Add(filtro,0,0);
            //tableLayoutPanel.Controls.Add(lista, 1, 0);
            //tableLayoutPanel.Controls.Add(subFiltro, 2, 0);
            //tableLayoutPanel.Controls.Add(lista2, 3, 0);
            //tableLayoutPanel.Controls.Add(btnIniciar, 1, 1);
            //tableLayoutPanel.Controls.Add(btnReiniciar, 1, 1);
            tableLayoutPanel.Controls.Add(tabControl1, 1, 0);
            //tableLayoutPanel.Controls.Add(btnSalir, 2, 1);
            //tableLayoutPanel.SetColumnSpan(tabControl1, 5);

            btnReiniciar.Visible = false;
            btnReiniciar.Enabled = false;

            //PESTAÑA PACKING LIST

            btnSalir.Click += btnSalir_Event;

            filtro.Name = "filtro";
            subFiltro.Name = "subFiltro";
            packing.Name = "packing";
            pedidosCli.Name = "pedidosCli";
            ordenesRecogida.Name = "ordenesRecogida";
            ordenesCarga.Name = "ordenesCarga";
            movSalida.Name = "movSalida";
            matricula.Name = "matricula";
            articulos.Name = "articulos";
            pedidosProv.Name = "pedidosProv";
            recepciones.Name = "recepciones";
            devolCli.Name = "devolCli";
            devolProv.Name = "devolProv";
            ordenesFab.Name = "ordenesFab";
            regEntrada.Name = "regEntrada";
            regSalida.Name = "regSalida";
            preavisos.Name = "preavisos";
            maquinas.Name = "maquinas";
            tareasPend.Name = "tareasPend";
            compStock.Name = "compStock";

            packingHoy.Name = "packingHoy";
            packingPendienteCarga.Name = "packingPendienteCarga";
            packingPendienteCargaHoy.Name = "packingPendienteCargaHoy";
            packingPendienteCargaHoySinOrden.Name = "packingPendienteCargaHoySinOrden";
            packingPendienteCargaHoyConOrden.Name = "packingPendienteCargaHoyConOrden";
            packingXNumero.Name = "packingXNumero";
            packingXFecha.Name = "packingXFecha";
            packingXUbicacion.Name = "packingXUbicacion";
            packingXEstados.Name = "packingXEstados";

            pedidosHoy.Name = "pedidosHoy";
            pedidosPend.Name = "pedidosPend";
            pedidosXNum.Name = "pedidosXNum";
            pedidosXFecha.Name = "pedidosXFecha";
            pedidosXCodigoRuta.Name = "pedidosXCodigoRuta";
            pedidosXCodigoCliente.Name = "pedidosXCodigoCliente";
            pedidosXEstado.Name = "pedidosXEstado";

            ordenesRecHoy.Name = "ordenesRecHoy";
            ordenesRecPendCarga.Name = "ordenesRecPendCarga";
            ordenesRecXNumOrden.Name = "ordenesRecXNumOrden";
            ordenesRecXEstado.Name = "ordenesRecXEstado";
            ordenesRecXFecha.Name = "ordenesRecXFecha";

            ordenesCargaHoy.Name = "ordenesCargaHoy";
            ordenesCargaPendCarga.Name = "ordenesCargaPendCarga";
            ordenesCargaXNumOrden.Name = "ordenesCargaXNumOrden";
            ordenesCargaXEstado.Name = "ordenesCargaXEstado";
            ordenesCargaXFecha.Name = "ordenesCargaXFecha";
            ordenesCargaXIdent.Name = "ordenesCargaXIdent";

            salidasHoy.Name = "salidasHoy";
            salidasXArticulo.Name = "salidasXArticulo";
            salidasXArticuloLote.Name = "salidasXArticuloLote";
            salidasXFechas.Name = "salidasXFechas";

            entradasMatricula.Name = "entradasMatricula";
            salidasMatricula.Name = "salidasMatricula";
            entradasMatrSerie.Name = "entradasMatrSerie";
            salidasMatrSerie.Name = "salidasMatrSerie";

            articulosEntradas.Name = "articulosEntradas";
            articulosSalidas.Name = "articulosSalidas";
            entradasEntreFechas.Name = "entradasEntreFechas";
            salidasEntreFechas.Name = "salidasEntreFechas";

            pedidosProvHoy.Name = "pedidosProvHoy";
            pedidosProvPendientes.Name = "pedidosProvPendientes";
            pedidosProvXReferencia.Name = "pedidosProvXReferencia";
            pedidosProvXFecha.Name = "pedidosProvXFecha";
            pedidosProvXCodigo.Name = "pedidosProvXCodigo";

            recepcionesHoy.Name = "recepcionesHoy";
            recepcionesPend.Name = "recepcionesPend";
            recepcionesXNumRecepcion.Name = "recepcionesXNumRecepcion";
            recepcionesXFecha.Name = "recepcionesXFecha";
            recepcionesXEstado.Name = "recepcionesXEstado";

            devolucionesCliHoy.Name = "devolucionesCliHoy";
            devolucionesCliPendientes.Name = "devolucionesCliPendientes";
            devolucionesCliXNumero.Name = "devolucionesCliXNumero";
            devolucionesCliXFecha.Name = "devolucionesCliXFecha";
            devolucionesCliXEstado.Name = "devolucionesCliXEstado";

            devolucionesProvHoy.Name = "devolucionesProvHoy";
            devolucionesProvPendientes.Name = "devolucionesProvPendientes";
            devolucionesProvXNumero.Name = "devolucionesProvXNumero";
            devolucionesProvXFecha.Name = "devolucionesProvXFecha";
            devolucionesProvXEstado.Name = "devolucionesProvXEstado";

            ordenesFabHoy.Name = "ordenesFabHoy";
            ordenesFabPendientes.Name = "ordenesFabPendientes";
            ordenesFabSinAsignar.Name = "ordenesFabSinAsignar";
            ordenesFabXNumOrden.Name = "ordenesFabXNumOrden";
            ordenesFabXFecha.Name = "ordenesFabXFecha";
            ordenesFabXFechaYArt.Name = "ordenesFabXFechaYArt";

            entradasRegularizacionHoy.Name = "entradasRegularizacionHoy";
            entradasRegularizacionXFecha.Name = "entradasRegularizacionXFecha";
            entradasRegularizacionXArticulo.Name = "entradasRegularizacionXArticulo";
            entradasRegularizacionXMotivo.Name = "entradasRegularizacionXMotivo";

            salidasRegularizacionHoy.Name = "salidasRegularizacionHoy";
            salidasRegularizacionXFecha.Name = "salidasRegularizacionXFecha";
            salidasRegularizacionXArticulo.Name = "salidasRegularizacionXArticulo";
            salidasRegularizacionXMotivo.Name = "salidasRegularizacionXMotivo";

            preavisosHoy.Name = "preavisosHoy";
            preavisosXNumPedido.Name = "preavisosXNumPedido";
            preavisosXFecha.Name = "preavisosXFecha";
            preavisosSSCC.Name = "preavisosSSCC";

            maquinasXNombre.Name = "maquinasXNombre";
            maquinasXZona.Name = "maquinasXZona";

            tareasPendientes.Name = "tareasPendientes";
            tareasPorFecha.Name = "tareasPorFecha";
            tareasPorTipo.Name = "tareasPorTipo";

            stockComparativa.Name = Constantes.STOCKCOMPARATIVA;
            stockPorArticulo.Name = Constantes.STOCKPORARTICULO;
            stockPorLote.Name = Constantes.STOCKPORLOTE;

            filtro.Nodes.Add(packing);
            filtro.Nodes.Add(pedidosCli);
            filtro.Nodes.Add(ordenesRecogida);
            filtro.Nodes.Add(ordenesCarga);
            filtro.Nodes.Add(movSalida);
            filtro.Nodes.Add(matricula);
            filtro.Nodes.Add(articulos);
            filtro.Nodes.Add(pedidosProv);
            filtro.Nodes.Add(recepciones);
            filtro.Nodes.Add(devolCli);
            filtro.Nodes.Add(devolProv);
            filtro.Nodes.Add(ordenesFab);
            filtro.Nodes.Add(regEntrada);
            filtro.Nodes.Add(regSalida);
            filtro.Nodes.Add(preavisos);
            filtro.Nodes.Add(maquinas);
            filtro.Nodes.Add(tareasPend);
            filtro.Nodes.Add(compStock);

            //foreach (RadTreeNode item in filtro.Nodes)
            //{
            //    item.Font = new Font("ninguno",12); ;
            //}
            //foreach (RadTreeNode item in subFiltro.Nodes)
            //{
            //    item.Font = (ninguno);
            //}

            radTreeView1.Nodes.Add(filtro);
            radTreeView1.Nodes.Add(subFiltro);

            foreach (RadTreeNode item in radTreeView1.TreeViewElement.GetNodes())
            {
                if (item.Nodes.Count > 0)
                {
                    //Nodo padre
                    item.Image = Resources.normal;
                    item.Font = new Font("ninguno", 12, FontStyle.Bold);
                }
                else
                {
                    //Nodo hijo
                    item.Font = new Font("ninguno", 10);
                }
            }

            radTreeView1.SelectedNodeChanged += cargarSubFiltros;
            filtro.Expanded = true;
            //cargarSubFiltros();

            this.Shown += form_Shown;
            this.Show();
        }

        private void cargarSubFiltros(object sender, RadTreeViewEventArgs e)
        {
            if (radTreeView1.SelectedNode == null) return;
            this.TopMost = false;
            switch (radTreeView1.SelectedNode.Name)
            {
                //FILTROS PADRE
                case "packing":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(packingHoy);
                    subFiltro.Nodes.Add(packingPendienteCarga);
                    subFiltro.Nodes.Add(packingPendienteCargaHoy);
                    subFiltro.Nodes.Add(packingPendienteCargaHoySinOrden);
                    subFiltro.Nodes.Add(packingPendienteCargaHoyConOrden);
                    subFiltro.Nodes.Add(packingXNumero);
                    subFiltro.Nodes.Add(packingXFecha);
                    subFiltro.Nodes.Add(packingXUbicacion);
                    subFiltro.Nodes.Add(packingXEstados);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "pedidosCli":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(pedidosHoy);
                    subFiltro.Nodes.Add(pedidosPend);
                    subFiltro.Nodes.Add(pedidosXNum);
                    subFiltro.Nodes.Add(pedidosXFecha);
                    subFiltro.Nodes.Add(pedidosXCodigoRuta);
                    subFiltro.Nodes.Add(pedidosXCodigoCliente);
                    subFiltro.Nodes.Add(pedidosXEstado);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "ordenesRecogida":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(ordenesRecHoy);
                    subFiltro.Nodes.Add(ordenesRecPendCarga);
                    subFiltro.Nodes.Add(ordenesRecXNumOrden);
                    subFiltro.Nodes.Add(ordenesRecXEstado);
                    subFiltro.Nodes.Add(ordenesRecXFecha);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "ordenesCarga":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(ordenesCargaHoy);
                    subFiltro.Nodes.Add(ordenesCargaPendCarga);
                    subFiltro.Nodes.Add(ordenesCargaXNumOrden);
                    subFiltro.Nodes.Add(ordenesCargaXEstado);
                    subFiltro.Nodes.Add(ordenesCargaXFecha);
                    subFiltro.Nodes.Add(ordenesCargaXIdent);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "movSalida":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(salidasHoy);
                    subFiltro.Nodes.Add(salidasXArticulo);
                    subFiltro.Nodes.Add(salidasXArticuloLote);
                    subFiltro.Nodes.Add(salidasXFechas);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "matricula":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(entradasMatricula);
                    subFiltro.Nodes.Add(salidasMatricula);
                    subFiltro.Nodes.Add(entradasMatrSerie);
                    subFiltro.Nodes.Add(salidasMatrSerie);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "articulos":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(articulosEntradas);
                    subFiltro.Nodes.Add(articulosSalidas);
                    subFiltro.Nodes.Add(entradasEntreFechas);
                    subFiltro.Nodes.Add(salidasEntreFechas);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "pedidosProv":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(pedidosProvHoy);
                    subFiltro.Nodes.Add(pedidosProvPendientes);
                    subFiltro.Nodes.Add(pedidosProvXReferencia);
                    subFiltro.Nodes.Add(pedidosProvXFecha);
                    subFiltro.Nodes.Add(pedidosProvXCodigo);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "recepciones":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(recepcionesHoy);
                    subFiltro.Nodes.Add(recepcionesPend);
                    subFiltro.Nodes.Add(recepcionesXNumRecepcion);
                    subFiltro.Nodes.Add(recepcionesXFecha);
                    subFiltro.Nodes.Add(recepcionesXEstado);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "devolCli":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(devolucionesCliHoy);
                    subFiltro.Nodes.Add(devolucionesCliPendientes);
                    subFiltro.Nodes.Add(devolucionesCliXNumero);
                    subFiltro.Nodes.Add(devolucionesCliXFecha);
                    subFiltro.Nodes.Add(devolucionesCliXEstado);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "devolProv":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(devolucionesProvHoy);
                    subFiltro.Nodes.Add(devolucionesProvPendientes);
                    subFiltro.Nodes.Add(devolucionesProvXNumero);
                    subFiltro.Nodes.Add(devolucionesProvXFecha);
                    subFiltro.Nodes.Add(devolucionesProvXEstado);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "ordenesFab":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(ordenesFabHoy);
                    subFiltro.Nodes.Add(ordenesFabPendientes);
                    subFiltro.Nodes.Add(ordenesFabSinAsignar);
                    subFiltro.Nodes.Add(ordenesFabXNumOrden);
                    subFiltro.Nodes.Add(ordenesFabXFecha);
                    subFiltro.Nodes.Add(ordenesFabXFechaYArt);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "regEntrada":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(entradasRegularizacionHoy);
                    subFiltro.Nodes.Add(entradasRegularizacionXFecha);
                    subFiltro.Nodes.Add(entradasRegularizacionXArticulo);
                    subFiltro.Nodes.Add(entradasRegularizacionXMotivo);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "regSalida":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(salidasRegularizacionHoy);
                    subFiltro.Nodes.Add(salidasRegularizacionXFecha);
                    subFiltro.Nodes.Add(salidasRegularizacionXArticulo);
                    subFiltro.Nodes.Add(salidasRegularizacionXMotivo);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "preavisos":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(preavisosHoy);
                    subFiltro.Nodes.Add(preavisosXNumPedido);
                    subFiltro.Nodes.Add(preavisosXFecha);
                    subFiltro.Nodes.Add(preavisosSSCC);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "maquinas":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(maquinasXNombre);
                    subFiltro.Nodes.Add(maquinasXZona);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case "tareasPend":
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(tareasPendientes);
                    subFiltro.Nodes.Add(tareasPorFecha);
                    subFiltro.Nodes.Add(tareasPorTipo);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;

                case Constantes.COMPSTOCK:
                    subFiltro.Nodes.Clear();
                    subFiltro.Nodes.Add(stockComparativa);
                    subFiltro.Nodes.Add(stockPorArticulo);
                    subFiltro.Nodes.Add(stockPorLote);
                    filtro.Expanded = false;
                    subFiltro.Expanded = true;
                    break;
            }
            if (radTreeView1.SelectedNode.Name == "packingHoy" || radTreeView1.SelectedNode.Name == "packingPendienteCarga" || radTreeView1.SelectedNode.Name == "packingPendienteCargaHoy"
                || radTreeView1.SelectedNode.Name == "packingPendienteCargaHoySinOrden" || radTreeView1.SelectedNode.Name == "packingPendienteCargaHoyConOrden"
                || radTreeView1.SelectedNode.Name == "packingXNumero" || radTreeView1.SelectedNode.Name == "packingXFecha" || radTreeView1.SelectedNode.Name == "packingXUbicacion" || radTreeView1.SelectedNode.Name == "packingXEstados")
            {
                cargarPackingList(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "pedidosHoy" || radTreeView1.SelectedNode.Name == "pedidosPend" || radTreeView1.SelectedNode.Name == "pedidosXNum"
               || radTreeView1.SelectedNode.Name == "pedidosXFecha" || radTreeView1.SelectedNode.Name == "pedidosXCodigoRuta"
               || radTreeView1.SelectedNode.Name == "pedidosXCodigoCliente" || radTreeView1.SelectedNode.Name == "pedidosXEstado")
            {
                cargarPedidos(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "ordenesRecHoy" || radTreeView1.SelectedNode.Name == "ordenesRecPendCarga" || radTreeView1.SelectedNode.Name == "ordenesRecXNumOrden"
                || radTreeView1.SelectedNode.Name == "ordenesRecXEstado" || radTreeView1.SelectedNode.Name == "ordenesRecXFecha")
            {
                cargarOrdenesRec(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "ordenesCargaHoy" || radTreeView1.SelectedNode.Name == "ordenesCargaPendCarga" || radTreeView1.SelectedNode.Name == "ordenesCargaXNumOrden"
               || radTreeView1.SelectedNode.Name == "ordenesCargaXEstado" || radTreeView1.SelectedNode.Name == "ordenesCargaXFecha"
               || radTreeView1.SelectedNode.Name == "ordenesCargaXIdent")
            {
                cargarOrdenesCarga(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "salidasHoy" || radTreeView1.SelectedNode.Name == "salidasXArticulo" || radTreeView1.SelectedNode.Name == "salidasXArticuloLote" || radTreeView1.SelectedNode.Name == "salidasXFechas")
            {
                cargarOrdenesMovimientosSalida(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "entradasMatricula" || radTreeView1.SelectedNode.Name == "salidasMatricula" || radTreeView1.SelectedNode.Name == "entradasMatrSerie" || radTreeView1.SelectedNode.Name == "salidasMatrSerie")
            {
                cargarMatricula(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "articulosEntradas" || radTreeView1.SelectedNode.Name == "articulosSalidas" || radTreeView1.SelectedNode.Name == "entradasEntreFechas"
               || radTreeView1.SelectedNode.Name == "salidasEntreFechas")
            {
                cargarArticulos(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "pedidosProvHoy" || radTreeView1.SelectedNode.Name == "pedidosProvPendientes" || radTreeView1.SelectedNode.Name == "pedidosProvXReferencia"
               || radTreeView1.SelectedNode.Name == "pedidosProvXFecha" || radTreeView1.SelectedNode.Name == "pedidosProvXCodigo")
            {
                cargarPedidosProv(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "recepcionesHoy" || radTreeView1.SelectedNode.Name == "recepcionesPend" || radTreeView1.SelectedNode.Name == "recepcionesXNumRecepcion"
               || radTreeView1.SelectedNode.Name == "recepcionesXFecha" || radTreeView1.SelectedNode.Name == "recepcionesXEstado")
            {
                cargarRecepciones(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "devolucionesCliHoy" || radTreeView1.SelectedNode.Name == "devolucionesCliPendientes" || radTreeView1.SelectedNode.Name == "devolucionesCliXNumero"
               || radTreeView1.SelectedNode.Name == "devolucionesCliXFecha" || radTreeView1.SelectedNode.Name == "devolucionesCliXEstado")
            {
                cargarDevolucionesCliente(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "devolucionesProvHoy" || radTreeView1.SelectedNode.Name == "devolucionesProvPendientes" || radTreeView1.SelectedNode.Name == "devolucionesProvXNumero"
               || radTreeView1.SelectedNode.Name == "devolucionesProvXFecha" || radTreeView1.SelectedNode.Name == "devolucionesProvXEstado")
            {
                cargarDevolucionesProveedor(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "ordenesFabHoy" || radTreeView1.SelectedNode.Name == "ordenesFabPendientes" || radTreeView1.SelectedNode.Name == "ordenesFabSinAsignar"
               || radTreeView1.SelectedNode.Name == "ordenesFabXNumOrden" || radTreeView1.SelectedNode.Name == "ordenesFabXFecha" || radTreeView1.SelectedNode.Name == "ordenesFabXFechaYArt")
            {
                cargarOrdenesFabricacion(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "entradasRegularizacionHoy" || radTreeView1.SelectedNode.Name == "entradasRegularizacionXFecha" || radTreeView1.SelectedNode.Name == "entradasRegularizacionXArticulo"
              || radTreeView1.SelectedNode.Name == "entradasRegularizacionXMotivo")
            {
                cargarRegularizacionesEntrada(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "salidasRegularizacionHoy" || radTreeView1.SelectedNode.Name == "salidasRegularizacionXFecha" || radTreeView1.SelectedNode.Name == "salidasRegularizacionXArticulo"
               || radTreeView1.SelectedNode.Name == "salidasRegularizacionXMotivo")
            {
                cargarRegularizacionesSalida(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "preavisosHoy" || radTreeView1.SelectedNode.Name == "preavisosXNumPedido" || radTreeView1.SelectedNode.Name == "preavisosXFecha"
               || radTreeView1.SelectedNode.Name == "preavisosSSCC")
            {
                cargarPreavisos(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "maquinasXNombre" || radTreeView1.SelectedNode.Name == "maquinasXZona")
            {
                cargarMaquina(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == "tareasPendientes" || radTreeView1.SelectedNode.Name == "tareasPorFecha" ||
                radTreeView1.SelectedNode.Name == "tareasPorTipo")
            {
                cargarTareasPendientes(radTreeView1.SelectedNode.Name);
            }
            if (radTreeView1.SelectedNode.Name == Constantes.STOCKPORARTICULO || radTreeView1.SelectedNode.Name == Constantes.STOCKPORLOTE ||
                radTreeView1.SelectedNode.Name == Constantes.STOCKCOMPARATIVA)
            {
                cargarComparativaStock(radTreeView1.SelectedNode.Name);
            }
            radTreeView1.ClearSelection();
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        private void cargarPackingList(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "packingHoy":
                    queryACargar = 1;
                    break;

                case "packingPendienteCarga":
                    queryACargar = 2;
                    break;

                case "packingPendienteCargaHoy":
                    queryACargar = 3;
                    break;

                case "packingPendienteCargaHoySinOrden":
                    queryACargar = 4;
                    break;

                case "packingPendienteCargaHoyConOrden":
                    queryACargar = 5;
                    break;

                case "packingXNumero":
                    queryACargar = 6;
                    break;

                case "packingXFecha":
                    queryACargar = 7;
                    break;

                case "packingXUbicacion":
                    queryACargar = 8;
                    break;

                case "packingXEstados":
                    queryACargar = 9;
                    break;
            }

            RadTabbedFormControlTab tabPackingList = new RadTabbedFormControlTab("Packing List");
            Packing packing = new Packing(queryACargar);
            tabPackingList.Controls.Add(packing);
            tabControl1.Controls.Add(tabPackingList);
            tabControl1.SelectedTab = tabPackingList;
            packing.Dock = DockStyle.Fill;
            tabControl1.Dock = DockStyle.Fill;
            packing.btnVerSqlPacking.Click += btnVerSql_Event;

            #region Visores

            packing.btnVerPedidos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            packing.btnVerOrdenes.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            packing.btnVerCarga.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });
            packing.btnVerContenido.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });

            #endregion Visores
        }

        public void cargarPedidos(string name)
        {
            int queryACargar = 0;
            string[] tabStrPedidosTan = { Lenguaje.traduce("Pedidos de Hoy"), Lenguaje.traduce("Pedidos Pendientes de Carga"), Lenguaje.traduce("Pedidos por Número de Pedido"), Lenguaje.traduce("Pedidos por Fecha Prevista de Envío"), Lenguaje.traduce("Por Código de Ruta"), Lenguaje.traduce("Por Código de Cliente"), Lenguaje.traduce("Por Estado de Pedido") };
            switch (name)
            {
                case "pedidosHoy":
                    queryACargar = 1;
                    break;

                case "pedidosPend":
                    queryACargar = 2;
                    break;

                case "pedidosXNum":
                    queryACargar = 3;
                    break;

                case "pedidosXFecha":
                    queryACargar = 4;
                    break;

                case "pedidosXCodigoRuta":
                    queryACargar = 5;
                    break;

                case "pedidosXCodigoCliente":
                    queryACargar = 6;
                    break;

                case "pedidosXEstado":
                    queryACargar = 7;
                    break;
            }

            PedidosTan controlPedidos = new PedidosTan(queryACargar);
            RadTabbedFormControlTab tabPedidos = new RadTabbedFormControlTab(queryACargar == 0 ? Lenguaje.traduce("Pedidos Cliente") : tabStrPedidosTan[queryACargar - 1]);
            controlPedidos.tableLayoutPanelTabPedidos.Dock = DockStyle.Fill;
            controlPedidos.gridPedidos.Dock = DockStyle.Fill;
            tabPedidos.Controls.Add(controlPedidos);
            tabControl1.Controls.Add(tabPedidos);
            tabControl1.SelectedTab = tabPedidos;
            controlPedidos.Dock = DockStyle.Fill;
            controlPedidos.btnVerSqlPedidos.Click += btnVerSql_Event;

            #region Visores

            controlPedidos.btnVerPacking.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            controlPedidos.btnVerOrdenes.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            controlPedidos.btnVerContenido.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                //cargarRegularizacionesSalida("");
                cargarOrdenesMovimientosSalida("");
            });
            controlPedidos.btnVerCarga.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });

            #endregion Visores
        }

        private void cargarOrdenesRec(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "ordenesRecHoy":
                    queryACargar = 1;
                    break;

                case "ordenesRecPendCarga":
                    queryACargar = 2;
                    break;

                case "ordenesRecXNumOrden":
                    queryACargar = 3;
                    break;

                case "ordenesRecXEstado":
                    queryACargar = 4;
                    break;

                case "ordenesRecXFecha":
                    queryACargar = 5;
                    break;
            }

            OrdenesRecogidaControl ordenesRecControl = new OrdenesRecogidaControl(queryACargar);
            RadTabbedFormControlTab tabOrdenesRecControl = new RadTabbedFormControlTab(Lenguaje.traduce("OrdenesRec"));
            ordenesRecControl.tableLayoutPanelOrdenesRec.Dock = DockStyle.Fill;
            ordenesRecControl.gridOrdenRec.Dock = DockStyle.Fill;
            tabOrdenesRecControl.Controls.Add(ordenesRecControl);
            tabControl1.Controls.Add(tabOrdenesRecControl);
            tabControl1.SelectedTab = tabOrdenesRecControl;
            ordenesRecControl.Dock = DockStyle.Fill;
            ordenesRecControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            ordenesRecControl.btnVerPacking.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            ordenesRecControl.btnVerPedidos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            ordenesRecControl.btnVerPedidosIncluidos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            ordenesRecControl.btnVerContenido.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });
            ordenesRecControl.btnVerCarga.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });

            #endregion Visores
        }

        private void cargarOrdenesCarga(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "ordenesCargaHoy":
                    queryACargar = 1;
                    break;

                case "ordenesCargaPendCarga":
                    queryACargar = 2;
                    break;

                case "ordenesCargaXNumOrden":
                    queryACargar = 3;
                    break;

                case "ordenesCargaXEstado":
                    queryACargar = 4;
                    break;

                case "ordenesCargaXFecha":
                    queryACargar = 5;
                    break;

                case "ordenesCargaXIdent":
                    queryACargar = 6;
                    break;
            }

            OrdenesCargaControl ordenesCargaControl = new OrdenesCargaControl(queryACargar);
            RadTabbedFormControlTab tabOrdenesCarga = new RadTabbedFormControlTab(Lenguaje.traduce("OrdenesCarga"));
            ordenesCargaControl.tableLayoutPanelOrdenesCarga.Dock = DockStyle.Fill;
            ordenesCargaControl.gridOrdenCarga.Dock = DockStyle.Fill;
            tabOrdenesCarga.Controls.Add(ordenesCargaControl);
            tabControl1.Controls.Add(tabOrdenesCarga);
            tabControl1.SelectedTab = tabOrdenesCarga;
            ordenesCargaControl.Dock = DockStyle.Fill;
            ordenesCargaControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            ordenesCargaControl.btnVerPacking.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            ordenesCargaControl.btnVerOrdenes.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            ordenesCargaControl.btnVerCont.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });
            ordenesCargaControl.btnVerPedidos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });

            #endregion Visores
        }

        private void cargarArticulos(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "articulosEntradas":
                    queryACargar = 1;
                    break;

                case "articulosSalidas":
                    queryACargar = 2;
                    break;

                case "entradasEntreFechas":
                    queryACargar = 3;
                    break;

                case "salidasEntreFechas":
                    queryACargar = 4;
                    break;
            }
            Articulos articulosControl = new Articulos(queryACargar);
            RadTabbedFormControlTab tabArticulos = new RadTabbedFormControlTab(Lenguaje.traduce("Articulos"));
            articulosControl.tableLayoutPanelArticulos.Dock = DockStyle.Fill;
            articulosControl.gridArticulos.Dock = DockStyle.Fill;
            tabArticulos.Controls.Add(articulosControl);
            tabControl1.Controls.Add(tabArticulos);
            tabControl1.SelectedTab = tabArticulos;
            articulosControl.Dock = DockStyle.Fill;
            articulosControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            articulosControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            articulosControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });
            articulosControl.btnVerRecepciones.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRecepciones("");
            });
            articulosControl.btnVerPedidosProveedor.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidosProv("");
            });
            articulosControl.btnVerDevolucionesCliente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarDevolucionesCliente("");
            });
            articulosControl.btnVerOrdenesFabricacion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesFabricacion("");
            });
            articulosControl.btnVerRegularizacion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesEntrada("");
            });
            articulosControl.btnVerPacking.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            articulosControl.btnVerOrdenesRecogida.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            articulosControl.btnVerPedidosCliente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            articulosControl.btnVerCargaCamion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });
            articulosControl.btnVerConsumos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });

            #endregion Visores
        }

        private void cargarOrdenesMovimientosSalida(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "salidasHoy":
                    queryACargar = 1;
                    break;

                case "salidasXArticulo":
                    queryACargar = 2;
                    break;

                case "salidasXArticuloLote":
                    queryACargar = 3;
                    break;

                case "salidasXFechas":
                    queryACargar = 4;
                    break;
            }
            OrdenesMovimientosSalidaControl ordenesMovimientosSalida = new OrdenesMovimientosSalidaControl(queryACargar);
            RadTabbedFormControlTab tabMovimientosSalida = new RadTabbedFormControlTab(Lenguaje.traduce("MovimientosSalida"));
            ordenesMovimientosSalida.tableLayoutPanelMovimientosSalida.Dock = DockStyle.Fill;
            ordenesMovimientosSalida.gridMovSalida.Dock = DockStyle.Fill;
            tabMovimientosSalida.Controls.Add(ordenesMovimientosSalida);
            tabControl1.Controls.Add(tabMovimientosSalida);
            tabControl1.SelectedTab = tabMovimientosSalida;
            ordenesMovimientosSalida.Dock = DockStyle.Fill;
            ordenesMovimientosSalida.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            ordenesMovimientosSalida.btnVerPedidos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            ordenesMovimientosSalida.btnVerOrdenes.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            ordenesMovimientosSalida.btnVerContenido.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                /*cargarRegularizacionesSalida("");*/
                cargarOrdenesMovimientosSalida("");
            });
            ordenesMovimientosSalida.btnVerCarga.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });
            ordenesMovimientosSalida.btnVerEntrada.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });

            #endregion Visores
        }

        private void cargarMatricula(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "entradasMatricula":
                    queryACargar = 1;
                    break;

                case "salidasMatricula":
                    queryACargar = 2;
                    break;

                case "entradasMatrSerie":
                    queryACargar = 3;
                    break;

                case "salidasMatrSerie":
                    queryACargar = 4;
                    break;
            }
            Matriculas matriculas = new Matriculas(queryACargar);
            RadTabbedFormControlTab tabMatriculas = new RadTabbedFormControlTab(Lenguaje.traduce("Matriculas"));
            matriculas.tableLayoutPanelMatriculas.Dock = DockStyle.Fill;
            matriculas.gridMatriculas.Dock = DockStyle.Fill;
            tabMatriculas.Controls.Add(matriculas);
            tabControl1.Controls.Add(tabMatriculas);
            tabControl1.SelectedTab = tabMatriculas;
            matriculas.Dock = DockStyle.Fill;
            matriculas.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            matriculas.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            matriculas.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });
            matriculas.btnVerRecepciones.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRecepciones("");
            });
            matriculas.btnVerPedidosProveedor.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidosProv("");
            });
            matriculas.btnVerDevolucionesCliente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarDevolucionesCliente("");
            });
            matriculas.btnVerOrdenesFabricacion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesFabricacion("");
            });
            matriculas.btnVerRegularizacion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            matriculas.btnVerPacking.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            matriculas.btnVerOrdenesRecogida.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            matriculas.btnVerPedidosCliente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            matriculas.btnVerCargaCamion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });
            matriculas.btnVerConsumos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });

            #endregion Visores
        }

        private void cargarPedidosProv(string name)
        {
            int queryACargar = 0;

            switch (name)
            {
                case "pedidosProvHoy":
                    queryACargar = 1;
                    break;

                case "pedidosProvPendientes":
                    queryACargar = 2;
                    break;

                case "pedidosProvXReferencia":
                    queryACargar = 3;
                    break;

                case "pedidosProvXFecha":
                    queryACargar = 4;
                    break;

                case "pedidosProvXCodigo":
                    queryACargar = 5;
                    break;

                default:
                    break;
            }
            PedidosProv pedidosProvControl = new PedidosProv(queryACargar);
            RadTabbedFormControlTab pedidosProvTab = new RadTabbedFormControlTab(Lenguaje.traduce("PedidosProv"));
            pedidosProvTab.Controls.Add(pedidosProvControl);
            tabControl1.Controls.Add(pedidosProvTab);
            tabControl1.SelectedTab = pedidosProvTab;
            pedidosProvControl.Dock = DockStyle.Fill;
            pedidosProvControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            pedidosProvControl.btnVerRecepciones.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRecepciones("");
            });
            pedidosProvControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            pedidosProvControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            pedidosProvControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });
            pedidosProvControl.btnVerPendiente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarGenerico();
            });

            #endregion Visores
        }

        private void cargarRecepciones(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "recepcionesHoy":
                    queryACargar = 1;
                    break;

                case "recepcionesPend":
                    queryACargar = 2;
                    break;

                case "recepcionesXNumRecepcion":
                    queryACargar = 3;
                    break;

                case "recepcionesXFecha":
                    queryACargar = 4;
                    break;

                case "recepcionesXEstado":
                    queryACargar = 5;
                    break;
            }
            Recepciones recepcionesControl = new Recepciones(queryACargar);
            RadTabbedFormControlTab recepcionesTab = new RadTabbedFormControlTab(Lenguaje.traduce("Recepciones"));
            recepcionesTab.Controls.Add(recepcionesControl);
            tabControl1.Controls.Add(recepcionesTab);
            tabControl1.SelectedTab = recepcionesTab;
            recepcionesControl.Dock = DockStyle.Fill;
            recepcionesControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            recepcionesControl.btnVerPedidosProv.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidosProv("");
            });
            recepcionesControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            recepcionesControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            recepcionesControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });
            recepcionesControl.btnVerPendiente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarGenerico();
            });

            #endregion Visores
        }

        private void cargarDevolucionesCliente(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "devolucionesCliHoy":
                    queryACargar = 1;
                    break;

                case "devolucionesCliPendientes":
                    queryACargar = 2;
                    break;

                case "devolucionesCliXNumero":
                    queryACargar = 3;
                    break;

                case "devolucionesCliXFecha":
                    queryACargar = 4;
                    break;

                case "devolucionesCliXEstado":
                    queryACargar = 5;
                    break;
            }
            DevolucionesCliente devolucionesCliControl = new DevolucionesCliente(queryACargar);
            RadTabbedFormControlTab devolucionesCliTab = new RadTabbedFormControlTab(Lenguaje.traduce("DevolucionesCli"));
            devolucionesCliTab.Controls.Add(devolucionesCliControl);
            tabControl1.Controls.Add(devolucionesCliTab);
            tabControl1.SelectedTab = devolucionesCliTab;
            devolucionesCliControl.Dock = DockStyle.Fill;
            devolucionesCliControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            devolucionesCliControl.btnVerLineas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarGenerico();
            });
            devolucionesCliControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            devolucionesCliControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            devolucionesCliControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });

            #endregion Visores
        }

        private void cargarDevolucionesProveedor(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "devolucionesProvHoy":
                    queryACargar = 1;
                    break;

                case "devolucionesProvPendientes":
                    queryACargar = 2;
                    break;

                case "devolucionesProvXNumero":
                    queryACargar = 3;
                    break;

                case "devolucionesProvXFecha":
                    queryACargar = 4;
                    break;

                case "devolucionesProvXEstado":
                    queryACargar = 5;
                    break;
            }
            DevolucionesProveedor devolucionesProvControl = new DevolucionesProveedor(queryACargar);
            RadTabbedFormControlTab devolucionesProvTab = new RadTabbedFormControlTab(Lenguaje.traduce("DevolProv"));
            devolucionesProvTab.Controls.Add(devolucionesProvControl);
            tabControl1.Controls.Add(devolucionesProvTab);
            tabControl1.SelectedTab = devolucionesProvTab;
            devolucionesProvControl.Dock = DockStyle.Fill;
            devolucionesProvControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            devolucionesProvControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            devolucionesProvControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            devolucionesProvControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });

            #endregion Visores
        }

        private void cargarOrdenesFabricacion(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "ordenesFabHoy":
                    queryACargar = 1;
                    break;

                case "ordenesFabPendientes":
                    queryACargar = 2;
                    break;

                case "ordenesFabSinAsignar":
                    queryACargar = 3;
                    break;

                case "ordenesFabXNumOrden":
                    queryACargar = 4;
                    break;

                case "ordenesFabXFecha":
                    queryACargar = 5;
                    break;

                case "ordenesFabXFechaYArt":
                    queryACargar = 6;
                    break;
            }
            OrdenesFabricacion ordenesFabControl = new OrdenesFabricacion(queryACargar);
            RadTabbedFormControlTab ordenesFabTab = new RadTabbedFormControlTab(Lenguaje.traduce("OrdenesFab"));
            ordenesFabTab.Controls.Add(ordenesFabControl);
            tabControl1.Controls.Add(ordenesFabTab);
            tabControl1.SelectedTab = ordenesFabTab;
            ordenesFabControl.Dock = DockStyle.Fill;
            ordenesFabControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            ordenesFabControl.btnVerMaquina.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMaquina("");
            });
            ordenesFabControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            ordenesFabControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            ordenesFabControl.btnVerConsumos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });
            ordenesFabControl.btnVerOtrasOrdenes.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesFabricacion("");
            });
            ordenesFabControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });

            #endregion Visores
        }

        private void cargarRegularizacionesEntrada(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "entradasRegularizacionHoy":
                    queryACargar = 1;
                    break;

                case "entradasRegularizacionXFecha":
                    queryACargar = 2;
                    break;

                case "entradasRegularizacionXArticulo":
                    queryACargar = 3;
                    break;

                case "entradasRegularizacionXMotivo":
                    queryACargar = 4;
                    break;
            }
            RegulaEntrada regularizacionesEntradaControl = new RegulaEntrada(queryACargar);
            RadTabbedFormControlTab regularizacionesEntradaTab = new RadTabbedFormControlTab(Lenguaje.traduce("Reg.Entrada"));
            regularizacionesEntradaTab.Controls.Add(regularizacionesEntradaControl);
            tabControl1.Controls.Add(regularizacionesEntradaTab);
            tabControl1.SelectedTab = regularizacionesEntradaTab;
            regularizacionesEntradaControl.Dock = DockStyle.Fill;
            regularizacionesEntradaControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            regularizacionesEntradaControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            regularizacionesEntradaControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });
            regularizacionesEntradaControl.btnVerRecepciones.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRecepciones("");
            });
            regularizacionesEntradaControl.btnVerPedidosProv.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidosProv("");
            });
            regularizacionesEntradaControl.btnVerDevolucionesCliente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarDevolucionesCliente("");
            });
            regularizacionesEntradaControl.btnVerOrdenesFab.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesFabricacion("");
            });
            regularizacionesEntradaControl.btnVerRegularizacion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            regularizacionesEntradaControl.btnVerPacking.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            regularizacionesEntradaControl.btnVerOrdenesRecogida.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            regularizacionesEntradaControl.btnVerPedidosCliente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            regularizacionesEntradaControl.btnVerCargaCamion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });
            regularizacionesEntradaControl.btnVerConsumos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });
            regularizacionesEntradaControl.btnVerPendiente.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarGenerico();
            });

            #endregion Visores
        }

        private void cargarRegularizacionesSalida(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "salidasRegularizacionHoy":
                    queryACargar = 1;
                    break;

                case "salidasRegularizacionXFecha":
                    queryACargar = 2;
                    break;

                case "salidasRegularizacionXArticulo":
                    queryACargar = 3;
                    break;

                case "salidasRegularizacionXMotivo":
                    queryACargar = 4;
                    break;
            }
            RegulaSalida regularizacionesSalidaControl = new RegulaSalida(queryACargar);
            RadTabbedFormControlTab regularizacionesSalidaTab = new RadTabbedFormControlTab(Lenguaje.traduce("Reg.Salida"));
            regularizacionesSalidaTab.Controls.Add(regularizacionesSalidaControl);
            tabControl1.Controls.Add(regularizacionesSalidaTab);
            tabControl1.SelectedTab = regularizacionesSalidaTab;
            regularizacionesSalidaControl.Dock = DockStyle.Fill;
            regularizacionesSalidaControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            regularizacionesSalidaControl.btnVerPedidos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            regularizacionesSalidaControl.btnVerOrdenes.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            regularizacionesSalidaControl.btnVerCargaCamion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });
            regularizacionesSalidaControl.btnVerEntrada.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });

            #endregion Visores
        }

        private void cargarPreavisos(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "preavisosHoy":
                    queryACargar = 1;
                    break;

                case "preavisosXNumPedido":
                    queryACargar = 2;
                    break;

                case "preavisosXFecha":
                    queryACargar = 3;
                    break;

                case "preavisosSSCC":
                    queryACargar = 4;
                    break;
            }
            Preavisos preavisosControl = new Preavisos(queryACargar);
            RadTabbedFormControlTab preavisosTab = new RadTabbedFormControlTab(Lenguaje.traduce("Preavisos"));
            preavisosTab.Controls.Add(preavisosControl);
            tabControl1.Controls.Add(preavisosTab);
            tabControl1.SelectedTab = preavisosTab;
            preavisosControl.Dock = DockStyle.Fill;
            preavisosControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            preavisosControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            preavisosControl.btnVerRecepciones.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRecepciones("");
            });
            preavisosControl.btnVerPedidosProv.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidosProv("");
            });

            #endregion Visores
        }

        private void cargarMaquina(string name)
        {
            int queryACargar = 0;
            switch (name)
            {
                case "maquinasXNombre":
                    queryACargar = 1;
                    break;

                case "maquinasXZona":
                    queryACargar = 2;
                    break;
            }
            Maquina maquinaControl = new Maquina(queryACargar);
            RadTabbedFormControlTab maquinaTab = new RadTabbedFormControlTab(Lenguaje.traduce("Maquina"));
            maquinaTab.Controls.Add(maquinaControl);
            tabControl1.Controls.Add(maquinaTab);
            tabControl1.SelectedTab = maquinaTab;
            maquinaControl.Dock = DockStyle.Fill;
            maquinaControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            maquinaControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            maquinaControl.btnVerConsumos.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });
            maquinaControl.btnVerOrdFab.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesFabricacion("");
            });
            maquinaControl.btnVerExistenciasTerm.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            maquinaControl.btnVerExistenciasZona.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });

            #endregion Visores
        }

        private void cargarTareasPendientes(String nombreNode)
        {
            int tipo = 0;
            switch (nombreNode)
            {
                case "tareasPendientes":
                    tipo = 1;
                    break;

                case "tareasPorFecha":
                    tipo = 2;
                    break;

                case "tareasPorTipo":
                    tipo = 3;
                    break;
            }

            TareasPendientes tareasPendientesControl = new TareasPendientes(tipo);
            RadTabbedFormControlTab tareasPendientesTab = new RadTabbedFormControlTab(Lenguaje.traduce("TareasPend"));
            tareasPendientesControl.gridTareas.Dock = DockStyle.Fill;
            tareasPendientesTab.Controls.Add(tareasPendientesControl);
            tabControl1.Controls.Add(tareasPendientesTab);
            tabControl1.SelectedTab = tareasPendientesTab;
            tareasPendientesControl.Dock = DockStyle.Fill;
            tareasPendientesControl.btnVerSql.Click += btnVerSql_Event;
        }

        private void cargarComparativaStock(string nombreNode)
        {
            int tipo = 0;
            switch (nombreNode)
            {
                case Constantes.STOCKCOMPARATIVA:
                    tipo = 1;
                    break;

                case Constantes.STOCKPORARTICULO:
                    tipo = 2;
                    break;

                case Constantes.STOCKPORLOTE:
                    tipo = 3;
                    break;
            }
            ComparativaStock comparativaStockControl = new ComparativaStock(tipo);
            RadTabbedFormControlTab comparativaStockTab = new RadTabbedFormControlTab(Lenguaje.traduce(Constantes.COMPARATIVASTOCKTITULO));
            comparativaStockControl.gridComparativaStock.Dock = DockStyle.Fill;
            comparativaStockTab.Controls.Add(comparativaStockControl);
            tabControl1.Controls.Add(comparativaStockTab);
            tabControl1.SelectedTab = comparativaStockTab;
            comparativaStockControl.Dock = DockStyle.Fill;
            comparativaStockControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            comparativaStockControl.btnVerExistencias.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarExistencias();
            });
            comparativaStockControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesMovimientosSalida("");
            });
            comparativaStockControl.btnVerEntradas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });

            #endregion Visores
        }

        private void cargarGenerico()
        {
            Generico genericoControl = new Generico();
            RadTabbedFormControlTab genericoTab = new RadTabbedFormControlTab(Lenguaje.traduce("Genérico"));
            genericoControl.gridTareas.Dock = DockStyle.Fill;
            genericoTab.Controls.Add(genericoControl);
            tabControl1.Controls.Add(genericoTab);
            tabControl1.SelectedTab = genericoTab;
            genericoControl.Dock = DockStyle.Fill;
            genericoControl.btnVerSql.Click += btnVerSql_Event;
        }

        private void cargarExistencias()
        {
            Existencias existenciasControl = new Existencias();
            RadTabbedFormControlTab existenciasTab = new RadTabbedFormControlTab(Lenguaje.traduce("Existencias"));
            existenciasControl.gridExistencias.Dock = DockStyle.Fill;
            existenciasTab.Controls.Add(existenciasControl);
            tabControl1.Controls.Add(existenciasTab);
            tabControl1.SelectedTab = existenciasTab;
            existenciasControl.Dock = DockStyle.Fill;
            existenciasControl.btnVerSql.Click += btnVerSql_Event;

            #region Visores

            existenciasControl.btnVerEntrada.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarMatricula("");
            });
            existenciasControl.btnVerRegularizacion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });
            existenciasControl.btnVerOrdenesFab.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesFabricacion("");
            });
            existenciasControl.btnVerPedidosPro.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidosProv("");
            });
            existenciasControl.btnVerDevolucionesCli.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarDevolucionesCliente("");
            });
            existenciasControl.btnVerSalidas.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarRegularizacionesSalida("");
            });
            existenciasControl.btnVerPacking.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPackingList("");
            });
            existenciasControl.btnVerOrdenRec.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesRec("");
            });
            existenciasControl.btnVerPedidosCli.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarPedidos("");
            });
            existenciasControl.btnVerCargaCamion.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                cargarOrdenesCarga("");
            });

            #endregion Visores
        }

        private void btnReiniciar_Event(object sender, EventArgs e)
        {
            //tabControl1.RadTabbedFormControlTabs.Clear();
            btnReiniciar.Enabled = false;
            btnReiniciar.Visible = false;
            btnIniciar.Enabled = true;
            btnIniciar.Visible = true;
        }

        private void Permiso(RadButtonItem control, int id)
        {
            if (User.Perm.comprobarAcceso(id) == false)
            {
                control.Enabled = false;
            }
            else
            {
                if (User.Perm.tienePermisoEscritura(id) == true)
                {
                    control.Enabled = true;
                }
                else
                {
                    control.Enabled = false;
                }
            }
        }

        private void btnVerSql_Event(object sender, EventArgs e)
        {
        }

        private void btnSalir_Event(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Menú Colapsable

        private void RadTreeView1_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            foreach (RadTreeNode item in radTreeView1.TreeViewElement.GetNodes())
            {
                if (item.GetNodeCount(true) > 0)
                {
                    if (!item.Expanded)
                    {
                        item.Image = Resources.normal;
                    }
                    else
                    {
                        item.Image = Resources.expanded;
                    }
                }
            }
        }

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node.Expanded && e.Node.GetNodeCount(true) > 0)
            {
                e.Node.Collapse();
                e.Node.Image = Resources.normal;
            }
            else
            {
                if (e.Node.GetNodeCount(true) > 0)
                {
                    e.Node.Expand();
                    e.Node.Image = Resources.expanded;
                }
            }

            foreach (RadTreeNode item in radTreeView1.TreeViewElement.GetNodes())
            {
                //if (item != e.Node && item.Expanded && e.Node.GetNodeCount(true) > 0 && e.Node.Parent != item)
                if (item != e.Node && item.Expanded && e.Node.Parent == null)
                {
                    item.Collapse();
                    item.Image = Resources.normal;
                }
            }
        }

        private void RadCollapsiblePanel1_Expanded(object sender, EventArgs e)
        {
            tableLayoutPanel.ColumnStyles[0].Width = 267;
        }

        private void RadCollapsiblePanel1_Collapsed(object sender, EventArgs e)
        {
            tableLayoutPanel.ColumnStyles[0].Width = 40;
        }

        private void radTreeView1_SelectedNodeChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            /*this.waitingBar.StartWaiting();
            tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 1));
            tableLayoutPanel1.Controls.Add(this.waitingBar, 1, 1);*/

            //this.initialLoader.RunWorkerAsync(argument:e.Node.Name);
        }

        #endregion Menú Colapsable

        private void ShapedForm1_Load(object sender, EventArgs e)
        {
        }

        private void TableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
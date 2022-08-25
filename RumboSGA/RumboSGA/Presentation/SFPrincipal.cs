using RumboSGA.Presentation.UserControls;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGA.Properties;
using RumboSGAManager;
using System;
using System.Deployment;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using RumboSGA.Presentation.UserControls.Mantenimientos.Herramientas;
using RumboSGA.Herramientas;
using RumboSGA.GestionAlmacen;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using RumboSGA.Herramientas.Stock;
using Telerik.WinControls.UI.Localization;
using RumboSGA.Presentation.Herramientas;
using RumboSGAManager.Model;
using System.Configuration;
using System.Diagnostics;
using RumboSGA.Maestros;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System.Reflection;
using Telerik.WinControls.Analytics;
using System.Linq;
using System.Xml;
using System.IO;
using RumboSGA.Presentation.Formulario_Recepciones;
using RumboSGA.Presentation.FormularioRecepciones;
using RumboSGA.Controles;
using System.Collections;
using Telerik.Charting;
using System.Data;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.Herramientas.Ventanas;
using Newtonsoft.Json;
using RumboSGA.GestionAlmacen.RumControlPersonalizado;

namespace RumboSGA.Presentation
{
    public partial class SFPrincipal : Telerik.WinControls.UI.RadRibbonForm
    {
        #region VariablesGlobales

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /*BaseGridControl proveedoresControl, clientesControl, reservaHistoricoControl, maquinasControl, familiasControl, agenciasControl, bomControl,
           zonaIntercambio, rutasControl, rutasPreparacionControl, motivosRegControl, incidenciasInventarioControl, tareasPendientesControl,
           combiPaletsControl, tareasTipoControl, paletsTipoControl, formatosSCCControl, lotesControl, propietariosControl, mantenimientoControl,
           parametrosControl, estMaq, estFab, estExist, pedidoPro, estadoTareas, existenciasHistorico, usuariosControl, usuariosGruposControl, articulosControl, operariosControl, recursosTareaControl, gruposControl, permisos, cargaCabControl;
        */
        private BaseGridControl baseGridControl;
        private ControlTareas conTareas;
        private TorreControl torreControl = null;

        private BaseGridControl clientesPedidosCabControl;
        private BaseGridControl zonaCabControl;
        private BaseGridControl rumLogControl;
        private BaseGridControl operariosMovControl;
        private Trazabilidad herramientaTrazabilidad;

        private Stock herramientaStock;

        private Movimientos herramientaMovimientos;

        private ProveedoresPedidosCabGridView cabGridView;

        private OrdenesRecogida ordenesRecogida;
        private Reposiciones herramientaReposiciones;
        private Reposicion herramientaReposicion;
        private AcopiosProduccion acoProduccion;
        private FrmDevolucionesCliente devolclientes;
        private FrmDevolucionesProveedor devolProveedor;

        private RadRibbonBarGroup grupoLabel = new RadRibbonBarGroup();
        private RumLabelElement lblCantidad = new RumLabelElement();

        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private ArticulosFrm articulosFrm;
        private FrmMapaAlmacen mapaAlmacen;
        private OperariosForm operariosForm;
        private RecursosTareaForm recursosTareaForm;
        private FrmProductividad productividad;
        private Menus menu;
        private int MantenimientoActivo;
        private TorreControl ctrlTwr;
        //private BackgroundWorker initialLoader;
        //private RadWaitingBar waitingBar;

        #endregion VariablesGlobales

        #region Constructor

        public SFPrincipal()
        {
            try
            {
                log.Debug("Iniciando UI");
                InitializeComponent();
                log.Debug("Asignando textos botones");
                ConfigurarIdioma();
                log.Debug("Texto botones asignado");
                //log.Debug("Configurando eventos");
                InicializarEventosBotones();
                this.FormClosing += Form_Closing;
                this.TopMost = false;
                log.Debug("Eventos configurados");
                radCollapsiblePanel1.CollapsiblePanelElement.HeaderElement.HorizontalHeaderAlignment = Telerik.WinControls.UI.RadHorizontalAlignment.Right;
                radCollapsiblePanel1.HeaderText = ConexionSQL.getNombreConexion();
                log.Debug("Configurando menu lateral");
                ConfigurarMenuLateral();
                log.Debug("Menu lateral configurado");
                //Inicializar combo temas
                log.Debug("Iniciando temas");
                InitializeThemesDropDown();
                log.Debug("Temas iniciados");
                //Cargar elementos del menú lateral
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Maximized;
                this.Load += SFPrincipal_Load;
                this.Icon = Properties.Resources.bruju;
                inicializarComboTamañofuente();
                InicializarComboPaginacion();
                log.Debug("UI iniciada");
                headerPaginacion.Text = "Registros Página";
                log.Debug("Creando/Buscando xml progress bar");
                CrearXMLProgressBar();
                //inicializarCuadroControl
                //CheckForIllegalCrossThreadCalls = false;
                if (Persistencia.TorreControl)
                {
                    ctrlTwr = new TorreControl(ref tcuadroControl);
                }
                if (this.Focused == true)
                {
                    log.Info("FOCUS 1");
                }
                addNodeTorreControlTreeMenu();
                log.Debug("Terminado constructor SFPrincipal");
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
            }
        }

        #endregion Constructor

        #region Eventos

        private void SFPrincipal_Load(object sender, EventArgs e)
        {
            this.radCollapsiblePanel1.Collapse();
            radCollapsiblePanel1.Expand();
            radRibbonBar1.Expanded = false;
            radRibbonBar1.CommandTabs.Remove(mantenimientosTab);
        }

        private void InicializarEventosBotones()
        {
            menuItemColumnas.Click += menuItemColumnas_Click;
            menuItemCargar.Click += menuItemCargar_Click;
            menuItemGuardar.Click += menuItemGuardar_Click;
            IncidenciaJira.Click += menuIncidenciaJira_Click;
            menuManual.Click += menuManual_Click;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            OnThemeChanged();
        }

        private void ConfigurarIdioma()
        {
            mantenimientosTab.Text = strings.Acciones;
            menuItemColumnas.Text = strings.Columnas;
            IncidenciaJira.Text = Lenguaje.traduce("Incidencia Jira");
            menuManual.Text = Lenguaje.traduce("Manual");
            menuItemCargar.Text = strings.Cargar;
            menuItemGuardar.Text = strings.Guardar;
            menuItemTemas.Text = strings.Temas;
            menuItemCargarControlTareas.Text = strings.Cargar;
            menuItemGuardarControlTareas.Text = strings.Guardar;
            menuItemColumnasControlTareas.Text = strings.Columnas;
            headerTamLetra.Text = "Tamaño Letra Tabla";
            btnNuevo.Text = strings.Nuevo;
            btnClonar.Text = strings.Clonar;
            bntBorrar.Text = strings.Borrar;
            modBarGroup.Text = strings.Modificaciones;
            vistaBarGroup.Text = strings.Ver;
            configBarGroup.Text = strings.Configuracion;
            btnPedidos.Text = strings.Pedidos;
            btnOperarios.Text = strings.Operario;
            btnCrearRecepcion.Text = strings.CrearRecepcion;
            btnCerrarRecepcion.Text = strings.CerrarRecepcion;
            btnBuscar.Text = strings.Buscar;
            btnAsignarRecurso.Text = strings.AsignarRecurso;
            btnDeshacer.Text = strings.DeshacerEntradas;
            btnCambiarPorPedido.Text = strings.PorPedido;
            //menuItemFiltroInicial.Click += new EventHandler(FiltroInicial_Click);

            RadGridLocalizationProvider.CurrentProvider = new MiRadGridLocalization();
            RadVirtualGridLocalizationProvider.CurrentProvider = new MiRadVirtualGridLocalizationProvider();
            DataFilterLocalizationProvider.CurrentProvider = new MiDataFilterLocalization();
        }

        private void ConfigurarMenuLateral()
        {
            string appPath = Persistencia.ConfigPath;
            this.rtvArbolMenu.ValueMember = "Value";
            string mensajeError = string.Empty;

            try
            {
                this.rtvArbolMenu.LoadXML(appPath + "\\menu.xml", null);
                Debug.WriteLine(appPath + "\\menu.xml");
                menu = new Menus();
                createMenuNodes(this.rtvArbolMenu);
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
            }

            this.rtvArbolMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F);

            //Cargar imágenes del menú
            foreach (RadTreeNode item in rtvArbolMenu.TreeViewElement.GetNodes())
            {
                switch (item.Name)
                {
                    case "herramientasNode":
                        createNodes(item, menu.getMenuHerramientas());
                        break;

                    case "menuInventario":
                        createNodes(item, menu.getMenuInventario());
                        break;

                    case "maestrosNode":
                        createNodes(item, menu.getMenuMaestros());
                        break;

                    case "clientesMainNode":
                        createNodes(item, menu.getMenuClientes());
                        break;

                    case "proveedoresNodeMain":
                        createNodes(item, menu.getMenuProveedores());
                        break;

                    case "historicosNode":
                        createNodes(item, menu.getMenuHistorico());
                        break;

                    case "confNode":
                        createNodes(item, menu.getMenuConfiguracion());
                        break;
                }

                if (item.Nodes.Count > 0)
                {
                    item.Image = Resources.normal;
                    item.Font = new Font(item.Font.FontFamily, 12, FontStyle.Bold);
                }
                else
                {
                    //Nodo hijo
                    item.Font = new Font(item.Font.FontFamily, 10);
                    if (item.Tag != null)
                    {
                        if (!User.Perm.comprobarAcceso(int.Parse(item.Tag.ToString())))
                        {
                            item.Enabled = false;
                        }
                    }
                    else
                    {
                        item.Enabled = false;
                    }
                }
                if (item.Name == "acerca")
                {
                    item.Enabled = true;
                    item.Font = new Font(item.Font.FontFamily, 12, FontStyle.Bold);
                }
            }
            this.radCollapsiblePanel1.CollapsiblePanelElement.HeaderElement.ShowHeaderLine = false;
            this.radCollapsiblePanel1.EnableAnimation = false;
            this.radCollapsiblePanel1.Collapsed += RadCollapsiblePanel1_Collapsed;
            this.radCollapsiblePanel1.Expanded += RadCollapsiblePanel1_Expanded;
            this.rtvArbolMenu.NodeMouseClick += radTreeView1_SelectedNodeChanged;
            this.rtvArbolMenu.NodeMouseClick += RadTreeView1_NodeMouseClick;
            this.rtvArbolMenu.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
        }

        private void lanzarVisorSQL(String nombreFormulario, String nombreJson)
        {
            log.Info("Se ejecuta lanzarVisorSQL(" + nombreFormulario + "," + nombreJson + ") ");
            if (nombreJson.Equals("Articulos"))
            {
                VisorSQLRibbonArticulos vsqla = new VisorSQLRibbonArticulos(nombreFormulario, nombreJson);
            }
            else if (nombreJson.Equals("RumLog"))
            {
                VisorSQLRibbon vsql = new VisorSQLRibbon(nombreFormulario, nombreJson, false, true);
            }
            else
            {
                VisorSQLRibbon vsql = new VisorSQLRibbon(nombreFormulario, nombreJson, false, false);
            }
        }

        private void cargarGridView(RadTreeViewEventArgs e)
        {
            String sql = "";
            try
            {
                List<TableScheme> lstEsquemaTabla = new List<TableScheme>();
                lstEsquemaTabla = null;
                baseGridControl = null;

                switch (e.Node.Name)
                {
                    case "PermisosCambioEstado":
                        AttachGridControlRumControl("PermisosCambio", 30100, "PermisosCambioEstado");
                        break;

                    case "sentenciasNode":
                        AttachGridControlRumControl("Sentencias", 30048, "Sentencias");
                        break;

                    case "accionesNode":
                        AttachGridControlRumControl("Acciones", 30047, "Acciones");
                        break;

                    case "algoritmosNode": //Realizado por Jhon se creó nuevo
                        AttachGridControlRumControl("Algoritmos", 30046, "Algoritmos");
                        break;

                    case "etiquetasNode": //Realizado por Jhon se creó nuevo
                        AttachGridControlRumControl("Etiquetas", 20050, "Etiquetas");
                        break;

                    case "ubicacionesNode": //Realizado por Jhon se creó nuevo
                        AttachGridControlRumControl("Ubicaciones", 20047, "Ubicaciones");
                        break;

                    case "movimientosEmbalajeNode": //Realizado por Jhon se creó nuevo
                        AttachGridControlRumControl(Lenguaje.traduce("Movimientos Embalaje"), 30038, "MovimientosEmbalaje");
                        break;

                    case "packingListNode": //Realizado por Jhon se creó nuevo
                        AttachGridControlRumControl(Lenguaje.traduce("Packing List"), 30033, "PackingList"); //Add to traduction or
                        //AttachGridControlRumControl("Packing List", 3033, "PackingList");
                        break;

                    case "transitoLinNode": //Realizado por Jhon se creó nuevo
                        AttachGridControlRumControl(Lenguaje.traduce("Transito Lineas"), 20099, "TransitoLin");
                        break;

                    case "presentacionesNode": //Realizado por Jhon se creó nuevo
                        AttachGridControlRumControl(Lenguaje.traduce("Presentaciones"), 30067, "Presentaciones");
                        break;

                    case "proveedoresNode":
                        //AttachGridControl<ProveedoresControl>(ref baseGridControl, Lenguaje.
                        //(NombresFormularios.Proveedores), 20025);
                        AttachGridControlRumControl(Lenguaje.traduce(NombresFormularios.Proveedores), 20025, "Proveedores");
                        break;

                    case "proveedoresPedidosNode":
                        //PRUEBA PARA NO ABRIR EL MISMO FORMULARIO 2 VECES
                        //if (Application.OpenForms.Cast<Form>().Any(form => form.Name == NombresFormularios.Recepciones))
                        //{
                        //    int a = 0;
                        //}
                        LanzarPedidoProveedor<ProveedoresPedidosCabGridView>(cabGridView, NombresFormularios.Recepciones, 20046);
                        //AttachGridControlRumControl(NombresFormularios.Recepciones, 20046, "ProveedoresPedidosCab");
                        break;

                    case "estadoPedidoNode":
                        AttachGridControl<PedidoProEstadoControl>(ref baseGridControl, NombresFormularios.EstadoPedidos, 40006);
                        break;

                    case "clientesNode":
                        AttachGridControl<ClientesControl>(ref baseGridControl, NombresFormularios.Clientes, 20007);
                        break;

                    case "clientesPedidosNode":
                        AttachGridControl<ClientesPedidosCabControl>(ref clientesPedidosCabControl, NombresFormularios.ClientesPedidos, 20023);
                        break;

                    case "clientesPedidosNuevoNode":
                        AttachGridControlRumControl("Clientes Pedidos", 20023, "ClientesPedidosCab");
                        break;

                    case "reservaHistoricoNode":
                        //AttachGridControl<ReservaHistoricoControl>(ref baseGridControl, NombresFormularios.ReservaHist, 30018);
                        sql = "ReservaHistorico";
                        /*sql = "";
                        if (String.IsNullOrEmpty(DataAccess.getExistenciaHistorico))
                            sql = Utilidades.lecturaJsonSelect("ReservaHistorico", "");
                        else
                            sql = DataAccess.getReservasHistorico;
                            */
                        lanzarVisorSQL(Lenguaje.traduce("Reserva historico"), sql);
                        break;

                    case "existenciasHistoricoNode":
                        /*AttachGridControl<ExistenciasHistoricoControl>(ref baseGridControl, NombresFormularios.ExistenciasHist, 30019);*/
                        sql = "ExistenciasHistorico";//Temporalmente se leera del JSON
                        /*if (String.IsNullOrEmpty(DataAccess.getExistenciaHistorico))
                            sql = Utilidades.lecturaJsonSelect("ExistenciasHistorico", "");
                        else
                            sql = DataAccess.getExistenciaHistorico;
                        */
                        lanzarVisorSQL(Lenguaje.traduce("Existencias historico"), sql);
                        break;

                    case "existenciasHistoricoNode2":
                        /*AttachGridControl<ExistenciasHistoricoControl>(ref baseGridControl, NombresFormularios.ExistenciasHist, 30019);*/
                        sql = "ExistenciasHistorico";//Temporalmente se leera del JSON
                        /*if (String.IsNullOrEmpty(DataAccess.getExistenciaHistorico))
                            sql = Utilidades.lecturaJsonSelect("ExistenciasHistorico", "");
                        else
                            sql = DataAccess.getExistenciaHistorico;
                        */
                        AttachGridControlRumControl("Existencias Historico", 30019, "ExistenciasHistorico");
                        break;

                    case "recepcionesHistoricoNode":
                        AttachGridControlRumControl("Recepciones Historico", 30030, "RecepcionesHistorico");
                        break;

                    case "devolucionesClienteMantenimientoNode":
                        AttachGridControlRumControl("Mantenimiento Devoluciones", 20065, "DevolucionesClienteMantenimiento");
                        break;

                    case "devolucionesProveedorMantenimientoNode":
                        AttachGridControlRumControl("Mantenimiento Devoluciones", 30006, "DevolucionesProveedorMantenimiento");
                        break;

                    case "pedidosProveedorMantenimientoNode":
                        AttachGridControlRumControl("Mantenimiento Pedidos Proveedor", 20046, "PedidosProveedorMantenimiento");
                        break;

                    case "ordenesRecogidaMantenimientoNode":
                        AttachGridControlRumControl("Mantenimiento Expediciones", 20004, "OrdenesRecogidaMantenimiento");
                        break;

                    case "stockStatusNode":
                        //AttachGridControl<StockStatusControl>(ref baseGridControl, NombresFormularios.StockStatus, 30019);
                        sql = "StockStatus";
                        lanzarVisorSQL(Lenguaje.traduce("Stock Status"), sql);
                        break;

                    case "maquinasNode":
                        //AttachGridControl<MaquinasControl>(ref baseGridControl, NombresFormularios.Maquinas, 20070);
                        AttachGridControlRumControl(NombresFormularios.Maquinas, 20070, "Maquinas");
                        break;

                    case "familiasNode":
                        //AttachGridControl<FamiliasControl>(ref baseGridControl, NombresFormularios.Familias, 20052);
                        AttachGridControlRumControl(NombresFormularios.Familias, 20052, "Familias");
                        break;

                    case "agenciasNode":
                        //AttachGridControl<AgenciasControl>(ref baseGridControl, NombresFormularios.Agencias, 20000);
                        AttachGridControlRumControl(NombresFormularios.Agencias, 20000, "Agencias");
                        break;

                    case "bomNode":
                        //AttachGridControl<BomControl>(ref baseGridControl, NombresFormularios.BOM, 20006);
                        AttachGridControlRumControl(NombresFormularios.BOM, 20006, "Bom");
                        break;

                    case "zonaIntercambioNode":
                        //AttachGridControl<ZonaIntercambioControl>(ref baseGridControl, NombresFormularios.ZonaIntercambio, 20097);
                        AttachGridControlRumControl(NombresFormularios.ZonaIntercambio, 20097, "ZonaIntercambio");
                        break;

                    case "rutasNode":
                        //AttachGridControl<RutasControl>(ref baseGridControl, NombresFormularios.Rutas, 20075);
                        AttachGridControlRumControl(NombresFormularios.Rutas, 20075, "Rutas");
                        break;

                    case "rutasPreparacionNode":
                        //AttachGridControl<RutasPreparacionControl>(ref baseGridControl, NombresFormularios.RutasPrep, 20080);
                        AttachGridControlRumControl("Rutas Preparación", 20080, "RutasPreparacion");
                        break;

                    case "motivosRegularizacionNode":
                        //AttachGridControl<MotivosRegControl>(ref baseGridControl, NombresFormularios.MotivosReg, 20016);
                        AttachGridControlRumControl("Motivos Regulacion", 20016, "MotivosReg");
                        break;

                    case "tareasPendientesNode":
                        //AttachGridControl<TareasPendientesControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.TareasPendientes), 20062);
                        AttachGridControlRumControl("Tareas Pendientes", 20062, "TareasPendientes");
                        break;

                    case "combiPaletsNode":
                        //AttachGridControl<CombiPaletsControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.CombiPalets), 20085);
                        AttachGridControlRumControl("Combi Palets", 20085, "CombiPalets");
                        break;

                    case "tareasTipoNode": //Realizado por Jhon
                        //AttachGridControl<TareasTipoControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.TareasTipo), 20061);
                        AttachGridControlRumControl("TareasTipo", 20061, "TareasTipo");
                        break;

                    case "paletsTipoNode": //Realizado por Jhon
                        //AttachGridControl<PaletsTipoControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.PaletsTipo), 20020);
                        AttachGridControlRumControl("PaletsTipo", 20020, "PaletsTipo");
                        break;

                    case "formatosNode": //Realizado por Jhon
                        //AttachGridControl<FormatosSCCControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.FormatosSCC), 20102);
                        AttachGridControlRumControl("Formatos", 20102, "Formatos");
                        break;

                    case "lotesNode": //Realizado por Jhon
                        //AttachGridControl<LotesControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.Lotes), 20092);
                        AttachGridControlRumControl("Lotes", 20092, "Lotes");
                        break;

                    case "propietariosNode":
                        //AttachGridControl<PropietariosControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.Propietarios), 30041);
                        AttachGridControlRumControl("Propietarios", 30041, "Propietarios");
                        break;

                    case "rumMantenimientoNode":
                        //AttachGridControl<MantenimientoControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.Mantenimiento), 20089);
                        AttachGridControlRumControl("Mantenimientos", 20089, "Mantenimiento");
                        break;

                    case "parametrosNode": //Realizado por Jhon
                        //AttachGridControl<ParametrosControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.Parametros), 20021);
                        AttachGridControlRumControl("Parametros", 20021, "Parametros");
                        break;

                    case "usuariosNode": //Realizado por Jhon
                        AttachGridControl<UsuariosControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.Usuarios), 20041);
                        //AttachGridControlRumControl("Usuarios", 20041, "Usuarios");
                        break;

                    case "usuariosGruposNode": //Realizado por Jhon
                        //AttachGridControl<UsuariosGruposControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.UsuariosGrupos), 20040);
                        AttachGridControlRumControl("UsuariosGrupos", 20040, "UsuariosGrupos");
                        break;

                    case "stockNNode":
                        lanzarVisorSQL(Lenguaje.traduce("Stock"), "stock");
                        break;

                    case "stockNode":
                        LanzarStock<Stock>(herramientaStock, Lenguaje.traduce(NombresFormularios.Stock), 20060);
                        break;

                    case "EANProveedor":
                        AttachGridControlRumControl("EAN Proveedor", 20002, "EANProveedor");
                        break;

                    case "trazabilidadNode":
                        herramientaTrazabilidad = new Trazabilidad();
                        LanzarTrazabilidad<Trazabilidad>(herramientaTrazabilidad, Lenguaje.traduce(NombresFormularios.Trazabilidad));
                        break;

                    case "devolProveedor":
                        LanzarDevolucionesProveedor<FrmDevolucionesProveedor>(devolProveedor, Lenguaje.traduce(NombresFormularios.DevolProveedor), 30006);
                        break;

                    case "devolCliente":
                        /*DevolucionesCliente devolCli = new DevolucionesCliente();*/
                        /*FrmDevolucionesCliente devolCli = new FrmDevolucionesCliente();
                        devolCli.Show();
                        devolCli.TopMost = true;
                        devolCli.TopMost = false;*/
                        LanzarDevolucionesCliente<FrmDevolucionesCliente>(devolclientes, Lenguaje.traduce(NombresFormularios.DevolClientes));
                        break;

                    case "ordenesRecogidaNode":
                        LanzarOrdenesRecogida<OrdenesRecogida>(ordenesRecogida, Lenguaje.traduce(NombresFormularios.Expediciones));
                        break;

                    case "reposiciones":
                        herramientaReposicion = new Reposicion();
                        //LanzarReposiciones<Reposiciones>(herramientaReposiciones, NombresFormularios.Reposiciones,20067);
                        break;

                    case "permisosNode":
                        //AttachGridControl<PermisosControl>(ref baseGridControl, "Permisos", 20026);
                        AttachGridControlRumControl("Permisos", 20026, "Permisos");
                        break;

                    case "inventarioInicialNode":
                        InventarioInicial inv = new InventarioInicial();
                        break;

                    case "incidenciasInventarioNode":
                        //AttachGridControl<IncidenciasInventarioControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.IncidenciasInventario), 20081);
                        AttachGridControlRumControl("Incidencias Inventario", 20081, "IncidenciasInventario");
                        break;

                    case "movimientosNode":
                        herramientaMovimientos = new Movimientos();
                        LanzarMovimientos<Movimientos>(herramientaMovimientos, Lenguaje.traduce(NombresFormularios.Movimientos));
                        break;

                    case "controlTareasNode":
                        LanzarControlTareas<ControlTareas>(conTareas, Lenguaje.traduce(NombresFormularios.ControlTareas), 20091);
                        break;

                    case "acopiosProduccionNode":
                        LanzarAcopiosProduccion<AcopiosProduccion>(acoProduccion, Lenguaje.traduce(NombresFormularios.AcopiosProduccion));
                        break;

                    case "estadoFabNode":
                        AttachGridControlRumControl(Lenguaje.traduce(NombresFormularios.EstadoFab), 40007, "EstadoPedido");
                        //AttachGridControl<EstadoFabricacionControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.EstadoFab), 40007);
                        break;

                    case "estadoMaqNode":
                        AttachGridControlRumControl(Lenguaje.traduce(NombresFormularios.EstadoMaq), 20071, "EstadoMaquina");
                        //AttachGridControl<EstadoMaquinaControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.EstadoMaq), 20071);
                        break;

                    case "estadoPedidoClienteNode":
                        AttachGridControlRumControl("Estados Pedido Cliente", 20044, "EstadoPedidoCliente");
                        //AttachGridControl<EstadoMaquinaControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.EstadoMaq), 20071);
                        break;

                    case "estadoExistNode":
                        AttachGridControlRumControl(Lenguaje.traduce(NombresFormularios.EstadoFab), 20071, "EstadoExistencias");
                        //AttachGridControl<EstadoExistenciasControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.EstadoExistencias), 20011);
                        break;

                    case "ordenFabricacion":
                        AttachGridControlRumControl("Orden Fabricacion", 20063, "OrdenFabricacionCab");
                        break;

                    case "articulosNode":

                        sql = "Articulos";
                        lanzarVisorSQL(Lenguaje.traduce("Artículos"), sql);
                        //ArticulosForm articulosForm = new ArticulosForm(articulosControl);

                        //Anterior
                        //LanzarArticulosGridView<ArticulosFrm>(articulosFrm, Lenguaje.traduce(NombresFormularios.Articulos), 20004);
                        break;

                    case "ubicaciones":
                        FrmUbicaciones formubi = new FrmUbicaciones();
                        formubi.Show();
                        break;

                    case "operarioNode":
                        AttachGridControlRumControl("Operarios", 20018, "Operarios");
                        //AttachGridControl<OperariosControl>(ref operariosControl, Lenguaje.traduce("Operarios"),20018);
                        //LanzarOperariosGridView<OperariosForm>(operariosForm, Lenguaje.traduce(NombresFormularios.Operarios), 20018);
                        break;

                    case "recursosTarea":
                        FrmGestorRecursos gestorRecursos = new FrmGestorRecursos();
                        gestorRecursos.Show();
                        //LanzarRecursosTareaGridView<RecursosTareaForm>(recursosTareaForm, Lenguaje.traduce(NombresFormularios.RecursosTarea), 20051);
                        break;

                    case "grupo":
                        AttachGridControl<GruposControl>(ref baseGridControl, Lenguaje.traduce(NombresFormularios.Usuarios), 20013);
                        break;

                    case "cargaCabNode":
                        AttachGridControl<CargaCabControl>(ref baseGridControl, Lenguaje.traduce("Órdenes de carga"), 30035);
                        break;
                    //case "planificacion":
                    //    //PlanificacionDiariaRecepciones p = new PlanificacionDiariaRecepciones();
                    //    //p.ShowDialog();
                    //break;
                    case "mapaAlmacenNode":
                        LanzarMapaAlmacen<FrmMapaAlmacen>(mapaAlmacen, Lenguaje.traduce(NombresFormularios.MapaAlmacen));
                        break;

                    case "zonaCabNode":
                        AttachGridControlRumControl(Lenguaje.traduce(NombresFormularios.ZonaLogica), 20042, "ZonaLogicaCab");
                        //AttachGridControl<ZonaLogicaCabControl>(ref zonaCabControl, NombresFormularios.ZonaLogica, 20042);
                        break;

                    case "productividadNode":
                        LanzarProductividad<FrmProductividad>(productividad, Lenguaje.traduce(NombresFormularios.Productividad), 20083);
                        break;

                    case "acerca":
                        AcercaDe acerca = new AcercaDe();
                        acerca.ShowDialog();
                        break;

                    case "TorreControl":
                        if (torreControl == null)
                            torreControl = new TorreControl(ref tcuadroControl);
                        cerrarMantenimientos();
                        break;

                    case "visorLogNode":
                        // AttachGridControl<RumLogControl>(ref rumLogControl, NombresFormularios.VisorLog, 30024);
                        sql = "RumLog";
                        lanzarVisorSQL(Lenguaje.traduce("Log Viewer"), sql);
                        break;

                    case "operariosMovNode":
                        //AttachGridControl<OperariosMovControl>(ref operariosMovControl, NombresFormularios.OperariosMov, 20083);
                        sql = "OperariosMovimientos";
                        lanzarVisorSQL(Lenguaje.traduce("Movimientos Historico"), sql);
                        break;

                    case "permisosEstadoNode":
                        AttachGridControlRumControl("Permisos Cambio Estado ", 30100, "PermisosCambioEstado");
                        break;

                    case "motivosComentarioNode":

                        AttachGridControlRumControl("Motivos Comentario", 30101, "MotivosComentario");
                        break;

                    case "carroMovilCabNode":

                        AttachGridControlRumControl("Carro Movil Cabezera", 30035, "carroMovilCab");
                        break;

                    case "clasificacionEntradaNode":

                        AttachGridControlRumControl("Clasificacion Entradas", 30401, "ClasificacionEntradas");
                        break;

                    case "CarroMovilLinNode":

                        AttachGridControlRumControl("Carro Movil Lineas", 30404, "carroMovilLin");
                        break;

                    case "SubsistemasNode":

                        AttachGridControlRumControl("Subsistemas", 30402, "Subsistemas");
                        break;

                    case "TipoSoporteNode":

                        AttachGridControlRumControl("Tipos Soporte", 30403, "TiposSoporte");
                        break;

                    case "informesNode":

                        FrmVisorInformes frm = new FrmVisorInformes();
                        frm.Show();
                        break;

                    default:
                        //tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                        return;
                }
                ctrlTwr.pararRelojCtrlTwr();

                this.Text = e.Node.Text;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            AñadirLabelCantidad();//TODO revisar
        }

        private void addNodeTorreControlTreeMenu()
        {
            if (!Persistencia.TorreControl) return;
            RadTreeNode nodeTorreControl = new RadTreeNode(Lenguaje.traduce("Torre Control"));
            nodeTorreControl.Name = "TorreControl";
            nodeTorreControl.Font = new Font(this.rtvArbolMenu.Font.FontFamily, 12, FontStyle.Bold);
            this.rtvArbolMenu.Nodes.Add(nodeTorreControl);
        }

        private void createMenuNodes(RadTreeView menuNode)
        {
            Dictionary<string, List<string>> menus = menu.getMenus();
            foreach (var item in menus)
            {
                if (!menuNode.Nodes.Contains(item.Value[0]) && item.Value[4] != null && Boolean.Parse(item.Value[4]) == true)
                {
                    RadTreeNode Node = new RadTreeNode(item.Value[0]);
                    Node.Expanded = bool.Parse(item.Value[1]);
                    Node.Text = Lenguaje.traduce(item.Value[2]);
                    var cvt = new FontConverter();
                    Font font = cvt.ConvertFromString(item.Value[3]) as Font;
                    Node.Font = font;
                    if (item.Value.Count >= 5)
                    {
                        Node.Visible = Boolean.Parse(item.Value[4]);
                    }

                    menuNode.Nodes.Add(Node);
                }
            }
        }

        private void createNodes(RadTreeNode menusInternNode, Dictionary<string, List<string>> menuFather)
        {
            if (menuFather != null)
            {
                foreach (var item in menuFather)
                {
                    if (!menusInternNode.Nodes.Contains(item.Value[0]) && item.Value[4] != null && Boolean.Parse(item.Value[4]) == true)
                    {
                        RadTreeNode Node = new RadTreeNode(item.Value[0]);
                        Node.Expanded = bool.Parse(item.Value[1]);
                        Node.Text = Lenguaje.traduce(item.Value[2]);
                        var cvt = new FontConverter();
                        Font font = cvt.ConvertFromString(item.Value[3]) as Font;
                        Node.Font = font;
                        if (item.Value.Count >= 5)
                        {
                            Node.Visible = Boolean.Parse(item.Value[4]);
                        }

                        if (item.Value.Count >= 6)
                        {
                            Node.Tag = item.Value[5];
                        }

                        menusInternNode.Nodes.Add(Node);
                    }
                }
            }
        }

        private void RadTreeView1_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            foreach (RadTreeNode item in rtvArbolMenu.TreeViewElement.GetNodes())
            {
                if (item.GetNodeCount(true) > 0)
                {
                    if (!item.Expanded)
                        item.Image = Resources.normal;
                    else
                        item.Image = Resources.expanded;
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

            foreach (RadTreeNode item in rtvArbolMenu.TreeViewElement.GetNodes())
            {
                if (item != e.Node && item.Expanded && e.Node.Parent == null)
                {
                    item.Collapse();
                    item.Image = Resources.normal;
                }
            }
        }

        private void RadCollapsiblePanel1_Expanded(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnStyles[0].Width = 15F;
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
        }

        private void RadCollapsiblePanel1_Collapsed(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnStyles[0].Width = 2F;
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
        }

        private void radTreeView1_SelectedNodeChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            cargarGridView(e);
        }

        #region Control de Tareas

        private void pedidos_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(1, 0) is ControlTareas)
            {
                ControlTareas control = (ControlTareas)tableLayoutPanel1.GetControlFromPosition(1, 0);
                control.PedidosButton_Click(sender, e);
            }
        }

        private void operarios_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(1, 0) is ControlTareas)
            {
                ControlTareas control = (ControlTareas)tableLayoutPanel1.GetControlFromPosition(1, 0);
                control.OperarioButton_Click(sender, e);
            }
        }

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(1, 0) is ControlTareas)
            {
                ControlTareas control = (ControlTareas)tableLayoutPanel1.GetControlFromPosition(1, 0);
                control.PrincipalButton_Click(sender, e);
            }
        }

        #endregion Control de Tareas

        private void RadCollapsiblePanel1_Expanded_1(object sender, EventArgs e)
        {
        }

        private void AñadirLabelCantidad()
        {
            lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            grupoLabel.Name = "EtiqCant";
            grupoLabel.Items.Add(lblCantidad);
            this.mantenimientosTab.Items.AddRange(grupoLabel);
        }

        private void RecogerCantidad(object sender, EventArgs e)
        {
            BaseGridControl ctrl = tableLayoutPanel1.GetControlFromPosition(1, 0) as BaseGridControl;
            this.lblCantidad.Text = ctrl.lblCantidad.Text;
        }

        private void DeshabilitarControlesPermisos(int idFormulario)
        {
            if (!User.Perm.tienePermisoEscritura(idFormulario))
            {
                foreach (RibbonTab item in radRibbonBar1.CommandTabs)
                {
                    foreach (RadRibbonBarGroup grupo in item.Items)
                    {
                        if (grupo.Name != "Vista" && grupo.Name != "Conf" && grupo.Name != "EtiqCant")
                        {
                            grupo.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (RibbonTab item in radRibbonBar1.CommandTabs)
                {
                    foreach (RadRibbonBarGroup grupo in item.Items)
                    {
                        if (grupo.Name != "Vista" && grupo.Name != "Conf" && grupo.Name != "EtiqCant")
                        {
                            grupo.Enabled = true;
                        }
                    }
                }
            }
        }

        private void Form_Closing(object sender, CancelEventArgs e)
        {
            // Application.Exit();
        }

        private void inicializarComboTamañofuente()
        {
            for (int i = 8; i < 21; i++)
            {
                menuItemTamLetra.Items.Add(i.ToString());
            }
            menuItemTamLetra.ComboBoxElement.SelectedIndexChanged += comboTamañoFuente_Changed;
        }

        private void comboTamañoFuente_Changed(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.GetControlFromPosition(1, 0) is BaseGridControl)
                {
                    BaseGridControl temp = tableLayoutPanel1.GetControlFromPosition(1, 0) as BaseGridControl;
                    float tamaño = float.Parse(menuItemTamLetra.ComboBoxElement.SelectedItem.Text);
                    temp.virtualGridControl.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    temp.GridView.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void InicializarComboPaginacion()
        {
            if (XmlReaderPropio.getPaginacion() <= 10)
            {
                for (int i = 20; i < 60; i++)
                {
                    menuComboItem.Items.Add(i.ToString());
                }
            }
            else
            {
                for (int i = XmlReaderPropio.getPaginacion(); i < 60; i++)
                {
                    menuComboItem.Items.Add(i.ToString());
                }
            }
            menuComboItem.ComboBoxElement.SelectedIndexChanged += comboPaginacion_Changed;
        }

        private void comboPaginacion_Changed(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.GetControlFromPosition(1, 0) is BaseGridControl)
                {
                    BaseGridControl temp = tableLayoutPanel1.GetControlFromPosition(1, 0) as BaseGridControl;
                    int tamaño = int.Parse(menuComboItem.ComboBoxElement.SelectedItem.Text);
                    temp.virtualGridControl.PageSize = tamaño;
                    temp.GridView.PageSize = tamaño;
                    XmlReaderPropio.setPaginacion(tamaño);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void CrearXMLProgressBar()
        {
            string path = string.Empty;
            string pathGlobal = string.Empty;

            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path = Persistencia.DirectorioBase + @"\RumboEstilos\Español\XML\";
                //pathGlobal = @"\\172.20.8.136\p\RumboEstilos\Español\XML\";
            }
            else
            {
                path = Persistencia.DirectorioBase + @"\RumboEstilos\Ingles\XML\";
                //pathGlobal = @"\\172.20.8.136\p\RumboEstilos\Ingles\XML\";
            }
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (DirectoryNotFoundException e)
                {
                    log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                    MessageBox.Show(strings.NoEncuentroRuta + ":" + path + "\n" + strings.CambiarPath);
                }
            }
            //if (!Directory.Exists(pathGlobal))
            //{
            //    try
            //    {
            //        Directory.CreateDirectory(pathGlobal);
            //    }
            //    catch (Exception e)
            //    {
            //        log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            //        MessageBox.Show(strings.NoEncuentroRuta + ":" + pathGlobal + "\n" + strings.CambiarPath);
            //    }
            //}
            /*QUITADO MONICA NO ENTIENDO PARA QUE SIRVE EN EL FORMULARIO PRINCIPALif (!File.Exists(path + @"\ColumnasProgress.xml"))
            {
                using (XmlWriter writer = XmlWriter.Create(path + @"\ColumnasProgress.xml"))
                {
                    writer.WriteStartElement("Personalizacion");
                    writer.WriteElementString("IndiceColumnaLineasReservadas", "0");
                    writer.WriteElementString("IndiceColumnaLineasPreparadas", "0");
                    writer.WriteElementString("IndiceColumnaPorcentLineas", "0");
                    writer.WriteElementString("IndiceColumnaLineasPorcentEntrada", "0");
                    writer.WriteElementString("IndiceColumnaLineaPorcentCantidadPteUbicar", "0");
                    writer.WriteElementString("IndiceColumnaCantidadReservada", "0");
                    writer.WriteElementString("IndiceColumnaCantidadServida", "0");
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }*/
            //if (!File.Exists(pathGlobal + @"\ColumnasProgress.xml"))
            //{
            //    using (XmlWriter writer = XmlWriter.Create(pathGlobal + @"\ColumnasProgress.xml"))
            //    {
            //        writer.WriteStartElement("Personalizacion");
            //        writer.WriteElementString("IndiceColumnaLineasReservadas", "0");
            //        writer.WriteElementString("IndiceColumnaLineasPreparadas", "0");
            //        writer.WriteElementString("IndiceColumnaPorcentLineas", "0");
            //        writer.WriteElementString("IndiceColumnaLineasPorcentEntrada", "0");
            //        writer.WriteElementString("IndiceColumnaLineaPorcentCantidadPteUbicar", "0");
            //        writer.WriteElementString("IndiceColumnaCantidadReservada", "0");
            //        writer.WriteElementString("IndiceColumnaCantidadServida", "0");
            //        writer.WriteEndElement();
            //        writer.Flush();
            //    }
            //}
        }

        #region Botones Eventos

        private void Nuevo_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.newButton_Click(sender, e);
        }

        private void Borrar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.deleteButton_Click(sender, e);
        }

        private void Clonar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.cloneButton_Click(sender, e);
        }

        private void CambiarVista_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.btnCambiarVista_Click(sender, e);
            if (a.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                btnCambiarVista.Text = strings.CambiarVistaGridView;
                btnExportar.Enabled = true;
                menuItemColumnas.Enabled = true;
            }
            else
            {
                btnCambiarVista.Text = strings.CambiarVistaVirtual;
                btnExportar.Enabled = false;
                menuItemColumnas.Enabled = false;
            }
        }

        private void QuitarFiltros_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.btnQuitarFiltros_Click(sender, e);
        }

        private void FiltroInicial_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(BaseGridControl.nombreJson))
            {
                log.Error("Filtro Inicial:  No hay json seleccionado");
                return;
            }

            FiltroInicialForm fi = new FiltroInicialForm(BaseGridControl.nombreJson, Modos.BaseGridControl);
            fi.ShowDialog();
        }

        private void BarraBusqueda_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            //a.OcultarBarraBusqueda(sender, e);
        }

        private void Refrescar_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(1, 0) is BaseGridControl)
            {
                BaseGridControl a = tableLayoutPanel1.GetControlFromPosition(1, 0) as BaseGridControl;
                if (a is RumControlGeneral)
                {
                    //El RumControl recarga los datos desde llenarGrid
                    //idependientemente de si es virtual o grid.
                    a.llenarGrid();
                }
                else if (a.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    a.llenarGrid();
                    a.ElegirEstilo();
                }
                else
                {
                    a.RefreshData(0);
                    a.ElegirEstilo();
                }
            }
        }

        private void Exportar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.btnExportacion_Click(sender, e);
        }

        private void menuItemGuardar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.SaveButton_Click(sender, e);
        }

        private void menuIncidenciaJira_Click(object sender, EventArgs e)
        {
            CrearIncidencia incidencia = new CrearIncidencia();
            incidencia.ShowDialog();
        }

        private void menuManual_Click(object sender, EventArgs e)
        {
            DataAccess.abrirManual(MantenimientoActivo);
        }

        private void menuItemCargar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.LoadButton_Click(sender, e);
        }

        private void menuItemColumnas_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(1, 0);
            a.btnColumnas_Click(sender, e);
        }

        #endregion Botones Eventos

        #endregion Eventos

        #region Funciones Auxiliares

        public void AttachGridControl<T>(ref BaseGridControl ctrl, string name, int id) where T : BaseGridControl, new()
        {
            this.SuspendLayout();

            if (ctrl == null)
            {
                ctrl = new T();
                ctrl.Dock = DockStyle.Fill;
                ctrl.Margin = new Padding(0, 0, 7, 7);
                ctrl.NombrarFormulario(ctrl.name);
                ctrl.name = name;
                ctrl.Name = name;
                ctrl.idFormulario = id;
                ctrl.ElegirEstilo();
            }
            if (radRibbonBar1.CommandTabs.Count() == 0)
            {
                radRibbonBar1.CommandTabs.Add(mantenimientosTab);
            }
            ComprobarEliminarButtonGroupTab(radRibbonBar1, Lenguaje.traduce("Operario"));
            radCollapsiblePanel1.Expand();
            radRibbonBar1.Expanded = true;
            radRibbonBar1.ExpandButton.Enabled = true;
            mantenimientosTab.Enabled = true;
            tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
            tableLayoutPanel1.Controls.Add(ctrl, 1, 0);

            BaseGridControl baseControl = tableLayoutPanel1.GetControlFromPosition(1, 0) as BaseGridControl;
            lblCantidad.Text = baseControl.lblCantidad.Text;
            baseControl.lblCantidad.TextChanged += RecogerCantidad;
            mantenimientosTab.IsSelected = true;
            if (baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                btnCambiarVista.Text = "Vista Rápida";
            }
            else
            {
                btnCambiarVista.Text = "Vista Avanzada";
            }
            DeshabilitarControlesPermisos(ctrl.idFormulario);
            this.ResumeLayout();

            //Eliminar layout button
            if (this.btnConf.Items["RadMenuItemEliminarLayout"] != null)
            {
                int indexBoton = this.btnConf.Items.IndexOf(this.btnConf.Items["RadMenuItemEliminarLayout"]);
                if (indexBoton != -1) this.btnConf.Items.RemoveAt(indexBoton);
            }
            if (baseControl != null && (baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView ||
                baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid))
            {
                FuncionesGenerales.AddEliminarLayoutButton(ref btnConf);
                if (this.btnConf.Items["RadMenuItemEliminarLayout"] != null)
                {
                    this.btnConf.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                        {
                            FuncionesGenerales.EliminarLayout(baseControl.Name + "GridView" + ConexionSQL.getNombreConexion() + ".xml", null);
                            baseControl.GridView.Refresh();
                        }
                        else
                        {
                            FuncionesGenerales.EliminarLayout(baseControl.Name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml", null);
                            baseControl.virtualGrid.Refresh();
                        }
                    });
                }
            }
        }

        public void AttachGridControlRumControl(string name, int id, String nombreJson)
        {
            this.SuspendLayout();

            if (radRibbonBar1.CommandTabs.Count() == 0)
            {
                radRibbonBar1.CommandTabs.Add(mantenimientosTab);
            }

            MantenimientoActivo = id;
            radCollapsiblePanel1.Expand();
            radRibbonBar1.Expanded = true;
            radRibbonBar1.ExpandButton.Enabled = true;
            mantenimientosTab.Enabled = true;
            if (nombreJson.Equals("Algoritmos"))
            {
                ComprobarEliminarButtonGroupTab(radRibbonBar1, Lenguaje.traduce("Algoritmos"));
                baseGridControl = new RumControlAlgoritmo(nombreJson);
            }
            else if (nombreJson.Equals("Operarios"))
            {
                ComprobarEliminarButtonGroupTab(radRibbonBar1, Lenguaje.traduce("Operario"));
                baseGridControl = new RumControlOperarios(nombreJson, radRibbonBar1);
            }
            /*else if (nombreJson.Equals("IncidenciasInventario"))
            {
                ComprobarEliminarButtonGroupTab(radRibbonBar1, "options");
                //baseGridControl = new RumControlInventario(nombreJson, radRibbonBar1);
            }*/
            else
            {
                ComprobarEliminarButtonGroupTab(radRibbonBar1, Lenguaje.traduce("Operario"));
                baseGridControl = new RumControlGeneral(nombreJson);
            }
            baseGridControl.Dock = DockStyle.Fill;
            baseGridControl.Margin = new Padding(0, 0, 7, 7);
            baseGridControl.NombrarFormulario(baseGridControl.name);
            baseGridControl.name = name;
            baseGridControl.Name = name;
            baseGridControl.idFormulario = id;
            baseGridControl.ElegirEstilo();
            tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
            tableLayoutPanel1.Controls.Add(baseGridControl, 1, 0);
            BaseGridControl baseControl = tableLayoutPanel1.GetControlFromPosition(1, 0) as BaseGridControl;
            lblCantidad.Text = baseControl.lblCantidad.Text;
            baseControl.lblCantidad.TextChanged += RecogerCantidad;
            mantenimientosTab.IsSelected = true;

            if (baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                btnCambiarVista.Text = "Vista Rápida";
            }
            else
            {
                btnCambiarVista.Text = "Vista Avanzada";
            }
            DeshabilitarControlesPermisos(baseGridControl.idFormulario);
            this.ResumeLayout();

            //Eliminar layout button
            if (this.btnConf.Items["RadMenuItemEliminarLayout"] != null)
            {
                int indexBoton = this.btnConf.Items.IndexOf(this.btnConf.Items["RadMenuItemEliminarLayout"]);
                if (indexBoton != -1) this.btnConf.Items.RemoveAt(indexBoton);
            }
            if (baseControl != null && (baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView ||
                baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid))
            {
                FuncionesGenerales.AddEliminarLayoutButton(ref btnConf);
                if (this.btnConf.Items["RadMenuItemEliminarLayout"] != null)
                {
                    this.btnConf.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                        {
                            FuncionesGenerales.EliminarLayout(baseControl.Name + "GridView" + ConexionSQL.getNombreConexion() + ".xml", null);
                            baseControl.GridView.Refresh();
                        }
                        else
                        {
                            FuncionesGenerales.EliminarLayout(baseControl.Name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml", null);
                            baseControl.virtualGrid.Refresh();
                        }
                    });
                }
            }
        }

        private void cerrarMantenimientos()
        {
            try
            {
                this.radCollapsiblePanel1.Show();
                radRibbonBar1.Expanded = false;
                radRibbonBar1.ExpandButton.Enabled = false;
                mantenimientosTab.Enabled = true;
                radRibbonBar1.CommandTabs.Clear();

                tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                if (tcuadroControl == null)
                {
                    tcuadroControl = new DBTableLayoutPanel();
                }
                tableLayoutPanel1.Controls.Add(tcuadroControl, 1, 0);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        public void LanzarDevolucionesProveedor<T>(FrmDevolucionesProveedor ctrl, string name, int id) where T : FrmDevolucionesProveedor, new()
        {
            //Pantalla de Devoluciones de proveedor en el menú principal
            ctrl = new FrmDevolucionesProveedor();
            //ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarOrdenesRecogida<T>(OrdenesRecogida ctrl, string name) where T : OrdenesRecogida, new()
        {
            //Pantalla de Expediciones en el menú principal
            ctrl = new OrdenesRecogida();
            //ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarDevolucionesCliente<T>(FrmDevolucionesCliente ctrl, string name) where T : FrmDevolucionesCliente, new()
        {
            //Pantalla de Expediciones en el menú principal
            ctrl = new FrmDevolucionesCliente();
            //ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarMapaAlmacen<T>(FrmMapaAlmacen ctrl, string name) where T : FrmMapaAlmacen, new()
        {
            //Pantalla mapa Almacen en el menú principal
            ctrl = new FrmMapaAlmacen();
            ctrl.Show();   //2 segundos
            this.Enabled = true;
        }

        public void LanzarControlTareas<T>(ControlTareas ctrl, string name, int id) where T : ControlTareas, new()
        {
            ctrl = new ControlTareas();
            ctrl.idFormulario = id;
            ctrl.Show();
            ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarProductividad<T>(FrmProductividad ctrl, string name, int id) where T : FrmProductividad, new()
        {
            ctrl = new FrmProductividad();
            ctrl.idFormulario = id;
            ctrl.Show();
            ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarArticulosGridView<A>(ArticulosFrm ctrl, string name, int id) where A : ArticulosFrm, new()
        {
            ctrl = new ArticulosFrm();
            ctrl.idFormulario = id;
            ctrl.Show();
            ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarOperariosGridView<O>(OperariosForm ctrl, string name, int id) where O : OperariosForm, new()
        {
            ctrl = new OperariosForm();
            ctrl.idFormulario = id;
            ctrl.Show();
            ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarRecursosTareaGridView<O>(RecursosTareaForm ctrl, string name, int id) where O : RecursosTareaForm, new()
        {
            ctrl = new RecursosTareaForm();
            ctrl.idFormulario = id;
            ctrl.Show();
            ctrl.TopMost = true;
            this.Enabled = true;
        }

        public void LanzarStock<T>(Stock ctrl, string name, int id) where T : Stock, new()
        {
            ctrl = new Stock();
            ctrl.idFormulario = id;
            ctrl.Location = this.Location;
            ctrl.TopMost = true;

            this.Enabled = true;
        }

        public void LanzarParam<T>(CambiarAtributos ctrl) where T : CambiarAtributos, new()
        {
            ctrl = new CambiarAtributos();
            if (ctrl == null)
            {
                ctrl = new T();
                ctrl.Dock = DockStyle.Fill;
                ctrl.Margin = new Padding(0, 0, 7, 7);
            }
            ctrl.Show();
            ctrl.TopMost = true;

            this.Enabled = true;
        }

        public void LanzarTrazabilidad<T>(Trazabilidad ctrl, string name) where T : Trazabilidad, new()
        {
            if (ctrl == null)
            {
                ctrl = new T();
                ctrl.Dock = DockStyle.Fill;
                ctrl.Margin = new Padding(0, 0, 7, 7);
                ctrl.name = name;
            }

            ctrl.Show();
        }

        public void LanzarMovimientos<M>(Movimientos ctrl, string name) where M : Movimientos, new()
        {
            if (ctrl == null)
            {
                ctrl = new M();

                ctrl.Dock = DockStyle.Fill;
                ctrl.Margin = new Padding(0, 0, 7, 7);
                ctrl.name = name;
            }
            ctrl.Show();
        }

        public void LanzarReposiciones<T>(Reposiciones ctrl, string name) where T : Reposiciones, new()
        {
            if (ctrl == null)
            {
                ctrl = new T();
                ctrl.Dock = DockStyle.Fill;
                ctrl.Margin = new Padding(0, 0, 7, 7);
                ctrl.Name = name;
            }
            ctrl.Show();
        }

        public void LanzarPedidoProveedor<T>(ProveedoresPedidosCabGridView ctrl, string name, int id) where T : ProveedoresPedidosCabGridView, new()
        {
            log.Info("Pulsado botón menu Recepciones " + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
            ctrl = new ProveedoresPedidosCabGridView();
            ctrl.idFormulario = id;
            ctrl.Show();
            ctrl.TopMost = true;
            ctrl.Name = name;

            this.Enabled = true;
            log.Info("Fin botón Menu Recepciones " + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
        }

        public void LanzarAcopiosProduccion<T>(AcopiosProduccion ctrl, string name) where T : AcopiosProduccion, new()
        {
            ctrl = new AcopiosProduccion();
            ctrl.Show();
            ctrl.TopMost = true;

            this.Enabled = true;
        }

        public void LanzarArticulosForm<A>(ArticulosFrm ctrl, string name) where A : ArticulosFrm, new()
        {
            ctrl = new ArticulosFrm();
            ctrl.Show();

            this.Enabled = true;
        }

        //Tomás función que elimina un boton
        public void ComprobarEliminarButtonGroupTab(RadRibbonBar bar, string nombreTab)
        {
            try
            {
                for (int i = 0; i < ((RibbonTab)bar.CommandTabs[0]).Items.Count; i++)
                {
                    if (((RibbonTab)bar.CommandTabs[0]).Items[i].Text == nombreTab)
                    {
                        RadRibbonBarGroup groupToRemove = ((RadRibbonBarGroup)(((RibbonTab)bar.CommandTabs[0]).Items[i]));
                        ((RibbonTab)bar.CommandTabs[0]).Items.Remove(groupToRemove);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        protected void OnThemeChanged()
        {
            Theme theme = ThemeResolutionService.GetTheme(ThemeResolutionService.ApplicationThemeName);

            foreach (StyleGroup styleGroup in theme.StyleGroups)
            {
                foreach (PropertySettingGroup propertySettingGroup in styleGroup.PropertySettingGroups)
                {
                    if (propertySettingGroup.Selector.Value == "RadFormElement")
                    {
                        foreach (PropertySetting propertySetting in propertySettingGroup.PropertySettings)
                        {
                            if (propertySetting.Name == "BackColor")
                            {
                                this.BackColor = (Color)propertySetting.Value;
                                return;
                            }
                        }
                    }
                    if (styleGroup.Registrations[0].ControlType == "Telerik.WinControls.UI.RadForm" && propertySettingGroup.Selector.Value == "Telerik.WinControls.RootRadElement")
                    {
                        foreach (PropertySetting propertySetting in propertySettingGroup.PropertySettings)
                        {
                            if (propertySetting.Name == "BackColor")
                            {
                                this.BackColor = (Color)propertySetting.Value;
                                return;
                            }
                        }
                    }
                }
            }
        }

        #endregion Funciones Auxiliares

        #region Temas

        private void InitializeThemesDropDown()
        {
            AddThemeItemToThemesDropDownList("Fluent", Resources.fluent);
            AddThemeItemToThemesDropDownList("FluentDark", Resources.fluent_dark);
            AddThemeItemToThemesDropDownList("Material", Resources.material);
            AddThemeItemToThemesDropDownList("MaterialPink", Resources.material_pink);
            AddThemeItemToThemesDropDownList("MaterialTeal", Resources.material_teal);
            AddThemeItemToThemesDropDownList("MaterialBlueGrey", Resources.material_blue_grey);
            AddThemeItemToThemesDropDownList("ControlDefault", Resources.control_default);
            AddThemeItemToThemesDropDownList("TelerikMetro", Resources.telerik_metro);
            AddThemeItemToThemesDropDownList("Windows8", Resources.windows8);

            loadedThemes.Add("ControlDefault", true);
            loadedThemes.Add("TelerikMetro", true);

            var assemblyName = "Telerik.WinControls.Themes.Windows8.dll";
            var strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), assemblyName);
            Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
            Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + "Windows8" + "Theme"));

            loadedThemes.Add("Windows8", true);
            ThemeResolutionService.ApplicationThemeName = "Windows8"; //Tema por Defecto de la Aplicación
        }

        private void temasItem_Click(object sender, EventArgs e)
        {
            var assemblyName = "Telerik.WinControls.Themes." + (sender as RadMenuItem).Text + ".dll";
            var themeName = (sender as RadMenuItem).Text;
            var strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), assemblyName);
            if (!System.IO.File.Exists(strTempAssmbPath)) // we are in the case of QSF as exe, so the Path is different
            {
                strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\Bin40");
                strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);

                if (!System.IO.File.Exists(strTempAssmbPath))
                {
                    strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\bin\\ReleaseTrial");
                    strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);
                }
            }

            if (!loadedThemes.ContainsKey(themeName))
            {
                Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
                Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + themeName + "Theme"));

                loadedThemes.Add(themeName, true);
            }

            ThemeResolutionService.ApplicationThemeName = themeName;
            if (ControlTraceMonitor.AnalyticsMonitor != null)
            {
                ControlTraceMonitor.AnalyticsMonitor.TrackAtomicFeature("ThemeChanged." + themeName);
            }
        }

        private void AddThemeItemToThemesDropDownList(string themeName, Image image)
        {
            RadMenuItem mainItem = menuItemTemas/*.Items[0] as RadMenuItem*/;
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(temasItem_Click);
            mainItem.Items.Add(temasItem);
        }

        #endregion Temas

        private void SFPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                {
                    var culture = new System.Globalization.CultureInfo("es-ES"); //Región España
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
                else
                {
                    var culture = new System.Globalization.CultureInfo("en-Uk"); //Región Inglesa
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }

                string traduccion = Lenguaje.traduce("Estas seguro que quieres salir del programa");

                //  DialogResult confirm = MessageBox.Show(traduccion, "", MessageBoxButtons.YesNo);
                SFCierre cierre = new SFCierre();
                DialogResult confirm = cierre.ShowDialog();

                if (confirm != DialogResult.Yes)
                {
                    e.Cancel = true;
                }

                log.Info("Cerrar programa ");
            }
        }

        private void SFPrincipal_Deactivate(object sender, EventArgs e)
        {
            if (this.Focused == false)
            {
                ctrlTwr.pararRelojCtrlTwr();
                log.Info("Se ha parado el reloj de la torre de control");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.Analytics;
using Telerik.WinControls;
using System.Reflection;
using RumboSGA.Properties;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGAManager.Model.Entities;
using RumboSGAManager;
using Telerik.WinControls.Data;
using System.Collections;
using RumboSGA.Presentation.UserControls.Mantenimientos.GridVirtual;
using System.IO;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Globalization;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using RumboSGA.Presentation.FormularioRecepciones;
using System.Dynamic;
using Newtonsoft.Json;
using RumboSGAManager.Model.Security;
using System.Diagnostics;
using RumboSGA.RecepcionMotor;
using Newtonsoft.Json.Linq;
using RumboSGA.Presentation.Formulario_Recepciones;
using RumboSGA.Presentation.UserControls;
using System.Xml.Linq;
using EstadoRecepciones;
using RumboSGA.Controles;
using Rumbo.Core.Herramientas;
using RumboSGA.EmbalajesMotor;
using RumboSGA.Presentation.Herramientas.Ventanas;
using RumboSGA.Presentation.Herramientas.PantallasWS;
using RumboSGA.GestionAlmacen.Formulario_Recepciones;
using Rumbo.Core.Herramientas.Herramientas;
using Telerik.WinControls.Enumerations;

namespace RumboSGA.Presentation.FormularioRecepciones
{
    public partial class ProveedoresPedidosCabGridView : Telerik.WinControls.UI.RadRibbonForm
    {
        #region Variables, Constantes y Propiedades

        public int idFormulario;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string nombreEstiloRecepciones = "VistaRecepcionesGridView.xml";
        private static string nombreEstiloPedidos = "VistaPedidosGridView.xml";

        private RumGridView rgvRecepciones = new RumGridView();
        private RadRibbonBarGroup grupoPedidos = new RadRibbonBarGroup();
        private RumRibbonBarGroup grupoRecepciones = new RumRibbonBarGroup();
        private RumLabelElement lblNumRegistros = new RumLabelElement();
        private RadWaitingBar waitingBar = new RadWaitingBar();
        private GridViewCheckBoxColumn checkBoxColumnJerarquicoLineas;
        private GridViewCheckBoxColumn checkBoxColumnJerarquicoPreavisos;
        private RadDataFilterDialog dialog = new RadDataFilterDialog();

        private static int jerarquicoPosicionLineasRec = 0;
        private static int jerarquicoPosicionEntradasRec = 1;
        private static int jerarquicoPosicionStockRec = 2;
        private static int jerarquicoPosicionMovimientosRec = 3;
        private static int jerarquicoPosicionTareasRec = 4;
        private static int jerarquicoPosicionAsnsRec = 5;
        private static int jerarquicoPosicionEmbalajeRec = 6;

        private RumButtonElement rBtnCrearRecepcion = new RumButtonElement();
        private RumButtonElement rBtncerrarRecepcion = new RumButtonElement();
        private RumButtonElement rBtnEntradaDirectaPreaviso = new RumButtonElement();
        private RumButtonElement rBtnEntradaDirectaPreavisoCarro = new RumButtonElement();
        private RumButtonElement rBtnModificarEmbalaje = new RumButtonElement();
        private RumButtonElement rBtnEliminarEmbalaje = new RumButtonElement();
        private RumButtonElement rBtnAltaPico = new RumButtonElement();
        private RumButtonElement rBtnAltaPalet = new RumButtonElement();
        private RumButtonElement rBtnAltaCarro = new RumButtonElement();
        private RumButtonElement rBtnAltaMulti = new RumButtonElement();
        private RumButtonElement rBtnAltaTotales = new RumButtonElement();
        private RumButtonElement rBtnEditarRecepcion = new RumButtonElement();
        private RumButtonElement rBtnCerrarPedido = new RumButtonElement();
        private RumButtonElement rBtnRecepcionParcial = new RumButtonElement();
        private RumButtonElement rBtnEliminarEntrada = new RumButtonElement();
        private RumButtonElement rBtnClasificacionEntradas = new RumButtonElement();

        private GridViewTemplate gtJerarquicoPedidosLineasPedidos = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoPedidosEntradas = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoPedidosPreavisos = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoPedidosRecepciones = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoRecepcionesLineas = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoRecepcionesMovimientos = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoRecepcionesEntradas = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoRecepcionesPreavisos = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoRecepcionesTareas = new GridViewTemplate();
        private GridViewTemplate gtJerarquicoRecepcionesEmbalajes = new GridViewTemplate();

        private ProgressBarColumn colJerLineasPedidoPorcentEntPend;
        private ProgressBarColumn colJerLineasPedidosPorcentEnt;
        private ProgressBarColumn colPedidosPorcentLineas;

        private BackgroundWorker bgwWorkerPedidos;// = new BackgroundWorker();

        private List<int> pedidosSeleccionados = new List<int>();
        private List<GridViewRowInfo> pedidosFilaSeleccionados = new List<GridViewRowInfo>();
        private List<int> recepcionesSeleccionadas = new List<int>();
        private int embalajeSeleccionado = 0;
        private int idPedidoProSeleccionado = 0;
        private int idPedidoProLinSeleccionado = 0;
        protected List<TableScheme> _lstEsquemaTablaProveedores = new List<TableScheme>();
        public List<TableScheme> _lstEsquemaTablaRecepciones = new List<TableScheme>();
        public List<TableScheme> _lstEsquemaTablaRecepcionesUnion = new List<TableScheme>();
        public List<TableScheme> _lstEsquemaTablaPedidosUnion = new List<TableScheme>();
        public List<TableScheme> _lstEsquemaTablaLineasPedido = new List<TableScheme>();
        public List<TableScheme> _lstEsquemaTablaEntradasPedido = new List<TableScheme>();
        public List<TableScheme> _lstEsquemaTablaPreavisos = new List<TableScheme>();

        //MONICA no se utiliza private BindingList<dynamic> data = new BindingList<dynamic>();
        //MONICA no se utiliza public Hashtable diccParamNuevo = new Hashtable();
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        public Action<BindingList<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private DataTable dtPedidos = new DataTable();
        //MONICA no se utilizaDataTable tablaJerarquiaRecepcionesDePedido = new DataTable();
        //MONICA no se utilizaDataTable tablaJerarquiaLineasPedido = new DataTable();
        //MONICA no se utilizaDataTable tablaJerarquiaEntradas = new DataTable();
        //MONICA no se utilizaDataTable tablaPreavisos = new DataTable();
        //DataTable dtPedidosLineas;
        //DataTable dtColPaletsCompletos;
        //DataTable dtColUSDSueltas;
        //DataTable dtPorcentLineas;
        //MONICA no se utiliza DataTable countLineas;

        private int _cantidadRegistros = 0;
        //MONICA no se utilizapublic int numRegistrosFiltrados;
        //MONICA no se utilizaprotected dynamic selectedRow;
        //MONICA no se utilizapublic const int _K_PAGINACION = 20;
        //MONICA no se utilizaprotected GridScheme _esquemGrid;
        //MONICA no se utilizapublic string FiltroExterno;
        //MONICA no se utiliza public string name;

        //MONICA no se utilizaprivate const int ROWCOUNT = 2;
        //MONICA no se utilizaprivate const int COLUMNCOUNT = 16;

        private string colID;
        private string colCantidadPteUbicar;
        private string colCantidadEntrada;
        private string colPorcentLineas;

        //private string colPaletsCompletos;
        //private string colUnidadesSueltas;
        private string colRecepciones;

        private Timer timerRefrescarPedidos;
        private Timer timerRefrescarRecepciones;

        //private string colLineas;
        //private string colNRecepciones;
        private RumDropDownButtonElement rDDBtnOpcionesRecepciones = new RumDropDownButtonElement();

        private RumMenuItem agregarEmbalajeMenuItem = new RumMenuItem();
        private RumMenuItem modificarEmbalajeMenuItem = new RumMenuItem();
        private RumMenuItem eliminarEmbalajeMenuItem = new RumMenuItem();
        private RumMenuItem modificarEntradaMenuItem = new RumMenuItem();
        private RumMenuItem eliminarEntradaMenuItem = new RumMenuItem();
        private RumMenuItem modificarLineasMenuItem = new RumMenuItem();
        private RumMenuItem AltaChepsMenuItem = new RumMenuItem();

        #endregion Variables, Constantes y Propiedades

        #region propiedades

        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        //public RadGridView radGridViewControl
        //{
        //    get { return this.rgvPedidos; }
        //}

        #endregion propiedades

        #region Constructor

        public ProveedoresPedidosCabGridView()
        {
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio Pantalla ProveedoresPedidosCabGridView ");
            InitializeComponent();

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            ////MONICA no se utilizathis.Name = Lenguaje.traduce("Recepciones");
            ///
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio Temas ");
            InitializeThemesDropDown();
            this.Shown += form_Shown;
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio traducciones ");
            InicializaTraducciones();
            this.Text = colRecepciones;

            gtJerarquicoPedidosEntradas.AllowAddNewRow = false;
            gtJerarquicoPedidosLineasPedidos.AllowAddNewRow = false;
            gtJerarquicoPedidosPreavisos.AllowAddNewRow = false;
            gtJerarquicoPedidosRecepciones.AllowAddNewRow = false;
            gtJerarquicoRecepcionesEmbalajes.AllowAddNewRow = false;
            gtJerarquicoRecepcionesEntradas.AllowAddNewRow = false;
            gtJerarquicoRecepcionesLineas.AllowAddNewRow = false;
            gtJerarquicoRecepcionesMovimientos.AllowAddNewRow = false;
            gtJerarquicoRecepcionesPreavisos.AllowAddNewRow = false;
            gtJerarquicoRecepcionesTareas.AllowAddNewRow = false;

            rrbBarraBotones.Text = colRecepciones;
            rrbBarraBotones.Name = colRecepciones;
            rgvPedidos.Name = "pedidos";
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio barra progreso ");
            ConfigurarBarraProgresoGridView();

            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio crear botones ");
            CrearBotonesPedidos();
            CrearBotonesRecepciones();
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio texto botones ");
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio show ");
            TextoBotones();
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio inicio columnas ");
            InicializarColumnas();

            this.Show();
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + "Fin Pantalla ProveedoresPedidosCabGridView");

            //waitingBar.StartWaiting();
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio inicio anyadir eventos ");
            //AnyadirEventos();
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio inicio preferencias ");
            //SetPreferences();
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio inicio esquemas ");
            //InicializarEsquemas();
            //ConfigurarComboFormatoFecha();
            // //Monica dudo si se debe hacer aqui
            //rgvPedidos.BeginUpdate();//Monica dudo si se debe hacer aqui
            //InicializarWorkerPedidos();
            //bgwWorkerPedidos.RunWorkerAsync();
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + "Fin Pantalla ProveedoresPedidosCabGridView");
            //rBtnEditarRecepcion.Visibility = ElementVisibility.Hidden;
            FuncionesGenerales.AddEliminarLayoutButton(ref rDDBtnConfiguracion);
            if (this.rDDBtnConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.rDDBtnConfiguracion.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                {
                    if (this.tlPrincipal.GetControlFromPosition(0, 0).Name == "pedidos")
                    {
                        FuncionesGenerales.EliminarLayout(this.Name + nombreEstiloPedidos, rgvPedidos);
                    }
                    else
                    {
                        FuncionesGenerales.EliminarLayout(this.Name + nombreEstiloRecepciones, rgvRecepciones);
                    }
                });
            }

            iniciarTimerPedidos();
        }

        private void iniciarTimerPedidos()
        {
            FuncionesGenerales.iniciarTimer(ref timerRefrescarPedidos);
            if (timerRefrescarPedidos != null)
                timerRefrescarPedidos.Tick += TimerRefrescar_Tick;
        }

        private void iniciarTimerRecepciones()
        {
            FuncionesGenerales.iniciarTimer(ref timerRefrescarRecepciones);
            if (timerRefrescarRecepciones != null)
                timerRefrescarRecepciones.Tick += TimerRefrescar_Tick;
        }

        private void TimerRefrescar_Tick(object sender, EventArgs e)
        {
            if (rBtnActualizar == null) return;
            rBtnActualizar.addRefrescoTimerColor();
            rBtnActualizar.enableRefrescoTimerColor();
        }

        private void refrescarTimerPedidos()
        {
            try
            {
                if (this.timerRefrescarPedidos != null)
                {
                    this.timerRefrescarPedidos.Stop();
                    this.timerRefrescarPedidos.Start();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void refrescarTimerRecepciones()
        {
            try
            {
                if (this.timerRefrescarRecepciones != null)
                {
                    this.timerRefrescarRecepciones.Stop();
                    this.timerRefrescarRecepciones.Start();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void ProveedoresPedidosCabGridView_Load(object sender, EventArgs e)
        {
            try
            {
                waitingBar.StartWaiting();
                //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio inicio anyadir eventos ");
                AnyadirEventos();
                //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio inicio preferencias ");
                SetPreferences();
                InicializarEsquemas();
                ConfigurarComboFormatoFecha();
                //Monica dudo si se debe hacer aqui
                //rgvPedidos.BeginUpdate();//Monica dudo si se debe hacer aqui
                InicializarWorkerPedidos();
                bgwWorkerPedidos.RunWorkerAsync();

                rBtnEditarRecepcion.Visibility = ElementVisibility.Hidden;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void InicializaTraducciones()
        {
            colID = Lenguaje.traduce("ID");
            colCantidadPteUbicar = Lenguaje.traduce("Cantidad Pte Ubicar");
            colCantidadEntrada = Lenguaje.traduce("Cantidad Entrada");
            colPorcentLineas = Lenguaje.traduce("% Líneas");
            //colPaletsCompletos = Lenguaje.traduce("Palets Completos");
            //colUnidadesSueltas = Lenguaje.traduce("Unidades Sueltas");
            //colLineas= Lenguaje.traduce("Lineas");
            //colNRecepciones= Lenguaje.traduce("N Recepciones");
            colRecepciones = Lenguaje.traduce("Recepciones");
        }

        private void AnyadirEventos()
        {
            //eventos
            //this.Shown += form_Shown;
            //COMENTO POR TIEMPOS rgvPedidos.ViewRowFormatting += rgvPedidos_ViewRowFormatting;
            rgvPedidos.FilterChanged += rgvPedidos_FilterChanged;
            //rgvPedidos.HeaderCellToggleStateChanged += radGridView1_HeaderCellToggleStateChanged;
            rgvPedidos.RowSourceNeeded += rgvPedidos_RowSourceNeeded;
            rgvPedidos.ValueChanged += rgvPedidos_ValueChanged;
            rgvRecepciones.RowSourceNeeded += rgvRecepciones_RowSourceNeeded;
            rgvRecepciones.ValueChanged += rgvRecepciones_ValueChanged;
            rgvRecepciones.ViewRowFormatting += rgvRecepciones_ViewRowFormatting;
            rgvRecepciones.CellClick += rgvRecepciones_CellClick;
        }

        private void InicializarColumnas()
        {
            checkBoxColumnJerarquicoLineas = new GridViewCheckBoxColumn();
            checkBoxColumnJerarquicoLineas.DataType = typeof(int);
            checkBoxColumnJerarquicoLineas.Name = "chkLineas";
            checkBoxColumnJerarquicoLineas.EnableHeaderCheckBox = true;
            checkBoxColumnJerarquicoLineas.ReadOnly = false;

            checkBoxColumnJerarquicoPreavisos = new GridViewCheckBoxColumn();
            checkBoxColumnJerarquicoPreavisos.DataType = typeof(int);
            checkBoxColumnJerarquicoPreavisos.Name = "chkPreavisos";

            colJerLineasPedidoPorcentEntPend = new ProgressBarColumn(colCantidadPteUbicar);
            colJerLineasPedidoPorcentEntPend.HeaderText = Lenguaje.traduce(colCantidadPteUbicar);
            colJerLineasPedidoPorcentEntPend.Name = Lenguaje.traduce(colCantidadPteUbicar);
            colJerLineasPedidoPorcentEntPend.Width = 100;

            colJerLineasPedidosPorcentEnt = new ProgressBarColumn(colCantidadEntrada);
            colJerLineasPedidosPorcentEnt.HeaderText = Lenguaje.traduce(colCantidadEntrada);
            colJerLineasPedidosPorcentEnt.Name = Lenguaje.traduce(colCantidadEntrada);
            colJerLineasPedidosPorcentEnt.Width = 100;

            colPedidosPorcentLineas = new ProgressBarColumn(colPorcentLineas);
            colPedidosPorcentLineas.HeaderText = colPorcentLineas;
            colPedidosPorcentLineas.Name = colPorcentLineas;
            colPedidosPorcentLineas.Width = 100;
        }

        private void InicializarEsquemas()
        {
            try
            {
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio Carga Esquemas iniciales");
                Business.GetPedidosProJerarquicoEntradasEsquema(ref _lstEsquemaTablaEntradasPedido);
                Business.GetPedidosProJerarquicoLineasEsquema(ref _lstEsquemaTablaLineasPedido);
                Business.GetPedidosProJerarquicoPreavisosEsquema(ref _lstEsquemaTablaPreavisos);
                Business.GetPedidosProveedorJerarquicoRecepcionesEsquemaUnion(ref _lstEsquemaTablaRecepcionesUnion);
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin Carga Esquemas iniciales");
                CreateChildTemplatePedidosProv();
                CreateChildTemplateRecepciones();
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void InicializarWorkerPedidos()
        {
            bgwWorkerPedidos = new BackgroundWorker();
            bgwWorkerPedidos.WorkerReportsProgress = true;
            bgwWorkerPedidos.WorkerSupportsCancellation = true;
            bgwWorkerPedidos.DoWork += bgWorkerPedidos_DoWork;
            bgwWorkerPedidos.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerPedidosCompleted);
        }

        private void bgWorkerPedidos_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio  backgroundWorkerPedidos_DoWork ProveedoresPedidosCabGridView ");
                /*SOLO DEBO CARGARLO UNA VEZBusiness.GetPedidosProJerarquicoEntradasEsquema(ref _lstEsquemaTablaEntradasPedido);
                Business.GetPedidosProJerarquicoLineasEsquema(ref _lstEsquemaTablaLineasPedido);
                Business.GetPedidosProJerarquicoPreavisosEsquema(ref _lstEsquemaTablaPreavisos);
                Business.GetPedidosProveedorJerarquicoRecepcionesEsquemaUnion(ref _lstEsquemaTablaRecepcionesUnion);*/
                CantidadRegistros = Business.GetPedidosProveedorCantidad(ref _lstEsquemaTablaProveedores);
                //Business.GetRecepcionesCantidad(ref _lstEsquemaTablaRecepciones);
                //countLineas = ConexionSQL.getDataTable("Select IDPEDIDOPRO, count (distinct IDPEDIDOPROLIN) Count From tblentradas ent GROUP BY IDPEDIDOPRO");
                dtPedidos = Business.GetPedidosProveedorDatosGridView(_lstEsquemaTablaProveedores);
                //MONICA AL INICIO CreateChildTemplatePedidosProv();

                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + "Fin  backgroundWorkerPedidos_DoWork ProveedoresPedidosCabGridView ");
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        //private void bgWorkerRecepciones_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        Debug.WriteLine("Inicio  backgroundWorkerRecepciones_DoWork ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));
        //        log.Info("Inicio  backgroundWorkerRecepciones_DoWork ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));
        //        /*SOLO DEBO CARGARLO UNA VEZBusiness.GetPedidosProJerarquicoEntradasEsquema(ref _lstEsquemaTablaEntradasPedido);
        //        Business.GetPedidosProJerarquicoLineasEsquema(ref _lstEsquemaTablaLineasPedido);
        //        Business.GetPedidosProJerarquicoPreavisosEsquema(ref _lstEsquemaTablaPreavisos);
        //        Business.GetPedidosProveedorJerarquicoRecepcionesEsquemaUnion(ref _lstEsquemaTablaRecepcionesUnion);*/
        //        CantidadRegistros = Business.GetPedidosProveedorCantidad(ref _lstEsquemaTablaProveedores);
        //        //Business.GetRecepcionesCantidad(ref _lstEsquemaTablaRecepciones);
        //        //MONICA NO SE UTILIZA countLineas = ConexionSQL.getDataTable("Select IDPEDIDOPRO, count (distinct IDPEDIDOPROLIN) Count From tblentradas ent GROUP BY IDPEDIDOPRO");
        //        dtPedidos = Business.GetPedidosProveedorDatosGridView(_lstEsquemaTablaProveedores);
        //        CreateChildTemplatePedidosProv();
        //        Debug.WriteLine("Fin  backgroundWorkerRecepciones_DoWork ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));
        //        log.Info("Fin  backgroundWorkerRecepciones_DoWork ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
        //    }
        //}
        private void bgWorkerPedidosCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio  backgroundWorkerPedidosCompleted ProveedoresPedidosCabGridView ");
            rgvPedidos.BeginUpdate();
            rgvPedidos.DataSource = dtPedidos;
            //MONICA lo paso al inicio gtJerarquicoPedidosLineasPedidos.Caption = Lenguaje.traduce("Lineas");
            //MONICA lo paso al iniciogtJerarquicoPedidosEntradas.Caption =Lenguaje.traduce("Entradas");
            //MONICA lo paso al iniciogtJerarquicoPedidosPreavisos.Caption =Lenguaje.traduce("Preavisos");
            //MONICA lo paso al iniciogtJerarquicoPedidosRecepciones.Caption = Lenguaje.traduce("Recepciones");
            OcultarColumnas();

            try
            {
                rgvPedidos.Templates.Add(gtJerarquicoPedidosLineasPedidos);
                rgvPedidos.Templates.Add(gtJerarquicoPedidosRecepciones);
                rgvPedidos.Templates.Add(gtJerarquicoPedidosEntradas);
                rgvPedidos.Templates.Add(gtJerarquicoPedidosPreavisos);
                rbtnPedidos.Enabled = false;
                rBtnGenerarTarea.Enabled = false;
                // LO PASO AL INICIOSetPreferences();
                // LO PASO AL INICIOInitializeThemesDropDown();
                //NO SE USATelerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
                rgvPedidos.BestFitColumns();
                rgvPedidos.Columns.Remove(rgvPedidos.Columns["RowNum"]);
                ColorEstadoPedidos();
                OcultarCamposPedidos();
                AñadirLabelCantidad();
                /*LO PASO AL INICIOTextoBotones();
                CrearBotonesPedidos();
               CrearBotonesRecepciones();*/
                rrbBarraBotones.Enabled = true;
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + "Fin  backgroundWorkerPedidosCompleted ProveedoresPedidosCabGridView ");
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + "Inicio Crear Columna check ");
            GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
            checkBoxColumn.DataType = typeof(int);
            checkBoxColumn.Name = "Check";
            checkBoxColumn.AllowReorder = false;
            checkBoxColumn.AllowFiltering = false;

            rgvPedidos.MasterTemplate.Columns.Add(checkBoxColumn);
            rgvPedidos.Columns.Move(rgvPedidos.Columns.Count - 1, 0);
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin Crear Columna check");
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio readonly columnas");
            foreach (GridViewDataColumn item in rgvPedidos.Columns)
            {
                if (item is GridViewCheckBoxColumn)
                {
                    item.ReadOnly = false;
                }
                else
                {
                    item.ReadOnly = true;
                }
            }
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin readonly columnas");
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio crear check recepciones");

            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin crear check recepciones");

            ((RibbonTab)rrbBarraBotones.CommandTabs[0]).Items.Remove(grupoPedidos);
            ((RibbonTab)rrbBarraBotones.CommandTabs[0]).Items.Remove(configBarGroup);
            ((RibbonTab)rrbBarraBotones.CommandTabs[0]).Items.Add(grupoRecepciones);
            ((RibbonTab)rrbBarraBotones.CommandTabs[0]).Items.Add(configBarGroup);
            ((RibbonTab)rrbBarraBotones.CommandTabs[0]).Items.Add(grupoPedidos);
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio añadir columnas lineas y elegir estilo");
            //dtPedidosLineas = ConexionSQL.getDataTable("SELECT * FROM TBLPEDIDOSPROLIN");

            try
            {
                AñadirColumnas();
                AñadirValorProgressBar();
                ElegirEstilo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin añadir columnas lineas y elegir estilo");
            //MONICA LO PONGO EN EL LOADConfigurarComboFormatoFecha();

            waitingBar.StopWaiting();
            rgvPedidos.EndUpdate();
        }

        private void OcultarCamposPedidos()
        {
            try
            {
                for (int i = 0; i < _lstEsquemaTablaProveedores.Count; i++)
                {
                    if (!_lstEsquemaTablaProveedores[i].EsVisible)
                    {
                        rgvPedidos.Columns[_lstEsquemaTablaProveedores[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void OcultarCamposrecepciones()
        {
            try
            {
                for (int i = 0; i < _lstEsquemaTablaRecepciones.Count; i++)
                {
                    if (!_lstEsquemaTablaRecepciones[i].EsVisible)
                    {
                        rgvRecepciones.Columns[_lstEsquemaTablaRecepciones[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void rgvRecepciones_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.Index > -1 && e.Row.HierarchyLevel == 1)
            {
                //first level template
                GridViewTemplate template = e.Row.ViewTemplate;
                if (template.Caption == Lenguaje.traduce("Embalajes"))
                {
                    GridViewRowInfo fila = e.Row;
                    embalajeSeleccionado = Convert.ToInt32(fila.Cells[Lenguaje.traduce("ID Movimiento")].Value);
                }
                else
                {
                    embalajeSeleccionado = 0;
                }
                if (template.Caption == Lenguaje.traduce("Lineas"))
                {
                    GridViewRowInfo fila = e.Row;
                    try
                    {
                        idPedidoProSeleccionado = Convert.ToInt32(fila.Cells[Lenguaje.traduce("ID Pedido")].Value);
                        idPedidoProLinSeleccionado = Convert.ToInt32(fila.Cells[Lenguaje.traduce("ID Pedido Pro Lin")].Value);
                    }
                    catch (Exception ex)
                    {
                        log.Info("rgvRecepciones_CellClick no ha encontrado ID Pedido o ID Pedido Pro Lin");
                        idPedidoProSeleccionado = 0;
                        idPedidoProLinSeleccionado = 0;
                    }
                }
                else
                {
                    idPedidoProSeleccionado = 0;
                    idPedidoProLinSeleccionado = 0;
                }
            }
            else
            {
                embalajeSeleccionado = 0;
                idPedidoProSeleccionado = 0;
                idPedidoProLinSeleccionado = 0;
            }
        }

        private void OcultarColumnas()
        {
            if (rgvPedidos.Columns[colID] != null)
            {
                rgvPedidos.Columns[colID].IsVisible = false;
            }
            /*if (rgvPedidos.Columns[Lenguaje.traduce("ID Proveedor")] != null)
            {
                rgvPedidos.Columns[Lenguaje.traduce("ID Proveedor")].IsVisible = false;
            }*/
            /*if (rgvVistaPrincipal.Columns["Supplier ID"] != null)
            {
                rgvVistaPrincipal.Columns["Supplier ID"].IsVisible = false;
            }*/
        }

        private void ConfigurarComboFormatoFecha()
        {
            menuHeaderFormatoFecha.Text = "Cambiar Formato Fecha";
            RadListDataItem a = new RadListDataItem();
            a.Tag = 0;
            a.Text = Lenguaje.traduce("Con Horas");
            RadListDataItem b = new RadListDataItem();
            b.Tag = 1;
            b.Text = Lenguaje.traduce("Sin Horas");
            menuComboFormatoFecha.Items.Add(a);
            menuComboFormatoFecha.Items.Add(b);

            menuComboFormatoFecha.ComboBoxElement.SelectedIndexChanged += CambiarFormatoFecha;
        }

        private Object[] FormarLineasJSON(string idpedidopro)
        {
            //DataTable temporal = Business.GetPedidosProveedorJerarquicoLineas(_lstEsquemaTablaLineasPedido, idpedidopro);
            DataTable temporal = ConexionSQL.getDataTable("SELECT lin.IDPEDIDOPROLIN," +
            "case when art.sincontrolstock = 1 then 0 else" +
            "(lin.cantidad - coalesce(vc.cantidadcerrada, 0) -" +
            "(case when coalesce(va.cantidadabierta, 0) > coalesce(vr.cantidadrecepecion, 0) " +
            "then coalesce(va.cantidadabierta, 0) else coalesce(vr.cantidadrecepecion, 0) end)) end " +
            "as 'Pendiente' FROM TBLPEDIDOSPROLIN  lin " +
            "left join VCANTIDADENTRADARECEPECIONCERRADA vc on vc.idpedidopro = lin.idpedidopro and vc.idpedidoprolin = lin.idpedidoprolin " +
            "left join VCANTIDADENTRADARECEPECIONABIERTA va on va.idpedidopro = lin.idpedidopro and va.idpedidoprolin = lin.idpedidoprolin " +
            "left join VCANTIDADRECEPECIONABIERTA vr on vr.idpedidopro = lin.idpedidopro and vr.idpedidoprolin = lin.idpedidoprolin " +
            "left join tblarticulos art on lin.idarticulo = art.idarticulo  where lin.idpedidopro =" + idpedidopro + " AND (case when art.sincontrolstock = 1 then 0 else" +
            "(lin.cantidad - coalesce(vc.cantidadcerrada, 0) -" +
            "(case when coalesce(va.cantidadabierta, 0) > coalesce(vr.cantidadrecepecion, 0) " +
            "then coalesce(va.cantidadabierta, 0) else coalesce(vr.cantidadrecepecion, 0) end)) end )>0");
            Object[] arrayLineas = new Object[temporal.Rows.Count];
            string json = string.Empty;
            int i = 0;
            foreach (DataRow row in temporal.Rows)
            {
                dynamic linea = new ExpandoObject();
                linea.idpedidopro = int.Parse(idpedidopro);
                linea.idpedidoprolin = int.Parse(row["IDPEDIDOPROLIN"].ToString());
                linea.cantidadpterecibir = int.Parse(row["Pendiente"].ToString());
                linea.albarantransportista = "";
                //linea.albarantransportist=a;
                json += JsonConvert.SerializeObject(linea, Formatting.Indented);
                if (linea.cantidadpterecibir < 0)
                {
                    arrayLineas = null;
                    break;
                }
                else
                {
                    arrayLineas[i] = linea;
                }
                i++;
            }
            return arrayLineas;
        }

        private void rBtnCerrarPedido_Click(object sender, EventArgs e)
        {
            if (this.pedidosFilaSeleccionados.Count > 0)
            {
                WSRecepcionMotorClient wsr = new WSRecepcionMotorClient();
                try
                {
                    List<int> listaPedidosCerrar = new List<int>();
                    //int idPedidoPro = int.Parse(this.rgvPedidos.SelectedRows[0].Cells[Lenguaje.traduce("ID")].Value.ToString());
                    foreach (var row in pedidosFilaSeleccionados)
                    {
                        listaPedidosCerrar.Add(int.Parse(row.Cells[Lenguaje.traduce("ID")].Value.ToString()));
                    }

                    DialogResult dialog = MessageBox.Show(Lenguaje.traduce("Vas a cerrar la cantidad de " + listaPedidosCerrar.Count + " pedido/s. ¿Estas seguro?"),
                        Lenguaje.traduce("Advertencia"), MessageBoxButtons.OKCancel);
                    if (dialog == DialogResult.Cancel)
                    {
                        return;
                    }

                    foreach (int pedido in listaPedidosCerrar)
                    {
                        wsr.cerrarPedidoProveedor(pedido, DatosThread.getInstancia().getArrayDatos());
                    }
                    Refrescar();
                    RadMessageBox.Show(this, Lenguaje.traduce("Pedido cerrado"), Lenguaje.traduce("Resultado"));
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarErrorWS(ex, wsr.Endpoint);
                }
            }
            else
            {
                RadMessageBox.Show(this, Lenguaje.traduce("Debes seleccionar una fila"), Lenguaje.traduce("Resultado"));
            }
        }

        private void rBtnCrearRecepcionParcial_Click(object sender, EventArgs e)
        {
            log.Info(" Los pedidos son ;" + pedidosFilaSeleccionados.Count);

            FrmCrearRecepcionParcial fap = new FrmCrearRecepcionParcial();
            fap.ShowDialog();
        }

        private void AñadirLabelCantidad()
        {
            lblNumRegistros.Text = Lenguaje.traduce("Registros:") + CantidadRegistros.ToString();
            lblNumRegistros.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            grupoPedidos.Items.Add(lblNumRegistros);
            this.ribbonTab1.Items.AddRange(grupoPedidos);
        }

        private void TextoBotones()
        {
            rBtnRecepciones.Text = "Recepción";
            rbtnPedidos.Text = "Pedidos";
            rBtnGenerarTarea.Text = "Enviar Tarea";
            editarMenuItem.Text = "Columnas";
            temasMenuItem.Text = "Temas";
            guardarMenuItem.Text = "Guardar Estilo";
            cargarMenuItem.Text = "Cargar Estilo";
            ribbonTab1.Text = "Acciones";
            procesosBarGroup.Text = "Vistas";
            configBarGroup.Text = "Configuración";
            tablaGroup.Text = "Ver";
            rBtnBorrarFiltro.Text = "Limpiar Filtros";
            /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {*/
            //MONICA no se utilizaname = "ProveedoresPedidos";
            GridDataView dataView = rgvPedidos.MasterTemplate.DataView as GridDataView;
            lblNumRegistros.Text = "Registros:" + dataView.Indexer.Items.Count;
            /*}
            else
            {
                name = "Order Supplier";
                GridDataView dataView = rgvPedidos.MasterTemplate.DataView as GridDataView;
                lblNumRegistros.Text = Lenguaje.traduce("Registros:")+ dataView.Indexer.Items.Count;
            }*/
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
            //this.WindowState = FormWindowState.Maximized;
        }

        private void OcultarBarraBusqueda(object sender, EventArgs e)
        {
            if (rgvPedidos.AllowSearchRow == true)
            {
                rgvPedidos.AllowSearchRow = false;
            }
            else
            {
                rgvPedidos.AllowSearchRow = true;
            }
            if (rgvRecepciones.AllowSearchRow == true)
            {
                rgvRecepciones.AllowSearchRow = false;
            }
            else
            {
                rgvRecepciones.AllowSearchRow = true;
            }
        }

        private void OcultarFiltros(object sender, EventArgs e)
        {
            FiltroInicialRecepciones fi = new FiltroInicialRecepciones("ProveedoresPedidosCab");
            if (fi.ShowDialog() == DialogResult.OK)
            {
                Refrescar();
            }
        }

        private void SetPreferences()
        {
            try
            {
                //this.radGridViewControl.Size = new System.Drawing.Size(1276, 848);
                this.rgvPedidos.MasterTemplate.EnableFiltering = true;
                this.rgvPedidos.MasterTemplate.EnableGrouping = true;
                this.rgvPedidos.ShowGroupPanel = true;
                this.rgvPedidos.MasterTemplate.AutoExpandGroups = true;
                this.rgvPedidos.EnableHotTracking = true;
                this.rgvPedidos.MasterTemplate.AllowAddNewRow = false;
                this.rgvPedidos.MasterTemplate.AllowColumnResize = true;
                this.rgvPedidos.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvPedidos.AllowSearchRow = true;
                this.rgvPedidos.AllowSearchRow = true;
                this.rgvPedidos.EnablePaging = false;
                this.rgvPedidos.TableElement.RowHeight = 40;
                this.rgvPedidos.AllowRowResize = false;
                this.rgvPedidos.MasterView.TableSearchRow.SearchDelay = 2000;

                //this.rgvRecepciones.Size = new System.Drawing.Size(1276, 848);
                this.rgvRecepciones.MasterTemplate.EnableFiltering = true;
                this.rgvRecepciones.MasterTemplate.EnableGrouping = true;
                this.rgvRecepciones.ShowGroupPanel = true;
                this.rgvRecepciones.MasterTemplate.AutoExpandGroups = true;
                this.rgvRecepciones.EnableHotTracking = true;
                this.rgvRecepciones.MasterTemplate.AllowAddNewRow = false;
                this.rgvRecepciones.MasterTemplate.AllowColumnResize = true;
                this.rgvRecepciones.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvRecepciones.AllowSearchRow = true;
                this.rgvRecepciones.EnablePaging = false;
                this.rgvRecepciones.TableElement.RowHeight = 40;
                this.rgvRecepciones.AllowRowResize = false;
                this.rgvRecepciones.MasterView.TableSearchRow.SearchDelay = 2000;
                this.rgvRecepciones.ValueChanged += radGridView1_ValueChanged;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Constructor

        #region Jerarquia

        private void rgvPedidos_RowSourceNeeded(object sender, GridViewRowSourceNeededEventArgs e)
        {
            try
            {
                String mensajeError = "";
                string id = string.Empty;
                DataRowView rowView = e.ParentRow.DataBoundItem as DataRowView;
                //TODO
                if (e.Template.Caption == Lenguaje.traduce("Lineas"))
                {
                    //long t = Environment.TickCount;
                    try
                    {
                        DataRow[] rows = rowView.Row.GetChildRows(Lenguaje.traduce("ID"));
                        id = e.ParentRow.Cells[Lenguaje.traduce("ID")].Value.ToString();
                        e.Template.MasterTemplate.MultiSelect = true;
                        DataTable lineas = Business.GetPedidosProveedorJerarquicoLineas(_lstEsquemaTablaLineasPedido, id);
                        if (lineas.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        for (int a = 0; a < lineas.Rows.Count; a++)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();

                            for (int i = 0; i < lineas.Columns.Count; i++)
                            {
                                try
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][lineas.Columns[i].ColumnName].ToString();
                                }
                                catch (Exception error)
                                {
                                    mensajeError += "Error añadiendo la columna " + lineas.Rows[a][lineas.Columns[i].ColumnName].ToString() + " en la linea:" + a + " error:" + error.Message;
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        //t = Environment.TickCount - t;
                        //log.Debug("Tiempo Lineas=" + t);
                    }
                    /*try
                    {
                        foreach (GridViewRowInfo row in e.SourceCollection)
                        {
                            DataTable progressColumnPorcentEntradasPteUbicar;
                            string cantPend = colCantidadPteUbicar;

                            progressColumnPorcentEntradasPteUbicar = ConexionSQL.getDataTable("SELECT L.IDPEDIDOPRO,L.IDPEDIDOPROLIN ,CAST((ISNULL(SUM(R.CANTIDAD), 0) / CAST(L.cantidad AS DECIMAL(9, 2)) * 100) AS DECIMAL(9, 2)) AS[%CTDADPTEUBICAR]" +
                            "FROM TBLPEDIDOSPROLIN L LEFT JOIN TBLRESERVAS R ON L.IDPEDIDOPRO = R.IDPEDIDOPRO AND L.IDPEDIDOPROLIN = R.IDPEDIDOPROLIN AND IDRESERVATIPO IN('RT') WHERE L.IDPEDIDOPROLIN = " + row.Cells[Lenguaje.traduce("ID Linea")].Value.ToString() +
                            " AND L.IDPEDIDOPRO=" + row.Cells[colID].Value.ToString() +
                            " GROUP BY L.IDPEDIDOPRO, L.IDPEDIDOPROLIN, L.CANTIDAD");

                            if (progressColumnPorcentEntradasPteUbicar.Rows.Count != 0)
                            {
                                float var;
                                float.TryParse(progressColumnPorcentEntradasPteUbicar.Rows[0][Lenguaje.traduce("%CTDADPTEUBICAR")].ToString(), out var);
                                row.Cells[cantPend].Value = Math.Round(var);
                            }
                            else
                            {
                                row.Cells[cantPend].Value = 0;
                            }
                        }
                        foreach (GridViewRowInfo row in e.SourceCollection)
                        {
                            DataTable progressColumnPorcentEntrada;
                            string entrada = colCantidadEntrada;
                            progressColumnPorcentEntrada = ConexionSQL.getDataTable("SELECT L.idpedidopro,L.idpedidoprolin,isnull(CAST ((E.CANTIDAD / CAST(L.cantidad AS DECIMAL(9, 2)) * 100) AS DECIMAL(9, 2)),0)" +
                                " AS[%Entrada]" +
                            "FROM TBLPEDIDOSPROLIN L left JOIN VRESUMENENTRADA E ON L.IDPEDIDOPRO = E.IDPEDIDOPRO AND L.IDPEDIDOPROLIN = " +
                            "E.IDPEDIDOPROLIN " +
                            "WHERE l.IDPEDIDOPRO=" + row.Cells[colID].Value.ToString()
                            + " AND L.IDPEDIDOPROLIN = " + row.Cells[Lenguaje.traduce("ID Linea")].Value.ToString());

                            if (progressColumnPorcentEntrada.Rows.Count != 0)
                            {
                                float var;
                                string columna = "%Entrada";
                                float.TryParse(progressColumnPorcentEntrada.Rows[0][columna].ToString(), out var);
                                row.Cells[entrada].Value = Math.Round(var);
                            }
                            else
                            {
                                row.Cells[entrada].Value = 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        //t = Environment.TickCount - t;
                        //log.Debug("Tiempo traducir=" + t);
                    }*/
                }
                else
                {
                    if (e.Template.Caption == colRecepciones)
                    {
                        /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                        {*/
                        DataRow[] rows = rowView.Row.GetChildRows("ID");
                        id = e.ParentRow.Cells[Lenguaje.traduce("ID")].Value.ToString();
                        /*}
                        else
                        {
                            DataRow[] rows = rowView.Row.GetChildRows("ID");
                            id = e.ParentRow.Cells["ID"].Value.ToString();
                        }*/
                        DataTable jerarquia = Business.GetPedidosProveedorJerarquicoRecepciones(_lstEsquemaTablaRecepciones, _lstEsquemaTablaRecepcionesUnion, id);
                        if (jerarquia.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        for (int a = 0; a < jerarquia.Rows.Count; a++)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();

                            for (int i = 0; i < jerarquia.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText.Contains("%"))
                                    {
                                    }
                                    else
                                    {
                                        row.Cells[jerarquia.Columns[i].ColumnName].Value = jerarquia.Rows[a][row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                    }
                                }
                                catch (Exception error)
                                {
                                    mensajeError += "Error añadiendo la columna " + jerarquia.Rows[a][row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString() + " en la linea:" + a + " error:" + error.Message;
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    else
                    {
                        if (e.Template.Caption == Lenguaje.traduce("Entradas"))
                        {
                            /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                            {*/
                            DataRow[] rows = rowView.Row.GetChildRows("ID");
                            id = e.ParentRow.Cells["ID"].Value.ToString();
                            /*}
                            else
                            {
                                DataRow[] rows = rowView.Row.GetChildRows("ID");
                                id = e.ParentRow.Cells["ID"].Value.ToString();
                            }*/
                            DataTable jerarquia = Business.GetPedidosProveedorJerarquicoEntradas(_lstEsquemaTablaEntradasPedido, id)/* ConexionSQL.getDataTable("SELECT * FROM TBLENTRADAS WHERE IDPEDIDOPRO=" + columnaFecha)*/;
                            if (jerarquia.Rows.Count == 0)
                            {
                                GridViewRowInfo row = e.Template.Rows.NewRow();
                                e.SourceCollection.Add(row);
                            }
                            for (int a = 0; a < jerarquia.Rows.Count; a++)
                            {
                                GridViewRowInfo row = e.Template.Rows.NewRow();

                                for (int i = 0; i < jerarquia.Columns.Count; i++)
                                {
                                    try
                                    {
                                        if (row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText.Contains("%"))
                                        {
                                        }
                                        else
                                        {
                                            row.Cells[jerarquia.Columns[i].ColumnName].Value = jerarquia.Rows[a][row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                    catch (Exception error)
                                    {
                                        mensajeError += "Error añadiendo la columna " + jerarquia.Rows[a][row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString() + " en la linea:" + a + " error:" + error.Message;
                                    }
                                }
                                e.SourceCollection.Add(row);
                            }
                        }
                        else
                        {
                            if (e.Template.Caption == Lenguaje.traduce("Preavisos"))
                            {
                                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                                {*/
                                DataRow[] rows = rowView.Row.GetChildRows("ID");
                                id = e.ParentRow.Cells[Lenguaje.traduce("ID")].Value.ToString();
                                /*}
                                else
                                {
                                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                                    id = e.ParentRow.Cells["ID"].Value.ToString();
                                }*/
                                DataTable jerarquia = new DataTable();

                                if (_lstEsquemaTablaPreavisos.Count > 0)
                                {
                                    jerarquia = Business.GetPedidosProveedorJerarquicoPreavisos(_lstEsquemaTablaPreavisos, id)/* ConexionSQL.getDataTable("SELECT * FROM TBLENTRADAS WHERE IDPEDIDOPRO=" + columnaFecha)*/;
                                }
                                if (jerarquia.Rows.Count == 0)
                                {
                                    GridViewRowInfo row = e.Template.Rows.NewRow();
                                    e.SourceCollection.Add(row);
                                }
                                for (int a = 0; a < jerarquia.Rows.Count; a++)
                                {
                                    GridViewRowInfo row = e.Template.Rows.NewRow();

                                    for (int i = 1; i < jerarquia.Columns.Count; i++)
                                    {
                                        try
                                        {
                                            if (row.Cells[jerarquia.Columns[i].ColumnName] != null)
                                            {
                                                if (row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText.Contains("%"))
                                                {
                                                }
                                                else
                                                {
                                                    row.Cells[jerarquia.Columns[i].ColumnName].Value = jerarquia.Rows[a][row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                                }
                                            }
                                        }
                                        catch (Exception error)
                                        {
                                            mensajeError += "Error añadiendo la columna " + jerarquia.Rows[a][row.Cells[jerarquia.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString() + " en la linea:" + a + " error:" + error.Message;
                                        }
                                    }
                                    e.SourceCollection.Add(row);
                                }
                            }
                        }
                    }
                }
                if (!mensajeError.Equals(""))
                {
                    log.Error(mensajeError);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void CreateChildTemplatePedidosProv()
        {
            try
            {
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio CreateChildTemplatePedidosProv ProveedoresPedidosCabGridView");
                //RECEPCIONES
                CreateChildTemplateJerarquicoPedidosRecepciones();

                //LINEAS PEDIDOS
                CreateChildTemplateJerarquicoLineasPedido();

                //ENTRADAS
                CreateChildTemplateJerarquicoPedidosEntradas();

                //PREAVISOS
                CreateChildTemplateJerarquicoPedidosPreavisos();

                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin CreateChildTemplatePedidosProv ProveedoresPedidosCabGridView");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void CreateChildTemplateJerarquicoPedidosPreavisos()
        {
            gtJerarquicoPedidosPreavisos.Caption = Lenguaje.traduce("Preavisos");
            string jsonPreavisos = DataAccess.LoadJson("PedidosProvJerarquicoPreavisos");
            JArray jsPreavisos = JArray.Parse(jsonPreavisos);
            string jsonEsquemaPreavisos = jsPreavisos.First()["Scheme"].ToString();
            List<TableScheme> esquemaPreavisos = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaPreavisos);
            gtJerarquicoPedidosPreavisos.ReadOnly = true;
            for (int i = 0; i < esquemaPreavisos.Count; i++)
            {
                if (esquemaPreavisos[i].Etiqueta != string.Empty && esquemaPreavisos[i].Etiqueta != null)
                {
                    gtJerarquicoPedidosPreavisos.Columns.Add(esquemaPreavisos[i].Etiqueta);
                    if (!esquemaPreavisos[i].EsVisible)
                    {
                        gtJerarquicoPedidosPreavisos.Columns[esquemaPreavisos[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoPedidosPreavisos.Columns.Add(checkBoxColumnJerarquicoPreavisos);
            gtJerarquicoPedidosPreavisos.BestFitColumns();
            gtJerarquicoPedidosPreavisos.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoPedidosPreavisos);
        }

        private void CreateChildTemplateJerarquicoPedidosEntradas()
        {
            gtJerarquicoPedidosEntradas.Caption = Lenguaje.traduce("Entradas");
            string jsonEntradas = DataAccess.LoadJson("PedidosProvJerarquicoEntradas");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            gtJerarquicoPedidosEntradas.ReadOnly = true;
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    gtJerarquicoPedidosEntradas.Columns.Add(esquemaEntradas[i].Etiqueta);
                    /*if (gtJerarquicoPedidosEntradas.Columns[esquemaEntradas[i].Etiqueta].HeaderText.Contains("ID"))
                    {
                        gtJerarquicoPedidosEntradas.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }*/
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        gtJerarquicoPedidosEntradas.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoPedidosEntradas.BestFitColumns();
            gtJerarquicoPedidosEntradas.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoPedidosEntradas);
        }

        private void CreateChildTemplateJerarquicoPedidosRecepciones()
        {
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio CreateChildTemplateJerarquicoPedidosRecepciones ProveedoresPedidosCabGridView" );
            gtJerarquicoPedidosRecepciones.Caption = colRecepciones;
            string jsonRecepciones = DataAccess.LoadJson("RecepcionesCab");
            JArray jsRecepciones = JArray.Parse(jsonRecepciones);
            string jsonEsquemaRecepciones = jsRecepciones.First()["Scheme"].ToString();
            List<TableScheme> esquemaRecepciones = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaRecepciones);
            //templateRecepciones.ReadOnly = true;
            for (int i = 0; i < esquemaRecepciones.Count; i++)
            {
                if (esquemaRecepciones[i].Etiqueta != string.Empty && esquemaRecepciones[i].Etiqueta != null)
                {
                    gtJerarquicoPedidosRecepciones.Columns.Add(esquemaRecepciones[i].Etiqueta);
                    if (!esquemaRecepciones[i].EsVisible)
                    {
                        gtJerarquicoPedidosRecepciones.Columns[esquemaRecepciones[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            if (gtJerarquicoPedidosRecepciones.Columns["ID"] != null)
            {
                gtJerarquicoPedidosRecepciones.Columns["ID"].IsVisible = false;
            }
            gtJerarquicoPedidosRecepciones.BestFitColumns();
            gtJerarquicoPedidosRecepciones.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoPedidosRecepciones);
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin CreateChildTemplateJerarquicoPedidosRecepciones ProveedoresPedidosCabGridView");
        }

        private void OcultarColumnasLineas()
        {
            if (gtJerarquicoPedidosLineasPedidos.Columns["ID"] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns["ID"].IsVisible = false;
            }
            if (gtJerarquicoPedidosLineasPedidos.Columns[Lenguaje.traduce("ID Linea")] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns[Lenguaje.traduce("ID Linea")].IsVisible = false;
            }
            /*if (gtJerarquicoPedidosLineasPedidos.Columns["Line ID"] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns["Line ID"].IsVisible = false;
            }*/
            if (gtJerarquicoPedidosLineasPedidos.Columns[Lenguaje.traduce("ID Articulo")] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns[Lenguaje.traduce("ID Articulo")].IsVisible = false;
            }
            /*if (gtJerarquicoPedidosLineasPedidos.Columns["Item ID"] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns["Item ID"].IsVisible = false;
            }*/
            if (gtJerarquicoPedidosLineasPedidos.Columns["Atrib1"] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns["Atrib1"].IsVisible = false;
            }
            if (gtJerarquicoPedidosLineasPedidos.Columns["Atrib2"] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns["Atrib2"].IsVisible = false;
            }
            if (gtJerarquicoPedidosLineasPedidos.Columns["Atrib3"] != null)
            {
                gtJerarquicoPedidosLineasPedidos.Columns["Atrib3"].IsVisible = false;
            }
        }

        #endregion Jerarquia

        #region Botones

        private void rbtnPedidos_Click(object sender, EventArgs e)
        {
            rBtnRecepciones.Enabled = true;
            //radGridView1.BestFitColumns();
            rbtnPedidos.Enabled = false;
            rBtnGenerarTarea.Enabled = false;
            tlPrincipal.Controls.Remove(rgvRecepciones);
            tlPrincipal.Controls.Add(rgvPedidos);
            procesosBarGroup.Items.Remove(rBtnCrearRecepcion);
            rgvPedidos.FilterDescriptors.Clear();
            lblNumRegistros.Text = Lenguaje.traduce("Registros:") + rgvPedidos.Rows.Count.ToString();
            //setFiltros1();
            ColorEstadoPedidos();
            CambiarBotonesPedidos();
        }

        private void rBtnCrearRecepcion_Click(object sender, EventArgs e)
        {
            if (pedidosFilaSeleccionados.Count != 0)
            {
                if (pedidosFilaSeleccionados.Count > 1)
                {
                    String mensaje = Lenguaje.traduce("Tienes seleccionado " + pedidosFilaSeleccionados.Count +
                        " pedidos. Se va a crear una recepción con los " + pedidosFilaSeleccionados.Count +
                        " seleccionados. ¿Quieres continuar?");
                    if (RadMessageBox.Show(this, mensaje, Lenguaje.traduce("Confirmación"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Cancelando"));
                        return;
                    }
                }
                string idproveedor = string.Empty;
                string referenciaPedido = string.Empty;
                string codigoProveedor = string.Empty;
                ArrayList pedidosPro = new ArrayList();
                Object[] j = null;
                foreach (GridViewRowInfo row in pedidosFilaSeleccionados)
                {
                    if (row.Cells[Lenguaje.traduce("ID Proveedor")] == null)
                    {
                        throw new Exception(Lenguaje.traduce("No existe proveedor"));
                    }

                    idproveedor = row.Cells[Lenguaje.traduce("ID Proveedor")].Value.ToString();
                    if (idproveedor == string.Empty)
                    {
                        throw new Exception(Lenguaje.traduce("Error identificando el IDPROVEEDOR de la tabla"));
                    }
                    if (row.Cells[Lenguaje.traduce("ID")].Value == null)
                    {
                        throw new Exception(Lenguaje.traduce("No existe pedido"));
                    }
                    string idPedidoPro = row.Cells[Lenguaje.traduce("ID")].Value.ToString();
                    referenciaPedido = row.Cells[Lenguaje.traduce("Codigo")].Value.ToString();
                    codigoProveedor = row.Cells[Lenguaje.traduce("Codigo Proveedor")].Value.ToString();

                    pedidosPro.Add(referenciaPedido);
                    if (row.Cells[Lenguaje.traduce("Estado")].Value.ToString() == "PC")
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se puede crear la recepción porque el pedido está cerrado"));
                        return;
                    }
                    Object[] lineas = FormarLineasJSON(idPedidoPro);
                    if (j == null)
                    {
                        j = lineas;
                    }
                    else
                    {
                        j = j.Concat(lineas).ToArray();
                    }
                }

                if (j != null && j.Count() > 0)
                {
                    AgregarRecepcion formAñadir = new AgregarRecepcion(j, idproveedor, codigoProveedor, pedidosPro);
                    DialogResult result;
                    result = formAñadir.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        MessageBox.Show(Lenguaje.traduce("Recepción generada correctamente"));
                        Refrescar();

                        pedidosSeleccionados.Clear();
                        pedidosFilaSeleccionados.Clear();
                    }
                    else
                    {
                        if (result == DialogResult.Abort)
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, Lenguaje.traduce("No se puede crear la recepción porque no hay cantidad pendiente de recibir"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Por favor,seleccione una fila"));
            }
        }

        private void rBtnEditarRecepcion_Click(object sender, EventArgs e)
        {
            if (recepcionesSeleccionadas.Count != 0)
            {
                string albaran = rgvRecepciones.SelectedRows[0].Cells[Lenguaje.traduce("Albaran Transportista")].Value.ToString();
                string fechaRecepcion = rgvRecepciones.SelectedRows[0].Cells[Lenguaje.traduce("F.Recepcion")].Value.ToString();
                string chofer = rgvRecepciones.SelectedRows[0].Cells[Lenguaje.traduce("Chofer")].Value.ToString();
                string matricula = rgvRecepciones.SelectedRows[0].Cells[Lenguaje.traduce("Matricula")].Value.ToString();
                string observaciones = rgvRecepciones.SelectedRows[0].Cells[Lenguaje.traduce("Obs")].Value.ToString();
                string muelle = rgvRecepciones.SelectedRows[0].Cells[Lenguaje.traduce("Muelle")].Value.ToString();
                string transportista = rgvRecepciones.SelectedRows[0].Cells[Lenguaje.traduce("Transportista")].Value.ToString();
                AgregarRecepcion formAñadir = new AgregarRecepcion(albaran, fechaRecepcion, muelle, transportista, matricula, chofer, observaciones);
                formAñadir.ShowDialog();
            }
        }

        private void rBtnCerrarRecepcion_Click(object sender, EventArgs e)
        {
            string mensaje = Lenguaje.traduce("Se van a cerrar las siguientes recepciones seleccionadas:");
            string json = string.Empty;
            String nombreRecepcion = "";
            string recepcionesSinEntrada = "";
            try
            {
                for (int i = 0; i < recepcionesSeleccionadas.Count; i++)
                {
                    int cantidadRecepciones = getCantidadEntradasRecepcionadas(recepcionesSeleccionadas[i]);
                    if (cantidadRecepciones == 0)
                    {
                        nombreRecepcion = getNombreRecepcion(recepcionesSeleccionadas[i]);
                        recepcionesSinEntrada += "\n" + nombreRecepcion;
                    }
                }

                if (!recepcionesSinEntrada.Equals(""))
                {
                    DialogResult dialog = MessageBox.Show(Lenguaje.traduce("Las líneas de este pedido no han sido confirmadas por el lector de" +
                            " código de barras, ¿le gustaría cerrar el pedido?") + recepcionesSinEntrada,
                            Lenguaje.traduce("Advertencia"), MessageBoxButtons.OKCancel);
                    if (dialog == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                if (recepcionesSeleccionadas.Count > 0)
                {
                    for (int i = 0; i < recepcionesSeleccionadas.Count; i++)
                    {
                        var objeto = recepcionesSeleccionadas[i];
                        if (i != recepcionesSeleccionadas.Count - 1)
                        {
                            string resp = FormarJSON(objeto);
                            if (resp != null && resp != string.Empty)
                            {
                                json += resp + ",";
                                mensaje += "\n" + objeto;
                            }
                        }
                        else
                        {
                            string resp = FormarJSON(objeto);
                            if (resp != null && resp != string.Empty)
                            {
                                json += resp;
                                mensaje += "\n" + objeto;
                            }
                        }
                    }
                    if (json != null && json != string.Empty)
                    {
                        mensaje += "\n" + Lenguaje.traduce("¿Estas seguro?");
                        DialogResult dialog = MessageBox.Show(mensaje, Lenguaje.traduce("Advertencia"), MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            LlamarWSCerrarRecepcion(json);
                            Refrescar();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se puede crear una tarea sobre una recepción cerrada");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error intentando cerrar la recepcion:" + nombreRecepcion);
            }
        }

        private int getCantidadEntradasRecepcionadas(int recepcion)
        {
            int r = 0;
            try
            {
                String sql = "SELECT COUNT(*) FROM TBLENTRADAS WHERE IDRECEPCION = " + recepcion + "";
                r = int.Parse(ConexionSQL.getDataTable(sql).Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en getCantidadEntradasRecepcionadas, recepcion:" + recepcion);
            }
            return r;
        }

        private string getNombreRecepcion(int recepcion)
        {
            String r = "";
            try
            {
                String sql = "SELECT RECEPCION FROM TBLRECEPCIONESCAB WHERE IDRECEPCION = " + recepcion + "";
                r = ConexionSQL.getDataTable(sql).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en getCantidadEntradasRecepcionadas, recepcion:" + recepcion);
            }
            return r;
        }

        private void rBtnEditar_Click(object sender, EventArgs e)
        {
            this.rgvPedidos.ShowColumnChooser();
        }

        private string confirmacion = Lenguaje.traduce("Datos exportados correctamente. ¿Desea abrir el archivo?");

        public void rBtnExportacion_Click(object sender, EventArgs e)
        {
            FuncionesGenerales.exportarAExcelGenerico(rgvPedidos);
        }

        public virtual void loadItem_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón load " + DateTime.Now);
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(2);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                LoadLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                LoadLayoutLocal();
            }
        }

        public virtual void SaveItem_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón guardar " + DateTime.Now);
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                SaveLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                SaveLayoutLocal();
            }
        }

        private void rBtnBorrarFiltro_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón borrar filtro " + DateTime.Now);

            if (tlPrincipal.Controls[this.rgvPedidos.Name] != null)
            {
                if (rgvPedidos.IsInEditMode)
                {
                    rgvPedidos.EndEdit();
                }
                this.rgvPedidos.FilterDescriptors.Clear();
                this.rgvPedidos.GroupDescriptors.Clear();
                this.rgvPedidos.BestFitColumns();
                return;
            }
            else
            {
                if (rgvRecepciones.IsInEditMode)
                {
                    rgvRecepciones.EndEdit();
                }
                this.rgvRecepciones.FilterDescriptors.Clear();
                this.rgvRecepciones.GroupDescriptors.Clear();
                this.rgvRecepciones.BestFitColumns();
                return;
            }

            /*try
            {
                string path = Persistencia.ConfigPath + @"\FiltroInicial.xml";
                XDocument doc = XDocument.Load(path);
                foreach (var item in doc.Descendants("filtroRecepcionesVistaPedido"))
                {
                    item.Descendants().Remove();
                }
                doc.Save(path);
                Refrescar();
            }catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }*/
        }

        private void rBtnGenerarTarea_Click(object sender, EventArgs e)
        {
            if (recepcionesSeleccionadas.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                return;
            }
            WSRecepcionMotorClient webservicerecepcion = new WSRecepcionMotorClient();
            log.Info("Pulsado botón generar tarea por el operario:" + User.IdOperario + " sobre la recepción " + recepcionesSeleccionadas[0] + " el dia " + DateTime.Now);
            if (recepcionesSeleccionadas.Count != 0)
            {
                try
                {
                    foreach (GridViewRowInfo rowinfo in rgvRecepciones.Rows)
                    {
                        if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                        {
                            DataRow rowRecepciones = (DataRow)((DataRowView)rowinfo.DataBoundItem).Row;

                            if (rowinfo.Cells[Lenguaje.traduce("Estado")].Value.ToString().Equals("PC"))
                            {
                                RadMessageBox.Show(Lenguaje.traduce("La recepción está cerrada"));
                                return;
                            }
                        }
                    }

                    int id = recepcionesSeleccionadas[0];
                    Debug.WriteLine(webservicerecepcion.Endpoint);
                    int respuesta = webservicerecepcion.generarTareaRecepcion(id);
                    if (respuesta == -1)
                    {
                        MessageBox.Show(Lenguaje.traduce("Ha ocurrido un error generando la tarea"));
                    }
                    else
                    {
                        if (respuesta > -1)
                        {
                            MessageBox.Show(Lenguaje.traduce("Tarea generada correctamente"));
                            Refrescar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarErrorWS(ex, webservicerecepcion.Endpoint);
                }
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
            }
        }

        private void CrearBotonesPedidos()
        {
            rBtnCrearRecepcion.Image = global::RumboSGA.Properties.Resources.Add;
            rBtnCrearRecepcion.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnCrearRecepcion.Text = "Crear Recepción ";
            rBtnCrearRecepcion.TextAlignment = ContentAlignment.BottomCenter;
            rBtnCrearRecepcion.Click += rBtnCrearRecepcion_Click;
            grupoRecepciones.Items.Add(rBtnCrearRecepcion);

            rBtnRecepcionParcial.Image = global::RumboSGA.Properties.Resources.Add1;
            rBtnRecepcionParcial.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnRecepcionParcial.Text = "Crear Recepción Parcial ";
            rBtnRecepcionParcial.TextAlignment = ContentAlignment.BottomCenter;
            rBtnRecepcionParcial.Enabled = false;
            rBtnRecepcionParcial.Visibility = ElementVisibility.Hidden;
            rBtnRecepcionParcial.Click += rBtnCrearRecepcionParcial_Click;
            grupoRecepciones.Items.Add(rBtnRecepcionParcial);

            rBtnCerrarPedido.Image = global::RumboSGA.Properties.Resources.Cancel;
            rBtnCerrarPedido.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnCerrarPedido.Text = "Cerrar pedido ";
            rBtnCerrarPedido.TextAlignment = ContentAlignment.BottomCenter;
            rBtnCerrarPedido.Click += rBtnCerrarPedido_Click;
            grupoRecepciones.Items.Add(rBtnCerrarPedido);
        }

        private void rBtnAlta_Click(object sender, EventArgs e)
        {
            try
            {
                string operario = string.Empty;
                string tipoPalet = string.Empty;

                try
                {
                    if (recepcionesSeleccionadas.Count != 0)
                    {
                        for (int i = 0; i < rgvRecepciones.Rows.Count; i++)
                        {
                            for (int j = 0; j < recepcionesSeleccionadas.Count; j++)
                            {
                                if (rgvRecepciones.Rows[i].Cells[Lenguaje.traduce("ID")].Value.ToString()
                                    .Equals(recepcionesSeleccionadas[j].ToString()))
                                {
                                    if (rgvRecepciones.Rows[i].Cells[Lenguaje.traduce("Estado")].Value.ToString()
                                        .Equals("PC"))
                                    {
                                        RadMessageBox.Show(
                                            Lenguaje.traduce("Estas modificando una recepción cerrada PC, recepción:"
                                            + recepcionesSeleccionadas[j].ToString()));
                                        return;
                                    }
                                }
                            }
                        }
                        if (idPedidoProSeleccionado > 0 && idPedidoProLinSeleccionado > 0)
                        {
                            int idRecepcion = recepcionesSeleccionadas[0];
                            String nombreBotonPulsado = (sender as RadButtonElement).Name;
                            Image icono = (sender as RadButtonElement).Image;
                            String nombreFormularioOpcion = (sender as RadButtonElement).Text;
                            int opcion = 0;
                            switch (nombreBotonPulsado)
                            {
                                case "rBtnAltaPalet":
                                    opcion = FormularioAltaProductoRecepcion.AltaPalet;
                                    break;

                                case "rBtnAltaCheps":
                                    opcion = FormularioAltaProductoRecepcion.AltaCheps;
                                    break;

                                case "rBtnAltaPico":
                                    opcion = FormularioAltaProductoRecepcion.AltaPaletPicos;
                                    break;

                                case "rBtnAltaMulti":
                                    opcion = FormularioAltaProductoRecepcion.AltaPaletMultireferencia;
                                    break;

                                case "rBtnAltaCarro":
                                    opcion = FormularioAltaProductoRecepcion.AltaPaletCarroMovil;
                                    break;

                                case "rBtnAltaTotales":
                                    opcion = FormularioAltaProductoRecepcion.AltaPaletEntradasTotales;
                                    break;
                            }

                            FormularioAltaProductoRecepcion fap =
                                new FormularioAltaProductoRecepcion(opcion, idRecepcion, idPedidoProSeleccionado, idPedidoProLinSeleccionado, nombreFormularioOpcion, icono);
                            fap.ShowDialog();
                            refrescarTemplatesRecepciones();
                        }
                        else
                        {
                            MessageBox.Show(Lenguaje.traduce("Debes seleccionar una línea"));
                        }
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex, ex.Message);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void CrearBotonesRecepciones()
        {
            rBtnAltaPico.Image = global::RumboSGA.Properties.Resources.palletPicos;
            rBtnAltaPico.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnAltaPico.Text = "Alta Pico";
            rBtnAltaPico.TextAlignment = ContentAlignment.BottomCenter;
            rBtnAltaPico.Name = "rBtnAltaPico";
            rBtnAltaPico.Click += rBtnAlta_Click;

            rBtnAltaPalet.Image = global::RumboSGA.Properties.Resources.pallet;
            rBtnAltaPalet.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnAltaPalet.Text = "Alta Palet";
            rBtnAltaPalet.Name = "rBtnAltaPalet";
            rBtnAltaPalet.TextAlignment = ContentAlignment.BottomCenter;
            rBtnAltaPalet.Click += rBtnAlta_Click;

            rBtnAltaCarro.Image = global::RumboSGA.Properties.Resources.CarroMovil;
            rBtnAltaCarro.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnAltaCarro.Text = "Alta Carro";
            rBtnAltaCarro.Name = "rBtnAltaCarro";
            rBtnAltaCarro.TextAlignment = ContentAlignment.BottomCenter;
            rBtnAltaCarro.Click += rBtnAlta_Click;

            rBtnAltaMulti.Image = global::RumboSGA.Properties.Resources.palletMulti;
            rBtnAltaMulti.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnAltaMulti.Text = "Alta Multi";
            rBtnAltaMulti.Name = "rBtnAltaMulti";
            rBtnAltaMulti.TextAlignment = ContentAlignment.BottomCenter;
            rBtnAltaMulti.Click += rBtnAlta_Click;

            rBtnAltaTotales.Image = global::RumboSGA.Properties.Resources.addbox;
            rBtnAltaTotales.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnAltaTotales.Text = "Alta Totales";
            rBtnAltaTotales.Name = "rBtnAltaTotales";
            rBtnAltaTotales.TextAlignment = ContentAlignment.BottomCenter;
            rBtnAltaTotales.Click += rBtnAlta_Click;

            rBtnEditarRecepcion.Image = global::RumboSGA.Properties.Resources.edit;
            rBtnEditarRecepcion.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnEditarRecepcion.Text = "Editar Recepción";
            rBtnEditarRecepcion.TextAlignment = ContentAlignment.BottomCenter;
            rBtnEditarRecepcion.Click += rBtnEditarRecepcion_Click;

            rBtncerrarRecepcion.Image = global::RumboSGA.Properties.Resources.Cancel;
            rBtncerrarRecepcion.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtncerrarRecepcion.Text = "Cerrar Recepción";
            rBtncerrarRecepcion.TextAlignment = ContentAlignment.BottomCenter;
            rBtncerrarRecepcion.Click += rBtnCerrarRecepcion_Click;

            //rBtnClasificacionEntradas.Image = global::RumboSGA.Properties.Resources.Clasificacion;
            rBtnClasificacionEntradas.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnClasificacionEntradas.Text = "Clasificacion Entradas";
            rBtnClasificacionEntradas.TextAlignment = ContentAlignment.BottomCenter;
            rBtnClasificacionEntradas.Click += rBtnClasificacionEntradas_Click;
            rBtnClasificacionEntradas.Enabled = true;

            this.rBtnGenerarTarea.Image = global::RumboSGA.Properties.Resources.CopyFromTask;
            this.rBtnGenerarTarea.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnGenerarTarea.Name = "genTareaButton";
            this.rBtnGenerarTarea.Text = "Generar Tarea";
            this.rBtnGenerarTarea.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnGenerarTarea.Click += new System.EventHandler(this.rBtnGenerarTarea_Click);

            /*Cambio por menu item rBtnAgregarEmbalaje.Image = global::RumboSGA.Properties.Resources.Add;
            rBtnAgregarEmbalaje.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnAgregarEmbalaje.Text = "Agregar Embalaje";
            rBtnAgregarEmbalaje.TextAlignment = ContentAlignment.BottomCenter;
            rBtnAgregarEmbalaje.Click += rBtnAgregarEmbalaje_Click;*/
            agregarEmbalajeMenuItem.Name = "agregarEmbalaje";
            agregarEmbalajeMenuItem.Text = "Agregar Embalaje";
            agregarEmbalajeMenuItem.Image = global::RumboSGA.Properties.Resources.Add;
            agregarEmbalajeMenuItem.ImageAlignment = ContentAlignment.MiddleCenter;
            this.agregarEmbalajeMenuItem.Click += new System.EventHandler(this.rBtnAgregarEmbalaje_Click);

            /*cambio por menu itemrBtnModificarEmbalaje.Image = global::RumboSGA.Properties.Resources.edit;
            rBtnModificarEmbalaje.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnModificarEmbalaje.Text = "Modificar Embalaje";
            rBtnModificarEmbalaje.TextAlignment = ContentAlignment.BottomCenter;
            rBtnModificarEmbalaje.Click += rBtnModificarEmbalaje_Click;*/

            modificarEmbalajeMenuItem.Name = "modificarEmbalaje";
            modificarEmbalajeMenuItem.Text = "Modificar Embalaje";
            modificarEmbalajeMenuItem.Image = global::RumboSGA.Properties.Resources.edit;
            modificarEmbalajeMenuItem.ImageAlignment = ContentAlignment.MiddleCenter;
            this.modificarEmbalajeMenuItem.Click += new System.EventHandler(this.rBtnModificarEmbalaje_Click);

            /*cambio por menu itemrBtnEliminarEmbalaje.Image = global::RumboSGA.Properties.Resources.Delete;
            rBtnEliminarEmbalaje.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnEliminarEmbalaje.Text = "Eliminar Embalaje";
            rBtnEliminarEmbalaje.TextAlignment = ContentAlignment.BottomCenter;
            rBtnEliminarEmbalaje.Click += rBtnEliminarEmbalaje_Click;*/
            eliminarEmbalajeMenuItem.Name = "eliminarEmbalaje";
            eliminarEmbalajeMenuItem.Text = "Eliminar Embalaje";
            eliminarEmbalajeMenuItem.Image = global::RumboSGA.Properties.Resources.Delete;
            eliminarEmbalajeMenuItem.ImageAlignment = ContentAlignment.MiddleCenter;
            this.eliminarEmbalajeMenuItem.Click += new System.EventHandler(this.rBtnEliminarEmbalaje_Click);

            rBtnEntradaDirectaPreaviso.Image = global::RumboSGA.Properties.Resources.truck;
            rBtnEntradaDirectaPreaviso.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnEntradaDirectaPreaviso.Text = "Ent Directa Preaviso";
            rBtnEntradaDirectaPreaviso.TextAlignment = ContentAlignment.BottomCenter;
            rBtnEntradaDirectaPreaviso.Click += rBtnEntradaDirectaPreaviso_Click;

            rBtnEntradaDirectaPreavisoCarro.Image = global::RumboSGA.Properties.Resources.CarroMovil;
            rBtnEntradaDirectaPreavisoCarro.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnEntradaDirectaPreavisoCarro.Text = "Ent Directa Preaviso Carro";
            rBtnEntradaDirectaPreavisoCarro.TextAlignment = ContentAlignment.BottomCenter;
            rBtnEntradaDirectaPreavisoCarro.Click += rBtnEntradaDirectaPreavisoCarro_Click;

            /*cambio por menu itemrBtnEliminarEntrada.Image = global::RumboSGA.Properties.Resources.Delete;
            rBtnEliminarEntrada.ImageAlignment = ContentAlignment.MiddleCenter;
            rBtnEliminarEntrada.Text = "Eliminar entrada";
            rBtnEliminarEntrada.TextAlignment = ContentAlignment.BottomCenter;
            rBtnEliminarEntrada.Click += rBtnEliminarEntrada_Click;*/
            eliminarEntradaMenuItem.Name = "eliminarEntrada";
            eliminarEntradaMenuItem.Text = "Eliminar entrada";
            eliminarEntradaMenuItem.Image = global::RumboSGA.Properties.Resources.Delete;
            eliminarEntradaMenuItem.ImageAlignment = ContentAlignment.MiddleCenter;
            this.eliminarEntradaMenuItem.Click += new System.EventHandler(this.rBtnEliminarEntrada_Click);

            modificarEntradaMenuItem.Name = "modificarEntrada";
            modificarEntradaMenuItem.Text = "Modificar entrada";
            modificarEntradaMenuItem.Image = global::RumboSGA.Properties.Resources.edit;
            modificarEntradaMenuItem.ImageAlignment = ContentAlignment.MiddleCenter;
            modificarEntradaMenuItem.Click += new System.EventHandler(this.rBtnModificarEntrada_Click);

            modificarLineasMenuItem.Name = "modificarLineas";
            modificarLineasMenuItem.Text = "Modificar líneas";
            modificarLineasMenuItem.Image = global::RumboSGA.Properties.Resources.edit;
            modificarLineasMenuItem.ImageAlignment = ContentAlignment.MiddleCenter;
            modificarLineasMenuItem.Click += new System.EventHandler(this.rBtnModificarLineas_Click);

            AltaChepsMenuItem.Name = "Agregar Linea Cheps";
            AltaChepsMenuItem.Text = "Agregar Linea Cheps";
            AltaChepsMenuItem.Image = global::RumboSGA.Properties.Resources.edit;
            AltaChepsMenuItem.ImageAlignment = ContentAlignment.MiddleCenter;
            AltaChepsMenuItem.Click += new System.EventHandler(this.rBtnAgregarLineaCheps_Click);

            rDDBtnOpcionesRecepciones.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            rDDBtnOpcionesRecepciones.AutoSize = false;
            rDDBtnOpcionesRecepciones.Bounds = new System.Drawing.Rectangle(0, 0, 68, 62);
            rDDBtnOpcionesRecepciones.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            rDDBtnOpcionesRecepciones.ExpandArrowButton = false;
            rDDBtnOpcionesRecepciones.Image = global::RumboSGA.Properties.Resources.mostrarmas;
            rDDBtnOpcionesRecepciones.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            rDDBtnOpcionesRecepciones.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            rDDBtnOpcionesRecepciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            agregarEmbalajeMenuItem,
            modificarEmbalajeMenuItem,
            eliminarEmbalajeMenuItem,
            modificarEntradaMenuItem,
            eliminarEntradaMenuItem,
            modificarLineasMenuItem,
            AltaChepsMenuItem
            });
            rDDBtnOpcionesRecepciones.Name = "rDDBtnOpcionesRecepciones";
            rDDBtnOpcionesRecepciones.ToolTipText = "Otras";
            rDDBtnOpcionesRecepciones.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            rDDBtnOpcionesRecepciones.UseCompatibleTextRendering = false;

            grupoRecepciones.Text = "Acciones";
        }

        private void rBtnClasificacionEntradas_Click(object sender, EventArgs e)
        {
            DataTable recepcionesLineas = new DataTable();
            DataTable recepcionesClasificacion = new DataTable();

            DataTable recepciones = new DataTable();
            int recepcionesSelecionadas = 0;
            string idRecepcion = "";
            string nombreRecepcion = "";
            try
            {
                bool recepcionSeleccionada = false;
                int idEntrada = -1;

                foreach (GridViewRowInfo rowinfo in rgvRecepciones.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        recepcionesSelecionadas++;
                    }
                    if (recepcionesSelecionadas > 1)
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No puede haber mas de una recepcion seleccionada "));
                        break;
                    }
                }
                if (recepcionesSelecionadas == 1)
                {
                    foreach (GridViewDataColumn columnaLineas in rgvRecepciones.Columns)
                    {
                        recepciones.Columns.Add(columnaLineas.Name, typeof(String));
                    }
                    foreach (GridViewRowInfo rowinfo in rgvRecepciones.Rows)
                    {
                        if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                        {
                            //DataRow rowRecepciones = (DataRow)((DataRowView)rowinfo.DataBoundItem).Row;

                            DataRow rowRecepciones = (DataRow)((DataRowView)rowinfo.DataBoundItem).Row;
                            recepciones.ImportRow(rowRecepciones);

                            if (rowinfo.Cells[Lenguaje.traduce("Estado")].Value.ToString().Equals("PC"))
                            {
                                RadMessageBox.Show(Lenguaje.traduce("La recepción está cerrada"));
                                return;
                            }
                            recepcionSeleccionada = true;

                            idRecepcion = rowinfo.Cells[Lenguaje.traduce("ID")].Value.ToString();
                            nombreRecepcion = rowinfo.Cells[Lenguaje.traduce("Recepcion")].Value.ToString();
                            recepcionesLineas = Business.GetRecepcionesJerarquicoLineas(idRecepcion);
                        }
                    }
                }
                if (recepcionSeleccionada)
                {
                    recepcionesClasificacion = recepcionesLineas;
                    DataColumn newColumn = new DataColumn("recepcion", typeof(System.String));
                    newColumn.DefaultValue = nombreRecepcion;
                    recepcionesClasificacion.Columns.Add(newColumn);
                    string jsonClasificacionEntrada = string.Empty;
                    jsonClasificacionEntrada = JsonConvert.SerializeObject(recepcionesClasificacion);
                    clasificacionEntradasForm clasificacionEntradas = new clasificacionEntradasForm(jsonClasificacionEntrada);
                    clasificacionEntradas.Show();
                    //clasificacionEntradas clasificacionEntradas = new clasificacionEntradas(jsonClasificacionEntrada);
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la recepción"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnEliminarEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                bool recepcionSeleccionada = false;
                List<int> entradas = new List<int>();
                foreach (GridViewRowInfo rowinfo in rgvRecepciones.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        if (rowinfo.Cells[Lenguaje.traduce("Estado")].Value.ToString().Equals("PC"))
                        {
                            RadMessageBox.Show(Lenguaje.traduce("La recepción está cerrada"));
                            return;
                        }
                        recepcionSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Existencias))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    int idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID")].Value);
                                    entradas.Add(idEntrada);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (recepcionSeleccionada)
                {
                    if (entradas.Count > 0)
                    {
                        WSRecepcionMotorClient wsr = new WSRecepcionMotorClient();
                        wsr.eliminarEntradas(entradas.ToArray(), User.IdOperario);
                        jerarquicoTemplateActualizar(jerarquicoPosicionEntradasRec);
                        jerarquicoTemplateActualizar(jerarquicoPosicionStockRec);
                    }
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la recepción"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnModificarLineas_Click(object sender, EventArgs e)
        {
            try
            {
                if (recepcionesSeleccionadas.Count != 0)
                {
                    int id = recepcionesSeleccionadas[0];
                    FrmModificarLineasRecepcion frm = new FrmModificarLineasRecepcion(id);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        MessageBox.Show(Lenguaje.traduce("Recepción modificada correctamente"));
                    }
                    jerarquicoTemplateActualizar(jerarquicoPosicionLineasRec);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rBtnModificarEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                bool recepcionSeleccionada = false;
                int idEntrada = -1;
                foreach (GridViewRowInfo rowinfo in rgvRecepciones.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        if (rowinfo.Cells[Lenguaje.traduce("Estado")].Value.ToString().Equals("PC"))
                        {
                            RadMessageBox.Show(Lenguaje.traduce("La recepción está cerrada"));
                            return;
                        }
                        recepcionSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Existencias))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID")].Value);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (recepcionSeleccionada)
                {
                    if (idEntrada > 0)
                    {
                        ModificarEntrada frm = new ModificarEntrada(idEntrada);
                        frm.ShowDialog();
                        jerarquicoTemplateActualizar(jerarquicoPosicionEntradasRec);
                        jerarquicoTemplateActualizar(jerarquicoPosicionStockRec);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar una matrícula"));
                    }
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la recepción"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnEntradaDirectaPreaviso_Click(object sender, EventArgs e)
        {
            try
            {
                if (recepcionesSeleccionadas.Count != 0)
                {
                    int id = recepcionesSeleccionadas[0];
                    FrmAltaDirectaPreaviso frm = new FrmAltaDirectaPreaviso(id);
                    frm.ShowDialog();
                    jerarquicoTemplateActualizar(jerarquicoPosicionEntradasRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionStockRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionMovimientosRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionLineasRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionTareasRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionAsnsRec);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void rBtnEntradaDirectaPreavisoCarro_Click(object sender, EventArgs e)
        {
            try
            {
                if (recepcionesSeleccionadas.Count != 0)
                {
                    int id = recepcionesSeleccionadas[0];
                    FrmAltaDirectaPreavisoCarro frm = new FrmAltaDirectaPreavisoCarro(id);
                    frm.ShowDialog();
                    jerarquicoTemplateActualizar(jerarquicoPosicionEntradasRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionStockRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionMovimientosRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionLineasRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionTareasRec);
                    jerarquicoTemplateActualizar(jerarquicoPosicionAsnsRec);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void rBtnEliminarEmbalaje_Click(object sender, EventArgs e)
        {
            EliminarEmbalaje();
        }

        private void EliminarEmbalaje()
        {
            try
            {
                WSEmbalajeMotorClient embalajeMotor = new WSEmbalajeMotorClient();
                if (embalajeSeleccionado > 0)
                {
                    embalajeMotor.eliminarMovimientoEmbalaje(embalajeSeleccionado, User.IdOperario);
                    jerarquicoTemplateActualizar(jerarquicoPosicionEmbalajeRec);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnModificarEmbalaje_Click(object sender, EventArgs e)
        {
            try
            {
                if (recepcionesSeleccionadas.Count != 0)
                {
                    int id = recepcionesSeleccionadas[0];
                    if (embalajeSeleccionado > 0)
                    {
                        FrmAltaModEmbalaje frm = new FrmAltaModEmbalaje(id, embalajeSeleccionado);
                        frm.ShowDialog();
                        jerarquicoTemplateActualizar(jerarquicoPosicionEmbalajeRec);
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce("Debes seleccionar un embalaje"));
                    }
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rBtnAgregarEmbalaje_Click(object sender, EventArgs e)
        {
            try
            {
                if (recepcionesSeleccionadas.Count != 0)
                {
                    int id = recepcionesSeleccionadas[0];
                    FrmAltaModEmbalaje frm = new FrmAltaModEmbalaje(id, 0);
                    frm.ShowDialog();
                    jerarquicoTemplateActualizar(jerarquicoPosicionEmbalajeRec);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rBtnAgregarLineaCheps_Click(object sender, EventArgs e)
        {
            try
            {
                if (recepcionesSeleccionadas.Count != 0)
                {
                    int id = recepcionesSeleccionadas[0];
                    FrmAltaChep frm = new FrmAltaChep(id, 0);
                    frm.ShowDialog();
                    jerarquicoTemplateActualizar(jerarquicoPosicionEmbalajeRec);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una recepción"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void jerarquicoTemplateActualizar(int templateAActualizar)
        {
            this.rgvRecepciones.MasterTemplate.Templates[templateAActualizar].Refresh();
        }

        private void CambiarBotonesPedidos()
        {
            grupoRecepciones.Items.Remove(rBtnEditarRecepcion);
            grupoRecepciones.Items.Remove(rBtncerrarRecepcion);
            grupoRecepciones.Items.Remove(rBtnClasificacionEntradas);
            grupoRecepciones.Items.Remove(rBtnGenerarTarea);
            grupoRecepciones.Items.Remove(rBtnAltaPalet);
            //grupoRecepciones.Items.Remove(rBtnAgregarEmbalaje);
            //grupoRecepciones.Items.Remove(rBtnModificarEmbalaje);
            //grupoRecepciones.Items.Remove(rBtnEliminarEmbalaje);
            grupoRecepciones.Items.Remove(rBtnEntradaDirectaPreaviso);
            grupoRecepciones.Items.Remove(rBtnEntradaDirectaPreavisoCarro);
            //grupoRecepciones.Items.Remove(rBtnEliminarEntrada);
            grupoRecepciones.Items.Remove(rBtnAltaPalet);
            grupoRecepciones.Items.Remove(rBtnAltaPico);
            grupoRecepciones.Items.Remove(rBtnAltaMulti);
            grupoRecepciones.Items.Remove(rBtnAltaCarro);
            grupoRecepciones.Items.Remove(rBtnAltaTotales);
            grupoRecepciones.Items.Remove(rDDBtnOpcionesRecepciones);
            grupoRecepciones.Items.Add(rBtnCrearRecepcion);
            grupoRecepciones.Items.Add(rBtnCerrarPedido);
        }

        private void CambiarBotonesRecepciones()
        {
            grupoRecepciones.Items.Remove(rBtnCrearRecepcion);
            grupoRecepciones.Items.Remove(rBtnCerrarPedido);
            grupoRecepciones.Items.Remove(rBtnRecepcionParcial);
            grupoRecepciones.Items.Add(rBtnAltaPalet);
            grupoRecepciones.Items.Add(rBtnAltaPico);
            grupoRecepciones.Items.Add(rBtnAltaMulti);
            grupoRecepciones.Items.Add(rBtnAltaCarro);
            grupoRecepciones.Items.Add(rBtnAltaTotales);
            grupoRecepciones.Items.Add(rBtnClasificacionEntradas);
            //grupoRecepciones.Items.Add(rBtnEditarRecepcion);
            grupoRecepciones.Items.Add(rBtnGenerarTarea);
            //grupoRecepciones.Items.Add(rBtnAgregarEmbalaje);
            //grupoRecepciones.Items.Add(rBtnModificarEmbalaje);
            //grupoRecepciones.Items.Add(rBtnEliminarEmbalaje);
            grupoRecepciones.Items.Add(rBtnEntradaDirectaPreaviso);
            grupoRecepciones.Items.Add(rBtnEntradaDirectaPreavisoCarro);
            //grupoRecepciones.Items.Add(rBtnEliminarEntrada);
            grupoRecepciones.Items.Add(rBtncerrarRecepcion);
            grupoRecepciones.Items.Add(rDDBtnOpcionesRecepciones);
            Permiso(eliminarEntradaMenuItem, 500091);
            Permiso(modificarEntradaMenuItem, 500092);
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

        #endregion Botones

        #region Color Estado

        private void ColorEstadoPedidos()
        {
            if (this.rgvPedidos.MasterTemplate.Columns[Lenguaje.traduce("Estado")] != null)
            {
                try
                {
                    ConditionalFormattingObject obj1 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "BQ", "", false);
                    obj1.CellBackColor = XmlReaderPropio.getColorPedidos("BQ");
                    ConditionalFormattingObject obj2 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "PC", "", false);
                    obj2.CellBackColor = XmlReaderPropio.getColorPedidos("PC");
                    ConditionalFormattingObject obj3 = new ConditionalFormattingObject("Mi Condición3", ConditionTypes.Equal, "PE", "", false);
                    obj3.CellBackColor = XmlReaderPropio.getColorPedidos("PE");
                    ConditionalFormattingObject obj4 = new ConditionalFormattingObject("Mi Condición4", ConditionTypes.Equal, "PR", "", false);
                    obj4.CellBackColor = XmlReaderPropio.getColorPedidos("PR");
                    ConditionalFormattingObject obj5 = new ConditionalFormattingObject("Mi Condición5", ConditionTypes.Equal, "PU", "", false);
                    obj5.CellBackColor = XmlReaderPropio.getColorPedidos("PU");
                    this.rgvPedidos.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj1);
                    this.rgvPedidos.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj2);
                    this.rgvPedidos.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj3);
                    this.rgvPedidos.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj4);
                    this.rgvPedidos.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj5);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
        }

        private void ColorEstadoRecepciones()
        {
            if (this.rgvPedidos.MasterTemplate.Columns[Lenguaje.traduce("Estado")] != null)
            {
                try
                {
                    ConditionalFormattingObject obj1 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "BQ", "", false);
                    obj1.CellBackColor = XmlReaderPropio.getColorPedidos("BQ");
                    ConditionalFormattingObject obj2 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "PC", "", false);
                    obj2.CellBackColor = XmlReaderPropio.getColorPedidos("PC");
                    ConditionalFormattingObject obj3 = new ConditionalFormattingObject("Mi Condición3", ConditionTypes.Equal, "PE", "", false);
                    obj3.CellBackColor = XmlReaderPropio.getColorPedidos("PE");
                    ConditionalFormattingObject obj4 = new ConditionalFormattingObject("Mi Condición4", ConditionTypes.Equal, "PR", "", false);
                    obj4.CellBackColor = XmlReaderPropio.getColorPedidos("PR");
                    ConditionalFormattingObject obj5 = new ConditionalFormattingObject("Mi Condición5", ConditionTypes.Equal, "PU", "", false);
                    obj5.CellBackColor = XmlReaderPropio.getColorPedidos("PU");

                    this.rgvRecepciones.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj1);
                    this.rgvRecepciones.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj2);
                    this.rgvRecepciones.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj3);
                    this.rgvRecepciones.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj4);
                    this.rgvRecepciones.MasterTemplate.Columns[Lenguaje.traduce("Estado")].ConditionalFormattingObjectList.Add(obj5);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
        }

        #endregion Color Estado

        #region Eventos Propios

        private void CambiarFormatoFecha(object sender, EventArgs e)
        {
            GridViewDateTimeColumn columnaFecha, columnaFecha2;
            if (rgvPedidos.Columns[Lenguaje.traduce("Fecha Recepcion")] is GridViewDateTimeColumn
                && rgvPedidos.Columns[Lenguaje.traduce("Fecha Prevista")] is GridViewDateTimeColumn)
            {
                columnaFecha = (GridViewDateTimeColumn)rgvPedidos.Columns[Lenguaje.traduce("Fecha Recepcion")];
                columnaFecha2 = (GridViewDateTimeColumn)rgvPedidos.Columns[Lenguaje.traduce("Fecha Prevista")];

                RadListDataItem item = (sender as RadDropDownListElement).SelectedItem as RadListDataItem;
                switch (item.Tag)
                {
                    case 0:
                        columnaFecha.FormatString = "{0:g}";
                        columnaFecha2.FormatString = "{0:g}";

                        break;

                    case 1:
                        columnaFecha.FormatString = "{0:d}";
                        columnaFecha2.FormatString = "{0:d}";

                        break;
                }

                rgvPedidos.Refresh();
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce("Error cambiando el formato. ¿Puede ser que sea necesario reiniciar el estilo?"));
                log.Error("Error: Fecha recepcion o Fecha Prevista no es formato GridViewDateTimeColumn");
            }
        }

        private void AñadirColumnas()
        {
            //RumTextBoxColumn textBoxColumn = new RumTextBoxColumn();
            //RumTextBoxColumn textBoxColumnRecepcionesCount = new RumTextBoxColumn();
            //RumTextBoxColumn textBoxColumnPalets = new RumTextBoxColumn();
            //RumTextBoxColumn textBoxColumnUdsSueltas = new RumTextBoxColumn();
            //DataTable lineas = ConexionSQL.getDataTable("SELECT * FROM TBLPEDIDOSPROLIN");

            //textBoxColumn.Name = colLineas;
            //textBoxColumn.HeaderText = colLineas;
            //rgvPedidos.Columns.Add(textBoxColumn);

            //textBoxColumnRecepcionesCount.Name = colNRecepciones;
            //textBoxColumnRecepcionesCount.HeaderText = colNRecepciones;
            //rgvPedidos.Columns.Add(textBoxColumnRecepcionesCount);

            //textBoxColumnPalets.Name = colPaletsCompletos;
            //textBoxColumnPalets.HeaderText = colPaletsCompletos;
            //rgvPedidos.Columns.Add(textBoxColumnPalets);

            //textBoxColumnUdsSueltas.Name = colUnidadesSueltas;
            //textBoxColumnUdsSueltas.HeaderText = colUnidadesSueltas;
            //rgvPedidos.Columns.Add(textBoxColumnUdsSueltas); ;

            /* MONICA LO PONGO AL INICIOcolPedidosPorcentLineas = new ProgressBarColumn(Lenguaje.traduce("PorcentLineas"));
            colPedidosPorcentLineas.HeaderText =Lenguaje.traduce("PorcentLineas");
            colPedidosPorcentLineas.Width = 100;*/

            this.rgvPedidos.Columns.Add(colPedidosPorcentLineas);

            //try
            //{
            //    foreach (var row in rgvPedidos.Rows)
            //    {
            //        log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Nueva linea AñadirColumnas rgvPedidos.Rows");
            //        string id = row.Cells[colID].Value.ToString();
            //        //int valor = dtPedidosLineas.Select("IDPEDIDOPRO=" + id).Count();
            //        //if (dtColPaletsCompletos.Select("IDPEDIDOPRO=" + id).Count() != 0)
            //        //{
            //        //    DataRow rowPalets = dtColPaletsCompletos.Select("IDPEDIDOPRO=" + id)[0];
            //        //    row.Cells[colPaletsCompletos].Value = rowPalets["PALETSCOMPLETOS"];
            //        //}
            //        //else
            //        //{
            //        //    row.Cells[colPaletsCompletos].Value = 0;

            //        //}
            //        //if (dtColUSDSueltas.Select("IDPEDIDOPRO=" + id).Count() != 0)
            //        //{
            //        //    DataRow rowPalets = dtColUSDSueltas.Select("IDPEDIDOPRO=" + id)[0];
            //        //    row.Cells[colUnidadesSueltas].Value = rowPalets["UNIDADESUELTAS"];
            //        //}
            //        //else
            //        //{
            //        //    row.Cells[colUnidadesSueltas].Value = 0;
            //        //}
            //        //if (lineas.Select("IDPEDIDOPRO=" + id) != null)
            //        //{
            //        //    row.Cells[colLineas].Value = lineas.Select("IDPEDIDOPRO=" + id).Count();
            //        //}
            //        //else
            //        //{
            //        //    row.Cells[colLineas].Value = 0;

            //        //}
            //        //row.Cells[colLineas].Value = valor;
            //        //DataTable countRecepciones = Business.GetPedidosProveedorJerarquicoRecepcionesCantidad(_lstEsquemaTablaRecepciones,_lstEsquemaTablaRecepcionesUnion, id);
            //        //if (countRecepciones!=null&&countRecepciones.Rows[0][0].ToString()!=string.Empty)
            //        //{
            //        //    row.Cells[colNRecepciones].Value = countRecepciones.Rows[0][0].ToString();
            //        //}
            //        //else
            //        //{
            //        //    row.Cells[colNRecepciones].Value = 0;
            //        //}

            //    }
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e.StackTrace);
            //}
        }

        private void AñadirValorProgressBar()
        {
            try
            {
                int value = 0;

                int i = 0;
                rgvPedidos.Columns[Lenguaje.traduce("Porcentaje Lineas")].IsVisible = false;
                rgvPedidos.Columns[Lenguaje.traduce("Porcentaje Lineas")].VisibleInColumnChooser = false;
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio  añadir valor progress bar");
                foreach (GridViewRowInfo row in rgvPedidos.Rows)
                {
                    //DataRow[] rows = dtPorcentLineas.Select("IDPEDIDOPRO=" + row.Cells["ID"].Value.ToString());

                    //if (rows.Count() > 0)
                    //{
                    int.TryParse(row.Cells[Lenguaje.traduce("Porcentaje Lineas")].Value.ToString(), out value);
                    row.Cells[colPorcentLineas].Value = value;

                    //}
                    //else
                    //{
                    //    row.Cells[colPorcentLineas].Value = 0;

                    //}
                    i++;
                }
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin  añadir valor progress bar");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        protected void rgvPedidos_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                GridDataView dataView = rgvPedidos.MasterTemplate.DataView as GridDataView;
                lblNumRegistros.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                /* }
                 else
                 {
                     GridDataView dataView = rgvPedidos.MasterTemplate.DataView as GridDataView;
                     lblNumRegistros.Text = "Records: " + dataView.Indexer.Items.Count;
                 }*/
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        /* MONICA NO SE HACE REFERERNCIA A ESTA FUNCION private void GridViewControl_SelectionChanged(object sender, EventArgs e)
        {
            selectedRow = null;
            if (this.radGridViewControl.CurrentCell != null && this.radGridViewControl.CurrentCell.RowIndex >= 0)
            {
                if ((radGridViewControl.CurrentCell.RowIndex % radGridViewControl.PageSize) >= data.Count)
                {
                    return;
                }
                int index = this.radGridViewControl.CurrentCell.RowIndex - this.radGridViewControl.PageSize * this.radGridViewControl.MasterTemplate.PageIndex;
                var item = data[index];
                selectedRow = item; //Fila seleccionada, en formato JSON
            }
        }*/

        //void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        //{
        //    //try
        //    //{
        //    //    if (e.RowIndex > -1 && e.ColumnIndex > -1)
        //    //    {
        //    //        e.CellElement.TextWrap = true;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
        //    //}
        //}
        //void radGridView1_CellFormattingWrapText(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        //{
        //    //try
        //    //{
        //    //    if (e.CellElement is GridHeaderCellElement)
        //    //    {
        //    //        e.CellElement.TextWrap = true;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
        //    //}
        //}
        private void rgvPedidos_ValueChanged(object sender, EventArgs e)
        {
            if (rgvPedidos.CurrentRow.HierarchyLevel == 0)
            {
                int id;
                if (this.rgvPedidos.ActiveEditor is RadCheckBoxEditor)
                {
                    if (rgvPedidos.ActiveEditor.Value.ToString() == "On")
                    {
                        if (rgvPedidos.CurrentRow.Cells[colID].Value == null) return;
                        id = int.Parse(rgvPedidos.CurrentRow.Cells[colID].Value.ToString());
                        if (!(pedidosSeleccionados.Contains(id)))
                        {
                            pedidosFilaSeleccionados.Add(rgvPedidos.CurrentRow);
                            pedidosSeleccionados.Add(int.Parse(rgvPedidos.CurrentRow.Cells[colID].Value.ToString()));
                        }
                    }
                    else
                    {
                        id = int.Parse(rgvPedidos.CurrentRow.Cells["ID"].Value.ToString());
                        if (pedidosSeleccionados.Contains(id))
                        {
                            pedidosFilaSeleccionados.Remove(rgvPedidos.CurrentRow);

                            pedidosSeleccionados.Remove(id);
                        }
                    }
                    /*MONICA lo quito. Es de depuracionint i = 0;
                    foreach (var item in seleccionads)
                    {
                        Console.WriteLine(seleccionads[i]);
                        i++;
                    }*/
                }
            }
        }

        private void rgvRecepciones_ValueChanged(object sender, EventArgs e)
        {
            int id;
            if (rgvRecepciones.MasterTemplate.CurrentRow.Cells["ID"] == null ||
                rgvRecepciones.MasterTemplate.CurrentRow.Cells["ID"].Value == null)
                return;
            id = int.Parse(rgvRecepciones.MasterTemplate.CurrentRow.Cells["ID"].Value.ToString());
            if (this.rgvRecepciones.ActiveEditor is RadCheckBoxEditor)
            {
                if (rgvRecepciones.ActiveEditor.Value.ToString() == "On")
                {
                    if (!(recepcionesSeleccionadas.Contains(id)))
                    {
                        recepcionesSeleccionadas.Add(id);
                    }
                }
                else
                {
                    if (recepcionesSeleccionadas.Contains(id))
                    {
                        recepcionesSeleccionadas.Remove(id);
                    }
                }
                //int i = 0;
                //foreach (var item in recepcionesSeleccionadas)
                //{
                //    Console.WriteLine(recepcionesSeleccionadas[i]);
                //    i++;
                //}
            }
        }

        private void ConfigurarBarraProgresoGridView()
        {
            this.waitingBar.Name = "radWaitingBar1";
            this.waitingBar.Size = new System.Drawing.Size(200, 20);
            this.waitingBar.TabIndex = 2;
            this.waitingBar.Text = "radWaitingBar1";
            this.waitingBar.AssociatedControl = this.rgvPedidos;
            this.Controls.Add(waitingBar);
        }

        #endregion Eventos Propios

        #region Estilos

        public void SaveLayoutGlobal()
        {
            int indexColumnPorcentLinea;
            int indexColumnPorcentEntrada;
            int indexColumnPorcentCantPte;
            string path = Persistencia.DirectorioGlobal;/*XmlReaderPropio.getLayoutPath(0);*/
            int pathGL = 0;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tlPrincipal.GetControlFromPosition(0, 0).Name == "pedidos")
                {
                    rgvPedidos.BeginUpdate();
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + nombreEstiloPedidos;
                    path.Replace(" ", "_");

                    indexColumnPorcentLinea = Convert.ToInt32(rgvPedidos.Columns[colPorcentLineas].Index);
                    indexColumnPorcentEntrada = Convert.ToInt32(gtJerarquicoPedidosLineasPedidos.Columns[colCantidadEntrada].Index);
                    indexColumnPorcentCantPte = Convert.ToInt32(gtJerarquicoPedidosLineasPedidos.Columns[colCantidadPteUbicar].Index);
                    if (indexColumnPorcentEntrada < indexColumnPorcentCantPte)
                    {
                        //indexColumnPorcentEntrada = indexColumnPorcentEntrada + 1;
                    }
                    else
                    {
                        indexColumnPorcentEntrada = indexColumnPorcentEntrada - 1;
                    }
                    XmlReaderPropio.setPorcentLineasPedProCab(indexColumnPorcentLinea, pathGL);
                    XmlReaderPropio.setPorcentEntradas(indexColumnPorcentEntrada, pathGL);
                    XmlReaderPropio.setPorcentEntradasPendientes(indexColumnPorcentCantPte, pathGL);

                    this.rgvPedidos.Columns.Remove(colPedidosPorcentLineas);
                    gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidosPorcentEnt);
                    gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidoPorcentEntPend);

                    rgvPedidos.SaveLayout(path);
                    this.rgvPedidos.Columns.Add(colPedidosPorcentLineas);
                    this.rgvPedidos.Columns.Move(rgvPedidos.Columns.Count - 1, indexColumnPorcentLinea);

                    gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidosPorcentEnt);
                    if (indexColumnPorcentEntrada != gtJerarquicoPedidosLineasPedidos.Columns.Count)
                    {
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidosPorcentEnt.Index, indexColumnPorcentEntrada);
                    }

                    gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidoPorcentEntPend);
                    if (indexColumnPorcentCantPte != gtJerarquicoPedidosLineasPedidos.Columns.Count)
                    {
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidoPorcentEntPend.Index, indexColumnPorcentCantPte);
                    }
                    rgvPedidos.EndUpdate();
                }
                else
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + nombreEstiloRecepciones;
                    path.Replace(" ", "_");
                    rgvRecepciones.SaveLayout(path);
                    log.Debug("Se ha guardado el layout rgvRecepciones en " + path);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce("No se ha podido encontrar el archivo en la dirección") + ":" + path + "\n" + Lenguaje.traduce("Puede cambiar esta dirección en el archivo PathLayouts.xml"));
            }
        }

        public void SaveLayoutLocal()
        {
            int indexColumnPorcentLinea;
            int indexColumnPorcentEntrada;
            int indexColumnPorcentCantPte;
            string path = Persistencia.DirectorioLocal;/*XmlReaderPropio.getLayoutPath(1);*/
            int pathGL = 1;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tlPrincipal.GetControlFromPosition(0, 0).Name == "pedidos")
                {
                    rgvPedidos.BeginUpdate();
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + nombreEstiloPedidos;
                    path.Replace(" ", "_");

                    indexColumnPorcentLinea = Convert.ToInt32(rgvPedidos.Columns[colPorcentLineas].Index);
                    indexColumnPorcentEntrada = Convert.ToInt32(gtJerarquicoPedidosLineasPedidos.Columns[colCantidadEntrada].Index);
                    indexColumnPorcentCantPte = Convert.ToInt32(gtJerarquicoPedidosLineasPedidos.Columns[colCantidadPteUbicar].Index);
                    if (indexColumnPorcentEntrada < indexColumnPorcentCantPte)
                    {
                        //indexColumnPorcentEntrada = indexColumnPorcentEntrada + 1;
                    }
                    else
                    {
                        indexColumnPorcentEntrada = indexColumnPorcentEntrada - 1;
                    }
                    XmlReaderPropio.setPorcentLineasPedProCab(indexColumnPorcentLinea, pathGL);
                    XmlReaderPropio.setPorcentEntradas(indexColumnPorcentEntrada, pathGL);
                    XmlReaderPropio.setPorcentEntradasPendientes(indexColumnPorcentCantPte, pathGL);

                    this.rgvPedidos.Columns.Remove(colPedidosPorcentLineas);
                    gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidosPorcentEnt);
                    gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidoPorcentEntPend);

                    rgvPedidos.SaveLayout(path);
                    this.rgvPedidos.Columns.Add(colPedidosPorcentLineas);
                    this.rgvPedidos.Columns.Move(rgvPedidos.Columns.Count - 1, indexColumnPorcentLinea);
                    gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidosPorcentEnt);
                    if (indexColumnPorcentEntrada != gtJerarquicoPedidosLineasPedidos.Columns.Count)
                    {
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidosPorcentEnt.Index, indexColumnPorcentEntrada);
                    }

                    gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidoPorcentEntPend);
                    if (indexColumnPorcentCantPte != gtJerarquicoPedidosLineasPedidos.Columns.Count)
                    {
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidoPorcentEntPend.Index, indexColumnPorcentCantPte);
                    }
                    rgvPedidos.EndUpdate();
                }
                else
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + nombreEstiloRecepciones;
                    path.Replace(" ", "_");
                    rgvRecepciones.SaveLayout(path);
                    log.Debug("Se ha guardado el layout rgvRecepciones en " + path);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void LoadLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;
            int pathGL = 0;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tlPrincipal.GetControlFromPosition(0, 0).Name == "pedidos")
                {
                    string s = path + "\\" + this.Name + nombreEstiloPedidos;
                    if (File.Exists(s))
                    {
                        rgvPedidos.BeginUpdate();
                        s.Replace(" ", "_");
                        //Monica ¿pòrque?rgvPedidos.RowSourceNeeded -= pedidos_RowSourceNeeded;

                        this.rgvPedidos.Columns.Remove(colPedidosPorcentLineas);
                        gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidosPorcentEnt);
                        gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidoPorcentEntPend);

                        rgvPedidos.LoadLayout(s);
                        this.rgvPedidos.Columns.Add(colPedidosPorcentLineas);
                        gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidosPorcentEnt);
                        gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidoPorcentEntPend);
                        rgvPedidos.Columns.Move(colPedidosPorcentLineas.Index, XmlReaderPropio.getPorcentLineasPedProCab(pathGL));
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidosPorcentEnt.Index, XmlReaderPropio.getPorcentEntradas(pathGL));
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidoPorcentEntPend.Index, XmlReaderPropio.getPorcentEntradasPendientes(pathGL));
                        //Monica ¿pòrque?rgvPedidos.RowSourceNeeded += pedidos_RowSourceNeeded;
                        gtJerarquicoPedidosLineasPedidos.Refresh();
                        gtJerarquicoPedidosPreavisos.Refresh();
                        gtJerarquicoPedidosRecepciones.Refresh();
                        gtJerarquicoPedidosEntradas.Refresh();

                        colPedidosPorcentLineas.Width = 100;
                        this.rgvPedidos.TableElement.RowHeight = 40;
                        rgvPedidos.EndUpdate();
                    }
                }
                else
                {
                    string s = path + "\\" + this.Name + nombreEstiloRecepciones;
                    s.Replace(" ", "_");
                    rgvRecepciones.LoadLayout(s);
                    log.Debug("Se ha cargado el layout rgvRecepciones desde " + path);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void LoadLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal;
            int pathGL = 1;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tlPrincipal.GetControlFromPosition(0, 0).Name == "pedidos")
                {
                    string s = path + "\\" + this.Name + nombreEstiloPedidos;
                    rgvPedidos.BeginUpdate();
                    s.Replace(" ", "_");
                    this.rgvPedidos.Columns.Remove(colPedidosPorcentLineas);
                    gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidosPorcentEnt);
                    gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidoPorcentEntPend);
                    //Monica ¿pòrque?rgvPedidos.RowSourceNeeded -= pedidos_RowSourceNeeded ;
                    rgvPedidos.LoadLayout(s);
                    this.rgvPedidos.Columns.Add(colPedidosPorcentLineas);
                    gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidosPorcentEnt);
                    gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidoPorcentEntPend);
                    rgvPedidos.Columns.Move(colPedidosPorcentLineas.Index, XmlReaderPropio.getPorcentLineasPedProCab(pathGL));
                    gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidosPorcentEnt.Index, XmlReaderPropio.getPorcentEntradas(pathGL));
                    gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidoPorcentEntPend.Index, XmlReaderPropio.getPorcentEntradasPendientes(pathGL));
                    //Monica ¿pòrque?rgvPedidos.RowSourceNeeded += pedidos_RowSourceNeeded ;
                    gtJerarquicoPedidosLineasPedidos.Refresh();
                    gtJerarquicoPedidosPreavisos.Refresh();
                    gtJerarquicoPedidosRecepciones.Refresh();
                    gtJerarquicoPedidosEntradas.Refresh();
                    rgvPedidos.EndUpdate();
                    colPedidosPorcentLineas.Width = 100;
                    this.rgvPedidos.TableElement.RowHeight = 40;
                }
                else
                {
                    string s = path + "\\" + this.Name + nombreEstiloRecepciones;

                    s.Replace(" ", "_");
                    rgvRecepciones.LoadLayout(s);
                    log.Debug("Se ha cargado el layout rgvRecepciones desde " + path);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(strings.NoEncuentroRuta + ":" + path + "\n" + strings.CambiarPath);
            }
        }

        public void ElegirEstilo()
        {
            try
            {
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio  elegir estilo");
                string pathLocal = Persistencia.DirectorioLocal;/*XmlReaderPropio.getLayoutPath(1);*/
                string pathGlobal = Persistencia.DirectorioGlobal;/*XmlReaderPropio.getLayoutPath(0);*/
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                {
                    pathLocal += "\\Español";
                    pathGlobal += "\\Español";
                }
                else
                {
                    pathLocal += "\\Ingles";
                    pathGlobal += "\\Ingles";
                }
                if (tlPrincipal.GetControlFromPosition(0, 0).Name == "pedidos")
                {
                    OcultarColumnas(); ;
                    string pathGridView = pathLocal + "\\" + this.Name + nombreEstiloPedidos;
                    bool existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        int pathGL = 1;
                        //Monica ¿pòrque?rgvPedidos.RowSourceNeeded -= pedidos_RowSourceNeeded;
                        this.rgvPedidos.Columns.Remove(colPedidosPorcentLineas);
                        gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidosPorcentEnt);
                        gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidoPorcentEntPend);
                        rgvPedidos.LoadLayout(pathGridView);
                        this.rgvPedidos.Columns.Add(colPedidosPorcentLineas);
                        gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidosPorcentEnt);
                        gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidoPorcentEntPend);
                        rgvPedidos.Columns.Move(colPedidosPorcentLineas.Index, XmlReaderPropio.getPorcentLineasPedProCab(pathGL));
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidosPorcentEnt.Index, XmlReaderPropio.getPorcentEntradas(pathGL));
                        gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidoPorcentEntPend.Index, XmlReaderPropio.getPorcentEntradasPendientes(pathGL));
                        //Monica ¿pòrque?rgvPedidos.RowSourceNeeded += pedidos_RowSourceNeeded;

                        this.rgvPedidos.TableElement.RowHeight = 40;
                    }
                    else
                    {
                        pathGridView = pathGlobal + "\\" + this.Name + nombreEstiloPedidos;
                        existsGridView = File.Exists(pathGridView);
                        if (existsGridView)
                        {
                            int pathGL = 0;
                            //Monica ¿pòrque?rgvPedidos.RowSourceNeeded -= pedidos_RowSourceNeeded;
                            this.rgvPedidos.Columns.Remove(colPedidosPorcentLineas);
                            gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidosPorcentEnt);
                            gtJerarquicoPedidosLineasPedidos.Columns.Remove(colJerLineasPedidoPorcentEntPend);

                            rgvPedidos.LoadLayout(pathGridView);
                            this.rgvPedidos.Columns.Add(colPedidosPorcentLineas);
                            gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidosPorcentEnt);
                            gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidoPorcentEntPend);
                            rgvPedidos.Columns.Move(colPedidosPorcentLineas.Index, XmlReaderPropio.getPorcentLineasPedProCab(pathGL));
                            gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidosPorcentEnt.Index, XmlReaderPropio.getPorcentEntradas(pathGL));
                            gtJerarquicoPedidosLineasPedidos.Columns.Move(colJerLineasPedidoPorcentEntPend.Index, XmlReaderPropio.getPorcentEntradasPendientes(pathGL));
                            //Monica ¿pòrque? rgvPedidos.RowSourceNeeded += pedidos_RowSourceNeeded;

                            this.rgvPedidos.TableElement.RowHeight = 40;
                        }
                        else
                        {
                            rgvPedidos.BestFitColumns();
                        }
                    }
                }
                else
                {
                    string pathGridView = pathLocal + "\\" + this.Name + nombreEstiloRecepciones;
                    bool existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        rgvRecepciones.LoadLayout(pathGridView);
                        this.rgvPedidos.TableElement.RowHeight = 40;
                        ColorEstadoRecepciones();
                    }
                    else
                    {
                        pathGridView = pathGlobal + "\\" + this.Name + nombreEstiloRecepciones;
                        existsGridView = File.Exists(pathGridView);
                        if (existsGridView)
                        {
                            rgvRecepciones.LoadLayout(pathGridView);
                            this.rgvRecepciones.TableElement.RowHeight = 40;
                            ColorEstadoRecepciones();
                        }
                        else
                        {
                            rgvRecepciones.BestFitColumns();
                            ColorEstadoRecepciones();
                        }
                    }
                }
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin  elegir estilo");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion Estilos

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
        }

        private void AddThemeItemToThemesDropDownList(string themeName, Image image)
        {
            RadMenuItem mainItem = rDDBtnConfiguracion.Items[0] as RadMenuItem;
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(temasItem_Click);
            mainItem.Items.Add(temasItem);
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

        #endregion Temas

        private void LlamarWSCerrarRecepcion(string json)
        {
            WSRecepcionMotorClient webservicerecepcion = new WSRecepcionMotorClient();
            json = "[" + json + "]";
            string respuesta = webservicerecepcion.cerrarRecepcionesMultiple(json, DatosThread.getInstancia().getArrayDatos());
            Debug.WriteLine(respuesta);
        }

        private string FormarJSON(int idrecepcion)
        {
            dynamic objDinamico = new ExpandoObject();
            DataTable prueba = ConexionSQL.getDataTable("SELECT IDRECEPCION,IDPEDIDOPRO,IDRECEPCIONESTADO FROM TBLRECEPCIONESCAB WHERE IDRECEPCION=" + idrecepcion);
            if (prueba.Rows[0]["IDRECEPCIONESTADO"].ToString() != "PC")
            {
                objDinamico.tipo = "A";
                objDinamico.idrecepcion = idrecepcion;
                objDinamico.idpedidopro = prueba.Rows[0]["IDPEDIDOPRO"];
                objDinamico.idoperario = User.IdOperario;
                objDinamico.idrecurso = 0;
                objDinamico.error = "";
                string json = JsonConvert.SerializeObject(objDinamico);
                Debug.WriteLine(json);
                return json;
            }
            else
            {
                return null;
            }
        }

        #region ProgressBarColumn

        public class ProgressBarCellElement : GridDataCellElement
        {
            private RadProgressBarElement radProgressBarElement;

            public ProgressBarCellElement(GridViewColumn column, GridRowElement row) : base(column, row)
            {
                radProgressBarElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }

            protected override void CreateChildElements()
            {
                try
                {
                    base.CreateChildElements();
                    radProgressBarElement = new RadProgressBarElement();
                    this.Children.Add(radProgressBarElement);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }

            protected override void SetContentCore(object value)
            {
                try
                {
                    if (this.Value != null && this.Value != DBNull.Value)
                    {
                        int valor = 0;
                        this.radProgressBarElement.Maximum = 100;
                        radProgressBarElement.Minimum = 0;
                        if (Convert.ToDecimal(this.Value) >= 100)
                        {
                            valor = 100;
                            radProgressBarElement.Value1 = valor;
                            this.radProgressBarElement.Text = valor + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                        else if (Convert.ToDecimal(this.Value) <= 0)
                        {
                            valor = 0;
                            radProgressBarElement.Value1 = valor;
                            this.radProgressBarElement.Text = valor + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                        else
                        {
                            this.radProgressBarElement.Value1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Value), 0));/*Convert.ToInt16(this.Value);*/
                            this.radProgressBarElement.Text = this.radProgressBarElement.Value1 + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }

            protected override Type ThemeEffectiveType
            {
                get
                {
                    return typeof(GridDataCellElement);
                }
            }

            public override bool IsCompatible(GridViewColumn data, object context)
            {
                return data is ProgressBarColumn && context is GridDataRowElement;
            }
        }

        public class ProgressBarColumn : GridViewDataColumn
        {
            public override string HeaderText
            {
                get => base.HeaderText;
                set
                {
                    String s = Lenguaje.traduce(value);
                    base.HeaderText = value;
                }
            }

            public ProgressBarColumn(string fieldName) : base(fieldName)
            {
            }

            public override Type GetCellType(GridViewRowInfo row)
            {
                if (row is GridViewDataRowInfo)
                {
                    return typeof(ProgressBarCellElement);
                }
                return base.GetCellType(row);
            }
        }

        #endregion ProgressBarColumn

        private void rBtnActualizar_Click(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void Refrescar()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (tlPrincipal.GetControlFromPosition(0, 0).Name == "pedidos")
                {
                    rgvPedidos.BeginUpdate();
                    rgvPedidos.ValueChanged -= rgvPedidos_ValueChanged;
                    rgvPedidos.DataSource = null;
                    // rgvPedidos.Columns.Remove(colLineas);
                    //rgvPedidos.Columns.Remove(colPaletsCompletos);
                    //rgvPedidos.Columns.Remove(colUnidadesSueltas);
                    rgvPedidos.Columns.Remove(colPorcentLineas);
                    //rgvPedidos.Columns.Remove(colNRecepciones);
                    rgvPedidos.DataSource = Business.GetPedidosProveedorDatosGridView(_lstEsquemaTablaProveedores);
                    //CreateChildTemplate();
                    AñadirColumnas();
                    AñadirValorProgressBar();
                    ColorEstadoPedidos();
                    ElegirEstilo();
                    pedidosFilaSeleccionados.Clear();
                    pedidosSeleccionados.Clear();
                    rgvPedidos.ValueChanged += rgvPedidos_ValueChanged;
                    rgvPedidos.EndUpdate();
                    refrescarTimerPedidos();
                }
                else
                {
                    rgvRecepciones.ValueChanged -= rgvPedidos_ValueChanged;
                    rgvRecepciones.DataSource = null;
                    string ids = ExtraerIdPedido();
                    DataTable dtRecepciones = Business.GetRecepcionesDatosGridView(_lstEsquemaTablaRecepciones, _lstEsquemaTablaRecepcionesUnion, ids);
                    RumboSGAManager.Utilidades.TraducirDataTableColumnName(ref dtRecepciones);
                    rgvRecepciones.DataSource = dtRecepciones;
                    refrescarTemplatesRecepciones();
                    ElegirEstilo();
                    rgvRecepciones.ValueChanged += rgvPedidos_ValueChanged;
                    recepcionesSeleccionadas.Clear();
                    refrescarTimerRecepciones();
                }
                if (rBtnActualizar != null)
                    rBtnActualizar.disableRefrescoTimerColor();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Error refrescando pantalla" + ex.Message);
                log.Error("Mensaje:Error refrescando pantalla" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            this.Cursor = Cursors.Default;
        }

        public void refrescarTemplatesRecepciones()
        {
            foreach (GridViewTemplate item in rgvRecepciones.Templates)
            {
                item.Refresh();
            }
        }

        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            GridDataCellElement cell = e.ContextMenuProvider as GridDataCellElement;

            GridViewRowInfo row = cell.RowInfo;

            if (row.HasChildRows())
            {
                //e.ContextMenu = firstContextMenu.DropDown;
            }
            else
            {
                //e.ContextMenu = secondContextMenu.DropDown;
            }
        }

        public string ExtraerIdPedido()
        {
            string idPedido = string.Empty;
            foreach (int item in pedidosSeleccionados)
            {
                idPedido += item + " ,";
            }
            if (idPedido == string.Empty)
            { }
            else
            {
                idPedido = idPedido.Substring(0, idPedido.Length - 2);
            }
            return idPedido;
        }

        public RadGridView gridViewControl
        {
            get { return this.rgvPedidos; }
        }

        #region Recepciones

        private void rgvRecepciones_RowSourceNeeded(object sender, GridViewRowSourceNeededEventArgs e)
        {
            String mensajeError = "";
            try
            {
                string valor = string.Empty;
                DataRowView rowView = e.ParentRow.DataBoundItem as DataRowView;
                e.Template.MasterTemplate.MultiSelect = true;
                if (e.Template.Caption == Lenguaje.traduce("Lineas"))
                {
                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                    valor = e.ParentRow.Cells["ID"].Value.ToString();
                    DataTable lineas = Business.GetRecepcionesJerarquicoLineas(valor);
                    if (lineas.Rows.Count == 0)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        e.SourceCollection.Add(row);
                    }
                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            try
                            {
                                if (lineas.Columns[i].DataType == typeof(Decimal))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = Decimal.Parse(lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                }
                                else
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                    //row.Cells[lineasPedido.Columns[i].ColumnName].Value = dataRow[row.Cells[lineasPedido.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                }
                            }
                            catch (Exception error)
                            {
                                mensajeError += "Error añadiendo la columna " + row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la linea:" + a + " error:" + error.Message;
                            }
                        }

                        e.SourceCollection.Add(row);
                    }
                }
                if (e.Template.Caption == Lenguaje.traduce("Entradas"))
                {
                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                    valor = e.ParentRow.Cells["ID"].Value.ToString();
                    DataTable lineas = Business.GetRecepcionesJerarquicoEntradas(valor);
                    if (lineas.Rows.Count == 0)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        e.SourceCollection.Add(row);
                    }
                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            try
                            {
                                if (lineas.Columns[i].DataType == typeof(Decimal))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = Decimal.Parse(lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                }
                                else
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                }

                                if (row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText.Contains("ID"))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.IsVisible = false;
                                }
                            }
                            catch (Exception error)
                            {
                                mensajeError += "Error añadiendo la columna " + row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la linea:" + a + " error:" + error.Message;
                            }
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                if (e.Template.Caption == Lenguaje.traduce("Existencias"))
                {
                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                    valor = e.ParentRow.Cells["ID"].Value.ToString();
                    DataTable lineas = Business.GetRecepcionesJerarquicoExistencias(valor);

                    if (lineas.Rows.Count == 0)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        e.SourceCollection.Add(row);
                    }

                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            try
                            {
                                if (lineas.Columns[i].DataType == typeof(Decimal))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = Decimal.Parse(lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                }
                                else
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                }
                                if (row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.FieldName.Contains("ID"))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.IsVisible = false;
                                }
                            }
                            catch (Exception error)
                            {
                                mensajeError += "Error añadiendo la columna " + row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la linea:" + a + " error:" + error.Message;
                            }
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                if (e.Template.Caption == Lenguaje.traduce("Ubicaciones"))
                {
                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                    valor = e.ParentRow.Cells["ID"].Value.ToString();
                    DataTable lineas = Business.GetRecepcionesJerarquicoMovimientos(valor);
                    if (lineas.Rows.Count == 0)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        e.SourceCollection.Add(row);
                    }
                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            try
                            {
                                if (lineas.Columns[i].DataType == typeof(Decimal))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = Decimal.Parse(lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                }
                                else
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                }

                                if (row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.FieldName.Contains("ID"))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.IsVisible = false;
                                }
                            }
                            catch (Exception error)
                            {
                                mensajeError += "Error añadiendo la columna " + row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la linea:" + a + " error:" + error.Message;
                            }
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                if (e.Template.Caption == Lenguaje.traduce("Tareas"))
                {
                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                    valor = e.ParentRow.Cells["ID"].Value.ToString();
                    DataTable lineas = Business.GetRecepcionesJerarquicoTareas(valor);
                    if (lineas.Rows.Count == 0)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        e.SourceCollection.Add(row);
                    }
                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            try
                            {
                                row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                if (row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.FieldName.Contains("ID"))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.IsVisible = false;
                                }
                            }
                            catch (Exception error)
                            {
                                mensajeError += "Error añadiendo la columna " + row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la linea:" + a + " error:" + error.Message;
                            }
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                if (e.Template.Caption == Lenguaje.traduce("Preavisos"))
                {
                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                    valor = e.ParentRow.Cells["ID"].Value.ToString();
                    DataTable lineas = Business.GetRecepcionesJerarquicoPreavisos(valor);
                    if (lineas.Rows.Count == 0)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        e.SourceCollection.Add(row);
                    }
                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            try
                            {
                                if (lineas.Columns[i].DataType == typeof(Decimal))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = Decimal.Parse(lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                }
                                else
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                }
                                if (row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.FieldName.Contains("ID"))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.IsVisible = false;
                                }
                            }
                            catch (Exception error)
                            {
                                mensajeError += "Error añadiendo la columna " + row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la linea:" + a + " error:" + error.Message;
                            }
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                if (e.Template.Caption == Lenguaje.traduce("Embalajes"))
                {
                    DataRow[] rows = rowView.Row.GetChildRows("ID");
                    valor = e.ParentRow.Cells["ID"].Value.ToString();
                    DataTable lineas = Business.GetRecepcionesJerarquicoEmbalajes(valor);

                    if (lineas.Rows.Count == 0)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        e.SourceCollection.Add(row);
                    }
                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            try
                            {
                                row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                if (row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.FieldName.Contains("ID"))
                                {
                                    row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.IsVisible = false;
                                }
                            }
                            catch (Exception error)
                            {
                                mensajeError += "Error añadiendo la columna " + row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la linea:" + a + " error:" + error.Message;
                            }
                        }
                        e.SourceCollection.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rBtnFiltros_Click(object sender, EventArgs e)
        {
            FiltroInicialRecepciones filtro = new FiltroInicialRecepciones("ProveedoresPedidosCab");
            filtro.ShowDialog();
        }

        private void LlenarGridRecepciones()
        {
            string ids = ExtraerIdPedido();
            lblNumRegistros.Text = Lenguaje.traduce("Registros:") + Business.GetRecepcionesCantidad(ref _lstEsquemaTablaRecepciones);
            //MONICA_lstEsquemaTablaRecepcionesUnion = null;
            //MONICA_lstEsquemaTablaRecepciones = null;
            //MONICA lo cargo al inicio Business.GetPedidosProveedorJerarquicoRecepcionesEsquemaUnion(ref _lstEsquemaTablaRecepcionesUnion);

            Business.GetRecepcionesCantidad(ref _lstEsquemaTablaRecepciones);
            if (rgvRecepciones.DataSource != null)
            {
                rgvRecepciones.DataSource = null;
            }
            rgvRecepciones.DataSource = Business.GetRecepcionesDatosGridView(_lstEsquemaTablaRecepciones, _lstEsquemaTablaRecepcionesUnion, ids);

            rgvRecepciones.BestFitColumns();
            rgvRecepciones.Dock = DockStyle.Fill;
        }

        private void rBtnRecepcionesButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                log.Info("Pulsado botón recepciones " + DateTime.Now);
                rBtnGenerarTarea.Enabled = true;

                rBtnRecepciones.Enabled = false;
                rbtnPedidos.Enabled = true;

                recepcionesSeleccionadas.Clear();

                LlenarGridRecepciones();
                CambiarBotonesRecepciones();
                tlPrincipal.Controls.Remove(rgvPedidos);
                tlPrincipal.Controls.Add(rgvRecepciones);
                rgvRecepciones.ReadOnly = false;

                int pr = rgvRecepciones.Rows.Count;

                GridViewCheckBoxColumn checkBoxColumnRecepciones = new GridViewCheckBoxColumn();
                checkBoxColumnRecepciones.DataType = typeof(int);
                checkBoxColumnRecepciones.Name = "Check";
                checkBoxColumnRecepciones.AllowReorder = false;
                checkBoxColumnRecepciones.AllowFiltering = false;
                checkBoxColumnRecepciones.Checked = ToggleState.On;

                rgvRecepciones.MasterTemplate.Columns.Add(checkBoxColumnRecepciones);
                rgvRecepciones.MasterTemplate.Columns.Move(checkBoxColumnRecepciones.Index, 0);

                foreach (GridViewDataColumn item in rgvRecepciones.Columns)
                {
                    if (item is GridViewCheckBoxColumn)
                    {
                        item.ReadOnly = false;
                    }
                    else
                    {
                        item.ReadOnly = true;
                    }
                }
                lblNumRegistros.Text = Lenguaje.traduce("Registros:") + rgvRecepciones.MasterTemplate.Rows.Count;
                /*¿Hace falta?*/

                if (rgvRecepciones.Rows.Count > 0)
                {
                    ElegirEstilo();
                    ColorEstadoRecepciones();
                    OcultarCamposrecepciones();
                    ElegirEstilo();
                }

                //César Muñoz, sí hace falta
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void radGridView1_ValueChanged(object sender, EventArgs e)
        {
            RadCheckBoxEditor editor = sender as RadCheckBoxEditor;
            this.rgvRecepciones.GridViewElement.BeginEdit(); foreach (GridViewDataRowInfo row in this.rgvRecepciones.Rows)
            {
                if (row == this.rgvRecepciones.CurrentRow)
                {
                    row.Cells["Check"].Value = 1;
                }
            }
            this.rgvRecepciones.GridViewElement.EndEdit();
        }

        private void CreateChildTemplateRecepciones()
        {
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio CreateChildTemplateRecepciones ProveedoresPedidosCabGridView");
            CreateChildTemplateRecepcionesLineas();
            CreateChildTemplateRecepcionesEntradas();
            CreateChildTemplateRecepcionesExistencias();
            CreateChildTemplateRecepcionesUbicaciones();
            CreateChildTemplateRecepcionesTareas();
            CreateChildTemplateRecepcionesPreavisos();
            CreateChildTemplateRecepcionesEmbalajes();
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin CreateChildTemplateRecepciones ProveedoresPedidosCabGridView");
        }

        private void CreateChildTemplateRecepcionesPreavisos()
        {
            //PREAVISOS
            gtJerarquicoRecepcionesPreavisos = new GridViewTemplate();
            gtJerarquicoRecepcionesPreavisos.Caption = Lenguaje.traduce("Preavisos");
            string jsonPreavisos = DataAccess.LoadJson("RecepcionesJerarquicoPreavisos");
            JArray js2 = JArray.Parse(jsonPreavisos);
            string jsonEsquemaPreav = js2.First()["Scheme"].ToString();
            List<TableScheme> esquema2 = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaPreav);
            for (int i = 0; i < esquema2.Count; i++)
            {
                if (esquema2[i].Etiqueta != string.Empty && esquema2[i].Etiqueta != null)
                {
                    gtJerarquicoRecepcionesPreavisos.Columns.Add(esquema2[i].Etiqueta);
                    if (!esquema2[i].EsVisible)
                    {
                        gtJerarquicoRecepcionesPreavisos.Columns[esquema2[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoRecepcionesPreavisos.BestFitColumns();
            rgvRecepciones.Templates.Add(gtJerarquicoRecepcionesPreavisos);
            gtJerarquicoRecepcionesPreavisos.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoRecepcionesPreavisos);

            //foreach (GridViewTemplate item in rgvRecepciones.Templates)
            //{
            //    item.ReadOnly = true;
            //}
        }

        private void CreateChildTemplateRecepcionesTareas()
        {
            //TAREAS
            string jsonTareas = DataAccess.LoadJson("RecepcionesJerarquicoTareas");
            JArray jsTareas = JArray.Parse(jsonTareas);
            string jsonEsquemaTareas = jsTareas.First()["Scheme"].ToString();
            List<TableScheme> esquemaTareas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaTareas);
            gtJerarquicoRecepcionesTareas = new GridViewTemplate();
            gtJerarquicoRecepcionesTareas.Caption = Lenguaje.traduce("Tareas");
            for (int i = 0; i < esquemaTareas.Count; i++)
            {
                if (esquemaTareas[i].Etiqueta != string.Empty && esquemaTareas[i].Etiqueta != null)
                {
                    gtJerarquicoRecepcionesTareas.Columns.Add(esquemaTareas[i].Etiqueta);
                    if (!esquemaTareas[i].EsVisible)
                    {
                        gtJerarquicoRecepcionesTareas.Columns[esquemaTareas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoRecepcionesTareas.BestFitColumns();
            rgvRecepciones.Templates.Add(gtJerarquicoRecepcionesTareas);
            gtJerarquicoRecepcionesTareas.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoRecepcionesTareas);
        }

        private void CreateChildTemplateRecepcionesUbicaciones()
        {
            //UBICACIONES
            string jsonUbicaciones = DataAccess.LoadJson("RecepcionesJerarquicoMovimientos");
            JArray jsUbicaciones = JArray.Parse(jsonUbicaciones);
            string jsonEsquemaUbicaciones = jsUbicaciones.First()["Scheme"].ToString();
            List<TableScheme> esquemaUbicaciones = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaUbicaciones);
            gtJerarquicoRecepcionesMovimientos = new GridViewTemplate();
            gtJerarquicoRecepcionesMovimientos.Caption = Lenguaje.traduce("Ubicaciones");
            for (int i = 0; i < esquemaUbicaciones.Count; i++)
            {
                if (esquemaUbicaciones[i].Etiqueta != string.Empty && esquemaUbicaciones[i].Etiqueta != null)
                {
                    gtJerarquicoRecepcionesMovimientos.Columns.Add(esquemaUbicaciones[i].Etiqueta);
                    if (!esquemaUbicaciones[i].EsVisible)
                    {
                        gtJerarquicoRecepcionesMovimientos.Columns[esquemaUbicaciones[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoRecepcionesMovimientos.BestFitColumns();
            rgvRecepciones.Templates.Add(gtJerarquicoRecepcionesMovimientos);
            gtJerarquicoRecepcionesMovimientos.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoRecepcionesMovimientos);
        }

        private void CreateChildTemplateRecepcionesEmbalajes()
        {
            //EMBALAJES
            string jsonEmbalajes = DataAccess.LoadJson("RecepcionesJerarquicoEmbalajes");
            JArray jsEmbalajes = JArray.Parse(jsonEmbalajes);
            string jsonEsquemaEmbalajes = jsEmbalajes.First()["Scheme"].ToString();
            List<TableScheme> esquemaEmbalajes = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEmbalajes);
            //gtJerarquicoRecepcionesEmbalajes = new GridViewTemplate();
            gtJerarquicoRecepcionesEmbalajes.Caption = Lenguaje.traduce("Embalajes");
            for (int i = 0; i < esquemaEmbalajes.Count; i++)
            {
                if (esquemaEmbalajes[i].Etiqueta != string.Empty && esquemaEmbalajes[i].Etiqueta != null)
                {
                    gtJerarquicoRecepcionesEmbalajes.Columns.Add(esquemaEmbalajes[i].Etiqueta);
                    if (!esquemaEmbalajes[i].EsVisible)
                    {
                        gtJerarquicoRecepcionesEmbalajes.Columns[esquemaEmbalajes[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoRecepcionesEmbalajes.BestFitColumns();
            rgvRecepciones.Templates.Add(gtJerarquicoRecepcionesEmbalajes);

            gtJerarquicoRecepcionesEmbalajes.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoRecepcionesEmbalajes);
        }

        private void CreateChildTemplateRecepcionesExistencias()
        {
            //EXISTENCIAS
            string jsonExistencias = DataAccess.LoadJson("RecepcionesJerarquicoExistencias");
            JArray jsExistencias = JArray.Parse(jsonExistencias);
            string jsonEsquemaExistencias = jsExistencias.First()["Scheme"].ToString();
            List<TableScheme> esquemaExistencias = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaExistencias);
            GridViewTemplate templateRecepcionesExistencias = new GridViewTemplate();
            templateRecepcionesExistencias.Caption = Lenguaje.traduce("Existencias");
            for (int i = 0; i < esquemaExistencias.Count; i++)
            {
                if (esquemaExistencias[i].Etiqueta != string.Empty && esquemaExistencias[i].Etiqueta != null)
                {
                    templateRecepcionesExistencias.Columns.Add(esquemaExistencias[i].Etiqueta);
                    if (!esquemaExistencias[i].EsVisible)
                    {
                        templateRecepcionesExistencias.Columns[esquemaExistencias[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            templateRecepcionesExistencias.BestFitColumns();
            templateRecepcionesExistencias.AllowEditRow = false;
            rgvRecepciones.Templates.Add(templateRecepcionesExistencias);
            templateRecepcionesExistencias.HierarchyDataProvider = new GridViewEventDataProvider(templateRecepcionesExistencias);
        }

        private void CreateChildTemplateRecepcionesEntradas()
        {
            ////ENTRADAS
            string jsonEntradas = DataAccess.LoadJson("RecepcionesJerarquicoEntradas");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            gtJerarquicoRecepcionesEntradas = new GridViewTemplate();
            gtJerarquicoRecepcionesEntradas.Caption = Lenguaje.traduce("Entradas");
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    gtJerarquicoRecepcionesEntradas.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        gtJerarquicoRecepcionesEntradas.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoRecepcionesEntradas.BestFitColumns();
            gtJerarquicoRecepcionesEntradas.AllowEditRow = true;
            rgvRecepciones.Templates.Add(gtJerarquicoRecepcionesEntradas);
            gtJerarquicoRecepcionesEntradas.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoRecepcionesEntradas);
            //foreach (GridViewDataColumn item in gtJerarquicoRecepcionesEntradas.Columns)
            //{
            //    if (item.FieldName == "Pallet Type ID" || item.FieldName == "ID Tipo Palet")
            //    {
            //    }
            //}
        }

        private void CreateChildTemplateRecepcionesLineas()
        {
            //LINEAS
            string json1 = DataAccess.LoadJson("RecepcionesJerarquicoLineas");
            JArray js = JArray.Parse(json1);
            string jsonEsquema = js.First()["Scheme"].ToString();
            List<TableScheme> esquema1 = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

            gtJerarquicoRecepcionesLineas = new GridViewTemplate();
            gtJerarquicoRecepcionesLineas.Caption = Lenguaje.traduce("Lineas");
            for (int i = 0; i < esquema1.Count; i++)
            {
                if (esquema1[i].Etiqueta != string.Empty && esquema1[i].Etiqueta != null)
                {
                    gtJerarquicoRecepcionesLineas.Columns.Add(esquema1[i].Etiqueta);
                    if (!esquema1[i].EsVisible)
                    {
                        gtJerarquicoRecepcionesLineas.Columns[esquema1[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            gtJerarquicoRecepcionesLineas.AllowEditRow = false;
            gtJerarquicoRecepcionesLineas.BestFitColumns();
            rgvRecepciones.Templates.Add(gtJerarquicoRecepcionesLineas);
            gtJerarquicoRecepcionesLineas.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoRecepcionesLineas);
        }

        /*void radGridView1_HeaderCellToggleStateChanged(object sender, GridViewHeaderCellEventArgs e)
        {
            bool selected = e.State.ToString() == "On";
            var row = rgvPedidos.MasterTemplate.Templates[0].Rows;
            foreach (object r in gtJerarquicoPedidosLineasPedidos.Rows)
            {
                //r.IsSelected = selected;
            }
        }*/

        private void CreateChildTemplateJerarquicoLineasPedido()
        {
            //LINEAS
            //GridViewTemplate template = new GridViewTemplate();
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio CreateChildTemplateJerarquicoLineasPedido ProveedoresPedidosCabGridView");
            gtJerarquicoPedidosLineasPedidos.Caption = Lenguaje.traduce("Lineas");
            string jsonLineas = DataAccess.LoadJson("PedidosProvJerarquicoLineas");
            JArray jsLineas = JArray.Parse(jsonLineas);
            string jsonEsquemaLineas = jsLineas.First()["Scheme"].ToString();
            List<TableScheme> esquemaLineas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaLineas);
            gtJerarquicoPedidosLineasPedidos.ReadOnly = false;
            for (int i = 0; i < esquemaLineas.Count; i++)
            {
                if (esquemaLineas[i].Etiqueta != string.Empty && esquemaLineas[i].Etiqueta != null)
                {
                    /*if (esquemaLineas[i].Etiqueta == Lenguaje.traduce("Cantidad Pendiente"))
                    {
                        GridViewDecimalColumn colInt = new GridViewDecimalColumn(esquemaLineas[i].Etiqueta);
                        colInt.DataType = typeof(int);
                        colInt.FormatString = "{0:0,###.##}";
                        gtJerarquicoPedidosLineasPedidos.Columns.Add(colInt);
                    }
                    else
                    {*/
                    GridViewTextBoxColumn col = new GridViewTextBoxColumn(esquemaLineas[i].Etiqueta);
                    col.FieldName = esquemaLineas[i].Etiqueta;
                    col.ReadOnly = true;

                    gtJerarquicoPedidosLineasPedidos.Columns.Add(col);
                    if (!esquemaLineas[i].EsVisible)
                    {
                        gtJerarquicoPedidosLineasPedidos.Columns[esquemaLineas[i].Etiqueta].IsVisible = false;
                    }
                    /*}*/
                }
            }

            gtJerarquicoPedidosLineasPedidos.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoPedidosLineasPedidos);
            gtJerarquicoPedidosLineasPedidos.Columns.Add(checkBoxColumnJerarquicoLineas);
            gtJerarquicoPedidosLineasPedidos.Columns.Move(checkBoxColumnJerarquicoLineas.Index, 0);
            gtJerarquicoPedidosLineasPedidos.Columns.Remove(colCantidadPteUbicar);
            gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidoPorcentEntPend);
            gtJerarquicoPedidosLineasPedidos.Columns.Remove(colCantidadEntrada);
            gtJerarquicoPedidosLineasPedidos.Columns.Add(colJerLineasPedidosPorcentEnt);
            OcultarColumnasLineas();
            gtJerarquicoPedidosLineasPedidos.BestFitColumns();
            //log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin CreateChildTemplateJerarquicoLineasPedido ProveedoresPedidosCabGridView");
        }

        private void rgvRecepciones_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.HierarchyLevel == 1 && e.RowElement.RowInfo is GridViewNewRowInfo)
            {
                e.RowElement.RowInfo.MaxHeight = 1;
            }
        }

        private void rgvPedidos_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.HierarchyLevel == 1 && e.RowElement.RowInfo is GridViewNewRowInfo)
            {
                e.RowElement.RowInfo.MaxHeight = 1;
            }
        }

        #endregion Recepciones
    }
}
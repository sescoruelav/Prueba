using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGA.PedidoCliMotor;
using RumboSGA.Presentation.Herramientas.PantallasWS;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using RumboSGAManager.Model.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.Data;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;
using Rumbo.Core.Herramientas;
using System.Dynamic;
using RumboSGA.CargaMotor;
using System.Web.Script.Serialization;
using RumboSGA.Controles;
using RumboSGA.GestionAlmacen;
using System.Data.SqlClient;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.SalidaMotor;
using Telerik.WinControls.Enumerations;
using RumboSGA.Presentation.Herramientas;
using Rumbo.Core.Herramientas.Herramientas;
using static RumboSGA.GestionAlmacen.FrmSeleccionFechaEntrega;

namespace RumboSGA.Maestros
{
    public partial class OrdenesRecogida : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        private WSPedidoCliMotorClient wsPedidoCli = new WSPedidoCliMotorClient();
        private WSSalidaMotorClient wsSalidaMotor = new WSSalidaMotorClient();

        private RadWaitingBar radWaitPedidos = new RadWaitingBar();
        private RadWaitingBar radWaitOrdenes = new RadWaitingBar();
        private RadWaitingBar radWaitCarga = new RadWaitingBar();
        private RadWaitingBar radWaitCargaPLPte = new RadWaitingBar();

        private BackgroundWorker bgWorkerGridViewPedidos;
        private BackgroundWorker bgWorkerGridViewOrdenes;
        private BackgroundWorker bgWorkerGridViewCarga;
        private BackgroundWorker bgWorkerGridViewPLPteCarga;

        private GridViewCheckBoxColumn chkColPedidos = new GridViewCheckBoxColumn();
        private GridViewCheckBoxColumn chkColOrdenesRecogida = new GridViewCheckBoxColumn();
        private GridViewCheckBoxColumn chkColOrdenesCarga = new GridViewCheckBoxColumn();
        private GridViewCheckBoxColumn chkColPLPteCarga = new GridViewCheckBoxColumn();
        private GridViewCheckBoxColumn chkColJerarquicoORLineas = new GridViewCheckBoxColumn();

        private GridViewCheckBoxColumn chkColJerarquicoOCPackingList = new GridViewCheckBoxColumn();

        private RadRibbonBarGroup grupoLabel = new RadRibbonBarGroup();
        private RadLabelElement lblCantidad = new RadLabelElement();
        private RadGridView rgvGridOrdenes = new RumGridView();
        private RadGridView rgvGridCarga = new RumGridView();
        private RadGridView rgvGridCargaPLPteCarga = new RumGridView();
        private RadGridView rgvGridCargaPackingList = new RumGridView();
        private RadGridView rgvGridCargaPedidos = new RumGridView();
        private RadGridView rgvGridCargaContenido = new RumGridView();
        private RadPanel rdPanelPLPteCarga = new RadPanel();

        private GridViewDecimalColumn colLineasPedido = new GridViewDecimalColumn();
        private GridViewDecimalColumn colPaletsCompletos = new GridViewDecimalColumn();
        private GridViewDecimalColumn colNumPickings = new GridViewDecimalColumn();
        private GridViewDecimalColumn colNumTareas = new GridViewDecimalColumn();
        private GridViewDecimalColumn colDuracionEstimada = new GridViewDecimalColumn();
        private GridViewDecimalColumn colTareasPendientes = new GridViewDecimalColumn();
        private DataColumn cantidadServidaJerarquico = new DataColumn(Lenguaje.traduce(strings.CantidadServida));
        private DataColumn cantidadReservadaJerarquico = new DataColumn(Lenguaje.traduce(strings.CantidadReservada));
        private ProgressBarColumn colLineasReservadas = new ProgressBarColumn(Lenguaje.traduce(strings.LineasReservadas));
        private ProgressBarColumn colLineasPreparadas = new ProgressBarColumn(Lenguaje.traduce(strings.LineasPreparadas));
        private ProgressBarColumn colpctCantidadReservada = new ProgressBarColumn(Lenguaje.traduce(strings.pctCantidadReservada));
        private ProgressBarColumn colpctCantidadServida = new ProgressBarColumn(Lenguaje.traduce(strings.pctCantidadServida));
        private RadDataFilterDialog radDataFilterPedidos = new RadDataFilterDialog();
        private RadDataFilterDialog radDataFilterOrdenes = new RadDataFilterDialog();
        private RadDataFilterDialog radDataFilterCarga = new RadDataFilterDialog();

        private RumButton btnDerecha = new RumButton();

        private RumButton btnIzquierda = new RumButton();
        private int cargaSeleccionadaWorker = 0;
        private int cargaAnteriorSeleccionada = 0;
        private DataTable dtPrincipal = new DataTable();
        private DataTable dtPLPteCargas = new DataTable();

        public string filterPedido;
        protected dynamic _selectedRow;
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        protected List<TableScheme> _lstEsquemaTablaPedidos = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaOrdenesRecogida = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaOrdenesCarga = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaCargaPacking = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaCargaPedidos = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaCargaContenido = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaCargaPackingPte = new List<TableScheme>();

        private FilterDescriptorCollection filtroAnteriorPedidos = new FilterDescriptorCollection();
        private WSSalidaMotorClient wssMotor = new WSSalidaMotorClient();
        private Timer timerRefrescar;
        private rRbnOrganizador organizador; //Para no generar dos o más organizadores
        private bool mostrarAvisoCierreCarga = Persistencia.getParametroBoolean("MOSTRARAVISOCIERRECARGA");
        private bool mostrarAvisoFaltas = Persistencia.getParametroBoolean("MOSTRARAVISOFALTAS");
        private int generarOrdenesPorFechaEntrega = Persistencia.getParametroInt("GENERARORDENESPORFECHAENTREGA");
        private string[] prueba;
        private List<string> datosCargaTotales = new List<string>();

        private Label lblUdsCant = new Label();
        private Label lblVolumen = new Label();
        private Label lblPeso = new Label();
        private Label lblPesoCant = new Label();
        private Label lblVolumenCant = new Label();
        private Label lblUds = new Label();
        private Label lblPalets = new Label();
        private Label lblPaletsCount = new Label();

        //Para mantener si hay una carga ya seleccionada

        private RadContextMenu contextMenuPedidos = new RadContextMenu();
        private RadContextMenu contextMenuOrdenes = new RadContextMenu();
        private RadContextMenu contextMenuCarga = new RadContextMenu();

        #endregion Variables

        #region Constructor

        public OrdenesRecogida()
        {
            try
            {
                InitializeComponent();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                InitializeThemesDropDown();
                rdfFiltro.Visible = false;
                this.Load += OrdenesRecogida_Load;
                ControlesLenguaje();
                this.Shown += form_Shown;
                this.Show();
                this.Text = Lenguaje.traduce("Expediciones");
                // name = strings.OrdenesRecogida;
                btnVistaOrdenesOrdenes.Enabled = false;
                rBtnVistaPedidosPedidos.Enabled = false;
                btnVistaCargaCarga.Enabled = false;
                btnDerecha.Image = Resources.forward;
                btnDerecha.BackgroundImageLayout = ImageLayout.Stretch;
                btnDerecha.ImageAlignment = ContentAlignment.MiddleCenter;
                btnDerecha.DisplayStyle = DisplayStyle.Image;
                btnDerecha.AutoSize = true;
                btnDerecha.Anchor = AnchorStyles.None;
                btnIzquierda.Image = Resources.back;
                btnIzquierda.BackgroundImageLayout = ImageLayout.Stretch;
                btnIzquierda.ImageAlignment = ContentAlignment.MiddleCenter;
                btnIzquierda.ImageAlignment = ContentAlignment.MiddleCenter;
                btnIzquierda.DisplayStyle = DisplayStyle.Image;
                btnIzquierda.AutoSize = true;
                btnIzquierda.Anchor = AnchorStyles.None;

                IniciarDatos();
                InicializarEsquemas();
                LlamadaEventos();
                GenerarGridPedidos();
                AñadirLabelCantidad();

                addEliminarLayoutOpciones();
                InicializarContextMenu();
                rgvGridPedidos.ContextMenuOpening += RgvGridPedidos_ContextMenuOpening;
                rgvGridOrdenes.ContextMenuOpening += RgvGridPedidos_ContextMenuOpening;
                rgvGridCarga.ContextMenuOpening += RgvGridPedidos_ContextMenuOpening;
                rgvGridCargaContenido.ContextMenuOpening += RgvGridPedidos_ContextMenuOpening;
                rgvGridCargaPackingList.ContextMenuOpening += RgvGridPedidos_ContextMenuOpening;
                rgvGridCargaPedidos.ContextMenuOpening += RgvGridPedidos_ContextMenuOpening;
                rgvGridCargaPLPteCarga.ContextMenuOpening += RgvGridPedidos_ContextMenuOpening;
                rgvGridCargaPedidos.CellEndEdit += RgvGridCargaPedidos_CellEndEdit;
                rgvGridCargaPackingList.CellEndEdit += RgvGridCargaPackingList_CellEndEdit;

                iniciarTimer();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void InicializarContextMenu()
        {
            InicializarContextMenuPedidos();
            InicializarContextMenuOrdenes();
            InicializarContextMenuOrdenesCarga();
        }

        private void InicializarContextMenuPedidos()
        {
            RumMenuItem agregarComentarioPedidoMenuItem = new RumMenuItem();
            agregarComentarioPedidoMenuItem.Text = strings.AgregarComentario;
            agregarComentarioPedidoMenuItem.Click += new EventHandler(agregarComentarioPedidoMenuItem_Event);
            contextMenuPedidos.Items.Add(agregarComentarioPedidoMenuItem);
            RumMenuItem verComentariosPedidoMenuItem = new RumMenuItem();
            verComentariosPedidoMenuItem.Text = strings.VerComentarios;
            verComentariosPedidoMenuItem.Click += new EventHandler(verComentariosPedidoMenuItem_Event);
            contextMenuPedidos.Items.Add(verComentariosPedidoMenuItem);
        }

        private void InicializarContextMenuOrdenes()
        {
            RumMenuItem agregarComentarioOrdenMenuItem = new RumMenuItem();
            agregarComentarioOrdenMenuItem.Text = strings.AgregarComentario;
            agregarComentarioOrdenMenuItem.Click += new EventHandler(agregarComentarioOrdenMenuItem_Event);
            contextMenuOrdenes.Items.Add(agregarComentarioOrdenMenuItem);
            RumMenuItem verComentariosOrdenesMenuItem = new RumMenuItem();
            verComentariosOrdenesMenuItem.Text = strings.VerComentarios;
            verComentariosOrdenesMenuItem.Click += new EventHandler(verComentariosOrdenesMenuItem_Event);
            contextMenuOrdenes.Items.Add(verComentariosOrdenesMenuItem);
        }

        private void InicializarContextMenuOrdenesCarga()
        {
            RumMenuItem agregarComentarioOrdenCargaMenuItem = new RumMenuItem();
            agregarComentarioOrdenCargaMenuItem.Text = strings.AgregarComentario;
            agregarComentarioOrdenCargaMenuItem.Click += new EventHandler(agregarComentarioOrdenCargaMenuItem_Event);
            contextMenuCarga.Items.Add(agregarComentarioOrdenCargaMenuItem);
            RumMenuItem verComentariosOrdenCargaMenuItem = new RumMenuItem();
            verComentariosOrdenCargaMenuItem.Text = strings.VerComentarios;
            verComentariosOrdenCargaMenuItem.Click += new EventHandler(verComentariosOrdenCargaMenuItem_Event);
            contextMenuCarga.Items.Add(verComentariosOrdenCargaMenuItem);
        }

        private void verComentariosPedidoMenuItem_Event(object sender, EventArgs e)
        {
            log.Info("Ver comentarios pedido ");
            try
            {
                object idPedidoCli = rgvGridPedidos.CurrentRow.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value;
                DataTable comentarios = DataAccess.getComentarios(int.Parse(idPedidoCli.ToString()), "PCC");
                VisorComentarios visor = new VisorComentarios(comentarios);
                DialogResult resp = visor.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void verComentariosOrdenesMenuItem_Event(object sender, EventArgs e)
        {
            log.Info("Ver comentarios ordenes ");
            try
            {
                object idOrdenRecogida = rgvGridOrdenes.CurrentRow.Cells["" + Lenguaje.traduce(strings.NumOrdenRecogida) + ""].Value;
                DataTable comentarios = DataAccess.getComentarios(int.Parse(idOrdenRecogida.ToString()), "ORC");
                VisorComentarios visor = new VisorComentarios(comentarios);
                DialogResult resp = visor.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void verComentariosOrdenCargaMenuItem_Event(object sender, EventArgs e)
        {
            log.Info("Ver comentarios orden Carga ");
            try
            {
                object idCarga = rgvGridCarga.CurrentRow.Cells["" + Lenguaje.traduce("Num Carga") + ""].Value;
                DataTable comentarios = DataAccess.getComentarios(int.Parse(idCarga.ToString()), "OCC");
                VisorComentarios visor = new VisorComentarios(comentarios);
                DialogResult resp = visor.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void agregarComentarioPedidoMenuItem_Event(object sender, EventArgs e)
        {
            log.Info("Agregando comentario pedido ");
            try
            {
                RumMessageInputBox message = new RumMessageInputBox(Lenguaje.traduce("Comentarios pedido"), "Introduzca comentario", "PCC");
                DialogResult resp = message.ShowDialog();
                if (resp == DialogResult.OK)
                {
                    object comentario = message.input;

                    string idMotivoComentario = message.idMotivo;
                    if (comentario == null || comentario.ToString().Equals(""))
                    {
                        if (message.path == null || message.path.Equals(""))
                        {
                            if (idMotivoComentario == null || idMotivoComentario.Equals(""))
                            {
                                return;
                            }
                            else
                            {
                                comentario = idMotivoComentario;
                            }
                        }
                        else
                        {
                            comentario = Lenguaje.traduce("Imagen");
                        }
                    }

                    object idPedidoCli = rgvGridPedidos.CurrentRow.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value;
                    DataAccess.InsertarComentario(comentario.ToString(), "PCC", int.Parse(idPedidoCli.ToString()), message.path, message.nombre, User.IdOperario, idMotivoComentario);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void agregarComentarioOrdenMenuItem_Event(object sender, EventArgs e)
        {
            log.Info("Agregando comentario orden ");
            try
            {
                RumMessageInputBox message = new RumMessageInputBox(Lenguaje.traduce("Comentarios orden"), "Introduzca comentario", "ORC");
                DialogResult resp = message.ShowDialog();
                if (resp == DialogResult.OK)
                {
                    object comentario = message.input;
                    string idMotivoComentario = message.idMotivo;
                    if (comentario == null || comentario.ToString().Equals(""))
                    {
                        if (message.path == null || message.path.Equals(""))
                        {
                            if (idMotivoComentario == null || idMotivoComentario.Equals(""))
                            {
                                return;
                            }
                            else
                            {
                                comentario = idMotivoComentario;
                            }
                        }
                        else
                        {
                            comentario = Lenguaje.traduce("Imagen");
                        }
                    }

                    object idOrdenRecogida = rgvGridOrdenes.CurrentRow.Cells["" + Lenguaje.traduce(strings.NumOrdenRecogida) + ""].Value;
                    DataAccess.InsertarComentario(comentario.ToString(), "ORC", int.Parse(idOrdenRecogida.ToString()), message.path, message.nombre, User.IdOperario, idMotivoComentario);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void agregarComentarioOrdenCargaMenuItem_Event(object sender, EventArgs e)
        {
            log.Info("Agregando comentario carga ");
            try
            {
                RumMessageInputBox message = new RumMessageInputBox(Lenguaje.traduce("Comentarios Carga"), "Introduzca comentario", "OCC");
                DialogResult resp = message.ShowDialog();
                if (resp == DialogResult.OK)
                {
                    object comentario = message.input;
                    string idMotivoComentario = message.idMotivo;
                    if (comentario == null || comentario.ToString().Equals(""))
                    {
                        if (message.path == null || message.path.Equals(""))
                        {
                            if (idMotivoComentario == null || idMotivoComentario.Equals(""))
                            {
                                return;
                            }
                            else
                            {
                                comentario = idMotivoComentario;
                            }
                        }
                        else
                        {
                            comentario = Lenguaje.traduce("Imagen");
                        }
                    }

                    object idCarga = rgvGridCarga.CurrentRow.Cells["" + Lenguaje.traduce("Num Carga") + ""].Value;
                    DataAccess.InsertarComentario(comentario.ToString(), "OCC", int.Parse(idCarga.ToString()), message.path, message.nombre, User.IdOperario, idMotivoComentario);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void RgvGridPedidos_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if (e.ContextMenuProvider == null) return;
            GridCellElement cellElement = e.ContextMenuProvider as GridCellElement;
            if (cellElement == null || cellElement.RowInfo is GridViewFilteringRowInfo || cellElement.RowInfo is GridViewTableHeaderRowInfo)
            {
                return;
            }
            if (cellElement.ColumnInfo == null || cellElement.ColumnInfo.HeaderText == null)
            {
                return;
            }
            if (e.ContextMenuProvider is GridDataCellElement)
            {
                if ((e.ContextMenuProvider as GridDataCellElement).RowIndex < 0) return;
                for (int i = 0; i < e.ContextMenu.Items.Count; i++)
                {
                    if (!e.ContextMenu.Items[i].Text.Equals("Copy") && !e.ContextMenu.Items[i].Text.Equals("Copiar"))
                    {
                        e.ContextMenu.Items[i].Visibility = ElementVisibility.Collapsed;
                    }
                }
            }
            if (sender == rgvGridPedidos)
            {
                e.ContextMenu = contextMenuPedidos.DropDown;
            }
            else if (sender == rgvGridOrdenes)
            {
                e.ContextMenu = contextMenuOrdenes.DropDown;
            }
            else if (sender == rgvGridCarga)
            {
                e.ContextMenu = contextMenuCarga.DropDown;
            }
        }

        private void iniciarTimer()
        {
            FuncionesGenerales.iniciarTimer(ref timerRefrescar);
            if (timerRefrescar != null)
                timerRefrescar.Tick += TimerRefrescar_Tick;
        }

        private void refrescarTimer()
        {
            try
            {
                if (this.timerRefrescar != null)
                {
                    this.timerRefrescar.Stop();
                    this.timerRefrescar.Start();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void TimerRefrescar_Tick(object sender, EventArgs e)
        {
            if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
            {
                btnVistaPedidosRefrescar.addRefrescoTimerColor();
                btnVistaPedidosRefrescar.enableRefrescoTimerColor();
            }
            else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
            {
                btnVistaOrdenesRefrescar.addRefrescoTimerColor();
                btnVistaOrdenesRefrescar.enableRefrescoTimerColor();
            }
            else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rdPanelPLPteCarga)
            {
                btnVistaCargaRefrescar.addRefrescoTimerColor();
                btnVistaCargaRefrescar.enableRefrescoTimerColor();
            }
        }

        private void addEliminarLayoutOpciones()
        {
            FuncionesGenerales.RumDropDownAddManual(ref btnVistaPedidosConfiguracion, 30061);
            FuncionesGenerales.AddEliminarLayoutButton(ref btnVistaPedidosConfiguracion);
            if (this.btnVistaPedidosConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.btnVistaPedidosConfiguracion.Items["RadMenuItemEliminarLayout"].Click += eliminarLayout;
            }

            FuncionesGenerales.RumDropDownAddManual(ref btnVistaOrdenesConfiguracion, 30061);
            FuncionesGenerales.AddEliminarLayoutButton(ref btnVistaOrdenesConfiguracion);
            if (this.btnVistaOrdenesConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.btnVistaOrdenesConfiguracion.Items["RadMenuItemEliminarLayout"].Click += eliminarLayout;
            }

            FuncionesGenerales.AddEliminarLayoutButton(ref btnVistaCargaConfiguracion);
            if (this.btnVistaCargaConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.btnVistaCargaConfiguracion.Items["RadMenuItemEliminarLayout"].Click += eliminarLayout;
            }
        }

        private void eliminarLayout(object e, EventArgs a)
        {
            if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
            {
                this.Name = "OrdenesCliente";
                FuncionesGenerales.EliminarLayout(this.Name + "GridView.xml", rgvGridPedidos);
            }
            else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
            {
                this.Name = "OrdenesRecogida";
                FuncionesGenerales.EliminarLayout(this.Name + "GridView.xml", rgvGridOrdenes);
            }
            else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rdPanelPLPteCarga)
            {
                this.Name = "OrdenesCarga";
                string pathP = this.Name + "GridView.xml";
                string pathContenido = "OrdenesCargaContenidoGridView.xml";
                string pathPacking = "OrdenesPackingGridView.xml";
                string pathPackingPte = "OrdenesPackingPteGridView.xml";
                string pathOrdenesCargaPedidos = "OrdenesCargaPedidosGridView.xml";

                string pathGeneral = "";
                try
                {
                    log.Info("Pulsado botón eliminarLayout " + DateTime.Now);

                    pathGeneral = "";

                    VentanaGuardarEstilo vge = new VentanaGuardarEstilo(3);
                    vge.ShowDialog();

                    if (VentanaGuardarEstilo.guardar == 0)
                    {
                        pathGeneral = Persistencia.DirectorioGlobal;
                    }
                    else if (VentanaGuardarEstilo.guardar > 0)
                    {
                        pathGeneral = Persistencia.DirectorioLocal;
                    }
                    else if (VentanaGuardarEstilo.guardar == -1)
                    {
                        return;
                    }

                    if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    {
                        pathGeneral += "\\Español";
                    }
                    else
                    {
                        pathGeneral += "\\Ingles";
                    }

                    pathP = pathGeneral + "\\" + pathP;
                    pathContenido = pathGeneral + "\\" + pathContenido;
                    pathPacking = pathGeneral + "\\" + pathPacking;
                    pathPackingPte = pathGeneral + "\\" + pathPackingPte;
                    pathOrdenesCargaPedidos = pathGeneral + "\\" + pathOrdenesCargaPedidos;
                    String mensajeNoExiste = "";
                    if (!File.Exists(pathP))
                    {
                        mensajeNoExiste += Lenguaje.traduce("No existe el archivo " + pathP) + "\n";
                        log.Debug("No se ha eliminado el estilo: " + pathP + " ya que no existe");
                    }
                    if (!File.Exists(pathContenido))
                    {
                        mensajeNoExiste += Lenguaje.traduce("No existe el archivo " + pathContenido) + "\n";
                        log.Debug("No se ha eliminado el estilo: " + pathContenido + " ya que no existe");
                    }
                    if (!File.Exists(pathPacking))
                    {
                        mensajeNoExiste += Lenguaje.traduce("No existe el archivo " + pathPacking) + "\n";
                        log.Debug("No se ha eliminado el estilo: " + pathPacking + " ya que no existe");
                    }
                    if (!File.Exists(pathPackingPte))
                    {
                        mensajeNoExiste += Lenguaje.traduce("No existe el archivo " + pathPackingPte) + "\n";
                        log.Debug("No se ha eliminado el estilo: " + pathPackingPte + " ya que no existe");
                    }
                    if (!File.Exists(pathOrdenesCargaPedidos))
                    {
                        mensajeNoExiste += Lenguaje.traduce("No existe el archivo " + pathOrdenesCargaPedidos) + "\n";
                        log.Debug("No se ha eliminado el estilo: " + pathOrdenesCargaPedidos + " ya que no existe");
                    }
                    if (!mensajeNoExiste.Equals("")) RadMessageBox.Show(mensajeNoExiste, "Eliminar estilo");
                    DialogResult dr = RadMessageBox.Show("¿" + Lenguaje.traduce("Estas seguro de querer eliminar el archivo ") + pathP + "?", "Eliminar estilo", MessageBoxButtons.YesNo);
                    if (dr.Equals(DialogResult.Yes))
                    {
                        File.Delete(pathP);
                        File.Delete(pathContenido);
                        File.Delete(pathPacking);
                        File.Delete(pathPackingPte);
                        File.Delete(pathOrdenesCargaPedidos);
                        log.Info("Se ha eliminado el archivo " + pathP);
                        log.Info("Se ha eliminado el archivo " + pathContenido);
                        log.Info("Se ha eliminado el archivo " + pathPacking);
                        log.Info("Se ha eliminado el archivo " + pathPackingPte);
                        log.Info("Se ha eliminado el archivo " + pathOrdenesCargaPedidos);

                        RadMessageBox.Show(Lenguaje.traduce("Eliminación completada"), Lenguaje.traduce("Resultado"));

                        return;
                    }
                    return;
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Error al eliminar el estilo"));
                    log.Error("Error eliminando el archivo " + pathP + "  " + ex.Message + " \n " + ex.StackTrace);
                }
            }
        }

        private void OrdenesRecogida_Load(object sender, EventArgs e)
        {
            try
            {
                //Añadimos jerarquicos vista pedidos
                GridViewTemplate gtLineasPedido = createChildTemplateJerarquicoLineasPedido();
                gtLineasPedido.Caption = Lenguaje.traduce(strings.LineasPedido);
                this.rgvGridPedidos.Templates.Add(gtLineasPedido);

                GridViewTemplate gtOrdenesRecogida = CreateChildTemplateJerarquicoRecogidaCab();
                gtOrdenesRecogida.Caption = Lenguaje.traduce(strings.OrdenesRecogida);
                this.rgvGridPedidos.Templates.Add(gtOrdenesRecogida);

                GridViewTemplate gtOrdenesCarga = CreateChildTemplateJerarquicoCargaCab();
                gtOrdenesCarga.Caption = Lenguaje.traduce(strings.OrdenesCarga);
                this.rgvGridPedidos.Templates.Add(gtOrdenesCarga);

                //Añadimos jerarquicos a ordenes de recogida
                GridViewTemplate lineasRecogida = CreateChildTemplateJerarquicoLineasRecogida();
                lineasRecogida.Caption = Lenguaje.traduce(strings.LineasOrden);
                this.rgvGridOrdenes.Templates.Add(lineasRecogida);

                GridViewTemplate tareas = CreateChildTemplateJerarquicoTareas();
                tareas.Caption = Lenguaje.traduce(strings.Tareas);
                this.rgvGridOrdenes.Templates.Add(tareas);

                GridViewTemplate reservas = CreateChildTemplateJerarquicoReservas();
                reservas.Caption = Lenguaje.traduce(strings.Reservas);
                this.rgvGridOrdenes.Templates.Add(reservas);

                GridViewTemplate salidas = CreateChildTemplateJerarquicoSalidas();
                salidas.Caption = Lenguaje.traduce(strings.Salidas);
                this.rgvGridOrdenes.Templates.Add(salidas);

                GridViewTemplate packing = CreateChildTemplateJerarquicoPackingList();
                packing.Caption = Lenguaje.traduce(strings.PackingList);
                this.rgvGridOrdenes.Templates.Add(packing);

                GridViewTemplate movBulto = CreateChildTemplateJerarquicoMovBulto();
                movBulto.Caption = Lenguaje.traduce(strings.MovimientosBulto);
                this.rgvGridOrdenes.Templates.Add(movBulto);
                InicializarQuerysPrecompiladas();
                //CAMBIO rgvGridPrincipal.Templates[0].Columns["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].IsVisible = false;
                rgvGridOrdenes.Templates[0].Columns.Add(chkColJerarquicoORLineas);
                rgvGridOrdenes.Templates[0].Columns.Move(chkColJerarquicoORLineas.Index, 0);
                //rgvGridOrdenes.Templates[2].Columns.Add(chkColJerarquicoORReservas);
                //rgvGridOrdenes.Templates[2].Columns.Move(chkColJerarquicoORReservas.Index, 0);

                //añadimos columnas calculadas a la vista de pedidos
                EstablecerNombresColumnasAñadidas();
                //rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce("Cantidad")].TextAlignment = ContentAlignment.BottomRight;
                //rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce("Cantidad Unidad")].TextAlignment = ContentAlignment.BottomRight;
                //rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce("Cantidad Minima")].TextAlignment = ContentAlignment.BottomRight;
                //rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce("Cantidad Completo")].TextAlignment = ContentAlignment.BottomRight;
                //rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce("Num Palets Completos")].TextAlignment = ContentAlignment.BottomRight;
                //rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce("Num Palets Picking")].TextAlignment = ContentAlignment.BottomRight;
                //rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce("Num Palets")].TextAlignment = ContentAlignment.BottomRight;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void InicializarEsquemas()
        {
            try
            {
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio Carga Esquemas iniciales");
                Business.GetOrdenesRecogidaEsquema(ref _lstEsquemaTablaPedidos);
                Business.GetOrdenesRecogidaCabEsquema(ref _lstEsquemaTablaOrdenesRecogida);
                Business.GetOrdenRecogidaExpedicionesEsquema(ref _lstEsquemaTablaOrdenesCarga);
                Business.GetOrdenesCargaJerarquicoPackingListEsquema(ref _lstEsquemaTablaCargaPacking);
                Business.GetOrdenesCargaJerarquicoPedidosEsquema(ref _lstEsquemaTablaCargaPedidos);
                Business.GetOrdenesCargaJerarquicoContenidoEsquema(ref _lstEsquemaTablaCargaContenido);
                Business.GetOrdenesRecogidaCabEsquema(ref _lstEsquemaTablaOrdenesRecogida);
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin Carga Esquemas iniciales");
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void InicializarQuerysPrecompiladas()
        {
            try
            {
                // Sacamos el where de la consulta inicial de los pedidos para saber cargar el datatable filtrado
                string[] campos = Business.GetFieldsSQLOrdenesRecogida(_lstEsquemaTablaPedidos);
                string where = campos[2];
                // Cargamos el datatable inicial de lineas reservadas
                //string queryReservadas = Business.getQueryLineasReservadasPedido(where);
                //dtLineasReservadas = ConexionSQL.getDataTable(queryReservadas);
                //// Cargamos el datatable inicial de lineas preparadas
                //string queryPreparadas = Business.getQueryLineasPreparadasPedido(where);
                //dtLineasPreparadas = ConexionSQL.getDataTable(queryPreparadas);
                //// Cargamos el datatable inicial datos pedido
                //string queryPalets = Business.getQueryPaletsPickingsDuracionEstimada(where);
                //dtPalets = ConexionSQL.getDataTable(queryPalets);
                //Monica lo pasamos al json
                //string queryPaletsLineas = Business.getQueryPaletsPickingsLineas(where);
                //dtPaletsLineas = ConexionSQL.getDataTable(queryPaletsLineas);
                // Cargamos el datatable inicial tareas pendientes pedido
                //string queryTareasPtes = Business.getQueryTareasPendientesPedido(where);
                //dtTareasPendientes = ConexionSQL.getDataTable(queryTareasPtes);
                //string queryReservadaServidaLineas = Business.getQueryQtyResSerLineasPedido(where);
                //dtQtyResSerLineas = ConexionSQL.getDataTable(queryReservadaServidaLineas);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion Constructor

        #region GenerarGrids

        private void GenerarGridPedidos()
        {
            try
            {
                InicializarQuerysPrecompiladas();
                Utilidades.refrescarJerarquico(ref this.rgvGridPedidos, -1);
                rgvGridPedidos.Columns.Clear();
                ribbonTabAccionesPedidos.Enabled = true;
                ribbonTabAccionesOrdenesRecogida.Enabled = false;
                ribbonTabAccionesOrdenesCarga.Enabled = false;
                ribbonTabAccionesPedidos.Select();
                ribbonTabAccionesPedidos.Visibility = ElementVisibility.Visible;
                ribbonTabAccionesOrdenesRecogida.Visibility = ElementVisibility.Collapsed;
                ribbonTabAccionesOrdenesCarga.Visibility = ElementVisibility.Collapsed;
                this.SuspendLayout();
                bgWorkerGridViewPedidos = new BackgroundWorker();
                bgWorkerGridViewPedidos.WorkerReportsProgress = true;
                bgWorkerGridViewPedidos.WorkerSupportsCancellation = true;
                bgWorkerGridViewPedidos.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewPedidos_RunWorkerCompleted);
                bgWorkerGridViewPedidos.DoWork += new DoWorkEventHandler(llenarGridPedidos);
                radWaitPedidos.StartWaiting();
                bgWorkerGridViewPedidos.RunWorkerAsync();
                if (btnVistaPedidosRefrescar != null)
                    btnVistaPedidosRefrescar.disableRefrescoTimerColor();
                refrescarTimer();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void GenerarGridOrdenesRecogida()
        {
            try
            {
                Utilidades.refrescarJerarquico(ref rgvGridOrdenes, -1);
                InicializarQuerysPrecompiladas();
                rgvGridOrdenes.Columns.Clear();
                ribbonTabAccionesPedidos.Enabled = false;
                ribbonTabAccionesOrdenesRecogida.Enabled = true;
                ribbonTabAccionesOrdenesCarga.Enabled = false;
                ribbonTabAccionesOrdenesRecogida.Select();
                ribbonTabAccionesOrdenesRecogida.Visibility = ElementVisibility.Visible;
                ribbonTabAccionesPedidos.Visibility = ElementVisibility.Collapsed;
                ribbonTabAccionesOrdenesCarga.Visibility = ElementVisibility.Collapsed;
                bgWorkerGridViewOrdenes = new BackgroundWorker();
                bgWorkerGridViewOrdenes.WorkerReportsProgress = true;
                bgWorkerGridViewOrdenes.WorkerSupportsCancellation = true;
                bgWorkerGridViewOrdenes.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewOrdenes_RunWorkerCompleted);
                bgWorkerGridViewOrdenes.DoWork += new DoWorkEventHandler(llenarGridRecogida);
                radWaitOrdenes.StartWaiting();
                this.SuspendLayout();
                bgWorkerGridViewOrdenes.RunWorkerAsync();

                refrescarTimer();
                if (btnVistaOrdenesRefrescar != null)
                    this.btnVistaOrdenesRefrescar.disableRefrescoTimerColor();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private int getIdCargaSeleccionada()
        {
            return getIdCargaSeleccionada(-1);
        }

        private int getIdCargaSeleccionada(int quitarCargaSeleccionadaAnterior)
        {
            try
            {
                if (rgvGridCarga == null)
                {
                    log.Error("Error al identificar la cargaSeleccionada, el rgvGridCarga vale null");
                }

                if (rgvGridCarga.Columns.Count < 1) return -1;
                //En cierto caso hay un momento en el que llega aquí y aunque SÍ existe el checkbox
                //no tiene el nombre correcto.
                int posicionCheckBox = -1;
                if (rgvGridCarga.Columns["chkColOrdenesCarga"] == null)
                {
                    posicionCheckBox = Utilidades.buscarDondeEstaCheckBox(ref rgvGridCarga);
                    if (posicionCheckBox > -1)
                    {
                        rgvGridCarga.Columns[posicionCheckBox].Name = "chkColOrdenesCarga";
                    }
                    else
                    {
                        log.Error("No hay checkBox en rgvGridCarga en su paso por getIdCargaSeleccionada(" +
                            quitarCargaSeleccionadaAnterior + ")");
                        return -1;
                    }
                }

                for (int i = 0; i < rgvGridCarga.RowCount; i++)
                {
                    if (rgvGridCarga.Rows[i].Cells["chkColOrdenesCarga"] == null)
                    {
                        break;
                    }
                    if (rgvGridCarga.Rows[i].Cells["chkColOrdenesCarga"].Value == null)
                    {
                        FuncionesGenerales.setCheckBoxFalse(ref rgvGridCarga, "chkColOrdenesCarga");
                        break;
                    }
                    if (rgvGridCarga.Rows[i].Cells["chkColOrdenesCarga"].Value.ToString().Equals("True"))
                    {
                        int carga = int.Parse(rgvGridCarga.Rows[i].Cells[Lenguaje.traduce("Num Carga")].Value.ToString());
                        datosCargaTotales = datosTotalesCarga(carga);
                        lblVolumenCant.Text = datosCargaTotales[1].ToString();
                        lblUdsCant.Text = datosCargaTotales[0].ToString();
                        lblPesoCant.Text = datosCargaTotales[2].ToString();
                        lblPaletsCount.Text = datosCargaTotales[3];
                        if (quitarCargaSeleccionadaAnterior > 0 && quitarCargaSeleccionadaAnterior == carga)
                        {
                            continue;
                        }
                        return carga;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error producido al intentar coger la carga actual en Expediciones, cargas.\n" + ex.Message);
            }
            return -1;
        }

        private void GenerarGridOrdenesCarga()
        {
            try
            {
                cargaSeleccionadaWorker = getIdCargaSeleccionada();
                ribbonTabAccionesPedidos.Enabled = false;
                ribbonTabAccionesOrdenesRecogida.Enabled = false;
                ribbonTabAccionesOrdenesCarga.Enabled = true;
                ribbonTabAccionesOrdenesCarga.Select();
                ribbonTabAccionesOrdenesCarga.Visibility = ElementVisibility.Visible;
                ribbonTabAccionesOrdenesRecogida.Visibility = ElementVisibility.Collapsed;
                ribbonTabAccionesPedidos.Visibility = ElementVisibility.Collapsed;
                //CargaPanelArbolCarga();
                bgWorkerGridViewCarga = new BackgroundWorker();
                bgWorkerGridViewCarga.WorkerReportsProgress = true;
                bgWorkerGridViewCarga.WorkerSupportsCancellation = true;
                bgWorkerGridViewCarga.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewCarga_RunWorkerCompleted);
                bgWorkerGridViewCarga.DoWork += new DoWorkEventHandler(llenarGridCarga);
                radWaitCarga.StartWaiting();
                this.SuspendLayout();
                bgWorkerGridViewCarga.RunWorkerAsync();
                ElegirEstilo();

                bgWorkerGridViewPLPteCarga = new BackgroundWorker();
                bgWorkerGridViewPLPteCarga.WorkerReportsProgress = true;
                bgWorkerGridViewPLPteCarga.WorkerSupportsCancellation = true;
                bgWorkerGridViewPLPteCarga.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewCargaPLPte_RunWorkerCompleted);
                bgWorkerGridViewPLPteCarga.DoWork += new DoWorkEventHandler(llenarGridCargaPLPte);
                radWaitCargaPLPte.StartWaiting();
                this.SuspendLayout();
                bgWorkerGridViewPLPteCarga.RunWorkerAsync();
                refrescarTimer();

                if (btnVistaCargaRefrescar != null)
                    this.btnVistaCargaRefrescar.disableRefrescoTimerColor();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion GenerarGrids

        #region ColoresColumnas

        private void colorLanzamiento(GridViewRowInfo row)
        {
            try
            {
                int valor = 0;
                if (row.Cells["" + Lenguaje.traduce(strings.ColorLanzamiento) + ""] == null ||
                    row.Cells["" + Lenguaje.traduce(strings.ColorLanzamiento) + ""].Value == DBNull.Value)
                {
                    return;
                }
                valor = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.ColorLanzamiento) + ""].Value);

                if (valor == 0)
                {
                    /*row.Cells["" + Lenguaje.traduce(strings.ColorLanzamiento) + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce(strings.ColorLanzamiento) + ""].Style.BackColor = Color.FromArgb(-13436544);*/
                }
                else
                {
                    row.Cells["" + Lenguaje.traduce("Tipo Lanzamiento")].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Tipo Lanzamiento") + ""].Style.BackColor = Color.FromArgb(valor);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void colorCliente(GridViewRowInfo row)
        {
            try
            {
                int valor = 0;
                if (row.Cells["" + Lenguaje.traduce(strings.ColorCliente) + ""].Value == DBNull.Value) { }
                else
                {
                    valor = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.ColorCliente) + ""].Value);
                }
                if (valor == 0)
                {
                    /*row.Cells["" + Lenguaje.traduce(strings.ColorCliente) + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce(strings.ColorCliente) + ""].Style.BackColor = Color.FromArgb(-13436544);*/
                }
                else
                {
                    row.Cells["" + Lenguaje.traduce("Codigo Cliente")].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Codigo Cliente")].Style.BackColor = Color.FromArgb(valor);
                    row.Cells["" + Lenguaje.traduce("Cliente")].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Cliente")].Style.BackColor = Color.FromArgb(valor);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void colorRutas(GridViewRowInfo row)
        {
            try
            {
                int valor = 0;

                if (row.Cells["" + Lenguaje.traduce(strings.ColorRutas) + ""].Value == DBNull.Value) { }
                else
                {
                    valor = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.ColorRutas) + ""].Value);
                }
                if (valor == 0)
                {
                    /*row.Cells["" + Lenguaje.traduce(strings.ColorRutas) + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce(strings.ColorRutas) + ""].Style.BackColor = Color.FromArgb(-13436544);*/
                }
                else
                {
                    row.Cells["" + Lenguaje.traduce("Codigo Ruta")].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Codigo Ruta")].Style.BackColor = Color.FromArgb(valor);
                    row.Cells["" + Lenguaje.traduce("Ruta")].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Ruta") + ""].Style.BackColor = Color.FromArgb(valor);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void colorAgencia(GridViewRowInfo row)
        {
            try
            {
                int valor = 0;

                if (row.Cells["" + Lenguaje.traduce(strings.ColorAgencia) + ""].Value == DBNull.Value) { }
                else
                {
                    valor = Convert.ToInt32(row.Cells["" + strings.ColorAgencia + ""].Value);
                }
                if (valor == 0)
                {
                    /*row.Cells["" + Lenguaje.traduce(strings.ColorAgencia) + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce(strings.ColorAgencia) + ""].Style.BackColor = Color.FromArgb(-13436544);*/
                }
                else
                {
                    row.Cells["" + Lenguaje.traduce("Codigo Agencia")].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Codigo Agencia")].Style.BackColor = Color.FromArgb(valor);
                    row.Cells["" + Lenguaje.traduce("Agencia")].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Agencia")].Style.BackColor = Color.FromArgb(valor);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void colorEstado(GridViewRowInfo row)
        {
            try
            {
                int valor = 0;

                if ((row.Cells["" + Lenguaje.traduce(strings.EstadoColor) + ""].Value == DBNull.Value)) { }
                else
                {
                    valor = Convert.ToInt32(row.Cells[Lenguaje.traduce(strings.EstadoColor)].Value);
                }
                if (valor == 0)
                {
                    /*row.Cells["" + Lenguaje.traduce("Estado Descripcion") + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Estado Descripcion") + ""].Style.BackColor = Color.FromArgb(-13436544);
                    row.Cells["" + Lenguaje.traduce("Estado Pedido") + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Estado Pedido") + ""].Style.BackColor = Color.FromArgb(-13436544);*/
                }
                else
                {
                    row.Cells["" + Lenguaje.traduce("Estado Descripcion") + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Estado Descripcion") + ""].Style.BackColor = Color.FromArgb(valor);
                    row.Cells["" + Lenguaje.traduce("Estado Pedido") + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Estado Pedido") + ""].Style.BackColor = Color.FromArgb(valor);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion ColoresColumnas

        #region Lenguajes

        private void ControlesLenguaje()
        {
            try
            {
                chkRecursoManual.Text = Lenguaje.traduce("Recurso Manual");
                rBtnddPedidosfiltradoOpciones.Text = Lenguaje.traduce("Filtrado por");
                this.tabPackingList.Text = Lenguaje.traduce("Packing List");
                this.tabPedidos.Text = Lenguaje.traduce("Pedidos");
                this.tabSalidas.Text = Lenguaje.traduce("Contenido");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Lenguajes

        #region Llamada Eventos

        private void LlamadaEventos()
        {
            log.Debug("Principio llamadaEventos en OrdenesRecogida");
            rgvGridPedidos.ContextMenuOpening += radGridView_ContextMenuOpening;
            rgvGridOrdenes.ContextMenuOpening += radGridView_ContextMenuOpening;
            rgvGridCarga.ContextMenuOpening += radGridView_ContextMenuOpening;
            rgvGridCarga.ValueChanged += rgvCargaGrid_ValueChanged;
            rgvGridPedidos.FilterChanged += radGridView_FilterChanged;
            rgvGridOrdenes.FilterChanged += radGridView_FilterChanged;
            rgvGridCarga.FilterChanged += radGridView_FilterChanged;
            rgvGridPedidos.RowSourceNeeded += radGridView_RowSourceNeeded;
            rgvGridOrdenes.RowSourceNeeded += radGridView_RowSourceNeeded;
            rgvGridOrdenes.ViewCellFormatting += radGridView_ViewCellFormattingHierarchicalGrid;
            rgvGridPedidos.ViewCellFormatting += rgvGridPedidos_ViewCellFormatting;
            rgvGridCarga.ViewCellFormatting += rgvGridCarga_ViewCellFormatting;
            rgvGridCarga.ViewRowFormatting += radGridView_ViewRowFormatting;

            btnDerecha.Click += btnDerecha_Click;
            btnIzquierda.Click += btnIzquierda_Click;
            log.Debug("Fin llamadaEventos en OrdenesRecogida");
        }

        private void rgvGridOrdenes_CellClick(object sender, GridViewCellEventArgs e)
        {
        }

        #endregion Llamada Eventos

        #region Iniciar Datos

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        private void AñadirLabelCantidad()
        {
            lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            grupoLabel.Items.Add(lblCantidad);
            this.ribbonTabAccionesPedidos.Items.AddRange(grupoLabel);
        }

        private void IniciarDatos()
        {
            try
            {
                rgvGridPedidos.Dock = DockStyle.Fill;
                //Vista pedidos
                rgvGridOrdenes.Dock = DockStyle.Fill;
                chkColPedidos.Name = "checkColumn";
                chkColPedidos.EnableHeaderCheckBox = true;
                chkColPedidos.HeaderText = "";
                chkColPedidos.EditMode = EditMode.OnValueChange;
                chkColPedidos.CheckFilteredRows = false;
                //Vista Carga
                tableLayoutPanelCarga.SetColumnSpan(rgvGridOrdenes, tableLayoutPanelCarga.ColumnCount);
                tableLayoutPanelCarga.Dock = DockStyle.Fill;
                rgvGridCarga.Dock = DockStyle.Fill;
                chkColOrdenesCarga.Name = "chkColOrdenesCarga";
                chkColOrdenesCarga.EnableHeaderCheckBox = false;
                chkColOrdenesCarga.HeaderText = "";
                chkColOrdenesCarga.EditMode = EditMode.OnValueChange;

                chkColPLPteCarga.Name = "chkColPLPteCarga";
                chkColPLPteCarga.EnableHeaderCheckBox = true;
                chkColPLPteCarga.HeaderText = "";
                chkColPLPteCarga.EditMode = EditMode.OnValueChange;
                chkColPLPteCarga.CheckFilteredRows = false;
                chkColPLPteCarga.EnableHeaderCheckBox = true;

                chkColOrdenesRecogida.Name = "chkColOrdenesRecogida";
                chkColOrdenesRecogida.EnableHeaderCheckBox = true;
                chkColOrdenesRecogida.HeaderText = "";
                chkColOrdenesRecogida.EditMode = EditMode.OnValueChange;

                chkColJerarquicoORLineas.Name = "chkColJerarquicoORLineas";
                chkColJerarquicoORLineas.EnableHeaderCheckBox = true;
                chkColJerarquicoORLineas.DataType = typeof(int);
                chkColJerarquicoORLineas.HeaderText = "";

                chkColJerarquicoOCPackingList.Name = "chkColJerarquicoOCPackingList";
                chkColJerarquicoOCPackingList.EnableHeaderCheckBox = true;
                chkColJerarquicoOCPackingList.HeaderText = "";
                chkColJerarquicoOCPackingList.EditMode = EditMode.OnValueChange;
                chkColJerarquicoOCPackingList.CheckFilteredRows = false;

                this.radWaitPedidos.Name = "rWBarPedidos";
                this.radWaitPedidos.Size = new System.Drawing.Size(200, 20);
                this.radWaitPedidos.TabIndex = 2;
                this.radWaitPedidos.Text = "rWBarPedidos";
                this.radWaitPedidos.AssociatedControl = this.rgvGridPedidos;
                this.radWaitOrdenes.Name = "rWBarOrdenes";
                this.radWaitOrdenes.Size = new System.Drawing.Size(200, 20);
                this.radWaitOrdenes.TabIndex = 2;
                this.radWaitOrdenes.Text = "rWBarOrdenes";
                this.radWaitOrdenes.AssociatedControl = this.rgvGridOrdenes;
                this.radWaitCarga.Name = "rWBarCarga";
                this.radWaitCarga.Size = new System.Drawing.Size(200, 20);
                this.radWaitCarga.TabIndex = 2;
                this.radWaitCarga.Text = "rWBarCarga";
                this.radWaitCarga.AssociatedControl = this.rgvGridCarga;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Iniciar Datos

        #region CreateChildTemplate

        private GridViewTemplate createChildTemplateJerarquicoLineasPedido()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.LineasPedido);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasClienteLineas");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            //template.ReadOnly = true;

            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            //template.Columns.Add(paletsCompletosJerarquico.ColumnName);
            //template.Columns.Add(numPickingsJerarquico.ColumnName);
            try
            {
                template.Columns.Remove(colpctCantidadServida.FieldName);
                template.Columns.Remove(colpctCantidadReservada.FieldName);
            }
            catch (Exception ex)
            {
            }
            template.Columns.Add(colpctCantidadServida);
            template.Columns.Add(colpctCantidadReservada);
            //template.Columns.Add(cantidadServidaJerarquico.ColumnName);
            //template.Columns.Add(cantidadReservadaJerarquico.ColumnName);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoRecogidaCab()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.OrdenesRecogida);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasCabJerarquico");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);

            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoCargaCab()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.OrdenesCarga);
            string jsonEntradas = DataAccess.LoadJson("OrdenCargaCabJerarquico");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoTareas()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.TareasPendientes);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasTareas");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);

            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoSalidas()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.Salidas);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasSalidas");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoLineasRecogida()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.LineasOrden);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasLineas");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoReservas()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.Reservas);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasReservas");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            //template.ReadOnly = true;
            /*template.ReadOnly = false;
            template.AllowAddNewRow = false;
            template.AllowEditRow = true;            */
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            //template.Columns.Add(chkColJerarquicoORReservas);
            //template.Columns.Move(chkColJerarquicoORReservas.Index, 0);
            //chkColJerarquicoORReservas.ReadOnly = false;
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);

            template.BestFitColumns();

            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoPackingList()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.PackingList);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasPackingList");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            //template.ReadOnly = true;
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        private GridViewTemplate CreateChildTemplateJerarquicoMovBulto()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.MovimientosBulto);
            string jsonEntradas = DataAccess.LoadJson("OrdenRecogidasMovBulto");
            JArray jsEntradas = JArray.Parse(jsonEntradas);
            string jsonEsquemaEntradas = jsEntradas.First()["Scheme"].ToString();
            List<TableScheme> esquemaEntradas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaEntradas);
            for (int i = 0; i < esquemaEntradas.Count; i++)
            {
                if (esquemaEntradas[i].Etiqueta != string.Empty && esquemaEntradas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaEntradas[i].Etiqueta);
                    if (!esquemaEntradas[i].EsVisible)
                    {
                        template.Columns[esquemaEntradas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            Utilidades.ReadOnlyTrueExceptoCheckBoxTemplate(ref template);
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.BestFitColumns();
            return template;
        }

        #endregion CreateChildTemplate

        #region Eventos

        private void radGridView_ContextMenuOpening(object sender, Telerik.WinControls.UI.ContextMenuOpeningEventArgs e)
        {
            try
            {
                for (int i = 0; i < e.ContextMenu.Items.Count; i++)
                {
                    if (e.ContextMenu.Items[i].Text == Lenguaje.traduce("Borrar Fila"))
                    {
                        e.ContextMenu.Items.Remove(e.ContextMenu.Items[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radGridView_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.HierarchyLevel == 1 && e.RowElement.RowInfo is GridViewNewRowInfo)
            {
                e.RowElement.RowInfo.MaxHeight = 1;
            }
        }

        private void rgvCargaGrid_ValueChanged(object sender, EventArgs e)
        {
            if (sender is RadCheckBoxEditor)
            {
                if (Convert.ToBoolean((sender as RadCheckBoxEditor).Value))
                    cargarCargaSeleccionada(-1);
                else
                {
                    (sender as RadCheckBoxEditor).Value = ToggleState.On;
                }
            }
        }

        private void cargarCargaSeleccionada(int cargaSel)
        {
            try
            {
                //Quitar la cargaPreviamente seleccionada.

                int _cargaIdSeleccionado = 0;
                if (cargaSel == -1)
                    _cargaIdSeleccionado = getIdCargaSeleccionada(cargaAnteriorSeleccionada);
                else
                    _cargaIdSeleccionado = cargaSel;

                llenarGridCargaHijos(_cargaIdSeleccionado);

                ElegirEstiloSoloCargaPl();
                LoadLayoutLocal();

                if (_cargaIdSeleccionado > 0)
                {
                    checkBoxCargaCheckear(_cargaIdSeleccionado);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void llenarGridCargaHijos(int cargaSel)
        {
            llenarGridCargaPacking(cargaSel);
            llenarGridCargaPedidos(cargaSel);
            llenarGridCargaContenido(cargaSel);
            Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCargaPackingList);
            Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCargaPedidos);
            Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCargaContenido);

            if (rgvGridCargaPedidos.Columns[Lenguaje.traduce("Sello")] != null)
            {
                rgvGridCargaPedidos.Columns[Lenguaje.traduce("Sello")].ReadOnly = false;
            }
            if (rgvGridCargaPackingList.Columns[Lenguaje.traduce("Orden")] != null)
            {
                rgvGridCargaPackingList.Columns[Lenguaje.traduce("Orden")].ReadOnly = false;
            }
        }

        private void radGridView_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    e.CellElement.TextWrap = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radGridView_CellFormattingWrapText(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement is GridHeaderCellElement)
                {
                    e.CellElement.TextWrap = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radGridView_RowSourceNeeded(object sender, GridViewRowSourceNeededEventArgs e)
        {
            try
            {
                this.SuspendLayout();
                this.Cursor = Cursors.WaitCursor;
                string idPedidoCli = string.Empty;
                string recogidaPrueba = string.Empty;
                if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                {
                    DataRowView rowView = e.ParentRow.DataBoundItem as DataRowView;
                    DataRow[] rows = rowView.Row.GetChildRows("" + Lenguaje.traduce(strings.NumPedidoCliente) + "");

                    //Está dando un error ya que intenta consultar el RowSourceNeeded de un pedido que ya no esta.
                    if (e.ParentRow.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""] != null)
                        idPedidoCli = e.ParentRow.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value.ToString();
                    else { return; };
                    log.Debug("Iniciando row source needed sobre el padre " + idPedidoCli);

                    if (e.Template.Caption == Lenguaje.traduce(strings.LineasPedido))
                    {
                        DataTable lineasPedido = Business.GetClientesPedidosLineasJerarquicoDatosGridView(idPedidoCli);
                        if (lineasPedido.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in lineasPedido.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < lineasPedido.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[lineasPedido.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (lineasPedido.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[lineasPedido.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[lineasPedido.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[lineasPedido.Columns[i].ColumnName].Value = dataRow[row.Cells[lineasPedido.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }
                                catch (Exception ex)

                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[i].ColumnInfo.HeaderText + " en la fila " + lineasPedido.Columns[0].ColumnName + " : " + dataRow[0].ToString());
                                }
                            }

                            e.SourceCollection.Add(row);
                        }
                    }
                    else if (e.Template.Caption == Lenguaje.traduce(strings.OrdenesRecogida))
                    {
                        DataTable recogidaCab = Business.GetRecogidaCabJerarquicoDatosGridView(idPedidoCli);
                        if (recogidaCab.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in recogidaCab.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < recogidaCab.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[recogidaCab.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (row.Cells[recogidaCab.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                        {
                                            if (recogidaCab.Columns[i].DataType == typeof(Decimal))
                                            {
                                                row.Cells[recogidaCab.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[recogidaCab.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                            }
                                            else
                                            {
                                                row.Cells[recogidaCab.Columns[i].ColumnName].Value = dataRow[row.Cells[recogidaCab.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)

                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[recogidaCab.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    else if (e.Template.Caption == Lenguaje.traduce(strings.OrdenesCarga))
                    {
                        DataTable cargaCab = Business.GetCargaCabDatosGridView(idPedidoCli);
                        if (cargaCab.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in cargaCab.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < cargaCab.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[cargaCab.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        row.Cells[cargaCab.Columns[i].ColumnName].Value = dataRow[row.Cells[cargaCab.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                    }
                                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[cargaCab.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                }

                if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                {
                    DataRowView rowViewRecogida = e.ParentRow.DataBoundItem as DataRowView;
                    DataRow[] rowsRecogida = rowViewRecogida.Row.GetChildRows("" + Lenguaje.traduce(strings.NumOrdenRecogida) + "");

                    //César: No da un error de programa pero cada vez que entra por aquí y da error, sobrecarga el log.
                    //Voy a meter está comprobación para que devuelva directamente.
                    if (e.ParentRow.Cells["" + Lenguaje.traduce(strings.NumOrdenRecogida) + ""] == null) return;
                    recogidaPrueba = e.ParentRow.Cells["" + Lenguaje.traduce(strings.NumOrdenRecogida) + ""].Value.ToString();
                    if (e.Template.Caption == Lenguaje.traduce(strings.LineasOrden))
                    {
                        DataTable lineasPedidoRecogida = Business.GetRecogidaPedidosLineasJerarquicoDatosGridView(recogidaPrueba);

                        if (lineasPedidoRecogida.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in lineasPedidoRecogida.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < lineasPedidoRecogida.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[lineasPedidoRecogida.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (lineasPedidoRecogida.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[lineasPedidoRecogida.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[lineasPedidoRecogida.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[lineasPedidoRecogida.Columns[i].ColumnName].Value = dataRow[row.Cells[lineasPedidoRecogida.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[lineasPedidoRecogida.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    else if (e.Template.Caption == Lenguaje.traduce(strings.Tareas))
                    {
                        DataTable tareas = Business.GetOrdenesRecogidaControlTareasJerarquicoDatosGridView(recogidaPrueba);

                        if (tareas.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in tareas.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < tareas.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[tareas.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        row.Cells[tareas.Columns[i].ColumnName].Value = dataRow[row.Cells[tareas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                    }
                                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[tareas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    else if (e.Template.Caption == Lenguaje.traduce(strings.Reservas))
                    {
                        DataTable reservas = Business.GetOrdenesRecogidaReservasJerarquicoDatosGridView(recogidaPrueba);

                        if (reservas.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in reservas.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < reservas.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[reservas.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (reservas.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[reservas.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[reservas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[reservas.Columns[i].ColumnName].Value = dataRow[row.Cells[reservas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[reservas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    else if (e.Template.Caption == Lenguaje.traduce(strings.Salidas))
                    {
                        DataTable salidas = Business.GetOrdenesRecogidaSalidasJerarquicoDatosGridView(recogidaPrueba);
                        if (salidas.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in salidas.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < salidas.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[salidas.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (salidas.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[salidas.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[salidas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[salidas.Columns[i].ColumnName].Value = dataRow[row.Cells[salidas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[salidas.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    else if (e.Template.Caption == Lenguaje.traduce(strings.PackingList))
                    {
                        DataTable packingList = Business.GetOrdenesRecogidaPackingListJerarquicoDatosGridView(recogidaPrueba);

                        if (packingList.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in packingList.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < packingList.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[packingList.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        row.Cells[packingList.Columns[i].ColumnName].Value = dataRow[row.Cells[packingList.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                    }
                                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[packingList.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    else if (e.Template.Caption == Lenguaje.traduce(strings.MovimientosBulto))
                    {
                        DataTable movBulto = Business.GetOrdenesRecogidaMovBultoJerarquicoDatosGridView(recogidaPrueba);

                        if (movBulto.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in movBulto.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < movBulto.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[movBulto.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        row.Cells[movBulto.Columns[i].ColumnName].Value = dataRow[row.Cells[movBulto.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                    }
                                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                                {
                                    log.Error("Error en RowSourceNeeded de Ordenes recogida, intentando insertar el campo " + row.Cells[movBulto.Columns[i].ColumnName].ColumnInfo.HeaderText + " en la fila " + dataRow[0].ToString());
                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    if (e.SourceCollection == null)
                    {
                        e.Template.AllowAddNewRow = true;
                    }
                    else if (e.Template.Rows.Count > 0)
                    {
                        e.Template.AllowAddNewRow = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            log.Info("Terminando RowSourceNeeded");
            this.ResumeLayout();
            this.Cursor = Cursors.Arrow;
        }

        protected void radGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
                if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                {
                    GridDataView dataView = rgvGridPedidos.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                    filtroAnteriorPedidos = rgvGridPedidos.FilterDescriptors;
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                {
                    GridDataView dataView = rgvGridOrdenes.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(1, 0) == rgvGridCarga)
                {
                    GridDataView dataView = rgvGridCarga.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void radGridView_ViewCellFormattingHierarchicalGrid(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                GridDetailViewCellElement detailsCell = e.CellElement as GridDetailViewCellElement;
                if (detailsCell != null)
                {
                    e.CellElement.Padding = new Padding(0);
                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.PaddingProperty, ValueResetFlags.Local);
                }
                if (e.ColumnIndex < 0 && e.RowIndex > -1 && e.CellElement is GridRowHeaderCellElement)
                {
                    if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                    {
                        if (rgvGridOrdenes.Rows[e.RowIndex].Cells[Lenguaje.traduce("Hay Comentarios")].Value.ToString() != "")
                        {
                            if (int.Parse(rgvGridOrdenes.Rows[e.RowIndex].Cells[Lenguaje.traduce("Hay Comentarios")].Value.ToString()) == 1)
                            {
                                e.CellElement.DrawImage = true;
                                e.CellElement.Image = Properties.Resources.email;
                                e.CellElement.ImageAlignment = ContentAlignment.TopLeft;
                            }
                            else
                            {
                                e.CellElement.ResetValue(LightVisualElement.DrawImageProperty, ValueResetFlags.Local);
                                e.CellElement.ResetValue(LightVisualElement.ImageProperty, ValueResetFlags.Local);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvGridPedidos_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0 && e.RowIndex > -1 && e.CellElement is GridRowHeaderCellElement)
                {
                    if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                    {
                    }
                }
            }
            catch (Exception)
            {
                log.Error("Evento ViewCellFormatting");
            }
        }

        private void rgvGridCarga_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0 && e.RowIndex > -1 && e.CellElement is GridRowHeaderCellElement)
                {
                    if (int.Parse(rgvGridCarga.Rows[e.RowIndex].Cells[Lenguaje.traduce("Hay Comentarios")].Value.ToString()) == 1)
                    {
                        e.CellElement.DrawImage = true;
                        e.CellElement.Image = Properties.Resources.email;
                        e.CellElement.ImageAlignment = ContentAlignment.TopLeft;
                    }
                    else
                    {
                        e.CellElement.ResetValue(LightVisualElement.DrawImageProperty, ValueResetFlags.Local);
                        e.CellElement.ResetValue(LightVisualElement.ImageProperty, ValueResetFlags.Local);
                    }
                }
            }
            catch (Exception)
            {
                log.Error("Evento ViewCellFormatting");
            }
        }

        #endregion Eventos

        #region BotonesPedidosCli

        private void btnVistaPedidosOrdenes_Click(object sender, EventArgs e)
        {
            rdfFiltro.Expression = "";
            rdfFiltro.Visible = false;
            tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(0, 0));
            if (this.tableLayoutPanelCarga.GetControlFromPosition(1, 0) != null)
            {
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(1, 0));
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(1, 1));
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(2, 0));
            }

            tableLayoutPanelCarga.Controls.Add(rgvGridOrdenes, 0, 0);
            GenerarGridOrdenesRecogida();
        }

        private void btnVistaPedidosCarga_Click(object sender, EventArgs e)
        {
            DibujarPanelCarga();
            GenerarGridOrdenesCarga();
        }

        private void DibujarPanelCarga()
        {
            rdfFiltro.Expression = "";
            rdfFiltro.Visible = false;
            rgvGridCargaPackingList.Dock = DockStyle.Fill;
            rgvGridCargaPackingList.BestFitColumns();
            rgvGridCargaPedidos.Dock = DockStyle.Fill;
            rgvGridCargaPedidos.BestFitColumns();

            rgvGridCargaContenido.Dock = DockStyle.Fill;
            rgvGridCargaContenido.BestFitColumns();
            rgvGridCargaPLPteCarga.Dock = DockStyle.Fill;
            rgvGridCargaPLPteCarga.BestFitColumns();
            rgvGridCargaPLPteCarga.HorizontalScroll.Enabled = true;
            rdPanelPLPteCarga.Dock = DockStyle.Fill;

            rdPanelPLPteCarga.Enabled = true;
            rdPanelPLPteCarga.Controls.Add(rgvGridCargaPLPteCarga);
            tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(0, 0));
            tableLayoutPanelCarga.Controls.Add(rdPanelPLPteCarga, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnDerecha, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnIzquierda, 0, 1);
            tableLayoutPanelCarga.Controls.Add(tableLayoutPanelBotones, 1, 0);
            tableLayoutPanelDetalleCarga.Controls.Add(rgvGridCarga, 0, 0);

            tbDetalleCarga.TabPages[0].Controls.Add(rgvGridCargaPackingList);
            tbDetalleCarga.TabPages[1].Controls.Add(rgvGridCargaPedidos);
            tbDetalleCarga.TabPages[2].Controls.Add(rgvGridCargaContenido);

            lblPeso.Text = Lenguaje.traduce("Peso") + ":";
            lblPeso.Font = new Font(lblPeso.Font, FontStyle.Bold);
            lblPeso.TextAlign = ContentAlignment.TopLeft;

            lblPesoCant.Text = Lenguaje.traduce("0");
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblPeso, 1, 0);
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblPesoCant, 2, 0);

            lblVolumen.Text = Lenguaje.traduce("Volumen") + ":";
            lblVolumen.Font = new Font(lblVolumen.Font, FontStyle.Bold);
            lblVolumenCant.Text = "0";
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblVolumen, 3, 0);
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblVolumenCant, 4, 0);

            lblUds.Text = Lenguaje.traduce("Uds") + ":";
            lblUds.Font = new Font(lblUds.Font, FontStyle.Bold);
            lblUdsCant.Text = "0";
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblUds, 5, 0);
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblUdsCant, 6, 0);
            lblPalets.Text = Lenguaje.traduce("Palets") + ":";
            lblPalets.Font = new Font(lblPalets.Font, FontStyle.Bold);
            lblPaletsCount.Text = "0";
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblPalets, 7, 0);
            tableLayoutPanelDetalleDatosTotal.Controls.Add(lblPaletsCount, 8, 0);

            tableLayoutPanelDetalleCarga.Controls.Add(tbDetalleCarga, 0, 1);
            tableLayoutPanelCarga.Controls.Add(tableLayoutPanelDetalleCarga, 2, 0);
            tableLayoutPanelCarga.Controls.Add(tableLayoutPanelDetalleDatosTotal, 2, 3);
        }

        private void RgvGridCargaPedidos_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e != null && e.Column != null && e.Row != null)
            {
                if (e.Column.Name.Equals(Lenguaje.traduce("Sello")))
                {
                    String valorSello = "";
                    if (e.Value == null)
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el sello"));
                        log.Error("No se ha podido actualizar el sello. El valor de la celda era NULL");
                        return;
                    }
                    else valorSello = e.Value.ToString();

                    int ordenPedidoID = 0;
                    String pedidoId = "";
                    if (e.Row.Cells[Lenguaje.traduce("ID")] == null)
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el sello"));
                        log.Error("No se ha podido actualizar el sello. El valor del ID era NULL");
                        return;
                    }
                    else pedidoId = e.Row.Cells[Lenguaje.traduce("ID")].Value.ToString();
                    if (!int.TryParse(pedidoId, out ordenPedidoID))
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el sello"));
                        log.Error("No se ha podido actualizar el sello. El valor del ID no era un int y ha fallado.");
                        return;
                    }

                    String resp = DataAccess.ActualizarSelloCarga(ordenPedidoID, valorSello);
                    if (!String.IsNullOrEmpty(resp))
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el sello"));
                        log.Error("No se ha podido actualizar el sello" + resp);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Proceso realizado correctamente"));
                    }
                }
            }
        }

        private void RgvGridCargaPackingList_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e != null && e.Column != null && e.Row != null)
            {
                if (e.Column.Name.Equals(Lenguaje.traduce("Orden")))
                {
                    int valorOrden = -1;
                    if (e.Value == null)
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el orden"));
                        log.Error("No se ha podido actualizar el orden. El valor de la celda era NULL");
                        return;
                    }
                    else valorOrden = Convert.ToInt32(e.Value);

                    int identificador = 0;
                    string identificadorStr = "";
                    if (e.Row.Cells[Lenguaje.traduce("ID")] == null)
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el orden"));
                        log.Error("No se ha podido actualizar el orden. El valor del ID era NULL");
                        return;
                    }
                    else identificadorStr = e.Row.Cells[Lenguaje.traduce("ID")].Value.ToString();
                    if (!int.TryParse(identificadorStr, out identificador))
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el orden"));
                        log.Error("No se ha podido actualizar el orden. El valor del ID no era un int y ha fallado.");
                        return;
                    }

                    String resp = DataAccess.ActualizarOrdenReparto(getIdCargaSeleccionada(), identificador, valorOrden);
                    if (!String.IsNullOrEmpty(resp))
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No se ha podido actualizar el orden"));
                        log.Error("No se ha podido actualizar el orden" + resp);
                    }
                }
            }
        }

        private void btnVistaPedidosRefrescar_Click(object sender, EventArgs e)
        {
            GenerarGridPedidos();
        }

        private void ItemColumnas_Click(object sender, EventArgs e)
        {
            try
            {
                this.rgvGridPedidos.ShowColumnChooser();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ImprimirEtiquetasPackingList_Click(object sender, EventArgs e)
        {
            try
            {
                ImprimirEtiquetas imprimirEtiquetas = new ImprimirEtiquetas();
                imprimirEtiquetas.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void LoadItem_Click(object sender, EventArgs e)
        {
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

        public void SaveItem_Click(object sender, EventArgs e)
        {
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

        private void btnVistaPedidosFiltrar_Click(object sender, EventArgs e)
        {
            if (rgvGridPedidos.Rows.Count > 0)
            {
                try
                {
                    radDataFilterPedidos.DataSource = rgvGridPedidos.DataSource;
                    if (radDataFilterPedidos.Visible == false)
                    {
                        radDataFilterPedidos.ShowDialog();
                    }
                    else
                    {
                        radDataFilterPedidos.Close();
                        radDataFilterPedidos.DataFilter.ApplyFilter();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("No hay datos que filtrar");
            }
        }

        private string confirmacion = Lenguaje.traduce(strings.ExportacionExito);

        public void btnVistaPedidosExportar_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón exportar " + DateTime.Now);
            GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(rgvGridPedidos);
            spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
            spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;
            bool openExportFile = false;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "";
            dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                spreadExporter.RunExport(dialog.FileName, new SpreadStreamExportRenderer());
                DialogResult dr = RadMessageBox.Show(confirmacion,
                Lenguaje.traduce(strings.ExportarExcel), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    openExportFile = true;
                }
            }
            if (openExportFile)
            {
                try
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
                catch (Exception ex)
                {
                    string message = String.Format(Lenguaje.traduce(strings.ExportarError) + "\nError message: {0}", ex.Message);
                    RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void btnVistaPedidosQuitarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón quitar filtro " + DateTime.Now);
                if (rgvGridPedidos.IsInEditMode)
                {
                    rgvGridPedidos.EndEdit();
                }
                rgvGridPedidos.FilterDescriptors.Clear();
                rgvGridPedidos.GroupDescriptors.Clear();
                radDataFilterPedidos.DataFilter.Expression = "";
                radDataFilterPedidos.DataFilter.ApplyFilter();

                rgvGridPedidos.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void cerrarOrdenRecogida(int idOrdenRecogida, int idOperario, int idRecurso)
        {
            Cursor.Current = Cursors.WaitCursor;
            SalidaMotor.WSSalidaMotorClient ws = null;
            try
            {
                ws = new SalidaMotor.WSSalidaMotorClient();
                ws.cerrarOrdenRecogida(idOrdenRecogida, DatosThread.getInstancia().getArrayDatos(), idOperario, idRecurso);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
            }
            Cursor.Current = Cursors.Default;
        }

        private void lanzamientoAgrupadoRecursoAutomatico()
        {
            try
            {
                List<LanzarPedidoCli> listaLanzar = new List<LanzarPedidoCli>();
                string jsonWS = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridPedidos.ChildRows, listaRecursiva);
                List<FechasEntrega> fechasEntregaSeleccionadas = obtenerFechasEntregaLineas(listaRecursiva);
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    LanzarPedidoCli lanzarPedido = new LanzarPedidoCli();
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        List<string> fechaPedido = new List<string>();
                        lanzarPedido.idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        lanzarPedido.muelle = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.Muelle) + ""].Value);
                        lanzarPedido.reservaautomatica = true;
                        lanzarPedido.recursoautomatico = true;
                        lanzarPedido.lanzamiento = "AG";
                        lanzarPedido.idusuario = User.IdUsuario;
                        lanzarPedido.error = "";
                        if (fechasEntregaSeleccionadas == null)
                        {
                            return;
                        }
                        List<FechasEntrega> fechasPedidoSeleccionadas = fechasEntregaSeleccionadas.FindAll(x => x.idpedidocli.Equals(lanzarPedido.idpedidocli));
                        foreach (FechasEntrega fecha in fechasPedidoSeleccionadas)
                        {
                            fechaPedido.Add(fecha.fechaentrega.ToString());
                        }
                        lanzarPedido.fechas = fechaPedido;
                        listaLanzar.Add(lanzarPedido);
                    }
                }

                jsonWS = JsonConvert.SerializeObject(listaLanzar);
                var respuestaWS = wsPedidoCli.lanzarPedidos(jsonWS, DatosThread.getInstancia().getArrayDatos());
                List<LanzarPedidoCli> errorText = JsonConvert.DeserializeObject<List<LanzarPedidoCli>>(respuestaWS);
                List<int> ordenesComprobar = new List<int>();
                for (int i = 0; i < errorText.Count; i++)
                {
                    if (errorText[i].error == string.Empty)
                    {
                        if (errorText[i].error == string.Empty)
                        {
                            ordenesComprobar = ordenesComprobar.Union(errorText[i].ordenes).ToList();
                        }
                        else
                        {
                            MessageBox.Show(errorText[i].error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(errorText[i].error);
                    }
                }
                if (mostrarAvisoFaltas)
                {
                    comprobarOrdenesSinFaltas(ordenesComprobar);
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rgvGridPedidos.Columns.Clear();
                rgvGridPedidos.Templates[0].Columns.Clear();
                GenerarGridPedidos();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void lanzamientoAgrupadoRecursoManual()
        {
            try
            {
                LanzamientoPedidoWS lanzarPedido = new LanzamientoPedidoWS();
                if (lanzarPedido.ShowDialog() == DialogResult.OK)
                {
                    int idHueco = lanzarPedido.idHueco;
                    string descHueco = lanzarPedido.descripcionHueco;
                    int idRecurso = lanzarPedido.idRecurso;
                    string descRecurso = lanzarPedido.descripcionRecurso;
                }
                else return;
                List<LanzarPedidoCli> listaLanzar = new List<LanzarPedidoCli>();
                string jsonWS = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridPedidos.ChildRows, listaRecursiva);
                List<FechasEntrega> fechasEntregaSeleccionadas = obtenerFechasEntregaLineas(listaRecursiva);
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    LanzarPedidoCli lanzarPedido2 = new LanzarPedidoCli();
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        List<string> fechaPedido = new List<string>();
                        lanzarPedido2.idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        lanzarPedido2.muelle = lanzarPedido.idHueco;
                        lanzarPedido2.reservaautomatica = true;
                        lanzarPedido2.recursoautomatico = false;
                        lanzarPedido2.idrecurso = lanzarPedido.idRecurso;
                        lanzarPedido2.lanzamiento = "AG";
                        lanzarPedido2.idusuario = User.IdUsuario;
                        lanzarPedido2.error = "";
                        if (fechasEntregaSeleccionadas == null)
                        {
                            return;
                        }
                        List<FechasEntrega> fechasPedidoSeleccionadas = fechasEntregaSeleccionadas.FindAll(x => x.idpedidocli.Equals(lanzarPedido2.idpedidocli));
                        foreach (FechasEntrega fecha in fechasPedidoSeleccionadas)
                        {
                            fechaPedido.Add(fecha.fechaentrega.ToString());
                        }
                        lanzarPedido2.fechas = fechaPedido;
                        listaLanzar.Add(lanzarPedido2);
                    }
                }

                if (listaLanzar.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona almenos una línea"), "Error");
                    return;
                }
                jsonWS = JsonConvert.SerializeObject(listaLanzar);
                var respuestaWS = wsPedidoCli.lanzarPedidos(jsonWS, DatosThread.getInstancia().getArrayDatos());
                List<LanzarPedidoCli> errorText = JsonConvert.DeserializeObject<List<LanzarPedidoCli>>(respuestaWS);
                List<int> ordenesComprobar = new List<int>();
                for (int i = 0; i < errorText.Count; i++)
                {
                    if (errorText[i].error == string.Empty)
                    {
                        if (errorText[i].error == string.Empty)
                        {
                            ordenesComprobar = ordenesComprobar.Union(errorText[i].ordenes).ToList();
                        }
                        else
                        {
                            MessageBox.Show(errorText[i].error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(errorText[i].error);
                    }
                }
                if (mostrarAvisoFaltas)
                {
                    comprobarOrdenesSinFaltas(ordenesComprobar);
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rgvGridPedidos.Columns.Clear();
                rgvGridPedidos.Templates[0].Columns.Clear();
                GenerarGridPedidos();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void lanzamientoRecursoAutomatico()
        {
            try
            {
                List<LanzarPedidoCli> listaLanzar = new List<LanzarPedidoCli>();
                List<string> errores = new List<string>();
                string jsonWS = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridPedidos.ChildRows, listaRecursiva);
                List<FechasEntrega> fechasEntregaSeleccionadas = obtenerFechasEntregaLineas(listaRecursiva);
                foreach (var childrow in listaRecursiva)
                {
                    LanzarPedidoCli lanzarPedido = new LanzarPedidoCli();
                    if (Convert.ToBoolean(childrow.Cells[0].Value) == true)
                    {
                        List<string> fechaPedido = new List<string>();
                        lanzarPedido.idpedidocli = Convert.ToInt32(childrow.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        lanzarPedido.muelle = Convert.ToInt32(childrow.Cells["" + Lenguaje.traduce(strings.Muelle) + ""].Value);
                        lanzarPedido.reservaautomatica = true;
                        lanzarPedido.recursoautomatico = true;
                        lanzarPedido.lanzamiento = "SA";
                        lanzarPedido.idusuario = User.IdUsuario;
                        lanzarPedido.error = "";
                        if (fechasEntregaSeleccionadas == null)
                        {
                            return;
                        }
                        List<FechasEntrega> fechasPedidoSeleccionadas = fechasEntregaSeleccionadas.FindAll(x => x.idpedidocli.Equals(lanzarPedido.idpedidocli));
                        foreach (FechasEntrega fecha in fechasPedidoSeleccionadas)
                        {
                            fechaPedido.Add(fecha.fechaentrega.ToString());
                        }
                        lanzarPedido.fechas = fechaPedido;
                        listaLanzar.Add(lanzarPedido);
                        log.Info("Detectado pedido seleccionado al lanzar: " + lanzarPedido.idpedidocli);
                    }
                }

                int numPedidos = listaLanzar.Count();

                foreach (LanzarPedidoCli pedido in listaLanzar)
                {
                    List<LanzarPedidoCli> pedidoList = new List<LanzarPedidoCli>();
                    pedidoList.Add(pedido);
                    jsonWS = JsonConvert.SerializeObject(/*listaLanzar*/pedidoList);
                    var respuestaWS = wsPedidoCli.lanzarPedidos(jsonWS, DatosThread.getInstancia().getArrayDatos());
                    if (respuestaWS != null)
                    {
                        List<LanzarPedidoCli> errorText = JsonConvert.DeserializeObject<List<LanzarPedidoCli>>(respuestaWS);
                        if (errorText != null)
                        {
                            List<int> ordenesComprobar = new List<int>();

                            for (int i = 0; i < errorText.Count; i++)
                            {
                                if (errorText[i].error == string.Empty)
                                {
                                    if (errorText[i].error == string.Empty)
                                    {
                                        ordenesComprobar = ordenesComprobar.Union(errorText[i].ordenes).ToList();
                                    }
                                    else
                                    {
                                        MessageBox.Show(errorText[i].error);
                                    }
                                }
                                else
                                {
                                    errores.Add(errorText[i].error);
                                }
                            }
                            if (mostrarAvisoFaltas)
                            {
                                comprobarOrdenesSinFaltas(ordenesComprobar);
                            }
                        }
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("WebService no responde correctamente."), "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                        log.Info("WebService no responde correctamente en Pedido: " + pedido.idpedidocli);
                    }
                }

                if (errores.Count > 0)
                {
                    String errorTxt = "";

                    foreach (string error in errores)
                    {
                        errorTxt = errorTxt + System.Environment.NewLine + errores[0];
                    }
                    RadMessageBox.Show(errorTxt, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                rgvGridPedidos.Columns.Clear();
                rgvGridPedidos.Templates[0].Columns.Clear();
                GenerarGridPedidos();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void ObtenerChildRowRecursivo(GridViewChildRowCollection childRows, List<GridViewRowInfo> list)
        {
            foreach (var child in childRows)
            {
                if (child.GetType().Name == "GridViewGroupRowInfo")
                {
                    ObtenerChildRowRecursivo(child.ChildRows, list);
                }
                if (child.GetType().Name == "GridViewRowInfo" || child.GetType().Name == "GridViewHierarchyRowInfo" || child.GetType().Name == "GridViewDataRowInfo")
                {
                    list.Add(child);
                }
            }
        }

        private void lanzamientoRecursoManual()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                LanzamientoPedidoWS lanzarPedido = new LanzamientoPedidoWS();
                if (lanzarPedido.ShowDialog() == DialogResult.OK)
                {
                    int idHueco = lanzarPedido.idHueco;
                    string descHueco = lanzarPedido.descripcionHueco;
                    int idRecurso = lanzarPedido.idRecurso;
                    string descRecurso = lanzarPedido.descripcionRecurso;
                }
                else return;
                List<LanzarPedidoCli> listaLanzar = new List<LanzarPedidoCli>();
                string jsonWS = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridPedidos.ChildRows, listaRecursiva);
                List<FechasEntrega> fechasEntregaSeleccionadas = obtenerFechasEntregaLineas(listaRecursiva);

                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    LanzarPedidoCli lanzarPedido2 = new LanzarPedidoCli();
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        List<string> fechaPedido = new List<string>();

                        lanzarPedido2.idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        lanzarPedido2.muelle = lanzarPedido.idHueco;
                        lanzarPedido2.reservaautomatica = true;
                        lanzarPedido2.recursoautomatico = false;
                        lanzarPedido2.idrecurso = lanzarPedido.idRecurso;
                        lanzarPedido2.lanzamiento = "SA";
                        lanzarPedido2.idusuario = User.IdUsuario;
                        lanzarPedido2.error = "";
                        lanzarPedido2.ordenes = null;
                        if (fechasEntregaSeleccionadas == null)
                        {
                            return;
                        }
                        List<FechasEntrega> fechasPedidoSeleccionadas = fechasEntregaSeleccionadas.FindAll(x => x.idpedidocli.Equals(lanzarPedido2.idpedidocli));
                        foreach (FechasEntrega fecha in fechasPedidoSeleccionadas)
                        {
                            fechaPedido.Add(fecha.fechaentrega.ToString());
                        }
                        lanzarPedido2.fechas = fechaPedido;

                        listaLanzar.Add(lanzarPedido2);
                        log.Info("Detectado pedido seleccionado al lanzar: " + lanzarPedido2.idpedidocli);
                    }
                }
                if (listaLanzar.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona al menos una línea"), "Error");
                    return;
                }

                jsonWS = JsonConvert.SerializeObject(listaLanzar);
                var respuestaWS = wsPedidoCli.lanzarPedidos(jsonWS, DatosThread.getInstancia().getArrayDatos());
                List<LanzarPedidoCli> errorText = JsonConvert.DeserializeObject<List<LanzarPedidoCli>>(respuestaWS);
                List<int> ordenesComprobar = new List<int>();

                for (int i = 0; i < errorText.Count; i++)
                {
                    if (errorText[i].error == string.Empty)
                    {
                        ordenesComprobar = ordenesComprobar.Union(errorText[i].ordenes).ToList();
                    }
                    else
                    {
                        MessageBox.Show(errorText[i].error);
                    }
                }
                if (mostrarAvisoFaltas)
                {
                    comprobarOrdenesSinFaltas(ordenesComprobar);
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rgvGridPedidos.Columns.Clear();
                //César:Esto proboca un error en el template porque cuando vuelve al rowsourceneeded no tiene esquema(columnas).
                GenerarGridPedidos();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private List<FechasEntrega> obtenerFechasEntregaLineas(List<GridViewRowInfo> listaRecursiva)
        {
            List<FechasEntrega> fechas = new List<FechasEntrega>();
            try
            {
                log.Debug("generarOrdenesPorFechaEntrega:" + generarOrdenesPorFechaEntrega);
                switch (generarOrdenesPorFechaEntrega)
                {
                    case -9999:
                        //no esta definido el parametro

                        return fechas;
                        break;

                    case 0:
                        //parametro no generar

                        return fechas;
                        break;

                    case 1://Seleccion manual
                        return obtenerFechasEntregaLineasManual(listaRecursiva);
                        break;

                    case 2: //seleccion automatica
                        return obtenerFechasEntregaLineasAutomatica(listaRecursiva);
                        break;

                    default:
                        return fechas;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, Lenguaje.traduce("Error obteniendo fechas entrega ."), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return fechas;
        }

        private List<FechasEntrega> obtenerFechasEntregaLineasAutomatica(List<GridViewRowInfo> listaRecursiva)
        {
            string where = "";
            List<FechasEntrega> listaFechas = new List<FechasEntrega>();
            try
            {
                bool primera = true;
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        int idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        if (primera)
                        {
                            where = where + idpedidocli;
                            primera = false;
                        }
                        else
                        {
                            where = where + "," + idpedidocli;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(where))
                {
                    DataTable dtFechas = Business.getDatosFechasEntregaLineasPedidos(where);
                    foreach (DataRow row in dtFechas.Rows)
                    {
                        FechasEntrega fechaEntrega = new FechasEntrega();
                        fechaEntrega.idpedidocli = Convert.ToInt32(row["idpedidocli"]);
                        fechaEntrega.fechaentrega = Convert.ToDateTime(row["Fecha Entrega"]);
                        listaFechas.Add(fechaEntrega);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, Lenguaje.traduce("Error obteniendo fechas entrega automatica."), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return listaFechas;
        }

        private List<FechasEntrega> obtenerFechasEntregaLineasManual(List<GridViewRowInfo> listaRecursiva)
        {
            string where = "";
            List<FechasEntrega> listaFechas = new List<FechasEntrega>();
            try
            {
                bool primera = true;
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        int idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        if (primera)
                        {
                            where = where + idpedidocli;
                            primera = false;
                        }
                        else
                        {
                            where = where + "," + idpedidocli;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(where))
                {
                    FrmSeleccionFechaEntrega frm = new FrmSeleccionFechaEntrega(where);
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        listaFechas = frm.fechas;
                    }
                    else
                    {
                        MessageBox.Show(this, Lenguaje.traduce("No se ha seleccionado ninguna fecha"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listaFechas = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, Lenguaje.traduce("Error obteniendo fechas entrega manual."), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return listaFechas;
        }

        private void comprobarOrdenesSinFaltas(List<int> ordenes)
        {
            try
            {
                int avisoFaltas = 0;
                ArrayList ordenesConFaltas = new ArrayList();
                foreach (int orden in ordenes)
                {
                    DataTable dtFaltas = DataAccess.GetSeleccion(DataAccess.GetDatatablePendienteOrdenRecogida(orden));
                    avisoFaltas = avisoFaltas + dtFaltas.Rows.Count;
                    ordenesConFaltas.Add((Int32.Parse(orden.ToString())));
                }
                if (avisoFaltas > 0)
                {
                    MessageBox.Show(Lenguaje.traduce("No se han podido reservar completamente " + avisoFaltas + " lineas de la/s orden/es " + string.Join(",", ordenesConFaltas.ToArray())), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnAccionLanzamientoPedido_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (chkRecursoManual.IsChecked == true)
                {
                    lanzamientoRecursoManual();
                }
                else
                {
                    lanzamientoRecursoAutomatico();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnAccionLanzamientoAgrupado_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (chkRecursoManual.IsChecked == true)
                {
                    lanzamientoAgrupadoRecursoManual();
                }
                else
                {
                    lanzamientoAgrupadoRecursoAutomatico();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void rBtnAccionPedidosLiberar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                List<LiberarPedidoCli> listaLiberar = new List<LiberarPedidoCli>();
                string jsonWS = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridPedidos.ChildRows, listaRecursiva);

                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    LiberarPedidoCli liberarPedido = new LiberarPedidoCli();
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        liberarPedido.idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        liberarPedido.idusuario = User.IdUsuario;
                        liberarPedido.error = "";

                        listaLiberar.Add(liberarPedido);
                    }
                }
                if (listaLiberar.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona almenos una línea"), "Error");
                    return;
                }
                jsonWS = JsonConvert.SerializeObject(listaLiberar);
                var respuestaWS = wsPedidoCli.liberarPedidos(jsonWS);
                List<LiberarPedidoCli> errorText = JsonConvert.DeserializeObject<List<LiberarPedidoCli>>(respuestaWS);
                for (int i = 0; i < errorText.Count; i++)
                {
                    if (errorText[i].error == string.Empty)
                    { }
                    else
                    {
                        MessageBox.Show(errorText[i].error);
                    }
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            GenerarGridPedidos();
        }

        private void btnDerecha_Click(object sender, EventArgs e)
        {
            try
            {
                int _cargaIdSeleccionado = getIdCargaSeleccionada();
                if (!comprovarEsCargaNoCerrada())
                {
                    return;
                }
                List<int> Ids = GetIdentificadoresPLrgvGridCargaPLPteCargaListSelected();
                if (_cargaIdSeleccionado > 0)
                {
                    string json = formarJsonAgregarQuitarCarga(_cargaIdSeleccionado, Ids);
                    WSCargaMotorClient cm = new WSCargaMotorClient();
                    String respuesta = cm.agregarIdentificadoresPLaCarga(json);
                    var jsonRespuesta = JsonConvert.DeserializeObject(respuesta);

                    if (jsonRespuesta != null)
                    {
                        try
                        {
                            if (!(jsonRespuesta as JArray).ElementAt(0).ElementAt(2).ElementAt(0).ToString().Equals(""))
                            {
                                RadMessageBox.Show(Lenguaje.traduce((jsonRespuesta as JArray).ElementAt(0).ElementAt(2).ElementAt(0).ToString()), "Error");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error deserializando el Json: " + respuesta);
                            ExceptionManager.GestionarError(ex);
                        }
                    }

                    cargaSeleccionadaWorker = _cargaIdSeleccionado;
                    GenerarGridOrdenesCarga();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, Lenguaje.traduce("Error al conectar con el web service."));
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnIzquierda_Click(object sender, EventArgs e)
        {
            WSCargaMotorClient cm = null;
            try
            {
                int _cargaIdSeleccionado = getIdCargaSeleccionada();
                if (!comprovarEsCargaNoCerrada())
                {
                    return;
                }
                if (_cargaIdSeleccionado > 0)
                {
                    if (!comprovarEsCargaNoCerrada())
                    {
                        return;
                    }
                    List<int> Ids = GetIdentificadoresPLrgvGridCargaPackingListSelected();
                    string json = formarJsonAgregarQuitarCarga(_cargaIdSeleccionado, Ids);
                    cm = new WSCargaMotorClient();
                    String respuesta = cm.quitarIdentificadoresPLaCarga(json);
                    var jsonRespuesta = JsonConvert.DeserializeObject(respuesta);

                    if (jsonRespuesta != null)
                    {
                        try
                        {
                            if (!(jsonRespuesta as JArray).ElementAt(0).ElementAt(2).ElementAt(0).ToString().Equals(""))
                            {
                                RadMessageBox.Show(Lenguaje.traduce((jsonRespuesta as JArray).ElementAt(0).ElementAt(2).ElementAt(0).ToString()), "Error");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error deserializando el Json: " + respuesta);
                            ExceptionManager.GestionarError(ex);
                        }
                    }

                    cargaSeleccionadaWorker = _cargaIdSeleccionado;
                    GenerarGridOrdenesCarga();
                }
            }
            catch (Exception ex)
            {
                if (cm == null)
                {
                    ExceptionManager.GestionarError(ex, "Error conectando a WebService");
                }
                else
                    ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private List<int> GetIdentificadoresPLrgvGridCargaPackingListSelected()
        {
            List<int> Ids = new List<int>();
            List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
            ObtenerChildRowRecursivo(rgvGridCargaPackingList.ChildRows, listaRecursiva);
            try
            {
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        int idPL = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID")].Value);
                        Ids.Add(idPL);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            return Ids;
        }

        private List<int> GetIdentificadoresPLrgvGridCargaPLPteCargaListSelected()
        {
            List<int> Ids = new List<int>();
            List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
            ObtenerChildRowRecursivo(rgvGridCargaPLPteCarga.ChildRows, listaRecursiva);
            try
            {
                int posicionCheckBox = Utilidades.buscarDondeEstaCheckBox(ref rgvGridCargaPLPteCarga);

                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    bool estaSeleccionado = false;
                    if ((row.Cells[posicionCheckBox].Value) is String)
                    {
                        if (row.Cells[posicionCheckBox].Value.ToString().Equals("On"))
                            estaSeleccionado = true;
                    }
                    else if (Convert.ToBoolean(row.Cells[posicionCheckBox].Value) == true)
                    {
                        estaSeleccionado = true;
                    }
                    if (estaSeleccionado)
                    {
                        int idPL = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID")].Value);
                        Ids.Add(idPL);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            return Ids;
        }

        #endregion BotonesPedidosCli

        #region BotonesOrdenesRecogida

        private void rBtnAccionServir_Click(object sender, EventArgs e)
        {
            try
            {
                List<ServirReserva> listaServir = new List<ServirReserva>();
                string jsonWS = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridOrdenes.ChildRows, listaRecursiva);
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    ServirReserva servirReserva = new ServirReserva();
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        servirReserva.idordenrecogida = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumOrdenRecogida) + ""].Value);
                        servirReserva.fechaservicio = DateTime.Now.ToShortDateString();
                        servirReserva.idusuario = User.IdUsuario;
                        servirReserva.error = new List<String>();
                        listaServir.Add(servirReserva);
                    }
                }
                if (listaServir.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona almenos una línea"), "Error");
                    return;
                }
                if (MessageBox.Show(Lenguaje.traduce("Se van a servir las ordenes seleccionadas. ¿Desea continuar?"), "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    jsonWS = JsonConvert.SerializeObject(listaServir);
                    var respuestaWS = wsPedidoCli.servirOrdenRecogida(jsonWS, DatosThread.getInstancia().getArrayDatos());
                    List<ServirReserva> errorText = JsonConvert.DeserializeObject<List<ServirReserva>>(respuestaWS);
                    bool hayError = false;
                    for (int i = 0; i < errorText.Count; i++)
                    {
                        if (errorText[i].error.Count == 0)
                        {
                            break;
                        }
                        if (string.IsNullOrEmpty(errorText[i].error[0]))
                        { hayError = true; }
                        else
                        {
                            MessageBox.Show(errorText[i].error[0]);
                        }
                    }
                    if (!hayError)
                    {
                        MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    rgvGridOrdenes.Columns.Clear();
                    GenerarGridOrdenesRecogida();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, Lenguaje.traduce(""));
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnAccionReservar_Click(object sender, EventArgs e)
        {
            try
            {
                List<ReservarOrden> listaReservar = new List<ReservarOrden>();
                string jsonWS = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                List<int> ordenesComprobar = new List<int>();
                ObtenerChildRowRecursivo(rgvGridOrdenes.ChildRows, listaRecursiva);
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    ReservarOrden reserva = new ReservarOrden();
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        reserva.idordenrecogida = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumOrdenRecogida) + ""].Value);
                        reserva.recursoAutomatico = true;
                        reserva.idRecurso = 0;
                        reserva.idusuario = User.IdUsuario;
                        reserva.error = new List<String>();
                        listaReservar.Add(reserva);
                        ordenesComprobar.Add(reserva.idordenrecogida);
                    }
                }
                if (listaReservar.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona almenos una línea"), "Error");
                    return;
                }
                jsonWS = JsonConvert.SerializeObject(listaReservar);
                var respuestaWS = wsPedidoCli.reservarOrdenRecogida(jsonWS);
                List<ReservarOrden> errorText = JsonConvert.DeserializeObject<List<ReservarOrden>>(respuestaWS);
                bool hayError = false;

                for (int i = 0; i < errorText.Count; i++)
                {
                    if (errorText[i].error.Count == 0)
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(errorText[i].error[0]))
                    { hayError = true; }
                    else
                    {
                        MessageBox.Show(errorText[i].error[0]);
                    }
                }
                if (mostrarAvisoFaltas)
                {
                    comprobarOrdenesSinFaltas(ordenesComprobar);
                }
                if (!hayError)
                    MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rgvGridOrdenes.Columns.Clear();
                GenerarGridOrdenesRecogida();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private bool IterateChildRows(GridViewRowInfo rowInfo)
        {
            bool err = false;
            GridViewHierarchyRowInfo hierarchyRow = rowInfo as GridViewHierarchyRowInfo;

            //To get current row childRows count
            int noOfChildRows = hierarchyRow.ChildRows.Count;

            //looping through the child rows
            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
            {
                //check if its current child row
                if (row.IsSelected)
                {
                    Debug.WriteLine(row.Cells[Lenguaje.traduce(strings.NumReserva)].Value);
                }
            }

            return err;
        }

        private void rBtnAccionCancelarReservar_Click(object sender, EventArgs e)
        {
            try
            {
                List<CancelarReserva> listaCancelar2 = new List<CancelarReserva>();
                string jsonWS2 = string.Empty;
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridOrdenes.ChildRows, listaRecursiva);
                foreach (GridViewRowInfo rowinfo2 in listaRecursiva)
                {
                    if (Convert.ToBoolean(rowinfo2.Cells[0].Value) == true)
                    {
                        string recogidaPrueba = rowinfo2.Cells[Lenguaje.traduce(strings.NumOrdenRecogida)].Value.ToString();
                        DataTable reservas = Business.GetOrdenesRecogidaReservasJerarquicoDatosGridView(recogidaPrueba);

                        foreach (DataRow row2 in reservas.Rows)
                        {
                            CancelarReserva cancelarReserva = new CancelarReserva();
                            cancelarReserva.idReserva = Convert.ToInt32(row2["" + Lenguaje.traduce(strings.NumReserva) + ""]);
                            cancelarReserva.idusuario = User.IdUsuario;
                            cancelarReserva.error = "";
                            listaCancelar2.Add(cancelarReserva);
                        }
                    }
                }
                if (listaCancelar2.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona almenos una línea"), "Error");
                    return;
                }

                List<CancelarReserva> listaCancelar = new List<CancelarReserva>();
                string jsonWS = string.Empty;
                foreach (GridViewRowInfo rowinfo in listaRecursiva)
                {
                    foreach (GridViewRowInfo row in rgvGridOrdenes.Templates[2].Rows)
                    {
                        CancelarReserva cancelarReserva = new CancelarReserva();
                        if (Convert.ToBoolean(row.Cells[0].Value) == true)
                        {
                            cancelarReserva.idReserva = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumReserva) + ""].Value);
                            cancelarReserva.error = "";
                            listaCancelar.Add(cancelarReserva);
                        }
                    }
                }
                jsonWS = JsonConvert.SerializeObject(listaCancelar);
                jsonWS2 = JsonConvert.SerializeObject(listaCancelar2);
                var respuestaWS = wsPedidoCli.cancelarReserva(jsonWS);
                var respuestaWS2 = wsPedidoCli.cancelarReserva(jsonWS2);
                List<CancelarReserva> errorText = JsonConvert.DeserializeObject<List<CancelarReserva>>(respuestaWS);
                for (int i = 0; i < errorText.Count; i++)
                {
                    if (errorText[i].error == string.Empty)
                    { }
                    else
                    {
                        MessageBox.Show(errorText[i].error);
                    }
                }
                List<CancelarReserva> errorText2 = JsonConvert.DeserializeObject<List<CancelarReserva>>(respuestaWS2);
                for (int i = 0; i < errorText2.Count; i++)
                {
                    if (errorText2[i].error == string.Empty)
                    { }
                    else
                    {
                        MessageBox.Show(errorText2[i].error);
                    }
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rgvGridOrdenes.Columns.Clear();
                GenerarGridOrdenesRecogida();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnVistaOrdenesPedidos_Click(object sender, EventArgs e)
        {
            radDataFilterOrdenes.DataFilter.Expression = "";
            radDataFilterOrdenes.Visible = false;
            tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(0, 0));
            tableLayoutPanelCarga.Controls.Add(rgvGridPedidos, 0, 0);
            ribbonTabAccionesPedidos.Enabled = true;
            ribbonTabAccionesOrdenesRecogida.Enabled = false;
            ribbonTabAccionesOrdenesCarga.Enabled = false;
            ribbonTabAccionesPedidos.Select();
            ribbonTabAccionesPedidos.Visibility = ElementVisibility.Visible;
            ribbonTabAccionesOrdenesRecogida.Visibility = ElementVisibility.Collapsed;
            ribbonTabAccionesOrdenesCarga.Visibility = ElementVisibility.Collapsed;
            GenerarGridPedidos();
        }

        private void btnVistaOrdenesOrdenesCarga_Click(object sender, EventArgs e)
        {
            DibujarPanelCarga();
            GenerarGridOrdenesCarga();
        }

        private void btnVistaOrdenesExportar_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón exportar " + DateTime.Now);
            GridViewSpreadExport spreadExporter = new GridViewSpreadExport(rgvGridOrdenes);
            spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
            spreadExporter.ExportFormat = SpreadExportFormat.Xlsx;
            spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;
            bool openExportFile = false;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "";
            dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                spreadExporter.RunExport(dialog.FileName, new SpreadExportRenderer());
                DialogResult dr = RadMessageBox.Show(confirmacion,
               Lenguaje.traduce(strings.ExportarExcel), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    openExportFile = true;
                }
            }
            if (openExportFile)
            {
                try
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
                catch (Exception ex)
                {
                    string message = String.Format(Lenguaje.traduce(strings.ExportarError) + "\nError message: {0}", ex.Message);
                    RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void btnVistaOrdenesFiltrar_Click(object sender, EventArgs e)
        {
            if (rgvGridOrdenes.Rows.Count > 0)
            {
                try
                {
                    radDataFilterOrdenes.DataSource = rgvGridOrdenes.DataSource;
                    if (radDataFilterOrdenes.Visible == false)
                    {
                        radDataFilterOrdenes.ShowDialog();
                    }
                    else
                    {
                        radDataFilterOrdenes.Close();
                        radDataFilterOrdenes.DataFilter.ApplyFilter();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("No hay datos que filtrar");
            }
        }

        private void btnVistaOrdenesQuitarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                if (rgvGridOrdenes.IsInEditMode)
                {
                    rgvGridOrdenes.EndEdit();
                }
                rgvGridOrdenes.FilterDescriptors.Clear();
                rgvGridOrdenes.GroupDescriptors.Clear();
                radDataFilterOrdenes.DataFilter.Expression = "";
                radDataFilterOrdenes.DataFilter.ApplyFilter();
                rgvGridOrdenes.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnVistaOrdenesRefrescar_Click(object sender, EventArgs e)
        {
            refrescarOrdenes();
        }

        private void refrescarOrdenes()
        {
            rgvGridOrdenes.DataSource = null;
            rgvGridOrdenes.Columns.Clear();
            GenerarGridOrdenesRecogida();
            Utilidades.refrescarJerarquico(ref this.rgvGridOrdenes, -1);
        }

        private void ItemColumnas_ClickRecogida(object sender, EventArgs e)
        {
            try
            {
                this.rgvGridOrdenes.ShowColumnChooser();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion BotonesOrdenesRecogida

        #region BotonesCarga

        private void btnVistaCargaPedidos_Click(object sender, EventArgs e)
        {
            radDataFilterCarga.DataFilter.Expression = "";
            radDataFilterCarga.Visible = false;
            tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(0, 0));
            if (this.tableLayoutPanelCarga.GetControlFromPosition(1, 0) != null)
            {
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(1, 0));
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(1, 1));
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(2, 0));
            }
            tableLayoutPanelCarga.Controls.Add(rgvGridPedidos, 0, 0);
            ribbonTabAccionesPedidos.Enabled = true;
            ribbonTabAccionesOrdenesRecogida.Enabled = false;
            ribbonTabAccionesOrdenesCarga.Enabled = false;
            ribbonTabAccionesPedidos.Select();
            ribbonTabAccionesPedidos.Visibility = ElementVisibility.Visible;
            ribbonTabAccionesOrdenesRecogida.Visibility = ElementVisibility.Collapsed;
            ribbonTabAccionesOrdenesCarga.Visibility = ElementVisibility.Collapsed;
        }

        private void btnVistaCargaOrdenesRecogida_Click(object sender, EventArgs e)
        {
            radDataFilterCarga.DataFilter.Expression = "";
            radDataFilterCarga.Visible = false;
            tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(0, 0));
            if (this.tableLayoutPanelCarga.GetControlFromPosition(1, 0) != null)
            {
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(1, 0));
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(1, 1));
                tableLayoutPanelCarga.Controls.Remove(this.tableLayoutPanelCarga.GetControlFromPosition(2, 0));
            }
            tableLayoutPanelCarga.Controls.Add(rgvGridOrdenes, 0, 0);
            GenerarGridOrdenesRecogida();
        }

        private void ItemColumnas_ClickCarga(object sender, EventArgs e)
        {
            try
            {
                this.rgvGridCarga.ShowColumnChooser();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnVistaCargaExportar_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón exportar " + DateTime.Now);
            GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(rgvGridCarga);
            spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
            spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;
            bool openExportFile = false;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "";
            dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                spreadExporter.RunExport(dialog.FileName, new SpreadStreamExportRenderer());
                DialogResult dr = RadMessageBox.Show(confirmacion,
                Lenguaje.traduce(strings.ExportarExcel), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    openExportFile = true;
                }
            }
            if (openExportFile)
            {
                try
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
                catch (Exception ex)
                {
                    string message = String.Format(Lenguaje.traduce(strings.ExportarError) + "\nError message: {0}", ex.Message);
                    RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void btnVistaCargaFiltrar_Click(object sender, EventArgs e)
        {
            if (rgvGridCarga.Rows.Count > 0)
            {
                try
                {
                    radDataFilterCarga.DataSource = rgvGridCarga.DataSource;
                    if (radDataFilterCarga.Visible == false)
                    {
                        radDataFilterCarga.ShowDialog();
                    }
                    else
                    {
                        radDataFilterCarga.Close();
                        radDataFilterCarga.DataFilter.ApplyFilter();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("No hay datos que filtrar");
            }
        }

        private void btnVistaCargaQuitarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón filtro " + DateTime.Now);
                if (rgvGridCarga.IsInEditMode)
                {
                    rgvGridCarga.EndEdit();
                }
                rgvGridCarga.FilterDescriptors.Clear();
                rgvGridCarga.GroupDescriptors.Clear();
                radDataFilterCarga.DataFilter.Expression = "";
                radDataFilterCarga.DataFilter.ApplyFilter();
                rgvGridCarga.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnVistaCargaRefrescar_Click(object sender, EventArgs e)
        {
            GenerarGridOrdenesCarga();
        }

        #endregion BotonesCarga

        #region Estilos

        public void SaveLayoutGlobal()
        {
            int indexLineasReservadas;
            int indexLineasPreparadas;
            int indexCantidadReservada;
            int indexCantidadServida;
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
                if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                {
                    this.Name = "OrdenesCliente";
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    path.Replace(" ", "_");
                    indexLineasReservadas = Convert.ToInt32(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasReservadas)].Index);
                    indexLineasPreparadas = Convert.ToInt32(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasPreparadas)].Index);

                    //Esto se hace para compensar en caso de que las líneas preparadas vayan en la posición delante.
                    if (indexLineasPreparadas < indexLineasReservadas)
                        indexLineasReservadas = indexLineasReservadas - 1;
                    indexCantidadReservada = Convert.ToInt32(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadReservada)].Index);
                    indexCantidadServida = Convert.ToInt32(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadServida)].Index);
                    if (indexCantidadServida < indexCantidadReservada)
                        indexCantidadReservada = indexCantidadReservada - 1;
                    XmlReaderPropio.setLineasPreparadas(indexLineasPreparadas, pathGL);
                    XmlReaderPropio.setLineasReservadas(indexLineasReservadas, pathGL);
                    XmlReaderPropio.setCantidadReservadaPorc(indexCantidadReservada, pathGL);
                    XmlReaderPropio.setCantidadServidaPorc(indexCantidadServida, pathGL);
                    rgvGridPedidos.Columns.Remove(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasReservadas)]);
                    rgvGridPedidos.Columns.Remove(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasPreparadas)]);
                    rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadReservada);
                    rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadServida);
                    rgvGridPedidos.SaveLayout(path);
                    rgvGridPedidos.Columns.Add(colLineasReservadas);
                    rgvGridPedidos.Columns.Add(colLineasPreparadas);
                    rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadReservada);
                    rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadServida);

                    rgvGridPedidos.Columns.Move(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasReservadas)].Index, indexLineasReservadas);
                    rgvGridPedidos.Columns.Move(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasPreparadas)].Index, indexLineasPreparadas);
                    rgvGridPedidos.Templates[0].Columns.Move(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadReservada)].Index, indexCantidadReservada);
                    rgvGridPedidos.Templates[0].Columns.Move(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadServida)].Index, indexCantidadServida);
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                {
                    this.Name = "OrdenesRecogida";
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    path.Replace(" ", "_");
                    rgvGridOrdenes.SaveLayout(path);
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rdPanelPLPteCarga)
                {
                    this.Name = "OrdenesCarga";
                    Directory.CreateDirectory(path);
                    string pathContenido = path;
                    string pathPacking = path;
                    string pathPackingPte = path;
                    string pathOrdenesCargaPedidos = path;

                    path += "\\" + this.Name + "GridView.xml";
                    pathContenido += "\\" + "OrdenesCargaContenidoGridView.xml";
                    pathPacking += "\\" + "OrdenesPackingGridView.xml";
                    pathPackingPte += "\\" + "OrdenesPackingPteGridView.xml";
                    pathOrdenesCargaPedidos += "\\" + "OrdenesCargaPedidosGridView.xml";

                    path.Replace(" ", "_");
                    rgvGridCarga.SaveLayout(path);
                    rgvGridCargaContenido.SaveLayout(pathContenido);
                    rgvGridCargaPackingList.SaveLayout(pathPacking);
                    rgvGridCargaPLPteCarga.SaveLayout(pathPackingPte);
                    rgvGridCargaPedidos.SaveLayout(pathOrdenesCargaPedidos);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void SaveLayoutLocal()
        {
            int indexLineasReservadas;
            int indexLineasPreparadas;
            int indexCantidadReservada;
            int indexCantidadServida;
            int pathGL = 1;
            string path = Persistencia.DirectorioLocal;/*XmlReaderPropio.getLayoutPath(1);*/
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
                if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                {
                    this.Name = "OrdenesCliente";
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    path.Replace(" ", "_");
                    indexLineasReservadas = Convert.ToInt32(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasReservadas)].Index);
                    indexLineasPreparadas = Convert.ToInt32(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasPreparadas)].Index);
                    indexCantidadReservada = Convert.ToInt32(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.pctCantidadReservada)].Index);

                    indexCantidadServida = Convert.ToInt32(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.pctCantidadServida)].Index);
                    XmlReaderPropio.setLineasPreparadas(indexLineasPreparadas, pathGL);
                    XmlReaderPropio.setLineasReservadas(indexLineasReservadas, pathGL);
                    XmlReaderPropio.setCantidadReservadaPorc(indexCantidadReservada, pathGL);
                    XmlReaderPropio.setCantidadServidaPorc(indexCantidadServida, pathGL);
                    rgvGridPedidos.Columns.Remove(colLineasReservadas/*rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasReservadas)]*/);
                    rgvGridPedidos.Columns.Remove(colLineasPreparadas/* rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasPreparadas)]*/);
                    rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadReservada/*rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadReservada)]*/);
                    rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadServida/*rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadServida)]*/);
                    rgvGridPedidos.SaveLayout(path);
                    rgvGridPedidos.Columns.Add(colLineasReservadas);
                    rgvGridPedidos.Columns.Add(colLineasPreparadas);
                    rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadReservada);
                    rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadServida);
                    rgvGridPedidos.Columns.Move(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasReservadas)].Index, indexLineasReservadas);
                    rgvGridPedidos.Columns.Move(rgvGridPedidos.Columns[Lenguaje.traduce(strings.LineasPreparadas)].Index, indexLineasPreparadas);
                    rgvGridPedidos.Templates[0].Columns.Move(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadReservada)].Index, indexCantidadReservada);
                    rgvGridPedidos.Templates[0].Columns.Move(rgvGridPedidos.Templates[0].Columns[Lenguaje.traduce(strings.CantidadServida)].Index, indexCantidadServida);
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                {
                    this.Name = "OrdenesRecogida";
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    path.Replace(" ", "_");
                    rgvGridOrdenes.SaveLayout(path);
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rdPanelPLPteCarga)
                {
                    this.Name = "OrdenesCarga";
                    Directory.CreateDirectory(path);
                    string pathContenido = path;
                    string pathPacking = path;
                    string pathPackingPte = path;
                    string pathOrdenesCargaPedidos = path;

                    path += "\\" + this.Name + "GridView.xml";
                    pathContenido += "\\" + "OrdenesCargaContenidoGridView.xml";
                    pathPacking += "\\" + "OrdenesPackingGridView.xml";
                    pathPackingPte += "\\" + "OrdenesPackingPteGridView.xml";
                    pathOrdenesCargaPedidos += "\\" + "OrdenesCargaPedidosGridView.xml";

                    path.Replace(" ", "_");
                    rgvGridCarga.SaveLayout(path);
                    rgvGridCargaContenido.SaveLayout(pathContenido);
                    rgvGridCargaPackingList.SaveLayout(pathPacking);
                    rgvGridCargaPLPteCarga.SaveLayout(pathPackingPte);
                    rgvGridCargaPedidos.SaveLayout(pathOrdenesCargaPedidos);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void LoadLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;/* XmlReaderPropio.getLayoutPath(0);*/
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
                if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                {
                    this.Name = "OrdenesCliente";
                    string s = path + "\\" + this.Name + "GridView.xml";
                    //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "GridView.xml";

                    s.Replace(" ", "_");
                    this.rgvGridPedidos.Columns.Remove(colLineasPreparadas);
                    this.rgvGridPedidos.Columns.Remove(colLineasReservadas);
                    this.rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadReservada);
                    this.rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadServida);
                    //gridViewControl.LoadLayout(s);
                    this.rgvGridPedidos.LoadLayout(s);
                    this.rgvGridPedidos.Columns.Add(colLineasPreparadas);
                    this.rgvGridPedidos.Columns.Add(colLineasReservadas);
                    this.rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadReservada);
                    this.rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadServida);
                    rgvGridPedidos.Columns.Move(colLineasPreparadas.Index, XmlReaderPropio.getLineasPreparadas(pathGL) - 1);
                    rgvGridPedidos.Columns.Move(colLineasReservadas.Index, XmlReaderPropio.getLineasReservadas(pathGL));
                    rgvGridPedidos.Templates[0].Columns.Move(colpctCantidadServida.Index, XmlReaderPropio.getCantidadServidaPorc(pathGL) - 1);
                    rgvGridPedidos.Templates[0].Columns.Move(colpctCantidadReservada.Index, XmlReaderPropio.getCantidadReservadaPorc(pathGL));
                    colLineasPreparadas.Width = 100;
                    colLineasReservadas.Width = 100;
                    colpctCantidadReservada.Width = 100;
                    colpctCantidadServida.Width = 100;
                    for (int i = 0; i < rgvGridPedidos.Templates.Count; i++)
                    {
                        rgvGridPedidos.Templates[i].Refresh();
                    }
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                {
                    this.Name = "OrdenesRecogida";
                    string s = path + "\\" + this.Name + "GridView.xml";
                    //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "GridView.xml";

                    s.Replace(" ", "_");
                    rgvGridOrdenes.LoadLayout(s);
                    for (int i = 0; i < rgvGridOrdenes.Templates.Count; i++)
                    {
                        rgvGridOrdenes.Templates[i].Refresh();
                    }
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rdPanelPLPteCarga)
                {
                    this.Name = "OrdenesCarga";
                    string pathContenido = path;
                    string pathPacking = path;
                    string pathPLPte = path;
                    string pathOrdenesCargaPedidos = path;
                    string s = path + "\\" + this.Name + "GridView.xml";

                    path += "\\" + this.Name + "GridView.xml";
                    pathContenido += "\\" + "OrdenesCargaContenidoGridView.xml";
                    pathPacking += "\\" + "OrdenesPackingGridView.xml";
                    pathPLPte += "\\" + "OrdenesPackingPteGridView.xml";
                    pathOrdenesCargaPedidos += "\\" + "OrdenesCargaPedidosGridView.xml";
                    s.Replace(" ", "_");
                    bool existsGridView = File.Exists(path);
                    if (existsGridView)
                    {
                        rgvGridCarga.LoadLayout(path);
                    }
                    existsGridView = File.Exists(pathContenido);
                    if (existsGridView)
                    {
                        rgvGridCargaContenido.LoadLayout(pathContenido);
                    }
                    existsGridView = File.Exists(pathPacking);
                    if (existsGridView)
                    {
                        rgvGridCargaPackingList.LoadLayout(pathPacking);
                    }
                    existsGridView = File.Exists(pathPLPte);
                    if (existsGridView)
                    {
                        rgvGridCargaPLPteCarga.LoadLayout(pathPLPte);
                    }
                    existsGridView = File.Exists(pathOrdenesCargaPedidos);
                    if (existsGridView)
                    {
                        rgvGridCargaPedidos.LoadLayout(pathOrdenesCargaPedidos);
                    }

                    //Para mantener la carga seleccionada al realizar alguna acción.
                    if (cargaSeleccionadaWorker > 0)
                    {
                        checkBoxCargaCheckear(cargaSeleccionadaWorker);
                    }

                    Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCargaPackingList);
                    Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCargaPedidos);
                    Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCargaContenido);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        //Metodo para poner a true la carga indicada en el checkbox
        private void checkBoxCargaCheckear(int _cargaIdSeleccionado)
        {
            foreach (GridViewDataRowInfo row in this.rgvGridCarga.Rows)
            {
                int idCarga = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num Carga")].Value);
                if (idCarga != _cargaIdSeleccionado)
                {
                    row.Cells[0].Value = false;
                }
                else
                {
                    row.Cells[0].Value = true;
                    cargaAnteriorSeleccionada = _cargaIdSeleccionado;
                }
            }
        }

        public void LoadLayoutLocal()
        {
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
                if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                {
                    this.Name = "OrdenesCliente";
                    string s = path + "\\" + this.Name + "GridView.xml";

                    s.Replace(" ", "_");
                    this.rgvGridPedidos.Columns.Remove(colLineasPreparadas);
                    this.rgvGridPedidos.Columns.Remove(colLineasReservadas);
                    this.rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadReservada);
                    this.rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadServida);
                    log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Empieza cargar layout");
                    this.rgvGridPedidos.LoadLayout(s);
                    log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Finaliza cargar layout");
                    this.rgvGridPedidos.Columns.Add(colLineasPreparadas);
                    this.rgvGridPedidos.Columns.Add(colLineasReservadas);
                    this.rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadReservada);
                    this.rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadServida);
                    int lineasPreparadasXML = XmlReaderPropio.getLineasPreparadas(pathGL);
                    if (lineasPreparadasXML > 0) lineasPreparadasXML -= 1;
                    rgvGridPedidos.Columns.Move(colLineasPreparadas.Index, lineasPreparadasXML);
                    rgvGridPedidos.Columns.Move(colLineasReservadas.Index, XmlReaderPropio.getLineasReservadas(pathGL));

                    int cantidadServidaPorcXML = XmlReaderPropio.getCantidadServidaPorc(pathGL);
                    if (cantidadServidaPorcXML > 0) cantidadServidaPorcXML -= 1;
                    rgvGridPedidos.Templates[0].Columns.Move(colpctCantidadServida.Index, cantidadServidaPorcXML);
                    rgvGridPedidos.Templates[0].Columns.Move(colpctCantidadReservada.Index, XmlReaderPropio.getCantidadReservadaPorc(pathGL));
                    colLineasPreparadas.Width = 100;
                    colLineasReservadas.Width = 100;
                    colpctCantidadReservada.Width = 100;
                    colpctCantidadServida.Width = 100;
                    this.rgvGridPedidos.TableElement.RowHeight = 40;
                    for (int i = 0; i < rgvGridPedidos.Templates.Count; i++)
                    {
                        rgvGridPedidos.Templates[i].Refresh();
                    }
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                {
                    this.Name = "OrdenesRecogida";
                    string s = path + "\\" + this.Name + "GridView.xml";

                    s.Replace(" ", "_");
                    rgvGridOrdenes.LoadLayout(s);
                    for (int i = 0; i < rgvGridOrdenes.Templates.Count; i++)
                    {
                        rgvGridOrdenes.Templates[i].Refresh();
                    }
                }
                else if (this.tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rdPanelPLPteCarga)
                {
                    this.Name = "OrdenesCarga";
                    string pathContenido = path;
                    string pathPacking = path;
                    string pathPLPte = path;
                    string pathOrdenesCargaPedidos = path;
                    string s = path + "\\" + this.Name + "GridView.xml";

                    path += "\\" + this.Name + "GridView.xml";
                    pathContenido += "\\" + "OrdenesCargaContenidoGridView.xml";
                    pathPacking += "\\" + "OrdenesPackingGridView.xml";
                    pathPLPte += "\\" + "OrdenesPackingPteGridView.xml";
                    pathOrdenesCargaPedidos += "\\" + "OrdenesCargaPedidosGridView.xml";
                    s.Replace(" ", "_");
                    bool existsGridView = File.Exists(path);
                    if (existsGridView)
                    {
                        rgvGridCarga.LoadLayout(path);
                    }
                    existsGridView = File.Exists(pathContenido);
                    if (existsGridView)
                    {
                        rgvGridCargaContenido.LoadLayout(pathContenido);
                    }
                    existsGridView = File.Exists(pathPacking);
                    if (existsGridView)
                    {
                        rgvGridCargaPackingList.LoadLayout(pathPacking);
                    }
                    existsGridView = File.Exists(pathPLPte);
                    if (existsGridView)
                    {
                        rgvGridCargaPLPteCarga.LoadLayout(pathPLPte);
                    }
                    existsGridView = File.Exists(pathOrdenesCargaPedidos);
                    if (existsGridView)
                    {
                        rgvGridCargaPedidos.LoadLayout(pathOrdenesCargaPedidos);
                    }

                    if (cargaSeleccionadaWorker > 0)
                    {
                        checkBoxCargaCheckear(cargaSeleccionadaWorker);
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void ElegirEstilo()
        {
            try
            {
                string pathLocal = Persistencia.DirectorioLocal;
                string pathGlobal = Persistencia.DirectorioGlobal;
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
                if (tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridPedidos)
                {
                    this.Name = "OrdenesCliente";
                    string pathGridView = pathLocal + "\\" + this.Name + "GridView.xml";
                    bool existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        LoadLayoutLocal();
                    }
                    else
                    {
#pragma warning disable CS0219 // La variable 'pathG' está asignada pero su valor nunca se usa
                        int pathG = 0;
#pragma warning restore CS0219 // La variable 'pathG' está asignada pero su valor nunca se usa
                        this.Name = "OrdenesCliente";
                        pathGridView = pathGlobal + "\\" + this.Name + "GridView.xml";
                        existsGridView = File.Exists(pathGridView);
                        if (existsGridView)
                        {
                            LoadLayoutGlobal();
                        }
                        else
                        {
                            rgvGridPedidos.Columns.Remove(Lenguaje.traduce(strings.LineasReservadas));
                            rgvGridPedidos.Columns.Remove(Lenguaje.traduce(strings.LineasPreparadas));
                            rgvGridPedidos.Columns.Add(colLineasReservadas);
                            rgvGridPedidos.Columns.Add(colLineasPreparadas);
                            if (rgvGridPedidos.Templates.Count > 0)
                            {
                                rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadReservada);
                                rgvGridPedidos.Templates[0].Columns.Remove(colpctCantidadServida);
                                rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadReservada);
                                rgvGridPedidos.Templates[0].Columns.Add(colpctCantidadServida);
                            }
                            rgvGridPedidos.BestFitColumns();
                        }
                    }
                }
                if (tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rgvGridOrdenes)
                {
                    this.Name = "OrdenesRecogida";
                    string pathGridView = pathLocal + "\\" + this.Name + "GridView.xml";
                    bool existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        try
                        {
                            rgvGridOrdenes.LoadLayout(pathGridView);
                        }
                        catch (Exception e)
                        {
                            log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                        }
                    }
                    else
                    {
                        this.Name = "OrdenesRecogida";
                        pathGridView = pathGlobal + "\\" + this.Name + "GridView.xml";
                        existsGridView = File.Exists(pathGridView);
                        if (existsGridView)
                        {
                            rgvGridOrdenes.LoadLayout(pathGridView);
                        }
                        else
                        {
                            rgvGridOrdenes.BestFitColumns();
                        }
                    }
                }
                if (tableLayoutPanelCarga.GetControlFromPosition(0, 0) == rdPanelPLPteCarga)
                {
                    this.Name = "OrdenesCarga";
                    string pathGridView = pathLocal + "\\" + this.Name + "GridView.xml";
                    bool existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        LoadLayoutLocal();
                    }
                    else
                    {
                        this.Name = "OrdenesCarga";
                        pathGridView = pathGlobal + "\\" + this.Name + "GridView.xml";
                        existsGridView = File.Exists(pathGridView);
                        if (existsGridView)
                        {
                            LoadLayoutGlobal();
                        }
                        else
                        {
                            rgvGridCarga.BestFitColumns();
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }
        }

        private void ElegirEstiloSoloCargaPl()
        {
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

            this.Name = "OrdenesCarga";
            string pathContenido = path;
            string pathPacking = path;
            string pathPLPte = path;
            string pathOrdenesCargaPedidos = path;
            string s = path + "\\" + this.Name + "GridView.xml";

            path += "\\" + this.Name + "GridView.xml";
            pathContenido += "\\" + "OrdenesCargaContenidoGridView.xml";
            pathPacking += "\\" + "OrdenesPackingGridView.xml";
            pathPLPte += "\\" + "OrdenesPackingPteGridView.xml";
            pathOrdenesCargaPedidos += "\\" + "OrdenesCargaPedidosGridView.xml";
            s.Replace(" ", "_");
            bool existsGridView = File.Exists(path);
            existsGridView = File.Exists(pathContenido);
            if (existsGridView)
            {
                rgvGridCargaContenido.LoadLayout(pathContenido);
            }
            existsGridView = File.Exists(pathPacking);
            if (existsGridView)
            {
                rgvGridCargaPackingList.LoadLayout(pathPacking);
            }
            existsGridView = File.Exists(pathOrdenesCargaPedidos);
            if (existsGridView)
            {
                rgvGridCargaPedidos.LoadLayout(pathOrdenesCargaPedidos);
            }
        }

        #endregion Estilos

        #region LlenarGridPrincipal

        private void EstablecerNombresColumnasAñadidas()
        {
            colLineasPedido.FieldName = Lenguaje.traduce(strings.LineasPedido);
            colLineasPedido.Name = Lenguaje.traduce(strings.LineasPedido);
            colLineasPedido.HeaderText = Lenguaje.traduce(strings.LineasPedido);
            colPaletsCompletos.FieldName = Lenguaje.traduce(strings.PaletsCompletos);
            colPaletsCompletos.Name = Lenguaje.traduce(strings.PaletsCompletos);
            colPaletsCompletos.HeaderText = Lenguaje.traduce(strings.PaletsCompletos);
            colNumPickings.FieldName = Lenguaje.traduce(strings.Pickings);
            colNumPickings.Name = Lenguaje.traduce(strings.Pickings);
            colNumPickings.HeaderText = Lenguaje.traduce(strings.Pickings);
            colNumTareas.FieldName = Lenguaje.traduce(strings.NumTareas);
            colNumTareas.Name = Lenguaje.traduce(strings.NumTareas);
            colNumTareas.HeaderText = Lenguaje.traduce(strings.NumTareas);
            colDuracionEstimada.FieldName = Lenguaje.traduce(strings.DuracionEstimada);
            colDuracionEstimada.Name = Lenguaje.traduce(strings.DuracionEstimada);
            colDuracionEstimada.HeaderText = Lenguaje.traduce(strings.DuracionEstimada);
            colLineasReservadas.FieldName = Lenguaje.traduce(strings.LineasReservadas);
            colLineasReservadas.Name = Lenguaje.traduce(strings.LineasReservadas);
            colLineasReservadas.HeaderText = Lenguaje.traduce(strings.LineasReservadas);
            colLineasReservadas.Width = 100;
            colLineasPreparadas.FieldName = Lenguaje.traduce(strings.LineasPreparadas);
            colLineasPreparadas.Name = Lenguaje.traduce(strings.LineasPreparadas);
            colLineasPreparadas.HeaderText = Lenguaje.traduce(strings.LineasPreparadas);
            colLineasPreparadas.Width = 100;
            colTareasPendientes.FieldName = Lenguaje.traduce(strings.TareasPendientes);
            colTareasPendientes.Name = Lenguaje.traduce(strings.TareasPendientes);
            colTareasPendientes.HeaderText = Lenguaje.traduce(strings.TareasPendientes);
        }

        private void llenarGridPedidos(object sender, DoWorkEventArgs e)
        {
            try
            {
                dtPrincipal = null;
                //CAMBIO tablaLlena = null;
                Business.GetOrdenesRecogidaEsquema(ref _lstEsquemaTablaPedidos);
                dtPrincipal = Business.GetOrdenesRecogidaDatosGridView(_lstEsquemaTablaPedidos);
                //CAMBIO tablaLlena = Business.GetOrdenesRecogidaDatosGridView(_lstEsquemaTabla);
                //CAMBIO GridDataView dataView = rgvGridPrincipal.MasterTemplate.DataView as GridDataView;
                lblCantidad.Text = Lenguaje.traduce("Registros:") + dtPrincipal.Rows.Count;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void bgWorkerGridViewPedidos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio bgWorkerGridViewPedidos_RunWorkerCompleted");
                DataTable tablaVacia = null;
                tablaVacia = dtPrincipal.Copy();
                tablaVacia.Rows.Clear();
                rgvGridPedidos.DataSource = tablaVacia;
                if (dtPrincipal.Rows.Count == 0)
                {
                    radWaitPedidos.StopWaiting();
                }
                else

                {
                    rgvGridPedidos.Refresh();
                    rgvGridPedidos.Columns.Clear();
                    rgvGridPedidos.DataSource = dtPrincipal;//CAMBIO tablaLlena;
                    if (rgvGridPedidos.Columns[0] != chkColPedidos)
                    {
                        rgvGridPedidos.Columns.Add(chkColPedidos);
                        rgvGridPedidos.Columns.Move(rgvGridPedidos.Columns.Count - 1, 0);
                    }
                    rgvGridPedidos.Refresh();
                    rgvGridPedidos.Columns.Remove(rgvGridPedidos.Columns["RowNum"]);
                    SetPreferencesPedidos();
                    ElegirEstilo();
                    GridDataView dataView = rgvGridPedidos.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                    colorearGridPedidos();

                    foreach (FilterDescriptor filtro in filtroAnteriorPedidos)
                    {
                        rgvGridPedidos.FilterDescriptors.Add(filtro);
                    }

                    radWaitPedidos.StopWaiting();
                }
                rgvGridPedidos.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                SetPreferencesPedidos();
                radWaitPedidos.StopWaiting();
            }
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio bgWorkerGridViewPedidos_RunWorkerCompleted");
            this.ResumeLayout();
        }

        private void colorearGridPedidos()
        {
            try
            {
                if (rgvGridPedidos.Rows.Count < 1) return;
                foreach (GridViewRowInfo rowInfo in rgvGridPedidos.Rows)
                {
                    colorEstado(rowInfo);
                    colorRutas(rowInfo);
                    colorLanzamiento(rowInfo);
                    colorCliente(rowInfo);
                    colorAgencia(rowInfo);
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarErrorNuevo(e);
            }
        }

        private void setFiltros1()
        {
            try
            {
                CompositeFilterDescriptor compositeFilter = new CompositeFilterDescriptor();
                compositeFilter.FilterDescriptors.Add(Lenguaje.traduce(strings.EstadoPedido), FilterOperator.IsEqualTo, "PC");
                compositeFilter.LogicalOperator = FilterLogicalOperator.And;
                compositeFilter.FilterDescriptors.Add(Lenguaje.traduce(strings.FechaPrevistaEnvio), FilterOperator.IsEqualTo, DateTime.Today);
                FilterDescriptor filter = new FilterDescriptor(Lenguaje.traduce(strings.EstadoPedido), FilterOperator.IsNotEqualTo, "PC");
                CompositeFilterDescriptor filterDescriptor2 = new CompositeFilterDescriptor();
                filterDescriptor2.FilterDescriptors.Add(compositeFilter);
                filterDescriptor2.FilterDescriptors.Add(filter);
                filterDescriptor2.LogicalOperator = FilterLogicalOperator.Or;
                rdfFiltro.DataSource = rgvGridPedidos.DataSource;
                rdfFiltro.Expression = filterDescriptor2.Expression;
                rdfFiltro.ApplyFilter();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion LlenarGridPrincipal

        #region LlenarGridRecogida

        public string ExtraerIdOrdenesCli()
        {
            string ordenesFab = string.Empty;
            List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
            ObtenerChildRowRecursivo(rgvGridPedidos.ChildRows, listaRecursiva);
            foreach (GridViewRowInfo rowInfo in listaRecursiva)
            {
                if (Convert.ToBoolean(rowInfo.Cells[Utilidades.buscarDondeEstaCheckBox(ref rgvGridPedidos)].Value) == true)
                {
                    ordenesFab = ordenesFab + rowInfo.Cells[Lenguaje.traduce(strings.NumPedidoCliente)].Value.ToString() + " ,";
                }
            }
            if (ordenesFab == string.Empty)
            { }
            else
            {
                ordenesFab = ordenesFab.Substring(0, ordenesFab.Length - 2);
            }
            return ordenesFab;
        }

        private void llenarGridRecogida(object sender, DoWorkEventArgs e)
        {
            try
            {
                filterPedido = ExtraerIdOrdenesCli();
                if (filterPedido == string.Empty)
                {
                    dtPrincipal = null;
                    dtPrincipal = Business.GetOrdenesRecogidaNoMarcadasCabDatosGridView(_lstEsquemaTablaOrdenesRecogida);
                }
                else
                {
                    dtPrincipal = null;
                    dtPrincipal = Business.GetOrdenesRecogidaCabDatosGridView(_lstEsquemaTablaOrdenesRecogida, filterPedido);
                }
                GridDataView dataView = rgvGridOrdenes.MasterTemplate.DataView as GridDataView;
                lblCantidad.Text = Lenguaje.traduce("Registros:") + 0;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void bgWorkerGridViewOrdenes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable tablaVacia = null;
                tablaVacia = dtPrincipal.Copy();
                tablaVacia.Rows.Clear();

                rgvGridOrdenes.DataSource = tablaVacia;
                if (dtPrincipal.Rows.Count == 0)
                {
                    radWaitOrdenes.StopWaiting();
                }
                else
                {
                    rgvGridOrdenes.Columns.Add(chkColOrdenesRecogida);
                    rgvGridOrdenes.Columns.Move(rgvGridOrdenes.Columns.Count - 1, 0);
                    rgvGridOrdenes.Refresh();
                    rgvGridOrdenes.DataSource = dtPrincipal; //CAMBIO tablaLlena;
                    rgvGridOrdenes.BestFitColumns();
                    rgvGridOrdenes.Refresh();
                    rgvGridOrdenes.Columns.Remove(rgvGridOrdenes.Columns["RowNum"]);
                    rgvGridOrdenes.Refresh();
                    ElegirEstilo();
                    SetPreferencesRecogida();
                    rgvGridOrdenes.BestFitColumns();
                    GridDataView dataView = rgvGridOrdenes.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                    radWaitOrdenes.StopWaiting();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                SetPreferencesRecogida();
                radWaitOrdenes.StopWaiting();
            }
            this.ResumeLayout();
        }

        private void FiltrosOrdenesRecogida()
        {
            FilterDescriptor filter = new FilterDescriptor(Lenguaje.traduce(strings.Estado), FilterOperator.IsNotEqualTo, "PC");
            rgvGridOrdenes.FilterDescriptors.Add(filter);
        }

        #endregion LlenarGridRecogida

        #region LlenarGridCargaCab

        private void llenarGridCarga(object sender, DoWorkEventArgs e)
        {
            try
            {
                dtPrincipal = null;
                dtPrincipal = Business.GetOrdenRecogidaExpedicionesDatosGridView(_lstEsquemaTablaOrdenesCarga);
                lblCantidad.Text = Lenguaje.traduce("Registros:") + 0;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void llenarGridCargaPLPte(object sender, DoWorkEventArgs e)
        {
            try
            {
                dtPLPteCargas = null;
                Business.GetOrdenesCargaPackingListPteEsquema(ref _lstEsquemaTablaCargaPackingPte);

                dtPLPteCargas = Business.GetOrdenRecogidaExpedicionesPLPteDatosGridView(_lstEsquemaTablaCargaPackingPte);
                GridDataView dataView = rgvGridCargaPLPteCarga.MasterTemplate.DataView as GridDataView;

                lblCantidad.Text = Lenguaje.traduce("Registros:") + 0;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void llenarGridCargaPacking(int cargaSel)
        {
            try
            {
                if (cargaSel > 0)
                {
                    DataTable packingTabla = Business.GetOrdenCargaJerarquicoPackingList(_lstEsquemaTablaCargaPacking, cargaSel.ToString());
                    rgvGridCargaPackingList.Columns.Clear();
                    rgvGridCargaPackingList.DataSource = packingTabla;

                    rgvGridCargaPackingList.Columns.Add(chkColJerarquicoOCPackingList);
                    rgvGridCargaPackingList.Columns.Move(chkColJerarquicoOCPackingList.Index, 0);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void llenarGridCargaPedidos(int cargaSel)
        {
            try
            {
                if (cargaSel > 0)
                {
                    DataTable pedidosTabla = Business.GetOrdenCargaJerarquicoPedidos(_lstEsquemaTablaCargaPedidos, cargaSel.ToString());
                    rgvGridCargaPedidos.DataSource = pedidosTabla;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void llenarGridCargaContenido(int cargaSel)
        {
            try
            {
                if (cargaSel > 0)
                {
                    DataTable contenidoTabla = Business.GetOrdenCargaJerarquicoContenido(_lstEsquemaTablaCargaContenido, cargaSel.ToString());
                    rgvGridCargaContenido.DataSource = contenidoTabla;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void bgWorkerGridViewCarga_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable tablaVacia = null;/*new DataTable();*/
                tablaVacia = dtPrincipal.Copy();
                rgvGridCarga.DataSource = tablaVacia;

                if (dtPrincipal.Rows.Count == 0)
                {
                    rgvGridCarga.DataSource = dtPrincipal;
                    radWaitCarga.StopWaiting();
                }
                else
                {
                    rgvGridCarga.Refresh();
                    rgvGridCarga.Columns.Clear();

                    rgvGridCarga.DataSource = dtPrincipal;
                    if (rgvGridCarga.Columns[0] != chkColOrdenesCarga)
                    {
                        chkColOrdenesCarga = new GridViewCheckBoxColumn();
                        chkColOrdenesCarga.Name = "chkColOrdenesCarga";
                        chkColOrdenesCarga.EnableHeaderCheckBox = false;
                        chkColOrdenesCarga.HeaderText = "";
                        chkColOrdenesCarga.EditMode = EditMode.OnValueChange;
                        rgvGridCarga.Columns.Add(chkColOrdenesCarga);
                        rgvGridCarga.Columns.Move(chkColOrdenesCarga.Index, 0);
                    }
                    rgvGridCarga.BestFitColumns();
                    rgvGridCarga.Refresh();
                    rgvGridCarga.Columns.Remove(rgvGridCarga.Columns["RowNum"]);
                    rgvGridCarga.Refresh();
                    SetPreferencesCarga();
                    rgvGridCarga.BestFitColumns();

                    GridDataView dataView = rgvGridCarga.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                    radWaitCarga.StopWaiting();
                    FuncionesGenerales.setCheckBoxFalse(ref rgvGridCarga, chkColOrdenesCarga.Name);
                    cargarCargaSeleccionada(cargaSeleccionadaWorker);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                SetPreferencesCarga();
                radWaitCarga.StopWaiting();
            }

            this.ResumeLayout();
        }

        private void seleccionarPrimeraCarga()
        {
            try
            {
                if (rgvGridCarga.Rows.Count > 0)
                {
                    GridViewRowInfo row = rgvGridCarga.Rows[0];
                    row.Cells[0].Value = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void bgWorkerGridViewCargaPLPte_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable tablaVacia = null;

                rgvGridCargaPLPteCarga.DataSource = tablaVacia;

                if (dtPLPteCargas == null || dtPLPteCargas.Rows.Count == 0)
                {
                    rgvGridCargaPLPteCarga.DataSource = dtPLPteCargas;
                    radWaitCargaPLPte.StopWaiting();
                    LoadLayoutLocal();
                }
                else
                {
                    rgvGridCargaPLPteCarga.Refresh();
                    rgvGridCargaPLPteCarga.Columns.Clear();

                    rgvGridCargaPLPteCarga.DataSource = dtPLPteCargas;
                    if (rgvGridCargaPLPteCarga.Columns[0] != chkColPLPteCarga)
                    {
                        rgvGridCargaPLPteCarga.Columns.Add(chkColPLPteCarga);
                        rgvGridCargaPLPteCarga.Columns.Move(rgvGridCargaPLPteCarga.Columns.Count - 1, 0);
                    }
                    FuncionesGenerales.setCheckBoxFalse(ref rgvGridCargaPLPteCarga, chkColPLPteCarga.Name);
                    rgvGridCargaPLPteCarga.BestFitColumns();
                    rgvGridCargaPLPteCarga.Refresh();
                    SetPreferencesCarga();
                    radWaitCargaPLPte.StopWaiting();
                    ElegirEstilo();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                SetPreferencesCarga();
                radWaitCarga.StopWaiting();
            }
            this.ResumeLayout();
        }

        #endregion LlenarGridCargaCab

        #region Preferencias

        private void SetPreferencesPedidos()
        {
            try
            {
                Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridPedidos);
                rgvGridPedidos.MultiSelect = true;
                this.rgvGridPedidos.MasterTemplate.EnableGrouping = true;
                this.rgvGridPedidos.ShowGroupPanel = true;
                this.rgvGridPedidos.MasterTemplate.AutoExpandGroups = true;
                this.rgvGridPedidos.EnableHotTracking = true;
                this.rgvGridPedidos.MasterTemplate.AllowAddNewRow = false;
                this.rgvGridPedidos.MasterTemplate.AllowColumnResize = true;
                this.rgvGridPedidos.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvGridPedidos.AllowSearchRow = true;
                this.rgvGridPedidos.EnablePaging = false;
                this.rgvGridPedidos.TableElement.RowHeight = 40;
                this.rgvGridPedidos.AllowRowResize = false;
                this.rgvGridPedidos.MasterView.TableSearchRow.SearchDelay = 2000;
                this.rgvGridPedidos.MasterTemplate.EnableFiltering = true;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void SetPreferencesRecogida()
        {
            try
            {
                Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridOrdenes);
                rgvGridOrdenes.MultiSelect = true;
                this.rgvGridOrdenes.MasterTemplate.MultiSelect = true;
                this.rgvGridOrdenes.MasterTemplate.EnableGrouping = true;
                this.rgvGridOrdenes.ShowGroupPanel = true;
                this.rgvGridOrdenes.MasterTemplate.AutoExpandGroups = true;
                this.rgvGridOrdenes.EnableHotTracking = true;
                this.rgvGridOrdenes.MasterTemplate.AllowAddNewRow = false;
                this.rgvGridOrdenes.MasterTemplate.AllowColumnResize = true;
                this.rgvGridOrdenes.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvGridOrdenes.AllowSearchRow = true;
                this.rgvGridOrdenes.EnablePaging = false;
                this.rgvGridOrdenes.TableElement.RowHeight = 40;
                this.rgvGridOrdenes.AllowRowResize = false;
                this.rgvGridOrdenes.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvGridOrdenes.MasterTemplate.EnableFiltering = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void SetPreferencesCarga()
        {
            try
            {
                rgvGridCarga.BestFitColumns();
                Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCarga);

                this.rgvGridCarga.MultiSelect = true;
                this.rgvGridCarga.MasterTemplate.EnableGrouping = true;
                this.rgvGridCarga.ShowGroupPanel = true;
                this.rgvGridCarga.MasterTemplate.AutoExpandGroups = true;
                this.rgvGridCarga.EnableHotTracking = true;
                this.rgvGridCarga.MasterTemplate.AllowAddNewRow = false;
                this.rgvGridCarga.MasterTemplate.AllowColumnResize = true;
                this.rgvGridCarga.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvGridCarga.AllowSearchRow = true;
                this.rgvGridCarga.EnablePaging = false;
                this.rgvGridCarga.TableElement.RowHeight = 40;
                this.rgvGridCarga.AllowRowResize = false;
                this.rgvGridCarga.MasterView.TableSearchRow.SearchDelay = 2000;
                this.rgvGridCarga.Dock = System.Windows.Forms.DockStyle.Fill;
                this.rgvGridCarga.MasterTemplate.EnableFiltering = true;

                rgvGridCargaPLPteCarga.BestFitColumns();
                Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridCargaPLPteCarga);

                this.rgvGridCargaPLPteCarga.MultiSelect = true;
                this.rgvGridCargaPLPteCarga.MasterTemplate.EnableGrouping = true;
                this.rgvGridCargaPLPteCarga.ShowGroupPanel = true;
                this.rgvGridCargaPLPteCarga.MasterTemplate.AutoExpandGroups = true;
                this.rgvGridCargaPLPteCarga.EnableHotTracking = true;
                this.rgvGridCargaPLPteCarga.MasterTemplate.AllowAddNewRow = false;
                this.rgvGridCargaPLPteCarga.MasterTemplate.AllowColumnResize = true;
                this.rgvGridCargaPLPteCarga.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvGridCargaPLPteCarga.AllowSearchRow = true;
                this.rgvGridCargaPLPteCarga.TableElement.RowHeight = 40;
                this.rgvGridCargaPLPteCarga.AllowRowResize = false;
                this.rgvGridCargaPLPteCarga.MasterView.TableSearchRow.SearchDelay = 2000;
                this.rgvGridCargaPLPteCarga.Dock = System.Windows.Forms.DockStyle.Fill;
                this.rgvGridCargaPLPteCarga.MasterTemplate.EnableFiltering = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Preferencias

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
            RadMenuItem mainItem = btnVistaPedidosConfiguracion.Items[0] as RadMenuItem;
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(temasItem_Click);
            mainItem.Items.Add(temasItem);

            RadMenuItem mainItem2 = btnVistaOrdenesConfiguracion.Items[0] as RadMenuItem;
            RadMenuItem temasItem2 = new RadMenuItem();
            temasItem2.Text = themeName;
            temasItem2.Image = image;
            temasItem2.Click += new EventHandler(temasItem_Click);
            mainItem2.Items.Add(temasItem2);

            RadMenuItem mainItem3 = btnVistaCargaConfiguracion.Items[0] as RadMenuItem;
            RadMenuItem temasItem3 = new RadMenuItem();
            temasItem3.Text = themeName;
            temasItem3.Image = image;
            temasItem3.Click += new EventHandler(temasItem_Click);
            mainItem3.Items.Add(temasItem3);
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

        #region Control Barra Superior

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void radPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        #endregion Control Barra Superior

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
                            this.radProgressBarElement.Value1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Value), 0));/* Convert.ToInt16(this.Value);*/
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

        //Si se puede modificar devolvera true.
        private bool comprovarEsCargaNoCerrada()
        {
            int _cargaIdSeleccionado = getIdCargaSeleccionada();
            foreach (GridViewRowInfo row in rgvGridCarga.Rows)
            {
                if (row.Cells["" + Lenguaje.traduce("Num Carga") + ""].Value == null) break;
                if (row.Cells["" + Lenguaje.traduce("Num Carga") + ""].Value.ToString().Equals(_cargaIdSeleccionado.ToString()))
                {
                    if (row.Cells[Lenguaje.traduce("Carga Estado")] != null &&
                    row.Cells[Lenguaje.traduce("Carga Estado")].Value != null &&
                    !row.Cells[Lenguaje.traduce("Carga Estado")].Value.ToString().Equals("PC"))
                    {
                        return true;
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("La carga seleccionada está cerrada."));
                        log.Debug("Se ha intentado modificar una carga ya cerrada.(" + _cargaIdSeleccionado + ")");
                        return false;
                    }
                }
            }
            return false;
        }

        private void btnAccionGenerarTarea_Click(object sender, EventArgs e)
        {
            if (!comprovarEsCargaNoCerrada())
            {
                return;
            }
            string json = formarJSONWSCarga();
            if (json != null)
            {
                string resp = llamarWSGenerarTareaCarga(json);
                var jss = new JavaScriptSerializer();
                dynamic d = jss.DeserializeObject(resp);
                var error = d[0]["Error"];
                if (error == string.Empty)
                {
                    MessageBox.Show(Lenguaje.traduce("Tarea generada correctamente."));
                    rgvGridCarga.DataSource = null;
                    GenerarGridOrdenesCarga();
                }
                else
                {
                    MessageBox.Show(error);
                }
            }
        }

        private void btnAccionConfirmaCarga_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            radWaitCarga.StartWaiting();
            bool cerrar = true;
            try
            {
                int idCarga = getIdCargaSeleccionada();
                if (idCarga > 0)
                {
                    if (mostrarAvisoCierreCarga)
                    {
                        FrmResumenPedidosCarga frm = new FrmResumenPedidosCarga(idCarga, false);
                        frm.ShowDialog();
                        if (frm.DialogResult != DialogResult.OK)
                        {
                            cerrar = false;
                        }
                    }

                    if (cerrar)
                    {
                        if (!comprovarEsCargaNoCerrada())
                        {
                            return;
                        }
                        string json = formarJSONWSCarga();
                        if (json != null)
                        {
                            string resp = llamarWSConfirmarCarga(json);
                            var jss = new JavaScriptSerializer();
                            dynamic d = jss.DeserializeObject(resp);
                            var error = d[0]["Error"];
                            if (error == string.Empty)
                            {
                                MessageBox.Show(Lenguaje.traduce("Carga confirmada correctamente."));
                                rgvGridCarga.DataSource = null;
                                GenerarGridOrdenesCarga();
                            }
                            else
                            {
                                MessageBox.Show(error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce("Se ha cancelado la operacion"));
                    }
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error confirmando la carga."));
                log.Error(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                radWaitCarga.StopWaiting();
            }
        }

        private void btnAccionCerrarCarga_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            radWaitCarga.StartWaiting();
            try
            {
                if (!comprovarEsCargaNoCerrada())
                {
                    return;
                }
                string json = formarJSONWSCarga();
                if (json != null)
                {
                    llamarWSCerrarCarga(json);
                    rgvGridCarga.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce("Error llamando al web service de carga."), "Error");
                log.Error(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                radWaitCarga.StopWaiting();
            }
            GenerarGridOrdenesCarga();
        }

        /*#endregion*/

        #region webservice

        private string llamarWSConfirmarCarga(string json)
        {
            WSCargaMotorClient webserviceConfirmarCarga = new WSCargaMotorClient();
            string respuesta = webserviceConfirmarCarga.confirmarCargaJson(json, DatosThread.getInstancia().getArrayDatos());
            Debug.WriteLine(respuesta);
            return respuesta;
        }

        private string llamarWSQuitarIdPLCarga(string json)
        {
            WSCargaMotorClient webserviceQuitarIdPLCarga = new WSCargaMotorClient();
            string respuesta = webserviceQuitarIdPLCarga.quitarIdentificadoresPLaCarga(json);
            Debug.WriteLine(respuesta);
            return respuesta;
        }

        private string llamarWSGenerarTareaCarga(string json)
        {
            WSCargaMotorClient webserviceGenerarTareaCarga = new WSCargaMotorClient();
            string respuesta = webserviceGenerarTareaCarga.generarTareaCarga(json);
            Debug.WriteLine(respuesta);
            return respuesta;
        }

        private void llamarWSCerrarCarga(string json)
        {
            //getSelectedRowCarga();
            int carga = getIdCargaSeleccionada();
            if (carga > 0)
            {
                WSCargaMotorClient webserviceCerrarCarga = new WSCargaMotorClient();
                webserviceCerrarCarga.cerrarCarga(carga, DatosThread.getInstancia().getArrayDatos(), User.IdOperario, 0);
            }
        }

        private string formarJSONWSCarga()
        {
            //getSelectedRowCarga();
            dynamic objetoDinamico = new ExpandoObject();
            int carga = getIdCargaSeleccionada();
            if (carga > 0)
            {
                objetoDinamico.idCarga = carga;
                objetoDinamico.idusuario = User.IdUsuario;
                objetoDinamico.Error = "";
                string json = JsonConvert.SerializeObject(objetoDinamico, Formatting.Indented);
                json = "[" + json + "]";
                return json;
            }
            else
            {
                return null;
            }
        }

        private string formarJsonAgregarQuitarCarga(int cargaIdSeleccionado, List<int> ids)
        {
            dynamic objetoDinamico = new ExpandoObject();
            Object[] objetoDinamicoIds = new Object[ids.Count];
            string json = string.Empty;
            int i = 0;
            foreach (int id in ids)
            {
                dynamic objetoDinamicoId = new ExpandoObject();
                objetoDinamicoId.identificador = id;
                objetoDinamicoId.error = "";

                objetoDinamicoIds[i] = objetoDinamicoId;
                i++;
            }
            objetoDinamico.idcarga = cargaIdSeleccionado;
            objetoDinamico.identificadores = objetoDinamicoIds;
            objetoDinamico.Error = "";
            json = JsonConvert.SerializeObject(objetoDinamico, Formatting.Indented);

            return "[" + json + "]";
        }

        #endregion webservice

        private void btnAccionCrearCargaButton_Click(object sender, EventArgs e)
        {
            GenerarMantenimientos gen = new GenerarMantenimientos("CargaCab", 20030, "OrdenCargaCab");
            gen.Show();
            gen.FormClosed += MantenimientoCarga_FormClosed;
        }

        private void MantenimientoCarga_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnVistaCargaRefrescar_Click(sender, e);
        }

        private void btnVistaCargaCarga_Click(object sender, EventArgs e)
        {
        }

        private void btnVistaPedidosConfiguracion_Click(object sender, EventArgs e)
        {
        }

        private void rBtnAccionImprimirInforme_Click(object sender, EventArgs e)
        {
            string parametros = "";
            int idOrdenRecogida = -1;
            foreach (GridViewRowInfo rowinfo in rgvGridOrdenes.Rows)
            {
                if (Convert.ToBoolean(rowinfo.Cells[Utilidades.buscarDondeEstaCheckBox(ref rgvGridPedidos)].Value) == true)
                {
                    idOrdenRecogida = Convert.ToInt32(rowinfo.Cells["" + Lenguaje.traduce(strings.NumOrdenRecogida) + ""].Value);
                    break;
                }
            }
            if (idOrdenRecogida <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            FrmSeleccionTipoMovimiento frm = new FrmSeleccionTipoMovimiento();
            frm.ShowDialog();
            frm.TopMost = true;
            if (!frm.DialogResult.Equals(DialogResult.OK))
            {
                return;
            }

            string tipoSeleccionado = frm.tipoMovimiento;
            parametros = "IDORDENRECOGIDA=" + idOrdenRecogida + ";IDRESERVATIPO=" + tipoSeleccionado;
            int idInforme = 14;
            if (tipoSeleccionado.Equals("RP"))
            {
                idInforme = 15;
            }
            VisorInforme v = new VisorInforme(idInforme, parametros);
            v.TopMost = true;
            v.ShowDialog();
        }

        private void RBtnCerrarOrden_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> listaOrdenes = new List<int>();
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridOrdenes.ChildRows, listaRecursiva);

                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        listaOrdenes.Add(int.Parse(row.Cells[Lenguaje.traduce("Num Orden Recogida")].Value.ToString()));
                    }
                }
                if (listaOrdenes.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona almenos una línea"), "Error");
                    return;
                }
                string ordenesrecogida = "";
                for (int i = 0; i < listaOrdenes.Count; i++)
                {
                    ordenesrecogida += listaOrdenes[i] + ",";
                }
                ordenesrecogida = ordenesrecogida.Remove(ordenesrecogida.LastIndexOf(','), 1);
                if (RadMessageBox.Show(this, Lenguaje.traduce("Se van a cerrar las siguientes ordenes:") + ordenesrecogida, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
                foreach (var orden in listaOrdenes)
                {
                    int idOperario = 0; //int.Parse(Persistencia.getParametro("OPERARIO").ToString());
                    int idRecurso = 0; //int.Parse(Persistencia.getParametro("IDRECURSO"));
                    int ordenRecogida = orden;

                    cerrarOrdenRecogida(ordenRecogida, idOperario, idRecurso);
                }

                RadMessageBox.Show(Lenguaje.traduce("Orden cerrada correctamente!"));
                refrescarOrdenes();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rBtnAsignarMuelle_Click(object sender, EventArgs e)
        {
            AsignarMuelle();
        }

        private void AsignarMuelle()
        {
            try
            {
                List<int> listaOrdenes = new List<int>();
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridOrdenes.ChildRows, listaRecursiva);
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        listaOrdenes.Add(int.Parse(row.Cells[Lenguaje.traduce("Num Orden Recogida")].Value.ToString()));
                    }
                }
                if (listaOrdenes.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona almenos una línea"), "Error");
                    return;
                }
                int ordenRecogida = listaOrdenes[0];
                if (ordenRecogida > 0)
                {
                    ElegirMuelle frm = new ElegirMuelle();
                    frm.ShowDialog();
                    int idMuelle = frm.idMuelle;

                    if (idMuelle > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        WSPedidoCliMotorClient ws = new WSPedidoCliMotorClient();
                        log.Debug("Se va asignar muelle " + idMuelle + " a la orden de recogida " + ordenRecogida);
                        ws.asignarMuelleOrdenRecogida(ordenRecogida, idMuelle, User.IdUsuario);
                        MessageBox.Show(Lenguaje.traduce("Muelle asignado correctamente"));
                        Cursor.Current = Cursors.Arrow;
                    }
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                }
                this.btnVistaOrdenesRefrescar.PerformClick();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void rBtnCancelarReservaSeleccionadas_Click(object sender, EventArgs e)
        {
            FilterDescriptorCollection filtro = new FilterDescriptorCollection();
            GroupDescriptorCollection description = new GroupDescriptorCollection();
            try
            {
                List<CancelarReserva> listaCancelar = new List<CancelarReserva>();
                Cursor.Current = Cursors.WaitCursor;
                bool ordenSeleccionada = false;
                string jsonWS = string.Empty;

                foreach (GridViewRowInfo rowinfo in rgvGridOrdenes.Rows)
                {
                    bool selecionada = false;
                    bool agrupadas = false;
                    GridViewRowInfo info = null;
                    if (rowinfo.Cells[0].Value != null)
                    {
                        if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                        {
                            for (int i = 0; i < rowinfo.ChildRows.Count; i++)
                            {
                                /*  if (Convert.ToBoolean(rowinfo.ChildRows[i].Cells[0].Value) == true)
                                  {
                                      selecionada = true;
                                      info = rowinfo.ChildRows[i];
                                      agrupadas = true;
                                  }*/
                                selecionada = true;
                                info = rowinfo;
                                agrupadas = true;
                            }
                        }
                    }

                    if (selecionada)
                    {
                        ordenSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow;
                        if (agrupadas)
                        {
                            hierarchyRow = info as GridViewHierarchyRowInfo;
                        }
                        else
                        {
                            hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        }
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Reservas))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    CancelarReserva cancelarReserva = new CancelarReserva();
                                    cancelarReserva.idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce(strings.NumReserva)].Value);
                                    cancelarReserva.idusuario = User.IdUsuario;
                                    cancelarReserva.error = "";
                                    listaCancelar.Add(cancelarReserva);
                                }
                            }
                        }
                        else
                        {
                            RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar alguna reserva"));
                            break;
                        }
                    }
                }
                if (ordenSeleccionada)
                {
                    if (listaCancelar.Count > 0)
                    {
                        bool hayError = false;
                        jsonWS = JsonConvert.SerializeObject(listaCancelar);
                        var respuestaWS = wsPedidoCli.cancelarReserva(jsonWS);
                        List<CancelarReserva> errorText = JsonConvert.DeserializeObject<List<CancelarReserva>>(respuestaWS);
                        for (int i = 0; i < errorText.Count; i++)
                        {
                            if (errorText[i].error != string.Empty)
                            {
                                hayError = true;
                                MessageBox.Show(errorText[i].error);
                            }
                        }
                        if (!hayError)
                        {
                            MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        Utilidades.refrescarJerarquico(ref this.rgvGridOrdenes, -1);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar alguna reserva"));
                    }
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la orden"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void btnModificarReserva_Click(object sender, EventArgs e)
        {
            ModificarReserva();
        }

        private void ModificarReserva()
        {
            try
            {
                bool ordenSeleccionada = false;
                int idReserva = -1;
                foreach (GridViewRowInfo rowinfo in rgvGridOrdenes.ChildRows)
                {
                    if (rowinfo.GetType().Name == "GridViewGroupRowInfo")
                    {
                        foreach (var subrow in rowinfo.Group)
                        {
                            if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                            {
                                ordenSeleccionada = true;
                                GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                                if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Reservas))
                                {
                                    int noOfChildRows = hierarchyRow.ChildRows.Count;

                                    //looping through the child rows
                                    foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                                    {
                                        //check if its current child row
                                        if (row.IsSelected)
                                        {
                                            idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce(strings.NumReserva)].Value);

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
                    }
                    else
                    {
                        if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                        {
                            ordenSeleccionada = true;
                            GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                            if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Reservas))
                            {
                                int noOfChildRows = hierarchyRow.ChildRows.Count;

                                //looping through the child rows
                                foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                                {
                                    //check if its current child row
                                    if (row.IsSelected)
                                    {
                                        idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce(strings.NumReserva)].Value);

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
                }

                if (ordenSeleccionada)
                {
                    if (idReserva > 0)
                    {
                        ModificarReserva mrf = new ModificarReserva(idReserva);
                        mrf.ShowDialog();
                        Utilidades.refrescarJerarquico(ref this.rgvGridOrdenes, -1);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar alguna reserva"));
                    }
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la orden"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void btnCambiarExistencia_Click(object sender, EventArgs e)
        {
            CambiarExistencia();
        }

        private bool EsEquivalente(int idEntradaOld, int idEntradaNew)
        {
            bool esEquivalente = false;

            esEquivalente = wssMotor.esEquivalente(idEntradaOld, idEntradaNew);
            return esEquivalente;
        }

        private bool IntercambiarPalets(int idReserva, int idEntrada)
        {
            try
            {
                wssMotor.intercambiarPalets(idReserva, idEntrada, User.IdOperario);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                return false;
            }
        }

        private void CambiarExistencia()
        {
            try
            {
                bool ordenSeleccionada = false;
                int idReserva = -1;
                int idEntradaAIntercambiar = -1;
                foreach (GridViewRowInfo rowinfo in rgvGridOrdenes.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        ordenSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Reservas))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce(strings.NumReserva)].Value);
                                    idEntradaAIntercambiar = Convert.ToInt32(row.Cells[Lenguaje.traduce(strings.Matricula)].Value);
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
                if (ordenSeleccionada)
                {
                    if (idReserva > 0)
                    {
                        FrmSeleccionarExistencia mrf = new FrmSeleccionarExistencia(idReserva);
                        mrf.ShowDialog(this);
                        if (mrf.DialogResult == DialogResult.OK)
                        {
                            int idEntrada = mrf.idEntrada;
                            if (EsEquivalente(idEntradaAIntercambiar, idEntrada))
                            {
                                // si se ha podido intercambiar
                                if (IntercambiarPalets(idReserva, idEntrada))
                                {
                                    Utilidades.refrescarJerarquico(ref this.rgvGridOrdenes, -1);
                                }
                                else
                                {
                                    RadMessageBox.Show(Lenguaje.traduce("No se ha podido intercambiar el palet"));
                                }
                            }
                            else
                            {
                                RadMessageBox.Show(Lenguaje.traduce("El palet no es equivalente"));
                            }
                        }
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar alguna reserva"));
                    }
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la orden"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnCerrarPedido_Click(object sender, EventArgs e)
        {
            CerrarPedido();
        }

        private void CerrarPedido()
        {
            try
            {
                List<LiberarPedidoCli> listaPedidos = new List<LiberarPedidoCli>();
                foreach (GridViewRowInfo row in rgvGridPedidos.Rows)
                {
                    LiberarPedidoCli pedido = new LiberarPedidoCli();
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        pedido.idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value);
                        listaPedidos.Add(pedido);
                    }
                }

                if (RadMessageBox.Show(this, Lenguaje.traduce("Se van a cerrar la siguiente cantidad de pedidos:") + listaPedidos.Count, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }

#pragma warning disable CS0219 // La variable 'pedidoCont' está asignada pero su valor nunca se usa
                int pedidoCont = 1;
#pragma warning restore CS0219 // La variable 'pedidoCont' está asignada pero su valor nunca se usa
                foreach (LiberarPedidoCli pedido in listaPedidos)
                {
                    List<LiberarPedidoCli> pedidoList = new List<LiberarPedidoCli>();
                    wsSalidaMotor.cerrarPedido(pedido.idpedidocli, DatosThread.getInstancia().getArrayDatos(), User.IdOperario, 0);
                }

                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                rgvGridPedidos.Columns.Clear();
                rgvGridPedidos.Templates[0].Columns.Clear();
                GenerarGridPedidos();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnServirSeleccionadas_Click(object sender, EventArgs e)
        {
            try
            {
                List<CancelarReserva> listaCancelar = new List<CancelarReserva>();
                Cursor.Current = Cursors.WaitCursor;
                bool ordenSeleccionada = false;
                string jsonWS = string.Empty;
                foreach (GridViewRowInfo rowinfo in rgvGridOrdenes.ChildRows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        ordenSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Reservas))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    CancelarReserva cancelarReserva = new CancelarReserva();
                                    cancelarReserva.idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce(strings.NumReserva)].Value);
                                    cancelarReserva.idusuario = User.IdUsuario;
                                    cancelarReserva.error = "";
                                    listaCancelar.Add(cancelarReserva);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (ordenSeleccionada)
                {
                    if (listaCancelar.Count > 0)
                    {
                        bool hayError = false;
                        FrmSeleccionarFecha frm = new FrmSeleccionarFecha();
                        frm.ShowDialog();
                        DateTime fecha = frm.fechaSeleccionada;
                        jsonWS = JsonConvert.SerializeObject(listaCancelar);
                        var respuestaWS = wsPedidoCli.servirReserva(jsonWS, fecha, DatosThread.getInstancia().getArrayDatos(), User.IdOperario);
                        List<CancelarReserva> errorText = JsonConvert.DeserializeObject<List<CancelarReserva>>(respuestaWS);
                        for (int i = 0; i < errorText.Count; i++)
                        {
                            if (errorText[i].error == string.Empty || errorText[i].error == null)
                            { }
                            else
                            {
                                hayError = true;
                                MessageBox.Show(errorText[i].error);
                            }
                        }
                        if (!hayError)
                        {
                            MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        Utilidades.refrescarJerarquico(ref this.rgvGridOrdenes, -1);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar alguna reserva"));
                    }
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la orden"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void rBtnImprimirCarga_Click(object sender, EventArgs e)
        {
            try
            {
                FrmSeleccionInforme frm = new FrmSeleccionInforme("5800,5900,13800,14000");
                frm.ShowDialog();
                frm.TopMost = true;
                if (!frm.DialogResult.Equals(DialogResult.OK))
                {
                    return;
                }

                int idInforme = frm.idInforme;
                string parametros = "";
                int idCarga = -1;
                foreach (GridViewRowInfo rowinfo in rgvGridCarga.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        idCarga = Convert.ToInt32(rowinfo.Cells["" + Lenguaje.traduce("Num Carga") + ""].Value);
                        break;
                    }
                }
                if (idCarga <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }

                parametros = "IDCARGA=" + idCarga;

                VisorInforme v = new VisorInforme(idInforme, parametros);
                v.TopMost = true;
                v.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnOrganizador_Click(object sender, EventArgs e)
        {
            try
            {
                if (organizador == null || organizador.Created == false)
                {
                    organizador = new rRbnOrganizador(rRbnOrganizador.tipoOrganizador.Pedidos_Cliente);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private List<string> datosTotalesCarga(int carga)
        {
            List<string> datos = new List<string>();
            DataTable table = Business.ObtencionDatosTotalesCarga(carga);
            int uds = 0;
            decimal volumen = 0;
            double peso = 0;
            foreach (DataRow item in table.Rows)
            {
                uds += (int)item[1];
                volumen += (decimal)item[2];
                peso += (double)item[3];
            }
            datos.Add(uds.ToString());
            datos.Add(volumen.ToString());
            datos.Add(peso.ToString());
            datos.Add(table.Rows.Count.ToString());
            return datos;
        }

        private void radRibbonBarAcciones_Click(object sender, EventArgs e)
        {
        }

        private void rMnuItmOrdenManual_Click(object sender, EventArgs e)
        {
            try
            {
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                List<int> pedidosSeleccionados = new List<int>();
                ObtenerChildRowRecursivo(rgvGridPedidos.ChildRows, listaRecursiva);
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        pedidosSeleccionados.Add(Convert.ToInt32(row.Cells["" + Lenguaje.traduce(strings.NumPedidoCliente) + ""].Value));
                    }
                }
                if (pedidosSeleccionados.Count > 0)
                {
                    //FrmGenerarOrdenRecogidaManual frm = new FrmGenerarOrdenRecogidaManual(pedidosSeleccionados);
                    // frm.ShowDialog();
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar algún pedido"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Error generando orden manual. ERROR:" + ex.Message);
            }
        }
    }

    #region DragAndDropTreeView

    internal class CustomDragDropService : TreeViewDragDropService
    {
        private RadTreeViewElement owner;
        private RadTreeNode draggedNode;

        //Initialize the service
        public CustomDragDropService(RadTreeViewElement owner)
            : base(owner)
        {
            this.owner = owner;
        }

        //Save the dragged node
        protected override void PerformStart()
        {
            base.PerformStart();
            TreeNodeElement draggedNodeElement = this.Context as TreeNodeElement;
            this.draggedNode = draggedNodeElement.Data;
        }

        //Clean the saved node
        protected override void PerformStop()
        {
            base.PerformStop();
            this.draggedNode = null;
        }

        //If tree element is hovered, allow drop
        protected override void OnPreviewDragOver(RadDragOverEventArgs e)
        {
            base.OnPreviewDragOver(e);
            GridDataRowElement targetElement = e.HitTarget as GridDataRowElement;
            if (e.HitTarget is GridDataRowElement)
            {
                e.CanDrop = true;
            }
        }

        //Create copies of the selected node(s) and add them to the hovered node/tree
        protected override void OnPreviewDragDrop(RadDropEventArgs e)
        {
            GridDataRowElement targetRowElement = e.HitTarget as GridDataRowElement;

            List<RadTreeNode> draggedNodes = this.GetDraggedNodes(draggedNode);
            List<string> packings = new List<string>();

            bool copyNodes = this.IsCopyingNodes;
            foreach (RadTreeNode node in draggedNodes)
            {
                if (node.Level == 4)
                {
                    packings.Add(node.Tag.ToString());
                    Debug.WriteLine("Packing:" + node.Tag);
                }
                if (node.Level == 3)
                {
                    foreach (RadTreeNode childNode in node.Nodes)
                    {
                        packings.Add(childNode.Tag.ToString());
                        Debug.WriteLine("Packing:" + childNode.Tag);
                    }
                }
                if (node.Level == 2)
                {
                    foreach (RadTreeNode childNode in node.Nodes)
                    {
                        foreach (RadTreeNode childNode2 in childNode.Nodes)
                        {
                            packings.Add(childNode2.Tag.ToString());
                            Debug.WriteLine("Packing:" + childNode2.Tag);
                        }
                    }
                }
                if (node.Level == 1)
                {
                    foreach (RadTreeNode childNode in node.Nodes)
                    {
                        foreach (RadTreeNode childNode2 in childNode.Nodes)
                        {
                            foreach (RadTreeNode childNode3 in childNode2.Nodes)
                            {
                                packings.Add(childNode3.Tag.ToString());
                                Debug.WriteLine("Packing:" + childNode3.Tag);
                            }
                        }
                    }
                }
                if (node.Level == 0)
                {
                    foreach (RadTreeNode childNode in node.Nodes)
                    {
                        foreach (RadTreeNode childNode2 in childNode.Nodes)
                        {
                            foreach (RadTreeNode childNode3 in childNode2.Nodes)
                            {
                                foreach (RadTreeNode childNode4 in childNode3.Nodes)
                                {
                                    packings.Add(childNode4.Tag.ToString());
                                    Debug.WriteLine("Packing:" + childNode4.Tag);
                                }
                            }
                        }
                    }
                }
            }
            string carga = targetRowElement.RowInfo.Cells[0].Value.ToString();
            string json = FormarJsonAgregarIdentificadoresPLCarga(carga, packings);
            WSCargaMotorClient a = new WSCargaMotorClient();
            string resp = a.agregarIdentificadoresPLaCarga(json);
            Debug.WriteLine("Error:" + resp);
            var jss = new JavaScriptSerializer();
            bool lineasCorrectas = true;
            string errorLineas = string.Empty;
            dynamic d = jss.DeserializeObject(resp);
            int i = 0;
            foreach (var item in d[0]["identificadores"])
            {
                var errorLinea = d[0]["identificadores"][i]["Error"];
                if (errorLinea != string.Empty)
                {
                    lineasCorrectas = false;
                    //log.Error("Error creando recepción: " + resp);
                    break;
                }
            }
            string s = d[0]["Error"];
            if (s != string.Empty || lineasCorrectas == false)
            {
                MessageBox.Show(Lenguaje.traduce("Ha ocurrido un error añadiendo los Packing List a la orden"));
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Se han añadido los packing a la carga"));
            }
            //this.owner.EndUpdate();
            //targetTreeView.EndUpdate();
        }

        private string FormarJsonAgregarIdentificadoresPLCarga(string idcarga, List<string> packingLists)
        {
            string json = string.Empty;
            dynamic objDinamico = new ExpandoObject();

            objDinamico.idcarga = idcarga;
            Object[] arrayLineas = new Object[packingLists.Count];
            int i = 0;
            foreach (string item in packingLists)
            {
                dynamic objDinamico2 = new ExpandoObject();
                objDinamico2.identificador = item;
                objDinamico2.Error = "";
                arrayLineas[i] = objDinamico2;
                i++;
            }
            string packings = "[" + JsonConvert.SerializeObject(arrayLineas) + "]";

            objDinamico.identificadores = arrayLineas;
            objDinamico.Error = "";
            json = "[" + JsonConvert.SerializeObject(objDinamico) + "]";
            Debug.WriteLine(json);
            return json;
        }

        //Return a copy of a node
        protected virtual RadTreeNode CreateNewTreeNode(RadTreeNode node)
        {
            return node.Clone() as RadTreeNode;
        }
    }

    internal class CustomTreeViewElement : RadTreeViewElement
    {
        //Enable themeing for the element
        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(RadTreeViewElement);
            }
        }

        //Replace the default drag drop service with the custom one
        protected override TreeViewDragDropService CreateDragDropService()
        {
            return new CustomDragDropService(this);
        }
    }

    internal class CustomTreeView : RadTreeView
    {
        //Replace the default element with the custom one
        protected override RadTreeViewElement CreateTreeViewElement()
        {
            return new CustomTreeViewElement();
        }

        //Enable theming for the control
        public override string ThemeClassName
        {
            get
            {
                return typeof(RadTreeView).FullName;
            }
        }

        #endregion DragAndDropTreeView
    }

    public class LanzarPedidoCli
    {
        [JsonProperty]
        public int idpedidocli { get; set; }

        public int muelle { get; set; }
        public bool reservaautomatica { get; set; }
        public bool recursoautomatico { get; set; }
        public string lanzamiento { get; set; }
        public int idrecurso { get; set; }
        public int idusuario { get; set; }
        public string error { get; set; }
        public List<int> ordenes { get; set; }
        public List<string> fechas { get; set; }
    }

    public class Orden
    {
        private List<LanzarPedidoCli> pedidoClis { get; set; }
    }

    public class LiberarPedidoCli
    {
        [JsonProperty]
        public int idpedidocli { get; set; }

        public int idusuario { get; set; }
        public string error { get; set; }
    }

    public class OrdenLiberar
    {
        private List<LiberarPedidoCli> pedidoClis { get; set; }
    }

    public class CancelarReserva
    {
        [JsonProperty]
        public int idReserva { get; set; }

        public int idusuario { get; set; }
        public string error { get; set; }
    }

    public class CancelarReservaList
    {
        private List<CancelarReserva> cancelarReservas { get; set; }
    }

    public class ServirReserva
    {
        [JsonProperty]
        public int idordenrecogida { get; set; }

        public string fechaservicio { get; set; }
        public int idusuario { get; set; }
        public List<String> error { get; set; }
    }

    public class ReservarOrden
    {
        [JsonProperty]
        public int idordenrecogida { get; set; }

        public bool recursoAutomatico { get; set; }
        public int idRecurso { get; set; }
        public int idusuario { get; set; }

        public List<String> error { get; set; }
    }

    public class ServirReservaList
    {
        private List<ServirReserva> servirReservas { get; set; }
    }

    #region Drag&Drop

    public class RowSelectionGridBehavior : GridHierarchyRowBehavior
    {
        protected override bool OnMouseDownLeft(MouseEventArgs e)
        {
            GridDataRowElement row = this.GetRowAtPoint(e.Location) as GridDataRowElement;
            if (row != null)
            {
                RadGridViewDragDropService svc = this.GridViewElement.GetService<RadGridViewDragDropService>();
                svc.Start(row);
            }
            return base.OnMouseDownLeft(e);
        }
    }

    public class CustomGridDataRowBehavior : GridDataRowBehavior
    {
        protected override bool OnMouseDownLeft(MouseEventArgs e)
        {
            GridDataRowElement row = this.GetRowAtPoint(e.Location) as GridDataRowElement;
            if (row != null)
            {
                RadGridViewDragDropService svc = this.GridViewElement.GetService<RadGridViewDragDropService>();
                svc.AllowAutoScrollColumnsWhileDragging = false;
                svc.AllowAutoScrollRowsWhileDragging = false;
                svc.Start(row);
            }
            return base.OnMouseDownLeft(e);
        }
    }

    #endregion Drag&Drop
}
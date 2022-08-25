using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Herramientas.Stock;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGA.TareasMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.Data;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;
using System.Collections;
using RumboSGA.ReservaMotor;
using RumboSGA.Presentation.Herramientas.PantallasWS;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class ControlTareas : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        public int idFormulario;

        private DataTable tablaGrid = new DataTable();
        private DataTable tablaGridTemplate = new DataTable();
        private DataTable ubicaciones = new DataTable();
        private DataTable trasvase = new DataTable();
        private DataTable movCarro = new DataTable();
        private DataTable movBulto = new DataTable();
        private DataTable reposiciones = new DataTable();
        private DataTable devolucionCli = new DataTable();
        private DataTable conteo = new DataTable();
        private DataTable ordenRecogida = new DataTable();
        private DataTable picking = new DataTable();
        private DataTable ordenCarga = new DataTable();
        private DataTable acopios = new DataTable();

        private int _cantidadRegistros = 0;
        private WSTareasPendientesMotorClient tareasMotorClient; /*= new WSTareasPendientesMotorClient();*/
        private WSReservaMotorClient reservasMotorClient; /*= new WSReservaMotorClient();*/
        private DataTable table = new DataTable();
        private Dictionary<string, string> tipos = new Dictionary<string, string>();
        private string connectionString = ConexionSQL.getConnectionString();

        //SqlConnection conn = new SqlConnection();
        private GridViewTemplate template = new GridViewTemplate();

        private RadWaitingBar radWait = new RadWaitingBar();
        private BackgroundWorker bgWorkerGridViewTareas;
        private DataTable tablaLlena = new DataTable();
        private RadRibbonBarButtonGroup grupoLabel = new RadRibbonBarButtonGroup();
        private RadLabelElement lblCantidad = new RadLabelElement();

        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        public RadGridView gridViewControl
        {
            get { return this.rgvPrincipal; }
        }

        private GridViewDecimalColumn column = new GridViewDecimalColumn("");
        protected dynamic _selectedRow;
        protected GridScheme _esquemaGrid;
        public string name = "Tareas";
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        protected List<TableScheme> _lstEsquemaTabla = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaJerarquico = new List<TableScheme>();

        private string colSinReserva;
        private string colTipoTarea;
        private string colIDTipoTarea;
        private string colEstadoTarea;
        private string colPrerequisito;
        private string strHuerfana;
        private string colNumTarea;
        private string colPrioridad;
        private string colColorAMostrar;
        private string colDescripcionTarea;
        private string nombreEstilo;
        private string colCantidad;

        //Colores
        private Color PEColor;

        private Color PAColor;
        private Color PCColor;
        private Color ASColor;
        private Color REColor;
        private Color EJColor;
        private Color BPColor;
        private Color BQColor;
        private DataTable dtColorTareaTipo;
        private Hashtable coloresTipoTarea = new Hashtable();

        #endregion Variables

        #region Constructor

        public ControlTareas()
        {
            try
            {
                InitializeComponent();
                tareasMotorClient = new WSTareasPendientesMotorClient();
                reservasMotorClient = new WSReservaMotorClient();
                IniciarControles();
                ControlesLenguaje();
                dtColorTareaTipo = ConexionSQL.getDataTable("SELECT IDTAREATIPO,COLORAMOSTRAR FROM TBLTAREASTIPO");
                CargarColores();
                this.Shown += form_Shown;
                this.Show();
                InicializaWorkerTareas();
                Eventos();
                InicializarComboPaginacion();
                radWait.StartWaiting();
                bgWorkerGridViewTareas.RunWorkerAsync();

                this.Name = name;
                nombreEstilo = this.Name + "VistaControlTareas.xml";

                FuncionesGenerales.AddEliminarLayoutButton(ref configButton);
                if (this.configButton.Items["RadMenuItemEliminarLayout"] != null)
                {
                    this.configButton.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        if (gridViewControl is RadGridView)
                        {
                            FuncionesGenerales.EliminarLayout(nombreEstilo, gridViewControl);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private Color CalculaColor(int c)
        {
            Color colorAlpha = Color.White;
            try
            {
                /*int c = -13436544;
                DataRow[] row = dtColorTareaTipo.Select("TIPOTAREA='" + tipoTarea + "'");
                if (row.Length > 0)
                {
                    c = Convert.ToInt32(row[0][1]);
                }*/

                Color color = new Color();
                color = Color.FromArgb(Convert.ToInt32(c));
                colorAlpha = Color.FromArgb(255, color);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            return colorAlpha;
        }

        private Color CalculaColor(string tipoTarea)
        {
            Color colorAlpha = Color.White;
            try
            {
                int c = -13436544;
                DataRow[] row = dtColorTareaTipo.Select("TIPOTAREA='" + tipoTarea + "'");
                if (row.Length > 0)
                {
                    c = Convert.ToInt32(row[0][1]);
                }

                Color color = new Color();
                color = Color.FromArgb(Convert.ToInt32(c));
                colorAlpha = Color.FromArgb(255, color);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            return colorAlpha;
        }

        private void CargarColores()
        {
            try
            {
                foreach (DataRow drTareatipo in dtColorTareaTipo.Rows)
                {
                    //log.Debug("tipo tarea: " + drTareatipo["IDTAREATIPO"].ToString());
                    //ExpressionFormattingObject obj1 = new ExpressionFormattingObject("MiCondicion" + drTareatipo["idtareatipo"], "[" + colIDTipoTarea + "] = " + drTareatipo["IDTAREATIPO"].ToString(), false);
                    //log.Debug("CellBackColor: ");
                    Color c = CalculaColor(Convert.ToInt32(drTareatipo["COLORAMOSTRAR"]));
                    if (coloresTipoTarea[Convert.ToInt32(drTareatipo["COLORAMOSTRAR"])] == null)
                    {
                        coloresTipoTarea.Add(Convert.ToInt32(drTareatipo["COLORAMOSTRAR"]), c);
                    }
                    //log.Debug("ConditionalFormattingObjectList: ");
                    //this.rgvPrincipal.Columns[colDescripcionTarea].ConditionalFormattingObjectList.Add(obj1);
                }
            }
            catch (Exception ex)
            {
                //ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            try
            {
                PEColor = XmlReaderPropio.getColorTareas("PE");
                PAColor = XmlReaderPropio.getColorTareas("PA");
                PCColor = XmlReaderPropio.getColorTareas("PC");
                REColor = Color.SkyBlue;// XmlReaderPropio.getColorTareas("RE");
                EJColor = XmlReaderPropio.getColorTareas("EJ");
                BPColor = XmlReaderPropio.getColorTareas("BP");
                BQColor = XmlReaderPropio.getColorTareas("BQ");
                ASColor = XmlReaderPropio.getColorTareas("AS");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void InicializaWorkerTareas()
        {
            bgWorkerGridViewTareas = new BackgroundWorker();
            bgWorkerGridViewTareas.WorkerReportsProgress = true;
            bgWorkerGridViewTareas.WorkerSupportsCancellation = true;

            bgWorkerGridViewTareas.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewTareas_RunWorkerCompleted);

            bgWorkerGridViewTareas.DoWork += new DoWorkEventHandler(llenarGridTareas);
        }

        private void IniciarControles()
        {
            try
            {
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                tlPanelPrincipal.Dock = DockStyle.Fill;
                rgvPrincipal.Dock = DockStyle.Fill;

                this.radWait.Name = "radWaitingBar1";
                this.radWait.Size = new System.Drawing.Size(200, 20);
                this.radWait.TabIndex = 2;
                this.radWait.Text = "";
                this.radWait.AssociatedControl = this.rgvPrincipal;
                InitializeThemesDropDown();
                EstablecerPreferenciasFiltros();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        private void ControlesLenguaje()
        {
            name = Lenguaje.traduce(strings.Tareas);
            this.Text = Lenguaje.traduce(NombresFormularios.ControlTareas);
            pedidosButton.Text = Lenguaje.traduce(strings.Pedidos);
            operarioButton.Text = Lenguaje.traduce("Recurso");
            editColumns.Text = Lenguaje.traduce(strings.Columnas);
            guardarButton.Text = Lenguaje.traduce(strings.GuardarEstilo);
            borrarButton.Text = Lenguaje.traduce(strings.LimpiarFiltros);
            cargarButton.Text = Lenguaje.traduce(strings.CargarEstilo);
            temasMenuItem.Text = Lenguaje.traduce(strings.Temas);
            lblCantidad.Text = Lenguaje.traduce(strings.NTareas) + ":" + CantidadRegistros;
            ribbonTab1.Text = Lenguaje.traduce(strings.Acciones);
            configBarGroup.Text = Lenguaje.traduce(strings.Configuracion);
            vistaBarGroup.Text = Lenguaje.traduce(strings.Ver);
            pagHeaderItem.Text = Lenguaje.traduce(strings.RegistrosPaginacion);
            colSinReserva = Lenguaje.traduce("Sin Reserva");
            colTipoTarea = Lenguaje.traduce("Tipo Tarea");
            colIDTipoTarea = Lenguaje.traduce("ID Tarea Tipo");
            strHuerfana = Lenguaje.traduce("Huerfana");
            colEstadoTarea = Lenguaje.traduce("Estado Tarea");
            colPrerequisito = Lenguaje.traduce("Prerrequisito");
            colNumTarea = Lenguaje.traduce("Num Tarea");
            colPrioridad = Lenguaje.traduce("Prioridad");
            colCantidad = Lenguaje.traduce("Cantidad");
            colDescripcionTarea = Lenguaje.traduce("Descripcion Tarea");
            colColorAMostrar = Lenguaje.traduce("Color a mostrar");
        }

        #endregion Constructor

        #region Llamada Eventos Propios

        private void Eventos()
        {
            try
            {
                rgvPrincipal.ViewRowFormatting += radGridView1_ViewRowFormatting;
                rgvPrincipal.FilterChanged += RadGridView1_FilterChanged;
                rgvPrincipal.ViewCellFormatting += radGridView1_ViewCellFormatting;
                rgvPrincipal.ChildViewExpanding += radGridView1_ChildViewExpanding;
                rgvPrincipal.GroupSummaryEvaluate += radGridView1_GroupSummaryEvaluate1;
                rgvPrincipal.ContextMenuOpening += radGridView1_ContextMenuOpening;
                rgvPrincipal.ViewCellFormatting += RadGridView1_ViewCellFormattingHierarchicalGrid;
                rgvPrincipal.CellFormatting += radGridView1_CellFormatting;
                rgvPrincipal.ViewCellFormatting += radGridView1_CellFormattingWrapText;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Llamada Eventos Propios

        #region Eventos

        #region Ocultar jerarquico si esta vacio

        private bool IsExpandable(GridViewRowInfo rowInfo)
        {
            if (rowInfo.Cells[colTipoTarea].Value == null
                || rowInfo.Cells[colTipoTarea].Value == null
                || rowInfo.Cells[colTipoTarea].Value == null
                || rowInfo.Cells[colTipoTarea].Value == null
                || rowInfo.Cells[colTipoTarea].Value == null
                || rowInfo.Cells[colTipoTarea].Value == null)
            {
                return false;
            }
            if (rowInfo.Cells[colTipoTarea].Value.ToString() == "PI"
            || rowInfo.Cells[colTipoTarea].Value.ToString() == "SP"
            || rowInfo.Cells[colTipoTarea].Value.ToString() == "SD"
            || rowInfo.Cells[colTipoTarea].Value.ToString() == "UP"
            || rowInfo.Cells[colTipoTarea].Value.ToString() == "RP"
            || rowInfo.Cells[colTipoTarea].Value.ToString() == "AC")
            {
                if (rowInfo.ChildRows != null && rowInfo.ChildRows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void radGridView1_ViewCellFormatting2(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
        }

        private void radGridView1_ChildViewExpanding(object sender, ChildViewExpandingEventArgs e)
        {
            e.Cancel = !IsExpandable(e.ParentRow);
        }

        #endregion Ocultar jerarquico si esta vacio

        private void radGridView1_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.HierarchyLevel == 1 && e.RowElement.RowInfo is GridViewNewRowInfo)
            {
                e.RowElement.RowInfo.MaxHeight = 1;
            }
        }

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex > 0 && e.ColumnIndex > -1)
                {
                    e.CellElement.TextWrap = true;
                    GridDataCellElement cell = e.CellElement as GridDataCellElement;
                    if (cell != null && cell.RowIndex > -1 && cell.ColumnInfo.Name == colDescripcionTarea)
                    {
                        if (rgvPrincipal.Rows[cell.RowIndex].Cells[colColorAMostrar] != null)
                        {
                            int color = -1;
                            if (cell.RowIndex < rgvPrincipal.ChildRows.Count)
                                color = Convert.ToInt32(rgvPrincipal.ChildRows[cell.RowIndex].Cells[colColorAMostrar].Value);

                            //César: Salta un error cuando el color no existe.
                            if (color > -1 && coloresTipoTarea[color] != null)
                                e.CellElement.BackColor = (Color)coloresTipoTarea[color];
                        }
                    }
                    else if (cell != null && cell.ColumnInfo.Name == colEstadoTarea)
                    {
                    }
                    else
                    {
                        e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radGridView1_CellFormattingWrapText(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
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

        protected void RadGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
                GridDataView dataView = rgvPrincipal.MasterTemplate.DataView as GridDataView;
                lblCantidad.Text = Lenguaje.traduce(strings.NTareas) + dataView.Indexer.Items.Count;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void radGridView1_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0 && e.RowIndex > -1 && e.CellElement is GridRowHeaderCellElement)
                {
                    /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                    {*/
                    if (rgvPrincipal.Rows[e.RowIndex].Cells[colSinReserva].Value.ToString() == strHuerfana
                    && rgvPrincipal.Rows[e.RowIndex].Cells[colTipoTarea].Value.ToString() != "PI")
                    {
                        e.CellElement.DrawImage = true;
                        e.CellElement.Image = Properties.Resources.Overdue;
                        e.CellElement.ImageAlignment = ContentAlignment.TopLeft;
                        e.CellElement.ToolTipText = Lenguaje.traduce("Esta tarea no tiene reserva");
                    }
                    else
                    {
                        e.CellElement.ResetValue(LightVisualElement.DrawImageProperty, ValueResetFlags.Local);
                        e.CellElement.ResetValue(LightVisualElement.ImageProperty, ValueResetFlags.Local);
                    }

                    /* }
                     else
                     {
                         if (radGridView1.Rows[e.RowIndex].Cells["Without Reserve"].Value.ToString() == "Huerfana" && radGridView1.Rows[e.RowIndex].Cells["Task Type"].Value.ToString() != "PI")
                         {
                             e.CellElement.DrawImage = true;
                             e.CellElement.Image = Properties.Resources.Overdue;
                             e.CellElement.ImageAlignment = ContentAlignment.TopLeft;
                             e.CellElement.ToolTipText = "This task hasn't reserve";
                         }
                         else
                         {
                             e.CellElement.ResetValue(LightVisualElement.DrawImageProperty, ValueResetFlags.Local);
                             e.CellElement.ResetValue(LightVisualElement.ImageProperty, ValueResetFlags.Local);
                         }
                     }*/
                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.DrawImageProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.ImageProperty, ValueResetFlags.Local);
                }
                if (e.CellElement.ViewTemplate.Parent != null)
                {
                    e.CellElement.ResetValue(LightVisualElement.DrawImageProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.ImageProperty, ValueResetFlags.Local);
                }
                GridGroupExpanderCellElement cell = e.CellElement as GridGroupExpanderCellElement;
                if (cell != null && e.CellElement.RowElement is GridDataRowElement)
                {
                    if (!IsExpandable(cell.RowInfo))
                    {
                        cell.Expander.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                    }
                    else
                    {
                        cell.Expander.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                    }
                }
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
               {*/

                if (e.Column == rgvPrincipal.Columns[colEstadoTarea] && e.RowIndex > -1)
                {
                    object valor = rgvPrincipal.Rows[e.RowIndex].Cells[colPrerequisito].Value;
                    if (valor != DBNull.Value && Convert.ToInt32(valor) != 0)
                    {
                        e.CellElement.DrawImage = true;
                        e.CellElement.Image = Properties.Resources.Warning;
                        e.CellElement.ImageAlignment = ContentAlignment.MiddleRight;
                        e.CellElement.ToolTipText = Lenguaje.traduce("Prerrequisito es mayor que 0");
                    }
                }
                /* else
                 {
                     if (e.Column == radGridView1.Columns["Task State"] && e.RowIndex > -1)
                     {
                         if (Convert.ToInt32(radGridView1.Rows[e.RowIndex].Cells["Prerequisite"].Value) != 0)
                         {
                             e.CellElement.DrawImage = true;
                             e.CellElement.Image = Properties.Resources.Warning;
                             e.CellElement.ImageAlignment = ContentAlignment.MiddleRight;
                             e.CellElement.ToolTipText = "Prerequisite is greater than 0";
                         }
                     }
                 }
             }*/
            }
            catch (Exception)
            {
                //ExceptionManager.GestionarError(ex);
                log.Error("Evento ViewCellFormatting");
            }
        }

        private void radGridView1_ViewCellFormatting3(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                //ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
                GridCellElement cellElement = e.ContextMenuProvider as GridCellElement;
                if (cellElement == null || cellElement.RowInfo is GridViewFilteringRowInfo || cellElement.RowInfo is GridViewTableHeaderRowInfo || cellElement.RowInfo is GridViewSearchRowInfo)
                {
                    return;
                }
                else
                //if (cellElement.ColumnInfo == null || cellElement.ColumnInfo.HeaderText == null)
                {
                    RadDropDownMenu menu = new RadDropDownMenu();
                    RadMenuItem menuItem = new RadMenuItem(Lenguaje.traduce("Asignar nuevo recurso"));
                    menuItem.Click += new EventHandler(menu_ItemClicked);
                    menu.Items.Add(menuItem);
                    e.ContextMenu = menu;
                    return;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void menu_ItemClicked(object sender, EventArgs e)
        {
            try
            {
                ElegirRecurso recursos = new ElegirRecurso();
                if (recursos.ShowDialog() == DialogResult.OK)
                {
                    CambiarTipoLanzamientoAManual();
                    string idRecurso = recursos.idRecurso;
                    LlamarWS(Convert.ToInt32(idRecurso));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void CambiarTipoLanzamientoAManual()
        {
            try
            {
                if (this.rgvPrincipal.CurrentRow.Cells[Lenguaje.traduce("Tipo Lanzamiento")].Value.ToString() == "A")
                {
                    this.rgvPrincipal.CurrentRow.Cells[Lenguaje.traduce("Tipo Lanzamiento")].Value = "M";
                    Business.UpdateCell(" IDTAREA = " + this.rgvPrincipal.CurrentRow.Cells[Lenguaje.traduce("Num Tarea")].Value.ToString(), "TBLTAREASPENDIENTES", " TIPOLANZAMIENTO = 'M' ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RadGridView1_ViewCellFormattingHierarchicalGrid(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
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
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Eventos

        #region ActualizarGrid

        private void llenarGridTareas(object sender, DoWorkEventArgs e)
        {
            try
            {
                Business.GetControlTareasCantidad(ref _lstEsquemaTabla);
                tablaGrid = Business.GetControlTareasDatosGridView(_lstEsquemaTabla);
                tablaLlena = tablaGrid.Copy();
                Business.GetControlTareasJerarquicoCantidad(ref _lstEsquemaTablaJerarquico);
                tablaGridTemplate = Business.GetControlTareasJerarquicoDatosGridView(_lstEsquemaTablaJerarquico);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #region ActualizarFilas

        private void actualizarUbicaciones()
        {
            try
            {
                string query = "SELECT TP.IDTAREA,EN.IDENTRADA,R.IDRESERVA,EN.SSCC ,HO.DESCRIPCION," +
                    "HD.DESCRIPCION,PPC.REFERENCIA,PPC.SERIE,PCC.REFERENCIA,PCC.SERIE," +
                    " P.CODIGO,P.NOMBRE,A.REFERENCIA, A.ATRIBUTO,A.DESCRIPCION,C.CODIGO,C.NOMBRE, RUT.CODIGO AS CODIGORUTA, R.IDORDENRECOGIDA,A.CANTIDAD " +
                    "FROM TBLTAREASPENDIENTES TP INNER JOIN TBLRESERVAS R ON TP.IDTAREA=R.IDTAREA " +
                    "INNER JOIN TBLENTRADAS EN ON EN.IDENTRADA = R.IDENTRADA " +
                    "INNER JOIN TBLARTICULOS A ON A.IDARTICULO = EN.IDARTICULO " +
                    "INNER JOIN TBLHUECOS HO ON HO.IDHUECO = R.IDHUECOORIGEN " +
                    "INNER JOIN TBLHUECOS HD ON HD.IDHUECO = R.IDHUECODESTINO " +
                    "LEFT JOIN TBLPEDIDOSPROCAB PPC ON PPC.IDPEDIDOPRO=R.IDPEDIDOPRO " +
                    "LEFT JOIN TBLPROVEEDORES P ON P.IDPROVEEDOR = PPC.IDPROVEEDOR " +
                    "LEFT JOIN TBLPEDIDOSCLICAB PCC ON PCC.IDPEDIDOCLI=R.IDPEDIDOCLI " +
                    "LEFT JOIN TBLCLIENTES C ON C.IDCLIENTE=PCC.IDCLIENTE " +
                    "LEFT JOIN TBLRUTAS RUT ON RUT.IDRUTA = PCC.IDRUTA ";//ESTO SE HACE CON LA RESERVA
                ubicaciones = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosUbicaciones(object r, int i)
        {
            DataRow[] row = null;
            row = ubicaciones.Select("IDTAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][2];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Destino")].Value = row[0][5];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Origen")].Value = row[0][4];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Articulo")].Value = row[0][12];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Articulo")].Value = row[0][14];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Empresa")].Value = row[0][6];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Nombre Empresa")].Value = row[0][11];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Cantidad")] != null && row[0]["Cantidad"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Cantidad")].Value = row[0]["Cantidad"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden Preparación")] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden Preparación")].Value = row[0]["idordenrecogida"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("SSCC")] != null && row[0]["SSCC"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("SSCC")].Value = row[0]["SSCC"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")] != null && row[0]["IDENTRADA"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")].Value = row[0]["IDENTRADA"];
                /*}
                 else
                 {
                     radGridView1.Rows[i].Cells["DestinyHole"].Value = row[0][5];
                     radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][2];
                     radGridView1.Rows[i].Cells["OriginHole"].Value = row[0][4];
                     radGridView1.Rows[i].Cells["Article Reference"].Value = row[0][12];
                     radGridView1.Rows[i].Cells["Article"].Value = row[0][14];
                     radGridView1.Rows[i].Cells["Business Reference"].Value = row[0][6];
                     radGridView1.Rows[i].Cells["Business Name"].Value = row[0][11];
                 }*/
            }
        }

        private void DatosCompletos(object r, int i)
        {
            DataRow[] row = null;
            row = ubicaciones.Select("IDTAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][2];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Destino")].Value = row[0][5];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Origen")].Value = row[0][4];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Articulo")].Value = row[0][12];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Articulo")].Value = row[0][14];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Empresa")].Value = row[0][8];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Nombre Empresa")].Value = row[0][16];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Código Ruta")].Value = row[0]["CODIGORUTA"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("SSCC")] != null && row[0]["SSCC"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("SSCC")].Value = row[0]["SSCC"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")] != null && row[0]["IDENTRADA"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")].Value = row[0]["IDENTRADA"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden Preparación")] != null && row[0]["idordenrecogida"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden Preparación")].Value = row[0]["idordenrecogida"];
                /*}
                 else
                 {
                     radGridView1.Rows[i].Cells["DestinyHole"].Value = row[0][5];
                     radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][2];
                     radGridView1.Rows[i].Cells["OriginHole"].Value = row[0][4];
                     radGridView1.Rows[i].Cells["Article Reference"].Value = row[0][12];
                     radGridView1.Rows[i].Cells["Article"].Value = row[0][14];
                     radGridView1.Rows[i].Cells["Business Reference"].Value = row[0][6];
                     radGridView1.Rows[i].Cells["Business Name"].Value = row[0][11];
                 }*/
            }
        }

        private void actualizarTrasvase()
        {
            try
            {
                string query = "SELECT tp.idtarea,EN.IDENTRADA,R.IDRESERVA,EN.SSCC,RC.IDRECEPCION,RC.ALBARANTRANSPORTISTA FROM TBLTAREASPENDIENTES TP INNER JOIN TBLRESERVAS R ON TP.IDTAREA=R.IDTAREA INNER JOIN TBLENTRADAS EN ON EN.IDENTRADA = R.IDENTRADA LEFT JOIN TBLRECEPCIONESCAB RC ON EN.IDRECEPCION=EN.IDRECEPCION WHERE TP.TIPOTAREA='TR'";
                trasvase = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosTrasvase(object r, int i)
        {
            DataRow[] row = null;
            row = trasvase.Select("IDTAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][2];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("SSCC")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Albaran Transportista")].Value = row[0][5];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")] != null && row[0]["IDENTRADA"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")].Value = row[0]["IDENTRADA"];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][2];
                    radGridView1.Rows[i].Cells["SSCC"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["Transporter Delivery Note"].Value = row[0][5];
                }*/
            }
        }

        private void actualizarMovCarro()
        {
            try
            {
                string query = "SELECT tp.tarea,MC.IDMOVIMIENTO,CC.IDCARRO,HC.DESCRIPCION AS CARRO,HO.DESCRIPCION AS HUECOORIGEN,HD.DESCRIPCION AS HUECODESTINO,HA.DESCRIPCION AS HUECOACTUAL FROM TBLTAREASPENDIENTES TP INNER JOIN TBLMOVIMIENTOSCARRO MC ON TP.TAREA=MC.IDMOVIMIENTO INNER JOIN TBLCARROMOVILCAB CC ON CC.IDCARRO = MC.IDCARRO INNER JOIN TBLHUECOS HC ON HC.IDHUECO = CC.IDUBICACIONPRINCIPAL INNER JOIN TBLHUECOS HO ON HO.IDHUECO = MC.IDHUECOORIGEN INNER JOIN TBLHUECOS HD ON HD.IDHUECO = MC.IDHUECODESTINO INNER JOIN TBLHUECOS HA ON HA.IDHUECO = CC.IDUBILOCALIZACION WHERE TP.TIPOTAREA='UC'";
                movCarro = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosMovCarro(object r, int i)
        {
            DataRow[] row = null;
            row = movCarro.Select("TAREA=" + r.ToString());
            if (row.Length == 0)
            {
                rgvPrincipal.Rows[i].Cells[colSinReserva].Value = strHuerfana;
            }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Carro")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Origen")].Value = row[0][4];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Destino")].Value = row[0][5];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Actual")].Value = row[0][6];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Kart"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["OriginHole"].Value = row[0][4];
                    radGridView1.Rows[i].Cells["DestinyHole"].Value = row[0][5];
                    radGridView1.Rows[i].Cells["Actual Hole"].Value = row[0][6];
                }*/
            }
        }

        private void actualizarMovBulto()
        {
            try
            {
                string query = "SELECT TP.IDTAREA,MC.IDMOVIMIENTO,HO.DESCRIPCION AS HUECOORIGEN,HD.DESCRIPCION AS HUECODESTINO,PL.IDPACKINGLIST,PL.IDPACKINGLISTPADRE,CC.IDCARGA FROM TBLTAREASPENDIENTES TP INNER JOIN TBLMOVIMIENTOSBULTO MC ON TP.idtarea=MC.idtarea INNER JOIN TBLPACKINGLIST PL ON MC.IDENTIFICADOR=PL.IDENTIFICADOR INNER JOIN TBLHUECOS HO ON HO.IDHUECO = MC.IDHUECOORIGEN INNER JOIN TBLHUECOS HD ON HD.IDHUECO = MC.IDHUECODESTINO LEFT JOIN TBLCARGACAB CC ON MC.IDCARGA=CC.IDCARGA WHERE TP.TIPOTAREA ='PL'";
                movBulto = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosMovBulto(object r, int i)
        {
            DataRow[] row = null;
            row = movBulto.Select("IDTAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Origen")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Destino")].Value = row[0][4];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("PackingList")].Value = row[0][5];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("PackingList Padre")].Value = row[0][6];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["OriginHole"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["DestinyHole"].Value = row[0][4];
                    radGridView1.Rows[i].Cells["PackingList"].Value = row[0][5];
                    radGridView1.Rows[i].Cells["Father PackingList"].Value = row[0][6];
                }*/
            }
        }

        private void actualizarReposiciones()
        {
            try
            {
                string query = "SELECT tp.tarea,RES.IDRESERVA,CC.IDCARRO,HC.DESCRIPCION AS CARRO,HA.DESCRIPCION AS HUECOACTUAL FROM TBLTAREASPENDIENTES TP left join TBLRESERVAS res on tp.IDTAREA=res.IDTAREA INNER JOIN TBLCARROMOVILCAB CC ON CC.IDCARRO = TP.TAREA INNER JOIN TBLHUECOS HC ON HC.IDHUECO = CC.IDUBICACIONPRINCIPAL INNER JOIN TBLHUECOS HA ON HA.IDHUECO = CC.IDUBILOCALIZACION WHERE TP.TIPOTAREA IN ('DC','DP')";
                reposiciones = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosReposiciones(object r, int i)
        {
            DataRow[] row = null;
            row = reposiciones.Select("TAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][1];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Carro")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Actual")].Value = row[0][4];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][1];
                    radGridView1.Rows[i].Cells["Kart"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["Actual Hole"].Value = row[0][4];
                }*/
            }
        }

        private void actualizarDevolucionCli()
        {
            try
            {
                string query = "SELECT TP.TAREA,RES.IDRESERVA,DC.REFERENCIA,DC.ALBARANTRANS,A.CODIGO AS CODIGOAGENCIA,A.NOMBRE AS NOMBREAGENCIA ,C.CODIGO,C.NOMBRE FROM TBLTAREASPENDIENTES tp left join TBLRESERVAS res on tp.IDTAREA=res.IDTAREA INNER JOIN TBLDEVOLCLICAB DC ON DC.IDDEVOLCLI=TP.TAREA INNER JOIN TBLCLIENTES C ON C.IDCLIENTE = DC.IDCLIENTE INNER JOIN TBLAGENCIAS A ON A.IDAGENCIA = DC.IDAGENCIA WHERE TP.TIPOTAREA='DE'";
                devolucionCli = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosDevolucionCli(object r, int i)
        {
            DataRow[] row = null;
            row = devolucionCli.Select("TAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][1];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Empresa")].Value = row[0][2];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Albaran Devolucion ClienteCab")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Nombre Empresa")].Value = row[0][7];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][1];
                    radGridView1.Rows[i].Cells["Business Reference"].Value = row[0][2];
                    radGridView1.Rows[i].Cells["Client Devolution Delivery Note"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["Business Name"].Value = row[0][7];
                }*/
            }
        }

        private void actualizarConteo()
        {
            try
            {
                string query = "SELECT DISTINCT TP.TAREA,RC.IDRECEPCION,P.CODIGO,P.NOMBRE,PC.REFERENCIA,PC.SERIE FROM TBLTAREASPENDIENTES TP INNER JOIN TBLRECEPCIONESCAB RC ON TP.TAREA = RC.IDRECEPCION INNER JOIN  TBLRECEPCIONESLIN RL ON RC.IDRECEPCION = (SELECT  TOP 1 IDRECEPCION FROM TBLRECEPCIONESLIN WHERE IDRECEPCION=RC.IDRECEPCION) INNER JOIN TBLPEDIDOSPROLIN PL ON PL.IDPEDIDOPRO = RL.IDPEDIDOPRO AND PL.IDPEDIDOPROLIN = RL.IDPEDIDOPROLIN INNER JOIN TBLPEDIDOSPROCAB PC ON PC.IDPEDIDOPRO = PL.IDPEDIDOPRO INNER JOIN TBLPROVEEDORES P ON P.IDPROVEEDOR = PC.IDPROVEEDOR WHERE TP.TIPOTAREA='CO' UNION SELECT DISTINCT tp.tarea,RC.IDRECEPCION,P.CODIGO,P.NOMBRE,PC.REFERENCIA,PC.SERIE FROM TBLTAREASPENDIENTES TP INNER JOIN TBLRECEPCIONESCAB RC ON TP.TAREA = RC.IDRECEPCION LEFT JOIN TBLPEDIDOSPROCAB PC ON RC.IDPEDIDOPRO = PC.IDPEDIDOPRO LEFT JOIN TBLPROVEEDORES P ON P.IDPROVEEDOR = PC.IDPROVEEDOR WHERE TP.TIPOTAREA='CO'";
                conteo = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosConteo(object r, int i)
        {
            DataRow[] row = null;
            row = conteo.Select("TAREA='" + r.ToString() + "'");
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Nombre Empresa")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Empresa")].Value = row[0][4];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Business Name"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["Business Reference"].Value = row[0][4];
                }*/
            }
        }

        private void actualizarOrdenRecogida()
        {
            try
            {
                string query = "SELECT tp.tarea,RES.IDRESERVA,OC.IDORDENRECOGIDA,PC.REFERENCIA,PC.SERIE,C.CODIGO,C.NOMBRE FROM TBLTAREASPENDIENTES tp left join TBLRESERVAS res on tp.IDTAREA=res.IDTAREA INNER JOIN TBLORDENRECOGIDACAB OC ON tp.TAREA=OC.IDORDENRECOGIDA INNER JOIN TBLORDENRECOGIDALIN OL ON OL.IDORDENRECOGIDA = (SELECT TOP(1)IDORDENRECOGIDA FROM TBLORDENRECOGIDALIN OL WHERE OL.IDORDENRECOGIDA= OC.IDORDENRECOGIDA) INNER JOIN TBLPEDIDOSCLICAB PC ON PC.IDPEDIDOCLI = OL.IDPEDIDOCLI INNER JOIN TBLCLIENTES C ON C.IDCLIENTE = PC.IDCLIENTE WHERE TP.TIPOTAREA='CA'";
                ordenRecogida = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosOrdenRecogida(string r, int i)
        {
            //DataRow[] row = new DataRow[0];
            log.Debug("TAREA = " + r.ToString());
            //ordenRecogida.Rows.Count();
            DataRow[] row = ordenRecogida.Select("tarea = " + "'" + r + "'");
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][1];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Empresa")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Nombre Empresa")].Value = row[0][6];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][1];
                    radGridView1.Rows[i].Cells["Business"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["Business Name"].Value = row[0][6];
                }*/
            }
        }

        private void actualizarOrdenCarga()
        {
            try
            {
                string query = "SELECT DISTINCT tp.tarea,RES.IDRESERVA,CC.IDCARGA,CC.CHOFER,CC.MATRICULA,A.CODIGO AS CODIGOAGENCIA,A.NOMBRE AS NOMBREAGENCIA,S.IDORDENRECOGIDA,PC.REFERENCIA,PC.SERIE,C.CODIGO AS CODIGOCLIENTE,C.NOMBRE AS NOMBRECLIENTE FROM TBLTAREASPENDIENTES TP left join TBLRESERVAS res on tp.IDTAREA=res.IDTAREA INNER JOIN TBLCARGACAB CC ON TP.TAREA=CC.IDCARGA INNER JOIN TBLAGENCIAS A ON A.IDAGENCIA = CC.IDAGENCIA LEFT JOIN TBLCARGALIN CL ON CC.IDCARGA = CL.IDCARGA LEFT JOIN TBLPACKINGLIST PL ON CL.IDENTIFICADOR=PL.IDENTIFICADOR LEFT JOIN TBLSALIDAS S ON PL.IDENTIFICADOR=(SELECT TOP(1)S.IDENTIFICADORPL FROM TBLSALIDAS WHERE S.IDENTIFICADORPL= PL.IDENTIFICADOR) LEFT JOIN TBLPEDIDOSCLICAB PC ON S.IDPEDIDOCLI=PC.IDPEDIDOCLI INNER JOIN TBLCLIENTES C ON C.IDCLIENTE = PC.IDCLIENTE WHERE TP.TIPOTAREA='OC'";
                ordenCarga = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosOrdenCarga(object r, int i)
        {
            DataRow[] row = null;
            row = ordenCarga.Select("TAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][1];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Chofer")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")].Value = row[0][4];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Empresa")].Value = row[0][8];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Nombre Empresa")].Value = row[0][10];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][1];
                    radGridView1.Rows[i].Cells["Driver"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["Enrollment"].Value = row[0][4];
                    radGridView1.Rows[i].Cells["Business Reference"].Value = row[0][8];
                    radGridView1.Rows[i].Cells["Business Name"].Value = row[0][10];
                }*/
            }
        }

        private void actualizarPicking()
        {
            try
            {
                string query =
                    "SELECT Distinct tp.TAREA,PED.REFERENCIA, C.NOMBRE, R.CODIGO, " +
                    "R.DESCRIPCION,PED.OBSERVACIONES,RES.idordenrecogida FROM DBO.TBLTAREASPENDIENTES TP " +
                    "LEFT OUTER JOIN DBO.TBLORDENRECOGIDALIN AS LN ON LN.IDORDENRECOGIDA =TP.TAREA LEFT OUTER JOIN DBO.TBLPEDIDOSCLICAB AS PED ON PED.IDPEDIDOCLI =LN.IDPEDIDOCLI  " +
                    "LEFT OUTER JOIN DBO.TBLRUTAS AS R ON PED.IDRUTA = R.IDRUTA " +
                    " LEFT OUTER JOIN DBO.TBLCLIENTES AS C ON PED.IDCLIENTE = C.IDCLIENTE " +
                    " LEFT OUTER JOIN DBO.TBLRESERVAS AS RES ON TP.IDTAREA = RES.IDTAREA " +
                    " WHERE TP.TIPOTAREA IN ('PI','OL')";
                picking = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosPicking(object r, int i)
        {
            DataRow[] row = null;
            row = picking.Select("TAREA='" + r.ToString() + "'");
            if (row.Length == 0) { }
            else
            {
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Empresa")].Value = row[0][1];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Nombre Empresa")].Value = row[0][2];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Observaciones Pedido")].Value = row[0][5];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Código Ruta")] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Código Ruta")].Value = row[0]["CODIGO"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden Preparación")] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden Preparación")].Value = row[0]["idordenrecogida"];
            }
        }

        private void actualizarAcopios()
        {
            try
            {
                string query = "select TP.IDTAREA,R.IDRESERVA,tp.TAREA,o.IDPEDIDOFAB,o.ORDEN," +
                    "HO.DESCRIPCION,HD.DESCRIPCION,A.REFERENCIA,A.DESCRIPCION, EN.IDENTRADA, EN.SSCC  " +
                    "FROM TBLTAREASPENDIENTES TP " +
                    "INNER JOIN TBLRESERVAS R ON TP.IDTAREA=R.IDTAREA " +
                    "INNER JOIN TBLENTRADAS EN ON EN.IDENTRADA = R.IDENTRADA " +
                    "INNER JOIN TBLARTICULOS A ON A.IDARTICULO = EN.IDARTICULO " +
                    "INNER JOIN TBLHUECOS HO on R.IDHUECOORIGEN=HO.IDHUECO " +
                    "INNER JOIN TBLHUECOS HD on R.IDHUECODESTINO=HD.IDHUECO " +
                    "inner join TBLORDENFABRICACIONCAB o on o.idpedidofab=r.idpedidofab WHERE TP.TIPOTAREA ='AC'";
                acopios = ConexionSQL.getDataTable(query);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void DatosAcopios(object r, int i)
        {
            DataRow[] row = null;
            row = acopios.Select("IDTAREA=" + r.ToString());
            if (row.Length == 0) { }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Num Reserva")].Value = row[0][1];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden Fab")].Value = row[0][3];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Orden")].Value = row[0][4];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Destino")].Value = row[0][6];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Hueco Origen")].Value = row[0][5];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Referencia Articulo")].Value = row[0][7];
                rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Articulo")].Value = row[0][8];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("SSCC")] != null && row[0]["SSCC"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("SSCC")].Value = row[0]["SSCC"];
                if (rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")] != null && row[0]["IDENTRADA"] != null)
                    rgvPrincipal.Rows[i].Cells[Lenguaje.traduce("Matricula")].Value = row[0]["IDENTRADA"];
                /*}
                else
                {
                    radGridView1.Rows[i].Cells["Num Reserve"].Value = row[0][1];
                    radGridView1.Rows[i].Cells["Order Fab"].Value = row[0][3];
                    radGridView1.Rows[i].Cells["Order"].Value = row[0][4];
                }*/
            }
        }

        private void LlamadaActualizar()
        {
            actualizarAcopios();
            actualizarConteo();
            actualizarDevolucionCli();
            actualizarMovBulto();
            actualizarMovCarro();
            actualizarOrdenCarga();
            actualizarOrdenRecogida();
            actualizarPicking();
            actualizarReposiciones();
            actualizarTrasvase();
            actualizarUbicaciones();
        }

        #endregion ActualizarFilas

        private void bgWorkerGridViewTareas_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable tablaVacia = new DataTable();
                tablaVacia = tablaGrid.Copy();
                DataRow row = tablaVacia.Rows[0];
                tablaVacia.Rows.Clear();
                tablaVacia.Rows.Add(row);
                log.Debug("Antes de tabla vacia");
                rgvPrincipal.DataSource = tablaVacia;
                //radGridView1.Refresh();
                log.Debug("Antes de tabla llena");
                rgvPrincipal.DataSource = tablaLlena;
                //radGridView1.Refresh();
                log.Debug("Antes de begin update");
                rgvPrincipal.BeginUpdate();
                log.Debug("p1:" + DateTime.Now.Millisecond);
                log.Debug("Antes de actualizar");
                LlamadaActualizar();
                for (int r = 0; r < rgvPrincipal.Rows.Count; r++)
                {
                    log.Debug("fila: " + r + " " + rgvPrincipal.Rows[r].Cells[colTipoTarea].Value + " " + rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value);

                    switch (rgvPrincipal.Rows[r].Cells[colTipoTarea].Value)
                    {
                        case "PI":
                            DatosPicking(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value, r);
                            break;

                        case "OL":
                            DatosPicking(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value, r);
                            if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }
                            break;

                        case "CO":
                            DatosConteo(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value, r);
                            break;

                        case "CA":
                            DatosOrdenRecogida(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value.ToString(), r);
                            //No tiene reservas
                            /*if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }*/
                            break;

                        case "OC":
                            DatosOrdenCarga(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value, r);
                            //No tiene reservas
                            /*if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value =strHuerfana;
                            }*/
                            break;

                        case "DE":
                            DatosDevolucionCli(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value, r);
                            //No tiene reservas
                            /*if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }*/
                            break;

                        case "SP":
                        case "SD":
                            DatosCompletos(rgvPrincipal.Rows[r].Cells[colNumTarea].Value, r);
                            if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }
                            break;

                        case "UP":
                        case "RP":
                        case "RJ":
                            DatosUbicaciones(rgvPrincipal.Rows[r].Cells[colNumTarea].Value, r);
                            if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }

                            break;

                        case "TR":
                            DatosTrasvase(rgvPrincipal.Rows[r].Cells[colNumTarea].Value, r);
                            if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }
                            break;

                        case "UC":
                            DatosMovCarro(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value, r);
                            break;

                        case "PL":
                            DatosMovBulto(rgvPrincipal.Rows[r].Cells[colNumTarea].Value, r);
                            break;

                        case "DP":
                        case "DC":
                            DatosReposiciones(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Tarea")].Value, r);
                            if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }
                            break;

                        case "AC":
                            DatosAcopios(rgvPrincipal.Rows[r].Cells[colNumTarea].Value, r);
                            if (Convert.ToInt32(rgvPrincipal.Rows[r].Cells[Lenguaje.traduce("Num Reserva")].Value) == 0)
                            {
                                rgvPrincipal.Rows[r].Cells[colSinReserva].Value = strHuerfana;
                            }
                            break;

                        default:
                            break;
                    }
                }
                log.Debug("Fuera for: " + DateTime.Now.Millisecond);
                rgvPrincipal.EndUpdate();
                rgvPrincipal.Columns.Remove(rgvPrincipal.Columns["RowNum"]);
                //log.Debug("Antes jerarquico PI: " + DateTime.Now.Millisecond);
                JerarquicoPi();

                //log.Debug("Despues jerarquico PI: " + DateTime.Now.Millisecond);
                rgvPrincipal.Templates[0].Columns.Remove(rgvPrincipal.Templates[0].Columns["RowNum"]);
                log.Debug("Antes columna prioridad: " + DateTime.Now.Millisecond);
                ColumnaPrioridad();
                log.Debug("despues columna prioridad: " + DateTime.Now.Millisecond);
                SetPreferences();
                log.Debug("Antes color tipo tarea: " + DateTime.Now.Millisecond);
                ColorTipoTareas();
                log.Debug("Antes color estado tareas: " + DateTime.Now.Millisecond);
                ColorEstadoTareas();
                log.Debug("Tiempo final: " + DateTime.Now);
                rgvPrincipal.Refresh();
                ElegirEstilo();
                this.rgvPrincipal.EnablePaging = false;
                radWait.StopWaiting();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                radWait.StopWaiting();
            }

            AñadirLabelCantidad();
        }

        private void ColumnaPrioridad()
        {
            try
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                rgvPrincipal.Columns[colPrioridad].FieldName = "Prioridad";
                /*}
                else
                {
                    radGridView1.Columns["Priority"].FieldName = "Prioridad";
                }*/
                GridViewDecimalColumn decimalColumn = new GridViewDecimalColumn(colPrioridad);
                decimalColumn.Minimum = 0;
                decimalColumn.Maximum = 10;
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                decimalColumn.HeaderText = colPrioridad;
                /*}
                else
                {
                    decimalColumn.HeaderText = "Priority";
                }*/
                rgvPrincipal.Columns.Remove(rgvPrincipal.Columns[colPrioridad]);
                rgvPrincipal.MasterTemplate.Columns.Add(decimalColumn);
                rgvPrincipal.Columns.Move(rgvPrincipal.Columns[colPrioridad].Index, 3);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion ActualizarGrid

        #region JerarquicoPicking

        private void JerarquicoPi()
        {
            try
            {
                string query = string.Empty;
                rgvPrincipal.Relations.Clear();
                GridViewTemplate childTemplate1 = new GridViewTemplate();
                childTemplate1.DataSource = tablaGridTemplate;
                if (this.rgvPrincipal.MasterTemplate.Templates.Count > 0)
                {
                    this.rgvPrincipal.MasterTemplate.Templates.RemoveAt(0);
                }
                this.rgvPrincipal.MasterTemplate.Templates.Add(childTemplate1);
                GridViewRelation relation1 = new GridViewRelation(this.rgvPrincipal.MasterTemplate);
                relation1.RelationName = "TR";
                relation1.ParentColumnNames.Add(colNumTarea);
                relation1.ChildColumnNames.Add(colNumTarea);
                relation1.ChildTemplate = childTemplate1;
                this.rgvPrincipal.Relations.Add(relation1);
                this.rgvPrincipal.Templates[0].AllowAddNewRow = false;
                rgvPrincipal.Templates[0].Columns[colNumTarea].IsVisible = false;
                rgvPrincipal.Templates[0].Columns[colNumTarea].VisibleInColumnChooser = true;
                rgvPrincipal.Templates[0].Columns[colNumTarea].AllowHide = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void refrescarJerarquicoPI()
        {
            try
            {
                rgvPrincipal.Templates[0].Columns[colNumTarea].IsVisible = false;
                rgvPrincipal.Templates[0].Columns[colNumTarea].VisibleInColumnChooser = true;
                rgvPrincipal.Templates[0].Columns[colNumTarea].AllowHide = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion JerarquicoPicking

        #region Agrupaciones

        private void setOperario()
        {
            try
            {
                this.rgvPrincipal.GroupDescriptors.Clear();
                GroupDescriptor descriptor = new GroupDescriptor();
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                descriptor.GroupNames.Add(Lenguaje.traduce("Nombre Operario"), ListSortDirection.Ascending);
                /*}
                else
                {
                    descriptor.GroupNames.Add("Operator Name", ListSortDirection.Ascending);
                }*/
                this.rgvPrincipal.GroupDescriptors.Add(descriptor);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void setRecursos()
        {
            try
            {
                GroupDescriptor descriptor = new GroupDescriptor();
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                descriptor.GroupNames.Add(Lenguaje.traduce("Recurso"), ListSortDirection.Ascending);
                /*}
                else
                {
                    descriptor.GroupNames.Add("Resource", ListSortDirection.Ascending);
                }*/
                this.rgvPrincipal.GroupDescriptors.Add(descriptor);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void setPedidos()
        {
            try
            {
                GroupDescriptor descriptor = new GroupDescriptor();
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                descriptor.GroupNames.Add(Lenguaje.traduce("Referencia Empresa"), ListSortDirection.Ascending);
                /*}
                else
                {
                    descriptor.GroupNames.Add("Business Reference", ListSortDirection.Ascending);
                }*/
                this.rgvPrincipal.GroupDescriptors.Add(descriptor);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radGridView1_GroupSummaryEvaluate1(object sender, Telerik.WinControls.UI.GroupSummaryEvaluationEventArgs e)
        {
            int count = e.Group.ItemCount;
            e.FormatString = String.Format("{1}: {0}", count, e.Value);
        }

        #endregion Agrupaciones

        #region Botones

        private string confirmacion = Lenguaje.traduce(strings.ExportacionExito);

        public void btnExportacion_Click(object sender, EventArgs e)
        {
            FuncionesGenerales.exportarAExcelGenerico(this.rgvPrincipal);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón refresh " + DateTime.Now);
            refrescar();
        }

        private void refrescar()
        {
            try
            {
                rgvPrincipal.EnablePaging = false;
                rgvPrincipal.Columns.Clear();
                //bgWorkerGridViewTareas.DoWork += new DoWorkEventHandler(llenarGridTareas);
                radWait.StartWaiting();
                bgWorkerGridViewTareas.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void PedidosButton_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón pedidos " + DateTime.Now);
                this.rgvPrincipal.GroupDescriptors.Clear();
                rgvPrincipal.MultiSelect = true;
                setPedidos();
                rgvPrincipal.BestFitColumns();
                //ColorTipoTareas();
                ColorEstadoTareas();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void ItemColumnas_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón editar columnas " + DateTime.Now);
            this.rgvPrincipal.ShowColumnChooser();
        }

        public void PrincipalButton_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón filtro " + DateTime.Now);
                if (rgvPrincipal.IsInEditMode)
                {
                    rgvPrincipal.EndEdit();
                }
                rgvPrincipal.FilterDescriptors.Clear();
                rgvPrincipal.GroupDescriptors.Clear();
                //ColorTipoTareas();
                ColorEstadoTareas();
                rgvPrincipal.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void OperarioButton_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón operario " + DateTime.Now);
                this.rgvPrincipal.GroupDescriptors.Clear();
                //setOperario();
                setRecursos();
                rgvPrincipal.MultiSelect = true;
                rgvPrincipal.BestFitColumns();
                //ColorTipoTareas();
                ColorEstadoTareas();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public virtual void LoadItem_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón cargar estilo " + DateTime.Now);
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
            log.Info("Pulsado botón guardar estilo " + DateTime.Now);
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

        #endregion Botones

        #region Estilos

        public void SaveLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;/*XmlReaderPropio.getLayoutPath(0);*/
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
                Directory.CreateDirectory(path);
                path += "\\" + nombreEstilo;
                path.Replace(" ", "_");
                rgvPrincipal.SaveLayout(path);
                MessageBox.Show("Se ha guardado el estilo correctamente");
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce("No se ha podido encontrar el archivo en la dirección") + ":" + path + "\n" + Lenguaje.traduce("Puede cambiar esta dirección en el archivo PathLayouts.xml"));
            }
        }

        public void SaveLayoutLocal()
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
                Directory.CreateDirectory(path);
                path += "\\" + nombreEstilo;
                path.Replace(" ", "_");
                rgvPrincipal.SaveLayout(path);
                MessageBox.Show("Se ha guardado el estilo correctamente");
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
                string s = path + "\\" + nombreEstilo;
                s.Replace(" ", "_");
                rgvPrincipal.LoadLayout(s);
                this.rgvPrincipal.TableElement.RowHeight = 40;
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
                string s = path + "\\" + nombreEstilo;

                s.Replace(" ", "_");

                rgvPrincipal.LoadLayout(s);
                this.rgvPrincipal.TableElement.RowHeight = 40;
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(strings.NoEncuentroRuta + ":" + path + "\n" + strings.CambiarPath);
            }
        }

        public void ElegirEstilo()
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
            if (tlPanelPrincipal.GetControlFromPosition(0, 0) is RadGridView)
            {
                string pathGridView = pathLocal + "\\" + this.Name + "VistaControlTareas.xml";
                bool existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    //gridViewControl.LoadLayout(pathGridView);
                    LoadLayoutLocal();
                }
                else
                {
                    pathGridView = pathGlobal + "\\" + this.Name + "VistaControlTareas.xml";
                    existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        //gridViewControl.LoadLayout(pathGridView);
                        LoadLayoutGlobal();
                    }
                    else
                    {
                        rgvPrincipal.BestFitColumns();
                    }
                }
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
            RadMenuItem mainItem = configButton.Items[0] as RadMenuItem;
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(temasItem_Click);
            mainItem.Items.Add(temasItem);
        }

        private void temasItem_Click(object sender, EventArgs e)
        {
            try
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
                        rgvPrincipal.BestFitColumns();
                    }
                }
                if (!loadedThemes.ContainsKey(themeName))
                {
                    Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
                    Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + themeName + "Theme"));
                    loadedThemes.Add(themeName, true);
                    rgvPrincipal.BestFitColumns();
                }
                ThemeResolutionService.ApplicationThemeName = themeName;
                if (ControlTraceMonitor.AnalyticsMonitor != null)
                {
                    ControlTraceMonitor.AnalyticsMonitor.TrackAtomicFeature("ThemeChanged." + themeName);
                    rgvPrincipal.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Temas

        #region ColorTipoTareas

        private void ColorTipoTareas()
        {
            try
            {
                foreach (DataRow drTareatipo in dtColorTareaTipo.Rows)
                {
                    log.Debug("tipo tarea: " + drTareatipo["IDTAREATIPO"].ToString());
                    ExpressionFormattingObject obj1 = new ExpressionFormattingObject("MiCondicion" + drTareatipo["idtareatipo"], "[" + colIDTipoTarea + "] = " + drTareatipo["IDTAREATIPO"].ToString(), false);
                    log.Debug("CellBackColor: ");
                    obj1.CellBackColor = CalculaColor(Convert.ToInt32(drTareatipo["COLORAMOSTRAR"]));
                    log.Debug("ConditionalFormattingObjectList: ");
                    this.rgvPrincipal.Columns[colDescripcionTarea].ConditionalFormattingObjectList.Add(obj1);
                }
            }
            catch (Exception ex)
            {
                //ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion ColorTipoTareas

        #region ColorEstadoTareas

        private void ColorEstadoTareas()
        {
            try
            {
                ConditionalFormattingObject obj1 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "PE", "", false);
                obj1.CellBackColor = PEColor;
                ConditionalFormattingObject obj2 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "AS", "", false);
                obj2.CellBackColor = ASColor;
                ConditionalFormattingObject obj3 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "RE", "", false);
                obj3.CellBackColor = REColor;
                ConditionalFormattingObject obj4 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "EJ", "", false);
                obj4.CellBackColor = EJColor;
                ConditionalFormattingObject obj5 = new ConditionalFormattingObject("Mi Condición5", ConditionTypes.Equal, "BP", "", false);
                obj5.CellBackColor = BPColor;
                ConditionalFormattingObject obj6 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "PA", "", false);
                obj6.CellBackColor = PAColor;
                ConditionalFormattingObject obj7 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "PC", "", false);
                obj7.CellBackColor = PCColor;
                ConditionalFormattingObject obj8 = new ConditionalFormattingObject("Mi Condición5", ConditionTypes.Equal, "BQ", "", false);
                obj8.CellBackColor = BQColor;

                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj1);
                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj2);
                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj3);
                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj4);
                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj5);
                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj6);
                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj7);
                this.rgvPrincipal.MasterTemplate.Columns[colEstadoTarea].ConditionalFormattingObjectList.Add(obj8);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion ColorEstadoTareas

        #region WS

        private string formarJSONAsignRecurso(int idTarea, int idRecurso)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idTarea = idTarea;
            objDinamico.idRecurso = idRecurso;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        private void LlamarWS(int idRecurso)
        {
            try
            {
                int idTarea;
                string json = string.Empty;
                JavaScriptSerializer ser = new JavaScriptSerializer();
                foreach (GridViewRowInfo row in rgvPrincipal.SelectedRows)
                {
                    /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                    {*/
                    idTarea = Convert.ToInt32(row.Cells[colNumTarea].Value);
                    json += formarJSONAsignRecurso(idTarea, idRecurso) + ",";
                    /*}
                    else
                    {
                        idTarea = Convert.ToInt32(row.Cells["Task Number"].Value);
                        json += formarJSONAsignRecurso(idTarea, idRecurso) + ",";
                    }*/
                }
                json = json.Substring(0, json.Length - 1);
                json = "[" + json + "]";
                var respuesta2 = tareasMotorClient.asignarRecursoTareaSePuedeRealizar(json);
                var listaRecursos = ser.Deserialize<Recursos[]>(respuesta2);
                if (listaRecursos[0].sePuedeRealizar.ToString() == "OK")
                {
                    // MessageBox.Show(Lenguaje.traduce("Se completó el cambio de recurso."));
                    log.Debug("Llamando a web service WSTareasPendientesMotor.asignarRecursoTarea con parametro \n" + json);
                    var respuestaFinal = tareasMotorClient.asignarRecursoTarea(json);
                    log.Debug("Llamada a web service WSTareasPendientesMotor.asignarRecursoTarea completada con respuesta " + respuestaFinal);
                }
                else
                {
                    /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                    {*/
                    var mensaje = MessageBox.Show(Lenguaje.traduce(listaRecursos[0].sePuedeRealizar.ToString())
                        + Lenguaje.traduce("¿Quiere forzar el recurso igualmente?"), "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (mensaje == DialogResult.Yes)
                    {
                        log.Debug("Llamando a web service WSTareasPendientesMotor.asignarRecursoTarea con parametro \n" + json);
                        var respuestaFinal = tareasMotorClient.asignarRecursoTarea(json);
                        log.Debug("Llamada a web service WSTareasPendientesMotor.asignarRecursoTarea completada con respuesta " + respuestaFinal);
                    }
                    /*}
                    else
                    {
                        var mensaje = MessageBox.Show(Lenguaje.traduce(listaRecursos[0].sePuedeRealizar.ToString()) + Lenguaje.traduce("Do you want force the resource?"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (mensaje == DialogResult.Yes)
                        {
                            var respuestaFinal = tareasMotorClient.asignarRecursoTarea(json);
                        }
                    }*/
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //bgWorkerGridViewTareas.DoWork += new DoWorkEventHandler(llenarGridTareas);
                radWait.StartWaiting();
                bgWorkerGridViewTareas.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, tareasMotorClient.Endpoint);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion WS

        #region Preferencias

        private void EstablecerPreferenciasFiltros()
        {
            rgvPrincipal.EnableFiltering = true;
            rgvPrincipal.MultiSelect = true;
            this.rgvPrincipal.MasterTemplate.EnableGrouping = true;
            this.rgvPrincipal.ShowGroupPanel = true;
            this.rgvPrincipal.MasterTemplate.AutoExpandGroups = true;
            this.rgvPrincipal.EnableHotTracking = true;
            this.rgvPrincipal.MasterTemplate.AllowAddNewRow = false;
            this.rgvPrincipal.MasterTemplate.AllowColumnResize = true;
            this.rgvPrincipal.MasterTemplate.AllowMultiColumnSorting = true;
            this.rgvPrincipal.AllowRowResize = false;
            this.rgvPrincipal.AllowSearchRow = true;
            this.rgvPrincipal.MasterView.TableSearchRow.SearchDelay = 2000;
        }

        private void SetPreferences()
        {
            try
            {
                gridViewControl.PageSize = 30;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvPrincipal.TableElement.RowHeight = 40;

                rgvPrincipal.BestFitColumns();
                rgvPrincipal.Templates[0].BestFitColumns();
                rgvPrincipal.Templates[0].Columns[Lenguaje.traduce("Cantidad")].FormatString = "{0:N2}";
                foreach (var grid in rgvPrincipal.Columns)
                {
                    grid.ReadOnly = false;
                    /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                    {*/
                    if (grid.FieldName != colPrioridad)
                    {
                        grid.ReadOnly = true;
                    }
                    /*}
                    else
                    {
                        if (grid.FieldName != "Priority")
                        {
                            grid.ReadOnly = true;
                        }
                    }*/
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Preferencias

        private void InicializarComboPaginacion()
        {
            if (XmlReaderPropio.getPaginacion() <= 10)
            {
                for (int i = 20; i < 501; i++)
                {
                    menuComboItem.Items.Add(i.ToString());
                }
            }
            else
            {
                for (int i = XmlReaderPropio.getPaginacion(); i < 501; i++)
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
                int tamaño = int.Parse(menuComboItem.ComboBoxElement.SelectedItem.Text);
                rgvPrincipal.PageSize = tamaño;
                XmlReaderPropio.setPaginacion(tamaño);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AñadirLabelCantidad()
        {
            lblCantidad.Text = Lenguaje.traduce(strings.NTareas) + ":" + this.rgvPrincipal.Rows.Count.ToString();
            lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            grupoLabel.Items.Add(lblCantidad);
            this.ribbonTab1.Items.AddRange(grupoLabel);
        }

        private void rBtnEliminarHuerfanas_Click(object sender, EventArgs e)
        {
            EliminarTareasHuerfanas();
        }

        private void EliminarTareasHuerfanas()
        {
            try
            {
                int num = reservasMotorClient.eliminarTareasPendientesHuerfanas(User.IdOperario);
                MessageBox.Show(Lenguaje.traduce("Se han eliminado " + num + " tareas huérfanas"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refrescar();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, reservasMotorClient.Endpoint);
            }
        }

        private void rBtnEliminarReposiciones_Click(object sender, EventArgs e)
        {
            EliminarReposiciones();
        }

        private void EliminarReposiciones()
        {
            try
            {
                FrmSeleccionarReposiciones frm = new FrmSeleccionarReposiciones();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, reservasMotorClient.Endpoint);
            }
        }

        private void rBtnMovimientosEnError_Click(object sender, EventArgs e)
        {
            try
            {
                FrmVerMovimientosError frm = new FrmVerMovimientosError();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, reservasMotorClient.Endpoint);
            }
        }
    }
}

#region ClaseRecursos

public class Recursos
{
    //[JsonProperty]
    public int idTarea { get; set; }

    public int idRecurso { get; set; }
    public string sePuedeRealizar { get; set; }
}

#endregion ClaseRecursos
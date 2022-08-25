//DONE: A Quitar Nº Tareas:
//DONE: A Boton Pantalla proceso desde operario dia hasta operario dia, no mas de una semana
//DONE: A Mejorar la estetica de boton de adelante atrás.
//DONE: A Presentar duracion en formato HH:MM:SS
//DONE: A Idiomas en mensajes /traducir textos
//DONE: A idiomas en los menus contextuales. LA OPCIÓN DE DESCARTAR (LA UNICA, ESTÁ TRADUCIDA
//DONE: A El web service de cálculo debe descartar los que tienen la marca de descartado (modificado con una "D"). --HECHO PENDIENTE PRUEBA
//DONE: B Poner en rojo el boton de grabar si han modificado algun registro a mano o con el boton de ajuste automático.
//DONE: B Ocultar descartados No va fino
//DONE: B Guardar log de cualquier cambio o modificacion
//DONE: B Que modificar un campo dispare poner a M el registro. Se puede modificar a mano pero dejaria rastro de que has modificado la duracion y quitado la M
//TODO: A Combo de empleado con codigo y nombre y que se pueda escoger nombre escribiendo
//TODO: B no llamar a elegirEstilo cada bucle
//TODO: B Contador de incidencias.contar banderitas
//TODO: C Colores configurables
//TODO: C Permisos en los botones de contextuales (tal vez suficiente con acceso a la aplicacion)

using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.OperarioMotor;
using RumboSGA.Presentation.Herramientas.Ventanas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using RumboSGA.Herramientas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.Data;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class FrmProductividad : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        public int idFormulario;
        private MaquinaEstados meOperarios = new MaquinaEstados();
        private DataTable tablaGrid = new DataTable();
        private DataTable tablaGridTemplate = new DataTable();
        private DataTable table = new DataTable();
        private int _cantidadRegistros = 0;
        private string valorAnterior = "";
        private RadWaitingBar radWait = new RadWaitingBar();
        private BackgroundWorker bgWorkerGridViewProductividad;
        private RadRibbonBarGroup grupoLabel = new RadRibbonBarGroup();
        private string nombreEstilo;
        private List<TableScheme> tableScheme = null;
        private String transModificado = Lenguaje.traduce("modificado");
        
        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        private RadContextMenu contextMenu = new RadContextMenu();

        public RadGridView gridViewControl
        {
            get { return this.rgvOperario; }
        }

        protected dynamic _selectedRow;
        protected GridScheme _esquemaGrid;
        public string name = Lenguaje.traduce("Productividad");
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        protected List<TableScheme> _lstEsquemaTabla = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaJerarquico = new List<TableScheme>();
        private string nombreJson = "";
        private dynamic lineaSeleccionadaActual;
        FuncionesGenerales fungen = new FuncionesGenerales();
        RumMenuItem descartarMenuItem = new RumMenuItem();
        #endregion
            #region Constructor

        public FrmProductividad()
        {
            try
            {
                InitializeComponent();
                this.nombreJson = "OperariosMovimientos";
                this.tableScheme = FuncionesGenerales.CargarEsquema(nombreJson);
                IniciarControles();
                ControlesLenguaje();
                this.Shown += form_Shown;
                this.Show();
                InicializaWorkerProductividad();
                Eventos();
                AñadirLabelCantidad();
                InicializarComboPaginacion();
                radWait.StartWaiting();
                bgWorkerGridViewProductividad.RunWorkerAsync();
                this.nombreEstilo = this.Name + "GridView.xml";
                this.Name = name;
                FuncionesGenerales.RumDropDownAddManual(ref rddBtnConfigurar, 20083);
                FuncionesGenerales.AddEliminarLayoutButton(ref rddBtnConfigurar);
                AñadirCamposBotonConfig(ref rddBtnConfigurar);
                if (this.rddBtnConfigurar.Items["RadMenuItemEliminarLayout"] != null)
                {
                    this.rddBtnConfigurar.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        if (gridViewControl is RadGridView)
                        {
                            FuncionesGenerales.EliminarLayout(nombreEstilo, gridViewControl);
                        }
                    });
                }
                //FuncionesGenerales.DeshabilitarControlesPermisos(20083,RibbonBar);
                InicializarContextMenu();
                
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void coloresModificado()
        {
            foreach (GridViewDataRowInfo linea in rgvOperario.Rows)
            {
                    if (linea.Cells[transModificado].Value.ToString() == "Si")
                    {
                        fungen.EstilosColor(linea, Color.Yellow, transModificado);

                    }
                    else if (linea.Cells[transModificado].Value.ToString() == "No")
                    {
                        fungen.EstilosColor(linea, Color.Green, transModificado);
                    }
                    else if (linea.Cells[transModificado].Value.ToString() == "Descartado")
                    {
                        fungen.EstilosColor(linea, Color.Red, transModificado);
                    }
            }
        }
        private void InicializarContextMenu()
        {
            try
            {
                gridViewControl.ContextMenuOpening += (sender, e) =>
                {
                    
                };
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            
            
        }

        private void descartarMenuItem_Event(object sender, EventArgs e)
        {
            //tableScheme.
            if (this.rgvOperario.CurrentRow.Cells[transModificado].Value.ToString() =="Descartado")
            {
                this.rgvOperario.CurrentRow.Cells[transModificado].Value = "No";
                Business.UpdateCell(" IDMOVIMIENTO = " + this.rgvOperario.CurrentRow.Cells[0].Value.ToString(), "TBLOPERARIOSMOV"," modificado = 'N' ");
            }
            else
            {
                this.rgvOperario.CurrentRow.Cells[transModificado].Value = "Descartado";
                Business.UpdateCell( " IDMOVIMIENTO = " + this.rgvOperario.CurrentRow.Cells[0].Value.ToString(), "TBLOPERARIOSMOV"," modificado= 'D' ");
            }

        }

        private void InicializaWorkerProductividad()
        {
            bgWorkerGridViewProductividad = new BackgroundWorker();
            bgWorkerGridViewProductividad.WorkerReportsProgress = true;
            bgWorkerGridViewProductividad.WorkerSupportsCancellation = true;

            bgWorkerGridViewProductividad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewProductividad_RunWorkerCompleted);

            bgWorkerGridViewProductividad.DoWork += new DoWorkEventHandler(llenarGridProductividad);
        }

        private void IniciarControles()
        {
            try
            {
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                tlPanelPrincipal.Dock = DockStyle.Fill;
                rgvOperario.Dock = DockStyle.Fill;

                this.radWait.Name = "radWaitingBar1";
                this.radWait.Size = new System.Drawing.Size(200, 20);
                this.radWait.TabIndex = 2;
                this.radWait.Text = "";
                this.radWait.AssociatedControl = this.rgvOperario;
                this.ddlOperario.DataSource = Business.GetListaOperarios();
                this.ddlOperario.DisplayMember = "Nombre";
                this.ddlOperario.ValueMember = "idOperario";

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
            //name = Lenguaje.traduce(strings.Tareas);
            this.Text = Lenguaje.traduce(NombresFormularios.Productividad);
            editColumns.Text = Lenguaje.traduce(strings.Columnas);
            guardarButton.Text = Lenguaje.traduce(strings.GuardarEstilo);
            rBtnBorrarFiltro.Text = Lenguaje.traduce(strings.LimpiarFiltros);
            cargarButton.Text = Lenguaje.traduce(strings.CargarEstilo);
            temasMenuItem.Text = Lenguaje.traduce(strings.Temas);
            ribbonTab1.Text = Lenguaje.traduce(strings.Acciones);
            configBarGroup.Text = Lenguaje.traduce(strings.Configuracion);
            vistaBarGroup.Text = Lenguaje.traduce(strings.Ver);
            pagHeaderItem.Text = Lenguaje.traduce(strings.RegistrosPaginacion);
            radRibbonBarGroupIncidencias.Text = Lenguaje.traduce("Incidencias");
            grupoSeleccion.Text = Lenguaje.traduce("Selección");
            checkNoAccion.Text = Lenguaje.traduce(checkNoAccion.Text);
            rChkOcultarDescartados.Text = Lenguaje.traduce(rChkOcultarDescartados.Text);
        }

        #endregion Constructor

        #region Llamada Eventos Propios

        private void Eventos()
        {
            try
            {
                rgvOperario.ViewRowFormatting += rgvPrincipal_ViewRowFormatting;
                rgvOperario.FilterChanged += rgvPrincipal_FilterChanged;
                rgvOperario.CellFormatting += rgvPrincipal_CellFormatting;
                rgvOperario.ViewCellFormatting += rgvPrincipal_CellFormattingWrapText;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvPrincipal_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            try
            {
                if (e.RowElement.RowInfo.HierarchyLevel == 1 && e.RowElement.RowInfo is GridViewNewRowInfo)
                {
                    e.RowElement.RowInfo.MaxHeight = 1;
                }
                if (!(e.RowElement.RowInfo.Cells[transModificado].Value is null))
                    if (e.RowElement.RowInfo.Cells[transModificado].Value.Equals("Descartado"))
                        e.RowElement.Font = new Font(e.RowElement.Font, FontStyle.Regular);
                    else
                        e.RowElement.Font = new Font(e.RowElement.Font, FontStyle.Regular);
                /*if (String.IsNullOrEmpty(e.RowElement.RowInfo.Cells["tipomovimiento"].Value.ToString()))
                    return; PBR 211206*/
                switch (e.RowElement.RowInfo.Cells[Lenguaje.traduce("tipomovimiento")].Value)
                {
                    case "LA"://entrada y salida amarillo fosfi
                    case "XA":
                        e.RowElement.BackColor = Color.Yellow;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "LP"://Picking en verdes
                    case "XP":
                        e.RowElement.BackColor = Color.GreenYellow;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "PI":
                        e.RowElement.BackColor = Color.Aquamarine;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "OP":
                        e.RowElement.BackColor = Color.LawnGreen;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "CP":
                        e.RowElement.BackColor = Color.LightSeaGreen;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "IM":
                        e.RowElement.BackColor = Color.PaleGreen;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "VE"://retrasar y ver en gris
                    case "RE":
                        e.RowElement.BackColor = Color.Gray;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "LH"://movimientos de palet completo en marrones
                    case "XH":
                        e.RowElement.BackColor = Color.RosyBrown;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "UC":
                        e.RowElement.BackColor = Color.SandyBrown;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "LC"://recepcion y conteo en azules
                    case "XC":
                        e.RowElement.BackColor = Color.Blue;
                        e.RowElement.ForeColor = Color.White;
                        break;

                    case "CO":
                        e.RowElement.BackColor = Color.AliceBlue;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "LB"://Produccion en rojos
                    case "XB":
                        e.RowElement.BackColor = Color.MediumVioletRed;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "AC":
                        e.RowElement.BackColor = Color.IndianRed;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    case "AB":
                        e.RowElement.BackColor = Color.PaleVioletRed;
                        e.RowElement.ForeColor = Color.Black;
                        break;

                    default:
                        e.RowElement.BackColor = Color.WhiteSmoke;
                        e.RowElement.ForeColor = Color.Black;
                        break;
                }
                coloresModificado();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                if (ex.Data.ToString().Equals("System.Collections.ListDictionaryInternal"))
                {
                    MessageBox.Show("ERROR al formatear tabla:" + ex.Message + "\n causa probable: tabla de movimientos sin actualizar");
                    log.Error("ERROR al formatear tabla:" + ex.Message + "\n ");
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("ERROR al formatear tabla:" + ex.Message);
                    log.Error("Error al formatear tabla: " + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
        }

        private void rgvPrincipal_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    e.CellElement.TextWrap = true;
                }
                GridDataCellElement cell = e.CellElement as GridDataCellElement;
                if (cell != null)
                {
                    if (cell.ContainsErrors)
                    {
                        cell.DrawBorder = true;
                        cell.BorderBoxStyle = BorderBoxStyle.FourBorders;

                        cell.BorderBottomColor = cell.BorderTopColor = cell.BorderRightShadowColor = cell.BorderLeftShadowColor =
                                cell.BorderBottomShadowColor = cell.BorderTopShadowColor = cell.BorderRightColor = cell.BorderLeftColor = Color.Red;
                        cell.BorderBottomWidth = cell.BorderTopWidth = cell.BorderRightWidth = cell.BorderLeftWidth = 2;

                        cell.ZIndex = 500;
                    }
                    else
                    {
                        cell.ResetValue(LightVisualElement.DrawBorderProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.BorderBoxStyleProperty, ValueResetFlags.Local);

                        cell.ResetValue(LightVisualElement.BorderBottomColorProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.BorderBottomShadowColorProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.BorderBottomWidthProperty, ValueResetFlags.Local);

                        cell.ResetValue(LightVisualElement.BorderTopColorProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.BorderTopShadowColorProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.BorderTopWidthProperty, ValueResetFlags.Local);

                        cell.ResetValue(LightVisualElement.BorderLeftColorProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.BorderLeftShadowColorProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.BorderLeftWidthProperty, ValueResetFlags.Local);

                        cell.ResetValue(LightVisualElement.BorderDrawModeProperty, ValueResetFlags.Local);
                        cell.ResetValue(LightVisualElement.ZIndexProperty, ValueResetFlags.Local);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvPrincipal_CellFormattingWrapText(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
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

        protected void rgvPrincipal_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
                GridDataView dataView = rgvOperario.MasterTemplate.DataView as GridDataView;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        #endregion Llamada Eventos Propios

        #region ActualizarGrid

        private void bgWorkerGridViewProductividad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //no es necesario ordenar si viene ordenado
                rgvOperario.DataSource = meOperarios.getFichajesValidados();
                //LoadLayoutLocal();

                ElegirEstilo();//No es necesario cada vez.
                foreach (DataRow fichaje in tablaGrid.Rows)
                {
                    if (fichaje.HasErrors)
                    {
                        int num = Convert.ToInt32(rTxtEleIncidencias.Text);
                        num++;
                        rTxtEleIncidencias.Text = num.ToString();
                    }
                }
                //rgvOperario.Refresh();
                SetPreferences();

                if (gridViewControl != null)
                {
                    ocultarDescartados();
                }
                if (gridViewControl != null && gridViewControl.Columns["duracion"] != null)
                {
                    (gridViewControl.Columns["duracion"] as GridViewDateTimeColumn).Format = DateTimePickerFormat.Custom;
                    (gridViewControl.Columns["duracion"] as GridViewDateTimeColumn).FormatString = "{0:T}";
                    //(gridViewControl.Columns["duracion"] as GridViewDateTimeColumn).FormatString =
                }
                rgvOperario.Refresh();
                rgvOperario.EndUpdate();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            radWait.StopWaiting();
        }

        #endregion ActualizarGrid

        #region Botones

        private string confirmacion = Lenguaje.traduce(strings.ExportacionExito);

        public void rBtnExportacion_Click(object sender, EventArgs e)
        {
            FuncionesGenerales.exportarAExcelGenerico(rgvOperario);
        }

        private void rBtnRefrescar_Click(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void Refrescar()
        {
            try
            {
                //log.Info("Pulsado botón refresh " + DateTime.Now);
                rgvOperario.EnablePaging = false;
                rgvOperario.Columns.Clear();
                bgWorkerGridViewProductividad.DoWork += new DoWorkEventHandler(llenarGridProductividad);
                radWait.StartWaiting();
                bgWorkerGridViewProductividad.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void llenarGridProductividad(object sender, DoWorkEventArgs e)
        {
            int idOperario = Convert.ToInt32(this.ddlOperario.SelectedValue);
            tablaGrid = Business.GetOperMov(Convert.ToDateTime(rdtFechaDesde.Value/*dtPicker.Value*/), idOperario);
            DepurarFichajes(sender, e);
        }

        private void DepurarFichajes(object sender, DoWorkEventArgs e)
        {
            try
            {
                //MaquinaEstados meOperarios = new MaquinaEstados();
                meOperarios.InicializaEstado();
                meOperarios.Compila(tablaGrid);
                //TODO actualizar rTxtEleIncidencias.Text= Convert.ToString( meOperarios.Errores);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Test" + ex.Message);
            }
        }

        public void ItemColumnas_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón editar columnas " + DateTime.Now);
            this.rgvOperario.ShowColumnChooser();
        }

        public void rBtnBorrarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón filtro " + DateTime.Now);
                if (rgvOperario.IsInEditMode)
                {
                    rgvOperario.EndEdit();
                }
                rgvOperario.FilterDescriptors.Clear();
                rgvOperario.GroupDescriptors.Clear();
                rgvOperario.BestFitColumns();
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
            string path = Persistencia.DirectorioGlobal;/* XmlReaderPropio.getLayoutPath(0);*/
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
                if (this.tlPanelPrincipal.GetControlFromPosition(0, 0) is RadGridView)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + nombreEstilo;
                    path.Replace(" ", "_");
                    gridViewControl.SaveLayout(path);
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
            string path = Persistencia.DirectorioLocal; /*XmlReaderPropio.getLayoutPath(1);*/
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
                if (this.tlPanelPrincipal.GetControlFromPosition(0, 0) is RadGridView)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    path.Replace(" ", "_");
                    gridViewControl.SaveLayout(path);
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void LoadLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal; /*XmlReaderPropio.getLayoutPath(0);*/
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
                if (this.tlPanelPrincipal.GetControlFromPosition(0, 0) is RadGridView)
                {
                    string s = path + "\\" + this.Name + "GridView.xml";

                    s.Replace(" ", "_");
                    string rutaFile = @"" + s;
                    if (File.Exists(rutaFile))
                    {
                        gridViewControl.LoadLayout(s);
                    }
                    else
                    {
                        MessageBox.Show("No existe ningun estilo en la ruta");
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                //MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void LoadLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal; /*XmlReaderPropio.getLayoutPath(1);*/
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
                if (this.tlPanelPrincipal.GetControlFromPosition(0, 0) is RadGridView)
                {
                    string s = path + "\\" + this.Name + "GridView.xml";

                    s.Replace(" ", "_");
                    string rutaFile = @"" + s;
                    if (File.Exists(rutaFile))
                    {
                        gridViewControl.LoadLayout(s);
                    }
                    else
                    {
                        MessageBox.Show("No existe ningun estilo en la ruta");
                    }
                    gridViewControl.LoadLayout(s);
                }
            }
            catch (DirectoryNotFoundException e)//TODO Si no está el fichero, el error no está controlado y el mensaje no ayuda.
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                //MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void ElegirEstilo()
        {
            string pathLocal = Persistencia.DirectorioLocal; /*XmlReaderPropio.getLayoutPath(1);*/
            string pathGlobal = Persistencia.DirectorioGlobal;/* XmlReaderPropio.getLayoutPath(0);*/
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
                string pathGridView = pathLocal + "\\" + this.Name + "GridView.xml";
                bool existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    gridViewControl.LoadLayout(pathGridView);
                }
                else
                {
                    pathGridView = pathGlobal + "\\" + this.Name + "GridView.xml";
                    existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        gridViewControl.LoadLayout(pathGridView);
                    }
                    else
                    {
                        rgvOperario.BestFitColumns();
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
            RadMenuItem mainItem = rddBtnConfigurar.Items[0] as RadMenuItem;
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
                        rgvOperario.BestFitColumns();
                    }
                }
                if (!loadedThemes.ContainsKey(themeName))
                {
                    Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
                    Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + themeName + "Theme"));
                    loadedThemes.Add(themeName, true);
                    rgvOperario.BestFitColumns();
                }
                ThemeResolutionService.ApplicationThemeName = themeName;
                if (ControlTraceMonitor.AnalyticsMonitor != null)
                {
                    ControlTraceMonitor.AnalyticsMonitor.TrackAtomicFeature("ThemeChanged." + themeName);
                    rgvOperario.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Temas

        #region Preferencias

        private void EstablecerPreferenciasFiltros()
        {
            this.rgvOperario.EnableFiltering = true;
            this.rgvOperario.MultiSelect = true;
            this.rgvOperario.MasterTemplate.EnableGrouping = true;
            this.rgvOperario.ShowGroupPanel = true;
            this.rgvOperario.MasterTemplate.AutoExpandGroups = true;
            this.rgvOperario.EnableHotTracking = true;
            this.rgvOperario.MasterTemplate.AllowAddNewRow = false;
            this.rgvOperario.MasterTemplate.AllowColumnResize = true;
            this.rgvOperario.MasterTemplate.AllowMultiColumnSorting = true;
            this.rgvOperario.AllowRowResize = false;
            this.rgvOperario.AllowSearchRow = true;
            this.rgvOperario.MasterView.TableSearchRow.SearchDelay = 2000;
        }

        private void SetPreferences()
        {
            try
            {
                gridViewControl.PageSize = 30;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvOperario.TableElement.RowHeight = 40;
                rgvOperario.BestFitColumns();
                if (rgvOperario.Templates.Count > 0)
                    rgvOperario.Templates[0].BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Preferencias

        private void AñadirCamposBotonConfig(ref RumDropDownButtonElement rmenu)
        {
            //Botón Modificar Tiemposgen.xml
            RumMenuItem TiemposGenLayout = new RumMenuItem();
            TiemposGenLayout.Name = "RadMenuItemTiemposGen";
            TiemposGenLayout.Text = Lenguaje.traduce("Modificar fichero configuración");
            TiemposGenLayout.Click +=RadMenuItemTiemposGen_OnClick;
            rmenu.Items.Add(TiemposGenLayout);

            //
            RumMenuItem EstadosOperariosLayout = new RumMenuItem();
            EstadosOperariosLayout.Name = "RadMenuItemEstadosOperarios";
            EstadosOperariosLayout.Text = Lenguaje.traduce("Modificar estados del operario");
            
            EstadosOperariosLayout.Click += RadMenuItemEstadoOperarios_Click;
            rmenu.Items.Add(EstadosOperariosLayout);
        }
        private void RadMenuItemEstadoOperarios_Click(object sender,EventArgs e)
        {
            try
            {
                AuxEditor ModificadorEstadosOperarios = new AuxEditor(Persistencia.DirectorioBase + "\\Configs\\EstadosOperarios.json");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            
        }
        private void RadMenuItemTiemposGen_OnClick(object sender, EventArgs e)
        {
            try
            {
                AuxEditor ModificadorTiemposGen = new AuxEditor(Persistencia.DirectorioBase + "\\Utilidades\\xmlconfiguracion\\tiemposGen.xml");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }
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
                rgvOperario.PageSize = tamaño;
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
            this.ribbonTab1.Items.AddRange(grupoLabel);
        }

        private void rbtnOperAtras_Click(object sender, EventArgs e)
        {
            MoverOperario(false);
            rBtnRefrescar_Click(sender, e);
        }

        private void rbtnoperAdelante_Click(object sender, EventArgs e)
        {
            MoverOperario(true);
            rBtnRefrescar_Click(sender, e);
        }

        private void MoverOperario(bool adelante)
        {
            try
            {
                int filaActual = ddlOperario.SelectedIndex;
                if (adelante)
                {
                    if (filaActual < ddlOperario.Items.Count)
                    {
                        ddlOperario.SelectedIndex++;
                    }
                }
                else
                {
                    if (filaActual > 0)
                        ddlOperario.SelectedIndex--;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error avanzando Operario") + "\n" + ex.Message);
            }
        }

        private void rcombooperario_SelectedIndexChanged(object sender, EventArgs e)
        {
            rBtnRefrescar_Click(sender, e);
        }

        private void radRibbonBarGroup2_Click(object sender, EventArgs e)
        {
        }

        private void rBtnResolver_Click(object sender, EventArgs e)
        {
            try
            {
                meOperarios.ResolverIncidencias();
                this.tablaGrid = meOperarios.getFichajesValidados();
                foreach (DataRow fichaje in tablaGrid.Rows)
                {
                    if (fichaje.HasErrors)
                    {
                        int num = Convert.ToInt32(rTxtEleIncidencias.Text);
                        num++;
                        rTxtEleIncidencias.Text = num.ToString();
                    }
                }
                this.gridViewControl.DataSource = tablaGrid;
                this.Refresh();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radCheckBoxElement1_Click(object sender, EventArgs e)
        {
            if (checkNoAccion.ToggleState == ToggleState.On)
            {
                meOperarios.mostrarNoAccion = false;
            }
            else
            {
                meOperarios.mostrarNoAccion = true;
            }
        }

        private void rbtnGuardar_Click(object sender, EventArgs e)
        {
            //Inserta o actualiza en la base de datos los registros añadidos o modificados.
            try
            {
                Business.SetOperMov(meOperarios.getFichajesValidados());
                rbtnGuardar.BackColor = Color.Transparent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Test" + ex.Message);
            }
        }

        private void rbtnCalcular_Click(object sender, EventArgs e)
        {
            calcularTiempos();
            rBtnRefrescar_Click(sender, e);
        }

        private void RbtnCalcularAvanzado_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechadesde = (DateTime)rdtFechaDesde.Value;
                /*VentanaDateTimePicker vtn = new VentanaDateTimePicker(fechadesde, fechadesde.AddDays(7));
                vtn.ShowDialog();*/
                FrmSeleccionOpDate vtn = new FrmSeleccionOpDate(fechadesde, fechadesde.AddDays(7));
                vtn.ShowDialog();
                if (vtn.DialogResult == DialogResult.Cancel) return;

                this.Cursor = Cursors.WaitCursor;
                int[] idOperario = vtn.operarios.ToArray(); ;
                //log.Debug("Se procede a calculoTiempos(" + idOperarioDesde + "," + idOperarioHasta + ","
                //    + vtn.FechaDesde.ToString("d") + "," + vtn.FechaHasta.ToString("d"));
                WSOperarioMotorClient ws = new WSOperarioMotorClient();
                string respuesta = ws.calculoTiempos(idOperario, vtn.FechaDesde, vtn.FechaHasta);
                //RumLog rml = RumLog.getRumLog("Productividad", "Calculo avanzado tiempos");
                //rml.parametros = Lenguaje.traduce("OperarioDesde") + idOperarioDesde + ";" + Lenguaje.traduce("OperarioHasta") + idOperarioHasta + ";"
                //    + Lenguaje.traduce("FechaDesde") + vtn.FechaDesde;
                //RumLog.EscribirRumLog(rml);
                //log.Debug(respuesta);
                rBtnRefrescar_Click(sender, e);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en calcular tiempos avanzado");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void calcularTiempos()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int[] idOperario = new int[1]{Convert.ToInt32(ddlOperario.SelectedValue)};
                DateTime fechadesde = (DateTime)rdtFechaDesde.Value;
                WSOperarioMotorClient ws = new WSOperarioMotorClient();
                ws.calculoTiempos(idOperario, fechadesde, fechadesde.AddDays(1));
                RumLog rml = RumLog.getRumLog("Productividad", "Calculo tiempos");
                rml.parametros = Lenguaje.traduce("OperarioDesde") + idOperario + ";" + Lenguaje.traduce("OperarioHasta") + idOperario + ";"
                    + Lenguaje.traduce("FechaDesde") + fechadesde.AddDays(1);
                RumLog.EscribirRumLog(rml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error llamando al servicio de cálculo de tiempos") + "\n" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ocultarDescartados()
        {
            if (rChkOcultarDescartados.Checked)//Cuando llega aqui el check todavia no ha cambiado. Si no esta checked es que lo quieren activar
            {
                //añado un filtro en el grid para que no se vean las marcadas como descartadas en el campo estado
                this.rgvOperario.FilterDescriptors.Remove(transModificado);
                FilterDescriptor descriptor = new FilterDescriptor(transModificado, FilterOperator.IsNotEqualTo, "Descartado");
                this.rgvOperario.FilterDescriptors.Add(descriptor);
            }
            else
            {
                try
                {
                    this.rgvOperario.FilterDescriptors.Remove(transModificado);
                }
                catch (Exception ex)
                { }
            }
            this.rgvOperario.Refresh();
        }

        private void RChkOcultarDescartados_CheckStateChanged(object sender, EventArgs e)
        {
            ocultarDescartados();
        }

        private void RgvOperario_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (lineaSeleccionadaActual == null)
                {
                    log.Error("Error seleccionando la línea actual, puede ser que el estilo este incorrecto?");
                    return;
                }
                if (valorAnterior.Equals("VALORNULODECOLUMNA") && e.Value == null)
                {
                    return;
                }
                else if (valorAnterior.Equals("VALORNULODECOLUMNA") && e.Value != null)
                {
                    marcarModificado(e.RowIndex);
                    return;
                }

                if (valorAnterior.Equals(e.Value.ToString()))
                {
                    return;
                }
                marcarModificado(e.RowIndex);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void marcarModificado(int rowIndex)
        {
            try
            {
                if (rgvOperario.Rows[rowIndex].Cells[transModificado] != null)
                {
                    rgvOperario.Rows[rowIndex].Cells[transModificado].Value = "Si";
                    rbtnGuardar.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void RgvOperario_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.Row.Cells[e.ColumnIndex].Value != null)
                {
                    lineaSeleccionadaActual = FuncionesGenerales.getRowGridView(rgvOperario, tableScheme, null, null, null, e.RowIndex);
                    this.valorAnterior = e.Row.Cells[e.ColumnIndex].Value.ToString();
                }
                else
                {
                    valorAnterior = "VALORNULODECOLUMNA";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void ddlOperario_SelectedValueChanged(object sender, Telerik.WinControls.UI.Data.ValueChangedEventArgs e)
        {
            Refrescar();
        }

        private void rdtFechaDesde_ValueChanged(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void rgvOperario_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
                /*GridCellElement cellElement = e.ContextMenuProvider as GridCellElement;
                    if (cellElement == null || cellElement.RowInfo is GridViewFilteringRowInfo || cellElement.RowInfo is GridViewTableHeaderRowInfo)
                    {
                        descartarMenuItem.Text = Lenguaje.traduce("Descartar");
                        descartarMenuItem.Click += new EventHandler(descartarMenuItem_Event);
                        contextMenu.Items.Add(descartarMenuItem);
                        return;
                    }
                    if (cellElement.ColumnInfo == null || cellElement.ColumnInfo.HeaderText == null)
                    {
                        return;
                    }
                    */
                GridCellElement cellElement = e.ContextMenuProvider as GridCellElement;
                if (cellElement == null || cellElement.RowInfo is GridViewFilteringRowInfo || cellElement.RowInfo is GridViewTableHeaderRowInfo || cellElement.RowInfo is GridViewSearchRowInfo)
                {

                    return;
                }
                else
                //if (cellElement.ColumnInfo == null || cellElement.ColumnInfo.HeaderText == null)
                {
                    descartarMenuItem.Text = Lenguaje.traduce("Descartar");
                    descartarMenuItem.Click += new EventHandler(descartarMenuItem_Event);
                    contextMenu.Items.Add(descartarMenuItem);
                    if (this.rgvOperario.CurrentRow.Cells[transModificado].Value.ToString() == "Descartado")
                    {
                        descartarMenuItem.Text = Lenguaje.traduce("Recuperar");
                    }
                    else if (this.rgvOperario.CurrentRow.Cells[transModificado].Value.ToString() == "No")
                    {
                        descartarMenuItem.Text = Lenguaje.traduce("Descartar");
                    }
                    return;
                }
                e.ContextMenu = this.contextMenu.DropDown;
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }
    }

    #region MaquinaEstados /////////////////////////////////////////////////////////////////////////////////////

    internal class MaquinaEstados
    {
        private struct Estado
        {
            private readonly string estadoIni;
            private readonly string accion;
            private readonly string estadoFin;
            private readonly string correcto;
            private readonly string mensaje;
            private readonly string correccion;
        }

        public string EstadoActual { get; set; }
        private static DataTable estados;
        private DataTable fichajesRaw;//solo movimientos
        private DataTable fichajesValidados;//movimientos y estados
        private DataTable fichajesAjustados;//movimientos, estados y nuevos fichajes de correccion.
        private DataRow drwEstadoActual;
        private DataTable descripcionMovimientos;
        private List<string> estadosNoAccion = null;
        public Dictionary<string, TareaTipo> dTareasTipo = new Dictionary<string, TareaTipo>();
        public bool mostrarNoAccion { get; set; }

        private int errores { get; }
        public int Errores { get; private set; }

        public MaquinaEstados()
        {
            string[] estNoAcc = { "VE", "RE" };
            CargaReservasTipo();
            descripcionMovimientos = Business.GetDescripcionMovimientos();//lo cargo una vez para siempre.
            CargaEstados();
            TraduceEstados();
            //cargo los estados que no son de accion para mejorar la visibilidad
            estadosNoAccion = new List<string>(estNoAcc);
        }

        public DataTable getFichajesValidados()
        {
            return fichajesValidados;
        }

        public void SetFichajesMovimientos(DataTable fichajes)
        {
            fichajesRaw = fichajes;
            fichajesValidados = CreaFichajesValidados(fichajes);//no importa si ya tiene los campos añadidos
        }

        public void Compila(DataTable fichajes)
        {
            SetFichajesMovimientos(fichajes);
            Compila();
        }

        public void Compila()
        {
            DataRow estadoEncontrado = null;
            DataRow fichajeValidado = null;
            bool vacio = true;
            //TODO guardar datatable
            try
            {
                Errores = 0;
                foreach (DataRow fichaje in fichajesRaw.Rows)
                {
                    vacio = false;
                    if (MostrarAccion(Convert.ToString(fichaje[Lenguaje.traduce("tipomovimiento")])))
                    {
                        estadoEncontrado = validarFichaje(EstadoActual, fichaje);
                        fichajeValidado = AddFichaje(fichaje, estadoEncontrado);
                    }
                }
                if (!EstadoActual.Equals("Fuera") && !vacio)
                {
                    if (fichajeValidado[Lenguaje.traduce("mensaje")].Equals(""))//Si ya tiene un error, ya pondremos este en otra vuelta.
                    {
                        object tmpTipoMovimiento = fichajeValidado[Lenguaje.traduce("tipomovimiento")];
                        fichajeValidado[Lenguaje.traduce("tipomovimiento")] = "";
                        estadoEncontrado = validarFichaje(EstadoActual, fichajeValidado);
                        fichajeValidado[Lenguaje.traduce("tipomovimiento")] = tmpTipoMovimiento;
                        if (estadoEncontrado is null)
                        {
                            fichajeValidado[Lenguaje.traduce("correcto")] = false;
                            fichajeValidado[Lenguaje.traduce("correccion")] = "Add XA post";
                            fichajeValidado[Lenguaje.traduce("mensaje")] = Lenguaje.traduce("Falta salida aplicación");
                            fichajeValidado.RowError = Lenguaje.traduce("Falta salida aplicación");
                        }
                        else
                        {
                            fichajeValidado[Lenguaje.traduce("correcto")] = estadoEncontrado[Lenguaje.traduce("correcto")];
                            fichajeValidado[Lenguaje.traduce("correccion")] = estadoEncontrado[Lenguaje.traduce("correccion")];
                            fichajeValidado[Lenguaje.traduce("mensaje")] = estadoEncontrado[Lenguaje.traduce("mensaje")];
                            fichajeValidado.RowError = Convert.ToString(estadoEncontrado[Lenguaje.traduce("mensaje")]);
                        }

                        Errores++;
                    }

                    //AddFichaje(fichajeValidado, estadoEncontrado);
                }
                //cierre ---------------------------------------------------------------
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("error Insertando incidencias actividades") + "\n" + e.Message);
            }
            return;
        }

        private DataTable CreaFichajesValidados(DataTable fichajes)
        {//Crea una tabla VACIA como fichajesRow mas las columnas de estado
            //como puedo reprocesar varias veces el datatable, es posible que ya tenga las columnas añadidas.
            try
            {
                DataTable fv = fichajes.Clone();
                DataColumn colNew;
                //y le añado las nuevas si no existen
                if (!fv.Columns.Contains("EstadoIni"))
                {
                    colNew = new DataColumn(Lenguaje.traduce("EstadoIni"), typeof(String));
                    fv.Columns.Add(colNew);
                }
                if (!fv.Columns.Contains("EstadoFin"))
                {
                    colNew = new DataColumn(Lenguaje.traduce("EstadoFin"), typeof(String));
                    fv.Columns.Add(colNew);
                }
                if (!fv.Columns.Contains("correcto"))
                {
                    colNew = new DataColumn(Lenguaje.traduce("correcto"), typeof(String));
                    fv.Columns.Add(colNew);
                }
                if (!fv.Columns.Contains("Mensaje"))
                {
                    colNew = new DataColumn(Lenguaje.traduce("Mensaje"), typeof(String));
                    fv.Columns.Add(colNew);
                }
                if (!fv.Columns.Contains("correccion"))
                {
                    colNew = new DataColumn(Lenguaje.traduce("correccion"), typeof(String));
                    fv.Columns.Add(colNew);
                }

                return fv;
            }
            catch (Exception e)
            {
                MessageBox.Show("error " + e.Message);
            }
            return null;
        }

        public void ResolverIncidencias()
        {
            //recorre el actual datatable y llama a añadir correcion para resolver
            //Las filas sin incidencia tambien se envian para que las copie al datatable nuevo
            fichajesAjustados = fichajesValidados.Clone();
            string correccion;
            try
            {
                foreach (DataRow fila in fichajesValidados.Rows)
                {
                    correccion = Convert.ToString(fila[Lenguaje.traduce("correccion")]).ToLower();
                    switch (correccion)
                    {
                        case "add la prev":
                            AddCorreccion(fila, "LA", true);
                            break;

                        case "add lh prev":
                            AddCorreccion(fila, "LH", true);
                            break;

                        case "add xa post":
                            AddCorreccion(fila, "XA", false);
                            break;

                        case "delete":
                            AddCorreccion(fila, "delete", false);
                            break;

                        default:
                            AddCorreccion(fila, null, true);
                            break;
                    }
                }
                fichajesValidados = fichajesAjustados;
                InicializaEstado();
                Compila(fichajesAjustados);
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("Error ajustando fichajes") + "\n" + e.Message);
            }
        }

        private void AddCorreccion(DataRow fichaje, string accion, bool anterior)
        {
            //recibe un dataRow de fichajes y lo inserta en un datatable nuevo:fichajesAjustados
            //Si el datarow tiene una accion de correccion la ejecuta, si es necesario añadiendo un datarow nuevo
            DataRow nuevo = null;
            string nombreCol;
            try
            {
                //limpio errores
                fichaje.ClearErrors();

                if (accion is null)
                {
                    //Es un fichaje sin incidencias: se añade una copia
                    fichajesAjustados.ImportRow(fichaje);
                }
                else
                {
                    if (accion.Equals("delete"))
                    {
                        fichaje[Lenguaje.traduce("Mensaje")] = "Duplicado";//Lo marco
                        fichaje[Lenguaje.traduce("correccion")] = "Descartar";
                        fichaje[Lenguaje.traduce("modificado")] = "D";
                        fichajesAjustados.ImportRow(fichaje);// y lo añado.
                        return;
                    }
                    else
                    {
                        //quito el mensaje de error del fichaje original porque seguramente ya no es error
                        fichaje["Mensaje"] = "";
                        fichaje["correccion"] = "";
                        fichaje["modificado"] = "N";
                        //copio los campos de un datarow a otro
                        nuevo = fichajesAjustados.NewRow();//creo uno vacio
                        for (int nCol = 0; nCol < fichajesValidados.Columns.Count; nCol++)
                        {
                            if (!(fichaje[nCol] is DBNull))
                            {
                                nombreCol = fichajesValidados.Columns[nCol].ColumnName;
                                Debug.Print(nombreCol);
                                switch (nombreCol)
                                {
                                    case "TIPOMOVIMIENTO":
                                        if (accion is null)
                                            nuevo[nCol] = fichaje[nCol];
                                        else
                                            nuevo[nCol] = accion;
                                        break;

                                    case "tipomovimiento":
                                        nuevo[nCol] = GetDescripcionMovimiento(accion);
                                        break;

                                    case "FECHAHORA":
                                        DateTime horaFichaje = Convert.ToDateTime(fichaje[nCol]);
                                        if (anterior)
                                            nuevo[nCol] = horaFichaje.AddSeconds(-1);
                                        else
                                            nuevo[nCol] = horaFichaje.AddSeconds(1);
                                        break;

                                    case "modificado":
                                        nuevo[nCol] = "N";
                                        break;

                                    default:
                                        nuevo[nCol] = fichaje[nCol];
                                        break;
                                }
                            }
                        }
                    }
                    //grabo los fichajes en orden
                    if (anterior)
                    {
                        fichajesAjustados.Rows.Add(nuevo);
                        fichajesAjustados.ImportRow(fichaje);
                    }
                    else
                    {
                        fichajesAjustados.ImportRow(fichaje);
                        fichajesAjustados.Rows.Add(nuevo);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Test" + e.Message);
            }
        }

        private string GetDescripcionMovimiento(string tipoMovimiento)
        {
            string select = "";
            try
            {
                select = "tipoMovimiento='" + tipoMovimiento + "'";
                DataRow[] dr = descripcionMovimientos.Select(select);
                if (dr.Length == 0)
                    return tipoMovimiento;//si el movimiento no está en la tabla devuelvo el tipo tambien como descripcion.
                else
                    return Convert.ToString(dr[0][1]);
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("error en descripcion de movimientos") + "\n" + select + "\n" + e.Message);
            }
            return tipoMovimiento;
        }

        private DataRow validarFichaje(string estadoActual, DataRow fichaje)
        {
            DataRow drEstado = buscarEstado(estadoActual, Convert.ToString(fichaje[Lenguaje.traduce("tipomovimiento")]));
            if (drEstado is null)
            {
                drEstado = null;
                //TODO dar la linea por usable
                //el estado no cambia
            }
            else
            {
                estadoActual = Convert.ToString(drEstado[Lenguaje.traduce("EstadoFin")]);
            }
            drwEstadoActual = drEstado;

            return drEstado;
        }

        private DataRow AddFichaje(DataRow fichaje, DataRow estado)
        {
            bool error = false;
            string msgError = null;
            try
            {
                DataRow drwFichVal = fichajesValidados.NewRow();
                foreach (DataColumn col in fichaje.Table.Columns)
                {
                    string colName = col.ColumnName;
                    drwFichVal[colName] = fichaje[colName];
                }
                if (estado is null)
                {
                    drwFichVal[Lenguaje.traduce("EstadoIni")] = EstadoActual;
                    drwFichVal[Lenguaje.traduce("EstadoFin")] = EstadoActual;
                }
                else
                {
                    foreach (DataColumn col in estado.Table.Columns)
                    {
                        string colName = col.ColumnName;
                        if (drwFichVal.Table.Columns.Contains(colName))
                        {
                            drwFichVal[colName] = estado[colName];
                            if (colName.Equals("correcto"))
                                if (!Convert.ToBoolean(estado[colName]))
                                {
                                    drwFichVal.SetColumnError(Lenguaje.traduce("tipomovimiento"), Convert.ToString(estado[Lenguaje.traduce("mensaje")]));
                                    msgError = Convert.ToString(estado[Lenguaje.traduce("mensaje")]);
                                    Errores++;
                                    error = true;
                                }
                        }
                    }
                    EstadoActual = Convert.ToString(drwFichVal[Lenguaje.traduce("estadoFin")]);
                }
                //Validar duraciones
                if (!ValidarDuraciones(drwFichVal) && fichajesValidados.Rows.Count > 0)
                    fichajesValidados.Rows[fichajesValidados.Rows.Count - 1].RowError = "duración anormal";
                fichajesValidados.Rows.Add(drwFichVal);
                fichajesValidados.Rows[fichajesValidados.Rows.Count - 1].RowError = msgError;
                return drwFichVal;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error creando fichajes\n" + e.Message);
            }
            return null;
        }

        private DataRow buscarEstado(string estadoActual, string accion)
        {
            string select = "";
            try
            {
                select = Lenguaje.traduce("EstadoIni") + "='" + estadoActual + "' AND "+Lenguaje.traduce("Accion")+"='" + accion + "'";
                DataRow[] dr = estados.Select(select);
                if (dr.Length == 0)
                {
                    //no ha encontrado el estado inicial y la accion
                    //TODO lo añado al fichero
                    return null;
                }
                else
                {
                    //TODO validar que el fichero no tienes duplicados, no deberia!
                    return dr[0];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("error buscando estados " + e.Message);
            }
            return null;
        }

        public bool MostrarAccion(String idTipoMovimiento)
        {
            if (mostrarNoAccion)
                return true;
            else
                return (!estadosNoAccion.Contains(idTipoMovimiento));
        }

        public void InicializaEstado()
        {
            EstadoActual = Lenguaje.traduce("Fuera");
        }

        private void TraduceEstados()
        {
            foreach (DataRow drw in estados.Rows)
            {
                drw["estadoIni"] = Lenguaje.traduce(Convert.ToString(drw["estadoIni"]));
                drw["estadoFin"] = Lenguaje.traduce(Convert.ToString(drw["estadoFin"]));
                drw["mensaje"] = Lenguaje.traduce(Convert.ToString(drw["mensaje"]));
            }
        }

        private void CargaEstados()
        {
            string origen = Persistencia.DirectorioBase + @"\configs\EstadosOperarios.json";
            try
            {
                StreamReader read = new StreamReader(origen);
                string json = read.ReadToEnd();
                estados = JsonConvert.DeserializeObject<DataTable>(json);
            }
            catch (Exception e)
            {
                MessageBox.Show("error cargando JSON estados operarios" + "\n origen" + origen + e.Message);
            }
        }

        public bool ValidarDuraciones(DataRow dr)
        {
            String tipoMovimiento = "??";
            TareaTipo tt;
            int duracion;
            try
            {
                tipoMovimiento = Convert.ToString(dr[Lenguaje.traduce("tipoMovimiento")]);
                if (tipoMovimiento is null)
                {
                    MessageBox.Show("No se ha encontrado el tipo de movimiento: " + dr[Lenguaje.traduce("tipoMovimiento")]);
                    return false;
                }
                tt = dTareasTipo[tipoMovimiento];
                duracion = Convert.ToInt32(dr[Lenguaje.traduce("segundos")]);//En la clase business se eliminan los nulos
                if (duracion < tt.Mini)
                {
                    dr[Lenguaje.traduce("mensaje")] = "Accion demasiado corta < " + tt.Mini;
                    return false;
                    //marcar error
                }
                else if ((duracion > 0) && (duracion > tt.Maxi))
                {
                    dr[Lenguaje.traduce("mensaje")] = "Accion demasiado larga > " + tt.Maxi;
                    return false;
                    //marcar error
                }
                return true;
            }
            catch (Exception e)
            {
                if (e.GetBaseException().GetType() == typeof(KeyNotFoundException))
                {
                    MessageBox.Show("Falta el tipo de movimiento en la tabla TBLRESERVASTIPO: " + tipoMovimiento + "\n" + e.Message);
                    //guardar en warnings
                    //abrir un jira con el problema
                }
                else
                {
                    MessageBox.Show("error validando duraciones \n " + "tipo de movimiento: " + tipoMovimiento + "\n" + e.Message);
                    //guardar en warnings
                    //abrir un jira con el problema
                }
                return false;
            }
        }

        private void CargaReservasTipo()
        {
            DataTable dtHashTareasTipo;

            try
            {
                dtHashTareasTipo = Business.GetTareasTipo();
                foreach (DataRow drT in dtHashTareasTipo.Rows)
                {
                    TareaTipo tt = new TareaTipo(Convert.ToString(drT["idreservatipo"]),
                                                Convert.ToInt32(drT["duracionmedia"]),
                                                Convert.ToInt32(drT["duracionminima"]),
                                                Convert.ToInt32(drT["duracionmaxima"]),
                                                Convert.ToDouble(drT["duraciondesviacion"])
                                                );
                    dTareasTipo.Add(Convert.ToString(drT["idreservatipo"]), tt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando tipos de reserva" + "\n" + ex.Message);
            }
        }
    }

    #endregion MaquinaEstados /////////////////////////////////////////////////////////////////////////////////////

    #region dictionary TareasTipo///////////////////// Clases para el diccionario  de tareas tipo

    internal class TareaTipo
    {
        public string TareaTip;
        public int Med, Mini, Maxi;
        public double Desv;

        public TareaTipo(string tareaTipo, int media, int Minimo, int Maximo, Double Desviacion)
        {
            TareaTip = tareaTipo;
            Med = media;
            Mini = Minimo;
            Maxi = Maximo;
            Desv = Desviacion;
        }

        public override string ToString()
        {
            return TareaTip + " Media:" + Med + " mimimo:" + Mini + " maximo:" + Maxi + "desv:" + Desv;
        }
    }

    #endregion dictionary TareasTipo///////////////////// Clases para el diccionario  de tareas tipo
}
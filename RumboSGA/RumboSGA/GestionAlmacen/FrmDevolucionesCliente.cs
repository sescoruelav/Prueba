using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using RumboSGA.Presentation.UserControls;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using RumboSGA.Presentation.Herramientas.PantallasWS;
using System.Web.Script.Serialization;
using RumboSGA.DevolucionesMotor;
using System.Dynamic;
using RumboSGA.Presentation.Herramientas.Ventanas;
using Rumbo.Core.Herramientas.Herramientas;

namespace RumboSGA.GestionAlmacen
{
    public partial class FrmDevolucionesCliente : Telerik.WinControls.UI.RadRibbonForm
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string nombreJson = "DevolucionesCliente";
        private static readonly string nombreJsonJerLineas = "DevolucionesClienteJerarquicoLineas";
        private static readonly string nombreJsonJerEntradas = "DevolucionesClienteJerarquicoEntradas";
        private static readonly string nombreJsonJerExistencias = "DevolucionesClienteJerarquicoExistencias";
        private static readonly string nombreJsonJerReservas = "DevolucionesClienteJerarquicoReservas";
        private static readonly string nombreJsonJerTareas = "DevolucionesClienteJerarquicoTareas";


        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        RadWaitingBar radWaitDevoluciones = new RadWaitingBar();

        GridViewCheckBoxColumn chkColDevoluciones = new GridViewCheckBoxColumn();

        protected List<TableScheme> _lstEsquemaTablaDevoluciones = new List<TableScheme>();

        private BackgroundWorker bgWorkerGridViewDevoluciones;

        private DataTable dtDevoluciones = new DataTable();


        private RadRibbonBarButtonGroup grupoLabel = new RadRibbonBarButtonGroup();
        private RadLabelElement lblCantidad = new RadLabelElement();

        private RadDataFilterDialog radDataFilterDevoluciones = new RadDataFilterDialog();

        private FilterDescriptorCollection filtroAnteriorDevoluciones = new FilterDescriptorCollection();


        int idDevolCliLinSeleccionado = -1;

        private static int jerarquicoPosicionLineas = 0;
        private static int jerarquicoPosicionEntradas = 1;
        private static int jerarquicoPosicionStock = 2;
        private static int jerarquicoPosicionMovimientos = 3;
        private static int jerarquicoPosicionTareas = 4;

        ProgressBarColumn colPedidosPorcentLineas;
        private string colPorcentLineas;
        ProgressBarColumn colJerLineasPedidosPorcentEnt;        
        private string colCantidadEntrada;
        ProgressBarColumn colJerLineasPedidoPorcentEntPend;
        private string colCantidadPteUbicar;
        

        /*  int devConLineas = Persistencia.getParametroInt("DEVCONLINEAS");
private const int CONLINEAS = 0;
private const int SINLINEAS = 1;*/
        public FrmDevolucionesCliente()
        {
            try
            {
                InitializeComponent();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                InitializeThemesDropDown();
                ControlesLenguaje();
                InicializarColumnas();
                this.Show();
                this.Text = Lenguaje.traduce(this.Text);
                IniciarDatos();
                InicializarEsquemas();               
                LlamadaEventos();
                GenerarGridDevoluciones();
                AñadirLabelCantidad();
                addEliminarLayoutOpciones();
                Permiso(itmMenuEliminarEntrada, 500091);
                Permiso(itmMenuModificarEntrada, 500092);


            }
            catch(Exception ex)
            {

            }
        }

        private void InicializarColumnas()
        {
            colPedidosPorcentLineas = new ProgressBarColumn(colPorcentLineas);
            colPedidosPorcentLineas.HeaderText = colPorcentLineas;
            colPedidosPorcentLineas.Name = colPorcentLineas;
            colPedidosPorcentLineas.Width = 100;

            colJerLineasPedidoPorcentEntPend = new ProgressBarColumn(colCantidadPteUbicar);
            colJerLineasPedidoPorcentEntPend.HeaderText = Lenguaje.traduce(colCantidadPteUbicar);
            colJerLineasPedidoPorcentEntPend.Name = Lenguaje.traduce(colCantidadPteUbicar);
            colJerLineasPedidoPorcentEntPend.Width = 100;

            colJerLineasPedidosPorcentEnt = new ProgressBarColumn(colCantidadEntrada);
            colJerLineasPedidosPorcentEnt.HeaderText = Lenguaje.traduce(colCantidadEntrada);
            colJerLineasPedidosPorcentEnt.Name = Lenguaje.traduce(colCantidadEntrada);
            colJerLineasPedidosPorcentEnt.Width = 100;
        }
        private void AñadirColumnas()
        {
            this.rgvGridDevoluciones.Columns.Add(colPedidosPorcentLineas);
        }
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
            RadMenuItem mainItem = rDDButtonConfiguracion.Items[0] as RadMenuItem;
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(evtTemasItem_Click);
            mainItem.Items.Add(temasItem);
        }
        private void AñadirLabelCantidad()
        {
            lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            grupoLabel.Items.Add(lblCantidad);
            this.ribbonTabAccionesDevoluciones.Items.AddRange(grupoLabel);
        }
        private void addEliminarLayoutOpciones()
        {
            FuncionesGenerales.AddEliminarLayoutButton(ref rDDButtonConfiguracion);
            if (this.rDDButtonConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.rDDButtonConfiguracion.Items["RadMenuItemEliminarLayout"].Click += eliminarLayout;
            }
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


        #region Lenguajes
        private void ControlesLenguaje()
        {
            try
            {
                colPorcentLineas = Lenguaje.traduce("% Líneas");
                colCantidadPteUbicar = Lenguaje.traduce("Cantidad Pte Ubicar");
                colCantidadEntrada = Lenguaje.traduce("Cantidad Entrada");

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        #endregion

        private void FrmDevolucionesCliente_Shown(object sender, EventArgs e)
        {

        }
        private void IniciarDatos()
        {
            try
            {
                chkColDevoluciones.Name = "chkDevoluciones";
                chkColDevoluciones.EnableHeaderCheckBox = true;
                chkColDevoluciones.HeaderText = "";
                chkColDevoluciones.EditMode = EditMode.OnValueChange;
                chkColDevoluciones.CheckFilteredRows = false;
                

                this.radWaitDevoluciones.Name = "rWBarDevoluciones";
                this.radWaitDevoluciones.Size = new System.Drawing.Size(200, 20);
                this.radWaitDevoluciones.TabIndex = 2;
                this.radWaitDevoluciones.Text = "rWBarDevoluciones";
                this.radWaitDevoluciones.AssociatedControl = this.rgvGridDevoluciones;
                
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
                Business.GetEsquemaGenerico(nombreJson, ref _lstEsquemaTablaDevoluciones);                
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin Carga Esquemas iniciales");

            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void LlamadaEventos()
        {
            log.Debug("Principio llamadaEventos en Devolucion Clientes"); 
           
            log.Debug("Fin llamadaEventos en  Devolucion Clientes");
        }

       

        #region GenerarGrids
        private void GenerarGridDevoluciones()
        {
            try
            {
                
                Utilidades.refrescarJerarquico(ref this.rgvGridDevoluciones, -1);
                rgvGridDevoluciones.Columns.Clear();               
                this.SuspendLayout();
                bgWorkerGridViewDevoluciones = new BackgroundWorker();
                bgWorkerGridViewDevoluciones.WorkerReportsProgress = true;
                bgWorkerGridViewDevoluciones.WorkerSupportsCancellation = true;
                bgWorkerGridViewDevoluciones.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewDevoluciones_RunWorkerCompleted);
                bgWorkerGridViewDevoluciones.DoWork += new DoWorkEventHandler(llenarGridDevoluciones);
                radWaitDevoluciones.StartWaiting();
                bgWorkerGridViewDevoluciones.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void eliminarLayout(object e, EventArgs a)
        {
                try {
                    this.Name = "DevolucionesCliente";
                    FuncionesGenerales.EliminarLayout(this.Name + "GridView.xml", rgvGridDevoluciones);
            }
                catch (Exception ex)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Error al eliminar el estilo"));
                    log.Error("Error eliminando el archivo " + /*pathP +*/ "  " + ex.Message + " \n " + ex.StackTrace);
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
            
            this.Name = "DevolucionesCliente";
            string pathGridView = pathLocal + "\\" + this.Name + "GridView.xml";
            bool existsGridView = File.Exists(pathGridView);
            if (existsGridView)
            {
                LoadLayoutLocal();                                    }
            else
            {
                this.Name = "DevolucionesCliente";
                pathGridView = pathGlobal + "\\" + this.Name + "GridView.xml";
                existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    LoadLayoutGlobal();
                }
                else
                {                      
                    rgvGridDevoluciones.BestFitColumns();
                }
            }
            
        }
        public void SaveLayoutGlobal()
        {
            int indexColumnPorcentLinea;
            int indexColumnPorcentEntrada;
            int indexColumnPorcentCantPte;
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
                
                    this.Name = "DevolucionesCliente";
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    path.Replace(" ", "_");
                    indexColumnPorcentLinea = Convert.ToInt32(rgvGridDevoluciones.Columns[colPorcentLineas].Index);
                    indexColumnPorcentEntrada = Convert.ToInt32(rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns[colCantidadEntrada].Index);
                    indexColumnPorcentCantPte = Convert.ToInt32(rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns[colCantidadPteUbicar].Index);
                    XmlReaderPropio.setPorcentLineasDevolCliCab(indexColumnPorcentLinea);
                    XmlReaderPropio.setPorcentEntradasDevolCli(indexColumnPorcentEntrada);
                    XmlReaderPropio.setPorcentEntradasPendientesDevolCli(indexColumnPorcentCantPte);
                    rgvGridDevoluciones.Columns.Remove(colPedidosPorcentLineas);
                    rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Remove(colJerLineasPedidosPorcentEnt);
                    rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Remove(colJerLineasPedidoPorcentEntPend);
                    rgvGridDevoluciones.SaveLayout(path);
                    this.rgvGridDevoluciones.Columns.Add(colPedidosPorcentLineas);
                    this.rgvGridDevoluciones.Columns.Move(rgvGridDevoluciones.Columns.Count - 1, indexColumnPorcentLinea);
                     rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidosPorcentEnt);
                    if (indexColumnPorcentEntrada != rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Count)
                    {
                    rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidosPorcentEnt.Index, indexColumnPorcentEntrada);

                    }

                    rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidoPorcentEntPend);
                    if (indexColumnPorcentCantPte != rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Count)
                    {
                    rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidoPorcentEntPend.Index, indexColumnPorcentCantPte);

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
            int indexColumnPorcentLinea;
            int indexColumnPorcentEntrada;
            int indexColumnPorcentCantPte;
            string path = Persistencia.DirectorioLocal;
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
               
                this.Name = "DevolucionesCliente";
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "GridView.xml";
                path.Replace(" ", "_");
                indexColumnPorcentLinea = Convert.ToInt32(rgvGridDevoluciones.Columns[colPorcentLineas].Index);
                indexColumnPorcentEntrada = Convert.ToInt32(rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns[colCantidadEntrada].Index);
                indexColumnPorcentCantPte = Convert.ToInt32(rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns[colCantidadPteUbicar].Index);                
                XmlReaderPropio.setPorcentLineasDevolCliCab(indexColumnPorcentLinea);
                XmlReaderPropio.setPorcentEntradasDevolCli(indexColumnPorcentEntrada);
                XmlReaderPropio.setPorcentEntradasPendientesDevolCli(indexColumnPorcentCantPte);
                this.rgvGridDevoluciones.Columns.Remove(colPedidosPorcentLineas);               
                rgvGridDevoluciones.SaveLayout(path);
                this.rgvGridDevoluciones.Columns.Add(colPedidosPorcentLineas);
                this.rgvGridDevoluciones.Columns.Move(rgvGridDevoluciones.Columns.Count - 1, indexColumnPorcentLinea);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidosPorcentEnt);
                if (indexColumnPorcentEntrada != rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Count)
                {
                    rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidosPorcentEnt.Index, indexColumnPorcentEntrada);

                }

                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidoPorcentEntPend);
                if (indexColumnPorcentCantPte != rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Count)
                {
                    rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidoPorcentEntPend.Index, indexColumnPorcentCantPte);

                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));

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
               
                this.Name = "DevolucionesCliente";
                string s = path + "\\" + this.Name + "GridView.xml";
                s.Replace(" ", "_");
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Empieza cargar layout");             
                rgvGridDevoluciones.Columns.Remove(colPedidosPorcentLineas);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Remove(colJerLineasPedidosPorcentEnt);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Remove(colJerLineasPedidoPorcentEntPend);
                rgvGridDevoluciones.LoadLayout(s);
                rgvGridDevoluciones.Columns.Add(colPedidosPorcentLineas);
                rgvGridDevoluciones.Columns.Move(colPedidosPorcentLineas.Index, XmlReaderPropio.getPorcentLineasDevolCliCab());
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidosPorcentEnt);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidoPorcentEntPend);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidosPorcentEnt.Index, XmlReaderPropio.getPorcentEntradasDevolCli());
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidoPorcentEntPend.Index, XmlReaderPropio.getPorcentEntradasPendientesDevolCli());
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Finaliza cargar layout");                
                rgvGridDevoluciones.TableElement.RowHeight = 40;
                for (int i = 0; i < rgvGridDevoluciones.Templates.Count; i++)
                {
                    rgvGridDevoluciones.Templates[i].Refresh();
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
                this.Name = "OrdenesCliente";
                string s = path + "\\" + this.Name + "GridView.xml";
                //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "GridView.xml";

                s.Replace(" ", "_");

                rgvGridDevoluciones.Columns.Remove(colPedidosPorcentLineas);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Remove(colJerLineasPedidosPorcentEnt);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Remove(colJerLineasPedidoPorcentEntPend);
                rgvGridDevoluciones.LoadLayout(s);
                rgvGridDevoluciones.Columns.Add(colPedidosPorcentLineas);
                rgvGridDevoluciones.Columns.Move(colPedidosPorcentLineas.Index, XmlReaderPropio.getPorcentLineasDevolCliCab());
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidosPorcentEnt);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Add(colJerLineasPedidoPorcentEntPend);
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidosPorcentEnt.Index, XmlReaderPropio.getPorcentEntradasDevolCli());
                rgvGridDevoluciones.Templates[jerarquicoPosicionLineas].Columns.Move(colJerLineasPedidoPorcentEntPend.Index, XmlReaderPropio.getPorcentEntradasPendientesDevolCli());
                for (int i = 0; i < rgvGridDevoluciones.Templates.Count; i++)
                {
                        rgvGridDevoluciones.Templates[i].Refresh();
                }
                
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

        }



        #endregion
        #region CreateChildTemplate
        private GridViewTemplate CreateChildTemplateJerarquicoLineasDevolucion()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.LineasDevolucion);
            string jsonLineas = DataAccess.LoadJson(nombreJsonJerLineas);
            JArray jsLineas = JArray.Parse(jsonLineas);
            string jsonEsquemaLineas = jsLineas.First()["Scheme"].ToString();
            List<TableScheme> esquemaLineas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaLineas);
           

            for (int i = 0; i < esquemaLineas.Count; i++)
            {
                if (esquemaLineas[i].Etiqueta != string.Empty && esquemaLineas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaLineas[i].Etiqueta);
                    if (!esquemaLineas[i].EsVisible)
                    {
                        template.Columns[esquemaLineas[i].Etiqueta].IsVisible = false;
                    }
                }
            }            
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.Columns.Remove(colCantidadPteUbicar);
            template.Columns.Add(colJerLineasPedidoPorcentEntPend);
            template.Columns.Remove(colCantidadEntrada);
            template.Columns.Add(colJerLineasPedidosPorcentEnt);
            template.AllowAddNewRow = false;
            
            template.BestFitColumns();
            return template;
        }
        private GridViewTemplate CreateChildTemplateJerarquicoEntradas()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.Entradas);
            string jsonLineas = DataAccess.LoadJson(nombreJsonJerEntradas);
            JArray jsLineas = JArray.Parse(jsonLineas);
            string jsonEsquemaLineas = jsLineas.First()["Scheme"].ToString();
            List<TableScheme> esquemaLineas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaLineas);


            for (int i = 0; i < esquemaLineas.Count; i++)
            {
                if (esquemaLineas[i].Etiqueta != string.Empty && esquemaLineas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaLineas[i].Etiqueta);
                    if (!esquemaLineas[i].EsVisible)
                    {
                        template.Columns[esquemaLineas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.AllowAddNewRow = false;
            template.BestFitColumns();
            return template;
        }
        private GridViewTemplate CreateChildTemplateJerarquicoExistencias()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.Existencias);
            string jsonLineas = DataAccess.LoadJson(nombreJsonJerExistencias);
            JArray jsLineas = JArray.Parse(jsonLineas);
            string jsonEsquemaLineas = jsLineas.First()["Scheme"].ToString();
            List<TableScheme> esquemaLineas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaLineas);


            for (int i = 0; i < esquemaLineas.Count; i++)
            {
                if (esquemaLineas[i].Etiqueta != string.Empty && esquemaLineas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaLineas[i].Etiqueta);
                    if (!esquemaLineas[i].EsVisible)
                    {
                        template.Columns[esquemaLineas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.AllowAddNewRow = false;
            template.BestFitColumns();
            return template;
        }
        private GridViewTemplate CreateChildTemplateJerarquicoMovimientos()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.Movimientos);
            string jsonLineas = DataAccess.LoadJson(nombreJsonJerReservas);
            JArray jsLineas = JArray.Parse(jsonLineas);
            string jsonEsquemaLineas = jsLineas.First()["Scheme"].ToString();
            List<TableScheme> esquemaLineas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaLineas);


            for (int i = 0; i < esquemaLineas.Count; i++)
            {
                if (esquemaLineas[i].Etiqueta != string.Empty && esquemaLineas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaLineas[i].Etiqueta);
                    if (!esquemaLineas[i].EsVisible)
                    {
                        template.Columns[esquemaLineas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.AllowAddNewRow = false;
            template.BestFitColumns();
            return template;
        }
        private GridViewTemplate CreateChildTemplateJerarquicoTareas()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.Caption = Lenguaje.traduce(strings.Tareas);
            string jsonLineas = DataAccess.LoadJson(nombreJsonJerTareas);
            JArray jsLineas = JArray.Parse(jsonLineas);
            string jsonEsquemaLineas = jsLineas.First()["Scheme"].ToString();
            List<TableScheme> esquemaLineas = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaLineas);


            for (int i = 0; i < esquemaLineas.Count; i++)
            {
                if (esquemaLineas[i].Etiqueta != string.Empty && esquemaLineas[i].Etiqueta != null)
                {
                    template.Columns.Add(esquemaLineas[i].Etiqueta);
                    if (!esquemaLineas[i].EsVisible)
                    {
                        template.Columns[esquemaLineas[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            template.HierarchyDataProvider = new GridViewEventDataProvider(template);
            template.AllowAddNewRow = false;
            template.BestFitColumns();
            return template;
        }
        #endregion
        #region eventos
        private void evtTemasItem_Click(object sender, EventArgs e)
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
            }catch(Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void FrmDevolucionesCliente_Load(object sender, EventArgs e)
        {
            try
            {
               /* if (devConLineas == CONLINEAS)
                {*/
                    //Añadimos jerarquicos vista devoluciones
                    GridViewTemplate gtLineasDevoluciones = CreateChildTemplateJerarquicoLineasDevolucion();
                    gtLineasDevoluciones.Caption = Lenguaje.traduce(strings.LineasDevolucion);
                    this.rgvGridDevoluciones.Templates.Add(gtLineasDevoluciones);
                /*}*/

                GridViewTemplate gtEntradas = CreateChildTemplateJerarquicoEntradas();
                gtEntradas.Caption = Lenguaje.traduce(strings.Entradas);
                this.rgvGridDevoluciones.Templates.Add(gtEntradas);

                GridViewTemplate gtExistencias = CreateChildTemplateJerarquicoExistencias();
                gtExistencias.Caption = Lenguaje.traduce(strings.Existencias);
                this.rgvGridDevoluciones.Templates.Add(gtExistencias);

                GridViewTemplate gtMovimientos = CreateChildTemplateJerarquicoMovimientos();
                gtMovimientos.Caption = Lenguaje.traduce(strings.Movimientos);
                this.rgvGridDevoluciones.Templates.Add(gtMovimientos);


                GridViewTemplate gtTareas = CreateChildTemplateJerarquicoTareas();
                gtTareas.Caption = Lenguaje.traduce(strings.Tareas);
                this.rgvGridDevoluciones.Templates.Add(gtTareas);

                

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

        }
        private void rgvGridDevoluciones_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

        }

        private void rgvGridDevoluciones_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
               
                    GridDataView dataView = rgvGridDevoluciones.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;
                    filtroAnteriorDevoluciones = rgvGridDevoluciones.FilterDescriptors;
                    
                
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rgvGridDevoluciones_RowSourceNeeded(object sender, GridViewRowSourceNeededEventArgs e)
        {
            try
            {
                this.SuspendLayout();
                this.Cursor = Cursors.WaitCursor;             
                string idDevolCli = string.Empty;
                DataRowView rowView = e.ParentRow.DataBoundItem as DataRowView;
                DataRow[] rows = rowView.Row.GetChildRows("" + Lenguaje.traduce("ID") + "");
                idDevolCli = e.ParentRow.Cells["" + Lenguaje.traduce("ID") + ""].Value.ToString();
                log.Debug("Iniciando row source needed sobre el padre " + idDevolCli);


                if (e.Template.Caption == Lenguaje.traduce(strings.LineasDevolucion))
                {
                    /*if (devConLineas == CONLINEAS)
                    {*/
                        try
                        {
                            DataTable lineasPedido = Business.GetGenericoJerarquicoDatosGridView(nombreJsonJerLineas, "[IDDEVOLCLI]", idDevolCli);
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
                                                row.Cells[lineasPedido.Columns[i].ColumnName].Value = dataRow[row.Cells[lineasPedido.Columns[i].ColumnName].ColumnInfo.HeaderText];
                                            }
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        log.Error("Error en RowSourceNeeded de Lineas devoluciones, intentando insertar el campo " + row.Cells[i].ColumnInfo.HeaderText + " en la fila " + lineasPedido.Columns[0].ColumnName + " : " + dataRow[0].ToString());

                                    }
                                }
                                e.SourceCollection.Add(row);
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);

                        }
                   /* }*/

                }
                else if (e.Template.Caption == Lenguaje.traduce(strings.Entradas))
                {
                    try
                    {
                        DataTable entradasDevolucion = Business.GetGenericoJerarquicoDatosGridView(nombreJsonJerEntradas, "[IDDEVOLCLI]", idDevolCli);
                        if (entradasDevolucion.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in entradasDevolucion.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < entradasDevolucion.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[entradasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (entradasDevolucion.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[entradasDevolucion.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[entradasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[entradasDevolucion.Columns[i].ColumnName].Value = dataRow[row.Cells[entradasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    log.Error("Error en RowSourceNeeded de entradas devoluciones, intentando insertar el campo " + row.Cells[i].ColumnInfo.HeaderText + " en la fila " + entradasDevolucion.Columns[0].ColumnName + " : " + dataRow[0].ToString());

                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);

                    }
                }
                else if (e.Template.Caption == Lenguaje.traduce(strings.Existencias))
                {
                    try
                    {
                        DataTable existenciasDevolucion = Business.GetGenericoJerarquicoDatosGridView(nombreJsonJerExistencias, "[IDDEVOLCLI]", idDevolCli);
                        if (existenciasDevolucion.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in existenciasDevolucion.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < existenciasDevolucion.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[existenciasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (existenciasDevolucion.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[existenciasDevolucion.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[existenciasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[existenciasDevolucion.Columns[i].ColumnName].Value = dataRow[row.Cells[existenciasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    log.Error("Error en RowSourceNeeded de existencvias devoluciones, intentando insertar el campo " + row.Cells[i].ColumnInfo.HeaderText + " en la fila " + existenciasDevolucion.Columns[0].ColumnName + " : " + dataRow[0].ToString());

                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);

                    }
                }
                else if (e.Template.Caption == Lenguaje.traduce(strings.Movimientos))
                {
                    try
                    {
                        DataTable movimientosDevolucion = Business.GetGenericoJerarquicoDatosGridView(nombreJsonJerReservas, "[IDDEVOLCLI]", idDevolCli);
                        if (movimientosDevolucion.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in movimientosDevolucion.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < movimientosDevolucion.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[movimientosDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (movimientosDevolucion.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[movimientosDevolucion.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[movimientosDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[movimientosDevolucion.Columns[i].ColumnName].Value = dataRow[row.Cells[movimientosDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    log.Error("Error en RowSourceNeeded de reservas devoluciones, intentando insertar el campo " + row.Cells[i].ColumnInfo.HeaderText + " en la fila " + movimientosDevolucion.Columns[0].ColumnName + " : " + dataRow[0].ToString());

                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);

                    }
                }
                else if (e.Template.Caption == Lenguaje.traduce(strings.Tareas))
                {
                    try
                    {
                        DataTable tareasDevolucion = Business.GetGenericoJerarquicoDatosGridView(nombreJsonJerTareas, "[IDDEVOLCLI]", idDevolCli);
                        if (tareasDevolucion.Rows.Count == 0)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            e.SourceCollection.Add(row);
                        }
                        foreach (DataRow dataRow in tareasDevolucion.Rows)
                        {
                            GridViewRowInfo row = e.Template.Rows.NewRow();
                            for (int i = 0; i < tareasDevolucion.Columns.Count; i++)
                            {
                                try
                                {
                                    if (row.Cells[tareasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText != null)
                                    {
                                        if (tareasDevolucion.Columns[i].DataType == typeof(Decimal))
                                        {
                                            row.Cells[tareasDevolucion.Columns[i].ColumnName].Value = Decimal.Parse(dataRow[row.Cells[tareasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString()).ToString("G29");
                                        }
                                        else
                                        {
                                            row.Cells[tareasDevolucion.Columns[i].ColumnName].Value = dataRow[row.Cells[tareasDevolucion.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    log.Error("Error en RowSourceNeeded de tareas devoluciones, intentando insertar el campo " + row.Cells[i].ColumnInfo.HeaderText + " en la fila " + tareasDevolucion.Columns[0].ColumnName + " : " + dataRow[0].ToString());

                                }
                            }
                            e.SourceCollection.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            log.Info("Terminando RowSourceNeeded");
            e.Template.BestFitColumns();
            this.ResumeLayout();
            this.Cursor = Cursors.Arrow;
        }
         private   void bgWorkerGridViewDevoluciones_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio bgWorkerGridViewDevoluciones_RunWorkerCompleted");
                DataTable tablaVacia = null;
                tablaVacia = dtDevoluciones.Copy();
               
                tablaVacia.Rows.Clear();
               
                rgvGridDevoluciones.DataSource = tablaVacia;
                if (dtDevoluciones.Rows.Count == 0)
                {
                    radWaitDevoluciones.StopWaiting();
                }
                else

                {
                    rgvGridDevoluciones.Refresh();
                    rgvGridDevoluciones.Columns.Clear();
                    rgvGridDevoluciones.DataSource = dtDevoluciones;//CAMBIO tablaLlena;  
                    if (rgvGridDevoluciones.Columns[0] != chkColDevoluciones)
                    {
                        rgvGridDevoluciones.Columns.Add(chkColDevoluciones);
                        rgvGridDevoluciones.Columns.Move(rgvGridDevoluciones.Columns.Count - 1, 0);
                    }
                    rgvGridDevoluciones.Refresh();
                    rgvGridDevoluciones.Columns.Remove(rgvGridDevoluciones.Columns["RowNum"]);                  
                    SetPreferencesDevoluciones();
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



                    GridDataView dataView = rgvGridDevoluciones.MasterTemplate.DataView as GridDataView;
                    lblCantidad.Text = Lenguaje.traduce("Registros:") + dataView.Indexer.Items.Count;                  
                    colorearGridPedidos();
                    OcultarCampos();
                    foreach (FilterDescriptor filtro in filtroAnteriorDevoluciones)
                    {
                        rgvGridDevoluciones.FilterDescriptors.Add(filtro);
                    }

                    radWaitDevoluciones.StopWaiting();
                }
                rgvGridDevoluciones.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                SetPreferencesDevoluciones();
                //setFiltros1();
                radWaitDevoluciones.StopWaiting();
            }
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio bgWorkerGridViewPedidos_RunWorkerCompleted");
            this.ResumeLayout();
        }

        private void OcultarCampos()
        {
            try
            {
                    for (int i = 0; i < _lstEsquemaTablaDevoluciones.Count; i++)
                    {
                       
                            if (!_lstEsquemaTablaDevoluciones[i].EsVisible)
                            {
                                rgvGridDevoluciones.Columns[_lstEsquemaTablaDevoluciones[i].Etiqueta].IsVisible = false;
                            }
                       
                    }
            
            }catch(Exception ex)
            {

            }
        }

        private void colorearGridPedidos()
        {
            if (rgvGridDevoluciones.Rows.Count < 1) return;
            foreach (GridViewRowInfo rowInfo in rgvGridDevoluciones.Rows)
            {
                colorEstado(rowInfo);
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
                   
                }
                else
                {
                    row.Cells["" + Lenguaje.traduce("Estado Descripción") + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Estado Descripción") + ""].Style.BackColor = Color.FromArgb(valor);
                    row.Cells["" + Lenguaje.traduce("Estado Devolución") + ""].Style.CustomizeFill = true;
                    row.Cells["" + Lenguaje.traduce("Estado Devolución") + ""].Style.BackColor = Color.FromArgb(valor);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void SetPreferencesDevoluciones()
        {
            try
            {
                Utilidades.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvGridDevoluciones);
                rgvGridDevoluciones.MultiSelect = true;
                rgvGridDevoluciones.MasterTemplate.EnableGrouping = true;
                rgvGridDevoluciones.ShowGroupPanel = true;
                rgvGridDevoluciones.MasterTemplate.AutoExpandGroups = true;
                rgvGridDevoluciones.EnableHotTracking = true;
                rgvGridDevoluciones.MasterTemplate.AllowAddNewRow = false;
                rgvGridDevoluciones.MasterTemplate.AllowColumnResize = true;
                rgvGridDevoluciones.MasterTemplate.AllowMultiColumnSorting = true;
                rgvGridDevoluciones.AllowSearchRow = true;
                rgvGridDevoluciones.EnablePaging = false;
                rgvGridDevoluciones.TableElement.RowHeight = 40;
                rgvGridDevoluciones.AllowRowResize = false;
                rgvGridDevoluciones.MasterView.TableSearchRow.SearchDelay = 2000;
                rgvGridDevoluciones.MasterTemplate.EnableFiltering = true;                

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void llenarGridDevoluciones(object sender, DoWorkEventArgs e)
        {
            try
            {
                dtDevoluciones = null;
             
                Business.GetEsquemaGenerico(nombreJson,ref _lstEsquemaTablaDevoluciones);
                dtDevoluciones = Business.GetDatosGridView(nombreJson, "DC.IDDEVOLCLI",_lstEsquemaTablaDevoluciones);               
                lblCantidad.Text = Lenguaje.traduce("Registros:") + dtDevoluciones.Rows.Count;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        #endregion

        private void itm_CargarMenu_Click(object sender, EventArgs e)
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

        private void itm_EditarColumnas_Click(object sender, EventArgs e)
        {

            try
            {
                this.rgvGridDevoluciones.ShowColumnChooser();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void itm_TemasMenu_Click(object sender, EventArgs e)
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

        private void itm_GuardarMenu_Click(object sender, EventArgs e)
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

        private void rBtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                
                log.Info("Pulsado botón exportar " + DateTime.Now);
                string confirmacion = Lenguaje.traduce(strings.ExportacionExito);
                GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(rgvGridDevoluciones);
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
            }catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rBtnFiltrar_Click(object sender, EventArgs e)
        {
            if (rgvGridDevoluciones.Rows.Count > 0)
            {
                try
                {

                    radDataFilterDevoluciones.DataSource = rgvGridDevoluciones.DataSource;
                    if (radDataFilterDevoluciones.Visible == false)
                    {
                        radDataFilterDevoluciones.ShowDialog();
                    }
                    else
                    {
                        radDataFilterDevoluciones.Close();
                        radDataFilterDevoluciones.DataFilter.ApplyFilter();
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

        private void rBtnQuitarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón quitar filtro " + DateTime.Now);
                if (rgvGridDevoluciones.IsInEditMode)
                {
                    rgvGridDevoluciones.EndEdit();
                }
                rgvGridDevoluciones.FilterDescriptors.Clear();
                rgvGridDevoluciones.GroupDescriptors.Clear();
                radDataFilterDevoluciones.DataFilter.Expression = "";
                radDataFilterDevoluciones.DataFilter.ApplyFilter();

                rgvGridDevoluciones.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rBtnRefrescar_Click(object sender, EventArgs e)
        {

            refrescar();
        }
        private void refrescar()
        {
            try
            {
                
                GenerarGridDevoluciones();
                

            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
           
        }
        private void AñadirValorProgressBar()
        {
            try
            {
                int value = 0;

                int i = 0;
                rgvGridDevoluciones.Columns[Lenguaje.traduce("Porcentaje Lineas")].IsVisible = false;
                rgvGridDevoluciones.Columns[Lenguaje.traduce("Porcentaje Lineas")].VisibleInColumnChooser = false;
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio  añadir valor progress bar");
                foreach (GridViewRowInfo row in rgvGridDevoluciones.Rows)
                {
                    
                    int.TryParse(row.Cells[Lenguaje.traduce("Porcentaje Lineas")].Value.ToString(), out value);
                    row.Cells[colPorcentLineas].Value = value;

                    
                    i++;
                }
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Fin  añadir valor progress bar");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void rBtnGenerarTarea_Click(object sender, EventArgs e)
        {
            try
            {
                if (!comprobarNoCerrada())
                {
                    return;
                }
                ElegirMuelle frm = new ElegirMuelle();
                frm.ShowDialog();
                int idMuelle = frm.idMuelle;

                if (idMuelle <= 0)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar muelle"));                    
                    return;
                }
                string json = formarJSONWSDevolucion(idMuelle);
                if (json != null)
                {
                    string resp = llamarWSGenerarTareaDevolucion(json);
                    var jss = new JavaScriptSerializer();
                    dynamic d = jss.DeserializeObject(resp);
                    var error = d[0]["Error"];
                    if (error == string.Empty)
                    {
                        MessageBox.Show(Lenguaje.traduce("Tarea generada correctamente."));
                        rgvGridDevoluciones.DataSource = null;
                        refrescar();
                    }
                    else
                    {
                        MessageBox.Show(error);
                    }
                }
            }catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }
        private string llamarWSGenerarTareaDevolucion(string json)
        {
            WSDevolucionesMotorClient ws = new WSDevolucionesMotorClient();
            string respuesta = ws.generarTareaDevolucion(json);
            log.Debug(respuesta);
            return respuesta;
        }
        private string formarJSONWSDevolucion(int idMuelle)
        {
            
            dynamic objetoDinamico = new ExpandoObject();
            int idDevolucion = getIdSeleccionada();
            if (idDevolucion > 0)
            {
                objetoDinamico.idDevolucion = idDevolucion;
                objetoDinamico.idMuelle = idMuelle;
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
        private bool comprobarNoCerrada()
        {
            int _id = getIdSeleccionada();
            List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
            ObtenerChildRowRecursivo(rgvGridDevoluciones.ChildRows, listaRecursiva);
            foreach (GridViewRowInfo row in listaRecursiva)
            {
               
                if (row.Cells["" + Lenguaje.traduce("ID") + ""].Value == null) break;
                if (row.Cells["" + Lenguaje.traduce("ID") + ""].Value.ToString().Equals(_id.ToString()))
                {

                    if (row.Cells[Lenguaje.traduce("Estado Devolución")] != null &&
                    row.Cells[Lenguaje.traduce("Estado Devolución")].Value != null &&
                    !row.Cells[Lenguaje.traduce("Estado Devolución")].Value.ToString().Equals("PC"))
                    {
                        return true;
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("La devolución seleccionada está cerrada."));
                        log.Debug("Se ha intentado modificar una devolución ya cerrada.(" + _id + ")");
                        return false;
                    }

                }
            }
            return false;

        }
        private int getIdSeleccionada()
        {
            return getIdSeleccionada(-1);
        }
        private int getIdSeleccionada(int quitarSeleccionadaAnterior)
        {
            try
            {

                if (rgvGridDevoluciones == null)
                {
                    log.Error("Error al identificar la devolucion seleccionada, el rgvGridDevoluciones vale null");
                }

                if (rgvGridDevoluciones.Columns.Count < 1) return -1;
                //En cierto caso hay un momento en el que llega aquí y aunque SÍ existe el checkbox
                //no tiene el nombre correcto.
                int posicionCheckBox = -1;
                if (rgvGridDevoluciones.Columns["chkDevoluciones"] == null)
                {
                    posicionCheckBox = Utilidades.buscarDondeEstaCheckBox(ref rgvGridDevoluciones);
                    if (posicionCheckBox > -1)
                    {
                        rgvGridDevoluciones.Columns[posicionCheckBox].Name = "chkDevoluciones";
                    }
                    else
                    {
                        log.Error("No hay checkBox en rgvGridDevoluciones en su paso por getIdSeleccionada(" +
                            quitarSeleccionadaAnterior + ")");
                        return -1;
                    }

                }


                for (int i = 0; i < rgvGridDevoluciones.RowCount; i++)
                {

                    if (rgvGridDevoluciones.Rows[i].Cells["chkDevoluciones"] == null)
                    {
                        break;
                    }
                    if (rgvGridDevoluciones.Rows[i].Cells["chkDevoluciones"].Value == null)
                    {
                        FuncionesGenerales.setCheckBoxFalse(ref rgvGridDevoluciones, "chkDevoluciones");
                        break;
                    }
                    if (rgvGridDevoluciones.Rows[i].Cells["chkDevoluciones"].Value.ToString().Equals("True"))
                    {
                        int id = int.Parse(rgvGridDevoluciones.Rows[i].Cells[Lenguaje.traduce("ID")].Value.ToString());
                        if (quitarSeleccionadaAnterior > 0 && quitarSeleccionadaAnterior == id)
                        {
                            continue;
                        }
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error producido al intentar coger la dev actual en Devoluciones.\n" + ex.Message);
            }
            return -1;
        }
        private void rBtnAlta_Click(object sender, EventArgs e)
        {

            try
            {


                string operario = string.Empty;
                string tipoPalet = string.Empty;

                try
                {
                    if (!comprobarNoCerrada())
                    {
                        return;
                    }

                    int _id = getIdSeleccionada();
                    if (_id> 0)
                        {
                        bool devConLineas=true;
                        if ((idDevolCliLinSeleccionado <= 0) )
                        {
                            String mensaje = Lenguaje.traduce("No se ha seleccionado una linea de devolución. ¿Desea hacer una entrada de un producto no previsto en la devolución?");
                            if (RadMessageBox.Show(this, mensaje, Lenguaje.traduce("Confirmación"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            {
                                RadMessageBox.Show(Lenguaje.traduce("Cancelando"));
                                return;
                            }
                            else
                            {
                                devConLineas = false;
                            }

                        }
                        if ((devConLineas && idDevolCliLinSeleccionado > 0) || !devConLineas) { 
                            String nombreBotonPulsado = (sender as RadButtonElement).Name;
                            Image icono = (sender as RadButtonElement).Image;
                            String nombreFormularioOpcion = (sender as RadButtonElement).Text;
                            int opcion = 0;
                            switch (nombreBotonPulsado)
                            {
                                case "rBtnAltaPalet":
                                    opcion = FormularioAltaProductoDevolucion.AltaPalet;
                                    break;
                                case "rBtnAltaPico":
                                    opcion = FormularioAltaProductoDevolucion.AltaPaletPicos;
                                    break;
                                case "rBtnAltaMulti":
                                    opcion = FormularioAltaProductoDevolucion.AltaPaletMultireferencia;
                                    break;
                                case "rBtnAltaCarro":
                                    opcion = FormularioAltaProductoDevolucion.AltaPaletCarroMovil;
                                    break;
                                case "rBtnAltaTotales":
                                    opcion = FormularioAltaProductoDevolucion.AltaPaletEntradasTotales;
                                    break;
                            }
                            
                                FormularioAltaProductoDevolucion fap =
                                        new FormularioAltaProductoDevolucion(opcion, _id, idDevolCliLinSeleccionado, nombreFormularioOpcion, icono, devConLineas);
                                fap.ShowDialog();
                            
                            refrescarTemplatesDevoluciones();
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

        private void rgvGridDevoluciones_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.Index > -1 && e.Row.HierarchyLevel == 1)
            {
                //first level template
                GridViewTemplate template = e.Row.ViewTemplate;               
                if (template.Caption == Lenguaje.traduce(strings.LineasDevolucion))
                {
                    GridViewRowInfo fila = e.Row;
                    try
                    {                        
                        idDevolCliLinSeleccionado = Convert.ToInt32(fila.Cells[Lenguaje.traduce("Línea")].Value);
                    }
                    catch (Exception ex)
                    {
                        log.Info("rgvGridDevoluciones_CellClick no ha encontrado ID Pedido o ID Pedido  Lin");
                       
                        idDevolCliLinSeleccionado = 0;
                    }
                }
                else
                {
                    
                    idDevolCliLinSeleccionado = 0;
                }
            }
            else
            {

                idDevolCliLinSeleccionado = 0;
            }
        }
        public void refrescarTemplatesDevoluciones()
        {
            foreach (GridViewTemplate item in rgvGridDevoluciones.Templates)
            {
                item.Refresh();
            }
        }

        private void rBtnCerrarDevolucion_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<int> listaDevoluciones = new List<int>();
                List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                ObtenerChildRowRecursivo(rgvGridDevoluciones.ChildRows, listaRecursiva);
                //En cierto caso hay un momento en el que llega aquí y aunque SÍ existe el checkbox
                //no tiene el nombre correcto.
                int posicionCheckBox = -1;
                if (rgvGridDevoluciones.Columns["chkDevoluciones"] == null)
                {
                    posicionCheckBox = Utilidades.buscarDondeEstaCheckBox(ref rgvGridDevoluciones);
                    if (posicionCheckBox > -1)
                    {
                        rgvGridDevoluciones.Columns[posicionCheckBox].Name = "chkDevoluciones";
                    }
                    else
                    {
                        log.Error("No hay checkBox en rgvGridDevoluciones ");
                        
                    }

                }
                foreach (GridViewRowInfo row in listaRecursiva)
                {
                    if (Convert.ToBoolean(row.Cells["chkDevoluciones"].Value) == true)
                    {
                        listaDevoluciones.Add(int.Parse(row.Cells[Lenguaje.traduce("ID")].Value.ToString()));
                    }
                }
                if (listaDevoluciones.Count < 1)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Selecciona al menos una línea"), "Error");
                    return;
                }
                foreach (var id in listaDevoluciones)
                {
                    int idOperario = User.IdOperario;
                    int idRecurso = 0; 
                    int idDevolucion = id;

                    cerrarDevolucion(idDevolucion, idOperario, idRecurso);
                }

                RadMessageBox.Show(Lenguaje.traduce("Devolución cerrada correctamente!"));
                refrescar();

            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void jerarquicoTemplateActualizar(int templateAActualizar)
        {
            this.rgvGridDevoluciones.MasterTemplate.Templates[templateAActualizar].Refresh();

        }

        private void cerrarDevolucion(int idDevolCli, int idOperario, int idRecurso)
        {
            Cursor.Current = Cursors.WaitCursor;
            WSDevolucionesMotorClient ws = null;
            try
            {
                ws = new WSDevolucionesMotorClient();
                ws.cerrarDevolucion(idDevolCli, DatosThread.getInstancia().getArrayDatos(), idOperario, idRecurso);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
            }
            Cursor.Current = Cursors.Default;
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

        private void itmMenuEliminarEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                bool devolucionSeleccionada = false;
                List<int> entradas = new List<int>();
                foreach (GridViewRowInfo rowinfo in rgvGridDevoluciones.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        if (rowinfo.Cells[Lenguaje.traduce("Estado Devolución")].Value.ToString().Equals("PC"))
                        {
                            RadMessageBox.Show(Lenguaje.traduce("La devolución está cerrada"));
                            return;
                        }
                        devolucionSeleccionada = true;
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
                                    int idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Entrada")].Value);
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
                if (devolucionSeleccionada)
                {
                    if (entradas.Count > 0)
                    {
                        WSDevolucionesMotorClient wsr = new WSDevolucionesMotorClient();
                        wsr.eliminarEntradas(entradas.ToArray(), User.IdOperario);
                        refrescarTemplatesDevoluciones();

                    }
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce("Debe seleccionar la devolución"));

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void itmMenuModificarEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                bool devolucionSeleccionada = false;
                int idEntrada = -1;
                foreach (GridViewRowInfo rowinfo in rgvGridDevoluciones.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[0].Value) == true)
                    {
                        if (rowinfo.Cells[Lenguaje.traduce("Estado Devolución")].Value.ToString().Equals("PC"))
                        {
                            RadMessageBox.Show(Lenguaje.traduce("La devolución está cerrada"));
                            return;
                        }
                        devolucionSeleccionada = true;
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
                                    idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Entrada")].Value);
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
                if (devolucionSeleccionada)
                {
                    if (idEntrada > 0)
                    {
                        ModificarEntrada frm = new ModificarEntrada(idEntrada);
                        frm.ShowDialog();
                        refrescarTemplatesDevoluciones();
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
                            this.radProgressBarElement.Value1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Value),0));
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
    }
}

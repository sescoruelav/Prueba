using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Herramientas.Stock;
using RumboSGA.Presentation.Herramientas.Stock;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGA.Properties;
using RumboSGA.TareasMotor;
using RumboSGA.ArticulosMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
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
using Telerik.WinControls.Layouts;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class ArticulosFrm : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        public int idFormulario;

        private int _cantidadRegistros = 0;
        private WSTareasPendientesMotorClient tareasMotorClient = new WSTareasPendientesMotorClient();
        private DataTable table = new DataTable();
        private Dictionary<string, string> tipos = new Dictionary<string, string>();
        private string connectionString = ConexionSQL.getConnectionString();
        private SqlConnection conn = new SqlConnection();
        private GridViewTemplate template = new GridViewTemplate();
        private RadWaitingBar radWait = new RadWaitingBar();

        private RadRibbonBarButtonGroup grupoLabel = new RadRibbonBarButtonGroup();
        public static RadLabelElement lblCantidad = new RadLabelElement();
        private RadLabelElement lblModoVista = new RadLabelElement();

        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        public ArticulosControl ArticulosGrid { get; private set; }

        public enum modoForm
        {
            lectura,
            nuevo,
            edicion,
            clonar
        }

        public modoForm ModoAperturaForm
        { get { return _modoAperturaForm; } set { _modoAperturaForm = value; } }

        private modoForm _modoAperturaForm;

        public GridScheme _esquemGrid { get; set; }

        protected dynamic _selectedRow;
        protected GridScheme _esquemaGrid;
        public string name = "Tareas";
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        protected List<TableScheme> _lstEsquemaTabla = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaJerarquico = new List<TableScheme>();

        #endregion Variables

        #region Constructor

        public ArticulosFrm()
        {
            try
            {
                InitializeComponent();
                //TODO: No se utilizan conexiones en las pantallas
                this.Name = name;

                //Asigna la cantidad de registros
                CantidadRegistros = Business.GetArticulosCantidad(ref _lstEsquemaTabla);
                lblCantidad.Text = Lenguaje.traduce(strings.Articulos) + ": " + CantidadRegistros;
                ControlesLenguaje();

                saveButton.Visibility = ElementVisibility.Collapsed;
                cancelarButton.Visibility = ElementVisibility.Collapsed;

                this.btnEstadistica.Enabled = false;
                this.btnCodigoBarras.Enabled = false;
                this.Shown += form_Shown;

                ArticulosGrid = new ArticulosControl();

                tableLayoutPanel2.Controls.Add(ArticulosGrid);
                ArticulosGrid.Dock = DockStyle.Fill;
                this.Show();
                AñadirLabelCantidad();
                CambiarLabelVistas();
                InitializeThemesDropDown();
                InicializarComboPaginacion();

                //Suscripción a eventos

                //Muestra el formulario

                //ArticulosGrid.RefrescarGrid();

                //ArticulosGrid.FilaPorDefecto();
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
            //TODO: LLamar a traducción general
            name = Lenguaje.traduce(strings.Articulos);
            this.Text = Lenguaje.traduce(NombresFormularios.Articulos);
            nuevoButton.Text = strings.Nuevo;
            editArticulo.Text = strings.Editar;
            editColumns.Text = strings.Columnas;
            guardarButton.Text = strings.GuardarEstilo;
            cargarButton.Text = strings.CargarEstilo;
            temasMenuItem.Text = strings.Temas;
            lblCantidad.Text = Lenguaje.traduce(strings.Articulos) + ": " + CantidadRegistros;
            ribbonTab1.Text = Lenguaje.traduce(strings.Acciones);
            radRibbonBarGroup1.Text = Lenguaje.traduce(strings.Modificaciones);
            radRibbonBarGroup2.Text = Lenguaje.traduce(strings.Ver);
            radRibbonBarGroup4.Text = Lenguaje.traduce(strings.Acciones);
            radRibbonBarGroup3.Text = "";

            vistaBarGroup.Text = strings.Ver;
            pagHeaderItem.Text = strings.RegistrosPaginacion;

            FuncionesGenerales.RumDropDownAddManual(ref configButton, 20004);
            FuncionesGenerales.AddEliminarLayoutButton(ref configButton);
            if (this.btnConfigurar.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.btnConfigurar.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                {
                    if (ArticulosGrid.TblLayout.Controls[0] is RadGridView)
                    {
                        FuncionesGenerales.EliminarLayout(ArticulosGrid.name + "GridView.xml", ArticulosGrid.GridView);
                    }
                    else
                    {
                        FuncionesGenerales.EliminarLayout(ArticulosGrid.name + "VirtualGrid.xml", null);
                        ArticulosGrid.virtualGrid.Refresh();
                    }
                });
            }
        }

        private void AñadirLabelCantidad()
        {
            lblCantidad.Text = Lenguaje.traduce(strings.Articulos) + ": " + CantidadRegistros.ToString();
            lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblCantidad.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            grupoLabel.Items.Add(lblCantidad);
            grupoLabel.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.grupoLabel.Padding = new System.Windows.Forms.Padding(5);
            this.ribbonTab1.Items.AddRange(grupoLabel);
        }

        #region Eventos Botones

        public void NuevoButton_Click(object sender, EventArgs e)
        {
            ArticulosGrid.NuevoArticulo();
        }

        public void ClonarButton_Click(object sender, EventArgs e)
        {
            ArticulosGrid.ClonarArticulo();
        }

        public void EliminarButton_Click(object sender, EventArgs e)
        {
            ArticulosGrid.EliminarArticulo();
        }

        public void RefreshButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ArticulosGrid.RefreshData(0);
                ArticulosGrid.RefrescarGrid();
            }
            catch (Exception ex)
            {
                log.Error("Error recargando datos de articulos. RefreshButton_Click");
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public void EditButton_Click(object sender, EventArgs e)
        {
            editArticulo.Visibility = ElementVisibility.Collapsed;
            nuevoButton.Visibility = ElementVisibility.Collapsed;
            cleanButton.Visibility = ElementVisibility.Collapsed;
            clonarButton.Visibility = ElementVisibility.Collapsed;
            saveButton.Visibility = ElementVisibility.Visible;
            cancelarButton.Visibility = ElementVisibility.Visible;

            this.ArticulosGrid = ArticulosGrid;
            if (ArticulosGrid != null)
            {
                ArticulosGrid.CambiarModo();
            }
        }

        public static void ActivarEditar()
        {
            editArticulo.Enabled = true;
            cleanButton.Enabled = true;
            clonarButton.Enabled = true;
        }

        public void CambiarLabelVistas()
        {
            switch (ArticulosGrid.estado)
            {
                case ArticulosControl.Estados.VistaRapida:
                    btnCambiarVista.Text = strings.CambiarVistaVirtual;
                    lblModoVista.Text = "";
                    grupoLabel.Items.Add(lblModoVista);
                    btnExportar.Enabled = true;
                    break;

                case ArticulosControl.Estados.VCHibrido:
                    btnCambiarVista.Text = strings.CambiarVistaVirtual;
                    lblModoVista.Text = Lenguaje.traduce("Modo Híbrido"); //Necesita traducción
                    lblModoVista.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    lblModoVista.Alignment = System.Drawing.ContentAlignment.BottomCenter;
                    grupoLabel.Items.Add(lblModoVista);
                    btnExportar.Enabled = false;
                    break;

                case ArticulosControl.Estados.VCTodoGrid:
                    btnCambiarVista.Text = strings.CambiarVistaVirtual;
                    lblModoVista.Text = Lenguaje.traduce("Modo Grid"); //Necesita traducción
                    lblModoVista.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    lblModoVista.Alignment = System.Drawing.ContentAlignment.BottomCenter;
                    btnExportar.Enabled = false;
                    break;

                case ArticulosControl.Estados.VCTodoDetalle:
                    btnCambiarVista.Text = strings.CambiarVistaGridView;
                    lblModoVista.Text = Lenguaje.traduce("Modo Detalle"); //Necesita traducción
                    lblModoVista.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    lblModoVista.Alignment = System.Drawing.ContentAlignment.BottomCenter;
                    btnExportar.Enabled = false;
                    break;

                default:
                    break;
            }
        }

        public void CambiarVista_Click(object sender, EventArgs e)
        {
            if (ArticulosGrid.numRegistrosFiltrados > 100000)
            {
                MessageBox.Show(Lenguaje.traduce(strings.ExcesoRegistros));
            }
            else
            {
                if (ArticulosGrid.numRegistrosFiltrados > 10000)
                {
                    DialogResult result = MessageBox.Show(Lenguaje.traduce("Aviso:" + strings.AvisoRegistros) + "\n" + Lenguaje.traduce("¿Estas seguro?"), strings.Confirmar, MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        ArticulosGrid.CambiarEstado();
                        CambiarLabelVistas();
                    }
                }
                else
                {
                    ArticulosGrid.CambiarEstado();
                    CambiarLabelVistas();
                }
            }
        }

        public void CambiarEstado_Click(object sender, EventArgs e)
        {
            this.ArticulosGrid = ArticulosGrid;
            if (ArticulosGrid != null)
            {
                ArticulosGrid.CambiarEstado();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.OK;
            editArticulo.Visibility = ElementVisibility.Visible;
            nuevoButton.Visibility = ElementVisibility.Visible;
            cleanButton.Visibility = ElementVisibility.Visible;
            clonarButton.Visibility = ElementVisibility.Visible;
            saveButton.Visibility = ElementVisibility.Collapsed;
            cancelarButton.Visibility = ElementVisibility.Collapsed;
            ArticulosGrid.AceptarFormulario();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            //form.DialogResult = DialogResult.Cancel;
            editArticulo.Visibility = ElementVisibility.Visible;
            nuevoButton.Visibility = ElementVisibility.Visible;
            cleanButton.Visibility = ElementVisibility.Visible;
            clonarButton.Visibility = ElementVisibility.Visible;
            saveButton.Visibility = ElementVisibility.Collapsed;
            cancelarButton.Visibility = ElementVisibility.Collapsed;
            ArticulosGrid.CancelarFormulario();
        }

        private void btnExportacion_Click(object sender, EventArgs e)
        {
            ArticulosGrid.ExportarExcel();
        }

        private void LoadItem_Click(object sender, EventArgs e)
        {
            ArticulosGrid.menuItemCargar_Click();
        }

        private void SaveItem_Click(object sender, EventArgs e)
        {
            ArticulosGrid.menuItemGuardar_Click();
        }

        private void ItemColumnas_Click(object sender, EventArgs e)
        {
            ArticulosGrid.Columnas_Click();
        }

        private void QuitarFiltros_Click(object sender, EventArgs e)
        {
            ArticulosGrid.QuitarFiltros_Click();
        }

        private void btnHistArticulos_Event(object sender, EventArgs e)
        {
            HistArticulosFecha formularioFecha = new HistArticulosFecha();
            DateTime fecha;
            int articulo;
            if (formularioFecha.ShowDialog() == DialogResult.OK)
            {
                fecha = formularioFecha.fecha;
                articulo = formularioFecha.idArticulo;
                List<GridViewRowInfo> rows = new List<GridViewRowInfo>();
                try
                {
                    string respJson = WebServiceHistorialArticulos(articulo, fecha);
                    GridHistArticulos gridHistArticulos = new GridHistArticulos(respJson);
                    gridHistArticulos.ShowDialog();
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                }
            }
        }

        private void btnConfigurarArticulos_Event(object sender, EventArgs e)
        {
            ArticulosGrid.ConfigurarArticulos();
        }

        public string WebServiceHistorialArticulos(int id, DateTime fecha)
        {
            WSArticulosClient wsArticulos = new WSArticulosClient();

            string fechaFormateada = fecha.ToString("dd/MM/yyyy");
            string j = formarJSONHistArticulos(id, fechaFormateada);
            log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada al WebService 'WSArticulosClient.getDatosHistoricoArticulo' en Stock con parametros" + j);

            string json = wsArticulos.getDatosHistoricoArticulo(j);
            log.Debug("Terminada llamada al WebService.Respuesta:" + json);
            return json;
        }

        private string formarJSONHistArticulos(int idArticulo, string fecha)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.id = idArticulo;
            objDinamico.fecha = fecha;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        #endregion Eventos Botones

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
            RadMenuItem mainItem = temasMenuItem;/*.Items[0] as RadMenuItem*/
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(temasItem_Click);
            mainItem.Items.Add(temasItem);
        }

        /* private void inicializarComboTamañofuente()
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
         }*/

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

        public void comboPaginacion_Changed(object sender, EventArgs e)
        {
            try
            {
                int tamaño = int.Parse(menuComboItem.ComboBoxElement.SelectedItem.Text);
                ArticulosGrid.virtualGrid.PageSize = tamaño;
                ArticulosGrid.GridView.PageSize = tamaño;
                XmlReaderPropio.setPaginacion(tamaño);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Temas
    }
}

#endregion Constructor
using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Herramientas.Stock;
using RumboSGA.Presentation.Herramientas.Stock;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGA.Properties;
using RumboSGA.TareasMotor;
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
    public partial class OperariosForm : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        public int idFormulario;

        private int _cantidadRegistros = 0;
        private DataTable table = new DataTable();
        private Dictionary<string, string> tipos = new Dictionary<string, string>();
        private string connectionString = ConexionSQL.getConnectionString();
        private SqlConnection conn = new SqlConnection();
        private GridViewTemplate template = new GridViewTemplate();
        private RadWaitingBar radWait = new RadWaitingBar();

        private RadRibbonBarButtonGroup grupoLabel = new RadRibbonBarButtonGroup();
        private RadLabelElement lblCantidad = new RadLabelElement();
        private string nombreEstiloGridView = "OperariosFormGridView.xml";
        private string nombreEstiloVirtualGrid = "OperariosFormVirtualGridView.xml";

        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        public OperariosControl OperariosGrid { get; private set; }

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
        public string name = Lenguaje.traduce("Operarios");
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        protected List<TableScheme> _lstEsquemaTabla = new List<TableScheme>();

        #endregion Variables

        #region Constructor

        public OperariosForm()
        {
            try
            {
                InitializeComponent();
                this.Text = Lenguaje.traduce(NombresFormularios.Operarios);
                this.Name = name;

                ControlesLenguaje();

                this.Shown += form_Shown;

                OperariosGrid = new OperariosControl();

                tableLayoutPanel2.Controls.Add(OperariosGrid);
                OperariosGrid.Dock = DockStyle.Fill;
                ElegirEstilo();
                this.Show();

                FuncionesGenerales.AddEliminarLayoutButton(ref configButton);
                if (this.configButton.Items["RadMenuItemEliminarLayout"] != null)
                {
                    this.configButton.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
                        {
                            FuncionesGenerales.EliminarLayout(nombreEstiloGridView, OperariosGrid.GridView);
                        }
                        else if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadVirtualGrid)
                        {
                            FuncionesGenerales.EliminarLayout(nombreEstiloGridView, null);
                            OperariosGrid.virtualGrid.Refresh();
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

            lblCantidad.Text = Lenguaje.traduce(strings.Articulos) + ": " + CantidadRegistros;
            ribbonTab1.Text = Lenguaje.traduce(strings.Acciones);
            this.radRibbonBarGroup1.Text = Lenguaje.traduce(strings.Operario);
            this.radRibbonBarGroup2.Text = Lenguaje.traduce(strings.Modificaciones);
            this.radRibbonBarGroup3.Text = Lenguaje.traduce(strings.Ver);

            vistaBarGroup.Text = strings.Ver;
        }

        private void btnAgregarGrupo_Click(object sender, EventArgs e)
        {
            OperariosGrid.AgregarGrupo();
        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            OperariosGrid.AgregarUsuario();
        }

        private void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            OperariosGrid.CambiarPassword();
        }

        private void btnEditarGrupo_Click(object sender, EventArgs e)
        {
            OperariosGrid.CambiarGrupo();
        }

        public void NuevoButton_Click(object sender, EventArgs e)
        {
            OperariosGrid.newButton_Click(sender, e);
        }

        public void ClonarButton_Click(object sender, EventArgs e)
        {
            OperariosGrid.cloneButton_Click(sender, e);
        }

        public void EliminarButton_Click(object sender, EventArgs e)
        {
            OperariosGrid.deleteButton_Click(sender, e);
        }

        public void ExportarExcel_Click(object sender, EventArgs e)
        {
            OperariosGrid.ExportarExcel();
        }

        public void RefreshButton_Click(object sender, EventArgs e)
        {
            OperariosGrid.RefreshButton_Click(sender, e);
        }

        public void QuitarFiltros_Click(object sender, EventArgs e)
        {
            OperariosGrid.QuitarFiltros_Click(sender, e);
        }

        #endregion Constructor

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
                path += "\\";
                path.Replace(" ", "_");
                if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
                {
                    RadGridView rgv = OperariosGrid.tableLayoutPanel1.Controls[0] as RadGridView;
                    rgv.SaveLayout(path + nombreEstiloGridView);
                }
                else if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadVirtualGrid)
                {
                    RadGridView rvg = OperariosGrid.tableLayoutPanel1.Controls[0] as RadGridView;
                    rvg.SaveLayout(path + nombreEstiloVirtualGrid);
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
                path += "\\";
                path.Replace(" ", "_");
                if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
                {
                    RadGridView rgv = OperariosGrid.tableLayoutPanel1.Controls[0] as RadGridView;
                    rgv.SaveLayout(path + nombreEstiloGridView);
                }
                else if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadVirtualGrid)
                {
                    RadGridView rvg = OperariosGrid.tableLayoutPanel1.Controls[0] as RadGridView;
                    rvg.SaveLayout(path + nombreEstiloVirtualGrid);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce("No se ha podido encontrar el archivo en la dirección") + ":" + path + "\n" + Lenguaje.traduce("Puede cambiar esta dirección en el archivo PathLayouts.xml"));
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
                string s = path + "\\";
                s.Replace(" ", "_");
                if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
                {
                    OperariosGrid.GridView.LoadLayout(s + nombreEstiloGridView);
                }
                else if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadVirtualGrid)
                {
                    OperariosGrid.virtualGrid.LoadLayout(s + nombreEstiloVirtualGrid);
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
                string s = path + "\\";
                s.Replace(" ", "_");
                if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
                {
                    OperariosGrid.GridView.LoadLayout(s + nombreEstiloGridView);
                }
                else if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadVirtualGrid)
                {
                    OperariosGrid.virtualGrid.LoadLayout(s + nombreEstiloVirtualGrid);
                }
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
            if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
            {
                OperariosGrid.GridView.ShowColumnChooser();
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
            if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
            {
                string pathGridView = pathLocal + "\\" + nombreEstiloGridView;
                bool existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    //gridViewControl.LoadLayout(pathGridView);
                    LoadLayoutLocal();
                }
                else
                {
                    pathGridView = pathGlobal + "\\" + nombreEstiloGridView;
                    existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        //gridViewControl.LoadLayout(pathGridView);
                        LoadLayoutGlobal();
                    }
                }
            }
            else if (OperariosGrid.tableLayoutPanel1.Controls[0] is RadVirtualGrid)
            {
                string pathGridView = pathLocal + "\\" + nombreEstiloVirtualGrid;
                bool existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    //gridViewControl.LoadLayout(pathGridView);
                    LoadLayoutLocal();
                }
                else
                {
                    pathGridView = pathGlobal + "\\" + nombreEstiloVirtualGrid;
                    existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        //gridViewControl.LoadLayout(pathGridView);
                        LoadLayoutGlobal();
                    }
                }
            }
        }

        #endregion Estilos
    }
}
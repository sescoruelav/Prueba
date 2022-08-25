using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Herramientas;
using RumboSGA.Herramientas.Stock;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.Presentation.Herramientas.Stock;
using RumboSGA.Presentation.Herramientas.Ventanas;
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
    public partial class RecursosTareaForm : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Variables
        public int idFormulario;

        private RadWaitingBar waitingBar = new RadWaitingBar();


        private int _cantidadRegistros = 0;
        DataTable table = new DataTable();
        Dictionary<string, string> tipos = new Dictionary<string, string>();
        string connectionString = ConexionSQL.getConnectionString();
        SqlConnection conn = new SqlConnection();
        GridViewTemplate template = new GridViewTemplate();
        RadWaitingBar radWait = new RadWaitingBar();


        RadRibbonBarButtonGroup grupoLabel = new RadRibbonBarButtonGroup();
        RadLabelElement lblCantidad = new RadLabelElement();
        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        public RecursosTareaControl recursoTareaControl { get; private set; }

        public enum modoForm
        {
            lectura,
            nuevo,
            edicion,
            clonar
        }

        public modoForm ModoAperturaForm { get { return _modoAperturaForm; } set { _modoAperturaForm = value; } }

        private modoForm _modoAperturaForm;


        public GridScheme _esquemGrid { get; set; }




        protected dynamic _selectedRow;
        protected GridScheme _esquemaGrid;
        public string name = Lenguaje.traduce("Recurso tarea");
        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        #endregion
        #region Constructor
        public RecursosTareaForm()
        {
            try
            {

                log.Info("Inicio Pantalla RecursosTareaForm " + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
                InitializeComponent();

                
                //inicializamos carga vista


                //TODO: No se utilizan conexiones en las pantallas
                //conn = new SqlConnection(connectionString);

                this.Name = name;

                //Asigna la cantidad de registros
                ControlesLenguaje();

                this.Shown += form_Shown;
                // Genera los dos componentes, grid y tabs, por separado en la misma ventana
                recursoTareaControl = new RecursosTareaControl();

                //El otro componente, tabs, se genera asíncronamente en ArticulosControl, en el evento CellClick

               tableLayoutPanel2.Controls.Add(recursoTareaControl);
                recursoTareaControl.Dock = DockStyle.Fill;

                this.Show();

                //AñadirLabelCantidad();



                //Suscripción a eventos

                //Muestra el formulario

                //ArticulosGrid.RefrescarGrid();

                //InicializarComboPaginacion();
                //InitializeThemesDropDown();
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
            radRibbonBarGroup2.Text = Lenguaje.traduce(radRibbonBarGroup2.Text);
            radRibbonBarGroup3.Text = Lenguaje.traduce(radRibbonBarGroup3.Text);
            radRibbonBarGroup4.Text = Lenguaje.traduce(radRibbonBarGroup4.Text);

            lblCantidad.Text = Lenguaje.traduce(strings.Articulos) + ": " + CantidadRegistros;
            ribbonTab1.Text = Lenguaje.traduce(strings.Acciones);

            vistaBarGroup.Text = strings.Ver;

        }


        public void NuevoButton_Click(object sender, EventArgs e)
        {
            recursoTareaControl.newButton_Click(sender, e);
        }

        public void ClonarButton_Click(object sender, EventArgs e)
        {
            recursoTareaControl.cloneButton_Click(sender, e);
        }

        public void EliminarButton_Click(object sender, EventArgs e)
        {
            recursoTareaControl.deleteButton_Click(sender, e);
        }

        public void ExportarExcel_Click(object sender, EventArgs e)
        {
            recursoTareaControl.ExportarExcel();
        }

        public void RefreshButton_Click(object sender, EventArgs e)
        {
            recursoTareaControl.RefreshButton_Click(sender, e);
        }

        public void QuitarFiltros_Click(object sender, EventArgs e)
        {
            recursoTareaControl.QuitarFiltros_Click(sender, e);
        }



        #endregion


        private void RumButtonElementTipoTarea_Click(object sender, EventArgs e)
        {
            FormularioCrearTareaTipo fm = new FormularioCrearTareaTipo();
            fm.ShowDialog();
            this.recursoTareaControl.RefreshData(0);
        }

        private void RumButtonElementAddZona_Click(object sender, EventArgs e)
        {
            FormularioCrearZonaCab fcz = new FormularioCrearZonaCab();
            fcz.ShowDialog();
            this.recursoTareaControl.RefreshData(0);

        }

        private void RumButtonElementHistoria_Click(object sender, EventArgs e)
        {
            if (this.recursoTareaControl.GridView.SelectedRows.Count != 1)
            {
                RadMessageBox.Show(this, Lenguaje.traduce("Selecciona una fila"), "Error", MessageBoxButtons.OK);
                return;
            }
            int idRecurso = int.Parse(this.recursoTareaControl.GridView.SelectedRows[0].Cells[0].Value.ToString());
            FrmMostrarHistorialRecurso fmh = new FrmMostrarHistorialRecurso(idRecurso);
            fmh.ShowDialog();
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
        public void SaveLayoutGlobal()
        {
/*            string path = Persistencia.DirectorioGlobal;*//*XmlReaderPropio.getLayoutPath(0);*//*
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
                if(OperariosGrid.tableLayoutPanel1.Controls[0] is RadGridView)
                {
                    RadGridView rgv = OperariosGrid.tableLayoutPanel1.Controls[0] as RadGridView;
                    rgv.SaveLayout(path+nombreEstiloGridView);
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
*/
        }
        public void SaveLayoutLocal()
        {
            /*string path = Persistencia.DirectorioLocal;*//*XmlReaderPropio.getLayoutPath(1);*//*
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
                path += "\\" ;
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

            }*/

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

        public void ItemColumnas_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón editar columnas " + DateTime.Now);
        }

        public void LoadLayoutGlobal()
        {
            /*string path = Persistencia.DirectorioGlobal;
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
*/
        }
        public void LoadLayoutLocal()
        {
            /*string path = Persistencia.DirectorioLocal;
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
            }*/
        }
    }
}
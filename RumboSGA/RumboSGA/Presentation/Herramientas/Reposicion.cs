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
using RumboSGA.RecursoMotor;
using System.Threading;

namespace RumboSGA.Presentation.Herramientas
{

    public partial class Reposicion : Telerik.WinControls.UI.RadRibbonForm
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Variables
        RadGridView grid = new RadGridView();
        System.Threading.Thread currentThread;
        WSRecursoMotorClient ws = null;
        WSReservaMotorClient ws2 = null;
        String resultado = "";
        RadLabelElement lblCantidad = null;
        RadDataFilterDialog radDataFilterReposiciones = new RadDataFilterDialog();
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        public RadGridView GridView { get => grid; set => grid = value; }
        #endregion
        #region Constructor
        public Reposicion()
        {
            InitializeComponent();
            InitializeThemesDropDown();
            this.WindowState = FormWindowState.Maximized;
            rBtnSoloConflictos.Text = Lenguaje.traduce(strings.SoloConflictos);
            rBtnGenerarReposiciones.Text = Lenguaje.traduce(strings.GenerarRepos);
            rBtnAsignarRecurso.Text = Lenguaje.traduce(strings.AsignarRecurso);
            ribbonTab1.Text = Lenguaje.traduce(strings.Acciones);
            vistaBarGroup.Text = Lenguaje.traduce(strings.Ver);
            lblCantidad = new RadLabelElement();
            rrbgConfiguracion.Items.Add(lblCantidad);
            grid.AllowAddNewRow = false;
            grid.ReadOnly = true;
            grid.Dock = DockStyle.Fill;
            grid.EnableFiltering = true;
            grid.MasterTemplate.EnableFiltering = true;
            grid.EnablePaging = false;
            ws = new WSRecursoMotorClient();
            // ws2 = new WSReservaMotorClient();
            //DataTable dt = null;
            //if (ws.isActivadoReposicionPorZona())
            //{
            //    dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesActivadoZona);
            //}
            //else
            //{
            //    dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesNoActivado);
            //}
            //
            //
            //
            //
            //Utilidades.TraducirDataTableColumnName(ref dt);
            //grid.DataSource = dt;
            grid.Dock = DockStyle.Fill;
            grid.MultiSelect = true;
            //lblCantidad.Text = "     " + Lenguaje.traduce("Registros: ") + dt.Rows.Count;

            tlPanelPrincipal.Controls.Add(grid, 0, 0);
            tlPanelPrincipal.SetColumnSpan(grid, 7);

            this.Shown += form_Shown;
            this.Show();


            RecogerDatos();
            int ancho = (grid.Width - 25) / (this.grid.Columns.Count - 1);
            for (int x = 0; x < this.grid.Columns.Count; x++)
            {
                this.grid.Columns[x].Width = ancho;

            }
            this.grid.Columns[0].IsVisible = false;

            pintarFilas(grid);

        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
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
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public void btnExportacion_Click(object sender, EventArgs e)
        {

            FuncionesGenerales.exportarAExcelGenerico(this.grid);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RecogerDatos();

        }

        private void RecogerDatos()
        {
         
            DataTable dt = null;
            if (ws.isActivadoReposicionPorZona())
            {
                dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesActivadoZona);
            }
            else
            {
                dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesNoActivado);
            }
            
            Utilidades.TraducirDataTableColumnName(ref dt);
            grid.DataSource = dt;
            grid.Dock = DockStyle.Fill;
            grid.MultiSelect = true;
            pintarFilas(grid);
            lblCantidad.Text = "     " + Lenguaje.traduce("Registros: ") + grid.Rows.Count;
            
           

        }


        private void pintarFilas(RadGridView grid)
        {

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                string total = grid.Rows[i].Cells["Total"].Value.ToString();
                string stockMinReab = grid.Rows[i].Cells["stockMinReab"].Value.ToString();
                string existenciasub = grid.Rows[i].Cells["ExistenciasUB"].Value.ToString();
                if (int.Parse(total) >= int.Parse(stockMinReab))
                {
                    for (int x = 0; x < grid.ColumnCount; x++)
                    {
                        grid.Rows[i].Cells[x].Style.ForeColor = Color.FromArgb(34, 139, 34);
                    }
                }
                else
                {
                    if (int.Parse(existenciasub) >= int.Parse(stockMinReab))
                    {
                        for (int x = 0; x < grid.ColumnCount; x++)
                        {
                            grid.Rows[i].Cells[x].Style.ForeColor = Color.FromArgb(255, 0, 0);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < grid.ColumnCount; x++)
                        {
                            grid.Rows[i].Cells[x].Style.ForeColor = Color.FromArgb(210, 105, 30);
                        }
                    }
                }
            }
        }



        private void rBtnSoloConflictos_Click(object sender, EventArgs e)
        {


            try
            {
                if (this.rBtnSoloConflictos.Text == Lenguaje.traduce(strings.SoloConflictos))
                {
                    ws = new WSRecursoMotorClient();
                    bool activadoPorZona = ws.isActivadoReposicionPorZona();
                    DataTable dt = null;
                    if (activadoPorZona)
                    {
                        dt = ConexionSQL.getDataTable(SentenciasSQL.querySoloConflictosActivadoPorZona);
                    }
                    else
                    {
                        dt = ConexionSQL.getDataTable(SentenciasSQL.querySoloConflictosNoActivadoPorZona);
                    }
                    Utilidades.TraducirDataTableColumnName(ref dt);
                    grid.DataSource = dt;
                    lblCantidad.Text = "     " + Lenguaje.traduce("Registros: ") + dt.Rows.Count;

                    this.rBtnSoloConflictos.Text = Lenguaje.traduce("Ver todos");
                }
                else
                {
                    DataTable dt = null;
                    if (ws.isActivadoReposicionPorZona())
                    {
                        dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesActivadoZona);
                    }
                    else
                    {
                        dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesNoActivado);
                    }
                    Utilidades.TraducirDataTableColumnName(ref dt);
                    grid.DataSource = dt;
                    lblCantidad.Text = "     " + Lenguaje.traduce("Registros: ") + dt.Rows.Count;
                    this.rBtnSoloConflictos.Text = Lenguaje.traduce(strings.SoloConflictos);

                }
                pintarFilas(grid);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }



        private void rBtnGenerarReposiciones_Click(object sender, EventArgs e)
        {

            ws = new WSRecursoMotorClient();
            ws2 = new WSReservaMotorClient();
            DialogCantidad dialog = null;
            resultado = "";
            try
            {
                if (!ws.isActivadoReposicionPorZona() && !ws.isActivadoRellenarUbPicking())
                {
                    dialog = new DialogCantidad("");
                    dialog.ShowDialog();
                }
                for (int x = 0; x < grid.SelectedRows.Count; x++)
                {


                    string json = crearJson(x);
                    if (ws.isActivadoReposicionPorZona())
                    {

                        ws2.generarReservasZona(json);
                        RadMessageBox.Show("Se ha generado las reposiciones correctamente");
                        
                    }
                    else
                    {
                        if (ws.isActivadoRellenarUbPicking())
                        {
                            ws2.generarReservasHPCapacidad(json);
                            RadMessageBox.Show("Se ha generado las reposiciones correctamente");
                        }
                        else
                        {
                            int num = 0;
                            try
                            {
                                if (dialog.accion > 0)
                                {
                                    num = int.Parse(dialog.textBox1.Text.ToString());
                                }
                                else
                                {
                                    resultado = Lenguaje.traduce("Se ha cancelado la operación");
                                }

                            }catch(Exception ex1)
                            {
                                ExceptionManager.GestionarError(ex1, "Cantidad incorrecta");
                            }
                            if (num > 0)
                            {
                                String[] wsResult = ws2.generarReservas(num, json);
                                if (wsResult.Length > 0)
                                {
                                    resultado = resultado + " \r\n " + wsResult[0].ToString(); ;
                                }
                                else
                                {
                                    resultado = Lenguaje.traduce("Proceso finalizado");
                                }
                            }
                            else
                            {
                                resultado = Lenguaje.traduce("Debe introducir el número de reposiciones a generar");
                            }
                        }
                    }
                }
                RadMessageBox.Show(resultado);

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                RadMessageBox.Show("Ha surgido un error al generar las reposiciones");

            }


        }

        private void rBtnAsignarRecurso_Click(object sender, EventArgs e)
        {
            ws = new WSRecursoMotorClient();
            int[] idArticulos = new int[1];
            try
            {
                for (int x = 0; x < grid.SelectedRows.Count; x++)
                {
                    String valor = grid.SelectedRows[x].Cells["idarticulo"].Value.ToString();
                    int valor2 = int.Parse(valor);
                    idArticulos[0] = valor2;

                    ws.asignarRecursoReposicion(idArticulos);
                }

                RadMessageBox.Show("Se ha asignado correctamente el recurso");

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                RadMessageBox.Show("Ha surgido un error al asignar recurso");

            }
        }

        private void btnVistaPedidosFiltrar_Click(object sender, EventArgs e)
        {
            if (grid.Rows.Count > 0)
            {
                try
                {

                    radDataFilterReposiciones.DataSource = grid.DataSource;
                    if (radDataFilterReposiciones.Visible == false)
                    {
                        radDataFilterReposiciones.ShowDialog();
                    }
                    else
                    {
                        radDataFilterReposiciones.Close();
                        radDataFilterReposiciones.DataFilter.ApplyFilter();
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

        private void btnVistaPedidosQuitarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón quitar filtro " + DateTime.Now);
                if (grid.IsInEditMode)
                {
                    grid.EndEdit();
                }
                grid.FilterDescriptors.Clear();
                grid.GroupDescriptors.Clear();
                radDataFilterReposiciones.DataFilter.Expression = "";
                radDataFilterReposiciones.DataFilter.ApplyFilter();
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

        private void menuItemCargar_Click(object sender, EventArgs e)
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
        private void LoadLayoutGlobal()
        {
            string path = XmlReaderPropio.getLayoutPath(0);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                string s = path + "\\" + this.Name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
                s.Replace(" ", "_");
                this.grid.LoadLayout(s);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                string s = path + "\\" + this.Name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                s.Replace(" ", "_");
                GridView.LoadLayout(s);
            }
            else
            {
                string s = path + "\\" + this.Name + "Grid" + ConexionSQL.getNombreConexion() + ".xml";
                s.Replace(" ", "_");
                GridView.LoadLayout(s);
            }
        }
        private void LoadLayoutLocal()
        {
            string path = XmlReaderPropio.getLayoutPath(1);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                string s = path + "\\" + this.Name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
                s.Replace(" ", "_");
                this.grid.LoadLayout(s);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                string s = path + "\\" + this.Name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                s.Replace(" ", "_");
                GridView.LoadLayout(s);
            }
            else
            {
                string s = path + "\\" + this.Name + "Grid" + ConexionSQL.getNombreConexion() + ".xml";
                s.Replace(" ", "_");
                GridView.LoadLayout(s);
            }
        }

        private void menuItemGuardar_Click(object sender, EventArgs e)
        {
            // BaseGridControl a = (BaseGridControl)tlPanelPrincipal.GetControlFromPosition(1, 0);
            //a.SaveButton_Click(sender, e);
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

        private void SaveLayoutGlobal()
        {
            string path = XmlReaderPropio.getLayoutPath(0);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
                path.Replace(" ", "_");
                this.grid.SaveLayout(path);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                path.Replace(" ", "_");
                GridView.SaveLayout(path);
            }
            else
            {
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "Grid" + ConexionSQL.getNombreConexion() + ".xml";
                path.Replace(" ", "_");
                this.grid.SaveLayout(path);
            }
        }
        private void SaveLayoutLocal()
        {
            string path = XmlReaderPropio.getLayoutPath(1);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
                path.Replace(" ", "_");
                this.grid.SaveLayout(path);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                path.Replace(" ", "_");
                GridView.SaveLayout(path);
            }
            else
            {
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "Grid" + ConexionSQL.getNombreConexion() + ".xml";
                path.Replace(" ", "_");
                this.grid.SaveLayout(path);
            }
        }


        private void itm_EditarColumnas_Click(object sender, EventArgs e)
        {

            try
            {
                this.grid.ShowColumnChooser();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }



        private string crearJson(int posicion)
        {

            ReposicionJson p = new ReposicionJson();
            p.IDARTICULO = grid.SelectedRows[posicion].Cells[0].Value.ToString();
            p.REFERENCIA = grid.SelectedRows[posicion].Cells[1].Value.ToString();
            p.DESCRIPCION = grid.SelectedRows[posicion].Cells[2].Value.ToString();
            p.HPICKING = grid.SelectedRows[posicion].Cells[3].Value.ToString();
            p.STOCKMINREAB = grid.SelectedRows[posicion].Cells[4].Value.ToString();
            p.EXISTENCIAS_PI = grid.SelectedRows[posicion].Cells[5].Value.ToString();
            p.EXISTENCIAS_UB = grid.SelectedRows[posicion].Cells[6].Value.ToString();
            p.RESERVAS_PI = grid.SelectedRows[posicion].Cells[7].Value.ToString();
            p.RESERVAS_RP = grid.SelectedRows[posicion].Cells[8].Value.ToString();
            p.EXISTENCIAS_TOTAL = grid.SelectedRows[posicion].Cells[9].Value.ToString();

            string json = JsonConvert.SerializeObject(p);

            return json;
        }

        private void tlPanelPrincipal_Paint(object sender, PaintEventArgs e)
        {

        }
    }

   
    #endregion
}

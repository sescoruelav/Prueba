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
using RumboSGA.Properties;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager;
using Telerik.WinControls;
using System.Reflection;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.UI.Docking;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class Existencias : UserControl
    {
        public RadButton btnVerEntrada = new RadButton();
        public RadButton btnVerPedidosPro = new RadButton();
        public RadButton btnVerDevolucionesCli = new RadButton();
        public RadButton btnVerOrdenesFab = new RadButton();
        public RadButton btnVerRegularizacion = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnVerPacking = new RadButton();
        public RadButton btnVerOrdenRec = new RadButton();
        public RadButton btnVerPedidosCli = new RadButton();
        public RadButton btnVerCargaCamion = new RadButton();
        public RadButton btnBuscarEquivalentes = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridExistencias = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelTabPedidos = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        public static string q { get; set; }
        public string celda { get; set; }
        
        public Existencias()
        {
            this.gridExistencias.Name = "TrazabilidadExistencias";
            tableLayoutPanelBotones.AutoSize = true;

            tableLayoutPanelTabPedidos.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelTabPedidos.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntrada, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosPro, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerDevolucionesCli, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenesFab, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerRegularizacion, 4, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 5, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPacking, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenRec, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosCli, 2, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerCargaCamion, 3, 1);
            tableLayoutPanelBotones.Controls.Add(btnBuscarEquivalentes, 4, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelTabPedidos.Controls.Add(gridExistencias);

            tableLayoutPanelBotones.RowCount = 2;
            tableLayoutPanelBotones.ColumnCount = 5;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelTabPedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelTabPedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerEntrada.Dock = DockStyle.Fill;
            btnVerPedidosPro.Dock = DockStyle.Fill;
            btnVerDevolucionesCli.Dock = DockStyle.Fill;
            btnVerOrdenesFab.Dock = DockStyle.Fill;
            btnVerRegularizacion.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnVerPacking.Dock = DockStyle.Fill;
            btnVerOrdenRec.Dock = DockStyle.Fill;
            btnVerPedidosCli.Dock = DockStyle.Fill;
            btnVerCargaCamion.Dock = DockStyle.Fill;
            btnBuscarEquivalentes.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            gridExistencias.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelTabPedidos.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelTabPedidos.SetColumnSpan(gridExistencias, 10);

            gridExistencias.MultiSelect = true; // Permite la selección de multiples articulos
            gridExistencias.EnableHotTracking = true;
            gridExistencias.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridExistencias.EnableFiltering = true;
            gridExistencias.MasterTemplate.EnableFiltering = true;
            gridExistencias.MasterTemplate.AllowAddNewRow = false;
            gridExistencias.AllowColumnReorder = false;
            gridExistencias.AllowDragToGroup = false;
            gridExistencias.AllowDeleteRow = false;
            gridExistencias.AllowEditRow = false;
            gridExistencias.BestFitColumns();
            LlenarGrid();

            //Ir quitando según implementación
            btnBuscarEquivalentes.Enabled = false;

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerPacking.Text = Lenguaje.traduce(strings.VerPacking);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);
            btnVerRegularizacion.Text = Lenguaje.traduce(strings.VerRegularizacion);
            btnVerCargaCamion.Text = Lenguaje.traduce(strings.VerCargaCamion);
            btnVerOrdenesFab.Text = Lenguaje.traduce(strings.VerOrdenesFab);
            btnVerDevolucionesCli.Text = Lenguaje.traduce(strings.VerDevolucionesCli);
            btnVerPedidosPro.Text = Lenguaje.traduce(strings.VerPedidosProv);
            btnVerEntrada.Text = Lenguaje.traduce(strings.VerEntradas);
            btnVerOrdenRec.Text = Lenguaje.traduce(strings.VerOrdenesRecogida);
            btnVerPedidosCli.Text = Lenguaje.traduce(strings.VerPedidosCliente);
            btnBuscarEquivalentes.Text = Lenguaje.traduce(strings.BuscarEquivalentes);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerPacking.Click += new EventHandler(VerPacking);
            btnVerSalidas.Click += new EventHandler(VerSalidas);
            btnVerRegularizacion.Click += new EventHandler(VerRegularizacion);
            btnVerCargaCamion.Click += new EventHandler(VerCargaCamion);
            btnVerOrdenesFab.Click += new EventHandler(VerOrdenesFab);
            btnVerDevolucionesCli.Click += new EventHandler(VerDevolucionesCli);
            btnVerPedidosPro.Click += new EventHandler(VerPedidosProv);
            btnVerEntrada.Click += new EventHandler(VerEntrada);
            btnVerOrdenRec.Click += new EventHandler(VerOrdenes);
            btnVerPedidosCli.Click += new EventHandler(VerPedidosCli);
            
            btnExportacion.Click += new EventHandler(Exportar);
            btnRefrescar.Click += new EventHandler(Refrescar);

            // btnConfig
            this.btnConfig.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnConfig.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnConfig.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConfig.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.idiomasMenuItem,
            this.btnVerSql});
            FuncionesGenerales.AddGuardarYCargarLayoutEnRadMenu(ref this.btnConfig);
            this.btnConfig.Items["RadMenuItemGuardarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.guardarLayout(this.gridExistencias, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridExistencias, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            //this.btnConfig.Location = new System.Drawing.Point(1046, 3);
            this.btnConfig.Name = "btnConfig";

            this.btnConfig.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 140, 24);
            this.btnConfig.Size = new System.Drawing.Size(69, 49);
            this.btnConfig.TabIndex = 9;

            this.temasMenuItem.Name = "temasMenuItem";
            this.temasMenuItem.Text = Lenguaje.traduce("Temas");
            this.temasMenuItem.UseCompatibleTextRendering = false;

            this.idiomasMenuItem.Name = "idiomasMenuItem";
            this.idiomasMenuItem.Text = Lenguaje.traduce("Idiomas");
            this.idiomasMenuItem.UseCompatibleTextRendering = false;

            this.btnVerSql.Name = "btnVerSql";
            this.btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            this.btnVerSql.UseCompatibleTextRendering = false;
            InitializeThemesDropDown();

            this.btnExportacion.Size = new Size(50, 50);
            this.btnExportacion.Image = Resources.ExportToExcel;
            this.btnExportacion.ImageAlignment = ContentAlignment.MiddleCenter;

            this.btnRefrescar.Size = new Size(50, 50);
            this.btnRefrescar.Image = Resources.Refresh;
            this.btnRefrescar.ImageAlignment = ContentAlignment.MiddleCenter;

            this.Controls.Add(tableLayoutPanelTabPedidos);
            FuncionesGenerales.ElegirLayout(this.gridExistencias, this.Name);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid();
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerPacking(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Packing.q != String.Empty)
                    Packing.q = "";
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerPedidosCli(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerPedidosProv(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "E.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (PedidosProv.q != String.Empty)
                    PedidosProv.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerDevolucionesCli(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "E.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (DevolucionesCliente.q != String.Empty)
                    DevolucionesCliente.q = "";
                celda = celda.Remove(celda.Length - 3);
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                celda = celda.Remove(celda.Length - 3);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerEntrada(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "EX.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                celda = celda.Remove(celda.Length - 3);
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Matriculas.q = null;
            }
        }

        private void VerOrdenes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (OrdenesRecogidaControl.q != String.Empty)
                    OrdenesRecogidaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerOrdenesFab(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "E.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (OrdenesFabricacion.q != String.Empty)
                    OrdenesFabricacion.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerRegularizacion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "(S.IDENTRADA = '" + sel.Cells[1].Value.ToString() + "' AND S.IDSALIDATIPO = 'SR') OR ";
                }
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                celda = celda.Remove(celda.Length - 3);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE S.IDENTRADA = null";
            }

        }

        private void VerCargaCamion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridExistencias.RowCount != 0)
            {
                foreach (var sel in gridExistencias.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                if (OrdenesCargaControl.q != String.Empty)
                    OrdenesCargaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE S.IDENTRADA = null";
            }            
        }
        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridExistencias);
        }
        public void SetQuery(string query)
        {
            q = query;
            gridExistencias.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid()
        {
            try
            {
                //string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                //q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE EX.IDENTRADA = null";
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridExistencias.DataSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

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
            RadMenuItem item = new RadMenuItem();            
            item.Text = themeName;
            item.Image = image;
            item.Click += new EventHandler(temasItem_Click);
            temasMenuItem.Items.Add(item);                        
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
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PedidosTan
            // 
            this.Name = "PedidosTan";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

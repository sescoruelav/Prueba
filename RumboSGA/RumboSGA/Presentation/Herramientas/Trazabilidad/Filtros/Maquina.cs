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
using RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System.Reflection;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public partial class Maquina : UserControl
    {
        public RadButton btnVerOrdFab = new RadButton();
        public RadButton btnVerEntradas = new RadButton();
        public RadButton btnVerConsumos = new RadButton();
        public RadButton btnVerExistenciasTerm = new RadButton();
        public RadButton btnVerExistenciasZona = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridMaquinas = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelDevProveedor = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private int tipo;
        public static string q { get; set; }
        public string celda { get; private set; }

        public Maquina(int tipo)
        {
            this.tipo = tipo;
            this.gridMaquinas.Name = "TrazabilidadMaquinas";
            
            tableLayoutPanelBotones.AutoSize = true;

            tableLayoutPanelDevProveedor.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelDevProveedor.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdFab, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntradas, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerConsumos, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerExistenciasTerm, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerExistenciasZona, 1, 1);

            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelDevProveedor.Controls.Add(gridMaquinas);

            tableLayoutPanelBotones.RowCount = 2;
            tableLayoutPanelBotones.ColumnCount = 3;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelDevProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelDevProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerOrdFab.Dock = DockStyle.Fill;
            btnVerEntradas.Dock = DockStyle.Fill;
            btnVerConsumos.Dock = DockStyle.Fill;
            btnVerExistenciasTerm.Dock = DockStyle.Fill;
            btnVerExistenciasZona.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            gridMaquinas.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelDevProveedor.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelDevProveedor.SetColumnSpan(gridMaquinas, 10);

            gridMaquinas.MultiSelect = true; // Permite la selección de multiples articulos
            gridMaquinas.EnableHotTracking = true;
            gridMaquinas.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridMaquinas.EnableFiltering = true;
            gridMaquinas.MasterTemplate.EnableFiltering = true;
            gridMaquinas.MasterTemplate.AllowAddNewRow = false;
            gridMaquinas.AllowDragToGroup = false;
            gridMaquinas.AllowColumnReorder = false;
            gridMaquinas.AllowDeleteRow = false;
            gridMaquinas.AllowEditRow = false;
            gridMaquinas.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerOrdFab.Text = Lenguaje.traduce(strings.VerOrdenesFab);
            btnVerEntradas.Text = Lenguaje.traduce(strings.VerEntradas);
            btnVerConsumos.Text = Lenguaje.traduce(strings.VerConsumos);
            btnVerExistenciasTerm.Text = Lenguaje.traduce(strings.VerExistenciasTerminado);
            btnVerExistenciasZona.Text = Lenguaje.traduce(strings.VerExistenciasZona);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnExportacion.Click += new EventHandler(Exportar);
            btnVerOrdFab.Click += new EventHandler(VerOrdenesFab);
            btnVerEntradas.Click += new EventHandler(VerEntrada);
            btnVerConsumos.Click += new EventHandler(VerConsumos);
            btnVerExistenciasTerm.Click += new EventHandler(VerExistenciasTerminado);
            btnVerExistenciasZona.Click += new EventHandler(VerExistenciasZona);
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
                FuncionesGenerales.guardarLayout(this.gridMaquinas, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridMaquinas, this.Name);
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

            this.Controls.Add(tableLayoutPanelDevProveedor);
            FuncionesGenerales.ElegirLayout(this.gridMaquinas, this.Name);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerOrdenesFab(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            if (gridMaquinas.RowCount != 0)
            {
                celda = gridMaquinas.SelectedRows[0].Cells[0].Value.ToString();
                if (OrdenesFabricacion.q != String.Empty)
                    OrdenesFabricacion.q = "";
                PorFecha porFecha = new PorFecha(gridMaquinas, "maquinas");
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE (OF1.IDMAQUINA=1 AND REPLACE(left(CONVERT(varchar(10),OF1.FECHACREACION, 126),10),'-','')=null)";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesFabricacion.q = null;
            }
        }

        private void VerEntrada(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            if (gridMaquinas.RowCount != 0)
            {
                celda = gridMaquinas.SelectedRows[0].Cells[0].Value.ToString();
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                new PorFecha(gridMaquinas, "maquinas");
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE (OF1.IDMAQUINA=1 AND REPLACE(left(CONVERT(varchar(10), OF1.FECHACREACION, 126),10),'-','')=null)";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Matriculas.q = null;
            }
        }

        private void VerConsumos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            if (gridMaquinas.RowCount != 0)
            {
                celda = gridMaquinas.SelectedRows[0].Cells[0].Value.ToString();
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                //La búsqueda entra dentro de porfecha en regularizaciones de salida a pesar de que es otra función
                new PorFecha(gridMaquinas, "maquinas");
                //RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE S.IDSALIDATIPO='SR' AND (REPLACE(left(CONVERT(varchar(10),S.FECHA, 126),10),'-','')=null)";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = null;
            }
        }

        private void VerExistenciasTerminado(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMaquinas.RowCount != 0)
            {
                foreach (var sel in gridMaquinas.SelectedRows)
                {
                    celda += "EX.IDHUECO=" + sel.Cells[Lenguaje.traduce("MUELLE")].Value.ToString() + " OR ";
                }
                if (Existencias.q != String.Empty)
                    Existencias.q = "";
                celda = celda.Remove(celda.Length - 3);
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Existencias.q = null;
            }
        }

        private void VerExistenciasZona(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMaquinas.RowCount != 0)
            {
                foreach (var sel in gridMaquinas.SelectedRows)
                {
                    celda += "VH.IDZONACAB=" + sel.Cells[2].Value.ToString() + " OR ";
                }
                if (Existencias.q != String.Empty)
                    Existencias.q = "";
                celda = celda.Remove(celda.Length - 3);
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Existencias.q = null;
            }
        }
        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridMaquinas);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridMaquinas.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        new PorNombreMaquina(gridMaquinas);
                        q = ConfigurarSQL.CargarConsulta(path, "MAQUINAS") + " WHERE IDMAQUINA=null";
                        break;
                    case 2:
                        new PorZonaMaquina(gridMaquinas);
                        q = ConfigurarSQL.CargarConsulta(path, "MAQUINAS") + " WHERE IDZONALOGICA=null";
                        break;
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "MAQUINAS");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridMaquinas.DataSource = dt.DefaultView;
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
            // DevolucionesProveedorTan
            // 
            this.Name = "DevolucionesProveedor";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

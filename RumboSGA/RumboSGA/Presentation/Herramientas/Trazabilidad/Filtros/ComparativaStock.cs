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
using Telerik.WinControls.Data;
using RumboSGA.Controles;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public partial class ComparativaStock : UserControl
    {
        private RadButton btnExportacion = new RadButton();
        private RadButton btnRefrescar = new RadButton();

        private RadDropDownButton btnAgrupar = new RadDropDownButton();
        private RumMenuItem stockPorArticuloItem = new RumMenuItem();
        private RumMenuItem stockPorLoteItem = new RumMenuItem();

        private RadDropDownButton btnConfig = new RadDropDownButton();
        private RadMenuItem temasMenuItem = new RadMenuItem();
        private RadMenuItem idiomasMenuItem = new RadMenuItem();

        public RadMenuItem btnVerSql = new RadMenuItem();

        public RadButton btnVerExistencias = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnVerEntradas = new RadButton();

        public RadGridView gridComparativaStock = new RadGridView();

        private TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        private TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        private TableLayoutPanel tableLayoutPanelComparativaStock = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private int tipo;
        private string refArticulo;

        public static string celda { get; set; }

        public static string q { get; set; }

        public ComparativaStock(int tipo)
        {
            this.tipo = tipo;
            this.gridComparativaStock.Name = Constantes.TRAZABILIDADCOMPARATIVASTOCK;

            tableLayoutPanelComparativaStock.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelComparativaStock.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelComparativaStock.Controls.Add(gridComparativaStock);

            tableLayoutPanelBotones.Controls.Add(btnVerExistencias, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntradas, 2, 0);

            tableLayoutPanelAuxiliar.Controls.Add(btnAgrupar, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 2, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 3, 0);

            tableLayoutPanelBotones.RowCount = 1;
            tableLayoutPanelBotones.ColumnCount = 4;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelComparativaStock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelComparativaStock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerExistencias.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnVerEntradas.Dock = DockStyle.Fill;
            gridComparativaStock.Dock = DockStyle.Fill;

            btnAgrupar.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Fill;

            tableLayoutPanelComparativaStock.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelComparativaStock.SetColumnSpan(gridComparativaStock, 10);

            gridComparativaStock.MultiSelect = true; // Permite la selección de multiples articulos
            gridComparativaStock.EnableHotTracking = true;
            gridComparativaStock.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridComparativaStock.EnableFiltering = true;
            gridComparativaStock.MasterTemplate.EnableFiltering = true;
            gridComparativaStock.MasterTemplate.AllowAddNewRow = false;
            gridComparativaStock.AllowDeleteRow = false;
            gridComparativaStock.AllowEditRow = false;

            gridComparativaStock.EnableGrouping = true;
            gridComparativaStock.AllowDragToGroup = true;
            gridComparativaStock.AllowColumnReorder = true;

            gridComparativaStock.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnExportacion.Click += new EventHandler(Exportar);
            btnRefrescar.Click += new EventHandler(Refrescar);
            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerEntradas.Click += new EventHandler(VerEntradas);
            btnVerSalidas.Click += new EventHandler(VerSalidas);

            stockPorArticuloItem.Click += stockPorArticuloItem_Click;
            stockPorLoteItem.Click += stockPorLoteItem_Click;

            //btn agrupar
            this.btnAgrupar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAgrupar.Image = global::RumboSGA.Properties.Resources.Table;
            this.btnAgrupar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAgrupar.Items.AddRange(new Telerik.WinControls.RadItem[] {
                this.stockPorArticuloItem,
                this.stockPorLoteItem
            });
            this.btnAgrupar.Name = "btnAgrupar";

            this.btnAgrupar.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 140, 24);
            this.btnAgrupar.Size = new System.Drawing.Size(69, 49);

            this.stockPorArticuloItem.Name = "stockPorArticuloItem";
            this.stockPorArticuloItem.Text = Lenguaje.traduce(Constantes.PORARTICULO);
            this.stockPorArticuloItem.UseCompatibleTextRendering = false;

            this.stockPorLoteItem.Name = "stockPorLoteItem";
            this.stockPorLoteItem.Text = Lenguaje.traduce(Constantes.PORLOTE);
            this.stockPorLoteItem.UseCompatibleTextRendering = false;

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
                FuncionesGenerales.guardarLayout(this.gridComparativaStock, this.Name);
                MessageBox.Show(Lenguaje.traduce(Constantes.ACCIONCOMPLETADAEXITO));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridComparativaStock, this.Name);
                MessageBox.Show(Lenguaje.traduce(Constantes.ACCIONCOMPLETADAEXITO));
            });
            //this.btnConfig.Location = new System.Drawing.Point(1046, 3);
            this.btnConfig.Name = "btnConfig";

            this.btnConfig.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 140, 24);
            this.btnConfig.Size = new System.Drawing.Size(69, 49);
            this.btnConfig.TabIndex = 9;

            this.temasMenuItem.Name = "temasMenuItem";
            this.temasMenuItem.Text = Lenguaje.traduce(Constantes.TEMAS);
            this.temasMenuItem.UseCompatibleTextRendering = false;

            this.idiomasMenuItem.Name = "idiomasMenuItem";
            this.idiomasMenuItem.Text = Lenguaje.traduce(Constantes.IDIOMAS);
            this.idiomasMenuItem.UseCompatibleTextRendering = false;

            btnVerExistencias.Text = Lenguaje.traduce(strings.VerExistencias);
            btnVerEntradas.Text = Lenguaje.traduce(strings.VerEntradas);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);


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

            this.Controls.Add(tableLayoutPanelComparativaStock);
            FuncionesGenerales.ElegirLayout(this.gridComparativaStock, this.Name);
            
        }
             


        private void VerExistencias(object sender, EventArgs e)
        {
            try
            {
                string path = Persistencia.DirectorioBase + Constantes.TRAZABILIDADXML;
                celda = "";
                if (gridComparativaStock.RowCount != 0)
                {
                    foreach (var sel in gridComparativaStock.SelectedRows)
                    {
                        celda += "A.IDARTICULO=" + sel.Cells[Lenguaje.traduce(Constantes.ARTICULO)].Value.ToString() + " OR ";
                    }
                    celda = celda.Remove(celda.Length - 3);
                    if (Existencias.q != String.Empty)
                        Existencias.q = "";
                    Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE E.IDENTRADA = null";
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }


        private void VerSalidas(object sender, EventArgs e)
        {
            try
            {
                string path = Persistencia.DirectorioBase + Constantes.TRAZABILIDADXML;
                celda = "";
                if (gridComparativaStock.RowCount != 0)
                {
                    foreach (var sel in gridComparativaStock.SelectedRows)
                    {
                        string lote = sel.Cells[Lenguaje.traduce(Constantes.LOTE)].Value.ToString().Remove(0, 3);

                        celda += "(A.IDARTICULO=" + sel.Cells[Lenguaje.traduce(Constantes.ARTICULO)].Value.ToString() +
                             " AND E.LOTE='" + lote + "')";
                        celda += " OR ";
                    }
                    celda = celda.Remove(celda.Length - 3);
                    if (OrdenesMovimientosSalidaControl.q != String.Empty)
                        OrdenesMovimientosSalidaControl.q = "";
                    OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, Constantes.MOVIMIENTOSSALIDA) + " WHERE " + celda;
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, Constantes.MOVIMIENTOSSALIDA) + " WHERE E.IDENTRADA = null";
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }

        private void VerEntradas(object sender, EventArgs e)
        {
            try
            {
                string path = Persistencia.DirectorioBase + Constantes.TRAZABILIDADXML;
                celda = "";
                if (gridComparativaStock.RowCount != 0)
                {
                    foreach (var sel in gridComparativaStock.SelectedRows)
                    {
                        string lote = sel.Cells[Lenguaje.traduce(Constantes.LOTE)].Value.ToString().Remove(0, 3);

                        celda += "(A.IDARTICULO=" + sel.Cells[Lenguaje.traduce(Constantes.ARTICULO)].Value.ToString() +
                            " AND E.LOTE='" + lote + "')";
                        celda += " OR ";
                    }
                    if (Matriculas.q != String.Empty)
                        Matriculas.q = "";
                    celda = celda.Remove(celda.Length - 3);
                    Matriculas.q = ConfigurarSQL.CargarConsulta(path, Constantes.MATRICULA) + " WHERE " + celda;
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    Matriculas.q = null;
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }


        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            try
            {
                VerSQL verSQL = new VerSQL(q);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridComparativaStock);
        }

        public void SetQuery(string query)
        {
            try
            {
                q = query;
                gridComparativaStock.DataSource = ConexionSQL.getDataTable(q).DefaultView;

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }

       

        private void LlenarGrid(int tipo)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string path = Persistencia.DirectorioBase + Constantes.TRAZABILIDADXML;
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, Constantes.COMPARATIVASTOCK);
                        break;
                    case 2:
                        PorNum porNum = new PorNum(gridComparativaStock, Lenguaje.traduce(Constantes.COMPARATIVASTOCKPORREFERENCIA));
                        q = ConfigurarSQL.CargarConsulta(path, Constantes.COMPARATIVASTOCK) + " WHERE REFERENCIA=null";
                        porNum.label1.Text = Lenguaje.traduce(Constantes.REFERENCIAARTICULO) + Constantes.DOSPUNTOS;
                        if (porNum.label2.Text == "")
                        {
                            porNum.label2.Hide();
                            porNum.textBox2.Hide();

                        }
                        porNum.ShowDialog();

                        refArticulo = porNum.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, Constantes.COMPARATIVASTOCK) + " WHERE REFERENCIA ='" + refArticulo + "'";
                        gridComparativaStock.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                    case 3:
                        PorNum porLote = new PorNum(gridComparativaStock, Lenguaje.traduce(Constantes.COMPARATIVASTOCKPORLOTE));
                        q = ConfigurarSQL.CargarConsulta(path, Constantes.COMPARATIVASTOCK) + " WHERE DESCRIPCION=null";
                        porLote.label1.Text = Lenguaje.traduce(Constantes.LOTEARTICULO) + Constantes.DOSPUNTOS;
                        if (porLote.label2.Text == "")
                        {
                            porLote.label2.Hide();
                            porLote.textBox2.Hide();

                        }
                        porLote.ShowDialog();

                        refArticulo = porLote.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, Constantes.COMPARATIVASTOCK) + " WHERE DESCRIPCION ='L: " + refArticulo + "'";
                        gridComparativaStock.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                    default:
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridComparativaStock.DataSource = dt.DefaultView;
                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                Cursor.Current = Cursors.Arrow;
            }
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
            RadMenuItem item = new RadMenuItem();
            item.Text = themeName;
            item.Image = image;
            item.Click += new EventHandler(temasItem_Click);
            temasMenuItem.Items.Add(item);
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
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }


        private void stockPorArticuloItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.gridComparativaStock.GroupDescriptors.Clear();
                GroupDescriptor descriptor = new GroupDescriptor();
                                
                descriptor.GroupNames.Add(Lenguaje.traduce(Constantes.REFERENCIA), ListSortDirection.Ascending);
                descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("STOCK_SGA") + ")");
                descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("STOCK_ERP") + ")");
                descriptor.Aggregates.Add("Count('" + Lenguaje.traduce(Constantes.ARTICULO) + "')");

                descriptor.Format = "{0}:{1}  | " + Lenguaje.traduce(Constantes.TOTALSTOCKSGA) + ": {2} | " +
                    Lenguaje.traduce(Constantes.TOTALSTOCKERP) + ": {3}| " + Lenguaje.traduce(Constantes.TOTALDISCREPANCIAS) + ": {4}";

                this.gridComparativaStock.GroupDescriptors.Add(descriptor);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }

        private void stockPorLoteItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.gridComparativaStock.GroupDescriptors.Clear();
                GroupDescriptor descriptor = new GroupDescriptor();

                descriptor.GroupNames.Add(Lenguaje.traduce(Constantes.LOTE), ListSortDirection.Ascending);
                descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("STOCK_SGA") + ")");
                descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("STOCK_ERP") + ")");
                descriptor.Aggregates.Add("Count('" + Lenguaje.traduce(Constantes.ARTICULO) + "')");

                descriptor.Format = "{1}  | " + Lenguaje.traduce(Constantes.TOTALSTOCKSGA) + ": {2} | " +
                    Lenguaje.traduce(Constantes.TOTALSTOCKERP) + ": {3}| " + Lenguaje.traduce(Constantes.TOTALDISCREPANCIAS) + ": {4}";

                this.gridComparativaStock.GroupDescriptors.Add(descriptor);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico) + " - " + ex.Message, "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
    }
}

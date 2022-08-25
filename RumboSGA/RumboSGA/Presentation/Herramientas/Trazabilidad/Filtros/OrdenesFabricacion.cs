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
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class OrdenesFabricacion : UserControl
    {
        public RadMenuItem btnVerSql = new RadMenuItem();
        public RadButton btnVerMaquina = new RadButton();
        public RadButton btnVerEntradas = new RadButton();
        public RadButton btnVerExistencias = new RadButton();
        public RadButton btnVerConsumos = new RadButton();
        public RadButton btnVerOtrasOrdenes = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnLimpiarOrdenFab = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridOrdenFab = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelOrdenesFabricacion = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        private string orden;
        public static string q { get; set; }
        public string celda { get; private set; }

        public string dateString { get; set; }
        public string dateStringMañana { get; set; }

        private int tipo;

        public OrdenesFabricacion(int tipo)
        {
            this.gridOrdenFab.Name = "TrazabilidadOrdenFabricacion";
            tableLayoutPanelOrdenesFabricacion.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelOrdenesFabricacion.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerMaquina, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntradas, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerExistencias, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerConsumos, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOtrasOrdenes, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnLimpiarOrdenFab, 2, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelOrdenesFabricacion.Controls.Add(gridOrdenFab);

            tableLayoutPanelBotones.RowCount = 2;
            tableLayoutPanelBotones.ColumnCount = 4;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelOrdenesFabricacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelOrdenesFabricacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerMaquina.Dock = DockStyle.Fill;
            btnVerEntradas.Dock = DockStyle.Fill;
            btnVerExistencias.Dock = DockStyle.Fill;
            btnVerConsumos.Dock = DockStyle.Fill;
            btnVerOtrasOrdenes.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnLimpiarOrdenFab.Dock = DockStyle.Fill;
            gridOrdenFab.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelOrdenesFabricacion.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelOrdenesFabricacion.SetColumnSpan(gridOrdenFab, 10);

            gridOrdenFab.MultiSelect = true; // Permite la selección de multiples articulos
            gridOrdenFab.EnableHotTracking = true;
            gridOrdenFab.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridOrdenFab.EnableFiltering = true;
            gridOrdenFab.MasterTemplate.EnableFiltering = true;
            gridOrdenFab.MasterTemplate.AllowAddNewRow = false;
            gridOrdenFab.AllowDragToGroup = false;
            gridOrdenFab.AllowColumnReorder = false;
            gridOrdenFab.AllowDeleteRow = false;
            gridOrdenFab.AllowEditRow = false;
            gridOrdenFab.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerMaquina.Text = Lenguaje.traduce(strings.VerMaquina);
            btnVerEntradas.Text = Lenguaje.traduce(strings.VerEntradas);
            btnVerExistencias.Text = Lenguaje.traduce(strings.VerExistencias);
            btnVerConsumos.Text = Lenguaje.traduce(strings.VerConsumos);
            btnVerOtrasOrdenes.Text = Lenguaje.traduce(strings.VerOtrasOrdenes);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);
            btnLimpiarOrdenFab.Text = Lenguaje.traduce(strings.LimpiarOrdenFab);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerEntradas.Click += new EventHandler(VerEntradas);
            btnVerConsumos.Click += new EventHandler(VerConsumos);
            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerMaquina.Click += new EventHandler(VerMaquinas);
            btnVerOtrasOrdenes.Click += new EventHandler(VerOtrasOrdenes);
            btnVerSalidas.Click += new EventHandler(VerSalidas);
            btnLimpiarOrdenFab.Click += new EventHandler(LimpiarOrdenFab);
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
                FuncionesGenerales.guardarLayout(this.gridOrdenFab, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridOrdenFab, this.Name);
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

            this.Controls.Add(tableLayoutPanelOrdenesFabricacion);
            FuncionesGenerales.ElegirLayout(this.gridOrdenFab, this.Name);
        }

        private void LimpiarOrdenFab(object sender, EventArgs e)
        {
            if (gridOrdenFab.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridOrdenFab.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDPEDIDOFAB")].Value.ToString());

                    using (var ws = new WSOrdenProduccionMotorClient())
                    {
                        ws.limpiarOrdenFabricacionRegistro(id);
                    }
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show(strings.ErrorGenerico);
                throw;
            }
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerMaquinas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenFab.RowCount != 0)
            {
                foreach (var sel in gridOrdenFab.SelectedRows)
                {
                    celda += "IDMAQUINA = " + sel.Cells[11].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Maquina.q != String.Empty)
                    Maquina.q = "";
                Maquina.q = ConfigurarSQL.CargarConsulta(path, "MAQUINAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Maquina.q = ConfigurarSQL.CargarConsulta(path, "MAQUINAS") + " WHERE IDMAQUINA = null";
            }
        }

        private void VerEntradas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenFab.RowCount != 0)
            {
                foreach (var sel in gridOrdenFab.SelectedRows)
                {
                    celda += "(E.IDENTRADATIPO = 'EF' AND E.IDPEDIDOFAB =" + sel.Cells[0].Value.ToString() + ") OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE E.IDPEDIDOFAB = null";
            }
        }

        private void VerExistencias(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenFab.RowCount != 0)
            {
                foreach (var sel in gridOrdenFab.SelectedRows)
                {
                    celda += "(E.IDENTRADATIPO = 'EF' AND E.IDPEDIDOFAB =" + sel.Cells[0].Value.ToString() + ") OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Existencias.q != String.Empty)
                    Existencias.q = "";
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE E.IDPEDIDOFAB = null";
            }
        }

        private void VerConsumos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenFab.RowCount != 0)
            {
                foreach (var sel in gridOrdenFab.SelectedRows)
                {
                    celda += "(S.IDSALIDATIPO='SF' AND S.IDPEDIDOFAB =" + sel.Cells[0].Value.ToString() + ") OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE S.IDPEDIDOFAB = null";
            }
        }

        private void VerOtrasOrdenes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenFab.RowCount != 0)
            {
                foreach (var sel in gridOrdenFab.SelectedRows)
                {
                    celda += "(OF1.IDARTICULO = " + sel.Cells[2].Value.ToString() + " AND OF1.IDPEDIDOFAB <> " + sel.Cells[0].Value.ToString() + " AND OF1.IDORDENFABESTADO <> 'PC') OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (OrdenesFabricacion.q != String.Empty)
                    OrdenesFabricacion.q = "";
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE OF1.IDARTICULO = null";
            }
        }

        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenFab.RowCount != 0)
            {
                foreach (var sel in gridOrdenFab.SelectedRows)
                {
                    celda += "(E.IDENTRADATIPO='EF' AND S.IDARTICULO=" + sel.Cells[2].Value.ToString() + ") OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (OrdenesMovimientosSalidaControl.q != String.Empty)
                    OrdenesMovimientosSalidaControl.q = "";
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE S.IDARTICULO = null";
            }
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridOrdenFab);
        }
        public void SetQuery(string query)
        {
            q = query;
            gridOrdenFab.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE REPLACE(left(CONVERT(varchar(10),OF1.FECHACREACION, 126),10),'-','')='" + DateTime.Today.ToString("yyyyMMdd") + "'";
                        break;
                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE IDORDENFABESTADO<>'PC'";
                        break;
                    case 3:
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE M.IDMAQUINA=0";
                        break;
                    case 4: //POR NÚMERO DE ORDEN DE FABRICACIÓN
                        PorNum porNum = new PorNum(gridOrdenFab, "Por número de orden de fabricación");
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE OF1.ORDEN=null";
                        porNum.label1.Text = Lenguaje.traduce("Número Orden:");
                        if (porNum.label2.Text == "")
                        {
                            porNum.label2.Hide();
                            porNum.textBox2.Hide();

                        }
                        porNum.ShowDialog();

                        orden = porNum.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE OF1.ORDEN='" + orden + "'";
                        gridOrdenFab.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                    case 5: //POR FECHA DE ORDEN DE FABRICACIÓN
                        PorFecha porFecha = new PorFecha(gridOrdenFab, "Por fecha Orden Fabricación");
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE REPLACE(left(CONVERT(varchar(10),OF1.FECHACREACION, 126),10),'-','')=null";
                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            switch (tipo)
                            {

                                //case maquinas
                                case 1:
                                    string consulta = "";
                                    foreach (var sel in gridOrdenFab.SelectedRows)
                                    {
                                        consulta += "(OF1.IDMAQUINA=" + sel.Cells[0].Value.ToString() + " AND REPLACE(left(CONVERT(varchar(10),FECHACREACION, 126),10),'-','')='" + dateString + "') OR ";
                                    }
                                    consulta = consulta.Remove(consulta.Length - 3);
                                    q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE " + consulta;
                                    break;

                                default:
                                    q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE REPLACE(left(CONVERT(varchar(10),OF1.FECHACREACION, 126),10),'-','')='" + dateString + "'";
                                    break;
                            }
                            gridOrdenFab.DataSource = ConexionSQL.getDataTable(q).DefaultView;

                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE REPLACE(left(CONVERT(varchar(10),OF1.FECHACREACION, 126),10),'-','')=null";
                        }
                        break;
                    case 6: //POR FECHA Y ARTICULO
                        PorArticuloFechaOrdFab porArticuloFechaOrdFab = new PorArticuloFechaOrdFab(gridOrdenFab);
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE OF1.IDARTICULO=null";
                        break;
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridOrdenFab.DataSource = dt.DefaultView;
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
            // OrdenesFabricacion
            // 
            this.Name = "OrdenesFabricacion";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

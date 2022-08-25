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
using RumboSGA.Controles;
using RumboSGAManager.Model;
using RumboSGA;
using RumboSGA.CargaMotor;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class OrdenesCargaControl : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadMenuItem btnVerSql = new RadMenuItem();
        public RadButton btnVerPacking = new RadButton();
        public RadButton btnVerPedidos = new RadButton();
        public RadButton btnVerOrdenes = new RadButton();
        public RadButton btnVerCont = new RadButton();
        public RadButton btnImpResumenOrdenCarga = new RadButton();
        public RadButton btnImpContOrdenCarga = new RadButton();
        public RadButton btnImpEtiqNomPLCliente = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridOrdenCarga = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelOrdenesCarga = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        private string NumAlbTransportista;
        private string idcarga;
        private int tipo;

        public static string q { get; set; }
        public string celda { get; set; }
        public string dateString { get; set; }
        public string dateStringMañana { get; set; }

        public OrdenesCargaControl(int tipo)
        {
            this.tipo = tipo;
            this.gridOrdenCarga.Name = "TrazabilidadOrdenesCarga";
            tableLayoutPanelOrdenesCarga.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelOrdenesCarga.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPacking, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidos, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenes, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerCont, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnImpContOrdenCarga, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnImpResumenOrdenCarga, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnImpEtiqNomPLCliente, 2, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelOrdenesCarga.Controls.Add(gridOrdenCarga);

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

            tableLayoutPanelOrdenesCarga.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelOrdenesCarga.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerPacking.Dock = DockStyle.Fill;
            btnVerPedidos.Dock = DockStyle.Fill;
            btnVerOrdenes.Dock = DockStyle.Fill;
            btnVerCont.Dock = DockStyle.Fill;
            btnImpContOrdenCarga.Dock = DockStyle.Fill;
            btnImpResumenOrdenCarga.Dock = DockStyle.Fill;
            btnImpEtiqNomPLCliente.Dock = DockStyle.Fill;
            gridOrdenCarga.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelOrdenesCarga.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelOrdenesCarga.SetColumnSpan(gridOrdenCarga, 10);

            gridOrdenCarga.MultiSelect = true; // Permite la selección de multiples articulos
            gridOrdenCarga.EnableHotTracking = true;
            gridOrdenCarga.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridOrdenCarga.EnableFiltering = true;
            gridOrdenCarga.MasterTemplate.EnableFiltering = true;
            gridOrdenCarga.MasterTemplate.AllowAddNewRow = false;
            gridOrdenCarga.AllowColumnReorder = false;
            gridOrdenCarga.AllowDragToGroup = false;
            gridOrdenCarga.AllowDeleteRow = false;
            gridOrdenCarga.AllowEditRow = false;
            gridOrdenCarga.BestFitColumns();

            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerPacking.Text = Lenguaje.traduce(strings.VerPacking);
            btnVerPedidos.Text = Lenguaje.traduce(strings.VerPedidos);
            btnVerOrdenes.Text = Lenguaje.traduce(strings.VerOrdenes);
            btnVerCont.Text = Lenguaje.traduce(strings.VerContenido);
            btnImpResumenOrdenCarga.Text = Lenguaje.traduce(strings.ImpResumenOrdenCarga);
            btnImpContOrdenCarga.Text = Lenguaje.traduce(strings.ImpContOrdenCarga);
            btnImpEtiqNomPLCliente.Text = Lenguaje.traduce(strings.ImpEtiqNomPLCliente);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerOrdenes.Click += new EventHandler(VerOrdenes);
            btnVerPedidos.Click += new EventHandler(VerPedidos);
            btnVerCont.Click += new EventHandler(VerContenido);
            btnVerPacking.Click += new EventHandler(VerPacking);
            btnExportacion.Click += new EventHandler(Exportar);
            btnRefrescar.Click += new EventHandler(Refrescar);
            btnImpContOrdenCarga.Click += new EventHandler(ImprimirInformeContenidoOrdenCarga);
            btnImpResumenOrdenCarga.Click += new EventHandler(ImprimirInformeResumenOrdenCarga);
            btnImpEtiqNomPLCliente.Click += new EventHandler(ImprimirEtiquetaNomPLCliente);

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
                FuncionesGenerales.guardarLayout(this.gridOrdenCarga, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridOrdenCarga, this.Name);
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

            this.Controls.Add(tableLayoutPanelOrdenesCarga);
            FuncionesGenerales.ElegirLayout(this.gridOrdenCarga, this.Name);
        }

        private void ImprimirEtiquetaNomPLCliente(object sender, EventArgs e)
        {
            if (gridOrdenCarga.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridOrdenCarga.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDCARGA")].Value.ToString());

                    var ws = new WSCargaMotorClient();
                    ws.imprimirPackingListNominativoCarga(id, User.NombreImpresora);
                }
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a la impresora "), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridOrdenCarga);
        }

        private void ImprimirInformeResumenOrdenCarga(object sender, EventArgs e)
        {
            try
            {
                if (gridOrdenCarga.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                var parametros = string.Empty;

                foreach (var sel in gridOrdenCarga.SelectedRows)
                {
                    var id = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDCARGA")].Value.ToString());
                    parametros += "IDCARGA=" + id + " OR ";
                }
                int idInforme = 17300;
                VisorInforme v = new VisorInforme(idInforme, parametros);
                v.TopMost = true;
                v.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ImprimirInformeContenidoOrdenCarga(object sender, EventArgs e)
        {
            try
            {
                if (gridOrdenCarga.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                var parametros = string.Empty;

                foreach (var sel in gridOrdenCarga.SelectedRows)
                {
                    var id = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDCARGA")].Value.ToString());
                    parametros += "IDCARGA=" + id + " OR ";
                }
                int idInforme = 17400;
                VisorInforme v = new VisorInforme(idInforme, parametros);
                v.TopMost = true;
                v.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void VerPedidos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenCarga.RowCount != 0)
            {
                foreach (var sel in gridOrdenCarga.SelectedRows)
                {
                    celda += "CL.IDCARGA = " + sel.Cells[0].Value.ToString() + " OR ";
                }
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE CL.IDCARGA = null";
            }
        }

        private void VerOrdenes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenCarga.RowCount != 0)
            {
                foreach (var sel in gridOrdenCarga.SelectedRows)
                {
                    celda += "CL.IDCARGA = " + sel.Cells[0].Value.ToString() + " OR ";
                }
                if (OrdenesRecogidaControl.q != String.Empty)
                    OrdenesRecogidaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE CL.IDCARGA = null";
            }
        }

        private void VerContenido(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenCarga.RowCount != 0)
            {
                foreach (var sel in gridOrdenCarga.SelectedRows)
                {
                    celda += "CL.IDCARGA = " + sel.Cells[0].Value.ToString() + " OR ";
                }
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                celda = celda.Remove(celda.Length - 3);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE CL.IDCARGA = null";
            }
        }

        private void VerPacking(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenCarga.RowCount != 0)
            {
                foreach (var sel in gridOrdenCarga.SelectedRows)
                {
                    celda += "CL.IDCARGA = " + sel.Cells[0].Value.ToString() + " OR ";
                }
                if (Packing.q != String.Empty)
                    Packing.q = "";
                celda = celda.Remove(celda.Length - 3);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE CL.IDCARGA = null";
            }
        }

        public void SetQuery(string query)
        {
            q = query;
            gridOrdenCarga.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.FECHACARGA >='" + DateTime.Today.ToString("yyyyMMdd") + "' AND CB.FECHACARGA <'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;

                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.IDCARGAESTADO='PE'";
                        break;

                    case 3: //NÚMERO DE ORDEN
                        PorNum porNum = new PorNum(gridOrdenCarga, Lenguaje.traduce("Por Nº Albaran Transportista"));
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CL.IDCARGA=null";
                        porNum.label1.Text = Lenguaje.traduce("Número Albarán Transportista:");
                        if (porNum.label2.Text == "")
                        {
                            porNum.label2.Hide();
                            porNum.textBox2.Hide();
                        }
                        porNum.ShowDialog();

                        NumAlbTransportista = porNum.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE PL.ALBARANTRANSPORTE='" + NumAlbTransportista + "'";
                        gridOrdenCarga.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;

                    case 4: //POR ESTADO
                        PorEstadoOrdCarga porEstadoOrdCarga = new PorEstadoOrdCarga(gridOrdenCarga);
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.IDCARGAESTADO=null";
                        break;

                    case 5: //POR FECHA
                        PorFecha porFecha = new PorFecha(gridOrdenCarga, "Por fecha");
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.IDCARGAESTADO=null";

                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.FECHACARGA>='" + dateString + "' AND CB.FECHACARGA<'" + dateStringMañana + "'"; ;
                            gridOrdenCarga.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.IDCARGAESTADO=null";
                        }
                        break;

                    case 6: //POR IDENTIFICADOR
                        PorNum porIdentificadorCarga = new PorNum(gridOrdenCarga, "Por identificador de carga");
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.IDCARGA=null";
                        porIdentificadorCarga.label1.Text = Lenguaje.traduce("Id carga:");
                        if (porIdentificadorCarga.label2.Text == "")
                        {
                            porIdentificadorCarga.label2.Hide();
                            porIdentificadorCarga.textBox2.Hide();
                        }
                        porIdentificadorCarga.ShowDialog();

                        idcarga = porIdentificadorCarga.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.IDCARGA='" + idcarga + "'";
                        gridOrdenCarga.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;

                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridOrdenCarga.DataSource = dt.DefaultView;
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

        #endregion Temas

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // OrdenesCargaControl
            //
            this.Name = "OrdenesCargaControl";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);
        }
    }
}
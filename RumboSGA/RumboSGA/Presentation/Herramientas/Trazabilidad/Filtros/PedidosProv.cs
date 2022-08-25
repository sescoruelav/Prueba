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
using System.Reflection;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using RumboSGAManager;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones;
using RumboSGAManager.Model.DataContext;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using Rumbo.Core.Herramientas;
using RumboSGA.PedidoCliMotor;
using RumboSGA.Maestros;
using Newtonsoft.Json;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public partial class PedidosProv : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadButton btnVerPendiente = new RadButton();
        public RadButton btnVerRecepciones = new RadButton();
        public RadButton btnVerEntradas = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnVerExistencias = new RadButton();
        public RadButton btnQuitarReservas = new RadButton();
        public RadButton btnCerrarPedido = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridPedidos = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelPedidos = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        public static string q { get; set; }
        public string celda { get; private set; }

        public string dateString { get; set; }
        public string dateStringMañana { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }

        private string referencia;

        private string serie;
        private int tipo;

        public PedidosProv(int queryRecibida)
        {
            this.tipo = queryRecibida;
            this.gridPedidos.Name = "TrazabilidadPedidosProveedor";
            tableLayoutPanelBotones.AutoSize = true;

            tableLayoutPanelPedidos.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelPedidos.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPendiente, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerRecepciones, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntradas, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerExistencias, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnQuitarReservas, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnCerrarPedido, 2, 1);

            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelPedidos.Controls.Add(gridPedidos);

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

            tableLayoutPanelPedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelPedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerPendiente.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnVerRecepciones.Dock = DockStyle.Fill;
            btnVerEntradas.Dock = DockStyle.Fill;
            btnVerExistencias.Dock = DockStyle.Fill;
            btnQuitarReservas.Dock = DockStyle.Fill;
            btnCerrarPedido.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            gridPedidos.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelPedidos.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelPedidos.SetColumnSpan(gridPedidos, 10);

            gridPedidos.MultiSelect = true; // Permite la selección de multiples articulos
            gridPedidos.EnableHotTracking = true;
            gridPedidos.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridPedidos.EnableFiltering = true;
            gridPedidos.MasterTemplate.EnableFiltering = true;
            gridPedidos.MasterTemplate.AllowAddNewRow = false;
            gridPedidos.AllowDragToGroup = false;
            gridPedidos.AllowColumnReorder = false;
            gridPedidos.AllowDeleteRow = false;
            gridPedidos.AllowEditRow = false;
            gridPedidos.BestFitColumns();
            LlenarGrid(queryRecibida);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerPendiente.Text = Lenguaje.traduce(strings.VerPendiente);
            btnVerRecepciones.Text = Lenguaje.traduce(strings.VerRecepciones);
            btnVerEntradas.Text = Lenguaje.traduce(strings.VerEntradas);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);
            btnVerExistencias.Text = Lenguaje.traduce(strings.VerExistencias);
            btnQuitarReservas.Text = Lenguaje.traduce(strings.QuitarReservas);
            btnCerrarPedido.Text = Lenguaje.traduce(strings.CerrarPedido);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerPendiente.Click += new EventHandler(VerPendientes);
            btnVerRecepciones.Click += new EventHandler(VerRecepPedidos);
            btnVerEntradas.Click += new EventHandler(VerEntradas);
            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerSalidas.Click += new EventHandler(VerSalidas);
            btnExportacion.Click += new EventHandler(Exportar);
            btnCerrarPedido.Click += new EventHandler(CerrarPedido);
            btnQuitarReservas.Click += new EventHandler(QuitarReservas);
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
                FuncionesGenerales.guardarLayout(this.gridPedidos, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridPedidos, this.Name);
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

            this.Controls.Add(tableLayoutPanelPedidos);
            FuncionesGenerales.ElegirLayout(this.gridPedidos, this.Name);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void QuitarReservas(object sender, EventArgs e)
        {
            using (WSPedidoCliMotorClient ws = new WSPedidoCliMotorClient())
            {
                try
                {
                    List<CancelarReserva> listaCancelar = new List<CancelarReserva>();
                    string jsonWS = string.Empty;
                    if (gridPedidos.Rows.Count <= 0)
                    {
                        MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                        return;
                    }
                    foreach (var sel in gridPedidos.SelectedRows)
                    {
                        var idPedidoPro = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDPEDIDOPRO")].Value.ToString()); // Tiene que ser variable

                        DataTable reservas = ConexionSQL.SQLClientLoad("SELECT * FROM TBLRESERVAS WHERE IDPEDIDOPRO = " + idPedidoPro);
                        foreach (DataRow row in reservas.Rows)
                        {
                            listaCancelar.Add(new CancelarReserva
                            {
                                idReserva = Convert.ToInt32(row[Lenguaje.traduce("IDRESERVA")]),
                                error = "",
                            });
                        }
                        jsonWS = JsonConvert.SerializeObject(listaCancelar);
                        var respuestaWS = ws.cancelarReserva(jsonWS);
                        MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            };

    }

        private void CerrarPedido(object sender, EventArgs e)
        {
            var idPedidoProv = Convert.ToInt32(gridPedidos.SelectedRows[0].Cells[0].Value.ToString()); // Tiene que ser variable
            string error = "";
            try
            {
                String query = "UPDATE TBLPEDIDOSPROCAB SET IDPEDIDOPROESTADO = 'CA' WHERE IDPEDIDOPRO = " + idPedidoProv;
                ConexionSQL.SQLClienteExec(query, ref error);
                if (error == "")
                {
                    MessageBox.Show(Lenguaje.traduce(error));
                    return;
                }
                LlenarGrid(tipo);
            }
            catch (Exception ex)
            { }
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerRecepPedidos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPedidos.RowCount != 0)
            {
                foreach (var sel in gridPedidos.SelectedRows)
                {
                    celda += "(RC.IDPEDIDOPRO=" + sel.Cells[0].Value.ToString() + " OR RL.IDPEDIDOPRO=" + sel.Cells[0].Value.ToString() + ") OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Recepciones.q != String.Empty)
                    Recepciones.q = "";
                Recepciones.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " LEFT JOIN TBLRECEPCIONESLIN RL ON RL.IDRECEPCION=RC.IDRECEPCION WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Recepciones.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE PC.IDPEDIDOCLI = null";
            }
        }

        private void VerPendientes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPedidos.RowCount != 0)
            {
                foreach (var sel in gridPedidos.SelectedRows)
                {
                    celda += "RL.IDPEDIDOPRO=" + sel.Cells[Lenguaje.traduce("CODIGO PROVEEDOR")].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Generico.q != String.Empty)
                    Generico.q = "";
                Generico.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONESPEDIDOS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Generico.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONESPEDIDOS") + " WHERE RL.IDPEDIDOPRO = null";
            }
        }

        private void VerEntradas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPedidos.RowCount != 0)
            {
                foreach (var sel in gridPedidos.SelectedRows)
                {
                    celda += "E.IDPEDIDOPRO =" + sel.Cells[Lenguaje.traduce("CODIGO PROVEEDOR")].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE E.IDPEDIDOPRO = null";
            }
        }

        private void VerExistencias(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPedidos.RowCount != 0)
            {
                foreach (var sel in gridPedidos.SelectedRows)
                {
                    celda += "E.IDPEDIDOPRO=" + sel.Cells[Lenguaje.traduce("CODIGO PROVEEDOR")].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Existencias.q != String.Empty)
                    Existencias.q = "";
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE E.IDPEDIDOPRO = null";
            }
        }
        
        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPedidos.RowCount != 0)
            {
                foreach (var sel in gridPedidos.SelectedRows)
                {
                    celda += "E.IDPEDIDOPRO =" + sel.Cells[Lenguaje.traduce("CODIGO PROVEEDOR")].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (OrdenesMovimientosSalidaControl.q != String.Empty)
                    OrdenesMovimientosSalidaControl.q = "";
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE E.IDPEDIDOPRO = null";
            }
        }
        
        public void SetQuery(string query)
        {
            q = query;
            gridPedidos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridPedidos);
        }
        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE REPLACE(left(CONVERT(varchar(10),FECHACREACION, 126),10),'-','')='" + DateTime.Today.ToString("yyyyMMdd") + "'";
                        break;
                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE IDPEDIDOPROESTADO<>'PC'";
                        break;
                    case 3:

                        PorNum porNum = new PorNum(gridPedidos, "Pedidos Proveedor por Referencia y Serie");
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE REFERENCIA=null AND SERIE=null";
                        porNum.label1.Text = Lenguaje.traduce("Referencia:");
                        porNum.label2.Text = Lenguaje.traduce("Serie:");
                        porNum.ShowDialog();

                        referencia = porNum.campo1;
                        serie = porNum.campo2;

                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE REFERENCIA LIKE '%" + referencia + "%' AND SERIE LIKE '%" + serie + "%'";
                        gridPedidos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                        
                    case 4:
                        PorFecha porFecha = new PorFecha(gridPedidos, "Por Fecha Prevista Entrada");
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE REPLACE(left(CONVERT(varchar(10),FECHAPREVRECEPCION, 126),10),'-','')=null";

                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE REPLACE(left(CONVERT(varchar(10), FECHAPREVRECEPCION, 126),10),'-','')= '" + dateString + "'";
                            gridPedidos.DataSource = ConexionSQL.getDataTable(q).DefaultView;

                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE REPLACE(left(CONVERT(varchar(10),FECHAPREVRECEPCION, 126),10),'-','')=null";
                        }
                        break;
                    case 5:
                        PorCodigoProveedor porCodigoProveedor = new PorCodigoProveedor(gridPedidos);
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE p.idproveedor=null";
                        break;                    
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridPedidos.DataSource = dt.DefaultView;
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
            this.Name = "Pedidos";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

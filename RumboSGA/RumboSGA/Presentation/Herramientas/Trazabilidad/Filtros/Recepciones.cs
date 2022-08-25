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
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public partial class Recepciones : UserControl
    {
        public RadButton btnVerPendiente = new RadButton();
        public RadButton btnVerPedidosProv = new RadButton();
        public RadButton btnVerEntradas = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnVerExistencias = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridRecepciones = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelRecepciones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        private string recepcion;
        private int tipo;

        public static string q { get; set; }
        public string celda { get; private set; }
        public string dateString { get; set; }
        public string dateStringMañana { get; set; }

        public Recepciones(int tipo)
        {
            this.tipo = tipo;
            this.gridRecepciones.Name = "TrazabilidadRecepciones";
            tableLayoutPanelBotones.AutoSize = true;

            tableLayoutPanelRecepciones.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelRecepciones.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPendiente, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosProv, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntradas, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerExistencias, 1, 1);

            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelRecepciones.Controls.Add(gridRecepciones);

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

            tableLayoutPanelRecepciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelRecepciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerPendiente.Dock = DockStyle.Fill;
            btnVerPedidosProv.Dock = DockStyle.Fill;
            btnVerEntradas.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnVerExistencias.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            gridRecepciones.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelRecepciones.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelRecepciones.SetColumnSpan(gridRecepciones, 10);

            gridRecepciones.MultiSelect = true; // Permite la selección de multiples articulos
            gridRecepciones.EnableHotTracking = true;
            gridRecepciones.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridRecepciones.EnableFiltering = true;
            gridRecepciones.MasterTemplate.EnableFiltering = true;
            gridRecepciones.MasterTemplate.AllowAddNewRow = false;
            gridRecepciones.AllowColumnReorder = false;
            gridRecepciones.AllowDragToGroup = false;
            gridRecepciones.AllowDeleteRow = false;
            gridRecepciones.AllowEditRow = false;
            gridRecepciones.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerPendiente.Text = Lenguaje.traduce(strings.VerPendiente);
            btnVerPedidosProv.Text = Lenguaje.traduce(strings.VerPedidosProv);
            btnVerEntradas.Text = Lenguaje.traduce(strings.VerEntradas);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);
            btnVerExistencias.Text = Lenguaje.traduce(strings.VerExistencias);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerPendiente.Click += new EventHandler(VerPendientes);
            btnVerPedidosProv.Click += new EventHandler(VerPedidosProv);
            btnVerEntradas.Click += new EventHandler(VerEntradas);
            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerSalidas.Click += new EventHandler(VerSalidas);
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
                FuncionesGenerales.guardarLayout(this.gridRecepciones, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridRecepciones, this.Name);
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

            this.Controls.Add(tableLayoutPanelRecepciones);
            FuncionesGenerales.ElegirLayout(this.gridRecepciones, this.Name);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerPedidosProv(object sender, EventArgs e)
        {
            //Evita que al apretar el botón se pueda dar el caso de que después de filtrar no hayamos seleccionado una fila.
            //Si no hemos seleccionado una fila(o varias) el método fallaría sin esta rectificación.
            if (gridRecepciones.RowCount != 0)
            {
                if (gridRecepciones.SelectedRows.Count == 0)
                {
                    this.gridRecepciones.Rows[0].IsSelected = true;
                }
            }

            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRecepciones.RowCount != 0)
            {
                foreach (var sel in gridRecepciones.SelectedRows)
                {
                    celda += "(RC.IDRECEPCION=" + sel.Cells[0].Value.ToString() + " OR RL.IDRECEPCION=" + sel.Cells[0].Value.ToString() + ") OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (PedidosProv.q != String.Empty)
                    PedidosProv.q = "";
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " LEFT JOIN TBLRECEPCIONESLIN RL ON RL.IDPEDIDOPRO=PP.IDPEDIDOPRO LEFT JOIN TBLRECEPCIONESCAB RC ON RC.IDPEDIDOPRO=PP.IDPEDIDOPRO WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE RL.IDRECEPCION = null";
            }
        }

        private void VerPendientes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRecepciones.RowCount != 0)
            {
                foreach (var sel in gridRecepciones.SelectedRows)
                {
                    celda += "RL.IDRECEPCION=" + sel.Cells[0].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Generico.q != String.Empty)
                    Generico.q = "";
                Generico.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONESPEDIDOS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Generico.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONESPEDIDOS") + " WHERE RL.IDRECEPCION = null";
            }
        }

        private void VerEntradas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRecepciones.RowCount != 0)
            {
                foreach (var sel in gridRecepciones.SelectedRows)
                {
                    celda += "E.IDRECEPCION =" + sel.Cells[0].Value.ToString() + " OR ";
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
            if (gridRecepciones.RowCount != 0)
            {
                foreach (var sel in gridRecepciones.SelectedRows)
                {
                    celda += "E.IDRECEPCION=" + sel.Cells[0].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Existencias.q != String.Empty)
                    Existencias.q = "";
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE E.IDRECEPCION = null";
            }
        }

        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRecepciones.RowCount != 0)
            {
                foreach (var sel in gridRecepciones.SelectedRows)
                {
                    celda += "E.IDRECEPCION =" + sel.Cells[0].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (OrdenesMovimientosSalidaControl.q != String.Empty)
                    OrdenesMovimientosSalidaControl.q = "";
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE E.IDRECEPCION = null";
            }
        }
        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridRecepciones);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridRecepciones.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE REPLACE(left(CONVERT(varchar(10),FECHACREACION, 126),10),'-','')='" + DateTime.Today.ToString("yyyyMMdd") + "'";
                        break;
                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE IDRECEPCIONESTADO<>'PC'";
                        break;
                    case 3:
                        PorNum porNum = new PorNum(gridRecepciones, "Recepciones por número");
                        q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE IDRECEPCION=null";
                        porNum.label1.Text = Lenguaje.traduce("Número Recepción:");
                        if (porNum.label2.Text == "")
                        {
                            porNum.label2.Hide();
                            porNum.textBox2.Hide();
                            
                        }
                        porNum.ShowDialog();

                        recepcion = porNum.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE IDRECEPCION='" + recepcion + "'";
                        gridRecepciones.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                    case 4:
                        PorFecha porFecha = new PorFecha(gridRecepciones, "Por Fecha Prevista Entrada");
                        q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE REPLACE(left(CONVERT(varchar(10),FECHARECEPCION, 126),10),'-','')=null";

                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE REPLACE(left(CONVERT(varchar(10), FECHARECEPCION, 126),10),'-','')= '" + dateString + "'";
                            gridRecepciones.DataSource = ConexionSQL.getDataTable(q).DefaultView;

                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE REPLACE(left(CONVERT(varchar(10),FECHARECEPCION, 126),10),'-','')=null";
                        }
                        break;
                    case 5:
                        PorEstadoRecepcion porEstadoRecepcion = new PorEstadoRecepcion(gridRecepciones);
                        q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE IDRECEPCIONESTADO=null";
                        break;
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridRecepciones.DataSource = dt.DefaultView;
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
            // RecepcionesTan
            // 
            this.Name = "Recepciones";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

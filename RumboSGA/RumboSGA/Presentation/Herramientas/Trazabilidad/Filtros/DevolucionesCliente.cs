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
    public partial class DevolucionesCliente : UserControl
    {
        public RadButton btnVerLineas = new RadButton();
        public RadButton btnVerEntradas = new RadButton();
        public RadButton btnVerExistencias = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridDevolucionesCliente = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelDevCliente = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        private string id;
        private int tipo;
        public static string q { get; set; }
        public string celda { get; private set; }
        public string dateString { get; set; }
        public string dateStringMañana { get; set; }

        public DevolucionesCliente(int tipo)
        {
            this.tipo = tipo;
            gridDevolucionesCliente.Name = "TrazabilidadDevolucionesCliente";
            tableLayoutPanelBotones.AutoSize = true;

            tableLayoutPanelDevCliente.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelDevCliente.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerLineas, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntradas, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerExistencias, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 1, 1);

            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelDevCliente.Controls.Add(gridDevolucionesCliente);

            tableLayoutPanelBotones.RowCount = 2;
            tableLayoutPanelBotones.ColumnCount = 2;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelDevCliente.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelDevCliente.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerLineas.Dock = DockStyle.Fill;
            btnVerEntradas.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnVerExistencias.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            gridDevolucionesCliente.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelDevCliente.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelDevCliente.SetColumnSpan(gridDevolucionesCliente, 10);

            gridDevolucionesCliente.MultiSelect = true; // Permite la selección de multiples articulos
            gridDevolucionesCliente.EnableHotTracking = true;
            gridDevolucionesCliente.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridDevolucionesCliente.EnableFiltering = true;
            gridDevolucionesCliente.MasterTemplate.EnableFiltering = true;
            gridDevolucionesCliente.MasterTemplate.AllowAddNewRow = false;
            gridDevolucionesCliente.AllowColumnReorder = false;
            gridDevolucionesCliente.AllowDragToGroup = false;
            gridDevolucionesCliente.AllowDeleteRow = false;
            gridDevolucionesCliente.AllowEditRow = false;
            gridDevolucionesCliente.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerLineas.Text = Lenguaje.traduce("Ver Lineas");
            btnVerEntradas.Text = Lenguaje.traduce(strings.VerEntradas);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);
            btnVerExistencias.Text = Lenguaje.traduce(strings.VerExistencias);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnExportacion.Click += new EventHandler(Exportar);
            btnRefrescar.Click += new EventHandler(Refrescar);
            btnVerEntradas.Click += new EventHandler(VerEntradas);
            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerLineas.Click += new EventHandler(VerLineas);
            btnVerSalidas.Click += new EventHandler(VerSalidas);

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
                FuncionesGenerales.guardarLayout(this.gridDevolucionesCliente, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridDevolucionesCliente, this.Name);
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

            this.Controls.Add(tableLayoutPanelDevCliente);
            FuncionesGenerales.ElegirLayout(this.gridDevolucionesCliente, this.Name);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerLineas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridDevolucionesCliente.RowCount != 0)
            {
                foreach (var sel in gridDevolucionesCliente.SelectedRows)
                {
                    celda += "DL.IDDEVOLCLI =" + sel.Cells[0].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Generico.q != String.Empty)
                    Generico.q = "";
                Generico.q = ConfigurarSQL.CargarConsulta(path, "LINEASDEVOLUCION") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Generico.q = ConfigurarSQL.CargarConsulta(path, "LINEASDEVOLUCION") + " WHERE DL.IDDEVOLCLI = null";
            }
        }

        private void VerEntradas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridDevolucionesCliente.RowCount != 0)
            {
                foreach (var sel in gridDevolucionesCliente.SelectedRows)
                {
                    celda += "E.IDDEVOLCLI =" + sel.Cells[0].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE E.IDDEVOLCLI = null";
            }
        }

        private void VerExistencias(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridDevolucionesCliente.RowCount != 0)
            {
                foreach (var sel in gridDevolucionesCliente.SelectedRows)
                {
                    celda += "E.IDDEVOLCLI=" + sel.Cells[0].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Existencias.q != String.Empty)
                    Existencias.q = "";
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE E.IDDEVOLCLI = null";
            }
        }

        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridDevolucionesCliente.RowCount != 0)
            {
                foreach (var sel in gridDevolucionesCliente.SelectedRows)
                {
                    celda += "E.IDDEVOLCLI =" + sel.Cells[0].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (OrdenesMovimientosSalidaControl.q != String.Empty)
                    OrdenesMovimientosSalidaControl.q = "";
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE E.IDDEVOLCLI = null";
            }
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridDevolucionesCliente);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridDevolucionesCliente.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE REPLACE(left(CONVERT(varchar(10),DC.FECHACREACION, 126),10),'-','')='" + DateTime.Today.ToString("yyyyMMdd") + "'";
                        break;
                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE DC.IDDEVOLCLIESTADO<>'PC'";
                        break;
                    case 3:
                        PorNum porNum = new PorNum(gridDevolucionesCliente, "Por número de devolución proveedor");
                        q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE DC.IDDEVOLCLIESTADO=null";
                        porNum.label1.Text = Lenguaje.traduce("Número Devolución Proveedor:");
                        if (porNum.label2.Text == "")
                        {
                            porNum.label2.Hide();
                            porNum.textBox2.Hide();

                        }
                        porNum.ShowDialog();

                        id = porNum.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE DC.IDDEVOLCLI='" + id + "'";
                        gridDevolucionesCliente.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                    case 4:
                        PorFecha porFecha = new PorFecha(gridDevolucionesCliente, "Por Fecha Prevista Devolución");
                        q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE REPLACE(left(CONVERT(varchar(10), DC.FECHADEV, 126),10),'-','')=null";

                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE REPLACE(left(CONVERT(varchar(10), DC.FECHADEV, 126),10),'-','')= '" + dateString + "'";
                            gridDevolucionesCliente.DataSource = ConexionSQL.getDataTable(q).DefaultView;

                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE REPLACE(left(CONVERT(varchar(10), DC.FECHADEV, 126),10),'-','')=null";
                        }

                        break;
                    case 5:
                        PorEstadoDevCliente porEstadoDevCliente = new PorEstadoDevCliente(gridDevolucionesCliente, "Devoluciones Clientes por estado");
                        q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE DC.IDDEVOLCLIESTADO=null";
                        break;
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridDevolucionesCliente.DataSource = dt.DefaultView;
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
            // DevolucionesClienteTan
            // 
            this.Name = "DevolucionesCliente";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

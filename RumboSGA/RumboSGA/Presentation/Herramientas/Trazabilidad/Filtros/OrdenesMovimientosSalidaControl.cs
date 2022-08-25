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
using RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using Rumbo.Core.Herramientas;
using RumboSGAManager.Model;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class OrdenesMovimientosSalidaControl : UserControl
    {
        public Panel paddingPanel = new Panel();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public RadButton btnVerContenido = new RadButton();
        public RadButton btnVerPedidos = new RadButton();
        public RadButton btnVerOrdenes = new RadButton();
        public RadButton btnVerCarga = new RadButton();
        public RadButton btnVerEntrada = new RadButton();
        public RadButton btnAbonarUnidades = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridMovSalida = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelMovimientosSalida = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private string refArticulo;
        private int tipo;

        public static string q { get; set; }
        public string celda { get; private set; }

        public OrdenesMovimientosSalidaControl(int tipo)
        {
            this.tipo = tipo;
            tableLayoutPanelMovimientosSalida.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelMovimientosSalida.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            //tableLayoutPanelBotones.Controls.Add(btnVerContenido, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidos, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenes, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerCarga, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntrada, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnAbonarUnidades, 1, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelMovimientosSalida.Controls.Add(gridMovSalida);

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

            tableLayoutPanelMovimientosSalida.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelMovimientosSalida.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerContenido.Dock = DockStyle.Fill;
            btnVerPedidos.Dock = DockStyle.Fill;
            btnVerOrdenes.Dock = DockStyle.Fill;
            btnVerCarga.Dock = DockStyle.Fill;
            btnVerEntrada.Dock = DockStyle.Fill;
            btnAbonarUnidades.Dock = DockStyle.Fill;
            gridMovSalida.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelMovimientosSalida.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelMovimientosSalida.SetColumnSpan(gridMovSalida, 10);

            gridMovSalida.MultiSelect = true; // Permite la selección de multiples articulos
            gridMovSalida.EnableHotTracking = true;
            gridMovSalida.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridMovSalida.EnableFiltering = true;
            gridMovSalida.MasterTemplate.EnableFiltering = true;
            gridMovSalida.MasterTemplate.AllowAddNewRow = false;
            gridMovSalida.AllowColumnReorder = false;
            gridMovSalida.AllowDragToGroup = false;
            gridMovSalida.AllowDeleteRow = false;
            gridMovSalida.AllowEditRow = false;
            gridMovSalida.Name = "gridMovSalida";
            gridMovSalida.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerContenido.Text = Lenguaje.traduce(strings.VerContenido);
            btnVerPedidos.Text = Lenguaje.traduce(strings.VerPedidos);
            btnVerOrdenes.Text = Lenguaje.traduce(strings.VerOrdenes);
            btnVerCarga.Text = Lenguaje.traduce(strings.VerCarga);
            btnVerEntrada.Text = Lenguaje.traduce(strings.VerEntradas);
            btnAbonarUnidades.Text = Lenguaje.traduce(strings.AbonarUnidades);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerOrdenes.Click += new EventHandler(VerOrdenes);
            btnVerPedidos.Click += new EventHandler(VerPedidos);
            //btnVerContenido.Click += new EventHandler(VerContenido);
            btnVerCarga.Click += new EventHandler(VerCargaCamion);
            btnVerEntrada.Click += new EventHandler(VerEntrada);
            btnAbonarUnidades.Click += new EventHandler(AbonarUnidades);
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
                FuncionesGenerales.guardarLayout(this.gridMovSalida, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridMovSalida, this.Name);
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

            this.Controls.Add(tableLayoutPanelMovimientosSalida);
        }

        private void AbonarUnidades(object sender, EventArgs e)
        {
            if (gridMovSalida.SelectedRows.Count != 1)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                var idsalida = Convert.ToInt32(gridMovSalida.SelectedRows[0].Cells[Lenguaje.traduce("IDSALIDA")].Value.ToString());
                var idusuario = User.IdUsuario;
                var idoperario = User.IdOperario;

                AbonarUnidadesRegSalida form = new AbonarUnidadesRegSalida(idsalida, idoperario, idusuario);
                form.Show();
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
            ExportHelper.ExportarExcel(gridMovSalida);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridMovSalida.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void VerPedidos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMovSalida.RowCount != 0)
            {
                foreach (var sel in gridMovSalida.SelectedRows)
                {
                    celda += "IDENTIFICADORPL = " + sel.Cells[Lenguaje.traduce("IDENTIFICADORPL")].Value.ToString() + " OR ";
                }
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE IDENTIFICADORPL = null";
            }
        }

        /*private void VerContenido(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMovSalida.RowCount != 0)
            {
                foreach (var sel in gridMovSalida.SelectedRows)
                {
                    celda += "((IDENTIFICADORPL = '" + sel.Cells[13].Value.ToString() + "') OR (IDENTIFICADORPL  IN(SELECT PL.IDENTIFICADOR FROM TBLPACKINGLIST PL JOIN TBLPACKINGLIST PP ON PP.IDPACKINGLIST = PL.IDPACKINGLISTPADRE WHERE PP.IDENTIFICADOR = '" + sel.Cells[14].Value.ToString() + "'))) OR ";
                }
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                celda = celda.Remove(celda.Length - 3);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE IDENTIFICADORPL = null";
            }
        }*/

        private void VerOrdenes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMovSalida.RowCount != 0)
            {
                foreach (var sel in gridMovSalida.SelectedRows)
                {
                    celda += "IDENTIFICADORPL = " + sel.Cells[Lenguaje.traduce("IDENTIFICADORPL")].Value.ToString() + " OR ";
                }
                if (OrdenesRecogidaControl.q != String.Empty)
                    OrdenesRecogidaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE IDENTIFICADORPL = null";
            }
        }

        private void VerCargaCamion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMovSalida.RowCount != 0)
            {
                foreach (var sel in gridMovSalida.SelectedRows)
                {
                    celda += "CB.IDCARGA = " + sel.Cells[Lenguaje.traduce("IDENTIFICADORPL")].Value.ToString() + " OR ";
                }
                if (OrdenesCargaControl.q != String.Empty)
                    OrdenesCargaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE " + celda;
            }            
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE CB.IDCARGA = null";
            }
        }

        private void VerEntrada(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMovSalida.RowCount != 0)
            {
                foreach (var sel in gridMovSalida.SelectedRows)
                {
                    celda += "S.IDENTRADA = '" + sel.Cells[1].Value.ToString() + "' OR ";
                }
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                celda = celda.Remove(celda.Length - 3);
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }            
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE S.IDENTRADA = null";
            }
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 0:
                        if (q.Equals(""))
                            q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA");                        
                        break;
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE S.FECHA >='" + DateTime.Today.ToString("yyyyMMdd") + "' AND S.FECHA <'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;
                    case 2: //Por número de referencia del artículo
                        PorNum porNum = new PorNum(gridMovSalida, "Movimientos salida por nºartículo");
                        q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE A.REFERENCIA=null";
                        porNum.label1.Text = Lenguaje.traduce("Número Referencia Artículo:");
                        if (porNum.label2.Text == "")
                        {
                            porNum.label2.Hide();
                            porNum.textBox2.Hide();

                        }
                        porNum.ShowDialog();

                        refArticulo = porNum.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE A.REFERENCIA ='" + refArticulo + "'";
                        gridMovSalida.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                    case 3:
                        PorArticuloLote porArticuloLote = new PorArticuloLote(5, gridMovSalida);
                        porArticuloLote.Show();
                        q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE A.REFERENCIA=null";
                        break;
                    case 4:                        
                        EntreFechas entreFechas = new EntreFechas(gridMovSalida, Lenguaje.traduce("Salidas Entre Fechas"));
                        entreFechas.Show();
                        q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE A.REFERENCIA=null";
                        break;
                    default:
                        q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridMovSalida.DataSource = dt.DefaultView;
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
            // OrdenesRecogidaControl
            // 
            this.Name = "OrdenesMovimientosSalidas";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

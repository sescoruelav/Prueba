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
    public partial class Preavisos : UserControl
    {
        public RadButton btnVerEntradas = new RadButton();
        public RadButton btnVerRecepciones = new RadButton();
        public RadButton btnVerPedidosProv = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridPreavisos = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelPreavisos = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private string SSCC;

        public static string q { get; set; }
        public string celda { get; private set; }

        public string dateString { get; set; }
        public string dateStringMañana { get; set; }

        public string campo1 { get; set; }
        public string campo2 { get; set; }

        private string referencia;

        private string serie;
        private int tipo;

        public Preavisos(int tipo)
        {
            this.tipo = tipo;
            this.gridPreavisos.Name = "TrazabilidadPreavisos";
            tableLayoutPanelPreavisos.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelPreavisos.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerEntradas, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerRecepciones, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosProv, 2, 0);


            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelPreavisos.Controls.Add(gridPreavisos);

            tableLayoutPanelBotones.RowCount = 1;
            tableLayoutPanelBotones.ColumnCount = 3;


            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelPreavisos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelPreavisos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerEntradas.Dock = DockStyle.Fill;
            btnVerRecepciones.Dock = DockStyle.Fill;
            btnVerPedidosProv.Dock = DockStyle.Fill;


            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            gridPreavisos.Dock = DockStyle.Fill;

            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelPreavisos.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelPreavisos.SetColumnSpan(gridPreavisos, 10);

            gridPreavisos.MultiSelect = true; // Permite la selección de multiples articulos
            gridPreavisos.EnableHotTracking = true;
            gridPreavisos.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridPreavisos.EnableFiltering = true;
            gridPreavisos.MasterTemplate.EnableFiltering = true;
            gridPreavisos.MasterTemplate.AllowAddNewRow = false;
            gridPreavisos.AllowDragToGroup = false;
            gridPreavisos.AllowColumnReorder = false;
            gridPreavisos.AllowDeleteRow = false;
            gridPreavisos.AllowEditRow = false;
            gridPreavisos.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerRecepciones.Text = Lenguaje.traduce(strings.VerRecepciones);
            btnVerPedidosProv.Text = Lenguaje.traduce(strings.VerPedidosProv);
            btnVerEntradas.Text = Lenguaje.traduce(strings.VerEntradas);


            btnVerSql.Click += new EventHandler(VistaSQL);
            btnExportacion.Click += new EventHandler(Exportar);
            btnVerEntradas.Click += new EventHandler(VerEntrada);
            btnVerRecepciones.Click += new EventHandler(VerRecepciones);
            btnVerPedidosProv.Click += new EventHandler(VerPedidosProv);
            btnRefrescar.Click += new EventHandler(Refrescar);

            // btnConfig
            this.btnConfig.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnConfig.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnConfig.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConfig.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.idiomasMenuItem,
            this.btnVerSql}); FuncionesGenerales.AddGuardarYCargarLayoutEnRadMenu(ref this.btnConfig);
            this.btnConfig.Items["RadMenuItemGuardarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.guardarLayout(this.gridPreavisos, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridPreavisos, this.Name);
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

            this.Controls.Add(tableLayoutPanelPreavisos);
            FuncionesGenerales.ElegirLayout(this.gridPreavisos, this.Name);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerEntrada(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPreavisos.RowCount != 0)
            {
                foreach (var sel in gridPreavisos.SelectedRows)
                {
                    celda += "(E.IDPEDIDOPRO=" + sel.Cells[1].Value.ToString() + " AND E.IDPEDIDOPROLIN=" + sel.Cells[4].Value.ToString() + ") OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesRecogidaControl.q = null;
            }
        }
        private void VerRecepciones(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPreavisos.RowCount != 0)
            {
                foreach (var sel in gridPreavisos.SelectedRows)
                {
                    celda += "IDPEDIDOPRO=" + sel.Cells[1].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Recepciones.q != String.Empty)
                    Recepciones.q = "";
                Recepciones.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE (RC.IDRECEPCION IN (SELECT IDRECEPCION FROM dbo.TBLRECEPCIONESLIN WHERE " + celda + "))";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Recepciones.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE IDPEDIDOPRO = null";
            }
        }
        private void VerPedidosProv(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPreavisos.RowCount != 0)
            {
                foreach (var sel in gridPreavisos.SelectedRows)
                {
                    celda += "PP.IDPEDIDOPRO = " + sel.Cells[1].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (PedidosProv.q != String.Empty)
                    PedidosProv.q = "";
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE PP.IDPEDIDOPRO = null";
            }
        }


        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridPreavisos);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridPreavisos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + " WHERE PP.FECHACREACION='" + DateTime.Today.ToString("yyyyMMdd") + "'";
                        break;
                    case 2:

                        PorNum porNum = new PorNum(gridPreavisos, "Pedidos Preavisos por Referencia y Serie");
                        q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + " WHERE PP.REFERENCIA=null AND PP.SERIE=null";
                        porNum.label1.Text = Lenguaje.traduce("Referencia:");
                        porNum.label2.Text = Lenguaje.traduce("Serie:");
                        porNum.ShowDialog();

                        referencia = porNum.campo1;
                        serie = porNum.campo2;

                        q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + " WHERE PP.REFERENCIA='" + referencia + "' AND PP.SERIE='" + serie + "'";
                        gridPreavisos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;
                    case 3:
                        PorFecha porFecha = new PorFecha(gridPreavisos, "PorFechaCreacionPreavisos");
                        q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + "  WHERE PP.FECHACREACION=null";

                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + " WHERE PP.FECHACREACION='" + dateString + "'"; 
                            gridPreavisos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + "  WHERE PP.FECHACREACION=null";
                        }
                        
                        break;
                    case 4:
                        PorNum porNumEtiqueta = new PorNum(gridPreavisos, "Por número de etiqueta SSCC");
                        q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + " WHERE T.ETIQUETA=null";
                        porNumEtiqueta.label1.Text = Lenguaje.traduce("Número Etiqueta SSCC:");
                        if (porNumEtiqueta.label2.Text == "")
                        {
                            porNumEtiqueta.label2.Hide();
                            porNumEtiqueta.textBox2.Hide();

                        }
                        porNumEtiqueta.ShowDialog();

                        SSCC = porNumEtiqueta.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS") + " WHERE T.ETIQUETA='" + SSCC + "'";
                        gridPreavisos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        
                        break;
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "PREAVISOS");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridPreavisos.DataSource = dt.DefaultView;
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
            // PreavisosTan
            // 
            this.Name = "Preavisos";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

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
using RumboSGA.SalidaMotor;
using RumboSGAManager.Model;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class OrdenesRecogidaControl : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadButton btnVerPacking = new RadButton();
        public RadButton btnVerPedidos = new RadButton();
        public RadButton btnVerPedidosIncluidos = new RadButton();
        public RadButton btnVerContenido = new RadButton();
        public RadButton btnVerCarga = new RadButton();
        public RadButton btnImpContOrdenRec = new RadButton();
        public RadButton btnImpDiscrepanciasOrdenRec = new RadButton();
        public RadButton btnCambiarEstadoOrdenRec = new RadButton();
        public RadButton btnImpEtiqNomPLCliente = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridOrdenRec = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelOrdenesRec = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private string orden;
        private int tipo;

        public static string q { get; set; }
        public string celda { get; set; }

        public string dateString { get; set; }
        public string dateStringMañana { get; set; }

        public OrdenesRecogidaControl(int tipo)
        {
            this.tipo = tipo;
            this.gridOrdenRec.Name = "TrazabilidadOrdenrecogida";
            tableLayoutPanelOrdenesRec.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelOrdenesRec.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPacking, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidos, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosIncluidos, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerContenido, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerCarga, 4, 0);
            tableLayoutPanelBotones.Controls.Add(btnImpContOrdenRec, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnImpDiscrepanciasOrdenRec, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnCambiarEstadoOrdenRec, 2, 1);
            tableLayoutPanelBotones.Controls.Add(btnImpEtiqNomPLCliente, 3, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelOrdenesRec.Controls.Add(gridOrdenRec);

            tableLayoutPanelBotones.RowCount = 2;
            tableLayoutPanelBotones.ColumnCount = 6;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelOrdenesRec.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelOrdenesRec.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerPacking.Dock = DockStyle.Fill;
            btnVerPedidos.Dock = DockStyle.Fill;
            btnVerPedidosIncluidos.Dock = DockStyle.Fill;
            btnVerContenido.Dock = DockStyle.Fill;
            btnVerCarga.Dock = DockStyle.Fill;
            btnImpContOrdenRec.Dock = DockStyle.Fill;
            btnImpDiscrepanciasOrdenRec.Dock = DockStyle.Fill;
            btnCambiarEstadoOrdenRec.Dock = DockStyle.Fill;
            btnImpEtiqNomPLCliente.Dock = DockStyle.Fill;
            gridOrdenRec.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelOrdenesRec.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelOrdenesRec.SetColumnSpan(gridOrdenRec, 10);

            gridOrdenRec.MultiSelect = true; // Permite la selección de multiples articulos
            gridOrdenRec.EnableHotTracking = true;
            gridOrdenRec.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridOrdenRec.EnableFiltering = true;
            gridOrdenRec.MasterTemplate.EnableFiltering = true;
            gridOrdenRec.MasterTemplate.AllowAddNewRow = false;
            gridOrdenRec.AllowDragToGroup = false;
            gridOrdenRec.AllowColumnReorder = false;
            gridOrdenRec.AllowDeleteRow = false;
            gridOrdenRec.AllowEditRow = false;
            gridOrdenRec.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerPacking.Text = Lenguaje.traduce(strings.VerPacking);
            btnVerPedidos.Text = Lenguaje.traduce(strings.VerPedidos);
            btnVerPedidosIncluidos.Text = Lenguaje.traduce(strings.VerPedidosIncluidos);
            btnVerContenido.Text = Lenguaje.traduce(strings.VerContenido);
            btnVerCarga.Text = Lenguaje.traduce(strings.VerCarga);
            btnImpContOrdenRec.Text = Lenguaje.traduce(strings.ImpContOrdenRec);
            btnImpDiscrepanciasOrdenRec.Text = Lenguaje.traduce(strings.ImpDiscrepanciasOrdenRec);
            btnCambiarEstadoOrdenRec.Text = Lenguaje.traduce("Cambiar Estado");
            btnImpEtiqNomPLCliente.Text = Lenguaje.traduce(strings.ImpEtiqNomPLCliente);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerPacking.Click += new EventHandler(VerPacking);
            btnVerPedidos.Click += new EventHandler(VerPedidos);
            btnVerPedidosIncluidos.Click += new EventHandler(VerPedidosIncluidos);
            btnVerContenido.Click += new EventHandler(VerContenido);
            btnVerCarga.Click += new EventHandler(VerCargaCamion);
            btnImpContOrdenRec.Click += new EventHandler(ImprimirContOrdenRec);
            btnImpDiscrepanciasOrdenRec.Click += new EventHandler(ImprimirDiscrepanciasOrdenRec);
            btnCambiarEstadoOrdenRec.Click += new EventHandler(CambiarEstadoOrdenRec);
            btnImpEtiqNomPLCliente.Click += new EventHandler(ImprimirEtiquetaNomPLCliente);
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
                FuncionesGenerales.guardarLayout(this.gridOrdenRec, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridOrdenRec, this.Name);
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
            this.idiomasMenuItem.Text = "Idiomas";
            this.idiomasMenuItem.UseCompatibleTextRendering = false;

            this.btnVerSql.Name = "btnVerSql";
            this.btnVerSql.Text = strings.VerSql;
            this.btnVerSql.UseCompatibleTextRendering = false;
            InitializeThemesDropDown();

            this.btnExportacion.Size = new Size(50, 50);
            this.btnExportacion.Image = Resources.ExportToExcel;
            this.btnExportacion.ImageAlignment = ContentAlignment.MiddleCenter;

            this.btnRefrescar.Size = new Size(50, 50);
            this.btnRefrescar.Image = Resources.Refresh;
            this.btnRefrescar.ImageAlignment = ContentAlignment.MiddleCenter;

            this.Controls.Add(tableLayoutPanelOrdenesRec);
            FuncionesGenerales.ElegirLayout(this.gridOrdenRec, this.Name);
        }

        private void CambiarEstadoOrdenRec(object sender, EventArgs e)
        {
            if (gridOrdenRec.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                List<int> ordenes = new List<int>();
                foreach (var row in gridOrdenRec.SelectedRows)
                {
                    ordenes.Add(Convert.ToInt32(row.Cells[Lenguaje.traduce("IDORDENRECOGIDA")].Value.ToString()));
                }
                CambiarEstadoOrdRec form = new CambiarEstadoOrdRec(ordenes.ToArray());
                form.Show();
            }
            catch (Exception)
            {
                MessageBox.Show(strings.ErrorGenerico);
                throw;
            }
        }

        private void ImprimirEtiquetaNomPLCliente(object sender, EventArgs e)
        {
            if (gridOrdenRec.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridOrdenRec.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDORDENRECOGIDA")].Value.ToString());

                    var ws = new WSSalidaMotorClient();
                    ws.imprimirPackingListNominativoPorCliente(id, User.NombreImpresora);
                }
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a la impresora "), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show(strings.ErrorGenerico);
                throw;
            }
        }

        private void ImprimirDiscrepanciasOrdenRec(object sender, EventArgs e)
        {
            try
            {
                if (gridOrdenRec.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                var parametros = string.Empty;
                foreach (var row in gridOrdenRec.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDORDENRECOGIDA")].Value.ToString());
                    parametros += "ORDEN=" + id + " OR ";
                }
                parametros = parametros.Remove(parametros.Length - 4);
                int idInforme = 16900;
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

        private void ImprimirContOrdenRec(object sender, EventArgs e)
        {
            try
            {
                if (gridOrdenRec.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                var parametros = string.Empty;
                foreach (var row in gridOrdenRec.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDORDENRECOGIDA")].Value.ToString());
                    parametros += "IDORDENRECOGIDA=" + id + " OR ";
                }
                parametros = parametros.Remove(parametros.Length - 4);
                int idInforme = 16800;
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

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerPacking(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenRec.RowCount != 0)
            {
                foreach (var sel in gridOrdenRec.SelectedRows)
                {
                    celda += "(PL.IDENTIFICADOR = '" + sel.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString() + "' AND S.IDORDENRECOGIDA = '" + sel.Cells[Lenguaje.traduce("IDORDENRECOGIDA")].Value.ToString() + "') OR ";
                }
                if (Packing.q != String.Empty)
                    Packing.q = "";
                celda = celda.Remove(celda.Length - 3);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PC.IDPEDIDOCLI = null";
            }
        }

        private void VerPedidos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenRec.RowCount != 0)
            {
                foreach (var sel in gridOrdenRec.SelectedRows)
                {
                    celda += "(PL.IDENTIFICADOR = '" + sel.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString() + "' AND S.IDORDENRECOGIDA = '" + sel.Cells[2].Value.ToString() + "') OR ";
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

        private void VerPedidosIncluidos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenRec.RowCount != 0)
            {
                foreach (var sel in gridOrdenRec.SelectedRows)
                {
                    celda += "OL.IDORDENRECOGIDA = '" + sel.Cells[Lenguaje.traduce("IDORDENRECOGIDA")].Value.ToString() + "' OR ";
                }
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE OL.IDORDENRECOGIDA = null";
            }
        }

        private void VerContenido(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenRec.RowCount != 0)
            {
                foreach (var sel in gridOrdenRec.SelectedRows)
                {
                    celda += "((IDENTIFICADORPL = '" + sel.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString() + "') OR(IDENTIFICADORPL IN (SELECT PL.IDENTIFICADOR FROM TBLPACKINGLIST PL JOIN TBLPACKINGLIST PP ON PP.IDPACKINGLIST = PL.IDPACKINGLISTPADRE WHERE PP.IDENTIFICADOR = '" + sel.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString() + "'))) OR ";
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
        }

        private void VerCargaCamion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridOrdenRec.RowCount != 0)
            {
                foreach (var sel in gridOrdenRec.SelectedRows)
                {
                    celda += "(PL.IDENTIFICADOR = '" + sel.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString() + "' AND S.IDORDENRECOGIDA = '" + sel.Cells[Lenguaje.traduce("IDORDENRECOGIDA")].Value.ToString() + "') OR ";
                }
                if (OrdenesCargaControl.q != String.Empty)
                    OrdenesCargaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE PL.IDENTIFICADOR = null";
            }
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridOrdenRec);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridOrdenRec.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.FECHACREACION>='" + DateTime.Today.ToString("yyyyMMdd") + "' AND OC.FECHACREACION<'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;

                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.IDORDENESTADO='PK'";
                        break;

                    case 3: //NÚMERO DE ORDEN
                        PorNum porNum = new PorNum(gridOrdenRec, "Por número de orden de recogida");
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.IDORDENRECOGIDA=null";
                        porNum.label1.Text = Lenguaje.traduce("Id Orden Recogida:");
                        if (porNum.label2.Text == "")
                        {
                            porNum.label2.Hide();
                            porNum.textBox2.Hide();
                        }
                        porNum.ShowDialog();

                        orden = porNum.campo1;

                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.IDORDENRECOGIDA='" + orden + "'";
                        gridOrdenRec.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;

                    case 4: //POR ESTADO
                        PorEstadoOrdRec porEstadoOrdRecorNumOrden = new PorEstadoOrdRec(gridOrdenRec);
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.IDORDENRECOGIDA=null";
                        break;

                    case 5: //POR FECHA
                        PorFecha porFecha = new PorFecha(gridOrdenRec, "Órdenes Recogida por Fecha");
                        q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.IDORDENRECOGIDA=null";
                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.FECHACREACION>='" + dateString + "' AND OC.FECHACREACION < '" + dateStringMañana + "'";
                            gridOrdenRec.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE OC.IDORDENRECOGIDA=null";
                        }
                        break;

                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridOrdenRec.DataSource = dt.DefaultView;
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
            // OrdenesRecogidaControl
            //
            this.Name = "OrdenesRecogidaControl";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);
        }
    }
}
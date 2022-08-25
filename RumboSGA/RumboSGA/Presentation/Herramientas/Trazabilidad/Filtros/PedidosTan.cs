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
using RumboSGA.PedidoCliMotor;
using RumboSGAManager.Model;
using RumboSGA.Maestros;
using Newtonsoft.Json;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class PedidosTan : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadButton btnVerPacking = new RadButton();
        public RadButton btnImpContPedidoCli = new RadButton();
        public RadButton btnImpDiscrepanciasPedido = new RadButton();
        public RadButton btnImpPackingPedidoCli = new RadButton();
        public RadButton btnImpEtiqEnvio = new RadButton();
        public RadButton btnImpEtiqCarga = new RadButton();
        public RadButton btnQuitarReservas = new RadButton();
        public RadButton btnCerrarPedido = new RadButton();
        public RadButton btnCancelarPedido = new RadButton();
        public RadButton btnVerOrdenes = new RadButton();
        public RadButton btnVerContenido = new RadButton();
        public RadButton btnVerCarga = new RadButton();
        public RadButton btnFiltrar = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridPedidos = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSqlPedidos = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelTabPedidos = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        public static string q { get; set; }
        public string celda { get; set; }
        public string dateString { get; set; }
        public string dateStringMañana { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }

        public int tipo;

        private string referencia;

        private string serie;

        public PedidosTan(int tipo)
        {
            this.tipo = tipo;
            gridPedidos.Name = "TrazabilidadPedidosCliente";

            tableLayoutPanelBotones.AutoSize = true;

            tableLayoutPanelTabPedidos.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelTabPedidos.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPacking, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenes, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerContenido, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerCarga, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnImpContPedidoCli, 4, 0);
            tableLayoutPanelBotones.Controls.Add(btnImpDiscrepanciasPedido, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnImpPackingPedidoCli, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnImpEtiqEnvio, 2, 1);
            tableLayoutPanelBotones.Controls.Add(btnQuitarReservas, 3, 1);
            tableLayoutPanelBotones.Controls.Add(btnCancelarPedido, 4, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelTabPedidos.Controls.Add(gridPedidos);

            tableLayoutPanelBotones.RowCount = 2;
            tableLayoutPanelBotones.ColumnCount = 5;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelTabPedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelTabPedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerPacking.Dock = DockStyle.Fill;
            btnVerOrdenes.Dock = DockStyle.Fill;
            btnVerContenido.Dock = DockStyle.Fill;
            btnVerCarga.Dock = DockStyle.Fill;
            btnImpContPedidoCli.Dock = DockStyle.Fill;
            btnImpDiscrepanciasPedido.Dock = DockStyle.Fill;
            btnImpPackingPedidoCli.Dock = DockStyle.Fill;
            btnImpEtiqEnvio.Dock = DockStyle.Fill;
            btnQuitarReservas.Dock = DockStyle.Fill;
            btnCancelarPedido.Dock = DockStyle.Fill;
            btnFiltrar.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            gridPedidos.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelTabPedidos.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelTabPedidos.SetColumnSpan(gridPedidos, 10);

            gridPedidos.MultiSelect = true; // Permite la selección de multiples articulos
            gridPedidos.EnableHotTracking = true;
            gridPedidos.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridPedidos.EnableFiltering = true;
            gridPedidos.MasterTemplate.EnableFiltering = true;
            gridPedidos.MasterTemplate.AllowAddNewRow = false;
            gridPedidos.AllowColumnReorder = false;
            gridPedidos.AllowDragToGroup = false;
            gridPedidos.AllowDeleteRow = false;
            gridPedidos.AllowEditRow = false;
            gridPedidos.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSqlPedidos.Text = Lenguaje.traduce(strings.VerSql);
            btnVerPacking.Text = Lenguaje.traduce(strings.VerPacking);
            btnVerOrdenes.Text = Lenguaje.traduce(strings.VerOrdenes);
            btnVerContenido.Text = Lenguaje.traduce(strings.VerContenido);
            btnVerCarga.Text = Lenguaje.traduce(strings.VerCarga);
            btnImpContPedidoCli.Text = Lenguaje.traduce(strings.ImpContPedidoCli);
            btnImpDiscrepanciasPedido.Text = Lenguaje.traduce(strings.ImpDiscrepancias);
            btnImpPackingPedidoCli.Text = Lenguaje.traduce(strings.ImpPackingPedidoCli);
            btnImpEtiqEnvio.Text = Lenguaje.traduce(strings.ImpEtiqEnvio);
            btnQuitarReservas.Text = Lenguaje.traduce(strings.QuitarReservas);
            btnCancelarPedido.Text = Lenguaje.traduce(strings.CancelarPedido);

            btnVerSqlPedidos.Click += new EventHandler(VistaSQL);
            btnVerOrdenes.Click += new EventHandler(VerOrdenes);
            btnVerPacking.Click += new EventHandler(VerPacking);
            btnVerContenido.Click += new EventHandler(VerContenido);
            btnVerCarga.Click += new EventHandler(VerCargaCamion);
            btnExportacion.Click += new EventHandler(Exportar);
            btnImpContPedidoCli.Click += new EventHandler(ImprimirInformeContPedidoCli);
            btnImpDiscrepanciasPedido.Click += new EventHandler(ImprimirInformeDiscrepanciasPedido);
            btnImpPackingPedidoCli.Click += new EventHandler(ImprimirInformePackingPedidoCli);
            btnImpEtiqEnvio.Click += new EventHandler(ImprimirEtiqEnvio);
            btnQuitarReservas.Click += new EventHandler(QuitarReservas);
            btnRefrescar.Click += new EventHandler(Refrescar);
            btnCancelarPedido.Click += new EventHandler(CancelarPedido);

            // btnConfig
            this.btnConfig.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnConfig.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnConfig.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConfig.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.btnVerSqlPedidos});
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

            //this.idiomasMenuItem.Name = "idiomasMenuItem";
            //this.idiomasMenuItem.Text = Lenguaje.traduce("Idiomas");
            //this.idiomasMenuItem.UseCompatibleTextRendering = false;

            this.btnVerSqlPedidos.Name = "btnVerSqlPedidos";
            this.btnVerSqlPedidos.Text = Lenguaje.traduce(strings.VerSql);
            this.btnVerSqlPedidos.UseCompatibleTextRendering = false;
            InitializeThemesDropDown();

            this.btnExportacion.Size = new Size(50, 50);
            this.btnExportacion.Image = Resources.ExportToExcel;
            this.btnExportacion.ImageAlignment = ContentAlignment.MiddleCenter;

            this.btnRefrescar.Size = new Size(50, 50);
            this.btnRefrescar.Image = Resources.Refresh;
            this.btnRefrescar.ImageAlignment = ContentAlignment.MiddleCenter;

            this.Controls.Add(tableLayoutPanelTabPedidos);
            FuncionesGenerales.ElegirLayout(this.gridPedidos, this.Name);
        }

        private void Refrescar(object sender, EventArgs e)
        {
            LlenarGrid(tipo);
        }

        private void CancelarPedido(object sender, EventArgs e)
        {
            try
            {
                if (gridPedidos.Rows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                foreach (var sel in gridPedidos.SelectedRows)
                {
                    var idPedidoCli = Convert.ToInt32(sel.Cells["IDPEDIDOCLI"].Value.ToString());
                    string error = "";
                    try
                    {
                        String query = "UPDATE TBLPEDIDOSCLICAB SET IDPEDIDOCLIESTADO = 'CA' WHERE IDPEDIDOCLI = " + idPedidoCli;
                        ConexionSQL.SQLClienteExec(query, ref error);
                        MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                LlenarGrid(tipo);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
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
                        var idPedidoCli = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDPEDIDOCLI")].Value.ToString()); // Tiene que ser variable

                        DataTable reservas = ConexionSQL.SQLClientLoad("SELECT * FROM TBLRESERVAS WHERE IDPEDIDOCLI = " + idPedidoCli);
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

        private void ImprimirContOrdenRec(object sender, EventArgs e)
        {
            if (gridPedidos.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridPedidos.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString());

                    using (WSPedidoCliMotorClient ws = new WSPedidoCliMotorClient())
                    {
                        ws.imprimirEtiquetaEnvio(id, User.NombreImpresora);
                    };
                }
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a la impresora "), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ImprimirEtiqEnvio(object sender, EventArgs e)
        {
            try
            {
                if (gridPedidos.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                foreach (var sel in gridPedidos.SelectedRows)
                {
                    var idPedidoCli = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDPEDIDOCLI")].Value.ToString());
                    using (WSPedidoCliMotorClient ws = new WSPedidoCliMotorClient())
                    {
                        try
                        {
                            ws.imprimirEtiquetaEnvio(idPedidoCli, User.NombreImpresora);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    };
                }
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a la impresora "), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ImprimirInformePackingPedidoCli(object sender, EventArgs e)
        {
            ImpresionInformes(17200);
            /*  try
              {
                  if (gridPedidos.SelectedRows.Count <= 0)
                  {
                      MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                      return;
                  }
                  var parametros = string.Empty;

                  foreach (var sel in gridPedidos.SelectedRows)
                  {
                      var id = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDPEDIDOCLI")].Value.ToString());
                      parametros += "IDPEDIDOCLI=" + id + " OR ";
                  }
                  int idInforme = 17200;
                  VisorInforme v = new VisorInforme(idInforme, parametros);
                  v.TopMost = true;
                  v.ShowDialog();
              }
              catch (Exception ex)
              {
                  ExceptionManager.GestionarError(ex);
                  log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
              }*/
        }

        private void ImprimirInformeDiscrepanciasPedido(object sender, EventArgs e)
        {
            // ImpresionInformes(17100);

            try
            {
                if (gridPedidos.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                var parametros = string.Empty;

                foreach (var sel in gridPedidos.SelectedRows)
                {
                    var id = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDPEDIDOCLI")].Value.ToString());
                    string serie = sel.Cells[Lenguaje.traduce("SERIE")].Value.ToString();
                    string referencia = sel.Cells[Lenguaje.traduce("REFERENCIA")].Value.ToString();
                    parametros += "IDPEDIDOCLI=" + id + ";SERIE=" + serie + ";REFERENCIA=" + referencia;
                }
                int idInforme = 17100;
                VisorInforme v = new VisorInforme(idInforme, parametros);
                v.TopMost = true;
                v.Activate();
                v.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ImprimirInformeContPedidoCli(object sender, EventArgs e)
        {
            ImpresionInformes(17200);
        }

        private void ImpresionInformes(int idInforme)
        {
            try
            {
                if (gridPedidos.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                var parametros = string.Empty;

                foreach (var sel in gridPedidos.SelectedRows)
                {
                    var id = Convert.ToInt32(sel.Cells[Lenguaje.traduce("IDPEDIDOCLI")].Value.ToString());
                    string serie = sel.Cells[Lenguaje.traduce("SERIE")].Value.ToString();
                    string referencia = sel.Cells[Lenguaje.traduce("REFERENCIA")].Value.ToString();
                    parametros += "IDPEDIDOCLI=" + id + ";SERIE=" + serie + ";PEDIDO=" + referencia;
                }
                //int idInforme = 17200;
                VisorInforme v = new VisorInforme(idInforme, parametros);
                v.TopMost = true;
                v.Activate();
                v.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void VistaSQL(object sender, EventArgs e)
        {
            VerSQL verSQL = new VerSQL(q);
        }

        private void VerPacking(object sender, EventArgs e)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                celda = "";
                if (gridPedidos.RowCount != 0)
                {
                    string where = "";
                    bool primero = true;
                    foreach (GridViewRowInfo row in gridPedidos.SelectedRows)
                    {
                        celda = row.Cells[0].Value.ToString();
                        if (primero)
                        {
                            where = celda;
                            primero = false;
                        }
                        else
                        {
                            where = where + "," + celda;
                        }
                    }
                    //celda = gridPedidos.SelectedRows[0].Cells[0].Value.ToString();
                    Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PC.IDPEDIDOCLI IN ( " + where + ")";
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PC.IDPEDIDOCLI = null";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void VerOrdenes(object sender, EventArgs e)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                celda = "";
                if (gridPedidos.RowCount != 0)
                {
                    //celda = gridPedidos.SelectedRows[0].Cells[0].Value.ToString();
                    string where = "";
                    bool primero = true;
                    foreach (GridViewRowInfo row in gridPedidos.SelectedRows)
                    {
                        celda = row.Cells[0].Value.ToString();
                        if (primero)
                        {
                            where = celda;
                            primero = false;
                        }
                        else
                        {
                            where = where + "," + celda;
                        }
                    }
                    OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE PC.IDPEDIDOCLI IN (" + where + ")";
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE PC.IDPEDIDOCLI = null";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void VerContenido(object sender, EventArgs e)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                celda = "";
                if (gridPedidos.RowCount != 0)
                {
                    string where = "";
                    bool primero = true;
                    foreach (GridViewRowInfo row in gridPedidos.SelectedRows)
                    {
                        celda = row.Cells[0].Value.ToString();
                        if (primero)
                        {
                            where = celda;
                            primero = false;
                        }
                        else
                        {
                            where = where + "," + celda;
                        }
                    }
                    OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE PC.IDPEDIDOCLI  IN ( " + where + ")";
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE PC.IDPEDIDOCLI = null";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void VerCargaCamion(object sender, EventArgs e)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                celda = "";
                if (gridPedidos.RowCount != 0)
                {
                    //celda = gridPedidos.SelectedRows[0].Cells[0].Value.ToString();
                    string where = "";
                    bool primero = true;
                    foreach (GridViewRowInfo row in gridPedidos.SelectedRows)
                    {
                        celda = row.Cells[0].Value.ToString();
                        if (primero)
                        {
                            where = celda;
                            primero = false;
                        }
                        else
                        {
                            where = where + "," + celda;
                        }
                    }
                    OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE PC.IDPEDIDOCLI IN (" + where + ")";
                }
                else
                {
                    RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE IDENTIFICADORPL = null";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridPedidos);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridPedidos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PC.FECHAPREVENVIO>='" + DateTime.Today.ToString("yyyyMMdd") + "' AND PC.FECHAPREVENVIO<'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;

                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PL.IDENTIFICADOR IS NOT NULL AND PL.IDPACKINGESTADO='PK'";
                        break;

                    case 3:
                        PorNum porNum = new PorNum(gridPedidos, "Pedidos Cliente por Referencia y Serie");
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PC.REFERENCIA=null AND SERIE=null";
                        porNum.label1.Text = Lenguaje.traduce("Referencia:");
                        porNum.label2.Text = Lenguaje.traduce("Serie:");

                        porNum.ShowDialog();

                        referencia = porNum.campo1;
                        serie = porNum.campo2;

                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PC.REFERENCIA='" + referencia + "' AND SERIE='" + serie + "'";
                        gridPedidos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;

                    case 4:
                        PorFecha porFecha = new PorFecha(gridPedidos, "Por Fecha Prevista de Envío");
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE FECHAPREVENVIO=null";

                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE FECHAPREVENVIO>='" + dateString + "' AND FECHAPREVENVIO<'" + dateStringMañana + "'";
                            gridPedidos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE FECHAPREVENVIO=null";
                        }
                        break;

                    case 5:
                        PorCodigoRuta porCodigoRuta = new PorCodigoRuta(gridPedidos);
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PC.IDRUTA=null";
                        break;

                    case 6:
                        PorCodigoCliente porCodigoCliente = new PorCodigoCliente(gridPedidos);
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PC.IDCLIENTE=null";
                        break;

                    case 7:
                        PorEstadoPedido porEstado = new PorEstadoPedido(gridPedidos);
                        q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PC.REFERENCIA='' AND SERIE=''";
                        break;

                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE");
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

        #endregion Temas

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // PedidosTan
            //
            this.Name = "PedidosTan";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);
        }
    }
}
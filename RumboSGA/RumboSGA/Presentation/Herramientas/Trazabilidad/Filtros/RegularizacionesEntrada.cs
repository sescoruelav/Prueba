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
    public partial class RegulaEntrada : UserControl
    {
        public RadButton btnVerExistencias = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnVerPendiente = new RadButton();
        public RadButton btnVerRecepciones = new RadButton();
        public RadButton btnVerPedidosProv = new RadButton();
        public RadButton btnVerDevolucionesCliente = new RadButton();
        public RadButton btnVerOrdenesFab = new RadButton();
        public RadButton btnVerRegularizacion = new RadButton();
        public RadButton btnVerPacking = new RadButton();
        public RadButton btnVerOrdenesRecogida = new RadButton();
        public RadButton btnVerPedidosCliente = new RadButton();
        public RadButton btnVerCargaCamion = new RadButton();
        public RadButton btnVerConsumos = new RadButton();
        public RadButton btnActualizarLote = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridRegulaEntrada = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelRegulaEntrada = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private int tipo;
        public static string q { get; set; }
        public string celda { get; private set; }
        public string dateString { get; set; }
        public string dateStringMañana { get; set; }


        public RegulaEntrada(int tipo)
        {
            this.gridRegulaEntrada.Name = "TrazabilidadRegularizacionesEntrada";
            tableLayoutPanelBotones.AutoSize = true;

            tableLayoutPanelRegulaEntrada.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelRegulaEntrada.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerExistencias, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPendiente, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerRecepciones, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosProv, 4, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerDevolucionesCliente, 5, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenesFab, 6, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerRegularizacion, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerPacking, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenesRecogida, 2, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosCliente, 3, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerCargaCamion, 4, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerConsumos, 5, 1);
            tableLayoutPanelBotones.Controls.Add(btnActualizarLote, 6, 1);

            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelRegulaEntrada.Controls.Add(gridRegulaEntrada);

            tableLayoutPanelBotones.RowCount = 2;
            tableLayoutPanelBotones.ColumnCount = 7;

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelRegulaEntrada.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelRegulaEntrada.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerExistencias.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnVerPendiente.Dock = DockStyle.Fill;
            btnVerRecepciones.Dock = DockStyle.Fill;
            btnVerPedidosProv.Dock = DockStyle.Fill;
            btnVerDevolucionesCliente.Dock = DockStyle.Fill;
            btnVerOrdenesFab.Dock = DockStyle.Fill;
            btnVerRegularizacion.Dock = DockStyle.Fill;
            btnVerPacking.Dock = DockStyle.Fill;
            btnVerOrdenesRecogida.Dock = DockStyle.Fill;
            btnVerPedidosCliente.Dock = DockStyle.Fill;
            btnVerCargaCamion.Dock = DockStyle.Fill;
            btnVerConsumos.Dock = DockStyle.Fill;
            btnActualizarLote.Dock = DockStyle.Fill;

            btnConfig.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            gridRegulaEntrada.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelRegulaEntrada.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelRegulaEntrada.SetColumnSpan(gridRegulaEntrada, 10);

            gridRegulaEntrada.MultiSelect = true; // Permite la selección de multiples articulos
            gridRegulaEntrada.EnableHotTracking = true;
            gridRegulaEntrada.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridRegulaEntrada.EnableFiltering = true;
            gridRegulaEntrada.MasterTemplate.EnableFiltering = true;
            gridRegulaEntrada.MasterTemplate.AllowAddNewRow = false;
            gridRegulaEntrada.AllowDragToGroup = false;
            gridRegulaEntrada.AllowColumnReorder = false;
            gridRegulaEntrada.AllowDeleteRow = false;
            gridRegulaEntrada.AllowEditRow = false;
            gridRegulaEntrada.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerExistencias.Text = Lenguaje.traduce(strings.VerExistencias);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);
            btnVerPendiente.Text = Lenguaje.traduce(strings.VerPendiente);
            btnVerRecepciones.Text = Lenguaje.traduce(strings.VerRecepciones);
            btnVerPedidosProv.Text = Lenguaje.traduce(strings.VerPedidosProv);
            btnVerDevolucionesCliente.Text = Lenguaje.traduce(strings.VerDevolucionesCli);
            btnVerOrdenesFab.Text = Lenguaje.traduce(strings.VerOrdenesFab);
            btnVerRegularizacion.Text = Lenguaje.traduce(strings.VerRegularizacion);
            btnVerPacking.Text = Lenguaje.traduce(strings.VerPacking);
            btnVerOrdenesRecogida.Text = Lenguaje.traduce(strings.VerOrdenesRecogida);
            btnVerPedidosCliente.Text = Lenguaje.traduce(strings.VerPedidosCliente);
            btnVerCargaCamion.Text = Lenguaje.traduce(strings.VerCargaCamion);
            btnVerConsumos.Text = Lenguaje.traduce(strings.VerConsumos);
            btnActualizarLote.Text = Lenguaje.traduce(strings.ActualizarLote);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnExportacion.Click += new EventHandler(Exportar);
            btnRefrescar.Click += new EventHandler(Refrescar);

            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerSalidas.Click += new EventHandler(VerSalidas);
            btnVerRecepciones.Click += new EventHandler(VerRecepciones);
            btnVerPedidosProv.Click += new EventHandler(VerPedidosProv);
            btnVerDevolucionesCliente.Click += new EventHandler(VerDevolucionesCli);
            btnVerOrdenesFab.Click += new EventHandler(VerOrdenesFab);
            btnVerRegularizacion.Click += new EventHandler(VerRegularizacion);
            btnVerPacking.Click += new EventHandler(VerPacking);
            btnVerOrdenesRecogida.Click += new EventHandler(VerOrdenesRecogida);
            btnVerPedidosCliente.Click += new EventHandler(VerPedidos);
            btnVerCargaCamion.Click += new EventHandler(VerCargaCamion);
            btnVerConsumos.Click += new EventHandler(VerConsumos);
            btnVerPendiente.Click += new EventHandler(VerPendiente);
            btnActualizarLote.Click += new EventHandler(ActualizarLote);

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
                FuncionesGenerales.guardarLayout(this.gridRegulaEntrada, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridRegulaEntrada, this.Name);
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

            this.Controls.Add(tableLayoutPanelRegulaEntrada);
            FuncionesGenerales.ElegirLayout(this.gridRegulaEntrada, this.Name);
        }
        private void ActualizarLote(object sender, EventArgs e)
        {
            try
            {
                if (gridRegulaEntrada.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debe seleccionar una fila"));
                }
                else
                {
                    using (LotesMotor.WSLotesClient ws = new LotesMotor.WSLotesClient())
                    {
                        foreach (var row in gridRegulaEntrada.SelectedRows)
                        {
                            var idEntrada = row.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString();
                            var lote = row.Cells[Lenguaje.traduce("LOTE")].Value.ToString();
                            ws.actualizaLote(Int32.Parse(idEntrada), lote);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex);
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

        private void VerExistencias(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE EX.IDENTRADA = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE EX.IDENTRADA = null";
            }
        }

        private void VerRecepciones(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[24].Value.ToString();
                if (Recepciones.q != String.Empty)
                    Recepciones.q = "";
                Recepciones.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE IDRECEPCION = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Recepciones.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE IDRECEPCION = null";
            }
        }

        private void VerPedidosProv(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[16].Value.ToString();
                if (PedidosProv.q != String.Empty)
                    PedidosProv.q = "";
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE IDPEDIDOPRO = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE IDPEDIDOPRO = null";
            }
        }
        private void VerPendiente(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                foreach (var sel in gridRegulaEntrada.SelectedRows)
                {
                    celda += "ENTRADA = '" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + "' OR ";
                }
                if (Generico.q != String.Empty)
                    Generico.q = "";
                celda = celda.Remove(celda.Length - 3);
                Generico.q = ConfigurarSQL.CargarConsulta(path, "PENDIENTES") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Generico.q = ConfigurarSQL.CargarConsulta(path, "PENDIENTES") + " WHERE ENTRADA = null";
            }
        }

        private void VerDevolucionesCli(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[25].Value.ToString();
                if (DevolucionesCliente.q != String.Empty)
                    DevolucionesCliente.q = "";
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE IDDEVOLCLI = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE IDDEVOLCLI = null";
            }
        }

        private void VerOrdenesFab(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[29].Value.ToString();
                if (OrdenesFabricacion.q != String.Empty)
                    OrdenesFabricacion.q = "";
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE IDPEDIDOFAB = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE IDPEDIDOFAB = null";
            }
        }

        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (OrdenesMovimientosSalidaControl.q != String.Empty)
                    OrdenesMovimientosSalidaControl.q = "";
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE S.IDENTRADA = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerRegularizacion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (Packing.q != String.Empty)
                    Packing.q = "";
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE ((S.IDENTRADA='" + celda + "') and IDSALIDATIPO='SR')";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerPacking(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (Packing.q != String.Empty)
                    Packing.q = "";
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE ((S.IDENTRADA='" + celda + "') and S.IDENTIFICADORPL<>0)";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerOrdenesRecogida(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (OrdenesRecogidaControl.q != String.Empty)
                    OrdenesRecogidaControl.q = "";
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE (S.IDENTRADA='" + celda + "')";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerPedidos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE S.IDENTRADA = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerCargaCamion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (OrdenesCargaControl.q != String.Empty)
                    OrdenesCargaControl.q = "";
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE S.IDENTRADA = '" + celda + "'";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerConsumos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridRegulaEntrada.RowCount != 0)
            {
                celda = gridRegulaEntrada.SelectedRows[0].Cells[0].Value.ToString();
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE ((S.PACKINGLIST='" + celda + "' AND S.IDSALIDATIPO='SF'))";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE S.PACKINGLIST = null";
            }
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridRegulaEntrada);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridRegulaEntrada.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA") + " WHERE IDENTRADATIPO='ER' AND (REPLACE(left(CONVERT(varchar(10),E.FECHA, 126),10),'-','')='" + DateTime.Today.ToString("yyyyMMdd") + "')";
                        break;
                    case 2:
                        PorFecha porFecha = new PorFecha(gridRegulaEntrada, "Regularizaciones entrada por fecha");
                        q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA") + " WHERE IDENTRADATIPO='ER' AND (REPLACE(left(CONVERT(varchar(10),E.FECHA, 126),10),'-','')=null)";
                        
                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA") + " WHERE IDENTRADATIPO='ER' AND (REPLACE(left(CONVERT(varchar(10),E.FECHA, 126),10),'-','')='" + dateString + "')";
                            gridRegulaEntrada.DataSource = ConexionSQL.getDataTable(q).DefaultView;

                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA") + " WHERE IDENTRADATIPO='ER' AND (REPLACE(left(CONVERT(varchar(10),E.FECHA, 126),10),'-','')=null)";
                        }
                        break;
                    case 3:
                        q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA") + " WHERE IDENTRADATIPO='ER' AND (E.LOTE = '' AND E.IDARTICULO = null)";
                        new PorArticuloLoteRegEntrada(gridRegulaEntrada);
                        break;
                    case 4:
                        new PorArticuloFechaRegEntrada(gridRegulaEntrada);
                        q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA") + " WHERE REPLACE(left(CONVERT(varchar(10),FECHARECEPCION, 126),10),'-','')=null";
                        break;
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridRegulaEntrada.DataSource = dt.DefaultView;
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
            // RegulaEntradaTan
            // 
            this.Name = "RegulaEntrada";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

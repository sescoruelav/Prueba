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
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class Matriculas : UserControl
    {
        public Panel paddingPanel = new Panel();
        public RadButton btnVerExistencias = new RadButton();
        public RadButton btnVerSalidas = new RadButton();
        public RadButton btnVerRecepciones = new RadButton();
        public RadButton btnVerPedidosProveedor = new RadButton();
        public RadButton btnVerDevolucionesCliente = new RadButton();
        public RadButton btnVerOrdenesFabricacion = new RadButton();
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
        public RadGridView gridMatriculas = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelMatriculas = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private int tipo;

        public static string q { get; set; }
        public string celda { get; set; }

        public Matriculas(int tipo)
        {
            this.tipo = tipo;
            this.gridMatriculas.Name = "TrazabilidadMatriculas";
            tableLayoutPanelMatriculas.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelMatriculas.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerExistencias, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerSalidas, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerRecepciones, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosProveedor, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerDevolucionesCliente, 4, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenesFabricacion, 5, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerRegularizacion, 6, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPacking, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenesRecogida, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidosCliente, 2, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerCargaCamion, 3, 1);
            tableLayoutPanelBotones.Controls.Add(btnVerConsumos, 4, 1);
            tableLayoutPanelBotones.Controls.Add(btnActualizarLote, 5, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelMatriculas.Controls.Add(gridMatriculas);

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

            tableLayoutPanelMatriculas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelMatriculas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerExistencias.Dock = DockStyle.Fill;
            btnVerSalidas.Dock = DockStyle.Fill;
            btnVerRecepciones.Dock = DockStyle.Fill;
            btnVerPedidosProveedor.Dock = DockStyle.Fill;
            btnVerDevolucionesCliente.Dock = DockStyle.Fill;
            btnVerOrdenesFabricacion.Dock = DockStyle.Fill;
            btnVerRegularizacion.Dock = DockStyle.Fill;
            btnVerPacking.Dock = DockStyle.Fill;
            btnVerOrdenesRecogida.Dock = DockStyle.Fill;
            btnVerPedidosCliente.Dock = DockStyle.Fill;
            btnVerCargaCamion.Dock = DockStyle.Fill;
            btnVerConsumos.Dock = DockStyle.Fill;
            btnActualizarLote.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            gridMatriculas.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelMatriculas.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelMatriculas.SetColumnSpan(gridMatriculas, 10);

            gridMatriculas.MultiSelect = true; // Permite la selección de multiples articulos
            gridMatriculas.EnableHotTracking = true;
            gridMatriculas.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridMatriculas.EnableFiltering = true;
            gridMatriculas.MasterTemplate.EnableFiltering = true;
            gridMatriculas.MasterTemplate.AllowAddNewRow = false;
            gridMatriculas.AllowDragToGroup = false;
            gridMatriculas.AllowColumnReorder = false;
            gridMatriculas.AllowDeleteRow = false;
            gridMatriculas.AllowEditRow = false;
            gridMatriculas.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSql.Text = Lenguaje.traduce(strings.VerSql);
            btnVerExistencias.Text = Lenguaje.traduce(strings.VerExistencias);
            btnVerSalidas.Text = Lenguaje.traduce(strings.VerSalidas);
            btnVerRecepciones.Text = Lenguaje.traduce(strings.VerRecepciones);
            btnVerPedidosProveedor.Text = Lenguaje.traduce(strings.VerPedidosProv);
            btnVerDevolucionesCliente.Text = Lenguaje.traduce(strings.VerDevolucionesCli);
            btnVerOrdenesFabricacion.Text = Lenguaje.traduce(strings.VerOrdenesFab);
            btnVerRegularizacion.Text = Lenguaje.traduce(strings.VerRegularizacion);
            btnVerPacking.Text = Lenguaje.traduce(strings.VerPacking);
            btnVerOrdenesRecogida.Text = Lenguaje.traduce(strings.VerOrdenesRecogida);
            btnVerPedidosCliente.Text = Lenguaje.traduce(strings.VerPedidosCliente);
            btnVerCargaCamion.Text = Lenguaje.traduce(strings.VerCargaCamion);
            btnVerConsumos.Text = Lenguaje.traduce(strings.VerConsumos);
            btnActualizarLote.Text = Lenguaje.traduce(strings.ActualizarLote);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnExportacion.Click += new EventHandler(Exportar);
            
            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerSalidas.Click += new EventHandler(VerSalidas);
            btnVerRecepciones.Click += new EventHandler(VerRecepciones);
            btnVerPedidosProveedor.Click += new EventHandler(VerPedidosProv);
            btnVerDevolucionesCliente.Click += new EventHandler(VerDevolucionesCli);
            btnVerOrdenesFabricacion.Click += new EventHandler(VerOrdenesFab);
            btnVerRegularizacion.Click += new EventHandler(VerRegularizacion);
            btnVerPacking.Click += new EventHandler(VerPacking);
            btnVerOrdenesRecogida.Click += new EventHandler(VerOrdenesRecogida);
            btnVerPedidosCliente.Click += new EventHandler(VerPedidos);
            btnVerCargaCamion.Click += new EventHandler(VerCargaCamion);
            btnVerConsumos.Click += new EventHandler(VerConsumos);
            btnActualizarLote.Click += new EventHandler(ActualizarLote);
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
                FuncionesGenerales.guardarLayout(this.gridMatriculas, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridMatriculas, this.Name);
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

            this.Controls.Add(tableLayoutPanelMatriculas);
            FuncionesGenerales.ElegirLayout(this.gridMatriculas, this.Name);
        }

        private void ActualizarLote(object sender, EventArgs e)
        {
            try
            {
                if (gridMatriculas.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debe seleccionar una fila"));
                }
                else
                {
                    using (LotesMotor.WSLotesClient ws = new LotesMotor.WSLotesClient())
                    {
                        foreach (var row in gridMatriculas.SelectedRows)
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "EX.IDENTRADA = " + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "IDRECEPCION = " + sel.Cells[Lenguaje.traduce("IDRECEPCION")].Value.ToString() + " OR ";
                }
                if (Recepciones.q != String.Empty)
                    Recepciones.q = "";
                celda = celda.Remove(celda.Length - 3);
                Recepciones.q = ConfigurarSQL.CargarConsulta(path, "RECEPCIONES") + " WHERE " + celda;
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "PP.IDPEDIDOPRO = " + sel.Cells[Lenguaje.traduce("IDPEDIDOPRO")].Value.ToString() + " OR ";
                }
                if (PedidosProv.q != String.Empty)
                    PedidosProv.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE PP.IDPEDIDOPRO = null";
            }
        }

        private void VerDevolucionesCli(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "DC.IDDEVOLCLI = " + sel.Cells[Lenguaje.traduce("IDDEVOLCLI")].Value.ToString() + " OR ";
                }
                if (DevolucionesCliente.q != String.Empty)
                    DevolucionesCliente.q = "";
                celda = celda.Remove(celda.Length - 3);
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE DC.IDDEVOLCLI = null";
            }
        }

        private void VerOrdenesFab(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "OF1.IDPEDIDOFAB = " + sel.Cells[Lenguaje.traduce("IDPEDIDOFAB")].Value.ToString() + " OR ";
                }
                if (OrdenesFabricacion.q != String.Empty)
                    OrdenesFabricacion.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE OF1.IDPEDIDOFAB = null";
            }
        }

        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "S.IDENTRADA = " + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (OrdenesMovimientosSalidaControl.q != String.Empty)
                    OrdenesMovimientosSalidaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE " + celda;
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "((S.IDENTRADA = '" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + "') and S.IDSALIDATIPO = 'SR') OR ";
                }
                if (Packing.q != String.Empty)
                    Packing.q = "";
                celda = celda.Remove(celda.Length - 3);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESALIDA") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerPacking(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "((S.IDENTRADA = '" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + "') and S.IDENTIFICADORPL<>0) OR ";
                }
                if (Packing.q != String.Empty)
                    Packing.q = "";
                celda = celda.Remove(celda.Length - 3);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE " + celda;
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "S.IDENTRADA = " + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (OrdenesRecogidaControl.q != String.Empty)
                    OrdenesRecogidaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE " + celda;
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "S.IDENTRADA = " + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (PedidosTan.q != String.Empty)
                    PedidosTan.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE " + celda;
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "S.IDENTRADA = " + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (OrdenesCargaControl.q != String.Empty)
                    OrdenesCargaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE " + celda;
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
            if (gridMatriculas.RowCount != 0)
            {
                foreach (var sel in gridMatriculas.SelectedRows)
                {
                    celda += "((S.PACKINGLIST = '" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + "' AND S.IDSALIDATIPO = 'SF')) OR ";
                }
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                celda = celda.Remove(celda.Length - 3);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE S.PACKINGLIST = null";
            }
        }

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridMatriculas);
        }

        public void SetQuery(string query)
        {
            q = query;
            gridMatriculas.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        new PorNumMatricula("e_matricula", gridMatriculas);
                        q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE (E.IDENTRADA=null OR E.SSCC=null)";
                        break;
                    case 2:
                        new PorNumMatricula("s_matricula", gridMatriculas);
                        q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE (S.IDENTRADA=null OR E.SSCC=null)";
                        break;
                    case 3:
                        new PorNumMatricula("e_serie", gridMatriculas);
                        q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE E.NSERIE = null";
                        break;
                    case 4:
                        new PorNumMatricula("s_serie", gridMatriculas);
                        q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE ((E.NSERIE = null) AND E.IDENTRADA = S.IDENTRADA)";
                        break;                    
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "MATRICULA");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridMatriculas.DataSource = dt.DefaultView;
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
            this.Name = "Matriculas";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);
        }
    }
}

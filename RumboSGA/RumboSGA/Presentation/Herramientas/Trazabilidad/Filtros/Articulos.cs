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
    public class Articulos : UserControl
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
        public RadButton btnCambiarEstadoOrd = new RadButton();
        public RadButton btnAbonarUnidades = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridArticulos = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public RadMenuItem btnVerSql = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelArticulos = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private int tipo;

        public static string q { get; set; }
        public static string celda { get; set; }

        public Articulos(int tipo)
        {
            this.tipo = tipo;
            gridArticulos.Name = "TrazabilidadArticulos";
            tableLayoutPanelArticulos.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelArticulos.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            switch (tipo)
            {
                case 2:
                    tableLayoutPanelBotones.Controls.Add(btnCambiarEstadoOrd, 0, 0);
                    tableLayoutPanelBotones.Controls.Add(btnAbonarUnidades, 1, 0);
                    tableLayoutPanelBotones.RowCount = 1;
                    tableLayoutPanelBotones.ColumnCount = 2;
                    break;
                case 4:
                    tableLayoutPanelBotones.Controls.Add(btnCambiarEstadoOrd, 0, 0);
                    tableLayoutPanelBotones.Controls.Add(btnAbonarUnidades, 1, 0);
                    tableLayoutPanelBotones.RowCount = 0;
                    tableLayoutPanelBotones.ColumnCount = 2;
                    break;
                default:
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
                    tableLayoutPanelBotones.RowCount = 2;
                    tableLayoutPanelBotones.ColumnCount = 7;
                    break;
            }
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelArticulos.Controls.Add(gridArticulos);            

            for (int i = 0; i < tableLayoutPanelBotones.RowCount; i++)
            {
                tableLayoutPanelBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.RowCount));
                for (int j = 0; j < tableLayoutPanelBotones.ColumnCount; j++)
                {
                    tableLayoutPanelBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100 / tableLayoutPanelBotones.ColumnCount));
                }
            }

            tableLayoutPanelArticulos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelArticulos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

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
            btnCambiarEstadoOrd.Dock = DockStyle.Fill;
            btnAbonarUnidades.Dock = DockStyle.Fill;
            gridArticulos.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelArticulos.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelArticulos.SetColumnSpan(gridArticulos, 10);

            gridArticulos.MultiSelect = true; // Permite la selección de multiples articulos
            gridArticulos.EnableHotTracking = true;
            gridArticulos.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridArticulos.EnableFiltering = true;
            gridArticulos.MasterTemplate.EnableFiltering = true;
            gridArticulos.MasterTemplate.AllowAddNewRow = false;
            gridArticulos.AllowDragToGroup = false;
            gridArticulos.AllowColumnReorder = false;
            gridArticulos.AllowDeleteRow = false;
            gridArticulos.AllowEditRow = false;
            gridArticulos.BestFitColumns();
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
            btnCambiarEstadoOrd.Text = Lenguaje.traduce(strings.CambiarEstadoOrdenes);
            btnAbonarUnidades.Text = Lenguaje.traduce(strings.AbonarUnidades);
            btnActualizarLote.Text = Lenguaje.traduce(strings.ActualizarLote);

            btnVerSql.Click += new EventHandler(VistaSQL);
            btnVerExistencias.Click += new EventHandler(VerExistencias);
            btnVerSalidas.Click += new EventHandler(VerSalidas);
            btnVerRecepciones.Click += new EventHandler(VerRecepciones);
            btnVerPedidosProveedor.Click += new EventHandler(VerPedidosProv);
            btnVerDevolucionesCliente.Click += new EventHandler(VerDevolucionesCli);
            btnVerOrdenesFabricacion.Click += new EventHandler(VerOrdenesFab);
            btnVerRegularizacion.Click += new EventHandler(VerRegularizacion);
            btnVerPacking.Click += new EventHandler(VerPacking);
            btnVerOrdenesRecogida.Click += new EventHandler(VerOrdenesRecogida);
            btnVerPedidosCliente.Click += new EventHandler(VerPedidosCliente);
            btnVerCargaCamion.Click += new EventHandler(VerCargaCamion);
            btnVerConsumos.Click += new EventHandler(VerConsumos);
            btnCambiarEstadoOrd.Click += new EventHandler(CambiarEstadoOrd);
            btnAbonarUnidades.Click += new EventHandler(AbonarUnidades);
            btnExportacion.Click += new EventHandler(Exportar);
            btnRefrescar.Click += new EventHandler(Refrescar);
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
            this.gridArticulos.EnablePaging = false;
            this.gridArticulos.EnableFiltering = true;
            this.gridArticulos.EnableGrouping = true;
            this.gridArticulos.EnableSorting = true;

            this.btnConfig.Items["RadMenuItemGuardarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.guardarLayout(this.gridArticulos, "ArticulosForm","ArticulosGrid");
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridArticulos, "ArticulosForm", "ArticulosGrid");
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

            this.Controls.Add(tableLayoutPanelArticulos);
            FuncionesGenerales.ElegirLayout(this.gridArticulos, this.Name);
        }

        private void CambiarEstadoOrd(object sender, EventArgs e)
        {
            if (gridArticulos.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                List<int> ordenes = new List<int>();
                foreach (var row in gridArticulos.SelectedRows)
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

        private void AbonarUnidades(object sender, EventArgs e)
        {
            if (gridArticulos.SelectedRows.Count != 1)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                var idsalida = Convert.ToInt32(gridArticulos.SelectedRows[0].Cells[Lenguaje.traduce("IDSALIDA")].Value.ToString());
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

        private void VerExistencias(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "E.IDENTRADA=" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Existencias.q != String.Empty)
                    Existencias.q = "";
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Existencias.q = ConfigurarSQL.CargarConsulta(path, "EXISTENCIAS") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerSalidas(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "E.IDENTRADA=" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (OrdenesMovimientosSalidaControl.q != String.Empty)
                    OrdenesMovimientosSalidaControl.q = "";
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerPacking(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[1].Value.ToString() + " OR ";
                }
                celda = celda.Remove(celda.Length - 3);
                if (Packing.q != String.Empty)
                    Packing.q = "";
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE S.IDENTRADA = null";
            }
        }

        private void VerPedidosCli(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
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

        private void VerPedidosProv(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "E.IDENTRADA =" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (PedidosProv.q != String.Empty)
                    PedidosProv.q = "";
                celda = celda.Remove(celda.Length - 3);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerDevolucionesCli(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "E.IDENTRADA =" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (DevolucionesCliente.q != String.Empty)
                    DevolucionesCliente.q = "";
                celda = celda.Remove(celda.Length - 3);
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerEntrada(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "EX.IDENTRADA =" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (Matriculas.q != String.Empty)
                    Matriculas.q = "";
                celda = celda.Remove(celda.Length - 3);
                Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                Matriculas.q = null;
            }
        }

        private void VerOrdenes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
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

        private void VerOrdenesFab(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "E.IDENTRADA =" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (OrdenesFabricacion.q != String.Empty)
                    OrdenesFabricacion.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesFabricacion.q = ConfigurarSQL.CargarConsulta(path, "ORDENESFABRICACION") + " WHERE E.IDENTRADA = null";
            }
        }

        private void VerRegularizacion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "(S.IDENTRADA = '" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + "' AND S.IDSALIDATIPO = 'SR') OR ";
                }
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                celda = celda.Remove(celda.Length - 3);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE S.IDENTRADA = null";
            }

        }

        private void VerCargaCamion(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                foreach (var sel in gridArticulos.SelectedRows)
                {
                    celda += "S.IDENTRADA =" + sel.Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString() + " OR ";
                }
                if (OrdenesCargaControl.q != String.Empty)
                    OrdenesCargaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesCargaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE S.IDENTRADA = null";
            }
        }
        private void VerRecepciones(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                celda = gridArticulos.SelectedRows[0].Cells[24].Value.ToString();
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
       

        private void VerOrdenesRecogida(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                celda = gridArticulos.SelectedRows[0].Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString();
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

        private void VerPedidosCliente(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                celda = gridArticulos.SelectedRows[0].Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString();
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

        

        private void VerConsumos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridArticulos.RowCount != 0)
            {
                celda = gridArticulos.SelectedRows[0].Cells[Lenguaje.traduce("IDENTRADA")].Value.ToString();
                if (RegulaSalida.q != String.Empty)
                    RegulaSalida.q = "";
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "CONSUMO") + " WHERE ((S.PACKINGLIST='" + celda + "' AND S.IDSALIDATIPO='SF'))";
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "ORDENESCARGA") + " WHERE S.PACKINGLIST = null";
            }
        }
        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridArticulos);
        }

        private void ActualizarLote(object sender, EventArgs e)
        {
            try
            {
                if (gridArticulos.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debe seleccionar una fila"));
                }
                else
                {
                    using (LotesMotor.WSLotesClient ws = new LotesMotor.WSLotesClient())
                    {
                        foreach (var row in gridArticulos.SelectedRows)
                        {
                            var idEntrada = row.Cells[Lenguaje.traduce(Lenguaje.traduce("IDENTRADA"))].Value.ToString();
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

        public void SetQuery(string query)
        {
            q = query;
            gridArticulos.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
				string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                switch (tipo)
                {
                    case 1:
                        new PorArticuloLote(tipo, gridArticulos);
                        q = ConfigurarSQL.CargarConsulta(path, "ARTICULOSENTRADA") + " WHERE E.LOTE=null AND A.IDARTICULO = null";
                        break;
                    case 2:
                        new PorArticuloLote(tipo, gridArticulos);
                        q = ConfigurarSQL.CargarConsulta(path, "ARTICULOSSALIDA") + " WHERE E.LOTE=null AND A.IDARTICULO = null";
                        break;
                    case 3:
                        new PorArticuloLote(tipo, gridArticulos);
                        q = ConfigurarSQL.CargarConsulta(path, "ARTICULOSENTRADA") + " WHERE E.LOTE=null AND A.IDARTICULO = null";
                        break;
                    case 4:
                        new PorArticuloLote(tipo, gridArticulos);
                        q = ConfigurarSQL.CargarConsulta(path, "ARTICULOSSALIDA") + " WHERE E.LOTE=null AND A.IDARTICULO = null";
                        break;
                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "ARTICULOSSALIDA");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridArticulos.DataSource = dt.DefaultView;
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
            this.Name = Lenguaje.traduce("Articulos");
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }
    }
}

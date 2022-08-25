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
using RumboSGA.Presentation.UserControls.Mantenimientos;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using Rumbo.Core.Herramientas;
using RumboSGA.PackingListMotor;
using RumboSGA.PedidoCliMotor;
using RumboSGAManager.Model;
using RumboSGA.Controles;
using System.IO;
using RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad
{
    public class Packing : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadMenuItem btnVerSqlPacking = new RadMenuItem();
        public RadButton btnVerPedidos = new RadButton();
        public RadButton btnVerOrdenes = new RadButton();
        public RadButton btnVerContenido = new RadButton();
        public RadButton btnVerCarga = new RadButton();
        public RadButton btnBuscarCompatibles = new RadButton();
        public RadButton btnCambiarUbicacion = new RadButton();
        public RadButton btnCambiarEstado = new RadButton();
        public RadButton btnGenerarTareaAbono = new RadButton();
        public RadButton btnAbonarPackingList = new RadButton();
        public RadButton btnImprimirEtiqueta = new RadButton();
        public RadButton btnImprimirEtiquetaNom = new RadButton();
        public RadButton btnImprimirDatos = new RadButton();
        public RadButton btnPonerCargado = new RadButton();
        public RadButton btnRefrescar = new RadButton();
        public RadButton btnExportacion = new RadButton();
        public RadDropDownButton btnConfig = new RadDropDownButton();
        public RadGridView gridPacking = new RadGridView();
        public RadMenuItem temasMenuItem = new RadMenuItem();
        public RadMenuItem idiomasMenuItem = new RadMenuItem();
        public TableLayoutPanel tableLayoutPanelPacking = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelBotones = new TableLayoutPanel();
        public TableLayoutPanel tableLayoutPanelAuxiliar = new TableLayoutPanel();

        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        public static string q { get; set; }
        public string celda { get; set; }
        public string dateString { get; set; }
        public string dateStringMañana { get; set; }

        public string campo1 { get; set; }
        public string campo2 { get; set; }

        private string packingListNum;

        private string nivel;

        private int tipo;

        public string estados { get; set; }

        public Packing(int tipo)
        {
            this.tipo = tipo;
            gridPacking.Name = "TrazabilidadPacking";
            tableLayoutPanelBotones.AutoSize = true;
            tableLayoutPanelPacking.Controls.Add(tableLayoutPanelBotones, 0, 0);
            tableLayoutPanelPacking.Controls.Add(tableLayoutPanelAuxiliar, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerPedidos, 0, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerOrdenes, 1, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerContenido, 2, 0);
            tableLayoutPanelBotones.Controls.Add(btnVerCarga, 3, 0);
            tableLayoutPanelBotones.Controls.Add(btnCambiarUbicacion, 4, 0);
            tableLayoutPanelBotones.Controls.Add(btnPonerCargado, 5, 0);
            tableLayoutPanelBotones.Controls.Add(btnCambiarEstado, 0, 1);
            tableLayoutPanelBotones.Controls.Add(btnImprimirEtiqueta, 1, 1);
            tableLayoutPanelBotones.Controls.Add(btnImprimirEtiquetaNom, 2, 1);
            tableLayoutPanelBotones.Controls.Add(btnImprimirDatos, 3, 1);
            tableLayoutPanelBotones.Controls.Add(btnGenerarTareaAbono, 4, 1);
            tableLayoutPanelBotones.Controls.Add(btnAbonarPackingList, 5, 1);
            tableLayoutPanelAuxiliar.Controls.Add(btnExportacion, 0, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnRefrescar, 1, 0);
            tableLayoutPanelAuxiliar.Controls.Add(btnConfig, 2, 0);
            tableLayoutPanelPacking.Controls.Add(gridPacking);

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

            tableLayoutPanelPacking.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80)); //Botones
            tableLayoutPanelPacking.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 20)); //Auxiliar

            btnVerPedidos.Dock = DockStyle.Fill;
            btnVerOrdenes.Dock = DockStyle.Fill;
            btnVerContenido.Dock = DockStyle.Fill;
            btnVerCarga.Dock = DockStyle.Fill;
            btnCambiarUbicacion.Dock = DockStyle.Fill;
            btnCambiarEstado.Dock = DockStyle.Fill;
            btnImprimirEtiqueta.Dock = DockStyle.Fill;
            btnImprimirEtiquetaNom.Dock = DockStyle.Fill;
            btnImprimirDatos.Dock = DockStyle.Fill;
            btnGenerarTareaAbono.Dock = DockStyle.Fill;
            btnAbonarPackingList.Dock = DockStyle.Fill;
            btnPonerCargado.Dock = DockStyle.Fill;
            btnExportacion.Dock = DockStyle.Fill;
            btnRefrescar.Dock = DockStyle.Fill;
            btnConfig.Dock = DockStyle.Fill;
            gridPacking.Dock = DockStyle.Fill;
            tableLayoutPanelAuxiliar.Dock = DockStyle.Right;
            tableLayoutPanelPacking.Dock = DockStyle.Fill;
            tableLayoutPanelBotones.Dock = DockStyle.Fill;

            tableLayoutPanelPacking.SetColumnSpan(gridPacking, 10);

            gridPacking.MultiSelect = true; // Permite la selección de multiples articulos
            gridPacking.EnableHotTracking = true;
            gridPacking.SelectionMode = GridViewSelectionMode.FullRowSelect;
            gridPacking.EnableFiltering = true;
            gridPacking.MasterTemplate.EnableFiltering = true;
            gridPacking.MasterTemplate.AllowAddNewRow = false;
            gridPacking.AllowColumnReorder = false;
            gridPacking.AllowDragToGroup = false;
            gridPacking.AllowDeleteRow = false;
            gridPacking.AllowEditRow = false;
            gridPacking.BestFitColumns();
            LlenarGrid(tipo);

            btnVerSqlPacking.Text = Lenguaje.traduce(strings.VerSql);
            btnVerPedidos.Text = Lenguaje.traduce("Ver Pedidos Cliente");
            btnVerOrdenes.Text = Lenguaje.traduce("Ver Órdenes");
            btnVerContenido.Text = Lenguaje.traduce(strings.VerContenido);
            btnVerCarga.Text = Lenguaje.traduce(strings.VerCarga);
            btnCambiarUbicacion.Text = Lenguaje.traduce(strings.CambiarUbicacion);
            btnCambiarEstado.Text = Lenguaje.traduce(strings.CambiarEstado);
            btnImprimirDatos.Text = Lenguaje.traduce(strings.ImpDatos);
            btnImprimirEtiqueta.Text = Lenguaje.traduce(strings.ImpEtiq);
            btnImprimirEtiquetaNom.Text = Lenguaje.traduce(strings.ImpEtiqNomPacking);
            btnImprimirDatos.Text = Lenguaje.traduce(strings.ImpDatos);
            btnPonerCargado.Text = Lenguaje.traduce(strings.PonerCargado);
            btnGenerarTareaAbono.Text = Lenguaje.traduce(strings.GenerarTareaAbono);
            btnAbonarPackingList.Text = Lenguaje.traduce(strings.AbonarPackingList);

            btnVerSqlPacking.Click += new EventHandler(VistaSQL);
            btnVerOrdenes.Click += new EventHandler(VerOrdenes);
            btnVerPedidos.Click += new EventHandler(VerPedidos);
            btnVerContenido.Click += new EventHandler(VerContenido);
            btnVerCarga.Click += new EventHandler(VerCargaCamion);
            btnExportacion.Click += new EventHandler(Exportar);
            btnRefrescar.Click += new EventHandler(Refrescar);
            btnImprimirDatos.Click += new EventHandler(ImprimirInformeDatos);
            btnImprimirEtiqueta.Click += new EventHandler(ImprimirEtiquetaPL);
            btnImprimirEtiquetaNom.Click += new EventHandler(ImprimirEtiquetaNominativoPL);
            btnPonerCargado.Click += new EventHandler(PonerCargado);
            btnGenerarTareaAbono.Click += new EventHandler(generarTareaAbono);
            btnAbonarPackingList.Click += new EventHandler(abonarPackingList);
            btnCambiarEstado.Click += new EventHandler(CambiarEstado);
            btnCambiarUbicacion.Click += new EventHandler(CambiarUbicacion);
            btnPonerCargado.Click += new EventHandler(PonerCargado);

            // btnConfig
            this.btnConfig.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnConfig.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnConfig.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConfig.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.idiomasMenuItem,
            this.btnVerSqlPacking});
            FuncionesGenerales.AddGuardarYCargarLayoutEnRadMenu(ref this.btnConfig);
            this.btnConfig.Items["RadMenuItemGuardarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.guardarLayout(this.gridPacking, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            this.btnConfig.Items["RadMenuItemCargarLayout"].Click += new EventHandler((object e, EventArgs a) =>
            {
                FuncionesGenerales.cargarLayout(this.gridPacking, this.Name);
                MessageBox.Show(Lenguaje.traduce("La acción se ha completado con éxito"));
            });
            //this.btnConfig.Location = new System.Drawing.Point(1046, 3);
            this.btnConfig.Name = "btnConfig";

            this.btnConfig.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 140, 24);
            this.btnConfig.Size = new System.Drawing.Size(70, 50);
            this.btnConfig.TabIndex = 9;

            this.temasMenuItem.Name = "temasMenuItem";
            this.temasMenuItem.Text = Lenguaje.traduce("Temas");
            this.temasMenuItem.UseCompatibleTextRendering = false;

            this.idiomasMenuItem.Name = "idiomasMenuItem";
            this.idiomasMenuItem.Text = Lenguaje.traduce("Idiomas");
            this.idiomasMenuItem.UseCompatibleTextRendering = false;

            this.btnVerSqlPacking.Name = "btnVerSqlPacking";
            this.btnVerSqlPacking.Text = Lenguaje.traduce(strings.VerSql);
            this.btnVerSqlPacking.UseCompatibleTextRendering = false;

            InitializeThemesDropDown();

            this.btnExportacion.Size = new Size(50, 50);
            this.btnExportacion.Image = Resources.ExportToExcel;
            this.btnExportacion.ImageAlignment = ContentAlignment.MiddleCenter;

            this.btnRefrescar.Size = new Size(50, 50);
            this.btnRefrescar.Image = Resources.Refresh;
            this.btnRefrescar.ImageAlignment = ContentAlignment.MiddleCenter;

            this.Controls.Add(tableLayoutPanelPacking);
            FuncionesGenerales.ElegirLayout(this.gridPacking, this.Name);
            Permiso(btnGenerarTareaAbono, 5000102);
            Permiso(btnAbonarPackingList, 5000103);
        }

        private void Permiso(RadButton control, int id)
        {
            if (User.Perm.comprobarAcceso(id) == false)
            {
                control.Enabled = false;
            }
            else
            {
                if (User.Perm.tienePermisoEscritura(id) == true)
                {
                    control.Enabled = true;
                }
                else
                {
                    control.Enabled = false;
                }
            }
        }

        private void ImprimirInformeDatos(object sender, EventArgs e)
        {
            try
            {
                if (gridPacking.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
                var parametros = string.Empty;
                foreach (var row in gridPacking.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDPACKINGLIST")].Value.ToString());
                    var nivel = Convert.ToInt32(row.Cells[Lenguaje.traduce("NIVEL")].Value.ToString());
                    parametros += "PACKING=" + id + ";NIVEL=" + nivel + " OR ";
                }
                parametros = parametros.Remove(parametros.Length - 4);
                int idInforme = 17600;
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

        private void CambiarUbicacion(object sender, EventArgs e)
        {
            if (gridPacking.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                List<int> ordenes = new List<int>();
                foreach (var row in gridPacking.SelectedRows)
                {
                    ordenes.Add(Convert.ToInt32(row.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString()));
                }
                CambiarUbicacionPacking cup = new CambiarUbicacionPacking(ordenes.ToArray());
                cup.Show();
            }
            catch (Exception)
            {
                MessageBox.Show(strings.ErrorGenerico);
                throw;
            }
        }

        private void CambiarEstado(object sender, EventArgs e)
        {
            try
            {
                List<string> campos = new List<string>();
                if (gridPacking.SelectedRows.Count != 0)
                {
                    foreach (var item in gridPacking.SelectedRows)
                    {
                        campos.Add(item.Cells[Lenguaje.traduce("IDPACKINGLIST")].Value.ToString());
                    }
                    CambiarEstadoPacking cep = new CambiarEstadoPacking(campos);
                    cep.ShowDialog();
                    if (cep.exito)
                        LlenarGrid(tipo);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                    return;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void PonerCargado(object sender, EventArgs e)
        {
            if (gridPacking.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridPacking.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDPACKINGLIST")].Value.ToString());

                    using (var ws = new WSPackingListMotorClient())
                    {
                        ws.cambiaEstadoPackingList(id, "PC");
                    }
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                throw;
            }
        }

        private void generarTareaAbono(object sender, EventArgs e)
        {
            if (gridPacking.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridPacking.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString());

                    using (var ws = new WSPackingListMotorClient())
                    {
                        ws.insertarTareaAbonoPackingList("AP", id);
                    }
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                throw;
            }
        }

        private void abonarPackingList(object sender, EventArgs e)
        {
            if (gridPacking.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                List<int> ordenes = new List<int>();
                foreach (var row in gridPacking.SelectedRows)
                {
                    ordenes.Add(Convert.ToInt32(row.Cells[Lenguaje.traduce("IDENTIFICADOR")].Value.ToString()));
                }
                AbonarPackingMuelle cup = new AbonarPackingMuelle(ordenes.ToArray());
                cup.ShowDialog();
                LlenarGrid(tipo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                throw;
            }
        }

        private void ImprimirEtiquetaPL(object sender, EventArgs e)
        {
            if (gridPacking.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridPacking.SelectedRows)
                {
                    var id = row.Cells[Lenguaje.traduce("IDPACKINGLIST")].Value.ToString();

                    using (var ws = new WSPedidoCliMotorClient())
                    {
                        ws.imprimirPackingListPorPackinglist(id, User.NombreImpresora);
                    }
                }
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a la impresora "), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show(strings.ErrorGenerico);
                throw;
            }
        }

        private void ImprimirEtiquetaNominativoPL(object sender, EventArgs e)
        {
            if (gridPacking.SelectedRows.Count <= 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar una fila"));
                return;
            }
            try
            {
                foreach (var row in gridPacking.SelectedRows)
                {
                    var id = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDPACKINGLIST")].Value.ToString());

                    using (var ws = new WSPedidoCliMotorClient())
                    {
                        ws.imprimirPackingListNominativo(id, User.NombreImpresora);
                    }
                }
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a la impresora "), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void VerPedidos(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPacking.RowCount != 0)
            {
                foreach (var sel in gridPacking.SelectedRows)
                {
                    celda += "IDENTIFICADORPL = '" + sel.Cells[0].Value.ToString() + "' OR ";
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

        private void VerOrdenes(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPacking.RowCount != 0)
            {
                foreach (var sel in gridPacking.SelectedRows)
                {
                    celda += "PL.IDENTIFICADOR = '" + sel.Cells[0].Value.ToString() + "' OR ";
                }
                if (OrdenesRecogidaControl.q != String.Empty)
                    OrdenesRecogidaControl.q = "";
                celda = celda.Remove(celda.Length - 3);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE " + celda;
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                OrdenesRecogidaControl.q = ConfigurarSQL.CargarConsulta(path, "ORDENESRECOGIDA") + " WHERE PL.IDENTIFICADOR = null";
            }
        }

        private void VerContenido(object sender, EventArgs e)
        {
            string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
            celda = "";
            if (gridPacking.RowCount != 0)
            {
                foreach (var sel in gridPacking.SelectedRows)
                {
                    celda += "((IDENTIFICADORPL = '" + sel.Cells[0].Value.ToString() + "') OR(IDENTIFICADORPL  IN(SELECT PL.IDENTIFICADOR FROM TBLPACKINGLIST PL JOIN TBLPACKINGLIST PP ON PP.IDPACKINGLIST = PL.IDPACKINGLISTPADRE WHERE PP.IDENTIFICADOR = '" + sel.Cells[0].Value.ToString() + "'))) OR ";
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
            if (gridPacking.RowCount != 0)
            {
                foreach (var sel in gridPacking.SelectedRows)
                {
                    celda += "CB.IDCARGA = '" + sel.Cells[14].Value.ToString() + "' OR ";
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

        private void Exportar(object sender, EventArgs e)
        {
            ExportHelper.ExportarExcel(gridPacking);
        }

        public void SetQuery(string query)
        {
            //Poner sql
            q = query;
            gridPacking.DataSource = ConexionSQL.getDataTable(q).DefaultView;
        }

        private void LlenarGrid(int tipo)
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
                if (!File.Exists(path))
                {
                    RadMessageBox.Show("File not found! Trazabilidad.xml");
                    log.Error("El fichero de trazabilidad no existe en el path:" + path);
                    return;
                }
                switch (tipo)
                {
                    case 1:
                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.FECHA>='" + DateTime.Today.ToString("yyyyMMdd") + "' AND PL.FECHA<'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;

                    case 2:
                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.IDPACKINGESTADO='PK'";
                        break;

                    case 3:
                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.IDPACKINGESTADO='PK' AND PL.FECHA>='" + DateTime.Today.ToString("yyyyMMdd") + "' AND PL.FECHA<'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;

                    case 4:
                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.IDPACKINGESTADO='PK' AND CL.IDCARGA =-1 AND PL.FECHA>='" + DateTime.Today.ToString("yyyyMMdd") + "' AND PL.FECHA<'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;

                    case 5:
                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.IDPACKINGESTADO='PK' AND CL.IDCARGA > 0 AND PL.FECHA>='" + DateTime.Today.ToString("yyyyMMdd") + "' AND PL.FECHA<'" + DateTime.Today.AddDays(1).ToString("yyyyMMdd") + "'";
                        break;

                    case 6:
                        PorNum porNum = new PorNum(gridPacking, "Packing List por Número y Nivel");
                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.IDPACKINGLIST=null AND PL.NIVEL=null";
                        porNum.label1.Text = Lenguaje.traduce("Id Packing List:");
                        porNum.label2.Text = Lenguaje.traduce("Nivel:");

                        porNum.ShowDialog();

                        packingListNum = porNum.campo1;
                        nivel = porNum.campo2;

                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.IDPACKINGLIST='" + packingListNum + "' AND PL.NIVEL='" + nivel + "'";
                        gridPacking.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        break;

                    case 7:

                        PorFecha porFecha = new PorFecha(gridPacking, "Packing List por Fecha de Creación");
                        porFecha.ShowDialog();

                        dateString = porFecha.dateString;
                        dateStringMañana = porFecha.dateStringMañana;

                        if (dateString != null)
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.FECHA>='" + dateString + "' AND PL.FECHA<'" + dateStringMañana + "'";
                            gridPacking.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                        }
                        else
                        {
                            q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.FECHA = ''";
                        }

                        break;

                    case 8:
                        PorUbicacionActual porUbicacionActual = new PorUbicacionActual(gridPacking);
                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PC.REFERENCIA='' AND SERIE=''";
                        break;

                    case 9:
                        /*
                           PorEstadoDevCliente porEstado = new PorEstadoDevCliente(gridPacking, "Packing List por estado");
                           q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE IDPACKINGESTADO = null";
                           estadosList.DataSource = estados;
                           porEstado.ShowDialog();

                           if (estadosList.SelectedItem != null)
                           {
                               q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE IDPACKINGESTADO = '" + estadosList.SelectedItem.Text + "'";
                               gridPacking.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                           }
                           else
                           {
                               q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE IDPACKINGESTADO = null";
                           }
                           gridPacking.DataSource = ConexionSQL.getDataTable(q).DefaultView;
                           */

                        PorEstadoPacking porEstado = new PorEstadoPacking(gridPacking);

                        q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE IDPACKINGESTADO = null";
                        break;

                    default:
                        //q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST");
                        break;
                }
                DataTable dt = ConexionSQL.getDataTable(q);
                Utilidades.TraducirDataTableColumnName(ref dt);
                gridPacking.DataSource = dt.DefaultView;
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
            this.Name = "Packing";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);
        }
    }
}
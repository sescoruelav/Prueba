/*
 * Version inicial Pablo Boix Dic 2020
 *
 * TODO Revisar catch exceptions
 * DONE Ajuste grids a paneles
 * DONE Abrir maximizado.
 * DONE filtro pool no funciona
 * DONE V1.0  tamaño de las descripciones de pedido y articulo mas grandes
 * DONE negrita en lineas de totales
 * TODO presentaciones
 * DONE ocultar campos: DisponibilidadInicial
 * DONE no funcionan los totales de lineas_ok de ordenes ni articulos.
 * DONE que no se puedan lanzar dos organizadores
 * DONE que el jerarquico de articulos no esté abierto por defecto.
 * DONE Añadir progress bar lineas, unidades, peso, volumen, reposiciones
 * DONE volver a conectar el click y doble clic de pedidos y articulos. OK El boton de bajar no va
 * TODO parametrizar limites.desde cero.
 * DONE lineasok de grid pedidos no puede ser modificable   OK
 * DONE Añadir lista de pedidos a la ola como si fuese manual desde el main para depuracion 1º
 * DONE Vedificar que los totales van bien
 * DONE verificar que subir y bajar va bien. lanza en curso
 * DONE exportar a excel grids  (Por ahora se exportan el rdenes y ordenes lin) 3 excels con datatable de articulosorden
 * DONE No se debe poder añadir manualmente lineas a los pedidos
 * DONE las lineas no muestran todas las lineas (no las repetidas)
 * DONE los contadores de lineas van mal: Problemas con la vista de picking cuando da mas de una fila
 * DONE Botón de configuracion de los parametros.
 *             ficheroaEditar = @"C:\Rumbo\Configs\organizadorAlgs.json";
 *          AuxEditor editor5 = new AuxEditor(ficheroaEditar, AuxType.DataTable);
 * DONE      ficheroaEditar = @"C:\Rumbo\Configs\organizadorLims.json";
            AuxEditor editor6 = new AuxEditor(ficheroaEditar, AuxType.DataTable);
 * DONE un simple clic no debe marcar los articulos de un pedido
 * DONE se ha perdido  la funcionalidad de quitar un pedido RECUPERAR funcionalidad llamando a la funcion quitar pedido
* TODO deberiamos tener la linea del pedido en el datatable dtOrdenesArticulo
* DONE permisos botones
* DONE cargar cantidad pendiente de generar no la cantidad del pedido
* DONE BestFitColumns al añadir todo
* DONE parametrizar rutas
* TODO No cargar pedidos con lineas completas ya generadas
* TODO Refrescar despues de lanzamiento
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using Rumbo.Core.Herramientas.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Herramientas;
using RumboSGA.Maestros;
using RumboSGA.OlasMotor;
using RumboSGA.PedidoCliMotor;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using System.Linq;
using System.Text.RegularExpressions;
using System.Dynamic;

namespace RumboSGA.GestionAlmacen
{
    public partial class rRbnOrganizador : Telerik.WinControls.UI.RadRibbonForm
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TableViewDefinition tableView;
        private ColumnGroupsViewDefinition columnGroupsView;
        private DataTable dtPool;
        private DataTable dtOrdenes;
        private DataTable dtArticulos;
        private DataTable dtOrdenesArticulos;
        private DataTable dtConfigOla = null;
        private DataTable dtFiltros = null;
        private bool lineasNoSatisfechas = false;
        private int maxPriority = 1;
        private Contador Pedidos, Lineas, Peso, Volumen, Unidades, Articulos;
        private DataRow filtroSeleccionado = null;

        public enum tipoOrganizador
        { Recepciones, Pedidos_Cliente, Ordenes_recogida, Fabricacion }

        public rRbnOrganizador(tipoOrganizador tipo)
        {
            try
            {
                InitializeComponent();
                this.WindowState = FormWindowState.Maximized; //maximizado en inicio
                //TODO comentamos esto porque cargaremos a demanda
                AjustarGridPool(tipo);
                AjustarGridOrdenes();
                AjustarGridArticulos();
                AjustarDTOrdenesArticulos();
                JerarquicoDetalleOrdenes();
                JerarquicoDetalleArticulos();
                WireEvents();
                GestionarTraduccion();
                CargarContadores();
                AnyadirFiltrosConfigurables();
#if DEBUG
                addBtn();
#endif
                PermisosAcciones();
                this.Show();//muestro la pantalla aunque esté vacia
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, Lenguaje.traduce("Error cargado organizador. Detalle:") + ex.Message);
            }
        }

        #region Permisos Acciones

        private void PermisosAcciones()
        {
            Permiso(rBtnOla, 500041);
            Permiso(rBntOlaManual, 500040);
            Permiso(rBtnLanzarAgrupado, 500004);
            Permiso(rBtnLanzar, 500005);
            Permiso(rMnuPickingInverso, 30081);
            Permiso(rMnuGenerarReposiciones, 20067);
            Permiso(rBtnPropuestaOla, 500098);
        }

        private void Permiso(RadButtonItem control, int id)
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

        #endregion Permisos Acciones

        #region GestionarTraduccion

        private void GestionarTraduccion()
        {
            rBtnFiltroPool.Text = Lenguaje.traduce(rBtnFiltroPool.Text);
            rBtnAnularSeleccion.Text = Lenguaje.traduce(rBtnAnularSeleccion.Text);
            rBtnAnulaSelArts.Text = Lenguaje.traduce(rBtnAnulaSelArts.Text);
            rBtnAddFullPool.Text = Lenguaje.traduce(rBtnAddFullPool.Text);
            rBtnAjustarOA.Text = Lenguaje.traduce(rBtnAjustarOA.Text);
            rBtnLanzar.Text = Lenguaje.traduce(rBtnLanzar.Text);
            rBtnMoveUp.Text = Lenguaje.traduce(rBtnMoveUp.Text);
            rBtnMoveDown.Text = Lenguaje.traduce(rBtnMoveDown.Text);
            ribbonTab1.Text = Lenguaje.traduce(ribbonTab1.Text);
            ribbonTab2.Text = Lenguaje.traduce(ribbonTab2.Text);
            rrBarCarga.Text = Lenguaje.traduce(rrBarCarga.Text);
            rrBarAjusteOrdenes.Text = Lenguaje.traduce(rrBarAjusteOrdenes.Text);
            rrBarAjusteArticulos.Text = Lenguaje.traduce(rrBarAjusteArticulos.Text);
            rrBarAcciones.Text = Lenguaje.traduce(rrBarAcciones.Text);
            /*rPBPedidos.Text = Lenguaje.traduce(rPBPedidos.Text);
            rPBLineas.Text = Lenguaje.traduce(rPBLineas.Text);
            rPBUnidades.Text = Lenguaje.traduce(rPBUnidades.Text);
            rPBPeso.Text = Lenguaje.traduce(rPBPeso.Text);
            rPBVolumen.Text = Lenguaje.traduce(rPBVolumen.Text);
            rPBReposiciones.Text = Lenguaje.traduce(rPBReposiciones.Text);*/ //se traduce en el Json de configuracion
        }

        #endregion GestionarTraduccion

        #region AjustarGrids

        private void AjustarGridOrdenes()
        {
            rGridOrdenes.EnableFiltering = false;
            rGridOrdenes.ShowFilteringRow = false;
            rGridOrdenes.ShowHeaderCellButtons = true;
            rGridOrdenes.AllowAddNewRow = false;
            rGridOrdenes.AllowEditRow = true;//5/5/21 pablo
            rGridOrdenes.AllowDeleteRow = false;//Capturamos el borrado y lo hacemos custom.
            rGridOrdenes.Dock = DockStyle.Fill;
            rGridOrdenes.EnableAlternatingRowColor = true;
            rGridOrdenes.AllowRowReorder = false;//Los bound datagrids no admiten reorder
            rGridOrdenes.ViewCellFormatting += summaryRowCambioEstilo;
            rGridOrdenes.CellClick += rGridOrdenes_CellClick;
            rGridOrdenes.CellBeginEdit += rGridOrdenes_CellBeginEdit;
            rGridOrdenes.ContextMenuOpening += rGridOrdenes_ContextMenuOpening;
            dtOrdenes = Business.CargaPedidoCliCab(-1);//con -1 cargará una tabla vacia.
            rGridOrdenes.DataSource = dtOrdenes;
            //Añado un formato condicional para que se distingan los seleccionados
            ExpressionFormattingObject obj = new ExpressionFormattingObject("CondPedSeleccionado", "seleccionado =1", false);
            obj.CellBackColor = Color.Yellow;
            obj.CellForeColor = Color.Black;
            this.rGridOrdenes.Columns["referencia"].ConditionalFormattingObjectList.Add(obj);
            //Añado un formato condicional para que se distingan las lineas no ok
            ExpressionFormattingObject obj2 = new ExpressionFormattingObject("CondPedlineasNOK", "lineas > lineas_ok", false);
            obj2.CellBackColor = Color.Red;
            obj2.CellForeColor = Color.White;
            this.rGridOrdenes.Columns["lineas_ok"].ConditionalFormattingObjectList.Add(obj2);
            //Ordeno el grid por priority.
            rGridOrdenes.MasterTemplate.EnableSorting = true;
            SortDescriptor descriptor = new SortDescriptor();
            descriptor.PropertyName = "priority";
            descriptor.Direction = ListSortDirection.Ascending;
            rGridOrdenes.MasterTemplate.SortDescriptors.Add(descriptor);
            //Añado totales para algunas columnas
            AddTotales(rGridOrdenes, "lineas");
            AddTotales(rGridOrdenes, "lineas_OK");
            AddTotales(rGridOrdenes, "peso");
            AddTotales(rGridOrdenes, "volumen");
            AddTotales(rGridOrdenes, "palets");
            AddTotales(rGridOrdenes, "REFERENCIA", GridAggregateFunction.Count);
            //rGridOrdenes.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rGridOrdenes.BestFitColumns(BestFitColumnMode.AllCells);
        }

        private void AddTotales(RadGridView grid, string columna)
        {
            AddTotales(grid, columna, GridAggregateFunction.Sum);
        }

        private void AddTotales(RadGridView grid, string columna, GridAggregateFunction tipoTotal)
        {
            try
            {
                GridViewSummaryItem summary = new GridViewSummaryItem();
                GridViewSummaryRowItem summaryRow;
                summary.Name = columna;
                summary.Aggregate = tipoTotal;
                if (grid.SummaryRowsBottom.Count == 0)
                {
                    summaryRow = new GridViewSummaryRowItem();
                    grid.SummaryRowsTop.Add(summaryRow);
                    grid.SummaryRowsBottom.Add(summaryRow);
                }
                else
                {
                    summaryRow = grid.SummaryRowsTop[0];
                }
                summaryRow.Add(summary);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error en cálculo totales") + ex.Message);
            }
        }

        private void addBtn()
        {
            RadButtonElement añadirOlaMasiva = new RadButtonElement();
            añadirOlaMasiva.Text = Lenguaje.traduce("Añadir Pedidos en ola Prueba");
            añadirOlaMasiva.Click += rbnañadirOla_Click;
            añadirOlaMasiva.Image = new Bitmap(RumboSGA.Properties.Resources.Add);
            añadirOlaMasiva.ImageAlignment = ContentAlignment.MiddleCenter;
            añadirOlaMasiva.DisplayStyle = DisplayStyle.ImageAndText;
            añadirOlaMasiva.TextImageRelation = TextImageRelation.ImageAboveText;
            añadirOlaMasiva.TextAlignment = ContentAlignment.BottomCenter;
            rrBarAcciones.Items.Add(añadirOlaMasiva);
        }

        private void CargarXmlFiltros()
        {
            log.Info("Accede funcion :CargarXmlFiltros");
            string configFiltros = string.Empty;
            AuxEditor editFiltros = null;
            try
            {
                configFiltros = Persistencia.DirectorioBase + @"\Configs\organizadorAlgs.json";//TODO
                editFiltros = new AuxEditor(configFiltros, AuxType.DataTable);
                dtFiltros = editFiltros.getDatatable();
                if (dtFiltros is null)
                    return;
                foreach (DataRow dr in dtFiltros.Rows)
                {
                    RumMenuItem rBtnOpcionCarga = new RumMenuItem();
                    rBtnOpcionCarga.Image = global::RumboSGA.Properties.Resources.addbox;
                    rBtnOpcionCarga.ImageAlignment = ContentAlignment.MiddleCenter;
                    rBtnOpcionCarga.Text = dr["Descripcion"].ToString();
                    rBtnOpcionCarga.Tag = dr["ID"].ToString();
                    rBtnOpcionCarga.TextAlignment = ContentAlignment.BottomCenter;
                    rBtnOpcionCarga.Click += rBtnOpcionCarga_Click;
                    rddBtnAnyadirPredefinidos.Items.Add(rBtnOpcionCarga);
                }

                //Añado manualmente un boton extra para limpiar los filtros
                RumMenuItem rBtnOpcionLimpiarCuadroOrdenes = new RumMenuItem();
                rBtnOpcionLimpiarCuadroOrdenes.Image = global::RumboSGA.Properties.Resources.Delete;
                rBtnOpcionLimpiarCuadroOrdenes.BackColor = Color.IndianRed;
                rBtnOpcionLimpiarCuadroOrdenes.ImageAlignment = ContentAlignment.MiddleCenter;
                rBtnOpcionLimpiarCuadroOrdenes.Text = "Resetear Cuadro de Ordenes";
                rBtnOpcionLimpiarCuadroOrdenes.TextAlignment = ContentAlignment.BottomCenter;
                rBtnOpcionLimpiarCuadroOrdenes.Click += rBtnLimpiarCuadroOrdenes_Click;
                rddBtnAnyadirPredefinidos.Items.Add(rBtnOpcionLimpiarCuadroOrdenes);

                log.Info("FIN funcion :CargarXmlFiltros");
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private void rBtnLimpiarCuadroOrdenes_Click(object sender, EventArgs e)
        {
            try
            {
                dtPool = null;
                this.rGridPoolOrdenes.DataSource = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AnyadirFiltrosConfigurables()
        {
            try
            {
                CargarXmlFiltros();
            }
            catch (Exception ex)
            {
            }
        }

        private void rBtnOpcionCarga_Click(object sender, EventArgs e)
        {
            log.Info("Accede funcion :rBtnOpcionCarga_Click");

            //TODO: esto se deberia hacer con una lista para que el numero de parametros sea extensible a
            //demanda por el creador del fichero
            string LABEL_1 = "LABEL_1";
            string LABEL_2 = "LABEL_2";
            string IDENTIFICADOR_1 = "IDENTIFICADOR_1";
            string IDENTIFICADOR_2 = "IDENTIFICADOR_2";

            try
            {
                RumMenuItem opcion = (RumMenuItem)sender;
                string id = opcion.Tag.ToString();
                log.Info("El id de la opcion elegida es :" + id);
                DataRow[] rowsFiltered = dtFiltros.Select("ID=" + id);
                if (rowsFiltered.Length > 0)
                {
                    filtroSeleccionado = rowsFiltered[0];
                    string tipo = filtroSeleccionado["tipo"].ToString();
                    string descripcion = filtroSeleccionado["descripcion"].ToString();
                    log.Info(String.Format("El tipo es: {0}. y la descripcion es:{1}", tipo, descripcion));

                    FrmPopUpHelper frmComboBoxHelper;

                    switch (tipo)
                    {
                        case "SQL":
                            CargarPoolDesdeSql(filtroSeleccionado["SQL"].ToString());
                            break;

                        case "DATEPICKER":
                            //Open form datetime picking
                            frmComboBoxHelper = new FrmPopUpHelper(descripcion, filtroSeleccionado[LABEL_1].ToString());
                            if (frmComboBoxHelper.ShowDialog() == DialogResult.OK)
                            {
                                //repuerar select la final
                                string querySQL = filtroSeleccionado["SQL"].ToString();
                                string newStringID1 = frmComboBoxHelper.PickedValue.ToString();
                                querySQL = querySQL.Replace(filtroSeleccionado[IDENTIFICADOR_1].ToString(), newStringID1);

                                //Ejecutar la select
                                CargarPoolDesdeSql(querySQL);
                            }
                            break;

                        case "CMB":
                            DataTable dt2;
                            string identificadorCombo2;
                            List<string> columnNamesList2;
                            RecuperaParametrosCombo(out dt2, out identificadorCombo2, out columnNamesList2, IDENTIFICADOR_1);

                            if (columnNamesList2.Count == 0)
                            {
                                MessageBox.Show("Por favor, revisa que haya dos campos seleccionados en el combo: " + identificadorCombo2);
                                break;
                            }

                            frmComboBoxHelper = new FrmPopUpHelper(
                                        descripcion,
                                        filtroSeleccionado[LABEL_1].ToString(),
                                        dt2, identificadorCombo2, columnNamesList2[0]);

                            if (frmComboBoxHelper.ShowDialog() == DialogResult.OK)
                            {
                                //repuerar select la final
                                string querySQL = filtroSeleccionado["SQL"].ToString();
                                string newStringID1 = frmComboBoxHelper.SelectedIndex.ToString();
                                querySQL = querySQL.Replace(filtroSeleccionado[IDENTIFICADOR_1].ToString(), newStringID1);

                                //Ejecutar la select
                                CargarPoolDesdeSql(querySQL);
                            }
                            break;

                        case "CMB,DATEPICKER":

                            DataTable dt;
                            string identificadorCombo;
                            List<string> columnNamesList;
                            RecuperaParametrosCombo(out dt, out identificadorCombo, out columnNamesList, IDENTIFICADOR_1);

                            if (columnNamesList.Count == 0)
                            {
                                MessageBox.Show("Por favor, revisa que haya dos campos seleccionados en el combo: " + identificadorCombo);
                                break;
                            }

                            frmComboBoxHelper = new FrmPopUpHelper(
                                        descripcion,
                                        filtroSeleccionado[LABEL_1].ToString(),
                                        dt, identificadorCombo, columnNamesList[0], filtroSeleccionado[LABEL_2].ToString());

                            if (frmComboBoxHelper.ShowDialog() == DialogResult.OK)
                            {
                                //repuerar select la final
                                string querySQL = filtroSeleccionado["SQL"].ToString();

                                string newStringID1 = frmComboBoxHelper.SelectedIndex.ToString();
                                string newStringID2 = frmComboBoxHelper.PickedValue.ToString();

                                querySQL = querySQL.Replace(filtroSeleccionado[IDENTIFICADOR_1].ToString(), newStringID1);
                                querySQL = querySQL.Replace(filtroSeleccionado[IDENTIFICADOR_2].ToString(), newStringID2);

                                //Ejecutar la select
                                CargarPoolDesdeSql(querySQL);
                            }

                            break;

                        default:
                            break;
                    }
                }

                log.Info("FIN funcion :rBtnOpcionCarga_Click");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void RecuperaParametrosCombo(out DataTable dt, out string identificadorCombo, out List<string> columnNamesList, string identificador)
        {
            //Ejecuta select del fichero
            string query = filtroSeleccionado["CMBSQL"].ToString();
            dt = ConexionSQL.getDataTable(query);

            //Obtengo el identificador para construir el combo
            identificadorCombo = filtroSeleccionado[identificador].ToString().Trim(new Char[] { '[', ']', ' ' });

            //Obtengo las columnas que se muestran para visualizacion en el combo
            columnNamesList = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName).ToList();
            //Elimino el identificador ya que no hay que mostrarlo
            columnNamesList.Remove(identificadorCombo);
        }

        private void CargarPoolDesdeSql(string query)
        {
            log.Info("Accede funcion: CargarPoolDesdeSql");
            try
            {
                CargaPreparaciones(query);
                log.Info("FIN funcion: CargarPoolDesdeSql");
            }
            catch (Exception ex)
            {
            }
        }

        private void AjustarGridArticulos()
        {
            this.rGridArticulos.EnableFiltering = true;
            this.rGridArticulos.ShowFilteringRow = false;
            this.rGridArticulos.ShowHeaderCellButtons = true;
            this.rGridArticulos.AllowAddNewRow = false;
            this.rGridArticulos.AllowEditRow = false;
            this.rGridArticulos.Dock = DockStyle.Fill;
            this.rGridArticulos.EnableAlternatingRowColor = true;
            this.rGridArticulos.ViewCellFormatting += summaryRowCambioEstilo;
            this.rGridArticulos.CellDoubleClick += rGridArticulos_CellDoubleClick;
            dtArticulos = Business.CargaPedidoCliLin(-1);//con -1 cargará una tabla vacia.
                                                         //dtArticulos.Columns.Remove("priority"); //La columna prioridad no tiene sentido en articulos pero como la funcion se comparte de business la dejo
            rGridArticulos.DataSource = dtArticulos;
            rGridArticulos.Columns["priority"].IsVisible = false; //La oculto
            rGridArticulos.Columns["disponible_inicial"].IsVisible = false; //La oculto
                                                                            //Añado un formato condicional para que se distingan los que no hay suficiente stock
            ExpressionFormattingObject obj = new ExpressionFormattingObject("CondInsuficiente", "disponible < 0", false);
            obj.CellBackColor = Color.Red;
            obj.CellForeColor = Color.White;
            this.rGridArticulos.Columns["solicitado"].ConditionalFormattingObjectList.Add(obj);
            //Añado un formato condicional para que se distingan los que no hay suficiente stock
            ExpressionFormattingObject obj2 = new ExpressionFormattingObject("condLin_ok", "lineas > lineas_ok", false);
            obj2.CellBackColor = Color.Red;
            obj2.CellForeColor = Color.White;
            this.rGridArticulos.Columns["lineas_ok"].ConditionalFormattingObjectList.Add(obj2);
            ExpressionFormattingObject obj3 = new ExpressionFormattingObject("CondArtSeleccionado", "seleccionado =1", false);
            obj3.CellBackColor = Color.Yellow;
            obj3.CellForeColor = Color.Black;
            this.rGridArticulos.Columns["referencia"].ConditionalFormattingObjectList.Add(obj3);
            //AddTotales(rGridArticulos, "disponible");//No tiene sentido
            AddTotales(rGridArticulos, "lineas");
            AddTotales(rGridArticulos, "lineas_OK");//TODO No funciona
            AddTotales(rGridArticulos, "uds");
            AddTotales(rGridArticulos, "cajas");
            AddTotales(rGridArticulos, "palets");
            AddTotales(rGridArticulos, "referencia", GridAggregateFunction.Count);
            rGridArticulos.BestFitColumns(BestFitColumnMode.AllCells);
        }

        private void AjustarGridPool(tipoOrganizador tipo)
        {
            this.rGridPoolOrdenes.EnableFiltering = true;
            this.rGridPoolOrdenes.MasterTemplate.EnableFiltering = true;
            this.rGridPoolOrdenes.ShowFilteringRow = false;
            this.rGridPoolOrdenes.ShowHeaderCellButtons = true;
            this.rGridPoolOrdenes.AllowAddNewRow = false;
            this.rGridPoolOrdenes.AllowEditRow = false;
            this.rGridPoolOrdenes.Dock = DockStyle.Fill;
            this.rGridPoolOrdenes.EnableAlternatingRowColor = true;
            this.rGridPoolOrdenes.CellDoubleClick += rGridPoolOrdenes_CellDoubleClick;
            //this.rGridPoolOrdenes.FilterChanged += RGridPoolOrdenes_FilterChanged;

            //doble linea
            //grupo de cabecera
            this.columnGroupsView = new ColumnGroupsViewDefinition();
            this.columnGroupsView.ColumnGroups.Add(new GridViewColumnGroup("General"));
            GridViewColumnGroupRow groupRow = new GridViewColumnGroupRow();                 //linea 0
            groupRow.MinHeight = 50;
            this.columnGroupsView.ColumnGroups[0].Rows.Add(groupRow);                       //agrego la linea 0 al grupo
            groupRow = new GridViewColumnGroupRow();                                        //linea 1
            groupRow.MinHeight = 50;
            this.columnGroupsView.ColumnGroups[0].Rows.Add(groupRow);                       //agrego la linea 1 al grupo
            this.columnGroupsView.ColumnGroups[0].Rows[0].ColumnNames.Add("cliente");       //agrego columna a la linea 0
            this.columnGroupsView.ColumnGroups[0].Rows[1].ColumnNames.Add("serie");         //agrego columna a la linea 1
            this.columnGroupsView.ColumnGroups[0].Rows[1].ColumnNames.Add("referencia");
            this.tableView = (TableViewDefinition)this.rGridPoolOrdenes.ViewDefinition;
            this.rGridPoolOrdenes.ViewDefinition = columnGroupsView;
            this.rGridPoolOrdenes.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            //Cargo el resto de columnas

            CargaPool(tipo, "", true);
            //añado un filtro para que las ordenes ya selecionadas desaparezcan
            //este filtro se activa y desactiva  con un botón. De inicio estará activado.
            FilterDescriptor descriptor = new FilterDescriptor("seleccionado", FilterOperator.IsEqualTo, 0);
            this.rGridPoolOrdenes.FilterDescriptors.Add(descriptor);
            //Pongo un formato condicional para que se distingan los añadidos
            ExpressionFormattingObject obj = new ExpressionFormattingObject("CondSeleccionado", "seleccionado > 0", false);
            obj.CellBackColor = Color.SkyBlue;
            obj.CellForeColor = Color.Red;
            this.rGridPoolOrdenes.Columns["cliente"].ConditionalFormattingObjectList.Add(obj);
        }

        private void AjustarDTOrdenesArticulos()
        {   //creo una tabla en memoria con la relacion entre ordenes y articulos para hacer hacia atrás rapidamente.
            try
            {
                dtOrdenesArticulos = new DataTable("OrdenesArticulos");
                dtOrdenesArticulos.Columns.Add("priority", typeof(Int32));
                dtOrdenesArticulos.Columns.Add("clase", typeof(String));
                dtOrdenesArticulos.Columns.Add("idOrden", typeof(Int32));
                //TODO deberiamos tener la linea del pedido
                //dtOrdenesArticulos.Columns.Add("idOrdenLin", typeof(Int32));
                dtOrdenesArticulos.Columns.Add("orden", typeof(String));
                dtOrdenesArticulos.Columns.Add("empresa", typeof(String));
                dtOrdenesArticulos.Columns.Add("idArticulo", typeof(Int32));
                dtOrdenesArticulos.Columns.Add("referencia", typeof(String));
                dtOrdenesArticulos.Columns.Add("Articulo", typeof(String));
                dtOrdenesArticulos.Columns.Add("disponible", typeof(Int32));//disponible antes de asignar
                dtOrdenesArticulos.Columns.Add("solicitado", typeof(Int32));
                dtOrdenesArticulos.Columns.Add("asignado", typeof(Int32));
            }
            catch (Exception ex)
            {
                throw new Exception("Error ajustando ordenes artículos " + ex.Message);
            }
        }

        private void JerarquicoDetalleOrdenes()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.DataSource = dtOrdenesArticulos;
            template.AllowAddNewRow = false;
            rGridOrdenes.MasterTemplate.Templates.Add(template);
            GridViewRelation relation = new GridViewRelation(rGridOrdenes.MasterTemplate);
            relation.ChildTemplate = template;
            relation.RelationName = "OrdenesArticulos";
            relation.ParentColumnNames.Add("idOrden");//TODO deberia relacionarse tambien por clase pq un id puede estar en cliente y proveedor
            relation.ChildColumnNames.Add("idOrden");
            template.Columns["priority"].IsVisible = false;
            template.Columns["clase"].IsVisible = false;
            template.Columns["idorden"].IsVisible = false;
            template.Columns["orden"].IsVisible = false;
            template.Columns["empresa"].IsVisible = false;
            template.Columns["idarticulo"].IsVisible = false;
            template.BestFitColumns();
            rGridOrdenes.Relations.Add(relation);
        }

        private void JerarquicoDetalleArticulos()
        {
            GridViewTemplate template = new GridViewTemplate();
            template.DataSource = dtOrdenesArticulos;
            rGridArticulos.MasterTemplate.Templates.Add(template);
            GridViewRelation relation = new GridViewRelation(rGridArticulos.MasterTemplate);
            template.AllowAddNewRow = false;
            relation.ChildTemplate = template;
            relation.RelationName = "OrdenesArticulos";
            relation.ParentColumnNames.Add("idArticulo");//TODO deberia relacionarse tambien por clase pq un id puede estar en cliente y proveedor
            relation.ChildColumnNames.Add("idarticulo");
            template.Columns["priority"].IsVisible = true;
            template.Columns["clase"].IsVisible = true;
            template.Columns["idorden"].IsVisible = false;
            template.Columns["orden"].IsVisible = true;
            template.Columns["empresa"].IsVisible = true;
            template.Columns["idarticulo"].IsVisible = false;
            template.Columns["referencia"].IsVisible = false;
            template.Columns["articulo"].IsVisible = false;
            //Ordeno el grid por priority.
            template.EnableSorting = true;
            SortDescriptor descriptor = new SortDescriptor();
            descriptor.PropertyName = "priority";
            descriptor.Direction = ListSortDirection.Ascending;
            template.SortDescriptors.Add(descriptor);
            template.BestFitColumns();
            rGridArticulos.Relations.Add(relation);
        }

        private void rGridPoolOrdenes_CustomFiltering(object sender, GridViewCustomFilteringEventArgs e)
        {
            e.Visible = (int)e.Row.Cells["seleccionado"].Value == 0;
        }

        #endregion AjustarGrids

        #region accionesForm

        private void rGridPoolOrdenes_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            bool ok;
            bool ajustar = false;
            try
            {
                //si no hay fila actual salimos
                if (rGridPoolOrdenes.CurrentCell is null)
                    return;
                //si es el primer elemento que inserto luego ajusto, solo una vez.
                ajustar = dtOrdenes.Rows.Count == 0;
                string celda = rGridPoolOrdenes.CurrentCell.Name;
                string clase = Convert.ToString(rGridPoolOrdenes.CurrentRow.Cells["CLASE"].Value);
                switch (clase.ToUpper())
                {
                    case "PEDIDOS_CLIENTE":
                        int idPedidoCli = Convert.ToInt32(rGridPoolOrdenes.CurrentRow.Cells["idOrden"].Value);
                        if ((int)(rGridPoolOrdenes.CurrentRow.Cells["seleccionado"].Value) == 1)
                        {
                            QuitarPedidoCli(clase, idPedidoCli);
                            rGridPoolOrdenes.CurrentRow.Cells["seleccionado"].Value = 0;
                        }
                        else
                        {
                            ok = AddPedidoCli(idPedidoCli);
                            if (ok)
                            {
                                rGridPoolOrdenes.CurrentRow.Cells["seleccionado"].Value = 1;
                                rGridOrdenes.BestFitColumns(BestFitColumnMode.AllCells);
                            }
                        }

                        break;

                    case "PEDIDOS PRO.":
                        break;
                }
                if (ajustar)
                {
                    rGridOrdenes.BestFitColumns();
                    rGridArticulos.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, Lenguaje.traduce("Error añadiendo pedido. ERROR:") + ex.Message);
            }
        }

        private void rGridPoolOrdenes_DoubleClick(object sender, EventArgs e)
        {
        }

        private void rBtnFiltroPool_Click(object sender, EventArgs e)
        {
            //este botón activa y desactiva el filtro
            //que hace que los objetos ya seleccionados del pool sean visibles o invisibles
            if (rGridPoolOrdenes.FilterDescriptors.Expression.Contains("[seleccionado] = 1"))
                rGridPoolOrdenes.FilterDescriptors.Expression = rGridPoolOrdenes.FilterDescriptors.Expression.Replace("[seleccionado] = 1", "[seleccionado] = 0");
            else
                rGridPoolOrdenes.FilterDescriptors.Expression = rGridPoolOrdenes.FilterDescriptors.Expression.Replace("[seleccionado] = 0", "[seleccionado] = 1");
        }

        private void rGridArticulos_DoubleClick(object sender, EventArgs e)
        {
        }

        private void rGridArticulos_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            //al hacer click sobre una fila de articulos, enciende o apaga los pedidos relacionados con ese articulo.
            //para que sea facil usaremos un campo que llamo seleccionado
            int idArticulo = 0;
            try
            {
                rGridArticulos.CurrentRow.Cells["seleccionado"].Value = 1;
                idArticulo = Convert.ToInt32(rGridArticulos.CurrentRow.Cells["idArticulo"].Value);
                DataRow[] drOrdenes = dtOrdenesArticulos.Select("idarticulo=" + idArticulo);
                foreach (DataRow drOA in drOrdenes)
                {
                    SeleccionaOrden((string)drOA["clase"], (int)drOA["idOrden"], 1);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        #endregion accionesForm

        public bool AddPedidoCli(int idPedidoCli)
        {
            int lineas;
            int lineasOk;
            string orden;
            string empresa;
            try
            {
                //buscamos el pedido, insertamos/actualizamos las lineas en el grid de articulos y añadimos en el grid de pedidos
                DataTable dtCab = Business.CargaPedidoCliCab(idPedidoCli);
                foreach (DataRow dr in dtCab.Rows)
                {
                    //lineas = AddPedidoCliLin(idPedidoCli,ref lineasOk);
                    orden = Convert.ToString(dr["serie"]) + "-" + Convert.ToString(dr["referencia"]);
                    empresa = Convert.ToString(dr["cliente"]);
                    lineas = AddPedidoCliLin(idPedidoCli, orden, empresa, maxPriority, out lineasOk);
                    //Lineas.incrementa(lineasOk);
                    Incrementa("lineas", lineasOk);
                    dr["lineas_ok"] = lineasOk;
                    dr["lineas"] = lineas;
                    dr["priority"] = maxPriority++;

                    //Reviso si peso y volumen vienen a nivel de cabecera para incrementarlo aqui
                    double peso = 0;
                    double volumen = 0;
                    if (!Persistencia.getParametroBoolean("ES_PESO_CALCULADO_OLA"))
                    {
                        peso = Convert.ToDouble(dr["peso"]);
                        Incrementa("peso", peso);
                    }
                    if (!Persistencia.getParametroBoolean("ES_VOLUMEN_CALCULADO_OLA"))
                    {
                        volumen = Convert.ToDouble(dr["volumen"]);
                        Incrementa("volumen", volumen);
                    }

                    rGridOrdenes.Rows.Add(dr.ItemArray);
                }
                //Pedidos.incrementa(1);
                Incrementa("Pedidos", 1);
                //refresco pantalla para que se muestre movimiento mientras se añaden pedidos
                splitPanel2.Refresh();
                rGridArticulos.Refresh();
                splitPanel3.Refresh();
                splitPanel4.Refresh();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error añadiendo Pedido con id :" + idPedidoCli + "\n" + ex.Message);
            }
        }

        private bool QuitarPedidoCli(string clase, int idPedidoCli)
        {
            int idArticulo;
            DataRow[] drArts;
            try
            {
                // PASO 1: quito las lineas del pedido de articulos
                DataRow[] dtLins = dtOrdenesArticulos.Select("clase='" + clase + "' and idOrden=" + idPedidoCli);
                foreach (DataRow drLin in dtLins)
                {
                    idArticulo = Convert.ToInt32(drLin["idarticulo"]);
                    //busco el idarticulo en articulos
                    drArts = dtArticulos.Select("idarticulo= " + idArticulo);
                    if (drArts.Length > 0)//solo deberia devolver un datarow en el array
                    {
                        //Actualizo limite "unidades" para que el display sea coherente
                        int asignado = Convert.ToInt32(drLin["asignado"]);
                        Incrementa("unidades", -asignado);

                        //Si el peso esta informado en la linea, actualizo limites "peso" aqui
                        if (Persistencia.getParametroBoolean("ES_PESO_CALCULADO_OLA"))
                        {
                            Incrementa("Peso", -asignado * Convert.ToDouble(drArts[0]["peso"]));
                        }
                        //Si el volumen esta informado en la linea, actualizo limites "volumen" aqui
                        if (Persistencia.getParametroBoolean("ES_VOLUMEN_CALCULADO_OLA"))
                        {
                            Incrementa("Volumen", -asignado * Convert.ToDouble(drArts[0]["volumen"]));
                        }

                        drArts[0]["asignado"] = Convert.ToInt32(drArts[0]["asignado"]) - Convert.ToInt32(drLin["asignado"]);
                        drArts[0]["lineas"] = Convert.ToInt32(drArts[0]["lineas"]) - 1;
                        if (Convert.ToInt32(drArts[0]["lineas"]) > 0)
                        {
                            drArts[0]["disponible"] = Convert.ToInt32(drArts[0]["disponible"]) + Convert.ToInt32(drLin["asignado"]);
                            if (lineasNoSatisfechas)//TODO posible mejora si disponible>lin asignado no faltaba
                                drArts[0]["lineas_ok"] = RecalculaLineasOk();
                            else
                                drArts[0]["lineas_OK"] = Convert.ToInt32(drArts[0]["lineas_OK"]) - 1;
                        }
                        else
                        {
                            drArts[0].Delete();

                            //Actualizo limites "articulos" ya que no quedan articulo de este id
                            Incrementa("Articulos", -1);
                        }
                    }
                    //quito la linea de la datatable ordenesArticulos
                    drLin.Delete();
                }

                //PASO 2: Quitamos el pedido de las ordenes
                //selecciono pedido
                DataRow drOrds = dtOrdenes.Select("clase='" + clase + "' and idorden=" + idPedidoCli).FirstOrDefault();

                if (drOrds != null)
                {
                    //Actualizo lineas_ok para que el display sea coherente
                    Incrementa("lineas", -Convert.ToInt32(drOrds["lineas_ok"]));

                    //Actualizo peso y volumen si vienen en cabecera
                    if (!Persistencia.getParametroBoolean("ES_PESO_CALCULADO_OLA"))
                    {
                        double peso = Convert.ToDouble(drOrds["peso"]);
                        Incrementa("peso", -peso);
                    }
                    if (!Persistencia.getParametroBoolean("ES_VOLUMEN_CALCULADO_OLA"))
                    {
                        double volumen = Convert.ToDouble(drOrds["volumen"]);
                        Incrementa("volumen", -volumen);
                    }

                    //Quito el pedido de ordenes
                    drOrds.Delete();

                    //Pedidos.incrementa(-1);
                    Incrementa("Pedidos", -1);
                    //ContadorPedidos(-1);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Test" + ex.Message);//TODO gestionar
                return false;
            }
        }

        private int RecalculaLineasOk()
        {
            //se da por hecho que esta funcion intercambia solo entre dos ordenes contiguas en orden de priority
            //El resultado de la orden siguiente y anterior a ambas no se ve afectado por el orden entre ambas
            try
            {
                // si no hay lineas no satisfechas no hay nada que recalcular
                if (!lineasNoSatisfechas)
                    return (0);
                //si hay lineas no satisfechas podria afectar solo a algunos artículos.

                //reiniciar la disponibilidad de articulo
                foreach (DataRow drArt in dtArticulos.Rows)
                {
                    drArt["disponible"] = drArt["disponible_inicial"];
                }
                //recorrer las ordenes en orden realculando lineas articulos y ordenes.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return 0;
        }

        private int AddPedidoCliLin(int idPedidoCli, string orden, string empresa, int priority, out int lineasOk)
        {
            int idArticulo;
            int lineas = 0;
            int solicitadoLin = 0, asignado = 0;
            int asignadoLin;
            int disponibleInicial;
            string referencia;
            string descripcion;
            lineasOk = 0;
            DataTable dtLin;
            try
            {
                if (filtroSeleccionado != null)
                {
                    string queryFiltrada = filtroSeleccionado["SQLLINEAS"].ToString();
                    dtLin = Business.CargaPedidoCliLin(idPedidoCli, queryFiltrada);
                }
                else
                {
                    dtLin = Business.CargaPedidoCliLin(idPedidoCli);
                }
                foreach (DataRow drLin in dtLin.Rows)
                {
                    lineas++;
                    idArticulo = Convert.ToInt32(drLin["idarticulo"]);
                    solicitadoLin = Convert.ToInt32(drLin["solicitado"]);
                    referencia = Convert.ToString(drLin["referencia"]);
                    descripcion = Convert.ToString(drLin["articulo"]);
                    //compruebo si ese articulo ya está
                    DataRow[] drArts = dtArticulos.Select("idarticulo= " + idArticulo);
                    if (drArts.Length == 0)
                    {
                        //añadir al recordset
                        //AjustarGridArticulos contadores
                        //Articulos.incrementa(1);
                        Incrementa("Articulos", 1);
                        drLin["lineas"] = 1;
                        if (Convert.ToInt32(drLin["disponible"]) >= Convert.ToInt32(drLin["solicitado"]))
                        {
                            lineasOk++;
                            drLin["lineas_ok"] = 1;
                        }
                        else
                            lineasNoSatisfechas = true;
                        asignadoLin = Math.Min(solicitadoLin, Convert.ToInt32(drLin["disponible"]));
                        disponibleInicial = Convert.ToInt32(drLin["disponible"]);
                        drLin["asignado"] = asignadoLin;
                        drLin["disponible"] = disponibleInicial - asignadoLin;
                        drLin["solicitado"] = solicitadoLin;
                        drLin["priority"] = priority;
                        //addOrdenArticulo(tipoOrganizador.Pedidos_Cliente, priority, idPedidoCli, orden, empresa, idArticulo, referencia, descripcion, disponibleInicial, solicitadoLin, asignadoLin);// Movido de linea 624; Rubén 07/05/2021
                        //TODO calcular reposiciones.
                        //drArts[0]["reposiciones"] = CalculaReposiciones(asignado, Convert.ToInt32(drArts[0]["enPicking"]), Convert.ToInt32(drArts[0]["reposiciones"]), Convert.ToInt32(drArts[0]["paletsCompletos"]), Convert.ToInt32(drArts[0]["cantPalet"]));
                        rGridArticulos.Rows.Add(drLin.ItemArray);
                    }
                    else
                    {
                        disponibleInicial = Convert.ToInt32(drArts[0]["disponible"]);
                        asignadoLin = Math.Min(solicitadoLin, disponibleInicial);
                        asignado = Convert.ToInt32(drArts[0]["asignado"]) + asignadoLin;
                        drArts[0]["asignado"] = asignado;
                        drArts[0]["disponible"] = Convert.ToInt32(drArts[0]["disponible"]) - asignadoLin;
                        drArts[0]["solicitado"] = Convert.ToInt32(drArts[0]["solicitado"]) + solicitadoLin;
                        drArts[0]["lineas"] = Convert.ToInt32(drArts[0]["lineas"]) + 1;
                        drArts[0]["reposiciones"] = CalculaReposiciones(asignado, Convert.ToInt32(drArts[0]["enPicking"]), Convert.ToInt32(drArts[0]["reposiciones"]), Convert.ToInt32(drArts[0]["paletsCompletos"]), Convert.ToInt32(drArts[0]["cantPalet"]));
                        if (asignadoLin == solicitadoLin)
                        {
                            lineasOk++;
                            drArts[0]["lineas_ok"] = Convert.ToInt32(drArts[0]["lineas_ok"]) + 1;
                        }
                        else
                            lineasNoSatisfechas = true;
                    }

                    //Incrementa unidades
                    Incrementa("Unidades", asignadoLin);

                    //Si el peso esta informado en la linea, hago aqui los calculo
                    if (Persistencia.getParametroBoolean("ES_PESO_CALCULADO_OLA"))
                    {
                        Incrementa("Peso", asignadoLin * Convert.ToDouble(drLin["peso"]));
                    }
                    //Si el volumen esta informado en la linea, hago aqui los calculos
                    if (Persistencia.getParametroBoolean("ES_VOLUMEN_CALCULADO_OLA"))
                    {
                        Incrementa("Volumen", asignadoLin * Convert.ToDouble(drLin["volumen"]));
                    }

                    addOrdenArticulo(tipoOrganizador.Pedidos_Cliente, priority, idPedidoCli, orden, empresa, idArticulo, referencia, descripcion, disponibleInicial, solicitadoLin, asignadoLin);
                    rGridArticulos.MasterTemplate.CollapseAll();
                }
                return lineas;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            return lineas;
        }

        private int CalculaReposiciones(int asignado, int picking, int reposiciones, int paletsCompletos, int udsPalet)
        {
            int saldoPicking = picking - asignado - (paletsCompletos * udsPalet) - (paletsCompletos * udsPalet) + (reposiciones * udsPalet);
            if (saldoPicking < 0)
            {
                reposiciones = -1 * saldoPicking / udsPalet;
            }
            return reposiciones;
        }

        private bool addOrdenArticulo(tipoOrganizador tipo, int priority, int idPedidoCli, string orden, string empresa, int idArticulo, string referencia, string descripcion, int disponibleInicial, int solicitado, int asignado)
        {
            try
            {
                DataRow dr = dtOrdenesArticulos.NewRow();
                dr["clase"] = tipo.ToString();
                dr["priority"] = priority;
                dr["idOrden"] = idPedidoCli;
                dr["orden"] = orden;
                dr["empresa"] = empresa;
                dr["idArticulo"] = idArticulo;
                dr["referencia"] = referencia;
                dr["Articulo"] = descripcion;
                dr["disponible"] = disponibleInicial;
                dr["solicitado"] = solicitado;
                dr["asignado"] = asignado;
                dtOrdenesArticulos.Rows.Add(dr);
                rGridArticulos.MasterTemplate.CollapseAllGroups();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error añadiendo orden al artículo.ERROR:" + ex.Message);
            }
        }

        private bool CargaPool(tipoOrganizador tipo, string query, bool soloEstructura)
        {
            switch (tipo)
            {
                case tipoOrganizador.Recepciones:
                    return CargaRecepciones();

                case tipoOrganizador.Pedidos_Cliente:
                    return CargaPreparaciones(query, soloEstructura);

                default:
                    return false;
            }
        }

        private bool CargaRecepciones()
        {
            try
            {
                dtPool = Business.CargaPoolPedidosClientePtes();
                if (dtPool is null)
                    return false;
                this.rGridPoolOrdenes.DataSource = dtPool;
                //cargar el grid
                //formatear el grid
                return true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Error cargando recepciones.ERROR:" + ex.Message);
            }
            return false;
        }

        private bool CargaPreparaciones(String query, bool soloEstructura = false)
        {
            try
            {
                log.Info(String.Format("Accede funcion: CargaPreparaciones. query:{0} , soloestructura:{1}", query, soloEstructura.ToString()));
                //TODO llamar al WS que nos devuelve el json los pedidos pendientes
                //TODO cargar el WS añadiendo a lo que ya tenga wl pool. Para vaciar el pool te sales!
                /* Version antigua: Sustituir por WS*/
                DataTable dtNewPool = Business.CargaPoolPedidosClientePtes(query, soloEstructura);

                if (dtPool == null)
                {
                    log.Info("dtPool es nulo. Es la primera vez o ha habido una limpieza del pool. Se cargan pedidos Clientes pendientes");
                    dtPool = Business.CargaPoolPedidosClientePtes(query, soloEstructura); //El nombre está mal puesto no son recepciones, son expediciones o preparaciones pendientes.
                    if (dtPool is null)
                        return false;
                }

                //Reviso si el datasource es nulo o no para ver si tengo que añadir o puedo hacer el binding directamente
                if (this.rGridPoolOrdenes.DataSource != null)
                {
                    //En caso de que no sea nulo, tengo que revisar esta la fila en la tabla, si no esta, la añado
                    //TODO:Busco por idOrden. es buena idea??
                    string idColumnName = "idorden";
                    for (int i = 0; i < dtNewPool.Rows.Count; i++)
                    {
                        int idOrden = dtNewPool.Rows[i].Field<int>(idColumnName);
                        bool contains = dtPool.AsEnumerable().Any(row => idOrden == row.Field<int>(idColumnName));

                        log.Info("Añadiendo al pool idOrden:" + idOrden.ToString());

                        if (!contains)
                        {
                            dtPool.Rows.Add(dtNewPool.Rows[i].ItemArray);
                        }
                        else
                        {
                            log.Info("No se añade porque ya existe");
                        }
                    }
                }
                else
                {
                    log.Info("rGridPoolOrdenes datasource es nulo. Hago binding a dtpool");
                    this.rGridPoolOrdenes.DataSource = dtPool;
                }

                //serializaDatatable(dtPool, @"C:\Rumbo\Configs\Cronus2020_91\poolPedidos.json");//Solo para ayuda a crear jsons
                //cargar el grid
                //formatear el grid
                log.Info("Accede funcion: CargaPreparaciones");

                return true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Error cargando preparaciones.ERROR:" + ex.Message);
            }
            return false;
        }

        private bool CargaPoolPedCli()
        {
            try
            {
                dtPool = Business.CargaPedidoCliCab(-1);//con -1 cargará una tabla vacia.
                if (dtPool is null)
                    return false;
                this.rGridOrdenes.DataSource = dtPool;
                //cargar el grid
                //formatear el grid
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void serializaDatatable(DataTable dt, string auxFile)
        {//se usa en desarrollo para crear rápidamente jsons de prueba
            try
            {
                //Serializar el objeto
                string json = JsonConvert.SerializeObject(dt);
                json = json.Replace("},{", "},\n{");//Añado saltos de lineas entre elementos para una mejor legibilidad si se usase notepad
                                                    //Guardar el fichero actual. Ojo si petase entre el paso anterior y este, habriamos cambiado ell nombre al fichero y no habria sustituto
                using (StreamWriter str = new StreamWriter(auxFile))
                {
                    str.Write(json);
                    str.Close();
                }
            }
            catch (DuplicateNameException)
            {
                MessageBox.Show("Ya existe ese nombre de fichero" + ":" + auxFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("se ha producido un error actualizando el fichero:" + "\n" + ex.Message + "\n" + auxFile);
                throw;
            }
        }

        private void SeleccionaOrden(string tipo, int idOrden, int seleccionado)
        {
            DataRow[] drs = null;
            //seleccino dtOrdenes del tipo y orden
            //actualizo seleccionado = true
            try
            {
                switch (tipo.ToUpper())
                {
                    case "PEDIDOS_CLIENTE":
                        drs = dtOrdenes.Select("Clase='" + tipo + "'  and idOrden=" + idOrden);
                        break;

                    case "":
                        if (idOrden > 0)
                            drs = dtOrdenes.Select("idOrden=" + idOrden);
                        else
                            drs = dtOrdenes.Select();
                        break;

                    case "PEDIDOS PRO.":
                        return;
                }
                for (int i = 0; i < drs.Length; i++)
                {
                    drs[i]["seleccionado"] = seleccionado;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error seleccionando la orden. ERROR:" + ex.Message);
            }
        }

        private void rBtnAnularSeleccion_Click(object sender, EventArgs e)
        {
            SeleccionaOrden("", -1, 0);
        }

        private void rGridOrdenes_CellClick(object sender, GridViewCellEventArgs e)
        {
            //al hacer click sobre una fila de ordenes, enciende  los articulos relacionados con esa orden.
            //para que sea facil usaremos un campo que llamo seleccionado
            int idOrden = 0;
            try
            {
                //Solo se hace anula la seleccion cuando sea la fila padre
                if (e.Row.IsSelected && e.Row.HierarchyLevel.Equals(0))
                {
                    AnularSeleccion();
                    rGridOrdenes.CurrentRow.Cells["seleccionado"].Value = 1;
                    idOrden = Convert.ToInt32(rGridOrdenes.CurrentRow.Cells["idOrden"].Value);
                    DataRow[] drOrdArticulos = dtOrdenesArticulos.Select("idOrden = " + idOrden);
                    foreach (DataRow drOA in drOrdArticulos)
                    {
                        DataRow[] drArticulos = dtArticulos.Select("idArticulo=" + drOA["idArticulo"]);
                        foreach (DataRow drArt in drArticulos)
                        {
                            drArt["seleccionado"] = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Se ha producido un error. ERROR:" + ex.Message);
            }
        }

        private void rGridOrdenes_CellBeginEdit(object sender, Telerik.WinControls.UI.GridViewCellCancelEventArgs e)
        {
            GridSpinEditor editor = this.rGridOrdenes.ActiveEditor as GridSpinEditor;
            if (e.Column.Name == "lineas_OK")
            {
                e.Cancel = true;
            }
        }

        private void AnularSeleccion()
        {
            try
            {
                SeleccionaOrden("", -1, 0);
                foreach (DataRow drArt in dtArticulos.Rows)
                {
                    drArt["seleccionado"] = 0;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void rBtnAnulaSelArts_Click(object sender, EventArgs e)
        {
            foreach (DataRow drArt in dtArticulos.Rows)
            {
                drArt["seleccionado"] = 0;
            }
        }

        private void rBtnAddFullPool_Click(object sender, EventArgs e)
        {
            bool ok;
            string clase;
            try
            {
                DataRow[] drPendientePool = dtPool.Select("seleccionado=0");
                foreach (DataRow drp in drPendientePool)
                {
                    clase = Convert.ToString(drp["CLASE"]);
                    switch (clase.ToUpper())
                    {
                        case "PEDIDOS_CLIENTE":
                            int idPedidoCli = Convert.ToInt32(drp["idOrden"]);
                            ok = AddPedidoCli(idPedidoCli);
                            drp["seleccionado"] = 1;
                            break;

                        case "PEDIDOS PRO.":
                            break;
                    }
                }
                rGridOrdenes.BestFitColumns();
                rGridArticulos.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Error añadiendo pedidos Pool. ERROR: " + ex.Message);
            }
        }

        private void QuitarElementoPoolOrdenes(int idOrden, string clase)
        {
            QuitarPedidoCli(clase, idOrden);

            //Lo vuelvo a mostrar en el PoolOrdenes
            DataRow row = dtPool.Select("idOrden=" + idOrden).FirstOrDefault();
            if (row != null)
            {
                row["seleccionado"] = 0;
            }
        }

        private void menuQuitarOrden_Click(object sender, EventArgs e)
        {
            if (rGridOrdenes.CurrentRow is null)
                MessageBox.Show(Lenguaje.traduce("Debe selecionar una fila"));
            else
            {
                int idOrden = Convert.ToInt32(rGridOrdenes.CurrentRow.Cells["idOrden"].Value);
                string clase = Convert.ToString(rGridOrdenes.CurrentRow.Cells["clase"].Value);

                QuitarElementoPoolOrdenes(idOrden, clase);
            }
        }

        private void rGridOrdenes_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuItem customMenuItem = new RadMenuItem();
            customMenuItem.Text = Lenguaje.traduce("Quitar orden");
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Add(separator);
            e.ContextMenu.Items.Add(customMenuItem);
            customMenuItem.Click += new EventHandler(menuQuitarOrden_Click);
        }

        #region reorden_ordenes /////////////////////////////////////////////////////

        private void MoveRow(bool moveUp)
        {
            int priorityActual, priorityDestino, indexDestino = -1, indexOrigen = -1;
            try
            {
                GridViewRowInfo currentRow = this.rGridOrdenes.CurrentRow;
                if (currentRow == null)
                    return;
                priorityActual = Convert.ToInt32(rGridOrdenes.CurrentRow.Cells["priority"].Value);
                if (moveUp)
                    priorityDestino = priorityActual - 1;
                else
                    priorityDestino = priorityActual + 1;
                if (priorityDestino <= 0 || priorityDestino > this.rGridOrdenes.RowCount)
                {
                    MessageBox.Show("Esta saliendo de los limites");
                    return;
                }
                //DataRow[] drDests = dtOrdenes.Select("priority=" + priorityDestino);
                //Necesito el index de la linea destino que en el datatable prodia no ser la fila consecutiva
                //despues de varias reordenaciones. No he encontrado un metodo menos cutre que recorrer el datatable.
                bool encontradoOrigen = false;
                bool encontradoDestino = false;
                for (int i = 0; (i < dtOrdenes.Rows.Count) && !(encontradoOrigen && encontradoDestino); i++)
                {
                    if (Convert.ToInt32(dtOrdenes.Rows[i]["priority"]) == priorityDestino)
                    {
                        indexDestino = i;
                        encontradoDestino = true;
                    }
                    if (Convert.ToInt32(dtOrdenes.Rows[i]["priority"]) == priorityActual)
                    {
                        indexOrigen = i;
                        encontradoOrigen = true;
                    }
                }
                SwapLineasGridOrdenes(currentRow, indexOrigen, indexDestino, moveUp);
                SwapPrioridadDtOA(priorityActual, priorityDestino);
                //actualizo la fila actual a la prioridad destino
                //rGridOrdenes.CurrentRow.Cells["priority"].Value = priorityDestino;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error moviendo pedido. ERROR: " + ex.Message);
            }
        }

        private void SwapPrioridadDtOA(int priorityActual, int priorityDestino)
        {
            //Buscamos las lineas de ordenes-articulos que coinciden en ambas ordenes. Empezamos recorriendo el origen.
            DataRow[] drOAOrigs = dtOrdenesArticulos.Select("priority=" + priorityActual + " or priority=" + priorityDestino);
            for (int i = 0; i < drOAOrigs.Length; i++)
            {
                if (Convert.ToInt32(drOAOrigs[i]["priority"]) == priorityActual)
                    drOAOrigs[i]["priority"] = priorityDestino;
                else
                    drOAOrigs[i]["priority"] = priorityActual;
            }
        }

        private int SwapLineasGridOrdenes(GridViewRowInfo origen, int indexOrigen, int indexDestino, bool moveUp)////////////////////////////////
        {   /* Manejamos tres datatables: el de ordenes que es donde sube o baja una orden
             * El de detalle de las lineas de las ordenes, que tengo que recorrer para cada una de sus lineas
             * Y el resumen de articulos
             * en el parametro origen viene una fila del datatable de ordenes,
             * en indexdestino el destino,  porque aunque lo veamos ordenado y parezca el siguiente o anterio podria no serlo.
             * Y si subimos o bajamos, que nos sirve para saber donde está en stock inicial del swap
             */
            string claseOrigen, claseDestino;
            int idOrdenOrigen, idOrdenDestino;
            int idArticuloOri;
            int stockDisponible, solicitado = -1, asignadoAntOri, asignadoAntDest, asignado = -1;
            int lineas_OK_ori = 0, lineas_OK_dest = 0;
            int priorityTmp;
            try
            {   //preparamos datos.
                claseOrigen = Convert.ToString(origen.Cells["CLASE"].Value);
                idOrdenOrigen = Convert.ToInt32(origen.Cells["idOrden"].Value);
                claseDestino = Convert.ToString(dtOrdenes.Rows[indexDestino]["CLASE"]);
                idOrdenDestino = Convert.ToInt32(dtOrdenes.Rows[indexDestino]["idOrden"]);

                //Buscamos las lineas de ordenes-articulos que coinciden en ambas ordenes. Empezamos recorriendo el origen.
                DataRow[] drOAOrigs = dtOrdenesArticulos.Select("clase='" + claseOrigen + "' and idorden=" + idOrdenOrigen);
                foreach (DataRow drOAorigen in drOAOrigs)
                {
                    idArticuloOri = Convert.ToInt32(drOAorigen["idarticulo"]);
                    //La condicion de posible recalculo es que en articulos haya mas demanda que disponibilidad
                    DataRow[] drArts = dtArticulos.Select("idarticulo=" + idArticuloOri);//tengo la certeza de que no hay duplicados
                    if (drArts.Length > 0)
                    {   ////tengo la certeza de que no hay duplicados por eso no recorro el vector y cojo el [0]
                        if (Convert.ToInt32(drArts[0]["disponible"]) < Convert.ToInt32(drArts[0]["solicitado"])) // si hay disponibilidad de sobra el swap es facil
                        {   //Habrá posible cambio de asignación si el artículo está en el origen y tambien en destino
                            DataRow[] drOADests = dtOrdenesArticulos.Select("clase='" + claseDestino + "' and idOrden=" + idOrdenDestino + " and idArticulo=" + idArticuloOri);
                            if (drOADests.Length > 0)
                            {
                                if (moveUp)
                                {
                                    stockDisponible = Convert.ToInt32(drOADests[0]["Disponible"]);
                                }
                                else
                                {
                                    stockDisponible = Convert.ToInt32(drOAorigen["Disponible"]);
                                }
                                asignadoAntDest = Convert.ToInt32(drOADests[0]["asignado"]);
                                asignadoAntOri = Convert.ToInt32(drOAorigen["asignado"]);
                                //ajusto cantidades destino
                                if (moveUp)
                                {
                                    drOAorigen["disponible"] = stockDisponible; //si muevo hacia arriba, el stock inicial se queda en el que ahora está abajo
                                    solicitado = Convert.ToInt32(drOAorigen["solicitado"]);
                                    asignado = Math.Min(stockDisponible, solicitado);
                                    stockDisponible -= asignado;
                                    drOAorigen["asignado"] = asignado;

                                    if (asignadoAntOri < solicitado && asignado >= solicitado)//, el que baja sol puede empeorar.
                                    {
                                        lineas_OK_ori = 1;
                                    }
                                    //ajusto destino
                                    drOADests[0]["disponible"] = stockDisponible;
                                    solicitado = Convert.ToInt32(drOADests[0]["solicitado"]);
                                    asignado = Math.Min(stockDisponible, solicitado);
                                    drOADests[0]["asignado"] = asignado;
                                    if (asignadoAntDest >= solicitado && asignado < solicitado)//el que baja sol puede empeorar.
                                    {
                                        lineas_OK_dest = -1;
                                    }
                                }
                                else
                                {
                                    //ajusto destino
                                    drOADests[0]["disponible"] = stockDisponible;
                                    solicitado = Convert.ToInt32(drOADests[0]["solicitado"]);
                                    asignado = Math.Min(stockDisponible, solicitado);
                                    stockDisponible -= asignado;
                                    drOADests[0]["asignado"] = asignado;
                                    if (asignadoAntDest < solicitado && asignado >= solicitado)//el que sube solo puede mejorar
                                    {
                                        lineas_OK_dest = 1;
                                    }
                                    //Ajusto origen
                                    drOAorigen["disponible"] = stockDisponible; //si muevo hacia arriba, el stock inicial se queda en el que ahora está abajo
                                    solicitado = Convert.ToInt32(drOAorigen["solicitado"]);
                                    asignado = Math.Min(stockDisponible, solicitado);
                                    stockDisponible -= asignado;
                                    drOAorigen["asignado"] = asignado;
                                }
                                //Ajusto lineas OK en articulos y en ordnes
                                drArts[0]["lineas_OK"] = Convert.ToInt32(drArts[0]["lineas_OK"]) + lineas_OK_ori + lineas_OK_dest;
                                dtOrdenes.Rows[indexDestino]["lineas_OK"] = Convert.ToInt32(dtOrdenes.Rows[indexDestino]["lineas_OK"]) + lineas_OK_dest;
                                dtOrdenes.Rows[origen.Index]["lineas_OK"] = Convert.ToInt32(dtOrdenes.Rows[origen.Index]["lineas_OK"]) + lineas_OK_ori;
                            }
                        }
                    }
                }
                //intercambio prioridades datatable ordenes
                priorityTmp = Convert.ToInt32(dtOrdenes.Rows[indexOrigen]["priority"]);//prioridad origen
                int prioritydestino = Convert.ToInt32(dtOrdenes.Rows[indexDestino]["priority"]);
                dtOrdenes.Rows[indexOrigen]["priority"] = prioritydestino;
                dtOrdenes.Rows[indexDestino]["priority"] = priorityTmp;
            }
            catch (Exception ex)
            {
                throw new Exception("Error realizando swap líneas. ERROR:" + ex.Message);
            }
            return 0;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            this.MoveRow(true);
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            this.MoveRow(false);
        }

        private void Rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                for (int i = 0; i < this.rGridOrdenes.Rows.Count; i++)
                {
                    this.rGridOrdenes.Rows[i].Cells["Priority"].Value = i + 1;
                }
            }
        }

        protected void WireEvents()
        {
            this.rBtnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            this.rBtnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
        }

        #endregion reorden_ordenes /////////////////////////////////////////////////////

        private void rBtnAjustarOA_Click(object sender, EventArgs e)
        {
            //este boton permite modificar las asignaciones DE CANTIDADES manualmente.
            Form childDetalle = new Form();
            childDetalle.Text = Lenguaje.traduce("detalle ordenes");
            RadGridView rGridDetalle = new RadGridView();
            childDetalle.Controls.Add(rGridDetalle);
            rGridDetalle.DataSource = dtOrdenesArticulos;
            rGridDetalle.EnableFiltering = true;
            rGridDetalle.ShowFilteringRow = false;
            rGridDetalle.ShowHeaderCellButtons = true;
            rGridDetalle.AllowAddNewRow = false;
            rGridDetalle.AllowEditRow = false;
            rGridDetalle.AllowDeleteRow = true;//Capturamos el borrado y lo hacemos custom.
            rGridDetalle.Dock = DockStyle.Fill;
            rGridDetalle.EnableAlternatingRowColor = true;
            rGridDetalle.BestFitColumns();
            childDetalle.Show();
        }

        private void rMnuOrdenExcel_Click(object sender, EventArgs e)
        {
            string exportFile = "";
            try
            {
                string path = Persistencia.DirectorioBase;
                exportFile = path + @"\exportedOrders.xlsx"; //TODO poder seleccionar
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    Telerik.WinControls.Export.GridViewSpreadExport exporter = new Telerik.WinControls.Export.GridViewSpreadExport(this.rGridOrdenes);
                    Telerik.WinControls.Export.SpreadExportRenderer renderer = new Telerik.WinControls.Export.SpreadExportRenderer();
                    exporter.RunExport(ms, renderer);

                    using (System.IO.FileStream fileStream = new System.IO.FileStream(exportFile, FileMode.Create, FileAccess.Write))
                    {
                        ms.WriteTo(fileStream);
                    }
                }
                MessageBox.Show("se ha guardado el excel en " + exportFile);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, Lenguaje.traduce("Error generando fichero excel ") + "\n" + exportFile + "\n" + ex.Message);
            }
        }

        private void rMnuOAExcel_Click(object sender, EventArgs e)
        {
            string exportFile = "";
            try
            {
                string path = Persistencia.DirectorioBase;
                exportFile = path + @"\exportedOrderLin.xlsx"; //TODO poder seleccionar
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    Telerik.WinControls.Export.GridViewSpreadExport exporter = new Telerik.WinControls.Export.GridViewSpreadExport(this.rGridArticulos);
                    Telerik.WinControls.Export.SpreadExportRenderer renderer = new Telerik.WinControls.Export.SpreadExportRenderer();
                    exporter.RunExport(ms, renderer);

                    using (System.IO.FileStream fileStream = new System.IO.FileStream(exportFile, FileMode.Create, FileAccess.Write))
                    {
                        ms.WriteTo(fileStream);
                    }
                    MessageBox.Show("se ha guardado el excel en " + exportFile);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, Lenguaje.traduce("Error generando fichero excel ") + "\n" + exportFile + "\n" + ex.Message);
            }
        }

        private void rBtnTodoAExcel(object sender, EventArgs e)
        {
            string exportFile = "";
            try
            {
                exportarExcel();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, Lenguaje.traduce("Error generando fichero excel ") + "\n" + exportFile + "\n" + ex.Message);
            }
        }

        private void exportarExcel()
        {
            if (rGridOrdenes.RowCount != 0)
            {
                MessageBox.Show("Se va a exportar el Grid de pedidos");
                FuncionesGenerales.exportarAExcelGenerico(rGridOrdenes);
                if (rGridArticulos.RowCount != 0)
                {
                    DialogResult dr = RadMessageBox.Show(Lenguaje.traduce("Quieres exportar las lineas del pedido"), Lenguaje.traduce("ExportarExcel"),
                        MessageBoxButtons.YesNo, RadMessageIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        FuncionesGenerales.exportarAExcelGenerico(rGridArticulos);
                    }
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Los pedidos vinculados no tienen lineas"));
                }
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("El excel de pedidos esta vacio"));
                MessageBox.Show(Lenguaje.traduce("El excel de pedidos esta vacio"));
            }
        }

        #region Botones de accion######################################

        private String GenerarJsonPropuestaOla(DataTable dt)
        {
            //quitamos los duplicados que se produzcan por linea
            var distinctValues = dt.AsEnumerable()
                        .Select(row => new
                        {
                            idOrden = row.Field<int>("idOrden")
                        })
                        .Distinct();

            string json = JsonConvert.SerializeObject(distinctValues);
            json = json.Replace("idOrden", "idpedidocli");

            return json;
        }

        //Clase para deserializar json que viene al llamar a ws CrearPropuestaOla
        private class PedidosJson
        {
            public int idpedidocli { get; set; }
        }

        //Mensajes Ola propuesta
        private const string GENERANDO_PROPUESTA_OLA = "Generando Propuesta Ola";

        private const string SIN_PEDIDOS_SELECCIONADOS = "No hay pedidos seleccionados en el pool de pedidos, por lo que no se ejecutará ningún proceso";
        private const string PROPUESTA_OLA = "Propuesta Ola";

        private async void rBtnPropuestaOla_Click(object sender, EventArgs e)
        {
            log.Info("Accede funcion :rBtnPropuestaOla_Click");

            try
            {
                //Cambiamos estado botones
                rBtnPropuestaOla.Image = global::RumboSGA.Properties.Resources.esperando;
                rBtnPropuestaOla.Text = Lenguaje.traduce(GENERANDO_PROPUESTA_OLA);
                rBtnPropuestaOla.Enabled = false;

                //Recoger los idpedido del pool de rGridPoolOrdenes y convertir a formato json
                //requerido. Sólo hay que enviar los que se esten visualizando
                string jsonGridPoolOrdenes = "[]";
                if (dtPool.Rows.Count != 0)
                {
                    jsonGridPoolOrdenes = GenerarJsonPropuestaOla(
                        dtPool.AsEnumerable()
                                 .Where(r => r.Field<int>("seleccionado") == 0)
                                 .CopyToDataTable()
                        );
                }
                else
                {
                    //Si no hay datos en el pool, no hay que llamar al WS, ya que no hay nada que hacer
                    log.Info("No hay pedidos en dtPool");
                    MessageBox.Show(Lenguaje.traduce(SIN_PEDIDOS_SELECCIONADOS));
                    return;
                }
                log.Info("Los pedidos del dtPool son: " + jsonGridPoolOrdenes);

                //Recoger id del pool de rGridOrdenes
                string jsonGridOrdenes = "[]";
                if (dtOrdenes.Rows.Count != 0)
                {
                    jsonGridOrdenes = GenerarJsonPropuestaOla(
                        dtOrdenes.AsEnumerable()
                             .CopyToDataTable()
                             );
                }
                log.Info("Los pedidos del pool dtOrdenes son: " + jsonGridPoolOrdenes);

                //Llama a webservice para generar propuesta de Ola

                string respuesta = await crearPropuestaOlaAsync(jsonGridPoolOrdenes, jsonGridOrdenes);
                log.Info("Respuesta WS: " + respuesta);

                //Convierto a JSON los pedidos de los 2 pools
                List<PedidosJson> pedidosOlaPropuestos = JsonConvert.DeserializeObject<List<PedidosJson>>(respuesta);
                List<PedidosJson> pedidosEnPool = JsonConvert.DeserializeObject<List<PedidosJson>>(jsonGridOrdenes);

                //Recorro los devuelto y los añado si es necesario
                foreach (var pedidoPropuesto in pedidosOlaPropuestos)
                {
                    //Solo añadimos cuando no este en el pool de pedidos
                    //TODO: Si se vuelven a producir duplciados al añadir, probar revisando directamente el datatable.
                    //var result = dtOrdenes.AsEnumerable().Where(r => r.Field<int>("idOrden") == pedidoPropuesto.idpedidocli).ToList();
                    log.Info("Añadiendo pedido al poolOrdenes.idPedido:" + pedidoPropuesto.idpedidocli.ToString());

                    if (!pedidosEnPool.Any(ped => ped.idpedidocli == pedidoPropuesto.idpedidocli))
                    {
                        bool ok = AddPedidoCli(pedidoPropuesto.idpedidocli);
                        if (ok)
                        {
                            log.Info("Pedido añadido correctamente");
                            //Marca seleccionado a 1 para que no salgan en el grid
                            DataRow row = dtPool.Select("idOrden=" + pedidoPropuesto.idpedidocli).FirstOrDefault();
                            if (row != null)
                            {
                                row["seleccionado"] = 1;
                            }
                        }
                        else
                        {
                            log.Warn("Ha ocurrido un error añadiendo el pedido al pool.");
                        }
                    }
                    else
                    {
                        log.Info("El idPedido ya existe en el pool. No se añadirá");
                    }
                }

                //Ajusto columnas
                rGridOrdenes.BestFitColumns();
                rGridArticulos.BestFitColumns();

                log.Info("FIN funcion :rBtnPropuestaOla_Click");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                //Volvemos los botones en estado originar
                rBtnPropuestaOla.Text = Lenguaje.traduce(PROPUESTA_OLA);
                rBtnPropuestaOla.Image = global::RumboSGA.Properties.Resources.olaPropuesta;
                rBtnPropuestaOla.Enabled = true;
            }
        }

        private void rBtnOla_Click(object sender, EventArgs e)
        {
            string configOla = string.Empty;
            AuxEditor eOla = null;
            try
            {
                if (dtConfigOla is null)
                {
                    configOla = Persistencia.DirectorioBase + @"\Configs\organizadorAlgs.json";//TODO cargar de persistencia
                    eOla = new AuxEditor(configOla, AuxType.DataTable);
                    dtConfigOla = eOla.getDatatable();
                }
                if (dtConfigOla is null)
                {
                    MessageBox.Show("no se ha podido cargar la configuracion de las olas");
                    return;
                };
                //Cargo la select del primer pedido
                //creo el datatable del primer pedido que cumpla
                //Añado el primero que cumple
                //Creo el datatable del primero que cumple
                //Mientras queden y no limite añado.
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Botones de accion######################################

        #region contadores ############################################

        private enum tipoContador
        { unidades, peso, volumen, pedidos, lineas, articulos }

        private Dictionary<string, Contador> dContadores = new Dictionary<string, Contador>();
        private bool limitesSinSobrepasar = true;

        private void Incrementa(string t, int valor)
        {
            Contador cont;
            try
            {
                cont = dContadores[t.ToLower()];
                cont.incrementa(valor);
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("no se ha encontrado el tipo de contador" + "\n" + t);//TODO Traducir
            }
            catch (Exception e)
            {
                MessageBox.Show("se ha producido un error buscando e incrmentando contador" + "\n" + e.Message);
            }
        }

        private async void rBtnLanzar_Click(object sender, EventArgs e)
        {
            try
            {
                rBtnLanzar.Image = global::RumboSGA.Properties.Resources.esperando;
                rBtnLanzar.Text = "Generando Orden Preparacion";
                rBtnLanzar.Enabled = false;
                string respuesta = await crearOlaAsync("SA");
                List<int> ordenes = JsonConvert.DeserializeObject<List<int>>(respuesta);
                foreach (int idOrdenRecogida in ordenes)
                {
                    rBtnLanzar.Text = "Reservando orden " + idOrdenRecogida;
                    string respuestaReserva = await reservarOrdenPreparacionAsync(idOrdenRecogida);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                rBtnLanzar.Text = "Lanzar Sin Agrupar";
                rBtnLanzar.Image = global::RumboSGA.Properties.Resources.edit;
                rBtnLanzar.Enabled = true;
            }
        }

        private async Task<string> reservarOrdenPreparacionAsync(int idOrdenRecogida)
        {
            String respuesta = "";
            String json;
            WSPedidoCliMotorClient pedidoCliMotor = null;
            try
            {
                try
                {
                    pedidoCliMotor = new WSPedidoCliMotorClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse al servicio.Revise la configuracion");
                    ExceptionManager.GestionarError(ex);
                    return "";
                }

                return await Task.Run(() =>
                {
                    string jsonWS = string.Empty;
                    string result = "";
                    List<ReservarOrden> listaReservar = new List<ReservarOrden>();
                    ReservarOrden reserva = new ReservarOrden();
                    reserva.idordenrecogida = idOrdenRecogida;
                    reserva.recursoAutomatico = false;
                    reserva.idRecurso = 0;
                    reserva.idusuario = User.IdUsuario;
                    reserva.error = new List<String>();
                    listaReservar.Add(reserva);
                    if (listaReservar.Count > 0)
                    {
                        jsonWS = JsonConvert.SerializeObject(listaReservar);
                        return pedidoCliMotor.reservarOrdenRecogidaAsync(jsonWS);
                    }
                    return null;
                });
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, pedidoCliMotor.Endpoint);
                return "Error";
            }
            return "";
        }

        private async Task<string> crearOlaAsync(string tipo)
        {
            String respuesta = "";
            String json;
            WSOlasMotorClient olasMotor = null;
            try
            {
                try
                {
                    olasMotor = new WSOlasMotorClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse al servicio.Revise la configuracion");
                    ExceptionManager.GestionarError(ex);
                    return "";
                }

                return await Task.Run(() =>
                {
                    DataView view = new DataView(dtOrdenesArticulos);
                    DataTable dtJson = view.ToTable(false, new string[] { "idOrden", "idArticulo", "asignado", "priority" });
                    json = JsonConvert.SerializeObject(dtJson);
                    json = json.Replace("},{", "},\n{");
                    return olasMotor.crearOlaOrganizadorAsync(json, tipo, DatosThread.getInstancia().getArrayDatos(), User.IdUsuario, User.IdOperario);
                });
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, olasMotor.Endpoint);
                return "Error";
            }
            return "";
        }

        private async Task<string> crearPropuestaOlaAsync(string jsonPoolOrdenes, string jsonOrdenes)
        {
            log.Info("Accede funcion :crearPropuestaOlaAsync");
            String respuesta = "";
            String json;
            WSPedidoCliMotorClient pedidoCliMotor = null;

            try
            {
                try
                {
                    pedidoCliMotor = new WSPedidoCliMotorClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse al servicio.Revise la configuracion");
                    ExceptionManager.GestionarError(ex);
                    return "";
                }

                log.Info(
                    String.Format(
                        "Llamando a servicio: crearPropuestaOlaOrganizadorAsync. jsonPoolOrdenes:{0}, jsonORdenes{1}, idusuario{2}, idoperario{3}",
                        jsonPoolOrdenes, jsonOrdenes, User.IdUsuario.ToString(), User.IdOperario.ToString()
                        )
                    );

                /* return await Task.Run(() =>
                 {
                     return pedidoCliMotor.crearPropuestaOlaOrganizadorAsyn(jsonPoolOrdenes, jsonOrdenes, DatosThread.getInstancia().getArrayDatos(), User.IdUsuario, User.IdOperario);
                 });**/
                return "";

                log.Info("FIN funcion :crearPropuestaOlaAsync");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, pedidoCliMotor.Endpoint);
                return "Error";
            }
        }

        private async void rBntLanzarOlaManual_Click(object sender, EventArgs e)
        {
            try
            {
                rBntOlaManual.Image = global::RumboSGA.Properties.Resources.esperando;
                rBntOlaManual.Enabled = false;
                rBntOlaManual.Text = "Generando Ola Preparacion";
                string respuesta = await crearOlaAsync("OL");
                List<int> ordenes = JsonConvert.DeserializeObject<List<int>>(respuesta);
                foreach (int idOrdenRecogida in ordenes)
                {
                    rBntOlaManual.Text = "Reservando orden " + idOrdenRecogida;
                    string respuestaReserva = await reservarOrdenPreparacionAsync(idOrdenRecogida);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                rBntOlaManual.Text = "Ola Manual";
                rBntOlaManual.Image = global::RumboSGA.Properties.Resources.ola;
                rBntOlaManual.Enabled = true;
            }
        }

        private async void rBtnLanzarAgrupado_Click(object sender, EventArgs e)
        {
            try
            {
                rBtnLanzarAgrupado.Image = global::RumboSGA.Properties.Resources.esperando;
                rBtnLanzarAgrupado.Enabled = false;
                rBtnLanzarAgrupado.Text = "Generando Orden Agrupada";

                string respuesta = await crearOlaAsync("AG");

                List<int> ordenes = JsonConvert.DeserializeObject<List<int>>(respuesta);
                foreach (int idOrdenRecogida in ordenes)
                {
                    rBtnLanzarAgrupado.Text = "Reservando orden " + idOrdenRecogida;
                    string respuestaReserva = await reservarOrdenPreparacionAsync(idOrdenRecogida);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                rBtnLanzarAgrupado.Text = "Lanzar Sin Agrupar";
                rBtnLanzarAgrupado.Image = global::RumboSGA.Properties.Resources.Table;
                rBtnLanzarAgrupado.Enabled = true;
            }
        }

        private void rBtnOla_Click_1(object sender, EventArgs e)
        {
            try
            {
                WSOlasMotorClient olasMotor = new WSOlasMotorClient();
                olasMotor.crearOlaAutomatica(User.IdUsuario, User.IdOperario);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rBtnEditarFiltrosPred_Click(object sender, EventArgs e)
        {
            string configFiltros = string.Empty;
            AuxEditor editFiltros = null;
            try
            {
                configFiltros = Persistencia.DirectorioBase + @"\Configs\organizadorAlgs.json";
                editFiltros = new AuxEditor(configFiltros, AuxType.JSON);
                editFiltros.Mostrar();
                CargarXmlFiltros();
            }
            catch (Exception ex)
            {
            }
        }

        private void rBtnConfigLimites_Click(object sender, EventArgs e)
        {
            string configFiltros = string.Empty;
            AuxEditor editFiltros = null;
            try
            {
                configFiltros = Persistencia.DirectorioBase + @"\Configs\organizadorLims.json";
                editFiltros = new AuxEditor(configFiltros, AuxType.DataTable);
                editFiltros.Mostrar();
                CargarContadores();//No solo se pueden modificar desde fichero. tambien desde base de datos
            }
            catch (Exception ex)
            {
            }
        }

        private void Incrementa(string t, double valor)
        {
            Contador cont;
            try
            {
                cont = dContadores[t.ToLower()];
                cont.incrementa(valor);
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("no se ha encontrado el tipo de contador" + "\n" + t);//TODO Traducir
            }
            catch (Exception e)
            {
                MessageBox.Show("se ha producido un error buscando e incrmentando contador" + "\n" + e.Message);
            }
        }

        private string LIMPIAR_ORDENES = "Limpiar Órdenes";
        private string LIMPIANDO_ORDENES = "Limpiando Órdenes";

        private async void LimpiarOrdenes_Click(object sender, EventArgs e)
        {
            log.Info("Accede funcion :LimpiarOrdenes_Click");

            //Eliminamos todas ordenes del Grid de ordenes
            try
            {
                //Cambiamos estado botones
                rBtnLimpiarOrdenes.Image = global::RumboSGA.Properties.Resources.esperando;
                rBtnLimpiarOrdenes.Text = Lenguaje.traduce(LIMPIANDO_ORDENES);
                rBtnLimpiarOrdenes.Enabled = false;

                //Al ser una funcion asincrona, necesita un metodo await para que espere al resto. Simulo un delay de 0.5seg para que la interfaz se actualice correctamente
                await Task.Delay(500);

                //recuperar todos los idOrden y clase del grid de ordenes
                var result = dtOrdenes.AsEnumerable().
                    Select(row => new
                    {
                        idOrden = row.Field<int>("idOrden"),
                        clase = row.Field<string>("clase")
                    }).
                    Distinct().ToList();

                foreach (var res in result)
                {
                    log.Info("Se va a eliminar la idOrden: " + res.idOrden.ToString() + " - CLASE: " + res.clase);
                    int idOrden = res.idOrden;
                    string clase = res.clase;
                    QuitarElementoPoolOrdenes(idOrden, clase);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                //Volvemos los botones en estado originar
                rBtnLimpiarOrdenes.Text = Lenguaje.traduce(LIMPIAR_ORDENES);
                rBtnLimpiarOrdenes.Image = global::RumboSGA.Properties.Resources.limpiarOrdenes;
                rBtnLimpiarOrdenes.Enabled = true;

                rGridOrdenes.BestFitColumns();
                rGridArticulos.BestFitColumns();

                log.Info("Sale de la funcion: LimpiarOrdenes_Click");
            }
        }

        private class Contador
        {
            private string nombre;
            private string de;
            private double value;
            private int posDecimales;
            private RadProgressBar barra;
            private static int numContador = 0;

            public Contador(string nombreContador, int max, int decimales, RadProgressBar bar)
            {
                Max = max;
                posDecimales = decimales;
                value = 0;
                barra = bar;
                barra.Maximum = Convert.ToInt32(Max * Math.Pow(10, posDecimales));
                nombre = Lenguaje.traduce(nombreContador) + ": ";
                de = " " + Lenguaje.traduce("de") + " ";
                numContador++;
            }

            public void incrementa(int valor)
            {
                value += valor;
                if (value * Math.Pow(10, posDecimales) > barra.Maximum)
                {
                    barra.Value1 = barra.Maximum;
                    barra.ProgressBarElement.IndicatorElement1.BackColor = Color.Red;
                    barra.ForeColor = Color.White;
                }
                else
                {
                    barra.Value1 = Convert.ToInt32(value * Math.Pow(10, posDecimales));
                    barra.ProgressBarElement.IndicatorElement1.BackColor = Color.FromArgb(6, 176, 37);
                    barra.ForeColor = Color.Black;
                }

                AjustaContador();
            }

            public void incrementa(double valor)
            {
                value += valor;
                if (value * Math.Pow(10, posDecimales) > barra.Maximum)
                {
                    barra.Value1 = barra.Maximum;
                    barra.ProgressBarElement.IndicatorElement1.BackColor = Color.Red;
                    barra.ForeColor = Color.White;
                }
                else
                {
                    barra.Value1 = Convert.ToInt32(value * Math.Pow(10, posDecimales));
                    barra.ProgressBarElement.IndicatorElement1.BackColor = Color.FromArgb(6, 176, 37);
                    barra.ForeColor = Color.Black;
                }
                AjustaContador();
            }

            public void setActual(int valor)
            {
                value = valor;
                if (value * Math.Pow(10, posDecimales) > barra.Maximum)
                {
                    barra.Value1 = barra.Maximum;
                    barra.ProgressBarElement.IndicatorElement1.BackColor = Color.Red;
                    barra.ForeColor = Color.White;
                }
                else
                    barra.Value1 = Convert.ToInt32(value * Math.Pow(10, posDecimales));
                AjustaContador();
            }

            public int Max { get; set; }

            public string Texto()
            {
                return $"{nombre}{value}{de}{Max}";
            }

            public void AjustaContador()
            {
                barra.Text = Texto();
            }
        }

        private void CargarContadoresDB()
        {
            try
            {
                //MÁXIMO DE PESO PARA UNA OLA
                Peso = new Contador("MÁXIMO DE PESO PARA UNA OLA", Persistencia.getParametroInt("MAXIMO_PESO_OLA"), 0, rPBPeso);
                dContadores.Add("peso", Peso);

                //MÁXIMO DE VOLUMEN PARA UNA OLA
                Volumen = new Contador("MÁXIMO DE VOLUMEN PARA UNA OLA", Persistencia.getParametroInt("MAXIMO_VOLUMEN_OLA"), 0, rPBVolumen);
                dContadores.Add("volumen", Volumen);

                //MÁXIMO DE UNIDADES PARA UNA OLA
                Unidades = new Contador("MÁXIMO DE UNIDADES PARA UNA OLA", Persistencia.getParametroInt("MAXIMO_UNIDADES_OLA"), 0, rPBUnidades);
                dContadores.Add("unidades", Unidades);

                //MAXIMO_ARTICULOS_OLA
                Articulos = new Contador("MÁXIMO DE ARTICULOS PARA UNA OLA", Persistencia.getParametroInt("MAXIMO_ARTICULOS_OLA"), 0, rPBArticulos);
                dContadores.Add("articulos", Articulos);

                //MAXIMO_PEDIDOS_OLA
                Pedidos = new Contador("MÁXIMO DE PEDIDOS PARA UNA OLA", Persistencia.getParametroInt("MAXIMO_PEDIDOS_OLA"), 0, rPBPedidos);
                dContadores.Add("pedidos", Pedidos);

                //MAXIMO_LINEAS_OLA
                Lineas = new Contador("MÁXIMO DE LINEAS PARA UNA OLA", Persistencia.getParametroInt("MAXIMO_LINEAS_OLA"), 0, rPBLineas);
                dContadores.Add("lineas", Lineas);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private void CargarContadores()
        {
            //TODO: pensar como parametrizar correctamente. Esta todo muy ligado a strings de fichero

            try
            {
                if (Persistencia.getParametroBoolean("CARGAR_LIMITES_OLA_DESDE_BD"))
                {
                    CargarContadoresDB();
                    return;
                }

                DataTable dtContadores = null;
                string configContadores = string.Empty;
                AuxEditor editContadores = null;

                configContadores = Persistencia.DirectorioBase + @"\Configs\organizadorLims.json";//TODO
                editContadores = new AuxEditor(configContadores, AuxType.DataTable);
                dtContadores = editContadores.getDatatable();
                if (dtContadores is null)
                    return;
                foreach (DataRow dr in dtContadores.Rows)
                {
                    //TODO no deberiamos necesitar tener creados de antemano los contadores sino instanciarlos por programa segun necesidades
                    //dContadores.Add(Convert.ToString(dr["tipo"]).ToLower(), new Contador(Convert.ToString(dr["descripcion"]), Convert.ToInt32(dr["max"]), 0));
                    switch (Convert.ToString(dr["tipo"]).ToLower())
                    {
                        case "peso":
                            Peso = new Contador(Convert.ToString(dr["descripcion"]), Convert.ToInt32(dr["max"]), 0, rPBPeso);
                            dContadores.Add(Convert.ToString(dr["tipo"]).ToLower(), Peso);
                            break;

                        case "volumen":
                            Volumen = new Contador(Convert.ToString(dr["descripcion"]), Convert.ToInt32(dr["max"]), 0, rPBVolumen);
                            dContadores.Add(Convert.ToString(dr["tipo"]).ToLower(), Volumen);
                            break;

                        case "unidades":
                            Unidades = new Contador(Convert.ToString(dr["descripcion"]), Convert.ToInt32(dr["max"]), 0, rPBUnidades);
                            dContadores.Add(Convert.ToString(dr["tipo"]).ToLower(), Unidades);
                            break;

                        case "articulos":
                            Articulos = new Contador(Convert.ToString(dr["descripcion"]), Convert.ToInt32(dr["max"]), 0, rPBArticulos);
                            dContadores.Add(Convert.ToString(dr["tipo"]).ToLower(), Articulos);
                            break;

                        case "pedidos":
                            Pedidos = new Contador(Convert.ToString(dr["descripcion"]), Convert.ToInt32(dr["max"]), 0, rPBPedidos);
                            dContadores.Add(Convert.ToString(dr["tipo"]).ToLower(), Pedidos);
                            break;

                        case "lineas":
                            Lineas = new Contador(Convert.ToString(dr["descripcion"]), Convert.ToInt32(dr["max"]), 0, rPBLineas);
                            dContadores.Add(Convert.ToString(dr["tipo"]).ToLower(), Lineas);
                            break;

                        default:
                            MessageBox.Show("el contador tipo:" + "\n" + Convert.ToString(dr["tipo"]) + "\nNo está implementado");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        #endregion contadores ############################################

        private void summaryRowCambioEstilo(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.RowInfo.Group == null && e.CellElement is GridSummaryCellElement)
                e.CellElement.Font = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            else
                e.CellElement.ResetValue(LightVisualElement.FontProperty, ValueResetFlags.Local);
        }

        #region testing ######################################

        private void rbnañadirOla_Click(object sender, EventArgs e)
        {
            try
            {
                AddPedidoCli(1185);
                AddPedidoCli(1243);
                AddPedidoCli(1246);
                AddPedidoCli(1248);
                //Valido que los datos de prueba son correctos
                if (Convert.ToInt32(dtOrdenes.Rows[0]["lineas"]) != 1)
                    MessageBox.Show("error en los datos de prueba pedido 1");
                if (Convert.ToInt32(dtOrdenes.Rows[0]["lineas_OK"]) != 0)
                    MessageBox.Show("error en los datos de prueba pedido 1");
                if (Convert.ToInt32(dtOrdenes.Rows[1]["lineas"]) != 4)
                    MessageBox.Show("error en los datos de prueba pedido 2");
                if (Convert.ToInt32(dtOrdenes.Rows[1]["lineas_OK"]) != 3)
                    MessageBox.Show("error en los datos de prueba pedido 2");
                //prueba 1 selecciono el primero pedido y lo bajo al ultimo
                rGridOrdenes.Rows[0].IsCurrent = true;
                MoveRow(false);
                MoveRow(false);
                MoveRow(false);
                //Valido que es el cuarto
                if (Convert.ToInt32(dtOrdenes.Rows[0]["priority"]) != 4)
                    MessageBox.Show("Fallo test 10:bajar al cuarto lugar");
                //Valido que la linea primera ahora tiene stock
                if (Convert.ToInt32(dtOrdenes.Rows[1]["lineas_OK"]) != 4)
                    MessageBox.Show("Fallo test 20: La linea 1 (2º del grid) deberia tener stock");
                MessageBox.Show("fin test");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        #endregion testing ######################################

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using RumboSGA.Herramientas;
using Telerik.WinControls.Data;
using RumboSGAManager.Model.DataContext;
using System.Threading;
using RumboSGA.Herramientas.Stock;
using System.Runtime.InteropServices;
using RumboSGA.Presentation.Herramientas.Stock;
using System.IO;
using RumboSGA.Properties;
using static RumboSGA.MiRadGridLocalization;
using System.Data;
using System.Diagnostics;
using Telerik.WinControls;
using Telerik.WinControls.UI.Export;
using RumboSGA.SalidaMotor;
using RumboSGAManager.Model.Security;
using RumboSGAManager;
using RumboSGA.UbicarMotor;
using System.Globalization;
using Telerik.WinControls.Export;
using Microsoft.CSharp.RuntimeBinder;
using RumboSGA.EntradaMotor;
using System.Dynamic;
using Newtonsoft.Json;
using System.Collections;
using RumboSGA.Presentation.Herramientas.Ventanas;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.LotesMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.ReservaMotor;
using RumboSGA.ArticulosMotor;

using RumboSGA.LotesMotor;

namespace RumboSGA.Presentation.UserControls.Mantenimientos.Herramientas
{
    public partial class Stock : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        public int idFormulario;
        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        protected List<TableScheme> _lstEsquemaTabla = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaGridView = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaJerarquia = new List<TableScheme>();
        protected List<TableScheme> campo = new List<TableScheme>();
        private List<string> visibles = new List<string>();

        private RadWaitingBar radWait = new RadWaitingBar();
        private WaitingBarIndicatorElement waitingBarIndicatorElement2 = new WaitingBarIndicatorElement();
        private WaitingBarIndicatorElement waitingBarIndicatorElement1 = new WaitingBarIndicatorElement();

        public const int _K_PAGINACION = 30;

        private BackgroundWorker myBackgroundWorkerVirtualGrid;
        private BackgroundWorker myBackgroundWorkerGridView;
        private RadDataFilterDialog filterDialog = new RadDataFilterDialog();
        private DataTable tabla = new DataTable();
        private DataTable tablaJerarquia = new DataTable();
        private GridViewTemplate jerarquia = new GridViewTemplate();
        public List<string> columnNames = new List<string>();
        public List<string> columnasVisibles = new List<string>();

        private GridViewSummaryItem summaryItemTop = new GridViewSummaryItem();
        private GridViewSummaryItem summaryItemBottom = new GridViewSummaryItem();

        private RadRibbonBarGroup grupoLabel = new RadRibbonBarGroup();
        private RadLabelElement lblCantidad = new RadLabelElement();

        public string errorUb = string.Empty;
        private string nombreJson = "Stock";

        private int _cantidadRegistros = 0;

        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        private RadContextMenu contextMenu = new RadContextMenu();
        private RadContextMenu contextMenuVirtual = new RadContextMenu();

        private DataTable dataVirtualGrid = new DataTable();
        private DataTable dtFiltradoVirtual = new DataTable();

        protected dynamic _selectedRow;

        protected GridScheme _esquemGrid;

        private string filterExpressionGridView = string.Empty;
        private List<ColumnaModificable> colsModificables = XmlReaderPropio.getColumnasModStock();
        private RumMenuItem cambiarValorColumnaMenuItem = new RumMenuItem();
        private RumMenuItem cambiarLoteColumnaMenuItem = new RumMenuItem();

        private WSLotesClient lotesMotor;
        private string ficheroExportacionStock;

        private List<FilterDescriptor> filtrado = new List<FilterDescriptor>();
        private List<SortDescriptor> ordenado = new List<SortDescriptor>();

        #endregion Variables

        public Stock()
        {
            InitializeComponent();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            this.ControlBox = true;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Shown += form_Shown;
            this.Show();
            this.Name = "Stock";
            this.Text = "Stock";
            ConfigurarBotones();
            CantidadRegistros = Business.GetStockCantidad(ref _lstEsquemaTabla);
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + CantidadRegistros;
            pagHeader.Text = Lenguaje.traduce(strings.RegistrosPaginacion);
            itemColumnas.Click += itemColumnas_Click;
            btnCambiarVista.Click += CambiarVista;
            inicializarComboTamañofuente();
            ConfigurarVirtualGrid();
            ConfigurarGridView();
            InicializarBackgroundWorkerVirtual();
            InicializarBackgroundWorkerGridView();
            InicializarContextMenu();
            ConfigurarBarraProgresoGridView();
            CargarStrings();
            AñadirLabelCantidad();
            //INICIALMENTE CARGAMOS VIRTUAL
            btnExportar.Enabled = false;
            btnAgruparStock.Enabled = true;
            virtualGrid.MasterViewInfo.IsWaiting = true;
            Elegir_Grid();
            //InicializarComboPaginacion();
            try
            {
                lotesMotor = new WSLotesClient();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, lotesMotor.Endpoint);
            }
        }

        #region VirtualGrid

        private void ConfigurarVirtualGrid()
        {
            virtualGrid.AllowAddNewRow = false;
            //virtualGrid.EnablePaging = false;
            virtualGrid.AllowMultiColumnSorting = true;
            virtualGrid.AllowSorting = true;
            virtualGrid.AllowFiltering = true;
            virtualGrid.SelectionMode = Telerik.WinControls.UI.VirtualGridSelectionMode.CellSelect;
            //virtualGrid.PageSize = 30;
            virtualGrid.CellValueNeeded += radVirtualGrid1_CellValueNeeded;
            virtualGrid.SortChanged += VirtualGrid_SortChanged;
            //PRUEBA FILTROS ON ENTER 25/11/2019
            virtualGrid.FilterChanged += VirtualGrid_FilterChanged;
            virtualGrid.AllowEdit = true;
            virtualGrid.EditorRequired += VirtualGrid_EditorRequired;
            virtualGrid.CellValuePushed += VirtualGrid_CellValuePushed;
            virtualGrid.ContextMenuOpening += virtualGrid_ContextMenuOpeningEvent;
            virtualGrid.MultiSelect = true;
            virtualGrid.CellEditorInitialized += RadVirtualGrid1_CellEditorInitialized;
        }

        private void InicializarBackgroundWorkerVirtual()
        {
            myBackgroundWorkerVirtualGrid = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            myBackgroundWorkerVirtualGrid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myBackgroundWorkerVirtualGrid_RunWorkerCompleted);
        }

        protected void RefreshData()
        {
            //Actualizamos el número de filas totales que tendrá el grid con la variable CantidadRegistros heredada y que puede haber sido modificada al crear, clonar y eliminar registros
            InicializarFilasColumnasGridVirtual();
            //Limpiamos datos
            //dataVirtualGrid.Clear();
            virtualGrid.RowCount = dataVirtualGrid.Rows.Count;
            this.virtualGrid.TableElement.SynchronizeRows();
            virtualGrid.MasterViewInfo.IsWaiting = false;
        }

        private void GenerarDatos(/*object sender, DoWorkEventArgs e*/)
        {
        }

        private void myBackgroundWorkerVirtualGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.virtualGrid.RowCount = CantidadRegistros;
            ElegirEstilo();
            VirtualGrid_SetDataType();
            btnCambiarVista.Enabled = true;

            virtualGrid.MasterViewInfo.IsWaiting = false;
            virtualGrid.Cursor = Cursors.Arrow;
            RefreshData();
        }

        private void radVirtualGrid1_CellValueNeeded(object sender, Telerik.WinControls.UI.VirtualGridCellValueNeededEventArgs e)
        {
            try
            {
                if (dtFiltradoVirtual == null) return;
                if (e.ColumnIndex < 0)
                    return;
                if (e.RowIndex == RadVirtualGrid.HeaderRowIndex)
                {
                    e.Value = dtFiltradoVirtual.Columns[e.ColumnIndex].ColumnName;
                }

                if (e.RowIndex < 0)
                {
                    //columnNames[e.ColumnIndex];
                    e.FieldName = dtFiltradoVirtual.Columns[e.ColumnIndex].ColumnName;
                }
                if (tabla == null || e.RowIndex > dtFiltradoVirtual.Rows.Count - 1) return;
                if (e.RowIndex >= 0 && e.RowIndex < tabla.Rows.Count)
                {
                    //e.Value = data[e.RowIndex][e.ColumnIndex];
                    e.Value = dtFiltradoVirtual.Rows[e.RowIndex][e.ColumnIndex];
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void VirtualGrid_SortChanged(object sender, EventArgs e)
        {
            filtradoVirtualGrid();
        }

        private void VirtualGrid_FilterChanged(object sender, VirtualGridEventArgs e)
        {
            filtradoVirtualGrid();
        }

        private void VirtualGrid_EditorRequired(object sender, Telerik.WinControls.UI.VirtualGridEditorRequiredEventArgs e)
        {
            if (!_lstEsquemaTabla[e.ColumnIndex].EsModificable && e.RowIndex >= 0)
            {
                e.Editor = null;
            }
        }

        private void filtradoVirtualGrid()
        {
            if (dataVirtualGrid == null) return;
            string ex = virtualGrid.FilterDescriptors.Expression;
            string sort = virtualGrid.SortDescriptors.Expression;

            dtFiltradoVirtual.Rows.Clear();
            //if (dtVirtualGrid.Select(ex, sort).Length > 0)
            DataRow[] dr = dataVirtualGrid.Select(ex, sort);
            if (dr.Length > 0)
                dtFiltradoVirtual = dr.CopyToDataTable();

            virtualGrid.RowCount = dr.Length;
            virtualGrid.TableElement.SynchronizeRows();

            /* Aquí no se alterna entre los gridViews
            if (dtFiltradoVirtual.Rows.Count < Persistencia.NumRegistrosVirtualGrid)
            {
                alternarGrid(GridOpciones.GridView);
                return;
            }
            */

            lblCantidad.Text = "     " + Lenguaje.traduce("Registros: ") + dr.Length;
        }

        private void InicializarFilasColumnasGridVirtual()
        {
            string filterExpression = virtualGrid.FilterDescriptors.Expression;
            foreach (var item in _lstEsquemaTabla)
            {
                if (item.EsVisible)
                {
                    visibles.Add(item.Etiqueta);
                }
            }
            virtualGrid.ColumnCount = _lstEsquemaTabla.Count;
        }

        private void VirtualGrid_CellValuePushed(object sender, Telerik.WinControls.UI.VirtualGridCellValuePushedEventArgs e)
        {
            Debug.WriteLine("Capturando evento cambio de valor en virtual");
        }

        private void VirtualGrid_SetDataType()
        {
            Type tipo = typeof(string);
            for (int i = 0; i < _lstEsquemaTabla.Count; i++)
            {
                switch (_lstEsquemaTabla[i].TipoColumna)
                {
                    case "int":
                        tipo = typeof(int);
                        break;

                    case "date":
                        tipo = typeof(DateTime);
                        break;

                    case "string":
                        tipo = typeof(string);
                        break;

                    default:
                        break;
                }
                virtualGrid.MasterViewInfo.SetColumnDataType(i, tipo);
            }
        }

        #endregion VirtualGrid

        #region GridView

        private void ConfigurarGridView()
        {
            gridView.AllowAddNewRow = false;
            gridView.AllowSearchRow = true;
            gridView.MasterView.TableSearchRow.SearchDelay = 1500;
            gridView.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.CellSelect;
            gridView.AllowMultiColumnSorting = true;
            gridView.MultiSelect = true;
            gridView.ViewCellFormatting += radGridView1_ViewCellFormatting;
            gridView.CellFormatting += radGridView1_CellFormatting;
            gridView.ChildViewExpanding += radGridView1_ChildViewExpanding;
            gridView.FilterChanged += RadGridView1_FilterChanged;
            gridView.CellEndEdit += RadGridView1_CellEndEdit;
            gridView.ReadOnly = true;
            gridView.ShowGroupPanel = true;
            gridView.ShowGroupedColumns = true;
            gridView.EnableFiltering = true;
            gridView.MasterTemplate.EnableFiltering = true;
            gridView.ContextMenuOpening += gridView_ContextMenuOpeningEvent;
            this.tableLayoutPanel1.SetColumnSpan(this.gridView, 10);
            gridView.MasterTemplate.Templates.Add(jerarquia);
            jerarquia.BestFitColumns();
            Configurar_Summary_Rows();
            gridView.EnablePaging = false;
        }

        private void InicializarBackgroundWorkerGridView()
        {
            myBackgroundWorkerGridView = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            myBackgroundWorkerGridView.DoWork += new DoWorkEventHandler(GenerarDatosGridView);
            myBackgroundWorkerGridView.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myBackgroundWorkerGridView_RunWorkerCompleted);
        }

        private void GenerarDatosGridView(object sender, DoWorkEventArgs e)
        {
            tabla = null;
            //Error cuando en el virtualGrid filtras y luego vuelves a gridview
            log.Debug("Empieza a descargar tabla stock");
            tabla = Business.GetStockDatosGridView(_lstEsquemaGridView, filterExpressionGridView);
            //_lstEsquemaTabla = FuncionesGenerales.CargarEsquema(nombreJson);
            //_lstEsquemaGridView = FuncionesGenerales.CargarEsquema(nombreJson);

            //César:He intentado poner el descargarJsonDatos para sustituir a la función que hay de getStockDatos pero la jerarquia no funciona correctamente
            //Y no encuentro la razón.
            //tabla = FuncionesGenerales.descargarJsonDatos(nombreJson, "");
            Business.GetReservasCantidad(ref _lstEsquemaJerarquia);
            tablaJerarquia = null;
            tablaJerarquia = Business.GetReservasDatos(_lstEsquemaJerarquia);
        }

        private void RelacionGridReservas()
        {
            try
            {
                GridViewRelation relacion = new GridViewRelation(gridView.MasterTemplate);
                relacion.ChildTemplate = jerarquia;
                relacion.ParentColumnNames.Add(Lenguaje.traduce("Matrícula"));
                relacion.ChildColumnNames.Add(Lenguaje.traduce("Matrícula"));
                gridView.Relations.Clear();
                gridView.Relations.Add(relacion);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void myBackgroundWorkerGridView_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                gridView.DataSource = null;
                DataTable tablaColumnas = new DataTable();
                tablaColumnas.Rows.Clear();

                gridView.DataSource = tablaColumnas;

                jerarquia.DataSource = null;

                if (tabla.Rows.Count != 0)
                {
                    gridView.DataSource = tabla;

                    dataVirtualGrid = tabla.Clone();
                    for (int i = 0; i < dataVirtualGrid.Columns.Count; i++)
                    {
                        dataVirtualGrid.Columns[i].DataType = typeof(String);
                    }
                    foreach (DataRow dtR in tabla.Rows)
                    {
                        dataVirtualGrid.ImportRow(dtR);
                    }

                    dtFiltradoVirtual = dataVirtualGrid.Copy();

                    ElegirEstilo();
                    AñadirColumnasNuevasGridView();

                    jerarquia.DataSource = tablaJerarquia;
                    RelacionGridReservas();

                    btnCambiarVista.Enabled = true;
                    btnExportar.Enabled = true;
                    try
                    {
                        gridView.Columns.Remove("RowNum");
                    }
                    catch (Exception ex)
                    {
                        //log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                        //si da error es porque no encuentra la columna,en cuyo caso nos da igual
                    }
                    ColumnasModificables(gridView);

                    radWait.StopWaiting();
                }
                else
                {
                    radWait.StopWaiting();
                    btnCambiarVista.Enabled = true;
                }
                gridView.Refresh();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void AñadirColumnasNuevasGridView()
        {
            for (int i = 0; i < tabla.Columns.Count; i++)
            {
                try
                {
                    /*
                    if (tabla.Columns[i].ColumnName != "RowNum")
                    {
                        string prueba = gridView.Columns[tabla.Columns[i].ColumnName].Name;
                    }*/
                    if (tabla.Columns[i].ColumnName != "RowNum" && gridView.Columns[tabla.Columns[i].ColumnName] == null)
                    {
                        string nombreCampo = string.Empty;
                        string textoHeader = string.Empty;
                        string name = string.Empty;
                        nombreCampo = _lstEsquemaTabla[i].Etiqueta;
                        textoHeader = _lstEsquemaTabla[i].Etiqueta;
                        name = tabla.Columns[i].ColumnName;
                        GridViewTextBoxColumn col = new GridViewTextBoxColumn();
                        col.FieldName = nombreCampo;
                        col.Name = name;
                        col.HeaderText = textoHeader;
                        gridView.Columns.Add(col);
                        gridView.Refresh();
                    }
                }
                catch (NullReferenceException ex)
                {
                    /* César 24 03 2021: No entiendo que es esto pero dentro de un catch me esta saltando siempre error.
                     * No es una buena practica poner código en el catch, voy a ponerlo en el try con un comprobante de Null y
                     * analizar si ocurre algo distinto
                    if (tabla.Columns[i].ColumnName != "RowNum")
                    {
                        string nombreCampo = string.Empty;
                        string textoHeader = string.Empty;
                        string name = string.Empty;
                        nombreCampo = _lstEsquemaTabla[i].Etiqueta;
                        textoHeader = _lstEsquemaTabla[i].Etiqueta;
                        name =tabla.Columns[i].ColumnName;
                        GridViewTextBoxColumn col = new GridViewTextBoxColumn();
                        col.FieldName = nombreCampo;
                        col.Name = name;
                        col.HeaderText = textoHeader;
                        gridView.Columns.Add(col);
                        gridView.Refresh();
                    }*/
                }
            }
        }

        private void GridControl_SelectionChanged(object sender, EventArgs e)
        {
            _selectedRow = null;

            if (this.gridView.CurrentCell != null && this.gridView.CurrentCell.RowIndex >= 0)
            {
                _selectedRow = FuncionesGenerales.getRowGridView(gridView);
            }
        }

        private void gridView_ValueChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Capturando evento cambio de valor en celda");
        }

        protected virtual void RadGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
                GridDataView dataView = gridView.MasterTemplate.DataView as GridDataView;
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + dataView.Indexer.Items.Count;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void Configurar_Summary_Rows()
        {
            summaryItemTop.Name = Lenguaje.traduce("Cantidad");
            summaryItemTop.Aggregate = GridAggregateFunction.Count;
            summaryItemTop.FormatString = Lenguaje.traduce("Num") + " = {0}";
            GridViewSummaryRowItem summaryRowItemTop = new GridViewSummaryRowItem();
            summaryRowItemTop.Add(summaryItemTop);
            this.gridView.Templates[0].SummaryRowsTop.Add(summaryRowItemTop);
            summaryItemBottom.Name = Lenguaje.traduce("Cantidad");
            summaryItemBottom.Aggregate = GridAggregateFunction.Sum;
            summaryItemBottom.FormatString = Lenguaje.traduce("Total") + " = {0}";
            GridViewSummaryRowItem summaryRowItemBottom = new GridViewSummaryRowItem();
            summaryRowItemBottom.Add(summaryItemBottom);
            this.gridView.Templates[0].SummaryRowsBottom.Add(summaryRowItemBottom);
        }

        //MODIFICAR COLUMNAS VIRTUAL (NO FUNCIONA)
        private void ColumnasModificables(RadGridView grid)
        {
            grid.MasterTemplate.AutoUpdateObjectRelationalSource = true;
            grid.ReadOnly = false;
            grid.AllowEditRow = true;
            foreach (GridViewDataColumn column in grid.Columns)
            {
                column.ReadOnly = true;
                if (colsModificables.Find(x => x.NombreColumnaEtiqueta.ToString().Equals(column.HeaderText, StringComparison.InvariantCultureIgnoreCase)) != null)
                {
                    column.ReadOnly = false;
                }
            }
        }

        private void Configurar_Columnas_GridView()
        {
            for (int i = 0; i < tabla.Columns.Count - 1; i++)
            {
                this.virtualGrid.MasterViewInfo.SetColumnDataType(i, tabla.Columns[i].DataType);
                if (_lstEsquemaTabla[i].EsModificable)
                {
                    gridView.Columns[i].ReadOnly = false;
                }
                else
                {
                    gridView.Columns[i].ReadOnly = true;
                }
            }
        }

        private void ConfigurarBarraProgresoGridView()
        {
            this.radWait.Name = "radWaitingBar1";
            this.radWait.Size = new System.Drawing.Size(200, 20);
            this.radWait.TabIndex = 2;
            this.radWait.Text = "radWaitingBar1";
            this.radWait.AssociatedControl = this.gridView;
        }

        #endregion GridView

        #region Botones

        private void ConfigurarBotones()
        {
            btnStockMin.Click += btnStockMinimo_Event;
            btnHistArticulo.Click += btnHistArticulos_Event;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Event;
            btnExportar.Click += btnExportar_Event;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Event;
            itemGuardarEstilo.Click += SaveItem_Click;
            itemCargarEstilo.Click += LoadItem_Click;
            FuncionesGenerales.RumDropDownAddManual(ref radDropDownButtonElement1, 20060);
            FuncionesGenerales.AddEliminarLayoutButton(ref radDropDownButtonElement1);
            if (this.radDropDownButtonElement1.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.radDropDownButtonElement1.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                {
                    if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)
                    {
                        FuncionesGenerales.EliminarLayout(this.Name + "VirtualGrid.xml", null);
                        virtualGrid.Refresh();
                    }
                    else
                    {
                        FuncionesGenerales.EliminarLayout(this.Name + "GridView.xml", null);
                        gridView.Refresh();
                    }
                });
            }

            stockXArticuloItem.Click += stockXArticuloItem_Click;
            stockXEstadoItem.Click += stockXEstadoItem_Click;
            stockXFamiliaItem.Click += stockXFamiliaItem_Click;
            stockXUbicacionItem.Click += stockXUbicacionItem_Click;
            reiniciarItem.Click += reiniciarItem_Click;

            btnAgruparStock.Text = strings.Agrupar;
            btnStockMin.Text = strings.StockMin;
            btnHistArticulo.Text = strings.HistArticulos;

            stockXArticuloItem.Text = strings.StockArticulo;
            stockXEstadoItem.Text = strings.StockEstado;
            stockXFamiliaItem.Text = strings.StockFamilia;
            reiniciarItem.Text = strings.Reiniciar;
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

        private void CambiarVista(object sender, EventArgs e)
        {
            var grid = this.tableLayoutPanel1.GetControlFromPosition(0, 1);
            if (grid is RadGridView)
            {
                this.virtualGrid.FilterDescriptors.Clear();
                this.virtualGrid.SortDescriptors.Clear();
                if (gridView.FilterDescriptors.Count > 0)
                {
                    filtrado.Clear();
                    foreach (FilterDescriptor item in gridView.FilterDescriptors)
                    {
                        filtrado.Add(item);
                    }

                    this.virtualGrid.FilterDescriptors.AddRange(filtrado);
                }
                if (gridView.SortDescriptors.Count > 0)
                {
                    ordenado.Clear();
                    foreach (SortDescriptor item in gridView.SortDescriptors)
                    {
                        ordenado.Add(item);
                    }
                    this.virtualGrid.SortDescriptors.AddRange(ordenado);
                }
                if (gridView.FilterDescriptors.Count == 0 && gridView.SortDescriptors.Count == 0)
                {
                    virtualGrid.RowCount = dataVirtualGrid.Rows.Count;
                    virtualGrid.ColumnCount = dataVirtualGrid.Columns.Count;
                    virtualGrid.TableElement.SynchronizeRows();
                }
                else
                {
                    virtualGrid.RowCount = dtFiltradoVirtual.Rows.Count;
                    virtualGrid.ColumnCount = dataVirtualGrid.Columns.Count;
                    virtualGrid.TableElement.SynchronizeRows();
                }

                btnCambiarVista.Text = strings.CambiarVistaVirtual;

                this.tableLayoutPanel1.Controls.Remove(grid);
                this.tableLayoutPanel1.Controls.Add(virtualGrid, 0, 1);
                btnExportar.Enabled = false;
                btnCambiarVista.Enabled = false;

                RefreshData();
                btnCambiarVista.Enabled = true;
            }
            else
            {
                if (grid is RadVirtualGrid)
                {
                    btnCambiarVista.Enabled = false;

                    if (tabla.Rows.Count > 90000)
                    {
                        MessageBox.Show(Lenguaje.traduce(strings.ExcesoRegistros));
                        btnCambiarVista.Enabled = true;
                        btnExportar.Enabled = false;
                        btnAgruparStock.Enabled = false;
                    }
                    else
                    {
                        btnCambiarVista.Text = strings.CambiarVistaGridView;
                        this.tableLayoutPanel1.Controls.Remove(grid);
                        this.tableLayoutPanel1.Controls.Add(gridView, 0, 1);
                        radWait.StartWaiting();

                        //No entiendo porque hay que descargar los datos cada vez que cambias de vista si son los mismos
                        //myBackgroundWorkerGridView.RunWorkerAsync();
                        radWait.StopWaiting();
                        btnExportar.Enabled = true;
                        btnAgruparStock.Enabled = true;
                    }

                    if (virtualGrid.FilterDescriptors.Count > 0)
                    {
                        filtrado.Clear();
                        foreach (FilterDescriptor item in virtualGrid.FilterDescriptors)
                        {
                            filtrado.Add(item);
                        }
                        this.gridView.FilterDescriptors.Clear();
                        this.gridView.FilterDescriptors.AddRange(filtrado);
                    }

                    if (virtualGrid.SortDescriptors.Count > 0)
                    {
                        ordenado.Clear();
                        foreach (SortDescriptor item in virtualGrid.SortDescriptors)
                        {
                            ordenado.Add(item);
                        }
                        this.gridView.SortDescriptors.Clear();
                        this.gridView.SortDescriptors.AddRange(ordenado);
                    }
                }
            }
            btnCambiarVista.Enabled = true;
        }

        private void btnStockMinimo_Event(object sender, EventArgs e)
        {
            string query = "SELECT a.idarticulo, a.referencia, a.descripcion, SUM(e.cantidad) AS Total_Existencias, a.stkminalmcen " +
                "FROM tblarticulos AS a LEFT OUTER JOIN tblexistencias AS e ON e.idarticulo=a.idarticulo " +
                "GROUP BY a.idarticulo, a.descripcion, a.referencia, a.stkminalmcen  HAVING SUM(e.cantidad)<a.stkminalmcen OR (SUM(e.cantidad) IS NULL AND a.stkminalmcen>0)";
            VentanaGrid ventana = new VentanaGrid(query, "Stock Minimo");
            ventana.GridViewBase.BestFitColumns();
            ventana.ShowDialog();
        }

        private void btnstockTotal_Event(object sender, EventArgs e)
        {
            string query = "SELECT * FROM VSTOCKTOTALESARTICULO";
            VentanaGrid ventana = new VentanaGrid(query, "Stock Total");
            ventana.ShowDialog();
        }

        private void btnSeleccion_Event(object sender, EventArgs e)
        {
            string[] text = new string[gridView.ColumnCount];
            for (int i = 0; i < gridView.ColumnCount; i++)
            {
                text[i] = gridView.CurrentRow.Cells[i].Value.ToString() + " ";
            }
        }

        private void btnLimpiarFiltros_Event(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
            {
                if (gridView.IsInEditMode)
                {
                    gridView.EndEdit();
                }
                gridView.FilterDescriptors.Clear();
            }
            else
            {
                if (virtualGrid.IsInEditMode)
                {
                    virtualGrid.EndEdit();
                }
                virtualGrid.FilterDescriptors.Clear();
            }
        }

        private void itemColumnas_Click(object sender, EventArgs e)
        {
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)
            {
                MessageBox.Show(Lenguaje.traduce(strings.MensajeCambiaVista));
            }
            else
            {
                gridView.ShowColumnChooser();
            }
        }

        private string confirmacion = Lenguaje.traduce(strings.ExportacionExito);

        private void btnExportar_Event(object sender, EventArgs e)
        {
            try
            {
                GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(this.gridView);
                spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
                //spreadExporter.ExportFormat = SpreadExportFormat.Xlsx;
                spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;
                spreadExporter.PagingExportOption = PagingExportOption.AllPages;
                spreadExporter.AsyncExportProgressChanged += spreadExporter_AsyncExportProgressChanged;
                spreadExporter.AsyncExportCompleted += spreadExporter_AsyncExportCompleted;

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "";
                dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    radProgressBar1.Visible = true;
                    ficheroExportacionStock = dialog.FileName;
                    spreadExporter.RunExportAsync(ficheroExportacionStock, new SpreadStreamExportRenderer());
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                radProgressBar1.Visible = false;
            }
        }

        private void spreadExporter_AsyncExportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.radProgressBar1.Value1 = e.ProgressPercentage;
        }

        private void spreadExporter_AsyncExportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                bool openExportFile = false;
                this.radProgressBar1.Value1 = 0;
                radProgressBar1.Visible = false;
                DialogResult dr = RadMessageBox.Show(confirmacion,
                       Lenguaje.traduce(strings.ExportarExcel), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    openExportFile = true;
                }
                if (openExportFile)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(ficheroExportacionStock);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format(Lenguaje.traduce(strings.ExportarError) + "\nError message: {0}", ex.Message);
                        RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                    finally
                    {
                        ficheroExportacionStock = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                radProgressBar1.Visible = false;
            }
        }

        public virtual void LoadItem_Click(object sender, EventArgs e)
        {
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(2);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                LoadLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                LoadLayoutLocal();
            }
        }

        public virtual void SaveItem_Click(object sender, EventArgs e)
        {
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                SaveLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                SaveLayoutLocal();
            }
        }

        #region Boton Agrupacion

        private void stockXArticuloItem_Click(object sender, EventArgs e)
        {
            this.gridView.GroupDescriptors.Clear();
            GroupDescriptor descriptor = new GroupDescriptor();
            /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {*/
            descriptor.GroupNames.Add(Lenguaje.traduce("Cod Articulo"), ListSortDirection.Ascending);
            descriptor.GroupNames.Add(Lenguaje.traduce("Desc Articulo"), ListSortDirection.Ascending);
            descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("UDS") + ")");
            descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("Disponible") + ")");
            descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("Reservado") + ")");
            descriptor.Aggregates.Add("Sum('" + Lenguaje.traduce("No OK") + "')");
            descriptor.Aggregates.Add("Count('" + Lenguaje.traduce("Cod Articulo") + "')");
            descriptor.Format = "{1}  | " + Lenguaje.traduce("Total Unidades") + ": {2} | " + Lenguaje.traduce("Total Disponible") + ": {3}| " + Lenguaje.traduce("Total Reservado") + ": {4}| " + Lenguaje.traduce("Total No OK") + ": {5}  | " + Lenguaje.traduce("Conteo") + ": {6}";

            /*}
            else
            {
                descriptor.GroupNames.Add("Item Code", ListSortDirection.Ascending);
                descriptor.GroupNames.Add("Item", ListSortDirection.Ascending);
                descriptor.Aggregates.Add("Sum(Units)");
                descriptor.Aggregates.Add("Count('Item Code')");
                descriptor.Format = "{1}  | Units Sum: {2} | Item Count: {3}";
            }*/
            this.gridView.GroupDescriptors.Add(descriptor);
        }

        private void stockXFamiliaItem_Click(object sender, EventArgs e)
        {
            this.gridView.GroupDescriptors.Clear();
            GroupDescriptor descriptor = new GroupDescriptor();
            /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {*/
            descriptor.GroupNames.Add(Lenguaje.traduce("Familia"), ListSortDirection.Ascending);
            descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("UDS") + ")");
            descriptor.Format = "{1}  | " + Lenguaje.traduce("Unidades") + ": {2}";
            /*}
            else
            {
                descriptor.GroupNames.Add("Family", ListSortDirection.Ascending);
                descriptor.Aggregates.Add("Sum(Units)");
                descriptor.Format = "{1}  | Units: {2}";
            }*/
            this.gridView.GroupDescriptors.Add(descriptor);
            this.gridView.ShowGroupPanel = false;
        }

        private void stockXEstadoItem_Click(object sender, EventArgs e)
        {
            this.gridView.GroupDescriptors.Clear();
            GroupDescriptor descriptor = new GroupDescriptor();
            GroupDescriptor descriptor2 = new GroupDescriptor();
            /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {*/
            descriptor.GroupNames.Add(Lenguaje.traduce("Cod Articulo"), ListSortDirection.Ascending);
            descriptor.GroupNames.Add(Lenguaje.traduce("Desc Articulo"), ListSortDirection.Ascending);
            descriptor.GroupNames.Add(Lenguaje.traduce("Estado Existencia"), ListSortDirection.Ascending);
            //tenemos que añadir un campo agregado aunque no lo mostremos o devuelve error de indice.
            descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("UDS") + ")");
            // descriptor2.Aggregates.Add("Sum(" + Lenguaje.traduce("UDS") + ")");
            descriptor.Aggregates.Add("Count('" + Lenguaje.traduce("Cod Articulo") + "')");

            descriptor.Format = "{1}" + "  |" + Lenguaje.traduce(" Total Unidades") + ": {2} | " + Lenguaje.traduce("Conteo") + ": {3}";

            /*}
            else
            {
                descriptor.GroupNames.Add("Item Code", ListSortDirection.Ascending);
                descriptor.GroupNames.Add("Item", ListSortDirection.Ascending);
                descriptor2.GroupNames.Add("Item Status", ListSortDirection.Ascending);
                //tenemos que añadir un campo agregado aunque no lo mostremos o devuelve error de indice.
                descriptor.Aggregates.Add("Sum(Units)");
                descriptor2.Aggregates.Add("Sum(Units)");
                descriptor2.Aggregates.Add("Count('Item Code')");
                descriptor2.Format = "Status: {1}  | Units Sum: {2} | Item Count: {3}";
            }*/

            //descriptor.Format = "{1}";
            this.gridView.GroupDescriptors.Add(descriptor);
            // this.gridView.GroupDescriptors.Add(descriptor2);
            //this.gridView.ShowGroupPanel = false;
        }

        private void stockXUbicacionItem_Click(object sender, EventArgs e)
        {
            this.gridView.GroupDescriptors.Clear();
            GroupDescriptor descriptor = new GroupDescriptor();
            /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {*/
            descriptor.GroupNames.Add(Lenguaje.traduce("Ubicacion"), ListSortDirection.Ascending);
            descriptor.Aggregates.Add("Sum(" + Lenguaje.traduce("UDS") + ")");
            descriptor.Aggregates.Add("Count('" + Lenguaje.traduce("Cod Articulo") + "')");
            descriptor.Format = "{1}  | " + Lenguaje.traduce(" Total Unidades") + ": {2} | " + Lenguaje.traduce("Conteo") + ": {3}";
            /*}
            else
            {
                descriptor.GroupNames.Add("Location", ListSortDirection.Ascending);
                descriptor.Aggregates.Add("Sum(Units)");
                descriptor.Aggregates.Add("Count('Item Code')");
                descriptor.Format = "{1}  | Units Sum: {2} | Item Count: {3}";
            }*/
            this.gridView.GroupDescriptors.Add(descriptor);
            this.gridView.ShowGroupPanel = false;
        }

        private void reiniciarItem_Click(object sender, EventArgs e)
        {
            this.gridView.GroupDescriptors.Clear();
            this.gridView.ShowGroupPanel = true;
        }

        #endregion Boton Agrupacion

        #endregion Botones

        #region Eventos

        private void InicializarEventos()
        {
            //Declaración Eventos
            this.gridView.SelectionChanged += GridControl_SelectionChanged;
        }

        private void CargarStrings()
        {
            btnAgruparStock.Text = Lenguaje.traduce(strings.Agrupar);
            btnHistArticulo.Text = Lenguaje.traduce(strings.HistArticulos);
            btnStockMin.Text = Lenguaje.traduce(strings.StockMin);

            grupoVer.Text = Lenguaje.traduce(strings.Ver);
            grupoTabla.Text = Lenguaje.traduce(strings.Tabla);
            grupoConf.Text = Lenguaje.traduce(strings.Configuracion);

            stockXEstadoItem.Text = Lenguaje.traduce(strings.StockEstado);
            stockXFamiliaItem.Text = Lenguaje.traduce(strings.StockFamilia);
            stockXUbicacionItem.Text = Lenguaje.traduce(strings.StockUbicacion);
            stockXArticuloItem.Text = Lenguaje.traduce(strings.StockArticulo);
            reiniciarItem.Text = Lenguaje.traduce(strings.Reiniciar);

            itemGuardarEstilo.Text = Lenguaje.traduce(strings.GuardarEstilo);
            itemCargarEstilo.Text = Lenguaje.traduce(strings.CargarEstilo);
            itemColumnas.Text = Lenguaje.traduce(strings.Columnas);
        }

        private void AñadirLabelCantidad()
        {
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + CantidadRegistros.ToString();
            lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            grupoLabel.Items.Add(lblCantidad);
            this.ribbonTab1.Items.AddRange(grupoLabel);
        }

        private void Elegir_Grid()
        {
            if (CantidadRegistros >= 90000)
            {
                virtualGrid.MasterViewInfo.IsWaiting = true;
                btnCambiarVista.Text = Lenguaje.traduce(strings.CambiarVistaVirtual);
                btnExportar.Enabled = false;
                btnAgruparStock.Enabled = false;
                btnCambiarVista.Enabled = false;
                //GenerarDatos();
                refrescar(null);
                btnCambiarVista.Enabled = true;
            }
            else
            {
                this.tableLayoutPanel1.Controls.Remove(virtualGrid);
                this.tableLayoutPanel1.Controls.Add(gridView, 0, 1);
                radWait.StartWaiting();
                btnCambiarVista.Text = Lenguaje.traduce(strings.CambiarVistaGridView);
                btnCambiarVista.Enabled = false;
                refrescar(null);
                btnAgruparStock.Enabled = true;
            }
        }

        private void inicializarComboTamañofuente()
        {
            for (int i = 8; i < 21; i++)
            {
                menuItemTamLetra.Items.Add(i.ToString());
            }
            menuItemTamLetra.ComboBoxElement.SelectedIndexChanged += comboTamañoFuente_Changed;
        }

        private void comboTamañoFuente_Changed(object sender, EventArgs e)
        {
            float tamaño = float.Parse(menuItemTamLetra.ComboBoxElement.SelectedItem.Text);
            virtualGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridView.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        #endregion Eventos

        #region Eventos Menu Contextual

        private void InicializarContextMenu()
        {
            RumMenuItem bomMenuItem = new RumMenuItem();
            bomMenuItem.Text = strings.VerFormularioBom;
            RumMenuItem histArticulosMenuItem = new RumMenuItem();
            histArticulosMenuItem.Text = strings.HistArticulos;
            RumMenuItem reubicarMenuItem = new RumMenuItem();
            reubicarMenuItem.Text = strings.Reubicar;
            RumMenuItem recalcularMenuItem = new RumMenuItem();
            recalcularMenuItem.Text = strings.RecalcularMov;
            RumMenuItem reimprimirMenuItem = new RumMenuItem();
            reimprimirMenuItem.Text = strings.Reimprimir;
            RumMenuItem regularizacionMenuItem = new RumMenuItem();
            regularizacionMenuItem.Text = strings.DarBaja;
            RumMenuItem reservasMenuItem = new RumMenuItem();
            reservasMenuItem.Text = strings.VerReservas;
            RumMenuItem atributosMenuItem = new RumMenuItem();
            atributosMenuItem.Text = strings.CambiarAtributos;
            Permiso(atributosMenuItem, 5000101);
            RumMenuItem cambiarEstadoPickingMenuItem = new RumMenuItem();
            cambiarEstadoPickingMenuItem.Text = strings.CambiarEstadoPicking;
            RumMenuItem cambiarEstadoExistenciaMenuItem = new RumMenuItem();
            cambiarEstadoExistenciaMenuItem.Text = strings.CambiarEstadoExistencia;
            RumMenuItem generarMovimientoAutomaticoColumnaMenuItem = new RumMenuItem();
            generarMovimientoAutomaticoColumnaMenuItem.Text = strings.RecalcularMov;
            cambiarValorColumnaMenuItem.Text = Lenguaje.traduce("Cambiar valor columna seleccionada");

            bomMenuItem.Click += new EventHandler(bomMenuItem_Event);
            histArticulosMenuItem.Click += new EventHandler(btnHistArticulos_Event);
            reubicarMenuItem.Click += new EventHandler(reubicarMenuItem_Event);
            reimprimirMenuItem.Click += new EventHandler(reimprimirMenuItem_Event);
            regularizacionMenuItem.Click += new EventHandler(regularizacionMenuItem_Event);
            //reservasMenuItem.Click += new EventHandler(reservasMenuItem_Event);
            atributosMenuItem.Click += new EventHandler(atributosMenuItem_Event);
            cambiarEstadoPickingMenuItem.Click += new EventHandler(cambiarEstadoPickingMenuItem_Event);
            cambiarEstadoExistenciaMenuItem.Click += new EventHandler(cambiarEstadoExistenciaMenuItem_Event);
            cambiarValorColumnaMenuItem.Click += new EventHandler(cambiarValorColumnaMenuItem_Event);
            cambiarLoteColumnaMenuItem.Click += new EventHandler(cambiarLoteColumnaMenuItem_Event);
            generarMovimientoAutomaticoColumnaMenuItem.Click += new EventHandler(generarMovimientoAutomaticoColumnaMenuItem_Event);
            contextMenu.Items.Add(bomMenuItem);
            contextMenu.Items.Add(histArticulosMenuItem);
            contextMenu.Items.Add(reubicarMenuItem);
            contextMenu.Items.Add(reimprimirMenuItem);
            contextMenu.Items.Add(regularizacionMenuItem);
            //contextMenu.Items.Add(reservasMenuItem);
            contextMenu.Items.Add(atributosMenuItem);
            contextMenu.Items.Add(cambiarEstadoPickingMenuItem);
            contextMenu.Items.Add(cambiarEstadoExistenciaMenuItem);
            contextMenu.Items.Add(cambiarValorColumnaMenuItem);
            contextMenu.Items.Add(cambiarLoteColumnaMenuItem);
            contextMenu.Items.Add(generarMovimientoAutomaticoColumnaMenuItem);
            cambiarValorColumnaMenuItem.Visibility = ElementVisibility.Collapsed;
            cambiarLoteColumnaMenuItem.Visibility = ElementVisibility.Collapsed;
            Permiso(cambiarEstadoExistenciaMenuItem, 500094);
            Permiso(generarMovimientoAutomaticoColumnaMenuItem, 500095);
            Permiso(cambiarLoteColumnaMenuItem, 500096);
            Permiso(rBtnCambiarEstadoExistencia, 500094);
        }

        private void generarMovimientoAutomaticoColumnaMenuItem_Event(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    List<int> campos = new List<int>();
                    if (gridView.SelectedCells.Count != 0)
                    {
                        foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                        {
                            campos.Add(Convert.ToInt32(item.Cells[Lenguaje.traduce("Matrícula")].Value));
                        }
                        WebServiceCalcularMovimiento(campos.ToArray());
                        refrescar(campos);
                    }
                }
                else
                {
                    if (virtualGrid.Selection.CurrentRowIndex > 0)
                    {
                        List<int> campos = new List<int>();
                        dynamic z = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncionesGenerales.getRowGridView(virtualGrid, _lstEsquemaTabla, dtFiltradoVirtual, dataVirtualGrid, tabla));
                        int pn = (int)z[this._lstEsquemaTabla[0].Etiqueta];
                        campos.Add(pn);

                        WebServiceCalcularMovimiento(campos.ToArray());
                        refrescar(campos);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void cambiarLoteColumnaMenuItem_Event(object sender, EventArgs e)
        {
            try
            {
                List<int> listEntrada = new List<int>();
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    List<string> campos = new List<string>();
                    if (gridView.SelectedCells.Count != 0)
                    {
                        foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                        {
                            campos.Add(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString());
                            listEntrada.Add(int.Parse(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString()));
                        }
                        string valor = PromptMessageBox.ShowDialog(Lenguaje.traduce("Introduzca valor"), Lenguaje.traduce("Introduzca valor"));
                        if (valor != null)
                        {
                            foreach (string campo in campos)
                            {
                                bool actualizado = lotesMotor.actualizaLote(Convert.ToInt32(campo), valor);
                            }
                        }
                    }
                }
                else
                {
                    if (virtualGrid.Selection.CurrentRowIndex > 0)
                    {
                        /* TODO Hacer para virtual grid List<string> campos = new List<string>();
                         var z = Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(this.data[virtualGrid.Selection.CurrentRowIndex]));
                         var pn = (string)z[this._lstEsquemaTabla[0].Etiqueta];
                         campos.Add(pn);
                         CambiarEstadoPicking cep = new CambiarEstadoPicking(campos);
                         cep.Text = Lenguaje.traduce(strings.TituloPicking);
                         cep.ShowDialog();
                         if (cep.exito)
                         {
                             RefreshData(virtualGrid.PageIndex);
                             myBackgroundWorkerGridView.RunWorkerAsync();
                         }
                         else
                         {
                             MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                         }*/
                    }
                }
                refrescar(listEntrada);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void gridView_ContextMenuOpeningEvent(object sender, Telerik.WinControls.UI.ContextMenuOpeningEventArgs e)

        {
            GridCellElement cellElement = e.ContextMenuProvider as GridCellElement;

            if (cellElement == null || cellElement.RowInfo is GridViewFilteringRowInfo || cellElement.RowInfo is GridViewTableHeaderRowInfo)
            {
                return;
            }
            if (cellElement.ColumnInfo == null || cellElement.ColumnInfo.HeaderText == null)
            {
                return;
            }
            ColumnaModificable colMod = colsModificables.Find(x => x.NombreColumnaEtiqueta.ToString().Equals(cellElement.ColumnInfo.HeaderText, StringComparison.InvariantCultureIgnoreCase));
            if (colMod != null)
            {
                cambiarValorColumnaMenuItem.Visibility = ElementVisibility.Visible;
                cambiarValorColumnaMenuItem.Text = Lenguaje.traduce("Cambiar valor columna ") + " " + cellElement.ColumnInfo.HeaderText;
            }
            else
            {
                cambiarValorColumnaMenuItem.Visibility = ElementVisibility.Collapsed;
            }
            if (cellElement.ColumnInfo.HeaderText.Equals(Lenguaje.traduce("Lote")))
            {
                cambiarLoteColumnaMenuItem.Visibility = ElementVisibility.Visible;
                cambiarLoteColumnaMenuItem.Text = Lenguaje.traduce("Cambiar valor columna ") + " " + cellElement.ColumnInfo.HeaderText;
            }
            else
            {
                cambiarLoteColumnaMenuItem.Visibility = ElementVisibility.Collapsed;
            }
            e.ContextMenu = contextMenu.DropDown;
        }

        private void virtualGrid_ContextMenuOpeningEvent(object sender, Telerik.WinControls.UI.VirtualGridContextMenuOpeningEventArgs e)
        {
            if (virtualGrid.Selection.CurrentRowIndex < 0 || virtualGrid.Selection.CurrentRowIndex == RadVirtualGrid.HeaderRowIndex)
            {
                return;
            }
            e.ContextMenu = contextMenu.DropDown;
        }

        private void bomMenuItem_Event(object sender, EventArgs e)
        {
            FormularioBOM bom = new FormularioBOM();
            bom.Show();
            bom.TopMost = true;
            bom.TopMost = false;
        }

        private void stockStatusMenuItem_Event(object sender, EventArgs e)
        {
            List<TableScheme> _lstEsquemaTablaTemporal = new List<TableScheme>();
            Business.GetStockStatusCantidad(ref _lstEsquemaTablaTemporal);
            DataTable tabla = Business.GetStockStatusDatosGridView(_lstEsquemaTablaTemporal, "");
            VentanaGrid v = new VentanaGrid(tabla, "Stock Estado");
            v.Show();
            v.TopMost = true;
            v.TopMost = false;
        }

        private void btnHistArticulos_Event(object sender, EventArgs e)
        {
            HistArticulosFecha formularioFecha = new HistArticulosFecha(); ;
            DateTime fecha;
            int articulo = 0;
            if (sender is RumMenuItem)
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    if (gridView.SelectedCells.Count != 0)
                    {
                        foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                        {
                            articulo = Convert.ToInt32(item.Cells[Lenguaje.traduce("Articulo")].Value);
                        }
                    }
                }
                else
                {
                }
            }
            if (articulo > 0)
            {
                formularioFecha.setArticulo(articulo);
            }

            if (formularioFecha.ShowDialog() == DialogResult.OK)
            {
                fecha = formularioFecha.fecha;
                articulo = formularioFecha.idArticulo;
                List<GridViewRowInfo> rows = new List<GridViewRowInfo>();
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string respJson = WebServiceHistorialArticulos(articulo, fecha);
                    GridHistArticulos gridHistArticulos = new GridHistArticulos(respJson);
                    gridHistArticulos.ShowDialog();
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Arrow;
                }
            }
        }

        private void reubicarMenuItem_Event(object sender, EventArgs e)
        {
            try
            {
                List<int> listEntradas = new List<int>();
                int idEntrada = 0;
                int idHuecoOriginal = 0;
                int cantidad = 0;
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    if (gridView.SelectedRows != null)
                    {
                        List<string> entradas = new List<string>();

                        foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                        {
                            entradas.Add(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString());
                        }

                        Reubicar reub = new Reubicar(entradas);
                        if (reub.ShowDialog() == DialogResult.OK)
                        {
                            int idHuecoNuevo = reub.idHueco;
                            bool conMovimiento = reub.conMovimiento;
                            int tipo;
                            if (conMovimiento)
                            {
                                tipo = 3;
                            }
                            else
                            {
                                tipo = 1;
                            }
                            foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                            {
                                try
                                {
                                    Int32.TryParse(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString(), out idEntrada);
                                    Int32.TryParse(item.Cells[Lenguaje.traduce("ID Hueco")].Value.ToString(), out idHuecoOriginal);
                                    listEntradas.Add(idEntrada);
                                    WebServiceReubicar(tipo, idEntrada, idHuecoNuevo);
                                }
                                catch (Exception ex)
                                {
                                    ExceptionManager.GestionarError(ex);
                                }
                            }
                            if (errorUb != string.Empty)
                            {
                                MessageBox.Show(errorUb);
                                errorUb = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce(strings.NingunaFilaSeleccionada));
                    }
                }
                else
                {
                    if (virtualGrid.Selection != null)
                    {
                        List<string> entradas = new List<string>();
                        foreach (var item in virtualGrid.Selection.SelectedRegions)
                        {
                            dynamic fragmentoJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncionesGenerales.getRowGridView(virtualGrid, _lstEsquemaTabla, dtFiltradoVirtual, dataVirtualGrid, tabla));

                            idEntrada = (int)fragmentoJSON[Lenguaje.traduce("Matrícula")];
                            idHuecoOriginal = (int)fragmentoJSON[Lenguaje.traduce("Id Hueco")];
                            try
                            {
                                cantidad = (int)fragmentoJSON[Lenguaje.traduce("UDS")];
                                entradas.Add(idEntrada.ToString());
                            }
                            catch (Exception ex)
                            {
                                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                                MessageBox.Show("Cantidad Nula");
                            }
                        }
                        Reubicar reub = new Reubicar(entradas);

                        if (reub.ShowDialog() == DialogResult.OK)
                        {
                            int idHuecoNuevo = reub.idHueco;
                            bool conMovimiento = reub.conMovimiento;
                            int tipo;
                            if (conMovimiento)
                            {
                                tipo = 3;
                            }
                            else
                            {
                                tipo = 1;
                            }
                            foreach (var item in entradas)
                            {
                                int entrada;
                                Int32.TryParse(item, out entrada);
                                listEntradas.Add(entrada);
                                WebServiceReubicar(tipo, entrada, idHuecoNuevo);
                            }
                            if (errorUb != string.Empty)
                            {
                                MessageBox.Show(errorUb);
                                errorUb = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce(strings.NingunaFilaSeleccionada));
                    }
                }
                refrescar(listEntradas);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void reimprimirMenuItem_Event(object sender, EventArgs e)
        {
            List<int> entradas = getEntradasSeleccionadas();
            int registrosSeleccionados = entradas.Count();
            if (registrosSeleccionados != 0)
            {
                DialogResult result = MessageBox.Show(Lenguaje.traduce("¿Estás seguro de reimprimir las " + registrosSeleccionados + " Matrículas seleccionadas?"), Lenguaje.traduce("Mensaje"), MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string json = formarJSONReimprimir(entradas);
                    WebServiceReimprimir(json, entradas);
                }
            }
        }

        private void regularizacionMenuItem_Event(object sender, EventArgs e)
        {
            SeleccionRegularizacion ventanaGrid = new SeleccionRegularizacion();
            List<int> listEntradas = new List<int>();
            if (ventanaGrid.ShowDialog() == DialogResult.OK)
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    string idMotivo = ventanaGrid.idMotivo;
                    string comentario = ventanaGrid.comentario;
                    List<GridViewRowInfo> rows = new List<GridViewRowInfo>();
                    if (gridView.SelectedCells.Count != 0)
                    {
                        foreach (var row in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                        {
                            try
                            {
                                int idEntrada = 0;
                                int cantidad = 0;
                                if (Int32.TryParse(row.Cells[Lenguaje.traduce("Matrícula")].Value.ToString(), out idEntrada) && Int32.TryParse(row.Cells[Lenguaje.traduce("UDS")].Value.ToString(), out cantidad))
                                {
                                    WebServiceRegularizaciones(idMotivo, idEntrada, cantidad, comentario);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.GestionarError(ex);
                            }
                        }
                    }
                    else
                    {
                        if (CultureInfo.CurrentUICulture.Name == "es-ES")
                        {
                            MessageBox.Show("Debes seleccionar al menos una fila");
                        }
                        else
                        {
                            MessageBox.Show("You need to select at least one row");
                        }
                    }
                    radWait.StartWaiting();
                    myBackgroundWorkerGridView.RunWorkerAsync();
                }
                else
                {
                    if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)
                    {
                        string idMotivo = ventanaGrid.idMotivo;
                        string comentario = ventanaGrid.comentario;
                        List<string> entradas = new List<string>();
                        foreach (var item in virtualGrid.Selection.SelectedRegions)
                        {
                            Debug.WriteLine(item.Top);
                            dynamic fragmentoJSON = FuncionesGenerales.getRowGridView(virtualGrid, _lstEsquemaTabla, dtFiltradoVirtual, dataVirtualGrid, tabla);
                            if (CultureInfo.CurrentUICulture.Name == "es-ES")
                            {
                                int idEntrada = fragmentoJSON["Matrícula"];
                                int cantidad = (int)fragmentoJSON["UDS"];
                                WebServiceRegularizaciones(idMotivo, idEntrada, cantidad, comentario);
                            }
                            else
                            {
                                int idEntrada = fragmentoJSON["Entry ID"];
                                int cantidad = (int)fragmentoJSON["Units"];
                                WebServiceRegularizaciones(idMotivo, idEntrada, cantidad, comentario);
                            }
                        }
                    }
                }
            }
            refrescar(listEntradas);
        }

        private void atributosMenuItem_Event(object sender, EventArgs e)
        {
            bool imprimir = false, reservar = false;
            int[] entradas;
            List<int> listEntradas = new List<int>();
            int cont = 0;
            CambiarAtributos parametros = new CambiarAtributos();
            if (parametros.ShowDialog() == DialogResult.OK)
            {
                Hashtable atributos = parametros.atributos;
                imprimir = parametros.imprimirEtiq;
                reservar = parametros.reservaRecurso;
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    if (gridView.SelectedCells.Count != 0)
                    {
                        entradas = new int[gridView.SelectedCells.Count];
                        try
                        {
                            foreach (var row in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                            {
                                int idEntrada = 0;

                                string idEntradaString = Lenguaje.traduce("Matrícula");
                                if (Int32.TryParse(row.Cells[idEntradaString].Value.ToString(), out idEntrada))
                                {
                                    listEntradas.Add(idEntrada);
                                    entradas[cont] = idEntrada;
                                    cont++;
                                }
                            }
                            string json = formarJSONAtributos(entradas, atributos, imprimir, reservar);
                            WebServiceCambiarAtributos(json);
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.GestionarError(ex);
                        }
                    }
                    else
                    {
                        Lenguaje.traduce("Debes seleccionar al menos una fila");
                    }
                }
                else
                {
                    if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)
                    {
                        cont = 0;
                        entradas = new int[virtualGrid.Selection.SelectedRegions.Count()];
                        foreach (var item in virtualGrid.Selection.SelectedRegions)
                        {
                            Debug.WriteLine(item.Top);
                            dynamic fragmentoJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncionesGenerales.getRowGridView(virtualGrid, _lstEsquemaTabla, dtFiltradoVirtual, dataVirtualGrid, tabla));

                            int idEntrada = fragmentoJSON[Lenguaje.traduce("Matrícula")];
                            entradas[cont] = idEntrada;
                            listEntradas.Add(idEntrada);
                            cont++;
                        }
                        string json = formarJSONAtributos(entradas, atributos, imprimir, reservar);
                        WebServiceCambiarAtributos(json);
                    }
                }
                refrescar(listEntradas);
            }
        }

        private void cambiarEstadoPickingMenuItem_Event(object sender, EventArgs e)
        {
            List<int> listEntradas = new List<int>();
            if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
            {
                List<string> campos = new List<string>();
                if (gridView.SelectedCells.Count != 0)
                {
                    foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                    {
                        campos.Add(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString());
                        listEntradas.Add(int.Parse(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString()));
                    }
                    CambiarPickingEstado cep = new CambiarPickingEstado(campos);
                    cep.Text = Lenguaje.traduce(strings.TituloPicking);
                    cep.ShowDialog();
                    if (cep.exito)
                    {
                        foreach (var item in campos)
                        {
                            string expresion = "[" + Lenguaje.traduce("Matrícula") + "]=" + item;
                            DataRow[] dr = tabla.Select(expresion);
                            Debug.WriteLine(dr.Single().ToString());
                            dr[0][Lenguaje.traduce("Estado Picking")] = cep.itemSeleccionado;
                        }
                    }
                }
            }
            else
            {
                if (virtualGrid.Selection.CurrentRowIndex > 0)
                {
                    List<string> campos = new List<string>();
                    dynamic z = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncionesGenerales.getRowGridView(virtualGrid, _lstEsquemaTabla, dtFiltradoVirtual, dataVirtualGrid, tabla));
                    var pn = (string)z[this._lstEsquemaTabla[0].Etiqueta];
                    campos.Add(pn);
                    CambiarPickingEstado cep = new CambiarPickingEstado(campos);
                    cep.Text = Lenguaje.traduce(strings.TituloPicking);
                    cep.ShowDialog();
                    if (cep.exito)
                    {
                        refrescar(cep.listEntradas);
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                    }
                }
            }
            refrescar(listEntradas);
        }

        private void cambiarEstadoExistenciaMenuItem_Event(object sender, EventArgs e)
        {
            List<int> listEntradas = new List<int>();
            if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
            {
                List<string[]> campos = new List<string[]>();
                if (gridView.SelectedCells.Count != 0)
                {
                    foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                    {
                        string[] exist = new string[2];
                        exist[0] = item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString();
                        listEntradas.Add(int.Parse(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString()));
                        exist[1] = item.Cells[Lenguaje.traduce("Estado Existencia")].Value.ToString();
                        campos.Add(exist);
                    }
                    CambiarEstadoExistencia cep = new CambiarEstadoExistencia(campos);
                    cep.ShowDialog();
                    if (cep.exito)
                    {
                        refrescar(listEntradas);
                    }
                }
            }
            else
            {
                if (virtualGrid.Selection.CurrentRowIndex > 0)
                {
                    /*List<string[]> campos = new List<string[]>();
                     var z = Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(this.data[virtualGrid.Selection.CurrentRowIndex]));
                     var pn = (string)z[this._lstEsquemaTabla[0].Etiqueta];
                     campos.Add(pn);
                     CambiarEstadoExistencia cep = new CambiarEstadoExistencia(campos);
                     cep.ShowDialog();
                     if (cep.exito)
                     {
                         RefreshData(virtualGrid.PageIndex);
                         myBackgroundWorkerGridView.RunWorkerAsync();
                     }
                     else
                     {
                         MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                     }*/
                }
            }
        }

        private void cambiarValorColumnaMenuItem_Event(object sender, EventArgs e)
        {
            List<int> listEntradas = new List<int>();
            try
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    List<string> campos = new List<string>();
                    string columna;
                    string tabla;
                    if (gridView.SelectedCells.Count != 0)
                    {
                        foreach (var item in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                        {
                            campos.Add(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString());
                            listEntradas.Add(int.Parse(item.Cells[Lenguaje.traduce("Matrícula")].Value.ToString()));
                            //colsModificables.Find(x => x.NombreColumnaEtiqueta.ToString() == item. HeaderText) != null)
                        }
                        if (gridView.SelectedCells.Count > 0)
                        {
                            columna = gridView.SelectedCells[0].ColumnInfo.HeaderText;

                            ColumnaModificable colMod = colsModificables.Find(x => x.NombreColumnaEtiqueta.ToString().Equals(columna, StringComparison.InvariantCultureIgnoreCase));
                            if (colMod != null)
                            {
                                string valor = PromptMessageBox.ShowDialog(Lenguaje.traduce("Introduzca valor"), Lenguaje.traduce("Introduzca valor"));
                                if (valor != null)
                                {
                                    foreach (string campo in campos)
                                    {
                                        Business.actualizarCampoStock(colMod.Tabla, colMod.NombreColumna, valor, Convert.ToInt32(campo));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (virtualGrid.Selection.CurrentRowIndex > 0)
                    {
                        /*TODO Hacer para virtual grid List<string> campos = new List<string>();
                         var z = Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(this.data[virtualGrid.Selection.CurrentRowIndex]));
                         var pn = (string)z[this._lstEsquemaTabla[0].Etiqueta];
                         campos.Add(pn);
                         CambiarEstadoPicking cep = new CambiarEstadoPicking(campos);
                         cep.Text = Lenguaje.traduce(strings.TituloPicking);
                         cep.ShowDialog();
                         if (cep.exito)
                         {
                             RefreshData(virtualGrid.PageIndex);
                             myBackgroundWorkerGridView.RunWorkerAsync();
                         }
                         else
                         {
                             MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                         }*/
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            refrescar(listEntradas);
        }

        #endregion Eventos Menu Contextual

        #region Control Barra Superior

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void radPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        #endregion Control Barra Superior

        #region Estilos

        public void SaveLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;/*XmlReaderPropio.getLayoutPath(0);*/
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)

                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "VirtualGrid.xml";

                    path.Replace(" ", "_");
                    this.virtualGrid.SaveLayout(path);
                }
                else if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "GridView.xml";

                    path.Replace(" ", "_");

                    gridView.SaveLayout(path);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void SaveLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal;/* XmlReaderPropio.getLayoutPath(1);*/
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)

                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "VirtualGrid.xml";

                    //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "Virtual.xml";

                    path.Replace(" ", "_");
                    this.virtualGrid.SaveLayout(path);
                }
                else if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.Name + "GridView.xml";
                    //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "GridView.xml";

                    path.Replace(" ", "_");

                    gridView.SaveLayout(path);
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void LoadLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal; /*XmlReaderPropio.getLayoutPath(0);*/
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)

                {
                    string s = path + "\\" + this.Name + "VirtualGrid.xml";
                    s.Replace(" ", "_");
                    this.virtualGrid.LoadLayout(s);
                }
                else if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    string s = path + "\\" + this.Name + "GridView.xml";
                    s.Replace(" ", "_");

                    gridView.LoadLayout(s);
                    //int i = 0;
                    //foreach (var item in gridView.Columns)
                    //{
                    //    if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    //    {
                    //        gridView.Columns[_lstEsquemaTablaProveedores[i].Etiqueta].FieldName = _lstEsquemaTablaProveedores[i].Etiqueta;
                    //    }
                    //    else
                    //    {
                    //        gridView.Columns[_lstEsquemaTablaProveedores[i].EtiqIngles].FieldName = _lstEsquemaTablaProveedores[i].EtiqIngles;
                    //    }
                    //    i++;
                    //}
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void LoadLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal;/*XmlReaderPropio.getLayoutPath(1);*/
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)

                {
                    string s = path + "\\" + this.Name + "VirtualGrid.xml";

                    s.Replace(" ", "_");
                    this.virtualGrid.LoadLayout(s);
                }
                else if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                {
                    string s = path + "\\" + this.Name + "GridView.xml";

                    s.Replace(" ", "_");

                    gridView.LoadLayout(s);
                    /*
                    int i = 0;
                    foreach (var item in _lstEsquemaTabla)
                    {
                        gridView.Columns[_lstEsquemaTabla[i].Etiqueta].FieldName = _lstEsquemaTabla[i].Etiqueta;
                        gridView.Columns[_lstEsquemaTabla[i].Etiqueta].Name = _lstEsquemaTabla[i].Nombre;
                        i++;
                    }*/
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void ElegirEstilo()
        {
            string pathLocal = Persistencia.DirectorioLocal; /*XmlReaderPropio.getLayoutPath(1);*/
            string pathGlobal = Persistencia.DirectorioGlobal; /*XmlReaderPropio.getLayoutPath(0);*/
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                pathLocal += "\\Español";
                pathGlobal += "\\Español";
            }
            else
            {
                pathLocal += "\\Ingles";
                pathGlobal += "\\Ingles";
            }

            if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)
            {
                string pathVirtual = pathLocal + "\\" + this.Name + "VirtualGrid.xml";
                bool existsVirtual = File.Exists(pathVirtual);
                if (existsVirtual)
                {
                    virtualGrid.LoadLayout(pathVirtual);
                }
                else
                {
                    pathVirtual = pathGlobal + "\\" + this.Name + "VirtualGrid.xml";
                    existsVirtual = File.Exists(pathVirtual);
                    if (existsVirtual)
                    {
                        virtualGrid.LoadLayout(pathVirtual);
                    }
                    else
                    {
                        virtualGrid.BestFitColumns();
                    }
                }
            }
            if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
            {
                string pathGridView = pathLocal + "\\" + this.Name + "GridView.xml";
                bool existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    gridView.LoadLayout(pathGridView);
                }
                else
                {
                    pathGridView = pathGlobal + "\\" + this.Name + "GridView.xml";
                    existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        gridView.LoadLayout(pathGridView);
                    }
                    else
                    {
                        gridView.BestFitColumns();
                    }
                }
            }
        }

        #endregion Estilos

        #region Ocultar Jerarquia si está vacia

        private bool IsExpandable(GridViewRowInfo rowInfo)
        {
            if (rowInfo.ChildRows != null && rowInfo.ChildRows.Count > 0)
            {
                return true;
            }

            return false;
        }

        private void radGridView1_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            GridGroupExpanderCellElement cell = e.CellElement as GridGroupExpanderCellElement;
            if (cell != null && e.CellElement.RowElement is GridDataRowElement)
            {
                if (!IsExpandable(cell.RowInfo))
                {
                    cell.Expander.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                }
                else
                {
                    cell.Expander.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                }
            }
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            try
            {
                List<int> listEntrada = new List<int>();
                if (e.RowIndex == -1)
                {
                    return;
                }
                string nuevoValor = e.Value.ToString();
                String columna = e.Column.Name;
                int idEntrada = Convert.ToInt32(e.Row.Cells[Lenguaje.traduce("Matrícula")].Value);
                listEntrada.Add(idEntrada);
                if (idEntrada > 0)
                {
                    ColumnaModificable colMod = colsModificables.Find(x => x.NombreColumnaEtiqueta.ToString().Equals(columna, StringComparison.InvariantCultureIgnoreCase));
                    if (colMod != null)
                    {
                        if (colMod.NombreColumnaEtiqueta.Equals(Lenguaje.traduce("Lote"), StringComparison.InvariantCultureIgnoreCase))
                        {
                            lotesMotor.actualizaLote(idEntrada, nuevoValor);
                        }
                        else if (colMod.NombreColumnaEtiqueta.Equals(Lenguaje.traduce("Estado Existencia"), StringComparison.InvariantCultureIgnoreCase))
                        {
                            actualizaEstadoExistencia(idEntrada, nuevoValor);
                        }
                        else
                        {
                            Business.actualizarCampoStock(colMod.Tabla, colMod.NombreColumna, nuevoValor, idEntrada);
                        }
                    }

                    refrescar(listEntrada);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void actualizaEstadoExistencia(int idEntrada, string nuevoValor)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string error = string.Empty;
                Object[] arrayLineas = new Object[1];
                List<string> errores = new List<string>();
                dynamic objDinamico = new ExpandoObject();
                objDinamico.identrada = idEntrada;
                objDinamico.idexistenciaestado = nuevoValor;
                objDinamico.Error = "";
                arrayLineas[0] = objDinamico;

                string json = JsonConvert.SerializeObject(arrayLineas);
                WSEntradaMotorClient wse = new WSEntradaMotorClient();
                String respuesta = wse.cambiarEstadoExistencias(json, User.IdOperario, 0);
                List<dynamic> errorText = JsonConvert.DeserializeObject<List<dynamic>>(respuesta);

                foreach (dynamic obj in errorText)
                {
                    if (obj.error == string.Empty)
                    { }
                    else
                    {
                        //MessageBox.Show(errorText[i].error);
                        errores.Add(Convert.ToString(obj.error));
                    }
                }
                if (errores.Count > 0)
                {
                    String errorTxt = "";

                    foreach (string errorStr in errores)
                    {
                        errorTxt = errorTxt + System.Environment.NewLine + errorStr;
                    }
                    throw new Exception(errorTxt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (colsModificables.Find(x => x.NombreColumnaEtiqueta.ToString().Equals(e.CellElement.ColumnInfo.Name, StringComparison.InvariantCultureIgnoreCase)) != null)
                {
                    //e.CellElement.ForeColor = Color.Blue;
                    Font fuente = e.CellElement.Font;
                    Font fuenteN = new Font(fuente.FontFamily, fuente.Size, FontStyle.Bold);
                    e.CellElement.Font = fuenteN;
                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void radGridView1_ChildViewExpanding(object sender, ChildViewExpandingEventArgs e)
        {
            e.Cancel = !IsExpandable(e.ParentRow);
        }

        #endregion Ocultar Jerarquia si está vacia

        #region WebServices

        public void WebServiceRegularizaciones(string idMotivo, int idEntrada, int cantidad, string comentario)
        {
            int idUsuario = User.IdUsuario;
            try
            {
                WSSalidaMotorClient salidaMotorClient = new WSSalidaMotorClient();
                Cursor.Current = Cursors.WaitCursor;
                log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'SalidaMotorClient.generarSalidaRegularizacionPaletCantidad' en Stock con parametros:IdUsuario(" + idUsuario + ")" +
                    ",IdRecurso(0),IdEntrada(" + idEntrada + "),IdMotivo(" + idMotivo + "),comentario(" + comentario + "),cantidad(" + cantidad + ")");
                salidaMotorClient.generarSalidaRegularizacionPaletCantidad(User.IdOperario, 0, idEntrada, idMotivo, comentario, cantidad);
                log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada");
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        public void WebServiceReubicar(int tipo, int idPalet, int idHueco)
        {
            int idUsuario = User.IdUsuario;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'WSUbicarMotorClient.modificarUbicacion' en Stock con parametros:IdPalet(" + idPalet + ")" +
                    ",IdHueco(" + idHueco + "),IdUsuario(" + idUsuario + ")");
                WSUbicarMotorClient ubicarMotor = new WSUbicarMotorClient();
                int respuesta = ubicarMotor.modificarUbicacion(tipo, idHueco, idPalet, User.IdOperario, 0);
                log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada con respuesta " + respuesta);
                if (respuesta != 0)
                {
                    errorUb += Lenguaje.traduce("No se puede reubicar la entrada ") + idPalet + ":" + Lenguaje.traduce(ErroresHueco.GetError(respuesta)) + "\n";
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                log.Error("Mensaje:" + e.Message + " \n StackTrace:" + e.StackTrace);
                Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        public void WebServiceCalcularMovimiento(int[] existencias)
        {
            int idUsuario = User.IdUsuario;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'WSUbicarMotorClient.eliminaMovimientoRTyRecalculaNuevo' en Stock con parametros:IdPalet(" + existencias.ToString() + ")" +
                 "),IdUsuario(" + idUsuario + ")");
                WSReservaMotorClient reservaMotor = new WSReservaMotorClient();
                string respuesta = reservaMotor.eliminaMovimientoRTyRecalculaNuevo(existencias, User.IdOperario);
                log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada con respuesta " + respuesta);
                if (!respuesta.Equals(""))
                {
                    throw new Exception(respuesta);
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
                log.Error("Mensaje:" + e.Message + " \n StackTrace:" + e.StackTrace);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        public void WebServiceReimprimir(string json, List<int> listEntradas)
        {
            WSEntradaMotorClient entradaMotor = new WSEntradaMotorClient();
            log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'WSEntradaMotorClient.reimprimirEtiquetasDesdeStock' en Stock con parametros:" + json);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string respuesta = entradaMotor.reimprimirEtiquetasDesdeStock(json);
                if (respuesta.Equals("OK"))
                {
                    log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada sin respuesta de error");
                }
                else
                {
                    log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada con error: " + respuesta);
                }
                refrescar(listEntradas);
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                log.Error("Mensaje:" + e.Message + " \n StackTrace:" + e.StackTrace);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        public void WebServiceCambiarAtributos(string json)
        {
            WSEntradaMotorClient entradaMotor = new WSEntradaMotorClient();
            log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'WSEntradaMotorClient.modificacionAtributos' en Stock con parametros:" + json);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string respuesta = entradaMotor.modificacionAtributos(json);
                if (respuesta.Equals(""))
                {
                    log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada sin respuesta de error");
                }
                else
                {
                    log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada con error: " + respuesta);
                }
                //El refrescar se hace desde la función superior
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                log.Error("Mensaje:" + e.Message + " \n StackTrace:" + e.StackTrace);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }

            //cambiarParam.OrderAtributos(entradas,atributos,usuario,contraseña,imprimirEtiq,reserva);
        }

        public string WebServiceHistorialArticulos(int id, DateTime fecha)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                WSArticulosClient wsArticulos = new WSArticulosClient();

                string fechaFormateada = fecha.ToString("dd/MM/yyyy");
                string j = formarJSONHistArticulos(id, fechaFormateada);
                log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada al WebService 'WSArticulosClient.getDatosHistoricoArticulo' en Stock con parametros" + j);

                string json = wsArticulos.getDatosHistoricoArticulo(j);
                log.Debug("Terminada llamada al WebService.Respuesta:" + json);
                return json;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
            return "";
        }

        #endregion WebServices

        private BindingSource bs = new BindingSource();

        private void RadVirtualGrid1_CellEditorInitialized(object sender, VirtualGridCellEditorInitializedEventArgs e)
        {
            if (e.RowIndex == RadVirtualGrid.FilterRowIndex)
            {
                VirtualGridTextBoxEditor editor = e.ActiveEditor as VirtualGridTextBoxEditor;
                RadTextBoxEditorElement el = editor.EditorElement as RadTextBoxEditorElement;
                el.KeyDown -= El_KeyDown;
                el.KeyDown += El_KeyDown;
            }
        }

        private void El_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                filtradoVirtualGrid();
            }
        }

        #region WebServices Jsons

        private string formarJSONAtributos(int[] idEntradas, Hashtable atributos, bool imprimir, bool reservar)
        {
            int usuario = User.IdOperario;
            dynamic objDinamico = new ExpandoObject();
            objDinamico.entradas = idEntradas;
            objDinamico.listaAtributos = atributos;
            objDinamico.usuario = usuario;
            objDinamico.nombreImpresora = ConexionSQL.getNombreImpresora();
            objDinamico.imprimir = imprimir;
            objDinamico.reservar = reservar;
            string json = JsonConvert.SerializeObject(objDinamico, Formatting.Indented);
            Debug.WriteLine(json);
            return json;
        }

        private string formarJSONHistArticulos(int idArticulo, string fecha)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.id = idArticulo;
            objDinamico.fecha = fecha;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        private string formarJSONReimprimir(List<int> idEntradas)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idsEntradas = idEntradas;
            objDinamico.nombreImpresora = User.NombreImpresora;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        #endregion WebServices Jsons

        //PRUEBA JERARQUIA VIRTUAL
        private List<dynamic> data2 = new List<dynamic>();

        private void VirtualGridHierarchy_Load(object sender, EventArgs e)
        {
            this.virtualGrid.QueryHasChildRows += radVirtualGrid1_QueryHasChildRows;
            this.virtualGrid.RowExpanding += radVirtualGrid1_RowExpanding;
            this.virtualGrid.TableElement.RowHeight = 120;
        }

        private void radVirtualGrid1_RowExpanding(object sender, VirtualGridRowExpandingEventArgs e)
        {
            e.ChildViewInfo.ColumnCount = data2.Count;
            e.ChildViewInfo.RowCount = data2[e.ChildViewInfo.ParentRowIndex].Count;
        }

        //FIN PRUEBA
        private List<int> getEntradasSeleccionadas()
        {
            List<int> entradas = new List<int>();
            if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
            {
                if (gridView.SelectedCells.Count != 0)
                {
                    foreach (var row in gridView.SelectedCells.Select(c => c.RowInfo).GroupBy(r => r.Index).Select(g => g.First()))
                    {
                        try
                        {
                            int idEntrada = 0;
                            int.TryParse(row.Cells[Lenguaje.traduce("Matrícula")].Value.ToString(), out idEntrada);
                            entradas.Add(idEntrada);
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.GestionarError(ex);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Debes seleccionar al menos una fila");
                }
            }
            else
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadVirtualGrid)
                {
                    foreach (var item in virtualGrid.Selection.SelectedRegions)
                    {
                        dynamic fragmentoJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncionesGenerales.getRowGridView(virtualGrid, _lstEsquemaTabla, dtFiltradoVirtual, dataVirtualGrid, tabla));

                        int idEntrada = fragmentoJSON[Lenguaje.traduce("Matrícula")];
                        entradas.Add(idEntrada);
                    }
                }
            }
            return entradas;
        }

        private void radVirtualGrid1_QueryHasChildRows(object sender, VirtualGridQueryHasChildRowsEventArgs e)
        {
            e.HasChildRows = (e.ViewInfo == this.virtualGrid.MasterViewInfo);
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            refrescar(null);
        }

        private void refrescar(List<int> listEntradas)
        {
            try
            {
                if (listEntradas != null)
                {
                    //Hay casos en los que voy a llamar a la función pero no se ha terminado de modificar ningun registro.
                    if (listEntradas.Count == 0) return;
                    addRegistrosTabla(listEntradas);
                    return;
                }
                else
                    refrescarTodo();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                refrescarTodo();
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void refrescarTodo()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                virtualGrid.MasterViewInfo.IsWaiting = true;
                myBackgroundWorkerGridView.RunWorkerAsync();
                RefreshData();
                virtualGrid.MasterViewInfo.IsWaiting = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void InicializarComboPaginacion()
        {
            if (XmlReaderPropio.getPaginacion() <= 10)
            {
                for (int i = 20; i < 501; i++)
                {
                    menuComboItem.Items.Add(i.ToString());
                }
            }
            else
            {
                for (int i = XmlReaderPropio.getPaginacion(); i < 501; i++)
                {
                    menuComboItem.Items.Add(i.ToString());
                }
            }
            menuComboItem.ComboBoxElement.SelectedIndexChanged += comboPaginacion_Changed;
        }

        private void comboPaginacion_Changed(object sender, EventArgs e)
        {
            try
            {
                int tamaño = int.Parse(menuComboItem.ComboBoxElement.SelectedItem.Text);
                virtualGrid.PageSize = tamaño;
                gridView.PageSize = tamaño;
                XmlReaderPropio.setPaginacion(tamaño);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                filterDialog.DataSource = gridView.DataSource;
                if (filterDialog.Visible == false)
                {
                    filterDialog.ShowDialog();
                }
                else
                {
                    filterDialog.Close();
                    filterDialog.DataFilter.ApplyFilter();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void EventoAbrirAltaProducto_Click(object sender, EventArgs e)
        {
            try
            {
                string operario = string.Empty;
                string tipoPalet = string.Empty;
                String nombreBotonPulsado = (sender as RadButtonElement).Name;
                Image icono = (sender as RadButtonElement).Image;
                String nombreFormularioOpcion = (sender as RadButtonElement).Text;
                int opcion = 0;
                switch (nombreBotonPulsado)
                {
                    case "rbtnAltaPaletRegularizacion":
                        opcion = FormularioAltaProductoRegularizacion.AltaPalet;
                        break;

                    case "rbtnAltaPaletPicosRegularizacion":
                        opcion = FormularioAltaProductoRegularizacion.AltaPaletPicos;
                        break;

                    case "rbtnAltaPaletMultiRegularizacion":
                        opcion = FormularioAltaProductoRegularizacion.AltaPaletMultireferencia;
                        break;
                }

                FormularioAltaProductoRegularizacion fap =
                        new FormularioAltaProductoRegularizacion(opcion, nombreFormularioOpcion, icono);
                fap.ShowDialog();
                //cambiamos el código para cargar una nueva línea.
                //Necesitamos descargar la línea nueva para meterla en el DataTable
                int resultado = fap.resultado;

                if (resultado > -1)
                {
                    List<int> listaEnt = new List<int>();
                    listaEnt.Add(resultado);
                    refrescar(listaEnt);
                }
                //refrescar();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void addRegistrosTabla(List<int> listaEntradas)
        {
            //Pasos:
            //Descargamos la información nueva de las entradas, luego comprobamos si se
            //tienen que actualizar o se tienen que añadir al final
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (int entrada in listaEntradas)
                {
                    bool encontrado = false;
                    DataTable linea = FuncionesGenerales.descargarJsonDatos(nombreJson, " EX.IDENTRADA = " + entrada);

                    if (linea.Rows.Count > 0)
                    {
                        for (int i = 0; i < tabla.Rows.Count; i++)
                        {
                            if (int.Parse(tabla.Rows[i][Lenguaje.traduce("Matrícula")].ToString()) == entrada)
                            {
                                for (int y = 0; y < tabla.Columns.Count; y++)
                                {
                                    if (linea.Columns[tabla.Columns[y].ColumnName] != null)
                                        tabla.Rows[i][tabla.Columns[y].ColumnName] = linea.Rows[0][tabla.Columns[y].ColumnName];
                                }

                                encontrado = true;
                                break;
                            }
                            if (encontrado) break;
                        }
                        if (!encontrado)
                        {
                            DataRow dtNuevo = tabla.Rows.Add();
                            int index = tabla.Rows.IndexOf(dtNuevo);

                            for (int y = 0; y < tabla.Columns.Count; y++)
                            {
                                if (linea.Columns[tabla.Columns[y].ColumnName] != null)
                                    tabla.Rows[index][tabla.Columns[y].ColumnName] = linea.Rows[0][tabla.Columns[y].ColumnName];
                            }
                        }
                    }
                    else
                    {
                        //El registro no se ha encontrado por lo que parece ser que se ha eliminado
                        for (int i = 0; i < tabla.Rows.Count; i++)
                        {
                            if (int.Parse(tabla.Rows[i][Lenguaje.traduce("Matrícula")].ToString()) == entrada)
                            {
                                tabla.Rows.Remove(tabla.Rows[i]);
                                encontrado = true;
                                break;
                            }
                            if (encontrado) break;
                        }
                    }
                }
                sincronizarDataTables();
                if (virtualGrid != null)
                    virtualGrid.TableElement.SynchronizeRows();
                GridDataView dataView = gridView.MasterTemplate.DataView as GridDataView;
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + dataView.Indexer.Items.Count;
                RefreshData();
                return;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void sincronizarDataTables()
        {
            //dtVirtualGrid =
            dataVirtualGrid = new DataTable();
            dataVirtualGrid = tabla.Clone();
            for (int i = 0; i < tabla.Columns.Count; i++)
            {
                dataVirtualGrid.Columns[i].DataType = typeof(String);
            }
            foreach (DataRow dtR in tabla.Rows)
            {
                dataVirtualGrid.ImportRow(dtR);
            }
            dtFiltradoVirtual = dataVirtualGrid.Copy();
        }

        private void rBtnBajaRegularizacion_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
            {
                if (gridView.SelectedCells.Count != 0)
                {
                    //GridViewRow row= gridView.Rows[gridView.SelectedCells[0].RowInfo.Index];
                    int idEntrada = Convert.ToInt32(gridView.ChildRows[gridView.SelectedCells[0].RowInfo.Index].Cells[Lenguaje.traduce("Matrícula")].Value);
                    FrmSalidasRegularizacion frm = new FrmSalidasRegularizacion(idEntrada);
                    frm.ShowDialog();
                    List<int> listEntradas = new List<int>();
                    listEntradas.Add(idEntrada);
                    refrescar(listEntradas);
                }
            }
            else
            {
                if (virtualGrid.Selection.CurrentRowIndex > 0)
                {
                    List<string> campos = new List<string>();
                    dynamic z = Newtonsoft.Json.JsonConvert.DeserializeObject(FuncionesGenerales.getRowGridView(virtualGrid, _lstEsquemaTabla, dtFiltradoVirtual, dataVirtualGrid, tabla));
                    var pn = (string)z[this._lstEsquemaTabla[0].Etiqueta];
                    campos.Add(pn);
                    CambiarPickingEstado cep = new CambiarPickingEstado(campos);
                    cep.Text = Lenguaje.traduce(strings.TituloPicking);
                    cep.ShowDialog();
                    if (cep.exito)
                    {
                        refrescar(cep.listEntradas);
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico));
                    }
                }
            }
        }

        private void rBtnImpresionSSCC_Click(object sender, EventArgs e)
        {
            FormularioImpresionEtiquetasSSCC frm = new FormularioImpresionEtiquetasSSCC();
            frm.ShowDialog();
        }

        private void rBtnImpExp_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FrmImportacionExportacion frm = new FrmImportacionExportacion();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error("Error importando stock:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public void btnExportacion_Click(object sender, EventArgs e)
        {
            try
            {
                FuncionesGenerales.exportarAExcelGenerico(this.gridView);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al exportar Excel en VisorSQLRibbon");
            }
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {
        }
    }
}
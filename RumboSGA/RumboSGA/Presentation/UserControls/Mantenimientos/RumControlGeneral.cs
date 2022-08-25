using Newtonsoft.Json.Linq;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;
using System.Data;
using System.Drawing;
using Telerik.WinControls.Data;
using Telerik.WinControls;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class RumControlGeneral : BaseGridControl
    {
        #region Variables y Propiedades

        private List<dynamic> data = new List<dynamic>();
        private DataTable datosGrid = null; //Está dataTable es necesaria para la funcionalidad de los RadVirtualGrid

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string nombreJsonLocal;
        private DataTable dt;//Data table principal
        private DataTable dtVirtualGrid = null; //Data table que se pone todos los tipos de columna

        //a string ya que el Virtualgrid solo actua sobre strings
        private DataTable dtFiltradoVirtual = null;

        //DataTable que es una copia del original pero con los registros filtrados, esto se separa para poder
        //reiniciar los filtros sin volver a descargar los datos
        private List<FilterDescriptor> filtrado = new List<FilterDescriptor>();

        private List<SortDescriptor> ordenado = new List<SortDescriptor>();

        #endregion Variables y Propiedades

        #region Constructor

        public RumControlGeneral(String nombreJson)
        {
            BaseGridControl.nombreJson = nombreJson;
            nombreJsonLocal = nombreJson;
            InitializeComponent();
            InicializarEventos();
            inicializarVirtualGrid();

            ElegirGrid();
            //ElegirEstilo();
            //RefreshData(0);
            if (GridView.Rows.Count != 0)
            {
                GridView.Rows[0].IsSelected = false;
            }
        }

        private void inicializarVirtualGrid()
        {
            //Esto se hace para remplazar el virtual grid que crea el baseGridControl por el nuevo que
            //usara este formulario.
            /*
            this.virtualGrid = new RadVirtualGrid();
            this.virtualGrid.SelectionMode = VirtualGridSelectionMode.FullRowSelect;
            this.virtualGrid.AllowFiltering = true;
            this.virtualGrid.AllowAddNewRow = false;
            this.virtualGrid.AllowColumnResize = true;
            this.virtualGrid.AllowRowResize = true;
            this.virtualGridControl.BestFitColumns();
            this.virtualGrid.CellValueNeeded += radVirtualGrid1_CellValueNeeded;
            this.virtualGrid.EnablePaging = false;
            this.virtualGrid.PageSize = _K_PAGINACION;
            this.virtualGrid.VirtualGridElement.PageIndexChanging += VirtualGridElement_PageIndexChanging;
            this.virtualGrid.BeginEditMode = RadVirtualGridBeginEditMode.BeginEditProgrammatically;
            //this.virtualGrid.VirtualGridElement.TableElement.PagingPanelElement.Margin = new Padding(0, 0, 0, 1);
            this.virtualGrid.AllowMultiColumnSorting = false;
            this.virtualGrid.SortChanged += RadGridView1_SortChanged;
            this.virtualGrid.FilterChanged += RadVirtualGrid_FilterChanged;
            this.virtualGrid.DoubleClick += RadGridView1_DoubleClick;
            this.virtualGrid.ContextMenuOpening += grid_ContextMenuOpening;
            this.virtualGrid.Font = new Font("Segoe UI", 8.25F);
            this.virtualGrid.TableElement.RowHeight = 40;
            this.virtualGrid.SelectionMode = VirtualGridSelectionMode.CellSelect;
            */
        }

        #endregion Constructor

        #region Funciones Auxiliares

        //Eventos Asíncronos y Síncronos
        private void InicializarEventos()
        {
            //Indicamos que las llamadas, para la recogida de datos, serán Asíncronas
            this.CallbackCantidad = new Action<int>(query =>
            {
                this.virtualGridControl.MasterViewInfo.IsWaiting = false;
                this.virtualGridControl.TableElement.SynchronizeRows();
            });

            this.Callback = new Action<List<dynamic>>(query =>
            {
                this.data = query;
                this.virtualGridControl.MasterViewInfo.IsWaiting = false;
                this.virtualGridControl.TableElement.SynchronizeRows();
                this.virtualGridControl.BestFitColumns();
            });

            //Declaración Eventos
        }

        //Prepara el grid con la cantidad total de filas y columnas a pintar luego por el evento RadVirtualGrid_CellValueNeeded
        private void InicializarFilasColumnasGrid()
        {
            this.virtualGridControl.ColumnCount = _lstEsquemaTabla.Count;
            this.virtualGridControl.RowCount = CantidadRegistros + 1;
        }

        //Obtención de cantidad total de registros
        private void ObtencionTotalRegistros()
        {
            //Obtenemos, asíncronamente, la cantidad total de registros, así como su esquema de datos, para poder preparar el Grid
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            CantidadRegistros = FuncionesGenerales.CargarEsquemaYSacarCount(nombreJsonLocal, "", ref _lstEsquemaTabla);
            recargarNumeroRegistros(CantidadRegistros);
        }

        private void recargarNumeroRegistros(int _cantidadRegistros)
        {
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + _cantidadRegistros;
        }

        #endregion Funciones Auxiliares

        #region Eventos Propios

        public override void btnCambiarVista_Click(object sender, EventArgs e)
        {
            if (TblLayout.GetControlFromPosition(0, 0) is RadGridView)
            {
                TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 0));
                TblLayout.Controls.Add(virtualGridControl, 0, 0);
                TblLayout.SetColumnSpan(virtualGridControl, 10);
                virtualGridControl.Dock = DockStyle.Fill;
                virtualGridControl.Refresh();
                if (GridView.FilterDescriptors.Count > 0)
                {
                    filtrado.Clear();
                    foreach (FilterDescriptor item in GridView.FilterDescriptors)
                    {
                        filtrado.Add(item);
                    }
                    this.virtualGridControl.FilterDescriptors.Clear();
                    this.virtualGridControl.FilterDescriptors.AddRange(filtrado);
                }
                if (GridView.SortDescriptors.Count > 0)
                {
                    ordenado.Clear();
                    foreach (SortDescriptor item in GridView.SortDescriptors)
                    {
                        ordenado.Add(item);
                    }
                    this.virtualGridControl.SortDescriptors.Clear();
                    this.virtualGridControl.SortDescriptors.AddRange(ordenado);
                }

                ElegirEstilo();
            }
            else if (TblLayout.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                if (numRegistrosFiltrados > 100000)
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ExcesoRegistros));
                    return;
                }
                else
                {
                    if (numRegistrosFiltrados > 10000)
                    {
                        DialogResult result = MessageBox.Show(Lenguaje.traduce("Aviso:" + strings.AvisoRegistros) + "\n" + Lenguaje.traduce("¿Estas seguro?"), strings.Confirmar, MessageBoxButtons.YesNo);
                        if (result == DialogResult.No) return;
                    }
                    if (GridView.DataSource == null)
                    {
                        llenarGrid();
                    }
                    TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 0));
                    TblLayout.Controls.Add(GridView, 0, 0);
                    TblLayout.SetColumnSpan(GridView, 10);
                    //btnExportacion.Enabled = true;
                    GridView.Dock = DockStyle.Fill;
                    ElegirEstilo();

                    if (virtualGrid.FilterDescriptors.Count > 0)
                    {
                        filtrado.Clear();
                        foreach (FilterDescriptor item in virtualGrid.FilterDescriptors)
                        {
                            filtrado.Add(item);
                        }
                        this.GridView.FilterDescriptors.Clear();
                        this.GridView.FilterDescriptors.AddRange(filtrado);
                    }

                    if (virtualGrid.SortDescriptors.Count > 0)
                    {
                        ordenado.Clear();
                        foreach (SortDescriptor item in virtualGrid.SortDescriptors)
                        {
                            ordenado.Add(item);
                        }
                        this.GridView.SortDescriptors.Clear();
                        this.GridView.SortDescriptors.AddRange(ordenado);
                    }
                }
            }
            else
            {
                MessageBox.Show("Error inesperado");
            }
        }

        //Selección de un registro del grid
        private void GridControl_SelectionChanged(object sender, EventArgs e)
        {
            _selectedRow = null;

            if (this.virtualGridControl.CurrentCell != null && this.virtualGridControl.CurrentCell.RowIndex >= 0)
            {
                if ((virtualGridControl.CurrentCell.RowIndex % virtualGridControl.PageSize) >= data.Count)
                {
                    return;
                }

                int index = this.virtualGridControl.CurrentCell.RowIndex - this.virtualGridControl.PageSize * this.virtualGridControl.PageIndex;
                var item = data[index];

                _selectedRow = item; //Fila seleccionada, en formato JSON
            }
        }

        protected override void getGridViewRow()
        {
            if (TblLayout.GetControlFromPosition(0, 0) != null)
                _selectedRow = FuncionesGenerales.getRowGridView(TblLayout.GetControlFromPosition(0, 0), _lstEsquemaTabla, dtFiltradoVirtual, dtVirtualGrid, dt);
            return;
        }

        #endregion Eventos Propios

        #region Eventos Sobreescritos de la clase padre BaseGridControl

        protected override void ElegirGrid()
        {
            ObtencionTotalRegistros();

            if (CantidadRegistros > Persistencia.NumRegistrosVirtualGrid)
            {
                TblLayout.Controls.Clear();
                TblLayout.Controls.Add(virtualGridControl, 0, 0);
            }
            else
            {
                TblLayout.Controls.Clear();
                TblLayout.Controls.Add(GridView, 0, 0);
                TblLayout.SetColumnSpan(GridView, 9);
                GridView.Dock = System.Windows.Forms.DockStyle.Fill;
                GridView.BestFitColumns();
            }
            llenarGrid();
        }

        public override void llenarGrid()
        {
            if (_lstEsquemaTabla.Count != 0)
            {
                try
                {
                    base.llenarGrid();

                    dt = null;
                    dtVirtualGrid = null;
                    dtFiltradoVirtual = null;

                    this.virtualGrid.MasterViewInfo.IsWaiting = true;
                    //Business.GetReservasHistoricoCantidad(ref _lstEsquemaTabla);
                    dt = FuncionesGenerales.descargarJsonDatos(nombreJsonLocal, "");

                    dtVirtualGrid = dt.Clone();
                    for (int i = 0; i < dtVirtualGrid.Columns.Count; i++)
                    {
                        dtVirtualGrid.Columns[i].DataType = typeof(String);
                    }

                    foreach (DataRow dtR in dt.Rows)
                    {
                        dtVirtualGrid.ImportRow(dtR);
                    }

                    dtFiltradoVirtual = dtVirtualGrid.Copy();
                    virtualGrid.ColumnCount = dtFiltradoVirtual.Columns.Count;
                    virtualGrid.RowCount = dtFiltradoVirtual.Rows.Count;

                    this.GridView.DataSource = dt;

                    this._esquemGrid = FuncionesGenerales.descargarJsonEsquemaGridAdjunto(nombreJsonLocal);
                    foreach (var column in GridView.Columns)
                    {
                        if (GridView.Columns[column.Index].Name.Contains("ID"))
                        {
                            GridView.Columns[column.Index].IsVisible = false;
                        }
                        //string Nombre = ("COLORAMOSTRAR");
                        //column.Name = _lstEsquemaTabla[column.Index].Nombre;
                        //column.FieldName = _lstEsquemaTabla[column.Index].Etiqueta;
                    }
                    recargarNumeroRegistros(dt.Rows.Count);
                    ElegirEstilo();
                    // Funcion que rellena el fondo si la columna es tipo Color
                    rellenarColorGrid(dt);
                    // Función que elige la fecha de acuerdo a el tipo de control DATE, TIME y DATETIME
                    elegirFormatoFecha(dt);
                }
                catch (Exception e)
                {
                    log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                    MessageBox.Show(strings.ErrorLlenarGrid);
                }
                finally
                {
                    this.virtualGridControl.MasterViewInfo.IsWaiting = false;
                }
            }
        }

        private void añadirCamposAuxiliares(RadGridView grid, DataTable dt)
        {
            for (int i = 0; i < _lstEsquemaTabla.Count; i++)
            {
                if (_lstEsquemaTabla[i].CmbObject.CampoMostradoAux != null)
                {
                    if (_lstEsquemaTabla[i].CmbObject.CampoMostradoAux.Contains(","))
                    {
                        GridView.Rows.Add(i++);
                    }
                    else
                    {
                    }
                }
            }
        }

        private void elegirFormatoFecha(DataTable dt) //Función que verifica el control del JSON y muestra fecha según el caso DATE, TIME & DATETIME.
        {
            foreach (TableScheme item in _lstEsquemaTabla)
            {
                var index = dt.Columns.IndexOf(item.Etiqueta);
                GridViewDataColumn datos = GridView.Columns[index];
                switch (item.Control.ToString())
                {
                    case "DATE": //Cambia el formato de la celda a un formato de tipo DATE
                        datos.FormatString = "{0:dd/MM/yyyy}";
                        break;

                    case "TIME": //Cambia el formato de la celda a un formato de tipo TIME
                        datos.FormatString = "{0:HH:mm:ss}";
                        break;

                    case "DATETIME": //Cambia el formato de la celda a un formato de tipo DATETIME
                        datos.FormatString = "{0:dd/MM/yyyy HH:mm:ss}";
                        break;
                }
            }
        }

        private void rellenarColorGrid(DataTable dt)
        {
            foreach (TableScheme item in _lstEsquemaTabla)

            {
                if (item.Control.Equals("COL"))
                {
                    // index obtiene el numero de columna que queremos modificar el color
                    var index = dt.Columns.IndexOf(item.Etiqueta);
                    for (int x = 0; x <= dt.Rows.Count - 1; x++)
                    {
                        //i obtiene los int del campo color
                        var i = dt.Rows[x].Field<int>(index);
                        var myColor = Color.FromArgb(i);
                        var cell = GridView.Rows[x].Cells[index];

                        //Cambia a transparente el color de la letra
                        cell.Style.ForeColor = Color.Transparent;

                        //Estilos del relleno
                        cell.Style.CustomizeFill = true;
                        cell.Style.GradientStyle = GradientStyles.Solid;
                        cell.Style.BackColor = myColor;

                        // Estilos de los bordes
                        cell.Style.CustomizeBorder = true;
                        cell.Style.BorderWidth = 1;
                        cell.Style.BorderColor = Color.Black;
                    }
                }
            }
        }

        //Este evento sólo se producirá la primera vez que cargue el número total de registros de BBDD
        protected override void TbCantidadRegistros_TextChanged(object sender, EventArgs e)
        {
        }

        //Función que, asíncronamente, consulta los datos de Proveedores de la página seleccionada en el Grid
        public override void RefreshData(int pageIndex)
        {
            //Actualizamos el número de filas totales que tendrá el grid con la variable CantidadRegistros heredada y que puede haber sido modificada al crear, clonar y eliminar registros
            //InicializarFilasColumnasGrid();

            //Limpiamos datos
            //llenarGrid();
            //ElegirEstilo();
        }

        //Evento que pinta, celda a celda, los datos del Grid
        protected override void RadVirtualGrid_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
        {
            try
            {
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

                if (e.RowIndex >= 0 && e.RowIndex < dt.Rows.Count)
                {
                    //e.Value = data[e.RowIndex][e.ColumnIndex];
                    e.Value = dtFiltradoVirtual.Rows[e.RowIndex][e.ColumnIndex];
                }
            }
            catch (Exception ex)
            {
                //No voy a registrar en el log ya que en caso de que esto de error, puede generar muchisimos logs ralentizarían el programa
            }
        }

        protected override void radGridView1_SortChanged(object sender, Telerik.WinControls.UI.GridViewCollectionChangedEventArgs e)
        {
            RadGridView_FilterChanged(sender, e);
        }

        protected override void RadGridView1_SortChanged(object sender, VirtualGridEventArgs e)
        {
            //Aunque se llama RadGridView1 este metodo es para virtualGrid.
            RadVirtualGrid_FilterChanged(sender, e);
        }

        protected override void RadVirtualGrid_FilterChanged(object sender, VirtualGridEventArgs e)
        {
            try
            {
                virtualGrid.MasterViewInfo.IsWaiting = true;
                if (dtVirtualGrid == null) return;
                string ex = virtualGrid.FilterDescriptors.Expression;
                string sort = virtualGrid.SortDescriptors.Expression;

                dtFiltradoVirtual.Rows.Clear();
                if (dtVirtualGrid.Select(ex, sort).Length > 0)
                    dtFiltradoVirtual = dtVirtualGrid.Select(ex, sort).CopyToDataTable();

                virtualGrid.RowCount = dtFiltradoVirtual.Rows.Count;
                recargarNumeroRegistros(dtFiltradoVirtual.Rows.Count);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en cambiar RadVirtualGrid_FilterChanged puede que se haya intentado filtrar algo con formato incorrecto ");
            }
            finally
            {
                virtualGrid.MasterViewInfo.IsWaiting = false;
            }
        }

        protected override void RadGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            /*virtualGrid.FilterDescriptors.Clear();
            foreach (var item in GridView.FilterDescriptors)
            {
                var newDescriptor = new FilterDescriptor(item.PropertyName, item.Operator, item.Value);
                virtualGrid.FilterDescriptors.Add(newDescriptor);
            }*/
        }

        public override void newButton_Click(object sender, EventArgs e)
        {
            getGridViewRow();
            base.newButton_Click(sender, e);
        }

        public override void cloneButton_Click(object sender, EventArgs e)
        {
            getGridViewRow();
            base.cloneButton_Click(sender, e);
        }

        public override void deleteButton_Click(object sender, EventArgs e)
        {
            getGridViewRow();
            base.deleteButton_Click(sender, e);
        }

        //Acción para crear un ReservaHistorico
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return FuncionesGenerales.jsonInsert(nombreJsonLocal, selectedRow);
        }

        //Acción para editar un ReservaHistorico
        protected override AckResponse EditData(dynamic rowNueva)
        {
            AckResponse ack = FuncionesGenerales.jsonUpdate(nombreJsonLocal, rowNueva, FuncionesGenerales.ComponerJson(_lstEsquemaTabla, _selectedRow));

            String resultado = FuncionesEspecialesRumControlGeneral.funcionesEspecialesDespuesEditar(
                nombreJsonLocal, rowNueva, ack);

            if (resultado.Equals(""))
            {
            }
            else if (resultado.Equals("KO"))
            {
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(resultado));
            }

            return ack;
        }

        //Acción para eliminar un ReservasHistorico
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return FuncionesGenerales.jsonDelete(nombreJsonLocal, FuncionesGenerales.ComponerJson(_lstEsquemaTabla, _selectedRow));
        }

        public override void GridViewPageChanged(object sender, EventArgs e)
        {
        }

        protected override void RadGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (sender is RadVirtualGrid)
            {
                if ((sender as RadVirtualGrid).CurrentCell == null || (sender as RadVirtualGrid).CurrentCell.RowIndex < 0) return;
            }
            if (sender is RadGridView)
            {
                if ((sender as RadGridView).CurrentCell == null || (sender as RadGridView).CurrentRow.Index < 0) return;
            }
            getGridViewRow();
            base.RadGridView1_DoubleClick(sender, e);
        }

        #endregion Eventos Sobreescritos de la clase padre BaseGridControl
    }
}
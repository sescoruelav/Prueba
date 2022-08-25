using RumboSGA.Properties;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class ProveedoresPedidosCabControl : BaseGridControl
    {
        #region Variables y Propiedades

        List<dynamic> data = new List<dynamic>();        

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }

        #endregion

        #region Constructor

        public ProveedoresPedidosCabControl()
        {
            InitializeComponent();

            InicializarEventos();

            ObtencionTotalRegistros();

            ElegirGrid();
            //ElegirEstilo();
            RefreshData(0);
            if (GridView.Rows.Count != 0)
            {
                GridView.Rows[0].IsSelected = false;
            }
        }

        #endregion

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
            this.virtualGridControl.SelectionChanged += GridControl_SelectionChanged;
            this.GridView.SelectionChanged += GridViewControl_SelectionChanged;
            this.GridView.FilterChanged += gridViewFilterChanged_Event;
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
            //Obtenemos, asíncronamente, la cantidad total de registros de Proveedores, así como su esquema de datos, para poder preparar el Grid                        
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            CantidadRegistros = Business.GetProveedoresPedidosCabCantidad(ref _lstEsquemaTabla, ref _esquemGrid);
            lblCantidad.Text = strings.NProductos + ":" + CantidadRegistros;

        }
        #endregion

        #region Eventos Propios               
        public override void btnCambiarVista_Click(object sender, EventArgs e)
        {


            if (TblLayout.GetControlFromPosition(0, 0) is RadGridView)
            {
                TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 0));
                TblLayout.Controls.Add(virtualGridControl, 0, 0);
                TblLayout.SetColumnSpan(virtualGridControl, 10);
                virtualGridControl.Dock = DockStyle.Fill;

                ElegirEstilo();
            }
            else if (TblLayout.GetControlFromPosition(0, 1) is RadVirtualGrid)
            {

                if (numRegistrosFiltrados > 100000)
                {
                    MessageBox.Show(strings.ExcesoRegistros);
                }
                else
                {
                    if (numRegistrosFiltrados > 50000)
                    {
                        MessageBox.Show("Aviso:" + strings.AvisoRegistros);
                    }
                    if (GridView.DataSource == null)
                    {
                        llenarGrid();
                    }
                    TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 0));
                    TblLayout.Controls.Add(GridView, 0, 0);
                    TblLayout.SetColumnSpan(GridView, 10);
                    GridView.Dock = DockStyle.Fill;

                    ElegirEstilo();
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
        private void GridViewControl_SelectionChanged(object sender, EventArgs e)
        {
            _selectedRow = null;

            if (this.GridView.CurrentCell != null && this.GridView.CurrentCell.RowIndex >= 0)
            {
                int index = GridView.CurrentCell.RowIndex;


                var item = data[index];
                _selectedRow = item; //Fila seleccionada, en formato JSON
            }
        }

        #endregion

        #region Eventos Sobreescritos de la clase padre BaseGridControl 
        protected override void ElegirGrid()
        {
            if (CantidadRegistros > 10000)
            {
                ObtencionTotalRegistros();

                TblLayout.Controls.Add(virtualGridControl, 0, 0);
                //btnExportacion.Enabled = false;

            }
            else
            {
                llenarGrid();
                TblLayout.Controls.Add(GridView, 0, 0);
                TblLayout.SetColumnSpan(GridView, 9);
                GridView.Dock = System.Windows.Forms.DockStyle.Fill;
                GridView.BestFitColumns();
                //btnExportacion.Enabled = true;

            }
        }
        public override void llenarGrid()
        {

            base.llenarGrid();
            Business.GetPaletsTipoCantidad(ref _lstEsquemaTabla);
            //GridView.DataSource = Business.(_lstEsquemaTabla);
        }
        //Este evento sólo se producirá la primera vez que cargue el número total de registros de BBDD
        protected override void TbCantidadRegistros_TextChanged(object sender, EventArgs e)
        {
            string cantidad = ((RadTextBox)sender).Text;
            if (!string.IsNullOrEmpty(cantidad) && (int.Parse(cantidad)) > 0)
            {
                CantidadRegistros = int.Parse(cantidad);
                //Cargamos la primera página del grid
                this.RefreshData(0);
                
            }
        }

        //Función que, asíncronamente, consulta los datos de Proveedores de la página seleccionada en el Grid
        public override void RefreshData(int pageIndex)
        {
            //Actualizamos el número de filas totales que tendrá el grid con la variable CantidadRegistros heredada y que puede haber sido modificada al crear, clonar y eliminar registros
            InicializarFilasColumnasGrid();

            //Limpiamos datos
            data.Clear();
            
            int pagInicial = 0;
            int pagFinal = 0;
            pageIndex++;

            if (pageIndex == 1)
            {
                pagInicial = 1;
                pagFinal = _K_PAGINACION;
            }
            else
            {
                pagInicial = _K_PAGINACION * (pageIndex - 1) + 1;
                pagFinal = (_K_PAGINACION * pageIndex);
            }

            string sortExpression = "";
            string filterExpression = this.virtualGridControl.FilterDescriptors.Expression;
            foreach (var item in this.virtualGridControl.SortDescriptors)
            {
                sortExpression += "PROCAB." + item.PropertyName;
                if (item.Direction.ToString() == "Ascending")
                {
                    sortExpression += " ASC,";
                }
                else
                {
                    sortExpression += " DESC,";
                }
            }
            if (sortExpression != string.Empty)
            {
                sortExpression = sortExpression.TrimEnd(',');

            }
            if (sortExpression == string.Empty)
            {
                sortExpression = "IDPEDIDOPRO ASC";
            }
            if (filterExpression != null&&filterExpression!=string.Empty)
            {
                filterExpression = " WHERE " + filterExpression;
                numRegistrosFiltrados = Business.GetProveedoresPedidosCabRegistrosFiltrados(filterExpression);
                lblCantidad.Text = strings.NProductos + ":" + numRegistrosFiltrados;

            }
            else
            {
                numRegistrosFiltrados = Business.GetProveedoresPedidosCabRegistrosFiltrados("");
                lblCantidad.Text = strings.NProductos + ":" + numRegistrosFiltrados;
            }

            this.virtualGridControl.MasterViewInfo.IsWaiting = true;

            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetProveedoresPedidosCabDatos(sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
            ElegirEstilo();
        }

        //Evento que pinta, celda a celda, los datos del Grid
        protected override void RadVirtualGrid_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
        {
            base.RadVirtualGrid_CellValueNeeded(sender, e);

            if (e.ColumnIndex < 0)
                return;
            if (e.RowIndex < 0)
            {
                e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
            }


            if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {
                if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta == null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                }
                else if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta != null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Etiqueta;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Etiqueta;
                }
            }
            else
            {
                if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta == null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                }
                else if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta != null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Etiqueta;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Etiqueta;
                }
            }


            if (e.RowIndex >= 0 && data.Count > 0)
            {
                if ((e.RowIndex % virtualGridControl.PageSize) >= data.Count)
                {
                    return;
                }

                try
                {
                    int index = e.RowIndex - this.virtualGridControl.PageSize * this.virtualGridControl.PageIndex;

                    var z = Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(this.data[index]));

                    var pn = (string)z[this._lstEsquemaTabla[e.ColumnIndex].Nombre];

                    if (pn != null)
                    {
                        e.Value = pn.ToString();
                    }
                    else
                    {
                        e.Value = string.Empty;
                    }
                }
                catch (Exception exc)
                {
                    string ex = exc.Message;
                }
            }
        }
        private void gridViewFilterChanged_Event(object sender, EventArgs e)
        {
            if (GridView.FilterDescriptors.Expression != null && GridView.FilterDescriptors.Expression != string.Empty)
            {
            lblCantidad.Text = strings.NProductos + ":" + Business.GetProveedoresPedidosCabRegistrosFiltrados(" WHERE " + GridView.FilterDescriptors.Expression).ToString();

            }
        }
        //Acción para crear un Pedido
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaProveedorPedidoCab(_lstEsquemaTabla, selectedRow);            
        }

        protected override AckResponse NewDataAndLines(dynamic selectedRow, bool bolCloneLines)
        {
            //if (bolCloneLines)
            //{
                return Business.AltaProveedorPedidoCabYLineas(_lstEsquemaTabla, selectedRow, _esquemGrid);
            //}
            //else
            //{
            //   return Business.AltaProveedorPedidoCab(_lstEsquemaTabla, selectedRow);
            //}            
        }

        //Acción para editar un Pedido
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarProveedorPedidoCab(_lstEsquemaTabla, selectedRow);
        }

        //Acción para eliminar un Pedido
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarProveedorPedidoCab(_lstEsquemaTabla, selectedRow);            
        }

        #endregion
    }
}

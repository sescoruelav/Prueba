using Newtonsoft.Json.Linq;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class ProveedoresPedidosLineasControl : BaseGridControl
    {
        #region Variables y Propiedades

        List<dynamic> data = new List<dynamic>();        
        //public List<dynamic> data = new List<dynamic>();

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }

        #endregion

        #region Constructor

        public ProveedoresPedidosLineasControl()
        {
            InitializeComponent();

            InicializarEventos();

            //LlenarGrid();
            ElegirEstilo();
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

            this.Load += ProveedoresPedidosLineasControl_Load;
        }

        private void ProveedoresPedidosLineasControl_Load(object sender, EventArgs e)
        {            
            ObtencionTotalRegistros();
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
            //int _cantidad = Business.GetProveedoresPedidosLineasCantidad(ref _lstEsquemaTablaProveedores, FiltroExterno);
            //TbCantidadRegistrosControl.Text = _cantidad.ToString();
        }
        #endregion

        #region Eventos Propios               

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

        #endregion

        #region Eventos Sobreescritos de la clase padre BaseGridControl 

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
                pagFinal = _K_PAGINACION - 1;
            }
            else
            {
                pagInicial = _K_PAGINACION * (pageIndex - 1);
                pagFinal = (_K_PAGINACION * pageIndex) - 1;
            }

            string sortExpression = this.virtualGridControl.SortDescriptors.Expression;
            string filterExpression = this.virtualGridControl.FilterDescriptors.Expression;

            if (sortExpression == string.Empty)
            {
                sortExpression = "IDPEDIDOPRO ASC";
            }

            /*if (filterExpression != string.Empty)
            {
                filterExpression = " WHERE " + filterExpression;
            }*/
            if (FiltroExterno != string.Empty)
            {
                filterExpression = FiltroExterno;
            }


            this.virtualGridControl.MasterViewInfo.IsWaiting = true;

            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetProveedoresPedidosLineasDatos(sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
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
                if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta == null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                }
                else if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta != null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Etiqueta;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
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
        public override void newButton_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    JObject objetoJson = JObject.Parse(data[i].ToString());

                    string jsonID = (string)objetoJson[_lstEsquemaTabla[0].Nombre];
                    if (this.GridView.CurrentRow.Cells[_lstEsquemaTabla[0].Nombre].Value.ToString() == jsonID)
                    {
                        _selectedRow = data[i];
                        break;
                    }
                }
                base.newButton_Click(sender, e);
            }
            else
            {
                base.newButton_Click(sender, e);
            }
        }
        public override void cloneButton_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    JObject objetoJson = JObject.Parse(data[i].ToString());

                    string jsonID = (string)objetoJson[_lstEsquemaTabla[0].Nombre];
                    if (this.GridView.CurrentRow.Cells[_lstEsquemaTabla[0].Nombre].Value.ToString() == jsonID)
                    {
                        _selectedRow = data[i];
                        break;
                    }
                }
                base.cloneButton_Click(sender, e);
            }
            else
            {
                base.cloneButton_Click(sender, e);
            }

        }
        public override void deleteButton_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    JObject objetoJson = JObject.Parse(data[i].ToString());

                    string jsonID = (string)objetoJson[_lstEsquemaTabla[0].Nombre];
                    if (this.GridView.CurrentRow.Cells[_lstEsquemaTabla[0].Nombre].Value.ToString() == jsonID)
                    {
                        _selectedRow = data[i];
                        break;
                    }
                }
                base.deleteButton_Click(sender, e);
            }
            else
            {
                base.deleteButton_Click(sender, e);
            }

        }

        //Acción para crear un Pedido
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaProveedorPedidoLineas(_lstEsquemaTabla, selectedRow);            
        }

        //Acción para editar un Pedido
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarProveedorPedidoLineas(_lstEsquemaTabla, selectedRow);
        }

        //Acción para eliminar un Pedido
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarProveedorPedidoLineas(_lstEsquemaTabla, selectedRow);            
        }

        #endregion
    }
}

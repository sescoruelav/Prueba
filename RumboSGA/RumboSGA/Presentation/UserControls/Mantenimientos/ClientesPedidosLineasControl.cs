using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class ClientesPedidosLineasControl : BaseGridControl
    {
        #region Variables y Propiedades

        List<dynamic> data = new List<dynamic>();
        //public List<dynamic> data = new List<dynamic>();

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor

        public ClientesPedidosLineasControl()
        {
            InitializeComponent();

            InicializarEventos();

            TblLayout.Controls.Add(virtualGridControl,0,0);
            TblLayout.SetColumnSpan(virtualGridControl,10);

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

            this.Load += ClientesPedidosLineasControl_Load;
        }

        private void ClientesPedidosLineasControl_Load(object sender, EventArgs e)
        {
            this.name = Lenguaje.traduce("Líneas pedido cliente");
            ObtencionTotalRegistros();
            RefreshData(0);
            llenarGrid();

        }
        protected override void ElegirGrid()
        {
            if (CantidadRegistros > 10000)
            {
                ObtencionTotalRegistros();

                TblLayout.Controls.Add(virtualGridControl, 0, 0);

            }
            else
            {
                llenarGrid();
                TblLayout.Controls.Add(GridView, 0, 0);
                TblLayout.SetColumnSpan(GridView, 9);
                GridView.Dock = System.Windows.Forms.DockStyle.Fill;
                GridView.BestFitColumns();
            }
        }
        //Prepara el grid con la cantidad total de filas y columnas a pintar luego por el evento RadVirtualGrid_CellValueNeeded
        private void InicializarFilasColumnasGrid()
        {
            this.virtualGridControl.ColumnCount = _lstEsquemaTabla.Count;
            //this.virtualGridControl.RowCount = CantidadRegistros + 1;
        }

        //Obtención de cantidad total de registros
        private void ObtencionTotalRegistros()
        {
            //Obtenemos, asíncronamente, la cantidad total de registros de Proveedores, así como su esquema de datos, para poder preparar el Grid                        
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            int _cantidad = Business.GetClientesPedidosLineasCantidad(ref _lstEsquemaTabla, FiltroExterno);
            //TbCantidadRegistrosControl.Text = _cantidad.ToString();
        }
        #endregion

        #region Eventos Propios               

        protected override void getGridViewRow()
        {

            try
            {
                //No hay ninguna fila seleccionada
                if (this.virtualGridControl.CurrentCell == null && this.virtualGridControl.CurrentCell.RowIndex < 0)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Error no hay línea seleccionada."));
                    log.Error("this this.GridView.CurrentRow vale null en el gridview:" + this.GridView.Name +
                        " de la ventana ClientesPedidosCabControl");
                    return;
                }


                int index = this.virtualGridControl.CurrentCell.RowIndex - this.virtualGridControl.PageSize * this.virtualGridControl.PageIndex;
                var item = data[index];

                _selectedRow = item;

            }
            catch (Exception ex)
            {
                log.Error("Error seleccionando la línea en getGridViewRow de ClientesPedidosLineasControl:" + ex.Message+" Stacktrace:"+ex.StackTrace);
                _selectedRow = null;
                return;
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

                _selectedRow = item;
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

                llenarGrid();

            }
        }

        public override void llenarGrid()
        {
            if (_lstEsquemaTabla.Count != 0)
            {
                try
                {
                    
                    base.llenarGrid();
                    GridView.BeginUpdate();                    
                    Business.GetClientesPedidosLineasCantidad(ref _lstEsquemaTabla, FiltroExterno);
                    DataTable dt = Business.GetClientesPedidosLineasDatosGridView(FiltroExterno, "IDPEDIDOCLI ASC", _lstEsquemaTabla);
                    Utilidades.TraducirDataTableColumnName(ref dt);
                    virtualGridControl.RowCount = dt.Rows.Count + 1;
                    GridView.DataSource = dt;
                    GridView.Refresh();
                    GridView.Dock = System.Windows.Forms.DockStyle.Fill;
                    if(GridView.Columns["ID Pedido"] != null)
                    {
                        //GridView.Columns["ID Pedido"].IsVisible = false;
                    }
                }
                catch (Exception e)
                {
                    log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorLlenarGrid));
                }
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
                sortExpression = "IDPEDIDOCLI ASC";
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
            
            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetClientesPedidosLineasDatos(sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
            ElegirEstilo();
        }
        public void deshabilitarBotones()
        {
            
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

        //Acción para crear un Pedido
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaClientePedidoLineas(_lstEsquemaTabla, selectedRow);
        }

        //Acción para editar un Pedido
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarClientePedidoLineas(_lstEsquemaTabla, selectedRow);
        }

        //Acción para eliminar un Pedido
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarClientePedidoLineas(_lstEsquemaTabla, selectedRow);
        }

        #endregion
    }
}
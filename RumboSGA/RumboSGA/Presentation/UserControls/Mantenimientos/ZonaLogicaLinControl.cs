
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class ZonaLogicaLinControl : BaseGridControl
    {


        #region Variables y Propiedades

        List<dynamic> data = new List<dynamic>();
        //public List<dynamic> data = new List<dynamic>();

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructor

        public ZonaLogicaLinControl()
        {
            InitializeComponent();

            InicializarEventos();

            TblLayout.Controls.Add(virtualGridControl,0,0);
            TblLayout.SetColumnSpan(virtualGridControl,10);
           // ElegirGrid();
            //ElegirEstilo();
            //RefreshData(0);

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

            this.Load += ZonaLogicaLinControl_Load;
           /* contextMenu = new RadContextMenu();
            RadMenuItem menuItem1 = new RadMenuItem("Nuevo");
            menuItem1.Click += new EventHandler(newButton_Click);
            RadMenuItem menuItem2 = new RadMenuItem("Eliminar");
            menuItem2.Click += new EventHandler(deleteButton_Click);
            contextMenu.Items.Add(menuItem1);
            contextMenu.Items.Add(menuItem2);
            this.virtualGridControl.ContextMenuOpening += radGridView1_ContextMenuOpening;*/
        }

        private void radGridView1_ContextMenuOpening(object sender, VirtualGridContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = contextMenu.DropDown;
        }

        private void ZonaLogicaLinControl_Load(object sender, EventArgs e)
        {
            ObtencionTotalRegistros();
            this.RefreshData(0);           
            /* ElegirGrid();
             RefreshData(0);
             if (GridView.Rows.Count != 0)
             {
                 GridView.Rows[0].IsSelected = true;
             }*/
        }

        //Prepara el grid con la cantidad total de filas y columnas a pintar luego por el evento RadVirtualGrid_CellValueNeeded
        private void InicializarFilasColumnasGrid()
        {
            this.virtualGridControl.ColumnCount = _lstEsquemaTabla.Count;
            this.virtualGridControl.RowCount = CantidadRegistros + 1;
            //InitializeRowContextMenuItems();
        }

        /*private void InitializeRowContextMenuItems()
        {
            ObservableCollection<MenuItem> items = new ObservableCollection<MenuItem>();
            MenuItem addItem = new MenuItem();
            addItem.Text = "Nuevo";
            items.Add(addItem);
            MenuItem editItem = new MenuItem();
            editItem.Text = "Editar";
            items.Add(editItem);
            MenuItem deleteItem = new MenuItem();
            deleteItem.Text = "Eliminar";
            items.Add(deleteItem);
            this.virtualGridControl.rowContextMenuItems = items;
        }*/

        //Obtención de cantidad total de registros
        private void ObtencionTotalRegistros()
        {
            //Obtenemos, asíncronamente, la cantidad total de registros de Proveedores, así como su esquema de datos, para poder preparar el Grid                        
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            CantidadRegistros = Business.GetZonaLogLineasCantidad(ref _lstEsquemaTabla, FiltroExterno);
            
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
        public override void llenarGrid()
        {
            if (_lstEsquemaTabla.Count != 0)
            {
                try
                {
                    base.llenarGrid();
                    Business.GetZonaLogLineasCantidad(ref _lstEsquemaTabla, FiltroExterno);
                    GridView.DataSource = Business.GetZonaLogLineasDatosGridView(_lstEsquemaTabla);
                    foreach (var column in GridView.Columns)
                    {
                        column.Name = _lstEsquemaTabla[column.Index].Nombre;
                        column.FieldName = _lstEsquemaTabla[column.Index].Etiqueta;

                    }
                }
                catch (Exception e)
                {
                    log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorLlenarGrid));
                }
            }
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
                sortExpression = "IDZONACAB ASC";
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

            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetZonaLogLineasDatos(sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
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

        //Acción para crear una Linea
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaZonaLogLineas(_lstEsquemaTabla, selectedRow);
        }

        //Acción para editar una Linea
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarZonaLogLineas(_lstEsquemaTabla, selectedRow);
        }

        //Acción para eliminar una Linea
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarZonaLogLineas(_lstEsquemaTabla, selectedRow);
        }

        #endregion
    }
}

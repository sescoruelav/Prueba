using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class RumControlGeneralLineas : BaseGridControl
    {
        #region Variables y Propiedades

        List<dynamic> data = new List<dynamic>();
        //public List<dynamic> data = new List<dynamic>();

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string nombreJsonLocal;
        #endregion

        #region Constructor

        public RumControlGeneralLineas(string nombre,String nombreJson,String filtroExterno, Hashtable _diccParamNuevo)
        {
            this.nombreJsonLocal = nombreJson;
            this.FiltroExterno = filtroExterno;
            this.diccParamNuevo = _diccParamNuevo;
            InitializeComponent();
            InicializarEventos();

            //ElegirGrid();
            

            TblLayout.SetColumnSpan(GridView, 9);
            GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            GridView.BestFitColumns();

            if (GridView.Rows.Count != 0)
            {
                GridView.Rows[0].IsSelected = false;
            }
            TblLayout.Controls.Add(GridView, 0, 0);

            this.name = FuncionesGenerales.descargarJsonNombreMantenimiento(nombreJson);
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

            this.Load += RumControlGeneralLineas_Load;
        }

        private void RumControlGeneralLineas_Load(object sender, EventArgs e)
        {
            
            ObtencionTotalRegistros();
            //RefreshData(0);
            llenarGrid();

        }
        protected override void ElegirGrid()
        {
            //César: Pongo un false siempre porque gridview es mas comodo de manejar ya líneas no está pensado para virtualGrid.
            if ( false && CantidadRegistros > 10000)
            {
                ObtencionTotalRegistros();

                TblLayout.Controls.Add(virtualGridControl, 0, 0);

            }
            else
            {
                llenarGrid();
                TblLayout.Controls.Clear();
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
            CantidadRegistros = FuncionesGenerales.CargarEsquemaYSacarCount(nombreJsonLocal, "", ref _lstEsquemaTabla);
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + CantidadRegistros;
        }
        #endregion

        #region Eventos Propios               

        protected override void getGridViewRow()
        {

            if (TblLayout.GetControlFromPosition(0, 0) != null)
                _selectedRow = FuncionesGenerales.getRowGridView(TblLayout.GetControlFromPosition(0, 0));
            return;
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
            
        }

        public override void llenarGrid()
        {
            if (_lstEsquemaTabla.Count != 0)
            {
                try
                {

                    base.llenarGrid();
                    //Business.GetReservasHistoricoCantidad(ref _lstEsquemaTabla);
                    DataTable dt = FuncionesGenerales.descargarJsonDatos(nombreJsonLocal, FiltroExterno);
                    if (dt.Rows.Count == 0)
                    {
                        dt.Rows.Add(dt.NewRow());
                    }
                    GridView.DataSource = dt;
                    this._esquemGrid = FuncionesGenerales.descargarJsonEsquemaGridAdjunto(nombreJsonLocal);
                    foreach (var column in GridView.Columns)
                    {
                        column.Name = _lstEsquemaTabla[column.Index].Nombre;
                        column.FieldName = _lstEsquemaTabla[column.Index].Etiqueta;
                    }

                    this.GridView.Refresh();
                    this.GridView.Update();
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

            this.GridView.DataSource = FuncionesGenerales.descargarJsonDatos(nombreJsonLocal, FiltroExterno);
            //ElegirEstilo();
            this.virtualGridControl.MasterViewInfo.IsWaiting = false;
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
            return FuncionesGenerales.jsonInsert(nombreJsonLocal, selectedRow);
        }

        //Acción para editar un Pedido
        protected override AckResponse EditData(dynamic rowNueva)
        {
            return FuncionesGenerales.jsonUpdate(nombreJsonLocal, rowNueva, FuncionesGenerales.ComponerJson(_lstEsquemaTabla, _selectedRow));
        }

        //Acción para eliminar un Pedido
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return FuncionesGenerales.jsonDelete(nombreJsonLocal, FuncionesGenerales.ComponerJson(_lstEsquemaTabla, _selectedRow));
        }

        #endregion
    }
}
using Newtonsoft.Json.Linq;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class EstadoMaquinaControl : BaseGridControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        RadColorDialog dialog = new RadColorDialog();
        #region Variables
        List<dynamic> data = new List<dynamic>();

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        #endregion
        public EstadoMaquinaControl()
        {
            try
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
                GridView.ReadOnly = true;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                GridView.BestFitColumns();
                this.GridView.ContextMenuOpening += GridView_ContextMenuOpening;
                this.GridView.BeginEditMode = RadGridViewBeginEditMode.BeginEditProgrammatically;
                GridView.EnableGrouping = false;
                ColorEstadoMaquina();
                if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {
                    name = "Estado Maquina";
                }
                else
                {
                    name = "Machine Status";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }
        #region Funciones Auxiliares

        //Eventos Asíncronos y Síncronos
        private void InicializarEventos()
        {
            //Indicamos que las llamadas, para la recogida de datos, serán Asíncronas            
            this.CallbackCantidad = new Action<int>(query =>
            {
                //TbCantidadRegistrosControl.Text = query.ToString();
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
            this.virtualGridControl.SelectionChanged += VirtualGridControl_SelectionChanged;
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
            //Obtenemos, asíncronamente, la cantidad total de registros de Agencias, así como su esquema de datos, para poder preparar el Grid                        
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            CantidadRegistros = Business.GetEstadoMaquinaControlCantidad(ref _lstEsquemaTabla);
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + CantidadRegistros.ToString();
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
                //btnExportacion.Enabled = false;
                RefreshData(0);
                ElegirEstilo();


            }
            else if (TblLayout.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {

                if (numRegistrosFiltrados > 100000)
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ExcesoRegistros));
                }
                else
                {
                    if (numRegistrosFiltrados > 50000)
                    {
                        MessageBox.Show(Lenguaje.traduce("Aviso:" + strings.AvisoRegistros));
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
                    ColorEstadoMaquina();
                }
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Error inesperado"));
            }

        }

        //Selección de un registro del grid
        private void VirtualGridControl_SelectionChanged(object sender, EventArgs e)
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
                //btnExportacion.Enabled = false;
                virtualGridControl.Dock = DockStyle.Fill;
            }
            else
            {
                llenarGrid();
                TblLayout.Controls.Add(GridView, 0, 0);
                TblLayout.SetColumnSpan(GridView, 9);
                GridView.Dock = System.Windows.Forms.DockStyle.Fill;
                //btnExportacion.Enabled = true;
            }
        }
        protected override void getGridViewRow()
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
        }
        public override void llenarGrid()
        {
            if (_lstEsquemaTabla.Count != 0)
            {
                try
                {
                    base.llenarGrid();
                    Business.GetEstadoMaquinaControlCantidad(ref _lstEsquemaTabla);
                    GridView.DataSource = Business.GetEstadoMaquinaControlDatosGridView(_lstEsquemaTabla);
                    foreach (var column in GridView.Columns)
                    {
                            column.Name = _lstEsquemaTabla[column.Index].Nombre;
                            column.FieldName = _lstEsquemaTabla[column.Index].Etiqueta;

                    }
                    ElegirEstilo();
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

        //Función que, asíncronamente, consulta los datos de Agencias de la página seleccionada en el Grid
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
                sortExpression += "AGEN." + item.PropertyName;
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
                sortExpression = "IDMAQUINAESTADO ASC";
            }

            if (filterExpression != string.Empty)
            {
                filterExpression = " WHERE " + filterExpression;
                numRegistrosFiltrados = Business.GetEstadoMaquinaControlRegistrosFiltrados(filterExpression);
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;

            }
            else
            {
                numRegistrosFiltrados = Business.GetEstadoMaquinaControlRegistrosFiltrados("");
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;
            }


            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetEstadoMaquinaControlDatos(sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
            ElegirEstilo();
            ColorEstadoMaquina();
        }

        //Evento que pinta, celda a celda, los datos del Grid
        protected override void RadVirtualGrid_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
        {
            //base.RadVirtualGrid_CellValueNeeded(sender, e);

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
        private void gridViewFilterChanged_Event(object sender, EventArgs e)
        {
            if (GridView.FilterDescriptors.Expression != null && GridView.FilterDescriptors.Expression != string.Empty)
            {
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetEstadoMaquinaControlRegistrosFiltrados(" WHERE " + GridView.FilterDescriptors.Expression).ToString();
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
        //Acción para crear una Agencia
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaEstadoMaquinaControl(_lstEsquemaTabla, selectedRow);
        }

        //Acción para editar una Agencia
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarEstadoMaquinaControl(_lstEsquemaTabla, selectedRow);
        }

        //Acción para eliminar una Agencia
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarEstadoMaquinaControl(_lstEsquemaTabla, selectedRow);
        }

        #endregion
        #region Color Estado
        private void GridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
                if (this.GridView.CurrentRow is GridViewDataRowInfo)
                {
                    if (CultureInfo.CurrentUICulture.Name == "es-ES")
                    {
                        if (GridView.CurrentColumn.Name == "IDMAQUINAESTADO")
                        {
                            RadDropDownMenu menu = new RadDropDownMenu();
                            RadMenuItem menuItem = new RadMenuItem("Asignar nuevo color");
                            menuItem.Click += new EventHandler(menu_ItemClicked);
                            menu.Items.Add(menuItem);
                            e.ContextMenu = menu;
                        }
                    }
                    else
                    {
                        if (GridView.CurrentColumn.Name == "IDMAQUINAESTADO")
                        {
                            RadDropDownMenu menu = new RadDropDownMenu();
                            RadMenuItem menuItem = new RadMenuItem("New Colour");
                            menuItem.Click += new EventHandler(menu_ItemClicked);
                            menu.Items.Add(menuItem);
                            e.ContextMenu = menu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void menu_ItemClicked(object sender, EventArgs e)
        {
            try
            {
                string color = SetCellBackground();
                GridView.CurrentCell.BackColor = dialog.SelectedColor;
                string estado = GridView.CurrentCell.Value.ToString();
                XmlReaderPropio.setColorMaquina(color, estado);
                Reload();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void Reload()
        {
            try
            {
                Business.GetEstadoMaquinaCantidad(ref _lstEsquemaTabla);
                GridView.DataSource = Business.GetEstadoMaquinaDatosGridView(_lstEsquemaTabla);
                GridView.ReadOnly = true;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                GridView.BestFitColumns();
                this.GridView.ContextMenuOpening += GridView_ContextMenuOpening;
                this.GridView.BeginEditMode = RadGridViewBeginEditMode.BeginEditProgrammatically;
                GridView.EnableGrouping = false;
                ColorEstadoMaquina();
                if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {
                    name = "Estado Maquina";
                }
                else
                {
                    name = "Machine Status";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private string SetCellBackground()
        {
            int row = (int)this.GridView.CurrentCell.RowInfo.Index;
            int column = (int)this.GridView.CurrentCell.ColumnInfo.Index;
            string valorColor = "";
            ((RadForm)dialog.ColorDialogForm).ThemeName = "CAMBIO COLOR";
            dialog.SelectedColor = this.GridView.Rows[row].Cells[column].Style.BackColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                GridView.Rows[row].Cells[column].Style.BackColor = dialog.SelectedColor;
                GridView.CurrentCell.BackColor = dialog.SelectedColor;
                valorColor = dialog.SelectedColor.ToArgb().ToString();
            }
            return valorColor;
        }
        public override void ElegirEstilo()
        {
            base.ElegirEstilo();
            ColorEstadoMaquina();
        }
        private void ColorEstadoMaquina()
        {
            try
            {
                ConditionalFormattingObject obj1 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "AC", "", false);
                obj1.CellBackColor = XmlReaderPropio.getColorMaquina("AC");
                ConditionalFormattingObject obj2 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "IN", "", false);
                obj2.CellBackColor = XmlReaderPropio.getColorMaquina("IN");
                this.GridView.MasterTemplate.Columns["IDMAQUINAESTADO"].ConditionalFormattingObjectList.Add(obj1);
                this.GridView.MasterTemplate.Columns["IDMAQUINAESTADO"].ConditionalFormattingObjectList.Add(obj2);

            }
            catch (Exception ex)
            {
                //ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        #endregion        public const int WM_NCLBUTTONDOWN = 0xA1;
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
    }
}
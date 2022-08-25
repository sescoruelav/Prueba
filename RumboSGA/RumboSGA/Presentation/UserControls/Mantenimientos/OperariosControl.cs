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
using RumboSGA.Presentation.Herramientas.Ventanas;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls;
using RumboSGAManager.Model.DataContext;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class OperariosControl : BaseGridControl
    {
        #region Variables y Propiedades

        private List<dynamic> data = new List<dynamic>();
        private string nombreTablaJson = "Operarios";
        private string nombreCampoIdJson = "op.IDOPERARIO";

        private string nombreTablaJsonUsuarios = "Usuarios";
        private string nombreCampoIdUsuario = "IDUSUARIO";

        protected List<TableScheme> _lstEsquemaTablaGrupos = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaOperarios = new List<TableScheme>();
        private string nombreOperario;
        private string apellidos;
        private string modoPant;
        private string estado;
        private string volumen;
        private string fuerza;
        private string ultimaMaquina;
        private string ultimaModificacion;
        private string sensibilidad;
        private string permisoRegularizacion;
        private string idOperario;
        private string idUsuario;
        private string nombreUsuario;

        //Para la modificación, eliminación y clonación de campos vamos a apuntar a la tabla de operarios.

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MasterGridViewTemplate MasterTemplate { get; }

        #endregion Variables y Propiedades

        #region Constructor

        public OperariosControl()
        {
            InitializeComponent();
            InicializarEventos();
            ObtencionTotalRegistros();
            ElegirGrid();
            //ElegirEstilo();
            //RefreshData(0);
            if (GridView.Rows.Count != 0)
            {
                GridView.Rows[0].IsSelected = false;
            }
            //ColorEstadoOperarios();
            //QuitarColumnas();
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
            this.virtualGridControl.SelectionChanged += GridControl_SelectionChanged;
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

            //Sustituido por el getDatosCantidad que es un poco mas universal así no tengo que hacer una función concreta de get datos
            //CantidadRegistros = Business.GetOperariosCantidad(ref _lstEsquemaTabla);
            CantidadRegistros = Business.GetDatosCantidad(ref _lstEsquemaTabla, nombreTablaJson);
            //lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" +CantidadRegistros;
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
                }
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Error inesperado"));
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

        #endregion Eventos Propios

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
                    GridView.DataSource = Business.GetDatosGridView(_lstEsquemaTabla, nombreTablaJson, nombreCampoIdJson);
                    ColorEstadoOperarios();
                    QuitarColumnas();
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
            ElegirEstilo();
            return;
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
                sortExpression += "OPERARIO." + item.PropertyName;
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
                sortExpression = "IDOPERARIO ASC";
            }
            if (filterExpression != null && filterExpression != string.Empty)
            {
                filterExpression = " WHERE " + filterExpression;
                numRegistrosFiltrados = Business.GetDatosRegistrosFiltrados(filterExpression, nombreTablaJson);
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;
            }
            else
            {
                numRegistrosFiltrados = Business.GetDatosRegistrosFiltrados("", nombreTablaJson);
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;
            }

            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetDatos(nombreTablaJson, "OP", "OP", sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
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

        private void gridViewFilterChanged_Event(object sender, EventArgs e)
        {
        }

        private void ColorEstadoOperarios()
        {
            if (GridView.Columns[Lenguaje.traduce(strings.Estado)] != null)
            {
                try
                {
                    ConditionalFormattingObject obj1 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "AC", "", false);
                    obj1.CellBackColor = XmlReaderPropio.getColorOperarios("AC");
                    ConditionalFormattingObject obj2 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "IN", "", false);
                    obj2.CellBackColor = XmlReaderPropio.getColorOperarios("IN");
                    this.GridView.Columns[Lenguaje.traduce(strings.Estado)].ConditionalFormattingObjectList.Add(obj1);
                    this.GridView.Columns[Lenguaje.traduce(strings.Estado)].ConditionalFormattingObjectList.Add(obj2);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
        }

        private void QuitarColumnas()
        {
            if (GridView.Columns["Password"] != null)
            {
                GridView.Columns["Password"].IsVisible = false;
            }
            if (GridView.Columns["RowNum"] != null)
            {
                GridView.Columns["RowNum"].IsVisible = false;
            }
        }

        protected override void RadGridView1_DoubleClick(object sender, EventArgs e)
        {
            EditarOperario();
        }

        private void radGridView1_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadDropDownListEditor editor = this.GridView.ActiveEditor as RadDropDownListEditor;
            if (editor != null)
            {
                ((RadDropDownListEditorElement)((RadDropDownListEditor)this.GridView.ActiveEditor).EditorElement).RightToLeft = true;
            }
        }

        public void ExportarExcel()
        {
            GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(this.GridView);
            spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
            spreadExporter.ExportVisualSettings = true;
            //spreadExporter.ExportFormat = SpreadExportFormat.Xlsx;
            spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;
            bool openExportFile = false;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "";
            dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                spreadExporter.RunExport(dialog.FileName, new SpreadStreamExportRenderer());
                DialogResult dr = RadMessageBox.Show(confirmacion,
                Lenguaje.traduce(strings.ExportarExcel), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    openExportFile = true;
                }
                ElegirGrid();
                //RefreshData(0);
            }
            if (openExportFile)
            {
                try
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
                catch (Exception ex)
                {
                    string message = String.Format(strings.ExportarError + "\nError message: {0}", ex.Message);
                    RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        public override void newButton_Click(object sender, EventArgs e)
        {
            CrearNuevoOperario();
        }

        public override void cloneButton_Click(object sender, EventArgs e)
        {
            ClonarOperario();
        }

        public override void deleteButton_Click(object sender, EventArgs e)
        {
            EliminarOperario();
        }

        public void RefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón refresh " + DateTime.Now);
                GridView.EnablePaging = false;
                GridView.Columns.Clear();
                ElegirGrid();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void QuitarFiltros_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón filtro " + DateTime.Now);
                if (GridView.IsInEditMode)
                {
                    GridView.EndEdit();
                }
                GridView.FilterDescriptors.Clear();
                GridView.GroupDescriptors.Clear();
                GridView.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void AgregarGrupo()
        {
            string idOperario;
            GridView.CurrentColumn = GridView.Columns[0];
            idOperario = GridView.CurrentCell.Value.ToString();
            AgregarGrupoOperario AgregarGrupo = new AgregarGrupoOperario(idOperario);
            AgregarGrupo.ShowDialog();
        }

        public void AgregarUsuario()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value != null)
            {
                idOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value.ToString();
            }
            AgregarUsuarioOperario AgregarUsuarioOperario = new AgregarUsuarioOperario(idOperario);
            AgregarUsuarioOperario.ShowDialog();
            ElegirGrid();
        }

        public void CambiarPassword()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("IDUsuario")].Value != null)
            {
                idUsuario = GridView.CurrentRow.Cells[Lenguaje.traduce("IDUsuario")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value != null)
            {
                idOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value.ToString();
            }
            CambiarPassword CambiarPassword = new CambiarPassword(idUsuario, idOperario);
            CambiarPassword.ShowDialog();
        }

        public void CambiarGrupo()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("IDUsuario")].Value != null)
            {
                idUsuario = GridView.CurrentRow.Cells[Lenguaje.traduce("IDUsuario")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value != null)
            {
                idOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Usuario")].Value != null)
            {
                nombreUsuario = GridView.CurrentRow.Cells[Lenguaje.traduce("Usuario")].Value.ToString();
            }
            if (nombreUsuario != "")
            {
                EditarGrupoOperario EditarGrupoOperario = new EditarGrupoOperario(idUsuario, idOperario, nombreUsuario);
                EditarGrupoOperario.ShowDialog();
                ElegirGrid();
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Por favor, cree un usuario para poder asignar un grupo nuevo al operario seleccionado"));
            }
        }

        public void CrearNuevoOperario()
        {
            CrearOperario CrearOperario = new CrearOperario();
            CrearOperario.ShowDialog();
            ElegirGrid();
        }

        public void ClonarOperario()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre")].Value != null)
            {
                nombreOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Apellidos")].Value != null)
            {
                apellidos = GridView.CurrentRow.Cells[Lenguaje.traduce("Apellidos")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("modoPant")].Value != null)
            {
                modoPant = GridView.CurrentRow.Cells[Lenguaje.traduce("modoPant")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value != null)
            {
                estado = GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Volumen")].Value != null)
            {
                volumen = GridView.CurrentRow.Cells[Lenguaje.traduce("Volumen")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Fuerza")].Value != null)
            {
                fuerza = GridView.CurrentRow.Cells[Lenguaje.traduce("Fuerza")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaMaquina")].Value != null)
            {
                ultimaMaquina = GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaMaquina")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaModificacion")].Value != null)
            {
                ultimaModificacion = GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaModificacion")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Sensibilidad")].Value != null)
            {
                sensibilidad = GridView.CurrentRow.Cells[Lenguaje.traduce("Sensibilidad")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("PermisoRegularizacion")].Value != null)
            {
                permisoRegularizacion = GridView.CurrentRow.Cells[Lenguaje.traduce("PermisoRegularizacion")].Value.ToString();
            }

            CrearOperario CrearOperario = new CrearOperario(nombreOperario, apellidos, modoPant, estado, volumen, fuerza, ultimaMaquina, ultimaModificacion, sensibilidad, permisoRegularizacion);
            CrearOperario.ShowDialog();
            ElegirGrid();
        }

        private void EditarOperario()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value != null)
            {
                idOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("ID")].Value.ToString();
            }

            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre")].Value != null)
            {
                nombreOperario = GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Apellidos")].Value != null)
            {
                apellidos = GridView.CurrentRow.Cells[Lenguaje.traduce("Apellidos")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("modoPant")].Value != null)
            {
                modoPant = GridView.CurrentRow.Cells[Lenguaje.traduce("modoPant")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value != null)
            {
                estado = GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Volumen")].Value != null)
            {
                volumen = GridView.CurrentRow.Cells[Lenguaje.traduce("Volumen")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Fuerza")].Value != null)
            {
                fuerza = GridView.CurrentRow.Cells[Lenguaje.traduce("Fuerza")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaMaquina")].Value != null)
            {
                ultimaMaquina = GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaMaquina")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaModificacion")].Value != null)
            {
                ultimaModificacion = GridView.CurrentRow.Cells[Lenguaje.traduce("UltimaModificacion")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Sensibilidad")].Value != null)
            {
                sensibilidad = GridView.CurrentRow.Cells[Lenguaje.traduce("Sensibilidad")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("PermisoRegularizacion")].Value != null)
            {
                permisoRegularizacion = GridView.CurrentRow.Cells[Lenguaje.traduce("PermisoRegularizacion")].Value.ToString();
            }
            EditarOperario EditarOperario = new EditarOperario(idOperario, nombreOperario, apellidos, modoPant, estado, volumen, fuerza, ultimaMaquina, ultimaModificacion, sensibilidad, permisoRegularizacion);
            EditarOperario.ShowDialog();
            ElegirGrid();
        }

        public void EliminarOperario()
        {
            DialogResult dr = RadMessageBox.Show(Lenguaje.traduce("Seguro que deseas eliminar a este operario?"),
               Lenguaje.traduce("Eliminar Operario"), MessageBoxButtons.YesNo, RadMessageIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string idOperario;

                GridView.CurrentColumn = GridView.Columns[0];
                idOperario = GridView.CurrentCell.Value.ToString();
                if (GridView.CurrentRow.Cells[Lenguaje.traduce("IDUsuario")].Value != null)
                {
                    idUsuario = GridView.CurrentRow.Cells[Lenguaje.traduce("IDUsuario")].Value.ToString();
                }
                if (idOperario != "")
                {
                    DataAccess.EliminarOperario(idOperario, idUsuario);
                    ElegirGrid();
                }
            }
        }

        //Acción para crear un Lote
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaOperarios(_lstEsquemaTabla, selectedRow);
        }

        //Acción para editar un Lote
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarOperarios(_lstEsquemaTabla, selectedRow);
        }

        //Acción para eliminar un Lote
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarOperarios(_lstEsquemaTabla, selectedRow);
        }

        #endregion Eventos Sobreescritos de la clase padre BaseGridControl
    }
}
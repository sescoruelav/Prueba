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
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.Export;
using Telerik.WinControls;
using RumboSGAManager.Model.DataContext;
using System.Linq;
using Newtonsoft.Json;
using System.Data;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.Presentation.Herramientas.Ventanas;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class RecursosTareaControl : BaseGridControl
    {
        #region Variables y Propiedades
        List<dynamic> data = new List<dynamic>();        
        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GridViewTemplate gtJerarquicoTareas = new GridViewTemplate();

        private static string nombreJsonRecursos = "Recursos";
        private static string nombreTablaJsonRecurso = "TBLRECURSOS";
        private static string nombreCampoIdJson = "IDRECURSO";

        private static string nombreJsonJerarquico = "RecursosJerarquicoTarea";
        private static string nombreCampoIdJsonJerarquico = "IDRECURSO";

        private static string colTareas;
        private static string colRecurso;
        private string descripcion;
        private string nombreImpresora;
        private string estado;
        private string maxTareas;
        private string fechaLogin;
        private string fechaLogout;
        private string ubicacionActual;
        private string ubicacionPrincipal;
        private string idRecurso;
        private string id;


        #endregion
        #region Constructor
        public RecursosTareaControl()
        {
            InicializarTraduccion();
            InitializeComponent();
            InicializarEventos();
            ObtencionTotalRegistros();
            ElegirGrid();
            this.name = "RecursosTareaControl";
            //ElegirEstilo();
            CreateChildTemplateJerarquicoTareas();
            RefreshData(0);
            quitarUltimaColumnaRowNumber();
            this.GridView.EnableFiltering = true;
            this.GridView.MasterTemplate.EnableFiltering = true;


            GridView.RowSourceNeeded += Tareas_RowSourceNeeded;

            
            if (GridView.Rows.Count != 0)
            {
                //GridView.Rows[0].IsSelected = false;
            }
        }
        #endregion
        #region Funciones Auxiliares


        private void CreateChildTemplateJerarquicoTareas()
        {
            log.Info("Inicio CreateChildTemplateJerarquicoTareasRecursos RecursosTareaControl" + DateTime.Now.ToString("HH:mm:ss:ffff"));

            //LINEAS
            string json1 = DataAccess.LoadJson("RecursosJerarquicoTarea");
            JArray js = JArray.Parse(json1);
            string jsonEsquema = js.First()["Scheme"].ToString();
            List<TableScheme> esquema1 = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

            gtJerarquicoTareas = new GridViewTemplate();
            gtJerarquicoTareas.Caption = colTareas;
            for (int i = 0; i < esquema1.Count; i++)
            {
                if (esquema1[i].Etiqueta != string.Empty && esquema1[i].Etiqueta != null)
                {
                    gtJerarquicoTareas.Columns.Add(esquema1[i].Etiqueta);
                }
            }

            gtJerarquicoTareas.BestFitColumns();
            this.GridView.Templates.Add(gtJerarquicoTareas);
            gtJerarquicoTareas.HierarchyDataProvider = new GridViewEventDataProvider(gtJerarquicoTareas);
            log.Info("Fin CreateChildTemplateJerarquicoTareasRecursos RecursosTareaControl" + DateTime.Now.ToString("HH:mm:ss:ffff"));

        }

        private void quitarUltimaColumnaRowNumber()
        {
            this.GridView.Columns[this.GridView.ColumnCount - 1].IsVisible = false;
        }

        

        private void Tareas_RowSourceNeeded(object sender, GridViewRowSourceNeededEventArgs e)
        {
            try
            {
                string valor = string.Empty;
                DataRowView rowView = e.ParentRow.DataBoundItem as DataRowView;
                if (e.Template.Caption == colTareas)
                {
                    valor = e.ParentRow.Cells[colRecurso].Value.ToString();
                    DataTable lineas = Business.GetRecursosJerarquicoTarea(valor, nombreJsonJerarquico, nombreCampoIdJsonJerarquico);

                    for (int a = 0; a < lineas.Rows.Count; a++)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();

                        for (int i = 0; i < lineas.Columns.Count; i++)
                        {
                            row.Cells[lineas.Columns[i].ColumnName].Value = lineas.Rows[a][row.Cells[lineas.Columns[i].ColumnName].ColumnInfo.HeaderText].ToString();
                        }
                        e.SourceCollection.Add(row);

                    }
                    //templateEntradas.Columns.Add(checkBoxColumnLineas);

                    //templateEntradas.Columns.Move(checkBoxColumnLineas.Index, 0);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }


        private void InicializarTraduccion()
        {
            colTareas = Lenguaje.traduce("Tareas");
            

            colRecurso = Lenguaje.traduce("Recurso");
        }
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
            //Obtenemos, asíncronamente, la cantidad total de registros, así como su esquema de datos, para poder preparar el Grid                        
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            CantidadRegistros = Business.GetDatosCantidad(ref _lstEsquemaTabla, nombreJsonRecursos);
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + CantidadRegistros;

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
        #endregion
        #region Eventos Sobreescritos de la clase padre BaseGridControl 
        protected override void ElegirGrid()
        {
            //Cesar por tema de jerarquicos voy a hacer que siempre sea un gridview
            if (CantidadRegistros > 10000 && true == false)
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
                GridView.BestFitColumns(BestFitColumnMode.DisplayedCells);
            }
            QuitarColumnas();
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

        private void ColorEstadoRecursos()
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

        protected override void RadGridView1_DoubleClick(object sender, EventArgs e)
        {
            EditarRecurso();
        }

        public override void llenarGrid()
        {
            if (_lstEsquemaTabla.Count != 0)
            {
                try
                {
                    base.llenarGrid();
                    Business.GetDatosCantidad(ref _lstEsquemaTabla,nombreJsonRecursos);
                    GridView.DataSource = Business.GetDatosGridView(_lstEsquemaTabla,nombreJsonRecursos, nombreCampoIdJson);
                    
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
                pagFinal = _K_PAGINACION;
            }
            else
            {
                pagInicial = _K_PAGINACION * (pageIndex - 1) + 1;
                pagFinal = (_K_PAGINACION * pageIndex);
            }

            string sortExpression = nombreCampoIdJson+" ASC";

            numRegistrosFiltrados = Business.GetArticulosRegistrosFiltrados("");
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;


            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            llenarGrid();
            //ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetDatos(nombreJsonRecursos, "","", sortExpression, "", pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
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
        private void gridViewFilterChanged_Event(object sender, EventArgs e)
        {
            if (GridView.FilterDescriptors.Expression != null && GridView.FilterDescriptors.Expression != string.Empty)
            {
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetDatosRegistrosFiltrados(" WHERE " + Utilidades.ConvertirFiltroValorBbdd(GridView.FilterDescriptors.Expression,base._lstEsquemaTabla,1,0),nombreJsonRecursos).ToString();

            }
        }

        public void crearTareaForm()
        {
            if(this.GridView.SelectedRows.Count>1 || this.GridView.SelectedRows.Count == 0)
            {
                return;
            }
            FormularioCrearTarea fct = new FormularioCrearTarea(int.Parse(this.GridView.SelectedRows[0].Cells[0].ToString()),
                this.GridView.SelectedRows[0].Cells[1].ToString(), false);
        }

        private void QuitarColumnas()
        {
            if (GridView.Columns["RowNum"] != null)
            {
                GridView.Columns["RowNum"].IsVisible = false;
            }

        }

        public override void newButton_Click(object sender, EventArgs e)
        {
            NuevoRecurso();
            /*if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
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
            }*/
        }
        public override void cloneButton_Click(object sender, EventArgs e)
        {
            ClonarRecurso();
            /*
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
            */
        }
        public override void deleteButton_Click(object sender, EventArgs e)
        {
            EliminarRecurso();
            /*comentado temporalmente para evitar que salte error
            if (true) return;
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
            */
        }

        private void NuevoRecurso()
        {
            CrearRecursoForm CrearRecurso = new CrearRecursoForm();
            CrearRecurso.ShowDialog();
            ElegirGrid();
        }

        public void ClonarRecurso()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Descripcion")].Value != null)
            {
                descripcion = GridView.CurrentRow.Cells[Lenguaje.traduce("Descripcion")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre Impresora")].Value != null)
            {
                nombreImpresora = GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre Impresora")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value != null)
            {
                estado = GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Max tareas")].Value != null)
            {
                maxTareas = GridView.CurrentRow.Cells[Lenguaje.traduce("Max tareas")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha login")].Value != null)
            {
                fechaLogin = GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha login")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha logout")].Value != null)
            {
                fechaLogout = GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha logout")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Actual")].Value != null)
            {
                ubicacionActual = GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Actual")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Principal")].Value != null)
            {
                ubicacionPrincipal = GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Principal")].Value.ToString();
            }
            
                CrearRecursoForm CrearRecurso = new CrearRecursoForm(descripcion, nombreImpresora, estado, maxTareas, fechaLogin, fechaLogout, ubicacionActual, ubicacionPrincipal);
            CrearRecurso.ShowDialog();
            ElegirGrid();
        }

        private void EditarRecurso()
        {
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Recurso")].Value != null)
            {
                idRecurso = GridView.CurrentRow.Cells[Lenguaje.traduce("Recurso")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Descripcion recurso")].Value != null)
            {
                descripcion = GridView.CurrentRow.Cells[Lenguaje.traduce("Descripcion recurso")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre Impresora")].Value != null)
            {
                nombreImpresora = GridView.CurrentRow.Cells[Lenguaje.traduce("Nombre Impresora")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value != null)
            {
                estado = GridView.CurrentRow.Cells[Lenguaje.traduce("Estado")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Max tareas")].Value != null)
            {
                maxTareas = GridView.CurrentRow.Cells[Lenguaje.traduce("Max tareas")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha login")].Value != null)
            {
                fechaLogin = GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha login")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha logout")].Value != null)
            {
                fechaLogout = GridView.CurrentRow.Cells[Lenguaje.traduce("Fecha logout")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Actual")].Value != null)
            {
                ubicacionActual = GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Actual")].Value.ToString();
            }
            if (GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Principal")].Value != null)
            {
                ubicacionPrincipal = GridView.CurrentRow.Cells[Lenguaje.traduce("Ubicacion Principal")].Value.ToString();
            }

            CrearRecursoForm EditarRecurso = new CrearRecursoForm(idRecurso, descripcion, nombreImpresora, estado, maxTareas, fechaLogin, fechaLogout, ubicacionActual, ubicacionPrincipal);
            EditarRecurso.ShowDialog();
            ElegirGrid();
        }
        private void EliminarRecurso()
        {
            DialogResult dr = RadMessageBox.Show(Lenguaje.traduce("Seguro que deseas eliminar este recurso?"),
               Lenguaje.traduce("Eliminar Recurso"), MessageBoxButtons.YesNo, RadMessageIcon.Question);
            if (dr == DialogResult.Yes)
            {
                
                if (GridView.CurrentRow.Cells[Lenguaje.traduce("Recurso")].Value != null)
                {
                    idRecurso = GridView.CurrentRow.Cells[Lenguaje.traduce("Recurso")].Value.ToString();
                }
               
                if (idRecurso != "")
                {
                    DataAccess.EliminarRecurso(idRecurso);
                    ElegirGrid();
                }
            }
        }
        //Acción para crear un ReservaHistorico
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaRecursosTarea(_lstEsquemaTabla, selectedRow);            
        }
        //Acción para editar un ReservaHistorico
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarRecursosTarea(_lstEsquemaTabla, selectedRow);
        }
        //Acción para eliminar un ReservasHistorico
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarRecursosTarea(_lstEsquemaTabla, selectedRow);            
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
        #endregion
    }
}

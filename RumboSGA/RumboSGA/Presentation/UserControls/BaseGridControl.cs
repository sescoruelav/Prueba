using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGA.Properties;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using System.Drawing;
using System.Reflection;
using Telerik.WinControls.Analytics;
using RumboSGAManager;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.Export;
using RumboSGAManager.Model.Security;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model;
using Rumbo.Core.Herramientas;
using Telerik.WinControls.VirtualKeyboard;

namespace RumboSGA.Presentation.UserControls
{
    public partial class BaseGridControl : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables, Constantes y Propiedades

        private const float K_RATIO_AMPLIAR_VENTANA = 0.5F;
        private const int ESTILO_LOCAL = 1;
        private const int ESTILO_GLOBAL = 0;
        public int idFormulario;
        private int _cantidadRegistros = 0;

        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        public int numRegistrosFiltrados;

        public TableLayoutPanel TblLayout
        {
            get { return this.tableLayoutPanel1; }
        }

        public const int _K_PAGINACION = 20;

        public RadVirtualGrid virtualGridControl
        {
            get { return this.virtualGrid; }
        }

        public RadGridView GridView { get => gridView; set => gridView = value; }
        protected List<TableScheme> _lstEsquemaTabla = new List<TableScheme>();
        public List<TableScheme> _lstEsquemaTablaPadre = null;
        public dynamic _selectedRow;
        protected GridScheme _esquemGrid;
        private RadGridView gridView;
        public string FiltroExterno;
        public Hashtable diccParamNuevo = new Hashtable();
        public string name;
        public static string nombreJson;
        public static string filtroInicialVolatil;

        public RadContextMenu contextMenu;

        #endregion Variables, Constantes y Propiedades

        #region Constructor

        public BaseGridControl()
        {
            InitializeComponent();
            //Inicialización de propiedades del Grid
            inicializarGrids();
            inicializarComboTamañofuente();
            contextMenu = new RadContextMenu();
            RadMenuItem menuItem1 = new RadMenuItem(Lenguaje.traduce("Nuevo"));
            menuItem1.Click += new EventHandler(newButton_Click);
            RadMenuItem menuItem2 = new RadMenuItem(Lenguaje.traduce("Eliminar"));
            menuItem2.Click += new EventHandler(deleteButton_Click);
            RadMenuItem menuItem3 = new RadMenuItem(Lenguaje.traduce("Clonar"));
            menuItem3.Click += new EventHandler(cloneButton_Click);
            contextMenu.Items.Add(menuItem1);
            contextMenu.Items.Add(menuItem2);
            contextMenu.Items.Add(menuItem3);
            tableLayoutPanel1.SetColumnSpan(virtualGrid, 10);
        }

        public BaseGridControl(string nombreJson) : this()
        {
            BaseGridControl.nombreJson = nombreJson;
        }

        private void inicializarGrids()
        {
            #region GridView

            GridView = new RadGridView();
            GridView.TableElement.BeginUpdate();
            GridView.BindingContext = new BindingContext();
            GridView.TableElement.EndUpdate(true);
            GridView.MasterTemplate.Refresh(null);
            GridView.TableElement.RowHeight = Persistencia.GridRowHeight;
            GridView.ReadOnly = true;
            GridView.MasterTemplate.EnableGrouping = true;
            GridView.ShowGroupPanel = true;
            GridView.MasterTemplate.AutoExpandGroups = true;
            GridView.EnableHotTracking = true;
            GridView.MasterTemplate.AllowAddNewRow = false;
            GridView.MasterTemplate.EnableFiltering = true;
            GridView.MasterTemplate.AllowColumnResize = true;
            GridView.MasterTemplate.AllowMultiColumnSorting = true;
            GridView.AllowSearchRow = true;
            GridView.EnablePaging = false;
            GridView.PageSize = _K_PAGINACION;
            GridView.MasterView.TableSearchRow.SearchDelay = 2000;
            GridView.PageChanged += gridView_CambioPagina;
            GridView.SortChanged += radGridView1_SortChanged;
            GridView.CellFormatting += radGridView1_CellFormatting;
            GridView.ViewCellFormatting += radGridView1_CellFormattingWrapText;
            GridView.FilterChanged += RadGridView_FilterChanged;
            GridView.PageChanged += GridViewPageChanged;
            this.GridView.DoubleClick += RadGridView1_DoubleClick;
            gridView.ContextMenuOpening += radGridView1_ContextMenuOpening;
            GridView.MasterTemplate.ShowHeaderCellButtons = true;
            GridView.MasterTemplate.ShowFilteringRow = false;
            this.GridView.EnablePaging = false;

            #endregion GridView

            #region VirtualGrid

            this.virtualGrid.SelectionMode = VirtualGridSelectionMode.FullRowSelect;
            this.virtualGrid.AllowFiltering = true;
            this.virtualGrid.AllowAddNewRow = false;
            this.virtualGrid.AllowColumnResize = true;
            this.virtualGrid.AllowRowResize = true;
            this.virtualGridControl.BestFitColumns();
            this.virtualGrid.CellValueNeeded += RadVirtualGrid_CellValueNeeded;
            this.virtualGrid.EnablePaging = false;
            this.virtualGrid.PageSize = _K_PAGINACION;
            this.virtualGrid.VirtualGridElement.PageIndexChanging += VirtualGridElement_PageIndexChanging;
            this.virtualGrid.BeginEditMode = RadVirtualGridBeginEditMode.BeginEditProgrammatically;
            this.virtualGrid.AllowMultiColumnSorting = false;
            this.virtualGrid.SortChanged += RadGridView1_SortChanged;
            this.virtualGrid.FilterChanged += RadVirtualGrid_FilterChanged;
            this.virtualGrid.DoubleClick += RadGridView1_DoubleClick;
            this.virtualGrid.ContextMenuOpening += grid_ContextMenuOpening;
            this.virtualGrid.Font = new Font("Segoe UI", 8.25F);
            this.virtualGrid.TableElement.RowHeight = 40;
            this.virtualGrid.EnablePaging = false;

            #endregion VirtualGrid
        }

        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
                if (e.ContextMenuProvider != null && e.ContextMenuProvider is GridDataCellElement)
                    if ((e.ContextMenuProvider as GridDataCellElement).RowIndex > -1)
                        e.ContextMenu = contextMenu.DropDown;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al abrir el menu"));
                log.Error(ex);
            }
        }

        #endregion Constructor

        #region Eventos propios (Filtrado, Orden, Paginación, Obtención asíncrona de datos)

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex > -1 && e.ColumnIndex > -1)
            //    {
            //        e.CellElement.TextWrap = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            //}
        }

        private void radGridView1_CellFormattingWrapText(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            //try
            //{
            //    if (e.CellElement is GridHeaderCellElement)
            //    {
            //        e.CellElement.TextWrap = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            //}
        }

        protected virtual void radGridView1_SortChanged(object sender, Telerik.WinControls.UI.GridViewCollectionChangedEventArgs e)
        {
            try
            {
                if (virtualGrid.SortDescriptors.Expression != gridView.SortDescriptors.Expression)
                {
                    string sortExpression = this.gridView.SortDescriptors.Expression;
                    this.virtualGrid.SortDescriptors.Expression = sortExpression;
                    RefreshData(virtualGrid.PageIndex);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al cambiar los filtros"));
                log.Error(ex);
            }
        }

        protected virtual void RadVirtualGrid_FilterChanged(object sender, VirtualGridEventArgs e)
        {
            try
            {
                GridView.FilterChanged -= RadGridView_FilterChanged;
                GridView.FilterDescriptors.Clear();
                foreach (var item in virtualGrid.FilterDescriptors)
                {
                    var newDescriptor = new FilterDescriptor(item.PropertyName, item.Operator, item.Value);
                    GridView.FilterDescriptors.Add(newDescriptor);
                }
                GridView.FilterChanged += RadGridView_FilterChanged;
                RefreshData(virtualGrid.PageIndex);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al cambiar los filtros"));
                log.Error(ex);
            }
        }

        protected virtual void RadGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
                virtualGrid.FilterChanged -= RadVirtualGrid_FilterChanged;
                virtualGrid.FilterDescriptors.Clear();
                foreach (var item in GridView.FilterDescriptors)
                {
                    var newDescriptor = new FilterDescriptor(item.PropertyName, item.Operator, item.Value);
                    virtualGrid.FilterDescriptors.Add(item);
                }
                virtualGrid.FilterChanged += RadVirtualGrid_FilterChanged;
                RefreshData(virtualGrid.PageIndex);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al cambiar los filtros"));
                log.Error(ex);
            }
        }

        //ORDENADO de registros, cada vez que se ordene, recargará la página seleccionada
        protected virtual void RadGridView1_SortChanged(object sender, VirtualGridEventArgs e)
        {
            try
            {
                var pag = virtualGrid.PageIndex;
                if (GridView.SortDescriptors.Expression != virtualGrid.SortDescriptors.Expression)
                {
                    GridView.SortDescriptors.Expression = virtualGrid.SortDescriptors.Expression;
                    RefreshData(pag);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al cambiar los filtros"));
                log.Error(ex);
            }
        }

        //OBTENCION DE REGISTROS ASINCRONO. Función que ejecutará una tarea asíncronamente, como la carga de datos paginados del grid
        protected async void ExecuteQueryAsync<T>(Task<T> task, Action<T> callback)
        {
            var result = await task;
            callback(result);
        }

        //PAGINADO de registros. Función que Refrescará los datos de la página seleccionada del Grid
        protected virtual void VirtualGridElement_PageIndexChanging(object sender, VirtualGridPageChangingEventArgs e)
        {
            try
            {
                var pag = e.NewIndex;
                RefreshData(pag);
                GridView.MasterTemplate.MoveToPage(pag);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al cambiar los filtros");
                log.Error(ex);
            }
        }

        private void gridView_CambioPagina(object sender, EventArgs e)
        {
            virtualGrid.PageIndex = int.Parse(GridView.GridViewElement.PagingPanelElement.PageNumberTextBox.Text) - 1;
        }

        protected void grid_ContextMenuOpening(object sender, Telerik.WinControls.UI.VirtualGridContextMenuOpeningEventArgs e)
        {
            /*int cont = 0;
            for (int i = 0; i < e.ContextMenu.Items.Count; i++)
            {
                if (e.ContextMenu.Items[i].Text == "Delete Row" || e.ContextMenu.Items[i].Text == "Clear Value" || e.ContextMenu.Items[i].Text == "Edit" || e.ContextMenu.Items[i].Text == "Paste")
                {
                    // hide the Conditional Formatting option from the header row context menu
                    e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                    // hide the separator below the CF option
                    cont++;
                }
            }
            e.ContextMenu.Items[cont + 1].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;*/
            try
            {
                if (e.RowIndex > -1)
                    e.ContextMenu = contextMenu.DropDown;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al abrir el menu");
                log.Error(ex);
            }
        }

        public void FiltroInicial_Click(object sender, EventArgs e)
        {
            try
            {
                FiltroInicialForm fi = new FiltroInicialForm(nombreJson, Modos.BaseGridControl);
                fi.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "No se ha podido abrir el filtro inicial");
                log.Error(ex);
            }
        }

        #endregion Eventos propios (Filtrado, Orden, Paginación, Obtención asíncrona de datos)

        #region Eventos que serán Sobreescritos por clases hijas

        //Cantidad inicial de registros
        protected virtual void TbCantidadRegistros_TextChanged(object sender, EventArgs e)
        {
        }

        //Pintado del grid
        protected virtual void RadVirtualGrid_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
        {
        }

        public virtual void llenarGrid()
        {
            this.GridView.DataSource = null;
            ElegirEstilo();
        }

        protected virtual void ElegirGrid()
        {
        }

        //Carga de datos
        public virtual void RefreshData(int pageIndex)
        {
        }

        //Creación de un registro
        protected virtual AckResponse NewData(dynamic selectedRow)
        {
            return new AckResponse();
        }

        //Clonar Registro de un registro
        protected virtual AckResponse NewDataAndLines(dynamic selectedRow, bool bolCloneLines)
        {
            return new AckResponse();
        }

        //Edición de un registro
        protected virtual AckResponse EditData(dynamic selectedRow)
        {
            return new AckResponse();
        }

        //Eliminación de un registro
        protected virtual AckResponse DeleteData(dynamic selectedRow)
        {
            return new AckResponse();
        }

        #endregion Eventos que serán Sobreescritos por clases hijas

        #region Botones y Doble clic sobre registro del Grid

        //DOBLE CLIC sobre una fila
        protected virtual void RadGridView1_DoubleClick(object sender, EventArgs e)
        {
            //César: Puede ocasionar un bug ya que este metodo también se llama desde VirtualGrid y el getGridViewRow
            //es exclusivo de los GridView
            try
            {
                try
                {
                    if (this.tableLayoutPanel1.Controls.Count > 0 && this.tableLayoutPanel1.Controls[0] is RadGridView)
                    {
                        getGridViewRow();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarErrorNuevo(ex, "error al obtener la columna");
                }
                log.Info("name=" + this.name);
                log.Info("selectedRow=" + _selectedRow);
                if (_selectedRow == null)
                {
                    if (_selectedRow == null)
                    {
                        if (this.name.Equals("ZonaLogicaLin"))
                        {
                            nuevoRegistro();
                        }
                        return;
                    }
                }
                else
                {
                    log.Info("no es null la linea");
                }
                var dialog = new SFDetalles(this.name, _lstEsquemaTabla, _selectedRow, SFDetalles.modoForm.lectura, _esquemGrid);
                log.Info("dialog" + dialog + " traduce=" + Lenguaje.traduce("Detalle"));
                dialog.AutoSize = true;
                dialog.AutoSizeMode = AutoSizeMode.GrowOnly;
                dialog.Text = Lenguaje.traduce("Detalle");
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.editado == true)
                    {
                        AckResponse ack = EditData(dialog.newRecord);
                        log.Info("ack=" + ack);
                        if (ack.Resultado == "OK")
                        {
                            RefreshData(this.virtualGridControl.PageIndex);
                            RadMessageBox.Show(ack.Mensaje);
                            llenarGrid();
                            log.Info("Se ha actualizado el campo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en la funcion generica");
                log.Error(ex);
            }
        }

        protected virtual void getGridViewRow()
        {
        }

        //Botón NUEVO
        public virtual void newButton_Click(object sender, EventArgs e)
        {
            nuevoRegistro();
        }

        private void nuevoRegistro()
        {
            try
            {
                var dialog = new SFDetalles(this.name, _lstEsquemaTabla, _selectedRow, SFDetalles.modoForm.nuevo, _esquemGrid, diccParamNuevo);
                dialog.Text = "Detalle";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AckResponse ack = NewData(dialog.newRecord);
                    if (ack.Resultado == "OK")
                    {
                        log.Debug("El usuario " + User.IdUsuario + " ha creado en el formulario " + this.name + " el registro " + _selectedRow);
                        CantidadRegistros += 1;
                        RefreshData(this.virtualGridControl.PageIndex);
                        llenarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        //Botón ELIMINAR
        public virtual void deleteButton_Click(object sender, EventArgs e)
        {
            borrarRegistro();
        }

        private void borrarRegistro()
        {
            try
            {
                if (_selectedRow == null)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Por favor, seleccione una fila."));
                    return;
                }
                else
                {
                    DialogResult ds = RadMessageBox.Show(this, Lenguaje.traduce("¿Desea eliminar el registro seleccionado?"), Lenguaje.traduce("Eliminar"), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                    if (ds == DialogResult.No)
                    {
                        return;
                    }
                    AckResponse ack = DeleteData(_selectedRow);
                    RadMessageBox.Show(Lenguaje.traduce(ack.Mensaje));
                    if (ack.Resultado == "OK")
                    {
                        log.Debug("El usuario " + User.IdUsuario + " ha borrado en el formulario " + this.name + " el registro " + _selectedRow);
                        CantidadRegistros -= 1;
                        RefreshData(this.virtualGridControl.PageIndex);
                        _selectedRow = null;
                        llenarGrid();
                        //LoadLayout();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al eliminar un registro"));
                log.Error(ex);
            }
        }

        //Botón CLONAR
        public virtual void cloneButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedRow == null)
                {
                    RadMessageBox.Show(strings.SeleccionaFila);
                    return;
                }
                else
                {
                    var dialog = new SFDetalles(this.name, _lstEsquemaTabla, _selectedRow, SFDetalles.modoForm.clonar, _esquemGrid, diccParamNuevo);
                    dialog.Text = Lenguaje.traduce("Detalle");
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        //AckResponse ack = NewData(dialog.newRecord);
                        bool clonarGrid = false;
                        AckResponse ack;
                        if (dialog.filasGrid > 0 && RadMessageBox.Show(strings.LineasDependientes, strings.Confirmar, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            clonarGrid = true;
                            ack = NewDataAndLines(dialog.newRecord, clonarGrid);
                        }
                        else
                        {
                            ack = NewData(dialog.newRecord);
                        }
                        if (ack.Mensaje != null)
                        {
                            RadMessageBox.Show(ack.Mensaje);
                        }
                        if (ack.Resultado == "OK")
                        {
                            log.Debug("El usuario " + User.IdUsuario + " ha clonado en el formulario " + this.name + " el registro " + _selectedRow);
                            CantidadRegistros += 1;
                            RefreshData(this.virtualGridControl.PageIndex);
                            llenarGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al clonar un registro"));
                log.Error(ex);
            }
        }

        //Boton Guardar
        public virtual void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
                vge.ShowDialog();
                if (VentanaGuardarEstilo.guardar == ESTILO_GLOBAL)
                {
                    SaveLayout(ESTILO_GLOBAL);
                }
                if (VentanaGuardarEstilo.guardar == ESTILO_LOCAL)
                {
                    SaveLayout(ESTILO_LOCAL);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al clonar al guardar el estilo"));
                log.Error(ex);
            }
        }

        //Boton cargar
        public virtual void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                VentanaGuardarEstilo vge = new VentanaGuardarEstilo(2);
                vge.ShowDialog();
                if (VentanaGuardarEstilo.guardar == ESTILO_GLOBAL)
                {
                    LoadLayout(ESTILO_GLOBAL);
                }
                if (VentanaGuardarEstilo.guardar == ESTILO_LOCAL)
                {
                    LoadLayout(ESTILO_LOCAL);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al cargar el estilo"));
                log.Error(ex);
            }
        }

        //No debería haber paginación
        public virtual void GridViewPageChanged(object sender, EventArgs e)
        {
            int index = _K_PAGINACION * GridView.MasterTemplate.PageIndex;
            GridView.CurrentRow = GridView.Rows[index];
        }

        public string confirmacion = strings.ExportacionExito;

        public void btnExportacion_Click(object sender, EventArgs e)
        {
            FuncionesGenerales.exportarAExcelGenerico(this.gridView);
            /*
            GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(this.gridView);
            spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
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
            }*/
        }

        public void btnQuitarFiltros_Click(object sender, EventArgs e)
        {
            QuitarFiltros();
        }

        public void btnColumnas_Click(object sender, EventArgs e)
        {
            GridView.ShowColumnChooser();
        }

        private void inicializarComboTamañofuente()
        {
            //for (int i = 1; i < 21; i++)
            //{
            //    //comboTamañofuente.Items.Add(i.ToString());
            //}
        }

        private void comboTamañoFuente_Changed(object sender, EventArgs e)
        {
            //float tamaño= float.Parse(comboTamañofuente.ComboBoxElement.SelectedItem.Text);
            //this.virtualGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.gridView.Font= new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        #endregion Botones y Doble clic sobre registro del Grid

        #region Estilos

        private void SaveLayout(int tipo)
        {
            try
            {
                string path = XmlReaderPropio.getLayoutPath(tipo);
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                {
                    path += "\\Español";
                }
                else
                {
                    path += "\\Ingles";
                }
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
                    path.Replace(" ", "_");
                    this.virtualGrid.SaveLayout(path);
                }
                else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + this.name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                    path.Replace(" ", "_");
                    GridView.SaveLayout(path);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al guardar el estilo"));
                log.Error(ex);
            }
        }

        private void LoadLayout(int tipo)
        {
            try
            {
                string path = XmlReaderPropio.getLayoutPath(tipo);
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                {
                    path += "\\Español";
                }
                else
                {
                    path += "\\Ingles";
                }
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
                {
                    string s = path + "\\" + this.name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
                    s.Replace(" ", "_");
                    this.virtualGrid.LoadLayout(s);
                }
                else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    string s = path + "\\" + this.name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                    s.Replace(" ", "_");
                    GridView.LoadLayout(s);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al cargar el estilo"));
                log.Error(ex);
            }
        }

        public virtual void ElegirEstilo()
        {
            try
            {
                string pathLocal =/* XmlReaderPropio.getLayoutPath(1);*/Persistencia.DirectorioLocal;
                string pathGlobal = /*XmlReaderPropio.getLayoutPath(0);*/ Persistencia.DirectorioGlobal;
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
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
                {
                    string pathVirtual = pathLocal + "\\" + this.name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
                    bool existsVirtual = File.Exists(pathVirtual);
                    if (existsVirtual)
                    {
                        virtualGrid.LoadLayout(pathVirtual);
                    }
                    else
                    {
                        pathVirtual = pathGlobal + "\\" + this.name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml";
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
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    string pathGridView = pathLocal + "\\" + this.name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                    bool existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        GridView.LoadLayout(pathGridView);
                    }
                    else
                    {
                        pathGridView = pathGlobal + "\\" + this.name + "GridView" + ConexionSQL.getNombreConexion() + ".xml";
                        existsGridView = File.Exists(pathGridView);
                        if (existsGridView)
                        {
                            GridView.LoadLayout(pathGridView);
                        }
                        else
                        {
                            gridView.BestFitColumns();
                            virtualGrid.BestFitColumns();
                        }
                    }
                }
                traducirEstilo();
            }
            catch (Exception ex)
            {
                log.Error("Error en ElegirEstilo() de BaseGridControl." + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void traducirEstilo()
        {
            try
            {
                foreach (GridViewColumn item in gridView.Columns)
                {
                    item.HeaderText = Lenguaje.traduce(item.HeaderText);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al traducir el estilo"));
                log.Error(ex);
            }
        }

        public void NombrarFormulario(string nombre)
        {
            //this.radLabel1.Text = nombre;
        }

        #endregion Estilos

        public virtual void btnCambiarVista_Click(object sender, EventArgs e)
        {
            //if (TblLayout.GetControlFromPosition(0, 1) is RadGridView)
            //{
            //    TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 1));
            //    TblLayout.Controls.Add(virtualGridControl, 0, 1);
            //    TblLayout.SetColumnSpan(virtualGridControl, 10);
            //    virtualGridControl.Dock = DockStyle.Fill;
            //}
            //else if (TblLayout.GetControlFromPosition(0, 1) is RadVirtualGrid)
            //{
            //    if (numRegistrosFiltrados > 100000)
            //    {
            //        MessageBox.Show(strings.ExcesoRegistros);
            //    }
            //    else
            //    {
            //        if (numRegistrosFiltrados > 50000)
            //        {
            //            MessageBox.Show("Aviso:" + strings.AvisoRegistros);
            //        }
            //        if (GridView.DataSource == null)
            //        {
            //            llenarGrid();
            //        }
            //        TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 1));
            //        TblLayout.Controls.Add(GridView, 0, 1);
            //        TblLayout.SetColumnSpan(GridView, 10);
            //        GridView.Dock = DockStyle.Fill;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Error inesperado");
            //}
        }

        private void itemColumnas_Click(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    gridView.ShowColumnChooser();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorColumnChooser));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void QuitarFiltros()
        {
            try
            {
                this.GridView.FilterDescriptors.Clear();
                this.virtualGrid.FilterDescriptors.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("Error al cargar los filtros"));
                log.Error(ex);
            }
        }
    }
}
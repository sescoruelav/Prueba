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
using RumboSGA.Presentation.Herramientas;

namespace RumboSGA.Presentation.UserControls
{
    public partial class ArticulosBaseGridControl : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Variables, Constantes y Propiedades
        private const float K_RATIO_AMPLIAR_VENTANA = 0.5F;
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
        RadGridView gridView;
        public string FiltroExterno;
        public Hashtable diccParamNuevo = new Hashtable();
        public string name;
        #endregion
        #region Constructor
        public ArticulosBaseGridControl()
        {
            name = "Articulos";
            InitializeComponent();
            //Inicialización de propiedades del Grid
            inicializarGrids();
            inicializarComboTamañofuente();
            tableLayoutPanel1.SetColumnSpan(virtualGrid, 10);
            
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
            GridView.EnablePaging = true;
            GridView.PageSize = _K_PAGINACION;
            GridView.MasterView.TableSearchRow.SearchDelay = 2000;
            GridView.PageChanged += gridView_CambioPagina;
            GridView.SortChanged += radGridView1_SortChanged;
            //GridView.CellFormatting += radGridView1_CellFormatting;
            //GridView.ViewCellFormatting += radGridView1_CellFormattingWrapText;
            GridView.FilterChanged += RadGridView_FilterChanged;
            GridView.PageChanged += GridViewPageChanged;
            //this.GridView.DoubleClick += RadGridView1_DoubleClick; ;
            #endregion
            #region VirtualGrid
            this.virtualGrid.SelectionMode = VirtualGridSelectionMode.FullRowSelect;
            this.virtualGrid.AllowFiltering = true;
            this.virtualGrid.AllowAddNewRow = false;
            this.virtualGrid.AllowColumnResize = true;
            this.virtualGrid.AllowRowResize = true;
            this.virtualGridControl.BestFitColumns();
            this.virtualGrid.CellValueNeeded += RadVirtualGrid_CellValueNeeded;
            this.virtualGrid.EnablePaging = true;
            this.virtualGrid.PageSize = _K_PAGINACION;
            this.virtualGrid.VirtualGridElement.PageIndexChanging += VirtualGridElement_PageIndexChanging;
            this.virtualGrid.BeginEditMode = RadVirtualGridBeginEditMode.BeginEditProgrammatically;
            this.virtualGrid.VirtualGridElement.TableElement.PagingPanelElement.Margin = new Padding(0, 0, 0, 1);
            this.virtualGrid.AllowMultiColumnSorting = false;
            this.virtualGrid.SortChanged += RadGridView1_SortChanged;
            this.virtualGrid.FilterChanged += RadVirtualGrid_FilterChanged;
            this.virtualGrid.DoubleClick += RadGridView1_DoubleClick; ;
            this.virtualGrid.ContextMenuOpening += grid_ContextMenuOpening;
            this.virtualGrid.Font = new Font("Segoe UI", 8.25F);
            this.virtualGrid.TableElement.RowHeight = 20;
            #endregion
        }
        #endregion
        #region Eventos propios (Filtrado, Orden, Paginación, Obtención asíncrona de datos)
        void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
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
        void radGridView1_CellFormattingWrapText(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
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
        void radGridView1_SortChanged(object sender, Telerik.WinControls.UI.GridViewCollectionChangedEventArgs e)
        {
            if (virtualGrid.SortDescriptors.Expression != gridView.SortDescriptors.Expression)
            {
                string sortExpression = this.gridView.SortDescriptors.Expression;
                this.virtualGrid.SortDescriptors.Expression = sortExpression;
                RefreshData(virtualGrid.PageIndex);
            }
        }
        private void RadVirtualGrid_FilterChanged(object sender, VirtualGridEventArgs e)
        {
            GridView.FilterChanged -= RadGridView_FilterChanged;
            GridView.FilterDescriptors.Clear();
            foreach (var item in virtualGrid.FilterDescriptors)
            {
                var newDescriptor = new FilterDescriptor(item.PropertyName, item.Operator, item.Value);
                GridView.FilterDescriptors.Add(item);
            }
            if (virtualGrid.FilterDescriptors.Expression != string.Empty)
                ArticulosFrm.lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetArticulosRegistrosFiltrados(" WHERE " + virtualGrid.FilterDescriptors.Expression).ToString();
            else
                ArticulosFrm.lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetArticulosRegistrosFiltrados("").ToString();
            GridView.FilterChanged += RadGridView_FilterChanged;
            RefreshData(virtualGrid.PageIndex);
        }
        private void RadGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            virtualGrid.FilterChanged -= RadVirtualGrid_FilterChanged;
            virtualGrid.FilterDescriptors.Clear();
            foreach (var item in GridView.FilterDescriptors)
            {
                var newDescriptor = new FilterDescriptor(item.PropertyName, item.Operator, item.Value);
                virtualGrid.FilterDescriptors.Add(newDescriptor);
            }
            if (gridView.FilterDescriptors.Expression != string.Empty)
                ArticulosFrm.lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetArticulosRegistrosFiltrados(" WHERE " + gridView.FilterDescriptors.Expression).ToString();
            else
                ArticulosFrm.lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetArticulosRegistrosFiltrados("").ToString();
            virtualGrid.FilterChanged += RadVirtualGrid_FilterChanged;
            RefreshData(virtualGrid.PageIndex);
        }
        //ORDENADO de registros, cada vez que se ordene, recargará la página seleccionada
        protected virtual void RadGridView1_SortChanged(object sender, VirtualGridEventArgs e)
        {
            var pag = virtualGrid.PageIndex;
            if (GridView.SortDescriptors.Expression != virtualGrid.SortDescriptors.Expression)
            {
                GridView.SortDescriptors.Expression = virtualGrid.SortDescriptors.Expression;
                //RefreshData(pag);
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
            var pag = e.NewIndex;
            RefreshData(pag);
            GridView.MasterTemplate.MoveToPage(pag);
        }
        private void gridView_CambioPagina(object sender, EventArgs e)
        {
            virtualGrid.PageIndex = int.Parse(GridView.GridViewElement.PagingPanelElement.PageNumberTextBox.Text) - 1;
        }
        void grid_ContextMenuOpening(object sender, Telerik.WinControls.UI.VirtualGridContextMenuOpeningEventArgs e)
        {
            int cont = 0;
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
            e.ContextMenu.Items[cont + 1].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
        }
        #endregion
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
        #endregion
        #region Botones y Doble clic sobre registro del Grid
        //DOBLE CLIC sobre una fila
        private void RadGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedRow == null)
            {
                getGridViewRow();
                if (_selectedRow==null)
                {
                    return;
                }
            }
            var dialog = new SFDetalles(this.name, _lstEsquemaTabla, _selectedRow, SFDetalles.modoForm.lectura, _esquemGrid);
            dialog.AutoSize = true;
            dialog.AutoSizeMode = AutoSizeMode.GrowOnly;
            dialog.Text = "Detalle";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.editado == true)
                {
                    AckResponse ack = EditData(dialog.newRecord);
                    RadMessageBox.Show(ack.Mensaje);
                    if (ack.Resultado == "OK")
                    {
                        RefreshData(this.virtualGridControl.PageIndex);
                        llenarGrid();
                    }
                }
            }
        }
        protected virtual void getGridViewRow()
        {

        }
        //Botón NUEVO
        public virtual void newButton_Click(object sender, EventArgs e)
        {
            var dialog = new SFDetalles(this.name, _lstEsquemaTabla, _selectedRow, SFDetalles.modoForm.nuevo, _esquemGrid, diccParamNuevo);
            dialog.Text = "Detalle";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                AckResponse ack = NewData(dialog.newRecord);
                RadMessageBox.Show(ack.Mensaje);
                if (ack.Resultado == "OK")
                {
                    log.Debug("El usuario " + User.IdUsuario + " ha creado en el formulario " + this.name + " el registro " + _selectedRow);
                    CantidadRegistros += 1;
                    RefreshData(this.virtualGridControl.PageIndex);
                    llenarGrid();
                }
            }
        }
        //Botón ELIMINAR
        public virtual void deleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedRow == null)
            {
                RadMessageBox.Show("Por favor, seleccione una fila.");
                return;
            }
            else
            {
                AckResponse ack = DeleteData(_selectedRow);
                RadMessageBox.Show(ack.Mensaje);
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
        //Botón CLONAR
        public virtual void cloneButton_Click(object sender, EventArgs e)
        {
            if (_selectedRow == null)
            {
                RadMessageBox.Show(strings.SeleccionaFila);
                return;
            }
            else
            {
                var dialog = new SFDetalles(this.name, _lstEsquemaTabla, _selectedRow, SFDetalles.modoForm.clonar, _esquemGrid, diccParamNuevo);
                dialog.Text = "Detalle";
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
                    RadMessageBox.Show(ack.Mensaje);
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
        //Boton Guardar
        public virtual void SaveButton_Click()
        {
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                SaveLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                SaveLayoutLocal();
            }
        }
        //Boton cargar
        public virtual void LoadButton_Click()
        {
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(2);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                LoadLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                LoadLayoutLocal();
            }
        }
        public void GridViewPageChanged(object sender,EventArgs e)
        {
            int index = _K_PAGINACION * GridView.MasterTemplate.PageIndex;
            GridView.CurrentRow = GridView.Rows[index];
        }
        public string confirmacion = strings.ExportacionExito;
        public void btnExportacion_Click()
        {
            FuncionesGenerales.exportarAExcelGenerico(this.gridView);
        }
        public void btnQuitarFiltros_Click(object sender, EventArgs e)
        {
            this.GridView.FilterDescriptors.Clear();
            this.virtualGrid.FilterDescriptors.Clear();
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
        #endregion
        #region Estilos
        public void SaveLayoutGlobal()
        {
            string path = XmlReaderPropio.getLayoutPath(0);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                Directory.CreateDirectory(path);
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                path += "\\" + this.name + "VirtualGrid.xml";
                path.Replace(" ", "_");
                this.virtualGrid.SaveLayout(path);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {                
                Directory.CreateDirectory(path);
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                path += "\\" + this.name + "GridView.xml";
                path.Replace(" ", "_");
                GridView.SaveLayout(path);
            }
        }
        public void SaveLayoutLocal()
        {
            string path = XmlReaderPropio.getLayoutPath(1);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                Directory.CreateDirectory(path);
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                path += "\\" + this.name + "VirtualGrid.xml";
                path.Replace(" ", "_");
                this.virtualGrid.SaveLayout(path);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                Directory.CreateDirectory(path);
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                path += "\\" + this.name + "GridView.xml";
                path.Replace(" ", "_");
                GridView.SaveLayout(path);
            }
        }
        public void LoadLayoutGlobal()
        {
            string path = XmlReaderPropio.getLayoutPath(0);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                string s = path + "\\" + this.name + "VirtualGrid.xml";
                s.Replace(" ", "_");
                this.virtualGrid.LoadLayout(s);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                string s = path + "\\" + this.name + "GridView.xml";
                s.Replace(" ", "_");
                GridView.LoadLayout(s);
            }
        }
        public void LoadLayoutLocal()
        {
            string path = XmlReaderPropio.getLayoutPath(1);
            if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                string s = path + "\\" + this.name + "VirtualGrid.xml";
                s.Replace(" ", "_");
                this.virtualGrid.LoadLayout(s);
            }
            else if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    path += "\\Español";
                else
                    path += "\\Ingles";
                string s = path + "\\" + this.name + "GridView.xml";
                s.Replace(" ", "_");
                GridView.LoadLayout(s);
            }
        }
        public void ElegirEstilo()
        {
            string pathLocal = XmlReaderPropio.getLayoutPath(1);
            string pathGlobal = XmlReaderPropio.getLayoutPath(0);
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
                /*for (int i = 0; i < _lstEsquemaTabla.Count; i++)
                {
                    if (_lstEsquemaTabla[i].Control.Equals("CBSN"))
                    {
                        for (int j = 0; j < gridView.Columns.Count; j++)
                        {
                            if(gridView.Columns[j].Name.Equals(_lstEsquemaTabla[i].Nombre) ||
                                gridView.Columns[j].HeaderText.Equals(_lstEsquemaTabla[i].Etiqueta))
                            {
                                GridViewCheckBoxColumn chbx = new GridViewCheckBoxColumn();
                                String nombreCheckBox = "CheckBox" + _lstEsquemaTabla[i].Nombre;
                                chbx.Name = nombreCheckBox;
                                gridView.Columns.Add(chbx);
                                for (int x = 0; x < gridView.Rows.Count; x++)
                                {
                                    if (gridView.Rows[x].Cells[nombreCheckBox] != null)
                                    {
                                        if (gridView.Rows[x].Cells[gridView.Columns[j].Name].Value.ToString().Equals("S"))
                                        {
                                            gridView.Rows[x].Cells[nombreCheckBox].Value = true;
                                        }
                                        else
                                        {
                                            gridView.Rows[x].Cells[nombreCheckBox].Value = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }*/
            }
            traducirEstilo();
        }
        public void traducirEstilo()
        {
            foreach (GridViewColumn item in gridView.Columns)
            {
                item.FieldName = Lenguaje.traduce(item.FieldName);
                item.HeaderText = Lenguaje.traduce(item.HeaderText);
            }
        }
        public void NombrarFormulario(string nombre)
        {
            //this.radLabel1.Text = nombre;
        }
        #endregion
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
            if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                gridView.ShowColumnChooser();
            }
            else
            {
                MessageBox.Show(strings.ErrorColumnChooser);
            }
        }
    }
}
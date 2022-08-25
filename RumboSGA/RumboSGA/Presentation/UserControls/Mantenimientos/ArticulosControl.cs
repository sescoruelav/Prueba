using Newtonsoft.Json.Linq;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.Herramientas;
using Telerik.WinControls;
using Telerik.WinControls.Layouts;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using System.ComponentModel;
using Newtonsoft.Json;
using Telerik.WinControls.Data;
using System.Data;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class ArticulosControl : ArticulosBaseGridControl
    {
        #region Variables y Propiedades
        private string modo;

        public List<dynamic> data = new List<dynamic>();

        public dynamic newRecord { get; set; }
        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        public EventHandler EditButton_Click { get; }
        public SFDetallesArticulos AgregarTabsGeneral { get; set; }
        public SFDetallesArticulos AgregarTabsUbicacion { get; set; }
        public SFDetallesArticulos AgregarTabsUbicacionUD { get; set; }
        public SFDetallesArticulos AgregarTabsSKU { get; set; }
        public SFDetallesArticulos AgregarTabsMedidas { get; set; }


        BackgroundWorker bgwWorkerCargaPantalla;

        RadMenuComboItem menuComboItem;

        RadWaitingBar waitingBar = new RadWaitingBar();

        public DialogResult DialogResult { get; private set; }


        //private string state;
        public object estado;
        public enum Estados
        {
            VistaRapida,
            VCTodoGrid,
            VCTodoDetalle,
            VCHibrido
        };
        private string nombreTab;
        private string filterExpressionActual;
        public int PageSize;
        //public const string estadoAbierto = "opened";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #endregion

        #region Constructor

        public ArticulosControl()
        {
            InitializeComponent();

            InicializarEstiloFilasPanelContenedor();            

            InicializarEventos();

            //inicializamos carga vista
            ConfigurarBarraProgresoGridView();
            /*waitingBar.StartWaiting();
            GridView.BeginUpdate();
            ConfigurarBarraProgresoGridView();
            InicializarWorker();
            bgwWorkerCargaPantalla.RunWorkerAsync();
            */
            ObtencionTotalRegistros();

            //RefreshData(0);
            ElegirGrid();
            
            //TODO: Refresh Data no funciona correctamente en el inicio;

            //ElegirEstilo();

            //Le asigna un valor al estado para poder controlar las 3 vistas
            RefreshData(0);
            CambiarNombresTabs();
            PageSize = _K_PAGINACION;
            this.Show();

        }

        private void InicializarEstiloFilasPanelContenedor()
        {
            PanelContenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            PanelContenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            PanelContenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
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
                if (this.data.Count > 0)
                {
                    FilaPorDefecto();
                }
            });

            //Declaración Eventos 
            this.virtualGridControl.SelectionChanged += GridControl_SelectionChanged;
            this.GridView.FilterChanged += gridViewFilterChanged_Event;
            this.GridView.Click += RadGridView1_Click;
        }

        //Prepara el grid con la cantidad total de filas y columnas a pintar luego por el evento RadVirtualGrid_CellValueNeeded
        private void InicializarFilasColumnasGrid()
        {
            this.virtualGridControl.ColumnCount = _lstEsquemaTabla.Count;
            this.virtualGridControl.RowCount = CantidadRegistros + 1;
        }

        private void InicializarWorker()
        {
            bgwWorkerCargaPantalla = new BackgroundWorker();
            bgwWorkerCargaPantalla.WorkerReportsProgress = true;
            bgwWorkerCargaPantalla.WorkerSupportsCancellation = true;
            bgwWorkerCargaPantalla.DoWork += backgroundWorker_DoWork;
            bgwWorkerCargaPantalla.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerCompleted);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ElegirGrid();

            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void backgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            //ElegirEstilo();
            waitingBar.StopWaiting();
            GridView.EndUpdate();
        }

        private void ConfigurarBarraProgresoGridView()
        {
            this.waitingBar.Name = "radWaitingBar1";
            this.waitingBar.Size = new System.Drawing.Size(200, 20);
            this.waitingBar.TabIndex = 2;
            this.waitingBar.Text = "radWaitingBar1";
            this.waitingBar.AssociatedControl = this.GridView;
        }

        //Cambia entre la vista de detalle y la vista normal, se le llama desde la clase ArticulosFrm que contiene la clase ArticulosControl

        public void CambiarEstado()
        {
            switch (estado)
            {
                case Estados.VistaRapida:
                    TblLayout.Controls.Remove(GridView);
                    TblLayout.Controls.Add(virtualGridControl, 0, 0);
                    TblLayout.SetColumnSpan(virtualGridControl, 10);
                    virtualGridControl.Dock = DockStyle.Fill;
                    llenarGrid();
                    estado = Estados.VCHibrido;
                    break;
                case Estados.VCHibrido: //Al darle, pasará a TodoGrid
                    tabControl1.Hide();
                    tableLayoutPanel1.SetRowSpan(tableLayoutPanel1, 2);
                    tableLayoutPanel1.Dock = DockStyle.Fill;
                    estado = Estados.VCTodoGrid;
                    break;
                case Estados.VCTodoGrid: //Al darle, pasará a TodoDetalle
                    tableLayoutPanel1.Hide();
                    PanelContenedor.RowStyles.Clear();
                    tabControl1.Dock = DockStyle.Fill;
                    tabControl1.Show();
                    estado = Estados.VCTodoDetalle;
                    break;
                case Estados.VCTodoDetalle: //Al darle, pasará a Hibrido
                    InicializarEstiloFilasPanelContenedor();
                    tableLayoutPanel1.Show();
                    tableLayoutPanel1.SetRowSpan(tableLayoutPanel1, 1);
                    tableLayoutPanel1.Dock = DockStyle.Fill;
                    tabControl1.Show();                    
                    TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 0));
                    TblLayout.Controls.Add(GridView, 0, 0);
                    TblLayout.SetColumnSpan(GridView, 10);
                    GridView.Dock = DockStyle.Fill;
                    estado = Estados.VistaRapida;
                    break;
            }

            //if (state == "opened")
            //{
            //    tabControl1.Hide();
            //    tableLayoutPanel1.SetRowSpan(tableLayoutPanel1, 2);
            //    tableLayoutPanel1.Dock = DockStyle.Fill;
            //    state = "closed";
            //}
            //else
            //{
            //    tableLayoutPanel1.SetRowSpan(tableLayoutPanel1, 1);
            //    tableLayoutPanel1.Dock = DockStyle.Fill;
            //    tabControl1.Show();
            //    state = "opened";
            //}
        }

        //Obtención de cantidad total de registros
        private void ObtencionTotalRegistros()
        {
            //Obtenemos, asíncronamente, la cantidad total de registros de Proveedores, así como su esquema de datos, para poder preparar el Grid                        
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            CantidadRegistros = Business.GetArticulosCantidad(ref _lstEsquemaTabla);
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
                    estado = Estados.VistaRapida;                    
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
                //int index
                int index = this.virtualGridControl.CurrentCell.RowIndex - this.virtualGridControl.PageSize * this.virtualGridControl.PageIndex;
                var item = data[index];

                _selectedRow = item; //Fila seleccionada, en formato JSON
                llenarTabs();
                ArticulosFrm.ActivarEditar();
            }
        }

        private void virtualGridControlFilterChanged_Event(object sender, EventArgs e)
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


        //Al hacer click en un elemento del grid llena la información de las pestañas
        public void RadGridView1_Click(object sender, EventArgs e)
        {
            
                _selectedRow = null;
                getGridViewRow();
            if (_selectedRow != null && AgregarTabsGeneral == null)
            {
                //Rellena las pestañas
                llenarTabs();
            }
            else
            {
                //gridViewFilterChanged_Event(sender, e);
                RefrescarTabs();

            }
            
            //Habilita los botones para poder modificar las pestáñas

            ArticulosFrm.editArticulo.Visibility = ElementVisibility.Visible;
            ArticulosFrm.nuevoButton.Visibility = ElementVisibility.Visible;
            ArticulosFrm.cleanButton.Visibility = ElementVisibility.Visible;
            ArticulosFrm.clonarButton.Visibility = ElementVisibility.Visible;
            ArticulosFrm.saveButton.Visibility = ElementVisibility.Collapsed;
            ArticulosFrm.cancelarButton.Visibility = ElementVisibility.Collapsed;

        }

        public void FilaPorDefecto()
        {
            
            var item = data[0];

            _selectedRow = item;
            

            //getGridViewRow();
            if (_selectedRow != null && AgregarTabsGeneral == null)
            {
                //Rellena las pestañas
                llenarTabs();
            }
            else
            {
                //gridViewFilterChanged_Event(sender, e);
                RefrescarTabs();

            }

            //Habilita los botones para poder modificar las pestáñas

            ArticulosFrm.editArticulo.Visibility = ElementVisibility.Visible;
            ArticulosFrm.nuevoButton.Visibility = ElementVisibility.Visible;
            ArticulosFrm.cleanButton.Visibility = ElementVisibility.Visible;
            ArticulosFrm.clonarButton.Visibility = ElementVisibility.Visible;
            ArticulosFrm.saveButton.Visibility = ElementVisibility.Collapsed;
            ArticulosFrm.cancelarButton.Visibility = ElementVisibility.Collapsed;

        }


        protected override void getGridViewRow()
        {
            if (this.GridView.CurrentCell == null) return; 
            if (this.GridView.CurrentCell.RowIndex < 0) return;

            Dictionary<dynamic, dynamic> fila = new Dictionary<dynamic, dynamic>();
            for (int i = 0; i < this.GridView.ColumnCount; i++)
            {
                fila.Add(this.GridView.Columns[i].Name, this.GridView.SelectedRows[0].Cells[i].Value);
            }
            string jsonLinea = JsonConvert.SerializeObject(fila);

            _selectedRow = jsonLinea;
            return;
            for (int i = 0; i < data.Count; i++)
            {
                JObject objetoJson = JObject.Parse(data[i].ToString());

                string jsonID = (string)objetoJson[_lstEsquemaTabla[0].Nombre];
                if (this.GridView.CurrentCell != null)
                {
                    if (this.GridView.CurrentCell.RowIndex >= 0)
                    {
                        if (this.GridView.CurrentRow.Cells[_lstEsquemaTabla[0].Nombre].Value.ToString() == jsonID)
                        {
                            _selectedRow = data[i];
                            break;
                        }
                    }
                }
                ArticulosFrm.ActivarEditar();
            }


        }

        #endregion

        #region Eventos Sobreescritos de la clase padre BaseGridControl 
        protected override void ElegirGrid()
        {
            //TODO: 10000 debe ser una constante en la cabecera
            if (CantidadRegistros > 10000)
            {
                ObtencionTotalRegistros();

                TblLayout.Controls.Add(virtualGridControl, 0, 0);
                TblLayout.SetColumnSpan(virtualGridControl, 9);
                ElegirEstilo();
                estado = Estados.VistaRapida;
            }
            else
            {
                llenarGrid();
                TblLayout.Controls.Add(GridView, 0, 0);
                TblLayout.SetColumnSpan(GridView, 9);
                GridView.Dock = System.Windows.Forms.DockStyle.Fill;
                GridView.BestFitColumns();
                ElegirEstilo();
                GridView.CurrentRow = GridView.Rows[0];
                GridView.CurrentColumn = GridView.Columns[0];
                GridView.TableElement.RowHeight = 30;
                estado = Estados.VCHibrido;
            }

        }

        public void RefrescarGrid()
        {
            ElegirGrid();
        }
        //Se comunica con la clase SFDetallesArticulos y ArticulosControl para pasarles la orden desde el botón editar
        public void CambiarModo()
        { if (_selectedRow != null) {
         HabilitarDeshabilitarControles(SFDetallesArticulos.modoForm.edicion);

            }
        }

        public void NuevoArticulo()
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

        public void ClonarArticulo()
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

        public void EliminarArticulo()
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

        public void ConfigurarArticulos()
        {
            if (GridView.ChildRows.Count == 0) return;
            SFConfigurarArticulos ConfigurarArticulos = new SFConfigurarArticulos(this.name, _lstEsquemaTabla, _selectedRow, SFConfigurarArticulos.modoForm.edicion, _esquemGrid, diccParamNuevo);
            ConfigurarArticulos.ShowDialog();

            Telerik.WinControls.Data.FilterDescriptorCollection filtrosAnteriores = this.GridView.FilterDescriptors;

            for (int i = 0; i < GridView.ChildRows.Count; i++)
            {
                string filtrosEnSQL = " IDARTICULO=" + this.GridView.ChildRows[i].Cells[0].Value;
                log.Debug("Se va actualizar ConfigurarArticulos Masivos, filtros telerik: " + this.GridView.FilterDescriptors.Expression + "; Filtro where:" + filtrosEnSQL);

                AckResponse ack = EditDataArticulosConfiguracion(ConfigurarArticulos.newRecord, filtrosEnSQL);
                //RadMessageBox.Show(ack.Mensaje);
                if (ack != null && ack.Resultado == "OK")
                {
                    log.Debug("Actualización realizada!");
                    //ElegirGrid();

                    //llenarTabs();
                }
            }
            ElegirGrid();
        }

        public void menuItemGuardarMethod()
        {
            menuItemGuardar_Click();
        }

        public void menuItemGuardar_Click()
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
        public void menuItemCargar_Click()
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

        public void Columnas_Click()
        {
            GridView.ShowColumnChooser();
        }

        public void QuitarFiltros_Click()
        {
            this.GridView.FilterDescriptors.Clear();
            this.virtualGrid.FilterDescriptors.Clear();
        }

        //Método para el botón aceptar
        public void AceptarFormulario()
        {
            AgregarTabsGeneral.AceptarFormulario();
            AgregarTabsUbicacion.AceptarFormulario();
            AgregarTabsUbicacionUD.AceptarFormulario();
            AgregarTabsSKU.AceptarFormulario();
            AgregarTabsMedidas.AceptarFormulario();

            DialogResult = DialogResult.OK;

            AckResponse ack = EditData(AgregarTabsGeneral.newRecord);
            AckResponse ackUbicacion = EditData(AgregarTabsUbicacion.newRecord);
            AckResponse ackUbicacionUD = EditData(AgregarTabsUbicacionUD.newRecord);
            AckResponse ackSKU = EditData(AgregarTabsSKU.newRecord);
            AckResponse ackMedidas = EditData(AgregarTabsMedidas.newRecord);
            RadMessageBox.Show(ack.Mensaje);
            if (ack.Resultado == "OK")
            {
                ElegirGrid();

                //llenarTabs();
            }


        }
        //Método para el botón cancelar
        public void CancelarFormulario()

        {
            //AgregarTabsGeneral.CancelarFormulario();
            DialogResult = DialogResult.Cancel;

            HabilitarDeshabilitarControles(SFDetallesArticulos.modoForm.lectura);
        }

        private void CambiarNombresTabs()
        {
            this.tabUbicacion.Text = Lenguaje.traduce(strings.Ubicacion);
            this.tabGeneral.Text = Lenguaje.traduce(strings.General);
            this.tabUbicacionUnidades.Text = Lenguaje.traduce(strings.UbicacionUnidades);
            this.tabMedidas.Text = Lenguaje.traduce(strings.Medidas);
        }
        public override void llenarGrid()
        {

            try
            {
                base.llenarGrid();
                DataTable dt=  Business.GetArticulosDatosGridView(_lstEsquemaTabla);

                Utilidades.RellenarConCBSN(ref dt, _lstEsquemaTabla);
                
                GridView.DataSource = dt;
                
                for (int i = 0; i < _lstEsquemaTabla.Count; i++)
                {
                    if (GridView.Columns[_lstEsquemaTabla[i].Etiqueta]!=null)
                    {
                        GridView.Columns[_lstEsquemaTabla[i].Etiqueta].FieldName = _lstEsquemaTabla[i].Etiqueta;
                        GridView.Columns[_lstEsquemaTabla[i].Etiqueta].Name = _lstEsquemaTabla[i].Nombre;
                    }
                }
                //Sustituido por un recorrido de lstEsquemaTabla para evitar error al insertar el columnsCheckBox
                /*foreach (var column in GridView.Columns)
                {
                    
                    column.Name = _lstEsquemaTabla[column.Index].Nombre;
                    column.FieldName = _lstEsquemaTabla[column.Index].Etiqueta;
                }*/
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.ErrorLlenarGrid));
            }
        }

        public void ExportarExcel()
        {
            FuncionesGenerales.exportarAExcelGenerico(this.GridView);
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

        //Método que llama a SFDetallesTabs para inicializar una instancia con los datos a mostrar en las pestañas, una en cada pestaña
        public void llenarTabs()
        {
            //Evita que se habilite el botón editar hasta que se haya seleccionado una fila


            //Agregar en la pestaña general
            
            AgregarTabsGeneral = new SFDetallesArticulos("SFDetallesGeneral", "General", _lstEsquemaTabla, _selectedRow, SFDetallesArticulos.modoForm.lectura, _esquemGrid);
            AgregarTabsGeneral.Dock = DockStyle.Fill;
            AgregarTabsGeneral.TopLevel = false;
            tableGeneral.Controls.Add(AgregarTabsGeneral);
            AgregarTabsGeneral.CargarDatos(_selectedRow,tableGeneral.Size);
            AgregarTabsGeneral.Show();
            
            //AgregarTabsGeneral.BringToFront();

            //Agregar en la pestaña ubicación
            AgregarTabsUbicacion = new SFDetallesArticulos("SFDetallesUbicacion", "ubicacion", _lstEsquemaTabla, _selectedRow, SFDetallesArticulos.modoForm.lectura, _esquemGrid);
            AgregarTabsUbicacion.Dock = DockStyle.Fill;
            AgregarTabsUbicacion.TopLevel = false;
            tableUbicacion.Controls.Add(AgregarTabsUbicacion);
            AgregarTabsUbicacion.CargarDatos(_selectedRow, tableGeneral.Size);
            AgregarTabsUbicacion.Show();
            
            //AgregarTabsUbicacion.BringToFront();

            //Agregar en la pestaña UbicaciónUD
            AgregarTabsUbicacionUD = new SFDetallesArticulos(this.name, "ubicacionud", _lstEsquemaTabla, _selectedRow, SFDetallesArticulos.modoForm.lectura, _esquemGrid);
            AgregarTabsUbicacionUD.TopLevel = false;
            tableUbicacionUD.Controls.Add(AgregarTabsUbicacionUD);
            AgregarTabsUbicacionUD.Dock = DockStyle.Fill;
            AgregarTabsUbicacionUD.CargarDatos(_selectedRow, tableGeneral.Size);
            AgregarTabsUbicacionUD.Show();
            
            //AgregarTabsUbicacionUD.BringToFront();

            AgregarTabsSKU = new SFDetallesArticulos(this.name, "sku", _lstEsquemaTabla, _selectedRow, SFDetallesArticulos.modoForm.lectura, _esquemGrid);
            AgregarTabsSKU.TopLevel = false;
            tableSKU.Controls.Add(AgregarTabsSKU);
            AgregarTabsSKU.Dock = DockStyle.Fill;
            AgregarTabsSKU.CargarDatos(_selectedRow, tableGeneral.Size);
            AgregarTabsSKU.Show();
            
            //AgregarTabsSKU.BringToFront();

            AgregarTabsMedidas = new SFDetallesArticulos(this.name, "medidas", _lstEsquemaTabla, _selectedRow, SFDetallesArticulos.modoForm.lectura, _esquemGrid);
            AgregarTabsMedidas.TopLevel = false;
            tableMedidas.Controls.Add(AgregarTabsMedidas);
            AgregarTabsMedidas.Dock = DockStyle.Fill;
            AgregarTabsMedidas.CargarDatos(_selectedRow, tableGeneral.Size);
            AgregarTabsMedidas.Show();
            
            //AgregarTabsMedidas.BringToFront();

            //tabPageGeneral.Controls.Add(AgregarTabs);
        }

        public void RefrescarTabs()
        {
            
            AgregarTabsGeneral.ActualizarDatos(_selectedRow);
            AgregarTabsUbicacion.ActualizarDatos(_selectedRow);
            AgregarTabsUbicacionUD.ActualizarDatos(_selectedRow);
            AgregarTabsSKU.ActualizarDatos(_selectedRow);
            AgregarTabsMedidas.ActualizarDatos(_selectedRow);
            
        }

        public void HabilitarDeshabilitarControles(SFDetallesArticulos.modoForm _modoAp)
        {
            try
            {
                AgregarTabsGeneral.ModoAperturaForm = _modoAp;
                AgregarTabsUbicacion.ModoAperturaForm = _modoAp;
                AgregarTabsUbicacionUD.ModoAperturaForm = _modoAp;
                AgregarTabsSKU.ModoAperturaForm = _modoAp;
                AgregarTabsMedidas.ModoAperturaForm = _modoAp;

                //botones




                switch (_modoAp)
                {
                    case SFDetallesArticulos.modoForm.lectura:

                        break;
                    case SFDetallesArticulos.modoForm.edicion:
                    case SFDetallesArticulos.modoForm.nuevo:
                    case SFDetallesArticulos.modoForm.clonar:

                        break;
                }


                //controles dinámicos
                if (AgregarTabsGeneral.Controls[0] != null)
                {
                    //TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0];


                    TableLayoutPanel tpanel = (TableLayoutPanel)this.AgregarTabsGeneral.dataDialog1.PanelControles.Controls[0].Controls[0];
                    tpanel.Dock = DockStyle.Fill;

                    //for (int i = 0; i < tpanel.RowCount; i++)
                    for (int i = 0; i < AgregarTabsGeneral._filasTotalesTabla + 1; i++)
                    {
                        for (int j = 0; j < tpanel.ColumnCount; j++)
                        {

                            NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);

                            if (par != null)
                            {

                                TableScheme esquemaBuscado;



                                switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                                {
                                    case "RadTextBox":

                                        RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);

                                        esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(tb.Name);


                                        tb.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);


                                        break;
                                    case "RadDropDownList":
                                        RadDropDownList combo = (RadDropDownList)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(combo.Name);
                                        combo.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;
                                    case "RadDateTimePicker":
                                        RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(dtPicker.Name);
                                        dtPicker.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;
                                    case "RadButtonTextBox":
                                        RadButtonTextBox rdColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(rdColor.Name);
                                        rdColor.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;
                                    case "RadMultiColumnComboBox":
                                        RadMultiColumnComboBox combobox = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(combobox.Name);
                                        combobox.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;
                                    case "RadCheckBox":
                                        RadCheckBox rcb = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(rcb.Name);
                                        rcb.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;

                                }

                            }
                        }


                    }
                }




            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

            if (AgregarTabsUbicacion.Controls[0] != null)
            {
                //TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0];


                TableLayoutPanel tpanel = (TableLayoutPanel)this.AgregarTabsUbicacion.dataDialog1.PanelControles.Controls[0].Controls[0];

                //for (int i = 0; i < tpanel.RowCount; i++)
                for (int i = 0; i < AgregarTabsUbicacion._filasTotalesTabla + 1; i++)
                {
                    for (int j = 0; j < tpanel.ColumnCount; j++)
                    {

                        NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);

                        if (par != null)
                        {

                            TableScheme esquemaBuscado;



                            switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                            {
                                case "RadTextBox":

                                    RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);

                                    esquemaBuscado = AgregarTabsUbicacion.BuscarEsquemaControl(tb.Name);


                                    tb.Enabled = AgregarTabsUbicacion.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);


                                    break;
                                case "RadDropDownList":
                                    RadDropDownList combo = (RadDropDownList)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacion.BuscarEsquemaControl(combo.Name);
                                    combo.Enabled = AgregarTabsUbicacion.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadDateTimePicker":
                                    RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacion.BuscarEsquemaControl(dtPicker.Name);
                                    dtPicker.Enabled = AgregarTabsUbicacion.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadButtonTextBox":
                                    RadButtonTextBox rdColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacion.BuscarEsquemaControl(rdColor.Name);
                                    rdColor.Enabled = AgregarTabsUbicacion.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadMultiColumnComboBox":
                                    RadMultiColumnComboBox combobox = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacion.BuscarEsquemaControl(combobox.Name);
                                    combobox.Enabled = AgregarTabsUbicacion.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadCheckBox":
                                    RadCheckBox rcb = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(rcb.Name);
                                    rcb.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;

                            }

                        }
                    }


                }
            }

            if (AgregarTabsUbicacionUD.Controls[0] != null)
            {
                //TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0];


                TableLayoutPanel tpanel = (TableLayoutPanel)this.AgregarTabsUbicacionUD.dataDialog1.PanelControles.Controls[0].Controls[0];

                //for (int i = 0; i < tpanel.RowCount; i++)
                for (int i = 0; i < AgregarTabsUbicacionUD._filasTotalesTabla + 1; i++)
                {
                    for (int j = 0; j < tpanel.ColumnCount; j++)
                    {

                        NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);

                        if (par != null)
                        {

                            TableScheme esquemaBuscado;



                            switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                            {
                                case "RadTextBox":

                                    RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);

                                    esquemaBuscado = AgregarTabsUbicacionUD.BuscarEsquemaControl(tb.Name);


                                    tb.Enabled = AgregarTabsUbicacionUD.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);


                                    break;
                                case "RadDropDownList":
                                    RadDropDownList combo = (RadDropDownList)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacionUD.BuscarEsquemaControl(combo.Name);
                                    combo.Enabled = AgregarTabsUbicacionUD.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadDateTimePicker":
                                    RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacionUD.BuscarEsquemaControl(dtPicker.Name);
                                    dtPicker.Enabled = AgregarTabsUbicacionUD.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadButtonTextBox":
                                    RadButtonTextBox rdColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacionUD.BuscarEsquemaControl(rdColor.Name);
                                    rdColor.Enabled = AgregarTabsUbicacionUD.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadMultiColumnComboBox":
                                    RadMultiColumnComboBox combobox = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsUbicacionUD.BuscarEsquemaControl(combobox.Name);
                                    combobox.Enabled = AgregarTabsUbicacionUD.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadCheckBox":
                                    RadCheckBox rcb = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(rcb.Name);
                                    rcb.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                            }

                        }
                    }


                }
            }

            if (AgregarTabsSKU.Controls[0] != null)
            {
                //TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0];


                TableLayoutPanel tpanel = (TableLayoutPanel)this.AgregarTabsSKU.dataDialog1.PanelControles.Controls[0].Controls[0];

                //for (int i = 0; i < tpanel.RowCount; i++)
                for (int i = 0; i < AgregarTabsSKU._filasTotalesTabla + 1; i++)
                {
                    for (int j = 0; j < tpanel.ColumnCount; j++)
                    {

                        NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);

                        if (par != null)
                        {

                            TableScheme esquemaBuscado;



                            switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                            {
                                case "RadTextBox":

                                    RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);

                                    esquemaBuscado = AgregarTabsSKU.BuscarEsquemaControl(tb.Name);


                                    tb.Enabled = AgregarTabsSKU.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);


                                    break;
                                case "RadDropDownList":
                                    RadDropDownList combo = (RadDropDownList)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsSKU.BuscarEsquemaControl(combo.Name);
                                    combo.Enabled = AgregarTabsSKU.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadDateTimePicker":
                                    RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsSKU.BuscarEsquemaControl(dtPicker.Name);
                                    dtPicker.Enabled = AgregarTabsSKU.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadButtonTextBox":
                                    RadButtonTextBox rdColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsSKU.BuscarEsquemaControl(rdColor.Name);
                                    rdColor.Enabled = AgregarTabsSKU.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadMultiColumnComboBox":
                                    RadMultiColumnComboBox combobox = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsSKU.BuscarEsquemaControl(combobox.Name);
                                    combobox.Enabled = AgregarTabsSKU.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadCheckBox":
                                    RadCheckBox rcb = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(rcb.Name);
                                    rcb.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;

                            }

                        }
                    }


                }
            }

            if (AgregarTabsMedidas.Controls[0] != null)
            {
                //TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0];


                TableLayoutPanel tpanel = (TableLayoutPanel)this.AgregarTabsMedidas.dataDialog1.PanelControles.Controls[0].Controls[0];

                //for (int i = 0; i < tpanel.RowCount; i++)
                for (int i = 0; i < AgregarTabsMedidas._filasTotalesTabla + 1; i++)
                {
                    for (int j = 0; j < tpanel.ColumnCount; j++)
                    {

                        NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);

                        if (par != null)
                        {

                            TableScheme esquemaBuscado;



                            switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                            {
                                case "RadTextBox":

                                    RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);

                                    esquemaBuscado = AgregarTabsMedidas.BuscarEsquemaControl(tb.Name);


                                    tb.Enabled = AgregarTabsMedidas.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);


                                    break;
                                case "RadDropDownList":
                                    RadDropDownList combo = (RadDropDownList)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsMedidas.BuscarEsquemaControl(combo.Name);
                                    combo.Enabled = AgregarTabsMedidas.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadDateTimePicker":
                                    RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsMedidas.BuscarEsquemaControl(dtPicker.Name);
                                    dtPicker.Enabled = AgregarTabsMedidas.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadButtonTextBox":
                                    RadButtonTextBox rdColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsMedidas.BuscarEsquemaControl(rdColor.Name);
                                    rdColor.Enabled = AgregarTabsMedidas.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadMultiColumnComboBox":
                                    RadMultiColumnComboBox combobox = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsMedidas.BuscarEsquemaControl(combobox.Name);
                                    combobox.Enabled = AgregarTabsMedidas.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;
                                case "RadCheckBox":
                                    RadCheckBox rcb = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = AgregarTabsGeneral.BuscarEsquemaControl(rcb.Name);
                                    rcb.Enabled = AgregarTabsGeneral.HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                    break;

                            }

                        }
                    }


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
                pagFinal = _K_PAGINACION;
            }
            else
            {
                pagInicial = _K_PAGINACION * (pageIndex - 1) + 1;
                pagFinal = (_K_PAGINACION * pageIndex);
            }

            string sortExpression = "";
            string filterExpression = this.virtualGridControl.FilterDescriptors.Expression;

            if (sortExpression == string.Empty)
            {
                sortExpression = "IDARTICULO ASC";
            }

            if (filterExpression != null && filterExpression != string.Empty)
            {
                filterExpression = " WHERE " + filterExpression;
                numRegistrosFiltrados = Business.GetArticulosRegistrosFiltrados(filterExpression);
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;

            }
            else
            {
                numRegistrosFiltrados = Business.GetArticulosRegistrosFiltrados("");
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;
            }

            
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;

            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetArticulosDatos(sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
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
            /*if (GridView.FilterDescriptors.Expression != null && GridView.FilterDescriptors.Expression != string.Empty)
            {
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetArticulosRegistrosFiltrados(" WHERE " + GridView.FilterDescriptors.Expression).ToString();
            }*/
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

        public void AceptarConfigurarArticulos()
        {
            DialogResult = DialogResult.OK;

            AckResponse ack = EditData(newRecord);
            RadMessageBox.Show(ack.Mensaje);
            if (ack.Resultado == "OK")
            {
                ElegirGrid();

                //llenarTabs();
            }
        }
        //Acción para crear un Proveedor
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaArticulo(_lstEsquemaTabla, selectedRow);
        }

        //Acción para editar un Proveedor
        protected override AckResponse EditData(dynamic selectedRow)
        {
            
            return Business.EditarArticulo(_lstEsquemaTabla, selectedRow);
        }

        public AckResponse EditDataArticulosConfiguracion(dynamic selectedRow, string where)
        {

            return Business.EditarArticulo(_lstEsquemaTabla, selectedRow,where);
        }

        //Acción para eliminar un Proveedor
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarArticulo(_lstEsquemaTabla, selectedRow);
        }

        

        #endregion


    }


}

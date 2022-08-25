using Rumbo.Core.Herramientas;
using RumboSGA.Presentation;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using static RumboSGA.FuncionesGenerales;

namespace RumboSGA.Controles
{
    public partial class VisorSQLRibbon : Telerik.WinControls.UI.RadRibbonForm
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string FiltroInicialVolatil = string.Empty;
        protected string nombreJson = string.Empty;
        protected RumGridView gridView = null;
        protected RadVirtualGrid virtualGrid = null;
        protected DataTable dt = null;
        protected DataTable dtVirtualGrid = null;
        protected DataTable dtFiltradoVirtual = null;
        private string nombreEstiloGridView = string.Empty;
        private string nombreEstiloVirtualGrid = string.Empty;
        private RadLabelElement lblCantidad = null;
        private RadLabelElement lblVistaSeleccionada = null;
        protected RumRibbonBarGroup grupoVer;
        protected RumRibbonBarGroup grupoCrud;
        public RumDropDownButtonElement rbConfiguracion;
        protected List<TableScheme> lstEsquemaTabla;
        protected RumPageView rumPageView;
        protected dynamic selectedRow;
        protected string name = "";
        protected bool editable = false;
        protected GridScheme esquemGrid;
        protected Hashtable diccParamNuevo = new Hashtable();
        protected RibbonTab rtAcciones;
        protected BackgroundWorker backGroudWorkerDescargaDatos;
        private int numeroRegistros = -1;
        private bool EnterPress = false;
        private List<FilterDescriptor> filtrado = new List<FilterDescriptor>();
        private List<SortDescriptor> ordenado = new List<SortDescriptor>();
        private RadWaitingBar radWaitingBar = null;
        private bool traducirDatos;
        private string tablaPrincipalJson, campoPrincipalJson;
        private int contadorListaTipos = 0;
        private List<tipoDato> listaString = new List<tipoDato>();
        private RumButtonElement editar = new RumButtonElement();
        private Panel panelDatos;
        private TableLayoutControlCollection guardarControl;

        public enum Estados
        {
            VCTodoGrid,
            VCTodoDetalle,
            VCHibrido
        };

        public enum GridOpciones
        {
            VirtualGrid,
            GridView
        };

        /**El parametro _editable, le dice al form si cargar tanto los eventos click como las
         * opciones superiores de nuevo, editar, clonar, eliminar.
         *
         */

        public VisorSQLRibbon(String _name, String nombreJson, bool _editable, bool traducirDatos, string wherePersonalizado = "", string tablaPrincipalJson = "", string campoPrincipalJson = "")
        {
            InitializeComponent();
            this.name = _name;
            this.tablaPrincipalJson = tablaPrincipalJson;
            this.campoPrincipalJson = campoPrincipalJson;
            this.nombreJson = nombreJson;
            this.editable = _editable;
            this.WindowState = FormWindowState.Maximized;
            cargarOpcionesRibbonBar();
            this.Text = Lenguaje.traduce(name);
            this.radRibbonBar1.Text = Lenguaje.traduce(name);
            this.radRibbonBar1.RibbonBarElement.RibbonCaption.Text = Lenguaje.traduce(name);
            this.AllowAero = false;
            radRibbonBar1.RibbonBarElement.CaptionFill.BackColor = Color.White;
            cargarVistas(VisorSQLRibbon.Estados.VCTodoGrid);
            this.Name = "VisorSQLRibbon" + name;
            this.nombreEstiloGridView = this.Name + "GridView.xml";
            this.nombreEstiloVirtualGrid = this.Name + "VirtualGrid.xml";
            this.traducirDatos = traducirDatos;
            FiltroInicialVolatil = wherePersonalizado;
            loadData();
            // listaTipoDatoTabla();
            this.Show();
        }

        #region ElementosVisuales

        private void RadVirtualGrid1_CellEditorInitialized(object sender, VirtualGridCellEditorInitializedEventArgs e)
        {
            if (e.RowIndex == RadVirtualGrid.FilterRowIndex)
            {
                VirtualGridTextBoxEditor editor = e.ActiveEditor as VirtualGridTextBoxEditor;
                RadTextBoxEditorElement el = editor.EditorElement as RadTextBoxEditorElement;
                el.KeyDown -= El_KeyDown;
                el.KeyDown += El_KeyDown;
            }
        }

        private void El_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                filtradoVirtualGrid();
            }
        }

        private void radGridView1_FilterChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            if (!EnterPress)
            {
                e.Cancel = true;
            }
            EnterPress = false;
        }

        private void radGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.Row is GridViewFilteringRowInfo)
            {
                if (e.ActiveEditor is RadTextBoxEditor)
                {
                    RadTextBoxEditor ed = e.ActiveEditor as RadTextBoxEditor;
                    RadTextBoxEditorElement el = ed.EditorElement as RadTextBoxEditorElement;
                    el.KeyDown -= el_KeyDown;
                    el.KeyDown += el_KeyDown;
                }
                else if (e.ActiveEditor is GridSpinEditor)
                {
                    GridSpinEditor ed = e.ActiveEditor as GridSpinEditor;
                    //ed.EditorElement
                    RadSpinEditorElement el = ed.EditorElement as RadSpinEditorElement;
                    el.KeyDown -= el_KeyDown;
                    el.KeyDown += el_KeyDown;
                }
                else if (e.ActiveEditor is RadDateTimeEditor)
                {
                    RadDateTimeEditor ed = e.ActiveEditor as RadDateTimeEditor;
                    //ed.EditorElement
                    RadDateTimeEditorElement el = ed.EditorElement as RadDateTimeEditorElement;
                    el.KeyDown -= el_KeyDown;
                    el.KeyDown += el_KeyDown;
                }
            }
        }

        private void el_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EnterPress = true;
            }
        }

        #endregion ElementosVisuales

        protected virtual void cargarOpcionesRibbonBar()
        {
            radRibbonBar1.CommandTabs.Clear();
            rtAcciones = new RibbonTab();
            if (editable)
            {
                grupoCrud = new RumRibbonBarGroup();

                RumButtonElement nuevo = new RumButtonElement();
                nuevo.Click += Nuevo_Click;
                nuevo.Text = "Nuevo";
                nuevo.Image = Properties.Resources.Add;
                nuevo.ImageAlignment = ContentAlignment.MiddleCenter;
                nuevo.TextAlignment = ContentAlignment.BottomCenter;
                grupoCrud.Items.Add(nuevo);

                editar.Click += Editar_Click;
                editar.Text = "Editar";
                editar.Image = Properties.Resources.edit;
                editar.ImageAlignment = ContentAlignment.MiddleCenter;
                editar.TextAlignment = ContentAlignment.BottomCenter;

                grupoCrud.Items.Add(editar);

                // editar.Enabled = false;

                RumButtonElement eliminar = new RumButtonElement();
                eliminar.Click += Eliminar_Click;
                eliminar.Text = "Eliminar";
                eliminar.Image = Properties.Resources.Delete;
                eliminar.ImageAlignment = ContentAlignment.MiddleCenter;
                eliminar.TextAlignment = ContentAlignment.BottomCenter;
                grupoCrud.Items.Add(eliminar);

                RumButtonElement clonar = new RumButtonElement();
                clonar.Click += Clonar_Click;
                clonar.Text = "Clonar";
                clonar.Image = Properties.Resources.copy;
                clonar.ImageAlignment = ContentAlignment.MiddleCenter;
                clonar.TextAlignment = ContentAlignment.BottomCenter;
                grupoCrud.Items.Add(clonar);

                rtAcciones.Items.Add(grupoCrud);
            }
            grupoVer = new RumRibbonBarGroup();
            grupoVer.Text = "Ver";

            RumRibbonBarGroup grupoConfiguracion = new RumRibbonBarGroup();
            grupoConfiguracion.Text = "Configuración";

            RumButtonElement quitarFiltrosVer = new RumButtonElement();
            quitarFiltrosVer.Click += QuitarFiltros_Click;
            quitarFiltrosVer.Text = "Quitar Filtros";
            quitarFiltrosVer.Image = Properties.Resources.ClearFilter;
            quitarFiltrosVer.ImageAlignment = ContentAlignment.MiddleCenter;
            quitarFiltrosVer.TextAlignment = ContentAlignment.BottomCenter;
            grupoVer.Items.Add(quitarFiltrosVer);

            RumButtonElement exportarExcelVer = new RumButtonElement();
            exportarExcelVer.Click += ExportarExcel_Click;
            exportarExcelVer.Text = "Exportar a Excel";
            exportarExcelVer.Image = Properties.Resources.ExportToExcel;
            exportarExcelVer.ImageAlignment = ContentAlignment.MiddleCenter;
            exportarExcelVer.TextAlignment = ContentAlignment.BottomCenter;
            grupoVer.Items.Add(exportarExcelVer);

            RumButtonElement refrescarVer = new RumButtonElement();
            refrescarVer.Click += Refrescar_Click;
            refrescarVer.Text = "Refrescar";
            refrescarVer.Image = Properties.Resources.Refresh;
            refrescarVer.ImageAlignment = ContentAlignment.MiddleCenter;
            refrescarVer.TextAlignment = ContentAlignment.BottomCenter;
            grupoVer.Items.Add(refrescarVer);

            RumButtonElement cambiarVistaVer = new RumButtonElement();
            cambiarVistaVer.Click += cambiarVista_Click;
            cambiarVistaVer.Text = "Cambiar Vista";
            cambiarVistaVer.Image = Properties.Resources.Cambiar;
            cambiarVistaVer.ImageAlignment = ContentAlignment.MiddleCenter;
            cambiarVistaVer.TextAlignment = ContentAlignment.BottomCenter;
            grupoVer.Items.Add(cambiarVistaVer);

            rtAcciones.Items.Add(grupoVer);

            rbConfiguracion = new RumDropDownButtonElement();
            rbConfiguracion.Text = "Configuración";
            rbConfiguracion.TextAlignment = ContentAlignment.BottomCenter;
            rbConfiguracion.ImageAlignment = ContentAlignment.MiddleCenter;

            RumMenuItem btnTemas = new RumMenuItem();
            btnTemas.Text = "Temas";

            RumMenuItem btnGuardarEstilo = new RumMenuItem();
            btnGuardarEstilo.Text = "Guardar Estilo";
            btnGuardarEstilo.Click += SaveItem_Click;

            RumMenuItem btnCargarEstilo = new RumMenuItem();
            btnCargarEstilo.Text = "Cargar Estilo";
            btnCargarEstilo.Click += LoadItem_Click;

            RumMenuItem editColumns = new RumMenuItem();
            editColumns.Text = "Editar Estilo";
            editColumns.Click += ItemColumnas_Click;

            RumMenuItem filtroInicial = new RumMenuItem();
            filtroInicial.Text = "Filtro Inicial";
            filtroInicial.Click += FiltroInicial_Click;

            FuncionesGenerales.AddEliminarLayoutButton(ref rbConfiguracion);
            if (rbConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
            {
                rbConfiguracion.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                {
                    if (panel.Controls.Contains(gridView))
                    {
                        FuncionesGenerales.EliminarLayout(this.nombreEstiloGridView, gridView);
                        gridView.Refresh();
                    }
                    else if (panel.Controls.Contains(virtualGrid))
                    {
                        FuncionesGenerales.EliminarLayout(this.nombreEstiloVirtualGrid, null);
                        virtualGrid.Refresh();
                    }
                });
            }

            rbConfiguracion.Items.Add(btnGuardarEstilo);
            rbConfiguracion.Items.Add(btnCargarEstilo);
            rbConfiguracion.Items.Add(editColumns);
            rbConfiguracion.Items.Add(filtroInicial);
            rbConfiguracion.Image = Properties.Resources.Administration;
            grupoConfiguracion.Items.Add(rbConfiguracion);

            lblCantidad = new RadLabelElement();
            grupoConfiguracion.Items.Add(lblCantidad);

            lblVistaSeleccionada = new RadLabelElement();
            grupoConfiguracion.Items.Add(lblVistaSeleccionada);

            rtAcciones.Items.Add(grupoConfiguracion);
            radRibbonBar1.CommandTabs.Add(rtAcciones);
        }

        private void Clonar_Click(object sender, EventArgs e)
        {
            abrirSFDetallesClonar();
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            borrarRegistro();
        }

        private void Editar_Click(object sender, EventArgs e)
        {
            abrirSFDetallesEditar();
        }

        private void Nuevo_Click(object sender, EventArgs e)
        {
            abrirSFDetallesNuevo();
        }

        public void Refrescar_Click(object sender, EventArgs e)
        {
            loadData();
        }

        public void ExportarExcel_Click(object sender, EventArgs e)
        {
            ExportarExcel();
        }

        public void FiltroInicial_Click(object sender, EventArgs e)
        {
            FiltroInicialForm fi = new FiltroInicialForm(nombreJson, Modos.VisorSql);
            fi.ShowDialog();
        }

        protected void cargarVistas(Enum Estado_)
        {
            switch (Estado_)
            {
                case Estados.VCHibrido:
                    this.tablaPrincipal.RowStyles[0].Height = 60;
                    this.tablaPrincipal.RowStyles[1].Height = 30;
                    this.tablaPrincipal.Controls.Remove(panelDatos);
                    if (guardarControl != null)
                    {
                        this.tablaPrincipal.Controls.Add(panel);
                        loadData();
                    }

                    if (rumPageView != null)
                        rumPageView.ViewMode = PageViewMode.Strip;
                    break;

                case Estados.VCTodoDetalle:
                    if (selectedRow == null)
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Selecciona una línea"));
                    }
                    else
                    {
                        RumCargaTablaDetalle cargaTablaDetalle = new RumCargaTablaDetalle();
                        this.tablaPrincipal.Dock = DockStyle.Fill;
                        guardarControl = this.tablaPrincipal.Controls;
                        this.tablaPrincipal.Controls.Clear();
                        this.tablaPrincipal.RowStyles[0].Height = 50;
                        this.tablaPrincipal.RowStyles[1].Height = 0;
                        //
                        // panelDatos
                        //
                        this.panelDatos = new System.Windows.Forms.Panel();

                        panelDatos.Dock = System.Windows.Forms.DockStyle.Fill;
                        panelDatos.Name = "panelDatos";
                        panelDatos.TabIndex = 1;
                        panelDatos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                        panelDatos.AutoScroll = true;

                        panelDatos = cargaTablaDetalle.CargaTablaDetalle(name, lstEsquemaTabla, selectedRow, false, false, tablaPrincipalJson);
                        this.tablaPrincipal.Controls.Add(panelDatos);
                        this.panelDatos.BringToFront();
                        this.tablaPrincipal.BringToFront();
                    }
                    break;

                case Estados.VCTodoGrid:
                    this.tablaPrincipal.RowStyles[0].Height = 50;
                    this.tablaPrincipal.RowStyles[1].Height = 0;

                    if (guardarControl != null)
                    {
                        this.tablaPrincipal.Controls.Remove(rumPageView);
                        this.tablaPrincipal.Controls.Remove(panelDatos);

                        this.tablaPrincipal.Controls.Add(panel);
                        loadData();
                        this.tablaPrincipal.Enabled = true;
                        this.tablaPrincipal.Visible = true;
                    }
                    if (rumPageView != null)
                        rumPageView.ViewMode = PageViewMode.Strip;

                    this.tablaPrincipal.BringToFront();

                    break;
            }
        }

        private void cargarGridView()
        {
            if (gridView == null)
            {
                gridView = new RumGridView();
                gridView.Dock = DockStyle.Fill;
                gridView.ReadOnly = true;
                gridView.EnableFiltering = true;
                gridView.AllowAddNewRow = false;
                gridView.FilterChanging += radGridView1_FilterChanging;
                gridView.CellBeginEdit += radGridView1_CellBeginEdit;
                gridView.FilterChanged += gridView_FilterChanged;
                gridView.SelectionMode = GridViewSelectionMode.CellSelect;

                if (this.editable)
                {
                    gridView.CellClick += gridView_CellClick;
                    gridView.CellDoubleClick += gridView_CellDoubleClick;
                }
            }
            this.gridView.DataSource = null;
        }

        private void gridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if (gridView.ChildRows.Count > Persistencia.NumRegistrosVirtualGrid)
            {
                alternarGrid(GridOpciones.VirtualGrid);
            }
        }

        private void insertarGridView()
        {
            this.lblVistaSeleccionada.Text = Lenguaje.traduce("Vista Avanzada");
            this.panel.Controls.Remove(virtualGrid);
            this.panel.Controls.Add(gridView);
            gridView.BestFitColumns();
        }

        private void insertarVirtualGrid()
        {
            this.lblVistaSeleccionada.Text = Lenguaje.traduce("Vista Rapida");
            this.panel.Controls.Remove(gridView);
            this.panel.Controls.Add(virtualGrid);
            virtualGrid.BestFitColumns();
        }

        private void gridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            abrirSFDetallesEditar();
        }

        private void cargarVirtualGrid()
        {
            if (virtualGrid == null)
            {
                virtualGrid = new RadVirtualGrid();

                virtualGrid.Dock = DockStyle.Fill;
                virtualGrid.AllowAddNewRow = false;
                virtualGrid.AllowFiltering = true;
                virtualGrid.AllowEdit = false;
                virtualGrid.CellValueNeeded += radVirtualGrid1_CellValueNeeded;
                virtualGrid.FilterChanged += radVirtualGrid1_FilterChanged;
                virtualGrid.SortChanged += radVirtualGrid1_SortChanged;
                virtualGrid.ContextMenuOpening += Remove_ContextMenuOpening;
                virtualGrid.BestFitColumns();
                virtualGrid.SelectionMode = VirtualGridSelectionMode.CellSelect;
                virtualGrid.CellEditorInitialized += RadVirtualGrid1_CellEditorInitialized;
                //listaTipoDatoTabla();
                contadorListaTipos = 0;
                virtualGrid.CellFormatting += radVirtualGrid1_CellFormatting;

                if (this.editable)
                {
                    virtualGrid.CellClick += virtualGrid_CellClick;
                    virtualGrid.CellDoubleClick += virtualGrid_CellDoubleClick;
                }
            }
            this.virtualGrid.RowCount = 0;
        }

        private void radVirtualGrid1_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
        {
            if (dtFiltradoVirtual == null) return;
            if (e.ColumnIndex < 0)
            {
                return;
            }
            if (e.RowIndex == RadVirtualGrid.HeaderRowIndex)
            {
                e.Value = dtFiltradoVirtual.Columns[e.ColumnIndex].ColumnName;
            }

            if (e.RowIndex < 0)
            {
                //columnNames[e.ColumnIndex];
                e.FieldName = dtFiltradoVirtual.Columns[e.ColumnIndex].ColumnName;

                log.Info(e.Value + "  Nombre :" + e.FieldName);
            }

            if (e.RowIndex >= 0 && e.RowIndex < dt.Rows.Count)
            {
                //e.Value = data[e.RowIndex][e.ColumnIndex];
                e.Value = dtFiltradoVirtual.Rows[e.RowIndex][e.ColumnIndex];
            }
        }

        private void Remove_ContextMenuOpening(object sender, VirtualGridContextMenuOpeningEventArgs e)
        {
            quitarOpcionesMenuContext(ref e);
        }

        private void quitarOpcionesMenuContext(ref VirtualGridContextMenuOpeningEventArgs e)
        {
            if (e == null || e.ContextMenu == null || e.ContextMenu.Items == null)
            {
                log.Debug("El RadVirtualGrid es null cuando llega a la función quitarOpcionesMenuContext");
                return;
            }
            if (e.RowIndex < 0) return;

            RadItem item = e.ContextMenu.Items[0];

            e.ContextMenu.Items.Clear();

            e.ContextMenu.Items.Add(item);
        }

        private void radVirtualGrid1_SortChanged(object sender, VirtualGridEventArgs e)
        {
            filtradoVirtualGrid();
        }

        private void radVirtualGrid1_FilterChanged(object sender, VirtualGridEventArgs e)
        {
        }

        private void filtradoVirtualGrid()
        {
            if (dtVirtualGrid == null) return;

            string ex = virtualGrid.FilterDescriptors.Expression;
            string sort = virtualGrid.SortDescriptors.Expression;
            dtFiltradoVirtual.Rows.Clear();
            if (ex != "")
            {
                listaTipoDatoTabla();
                foreach (var item in listaString)
                {
                    if (item.nombre == virtualGrid.FilterDescriptors[0].PropertyName.ToString())
                    {
                        if (item.tipo == 1)
                        {
                            ex = "[" + virtualGrid.FilterDescriptors[0].PropertyName.ToString() + "] = " + virtualGrid.FilterDescriptors[0].Value.ToString();
                        }
                    }
                }
            }
            DataRow[] dr = dtVirtualGrid.Select(ex, sort);

            log.Info(ex);

            if (dr.Length > 0)
                dtFiltradoVirtual = dr.CopyToDataTable();

            virtualGrid.RowCount = dr.Length;
            virtualGrid.TableElement.SynchronizeRows();
            if (dtFiltradoVirtual.Rows.Count < Persistencia.NumRegistrosVirtualGrid)
            {
                alternarGrid(GridOpciones.GridView);
                return;
            }

            lblCantidad.Text = "     " + Lenguaje.traduce("Registros: ") + dr.Length;
        }

        private void cambiarVista_Click(object sender, EventArgs e)
        {
            if (this.panel.Controls.Contains(gridView))
                alternarGrid(GridOpciones.VirtualGrid);
            else if (this.panel.Controls.Contains(virtualGrid))
                alternarGrid(GridOpciones.GridView);
        }

        protected void alternarGrid(GridOpciones go)
        {
            if (go == GridOpciones.VirtualGrid)
            {
                insertarVirtualGrid();
                this.virtualGrid.FilterDescriptors.Clear();
                this.virtualGrid.SortDescriptors.Clear();

                if (gridView.FilterDescriptors.Count > 0)
                {
                    filtrado.Clear();
                    foreach (FilterDescriptor item in gridView.FilterDescriptors)
                    {
                        filtrado.Add(item);
                    }

                    this.virtualGrid.FilterDescriptors.AddRange(filtrado);
                }
                if (gridView.SortDescriptors.Count > 0)
                {
                    ordenado.Clear();
                    foreach (SortDescriptor item in gridView.SortDescriptors)
                    {
                        ordenado.Add(item);
                    }
                    this.virtualGrid.SortDescriptors.AddRange(ordenado);
                }
                if (gridView.FilterDescriptors.Count == 0 && gridView.SortDescriptors.Count == 0)
                {
                    virtualGrid.RowCount = dtVirtualGrid.Rows.Count;
                    virtualGrid.ColumnCount = dtVirtualGrid.Columns.Count;
                    virtualGrid.TableElement.SynchronizeRows();
                }
                int a = virtualGrid.ColumnCount;
                log.Info("Info columna " + a);
            }
            else if (go == GridOpciones.GridView)
            {
                //if (dt.Rows.Count > Persistencia.NumRegistrosVirtualGrid)
                //{
                //    String mensaje = Lenguaje.traduce("Atención, hay muchos registros para la vista normal, ¿estas seguro de querer continuar?");
                //    if (RadMessageBox.Show(this, mensaje, Lenguaje.traduce("Confirmación"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                //    {
                //        RadMessageBox.Show(Lenguaje.traduce("Cancelando!"));
                //        return;
                //    }

                //}
                insertarGridView();
                gridView.DataSource = dt;
                int a = gridView.ColumnCount;
                log.Info("Info columna " + a);
                if (virtualGrid.FilterDescriptors.Count > 0)
                {
                    filtrado.Clear();
                    foreach (FilterDescriptor item in virtualGrid.FilterDescriptors)
                    {
                        filtrado.Add(item);
                    }
                    this.gridView.FilterDescriptors.Clear();
                    this.gridView.FilterDescriptors.AddRange(filtrado);
                }

                if (virtualGrid.SortDescriptors.Count > 0)
                {
                    ordenado.Clear();
                    foreach (SortDescriptor item in virtualGrid.SortDescriptors)
                    {
                        ordenado.Add(item);
                    }
                    this.gridView.SortDescriptors.Clear();
                    this.gridView.SortDescriptors.AddRange(ordenado);
                }
            }

            ElegirEstilo();
        }

        protected void getGridViewRow()
        {
            if (panel.Controls.Contains(virtualGrid))
            {
                selectedRow = FuncionesGenerales.getRowGridView(virtualGrid, lstEsquemaTabla, dtFiltradoVirtual, dtVirtualGrid, dt);
            }
            else if (panel.Controls.Contains(gridView))
            {
                selectedRow = FuncionesGenerales.getRowGridView(gridView, lstEsquemaTabla, dtFiltradoVirtual, dtVirtualGrid, dt);
            }
        }

        private void virtualGrid_CellDoubleClick(object sender, EventArgs e)
        {
            if (e is VirtualGridCellElementEventArgs)
            {
                if ((e as VirtualGridCellElementEventArgs).CellElement.RowIndex < 0) return;
            }

            abrirSFDetallesEditar();
        }

        protected virtual void virtualGrid_CellClick(object sender, VirtualGridCellElementEventArgs e)
        {
            if (e.CellElement.RowIndex > -1)
                selectedRow = FuncionesGenerales.getRowGridView(virtualGrid, lstEsquemaTabla, dtFiltradoVirtual, dtVirtualGrid, dt);
        }

        protected virtual void gridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                selectedRow = FuncionesGenerales.getRowGridView(gridView, lstEsquemaTabla, dtFiltradoVirtual, dtVirtualGrid, dt);
        }

        #region CRUD

        private void abrirSFDetallesNuevo()
        {
            try
            {
                var dialog = new SFDetalle(name, lstEsquemaTabla, selectedRow, true, false, tablaPrincipalJson);

                //var dialog = new SFDetalles(this.name, lstEsquemaTabla, selectedRow, SFDetalles.modoForm.nuevo, esquemGrid, diccParamNuevo);

                dialog.Text = "Detalle";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AckResponse ack = NewData(dialog.newRecord);
                    RadMessageBox.Show(ack.Mensaje);
                    if (ack.Resultado == "OK")
                    {
                        log.Debug("El usuario " + User.IdUsuario + " ha creado en el formulario " + this.name + " el registro " + selectedRow);
                        //loadData();
                        cargarLinea(enumLineaOpciones.add, dialog.newRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void abrirSFDetallesClonar()
        {
            if (this.selectedRow == null)
            {
                getGridViewRow();
            }
            if (selectedRow == null)
            {
                RadMessageBox.Show(Lenguaje.traduce("Selecciona una fila"));
            }
            var dialog = new SFDetalle(name, lstEsquemaTabla, selectedRow, false, true, tablaPrincipalJson);
            dialog.Text = Lenguaje.traduce("Detalle");
       //     dialog.ComprobarValores = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                AckResponse ack = NewData(dialog.newRecord);
                RadMessageBox.Show(ack.Mensaje);
                if (ack.Resultado == "OK")
                {
                    log.Debug("El usuario " + User.IdUsuario + " ha creado en el formulario " + this.name + " el registro " + selectedRow);
                    //loadData();
                    cargarLinea(enumLineaOpciones.add, dialog.newRecord);
                }
            }
        }

        protected virtual void abrirSFDetallesEditar()
        {
            try
            {
                if (this.selectedRow == null)
                {
                    getGridViewRow();
                }
                if (selectedRow == null)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Selecciona una fila"));
                }
                var dialog = new SFDetalles(this.name, lstEsquemaTabla, selectedRow, SFDetalles.modoForm.lectura, esquemGrid, diccParamNuevo);
                dialog.AutoSize = true;
                //dialogN.ComprobarValores = true;
                dialog.AutoSizeMode = AutoSizeMode.GrowOnly;
                dialog.Text = Lenguaje.traduce("Detalle");
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.editado == true)
                    {
                        AckResponse ack = EditData(dialog.newRecord);
                        RadMessageBox.Show(ack.Mensaje);
                        if (ack.Resultado == "OK")
                        {
                            //loadData();
                            cargarLinea(enumLineaOpciones.edit, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }



        //Metodo para cargar una línea editada o una nueva sin necesidad de consultar en la bbdd
        protected void cargarLinea(enumLineaOpciones opcion, dynamic lineaNueva)
        {
            try
            {
                //TODO: MEJORAR ESTOS CAMPOS PARA QUE LOS LEA DE LOS JSON. AHORA SE HACE ASÍ PARA RESOLVER DRUNI RAPIDAMENTE
                if (campoPrincipalJson.Equals(string.Empty) || tablaPrincipalJson.Equals(string.Empty))
                {
                    loadData();
                    return;
                }

                string filtroWhere = " " + campoPrincipalJson + " = (SELECT TOP 1 " + campoPrincipalJson + " FROM " + tablaPrincipalJson + " ORDER BY " + campoPrincipalJson + " DESC)";
                switch (opcion)
                {
                    case enumLineaOpciones.edit:
                        FuncionesGenerales.DataTableLineaEdit(ref dt, selectedRow, lstEsquemaTabla, nombreJson);
                        break;

                    case enumLineaOpciones.add:
                        //FuncionesGenerales.DataTableLineaAdd(ref dt, lineaNueva, lstEsquemaTabla, nombreJson, filtroWhere);
                        FuncionesGenerales.DataTableLineaAdd(ref dt, selectedRow, lstEsquemaTabla, nombreJson, filtroWhere);
                        break;

                    case enumLineaOpciones.delete:
                        FuncionesGenerales.DataTableLineaDelete(ref dt, selectedRow, lstEsquemaTabla, nombreJson);
                        break;
                }
                sincronizarDataTables();
                if (virtualGrid != null)
                    virtualGrid.TableElement.SynchronizeRows();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void radVirtualGrid1_CellFormatting(object sender, Telerik.WinControls.UI.VirtualGridCellElementEventArgs e)
        {
            if (listaString.Count == 0)
            {
                listaTipoDatoTabla();
            }
            if (e.CellElement.ColumnIndex >= 0)
            {
                if (e.CellElement.ColumnIndex == contadorListaTipos && listaString[contadorListaTipos].tipo == 0)
                {
                    e.CellElement.TextAlignment = ContentAlignment.MiddleLeft;
                }
                else
                {
                    e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                }
                contadorListaTipos++;
            }
            else
            {
                contadorListaTipos = 0;
            }
        }

        private void listaTipoDatoTabla()
        {
            if (dtFiltradoVirtual != null)
            {
                foreach (DataColumn item in dtFiltradoVirtual.Columns)
                {
                    tipoDato tipo = new tipoDato();
                    tipo.nombre = item.ColumnName;

                    if (item.DataType.ToString() != "System.String")
                    {
                        tipo.tipo = 1;
                        listaString.Add(tipo);
                    }
                    else
                    {
                        tipo.tipo = 0;
                        listaString.Add(tipo);
                    }
                }
            }
            //contadorListaTipos = 0;
        }

        private void borrarRegistro()
        {
            if (this.selectedRow == null)
            {
                getGridViewRow();
            }
            if (selectedRow == null)
            {
                RadMessageBox.Show(Lenguaje.traduce("Selecciona una fila"));
            }
            DialogResult ds = RadMessageBox.Show(this, Lenguaje.traduce("¿Desea eliminar el registro seleccionado?"), Lenguaje.traduce("Eliminar"), MessageBoxButtons.YesNo, RadMessageIcon.Question);
            if (ds == DialogResult.No)
            {
                return;
            }
            AckResponse ack = DeleteData(selectedRow);
            RadMessageBox.Show(Lenguaje.traduce(ack.Mensaje));
            if (ack.Resultado == "OK")
            {
                log.Debug("El usuario " + User.IdUsuario + " ha borrado en el formulario " + this.name + " el registro " + selectedRow);
                //loadData();
                cargarLinea(enumLineaOpciones.delete, null);
            }
        }

        protected AckResponse EditData(dynamic rowNueva)
        {
            AckResponse ack = FuncionesGenerales.jsonUpdate(nombreJson, rowNueva, FuncionesGenerales.ComponerJson(lstEsquemaTabla, selectedRow));

            String resultado = FuncionesEspecialesRumControlGeneral.funcionesEspecialesDespuesEditar(
                nombreJson, rowNueva, ack);

            if (resultado.Equals(""))
            {
            }
            else if (resultado.Equals("KO"))
            {
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(resultado));
            }

            return ack;
        }

        protected AckResponse DeleteData(dynamic selectedRow)
        {
            return RumboSGA.FuncionesGenerales.jsonDelete(nombreJson, RumboSGA.FuncionesGenerales.ComponerJson(lstEsquemaTabla, this.selectedRow));
        }

        protected AckResponse NewData(dynamic selectedRow)
        {
            return FuncionesGenerales.jsonInsert(nombreJson, selectedRow);
        }

        #endregion CRUD

        #region Estilos

        public void ExportarExcel()
        {
            try
            {
                if (this.panel.Controls.Count < 1 || !panel.Controls.Contains(gridView))
                {
                    RadMessageBox.Show(Lenguaje.traduce("Opción solo disponible en vista normal"));
                    return;
                }
                FuncionesGenerales.exportarAExcelGenerico(this.gridView);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al exportar Excel en VisorSQLRibbon");
            }
        }

        public void QuitarFiltros_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón quitar filtro " + DateTime.Now);
                if (panel.Controls.Count < 1) return;

                if (panel.Contains(gridView))
                {
                    EnterPress = true;
                    if (gridView.IsInEditMode)
                    {
                        gridView.EndEdit();
                    }
                    gridView.FilterDescriptors.Clear();
                    gridView.GroupDescriptors.Clear();
                    gridView.SortDescriptors.Clear();
                    gridView.BestFitColumns();
                }
                else if (panel.Controls.Contains(virtualGrid))
                {
                    if (virtualGrid.IsInEditMode)
                    {
                        virtualGrid.EndEdit();
                    }
                    EnterPress = true;
                    virtualGrid.FilterDescriptors.Clear();
                    virtualGrid.SortDescriptors.Clear();
                    virtualGrid.BestFitColumns();
                    virtualGrid.TableElement.SynchronizeRows();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public virtual void SaveItem_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón guardar estilo " + DateTime.Now);
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

        public void SaveLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;/*XmlReaderPropio.getLayoutPath(0);*/
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                Directory.CreateDirectory(path);
                path += "\\";
                path.Replace(" ", "_");
                if (panel.Controls.Contains(gridView))
                {
                    gridView.SaveLayout(path + this.nombreEstiloGridView);
                }
                else if (panel.Controls.Contains(virtualGrid))
                {
                    virtualGrid.SaveLayout(path + this.nombreEstiloVirtualGrid);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce("No se ha podido encontrar el archivo en la dirección") + ":" + path + "\n" + Lenguaje.traduce("Puede cambiar esta dirección en el archivo PathLayouts.xml"));
            }
        }

        public void SaveLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal;/*XmlReaderPropio.getLayoutPath(1);*/
            int pathGL = 1;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                Directory.CreateDirectory(path);
                path += "\\";
                path.Replace(" ", "_");
                if (panel.Controls.Contains(gridView))
                {
                    gridView.SaveLayout(path + this.nombreEstiloGridView);
                }
                else if (panel.Controls.Contains(virtualGrid))
                {
                    virtualGrid.SaveLayout(path + this.nombreEstiloVirtualGrid);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce("No se ha podido encontrar el archivo en la dirección") + ":" + path + "\n" + Lenguaje.traduce("Puede cambiar esta dirección en el archivo PathLayouts.xml"));
            }
        }

        public virtual void LoadItem_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón cargar estilo " + DateTime.Now);
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

        public void LoadLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;
            int pathGL = 0;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                string s = path + "\\";
                s.Replace(" ", "_");
                if (panel.Controls.Contains(gridView))
                {
                    gridView.LoadLayout(s + this.nombreEstiloGridView);
                }
                else if (panel.Controls.Contains(virtualGrid))
                {
                    virtualGrid.LoadLayout(s + this.nombreEstiloVirtualGrid);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void LoadLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal;
            int pathGL = 1;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                string s = path + "\\";
                s.Replace(" ", "_");
                if (panel.Controls.Contains(gridView))
                {
                    gridView.LoadLayout(s + this.nombreEstiloGridView);
                }
                else if (panel.Controls.Contains(virtualGrid))
                {
                    virtualGrid.LoadLayout(s + this.nombreEstiloVirtualGrid);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void ItemColumnas_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón editar columnas " + DateTime.Now);
            if (panel.Controls.Contains(gridView))
            {
                gridView.ShowColumnChooser();
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce("Opción solo disponible en vista normal"));
            }
        }

        public virtual void ElegirEstilo()
        {
            try
            {
                if (name == "Artículos")
                {
                    string estilosInicio = "VisorSQLRibbonArtículosInicio.xml";
                    //   nombreEstiloVirtualGrid = "VisorSQLRibbonArtículosInicio.xml";
                    //   nombreEstiloGridView = "VisorSQLRibbonArtículosInicio.xml";
                }
                if (panel.Controls.Count < 1) return;
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
                if (panel.Controls.Contains(virtualGrid))
                {
                    string pathVirtual = pathLocal + "\\" + nombreEstiloVirtualGrid;
                    bool existsVirtual = File.Exists(pathVirtual);
                    if (existsVirtual)
                    {
                        virtualGrid.LoadLayout(pathVirtual);
                    }
                    else
                    {
                        pathVirtual = pathGlobal + "\\" + nombreEstiloVirtualGrid;
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
                if (panel.Controls.Contains(gridView))
                {
                    string pathGridView = pathLocal + "\\" + nombreEstiloGridView;
                    bool existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        gridView.LoadLayout(pathGridView);
                    }
                    else
                    {
                        pathGridView = pathGlobal + "\\" + nombreEstiloGridView;
                        existsGridView = File.Exists(pathGridView);
                        if (existsGridView)
                        {
                            gridView.LoadLayout(pathGridView);
                        }
                        else
                        {
                            gridView.BestFitColumns();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error en ElegirEstilo() de BaseGridControl." + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Estilos

        protected virtual void loadData()
        {
            //Cargamos los grids vacíos de manera que no ocupan apenas memoria ni recursos hasta que no añadas datasource
            try
            {
                pantallaCargaShow();
                dt = null;
                numeroRegistros = FuncionesGenerales.CargarEsquemaYSacarCount(nombreJson, "", ref lstEsquemaTabla);
                cargarVirtualGrid();
                cargarGridView();
                descargarEsquemaGrid();

                if (backGroudWorkerDescargaDatos == null)
                {
                    crearWorkerDescargarDatos();
                }

                if (this.panel.Controls.Contains(gridView))
                {
                    insertarGridView();
                }
                else if (this.panel.Controls.Contains(virtualGrid))
                {
                    insertarVirtualGrid();
                }
                else if (this.numeroRegistros > Persistencia.NumRegistrosVirtualGrid)
                {
                    insertarVirtualGrid();
                }
                else
                {
                    insertarGridView();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            //El background tiene su propio gestor de errores.
            if (!String.IsNullOrEmpty(nombreJson))
            {
                backGroudWorkerDescargaDatos.RunWorkerAsync();
            }
            else return;
        }

        private void descargarEsquemaGrid()
        {
            try
            {
                this.esquemGrid = FuncionesGenerales.descargarJsonEsquemaGridAdjunto(nombreJson);
            }
            catch (Exception ex2)
            {
                ExceptionManager.GestionarErrorNuevo(ex2, "Error descargando el GridScheme:" + nombreJson);
            }
        }

        #region BackGroundWorker

        private void crearWorkerDescargarDatos()
        {
            backGroudWorkerDescargaDatos = new BackgroundWorker();
            backGroudWorkerDescargaDatos.DoWork += DescargaDatos_DoWork;
            backGroudWorkerDescargaDatos.RunWorkerCompleted += backGroudWorkerDescargaDatos_RunWorkerCompleted;
        }

        private void backGroudWorkerDescargaDatos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    log.Error("BackGroudWorker error: " + e.Error);
                }
                if (e.Result == null)
                {
                    log.Error("El runworker a devuelto null");
                    return;
                }
                dt = (e.Result as List<DataTable>)[0];
                //dtVirtualGrid = (e.Result as List<DataTable>)[1];
                dtVirtualGrid = dt;

                if (dt == null)
                {
                    RadMessageBox.Show(Lenguaje.traduce("No hay datos"));
                    log.Error("No hay datos en RunWorkerCompleted para mostrar, JSON:" + nombreJson);
                }

                // dtFiltradoVirtual = dtVirtualGrid.Copy();
                dtFiltradoVirtual = dt.Copy();

                lblCantidad.Text = "     " + Lenguaje.traduce("Registros: ") + dt.Rows.Count;
                //pantallaCargaClose();

                if (panel.Controls.Contains(gridView))
                {
                    gridView.DataSource = null;
                    alternarGrid(GridOpciones.GridView);
                    gridView.DataSource = dt;
                    int a = virtualGrid.ColumnCount;
                }
                else if (panel.Controls.Contains(virtualGrid))
                {
                    alternarGrid(GridOpciones.VirtualGrid);
                }
                contadorListaTipos = 0;

                ElegirEstilo();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en backgroundworker " + nombreJson);
            }
            finally
            {
                pantallaCargaClose();
            }
        }

        private void DescargaDatos_DoWork(object sender, DoWorkEventArgs e)
        {
            //Aquí empieza a cargar los datos, muy importante que los datos se administren desde aquí y desde el RunWorkerCompleted.
            List<DataTable> r = new List<DataTable>();
            DataTable dt1 = FuncionesGenerales.descargarJsonDatos(nombreJson, FiltroInicialVolatil);
            if (traducirDatos)
            {
                Utilidades.TraducirDataTableDatos(ref dt1);
            }
            Utilidades.TraducirDataTableColumnName(ref dt1);
            r.Add(dt1);
            DataTable dt2 = dt1.Clone();
            //dtVirtualGrid =
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dt2.Columns[i].DataType = typeof(String);
            }
            foreach (DataRow dtR in dt1.Rows)
            {
                dt2.ImportRow(dtR);
            }
            r.Add(dt2);
            e.Result = r;
        }

        private void sincronizarDataTables()
        {
            //dtVirtualGrid =
            dtVirtualGrid = new DataTable();
            dtVirtualGrid = dt.Clone();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dtVirtualGrid.Columns[i].DataType = typeof(String);
            }
            foreach (DataRow dtR in dt.Rows)
            {
                dtVirtualGrid.ImportRow(dtR);
            }
            dtFiltradoVirtual = dtVirtualGrid.Copy();
        }

        private void pantallaCargaShow()
        {
            if (radWaitingBar == null)
            {
                radWaitingBar = new RadWaitingBar();
                radWaitingBar.Location = new Point((panel.Width - radWaitingBar.Width) /
                    2, (panel.Height - radWaitingBar.Height) / 2);
                radWaitingBar.Size = new Size(200, 20);
                radWaitingBar.Anchor = AnchorStyles.None;
            }
            panel.Controls.Add(radWaitingBar);
            radWaitingBar.Visible = true;
            radWaitingBar.StartWaiting();
            radWaitingBar.BringToFront();
        }

        private void pantallaCargaClose()
        {
            if (this.radWaitingBar != null)
            {
                radWaitingBar.StopWaiting();
                radWaitingBar.Visible = false;
                panel.Controls.Remove(panel);
            }
        }

        #endregion BackGroundWorker

        private void VisorSQLRibbon_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (backGroudWorkerDescargaDatos.IsBusy)
                {
                    //No funciona
                    //backGroudWorkerDescargaDatos.CancelAsync();
                }
                gridView.Dispose();
                virtualGrid.Dispose();
                if (dt != null)
                    dt.Dispose();

                if (dtFiltradoVirtual != null)
                    dtFiltradoVirtual.Dispose();

                if (dtVirtualGrid != null)
                    dtVirtualGrid.Dispose();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        public class tipoDato
        {
            // Auto-implemented properties.
            public string nombre { get; set; }

            public int tipo { get; set; }

            public tipoDato()
            {
            }

            public tipoDato(string nombre, int tipo)
            {
                this.nombre = nombre;
                this.tipo = tipo;
            }
        }
    }
}
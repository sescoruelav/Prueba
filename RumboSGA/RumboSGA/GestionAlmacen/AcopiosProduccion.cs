using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using RumboSGAManager.Model.DataContext;
using System.Configuration;
using RumboSGAManager.Model.Entities;
using System.Data.SqlClient;
using Telerik.WinControls.Data;
using System.IO;
using System.Diagnostics;
using RumboSGA.ProduccionMotor;
using System.Globalization;
using Telerik.WinControls;
using Newtonsoft.Json;
using RumboSGA.Presentation;
using RumboSGAManager;
using RumboSGAManager.Model.Security;
using RumboSGAManager.Model;
using RumboSGA.Properties;
using System.Reflection;
using Telerik.WinControls.Analytics;
using RumboSGA.Presentation.UserControls;
using System.Dynamic;
using System.Runtime.InteropServices;
using RumboSGA.Herramientas.Stock;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.Presentation.Herramientas.PantallasWS;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.Herramientas.Ventanas;
using RumboSGA.Controles;
using Rumbo.Core.Herramientas.Herramientas;

namespace RumboSGA.Maestros
{
    public partial class AcopiosProduccion : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        private WSProduccionMotorClient WSProduccionMotorClient = new WSProduccionMotorClient();
        private WSOrdenProduccionMotorClient WSOrdenProduccionMotorClient = new WSOrdenProduccionMotorClient();

        private RadWaitingBar rWaitOrdenes = new RadWaitingBar();
        private RadWaitingBar rWaitArticulos = new RadWaitingBar();
        private BackgroundWorker bgWorkerGridViewOrdenes;
        private BackgroundWorker bgWorkerGridViewArticulos;
        private RadRibbonBarGroup grupoLabel = new RadRibbonBarGroup();
        private RadLabelElement lblCantidad = new RadLabelElement();
        private GridViewTemplate templateArticulos = new GridViewTemplate();
        private GridViewSummaryRowItem sumatorioFilaItem = new GridViewSummaryRowItem();

        //Stopwatch watch = new Stopwatch();
        private DataTable dtOrdenes = new DataTable();

        private DataTable dtArticulos = new DataTable();

        // YA USAMOS LOS DE ARRIBADataTable tabla = new DataTable();
        private DataTable dtTemplateArticulos = new DataTable();

        private RadGridView rgvGridArticulos = new RadGridView();
        public string name = "Produccion";
        private Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        private GridViewCheckBoxColumn chkSeleccionOrden = new GridViewCheckBoxColumn();

        //GridViewDecimalColumn udsNecesariasColumna = new GridViewDecimalColumn();
        //GridViewDecimalColumn cantidadFabricadoColumna = new GridViewDecimalColumn();

        //GridViewDecimalColumn pctMermaColumna = new GridViewDecimalColumn();
        private ProgressBarColumn pctPteAcopioColumna;

        private ProgressBarColumn pctFabricadoColumna;

        protected List<TableScheme> _lstEsquemaTablaOrdenes = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaArticulos = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaJerarquicoArticulos = new List<TableScheme>();
        private RadDataFilterDialog dataFilter = new RadDataFilterDialog();
        private GridViewSummaryItem sumSolicitarItem;

        private int _cantidadRegistros = 0;

        public RadGridView gridViewControl
        {
            get { return this.rgvOrdenes; }
        }

        public int CantidadRegistros
        {
            get { return this._cantidadRegistros; }
            set { this._cantidadRegistros = value; }
        }

        public int indexFabricado;
        protected dynamic _selectedRow;

        protected GridScheme _esquemGrid;
        private string ordenes;

        #endregion Variables

        #region Constructor

        public AcopiosProduccion()
        {
            try
            {
                InitializeComponent();
                this.Text = Lenguaje.traduce("Producción");
                this.Load += FrmAcopiosProduccion_Load;
                this.Shown += FrmAcopiosProduccion_Shown;
                this.Load += Form1_Load2;//NO NECESITAMOS DOS LOADS
                CargarEsquemasIniciales();
                LlenarComboMaquinas();
                //CantidadRegistros = Business.GetAcopiosProduccionCantidad(ref _lstEsquemaTablaProveedores);
                accionesArtículosBarGgroup.Visibility = ElementVisibility.Collapsed;
                ControlesLenguaje();
                this.Show();
                this.rgvOrdenes.EnableCustomSorting = true;
                IniciarDatos();
                InitializeThemesDropDown();
                InicializarBGWorkers();
                Eventos();
                //InicializarColumnas();
                rWaitOrdenes.StartWaiting();
                this.SuspendLayout();
                bgWorkerGridViewOrdenes.RunWorkerAsync();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                AñadirLabelCantidad();
                MascaraNumerosArticulos();
                this.Icon = Resources.bruju;
                this.Load += new System.EventHandler(this.FrmSeleccionarExistencia_Load);

                FuncionesGenerales.RumDropDownAddManual(ref rddbtnConfigurar, 50020);
                FuncionesGenerales.AddEliminarLayoutButton(ref rddbtnConfigurar);
                if (this.rddbtnConfigurar.Items["RadMenuItemEliminarLayout"] != null)
                {
                    this.rddbtnConfigurar.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        FuncionesGenerales.EliminarLayout(name + "GridView.xml", null);
                        if (name == "Produccion")
                        {
                            rgvOrdenes.Refresh();
                        }
                        else
                        {
                            rgvGridArticulos.Refresh();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void FrmSeleccionarExistencia_Load(object sender, EventArgs e)
        {
            foreach (GridViewDataColumn dCol in rgvGridArticulos.Columns)

            {
                if (dCol.DataType == typeof(Decimal))
                {
                    /*if(dCol.FormatString.ToLower() =="{0}")
                    {*/
                    dCol.FormatString = "{0:N2}";  //                        "{0:N2}"
                    /*}*/
                }
                if (dCol.DataType == typeof(Single))
                {
                    /*if (dCol.FormatString.ToLower() == "{0}")
                    {*/
                    dCol.FormatString = "{0:N2}";  //                        "{0:N2}"
                    /*}*/
                }

                if (dCol.FieldName.Equals(Lenguaje.traduce("UDSNecesarias")))
                {
                    /*if (dCol.FormatString.ToLower() == "{0}")
                    {*/
                    dCol.FormatString = "{0:N2}";  //                        "{0:N2}"
                    /*}*/
                }
            }
        }

        private void previstasFabricarEditable(bool editable)
        {
            if (gridViewControl.Columns[Lenguaje.traduce("Previstas Fabricacion")] != null)
            {
                gridViewControl.Columns[Lenguaje.traduce("Previstas Fabricacion")].ReadOnly = false;
            }
        }

        private void InicializarColumnas()
        {
            try
            {
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void InicializarBGWorkers()
        {
            bgWorkerGridViewOrdenes = new BackgroundWorker();
            bgWorkerGridViewOrdenes.WorkerReportsProgress = true;
            bgWorkerGridViewOrdenes.WorkerSupportsCancellation = true;
            bgWorkerGridViewOrdenes.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewOrdenes_RunWorkerCompleted);
            bgWorkerGridViewOrdenes.DoWork += new DoWorkEventHandler(llenarGridOrdenes);

            bgWorkerGridViewArticulos = new BackgroundWorker();
            bgWorkerGridViewArticulos.WorkerReportsProgress = true;
            bgWorkerGridViewArticulos.WorkerSupportsCancellation = true;
            bgWorkerGridViewArticulos.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerGridViewArticulos_RunWorkerCompleted);
            bgWorkerGridViewArticulos.DoWork += new DoWorkEventHandler(LlenarGridArticulos);
        }

        private void CargarEsquemasIniciales()
        {
            Business.GetAcopiosProduccionCantidad(ref _lstEsquemaTablaArticulos);
            Business.GetArticulosAcopiosProduccionCantidad(ref _lstEsquemaTablaArticulos);
        }

        private void AñadirLabelCantidad()
        {
            lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            grupoLabel.Items.Add(lblCantidad);
            this.ribbonTabAcciones.Items.AddRange(grupoLabel);
        }

        private void LlenarComboMaquinas()
        {
            try
            {
                string query = "SELECT -1 -1 AS \"IDMAQUINA\",'" + Lenguaje.traduce(strings.Todas) + "' as \"DESCRIPCION\" UNION Select IDMAQUINA,DESCRIPCION FROM TBLMAQUINAS";
                rddlFiltrarMaquina.DataSource = ConexionSQL.getDataTable(query).DefaultView;
                rddlFiltrarMaquina.ValueMember = "IDMAQUINA";
                rddlFiltrarMaquina.DisplayMember = "DESCRIPCION";

                //RadListDataItem item = new RadListDataItem(Lenguaje.traduce(strings.Todas),-1);
                //DescriptionTextListDataItem item = new DescriptionTextListDataItem();
                //item.Text = Lenguaje.traduce(strings.Todas);

                //item.DescriptionText = Lenguaje.traduce(strings.Todas);
                //item.DisplayValue = Lenguaje.traduce(strings.Todas);
                //rddlFiltrarMaquina.Items.Add(/*Lenguaje.traduce(strings.Todas)*/ item);
                //rddlFiltrarMaquina.Text =Lenguaje.traduce(strings.Todas);
                rddlFiltrarMaquina.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void FrmAcopiosProduccion_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        #endregion Constructor

        #region ControlesIngles

        private void ControlesLenguaje()
        {
            try
            {
                lblCantidad.Text = Lenguaje.traduce(strings.Registros + 0);
                rbtnRefrescar.Text = Lenguaje.traduce(strings.Refrescar);
                editColumnas.Text = Lenguaje.traduce(strings.Columnas);
                temasMenu.Text = Lenguaje.traduce(strings.Temas);
                filtrosMenu.Text = Lenguaje.traduce(strings.Filtro);
                cargarMenu.Text = Lenguaje.traduce(strings.CargarEstilo);
                guardarMenu.Text = Lenguaje.traduce(strings.GuardarEstilo);
                btnAsignarMaquina.Text = Lenguaje.traduce(strings.LineaProduccion);
                rbtnCerrarOrden.Text = Lenguaje.traduce(strings.CerrarOrden);
                rbtnConsumir.Text = Lenguaje.traduce(strings.Consumir);
                rbtnSolicitar.Text = Lenguaje.traduce(strings.Solicitar);
                ribbonTabAcciones.Text = Lenguaje.traduce(strings.Acciones);
                rbtnVistaArticulos.Text = Lenguaje.traduce(strings.Articulos);
                maqBarGroup.Text = Lenguaje.traduce(strings.LineasProduccion);
                pedidosBarGroup.Text = Lenguaje.traduce(strings.Pedidos);
                configBarGroup.Text = Lenguaje.traduce(strings.Configuracion);
                vistaGroup.Text = Lenguaje.traduce(strings.Ver);
                rbtnBorrarFiltro.Text = Lenguaje.traduce(strings.LimpiarFiltros);
                /* if (CultureInfo.CurrentUICulture.Name == "es-ES")
                 {}else
                 {
                     lblCantidad.Text = "Records:" +0;
                     name = "Production";
                     this.Text = "Production";
                 }*/
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion ControlesIngles

        #region RelacionArticulosGrid

        private void RelacionGridArticulos()
        {
            try
            {
                rbtnVistaArticulos.Text = Lenguaje.traduce("Articulos");
                GridViewRelation relacion = new GridViewRelation(rgvGridArticulos.MasterTemplate);
                relacion.ChildTemplate = templateArticulos;
                relacion.RelationName = "ArticuloLineas";
                relacion.ParentColumnNames.Add(Lenguaje.traduce("ID Articulo"));
                relacion.ChildColumnNames.Add(Lenguaje.traduce("ID Articulo"));
                //Solucion provisional para CCT para sacar por lote la relacion
                try
                {
                    if (rgvGridArticulos.Columns.Contains(Lenguaje.traduce("Lote")) && templateArticulos.Columns.Contains(Lenguaje.traduce("Lote")))
                    {
                        relacion.ParentColumnNames.Add(Lenguaje.traduce("Lote"));
                        relacion.ChildColumnNames.Add(Lenguaje.traduce("Lote"));
                    }
                }
                catch (Exception ex)
                {
                }

                rgvGridArticulos.Relations.Add(relacion);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion RelacionArticulosGrid

        #region CheckColumn y BarraProgreso

        private void IniciarDatos()
        {
            try
            {
                tableLayoutPanel1.Dock = DockStyle.Fill;
                rgvOrdenes.Dock = DockStyle.Fill;
                chkSeleccionOrden.Name = "checkColumn";
                chkSeleccionOrden.EnableHeaderCheckBox = true;
                chkSeleccionOrden.HeaderText = "";
                chkSeleccionOrden.EditMode = EditMode.OnValueChange;

                this.rWaitOrdenes.Name = "radWaitingBar1";
                this.rWaitOrdenes.Size = new System.Drawing.Size(200, 20);
                this.rWaitOrdenes.TabIndex = 2;
                this.rWaitOrdenes.Text = "radWaitingBar1";
                this.rWaitOrdenes.AssociatedControl = this.rgvOrdenes;
                this.rWaitArticulos.Name = "radWaitingBar2";
                this.rWaitArticulos.Size = new System.Drawing.Size(200, 20);
                this.rWaitArticulos.TabIndex = 2;
                this.rWaitArticulos.Text = "radWaitingBar2";
                this.rWaitArticulos.AssociatedControl = this.rgvGridArticulos;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion CheckColumn y BarraProgreso

        #region Llamada Eventos Propios

        private void Eventos()
        {
            try
            {
                rgvOrdenes.ViewRowFormatting += rgvGeneral_ViewRowFormatting;
                rgvOrdenes.CellFormatting += rgvGridOrdenes_CellFormatting;
                rgvOrdenes.ViewCellFormatting += rgvGridOrdenes_CellFormattingWrapText;
                rgvOrdenes.FilterChanged += rgvGeneral_FilterChanged;
                rgvOrdenes.CustomSorting += rgvGridOrdenes_CustomSorting;
                rgvOrdenes.RowSourceNeeded += rgvGridOrdenes_RowSourceNeeded;
                rgvOrdenes.ViewCellFormatting += rgvPrincipal_ViewCellFormattingHierarchicalGrid;
                rgvOrdenes.ContextMenuOpening += rgvPrincipal_ContextMenuOpening;
                rgvOrdenes.DataBindingComplete += radGridView1_DataBindingComplete;

                rgvOrdenes.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(rgvPrincipal_ViewCellFormatting);

                rgvGridArticulos.ViewRowFormatting += rgvGeneral_ViewRowFormatting;
                rgvGridArticulos.CellFormatting += rgvGridArticulos_CellFormatting;
                rgvGridArticulos.ViewCellFormatting += rgvGridArticulos_ViewCellFormatting;
                rgvGridArticulos.FilterChanged += rgvGeneral_FilterChanged;
                rgvGridArticulos.CustomSorting += rgvGridArticulos_CustomSorting;
                rgvGridArticulos.ViewCellFormatting += rgvPrincipal_ViewCellFormattingHierarchicalGrid;
                rgvGridArticulos.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(rgvPrincipal_ViewCellFormatting);

                rgvGridArticulos.ContextMenuOpening += rgvPrincipal_ContextMenuOpening;
                /*gridViewArticulos.GroupSummaryEvaluate += new GroupSummaryEvaluateEventHandler(gridViewArticulos_GroupSummaryEvaluate);*/
                rddlFiltrarMaquina.SelectedIndexChanged += rddlFiltrarMaquina_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                //ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void radGridView1_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            for (int j = 0; j < this.rgvOrdenes.Columns.Count; j++)
            {
                if (this.rgvOrdenes.Columns[j].FieldName == "Fecha Caducidad" || this.rgvOrdenes.Columns[j].FieldName == "FechaPrevFab")
                {
                    if (this.rgvOrdenes.Columns[j].GetType() == typeof(Telerik.WinControls.UI.GridViewDateTimeColumn))
                    {
                        ((GridViewDateTimeColumn)this.rgvOrdenes.Columns[j]).FormatString = "{0:MM/dd/yyyy}";
                        ((GridViewDateTimeColumn)this.rgvOrdenes.Columns[j]).Format = DateTimePickerFormat.Custom;
                        ((GridViewDateTimeColumn)this.rgvOrdenes.Columns[j]).CustomFormat = "MM/dd/yyyy";
                    }
                }
            }

            /* if (!string.IsNullOrWhiteSpace(e.CellElement.Value.ToString()))
             {
                 if (e.CellElement.Value.GetType().Name == "DateTime")
                 {
                     CultureInfo culture = new CultureInfo("es-ES");
                     string fechaCreacion = e.CellElement.Value.ToString();
                     DateTime date = DateTime.Parse(fechaCreacion, culture);
                     string fecha = date.ToString("dd/MM/yyyy");
                     e.CellElement.Value = fecha;
                 }
             }*/
            ///      e.CellElement.Value = fechaCreacion.ToString("dd/MM/yyyy");
        }

        private void rgvGridArticulos_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement.ColumnInfo.Name.Equals("SolicitarArticulos"))
                {
                    GridViewRowInfo row = e.CellElement.RowInfo;
                    if (row.Index >= 0)
                    {
                        GridViewSummaryRowItem rowSum = rgvGridArticulos.Templates[0].SummaryRowsBottom[0];
                        string expresion = rowSum[3].GetSummaryExpression();
                        Debug.WriteLine("");
                        decimal valor = Decimal.Parse(e.CellElement.Value.ToString());
                        if (rgvGridArticulos.Templates[0].DataView.Count == 0)
                        {
                            e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                            return;
                        }
                        decimal valorHijo = Decimal.Parse(rgvGridArticulos.Templates[0].DataView.Evaluate(expresion, row.ChildRows).ToString());
                        if (valorHijo > valor)
                        {
                            e.CellElement.ForeColor = Color.Red;
                        }
                        else
                        {
                            e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        }
                    }
                }
                if (e.CellElement.ColumnInfo.Name.Equals("UDSNecesarias"))
                {
                    e.CellElement.FormatString = "{0:N0}";
                    //((GridViewDateTimeColumn)this.rgvOrdenes.Columns[j]).Format = Format;
                    //((GridViewDateTimeColumn)this.rgvOrdenes.Columns[j]).CustomFormat = "MM/dd/yyyy";
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Llamada Eventos Propios

        #region CreateChildTemplate

        private GridViewTemplate CreateChildTemplateSalidas()
        {
            GridViewTemplate template = new GridViewTemplate();
            string prueba = "999999";
            try
            {
                DataTable a = Business.GetAcopiosSalidasDatosGridView(prueba);
                foreach (DataColumn item in a.Columns)
                {
                    template.Columns.Add(item.Caption);
                }
                template.HierarchyDataProvider = new GridViewEventDataProvider(template);
                //template.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                template.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            return template;
        }

        private void FormatearChildTemplateArticulos()
        {
            try
            {
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Orden")].IsVisible = false;

                //int indColUDSNecesarias = rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("UDSNecesarias")].Index;
                //rgvOrdenes.Templates[0].Columns.Remove(Lenguaje.traduce("UDSNecesarias"));
                //rgvOrdenes.Templates[0].Columns.Add(udsNecesariasColumna);
                //rgvOrdenes.Templates[0].Columns.Move(udsNecesariasColumna.Index, indColUDSNecesarias);
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("UDSNecesarias")].TextAlignment = ContentAlignment.BottomRight;
                // rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("% Merma")].FormatString = "{0:N0}";

                //int indColMerma = rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("%Merma")].Index;
                //rgvOrdenes.Templates[0].Columns.Remove(Lenguaje.traduce("%Merma"));
                //rgvOrdenes.Templates[0].Columns.Add(pctMermaColumna);
                //rgvOrdenes.Templates[0].Columns.Move(pctMermaColumna.Index, indColMerma);
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("%Merma")].TextAlignment = ContentAlignment.BottomRight;
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("%Merma")].FormatString = "{0:N2}";

                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num PedidoFab")].IsVisible = false;
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num PedidoFab")].VisibleInColumnChooser = true;
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num PedidoFab")].AllowHide = true;
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num Articulo")].IsVisible = false;
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num Articulo")].VisibleInColumnChooser = true;
                //rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num Articulo")].AllowHide = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void FormatearChildTemplateSalidas()
        {
            rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad")].FormatString = "{0:N0}";
            rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad")].TextAlignment = ContentAlignment.BottomRight;
            rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad Unidad")].FormatString = "{0:N0}";
            rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad Unidad")].TextAlignment = ContentAlignment.BottomRight;
        }

        private void FormatearChildTemplateEntradas()
        {
            rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad")].FormatString = "{0:N0}";
            rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad")].TextAlignment = ContentAlignment.BottomRight;

            rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad Unidad")].FormatString = "{0:N0}";
            rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad Unidad")].TextAlignment = ContentAlignment.BottomRight;
        }

        private GridViewTemplate CreateChildTemplateArticulos()
        {
            GridViewTemplate template = new GridViewTemplate();
            string prueba = "-1";
            try
            {
                DataTable a = Business.GetAcopiosArticulosDatosGridView(prueba);
                foreach (DataColumn item in a.Columns)
                {
                    template.Columns.Add(item.Caption);
                }
                // template.Columns.Add(Lenguaje.traduce("Tipo Unidad"));
                template.HierarchyDataProvider = new GridViewEventDataProvider(template);
                //template.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                template.BestFitColumns(BestFitColumnMode.DisplayedCells);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            return template;
        }

        private GridViewTemplate CreateChildTemplateEntradas()
        {
            GridViewTemplate template = new GridViewTemplate();
            string prueba = "999999";
            try
            {
                DataTable a = Business.GetAcopiosEntradasDatosGridView(prueba);
                foreach (DataColumn item in a.Columns)
                {
                    template.Columns.Add(item.Caption);
                }
                template.HierarchyDataProvider = new GridViewEventDataProvider(template);
                //template.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                template.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            return template;
        }

        #endregion CreateChildTemplate

        #region LLenarGridOrdenes

        private void llenarGridOrdenes(object sender, DoWorkEventArgs e)
        {
            try
            {
                //PRUEBA CARGA Business.GetAcopiosProduccionCantidad(ref _lstEsquemaTablaPrincipal);
                //tabla = Business.GetAcopiosProduccionDatosGridView(_lstEsquemaTablaPrincipal);
                CantidadRegistros = Business.GetAcopiosProduccionCantidad(ref _lstEsquemaTablaOrdenes);
                dtOrdenes = Business.GetAcopiosProduccionDatosGridView(_lstEsquemaTablaOrdenes);
                //dtOrdenes.Columns.Add(Lenguaje.traduce("Fabricado"));
                //dtOrdenes.Columns.Add(Lenguaje.traduce("Tipo Unidad"));
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void bgWorkerGridViewOrdenes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (dtOrdenes.Rows.Count <= 0)
                {
                    rWaitOrdenes.StopWaiting();
                }
                else
                {
                    rgvOrdenes.DataSource = null;
                    rgvOrdenes.Columns.Clear();
                    rgvOrdenes.DataSource = dtOrdenes;
                    rgvOrdenes.Columns.Add(chkSeleccionOrden);
                    rgvOrdenes.Columns.Move(rgvOrdenes.Columns.Count - 1, 0);
                    rgvOrdenes.Refresh();
                    AñadirDatosGridOrdenes();
                    SetPreferences();
                    //setFiltros1();
                    OcultarDatosGrid1();
                    ColorEstado();
                    ColorEstadoMaquina();
                    OcultarCamposOrdenes();
                    // MascaraNumerosOrdenes();
                    rgvOrdenes.BestFitColumns(BestFitColumnMode.DisplayedCells);
                    ComprobacionAcceso();
                    //ComprobacionCerrarOrden();
                    ElegirEstilo();
                    rgvOrdenes.Refresh();
                    rWaitOrdenes.StopWaiting();
                    previstasFabricarEditable(true);

                    GridViewMaskBoxColumn maskBoxColumn = new GridViewMaskBoxColumn("UDSNecesarias");
                    maskBoxColumn.FieldName = "UDSNecesarias";
                    maskBoxColumn.MaskType = MaskType.Numeric;
                    maskBoxColumn.Mask = "N";
                    maskBoxColumn.FormatString = "{0:N0}";
                    rgvGridArticulos.MasterTemplate.Columns.Add(maskBoxColumn);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                rWaitOrdenes.StopWaiting();
            }
            this.ResumeLayout();
        }

        private void OcultarCamposOrdenes()
        {
            try
            {
                for (int i = 0; i < _lstEsquemaTablaOrdenes.Count; i++)
                {
                    if (!_lstEsquemaTablaOrdenes[i].EsVisible)
                    {
                        rgvOrdenes.Columns[_lstEsquemaTablaOrdenes[i].Etiqueta].IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void AddColumnaPctFabricadoOrdenes()
        {
            try
            {
                pctFabricadoColumna = new ProgressBarColumn(Lenguaje.traduce(strings.PorcentajeFabricado));
                pctFabricadoColumna.HeaderText = Lenguaje.traduce(strings.PorcentajeFabricado);
                pctFabricadoColumna.Width = 100;
                rgvOrdenes.Columns.Remove(rgvOrdenes.Columns[Lenguaje.traduce(strings.PorcentajeFabricado)]);
                this.rgvOrdenes.Columns.Add(pctFabricadoColumna);
            }
            catch (Exception ex)
            {
            }
        }

        private void AñadirDatosGridOrdenes()
        {
            try
            {
                AddColumnaPctFabricadoOrdenes();
                foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                {
                    if (row.Cells[Lenguaje.traduce("Cantidad Fabricado")].Value == null || row.Cells[Lenguaje.traduce("Cantidad Fabricado")].Value.ToString() == "")
                    {
                        row.Cells[Lenguaje.traduce("Cantidad Fabricado")].Value = 0;
                    }
                    if (row.Cells[Lenguaje.traduce("Cantidad Fabricado")].Value != null)
                    {
                        decimal ctdadFabricado = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Cantidad Fabricado")].Value.ToString());
                        decimal previstas = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Previstas Fabricacion")].Value.ToString());
                        decimal pct = ctdadFabricado * 100 / previstas;

                        row.Cells[Lenguaje.traduce(strings.PorcentajeFabricado)].Value = pct;
                    }

                    //AplicarPresentaciones
                    //Debug.Print(row.Cells[Lenguaje.traduce("Num PedidoFab")].Value.ToString() +"-" + row.Cells[Lenguaje.traduce(strings.PorcentajeFabricado)].Value.ToString());
                    ////int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                    //int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);

                    //object[] presPrev = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, previstas);

                    //row.Cells[Lenguaje.traduce("Previstas Fabricacion")].Value = Convert.ToDecimal(presPrev[0]); ;
                    //row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presPrev[1].ToString();
                    //object[] presFab =Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, ctdadFabricado);
                    //row.Cells[Lenguaje.traduce("Cantidad Fabricado")].Value = Convert.ToDecimal(presFab[0]);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void OcultarDatosGrid1()
        {
            try
            {
                rgvOrdenes.Columns.Remove(rgvOrdenes.Columns["RowNum"]);
                // rgvOrdenes.Columns["checkColumn"].IsVisible = true;
                //rgvOrdenes.Columns[Lenguaje.traduce("Num PedidoFab")].IsVisible = false;
                //rgvOrdenes.Columns[Lenguaje.traduce("Num PedidoFab")].VisibleInColumnChooser = true;
                //rgvOrdenes.Columns[Lenguaje.traduce("Num PedidoFab")].AllowHide = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        //private void MascaraNumerosOrdenes()
        //{
        //    try
        //    {
        //            rgvOrdenes.Columns[Lenguaje.traduce("Previstas Fabricacion")].FormatString = "{0:N2}";
        //            rgvOrdenes.Columns[Lenguaje.traduce("Cantidad Fabricado")].FormatString = "{0:N2}";
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.GestionarError(ex);
        //        log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
        //    }
        //}

        #endregion LLenarGridOrdenes

        #region LlenarGridArticulos

        public string extraerIdOrdenesFab()
        {
            string ordenesFab = string.Empty;
            try
            {
                foreach (GridViewRowInfo rowInfo in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(rowInfo.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value) == true)
                    {
                        ordenesFab = ordenesFab + rowInfo.Cells[Lenguaje.traduce("Num PedidoFab")].Value.ToString() + " ,";
                    }
                }
                if (String.IsNullOrEmpty(ordenesFab))
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorFilasMarcadas));
                    return ordenesFab;
                }
                ordenesFab = ordenesFab.Substring(0, ordenesFab.Length - 2);
            }
            catch (Exception)
            {
                MessageBox.Show(Lenguaje.traduce(strings.ErrorFilasMarcadas));
            }
            return ordenesFab;
        }

        private void LlenarGridArticulos(object sender, DoWorkEventArgs e)
        {
            try
            {
                gridViewControl.ClearSelection();

                string filtroArticulos = extraerIdOrdenesFab();

                if (filtroArticulos == String.Empty)
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorFilasMarcadas));
                }
                else
                {
                    //Lo cargo en el load solo una vezBusiness.GetArticulosAcopiosProduccionCantidad(ref _lstEsquemaTablaPrincipal);
                    //NO CARGAR DOS VECEStabla = Business.GetArticulosAcopiosProduccionDatosGridView(_lstEsquemaTablaPrincipal, filtroArticulos);
                    dtArticulos = Business.GetArticulosAcopiosProduccionDatosGridView(_lstEsquemaTablaArticulos, filtroArticulos);
                    //2020 No lo añadimos aqui dtArticulos.Columns.Add("NoFabricado");
                    //Business.GetArticulosAcopiosProduccionJerarquicoCantidad(ref _lstEsquemaTablaJerarquicoArticulos);
                    //dtTemplateArticulos = Business.GetArticulosAcopiosProduccionJerarquicoDatosGridView(_lstEsquemaTablaJerarquicoArticulos, filtroArticulos);

                    List<int> listaOrdenesFab = new List<int>();
                    List<int> listaPrevistasFabricar = new List<int>();
                    //dtTemplateArticulos.Rows.Clear();
                    try
                    {
                        foreach (GridViewRowInfo rowInfo in rgvOrdenes.Rows)
                        {
                            if (Convert.ToBoolean(rowInfo.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value) == true)
                            {
                                listaOrdenesFab.Add(int.Parse(rowInfo.Cells[Lenguaje.traduce("Num PedidoFab")].Value.ToString()));
                                decimal cantidad = Convert.ToDecimal(rowInfo.Cells[Lenguaje.traduce("Previstas Fabricacion")].Value);
                                int idPresentacion = Convert.ToInt32(rowInfo.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                                int idArticulo = Convert.ToInt32(rowInfo.Cells[Lenguaje.traduce("ID Articulo")].Value);
                                object[] presAlm = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidad);
                                listaPrevistasFabricar.Add(Convert.ToInt32(presAlm[0].ToString()));
                            }
                        }
                        if (listaOrdenesFab.Count < 1)
                        {
                            MessageBox.Show(Lenguaje.traduce(strings.ErrorFilasMarcadas));
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        log.Debug(strings.ErrorFilasMarcadas + " o hay un string en la columna Previstas Fabricacion");
                        MessageBox.Show(Lenguaje.traduce(strings.ErrorFilasMarcadas));
                    }

                    dtTemplateArticulos = Business.GetArticulosAcopiosProduccionJerarquicoDatosGridView(_lstEsquemaTablaJerarquicoArticulos, listaOrdenesFab[0], listaPrevistasFabricar[0]);

                    for (int i = 1; i < listaOrdenesFab.Count; i++)
                    {
                        DataTable siguienteOrden = Business.GetArticulosAcopiosProduccionJerarquicoDatosGridView(_lstEsquemaTablaJerarquicoArticulos, listaOrdenesFab[i], listaPrevistasFabricar[i]);
                        for (int y = 0; y < siguienteOrden.Rows.Count; y++)
                        {
                            dtTemplateArticulos.ImportRow(siguienteOrden.Rows[y]);
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

        private void bgWorkerGridViewArticulos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (dtArticulos.Rows.Count <= 0)
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorSinJerarquia));
                    ClickProduccionDatos();
                    rWaitArticulos.StopWaiting();
                }
                else
                {
                    /*DataTable tablaVacia = new DataTable();
                    tablaVacia = tabla;
                    DataRow row = tablaVacia.Rows[0];
                    tablaVacia.Rows.Clear();
                    tablaVacia.Rows.Add(row);
                    gridViewArticulos.DataSource = tablaVacia;*/
                    AñadirDatosGridArticulos();
                    RelacionGridArticulos();
                    rgvGridArticulos.Refresh();
                    AñadirColumnasGridArticulos();
                    OcultarDatosGridArticulos();
                    SetPreferences2();
                    rgvGridArticulos.Refresh();
                    rgvGridArticulos.EnableCustomSorting = true;
                    rgvGridArticulos.Templates[0].AllowAddNewRow = false;
                    rgvGridArticulos.Templates[0].ReadOnly = false;
                    name = "ArticulosAcopiosProduccion";
                    foreach (var grid in rgvGridArticulos.Columns)
                    {
                        if (!(grid is GridViewCheckBoxColumn))
                            grid.ReadOnly = true;
                    }
                    foreach (var item in rgvGridArticulos.Templates[0].Columns)
                    {
                        if (item.FieldName != Lenguaje.traduce("Solicitar"))
                        {
                            item.ReadOnly = true;
                        }
                    }

                    rgvGridArticulos.BestFitColumns();
                    rWaitArticulos.StopWaiting();
                    ElegirEstilo();

                    if (rCheckSoloPendientes.IsChecked)
                    {
                        FilterDescriptor filter = new FilterDescriptor();
                        filter.PropertyName = "SolicitarArticulos";
                        filter.Operator = FilterOperator.IsNotEqualTo;
                        filter.Value = "0";
                        filter.IsFilterEditor = true;
                        rgvGridArticulos.FilterDescriptors.Add(filter);
                    }
                }
            }
            catch (Exception ex)
            {
                rWaitArticulos.StopWaiting();
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AñadirDatosGridArticulos()
        {
            try
            {
                rgvGridArticulos.DataSource = null;
                rgvGridArticulos.DataSource = dtArticulos;
                rgvGridArticulos.Templates.Clear();
                templateArticulos.DataSource = null;
                templateArticulos.DataSource = dtTemplateArticulos;
                /*if(templateArticulos.Columns[Lenguaje.traduce("Tipo Unidad")]==null)
                    templateArticulos.Columns.Add(Lenguaje.traduce("Tipo Unidad"));*/
                rgvGridArticulos.Templates.Add(templateArticulos);

                //César: No puedes modificar los datos de un template, solo son de lectura,
                //se tiene que modificar antes de añadirlos al
                foreach (GridViewRowInfo row in templateArticulos.Rows)
                {
                    //AplicarPresentaciones
                    try
                    {
                        int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                        decimal necesario = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Necesario")].Value);
                        decimal solicitado = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Solicitado")].Value);
                        decimal consumido = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Consumido")].Value);

                        object[] presNecesario = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, necesario);
                        if (presNecesario[0].ToString().Contains(","))
                        {
                            row.Cells[Lenguaje.traduce("Necesario")].Value = presNecesario[0].ToString();
                        }
                        else
                        {
                            row.Cells[Lenguaje.traduce("Necesario")].Value = Decimal.Parse(presNecesario[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                        }
                        //row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presNecesario[1].ToString();
                        object[] presSolicitado = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, solicitado);
                        if (presSolicitado[0].ToString().Contains(","))
                        {
                            row.Cells[Lenguaje.traduce("Solicitado")].Value = presSolicitado[0].ToString();
                        }
                        else
                        {
                            row.Cells[Lenguaje.traduce("Solicitado")].Value = Decimal.Parse(presSolicitado[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                        }

                        object[] presConsumido = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, consumido);
                        if (presConsumido[0].ToString().Contains(","))
                        {
                            row.Cells[Lenguaje.traduce("Consumido")].Value = Decimal.Parse(presConsumido[0].ToString());
                        }
                        else
                        {
                            row.Cells[Lenguaje.traduce("Consumido")].Value = Decimal.Parse(presConsumido[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                        }

                        Debug.Print("Necesario " + row.Cells[Lenguaje.traduce("Necesario")].Value + "Solicitado " + row.Cells[Lenguaje.traduce("Solicitado")].Value + "Consumido " + row.Cells[Lenguaje.traduce("Consumido")].Value);
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.GestionarError(ex);
                    }
                }
                //foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                //{
                //    int acopiado = Convert.ToInt32(row.Cells[Lenguaje.traduce("Acopiado")].Value);
                //    int stock = Convert.ToInt32(row.Cells[Lenguaje.traduce("Stock")].Value);
                //    int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                //    object[] presAcopiado = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, acopiado);
                //    row.Cells[Lenguaje.traduce("Acopiado")].Value = presAcopiado[0];
                //    object[] presStock = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, stock);
                //    row.Cells[Lenguaje.traduce("Stock")].Value = presStock[0];
                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AñadirColumnasGridArticulos()
        {
            try
            {
                rgvGridArticulos.Columns.Remove(rgvGridArticulos.Columns["RowNum"]);
                AddColumnaNecesario();
                AddColumnaSolicitado();
                AddColumnaConsumido();
                AddColumnaUdsPendientes();
                AddColumnaUdsSolicitar();
                AddColumnaPorcentajePteAcopio();
                AddColumnaJerarquicoSolicitar();
                AddValorColumnaJerarquicoSolicitar();
                AnyadirFilasResumenSumatorio();
                ColorAlertaExcesoSolicitar();
                //MascaraNumerosArticulos();
                AddColumnaCheckPaletCompleto();
                AddColumnaCheckArticulo();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AddColumnaCheckArticulo()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("chkSeleccionArticulo");
                GridViewCheckBoxColumn chkSeleccionArticulo = new GridViewCheckBoxColumn();
                chkSeleccionArticulo.Name = "chkSeleccionArticulo";
                chkSeleccionArticulo.EnableHeaderCheckBox = true;
                chkSeleccionArticulo.HeaderText = "";
                chkSeleccionArticulo.EditMode = EditMode.OnValueChange;
                rgvGridArticulos.Columns.Add(chkSeleccionArticulo);
                rgvGridArticulos.Columns.Move(rgvGridArticulos.Columns.Count - 1, 0);
                FuncionesGenerales.setCheckBoxFalse(ref rgvGridArticulos, chkSeleccionArticulo.Name);
                rgvGridArticulos.Columns[0].ReadOnly = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AddColumnaCheckPaletCompleto()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("PaletCompleto");
                GridViewCheckBoxColumn chkPaletCompletoColumn = new GridViewCheckBoxColumn();
                chkPaletCompletoColumn.Name = "PaletCompleto";
                chkPaletCompletoColumn.HeaderText = Lenguaje.traduce("Palet Completo");
                chkPaletCompletoColumn.EnableHeaderCheckBox = true;
                chkPaletCompletoColumn.EditMode = EditMode.OnValueChange;

                rgvGridArticulos.Columns.Add(chkPaletCompletoColumn);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ColorAlertaExcesoSolicitar()
        {
            try
            {
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        /*private void DatosColumnas()
        {
            try
            {
                cantidadFabricadoColumna.HeaderText =Lenguaje.traduce("CantidadConsumida2");
                cantidadFabricadoColumna.Name = Lenguaje.traduce("CantidadConsumida2");
                cantidadFabricadoColumna.FieldName = Lenguaje.traduce("CantidadConsumida2");
                gridViewArticulos.Columns.Add(cantidadFabricadoColumna);
                udsNecesariasColumna.HeaderText =Lenguaje.traduce("UDSNecesarias");
                udsNecesariasColumna.FieldName = Lenguaje.traduce("UDSNecesarias");
                udsNecesariasColumna.Name = Lenguaje.traduce("UDSNecesarias");
                gridViewArticulos.Columns.Add(udsNecesariasColumna);
                foreach (GridViewRowInfo row in gridViewArticulos.Rows)
                {
                    int unidades = 0;
                    foreach (GridViewRowInfo rowinfo in row.ChildRows)
                    {
                            unidades = unidades + Convert.ToInt32(rowinfo.Cells[Lenguaje.traduce("UDSNecesarias")].Value);
                            row.Cells[Lenguaje.traduce("CantidadConsumida2")].Value = rowinfo.Cells[Lenguaje.traduce("Cantidad Consumida")].Value;
                            row.Cells[Lenguaje.traduce("UDSNecesarias")].Value = unidades;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }*/

        private void AddColumnaPorcentajePteAcopio()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("PendienteAcopio");

                pctPteAcopioColumna = new ProgressBarColumn(Lenguaje.traduce("PendienteAcopio"));
                pctPteAcopioColumna.FieldName = "PendienteAcopio";
                pctPteAcopioColumna.Name = "PendienteAcopio";
                pctPteAcopioColumna.HeaderText = Lenguaje.traduce("PendienteAcopio");
                rgvGridArticulos.Columns.Add(pctPteAcopioColumna);

                rgvGridArticulos.Columns.Remove("SolicitadoAcopio");
                GridViewDecimalColumn pctValorAcopiadoColumna = new GridViewDecimalColumn();
                pctValorAcopiadoColumna.FieldName = "SolicitadoAcopio";
                pctValorAcopiadoColumna.Name = "SolicitadoAcopio";
                pctValorAcopiadoColumna.HeaderText = Lenguaje.traduce("SolicitadoAcopio");
                pctValorAcopiadoColumna.Expression = "(" + "SolicitadoArticulos" + " + " + "Acopiado" + ") * 100 /" + "NecesarioArticulos";

                rgvGridArticulos.Columns.Add(pctValorAcopiadoColumna);
                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    int idArticulo = Convert.ToInt32(row.Cells[0].Value);
                    double acopiado = Convert.ToDouble(row.Cells[Lenguaje.traduce("SolicitadoArticulos")].Value) + Convert.ToDouble(row.Cells[Lenguaje.traduce("Acopiado")].Value);
                    double necesario = Convert.ToDouble(row.Cells[Lenguaje.traduce("NecesarioArticulos")].Value);
                    double consumido = Convert.ToDouble(row.Cells[Lenguaje.traduce("ConsumidoArticulos")].Value);
                    double pteAcopio = Convert.ToDouble(row.Cells[Lenguaje.traduce("SolicitarArticulos")].Value);
                    double pctPteAcopio;
                    if (pteAcopio <= 0)
                        pctPteAcopio = 0;
                    else
                        pctPteAcopio = pteAcopio * 100 / (necesario /*- consumido*/);

                    if (pctPteAcopio <= 0)
                        row.Cells[Lenguaje.traduce("PendienteAcopio")].Value = 0;
                    else if (pctPteAcopio >= 100)
                        row.Cells[Lenguaje.traduce("PendienteAcopio")].Value = 100;
                    else
                        row.Cells[Lenguaje.traduce("PendienteAcopio")].Value = Convert.ToInt32(pctPteAcopio);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        //private void AddColumnaPorcentajeFabricado()
        //{
        //    try
        //    {
        //        pctPteAcopioColumna = new ProgressBarColumn(Lenguaje.traduce("PendienteAcopio"));
        //        pctPteAcopioColumna.Width = 100;
        //        pctPteAcopioColumna.FieldName = "PendienteAcopio";
        //        pctPteAcopioColumna.Name = "PendienteAcopio";
        //        pctPteAcopioColumna.HeaderText = Lenguaje.traduce("PendienteAcopio");
        //        rgvGridArticulos.Columns.Add(pctPteAcopioColumna);
        //        pctValorAcopiadoColumna.FieldName = Lenguaje.traduce("SolicitadoAcopio");
        //        pctValorAcopiadoColumna.Name = Lenguaje.traduce("SolicitadoAcopio");
        //        pctValorAcopiadoColumna.HeaderText = Lenguaje.traduce("SolicitadoAcopio");
        //        Debug.WriteLine("(" + Lenguaje.traduce("SolicitadoArticulos") + " + " + Lenguaje.traduce("Acopiado") + ") * 100 /" +/* Lenguaje.traduce("UDSPendientes")*/Lenguaje.traduce("NecesarioArticulos"));
        //        pctValorAcopiadoColumna.Expression = "(" + Lenguaje.traduce("SolicitadoArticulos") + " + " + Lenguaje.traduce("Acopiado") + ") * 100 /" +/*Lenguaje.traduce("UDSPendientes")*/Lenguaje.traduce("NecesarioArticulos");
        //        rgvGridArticulos.Columns.Add(pctValorAcopiadoColumna);
        //        foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
        //        {
        //            if (Convert.ToInt32(row.Cells[Lenguaje.traduce("SolicitadoAcopio")].Value) <= 0)
        //            {
        //                row.Cells[Lenguaje.traduce("PendienteAcopio")].Value = 100;
        //            }
        //            else if (Convert.ToInt32(row.Cells[Lenguaje.traduce("SolicitadoAcopio")].Value) >= 100)
        //            {
        //                row.Cells[Lenguaje.traduce("PendienteAcopio")].Value = 0;
        //            }
        //            else
        //            {
        //                row.Cells[Lenguaje.traduce("PendienteAcopio")].Value = 100 - Convert.ToInt32(row.Cells[Lenguaje.traduce("SolicitadoAcopio")].Value);
        //            }
        //        }

        //        /*gridViewArticulos.Columns.Remove(gridViewArticulos.Columns[Lenguaje.traduce("NoFabricado")]);
        //        this.gridViewArticulos.Columns.Add(noFabricadoColumna);*/
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.GestionarError(ex);
        //        log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
        //    }
        //}
        private void AddColumnaJerarquicoSolicitar()
        {
            try
            {
                rgvGridArticulos.Templates[0].Columns.Remove("Solicitar");

                GridViewDecimalColumn solicitarJerarquicoColumna = new GridViewDecimalColumn();
                solicitarJerarquicoColumna.HeaderText = Lenguaje.traduce("Solicitar");
                solicitarJerarquicoColumna.FieldName = "Solicitar";
                solicitarJerarquicoColumna.Name = "Solicitar";
                solicitarJerarquicoColumna.Minimum = 0;
                // solicitarJerarquicoColumna.FormatString = "{0:N2}";
                rgvGridArticulos.Templates[0].Columns.Add(solicitarJerarquicoColumna);
                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    foreach (GridViewRowInfo rowinfo in row.ChildRows)
                    {
                        rowinfo.Cells["Solicitar"].Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AddValorColumnaJerarquicoSolicitar()
        {
            try
            {
                decimal valor = 0;

                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    foreach (GridViewRowInfo rowinfo in row.ChildRows)
                    {
                        try
                        {
                            decimal udsNecesarias = Decimal.Parse(rowinfo.Cells[Lenguaje.traduce("Necesario")].Value.ToString());
                            decimal solicitado = Decimal.Parse(rowinfo.Cells[Lenguaje.traduce("Solicitado")].Value.ToString());
                            decimal consumido = Decimal.Parse(rowinfo.Cells[Lenguaje.traduce("Consumido")].Value.ToString());
                            decimal merma = Decimal.Parse(rowinfo.Cells[Lenguaje.traduce("%Merma")].Value.ToString());
                            valor = (udsNecesarias + (udsNecesarias * (merma / 100))) - solicitado - consumido;
                            Debug.Print("Valor Jerarquico Solicitar " + valor);

                            if (valor >= 0)
                            {
                                rowinfo.Cells["Solicitar"].Value = valor.ToString("G29");
                            }
                            else
                            {
                                rowinfo.Cells["Solicitar"].Value = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
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

        //private void AddColumnaUdsNecesariasJerarquicoOrdenes()
        //{
        //    udsNecesariasColumna.FieldName = Lenguaje.traduce("UDSNecesarias");
        //    udsNecesariasColumna.Name = Lenguaje.traduce("UDSNecesarias");
        //    udsNecesariasColumna.HeaderText = Lenguaje.traduce("UDSNecesarias");
        //    udsNecesariasColumna.FormatString = "{0:N}";
        //}
        //private void AddColumnaMermaJerarquicoOrdenes()
        //{
        //    pctMermaColumna.FieldName = Lenguaje.traduce("%Merma");
        //    pctMermaColumna.Name = Lenguaje.traduce("%Merma");
        //    pctMermaColumna.HeaderText = Lenguaje.traduce("%Merma");
        //    pctMermaColumna.FormatString = "{0:N}";
        //}

        private void AddColumnaUdsPendientes()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("PendientesArticulos");
                GridViewDecimalColumn unidadesPendientesArticulosColumna = new GridViewDecimalColumn();
                unidadesPendientesArticulosColumna.FieldName = "PendientesArticulos";
                unidadesPendientesArticulosColumna.Name = "PendientesArticulos";
                unidadesPendientesArticulosColumna.HeaderText = Lenguaje.traduce("Pendientes");
                //unidadesPendientesArticulosColumna.FormatString = "{0:N2}";
                rgvGridArticulos.Columns.Add(unidadesPendientesArticulosColumna);
                decimal valor = 0;
                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    Debug.Print("NecesarioArticulos" + Decimal.Parse(row.Cells[Lenguaje.traduce("NecesarioArticulos")].Value.ToString()));
                    Debug.Print("ConsumidoArticulos" + Decimal.Parse(
                           row.Cells[Lenguaje.traduce("ConsumidoArticulos")].Value.ToString()));
                    valor = Decimal.Parse(
                            row.Cells[Lenguaje.traduce("NecesarioArticulos")].Value.ToString()) - Decimal.Parse(
                            row.Cells[Lenguaje.traduce("ConsumidoArticulos")].Value.ToString());
                    Debug.Print("Valor resta " + valor);
                    if (valor > 0)
                    {
                        row.Cells[Lenguaje.traduce("PendientesArticulos")].Value = valor.ToString("G29");
                    }
                    else
                    {
                        row.Cells[Lenguaje.traduce("PendientesArticulos")].Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AddColumnaUdsSolicitar()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("SolicitarArticulos");
                GridViewDecimalColumn unidadesSolicitarArticulosColumna = new GridViewDecimalColumn();
                unidadesSolicitarArticulosColumna.FieldName = "SolicitarArticulos";
                unidadesSolicitarArticulosColumna.Name = "SolicitarArticulos";
                unidadesSolicitarArticulosColumna.HeaderText = Lenguaje.traduce("Solicitar");
                //unidadesSolicitarArticulosColumna.FormatString = "{0:N2}";
                rgvGridArticulos.Columns.Add(unidadesSolicitarArticulosColumna);
                decimal valor = 0;
                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    valor = Decimal.Parse(
                             row.Cells[Lenguaje.traduce("PendientesArticulos")].Value.ToString()) - Decimal.Parse(
                             row.Cells[Lenguaje.traduce("SolicitadoArticulos")].Value.ToString()) - Decimal.Parse(
                             row.Cells[Lenguaje.traduce("Acopiado")].Value.ToString());
                    if (valor >= 0)
                    {
                        row.Cells[Lenguaje.traduce("SolicitarArticulos")].Value = valor.ToString("G29");
                    }
                    else
                    {
                        row.Cells[Lenguaje.traduce("SolicitarArticulos")].Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AddColumnaSolicitado()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("SolicitadoArticulos");
                GridViewDecimalColumn solicitadoArticulosColumna = new GridViewDecimalColumn();
                solicitadoArticulosColumna.FieldName = "SolicitadoArticulos";
                solicitadoArticulosColumna.Name = "SolicitadoArticulos";
                solicitadoArticulosColumna.HeaderText = Lenguaje.traduce("Solicitado");
                // solicitadoArticulosColumna.FormatString = "{0:N2}";
                rgvGridArticulos.Columns.Add(solicitadoArticulosColumna);
                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    decimal solicitadoTotal = 0;
                    foreach (GridViewRowInfo rowinfo in row.ChildRows)
                    {
                        Debug.Print(" AddColumnaSolicitado SolicitadoArticulos " + rowinfo.Cells[Lenguaje.traduce("Solicitado")].Value);
                        solicitadoTotal += Decimal.Parse(rowinfo.Cells[Lenguaje.traduce("Solicitado")].Value.ToString());
                        Debug.Print("" + solicitadoTotal);
                    }
                    row.Cells[Lenguaje.traduce("SolicitadoArticulos")].Value = solicitadoTotal.ToString("G29");
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AddColumnaConsumido()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("ConsumidoArticulos");
                GridViewDecimalColumn consumidoArticulosColumna = new GridViewDecimalColumn();
                consumidoArticulosColumna.FieldName = "ConsumidoArticulos";
                consumidoArticulosColumna.Name = "consumidoArticulos";
                consumidoArticulosColumna.HeaderText = Lenguaje.traduce("Consumido");
                // consumidoArticulosColumna.FormatString = "{0:N2}";
                rgvGridArticulos.Columns.Add(consumidoArticulosColumna);
                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    decimal consumidoTotal = 0;
                    foreach (GridViewRowInfo rowinfo in row.ChildRows)
                    {
                        consumidoTotal += Decimal.Parse(rowinfo.Cells[Lenguaje.traduce("Consumido")].Value.ToString());
                        Debug.Print("" + consumidoTotal);
                    }
                    row.Cells[Lenguaje.traduce("ConsumidoArticulos")].Value = consumidoTotal.ToString("G29");
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AddColumnaNecesario()
        {
            try
            {
                rgvGridArticulos.Columns.Remove("NecesarioArticulos");
                GridViewDecimalColumn necesariasArticulosColumna = new GridViewDecimalColumn();
                necesariasArticulosColumna.FieldName = "NecesarioArticulos";
                necesariasArticulosColumna.Name = "NecesarioArticulos";
                necesariasArticulosColumna.HeaderText = Lenguaje.traduce("Necesario");
                //necesariasArticulosColumna.FormatString = "{0:N2}";

                rgvGridArticulos.Columns.Add(necesariasArticulosColumna);

                foreach (GridViewRowInfo row in rgvGridArticulos.Rows)
                {
                    decimal necesarioTotal = 0;
                    foreach (GridViewRowInfo rowinfo in row.ChildRows)
                    {
                        necesarioTotal += Decimal.Parse(rowinfo.Cells[Lenguaje.traduce("Necesario")].Value.ToString());
                        Debug.Print("" + necesarioTotal);
                    }
                    row.Cells["NecesarioArticulos"].Value = necesarioTotal.ToString("G29");
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void MascaraNumerosArticulos()
        {
            try
            {
                if (rgvGridArticulos.Templates[0].Columns[Lenguaje.traduce("Solicitar")] != null)
                {
                    rgvGridArticulos.Templates[0].Columns[Lenguaje.traduce("Solicitar")].FormatString = "{0:N2}";
                }
                else log.Error("No encontrado la columna [Solicitar] en el grid de ProducciónArticulos.");

                rgvGridArticulos.Templates[0].Columns["Solicitado"].FormatString = "{0:N2}";
                rgvGridArticulos.Templates[0].Columns["Necesario"].FormatString = "{0:N2}";
                rgvGridArticulos.Templates[0].Columns["UDSNecesarias"].FormatString = "{0:N2}";
                rgvGridArticulos.Columns[Lenguaje.traduce("Acopiado")].FormatString = "{0:N2}";
                rgvGridArticulos.Columns[Lenguaje.traduce("Stock")].FormatString = "{0:N2}";

                for (int j = 0; j < this.rgvGridArticulos.Columns.Count; j++)
                {
                    if (this.rgvGridArticulos.Columns[j].FieldName == "UDSNecesarias")
                    {
                        this.rgvGridArticulos.Columns[j].FormatString = "{0:N0}";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AnyadirFilasResumenSumatorio()
        {
            try

            {
                int decimales = 6;
                if (Persistencia.getParametroInt("DECIMALESVISUALIZACION") > 0)
                {
                    decimales = Persistencia.getParametroInt("DECIMALESVISUALIZACION");
                }
                string formato = "{0:N" + decimales + "}";
                GridViewSummaryItem sumUdsNecesariasItem = new GridViewSummaryItem(Lenguaje.traduce("Necesario"), formato, GridAggregateFunction.Sum);
                GridViewSummaryItem sumCantidadConsumidaItem = new GridViewSummaryItem(Lenguaje.traduce("Consumido"), formato, GridAggregateFunction.Sum);
                GridViewSummaryItem sumSolicitadoItem = new GridViewSummaryItem(Lenguaje.traduce("Solicitado"), formato, GridAggregateFunction.Sum);
                sumSolicitarItem = new GridViewSummaryItem(Lenguaje.traduce("Solicitar"), formato, GridAggregateFunction.Sum);

                sumatorioFilaItem.Add(sumUdsNecesariasItem);
                sumatorioFilaItem.Add(sumCantidadConsumidaItem);
                sumatorioFilaItem.Add(sumSolicitadoItem);
                sumatorioFilaItem.Add(sumSolicitarItem);
                templateArticulos.SummaryRowsBottom.Add(sumatorioFilaItem);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void OcultarDatosGridArticulos()
        {
            try
            {
                /*NoVisibleArticulos();*/
                name = "ArticulosLineas";
                rbtnVistaArticulos.Text = Lenguaje.traduce("Producción");
                rgvGridArticulos.Columns[Lenguaje.traduce("SolicitadoAcopio")].IsVisible = false;
                /*gridViewArticulos.Columns[Lenguaje.traduce("Orden")].IsVisible = false;*/
                rgvGridArticulos.Templates[0].Columns.Remove(rgvGridArticulos.Templates[0].Columns["RowNum"]);
                foreach (TableScheme esquema in _lstEsquemaTablaArticulos)
                {
                    if (rgvGridArticulos.Columns[Lenguaje.traduce(esquema.Etiqueta)] != null)
                    {
                        Debug.WriteLine(esquema.Etiqueta);
                        rgvGridArticulos.Columns[Lenguaje.traduce(esquema.Etiqueta)].IsVisible = esquema.EsVisible;
                    }
                }
                foreach (TableScheme esquema in _lstEsquemaTablaJerarquicoArticulos)
                {
                    if (rgvGridArticulos.Templates[0].Columns[Lenguaje.traduce(esquema.Etiqueta)] != null)
                    {
                        Debug.WriteLine(esquema.Etiqueta);
                        rgvGridArticulos.Templates[0].Columns[Lenguaje.traduce(esquema.Etiqueta)].IsVisible = esquema.EsVisible;
                    }
                }
                rgvGridArticulos.BestFitColumns();
                rgvGridArticulos.Templates[0].BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void NoVisibleArticulos()
        {
            try
            {
                /*gridViewArticulos.Columns.Remove(gridViewArticulos.Columns[Lenguaje.traduce("Fabricado2")]);
                gridViewArticulos.Columns.Remove(gridViewArticulos.Columns[Lenguaje.traduce("UDSNecesarias")]);
                gridViewArticulos.Columns.Remove(gridViewArticulos.Columns[Lenguaje.traduce("CantidadConsumida2")]);*/
                /*gridViewArticulos.Columns[Lenguaje.traduce("Previstas Fabricacion")].IsVisible = false;
                gridViewArticulos.Columns[Lenguaje.traduce("Previstas Fabricacion")].VisibleInColumnChooser = true;
                gridViewArticulos.Columns[Lenguaje.traduce("Previstas Fabricacion")].AllowHide = true;*/
                /*gridViewArticulos.Columns[Lenguaje.traduce("Num PedidoFab")].IsVisible = false;
                gridViewArticulos.Columns[Lenguaje.traduce("Num PedidoFab")].VisibleInColumnChooser = true;
                gridViewArticulos.Columns[Lenguaje.traduce("Num PedidoFab")].AllowHide = true;*/
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion LlenarGridArticulos

        #region Eventos

        private void rgvGeneral_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.HierarchyLevel == 1 && e.RowElement.RowInfo is GridViewNewRowInfo)
            {
                e.RowElement.RowInfo.MaxHeight = 1;
            }
        }

        /*private void gridViewArticulos_GroupSummaryEvaluate(object sender, GroupSummaryEvaluationEventArgs e)
        {
            if (e.SummaryItem == sumSolicitarItem)
            {
                decimal valor=Convert.ToDecimal(e.Value);
                GridViewHierarchyRowInfo padre=(GridViewHierarchyRowInfo)e.Parent;
                decimal valorPadre = Convert.ToDecimal(padre.Cells["SolicitarArticulos"].Value);
                if (valor > valorPadre)
                {
                }
            }
        }*/

        private void FrmAcopiosProduccion_Load(object sender, EventArgs e)
        {
            try
            {
                GridViewTemplate gtArticulos = CreateChildTemplateArticulos();
                gtArticulos.Caption = strings.Articulos;
                this.rgvOrdenes.Templates.Add(gtArticulos);
                //AddColumnaUdsNecesariasJerarquicoOrdenes();
                //AddColumnaMermaJerarquicoOrdenes();
                //FormatearChildTemplateArticulos();
                GridViewTemplate gtSalidas = CreateChildTemplateSalidas();
                gtSalidas.Caption = strings.Consumidos;
                this.rgvOrdenes.Templates.Add(gtSalidas);
                //FormatearChildTemplateSalidas();
                GridViewTemplate gtEntradas = CreateChildTemplateEntradas();
                gtEntradas.Caption = strings.Producidos;
                this.rgvOrdenes.Templates.Add(gtEntradas);
                //FormatearChildTemplateEntradas();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void Form1_Load2(object sender, EventArgs e)
        {
            try
            {
                //if (CultureInfo.CurrentUICulture.Name == "es-ES")
                //{
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Orden")].IsVisible = false;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("UDSNecesarias")].FormatString = "{0:N0}";
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Cantidad Consumida")].FormatString = "{0:N0}";
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num PedidoFab")].IsVisible = false;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num PedidoFab")].VisibleInColumnChooser = true;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num PedidoFab")].AllowHide = true;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num Articulo")].IsVisible = false;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num Articulo")].VisibleInColumnChooser = true;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Num Articulo")].AllowHide = true;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("UDSNecesarias")].TextAlignment = ContentAlignment.BottomRight;
                rgvOrdenes.Templates[0].Columns[Lenguaje.traduce("Cantidad Consumida")].TextAlignment = ContentAlignment.BottomRight;
                rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad")].FormatString = "{0:N0}";
                rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad")].TextAlignment = ContentAlignment.BottomRight;
                rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad")].FormatString = "{0:N0}";
                rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad")].TextAlignment = ContentAlignment.BottomRight;
                rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad Unidad")].FormatString = "{0:N0}";
                rgvOrdenes.Templates[1].Columns[Lenguaje.traduce("Cantidad Unidad")].TextAlignment = ContentAlignment.BottomRight;
                rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad Unidad")].FormatString = "{0:N0}";
                rgvOrdenes.Templates[2].Columns[Lenguaje.traduce("Cantidad Unidad")].TextAlignment = ContentAlignment.BottomRight;
                //}
                //else
                //{
                //    radGridView1.Templates[0].Columns["Order"].IsVisible = false;
                //    radGridView1.Templates[0].Columns["Necessary Unities"].FormatString = "{0:N0}";
                //    radGridView1.Templates[0].Columns["QTY Consumed"].FormatString = "{0:N0}";
                //    radGridView1.Templates[0].Columns["Order Number"].IsVisible = false;
                //    radGridView1.Templates[0].Columns["Order Number"].VisibleInColumnChooser = true;
                //    radGridView1.Templates[0].Columns["Order Number"].AllowHide = true;
                //    radGridView1.Templates[0].Columns["Article Number"].IsVisible = false;
                //    radGridView1.Templates[0].Columns["Article Number"].VisibleInColumnChooser = true;
                //    radGridView1.Templates[0].Columns["Article Number"].AllowHide = true;
                //    radGridView1.Templates[0].Columns["Necessary Unities"].TextAlignment = ContentAlignment.BottomRight;
                //    radGridView1.Templates[0].Columns["QTY Consumed"].TextAlignment = ContentAlignment.BottomRight;
                //    radGridView1.Templates[1].Columns["QTY"].FormatString = "{0:N0}";
                //    radGridView1.Templates[1].Columns["QTY"].TextAlignment = ContentAlignment.BottomRight;
                //    radGridView1.Templates[2].Columns["QTY"].FormatString = "{0:N0}";
                //    radGridView1.Templates[2].Columns["QTY"].TextAlignment = ContentAlignment.BottomRight;
                //    radGridView1.Templates[1].Columns["Unity QTY"].FormatString = "{0:N0}";
                //    radGridView1.Templates[1].Columns["Unity QTY"].TextAlignment = ContentAlignment.BottomRight;
                //    radGridView1.Templates[2].Columns["Unity QTY"].FormatString = "{0:N0}";
                //    radGridView1.Templates[2].Columns["Unity QTY"].TextAlignment = ContentAlignment.BottomRight;
                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvGridOrdenes_RowSourceNeeded(object sender, GridViewRowSourceNeededEventArgs e)
        {
            try
            {
                string prueba = string.Empty;
                DataRowView rowView = e.ParentRow.DataBoundItem as DataRowView;
                DataRow[] rows = rowView.Row.GetChildRows(Lenguaje.traduce("Num PedidoFab"));
                prueba = e.ParentRow.Cells[Lenguaje.traduce("Num PedidoFab")].Value.ToString();

                if (e.Template.Caption == strings.Consumidos)
                {
                    DataTable salidas = Business.GetAcopiosSalidasDatosGridView(prueba);

                    foreach (DataRow dataRow in salidas.Rows)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        for (int i = 0; i < salidas.Columns.Count; i++)
                        {
                            GridViewCellInfo columna = row.Cells[salidas.Columns[i].ColumnName]; ;
                            try
                            {
                                if (columna != null)
                                {
                                    if (columna.ColumnInfo.HeaderText != null)
                                    {
                                        if (row.Cells[salidas.Columns[i].ColumnName] != null && !(dataRow[salidas.Columns[i].ColumnName] is DBNull))
                                        {
                                            Utilidades.ConversorMascaraTipo(salidas, row, dataRow, i);

                                            /*
                                                if (e.Column.FieldName == "Fecha Caducidad")
                                                {
                                                    if (!string.IsNullOrWhiteSpace(e.CellElement.Value.ToString()))
                                                    {
                                                        if (e.CellElement.Value.GetType().Name == "DateTime")
                                                        {
                                                            CultureInfo culture = new CultureInfo("es-ES");
                                                            string fechaCreacion = e.CellElement.Value.ToString();
                                                            DateTime date = DateTime.Parse(fechaCreacion, culture);
                                                            string fecha = date.ToString("dd/MM/yyyy");
                                                            e.CellElement.Value = fecha;
                                                        }
                                                    }
                                                    ///      e.CellElement.Value = fechaCreacion.ToString("dd/MM/yyyy");
                                                }*/
                                        }

                                        /*if (columna.ColumnInfo.HeaderText.Equals(Lenguaje.traduce("Cantidad")))
                                         {
                                             int idarticulo = Convert.ToInt32(e.ParentRow.Cells[Lenguaje.traduce("ID Articulo")].Value);

                                             int cantidad = Convert.ToInt32(dataRow[salidas.Columns[i].ColumnName]);
                                             object[] presPrev = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idarticulo, cantidad);
                                             row.Cells[salidas.Columns[i].ColumnName].Value = Convert.ToDecimal(presPrev[0], CultureInfo.CurrentUICulture);
                                             if (row.Cells[Lenguaje.traduce("Tipo Unidad")] != null)
                                                 row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presPrev[1].ToString();
                                         }
                                         else if (columna.ColumnInfo.HeaderText.Equals(Lenguaje.traduce("Cantidad Unidad")))
                                         {
                                            int idarticulo = Convert.ToInt32(e.ParentRow.Cells[Lenguaje.traduce("ID Articulo")].Value);

                                             int cantidadunidad = Convert.ToInt32(dataRow[salidas.Columns[i].ColumnName]);
                                             object[] presPrev = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idarticulo, cantidadunidad);
                                             row.Cells[salidas.Columns[i].ColumnName].Value = Convert.ToDecimal(presPrev[0], CultureInfo.CurrentUICulture);
                                             if (row.Cells[Lenguaje.traduce("Tipo Unidad")] != null)
                                                 row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presPrev[1].ToString();
                                         }
                                         else
                                         {
                                             if (columna.ColumnInfo.HeaderText.Equals(Lenguaje.traduce("Tipo Unidad"))) {
                                                 int idPresentacion = Convert.ToInt32(e.ParentRow.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                                             if (idPresentacion <= 0)
                                             {
                                                 columna.Value = dataRow[columna.ColumnInfo.HeaderText].ToString();
                                             }
                                         }else
                                         {
                                                 columna.Value = dataRow[columna.ColumnInfo.HeaderText].ToString();
                                             }
                                         }*/
                                    }
                                }
                                //}
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error cargando datos columna " + columna.ColumnInfo.HeaderText);
                            }
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                else if (e.Template.Caption == strings.Articulos)
                {
                    DataTable articulos = Business.GetAcopiosArticulosDatosGridView(prueba);
                    foreach (DataRow dataRow in articulos.Rows)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        for (int i = 0; i < articulos.Columns.Count; i++)
                        {
                            GridViewCellInfo columna = row.Cells[articulos.Columns[i].ColumnName];
                            try
                            {
                                if (columna.ColumnInfo.HeaderText.Equals("%Merma"))
                                {
                                    row.Cells[articulos.Columns[i].ColumnName].ColumnInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;

                                    if (dataRow[i].ToString().Equals(""))
                                    {
                                        row.Cells[articulos.Columns[i].ColumnName].Value = 0;
                                    }
                                }
                                else
                                {
                                    if (row.Cells[articulos.Columns[i].ColumnName] != null && !(dataRow[articulos.Columns[i].ColumnName] is DBNull))
                                    {
                                        Utilidades.ConversorMascaraTipo(articulos, row, dataRow, i);

                                        //if (articulos.Columns[i].DataType == typeof(Decimal))
                                        //{
                                        //    row.Cells[articulos.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Decimal.Parse(dataRow[articulos.Columns[i].ColumnName].ToString()));
                                        //}
                                        //row.Cells[articulos.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Decimal.Parse(dataRow[articulos.Columns[i].ColumnName].ToString()));

                                        //if (articulos.Columns[i].DataType == typeof(Single))
                                        //{
                                        //    row.Cells[articulos.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Single.Parse(dataRow[articulos.Columns[i].ColumnName].ToString()));
                                        //}
                                        //if (articulos.Columns[i].DataType == typeof(Double))
                                        //{
                                        //    row.Cells[articulos.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Double.Parse(dataRow[articulos.Columns[i].ColumnName].ToString()));
                                        //}
                                        //if (articulos.Columns[i].DataType == typeof(Int64))
                                        //{
                                        //    row.Cells[articulos.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Int64.Parse(dataRow[articulos.Columns[i].ColumnName].ToString()));
                                        //}

                                        //if (articulos.columns[i].datatype == typeof(decimal))
                                        //{
                                        //    row.cells[articuloscolumns[i].columnname].value = decimal.parse(datarow[articulos.columns[i].columnname].tostring()).tostring("g29");
                                        //}
                                    }
                                    else
                                    {
                                        row.Cells[articulos.Columns[i].ColumnName].Value = dataRow[articulos.Columns[i].ColumnName].ToString();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error cargando datos columna " + columna.ColumnInfo.HeaderText);
                            }
                            //row.Cells[i].Value = dataRow[i].ToString();
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                else if (e.Template.Caption == strings.Producidos)
                {
                    DataTable entradas = Business.GetAcopiosEntradasDatosGridView(prueba);
                    foreach (DataRow dataRow in entradas.Rows)
                    {
                        GridViewRowInfo row = e.Template.Rows.NewRow();
                        for (int i = 0; i < entradas.Columns.Count; i++)
                        {
                            GridViewCellInfo columna = row.Cells[entradas.Columns[i].ColumnName];
                            try
                            {
                                if (columna != null)
                                {
                                    if (columna.ColumnInfo.HeaderText != null)
                                    {
                                        /* if (columna.ColumnInfo.HeaderText.Equals(Lenguaje.traduce("Cantidad")))
                                         {
                                             //int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                                             int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                                             int cantidad = Convert.ToInt32(dataRow[i]);
                                             object[] presPrev = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidad);
                                             row.Cells[entradas.Columns[i].ColumnName].Value = Convert.ToDouble(presPrev[0]);
                                             if (row.Cells[Lenguaje.traduce("Tipo Unidad")] != null)
                                                 row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presPrev[1].ToString();
                                         }else if (columna.ColumnInfo.HeaderText.Equals(Lenguaje.traduce("Cantidad Unidad")))
                                         {
                                             //int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                                             int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                                             int cantidadunidad = Convert.ToInt32(dataRow[i]);
                                             object[] presPrev = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidadunidad);
                                             row.Cells[entradas.Columns[i].ColumnName].Value = Convert.ToDouble(presPrev[0]);
                                             if (row.Cells[Lenguaje.traduce("Tipo Unidad")] != null)
                                                 row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presPrev[1].ToString();
                                         }else if(columna.ColumnInfo.HeaderText.Equals(Lenguaje.traduce("Tipo Unidad"))){
                                             //Cuando sea Tipo Unidad que no haga nada ya el valor se pone en Cantidad.
                                         }
                                         else
                                         {
                                             columna.Value = dataRow[columna.ColumnInfo.HeaderText].ToString();
                                         }*/
                                        if (row.Cells[entradas.Columns[i].ColumnName] != null && !(dataRow[entradas.Columns[i].ColumnName] is DBNull))
                                        {
                                            Utilidades.ConversorMascaraTipo(entradas, row, dataRow, i);
                                        }
                                    }
                                }
                                //}
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error cargando datos columna " + columna.ColumnInfo.HeaderText);
                            }
                            //row.Cells[i].Value = dataRow[i].ToString();
                        }
                        e.SourceCollection.Add(row);
                    }
                }
                //FormatearChildTemplateArticulos();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        protected void rgvGeneral_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            try
            {
                GridDataView dataView = rgvOrdenes.MasterTemplate.DataView as GridDataView;
                lblCantidad.Text = strings.Registros + dataView.Indexer.Items.Count;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvGridOrdenes_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Fecha Caducidad")
                {
                    if (!string.IsNullOrWhiteSpace(e.CellElement.Value.ToString()))
                    {
                        if (e.CellElement.Value.GetType().Name == "DateTime")
                        {
                            CultureInfo culture = new CultureInfo("es-ES");
                            string fechaCreacion = e.CellElement.Value.ToString();
                            DateTime date = DateTime.Parse(fechaCreacion, culture);
                            string fecha = date.ToString("dd/MM/yyyy");
                            e.CellElement.Value = fecha;
                        }
                    }
                    ///      e.CellElement.Value = fechaCreacion.ToString("dd/MM/yyyy");
                }
                if (e.ColumnIndex != null)
                {
                    if (e.RowIndex > -1 && e.ColumnIndex > -1)
                    {
                        e.CellElement.TextWrap = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvGridArticulos_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement is GridHeaderCellElement)
                {
                    e.CellElement.TextWrap = true;
                }

                GridViewDataColumn columna = rgvGridArticulos.Columns[e.Column.HeaderText];
                if (columna != null && columna.DataType != null)
                {
                    if (columna.DataType == typeof(Decimal))
                    {
                        columna.FormatString = "{0:N2}";
                    }
                    if (columna.DataType == typeof(Single))
                    {
                        columna.FormatString = "{0:N2}";
                    }
                    if (columna.DataType == typeof(Double))
                    {
                        columna.FormatString = "{0:N2}";
                    }
                    if (columna.FieldName.Equals(Lenguaje.traduce("Cantidad")) && columna.DataType == typeof(Int32))
                    {
                        columna.FormatString = "{0:N2}";
                    }
                    if (columna.FieldName.Equals(Lenguaje.traduce("UDSNecesarias")) && columna.DataType == typeof(Int32))
                    {
                        columna.FormatString = "{0:N2}";
                    }
                }
                if (e.CellElement.ColumnInfo.Name.Equals("UDSNecesarias"))
                {
                    e.CellElement.FormatString = "{0:N0}";
                    //((GridViewDateTimeColumn)this.rgvOrdenes.Columns[j]).Format = Format;
                    //((GridViewDateTimeColumn)this.rgvOrdenes.Columns[j]).CustomFormat = "MM/dd/yyyy";
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvGridOrdenes_CellFormattingWrapText(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement is GridHeaderCellElement)
                {
                    e.CellElement.TextWrap = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvGridOrdenes_CustomSorting(object sender, GridViewCustomSortingEventArgs e)
        {
            try
            {
                int descriptorIndex = -1;
                for (int i = 0; i < this.rgvOrdenes.SortDescriptors.Count; i++)
                {
                    if (rgvOrdenes.SortDescriptors[i].PropertyName == Lenguaje.traduce("Fabricado"))
                    {
                        descriptorIndex = i;
                        break;
                    }
                }
                if (descriptorIndex != -1)
                {
                    int cellValue1 = int.Parse(e.Row1.Cells[Lenguaje.traduce("Fabricado")].Value.ToString());
                    int cellValue2 = int.Parse(e.Row2.Cells[Lenguaje.traduce("Fabricado")].Value.ToString());

                    int result = cellValue1 - cellValue2;
                    if (result != 0)
                    {
                        if (this.rgvOrdenes.SortDescriptors[descriptorIndex].Direction == ListSortDirection.Descending)
                        {
                            result = -result;
                        }
                    }
                    e.SortResult = (int)result;
                }
                else
                {
                    e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvGridArticulos_CustomSorting(object sender, GridViewCustomSortingEventArgs e)
        {
            try
            {
                int descriptorIndex = -1;
                for (int i = 0; i < this.rgvGridArticulos.SortDescriptors.Count; i++)
                {
                    if (rgvGridArticulos.SortDescriptors[i].PropertyName == "NoFabricado")
                    {
                        descriptorIndex = i;
                        break;
                    }
                }
                if (descriptorIndex != -1)
                {
                    int cellValue1 = int.Parse(e.Row1.Cells["NoFabricado"].Value.ToString());
                    int cellValue2 = int.Parse(e.Row2.Cells["NoFabricado"].Value.ToString());

                    int result = cellValue1 - cellValue2;
                    if (result != 0)
                    {
                        if (this.rgvGridArticulos.SortDescriptors[descriptorIndex].Direction == ListSortDirection.Descending)
                        {
                            result = -result;
                        }
                    }
                    e.SortResult = (int)result;
                }
                else
                {
                    e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvPrincipal_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                if (e.CellElement is GridSummaryCellElement)
                {
                    e.CellElement.TextAlignment = ContentAlignment.BottomRight;
                }
                //if (e.CellElement != null && e.CellElement.Value != null && e.CellElement.Value != "" && e.CellElement.Value.GetType() != null)
                //{
                //    if (e.CellElement.Value.GetType() == typeof(Decimal))
                //    {
                //        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Decimal.Parse(e.CellElement.Value.ToString()));
                //    }
                //    if (e.CellElement.Value.GetType() == typeof(Single))
                //    {
                //        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Single.Parse(e.CellElement.Value.ToString()));
                //    }
                //    if (e.CellElement.Value.GetType() == typeof(Double))
                //    {
                //        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Double.Parse(e.CellElement.Value.ToString()));
                //    }
                //    if (e.CellElement.Value.GetType() == typeof(Int64))
                //    {
                //        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Int64.Parse(e.CellElement.Value.ToString()));
                //    }
                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        /* No hace nadavoid rgvPrincipal_ValueChanged(object sender, EventArgs e)
         {
             try
             {
                 if (this.rgvOrdenes.ActiveEditor is RadCheckBoxEditor)
                 {
                     Debug.WriteLine(this.rgvOrdenes.CurrentCell.RowIndex);
                     Debug.WriteLine(this.rgvOrdenes.ActiveEditor.Value);
                 }
             }
             catch (Exception ex)
             {
                 ExceptionManager.GestionarError(ex);
                 log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
             }
         }*/

        private void rgvPrincipal_ViewCellFormattingHierarchicalGrid(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            try
            {
                GridDetailViewCellElement detailsCell = e.CellElement as GridDetailViewCellElement;
                if (detailsCell != null)
                {
                    e.CellElement.Padding = new Padding(0);
                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.PaddingProperty, ValueResetFlags.Local);
                }
                if (e.Column.FieldName == "Fecha Caducidad")
                {
                    if (!string.IsNullOrWhiteSpace(e.CellElement.Value.ToString()))
                    {
                        if (e.CellElement.Value.GetType().Name == "DateTime")
                        {
                            CultureInfo culture = new CultureInfo("es-ES");
                            string fechaCreacion = e.CellElement.Value.ToString();
                            DateTime date = DateTime.Parse(fechaCreacion, culture);
                            string fecha = date.ToString("dd/MM/yyyy");
                            e.CellElement.Value = new DateTime(date.Year, date.Month, date.Day);
                        }
                    }
                    ///      e.CellElement.Value = fechaCreacion.ToString("dd/MM/yyyy");
                }

                if (e.CellElement != null && e.CellElement.Value != null && e.CellElement.Value != "" && e.CellElement.Value.GetType() != null)
                {
                    if (e.CellElement.Value.GetType() == typeof(Decimal))
                    {
                        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Decimal.Parse(e.CellElement.Value.ToString()));
                    }
                    if (e.CellElement.Value.GetType() == typeof(Single))
                    {
                        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Single.Parse(e.CellElement.Value.ToString()));
                    }
                    if (e.CellElement.Value.GetType() == typeof(Double))
                    {
                        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Double.Parse(e.CellElement.Value.ToString()));
                    }
                    if (e.CellElement.Value.GetType() == typeof(Int64))
                    {
                        // e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Int64.Parse(e.CellElement.Value.ToString()));
                    }
                }

                /*   if (e.CellElement.ColumnInfo.Name.Equals("UDSNecesarias"))
                      {
                        e.CellElement.Value = String.Format(CultureInfo.InvariantCulture, "{0:N2}", Decimal.Parse(e.CellElement.Value.ToString()));
                    }
                */
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvPrincipal_ContextMenuOpening(object sender, Telerik.WinControls.UI.ContextMenuOpeningEventArgs e)
        {
            try
            {
                for (int i = 0; i < e.ContextMenu.Items.Count; i++)
                {
                    if (e.ContextMenu.Items[i].Text == Lenguaje.traduce("Borrar Fila"))
                    {
                        e.ContextMenu.Items.Remove(e.ContextMenu.Items[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Eventos

        #region Preferencias

        private void SetPreferences()
        {
            try
            {
                rgvOrdenes.BestFitColumns();
                for (int i = 1; i < rgvOrdenes.Columns.Count; i++)
                {
                    rgvOrdenes.Columns[i].ReadOnly = true;
                }
                rgvOrdenes.MultiSelect = true;
                this.rgvOrdenes.MasterTemplate.EnableGrouping = true;
                this.rgvOrdenes.ShowGroupPanel = true;
                this.rgvOrdenes.MasterTemplate.AutoExpandGroups = true;
                this.rgvOrdenes.EnableHotTracking = true;
                this.rgvOrdenes.MasterTemplate.AllowAddNewRow = false;
                this.rgvOrdenes.MasterTemplate.AllowColumnResize = true;
                this.rgvOrdenes.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvOrdenes.AllowColumnReorder = true;
                this.rgvOrdenes.AllowSearchRow = true;
                this.rgvOrdenes.EnablePaging = false;
                this.rgvOrdenes.TableElement.RowHeight = 40;
                this.rgvOrdenes.AllowRowResize = false;
                this.rgvOrdenes.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvOrdenes.EnableFiltering = true;
                rgvOrdenes.MasterTemplate.EnableFiltering = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void SetPreferences2()
        {
            try
            {
                rgvGridArticulos.BestFitColumns();
                rgvGridArticulos.MultiSelect = true;
                this.rgvGridArticulos.MasterTemplate.EnableGrouping = true;
                this.rgvGridArticulos.ShowGroupPanel = true;
                this.rgvGridArticulos.MasterTemplate.AutoExpandGroups = true;
                this.rgvGridArticulos.EnableHotTracking = true;
                this.rgvGridArticulos.MasterTemplate.AllowAddNewRow = false;
                this.rgvGridArticulos.MasterTemplate.AllowColumnResize = true;
                this.rgvGridArticulos.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvGridArticulos.AllowSearchRow = true;
                this.rgvGridArticulos.AllowColumnReorder = true;
                this.rgvGridArticulos.EnablePaging = false;
                this.rgvGridArticulos.TableElement.RowHeight = 40;
                this.rgvGridArticulos.AllowRowResize = false;
                this.rgvGridArticulos.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvGridArticulos.EnableFiltering = true;
                rgvGridArticulos.MasterTemplate.EnableFiltering = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Preferencias

        #region Filtros

        private void setFiltros1()
        {
            try
            {
                FilterDescriptor filter1 = new FilterDescriptor(Lenguaje.traduce("Estado Fabricacion"), FilterOperator.IsNotEqualTo, "PC");
                filter1.IsFilterEditor = true;
                this.rgvOrdenes.FilterDescriptors.Add(filter1);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Filtros

        #region Boton ruedecilla

        private void ItemColumnas_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón editar columnas " + DateTime.Now);
                if (rbtnVistaArticulos.Text == strings.Articulos)
                {
                    rgvOrdenes.ShowColumnChooser();
                }
                else if (rbtnVistaArticulos.Text == strings.Produccion)
                {
                    rgvGridArticulos.ShowColumnChooser();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void LoadItem_Click(object sender, EventArgs e)
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

        private void MenuOrdenesEnCurso_Click(object sender, EventArgs e)
        {
            rgvOrdenes.FilterDescriptors.Clear();
            log.Info("Pulsado botón MenuOrdenesEnCurso_Click " + DateTime.Now);
            CompositeFilterDescriptor compositeFilter1 = new CompositeFilterDescriptor();
            compositeFilter1.FilterDescriptors.Add(new FilterDescriptor(Lenguaje.traduce("Estado Fabricacion"), FilterOperator.NotContains, "PC"));
            compositeFilter1.FilterDescriptors.Add(new FilterDescriptor(Lenguaje.traduce("Estado Fabricacion"), FilterOperator.NotContains, "PS"));
            compositeFilter1.FilterDescriptors.Add(new FilterDescriptor(Lenguaje.traduce("Estado Fabricacion"), FilterOperator.NotContains, "PP"));
            compositeFilter1.LogicalOperator = FilterLogicalOperator.And;

            rgvOrdenes.FilterDescriptors.Add(compositeFilter1);
        }

        public void MenuOrdenesPendientes_Click(object sender, EventArgs e)
        {
            rgvOrdenes.FilterDescriptors.Clear();
            log.Info("Pulsado botón MenuOrdenesEnCurso_Click " + DateTime.Now);
            CompositeFilterDescriptor compositeFilter1 = new CompositeFilterDescriptor();
            compositeFilter1.FilterDescriptors.Add(new FilterDescriptor(Lenguaje.traduce("Estado Fabricacion"), FilterOperator.IsEqualTo, "PS"));
            compositeFilter1.FilterDescriptors.Add(new FilterDescriptor(Lenguaje.traduce("Estado Fabricacion"), FilterOperator.IsEqualTo, "PP"));

            compositeFilter1.LogicalOperator = FilterLogicalOperator.Or;
            rgvOrdenes.FilterDescriptors.Add(compositeFilter1);
        }

        public void MenuSinFiltro_Click(object sender, EventArgs e)
        {
            rgvOrdenes.FilterDescriptors.Clear();
        }

        public void SaveItem_Click(object sender, EventArgs e)
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

        private void FiltroMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón filtro " + DateTime.Now);
                dataFilter.DataSource = rgvOrdenes.DataSource;
                dataFilter.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Boton ruedecilla

        #region BotonesPrincipales

        private string confirmacion = strings.ExportacionExito;

        public void btnExportacion_Click(object sender, EventArgs e)
        {
            if (rbtnVistaArticulos.Text == strings.Articulos)
            {
                FuncionesGenerales.exportarAExcelGenerico(this.rgvOrdenes);
            }
            else
            {
                FuncionesGenerales.exportarAExcelGenerico(this.rgvGridArticulos);
            }
        }

        private void btnBorrarFiltro_Click(object sender, EventArgs e)
        {
            try
            {
                if (rgvOrdenes.IsInEditMode)
                {
                    rgvOrdenes.EndEdit();
                }
                rgvOrdenes.FilterDescriptors.Clear();
                rgvOrdenes.GroupDescriptors.Clear();
                setFiltros1();
                rgvOrdenes.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnVistaArticulos_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbtnVistaArticulos.Text == Lenguaje.traduce(strings.Articulos))
                {
                    ClickArticulosDatos();
                    log.Info("Pulsado botón articulos " + DateTime.Now);
                }
                else if (rbtnVistaArticulos.Text == Lenguaje.traduce(strings.Produccion))
                {
                    ClickProduccionDatos();
                    log.Info("Pulsado botón produccion " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ClickArticulosDatos()
        {
            try
            {
                string filtroArticulos = extraerIdOrdenesFab();
                if (filtroArticulos == String.Empty)
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorDatosRelacionados));
                }
                else
                {
                    //ComprobacionGenerarAcopio();
                    tableLayoutPanel1.Controls.Remove(rgvOrdenes);
                    tableLayoutPanel1.Controls.Add(rgvGridArticulos, 0, 0);
                    tableLayoutPanel1.SetColumnSpan(rgvGridArticulos, tableLayoutPanel1.ColumnCount);
                    rgvGridArticulos.Dock = DockStyle.Fill;
                    rddlFiltrarMaquina.Enabled = false;
                    grupoLabel.Visibility = ElementVisibility.Hidden;
                    rddFiltradoPor.Visibility = ElementVisibility.Collapsed;
                    accionesArtículosBarGgroup.Visibility = ElementVisibility.Visible;
                    pedidosBarGroup.Visibility = ElementVisibility.Collapsed;
                    maqBarGroup.Visibility = ElementVisibility.Collapsed;
                    this.rgvGridArticulos.EnablePaging = false;

                    rWaitArticulos.StartWaiting();
                    bgWorkerGridViewArticulos.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void refrescar()
        {
            try
            {
                if (rbtnVistaArticulos.Text == Lenguaje.traduce(strings.Articulos))
                {
                    rgvOrdenes.Columns.Remove(chkSeleccionOrden);
                    rgvOrdenes.Relations.Clear();
                    rgvOrdenes.Columns.Clear();
                    Utilidades.refrescarJerarquico(ref rgvOrdenes, -1);
                    this.rgvOrdenes.EnablePaging = false;
                    //César: Se acumulan los worker, cuando pulsas varias veces refrescar
                    //bgWorkerGridViewOrdenes.DoWork += new DoWorkEventHandler(llenarGridOrdenes);
                    rWaitOrdenes.StartWaiting();
                    bgWorkerGridViewOrdenes.RunWorkerAsync();
                }
                else if (rbtnVistaArticulos.Text == Lenguaje.traduce(strings.Produccion))
                {
                    rgvGridArticulos.Relations.Clear();
                    rgvGridArticulos.Columns.Clear();
                    Utilidades.refrescarJerarquico(ref rgvOrdenes, -1);
                    templateArticulos.SummaryRowsBottom.Clear();
                    sumatorioFilaItem.Clear();
                    this.rgvGridArticulos.EnablePaging = false;
                    //César: Se acumulan los worker, cuando pulsas varias veces refrescar
                    //bgWorkerGridViewArticulos.DoWork += new DoWorkEventHandler(LlenarGridArticulos);
                    rWaitArticulos.StartWaiting();
                    bgWorkerGridViewArticulos.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ClickProduccionDatos()
        {
            try
            {
                ComprobacionAcceso();
                //ComprobacionCerrarOrden();
                rgvGridArticulos.Relations.Clear();
                rgvGridArticulos.Columns.Clear();
                templateArticulos.Columns.Clear();
                templateArticulos.SummaryRowsBottom.Clear();
                sumatorioFilaItem.Clear();
                tableLayoutPanel1.Controls.Remove(rgvGridArticulos);
                tableLayoutPanel1.Controls.Add(rgvOrdenes);
                rddlFiltrarMaquina.Enabled = true;
                rddFiltradoPor.Visibility = ElementVisibility.Visible;
                pedidosBarGroup.Visibility = ElementVisibility.Visible;
                maqBarGroup.Visibility = ElementVisibility.Visible;
                accionesArtículosBarGgroup.Visibility = ElementVisibility.Collapsed;
                grupoLabel.Visibility = ElementVisibility.Visible;
                SetPreferences();
                name = "Produccion";
                rgvOrdenes.Columns[0].ReadOnly = false;
                rbtnVistaArticulos.Text = strings.Articulos;
                previstasFabricarEditable(true);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rbtnSolicitar_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón solicitar web service " + DateTime.Now);
            LLamarWS();
        }

        private void MaquinaButtonWS_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón maquina web service " + DateTime.Now);
                string check = string.Empty;
                foreach (GridViewRowInfo rowInfo in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(rowInfo.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value) == true)
                    {
                        check = check + rowInfo.Cells[Lenguaje.traduce("Num PedidoFab")].Value.ToString() + " ,";
                    }
                }
                if (check == string.Empty)
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorFilasMarcadas));
                }
                else
                {
                    ElegirMaquina maquinas = new ElegirMaquina();
                    if (maquinas.ShowDialog() == DialogResult.OK)
                    {
                        int idMaquina = maquinas.idMaquina;
                        string desc = maquinas.descripcion;
                        AsignarMaquinaWS(Convert.ToInt32(idMaquina), desc);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private int EncontrarCheckBox(GridViewColumnCollection columns)
        {
            foreach (var col in columns)
            {
                if (col.GetType().Name == "GridViewCheckBoxColumn")
                    return col.Index;
            }
            return 0;
        }

        private void rBtnCerrarOrden_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón cerrar orden web service " + DateTime.Now);
            CerrarOrdenWS();
        }

        private void btnConsumir_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón consumir " + DateTime.Now);
                ordenes = extraerIdOrdenesFab();
                if (ordenes == string.Empty)
                { }
                else
                {
                    ConsumosOrdenFab consumosOrden = new ConsumosOrdenFab(ordenes);
                    consumosOrden.ShowDialog();
                    refrescar();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnConsumirMatricula_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón consumir " + DateTime.Now);
                ordenes = extraerIdOrdenesFab();
                if (ordenes == string.Empty)
                { }
                else
                {
                    ConsumosOrdenFab consumosOrden = new ConsumosOrdenFab(ordenes);
                    refrescar();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AltaButton_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón alta producto " + DateTime.Now);
            AltaProductoLlamadaWS();
        }

        private void rbtnRefrescarButton_Click(object sender, EventArgs e)
        {
            refrescar();
        }

        private void EventoAbrirAltaProducto_Click(object sender, EventArgs e)
        {
            try
            {
                String idOrden = "";
                int contador = 0;
                string operario = string.Empty;
                string tipoPalet = string.Empty;
                string maquina = string.Empty;
                foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                {
                    if ((Convert.ToBoolean(row.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value)) == true)
                    {
                        idOrden = row.Cells[Lenguaje.traduce("Orden")].Value.ToString();
                        contador = contador + 1;
                    }
                }
                if (contador == 1)
                {
                    //var respuesta = motorClient.getDatosDefectoAlta(idOrden);
                    //var atributosEntrada = entrada.getAtributosEntrada();F
                    //AltaProductoWS alta = new AltaProductoWS(respuesta,atributosEntrada,operario, tipoPalet, maquina,idOrden);
                    String nombreBotonPulsado = (sender as RumMenuItem).Name;
                    Image icono = (sender as RumMenuItem).Image;
                    String nombreFormularioOpcion = (sender as RumMenuItem).Text;
                    int opcion = 0;
                    switch (nombreBotonPulsado)
                    {
                        case "rBtnAltaPalet":
                            opcion = FormularioAltaProducto.AltaPalet;
                            break;

                        case "rBtnAltaPico":
                            opcion = FormularioAltaProducto.AltaPaletPicos;
                            break;

                        case "rBtnAltaMulti":
                            opcion = FormularioAltaProducto.AltaPaletMultireferencia;
                            break;

                        case "rBtnAltaCarro":
                            opcion = FormularioAltaProducto.AltaPaletCarroMovil;
                            break;

                        case "rBtnAltaTotales":
                            opcion = FormularioAltaProducto.AltaPaletEntradasTotales;
                            break;
                    }

                    FormularioAltaProducto fap =
                        new FormularioAltaProducto(opcion, idOrden, nombreFormularioOpcion, icono);
                    fap.ShowDialog();
                    refrescar();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener una única fila marcada."));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        #endregion BotonesPrincipales

        #region Permisos

        private void ComprobacionAcceso()
        {
            /*if (User.Perm.comprobarAcceso(20068) == false)
            {
                btnAsignarMaquina.Enabled = false;
                rbtnConsumir.Enabled = false;
            }
            else
            {
                if (User.Perm.tienePermisoEscritura(20068) == true)
                {
                    btnAsignarMaquina.Enabled = true;
                    rbtnConsumir.Enabled = true;
                }
                else
                {
                    btnAsignarMaquina.Enabled = false;
                    rbtnConsumir.Enabled = false;
                }
            }*/
            Permiso(rBtnInicioOrden, 500090);
            Permiso(btnAsignarMaquina, 20068);
            Permiso(rbtnConsumir, 500013);
            Permiso(rBtnConsumirMatricula, 500043);
            Permiso(rBtnEliminarAcopios, 500098);
            Permiso(rBtnAnularConsumo, 500042);
            Permiso(rBtnAnularConsumoParcial, 5000100);
            Permiso(rBtnAnularEntrada, 500043);
            Permiso(rbtnCerrarOrden, 500007);
        }

        private void Permiso(RadButtonItem control, int id)
        {
            if (User.Perm.comprobarAcceso(id) == false)
            {
                control.Enabled = false;
            }
            else
            {
                if (User.Perm.tienePermisoEscritura(id) == true)
                {
                    control.Enabled = true;
                }
                else
                {
                    control.Enabled = false;
                }
            }
        }

        private void ComprobacionGenerarAcopio()
        {
            if (User.Perm.comprobarAcceso(50015) == false)
            {
                rbtnSolicitar.Enabled = false;
            }
            else
            {
                if (User.Perm.tienePermisoEscritura(50015) == true)
                {
                    rbtnSolicitar.Enabled = true;
                }
                else
                {
                    rbtnSolicitar.Enabled = false;
                }
            }
        }

        //private void ComprobacionCerrarOrden()
        //{
        //    if (User.Perm.comprobarAcceso(500007) == false)
        //    {
        //        rbtnCerrarOrden.Enabled = false;
        //    }
        //    else
        //    {
        //        if (User.Perm.tienePermisoEscritura(500007) == true)
        //        {
        //            rbtnCerrarOrden.Enabled = true;
        //        }
        //        else
        //        {
        //            rbtnCerrarOrden.Enabled = false;
        //        }
        //    }
        //}

        #endregion Permisos

        #region Maquinas

        private void rddlFiltrarMaquina_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rddlFiltrarMaquina.Text == "" || rddlFiltrarMaquina.Text == Lenguaje.traduce(strings.Todas))
                {
                    rgvOrdenes.FilterDescriptors.Clear();
                    setFiltros1();
                }
                else
                {
                    rgvOrdenes.FilterDescriptors.Clear();
                    setFiltros1();
                    FilterDescriptor filter1 = new FilterDescriptor(Lenguaje.traduce("Línea Producción"), FilterOperator.IsEqualTo, rddlFiltrarMaquina.SelectedText);
                    filter1.IsFilterEditor = true;
                    this.rgvOrdenes.FilterDescriptors.Add(filter1);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Maquinas

        #region Estilos

        public void SaveLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal; /*XmlReaderPropio.getLayoutPath(0);*/
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
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + /*this.Name*/ name + "GridView.xml";
                    //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "GridView.xml";

                    path.Replace(" ", "_");
                    if (name == "Produccion")
                    {
                        indexFabricado = rgvOrdenes.Columns[Lenguaje.traduce(strings.PorcentajeFabricado)].Index;
                        rgvOrdenes.Columns.Remove(rgvOrdenes.Columns[Lenguaje.traduce(strings.PorcentajeFabricado)]);
                        rgvOrdenes.SaveLayout(path);
                        rgvOrdenes.Columns.Add(pctFabricadoColumna);
                        rgvOrdenes.Columns.Move(pctFabricadoColumna.Index, indexFabricado);
                        pctFabricadoColumna.HeaderText = Lenguaje.traduce(strings.PorcentajeFabricado);
                    }
                    else
                    {
                        indexFabricado = rgvGridArticulos.Columns[Lenguaje.traduce("PendienteAcopio")].Index;
                        rgvGridArticulos.Columns.Remove(rgvGridArticulos.Columns[Lenguaje.traduce("PendienteAcopio")]);
                        rgvGridArticulos.SaveLayout(path);
                        rgvGridArticulos.Columns.Add(pctPteAcopioColumna);
                        rgvGridArticulos.Columns.Move(pctPteAcopioColumna.Index, indexFabricado);
                        pctPteAcopioColumna.HeaderText = Lenguaje.traduce("PendienteAcopio");
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + strings.CambiarPath);
            }
        }

        public void SaveLayoutLocal()
        {
            int indexPctFabricadoOrden;
            int pathGL = 1;
            string path = Persistencia.DirectorioLocal;/* XmlReaderPropio.getLayoutPath(1);*/
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
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    Directory.CreateDirectory(path);
                    path += "\\" + /*this.Name*/name + "GridView.xml";

                    path.Replace(" ", "_");
                    if (name == "Produccion")
                    {
                        indexPctFabricadoOrden = rgvOrdenes.Columns[Lenguaje.traduce(strings.PorcentajeFabricado)].Index;
                        rgvOrdenes.Columns.Remove(rgvOrdenes.Columns[Lenguaje.traduce(strings.PorcentajeFabricado)]);
                        XmlReaderPropio.setPctFabricacionOrdenesProduccion(indexPctFabricadoOrden, pathGL);
                        rgvOrdenes.SaveLayout(path);
                        rgvOrdenes.Columns.Add(pctFabricadoColumna);
                        rgvOrdenes.Columns.Move(pctFabricadoColumna.Index, indexPctFabricadoOrden);
                        pctFabricadoColumna.HeaderText = Lenguaje.traduce(strings.PorcentajeFabricado);
                    }
                    else
                    {
                        indexFabricado = rgvGridArticulos.Columns[Lenguaje.traduce("PendienteAcopio")].Index;
                        XmlReaderPropio.setPctArticulosPteAcopio(indexFabricado, pathGL);
                        rgvGridArticulos.Columns.Remove(rgvGridArticulos.Columns[Lenguaje.traduce("PendienteAcopio")]);
                        rgvGridArticulos.SaveLayout(path);
                        rgvGridArticulos.Columns.Add(pctPteAcopioColumna);
                        rgvGridArticulos.Columns.Move(pctPteAcopioColumna.Index, indexFabricado);
                        pctPteAcopioColumna.HeaderText = Lenguaje.traduce("PendienteAcopio");
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + strings.CambiarPath);
            }
        }

        public void LoadLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal; /*XmlReaderPropio.getLayoutPath(0);*/
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
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    string s = path + "\\" + /*this.Name*/ name + "GridView.xml";
                    //string path = @"C:\MisRecursos\PRUEBASGA\Local\" + this.name + "GridView.xml";
                    int pathGL = 0;
                    s.Replace(" ", "_");
                    string rutaFile = @"" + s;
                    if (File.Exists(rutaFile))
                    {
                        if (name == "Produccion")
                        {
                            this.rgvOrdenes.Columns.Remove(pctFabricadoColumna);
                            rgvOrdenes.LoadLayout(s);
                            this.rgvOrdenes.Columns.Add(pctFabricadoColumna);
                            rgvOrdenes.Columns.Move(pctFabricadoColumna.Index, XmlReaderPropio.getPctFabricacionOrdenesProduccion(pathGL));
                            rgvOrdenes.Columns[Lenguaje.traduce("ID Presentacion")].IsVisible = false;
                            rgvOrdenes.Columns[Lenguaje.traduce("ID Articulo")].IsVisible = false;
                            rgvOrdenes.TableElement.RowHeight = 40;
                            //    rgvOrdenes.LoadLayout(s);

                            //rgvOrdenes.TableElement.RowHeight = 40;
                        }
                        else
                        {
                            this.rgvGridArticulos.Columns.Remove(pctPteAcopioColumna);
                            rgvGridArticulos.LoadLayout(s);
                            this.rgvGridArticulos.Columns.Add(pctPteAcopioColumna);
                            rgvGridArticulos.Columns.Move(pctPteAcopioColumna.Index, XmlReaderPropio.getPctArticulosPteAcopio(pathGL));
                            rgvGridArticulos.Columns[Lenguaje.traduce("ID Presentacion")].IsVisible = false;
                            rgvGridArticulos.TableElement.RowHeight = 40;
                            //rgvGridArticulos.LoadLayout(s);

                            //rgvGridArticulos.TableElement.RowHeight = 40;
                        }
                    }
                    else
                    {
                        // MessageBox.Show(Lenguaje.traduce("No se ha podido encontrar el archivo"), "", MessageBoxButtons.OK,MessageBoxIcon.Error);
                        log.Debug("No se ha podido encontrar el archivo" + rutaFile);
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);

                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + strings.CambiarPath);
            }
        }

        public void LoadLayoutLocal()
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
                if (this.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    string s = path + "\\" + /*this.Name*/name + "GridView.xml";

                    s.Replace(" ", "_");
                    string rutaFile = @"" + s;
                    if (File.Exists(rutaFile))
                    {
                        if (name == "Produccion")
                        {
                            this.rgvOrdenes.Columns.Remove(pctFabricadoColumna.Name);
                            rgvOrdenes.LoadLayout(s);
                            this.rgvOrdenes.Columns.Add(pctFabricadoColumna);
                            rgvOrdenes.Columns.Move(pctFabricadoColumna.Index, XmlReaderPropio.getPctFabricacionOrdenesProduccion(pathGL));
                            rgvOrdenes.Columns[Lenguaje.traduce("ID Presentacion")].IsVisible = false;
                            rgvOrdenes.Columns[Lenguaje.traduce("ID Articulo")].IsVisible = false;
                            rgvOrdenes.TableElement.RowHeight = 40;
                        }
                        else
                        {
                            this.rgvGridArticulos.Columns.Remove(pctPteAcopioColumna.Name);
                            rgvGridArticulos.LoadLayout(s);
                            this.rgvGridArticulos.Columns.Add(pctPteAcopioColumna);
                            rgvGridArticulos.Columns.Move(pctPteAcopioColumna.Index, XmlReaderPropio.getPctArticulosPteAcopio(pathGL));
                            rgvGridArticulos.Columns[Lenguaje.traduce("ID Presentacion")].IsVisible = false;
                            rgvGridArticulos.TableElement.RowHeight = 40;
                        }
                    }
                    else
                    {
                        // MessageBox.Show(Lenguaje.traduce("No se ha podido encontrar el archivo"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Debug("No se ha podido encontrar el archivo" + rutaFile);
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + strings.CambiarPath);
            }
        }

        public void ElegirEstilo()
        {
            string pathLocal = Persistencia.DirectorioLocal;
            string pathGlobal = Persistencia.DirectorioGlobal;
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
            if (tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                string pathGridView = pathLocal + "\\" + /*this.Name*/name + "GridView.xml";
                bool existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    LoadLayoutLocal();
                }
                else
                {
                    LoadLayoutGlobal();
                }
            }
        }

        #endregion Estilos

        #region Color Estado Fabricacion

        private void ColorEstado()
        {
            List<ConditionalFormattingObject> listaCondicionales = new List<ConditionalFormattingObject>();
            if (Business.getEstadoFabricacionCondicionalesColor(ref listaCondicionales) > 0)
            {
                foreach (ConditionalFormattingObject item in listaCondicionales)
                {
                    this.rgvOrdenes.Columns[Lenguaje.traduce("Estado Fabricacion")].ConditionalFormattingObjectList.Add(item);
                }
            }
        }

        #endregion Color Estado Fabricacion

        #region Color Estado Maquina

        private void ColorEstadoMaquina()
        {
            try
            {
                ConditionalFormattingObject obj1 = new ConditionalFormattingObject("Mi Condición1", ConditionTypes.Equal, "AC", "", false);
                obj1.CellBackColor = XmlReaderPropio.getColorMaquina("AC");
                ConditionalFormattingObject obj2 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "IN", "", false);
                obj2.CellBackColor = XmlReaderPropio.getColorMaquina("IN");
                this.rgvOrdenes.MasterTemplate.Columns[Lenguaje.traduce("Estado Línea")].ConditionalFormattingObjectList.Add(obj1);
                this.rgvOrdenes.MasterTemplate.Columns[Lenguaje.traduce("Estado Línea")].ConditionalFormattingObjectList.Add(obj2);

                ConditionalFormattingObject obj3 = new ConditionalFormattingObject("Mi Condición3", ConditionTypes.NotEqual, "", "", false);
                obj3.CellBackColor = Color.FromArgb(255, 204, 255);
                this.rgvOrdenes.MasterTemplate.Columns[Lenguaje.traduce("Cantidad Fabricado")].ConditionalFormattingObjectList.Add(obj3);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        #endregion Color Estado Maquina

        #region Llamada WebService Generar Acopio

        public void LLamarWS()
        {
            try
            {
                List<LineaOrdenGenerarAcopio> acopioList = new List<LineaOrdenGenerarAcopio>();
                string jsonWS = string.Empty;
                foreach (GridViewRowInfo rowInfo in rgvGridArticulos.Rows)
                {
                    if (Convert.ToBoolean(rowInfo.Cells[EncontrarCheckBox(rgvGridArticulos.Columns)].Value))
                    {
                        foreach (GridViewRowInfo info in rowInfo.ChildRows)
                        {
                            LineaOrdenGenerarAcopio acopio = new LineaOrdenGenerarAcopio();
                            acopio.idpedidofab = Convert.ToInt32(info.Cells[Lenguaje.traduce("Num Orden")].Value);
                            acopio.idpedidofablin = Convert.ToInt32(info.Cells[Lenguaje.traduce("Linea Orden")].Value);
                            decimal cantidad = Convert.ToDecimal(info.Cells["Solicitar"].Value);
                            int idPresentacion = Convert.ToInt32(info.Cells["ID Presentacion"].Value);
                            int idArticulo = Convert.ToInt32(info.Cells["ID Articulo"].Value);
                            object[] presAlm = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidad);
                            acopio.cantidad = /*Convert.ToInt32(info.Cells["Solicitar"].Value);*/Convert.ToInt32(presAlm[0].ToString());
                            acopio.usuario = User.IdUsuario;
                            if (Convert.ToBoolean(rowInfo.Cells[rgvGridArticulos.Columns[Lenguaje.traduce("PaletCompleto")].Index].Value))
                            {
                                acopio.paletcompleto = true;
                            }
                            else
                            {
                                acopio.paletcompleto = false;
                            }
                            acopio.Error = " ";

                            if (acopio.cantidad > 0)
                            {
                                acopioList.Add(acopio);
                            }
                        }
                    }
                }

                jsonWS = JsonConvert.SerializeObject(acopioList, Formatting.Indented);
                if (jsonWS == String.Empty || acopioList.Count == 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener un valor en la columna Solicitar."));
                }
                else
                {
                    string mensajeError = "";
                    var solicitar = WSProduccionMotorClient.generarAcopio(jsonWS);
                    List<LineaOrdenGenerarAcopio> errorText = JsonConvert.DeserializeObject<List<LineaOrdenGenerarAcopio>>(solicitar);
                    for (int i = 0; i < errorText.Count; i++)
                    {
                        if (errorText[i].Error == string.Empty)
                        { }
                        else
                        {
                            // MessageBox.Show(Lenguaje.traduce(errorText[i].Error));
                            mensajeError = mensajeError + Lenguaje.traduce(errorText[i].Error) + System.Environment.NewLine;
                        }
                    }
                    if (!String.IsNullOrEmpty(mensajeError))
                    {
                        throw new Exception(mensajeError);
                    }
                    else
                    {
                    }
                    MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    ExceptionManager.GestionarErrorWS(ex, WSOrdenProduccionMotorClient.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarError(ex, ex.Message);
                }
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            refrescar();
        }

        #endregion Llamada WebService Generar Acopio

        #region WebService Maquina

        private void AsignarMaquinaWS(int idMaquina, string descripcion)
        {
            try
            {
                string jsonWS = string.Empty;
                int idOrdenFab;
                foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        idOrdenFab = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num PedidoFab")].Value);
                        jsonWS += formarJSONAsignMaquina(idOrdenFab, idMaquina) + ",";
                    }
                }
                jsonWS = jsonWS.Substring(0, jsonWS.Length - 1);
                jsonWS = "[" + jsonWS + "]";
                var respuesta = WSOrdenProduccionMotorClient.asignarMaquinaLista(jsonWS);
                if (respuesta == "OK")
                {
                    foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value) == true)
                        {
                            row.Cells[Lenguaje.traduce("Línea Producción")].Value = descripcion;
                        }
                    }
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                ExceptionManager.GestionarErrorWS(ex, WSOrdenProduccionMotorClient.Endpoint);
            }
        }

        private string formarJSONAsignMaquina(int idOrdenFab, int idMaquina)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idOrdenFab = idOrdenFab;
            objDinamico.idMaquina = idMaquina;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        #endregion WebService Maquina

        #region WebService AltaProducto

        private void AltaProductoLlamadaWS()
        {
            try
            {
                int idOrden = 0;
                int contador = 0;
                string operario = string.Empty;
                string tipoPalet = string.Empty;
                string maquina = string.Empty;
                foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        idOrden = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num PedidoFab")].Value);
                        operario = row.Cells[Lenguaje.traduce("Nombre Operario")].Value.ToString();
                        tipoPalet = row.Cells[Lenguaje.traduce("Tipo Palet")].Value.ToString();
                        maquina = row.Cells[Lenguaje.traduce("Maquina")].Value.ToString();
                        contador = contador + 1;
                    }
                }
                refrescar();
                if (contador == 1)
                {
                    //var respuesta = motorClient.getDatosDefectoAlta(idOrden);
                    //var atributosEntrada = entrada.getAtributosEntrada();
                    //AltaProductoWS alta = new AltaProductoWS(respuesta,atributosEntrada,operario, tipoPalet, maquina,idOrden);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener una única fila marcada."));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, WSOrdenProduccionMotorClient.Endpoint);
            }
        }

        #endregion WebService AltaProducto

        #region WebService CerrarOrden

        private void CerrarOrdenWS()
        {
            try
            {
                int idOrdenFab;
                int idOperario;
                int idRecurso;
                int contador = 0;
                foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        contador = contador + 1;
                        idOrdenFab = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num PedidoFab")].Value);
                        idOperario = User.IdOperario;//Convert.ToInt32(row.Cells[Lenguaje.traduce("Operario")].Value);
                        idRecurso = 0;//Convert.ToInt32(row.Cells[Lenguaje.traduce("Num Recurso")].Value);
                        WSOrdenProduccionMotorClient.cerrarOrdenProduccion(idOrdenFab, DatosThread.getInstancia().getArrayDatos(), idOperario, idRecurso);
                        row.IsVisible = false;
                    }
                }
                if (contador == 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener una única fila seleccionada."));
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refrescar();
            }
            catch (Exception)
            {
                MessageBox.Show(Lenguaje.traduce("No se cumplen los requisitos para cerrar la orden."));
            }
        }

        private void IniciarOrdenWS()
        {
            try
            {
                int idOrdenFab;
                int idOperario;
                int idRecurso;
                int contador = 0;
                foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        contador = contador + 1;
                        idOrdenFab = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num PedidoFab")].Value);
                        idOperario = User.IdOperario;
                        idRecurso = 0;
                        WSOrdenProduccionMotorClient.iniciarOrdenProduccion(idOrdenFab, idOperario, idRecurso);
                        rgvOrdenes.Refresh();
                    }
                }
                if (contador == 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener una única fila seleccionada."));
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refrescar();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        #endregion WebService CerrarOrden

        #region ProgressBarColumn

        public class ProgressBarCellElement : GridDataCellElement
        {
            private RadProgressBarElement radProgressBarElement;

            public ProgressBarCellElement(GridViewColumn column, GridRowElement row) : base(column, row)
            {
                if (column.FieldName == Lenguaje.traduce("NoFabricado"))
                {
                    UpperProgressIndicatorElement upperIndicator = radProgressBarElement.Children[1] as UpperProgressIndicatorElement;
                    upperIndicator.DrawFill = true;
                    upperIndicator.BackColor = Color.Yellow;
                    upperIndicator.BackColor2 = Color.Yellow;
                    upperIndicator.BackColor3 = Color.Yellow;
                    upperIndicator.BackColor4 = Color.Yellow;
                }
                else
                {
                    radProgressBarElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                }
            }

            protected override void CreateChildElements()
            {
                try
                {
                    base.CreateChildElements();
                    radProgressBarElement = new RadProgressBarElement();
                    this.Children.Add(radProgressBarElement);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }

            protected override void SetContentCore(object value)
            {
                try
                {
                    if (this.Value != null && this.Value != DBNull.Value)
                    {
                        int valor = 0;
                        this.radProgressBarElement.Maximum = 100;
                        radProgressBarElement.Minimum = 0;
                        if (Convert.ToDecimal(this.Value) >= 100)
                        {
                            valor = 100;
                            radProgressBarElement.Value1 = valor;
                            this.radProgressBarElement.Text = valor + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                        else if (Convert.ToDecimal(this.Value) <= 0)
                        {
                            valor = 0;
                            radProgressBarElement.Value1 = valor;
                            this.radProgressBarElement.Text = valor + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                        else
                        {
                            this.radProgressBarElement.Value1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Value), 0)); /*Convert.ToInt16(this.Value);*/
                            this.radProgressBarElement.Text = this.radProgressBarElement.Value1 + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }

            protected override Type ThemeEffectiveType
            {
                get
                {
                    return typeof(GridDataCellElement);
                }
            }

            public override bool IsCompatible(GridViewColumn data, object context)
            {
                return data is ProgressBarColumn && context is GridDataRowElement;
            }
        }

        public class ProgressBarColumn : GridViewDataColumn
        {
            public ProgressBarColumn(string fieldName) : base(fieldName)
            {
            }

            public override Type GetCellType(GridViewRowInfo row)
            {
                if (row is GridViewDataRowInfo)
                {
                    return typeof(ProgressBarCellElement);
                }
                return base.GetCellType(row);
            }
        }

        #endregion ProgressBarColumn

        #region Temas

        private void InitializeThemesDropDown()
        {
            AddThemeItemToThemesDropDownList("Fluent", Resources.fluent);
            AddThemeItemToThemesDropDownList("FluentDark", Resources.fluent_dark);
            AddThemeItemToThemesDropDownList("Material", Resources.material);
            AddThemeItemToThemesDropDownList("MaterialPink", Resources.material_pink);
            AddThemeItemToThemesDropDownList("MaterialTeal", Resources.material_teal);
            AddThemeItemToThemesDropDownList("MaterialBlueGrey", Resources.material_blue_grey);
            AddThemeItemToThemesDropDownList("ControlDefault", Resources.control_default);
            AddThemeItemToThemesDropDownList("TelerikMetro", Resources.telerik_metro);
            AddThemeItemToThemesDropDownList("Windows8", Resources.windows8);
            loadedThemes.Add("ControlDefault", true);
            loadedThemes.Add("TelerikMetro", true);
        }

        private void AddThemeItemToThemesDropDownList(string themeName, Image image)
        {
            RadMenuItem mainItem = rddbtnConfigurar.Items[0] as RadMenuItem;
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(temasItem_Click);
            mainItem.Items.Add(temasItem);
        }

        private void temasItem_Click(object sender, EventArgs e)
        {
            var assemblyName = "Telerik.WinControls.Themes." + (sender as RadMenuItem).Text + ".dll";
            var themeName = (sender as RadMenuItem).Text;
            var strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), assemblyName);
            if (!System.IO.File.Exists(strTempAssmbPath)) // we are in the case of QSF as exe, so the Path is different
            {
                strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\Bin40");
                strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);
                if (!System.IO.File.Exists(strTempAssmbPath))
                {
                    strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\bin\\ReleaseTrial");
                    strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);
                }
            }
            if (!loadedThemes.ContainsKey(themeName))
            {
                Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
                Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + themeName + "Theme"));
                loadedThemes.Add(themeName, true);
            }
            ThemeResolutionService.ApplicationThemeName = themeName;
            if (ControlTraceMonitor.AnalyticsMonitor != null)
            {
                ControlTraceMonitor.AnalyticsMonitor.TrackAtomicFeature("ThemeChanged." + themeName);
            }
        }

        #endregion Temas

        #region Control Barra Superior

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

        #endregion Control Barra Superior

        private void rBtnDevolucion_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón devolver " + DateTime.Now);
                if (rddlFiltrarMaquina.SelectedValue == null || rddlFiltrarMaquina.SelectedValue.ToString().Equals("") || Convert.ToInt32(rddlFiltrarMaquina.SelectedValue) < 1)
                {
                    MessageBox.Show("Debe seleccionar una linea de produccion");
                    return;
                }
                int idMaquina = Convert.ToInt32(rddlFiltrarMaquina.SelectedValue);
                string nombreMaquina = rddlFiltrarMaquina.SelectedText;
                FrmDevolucionesOrdenFab frm = new FrmDevolucionesOrdenFab(idMaquina, nombreMaquina);

                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rBtnInicioOrden_Click(object sender, EventArgs e)
        {
            log.Info("Pulsado botón inicio orden web service " + DateTime.Now);
            IniciarOrdenWS();
        }

        private void rddFiltradoPor_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("");
        }

        private void rCheckSoloPendientes_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            rgvGridArticulos.FilterDescriptors.Clear();
            log.Info("Pulsado botón rCheckSoloPendientes_ToggleStateChanged " + DateTime.Now);
            if (rCheckSoloPendientes.IsChecked)
            {
                FilterDescriptor filter = new FilterDescriptor();
                filter.PropertyName = "SolicitarArticulos";
                filter.Operator = FilterOperator.IsNotEqualTo;
                filter.Value = "0";
                filter.IsFilterEditor = true;
                rgvGridArticulos.FilterDescriptors.Add(filter);
            }
        }

        private void rBtnEliminarAcopios_Click(object sender, EventArgs e)
        {
            try
            {
                List<LineaOrdenGenerarAcopio> acopioList = new List<LineaOrdenGenerarAcopio>();
                string jsonWS = string.Empty;
                foreach (GridViewRowInfo rowInfo in rgvGridArticulos.Rows)
                {
                    if (Convert.ToBoolean(rowInfo.Cells[EncontrarCheckBox(rgvGridArticulos.Columns)].Value) == true)
                    {
                        foreach (GridViewRowInfo info in rowInfo.ChildRows)
                        {
                            decimal cantidad = Convert.ToDecimal(info.Cells["Solicitado"].Value);
                            int idPresentacion = Convert.ToInt32(info.Cells["ID Presentacion"].Value);
                            int idArticulo = Convert.ToInt32(info.Cells["ID Articulo"].Value);
                            object[] presAlm = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidad);
                            int cantInt = Convert.ToInt32(presAlm[0]);
                            if (cantInt > 0)
                            {
                                LineaOrdenGenerarAcopio acopio = new LineaOrdenGenerarAcopio();
                                acopio.idpedidofab = Convert.ToInt32(info.Cells[Lenguaje.traduce("Num Orden")].Value);
                                acopio.idpedidofablin = Convert.ToInt32(info.Cells[Lenguaje.traduce("Linea Orden")].Value);
                                acopio.cantidad = 0;
                                acopio.usuario = User.IdUsuario;
                                acopio.paletcompleto = false;
                                acopio.Error = " ";
                                acopioList.Add(acopio);
                            }
                        }
                    }
                }
                jsonWS = JsonConvert.SerializeObject(acopioList, Formatting.Indented);
                if (jsonWS == String.Empty)
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener un valor en la columna Solicitar."));
                }
                else
                {
                    string mensajeError = "";
                    var solicitar = WSProduccionMotorClient.eliminarAcopio(jsonWS);
                    List<LineaOrdenGenerarAcopio> errorText = JsonConvert.DeserializeObject<List<LineaOrdenGenerarAcopio>>(solicitar);
                    for (int i = 0; i < errorText.Count; i++)
                    {
                        if (errorText[i].Error == string.Empty)
                        { }
                        else
                        {
                            mensajeError = mensajeError + Lenguaje.traduce(errorText[i].Error) + System.Environment.NewLine;
                        }
                    }
                    if (!String.IsNullOrEmpty(mensajeError))
                    {
                        throw new Exception(mensajeError);
                    }
                    else
                    {
                    }
                    MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refrescar();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rBtnConsumirMatricula_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón consumir matricula " + DateTime.Now);
                int idPedidoFab = -1;
                int contador = 0;
                foreach (GridViewRowInfo row in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value) == true)
                    {
                        idPedidoFab = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num PedidoFab")].Value);

                        contador = contador + 1;
                        break;
                    }
                }
                if (contador == 1 && idPedidoFab > 0)
                {
                    ConsumosOrdenFabMatricula consumosOrden = new ConsumosOrdenFabMatricula(idPedidoFab);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener una única fila marcada."));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rBtnAnularConsumo_Click(object sender, EventArgs e)
        {
            try
            {
                bool ordenFabSeleccionada = false;
                string jsonWS = "";
                foreach (GridViewRowInfo rowinfo in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value) == true)
                    {
                        ordenFabSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Consumidos))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    int idsalida = Convert.ToInt32(row.Cells[Lenguaje.traduce("Salida")].Value);
                                    jsonWS += formarJSONAnularConsumo(idsalida);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (jsonWS.Length > 0)
                {
                    jsonWS = "[" + jsonWS + "]";
                    var respuesta = WSOrdenProduccionMotorClient.anularConsumo(jsonWS);
                    if (respuesta == "OK")
                    {
                        Utilidades.refrescarJerarquico(ref this.rgvOrdenes, 1);
                        MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debe seleccionar una orden y una línea"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnAnularConsumoParcial_Click(object sender, EventArgs e)
        {
            try
            {
                bool ordenFabSeleccionada = false;
                int idsalida = -1;
                foreach (GridViewRowInfo rowinfo in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value) == true)
                    {
                        ordenFabSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Consumidos))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    idsalida = Convert.ToInt32(row.Cells[Lenguaje.traduce("Salida")].Value);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (idsalida > 0)
                {
                    AnularConsumoParcial frm = new AnularConsumoParcial(idsalida);
                    frm.ShowDialog();
                    Utilidades.refrescarJerarquico(ref this.rgvOrdenes, 1);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debe seleccionar una orden y una línea"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rBtnAnularEntrada_Click(object sender, EventArgs e)
        {
            try
            {
                bool ordenFabSeleccionada = false;
                string jsonWS = "";
                foreach (GridViewRowInfo rowinfo in rgvOrdenes.Rows)
                {
                    if (Convert.ToBoolean(rowinfo.Cells[EncontrarCheckBox(rgvOrdenes.Columns)].Value) == true)
                    {
                        ordenFabSeleccionada = true;
                        GridViewHierarchyRowInfo hierarchyRow = rowinfo as GridViewHierarchyRowInfo;
                        if (hierarchyRow.ActiveView.ViewTemplate.Caption == Lenguaje.traduce(strings.Producidos))
                        {
                            int noOfChildRows = hierarchyRow.ChildRows.Count;

                            //looping through the child rows
                            foreach (GridViewRowInfo row in hierarchyRow.ChildRows)
                            {
                                //check if its current child row
                                if (row.IsSelected)
                                {
                                    int identrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("Entrada")].Value);
                                    jsonWS += formarJSONAnularEntrada(identrada);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (jsonWS.Length > 0)
                {
                    jsonWS = "[" + jsonWS + "]";
                    var respuesta = WSOrdenProduccionMotorClient.anularEntrada(jsonWS);
                    if (respuesta == "OK")
                    {
                        Utilidades.refrescarJerarquico(ref this.rgvOrdenes, 2);
                        MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debe seleccionar una orden y una línea"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private string formarJSONAnularConsumo(int idSalida)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idsalida = idSalida;
            objDinamico.idoperario = User.IdOperario;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        private string formarJSONAnularEntrada(int identrada)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.identrada = identrada;
            objDinamico.idoperario = User.IdOperario;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }
    }
}
using CalsificacionEntrada.Clasificacion;
using CalsificacionEntrada.RecepcionMotor;
using CalsificacionEntrada.model;
using Newtonsoft.Json;
using RumboSGA;
using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Data.SqlClient;
using System.ServiceModel;
using RumboSGAManager.Model.DataContext;

namespace EstadoRecepciones
{
    public partial class clasificacionEntradasForm : Telerik.WinControls.UI.RadRibbonForm
    {
        protected System.Windows.Forms.Panel panel;
        public RumGridView gridViewLineas = new RumGridView();
        public RumGridView gridViewCabezera = new RumGridView();
        public Telerik.WinControls.UI.RadRibbonBar radRibbonBar;
        public RumRibbonBarGroup grupoBotonesAccion = new RumRibbonBarGroup();
        public RumRibbonBarGroup grupoBotonesEstilos = new RumRibbonBarGroup();
        public RumButtonElement btnCalculo = new RumButtonElement();
        public RumButtonElement btnConfirmar = new RumButtonElement();
        public RumMenuItem btnEditColumns = new RumMenuItem();
        public RumMenuItem btnguardarColumnas = new RumMenuItem();
        public RumMenuItem btnCargarColumnas = new RumMenuItem();
        protected RibbonTab rtAcciones = new RibbonTab();
        protected RibbonTab rtEstilos = new RibbonTab();
        private string jsonDatos = "";
        private GridViewTemplate jerarquia = new GridViewTemplate();
        private List<RespuestaCalculo> listaRespuestaCalculo = new List<RespuestaCalculo>();
        public RumDropDownButtonElement rbConfiguracion = new RumDropDownButtonElement();
        private string[] cabezeraJson = new string[] { "idrecepcion", "idpedidopro", "idpedidoprolin", "cantidad", "codArticulo", "cantPedido", "cantRecepcion", "entRecepcio", "idPedido", "codigoProveedor", "idArticulo" };
        private string[] cabezeraAMostrar = new string[] { "Recepcion", "Pedido", "Linea", "cantidad", "Codigo Articulo", "Cant Pedido", "Cant Recepcion", "Ent Recepcion", "ID Pedido", "Codigo Proveedor", "ID Articulo" };
        private DataTable tablaLineas = new DataTable();
        private List<LineasRecepcion> listaLineas = new List<LineasRecepcion>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public clasificacionEntradasForm()
        {
            InitializeComponent();
        }

        public clasificacionEntradasForm(string json)
        {
            try
            {
                InitializeComponent();
                InstanciarElementos();
                jsonDatos = json;
                ConversionJsonTabla(json);
                string path = Persistencia.DirectorioLocal;
                Utilidades.LoadLayout(path, gridViewLineas, this.ProductName);
            }
            catch (Exception e)
            {
                MessageBox.Show("No se ha podido cargar bien la ventana");
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }
        }

        private void InstanciarElementos()
        {
            try
            {
                log.Info("Generamos los elementos de la pantalla");

                this.radRibbonBar = new Telerik.WinControls.UI.RadRibbonBar();
                this.panel = new System.Windows.Forms.Panel();

                ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar)).BeginInit();
                //
                // radRibbonBar
                //
                this.radRibbonBar.ExitButton.Text = "Exit";
                this.radRibbonBar.LocalizationSettings.LayoutModeText = "Simplified Layout";
                this.radRibbonBar.Location = new System.Drawing.Point(0, 0);
                this.radRibbonBar.Name = "radRibbonBar";

                this.radRibbonBar.OptionsButton.Text = "Options";
                this.radRibbonBar.TabIndex = 3;
                ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar.GetChildAt(0))).Text = "";
                ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                this.Controls.Add(this.radRibbonBar);
                this.radRibbonBar.Dock = DockStyle.Top;

                //
                //

                this.panel.AutoSize = true;
                this.panel.TabIndex = 0;
                this.panel.Dock = DockStyle.Fill;

                inicializarGridView();

                this.gridViewCabezera.Dock = DockStyle.Top;

                //Botones

                this.btnCalculo.Image = global::CalsificacionEntrada.Properties.Resources._003_calculator;
                this.btnCalculo.ImageAlignment = ContentAlignment.MiddleCenter;
                this.btnCalculo.Text = "Calculo Destinos ";
                this.btnCalculo.TextAlignment = ContentAlignment.BottomCenter;
                this.btnCalculo.Click += btnCalculo_Click;

                this.btnConfirmar.Image = global::CalsificacionEntrada.Properties.Resources.Approve;
                this.btnConfirmar.ImageAlignment = ContentAlignment.MiddleCenter;
                this.btnConfirmar.TextAlignment = ContentAlignment.BottomCenter;
                this.btnConfirmar.Text = "Confirmar";
                this.btnConfirmar.Click += btnConfirmar_Click;
                this.btnConfirmar.Enabled = false;

                this.btnEditColumns.Text = "Editar Estilo";
                this.btnEditColumns.Click += ItemColumnas_Click;

                this.btnguardarColumnas.Text = "Guardar Estilo";
                this.btnguardarColumnas.Click += SaveItem_Click;

                this.btnCargarColumnas.Text = "Cargar Estilo";
                this.btnCargarColumnas.Click += LoadItem_Click;

                RumRibbonBarGroup grupoConfiguracion = new RumRibbonBarGroup();

                rbConfiguracion.Text = "Configuración";
                rbConfiguracion.TextAlignment = ContentAlignment.BottomCenter;
                rbConfiguracion.ImageAlignment = ContentAlignment.MiddleCenter;

                FuncionesGenerales.AddEliminarLayoutButton(ref rbConfiguracion);
                if (rbConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
                {
                    rbConfiguracion.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        if (panel.Controls.Contains(gridViewLineas))
                        {
                            FuncionesGenerales.EliminarLayout("CalsificacionEntrada2GridView", gridViewLineas);
                            gridViewLineas.Refresh();
                        }
                    });
                }

                rbConfiguracion.Items.Add(btnguardarColumnas);
                rbConfiguracion.Items.Add(btnEditColumns);
                rbConfiguracion.Items.Add(btnCargarColumnas);
                rbConfiguracion.Image = CalsificacionEntrada.Properties.Resources.Administration;
                grupoConfiguracion.Items.Add(rbConfiguracion);

                //Tag con los botones accion
                grupoBotonesAccion.Items.Add(btnCalculo);
                grupoBotonesAccion.Items.Add(btnConfirmar);

                rtAcciones.Text = "Accion";
                rtAcciones.Items.Add(grupoBotonesAccion);
                rtAcciones.Items.Add(grupoConfiguracion);

                radRibbonBar.CommandTabs.Add(rtAcciones);
                this.panel.Controls.Add(gridViewLineas);

                this.Controls.Add(panel);
                this.Controls.Add(radRibbonBar);
                this.panel.Size = new System.Drawing.Size(MaximumSize.Width, MaximumSize.Height);

                ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar)).EndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se ha generado correctamente la ventana Clasificacion Entradas: ");
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                inicializarGridView();
                DataTable tableLineas = new DataTable();
                tableLineas = (DataTable)jerarquia.DataSource;

                List<DatosTodasLineas> listatablaWebServices = new List<DatosTodasLineas>();

                foreach (GridViewRowInfo rowinfo in gridViewLineas.Rows)
                {
                    DatosTodasLineas cabezera = new DatosTodasLineas();
                    cabezera.idrecepcion = rowinfo.Cells["ID"].Value.ToString();
                    cabezera.idpedidopro = rowinfo.Cells["ID Pedido"].Value.ToString();
                    cabezera.idpedidoprolin = Convert.ToInt32(rowinfo.Cells["Linea"].Value);
                    cabezera.cantidad = Convert.ToInt32(rowinfo.Cells["Cant Pedido"].Value);
                    cabezera.lineas = new List<Lineas>();
                    listatablaWebServices.Add(cabezera);
                }
                foreach (GridViewRowInfo rowinfo in jerarquia.Rows)
                {
                    // DatosTodasLineas cabezera = new DatosTodasLineas();
                    Lineas lineas = new Lineas();
                    lineas.subsistema = rowinfo.Cells["subsistema"].Value.ToString();
                    lineas.tiposoporte = rowinfo.Cells["tiposoporte"].Value.ToString();
                    lineas.id = Convert.ToInt32(rowinfo.Cells["Id"].Value);
                    lineas.cantidad = Convert.ToInt32(rowinfo.Cells["cantidad"].Value);

                    foreach (DatosTodasLineas cabezera in listatablaWebServices)
                    {
                        if (Convert.ToInt32(rowinfo.Cells["Linea"].Value) == cabezera.idpedidoprolin)
                        {
                            cabezera.lineas.Add(lineas);
                        }
                    }
                }
                string json = JsonConvert.SerializeObject(listatablaWebServices);
                log.Info("Extraemos los datos para enviarlos al webservice  " + json);

                WSClasificacionClient wsr = new WSClasificacionClient();
                string[] identificador = null;
                string resultado = wsr.confirmarPreclasificacionEntradas(json, identificador);

                DataTable table = new DataTable();

                table = ConversionDatosServicioATabla(json);

                gridViewLineas.DataSource = table;
                gridViewLineas.BestFitColumns();
                gridViewLineas.BeginEdit();
                ExpandAllRows(this.gridViewLineas.MasterTemplate, true);
                gridViewLineas.EndEdit();
                ColumnasModificables(gridViewLineas);
                gridViewLineas.Refresh();

                this.btnConfirmar.Enabled = true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("No se cargaron los datos de las lineas correctamente");
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show("La tabla no se ha generado correctamente");
                log.Error("Mensaje:" + aex.Message + "\n StackTrace:" + aex.StackTrace);
            }
            catch (FaultException fes)
            {
                MessageBox.Show("No se an calculado ningun dato ");
                log.Error("Mensaje:" + fes.Message + "\n StackTrace:" + fes.StackTrace);
            }
            catch (Exception es)
            {
                MessageBox.Show("No se ha podido acceder al servidor ");
                log.Error("Mensaje:" + es.Message + "\n StackTrace:" + es.StackTrace);
            }

            //FaultException
        }

        private void inicializarGridView()
        {
            this.gridViewLineas.Dock = DockStyle.Fill;
            this.gridViewLineas.AutoScroll = true;
            this.gridViewLineas.ReadOnly = true;
            this.gridViewLineas.EnableFiltering = true;
            this.gridViewLineas.MasterTemplate.EnableFiltering = true;
            this.gridViewLineas.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.CellSelect;
            this.gridViewLineas.BestFitColumns();
            this.gridViewLineas.AllowColumnReorder = true;
            this.gridViewLineas.AllowDragToGroup = true;
            this.gridViewLineas.AllowAutoSizeColumns = true;
            this.gridViewLineas.CellBeginEdit += RadGridView1_CellBeginEdit;
            //   this.gridViewLineas.CellEndEdit += RadGridView1_CellEndEdit;
            this.gridViewLineas.Enabled = true;
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            try
            {
                List<int> listEntrada = new List<int>();
                if (e.RowIndex == -1)
                {
                    return;
                }
                string nuevoValor = e.Value.ToString();
                String columna = e.Column.Name;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void btnCalculo_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón Calcular " + DateTime.Now);

                inicializarGridView();

                string datosWebservices = llamadaWebService(jsonDatos);

                this.gridViewLineas.MasterTemplate.Templates.Clear();
                this.gridViewLineas.MasterTemplate.Templates.Add(jerarquia);
                jerarquia.BestFitColumns();
                jerarquia.EnableSorting = true;

                log.Info("Recargamos la tabla con la relacion en el DataGrid  ");
                string path = Persistencia.DirectorioLocal;

                DataTable table = new DataTable();
                table = ConversionDatosServicioATabla(datosWebservices);

                gridViewLineas.DataSource = table;
                gridViewLineas.BestFitColumns();
                gridViewLineas.BeginEdit();
                ExpandAllRows(this.gridViewLineas.MasterTemplate, true);
                gridViewLineas.EndEdit();
                ColumnasModificables(gridViewLineas);
                gridViewLineas.Refresh();
                this.btnConfirmar.Enabled = true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("No se cargaron los datos de las lineas correctamente");
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show("La tabla no se ha generado correctamente");
                log.Error("Mensaje:" + aex.Message + "\n StackTrace:" + aex.StackTrace);
            }
        }

        private void GridViewLineas_ChildViewExpanded(object sender, ChildViewExpandedEventArgs e)
        {
        }

        private void ExpandAllRows(GridViewTemplate template, bool expanded)
        {
            foreach (GridViewRowInfo row in template.Rows)
            {
                row.IsExpanded = expanded;
            }
            if (template.Templates.Count > 0)
            {
                foreach (GridViewTemplate childTemplate in template.Templates)
                {
                    ExpandAllRows(childTemplate, true);
                }
            }
        }

        private void GenerarListaRespuestaCalculo(List<DatosTodasLineas> listatablaWebServices)
        {
            try
            {
                log.Info("Generamos la lista con todos los datos recibidos del WebService");
                listaRespuestaCalculo = new List<RespuestaCalculo>();

                foreach (var item in listatablaWebServices)
                {
                    foreach (var itemLines in item.lineas)
                    {
                        RespuestaCalculo respuestaCalculo = new RespuestaCalculo();
                        respuestaCalculo.idPedidoProLin = item.idpedidoprolin;
                        respuestaCalculo.cantidad = itemLines.cantidad;
                        respuestaCalculo.id = itemLines.id;
                        respuestaCalculo.subsistema = itemLines.subsistema;
                        respuestaCalculo.tiposoporte = itemLines.tiposoporte;
                        listaRespuestaCalculo.Add(respuestaCalculo);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("No se an podido cargar los datos del json");
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }
        }

        private void RelacionGridReservas(List<RespuestaCalculo> tablaJerarquia)
        {
            try
            {
                log.Info("Generamos la relacion de los datos del webServices y la tabla ");

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(RespuestaCalculo));

                DataTable table = new DataTable();

                foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (RespuestaCalculo item in listaRespuestaCalculo)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }

                foreach (DataColumn item in table.Columns)
                {
                    if (item.ColumnName == "idPedidoProLin" && item.ColumnName == "id")
                    {
                        item.ColumnMapping = MappingType.Hidden;
                    }

                    if (item.ColumnName == "idPedidoProLin")
                    {
                        item.ColumnName = "Linea";
                    }
                }

                jerarquia.DataSource = table;
                jerarquia.BestFitColumns();
                this.jerarquia.Columns["id"].IsVisible = false;
                this.jerarquia.Columns["id"].VisibleInColumnChooser = false;
                GridViewRelation relacion = new GridViewRelation(gridViewLineas.MasterTemplate);
                relacion.ChildTemplate = jerarquia;
                relacion.ParentColumnNames.Add("Linea");
                relacion.ChildColumnNames.Add("Linea");
                gridViewLineas.Relations.Clear();
                gridViewLineas.Relations.Add(relacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se cargaron los datos del calculo correctamente");
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void LoadItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Pulsado botón cargar estilo " + DateTime.Now);
            log.Info("Pulsado botón cargar estilo " + DateTime.Now);

            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                string path = Persistencia.DirectorioGlobal;

                Utilidades.LoadLayout(path, gridViewLineas, this.ProductName);
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                string path = Persistencia.DirectorioLocal;

                Utilidades.LoadLayout(path, gridViewLineas, this.ProductName);
            }
        }

        private void SaveItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Pulsado botón guardar estilo " + DateTime.Now);
            log.Info("Pulsado botón guardar estilo " + DateTime.Now);

            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                string path = Persistencia.DirectorioGlobal;

                Utilidades.SaveLayout(path, gridViewLineas, this.ProductName);
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                string path = Persistencia.DirectorioLocal;

                Utilidades.SaveLayout(path, gridViewLineas, this.ProductName);
            }
        }

        public void ConversionJsonTabla(string json)
        {
            try
            {
                log.Info("Convertimos el json en tabla y la añadimos al grid ");
                tablaLineas = JsonConvert.DeserializeObject<DataTable>(json);

                foreach (DataColumn item in tablaLineas.Columns)
                {
                    if (item.ColumnName == "ID Pedido Pro Lin")
                    {
                        item.ColumnName = "Linea";
                    }
                }
                gridViewLineas.DataSource = tablaLineas;
                extraerDatosParaWebService(json);

                DataTable table = new DataTable();
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(LineasRecepcion));

                foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (LineasRecepcion item in listaLineas)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                for (int i = 0; i < cabezeraJson.Length; i++)
                {
                    for (int y = 0; y < table.Columns.Count; y++)
                    {
                        if (cabezeraJson[i] == table.Columns[y].ColumnName)
                        {
                            table.Columns[y].ColumnName = cabezeraAMostrar[i];
                        }
                    }
                }

                getClasificacionEntradas(Convert.ToInt32(tablaLineas.Rows[0]["ID"]));

                this.gridViewLineas.MasterTemplate.Templates.Clear();
                this.gridViewLineas.MasterTemplate.Templates.Add(jerarquia);
                jerarquia.BestFitColumns();
                jerarquia.EnableSorting = true;

                gridViewLineas.DataSource = table;
                gridViewLineas.BestFitColumns();
                gridViewLineas.BeginEdit();
                ExpandAllRows(this.gridViewLineas.MasterTemplate, true);
                gridViewLineas.EndEdit();
                ColumnasModificables(gridViewLineas);
                gridViewLineas.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show("No se ha podido cargar Los datos en las tablas");
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }
        }

        public string llamadaWebService(string json)
        {
            //llamada webservices
            string jsonPruebas = "";
            string urlError = "";
            try
            {
                log.Info("Hacemos la llamada al WebService ");
                string jsonResultadosWebServices = extraerDatosParaWebService(json);
                WSClasificacionClient wsr = new WSClasificacionClient();
                string[] identificador = null;
                jsonPruebas = wsr.generarPreclasificacionEntradas(jsonResultadosWebServices, identificador);
                urlError = wsr.ChannelFactory.Endpoint.ListenUri.AbsoluteUri;
                log.Info("La generacion de preclasificacion a devuelto : " + jsonPruebas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se ha podido acceder al servidor ");
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            return jsonPruebas;
        }

        public string extraerDatosParaWebService(string json)
        {
            try
            {
                log.Info("Extraemos los datos para enviarlos al webservice  " + json);
                listaLineas = new List<LineasRecepcion>();

                foreach (GridViewRowInfo rowinfo in gridViewLineas.Rows)
                {
                    LineasRecepcion lineas = new LineasRecepcion();
                    lineas.id = rowinfo.Cells["ID"].Value.ToString();
                    lineas.idPedido = Convert.ToInt32(rowinfo.Cells["ID Pedido"].Value);
                    lineas.idpedidoprolin = Convert.ToInt32(rowinfo.Cells["Linea"].Value);
                    lineas.codigoProveedor = rowinfo.Cells["Codigo Proveedor"].Value.ToString();
                    lineas.atrib1 = rowinfo.Cells["Atrib1"].Value.ToString();
                    lineas.pedido = rowinfo.Cells["Pedido"].Value.ToString();
                    lineas.serie = rowinfo.Cells["Serie"].Value.ToString();
                    lineas.idArticulo = Convert.ToInt32(rowinfo.Cells["ID Articulo"].Value);
                    lineas.codArticulo = rowinfo.Cells["Codigo Articulo"].Value.ToString();
                    lineas.articulo = rowinfo.Cells["Articulo"].Value.ToString();
                    lineas.cantPedido = Convert.ToInt32(rowinfo.Cells["Cant Pedido"].Value);
                    lineas.cantRecepcion = Convert.ToInt32(rowinfo.Cells["Cant Recepcion"].Value);
                    lineas.entRecepcio = Convert.ToInt32(rowinfo.Cells["Ent Recepcion"].Value);
                    lineas.recepcion = rowinfo.Cells["recepcion"].Value.ToString();
                    listaLineas.Add(lineas);
                }
                List<DatosLineas> listaDatosLinea = new List<DatosLineas>();
                foreach (LineasRecepcion item in listaLineas)
                {
                    DatosLineas datos = new DatosLineas();
                    datos.idpedidopro = item.idPedido.ToString();
                    datos.idrecepcion = item.id;
                    datos.idpedidoprolin = item.idpedidoprolin;
                    datos.cantidad = item.cantRecepcion;

                    listaDatosLinea.Add(datos);
                }

                string jsonStringResult = JsonConvert.SerializeObject(listaDatosLinea);
                Console.WriteLine(jsonStringResult);
                return jsonStringResult;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("No se cargaron los datos de las lineas correctamente");
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                return null;
            }
        }

        public void ItemColumnas_Click(object sender, EventArgs e)
        {
            gridViewLineas.ShowColumnChooser();
        }

        private void ColumnasModificables(RadGridView grid)
        {
            this.gridViewLineas.CellBeginEdit += RadGridView1_CellBeginEdit;
            grid.MasterTemplate.AutoUpdateObjectRelationalSource = true;
            grid.ReadOnly = false;
            grid.AllowEditRow = true;
            foreach (GridViewDataColumn column in grid.Columns)
            {
                if (column.Name == "Cantidad")
                {
                    column.ReadOnly = false;
                    column.EnableExpressionEditor = true;
                }
            }
            //   this.gridViewLineas.CellEndEdit += RadGridView1_CellEndEdit;
        }

        public static string DataTableToJson(DataTable table)
        {
            string json = string.Empty;
            json = JsonConvert.SerializeObject(table);
            return json;
        }

        private DataTable ConversionDatosServicioATabla(string json)
        {
            List<DatosTodasLineas> listatablaWebServices = new List<DatosTodasLineas>();

            listatablaWebServices = JsonConvert.DeserializeObject<List<DatosTodasLineas>>(json);
            GenerarListaRespuestaCalculo(listatablaWebServices);

            return generarTabla(listatablaWebServices);
        }

        private DataTable generarTabla(List<DatosTodasLineas> listaDatosLineas)
        {
            Console.WriteLine(listaDatosLineas);
            RelacionGridReservas(listaRespuestaCalculo);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(LineasRecepcion));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (LineasRecepcion item in listaLineas)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties) row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            for (int i = 0; i < cabezeraJson.Length; i++)
            {
                for (int y = 0; y < table.Columns.Count; y++)
                {
                    if (cabezeraJson[i] == table.Columns[y].ColumnName)
                    {
                        table.Columns[y].ColumnName = cabezeraAMostrar[i];
                    }
                }
            }
            this.gridViewLineas.Columns["id"].IsVisible = false;
            this.gridViewLineas.Columns["id"].VisibleInColumnChooser = false;
            return table;
        }

        private void getClasificacionEntradas(int recepcion)
        {
            string fullQuery = "SELECT  * FROM TBLCLASIFICACIONENTRADAS WHERE IDRECEPCION = " + recepcion;
            DataTable tabla = ConexionSQL.getDataTable(fullQuery);
            string json = DataTableToJson(tabla);
            log.Info("Devolvemos los datos de la clasificacion entradas : " + json);
            List<DatosTodasLineas> listaDatosLineas = new List<DatosTodasLineas>();

            log.Info("Generamos la lista con todos los datos recibidos del WebService");
            listaRespuestaCalculo = new List<RespuestaCalculo>();
            if (tabla.Rows.Count > 0)
            {
                btnConfirmar.Enabled = true;
            }
            foreach (DataRow rowinfo in tabla.Rows)
            {
                RespuestaCalculo respuestaCalculo = new RespuestaCalculo();
                respuestaCalculo.idPedidoProLin = Convert.ToInt32(rowinfo["idpedidoprolin"]);
                respuestaCalculo.cantidad = Convert.ToInt32(rowinfo["cantidad"]);
                respuestaCalculo.id = Convert.ToInt32(rowinfo["ID"]);
                respuestaCalculo.subsistema = rowinfo["subsistema"].ToString();
                respuestaCalculo.tiposoporte = rowinfo["tiposoporte"].ToString();
                listaRespuestaCalculo.Add(respuestaCalculo);
            }

            RelacionGridReservas(listaRespuestaCalculo);
        }

        private void RadGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.Column.Name != "cantidad")
            {
                e.Cancel = true;
            }
        }
    }
}
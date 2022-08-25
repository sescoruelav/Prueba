using System;
using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using Telerik.WinControls.Data;

namespace RumboSGA.Maestros
{
    partial class AcopiosProduccion
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Código generado por el Diseñador de componentes
        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcopiosProduccion));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgvOrdenes = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonBar = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTabAcciones = new Telerik.WinControls.UI.RibbonTab();
            this.articulosBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rbtnVistaArticulos = new RumboSGA.RumButtonElement();
            this.rddFiltradoPor = new RumboSGA.Controles.RumDropDownButtonElement();
            this.rMenuOrdenesEnCurso = new RumboSGA.Controles.RumMenuItem();
            this.rMenuOrdenesPendientes = new RumboSGA.Controles.RumMenuItem();
            this.rMenuSinfiltro = new RumboSGA.Controles.RumMenuItem();
            this.rddBtnOpcionesArticulos = new RumboSGA.Controles.RumDropDownButtonElement();
            this.rBtnEliminarAcopios = new RumboSGA.Controles.RumMenuItem();
            this.rCheckSoloPendientes = new Telerik.WinControls.UI.RadCheckBoxElement();
            this.maqBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.btnAsignarMaquina = new RumboSGA.RumButtonElement();
            this.rddlFiltrarMaquina = new Telerik.WinControls.UI.RadDropDownListElement();
            this.pedidosBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnInicioOrden = new RumboSGA.RumButtonElement();
            this.rbtnConsumir = new RumboSGA.RumButtonElement();
            this.rBtnConsumirMatricula = new RumboSGA.RumButtonElement();
            this.rBtnDevolucion = new RumboSGA.RumButtonElement();
            this.rbtnCerrarOrden = new RumboSGA.RumButtonElement();
            this.rddBtnOpcionesFabricacion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.rBtnAltaPalet = new RumboSGA.Controles.RumMenuItem();
            this.rBtnAltaPico = new RumboSGA.Controles.RumMenuItem();
            this.rBtnAltaMulti = new RumboSGA.Controles.RumMenuItem();
            this.rBtnAltaCarro = new RumboSGA.Controles.RumMenuItem();
            this.rBtnAltaTotales = new RumboSGA.Controles.RumMenuItem();
            this.rBtnAnularConsumo = new RumboSGA.Controles.RumMenuItem();
            this.rBtnAnularConsumoParcial = new RumboSGA.Controles.RumMenuItem();
            this.rBtnAnularEntrada = new RumboSGA.Controles.RumMenuItem();
            this.accionesArtículosBarGgroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rbtnSolicitar = new RumboSGA.RumButtonElement();
            this.vistaGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rbtnExportar = new RumboSGA.RumButtonElement();
            this.rbtnBorrarFiltro = new RumboSGA.RumButtonElement();
            this.configBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rddbtnConfigurar = new RumboSGA.Controles.RumDropDownButtonElement();
            this.temasMenu = new RumboSGA.Controles.RumMenuItem();
            this.editColumnas = new RumboSGA.Controles.RumMenuItem();
            this.filtrosMenu = new RumboSGA.Controles.RumMenuItem();
            this.guardarMenu = new RumboSGA.Controles.RumMenuItem();
            this.cargarMenu = new RumboSGA.Controles.RumMenuItem();
            this.rbtnRefrescar = new RumboSGA.RumButtonElement();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvOrdenes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvOrdenes.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rddlFiltrarMaquina)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.rgvOrdenes, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 165);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1378, 570);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rgvOrdenes
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rgvOrdenes, 5);
            this.rgvOrdenes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvOrdenes.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvOrdenes.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvOrdenes.Name = "rgvOrdenes";
            this.rgvOrdenes.Size = new System.Drawing.Size(1372, 564);
            this.rgvOrdenes.TabIndex = 0;
            // 
            // radRibbonBar
            // 
            this.radRibbonBar.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTabAcciones});
            // 
            // 
            // 
            // 
            // 
            // 
            this.radRibbonBar.ExitButton.ButtonElement.ShowBorder = false;
            this.radRibbonBar.ExitButton.Text = "Exit";
            this.radRibbonBar.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.radRibbonBar.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar.Name = "radRibbonBar";
            // 
            // 
            // 
            // 
            // 
            // 
            this.radRibbonBar.OptionsButton.ButtonElement.ShowBorder = false;
            this.radRibbonBar.OptionsButton.Text = "Options";
            this.radRibbonBar.SimplifiedHeight = 100;
            this.radRibbonBar.Size = new System.Drawing.Size(1378, 165);
            this.radRibbonBar.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radRibbonBar.StartButtonImage")));
            this.radRibbonBar.TabIndex = 3;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTabAcciones
            // 
            this.ribbonTabAcciones.AutoEllipsis = false;
            this.ribbonTabAcciones.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.ribbonTabAcciones.IsSelected = true;
            this.ribbonTabAcciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.articulosBarGroup,
            this.maqBarGroup,
            this.pedidosBarGroup,
            this.accionesArtículosBarGgroup,
            this.vistaGroup,
            this.configBarGroup});
            this.ribbonTabAcciones.Name = "ribbonTabAcciones";
            this.ribbonTabAcciones.Text = "Acciones";
            this.ribbonTabAcciones.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.ribbonTabAcciones.UseCompatibleTextRendering = false;
            this.ribbonTabAcciones.UseMnemonic = false;
            // 
            // articulosBarGroup
            // 
            this.articulosBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rbtnVistaArticulos,
            this.rddFiltradoPor,
            this.rddBtnOpcionesArticulos,
            this.rCheckSoloPendientes});
            this.articulosBarGroup.Margin = new System.Windows.Forms.Padding(0);
            this.articulosBarGroup.MaxSize = new System.Drawing.Size(0, 0);
            this.articulosBarGroup.MinSize = new System.Drawing.Size(0, 0);
            this.articulosBarGroup.Name = "articulosBarGroup";
            this.articulosBarGroup.Text = "Vista";
            this.articulosBarGroup.UseCompatibleTextRendering = false;
            // 
            // rbtnVistaArticulos
            // 
            this.rbtnVistaArticulos.DisplayStyle = Telerik.WinControls.DisplayStyle.ImageAndText;
            this.rbtnVistaArticulos.EnableFocusBorderAnimation = true;
            this.rbtnVistaArticulos.EnableRippleAnimation = true;
            this.rbtnVistaArticulos.Image = global::RumboSGA.Properties.Resources.edit;
            this.rbtnVistaArticulos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnVistaArticulos.Name = "rbtnVistaArticulos";
            this.rbtnVistaArticulos.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtnVistaArticulos.Text = "Articulos";
            this.rbtnVistaArticulos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnVistaArticulos.ToolTipText = "Articulos";
            this.rbtnVistaArticulos.UseCompatibleTextRendering = false;
            this.rbtnVistaArticulos.Click += new System.EventHandler(this.btnVistaArticulos_Click);
            // 
            // rddFiltradoPor
            // 
            this.rddFiltradoPor.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rddFiltradoPor.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rddFiltradoPor.ExpandArrowButton = false;
            this.rddFiltradoPor.Image = global::RumboSGA.Properties.Resources.Debug;
            this.rddFiltradoPor.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rddFiltradoPor.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rMenuOrdenesEnCurso,
            this.rMenuOrdenesPendientes,
            this.rMenuSinfiltro});
            this.rddFiltradoPor.Name = "rddFiltradoPor";
            this.rddFiltradoPor.Text = "Filtrado por";
            this.rddFiltradoPor.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rddFiltradoPor.Click += new System.EventHandler(this.rddFiltradoPor_Click);
            // 
            // rMenuOrdenesEnCurso
            // 
            this.rMenuOrdenesEnCurso.Name = "rMenuOrdenesEnCurso";
            this.rMenuOrdenesEnCurso.Text = "Ordenes en curso";
            this.rMenuOrdenesEnCurso.Click += new System.EventHandler(this.MenuOrdenesEnCurso_Click);
            // 
            // rMenuOrdenesPendientes
            // 
            this.rMenuOrdenesPendientes.Name = "rMenuOrdenesPendientes";
            this.rMenuOrdenesPendientes.Text = "Ordenes pendientes";
            this.rMenuOrdenesPendientes.Click += new System.EventHandler(this.MenuOrdenesPendientes_Click);
            // 
            // rMenuSinfiltro
            // 
            this.rMenuSinfiltro.Name = "rMenuSinfiltro";
            this.rMenuSinfiltro.Text = "Sin filtro";
            this.rMenuSinfiltro.Click += new System.EventHandler(this.MenuSinFiltro_Click);
            // 
            // rddBtnOpcionesArticulos
            // 
            this.rddBtnOpcionesArticulos.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rddBtnOpcionesArticulos.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rddBtnOpcionesArticulos.ExpandArrowButton = false;
            this.rddBtnOpcionesArticulos.Image = global::RumboSGA.Properties.Resources.mostrarmas;
            this.rddBtnOpcionesArticulos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rddBtnOpcionesArticulos.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnEliminarAcopios});
            this.rddBtnOpcionesArticulos.Name = "rddBtnOpcionesArticulos";
            this.rddBtnOpcionesArticulos.Text = "";
            // 
            // rBtnEliminarAcopios
            // 
            this.rBtnEliminarAcopios.Image = global::RumboSGA.Properties.Resources.Delete;
            this.rBtnEliminarAcopios.ImageAlignment = System.Drawing.ContentAlignment.TopRight;
            this.rBtnEliminarAcopios.Name = "rBtnEliminarAcopios";
            this.rBtnEliminarAcopios.Text = "Eliminar Acopios";
            this.rBtnEliminarAcopios.Click += new System.EventHandler(this.rBtnEliminarAcopios_Click);
            // 
            // rCheckSoloPendientes
            // 
            this.rCheckSoloPendientes.Checked = false;
            this.rCheckSoloPendientes.Name = "rCheckSoloPendientes";
            this.rCheckSoloPendientes.ReadOnly = false;
            this.rCheckSoloPendientes.StretchVertically = false;
            this.rCheckSoloPendientes.Text = "Solo Pendientes";
            this.rCheckSoloPendientes.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.rCheckSoloPendientes_ToggleStateChanged);
            // 
            // maqBarGroup
            // 
            this.maqBarGroup.AutoSize = false;
            this.maqBarGroup.Bounds = new System.Drawing.Rectangle(0, 0, 357, 92);
            this.maqBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnAsignarMaquina,
            this.rddlFiltrarMaquina});
            this.maqBarGroup.Margin = new System.Windows.Forms.Padding(0);
            this.maqBarGroup.MaxSize = new System.Drawing.Size(0, 0);
            this.maqBarGroup.MinSize = new System.Drawing.Size(0, 0);
            this.maqBarGroup.Name = "maqBarGroup";
            this.maqBarGroup.Text = "Maquinas";
            this.maqBarGroup.UseCompatibleTextRendering = false;
            // 
            // btnAsignarMaquina
            // 
            this.btnAsignarMaquina.EnableFocusBorderAnimation = true;
            this.btnAsignarMaquina.EnableRippleAnimation = true;
            this.btnAsignarMaquina.Image = global::RumboSGA.Properties.Resources.Debug;
            this.btnAsignarMaquina.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAsignarMaquina.Name = "btnAsignarMaquina";
            this.btnAsignarMaquina.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAsignarMaquina.Text = "Asignar";
            this.btnAsignarMaquina.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAsignarMaquina.ToolTipText = "Asignar";
            this.btnAsignarMaquina.UseCompatibleTextRendering = false;
            this.btnAsignarMaquina.Click += new System.EventHandler(this.MaquinaButtonWS_Click);
            // 
            // rddlFiltrarMaquina
            // 
            this.rddlFiltrarMaquina.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rddlFiltrarMaquina.ArrowButtonMinWidth = 17;
            this.rddlFiltrarMaquina.AutoCompleteAppend = null;
            this.rddlFiltrarMaquina.AutoCompleteDataSource = null;
            this.rddlFiltrarMaquina.AutoCompleteSuggest = null;
            this.rddlFiltrarMaquina.AutoSize = false;
            this.rddlFiltrarMaquina.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.rddlFiltrarMaquina.Bounds = new System.Drawing.Rectangle(0, 0, 211, 20);
            this.rddlFiltrarMaquina.DataMember = "";
            this.rddlFiltrarMaquina.DataSource = null;
            this.rddlFiltrarMaquina.DefaultValue = null;
            this.rddlFiltrarMaquina.DisplayMember = "";
            this.rddlFiltrarMaquina.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            this.rddlFiltrarMaquina.DropDownAnimationEnabled = true;
            this.rddlFiltrarMaquina.EditableElementText = "";
            this.rddlFiltrarMaquina.EditorElement = this.rddlFiltrarMaquina;
            this.rddlFiltrarMaquina.EditorManager = null;
            this.rddlFiltrarMaquina.Filter = null;
            this.rddlFiltrarMaquina.FilterExpression = "";
            this.rddlFiltrarMaquina.Focusable = true;
            this.rddlFiltrarMaquina.FormatString = "";
            this.rddlFiltrarMaquina.FormattingEnabled = true;
            this.rddlFiltrarMaquina.MaxDropDownItems = 0;
            this.rddlFiltrarMaquina.MaxLength = 32767;
            this.rddlFiltrarMaquina.MaxValue = null;
            this.rddlFiltrarMaquina.MinSize = new System.Drawing.Size(140, 0);
            this.rddlFiltrarMaquina.MinValue = null;
            this.rddlFiltrarMaquina.Name = "rddlFiltrarMaquina";
            this.rddlFiltrarMaquina.NullValue = null;
            this.rddlFiltrarMaquina.OwnerOffset = 0;
            this.rddlFiltrarMaquina.ShowImageInEditorArea = true;
            this.rddlFiltrarMaquina.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            this.rddlFiltrarMaquina.StretchVertically = false;
            this.rddlFiltrarMaquina.UseCompatibleTextRendering = false;
            this.rddlFiltrarMaquina.Value = null;
            this.rddlFiltrarMaquina.ValueMember = "";
            // 
            // pedidosBarGroup
            // 
            this.pedidosBarGroup.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentBounds;
            this.pedidosBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnInicioOrden,
            this.rbtnConsumir,
            this.rBtnConsumirMatricula,
            this.rBtnDevolucion,
            this.rbtnCerrarOrden,
            this.rddBtnOpcionesFabricacion});
            this.pedidosBarGroup.Margin = new System.Windows.Forms.Padding(0);
            this.pedidosBarGroup.MaxSize = new System.Drawing.Size(0, 0);
            this.pedidosBarGroup.MinSize = new System.Drawing.Size(0, 0);
            this.pedidosBarGroup.Name = "pedidosBarGroup";
            this.pedidosBarGroup.Text = "Pedidos";
            this.pedidosBarGroup.UseCompatibleTextRendering = false;
            // 
            // rBtnInicioOrden
            // 
            this.rBtnInicioOrden.EnableFocusBorderAnimation = true;
            this.rBtnInicioOrden.EnableRippleAnimation = true;
            this.rBtnInicioOrden.Image = global::RumboSGA.Properties.Resources.CopyFromTask;
            this.rBtnInicioOrden.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnInicioOrden.Name = "rBtnInicioOrden";
            this.rBtnInicioOrden.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnInicioOrden.Text = "Inicio Orden";
            this.rBtnInicioOrden.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnInicioOrden.ToolTipText = "Inicio Orden";
            this.rBtnInicioOrden.Click += new System.EventHandler(this.rBtnInicioOrden_Click);
            // 
            // rbtnConsumir
            // 
            this.rbtnConsumir.EnableFocusBorderAnimation = true;
            this.rbtnConsumir.EnableRippleAnimation = true;
            this.rbtnConsumir.Image = global::RumboSGA.Properties.Resources.product_icon;
            this.rbtnConsumir.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnConsumir.Name = "rbtnConsumir";
            this.rbtnConsumir.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtnConsumir.Text = "Consumir";
            this.rbtnConsumir.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnConsumir.ToolTipText = "Consumir";
            this.rbtnConsumir.UseCompatibleTextRendering = false;
            this.rbtnConsumir.Click += new System.EventHandler(this.btnConsumir_Click);
            // 
            // rBtnConsumirMatricula
            // 
            this.rBtnConsumirMatricula.EnableFocusBorderAnimation = true;
            this.rBtnConsumirMatricula.EnableRippleAnimation = true;
            this.rBtnConsumirMatricula.Image = global::RumboSGA.Properties.Resources.NewResource;
            this.rBtnConsumirMatricula.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnConsumirMatricula.Name = "rBtnConsumirMatricula";
            this.rBtnConsumirMatricula.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnConsumirMatricula.Text = "Consumir Matrícula";
            this.rBtnConsumirMatricula.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnConsumirMatricula.ToolTipText = "Consumir Matrícula";
            this.rBtnConsumirMatricula.Click += new System.EventHandler(this.rBtnConsumirMatricula_Click);
            // 
            // rBtnDevolucion
            // 
            this.rBtnDevolucion.EnableFocusBorderAnimation = true;
            this.rBtnDevolucion.EnableRippleAnimation = true;
            this.rBtnDevolucion.Image = global::RumboSGA.Properties.Resources.back;
            this.rBtnDevolucion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnDevolucion.Name = "rBtnDevolucion";
            this.rBtnDevolucion.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnDevolucion.Text = "Devolución";
            this.rBtnDevolucion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnDevolucion.ToolTipText = "Devolución";
            this.rBtnDevolucion.Click += new System.EventHandler(this.rBtnDevolucion_Click);
            // 
            // rbtnCerrarOrden
            // 
            this.rbtnCerrarOrden.EnableFocusBorderAnimation = true;
            this.rbtnCerrarOrden.EnableRippleAnimation = true;
            this.rbtnCerrarOrden.Image = global::RumboSGA.Properties.Resources.Delete;
            this.rbtnCerrarOrden.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnCerrarOrden.Name = "rbtnCerrarOrden";
            this.rbtnCerrarOrden.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtnCerrarOrden.Text = "Cerrar";
            this.rbtnCerrarOrden.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnCerrarOrden.ToolTipText = "Cerrar";
            this.rbtnCerrarOrden.UseCompatibleTextRendering = false;
            this.rbtnCerrarOrden.Click += new System.EventHandler(this.rBtnCerrarOrden_Click);
            // 
            // rddBtnOpcionesFabricacion
            // 
            this.rddBtnOpcionesFabricacion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rddBtnOpcionesFabricacion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rddBtnOpcionesFabricacion.ExpandArrowButton = false;
            this.rddBtnOpcionesFabricacion.Image = global::RumboSGA.Properties.Resources.mostrarmas;
            this.rddBtnOpcionesFabricacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rddBtnOpcionesFabricacion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnAltaPalet,
            this.rBtnAltaPico,
            this.rBtnAltaMulti,
            this.rBtnAltaCarro,
            this.rBtnAltaTotales,
            this.rBtnAnularConsumo,
            this.rBtnAnularConsumoParcial,
            this.rBtnAnularEntrada});
            this.rddBtnOpcionesFabricacion.Name = "rddBtnOpcionesFabricacion";
            this.rddBtnOpcionesFabricacion.Text = "";
            // 
            // rBtnAltaPalet
            // 
            this.rBtnAltaPalet.Image = global::RumboSGA.Properties.Resources.pallet;
            this.rBtnAltaPalet.Name = "rBtnAltaPalet";
            this.rBtnAltaPalet.Text = "Alta Palet";
            this.rBtnAltaPalet.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rBtnAltaPico
            // 
            this.rBtnAltaPico.Image = global::RumboSGA.Properties.Resources.palletPicos;
            this.rBtnAltaPico.Name = "rBtnAltaPico";
            this.rBtnAltaPico.Text = "Alta palet incompleto";
            this.rBtnAltaPico.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rBtnAltaMulti
            // 
            this.rBtnAltaMulti.Image = global::RumboSGA.Properties.Resources.palletMulti;
            this.rBtnAltaMulti.Name = "rBtnAltaMulti";
            this.rBtnAltaMulti.Text = "Alta palet multireferencia";
            this.rBtnAltaMulti.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rBtnAltaCarro
            // 
            this.rBtnAltaCarro.Image = global::RumboSGA.Properties.Resources.CarroMovil;
            this.rBtnAltaCarro.Name = "rBtnAltaCarro";
            this.rBtnAltaCarro.Text = "Alta carro";
            this.rBtnAltaCarro.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rBtnAltaTotales
            // 
            this.rBtnAltaTotales.Image = global::RumboSGA.Properties.Resources.addbox;
            this.rBtnAltaTotales.Name = "rBtnAltaTotales";
            this.rBtnAltaTotales.Text = "Alta Entradas Totales";
            this.rBtnAltaTotales.Click += new System.EventHandler(this.EventoAbrirAltaProducto_Click);
            // 
            // rBtnAnularConsumo
            // 
            this.rBtnAnularConsumo.Image = global::RumboSGA.Properties.Resources.Delete;
            this.rBtnAnularConsumo.Name = "rBtnAnularConsumo";
            this.rBtnAnularConsumo.Text = "Anular Consumo";
            this.rBtnAnularConsumo.Click += new System.EventHandler(this.rBtnAnularConsumo_Click);
            // 
            // rBtnAnularConsumoParcial
            // 
            this.rBtnAnularConsumoParcial.Image = global::RumboSGA.Properties.Resources.Delete;
            this.rBtnAnularConsumoParcial.Name = "rBtnAnularConsumoParcial";
            this.rBtnAnularConsumoParcial.Text = "Anular Consumo Parcial";
            this.rBtnAnularConsumoParcial.Click += new System.EventHandler(this.rBtnAnularConsumoParcial_Click);
            // 
            // rBtnAnularEntrada
            // 
            this.rBtnAnularEntrada.Image = global::RumboSGA.Properties.Resources.Delete;
            this.rBtnAnularEntrada.Name = "rBtnAnularEntrada";
            this.rBtnAnularEntrada.Text = "Anular Entrada";
            this.rBtnAnularEntrada.Click += new System.EventHandler(this.rBtnAnularEntrada_Click);
            // 
            // accionesArtículosBarGgroup
            // 
            this.accionesArtículosBarGgroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rbtnSolicitar,
            this.rCheckSoloPendientes,
            this.rddBtnOpcionesArticulos});
            this.accionesArtículosBarGgroup.Name = "accionesArtículosBarGgroup";
            this.accionesArtículosBarGgroup.Text = "Artículos";
            // 
            // rbtnSolicitar
            // 
            this.rbtnSolicitar.EnableFocusBorderAnimation = true;
            this.rbtnSolicitar.EnableRippleAnimation = true;
            this.rbtnSolicitar.Image = global::RumboSGA.Properties.Resources.GoTo;
            this.rbtnSolicitar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnSolicitar.Name = "rbtnSolicitar";
            this.rbtnSolicitar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtnSolicitar.Text = "Solicitar";
            this.rbtnSolicitar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnSolicitar.ToolTipText = "Solicitar";
            this.rbtnSolicitar.UseCompatibleTextRendering = false;
            this.rbtnSolicitar.Click += new System.EventHandler(this.rbtnSolicitar_Click);
            // 
            // vistaGroup
            // 
            this.vistaGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rbtnExportar,
            this.rbtnBorrarFiltro});
            this.vistaGroup.Name = "vistaGroup";
            this.vistaGroup.Text = "Ver";
            // 
            // rbtnExportar
            // 
            this.rbtnExportar.EnableFocusBorderAnimation = true;
            this.rbtnExportar.EnableRippleAnimation = true;
            this.rbtnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rbtnExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnExportar.Name = "rbtnExportar";
            this.rbtnExportar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtnExportar.Text = "";
            this.rbtnExportar.Click += new System.EventHandler(this.btnExportacion_Click);
            // 
            // rbtnBorrarFiltro
            // 
            this.rbtnBorrarFiltro.EnableFocusBorderAnimation = true;
            this.rbtnBorrarFiltro.EnableRippleAnimation = true;
            this.rbtnBorrarFiltro.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.rbtnBorrarFiltro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnBorrarFiltro.Name = "rbtnBorrarFiltro";
            this.rbtnBorrarFiltro.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtnBorrarFiltro.Text = "";
            this.rbtnBorrarFiltro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnBorrarFiltro.Click += new System.EventHandler(this.btnBorrarFiltro_Click);
            // 
            // configBarGroup
            // 
            this.configBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rddbtnConfigurar,
            this.rbtnRefrescar});
            this.configBarGroup.Margin = new System.Windows.Forms.Padding(0);
            this.configBarGroup.MaxSize = new System.Drawing.Size(0, 0);
            this.configBarGroup.MinSize = new System.Drawing.Size(0, 0);
            this.configBarGroup.Name = "configBarGroup";
            this.configBarGroup.Text = "Configuracion";
            this.configBarGroup.UseCompatibleTextRendering = false;
            // 
            // rddbtnConfigurar
            // 
            this.rddbtnConfigurar.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rddbtnConfigurar.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rddbtnConfigurar.AutoSize = false;
            this.rddbtnConfigurar.Bounds = new System.Drawing.Rectangle(0, 0, 76, 69);
            this.rddbtnConfigurar.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rddbtnConfigurar.ExpandArrowButton = false;
            this.rddbtnConfigurar.Image = global::RumboSGA.Properties.Resources.Administration;
            this.rddbtnConfigurar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rddbtnConfigurar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenu,
            this.editColumnas,
            this.filtrosMenu,
            this.guardarMenu,
            this.cargarMenu});
            this.rddbtnConfigurar.Name = "rddbtnConfigurar";
            this.rddbtnConfigurar.Text = "";
            this.rddbtnConfigurar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rddbtnConfigurar.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.rddbtnConfigurar.UseCompatibleTextRendering = false;
            // 
            // temasMenu
            // 
            this.temasMenu.Name = "temasMenu";
            this.temasMenu.Text = "Temas";
            // 
            // editColumnas
            // 
            this.editColumnas.Name = "editColumnas";
            this.editColumnas.Text = "Editar Columnas";
            this.editColumnas.Click += new System.EventHandler(this.ItemColumnas_Click);
            // 
            // filtrosMenu
            // 
            this.filtrosMenu.Name = "filtrosMenu";
            this.filtrosMenu.Text = "Filtros";
            this.filtrosMenu.Click += new System.EventHandler(this.FiltroMenuItem_Click);
            // 
            // guardarMenu
            // 
            this.guardarMenu.Name = "guardarMenu";
            this.guardarMenu.Text = "Guardar Estilo";
            this.guardarMenu.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarMenu
            // 
            this.cargarMenu.Name = "cargarMenu";
            this.cargarMenu.Text = "Cargar Estilo";
            this.cargarMenu.Click += new System.EventHandler(this.LoadItem_Click);
            // 
            // rbtnRefrescar
            // 
            this.rbtnRefrescar.EnableFocusBorderAnimation = true;
            this.rbtnRefrescar.EnableRippleAnimation = true;
            this.rbtnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rbtnRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnRefrescar.Name = "rbtnRefrescar";
            this.rbtnRefrescar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rbtnRefrescar.Text = "Refrescar";
            this.rbtnRefrescar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnRefrescar.ToolTipText = "Refrescar";
            this.rbtnRefrescar.Click += new System.EventHandler(this.rbtnRefrescarButton_Click);
            // 
            // AcopiosProduccion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1378, 735);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar);
            this.MainMenuStrip = null;
            this.Name = "AcopiosProduccion";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvOrdenes.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvOrdenes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rddlFiltrarMaquina)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
        #endregion
        private Telerik.WinControls.UI.RibbonTab ribbonTabAcciones;
        private RumRibbonBarGroup articulosBarGroup;
        private RumButtonElement rbtnVistaArticulos;
        private RumButtonElement rbtnSolicitar;
        private RumRibbonBarGroup maqBarGroup;
        private RumButtonElement btnAsignarMaquina;
        private RumRibbonBarGroup pedidosBarGroup;
        private RumButtonElement rbtnCerrarOrden;
        private RumButtonElement rbtnConsumir;
        private RumRibbonBarGroup configBarGroup;
        private RumDropDownButtonElement rddbtnConfigurar;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar;
        private RumMenuItem temasMenu;
        private RumMenuItem editColumnas;
        private RumMenuItem filtrosMenu;
        private RumMenuItem guardarMenu;
        private RumMenuItem cargarMenu;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumButtonElement rbtnRefrescar;
        private Telerik.WinControls.UI.RadDropDownListElement rddlFiltrarMaquina;
        private Telerik.WinControls.UI.RadGridView rgvOrdenes;
        private RumRibbonBarGroup vistaGroup;
        private RumButtonElement rbtnExportar;
        private RumButtonElement rbtnBorrarFiltro;
        private RumButtonElement rBtnDevolucion;
        private RumButtonElement rBtnInicioOrden;
        private RumDropDownButtonElement rddFiltradoPor;
        private RumMenuItem rMenuOrdenesEnCurso;
        private RumMenuItem rMenuOrdenesPendientes;
        private Telerik.WinControls.UI.RadCheckBoxElement rCheckSoloPendientes;
        private RumMenuItem rMenuSinfiltro;
        private RumDropDownButtonElement rddBtnOpcionesArticulos;
        private RumMenuItem rBtnEliminarAcopios;
        private RumDropDownButtonElement rddBtnOpcionesFabricacion;
        private RumMenuItem rBtnAltaPalet;
        private RumMenuItem rBtnAltaPico;
        private RumMenuItem rBtnAltaMulti;
        private RumMenuItem rBtnAltaCarro;
        private RumMenuItem rBtnAltaTotales;
        private RumRibbonBarGroup accionesArtículosBarGgroup;
        private RumButtonElement rBtnConsumirMatricula;
        private RumMenuItem rBtnAnularConsumo;
        private RumMenuItem rBtnAnularConsumoParcial;
        private RumMenuItem rBtnAnularEntrada;
    }
}

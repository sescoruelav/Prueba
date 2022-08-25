namespace RumboSGA.GestionAlmacen
{
    partial class FrmDevolucionesProveedor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rgvGridDevoluciones = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonAcciones = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTabAccionesDevoluciones = new Telerik.WinControls.UI.RibbonTab();
            this.rRibbonBarGroupAcciones = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnRetirarDevolucion = new RumboSGA.RumButtonElement();
            this.rBtnCerrarDevolucion = new RumboSGA.RumButtonElement();
            this.rRibbonBarGroupVer = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnExportar = new RumboSGA.RumButtonElement();
            this.rBtnFiltrar = new RumboSGA.RumButtonElement();
            this.rBtnQuitarFiltro = new RumboSGA.RumButtonElement();
            this.rBtnRefrescar = new RumboSGA.RumButtonElement();
            this.rrbgConfiguracion = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rDDButtonConfiguracion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.itm_TemasMenu = new RumboSGA.Controles.RumMenuItem();
            this.itm_GuardarMenu = new RumboSGA.Controles.RumMenuItem();
            this.itm_CargarMenu = new RumboSGA.Controles.RumMenuItem();
            this.itm_EditarColumnas = new RumboSGA.Controles.RumMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridDevoluciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridDevoluciones.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonAcciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 711);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(1152, 24);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rgvGridDevoluciones);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 169);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1152, 542);
            this.panel1.TabIndex = 2;
            // 
            // rgvGridDevoluciones
            // 
            this.rgvGridDevoluciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvGridDevoluciones.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rgvGridDevoluciones.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvGridDevoluciones.Name = "rgvGridDevoluciones";
            this.rgvGridDevoluciones.Size = new System.Drawing.Size(1152, 542);
            this.rgvGridDevoluciones.TabIndex = 1;
            this.rgvGridDevoluciones.RowSourceNeeded += new Telerik.WinControls.UI.GridViewRowSourceNeededEventHandler(this.rgvGridDevoluciones_RowSourceNeeded);
            this.rgvGridDevoluciones.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.rgvGridDevoluciones_CellClick);
            this.rgvGridDevoluciones.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.rgvGridDevoluciones_ContextMenuOpening);
            this.rgvGridDevoluciones.FilterChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.rgvGridDevoluciones_FilterChanged);
            // 
            // radRibbonAcciones
            // 
            this.radRibbonAcciones.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTabAccionesDevoluciones});
            this.radRibbonAcciones.Location = new System.Drawing.Point(0, 0);
            this.radRibbonAcciones.Name = "radRibbonAcciones";
            // 
            // 
            // 
            this.radRibbonAcciones.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radRibbonAcciones.Size = new System.Drawing.Size(1152, 169);
            this.radRibbonAcciones.TabIndex = 0;
            this.radRibbonAcciones.Text = "Devoluciones Proveedor";
            // 
            // ribbonTabAccionesDevoluciones
            // 
            this.ribbonTabAccionesDevoluciones.IsSelected = true;
            this.ribbonTabAccionesDevoluciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rRibbonBarGroupAcciones,
            this.rRibbonBarGroupVer,
            this.rrbgConfiguracion});
            this.ribbonTabAccionesDevoluciones.Margin = new System.Windows.Forms.Padding(3);
            this.ribbonTabAccionesDevoluciones.Name = "ribbonTabAccionesDevoluciones";
            this.ribbonTabAccionesDevoluciones.Text = "Devoluciones";
            this.ribbonTabAccionesDevoluciones.UseMnemonic = false;
            // 
            // rRibbonBarGroupAcciones
            // 
            this.rRibbonBarGroupAcciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnRetirarDevolucion,
            this.rBtnCerrarDevolucion});
            this.rRibbonBarGroupAcciones.Name = "rRibbonBarGroupAcciones";
            this.rRibbonBarGroupAcciones.Text = "Acciones";
            // 
            // rBtnRetirarDevolucion
            // 
            this.rBtnRetirarDevolucion.EnableBorderHighlight = true;
            this.rBtnRetirarDevolucion.EnableElementShadow = false;
            this.rBtnRetirarDevolucion.EnableFocusBorder = true;
            this.rBtnRetirarDevolucion.EnableHighlight = true;
            this.rBtnRetirarDevolucion.EnableRippleAnimation = true;
            this.rBtnRetirarDevolucion.FocusBorderColor = System.Drawing.Color.Gray;
            this.rBtnRetirarDevolucion.FocusBorderWidth = 5;
            this.rBtnRetirarDevolucion.Image = global::RumboSGA.Properties.Resources.CopyFromTask;
            this.rBtnRetirarDevolucion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnRetirarDevolucion.Name = "rBtnRetirarDevolucion";
            this.rBtnRetirarDevolucion.RippleAnimationColor = System.Drawing.Color.White;
            this.rBtnRetirarDevolucion.Text = "Retirar Devolución";
            this.rBtnRetirarDevolucion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnRetirarDevolucion.ToolTipText = "Retirar Devolución";
            this.rBtnRetirarDevolucion.Click += new System.EventHandler(this.rBtnRetirarDevolucion_Click);
            // 
            // rBtnCerrarDevolucion
            // 
            this.rBtnCerrarDevolucion.EnableBorderHighlight = true;
            this.rBtnCerrarDevolucion.EnableFocusBorder = true;
            this.rBtnCerrarDevolucion.EnableHighlight = true;
            this.rBtnCerrarDevolucion.EnableRippleAnimation = true;
            this.rBtnCerrarDevolucion.Image = global::RumboSGA.Properties.Resources.Close;
            this.rBtnCerrarDevolucion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCerrarDevolucion.Name = "rBtnCerrarDevolucion";
            this.rBtnCerrarDevolucion.Text = "Cerrar Devolución";
            this.rBtnCerrarDevolucion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCerrarDevolucion.ToolTipText = "Cerrar Devolución";
            this.rBtnCerrarDevolucion.Click += new System.EventHandler(this.rBtnCerrarDevolucion_Click);
            // 
            // rRibbonBarGroupVer
            // 
            this.rRibbonBarGroupVer.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnExportar,
            this.rBtnFiltrar,
            this.rBtnQuitarFiltro,
            this.rBtnRefrescar});
            this.rRibbonBarGroupVer.Name = "rRibbonBarGroupVer";
            this.rRibbonBarGroupVer.Text = "Ver";
            // 
            // rBtnExportar
            // 
            this.rBtnExportar.EnableBorderHighlight = true;
            this.rBtnExportar.EnableElementShadow = false;
            this.rBtnExportar.EnableFocusBorder = true;
            this.rBtnExportar.EnableFocusBorderAnimation = true;
            this.rBtnExportar.EnableHighlight = true;
            this.rBtnExportar.EnableRippleAnimation = true;
            this.rBtnExportar.FocusBorderWidth = 5;
            this.rBtnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rBtnExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnExportar.Name = "rBtnExportar";
            this.rBtnExportar.Text = "Exportar";
            this.rBtnExportar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnExportar.ToolTipText = "Exportar";
            this.rBtnExportar.Click += new System.EventHandler(this.rBtnExportar_Click);
            // 
            // rBtnFiltrar
            // 
            this.rBtnFiltrar.EnableBorderHighlight = true;
            this.rBtnFiltrar.EnableFocusBorder = true;
            this.rBtnFiltrar.EnableFocusBorderAnimation = false;
            this.rBtnFiltrar.EnableHighlight = true;
            this.rBtnFiltrar.EnableRippleAnimation = true;
            this.rBtnFiltrar.FocusBorderWidth = 5;
            this.rBtnFiltrar.Image = global::RumboSGA.Properties.Resources.Filter;
            this.rBtnFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnFiltrar.Name = "rBtnFiltrar";
            this.rBtnFiltrar.Text = "Filtrar";
            this.rBtnFiltrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnFiltrar.ToolTipText = "Filtrar";
            this.rBtnFiltrar.Click += new System.EventHandler(this.rBtnFiltrar_Click);
            // 
            // rBtnQuitarFiltro
            // 
            this.rBtnQuitarFiltro.EnableBorderHighlight = true;
            this.rBtnQuitarFiltro.EnableFocusBorder = true;
            this.rBtnQuitarFiltro.EnableHighlight = true;
            this.rBtnQuitarFiltro.EnableRippleAnimation = true;
            this.rBtnQuitarFiltro.FocusBorderWidth = 5;
            this.rBtnQuitarFiltro.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.rBtnQuitarFiltro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnQuitarFiltro.Name = "rBtnQuitarFiltro";
            this.rBtnQuitarFiltro.Text = "Quitar Filtro";
            this.rBtnQuitarFiltro.TextAlignment = System.Drawing.ContentAlignment.BottomRight;
            this.rBtnQuitarFiltro.ToolTipText = "Quitar Filtro";
            this.rBtnQuitarFiltro.Click += new System.EventHandler(this.rBtnQuitarFiltro_Click);
            // 
            // rBtnRefrescar
            // 
            this.rBtnRefrescar.EnableBorderHighlight = true;
            this.rBtnRefrescar.EnableElementShadow = false;
            this.rBtnRefrescar.EnableFocusBorder = true;
            this.rBtnRefrescar.EnableHighlight = true;
            this.rBtnRefrescar.EnableRippleAnimation = true;
            this.rBtnRefrescar.FocusBorderWidth = 5;
            this.rBtnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rBtnRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnRefrescar.Name = "rBtnRefrescar";
            this.rBtnRefrescar.Text = "Refrescar";
            this.rBtnRefrescar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnRefrescar.ToolTipText = "Refrescar";
            this.rBtnRefrescar.Click += new System.EventHandler(this.rBtnRefrescar_Click);
            // 
            // rrbgConfiguracion
            // 
            this.rrbgConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rDDButtonConfiguracion});
            this.rrbgConfiguracion.Name = "rrbgConfiguracion";
            this.rrbgConfiguracion.Text = "Configuración";
            // 
            // rDDButtonConfiguracion
            // 
            this.rDDButtonConfiguracion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rDDButtonConfiguracion.AutoSize = false;
            this.rDDButtonConfiguracion.Bounds = new System.Drawing.Rectangle(0, 0, 53, 62);
            this.rDDButtonConfiguracion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rDDButtonConfiguracion.EnableBorderHighlight = true;
            this.rDDButtonConfiguracion.EnableFocusBorder = true;
            this.rDDButtonConfiguracion.EnableFocusBorderAnimation = true;
            this.rDDButtonConfiguracion.EnableHighlight = true;
            this.rDDButtonConfiguracion.EnableRippleAnimation = true;
            this.rDDButtonConfiguracion.ExpandArrowButton = false;
            this.rDDButtonConfiguracion.Image = global::RumboSGA.Properties.Resources.Administration;
            this.rDDButtonConfiguracion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rDDButtonConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.itm_TemasMenu,
            this.itm_GuardarMenu,
            this.itm_CargarMenu,
            this.itm_EditarColumnas});
            this.rDDButtonConfiguracion.Name = "rDDButtonConfiguracion";
            this.rDDButtonConfiguracion.Text = "";
            // 
            // itm_TemasMenu
            // 
            this.itm_TemasMenu.Name = "itm_TemasMenu";
            this.itm_TemasMenu.Text = "Temas";
            this.itm_TemasMenu.Click += new System.EventHandler(this.itm_TemasMenu_Click);
            // 
            // itm_GuardarMenu
            // 
            this.itm_GuardarMenu.Name = "itm_GuardarMenu";
            this.itm_GuardarMenu.Text = "Guardar Estilo";
            this.itm_GuardarMenu.Click += new System.EventHandler(this.itm_GuardarMenu_Click);
            // 
            // itm_CargarMenu
            // 
            this.itm_CargarMenu.Name = "itm_CargarMenu";
            this.itm_CargarMenu.Text = "Cargar Estilo";
            this.itm_CargarMenu.Click += new System.EventHandler(this.itm_CargarMenu_Click);
            // 
            // itm_EditarColumnas
            // 
            this.itm_EditarColumnas.Name = "itm_EditarColumnas";
            this.itm_EditarColumnas.Text = "Editar Columnas";
            this.itm_EditarColumnas.Click += new System.EventHandler(this.itm_EditarColumnas_Click);
            // 
            // FrmDevolucionesProveedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 735);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonAcciones);
            this.MainMenuStrip = null;
            this.Name = "FrmDevolucionesProveedor";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Devoluciones Proveedor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmDevolucionesProveedor_Load);
            this.Shown += new System.EventHandler(this.FrmDevolucionesProveedor_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridDevoluciones.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvGridDevoluciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonAcciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRibbonBar radRibbonAcciones;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RibbonTab ribbonTabAccionesDevoluciones;
        private Telerik.WinControls.UI.RadDropDownButtonElement radDropDownButtonElement1;
        private Controles.RumRibbonBarGroup rRibbonBarGroupAcciones;
        private Controles.RumRibbonBarGroup rRibbonBarGroupVer;
        private RumButtonElement rBtnRetirarDevolucion;
        private Telerik.WinControls.UI.RadRibbonBarGroup rrbgConfiguracion;
        private Controles.RumDropDownButtonElement rDDButtonConfiguracion;
        private RumboSGA.Controles.RumMenuItem itm_TemasMenu;
        private RumboSGA.Controles.RumMenuItem itm_GuardarMenu;
        private RumboSGA.Controles.RumMenuItem itm_CargarMenu;
        private RumboSGA.Controles.RumMenuItem itm_EditarColumnas;
        private Telerik.WinControls.UI.RadGridView rgvGridDevoluciones;
        private RumboSGA.RumButtonElement rBtnFiltrar;
        private RumboSGA.RumButtonElement rBtnQuitarFiltro;
        private RumboSGA.RumButtonElement rBtnRefrescar;
        private RumButtonElement rBtnExportar;
        private RumButtonElement rBtnCerrarDevolucion;
    }
}

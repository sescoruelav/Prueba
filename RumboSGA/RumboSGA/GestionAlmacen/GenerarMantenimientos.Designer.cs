namespace RumboSGA.GestionAlmacen
{
    partial class GenerarMantenimientos
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
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.mantenimientosTab = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnNuevo = new RumButtonElement();
            this.btnBorrar = new RumButtonElement();
            this.btnClonar = new RumButtonElement();
            this.radRibbonBarGroup3 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnCambiarVista = new RumButtonElement();
            this.btnExportar = new RumButtonElement();
            this.btnQuitarFiltros = new RumButtonElement();
            this.btnRefrescar = new RumButtonElement();
            this.btnBarraBusqueda = new RumButtonElement();
            this.radRibbonBarGroup2 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnConf = new Telerik.WinControls.UI.RadDropDownButtonElement();
            this.menuItemColumnas = new Telerik.WinControls.UI.RadMenuItem();
            this.menuItemGuardar = new Telerik.WinControls.UI.RadMenuItem();
            this.menuItemCargar = new Telerik.WinControls.UI.RadMenuItem();
            this.menuItemTemas = new Telerik.WinControls.UI.RadMenuItem();
            this.headerTamLetra = new Telerik.WinControls.UI.RadMenuHeaderItem();
            this.headerPaginacion = new Telerik.WinControls.UI.RadMenuHeaderItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuItemTamLetra = new Telerik.WinControls.UI.RadMenuComboItem();
            this.menuComboItem = new Telerik.WinControls.UI.RadMenuComboItem();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.mantenimientosTab});
            // 
            // 
            // 
            this.radRibbonBar1.ExitButton.Text = "Exit";
            this.radRibbonBar1.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radRibbonBar1.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.OptionsButton.Text = "Options";
            this.radRibbonBar1.OptionsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            // 
            // 
            // 
            this.radRibbonBar1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radRibbonBar1.Size = new System.Drawing.Size(852, 162);
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "generarMantenimientos";
            // 
            // mantenimientosTab
            // 
            this.mantenimientosTab.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
            this.mantenimientosTab.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentContent;
            this.mantenimientosTab.IsSelected = true;
            this.mantenimientosTab.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup1,
            this.radRibbonBarGroup3,
            this.radRibbonBarGroup2});
            this.mantenimientosTab.Name = "mantenimientosTab";
            this.mantenimientosTab.Text = "Herramientas";
            this.mantenimientosTab.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnNuevo,
            this.btnBorrar,
            this.btnClonar});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Herramientas";
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = global::RumboSGA.Properties.Resources.Add;
            this.btnNuevo.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnBorrar
            // 
            this.btnBorrar.Image = global::RumboSGA.Properties.Resources.Delete;
            this.btnBorrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBorrar.Name = "btnBorrar";
            this.btnBorrar.Text = "Borrar";
            this.btnBorrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
            // 
            // btnClonar
            // 
            this.btnClonar.Image = global::RumboSGA.Properties.Resources.copy;
            this.btnClonar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClonar.Name = "btnClonar";
            this.btnClonar.Text = "Clonar";
            this.btnClonar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnClonar.Click += new System.EventHandler(this.btnClonar_Click);
            // 
            // radRibbonBarGroup3
            // 
            this.radRibbonBarGroup3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnCambiarVista,
            this.btnExportar,
            this.btnQuitarFiltros,
            this.btnRefrescar,
            this.btnBarraBusqueda});
            this.radRibbonBarGroup3.Name = "radRibbonBarGroup3";
            this.radRibbonBarGroup3.Text = "Ver";
            // 
            // btnCambiarVista
            // 
            this.btnCambiarVista.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.btnCambiarVista.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCambiarVista.Name = "btnCambiarVista";
            this.btnCambiarVista.Text = "Cambiar Vista";
            this.btnCambiarVista.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCambiarVista.Click += new System.EventHandler(this.CambiarVista_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Text = "radButtonElement2";
            this.btnExportar.Click += new System.EventHandler(this.Exportar_Click);
            // 
            // btnQuitarFiltros
            // 
            this.btnQuitarFiltros.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnQuitarFiltros.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnQuitarFiltros.Name = "btnQuitarFiltros";
            this.btnQuitarFiltros.Text = "radButtonElement3";
            this.btnQuitarFiltros.Click += new System.EventHandler(this.QuitarFiltros_Click);
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Text = "radButtonElement4";
            this.btnRefrescar.Click += new System.EventHandler(this.Refrescar_Click);
            // 
            // btnBarraBusqueda
            // 
            this.btnBarraBusqueda.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
            this.btnBarraBusqueda.Image = global::RumboSGA.Properties.Resources.View;
            this.btnBarraBusqueda.Name = "btnBarraBusqueda";
            this.btnBarraBusqueda.Text = "radButtonElement5";
            this.btnBarraBusqueda.Click += new System.EventHandler(this.BarraBusqueda_Click);
            // 
            // radRibbonBarGroup2
            // 
            this.radRibbonBarGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnConf});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = "";
            // 
            // btnConf
            // 
            this.btnConf.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.btnConf.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.btnConf.ExpandArrowButton = false;
            this.btnConf.Image = global::RumboSGA.Properties.Resources.Administration;
            this.btnConf.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConf.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuItemColumnas,
            this.menuItemGuardar,
            this.menuItemCargar,
            this.menuItemTemas,
            this.headerTamLetra,
            this.menuItemTamLetra,
            this.headerPaginacion,
            this.menuComboItem});
            this.btnConf.Name = "btnConf";
            this.btnConf.Text = "Configuracion";
            this.btnConf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // menuItemColumnas
            // 
            this.menuItemColumnas.Name = "menuItemColumnas";
            this.menuItemColumnas.Text = "Columnas";
            // 
            // menuItemGuardar
            // 
            this.menuItemGuardar.Name = "menuItemGuardar";
            this.menuItemGuardar.Text = "Guardar Estilo";
            // 
            // menuItemCargar
            // 
            this.menuItemCargar.Name = "menuItemCargar";
            this.menuItemCargar.Text = "Cargar Estilo";
            // 
            // menuItemTemas
            // 
            this.menuItemTemas.Name = "menuItemTemas";
            this.menuItemTemas.Text = "Temas";
            // 
            // headerTamLetra
            // 
            this.headerTamLetra.Name = "headerTamLetra";
            this.headerTamLetra.Text = "Cambiar Letra";
            // 
            // headerPaginacion
            // 
            this.headerPaginacion.Name = "headerPaginacion";
            this.headerPaginacion.Text = "Paginacion";
            this.headerPaginacion.UseCompatibleTextRendering = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(852, 476);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // menuItemTamLetra
            // 
            // 
            // 
            // 
            this.menuItemTamLetra.ComboBoxElement.AutoCompleteAppend = null;
            this.menuItemTamLetra.ComboBoxElement.AutoCompleteDataSource = null;
            this.menuItemTamLetra.ComboBoxElement.AutoCompleteSuggest = null;
            this.menuItemTamLetra.ComboBoxElement.DataMember = "";
            this.menuItemTamLetra.ComboBoxElement.DataSource = null;
            this.menuItemTamLetra.ComboBoxElement.DefaultValue = null;
            this.menuItemTamLetra.ComboBoxElement.DisplayMember = "";
            this.menuItemTamLetra.ComboBoxElement.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            this.menuItemTamLetra.ComboBoxElement.DropDownAnimationEnabled = true;
            this.menuItemTamLetra.ComboBoxElement.EditableElementText = "";
            this.menuItemTamLetra.ComboBoxElement.EditorElement = this.menuItemTamLetra.ComboBoxElement;
            this.menuItemTamLetra.ComboBoxElement.EditorManager = null;
            this.menuItemTamLetra.ComboBoxElement.Filter = null;
            this.menuItemTamLetra.ComboBoxElement.FilterExpression = "";
            this.menuItemTamLetra.ComboBoxElement.Focusable = true;
            this.menuItemTamLetra.ComboBoxElement.FormatString = "";
            this.menuItemTamLetra.ComboBoxElement.FormattingEnabled = true;
            this.menuItemTamLetra.ComboBoxElement.MaxDropDownItems = 0;
            this.menuItemTamLetra.ComboBoxElement.MaxLength = 32767;
            this.menuItemTamLetra.ComboBoxElement.MaxValue = null;
            this.menuItemTamLetra.ComboBoxElement.MinValue = null;
            this.menuItemTamLetra.ComboBoxElement.NullValue = null;
            this.menuItemTamLetra.ComboBoxElement.OwnerOffset = 0;
            this.menuItemTamLetra.ComboBoxElement.ShowImageInEditorArea = true;
            this.menuItemTamLetra.ComboBoxElement.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            this.menuItemTamLetra.ComboBoxElement.Value = null;
            this.menuItemTamLetra.ComboBoxElement.ValueMember = "";
            this.menuItemTamLetra.Name = "menuItemTamLetra";
            this.menuItemTamLetra.Text = "";
            // 
            // menuComboItem
            // 
            // 
            // 
            // 
            this.menuComboItem.ComboBoxElement.AutoCompleteAppend = null;
            this.menuComboItem.ComboBoxElement.AutoCompleteDataSource = null;
            this.menuComboItem.ComboBoxElement.AutoCompleteSuggest = null;
            this.menuComboItem.ComboBoxElement.DataMember = "";
            this.menuComboItem.ComboBoxElement.DataSource = null;
            this.menuComboItem.ComboBoxElement.DefaultValue = null;
            this.menuComboItem.ComboBoxElement.DisplayMember = "";
            this.menuComboItem.ComboBoxElement.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            this.menuComboItem.ComboBoxElement.DropDownAnimationEnabled = true;
            this.menuComboItem.ComboBoxElement.EditableElementText = "";
            this.menuComboItem.ComboBoxElement.EditorElement = this.menuComboItem.ComboBoxElement;
            this.menuComboItem.ComboBoxElement.EditorManager = null;
            this.menuComboItem.ComboBoxElement.Filter = null;
            this.menuComboItem.ComboBoxElement.FilterExpression = "";
            this.menuComboItem.ComboBoxElement.Focusable = true;
            this.menuComboItem.ComboBoxElement.FormatString = "";
            this.menuComboItem.ComboBoxElement.FormattingEnabled = true;
            this.menuComboItem.ComboBoxElement.MaxDropDownItems = 0;
            this.menuComboItem.ComboBoxElement.MaxLength = 32767;
            this.menuComboItem.ComboBoxElement.MaxValue = null;
            this.menuComboItem.ComboBoxElement.MinValue = null;
            this.menuComboItem.ComboBoxElement.NullValue = null;
            this.menuComboItem.ComboBoxElement.OwnerOffset = 0;
            this.menuComboItem.ComboBoxElement.ShowImageInEditorArea = true;
            this.menuComboItem.ComboBoxElement.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            this.menuComboItem.ComboBoxElement.Value = null;
            this.menuComboItem.ComboBoxElement.ValueMember = "";
            this.menuComboItem.Name = "menuComboItem";
            this.menuComboItem.Text = "";
            // 
            // GenerarMantenimientos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 638);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "GenerarMantenimientos";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "generarMantenimientos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab mantenimientosTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement btnNuevo;
        private RumButtonElement btnBorrar;
        private RumButtonElement btnClonar;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup2;
        private Telerik.WinControls.UI.RadDropDownButtonElement btnConf;
        private Telerik.WinControls.UI.RadMenuItem menuItemColumnas;
        private Telerik.WinControls.UI.RadMenuItem menuItemGuardar;
        private Telerik.WinControls.UI.RadMenuItem menuItemCargar;
        private Telerik.WinControls.UI.RadMenuItem menuItemTemas;
        private Telerik.WinControls.UI.RadMenuHeaderItem headerTamLetra;
        private Telerik.WinControls.UI.RadMenuHeaderItem headerPaginacion;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup3;
        private RumButtonElement btnCambiarVista;
        private RumButtonElement btnExportar;
        private RumButtonElement btnQuitarFiltros;
        private RumButtonElement btnRefrescar;
        private RumButtonElement btnBarraBusqueda;
        private Telerik.WinControls.UI.RadMenuComboItem menuItemTamLetra;
        private Telerik.WinControls.UI.RadMenuComboItem menuComboItem;
    }
}

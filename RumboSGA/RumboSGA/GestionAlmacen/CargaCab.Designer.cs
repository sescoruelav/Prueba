using RumboSGA.Controles;

namespace RumboSGA.GestionAlmacen
{
    partial class CargaCab
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
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.modBarGroup = new RumRibbonBarGroup();
            this.btnNuevo = new RumButtonElement();
            this.btnBorrar = new RumButtonElement();
            this.btnClonar = new RumButtonElement();
            this.vistaBarGroup = new RumRibbonBarGroup();
            this.btnCambiarVista = new RumButtonElement();
            this.btnExportar = new RumButtonElement();
            this.radButtonElement1 = new RumButtonElement();
            this.btnActualizar = new RumButtonElement();
            this.configBarGroup = new RumRibbonBarGroup();
            this.btnConf = new RumDropDownButtonElement();
            this.menuItemColumnas = new RumboSGA.Controles.RumMenuItem();
            this.menuItemGuardar = new RumboSGA.Controles.RumMenuItem();
            this.menuItemCargar = new RumboSGA.Controles.RumMenuItem();
            this.menuItemTemas = new RumboSGA.Controles.RumMenuItem();
            this.headerTamLetra = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuItemTamLetra = new RumboSGA.Controles.RumMenuComboItem();
            this.headerPaginacion = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuComboItem = new RumboSGA.Controles.RumMenuComboItem();
            this.radRibbonBarGroup4 = new RumRibbonBarGroup();
            this.radButtonElement2 = new RumButtonElement();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
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
            this.radRibbonBar1.Size = new System.Drawing.Size(1077, 164);
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "CargaCab";
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.modBarGroup,
            this.vistaBarGroup,
            this.configBarGroup,
            this.radRibbonBarGroup4});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Acciones";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // modBarGroup
            // 
            this.modBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnNuevo,
            this.btnBorrar,
            this.btnClonar});
            this.modBarGroup.Name = "modBarGroup";
            this.modBarGroup.Text = "Modificaciones";
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = global::RumboSGA.Properties.Resources.Add;
            this.btnNuevo.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNuevo.Click += new System.EventHandler(this.Nuevo_Click);
            // 
            // btnBorrar
            // 
            this.btnBorrar.Image = global::RumboSGA.Properties.Resources.Delete;
            this.btnBorrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBorrar.Name = "btnBorrar";
            this.btnBorrar.Text = "Borrar";
            this.btnBorrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBorrar.Click += new System.EventHandler(this.Borrar_Click);
            // 
            // btnClonar
            // 
            this.btnClonar.Image = global::RumboSGA.Properties.Resources.copy;
            this.btnClonar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClonar.Name = "btnClonar";
            this.btnClonar.Text = "Clonar";
            this.btnClonar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClonar.Click += new System.EventHandler(this.Clonar_Click);
            // 
            // vistaBarGroup
            // 
            this.vistaBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnCambiarVista,
            this.btnExportar,
            this.radButtonElement1,
            this.btnActualizar});
            this.vistaBarGroup.Name = "vistaBarGroup";
            this.vistaBarGroup.Text = "Ver";
            // 
            // btnCambiarVista
            // 
            this.btnCambiarVista.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.btnCambiarVista.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCambiarVista.Name = "btnCambiarVista";
            this.btnCambiarVista.Text = "Cambiar Vista";
            this.btnCambiarVista.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCambiarVista.Click += new System.EventHandler(this.CambiarVista_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.btnExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Text = "";
            this.btnExportar.Click += new System.EventHandler(this.Exportar_Click);
            // 
            // radButtonElement1
            // 
            this.radButtonElement1.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.radButtonElement1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement1.Name = "radButtonElement1";
            this.radButtonElement1.Text = "";
            this.radButtonElement1.Click += new System.EventHandler(this.QuitarFiltros_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnActualizar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Text = "";
            this.btnActualizar.Click += new System.EventHandler(this.Refrescar_Click);
            // 
            // configBarGroup
            // 
            this.configBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnConf});
            this.configBarGroup.Name = "configBarGroup";
            this.configBarGroup.Text = "Configuracion";
            // 
            // btnConf
            // 
            this.btnConf.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConf.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.btnConf.AutoSize = false;
            this.btnConf.Bounds = new System.Drawing.Rectangle(0, 0, 74, 65);
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
            this.btnConf.Text = "";
            // 
            // menuItemColumnas
            // 
            this.menuItemColumnas.Name = "menuItemColumnas";
            this.menuItemColumnas.Text = "radMenuItem1";
            // 
            // menuItemGuardar
            // 
            this.menuItemGuardar.Name = "menuItemGuardar";
            this.menuItemGuardar.Text = "radMenuItem1";
            // 
            // menuItemCargar
            // 
            this.menuItemCargar.Name = "menuItemCargar";
            this.menuItemCargar.Text = "radMenuItem1";
            // 
            // menuItemTemas
            // 
            this.menuItemTemas.Name = "menuItemTemas";
            this.menuItemTemas.Text = "Temas";
            // 
            // headerTamLetra
            // 
            this.headerTamLetra.Name = "headerTamLetra";
            this.headerTamLetra.Text = "Tamaño Letra";
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
            this.menuItemTamLetra.Text = "Tamaño Letra Tabla";
            // 
            // headerPaginacion
            // 
            this.headerPaginacion.Name = "headerPaginacion";
            this.headerPaginacion.Text = "Registros Paginación";
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
            // radRibbonBarGroup4
            // 
            this.radRibbonBarGroup4.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElement2});
            this.radRibbonBarGroup4.Name = "radRibbonBarGroup4";
            this.radRibbonBarGroup4.Text = "Jira";
            // 
            // radButtonElement2
            // 
            this.radButtonElement2.Image = global::RumboSGA.Properties.Resources.support;
            this.radButtonElement2.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement2.Name = "radButtonElement2";
            this.radButtonElement2.Text = "";
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 577);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(1077, 26);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 164);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1077, 413);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 413F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1077, 413);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // CargaCab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 603);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "CargaCab";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "CargaCab";
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuItemTamLetra.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup modBarGroup;
        private RumButtonElement btnNuevo;
        private RumRibbonBarGroup vistaBarGroup;
        private RumRibbonBarGroup configBarGroup;
        private RumRibbonBarGroup radRibbonBarGroup4;
        private RumButtonElement btnBorrar;
        private RumButtonElement btnClonar;
        private RumDropDownButtonElement btnConf;
        private RumMenuItem menuItemColumnas;
        private RumMenuItem menuItemGuardar;
        private RumMenuItem menuItemCargar;
        private RumMenuItem menuItemTemas;
        private RumMenuComboItem menuItemTamLetra;
        private RumMenuHeaderItem headerTamLetra;
        private RumMenuHeaderItem headerPaginacion;
        private RumMenuComboItem menuComboItem;
        private RumButtonElement btnCambiarVista;
        private RumButtonElement btnExportar;
        private RumButtonElement btnActualizar;
        private RumButtonElement radButtonElement1;
        private RumButtonElement radButtonElement2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

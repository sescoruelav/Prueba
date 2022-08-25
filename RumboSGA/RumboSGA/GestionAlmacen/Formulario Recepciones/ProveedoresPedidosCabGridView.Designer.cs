using RumboSGA.Controles;

namespace RumboSGA.Presentation.FormularioRecepciones
{
    partial class ProveedoresPedidosCabGridView
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
            this.rgvPedidos = new RumboSGA.Controles.RumGridView();
            this.tlPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.rrbBarraBotones = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new RumboSGA.Controles.RumRibbonTab();
            this.procesosBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rbtnPedidos = new RumboSGA.RumButtonElement();
            this.rBtnRecepciones = new RumboSGA.RumButtonElement();
            this.tablaGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnExportacion = new RumboSGA.RumButtonElement();
            this.rBtnFiltros = new RumboSGA.RumButtonElement();
            this.rBtnBorrarFiltro = new RumboSGA.RumButtonElement();
            this.rBtnActualizar = new RumboSGA.RumButtonElement();
            this.rBtnBarraBusqueda = new RumboSGA.RumButtonElement();
            this.configBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rDDBtnConfiguracion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.temasMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.guardarMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.cargarMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.editarMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.menuHeaderFormatoFecha = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuComboFormatoFecha = new RumboSGA.Controles.RumMenuComboItem();
            this.rBtnGenerarTarea = new RumboSGA.RumButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPedidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPedidos.MasterTemplate)).BeginInit();
            this.tlPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rrbBarraBotones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboFormatoFecha.ComboBoxElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvPedidos
            // 
            this.rgvPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvPedidos.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvPedidos.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvPedidos.Name = "rgvPedidos";
            this.rgvPedidos.Size = new System.Drawing.Size(1112, 398);
            this.rgvPedidos.TabIndex = 0;
            // 
            // tlPrincipal
            // 
            this.tlPrincipal.ColumnCount = 1;
            this.tlPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPrincipal.Controls.Add(this.rgvPedidos, 0, 0);
            this.tlPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlPrincipal.Location = new System.Drawing.Point(0, 162);
            this.tlPrincipal.Name = "tlPrincipal";
            this.tlPrincipal.RowCount = 1;
            this.tlPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPrincipal.Size = new System.Drawing.Size(1118, 404);
            this.tlPrincipal.TabIndex = 5;
            // 
            // rrbBarraBotones
            // 
            this.rrbBarraBotones.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            // 
            // 
            // 
            this.rrbBarraBotones.ExitButton.Text = "Exit";
            this.rrbBarraBotones.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.rrbBarraBotones.Location = new System.Drawing.Point(0, 0);
            this.rrbBarraBotones.Name = "rrbBarraBotones";
            // 
            // 
            // 
            this.rrbBarraBotones.OptionsButton.Text = "Options";
            this.rrbBarraBotones.Size = new System.Drawing.Size(1118, 162);
            this.rrbBarraBotones.TabIndex = 4;
            this.rrbBarraBotones.Text = "Recepciones";
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.rrbBarraBotones.GetChildAt(0))).Text = "Recepciones";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.rrbBarraBotones.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.procesosBarGroup,
            this.tablaGroup,
            this.configBarGroup});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // procesosBarGroup
            // 
            this.procesosBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rbtnPedidos,
            this.rBtnRecepciones});
            this.procesosBarGroup.Name = "procesosBarGroup";
            // 
            // rbtnPedidos
            // 
            this.rbtnPedidos.AutoSize = false;
            this.rbtnPedidos.Bounds = new System.Drawing.Rectangle(0, 0, 62, 62);
            this.rbtnPedidos.Image = global::RumboSGA.Properties.Resources.edit;
            this.rbtnPedidos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnPedidos.Name = "rbtnPedidos";
            this.rbtnPedidos.Text = "Pedidos";
            this.rbtnPedidos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnPedidos.ToolTipText = "Pedidos";
            this.rbtnPedidos.Click += new System.EventHandler(this.rbtnPedidos_Click);
            // 
            // rBtnRecepciones
            // 
            this.rBtnRecepciones.AutoSize = false;
            this.rBtnRecepciones.Bounds = new System.Drawing.Rectangle(0, 0, 68, 62);
            this.rBtnRecepciones.Image = global::RumboSGA.Properties.Resources.GoTo;
            this.rBtnRecepciones.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnRecepciones.Name = "rBtnRecepciones";
            this.rBtnRecepciones.Text = "Recepciones";
            this.rBtnRecepciones.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnRecepciones.ToolTipText = "Recepciones";
            this.rBtnRecepciones.Click += new System.EventHandler(this.rBtnRecepcionesButton_Click);
            // 
            // tablaGroup
            // 
            this.tablaGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnExportacion,
            this.rBtnFiltros,
            this.rBtnBorrarFiltro,
            this.rBtnActualizar,
            this.rBtnBarraBusqueda});
            this.tablaGroup.Name = "tablaGroup";
            this.tablaGroup.Text = "";
            // 
            // rBtnExportacion
            // 
            this.rBtnExportacion.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rBtnExportacion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnExportacion.Name = "rBtnExportacion";
            this.rBtnExportacion.Text = "";
            this.rBtnExportacion.Click += new System.EventHandler(this.rBtnExportacion_Click);
            // 
            // rBtnFiltros
            // 
            this.rBtnFiltros.Image = global::RumboSGA.Properties.Resources.Filter;
            this.rBtnFiltros.Name = "rBtnFiltros";
            this.rBtnFiltros.Text = "";
            this.rBtnFiltros.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.rBtnFiltros.Click += new System.EventHandler(this.OcultarFiltros);
            // 
            // rBtnBorrarFiltro
            // 
            this.rBtnBorrarFiltro.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.rBtnBorrarFiltro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnBorrarFiltro.Name = "rBtnBorrarFiltro";
            this.rBtnBorrarFiltro.Text = "";
            this.rBtnBorrarFiltro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnBorrarFiltro.Click += new System.EventHandler(this.rBtnBorrarFiltro_Click);
            // 
            // rBtnActualizar
            // 
            this.rBtnActualizar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rBtnActualizar.Name = "rBtnActualizar";
            this.rBtnActualizar.Text = "";
            this.rBtnActualizar.Click += new System.EventHandler(this.rBtnActualizar_Click);
            // 
            // rBtnBarraBusqueda
            // 
            this.rBtnBarraBusqueda.Image = global::RumboSGA.Properties.Resources.View;
            this.rBtnBarraBusqueda.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnBarraBusqueda.Name = "rBtnBarraBusqueda";
            this.rBtnBarraBusqueda.Text = "";
            this.rBtnBarraBusqueda.Click += new System.EventHandler(this.OcultarBarraBusqueda);
            // 
            // configBarGroup
            // 
            this.configBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rDDBtnConfiguracion});
            this.configBarGroup.Name = "configBarGroup";
            // 
            // rDDBtnConfiguracion
            // 
            this.rDDBtnConfiguracion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rDDBtnConfiguracion.AutoSize = false;
            this.rDDBtnConfiguracion.Bounds = new System.Drawing.Rectangle(0, 0, 70, 62);
            this.rDDBtnConfiguracion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rDDBtnConfiguracion.ExpandArrowButton = false;
            this.rDDBtnConfiguracion.Image = global::RumboSGA.Properties.Resources.Administration;
            this.rDDBtnConfiguracion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rDDBtnConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.guardarMenuItem,
            this.cargarMenuItem,
            this.editarMenuItem,
            this.menuHeaderFormatoFecha,
            this.menuComboFormatoFecha});
            this.rDDBtnConfiguracion.Name = "rDDBtnConfiguracion";
            this.rDDBtnConfiguracion.Text = "";
            this.rDDBtnConfiguracion.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.rDDBtnConfiguracion.UseCompatibleTextRendering = false;
            // 
            // temasMenuItem
            // 
            this.temasMenuItem.Name = "temasMenuItem";
            this.temasMenuItem.Text = "Temas";
            // 
            // guardarMenuItem
            // 
            this.guardarMenuItem.Name = "guardarMenuItem";
            this.guardarMenuItem.Text = "Guardar Estilo";
            this.guardarMenuItem.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarMenuItem
            // 
            this.cargarMenuItem.Name = "cargarMenuItem";
            this.cargarMenuItem.Text = "Cargar Estilo";
            this.cargarMenuItem.Click += new System.EventHandler(this.loadItem_Click);
            // 
            // editarMenuItem
            // 
            this.editarMenuItem.Name = "editarMenuItem";
            this.editarMenuItem.Text = "Editar Columnas";
            this.editarMenuItem.Click += new System.EventHandler(this.rBtnEditar_Click);
            // 
            // menuHeaderFormatoFecha
            // 
            this.menuHeaderFormatoFecha.Name = "menuHeaderFormatoFecha";
            this.menuHeaderFormatoFecha.Text = "Cambiar Formato Fecha";
            // 
            // menuComboFormatoFecha
            // 
            // 
            // 
            // 
            this.menuComboFormatoFecha.ComboBoxElement.AutoCompleteAppend = null;
            this.menuComboFormatoFecha.ComboBoxElement.AutoCompleteDataSource = null;
            this.menuComboFormatoFecha.ComboBoxElement.AutoCompleteSuggest = null;
            this.menuComboFormatoFecha.ComboBoxElement.DataMember = "";
            this.menuComboFormatoFecha.ComboBoxElement.DataSource = null;
            this.menuComboFormatoFecha.ComboBoxElement.DefaultValue = null;
            this.menuComboFormatoFecha.ComboBoxElement.DisplayMember = "";
            this.menuComboFormatoFecha.ComboBoxElement.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            this.menuComboFormatoFecha.ComboBoxElement.DropDownAnimationEnabled = true;
            this.menuComboFormatoFecha.ComboBoxElement.EditableElementText = "";
            this.menuComboFormatoFecha.ComboBoxElement.EditorElement = this.menuComboFormatoFecha.ComboBoxElement;
            this.menuComboFormatoFecha.ComboBoxElement.EditorManager = null;
            this.menuComboFormatoFecha.ComboBoxElement.Filter = null;
            this.menuComboFormatoFecha.ComboBoxElement.FilterExpression = "";
            this.menuComboFormatoFecha.ComboBoxElement.Focusable = true;
            this.menuComboFormatoFecha.ComboBoxElement.FormatString = "";
            this.menuComboFormatoFecha.ComboBoxElement.FormattingEnabled = true;
            this.menuComboFormatoFecha.ComboBoxElement.MaxDropDownItems = 0;
            this.menuComboFormatoFecha.ComboBoxElement.MaxLength = 32767;
            this.menuComboFormatoFecha.ComboBoxElement.MaxValue = null;
            this.menuComboFormatoFecha.ComboBoxElement.MinValue = null;
            this.menuComboFormatoFecha.ComboBoxElement.NullValue = null;
            this.menuComboFormatoFecha.ComboBoxElement.OwnerOffset = 0;
            this.menuComboFormatoFecha.ComboBoxElement.ShowImageInEditorArea = true;
            this.menuComboFormatoFecha.ComboBoxElement.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            this.menuComboFormatoFecha.ComboBoxElement.Value = null;
            this.menuComboFormatoFecha.ComboBoxElement.ValueMember = "";
            this.menuComboFormatoFecha.Name = "menuComboFormatoFecha";
            this.menuComboFormatoFecha.Text = "radMenuComboItem1";
            // 
            // rBtnGenerarTarea
            // 
            this.rBtnGenerarTarea.Name = "rBtnGenerarTarea";
            // 
            // ProveedoresPedidosCabGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 566);
            this.Controls.Add(this.tlPrincipal);
            this.Controls.Add(this.rrbBarraBotones);
            this.MainMenuStrip = null;
            this.Name = "ProveedoresPedidosCabGridView";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Recepciones";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ProveedoresPedidosCabGridView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgvPedidos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPedidos)).EndInit();
            this.tlPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rrbBarraBotones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboFormatoFecha.ComboBoxElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Telerik.WinControls.UI.RadRibbonBar rrbBarraBotones;
        private RumRibbonTab ribbonTab1;
        private RumRibbonBarGroup procesosBarGroup;
        private RumButtonElement rBtnRecepciones;
        private RumButtonElement rbtnPedidos;
        private RumButtonElement rBtnGenerarTarea;
        private RumRibbonBarGroup configBarGroup;
        private RumMenuItem temasMenuItem;
        private RumMenuItem guardarMenuItem;
        private RumMenuItem cargarMenuItem;
        private RumMenuItem editarMenuItem;
        private RumDropDownButtonElement rDDBtnConfiguracion;       
        private System.Windows.Forms.TableLayoutPanel tlPrincipal;
        private RumboSGA.Controles.RumGridView rgvPedidos;
        private RumRibbonBarGroup tablaGroup;
        private RumButtonElement rBtnExportacion;
        private RumButtonElement rBtnBorrarFiltro;
        private RumMenuHeaderItem menuHeaderFormatoFecha;
        private RumMenuComboItem menuComboFormatoFecha;
        private RumButtonElement rBtnActualizar;
        private RumButtonElement rBtnFiltros;
        private RumButtonElement rBtnBarraBusqueda;
    }
}

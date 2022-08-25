using RumboSGA.Controles;

namespace RumboSGA.Presentation.Herramientas
{
    partial class ControlTareas
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
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.pedidosButton = new RumboSGA.RumButtonElement();
            this.operarioButton = new RumboSGA.RumButtonElement();
            this.borrarButton = new RumboSGA.RumButtonElement();
            this.exportarButton = new RumboSGA.RumButtonElement();
            this.refreshButton = new RumboSGA.RumButtonElement();
            this.configBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.configButton = new RumDropDownButtonElement();
            this.temasMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.guardarButton = new RumboSGA.Controles.RumMenuItem();
            this.cargarButton = new RumboSGA.Controles.RumMenuItem();
            this.editColumns = new RumboSGA.Controles.RumMenuItem();
            this.pagHeaderItem = new RumboSGA.Controles.RumMenuHeaderItem();
            this.menuComboItem = new Telerik.WinControls.UI.RadMenuComboItem();
            this.groupAcciones = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnEliminarHuerfanas = new RumboSGA.RumButtonElement();
            this.rBtnMovimientosEnError = new RumboSGA.RumButtonElement();
            this.rBtnEliminarReposiciones = new RumboSGA.RumButtonElement();
            this.tlPanelPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.rgvPrincipal = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).BeginInit();
            this.tlPanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPrincipal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPrincipal.MasterTemplate)).BeginInit();
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
            this.radRibbonBar1.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.OptionsButton.Text = "Options";
            this.radRibbonBar1.Size = new System.Drawing.Size(964, 162);
            this.radRibbonBar1.TabIndex = 1;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.vistaBarGroup,
            this.configBarGroup,
            this.groupAcciones});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // vistaBarGroup
            // 
            this.vistaBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.pedidosButton,
            this.operarioButton,
            this.borrarButton,
            this.exportarButton,
            this.refreshButton});
            this.vistaBarGroup.Name = "vistaBarGroup";
            this.vistaBarGroup.Text = "";
            // 
            // pedidosButton
            // 
            this.pedidosButton.AutoSize = true;
            this.pedidosButton.Image = global::RumboSGA.Properties.Resources.edit;
            this.pedidosButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.pedidosButton.Name = "pedidosButton";
            this.pedidosButton.Text = "";
            this.pedidosButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.pedidosButton.Click += new System.EventHandler(this.PedidosButton_Click);
            // 
            // operarioButton
            // 
            this.operarioButton.AutoSize = true;
            this.operarioButton.Image = global::RumboSGA.Properties.Resources.Table;
            this.operarioButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.operarioButton.Name = "operarioButton";
            this.operarioButton.Text = "";
            this.operarioButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.operarioButton.Click += new System.EventHandler(this.OperarioButton_Click);
            // 
            // borrarButton
            // 
            this.borrarButton.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.borrarButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.borrarButton.Name = "borrarButton";
            this.borrarButton.Text = "";
            this.borrarButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.borrarButton.Click += new System.EventHandler(this.PrincipalButton_Click);
            // 
            // exportarButton
            // 
            this.exportarButton.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.exportarButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.exportarButton.Name = "exportarButton";
            this.exportarButton.Text = "";
            this.exportarButton.Click += new System.EventHandler(this.btnExportacion_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.refreshButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Text = "";
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // configBarGroup
            // 
            this.configBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.configButton});
            this.configBarGroup.Name = "configBarGroup";
            this.configBarGroup.Text = "";
            // 
            // configButton
            // 
            this.configButton.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.configButton.AutoSize = false;
            this.configButton.Bounds = new System.Drawing.Rectangle(0, 0, 80, 62);
            this.configButton.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.configButton.ExpandArrowButton = false;
            this.configButton.Image = global::RumboSGA.Properties.Resources.Administration;
            this.configButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.configButton.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.guardarButton,
            this.cargarButton,
            this.editColumns,
            this.pagHeaderItem,
            this.menuComboItem});
            this.configButton.Name = "configButton";
            this.configButton.Text = "";
            // 
            // temasMenuItem
            // 
            this.temasMenuItem.Name = "temasMenuItem";
            this.temasMenuItem.Text = "";
            // 
            // guardarButton
            // 
            this.guardarButton.Name = "guardarButton";
            this.guardarButton.Text = "";
            this.guardarButton.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarButton
            // 
            this.cargarButton.Name = "cargarButton";
            this.cargarButton.Text = "";
            this.cargarButton.Click += new System.EventHandler(this.LoadItem_Click);
            // 
            // editColumns
            // 
            this.editColumns.Name = "editColumns";
            this.editColumns.Text = "";
            this.editColumns.Click += new System.EventHandler(this.ItemColumnas_Click);
            // 
            // pagHeaderItem
            // 
            this.pagHeaderItem.Name = "pagHeaderItem";
            this.pagHeaderItem.Text = "Registros Paginación";
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
            // groupAcciones
            // 
            this.groupAcciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnEliminarHuerfanas,
            this.rBtnMovimientosEnError,
            this.rBtnEliminarReposiciones});
            this.groupAcciones.Name = "groupAcciones";
            this.groupAcciones.Text = "Acciones";
            // 
            // rBtnEliminarHuerfanas
            // 
            this.rBtnEliminarHuerfanas.Image = global::RumboSGA.Properties.Resources.trash;
            this.rBtnEliminarHuerfanas.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnEliminarHuerfanas.Name = "rBtnEliminarHuerfanas";
            this.rBtnEliminarHuerfanas.Text = "Eliminar Huérfanas";
            this.rBtnEliminarHuerfanas.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnEliminarHuerfanas.Click += new System.EventHandler(this.rBtnEliminarHuerfanas_Click);
            // 
            // rBtnMovimientosEnError
            // 
            this.rBtnMovimientosEnError.Image = global::RumboSGA.Properties.Resources.Overdue;
            this.rBtnMovimientosEnError.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnMovimientosEnError.Name = "rBtnMovimientosEnError";
            this.rBtnMovimientosEnError.Text = "Movimientos en Error";
            this.rBtnMovimientosEnError.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnMovimientosEnError.Click += new System.EventHandler(this.rBtnMovimientosEnError_Click);
            // 
            // rBtnEliminarReposiciones
            // 
            this.rBtnEliminarReposiciones.Image = global::RumboSGA.Properties.Resources.trash;
            this.rBtnEliminarReposiciones.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnEliminarReposiciones.Name = "rBtnEliminarReposiciones";
            this.rBtnEliminarReposiciones.Text = "Eliminar Reposiciones";
            this.rBtnEliminarReposiciones.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnEliminarReposiciones.Click += new System.EventHandler(this.rBtnEliminarReposiciones_Click);
            // 
            // tlPanelPrincipal
            // 
            this.tlPanelPrincipal.ColumnCount = 4;
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlPanelPrincipal.Controls.Add(this.rgvPrincipal, 0, 0);
            this.tlPanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlPanelPrincipal.Location = new System.Drawing.Point(0, 162);
            this.tlPanelPrincipal.Name = "tlPanelPrincipal";
            this.tlPanelPrincipal.RowCount = 1;
            this.tlPanelPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanelPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlPanelPrincipal.Size = new System.Drawing.Size(964, 380);
            this.tlPanelPrincipal.TabIndex = 2;
            // 
            // rgvPrincipal
            // 
            this.tlPanelPrincipal.SetColumnSpan(this.rgvPrincipal, 4);
            this.rgvPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvPrincipal.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvPrincipal.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvPrincipal.Name = "rgvPrincipal";
            this.rgvPrincipal.Size = new System.Drawing.Size(958, 374);
            this.rgvPrincipal.TabIndex = 12;
            // 
            // ControlTareas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 542);
            this.Controls.Add(this.tlPanelPrincipal);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "ControlTareas";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuComboItem.ComboBoxElement)).EndInit();
            this.tlPanelPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvPrincipal.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPrincipal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup vistaBarGroup;
        private RumButtonElement pedidosButton;
        private RumButtonElement operarioButton;
        private RumButtonElement borrarButton;
        private RumRibbonBarGroup configBarGroup;
        private RumDropDownButtonElement configButton;
        private RumMenuItem guardarButton;
        private RumMenuItem cargarButton;
        private RumMenuItem editColumns;
        private System.Windows.Forms.TableLayoutPanel tlPanelPrincipal;
        private Telerik.WinControls.UI.RadGridView rgvPrincipal;
        private RumMenuItem temasMenuItem;
        private RumButtonElement exportarButton;
        private RumButtonElement refreshButton;
        private RumMenuHeaderItem pagHeaderItem;
        private Telerik.WinControls.UI.RadMenuComboItem menuComboItem;
        private RumRibbonBarGroup groupAcciones;
        private RumButtonElement rBtnEliminarHuerfanas;
        private RumButtonElement rBtnMovimientosEnError;
        private RumButtonElement rBtnEliminarReposiciones;
    }
}

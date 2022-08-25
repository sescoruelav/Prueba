using System;
using RumboSGA.Controles;

namespace RumboSGA.Presentation.Herramientas
{
    partial class ArticulosFrm
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
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            nuevoButton = new RumboSGA.RumButtonElement();
            editArticulo = new RumboSGA.RumButtonElement();
            cleanButton = new RumboSGA.RumButtonElement();
            clonarButton = new RumboSGA.RumButtonElement();
            saveButton = new RumboSGA.RumButtonElement();
            cancelarButton = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup2 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnFiltrar = new RumboSGA.RumButtonElement();
            this.btnExportar = new RumboSGA.RumButtonElement();
            this.btnRefrescar = new RumboSGA.RumButtonElement();
            this.btnCambiarVista = new RumboSGA.RumButtonElement();
            this.editButton = new Telerik.WinControls.UI.RadButtonElement();
            this.radRibbonBarGroup4 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.btnHistoricoArticulos = new RumboSGA.RumButtonElement();
            this.btnConfigurar = new RumDropDownButtonElement();
            this.btnEstadistica = new RumboSGA.RumButtonElement();
            this.btnCodigoBarras = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup3 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.configButton = new RumDropDownButtonElement();
            this.temasMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.guardarButton = new RumboSGA.Controles.RumMenuItem();
            this.cargarButton = new RumboSGA.Controles.RumMenuItem();
            this.editColumns = new RumboSGA.Controles.RumMenuItem();
            this.pagHeaderItem = new RumboSGA.Controles.RumMenuHeaderItem();
            menuComboItem = new Telerik.WinControls.UI.RadMenuComboItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.radButtonElement12 = new Telerik.WinControls.UI.RadButtonElement();
            this.radButtonElement13 = new Telerik.WinControls.UI.RadButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(menuComboItem.ComboBoxElement)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.radRibbonBar1.TabStop = false;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup1,
            this.radRibbonBarGroup2,
            this.radRibbonBarGroup4,
            this.radRibbonBarGroup3});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Acciones";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            nuevoButton,
            editArticulo,
            cleanButton,
            clonarButton,
            saveButton,
            cancelarButton});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Modificaciones";
            // 
            // nuevoButton
            // 
            nuevoButton.Image = global::RumboSGA.Properties.Resources.Add;
            nuevoButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            nuevoButton.Name = "nuevoButton";
            nuevoButton.Text = "Nuevo";
            nuevoButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            nuevoButton.Click += new System.EventHandler(this.NuevoButton_Click);
            // 
            // editArticulo
            // 
            editArticulo.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            editArticulo.Image = global::RumboSGA.Properties.Resources.edit;
            editArticulo.ImageAlignment = System.Drawing.ContentAlignment.MiddleRight;
            editArticulo.Name = "editArticulo";
            editArticulo.Text = "Editar";
            editArticulo.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            editArticulo.UseCompatibleTextRendering = true;
            editArticulo.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // cleanButton
            // 
            cleanButton.Image = global::RumboSGA.Properties.Resources.Delete;
            cleanButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            cleanButton.Name = "cleanButton";
            cleanButton.Text = "Borrar";
            cleanButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            cleanButton.Click += new System.EventHandler(this.EliminarButton_Click);
            // 
            // clonarButton
            // 
            clonarButton.Image = global::RumboSGA.Properties.Resources.copy;
            clonarButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            clonarButton.Name = "clonarButton";
            clonarButton.Text = "Clonar";
            clonarButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            clonarButton.Click += new System.EventHandler(this.ClonarButton_Click);
            // 
            // saveButton
            // 
            saveButton.Image = global::RumboSGA.Properties.Resources.Save;
            saveButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            saveButton.Name = "saveButton";
            saveButton.Text = "Guardar";
            saveButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            saveButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelarButton
            // 
            cancelarButton.Image = global::RumboSGA.Properties.Resources.Cancel;
            cancelarButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            cancelarButton.Name = "cancelarButton";
            cancelarButton.Text = "Cancelar";
            cancelarButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            cancelarButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // radRibbonBarGroup2
            // 
            this.radRibbonBarGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnFiltrar,
            this.btnExportar,
            this.btnRefrescar,
            this.btnCambiarVista,
            this.editButton});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = "Ver";
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFiltrar.Click += new System.EventHandler(this.QuitarFiltros_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.btnExportar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Text = "Excel";
            this.btnExportar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExportar.Click += new System.EventHandler(this.btnExportacion_Click);
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnRefrescar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefrescar.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // btnCambiarVista
            // 
            this.btnCambiarVista.Image = global::RumboSGA.Properties.Resources.Cambiar;
            this.btnCambiarVista.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCambiarVista.Name = "btnCambiarVista";
            this.btnCambiarVista.Text = "Cambiar a vista rápida";
            this.btnCambiarVista.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCambiarVista.Click += new System.EventHandler(this.CambiarVista_Click);
            // 
            // editButton
            // 
            this.editButton.Name = "editButton";
            // 
            // radRibbonBarGroup4
            // 
            this.radRibbonBarGroup4.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnHistoricoArticulos,
            this.btnConfigurar,
            this.btnEstadistica,
            this.btnCodigoBarras});
            this.radRibbonBarGroup4.Name = "radRibbonBarGroup4";
            this.radRibbonBarGroup4.Text = "Acciones";
            // 
            // btnHistoricoArticulos
            // 
            this.btnHistoricoArticulos.Image = global::RumboSGA.Properties.Resources.History;
            this.btnHistoricoArticulos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnHistoricoArticulos.Name = "btnHistoricoArticulos";
            this.btnHistoricoArticulos.Text = "Histórico";
            this.btnHistoricoArticulos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHistoricoArticulos.Click += new System.EventHandler(this.btnHistArticulos_Event);
            // 
            // btnConfigurar
            // 
            this.btnConfigurar.Image = global::RumboSGA.Properties.Resources.EditList;
            this.btnConfigurar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnConfigurar.Name = "btnConfigurar";
            this.btnConfigurar.Text = "Configurar";
            this.btnConfigurar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConfigurar.Click += new System.EventHandler(this.btnConfigurarArticulos_Event);
            // 
            // btnEstadistica
            // 
            this.btnEstadistica.Image = global::RumboSGA.Properties.Resources.estadistica;
            this.btnEstadistica.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEstadistica.Name = "btnEstadistica";
            this.btnEstadistica.Text = "Estadística";
            this.btnEstadistica.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCodigoBarras
            // 
            this.btnCodigoBarras.Image = global::RumboSGA.Properties.Resources.codigobar;
            this.btnCodigoBarras.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCodigoBarras.Name = "btnCodigoBarras";
            this.btnCodigoBarras.Text = "Código Barras";
            this.btnCodigoBarras.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // radRibbonBarGroup3
            // 
            this.radRibbonBarGroup3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.configButton});
            this.radRibbonBarGroup3.Name = "radRibbonBarGroup3";
            this.radRibbonBarGroup3.Text = "Herramientas";
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
            menuComboItem});
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
            menuComboItem.ComboBoxElement.AutoCompleteAppend = null;
            menuComboItem.ComboBoxElement.AutoCompleteDataSource = null;
            menuComboItem.ComboBoxElement.AutoCompleteSuggest = null;
            menuComboItem.ComboBoxElement.DataMember = "";
            menuComboItem.ComboBoxElement.DataSource = null;
            menuComboItem.ComboBoxElement.DefaultValue = null;
            menuComboItem.ComboBoxElement.DisplayMember = "";
            menuComboItem.ComboBoxElement.DropDownAnimationEasing = Telerik.WinControls.RadEasingType.InQuad;
            menuComboItem.ComboBoxElement.DropDownAnimationEnabled = true;
            menuComboItem.ComboBoxElement.EditableElementText = "";
            menuComboItem.ComboBoxElement.EditorElement = menuComboItem.ComboBoxElement;
            menuComboItem.ComboBoxElement.EditorManager = null;
            menuComboItem.ComboBoxElement.Filter = null;
            menuComboItem.ComboBoxElement.FilterExpression = "";
            menuComboItem.ComboBoxElement.Focusable = true;
            menuComboItem.ComboBoxElement.FormatString = "";
            menuComboItem.ComboBoxElement.FormattingEnabled = true;
            menuComboItem.ComboBoxElement.MaxDropDownItems = 0;
            menuComboItem.ComboBoxElement.MaxLength = 32767;
            menuComboItem.ComboBoxElement.MaxValue = null;
            menuComboItem.ComboBoxElement.MinValue = null;
            menuComboItem.ComboBoxElement.NullValue = null;
            menuComboItem.ComboBoxElement.OwnerOffset = 0;
            menuComboItem.ComboBoxElement.ShowImageInEditorArea = true;
            menuComboItem.ComboBoxElement.SortStyle = Telerik.WinControls.Enumerations.SortStyle.None;
            menuComboItem.ComboBoxElement.Value = null;
            menuComboItem.ComboBoxElement.ValueMember = "";
            menuComboItem.Name = "menuComboItem";
            menuComboItem.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(964, 380);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 4);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 374F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(958, 374);
            this.tableLayoutPanel2.TabIndex = 14;
            // 
            // vistaBarGroup
            // 
            this.vistaBarGroup.Margin = new System.Windows.Forms.Padding(0);
            this.vistaBarGroup.MaxSize = new System.Drawing.Size(0, 0);
            this.vistaBarGroup.MinSize = new System.Drawing.Size(0, 0);
            this.vistaBarGroup.Name = "vistaBarGroup";
            this.vistaBarGroup.Text = "";
            this.vistaBarGroup.UseCompatibleTextRendering = false;
            // 
            // radButtonElement12
            // 
            this.radButtonElement12.Image = global::RumboSGA.Properties.Resources.copy;
            this.radButtonElement12.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement12.Name = "radButtonElement12";
            this.radButtonElement12.Text = "Clonar";
            this.radButtonElement12.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement12.UseCompatibleTextRendering = false;
            // 
            // radButtonElement13
            // 
            this.radButtonElement13.Image = global::RumboSGA.Properties.Resources.copy;
            this.radButtonElement13.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement13.Name = "radButtonElement13";
            this.radButtonElement13.Text = "Clonar";
            this.radButtonElement13.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement13.UseCompatibleTextRendering = false;
            // 
            // ArticulosFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 542);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "ArticulosFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(menuComboItem.ComboBoxElement)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       /* private void RefreshButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }*/

        private void PedidosButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OperarioButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PrincipalButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        

        
        #endregion
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumDropDownButtonElement configButton;
        private RumMenuItem guardarButton;
        private RumMenuItem cargarButton;
        private RumMenuItem editColumns;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumMenuItem temasMenuItem;
        private RumMenuHeaderItem pagHeaderItem;
        public static Telerik.WinControls.UI.RadMenuComboItem menuComboItem;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup2;
        private RumButtonElement btnFiltrar;
        private RumButtonElement btnExportar;
        private RumButtonElement btnRefrescar;
        private RumButtonElement btnCambiarVista;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup3;
        private RumRibbonBarGroup vistaBarGroup;
        public System.Windows.Forms.TabPage tabPageGeneral { get; set; }
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Telerik.WinControls.UI.RadButtonElement radButtonElement12;
        private Telerik.WinControls.UI.RadButtonElement radButtonElement13;
        public Telerik.WinControls.UI.RadButtonElement editButton;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup4;
        private RumButtonElement btnHistoricoArticulos;
        private RumDropDownButtonElement btnConfigurar;
        private RumButtonElement btnEstadistica;
        private RumButtonElement btnCodigoBarras;
        public static RumButtonElement nuevoButton;
        public static RumButtonElement cleanButton;
        public static RumButtonElement clonarButton;
        public static RumButtonElement editArticulo;
        public static RumButtonElement saveButton;
        public static RumButtonElement cancelarButton;
    }
}

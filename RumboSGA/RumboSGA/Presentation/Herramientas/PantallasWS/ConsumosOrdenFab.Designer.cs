using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using System.Windows.Forms;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class ConsumosOrdenFab
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgvConsumos = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.consumeButton = new RumboSGA.RumButtonElement();
            this.cancelarButton = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup2 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rddBtnConfiguracion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.temasMenu = new RumboSGA.Controles.RumMenuItem();
            this.guardarMenu = new RumboSGA.Controles.RumMenuItem();
            this.cargarMenu = new RumboSGA.Controles.RumMenuItem();
            this.editColumns = new RumboSGA.Controles.RumMenuItem();           
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvConsumos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvConsumos.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.rgvConsumos, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(955, 629);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rgvConsumos
            // 
            this.rgvConsumos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvConsumos.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvConsumos.MasterTemplate.AllowAddNewRow = false;
            this.rgvConsumos.MasterTemplate.AllowDeleteRow = false;
            this.rgvConsumos.MasterTemplate.EnableFiltering = true;
            this.rgvConsumos.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.rgvConsumos.Name = "rgvConsumos";
            this.rgvConsumos.Size = new System.Drawing.Size(949, 623);
            this.rgvConsumos.TabIndex = 12;
            this.rgvConsumos.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.rgvConsumos_ViewCellFormatting);
            this.rgvConsumos.CellValidating += new Telerik.WinControls.UI.CellValidatingEventHandler(this.rgvConsumos_CellValidating);
            this.rgvConsumos.ChildViewExpanding += new Telerik.WinControls.UI.ChildViewExpandingEventHandler(this.rgvConsumos_ChildViewExpanding);
            this.rgvConsumos.CellValueChanged += new Telerik.WinControls.UI.GridViewCellEventHandler(this.rgvConsumos_CellValueChanged);
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
            this.radRibbonBar1.Size = new System.Drawing.Size(955, 162);
            this.radRibbonBar1.TabIndex = 2;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup1,
            this.radRibbonBarGroup2});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.consumeButton,
            this.cancelarButton});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Consumir";
            // 
            // consumeButton
            // 
            this.consumeButton.Image = global::RumboSGA.Properties.Resources.edit;
            this.consumeButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.consumeButton.Name = "consumeButton";
            this.consumeButton.Text = "Consumir";
            this.consumeButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.consumeButton.ToolTipText = "Consumir";
            this.consumeButton.Click += new System.EventHandler(this.ConsumeButton_Click);
            // 
            // cancelarButton
            // 
            this.cancelarButton.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.cancelarButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.cancelarButton.Name = "cancelarButton";
            this.cancelarButton.Text = "Exit";
            this.cancelarButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.cancelarButton.ToolTipText = "Exit";
            this.cancelarButton.Click += new System.EventHandler(this.CancelarBtton_Click);
            // 
            // radRibbonBarGroup2
            // 
            this.radRibbonBarGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rddBtnConfiguracion});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = "Configuración";
            // 
            // rddBtnConfiguracion
            // 
            this.rddBtnConfiguracion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rddBtnConfiguracion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rddBtnConfiguracion.ExpandArrowButton = false;
            this.rddBtnConfiguracion.Image = global::RumboSGA.Properties.Resources.Administration;
            this.rddBtnConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenu,
            this.guardarMenu,
            this.cargarMenu,
            this.editColumns});
            this.rddBtnConfiguracion.Name = "rddBtnConfiguracion";
            this.rddBtnConfiguracion.Text = "";
            this.rddBtnConfiguracion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // temasMenu
            // 
            this.temasMenu.Name = "temasMenu";
            this.temasMenu.Text = "Temas";
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
            // editColumns
            // 
            this.editColumns.Name = "editColumns";
            this.editColumns.Text = "Editar Columnas";
            this.editColumns.Click += new System.EventHandler(this.ItemColumnas_Click);
            
            // 
            // ConsumosOrdenFab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 791);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "ConsumosOrdenFab";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvConsumos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvConsumos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement consumeButton;
        private RumButtonElement cancelarButton;
        private Telerik.WinControls.UI.RadGridView rgvConsumos;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup2;
        private RumDropDownButtonElement rddBtnConfiguracion;
        private RumMenuItem temasMenu;
        private RumMenuItem guardarMenu;
        private RumMenuItem cargarMenu;
        private RumMenuItem editColumns;
       
    }
}

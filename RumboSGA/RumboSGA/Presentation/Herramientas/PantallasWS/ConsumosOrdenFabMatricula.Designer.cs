using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using System.Windows.Forms;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class ConsumosOrdenFabMatricula
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.consumosGridView = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.consumeButton = new RumboSGA.RumButtonElement();
            this.cancelarButton = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup2 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rddFiltradoPor = new RumboSGA.Controles.RumDropDownButtonElement();
            this.rBtnItmExistenciasZona = new RumboSGA.Controles.RumMenuItem();
            this.rBtnItmExistenciasFueraZona = new RumboSGA.Controles.RumMenuItem();
            this.rBtnItmOtrosArticulos = new RumboSGA.Controles.RumMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.consumosGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.consumosGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.562795F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.562795F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.87441F));
            this.tableLayoutPanel1.Controls.Add(this.consumosGridView, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 146);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 645F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 645F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(955, 645);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // consumosGridView
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.consumosGridView, 3);
            this.consumosGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consumosGridView.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.consumosGridView.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.consumosGridView.Name = "consumosGridView";
            this.consumosGridView.Size = new System.Drawing.Size(949, 639);
            this.consumosGridView.TabIndex = 12;
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
            this.radRibbonBar1.Size = new System.Drawing.Size(955, 146);
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
            this.rddFiltradoPor});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = "Filtros";
            // 
            // rddFiltradoPor
            // 
            this.rddFiltradoPor.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rddFiltradoPor.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rddFiltradoPor.ExpandArrowButton = false;
            this.rddFiltradoPor.Image = global::RumboSGA.Properties.Resources.Debug;
            this.rddFiltradoPor.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rddFiltradoPor.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnItmExistenciasZona,
            this.rBtnItmExistenciasFueraZona,
            this.rBtnItmOtrosArticulos});
            this.rddFiltradoPor.Name = "rddFiltradoPor";
            this.rddFiltradoPor.Text = "Filtrado por";
            this.rddFiltradoPor.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // rBtnItmExistenciasZona
            // 
            this.rBtnItmExistenciasZona.Name = "rBtnItmExistenciasZona";
            this.rBtnItmExistenciasZona.Text = "Existencias en la zona de fabricación";
            this.rBtnItmExistenciasZona.Click += new System.EventHandler(this.rBtnItmExistenciasZona_Click);
            // 
            // rBtnItmExistenciasFueraZona
            // 
            this.rBtnItmExistenciasFueraZona.Name = "rBtnItmExistenciasFueraZona";
            this.rBtnItmExistenciasFueraZona.Text = "Existencias fuera de la zona de fabricación";
            this.rBtnItmExistenciasFueraZona.Click += new System.EventHandler(this.rBtnItmExistenciasFueraZona_Click);
            // 
            // rBtnItmOtrosArticulos
            // 
            this.rBtnItmOtrosArticulos.Name = "rBtnItmOtrosArticulos";
            this.rBtnItmOtrosArticulos.Text = "Otros Artículos";
            this.rBtnItmOtrosArticulos.Click += new System.EventHandler(this.rBtnItmOtrosArticulos_Click);
            // 
            // ConsumosOrdenFabMatricula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 791);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "ConsumosOrdenFabMatricula";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.consumosGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.consumosGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView consumosGridView;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement consumeButton;
        private RumButtonElement cancelarButton;
        private RumDropDownButtonElement rddFiltradoPor;
        private RumMenuItem rBtnItmExistenciasZona;
        private RumMenuItem rBtnItmExistenciasFueraZona;
        private RumMenuItem rBtnItmOtrosArticulos;
        private RumRibbonBarGroup radRibbonBarGroup2;
    }
}

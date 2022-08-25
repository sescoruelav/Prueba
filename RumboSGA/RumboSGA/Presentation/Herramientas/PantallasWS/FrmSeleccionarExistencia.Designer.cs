using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using System.Windows.Forms;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class FrmSeleccionarExistencia
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgvExistencias = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnSeleccionar = new RumboSGA.RumButtonElement();
            this.rBtnCancelar = new RumboSGA.RumButtonElement();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvExistencias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvExistencias.MasterTemplate)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.rgvExistencias, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 629F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 629F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(955, 629);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rgvExistencias
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rgvExistencias, 3);
            this.rgvExistencias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvExistencias.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvExistencias.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvExistencias.Name = "rgvExistencias";
            this.rgvExistencias.Size = new System.Drawing.Size(949, 623);
            this.rgvExistencias.TabIndex = 12;
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
            this.radRibbonBarGroup1});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Botones";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnSeleccionar,
            this.rBtnCancelar});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Acciones";
            // 
            // rBtnSeleccionar
            // 
            this.rBtnSeleccionar.Image = global::RumboSGA.Properties.Resources.edit;
            this.rBtnSeleccionar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnSeleccionar.Name = "rBtnSeleccionar";
            this.rBtnSeleccionar.Text = "Seleccionar";
            this.rBtnSeleccionar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnSeleccionar.Click += new System.EventHandler(this.rBtnSeleccionar_Click);
            // 
            // rBtnCancelar
            // 
            this.rBtnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rBtnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCancelar.Name = "rBtnCancelar";
            this.rBtnCancelar.Text = "Cancelar";
            this.rBtnCancelar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCancelar.Click += new System.EventHandler(this.rBtnCancelar_Click);
            // 
            // FrmSeleccionarExistencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 791);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.MainMenuStrip = null;
            this.Name = "FrmSeleccionarExistencia";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmSeleccionarExistencia_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvExistencias.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvExistencias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView rgvExistencias;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement rBtnSeleccionar;
        private RumButtonElement rBtnCancelar;
    }
}

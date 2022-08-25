using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using System.Windows.Forms;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class FrmVerMovimientosError
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
            this.rgvMovimientos = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTabMovimientos = new Telerik.WinControls.UI.RibbonTab();
            this.ribbonTabCarro = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup2 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnRegenerarTareaCarro = new RumboSGA.RumButtonElement();
            this.rBtnCancelarCarro = new RumboSGA.RumButtonElement();
            this.ribbonTabBulto = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup3 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnRegenerarTareaBulto = new RumboSGA.RumButtonElement();
            this.rBtnCancelarBulto = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup1 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnRegenerarTareaReserva = new RumboSGA.RumButtonElement();
            this.rBtnCancelar = new RumboSGA.RumButtonElement();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMovimientos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMovimientos.MasterTemplate)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.rgvMovimientos, 0, 0);
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
            // rgvMovimientos
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rgvMovimientos, 3);
            this.rgvMovimientos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvMovimientos.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvMovimientos.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvMovimientos.Name = "rgvMovimientos";
            this.rgvMovimientos.Size = new System.Drawing.Size(949, 623);
            this.rgvMovimientos.TabIndex = 12;
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTabMovimientos,
            this.ribbonTabCarro,
            this.ribbonTabBulto});
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
            this.radRibbonBar1.Text = "Movimientos";
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).Text = "Movimientos";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTabMovimientos
            // 
            this.ribbonTabMovimientos.IsSelected = true;
            this.ribbonTabMovimientos.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup1});
            this.ribbonTabMovimientos.Name = "ribbonTabMovimientos";
            this.ribbonTabMovimientos.Text = "Ubicación";
            this.ribbonTabMovimientos.UseMnemonic = false;
            this.ribbonTabMovimientos.Click += new System.EventHandler(this.ribbonTabMovimientos_Click);
            // 
            // ribbonTabCarro
            // 
            this.ribbonTabCarro.IsSelected = false;
            this.ribbonTabCarro.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup2});
            this.ribbonTabCarro.Name = "ribbonTabCarro";
            this.ribbonTabCarro.Text = "Carro";
            this.ribbonTabCarro.UseMnemonic = false;
            this.ribbonTabCarro.Click += new System.EventHandler(this.ribbonTabCarro_Click);
            // 
            // radRibbonBarGroup2
            // 
            this.radRibbonBarGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnRegenerarTareaCarro,
            this.rBtnCancelarCarro});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = "Acciones";
            // 
            // rBtnRegenerarTareaCarro
            // 
            this.rBtnRegenerarTareaCarro.Image = global::RumboSGA.Properties.Resources._001_repair_tools;
            this.rBtnRegenerarTareaCarro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnRegenerarTareaCarro.Name = "rBtnRegenerarTareaCarro";
            this.rBtnRegenerarTareaCarro.Text = "Regenerar tarea";
            this.rBtnRegenerarTareaCarro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnRegenerarTareaCarro.Click += new System.EventHandler(this.rBtnRegenerarTareaCarro_Click);
            // 
            // rBtnCancelarCarro
            // 
            this.rBtnCancelarCarro.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rBtnCancelarCarro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCancelarCarro.Name = "rBtnCancelarCarro";
            this.rBtnCancelarCarro.Text = "Cancelar";
            this.rBtnCancelarCarro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCancelarCarro.Click += new System.EventHandler(this.rBtnCancelarCarro_Click);
            // 
            // ribbonTabBulto
            // 
            this.ribbonTabBulto.IsSelected = false;
            this.ribbonTabBulto.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup3});
            this.ribbonTabBulto.Name = "ribbonTabBulto";
            this.ribbonTabBulto.Text = "Bulto";
            this.ribbonTabBulto.UseMnemonic = false;
            this.ribbonTabBulto.Click += new System.EventHandler(this.ribbonTabBulto_Click);
            // 
            // radRibbonBarGroup3
            // 
            this.radRibbonBarGroup3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnRegenerarTareaBulto,
            this.rBtnCancelarBulto});
            this.radRibbonBarGroup3.Name = "radRibbonBarGroup3";
            this.radRibbonBarGroup3.Text = "Acciones";
            // 
            // rBtnRegenerarTareaBulto
            // 
            this.rBtnRegenerarTareaBulto.Image = global::RumboSGA.Properties.Resources._001_repair_tools;
            this.rBtnRegenerarTareaBulto.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnRegenerarTareaBulto.Name = "rBtnRegenerarTareaBulto";
            this.rBtnRegenerarTareaBulto.Text = "Regenerar tarea";
            this.rBtnRegenerarTareaBulto.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnRegenerarTareaBulto.Click += new System.EventHandler(this.rBtnRegenerarTareaBulto_Click);
            // 
            // rBtnCancelarBulto
            // 
            this.rBtnCancelarBulto.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rBtnCancelarBulto.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCancelarBulto.Name = "rBtnCancelarBulto";
            this.rBtnCancelarBulto.Text = "Cancelar";
            this.rBtnCancelarBulto.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCancelarBulto.Click += new System.EventHandler(this.rBtnCancelarBulto_Click);
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnRegenerarTareaReserva,
            this.rBtnCancelar});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Acciones";
            // 
            // rBtnRegenerarTareaReserva
            // 
            this.rBtnRegenerarTareaReserva.Image = global::RumboSGA.Properties.Resources._001_repair_tools;
            this.rBtnRegenerarTareaReserva.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnRegenerarTareaReserva.Name = "rBtnRegenerarTareaReserva";
            this.rBtnRegenerarTareaReserva.Text = "Regenerar tarea";
            this.rBtnRegenerarTareaReserva.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnRegenerarTareaReserva.Click += new System.EventHandler(this.rBtnRegenerarTareaMov_Click);
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
            // FrmVerMovimientosError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 791);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.MainMenuStrip = null;
            this.Name = "FrmVerMovimientosError";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Movimientos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmVerMovimientosError_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvMovimientos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMovimientos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView rgvMovimientos;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTabMovimientos;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement rBtnRegenerarTareaReserva;
        private RumButtonElement rBtnCancelar;
        private Telerik.WinControls.UI.RibbonTab ribbonTabCarro;
        private Telerik.WinControls.UI.RibbonTab ribbonTabBulto;
        private RumRibbonBarGroup radRibbonBarGroup2;
        private RumButtonElement rBtnRegenerarTareaCarro;
        private RumButtonElement rBtnCancelarCarro;
        private RumRibbonBarGroup radRibbonBarGroup3;
        private RumButtonElement rBtnRegenerarTareaBulto;
        private RumButtonElement rBtnCancelarBulto;
    }
}

using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using System.Windows.Forms;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class FrmResumenPedidosCarga
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
            this.rgvResumenCargaPedidos = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new RumboSGA.Controles.RumRibbonTab();
            this.radRibbonBarGroup1 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnConfirmar = new RumboSGA.RumButtonElement();
            this.rBtnCancelar = new RumboSGA.RumButtonElement();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvResumenCargaPedidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvResumenCargaPedidos.MasterTemplate)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.rgvResumenCargaPedidos, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 629F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 629F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1182, 450);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rgvResumenCargaPedidos
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rgvResumenCargaPedidos, 3);
            this.rgvResumenCargaPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvResumenCargaPedidos.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.rgvResumenCargaPedidos.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.rgvResumenCargaPedidos.Name = "rgvResumenCargaPedidos";
            this.rgvResumenCargaPedidos.Size = new System.Drawing.Size(1176, 444);
            this.rgvResumenCargaPedidos.TabIndex = 12;
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
            this.radRibbonBar1.Size = new System.Drawing.Size(1182, 162);
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
            this.ribbonTab1.Tag = "";
            this.ribbonTab1.Text = "Confirmación cierre carga";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnConfirmar,
            this.rBtnCancelar});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Acciones";
            // 
            // rBtnConfirmar
            // 
            this.rBtnConfirmar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.rBtnConfirmar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnConfirmar.Name = "rBtnConfirmar";
            this.rBtnConfirmar.Text = "Confirmar";
            this.rBtnConfirmar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnConfirmar.ToolTipText = "Confirmar";
            this.rBtnConfirmar.Click += new System.EventHandler(this.rBtnConfirmar_Click);
            // 
            // rBtnCancelar
            // 
            this.rBtnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rBtnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCancelar.Name = "rBtnCancelar";
            this.rBtnCancelar.Text = "Cancelar";
            this.rBtnCancelar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCancelar.ToolTipText = "Cancelar";
            this.rBtnCancelar.Click += new System.EventHandler(this.rBtnCancelar_Click);
            // 
            // FrmResumenPedidosCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 612);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.MainMenuStrip = null;
            this.Name = "FrmResumenPedidosCarga";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Load += new System.EventHandler(this.FrmResumenPedidosCarga_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvResumenCargaPedidos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvResumenCargaPedidos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView rgvResumenCargaPedidos;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private RumRibbonTab ribbonTab1;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement rBtnConfirmar;
        private RumButtonElement rBtnCancelar;
    }
}

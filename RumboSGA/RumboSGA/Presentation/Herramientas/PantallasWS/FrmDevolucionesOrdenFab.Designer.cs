using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using System.Windows.Forms;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    partial class FrmDevolucionesOrdenFab
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
            this.gvDevoluciones = new Telerik.WinControls.UI.RadGridView();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnDevolver = new RumboSGA.RumButtonElement();
            this.rBtnImprimirEtiqueta = new RumboSGA.RumButtonElement();
            this.rbtnCancelar = new RumboSGA.RumButtonElement();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDevoluciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDevoluciones.MasterTemplate)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.gvDevoluciones, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 162);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(955, 629);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // gvDevoluciones
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.gvDevoluciones, 3);
            this.gvDevoluciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDevoluciones.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.gvDevoluciones.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gvDevoluciones.Name = "gvDevoluciones";
            this.gvDevoluciones.Size = new System.Drawing.Size(949, 623);
            this.gvDevoluciones.TabIndex = 12;
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
            this.rBtnDevolver,
            this.rBtnImprimirEtiqueta,
            this.rbtnCancelar});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Devolución";
            // 
            // rBtnDevolver
            // 
            this.rBtnDevolver.Image = global::RumboSGA.Properties.Resources.edit;
            this.rBtnDevolver.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnDevolver.Name = "rBtnDevolver";
            this.rBtnDevolver.Text = "Devolver";
            this.rBtnDevolver.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnDevolver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rBtnDevolver.Click += new System.EventHandler(this.rBtnDevolver_Click);
            // 
            // rBtnImprimirEtiqueta
            // 
            this.rBtnImprimirEtiqueta.Image = global::RumboSGA.Properties.Resources.codigobar;
            this.rBtnImprimirEtiqueta.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnImprimirEtiqueta.Name = "rBtnImprimirEtiqueta";
            this.rBtnImprimirEtiqueta.Text = "Imprimir Etiqueta";
            this.rBtnImprimirEtiqueta.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnImprimirEtiqueta.Click += new System.EventHandler(this.rBtnImprimirEtiqueta_Click);
            // 
            // rbtnCancelar
            // 
            this.rbtnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rbtnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnCancelar.Name = "rbtnCancelar";
            this.rbtnCancelar.Text = "Cancelar";
            this.rbtnCancelar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rbtnCancelar.Click += new System.EventHandler(this.rBtnCancelar_Click);
            // 
            // FrmDevolucionesOrdenFab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 791);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "FrmDevolucionesOrdenFab";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmDevolucionesOrdenFab_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDevoluciones.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDevoluciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView gvDevoluciones;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private RumButtonElement rBtnDevolver;
        private RumButtonElement rbtnCancelar;
        private RumButtonElement rBtnImprimirEtiqueta;
    }
}

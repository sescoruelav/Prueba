using RumboSGA.Controles;

namespace RumboSGA.GestionAlmacen
{
    partial class FrmGenerarOrdenRecogidaManual
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
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnCrearOrdenManual = new Telerik.WinControls.UI.RadButtonElement();
            this.chkRecursoManual = new Telerik.WinControls.UI.RadCheckBoxElement();
            this.rBtnCancelar = new Telerik.WinControls.UI.RadButtonElement();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rgvLineas = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvLineas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvLineas.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radRibbonBar1.Size = new System.Drawing.Size(1129, 162);
            this.radRibbonBar1.StartButtonImage = null;
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "Orden Recogida Manual";
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radRibbonBarGroup1});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Acciones";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnCrearOrdenManual,
            this.chkRecursoManual,
            this.rBtnCancelar});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Acciones";
            // 
            // rBtnCrearOrdenManual
            // 
            this.rBtnCrearOrdenManual.Image = global::RumboSGA.Properties.Resources.edit;
            this.rBtnCrearOrdenManual.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCrearOrdenManual.Name = "rBtnCrearOrdenManual";
            this.rBtnCrearOrdenManual.Text = "Crear y Lanzar";
            this.rBtnCrearOrdenManual.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCrearOrdenManual.Click += new System.EventHandler(this.rBtnCrearOrdenManual_Click);
            // 
            // chkRecursoManual
            // 
            this.chkRecursoManual.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRecursoManual.Checked = false;
            this.chkRecursoManual.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRecursoManual.Name = "chkRecursoManual";
            this.chkRecursoManual.ReadOnly = false;
            this.chkRecursoManual.StretchVertically = false;
            this.chkRecursoManual.Text = "Recurso Manual";
            this.chkRecursoManual.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // rBtnCancelar
            // 
            this.rBtnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rBtnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnCancelar.Name = "rBtnCancelar";
            this.rBtnCancelar.Text = "Salir";
            this.rBtnCancelar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnCancelar.Click += new System.EventHandler(this.rBtnCancelar_Click);
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 833);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(1129, 26);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rgvLineas);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 162);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1129, 671);
            this.panel1.TabIndex = 2;
            // 
            // rgvLineas
            // 
            this.rgvLineas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvLineas.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rgvLineas.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.rgvLineas.Name = "rgvLineas";
            this.rgvLineas.Size = new System.Drawing.Size(1129, 671);
            this.rgvLineas.TabIndex = 13;
            // 
            // FrmGenerarOrdenRecogidaManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 859);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "FrmGenerarOrdenRecogidaManual";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Orden Recogida Manual";
            this.Load += new System.EventHandler(this.FrmGenerarOrdenRecogidaManual_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvLineas.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvLineas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadGridView rgvLineas;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup radRibbonBarGroup1;
        private Telerik.WinControls.UI.RadCheckBoxElement chkRecursoManual;
        private Telerik.WinControls.UI.RadButtonElement rBtnCrearOrdenManual;
        private Telerik.WinControls.UI.RadButtonElement rBtnCancelar;
    }
}

namespace RumboSGA.Presentation.Herramientas
{
    partial class FrmValidarPrimeraPasada
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rBtnFiltrar = new RumboSGA.RumButtonElement();
            this.rBtnValidar = new RumboSGA.RumButtonElement();
            this.radRibbonFormBehavior1 = new Telerik.WinControls.UI.RadRibbonFormBehavior();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgvIncidencias = new RumboSGA.Controles.RumGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radLabel6 = new RumboSGA.RumLabel();
            this.radLabel5 = new RumboSGA.RumLabel();
            this.radLabel4 = new RumboSGA.RumLabel();
            this.radLabel3 = new RumboSGA.RumLabel();
            this.radLabel2 = new RumboSGA.RumLabel();
            this.radLabel1 = new RumboSGA.RumLabel();
            this.Almacén = new RumboSGA.RumLabel();
            this.radComboBoxAlmacen = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.tbAceraDesde = new Telerik.WinControls.UI.RadTextBox();
            this.tbPortalDesde = new Telerik.WinControls.UI.RadTextBox();
            this.tbPisoDesde = new Telerik.WinControls.UI.RadTextBox();
            this.tbAceraHasta = new Telerik.WinControls.UI.RadTextBox();
            this.tbPortalHasta = new Telerik.WinControls.UI.RadTextBox();
            this.tbPisoHasta = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvIncidencias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvIncidencias.MasterTemplate)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Almacén)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxAlmacen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxAlmacen.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxAlmacen.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAceraDesde)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPortalDesde)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPisoDesde)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAceraHasta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPortalHasta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPisoHasta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.CommandTabs.AddRange(new Telerik.WinControls.RadItem[] {
            this.ribbonTab1});
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            this.radRibbonBar1.Size = new System.Drawing.Size(1435, 159);
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "Validar Primera Pasada";
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
            this.rBtnFiltrar,
            this.rBtnValidar});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Acciones";
            // 
            // rBtnFiltrar
            // 
            this.rBtnFiltrar.EnableFocusBorderAnimation = true;
            this.rBtnFiltrar.EnableRippleAnimation = true;
            this.rBtnFiltrar.Image = global::RumboSGA.Properties.Resources.Filter;
            this.rBtnFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnFiltrar.Name = "rBtnFiltrar";
            this.rBtnFiltrar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnFiltrar.Text = "Filtrar";
            this.rBtnFiltrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnFiltrar.ToolTipText = "Filtrar";
            this.rBtnFiltrar.Click += new System.EventHandler(this.rBtnFiltrar_Click);
            // 
            // rBtnValidar
            // 
            this.rBtnValidar.EnableFocusBorderAnimation = true;
            this.rBtnValidar.EnableRippleAnimation = true;
            this.rBtnValidar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.rBtnValidar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnValidar.Name = "rBtnValidar";
            this.rBtnValidar.RippleAnimationColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rBtnValidar.Text = "Validar";
            this.rBtnValidar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnValidar.ToolTipText = "Validar";
            this.rBtnValidar.Click += new System.EventHandler(this.rBtnValidar_Click);
            // 
            // radRibbonFormBehavior1
            // 
            this.radRibbonFormBehavior1.Form = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rgvIncidencias, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 159);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1435, 611);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rgvIncidencias
            // 
            this.rgvIncidencias.AutoScroll = true;
            this.rgvIncidencias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvIncidencias.Location = new System.Drawing.Point(3, 186);
            // 
            // 
            // 
            this.rgvIncidencias.MasterTemplate.AllowAddNewRow = false;
            this.rgvIncidencias.MasterTemplate.AllowDeleteRow = false;
            this.rgvIncidencias.MasterTemplate.AllowEditRow = false;
            this.rgvIncidencias.MasterTemplate.EnableFiltering = true;
            this.rgvIncidencias.MasterTemplate.HorizontalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.rgvIncidencias.MasterTemplate.ShowFilteringRow = false;
            this.rgvIncidencias.MasterTemplate.ShowHeaderCellButtons = true;
            this.rgvIncidencias.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.rgvIncidencias.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvIncidencias.Name = "rgvIncidencias";
            this.rgvIncidencias.ShowHeaderCellButtons = true;
            this.rgvIncidencias.Size = new System.Drawing.Size(1429, 422);
            this.rgvIncidencias.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.Controls.Add(this.radLabel6, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.radLabel5, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.radLabel4, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.radLabel3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.radLabel2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.radLabel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.Almacén, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.radComboBoxAlmacen, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbAceraDesde, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbPortalDesde, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbPisoDesde, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.tbAceraHasta, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbPortalHasta, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbPisoHasta, 3, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1429, 177);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // radLabel6
            // 
            this.radLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel6.Location = new System.Drawing.Point(717, 135);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(208, 39);
            this.radLabel6.TabIndex = 7;
            this.radLabel6.Text = "Piso Desde";
            this.radLabel6.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // radLabel5
            // 
            this.radLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel5.Location = new System.Drawing.Point(717, 91);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(208, 38);
            this.radLabel5.TabIndex = 6;
            this.radLabel5.Text = "Portal Hasta";
            this.radLabel5.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // radLabel4
            // 
            this.radLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel4.Location = new System.Drawing.Point(717, 47);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(208, 38);
            this.radLabel4.TabIndex = 5;
            this.radLabel4.Text = "Acera Hasta";
            this.radLabel4.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // radLabel3
            // 
            this.radLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel3.Location = new System.Drawing.Point(3, 135);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(208, 39);
            this.radLabel3.TabIndex = 4;
            this.radLabel3.Text = "Piso Desde";
            this.radLabel3.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // radLabel2
            // 
            this.radLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel2.Location = new System.Drawing.Point(3, 91);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(208, 38);
            this.radLabel2.TabIndex = 3;
            this.radLabel2.Text = "Portal Desde";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // radLabel1
            // 
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.Location = new System.Drawing.Point(3, 47);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(208, 38);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "Acera Desde";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // Almacén
            // 
            this.Almacén.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Almacén.Location = new System.Drawing.Point(3, 3);
            this.Almacén.Name = "Almacén";
            this.Almacén.Size = new System.Drawing.Size(208, 38);
            this.Almacén.TabIndex = 0;
            this.Almacén.Text = "Almacén";
            this.Almacén.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // radComboBoxAlmacen
            // 
            this.radComboBoxAlmacen.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // radComboBoxAlmacen.NestedRadGridView
            // 
            this.radComboBoxAlmacen.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radComboBoxAlmacen.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radComboBoxAlmacen.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radComboBoxAlmacen.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radComboBoxAlmacen.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radComboBoxAlmacen.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radComboBoxAlmacen.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radComboBoxAlmacen.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radComboBoxAlmacen.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radComboBoxAlmacen.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radComboBoxAlmacen.EditorControl.Name = "NestedRadGridView";
            this.radComboBoxAlmacen.EditorControl.ReadOnly = true;
            this.radComboBoxAlmacen.EditorControl.ShowGroupPanel = false;
            this.radComboBoxAlmacen.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radComboBoxAlmacen.EditorControl.TabIndex = 0;
            this.radComboBoxAlmacen.Location = new System.Drawing.Point(217, 3);
            this.radComboBoxAlmacen.Name = "radComboBoxAlmacen";
            this.radComboBoxAlmacen.Size = new System.Drawing.Size(494, 38);
            this.radComboBoxAlmacen.TabIndex = 8;
            this.radComboBoxAlmacen.TabStop = false;
            // 
            // tbAceraDesde
            // 
            this.tbAceraDesde.Location = new System.Drawing.Point(217, 47);
            this.tbAceraDesde.Name = "tbAceraDesde";
            this.tbAceraDesde.Size = new System.Drawing.Size(142, 24);
            this.tbAceraDesde.TabIndex = 9;
            // 
            // tbPortalDesde
            // 
            this.tbPortalDesde.Location = new System.Drawing.Point(217, 91);
            this.tbPortalDesde.Name = "tbPortalDesde";
            this.tbPortalDesde.Size = new System.Drawing.Size(142, 24);
            this.tbPortalDesde.TabIndex = 10;
            // 
            // tbPisoDesde
            // 
            this.tbPisoDesde.Location = new System.Drawing.Point(217, 135);
            this.tbPisoDesde.Name = "tbPisoDesde";
            this.tbPisoDesde.Size = new System.Drawing.Size(142, 24);
            this.tbPisoDesde.TabIndex = 11;
            // 
            // tbAceraHasta
            // 
            this.tbAceraHasta.Location = new System.Drawing.Point(931, 47);
            this.tbAceraHasta.Name = "tbAceraHasta";
            this.tbAceraHasta.Size = new System.Drawing.Size(142, 24);
            this.tbAceraHasta.TabIndex = 12;
            // 
            // tbPortalHasta
            // 
            this.tbPortalHasta.Location = new System.Drawing.Point(931, 91);
            this.tbPortalHasta.Name = "tbPortalHasta";
            this.tbPortalHasta.Size = new System.Drawing.Size(142, 24);
            this.tbPortalHasta.TabIndex = 13;
            // 
            // tbPisoHasta
            // 
            this.tbPisoHasta.Location = new System.Drawing.Point(931, 135);
            this.tbPisoHasta.Name = "tbPisoHasta";
            this.tbPisoHasta.Size = new System.Drawing.Size(142, 24);
            this.tbPisoHasta.TabIndex = 14;
            // 
            // FrmValidarPrimeraPasada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1435, 770);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.FormBehavior = this.radRibbonFormBehavior1;
            this.IconScaling = Telerik.WinControls.Enumerations.ImageScaling.None;
            this.Name = "FrmValidarPrimeraPasada";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Validar Primera Pasada";
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgvIncidencias.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvIncidencias)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Almacén)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxAlmacen.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxAlmacen.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxAlmacen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAceraDesde)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPortalDesde)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPisoDesde)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAceraHasta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPortalHasta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPisoHasta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RadRibbonFormBehavior radRibbonFormBehavior1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private RumboSGA.RumButtonElement rBtnFiltrar;
        private RumboSGA.RumButtonElement rBtnValidar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RumboSGA.RumLabel radLabel3;
        private RumboSGA.RumLabel radLabel2;
        private RumboSGA.RumLabel radLabel1;
        private RumboSGA.RumLabel Almacén;
        private RumboSGA.RumLabel radLabel6;
        private RumboSGA.RumLabel radLabel5;
        private RumboSGA.RumLabel radLabel4;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radComboBoxAlmacen;
        private Telerik.WinControls.UI.RadTextBox tbAceraDesde;
        private Telerik.WinControls.UI.RadTextBox tbPortalDesde;
        private Telerik.WinControls.UI.RadTextBox tbPisoDesde;
        private Telerik.WinControls.UI.RadTextBox tbAceraHasta;
        private Telerik.WinControls.UI.RadTextBox tbPortalHasta;
        private Telerik.WinControls.UI.RadTextBox tbPisoHasta;
        private RumboSGA.Controles.RumGridView rgvIncidencias;
    }
}

using System;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Properties;

namespace RumboSGA.Presentation.Herramientas
{
    partial class OperariosForm
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
            this.radRibbonBarGroup2 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.radButtonElement4 = new RumboSGA.RumButtonElement();
            this.radButtonElement5 = new RumboSGA.RumButtonElement();
            this.radButtonElement6 = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup3 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.radButtonElement7 = new RumboSGA.RumButtonElement();
            this.radButtonElement8 = new RumboSGA.RumButtonElement();
            this.radButtonElement9 = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.radButtonElement1 = new RumboSGA.RumButtonElement();
            this.radButtonElement2 = new RumboSGA.RumButtonElement();
            this.radButtonElement3 = new RumboSGA.RumButtonElement();
            this.radButtonElement14 = new RumboSGA.RumButtonElement();
            this.configBarGroup = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radButtonElement12 = new RumboSGA.RumButtonElement();
            this.radButtonElement13 = new RumboSGA.RumButtonElement();
            this.temasMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.guardarButton = new RumboSGA.Controles.RumMenuItem();
            this.cargarButton = new RumboSGA.Controles.RumMenuItem();
            this.editColumns = new RumboSGA.Controles.RumMenuItem();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.configButton = new RumboSGA.Controles.RumDropDownButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
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
            this.radRibbonBarGroup2,
            this.radRibbonBarGroup3,
            this.radRibbonBarGroup1,
            this.configBarGroup});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Acciones";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // radRibbonBarGroup2
            // 
            this.radRibbonBarGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElement4,
            this.radButtonElement5,
            this.radButtonElement6});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = global::RumboSGA.Properties.strings.Modificaciones;
            // 
            // radButtonElement4
            // 
            this.radButtonElement4.Image = global::RumboSGA.Properties.Resources.Add;
            this.radButtonElement4.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement4.Name = "radButtonElement4";
            this.radButtonElement4.Text = "Nuevo";
            this.radButtonElement4.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement4.ToolTipText = "Nuevo";
            this.radButtonElement4.Click += new System.EventHandler(this.NuevoButton_Click);
            // 
            // radButtonElement5
            // 
            this.radButtonElement5.Image = global::RumboSGA.Properties.Resources.Delete;
            this.radButtonElement5.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement5.Name = "radButtonElement5";
            this.radButtonElement5.Text = "Borrar";
            this.radButtonElement5.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement5.ToolTipText = "Borrar";
            this.radButtonElement5.Click += new System.EventHandler(this.EliminarButton_Click);
            // 
            // radButtonElement6
            // 
            this.radButtonElement6.Image = global::RumboSGA.Properties.Resources.copy;
            this.radButtonElement6.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement6.Name = "radButtonElement6";
            this.radButtonElement6.Text = "Clonar";
            this.radButtonElement6.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement6.ToolTipText = "Clonar";
            this.radButtonElement6.Click += new System.EventHandler(this.ClonarButton_Click);
            // 
            // radRibbonBarGroup3
            // 
            this.radRibbonBarGroup3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElement7,
            this.radButtonElement8,
            this.radButtonElement9});
            this.radRibbonBarGroup3.Name = "radRibbonBarGroup3";
            this.radRibbonBarGroup3.Text = global::RumboSGA.Properties.strings.Ver;
            // 
            // radButtonElement7
            // 
            this.radButtonElement7.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.radButtonElement7.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement7.Name = "radButtonElement7";
            this.radButtonElement7.Text = "Quitar Filtros";
            this.radButtonElement7.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement7.ToolTipText = "Quitar Filtros";
            this.radButtonElement7.Click += new System.EventHandler(this.QuitarFiltros_Click);
            // 
            // radButtonElement8
            // 
            this.radButtonElement8.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.radButtonElement8.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement8.Name = "radButtonElement8";
            this.radButtonElement8.Text = "Exportar a Excel";
            this.radButtonElement8.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement8.ToolTipText = "Exportar a Excel";
            this.radButtonElement8.Click += new System.EventHandler(this.ExportarExcel_Click);
            // 
            // radButtonElement9
            // 
            this.radButtonElement9.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.radButtonElement9.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement9.Name = "radButtonElement9";
            this.radButtonElement9.Text = "Refrescar";
            this.radButtonElement9.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement9.ToolTipText = "Refrescar";
            this.radButtonElement9.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radButtonElement1,
            this.radButtonElement2,
            this.radButtonElement3,
            this.radButtonElement14});
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = global::RumboSGA.Properties.strings.Operario;
            // 
            // temasMenuItem
            // 
            this.temasMenuItem.Name = "temasMenuItem";
            this.temasMenuItem.Text = "Cargar Temas";
            // 
            // guardarButton
            // 
            this.guardarButton.Name = "guardarButton";
            this.guardarButton.Text = Lenguaje.traduce("Guardar Layout");
            this.guardarButton.Click += new System.EventHandler(this.SaveItem_Click);
            // 
            // cargarButton
            // 
            this.cargarButton.Name = "cargarButton";
            this.cargarButton.Text = Lenguaje.traduce("Cargar Layout");
            this.cargarButton.Click += new System.EventHandler(this.LoadItem_Click);
            // 
            // editColumns
            // 
            this.editColumns.Name = "editColumns";
            this.editColumns.Text = Lenguaje.traduce("Editar columnas");
            this.editColumns.Click += new System.EventHandler(this.ItemColumnas_Click);

            // 
            // radButtonElement1
            // 
            this.radButtonElement1.Image = global::RumboSGA.Properties.Resources.add_group;
            this.radButtonElement1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement1.Name = "radButtonElement1";
            this.radButtonElement1.Text = "Añadir Grupo";
            this.radButtonElement1.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement1.ToolTipText = "Añadir Grupo";
            this.radButtonElement1.Click += new System.EventHandler(this.btnAgregarGrupo_Click);
            // 
            // radButtonElement2
            // 
            this.radButtonElement2.Image = global::RumboSGA.Properties.Resources.add_user;
            this.radButtonElement2.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement2.Name = "radButtonElement2";
            this.radButtonElement2.Text = "Añadir Usuario";
            this.radButtonElement2.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement2.ToolTipText = "Añadir Usuario";
            this.radButtonElement2.Click += new System.EventHandler(this.btnAgregarUsuario_Click);
            // 
            // radButtonElement3
            // 
            this.radButtonElement3.Image = global::RumboSGA.Properties.Resources.password;
            this.radButtonElement3.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement3.Name = "radButtonElement3";
            this.radButtonElement3.Text = "Cambiar Clave";
            this.radButtonElement3.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement3.ToolTipText = "Cambiar Clave";
            this.radButtonElement3.UseCompatibleTextRendering = true;
            this.radButtonElement3.Click += new System.EventHandler(this.btnCambiarPassword_Click);
            // 
            // radButtonElement14
            // 
            this.radButtonElement14.Image = global::RumboSGA.Properties.Resources.cambiar_grupo;
            this.radButtonElement14.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement14.Name = "radButtonElement14";
            this.radButtonElement14.Text = "Cambiar Grupo";
            this.radButtonElement14.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement14.ToolTipText = "Cambiar Grupo";
            this.radButtonElement14.UseCompatibleTextRendering = true;
            this.radButtonElement14.Click += new System.EventHandler(this.btnEditarGrupo_Click);
            // 
            // configBarGroup
            // 
            this.configBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.configButton});
            this.configBarGroup.Name = "configBarGroup";
            this.configBarGroup.Text = "";
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 380F));
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
            // radButtonElement12
            // 
            this.radButtonElement12.Image = global::RumboSGA.Properties.Resources.copy;
            this.radButtonElement12.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement12.Name = "radButtonElement12";
            this.radButtonElement12.ShowBorder = false;
            this.radButtonElement12.Text = "Clonar";
            this.radButtonElement12.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement12.ToolTipText = "Clonar";
            this.radButtonElement12.UseCompatibleTextRendering = false;
            // 
            // radButtonElement13
            // 
            this.radButtonElement13.Image = global::RumboSGA.Properties.Resources.copy;
            this.radButtonElement13.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radButtonElement13.Name = "radButtonElement13";
            this.radButtonElement13.ShowBorder = false;
            this.radButtonElement13.Text = "Clonar";
            this.radButtonElement13.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radButtonElement13.ToolTipText = "Clonar";
            this.radButtonElement13.UseCompatibleTextRendering = false;
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
            // configButton
            // 
            this.configButton.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.configButton.AutoSize = false;
            this.configButton.Bounds = new System.Drawing.Rectangle(0, 0, 80, 62);
            this.configButton.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.configButton.ExpandArrowButton = false;
            this.configButton.Image = global::RumboSGA.Properties.Resources.Administration;
            this.configButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.configButton.Name = "configButton";
            this.configButton.Text = "";
            this.configButton.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.configButton.UseCompatibleTextRendering = false;
            this.configButton.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.temasMenuItem,
            this.guardarButton,
            this.cargarButton,
            this.editColumns});
            // 
            // OperariosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 542);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "OperariosForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumRibbonBarGroup vistaBarGroup;
        public System.Windows.Forms.TabPage tabPageGeneral { get; set; }
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RumButtonElement radButtonElement12;
        private RumButtonElement radButtonElement13;
        private RumButtonElement radButtonElement14;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup2;
        private RumButtonElement radButtonElement1;
        private RumButtonElement radButtonElement2;
        private RumButtonElement radButtonElement3;
        private RumButtonElement radButtonElement4;
        private RumButtonElement radButtonElement5;
        private RumButtonElement radButtonElement6;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup3;
        private RumButtonElement radButtonElement7;
        private RumButtonElement radButtonElement8;
        private RumButtonElement radButtonElement9;
        private Telerik.WinControls.UI.RadRibbonBarGroup configBarGroup;
        private RumDropDownButtonElement configButton;
        private RumMenuItem guardarButton;
        private RumMenuItem cargarButton;
        private RumMenuItem editColumns;
        private RumMenuItem temasMenuItem;
    }
}

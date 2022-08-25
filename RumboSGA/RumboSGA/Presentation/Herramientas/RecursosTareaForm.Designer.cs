using System;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;

namespace RumboSGA.Presentation.Herramientas
{
    partial class RecursosTareaForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.radRibbonBarGroup2 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rumButtonElement4 = new RumboSGA.RumButtonElement();
            this.rumButtonElement5 = new RumboSGA.RumButtonElement();
            this.rumButtonElement6 = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup3 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rumButtonElement7 = new RumboSGA.RumButtonElement();
            this.rumButtonElement8 = new RumboSGA.RumButtonElement();
            this.rumButtonElement9 = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup4 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rumButtonElementTipoTarea = new RumboSGA.RumButtonElement();
            this.rumButtonElementAddZona = new RumboSGA.RumButtonElement();
            this.rumButtonElementLiberar = new RumboSGA.RumButtonElement();
            this.rumButtonElementHistoria = new RumboSGA.RumButtonElement();
            this.rumButtonElement12 = new RumboSGA.RumButtonElement();
            this.rumButtonElement13 = new RumboSGA.RumButtonElement();
            this.rumButtonElement1 = new RumboSGA.RumButtonElement();
            this.rumButtonElement2 = new RumboSGA.RumButtonElement();
            this.rumButtonElement3 = new RumboSGA.RumButtonElement();
            this.rumButtonElement10 = new RumboSGA.RumButtonElement();
            this.rumButtonElement11 = new RumboSGA.RumButtonElement();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rumButtonElement14 = new RumboSGA.RumButtonElement();
            this.rumButtonElement15 = new RumboSGA.RumButtonElement();
            this.rumButtonElement16 = new RumboSGA.RumButtonElement();
            this.rumButtonElement17 = new RumboSGA.RumButtonElement();
            this.rumButtonElement18 = new RumboSGA.RumButtonElement();
            this.radRibbonBarGroup1 = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.configButton = new RumDropDownButtonElement();
            this.temasMenuItem = new RumboSGA.Controles.RumMenuItem();
            this.guardarButton = new RumboSGA.Controles.RumMenuItem();
            this.cargarButton = new RumboSGA.Controles.RumMenuItem();
            this.editColumns = new RumboSGA.Controles.RumMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
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
            this.radRibbonBarGroup4,
            this.radRibbonBarGroup1});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Acciones";
            this.ribbonTab1.UseMnemonic = false;
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
            // radRibbonBarGroup2
            // 
            this.radRibbonBarGroup2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rumButtonElement4,
            this.rumButtonElement5,
            this.rumButtonElement6});
            this.radRibbonBarGroup2.Name = "radRibbonBarGroup2";
            this.radRibbonBarGroup2.Text = "Modificaciones";
            // 
            // rumButtonElement4
            // 
            this.rumButtonElement4.Image = global::RumboSGA.Properties.Resources.Add;
            this.rumButtonElement4.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement4.Name = "rumButtonElement4";
            this.rumButtonElement4.Text = "Nuevo";
            this.rumButtonElement4.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement4.Click += new System.EventHandler(this.NuevoButton_Click);
            // 
            // rumButtonElement5
            // 
            this.rumButtonElement5.Image = global::RumboSGA.Properties.Resources.Delete;
            this.rumButtonElement5.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement5.Name = "rumButtonElement5";
            this.rumButtonElement5.Text = "Borrar";
            this.rumButtonElement5.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement5.Click += new System.EventHandler(this.EliminarButton_Click);
            // 
            // rumButtonElement6
            // 
            this.rumButtonElement6.Image = global::RumboSGA.Properties.Resources.copy;
            this.rumButtonElement6.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement6.Name = "rumButtonElement6";
            this.rumButtonElement6.Text = "Clonar";
            this.rumButtonElement6.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement6.Click += new System.EventHandler(this.ClonarButton_Click);
            // 
            // radRibbonBarGroup3
            // 
            this.radRibbonBarGroup3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rumButtonElement7,
            this.rumButtonElement8,
            this.rumButtonElement9});
            this.radRibbonBarGroup3.Name = "radRibbonBarGroup3";
            this.radRibbonBarGroup3.Text = "Ver";
            // 
            // rumButtonElement7
            // 
            this.rumButtonElement7.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.rumButtonElement7.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement7.Name = "rumButtonElement7";
            this.rumButtonElement7.Text = "Quitar Filtros";
            this.rumButtonElement7.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement7.Click += new System.EventHandler(this.QuitarFiltros_Click);
            // 
            // rumButtonElement8
            // 
            this.rumButtonElement8.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.rumButtonElement8.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement8.Name = "rumButtonElement8";
            this.rumButtonElement8.Text = "Exportar a Excel";
            this.rumButtonElement8.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement8.Click += new System.EventHandler(this.ExportarExcel_Click);
            // 
            // rumButtonElement9
            // 
            this.rumButtonElement9.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rumButtonElement9.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement9.Name = "rumButtonElement9";
            this.rumButtonElement9.Text = "Refrescar";
            this.rumButtonElement9.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement9.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // radRibbonBarGroup4
            // 
            this.radRibbonBarGroup4.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rumButtonElementTipoTarea,
            this.rumButtonElementAddZona,
            this.rumButtonElementLiberar,
            this.rumButtonElementHistoria});
            this.radRibbonBarGroup4.Name = "radRibbonBarGroup4";
            this.radRibbonBarGroup4.Text = "Recursos Tarea";
            // 
            // rumButtonElementTipoTarea
            // 
            this.rumButtonElementTipoTarea.Image = global::RumboSGA.Properties.Resources.Task;
            this.rumButtonElementTipoTarea.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElementTipoTarea.Name = "rumButtonElementTipoTarea";
            this.rumButtonElementTipoTarea.Text = "Nuevo Tipo Tarea";
            this.rumButtonElementTipoTarea.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElementTipoTarea.Click += new System.EventHandler(this.RumButtonElementTipoTarea_Click);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementTipoTarea.GetChildAt(1).GetChildAt(1))).StretchVertically = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementTipoTarea.GetChildAt(1).GetChildAt(1))).TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementTipoTarea.GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementTipoTarea.GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.BottomCenter;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementTipoTarea.GetChildAt(1).GetChildAt(1))).RightToLeft = false;
            // 
            // rumButtonElementAddZona
            // 
            this.rumButtonElementAddZona.Image = global::RumboSGA.Properties.Resources.Add1;
            this.rumButtonElementAddZona.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElementAddZona.Name = "rumButtonElementAddZona";
            this.rumButtonElementAddZona.Text = "Nueva Zona";
            this.rumButtonElementAddZona.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElementAddZona.Click += new System.EventHandler(this.RumButtonElementAddZona_Click);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementAddZona.GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementAddZona.GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // rumButtonElementLiberar
            // 
            this.rumButtonElementLiberar.Name = "rumButtonElementLiberar";
            this.rumButtonElementLiberar.Text = "Liberar";
            this.rumButtonElementLiberar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementLiberar.GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementLiberar.GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // rumButtonElementHistoria
            // 
            this.rumButtonElementHistoria.Name = "rumButtonElementHistoria";
            this.rumButtonElementHistoria.Text = "Historia";
            this.rumButtonElementHistoria.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElementHistoria.Click += new System.EventHandler(this.RumButtonElementHistoria_Click);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementHistoria.GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.rumButtonElementHistoria.GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // rumButtonElement12
            // 
            this.rumButtonElement12.Image = global::RumboSGA.Properties.Resources.copy;
            this.rumButtonElement12.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement12.Name = "rumButtonElement12";
            this.rumButtonElement12.ShowBorder = false;
            this.rumButtonElement12.Text = "Clonar";
            this.rumButtonElement12.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement12.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement13
            // 
            this.rumButtonElement13.Image = global::RumboSGA.Properties.Resources.copy;
            this.rumButtonElement13.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement13.Name = "rumButtonElement13";
            this.rumButtonElement13.ShowBorder = false;
            this.rumButtonElement13.Text = "Clonar";
            this.rumButtonElement13.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement13.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement1
            // 
            this.rumButtonElement1.Image = global::RumboSGA.Properties.Resources.add_group;
            this.rumButtonElement1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement1.Name = "rumButtonElement1";
            this.rumButtonElement1.ShowBorder = false;
            this.rumButtonElement1.Text = "Añadir Grupo";
            this.rumButtonElement1.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement1.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement2
            // 
            this.rumButtonElement2.Image = global::RumboSGA.Properties.Resources.add_user;
            this.rumButtonElement2.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement2.Name = "rumButtonElement2";
            this.rumButtonElement2.ShowBorder = false;
            this.rumButtonElement2.Text = "Añadir Usuario";
            this.rumButtonElement2.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement2.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement3
            // 
            this.rumButtonElement3.Image = global::RumboSGA.Properties.Resources.password;
            this.rumButtonElement3.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement3.Name = "rumButtonElement3";
            this.rumButtonElement3.ShowBorder = false;
            this.rumButtonElement3.Text = "Cambiar Clave";
            this.rumButtonElement3.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement3.UseCompatibleTextRendering = true;
            // 
            // rumButtonElement10
            // 
            this.rumButtonElement10.Image = global::RumboSGA.Properties.Resources.Add;
            this.rumButtonElement10.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement10.Name = "rumButtonElement10";
            this.rumButtonElement10.ShowBorder = false;
            this.rumButtonElement10.Text = "Nuevo";
            this.rumButtonElement10.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement10.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement11
            // 
            this.rumButtonElement11.Image = global::RumboSGA.Properties.Resources.Add;
            this.rumButtonElement11.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement11.Name = "rumButtonElement11";
            this.rumButtonElement11.ShowBorder = false;
            this.rumButtonElement11.Text = "Nuevo";
            this.rumButtonElement11.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement11.UseCompatibleTextRendering = false;
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
            // rumButtonElement14
            // 
            this.rumButtonElement14.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rumButtonElement14.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement14.Name = "rumButtonElement14";
            this.rumButtonElement14.ShowBorder = false;
            this.rumButtonElement14.Text = "Refrescar";
            this.rumButtonElement14.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement14.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement15
            // 
            this.rumButtonElement15.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rumButtonElement15.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement15.Name = "rumButtonElement15";
            this.rumButtonElement15.ShowBorder = false;
            this.rumButtonElement15.Text = "Refrescar";
            this.rumButtonElement15.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement15.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement16
            // 
            this.rumButtonElement16.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rumButtonElement16.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement16.Name = "rumButtonElement16";
            this.rumButtonElement16.ShowBorder = false;
            this.rumButtonElement16.Text = "Refrescar";
            this.rumButtonElement16.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement16.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement17
            // 
            this.rumButtonElement17.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rumButtonElement17.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement17.Name = "rumButtonElement17";
            this.rumButtonElement17.ShowBorder = false;
            this.rumButtonElement17.Text = "Refrescar";
            this.rumButtonElement17.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement17.UseCompatibleTextRendering = false;
            // 
            // rumButtonElement18
            // 
            this.rumButtonElement18.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.rumButtonElement18.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButtonElement18.Name = "rumButtonElement18";
            this.rumButtonElement18.ShowBorder = false;
            this.rumButtonElement18.Text = "Refrescar";
            this.rumButtonElement18.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonElement18.UseCompatibleTextRendering = false;
            // 
            // radRibbonBarGroup1
            // 
            this.radRibbonBarGroup1.Name = "radRibbonBarGroup1";
            this.radRibbonBarGroup1.Text = "Herramientas";
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
            // RecursosTareaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 542);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radRibbonBar1);
            this.MainMenuStrip = null;
            this.Name = "RecursosTareaForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
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
        private RumButtonElement rumButtonElement12;
        private RumButtonElement rumButtonElement13;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup2;
        private RumButtonElement rumButtonElement4;
        private RumButtonElement rumButtonElement5;
        private RumButtonElement rumButtonElement6;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup3;
        private RumButtonElement rumButtonElement7;
        private RumButtonElement rumButtonElement8;
        private RumButtonElement rumButtonElement9;
        private RumButtonElement rumButtonElement1;
        private RumButtonElement rumButtonElement2;
        private RumButtonElement rumButtonElement3;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup4;
        private RumButtonElement rumButtonElementTipoTarea;
        private RumButtonElement rumButtonElement10;
        private RumButtonElement rumButtonElement11;
        private RumButtonElement rumButtonElementAddZona;
        private RumButtonElement rumButtonElementLiberar;
        private RumButtonElement rumButtonElementHistoria;
        private RumButtonElement rumButtonElement14;
        private RumButtonElement rumButtonElement15;
        private RumButtonElement rumButtonElement16;
        private RumButtonElement rumButtonElement17;
        private RumButtonElement rumButtonElement18;
        private Telerik.WinControls.UI.RadRibbonBarGroup radRibbonBarGroup1;
        private RumDropDownButtonElement configButton;

        private RumMenuItem guardarButton;
        private RumMenuItem cargarButton;
        private RumMenuItem editColumns;
        private RumMenuItem temasMenuItem;
    }
}

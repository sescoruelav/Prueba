using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGAManager;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas
{
    partial class Reposicion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reposicion));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            this.ribbonTab1 = new Telerik.WinControls.UI.RibbonTab();
            this.groupAcciones = new RumboSGA.Controles.RumRibbonBarGroup();
            this.rBtnSoloConflictos = new RumboSGA.RumButtonElement();
            this.rBtnGenerarReposiciones = new RumboSGA.RumButtonElement();
            this.rBtnAsignarRecurso = new RumboSGA.RumButtonElement();
            this.vistaBarGroup = new RumboSGA.Controles.RumRibbonBarGroup();
            this.exportarButton = new RumboSGA.RumButtonElement();
            this.rBtnVistaPedidosFiltrar = new RumboSGA.RumButtonElement();
            this.btnVistaPedidosQuitarFiltro = new RumboSGA.RumButtonElement();
            this.refreshButton = new RumboSGA.RumButtonElement();
            this.rrbgConfiguracion = new Telerik.WinControls.UI.RadRibbonBarGroup();
            this.rDDButtonConfiguracion = new RumboSGA.Controles.RumDropDownButtonElement();
            this.itm_TemasMenu = new RumboSGA.Controles.RumMenuItem();
            this.itm_GuardarMenu = new RumboSGA.Controles.RumMenuItem();
            this.itm_CargarMenu = new RumboSGA.Controles.RumMenuItem();
            this.itm_EditarColumnas = new RumboSGA.Controles.RumMenuItem();
            this.tlPanelPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.borrarButton = new RumboSGA.RumButtonElement();
            this.operarioButton = new RumboSGA.RumButtonElement();
            this.pedidosButton = new RumboSGA.RumButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.radRibbonBar1.SimplifiedHeight = 100;
            this.radRibbonBar1.Size = new System.Drawing.Size(964, 163);
            this.radRibbonBar1.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radRibbonBar1.StartButtonImage")));
            this.radRibbonBar1.TabIndex = 1;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).SimplifiedHeight = 100;
            ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.IsSelected = true;
            this.ribbonTab1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.groupAcciones,
            this.vistaBarGroup,
            this.rrbgConfiguracion});
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "";
            this.ribbonTab1.UseMnemonic = false;
            // 
            // groupAcciones
            // 
            this.groupAcciones.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rBtnSoloConflictos,
            this.rBtnGenerarReposiciones,
            this.rBtnAsignarRecurso});
            this.groupAcciones.Name = "groupAcciones";
            this.groupAcciones.Text = "Acciones";
            // 
            // rBtnSoloConflictos
            // 
            this.rBtnSoloConflictos.Image = global::RumboSGA.Properties.Resources.Overdue;
            this.rBtnSoloConflictos.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnSoloConflictos.Name = "rBtnSoloConflictos";
            this.rBtnSoloConflictos.Text = "Solo Conflictos";
            this.rBtnSoloConflictos.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnSoloConflictos.ToolTipText = "Solo Conflictos";
            this.rBtnSoloConflictos.Click += new System.EventHandler(this.rBtnSoloConflictos_Click);
            // 
            // rBtnGenerarReposiciones
            // 
            this.rBtnGenerarReposiciones.Image = global::RumboSGA.Properties.Resources.Add1;
            this.rBtnGenerarReposiciones.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnGenerarReposiciones.Name = "rBtnGenerarReposiciones";
            this.rBtnGenerarReposiciones.Text = "Generar Reposiciones";
            this.rBtnGenerarReposiciones.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnGenerarReposiciones.ToolTipText = "Generar Reposiciones";
            this.rBtnGenerarReposiciones.Click += new System.EventHandler(this.rBtnGenerarReposiciones_Click);
            // 
            // rBtnAsignarRecurso
            // 
            this.rBtnAsignarRecurso.Image = global::RumboSGA.Properties.Resources.Add3;
            this.rBtnAsignarRecurso.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnAsignarRecurso.Name = "rBtnAsignarRecurso";
            this.rBtnAsignarRecurso.Text = "Asignar Recurso";
            this.rBtnAsignarRecurso.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnAsignarRecurso.ToolTipText = "Asignar Recurso";
            this.rBtnAsignarRecurso.Click += new System.EventHandler(this.rBtnAsignarRecurso_Click);
            // 
            // vistaBarGroup
            // 
            this.vistaBarGroup.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.exportarButton,
            this.rBtnVistaPedidosFiltrar,
            this.btnVistaPedidosQuitarFiltro,
            this.refreshButton});
            this.vistaBarGroup.Name = "vistaBarGroup";
            this.vistaBarGroup.Text = "";
            // 
            // exportarButton
            // 
            this.exportarButton.Image = global::RumboSGA.Properties.Resources.ExportToExcel;
            this.exportarButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.exportarButton.Name = "exportarButton";
            this.exportarButton.Text = "";
            this.exportarButton.Click += new System.EventHandler(this.btnExportacion_Click);
            // 
            // rBtnVistaPedidosFiltrar
            // 
            this.rBtnVistaPedidosFiltrar.Image = global::RumboSGA.Properties.Resources.Filter;
            this.rBtnVistaPedidosFiltrar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rBtnVistaPedidosFiltrar.Name = "rBtnVistaPedidosFiltrar";
            this.rBtnVistaPedidosFiltrar.Text = "";
            this.rBtnVistaPedidosFiltrar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rBtnVistaPedidosFiltrar.Click += new System.EventHandler(this.btnVistaPedidosFiltrar_Click);
            // 
            // btnVistaPedidosQuitarFiltro
            // 
            this.btnVistaPedidosQuitarFiltro.Image = global::RumboSGA.Properties.Resources.ClearFilter;
            this.btnVistaPedidosQuitarFiltro.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVistaPedidosQuitarFiltro.Name = "btnVistaPedidosQuitarFiltro";
            this.btnVistaPedidosQuitarFiltro.Text = "";
            this.btnVistaPedidosQuitarFiltro.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVistaPedidosQuitarFiltro.Click += new System.EventHandler(this.btnVistaPedidosQuitarFiltro_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.refreshButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Text = "Actualizar";
            this.refreshButton.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.refreshButton.ToolTipText = "Actualizar";
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // rrbgConfiguracion
            // 
            this.rrbgConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rDDButtonConfiguracion});
            this.rrbgConfiguracion.Name = "rrbgConfiguracion";
            this.rrbgConfiguracion.Text = "Configuración";
            // 
            // rDDButtonConfiguracion
            // 
            this.rDDButtonConfiguracion.ArrowButtonMinSize = new System.Drawing.Size(12, 12);
            this.rDDButtonConfiguracion.AutoSize = false;
            this.rDDButtonConfiguracion.Bounds = new System.Drawing.Rectangle(0, 0, 53, 62);
            this.rDDButtonConfiguracion.DropDownDirection = Telerik.WinControls.UI.RadDirection.Down;
            this.rDDButtonConfiguracion.EnableBorderHighlight = true;
            this.rDDButtonConfiguracion.EnableFocusBorder = true;
            this.rDDButtonConfiguracion.EnableFocusBorderAnimation = true;
            this.rDDButtonConfiguracion.EnableHighlight = true;
            this.rDDButtonConfiguracion.EnableRippleAnimation = true;
            this.rDDButtonConfiguracion.ExpandArrowButton = false;
            this.rDDButtonConfiguracion.Image = global::RumboSGA.Properties.Resources.Administration;
            this.rDDButtonConfiguracion.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rDDButtonConfiguracion.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.itm_TemasMenu,
            this.itm_GuardarMenu,
            this.itm_CargarMenu,
            this.itm_EditarColumnas});
            this.rDDButtonConfiguracion.Name = "rDDButtonConfiguracion";
            this.rDDButtonConfiguracion.Text = "";
            // 
            // itm_TemasMenu
            // 
            this.itm_TemasMenu.Name = "itm_TemasMenu";
            this.itm_TemasMenu.Text = "Temas";
            // 
            // itm_GuardarMenu
            // 
            this.itm_GuardarMenu.Name = "itm_GuardarMenu";
            this.itm_GuardarMenu.Text = "Guardar Estilo";
            this.itm_GuardarMenu.Click += new System.EventHandler(this.menuItemGuardar_Click);
            // 
            // itm_CargarMenu
            // 
            this.itm_CargarMenu.Name = "itm_CargarMenu";
            this.itm_CargarMenu.Text = "Cargar Estilo";
            this.itm_CargarMenu.Click += new System.EventHandler(this.menuItemCargar_Click);
            // 
            // itm_EditarColumnas
            // 
            this.itm_EditarColumnas.Name = "itm_EditarColumnas";
            this.itm_EditarColumnas.Text = "Editar Columnas";
            this.itm_EditarColumnas.Click += new System.EventHandler(this.itm_EditarColumnas_Click);
            // 
            // tlPanelPrincipal
            // 
            this.tlPanelPrincipal.ColumnCount = 1;
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlPanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlPanelPrincipal.Location = new System.Drawing.Point(0, 163);
            this.tlPanelPrincipal.Name = "tlPanelPrincipal";
            this.tlPanelPrincipal.RowCount = 1;
            this.tlPanelPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanelPrincipal.Size = new System.Drawing.Size(964, 379);
            this.tlPanelPrincipal.TabIndex = 2;
            this.tlPanelPrincipal.Paint += new System.Windows.Forms.PaintEventHandler(this.tlPanelPrincipal_Paint);
            // 
            // borrarButton
            // 
            this.borrarButton.Name = "borrarButton";
            // 
            // operarioButton
            // 
            this.operarioButton.Name = "operarioButton";
            // 
            // pedidosButton
            // 
            this.pedidosButton.Name = "pedidosButton";
            // 
            // Reposicion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 542);
            this.Controls.Add(this.tlPanelPrincipal);
            this.Controls.Add(this.radRibbonBar1);
            this.Name = "Reposicion";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
        #endregion
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
        private Telerik.WinControls.UI.RibbonTab ribbonTab1;
        private RumRibbonBarGroup vistaBarGroup;
        private System.Windows.Forms.TableLayoutPanel tlPanelPrincipal;
        private RumButtonElement exportarButton;
        private RumButtonElement rBtnVistaPedidosFiltrar;
        private RumButtonElement btnVistaPedidosQuitarFiltro;
        private RumButtonElement refreshButton;
        private RumRibbonBarGroup groupAcciones;
        private RumButtonElement rBtnSoloConflictos;
        private RumButtonElement rBtnGenerarReposiciones;
        private RumButtonElement rBtnAsignarRecurso;
        private RumButtonElement borrarButton;
        private RumButtonElement operarioButton;
        private RumButtonElement pedidosButton;
        private Telerik.WinControls.UI.RadRibbonBarGroup rrbgConfiguracion;
        private Controles.RumDropDownButtonElement rDDButtonConfiguracion;
        private RumboSGA.Controles.RumMenuItem itm_TemasMenu;
        private RumboSGA.Controles.RumMenuItem itm_GuardarMenu;
        private RumboSGA.Controles.RumMenuItem itm_CargarMenu;
        private RumboSGA.Controles.RumMenuItem itm_EditarColumnas;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

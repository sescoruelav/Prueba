
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls
{
    partial class ArticulosBaseGridControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.virtualGrid = new Telerik.WinControls.UI.RadVirtualGrid();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.PanelContenedor = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tableGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.tabUbicacion = new System.Windows.Forms.TabPage();
            this.tableUbicacion = new System.Windows.Forms.TableLayoutPanel();
            this.tabUbicacionUnidades = new System.Windows.Forms.TabPage();
            this.tableUbicacionUD = new System.Windows.Forms.TableLayoutPanel();
            this.tabSKU = new System.Windows.Forms.TabPage();
            this.tableSKU = new System.Windows.Forms.TableLayoutPanel();
            this.tabMedidas = new System.Windows.Forms.TabPage();
            this.tableMedidas = new System.Windows.Forms.TableLayoutPanel();
            this.lblCantidad = new RumboSGA.RumLabel();
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).BeginInit();
            this.PanelContenedor.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabUbicacion.SuspendLayout();
            this.tabUbicacionUnidades.SuspendLayout();
            this.tabSKU.SuspendLayout();
            this.tabMedidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblCantidad)).BeginInit();
            this.SuspendLayout();
            // 
            // virtualGrid
            // 
            this.virtualGrid.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.virtualGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualGrid.Location = new System.Drawing.Point(3, 58);
            this.virtualGrid.Name = "virtualGrid";
            // 
            // 
            // 
            this.virtualGrid.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 58, 300, 187);
            this.virtualGrid.Size = new System.Drawing.Size(778, 501);
            this.virtualGrid.TabIndex = 0;
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "";
            // 
            // PanelContenedor
            // 
            this.PanelContenedor.ColumnCount = 1;
            this.PanelContenedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PanelContenedor.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.PanelContenedor.Controls.Add(this.tabControl1, 0, 1);
            this.PanelContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelContenedor.Location = new System.Drawing.Point(0, 0);
            this.PanelContenedor.Name = "PanelContenedor";
            this.PanelContenedor.RowCount = 2;
            //this.PanelContenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            //this.PanelContenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            //this.PanelContenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.PanelContenedor.Size = new System.Drawing.Size(1407, 905);
            this.PanelContenedor.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 489F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1399, 489);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabUbicacion);
            this.tabControl1.Controls.Add(this.tabUbicacionUnidades);
            this.tabControl1.Controls.Add(this.tabSKU);
            this.tabControl1.Controls.Add(this.tabMedidas);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 500);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1401, 402);
            this.tabControl1.TabIndex = 4;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.tableGeneral);
            this.tabGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGeneral.Location = new System.Drawing.Point(4, 30);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(1393, 368);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = Lenguaje.traduce("General");
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tableGeneral
            // 
            this.tableGeneral.ColumnCount = 1;
            this.tableGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableGeneral.Location = new System.Drawing.Point(3, 3);
            this.tableGeneral.Name = "tableGeneral";
            this.tableGeneral.RowCount = 1;
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableGeneral.Size = new System.Drawing.Size(1387, 362);
            this.tableGeneral.TabIndex = 1;
            // 
            // tabUbicacion
            // 
            this.tabUbicacion.Controls.Add(this.tableUbicacion);
            this.tabUbicacion.Location = new System.Drawing.Point(4, 29);
            this.tabUbicacion.Name = "tabUbicacion";
            this.tabUbicacion.Padding = new System.Windows.Forms.Padding(3);
            this.tabUbicacion.Size = new System.Drawing.Size(1393, 369);
            this.tabUbicacion.TabIndex = 0;
            this.tabUbicacion.Text = Lenguaje.traduce("Ubicación");
            this.tabUbicacion.UseVisualStyleBackColor = true;
            // 
            // tableUbicacion
            // 
            this.tableUbicacion.ColumnCount = 1;
            this.tableUbicacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableUbicacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableUbicacion.Location = new System.Drawing.Point(3, 3);
            this.tableUbicacion.Name = "tableUbicacion";
            this.tableUbicacion.RowCount = 1;
            this.tableUbicacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableUbicacion.Size = new System.Drawing.Size(1387, 363);
            this.tableUbicacion.TabIndex = 0;
            // 
            // tabUbicacionUnidades
            // 
            this.tabUbicacionUnidades.Controls.Add(this.tableUbicacionUD);
            this.tabUbicacionUnidades.Location = new System.Drawing.Point(4, 29);
            this.tabUbicacionUnidades.Name = "tabUbicacionUnidades";
            this.tabUbicacionUnidades.Padding = new System.Windows.Forms.Padding(3);
            this.tabUbicacionUnidades.Size = new System.Drawing.Size(1393, 369);
            this.tabUbicacionUnidades.TabIndex = 2;
            this.tabUbicacionUnidades.Text = Lenguaje.traduce("Ubicación Unidades");
            this.tabUbicacionUnidades.UseVisualStyleBackColor = true;
            // 
            // tableUbicacionUD
            // 
            this.tableUbicacionUD.ColumnCount = 1;
            this.tableUbicacionUD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableUbicacionUD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableUbicacionUD.Location = new System.Drawing.Point(3, 3);
            this.tableUbicacionUD.Name = "tableUbicacionUD";
            this.tableUbicacionUD.RowCount = 1;
            this.tableUbicacionUD.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableUbicacionUD.Size = new System.Drawing.Size(1387, 363);
            this.tableUbicacionUD.TabIndex = 0;
            // 
            // tabSKU
            // 
            this.tabSKU.Controls.Add(this.tableSKU);
            this.tabSKU.Location = new System.Drawing.Point(4, 29);
            this.tabSKU.Name = "tabSKU";
            this.tabSKU.Padding = new System.Windows.Forms.Padding(3);
            this.tabSKU.Size = new System.Drawing.Size(1393, 369);
            this.tabSKU.TabIndex = 3;
            this.tabSKU.Text = "SKU";
            this.tabSKU.UseVisualStyleBackColor = true;
            // 
            // tableSKU
            // 
            this.tableSKU.ColumnCount = 1;
            this.tableSKU.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSKU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSKU.Location = new System.Drawing.Point(3, 3);
            this.tableSKU.Name = "tableSKU";
            this.tableSKU.RowCount = 1;
            this.tableSKU.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSKU.Size = new System.Drawing.Size(1387, 363);
            this.tableSKU.TabIndex = 0;
            // 
            // tabMedidas
            // 
            this.tabMedidas.Controls.Add(this.tableMedidas);
            this.tabMedidas.Location = new System.Drawing.Point(4, 29);
            this.tabMedidas.Name = "tabMedidas";
            this.tabMedidas.Padding = new System.Windows.Forms.Padding(3);
            this.tabMedidas.Size = new System.Drawing.Size(1393, 369);
            this.tabMedidas.TabIndex = 4;
            this.tabMedidas.Text = Lenguaje.traduce("Medidas");
            this.tabMedidas.UseVisualStyleBackColor = true;
            // 
            // tableMedidas
            // 
            this.tableMedidas.ColumnCount = 1;
            this.tableMedidas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMedidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMedidas.Location = new System.Drawing.Point(3, 3);
            this.tableMedidas.Name = "tableMedidas";
            this.tableMedidas.RowCount = 1;
            this.tableMedidas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMedidas.Size = new System.Drawing.Size(1387, 363);
            this.tableMedidas.TabIndex = 0;
            // 
            // lblCantidad
            // 
            this.lblCantidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidad.Location = new System.Drawing.Point(3, 3);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(93, 30);
            this.lblCantidad.TabIndex = 0;
            this.lblCantidad.Text = "radLabel1";
            // 
            // ArticulosBaseGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelContenedor);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ArticulosBaseGridControl";
            this.Size = new System.Drawing.Size(1407, 905);
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).EndInit();
            this.PanelContenedor.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabUbicacion.ResumeLayout(false);
            this.tabUbicacionUnidades.ResumeLayout(false);
            this.tabSKU.ResumeLayout(false);
            this.tabMedidas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblCantidad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public Telerik.WinControls.UI.RadVirtualGrid virtualGrid;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        public RumLabel lblCantidad;
        public TableLayoutPanel PanelContenedor;
        public TableLayoutPanel tableLayoutPanel1;
        public TabControl tabControl1;
        public TabPage tabGeneral;
        public TabPage tabUbicacion;
        public TableLayoutPanel tableGeneral;
        public TableLayoutPanel tableUbicacion;
        public TabPage tabUbicacionUnidades;
        private TabPage tabSKU;
        public TabPage tabMedidas;
        public TableLayoutPanel tableUbicacionUD;
        public TableLayoutPanel tableSKU;
        public TableLayoutPanel tableMedidas;
    }
}

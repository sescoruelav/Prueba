using RumboSGA.Properties;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls
{
    partial class BaseGridControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.virtualGrid = new Telerik.WinControls.UI.RadVirtualGrid();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.lblCantidad = new RumboSGA.RumLabel();
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCantidad)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 905F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1407, 905);
            this.tableLayoutPanel1.TabIndex = 2;
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
            this.virtualGrid.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 58, 778, 501);
            this.virtualGrid.Size = new System.Drawing.Size(778, 501);
            this.virtualGrid.TabIndex = 0;
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "";
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
            // BaseGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BaseGridControl";
            this.Size = new System.Drawing.Size(1407, 905);
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCantidad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public Telerik.WinControls.UI.RadVirtualGrid virtualGrid;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        public RumLabel lblCantidad;
    }
}

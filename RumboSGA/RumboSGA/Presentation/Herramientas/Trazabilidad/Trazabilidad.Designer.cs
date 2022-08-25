using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas
{
    partial class Trazabilidad
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
            this.contenedor = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.radCollapsiblePanel1 = new Telerik.WinControls.UI.RadCollapsiblePanel();
            this.radTreeView1 = new Telerik.WinControls.UI.RadTreeView();
            this.titleBarControl = new RumboSGA.Presentation.UserControls.TitleBarControl();
            this.contenedor.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).BeginInit();
            this.radCollapsiblePanel1.PanelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contenedor
            // 
            this.contenedor.Controls.Add(this.tableLayoutPanel);
            this.contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contenedor.Location = new System.Drawing.Point(0, 36);
            this.contenedor.Name = "contenedor";
            this.contenedor.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.contenedor.Size = new System.Drawing.Size(1071, 669);
            this.contenedor.TabIndex = 1;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 267F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.radCollapsiblePanel1, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(5, 20);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1061, 644);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // radCollapsiblePanel1
            // 
            this.radCollapsiblePanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radCollapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radCollapsiblePanel1.ExpandDirection = Telerik.WinControls.UI.RadDirection.Right;
            this.radCollapsiblePanel1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radCollapsiblePanel1.Location = new System.Drawing.Point(8, 8);
            this.radCollapsiblePanel1.Name = "radCollapsiblePanel1";
            this.radCollapsiblePanel1.OwnerBoundsCache = new System.Drawing.Rectangle(3, 85, 261, 735);
            // 
            // radCollapsiblePanel1.PanelContainer
            // 
            this.radCollapsiblePanel1.PanelContainer.Controls.Add(this.radTreeView1);
            this.radCollapsiblePanel1.PanelContainer.Size = new System.Drawing.Size(1017, 626);
            // 
            // 
            // 
            this.radCollapsiblePanel1.RootElement.ControlBounds = new System.Drawing.Rectangle(8, 8, 1045, 480);
            this.radCollapsiblePanel1.Size = new System.Drawing.Size(1045, 628);
            this.radCollapsiblePanel1.TabIndex = 2;
            // 
            // radTreeView1
            // 
            this.radTreeView1.BackColor = System.Drawing.SystemColors.Control;
            this.radTreeView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.radTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeView1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.radTreeView1.ForeColor = System.Drawing.Color.Black;
            this.radTreeView1.ItemHeight = 28;
            this.radTreeView1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.radTreeView1.LineStyle = Telerik.WinControls.UI.TreeLineStyle.Solid;
            this.radTreeView1.Location = new System.Drawing.Point(0, 0);
            this.radTreeView1.Name = "radTreeView1";
            this.radTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.radTreeView1.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 1005, 466);
            this.radTreeView1.ShowExpandCollapse = false;
            this.radTreeView1.Size = new System.Drawing.Size(1017, 626);
            this.radTreeView1.SpacingBetweenNodes = -1;
            this.radTreeView1.TabIndex = 1;
            // 
            // titleBarControl
            // 
            this.titleBarControl.AllowDrop = true;
            this.titleBarControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(53)))), ((int)(((byte)(54)))));
            this.titleBarControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarControl.Location = new System.Drawing.Point(0, 0);
            this.titleBarControl.Name = "titleBarControl";
            this.titleBarControl.Size = new System.Drawing.Size(1071, 36);
            this.titleBarControl.TabIndex = 0;
            // 
            // Trazabilidad
            // 
            this.ClientSize = new System.Drawing.Size(1071, 705);
            this.Controls.Add(this.contenedor);
            this.Controls.Add(this.titleBarControl);
            this.MainMenuStrip = null;
            this.Name = "Trazabilidad";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Trazabilidad";
            this.Load += new System.EventHandler(this.ShapedForm1_Load);
            this.contenedor.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.radCollapsiblePanel1.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadGridView gridView;
        private Presentation.UserControls.TitleBarControl titleBarControl;
        private RadCollapsiblePanel radCollapsiblePanel1;
        private RadTreeView radTreeView1;
        private Panel contenedor;
    }
}
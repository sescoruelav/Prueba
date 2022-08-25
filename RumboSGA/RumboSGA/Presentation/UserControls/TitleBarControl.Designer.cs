namespace RumboSGA.Presentation.UserControls
{
    partial class TitleBarControl
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
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.472F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.52799F));
            this.tableLayoutPanel1.Controls.Add(this.radPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radTitleBar1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(951, 57);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.radPanel1.Name = "radPanel1";
            // 
            // 
            // 
            this.radPanel1.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 3, 200, 100);
            this.radPanel1.Size = new System.Drawing.Size(232, 57);
            this.radPanel1.TabIndex = 2;
            this.radPanel1.Text = "radPanel1";
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.AllowResize = false;
            this.radTitleBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(53)))), ((int)(((byte)(54)))));
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTitleBar1.Location = new System.Drawing.Point(232, 0);
            this.radTitleBar1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.radTitleBar1.Name = "radTitleBar1";
            // 
            // 
            // 
            this.radTitleBar1.RootElement.ApplyShapeToControl = true;
            this.radTitleBar1.RootElement.ControlBounds = new System.Drawing.Rectangle(192, 3, 220, 23);
            this.radTitleBar1.Size = new System.Drawing.Size(719, 57);
            this.radTitleBar1.TabIndex = 3;
            this.radTitleBar1.TabStop = false;
            // 
            // TitleBarControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TitleBarControl";
            this.Size = new System.Drawing.Size(951, 57);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
    }
}

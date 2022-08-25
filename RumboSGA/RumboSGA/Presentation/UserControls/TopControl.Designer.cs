namespace RumboSGA.Presentation.UserControls
{
    partial class TopControl
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
            this.chooseThemeDropDown = new Telerik.WinControls.UI.RadDropDownList();
            this.chooseThemeLabel = new Telerik.WinControls.UI.RadLabel();
            this.viewLabel = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chooseThemeDropDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chooseThemeLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.33253F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.84834F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.81913F));
            this.tableLayoutPanel1.Controls.Add(this.chooseThemeDropDown, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chooseThemeLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1046, 53);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chooseThemeDropDown
            // 
            this.chooseThemeDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseThemeDropDown.DropDownAnimationEnabled = false;
            this.chooseThemeDropDown.Location = new System.Drawing.Point(817, 14);
            this.chooseThemeDropDown.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.chooseThemeDropDown.Name = "chooseThemeDropDown";
            this.chooseThemeDropDown.Size = new System.Drawing.Size(219, 24);
            this.chooseThemeDropDown.TabIndex = 3;
            // 
            // chooseThemeLabel
            // 
            this.chooseThemeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chooseThemeLabel.Location = new System.Drawing.Point(720, 18);
            this.chooseThemeLabel.Name = "chooseThemeLabel";
            this.chooseThemeLabel.Size = new System.Drawing.Size(84, 16);
            this.chooseThemeLabel.TabIndex = 2;
            this.chooseThemeLabel.Text = "Cambiar Tema:";
            // 
            // viewLabel
            // 
            this.viewLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.viewLabel.Location = new System.Drawing.Point(0, 18);
            this.viewLabel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.viewLabel.Name = "viewLabel";
            this.viewLabel.Size = new System.Drawing.Size(59, 16);
            this.viewLabel.TabIndex = 1;
            this.viewLabel.Text = "ViewLabel";
            // 
            // TopControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TopControl";
            this.Size = new System.Drawing.Size(1046, 53);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chooseThemeDropDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chooseThemeLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewLabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadDropDownList chooseThemeDropDown;
        private Telerik.WinControls.UI.RadLabel chooseThemeLabel;
        private Telerik.WinControls.UI.RadLabel viewLabel;
    }
}

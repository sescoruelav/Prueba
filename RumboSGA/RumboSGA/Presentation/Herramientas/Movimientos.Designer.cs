namespace RumboSGA.Herramientas
{
    partial class Movimientos
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.titleBarControl1 = new RumboSGA.Presentation.UserControls.TitleBarControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.radButton1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radButton2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.radGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radButton3, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 36);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.019347F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.98065F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1489, 891);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnCancelar
            // 
            this.radButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radButton1.Location = new System.Drawing.Point(449, 3);
            this.radButton1.Name = "btnCancelar";
            this.radButton1.Size = new System.Drawing.Size(142, 20);
            this.radButton1.TabIndex = 0;
            this.radButton1.Text = "Columnas";
            this.radButton1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radButton2
            // 
            this.radButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radButton2.Location = new System.Drawing.Point(597, 3);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(291, 20);
            this.radButton2.TabIndex = 1;
            this.radButton2.Text = "Reservas X Articulos";
            this.radButton2.Click += new System.EventHandler(this.button2_Click);
            // 
            // radGridView1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.radGridView1, 5);
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(3, 29);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(1483, 859);
            this.radGridView1.TabIndex = 2;
            // 
            // radButton3
            // 
            this.radButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radButton3.Location = new System.Drawing.Point(894, 3);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(142, 20);
            this.radButton3.TabIndex = 3;
            this.radButton3.Text = "Reiniciar";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // titleBarControl1
            // 
            this.titleBarControl1.AllowDrop = true;
            this.titleBarControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(53)))), ((int)(((byte)(54)))));
            this.titleBarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarControl1.Location = new System.Drawing.Point(0, 0);
            this.titleBarControl1.Name = "titleBarControl1";
            this.titleBarControl1.Size = new System.Drawing.Size(1489, 36);
            this.titleBarControl1.TabIndex = 0;
            // 
            // Movimientos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 927);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.titleBarControl1);
            this.Name = "Movimientos";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Movimientos";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Presentation.UserControls.TitleBarControl titleBarControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButton radButton3;
    }
}

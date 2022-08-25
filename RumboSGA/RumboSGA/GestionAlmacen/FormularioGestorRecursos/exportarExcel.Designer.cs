namespace RumboSGA.GestionAlmacen.FormularioGestorRecursos
{
    partial class exportarExcel
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
            this.components = new System.ComponentModel.Container();
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dbTableLayoutPanel1 = new RumboSGA.Controles.DBTableLayoutPanel(this.components);
            this.rumButton1 = new RumboSGA.RumButton();
            this.rumButton2 = new RumboSGA.RumButton();
            this.rumLabel1 = new RumboSGA.RumLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            this.dbTableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.BackColor = System.Drawing.Color.Silver;
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTitleBar1.Location = new System.Drawing.Point(0, 0);
            this.radTitleBar1.Name = "radTitleBar1";
            this.radTitleBar1.Size = new System.Drawing.Size(348, 38);
            this.radTitleBar1.TabIndex = 0;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.Text = "importarExcel";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Recursos"});
            this.comboBox1.Location = new System.Drawing.Point(88, 150);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(176, 24);
            this.comboBox1.TabIndex = 1;
            // 
            // dbTableLayoutPanel1
            // 
            this.dbTableLayoutPanel1.ColumnCount = 5;
            this.dbTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.dbTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.dbTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.dbTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.dbTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.dbTableLayoutPanel1.Controls.Add(this.rumButton1, 1, 0);
            this.dbTableLayoutPanel1.Controls.Add(this.rumButton2, 3, 0);
            this.dbTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbTableLayoutPanel1.Location = new System.Drawing.Point(0, 273);
            this.dbTableLayoutPanel1.Name = "dbTableLayoutPanel1";
            this.dbTableLayoutPanel1.RowCount = 1;
            this.dbTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dbTableLayoutPanel1.Size = new System.Drawing.Size(348, 31);
            this.dbTableLayoutPanel1.TabIndex = 3;
            // 
            // rumButton1
            // 
            this.rumButton1.Image = global::RumboSGA.Properties.Resources.Approve;
            this.rumButton1.Location = new System.Drawing.Point(37, 3);
            this.rumButton1.Name = "rumButton1";
            this.rumButton1.Size = new System.Drawing.Size(98, 25);
            this.rumButton1.TabIndex = 4;
            this.rumButton1.Text = "Aceptar";
            this.rumButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rumButton1.Click += new System.EventHandler(this.rumButton1_Click);
            // 
            // rumButton2
            // 
            this.rumButton2.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rumButton2.Location = new System.Drawing.Point(210, 3);
            this.rumButton2.Name = "rumButton2";
            this.rumButton2.Size = new System.Drawing.Size(98, 25);
            this.rumButton2.TabIndex = 5;
            this.rumButton2.Text = "Cancelar";
            this.rumButton2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.rumButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rumButton2.Click += new System.EventHandler(this.rumButton2_Click);
            // 
            // rumLabel1
            // 
            this.rumLabel1.Location = new System.Drawing.Point(61, 110);
            this.rumLabel1.Name = "rumLabel1";
            this.rumLabel1.Size = new System.Drawing.Size(287, 25);
            this.rumLabel1.TabIndex = 2;
            this.rumLabel1.Text = "Selecciona el grid que quieras exportar";
            // 
            // exportarExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 304);
            this.Controls.Add(this.dbTableLayoutPanel1);
            this.Controls.Add(this.rumLabel1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.radTitleBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "exportarExcel";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "importarExcel";
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            this.dbTableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rumButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
        private System.Windows.Forms.ComboBox comboBox1;
        private RumLabel rumLabel1;
        private Controles.DBTableLayoutPanel dbTableLayoutPanel1;
        private RumButton rumButton1;
        private RumButton rumButton2;
    }
}

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class CrearIncidencia
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radTextBoxControl1 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radTextBoxControl2 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.rumButton2 = new RumboSGA.RumButton();
            this.rumLabel6 = new RumboSGA.RumLabel();
            this.rumLabel4 = new RumboSGA.RumLabel();
            this.rumLabel3 = new RumboSGA.RumLabel();
            this.rumLabel2 = new RumboSGA.RumLabel();
            this.rumLabel1 = new RumboSGA.RumLabel();
            this.rumButton1 = new RumboSGA.RumButton();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(139, 70);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(208, 24);
            this.comboBox1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(29, 441);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(616, 171);
            this.panel1.TabIndex = 4;
            this.panel1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.panel1_ControlRemoved);
            // 
            // radTextBoxControl1
            // 
            this.radTextBoxControl1.Location = new System.Drawing.Point(29, 236);
            this.radTextBoxControl1.Name = "radTextBoxControl1";
            this.radTextBoxControl1.Size = new System.Drawing.Size(616, 75);
            this.radTextBoxControl1.TabIndex = 7;
            // 
            // radTextBoxControl2
            // 
            this.radTextBoxControl2.Location = new System.Drawing.Point(29, 137);
            this.radTextBoxControl2.Name = "radTextBoxControl2";
            this.radTextBoxControl2.Size = new System.Drawing.Size(616, 52);
            this.radTextBoxControl2.TabIndex = 10;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Baja",
            "Media",
            "Alta"});
            this.comboBox2.Location = new System.Drawing.Point(29, 355);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(128, 24);
            this.comboBox2.TabIndex = 12;
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.BackColor = System.Drawing.Color.Gray;
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTitleBar1.Location = new System.Drawing.Point(0, 0);
            this.radTitleBar1.Name = "radTitleBar1";
            this.radTitleBar1.Size = new System.Drawing.Size(690, 44);
            this.radTitleBar1.TabIndex = 15;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.Text = "Crear Incidencia";
            // 
            // rumButton2
            // 
            this.rumButton2.Image = global::RumboSGA.Properties.Resources.Add;
            this.rumButton2.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rumButton2.Location = new System.Drawing.Point(29, 618);
            this.rumButton2.Name = "rumButton2";
            this.rumButton2.Size = new System.Drawing.Size(128, 36);
            this.rumButton2.TabIndex = 5;
            this.rumButton2.Text = "Añadir archivo";
            this.rumButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rumButton2.Click += new System.EventHandler(this.rumButton2_Click);
            // 
            // rumLabel6
            // 
            this.rumLabel6.Location = new System.Drawing.Point(29, 419);
            this.rumLabel6.Name = "rumLabel6";
            this.rumLabel6.Size = new System.Drawing.Size(56, 20);
            this.rumLabel6.TabIndex = 14;
            this.rumLabel6.Text = "Archivos";
            // 
            // rumLabel4
            // 
            this.rumLabel4.Location = new System.Drawing.Point(29, 333);
            this.rumLabel4.Name = "rumLabel4";
            this.rumLabel4.Size = new System.Drawing.Size(59, 20);
            this.rumLabel4.TabIndex = 13;
            this.rumLabel4.Text = "Prioridad";
            // 
            // rumLabel3
            // 
            this.rumLabel3.Location = new System.Drawing.Point(25, 71);
            this.rumLabel3.Name = "rumLabel3";
            this.rumLabel3.Size = new System.Drawing.Size(111, 20);
            this.rumLabel3.TabIndex = 11;
            this.rumLabel3.Text = "Tipo de Incidencia";
            // 
            // rumLabel2
            // 
            this.rumLabel2.Location = new System.Drawing.Point(29, 111);
            this.rumLabel2.Name = "rumLabel2";
            this.rumLabel2.Size = new System.Drawing.Size(48, 20);
            this.rumLabel2.TabIndex = 9;
            this.rumLabel2.Text = "Asunto";
            // 
            // rumLabel1
            // 
            this.rumLabel1.Location = new System.Drawing.Point(29, 210);
            this.rumLabel1.Name = "rumLabel1";
            this.rumLabel1.Size = new System.Drawing.Size(74, 20);
            this.rumLabel1.TabIndex = 8;
            this.rumLabel1.Text = "Descripción";
            // 
            // rumButton1
            // 
            this.rumButton1.Location = new System.Drawing.Point(277, 703);
            this.rumButton1.Name = "rumButton1";
            this.rumButton1.Size = new System.Drawing.Size(137, 30);
            this.rumButton1.TabIndex = 1;
            this.rumButton1.Text = "Lanzar Incidencia";
            this.rumButton1.Click += new System.EventHandler(this.rumButton1_Click);
            // 
            // CrearIncidencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 745);
            this.Controls.Add(this.rumButton2);
            this.Controls.Add(this.radTitleBar1);
            this.Controls.Add(this.rumLabel6);
            this.Controls.Add(this.rumLabel4);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.rumLabel3);
            this.Controls.Add(this.radTextBoxControl2);
            this.Controls.Add(this.rumLabel2);
            this.Controls.Add(this.rumLabel1);
            this.Controls.Add(this.radTextBoxControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rumButton1);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CrearIncidencia";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Crear Incidencia";
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private RumButton rumButton1;
        private System.Windows.Forms.Panel panel1;
        private RumButton rumButton2;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControl1;
        private RumLabel rumLabel1;
        private RumLabel rumLabel2;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControl2;
        private RumLabel rumLabel3;
        private System.Windows.Forms.ComboBox comboBox2;
        private RumLabel rumLabel4;
        private RumLabel rumLabel6;
        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
    }
}

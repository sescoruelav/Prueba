namespace RumboSGA.Presentation.Formulario_Recepciones
{
    partial class FiltroInicialRecepciones
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
            this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
            this.btnAceptar = new RumButton();
            this.btnOr = new RumButton();
            this.btnAnd = new RumButton();
            this.btnBorrar = new RumButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
            this.radScrollablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBorrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.radScrollablePanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOr, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnAnd, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnBorrar, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.87135F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.12865F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(516, 367);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radScrollablePanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.radScrollablePanel1, 4);
            this.radScrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radScrollablePanel1.Location = new System.Drawing.Point(3, 3);
            this.radScrollablePanel1.Name = "radScrollablePanel1";
            // 
            // radScrollablePanel1.PanelContainer
            // 
            this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(508, 292);
            this.radScrollablePanel1.Size = new System.Drawing.Size(510, 294);
            this.radScrollablePanel1.TabIndex = 1;
            // 
            // radButton1
            // 
            this.btnAceptar.Location = new System.Drawing.Point(3, 303);
            this.btnAceptar.Name = "radButton1";
            this.btnAceptar.Size = new System.Drawing.Size(110, 24);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "radButton1";
            this.btnAceptar.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // btnOr
            // 
            this.btnOr.Location = new System.Drawing.Point(390, 303);
            this.btnOr.Name = "btnOr";
            this.btnOr.Size = new System.Drawing.Size(110, 24);
            this.btnOr.TabIndex = 5;
            this.btnOr.Text = "OR";
            this.btnOr.Click += new System.EventHandler(this.btnOr_Click);
            // 
            // btnAnd
            // 
            this.btnAnd.Location = new System.Drawing.Point(261, 303);
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.Size = new System.Drawing.Size(110, 24);
            this.btnAnd.TabIndex = 4;
            this.btnAnd.Text = "AND";
            this.btnAnd.Click += new System.EventHandler(this.btnAnd_Click);
            // 
            // btnBorrar
            // 
            this.btnBorrar.Location = new System.Drawing.Point(132, 303);
            this.btnBorrar.Name = "btnBorrar";
            this.btnBorrar.Size = new System.Drawing.Size(110, 24);
            this.btnBorrar.TabIndex = 6;
            this.btnBorrar.Text = "radButton2";
            // 
            // FiltroInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(516, 367);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FiltroInicial";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "FiltroInicial";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
            this.radScrollablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBorrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadScrollablePanel radScrollablePanel1;
        private RumButton btnAceptar;
        private RumButton btnAnd;
        private RumButton btnOr;
        private RumButton btnBorrar;
    }
}

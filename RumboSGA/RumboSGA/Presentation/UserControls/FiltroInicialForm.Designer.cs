namespace RumboSGA.Presentation
{
    partial class FiltroInicialForm
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
            this.ordersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.radDataFilter1 = new Telerik.WinControls.UI.RadDataFilter();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radScrollablePanel1 = new Telerik.WinControls.UI.RadScrollablePanel();
            this.btnCancelar = new RumboSGA.RumButton();
            this.btnGuardar = new RumboSGA.RumButton();
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDataFilter1)).BeginInit();
            this.radDataFilter1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).BeginInit();
            this.radScrollablePanel1.PanelContainer.SuspendLayout();
            this.radScrollablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGuardar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radDataFilter1
            // 
            this.radDataFilter1.Controls.Add(this.radPanel1);
            this.radDataFilter1.DataSource = this.ordersBindingSource;
            this.radDataFilter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDataFilter1.Location = new System.Drawing.Point(0, 0);
            this.radDataFilter1.Name = "radDataFilter1";
            this.radDataFilter1.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.radDataFilter1.Size = new System.Drawing.Size(502, 304);
            this.radDataFilter1.TabIndex = 2;
            // 
            // radPanel1
            // 
            this.radPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanel1.Controls.Add(this.btnCancelar);
            this.radPanel1.Controls.Add(this.btnGuardar);
            this.radPanel1.Location = new System.Drawing.Point(0, 268);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(502, 36);
            this.radPanel1.TabIndex = 1;
            // 
            // radScrollablePanel1
            // 
            this.radScrollablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radScrollablePanel1.Location = new System.Drawing.Point(0, 0);
            this.radScrollablePanel1.Name = "radScrollablePanel1";
            // 
            // radScrollablePanel1.PanelContainer
            // 
            this.radScrollablePanel1.PanelContainer.Controls.Add(this.radDataFilter1);
            this.radScrollablePanel1.PanelContainer.Size = new System.Drawing.Size(502, 304);
            this.radScrollablePanel1.Size = new System.Drawing.Size(504, 306);
            this.radScrollablePanel1.TabIndex = 2;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(381, 5);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 24);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "btnCancelar";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGuardar.Location = new System.Drawing.Point(11, 5);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 24);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "btnGuardar";
            // 
            // FiltroInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(504, 306);
            this.Controls.Add(this.radScrollablePanel1);
            this.Name = "FiltroInicial";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            ((System.ComponentModel.ISupportInitialize)(this.ordersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDataFilter1)).EndInit();
            this.radDataFilter1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radScrollablePanel1.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radScrollablePanel1)).EndInit();
            this.radScrollablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGuardar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource ordersBindingSource;
        private Telerik.WinControls.UI.RadDataFilter radDataFilter1;
        private Telerik.WinControls.UI.RadScrollablePanel radScrollablePanel1;        
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private RumButton btnCancelar;
        private RumButton btnGuardar;
    }
} 

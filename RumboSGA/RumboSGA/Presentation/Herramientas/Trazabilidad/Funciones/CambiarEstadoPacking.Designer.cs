namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    partial class CambiarEstadoPacking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CambiarEstadoPacking));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.lblCambiarEstado = new RumboSGA.RumLabel();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.btnCancelar = new RumboSGA.RumButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCambiarEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCambiarEstado, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radDropDownList1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.44444F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(562, 80);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(3, 38);
            this.btnAceptar.Name = "btnAceptar";
            // 
            // 
            // 
            this.btnAceptar.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 37, 110, 24);
            this.btnAceptar.Size = new System.Drawing.Size(275, 39);
            this.btnAceptar.TabIndex = 1;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // lblCambiarEstado
            // 
            this.lblCambiarEstado.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCambiarEstado.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCambiarEstado.Location = new System.Drawing.Point(109, 10);
            this.lblCambiarEstado.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.lblCambiarEstado.Name = "lblCambiarEstado";
            // 
            // 
            // 
            this.lblCambiarEstado.RootElement.ControlBounds = new System.Drawing.Rectangle(103, 10, 100, 18);
            this.lblCambiarEstado.Size = new System.Drawing.Size(62, 16);
            this.lblCambiarEstado.TabIndex = 0;
            this.lblCambiarEstado.Text = "labelFecha";
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radDropDownList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDropDownList1.Location = new System.Drawing.Point(284, 10);
            this.radDropDownList1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.radDropDownList1.Name = "radDropDownList1";
            // 
            // 
            // 
            this.radDropDownList1.RootElement.ControlBounds = new System.Drawing.Rectangle(272, 10, 125, 20);
            this.radDropDownList1.RootElement.StretchVertically = true;
            this.radDropDownList1.Size = new System.Drawing.Size(275, 22);
            this.radDropDownList1.TabIndex = 0;
            this.radDropDownList1.Text = "radDropDownList1";
            this.radDropDownList1.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.radDropDownList1_SelectedIndexChanged);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(284, 38);
            this.btnCancelar.Name = "btnCancelar";
            // 
            // 
            // 
            this.btnCancelar.RootElement.ControlBounds = new System.Drawing.Rectangle(272, 37, 110, 24);
            this.btnCancelar.Size = new System.Drawing.Size(275, 39);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // CambiarEstadoExistencia
            // 
            this.AllowResize = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 80);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(578, 119);
            this.MinimumSize = new System.Drawing.Size(578, 119);
            this.Name = "CambiarEstadoExistencia";
            this.Text = "Cambiar Estado Existencia";
            this.Load += new System.EventHandler(this.CambiarEstadoPicking_Load);
            this.Shown += new System.EventHandler(this.form_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCambiarEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumLabel lblCambiarEstado;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
    }
}

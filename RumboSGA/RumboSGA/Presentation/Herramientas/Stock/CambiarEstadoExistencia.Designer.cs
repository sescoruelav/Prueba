namespace RumboSGA.Presentation.Herramientas.Stock
{
    partial class CambiarEstadoExistencia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CambiarEstadoExistencia));
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
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 97);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(351, 69);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAceptar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(8, 13);
            this.btnAceptar.Name = "btnAceptar";
            // 
            // 
            // 
            this.btnAceptar.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 38, 137, 30);
            this.btnAceptar.Size = new System.Drawing.Size(159, 53);
            this.btnAceptar.TabIndex = 1;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // lblCambiarEstado
            // 
            this.lblCambiarEstado.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCambiarEstado.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCambiarEstado.Location = new System.Drawing.Point(22, 18);
            this.lblCambiarEstado.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.lblCambiarEstado.Name = "lblCambiarEstado";
            // 
            // 
            // 
            this.lblCambiarEstado.RootElement.ControlBounds = new System.Drawing.Rectangle(109, 10, 125, 22);
            this.lblCambiarEstado.Size = new System.Drawing.Size(64, 19);
            this.lblCambiarEstado.TabIndex = 0;
            this.lblCambiarEstado.Text = "labelFecha";
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radDropDownList1.DropDownAnimationEnabled = true;
            this.radDropDownList1.Location = new System.Drawing.Point(22, 50);
            this.radDropDownList1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.radDropDownList1.Name = "radDropDownList1";
            // 
            // 
            // 
            this.radDropDownList1.RootElement.ControlBounds = new System.Drawing.Rectangle(284, 10, 156, 25);
            this.radDropDownList1.RootElement.StretchVertically = true;
            this.radDropDownList1.Size = new System.Drawing.Size(304, 32);
            this.radDropDownList1.TabIndex = 0;
            this.radDropDownList1.Text = "radDropDownList1";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancelar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(183, 13);
            this.btnCancelar.Name = "btnCancelar";
            // 
            // 
            // 
            this.btnCancelar.RootElement.ControlBounds = new System.Drawing.Rectangle(284, 38, 137, 30);
            this.btnCancelar.Size = new System.Drawing.Size(159, 53);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // CambiarEstadoExistencia
            // 
            this.AllowResize = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 166);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radDropDownList1);
            this.Controls.Add(this.lblCambiarEstado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CambiarEstadoExistencia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cambiar Estado Existencia";
            this.Load += new System.EventHandler(this.CambiarEstadoExistencia_Load);
            this.Shown += new System.EventHandler(this.form_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCambiarEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumLabel lblCambiarEstado;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
    }
}

namespace RumboSGA.Presentation.UserControls
{
    partial class VentanaGuardarEstilo
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAceptar = new RumButton();
            this.btnCancelar = new RumButton();
            this.radLabel1 = new RumLabel();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.radDropDownList1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.radLabel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(358, 61);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(66, 33);
            this.btnAceptar.Name = "btnCancelar";
            this.btnAceptar.Size = new System.Drawing.Size(110, 24);
            this.btnAceptar.TabIndex = 0;
            // 
            // btnAceptar
            // 
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(182, 33);
            this.btnCancelar.Name = "btnAceptar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 24);
            this.btnCancelar.TabIndex = 1;
            // 
            // labelFecha
            // 
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "labelFecha";
            this.radLabel1.Size = new System.Drawing.Size(169, 18);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "Selecciona tipo de configuracion";
            // 
            // radDropDownList1
            // 
            radListDataItem3.Text = "Local";
            radListDataItem4.Text = "Global";
            this.radDropDownList1.Items.Add(radListDataItem3);
            this.radDropDownList1.Items.Add(radListDataItem4);
            this.radDropDownList1.Location = new System.Drawing.Point(182, 3);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(125, 20);
            this.radDropDownList1.TabIndex = 1;
            // 
            // VentanaGuardarEstilo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(358, 61);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "VentanaGuardarEstilo";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "VentanaGuardarEstilo";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
        private RumLabel radLabel1;
    }
}

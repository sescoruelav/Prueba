namespace RumboSGA.GestionAlmacen
{
    partial class DevolucionesProveedorPendientes
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
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.btnVerDevolucion = new Telerik.WinControls.UI.RadButton();
            this.btnRefrescar = new Telerik.WinControls.UI.RadButton();
            this.btnSalir = new Telerik.WinControls.UI.RadButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnVerDevolucion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRefrescar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.radGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnVerDevolucion, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRefrescar, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSalir, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.409595F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.59041F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(656, 542);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radGridView1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.radGridView1, 10);
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(3, 54);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(650, 485);
            this.radGridView1.TabIndex = 0;
            // 
            // btnVerDevolucion
            // 
            this.btnVerDevolucion.BackColor = System.Drawing.Color.White;
            this.btnVerDevolucion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVerDevolucion.Image = global::RumboSGA.Properties.Resources.View;
            this.btnVerDevolucion.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.btnVerDevolucion.Location = new System.Drawing.Point(3, 3);
            this.btnVerDevolucion.Name = "btnVerDevolucion";
            this.btnVerDevolucion.Size = new System.Drawing.Size(158, 45);
            this.btnVerDevolucion.TabIndex = 1;
            this.btnVerDevolucion.Text = "Ver Devolución";
            this.btnVerDevolucion.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.BackColor = System.Drawing.Color.White;
            this.btnRefrescar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefrescar.Image = global::RumboSGA.Properties.Resources.Refresh;
            this.btnRefrescar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.btnRefrescar.Location = new System.Drawing.Point(167, 3);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(158, 45);
            this.btnRefrescar.TabIndex = 2;
            this.btnRefrescar.Text = "Refrescar Pantalla";
            this.btnRefrescar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.White;
            this.btnSalir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSalir.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnSalir.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.btnSalir.Location = new System.Drawing.Point(331, 3);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(158, 45);
            this.btnSalir.TabIndex = 3;
            this.btnSalir.Text = "Salir";
            this.btnSalir.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // DevolucionesProveedorPendientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 542);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DevolucionesProveedorPendientes";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "DevolucionesProveedor";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnVerDevolucion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRefrescar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButton btnVerDevolucion;
        private Telerik.WinControls.UI.RadButton btnRefrescar;
        private Telerik.WinControls.UI.RadButton btnSalir;
    }
}

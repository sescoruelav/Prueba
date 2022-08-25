namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    partial class ArticulosControl
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
            this.virtualGrid = new Telerik.WinControls.UI.RadVirtualGrid();
            this.lblCantidad = new RumboSGA.RumLabel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCantidad)).BeginInit();
            this.SuspendLayout();
            // 
            // virtualGrid
            // 
            this.virtualGrid.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.virtualGrid.BeginEditMode = Telerik.WinControls.UI.RadVirtualGridBeginEditMode.BeginEditProgrammatically;
            this.virtualGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualGrid.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.virtualGrid.Location = new System.Drawing.Point(3, 58);
            this.virtualGrid.MasterViewInfo.AllowRowResize = true;
            this.virtualGrid.MasterViewInfo.EnablePaging = true;
            this.virtualGrid.MasterViewInfo.PageSize = 20;
            this.virtualGrid.MasterViewInfo.RowsViewState.EnablePaging = true;
            this.virtualGrid.MasterViewInfo.RowsViewState.PageSize = 20;
            this.virtualGrid.MasterViewInfo.ShowNewRow = false;
            this.virtualGrid.Name = "virtualGrid";
            // 
            // 
            // 
            this.virtualGrid.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 58, 778, 501);
            this.virtualGrid.SelectionMode = Telerik.WinControls.UI.VirtualGridSelectionMode.FullRowSelect;
            this.virtualGrid.Size = new System.Drawing.Size(778, 501);
            this.virtualGrid.TabIndex = 0;
            // 
            // lblCantidad
            // 
            this.lblCantidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCantidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidad.Location = new System.Drawing.Point(3, 3);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(93, 30);
            this.lblCantidad.TabIndex = 0;
            this.lblCantidad.Text = "radLabel1";
            // 
            // tabGeneral
            // 
            this.tabGeneral.Location = new System.Drawing.Point(4, 25);
            this.tabGeneral.Size = new System.Drawing.Size(821, 65);
            // 
            // tabUbicacion
            // 
            this.tabUbicacion.Size = new System.Drawing.Size(1043, 299);
            // 
            // tabUbicacionUnidades
            // 
            this.tabUbicacionUnidades.Size = new System.Drawing.Size(1043, 299);
            // 
            // tabMedidas
            // 
            this.tabMedidas.Size = new System.Drawing.Size(1043, 299);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1151, 71);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 71);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ArticulosControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ArticulosControl";
            this.Size = new System.Drawing.Size(833, 118);
            ((System.ComponentModel.ISupportInitialize)(this.virtualGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCantidad)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Telerik.WinControls.UI.RadVirtualGrid virtualGrid;
        private RumLabel lblCantidad;
    }
}

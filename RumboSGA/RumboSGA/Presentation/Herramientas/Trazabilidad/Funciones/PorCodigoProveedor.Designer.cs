namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    partial class PorCodigoProveedor
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
            this.radMultiColumnComboBoxProveedores = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radButtonAceptar = new Telerik.WinControls.UI.RadButton();
            this.radButtonCancelar = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProveedores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProveedores.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radMultiColumnComboBoxProveedores
            // 
            // 
            // radMultiColumnComboBoxProveedores.NestedRadGridView
            // 
            this.radMultiColumnComboBoxProveedores.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radMultiColumnComboBoxProveedores.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMultiColumnComboBoxProveedores.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radMultiColumnComboBoxProveedores.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radMultiColumnComboBoxProveedores.EditorControl.Name = "NestedRadGridView";
            this.radMultiColumnComboBoxProveedores.EditorControl.ReadOnly = true;
            this.radMultiColumnComboBoxProveedores.EditorControl.ShowGroupPanel = false;
            this.radMultiColumnComboBoxProveedores.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radMultiColumnComboBoxProveedores.EditorControl.TabIndex = 0;
            this.radMultiColumnComboBoxProveedores.Location = new System.Drawing.Point(13, 13);
            this.radMultiColumnComboBoxProveedores.Name = "radMultiColumnComboBoxProveedores";
            this.radMultiColumnComboBoxProveedores.Size = new System.Drawing.Size(373, 20);
            this.radMultiColumnComboBoxProveedores.TabIndex = 0;
            this.radMultiColumnComboBoxProveedores.TabStop = false;
            // 
            // radButtonAceptar
            // 
            this.radButtonAceptar.Location = new System.Drawing.Point(12, 90);
            this.radButtonAceptar.Name = "radButtonAceptar";
            this.radButtonAceptar.Size = new System.Drawing.Size(110, 24);
            this.radButtonAceptar.TabIndex = 1;
            this.radButtonAceptar.Text = "radButton1";
            this.radButtonAceptar.Click += new System.EventHandler(this.aceptar_Event);
            // 
            // radButtonCancelar
            // 
            this.radButtonCancelar.Location = new System.Drawing.Point(276, 90);
            this.radButtonCancelar.Name = "radButtonCancelar";
            this.radButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancelar.TabIndex = 2;
            this.radButtonCancelar.Text = "radButton2";
            this.radButtonCancelar.Click += new System.EventHandler(this.salir_Event);
            // 
            // PorCodigoRuta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 126);
            this.Controls.Add(this.radButtonCancelar);
            this.Controls.Add(this.radButtonAceptar);
            this.Controls.Add(this.radMultiColumnComboBoxProveedores);
            this.Name = "PorCodigoProveedor";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PorCodigoProvedor";
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProveedores.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProveedores.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxProveedores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Telerik.WinControls.UI.RadMultiColumnComboBox radMultiColumnComboBoxProveedores;
        private Telerik.WinControls.UI.RadButton radButtonAceptar;
        private Telerik.WinControls.UI.RadButton radButtonCancelar;
    }
}
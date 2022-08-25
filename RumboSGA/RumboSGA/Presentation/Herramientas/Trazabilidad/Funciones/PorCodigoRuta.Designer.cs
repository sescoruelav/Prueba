namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    partial class PorCodigoRuta
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
            this.radMultiColumnComboBoxRutas = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radButtonAceptar = new Telerik.WinControls.UI.RadButton();
            this.radButtonCancelar = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxRutas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxRutas.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radMultiColumnComboBoxRutas
            // 
            // 
            // radMultiColumnComboBoxRutas.NestedRadGridView
            // 
            this.radMultiColumnComboBoxRutas.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radMultiColumnComboBoxRutas.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMultiColumnComboBoxRutas.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radMultiColumnComboBoxRutas.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radMultiColumnComboBoxRutas.EditorControl.Name = "NestedRadGridView";
            this.radMultiColumnComboBoxRutas.EditorControl.ReadOnly = true;
            this.radMultiColumnComboBoxRutas.EditorControl.ShowGroupPanel = false;
            this.radMultiColumnComboBoxRutas.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radMultiColumnComboBoxRutas.EditorControl.TabIndex = 0;
            this.radMultiColumnComboBoxRutas.Location = new System.Drawing.Point(13, 13);
            this.radMultiColumnComboBoxRutas.Name = "radMultiColumnComboBoxRutas";
            this.radMultiColumnComboBoxRutas.Size = new System.Drawing.Size(373, 20);
            this.radMultiColumnComboBoxRutas.TabIndex = 0;
            this.radMultiColumnComboBoxRutas.TabStop = false;
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
            this.Controls.Add(this.radMultiColumnComboBoxRutas);
            this.Name = "PorCodigoRuta";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PorCodigoRuta";
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxRutas.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxRutas.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxRutas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
    }

    #endregion

    private Telerik.WinControls.UI.RadMultiColumnComboBox radMultiColumnComboBoxRutas;
        private Telerik.WinControls.UI.RadButton radButtonAceptar;
        private Telerik.WinControls.UI.RadButton radButtonCancelar;
    }
}
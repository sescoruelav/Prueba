using System;

namespace RumboSGA.Presentation.FormulariosComunes
{
    partial class FormularioCrearPaletMultireferencia
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumLabelArticulo = new RumboSGA.RumLabel();
            this.rumLabelUbicacion = new RumboSGA.RumLabel();
            this.rumLabelPaletTipo = new RumboSGA.RumLabel();
            this.radComboBoxTipoPalet = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radMultiColumnComboBoxArticulos = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radMultiColumnComboBoxUbicacion = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelArticulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelUbicacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelPaletTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxTipoPalet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxTipoPalet.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxTipoPalet.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxArticulos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxArticulos.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUbicacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUbicacion.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rumButtonAceptar
            // 
            this.rumButtonAceptar.Location = new System.Drawing.Point(12, 204);
            this.rumButtonAceptar.Name = "rumButtonAceptar";
            this.rumButtonAceptar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonAceptar.TabIndex = 0;
            this.rumButtonAceptar.Text = "Aceptar";
            this.rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(220, 204);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonCancelar.TabIndex = 1;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // rumLabelArticulo
            // 
            this.rumLabelArticulo.AutoSize = false;
            this.rumLabelArticulo.Location = new System.Drawing.Point(56, 13);
            this.rumLabelArticulo.Name = "rumLabelArticulo";
            this.rumLabelArticulo.Size = new System.Drawing.Size(50, 18);
            this.rumLabelArticulo.TabIndex = 2;
            this.rumLabelArticulo.Text = "Artículo:";
            // 
            // rumLabelUbicacion
            // 
            this.rumLabelUbicacion.AutoSize = false;
            this.rumLabelUbicacion.Location = new System.Drawing.Point(45, 39);
            this.rumLabelUbicacion.Name = "rumLabelUbicacion";
            this.rumLabelUbicacion.Size = new System.Drawing.Size(61, 18);
            this.rumLabelUbicacion.TabIndex = 3;
            this.rumLabelUbicacion.Text = "Ubicación:";
            // 
            // rumLabelPaletTipo
            // 
            this.rumLabelPaletTipo.AutoSize = false;
            this.rumLabelPaletTipo.Location = new System.Drawing.Point(12, 65);
            this.rumLabelPaletTipo.Name = "rumLabelPaletTipo";
            this.rumLabelPaletTipo.Size = new System.Drawing.Size(97, 18);
            this.rumLabelPaletTipo.TabIndex = 4;
            this.rumLabelPaletTipo.Text = "Tipo Palet:";
            this.rumLabelPaletTipo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radComboBoxTipoPalet
            // 
            this.radComboBoxTipoPalet.AutoSizeDropDownHeight = true;
            this.radComboBoxTipoPalet.AutoSizeDropDownToBestFit = true;
            // 
            // radComboBoxTipoPalet.NestedRadGridView
            // 
            this.radComboBoxTipoPalet.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radComboBoxTipoPalet.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radComboBoxTipoPalet.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radComboBoxTipoPalet.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radComboBoxTipoPalet.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radComboBoxTipoPalet.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radComboBoxTipoPalet.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radComboBoxTipoPalet.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radComboBoxTipoPalet.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radComboBoxTipoPalet.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radComboBoxTipoPalet.EditorControl.Name = "NestedRadGridView";
            this.radComboBoxTipoPalet.EditorControl.ReadOnly = true;
            this.radComboBoxTipoPalet.EditorControl.ShowGroupPanel = false;
            this.radComboBoxTipoPalet.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radComboBoxTipoPalet.EditorControl.TabIndex = 0;
            this.radComboBoxTipoPalet.Location = new System.Drawing.Point(115, 65);
            this.radComboBoxTipoPalet.Name = "radComboBoxTipoPalet";
            this.radComboBoxTipoPalet.Size = new System.Drawing.Size(190, 24);
            this.radComboBoxTipoPalet.TabIndex = 7;
            this.radComboBoxTipoPalet.TabStop = false;
            // 
            // radMultiColumnComboBoxArticulos
            // 
            this.radMultiColumnComboBoxArticulos.AutoSizeDropDownHeight = true;
            this.radMultiColumnComboBoxArticulos.AutoSizeDropDownToBestFit = true;
            // 
            // radMultiColumnComboBoxArticulos.NestedRadGridView
            // 
            this.radMultiColumnComboBoxArticulos.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radMultiColumnComboBoxArticulos.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMultiColumnComboBoxArticulos.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radMultiColumnComboBoxArticulos.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radMultiColumnComboBoxArticulos.EditorControl.Name = "NestedRadGridView";
            this.radMultiColumnComboBoxArticulos.EditorControl.ReadOnly = true;
            this.radMultiColumnComboBoxArticulos.EditorControl.ShowGroupPanel = false;
            this.radMultiColumnComboBoxArticulos.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radMultiColumnComboBoxArticulos.EditorControl.TabIndex = 0;
            this.radMultiColumnComboBoxArticulos.Location = new System.Drawing.Point(115, 12);
            this.radMultiColumnComboBoxArticulos.Name = "radMultiColumnComboBoxArticulos";
            this.radMultiColumnComboBoxArticulos.Size = new System.Drawing.Size(190, 24);
            this.radMultiColumnComboBoxArticulos.TabIndex = 8;
            this.radMultiColumnComboBoxArticulos.TabStop = false;
            // 
            // radMultiColumnComboBoxUbicacion
            // 
            this.radMultiColumnComboBoxUbicacion.AutoSizeDropDownHeight = true;
            this.radMultiColumnComboBoxUbicacion.AutoSizeDropDownToBestFit = true;
            // 
            // radMultiColumnComboBoxUbicacion.NestedRadGridView
            // 
            this.radMultiColumnComboBoxUbicacion.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radMultiColumnComboBoxUbicacion.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMultiColumnComboBoxUbicacion.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radMultiColumnComboBoxUbicacion.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.radMultiColumnComboBoxUbicacion.EditorControl.Name = "NestedRadGridView";
            this.radMultiColumnComboBoxUbicacion.EditorControl.ReadOnly = true;
            this.radMultiColumnComboBoxUbicacion.EditorControl.ShowGroupPanel = false;
            this.radMultiColumnComboBoxUbicacion.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radMultiColumnComboBoxUbicacion.EditorControl.TabIndex = 0;
            this.radMultiColumnComboBoxUbicacion.Location = new System.Drawing.Point(115, 38);
            this.radMultiColumnComboBoxUbicacion.Name = "radMultiColumnComboBoxUbicacion";
            this.radMultiColumnComboBoxUbicacion.Size = new System.Drawing.Size(190, 24);
            this.radMultiColumnComboBoxUbicacion.TabIndex = 9;
            this.radMultiColumnComboBoxUbicacion.TabStop = false;
            // 
            // FormularioCrearPaletMultireferencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 237);
            this.Controls.Add(this.radMultiColumnComboBoxUbicacion);
            this.Controls.Add(this.radMultiColumnComboBoxArticulos);
            this.Controls.Add(this.radComboBoxTipoPalet);
            this.Controls.Add(this.rumLabelPaletTipo);
            this.Controls.Add(this.rumLabelUbicacion);
            this.Controls.Add(this.rumLabelArticulo);
            this.Controls.Add(this.rumButtonCancelar);
            this.Controls.Add(this.rumButtonAceptar);
            this.MaximumSize = new System.Drawing.Size(350, 270);
            this.MinimumSize = new System.Drawing.Size(350, 270);
            this.Name = "FormularioCrearPaletMultireferencia";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(350, 270);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormularioCrearPaletMultireferencia";
            this.Load += new System.EventHandler(this.FormularioCrearPaletMultireferencia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelArticulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelUbicacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelPaletTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxTipoPalet.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxTipoPalet.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxTipoPalet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxArticulos.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxArticulos.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxArticulos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUbicacion.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUbicacion.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMultiColumnComboBoxUbicacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private RumButton rumButtonAceptar;
        private RumButton rumButtonCancelar;
        private RumLabel rumLabelArticulo;
        private RumLabel rumLabelUbicacion;
        private RumLabel rumLabelPaletTipo;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radComboBoxTipoPalet;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radMultiColumnComboBoxArticulos;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radMultiColumnComboBoxUbicacion;
    }
}

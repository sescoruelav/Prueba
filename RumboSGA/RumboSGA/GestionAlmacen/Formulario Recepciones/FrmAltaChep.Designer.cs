using Rumbo.Core.Herramientas;

namespace RumboSGA.GestionAlmacen.Formulario_Recepciones
{
    partial class FrmAltaChep
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rTxtCantidad = new Telerik.WinControls.UI.RadTextBox();
            this.rumLabel3 = new RumboSGA.RumLabel();
            this.rumLabel2 = new RumboSGA.RumLabel();
            this.rmccmbArticulo = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.rmccmbPedido = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.rumLabel1 = new RumboSGA.RumLabel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.btnCancelar = new RumboSGA.RumButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rTxtCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbArticulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbArticulo.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbArticulo.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbPedido)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbPedido.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbPedido.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rTxtCantidad, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.rumLabel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rumLabel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rmccmbArticulo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.rmccmbPedido, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rumLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 220);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rTxtCantidad
            // 
            this.rTxtCantidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rTxtCantidad.Location = new System.Drawing.Point(166, 113);
            this.rTxtCantidad.Name = "rTxtCantidad";
            this.rTxtCantidad.Size = new System.Drawing.Size(158, 49);
            this.rTxtCantidad.TabIndex = 14;
            this.rTxtCantidad.Text = "1000";
            // 
            // rumLabel3
            // 
            this.rumLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rumLabel3.Location = new System.Drawing.Point(3, 120);
            this.rumLabel3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rumLabel3.Name = "rumLabel3";
            this.rumLabel3.Size = new System.Drawing.Size(157, 42);
            this.rumLabel3.TabIndex = 13;
            this.rumLabel3.Text = "Cantidad";
            // 
            // rumLabel2
            // 
            this.rumLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rumLabel2.Location = new System.Drawing.Point(3, 65);
            this.rumLabel2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rumLabel2.Name = "rumLabel2";
            this.rumLabel2.Size = new System.Drawing.Size(157, 42);
            this.rumLabel2.TabIndex = 12;
            this.rumLabel2.Text = "Artículo";
            // 
            // rmccmbArticulo
            // 
            this.rmccmbArticulo.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // rmccmbArticulo.NestedRadGridView
            // 
            this.rmccmbArticulo.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.rmccmbArticulo.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbArticulo.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rmccmbArticulo.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rmccmbArticulo.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.rmccmbArticulo.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.rmccmbArticulo.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.rmccmbArticulo.EditorControl.MasterTemplate.EnableGrouping = false;
            this.rmccmbArticulo.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.rmccmbArticulo.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rmccmbArticulo.EditorControl.Name = "NestedRadGridView";
            this.rmccmbArticulo.EditorControl.ReadOnly = true;
            this.rmccmbArticulo.EditorControl.ShowGroupPanel = false;
            this.rmccmbArticulo.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rmccmbArticulo.EditorControl.TabIndex = 0;
            this.rmccmbArticulo.Location = new System.Drawing.Point(166, 65);
            this.rmccmbArticulo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rmccmbArticulo.Name = "rmccmbArticulo";
            this.rmccmbArticulo.Size = new System.Drawing.Size(158, 42);
            this.rmccmbArticulo.TabIndex = 11;
            this.rmccmbArticulo.TabStop = false;
            this.rmccmbArticulo.Text = "comboArticulo";
            // 
            // rmccmbPedido
            // 
            this.rmccmbPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // rmccmbPedido.NestedRadGridView
            // 
            this.rmccmbPedido.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.rmccmbPedido.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmccmbPedido.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rmccmbPedido.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rmccmbPedido.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.rmccmbPedido.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.rmccmbPedido.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.rmccmbPedido.EditorControl.MasterTemplate.EnableGrouping = false;
            this.rmccmbPedido.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.rmccmbPedido.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.rmccmbPedido.EditorControl.Name = "NestedRadGridView";
            this.rmccmbPedido.EditorControl.ReadOnly = true;
            this.rmccmbPedido.EditorControl.ShowGroupPanel = false;
            this.rmccmbPedido.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.rmccmbPedido.EditorControl.TabIndex = 0;
            this.rmccmbPedido.Location = new System.Drawing.Point(166, 10);
            this.rmccmbPedido.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rmccmbPedido.Name = "rmccmbPedido";
            this.rmccmbPedido.Size = new System.Drawing.Size(158, 42);
            this.rmccmbPedido.TabIndex = 10;
            this.rmccmbPedido.TabStop = false;
            this.rmccmbPedido.Text = "comboPedido";
            // 
            // rumLabel1
            // 
            this.rumLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rumLabel1.Location = new System.Drawing.Point(3, 10);
            this.rumLabel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.rumLabel1.Name = "rumLabel1";
            this.rumLabel1.Size = new System.Drawing.Size(157, 42);
            this.rumLabel1.TabIndex = 9;
            this.rumLabel1.Text = "Pedido";
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAceptar.Location = new System.Drawing.Point(3, 168);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(110, 24);
            this.btnAceptar.TabIndex = 6;
            this.btnAceptar.Text = Lenguaje.traduce("Aceptar"); 
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCancelar.Location = new System.Drawing.Point(166, 168);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 24);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = Lenguaje.traduce("Cancelar"); 
            // 
            // FrmAltaChep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(327, 220);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmAltaChep";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Alta Cheps";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rTxtCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbArticulo.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbArticulo.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbArticulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbPedido.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbPedido.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rmccmbPedido)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
        private RumLabel rumLabel1;
        private RumLabel rumLabel3;
        private RumLabel rumLabel2;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rmccmbArticulo;
        private Telerik.WinControls.UI.RadMultiColumnComboBox rmccmbPedido;
        private Telerik.WinControls.UI.RadTextBox rTxtCantidad;
    }
}

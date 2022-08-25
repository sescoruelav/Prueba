using RumboSGA.Properties;

namespace RumboSGA.Herramientas.Stock
{
    partial class CambiarAtributos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CambiarAtributos));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAceptar = new RumboSGA.RumButton();
            this.btnCancelar = new RumboSGA.RumButton();
            this.panelControles = new Telerik.WinControls.UI.RadPanel();
            this.opcionesReservaGroup = new Telerik.WinControls.UI.RadGroupBox();
            this.entradaReserva = new RumboSGA.RumRadioButton();
            this.entradaNormal = new RumboSGA.RumRadioButton();
            this.opcionesImpresionGroup = new Telerik.WinControls.UI.RadGroupBox();
            this.noImprimirEtiqueta = new RumboSGA.RumRadioButton();
            this.imprimirEtiqueta = new RumboSGA.RumRadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opcionesReservaGroup)).BeginInit();
            this.opcionesReservaGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entradaReserva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entradaNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opcionesImpresionGroup)).BeginInit();
            this.opcionesImpresionGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.noImprimirEtiqueta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imprimirEtiqueta)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnAceptar, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelar, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panelControles, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.opcionesReservaGroup, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.opcionesImpresionGroup, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.47191F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.47191F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.47191F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.47191F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.11236F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 223);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.btnAceptar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAceptar.Location = new System.Drawing.Point(3, 203);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(89, 17);
            this.btnAceptar.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.btnCancelar.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.Location = new System.Drawing.Point(98, 203);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(89, 17);
            this.btnCancelar.TabIndex = 1;
            // 
            // panelControles
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panelControles, 3);
            this.panelControles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControles.Location = new System.Drawing.Point(3, 3);
            this.panelControles.Name = "panelControles";
            this.panelControles.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.SetRowSpan(this.panelControles, 4);
            this.panelControles.Size = new System.Drawing.Size(580, 194);
            this.panelControles.TabIndex = 2;
            // 
            // opcionesReservaGroup
            // 
            this.opcionesReservaGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.opcionesReservaGroup.Controls.Add(this.entradaReserva);
            this.opcionesReservaGroup.Controls.Add(this.entradaNormal);
            this.opcionesReservaGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opcionesReservaGroup.HeaderText = "opcionesReservaGroup";
            this.opcionesReservaGroup.Location = new System.Drawing.Point(589, 3);
            this.opcionesReservaGroup.Name = "opcionesReservaGroup";
            this.tableLayoutPanel1.SetRowSpan(this.opcionesReservaGroup, 2);
            this.opcionesReservaGroup.Size = new System.Drawing.Size(142, 94);
            this.opcionesReservaGroup.TabIndex = 3;
            this.opcionesReservaGroup.Text = "opcionesReservaGroup";
            // 
            // entradaReserva
            // 
            this.entradaReserva.Location = new System.Drawing.Point(5, 45);
            this.entradaReserva.Name = "entradaReserva";
            this.entradaReserva.Size = new System.Drawing.Size(97, 18);
            this.entradaReserva.TabIndex = 1;
            this.entradaReserva.Text = "entradaReserva";
            // 
            // entradaNormal
            // 
            this.entradaNormal.Location = new System.Drawing.Point(5, 21);
            this.entradaNormal.Name = "entradaNormal";
            this.entradaNormal.Size = new System.Drawing.Size(96, 18);
            this.entradaNormal.TabIndex = 0;
            this.entradaNormal.Text = "entradaNormal";
            // 
            // opcionesImpresionGroup
            // 
            this.opcionesImpresionGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.opcionesImpresionGroup.Controls.Add(this.noImprimirEtiqueta);
            this.opcionesImpresionGroup.Controls.Add(this.imprimirEtiqueta);
            this.opcionesImpresionGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opcionesImpresionGroup.HeaderText = "opcionesImpresionGroup";
            this.opcionesImpresionGroup.Location = new System.Drawing.Point(589, 103);
            this.opcionesImpresionGroup.Name = "opcionesImpresionGroup";
            this.tableLayoutPanel1.SetRowSpan(this.opcionesImpresionGroup, 2);
            this.opcionesImpresionGroup.Size = new System.Drawing.Size(142, 94);
            this.opcionesImpresionGroup.TabIndex = 4;
            this.opcionesImpresionGroup.Text = "opcionesImpresionGroup";
            // 
            // noImprimirEtiqueta
            // 
            this.noImprimirEtiqueta.Location = new System.Drawing.Point(5, 45);
            this.noImprimirEtiqueta.Name = "noImprimirEtiqueta";
            this.noImprimirEtiqueta.Size = new System.Drawing.Size(117, 18);
            this.noImprimirEtiqueta.TabIndex = 1;
            this.noImprimirEtiqueta.Text = "noImprimirEtiqueta";
            // 
            // imprimirEtiqueta
            // 
            this.imprimirEtiqueta.Location = new System.Drawing.Point(5, 21);
            this.imprimirEtiqueta.Name = "imprimirEtiqueta";
            this.imprimirEtiqueta.Size = new System.Drawing.Size(103, 18);
            this.imprimirEtiqueta.TabIndex = 0;
            this.imprimirEtiqueta.Text = "imprimirEtiqueta";
            // 
            // CambiarAtributos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 223);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CambiarAtributos";
            this.Text = "CambiarAtributos";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opcionesReservaGroup)).EndInit();
            this.opcionesReservaGroup.ResumeLayout(false);
            this.opcionesReservaGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entradaReserva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entradaNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opcionesImpresionGroup)).EndInit();
            this.opcionesImpresionGroup.ResumeLayout(false);
            this.opcionesImpresionGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.noImprimirEtiqueta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imprimirEtiqueta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumButton btnAceptar;
        private RumButton btnCancelar;
        private Telerik.WinControls.UI.RadPanel panelControles;
        private Telerik.WinControls.UI.RadGroupBox opcionesImpresionGroup;
        private Telerik.WinControls.UI.RadGroupBox opcionesReservaGroup;
        private RumRadioButton entradaReserva;
        private RumRadioButton entradaNormal;
        private RumRadioButton noImprimirEtiqueta;
        private RumRadioButton imprimirEtiqueta;
    }
}

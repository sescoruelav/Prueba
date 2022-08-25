using RumboSGA.Properties;

namespace RumboSGA.Herramientas
{
    partial class InventarioInicial
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnImprimir = new RumboSGA.RumButton();
            this.btnSalir = new RumboSGA.RumButton();
            this.btnLimpiar = new RumboSGA.RumButton();
            this.btnValidar = new RumboSGA.RumButton();
            this.radLabel1 = new RumboSGA.RumLabel();
            this.txtNumEtiquetas = new Telerik.WinControls.UI.RadTextBox();
            this.topLabel = new RumboSGA.RumLabel();
            this.titleBarControl = new RumboSGA.Presentation.UserControls.TitleBarControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnImprimir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLimpiar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnValidar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnImprimir, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSalir, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnLimpiar, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnValidar, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.radLabel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtNumEtiquetas, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.topLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 57);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(292, 213);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnImprimir
            // 
            this.btnImprimir.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnImprimir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Location = new System.Drawing.Point(3, 87);
            this.btnImprimir.Name = "btnImprimir";
            // 
            // 
            // 
            this.btnImprimir.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 87, 110, 24);
            this.btnImprimir.Size = new System.Drawing.Size(140, 36);
            this.btnImprimir.TabIndex = 0;
            this.btnImprimir.Text = global::RumboSGA.Properties.strings.Imprimir;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSalir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Location = new System.Drawing.Point(149, 87);
            this.btnSalir.Name = "btnSalir";
            // 
            // 
            // 
            this.btnSalir.RootElement.ControlBounds = new System.Drawing.Rectangle(149, 87, 110, 24);
            this.btnSalir.Size = new System.Drawing.Size(140, 36);
            this.btnSalir.TabIndex = 1;
            this.btnSalir.Text = global::RumboSGA.Properties.strings.Salir;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnLimpiar, 2);
            this.btnLimpiar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.Location = new System.Drawing.Point(3, 129);
            this.btnLimpiar.Name = "btnLimpiar";
            // 
            // 
            // 
            this.btnLimpiar.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 129, 110, 24);
            this.btnLimpiar.Size = new System.Drawing.Size(286, 36);
            this.btnLimpiar.TabIndex = 2;
            this.btnLimpiar.Text = global::RumboSGA.Properties.strings.LimpiarInvInicial;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnValidar
            // 
            this.btnValidar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel1.SetColumnSpan(this.btnValidar, 2);
            this.btnValidar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnValidar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidar.Location = new System.Drawing.Point(3, 171);
            this.btnValidar.Name = "btnValidar";
            // 
            // 
            // 
            this.btnValidar.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 171, 110, 24);
            this.btnValidar.Size = new System.Drawing.Size(286, 39);
            this.btnValidar.TabIndex = 3;
            this.btnValidar.Text = global::RumboSGA.Properties.strings.ValidarPrimeraPasada;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.Location = new System.Drawing.Point(3, 45);
            this.radLabel1.Name = "radLabel1";
            // 
            // 
            // 
            this.radLabel1.RootElement.ControlBounds = new System.Drawing.Rectangle(3, 45, 100, 18);
            this.radLabel1.Size = new System.Drawing.Size(140, 36);
            this.radLabel1.TabIndex = 5;
            this.radLabel1.Text = "Nº Etiquetas";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNumEtiquetas
            // 
            this.txtNumEtiquetas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNumEtiquetas.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtNumEtiquetas.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumEtiquetas.Location = new System.Drawing.Point(149, 53);
            this.txtNumEtiquetas.Name = "txtNumEtiquetas";
            // 
            // 
            // 
            this.txtNumEtiquetas.RootElement.ControlBounds = new System.Drawing.Rectangle(149, 53, 100, 20);
            this.txtNumEtiquetas.RootElement.StretchVertically = true;
            this.txtNumEtiquetas.Size = new System.Drawing.Size(140, 20);
            this.txtNumEtiquetas.TabIndex = 7;
            // 
            // topLabel
            // 
            this.topLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.topLabel.AutoSize = false;
            this.topLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.topLabel.BorderVisible = true;
            this.tableLayoutPanel1.SetColumnSpan(this.topLabel, 2);
            this.topLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topLabel.Location = new System.Drawing.Point(46, 12);
            this.topLabel.Name = "topLabel";
            // 
            // 
            // 
            this.topLabel.RootElement.ControlBounds = new System.Drawing.Rectangle(46, 12, 100, 18);
            this.topLabel.Size = new System.Drawing.Size(200, 18);
            this.topLabel.TabIndex = 6;
            this.topLabel.Text = "Etiquetas para Inventario Inicial";
            this.topLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleBarControl
            // 
            this.titleBarControl.AllowDrop = true;
            this.titleBarControl.BackColor = System.Drawing.SystemColors.Window;
            this.titleBarControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.titleBarControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarControl.Location = new System.Drawing.Point(0, 0);
            this.titleBarControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.titleBarControl.Name = "titleBarControl";
            this.titleBarControl.Size = new System.Drawing.Size(292, 57);
            this.titleBarControl.TabIndex = 0;
            // 
            // InventarioInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(292, 270);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.titleBarControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(400, 400);
            this.Name = "InventarioInicial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MenuInventario";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnImprimir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLimpiar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnValidar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topLabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RumboSGA.RumButton btnImprimir;
        private RumboSGA.RumButton btnSalir;
        private RumboSGA.RumButton btnLimpiar;
        private RumboSGA.RumButton btnValidar;
        private RumboSGA.RumLabel radLabel1;
        private RumboSGA.RumLabel topLabel;
        private Telerik.WinControls.UI.RadTextBox txtNumEtiquetas;
        private Presentation.UserControls.TitleBarControl titleBarControl;
    }
}

namespace RumboSGA.Presentation
{
    partial class SFDetallesN
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
            this.tableLayoutPanelPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.rumLabelTitulo = new RumboSGA.RumLabel();
            this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumButtonEditar = new RumboSGA.RumButton();
            this.titleBarControlRumbo = new RumboSGA.Presentation.UserControls.TitleBarControl();
            this.tableLayoutPanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTitulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonEditar)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPrincipal
            // 
            this.tableLayoutPanelPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelPrincipal.ColumnCount = 1;
            this.tableLayoutPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPrincipal.Controls.Add(this.panelPrincipal, 0, 0);
            this.tableLayoutPanelPrincipal.Location = new System.Drawing.Point(12, 126);
            this.tableLayoutPanelPrincipal.Name = "tableLayoutPanelPrincipal";
            this.tableLayoutPanelPrincipal.RowCount = 1;
            this.tableLayoutPanelPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPrincipal.Size = new System.Drawing.Size(862, 364);
            this.tableLayoutPanelPrincipal.TabIndex = 5;
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.AutoScroll = true;
            this.panelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrincipal.Location = new System.Drawing.Point(3, 3);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Size = new System.Drawing.Size(856, 358);
            this.panelPrincipal.TabIndex = 0;
            // 
            // rumLabelTitulo
            // 
            this.rumLabelTitulo.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rumLabelTitulo.Location = new System.Drawing.Point(92, 70);
            this.rumLabelTitulo.Name = "rumLabelTitulo";
            this.rumLabelTitulo.Size = new System.Drawing.Size(124, 44);
            this.rumLabelTitulo.TabIndex = 6;
            this.rumLabelTitulo.Text = "Detalles";
            // 
            // rumButtonAceptar
            // 
            this.rumButtonAceptar.BackColor = System.Drawing.Color.Transparent;
            this.rumButtonAceptar.Dock = System.Windows.Forms.DockStyle.Right;
            this.rumButtonAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.rumButtonAceptar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.rumButtonAceptar.Location = new System.Drawing.Point(738, 57);
            this.rumButtonAceptar.Margin = new System.Windows.Forms.Padding(10);
            this.rumButtonAceptar.MaximumSize = new System.Drawing.Size(74, 60);
            this.rumButtonAceptar.Name = "rumButtonAceptar";
            // 
            // 
            // 
            this.rumButtonAceptar.RootElement.MaxSize = new System.Drawing.Size(74, 60);
            this.rumButtonAceptar.Size = new System.Drawing.Size(74, 60);
            this.rumButtonAceptar.TabIndex = 3;
            this.rumButtonAceptar.Text = "Aceptar";
            this.rumButtonAceptar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonAceptar.Click += new System.EventHandler(this.RumButtonAceptar_Click);
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.BackColor = System.Drawing.Color.Transparent;
            this.rumButtonCancelar.Dock = System.Windows.Forms.DockStyle.Right;
            this.rumButtonCancelar.Image = global::RumboSGA.Properties.Resources.Cancel;
            this.rumButtonCancelar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.rumButtonCancelar.Location = new System.Drawing.Point(812, 57);
            this.rumButtonCancelar.Margin = new System.Windows.Forms.Padding(10);
            this.rumButtonCancelar.MaximumSize = new System.Drawing.Size(74, 60);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            // 
            // 
            // 
            this.rumButtonCancelar.RootElement.MaxSize = new System.Drawing.Size(74, 60);
            this.rumButtonCancelar.Size = new System.Drawing.Size(74, 60);
            this.rumButtonCancelar.TabIndex = 2;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // rumButtonEditar
            // 
            this.rumButtonEditar.BackColor = System.Drawing.Color.Transparent;
            this.rumButtonEditar.Image = global::RumboSGA.Properties.Resources.edit;
            this.rumButtonEditar.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.rumButtonEditar.Location = new System.Drawing.Point(12, 60);
            this.rumButtonEditar.Name = "rumButtonEditar";
            this.rumButtonEditar.Size = new System.Drawing.Size(74, 60);
            this.rumButtonEditar.TabIndex = 1;
            this.rumButtonEditar.Text = "Editar";
            this.rumButtonEditar.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.rumButtonEditar.Click += new System.EventHandler(this.RumButtonEditar_Click);
            // 
            // titleBarControlRumbo
            // 
            this.titleBarControlRumbo.AllowDrop = true;
            this.titleBarControlRumbo.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarControlRumbo.Location = new System.Drawing.Point(0, 0);
            this.titleBarControlRumbo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.titleBarControlRumbo.Name = "titleBarControlRumbo";
            this.titleBarControlRumbo.Size = new System.Drawing.Size(886, 57);
            this.titleBarControlRumbo.TabIndex = 0;
            // 
            // SFDetallesN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 502);
            this.Controls.Add(this.rumLabelTitulo);
            this.Controls.Add(this.tableLayoutPanelPrincipal);
            this.Controls.Add(this.rumButtonAceptar);
            this.Controls.Add(this.rumButtonCancelar);
            this.Controls.Add(this.rumButtonEditar);
            this.Controls.Add(this.titleBarControlRumbo);
            this.Name = "SFDetallesN";
            this.Text = "SFDetallesN";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SFDetallesN_FormClosed);
            this.Load += new System.EventHandler(this.SFDetallesN_Load);
            this.tableLayoutPanelPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTitulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonEditar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private UserControls.TitleBarControl titleBarControlRumbo;
        private RumButton rumButtonEditar;
        private RumButton rumButtonCancelar;
        private RumButton rumButtonAceptar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPrincipal;
        private RumLabel rumLabelTitulo;
        private System.Windows.Forms.Panel panelPrincipal;
    }
}

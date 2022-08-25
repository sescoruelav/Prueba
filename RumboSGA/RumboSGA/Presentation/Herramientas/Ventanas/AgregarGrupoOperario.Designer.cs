namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class AgregarGrupoOperario
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
            this.radPanelPrincipal = new Telerik.WinControls.UI.RadPanel();
            this.descripcionGrupoTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.descripcionGrupoLabel = new Telerik.WinControls.UI.RadLabel();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumButtonAlta = new RumboSGA.RumButton();
            this.rumLabel1 = new RumboSGA.RumLabel();
            this.idGrupoTextBox = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).BeginInit();
            this.radPanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.descripcionGrupoTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.descripcionGrupoLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idGrupoTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelPrincipal
            // 
            this.radPanelPrincipal.Controls.Add(this.idGrupoTextBox);
            this.radPanelPrincipal.Controls.Add(this.rumLabel1);
            this.radPanelPrincipal.Controls.Add(this.descripcionGrupoTextBox);
            this.radPanelPrincipal.Controls.Add(this.descripcionGrupoLabel);
            this.radPanelPrincipal.Controls.Add(this.rumButtonCancelar);
            this.radPanelPrincipal.Controls.Add(this.rumButtonAlta);
            this.radPanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.radPanelPrincipal.Name = "radPanelPrincipal";
            this.radPanelPrincipal.Size = new System.Drawing.Size(367, 189);
            this.radPanelPrincipal.TabIndex = 12;
            this.radPanelPrincipal.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelPrincipal.GetChildAt(0))).BorderHighlightThickness = 1;
            // 
            // descripcionGrupoTextBox
            // 
            this.descripcionGrupoTextBox.Location = new System.Drawing.Point(146, 49);
            this.descripcionGrupoTextBox.Name = "descripcionGrupoTextBox";
            this.descripcionGrupoTextBox.Size = new System.Drawing.Size(125, 24);
            this.descripcionGrupoTextBox.TabIndex = 49;
            // 
            // descripcionGrupoLabel
            // 
            this.descripcionGrupoLabel.Location = new System.Drawing.Point(39, 49);
            this.descripcionGrupoLabel.Name = "descripcionGrupoLabel";
            this.descripcionGrupoLabel.Size = new System.Drawing.Size(101, 18);
            this.descripcionGrupoLabel.TabIndex = 48;
            this.descripcionGrupoLabel.Text = "Nombre del Grupo";
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(216, 133);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonCancelar.TabIndex = 47;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // rumButtonAlta
            // 
            this.rumButtonAlta.Location = new System.Drawing.Point(39, 133);
            this.rumButtonAlta.Name = "rumButtonAlta";
            this.rumButtonAlta.Size = new System.Drawing.Size(110, 24);
            this.rumButtonAlta.TabIndex = 46;
            this.rumButtonAlta.Text = "Aceptar";
            this.rumButtonAlta.Click += new System.EventHandler(this.RumButtonAlta_Click);
            // 
            // rumLabel1
            // 
            this.rumLabel1.Location = new System.Drawing.Point(39, 13);
            this.rumLabel1.Name = "rumLabel1";
            this.rumLabel1.Size = new System.Drawing.Size(69, 18);
            this.rumLabel1.TabIndex = 50;
            this.rumLabel1.Text = "Id del Grupo";
            // 
            // idGrupoTextBox
            // 
            this.idGrupoTextBox.Location = new System.Drawing.Point(146, 13);
            this.idGrupoTextBox.Name = "idGrupoTextBox";
            this.idGrupoTextBox.Size = new System.Drawing.Size(125, 24);
            this.idGrupoTextBox.TabIndex = 51;
            // 
            // AgregarGrupoOperario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(367, 189);
            this.Controls.Add(this.radPanelPrincipal);
            this.MaximumSize = new System.Drawing.Size(667, 543);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "AgregarGrupoOperario";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(667, 543);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agregar Grupo";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).EndInit();
            this.radPanelPrincipal.ResumeLayout(false);
            this.radPanelPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.descripcionGrupoTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.descripcionGrupoLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idGrupoTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPanel radPanelPrincipal;
        private RumButton rumButtonCancelar;
        private RumButton rumButtonAlta;
        private Telerik.WinControls.UI.RadTextBox descripcionGrupoTextBox;
        private Telerik.WinControls.UI.RadLabel descripcionGrupoLabel;
        private Telerik.WinControls.UI.RadTextBox idGrupoTextBox;
        private RumLabel rumLabel1;
    }
}

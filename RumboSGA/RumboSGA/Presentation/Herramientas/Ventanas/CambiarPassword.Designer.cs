namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class CambiarPassword
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
            this.nuevaPassword = new Telerik.WinControls.UI.RadLabel();
            this.nuevaPasswordTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumButtonAlta = new RumboSGA.RumButton();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).BeginInit();
            this.radPanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuevaPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuevaPasswordTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelPrincipal
            // 
            this.radPanelPrincipal.Controls.Add(this.nuevaPassword);
            this.radPanelPrincipal.Controls.Add(this.nuevaPasswordTextBox);
            this.radPanelPrincipal.Controls.Add(this.rumButtonCancelar);
            this.radPanelPrincipal.Controls.Add(this.rumButtonAlta);
            this.radPanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.radPanelPrincipal.Name = "radPanelPrincipal";
            this.radPanelPrincipal.Size = new System.Drawing.Size(366, 189);
            this.radPanelPrincipal.TabIndex = 12;
            this.radPanelPrincipal.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelPrincipal.GetChildAt(0))).BorderHighlightThickness = 1;
            // 
            // nuevaPassword
            // 
            this.nuevaPassword.Location = new System.Drawing.Point(39, 48);
            this.nuevaPassword.Name = "nuevaPassword";
            this.nuevaPassword.Size = new System.Drawing.Size(96, 18);
            this.nuevaPassword.TabIndex = 51;
            this.nuevaPassword.Text = "Nueva contraseña";
            // 
            // nuevaPasswordTextBox
            // 
            this.nuevaPasswordTextBox.Location = new System.Drawing.Point(197, 48);
            this.nuevaPasswordTextBox.Name = "nuevaPasswordTextBox";
            this.nuevaPasswordTextBox.PasswordChar = '*';
            this.nuevaPasswordTextBox.Size = new System.Drawing.Size(125, 20);
            this.nuevaPasswordTextBox.TabIndex = 49;
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(212, 133);
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
            // CambiarPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(366, 189);
            this.Controls.Add(this.radPanelPrincipal);
            this.MaximumSize = new System.Drawing.Size(667, 543);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "CambiarPassword";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(667, 543);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cambiar Contraseña";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).EndInit();
            this.radPanelPrincipal.ResumeLayout(false);
            this.radPanelPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuevaPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuevaPasswordTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPanel radPanelPrincipal;
        private RumButton rumButtonCancelar;
        private RumButton rumButtonAlta;
        private Telerik.WinControls.UI.RadLabel nuevaPassword;
        private Telerik.WinControls.UI.RadTextBox nuevaPasswordTextBox;
    }
}

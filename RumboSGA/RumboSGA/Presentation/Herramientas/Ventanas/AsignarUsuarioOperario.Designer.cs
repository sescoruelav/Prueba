
namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class AsignarUsuarioOperario
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
            this.operarioLabel = new Telerik.WinControls.UI.RadLabel();
            this.nombreUsuarioLabel = new Telerik.WinControls.UI.RadLabel();
            this.operarioTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.UsuarioTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumButtonAlta = new RumboSGA.RumButton();

            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).BeginInit();
            this.radPanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operarioLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nombreUsuarioLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operarioTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsuarioTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).BeginInit();
            // 
            // radPanelPrincipal
            // 
            this.radPanelPrincipal.Controls.Add(this.operarioLabel);
            this.radPanelPrincipal.Controls.Add(this.nombreUsuarioLabel);
            this.radPanelPrincipal.Controls.Add(this.operarioTextBox);
            this.radPanelPrincipal.Controls.Add(this.UsuarioTextBox);
            this.radPanelPrincipal.Controls.Add(this.rumButtonCancelar);
            this.radPanelPrincipal.Controls.Add(this.rumButtonAlta);
            this.radPanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.radPanelPrincipal.Name = "radPanelPrincipal";
            this.radPanelPrincipal.Size = new System.Drawing.Size(366, 189);
            this.radPanelPrincipal.TabIndex = 12;
            this.radPanelPrincipal.ThemeName = "ControlDefault";
            this.radPanelPrincipal.Paint += new System.Windows.Forms.PaintEventHandler(this.RadPanelPrincipal_Paint);
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelPrincipal.GetChildAt(0))).BorderHighlightThickness = 1;
            // 
            // passwordLabel
            // 
            this.operarioLabel.Location = new System.Drawing.Point(64, 84);
            this.operarioLabel.Name = "operarioLabel";
            this.operarioLabel.Size = new System.Drawing.Size(63, 18);
            this.operarioLabel.TabIndex = 50;
            this.operarioLabel.Text = "Operario";
            // 
            // nombreUsuarioLabel
            // 
            this.nombreUsuarioLabel.Location = new System.Drawing.Point(64, 49);
            this.nombreUsuarioLabel.Name = "nombreUsuarioLabel";
            this.nombreUsuarioLabel.Size = new System.Drawing.Size(44, 18);
            this.nombreUsuarioLabel.TabIndex = 49;
            this.nombreUsuarioLabel.Text = "Usuario";
            // 
            // passwordTextBox
            // 
            this.operarioTextBox.Location = new System.Drawing.Point(134, 84);
            this.operarioTextBox.Name = "operarioTextBox";
            this.operarioTextBox.Size = new System.Drawing.Size(125, 24);
            this.operarioTextBox.TabIndex = 48;
            // 
            // UsuarioTextBox
            // 
            this.UsuarioTextBox.Location = new System.Drawing.Point(134, 49);
            this.UsuarioTextBox.Name = "UsuarioTextBox";
            this.UsuarioTextBox.Size = new System.Drawing.Size(125, 24);
            this.UsuarioTextBox.TabIndex = 1;
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(211, 133);
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

            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(366, 189);
            this.Controls.Add(this.radPanelPrincipal);
            this.MaximumSize = new System.Drawing.Size(667, 543);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "AsignarUsuarioOperario";


            //this.RootElement.ApplyShapeToControl = true;
           // this.RootElement.MaxSize = new System.Drawing.Size(667, 543);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "asignar Usuario";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).EndInit();
            this.radPanelPrincipal.ResumeLayout(false);
            this.radPanelPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operarioLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nombreUsuarioLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operarioTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UsuarioTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).EndInit();
            this.ResumeLayout(false);
        }

        private Telerik.WinControls.UI.RadPanel radPanelPrincipal;
        private RumButton rumButtonCancelar;
        private RumButton rumButtonAlta;
        private Telerik.WinControls.UI.RadLabel operarioLabel;
        private Telerik.WinControls.UI.RadLabel nombreUsuarioLabel;
        private Telerik.WinControls.UI.RadTextBox operarioTextBox;
        private Telerik.WinControls.UI.RadTextBox UsuarioTextBox;
        #endregion
    }
}
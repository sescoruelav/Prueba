
using System.Drawing;

namespace RumboSGA.Controles
{
    partial class SFDetalle
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

            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.panelCabezera = new System.Windows.Forms.Panel();
            this.panelDatos = new System.Windows.Forms.Panel();
           /* this.rumButtonAceptar = new RumboSGA.RumButton();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumButtonEditar = new RumboSGA.RumButton();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonEditar)).BeginInit();
           */


            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "SFDetalle";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.Controls.Add(this.panelCabezera);
          //  this.panelPrincipal.Controls.Add(this.panelDatos);
            this.panelPrincipal.Location = new System.Drawing.Point(3, 3);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill; 
           // this.panelPrincipal.Size = new System.Drawing.Size(856, 358);
            this.panelPrincipal.TabIndex = 0;
            this.panelPrincipal.AutoScroll = true;
            this.panelPrincipal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;


            //  panelCabezera.BringToFront();

        }
        /*
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
    }
                    panelCabezera.BringToFront();


    private RumButton rumButtonEditar;
    private RumButton rumButtonCancelar;
    private RumButton rumButtonAceptar;*/

        private System.Windows.Forms.Panel panelPrincipal;
      //  private System.Windows.Forms.Panel panelCabezera;

        #endregion
    }
}
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    partial class EntreFechas
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
            this.dateTimeLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dateTimeLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.dateTimePicker1 = new Telerik.WinControls.UI.RadDateTimePicker();
            this.dateTimePicker2 = new Telerik.WinControls.UI.RadDateTimePicker();
            this.btnAceptar = new Telerik.WinControls.UI.RadButton();
            this.btnSalir = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimeLabel1
            // 
            this.dateTimeLabel1.Location = new System.Drawing.Point(12, 10);
            this.dateTimeLabel1.Name = "dateTimeLabel1";
            this.dateTimeLabel1.Size = new System.Drawing.Size(205, 24);
            // 
            // dateTimeLabel2
            // 
            this.dateTimeLabel2.Location = new System.Drawing.Point(12, 40);
            this.dateTimeLabel2.Name = "dateTimeLabel2";
            this.dateTimeLabel2.Size = new System.Drawing.Size(205, 24);
            
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(100, 10);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(205, 24);
            this.dateTimePicker1.TabIndex = 0;
            this.dateTimePicker1.TabStop = false;
            this.dateTimePicker1.Value = new System.DateTime();
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(100, 40);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(205, 24);
            this.dateTimePicker2.TabIndex = 0;
            this.dateTimePicker2.TabStop = false;
            this.dateTimePicker2.Value = new System.DateTime();
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(12, 70);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(137, 30);
            this.btnAceptar.TabIndex = 1;
            this.btnAceptar.Text = "Aceptar";

            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(168, 70);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(137, 30);
            this.btnSalir.TabIndex = 2;
            this.btnSalir.Text = "Salir";
            // 
            // PorFechaCreacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(330, 160);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimeLabel1);
            this.Controls.Add(this.dateTimeLabel2);
            this.Name = "PorFechaCreacion";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PorFechaPrevEnvio";
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimePicker2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSalir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private RadLabel dateTimeLabel1;
        private RadLabel dateTimeLabel2;

        #endregion

        private Telerik.WinControls.UI.RadDateTimePicker dateTimePicker1;
        private Telerik.WinControls.UI.RadDateTimePicker dateTimePicker2;
        private Telerik.WinControls.UI.RadButton btnAceptar;
        private Telerik.WinControls.UI.RadButton btnSalir;
    }
}
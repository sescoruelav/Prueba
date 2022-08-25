namespace RumboSGA.Presentation.Herramientas
{
    partial class EnCurso
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
            this.radDiagram1 = new Telerik.WinControls.UI.RadDiagram();
            ((System.ComponentModel.ISupportInitialize)(this.radDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radDiagram1
            // 
            this.radDiagram1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDiagram1.Location = new System.Drawing.Point(0, 0);
            this.radDiagram1.Name = "radDiagram1";
            this.radDiagram1.Size = new System.Drawing.Size(1146, 597);
            this.radDiagram1.TabIndex = 0;
            this.radDiagram1.Text = "radDiagram1";
            this.radDiagram1.Click += new System.EventHandler(this.radDiagram1_Click);
            // 
            // EnCurso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1146, 597);
            this.Controls.Add(this.radDiagram1);
            this.Name = "EnCurso";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "EnCurso";
            ((System.ComponentModel.ISupportInitialize)(this.radDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadDiagram radDiagram1;
    }
}

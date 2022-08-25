
namespace RumboSGA.Controles
{
    partial class RumMessageBox
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
            this.radTextBoxControl1 = new Telerik.WinControls.UI.RadTextBoxControl();
            this.rumButton1 = new RumboSGA.RumButton();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxControl1
            // 
            this.radTextBoxControl1.Location = new System.Drawing.Point(12, 12);
            this.radTextBoxControl1.Name = "radTextBoxControl1";
            this.radTextBoxControl1.Size = new System.Drawing.Size(424, 479);
            this.radTextBoxControl1.TabIndex = 1;
            // 
            // rumButton1
            // 
            this.rumButton1.Location = new System.Drawing.Point(159, 504);
            this.rumButton1.Name = "rumButton1";
            this.rumButton1.Size = new System.Drawing.Size(137, 30);
            this.rumButton1.TabIndex = 0;
            this.rumButton1.Text = "Ok";
            this.rumButton1.Click += new System.EventHandler(this.rumButton1_Click);
            // 
            // RumMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 546);
            this.Controls.Add(this.radTextBoxControl1);
            this.Controls.Add(this.rumButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RumMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RumMessageBox";
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButton1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RumButton rumButton1;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControl1;
    }
}
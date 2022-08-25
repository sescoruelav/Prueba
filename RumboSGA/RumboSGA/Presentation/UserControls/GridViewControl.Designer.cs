namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    partial class GridViewControl
    {
        ///  
        /// Required designer variable. 
        ///  
        private System.ComponentModel.IContainer components = null;

        ///  
        /// Clean up any resources being used. 
        ///  
        /// true if managed resources should be disposed; otherwise, false. 
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code 

        ///  
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor. 
        ///  
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.gridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // gridViewArticulos
            // 
            this.gridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridView.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.gridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridView.Name = "gridViewArticulos";
            this.gridView.Size = new System.Drawing.Size(1276, 848);
            this.gridView.TabIndex = 0;
            // 
            // GridViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            //| System.Windows.Forms.AnchorStyles.Left)
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.Controls.Add(this.gridView);
            this.Name = "GridViewControl";
            this.Size = new System.Drawing.Size(1036, 698);
            ((System.ComponentModel.ISupportInitialize)(this.gridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);

        }

        private Telerik.WinControls.UI.RadGridView gridView;
        #endregion
    }
}
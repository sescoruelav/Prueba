
namespace XML_Edit
{
    partial class RadForm1
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radButtonTextBox1 = new Telerik.WinControls.UI.RadButtonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            this.radGridView1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView1
            // 
            this.radGridView1.Controls.Add(this.radButtonTextBox1);
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(688, 435);
            this.radGridView1.TabIndex = 0;
            // 
            // radButtonTextBox1
            // 
            this.radButtonTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radButtonTextBox1.Location = new System.Drawing.Point(0, 0);
            this.radButtonTextBox1.Name = "radButtonTextBox1";
            this.radButtonTextBox1.Size = new System.Drawing.Size(1032, 47);
            this.radButtonTextBox1.TabIndex = 1;
            this.radButtonTextBox1.Text = "guardar";
            this.radButtonTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.radButtonTextBox1.TextChanged += new System.EventHandler(this.radButtonTextBox1_TextChanged);
            // 
            // RadForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 435);
            this.Controls.Add(this.radGridView1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RadForm1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "RadForm1";
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            this.radGridView1.ResumeLayout(false);
            this.radGridView1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButtonTextBox radButtonTextBox1;
    }
}
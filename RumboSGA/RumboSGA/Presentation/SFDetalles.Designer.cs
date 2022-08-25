using System;

namespace RumboSGA.Presentation
{
    partial class SFDetalles
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
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1_Detalle = new System.Windows.Forms.TableLayoutPanel();
            this.dataDialog1 = new RumboSGA.UserControls.DataDialog();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
            this.waitingBarIndicatorElement2 = new Telerik.WinControls.UI.WaitingBarIndicatorElement();
            this.waitingBarIndicatorElement1 = new Telerik.WinControls.UI.WaitingBarIndicatorElement();
            this.myBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.titleBarControl1 = new RumboSGA.Presentation.UserControls.TitleBarControl();
            this.tableLayoutPanel1_Detalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1_Detalle
            // 
            this.tableLayoutPanel1_Detalle.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1_Detalle.ColumnCount = 1;
            this.tableLayoutPanel1_Detalle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1_Detalle.Controls.Add(this.dataDialog1, 0, 0);
            this.tableLayoutPanel1_Detalle.Controls.Add(this.panelFooter, 0, 2);
            this.tableLayoutPanel1_Detalle.Controls.Add(this.radWaitingBar1, 0, 1);
            this.tableLayoutPanel1_Detalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1_Detalle.Location = new System.Drawing.Point(0, 57);
            this.tableLayoutPanel1_Detalle.Name = "tableLayoutPanel1_Detalle";
            this.tableLayoutPanel1_Detalle.RowCount = 3;
            this.tableLayoutPanel1_Detalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1_Detalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1_Detalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1_Detalle.Size = new System.Drawing.Size(886, 445);
            this.tableLayoutPanel1_Detalle.TabIndex = 2;
            // 
            // dataDialog1
            // 
            this.dataDialog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataDialog1.Location = new System.Drawing.Point(4, 4);
            this.dataDialog1.Name = "dataDialog1";
            this.dataDialog1.Size = new System.Drawing.Size(878, 385);
            this.dataDialog1.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(53)))), ((int)(((byte)(54)))));
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(4, 427);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(878, 14);
            this.panelFooter.TabIndex = 1;
            // 
            // radWaitingBar1
            // 
            this.radWaitingBar1.Location = new System.Drawing.Point(4, 396);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.Size = new System.Drawing.Size(681, 24);
            this.radWaitingBar1.TabIndex = 2;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.WaitingIndicators.Add(this.waitingBarIndicatorElement2);
            this.radWaitingBar1.WaitingIndicators.Add(this.waitingBarIndicatorElement1);
            // 
            // waitingBarIndicatorElement2
            // 
            this.waitingBarIndicatorElement2.Name = "waitingBarIndicatorElement2";
            this.waitingBarIndicatorElement2.StretchHorizontally = false;
            // 
            // waitingBarIndicatorElement1
            // 
            this.waitingBarIndicatorElement1.Name = "waitingBarIndicatorElement1";
            this.waitingBarIndicatorElement1.StretchHorizontally = false;
            // 
            // titleBarControl1
            // 
            this.titleBarControl1.AllowDrop = true;
            this.titleBarControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(53)))), ((int)(((byte)(54)))));
            this.titleBarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBarControl1.Location = new System.Drawing.Point(0, 0);
            this.titleBarControl1.Name = "titleBarControl1";
            this.titleBarControl1.Size = new System.Drawing.Size(886, 57);
            this.titleBarControl1.TabIndex = 0;
            // 
            // SFDetalles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 502);
            this.Controls.Add(this.tableLayoutPanel1_Detalle);
            this.Controls.Add(this.titleBarControl1);
            this.Name = "SFDetalles";
            this.Text = "SFDetalles";
            this.tableLayoutPanel1_Detalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion
        //private Telerik.WinControls.RoundRectShape roundRectShapeForm;
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanel1_Detalle;
        protected RumboSGA.UserControls.DataDialog dataDialog1;
        private System.Windows.Forms.Panel panelFooter;
        private System.ComponentModel.BackgroundWorker myBackgroundWorker;
        protected Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
        private Telerik.WinControls.UI.WaitingBarIndicatorElement waitingBarIndicatorElement2;
        private Telerik.WinControls.UI.WaitingBarIndicatorElement waitingBarIndicatorElement1;
        private UserControls.TitleBarControl titleBarControl1;
    }
}

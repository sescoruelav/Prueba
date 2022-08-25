
namespace RumboSGA.Herramientas
{
    partial class FrmVisorInformes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVisorInformes));
            Telerik.Reporting.UriReportSource uriReportSource1 = new Telerik.Reporting.UriReportSource();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.crViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.tlkViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            this.radCollapsiblePanel1 = new Telerik.WinControls.UI.RadCollapsiblePanel();
            this.rMenuInformes = new Telerik.WinControls.UI.RadTreeView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).BeginInit();
            this.radCollapsiblePanel1.PanelContainer.SuspendLayout();
            this.radCollapsiblePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rMenuInformes)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Controls.Add(this.crViewer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tlkViewer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radCollapsiblePanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1083, 587);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // crViewer
            // 
            this.crViewer.ActiveViewIndex = -1;
            this.crViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.crViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crViewer.Location = new System.Drawing.Point(219, 570);
            this.crViewer.Name = "crViewer";
            this.crViewer.ShowLogo = false;
            this.crViewer.Size = new System.Drawing.Size(861, 14);
            this.crViewer.TabIndex = 7;
            // 
            // tlkViewer
            // 
            this.tlkViewer.AccessibilityKeyMap = ((System.Collections.Generic.Dictionary<int, Telerik.ReportViewer.Common.Accessibility.ShortcutKeys>)(resources.GetObject("tlkViewer.AccessibilityKeyMap")));
            this.tlkViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlkViewer.EnableAccessibility = true;
            this.tlkViewer.Location = new System.Drawing.Point(219, 3);
            this.tlkViewer.Name = "tlkViewer";
            this.tlkViewer.ReportEngineConnection = "engine=Embedded";
            uriReportSource1.Uri = "Reports/Oleada.trdp";
            this.tlkViewer.ReportSource = uriReportSource1;
            this.tlkViewer.Size = new System.Drawing.Size(861, 561);
            this.tlkViewer.TabIndex = 5;
            // 
            // radCollapsiblePanel1
            // 
            this.radCollapsiblePanel1.BackColor = System.Drawing.SystemColors.Control;
            this.radCollapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radCollapsiblePanel1.ExpandDirection = Telerik.WinControls.UI.RadDirection.Right;
            this.radCollapsiblePanel1.Location = new System.Drawing.Point(3, 3);
            this.radCollapsiblePanel1.Name = "radCollapsiblePanel1";
            // 
            // radCollapsiblePanel1.PanelContainer
            // 
            this.radCollapsiblePanel1.PanelContainer.Controls.Add(this.rMenuInformes);
            this.radCollapsiblePanel1.PanelContainer.Size = new System.Drawing.Size(185, 559);
            this.radCollapsiblePanel1.Size = new System.Drawing.Size(210, 561);
            this.radCollapsiblePanel1.TabIndex = 4;
            // 
            // rMenuInformes
            // 
            this.rMenuInformes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rMenuInformes.ItemHeight = 28;
            this.rMenuInformes.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.rMenuInformes.LineStyle = Telerik.WinControls.UI.TreeLineStyle.Solid;
            this.rMenuInformes.Location = new System.Drawing.Point(0, 0);
            this.rMenuInformes.Name = "rMenuInformes";
            this.rMenuInformes.Size = new System.Drawing.Size(185, 559);
            this.rMenuInformes.TabIndex = 1;
            this.rMenuInformes.ToggleMode = Telerik.WinControls.UI.ToggleMode.SingleClick;
            // 
            // FrmVisorInformes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 587);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmVisorInformes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visor Informes";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.radCollapsiblePanel1.PanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).EndInit();
            this.radCollapsiblePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rMenuInformes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadCollapsiblePanel radCollapsiblePanel1;
        private Telerik.WinControls.UI.RadTreeView rMenuInformes;
        private Telerik.ReportViewer.WinForms.ReportViewer tlkViewer;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crViewer;
    }
}
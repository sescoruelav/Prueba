namespace RumboSGA.Controles
{
    partial class VisorSQLRibbon
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisorSQLRibbon));
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.panelTrasero = new System.Windows.Forms.Panel();
            this.tablaPrincipal = new RumboSGA.Controles.DBTableLayoutPanel(this.components);
            this.panel = new System.Windows.Forms.Panel();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            this.panelTrasero.SuspendLayout();
            this.tablaPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 636);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(1334, 24);
            this.radStatusStrip1.SizingGrip = false;
            this.radStatusStrip1.TabIndex = 1;
            // 
            // panelTrasero
            // 
            this.panelTrasero.Controls.Add(this.tablaPrincipal);
            this.panelTrasero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTrasero.Location = new System.Drawing.Point(0, 146);
            this.panelTrasero.Name = "panelTrasero";
            this.panelTrasero.Size = new System.Drawing.Size(1334, 490);
            this.panelTrasero.TabIndex = 2;
            // 
            // tablaPrincipal
            // 
            this.tablaPrincipal.ColumnCount = 1;
            this.tablaPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablaPrincipal.Controls.Add(this.panel, 0, 0);
            this.tablaPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaPrincipal.Location = new System.Drawing.Point(0, 0);
            this.tablaPrincipal.Name = "tablaPrincipal";
            this.tablaPrincipal.RowCount = 2;
            this.tablaPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tablaPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tablaPrincipal.Size = new System.Drawing.Size(1334, 490);
            this.tablaPrincipal.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(3, 3);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1328, 337);
            this.panel.TabIndex = 0;
            // 
            // radRibbonBar1
            // 
            // 
            // 
            // 
            this.radRibbonBar1.ExitButton.Text = "Exit";
            this.radRibbonBar1.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radRibbonBar1.LocalizationSettings.LayoutModeText = "Simplified Layout";
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.OptionsButton.Text = "Options";
            this.radRibbonBar1.OptionsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            // 
            // 
            // 
            this.radRibbonBar1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.radRibbonBar1.SimplifiedHeight = 100;
            this.radRibbonBar1.Size = new System.Drawing.Size(1334, 146);
            this.radRibbonBar1.StartButtonImage = ((System.Drawing.Image)(resources.GetObject("radRibbonBar1.StartButtonImage")));
            this.radRibbonBar1.TabIndex = 0;
            this.radRibbonBar1.Text = "VisorSQLRibbon";
            this.radRibbonBar1.Click += new System.EventHandler(this.radRibbonBar1_Click);
            // 
            // VisorSQLRibbon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 660);
            this.Controls.Add(this.panelTrasero);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonBar1);
            this.MainMenuStrip = null;
            this.Name = "VisorSQLRibbon";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "VisorSQLRibbon";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VisorSQLRibbon_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            this.panelTrasero.ResumeLayout(false);
            this.tablaPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private System.Windows.Forms.Panel panelTrasero;
        protected DBTableLayoutPanel tablaPrincipal;
        protected System.Windows.Forms.Panel panel;
        private Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
    }
}

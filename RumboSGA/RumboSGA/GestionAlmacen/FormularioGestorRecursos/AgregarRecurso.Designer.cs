
namespace RumboSGA.GestionAlmacen.FormularioGestorRecursos
{
    partial class AgregarRecurso
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
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.radGridViewRecursos = new Telerik.WinControls.UI.RadGridView();
            this.radBtnAceptar = new RumboSGA.RumButton();
            this.radBtnCancelar = new RumboSGA.RumButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewRecursos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewRecursos.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnAceptar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancelar)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTitleBar1.Location = new System.Drawing.Point(0, 0);
            this.radTitleBar1.Name = "radTitleBar1";
            this.radTitleBar1.Size = new System.Drawing.Size(819, 46);
            this.radTitleBar1.TabIndex = 0;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.Text = "AgregarRecurso";
            // 
            // radGridViewRecursos
            // 
            this.radGridViewRecursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewRecursos.Location = new System.Drawing.Point(0, 46);
            // 
            // 
            // 
            this.radGridViewRecursos.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridViewRecursos.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewRecursos.Name = "radGridViewRecursos";
            this.radGridViewRecursos.Size = new System.Drawing.Size(819, 431);
            this.radGridViewRecursos.TabIndex = 1;
            this.radGridViewRecursos.ValueChanged += new System.EventHandler(this.radGridViewRecursos_ValueChanged);
            // 
            // radBtnAceptar
            // 
            this.radBtnAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radBtnAceptar.Image = global::RumboSGA.Properties.Resources.Approve;
            this.radBtnAceptar.Location = new System.Drawing.Point(146, 3);
            this.radBtnAceptar.Name = "radBtnAceptar";
            this.radBtnAceptar.Size = new System.Drawing.Size(116, 38);
            this.radBtnAceptar.TabIndex = 2;
            this.radBtnAceptar.Text = "aceptar";
            this.radBtnAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radBtnAceptar.Click += new System.EventHandler(this.radBtnAceptar_Click);
            // 
            // radBtnCancelar
            // 
            this.radBtnCancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radBtnCancelar.Image = global::RumboSGA.Properties.Resources.Delete;
            this.radBtnCancelar.Location = new System.Drawing.Point(554, 3);
            this.radBtnCancelar.Name = "radBtnCancelar";
            this.radBtnCancelar.Size = new System.Drawing.Size(116, 38);
            this.radBtnCancelar.TabIndex = 3;
            this.radBtnCancelar.Text = "Cancelar";
            this.radBtnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radBtnCancelar.Click += new System.EventHandler(this.radBtnCancelar_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.5F));
            this.tableLayoutPanel1.Controls.Add(this.radBtnCancelar, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.radBtnAceptar, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 433);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(819, 44);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // AgregarRecurso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 477);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.radGridViewRecursos);
            this.Controls.Add(this.radTitleBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AgregarRecurso";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AgregarRecurso";
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewRecursos.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewRecursos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnAceptar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancelar)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
        private Telerik.WinControls.UI.RadGridView radGridViewRecursos;
        private RumButton radBtnAceptar;
        private RumButton radBtnCancelar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
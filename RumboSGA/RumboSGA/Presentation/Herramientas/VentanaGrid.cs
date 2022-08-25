using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas
{
    public partial class VentanaGrid : Telerik.WinControls.UI.RadForm
    {
        RadGridView gridViewBase = new RadGridView();
        Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
        public RadGridView GridViewBase { get => gridViewBase; set => gridViewBase = value; }
        public Panel panel = new Panel();
        public VentanaGrid(string query/*,Boolean agrupar*/)
        {
            InitializeComponent();
            try
            {
                GridViewBase.DataSource = ConexionSQL.getDataTable(query);

                GridViewBase.AutoGenerateColumns = true;
                GridViewBase.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                GridViewBase.BestFitColumns();
                GridViewBase.Dock = DockStyle.Fill;
                GridViewBase.AllowSearchRow=true;
                GridViewBase.EnableFiltering = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            this.GridViewBase.MasterTemplate.Refresh(null);
            //this.AutoSize = true;
            //this.GridViewBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridViewBase.Location = new System.Drawing.Point(4, 37);

            this.GridViewBase.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.GridViewBase.Name = "gridView";
            //this.GridViewBase.Size = new System.Drawing.Size(947, 640);

            //this.GridViewBase.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            
            //Bug de Telerik. Si ponemos autosize a true y activamos agrupación tenemos que especificar el tamaño máximo del panel de agrupación o devuelve excepción
            //this.gridViewBase.GridViewElement.GroupPanelElement.MaxSize = new Size(500, 500);
            //this.GridViewBase.Dock = DockStyle.Fill;
            panel.Controls.Add(GridViewBase);
            this.Size = new Size(1000, 600);
            //panel.AutoSize = true;
            panel.Dock = DockStyle.Fill;
            this.Controls.Add(panel);
            
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GridViewBase.TableElement.BeginUpdate();
            SetPrefferences();
            GridViewBase.TableElement.EndUpdate(true);

        }
        public void OnDoubleClick_Cerrar(object sender,EventArgs e)
        {
            this.Close();
        }

        private void SetPrefferences()
        {
            this.GridViewBase.TabIndex = 0;
            this.GridViewBase.MasterTemplate.EnableFiltering = true;
            this.GridViewBase.EnableFiltering = true;
            this.GridViewBase.EnablePaging = true;
            this.GridViewBase.EnableGrouping = false;
            this.GridViewBase.ReadOnly = true;
            this.GridViewBase.AllowAddNewRow = false;
            this.GridViewBase.SelectionMode = GridViewSelectionMode.FullRowSelect;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using Telerik.WinControls.Data;
using RumboSGAManager.Model.Entities;
using RumboSGAManager;
using Rumbo.Core.Herramientas;

namespace RumboSGA.Herramientas
{
    public partial class Movimientos : Telerik.WinControls.UI.RadForm
    {
        public string name;

        public Movimientos()
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce(this.Text);
            radGridView1.ReadOnly = true;
            radGridView1.EnablePaging = false;
            radGridView1.PageSize = 30;
            radGridView1.AllowSearchRow = true;
            radGridView1.MasterView.TableSearchRow.SearchDelay = 1500;
            this.radGridView1.ShowGroupPanel = true;
            this.radGridView1.EnableFiltering = true;
            this.radGridView1.Location = new System.Drawing.Point(3, 94);
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(680, 353);
            this.radGridView1.TabIndex = 0;
            //La linea comentada es la query real,que no devuelve datos,la de ahora es para pruebas
            //string queryMovimientos = SentenciasSQL.queryHerramientaMovimientos;
            string queryMovimientos = "SELECT * FROM TBLMOVIMIENTOSEMBALAJE";
            DataTable dt = ConexionSQL.getDataTable(queryMovimientos);
            Utilidades.TraducirDataTableColumnName(ref dt);
            radGridView1.DataSource = dt;
            radGridView1.BestFitColumns();
            radButton1.Text = Lenguaje.traduce(radButton1.Text);
            radButton2.Text = Lenguaje.traduce(radButton2.Text);
            radButton3.Text = Lenguaje.traduce(radButton3.Text);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Shown += form_Shown;
            this.Show();
        }
        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }
        private void radGridView1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            radGridView1.ShowColumnChooser();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.radGridView1.GroupDescriptors.Clear();
            GroupDescriptor descriptor = new GroupDescriptor();
            descriptor.GroupNames.Add("IDMOVIMIENTO", ListSortDirection.Ascending);
            descriptor.Aggregates.Add("Sum(CANTIDAD)");
            descriptor.Format = "{1}  |  {2}";
            this.radGridView1.GroupDescriptors.Add(descriptor);
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            this.radGridView1.GroupDescriptors.Clear();
            this.radGridView1.ShowGroupPanel = true;
        }
    }
}

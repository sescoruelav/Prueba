using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using Telerik.WinControls.UI;
using RumboSGAManager;
using RumboSGAManager.Model.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class GridViewControl : UserControl
    {
        //  private BindingList<Item> list = new BindingList<Item>();
        //RadGridView rgv;
        string query;
        public GridViewControl(string queryRecibida)
        {
            InitializeComponent();

            this.query = queryRecibida;
            //string connectionString= "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=DEMO;Integrated Security=SSPI";
            string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                gridView.DataSource = null;
                try
                {
                    gridView.DataSource = GetData(conn)[0];
                    gridView.BestFitColumns();
                    gridView.Dock = DockStyle.Fill;
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

            }
        }
        public RadGridView getGrid()
        {
            return gridView;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            gridView.TableElement.BeginUpdate();
            gridView.BindingContext = new BindingContext();
       //     gridViewArticulos.MasterTemplate.AutoGenerateColumns = true;
            SetPreferences();
            gridView.TableElement.EndUpdate(true);
            gridView.MasterTemplate.Refresh(null);
            gridView.TableElement.RowHeight = 40;
        }
        
        private void SetPreferences()
        {
            gridView.MasterTemplate.EnableGrouping = true;
            gridView.ShowGroupPanel = true;
            gridView.MasterTemplate.AutoExpandGroups = true;
            gridView.EnableHotTracking = true;
            gridView.MasterTemplate.AllowAddNewRow = false;
            gridView.MasterTemplate.EnableFiltering = true;
            gridView.MasterTemplate.AllowColumnResize = true;
            gridView.MasterTemplate.AllowMultiColumnSorting = true;
            gridView.AllowSearchRow = true;
            gridView.EnablePaging = false;
            gridView.MasterView.TableSearchRow.SearchDelay = 2000;
            foreach(GridViewColumn col in gridView.MasterTemplate.Columns)
                col.TextAlignment = ContentAlignment.MiddleCenter;
        }
        
        public BindingList<DataTable> GetData(SqlConnection conn)
        {
            BindingList<DataTable> lista = new BindingList<DataTable>();

            //string query = intermedia.Query;
            //string query = "SELECT * FROM "+intermedia.NombreTabla+" "+intermedia.Filtros;
            SqlCommand cmd = new SqlCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            lista.Add(dt);
            return lista;
        }

        protected void Eventos()
        {
            //this.gridViewArticulos.ColumnChooserCreated += this.RadGridView1_ColumnChooserCreated;
        }

        private void RadGridView1_ColumnChooserCreated(object sender, ColumnChooserCreatedEventArgs e)
        {
            //e.ColumnChooser.EnableFilter = this.radCheckBoxEnableFiltering.Checked;
        }
        

        #region INotifyPropertyChanged Members 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion
        protected virtual void btnEditar_onClick(object sender, EventArgs e)
        {
            gridView.ShowColumnChooser();
        }
     

        private void radGridView1_Click(object sender, EventArgs e)
        {

        }
    }
}  
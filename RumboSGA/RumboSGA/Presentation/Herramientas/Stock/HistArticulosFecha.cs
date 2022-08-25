using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class HistArticulosFecha : Telerik.WinControls.UI.RadForm
    {
        public DateTime fecha;
        public int idArticulo;
        public HistArticulosFecha()
        {
            InitializeComponent();           
            labelArticulo.Text = strings.Articulo;
            labelFecha.Text = strings.Fecha;
            radDateTimePicker1.ValueChanged += ValorCambiadoFecha;
            radDateTimePicker1.Value = DateTime.Now;
            if (CultureInfo.CurrentUICulture.Name=="en-US")
            {
               radDateTimePicker1.Culture = new System.Globalization.CultureInfo("en-US");
            }
            btnCancelar.Click += btnCancelar_click;
            btnAceptar.Click += btnAceptar_click;
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBox1, DataAccess.GetIdReferenciaDescripcionAtributoArticulos(), "IDARTICULO", "DESCRIPCION", "0", new String[] { "REFERENCIA", "DESCRIPCION" }, true);
            /*string query = "SELECT IDARTICULO,REFERENCIA,DESCRIPCION FROM TBLARTICULOS";
            DataTable dt = ConexionSQL.getDataTable(query);
            Utilidades.TraducirDataTableColumnName(ref dt);
            radMultiColumnComboBox1.DataSource = dt;
            radMultiColumnComboBox1.ValueMember = dt.Columns[0].ColumnName;
            radMultiColumnComboBox1.DisplayMember = dt.Columns[2].ColumnName;*/
            radMultiColumnComboBox1.SelectedIndexChanged += ValorCambiadoGrid;
            /*radMultiColumnComboBox1.BestFitColumns();
            radMultiColumnComboBox1.AutoCompleteMode = AutoCompleteMode.Append;*/
            this.Text = Lenguaje.traduce("HistArticulosFecha");

        }
        
            public void ValorCambiadoFecha(object a,EventArgs args)
        {
            RadDateTimePicker datePicker = a as RadDateTimePicker;
            fecha = radDateTimePicker1.Value;
        }
        public void ValorCambiadoGrid(object a,EventArgs args)
        {
            if (radMultiColumnComboBox1.SelectedValue!=null)
            {
                int.TryParse(radMultiColumnComboBox1.SelectedValue.ToString(), out idArticulo);
            }
        }
        private void btnAceptar_click(object a, EventArgs args)
        {
            if (radMultiColumnComboBox1.SelectedValue != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                /*if (CultureInfo.CurrentUICulture.Name == "es-ES")
                {*/
                MessageBox.Show(Lenguaje.traduce("Debes seleccionar un valor de la lista"));
                /*}
                else
                {
                    MessageBox.Show("You need to select a value from the list");

                }*/
            }
        }
        private void btnCancelar_click(object a,EventArgs args)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void HistArticulosFecha_Load(object sender, EventArgs e)
        {
          
        }

        internal void setArticulo(int articulo)
        {
            try
            {
                idArticulo = articulo;
                radMultiColumnComboBox1.SelectedValue = articulo;

            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }
    }
}

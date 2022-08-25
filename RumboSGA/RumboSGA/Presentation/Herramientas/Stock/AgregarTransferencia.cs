using RumboSGA.Properties;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class AgregarTransferencia : Telerik.WinControls.UI.ShapedForm
    {
        public AgregarTransferencia(string entrada,string articulo) { 
            InitializeComponent();
            labelEntrada.Text = strings.Entrada;
            labelArticulo.Text =strings.Articulo;
            labelHuecoDest.Text =strings.HuecoDest;
            labelCantTotal.Text =strings.CantTotal;
            labelCantPalet.Text =strings.CantPalet;
            labelCantUnidad.Text =strings.CantUnidad;
            labelComentario.Text =strings.Comentario;
            labelEntradaValor.Text =entrada;
            labelArticuloValor.Text =articulo;
            radCheckBox1.Text = strings.ImprimirEtiq;
            radDropDownList1_GetSource();
            radDropDownList1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;

        }
        private void radDropDownList1_GetSource()
        {
            string query = "SELECT * FROM TBLHUECOS";
            DataTable dt= ConexionSQL.getDataTable(query);
            radDropDownList1.DataSource = dt.DefaultView;
            radDropDownList1.DisplayMember = "DESCRIPCION";
            radDropDownList1.ValueMember = "IDHUECO"; 
        }
        private void btnCancelar_Click(object sender,EventArgs e)
        {
            this.Close();
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

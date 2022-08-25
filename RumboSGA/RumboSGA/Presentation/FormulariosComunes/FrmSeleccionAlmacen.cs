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
using RumboSGAManager;
using Rumbo.Core.Herramientas;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FrmSeleccionAlmacen : Telerik.WinControls.UI.RadForm
    {
        public int almacen;
        public int celda;
        public string nombreHoja;
        public string orientacion;
        public string portalesCrecientes;
        public string acerasCrecientes;

        public FrmSeleccionAlmacen()
        {
            InitializeComponent();//TODO revisar si hace falta
            string query = "SELECT IDHUECOALMACEN,DESCRIPCION FROM TBLHUECOSALMACEN";
          
            DataTable dtAlmacenes = ConexionSQL.getDataTable(query);          
            rmccmbAlmacen.DataSource = dtAlmacenes.DefaultView;
            //rmccmbAlmacen.MultiColumnComboBoxElement.DropDownWidth = 500;
            rmccmbAlmacen.MultiColumnComboBoxElement.BestFitColumns();
            rmccmbAlmacen.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            rmccmbAlmacen.MultiColumnComboBoxElement.AutoSizeDropDownColumnMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            rmccmbAlmacen.DisplayMember = "DESCRIPCION";
            rmccmbAlmacen.ValueMember = "IDHUECOALMACEN";
            rmccmbAlmacen.ClearTextOnValidation = true;
            rtxtCelda.Text = "2";
            
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;

            this.Text = Lenguaje.traduce(this.Text);
            rDropDownOrientacion.Text = rDropDownOrientacion.Items[0].Text;
            rDropDownAcerasCrecientes.Text = rDropDownAcerasCrecientes.Items[0].Text;
            rDropDownPortalesCrecientes.Text = rDropDownPortalesCrecientes.Items[0].Text;

            Utilidades.traduccionDropDown(ref rDropDownAcerasCrecientes);
            Utilidades.traduccionDropDown(ref rDropDownOrientacion);
            Utilidades.traduccionDropDown(ref rDropDownPortalesCrecientes);
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {

                almacen =Convert.ToInt32(rmccmbAlmacen.SelectedValue);
                celda = Convert.ToInt32(rtxtCelda.Text);
                orientacion = rDropDownOrientacion.Text;
                nombreHoja = rTxtNombreHoja.Text;
                portalesCrecientes = rDropDownPortalesCrecientes.Text;
                acerasCrecientes = rDropDownAcerasCrecientes.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException exc)
            {
                MessageBox.Show(Lenguaje.traduce("Valores introducidos incorrectos"));
                ExceptionManager.GestionarError(exc);
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rumLabel2_Click(object sender, EventArgs e)
        {

        }

        

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {

        }

        private void BtnAceptar_Click_1(object sender, EventArgs e)
        {

        }
    }
}

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

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FrmSeleccionZona : Telerik.WinControls.UI.RadForm
    {
        public int zona1;
        public Color color1;
        public int zona2;
        public Color color2;
        public int zona3;
        public Color color3;

        public FrmSeleccionZona()
        {
            InitializeComponent();//TODO revisar si hace falta
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;
            string query = "SELECT IdzonaCab, DESCRIPCION FROM TBLZONALOGCAB";
          
            DataTable dt1 = ConexionSQL.getDataTable(query); 
            //zona1
            rmccmbZona1.DataSource = dt1.DefaultView;
            //rmccmbAlmacen.MultiColumnComboBoxElement.DropDownWidth = 500;
            rmccmbZona1.MultiColumnComboBoxElement.BestFitColumns();
            rmccmbZona1.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            rmccmbZona1.MultiColumnComboBoxElement.AutoSizeDropDownColumnMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            rmccmbZona1.DisplayMember = "DESCRIPCION";
            rmccmbZona1.ValueMember = "IDZONACAB";
            rmccmbZona1.ClearTextOnValidation = true;
            rColorBox1.Value = Color.Yellow;
            //Zona2
            DataTable dt2 = ConexionSQL.getDataTable(query);
            rmccmbZona2.DataSource = dt2.DefaultView;
            //rmccmbAlmacen.MultiColumnComboBoxElement.DropDownWidth = 500;
            rmccmbZona2.MultiColumnComboBoxElement.BestFitColumns();
            rmccmbZona2.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            rmccmbZona2.MultiColumnComboBoxElement.AutoSizeDropDownColumnMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            rmccmbZona2.DisplayMember = "DESCRIPCION";
            rmccmbZona2.ValueMember = "IDZONACAB";
            rmccmbZona2.ClearTextOnValidation = true;
            rColorBox2.Value = Color.Blue;
            //Zona3
            DataTable dt3 = ConexionSQL.getDataTable(query);
            rmccmbZona3.DataSource = dt3.DefaultView;
            //rmccmbAlmacen.MultiColumnComboBoxElement.DropDownWidth = 500;
            rmccmbZona3.MultiColumnComboBoxElement.BestFitColumns();
            rmccmbZona3.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            rmccmbZona3.MultiColumnComboBoxElement.AutoSizeDropDownColumnMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            rmccmbZona3.DisplayMember = "DESCRIPCION";
            rmccmbZona3.ValueMember = "IDZONACAB";
            rmccmbZona3.ClearTextOnValidation = true;
            rColorBox3.Value = Color.Red;

            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            this.Text = Lenguaje.traduce(this.Text);
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {
                zona1 =Convert.ToInt32(rmccmbZona1.SelectedValue);
                color1 = rColorBox1.Value;
                zona2 = Convert.ToInt32(rmccmbZona2.SelectedValue);
                color2 = rColorBox2.Value;
                zona3 = Convert.ToInt32(rmccmbZona3.SelectedValue);
                color3 = rColorBox3.Value;
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
    }
}

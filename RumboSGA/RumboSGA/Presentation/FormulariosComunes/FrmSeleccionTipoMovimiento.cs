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
    public partial class FrmSeleccionTipoMovimiento : Telerik.WinControls.UI.RadForm
    {
        public string tipoMovimiento;        

        public FrmSeleccionTipoMovimiento()
        {
            InitializeComponent();//TODO revisar si hace falta
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;
            this.Text = Lenguaje.traduce("Selección Tipo");
            string query = "SELECT IDRESERVATIPO, DESCRIPCION FROM TBLRESERVASTIPO WHERE IDRESERVATIPO IN ('PI','RP','SC')";
          
            DataTable dt1 = ConexionSQL.getDataTable(query); 
            //zona1
            rmccmbReservaTipo.DataSource = dt1.DefaultView;
            //rmccmbAlmacen.MultiColumnComboBoxElement.DropDownWidth = 500;
            rmccmbReservaTipo.MultiColumnComboBoxElement.BestFitColumns();
            rmccmbReservaTipo.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            rmccmbReservaTipo.MultiColumnComboBoxElement.AutoSizeDropDownColumnMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            rmccmbReservaTipo.DisplayMember = "DESCRIPCION";
            rmccmbReservaTipo.ValueMember = "IDRESERVATIPO";
            rmccmbReservaTipo.ClearTextOnValidation = true;
            

            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {
                tipoMovimiento =rmccmbReservaTipo.SelectedValue.ToString();                
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

        
    }
}

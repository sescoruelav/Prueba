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
    public partial class FrmSeleccionInforme : Telerik.WinControls.UI.RadForm
    {
        public int idInforme;        

        public FrmSeleccionInforme(string informes)
        {
            InitializeComponent();//TODO revisar si hace falta
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;
            this.Text = Lenguaje.traduce("Selección Informe");
            string query = "SELECT IDINFORME, TITULO FROM RUMINFORMES WHERE IDINFORME IN ("+informes+")";
          
            DataTable dt1 = ConexionSQL.getDataTable(query); 
            //zona1
            rmccmbInforme.DataSource = dt1.DefaultView;           
            rmccmbInforme.MultiColumnComboBoxElement.BestFitColumns();
            rmccmbInforme.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            rmccmbInforme.MultiColumnComboBoxElement.AutoSizeDropDownColumnMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            rmccmbInforme.DisplayMember = "TITULO";
            rmccmbInforme.ValueMember = "IDINFORME";
            rmccmbInforme.ClearTextOnValidation = true;
            

            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {
                idInforme =Convert.ToInt32(rmccmbInforme.SelectedValue);                
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

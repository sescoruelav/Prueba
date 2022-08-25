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
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FrmImportarStock : Telerik.WinControls.UI.RadForm
    {
        public int almacen;
        public int celda;
        public string nombreHoja;
        public string orientacion;
        public string portalesCrecientes;
        public string acerasCrecientes;

        public FrmImportarStock()
        {
            
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {

                
            }
            catch (FormatException exc)
            {
                //MessageBox.Show(Lenguaje.traduce("Valores introducidos incorrectos"));
                ExceptionManager.GestionarError(exc);
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            
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

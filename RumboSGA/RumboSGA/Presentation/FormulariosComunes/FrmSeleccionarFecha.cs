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
    public partial class FrmSeleccionarFecha : Telerik.WinControls.UI.RadForm
    {
        //nueva
        public DateTime fechaSeleccionada;

        public FrmSeleccionarFecha()
        {
            InitializeComponent();//TODO revisar si hace falta
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;

            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            this.Text = Lenguaje.traduce(this.Text);
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {
                fechaSeleccionada = Convert.ToDateTime(rCalFechaDesde.SelectedDate);
                if (fechaSeleccionada.Day == 1 && fechaSeleccionada.Month == 1 && fechaSeleccionada.Year == 1900)
                {
                    fechaSeleccionada = DateTime.Today;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (FormatException exc)
            {
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


    }
}

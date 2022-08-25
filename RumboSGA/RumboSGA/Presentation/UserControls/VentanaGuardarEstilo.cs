using Rumbo.Core.Herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.UserControls
{
    public partial class VentanaGuardarEstilo : Telerik.WinControls.UI.RadForm
    {
        public static int guardar=2;

        public VentanaGuardarEstilo(int tipo)
        {
            InitializeComponent();
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            radDropDownList1.SelectedIndex = 1;
            if (tipo == 1)
            {
                this.Text = Lenguaje.traduce("VentanaGuardarEstilo");
            }
            else if(tipo == 2)
            {
                this.Text = Lenguaje.traduce("VentanaCargarEstilo");
            }
            else if(tipo == 3)
            {
                this.Text = Lenguaje.traduce("VentanaEliminarEstilo");
            }
            this.radLabel1.Text= "Selecciona tipo de configuracion";
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            if (radDropDownList1.SelectedItem.Text == "Global")
                guardar = 0;
            else
            {
                if (radDropDownList1.SelectedItem.Text == "Local")
                    guardar = 1;
                else
                    guardar = 2;
            }
            this.Close();
        }
        private void btnCancelar_Click(object sender,EventArgs e)
        {
            guardar = -1;
            this.Close();
        }
    }
}

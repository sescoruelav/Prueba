using Rumbo.Core.Herramientas;
using RumboSGA.RecursoMotor;
using RumboSGA.ReservaMotor;
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class DialogCantidad : Telerik.WinControls.UI.RadForm
    {
        WSRecursoMotorClient ws = null;
        WSReservaMotorClient ws2 = null;
        String json = "";
        static int OK = 1;
        static int Cancel = 0;
        public int accion = Cancel;
        public DialogCantidad(String jsonReposicion)
        {
            InitializeComponent();
            label1.Text = Lenguaje.traduce("Numero de reposiciones");
            button1.Text = Lenguaje.traduce("Aceptar");
            button2.Text = Lenguaje.traduce("Cancelar");
            json = jsonReposicion;
            textBox1.KeyPress += new KeyPressEventHandler(txt_KeyPress);

        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            int temp = 0;
            if (!(int.TryParse(e.KeyChar.ToString(), out temp)) && !(e.KeyChar == (Char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                accion = OK;
                this.Close();
            }


            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            accion = Cancel;
            this.Close();
        }
    }
}

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class PorNum : Telerik.WinControls.UI.RadForm
    {


        public string campo1 { get; set; }
        public string campo2 { get; set; }

        private string nombreFormulario;

        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        private RadGridView gridPacking;

        public PorNum(RadGridView gridPedidos, string nombreFormulario)
        {
            InitializeComponent();
            this.nombreFormulario = nombreFormulario;
            this.Text = Lenguaje.traduce(nombreFormulario);
            this.gridPacking = gridPedidos;
            this.Shown += form_Shown;
            this.ShowIcon = false;


            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnSalir.Text = Lenguaje.traduce("Salir");
            btnAceptar.Click += aceptar_Event;
            btnSalir.Click += salir_Event;
            //textBox1.TextChanging += radTextBox_TextChanging; Alfonso ha dicho que pueden llevar letras
            //textBox2.TextChanging += radTextBox_TextChanging;
        }

        private void aceptar_Event(object sender, EventArgs e)
        {
            campo1 = textBox1.Text;
            campo2 = textBox2.Text;
            this.Close();
        }

        private void radTextBox_TextChanging(object sender, TextChangingEventArgs e)
        {
            e.Cancel = !IsNumber(e.NewValue);
        }

        private bool IsNumber(string text)
        {
            bool res = true;
            try
            {
                if (!string.IsNullOrEmpty(text) && ((text.Length != 1) || (text != "-")))
                {
                    Decimal d = decimal.Parse(text, CultureInfo.CurrentCulture);
                }
            }
            catch
            {
                res = false;
            }
            return res;
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }


    }
}

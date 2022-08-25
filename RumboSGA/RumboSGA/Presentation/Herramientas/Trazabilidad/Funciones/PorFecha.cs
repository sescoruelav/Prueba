using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using RumboSGA.Presentation.Herramientas.Trazabilidad;
using RumboSGAManager.Model.DataContext;
using Rumbo.Core.Herramientas;
using RumboSGAManager;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class PorFecha : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
       
        private RadGridView gridView;
        string dia,diaMañana, mes, año;
        private string nombreFormulario;
        public string dateString { get; set; }
        public string dateStringMañana { get; set; }


        public PorFecha(RadGridView gridView, string nombreFormulario)
        {
            InitializeComponent();
            this.nombreFormulario = nombreFormulario;
            this.Text = Lenguaje.traduce(nombreFormulario);
            this.gridView = gridView;
            this.Shown += form_Shown;
            this.ShowIcon = false;
            //this.Show();
            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnSalir.Text = Lenguaje.traduce("Salir");
            btnAceptar.Click += aceptar_Event;
            btnSalir.Click += salir_Event;
            dateTimePicker.Value = DateTime.Today;
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

      

        public void aceptar_Event(object sender, EventArgs e)
        {
            int d = dateTimePicker.Value.Day + 1;
            diaMañana = validarFecha(d.ToString());
            dia = validarFecha(dateTimePicker.Value.Day.ToString());
            mes = validarFecha(dateTimePicker.Value.Month.ToString());
            año = dateTimePicker.Value.Year.ToString();
            
            dateString = año + mes + dia;
            dateStringMañana = año + mes + diaMañana.ToString();

            

            this.Close();
        }
         

        private string validarFecha(string s)
        {
            if (s.Length < 2)
                s = "0" + s;
            return s;
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

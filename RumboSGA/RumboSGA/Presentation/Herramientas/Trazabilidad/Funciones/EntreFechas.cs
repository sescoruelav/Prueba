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
    public partial class EntreFechas : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
       
        private RadGridView gridView;
        string dia, mes, año;
        private string nombreFormulario;
        public string dateString { get; set; }
        public string dateString2 { get; set; }


        public EntreFechas(RadGridView gridView, string nombreFormulario)
        {
            InitializeComponent();
            this.nombreFormulario = nombreFormulario;
            this.Text = Lenguaje.traduce(nombreFormulario);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.gridView = gridView;
            this.Shown += form_Shown;
            this.ShowIcon = false;
            //this.Show();
            dateTimeLabel1.Text = Lenguaje.traduce("Filtra desde");
            dateTimeLabel2.Text = Lenguaje.traduce("Hasta");
            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnSalir.Text = Lenguaje.traduce("Salir");
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            btnAceptar.Click += aceptar_Event;
            btnSalir.Click += salir_Event;
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = true;
        }

      

        public void aceptar_Event(object sender, EventArgs e)
        {
            dia = validarFecha(dateTimePicker1.Value.Day.ToString());
            mes = validarFecha(dateTimePicker1.Value.Month.ToString());
            año = dateTimePicker1.Value.Year.ToString();            
            dateString = año + mes + dia;

            dia = validarFecha(dateTimePicker2.Value.Day.ToString());
            mes = validarFecha(dateTimePicker2.Value.Month.ToString());
            año = dateTimePicker2.Value.Year.ToString();
            dateString2 = año + mes + dia;

            switch (gridView.Name)
            {
                case "gridMovSalida":
                    OrdenesMovimientosSalidaControl.q = ConfigurarSQL.CargarConsulta(path, "MOVIMIENTOSSALIDA") + " WHERE (REPLACE(left(CONVERT(varchar(10), S.FECHA, 126), 10), '-', '') >= '" + dateString + "' AND REPLACE(left(CONVERT(varchar(10), S.FECHA, 126), 10), '-', '') <= '" + dateString2 + "')";
                    gridView.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(OrdenesMovimientosSalidaControl.q)).DefaultView;
                    break;
                default:
                    break;
            }

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

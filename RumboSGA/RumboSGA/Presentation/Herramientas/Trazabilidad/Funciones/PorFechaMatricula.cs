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
    public partial class PorFechaMatricula : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        RadDateTimePicker dateTimePicker = new RadDateTimePicker();
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        private RadGridView gridMatricula;
        public string tipo { get; set; }

        string dia, mes, año;

        public PorFechaMatricula(RadGridView gridMatricula, string tipo)
        {
            InitializeComponent();
            this.tipo = tipo;
            this.gridMatricula = gridMatricula;
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(250, 125);
            this.Text = Lenguaje.traduce("Matrícula por Fecha");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            tableLayout2.Dock = DockStyle.Fill;
            dateTimePicker.Dock = DockStyle.Fill;

            dateTimePicker.Value = DateTime.Today;

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);
            tableLayout.Controls.Add(dateTimePicker, 0, 0);
            tableLayout.Controls.Add(tableLayout2);
            tableLayout2.Controls.Add(btnAceptar, 0, 1);
            tableLayout2.Controls.Add(btnSalir, 1, 1);
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
            dia = validarFecha(dateTimePicker.Value.Day.ToString());
            mes = validarFecha(dateTimePicker.Value.Month.ToString());
            año = dateTimePicker.Value.Year.ToString();
            
            string dateString = año + mes + dia;

            switch (tipo)
            {
                case "maquinas":
                    string consulta = "";
                    foreach (var sel in gridMatricula.SelectedRows)
                    {
                        consulta += "(OF1.IDMAQUINA=" + sel.Cells[0].Value.ToString() + " AND REPLACE(left(CONVERT(varchar(10),OF1.FECHACREACION, 126),10),'-','')='" + dateString + "') OR ";
                    }
                    consulta = consulta.Remove(consulta.Length - 3);
                    Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE " + consulta;
                    break;
                default:
                    Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE REPLACE(left(CONVERT(varchar(10),OF1.FECHACREACION, 126),10),'-','')='" + dateString + "'"; ;
                    break;
            }
            gridMatricula.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(Matriculas.q)).DefaultView;
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

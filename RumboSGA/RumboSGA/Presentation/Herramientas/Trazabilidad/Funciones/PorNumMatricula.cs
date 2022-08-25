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
    public partial class PorNumMatricula : Telerik.WinControls.UI.RadForm
    {
        RadTextBox textBoxMatricula = new RadTextBox();
        RadTextBox textBoxSerie = new RadTextBox();
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        RumLabel labMatricula = new RumLabel();
        RumLabel labSerie = new RumLabel();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        public string tipo { get; set; }
        public string numMatricula { get; set; }
        public string numSerie { get; set; }
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        private RadGridView gridMatricula;

        public PorNumMatricula(string tipo, RadGridView gridPedidos)
        {
            InitializeComponent();
            this.tipo = tipo;
            this.gridMatricula = gridPedidos;
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(250, 125);
            this.Text = Lenguaje.traduce("Matrícula por Número");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            textBoxMatricula.Dock = DockStyle.Fill;
            textBoxSerie.Dock = DockStyle.Fill;
            labMatricula.Dock = DockStyle.Fill;
            labSerie.Dock = DockStyle.Fill;

            labMatricula.Text = Lenguaje.traduce("Matrícula:");
            labSerie.Text = Lenguaje.traduce("Serie:");
            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnSalir.Text = Lenguaje.traduce("Salir");
          
            btnAceptar.Click += aceptar_Event;
            btnSalir.Click += salir_Event;
            textBoxMatricula.TextChanging += radTextBox_TextChanging;
            textBoxSerie.TextChanging += radTextBox_TextChanging;

            this.Controls.Add(tableLayout);

            switch (tipo)
            {
                case "e_matricula":
                    tableLayout.Controls.Add(labMatricula, 0, 0);
                    tableLayout.Controls.Add(textBoxMatricula, 1, 0);
                    break;
                case "s_matricula":
                    tableLayout.Controls.Add(labMatricula, 0, 0);
                    tableLayout.Controls.Add(textBoxMatricula, 1, 0);
                    break;
                case "e_serie":
                    tableLayout.Controls.Add(labSerie, 0, 0);
                    tableLayout.Controls.Add(textBoxSerie, 1, 0);
                    break;
                case "s_serie":
                    tableLayout.Controls.Add(labSerie, 0, 0);
                    tableLayout.Controls.Add(textBoxSerie, 1, 0);
                    break;
            }
            tableLayout.Controls.Add(btnAceptar, 0, 2);
            tableLayout.Controls.Add(btnSalir, 1, 2);
        }

        private void aceptar_Event(object sender, EventArgs e)
        {
            switch (tipo)
            {
                case "e_matricula":
                    this.numMatricula = textBoxMatricula.Text;
                    Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE (E.IDENTRADA=" + numMatricula + " OR E.SSCC='" + numMatricula + "')";
                    break;
                case "s_matricula":
                    this.numMatricula = textBoxMatricula.Text;
                    Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE (S.IDENTRADA=" + numMatricula + " OR E.SSCC='" + numMatricula + "')";
                    break;
                case "e_serie":
                    this.numSerie = textBoxSerie.Text;
                    Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE E.NSERIE = '" + numSerie + "'";
                    break;
                case "s_serie":
                    this.numSerie = textBoxSerie.Text;
                    Matriculas.q = ConfigurarSQL.CargarConsulta(path, "MATRICULA") + " WHERE E.NSERIE = '" + numSerie + "'";
                    break;
            }
            gridMatricula.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(Matriculas.q)).DefaultView;
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

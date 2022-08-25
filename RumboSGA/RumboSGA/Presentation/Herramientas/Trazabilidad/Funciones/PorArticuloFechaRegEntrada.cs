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
using Telerik.WinControls;
using Rumbo.Core.Herramientas;
using RumboSGAManager;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class PorArticuloFechaRegEntrada : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        List<string> motivos = new List<string>();
        RadDateTimePicker dateTimePicker1 = new RadDateTimePicker();
        RadDateTimePicker dateTimePicker2 = new RadDateTimePicker();
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        TableLayoutPanel tableLayout3 = new TableLayoutPanel();
        RadLabel loteLbl = new RadLabel();
        RadLabel fechacreaciondesdeLbl = new RadLabel();
        RadLabel fechacreacionhastaLbl = new RadLabel();
        RadTextBox loteTextBox = new RadTextBox();
        RadGridView motivosList = new RadGridView();
        RadGridView gridRegulaEntrada;
        
        public PorArticuloFechaRegEntrada(RadGridView gridRegulaEntrada)
        {
            InitializeComponent();
            this.gridRegulaEntrada = gridRegulaEntrada;
            this.Shown += form_Shown;
            this.Show();
            this.MaximizeBox = false; // Evita que el usuario maximice el formulario
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Evita que el usuario cambie el tamaño de la ventana
            this.Size = new Size(500, 400);
            tableLayout.Size = new Size(300, 250);
            motivosList.Size = tableLayout.Size;
            tableLayout2.Size = new Size(300, 50);
            tableLayout3.Size = new Size(300, 50);            
            this.Text = Lenguaje.traduce("Regularización Entrada por Artículo y Fecha");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            tableLayout3.Dock = DockStyle.Fill;
            motivosList.Dock = DockStyle.Fill;
            loteTextBox.Dock = DockStyle.Fill;

            loteLbl.Text = strings.LabelLote;
            fechacreaciondesdeLbl.Text = Lenguaje.traduce("Fecha Creación desde:");
            fechacreacionhastaLbl.Text = Lenguaje.traduce("Fecha Creación hasta:");

            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;

            LlenarDiccionarioEstados();

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(motivosList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);
            tableLayout.Controls.Add(tableLayout3, 0, 2);

            tableLayout2.Controls.Add(fechacreaciondesdeLbl, 0, 0);
            tableLayout2.Controls.Add(dateTimePicker1, 1, 0);
            tableLayout2.Controls.Add(fechacreacionhastaLbl, 0, 1);
            tableLayout2.Controls.Add(dateTimePicker2, 1, 1);

            tableLayout3.Controls.Add(btnAceptar, 0, 0);
            tableLayout3.Controls.Add(btnSalir, 1, 0);
        }

        private void LlenarDiccionarioEstados()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOMOTIVOS")));
            motivosList.DataSource = table;
            motivosList.EnableGrouping = false;
            motivosList.AllowAddNewRow = false;
            motivosList.AllowEditRow = false;
            motivosList.AllowDeleteRow = false;
            motivosList.EnableFiltering = true;
            motivosList.BestFitColumns();
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
            RegulaEntrada.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESENTRADA") + " WHERE IDENTRADATIPO = 'ER' AND(REPLACE(left(CONVERT(varchar(10), E.FECHA, 126), 10), '-', '') >= '" + validarFecha(dateTimePicker1) + "' AND REPLACE(left(CONVERT(varchar(10), E.FECHA, 126), 10), '-', '') <= '" + validarFecha(dateTimePicker2) + "' AND E.IDMOTIVO = '" + motivosList.SelectedRows[0].Cells[0].Value + "')";
            gridRegulaEntrada.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(RegulaEntrada.q)).DefaultView;
            this.Close();
        }
        private string validarFecha(RadDateTimePicker dtPicker)
        {
            string dia = dtPicker.Value.Day.ToString();
            string mes = dtPicker.Value.Month.ToString();
            string año = dtPicker.Value.Year.ToString();

            if (dia.Length < 2)
                dia = "0" + dia;
            if (mes.Length < 2)
                mes = "0" + mes;         

            return año + mes + dia;
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

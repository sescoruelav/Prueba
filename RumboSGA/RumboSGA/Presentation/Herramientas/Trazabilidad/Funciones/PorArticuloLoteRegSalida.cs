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
    public partial class PorArticuloLoteRegSalida : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        List<string> articulos = new List<string>();
        RadDateTimePicker dateTimePicker1 = new RadDateTimePicker();
        RadDateTimePicker dateTimePicker2 = new RadDateTimePicker();
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        TableLayoutPanel tableLayout3 = new TableLayoutPanel();
        RadLabel loteLbl = new RadLabel();
        RadTextBox loteTextBox = new RadTextBox();
        RadGridView articulosList = new RadGridView();
        RadGridView gridArticulos;
        
        public PorArticuloLoteRegSalida(RadGridView gridArticulos)
        {
            InitializeComponent();
            this.gridArticulos = gridArticulos;
            this.Shown += form_Shown;
            this.Show();
            this.MaximizeBox = false; // Evita que el usuario maximice el formulario
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Evita que el usuario cambie el tamaño de la ventana
            this.Size = new Size(400, 400);
            tableLayout.Size = new Size(300, 250);
            articulosList.Size = tableLayout.Size;
            tableLayout2.Size = new Size(300, 50);
            tableLayout3.Size = new Size(300, 50);            
            this.Text = Lenguaje.traduce("Regularización Salida por Artículo y Lote");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            tableLayout3.Dock = DockStyle.Fill;
            articulosList.Dock = DockStyle.Fill;
            loteTextBox.Dock = DockStyle.Fill;

            loteLbl.Text = Lenguaje.traduce("Lote:");

            LlenarDiccionarioEstados();

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(articulosList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);
            tableLayout.Controls.Add(tableLayout3, 0, 2);

            tableLayout2.Controls.Add(loteLbl, 0, 0);
            tableLayout2.Controls.Add(loteTextBox, 1, 0);
            
            tableLayout3.Controls.Add(btnAceptar, 0, 0);
            tableLayout3.Controls.Add(btnSalir, 1, 0);
        }

        private void LlenarDiccionarioEstados()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOARTICULOS")));
            articulosList.DataSource = table;
            articulosList.EnableGrouping = false;
            articulosList.AllowAddNewRow = false;
            articulosList.AllowEditRow = false;
            articulosList.AllowDeleteRow = false;
            articulosList.EnableFiltering = true;
            articulosList.BestFitColumns();
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
            if (articulosList.SelectedRows.Count == 0)
            {
                MessageBox.Show(Lenguaje.traduce("Debe seleccionar una fila"));
            }
            else
            {
                RegulaSalida.q = ConfigurarSQL.CargarConsulta(path, "REGULARIZACIONESSALIDA") + " WHERE S.IDSALIDATIPO = 'SR' AND (LOTE = '" + loteTextBox.Text + "' AND S.IDARTICULO = '" + articulosList.SelectedRows[0].Cells[0].Value + "')";
                gridArticulos.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(RegulaSalida.q)).DefaultView;
                this.Close();
            }            
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

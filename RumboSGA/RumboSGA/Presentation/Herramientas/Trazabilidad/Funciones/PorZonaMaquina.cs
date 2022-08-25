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
    public partial class PorZonaMaquina : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        RadTextBox loteTextBox = new RadTextBox();
        RadGridView zonasList = new RadGridView();
        RadGridView gridmaquinas;
        
        public PorZonaMaquina(RadGridView gridmaquinas)
        {
            InitializeComponent();
            this.gridmaquinas = gridmaquinas;
            this.Shown += form_Shown;
            this.Show();
            this.MaximizeBox = false; // Evita que el usuario maximice el formulario
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Evita que el usuario cambie el tamaño de la ventana
            this.Size = new Size(500, 400);
            tableLayout.Size = new Size(250, 300);
            zonasList.Size = tableLayout.Size;
            tableLayout2.Size = new Size(250, 50);       
            this.Text = Lenguaje.traduce("Máquinas por Zona");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            zonasList.Dock = DockStyle.Fill;
            loteTextBox.Dock = DockStyle.Fill;

            LlenarDiccionario();

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnSalir.Text = Lenguaje.traduce("Salir");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(zonasList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);                       
            tableLayout2.Controls.Add(btnAceptar, 0, 0);
            tableLayout2.Controls.Add(btnSalir, 1, 0);
        }

        private void LlenarDiccionario()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOZONAS")));
            zonasList.DataSource = table;
            zonasList.EnableGrouping = false;
            zonasList.AllowAddNewRow = false;
            zonasList.AllowEditRow = false;
            zonasList.AllowDeleteRow = false;
            zonasList.EnableFiltering = true;
            zonasList.BestFitColumns();
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
            PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "MAQUINAS") + " WHERE IDZONALOGICA='" + zonasList.SelectedRows[0].Cells[0].Value + "'";
            gridmaquinas.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(PedidosProv.q)).DefaultView;
            this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

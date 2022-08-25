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
    public partial class PorNombreMaquina : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        RadTextBox loteTextBox = new RadTextBox();
        RadGridView maquinasList = new RadGridView();
        RadGridView gridmaquinas;
        
        public PorNombreMaquina(RadGridView gridmaquinas)
        {
            InitializeComponent();
            this.gridmaquinas = gridmaquinas;
            this.Shown += form_Shown;
            this.Show();
            this.MaximizeBox = false; // Evita que el usuario maximice el formulario
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Evita que el usuario cambie el tamaño de la ventana
            this.Size = new Size(500, 420);
            tableLayout.Size = new Size(300, 325);
            maquinasList.Size = tableLayout.Size;
            tableLayout2.Size = new Size(300, 50);       
            this.Text = Lenguaje.traduce("Máquinas por Nombre");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            maquinasList.Dock = DockStyle.Fill;
            loteTextBox.Dock = DockStyle.Fill;

            LlenarDiccionario();

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(maquinasList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);                       
            tableLayout2.Controls.Add(btnAceptar, 0, 0);
            tableLayout2.Controls.Add(btnSalir, 1, 0);
        }

        private void LlenarDiccionario()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOMAQUINAS")));
            maquinasList.DataSource = table;
            maquinasList.EnableGrouping = false;
            maquinasList.AllowAddNewRow = false;
            maquinasList.AllowEditRow = false;
            maquinasList.AllowDeleteRow = false;
            maquinasList.EnableFiltering = true;
            maquinasList.BestFitColumns();
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
            PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "MAQUINAS") + " WHERE IDMAQUINA='" + maquinasList.SelectedRows[0].Cells[0].Value + "'";
            gridmaquinas.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(PedidosProv.q)).DefaultView;
            this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

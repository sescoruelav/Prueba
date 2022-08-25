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
using RumboSGA.SalidaMotor;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class CambiarEstadoOrdRec : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        int[] ordenes;
        List<string> estados = new List<string>();
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        RadListView estadosList = new RadListView();
        RadGridView gridPacking;
        
        public CambiarEstadoOrdRec(int[] ordenes)
        {
            InitializeComponent();
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(400, 250);
            this.Text = Lenguaje.traduce("Cambiar Estado");
            this.ShowIcon = false;
            this.ordenes = ordenes;
            tableLayout.Dock = DockStyle.Fill;
            tableLayout2.Dock = DockStyle.Fill;
            estadosList.Dock = DockStyle.Fill;

            LlenarDiccionarioEstados();
            estadosList.DataSource = estados;

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(estadosList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);
            tableLayout2.Controls.Add(btnAceptar, 0, 1);
            tableLayout2.Controls.Add(btnSalir, 1, 1);

        }

        private void LlenarDiccionarioEstados()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOESTADOSORDENREC")));
            for (int i = 0; i < table.Rows.Count; i++)
            {
                estados.Add(table.Rows[i][0].ToString());
            }
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
            using (var ws = new WSSalidaMotorClient())
            {
                ws.cambiarEstadoOrdenesRecogida(ordenes, estadosList.SelectedItem.Text);
            }
            MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

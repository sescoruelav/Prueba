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
using RumboSGA.PackingListMotor;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class CambiarUbicacionPacking : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        int[] ordenes;
        List<string> ubicaciones = new List<string>();
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        RadGridView ubicacionesList = new RadGridView();
        
        public CambiarUbicacionPacking(int[] ordenes)
        {
            InitializeComponent();
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(300, 350);
            this.Text = Lenguaje.traduce("Cambiar Ubicación");
            this.ShowIcon = false;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.ordenes = ordenes;
            tableLayout.Dock = DockStyle.Fill;
            tableLayout2.Dock = DockStyle.Fill;
            ubicacionesList.Height = 250;
            ubicacionesList.Dock = DockStyle.Fill;
            ubicacionesList.MultiSelect = false;
            ubicacionesList.BestFitColumns();
            ubicacionesList.AllowAddNewRow = false;
            ubicacionesList.AllowColumnReorder = false;
            ubicacionesList.AllowDragToGroup = false;

            ubicacionesList.DataSource = LlenarDiccionarioUbicaciones();

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(ubicacionesList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);
            tableLayout2.Controls.Add(btnAceptar, 0, 1);
            tableLayout2.Controls.Add(btnSalir, 1, 1);

        }

        private DataTable LlenarDiccionarioUbicaciones()
        {
            return Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOUBICACIONES")));
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
            using (var ws = new WSPackingListMotorClient())
            {
                foreach (var o in ordenes)
                {
                    ws.cambiaUbicacionPackingList(o, Convert.ToInt32(ubicacionesList.SelectedRows[0].Cells["idubicacionactual"].Value));
                }
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

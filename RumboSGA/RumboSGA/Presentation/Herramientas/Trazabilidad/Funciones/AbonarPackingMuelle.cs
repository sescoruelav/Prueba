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
using RumboSGAManager.Model;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class AbonarPackingMuelle : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        int[] packings;
        //List<string> ubicaciones = new List<string>();
       // RadButton btnAceptar = new RadButton();
        //RadButton btnSalir = new RadButton();
        //TableLayoutPanel tableLayout = new TableLayoutPanel();
        //TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        //RadGridView ubicacionesList = new RadGridView();
        
        public AbonarPackingMuelle(int[] packings)
        {
            InitializeComponent();
           // this.Shown += form_Shown;
          //  this.Show();
           // this.Size = new Size(300, 350);
            this.Text = Lenguaje.traduce("Cambiar Ubicación");
            this.ShowIcon = false;
            //this.MinimumSize = this.Size;
            //this.MaximumSize = this.Size;
            this.packings = packings;
            /*tableLayout.Dock = DockStyle.Fill;
            tableLayout2.Dock = DockStyle.Fill;
            ubicacionesList.Height = 250;
            ubicacionesList.Dock = DockStyle.Fill;
            ubicacionesList.MultiSelect = false;
            ubicacionesList.BestFitColumns();
            ubicacionesList.AllowAddNewRow = false;
            ubicacionesList.AllowColumnReorder = false;
            ubicacionesList.AllowDragToGroup = false;

            ubicacionesList.DataSource = LlenarDiccionarioUbicaciones();*/

            lblUbicacion.Text = Lenguaje.traduce("Ubicación");
            //btnAceptar.Size = new Size(75, 25);
            //btnAceptar.Click += aceptar_Event;

            //btnSalir.Text = Lenguaje.traduce("Salir");
            //btnSalir.Size = new Size(75, 25);
            //btnSalir.Click += salir_Event;

            /*this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(ubicacionesList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);
            tableLayout2.Controls.Add(btnAceptar, 0, 1);
            tableLayout2.Controls.Add(btnSalir, 1, 1);*/
            DataTable dt = DataAccess.GetIdDescripcionHueco();
            Utilidades.TraducirDataTableColumnName(ref dt);
            Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion,dt, Lenguaje.traduce("IDHUECO"), Lenguaje.traduce("DESCRIPCION"), "", new String[] { "TODOS" }, true);

        }

        private DataTable LlenarDiccionarioUbicaciones()
        {
            return Utilidades.TraducirDataTableColumnNameDT(DataAccess.GetIdDescripcionHueco());
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
            int idHueco = Convert.ToInt32(radComboBoxUbicacion.SelectedValue);
            if (idHueco > 0)
            {

                using (var ws = new WSPackingListMotorClient())
                {
                    foreach (var o in packings)
                    {
                        ws.abonarPackingListMuelleDevoluciones(o, idHueco, User.IdOperario, 0);
                    }
                }
                MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else{
                MessageBox.Show(Lenguaje.traduce(strings.ErrorFilasMarcadas), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

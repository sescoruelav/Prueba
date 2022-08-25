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
    public partial class AbonarUnidadesRegSalida : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        int idSalida, idOperario, idUsuario, cantidad;
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        RadLabel lblIdSalida = new RadLabel();
        RadLabel lblIdOperario = new RadLabel();
        RadLabel lblIdUsuario = new RadLabel();
        RadLabel lblCantidad = new RadLabel();
        NumericUpDown numCantidad = new NumericUpDown();

        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        
        public AbonarUnidadesRegSalida(int idsalida, int idoperario, int idusuario)
        {
            InitializeComponent();
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(400, 150);
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.Text = Lenguaje.traduce(strings.AbonarUnidades);
            this.ShowIcon = false;
            this.idSalida = idsalida;
            this.idOperario = idoperario;
            this.idUsuario = idusuario;
            tableLayout.Dock = DockStyle.Fill;
            tableLayout2.Dock = DockStyle.Fill;

            lblIdSalida.Text = Lenguaje.traduce("IDSALIDA") + ": " + idSalida;
            lblCantidad.Text = Lenguaje.traduce("Cantidad:");

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(lblIdSalida, 0, 0);
            tableLayout.Controls.Add(lblCantidad, 0, 1);
            tableLayout.Controls.Add(numCantidad, 1, 1);
            tableLayout.Controls.Add(tableLayout2, 0, 2);
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
            try
            {
                using (var ws = new WSSalidaMotorClient())
                {
                    ws.salidaAbono(idSalida, cantidad, idOperario, idUsuario);
                }
            }
            catch
            {
                MessageBox.Show(Lenguaje.traduce(strings.ErrorGenerico), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

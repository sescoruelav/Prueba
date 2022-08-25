using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class VerSQL : Telerik.WinControls.UI.RadForm
    {
        TableLayoutPanel tableLayoutVentana = new TableLayoutPanel();
        RadTextBox textbox = new RadTextBox();
        Panel panelTextBox = new Panel();
        Panel panelBotones = new Panel();
        RadButton btnSalir = new RadButton();

        public VerSQL(string q)
        {
            InitializeComponent();
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(750, 500);
            this.Text = Lenguaje.traduce("Ver SQL");
            this.ShowIcon = false;
            tableLayoutVentana.Dock = DockStyle.Fill;
            panelTextBox.Dock = DockStyle.Fill;
            textbox.Dock = DockStyle.Fill;
            panelBotones.Dock = DockStyle.Bottom;
            btnSalir.Dock = DockStyle.Fill;

            panelTextBox.Controls.Add(textbox);
            panelBotones.Controls.Add(btnSalir);

            btnSalir.Text = Lenguaje.traduce("Salir");
            textbox.Text = q;
            panelTextBox.Height = (this.Height - btnSalir.Height) - 50;

            textbox.Multiline = true;

            btnSalir.Click += salir_Event;
            tableLayoutVentana.Controls.Add(panelTextBox, 0, 0);
            tableLayoutVentana.Controls.Add(panelBotones, 0, 1);
            this.Controls.Add(tableLayoutVentana);
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
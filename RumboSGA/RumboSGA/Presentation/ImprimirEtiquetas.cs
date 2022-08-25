using log4net.Repository.Hierarchy;
using Rumbo.Core.Herramientas;
using RumboSGA.PackingListMotor;
using RumboSGA.Presentation.UserControls;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class ImprimirEtiquetas : Telerik.WinControls.UI.RadForm
    {
        public ImprimirEtiquetas()
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Impresión PackingList");
            TableScheme tableScheme = new TableScheme();
            tableScheme.Etiqueta = "Nº de Impresiones:";
            tableScheme.Nombre = "NumeroImpresiones";
            tableScheme.Control = "TB";
            tableScheme.Alto = 0;
            tableScheme.Ancho = 60;
            this.numeroImpresiones = new NameValue(tableScheme, "");

            this.numeroImpresiones.Name = "numeroImpresiones";
            this.numeroImpresiones.Location = new System.Drawing.Point(0, 25);
            this.numeroImpresiones.tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.Controls.Add(this.numeroImpresiones);

            this.buttonImprimir.Text = Lenguaje.traduce("Imprimir");
            this.buttonSalir.Text = Lenguaje.traduce("Salir");
        }

        private void buttonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonImprimir_Click(object sender, EventArgs e)
        {
            if (this.numeroImpresiones.tb.Text != null && !this.numeroImpresiones.tb.Text.Equals(""))
            {
                try
                {
                    int cantidad = Convert.ToInt32(this.numeroImpresiones.tb.Text);

                    using (var ws = new WSPackingListMotorClient())
                    {
                        ws.imprimirEtiquetasPackingList(User.NombreImpresora, cantidad);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                this.Close();
            }
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            int temp = 0;
            if (!(int.TryParse(e.KeyChar.ToString(), out temp)) && !(e.KeyChar == (Char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void ImprimirEtiquetas_Load(object sender, EventArgs e)
        {
        }
    }
}
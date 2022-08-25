using Rumbo.Core.Herramientas;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FormularioCrearZonaCab : Telerik.WinControls.UI.RadForm
    {
        public FormularioCrearZonaCab()
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Crear zona");
        }


        private bool ComprobarValores()
        {
            string mensajeError = "";

            if (string.IsNullOrEmpty(this.radTextBoxDescripcion.Text))
            {
                mensajeError += Lenguaje.traduce("Descripción tiene que tener algún valor")+" \n";
            }

            if (string.IsNullOrEmpty(this.radTextBoxTipoZona.Text) && this.radTextBoxTipoZona.Text.Length != 2)
            {
                mensajeError += Lenguaje.traduce("Tipo zona tiene que ser dos caracteres")+" \n";
            }


            if (mensajeError.Equals("")) return true;

            RadMessageBox.Show(this, mensajeError, "Error", MessageBoxButtons.OK);
            return false;

        }

        private void FormularioCrearZonaCab_Load(object sender, EventArgs e)
        {
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            if (!ComprobarValores()) return;
            AckResponse res = DataAccess.CrearZonaLogCab(this.radTextBoxDescripcion.Text, this.radTextBoxTipoZona.Text);

            if (res == null) return;

            RadMessageBox.Show(this, res.Mensaje, res.Resultado, MessageBoxButtons.OK);

            if (res.Resultado.Equals("OK"))
            {
                this.Close();
            }

        }
    }
}

using Rumbo.Core.Herramientas;
using RumboSGA.LotesMotor;
using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FormularioCrearLote : Telerik.WinControls.UI.RadForm
    {
        private String lote;
        private int unidades, idArticulo;
        private object[] resultado;

        public object[] Resultado { get => resultado; set => resultado = value; }

        public FormularioCrearLote(int idArticulo, string lote, int unidades, DateTime fecha)
        {
            InitializeComponent();
            this.resultado = null;
            this.Text = Lenguaje.traduce("Crear Lote");
            this.Icon = Resources.bruju;
            this.idArticulo = idArticulo;
            this.lote = lote;
            this.unidades = unidades;
        }

        public FormularioCrearLote(int idArticulo, string lote, int unidades)
        {
            InitializeComponent();
            this.resultado = null;
            this.Text = Lenguaje.traduce("Crear Lote");
            this.Icon = Resources.bruju;
            this.idArticulo = idArticulo;
            this.lote = lote;
            this.unidades = unidades;
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            WSLotesClient wsLotes = new WSLotesClient();
            Resultado = wsLotes.insertarLoteConArticulo(idArticulo, radDateTimeFechaCaducidad.Value, lote, 0);
            this.Close();
        }
    }
}
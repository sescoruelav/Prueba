using Rumbo.Core.Herramientas;
using RumboSGA.LotesMotor;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.FormulariosComunes
{
    public partial class FormularioCrearLote : Telerik.WinControls.UI.RadForm
    {
        private String lote;
        private int unidades,idArticulo;
        private object[] resultado;
        

        public object[] Resultado { get => resultado; set => resultado = value; }

        public FormularioCrearLote(int idArticulo,string lote,int unidades)
        {
            InitializeComponent();
            this.resultado = null;
            this.Text = Lenguaje.traduce("Crear Lote");
            this.Icon = Resources.bruju;
            this.idArticulo = idArticulo;
            this.lote = lote;
            this.unidades = unidades;
            this.radTextBoxLote.Text = lote;
            
        }

        

            public FormularioCrearLote(int idArticulo, string lote, int unidades,DateTime fechaCaducidad)
        {
            InitializeComponent();
            this.resultado = null;
            this.Text = Lenguaje.traduce("Crear Lote");
            this.Icon = Resources.bruju;
            this.idArticulo = idArticulo;
            this.lote = lote;
            this.unidades = unidades;
            this.radTextBoxLote.Text = lote;
            this.radDateTimeFechaCaducidad.Value = fechaCaducidad;
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            lote = radTextBoxLote.Text;
            if (!lote.Equals(""))
            {
                if (DateTime.Compare(radDateTimeFechaCaducidad.Value.Date, DateTime.Now.Date) <= 0)
                {
                    MessageBox.Show(this, Lenguaje.traduce("La fecha de caducidad no puede ser menor o igual que hoy"), Lenguaje.traduce("Lote Incorrecto"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;                   
                }
                WSLotesClient LotesMotor = new WSLotesClient();
                Resultado = LotesMotor.insertarLoteConArticulo(idArticulo, radDateTimeFechaCaducidad.Value, lote, 0);
            }
            else
            {
                MessageBox.Show(this, Lenguaje.traduce("Debe introducir el lote"), Lenguaje.traduce("Lote Incorrecto"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Close();
        }
    }
}

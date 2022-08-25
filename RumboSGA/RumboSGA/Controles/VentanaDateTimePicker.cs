using Rumbo.Core.Herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Controles
{
    public partial class VentanaDateTimePicker : Telerik.WinControls.UI.RadForm
    {
        public DateTime FechaDesde = new DateTime();
        public DateTime FechaHasta = new DateTime();
        public VentanaDateTimePicker(DateTime fechaDesde, DateTime fechaLimite)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce(this.Text);
            FechaDesde = fechaDesde;
            rumDateTimePickerDesde.Value = fechaDesde;
            rumDateTimePickerDesde.ReadOnly = true;
            rumDateTimePickerDesde.MinDate = fechaDesde;
            rumDateTimePickerDesde.MaxDate = fechaDesde;


            rumDateTimePickerHasta.MinDate = fechaDesde;
            rumDateTimePickerHasta.MaxDate = fechaLimite;
            FechaHasta = fechaDesde.AddDays(1);
            rumDateTimePickerHasta.Value = FechaHasta;
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void RumDateTimePickerDesde_ValueChanged(object sender, EventArgs e)
        {
            FechaDesde = rumDateTimePickerDesde.Value;
        }

        private void RumDateTimePickerHasta_ValueChanged(object sender, EventArgs e)
        {
            FechaHasta = rumDateTimePickerHasta.Value;
        }
    }
}

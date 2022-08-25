using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.GestionAlmacen
{
    public partial class DevolucionProveedor : Telerik.WinControls.UI.RadForm
    {
        public DevolucionProveedor(string id,string numero,string estado,string provedor,string fecha)
        {
            InitializeComponent();
            ConfIdioma();
            

        }
        private void ConfIdioma()
        {
            this.Name = NombresFormularios.DevolucionPro;

            lblFecha.Text = Lenguaje.traduce(strings.Fecha);
            lblId.Text = Lenguaje.traduce(strings.ID);
            lblNumero.Text = Lenguaje.traduce(strings.Numero);
            lblEstado.Text = Lenguaje.traduce(strings.Estado);

            btnCerrarDevolucion.Text = Lenguaje.traduce(strings.CerrarDevolucion);
            btnRefrescar.Text = Lenguaje.traduce(strings.RefrescarPantalla);
            btnRetirarDevolucion.Text = Lenguaje.traduce(strings.RetirarDevolucion);
            btnSalir.Text = Lenguaje.traduce(strings.Salir);

            pagEntradas.Text = Lenguaje.traduce(strings.Entradas);
            pagExistencias.Text = Lenguaje.traduce(strings.Existencias);
            pagLineasPedido.Text = Lenguaje.traduce(strings.LineasPedido);
            pagMovimientosUb.Text = Lenguaje.traduce(strings.MovimientosUb);
            pagSalidas.Text = Lenguaje.traduce(strings.Salidas);

            cabecera.Text = Lenguaje.traduce(strings.CabeceraDevol);
            detalle.Text = Lenguaje.traduce(strings.DetalleDevol);
        }
    }
}

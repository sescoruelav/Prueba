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
    public partial class DevolucionesCliente : Telerik.WinControls.UI.RadForm
    {
        public DevolucionesCliente()
        {
            InitializeComponent();
            ConfIdiomas();
        }
        private void ConfIdiomas()
        {
            this.Name = NombresFormularios.DevolClientes;
            btnBuscarDevol.Text = Lenguaje.traduce(strings.BuscarDevol);
            btnCerrarDevol.Text = Lenguaje.traduce(strings.CerrarDevolucion);
            btnDeshacerEntradas.Text = Lenguaje.traduce(strings.DeshacerEntradas);
            btnGenerarTarea.Text = Lenguaje.traduce(strings.GenerarTarea);
            btnNueva.Text = Lenguaje.traduce(strings.Nueva);
            btnRefrescar.Text = Lenguaje.traduce(strings.RefrescarPantalla);
            btnUbicar.Text = Lenguaje.traduce(strings.Ubicar);

            btnAñadirEntrada.Text = Lenguaje.traduce(strings.AñadirEntrada);
            btnEntradaCarro.Text = Lenguaje.traduce(strings.EntradaCarro);
            btnModificarEntrada.Text = Lenguaje.traduce(strings.ModificarEntrada);
            btnEliminarEntrada.Text = Lenguaje.traduce(strings.EliminarEntrada);
            btnReimprimirEtiq.Text = Lenguaje.traduce(strings.ReimprimirEtiqueta);
            btnImprimirEtiquetas.Text = Lenguaje.traduce(strings.ImprimirEtiq);
            btnCambiarUbi.Text = Lenguaje.traduce(strings.CambiarUbicacion);
            btnCambiarUbiDest.Text = Lenguaje.traduce(strings.CambiarUbiDestino);

            radGroupBox1.Text = Lenguaje.traduce(radGroupBox1.Text);

            lblAlbaran.Text = Lenguaje.traduce(strings.Albaran);
            lblCliente.Text = Lenguaje.traduce(strings.Cliente);
            lblDev.Text = Lenguaje.traduce(strings.Dev);
            lblEstado.Text = Lenguaje.traduce(strings.Estado);
            lblMuelle.Text = Lenguaje.traduce(strings.Muelle);
            lblObsv.Text = Lenguaje.traduce(strings.Obsv);
            lblPedido.Text = Lenguaje.traduce(strings.Pedido);
            lblTransporte.Text = Lenguaje.traduce(strings.Transporte);

            pagEntradas.Text = Lenguaje.traduce(strings.Entradas);
            pagExistencias.Text = Lenguaje.traduce(strings.Existencias);
            pagMovsUbicacion.Text = Lenguaje.traduce(strings.MovsUbicacion);
            pagTareas.Text = Lenguaje.traduce(strings.Tareas);
        }

    }
}

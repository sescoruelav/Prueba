using RumboSGA.Properties;
using System;
using System.Data;
using System.Drawing;
using Telerik.WinControls.UI;
using RumboSGAManager.Model.DataContext;
using Telerik.WinControls;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections.Generic;
using Rumbo.Core.Herramientas;
using RumboSGAManager;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class PorCodigoCliente : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        Dictionary<string, string> clientes = new Dictionary<string, string>();
        RadLabel descLbl = new RadLabel();
        RadTextBox clienteTextBox = new RadTextBox();
        RadListView clientesList = new RadListView();
        RadGridView gridPedidos;


        public PorCodigoCliente(RadGridView gridPedidos)
        {
            InitializeComponent();
            this.gridPedidos = gridPedidos;
            this.Shown += form_Shown;
            this.Show();
            this.Text = Lenguaje.traduce("Código cliente");
            this.ShowIcon = false;

            clienteTextBox.NullText = Lenguaje.traduce(strings.IDCliente);
            clienteTextBox.ShowNullText = true;
            clienteTextBox.Dock = DockStyle.Fill;

            descLbl.Text = Lenguaje.traduce("[VACÍO]");

            LlenarDiccionarioClientes();
            clientesList.DataSource = clientes;
            clienteTextBox.TextChanging += new TextChangingEventHandler(rutaTextbox_TextChanging);
            this.radButtonAceptar.Text = Lenguaje.traduce("Aceptar");
            this.radButtonCancelar.Text = Lenguaje.traduce("Cancelar");
        }

        private void LlenarDiccionarioClientes()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOCLIENTES")));
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxClientes, table, Lenguaje.traduce("IDCLIENTE"), Lenguaje.traduce("NOMBRE CLIENTE"), "", null,true);

        }

        private void rutaTextbox_TextChanging(object sender, TextChangingEventArgs e)
        {
            //descLbl.Text = clientes.GetValueOrNull(clienteTextBox.Text);
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
            String idCliente = radMultiColumnComboBoxClientes.SelectedValue.ToString();
            if (String.IsNullOrEmpty(idCliente))
                return;

            PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE PC.IDCLIENTE='" + idCliente + "'";
            gridPedidos.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(PedidosTan.q)).DefaultView;
            this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

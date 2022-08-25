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

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class PorCodigoRuta : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        Dictionary<string, string> rutas = new Dictionary<string, string>();
        /*RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        TableLayoutPanel tableLayout3 = new TableLayoutPanel();
        RadCollapsiblePanel collapsiblePanel = new RadCollapsiblePanel();*/
        RadLabel descLbl = new RadLabel();
        RadTextBox rutaTextBox = new RadTextBox();
        RadListView rutasList = new RadListView();
        RadGridView gridPedidos;        

        public PorCodigoRuta(RadGridView gridPedidos)
        {
            /*InitializeComponent();
            this.gridPedidos = gridPedidos;
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(500, 250);
            this.Text = Lenguaje.traduce("Por Código de Ruta");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            tableLayout2.Dock = DockStyle.Fill;
            tableLayout3.Dock = DockStyle.Fill;
            rutasList.Dock = DockStyle.Fill;
            collapsiblePanel.Dock = DockStyle.Fill;
            collapsiblePanel.ExpandDirection = RadDirection.Right;

            rutaTextBox.NullText = strings.IDRuta;
            rutaTextBox.ShowNullText = true;
            rutaTextBox.Dock = DockStyle.Fill;

            descLbl.Text = Lenguaje.traduce("[VACÍO]");

            LlenarDiccionarioRutas();
            rutasList.DataSource = rutas;
            rutaTextBox.TextChanging += new TextChangingEventHandler(rutaTextbox_TextChanging);
            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);
            tableLayout.Controls.Add(rutaTextBox, 0, 0);
            tableLayout.Controls.Add(descLbl, 0, 1);            
            collapsiblePanel.Controls.Add(rutasList);
            tableLayout.Controls.Add(tableLayout2, 0, 2);
            tableLayout2.Controls.Add(btnAceptar, 0, 1);
            tableLayout2.Controls.Add(btnSalir, 1, 1);
            tableLayout.Controls.Add(tableLayout3, 2, 0);
            tableLayout3.Controls.Add(collapsiblePanel);*/
            InitializeComponent();
            this.gridPedidos = gridPedidos;
            this.Shown += form_Shown;
            this.Show();
            this.Text = Lenguaje.traduce("Código ruta");
            this.ShowIcon = false;

            rutaTextBox.NullText = strings.IDRuta;
            rutaTextBox.ShowNullText = true;
            rutaTextBox.Dock = DockStyle.Fill;

            descLbl.Text = Lenguaje.traduce("[VACÍO]");

            LlenarDiccionarioRutas();
            rutasList.DataSource = rutas;
            rutaTextBox.TextChanging += new TextChangingEventHandler(rutaTextbox_TextChanging);
            this.radButtonAceptar.Text = Lenguaje.traduce("Aceptar");
            this.radButtonCancelar.Text = Lenguaje.traduce("Cancelar");
        }

        private void LlenarDiccionarioRutas()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIORUTAS")));
            /*for (int i = 0; i < table.Rows.Count; i++)
            {
                rutas.Add(table.Rows[i][0].ToString(), table.Rows[i][1].ToString());
            }*/
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxRutas, table, Lenguaje.traduce("IDRUTA"), Lenguaje.traduce("RUTA DESCRIPCION"), "", null, true);
        }

        private void rutaTextbox_TextChanging(object sender, TextChangingEventArgs e)
        {
           // descLbl.Text = rutas.GetValueOrNull(rutaTextBox.Text);
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
            if (radMultiColumnComboBoxRutas.Text != null)
            {
                String codRuta = radMultiColumnComboBoxRutas.Text;
                if (String.IsNullOrEmpty(codRuta))
                    return;

                PedidosTan.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSCLIENTE") + " WHERE R.CODIGO ='" + codRuta + "'";
                gridPedidos.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(PedidosTan.q)).DefaultView;
                this.Close();
            }
            else
                this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

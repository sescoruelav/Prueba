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
    public partial class PorCodigoProveedor : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        RadLabel loteLbl = new RadLabel();
        RadLabel entradaLbl = new RadLabel();
        RadLabel salidaLbl = new RadLabel();
        RadTextBox loteTextBox = new RadTextBox();
        RadGridView proveedorList = new RadGridView();
        RadGridView gridproveedor;
        
        public PorCodigoProveedor(RadGridView gridproveedor)
        {
            /*InitializeComponent();
            this.gridproveedor = gridproveedor;
            this.Shown += form_Shown;
            this.Show();
            this.MaximizeBox = false; // Evita que el usuario maximice el formulario
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Evita que el usuario cambie el tamaño de la ventana
            this.Size = new Size(400, 400);
            tableLayout.Size = new Size(300, 250);
            proveedorList.Size = tableLayout.Size;
            tableLayout2.Size = new Size(300, 50);       
            this.Text = Lenguaje.traduce("Por Código de Proveedor");
            this.ShowIcon = false;
            tableLayout.Dock = DockStyle.Fill;
            proveedorList.Dock = DockStyle.Fill;
            loteTextBox.Dock = DockStyle.Fill;

            loteLbl.Text = strings.LabelLote;
            entradaLbl.Text = strings.entradaLbl;
            salidaLbl.Text = strings.salidaLbl;

            LlenarDiccionarioEstados();

            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;

            btnSalir.Text = Lenguaje.traduce("Salir");
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayout);          
            tableLayout.Controls.Add(proveedorList, 0, 0);
            tableLayout.Controls.Add(tableLayout2, 0, 1);                       
            tableLayout2.Controls.Add(btnAceptar, 0, 0);
            tableLayout2.Controls.Add(btnSalir, 1, 0);*/
            InitializeComponent();
            this.gridproveedor = gridproveedor;
            this.Shown += form_Shown;
            this.Show();
            this.Text = Lenguaje.traduce("Código ruta");
            this.ShowIcon = false;

            loteTextBox.NullText = "test"; //strings.IDProveedor;
            loteTextBox.ShowNullText = true;
            loteTextBox.Dock = DockStyle.Fill;

            loteLbl.Text = Lenguaje.traduce("[VACÍO]");

            LlenarDiccionarioEstados();
            //loteTextBox.TextChanging += new TextChangingEventHandler(loteTextBox_TextChanging);
            this.radButtonAceptar.Text = Lenguaje.traduce("Aceptar");
            this.radButtonCancelar.Text = Lenguaje.traduce("Cancelar");
        }

        private void LlenarDiccionarioEstados()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOPROVEEDOR")));
            /*proveedorList.DataSource = table;
            proveedorList.EnableGrouping = false;
            proveedorList.AllowAddNewRow = false;
            proveedorList.AllowEditRow = false;
            proveedorList.AllowDeleteRow = false;
            proveedorList.EnableFiltering = true;
            proveedorList.BestFitColumns();*/
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxProveedores, table, Lenguaje.traduce("IDPROVEEDOR"), Lenguaje.traduce("NOMBRE"), "", null, true);
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
            String idProveedor = radMultiColumnComboBoxProveedores.SelectedValue.ToString();
            if (String.IsNullOrEmpty(idProveedor))
                return;

            PedidosProv.q = ConfigurarSQL.CargarConsulta(path, "PEDIDOSPROVEEDOR") + " WHERE p.idproveedor='" + idProveedor + "'";
            gridproveedor.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(PedidosProv.q)).DefaultView;
            this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

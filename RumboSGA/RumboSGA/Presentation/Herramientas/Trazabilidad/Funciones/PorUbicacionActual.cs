using RumboSGA.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RumboSGA.Presentation.Herramientas.Trazabilidad;
using RumboSGAManager.Model.DataContext;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using System.Collections.Generic;
using Rumbo.Core.Herramientas;
using RumboSGAManager;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class PorUbicacionActual : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        Dictionary<string, string> ubicaciones = new Dictionary<string, string>();
        RadButton btnAceptar = new RadButton();
        RadButton btnSalir = new RadButton();
        TableLayoutPanel tableLayoutGeneral = new TableLayoutPanel();
        TableLayoutPanel tableLayout2 = new TableLayoutPanel();
        TableLayoutPanel tableLayout3 = new TableLayoutPanel();
        TableLayoutPanel tableLayoutBotones = new TableLayoutPanel();
        RadLabel descLbl = new RadLabel();
        RadTextBox ubicacionTextBox = new RadTextBox();
        RadListView ubicacionesList = new RadListView();
        RadGridView gridPedidos;
        
        public PorUbicacionActual(RadGridView gridPedidos)
        {
            InitializeComponent();
            this.gridPedidos = gridPedidos;
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(500, 200);
            this.Text = Lenguaje.traduce("Por Ubicación Actual");
            this.ShowIcon = false;
            tableLayoutGeneral.Dock = DockStyle.Fill;
            tableLayout2.Dock = DockStyle.Fill;
            tableLayout3.Dock = DockStyle.Fill;
            ubicacionesList.Dock = DockStyle.Fill;

            ubicacionTextBox.NullText = Lenguaje.traduce("Ubicación Actual");
            ubicacionTextBox.ShowNullText = true;
            ubicacionTextBox.Dock = DockStyle.Fill;

            descLbl.Text = Lenguaje.traduce("[VACÍO]");

            LlenarDiccionarioUbicaciones();
            ubicacionesList.DataSource = ubicaciones;
            ubicacionTextBox.TextChanging += new TextChangingEventHandler(ubicacionTextbox_TextChanging);
            ubicacionesList.SelectedIndexChanged += new EventHandler(ubicacionList_SelectedIndexChanged);
            btnAceptar.Text = Lenguaje.traduce("Aceptar");
            btnSalir.Text = Lenguaje.traduce("Salir");
            btnAceptar.Size = new Size(75, 25);
            btnAceptar.Click += aceptar_Event;
            btnSalir.Size = new Size(75, 25);
            btnSalir.Click += salir_Event;

            this.Controls.Add(tableLayoutGeneral);
            tableLayoutGeneral.Controls.Add(tableLayout2, 0, 0);
            tableLayoutGeneral.Controls.Add(tableLayout3, 1, 0);
            tableLayoutGeneral.Controls.Add(tableLayoutBotones, 0, 1);
            tableLayout2.Controls.Add(ubicacionTextBox, 0, 0);
            tableLayout2.Controls.Add(descLbl, 0, 1);
            tableLayout3.Controls.Add(ubicacionesList);
            tableLayoutBotones.Controls.Add(btnAceptar, 0, 0);
            tableLayoutBotones.Controls.Add(btnSalir, 1, 0);
        }

        private void LlenarDiccionarioUbicaciones()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOUBICACIONES")));
            for (int i = 0; i < table.Rows.Count; i++)
            {
                ubicaciones.Add(table.Rows[i][0].ToString(), table.Rows[i][1].ToString());
            }
        }

        private void ubicacionTextbox_TextChanging(object sender, TextChangingEventArgs e)
        {
            //descLbl.Text = ubicaciones.GetValueOrNull(ubicacionTextBox.Text);
            ubicacionesList.FilterDescriptors.Clear();

            if (String.IsNullOrEmpty(this.ubicacionTextBox.Text))
            {
                this.ubicacionesList.EnableFiltering = false;
            }
            else
            {
                this.ubicacionesList.FilterDescriptors.LogicalOperator = FilterLogicalOperator.Or;
                this.ubicacionesList.FilterDescriptors.Add("Ubicación", FilterOperator.Contains, this.ubicacionTextBox.Text);
                this.ubicacionesList.EnableFiltering = true;
            }
        }

        private void ubicacionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            descLbl.Text = ubicacionesList.SelectedItem.Text;
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
            Packing.q = ConfigurarSQL.CargarConsulta(path, "PACKINGLIST") + " WHERE PL.IDUBICACIONACTUAL='" + ubicacionTextBox.Text + "'";
            gridPedidos.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(Packing.q)).DefaultView;
            this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

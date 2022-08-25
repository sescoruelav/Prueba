using RumboSGA.Properties;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.GestionAlmacen
{
    public partial class BuscarRecepcion : Telerik.WinControls.UI.RadForm
    {
        public string recepcion = string.Empty, albaran = string.Empty, estado = string.Empty, muelle = string.Empty, proveedor = string.Empty, fCreacion = string.Empty,
                 fRecepcion = string.Empty, obs = string.Empty, transport = string.Empty, matricula = string.Empty, chofer = string.Empty;
        List<TableScheme> lstEsquemaTabla = new List<TableScheme>();
        public BuscarRecepcion()
        {
            InitializeComponent();
            lblFiltro.Text = strings.Filtro;
            LlenarGrid();
            radGridView1.DoubleClick += GridDoubleClick;
            radGridView1.ReadOnly = true;

            lblFiltro.Text = strings.Filtro;
            btnIniciarFiltro.Text = strings.Iniciar;
            IdiomaCombo();

        }
        private void IdiomaCombo()
        {
            if (CultureInfo.CurrentUICulture.Name=="es-ES")
            {
                comboFiltros.Items.AddRange(new string[] {
            "Recepción creada sin enviar al terminal",
            "Recepción creada enviada al terminal sin empezar",
            "Recepción creada enviada al terminal empezada" ,
            "Recepción creada enviada al terminal terminada",
            "Recepción creada enviada al terminal terminada con discrepancias",
            "Recepción creada enviada al terminal terminada sin discrepancias",
            "Recepción con todas líneas contrapedido"});
            }
            else
            {
                comboFiltros.Items.AddRange(new string[] {
            "Created reception without sending to terminal",
            "Created reception sent to the terminal without starting",
            "Created reception sent to the terminal started" ,
            "Created reception sent to the terminal and finished",
            "Created Reception sent to the terminal finished with discrepancies",
            "Created Reception sent to the terminal finished without discrepancies",
            "Recepción con todas líneas contrapedido"});
            }
            comboFiltros.SelectedIndex = 0;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            switch (comboFiltros.SelectedIndex)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;

                default:
                    break;
            }
        }
        private void LlenarGrid()
        {
            Business.GetRecepcionesPendientesCantidad(ref lstEsquemaTabla);
            //radGridView1.MasterView.Sho
            this.radGridView1.DataSource =Business.GetRecepcionesPendientesDatosGridView(lstEsquemaTabla);
            this.radGridView1.BestFitColumns();
        }
        private void CargarStrings()
        {

        }
        private void GridDoubleClick(object sender,EventArgs e)
        {
            if (radGridView1.SelectedRows[0].Index>=0)
            {
                //btnFiltrar.Text = strings.Filtrar;
                lblFiltro.Text = strings.Pedido;
                recepcion = radGridView1.SelectedRows[0].Cells["ID Recepcion"].Value.ToString();
                albaran = radGridView1.SelectedRows[0].Cells["Albaran Transportista"].Value.ToString();
                //estado= radGridView1.SelectedRows[0].Cells[""].ToString();
                muelle = radGridView1.SelectedRows[0].Cells["Muelle Desc."].Value.ToString();
                proveedor = radGridView1.SelectedRows[0].Cells["Chofer"].Value.ToString();
                //fCreacion= radGridView1.SelectedRows[0].Cells[""].ToString();
                fRecepcion = radGridView1.SelectedRows[0].Cells["Fecha Recepcion"].Value.ToString();
                //obs= radGridView1.SelectedRows[0].Cells[""].ToString();
                transport = radGridView1.SelectedRows[0].Cells["Transportista"].Value.ToString();
                matricula = radGridView1.SelectedRows[0].Cells["Matricula Camion"].Value.ToString();
                chofer = radGridView1.SelectedRows[0].Cells["Chofer"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }
    }
}

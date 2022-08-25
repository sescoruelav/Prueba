using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class CambiarPickingEstado : Form
    {
        List<string> campos;
        public string itemSeleccionado;
        public bool exito = false;
        public List<int> listEntradas = new List<int>();
        public CambiarPickingEstado(List<string> celdas)
        {
            this.Text = "Cambiar Estado de Picking";

            InitializeComponent();
            //this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            lblCambiarEstado.Text = strings.LabelCambiarEstado;
            campos = celdas;
            DataTable dt = DataAccess.getPickingEstado();
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBox1, dt, "IDPICKINGESTADO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int contador = 0;
            string where = "";
            itemSeleccionado = radMultiColumnComboBox1.SelectedValue.ToString();
            string error = string.Empty;
            foreach (var campo in campos)
            {
                if (contador == 0)
                {
                    where = "IDENTRADA=" + campo;
                    listEntradas.Add(int.Parse(campo.ToString()));
                    contador++;
                }
                else
                {
                    where += " OR IDENTRADA=" + campo;
                    listEntradas.Add(int.Parse(campo.ToString()));
                    contador++;
                }
            }
            string query = "UPDATE TBLEXISTENCIAS SET IDPICKINGESTADO='" + itemSeleccionado + "' WHERE " + where;
            ConexionSQL.SQLClienteExec(query, ref error);
            if (error == string.Empty)
            {
                exito = true;
            }
            else
            {
                exito = false;
            }
            this.Close();
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }
    }
}

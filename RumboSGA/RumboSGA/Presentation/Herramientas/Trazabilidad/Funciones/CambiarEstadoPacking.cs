using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.PackingListMotor;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class CambiarEstadoPacking : Telerik.WinControls.UI.ShapedForm
    {
        List<string> campos;
        public string itemSeleccionado;
        public bool exito = false;
        public CambiarEstadoPacking(List<string> celdas)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Cambiar estado de Picking");
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            lblCambiarEstado.Text = Lenguaje.traduce(strings.LabelCambiarEstado);
            campos = celdas;
           
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                itemSeleccionado = radDropDownList1.SelectedItem.Value.ToString();
                string error = string.Empty;
                string where = string.Empty;
                List<string> errores = new List<string>();
                foreach (var campo in campos)
                {
                    using (var ws = new WSPackingListMotorClient())
                    {
                        ws.cambiaEstadoPackingList(Int32.Parse(campo), itemSeleccionado);
                    }
                }         
                if (error != string.Empty)
                {
                    String errorTxt = "";

                    foreach (string errorStr in errores)
                    {
                        errorTxt = errorTxt + System.Environment.NewLine + errorStr;
                    }
                    RadMessageBox.Show(errorTxt, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                exito = false;
            }
            this.Close();
            exito = true;
        }

        private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            itemSeleccionado = radDropDownList1.SelectedItem.Value.ToString();
        }

        private void CambiarEstadoPicking_Load(object sender, EventArgs e)
        {
            var q = "SELECT * FROM TBLPACKINGESTADO";
            radDropDownList1.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(q)).DefaultView;
            radDropDownList1.DisplayMember = Lenguaje.traduce("DESCRIPCION");
            radDropDownList1.ValueMember = Lenguaje.traduce("IDPACKINGESTADO");
            radDropDownList1.AutoCompleteMode = AutoCompleteMode.Append;
        }
    }
}

using RumboSGA.Properties;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using RumboSGAManager;
using RumboSGA.EmbalajesMotor;
using RumboSGA.RecepcionMotor;
using Rumbo.Core.Herramientas;
using RumboSGAManager.Model;
using Rumbo.Core.Herramientas.Herramientas;

namespace RumboSGA.Presentation.FormularioRecepciones
{
    public partial class FrmAltaDirectaPreaviso : Telerik.WinControls.UI.RadForm
    {
        public int idRecepcion;
       
        
        public FrmAltaDirectaPreaviso(int idRecepcion_)
        {

            InitializeComponent();//TODO revisar si hace falta
            this.Text = Lenguaje.traduce("Alta directa preaviso");
            this.idRecepcion = idRecepcion_;
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;
            DataTable tblHuecos = DataAccess.getDataTableSQL(new String[] { "IDHUECO", "DESCRIPCION" }, "TBLHUECOS", " IDHUECOESTANTE = 'MU'");
            Utilidades.RellenarMultiColumnComboBox(ref rmccmbUbicacion, tblHuecos, "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
            Utilidades.RellenarMultiColumnComboBox(ref rmccmbEstado, DataAccess.GetIdDescripcionExistenciaEstados(), "IDEXISTENCIAESTADO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
            this.rCheckBoxConReserva.Text = Lenguaje.traduce(rCheckBoxConReserva.Text);


            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            WSRecepcionMotorClient recepcionmotor = null;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int idHueco = -1;
                string idExistenciaestado = "";
                if (rmccmbUbicacion.SelectedValue != null && !String.IsNullOrEmpty(rmccmbUbicacion.Text))
                {
                    idHueco = Convert.ToInt32(rmccmbUbicacion.SelectedValue);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar un hueco"), "Error");
                }
                if (rmccmbEstado.SelectedValue != null && !String.IsNullOrEmpty(rmccmbEstado.Text))
                {
                    idExistenciaestado = rmccmbEstado.SelectedValue.ToString();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Debes seleccionar un estado"), "Error");
                }
                recepcionmotor = new WSRecepcionMotorClient();
                object[] respuesta= recepcionmotor.generarEntradaDirectaPreavisoRecepcion(idRecepcion, idHueco, idExistenciaestado, DatosThread.getInstancia().getArrayDatos(), !rCheckBoxConReserva.Checked,User.IdOperario,0);
                if (respuesta.Length > 0)
                {
                    String errorTxt = "";

                    foreach (string errorStr in respuesta)
                    {
                        errorTxt = errorTxt + System.Environment.NewLine + Lenguaje.traduce(errorStr);
                    }
                    RadMessageBox.Show(errorTxt, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception exc)
            {
                ExceptionManager.GestionarErrorWS(exc, recepcionmotor.Endpoint);
            }
            this.Cursor = Cursors.Arrow;
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

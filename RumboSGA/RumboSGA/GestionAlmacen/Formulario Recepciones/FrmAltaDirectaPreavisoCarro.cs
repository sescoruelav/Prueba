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
    public partial class FrmAltaDirectaPreavisoCarro : Telerik.WinControls.UI.RadForm
    {
        public int idRecepcion;
       
        
        public FrmAltaDirectaPreavisoCarro(int idRecepcion_)
        {

            InitializeComponent();//TODO revisar si hace falta
            this.Text = Lenguaje.traduce("Alta directa preaviso");
            this.idRecepcion = idRecepcion_;
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;
            DataTable tblHuecos = DataAccess.getDataTableSQL(new String[] { "CC.IDCARRO", "H.DESCRIPCION" }, "dbo.TBLCARROMOVILCAB CC INNER JOIN dbo.TBLHUECOS H ON H.IDHUECO = CC.IDUBICACIONPRINCIPAL", "");
            Utilidades.RellenarMultiColumnComboBox(ref rmccmbUbicacion, tblHuecos, "IDCARRO", "DESCRIPCION", "", new String[] { "TODOS" }, true);            


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
               
                recepcionmotor = new WSRecepcionMotorClient();
                int result = recepcionmotor.generarEntradaDirectaPreavisoCarroRecepcion(idRecepcion, idHueco,  DatosThread.getInstancia().getArrayDatos(),User.IdOperario,0,User.NombreImpresora);
                if (result < 0)
                {
                   
                    RadMessageBox.Show("Se ha producido un error generando la entrada del preaviso", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception exc)
            {
                
                ExceptionManager.GestionarError(exc, "Se ha producido un error generando la entrada del preaviso." );
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

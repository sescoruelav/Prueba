using RumboSGA.Presentation;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Security;
using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.UserControls
{
    public partial class DataDialog : UserControl
    {
        #region Variables y Propiedades
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RadScrollablePanel PanelControles
        {
            get
            {
                return this.radScrollablePanel1;
            }
        }

        public RadButton BotonAceptarControl
        {
            get
            {
                return this.okButton;
            }
        }

        public RadButton BotonEditarControl
        {
            get
            {
                return this.editButton;
            }
        }

        public RadButton BotonCancelarControl
        {
            get
            {
                return this.cancelButton;
            }
        }

        #endregion

        #region Constructor

        public DataDialog()
        {
            InitializeComponent();

            //CheckForIllegalCrossThreadCalls = false;
        }

        #endregion
                
        #region Eventos - Botones

        private void okButton_Click(object sender, EventArgs e)
        {         
            var form = this.Parent.Parent as SFDetalles;
            if (form != null)
            {
                //log.Debug("El usuario con ID " + User.IdUsuario + " ha pulsado el botón de aceptar en "+tituloForm.Text+" en el modo "+form.ModoAperturaForm+" sobre el registro:"+ form.registro);
                //César: Para comprobar que los valores están bien introducidos
                //lo ideal sería rehacer SFDetalles con esto en mente en un futuro.
                if (form.ComprobarValores)
                {
                    string error = "";
                    form.getLineaValoresValidos(ref error);
                    if (!error.Equals(""))
                    {
                        RadMessageBox.Show(error);
                        return;
                    }
                }
                form.DialogResult = DialogResult.OK;
                form.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {            
            var form = this.Parent.Parent as SFDetalles;
            if (form != null)
            {
                form.DialogResult = DialogResult.Cancel;
                form.Close();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {            
            var form = this.Parent.Parent as SFDetalles;
            if (form != null)
            {         
                form.HabilitarDeshabilitarControles(SFDetalles.modoForm.edicion);
            }
        }

        #endregion
        public void SetLogInfo(string nombre,string modo)
        {

        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

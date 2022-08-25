using Rumbo.Core.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.LotesMotor;
using RumboSGA.Properties;
using RumboSGA.RecepcionMotor;
using RumboSGA.VariablesMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class AgregarUsuarioOperario : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        private void RadPanelPrincipal_Paint(object sender, PaintEventArgs e)
        {
        }

        private string idOperario;
        private string nombreElegido;
        private string claveElegida;

        #endregion Variables

        public AgregarUsuarioOperario(string idOperario)
        {
            this.idOperario = idOperario;

            InitializeComponent();
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.MaxLength = 5;

            this.Text = Lenguaje.traduce("Agregar Usuario");
            this.nombreUsuarioLabel.Text = Lenguaje.traduce(nombreUsuarioLabel.Text);
            this.passwordLabel.Text = Lenguaje.traduce(passwordLabel.Text);

            this.Icon = Resources.bruju;
        }

        #region Metodos

        private void AgregarUsuarioNuevo(string idOperario)

        {
            nombreElegido = this.UsuarioTextBox.Text.ToString();
            claveElegida = this.passwordTextBox.Text.ToString();
            /*if (nuevaPasswordTextBox.Text != null)
            {
                claveElegida = this.nuevaPasswordTextBox.Text.ToString();
            }*/

            try
            {
                DataAccess.NuevoUsuarioVinculadoOperario(nombreElegido, claveElegida);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

            this.Close();
        }

        #endregion Metodos

        #region MetodosPredefinidosYBotones

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            AgregarUsuarioNuevo(idOperario);
            log.Info("Agregando Usuario a Operario");
            String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
            bool errorBool = false;
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadTextBoxControlLote_Leave(object sender, EventArgs e)
        {
        }

        #endregion MetodosPredefinidosYBotones
    }
}
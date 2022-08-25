using Rumbo.Core.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.LotesMotor;
using RumboSGA.ProduccionMotor;
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
    public partial class CambiarPassword : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables
        private string claveElegida;

        private string idUsuario;

        private string idOperario;

        #endregion

        public CambiarPassword(string idUsuario, string idOperario)
        {
          
            this.idUsuario = idUsuario;
            this.idOperario = idOperario;
            InitializeComponent();
            
            
            this.Text = Lenguaje.traduce("Cambiar Contraseña");
           this.nuevaPassword.Text = Lenguaje.traduce(nuevaPassword.Text);
            this.Icon = Resources.bruju;
        }

        #region Metodos

      

        private void CambiarPasswordUsuario()
        {
            if (nuevaPasswordTextBox.Text != null) {
                claveElegida = this.nuevaPasswordTextBox.Text.ToString();
            }
            
            try
            {
                //cambia la clave para el usuario y el operario
                DataAccess.ActualizarPasswordUsuarioOperario(idUsuario, idOperario, claveElegida);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

            this.Close();


        }


        #endregion

        #region MetodosPredefinidosYBotones

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            log.Info("Cambiando la contraseña");
            CambiarPasswordUsuario();
            String errorMensaje = Lenguaje.traduce("Error al cambiar la contraseña");
            bool errorBool = false;

            
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}

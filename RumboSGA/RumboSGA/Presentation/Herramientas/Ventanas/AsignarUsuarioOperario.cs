using Rumbo.Core.Herramientas;
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

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class AsignarUsuarioOperario : Form
    {
        private static readonly log4net.ILog log =
             log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        private void RadPanelPrincipal_Paint(object sender, PaintEventArgs e)
        {
        }

        private string idOperario;
        private string idUsuario;

        #endregion Variables

        public AsignarUsuarioOperario(string idOperario, string idUsuario)
        {
            this.idOperario = idOperario;
            this.idUsuario = idUsuario;

            InitializeComponent();

            this.Text = Lenguaje.traduce("Asignar Usuario");
            this.nombreUsuarioLabel.Text = Lenguaje.traduce(nombreUsuarioLabel.Text);
            this.operarioLabel.Text = Lenguaje.traduce(operarioLabel.Text);

            this.Icon = Resources.bruju;
        }

        #region Metodos

        private void AsignarNuevoUsuarioOperario(string idOperario, string idUsuario)

        {
            idUsuario = this.UsuarioTextBox.Text.ToString();
            idOperario = this.operarioTextBox.Text.ToString();

            try
            {
                DataAccess.asignarUsuarioOperario(idUsuario, idOperario);
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
            AsignarNuevoUsuarioOperario(idOperario, idUsuario);
            log.Info("Agregando Usuario a Operario");
            String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
            bool errorBool = false;
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        # endregion MetodosPredefinidosYBotones
    }
}
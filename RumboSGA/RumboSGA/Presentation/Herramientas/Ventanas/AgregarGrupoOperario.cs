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
    public partial class AgregarGrupoOperario : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables
        private string idOperario;
        private string idGrupo;
        private string descripcionGrupo;

        #endregion

        public AgregarGrupoOperario(string idOperario)
        {
            
            this.idOperario = idOperario;
            InitializeComponent();
            
            
            this.Text = Lenguaje.traduce("Agregar Grupo");
           
            this.Icon = Resources.bruju;
        }

        #region Metodos

        private void AgregarGrupo()
        {
            if (descripcionGrupoTextBox.Text != null && idGrupoTextBox.Text != null)
            {
                idGrupo = this.idGrupoTextBox.Text.ToString();
                descripcionGrupo = this.descripcionGrupoTextBox.Text.ToString();
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Por favor, rellena todos los datos"));
            }
            
            try
            {
                DataAccess.CrearNuevoGrupo(idGrupo, descripcionGrupo);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }


        }



        #endregion

        #region MetodosPredefinidosYBotones

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            log.Info("Agregando Grupo");
            AgregarGrupo();
            String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
            bool errorBool = false;

            
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}

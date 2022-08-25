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
    public partial class EditarGrupoOperario : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string idUsuario;
        protected List<TableScheme> _lstEsquemaTablaGrupos = new List<TableScheme>();

        #region Variables
        private string idOperario;
        private string nombreUsuario;
        private string idGrupo;
        private string descripcionGrupo;
        private string grupoSeleccionado;

        #endregion

        public EditarGrupoOperario(string idUsuario, string idOperario, string nombreUsuario)
        {
            this.idUsuario = idUsuario;
            this.idOperario = idOperario;
            this.nombreUsuario = nombreUsuario;
            InitializeComponent();
            this.nombreOperarioTextBox.Text = nombreUsuario;
            this.nombreOperarioTextBox.Enabled = false;

            radMultiColumnComboBox1.DataSource = Business.GetDatosGridView(_lstEsquemaTablaGrupos, "Grupos", "IDGRUPO");
            radMultiColumnComboBox1.ValueMember = Lenguaje.traduce("Grupo");
            radMultiColumnComboBox1.DisplayMember = Lenguaje.traduce("Nombre");
            this.Text = Lenguaje.traduce("Cambiar Grupo");
            this.descripcionGrupoLabel.Text = Lenguaje.traduce(descripcionGrupoLabel.Text);
            this.Icon = Resources.bruju;
        }

        #region Metodos

        private void EditarGrupoDelUsuario()
        {
            grupoSeleccionado = radMultiColumnComboBox1.SelectedValue.ToString();
           /* if (descripcionGrupoTextBox.Text != null && idGrupoTextBox.Text != null)
            {
                idGrupo = this.idGrupoTextBox.Text.ToString();
                descripcionGrupo = this.descripcionGrupoTextBox.Text.ToString();
            }
            else
            {
                MessageBox.Show(Lenguaje.traduce("Por favor, rellena todos los datos"));
            }
            */
            try
            {
                DataAccess.EditarGrupoUsuario(grupoSeleccionado, idUsuario);
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
            log.Info("Cambiando el grupo del operario");
            EditarGrupoDelUsuario();
            String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
            bool errorBool = false;
            this.Close();

            
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}

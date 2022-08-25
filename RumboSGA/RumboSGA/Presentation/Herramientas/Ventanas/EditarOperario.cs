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
    public partial class EditarOperario : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string idUsuario;
        protected List<TableScheme> _lstEsquemaTablaOperarios = new List<TableScheme>();
        private string idOperario;

        #region Variables
        private string nombreOperario;
        private string estadoSeleccionado;
        private string apellidos;
        private string password;
        private string modoPantalla;
        private string fuerza;
        private string estado;
        private string volumen;
        private string ultimaMaquina;
        private string ultimaModificacion;
        private string sensibilidad;
        private string permisoRegularizacion;

        #endregion

        public EditarOperario(string idOperario, string nombreOperario, string apellidos, string modoPant, string estado, string volumen, string fuerza, string ultimaMaquina, string ultimaModificacion, string sensibilidad, string permisoRegularizacion)
        {
            
            InitializeComponent();

            List<string> items = new List<string>();

            items.Add("AC");
            items.Add("IN");
            this.idOperario = idOperario;
            this.nombreOperario = nombreOperario;
            this.apellidos = apellidos;
            this.modoPantalla = modoPant;
            this.volumen = volumen;
            this.fuerza = fuerza;
            this.estado = estado;
            this.ultimaMaquina = ultimaMaquina;
            this.ultimaModificacion = ultimaModificacion;
            this.sensibilidad = sensibilidad;
            this.permisoRegularizacion = permisoRegularizacion;
            radDropDownList1.DataSource = items;
            radDropDownList1.ValueMember = Lenguaje.traduce("IDOperario");
            radDropDownList1.DisplayMember = Lenguaje.traduce("Estado");
            this.Text = Lenguaje.traduce("Editar Operario");
            CargarDatosOperario();
           
            this.Icon = Resources.bruju;
        }

        #region Metodos

        private void CargarDatosOperario()
        {
            this.radDropDownList1.SelectedValue = estado;
            this.nombreOperarioTextBox.Text = nombreOperario;
            this.apellidosTextBox.Text = apellidos;
            this.modoPantallaTextBox.Text = modoPantalla;
            this.fuerzaTextBox.Text = fuerza;
            this.volumenTextBox.Text = volumen;
            this.ultimaMaquinaTextBox.Text = ultimaMaquina;
            this.ultimaModificacionTextBox.Text = ultimaModificacion;
            this.sensibilidadTextBox.Text = sensibilidad;
            this.permisoRegularizacionTextBox.Text = permisoRegularizacion;
        }

        private void EditarUnOperario()
        {
            estadoSeleccionado = radDropDownList1.SelectedValue.ToString();
            nombreOperario = this.nombreOperarioTextBox.Text;
            apellidos = this.apellidosTextBox.Text;
            modoPantalla = this.modoPantallaTextBox.Text;
            fuerza = this.fuerzaTextBox.Text;
            volumen = this.volumenTextBox.Text;
            ultimaMaquina = this.ultimaMaquinaTextBox.Text;
            ultimaModificacion = this.ultimaModificacionTextBox.Text;
            sensibilidad = this.sensibilidadTextBox.Text;
            permisoRegularizacion = this.permisoRegularizacionTextBox.Text;
            
            try
            {
                DataAccess.EditarOperario(nombreOperario, apellidos, password, modoPantalla, fuerza, volumen, ultimaMaquina, ultimaModificacion,
                    sensibilidad, permisoRegularizacion, estadoSeleccionado, idOperario);
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
            log.Info("Editando el operario");
            EditarUnOperario();
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

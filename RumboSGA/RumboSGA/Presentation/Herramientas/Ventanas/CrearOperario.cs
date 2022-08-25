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
    public partial class CrearOperario : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string idUsuario;
        protected List<TableScheme> _lstEsquemaTablaOperarios = new List<TableScheme>();

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

        public CrearOperario()
        {
            //este constructor se utiliza para recuperar los datos en el método nuevo
            
            InitializeComponent();

            List<string> items = new List<string>();

            items.Add("AC");
            items.Add("IN");

            
            radDropDownList1.DataSource = items;
            radDropDownList1.ValueMember = Lenguaje.traduce("IDOperario");
            radDropDownList1.DisplayMember = Lenguaje.traduce("Estado");
            this.Text = Lenguaje.traduce("Crear Operario");
           
            this.Icon = Resources.bruju;
        }

        public CrearOperario(string nombreOperario,string apellidos, string modoPant, string estado, string volumen, string fuerza, string ultimaMaquina, string ultimaModificacion, string sensibilidad,string permisoRegularizacion )
        {
            //este constructor se utiliza para recuperar los datos en el método clonar
            InitializeComponent();

            List<string> items = new List<string>();

            items.Add("AC");
            items.Add("IN");
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
            this.Text = Lenguaje.traduce("Clonar Operario");

            this.Icon = Resources.bruju;
            //cargamos los datos del operario seleccionado, salvo su id y su contraseña, que se deberán asignar nuevas
            CargarDatosOperario();
            
        }

            #region Metodos

            private void NuevoOperario()
        {
            //asignamos los datos tanto en el método nuevo como clonar
            estadoSeleccionado = radDropDownList1.SelectedValue.ToString();
            nombreOperario = this.nombreOperarioTextBox.Text;
            apellidos = this.apellidosTextBox.Text;
            password = this.passwordTextBox.Text;
            modoPantalla = this.modoPantallaTextBox.Text;
            fuerza = this.fuerzaTextBox.Text;
            volumen = this.volumenTextBox.Text;
            ultimaMaquina = this.ultimaMaquinaTextBox.Text;
            ultimaModificacion = this.ultimaModificacionTextBox.Text;
            permisoRegularizacion = this.permisoRegularizacionTextBox.Text;
            
            try
            {
                DataAccess.NuevoOperario(nombreOperario, apellidos, password, modoPantalla, fuerza, volumen, ultimaMaquina, ultimaModificacion,
                    permisoRegularizacion, estadoSeleccionado);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }


        }

        

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
            this.permisoRegularizacionTextBox.Text = permisoRegularizacion;
        }


        #endregion

        #region MetodosPredefinidosYBotones

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            log.Info("Cambiando el grupo del operario");
            NuevoOperario();
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

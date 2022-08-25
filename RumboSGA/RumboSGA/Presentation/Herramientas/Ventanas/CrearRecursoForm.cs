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
    public partial class CrearRecursoForm : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string idUsuario;
        protected List<TableScheme> _lstEsquemaTablaOperarios = new List<TableScheme>();

        private static String[] camposUbicacion = {"IDHUECO","DESCRIPCION"};
        private static String[] filtros = {"TODOS"};
        private static string fromUbicacion = "TBLHUECOS";
        private string estadoSeleccionado;
        private string idRecurso;

        #region Variables
        private string descripcion;
        private string nombreImpresora;
        private string estado;
        private string maxTareas;
        private string fechaLogIn;
        private string fechaLogout;
        private string ubicacionActual;
        private string ubicacionPrincipal;
        private string modoForm;

        #endregion

        public CrearRecursoForm()
        {
            //este constructor se utiliza para recuperar los datos en el método nuevo
            
            InitializeComponent();

            List<string> items = new List<string>();

            items.Add("AC");
            items.Add("IN");

            
            radDropDownList1.DataSource = items;
            radDropDownList1.ValueMember = Lenguaje.traduce("Recurso");
            radDropDownList1.DisplayMember = Lenguaje.traduce("Estado");
            this.Text = Lenguaje.traduce("Crear Recurso");


            DataTable dtActual = DataAccess.getDataTableSQL(camposUbicacion, fromUbicacion,"");
            DataTable dtPrincipal = DataAccess.getDataTableSQL(camposUbicacion, fromUbicacion,"");
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacionActual, dtActual, camposUbicacion[0], camposUbicacion[1], "", filtros, true);
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacionPrincipal, dtPrincipal, camposUbicacion[0], camposUbicacion[1], "", filtros, true);


            this.Icon = Resources.bruju;
        }

        public CrearRecursoForm(string descripcion, string nombreImpresora, string estado, string maxTareas, string fechaLogin, string fechaLogout, string ubicacionActual, string ubicacionPrincipal )
        {
            //este constructor se utiliza para recuperar los datos en el método clonar
            InitializeComponent();

            List<string> items = new List<string>();

            items.Add("AC");
            items.Add("IN");
            this.descripcion = descripcion;
            this.nombreImpresora = nombreImpresora;
            this.maxTareas = maxTareas;
            this.estado = estado;
            this.fechaLogIn = fechaLogin;
            this.fechaLogout = fechaLogout;
            this.ubicacionActual = ubicacionActual;
            this.ubicacionPrincipal = ubicacionPrincipal;

            radDropDownList1.DataSource = items;
            radDropDownList1.ValueMember = Lenguaje.traduce("Recurso");
            radDropDownList1.DisplayMember = Lenguaje.traduce("Estado");
            this.Text = Lenguaje.traduce("Clonar Recurso");

            DataTable dtActual = DataAccess.getDataTableSQL(camposUbicacion, fromUbicacion,"");
            DataTable dtPrincipal = DataAccess.getDataTableSQL(camposUbicacion, fromUbicacion,"");
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacionActual, dtActual, camposUbicacion[0], camposUbicacion[1], "", filtros, true);
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacionPrincipal, dtPrincipal, camposUbicacion[0], camposUbicacion[1], "", filtros, true);

            this.Icon = Resources.bruju;
            //cargamos los datos del recurso seleccionado
            CargarDatosRecurso();
            
        }

        public CrearRecursoForm(string id, string descripcion, string nombreImpresora, string estado, string maxTareas, string fechaLogin, string fechaLogout, string ubicacionActual, string ubicacionPrincipal)
        {
            //este constructor se utiliza para recuperar los datos en el método clonar
            InitializeComponent();

            List<string> items = new List<string>();

            items.Add("AC");
            items.Add("IN");
            this.idRecurso = id;
            this.descripcion = descripcion;
            this.nombreImpresora = nombreImpresora;
            this.maxTareas = maxTareas;
            this.estado = estado;
            this.fechaLogIn = fechaLogin;
            this.fechaLogout = fechaLogout;
            this.ubicacionActual = ubicacionActual;
            this.ubicacionPrincipal = ubicacionPrincipal;

            radDropDownList1.DataSource = items;
            radDropDownList1.ValueMember = Lenguaje.traduce("Recurso");
            radDropDownList1.DisplayMember = Lenguaje.traduce("Estado");
            this.Text = Lenguaje.traduce("Editar Recurso");

            DataTable dtActual = DataAccess.getDataTableSQL(camposUbicacion, fromUbicacion,"");
            DataTable dtPrincipal = DataAccess.getDataTableSQL(camposUbicacion, fromUbicacion,"");
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacionActual, dtActual, camposUbicacion[0], camposUbicacion[1], "", filtros, true);
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacionPrincipal, dtPrincipal, camposUbicacion[0], camposUbicacion[1], "", filtros, true);

            this.Icon = Resources.bruju;
            //cargamos los datos del recurso seleccionado
            CargarDatosRecurso();
            modoForm = "editar";

        }

        #region Metodos

        private void NuevoRecurso()
        {
            //asignamos los datos tanto en el método nuevo como clonar
            estadoSeleccionado = radDropDownList1.SelectedValue.ToString();
            descripcion = this.descripcionTextBox.Text;
            nombreImpresora = this.nombreImpresoraTextBox.Text;
            maxTareas = this.maxTareasTextBox.Text;
            fechaLogIn = this.fechaLoginTextBox.Text;
            fechaLogout = this.fechaLogoutTextBox.Text;
            if (radMultiColumnComboBoxUbicacionActual.SelectedValue != null) {
                ubicacionActual = this.radMultiColumnComboBoxUbicacionActual.SelectedValue.ToString();
            }

            if (radMultiColumnComboBoxUbicacionPrincipal.SelectedValue != null)
            {
                ubicacionPrincipal = this.radMultiColumnComboBoxUbicacionPrincipal.SelectedValue.ToString();
            }
            

            try
            {
                DataAccess.NuevoRecurso(estadoSeleccionado, descripcion, nombreImpresora, maxTareas, fechaLogIn, fechaLogout, ubicacionActual, ubicacionPrincipal);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }


        }

        private void EditarRecurso()
        {
            //asignamos los datos tanto en el método nuevo como clonar
            estadoSeleccionado = radDropDownList1.SelectedValue.ToString();
            descripcion = this.descripcionTextBox.Text;
            nombreImpresora = this.nombreImpresoraTextBox.Text;
            maxTareas = this.maxTareasTextBox.Text;
            fechaLogIn = this.fechaLoginTextBox.Text;
            fechaLogout = this.fechaLogoutTextBox.Text;
            ubicacionActual = this.radMultiColumnComboBoxUbicacionActual.SelectedValue.ToString();
            ubicacionPrincipal = this.radMultiColumnComboBoxUbicacionPrincipal.SelectedValue.ToString();

            try
            {
                //Falta probar este metodo correctamente 
                DataAccess.EditarRecurso(idRecurso, estadoSeleccionado, descripcion, nombreImpresora, maxTareas, fechaLogIn, fechaLogout, ubicacionActual, ubicacionPrincipal);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            this.Close();

        }



        private void CargarDatosRecurso()
        {
            this.radMultiColumnComboBoxUbicacionActual.Text = ubicacionActual;
            this.radMultiColumnComboBoxUbicacionPrincipal.Text = ubicacionPrincipal;
            this.radDropDownList1.SelectedValue = estado;
            this.descripcionTextBox.Text = descripcion;
            this.nombreImpresoraTextBox.Text = nombreImpresora;
            this.maxTareasTextBox.Text = maxTareas;
            this.fechaLoginTextBox.Text = fechaLogIn;
            this.fechaLogoutTextBox.Text = fechaLogout;
        }


        #endregion

        #region MetodosPredefinidosYBotones

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            if (modoForm == "editar")
            {
                log.Info("Creando nuevo recurso");
                EditarRecurso();
                String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
                bool errorBool = false;
                this.Close();
            }
            else
            {
                log.Info("Creando nuevo recurso");
                NuevoRecurso();
                String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
                bool errorBool = false;
                this.Close();
            }

            
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}

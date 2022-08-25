using RumboSGA.Properties;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Security;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Telerik.WinControls.UI;
using RumboSGAManager.Model;
using RumboSGAManager;
using RumboSGAManager.Model.Entities;
using System.IO;
using Rumbo.Core.Herramientas;
using System.Drawing;

namespace RumboSGA.Presentation
{
    public partial class SFLoginForm : Telerik.WinControls.UI.ShapedForm
    {
        #region Variables y Propiedades

        private bool _validado = false;

        public bool Validado
        { get { return _validado; } set { _validado = value; } }

        public string idiomaStr = String.Empty;
        private string avisoLogin, errorLogin;

        #endregion Variables y Propiedades

        #region Constructor

        public SFLoginForm()
        {
            InitializeComponent();
            InitializeIdiomas();
            inicializarComboBases();
            InicializarImagenLogo();
            this.StartPosition = FormStartPosition.CenterScreen;
            //this.radLabel1.Focus(); //Para que no se posicione sobre el textbox de USER inicialmente
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            lblVersion.Text = "V." + fvi.FileVersion;
            Persistencia.VersionPC = fvi.FileVersion;
            DBDropdown.Font = tbUser.Font;
            this.tbUser.Select(); //Incidencia 271 - Focus sobre textbox de USER
            idiomasDropDownList.TabStop = false; //Incidencia 271 - Tab se salta idiomas
            DBDropdown.TabStop = false; //Incidencia 271 - Tab se salta instalación

            //Para cargar unos ajustes basandose en el ultimo loggin.
            cargarDatosInicio();
        }

        private void InicializarImagenLogo()
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\Configs";
                if (File.Exists(path + @"\logoEmpresa.png"))
                    pictureBox1.Image = Image.FromFile(path + @"\logoEmpresa.png");
            }
            catch (Exception ex)
            {
                //TODO gestion de errores
            }
        }

        private void InitializeIdiomas()
        {
            RadListDataItem itemCastellano = new RadListDataItem(strings.Español);
            itemCastellano.Tag = "español";
            itemCastellano.Image = Resources.spain_flag_icon;
            RadListDataItem itemIngles = new RadListDataItem(strings.Ingles);
            itemIngles.Tag = "ingles";
            itemIngles.Image = Resources.united_kingdom_flag_icon;
            //idiomasDropDownList

            idiomasDropDownList.Items.Add(itemCastellano);
            idiomasDropDownList.Items.Add(itemIngles);
            idiomasDropDownList.SelectedIndexChanged += IdiomasDropDown_SelectedIndexChanged;
            if (Persistencia.Idioma == 0)
            {
                idiomasDropDownList.SelectedItem = itemCastellano;
                errorLogin = strings.ErrorLogin;
                avisoLogin = strings.AvisoLogin;
            }
            else
            {
                idiomasDropDownList.SelectedItem = itemIngles;
                errorLogin = "Login failed";
                avisoLogin = "User name or password are empty";
            }
            idiomasDropDownList.AutoCompleteMode = AutoCompleteMode.Append;
            titleBarControl1.radPanel1.MouseDown += panel1_MouseDown;
            DBDropdown.SelectedIndexChanged += DBDropDown_SelectedIndexChanged;
        }

        private void IdiomasDropDown_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            cambiarIdioma();
        }

        private void cambiarIdioma()
        {
            RadDropDownList lista = (RadDropDownList)this.idiomasDropDownList;
            if (lista.SelectedItem == null)
            {
                MessageBox.Show(Lenguaje.traduce("Por favor,seleccione una fila"));
                return;
            }
            switch (lista.SelectedItem.Tag)
            {
                case "español":
                    Lenguaje.cambiarLenguaje("es");
                    radLabel1.Text = "Identificación";
                    tbUser.NullText = "Usuario";
                    tbPassword.NullText = "Contraseña";
                    btnAceptar.Text = "Aceptar";
                    btnCancelar.Text = "Cancelar";
                    idiomaStr = "es";
                    errorLogin = strings.ErrorLogin;
                    avisoLogin = strings.AvisoLogin;
                    Persistencia.Idioma = 0;
                    // Properties.Settings.Default["idioma"] = 0;
                    // Properties.Settings.Default.Save();
                    break;

                case "ingles":
                    Lenguaje.cambiarLenguaje("en");
                    radLabel1.Text = "Identification";
                    tbUser.NullText = "User";
                    tbPassword.NullText = "Password";
                    btnAceptar.Text = "Accept";
                    btnCancelar.Text = "Cancel";
                    idiomaStr = "en";
                    errorLogin = "Login failed";
                    avisoLogin = "User name or password are empty";
                    Persistencia.Idioma = 1;
                    //Properties.Settings.Default["idioma"] = 1;
                    //Properties.Settings.Default.Save();
                    break;
            }
        }

        #endregion Constructor

        #region Eventos

        private void guardarDatosInicio(string usuario, string idioma, string configuracion)
        {
            try
            {
                Properties.Settings.Default.usuario = usuario;
                Properties.Settings.Default.idioma = idioma;
                Properties.Settings.Default.configuracion = configuracion;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void cargarDatosInicio()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.usuario))
            {
                tbUser.Text = Properties.Settings.Default.usuario;
            }
            if (!String.IsNullOrEmpty(Properties.Settings.Default.idioma))
            {
                idiomasDropDownList.Text = Properties.Settings.Default.idioma;
            }
            if (!String.IsNullOrEmpty(Properties.Settings.Default.configuracion))
            {
                DBDropdown.Text = Properties.Settings.Default.configuracion;
                DBDropdown.SelectedValue = DBDropdown.Text;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            cambiarIdioma();
            if (DBDropdown.SelectedItem == null)
            {
                MessageBox.Show(Lenguaje.traduce("No ha seleccionado instalación"));
                return;
            }
            string db = DBDropdown.SelectedItem.Text;
            string path = Persistencia.DirectorioBase + @"\Configs";

            foreach (var item in Directory.GetDirectories(path))
            {
                if (Path.GetFileName(item) == db)
                {
                    Persistencia.ConfigPath = item;
                    //Si la carpeta tiene su propio fichero de configuración,lo cargamos
                    if (File.Exists(Persistencia.ConfigPath + @"\services.config"))
                    {
                        AppConfig.Change(Persistencia.ConfigPath + @"\services.config");
                        ConexionSQL.setNombreConexion(db);
                        string con = ConfigurationManager.ConnectionStrings[db].ConnectionString;
                        ConexionSQL.setConnectionString(con);
                    }
                    break;
                }
            }
            ///////////////////////////////////////
            string user = tbUser.Text;
            string password = tbPassword.Text;
            if (user != String.Empty && password != String.Empty && Security.validarLogin(user, password))
            {
                Validado = true;
                guardarDatosInicio(user, idiomasDropDownList.Text, db);

                this.Close();
            }
            else if (user == String.Empty || password == String.Empty)
            {
                MessageBox.Show(avisoLogin);
            }
            else
            {
                Validado = false;
                MessageBox.Show(errorLogin);
            }
        }

        private void inicializarComboBases()
        {
            try
            {
                string path = Persistencia.DirectorioBase + @"\Configs";
                foreach (string item in Directory.GetDirectories(path))
                {
                    DBDropdown.Items.Add(Path.GetFileName(item));
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
            }

            if (DBDropdown.Items.Count != 0)
            {
                DBDropdown.SelectedIndex = 0;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DBDropDown_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
        }

        #endregion Eventos

        //codigo para poder mover el formulario desde el control superior
        public const int WM_NCLBUTTONDOWN = 0xA1;

        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
    }
}
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RumboSGAManager
{
    public static class Persistencia
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DataTable parametros;
        private static string directorioBase = @"C:\Rumbo";
        private static string directorioGlobal = @"X:\Instalacion\RumboEstilos";
        private static string directorioLocal = @"C:\Rumbo\RumboEstilos";
        private static string directorioInformes = @"X:\Informes";
        private static string smtpServer = "";
        private static string reportServerUrl = "";
        private static string reportServerUser = "";
        private static string reportServerPwd = "";
        private static string reportServerPath = "";
        private static int idioma = 0;
        private static int gridRowHeight = 40;
        private static string configPath;       
        private static DataTable Parametros { get => parametros; set => parametros = value; }
        public static string ConfigPath { get => configPath; set => configPath = value; }
        public static int NumRegistrosVirtualGrid = 100;

        //RefrescarButtonIluminacion
        private static int alertaRefrescoTiempo = -1;
        public static int TimerRefrescoCambioColor = 400;
        public static Color colorFondoRefresco = Color.Red;
        
        public static void inicializarParametros()
        {//TODO debe set un setParametros al que se le pasa un datatable. Si parametros es null se asigna,
         //si no se añaden parametros actualizando contenido de los existentes. Y quitar el dataaccess 
            try
            {
                parametros = DataAccess.GetParametrosTodos();
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
            }
        }
        public static string getParametro(String nombre, int indice)
        {
            try
            {
                String s = getParametro(nombre);
                if (s == null || s.Length == 0)
                {
                    return "";
                }
                if (indice >= s.Length)
                {
                    return "";
                }
                return "" + s[indice];
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e,e.Message);
            }
            return "";
        }
        public static string getParametro(string name)
        {
            try
            {
                DataRow[] parametro = Parametros.Select("PARAMETRO='" + name +"'");
                if (parametro.Count() > 0)
                {
                    DataRow par=parametro[0];
                    return par["valor"].ToString();
                }
                else
                {
                    log.Debug("No existe parametro" + name);
                    return "";
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
                throw;
            }
        }
        public static int getParametroInt(string name)
        {
            try
            {
                DataRow[] parametro = Parametros.Select("PARAMETRO='" + name+"'");
                if (parametro.Count() > 0)
                {
                    DataRow par = parametro[0];
                    return Convert.ToInt32(par["valor"]);
                }
                else
                {
                    log.Debug("No existe parametro" + name);
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
            }
            return -9999;
        }
        public static bool getParametroBoolean(string name)
        {
            try
            {
                DataRow[] parametro = Parametros.Select("PARAMETRO='" + name + "'");
                if (parametro.Count() > 0)
                {
                    DataRow par = parametro[0];
                    return Convert.ToBoolean(par["valor"]);
                }
                else
                {
                    log.Debug("No existe parametro" + name);
                   
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
            }
            return false;
        }

        public static string DirectorioBase
        {
            get {
                try
                {
                    String dirBase = ConfigurationManager.AppSettings["directorioBase"];
                    if (!String.IsNullOrEmpty(dirBase))
                    {
                        directorioBase = dirBase;
                    }
                    return directorioBase;
                }
                catch (Exception e)
                {
                    ExceptionManager.GestionarError(e, e.Message);
                    throw;
                }


            }
            set { directorioBase = value; }
        }
        /**
         * 1 Sí
         * 0 No
         */
        public static bool TorreControl
        {
            get
            {
                try
                {
                    string valor = ConfigurationManager.AppSettings["torreControl"];
                    if (String.IsNullOrEmpty(valor)) return true;
                    if (valor.Equals("true")) return true;
                    return false;
                }
                catch(Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    return false;
                }

            }
        }

        public static bool AlertaRefresco
        {
            get
            {
                try
                {
                    string valor = ConfigurationManager.AppSettings["alertaRefresco"];
                    if (String.IsNullOrEmpty(valor)) return true;
                    if (valor.Equals("true")) return true;
                    return false;
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    return false;
                }

            }
        }

        public static int AlertaRefrescoTiempo
        {
            get
            {
                try
                {
                    if (alertaRefrescoTiempo != -1) return alertaRefrescoTiempo;

                    string valor = ConfigurationManager.AppSettings["alertaRefrescoTiempo"];
                    if (String.IsNullOrEmpty(valor)) alertaRefrescoTiempo = 300000;
                    int valorInt = -1;
                    if(int.TryParse(valor,out valorInt))
                    {
                        alertaRefrescoTiempo = valorInt;
                    }
                    else
                    {
                        alertaRefrescoTiempo = 300000;
                    }
                    return alertaRefrescoTiempo;
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    return 300000;
                }

            }
            set { alertaRefrescoTiempo = value; }
        }

        public static string DirectorioInformes
        {
            get
            {
                try { 
                String dirBase = ConfigurationManager.AppSettings["directorioInformes"];
                if (!String.IsNullOrEmpty(dirBase))
                {
                    directorioInformes = dirBase;
                }
                return directorioInformes;
                }
                catch (Exception e)
                {
                    ExceptionManager.GestionarError(e, e.Message);
                    throw;
                }

            }
            set { directorioInformes = value; }
        }
        public static string DirectorioGlobal
        {
            get
            {
                try { 
                String dirBase = ConfigurationManager.AppSettings["directorioGlobal"];
                if (!String.IsNullOrEmpty(dirBase))
                {
                    directorioGlobal = dirBase;
                }
                return directorioGlobal;
                }
                catch (Exception e)
                {
                    ExceptionManager.GestionarError(e, e.Message);
                    throw;
                }

            }
            set { directorioBase = value; }
        }
        public static string DirectorioLocal
        {
            get
            {
                String dirBase = ConfigurationManager.AppSettings["directorioLocal"];
                if (!String.IsNullOrEmpty(dirBase))
                {
                    directorioLocal = dirBase;
                }
                return directorioLocal;


            }
            set { directorioBase = value; }
        }
        public static string ReportServerUrl
        {
            get
            {
                String url = ConfigurationManager.AppSettings["ReportServerUrl"];
                if (!String.IsNullOrEmpty(url))
                {
                    reportServerUrl = url;
                }
                return reportServerUrl;


            }
            set { reportServerUrl = value; }
        }
        public static string ReportServerUser
        {
            get
            {
                String url = ConfigurationManager.AppSettings["reportServerUser"];
                if (!String.IsNullOrEmpty(url))
                {
                    reportServerUser = url;
                }
                return reportServerUser;


            }
            set { reportServerUser = value; }
        }
        public static string ReportServerPwd
        {
            get
            {
                String url = ConfigurationManager.AppSettings["reportServerPwd"];
                if (!String.IsNullOrEmpty(url))
                {
                    reportServerPwd = url;
                }
                return reportServerPwd;


            }
            set { reportServerPwd = value; }
        }
        public static string ReportServerPath
        {
            get
            {
                String url = ConfigurationManager.AppSettings["reportServerPath"];
                if (!String.IsNullOrEmpty(url))
                {
                    reportServerPath = url;
                }
                return reportServerPath;


            }
            set { reportServerPath = value; }
        }
        public static int Idioma
        {
            get
            {

                string path = @"XML\" + Environment.UserName + @"\PersonalizacionUsuario.xml";
                int idioma = 0;
                try
                {
                    XDocument X = XDocument.Load(path);
                    idioma = Convert.ToInt32(X.Element("Personalizacion").Element("Idioma").Value);
                }
                catch (Exception e)
                {
                    log.Error(path);
                    log.Error(e.Message);
                }
                return idioma;
            }
            set
            {
                try { 
                idioma = value;
                string path = @"XML\" + Environment.UserName + @"\PersonalizacionUsuario.xml";
                if (!Directory.Exists(@"XML\" + Environment.UserName))
                {
                    Directory.CreateDirectory(@"XML\" + Environment.UserName);
                    if (File.Exists(@"XML\PersonalizacionUsuario.xml"))
                    {
                        File.Copy(@"XML\PersonalizacionUsuario.xml", @"XML\" + Environment.UserName + @"\PersonalizacionUsuario.xml");
                    }
                }
                else
                {
                    if (!File.Exists(@"XML\" + Environment.UserName + @"\PersonalizacionUsuario.xml"))
                    {
                        if (File.Exists(@"XML\PersonalizacionUsuario.xml"))
                        {
                            File.Copy(@"XML\PersonalizacionUsuario.xml", @"XML\" + Environment.UserName + @"\PersonalizacionUsuario.xml");
                        }
                    }
                }

                if (File.Exists(@"XML\" + Environment.UserName + @"\PersonalizacionUsuario.xml")) { 
                    XDocument X = XDocument.Load(path);
                    X.Element("Personalizacion").Element("Idioma").SetValue(idioma);
                    X.Save(path);
                }
                }
                catch (Exception e)
                {
                    ExceptionManager.GestionarError(e, e.Message);
                    throw;
                }
            }
        }

      

        public static int GridRowHeight
        {
            get{
                int height = 40;
                string path = @"XML\"+Environment.UserName+ @"\PersonalizacionUsuario.xml";

                try
                {
                    XDocument X = XDocument.Load(path);
                    if(X.Element("XML") != null && X.Element("XML").Element("RowHeight").Value != null)
                        int.TryParse(X.Element("XML").Element("RowHeight").Value, out height);
                }
                catch (Exception e)
                {
                    log.Error(path);
                    log.Error(e.Message);
                }
                return height;
            }
        }

        public static string VersionPC { get; set; }
        public static string VersionWS { get; set; }
        public static string VersionDebug { get; set; }     
        public static string SmtpServer
        {
            get
            {
                String url = ConfigurationManager.AppSettings["SmtpServer"];
                if (!String.IsNullOrEmpty(url))
                {
                    smtpServer = url;
                }
                return smtpServer;


            }
            set { smtpServer = value; }
        }
    }
    
    


}

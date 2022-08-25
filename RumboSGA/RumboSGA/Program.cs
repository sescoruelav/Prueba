using Rumbo.Core.Herramientas;
using RumboSGA.Presentation;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using RumboSGA.VariablesMotor;
using RumboSGA.CargaMotor;
using RumboSGA.EmbalajesMotor;
using RumboSGA.EntradaMotor;
using RumboSGA.LotesMotor;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.PackingListMotor;
using RumboSGA.PedidoCliMotor;
using RumboSGA.ProduccionMotor;
using RumboSGA.RecepcionMotor;
using RumboSGA.ReservaMotor;
using RumboSGA.SalidaMotor;
using RumboSGA.TareasMotor;
using RumboSGA.UbicarMotor;
using RumboSGA.Properties;
using System.Data.SqlClient;

namespace RumboSGA
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        ///
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class LenguajeFicheroConfiguracion : ILenguajeFicheroConfiguracion
        {
            public string GetFicheroConfiguracion(string lenguaje)
            {
                //String st = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

                //String fichero = XmlReaderPropio.getLenguajePath(lenguaje);
                String fichero = ConfigurationManager.AppSettings["lenguajes"]
                    + System.IO.Path.DirectorySeparatorChar + lenguaje + ".properties";
                /*if (!String.IsNullOrEmpty(fichero))
                {
                    directorioBase = dirBase;
                }*/
                return fichero;
            }

            public String GetEndondig(String lenguaje)
            {
                return "ISO-8859-1";
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                try
                {
                    if (args.Length > 0)
                    {
                        string debug = args[0];
                        if (!debug.Equals(""))
                        {
                            Persistencia.VersionDebug = debug;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SFLoginForm fLogin = new SFLoginForm();

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                log.Debug("Assembly:" + assembly + "\n" + "_____________________________" + "\n File Version:" + fvi.FileVersion);

                Application.Run(fLogin);

                if (fLogin.Validado)
                {
                    if (fLogin.idiomaStr.Equals("en"))
                    {
                        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                        Lenguaje.InicializarLenguaje("en", new LenguajeFicheroConfiguracion());
                        log.Debug("Login superado|Idioma:Ingles");
                    }
                    else
                    {
                        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("es-ES");
                        Lenguaje.InicializarLenguaje("es", new LenguajeFicheroConfiguracion());
                        log.Debug("Login superado|Idioma:Español");
                    }
                    try
                    {
                        Persistencia.inicializarParametros();
                    }
                    catch (Exception ex)
                    {
                        log.Error("No se han podido inicializar los parametros de la aplicacion");
                    }
                    /*MONICA comentado hasta que se pueda probar*/
                    /*if (Persistencia.getParametroBoolean("COMPROBARVERSION")){
                        comprobarGuardarVersionWS();
                        comprobarMismaBD();
                    }*/
                    //Si hemos accedido con credenciales válidas, abrirá el formulario principal de la aplicación principal
                    Application.Run(new SFPrincipal());
                }
            }
            catch (OutOfMemoryException e)
            {
                ExceptionManager.GestionarError(e, "El ordenador se ha quedado sin memoria ram disponible y se va a cerrar");
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
            }
        }

        public static void comprobarGuardarVersionWS()
        {
            try
            {
                /*MONICA comentado hasta que se pueda probar*/
                /* String versionPC = Persistencia.VersionPC;
                 //WSVariables
                 WSVariablesClient wSVariables = new WSVariablesClient();
                 Persistencia.VersionWS = wSVariables.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception(" Revisar configuracion WSVariablesClient");
                 }
                 // WSArticulos
                 WSArticulosClient wSArticulos = new WSArticulosClient();
                 Persistencia.VersionWS = wSArticulos.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSArticulosClient");
                 }
                 //WSCargaMotor
                 WSCargaMotorClient wSCargaMotor = new WSCargaMotorClient();
                 Persistencia.VersionWS = wSCargaMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSCargaMotorClient");
                 }
                 //wSEmbalajeMotor
                 WSEmbalajeMotorClient wSEmbalajeMotor = new WSEmbalajeMotorClient();
                 Persistencia.VersionWS = wSEmbalajeMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSEmbalajeMotorClient");
                 }
                 //wSEntradaMotor
                 WSEntradaMotorClient wSEntradaMotor = new WSEntradaMotorClient();
                 Persistencia.VersionWS = wSEntradaMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSEntradaMotorClient");
                 }
                 //wSLotesMotor
                 WSLotesClient wSLotes = new WSLotesClient();
                 Persistencia.VersionWS = wSLotes.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSLotesClient");
                 }
                 //wSOperarioMotor
                 WSOperarioMotorClient wSOperarioMotor = new WSOperarioMotorClient();
                 Persistencia.VersionWS = wSOperarioMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSOperarioMotorClient");
                 }
                 //wSOrdenProduccionMotor
                 WSOrdenProduccionMotorClient wSOrdenProduccionMotor = new WSOrdenProduccionMotorClient();
                 Persistencia.VersionWS = wSOrdenProduccionMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSOrdenProduccionMotorClient");
                 }
                 //wSOrdenProduccionMotor
                 WSPackingListMotorClient wSPackingListMotor = new WSPackingListMotorClient();
                 Persistencia.VersionWS = wSPackingListMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSPackingListMotorClient");
                 }
                 //wSPedidoCliMotor
                 WSPedidoCliMotorClient wSPedidoCliMotor = new WSPedidoCliMotorClient();
                 Persistencia.VersionWS = wSPedidoCliMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSPedidoCliMotorClient");
                 }
                 //wSProduccionMotor
                 WSProduccionMotorClient wSProduccionMotor = new WSProduccionMotorClient();
                 Persistencia.VersionWS = wSProduccionMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSProduccionMotorClient");
                 }
                 //wSRecepcionMotor
                 WSRecepcionMotorClient wSRecepcionMotor = new WSRecepcionMotorClient();
                 Persistencia.VersionWS = wSRecepcionMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSRecepcionMotorClient");
                 }
                 //wSReservaMotor
                 WSReservaMotorClient wSReservaMotor = new WSReservaMotorClient();
                 Persistencia.VersionWS = wSReservaMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSReservaMotorClient");
                 }
                 //wSSalidaMotor
                 WSSalidaMotorClient wSSalidaMotor = new WSSalidaMotorClient();
                 Persistencia.VersionWS = wSSalidaMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSSalidaMotorClient");
                 }
                 //wSTareasPendientesMotor
                 WSTareasPendientesMotorClient wSTareasPendientesMotor = new WSTareasPendientesMotorClient();
                 Persistencia.VersionWS = wSTareasPendientesMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSTareasPendientesMotorClient");
                 }
                 //wSUbicarMotor
                 WSUbicarMotorClient wSUbicarMotor = new WSUbicarMotorClient();
                 Persistencia.VersionWS = wSUbicarMotor.getVersion();
                 if (!Persistencia.VersionDebug.Equals(""))
                 {
                     Persistencia.VersionWS = Persistencia.VersionDebug;
                 }
                 if (!Persistencia.VersionPC.Contains(Persistencia.VersionWS))
                 {
                     throw new Exception("Revisar configuracion WSUbicarMotorClient");
                 }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce(strings.ErrorInconsistencia), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionManager.GestionarError(ex);
            }
        }

        public static void comprobarMismaBD()
        {
            try
            {
                /*MONICA comentado hasta que se pueda probar*/
                /*  string conectionString = ConexionSQL.getConnectionString();
                  SqlConnectionStringBuilder cb = new System.Data.SqlClient.SqlConnectionStringBuilder(conectionString);
                  //wsVariables
                  WSVariablesClient wSVariables = new WSVariablesClient();
                  string[] datos = wSVariables.getDatosBD();
                  string database = datos[0];
                  string url = datos[1];
                  string serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSVariablesClient");
                  }
                  //wSArticulos
                  WSArticulosClient wSArticulos = new WSArticulosClient();
                  datos = wSArticulos.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSArticulosClient");
                  }
                  //wSCargaMotor
                  WSCargaMotorClient wSCargaMotor = new WSCargaMotorClient();
                  datos = wSCargaMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSCargaMotorClient");
                  }
                  //wSEmbalajeMotor
                  WSEmbalajeMotorClient wSEmbalajeMotor = new WSEmbalajeMotorClient();
                  datos = wSEmbalajeMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSEmbalajeMotorClient");
                  }
                  //wSEntradaMotor
                  WSEntradaMotorClient wSEntradaMotor = new WSEntradaMotorClient();
                  datos = wSEntradaMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSEntradaMotorClient");
                  }
                  //WSLotes
                  WSLotesClient WSLotes = new WSLotesClient();
                  datos = WSLotes.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSLotesClient");
                  }
                  //wSOperarioMotor
                  WSOperarioMotorClient wSOperarioMotor = new WSOperarioMotorClient();
                  datos = wSOperarioMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSOperarioMotorClient");
                  }
                  //wSOrdenProduccionMotor
                  WSOrdenProduccionMotorClient wSOrdenProduccionMotor = new WSOrdenProduccionMotorClient();
                  datos = wSOrdenProduccionMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSOrdenProduccionMotorClient");
                  }
                  //wSPackingListMotor
                  WSPackingListMotorClient wSPackingListMotor = new WSPackingListMotorClient();
                  datos = wSPackingListMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSPackingListMotorClient");
                  }
                  //wSPedidoCliMotor
                  WSPedidoCliMotorClient wSPedidoCliMotor = new WSPedidoCliMotorClient();
                  datos = wSPedidoCliMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSPedidoCliMotorClient");
                  }
                  //wSProduccionMotor
                  WSProduccionMotorClient wSProduccionMotor = new WSProduccionMotorClient();
                  datos = wSProduccionMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSProduccionMotorClient");
                  }
                  //wSRecepcionMotor
                  WSRecepcionMotorClient wSRecepcionMotor = new WSRecepcionMotorClient();
                  datos = wSRecepcionMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSRecepcionMotorClient");
                  }
                  //wSReservaMotor
                  WSReservaMotorClient wSReservaMotor = new WSReservaMotorClient();
                  datos = wSReservaMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSReservaMotorClient");
                  }
                  //wSSalidaMotor
                  WSSalidaMotorClient wSSalidaMotor = new WSSalidaMotorClient();
                  datos = wSSalidaMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSSalidaMotorClient");
                  }
                  //wSTareasPendientesMotor
                  WSTareasPendientesMotorClient wSTareasPendientesMotor = new WSTareasPendientesMotorClient();
                  datos = wSTareasPendientesMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSTareasPendientesMotorClient");
                  }
                  //wSUbicarMotor
                  WSUbicarMotorClient wSUbicarMotor = new WSUbicarMotorClient();
                  datos = wSUbicarMotor.getDatosBD();
                  database = datos[0];
                  url = datos[1];
                  serverName = extraerServerName(url);
                  if (!cb.DataSource.Contains(serverName) || !cb.InitialCatalog.Contains(database))
                  {
                      throw new Exception(" Revisar configuracion  DB WSUbicarMotorClient");
                  }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce(strings.ErrorInconsistenciaBD), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionManager.GestionarError(ex);
            }
        }

        public static string extraerServerName(string url)
        {
            string result = "";
            try
            {
                string prefijo = "jdbc:jtds:sqlserver://";
                int index = url.LastIndexOf(":");

                if (url.Contains("jdbc:jtds:sqlserver://"))
                {
                    result = url.Substring(prefijo.Length, index - prefijo.Length);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            return result;
        }
    }
}
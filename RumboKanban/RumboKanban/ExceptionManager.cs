using Rumbo.Core.Herramientas;
using System;
using System.Data.SqlClient;
using System.ServiceModel.Description;
//using System.ServiceModel.Description;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGAManager
{
   public static class ExceptionManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void GestionarError(Exception ex)
        {
            //Comentado por Pablo Boix 22/08/20, movido a ConexionSQL porque tenemos la query para mostrarla
            //MessageBox.Show(ex.Message + "\n" + ex.Source );
            // Debug.WriteLine(ex.StackTrace);
            /*log.Error("Mensaje:" + ex.Message + "\n StackTrace:"+ex.StackTrace);
            log.Error(ex);
            if(ex is SqlException)
            {
                if ((ex as SqlException).Number == 8156)
                {
                    String mensaje = ex.Message;
                    RadMessageBox.Show(mensaje);
                }
            }*/
            GestionarErrorNuevo(ex);
        }
        public static void GestionarError(Exception ex,string mensaje)
        {
            GestionarErrorNuevo(ex,mensaje);
            /*
            if (mensaje.Contains("java.lang.Exception:"))
            {
                mensaje = mensaje.Substring(20);
            }
            if (mensaje.Contains("java.sql.SQLException:"))
            {
                mensaje = mensaje.Substring(22);
            }
            if (mensaje.Contains("Data truncation"))
            {
                mensaje ="Longitud de los campos demasiado larga.Revise los campos introducidos ";
            }
            MessageBox.Show(Lenguaje.traduce(mensaje), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            Debug.WriteLine(ex.StackTrace);
            log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            */
        }
        public static void GestionarErrorWS(Exception ex, ServiceEndpoint endPoint)
        {
            //log.Error("Mensaje:" + mensaje + "\n StackTrace:" + endPoint);
            GestionarErrorNuevo(ex, "Error en webService mensaje:" + ex.Message + "\nEndPoint:" + endPoint.ToString());
            MessageBox.Show(ex.Message + "  " + endPoint);
        }

        public static void GestionarErrorNuevo(Exception ex)
        {
            GestionarErrorNuevo(ex, "");
        }
            //César nuevo metodo para gestionar errores, aún no implementado, desarrollando.
            //MensajeEspecifico: consultas SQL (por ejemplo)

        public static void GestionarErrorNuevo(Exception ex, String mensajeEspecifico)
        {
            StringBuilder sb = new StringBuilder();
            String mensajeAMostrar = "";

            sb.AppendLine("Se ha generado una excepción.");
            if (ex.InnerException != null)
            {
                sb.AppendLine("InnerExceptionMessage:");
                sb.AppendLine(ex.InnerException.Message);
            }
            sb.AppendLine("StackTrace:");
            sb.AppendLine(ex.StackTrace);

            if (ex is SqlException)
            {
                SqlException sqlEx = (ex as SqlException);
                sb.AppendLine("SQLException:");

                for (int i = 0; i < sqlEx.Errors.Count; i++)
                {
                    sb.AppendLine("Error N" + i);
                    sb.AppendLine(sqlEx.Errors[i].ToString());
                }
                if ((ex as SqlException).Number == 8156)
                {
                    String mensaje = ex.Message;
                    RadMessageBox.Show(mensaje);
                }
            }

            String errorJava = "";
            if (ex.Message.Contains("java.lang.Exception:"))
            {
                errorJava = ex.Message.Substring(20);
                mensajeAMostrar+= Lenguaje.traduce(errorJava);
            }
            if (ex.Message.Contains("java.sql.SQLException:"))
            {
                errorJava = ex.Message.Substring(22);
                mensajeAMostrar += Lenguaje.traduce(errorJava);
            }
            if (ex.Message.Contains("Data truncation"))
            {
                errorJava = "Longitud de los campos demasiado larga.Revise los campos introducidos";
                mensajeAMostrar += Lenguaje.traduce("Longitud de los campos demasiado larga.Revise los campos introducidos");
            }

            if (!String.IsNullOrEmpty(errorJava))
            {
                sb.AppendLine("Error Java:");
                sb.AppendLine(errorJava);
            }

            if (!String.IsNullOrEmpty(mensajeEspecifico) && String.IsNullOrEmpty(mensajeAMostrar))
            {
                sb.AppendLine("Mensaje especifico:");
                sb.AppendLine(mensajeEspecifico);
                mensajeAMostrar = mensajeEspecifico;
            }
            log.Error(sb);

            if (!mensajeAMostrar.Equals(""))
            {
                RadMessageBox.Show(mensajeAMostrar);
            }

        }
    }
}

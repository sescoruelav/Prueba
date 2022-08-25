using Rumbo.Core.Herramientas;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class RumLog
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int idLogoOperaciones;
        private DateTime fecha;
        private string usuario;
        private string ipEstacion;
        private string programa;
        private string operacion;
        private int idInicio1;
        private int idInicio2;
        private int idDestino1;
        private int idDestino2;
        public string parametros;
        private string observaciones;
        private string observacionesUsuario;
        public string resultados;
        private DateTime horaFin;

        public RumLog(DateTime fecha, string usuario, string ipEstacion, string programa, 
            string operacion, int idInicio1, int idInicio2, int idDestino1, int idDestino2, string parametros,
            string observaciones, string observacionesUsuario, string resultados)
        {
            this.fecha = fecha;
            this.usuario = usuario;
            this.ipEstacion = ipEstacion;
            this.programa = programa;
            this.operacion = operacion;
            this.idInicio1 = idInicio1;
            this.idInicio2 = idInicio2;
            this.idDestino1 = idDestino1;
            this.idDestino2 = idDestino2;
            this.parametros = parametros;
            this.observaciones = observaciones;
            this.observacionesUsuario = observacionesUsuario;
            this.resultados = resultados;
        }


        public static RumLog getRumLog(String programa, String operacion)
        {
            try
            {
                return getRumLog(Lenguaje.traduce(programa), Lenguaje.traduce(operacion), 0, 0, 0, 0, "", "", "", "");
            }catch(Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex,"Error al hacer getRumLog. String programa:"+
                    programa+". String operación:"+operacion);
            }
            return null;
        }
        public static RumLog getRumLog(String programa,String operacion,int idInicio1,int idInicio2,int idDestino1,
            int idDestino2, string parametros,string observaciones,string observacionesUsuarios,string resultados)
        {
            try
            {
                RumLog rm = new RumLog(DateTime.Now, User.NombreUsuario, Dns.GetHostName(), programa, operacion, idInicio1, idInicio2, idDestino1, idDestino2, parametros,
                    observaciones, observacionesUsuarios, resultados);
                return rm;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al hacer getRumLog. String programa:" +
                    programa + ". String operación:" + operacion);
            }
            return null;
        }

        public static string EscribirRumLog(RumLog rl)
        {
            if (rl != null)
            {
                return rl.escribirRumLog();
            }
            return "";
        }

        private string escribirRumLog()
        {
            try
            {
                this.horaFin = DateTime.Now;
                String sql = "INSERT INTO[dbo].[RUMLOG] ([FECHA],[USUARIO],[IPESTACION],[PROGRAMA],[OPERACION]," +
                    "[IDINICIO1],[IDINICIO2],[IDDESTINO1],[IDDESTINO2],[PARAMETROS],[OBSERVACIONES],[OBSERVACIONESUSUARIO]" +
                    ",[RESULTADOS],[HORAFIN]) VALUES ('" + this.fecha.ToString("s") + "','" + this.usuario + "','" + this.ipEstacion + "'," +
                    "'" + this.programa + "','" + operacion + "',";
                if (this.idInicio1 == 0)
                {
                    sql += "NULL,";
                }
                else
                {
                    sql += idInicio1 + ",";
                }

                if (this.idInicio2 == 0)
                {
                    sql += "NULL,";
                }
                else
                {
                    sql += idInicio2 + ",";
                }

                if (this.idDestino1 == 0)
                {
                    sql += "NULL,";
                }
                else
                {
                    sql += idDestino1 + ",";
                }

                if (this.idDestino2 == 0)
                {
                    sql += "NULL,";
                }
                else
                {
                    sql += idDestino2 + ",";
                }

                if (parametros.Length > 500)
                {
                    parametros = parametros.Substring(0, 499);
                }
                sql += "'" + parametros.Replace("'", "''") + "',";


                if (observaciones.Length > 200)
                {
                    observaciones = observaciones.Substring(0, 199);
                }
                if (observaciones.Equals(""))
                {
                    sql += "NULL,";
                }
                else
                {
                    sql += "'" +observaciones + "',";
                }

                if (observacionesUsuario.Length > 200)
                {
                    observacionesUsuario = observacionesUsuario.Substring(0, 199);
                }
                if (observacionesUsuario.Equals(""))
                {
                    sql += "NULL,";
                }
                else
                {
                    sql += "'" + observacionesUsuario + "',";
                }
                sql += "'" + resultados.Replace("'","''") + "','" + horaFin.ToString("s") + "')";

                String error = "";
                ConexionSQL.SQLClienteExec(sql, ref error);

                return error;
            }catch(Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return "";
        }
    }
}

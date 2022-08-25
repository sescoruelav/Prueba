using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using BCrypt;
using RumboSGAManager.Model.DataContext;
using Rumbo.Core.Herramientas;

namespace RumboSGAManager.Model.Security
{
   public static class Security
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Boolean validarLogin(string user, string passwd)
        {
            Boolean valido = false;
            Boolean basedeDatosConectada = false;
            string connectionString = ConexionSQL.getConnectionString();
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    
                    SqlCommand command = new SqlCommand("select IDUSUARIO,NOMUSUARIO,CLAVEUSUARIO,RUTAIMP from RUMUSUARIOS WHERE NOMUSUARIO='" + user + "'", conexion);
                    conexion.Open();
                    basedeDatosConectada = true;
                    log.Debug("Conexion abierta por el usuario:"+user);
                    SqlDataReader reader = command.ExecuteReader();
                    log.Debug("Buscando usuario en la base de datos:" + user);

                    while (reader.Read())
                    {
                        if (reader["IDUSUARIO"] is DBNull)
                        {
                            log.Error("El IDUSUARIO es DBNull en la bbdd:"+connectionString);
                            return false;
                        }
                        int idUser = (int)reader["IDUSUARIO"];

                        if (reader["NOMUSUARIO"] is DBNull)
                        {
                            log.Error("El NOMUSUARIO es DBNull en la bbdd:" + connectionString);
                            return false;
                        }
                        string bdUser = (string)reader["NOMUSUARIO"];

                        if (reader["CLAVEUSUARIO"] is DBNull)
                        {
                            log.Error("El CLAVEUSUARIO es DBNull en la bbdd:" + connectionString);
                            return false;
                        }
                        string bdPasswd = (string)reader["CLAVEUSUARIO"];

                        string impresora = "";

                        if (!(reader["RUTAIMP"] is DBNull))
                            impresora = (string)reader["RUTAIMP"];


                        if (bdUser.Equals(user,StringComparison.InvariantCultureIgnoreCase) && bdPasswd.Equals(passwd,StringComparison.InvariantCultureIgnoreCase))
                        {
                            log.Debug("Usuario encontrado");
                            log.Debug("Buscando operario");


                            User.IdUsuario = idUser;
                            User.NombreImpresora = impresora;
                            User.NombreUsuario = bdUser;
                            User.PwdUsuario = bdPasswd;

                            string idOp = ConexionSQL.getIdOperario(idUser);
                            log.Debug("Operario encontrado,IDOPERARIO:" + idOp);
                            if (idOp == null || idOp == string.Empty)
                            {
                                valido = false;
                            }
                            else
                            {
                                User.IdOperario = int.Parse(idOp);
                                valido = true;
                                log.Debug("Buscando grupo");
                                User.BuscarGrupo();
                                log.Debug("Grupo encontrado,buscando permisos");
                                User.IniciarPermisos();
                                log.Debug("Permisos encontrados");
                                break;
                            }
                        }
                        else
                        {
                            valido = false;
                        }
                    }
                }
                catch (SqlException e)
                {
                    if (!basedeDatosConectada)
                    {
                        System.Windows.Forms.MessageBox.Show(Lenguaje.traduce("No se ha podido conectar a la base de datos"));
                    }
                    ExceptionManager.GestionarError(e);
                    log.Error("Mensaje:"+e.Message+"|StackTrace:"+e.StackTrace);
                }
                finally
                {
                    conexion.Close();
                    conexion.Dispose();
                }
            }
            return valido;
        }
    }
}

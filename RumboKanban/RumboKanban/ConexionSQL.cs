using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

//NOTA: Estas clases deberían ser modificadas, en un futuro, por conexiones a WS
namespace RumboSGAManager.Model.DataContext
{
    //CLASE DE CONEXION DIRECTA CON SQL SERVER
    public static class ConexionSQL
    {
        #region Variables y Propiedades
        private static String connectionString = string.Empty;
        private static String nombreBD=string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SqlConnection conexionActual;

        #endregion

        #region Funciones CRUD Genéricas

        //R (read) - Obtención de datos mediante conexión SQL y serializado a JSON
        public static string SQLClientLoadJSON(string sqlCommand)
        {
            string json = string.Empty;
            DataTable dttDatos = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandTimeout = 60;
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dttDatos);
                    json = JsonConvert.SerializeObject(dttDatos, Formatting.Indented);
                }
                catch (SqlException e)
                {
                    //MessageBox.Show("");
                    log.Error("ERROR ejecutando :" + sqlCommand);
                    ExceptionManager.GestionarError(e);

                    //Columna especificada dos veces error.
                    
                    //throw;
                }
                finally
                {
                    connection.Close();
                }

                return json;
            }
        }
        //R (read) - Obtención de datos mediante conexión SQL y serializado a JSON
        public static DataTable SQLClientLoad(string sqlCommand)
        {
           
            DataTable dttDatos = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dttDatos);
                    
                }
                catch (SqlException e)
                {
                    //MessageBox.Show("");
                    ExceptionManager.GestionarError(e);
                    //throw;
                }
                finally
                {
                    connection.Close();
                }

                return dttDatos;
            }
        }

        //C (create) U (update) D (delete) -
        public static bool SQLClienteExec(string sqlCommand, ref string error)
        {
            bool bolOk = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                    bolOk = true;
                }

                catch (SqlException ex)
                {
                    ExceptionManager.GestionarErrorNuevo(ex, sqlCommand);
                    error = ex.Message;
                    //log.Error("Error:" + ex.Message + "  \n Stack:" + ex.StackTrace);
                    //Clave duplicada
                    if (ex.Number == 2627)
                    {
                        error = Lenguaje.traduce("No se puede insertar una clave duplicada");

                    }
                    if (ex.Number == 547)
                    {
                        error = Lenguaje.traduce("No se puede eliminar el registro porque hay otros que dependen de él");
                    }
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
                finally
                {
                    connection.Close();
                }
                return bolOk;
            }
        }

        //C (create) U (update) D (delete) -
        public static bool SQLClienteExecScopeIdentity(string sqlCommand, ref string error, ref object scopeIdentity)
        {
            bool bolOk = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sqlCommand + ";SELECT SCOPE_IDENTITY()";
                    command.CommandType = CommandType.Text;
                    //command.ExecuteNonQuery();
                    scopeIdentity = command.ExecuteScalar();

                    bolOk = true;
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
                finally
                {
                    connection.Close();
                }
                return bolOk;
            }
        }
        public static DataTable getDataTable(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection())
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 500;
                con.ConnectionString = connectionString;
                try
                {
                    con.Open();
                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            a.Fill(dt);
                        }
                        catch (SqlException e)
                        {
                            log.Error("Mensaje:" + e.Message + "  ||StackTrace:" + e.StackTrace);
                            //añadido Pablo Boix 22/08/20
                            MessageBox.Show("Se ha producido un error de base de datos:" + "\n" + e.Message + " \n en:\n"+query);
                            throw e;
                            
                        }
                    }
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarErrorNuevo(e,query);
                    dt = null;
                }
                finally
                {
                    con.Close();
                }
                return dt;
            }

        }
        public static SqlCommand getCommandParameterString(string query,string nombreParametro)
        { 
            try
            {
                conexionActual = new SqlConnection();
                conexionActual.ConnectionString = connectionString;
                conexionActual.Open();
                SqlCommand cmdParametros = new SqlCommand(query, conexionActual);
                cmdParametros.Parameters.Add(nombreParametro, SqlDbType.VarChar);
                //cmdParametros.Prepare();
                return cmdParametros;
            }
            catch (SqlException e)
            {
                conexionActual.Close();//La conexion en el caso de parametros se cierra despues o si hay error.
                ExceptionManager.GestionarError(e);
            }
            return null;
        }
        public static SqlDataReader getDataReaderParameter(SqlCommand sqlPrecompilada,string nombreParametro , string  valorParametro )
        {
            SqlDataReader dr = null;
            try
            {
                //sqlPrecompilada.Parameters.AddWithValue(nombreParametro, valorParametro);
                sqlPrecompilada.Parameters[nombreParametro].Value = valorParametro;
                dr = sqlPrecompilada.ExecuteReader();
                return dr;
            }
            catch(SqlException e)
            {
                conexionActual.Close();//La conexion en el caso de parametros se cierra despues o si hay error.
                ExceptionManager.GestionarError(e);
            }
            return null;
        }
        public static SqlDataReader getDataReaderParameter(SqlCommand sqlPrecompilada, string nombreParametro, int valorParametro)
        {
            SqlDataReader dr = null;
            try
            {
                //sqlPrecompilada.Parameters.AddWithValue(nombreParametro, valorParametro);
                sqlPrecompilada.Parameters[nombreParametro].Value = valorParametro;
                dr = sqlPrecompilada.ExecuteReader();
                return dr;
            }
            catch (SqlException e)
            {
                conexionActual.Close();//La conexion en el caso de parametros se cierra despues o si hay error.
                ExceptionManager.GestionarError(e);
            }
            return null;
        }
        public static void closeCommandParameter()
        {
            try
            {
                conexionActual.Close();
            }
            catch (SqlException e)
            {
                ExceptionManager.GestionarError(e);
            }

        }
        /*
        private static void MessageBox(string v)
        {
            throw new NotImplementedException();
        }
        */
        public static int getRegistrosFiltrados(string query)
        {
            int registrosFiltrados = 0;
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd.Connection = connection;
                connection.Open();
                try
                {
                    cmd.CommandText = query;
                    registrosFiltrados = (int)cmd.ExecuteScalar();

                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);

                }
                finally
                {
                    connection.Close();
                }
            }
            return registrosFiltrados;
        }
        public static bool DataTablesEquals(DataTable dt1, DataTable dt2)
        {
            try
            {
                if (dt1 == null || dt2 == null) return false;
                dt1.Merge(dt2);
                DataTable d3 = dt2.GetChanges();
                if (d3 is null)
                    return true;
                else
                    return d3.Rows.Count == 0;
            }
            catch(Exception e)
            {
                return false;// no vale la pena devolver otra cosa
            }
           
        }
        public static string getIdOperario(int idUsuario)//TODO esto no va aqui 29/ago/20 pablo
        {
            string idOperario=null;
            string query = "SELECT IDOPERARIO FROM TBLUSUARIOOPERARIO WHERE IDUSUARIO=" + idUsuario;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con=new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    //Se castea a string para que pueda ser nulo
                    idOperario = cmd.ExecuteScalar().ToString();
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:"+e.Message+" ||Stack:"+e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace+"||IdUsuario:"+idUsuario);
                    if (CultureInfo.CurrentUICulture.Name == "es-ES")
                    {
                        MessageBox.Show("No se ha encontrado ningun operario asignado a este usuario");

                    }
                    else
                    {
                        MessageBox.Show("Unable to find an operator that matches this user");
                    }
                }
                finally
                {
                    con.Close();
                }
            }
            return idOperario;
        }

        public static string getNombreImpresora()//TODO esto no va aqui 29/ago/20 pablo
        {
            string nombreImpresora;
            using (SqlConnection con=new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //cmd.CommandText ="SELECT RUTAIMP FROM RUMUSUARIOS WHERE IDUSUARIO="+ User.IdUsuario;
                //TODO Mover esta funcion 7/ene/22
                nombreImpresora = (string)cmd.ExecuteScalar();

            }
            return nombreImpresora;
        }
        public static void setConnectionString(string conString)
        {
           connectionString=conString;
        }
        public static string getConnectionString()
        {
            return connectionString;
        }
        public static void setNombreConexion(string conName)
        {
            nombreBD = conName;
        }
        public static string getNombreConexion()
        {
            return nombreBD;
        }
        #endregion
    }
}
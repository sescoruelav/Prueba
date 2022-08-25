using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

//NOTA: Estas clases deberían ser modificadas, en un futuro, por conexiones a WS
namespace RumboSGAManager.Model.DataContext
{
    //CLASE PARA DE OBTENCION DE DATOS MEDIANTE CONEXION DIRECTA SQL
    public static class DataAccess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Funciones Públicas

        #region Variables

        private static String connectionString = string.Empty;

        #endregion Variables

        #region Alta Producto

        public static DataTable GetAltaProductoDefectoOrden(string orden)
        {
            return ConexionSQL.getDataTable(GetAltaProductoDefectoSQL + " WHERE OFC.ORDEN = '" + orden + "'");
        }

        public static DataTable GetAltaArticuloDefecto(int idArticulo)
        {
            return ConexionSQL.getDataTable(GetAltaArticuloDefectoSQL + " WHERE IDARTICULO = " + idArticulo);
        }

        public static DataTable GetAltaArticuloDefectoPedidoPro(int idPedidoPro, int idPedidoProLin)
        {
            return ConexionSQL.getDataTable(GetAltaProductoPedidoProDefectoSQL + " WHERE PL.IDPEDIDOPRO = " + idPedidoPro + " AND PL.IDPEDIDOPROLIN = " + idPedidoProLin);
        }

        public static DataTable GetAltaArticuloDefectoDevolCli(int idDevolCli, int idDevolCliLin)
        {
            return ConexionSQL.getDataTable(GetAltaProductoDevolCliDefectoSQL + " WHERE PL.IDDEVOLCLI = " + idDevolCli + " AND PL.IDDEVOLCLILIN = " + idDevolCliLin);
        }

        public static DataTable GetIdDescripcionUnidadesTipo()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionUnidadesTipoSQL);
        }

        public static DataTable GetIdMotivosComentario(string tipoPadre)
        {
            return ConexionSQL.getDataTable(GetIdDescripcionMotivosComentarioSQL + " OR TIPOPADRE='" + tipoPadre + "'");
        }

        public static DataTable GetIdDescripcionUnidadesTipoPresentacion(int idPresentacion)
        {
            return ConexionSQL.getDataTable(GetIdDescripcionUnidadesTipoPresentacionSQL + " WHERE PL.IDPRESENTACION =   " + idPresentacion);
        }

        public static DataTable GetIdDescripcionTipoPalets()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionTipoPaletSQL);
        }

        public static DataTable GetIdDescripcionMaquinas()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionMaquinasSQL);
        }

        public static DataTable GetIdDescripcionOperarios()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionOperariosSQL);
        }

        public static DataTable GetIdDescripcionOperariosOrder(string order)
        {
            return ConexionSQL.getDataTable(GetIdDescripcionOperariosSQL + order);
        }

        public static DataTable GetIdDescripcionMotivosTipo(string tipo)
        {
            return ConexionSQL.getDataTable(GetIdDescripcionMotivosTipoSQL + " WHERE TIPOMOT LIKE '%" + tipo + "%'");
        }

        public static DataTable GetIdDescripcionExistenciaEstados()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionFabricacionEstadosSQL);
        }

        public static DataTable GetIdDescripcionCarrosMovil()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionCarrosMovilSQL);
        }

        public static DataTable GetIdReferenciaDescripcionAtributoGenericosArticulos()
        {
            return ConexionSQL.getDataTable(GetIdReferenciaDescripcionAtributoArticulosGenericosSQL);
        }

        public static DataTable GetIdReferenciaDescripcionAtributoArticulos()
        {
            return ConexionSQL.getDataTable(GetIdReferenciaDescripcionAtributoArticulosSQL);
        }

        public static DataTable GetIdDescripcionHueco()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionHuecoSQL);
        }

        public static DataTable GetIdDescripcionHuecoMuelle()
        {
            return ConexionSQL.getDataTable(GetIdDescripcionHuecoMuelleSQL);
        }

        public static DataTable GetIdDescripcionHuecoTipoHueco(String tipoHueco)
        {
            return ConexionSQL.getDataTable(GetIdDescripcionHuecoSQL);
        }

        public static int GetMuelleRecepcion(int idRecepcion)
        {
            return int.Parse(ConexionSQL.getDataTable("SELECT TOP 1 MUELLE FROM TBLRECEPCIONESCAB WHERE IDRECEPCION = " + idRecepcion).Rows[0][0].ToString());
        }

        public static int GetMuelleMaquina(int idMaquina)
        {
            return int.Parse(ConexionSQL.getDataTable("SELECT TOP 1 MUELLE FROM TBLMAQUINAS WHERE IDMAQUINA = " + idMaquina).Rows[0][0].ToString());
        }

        #endregion Alta Producto

        #region OperariosMov

        //Tamaño OperariosMov. Devuelve el esquema de datos y la cantidad de registros totales de la tabla OperariosMov
        public static string ObtencionOperariosMovEsquema()
        {
            string json = LoadJson("OperariosMovimientos");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema OperariosMovimientos|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionOperariosMovRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("OperariosMovimientos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos OperariosMovimientos. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ExistenciasHistorico
        public static string ObtencionOperariosMovDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("OperariosMovimientos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " OPERMOV " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string registrosFiltrados = query + selectExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionOperariosMovDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("OperariosMovimientos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " OPERMOV" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una OperariosMovimientos
        public static string AltaOperariosMov(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OperariosMovimientos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una OperariosMovimientos
        public static string EditarOperariosMov(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OperariosMovimientos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una OperariosMovimientos
        public static string EliminarOperariosMov(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OperariosMovimientos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion OperariosMov

        #region Proveedores

        //Tamaño Proveedores. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string ObtencionProveedoresEsquema()
        {
            string json = LoadJson("Proveedores");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "PRO";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionProveedoresRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Proveedores");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores
        public static string ObtencionProveedoresDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Proveedores");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            //intermedia.Query = query + " ORDER BY " + sortExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionProveedoresDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Proveedores");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Proveedor
        public static string AltaProveedor(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            //List<Proveedor> provList = JsonConvert.DeserializeObject<List<Proveedor>>(jo.First()["Data"].ToString());
            //Proveedor prov = provList.First();
            Hashtable hshDatos = new Hashtable();

            /*foreach (PropertyInfo property in typeof(Proveedor).GetProperties())
            {
                if (!prov.hshPK.ContainsValue(property.Name))
                {
                    hshDatos.Add(property.Name, property.GetValue(prov));
                }
            }*/
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Proveedores");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string EditarProveedor(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Proveedores");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string EliminarProveedor(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Proveedores");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Proveedores

        #region Clientes

        //Tamaño Clientes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string ObtencionClientesEsquema()
        {
            string json = LoadJson("Clientes");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "CLI";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionClientesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Clientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Clientes. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores
        public static string ObtencionClientesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Clientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLI" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            //intermedia.Query = query + " ORDER BY " + sortExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionClientesDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Clientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLI" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Cliente
        public static string AltaCliente(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Clientes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Cliente
        public static string EditarCliente(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Clientes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Cliente
        public static string EliminarCliente(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Clientes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Clientes

        #region Carga

        public static string ActualizarSelloCarga(int idPedidoCli, String sello)
        {
            String resp = "";
            ConexionSQL.SQLClienteExec("UPDATE TBLPEDIDOSCLICAB SET SELLO = '" + sello + "' WHERE IDPEDIDOCLI = " + idPedidoCli, ref resp);

            return resp;
        }

        public static string ActualizarOrdenReparto(int idcarga, int identificador, int orden)
        {
            String resp = "";
            ConexionSQL.SQLClienteExec("UPDATE TBLCARGALIN SET ORDEN = " + orden + " WHERE IDENTIFICADOR = " + identificador + " AND IDCARGA=" + idcarga, ref resp);

            return resp;
        }

        public static string InsertarComentario(string comentario, string tipoPadre, int idPadre, string pathImagen, string nombreImagen, int idOperario, string idmotivocomentario)
        {
            string resp = "";
            object identity = -1;
            ConexionSQL.SQLClienteExecScopeIdentity("INSERT INTO dbo.TBLCOMENTARIOS (COMENTARIO, PATHIMAGEN, NOMBREIMAGEN,IDOPERARIO, IDMOTIVOCOMENTARIO) "
                + " VALUES('" + comentario + "','" + pathImagen + "', '" + nombreImagen + "'," + idOperario + ",'" + idmotivocomentario + "')", ref resp, ref identity);
            if (resp.Equals(""))
            {
                ConexionSQL.SQLClienteExec("INSERT INTO dbo.TBLRELACIONCOMENTARIOS(IDPADRE, TIPOPADRE, IDCOMENTARIO)"
                + "VALUES(" + idPadre + ", '" + tipoPadre + "'," + identity + ")", ref resp);
            }

            return resp;
        }

        public static DataTable getComentarios(int id, string tipoPadre)
        {
            DataTable table = GetSeleccion("SELECT c.IDCOMENTARIO AS ID ,c.COMENTARIO AS Comentario," +
                " c.PATHIMAGEN AS [Ruta Imagen], c.NOMBREIMAGEN AS [Nombre Imagen] ,c.FECHAHORA AS Fecha," +
                "O.nombre + ' ' +  o.APELLIDOS AS Operario,M.DESCRIPCION as Motivo " +
                "FROM dbo.TBLRELACIONCOMENTARIOS rc " +
                "INNER JOIN dbo.TBLCOMENTARIOS c ON c.IDCOMENTARIO = rc.IDCOMENTARIO " +
                " LEFT JOIN TBLMOTIVOSCOMENTARIO m ON m.IDMOTIVOCOMENTARIO=c.IDMOTIVOCOMENTARIO AND RC.TIPOPADRE=M.TIPOPADRE" +
                " LEFT JOIN TBLOPERARIOS O ON O.IDOPERARIO=C.IDOPERARIO " +
                "WHERE rc.TIPOPADRE = '" + tipoPadre + "' AND rc.IDPADRE =" + id);

            return table;
        }

        //Tamaño CargaCab. Devuelve el esquema de datos y la cantidad de registros totales de la tabla TBLCARGACAB
        public static string ObtencionCargaCabEsquema()
        {
            string json = LoadJson("OrdenCargaCab");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "CARGACAB";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonWhere = js.First()["WHERE"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string where = "";
                if (!string.IsNullOrEmpty(jsonWhere))
                {
                    where = " WHERE " + jsonWhere;
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones + where;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionCargaCabRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("OrdenCargaCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Carga Cab. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla TBLCARGACAB
        public static string ObtencionCargaCabDatos(string sortExpression, string filterExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("OrdenCargaCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CARGACAB" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query;
            //intermedia.Query = query + " ORDER BY " + sortExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionCargaCabDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("OrdenCargaCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            string where = "";
            if (!string.IsNullOrEmpty(jsonWhere))
            {
                where = " WHERE " + jsonWhere;
            }
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CARGACAB " + strRelaciones + " " + where + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Cliente
        public static string AltaCargaCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OrdenCargaCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Cliente
        public static string EditarCargaCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OrdenCargaCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Cliente
        public static string EliminarCargaCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OrdenCargaCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Carga

        #region Clientes Pedidos Cab

        //Tamaño ClientesPedidosCab. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ClientesPedidosCab
        public static string ObtencionClientesPedidosCabEsquema()
        {
            string json = LoadJson("ClientesPedidosCab");

            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "CLICAB";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " CLICAB " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionClientesPedidosCabRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;

            //Esto no funciona
            //int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return -1;
        }

        //Datos ClientesPedidosCab. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ClientesPedidosCab
        public static string ObtencionClientesPedidosCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLICAB" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionClientesPedidosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLICAB" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ClientesPedidosCab
        public static string AltaClientePedidoCab(string json, bool altaLineas)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            object scopeIdentity = null;
            bool ok = ConexionSQL.SQLClienteExecScopeIdentity(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error, ref scopeIdentity);

            if (altaLineas && ok && scopeIdentity != null)
            {
                //Crear líneas de pedido
                string jsonGridEsquema = jo.First()["DataGridLin"].ToString();
                if (!string.IsNullOrEmpty(jsonGridEsquema))
                {
                    //GridScheme esquemaGrid = JsonConvert.DeserializeObject<GridScheme>(jsonGridEsquema);
                    GridScheme _esquemaGrid = JsonConvert.DeserializeObject<List<GridScheme>>(jsonGridEsquema)[0];

                    //string a = "";
                    string jsonLineas = LoadJson(_esquemaGrid.ArchivoJSON);
                    JArray joLineas = JArray.Parse(jsonLineas);
                    List<TableScheme> lstEsquemasLineas = JsonConvert.DeserializeObject<List<TableScheme>>(joLineas.First()["Scheme"].ToString());

                    Hashtable hshColumnasLineas = new Hashtable();
                    Hashtable hshClausulaLineas = new Hashtable();

                    for (int i = 0; i < lstEsquemasLineas.Count; i++)
                    {
                        if (lstEsquemasLineas[i].Nombre == _esquemaGrid.Filtro)
                        {
                            hshColumnasLineas.Add(lstEsquemasLineas[i].Nombre, scopeIdentity);
                        }
                        else
                        {
                            hshColumnasLineas.Add(lstEsquemasLineas[i].Nombre, "[CAMPO_TABLA]");
                        }
                    }

                    if (!string.IsNullOrEmpty(_esquemaGrid.Filtro))
                    {
                        hshClausulaLineas.Add(_esquemaGrid.Filtro, _esquemaGrid.ValorFiltro);
                    }
                    ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT_SELECT", jsonFrom, hshColumnasLineas, hshClausulaLineas), ref error);
                }
            }

            return ComponerACK(ok, error);
        }

        //Editar un ClientesPedidosCab
        public static string EditarClientePedidoCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un ClientesPedidosCab
        public static string EliminarClientePedidoCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Clientes Pedidos Cab

        #region Clientes Pedidos Lineas

        //Tamaño ClientePedidoLineas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ClientePedidoLineas
        public static string ObtencionClientesPedidosLineasEsquema(string _filtro)
        {
            string json = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + _filtro;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Datos ClientePedidoLineas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ClientePedidoLineas
        public static string ObtencionClientesPedidosLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLILIN" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionClientesPedidosLineasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones, string filtroIdPedidoCli)
        {
            string json = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLILIN" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte " + filtroIdPedidoCli;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ClientePedidoLineas
        public static string AltaClientePedidoLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    //if (!tesquema.EsPK)
                    //{
                    if (string.IsNullOrEmpty(_valor))
                    {
                        _valor = "NULL";
                    }
                    hshDatos.Add(_nombre, _valor);
                    //}
                }
            }
            string jsEntero = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un ClientePedidoLineas
        public static string EditarClientePedidoLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar ClientePedidoLineas
        public static string EliminarClientePedidoLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Clientes Pedidos Lineas

        #region Proveedores Pedidos Cab

        //Tamaño Proveedores. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string ObtencionProveedoresPedidosCabEsquema()
        {
            string json = LoadJson("ProveedoresPedidosCab");

            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionProveedoresPedidosCabRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores
        public static string ObtencionProveedoresPedidosCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROCAB" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            SentenciasSQL.queryPedidosProCab = query + " ORDER BY " + sortExpression;
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Proveedor
        public static string AltaProveedorPedidoCab(string json, bool altaLineas)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            object scopeIdentity = null;
            bool ok = ConexionSQL.SQLClienteExecScopeIdentity(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error, ref scopeIdentity);

            if (altaLineas && ok && scopeIdentity != null)
            {
                //Crear líneas de pedido
                string jsonGridEsquema = jo.First()["DataGridLin"].ToString();
                if (!string.IsNullOrEmpty(jsonGridEsquema))
                {
                    //GridScheme esquemaGrid = JsonConvert.DeserializeObject<GridScheme>(jsonGridEsquema);
                    GridScheme _esquemaGrid = JsonConvert.DeserializeObject<List<GridScheme>>(jsonGridEsquema)[0];

                    //string a = "";
                    string jsonLineas = LoadJson(_esquemaGrid.ArchivoJSON);
                    JArray joLineas = JArray.Parse(jsonLineas);
                    List<TableScheme> lstEsquemasLineas = JsonConvert.DeserializeObject<List<TableScheme>>(joLineas.First()["Scheme"].ToString());

                    Hashtable hshColumnasLineas = new Hashtable();
                    Hashtable hshClausulaLineas = new Hashtable();

                    for (int i = 0; i < lstEsquemasLineas.Count; i++)
                    {
                        if (lstEsquemasLineas[i].Nombre == _esquemaGrid.Filtro)
                        {
                            hshColumnasLineas.Add(lstEsquemasLineas[i].Nombre, scopeIdentity);
                        }
                        else
                        {
                            hshColumnasLineas.Add(lstEsquemasLineas[i].Nombre, "[CAMPO_TABLA]");
                        }
                    }

                    if (!string.IsNullOrEmpty(_esquemaGrid.Filtro))
                    {
                        hshClausulaLineas.Add(_esquemaGrid.Filtro, _esquemaGrid.ValorFiltro);
                    }
                    ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT_SELECT", jsonFrom, hshColumnasLineas, hshClausulaLineas), ref error);
                }
            }

            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string EditarProveedorPedidoCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string EliminarProveedorPedidoCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Proveedores Pedidos Cab

        #region Proveedores Pedidos Lineas

        //Tamaño Proveedores. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string ObtencionProveedoresPedidosLineasEsquema(string _filtro)
        {
            string json = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + _filtro;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores
        public static string ObtencionProveedoresPedidosLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string fields = strCampos;
            string selectExpression = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROLIN" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionProveedoresPedidosLineasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROLIN" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Proveedor
        public static string AltaProveedorPedidoLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    //if (!tesquema.EsPK)
                    //{
                    if (string.IsNullOrEmpty(_valor))
                    {
                        _valor = "NULL";
                    }
                    hshDatos.Add(_nombre, _valor);
                    //}
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string EditarProveedorPedidoLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string EliminarProveedorPedidoLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Proveedores Pedidos Lineas

        #region Articulos

        //Tamaño Familia. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Familias
        public static string ObtencionArticulosEsquema()
        {
            string json = LoadJson("Articulos");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionArticulosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Articulos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Familias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Familias
        public static string ObtencionArticulosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Articulos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ART" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionArticulosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Articulos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ART" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static DataTable GetArticulosCombo()
        {
            return ConexionSQL.getDataTable(GetArticulosData);
        }

        //Crear una Familia
        public static string AltaArticulo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Articulos");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Familia
        public static string EditarArticulo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Articulos");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            string sqlComand = GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula);
            if (String.IsNullOrEmpty(sqlComand))
            {
                return ComponerACK(false, error);
            }
            bool ok = ConexionSQL.SQLClienteExec(sqlComand, ref error);
            return ComponerACK(ok, error);
        }

        //César : Metodo copiado rapido por necesidad.
        public static string EditarArticulo(string json, string where)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Articulos");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            if (hshDatos.Count == 0) return "";
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, where, hshClausula), ref error);

            return ComponerACK(ok, error);
        }

        //Eliminar una Familia
        public static string EliminarArticulo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Articulos");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Articulos

        #region ReservaHistorico

        //Tamaño ReservaHistorico. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ReservaHistorico
        public static string ObtencionReservasHistoricoEsquema()
        {
            string json = LoadJson("ReservaHistorico");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema||  " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionReservasHistoricoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos ReservaHistorico. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ReservaHistorico
        public static string ObtencionReservasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RESHIST" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionReservasHistoricoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RESHIST" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ReservaHistorico
        public static string AltaReservasHistorico(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string EditarReservasHistorico(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string EliminarReservasHistorico(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion ReservaHistorico

        #region Maquina

        //Tamaño Maquina. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Maquinas
        public static string ObtencionMaquinasEsquema()
        {
            string json = LoadJson("Maquinas");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionMaquinasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Maquinas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Maquinas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Maquinas
        public static string ObtencionMaquinaDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Maquinas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MAQ" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionMaquinasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Maquinas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MAQ" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Maquina
        public static string AltaMaquina(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Maquinas");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Maquina
        public static string EditarMaquina(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Maquinas");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Maquina
        public static string EliminarMaquina(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Maquinas");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Maquina

        #region Familias

        //Tamaño Familia. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Familias
        public static string ObtencionFamiliasEsquema()
        {
            string json = LoadJson("Familias");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionFamiliasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Familias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Familias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Familias
        public static string ObtencionFamiliasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Familias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FAM" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionFamiliasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Familias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FAM" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Familia
        public static string AltaFamilia(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Familias");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Familia
        public static string EditarFamilia(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Familias");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Familia
        public static string EliminarFamilia(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Maquinas");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Familias

        #region Agencias

        //Tamaño Agencias. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Agencias
        public static string ObtencionAgenciasEsquema()
        {
            string json = LoadJson("Agencias");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionAgenciasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Agencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Agencias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Agencias
        public static string ObtencionAgenciasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Agencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionAgenciasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Agencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Agencia
        public static string AltaAgencia(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Agencias");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Agencia
        public static string WS_EditarAgencia(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Agencias");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Agencia
        public static string EliminarAgencia(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Agencias");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Agencias

        #region EstadoFabricacionControl

        //Tamaño Agencias. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Agencias
        public static string ObtencionEstadoFabricacionControlEsquema()
        {
            string json = LoadJson("OrdenFabricacionesEstado");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoFabricacionControlRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Agencias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Agencias
        public static string ObtencionEstadoFabricacionControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionEstadoFabricacionControlDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Agencia
        public static string AltaEstadoFabricacionControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Agencia
        public static string WS_EditarEstadoFabricacionControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Agencia
        public static string EliminarEstadoFabricacionControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion EstadoFabricacionControl

        #region EstadoMaquinaControl

        //Tamaño Agencias. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Agencias
        public static string ObtencionEstadoMaquinaControlEsquema()
        {
            string json = LoadJson("EstadoMaquina");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoMaquinaControlRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Agencias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Agencias
        public static string ObtencionEstadoMaquinaControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionEstadoMaquinaControlDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Agencia
        public static string AltaEstadoMaquinaControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Agencia
        public static string WS_EditarEstadoMaquinaControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Agencia
        public static string EliminarEstadoMaquinaControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion EstadoMaquinaControl

        #region EstadoExistenciasControl

        //Tamaño Agencias. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Agencias
        public static string ObtencionEstadoExistenciasControlEsquema()
        {
            string json = LoadJson("EstadoExistencias");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoExistenciasControlRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Agencias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Agencias
        public static string ObtencionEstadoExistenciasControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionEstadoExistenciasControlDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Agencia
        public static string AltaEstadoExistenciasControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Agencia
        public static string WS_EditarEstadoExistenciasControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Agencia
        public static string EliminarEstadoExistenciasControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion EstadoExistenciasControl

        #region EstadoPedidoControl

        //Tamaño Agencias. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Agencias
        public static string ObtencionEstadoPedidoControlEsquema()
        {
            string json = LoadJson("EstadoPedido");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoPedidoControlRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("EstadoPedido");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Agencias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Agencias
        public static string ObtencionEstadoPedidoControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoPedido");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionEstadoPedidoControlDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoPedido");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Agencia
        public static string AltaEstadoPedidoControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoPedido");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Agencia
        public static string WS_EditarEstadoPedidoControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoPedido");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Agencia
        public static string EliminarEstadoPedidoControl(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("EstadoPedido");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion EstadoPedidoControl

        #region Bom

        //Tamaño Bom. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Bom
        public static string ObtencionBomEsquema()
        {
            string json = LoadJson("Bom");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionBomRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Bom");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Bom. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Bom
        public static string WS_ObtencionBomDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Bom");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " BOM" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionBomDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Bom");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " BOM" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Bom
        public static string AltaBom(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Bom
        public static string EditarBom(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Bom
        public static string EliminarBom(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Bom

        #region ZonaIntercambio

        //Tamaño ZonaIntercambio. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ZonaIntercambio
        public static string ObtencionZonaIntercambioEsquema()
        {
            string json = LoadJson("ZonaIntercambio");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionZonaIntercambioRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos ZonaIntercambio. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ZonaIntercambio
        public static string ObtencionZonaIntercambioDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZON" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionZonaIntercambioDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZON" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ZonaIntercambio
        public static string AltaZonaIntercambio(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }

            string jsEntero = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un ZonaIntercambio
        public static string EditarZonaIntercambio(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un ZonaIntercambio
        public static string EliminarZonaIntercambio(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion ZonaIntercambio

        #region MotivosReg

        //Tamaño MotivosReg. Devuelve el esquema de datos y la cantidad de registros totales de la tabla MotivosReg
        public static string ObtencionMotivosRegEsquema()
        {
            string json = LoadJson("MotivosReg");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionMotivosRegRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("MotivosReg");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos MotivosReg. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla MotivosReg
        public static string ObtencionMotivosRegDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("MotivosReg");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MOTREG" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionMotivosRegDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("MotivosReg");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MOTREG" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un MotivosReg
        public static string AltaMotivosReg(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }

            string jsEntero = LoadJson("MotivosReg");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un MotivosReg
        public static string EditarMotivosReg(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("MotivosReg");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un MotivosReg
        public static string EliminarMotivosReg(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("MotivosReg");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion MotivosReg

        #region Rutas

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionRutasEsquema()
        {
            string json = LoadJson("Rutas");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionRutasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Rutas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Rutas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Rutas
        public static string ObtencionRutasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Rutas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTAS" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionRutasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Rutas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTAS" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Ruta
        public static string AltaRuta(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Rutas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Ruta
        public static string EditarRuta(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Rutas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Ruta
        public static string EliminarRuta(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Rutas");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Rutas

        #region RutasPreparacion

        //Tamaño RutasPreparacion. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RutasPreparacion
        public static string ObtencionRutasPreparacionEsquema()
        {
            string json = LoadJson("RutasPreparacion");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionRutasPreparacionRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos RutasPreparacion. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla RutasPreparacion
        public static string ObtencionRutasPreparacionDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTASPREP" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionRutasPreparacionDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTASPREP" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una RutaPreparacion
        public static string AltaRutaPreparacion(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (string.IsNullOrEmpty(_valor))
                    {
                        _valor = "NULL";
                    }
                    hshDatos.Add(_nombre, _valor);
                }
            }
            string jsEntero = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una RutaPreparacion
        public static string EditarRutaPreparacion(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            bool cerrar = false;
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);

            return ComponerACK(ok, error);
        }

        //Eliminar una RutaPreparacion
        public static string EliminarRutaPreparacion(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion RutasPreparacion

        #region IncidenciasInventario

        //Tamaño IncidenciasInventario. Devuelve el esquema de datos y la cantidad de registros totales de la tabla IncidenciasInventario
        public static string ObtencionIncidenciasInventarioEsquema()
        {
            string json = LoadJson("IncidenciasInventario");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionIncidenciasInventarioRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos IncidenciasInventario. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla IncidenciasInventario
        public static string ObtencionIncidenciasInventarioDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " INCIDENCIASINV" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            SentenciasSQL.queryIncidenciasInventario = query + " ORDER BY " + sortExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionIncidenciasInventarioDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " INCIDENCIASINV" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una IncidenciaInventario
        public static string AltaIncidenciaInventario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una RutaPreparacion
        public static string EditarIncidenciaInventario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una RutaPreparacion
        public static string EliminarIncidenciaInventario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion IncidenciasInventario

        #region TareasPendientes

        //Tamaño TareasPendientes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla TareasPendientes
        public static string ObtencionTareasPendientesEsquema()
        {
            string json = LoadJson("TareasPendientes");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionTareasPendientesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos TareasPendientes. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla TareasPendientes
        public static string ObtencionTareasPendientesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASPEND" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionTareasPendientesDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASPEND" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una TareaPendientes
        public static string AltaTareaPendiente(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }

            string jsEntero = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una TareaPendientes
        public static string EditarTareaPendiente(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una TareaPendientes
        public static string EliminarTareaPendiente(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion TareasPendientes

        #region TareasTipo

        //Tamaño TareasTipo. Devuelve el esquema de datos y la cantidad de registros totales de la tabla TareasTipo
        public static string ObtencionTareasTipoEsquema()
        {
            string json = LoadJson("TareasTipo");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionTareasTipoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("TareasTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos TareasTipo. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla TareasTipo
        public static string ObtencionTareasTipoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASTIPO" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionTareasTipoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASTIPO" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una TareaTipo
        public static string AltaTareaTipo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }

            string jsEntero = LoadJson("TareasTipo");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una TareaTipo
        public static string EditarTareaTipo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("TareasTipo");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una TareaTipo
        public static string EliminarTareaTipo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("TareasTipo");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion TareasTipo

        #region CombiPalets

        //Tamaño CombiPalets. Devuelve el esquema de datos y la cantidad de registros totales de la tabla CombiPalets
        public static string ObtencionCombiPaletsEsquema()
        {
            string json = LoadJson("CombiPalets");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionCombiPaletsRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("CombiPalets");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString(); string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos CombiPalets. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla CombiPalets
        public static string ObtencionCombiPaletsDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("CombiPalets");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " COMBIPALETS" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionCombiPaletsDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("CombiPalets");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " COMBIPALETS" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un CombiPalet
        public static string AltaCombiPalet(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("CombiPalets");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un CombiPalet
        public static string EditarCombiPalet(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("CombiPalets");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un CombiPalet
        public static string EliminarCombiPalet(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("CombiPalets");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion CombiPalets

        #region PaletsTipo

        //Tamaño PaletsTipo. Devuelve el esquema de datos y la cantidad de registros totales de la tabla PaletsTipo
        public static string ObtencionPaletsTipoEsquema()
        {
            string json = LoadJson("PaletsTipo");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionPaletsTipoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos PaletsTipo. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla PaletsTipo
        public static string ObtencionPaletsTipoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PALETSTIPO" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionPaletsTipoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PALETSTIPO" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un PaletTipo
        public static string AltaPaletTipo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }

            string jsEntero = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un PaletTipo
        public static string EditarPaletTipo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un PaletTipo
        public static string EliminarPaletTipo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion PaletsTipo

        #region FormatosSCC

        //Tamaño FormatosSCC. Devuelve el esquema de datos y la cantidad de registros totales de la tabla FormatosSCC
        public static string ObtencionFormatoSSCCEsquema()
        {
            string json = LoadJson("FormatosSCC");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionFormatoSSCCRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("FormatosSCC");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos FormatosSCC. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla FormatosSSCC
        public static string ObtencionFormatoSSCCDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("FormatoSSCC");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FORMATOSSCC" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionFormatoSSCCDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("FormatoSSCC");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FORMATOSSCC" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un FormatoSCC
        public static string AltaFormatoSCC(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("FormatoSSCC");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un FormatoSCC
        public static string EditarFormatoSCC(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("FormatoSSCC");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un FormatoSCC
        public static string EliminarFormatoSCC(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("FormatoSSCC");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion FormatosSCC

        #region Lotes

        //Tamaño Lotes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla FormatosSCC
        public static string ObtencionLotesEsquema()
        {
            string json = LoadJson("Lotes");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionLotesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Lotes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Lotes. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla FormatosSSCC
        public static string ObtencionLotesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Lotes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " LOT" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionLotesDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Lotes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " LOT" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Lote
        public static string AltaLote(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Lotes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Lote
        public static string EditarLote(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Lotes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Lote
        public static string EliminarLote(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Lotes");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Lotes

        #region Propietarios

        //Tamaño Propietarios. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Propietarios
        public static string ObtencionPropietariosEsquema()
        {
            string json = LoadJson("Propietarios");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionPropietariosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Propietarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Propietarios. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Propietarios
        public static string ObtencionPropietariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Propietarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROP" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionPropietariosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Propietarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROP" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Propietario
        public static string AltaPropietario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Propietarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Propietario
        public static string EditarPropietario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Propietarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Propietario
        public static string EliminarPropietario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Propietarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Propietarios

        #region Mantenimiento

        //Tamaño Mantenimiento. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RUMMANTENIMIENTO
        public static string ObtencionMantenimientoEsquema()
        {
            string json = LoadJson("Mantenimiento");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionMantenimientoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Mantenimiento. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Mantenimiento
        public static string ObtencionMantenimientoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MANT" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionMantenimientoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MANT" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Mantenimiento
        public static string AltaMantenimiento(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }

            string jsEntero = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Mantenimiento
        public static string EditarMantenimiento(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Mantenimiento
        public static string EliminarMantenimiento(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Mantenimiento

        #region Parametros

        public static DataTable GetParametrosTodos()
        {
            return ConexionSQL.getDataTable(GetParametrosTodosSQL);
        }

        //Tamaño Parametros. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RUMPARAMETROS
        public static string ObtencionParametrosEsquema()
        {
            string json = LoadJson("Parametros");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionParametrosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Parametros");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Parametros. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Mantenimiento
        public static string ObtencionParametrosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Parametros");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PARAM" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionParametrosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Parametros");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PARAM" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Parametro
        public static string AltaParametro(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }

            string jsEntero = LoadJson("Parametros");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Parametro
        public static string EditarParametro(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Parametros");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Parametro
        public static string EliminarParametro(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Parametros");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Parametros

        #region Usuarios

        //Tamaño Usuarios. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Usuarios
        public static string ObtencionUsuariosEsquema()
        {
            string json = LoadJson("Usuarios");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionUsuariosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Usuarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Usuarios. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Usuarios
        public static string ObtencionUsuariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Usuarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USU" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionUsuariosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Usuarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USU" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Usuario
        public static string AltaUsuario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (string.IsNullOrEmpty(_valor))
                    {
                        _valor = "NULL";
                    }
                    hshDatos.Add(_nombre, _valor);
                }
            }
            string jsEntero = LoadJson("Usuarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Usuario
        public static string EditarUsuario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Usuarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Usuario
        public static string EliminarUsuario(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Usuarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Usuarios

        #region ExistenciasHistorico

        //Tamaño ExistenciasHistorico. Devuelve el esquema de datos y la cantidad de registros totales de la tabla IncidenciasInventario
        public static string ObtencionExistenciasHistoricoEsquema()
        {
            string json = LoadJson("ExistenciasHistorico");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionExistenciasHistoricoRegistrosFiltrados(string filterExpressions, string strRelaciones)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = ConvertirFormatoWhereJSON(js.First()["WHERE"].ToString());
            string query = string.Empty;
            if (string.IsNullOrEmpty(jsonWhere) && string.IsNullOrEmpty(filterExpressions))
                query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones;
            else if (string.IsNullOrEmpty(jsonWhere) && !string.IsNullOrEmpty(filterExpressions))
                query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpressions;
            else if (!string.IsNullOrEmpty(jsonWhere) && string.IsNullOrEmpty(filterExpressions))
                query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " WHERE " + jsonWhere;
            else
                query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpressions + " AND " + jsonWhere;
            //string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions; //OBSOLETO
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static int ObtencionExistenciasHistoricoRegistrosFiltrados(string filterExpressions, string strRelaciones, bool filtroPersonalizado)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = ConvertirFormatoWhereJSON(js.First()["WHERE"].ToString());
            string query = string.Empty;
            if (filtroPersonalizado == true)
            {
                query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpressions;
            }
            else
            {
                if (string.IsNullOrEmpty(jsonWhere) && string.IsNullOrEmpty(filterExpressions))
                    query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones;
                else if (string.IsNullOrEmpty(jsonWhere) && !string.IsNullOrEmpty(filterExpressions))
                    query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpressions;
                else if (!string.IsNullOrEmpty(jsonWhere) && string.IsNullOrEmpty(filterExpressions))
                    query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " WHERE " + jsonWhere;
                else
                    query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpressions + " AND " + jsonWhere;
            }
            //string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions; //OBSOLETO
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos ExistenciasHistorico. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ExistenciasHistorico
        public static string ObtencionExistenciasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = ConvertirFormatoWhereJSON(js.First()["WHERE"].ToString());
            string query = string.Empty;
            if (string.IsNullOrEmpty(jsonWhere))
                query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            else
                query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte WHERE " + jsonWhere;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionExistenciasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones, string filtroInicial)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = ConvertirFormatoWhereJSON(js.First()["WHERE"].ToString());
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte" + filtroInicial;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        private static string ConvertirFormatoWhereJSON(string v)
        {
            var dict = new Dictionary<string, string>()
            {
                {"@hoy", "GETDATE()"},
            };

            foreach (var item in dict)
            {
                if (v.Contains(item.Key))
                    v = v.Replace(item.Key, item.Value);
            }
            return v;
        }

        public static string ObtencionExistenciasHistoricoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = ConvertirFormatoWhereJSON(js.First()["WHERE"].ToString());
            string query = string.Empty;
            if (string.IsNullOrEmpty(jsonWhere))
                query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            else
                query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte WHERE " + jsonWhere;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionExistenciasHistoricoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones, string filtroInicial)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = ConvertirFormatoWhereJSON(js.First()["WHERE"].ToString());
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte" + filtroInicial;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una ExistenciaHistorico
        public static string AltaExistenciasHistorico(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una ExistenciasHistorico
        public static string EditarExistenciasHistorico(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una RutaPreparacion
        public static string EliminarExistenciasHistorico(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion ExistenciasHistorico

        #region Visor Log

        //Tamaño RumLog. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RumLog
        public static string ObtencionRumLogEsquema()
        {
            string json = LoadJson("RumLog");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema RumLog|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionRumLogRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("RumLog");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos ExistenciasHistorico. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla RumLog
        public static string ObtencionRumLogDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RumLog");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUMLOG " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string registrosFiltrados = query + selectExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionRumLogDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RumLog");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUMLOG" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una RumLog
        public static string AltaRumLog(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RumLog");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una RumLog
        public static string EditarRumLog(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RumLog");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una RutaPreparacion
        public static string EliminarRumLog(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RumLog");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Visor Log

        #region Permisos

        public static string ObtencionPermisosEsquema()
        {
            string json = LoadJson("Permisos");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int getIdGrupo(String nombreGrupo)
        {
            int res = -1;
            DataTable dt = ConexionSQL.getDataTable("SELECT IDGRUPO FROM RUMGRUPOS WHERE DESCGRUPO LIKE '" + nombreGrupo + "'");
            if (dt != null)
            {
                int.TryParse(dt.Rows[0][0].ToString(), out res);
            }
            return res;
        }

        public static int getIdMantenimiento(String nombreMantenimiento)
        {
            int res = -1;
            DataTable dt = ConexionSQL.getDataTable("SELECT IDMANTENIMIENTO FROM RUMMANTENIMIENTO WHERE CAPTION LIKE '" + nombreMantenimiento + "'");
            if (dt != null)
            {
                int.TryParse(dt.Rows[0][0].ToString(), out res);
            }
            return res;
        }

        public static int ObtencionPermisosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Permisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string ObtencionPermisosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Permisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PERM " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionPermisosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Permisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PERM " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string AltaPermiso(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    //En alta permisos se tiene que poder añadir todos  los campos no solo los PrimaryKey
                    if (!tesquema.EsPK || true)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Permisos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Permiso
        public static string EditarPermiso(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Permisos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Permiso
        public static string EliminarPermiso(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        if (_nombre.Equals("IDGRUPO"))
                        {
                            _valor = DataAccess.getIdGrupo(_valor).ToString();
                        }
                        if (_nombre.Equals("RECURSO"))
                        {
                            _valor = DataAccess.getIdMantenimiento(_valor).ToString();
                        }
                        if (_valor.Equals("-1"))
                        {
                            log.Error("No se ha completado la operación porque no se ha encontrado el " + _nombre + " en la bbdd");
                            return ComponerACK(false, Lenguaje.traduce("No se ha completado la operación")); ;
                        }
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            if (hshClausula.Count == 0)
            {
                foreach (JObject content in array.Children<JObject>())
                {
                    foreach (JProperty prop in content.Properties())
                    {
                        string _nombre = prop.Name;
                        string _valor = prop.Value.ToString();

                        TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Permisos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Permisos

        #region PedidoProveedor

        //Crear un Proveedor

        public static string AltaPedidoProveedor(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);

            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string EditarPedidoProveedor(string json)

        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor

        public static string EliminarPedidoProveedor(string json)
        {
            string error = string.Empty;

            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;

                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion PedidoProveedor

        #region OrdenesRecogidaPedidoCli

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionOrdenesRecogidaEsquema()
        {
            string json = LoadJson("OrdenesRecogida");

            try

            {
                JArray js = JArray.Parse(json);

                string jsonFrom = js.First()["FROM"].ToString();

                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;

                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)

            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);

                json = "";

                return json;
            }
        }

        public static int ObtencionOrdenesRecogidaRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("OrdenesRecogida");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionOrdenesRecogidaDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("OrdenesRecogida");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " " + strCampos[2] + "  ) SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion OrdenesRecogidaPedidoCli

        #region OrdenesRecogidaCab

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionOrdenesRecogidaCabEsquema()
        {
            string json = LoadJson("OrdenRecogidasCab");

            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema  ObtencionOrdenesRecogidaCabEsquema OrdenRecogidasCab|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionOrdenesRecogidaCabRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("OrdenRecogidasCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }
            return registrosFiltrados;
        }

        public static string ObtencionOrdenesRecogidaCabDatosGridView(string sortExpression, string[] strCampos, string filter)
        {
            string json = LoadJson("OrdenRecogidasCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " WHERE ol.idpedidocli in(" + filter + ")) SELECT * " + " FROM PagingCte";
            //Monica 04/05/2021 Utilizo DENSERANK en vez de ROWNUMBER para evitar duplicados
            string query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = DENSE_RANK() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " WHERE ol.idpedidocli in(" + filter + ")) SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOrdenesRecogidaCabNoMarcadasDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("OrdenRecogidasCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();

            if (!String.IsNullOrEmpty(strCampos[2]))
            {
                strCampos[2] = " WHERE " + strCampos[2];
            }

            //string query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " " +strCampos[2] +" ) SELECT * " + " FROM PagingCte";

            //Monica 04/05/2021 Utilizo DENSERANK en vez de ROWNUMBER para evitar duplicados
            string query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = DENSE_RANK() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " " + strCampos[2] + " ) SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion OrdenesRecogidaCab

        #region OrdenesRecogidaCarga

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionOrdenRecogidaExpedicionesEsquema()
        {
            string json = LoadJson("OrdenRecogidasExpediciones");

            try

            {
                JArray js = JArray.Parse(json);

                string jsonFrom = js.First()["FROM"].ToString();

                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;

                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionOrdenRecogidaExpedicionesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("OrdenRecogidasExpediciones");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }
            return registrosFiltrados;
        }

        public static string ObtencionOrdenRecogidaExpedicionesDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("OrdenRecogidasExpediciones");
            JArray js = JArray.Parse(json);
            string query = "";
            if (strCampos[2] != null)
                query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " where " + strCampos[2] +/*ca.idcargaestado not like('PC')*/") SELECT * " + " FROM PagingCte";
            else
                query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] +/*ca.idcargaestado not like('PC')*/") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOrdenesCargaPackingListPteDatosGridView()
        {
            string json = LoadJson("OrdenesCargaPackingListPte");
            string jsonUnion = LoadJson("OrdenesCargaPackingListPadrePte");
            string[] strCampos = Business.GetFieldSQLUnion("OrdenesCargaPackingListPte", "OrdenesCargaPackingListPadrePte");
            string query = "SELECT DISTINCT " + strCampos[0] + " FROM " + strCampos[1] + " WHERE " + strCampos[2] + " UNION SELECT DISTINCT "
                + strCampos[3] + " FROM " + strCampos[4] + " WHERE " + strCampos[5];

            //string query = ";WITH PagingCte AS (SELECT Distinct " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " where ca.idcargaestado not like('PC')) SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOrdenesCargaJerarquicoPackingListEsquema()
        {
            string json = LoadJson("OrdenesCargaJerarquicoPackingList");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionOrdenesCargaPackingListPteEsquema()
        {
            string json = LoadJson("OrdenesCargaPackingListPte");
            string jsonUnion = LoadJson("OrdenesCargaPackingListPadrePte");
            string[] strCampos = Business.GetFieldSQLUnion("OrdenesCargaPackingListPte", "OrdenesCargaPackingListPadrePte");
            string query = "SELECT DISTINCT " + strCampos[0] + " FROM " + strCampos[1] + " WHERE " + strCampos[2] + " UNION SELECT DISTINCT "
                + strCampos[3] + " FROM " + strCampos[4] + " WHERE " + strCampos[5];
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOrdenesCargaJerarquicoPackingList(string sortExpression, string[] strCampos, string valor)
        {
            string json = LoadJson("OrdenesCargaPackingListPte");
            JArray js = JArray.Parse(json);
            if (strCampos[3] != string.Empty)
            {
                strCampos[3] = " group by " + strCampos[3];
            }
            string query = "SELECT " + strCampos[0] + " FROM " + strCampos[1] + " where " + strCampos[2] + valor + strCampos[3];
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOrdenesCargaJerarquicoPedidosEsquema()
        {
            string json = LoadJson("OrdenesCargaJerarquicoPedidos");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionOrdenesCargaJerarquicoPedidos(string[] strCampos, string valor)
        {
            string json = LoadJson("OrdenesCargaJerarquicoPedidos");
            JArray js = JArray.Parse(json);
            if (strCampos[3] != string.Empty)
            {
                strCampos[3] = " group by " + strCampos[3];
            }
            string query = "SELECT DISTINCT " + strCampos[0] + " FROM " + strCampos[1] + " where " + strCampos[2] + valor + strCampos[3];
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOrdenCargaPedidosResumen(string[] strCampos, string valor)
        {
            string json = LoadJson("OrdenesCargaPedidosResumen");
            JArray js = JArray.Parse(json);
            if (strCampos[3] != string.Empty)
            {
                strCampos[3] = " group by " + strCampos[3];
            }
            string query = "SELECT DISTINCT " + strCampos[0] + " FROM " + strCampos[1] + " where " + strCampos[2] + valor + strCampos[3];
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOrdenCargaPedidosResumenSql(string[] strCampos, string valor)
        {
            string json = LoadJson("OrdenesCargaPedidosResumen");
            JArray js = JArray.Parse(json);
            if (strCampos[3] != string.Empty)
            {
                strCampos[3] = " group by " + strCampos[3];
            }
            string query = "SELECT DISTINCT " + strCampos[0] + " FROM " + strCampos[1] + " where " + strCampos[2] + valor + strCampos[3];
            return query;
        }

        public static string ObtencionOrdenesCargaJerarquicoContenidoEsquema()
        {
            string json = LoadJson("OrdenesCargaJerarquicoContenido");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionOrdenesCargaJerarquicoContenido(string[] strCampos, string valor)
        {
            string json = LoadJson("OrdenesCargaJerarquicoContenido");
            JArray js = JArray.Parse(json);
            if (strCampos[3] != string.Empty)
            {
                strCampos[3] = " group by " + strCampos[3];
            }
            string query = "SELECT " + strCampos[0] + " FROM " + strCampos[1] + " where " + strCampos[2] + valor + strCampos[3];
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion OrdenesRecogidaCarga

        #region AcopiosProduccion

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionAcopiosProduccionEsquema()
        {
            string json = LoadJson("AcopiosProduccion");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionAcopiosProduccionRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("AcopiosProduccion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionAcopiosProduccionDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("AcopiosProduccion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonGroup = js.First()["Group By"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            if (jsonGroup != string.Empty)
            {
                jsonGroup = " GROUP BY " + jsonGroup;
            }
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " " + jsonWhere + " " + jsonGroup + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion AcopiosProduccion

        #region AcopiosProduccionJerarquicoArticulos

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionAcopiosProduccionJerarquicoEsquema()
        {
            string json = LoadJson("AcopiosProduccionJerarquico");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionAcopiosProduccionJerarquicoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("AcopiosProduccionJerarquico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConexionSQL.getConnectionString());
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionAcopiosProduccionJerarquicoDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("AcopiosProduccionJerarquico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonGroup = js.First()["Group By"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            if (jsonGroup != string.Empty)
            {
                jsonGroup = " GROUP BY " + jsonGroup;
            }
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " " + jsonWhere + " " + jsonGroup + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion AcopiosProduccionJerarquicoArticulos

        #region AcopiosProduccionJerarquicoSalidas

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionAcopiosProduccionJerarquicoSalidasEsquema()
        {
            string json = LoadJson("AcopiosSalidas");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionAcopiosProduccionJerarquicoSalidasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("AcopiosSalidas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConexionSQL.getConnectionString());
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionAcopiosProduccionJerarquicoSalidasDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("AcopiosSalidas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion AcopiosProduccionJerarquicoSalidas

        #region AcopiosProduccionJerarquicoEntradas

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionAcopiosProduccionJerarquicoEntradasEsquema()
        {
            string json = LoadJson("AcopiosEntradas");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionAcopiosProduccionJerarquicoEntradasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("AcopiosEntradas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConexionSQL.getConnectionString());
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionAcopiosProduccionJerarquicoEntradasDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("AcopiosEntradas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion AcopiosProduccionJerarquicoEntradas

        #region ArticulosAcopiosProduccion

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionArticulosAcopiosProduccionEsquema()
        {
            string json = LoadJson("ArticulosAcopiosProduccion");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();
                string jsonWhere = js.First()["WHERE"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                if (relaciones.Length > 0)
                {
                    selectExpression = selectExpression + " " + aliasTabla + " " + relaciones;
                }
                if (jsonWhere != null && !jsonWhere.Equals(""))
                {
                    selectExpression = selectExpression + " WHERE " + jsonWhere;
                }
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema ArticulosAcopiosProduccion || " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionArticulosAcopiosProduccionRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ArticulosAcopiosProduccion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConexionSQL.getConnectionString());
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionArticulosAcopiosProduccionDatosGridView(string sortExpression, string filterExpression, string[] strCampos)
        {
            string json = LoadJson("ArticulosAcopiosProduccion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " WHERE fcc.IDPEDIDOFAB in( " + filterExpression + ")  group by " + strCampos[3] + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion ArticulosAcopiosProduccion

        #region ArticulosAcopiosProduccionJerarquico

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionArticulosAcopiosProduccionJerarquicoEsquema()
        {
            string json = LoadJson("ArticulosAcopiosProduccionJerarquico");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }

                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                if (relaciones.Length > 0)
                {
                    selectExpression = selectExpression + " " + aliasTabla + " " + relaciones;
                }
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionArticulosAcopiosProduccionJerarquicoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ArticulosAcopiosProduccionJerarquico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConexionSQL.getConnectionString());
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionArticulosAcopiosProduccionJerarquicoDatosGridView(string sortExpression, int ordenFab, string[] strCampos)
        {
            string json = LoadJson("ArticulosAcopiosProduccionJerarquico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonGroup = js.First()["Group By"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            if (jsonGroup != string.Empty)
            {
                jsonGroup = " GROUP BY " + jsonGroup;
            }
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " WHERE fc.IDPEDIDOFAB in( " + ordenFab + ")  " + jsonGroup + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion ArticulosAcopiosProduccionJerarquico

        #region EstadoFabricacion

        public static string ObtencionEstadoFabricacionEsquema()

        {
            string json = LoadJson("OrdenFabricacionesEstado");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoFabricacionRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string ObtencionEstadoFabricacionDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("OrdenFabricacionesEstado");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion EstadoFabricacion

        #region EstadoMaquina

        public static string ObtencionEstadoMaquinaEsquema()

        {
            string json = LoadJson("EstadoMaquina");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoMaquinaRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string ObtencionEstadoMaquinaDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoMaquina");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion EstadoMaquina

        #region EstadoExistencias

        public static string ObtencionEstadoExistenciasEsquema()

        {
            string json = LoadJson("EstadoExistencias");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoExistenciasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            if (!filterExpressions.Equals(""))
            {
                query += " WHERE " + filterExpressions;
            }
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores
        public static string ObtencionEstadoExistenciasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoExistencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion EstadoExistencias

        #region EstadoTareas

        public static string ObtencionEstadoTareasEsquema()

        {
            string json = LoadJson("EstadoTareas");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionEstadoTareasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("EstadoTareas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores
        public static string ObtencionEstadoTareasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("EstadoTareas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonGroup = js.First()["Group By"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + " group by " + jsonGroup + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion EstadoTareas

        #region ControlTareas

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionControlTareasEsquema()
        {
            string json = LoadJson("ControlTareas");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionControlTareasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ControlTareas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionControlTareasDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("ControlTareas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion ControlTareas

        #region ControlTareasJerarquico

        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string ObtencionControlTareasJerarquicoEsquema()
        {
            string json = LoadJson("ControlTareasJerarquico");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionControlTareasJerarquicoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ControlTareasJerarquico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }

            return registrosFiltrados;
        }

        public static string ObtencionControlTareasJerarquicoDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("ControlTareasJerarquico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion ControlTareasJerarquico

        #region Combos

        public static DataTable ObtencionDatosComboMulti(string elemento, string valor, string tabla)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + "), " + valor + " from " + tabla + " ORDER BY " + valor + " ASC";
            DataTable dt = ConexionSQL.getDataTable(selectExpression);

            return dt;
        }

        public static DataTable ObtencionDatosComboMulti(string elemento, string valor, string tabla, string filtro)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + ") , " + valor + "  from " + tabla + " " + filtro + " ORDER BY " + valor + " ASC";
            DataTable dt = ConexionSQL.getDataTable(selectExpression);

            return dt;
        }

        public static DataTable ObtencionDatosComboMultiSQL(string elemento, string valor, string from)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + ") AS ELEMENTO, " + valor + " AS VALOR from " + from;
            DataTable dt = ConexionSQL.getDataTable(selectExpression);

            return dt;
        }

        public static string ObtencionDatosCombo(string elemento, string valor, string tabla)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + ") AS ELEMENTO, " + valor + " AS VALOR from " + tabla + " ORDER BY " + valor + " ASC";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            return datosJSON;
        }

        public static string ObtencionDatosCombo(string elemento, string valor, string tabla, string filtro)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + ") AS ELEMENTO, " + valor + " AS VALOR from " + tabla + " " + filtro + " ORDER BY " + valor + " ASC";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            return datosJSON;
        }

        public static string ObtencionDatosComboSQL(string elemento, string valor, string from)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + ") AS ELEMENTO, " + valor + " AS VALOR from " + from;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            return datosJSON;
        }

        #endregion Combos

        #region stock

        public static string ObtencionStockEsquema()
        {
            string json = LoadJson("Stock");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionStockDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string[] strCampos)
        {
            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (" + sortExpression + ") FROM " + strCampos[1] + " " + filterExpression + ") SELECT * FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionStockDatosGridView(string sortExpression, string filterExpression, string[] strCampos)
        {
            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " " + filterExpression + ") SELECT * FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static int ObtencionStockCantidadFiltrados(string filterExpression)
        {
            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpression;
            int cant = ConexionSQL.getRegistrosFiltrados(selectExpression);

            return cant;
        }

        public static string ObtencionStockStatusEsquema()
        {
            string json = LoadJson("StockStatus");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionStockStatusDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("StockStatus");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " STOCKSTA" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionStockStatusDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("StockStatus");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RESHIST" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //public static DataTable WS_ObtencionStockCantidadDiusframi()
        //{
        //    string query = XmlReader.getStockCant();
        //    DataTable dt = ConexionSQL.getDataTableDiusframi(query);
        //    return dt;

        //}
        //public static DataTable WS_ObtencionStockDatosDiusframi(List<TableScheme> lstEsquemaTabla)
        //{
        //    //string query = XmlReader.getStockSelect();
        //    string[] query=Business.GetFieldsSQLStock(lstEsquemaTabla);

        //    DataTable dt = ConexionSQL.getDataTableDiusframi(query);
        //    return dt;
        //}
        //public static DataTable WS_ObtencionStockDatosFiltrados(string filtro)
        //{
        //    string query = XmlReader.getStockFiltrado(filtro);
        //    DataTable dt = ConexionSQL.getDataTableDiusframi(query);
        //    return dt;
        //}

        #endregion stock

        #region UsuariosGrupos

        //Tamaño Usuarios. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Usuarios
        public static string ObtencionUsuariosGruposEsquema()
        {
            string json = LoadJson("UsuariosGrupos");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionUsuariosGruposRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Usuarios. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Usuarios
        public static string ObtencionUsuariosGruposDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USUGRUP" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionUsuariosGruposDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USUGRUP" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        //Crear un UsuarioGrupo
        public static string AltaUsuarioGrupo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un UsuarioGrupo
        public static string EditarUsuarioGrupo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un UsuarioGrupo
        public static string EliminarUsuarioGrupo(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion UsuariosGrupos

        #region Grupos

        //Tamaño Maquina. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Maquinas
        public static string ObtencionGruposEsquema()
        {
            string json = LoadJson("Grupos");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionGruposRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Grupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Maquinas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Maquinas
        public static string ObtencionGruposDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Grupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " GRUP" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionGruposDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Grupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " GRUP" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        //Crear una Maquina
        public static string AltaGrupos(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (string.IsNullOrEmpty(_valor))
                    {
                        _valor = "NULL";
                    }
                    if (_valor.Contains("''") && !_valor.Equals("''"))
                    {
                        _valor = _valor.Replace("''", "'''");
                    }
                    hshDatos.Add(_nombre, _valor);
                }
            }
            string ja = LoadJson("Grupos");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Maquina
        public static string EditarGrupos(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        if (_valor.Contains("''") && !_valor.Equals("''"))
                        {
                            _valor = _valor.Replace("''", "'''");
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Grupos");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Maquina
        public static string EliminarGrupos(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string ja = LoadJson("Grupos");
            JArray js = JArray.Parse(ja);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Grupos

        #region Operarios

        //Tamaño Lotes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla FormatosSCC
        public static string ObtencionOperariosEsquema()
        {
            string json = LoadJson("Operarios");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionOperariosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Operarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Lotes. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla FormatosSSCC
        public static string ObtencionOperariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Operarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " LOT" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionOperariosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Operarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " LOT" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        //Crear un Lote
        public static string AltaOperarios(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Operarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Lote
        public static string EditarOperarios(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Operarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Lote
        public static string EliminarOperarios(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("Operarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        public static void ActualizarPasswordUsuarioOperario(string idUsuario, string idOperario, string claveElegida)
        {
            string queryUsuario = "UPDATE RUMUSUARIOS " + "SET CLAVEUSUARIO ='" + claveElegida + "' WHERE IDUSUARIO =" + idUsuario;
            string queryOperario = "UPDATE TBLOPERARIOS " + "SET PASSWORD ='" + claveElegida + "' WHERE IDOPERARIO =" + idOperario;
            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = queryOperario;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = queryUsuario;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(Lenguaje.traduce("Se ha cambiado la clave correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:" + idUsuario);

                    MessageBox.Show(Lenguaje.traduce("No se ha podido cambiar la contraseña de este usuario"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void CrearNuevoGrupo(string idGrupo, string descripcionGrupo)
        {
            string query = "INSERT INTO RUMGRUPOS(IDGRUPO, DESCGRUPO, RUMLOGS) VALUES('" + idGrupo + "', '" + descripcionGrupo + "',  NULL)";
            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(Lenguaje.traduce("Se ha creado el nuevo grupo correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdGrupo:" + idGrupo);

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este grupo"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void EditarGrupoOperario(string idGrupo, string descripcionGrupo)
        {
            string query = "INSERT INTO RUMGRUPOS(IDGRUPO, DESCGRUPO, RUMLOGS) VALUES('" + idGrupo + "', '" + descripcionGrupo + "',  NULL)";
            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(Lenguaje.traduce("Se ha creado el nuevo grupo correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdGrupo:" + idGrupo);

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este grupo"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void EditarGrupoUsuario(string idGrupo, string idUsuario)
        {
            /*
            UPDATE RUMUSUGRUPO
            SET IDGRUPO = ‘grupoelegido’
            WHERE IDUSUARIO = ‘Id del usuario seleccionado’;

            SELECT IDUSUARIO FROM RUMUSUGRUPO WHERE IDUSUARIO = idUsuario
            */

            string query = "UPDATE RUMUSUGRUPO SET IDGRUPO = " + idGrupo + " WHERE IDUSUARIO = " + idUsuario;
            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(Lenguaje.traduce("Se ha cambiado el nuevo grupo correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdGrupo:" + idGrupo);

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este grupo"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void NuevoUsuarioVinculadoOperario(string nombreElegido, string claveElegida)
        {
            string queryIUsuarios = "SELECT TOP 1 IDUSUARIO FROM RUMUSUARIOS ORDER BY IDUSUARIO DESC";
            //       string query = "INSERT INTO RUMUSUARIOS ( NOMUSUARIO, CLAVEUSUARIO) VALUES('" + nombreElegido + "', '" + claveElegida + "')";
            //     string query = "INSERT INTO RUMUSUARIOS (IDUSUARIO, NOMUSUARIO, CLAVEUSUARIO) VALUES('" + idUsuario + "', '" + nombreElegido + "', '" + claveElegida + "')";
            string queryOperario = "INSERT INTO TBLOPERARIOS " +
                                   "(NOMBRE, APELLIDOS, PASSWORD, MODOPANT, IDESTADO, VOLUMEN, STRENGTH, ULTIMAMAQUINA, ULTIMAMODIFICACION, SENSITIVITY, PERMISOREGULARIZACION)" +
                                   "VALUES ('" + nombreElegido + "', '', ' " + claveElegida + "', '', 'AC', 50, -2500, '', '', 50, '');" +
                                   "SELECT IDOPERARIO FROM TBLOPERARIOS " +
                                      "WHERE IDOPERARIO = SCOPE_IDENTITY()";
            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();
            string idUsuarioLog = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = queryIUsuarios;
                    cmd.CommandType = CommandType.Text;
                    var selectIdUsuario = cmd.ExecuteScalar();
                    int idUsuario = (int)selectIdUsuario + 1;
                    idUsuarioLog = idUsuario.ToString();
                    string query = "INSERT INTO RUMUSUARIOS (IDUSUARIO, NOMUSUARIO, CLAVEUSUARIO) VALUES('" + idUsuario + "', '" + nombreElegido + "', '" + claveElegida + "')";
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = queryOperario;
                    cmd.CommandType = CommandType.Text;
                    var idOperario = cmd.ExecuteScalar();
                    string queryVincular = "INSERT INTO TBLUSUARIOOPERARIO(IDUSUARIO, IDOPERARIO) VALUES('" + idUsuario + "', '" + idOperario.ToString() + "')";
                    cmd.CommandText = queryVincular;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    string grupoPorDefecto = "INSERT INTO RUMUSUGRUPO(IDUSUARIO, IDGRUPO) VALUES('" + idUsuario + "', '4')";
                    cmd.CommandText = grupoPorDefecto;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(Lenguaje.traduce("Se ha creado el nuevo usuario y se ha vinculado al operario en curso correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este usuario"));
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:" + idUsuarioLog + 1);

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este usuario"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void EditarOperario(string nombreOperario, string apellidos, string password, string modoPantalla, string fuerza,
            string volumen, string ultimaMaquina, string ultimaModificacion, string sensibilidad, string permisoRegularizacion, string estadoSeleccionado, string idOperario)
        {
            string query = "UPDATE TBLOPERARIOS " + "SET NOMBRE ='" + nombreOperario + "', APELLIDOS ='" + apellidos +
                "', MODOPANT ='" + modoPantalla + "', IDESTADO ='" + estadoSeleccionado + "', VOLUMEN ='" + volumen + "', STRENGTH ='" + fuerza + "', ULTIMAMAQUINA ='" + ultimaMaquina +
                "', ULTIMAMODIFICACION ='" + ultimaModificacion + "', SENSIBILIDAD ='" + sensibilidad + "', PERMISOREGULARIZACION ='" + permisoRegularizacion +
                "' WHERE IDOPERARIO =" + idOperario;
            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(Lenguaje.traduce("Se ha editado el operario correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:");

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este usuario"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void NuevoOperario(string nombreOperario, string apellidos, string password, string modoPantalla, string fuerza,
            string volumen, string ultimaMaquina, string ultimaModificacion, string permisoRegularizacion, string estadoSeleccionado)
        {
            string query = "INSERT INTO TBLOPERARIOS (NOMBRE, APELLIDOS, PASSWORD, MODOPANT, IDESTADO, VOLUMEN, STRENGTH, ULTIMAMAQUINA, ULTIMAMODIFICACION, PERMISOREGULARIZACION ) VALUES('" + nombreOperario + "', '" + apellidos + "', '" + password + "', '" + modoPantalla + "','" + estadoSeleccionado + "', '" + volumen + "',  '" + fuerza + "', '" + ultimaMaquina + "', '" + ultimaModificacion + "', '" + permisoRegularizacion + "')";

            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(Lenguaje.traduce("Se ha creado el operario correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:");

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este usuario"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void asignarUsuarioOperario(string usuario, string operario)
        {
            string query = "INSERT INTO TBLUSUARIOOPERARIO (IDUSUARIO,IDOPERARIO ) VALUES('" + usuario + "', '" + operario + "')";

            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(Lenguaje.traduce("Se ha asignado el usuario correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:");

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este usuario"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static DataTable getDatosReserva(int idReserva)
        {
            return ConexionSQL.getDataTable("select r.*,ex.cantidad as cantidadpalet, e.sscc,a.referencia,a.descripcion,a.atributo " +
                "from dbo.tblreservas r inner join dbo.tblarticulos a on r.idarticulo=a.idarticulo" +
                " inner join dbo.tblentradas e on r.identrada=e.identrada " +
                " inner join dbo.tblexistencias ex on e.identrada=ex.identrada " +
                " where r.idReserva=" + idReserva);
        }

        public static DataTable getDatosExistencia(int idEntrada)
        {
            return ConexionSQL.getDataTable("select ex.*, e.sscc,e.lote,a.referencia,a.descripcion,a.atributo " +
                "from dbo.tblentradas e " +
                " inner join dbo.tblexistencias ex on e.identrada=ex.identrada " +
                " inner join dbo.tblarticulos a on ex.idarticulo=a.idarticulo" +
                " where ex.idEntrada=" + idEntrada);
        }

        public static DataTable getDatosSalida(int idSalida)
        {
            return ConexionSQL.getDataTable("SELECT S.*, A.REFERENCIA,A.ATRIBUTO,A.DESCRIPCION,A.IDPRESENTACION,A.TIPOPRESENTACION,EN.LOTE,EN.SSCC"
                    + " FROM DBO.TBLSALIDAS S "
                    + " INNER JOIN DBO.TBLENTRADAS EN ON EN.IDENTRADA = S.IDENTRADA"
                    + " INNER JOIN DBO.TBLARTICULOS A ON A.IDARTICULO = EN.IDARTICULO"
                    + " WHERE S.IDSALIDA = " + idSalida);
        }

        public static void EliminarOperario(string idOperario, string idUsuario)
        {
            if (idUsuario != "")
            {
                queryRelacionGrupo = "DELETE FROM RUMUSUGRUPO WHERE IDUSUARIO=" + idUsuario;
                queryRelacion = "DELETE FROM TBLUSUARIOOPERARIO WHERE IDUSUARIO=" + idUsuario;
                queryUsuario = "DELETE FROM RUMUSUARIOS WHERE IDUSUARIO=" + idUsuario;
            }
            string query = "DELETE FROM TBLOPERARIOS WHERE IDOPERARIO=" + idOperario;

            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    if (idUsuario != "")
                    {
                        cmd.CommandText = queryRelacionGrupo;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = queryRelacion;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = queryUsuario;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(Lenguaje.traduce("Se ha eliminado el operario correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:");

                    MessageBox.Show(Lenguaje.traduce("No se ha podido eliminar este operario"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion Operarios

        #region RecursosTarea

        //Tamaño Mantenimiento. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RUMMANTENIMIENTO
        public static string ObtencionRecursosTareaEsquema()
        {
            string json = LoadJson("RecursosTarea");
            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ALIAS";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + aliasTabla + " " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionRecursosTareaRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("RecursosTarea");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Mantenimiento. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Mantenimiento
        public static string ObtencionRecursosTareaDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RecursosTarea");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MANT" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionRecursosTareaDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RecursosTarea");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MANT" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        //Crear un Mantenimiento
        public static string AltaRecursosTarea(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RecursosTarea");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        public static void NuevoRecurso(string estadoSeleccionado, string descripcion, string nombreImpresora, string maxTareas, string fechaLogIn, string fechaLogout, string ubicacionActual, string ubicacionPrincipal)
        {
            string query = "INSERT INTO TBLRECURSOS (DESCRIPCION, ESTADO, NOMBREIMPRESORA, MAXTAREASRECURSO, FECHALOGIN, FECHALOGOUT ) VALUES('" + descripcion + "', '" + estadoSeleccionado + "', '" + nombreImpresora + "', '" + maxTareas + "', '" + fechaLogIn + "',  '" + fechaLogout + "')";

            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(Lenguaje.traduce("Se ha creado el recurso correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:");

                    MessageBox.Show(Lenguaje.traduce("No se ha podido crear este recurso"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void EditarRecurso(string idRecurso, string estadoSeleccionado, string descripcion, string nombreImpresora, string maxTareas, string fechaLogIn, string fechaLogout, string ubicacionActual, string ubicacionPrincipal)
        {
            string query = "UPDATE TBLRECURSOS " + "SET DESCRIPCION ='" + descripcion + "', ESTADO ='" + estadoSeleccionado + "', NOMBREIMPRESORA ='" + nombreImpresora +
                "', MAXTAREASRECURSO ='" + maxTareas + "', IDUBICACIONACTUAL ='" + ubicacionActual + "', FECHALOGIN ='" + fechaLogIn + "', FECHALOGOUT ='" + fechaLogout +
                "' WHERE IDRECURSO =" + idRecurso;

            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(Lenguaje.traduce("Se ha editado el recurso correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:");

                    MessageBox.Show(Lenguaje.traduce("No se ha podido editar este recurso"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void EliminarRecurso(string idRecurso)
        {
            string query = "DELETE FROM TBLRECURSOS WHERE IDRECURSO=" + idRecurso;

            SqlCommand cmd = new SqlCommand();
            connectionString = ConexionSQL.getConnectionString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(Lenguaje.traduce("Se ha eliminado el recurso correctamente"));
                }
                catch (SqlException e)
                {
                    ExceptionManager.GestionarError(e);
                    log.Debug("Mensaje:" + e.Message + " ||Stack:" + e.StackTrace);
                }
                catch (NullReferenceException ex)
                {
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace + "||IdUsuario:");

                    MessageBox.Show(Lenguaje.traduce("No se ha podido eliminar este recurso"));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        //Editar un Mantenimiento
        public static string EditarRecursosTarea(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RecursosTarea");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Mantenimiento
        public static string EliminarRecursosTarea(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("RecursosTarea");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion RecursosTarea

        #region PedidoProveedor

        public static string ObtencionPedidoProveedorEsquema()
        {
            string json = LoadJson("ProveedoresPedidosCab");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonWhere = js.First()["WHERE"].ToString();
                string where = "";
                if (jsonWhere != string.Empty)
                {
                    where = " WHERE " + jsonWhere;
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + where;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema ProveedoresPedidosCab || " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionPedidoProveedorRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE((IDPEDIDOPROESTADO = 'PC' AND[FechaCreacion] = '" + DateTime.Today + "') OR IDPEDIDOPROESTADO<> 'PC')";
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);
            cmd.Connection = connection;
            int registrosFiltrados = 0;
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
                connection.Dispose();
            }
            return registrosFiltrados;
        }

        public static string ObtencionPedidoProveedorDatosGridView(string sortExpression, string strCampos, string campoAlias, string relaciones)
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string where = "";
            string jsonWhere = js.First()["WHERE"].ToString();
            //string where = XmlReaderPropio.getFiltroInicial();
            //if (where!=string.Empty)
            //{
            //    where = " WHERE " + where;
            //}
            //else
            //{
            if (jsonWhere != string.Empty)
            {
                where = " WHERE " + jsonWhere;
            }
            //}

            //string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + ") SELECT * " + " FROM PagingCte";
            string query = "SELECT " + strCampos + " FROM " + jsonFrom + where;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static DataTable ObtencionRecepcionesCantidad(string[] strCampos, string[] strCamposUnion, string filter)
        {
            string json = LoadJson("RecepcionesCab");
            string jsonUnion = LoadJson("PedidosProvJerarquicoRecepciones");
            JArray js = JArray.Parse(json);
            JArray jsUnion = JArray.Parse(jsonUnion);
            string jsonWhere = js.First()["WHERE"].ToString();
            string jsonWhereUnion = jsUnion.First()["WHERE"].ToString();
            if (filter != string.Empty)
            {
                jsonWhere = " WHERE recep.IDPEDIDOPRO=" + filter;
                jsonWhereUnion = " WHERE lin.IDPEDIDOPRO=" + filter;
            }
            string query = "SELECT Count(*) FROM(SELECT " + strCampos[0] + " FROM " + strCampos[1] + " " + jsonWhere + " UNION SELECT DISTINCT " + strCamposUnion[0] + " FROM " + strCamposUnion[1] + " " + jsonWhereUnion + ") as count";

            //DataTable tabla = ConexionSQL.getDataTable(query);
            return null;
        }

        public static string ObtencionPedidosJerarquicoRecepcionesUnionEsquema(ref List<TableScheme> _lstEsquemaTabla)
        {
            string json = LoadJson("PedidosProvJerarquicoRecepciones");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionPedidoJerarquicoRecepciones(string sortExpression, string[] strCampos, string[] strCamposUnion, string strRelaciones, string strRelacionesUnion, string strCamposAlias, string strCamposAliasUnion, string filter)
        {
            string json = LoadJson("RecepcionesCab");
            string jsonUnion = LoadJson("PedidosProvJerarquicoRecepciones");
            JArray js = JArray.Parse(json);
            JArray jsUnion = JArray.Parse(jsonUnion);
            string jsonWhere = js.First()["WHERE"].ToString();
            string jsonWhereUnion = jsUnion.First()["WHERE"].ToString();
            if (filter != string.Empty)
            {
                jsonWhere = " WHERE recep.IDPEDIDOPRO=" + filter;
                jsonWhereUnion = " WHERE lin.IDPEDIDOPRO=" + filter;
            }
            string query = "SELECT " + strCampos[0] + " FROM " + strCampos[1] + " " + jsonWhere + " UNION SELECT DISTINCT " + strCamposUnion[0] + " FROM " + strCamposUnion[1] + " " + jsonWhereUnion;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionPedidosProJerarquicoEntradasEsquema()
        {
            string json = LoadJson("PedidosProvJerarquicoEntradas");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionPedidoJerarquicoEntradas(string sortExpression, string strCampos, string strRelaciones, string strCamposAlias, string filter)
        {
            string json = LoadJson("PedidosProvJerarquicoEntradas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            string select;
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            else
            {
                jsonWhere = string.Empty;
            }

            if (filter == "9999999999")
            {
                select = "SELECT TOP 1 ";
                filter = string.Empty;
            }
            else
            {
                select = "SELECT ";
                if (filter != string.Empty)
                {
                    filter = " WHERE IDPEDIDOPRO=" + filter;
                }
            }
            //string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
            string query = select + strCampos /*+ ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") */+ "FROM " + jsonFrom + " " + filter;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionPedidosProJerarquicoLineasEsquema()
        {
            string json = LoadJson("PedidosProvJerarquicoLineas");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionPedidoJerarquicoLineas(string sortExpression, string strCampos, string strRelaciones, string strCamposAlias, string filter)
        {
            string json = LoadJson("PedidosProvJerarquicoLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            string select = string.Empty;
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            else
            {
                jsonWhere = string.Empty;
            }
            select = "SELECT ";
            if (filter != string.Empty)
            {
                filter = " WHERE lin.IDPEDIDOPRO=" + filter;
            }
            string query = select + strCampos + " FROM " + jsonFrom + filter;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionPedidosProJerarquicoPreavisosEsquema()
        {
            string json = LoadJson("PedidosProvJerarquicoPreavisos");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionPedidoJerarquicoPreavisos(string sortExpression, string strCampos, string strRelaciones, string strCamposAlias, string filter)
        {
            string json = LoadJson("PedidosProvJerarquicoPreavisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            string select = string.Empty;
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            else
            {
                jsonWhere = string.Empty;
            }

            if (filter == "9999999999")
            {
                select = "SELECT TOP 1 ";
                filter = string.Empty;
            }
            else
            {
                select = "SELECT ";
                if (filter != string.Empty)
                {
                    filter = " WHERE T.IDPEDIDOPRO=" + filter;
                }
            }
            string query = select + strCampos + " FROM " + jsonFrom +/* " LIN" + strRelaciones +*/ filter;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        private static string GetProgressBarPorcentCantidadPteUbicar(List<TableScheme> lstEsquemaTabla, string filter)
        {
            string fields = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre;
                }
                if (i < lstEsquemaTabla.Count - 1)
                {
                    fields += ",";
                }
            }
            string json = LoadJson("PedidosProvJerarquicoPreavisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            string select = string.Empty;
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            else
            {
                jsonWhere = string.Empty;
            }

            if (filter == "9999999999")
            {
                select = "SELECT TOP 1 ";
                filter = string.Empty;
            }
            else
            {
                select = "SELECT ";
                if (filter != string.Empty)
                {
                    filter = " WHERE T.IDPEDIDOPRO=" + filter;
                }
            }
            string query = select + fields + " FROM " + jsonFrom +/* " LIN" + strRelaciones +*/ filter;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static DataTable ObtencionRecepcionesJerarquicoLineas(string[] query, string idRecepcion)
        {
            string select = "SELECT ";
            query[2] = query[2] + idRecepcion;
            query[4] = query[4] + idRecepcion;
            query[5] = query[5] + idRecepcion;
            string fullQuery = "( " + select + " " + query[0] + " " + query[1] + " " + query[2] + " " + query[6] + ") union (SELECT " + query[3] + " " + query[4] + " " + query[8] + " " + query[5] + " " + query[7] +/* " WHERE " + query[5]+idRecepcion+*/")";
            DataTable tabla = ConexionSQL.getDataTable(fullQuery);
            return tabla;
        }

        public static DataTable ObtencionRecepcionesJerarquicoEntradas(string[] query, string idRecepcion)
        {
            string select = "SELECT ";
            query[2] = query[2] + idRecepcion;
            query[5] = query[5] + idRecepcion;
            string fullQuery = "( " + select + " " + query[0] + " " + query[1] + " " + query[2] + ") union (SELECT DISTINCT " + query[3] + " " + query[4] + " " + query[5] +/* " WHERE " + query[5]+idRecepcion+*/")";
            DataTable tabla = ConexionSQL.getDataTable(fullQuery);
            return tabla;
        }

        public static DataTable ObtencionRecepcionesJerarquicoExistencias(string[] query, string idRecepcion)
        {
            string select = "SELECT ";
            query[2] = query[2] + idRecepcion;
            query[5] = query[5] + idRecepcion;
            string fullQuery = "( " + select + query[0] + " " + query[1] + " " + query[2] + ") union (SELECT DISTINCT " + query[3] + " " + query[4] + " " + query[5] +/* " WHERE " + query[5]+idRecepcion+*/")";
            DataTable tabla = ConexionSQL.getDataTable(fullQuery);
            return tabla;
        }

        public static DataTable ObtencionRecepcionesJerarquicoMovimientos(string[] query, string idRecepcion)
        {
            string select = "SELECT ";
            query[2] = query[2] + idRecepcion;
            query[5] = query[5] + idRecepcion;
            string fullQuery = select + query[0] + " " + query[1] + " " + query[2] + " union (SELECT DISTINCT " + query[3] + " " + query[4] + " " + query[5] +/* " WHERE " + query[5]+idRecepcion+*/")";
            DataTable tabla = ConexionSQL.getDataTable(fullQuery);
            return tabla;
        }

        public static DataTable ObtencionRecepcionesJerarquicoTareas(string[] query, string idRecepcion)
        {
            string select = "SELECT ";
            query[2] += idRecepcion;
            query[3] += idRecepcion + ")";
            string fullQuery = select + query[0] + " " + query[1] + " " + query[2] + " " + query[3];
            DataTable tabla = ConexionSQL.getDataTable(fullQuery);
            return tabla;
        }

        public static DataTable ObtencionRecepcionesJerarquicoPreavisos(string sortExpression, string[] strCampos, string strRelaciones, string strCamposAlias, string filter)
        {
            string json = LoadJson("RecepcionesJerarquicoPreavisos");
            JArray js = JArray.Parse(json);
            string select = string.Empty;
            select = "SELECT ";
            if (filter != string.Empty)
            {
                strCampos[2] += filter;
            }
            string query = select + " " + strCampos[0] + " " + strCampos[1] + " " + strCampos[2] /* " LIN" + strRelaciones +*/;

            DataTable tabla = ConexionSQL.getDataTable(query);
            //json = json.Replace("\"[DATA]\"", datosJSON);
            return tabla;
        }

        public static DataTable ObtencionRecepcionesJerarquicoEmbalajes(string sortExpression, string[] strCampos, string strRelaciones, string strCamposAlias, string filter)
        {
            string json = LoadJson("RecepcionesJerarquicoEmbalajes");
            JArray js = JArray.Parse(json);
            string select = string.Empty;
            select = "SELECT ";
            if (filter != string.Empty)
            {
                strCampos[2] += filter;
            }
            string query = select + " " + strCampos[0] + " " + strCampos[1] + " " + strCampos[2] /* " LIN" + strRelaciones +*/;

            DataTable tabla = ConexionSQL.getDataTable(query);
            //json = json.Replace("\"[DATA]\"", datosJSON);
            return tabla;
        }

        public static DataTable ObtencionDatosJerarquico(string nombreJson, string sortExpression, string[] strCampos, string strRelaciones, string strCamposAlias, string filter)
        {
            string json = LoadJson("RecepcionesJerarquicoPreavisos");
            JArray js = JArray.Parse(json);
            string select = string.Empty;
            select = "SELECT ";
            if (filter != string.Empty)
            {
                strCampos[2] += filter;
            }
            if (!strCampos[1].Contains("FROM"))
            {
                strCampos[1] = " FROM " + strCampos[1];
            }
            string query = select + " " + strCampos[0] + " " + strCampos[1] + " " + strCampos[2] /* " LIN" + strRelaciones +*/;

            DataTable tabla = ConexionSQL.getDataTable(query);
            //json = json.Replace("\"[DATA]\"", datosJSON);
            return tabla;
        }

        //Crear un Proveedor
        public static string AltaPedidoProveedorSergio(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string EditarPedidoProveedorSergio(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string EliminarPedidoProveedorSergio(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);
            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());
            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();
                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);
                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion PedidoProveedor

        #region RecepcionesProCab

        public static string ObtencionRecepcionesPedidodosProveedorEsquema()
        {
            string json = LoadJson("RecepcionesCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static int ObtencionRecepcionesPedidosProveedorRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("RecepcionesCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //public static string ObtencionRecepcionesPedidosProveedorDatosGridView(string sortExpression, string[] strCampos)
        //{
        //    string json = LoadJson("RecepcionesCab");
        //    JArray js = JArray.Parse(json);
        //    string jsonFrom = js.First()["FROM"].ToString();
        //    string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
        //    string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
        //    json = json.Replace("\"[DATA]\"", datosJSON);
        //    return json;
        //}
        public static string ObtencionRecepciones(string sortExpression, string[] strCampos, string[] strCamposUnion, string strRelaciones, string strRelacionesUnion, string strCamposAlias, string strCamposAliasUnion, string filter)
        {
            string query;
            string json = LoadJson("RecepcionesCab");
            string jsonFrom = strCampos[1];
            string jsonFromUnion = strCamposUnion[1];
            string jsonWhere = strCampos[2];
            string jsonWhereUnion = strCamposUnion[2];

            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            else
            {
                jsonWhere = string.Empty;
            }
            if (jsonWhereUnion != string.Empty)
            {
                jsonWhereUnion = " WHERE " + jsonWhereUnion;
            }
            else
            {
                jsonWhereUnion = string.Empty;
            }
            if (filter != string.Empty)
            {
                jsonWhere = " WHERE recep.IDPEDIDOPRO in (" + filter + ")";
                jsonWhereUnion = " WHERE lin.IDPEDIDOPRO in (" + filter + ")";
            }
            query = "SELECT " + strCampos[0] + " FROM " + jsonFrom + " " + jsonWhere + " UNION SELECT DISTINCT " + strCamposUnion[0] + " FROM " + jsonFromUnion + " " + jsonWhereUnion;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static DataTable WS_ObtencionDevolucionesProveedor()
        {
            string query = "SELECT * FROM TBLDEVOLPROCAB";
            DataTable datos = ConexionSQL.getDataTable(query);
            return datos;
        }

        #endregion RecepcionesProCab

        #region Recepciones

        public static string ObtencionRecepcionesEsquema()
        {
            string json = LoadJson("RecepcionesCab");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionRecepcionesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("RecepcionesCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string ObtencionRecepcionesDatosGridView(string sortExpression, string strCampos, string strRelaciones, string strCamposAlias)
        {
            string json = LoadJson("RecepcionesCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            if (jsonWhere != string.Empty)
            {
                jsonWhere = " WHERE " + jsonWhere;
            }
            else
            {
                jsonWhere = string.Empty;
            }
            //string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RECEP" + strRelaciones + jsonWhere + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion Recepciones

        #region TipoTareas

        public static string ObtencionTipoTareasEsquema()
        {
            string json = LoadJson("TipoTareasColor");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionTipoTareasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("TipoTareasColor");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores
        public static string ObtencionTipoTareasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TipoTareasColor");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static AckResponse CrearZonaLogCab(string descripcion, string tipoZona)
        {
            String insert = "INSERT INTO dbo.TBLZONALOGCAB (DESCRIPCION, TIPOZONA) VALUES('" + descripcion + "', '" + tipoZona + "')";
            string error = "";
            bool ok = ConexionSQL.SQLClienteExec(insert, ref error);

            AckResponse res = ComponerACKO(ok, error);

            return res;
        }

        public static AckResponse CrearTareaTipo(string tipoTarea, string descripcion, int prioridad, int zonaLogAct, int maxNumLin, String tipoMovAsociado, int durSegundos)
        {
            String insert = " INSERT INTO dbo.TBLTAREASTIPO (TIPOTAREA, DESCRIPCION, PRIORIDAD, ZONALOGACTUACION, MAXNUMLINEAS, TIPOMOVASOCIADO, " +
                "DURSEGUNDOS) VALUES ('" + tipoTarea + "','" + descripcion + "', " + prioridad + ", " +
                zonaLogAct + ", " + maxNumLin + ", '''" + tipoMovAsociado + "''', " + durSegundos + ")";
            string error = "";
            bool ok = ConexionSQL.SQLClienteExec(insert, ref error);

            AckResponse res = ComponerACKO(ok, error);

            return res;
        }

        #endregion TipoTareas

        public static DataTable getRumControles(int idFormulario)
        {
            string query = "SELECT * from RUMCONTROLES WHERE IDFORMULARIO=" + idFormulario;
            DataTable dt = ConexionSQL.getDataTable(query);
            return dt;
        }

        public static DataTable getPickingEstado()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TBLPICKINGESTADO";
            dt = ConexionSQL.getDataTable(query);
            return dt;
        }

        public static DataTable getExistenciaEstado()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TBLEXISTENCIASESTADO";
            dt = ConexionSQL.getDataTable(query);
            return dt;
        }

        public static DataTable getExistenciaEstadoPermitidos()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TBLEXISTENCIASESTADO WHERE IDEXISTENCIAESTADO IN (SELECT ESTADOFIN FROM RUMPERMISOSPORESTADO WHERE IDGRUPO=" + User.IdGrupo + " AND CORRECTO='Y')";
            dt = ConexionSQL.getDataTable(query);
            return dt;
        }

        #region Recepciones

        public static string ObtencionRecepcionesAlbaranPedidoEsquema()
        {
            string json = LoadJson("RecepcionesPorAlbaranPedido");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionRecepcionesPendientesEsquema()
        {
            string json = LoadJson("RecepcionesPendientes");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);
                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionRecepcionesPendientesDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RecepcionesPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RECEP) SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static int ObtencionRecepcionesAlbaranPedidoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("RecepcionesPorAlbaranPedido");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string ObtencionRecepcionesAlbaranPedidoDatosGridView(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("RecepcionesPorAlbaranPedido");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static DataTable ObtencionRecepcionesAlbaranGridEntradas(string[] query, string idRecepcion)
        {
            string fullQuery = "(SELECT " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idRecepcion + ") union (SELECT DISTINCT " + query[3] + " FROM " + query[4] +/* " WHERE " + query[5]+idRecepcion+*/")";
            DataTable tabla = ConexionSQL.getDataTable(fullQuery);
            return tabla;
        }

        public static string ObtencionRecepcionesAlbaranGridLineas(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("gridRecepcionesAlbaranLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionRecepcionesAlbaranGridPreavisos()
        {
            string json = LoadJson("gridRecepcionesAlbaranPreavisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ") SELECT * " + " FROM PagingCte";
            //string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            //json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        //public static string ObtencionRecepcionesAlbaranGridTareas()
        //{
        //}

        #endregion Recepciones

        public static string ObtencionAcopiosSalidas(string sortExpression, string[] strCampos)
        {
            string json = LoadJson("AcopiosSalidas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + ")";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        public static string ObtencionDatosGridView(string nombreJson, string sortExpression, string[] strCampos)
        {
            string json = LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string sql = "SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") FROM " + strCampos[1];
            if (strCampos.Length > 2 && !string.IsNullOrEmpty(strCampos[2]))
            {
                sql = sql + " " + strCampos[2];
            }
            string query = ";WITH PagingCte AS (" +/*SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] +*/sql + ") SELECT * " + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionDatos(string nombreJson, string aliasFrom, string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom +
                " " + aliasFrom + " " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static int ObtencionDatosRegistrosFiltrados(string filterExpressions, string nombreJson)
        {
            string json = LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();

            //Cesar comprobamos que el filterExpressions sea vacio o contenga where
            string sentenciaWhere = "";
            if (!filterExpressions.Equals(""))
            {
                if (filterExpressions.Contains("WHERE"))
                {
                    sentenciaWhere = filterExpressions;
                }
                else
                {
                    sentenciaWhere = "WHERE " + filterExpressions;
                }
            }
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + sentenciaWhere;

            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        #region Reservas

        //Tamaño Lotes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla FormatosSCC
        public static string ObtencionReservasEsquema()
        {
            string json = LoadJson("ReservasTemplate");
            try
            {
                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static int ObtencionReservasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ReservasTemplate");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " WHERE " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string ObtencionReservasDatos(string sortExpression, string[] strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ReservasTemplate");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionReservasDatosGridView(string sortExpression, string filterExpression, string[] strCampos)
        {
            string json = LoadJson("ReservasTemplate");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] + " " + filterExpression + ") SELECT * FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        #endregion Reservas

        #region Funciones Auxiliares Genéricas

        public static string ObtencionParametrosControlesEsquema()
        {
            string json = LoadJson("ParametrosControles");

            return json;
        }

        public static DataTable ObtencionControlTareas()
        {
            string query = XmlReaderPropio.getControlTareas();
            DataTable dt = ConexionSQL.getDataTable(query);
            return dt;
        }

        public static string ObtencionMayorElemento(string elemento, string tabla, string filtro)
        {
            if (!filtro.ToLower().Contains("where"))
            {
                filtro = "WHERE" + filtro;
            }
            string selectExpression = "SELECT MAX(" + elemento + ") AS VALOR from " + tabla + " " + filtro;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            return datosJSON;
        }

        //Compone un ACK o NACK en formato JSON
        public static string ComponerACK(bool ok, string error)
        {
            string ack = LoadJson("ack");

            error = error.Replace("\"", "'");

            if (ok)
            {
                ack = ack.Replace("[RESULTADO]", "OK").Replace("[MENSAJE]", "Proceso realizado correctamente");
            }
            else
            {
                ack = ack.Replace("[RESULTADO]", "KO").Replace("[MENSAJE]", error);
            }

            return ack;
        }

        public static AckResponse ComponerACKO(bool ok, string error)
        {
            AckResponse ack = new AckResponse();
            if (ok)
            {
                ack.Resultado = "OK";
                ack.Mensaje = Lenguaje.traduce("Proceso realizado correctamente");
            }
            else
            {
                ack.Resultado = "Error";
                if (error != "")
                {
                    ack.Mensaje = Lenguaje.traduce(error);
                }
            }

            return ack;
        }

        //Genera el comando SQL a partir de datos de dentrada
        private static string GenerarComandoSQL(string tipo, string tabla, Hashtable hshDatos, Hashtable hshClausula = null)
        {
            string sql = string.Empty;

            switch (tipo)
            {
                case "INSERT":

                    sql = "INSERT INTO " + tabla + "(";
                    string columnas = "";
                    string valores = "";

                    foreach (DictionaryEntry de in hshDatos)
                    {
                        columnas += "" + de.Key + ",";
                        valores += "" + de.Value + ",";
                    }

                    columnas = columnas.Remove(columnas.LastIndexOf(','));
                    valores = valores.Remove(valores.LastIndexOf(','));

                    sql += columnas + ") VALUES (" + valores + "); SELECT SCOPE_IDENTITY()";

                    break;

                case "UPDATE":
                    sql = "UPDATE " + tabla + " SET ";
                    if (hshDatos.Count == 0) return null;
                    foreach (DictionaryEntry de in hshDatos)
                    {
                        sql += "" + de.Key + " = " + de.Value + ", ";
                    }
                    sql = sql.Remove(sql.LastIndexOf(','));

                    if (hshClausula.Count > 0)
                    {
                        sql += " WHERE ";
                        int contClausulas = 1;
                        foreach (DictionaryEntry de in hshClausula)
                        {
                            if (contClausulas == hshClausula.Count)
                            {
                                sql += "" + de.Key + " = " + de.Value;
                            }
                            else
                            {
                                sql += "" + de.Key + " = " + de.Value + " AND ";
                            }
                            contClausulas++;
                        }
                    }

                    break;

                case "DELETE":
                    sql = "DELETE FROM " + tabla + " ";
                    if (hshClausula.Count > 0)
                    {
                        sql += "WHERE ";
                        int contClausulas = 1;
                        foreach (DictionaryEntry de in hshClausula)
                        {
                            if (contClausulas == hshClausula.Count)
                            {
                                sql += "" + de.Key + " = " + de.Value;
                            }
                            else
                            {
                                sql += "" + de.Key + " = " + de.Value + " AND ";
                            }
                            contClausulas++;
                        }
                    }
                    break;

                case "INSERT_SELECT":

                    sql = "INSERT INTO " + tabla + "(";
                    string columnas2 = "";
                    string valores2 = "";

                    foreach (DictionaryEntry de in hshDatos)
                    {
                        columnas2 += "" + de.Key + ",";

                        if (de.Value.ToString() == "[CAMPO_TABLA]")
                        {
                            valores2 += "" + de.Key + ",";
                        }
                        else
                        {
                            valores2 += "" + de.Value + ",";
                        }
                    }

                    columnas2 = columnas2.Remove(columnas2.LastIndexOf(','));
                    valores2 = valores2.Remove(valores2.LastIndexOf(','));

                    string filtro = "";
                    if (hshClausula.Count > 0)
                    {
                        filtro += "WHERE ";
                        int contClausulas = 1;
                        foreach (DictionaryEntry de in hshClausula)
                        {
                            if (contClausulas == hshClausula.Count)
                            {
                                filtro += "" + de.Key + " = " + de.Value;
                            }
                            else
                            {
                                filtro += "" + de.Key + " = " + de.Value + " AND ";
                            }
                            contClausulas++;
                        }
                    }

                    sql += columnas2 + ") SELECT " + valores2 + " FROM " + tabla + " " + filtro;

                    break;
            }

            return sql;
        }

        private static string GenerarComandoSQL(string tipo, string tabla, Hashtable hshDatos, string where, Hashtable hshClausula = null)
        {
            string sql = string.Empty;

            switch (tipo)
            {
                case "INSERT":

                    sql = "INSERT INTO " + tabla + "(";
                    string columnas = "";
                    string valores = "";

                    foreach (DictionaryEntry de in hshDatos)
                    {
                        columnas += "" + de.Key + ",";
                        valores += "" + de.Value + ",";
                    }

                    columnas = columnas.Remove(columnas.LastIndexOf(','));
                    valores = valores.Remove(valores.LastIndexOf(','));

                    sql += columnas + ") VALUES (" + valores + "); SELECT SCOPE_IDENTITY()";

                    break;

                case "UPDATE":
                    sql = "UPDATE " + tabla + " SET ";
                    foreach (DictionaryEntry de in hshDatos)
                    {
                        sql += "" + de.Key + " = " + de.Value + ", ";
                    }
                    sql = sql.Remove(sql.LastIndexOf(','));

                    sql += " WHERE " + where;

                    break;

                case "DELETE":
                    sql = "DELETE FROM " + tabla + " ";
                    if (hshClausula.Count > 0)
                    {
                        sql += "WHERE ";
                        int contClausulas = 1;
                        foreach (DictionaryEntry de in hshClausula)
                        {
                            if (contClausulas == hshClausula.Count)
                            {
                                sql += "" + de.Key + " = " + de.Value;
                            }
                            else
                            {
                                sql += "" + de.Key + " = " + de.Value + " AND ";
                            }
                            contClausulas++;
                        }
                    }
                    break;

                case "INSERT_SELECT":

                    sql = "INSERT INTO " + tabla + "(";
                    string columnas2 = "";
                    string valores2 = "";

                    foreach (DictionaryEntry de in hshDatos)
                    {
                        columnas2 += "" + de.Key + ",";

                        if (de.Value.ToString() == "[CAMPO_TABLA]")
                        {
                            valores2 += "" + de.Key + ",";
                        }
                        else
                        {
                            valores2 += "" + de.Value + ",";
                        }
                    }

                    columnas2 = columnas2.Remove(columnas2.LastIndexOf(','));
                    valores2 = valores2.Remove(valores2.LastIndexOf(','));

                    string filtro = "";
                    if (hshClausula.Count > 0)
                    {
                        filtro += "WHERE ";
                        int contClausulas = 1;
                        foreach (DictionaryEntry de in hshClausula)
                        {
                            if (contClausulas == hshClausula.Count)
                            {
                                filtro += "" + de.Key + " = " + de.Value;
                            }
                            else
                            {
                                filtro += "" + de.Key + " = " + de.Value + " AND ";
                            }
                            contClausulas++;
                        }
                    }

                    sql += columnas2 + ") SELECT " + valores2 + " FROM " + tabla + " " + where;

                    break;
            }

            return sql;
        }

        //Carga un JSON desde Archivo de recursos
        public static string LoadJson(string tipo)
        {
            string json = string.Empty;
            log.Debug("Se va a leer el JSON: " + tipo);
            try
            {
                using (StreamReader r = new StreamReader(Persistencia.ConfigPath + "\\JSON\\" + tipo + ".json"))
                {
                    Debug.WriteLine(Persistencia.ConfigPath + "\\" + tipo + ".json");
                    json = r.ReadToEnd();
                }
                if (json != null && !String.IsNullOrEmpty(json))
                {
                    String jsonLower = json.ToLower();
                    if (jsonLower.Contains("\"from\""))
                    {
                        int index = jsonLower.IndexOf("\"from\"");
                        json = json.Remove(index, 6);
                        json = json.Insert(index, "\"FROM\"");
                    }

                    if (jsonLower.Contains("\"where\""))
                    {
                        int index = jsonLower.IndexOf("\"where\"");
                        json = json.Remove(index, 7);
                        json = json.Insert(index, "\"WHERE\"");
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarErrorNuevo(e, e.Message);
            }
            return json;
        }

        private static TableScheme BuscarEsquemaControl(string strNombre, List<TableScheme> _lstEsquemasForm)
        {
            TableScheme esquema = new TableScheme();

            for (int i = 0; i < _lstEsquemasForm.Count; i++)
            {
                if (_lstEsquemasForm[i].Nombre == strNombre)
                {
                    esquema = _lstEsquemasForm[i];
                    break;
                }
            }

            return esquema;
        }

        public static string ObtencionEsquema(string jsonNombre)
        {
            string json = LoadJson(jsonNombre);
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.Equals(column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                        if (dr[column.ColumnName] == null)
                        {
                            pro.SetValue(obj, null, null);
                        }
                        else
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static DataTable GetSeleccion(String query)
        {
            DataTable dt = ConexionSQL.getDataTable(query);

            return dt;
        }

        public static string ObtencionOrdenesCargaPedidosResumen(string[] strCampos, string valor)
        {
            string json = LoadJson("OrdenesCargaPedidosResumen");
            JArray js = JArray.Parse(json);
            if (strCampos[3] != string.Empty)
            {
                strCampos[3] = " group by " + strCampos[3];
            }
            string query = "SELECT DISTINCT " + strCampos[0] + " FROM " + strCampos[1] + " where " + strCampos[2] + valor + strCampos[3];
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        #endregion Funciones Auxiliares Genéricas

        #endregion Funciones Públicas

        #region Zona Log Cab

        public static string ObtencionZonaLogCabEsquema()
        {
            string json = LoadJson("ZonaLogicaCab");

            try
            {
                string relaciones = string.Empty;
                string aliasTabla = "ZONACAB";

                JArray js = JArray.Parse(json);
                string jsonFrom = js.First()["FROM"].ToString();
                string jsonEsquema = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);

                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        relaciones += " " + oaAux + " ";
                    }
                }
                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " ZONACAB " + relaciones;
                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)
            {
                log.Error("Error Obteniendo el esquema|| " + e.Message + "||" + e.StackTrace);
                json = "";
                return json;
            }
        }

        public static string ObtencionZonaLogCabDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaLogicaCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZONACAB" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static int ObtencionZonaLogCabRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ZonaLogicaCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string ObtencionZonaLogCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaLogicaCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZONACAB" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string AltaZonaCab(string json, bool altaLineas)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ZonaLogicaCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            object scopeIdentity = null;
            bool ok = ConexionSQL.SQLClienteExecScopeIdentity(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error, ref scopeIdentity);

            if (altaLineas && ok && scopeIdentity != null)
            {
                //Crear líneas de pedido
                string jsonGridEsquema = jo.First()["DataGridLin"].ToString();
                if (!string.IsNullOrEmpty(jsonGridEsquema))
                {
                    //GridScheme esquemaGrid = JsonConvert.DeserializeObject<GridScheme>(jsonGridEsquema);
                    GridScheme _esquemaGrid = JsonConvert.DeserializeObject<List<GridScheme>>(jsonGridEsquema)[0];

                    //string a = "";
                    string jsonLineas = LoadJson(_esquemaGrid.ArchivoJSON);
                    JArray joLineas = JArray.Parse(jsonLineas);
                    List<TableScheme> lstEsquemasLineas = JsonConvert.DeserializeObject<List<TableScheme>>(joLineas.First()["Scheme"].ToString());

                    Hashtable hshColumnasLineas = new Hashtable();
                    Hashtable hshClausulaLineas = new Hashtable();

                    for (int i = 0; i < lstEsquemasLineas.Count; i++)
                    {
                        if (lstEsquemasLineas[i].Nombre == _esquemaGrid.Filtro)
                        {
                            hshColumnasLineas.Add(lstEsquemasLineas[i].Nombre, scopeIdentity);
                        }
                        else
                        {
                            hshColumnasLineas.Add(lstEsquemasLineas[i].Nombre, "[CAMPO_TABLA]");
                        }
                    }

                    if (!string.IsNullOrEmpty(_esquemaGrid.Filtro))
                    {
                        hshClausulaLineas.Add(_esquemaGrid.Filtro, _esquemaGrid.ValorFiltro);
                    }
                    ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT_SELECT", jsonFrom, hshColumnasLineas, hshClausulaLineas), ref error);
                }
            }

            return ComponerACK(ok, error);
        }

        //Editar un ZonaCab
        public static string EditarZonaCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ZonaLogicaCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un ZonaCab
        public static string EliminarZonaCab(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ZonaLogicaCab");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion Zona Log Cab

        #region ZonaLogLin

        //Tamaño ClientePedidoLineas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ClientePedidoLineas
        public static string ObtencionZonaLogLineasEsquema(string _filtro)
        {
            string json = LoadJson("ZonaLogicaLin");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + _filtro;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Datos ClientePedidoLineas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ClientePedidoLineas
        public static string ObtencionZonaLogLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaLogicaLin");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZONALIN" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string ObtencionZonaLogLineasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaLogicaLin");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZONALIN" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ClientePedidoLineas
        public static string AltaZonaLogLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    //if (!tesquema.EsPK)
                    //{
                    if (string.IsNullOrEmpty(_valor))
                    {
                        _valor = "NULL";
                    }
                    hshDatos.Add(_nombre, _valor);
                    //}
                }
            }
            string jsEntero = LoadJson("ZonaLogicaLin");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un ClientePedidoLineas
        public static string EditarZonaLogLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();
            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());

            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (!tesquema.EsPK)
                    {
                        if (string.IsNullOrEmpty(_valor))
                        {
                            _valor = "NULL";
                        }
                        hshDatos.Add(_nombre, _valor);
                    }
                    else
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ZonaLogicaLin");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar ClientePedidoLineas
        public static string EliminarZonaLogLineas(string json)
        {
            string error = string.Empty;
            JArray jo = JArray.Parse(json);

            Hashtable hshDatos = new Hashtable();
            Hashtable hshClausula = new Hashtable();

            List<TableScheme> _lstEsquemasForm = JsonConvert.DeserializeObject<List<TableScheme>>(jo.First()["Scheme"].ToString());
            JArray array = JArray.Parse(jo.First()["Data"].ToString());

            foreach (JObject content in array.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    string _nombre = prop.Name;
                    string _valor = prop.Value.ToString();

                    TableScheme tesquema = BuscarEsquemaControl(_nombre, _lstEsquemasForm);

                    if (tesquema.EsPK)
                    {
                        hshClausula.Add(_nombre, _valor);
                    }
                }
            }
            string jsEntero = LoadJson("ZonaLogicaLin");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["FROM"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        public static string ObtencionEsquemaGenerico(string jsonNombre)
        {
            string json = LoadJson(jsonNombre);

            try

            {
                JArray js = JArray.Parse(json);

                string jsonFrom = js.First()["FROM"].ToString();

                string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;

                string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

                json = json.Replace("\"[DATA]\"", datosJSON);

                return json;
            }
            catch (Exception e)

            {
                log.Error("Error Obteniendo el esquema jsonNombre|| " + e.Message + "||" + e.StackTrace);

                json = "";

                return json;
            }
        }

        #endregion ZonaLogLin

        #region Sentencias SQL Generales

        public static DataTable getDataTableSQL(String[] campos, string from, string filtroWhere)
        {
            try
            {
                String select = "SELECT ";
                DataTable t = new DataTable();

                if (campos is null || campos.Length == 0)
                {
                    log.Error("Error en getDataTable, string 'CAMPOS' vale null o 0");
                    return null;
                }
                if (from is null || from == String.Empty)
                {
                    log.Error("Error en getDataTable, string 'FROM' es vacio o null");
                    return null;
                };

                for (int i = 0; i < campos.Length; i++)
                {
                    if (i == campos.Length - 1)
                    {
                        select = select + campos[i];
                    }
                    else
                    {
                        select = select + campos[i] + ",";
                    }
                }
                select = select + " FROM " + from;

                if (!String.IsNullOrEmpty(filtroWhere))
                {
                    select = select + " WHERE " + filtroWhere;
                }
                return ConexionSQL.getDataTable(select);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            return null;
        }

        public static string GetParametrosTodosSQL = "SELECT * FROM RUMPARAMETROS";

        #endregion Sentencias SQL Generales

        #region Sentencias Sql AltaProducto

        public static string GetAltaProductoDefectoSQL = "SELECT OFC.*,AR.ESTADOENTRADA,OFC.IDARTICULO," +
            "AR.DESCRIPCION,AR.LOTEESTRICTO,AR.CANTIDAD,AR.IDPALETTIPO," +
            "AR.IDUNIDADTIPO,AR.TIPOPRESENTACION,AR.IDPRESENTACION,OFC.IDMAQUINA, M.MUELLE, AR.CANTIDADUNIDAD,AR.DIASCADUCIDAD " +
            " FROM TBLORDENFABRICACIONCAB OFC JOIN TBLARTICULOS AR ON OFC.IDARTICULO = AR.IDARTICULO JOIN TBLMAQUINAS M " +
            "ON OFC.IDMAQUINA = M.IDMAQUINA ";

        public static string GetAltaProductoPedidoProDefectoSQL = "SELECT PL.* ,AR.ESTADOENTRADA," +
            "AR.DESCRIPCION,AR.LOTEESTRICTO,AR.CANTIDAD AS CANTIDADARTICULO,AR.IDPALETTIPO," +
            "AR.IDUNIDADTIPO AS IDUNIDADTIPOARTICULO,AR.TIPOPRESENTACION,AR.IDPRESENTACION,AR.CANTIDADUNIDAD AS CANTIDADUNIDADARTICULO,AR.CAPTURANSERIE,AR.DIASCADUCIDAD,AR.REGISTRARPESO " +
                " FROM TBLPEDIDOSPROLIN PL JOIN TBLARTICULOS AR ON PL.IDARTICULO = AR.IDARTICULO";

        /*"SELECT PL.* ,AR.ESTADOENTRADA," +
            "AR.DESCRIPCION,AR.LOTEESTRICTO,AR.CANTIDAD,AR.IDPALETTIPO," +
            "AR.IDUNIDADTIPO,AR.TIPOPRESENTACION,AR.IDPRESENTACION,AR.CANTIDADUNIDAD,AR.CAPTURANSERIE,AR.DIASCADUCIDAD " +
            " FROM TBLPEDIDOSPROLIN PL JOIN TBLARTICULOS AR ON PL.IDARTICULO = AR.IDARTICULO ";*/

        public static string GetAltaArticuloDefectoSQL = "SELECT IDARTICULO," +
            "REFERENCIA,ATRIBUTO,DESCRIPCION,LOTEESTRICTO,CANTIDAD AS CANTIDADARTICULO,IDPALETTIPO," +
            "IDUNIDADTIPO AS IDUNIDADTIPOARTICULO,TIPOPRESENTACION,IDPRESENTACION,CANTIDADUNIDAD AS CANTIDADUNIDADARTICULO,ESTADOENTRADA,CAPTURANSERIE,DIASCADUCIDAD,REGISTRARPESO FROM DBO.TBLARTICULOS";

        public static string GetAltaProductoDevolCliDefectoSQL = "SELECT PL.IDDEVOLCLI, PL.IDDEVOLCLILIN ,AR.ESTADOENTRADA,PL.IDARTICULO," +
           "AR.REFERENCIA,AR.ATRIBUTO,AR.DESCRIPCION,AR.LOTEESTRICTO,AR.CANTIDAD AS CANTIDADARTICULO ,AR.IDPALETTIPO," +
           "AR.IDUNIDADTIPO as IDUNIDADTIPOARTICULO ,AR.TIPOPRESENTACION,AR.IDPRESENTACION,AR.CANTIDADUNIDAD AS CANTIDADUNIDADARTICULO,AR.CAPTURANSERIE,AR.DIASCADUCIDAD " +
           " FROM TBLDEVOLCLILIN PL JOIN TBLARTICULOS AR ON PL.IDARTICULO = AR.IDARTICULO ";

        public static string GetIdDescripcionUnidadesTipoPresentacionSQL = "SELECT UN.IDUNIDADTIPO,UN.DESCRIPCION "
                        + " FROM TBLUNIDADESTIPO UN "
                        + " INNER JOIN TBLPRESENTACIONESLIN PL ON PL.IDUNIDADTIPODESTINO = UN.IDUNIDADTIPO";

        public static string GetIdDescripcionUnidadesTipoSQL = "SELECT IDUNIDADTIPO,DESCRIPCION FROM TBLUNIDADESTIPO ";
        public static string GetIdDescripcionMotivosComentarioSQL = "SELECT IDMOTIVOCOMENTARIO,DESCRIPCION FROM TBLMOTIVOSCOMENTARIO WHERE TIPOPADRE ='' ";

        public static string GetIdDescripcionTipoPaletSQL = "SELECT IDPALETTIPO,DESCRIPCION FROM TBLPALETSTIPO ";

        public static string GetIdDescripcionMaquinasSQL = "SELECT IDMAQUINA,DESCRIPCION FROM TBLMAQUINAS ";

        public static string GetIdDescripcionOperariosSQL = "SELECT IDOPERARIO,NOMBRE FROM TBLOPERARIOS ";
        public static string GetIdDescripcionMotivosTipoSQL = "SELECT IDMOTIVO,DESCRIPCION FROM dbo.TBLMOTIVOSREG ";

        public static string GetIdDescripcionFabricacionEstadosSQL = "select IDEXISTENCIAESTADO,DESCRIPCION from TBLEXISTENCIASESTADO ";

        public static string GetIdDescripcionCarrosMovilSQL = "select IDHUECO,DESCRIPCION from tblhuecos where idhuecoestante='CM' ";

        public static string GetIdReferenciaDescripcionAtributoArticulosGenericosSQL = "SELECT IDARTICULO, REFERENCIA, DESCRIPCION,ATRIBUTO FROM TBLARTICULOS WHERE TIPOARTICULO='GE' order by referencia";
        public static string GetIdReferenciaDescripcionAtributoArticulosSQL = "SELECT IDARTICULO, REFERENCIA, DESCRIPCION,ATRIBUTO FROM TBLARTICULOS WHERE TIPOARTICULO<>'GE' or TIPOARTICULO IS null OR TIPOARTICULO ='' order by referencia";

        public static string GetIdDescripcionHuecoSQL = "select IDHUECO,DESCRIPCION from TBLHUECOS";
        public static string GetIdDescripcionHuecoMuelleSQL = "select IDHUECO,DESCRIPCION from TBLHUECOS WHERE IDHUECOESTANTE='MU'";

        public static string GetDatatablePendienteOrdenRecogida(int idOrdenrecogida)
        {
            return "SELECT* FROM dbo.FNGETPENDIENTERESERVASALIDAORDEN(" + idOrdenrecogida + ")";
        }

        #endregion Sentencias Sql AltaProducto

        #region Sentencias Artículos

        public static string GetArticulosData = "SELECT Tipo, Descripcion FROM TBLTIPOFORMA ";
        private static string queryRelacionGrupo;
        private static string queryRelacion;
        private static string queryUsuario;

        #endregion Sentencias Artículos

        #region Sentencias Historicos

        public static string getExistenciaHistorico = "";
        public static string getReservasHistorico = "";
        public static string getLogsHistorico = "";

        #endregion Sentencias Historicos

        #region Sentencias inventario

        public static string GetIncidenciasPrimeraPasada(int idhuecoalmacen, int aceradesde,
            int acerahasta, int portaldesde, int portalhasta, int pisodesde,
            int pisohasta)
        {
            return "select a.REFERENCIA as Referencia,A.ATRIBUTO as Atributo,a.DESCRIPCION as Descripcion,h.descripcion as Ubicacion,"
                + " inv.IDENTRADA AS Matrícula,inv.IdIncidencia,inv.CANTIDADORD AS 'Cantidad Ord', inv.CANTIDADOP1 AS 'Cantidad Op 1' "
                + " , inv.CANTIDADOP2 AS  'Cantidad Op 2', inv.CANTUNIDADORD AS 'Cantidad Unidad Ord',inv.CANTUNIDADOP1 as  'Cantidad Unidad  Op 1', "
                + "  inv.CANTUNIDADOP2 as  'Cantidad Unidad  Op 2' , inv.CANTIDADRETORD AS 'Cantidad Ret Ord', inv.CANTIDADRETOP1 AS 'Cantidad Ret Op 1'"
                + " , inv.CANTIDADRETOP2 AS 'Cantidad Ret Op 2' "
                + " , inv.FECHA AS Fecha, inv.TIPOINVENTARIO AS 'Tipo Inventario',inv.ESTADO AS Estado, inv.IDINCIDENCIA AS Id "
                + " from dbo.tblincidenciasinventario inv "
                + " join dbo.tblhuecos h on h.idhueco=inv.idhueco "
                + " left join dbo.tblentradas e on e.identrada=inv.identrada "
                + " left join dbo.tblarticulos a on a.idarticulo = e.idarticulo "
                + " where h.idhuecoalmacen=" + idhuecoalmacen
                + " and (h.acera>=" + aceradesde + " and h.acera<=" + acerahasta
                + ") " + " and (h.portal>=" + portaldesde + " and h.portal<="
                + portalhasta + ") " + " and (h.piso>=" + pisodesde
                + " and h.piso<=" + pisohasta
                + ") and (inv.cantidadop2 is null)";
        }

        #endregion Sentencias inventario

        public static void abrirManual(int rummantenimiento)
        {
            try
            {
                string sql = "select LINKMANUAL from RUMMANTENIMIENTO where IDMANTENIMIENTO=" + rummantenimiento;
                DataTable respuesta = ConexionSQL.getDataTable(sql);
                if (respuesta.Rows.Count == 0 || respuesta.Rows[0][0] == System.DBNull.Value)
                {
                    MessageBox.Show("NO SE ENCUENTRA RUTA DE MANUAL");
                }
                else
                {
                    // MessageBox.Show("Se procede abrir el manual en el navegador");
                    Process.Start(respuesta.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en el manual" + ex);
            }
        }

        public static object getValorDefectoCampoTabla(string tabla, string campo)
        {
            object valor = null;
            try
            {
                string query = "SELECT * "
                + " FROM INFORMATION_SCHEMA.Columns "
                + " WHERE TABLE_NAME = '" + tabla + "' AND COLUMN_NAME='" + campo + "'";
                DataTable dt = ConexionSQL.getDataTable(query);

                if (dt.Rows.Count > 0)
                {
                    if (!dt.Rows[0]["COLUMN_DEFAULT"].ToString().Equals(""))
                    {
                        String valorDefecto = dt.Rows[0]["COLUMN_DEFAULT"].ToString();

                        DataTable dt1 = ConexionSQL.getDataTable("SELECT " + valorDefecto);
                        if (dt1.Rows.Count > 0)
                        {
                            valor = dt1.Rows[0][0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }

            return valor;
        }
    }
}
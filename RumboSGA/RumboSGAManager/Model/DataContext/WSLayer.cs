using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


//NOTA: Estas clases deberían ser modificadas, en un futuro, por conexiones a WS
namespace RumboSGAManager.Model.DataContext
{
    //CLASE PARA DE OBTENCION DE DATOS MEDIANTE CONEXION DIRECTA SQL
    public static class WSLayer
    {
        #region Funciones Públicas

        #region Proveedores

        //Tamaño Proveedores. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string WS_ObtencionProveedoresEsquema()
        {
            string json = LoadJson("Proveedores");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionProveedoresRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Proveedores");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados=ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores        
        public static string WS_ObtencionProveedoresDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Proveedores");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            //intermedia.Query = query + " ORDER BY " + sortExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionProveedoresDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Proveedores");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Proveedor
        public static string WS_AltaProveedor(string json)
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
            string jsEntero = LoadJson("Agencias");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string WS_EditarProveedor(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string WS_EliminarProveedor(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion
        #region Clientes

        //Tamaño Clientes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string WS_ObtencionClientesEsquema()
        {
            string json = LoadJson("Clientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionClientesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Clientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Clientes. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores        
        public static string WS_ObtencionClientesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Clientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLI" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            //intermedia.Query = query + " ORDER BY " + sortExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionClientesDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Clientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLI" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Cliente
        public static string WS_AltaCliente(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Cliente
        public static string WS_EditarCliente(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Cliente
        public static string WS_EliminarCliente(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Clientes Pedidos Cab

        //Tamaño ClientesPedidosCab. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ClientesPedidosCab
        public static string WS_ObtencionClientesPedidosCabEsquema()
        {
            string json = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionClientesPedidosCabRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos ClientesPedidosCab. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ClientesPedidosCab        
        public static string WS_ObtencionClientesPedidosCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLICAB" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static string WS_ObtencionClientesPedidosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ClientesPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLICAB" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ClientesPedidosCab
        public static string WS_AltaClientePedidoCab(string json, bool altaLineas)
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
            string jsonFrom = js.First()["From"].ToString();
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
        public static string WS_EditarClientePedidoCab(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un ClientesPedidosCab
        public static string WS_EliminarClientePedidoCab(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion
        #region Clientes Pedidos Lineas

        //Tamaño ClientePedidoLineas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ClientePedidoLineas
        public static string WS_ObtencionClientesPedidosLineasEsquema(string _filtro)
        {
            string json = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + _filtro;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Datos ClientePedidoLineas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ClientePedidoLineas        
        public static string WS_ObtencionClientesPedidosLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLILIN" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionClientesPedidosLineasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ClientesPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " CLILIN" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ClientePedidoLineas
        public static string WS_AltaClientePedidoLineas(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un ClientePedidoLineas
        public static string WS_EditarClientePedidoLineas(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar ClientePedidoLineas
        public static string WS_EliminarClientePedidoLineas(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion
        #region Proveedores Pedidos Cab

        //Tamaño Proveedores. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string WS_ObtencionProveedoresPedidosCabEsquema()
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionProveedoresPedidosCabRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores        
        public static string WS_ObtencionProveedoresPedidosCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROCAB" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            SentenciasSQL.queryPedidosProCab = query + " ORDER BY " + sortExpression;
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }


        //Crear un Proveedor
        public static string WS_AltaProveedorPedidoCab(string json, bool altaLineas)
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
            string jsonFrom = js.First()["From"].ToString();
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
        public static string WS_EditarProveedorPedidoCab(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string WS_EliminarProveedorPedidoCab(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion
        #region Proveedores Pedidos Lineas

        //Tamaño Proveedores. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Proveedores
        public static string WS_ObtencionProveedoresPedidosLineasEsquema(string _filtro)
        {
            string json = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + _filtro;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores        
        public static string WS_ObtencionProveedoresPedidosLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            //string fields = strCampos;
            string selectExpression = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROLIN" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static string WS_ObtencionProveedoresPedidosLineasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ProveedoresPedidosLineas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROLIN" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Proveedor
        public static string WS_AltaProveedorPedidoLineas(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string WS_EditarProveedorPedidoLineas(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string WS_EliminarProveedorPedidoLineas(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        #endregion
        #region ReservaHistorico
        //Tamaño ReservaHistorico. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ReservaHistorico
        public static string WS_ObtencionReservasHistoricoEsquema()
        {
            string json = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionReservasHistoricoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }


        //Datos ReservaHistorico. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ReservaHistorico        
        public static string WS_ObtencionReservasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            //string fields = strCampos;
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RESHIST" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionReservasHistoricoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ReservaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RESHIST" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ReservaHistorico
        public static string WS_AltaReservasHistorico(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Proveedor
        public static string WS_EditarReservasHistorico(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Proveedor
        public static string WS_EliminarReservasHistorico(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Maquina

        //Tamaño Maquina. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Maquinas
        public static string WS_ObtencionMaquinasEsquema()
        {
            string json = LoadJson("Maquinas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionMaquinasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Maquinas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Maquinas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Maquinas        
        public static string WS_ObtencionMaquinaDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Maquinas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MAQ" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionMaquinasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Maquinas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MAQ" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Maquina
        public static string WS_AltaMaquina(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Maquina
        public static string WS_EditarMaquina(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Maquina
        public static string WS_EliminarMaquina(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Familias
        //Tamaño Familia. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Familias
        public static string WS_ObtencionFamiliasEsquema()
        {
            string json = LoadJson("Familias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionFamiliasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Familias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + "" + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Familias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Familias        
        public static string WS_ObtencionFamiliasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Familias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FAM" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionFamiliasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Familias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FAM" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Familia
        public static string WS_AltaFamilia(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Familia
        public static string WS_EditarFamilia(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Familia
        public static string WS_EliminarFamilia(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Agencias
        //Tamaño Agencias. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Agencias
        public static string WS_ObtencionAgenciasEsquema()
        {
            string json = LoadJson("Agencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionAgenciasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Agencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Agencias. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Agencias        
        public static string WS_ObtencionAgenciasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Agencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionAgenciasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Agencias");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " AGEN " + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Agencia
        public static string WS_AltaAgencia(string json)
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
            string jsonFrom = js.First()["From"].ToString();
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Agencia
        public static string WS_EliminarAgencia(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Bom
        //Tamaño Bom. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Bom
        public static string WS_ObtencionBomEsquema()
        {
            string json = LoadJson("Bom");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionBomRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Bom");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + "" + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Bom. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Bom        
        public static string WS_ObtencionBomDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Bom");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " BOM" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionBomDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Bom");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " BOM" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Bom
        public static string WS_AltaBom(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Bom
        public static string WS_EditarBom(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Bom
        public static string WS_EliminarBom(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region ZonaIntercambio
        //Tamaño ZonaIntercambio. Devuelve el esquema de datos y la cantidad de registros totales de la tabla ZonaIntercambio
        public static string WS_ObtencionZonaIntercambioEsquema()
        {
            string json = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionZonaIntercambioRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos ZonaIntercambio. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ZonaIntercambio        
        public static string WS_ObtencionZonaIntercambioDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZON" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionZonaIntercambioDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ZonaIntercambio");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " ZON" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un ZonaIntercambio
        public static string WS_AltaZonaIntercambio(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un ZonaIntercambio
        public static string WS_EditarZonaIntercambio(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un ZonaIntercambio
        public static string WS_EliminarZonaIntercambio(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region MotivosReg
        //Tamaño MotivosReg. Devuelve el esquema de datos y la cantidad de registros totales de la tabla MotivosReg
        public static string WS_ObtencionMotivosRegEsquema()
        {
            string json = LoadJson("MotivosReg");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionMotivosRegRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("MotivosReg");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos MotivosReg. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla MotivosReg        
        public static string WS_ObtencionMotivosRegDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("MotivosReg");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MOTREG" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static string WS_ObtencionMotivosRegDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("MotivosReg");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MOTREG" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un MotivosReg
        public static string WS_AltaMotivosReg(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un MotivosReg
        public static string WS_EditarMotivosReg(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un MotivosReg
        public static string WS_EliminarMotivosReg(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Rutas
        //Tamaño Rutas. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Rutas
        public static string WS_ObtencionRutasEsquema()
        {
            string json = LoadJson("Rutas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionRutasRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Rutas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Rutas. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Rutas        
        public static string WS_ObtencionRutasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Rutas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTAS" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionRutasDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Rutas");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTAS" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una Ruta
        public static string WS_AltaRuta(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una Ruta
        public static string WS_EditarRuta(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una Ruta
        public static string WS_EliminarRuta(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region RutasPreparacion
        //Tamaño RutasPreparacion. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RutasPreparacion
        public static string WS_ObtencionRutasPreparacionEsquema()
        {
            string json = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionRutasPreparacionRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos RutasPreparacion. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla RutasPreparacion
        public static string WS_ObtencionRutasPreparacionDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTASPREP" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionRutasPreparacionDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " RUTASPREP" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una RutaPreparacion
        public static string WS_AltaRutaPreparacion(string json)
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
            string jsEntero = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una RutaPreparacion
        public static string WS_EditarRutaPreparacion(string json)
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
            string jsEntero = LoadJson("RutasPreparacion");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una RutaPreparacion
        public static string WS_EliminarRutaPreparacion(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region IncidenciasInventario
        //Tamaño IncidenciasInventario. Devuelve el esquema de datos y la cantidad de registros totales de la tabla IncidenciasInventario
        public static string WS_ObtencionIncidenciasInventarioEsquema()
        {
            string json = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionIncidenciasInventarioRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos IncidenciasInventario. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla IncidenciasInventario
        public static string WS_ObtencionIncidenciasInventarioDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " INCIDENCIASINV" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            SentenciasSQL.queryIncidenciasInventario = query + " ORDER BY " + sortExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionIncidenciasInventarioDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("IncidenciasInventario");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " INCIDENCIASINV" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una IncidenciaInventario
        public static string WS_AltaIncidenciaInventario(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una RutaPreparacion
        public static string WS_EditarIncidenciaInventario(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una RutaPreparacion
        public static string WS_EliminarIncidenciaInventario(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region TareasPendientes
        //Tamaño TareasPendientes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla TareasPendientes
        public static string WS_ObtencionTareasPendientesEsquema()
        {
            string json = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionTareasPendientesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos TareasPendientes. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla TareasPendientes
        public static string WS_ObtencionTareasPendientesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASPEND" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionTareasPendientesDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasPendientes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASPEND" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una TareaPendientes
        public static string WS_AltaTareaPendiente(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una TareaPendientes
        public static string WS_EditarTareaPendiente(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una TareaPendientes
        public static string WS_EliminarTareaPendiente(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region TareasTipo
        //Tamaño TareasTipo. Devuelve el esquema de datos y la cantidad de registros totales de la tabla TareasTipo
        public static string WS_ObtencionTareasTipoEsquema()
        {
            string json = LoadJson("TareasTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionTareasTipoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("TareasTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos TareasTipo. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla TareasTipo
        public static string WS_ObtencionTareasTipoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASTIPO" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static string WS_ObtencionTareasTipoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("TareasTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " TAREASTIPO" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una TareaTipo
        public static string WS_AltaTareaTipo(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una TareaTipo
        public static string WS_EditarTareaTipo(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una TareaTipo
        public static string WS_EliminarTareaTipo(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region CombiPalets
        //Tamaño CombiPalets. Devuelve el esquema de datos y la cantidad de registros totales de la tabla CombiPalets
        public static string WS_ObtencionCombiPaletsEsquema()
        {
            string json = LoadJson("CombiPalets");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionCombiPaletsRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("CombiPalets");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString(); string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos CombiPalets. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla CombiPalets
        public static string WS_ObtencionCombiPaletsDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("CombiPalets");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " COMBIPALETS" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionCombiPaletsDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("CombiPalets");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " COMBIPALETS" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un CombiPalet
        public static string WS_AltaCombiPalet(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un CombiPalet
        public static string WS_EditarCombiPalet(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un CombiPalet
        public static string WS_EliminarCombiPalet(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region PaletsTipo
        //Tamaño PaletsTipo. Devuelve el esquema de datos y la cantidad de registros totales de la tabla PaletsTipo
        public static string WS_ObtencionPaletsTipoEsquema()
        {
            string json = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionPaletsTipoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos PaletsTipo. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla PaletsTipo
        public static string WS_ObtencionPaletsTipoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PALETSTIPO" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionPaletsTipoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("PaletsTipo");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PALETSTIPO" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un PaletTipo
        public static string WS_AltaPaletTipo(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un PaletTipo
        public static string WS_EditarPaletTipo(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un PaletTipo
        public static string WS_EliminarPaletTipo(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region FormatosSCC
        //Tamaño FormatosSCC. Devuelve el esquema de datos y la cantidad de registros totales de la tabla FormatosSCC
        public static string WS_ObtencionFormatoSSCCEsquema()
        {
            string json = LoadJson("FormatosSCC");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionFormatoSSCCRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("FormatosSCC");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos FormatosSCC. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla FormatosSSCC
        public static string WS_ObtencionFormatoSSCCDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("FormatoSSCC");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FORMATOSSCC" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionFormatoSSCCDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("FormatoSSCC");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " FORMATOSSCC" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un FormatoSCC
        public static string WS_AltaFormatoSCC(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un FormatoSCC
        public static string WS_EditarFormatoSCC(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un FormatoSCC
        public static string WS_EliminarFormatoSCC(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Lotes
        //Tamaño Lotes. Devuelve el esquema de datos y la cantidad de registros totales de la tabla FormatosSCC
        public static string WS_ObtencionLotesEsquema()
        {
            string json = LoadJson("Lotes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionLotesRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Lotes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Lotes. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla FormatosSSCC
        public static string WS_ObtencionLotesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Lotes");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
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
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " LOT" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Lote
        public static string WS_AltaLote(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Lote
        public static string WS_EditarLote(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Lote
        public static string WS_EliminarLote(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Propietarios
        //Tamaño Propietarios. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Propietarios
        public static string WS_ObtencionPropietariosEsquema()
        {
            string json = LoadJson("Propietarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionPropietariosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Propietarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Propietarios. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Propietarios
        public static string WS_ObtencionPropietariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Propietarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROP" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionPropietariosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Propietarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROP" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Propietario
        public static string WS_AltaPropietario(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Propietario
        public static string WS_EditarPropietario(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Propietario
        public static string WS_EliminarPropietario(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Mantenimiento
        //Tamaño Mantenimiento. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RUMMANTENIMIENTO
        public static string WS_ObtencionMantenimientoEsquema()
        {
            string json = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionMantenimientoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Mantenimiento. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Mantenimiento
        public static string WS_ObtencionMantenimientoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MANT" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionMantenimientoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Mantenimiento");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " MANT" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Mantenimiento
        public static string WS_AltaMantenimiento(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Mantenimiento
        public static string WS_EditarMantenimiento(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Mantenimiento
        public static string WS_EliminarMantenimiento(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Parametros
        //Tamaño Parametros. Devuelve el esquema de datos y la cantidad de registros totales de la tabla RUMPARAMETROS
        public static string WS_ObtencionParametrosEsquema()
        {
            string json = LoadJson("Parametros");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionParametrosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Parametros");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Parametros. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Mantenimiento
        public static string WS_ObtencionParametrosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Parametros");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PARAM" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionParametrosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Parametros");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PARAM" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Parametro
        public static string WS_AltaParametro(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Parametro
        public static string WS_EditarParametro(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Parametro
        public static string WS_EliminarParametro(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Usuarios
        //Tamaño Usuarios. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Usuarios
        public static string WS_ObtencionUsuariosEsquema()
        {
            string json = LoadJson("Usuarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionUsuariosRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Usuarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }
        //Datos Usuarios. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Usuarios
        public static string WS_ObtencionUsuariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Usuarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USU" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionUsuariosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Usuarios");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USU" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un Usuario
        public static string WS_AltaUsuario(string json)
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
            string jsEntero = LoadJson("Usuarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Usuario
        public static string WS_EditarUsuario(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Usuario
        public static string WS_EliminarUsuario(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region UsuariosGrupos
        //Tamaño Usuarios. Devuelve el esquema de datos y la cantidad de registros totales de la tabla Usuarios
        public static string WS_ObtencionUsuariosGruposEsquema()
        {
            string json = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionUsuariosGruposRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos Usuarios. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Usuarios
        public static string WS_ObtencionUsuariosGruposDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USUGRUP" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionUsuariosGruposDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("UsuariosGrupos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " USUGRUP" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear un UsuarioGrupo
        public static string WS_AltaUsuarioGrupo(string json)
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
            string jsonFrom = js.First()["From"].ToString();

            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un UsuarioGrupo
        public static string WS_EditarUsuarioGrupo(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un UsuarioGrupo
        public static string WS_EliminarUsuarioGrupo(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region ExistenciasHistorico
        //Tamaño ExistenciasHistorico. Devuelve el esquema de datos y la cantidad de registros totales de la tabla IncidenciasInventario
        public static string WS_ObtencionExistenciasHistoricoEsquema()
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static int WS_ObtencionExistenciasHistoricoRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ExistenciaHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        //Datos ExistenciasHistorico. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla ExistenciasHistorico
        public static string WS_ObtencionExistenciasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST " + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string registrosFiltrados = query + selectExpression;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static string WS_ObtencionExistenciasHistoricoDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("ExistenciasHistorico");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " EXISTHIST" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        //Crear una ExistenciaHistorico
        public static string WS_AltaExistenciasHistorico(string json)
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
            string jsEntero = LoadJson("Usuarios");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar una ExistenciasHistorico
        public static string WS_EditarExistenciasHistorico(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar una RutaPreparacion
        public static string WS_EliminarExistenciasHistorico(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region Permisos
        public static string WS_ObtencionPermisosEsquema()
        {
            string json = LoadJson("Permisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static string WS_ObtencionPermisosDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Permisos");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PERM" + strRelaciones + " " + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        public static string WS_AltaPermiso(string json)
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
            string jsEntero = LoadJson("Permisos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        //Editar un Permiso
        public static string WS_EditarPermiso(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }

        //Eliminar un Permiso
        public static string WS_EliminarPermiso(string json)
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
            string jsEntero = LoadJson("Permisos");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion

        #region PedidoProveedor 
        public static string WS_ObtencionPedidoProveedoresEsquema()

        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;

        }

        public static int WS_ObtencionPedidoProveedoresRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;
            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;

        }

        //Datos Proveedores. Devuelve un número determinado de registros (marcados por la página inicial y final), filtrados y ordenados, junto con su esquema, de la tabla Proveedores         
        public static string WS_ObtencionPedidoProveedoresDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {

            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            //string fields = strCampos; 
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;


            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }
        public static string WS_ObtencionPedidoProveedoresDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)
        {


            string json = LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " PROV" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;

        }

        //Crear un Proveedor 

        public static string WS_AltaPedidoProveedor(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);

            return ComponerACK(ok, error);
        }
        //Editar un Proveedor 
        public static string WS_EditarPedidoProveedor(string json)

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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);




        }
        //Eliminar un Proveedor 

        public static string WS_EliminarPedidoProveedor(string json)
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
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }
        #endregion
        #region RecepcionesProCab 
        public static string WS_ObtencionRecepcionesPedidodosProveedorEsquema()
        {
            string json = LoadJson("Recepciones");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }
        public static int WS_ObtencionRecepcionesPedidosProveedorRegistrosFiltrados(string filterExpressions)
        {
            string json = LoadJson("Recepciones");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom + " " + filterExpressions;

            int registrosFiltrados = ConexionSQL.getRegistrosFiltrados(query);
            return registrosFiltrados;
        }

        public static string WS_ObtencionRecepcionesPedidosProveedoresDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Recepciones");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " REC" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;

            SentenciasSQL.queryRecepciones = query + " ORDER BY " + sortExpression;

            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);


            return json;
        }

        public static string WS_ObtencionRecepcionesPedidosProveedoresDatosGridView(string sortExpression, string strCampos, string strCamposAlias, string strRelaciones)

        {
            string json = LoadJson("Recepciones");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " REC" + strRelaciones + ") SELECT " + strCamposAlias + " FROM PagingCte";

            string datosJSON = ConexionSQL.SQLClientLoadJSON(query);

            json = json.Replace("\"[DATA]\"", datosJSON);
            return json;
        }

        //Crear un Proveedor 
        public static string WS_AltaRecepcionesPedidosProveedor(string json)
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
            string jsEntero = LoadJson("Recepciones");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("INSERT", jsonFrom, hshDatos), ref error);
            return ComponerACK(ok, error);
        }

        public static string WS_EditarRecepcionesPedidosProveedor(string json)
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
            string jsEntero = LoadJson("Recepciones");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("UPDATE", jsonFrom, hshDatos, hshClausula), ref error);




            return ComponerACK(ok, error);
        }
        //Eliminar un Proveedor 
        public static string WS_EliminarRecepcionesPedidosProveedor(string json)
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
            string jsEntero = LoadJson("Recepciones");
            JArray js = JArray.Parse(jsEntero);
            string jsonFrom = js.First()["From"].ToString();
            bool ok = ConexionSQL.SQLClienteExec(GenerarComandoSQL("DELETE", jsonFrom, hshDatos, hshClausula), ref error);
            return ComponerACK(ok, error);
        }




        #region A pelo(se borrará)


        public static DataTable WS_ObtencionDevolucionesProveedor()
        {
            string query = "SELECT * FROM TBLDEVOLPROCAB";
            DataTable datos = ConexionSQL.getDataTable(query);
            return datos;
        }






        #endregion
        #endregion

        #region Combos

        public static string WS_ObtencionDatosCombo(string elemento, string valor, string tabla)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + ") AS ELEMENTO, " + valor + " AS VALOR from " + tabla + " ORDER BY " + valor + " ASC";
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            return datosJSON;
        }

        public static string WS_ObtencionDatosComboSQL(string elemento, string valor, string from)
        {
            string selectExpression = "SELECT DISTINCT(" + elemento + ") AS ELEMENTO, " + valor + " AS VALOR from " + from;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            return datosJSON;
        }

        #endregion

        public static string WS_ObtencionStockDatos2(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + jsonFrom + " STOCK" + strRelaciones + " " + filterExpression + ") SELECT " + strCamposAlias + " FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }



        #region stock
        public static string WS_ObtencionStockEsquema()
        {
            //string query = XmlReader.getStockCant();
            //DataTable dt = ConexionSQL.getDataTable(query);
            //return dt;
            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSONStock(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;

        }
        public static string WS_ObtencionStockDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, string[] strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER ("+ sortExpression + ") FROM " + strCampos[1] + " " + filterExpression + ") SELECT * FROM PagingCte";
            string selectExpression = query + " WHERE RowNum BETWEEN " + pagInicial + " AND " + pagFinal;

                string datosJSON = ConexionSQL.SQLClientLoadJSONStock(selectExpression);
                json = json.Replace("\"[DATA]\"", datosJSON);

            

            return json;
        }
        public static string WS_ObtencionStockDatosGridView(string sortExpression,string filterExpression, string[] strCampos, string strCamposAlias, string strRelaciones)
        {
            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string query = ";WITH PagingCte AS (SELECT " + strCampos[0] + ", RowNum = ROW_NUMBER() OVER (ORDER BY " + sortExpression + ") FROM " + strCampos[1] +" "+ filterExpression+ ") SELECT * FROM PagingCte";
            string datosJSON = ConexionSQL.SQLClientLoadJSONStock(query);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }

        public static int WS_ObtencionStockCantidadFiltrados(string filterExpression)
        {

            string json = LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom+" "+filterExpression;
            int cant = ConexionSQL.getRegistrosFiltradosDiusframi(selectExpression);


            return cant;

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

        #endregion

        public static DataTable getPickingEstado()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TBLPICKINGESTADO";
            dt = ConexionSQL.getDataTable(query);
            return dt;
        }
        #region Funciones Auxiliares Genéricas
        public static string WS_ObtencionParametrosControlesEsquema()
        {
            string json = LoadJson("ParametrosControles");

            return json;
        }
        public static void WS_ObtencionParametrosControlesDatos(List<string> campos, string from, ref string json)
        {
            string camposSelect = "";
            foreach (var item in campos)
            {
                camposSelect += item + ",";
            }
            camposSelect = camposSelect.TrimEnd(',');
            string selectExpression = "SELECT " + campos + " from " + from;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            json = json.Replace("\"[DATA]\"", datosJSON);

        }

        public static DataTable WS_ObtencionControlTareas()
        {
            string query = XmlReader.getControlTareas();
            DataTable dt = ConexionSQL.getDataTable(query);
            return dt;
        }

        public static string WS_ObtencionMayorElemento(string elemento, string tabla, string filtro)
        {
            string selectExpression = "SELECT MAX(" + elemento + ") AS VALOR from " + tabla + " " + filtro;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);

            return datosJSON;
        }

        //Compone un ACK o NACK en formato JSON
        private static string ComponerACK(bool ok, string error)
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

        //Carga un JSON desde Archivo de recursos
        public static string LoadJson(string tipo)
        {
            string json = string.Empty;
            try
            {
                using (StreamReader r = new StreamReader("JSON\\" + tipo + ".json"))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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


        public static string WS_ObtencionEsquema(string jsonNombre)
        {
            string json = LoadJson(jsonNombre);
            JArray js = JArray.Parse(json);
            string jsonFrom = js.First()["From"].ToString();
            string selectExpression = "SELECT COUNT(*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);

            return json;
        }
        #endregion
        #endregion
    }

    //CLASE DE CONEXION DIRECTA CON SQL SERVER
    public static class ConexionSQL
    {
        #region Variables y Propiedades

        private static String connectionString = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;
        private static String connectionStringDiusframi = "packet size=4096;user id = monica; data source = 'PORT-XRODRIGUEZ\\SQLEXPRESS01'; Failover Partner = '\';persist security info=False;initial catalog=DIUSFRAMI;Integrated Security=False;password=monica;Pooling=True;Connection TimeOut=300";


        //private static String connectionStringDiusframi = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=DEMO;Integrated Security=SSPI";

        //private static string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=DEMO;Integrated Security=SSPI";


        //"packet size=4096;user id=sa;data source='TCP:172.20.8.139'; Failover Partner='\';persist security info=True;initial catalog=DEMO;password=Es3a2xSQL;Pooling=False;Connection TimeOut=300";

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
                    command.Connection = connection;
                    command.CommandText = sqlCommand;
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                    dataAdapter.Fill(dttDatos);

                    json = JsonConvert.SerializeObject(dttDatos, Formatting.Indented);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                }
                finally
                {
                    connection.Close();
                }

                return json;
            }
        }
        public static string SQLClientLoadJSONStock(string sqlCommand)
        {
            string json = string.Empty;
            DataTable dttDatos = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionStringDiusframi))
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

                    json = JsonConvert.SerializeObject(dttDatos, Formatting.Indented);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                }
                finally
                {
                    connection.Close();
                }

                return json;
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
                    command.CommandText = sqlCommand;
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
                con.ConnectionString = connectionString;
                try
                {
                    con.Open();
                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(dt);
                    }
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);

                }
                finally
                {
                    con.Close();
                }
                return dt;
            }

        }
        public static DataTable getDataTableDiusframi(string query)
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection())
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.ConnectionString = connectionStringDiusframi;
                try
                {
                    con.Open();
                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(dt);
                    }
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message);

                }
                finally
                {
                    con.Close();
                }
                return dt;
            }
        }
        public static string ConvertDataTableToJSON(DataTable dt, int index)
        {
            var JSONString = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            Debug.WriteLine(JSONString.ToString());
            return JSONString.ToString();
        }
        public static string ConvertDataRowToJson(DataTable dt, int index)
        {
            //var rowIndex = 0;

            var jArray = JArray.FromObject(dt, JsonSerializer.CreateDefault(new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            var rowJToken = jArray[index];
            var rowJson = rowJToken.ToString(Formatting.Indented);  // Or Formatting.None if you prefer
            return rowJson;
        }
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
                    connection.Dispose();
                }
            }
            return registrosFiltrados;
        }

        public static int getRegistrosFiltradosDiusframi(string query)
        {
            int registrosFiltrados = 0;
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionStringDiusframi))
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
                    connection.Dispose();
                }
            }
            return registrosFiltrados;
        }

        #endregion
    }
}
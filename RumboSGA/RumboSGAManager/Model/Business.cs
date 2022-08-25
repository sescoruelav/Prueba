using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGAManager.Model
{
    public static class Business
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
               
        private static SqlCommand sqlCommandUbicacion = null;


        #region Funciones Públicas   
        #region Proveedores               
        //Obtiene la cantidad total de proveedores
        public static int GetProveedoresCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionProveedoresEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON Proveedores:" + e.Message);
            }
            return _cantidadRegistros;
        }

        public static DataTable GetLineasPedidosCliPendientesRecogida(List<int> pedidosSeleccionados)
        {
            String listaPedidos = "";
            try
            {
                bool primero = true;
                foreach (int idPedidoCli in pedidosSeleccionados)
                {
                    if (primero)
                    {
                        listaPedidos = listaPedidos + idPedidoCli;
                        primero = false;
                    }
                    else
                    {
                        listaPedidos = listaPedidos +","+ idPedidoCli;
                    }
                }
                if (listaPedidos.Length == 0)
                {
                    throw new Exception("No se ha seleccionado ningun pedido");
                }
                DataTable dt = ConexionSQL.getDataTable("SELECT DISTINCT PL.idpedidocli,PL.idpedidoclilin,PC.REFERENCIA as Pedido, PC.Serie ,PL.IDPEDIDOCLILIN as Línea,PL.idarticulo,A.REFERENCIA as [Código Artículo],A.ATRIBUTO as [Atributo],A.DESCRIPCION as [Artículo],PL.FECHAENTREGA as [Fecha Entrega],"
                + " PL.CANTIDAD AS Pedida, coalesce(VR.CANTIDAD, 0) AS Recogida "
                + " , PL.CANTIDAD - coalesce(VR.CANTIDAD, 0) AS Pendiente "
                + " FROM dbo.TBLPEDIDOSCLILIN PL "
                + " INNER JOIN dbo.TBLPEDIDOSCLICAB PC ON PC.IDPEDIDOCLI= PL.IDPEDIDOCLI "
                + " INNER JOIN dbo.TBLARTICULOS A ON A.IDARTICULO = PL.IDARTICULO "
                + " LEFT JOIN  dbo.VRECOGIDOPEDIDO VR ON VR.IDPEDIDOCLI = PL.IDPEDIDOCLI AND PL.IDPEDIDOCLILIN = VR.IDPEDIDOCLILIN "
                + " WHERE PL.IDPEDIDOCLI IN (" + listaPedidos + ") AND "
                + " (PL.CANTIDAD - coalesce(VR.CANTIDAD, 0)) > 0 ORDER BY PL.IDPEDIDOCLI, PL.IDPEDIDOCLILIN ");
                return dt;
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            return null;
        }

        public static DataRow getMovimientoEmbalaje(int idMovimiento)
        {
            try
            {
                DataTable dt = ConexionSQL.getDataTable("SELECT ME.*,PC.REFERENCIA AS PEDIDO," +
                    " A.REFERENCIA AS CODARTICULO,A.DESCRIPCION AS DESCARTICULO "
                    + " FROM TBLMOVIMIENTOSEMBALAJE  ME "
                    + " INNER JOIN TBLPEDIDOSPROCAB PC ON ME.IDPEDIDOPRO = PC.IDPEDIDOPRO "
                    + " INNER JOIN TBLARTICULOS A ON A.IDARTICULO = ME.IDARTICULO "
                    + " WHERE ME.IDMOVIMIENTO = "+idMovimiento);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                
                   
                
            }catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            return null;
        }
        
        #endregion
        #region MapaAlmacen
        public static DataTable getHuecosAlmacen(int v)
        {
            try
            {
                String query = "Select * from tblhuecos where idhuecoalmacen = " + v + " order by acera,portal, piso";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return null;
        }
        public static DataTable getHuecos()
        {
            try
            {
                String query = "Select * from tblhuecos";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return null;
        }

        public static DataTable ObtencionDatosTotalesCarga(int carga)
        {
            string query = "SELECT pck.idpackinglist,sum(sal.cantidad) as cantidad,sum(SAL.cantidad * art.volumen) as volumen,CAST (sum(SAL.cantidad * art.peso) as float)  as peso"
                    + " FROM TBLPACKINGLIST pck join TBLSALIDAS sal on sal.IDENTIFICADORPL = pck.IDENTIFICADOR"
                    + " join tblarticulos art on art.idarticulo = sal.idarticulo"
                    + " join tblcargalin lin on pck.identificador = lin.identificador"
                    + " where pck.identificador in (select identificador from tblcargalin where idcarga = " + carga + ") group by pck.idpackinglist";
            return DataAccess.GetSeleccion(query);
        }

        public static string GetRutaInforme(int idInforme)
        {
            string ruta = "";
            try
            {
                DataTable dt = DataAccess.GetSeleccion("SELECT * FROM RUMINFORMES WHERE IDINFORME=" + idInforme);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    ruta = Persistencia.DirectorioInformes +@"\"+ row["ARCHIVO"] + ".rpt";
                }

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            return ruta;
        }

        public static DataTable getHuecosAlmacenAcera(int almacen, int acera, int piso, Boolean portalAscendente)
        { //tiene un orden especial para dibujar bien el mapa de almacén. Pablo 22/08/20
            String query;
            try
            {
                if (piso >= 0)
                {
                    query = "Select * from tblhuecos where idhuecoestante not in ('CM','MU') and idhuecoalmacen = " + almacen + " and acera=" + acera + " and piso=" + piso + " order by acera,portal, piso";
                }
                else
                {
                    if (portalAscendente)
                        query = "Select * from tblhuecos where idhuecoestante not in ('CM','MU') and idhuecoalmacen = " + almacen + " and acera=" + acera + " order by acera, ( SELECT CASE WHEN (acera % 2)=1 THEN piso* -1 ELSE piso END) , portal";
                    else
                        query = "Select * from tblhuecos where idhuecoestante not in ('CM','MU') and idhuecoalmacen = " + almacen + " and acera=" + acera + " order by acera , ( SELECT CASE WHEN (acera % 2)=1 THEN piso* -1 ELSE piso END) , portal desc";
                }
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return null;
        }

        public static DataTable getDatosDevolucionesProduccionMaquina(int idMaquina)
        {
            try
            {
                String query =    "SELECT EX.IDENTRADA as [ID Entrada],EX.IDARTICULO AS [ID Articulo],"
                                + " EX.CANTIDAD as [Cantidad] ,A.REFERENCIA AS[Referencia], A.DESCRIPCION AS [Artículo],"
                                + " A.ATRIBUTO as [Atributo] ,A.IDPRESENTACION AS [ID Presentacion], H.DESCRIPCION AS Hueco ,"
                                + " E.LOTE AS Lote,L.FECHACADUCIDAD AS [Fecha Caducidad],"
                                + " isnull(R.CANTIDAD, 0) AS [Uds en Curso]," 
                                + " (ISNULL(N.UDSNECESARIAS,0) - ISNULL(C.CONSUMIDA,0)) AS NECESIDAD"
                                + " FROM dbo.TBLMAQUINAS AS M"
                                + " INNER JOIN VHUECOSZONA AS HZ ON M.IDZONALOGICA = HZ.IDZONACAB"
                                + "  INNER JOIN TBLEXISTENCIAS AS EX ON HZ.IDHUECO = EX.IDHUECO"
                                + "  INNER JOIN TBLARTICULOS AS A ON EX.IDARTICULO = A.IDARTICULO"
                                + "  INNER JOIN TBLHUECOS AS H ON H.IDHUECO = EX.IDHUECO INNER JOIN TBLENTRADAS E ON E.IDENTRADA = EX.IDENTRADA"
                                + "  LEFT JOIN VRESERVASHEFECTIVAS R ON R.IDENTRADA = EX.IDENTRADA AND EX.IDHUECO = r.IDHUECOORIGEN"
                                + "  LEFT JOIN TBLLOTES L ON E.IDARTICULO = L.IDARTICULO AND E.LOTE = L.LOTE"
                                + "  LEFT JOIN VNECESIDADESARTICULOMAQUINA N ON M.IDMAQUINA = N.IDMAQUINA AND EX.IDARTICULO = N.IDARTICULO"
                                + "  LEFT JOIN VSALIDASCONSUMOMAQUINAARTICULO C ON M.IDMAQUINA = C.IDMAQUINA AND EX.IDARTICULO = C.IDARTICULO"
                                + "  WHERE m.IDMAQUINA =  " + idMaquina;

                    /* SELECT IDARTICULO as [ID Articulo], REFERENCIA as [Referencia], DESCRIPCION as [Artículo],"
                                + " ATRIBUTO as [Atributo],IDPRESENTACION as [ID Presentacion], "
                                + " Hueco AS Hueco,IDENTRADA as [ID Entrada],LOTE AS Lote,FECHACADUCIDAD AS[Fecha Caducidad], "
                                + " sum(cantidad) AS cantidad,(sum(UDSNECESARIAS) - sum(consumida)) AS NECESIDAD"
                                + " FROM devoluciones"
                                + "  GROUP BY IDENTRADA,IDARTICULO, hueco, REFERENCIA, DESCRIPCION, ATRIBUTO,LOTE,FECHACADUCIDAD ,IDPRESENTACION"; */

                DataTable dt = ConexionSQL.getDataTable(query);
                Utilidades.TraducirDataTableColumnName(ref dt);
                return dt;
            }catch(Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }

        }

        public static DataTable getAcerasAlmacen(int v, string orden)
        {
            try
            {
                String query = "Select * from tblaceras where idhuecoalmacen = " + v + " ORDER BY " + orden;
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return null;
        }

        public static DataTable getLlenadoMapa()
        {
            try
            {
                String query = "SELECT hu.DESCRIPCION as ubicacion, coalesce(ex.CANTIDAD,0) as cantidad," +
                    " ex.CANTIDADUNIDAD, art.REFERENCIA, coalesce(art.CANTIDAD,1) as cantidadPalet, " +
                    " art.DESCRIPCION as descArticulo " +
                    " FROM[dbo].TBLHUECOS hu " +
                    " left outer join dbo.TBLEXISTENCIAS ex on hu.IDHUECO = ex.IDHUECO " +
                    " left outer join dbo.TBLARTICULOS art on art.IDARTICULO = ex.IDARTICULO " +
                    " where idhuecoestante not in ('CM', 'MU')";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return null;
        }
        public static Boolean iniReaderDatosUbicacion()
        {
            try
            {
                string query = "SELECT exi.cantidad ,exi.idunidadtipo,exi.CANTIDADUNIDAD,exi.FECHACADUCIDAD,exi.IDEXISTENCIAESTADO, exi.prioridad, " +
                                "   hu.DESCRIPCION as UBICACION, hu.CAPACIDAD,hu.CAPACIDADART, hu.IDHUECOESTADO, " +
                                "   art.referencia, art.descripcion , art.cantidad as cantidadPalet " +
                                "FROM[dbo].[TBLEXISTENCIAS] exi left outer join dbo.TBLHUECOS hu on exi.idhueco=hu.IDHUECO " +
                                "   left outer join dbo.TBLARTICULOS art on exi.idarticulo= art.idarticulo " +
                                "Where hu.descripcion= @UBICACION";
                sqlCommandUbicacion = ConexionSQL.getCommandParameterString(query, "@UBICACION");
                return true;
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return false;
        }
        public static Boolean iniReaderDatosUbicacion(string tipoConsulta)
        {
            return iniReaderDatosUbicacion(tipoConsulta, 0);
        }
        //Solucion mas rápida
        public static DataTable getFrecuenciaAcceso(string tipoConsulta, int parametro)
        {
            string query = null;
            try
            {
                if (tipoConsulta.Equals("LLENADOPORUBICACIONES"))
                {
                    query = "SELECT exi.cantidad ,exi.idunidadtipo,exi.CANTIDADUNIDAD,exi.FECHACADUCIDAD,exi.IDEXISTENCIAESTADO, exi.prioridad, " +
                               "   hu.DESCRIPCION as UBICACION, hu.CAPACIDAD,hu.CAPACIDADART, hu.IDHUECOESTADO, " +
                               "   art.referencia, art.descripcion , art.cantidad as cantidadPalet " +
                               "FROM [dbo].[TBLEXISTENCIAS] exi left outer join dbo.TBLHUECOS hu on exi.idhueco=hu.IDHUECO " +
                               "   left outer join dbo.TBLARTICULOS art on exi.idarticulo= art.idarticulo ";
                }
                else if (tipoConsulta.Equals("FrecuenciaAcceso"))
                {
                    /*query = "SELECT hu.DESCRIPCION as UBICACION, count(*) as frecuencia " +
                        "FROM[dbo].tblhuecos hu left outer join dbo.TBLOPERARIOSMOV OMo on hu.IDHUECO = omo.IDHUECOORIGEN " +
                        "left outer join dbo.TBLOPERARIOSMOV OMD on hu.IDHUECO = omd.IDHUECODESTINO " +
                        "where hu.idhueco <> 0 and omd.FECHAHORA >= getdate() - " + Convert.ToString(parametro) +
                        " and omo.FECHAHORA >= getdate() - " + Convert.ToString(parametro) +
                        " group by hu.DESCRIPCION ";*///iba lento
                    query = "SELECT hu.DESCRIPCION as UBICACION, count(*) as frecuencia " +
                        "FROM[dbo].tblhuecos hu left outer join dbo.TBLOPERARIOSMOV OMo on hu.IDHUECO = omo.IDHUECOORIGEN " +
                        "where hu.idhueco <> 0 and omo.FECHAHORA >= (getdate() - " + Convert.ToString(parametro) + ") "+
                        " group by hu.DESCRIPCION " +
                        "UNION " +
                        "SELECT hu.DESCRIPCION as UBICACION, count(*) as frecuencia " +
                        "FROM[dbo].tblhuecos hu left outer join dbo.TBLOPERARIOSMOV OMD on hu.IDHUECO = OMD.IDHUECODESTINO " +
                        "where hu.idhueco <> 0 and OMD.FECHAHORA >= (getdate() - " + Convert.ToString(parametro) +") "+
                        " group by hu.DESCRIPCION";
                }
                else if (tipoConsulta.Equals("Zona"))
                {
                    query = "SELECT hu.DESCRIPCION as UBICACION " +
                        "FROM [dbo].[VHUECOSZONA] hu " +
                        "Where idzonacab= "+ Convert.ToString(parametro);
                }
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return null;
        }
        public static Boolean iniReaderDatosUbicacion(string tipoConsulta, int dias)
        {
            try
            {
                string query = null;
                if (tipoConsulta.Equals("LLENADOPORUBICACIONES"))
                {
                     query = "SELECT exi.cantidad ,exi.idunidadtipo,exi.CANTIDADUNIDAD,exi.FECHACADUCIDAD,exi.IDEXISTENCIAESTADO, exi.prioridad, " +
                                "   hu.DESCRIPCION as UBICACION, hu.CAPACIDAD,hu.CAPACIDADART, hu.IDHUECOESTADO, " +
                                "   art.referencia, art.descripcion , art.cantidad as cantidadPalet, ent.LOTE ,ent.identrada as matricula, ent.sscc  " +
                                "FROM dbo.TBLHUECOS hu left outer join [TBLEXISTENCIAS] exi    on exi.idhueco=hu.IDHUECO left outer join tblentradas ent on exi.IDENTRADA=ent.IDENTRADA "+
                                "  left outer join dbo.TBLARTICULOS art on exi.idarticulo = art.idarticulo " +
                                "Where hu.descripcion= @UBICACION";
                }
                else if (tipoConsulta.Equals("REFERENCIAPICKING"))
                {
                    query = "SELECT art.referencia, art.descripcion ,hp.descripcion as huecoPicking " +
                            "FROM  dbo.TBLARTICULOS art left outer join dbo.TBLHUECOS hp on art.IDHUECOPICKING = hp.IDHUECO " +
                            "Where hp.descripcion= @UBICACION";
                }
                else if (tipoConsulta.Equals("FrecuenciaAcceso"))
                {
                    query = "SELECT hu.DESCRIPCION, count(*) " + 
                        "FROM[dbo].tblhuecos hu left outer join dbo.TBLOPERARIOSMOV OMo on hu.IDHUECO = omo.IDHUECOORIGEN "+
                        "left outer join dbo.TBLOPERARIOSMOV OMD on hu.IDHUECO = omd.IDHUECODESTINO "+
                        "where hu.idhueco <> 0 and omd.FECHAHORA >= getdate() - " + dias +
                        " and omo.FECHAHORA >= getdate() - " + dias +
                        " and hu.descripcion= @UBICACION "+
                        " group by hu.DESCRIPCION ";
                }

                sqlCommandUbicacion = ConexionSQL.getCommandParameterString(query, "@UBICACION");
                return true;
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return false;
        }
        //TODO CAMBIAR
        public static SqlDataReader getReaderDatosUbicacion(string ubicacion)
        {
            try
            {
                if(sqlCommandUbicacion is null)
                {
                    MessageBox.Show("No se ha podido recuperar la información de la ubicacion");
                    return null;
                }
                SqlDataReader sqlDataReader= ConexionSQL.getDataReaderParameter(sqlCommandUbicacion, "@UBICACION", ubicacion);
                return sqlDataReader;
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
            }
            return null;
        }
        public static void closeDatosUbicacion()
        {
            ConexionSQL.closeCommandParameter();
        }
        #endregion
        #region xxx
        public static int GetProveedoresRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionProveedoresRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación
        public static List<dynamic> GetProveedoresDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "PROV");
                string json = DataAccess.ObtencionProveedoresDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON Proveedores:" + e.Message);
            }
            return lstDatos;
        }

        public static string getQueryLineasReservadasPedido(string where)
        {
            return "SELECT pl.IDPEDIDOCLI," +
                     "(count(r.IDPEDIDOCLILIN) / CAST(count(pl.IDPEDIDOCLILIN) AS DECIMAL(9, 2))) * 100 AS[% LINEASRESERVADAS] " +
                     "FROM TBLPEDIDOSCLILIN pl " +
                     "INNER JOIN TBLPEDIDOSCLICAB PC ON PC.IDPEDIDOCLI=PL.IDPEDIDOCLI" +
                     " left JOIN VRESUMENRESERVA r " +
                     " ON pl.IDPEDIDOCLI = r.IDPEDIDOCLI AND pl.IDPEDIDOCLILIN = r.IDPEDIDOCLILIN " +                   
                     where +
                     " GROUP BY pl.IDPEDIDOCLI";
        }

        public static string getQueryPaletsPickingsDuracionEstimada(string where)
        {
            return "select pcl.idpedidocli,coalesce(sum(pcl.CANTIDAD/art.CANTIDAD),0) as palets," +
                     "sum(case when pcl.CANTIDAD% art.cantidad>0 then 1 else 0 end)  AS pickings," +
                     "sum((pcl.cantidad/art.cantidad) * art.tiempomedsc + (case when pcl.cantidad%art.cantidad>0 then 1 else 0 end) * art.tiempomedpi) " +
                     "as tiempoestimado,count(idpedidoclilin) AS numlineas  " +
                     "from TBLPEDIDOSCLILIN PCL " +
                     " INNER JOIN TBLPEDIDOSCLICAB PC ON PC.IDPEDIDOCLI=PCL.IDPEDIDOCLI " +
                     "left outer join TBLARTICULOS art on pcl.idarticulo= art.idarticulo " +
                      where +
                     "GROUP BY PCL.IDPEDIDOCLI";
        }

        public static string getQueryQtyResSerLineasPedido(string where)
        {
            return "SELECT R.idpedidocli,R.idpedidoclilin" +
                ",CAST ((R.reservada /CAST(R.cantidad AS DECIMAL (18,2)) * 100) AS DECIMAL (18,2)) AS [%RESERVADA], " +
                " CAST((R.SERVIDA / CAST(R.cantidad AS DECIMAL(18, 2)) * 100) AS DECIMAL(18, 2)) AS[% SERVIDA],r.reservada,r.servida " +
                "FROM VRESUMENPEDIDO R INNER JOIN TBLPEDIDOSCLICAB PC ON R.IDPEDIDOCLI=PC.IDPEDIDOCLI " + where;
        }

        public static string getQueryPaletsPickingsLineas(string where)
        {
            return
                "select pcl.idpedidocli,pcl.idpedidoclilin," +
                "coalesce(sum(pcl.CANTIDAD / CASE WHEN art.CANTIDAD > 0 THEN art.CANTIDAD ELSE 1 END), 0) as Palets, " +
                " sum(case when art.cantidad>0 AND pcl.CANTIDAD % art.cantidad > 0 then 1 else 0 end) " +
                "from TBLPEDIDOSCLILIN PCL " +
                " INNER JOIN TBLPEDIDOSCLICAB PC ON PC.IDPEDIDOCLI=PCL.IDPEDIDOCLI " +
                "left outer join TBLARTICULOS art on pcl.idarticulo = art.idarticulo " +
               where
                + " group by pcl.idpedidocli,pcl.idpedidoclilin";
                
        }

        public static string getQueryTareasPendientesPedido(string where)
        {
            return "SELECT  PC.IDPEDIDOCLI,COUNT(DISTINCT TP.IDTAREA) AS NUMTAREAS " +
                    "FROM TBLPEDIDOSCLICAB PC " +
                    "LEFT JOIN TBLRESERVAS R ON PC.IDPEDIDOCLI = R.IDPEDIDOCLI " +
                    "LEFT JOIN DBO.TBLTAREASPENDIENTES AS TP ON R.IDTAREA = TP.IDTAREA AND TP.TIPOTAREA IN('PI', 'SP', 'SD') " +                    
                    where +
                    "GROUP BY PC.IDPEDIDOCLI";
        }

        public static string getQueryLineasPreparadasPedido(string where)
        {
            return "SELECT pl.IDPEDIDOCLI," +
                      "CAST ((count(S.IDPEDIDOCLILIN) /    CAST(count(pl.IDPEDIDOCLILIN) AS DECIMAL (9,2)) * 100) AS DECIMAL (9,2))" +
                      " FROM TBLPEDIDOSCLILIN pl" +
                      " INNER JOIN TBLPEDIDOSCLICAB PC ON PC.IDPEDIDOCLI=PL.IDPEDIDOCLI " +
                      " left JOIN VRESUMENSALIDA s ON pl.IDPEDIDOCLI=s.IDPEDIDOCLI AND pl.IDPEDIDOCLILIN=s.IDPEDIDOCLILIN " +                      
                      where +
                      " GROUP BY pl.IDPEDIDOCLI";
        }

        public static DataTable GetProveedoresDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROV");
                string json = DataAccess.ObtencionProveedoresDatosGridView("IDPROVEEDOR", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON Proveedores:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un proveedor
        public static AckResponse AltaProveedor(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaProveedor(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un proveedor
        public static AckResponse EditarProveedor(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarProveedor(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un proveedor
        public static AckResponse EliminarProveedor(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarProveedor(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Clientes
        //Obtiene la cantidad total de clientes
        public static int GetClientesCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionClientesEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetClientesRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionClientesRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los clientes, según paginación
        public static List<dynamic> GetClientesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "CLI");
                string json = DataAccess.ObtencionClientesDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetClientesDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "CLI");
                string json = DataAccess.ObtencionClientesDatosGridView("IDCLIENTE", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un cliente
        public static AckResponse AltaCliente(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaCliente(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un cliente
        public static AckResponse EditarCliente(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarCliente(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un proveedor
        public static AckResponse EliminarCliente(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarCliente(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Cliente Pedidos Cab
        public static int GetClientesPedidosCabCantidad(ref List<TableScheme> lstEsquemaTabla, ref GridScheme esquemaGrid)
        {
            int _cantidadRegistros = 0;
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;

                string json = DataAccess.ObtencionClientesPedidosCabEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
                string jsonGridEsquema = jo.First()["DataGridLin"].ToString();
                if (!string.IsNullOrEmpty(jsonGridEsquema))
                {
                    esquemaGrid = JsonConvert.DeserializeObject<GridScheme>(jsonGridEsquema);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetClientesPedidosCabRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionClientesPedidosCabRegistrosFiltrados(filterExpression);
        }
        public static List<dynamic> GetClientesPedidosCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "CLICAB");
                string json = DataAccess.ObtencionClientesPedidosCabDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetClientesPedidosDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "CLICAB");
                string json = DataAccess.ObtencionClientesPedidosDatosGridView("IDPEDIDOCLI", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaClientePedidoCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaClientePedidoCab(json, false);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse AltaClientePedidoCabYLineas(List<TableScheme> lstEsquemaTabla, dynamic values, GridScheme EsquemaGrid)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "], \r\n\"DataGridLin\":[" + JsonConvert.SerializeObject(EsquemaGrid) + "]}]";
                string ack = DataAccess.AltaClientePedidoCab(json, true);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarClientePedidoCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarClientePedidoCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un ClientesPedidosCab
        public static AckResponse EliminarClientePedidoCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarClientePedidoCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static DataTable CargaPoolPedidosClientePtes(String queryFilter=null,bool soloEstructura=false)

        {   
            
            //la clase tiene que ser PEDIDOS_CLIENTE para que coincida con el enum tipoOrganizador
            string query = " SELECT 'PEDIDOS_CLIENTE' as CLASE, pcc.[SERIE],pcc.[REFERENCIA], cli.nombre as cliente ,  age.nombre as agencia ,[IDPEDIDOCLIESTADO],[FECHAPREVENVIO] " +
                            ", pcc.[MUELLE], ru.DESCRIPCION as ruta ,[FECHATOPEENVIO] ,[BULTOS],pcc.[POBLACION] " +
                            ",[TIPOLANZAMIENTO],[OBSERVACIONES] ,[HORAPLANIFICADA]  " +
                            " ,[TIEMPOESTIMADO] ,[VOLUMEN],[PESO], idpedidoCli as idOrden, 0 as seleccionado " +
                           " FROM[TBLPEDIDOSCLICAB] PCC join tblclientes cli on pcc.IDCLIENTE=cli.IDCLIENTE " +
                            " join TBLRUTAS ru on ru.IDRUTA=pcc.IDRUTA " +
                            " join TBLAGENCIAS age on age.IDAGENCIA= pcc.IDAGENCIA " +
                            " where IDPEDIDOCLIESTADO <>'PC'";
            if (soloEstructura)
            {
                query = query + " AND 4=5";
            }
            if (!String.IsNullOrEmpty(queryFilter))
            {
                query = queryFilter;
            }
                try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static DataTable CargaPedidoCliCab(int idPedidoCli)
        {
            string query = "SELECT 1 as priority, [SERIE],[REFERENCIA], cli.nombre as cliente, 0 as lineas, 0 as lineas_OK,   pcc.peso as peso, pcc.volumen as volumen, 0 as palets, 0 as tiempo," +
                " 0 as seleccionado, 'PEDIDOS_CLIENTE' as CLASE, idPedidoCli as idOrden, 0 as rowID" +
                           " FROM[TBLPEDIDOSCLICAB] PCC join tblclientes cli on pcc.IDCLIENTE=cli.IDCLIENTE " +
                            " where idpedidocli=" + idPedidoCli; //con -1 esperamos una tabla vacia
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static DataTable CargaPedidoCliLin(int idPedidoCli, string queryFiltrada="")
        {

            string query = "SELECT 0 as priority, art.referencia,  art.descripcion as articulo, coalesce(vdisp.stock, 0) as disponible, pcl.pterecogida as solicitado, 0 as asignado,coalesce(vpick.cantidad, 0) as enPicking," +
                " 0 as picking, 0 as paletsCompletos, 0 as reposiciones,  0 as lineas, 0 as lineas_OK, 0 as esperado, 0 as uds, 0 as cajas, 0 as palets, art.cantidadunidad, art.cantidad as cantPalet, 'XX' as presentacionStd, 0 as udsStd , 0 as seleccionado, pcl.idarticulo ,coalesce(vdisp.stock, 0) as disponible_inicial, art.peso, art.volumen " +
                           " FROM [VPTERECOGIDAPEDIDO] PCL join tblarticulos art  on pcl.idarticulo=art.idarticulo left outer join vdisponibleArticulo vdisp on pcl.idarticulo=vdisp.idarticulo  left outer join VEXISTENCIASPI vpick on vpick.idarticulo=pcl.IDARTICULO" +
                            " where pcl.idpedidocli=" + idPedidoCli; //con -1 esperamos una tabla vacia
            if (!string.IsNullOrEmpty(queryFiltrada))
            {
                query = queryFiltrada + idPedidoCli;
            }
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static DataTable PedidoTieneArticulo(int idPedidoCli, int idarticulo)
        {
            string query = " select count(*) as tiene from [TBLPEDIDOSCLILIN] PCL where pcl.idpedidocli=" + idPedidoCli + " and idarticulo=" + idarticulo;
            
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        #endregion
        #region Algoritmos
        public static DataTable CargaAlgoritmo(int idAlgoritmo)
        {
            string query = " select  [IDALG] ,[SALIDA] ,[CONDICION] ,[PARAMETROS] ,[NOMBRE]  ,[DESCRIPCION],[INICIALIZACION] ,[CLASIFICACION] ,[IDENTIFICADORCLAVE] ," +
                "[NIVELLOG],[HABILITADO] from [RUMALGORITMOS] alg where alg.IDALG=" + idAlgoritmo;

            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static DataTable CargaAccionesAlgoritmos(int idAlgoritmo)
        {
            string query = " select  [ID],[SENTENCIA] ,[ORDEN] ,[TIPO] ,[HABILITADO] ,[CONDICION] ,[INICIALIZACION]" +
                " from [RUMACCIONES] acc where acc.id=" + idAlgoritmo+ " order by orden";
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static DataTable CargaSentenciasAlgoritmos(int idSentencia)
        {
            string query = " select  [IDSNT] ,[SENTENCIAS]   ,[SALIDA]  ,[SINULL] ,[MIENTRAS]  ,[NOMBRE]   ,[DESCRIPCION] ,[PARAMETROS] ,[CONDICION] ,[SINO] ,[INICIALIZACION]  ,[IDENTIFICADORCLAVE]  ,[NIVELLOG]" +
                " from [RUMSENTENCIAS] snt where snt.IDSNT=" + idSentencia;
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        #endregion 
        #region Cliente Pedidos Lineas
        //Obtiene la cantidad total de ClientesPedidosLinea
        public static int GetClientesPedidosLineasCantidad(ref List<TableScheme> lstEsquemaTabla, string _filtro)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionClientesPedidosLineasEsquema(_filtro);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación
        public static List<dynamic> GetClientesPedidosLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "CLILIN");
                string json = DataAccess.ObtencionClientesPedidosLineasDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                //lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetClientesPedidosLineasJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasClienteLineas");
                string fullQuery = "SELECT Distinct " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json OrdenRecogidasClienteLineas: " + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenesCargaPedidosResumenLineasJerarquicoDatosGridView(string idcarga)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenesCargaPedidosResumenLineas");
                string fullQuery = "SELECT Distinct " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idcarga;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json OrdenRecogidasClienteLineas: " + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetGenericoJerarquicoDatosGridView(string jsonNombre, string parametro,string id)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico(jsonNombre);
                query[2] = query[2].Replace(parametro, id);
                string fullQuery = "SELECT Distinct " + query[0]  + query[1]+ query[2];
               
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json : " + jsonNombre + " " + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetClientesPedidosLineasDatosGridView(string idPedidoFiltro,string sortExpression, List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "CLILIN");
                string json = DataAccess.ObtencionClientesPedidosLineasDatosGridView(sortExpression, strCampos, strCamposAlias, strRelaciones, idPedidoFiltro);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un ClientesPedidosLinea
        public static AckResponse AltaClientePedidoLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaClientePedidoLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un ClientesPedidosLinea
        public static AckResponse EditarClientePedidoLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarClientePedidoLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un ClientesPedidosLinea
        public static AckResponse EliminarClientePedidoLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarClientePedidoLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Carga Cabecera    carga           
        //Obtiene la cantidad total de cargas
        public static int GetCargaCabCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionCargaCabEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetCargaCabRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionCargaCabRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las cargas, según paginación
        public static List<dynamic> GetCargaCabDatos(string sortExpression, string filterExpression, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "CARGACAB");
                string json = DataAccess.ObtencionCargaCabDatos(sortExpression, filterExpression, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetCargaCabDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "CARGACAB");
                string json = DataAccess.ObtencionCargaCabDatosGridView("IDCARGA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una carga
        public static AckResponse AltaCarga(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaCargaCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica una carga
        public static AckResponse EditarCarga(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarCargaCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina una carga
        public static AckResponse EliminarCarga(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarCargaCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Proveedores Pedidos Cab
        //Obtiene la cantidad total de proveedores
        public static int GetProveedoresPedidosCabCantidad(ref List<TableScheme> lstEsquemaTabla, ref GridScheme esquemaGrid)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionProveedoresPedidosCabEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
                string jsonGridEsquema = jo.First()["DataGridLin"].ToString();
                if (!string.IsNullOrEmpty(jsonGridEsquema))
                {
                    //esquemaGrid = JsonConvert.DeserializeObject<List<GridScheme>>(jsonGridEsquema)[0];
                    esquemaGrid = JsonConvert.DeserializeObject<GridScheme>(jsonGridEsquema);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetProveedoresPedidosCabRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionProveedoresPedidosCabRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación
        public static List<dynamic> GetProveedoresPedidosCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "PROCAB");
                string json = DataAccess.ObtencionProveedoresPedidosCabDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un proveedor
        public static AckResponse AltaProveedorPedidoCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaProveedorPedidoCab(json, false);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse AltaProveedorPedidoCabYLineas(List<TableScheme> lstEsquemaTabla, dynamic values, GridScheme EsquemaGrid)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "], \r\n\"DataGridLin\":[" + JsonConvert.SerializeObject(EsquemaGrid) + "]}]";
                string ack = DataAccess.AltaProveedorPedidoCab(json, true);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un proveedor
        public static AckResponse EditarProveedorPedidoCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarProveedorPedidoCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un proveedor
        public static AckResponse EliminarProveedorPedidoCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarProveedorPedidoCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static string[] GetFieldsSQLProvPedidos(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("ProveedoresPedidosCab");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            //if (lstEsquemaTabla[i].EsFK)
            //{
            //    //Componer Outer Apply
            //    string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
            //    strRelaciones += " " + oaAux + " ";
            //    fields += "OA" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoMostrado + " AS " + lstEsquemaTabla[i].Nombre;
            //    if (lstEsquemaTabla[i].EtiqIngles != string.Empty && lstEsquemaTabla[i].EtiqIngles != null)
            //    {
            //        strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].EtiqIngles + "'";
            //    }
            //    else
            //    {
            //        strCamposAlias += lstEsquemaTabla[i].Nombre;
            //    }
            //}
            //else
            //{
            //    if (lstEsquemaTabla[i].EtiqIngles != null && lstEsquemaTabla[i].EtiqIngles != string.Empty)
            //    {
            //        fields += aliasTabla + "." + lstEsquemaTabla[i].Nombre;
            //        strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].EtiqIngles + "'";
            //    }
            //    else
            //    {
            //        fields += aliasTabla + "." + lstEsquemaTabla[i].Nombre;
            //        strCamposAlias += lstEsquemaTabla[i].Nombre;
            //    }

            //}
            string jsonFrom = js.First()["FROM"].ToString();

            string selectExpression = "SELECT COUNT (IDPEDIDOPRO) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        #endregion
        #region Proveedores Pedidos Lineas
        //Obtiene la cantidad total de proveedores
        public static int GetProveedoresPedidosLineasCantidad(ref List<TableScheme> lstEsquemaTabla, string _filtro)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionProveedoresPedidosLineasEsquema(_filtro);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación
        public static List<dynamic> GetProveedoresPedidosLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "PROLIN");
                string json = DataAccess.ObtencionProveedoresPedidosLineasDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                //lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetProveedoresPedidosLineasDatosGridView(string sortExpression, List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROLIN");
                string json = DataAccess.ObtencionProveedoresPedidosLineasDatosGridView(sortExpression, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
               
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un proveedor
        public static AckResponse AltaProveedorPedidoLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaProveedorPedidoLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un proveedor
        public static AckResponse EditarProveedorPedidoLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarProveedorPedidoLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un proveedor
        public static AckResponse EliminarProveedorPedidoLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarProveedorPedidoLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Articulos
        //Obtiene la cantidad total de Familias
        public static int GetArticulosCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionArticulosEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetArticulosRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionArticulosRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las Familias, según paginación
        public static List<dynamic> GetArticulosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "ART");
                string json = DataAccess.ObtencionArticulosDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetArticulosDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "ART");
                string json = DataAccess.ObtencionArticulosDatosGridView("IDARTICULO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una Familia
        public static AckResponse AltaArticulo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaArticulo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }

        public static AckResponse EditarArticulo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarArticulo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }

        public static AckResponse EditDataMasivo(dynamic selectedRow, string where,string nombretabla)
        {
            try
            {
                string set = "";
                string error = "";
                JObject lineaEditada = JsonConvert.DeserializeObject<JObject>(selectedRow);
                foreach (KeyValuePair<string, JToken> x in lineaEditada)
                {
                    string key = x.Key;
                    key = Utilidades.sacarCampo(key);
                    string value = x.Value.ToString();
                    set = set + "" + key + "=" + value + ",";
                }
                set = set.Remove(set.LastIndexOf(','), 1);
                string sqlComand = "UPDATE "+nombretabla+" SET " + set + " WHERE " + where;
                bool ok = ConexionSQL.SQLClienteExec(sqlComand, ref error);
                return DataAccess.ComponerACKO(ok, error);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }
        public static AckResponse UpdateCell(string where, string tabla, string set)
        {
            try
            {
                string error = "";
                string sqlComand = "UPDATE " + tabla + " SET " + set + " WHERE " + where;
                bool ok = ConexionSQL.SQLClienteExec(sqlComand, ref error);
                return DataAccess.ComponerACKO(ok, error);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }
        //Tomás Update de la fila seleccionada editable editable pero se puede hacer para que sea masivo
        public static AckResponse UpdateRow(GridViewRowInfo linea, string where,string tabla,List<TableScheme> lista)
        {
            try
            {
                string set = "";
                string error = "";
                string columnaSQL="";
                string tipo = "";
                string aliasPrincipal = "";
                TableScheme fila = null ;
                List<TableScheme> schemes = lista;
                foreach (GridViewCellInfo celda in linea.Cells)
                {
                    string valor = celda.Value.ToString();
                    string nombreColumna = celda.ColumnInfo.FieldName;
                    foreach (TableScheme table in schemes)
                    {
                        if (table.Etiqueta == nombreColumna)
                        {
                            columnaSQL = table.Nombre;
                            columnaSQL = columnaSQL.Substring(columnaSQL.IndexOf('.') + 1);
                            tipo = table.Tipo;
                            fila = table;
                            if (table==schemes[0])
                            {
                                aliasPrincipal = fila.Nombre.Substring(0, fila.Nombre.IndexOf("."));
                            }
                            break;
                        }
                        else
                        {
                            fila = null;
                        }
                    }
                    if (fila != null)
                    {

                        if (fila.Nombre.Substring(0, fila.Nombre.IndexOf(".")) ==aliasPrincipal)
                        {
                            if (fila.Control == "CMB")
                            {

                                string sql = "Select " + fila.CmbObject.CampoRelacionado + " from " + fila.CmbObject.TablaRelacionada + " where " + fila.CmbObject.CampoMostrado + " = '" + valor + "'";
                                DataTable data = ConexionSQL.SQLClientLoad(sql);
                                valor = data.Rows[0][0].ToString();
                                if (tipo == "VARCHAR")
                                {
                                    set = set + "" + columnaSQL.Trim() + "= '" + valor + "',";
                                }
                                else { set = set + "" + columnaSQL.Trim() + "= " + valor + ","; }
                            }
                            else if (valor == "")
                            {
                                valor = "null";
                                set = set + "" + columnaSQL.Trim() + "=" + valor + ",";
                            }
                            else if (tipo == "VARCHAR"||tipo=="DATETIME")
                            {
                                set = set + "" + columnaSQL.Trim() + "= '" + valor + "',";
                            }
                            else if (tipo == "INT")
                            {
                                set = set + "" + columnaSQL.Trim() + "=" + valor + ",";
                            }
                        }

                    }
                    
                }
                set = set.Remove(set.LastIndexOf(','), 1);
                string sqlComand = "UPDATE "+tabla+" SET " + set + " WHERE " + where;
                bool ok = ConexionSQL.SQLClienteExec(sqlComand, ref error);
                return DataAccess.ComponerACKO(ok, error);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        public static AckResponse EditarArticulo(List<TableScheme> lstEsquemaTabla, dynamic values,string where)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarArticulo(json,where);
                if (!ack.Equals(""))
                    ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
                else
                    return null;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un proveedor
        public static AckResponse EliminarArticulo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarArticulo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static DataTable GetArticuloDescripcion()
        {
            String query = "";
            try
            {
                query = "SELECT  art.IDARTICULO,art.referencia, art.DESCRIPCION FROM TBLARTICULOS art ";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
                throw new Exception(e.Message + "\n " + query);
            }
        }
        public static DataTable getArticuloDisponibilidad(int idarticulo)
        {
            String query = "";
            try
            {
                query = "SELECT  art.IDARTICULO,art.referencia, art.DESCRIPCION FROM TBLARTICULOS art where idarticulo= "+ idarticulo;
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
                throw new Exception(e.Message + "\n " + query);
            }
        }
        #endregion
        #region ReservaHistoricoControl
        //Obtiene la cantidad total de ReservasHistorico
        public static int GetReservasHistoricoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionReservasHistoricoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetReservasHistoricoRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionReservasHistoricoRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los ReservasHistoricos, según paginación
        public static List<dynamic> GetReservasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "RESHIST");
                string json = DataAccess.ObtencionReservasHistoricoDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetReservasHistoricoDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "RESHIST");
                string json = DataAccess.ObtencionReservasHistoricoDatosGridView("IDHISTORICO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un ReservaHistorico
        public static AckResponse AltaReservaHistorico(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaReservasHistorico(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un ReservaHsitorico
        public static AckResponse EditarReservaHistorico(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarReservasHistorico(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un ReservaHistorico
        public static AckResponse EliminarReservaHistorico(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarReservasHistorico(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
 
            return ackobject.First();
        }
        #endregion
        #region Maquinas
        //Obtiene la cantidad total de maquinas
        public static int GetMaquinasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionMaquinasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }

        public static string actualizarCampoStock(string tabla, string nombreColumna, string nuevoValor, int idEntrada)
        {
            string error="";
            String query = "";
            try
            {
                string[] tablas =tabla.Split(';');
                foreach (string t in tablas)
                {
                    query = "UPDATE " + t + " SET " + nombreColumna + "='" + nuevoValor + "'" + " WHERE IDENTRADA=" + idEntrada;
                    ConexionSQL.SQLClienteExec(query, ref error);
                }
                
              
            }catch(Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex,query);
            }
            return error;
        }

        public static int GetMaquinasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionMaquinasRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las maquinas, según paginación
        public static List<dynamic> GetMaquinasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "MAQ");
                string json = DataAccess.ObtencionMaquinaDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetMaquinasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "MAQ");
                string json = DataAccess.ObtencionMaquinasDatosGridView("IDMAQUINA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una maquina
        public static AckResponse AltaMaquina(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaMaquina(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica una maquina
        public static AckResponse EditarMaquina(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarMaquina(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina una maquina
        public static AckResponse EliminarMaquina(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarMaquina(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Familias
        //Obtiene la cantidad total de Familias
        public static int GetFamiliasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionFamiliasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetFamiliasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionFamiliasRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las Familias, según paginación
        public static List<dynamic> GetFamiliasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "FAM");
                string json = DataAccess.ObtencionFamiliasDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetFamiliasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "FAM");
                string json = DataAccess.ObtencionFamiliasDatosGridView("IDFAMILIA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una Familia
        public static AckResponse AltaFamilia(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaFamilia(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica una Familia
        public static AckResponse EditarFamilia(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarFamilia(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un proveedor
        public static AckResponse EliminarFamilia(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarFamilia(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Agencias
        //Obtiene la cantidad total de agencias
        public static int GetAgenciasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionAgenciasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetAgenciasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionAgenciasRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las Agencias, según paginación
        public static List<dynamic> GetAgenciasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "AGEN");
                string json = DataAccess.ObtencionAgenciasDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:"+e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetAgenciasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "AGEN");
                string json = DataAccess.ObtencionAgenciasDatosGridView("IDAGENCIA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaAgencia(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaAgencia(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarAgencia(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.WS_EditarAgencia(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarAgencia(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarAgencia(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region EstadoFabricacionControl
        //Obtiene la cantidad total de agencias
        public static int GetEstadoFabricacionControlCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoFabricacionControlEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoFabricacionControlRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoFabricacionControlRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las Agencias, según paginación
        public static List<dynamic> GetEstadoFabricacionControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "AGEN");
                string json = DataAccess.ObtencionEstadoFabricacionControlDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetEstadoFabricacionControlDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "AGEN");
                string json = DataAccess.ObtencionEstadoFabricacionControlDatosGridView("IDORDENFABESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaEstadoFabricacionControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaEstadoFabricacionControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarEstadoFabricacionControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.WS_EditarEstadoFabricacionControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarEstadoFabricacionControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarEstadoFabricacionControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static int getEstadoFabricacionCondicionalesColor(ref List<ConditionalFormattingObject> listaCondicionales)
        {
            try
            {
                DataTable resultado = ConexionSQL.getDataTable("SELECT * FROM TBLORDENFABRICACIONESTADO");
                if (resultado.Rows.Count == 0)
                {
                    log.Error("Mensaje: SELECT * FROM TBLORDENFABRICACIONESTADO no ha devuelto ningún valor en ColorEstado() de AcopiosProducción");
                }
                else
                {
                    foreach (DataRow row in resultado.Rows)
                    {
                        ConditionalFormattingObject objCondicion =
                            new ConditionalFormattingObject(row["DESCRIPCION"].ToString(), ConditionTypes.Equal, row["IDORDENFABESTADO"].ToString(), "", false);


                        Color colorcito = new Color();
                        colorcito = Color.FromArgb(Convert.ToInt32(row["COLORAMOSTRAR"]));
                        objCondicion.CellBackColor = colorcito;

                        listaCondicionales.Add(objCondicion);
                    }
                }
                return 1;

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                return -1;
            }
        }


        #endregion
        #region EstadoMaquinaControl
        //Obtiene la cantidad total de agencias
        public static int GetEstadoMaquinaControlCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoMaquinaControlEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoMaquinaControlRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoMaquinaControlRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las Agencias, según paginación
        public static List<dynamic> GetEstadoMaquinaControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "AGEN");
                string json = DataAccess.ObtencionEstadoMaquinaControlDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetEstadoMaquinaControlDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "AGEN");
                string json = DataAccess.ObtencionEstadoMaquinaControlDatosGridView("IDMAQUINAESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaEstadoMaquinaControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaEstadoMaquinaControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarEstadoMaquinaControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.WS_EditarEstadoMaquinaControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarEstadoMaquinaControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarEstadoMaquinaControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region EstadoExistenciasControl
        //Obtiene la cantidad total de agencias
        public static int GetEstadoExistenciasControlCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoExistenciasControlEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoExistenciasControlRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoExistenciasControlRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las Agencias, según paginación
        public static List<dynamic> GetEstadoExistenciasControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "AGEN");
                string json = DataAccess.ObtencionEstadoExistenciasControlDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetEstadoExistenciasControlDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "AGEN");
                string json = DataAccess.ObtencionEstadoExistenciasControlDatosGridView("IDEXISTENCIAESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaEstadoExistenciasControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaEstadoExistenciasControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarEstadoExistenciasControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.WS_EditarEstadoExistenciasControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        
        public static AckResponse EliminarEstadoExistenciasControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarEstadoExistenciasControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }

        public static int getEstadoExistenciasCondicionalesColor(ref List<ConditionalFormattingObject> listaCondicionales)
        {
            try
            {
                DataTable resultado = ConexionSQL.getDataTable("SELECT * FROM TBLEXISTENCIASESTADO");
                if (resultado.Rows.Count == 0)
                {
                    log.Error("Mensaje: SELECT * FROM TBLEXISTENCIASESTADO no ha devuelto ningún valor en ColorEstado() de AcopiosProducción");
                }
                else
                {
                    foreach (DataRow row in resultado.Rows)
                    {
                        ConditionalFormattingObject objCondicion =
                            new ConditionalFormattingObject(row["DESCRIPCION"].ToString(), ConditionTypes.Equal, row["IDEXISTENCIAESTADO"].ToString(), "", false);


                        Color colorcito = new Color();
                        colorcito = Color.FromArgb(Convert.ToInt32(row["COLORAMOSTRAR"]));
                        objCondicion.CellBackColor = colorcito;

                        listaCondicionales.Add(objCondicion);
                    }
                }
                return 1;

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                return -1;
            }
        }

        #endregion
        #region EstadoPedidoControl
        //Obtiene la cantidad total de agencias
        public static int GetEstadoPedidoControlCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoPedidoControlEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoPedidoControlRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoPedidoControlRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las Agencias, según paginación
        public static List<dynamic> GetEstadoPedidoControlDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "AGEN");
                string json = DataAccess.ObtencionEstadoPedidoControlDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetEstadoPedidoControlDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "AGEN");
                string json = DataAccess.ObtencionEstadoPedidoControlDatosGridView("IDPEDIDOPROESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaEstadoPedidoControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaEstadoPedidoControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarEstadoPedidoControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.WS_EditarEstadoPedidoControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarEstadoPedidoControl(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarEstadoPedidoControl(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region EstadoRecepcionControl
        public static AckResponse insertarEnTabla(Dictionary<String,Object> clavesValor, string from)
        {
            AckResponse res;
            String insert = "INSERT INTO "+from+" VALUES( ";
            for (int i = 0; i < clavesValor.Count; i++)
            {
                insert += clavesValor.ElementAt(i).Key;
                if (i == clavesValor.Count - 1) break;
                insert += ",";
            }
            insert += ") VALUES (";

            for (int i = 0; i < clavesValor.Count; i++)
            {
                insert += clavesValor.ElementAt(i).Value.ToString();
                if (i == clavesValor.Count - 1) break;
                insert += ",";
            }
            insert += ")";
            string error = "";

            res = DataAccess.ComponerACKO(ConexionSQL.SQLClienteExec(insert, ref error),error);


            return res;
        }

        public static int getEstadoRecepcionCondicionalesColor(ref List<ConditionalFormattingObject> listaCondicionales)
        {
            try
            {
                DataTable resultado = ConexionSQL.getDataTable("SELECT * FROM TBLRECEPCIONESTADO");
                if (resultado.Rows.Count == 0)
                {
                    log.Error("Mensaje: SELECT * FROM TBLRECEPCIONESTADO no ha devuelto ningún valor en ColorEstado()");
                }
                else
                {
                    foreach (DataRow row in resultado.Rows)
                    {
                        ConditionalFormattingObject objCondicion =
                            new ConditionalFormattingObject(row["DESCRIPCION"].ToString(), ConditionTypes.Equal, row["TBLRECEPCIONESTADO"].ToString(), "", false);


                        Color colorcito = new Color();
                        colorcito = Color.FromArgb(Convert.ToInt32(row["COLORAMOSTRAR"]));
                        objCondicion.CellBackColor = colorcito;

                        listaCondicionales.Add(objCondicion);
                    }
                }
                return 1;

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                return -1;
            }
        }

        #endregion
        #region Bom
        //Obtiene la cantidad total de Bom
        public static int GetBomCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionBomEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetBomRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionBomRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Bom, según paginación
        public static List<dynamic> GetBomDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "BOM");
                string json = DataAccess.WS_ObtencionBomDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetBomDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "BOM");
                string json = DataAccess.ObtencionBomDatosGridView("IDARTICULOPADRE", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un Bom
        public static AckResponse AltaBom(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
            string ack = DataAccess.AltaBom(json);
            ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un Bom
        public static AckResponse EditarBom(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
            string ack = DataAccess.EditarBom(json);
            ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un Bom
        public static AckResponse EliminarBom(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarBom(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region MotivosReg
        public static int GetMotivosRegCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionMotivosRegEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetMotivosRegRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionMotivosRegRegistrosFiltrados(filterExpression);
        }
        public static List<dynamic> GetMotivosRegDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "MOTREG");
                string json = DataAccess.ObtencionMotivosRegDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetMotivosRegDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "MOTREG");
                string json = DataAccess.ObtencionMotivosRegDatosGridView("IDMOTIVO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un MotivosReg
        public static AckResponse AltaMotivosReg(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaMotivosReg(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarMotivosReg(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarMotivosReg(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarMotivosReg(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarMotivosReg(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Ruta
        //Obtiene la cantidad total de Ruta
        public static int GetRutasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionRutasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetRutasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionRutasRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Bom, según paginación
        public static List<dynamic> GetRutasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "RUTAS");
                string json = DataAccess.ObtencionRutasDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRutasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "RUTAS");
                string json = DataAccess.ObtencionRutasDatosGridView("IDRUTA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaRuta(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaRuta(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarRuta(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarRuta(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarRuta(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarRuta(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region RutasPreparacion
        public static int GetRutasPreparacionCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionRutasPreparacionEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetRutasPreparacionRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionRutasPreparacionRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los RutasPreparacion, según paginación
        public static List<dynamic> GetRutasPreparacionDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "RUTASPREP");
                string json = DataAccess.ObtencionRutasPreparacionDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRutasPreparacionDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "RUTASPREP");
                string json = DataAccess.ObtencionRutasPreparacionDatosGridView("IDRUTA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un RutasPreparacion
        public static AckResponse AltaRutaPreparacion(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaRutaPreparacion(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un ZonaIntercambio
        public static AckResponse EditarRutaPreparacion(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarRutaPreparacion(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un Bom
        public static AckResponse EliminarRutaPreparacion(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarRutaPreparacion(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region ZonaIntercambio
        //Obtiene la cantidad total de ZonaIntercambio
        public static int GetZonaIntercambioCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionZonaIntercambioEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetZonaIntercambioRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionZonaIntercambioRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los ZonaIntercambio, según paginación
        public static List<dynamic> GetZonaIntercambioDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "ZON");
                string json = DataAccess.ObtencionZonaIntercambioDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetZonaIntercambioDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "ZON");
                string json = DataAccess.ObtencionZonaIntercambioDatosGridView("ZLORIGEN", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaZonaIntercambio(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaZonaIntercambio(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarZonaIntercambio(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarZonaIntercambio(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarZonaIntercambio(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarZonaIntercambio(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region IncidenciasInventario
        //Obtiene la cantidad total de IncidenciasInventario
        public static int GetIncidenciasInventarioCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionIncidenciasInventarioEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetIncidenciasInventarioRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionIncidenciasInventarioRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las IncidenciasInventario, según paginación
        public static List<dynamic> GetIncidenciasInventarioDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "INCIDENCIASINV");
                string json = DataAccess.ObtencionIncidenciasInventarioDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetIncidenciasInventarioDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "INCIDENCIASINV");
                string json = DataAccess.ObtencionIncidenciasInventarioDatosGridView("IDINCIDENCIA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una IncidenciaInventario
        public static AckResponse AltaIncidenciaInventario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaIncidenciaInventario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica una IncidenciaInventario
        public static AckResponse EditarIncidenciaInventario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarIncidenciaInventario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina una IncidenciaInventario
        public static AckResponse EliminarIncidenciaInventario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarIncidenciaInventario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region TareasPendientes
        public static int GetTareasPendientesCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionTareasPendientesEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetTareasPendientesRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionTareasPendientesRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las TareasPendientes, según paginación
        public static List<dynamic> GetTareasPendientesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "TAREASPEND");
                string json = DataAccess.ObtencionTareasPendientesDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetTareasPendientesDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "TAREASPEND");
                string json = DataAccess.ObtencionTareasPendientesDatosGridView("TIPOTAREA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaTareaPendiente(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaTareaPendiente(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica una TareaPendiente
        public static AckResponse EditarTareaPendiente(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarTareaPendiente(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina una TareaPendiente
        public static AckResponse EliminarTareaPendiente(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarTareaPendiente(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region TareasTipo
        //Obtiene la cantidad total de TareasTipo
        public static int GetTareasTipoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionTareasTipoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetTareasTipoRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionTareasTipoRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las TareasTipo, según paginación
        public static List<dynamic> GetTareasTipoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "TAREASTIPO");
                string json = DataAccess.ObtencionTareasTipoDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetTareasTipoDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "TAREASTIPO");
                string json = DataAccess.ObtencionTareasTipoDatosGridView("TIPOTAREA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una TareaTipo
        public static AckResponse AltaTareaTipo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaTareaTipo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarTareaTipo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarTareaTipo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarTareaTipo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarTareaTipo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region CombiPalet
        public static int GetCombiPaletsCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionCombiPaletsEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetCombiPaletsRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionCombiPaletsRegistrosFiltrados(filterExpression);
        }
        public static List<dynamic> GetCombiPaletsDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "COMBIPALETS");
                string json = DataAccess.ObtencionCombiPaletsDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetCombiPaletsDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "COMBIPALETS");
                string json = DataAccess.ObtencionCombiPaletsDatosGridView("TIPOANCHOA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaCombiPalet(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaCombiPalet(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarCombiPalet(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarCombiPalet(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarCombiPalet(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarCombiPalet(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region PaletsTipo
        public static int GetPaletsTipoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionPaletsTipoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetPaletsTipoRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionPaletsTipoRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los CombiPalets, según paginación
        public static List<dynamic> GetPaletsTipoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "PALETSTIPO");
                string json = DataAccess.ObtencionPaletsTipoDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetPaletsTipoDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PALETSTIPO");
                string json = DataAccess.ObtencionPaletsTipoDatosGridView("IDPALETTIPO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un PaletTipo
        public static AckResponse AltaPaletTipo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaPaletTipo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarPaletTipo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarPaletTipo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarPaletTipo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
            string ack = DataAccess.EliminarPaletTipo(json);
            ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            return ackobject.First();
        }
        #endregion
        #region FormatosSCC
        public static int GetFormatosSCCCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionFormatoSSCCEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetFormatosSCCRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionFormatoSSCCRegistrosFiltrados(filterExpression);
        }
        public static List<dynamic> GetFormatoSSCCDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "FORMATOSSCC");
                string json = DataAccess.ObtencionFormatoSSCCDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetFormatoSSCCDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "FORMATOSSCC");
                string json = DataAccess.ObtencionFormatoSSCCDatosGridView("IDFORMATO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);

            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaFormatoSCC(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaFormatoSCC(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarFormatoSCC(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarFormatoSCC(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarFormatoSCC(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarFormatoSCC(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Lotes
        public static int GetLotesCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionLotesEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetLotesRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionLotesRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Lotes, según paginación
        public static List<dynamic> GetLotesDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "LOT");
                string json = DataAccess.ObtencionLotesDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetLotesDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "LOT");
                string json = DataAccess.WS_ObtencionLotesDatosGridView("IDLOTE", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un Lote
        public static AckResponse AltaLote(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaLote(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un Lote
        public static AckResponse EditarLote(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarLote(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un Lote
        public static AckResponse EliminarLote(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarLote(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Propietarios
        //Obtiene la cantidad total de Propietarios
        public static int GetPropietariosCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionPropietariosEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetPropietariosRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionPropietariosRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Propietarios, según paginación
        public static List<dynamic> GetPropietariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "PROP");
                string json = DataAccess.ObtencionPropietariosDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetPropietariosDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROP");
                string json = DataAccess.ObtencionPropietariosDatosGridView("IDPROPIETARIO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaPropietario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaPropietario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarPropietario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarPropietario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarPropietario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarPropietario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Mantenimiento
        //Obtiene la cantidad total de Mantenimiento
        public static int GetMantenimientoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionMantenimientoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetMantenimientoRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionMantenimientoRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Mantenimiento, según paginación
        public static List<dynamic> GetMantenimientoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "MANT");
                string json = DataAccess.ObtencionMantenimientoDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetMantenimientoDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "MANT");
                string json = DataAccess.ObtencionMantenimientoDatosGridView("IDMANTENIMIENTO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return lstDatos;
        }
        public static AckResponse AltaMantenimiento(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaMantenimiento(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarMantenimiento(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarMantenimiento(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarMantenimiento(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarMantenimiento(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Parametros
        //Obtiene la cantidad total de Parametros
        public static int GetParametrosCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionParametrosEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetParametrosRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionParametrosRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Mantenimiento, según paginación
        public static List<dynamic> GetParametrosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "PARAM");
                string json = DataAccess.ObtencionParametrosDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetParametrosDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PARAM");
                string json = DataAccess.ObtencionParametrosDatosGridView("PARAMETRO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un Parametro
        public static AckResponse AltaParametro(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaParametro(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarParametro(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarMantenimiento(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarParametro(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarParametro(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region ExistenciasHistorico
        //Obtiene la cantidad total de ExistenciasHistorico
        public static int GetExistenciasHistoricoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionExistenciasHistoricoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetExistenciasHistoricoRegistrosFiltrados(string filterExpression, List<TableScheme> lstEsquemaTabla)
        {
            string strRelaciones = string.Empty;
            string strCamposAlias = string.Empty;
            string sortExpression = string.Empty;
            string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "EXISTHIST");
            return DataAccess.ObtencionExistenciasHistoricoRegistrosFiltrados(filterExpression, strRelaciones);
        }

        public static int GetExistenciasHistoricoRegistrosFiltrados(string filterExpression, List<TableScheme> lstEsquemaTabla, bool filtroPersonalizado)
        {
            string strRelaciones = string.Empty;
            string strCamposAlias = string.Empty;
            string sortExpression = string.Empty;
            string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "EXISTHIST");
            return DataAccess.ObtencionExistenciasHistoricoRegistrosFiltrados(filterExpression, strRelaciones, filtroPersonalizado);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Mantenimiento, según paginación
        public static List<dynamic> GetExistenciasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "EXISTHIST");
                string json = DataAccess.ObtencionExistenciasHistoricoDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }

        public static List<dynamic> GetExistenciasHistoricoDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla, string filtroInicial)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "EXISTHIST");
                string json = DataAccess.ObtencionExistenciasHistoricoDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones, filtroInicial);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetExistenciasHistoricoDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "EXISTHIST");
                string json = DataAccess.ObtencionExistenciasHistoricoDatosGridView("IDHISTORICO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON ExistenciasHistorico:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetExistenciasHistoricoDatosGridView(List<TableScheme> lstEsquemaTabla, string filtroInicial)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "EXISTHIST");
                string json = DataAccess.ObtencionExistenciasHistoricoDatosGridView("IDHISTORICO", strCampos, strCamposAlias, strRelaciones, filtroInicial);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON ExistenciasHistorico:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un ExistenciasHistorico
        public static AckResponse AltaExistenciasHistorico(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaExistenciasHistorico(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un ExistenciasHistorico
        public static AckResponse EditarExistenciasHistorico(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarExistenciasHistorico(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un ExistenciasHistorico
        public static AckResponse EliminarExistenciasHistorico(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarExistenciasHistorico(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Visor Log
        //Obtiene la cantidad total de ExistenciasHistorico
        public static int GetRumLogCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionRumLogEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON RumLog:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetRumLogRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionRumLogRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Mantenimiento, según paginación
        public static List<dynamic> GetRumLogDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "RUMLOG");
                string json = DataAccess.ObtencionRumLogDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRumLogDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "RUMLOG");
                string json = DataAccess.ObtencionRumLogDatosGridView("IDLOGOPERACIONES", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON RumLog:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un RumLog
        public static AckResponse AltaRumLog(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaRumLog(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un RumLog
        public static AckResponse EditarRumLog(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarRumLog(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un RumLog
        public static AckResponse EliminarRumLog(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarRumLog(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region Usuarios
        //Obtiene la cantidad total de Usuarios
        public static int GetUsuariosCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionUsuariosEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetUsuariosRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionUsuariosRegistrosFiltrados(filterExpression);
        }
        public static List<dynamic> GetUsuariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "USU");
                string json = DataAccess.ObtencionUsuariosDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetUsuariosDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "USU");
                string json = DataAccess.ObtencionUsuariosDatosGridView("IDUSUARIO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaUsuario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaUsuario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarUsuario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarUsuario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarUsuario(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarUsuario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region EstadoFabricacion 
        public static int GetEstadoFabricacionCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoFabricacionEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoFabricacionRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoFabricacionRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetEstadoFabricacionDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROV");
                string json = DataAccess.ObtencionEstadoFabricacionDatosGridView("IDORDENFABESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region EstadoMaquina 
        public static int GetEstadoMaquinaCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoMaquinaEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoMaquinaRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoMaquinaRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación 
        public static DataTable GetEstadoMaquinaDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROV");
                string json = DataAccess.ObtencionEstadoMaquinaDatosGridView("IDMAQUINAESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region EstadoExistencias
        public static int GetEstadoExistenciasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoExistenciasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoExistenciasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoExistenciasRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación 
        public static DataTable GetEstadoExistenciasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROV");
                string json = DataAccess.ObtencionEstadoExistenciasDatosGridView("IDEXISTENCIAESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region AcopiosProduccion
        //Obtiene la cantidad total de agencias
        public static int GetAcopiosProduccionCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionAcopiosProduccionEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetAcopiosProduccionRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionAcopiosProduccionRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetAcopiosProduccionDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string[] strCampos = GetFieldsSQLAcopiosProduccion(lstEsquemaTabla);
                string json = DataAccess.ObtencionAcopiosProduccionDatosGridView("fc.ORDEN", strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static string[] GetFieldsSQLAcopiosProduccion(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("AcopiosProduccion");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                Debug.WriteLine(lstEsquemaTabla[i].Nombre + " " + lstEsquemaTabla[i].Etiqueta + " " + lstEsquemaTabla[i].EsVisible + " " + lstEsquemaTabla[i].EsModificable);
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    if (lstEsquemaTabla[i].Nombre == "fc.UDSPREVISTASFAB")
                    {
                        fields += lstEsquemaTabla[i].Nombre + " AS 'Previstas Fabricacion',";

                    }
                    else
                    {
                        fields += lstEsquemaTabla[i].Nombre + ",";

                    }
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();

            string selectExpression = "SELECT COUNT (fc.ORDEN) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        #endregion
        #region ArticulosAcopiosProduccion
        //Obtiene la cantidad total de agencias
        public static int GetArticulosAcopiosProduccionCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionArticulosAcopiosProduccionEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetArticulosAcopiosProduccionRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionArticulosAcopiosProduccionRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetArticulosAcopiosProduccionDatosGridView(List<TableScheme> lstEsquemaTabla, string filterExpression)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] strCampos = GetFieldsSQLArticulosAcopiosProduccion(lstEsquemaTabla);
                string json = DataAccess.ObtencionArticulosAcopiosProduccionDatosGridView("ART.IDARTICULO", filterExpression, strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON ArticulosAcopiosProduccion:" + e.Message);
            }
            return lstDatos;
        }
        public static string[] GetFieldsSQLArticulosAcopiosProduccion(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("ArticulosAcopiosProduccion");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonGroup = js.First()["Group By"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            string selectExpression = "SELECT COUNT (art.IDARTICULO) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[4];
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            query[3] = jsonGroup;
            return query;
        }

        #endregion
        #region ArticulosAcopiosProduccionJerarquico
        //Obtiene la cantidad total de agencias
        public static int GetArticulosAcopiosProduccionJerarquicoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionArticulosAcopiosProduccionJerarquicoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetArticulosAcopiosProduccionJerarquicoRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionArticulosAcopiosProduccionRegistrosFiltrados(filterExpression);
        }


        public static DataTable GetArticulosAcopiosProduccionJerarquicoDatosGridView1(int ordenFabricacion,int previstaFabricar)
        {
            DataTable resultado = null;
            try
            {
                string sqlSelect = ";WITH PagingCte AS ( SELECT art.IDARTICULO AS 'ID Articulo',Fc.ORDEN AS 'Orden',ARTOF.REFERENCIA AS 'Referencia Final'," +
                        "ARTOF.DESCRIPCION AS 'Articulo final',ART.IDPRESENTACION AS 'ID Presentacion',fc.IDPEDIDOFAB AS 'Num Orden'," +
                        "ord.IDPEDIDOFABLIN AS 'Linea Orden',"+// coalesce(ord.UDSNECESARIAS,0) AS 'Necesario'," +
                        "CAST(("+previstaFabricar+ " * coalesce(ord.UDSNECESARIAS,0) / isnull(SUM( cast (Fc.udsprevistasfab as bigint)),0)) AS float) AS 'Necesario'," +
                        "CAST (coalesce(sum(rev.CANTIDAD),0) AS float) AS 'Solicitado',"+
                        "coalesce(sum(sal.cantidad),0) AS 'Consumido'," +
                        "coalesce((ord.merma),0) AS '%Merma' , RowNum = ROW_NUMBER() OVER (ORDER BY art.IDARTICULO) FROM  TBLARTICULOS ART 	" +
                        "left join TBLORDENFABRICACIONLIN ord on art.IDARTICULO = ord.IDARTICULO" +
                        " left join TBLORDENFABRICACIONCAB Fc on fc.IDPEDIDOFAB = ord.IDPEDIDOFAB left join TBLSALIDAS sal" +
                        " on sal.IDPEDIDOFAB=fc.IDPEDIDOFAB and sal.IDPEDIDOFABLIN=ord.IDPEDIDOFABLIN left join TBLRESERVAS rev on " +
                        "rev.IDPEDIDOFAB=ord.IDPEDIDOFAB AND rev.IDPEDIDOFABLIN=ord.IDPEDIDOFABLIN left join TBLARTICULOS ARTOF on " +
                        "ARTOF.IDARTICULO=FC.IDARTICULO WHERE fc.IDPEDIDOFAB in(" + ordenFabricacion + ") GROUP BY art.REFERENCIA, ART.[DESCRIPCION]," +
                        " Fc.ORDEN,ord.IDPEDIDOFABLIN, ord.UDSNECESARIAS,art.IDARTICULO,fc.IDPEDIDOFAB,ord.merma,ARTOF.REFERENCIA," +
                        "ARTOF.DESCRIPCION,ART.IDPRESENTACION ) SELECT *  FROM PagingCte";
                resultado = ConexionSQL.getDataTable(sqlSelect);

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return resultado;
        }

        public static DataTable GetArticulosAcopiosProduccionJerarquicoDatosGridView(List<TableScheme> lstEsquemaTabla, int ordenFab,int previstaFabricar)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string[] strCampos = GetFieldsSQLArticulosAcopiosProduccionJerarquico(lstEsquemaTabla,previstaFabricar);
                string json = DataAccess.ObtencionArticulosAcopiosProduccionJerarquicoDatosGridView("art.IDARTICULO", ordenFab, strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static string[] GetFieldsSQLArticulosAcopiosProduccionJerarquico(List<TableScheme> lstEsquemaTabla,int previstaFabricar)
        {
            string json = DataAccess.LoadJson("ArticulosAcopiosProduccionJerarquico");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta.Equals("Necesario"))
                {
                    //fields += "CAST((" + previstaFabricar + " * coalesce(ord.UDSNECESARIAS,0) / isnull(SUM( cast (Fc.udsprevistasfab as bigint)),0)) AS float) AS 'Necesario',";
                    fields += "CAST((" + previstaFabricar + " * coalesce(ord.UDSNECESARIAS,0) / isnull(cast (Fc.udsprevistasfab as bigint),0)) AS float) AS 'Necesario',";
                    continue;
                }
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT (ART.DESCRIPCION) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        #endregion
        #region AcopiosProduccionJerarquicoArticulos
        //Obtiene la cantidad total de agencias
        public static int GetAcopiosProduccionJerarquicoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionAcopiosProduccionJerarquicoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static int GetAcopiosProduccionJerarquicoRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionAcopiosProduccionRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetAcopiosArticulosDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("AcopiosProduccionJerarquico");
                string fullQuery = "SELECT " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idPedidoFab;
            if (!query[3].Equals("")) { 
                fullQuery= fullQuery   +" Group By " + query[3];
            }
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region AcopiosProduccionJerarquicoSalidas
        //Obtiene la cantidad total de agencias
        public static int GetAcopiosProduccionJerarquicoSalidasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionAcopiosProduccionJerarquicoSalidasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static int GetAcopiosProduccionJerarquicoSalidasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionAcopiosProduccionJerarquicoSalidasRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetAcopiosSalidasDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("AcopiosSalidas");
                string fullQuery = "SELECT " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region AcopiosProduccionJerarquicoEntradas
        //Obtiene la cantidad total de agencias
        public static int GetAcopiosProduccionJerarquicoEntradasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionAcopiosProduccionJerarquicoEntradasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static int GetAcopiosProduccionJerarquicoEntradasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionAcopiosProduccionJerarquicoEntradasRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetAcopiosEntradasDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("AcopiosEntradas");
                string fullQuery = "SELECT " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region OrdenesRecogidaPedidoCli
        //Obtiene la cantidad total de agencias
        public static int GetOrdenesRecogidaEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOrdenesRecogidaEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }



            return _cantidadRegistros;
        }
        public static int GetOrdenesRecogidaRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionOrdenesRecogidaRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetOrdenesRecogidaDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLOrdenesRecogida(lstEsquemaTabla);
                string json = DataAccess.ObtencionOrdenesRecogidaDatosGridView("PC.IDPEDIDOCLI", strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenesRecogidaSalidasJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasSalidas");
                string fullQuery = "SELECT Distinct " + query[0] + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenesRecogidaMovBultoJerarquicoDatosGridView(string idagrupacion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasMovBulto");
                string fullQuery = "SELECT Distinct " + query[0] + query[1] + query[2] + idagrupacion;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenesRecogidaLineasJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasLineas");
                string fullQuery = "SELECT Distinct " + query[0] + query[1] + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenesRecogidaReservasJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasReservas");
                string fullQuery = "SELECT Distinct " + query[0] + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenesRecogidaPackingListJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasPackingList");
                string fullQuery = "SELECT Distinct " + query[0] + query[1] + " WHERE S.IDORDENRECOGIDA= " + idPedidoFab + query[2] + "  PL.IDPACKINGLISTPADRE=" + 0 + " and S.IDORDENRECOGIDA=" + idPedidoFab + query[3];
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecogidaCabJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasCabJerarquico");
                string fullQuery = "SELECT Distinct " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetCargaCabDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenCargaCabJerarquico");
                string fullQuery = "SELECT Distinct " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecogidaPedidosLineasJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasLineas");
                string fullQuery = "SELECT Distinct " + query[0] + query[1] + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }      
        public static DataTable GetOrdenesRecogidaControlTareasJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasTareas");
                string fullQuery = "SELECT Distinct " + query[0] + query[1] + " WHERE TP.TIPOTAREA IN ('CA', 'PI','SP','SD','PL','UP', 'OL') AND EXISTS (SELECT 1 FROM TBLRESERVAS re  WHERE re.idtarea = tp.idtarea AND re.idordenrecogida =" + idPedidoFab +" ) OR EXISTS (SELECT 1 FROM TBLMOVIMIENTOSBULTO mb INNER JOIN TBLPACKINGLIST pl ON pl.IDENTIFICADOR = mb.identificador WHERE mb.IDTAREA = tp.idtarea AND pl.idagrupacion =" + idPedidoFab +" )";
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static string[] GetFieldsSQLOrdenRecogidasExpediciones(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("OrdenRecogidasExpediciones");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }

            string jsonFrom = null;
            if (js.First["FROM"] != null)
            {
                jsonFrom = js.First()["FROM"].ToString();
            }

            string jsonWhere = null;
            if (js.First["WHERE"] != null)
            {
                jsonWhere = js.First["WHERE"].ToString();
            }

            string where="";
            if (!string.IsNullOrEmpty(jsonWhere))
            {
                where = " WHERE " + jsonWhere;
            }
            string selectExpression = "SELECT COUNT (ca.idcarga) as NumRegistros FROM " + jsonFrom+where/*+" "+jsonGroup*/;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[3];
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            return query;
        }
        public static string[] GetFieldsSQLOrdenesRecogidaCab(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("OrdenRecogidasCab");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();
            string selectExpression = "SELECT COUNT (o.idordenrecogida) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[3];
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            return query;
        }
        public static DataTable getDatosFechasEntregaLineasPedidos(string where)
        {
            try
            {
                
                String sql = "SELECT DISTINCT PC.IDPEDIDOCLI,PC.REFERENCIA AS Pedido ,PC.SERIE as Serie,PL.FECHAENTREGA as [Fecha Entrega],COUNT (*) AS Líneas FROM TBLPEDIDOSCLILIN " +
                    "PL INNER JOIN TBLPEDIDOSCLICAB PC ON PC.IDPEDIDOCLI = PL.IDPEDIDOCLI " +
                    "WHERE PL.FECHAENTREGA IS NOT NULL AND PL.IDPEDIDOCLI IN ("+
                where + " ) GROUP BY PC.IDPEDIDOCLI,PC.REFERENCIA ,PC.SERIE,PL.fechaentrega order by PL.fechaentrega ";
                return ConexionSQL.getDataTable(sql);
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                throw ex;
            }
        }
        #endregion
        #region operariosMov

        //Obtiene la cantidad total de ExistenciasHistorico
        public static int GetOperariosMovCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOperariosMovEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetOperariosMovRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionOperariosMovRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Mantenimiento, según paginación
        public static List<dynamic> GetOperariosMovDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "OPERMOV");
                string json = DataAccess.ObtencionOperariosMovDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOperariosMovDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "OPERMOV");
                string json = DataAccess.ObtencionOperariosMovDatosGridView("IDMOVIMIENTO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON OperariosMovimientos:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta un OperariosMovimientos
        public static AckResponse AltaOperariosMov(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaOperariosMov(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un OperariosMovimientos
        public static AckResponse EditarOperariosMov(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarOperariosMov(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un OperariosMovimientos
        public static AckResponse EliminarOperariosMov(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarOperariosMov(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region ControlTareas
        //Obtiene la cantidad total de agencias
        public static int GetControlTareasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionControlTareasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static int GetControlTareasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionControlTareasRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetControlTareasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLTareas(lstEsquemaTabla);
                string json = DataAccess.ObtencionControlTareasDatosGridView("TP.IDTAREA", strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return lstDatos;
        }
        public static string[] GetFieldsSQLTareas(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("ControlTareas");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT (TP.IDTAREA) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            JArray jo = JArray.Parse(json);
            string jsonDatos = jo.First()["FROM"].ToString();
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        

        #endregion
        #region ControlTareasJerarquico
        //Obtiene la cantidad total de agencias
        public static int GetControlTareasJerarquicoCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionControlTareasJerarquicoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static int GetControlTareasJerarquicoRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionControlTareasJerarquicoRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetControlTareasJerarquicoDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLTareasJerarquico(lstEsquemaTabla);
                string json = DataAccess.ObtencionControlTareasJerarquicoDatosGridView("TP.IDTAREA", strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return lstDatos;
        }
        public static string[] GetFieldsSQLTareasJerarquico(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("ControlTareasJerarquico");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT (TP.IDTAREA) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            JArray jo = JArray.Parse(json);
            string jsonDatos = jo.First()["FROM"].ToString();
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        #endregion

        #region jerarquiaStock
        //Obtiene la cantidad total de ZonaIntercambio
        public static int GetReservasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionReservasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static int GetReservasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionReservasRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetReservasDatos(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos=new DataTable();
            try
            {
                //string strRelaciones = string.Empty;
                //string strCamposAlias = string.Empty;
                //string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "");
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
               
                string[] strCampos = GetFieldsSQLReservasJerarquia(lstEsquemaTabla);
                string json = DataAccess.ObtencionReservasDatosGridView("IDRESERVA","",strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON ReservasTemplate:" + e.Message);
            }

            return lstDatos;
        }
        
        public static string[] GetFieldsSQLReservasJerarquia(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("ReservasTemplate");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].EsVisible == true)
                {
                    if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                    {
                        fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                    }
                    else
                    {
                        fields += lstEsquemaTabla[i].Nombre + ",";
                    }
                }
            }

            string jsonFrom = js.First()["FROM"].ToString();
            fields = fields.TrimEnd(',');
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }

        #endregion

        #region RecursosTarea
        //Obtiene la cantidad total de Mantenimiento
        public static int GetRecursosTareaCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionRecursosTareaEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetRecursosTareaRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionRecursosTareaRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Mantenimiento, según paginación
        public static List<dynamic> GetRecursosTareaDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "MANT");
                string json = DataAccess.ObtencionRecursosTareaDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecursosTareaDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "MANT");
                string json = DataAccess.ObtencionRecursosTareaDatosGridView("IDRECURSO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecursosJerarquicoTarea(string filter,string nombreJsonJerarquico,string nombreCampoId)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string filterExpression = string.Empty;
                string sortExpression = string.Empty;
                string[] strCampos = GetFieldSQLDatos(nombreJsonJerarquico);
                DataTable json = DataAccess.ObtencionDatosJerarquico(nombreJsonJerarquico,nombreCampoId, strCampos, strRelaciones, strCamposAlias, filter);
                //JArray jo = JArray.Parse(json);
                //string jsonEsquema = jo.First()["Scheme"].ToString();
                //string jsonDatos = jo.First()["Data"].ToString();
                //lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
                return json;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }


        public static AckResponse AltaRecursosTarea(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaRecursosTarea(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarRecursosTarea(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarRecursosTarea(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarRecursosTarea(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarRecursosTarea(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region UsuariosGrupos
        public static int GetUsuariosGruposCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionUsuariosGruposEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetUsuariosGruposRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionUsuariosGruposRegistrosFiltrados(filterExpression);
        }
        public static List<dynamic> GetUsuariosGruposDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "USUGRUP");
                string json = DataAccess.ObtencionUsuariosGruposDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetUsuariosGruposDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string sortExpression = string.Empty;
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "USUGRUP");
                string json = DataAccess.ObtencionUsuariosGruposDatosGridView("IDUSUARIO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaUsuarioGrupo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaUsuario(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditarUsuarioGrupo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarUsuarioGrupo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarUsuarioGrupo(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarUsuarioGrupo(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region PedidosProveedor 
        public static int GetPedidosProveedorCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionPedidoProveedorEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON ProveedoresPedidosCab:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetPedidosProveedorRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionPedidoProveedorRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetPedidosProveedorDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                Debug.WriteLine("Inicio GetPedidosProveedorDatosGridView ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));
                log.Info("Inicio GetPedidosProveedorDatosGridView ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));
                string campoAlias = string.Empty;
                string strRelaciones = string.Empty;
                //string[] strCampos = GetFieldsSQLProvPedidos(lstEsquemaTabla);
                string strCampos = GetFieldsSQLineasPedido(lstEsquemaTabla,ref campoAlias,ref strRelaciones,"pro");

                string json = DataAccess.ObtencionPedidoProveedorDatosGridView("IDPEDIDOPRO", strCampos, campoAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
                log.Info("Fin GetPedidosProveedorDatosGridView ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));
                Debug.WriteLine("Inicio GetPedidosProveedorDatosGridView ProveedoresPedidosCabGridView " + DateTime.Now.ToString("HH:mm:ss:ffff"));
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetPedidosProveedorJerarquicoRecepcionesEsquemaUnion(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionPedidosJerarquicoRecepcionesUnionEsquema(ref lstEsquemaTabla);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static DataTable GetPedidosProveedorJerarquicoRecepcionesCantidad(List<TableScheme> lstEsquemaTabla, List<TableScheme> lstEsquemaTablaUnion, string filter)
        {
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff")+ " Principio GetPedidosProveedorJerarquicoRecepcionesCantidad");
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strRelacionesUnion = string.Empty;
                string strCamposAliasUnion = string.Empty;
                string[] strCampos = GetFieldsSQLRecepciones(lstEsquemaTabla, "RecepcionesCab");
                string[] strCamposUnion = GetFieldsSQLRecepciones(lstEsquemaTablaUnion, "PedidosProvJerarquicoRecepciones");
                //string json = DataAccess.ObtencionPedidoJerarquicoRecepciones("IDRECEPCION", strCampos,strCamposUnion, strRelaciones,strRelacionesUnion, strCamposAlias,strCamposAliasUnion,filter);
                lstDatos = DataAccess.ObtencionRecepcionesCantidad(strCampos, strCamposUnion, filter);
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Salida GetPedidosProveedorJerarquicoRecepcionesCantidad");
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetPedidosProveedorJerarquicoRecepciones(List<TableScheme> lstEsquemaTabla, List<TableScheme> lstEsquemaTablaUnion,string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strRelacionesUnion = string.Empty;
                string strCamposAliasUnion = string.Empty;
                string[] strCampos = GetFieldsSQLRecepciones(lstEsquemaTabla, "RecepcionesCab");
                string[] strCamposUnion = GetFieldsSQLRecepciones(lstEsquemaTablaUnion, "PedidosProvJerarquicoRecepciones");
                //string json = DataAccess.ObtencionPedidoJerarquicoRecepciones("IDRECEPCION", strCampos,strCamposUnion, strRelaciones,strRelacionesUnion, strCamposAlias,strCamposAliasUnion,filter);
                string json = DataAccess.ObtencionPedidoJerarquicoRecepciones("IDRECEPCION", strCampos, strCamposUnion, strRelaciones, strRelacionesUnion, strCamposAlias, strCamposAliasUnion, filter);

                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetPedidosProJerarquicoEntradasEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionPedidosProJerarquicoEntradasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static DataTable GetPedidosProveedorJerarquicoEntradas(List<TableScheme> lstEsquemaTabla, string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLineasPedido(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "ENTR");
                string json = DataAccess.ObtencionPedidoJerarquicoEntradas("IDENTRADA", strCampos, strRelaciones, strCamposAlias, filter);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetPedidosProJerarquicoLineasEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionPedidosProJerarquicoLineasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static DataTable GetPedidosProveedorJerarquicoLineas(List<TableScheme> lstEsquemaTabla, string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLineasPedido(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "LIN");
                string json = DataAccess.ObtencionPedidoJerarquicoLineas("IDPEDIDOPRO", strCampos, strRelaciones, strCamposAlias, filter);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON PedidosProvJerarquicoLineas:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetPedidosProJerarquicoPreavisosEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionPedidosProJerarquicoPreavisosEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static DataTable GetPedidosProveedorJerarquicoPreavisos(List<TableScheme> lstEsquemaTabla, string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string filterExpression = string.Empty;
                string sortExpression = string.Empty;
                string strCampos = GetFieldsSQLineasPedido(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "");
                string json = DataAccess.ObtencionPedidoJerarquicoPreavisos("IDENTRADA", strCampos, strRelaciones, strCamposAlias, filter);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }



        public static DataTable GetRecepcionesProveedorJerar(List<TableScheme> lstEsquemaTabla, List<TableScheme> lstEsquemaTablaUnion, string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strRelacionesUnion = string.Empty;
                string strCamposAliasUnion = string.Empty;
                string strCampos = GetFieldsSQLineasPedido(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "RECEP");
                string strCamposUnion = GetFieldsSQLGridView(lstEsquemaTablaUnion, ref strCamposAliasUnion, ref strRelacionesUnion, "RECEP");
                //string json = DataAccess.ObtencionPedidoJerarquicoRecepciones("IDRECEPCION", strCampos, strCamposUnion, strRelaciones, strRelacionesUnion, strCamposAlias, strCamposAliasUnion, filter);
                //JArray jo = JArray.Parse(json);
                //string jsonEsquema = jo.First()["Scheme"].ToString();
                //string jsonDatos = jo.First()["Data"].ToString();
                //lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }

        public static DataTable GetRecepcionesJerarquicoLineas(string idRecepcion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLUnion("RecepcionesJerarquicoLineas", "RecepcionesJerarquicoLineasUnion");
                DataTable tabla = DataAccess.ObtencionRecepcionesJerarquicoLineas(query, idRecepcion);
                return tabla;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecepcionesJerarquicoEntradas(string idRecepcion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLUnion("RecepcionesJerarquicoEntradas", "RecepcionesJerarquicoEntradasUnion");
                DataTable tabla = DataAccess.ObtencionRecepcionesJerarquicoEntradas(query, idRecepcion);
                return tabla;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecepcionesJerarquicoExistencias(string idRecepcion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLUnion("RecepcionesJerarquicoExistencias", "RecepcionesJerarquicoExistenciasUnion");
                DataTable tabla = DataAccess.ObtencionRecepcionesJerarquicoExistencias(query, idRecepcion);
                return tabla;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json:" + e.Message);
            }
            return lstDatos;
        }

        public static DataTable GetRecepcionesJerarquicoMovimientos(string idRecepcion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLUnion("RecepcionesJerarquicoMovimientos", "RecepcionesJerarquicoMovimientosUnion");
                DataTable tabla = DataAccess.ObtencionRecepcionesJerarquicoMovimientos(query, idRecepcion);
                return tabla;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json:" + e.Message);
            }
            return lstDatos;
        }


        public static DataTable GetRecepcionesJerarquicoTareas(string idRecepcion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLRecepcionesTareas("RecepcionesJerarquicoTareas");
                DataTable tabla = DataAccess.ObtencionRecepcionesJerarquicoTareas(query, idRecepcion);
                return tabla;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecepcionesJerarquicoPreavisos(string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string filterExpression = string.Empty;
                string sortExpression = string.Empty;
                string[] strCampos = GetFieldSQLRecepcionesTareas("RecepcionesJerarquicoPreavisos");
                DataTable json = DataAccess.ObtencionRecepcionesJerarquicoPreavisos("IDENTIFICADOR", strCampos, strRelaciones, strCamposAlias, filter);
                //JArray jo = JArray.Parse(json);
                //string jsonEsquema = jo.First()["Scheme"].ToString();
                //string jsonDatos = jo.First()["Data"].ToString();
                //lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
                return json;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecepcionesJerarquicoEmbalajes(string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string filterExpression = string.Empty;
                string sortExpression = string.Empty;
                string[] strCampos =  GetFieldSQLRecepcionesTareas("RecepcionesJerarquicoEmbalajes");
                DataTable json = DataAccess.ObtencionRecepcionesJerarquicoEmbalajes("IDMOVIMIENTO", strCampos, strRelaciones, strCamposAlias, filter);
                //JArray jo = JArray.Parse(json);
                //string jsonEsquema = jo.First()["Scheme"].ToString();
                //string jsonDatos = jo.First()["Data"].ToString();
                //lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
                return json;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }

        public static AckResponse AltaPedidoProveedor(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaPedidoProveedor(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EditaPedidorProveedor(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarPedidoProveedor(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static AckResponse EliminarPedidoProveedor(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarPedidoProveedor(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        public static string[] GetFieldsSQLRecepciones(List<TableScheme> lstEsquemaTabla, string name)
        {
            string json = DataAccess.LoadJson(name);
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First["WHERE"].ToString();

            //string selectExpression = "SELECT COUNT (IDRECEPCION) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            //string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            //json = json.Replace("\"[DATA]\"", datosJSON);
            //lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[3];
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            return query;
        }
        public static string[] GetFieldsSQLRecepcionesPorAlbaranPedido()
        {
            string json = DataAccess.LoadJson("RecepcionesPorAlbaran");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First["WHERE"].ToString();
            string selectExpression = "SELECT COUNT (IDPEDIDOPRO) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            return query;
        }
        public static string[] GetFieldSQLRecepcionesTareas(string nombreJson)
        {
            string json = DataAccess.LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();


            string[] query = new string[4];
            fields = fields.TrimEnd(',');
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            if (js.First()["Where2"] != null)
            {
                string jsonWhere2 = js.First()["Where2"].ToString();
                query[3] = jsonWhere2;
            }

            //string selectExpression = "SELECT COUNT (IDRECEPCION) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            //string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            //json = json.Replace("\"[DATA]\"", datosJSON);
            //lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);

            return query;
        }

        public static string[] GetFieldSQLDatos(string nombreJson)
        {
            string json = DataAccess.LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();


            string[] query = new string[4];
            fields = fields.TrimEnd(',');
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            if (js.First()["Where2"] != null)
            {
                string jsonWhere2 = js.First()["Where2"].ToString();
                query[3] = jsonWhere2;
            }

            //string selectExpression = "SELECT COUNT (IDRECEPCION) as NumRegistros FROM " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            //string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            //json = json.Replace("\"[DATA]\"", datosJSON);
            //lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);

            return query;
        }

        private static string GetFieldsSQLineasPedido(List<TableScheme> lstEsquemaTabla, ref string strCamposAlias, ref string strRelaciones, string aliasTabla)
        {
            string fields = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                //if (!lstEsquemaTabla[i].EsGrid)
                //{
                if (lstEsquemaTabla[i].EsFK)
                {
                    //Componer Outer Apply
                    string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                    strRelaciones += " " + oaAux + " ";
                    fields += "OA" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoMostrado + " AS " + lstEsquemaTabla[i].Nombre;
                    if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                    {
                        strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                    }
                    else
                    {
                        strCamposAlias += lstEsquemaTabla[i].Nombre;
                    }
                }
                else
                {

                    if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                    {
                        fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                        //strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                    }
                    else
                    {
                        fields += lstEsquemaTabla[i].Nombre;
                        strCamposAlias += lstEsquemaTabla[i].Nombre;
                    }
                }
                if (i < lstEsquemaTabla.Count - 1)
                {
                    fields += ",";
                    strCamposAlias += ",";
                }
            }

            return fields;
        }
        #endregion
        #region Recepciones
        public static int GetRecepcionesCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionRecepcionesEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON RecepcionesCab:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetRecepcionesRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionRecepcionesRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetRecepcionesDatosGridView(List<TableScheme> lstEsquemaTabla,List<TableScheme> lstEsquemaTablaUnion,string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strRelacionesUnion = string.Empty;
                string strCamposAliasUnion = string.Empty;
                string[] strCampos = GetFieldsSQLRecepciones(lstEsquemaTabla,"RecepcionesCab");
                string[] strCamposUnion = GetFieldsSQLRecepciones(lstEsquemaTablaUnion, "PedidosProvJerarquicoRecepciones");
                string json = DataAccess.ObtencionRecepciones("IDRECEPCION", strCampos, strCamposUnion, strRelaciones, strRelacionesUnion, strCamposAlias, strCamposAliasUnion, filter);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON: RecepcionesCab PedidosProvJerarquicoRecepciones" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region EstadoTareas
        public static int GetEstadoTareasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEstadoTareasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return _cantidadRegistros;
        }
        public static int GetEstadoTareasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionEstadoTareasRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación 
        public static DataTable GetEstadoTareasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROV");
                string json = DataAccess.ObtencionEstadoTareasDatosGridView("IDTAREAESTADO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return lstDatos;
        }
        #endregion
        #region TipoTareas
        public static int GetTipoTareasCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionTipoTareasEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return _cantidadRegistros;
        }
        public static int GetTipoTareasRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionTipoTareasRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación 
        public static DataTable GeTipoTareasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PROV");
                string json = DataAccess.ObtencionTipoTareasDatosGridView("TIPOTAREA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return lstDatos;
        }
        #endregion
        #region Operarios
        public static int GetOperariosCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOperariosEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return _cantidadRegistros;
        }

        public static int GetOperariosRegistrosFiltrados(string filterExpression)
        {
            //return DataAccess.ObtencionLotesRegistrosFiltrados(filterExpression);
            return DataAccess.ObtencionOperariosRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los Lotes, según paginación
        public static List<dynamic> GetOperariosViejosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "OP");
                string json = DataAccess.ObtencionOperariosDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return lstDatos;
        }

        public static List<dynamic> GetOperariosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "OP");
                string json = DataAccess.ObtencionOperariosDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        

        //A partir de un registro dinámico, da de alta un Lote
        public static AckResponse AltaOperarios(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaOperarios(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un Lote
        public static AckResponse EditarOperarios(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarOperarios(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un Lote
        public static AckResponse EliminarOperarios(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarOperarios(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
      
        #endregion
        #region Grupos
        //Obtiene la cantidad total de maquinas
        public static int GetGruposCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionGruposEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return _cantidadRegistros;
        }
        public static int GetGruposRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionGruposRegistrosFiltrados(filterExpression);
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de las maquinas, según paginación
        public static List<dynamic> GetGruposDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "GRUP");
                string json = DataAccess.ObtencionGruposDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return lstDatos;
        }

        public static List<dynamic> GetGruposDatosCombo(List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "GRUP");
                string json = DataAccess.ObtencionGruposDatosGridView("IDGRUPO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return lstDatos;
        }
        public static DataTable GetGruposDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "GRUP");
                string json = DataAccess.ObtencionGruposDatosGridView("IDGRUPO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una maquina
        public static AckResponse AltaGrupos(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaGrupos(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica una maquina
        public static AckResponse EditarGrupos(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarGrupos(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina una maquina
        public static AckResponse EliminarGrupos(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarGrupos(json);
                //Devolver objeto ACK deserializado en una clase nueva           
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region ZonaLogCab
        public static int GetZonaLogCabCantidad(ref List<TableScheme> lstEsquemaTabla, ref GridScheme esquemaGrid)
        {
            int _cantidadRegistros = 0;
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;

                string json = DataAccess.ObtencionZonaLogCabEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
                string jsonGridEsquema = jo.First()["DataGridLin"].ToString();
                if (!string.IsNullOrEmpty(jsonGridEsquema))
                {
                    esquemaGrid = JsonConvert.DeserializeObject<GridScheme>(jsonGridEsquema);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static DataTable GetZonaLogCabDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "ZONACAB");
                string json = DataAccess.ObtencionZonaLogCabDatosGridView("IDZONACAB", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetZonaLogCabRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionZonaLogCabRegistrosFiltrados(filterExpression);
        }
        public static List<dynamic> GetZonaLogCabDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "ZONACAB");
                string json = DataAccess.ObtencionZonaLogCabDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static AckResponse AltaZonaCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaZonaCab(json, false);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        /*public static AckResponse AltaZonaCabYLineas(List<TableScheme> lstEsquemaTabla, dynamic values, GridScheme EsquemaGrid)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "], \r\n\"DataGridLin\":[" + JsonConvert.SerializeObject(EsquemaGrid) + "]}]";
                string ack = DataAccess.AltaClientePedidoCab(json, true);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }*/
        public static AckResponse EditarZonaCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarZonaCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un ClientesPedidosCab
        public static AckResponse EliminarZonaCab(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarZonaCab(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion
        #region ZonaLogLin
        //Obtiene la cantidad total de ZonaLogLinea
        public static int GetZonaLogLineasCantidad(ref List<TableScheme> lstEsquemaTabla, string _filtro)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionZonaLogLineasEsquema(_filtro);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        //Obtiene los datos, en modo de listado de objetos dinámicos, de los proveedores, según paginación
        public static List<dynamic> GetZonaLogLineasDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "ZONALIN");
                string json = DataAccess.ObtencionZonaLogLineasDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                //lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        /*public static DataTable GetClientesPedidosLineasJerarquicoDatosGridView(string idPedidoFab)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("OrdenRecogidasClienteLineas");
                string fullQuery = "SELECT Distinct " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + idPedidoFab;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }*/
        public static DataTable GetZonaLogLineasDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "ZONALIN");
                string json = DataAccess.ObtencionZonaLogLineasDatosGridView("IDZONALIN", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        //A partir de un registro dinámico, da de alta una Zona Linea
        public static AckResponse AltaZonaLogLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.AltaZonaLogLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica una Zona Linea
        public static AckResponse EditarZonaLogLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
                string ack = DataAccess.EditarZonaLogLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina una Zona Linea
        public static AckResponse EliminarZonaLogLineas(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            List<AckResponse> ackobject = new List<AckResponse>();
            try
            {
                string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
                string ack = DataAccess.EliminarZonaLogLineas(json);
                ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return ackobject.First();
        }
        #endregion

        #region Stock
        public static int GetStockCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionStockEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static List<dynamic> GetStockDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos=new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                if (sortExpression == string.Empty)
                {
                    sortExpression = " ORDER BY " + lstEsquemaTabla[0].Nombre + " ASC";
                }
                string[] strCampos = GetFieldsSQLStock(lstEsquemaTabla);
                string json = DataAccess.ObtencionStockDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetStockDatosGridView(List<TableScheme> lstEsquemaTabla,string filterExpression)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] strCampos = GetFieldsSQLStock(lstEsquemaTabla);
                string json = DataAccess.ObtencionStockDatosGridView("ex.IDENTRADA ASC", filterExpression, strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);


            }



            return lstDatos;
        }

        public static string[] GetFieldsSQLStock(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].EsVisible == true)
                {
                    if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                    {
                        fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                    }
                    else
                    {
                        fields += lstEsquemaTabla[i].Nombre + ",";
                    }
                }
            }

            string jsonFrom = js.First()["FROM"].ToString();
            fields = fields.TrimEnd(',');
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        public static string[] GetFieldsSQLStockGridView(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("Stock");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            fields = fields.TrimEnd(',');
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        public static DataTable GetStockActual()
        {
            String query = "";
            try
            {
                query = "SELECT  exi.[IDENTRADA] ,exi.[IDARTICULO] ,exi.[IDUNIDADTIPO] ,exi.[CANTIDADUNIDAD], exi.cantidad ,exi.IDHUECO "+
                            " ,[IDEXISTENCIAESTADO], exi.[ATRIB1]  ,exi.[ATRIB2] ,exi.[ATRIB3],exi.[ATRIB4] "+
                            " ,ent.[EAN],ent.SSCC,ent.FECHACADUCIDAD, ent.LOTE, art.DESCRIPCION , art.referencia,  hu.descripcion as UBICACION "+
                             "FROM[TBLEXISTENCIAS] exi join tblentradas ent on exi.IDENTRADA=ent.IDENTRADA  join TBLARTICULOS art on exi.IDARTICULO= art.IDARTICULO " +
                             " join tblhuecos hu on hu.idhueco=exi.idhueco";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
                throw new Exception(e.Message + "\n " + query);
            }
        }
        #endregion
        #region OrdenesRecogidaCab
        //Obtiene la cantidad total de agencias
        public static int GetOrdenesRecogidaCabEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOrdenesRecogidaCabEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetOrdenesRecogidaCabRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionOrdenesRecogidaCabRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetOrdenesRecogidaCabDatosGridView(List<TableScheme> lstEsquemaTabla,string filter)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLOrdenesRecogidaCab(lstEsquemaTabla);
                string json = DataAccess.ObtencionOrdenesRecogidaCabDatosGridView("o.idordenrecogida", strCampos,filter);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenesRecogidaNoMarcadasCabDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLOrdenesRecogidaCab(lstEsquemaTabla);
                string json = DataAccess.ObtencionOrdenesRecogidaCabNoMarcadasDatosGridView("o.idordenrecogida", strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static string[] GetFieldsSQLOrdenesRecogida(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("OrdenesRecogida");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT (pc.IDPEDIDOCLI) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            JArray jo = JArray.Parse(json);
            string jsonDatos = jo.First()["FROM"].ToString();
            string jsonWhere = "";
            if (jo.First()["WHERE"] != null)
            {
                jsonWhere= "WHERE " + jo.First()["WHERE"].ToString();
            }
            string[] query = new string[3];
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            return query;
        }
        #endregion
        #region OrdenesCarga
        //Obtiene la cantidad total de cargas
        public static int GetOrdenRecogidaExpedicionesEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOrdenRecogidaExpedicionesEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static int GetOrdenRecogidaExpedicionesRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionOrdenRecogidaExpedicionesRegistrosFiltrados(filterExpression);
        }
        public static DataTable GetOrdenRecogidaExpedicionesDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLOrdenRecogidasExpediciones(lstEsquemaTabla);
                string json = DataAccess.ObtencionOrdenRecogidaExpedicionesDatosGridView("ca.idcarga", strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON OrdenRecogidasExpediciones:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenRecogidaExpedicionesPLPteDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldSQLUnion("OrdenesCargaPackingListPte", "OrdenesCargaPackingListPadrePte");
                string json = DataAccess.ObtencionOrdenesCargaPackingListPteDatosGridView();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }

        public static int GetOrdenesCargaPackingListPteEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOrdenesCargaPackingListPteEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                if (_Datos.Count == 0)
                {
                    return 0;
                }
                dynamic d = JObject.Parse(_Datos[0].ToString());
               //CAMBIAR _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON OrdenesCargaPackingListPte:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static int GetOrdenesCargaJerarquicoPackingListEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOrdenesCargaJerarquicoPackingListEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static DataTable GetOrdenCargaJerarquicoPackingList(List<TableScheme> lstEsquemaTabla,string valor)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLJerarquicoLoadGenerico("OrdenesCargaJerarquicoPackingList");
                string json = DataAccess.ObtencionOrdenesCargaJerarquicoPackingList("cl.identificador", strCampos,valor);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetOrdenesCargaJerarquicoPedidosEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOrdenesCargaJerarquicoPedidosEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static DataTable GetOrdenCargaJerarquicoPedidos(List<TableScheme> lstEsquemaTabla, string valor)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLJerarquicoLoadGenerico("OrdenesCargaJerarquicoPedidos");
                string json = DataAccess.ObtencionOrdenesCargaJerarquicoPedidos(strCampos, valor);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetOrdenesCargaJerarquicoContenidoEsquema(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionOrdenesCargaJerarquicoContenidoEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return _cantidadRegistros;
        }
        public static DataTable GetOrdenCargaJerarquicoContenido(List<TableScheme> lstEsquemaTabla, string valor)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLJerarquicoLoadGenerico("OrdenesCargaJerarquicoContenido");
                string json = DataAccess.ObtencionOrdenesCargaJerarquicoContenido(strCampos, valor);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        #endregion

        #region Stock Status

        public static int GetStockStatusCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionStockStatusEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static DataTable GetStockStatusDatosGridView(List<TableScheme> lstEsquemaTabla, string filterExpression)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "STOCKSTA");
                string json = DataAccess.ObtencionStockStatusDatosGridView("IDEXISTENCIA", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static List<dynamic> GetStockStatusDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "RESHIST");
                string json = DataAccess.ObtencionStockStatusDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
       
        public static DataTable GetStockStatus(DateTime fecha)
        {
            string tFecha = "{ ts '" + fecha.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'}";
            String query = "";
            try
            {
                query= "SELECT ss.[DIA] ,ss.[IDEXISTENCIA],ent.sscc,art.REFERENCIA,art.DESCRIPCION , ent.lote,ent.FECHACADUCIDAD,ent.ATRIB1 " + 
                        " ,ss.[CANTIDAD] ,ubi.DESCRIPCION,ss.[SERIEALB],ss.[NALB] "+
                        " FROM [TBLSTOCKSTATUS] SS left outer join tblentradas ent on ss.idexistencia=ent.IDENTRADA "+
                        " left outer join TBLARTICULOS art on ent.IDARTICULO= art.IDARTICULO " +
                        " left outer join tblhuecos ubi on ss.IDHUECO= ubi.IDHUECO "+
                        " where dia = " + tFecha+ "";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error("Error:" + e.Message);
                throw new Exception(e.Message + "\n " + query);
            }
        }
        #endregion
        #region Devoluciones Proveedor
        public static DataTable GetDevolucionesProveedor()
        {
            DataTable tabla = DataAccess.WS_ObtencionDevolucionesProveedor();
            return tabla;
        }
        #endregion
        #region permisos
        public static int GetPermisosCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.ObtencionPermisosEsquema();
            int _cantidadRegistros = 0;
            JArray jo = JArray.Parse(json);
            string jsonEsquema = jo.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string jsonDatos = jo.First()["Data"].ToString();
            IList<JToken> _Datos = jo.First()["Data"].ToList();
            dynamic d = JObject.Parse(_Datos[0].ToString());
            _cantidadRegistros = d.NumRegistros;
            return _cantidadRegistros;
        }
        public static List<dynamic> GetPermisosDatos(string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, "PERM");
                string json = DataAccess.ObtencionPermisosDatos(sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
                //System.Threading.Thread.Sleep(1500); //DELAY obligado para mostrar la barra de navegación, si se desea
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetPermisosDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "PERM");
                string json = DataAccess.ObtencionPermisosDatosGridView("IDGRUPO", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        public static int GetPermisosRegistrosFiltrados(string filterExpression)
        {
            return DataAccess.ObtencionPermisosRegistrosFiltrados(filterExpression);
        }
        public static AckResponse AltaPermiso(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
            string ack = DataAccess.AltaPermiso(json);
            //Devolver objeto ACK deserializado en una clase nueva           
            List<AckResponse> ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            return ackobject.First();
        }
        //A partir de un registro dinámico, modifica un Usuario
        public static AckResponse EditarPermiso(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + (string)values + "]\r\n}]";
            string ack = DataAccess.EditarPermiso(json);
            //Devolver objeto ACK deserializado en una clase nueva           
            List<AckResponse> ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            return ackobject.First();
        }
        //A partir de un registro dinámico, elimina un Usuario
        public static AckResponse EliminarPermiso(List<TableScheme> lstEsquemaTabla, dynamic values)
        {
            string json = "[{\r\n \"Scheme\":" + JsonConvert.SerializeObject(lstEsquemaTabla) + ", \r\n\"Data\":[" + JsonConvert.SerializeObject(values) + "]\r\n}]";
            string ack = DataAccess.EliminarPermiso(json);
            //Devolver objeto ACK deserializado en una clase nueva           
            List<AckResponse> ackobject = JsonConvert.DeserializeObject<List<AckResponse>>(ack);
            return ackobject.First();
        }
        #endregion //NO IMPLEMENTADO
        #region operarios
        private static string GetFieldsSQLOperariosGridView(List<TableScheme> lstEsquemaTabla, ref string strCamposAlias, ref string strRelaciones, string aliasTabla)
        {
            string fields = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                //if (!lstEsquemaTabla[i].EsGrid)
                //{
                if (lstEsquemaTabla[i].EsFK)
                {
                    //Componer Outer Apply
                    string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                    strRelaciones += " " + oaAux + " ";
                    fields += "OA" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoMostrado + " AS " + lstEsquemaTabla[i].Nombre;
                    if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                    {
                        strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                    }
                    else
                    {
                        strCamposAlias += lstEsquemaTabla[i].Nombre;
                    }
                }
                else
                {
                    if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                    {
                        fields += lstEsquemaTabla[i].Nombre;
                        strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                    }
                    else
                    {
                        fields += lstEsquemaTabla[i].Nombre;
                        strCamposAlias += lstEsquemaTabla[i].Nombre;
                    }
                }
                if (i < lstEsquemaTabla.Count - 1)
                {
                    fields += ",";
                    strCamposAlias += ",";
                }
            }
            return fields;
        }

        public static string[] GetFieldsSQLOperarios(List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson("Operarios");
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT (IDOPERARIO) as NumRegistros From " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            JArray jo = JArray.Parse(json);
            string jsonDatos = jo.First()["FROM"].ToString();
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        public static DataTable getOperarios()
        {
            string query= "SELECT IDOPERARIO ,NOMBRE  from tbloperarios order by id operario"; 
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw new Exception(e.Message + " " + query);
            }
        }
        #endregion

        #region Recepciones
        public static int GetRecepcionesPendientesCantidad(ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionRecepcionesPendientesEsquema();
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return _cantidadRegistros;
        }
        public static DataTable GetRecepcionesPendientesDatosGridView(List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string strCampos = GetFieldsSQLGridView(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, "RECEP");
                string json = DataAccess.ObtencionRecepcionesPendientesDatosGridView("IDRECEPCION", strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return lstDatos;
        }
        public static DataTable GetRecepcionesAlbaranPedido()
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] fields = GetFieldsSQLRecepcionesPorAlbaranPedido();
                //DataTable tabla = DataAccess.Obtencions
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }
            return lstDatos;
        }
        public static DataTable GetRecepcionesAlbaranEntradas(string idRecepcion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLUnion("gridRecepcionesAlbaranEntradas", "gridRecepcionesAlbaranEntradas2");
                DataTable tabla = DataAccess.ObtencionRecepcionesAlbaranGridEntradas(query, idRecepcion);
                return tabla;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        //public static DataTable GetRecepcionesAlbaranExistencias()
        //{
        //    DataTable lstDatos = new DataTable();
        //    try
        //    {
        //        string[] query = GetFieldSQLUnion("gridRecepcionesAlbaranExistencias", "gridRecepcionesAlbaranExistencias2");
        //        string fullQuery = "(SELECT " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + ") union (SELECT DISTINCT " + query[3] + " FROM " + query[4] + " WHERE " + query[5];
        //        Debug.WriteLine("Existencias:   "+fullQuery);
        //    }
        //    catch (Newtonsoft.Json.JsonReaderException e)
        //    {
        //        log.Error("Error desearilzando json:" + e.Message);
        //    }
        //    return lstDatos;
        //}
        public static DataTable GetRecepcionesLineas()
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string camposAlias = string.Empty;
                string relaciones = string.Empty;
                string[] query = GetFieldSQLRecepcionesTareas("gridRecepcionesAlbaranLineas");
                string json = DataAccess.LoadJson("gridRecepcionesAlbaranLineas");
                JArray js = JArray.Parse(json);
                string jsonGroup = js.First()["Group"].ToString();
                string fullQuery = "SELECT " + query[0] + " FROM " + query[1] + " GROUP BY " + jsonGroup /*+ " WHERE " + query[2] +idRecepcion*//*+ " union (SELECT " + query[3] + " FROM " + query[4] +*//* " WHERE " + query[5]+idRecepcion+*/;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecepcionesAlbaranTareas(string idRecepcion)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLRecepcionesTareas("gridRecepcionesAlbaranTareas");
                string fullQuery = "SELECT " + query[0] + " FROM " + query[1] + " WHERE " + query[2] + "'" + idRecepcion + "')"/*+query[3]+idRecepcion+")"*/;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetRecepcionesAlbaranPreavisos()
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldSQLRecepcionesTareas("gridRecepcionesAlbaranPreavisos");
                string fullQuery = "SELECT " + query[0] + " FROM " + query[1];
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error desearilzando json:" + e.Message);
            }
            return lstDatos;
        }
        #endregion
        #region funciones refactorizadas generales
        public static int GetCantidad(string name)
        {
            string json = DataAccess.ObtencionEsquema(name);
            int _cantidadRegistros = 0;
            JArray jo = JArray.Parse(json);
            string jsonEsquema = jo.First()["Scheme"].ToString();
            string jsonDatos = jo.First()["Data"].ToString();
            IList<JToken> _Datos = jo.First()["Data"].ToList();
            dynamic d = JObject.Parse(_Datos[0].ToString());
            _cantidadRegistros = d.NumRegistros;
            return _cantidadRegistros;
        }

        public static int GetDatosRegistrosFiltrados(string filterExpression,string nombreJson)
        {
            //return DataAccess.ObtencionLotesRegistrosFiltrados(filterExpression);
            return DataAccess.ObtencionDatosRegistrosFiltrados(filterExpression,nombreJson);
        }


        public static int GetDatosCantidad(ref List<TableScheme> lstEsquemaTabla,String nombreDelEsquema)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEsquema(nombreDelEsquema);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                ExceptionManager.GestionarError(e);
            }
            return _cantidadRegistros;
        }
        public static DataTable GetDatosGridView(List<TableScheme> lstEsquemaTabla,String nombreJson,String nombreCampoIdJson)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLDatos(lstEsquemaTabla,nombreJson, nombreCampoIdJson);
                string json = DataAccess.ObtencionDatosGridView(nombreJson,nombreCampoIdJson, strCampos);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }

            return lstDatos;
        }
        #endregion

        #region operariosMov
        
        //Obtiene la cantidad total de Usuarios
        public static DataTable GetOperMov(DateTime fechaDesde, int idOperario)
        {
            return GetOperMov(fechaDesde.AddHours(fechaDesde.Hour * -1), fechaDesde.AddHours(24 - fechaDesde.Hour), idOperario);
        }
        public static DataTable GetTareasTipo()
        {
            string query = "";
            try
            {
                query = "SELECT  [IDRESERVATIPO],[DuracionMedia],[DuracionMinima] ,[DuracionMaxima] ,[DuracionDesviacion]" +
                    " FROM TBLRESERVASTIPO";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static DataTable GetOperMov(DateTime fechaDesde, DateTime fechaHasta, int idOperario)
        {
            string query="";
            string tFechaDesde = "{ ts '" + fechaDesde.ToString("yyyy-MM-dd HH:mm:ss") + "'}";
            string tFechaHasta = "{ ts '" + fechaHasta.ToString("yyyy-MM-dd HH:mm:ss") + "'}";

            try
            {

                string filtro = "where OPM.fechahora between " + tFechaDesde + " and " + tFechaHasta + " and OPM.idoperario=" + idOperario.ToString()+" ";
                query = Utilidades.lecturaJsonSelect("OperariosMovimientos", filtro,"", " order by OPM.fechahora ");
                /*string queryAnterior = "SELECT 'ok' as estado, 'tarea' as tarea, om.[IDMOVIMIENTO] ," +
                    "om.[IDOPERARIO] , op.NOMBRE,om.[IDRECURSO],om.[IDHUECOORIGEN]," +
                    "ho.DESCRIPCION a, om.[IDSALIDA] ,om.[TIPOMOVIMIENTO], rt.DESCRIPCION as tipo_movimiento " +
                    ",om.[MSGERROR] ,om.[PLIST] ,coalesce(om.[SEGUNDOS] , 0) as segundos," +
                    " om.[FECHAHORA] ,om.[UNIDADES],om.[IDPEDIDO] " +
                    " ,om.[IDPEDIDOLIN] " +
                    ",om.[MOVPAREJA] " +
                    ",om.[IDRESERVA] " +
                    ",om.[IDPLISTORIGEN]," +
                    "om.[IDMAQUINA]" +
                    ", om.modificado  " +
                    ", dateadd(s,om.segundos,0) as Duracion "+
                    "FROM [dbo].[TBLOPERARIOSMOV] om left outer join tbloperarios op on op.IDOPERARIO = om.IDOPERARIO " +
                    "left outer join TBLRESERVASTIPO rt on om.TIPOMOVIMIENTO = rt.IDRESERVATIPO "+
                    "left outer join TBLHUECOS Ho on om.IDHUECOORIGEN = ho.IDHUECO "+
                    "left outer join TBLHUECOS hd on om.IDHUECODESTINO = hd.IDHUECO "+
                    "left outer join TBLENTRADAS en on om.IDENTRADA = en.identrada "+
                    "left outer join tblarticulos ar on en.idarticulo = ar.idarticulo "+
                    "where fechahora between "+ tFechaDesde + " and " + tFechaHasta + " and om.idoperario="+ idOperario.ToString() +
                    " order by fechahora ";*/
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message+ " " + query);
                throw new Exception(e.Message+ " " +query);
            }
        }
        public static void SetOperMov(DataTable dtOperMovUpdtd)
        {
            try
            {
                foreach (DataRow fila in dtOperMovUpdtd.Rows)
                {
                    switch (fila["modificado"])
                    {
                        case "Si":
                        case "Descartado":
                            OperMovUpdt(fila);
                            break;
                        case "No":
                            OperMovAdd(fila);
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message );
            }
        }
        private static void OperMovUpdt(DataRow fila)
        {
            RumLog rml = RumLog.getRumLog("Productividad", "Calculo avanzado tiempos");
            

            string query = "Update tblOperariosMov ";
            string error = "";
            try
            {
                DateTime fechaHora = Convert.ToDateTime(fila["Fecha Hora"]);
                string tFechaDesde = "{ ts '" + fechaHora.ToString("yyyy-MM-dd HH:mm:ss") + "'}";
                query +=" set fechahora= " + tFechaDesde;
                query += " , modificado= '" + fila["Modificado"] +"'";
                query += " , segundos= " + fila["segundos"] ;
                query += " where idmovimiento= " + Convert.ToString(fila["ID Movimiento"]);
                ConexionSQL.SQLClienteExec(query, ref error);
                rml.parametros = query;
                RumLog.EscribirRumLog(rml);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message+ "\n"+ error);
            }
        }
        private static void OperMovAdd(DataRow fila)
        {
            string query = "insert into TBLOPERARIOSMOV (IDOPERARIO ,IDRECURSO ,IDHUECOORIGEN  ,IDHUECODESTINO  ,IDENTRADA " +
                        ", IDSALIDA, TIPOMOVIMIENTO, MSGERROR, PLIST, SEGUNDOS, FECHAHORA, UNIDADES, IDPEDIDO " +
                        ", IDPEDIDOLIN, MOVPAREJA, IDRESERVA, IDPLISTORIGEN, IDMAQUINA, modificado)  " +
                        " values( ";
           
            string error = "";
            try
            {
                DateTime fechaHora = Convert.ToDateTime(fila["fecha Hora"]);
                string tFechaDesde = "{ ts '" + fechaHora.ToString("yyyy-MM-dd HH:mm:ss") + "'}";
                query += fila["operario"] + " , ";
                query += fila["Recurso"] + " , ";
                query += fila["Hueco Origen"] + " , ";
                query += fila["Hueco Destino"] + " , ";
                query += fila["ID ENTRADA"] + " , ";
                query += fila["ID SALIDA"] + " , ";
                query += "'" + fila["tipoMovimiento"] + "' , ";
                query += "'" + fila["msgError"] + "' , ";
                query += fila["Packing List"] + " , ";
                if (fila["Mov Pareja"] is DBNull)
                    query += " NULL , ";
                else
                    query += Convert.ToInt32(fila["SEGUNDOS"]).ToString() + " , ";
                query += tFechaDesde+ " , ";
                query += fila["unidades"] + " , ";
                query += fila["idPedido"] + " , ";
                query += fila["idPedidoLin"] + " , ";
                if (fila["Mov Pareja"] is DBNull)
                    query += " NULL , ";
                else
                    query += fila["Mov Pareja"] + " , ";
                query += fila["idReserva"] + " , ";
                query += fila["IDPLISTORIGEN"] + " , ";
                if (fila["IDMAQUINA"] is DBNull)
                    query +=  " NULL , ";
                else
                    query += fila["IDMAQUINA"] + " , ";
                query += "'" + fila["modificado"] + "' ) ";
                ConexionSQL.SQLClienteExec(query, ref error);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n" + error);
            }
        }
        public static DataTable GetListaOperarios()
        {
            string query = "";
            try
            {
                query = "select idOperario, Nombre from tbloperarios order by idOperario";
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static DataTable GetDescripcionMovimientos()
        {
            string query = "select rt.idreservatipo as tipoMovimiento, rt.DESCRIPCION as tipo_movimiento from TBLRESERVASTIPO rt";
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        #endregion

        #region inventario
        public static DataTable getIncidenciasInventarioPrimeraPasada(int idhuecoalmacen, int aceradesde, int acerahasta, int portaldesde, int portalhasta, int pisodesde, int pisohasta)
        {
            String sql = DataAccess.GetIncidenciasPrimeraPasada(idhuecoalmacen, aceradesde, acerahasta, portaldesde, portalhasta, pisodesde, pisohasta);
            return DataAccess.GetSeleccion(sql);
        }
        #endregion
        #region Funciones Genéricas
        //Obtiene los datos para cargar un combo genérico (pasado por parámetro)
        public static int GetMayorElemento(string elemento, string tabla, string filtro)
        {
            int _cantidadRegistros = 0;
            string json = DataAccess.ObtencionMayorElemento(elemento, tabla, filtro);                    
            JArray jo = JArray.Parse(json);
            IList<JToken> _Datos = jo.ToList();
            dynamic d = JObject.Parse(_Datos[0].ToString());
            if (d.VALOR != null)
            {
                _cantidadRegistros = d.VALOR;
            }
            return _cantidadRegistros;
        }
        //Obtiene los datos para cargar un combo genérico (pasado por parámetro)
        public static DataTable GetDatosComboMulti(string elemento, string valor, string tabla)
        {
            return DataAccess.ObtencionDatosComboMulti(elemento, valor, tabla);
        }
        public static DataTable GetDatosComboMulti(string elemento, string valor, string tabla, string filtro)
        {
            return DataAccess.ObtencionDatosComboMulti(elemento, valor, tabla, filtro);
            
        }

        public static List<ElementoValor> GetDatosCombo(string elemento, string valor, string tabla)
        {
            string json = DataAccess.ObtencionDatosCombo(elemento, valor, tabla);
            List<ElementoValor> lstDatos;
            lstDatos = JsonConvert.DeserializeObject<List<ElementoValor>>(json);
            return lstDatos;
        }
        public static List<ElementoValor> GetDatosCombo(string elemento, string valor, string tabla, string filtro)
        {
            string json = DataAccess.ObtencionDatosCombo(elemento, valor, tabla, filtro);
            List<ElementoValor> lstDatos;
            lstDatos = JsonConvert.DeserializeObject<List<ElementoValor>>(json);
            return lstDatos;
        }
        public static List<ElementoValor> GetDatosComboSQL(string elemento, string valor, string from)
        {
            string json = DataAccess.ObtencionDatosComboSQL(elemento, valor, from);
            List<ElementoValor> lstDatos;
            lstDatos = JsonConvert.DeserializeObject<List<ElementoValor>>(json);
            return lstDatos;
        }
        private static string GetFieldsSQL(List<TableScheme> lstEsquemaTabla, ref string strCampoAlias, ref string strRelaciones, ref string sortExpression, ref string filterExpression, string aliasTabla)
        {
            string fields = string.Empty;
                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        //Componer Outer Apply
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        strRelaciones += " " + oaAux + " ";
                        fields += "OA" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoMostrado + " AS " + lstEsquemaTabla[i].Nombre;
                        strCampoAlias += lstEsquemaTabla[i].Nombre;
                        sortExpression = sortExpression.Replace("[" + lstEsquemaTabla[i].Nombre + "]", "OA" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoMostrado);
                    filterExpression = filterExpression.Replace("[" + lstEsquemaTabla[i].Nombre + "]", "OA" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoMostrado);
                }
                else
                {
                    if (!aliasTabla.Equals(""))
                    {
                        fields += aliasTabla + "." + lstEsquemaTabla[i].Nombre;
                    }
                    else
                    {
                        fields += lstEsquemaTabla[i].Nombre;
                    }
                    strCampoAlias += lstEsquemaTabla[i].Nombre;
                }
                if (i < lstEsquemaTabla.Count - 1)
                    {
                        fields += ",";
                        strCampoAlias += ",";
                    }
                }
                return fields;
            }
        /**
         * Alias FROM se puede dejar Vacio si solo apunta a una tabla.
         * Alias table tiene que contener aunque sea el nombre de la tabla
         */
        public static List<dynamic> GetDatos(string nombreJson,string aliasFrom,string aliasTabla, string sortExpression, string filterExpression, int pagInicial, int pagFinal, List<TableScheme> lstEsquemaTabla)
        {
            List<dynamic> lstDatos = new List<dynamic>();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                if (!filterExpression.Contains("WHERE") && filterExpression!=string.Empty)
                {
                    filterExpression = " WHERE " + filterExpression;
                }
                string strCampos = GetFieldsSQL(lstEsquemaTabla, ref strCamposAlias, ref strRelaciones, ref sortExpression, ref filterExpression, aliasTabla);
                string json = DataAccess.ObtencionDatos(nombreJson,aliasFrom,sortExpression, filterExpression, pagInicial, pagFinal, strCampos, strCamposAlias, strRelaciones);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<List<dynamic>>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }
        /**
         * El nombreCampoIdJson debe incluir la referencia al from, por ejemplo, si el from principal es 
         * "TBLOPERARIOS OP" pondremos OP.IDOPERARIO
         */
        public static string[] GetFieldsSQLDatos(List<TableScheme> lstEsquemaTabla, string nombreJson, string nombreCampoIdJson)
        {
            string json = DataAccess.LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT ("+ nombreCampoIdJson + ") as NumRegistros From " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            JArray jo = JArray.Parse(json);
            string jsonDatos = jo.First()["FROM"].ToString();
            string[] query = new string[2];
            query[0] = fields;
            query[1] = jsonFrom;
            return query;
        }
        private static string GetFieldsSQLGridView(List<TableScheme> lstEsquemaTabla, ref string strCamposAlias, ref string strRelaciones, string aliasTabla)
        {
            string fields = string.Empty;
                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    //if (!lstEsquemaTabla[i].EsGrid)
                    //{
                    if (lstEsquemaTabla[i].EsFK)
                    {
                        //Componer Outer Apply
                        string oaAux = "OUTER APPLY (SELECT " + lstEsquemaTabla[i].CmbObject.CampoMostrado + " FROM " + lstEsquemaTabla[i].CmbObject.TablaRelacionada + " TREL" + i.ToString() + " WHERE " + "TREL" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoRelacionado + " = " + aliasTabla + "." + lstEsquemaTabla[i].Nombre + ") OA" + i.ToString();
                        strRelaciones += " " + oaAux + " ";
                        fields += "OA" + i.ToString() + "." + lstEsquemaTabla[i].CmbObject.CampoMostrado + " AS " + lstEsquemaTabla[i].Nombre;
                        if (lstEsquemaTabla[i].Etiqueta!=string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                        {
                            strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta+"'";
                        }
                        else
                        {
                            strCamposAlias += lstEsquemaTabla[i].Nombre;
                        }
                    }
                    else
                    {
                        if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                        {
                        if (string.IsNullOrEmpty(aliasTabla))
                        {
                            fields +=  lstEsquemaTabla[i].Nombre;
                            strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                        }
                        else
                        {
                            fields += aliasTabla + "." + lstEsquemaTabla[i].Nombre;
                            strCamposAlias += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "'";
                        }
                        }
                        else
                        {
                            fields += aliasTabla + "." + lstEsquemaTabla[i].Nombre;
                            strCamposAlias += lstEsquemaTabla[i].Nombre;
                        }
                    }
                    if (i < lstEsquemaTabla.Count - 1)
                    {
                        fields += ",";
                        strCamposAlias += ",";
                    }
                }
            return fields;
        }
        private static string[] GetFieldsSQLJerarquicoLoadGenerico(string nombreJson)
        {
            string json = DataAccess.LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string jsonWhere = js.First()["WHERE"].ToString();            
            string jsonGroup = "";
            if (js.First()["Group By"] != null)
            {
                         jsonGroup = js.First()["Group By"].ToString();
            }

            string[] query = new string[4];
            fields = fields.TrimEnd(',');
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            query[3] = jsonGroup;
            
            
            return query;
        }


        public static string[] GetFieldSQLUnion(string nombreJson1, string nombreJson2)
        {
            string json1 = DataAccess.LoadJson(nombreJson1);
            string json2 = DataAccess.LoadJson(nombreJson2);
            JArray js = JArray.Parse(json1);
            JArray js2 = JArray.Parse(json2);
            string jsonEsquema = js.First()["Scheme"].ToString();
            string jsonEsquema2 = js2.First()["Scheme"].ToString();
            List<TableScheme> esquema1 = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            List<TableScheme> esquema2 = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema2);

            string fields = string.Empty;
            string fields2 = string.Empty;
            for (int i = 0; i < esquema1.Count; i++)
            {
                if (esquema1[i].Etiqueta != string.Empty && esquema1[i].Etiqueta != null)
                {
                    fields += esquema1[i].Nombre + " AS '" + esquema1[i].Etiqueta + "',";
                }
                else
                {
                    fields += esquema1[i].Nombre + ",";
                }
            }
            for (int i = 0; i < esquema2.Count; i++)
            {
                if (esquema2[i].Etiqueta != string.Empty && esquema2[i].Etiqueta != null)
                {
                    fields2 += esquema2[i].Nombre + " AS '" + esquema2[i].Etiqueta + "',";
                }
                else
                {
                    fields2 += esquema2[i].Nombre + ",";
                }
            }
            string[] query = new string[9];

            string jsonFrom = js.First()["FROM"].ToString();
            string jsonFromUnion = js2.First()["FROM"].ToString();
            string jsonFromUnion2;
            string jsonWhere = js.First()["WHERE"].ToString();
            string jsonWhere2 = js2.First()["WHERE"].ToString();
            string groupby, groupby2;
            try
            {
                if (js.First()["Group"] != null && !js.First()["Group"].ToString().Equals(String.Empty))
                {
                    groupby = js.First()["Group"].ToString();
                    query[6] = groupby;
                }

            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }
            try
            {
                if (js.First()["Group"] != null && !js.First()["Group"].ToString().Equals(String.Empty))
                {
                    groupby2 = js2.First()["Group"].ToString();
                    query[7] = groupby2;
                }
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }
            try
            {
                if (js2.First()["From2"] != null)
                {
                    jsonFromUnion2 = js2.First()["From2"].ToString();
                    query[8] = jsonFromUnion2;
                }
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
            }

            string selectExpression = "SELECT DISTINCT " + jsonFrom/*+" "+jsonWhere+" "+jsonGroup*/;
            fields = fields.TrimEnd(',');
            fields2 = fields2.TrimEnd(',');
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            query[3] = fields2;
            query[4] = jsonFromUnion;
            query[5] = jsonWhere2;
            return query;
        }
        public static int GetEsquemaGenerico(string nombreJson,ref List<TableScheme> lstEsquemaTabla)
        {
            int _cantidadRegistros = 0;
            try
            {
                string json = DataAccess.ObtencionEsquemaGenerico(nombreJson);
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
                string jsonDatos = jo.First()["Data"].ToString();
                IList<JToken> _Datos = jo.First()["Data"].ToList();
                dynamic d = JObject.Parse(_Datos[0].ToString());
                _cantidadRegistros = d.NumRegistros;
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON :"+ nombreJson+ " ERROR:" + e.Message);
            }



            return _cantidadRegistros;
        }
        public static string[] GetFieldsSQL(string nombreJson,List<TableScheme> lstEsquemaTabla)
        {
            string json = DataAccess.LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            string fields = string.Empty;
            string from = string.Empty;
            for (int i = 0; i < lstEsquemaTabla.Count; i++)
            {
                if (lstEsquemaTabla[i].Etiqueta != string.Empty && lstEsquemaTabla[i].Etiqueta != null)
                {
                    fields += lstEsquemaTabla[i].Nombre + " AS '" + lstEsquemaTabla[i].Etiqueta + "',";
                }
                else
                {
                    fields += lstEsquemaTabla[i].Nombre + ",";
                }
            }
            string jsonFrom = js.First()["FROM"].ToString();
            string selectExpression = "SELECT COUNT (*) as NumRegistros FROM " + jsonFrom;
            string datosJSON = ConexionSQL.SQLClientLoadJSON(selectExpression);
            json = json.Replace("\"[DATA]\"", datosJSON);
            lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(json);
            fields = fields.TrimEnd(',');
            JArray jo = JArray.Parse(json);
            string jsonDatos = jo.First()["FROM"].ToString();
            string jsonWhere = "";
            if (jo.First()["WHERE"] != null)
            {
                jsonWhere = "Where " + jo.First()["WHERE"].ToString();
            }
            string[] query = new string[3];
            query[0] = fields;
            query[1] = jsonFrom;
            query[2] = jsonWhere;
            return query;
        }
        public static DataTable GetDatosGridView(string nombreJson,string sortExpression,List<TableScheme> lstEsquemaTabla)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQL( nombreJson, lstEsquemaTabla);
                string json = DataAccess.ObtencionDatosGridView(nombreJson,sortExpression, strCampos); // TODO revisar ordenacion
                JArray jo = JArray.Parse(json);
                string jsonEsquema = jo.First()["Scheme"].ToString();
                string jsonDatos = jo.First()["Data"].ToString();
                lstDatos = JsonConvert.DeserializeObject<DataTable>(jsonDatos);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON :" + nombreJson +" "+ e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetOrdenCargaPedidosResumen(List<TableScheme> lstEsquemaTabla, string valor)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string strRelaciones = string.Empty;
                string strCamposAlias = string.Empty;
                string[] strCampos = GetFieldsSQLJerarquicoLoadGenerico("OrdenesCargaPedidosResumen");
                string query = DataAccess.ObtencionOrdenCargaPedidosResumenSql(strCampos, valor);
                return ConexionSQL.getDataTable(query);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando JSON:" + e.Message);
            }
            return lstDatos;
        }       
        public static DataTable GetDatosReserva(int idReserva)
        {
            return DataAccess.getDatosReserva(idReserva);
        }
        public static DataTable GetDatosExistencia(int idEntrada)
        {
            return DataAccess.getDatosExistencia(idEntrada);
        }
        public static DataTable GetDatosSalida(int idSalida)
        {
            return DataAccess.getDatosSalida(idSalida);
        }
        public static DataTable GetAcopiosConsumirLineas(string ordenes)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("AcopiosConsumir");
                string where = query[2].Replace("[ORDENES]", ordenes);
                string fullQuery = "SELECT Distinct " + query[0] + " FROM " + query[1] + " WHERE " +where;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json AcopiosConsumir: " + e.Message);
            }
            return lstDatos;
        }
        public static DataTable GetAcopiosConsumirLineasJerarquico(string ordenes)
        {
            DataTable lstDatos = new DataTable();
            try
            {
                string[] query = GetFieldsSQLJerarquicoLoadGenerico("AcopiosConsumirJeraquico");
                string where = query[2].Replace("[ORDENES]", ordenes);
                string fullQuery = "SELECT Distinct " + query[0] + " FROM " + query[1] + " WHERE " + where;
                lstDatos = ConexionSQL.getDataTable(fullQuery);
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                log.Error("Error deserializando json AcopiosConsumir: " + e.Message);
            }
            return lstDatos;
        }
        #endregion
        public static DataTable GetWarnings(string query,int idOperario)
        {
            
            try
            {
                return DataAccess.GetSeleccion(query);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + query);
                throw new Exception(e.Message + " " + query);
            }
        }
        public static object getValorDefectoCampoTabla(string tabla, string campo)
        {

            try
            {
                return DataAccess.getValorDefectoCampoTabla(tabla, campo);
            }
            catch (Exception e)
            {
                log.Error(e.Message + " " + tabla);
                throw new Exception(e.Message + " " + tabla);
            }
        }







        #endregion
    }
}

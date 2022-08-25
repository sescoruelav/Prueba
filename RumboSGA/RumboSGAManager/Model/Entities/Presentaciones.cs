using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RumboSGAManager.Model.Entities
{
    public class Presentaciones
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DataTable dtPresentaciones = null;
        private static DataTable dtConversionesVisualizacion = null;
        private static DataTable dtConversionesAlmacenamiento = null;
        private static string UNIDADALMACENAMIENTO = "UNIDADALMACENAMIENTO";
        private static string UNIDADVISUALIZACION = "UNIDADVISUALIZACION";
       
        public DataRow[] GetPresentacionesArticulo(int idPresentacion)
        {

            try
            {
                //log.Debug("Iniciamos la carga de la presentacion " + idPresentacion + " al GetPresentacionesArticulo");
                if (dtPresentaciones == null)
                {
                    dtPresentaciones = ConexionSQL.getDataTable("SELECT * FROM dbo.TBLPRESENTACIONESLIN");
                }
                DataRow[] dtPres = dtPresentaciones.Select("IDPRESENTACION=" + idPresentacion);
                return dtPres;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //log.Debug("Fin función GetPresentacionesArticulo Presentacion" + idPresentacion);
            }
        }
        public static DataRow GetPresentacionVisualizacionArticulo(int idPresentacion)
        {
            DataRow pres = null;
            try
            {
                //log.Debug("Iniciamos la carga de la presentacion " + idPresentacion + " al GetPresentacionesArticulo");
                if (dtPresentaciones == null)
                {
                    dtPresentaciones = ConexionSQL.getDataTable("SELECT * FROM dbo.TBLPRESENTACIONESLIN");
                }
                DataRow[] dtPres = dtPresentaciones.Select("IDPRESENTACION=" + idPresentacion + " AND UNIDADVISUALIZACION=1");
                if (dtPres.Count() > 0)
                {
                    pres = dtPres[0];
                }
                return pres;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //log.Debug("Fin función GetPresentacionesArticulo Presentacion" + idPresentacion);
            }
        }
       
        public static DataRow GetPresentacionAlmacenamientoArticulo(int idPresentacion)
        {
            DataRow pres = null;
            try
            {
               // log.Debug("Iniciamos la carga de la presentacion " + idPresentacion + " al GetPresentacionesArticulo");
                if (dtPresentaciones == null)
                {
                    dtPresentaciones = ConexionSQL.getDataTable("SELECT * FROM dbo.TBLPRESENTACIONESLIN");
                }
                DataRow[] dtPres = dtPresentaciones.Select("IDPRESENTACION=" + idPresentacion + " AND UNIDADALMACENAMIENTO=1");
                if (dtPres.Count() > 0)
                {
                    pres = dtPres[0];
                }
                return pres;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //log.Debug("Fin función GetPresentacionesArticulo Presentacion" + idPresentacion);
            }
        }

        public static object[] getCantidadPresentacionVisualizacion(int idPresentacion, int cantidad)
        {
            double cantVis = cantidad;
            object[] presentacion = new object[2];
            try
            {
                DataRow pres = GetPresentacionVisualizacionArticulo(idPresentacion);
                if (pres!=null)
                {
                    //DataRow pres = drPresVis[0];
                    double conversion = Convert.ToDouble(pres["conversion"]);
                    cantVis = cantidad / conversion;
                    presentacion[0] = cantVis;
                    presentacion[1] = pres["idunidadtipodestino"];
                }
                else
                {
                    presentacion[0] = cantVis;
                    presentacion[1] = "UN";
                }

            } catch (Exception ex)
            {
                log.Error("Error aplicando conversion presentacion");
            }
            return presentacion;
        }

        public static object[] getCantidadPresentacionVisualizacionArticulo(int idArticulo, decimal cantidad)
        {
            decimal cantVis = cantidad;
            object[] presentacion = new object[2];
            int decimales = 6;//Parametrizar
            try
            {
                if (Persistencia.getParametroInt("DECIMALESVISUALIZACION") > 0)
                {
                    decimales = Persistencia.getParametroInt("DECIMALESVISUALIZACION");
                }
                DataRow pres = GetVisualizacion(idArticulo);
                if (pres != null)
                {
                    //DataRow pres = drPresVis[0];
                    decimal conversion =(decimal) pres["convtotal"];
                    cantVis = cantidad * conversion;
                    cantVis = Math.Round(cantVis,decimales);
                    presentacion[0] = cantVis;
                    presentacion[1] = pres["codigodestino"].ToString();
                }
                else
                {
                    presentacion[0] = cantVis;
                    presentacion[1] = "UN";
                }

            }
            catch (Exception ex)
            {
                log.Error("Error aplicando conversion presentacion");
            }
            return presentacion;
        }
        public static object[] getCantidadPresentacionAlmacenamientoArticulo(int idArticulo, decimal cantidad)
        {
            decimal cantVis = cantidad;
            object[] presentacion = new object[2];
            
            try
            {
               
                DataRow pres = GetAlmacenamiento(idArticulo);
                if (pres != null)
                {
                    //DataRow pres = drPresVis[0];
                    decimal conversion = (decimal)pres["convtotal"];
                    cantVis = cantidad * conversion;
                    cantVis = Math.Round(cantVis, 0);
                    presentacion[0] = Int64.Parse(cantVis.ToString());
                    presentacion[1] = pres["codigodestino"].ToString();
                }
                else
                {
                    presentacion[0] = Int64.Parse(cantVis.ToString());
                    presentacion[1] = "UN";
                }

            }
            catch (Exception ex)
            {
                log.Error("Error aplicando conversion presentacion");
            }
            return presentacion;
        }

        public static object[] getCantidadVisualizacion(int idPresentacion, int cantidad)
        {
            double cantVis = cantidad;
            object[] presentacion = new object[2];
            try
            {
                DataRow pres = GetPresentacionVisualizacionArticulo(idPresentacion);
                if (pres != null)
                {
                    //DataRow pres = drPresVis[0];
                    double conversion = Convert.ToDouble(pres["conversion"]);
                    cantVis = cantidad / conversion;
                    presentacion[0] = cantVis;
                    presentacion[1] = pres["idunidadtipodestino"];
                }
                else
                {
                    presentacion[0] = cantVis;
                    presentacion[1] = "UN";
                }

            }
            catch (Exception ex)
            {
                log.Error("Error aplicando conversion presentacion");
            }
            return presentacion;
        }
        public static int getCantidadPresentacionAlmacenamiento(int idPresentacion,string idUnidadTipoOrigen, double cantidad)
        {
            double cantAlm = cantidad;
            int cantidadDev = Convert.ToInt32(Math.Ceiling(cantAlm));

            try
            {
                DataRow presAlm = GetPresentacionAlmacenamientoArticulo(idPresentacion);                
                DataRow[] drPresentaciones = GetPresentacionesLin(idPresentacion);
                if (presAlm != null  && drPresentaciones.Count() > 0)
                {

                    cantidadDev = Convert.ToInt32(GetConversion(drPresentaciones, idUnidadTipoOrigen, presAlm["idunidadtipodestino"].ToString(), cantidad));
                }

            }
            catch (Exception ex)
            {
                log.Error("Error aplicando conversion presentacion");
            }
            return cantidadDev;
        }
            public static int getCantidadPresentacionAlmacenamiento(int idPresentacion, double cantidad)
        {
            double cantAlm = cantidad;
            int cantidadDev = Convert.ToInt32(Math.Ceiling(cantAlm));

            try
            {
                DataRow presAlm = GetPresentacionAlmacenamientoArticulo(idPresentacion);
                DataRow presVis = GetPresentacionVisualizacionArticulo(idPresentacion);
                DataRow[] drPresentaciones = GetPresentacionesLin(idPresentacion);
                if (presAlm!=null && presVis != null && drPresentaciones.Count() > 0)
                {
                    
                    cantidadDev = Convert.ToInt32(GetConversion(drPresentaciones, presVis["idunidadtipodestino"].ToString(), presAlm["idunidadtipodestino"].ToString(), cantidad));
                }

            }
            catch (Exception ex)
            {
                log.Error("Error aplicando conversion presentacion");
            }
            return cantidadDev;
        }

        private static DataRow[] GetPresentacionesLin(int idPresentacion)
        {
            try
            {
                //log.Debug("Iniciamos la carga de la presentacion " + idPresentacion + " al GetPresentacionesArticulo");
                if (dtPresentaciones == null)
                {
                    dtPresentaciones = ConexionSQL.getDataTable("SELECT * FROM dbo.TBLPRESENTACIONESLIN");
                }
                DataRow[] dtPres = dtPresentaciones.Select("IDPRESENTACION=" + idPresentacion);
                return dtPres;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //log.Debug("Fin función GetPresentacionesArticulo Presentacion" + idPresentacion);
            }
        }

        public static double GetConversion(DataRow[] lineas, String idunidadOrigen, String idUnidadDestino, double cantidad)
        {
            try
            {
                if (lineas == null || lineas.Count() == 0)
                {
                    return cantidad;
                }
                double conversion = GetFactorDestino(lineas, idunidadOrigen, idUnidadDestino);
                double i = cantidad * conversion;
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public static DataRow GetVisualizacion(int idArticulo)
        {
            try
            {
                if (dtConversionesVisualizacion == null)
                {
                    dtConversionesVisualizacion = ConexionSQL.getDataTable("SELECT PR.* FROM TBLARTICULOS a JOIN FNTPRESENTACIONUNIDADES('UNIDADALMACENAMIENTO','UNIDADVISUALIZACION') PR ON a.IDARTICULO=PR.IDARTICULO");
                }
                DataRow[] lineas = dtConversionesVisualizacion.Select("IDARTICULO=" + idArticulo);
                if (lineas.Length>0)
                return lineas[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        
        public static DataRow GetAlmacenamiento(int idArticulo)
        {
            try
            {
                if (dtConversionesAlmacenamiento == null)
                {
                    dtConversionesAlmacenamiento = ConexionSQL.getDataTable("SELECT PR.* FROM TBLARTICULOS a JOIN FNTPRESENTACIONUNIDADES('UNIDADVISUALIZACION','UNIDADALMACENAMIENTO') PR ON a.IDARTICULO=PR.IDARTICULO");
                }
                DataRow[] lineas = dtConversionesAlmacenamiento.Select("IDARTICULO=" + idArticulo);
                if (lineas.Length > 0)
                    return lineas[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }

        public static double GetFactorDestino(DataRow[] lineas, String idunidadOrigen, String idUnidadDestino)
        {
            double aUnidades = GetFactorNucleo(lineas, "UN", idunidadOrigen);
            double aDestino = GetFactorNucleo(lineas, "UN", idUnidadDestino);
            double i = aUnidades / aDestino;
            return i;
        }
        private static double GetFactorNucleo(DataRow[] lineas, String idunidadOrigen, String idUnidadDestino)
        {
            foreach (DataRow linea in lineas)
            {
                if (linea["Idunidadtipoorigen"].ToString().Equals(idunidadOrigen, StringComparison.OrdinalIgnoreCase)
                    && linea["Idunidadtipodestino"].ToString().Equals(idUnidadDestino, StringComparison.OrdinalIgnoreCase))
                {
                    return Convert.ToDouble(linea["Conversion"]);
                }
            }
            return 1;
        }
        public static void RellenarUnidadesTipoPresentaciones(int idPresentacion, int tipoPresentacion, ref RadMultiColumnComboBox radComboBoxUnidadTipo, string valorPorDefecto)
        {
            try
            {
                if (tipoPresentacion > 0 &&  idPresentacion>0)
                {
                    DataTable tabla= DataAccess.GetIdDescripcionUnidadesTipoPresentacion(idPresentacion);
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, tabla, "IDUNIDADTIPO", "DESCRIPCION", valorPorDefecto, null,true);
                }
                else
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, DataAccess.GetIdDescripcionUnidadesTipo(), "IDUNIDADTIPO", "DESCRIPCION", valorPorDefecto, null,true);
                }
            
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public static object[] getTipoUnidadPresentacionVisualizacion(int idPresentacion)
        {
            //log.Debug("Inicio función getTipoUnidadPresentacionVisualizacion Presentacion" + idPresentacion);
            object[] presentacion = new object[2];
            try
            {
                DataRow pres = GetPresentacionVisualizacionArticulo(idPresentacion);
                if (pres != null)
                {
                    
                    presentacion[0] = pres["conversion"];
                    presentacion[1] = pres["idunidadtipodestino"];
                }
                return presentacion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
              //  log.Debug("Fin función getTipoUnidadPresentacionVisualizacion Presentacion" + idPresentacion);
            }
        }
        public static object[] getTipoUnidadPresentacionAlmacenamiento(int idPresentacion)
        {
            //log.Debug("Inicio función getTipoUnidadPresentacionAlmacenamiento Presentacion" + idPresentacion);
            object[] presentacion = new object[2];
            try
            {
                DataRow pres = GetPresentacionAlmacenamientoArticulo(idPresentacion);
                if (pres != null)
                {

                    presentacion[0] = pres["conversion"];
                    presentacion[1] = pres["idunidadtipodestino"];
                }
                return presentacion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               // log.Debug("Fin función getTipoUnidadPresentacionAlmacenamiento Presentacion" + idPresentacion);
            }
        }
        public static object[] getTipoUnidadPresentacion(int idPresentacion,String idUnidadTipo)
        {
            
            object[] presentacion = new object[2];
            try
            {
                DataRow pres = GetPresentacionArticulo(idPresentacion,idUnidadTipo);
                if (pres != null)
                {

                    presentacion[0] = pres["conversion"];
                    presentacion[1] = pres["idunidadtipodestino"];
                }
                return presentacion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //  log.Debug("Fin función getTipoUnidadPresentacionVisualizacion Presentacion" + idPresentacion);
            }
        }

        public static DataRow GetPresentacionArticulo(int idPresentacion,string tipoUnidad)
        {
            DataRow pres = null;
            try
            {
                //log.Debug("Iniciamos la carga de la presentacion " + idPresentacion + " al GetPresentacionesArticulo");
                if (dtPresentaciones == null)
                {
                    dtPresentaciones = ConexionSQL.getDataTable("SELECT * FROM dbo.TBLPRESENTACIONESLIN");
                }
                DataRow[] dtPres = dtPresentaciones.Select("IDPRESENTACION=" + idPresentacion + " AND IDUNIDADTIPODESTINO='" + tipoUnidad + "'");
                if (dtPres.Count() > 0)
                {
                    pres = dtPres[0];
                }
                return pres;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //log.Debug("Fin función GetPresentacionesArticulo Presentacion" + idPresentacion);
            }
        }

    }
}

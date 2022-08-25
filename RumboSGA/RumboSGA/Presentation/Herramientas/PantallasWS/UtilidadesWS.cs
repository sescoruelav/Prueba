using RumboSGA.VariablesMotor;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public static class UtilidadesWS
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string CARGA_INI_ATRIBUTOS_ENTRADAS = "ATRIBUTOS_ENTRADAS";
        /// Es el parámetro que carga los diferentes registros de la tabla
        /// TBLCONFATRIBPERSONALIZADOS si hubiera alguno creado para luego
        /// utilizarlos en los diferentes formularios
        /// </summary>
        public const string CARGA_INI_ATRIBUTOS_PERSONALIZADOS = "ATRIBUTOS_PERSONALIZADOS";
        public static List<OAtributo> getListaAtributos()
        {
            WSVariablesClient wsv = null;
            List<OAtributo> listaAtributos = null;
            List<OAtributoPersonalizado> listaAtributosPers = null;
            try
            {
                wsv = new WSVariablesClient();
                object[] resultadoCompuesto = wsv.cargarDatos(new String[] { CARGA_INI_ATRIBUTOS_ENTRADAS });
                object[] resultadoCompuestoPers = wsv.cargarDatos(new String[] { CARGA_INI_ATRIBUTOS_PERSONALIZADOS });
                object resultado = DescomponVariables(resultadoCompuesto, CARGA_INI_ATRIBUTOS_ENTRADAS);
                object resultadoPers = DescomponVariables(resultadoCompuestoPers, CARGA_INI_ATRIBUTOS_PERSONALIZADOS);
                listaAtributos = resultado as List<OAtributo>;
                listaAtributosPers = resultadoPers as List<OAtributoPersonalizado>;
                foreach (OAtributo atrib in listaAtributos)
                {
                    if (esAtributoClave(atrib,listaAtributosPers))
                    {
                        atrib.Clave = true;
                    }
                    else
                    {
                        atrib.Clave=false;
                    }
                    

                }
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, wsv.Endpoint);
            }
            return listaAtributos;
        }
        public static object DescomponVariables(object[] datosCompuestos, string datosASacar)
        {
            //Metodo para ayudar a sacar las variables
            //Introduces los datos compuestos que se reciben del WS y que quieres sacar y te devuleve un objecto con los resultados
            //En datosASacar hay que introducir una de las constantes de Variables,
            object resultadoDevolver = null;
            if (datosCompuestos == null || datosCompuestos.Length == 0)
            {
                log.Debug("No hay atributos a cargar");
                return null;
            }
            foreach (object[] datosCargados in datosCompuestos)
            {
                if (datosCargados[0].Equals(datosASacar))
                {
                    switch (datosCargados[0])
                    {
                        case CARGA_INI_ATRIBUTOS_ENTRADAS:
                            if (datosCargados[1] == null)
                            {
                                break;
                            }
                            object[][][] listaAtributos = (datosCargados[1] as object[][][]);
                            List<OAtributo> listaDevolver = new List<OAtributo>();
                            foreach (object[] atributoIndividual in listaAtributos)
                            {
                                OAtributo atr = new OAtributo(atributoIndividual);
                                listaDevolver.Add(atr);
                            }

                            resultadoDevolver = listaDevolver;
                            break;
                        case CARGA_INI_ATRIBUTOS_PERSONALIZADOS:
                            if (datosCargados[1] == null)
                            {
                                break;
                            }
                            object[][][] listaAtributosP = (datosCargados[1] as object[][][]);
                            List<OAtributoPersonalizado> listaDevolverP = new List<OAtributoPersonalizado>();
                            foreach (object[] atributoIndividual in listaAtributosP)
                            {
                                OAtributoPersonalizado atr = new OAtributoPersonalizado(atributoIndividual);

                                listaDevolverP.Add(atr);
                            }

                            resultadoDevolver = listaDevolverP;
                            break;
                    }
                }

            }
            return resultadoDevolver;
        }
        private static bool esAtributoClave(OAtributo atri, List<OAtributoPersonalizado> listaAtributosPers)
        {
            bool b = false;
            try
            {
                if (listaAtributosPers == null)
                {
                    return false;
                }
                foreach (OAtributoPersonalizado atribPer in listaAtributosPers)
                {

                    if (atribPer.ReferenciaTabla.Equals("TBLENTRADAS") && atribPer.ReferenciaCampo.Equals(atri.CampoOrigen) && atribPer.Clave)
                    {
                        b = true;
                        atri.AtribPers = atribPer;
                    }

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
            return b;

        }
        private static object getColumnaValorAtributoClave(OAtributo atri, List<OAtributoPersonalizado> listaAtributosPers)
        {
            bool b = false;
            try
            {
                if (listaAtributosPers == null)
                {
                    return null;
                }
                foreach (OAtributoPersonalizado atribPer in listaAtributosPers)
                {

                    if (atribPer.ReferenciaTabla.Equals("TBLENTRADAS") && atribPer.ReferenciaCampo.Equals(atri.CampoOrigen) && atribPer.Clave)
                    {
                        return atribPer.ReferenciaCampo;
                    }

                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
            return b;

        }
    }
}

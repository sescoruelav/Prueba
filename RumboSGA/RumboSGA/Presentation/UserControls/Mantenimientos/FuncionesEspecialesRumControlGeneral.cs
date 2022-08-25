using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using Rumbo.Core.Herramientas.Herramientas;
using RumboSGA.PedidoCliMotor;
using RumboSGAManager;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public class FuncionesEspecialesRumControlGeneral
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string funcionesEspecialesDespuesEditar(String nombreJson,dynamic selectedRow, AckResponse ack)
        {
            String respuesta = "";
            try
            {
                
                switch (nombreJson)
                {
                    case "RutasPreparacion":
                        respuesta = cerrarRutaPreparacion(ack, selectedRow);
                        break;


                }
            }catch(Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en funcionesEspecialesDespuesEditar con Json:"
                    +nombreJson+"");
                return "KO";
            }
            return respuesta;
        }


        private static string cerrarRutaPreparacion(AckResponse ack, dynamic selectedRow)
        {
            try
            {
                if (ack.Resultado.Equals("OK"))
                {
                    string json = JsonConvert.SerializeObject(selectedRow);
                    JArray valores = JArray.Parse("[" + json + "]");
                    Dictionary<string, object> valores1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(valores[0].ToString());

                    if (valores1["RP.IDRUTAESTADO"].ToString().Equals("'CE'"))
                    {
                        WSPedidoCliMotorClient ws = new WSPedidoCliMotorClient();
                        ws.cerrarRutaPreparacion(Convert.ToInt32(valores1["RP.IDRUTA"]), valores1["RP.DIA"].ToString(), DatosThread.getInstancia().getArrayDatos());
                        log.Debug("Ruta "+ valores1["RP.IDRUTA"] +" cerrada con día:"+ valores1["RP.DIA"].ToString());
                        return Lenguaje.traduce("Ruta " + valores1["RP.IDRUTA"] + " cerrada con día:" + valores1["RP.DIA"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "KO";
            
        }
    }
}

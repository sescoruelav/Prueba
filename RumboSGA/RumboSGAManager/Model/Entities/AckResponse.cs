using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class AckResponse
    {
        private string _resultado;
        private string _mensaje;
                
        public string Resultado { get { return _resultado; } set { _resultado = value; } }
        public string Mensaje { get { return Lenguaje.traduce(_mensaje); } set { _mensaje = value; } }
    }
}

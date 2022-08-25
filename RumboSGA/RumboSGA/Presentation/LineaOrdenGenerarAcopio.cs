using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGA.Presentation
{
    public class LineaOrdenGenerarAcopio
    {
        [JsonProperty]
        public int idpedidofab { get; set; }
        public int idpedidofablin { get; set; }
        public int cantidad { get; set; }
        public int usuario { get; set; }
        public bool paletcompleto { get; set; }
        public string Error { get; set; }
    }
    public class LineaOrden
    {
        List<LineaOrdenGenerarAcopio> acopio { get; set; }
    }
}


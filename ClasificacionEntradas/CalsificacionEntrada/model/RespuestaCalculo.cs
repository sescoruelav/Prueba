using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalsificacionEntrada.model
{
    internal class RespuestaCalculo
    {
        public string subsistema { get; set; }
        public string tiposoporte { get; set; }
        public int id { get; set; }
        public int cantidad { get; set; }
        public int idPedidoProLin { get; set; }

        public RespuestaCalculo()
        {
        }

        public RespuestaCalculo(int idPedidoProLin, string subsistema, string tiposoporte, int id, int cantidad)
        {
            this.idPedidoProLin = idPedidoProLin;
            this.subsistema = subsistema;
            this.tiposoporte = tiposoporte;
            this.id = id;
            this.cantidad = cantidad;
        }
    }
}
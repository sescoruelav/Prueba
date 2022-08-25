using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalsificacionEntrada.model
{
    internal class DatosLineas
    {
        public string idrecepcion { get; set; }
        public string idpedidopro { get; set; }
        public int idpedidoprolin { get; set; }
        public int cantidad { get; set; }

        public DatosLineas()
        {
        }

        public DatosLineas(string idrecepcion, string idpedidopro, int idpedidoprolin, int cantidad)
        {
            this.idrecepcion = idrecepcion;
            this.idpedidopro = idpedidopro;
            this.idpedidoprolin = idpedidoprolin;
            this.cantidad = cantidad;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalsificacionEntrada.model
{
    internal class DatosTodasLineas : DatosLineas
    {
        public List<Lineas> lineas { get; set; }

        public DatosTodasLineas()
        {
        }

        public DatosTodasLineas(string idrecepcion, string idpedidopro, int idpedidoprolin, int cantidad, List<Lineas> lineas)
        {
            this.idrecepcion = idrecepcion;
            this.idpedidopro = idpedidopro;
            this.idpedidoprolin = idpedidoprolin;
            this.cantidad = cantidad;
            this.lineas = lineas;
        }
    }
}
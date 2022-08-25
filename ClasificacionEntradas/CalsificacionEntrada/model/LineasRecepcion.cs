using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalsificacionEntrada.model
{
    public class LineasRecepcion
    {
        public string id { get; set; }
        public int idPedido { get; set; }
        public int idpedidoprolin { get; set; }
        public string codigoProveedor { get; set; }
        public string atrib1 { get; set; }
        public string pedido { get; set; }
        public string serie { get; set; }
        public int idArticulo { get; set; }
        public string codArticulo { get; set; }
        public string articulo { get; set; }
        public int cantPedido { get; set; }
        public int cantRecepcion { get; set; }
        public int entRecepcio { get; set; }
        public string recepcion { get; set; }

        public LineasRecepcion()
        {
        }

        public LineasRecepcion(string id, int idPedido, int idPedidoProLin, string codigoProveedor, string atrib1, string pedido, string serie, int idArticulo, string codArticulo, string articulo, int cantPedido, int cantRecepcion, int entRecepcio, string recepcion)
        {
            this.id = id;
            this.idPedido = idPedido;
            this.idpedidoprolin = idpedidoprolin;
            this.codigoProveedor = codigoProveedor;
            this.atrib1 = atrib1;
            this.pedido = pedido;
            this.serie = serie;
            this.idArticulo = idArticulo;
            this.codArticulo = codArticulo;
            this.articulo = articulo;
            this.cantPedido = cantPedido;
            this.cantRecepcion = cantRecepcion;
            this.entRecepcio = entRecepcio;
            this.recepcion = recepcion;
        }
    }
}
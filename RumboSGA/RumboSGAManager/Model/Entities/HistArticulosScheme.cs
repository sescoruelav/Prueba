using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class HistArticulosScheme
    {
        private LineaScheme _primeraLinea;
        private LineaScheme[] _lineasMedias;
        private LineaScheme _lineaSaldo;
        private LineaScheme _lineaExistencia;
        private string _alertaStockStatus;
        private string _faltasStockStatus;


        public LineaScheme PrimeraLinea { get => _primeraLinea; set => _primeraLinea = value; }
        public LineaScheme[] LineasMedias { get => _lineasMedias; set => _lineasMedias = value; }
        public LineaScheme LineaSaldo { get => _lineaSaldo; set => _lineaSaldo = value; }
        public LineaScheme LineaExistencia { get => _lineaExistencia; set => _lineaExistencia = value; }
        public string AlertaStockStatus { get => _alertaStockStatus; set => _alertaStockStatus = value; }
        public string FaltasStockStatus { get => _faltasStockStatus; set => _faltasStockStatus = value; }
    }
    public class LineaScheme
    {
        private int id;
        private int cantidad;
        private int idRecurso;
        private string fecha;
        private string hora;
        private int tipo;
        private string movimiento;
        private int saldo;
        private string concepto;
        private int stockStatus;
        private int idEntrada;
        private string huecoOrigen;
        private string huecoDestino;
        private string motivo;
        private int idoperario;
        private string nombreOperario;
        private string lote;
        private string informaerp;
        private string comentario;
        private string pedidoVenta;
        private string pedidoCompra;
        private string fechaCaducidad;

        public int Id { get => id; set => id = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public int IdRecurso { get => idRecurso; set => idRecurso = value; }
        public string Fecha { get => fecha; set => fecha = value; }
        public string Hora { get => hora; set => hora = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public string Movimiento { get => movimiento; set => movimiento = value; }
        public int Saldo { get => saldo; set => saldo = value; }
        public string Concepto { get => concepto; set => concepto = value; }
        public int StockStatus { get => stockStatus; set => stockStatus = value; }
        public int IdEntrada { get => idEntrada; set => idEntrada = value; }
        public string HuecoOrigen { get => huecoOrigen; set => huecoOrigen = value; }
        public string HuecoDestino { get => huecoDestino; set => huecoDestino = value; }
        public string Motivo { get => motivo; set => motivo = value; }
        public int Idoperario { get => idoperario; set => idoperario = value; }
        public string NombreOperario { get => nombreOperario; set => nombreOperario = value; }
        public string Lote { get => lote; set => lote = value; }
        public string Informaerp { get => informaerp; set => informaerp = value; }
        public string Comentario { get => comentario; set => comentario = value; }
        public string PedidoVenta { get => pedidoVenta; set => pedidoVenta = value; }
        public string PedidoCompra { get => pedidoCompra; set => pedidoCompra = value; }
        public string FechaCaducidad { get => fechaCaducidad; set => fechaCaducidad = value; }
    }

}

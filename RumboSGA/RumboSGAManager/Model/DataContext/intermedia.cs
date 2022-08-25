using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager
{
    public static class intermedia
    {
        private static string filtros;
        private static string nombreTabla;
        private static string select;
        private static string name;
      //  private static BaseGridControl virtualGridActual;
        private static string query;
        private static string template;
        private static int numRegistrosFiltrados;
        public static string Name { get => name; set => name = value; }
        public static string NombreTabla { get => nombreTabla; set => nombreTabla = value; }
        public static int NumRegistrosFiltrados { get => numRegistrosFiltrados; set => numRegistrosFiltrados = value; }
        public static string Select { get => select; set => select = value; }
      //  public static BaseGridControl VirtualGridActual { get => virtualGridActual; set => virtualGridActual = value; }
        public static string Query { get => query; set => query = value; }
        public static string Template { get => template; set => template = value; }
        public static string Filtros { get => filtros; set => filtros = value; }
    }
}

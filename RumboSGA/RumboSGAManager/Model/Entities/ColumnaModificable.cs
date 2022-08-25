using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class ColumnaModificable
    {
        string nombreColumna;
        string tabla;
        string nombreColumnaEtiqueta;
        public ColumnaModificable()
        {
        
        }
        public ColumnaModificable(Object[] columnas)
        {
            nombreColumna = (columnas[0] as object[])[1].ToString();
            tabla = (columnas[1] as object[])[1].ToString();
            nombreColumnaEtiqueta = (columnas[2] as object[])[1].ToString();
        }

        public ColumnaModificable(string nombreColumna_, string tabla_,string nombreColumnaEtiqueta)
        {
            this.nombreColumna = nombreColumna_;
            this.tabla = tabla_;            
        }

        public string NombreColumna { get => nombreColumna; set => nombreColumna = value; }
        public string Tabla { get => tabla; set => tabla = value; }
        public string NombreColumnaEtiqueta { get => nombreColumnaEtiqueta; set => nombreColumnaEtiqueta = value; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Constantes
{
    
        /// <summary>
        /// Constantes con los posibles valores del campo CONFLOTE de 
        /// la tabla articulos
        /// </summary>
        public class ConstArticulosConfLote
        {
            /// <summary>
            /// No se confirma ni introducir el lote ya que lo calcula una regla 
            /// </summary>
            public const string AUTOMATICO = "A";

            /// <summary>
            /// No se confirma ni introducir el lote
            /// </summary>
            public const string NO = "N";
            /// <summary>
            /// en este caso, al usuario se le muestra en pantalla 
            /// el lote del que debe recoger, pudiendo borrar el que 
            /// se le muestra e introducir el que se lleva realmente
            /// </summary>
            public const string SI = "S";
            /// <summary>
            /// No se introduce la fecha de caducidad
            /// </summary>
            public const string NO_CONFIRMAR_FECHA = "L";
            /// <summary>
            /// en este caso le pide confirmar el lote únicamente 
            /// cuando en la ubicación conviven más de un lote,
            /// pudiendo en el caso de existir varios, borrarlo
            /// en introducir el que realmente recoge. 
            /// </summary>
            public const string CONFIRMAR = "C";
            /// <summary>
            /// en este caso el operario debe realizar la lectura 
            /// del lote ó introducirla manualmente desde el teclado
            /// </summary>
            public const string INTRODUCIR = "I";

            public const string NO_CONFIRMAR_LOTE = "L";

        }
 
}

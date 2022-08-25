using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGA
{
        public static class ErroresHueco
        {
            public enum Error
            {
                OK = 0,
                CABE_ERR = -1,
                NOCABE_LLENO = 1,
                NOCABE_MOV_PTE = 2,
                NOCABE_ART_DIFERENTE = 3,
                NOCABE_NO_COMPATIBLE = 4,
                NOCABE_UDS = 5,
                NO_CONSOLIDABLE = 6,
                HUECO_NO_ACTIVO = 7,
                EX_NO_ENCONTRADA = 8,
                EX_NO_OK = 9,
                EX_BQ = 10,
                HP_OTRO_ART = 11,
                NO_UBICAR_EN_RECURSO = 12
            }
            public const string MSG_CABE_ERR = "Error comprobando capacidad";
            public const string MSG_NOCABE_LLENO = "No cabe, hueco lleno";
            public const string MSG_NOCABE_MOV_PTE = "No cabe, movimiento pendiente";
            public const string MSG_NOCABE_ART_DIFERENTE = "No cabe, distinto articulo";
            public const string MSG_NOCABE_NO_COMPATIBLE = "No cabe, hueco no compatible";
            public const string MSG_NOCABE_UDS = "No cabe, demasiadas unidades";
            public const string MSG_HUECO_NO_ACTIVO = "El hueco no esta activo";
            public const string MSG_EX_NO_ENCONTRADA = "No se ha encontrado la existencia";
            public const string MSG_NO_OK = "Existencia no OK";
            public const string MSG_EX_BQ = "Existencia bloqueada";
            public const string MSG_NO_CONSOLIDABLE = "Error existencia no consolidable";
            public const string MSG_HP_OTRO_ART = "Hueco de picking de otro artículo";
            public const string MSG_NO_UBICAR_EN_RECURSO = "No se puede ubicar en el recurso";



            private static Dictionary<int, string> _errores = new Dictionary<int, string>()
        {
                {(int)Error.CABE_ERR, MSG_CABE_ERR},
                {(int)Error.NOCABE_LLENO, MSG_NOCABE_LLENO},
                {(int)Error.NOCABE_MOV_PTE, MSG_NOCABE_MOV_PTE},
                {(int)Error.NOCABE_ART_DIFERENTE, MSG_NOCABE_ART_DIFERENTE},
                {(int)Error.NOCABE_NO_COMPATIBLE, MSG_NOCABE_NO_COMPATIBLE},
                {(int)Error.NOCABE_UDS, MSG_NOCABE_UDS},
                {(int)Error.EX_NO_ENCONTRADA, MSG_EX_NO_ENCONTRADA},
                {(int)Error.EX_NO_OK, MSG_NO_OK},
                {(int)Error.EX_BQ, MSG_EX_BQ},
                {(int)Error.HUECO_NO_ACTIVO, MSG_HUECO_NO_ACTIVO},
                {(int)Error.NO_CONSOLIDABLE, MSG_NO_CONSOLIDABLE},
                {(int)Error.HP_OTRO_ART, MSG_HP_OTRO_ART},
                {(int)Error.NO_UBICAR_EN_RECURSO, MSG_NO_UBICAR_EN_RECURSO},
                {(int)Error.OK, string.Empty},
        };



            /// <summary>
            /// Obtiene el error producido
            /// </summary>
            /// <param name="numError">Número de error</param>
            /// <returns>string con el mensaje de error</returns>
            public static String GetError(int numError)
            {
                try
                {
                    return _errores[numError];
                }
                catch
                {
                    return "Num Error=" + numError + " Error no definido";
                }
            }



            /// <summary>
            /// Obtiene el error producido
            /// </summary>
            /// <param name="error">Enumeracion</param>
            /// <returns>string con el mensaje error</returns>
            public static String GetError(Error error)
            {
                try
                {
                    return GetError((int)error);
                }
                catch
                {
                    
                    return "Num Error=" + (int)error + " Error no definido";
                }
            }

        }
    }

using EstadoRecepciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EjecutablePruebas
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string ruta = @"C:\Users\sescoruela\Desktop\clasificacionRecepciones\PruebaClasificacion.json";

            string json = Utilidades.LoadJson(ruta);
            Console.WriteLine(json);

            string path = "C:\\Users\\sescoruela\\Documents\\ArticulosStript.sql";

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("GO");
                writer.WriteLine("SET IDENTITY_INSERT[dbo].[TBLARTICULOS] ON");

                for (int i = 0; i < 1000; i++)
                {
                    writer.WriteLine("INSERT[dbo].[TBLARTICULOS]([IDARTICULO], [CANTIDAD], [IDPALETTIPO], [IDUNIDADTIPO], [CANTIDADUNIDAD], [DIASCADUCIDAD], [MINPICKING], [IDHUECOPICKING], [IDZONAPICKING], [IDZONAUBICACION], [IDZONAUBICACION2], [IDARTICULOPOSE], [IDARTICULOESTADO], [TIPOARTICULO], [PESO], [IDPROVEEDOR], [FORMAUBICACION], [ESTADOENTRADA], [REFERENCIA], [DESCRIPCION], [FAMILIA], [STOCKMINREAB], [STKMINALMCEN], [STKMAXALMCEN], [CANTALMAUX], [ZONALOGALMAUX], [VOLUMEN], [CANTAALMAUX], [ATRIBUTO], [CAPACIDADPICKING], [TOPEPICKABLE], [CAMPOSCOMPTRAZA], [IDZONAGV], [CANTALMGV], [FACTORCONVERSION], [LOTEESTRICTO], [EQUIVALENCIA], [IDCOMENTARIO], [CMD], [ROTACION], [COSTEARTICULO], [MARGENMEDIO], [COBERTURADESEADA], [LEADTIME], [DIRECTORIOIMAGEN], [IMAGEN], [CONSOLIDABLE], [CONSOLIDAR], [CONFLOTE], [SINPEDIDO], [CLASIFICACION1], [CLASIFICACION2], [TIPODESBORDAMIENTO], [IDHUECODESB], [PICKINGUDS], [IDHUECOPICKINGUDS], [STOCKMINREABUDS], [IDZONAPICKINGUDS], [MINPICKINGUDS], [CAPACIDADPICKINGUDS], [CONFIGURADO], [PORCENTAJEEXCESO], [DESCRIPCIONVOZ], [PEDIRCANTCJ], [SUPCANTPICKING], [IDEMBALAJE], [REGISTROTIPOPALET], [IDPROPIETARIO], [IDALGORITMOUBICACION], [IDALGORITMOEXTRACCION], [MARCACLIENTE], [ATRIBUTO2], [IDARTICULOANTERIOR], [ARTTIPO], [STOCKMAX], [CANTIDADRETRACTIL], [SUBSISTEMALOGISTICO], [STOCKMAXPICKING], [STOCKMINPICKING], [ACTUALIZADO], [SINCONTROLSTOCK], [CAPTURANSERIE], [CLASIFICACIONPLAN], [TIEMPOPRODUCCIONPIEZA], [TIEMPOPREPARACIONORDEN], [REFPROVEEDOR], [TiempoMedPI], [TiempoMedSC], [TiempoMedCo], [idzonatrabajopicker], [CAJASCAPA], [CAPASPALET], [PREPARACIONZONACALIENTE], [TIPOIMPUTACIONUNIDADES], [IDPRESENTACION], [TIPOPRESENTACION], [IDFORMATO], [REPPALETCOMPLETO], [TIPOETIQ], [PREFIJOEAN], [CONSOLIDARENCARRO], [APILABILIDAD], [ABCMovimientos], [COMPLETOCAMBIANTE], [TantoXCientoCompleto], [REGISTROESTADOEXISTENCIA], [UNIDADCONSUMO], [UDSPICKER], [NOPERMITIRLECTURAUBICACIONPICKING], [IDZONAGVSALIDAS], [CANTALMGVSALIDAS], [IDALGORITMOPRECLASIFICACION], [REGISTRARPESO])" +
                        " VALUES(" + 987678 + i + ", 1, N'E1', N'CJ', 1, 0, 0, 0, 15, 15, 0, N'TE', N'AC', NULL, 0, 0, N'NP', N'PU', N'5031504" + i + 9937 + "', N'LOTE BANDEJA 1 FS ORGANIC SHOP 2020', N'GRA', 4, 0, 0, 0, 0, 0, 0, N'0001" + i + 37 + "', 0, 0, N'', 0, 0, 1, N'N', N'EX.IDARTICULO, EX.IDHUECO', NULL, 0, 0, 0, 0, 0, 0, NULL, NULL, N'IDARTICULO', N'IDARTICULO', N'N', N'N', N'AN', NULL, N'TO', 0, 0, 0, 0, 0, 0, 0, 0, 500, N'LOT BANDEJA 1 FS OSH', N'N', N'S', N'NO', N'A', 0, NULL, NULL, 0, NULL, 0, N'L', 0, 1, N'OVERFLOW', 0, 0, 1, 0, N'000', N'GE', 0, 0, NULL, 1, 1, 1, 12, 0, 0, N'N', N'0', 0, -1, NULL, 0, NULL, NULL, 0, 0, N'C', 0, 0, N'A', 1, 0, 0, 0, 0, 0, N'SS')");
                }
                writer.WriteLine("GO");
                writer.WriteLine("SET IDENTITY_INSERT[dbo].[TBLARTICULOS] OFF");
            }

            clasificacionEntradas inicio = new clasificacionEntradas(json);
            Application.Run();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EstadoRecepciones
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string json = Utilidades.LoadJson();
            Console.WriteLine(json);
            clasificacionEntradas inicio = new clasificacionEntradas(json);
            //  clasificacionEntradas clasificacion = new clasificacionEntradas(json);

            Application.Run();
        }
    }
}
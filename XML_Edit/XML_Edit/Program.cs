using System;
using System.Linq;
using System.Windows.Forms;

namespace XML_Edit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string ficheroaEditar;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*ficheroaEditar = @"C:\Rumbo\Configs\Cronus2020_91\Trazabilidad.xml";
            AuxEditor editor = new AuxEditor(ficheroaEditar);*/
            
            ficheroaEditar = @"C:\Rumbo\Configs\rumbokanban.json";
            AuxEditor editor2 = new AuxEditor(ficheroaEditar, AuxType.DataTable);
            editor2.Mostrar();

            /*ficheroaEditar = @"C:\Rumbo\Configs\rumbokanban.json";
            AuxEditor editor3 = new AuxEditor(ficheroaEditar);

            ficheroaEditar = @"C:\Rumbo\Configs\Cronus2020_91\menu.xml";
            AuxEditor editor4 = new AuxEditor(ficheroaEditar);*/

            ficheroaEditar = @"C:\Rumbo\Configs\organizadorAlgs.json";
            AuxEditor editor5 = new AuxEditor(ficheroaEditar, AuxType.DataTable);
            System.Data.DataTable dt = editor5.getDatatable();

            ficheroaEditar = @"C:\Rumbo\Configs\organizadorLims.json";
            AuxEditor editor6 = new AuxEditor(ficheroaEditar, AuxType.DataTable);



            Application.Run(new RadForm1());
        }
    }
}
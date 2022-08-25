using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace EstadoRecepciones
{
    public class Utilidades
    {
        private static string nombreEstiloGridView = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Carga un JSON desde Archivo de recursos
        public static string LoadJson(string ruta)
        {
            string json = string.Empty;
            try
            {
                using (StreamReader r = new StreamReader(ruta))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show("No se ha podido cargar el Json para la prueba");
            }
            return json;
        }

        public static RadGridView SaveLayout(string path, RadGridView gridView, string nombreEstilos)
        {
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                Directory.CreateDirectory(path);
                path += "\\";
                path.Replace(" ", "_");

                gridView.SaveLayout(path + nombreEstilos + "GridView");
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show("No se ha podido encontrar el archivo en la dirección" + ":" + path + "\n" + "Puede cambiar esta dirección en el archivo PathLayouts.xml");
            }

            return gridView;
        }

        public static RadGridView LoadLayout(string path, RadGridView gridView, string nombreEstilos)
        {
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                string ruta = path + "\\";
                ruta = ruta + nombreEstilos + "GridView";
                ruta.Replace(" ", "_");
                if (File.Exists(ruta))
                {
                    gridView.LoadLayout(ruta);
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                MessageBox.Show("No se ha podido encontrar el archivo en la dirección" + ":" + path + "\n" + "Puede cambiar esta dirección en el archivo PathLayouts.xml");
            }
            return gridView;
        }
    }
}
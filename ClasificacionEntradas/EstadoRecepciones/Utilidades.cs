using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace EstadoRecepciones
{
    internal class Utilidades
    {
        private static string nombreEstiloGridView = string.Empty;

        //Carga un JSON desde Archivo de recursos
        public static string LoadJson()
        {
            string json = string.Empty;
            try
            {
                string ruta = @"C:\Users\sescoruela\Desktop\clasificacionRecepciones\PruebaClasificacion.json";
                using (StreamReader r = new StreamReader(ruta))
                {
                    json = r.ReadToEnd();
                }
                if (json != null && !String.IsNullOrEmpty(json))
                {
                    String jsonLower = json.ToLower();
                    if (jsonLower.Contains("\"from\""))
                    {
                        int index = jsonLower.IndexOf("\"from\"");
                        json = json.Remove(index, 6);
                        json = json.Insert(index, "\"FROM\"");
                    }

                    if (jsonLower.Contains("\"where\""))
                    {
                        int index = jsonLower.IndexOf("\"where\"");
                        json = json.Remove(index, 7);
                        json = json.Insert(index, "\"WHERE\"");
                    }
                }
            }
            catch (Exception e)
            {
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

                gridView.SaveLayout(path + nombreEstilos);
            }
            catch (DirectoryNotFoundException e)
            {
                //  log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
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
                string s = path + "\\";
                s.Replace(" ", "_");

                gridView.LoadLayout(s + nombreEstilos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se ha podido encontrar el archivo en la dirección" + ":" + path + "\n" + "Puede cambiar esta dirección en el archivo PathLayouts.xml");
            }
            return gridView;
        }
    }
}
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public class ConfigurarSQL
    {
		public static string CargarConsulta(string path, string node)
		{
			try
			{
				XDocument X = XDocument.Load(path);
				return X.Descendants(node).ToArray()[0].Value.Replace("\n", String.Empty).Trim();
				//Replace -> Limpia la consulta de saltos de linea
				//Trim -> Limpia el principio y el final de la consulta de espacios en blanco
			}
			catch (Exception ex)
			{
				ExceptionManager.GestionarError(ex);
				return null;
			}            
        }
    }
}

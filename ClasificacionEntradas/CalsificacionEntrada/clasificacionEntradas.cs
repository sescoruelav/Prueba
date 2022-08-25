using System;
using System.Collections.Generic;
using System.Text;

namespace EstadoRecepciones
{
    public class clasificacionEntradas
    {
        public clasificacionEntradas(string json)
        {
            //string json = Utilidades.LoadJson();
            //Console.WriteLine(json);
            clasificacionEntradasForm clasificacion = new clasificacionEntradasForm(json);
            clasificacion.ShowDialog();
        }
    }
}
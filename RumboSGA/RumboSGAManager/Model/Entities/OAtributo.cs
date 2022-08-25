using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class OAtributo
    {
        String atributo, descripcion, tablaOrigen, campoOrigen, nombre, tipoDato, defecto;
        int obligatorio;
        bool clave;
        OAtributoPersonalizado atribPers;
        

        public OAtributo(Object[] atributosObject)
        {
            atributo = (atributosObject[0] as object[])[1].ToString();
            descripcion = (atributosObject[1] as object[])[1].ToString();
            if ((atributosObject[2] as object[])[1] is null) tablaOrigen = null;
            else tablaOrigen = (atributosObject[2] as object[])[1].ToString();
            //tablaOrigen = (atributosObject[2] as object[])[1].ToString();
            if ((atributosObject[3] as object[])[1] is null) campoOrigen = null;
            else campoOrigen = (atributosObject[3] as object[])[1].ToString();
            //campoOrigen = (atributosObject[3] as object[])[1].ToString();
            nombre = (atributosObject[4] as object[])[1].ToString();
            tipoDato = (atributosObject[5] as object[])[1].ToString();
            obligatorio = int.Parse((atributosObject[6] as object[])[1].ToString());
            if((atributosObject[7] as object[])[1] is null) defecto = null;
            else defecto = (atributosObject[7] as object[])[1].ToString();
        }

        public OAtributo(string atributo, string descripcion, string tablaOrigen, string campoOrigen, string nombre, string tipoDato,  int obligatorio, string defecto)
        {
            this.atributo = atributo;
            this.descripcion = descripcion;
            this.tablaOrigen = tablaOrigen;
            this.campoOrigen = campoOrigen;
            this.nombre = nombre;
            this.tipoDato = tipoDato;
            this.defecto = defecto;
            this.obligatorio = obligatorio;
        }

        public string Atributo { get => atributo; set => atributo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string TablaOrigen { get => tablaOrigen; set => tablaOrigen = value; }
        public string CampoOrigen { get => campoOrigen; set => campoOrigen = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string TipoDato { get => tipoDato; set => tipoDato = value; }
        public string Defecto { get => defecto; set => defecto = value; }
        public int Obligatorio { get => obligatorio; set => obligatorio = value; }
        public bool Clave { get => clave; set => clave = value; }
        public OAtributoPersonalizado AtribPers { get => atribPers; set => atribPers = value; }




    }
}

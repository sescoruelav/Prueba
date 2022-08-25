using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class OAtributoPersonalizado
    {
       
        private int idatributo;
        private string nombretabla;
        private string campotabla;
        private string tipodato;
        private string nombre;
        private string descripcion;
        private string referenciatabla;
        private string referenciacampo;
       
        
        
        
        private bool clave;


        public OAtributoPersonalizado(Object[] atributosObject)
        {
            idatributo = Convert.ToInt32((atributosObject[0] as object[])[1]);
            nombretabla = (atributosObject[1] as object[])[1].ToString();
            if ((atributosObject[2] as object[])[1] is null) campotabla = null;
            else campotabla = (atributosObject[2] as object[])[1].ToString();
            tipodato = (atributosObject[3] as object[])[1].ToString();
            nombre = (atributosObject[4] as object[])[1].ToString();
            if ((atributosObject[5] as object[])[1] is null) descripcion = null;
            else descripcion = (atributosObject[5] as object[])[1].ToString();

            if ((atributosObject[6] as object[])[1] is null) referenciatabla = null;
            else referenciatabla = (atributosObject[6] as object[])[1].ToString();
            if ((atributosObject[7] as object[])[1] is null) referenciacampo = null;
            else referenciacampo = (atributosObject[7] as object[])[1].ToString();          
           
           
           
            
            if ((atributosObject[8] as object[])[1] is null) clave = false;
            else clave = Convert.ToBoolean((atributosObject[8] as object[])[1]);
        }

        public OAtributoPersonalizado(int idatributo, string nombretabla, string referenciatabla, string referenciacampo, string tipodato, string campotabla, string nombre , 
            string descripcion, bool clave)
        {
            this.idatributo = idatributo;
            this.nombretabla = nombretabla;
            this.referenciatabla = referenciatabla;
            this.referenciacampo = referenciacampo;
            this.tipodato = tipodato;
            this.campotabla = campotabla;
            this.descripcion = descripcion;
            this.clave = clave;
        }

        public int IdAtributo { get => idatributo; set => idatributo = value; }       
        public string NombreTabla { get => nombretabla; set => nombretabla = value; }
        public string ReferenciaTabla { get => referenciatabla; set => referenciatabla = value; }
        public string ReferenciaCampo { get => referenciacampo; set => referenciacampo = value; }
        public string TipoDato { get => tipodato; set => tipodato = value; }
        public string CampoTabla { get => campotabla; set => campotabla = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public bool Clave { get => clave; set => clave = value; }




    }
}

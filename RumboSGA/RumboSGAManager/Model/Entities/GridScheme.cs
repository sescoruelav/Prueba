using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class GridScheme
    {
        private string _archivoJSON;
        private string _filtro;
        private string _filtroEnJsonLinea;
        private string _valorFiltro;
        private string _tablaOrigen;

        public string ArchivoJSON { get => _archivoJSON; set => _archivoJSON = value; }
        public string Filtro { get => _filtro; set => _filtro = value; }
        public string FiltroEnJsonLinea { get => _filtroEnJsonLinea; set => _filtroEnJsonLinea = value; }
        public string ValorFiltro { get => _valorFiltro; set => _valorFiltro = value; }
        public string TablaOrigen { get => _tablaOrigen; set => _tablaOrigen = value; }

        public List<Claves> Claves = new List<Claves>();

        public void SetValorClave(string nombreBuscado, string valor)
        {
            //string _valor = string.Empty;

            for (int i=0; i < Claves.Count; i++)
            {
                if (nombreBuscado == Claves[i].CampoRelacionadoPadre)
                {
                    //_valor = Claves[i].Valor;
                    Claves[i].Valor = valor;
                    break;
                }
            }

            //return _valor;
        }
    }


    public class Claves
    {
        private string _nombre;
        private string _valor;
        private string _campoRelacionadoPadre;
        private ValorTabla _valorTabla;

        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Valor { get => _valor; set => _valor = value; }

        public ValorTabla ValorTabla { get => _valorTabla; set => _valorTabla = value; }
        public string CampoRelacionadoPadre { get => _campoRelacionadoPadre; set => _campoRelacionadoPadre = value; }

        public Claves()
        {
            _valorTabla = new ValorTabla();
        }
                
    }

    public class ValorTabla
    {
        private string _tablaRelacionada;
        private string _campoRelacionado;
        private string _campoMostrado;
        private string _valorDefecto;

        public string TablaRelacionada { get => _tablaRelacionada; set => _tablaRelacionada = value; }
        public string CampoRelacionado { get => _campoRelacionado; set => _campoRelacionado = value; }
        public string CampoMostrado { get => _campoMostrado; set => _campoMostrado = value; }
        public string ValorDefecto { get => _valorDefecto; set => _valorDefecto = value; }

        public ValorTabla()
        {
            /*_tablaRelacionada = string.Empty;
            _campoRelacionado = string.Empty;
            _campoMostrado = string.Empty;
            _valorDefecto = string.Empty;*/
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rumbo.Core.Herramientas;

namespace RumboSGAManager.Model.Entities
{
    public class TableScheme
    {
        private string _nombre;
        private string _etiqueta;
        private string _tipo;
        private string _tipoColumna;
        private string _control;
        private string _tab;
        private bool _esPK = false;
        private bool _esModificable = true;
        private int _columna;
        private int _fila;
        private int _alto;
        private int _ancho;
        private bool _esFK = false;
        private bool _esVisible = false;
        private ComboScheme _cmbObject;
        private string _valorDefecto;
        private bool _autoIncrementado = false;
        private int _numeroColumnas;
        private bool _puedeNull = true;
        private bool _puedeVacio = true;
        private bool _puedeMasivo = false;
        private bool _existeTabla = false;
        private bool _indexCmb = false;

        public string Nombre
        { get { return _nombre; } set { _nombre = value; } }

        public string Tipo
        { get { return _tipo; } set { _tipo = value; } }

        public string Control
        { get { return _control; } set { _control = value; } }

        public string Tab
        { get { return _tab; } set { _tab = value; } }

        public bool EsPK
        { get { return _esPK; } set { _esPK = value; } }

        public bool EsModificable
        { get { return _esModificable; } set { _esModificable = value; } }

        public bool AutoIncrementado
        { get { return _autoIncrementado; } set { _autoIncrementado = value; } }

        public int Columna
        { get { return _columna; } set { _columna = value; } }

        public int Fila
        { get { return _fila; } set { _fila = value; } }

        public int Alto
        { get { return _alto; } set { _alto = value; } }

        public int Ancho
        { get { return _ancho; } set { _ancho = value; } }

        public int NumeroColumnas
        { get { return _numeroColumnas; } set { _numeroColumnas = value; } }

        public ComboScheme CmbObject { get => _cmbObject; set => _cmbObject = value; }
        public bool EsFK { get => _esFK; set => _esFK = value; }
        public string ValorDefecto { get => _valorDefecto; set => _valorDefecto = value; }

        public string Etiqueta
        {
            get
            {
                String s = Lenguaje.traduce(_etiqueta);
                return s;
            }
            set => _etiqueta = value;
        }

        public bool EsVisible { get => _esVisible; set => _esVisible = value; }
        public string TipoColumna { get => _tipoColumna; set => _tipoColumna = value; }
        public bool PuedeNull { get => _puedeNull; set => _puedeNull = value; }
        public bool PuedeVacio { get => _puedeVacio; set => _puedeVacio = value; }
        public bool PuedeMasivo { get => _puedeMasivo; set => _puedeMasivo = value; }
        public bool ExisteTabla { get => _existeTabla; set => _existeTabla = value; }
        public bool IndexCmb { get => _indexCmb; set => _indexCmb = value; }

        public TableScheme()
        {
            _cmbObject = new ComboScheme();
        }
    }

    public class ComboScheme
    {
        private string _tablaRelacionada;
        private string _campoRelacionado;
        private string _campoMostrado;
        private string _campoMostradoAux;
        private string _valorDefecto;
        private string _from;
        private string _filtro;
        private string _aliasPadre;
        private bool _esVisible; // John Campo creado para saber si es visible el campo relacionado

        public bool EsVisible { get => _esVisible; set => _esVisible = value; }
        public string TablaRelacionada { get => _tablaRelacionada; set => _tablaRelacionada = value; }
        public string CampoRelacionado { get => _campoRelacionado; set => _campoRelacionado = value; }
        public string CampoMostrado { get => _campoMostrado; set => _campoMostrado = value; }
        public string CampoMostradoAux { get => _campoMostradoAux; set => _campoMostradoAux = value; }
        public string ValorDefecto { get => _valorDefecto; set => _valorDefecto = value; }

        public string From
        { get { return _from; } set { _from = value; } }

        public string Filtro
        { get { return _filtro; } set { _filtro = value; } }

        public string AliasPadre
        { get { return _aliasPadre; } set { _aliasPadre = value; } }

        public ComboScheme()
        {
            /*_tablaRelacionada = string.Empty;
            _campoRelacionado = string.Empty;
            _campoMostrado = string.Empty;
            _valorDefecto = string.Empty;*/
        }
    }
}
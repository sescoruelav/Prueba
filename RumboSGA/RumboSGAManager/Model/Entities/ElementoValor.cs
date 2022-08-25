using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class ElementoValor
    {
        private string _elemento;
        private string _valor;

        public string ELEMENTO { get => _elemento; set => _elemento = value; }
        public string VALOR { get => _valor; set => _valor = value; }
    }
}

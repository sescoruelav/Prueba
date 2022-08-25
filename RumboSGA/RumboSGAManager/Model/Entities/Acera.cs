using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class TblAcera
    {
        public int IdAcera { get; set; }
        public int IHuecoAlmacen { get; set; }
        public int Acera { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public int Orientacion { get; set; }
        public string Numeracion { get; set; }
        public int Ancho { get; set; }
        public int Profundo { get; set; }
        public int OrdenPicking { get; set; }
        public TblAcera()
        {
 
        }
        public TblAcera(int IdAcera_, int IHuecoAlmacen_, int Acera_, double PosX_, double PosY_, int Orientacion_, string Numeracion_, int Ancho_, int Profundo_, int OrdenPicking_)
        {
            this.IdAcera = IdAcera_;
            this.IHuecoAlmacen = IHuecoAlmacen_;
            this.Acera = Acera_;
            this.PosX = PosX_;
            this.PosY = PosY_;
            this.Orientacion = Orientacion_;
            this.Numeracion = Numeracion_;
            this.Ancho = Ancho_;
            this.Profundo = Profundo_;
            this.OrdenPicking = OrdenPicking_;
        }
    }
}

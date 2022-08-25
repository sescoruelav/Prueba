using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model.Entities
{
    public class TblHuecos
    {
        public int IdHueco { get; set; }
        public string Descripcion { get; set; }
        public string IdHuecoEstado { get; set; }
        public string IdHuecoTipo { get; set; }
        public string IdHuecoEstante { get; set; }
        public int IdHuecoAlmacen { get; set; }
        public int Acera { get; set; }
        public int Portal { get; set; }
        public int Piso { get; set; }
        public int Capacidad { get; set; }
        public string PickingSN { get; set; }
        public int? Bloque { get; set; }
        public int CapacidadArt { get; set; }
        public int? Clave { get; set; }
        public int? DigitoControl { get; set; }
        public string TipoCarga { get; set; }
        public int? PermiteUbicacionMultiple { get; set; }
        public TblHuecos()
        {

        }

        public TblHuecos(int IdHueco_, string Descripcion_, string IdHuecoEstado_, string IdHuecoTipo_, string IdHuecoEstante_, int IdHuecoAlmacen_, int Acera_, int Portal_, int Piso_, int Capacidad_, string PickingSN_, int Bloque_, int CapacidadArt_, int Clave_, int DigitoControl_, string TipoCarga_, int PermiteUbicacionMultiple_)
        {
            this.IdHueco = IdHueco_;
            this.Descripcion = Descripcion_;
            this.IdHuecoEstado = IdHuecoEstado_;
            this.IdHuecoTipo = IdHuecoTipo_;
            this.IdHuecoEstante = IdHuecoEstante_;
            this.IdHuecoAlmacen = IdHuecoAlmacen_;
            this.Acera = Acera_;
            this.Portal = Portal_;
            this.Piso = Piso_;
            this.Capacidad = Capacidad_;
            this.PickingSN = PickingSN_;
            this.Bloque = Bloque_;
            this.CapacidadArt = CapacidadArt_;
            this.Clave = Clave_;
            this.DigitoControl = DigitoControl_;
            this.TipoCarga = TipoCarga_;
            this.PermiteUbicacionMultiple = PermiteUbicacionMultiple_;
        }
    }
}

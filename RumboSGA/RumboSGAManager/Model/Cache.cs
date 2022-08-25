using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model
{
    public static class Cache
    {
        public static List<ElementoValor> ComboMuelleDevoluciones;

        public static bool CargarCache()
        {
            bool ok = false;

            //ComboMuelleDevoluciones = Business.GetDatosCombo(_tableScheme.CmbObject.CampoRelacionado, _tableScheme.CmbObject.CampoMostrado, _tableScheme.CmbObject.TablaRelacionada);
            ComboMuelleDevoluciones = Business.GetDatosCombo("IDHUECO", "DESCRIPCION", "TBLHUECOS");

            ok = true;

            return ok;
        }

    }
}

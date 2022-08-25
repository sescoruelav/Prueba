using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumboSGAManager.Model
{
   public class Permisos
    {
        private DataTable tablaPermisos;
        public void iniciarPermisos(int idGrupo)
        {
            tablaPermisos = ConexionSQL.getDataTable("SELECT * FROM RUMPERMGRUPOS WHERE IDGRUPO="+idGrupo);
        }
        public bool tienePermisoEscritura(int idPermiso)
        {
            DataView view = new DataView(tablaPermisos);
            view.Sort = "RECURSO";
            DataRow[] fila=tablaPermisos.Select("RECURSO="+idPermiso);
            if (fila[0]["PERMISO"].ToString()=="E")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool comprobarAcceso(int idPermiso)
        {
            DataView view = new DataView(tablaPermisos);
            view.Sort = "RECURSO";
            if (tablaPermisos.Select("RECURSO=" + idPermiso).Count()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

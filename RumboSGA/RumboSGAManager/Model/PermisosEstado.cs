using Rumbo.Core.Herramientas;
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
    public class PermisosEstado
    {
        private DataTable tablaPermisosEstado;

        public void iniciarPermisos(int idGrupo)
        {
            tablaPermisosEstado = ConexionSQL.getDataTable("SELECT * FROM RUMPERMISOSPORESTADO WHERE IDGRUPO=" + idGrupo);
            
        }
        public object[] tienePermiso(string estadoorigen, string estadofin)
        {
            object[] respuesta = new object[2];
            bool activado = Persistencia.getParametroBoolean("PERMISOCAMBIOSESTADO");
            if (activado)
            {
                DataView view = new DataView(tablaPermisosEstado);
                DataRow[] fila = tablaPermisosEstado.Select("ESTADOINICIO='" + estadoorigen + "' AND ESTADOFIN='" + estadofin + "'");
                if (fila.Length > 0)
                {
                    if (fila[0]["CORRECTO"].ToString().Equals("Y"))
                    {
                        respuesta[0] = true;
                        respuesta[1] = "";
                    }
                    else
                    {
                        respuesta[0] = false;
                        respuesta[1] = fila[0]["MENSAJE"];
                    }
                    return respuesta;
                }
                respuesta[0] = false;
                respuesta[1] = Lenguaje.traduce("No tiene permiso definido para cambiar el estado de " + estadoorigen + " a " + estadofin);
                return respuesta;
            }
            else
            {
                respuesta[0] = true;
                respuesta[1] = "";
                return respuesta;
            }
        }

    }
}

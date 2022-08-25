using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RumboSGAManager.Model
{
    public static class User
    {
        private static int idOperario;
        private static int idUsuario;
        private static int idGrupo;
        private static string nombreImpresora;
        private static string nombreUsuario;
        private static string pwdUsuario;
        private static Permisos perm;
        private static PermisosEstado permEstado;
      
        private static string Usuario { get; set; }
        private static string Pwd { get; set; }
        public static int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public static int IdGrupo { get => idGrupo; set => idGrupo = value; }
        public static int IdOperario { get => idOperario; set => idOperario = value; }
        public static string NombreImpresora { get => nombreImpresora; set => nombreImpresora = value; }
        public static string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        public static string PwdUsuario { get => pwdUsuario; set => pwdUsuario = value; }
        public static Permisos Perm { get => perm; set => perm = value; }
        public static PermisosEstado PermEstado { get => permEstado; set => permEstado = value; }

        public static void IniciarPermisos()
        {
            Perm = new Permisos();
            PermEstado = new PermisosEstado();
            Perm.iniciarPermisos(IdGrupo);          
            PermEstado.iniciarPermisos(IdGrupo);
           
        }
        public static void BuscarGrupo()
        {
            DataTable temp = ConexionSQL.getDataTable("SELECT * FROM RUMUSUGRUPO WHERE IDUSUARIO=" + IdUsuario);
            if (temp.Rows.Count > 0)
            {
                DataRow row = temp.Rows[0];
                IdGrupo = int.Parse(row["IDGRUPO"].ToString());
            }
            else
            {
                throw new Exception("El usuario " + IdUsuario + " no tiene asignado ningun grupo");
            }
        }
        private static void Permiso(RadButtonItem control, int id)
        {
            if (User.Perm.comprobarAcceso(id) == false)
            {
                control.Enabled = false;
            }
            else
            {
                if (User.Perm.tienePermisoEscritura(id) == true)
                {
                    control.Enabled = true;
                }
                else
                {
                    control.Enabled = false;
                }
            }
        }
        private static object[] PermisoEstado(string estadoAnterior, string estadoNuevo)
        {
            return User.PermEstado.tienePermiso(estadoAnterior, estadoNuevo);            
        }
    }
}

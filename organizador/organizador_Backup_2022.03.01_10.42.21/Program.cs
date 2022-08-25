using RumboSGAManager.Model.DataContext;
using System;
using System.Windows.Forms;

namespace organizador
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConexionSQL.setConnectionString("packet size=4096;data source='TCP:172.20.8.91'; Failover Partner='\';persist security info=False;initial catalog=Cronus2020;user id=sa;password=Es3a2xSQL;Pooling=True;Connection TimeOut=10");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RumboSGA.GestionAlmacen.rRbnOrganizador(RumboSGA.GestionAlmacen.rRbnOrganizador.tipoOrganizador.Pedidos_Cliente));
        }
    }
}
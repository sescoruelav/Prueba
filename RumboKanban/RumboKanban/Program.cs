using System;
using System.Windows.Forms;
using RumboSGAManager.Model.DataContext;

namespace RumboKanban
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConexionSQL.setConnectionString("packet size=4096;data source='TCP:172.20.8.91'; Failover Partner='\';persist security info=False;initial catalog=Cronus2020;user id=sa;password=Es3a2xSQL;Pooling=True;Connection TimeOut=10");

            Application.Run(new RumboKanban());
        }
    }
}
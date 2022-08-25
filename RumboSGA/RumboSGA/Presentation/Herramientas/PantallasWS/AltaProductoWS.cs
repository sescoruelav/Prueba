using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class AltaProductoWS : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AltaProductoWS(string respuesta)
        {
            try
            { 
                InitializeComponent();
                this.Show();
                DatosMonoProducto();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void DatosMonoProducto()
        {
            try
            {
                enMonoCombo.DataSource = ConexionSQL.getDataTable("SELECT * FROM TBLUNIDADESTIPO");
                enMonoCombo.DisplayMember = "DESCRIPCION";
                enMonoCombo.Text = string.Empty;
                operarioMonoproductoCombo.DataSource = ConexionSQL.getDataTable("SELECT * FROM TBLOPERARIOS");
                operarioMonoproductoCombo.DisplayMember = "NOMBRE";
                operarioMonoproductoCombo.Text = string.Empty;
                tipoPaletMonoCombo.DataSource = ConexionSQL.getDataTable("SELECT * FROM tblpaletstipo");
                tipoPaletMonoCombo.DisplayMember = "DESCRIPCION";
                tipoPaletMonoCombo.Text = string.Empty;
                maquinaMonoCombo.DataSource = ConexionSQL.getDataTable("SELECT * FROM tblmaquinas");
                maquinaMonoCombo.DisplayMember = "DESCRIPCION";
                maquinaMonoCombo.Text = string.Empty;
                estadoMonoCombo.DataSource = ConexionSQL.getDataTable("SELECT * FROM tblexistenciasestado");
                estadoMonoCombo.DisplayMember = "DESCRIPCION";
                estadoMonoCombo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
    }
}

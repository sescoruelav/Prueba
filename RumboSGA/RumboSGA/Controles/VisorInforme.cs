using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Rumbo.Core.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Controles
{
    public partial class VisorInforme : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string file;
        private string parameters = "";

        public VisorInforme(string file_, string parameters_)
        {
            InitializeComponent();
            this.file = file_;
            this.parameters = parameters_;
            this.Shown += form_Shown;
        }

        public VisorInforme(int idInforme, string parameters_)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Visor Informes");
            string informeArchivo = Informe.GetPathInforme(idInforme);
            log.Debug("Cargar Informe " + informeArchivo);
            this.file = informeArchivo;
            this.parameters = parameters_;
            this.Shown += form_Shown;
            crViewer.SetProductLocale(CultureInfo.CurrentUICulture.LCID);
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.TopLevel = true;
            this.Focus();
            this.BringToFront();
            this.Activate();
            bool foco = this.ContainsFocus;
            log.Debug("Cargar Foco " + foco);
            this.TopMost = false;
        }

        private void VisorInforme_Load(object sender, EventArgs e)
        {
            try
            {
                log.Debug("Cargando informe " + file);
                Informe i = new Informe();
                ReportDocument inf = i.ReportLoad(file);

                string conectionString = ConexionSQL.getConnectionString();
                log.Debug("Antes aplicar credenciales " + inf.DataSourceConnections[0].DatabaseName + " " + inf.DataSourceConnections[0].ServerName + " " + inf.DataSourceConnections[0].UserID + " " + inf.DataSourceConnections[0].Password);
                inf = Informe.AplicarCredentialsDesdeConnectionString(inf, conectionString);
                log.Debug("Despues aplicar credenciales " + inf.DataSourceConnections[0].DatabaseName + " " + inf.DataSourceConnections[0].ServerName + " " + inf.DataSourceConnections[0].UserID + " " + inf.DataSourceConnections[0].Password);

                if (!parameters.Equals(""))
                {
                    log.Debug("Aplicando parametros");
                    inf = Informe.AplicarParametros(inf, parameters, true);
                }

                log.Debug("Despues aplicar parametros " + inf.DataSourceConnections[0].DatabaseName + " " + inf.DataSourceConnections[0].ServerName + " " + inf.DataSourceConnections[0].UserID + " " + inf.DataSourceConnections[0].Password);
                crViewer.ReportSource = inf;
                crViewer.Show();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
    }
}
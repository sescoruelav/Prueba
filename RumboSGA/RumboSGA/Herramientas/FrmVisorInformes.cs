using CrystalDecisions.CrystalReports.Engine;
using RumboSGA.Controles;
using RumboSGA.Presentation.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.ReportViewer.WinForms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using static RumboSGA.Presentation.Herramientas.RumboReport;

namespace RumboSGA.Herramientas
{
    public partial class FrmVisorInformes : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        System.ComponentModel.ComponentResourceManager resources;
        public string ViewerName { get; set; } = "Report viewer";
        ReportEngine reportEngine = ReportEngine.ReportServer;
        public string ReportURI { get; set; }
        int timeOut = 100;
        Telerik.Reporting.UriReportSource uriReportSource = new Telerik.Reporting.UriReportSource();
        DataTable informes;
        private string tipo;
        private const string CRYSTAL = "CR";
        private const string TELERIK = "TK";
        public FrmVisorInformes()
        {
            InitializeComponent();
            InicializarDataTableInformes();            
            CrearMenuInformes();
            CrearEventos();
        }

        private void InicializarDataTableInformes()
        {
            try
            {
             informes =ConexionSQL.getDataTable("SELECT * FROM dbo.RUMINFORMES WHERE CARGAR ='S'");
            }
            catch(Exception ex)
            {

            }

        }

        private void CrearEventos()
        {
            this.rMenuInformes.NodeMouseClick += rMenuInformes_SelectedNodeChanged;
            this.radCollapsiblePanel1.CollapsiblePanelElement.HeaderElement.ShowHeaderLine = false;
            this.radCollapsiblePanel1.EnableAnimation = false;
            this.radCollapsiblePanel1.Collapsed += RadCollapsiblePanel1_Collapsed;
            this.radCollapsiblePanel1.Expanded += RadCollapsiblePanel1_Expanded;
            ThemeResolutionService.ApplicationThemeName = "Windows8";
        }

        private void rMenuInformes_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            try
            {
                string idInforme = e.Node.Name;
                int n;
                bool isNumeric = int.TryParse(idInforme, out n);
                if (isNumeric)
                {
                    DataRow[] drInforme = informes.Select("IDINFORME = " + idInforme + "");
                    if (drInforme.Length > 0)
                    {
                        DataRow informe = drInforme[0];
                        switch (informe["TIPO"].ToString())
                        {
                            case CRYSTAL:
                                CargarInformeCrystal(Convert.ToInt32(idInforme));
                                break;
                            case TELERIK:
                                CargarInformeTelerik(informe);
                                break;
                        }

                    }
                }

                }catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void CargarInformeTelerik(DataRow informe)
        {


            RumboReport rpt = new RumboReport(RumboReport.ReportEngine.ReportServer);
            rpt.rptVwr = tlkViewer;
            rpt.ServerUrl = Persistencia.ReportServerUrl;
            rpt.ServerUser = Persistencia.ReportServerUser;
            rpt.ServerPwd = Persistencia.ReportServerPwd;
            rpt.ReportCategory = informe["CATEGORIA"].ToString();//"NewBrunswick";

            rpt.ReportName = informe["NOMBRE"].ToString();//"DeliverySlip";
            if (ReportURI is null)
                ReportURI = rpt.ReportCategory + "/" + rpt.ReportName;
            uriReportSource.Uri = ReportURI;
            rpt.rptVwr.ReportSource = uriReportSource;
            
            rpt.rptVwr =CrearInformeTelerik(rpt.rptVwr);
            //rpt.Show();
            this.tlkViewer = rpt.rptVwr;

            //tlkViewer.RefreshReport();
            //tlkViewer.Preview();
            MostrarViewer(TELERIK);


        }

        private void MostrarViewer(string tipo)
        {
            switch (tipo)
            {
                case TELERIK:
                    this.tableLayoutPanel1.Controls.Remove(crViewer);
                    this.tableLayoutPanel1.Controls.Add(this.tlkViewer, 1, 0);
                    tlkViewer.Visible = true;
                    crViewer.Visible = false;
                    tlkViewer.RefreshReport();
                    break;
                    
                case CRYSTAL:
                    this.tableLayoutPanel1.Controls.Remove(tlkViewer);
                    this.tableLayoutPanel1.Controls.Add(this.crViewer, 1, 0);
                    tlkViewer.Visible = false;
                    crViewer.Visible = true;
                    break;
            }
        }

        private ReportViewer CrearInformeTelerik( ReportViewer rptViewer)
        {
            try
            {
                resources = new System.ComponentModel.ComponentResourceManager(typeof(RptVwrFrm));
                
                //rptViewer.AccessibilityKeyMap = ((System.Collections.Generic.Dictionary<int, Telerik.ReportViewer.Common.Accessibility.ShortcutKeys>)(resources.GetObject("rptViewer.AccessibilityKeyMap")));
                rptViewer.Dock = System.Windows.Forms.DockStyle.Fill;
                rptViewer.EnableAccessibility = true;
                rptViewer.Location = new System.Drawing.Point(0, 0);
                rptViewer.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
                rptViewer.Name = ViewerName;
                rptViewer.ReportEngineConnection = "engine=" + reportEngine.ToString() + ";uri=" + Persistencia.ReportServerUrl + ";";
                rptViewer.ReportEngineConnection += "username=" + Persistencia.ReportServerUser + ";password=" +  Persistencia.ReportServerPwd + ";timeout=" + timeOut + ";";
                //uriReportSource = new Telerik.Reporting.UriReportSource();//lo muevo al constructor para añadir parametros
                /*if (rptViewer.ReportURI is null)
                    ReportURI = "NewBrunswick"ReportCategory + "/" +/* ReportName"DeliverySlip";//se  puede usar uri o categoria y report*/
                //uriReportSource.Uri = ReportURI;
                //rptViewer.ReportSource = uriReportSource;
                rptViewer.Size = new System.Drawing.Size(1350, 1077);
                rptViewer.TabIndex = 0;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);//TODO Mejorar gestion
            }
            return rptViewer;
        }

        private void RadCollapsiblePanel1_Expanded(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnStyles[0].Width = 15F;
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
        }

        private void RadCollapsiblePanel1_Collapsed(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnStyles[0].Width = 2F;
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
        }

        private void CargarInformeCrystal(int idInforme)
        {
            try
            {
                string informeArchivo = Informe.GetPathInforme(idInforme);
                log.Debug("Cargando informe " + informeArchivo);
                Informe i = new Informe();
                ReportDocument inf = i.ReportLoad(informeArchivo);
                                       
                string conectionString = ConexionSQL.getConnectionString();
                log.Debug("Antes aplicar credenciales " + inf.DataSourceConnections[0].DatabaseName + " " + inf.DataSourceConnections[0].ServerName + " " + inf.DataSourceConnections[0].UserID + " " + inf.DataSourceConnections[0].Password);
                inf = Informe.AplicarCredentialsDesdeConnectionString(inf, conectionString);
                log.Debug("Despues aplicar credenciales " + inf.DataSourceConnections[0].DatabaseName + " " + inf.DataSourceConnections[0].ServerName + " " + inf.DataSourceConnections[0].UserID + " " + inf.DataSourceConnections[0].Password);                
                crViewer.ReportSource = inf;
                crViewer.Show();
                MostrarViewer(CRYSTAL);



            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void CrearMenuInformes()
        {
            try
            {   
                DataTable tipos = ConexionSQL.getDataTable("SELECT DISTINCT CLASIFICACION FROM dbo.RUMINFORMES WHERE CARGAR ='S'");
                foreach (DataRow tipo in tipos.Rows)
                {
                    CrearMenuInformes(tipo);
                }
            }catch(Exception ex)
            {

            }
        }

        private void CrearMenuInformes(DataRow tipo)
        {
            try
            {
                RadTreeNode clasificacion = new RadTreeNode();
                clasificacion.Text = tipo["CLASIFICACION"].ToString();
                clasificacion.Name = tipo["CLASIFICACION"].ToString();
                
                clasificacion.Font=new Font(this.rMenuInformes.Font.FontFamily, this.rMenuInformes.Font.Size, FontStyle.Bold);
               
                rMenuInformes.Nodes.Add(clasificacion);
                //DataTable informes = ConexionSQL.getDataTable("SELECT * FROM dbo.RUMINFORMES WHERE CLASIFICACION ='" + tipo["CLASIFICACION"] + "'");
                DataRow[] drInformes = informes.Select("CLASIFICACION = '" + tipo["CLASIFICACION"] + "'"); 
                foreach (DataRow informe in drInformes)
                {
                    RadTreeNode informeItem = new RadTreeNode();
                    informeItem.Text = informe["TITULO"].ToString();
                    informeItem.Name = informe["IDINFORME"].ToString();                                
                    clasificacion.Nodes.Add(informeItem);
                }
            }
            catch (Exception ex)
            {

            }
        }
}
}

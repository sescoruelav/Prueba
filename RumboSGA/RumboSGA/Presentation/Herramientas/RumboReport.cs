using Rumbo.Core.Herramientas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.Reporting.Processing;
using Telerik.ReportViewer.WinForms;
using Telerik.ReportServer.HttpClient;
using Telerik.ReportServer.Services.Models;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace RumboSGA.Presentation.Herramientas
{
    public class RumboReport
    {
        public enum ReportEngine { Embebed, ReportServer, WebBrowser }
        ReportEngine reportEngine = ReportEngine.ReportServer;//valor por defecto.
        RptVwrFrm rptVwrFrm;
        public ReportViewer rptVwr;
       
        Telerik.Reporting.UriReportSource uriReportSource;
        Report rpt;
        int timeOut = 100;
        private bool linksOpenInSystemBrowser = false;
        string parametrosUrl="";
        System.ComponentModel.ComponentResourceManager resources;
        public string ServerUser { get ; set; } //TODO = User.Usuario;
        public string ServerPwd { get; set; }//TODO = User.Pwd;
        public string ServerUrl { get; set; }
        public string ViewerName { get; set; } ="Report viewer";
        public string ReportURI { get; set; }
        public string ReportCategory { get; set; }
        private string ReportCategoryId = "";
        public string ReportName { get; set; }
        private string ReportNameId = "";
        public string SmtpServer { get; set; }= "smtp.gmail.com";
        public string ReportUrl { get; set; }

        public RumboReport(ReportEngine tipoReport)
        {
            reportEngine = tipoReport;
            switch (reportEngine)
            {
                case ReportEngine.ReportServer:
                case ReportEngine.Embebed:
                    uriReportSource = new Telerik.Reporting.UriReportSource();
                    break;
                case ReportEngine.WebBrowser:

                    break;
            }
            
        }
        public void Show()
        {
            switch (reportEngine)
            {
                case ReportEngine.ReportServer:
                    rptVwr = CrearViewer(ref rptVwr);
                    rptVwrFrm = CrearFrm(rptVwr);
                    rptVwrFrm.Show();
                    break;
                case ReportEngine.WebBrowser:
                    OpenUrl(ReportUrl+ReportCategory+ "/"+ReportName+parametrosUrl);
                    break;
            }
        }

        private ReportViewer CrearViewer(ref ReportViewer rptViewer)
        {           
            try
            {
                resources = new System.ComponentModel.ComponentResourceManager(typeof(RptVwrFrm));
                rptViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
                //rptViewer.AccessibilityKeyMap = ((System.Collections.Generic.Dictionary<int, Telerik.ReportViewer.Common.Accessibility.ShortcutKeys>)(resources.GetObject("rptViewer.AccessibilityKeyMap")));
                rptViewer.Dock = System.Windows.Forms.DockStyle.Fill;
                rptViewer.EnableAccessibility = true;
                rptViewer.Location = new System.Drawing.Point(0, 0);
                rptViewer.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
                rptViewer.Name =ViewerName ;
                rptViewer.ReportEngineConnection = "engine=" + reportEngine.ToString()+";uri=" + ServerUrl+";";
                rptViewer.ReportEngineConnection += "username=" + ServerUser + ";password=" + ServerPwd + ";timeout="+timeOut+";";
                //uriReportSource = new Telerik.Reporting.UriReportSource();//lo muevo al constructor para añadir parametros
                if (ReportURI is null)
                    ReportURI = ReportCategory + "/" + ReportName;//se  puede usar uri o categoria y report
                uriReportSource.Uri = ReportURI;
                rptViewer.ReportSource = uriReportSource;
                rptViewer.Size = new System.Drawing.Size(1350, 1077);
                rptViewer.TabIndex = 0;
                return rptViewer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);//TODO Mejorar gestion
            }
        }
        public void SetParameter(string nombre, int valor)
        {
                SetParameter(nombre, valor.ToString());
        }
        public void SetParameter(string nombre,string valor)
        {
            switch (reportEngine)
            {
                case ReportEngine.ReportServer:
                case ReportEngine.Embebed:
                    uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter(nombre, valor));
                    break;
                case ReportEngine.WebBrowser:
                    if (parametrosUrl.Length == 0)
                        parametrosUrl = "?";
                    else
                        parametrosUrl += "&";
                    parametrosUrl += nombre + "=" + valor;
                    break;
            }
        }
        private RptVwrFrm CrearFrm(ReportViewer rv)
        {
            RptVwrFrm frm = new RptVwrFrm(rv);
            return frm;
        }
        public void MailReport(string from, string to, string subject, string body)
        {
            try
            {
                Settings rsSettings = new Settings()
                {
                    BaseAddress = ServerUrl
                };
                //hacemos login en el servidor
                ReportServerClient rsClient = new ReportServerClient(rsSettings);
                rsClient.Login(ServerUser, ServerPwd);
                //categoria
                string ReportCategoryId="";

                foreach(Category myCategory in rsClient.GetCategories())
                {
                    if (myCategory.Name.ToUpper().Equals(ReportCategory.ToUpper()))
                        ReportCategoryId = myCategory.Id;
                }
                foreach (ReportInfo myReport in rsClient.GetCategoryReports(ReportCategoryId))
                {
                    if (myReport.Name.ToUpper().Equals(ReportName.ToUpper()))
                    {
                        ReportNameId = myReport.Id;
                    }
                }
                CreateDocumentData docuementModel = new CreateDocumentData()
                {
                    Format = "PDF",
                    ReportId = ReportNameId
                };
                docuementModel.ParameterValues = new Dictionary<string, object>();
                docuementModel.ParameterValues.Add("Pedido", 122);

                string documentId = rsClient.CreateDocument(docuementModel);
                byte[] documentBytes = rsClient.GetDocument(documentId);

                string documentPath = $"C:\\Users\\pboix\\Documents\\{ReportName}.{docuementModel.Format}";//TODO
                FileStream fs;
                using ( fs = new FileStream(documentPath, FileMode.Create))
                {
                    fs.Write(documentBytes, 0, documentBytes.Length);
                }
                Attachment attachment = new Attachment(documentPath);
                MailMessage msg = new MailMessage(from, to, subject, body);
                msg.To.Add(new MailAddress(to));
                msg.From = new MailAddress(from);
                msg.Attachments.Add(attachment);
                //Preparo el email
                using (SmtpClient client = new SmtpClient(SmtpServer, 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(from, "xxxxxxxx");//TODO
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Host = SmtpServer;
                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    //envío el email
                    client.Send(msg);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error enviando informe por email")+ "\n"+ ex.Message);
            }
        }


        void rptVwr_ExportEnd(object sender, Telerik.ReportViewer.Common.ExportEndEventArgs args)
        {
            if (null != args.Exception)
            {
                //some logic if report processing is not successful
            }
            else
            {
                //suppress the viewer SaveFileDialog and use your own
                args.Handled = true;
                var fileName = args.DocumentName;
                var extension = args.DocumentExtension;
                var filter = string.Format("{0} (*.{1})|*.{1}|All Files (*.*)|*.*",
                                            fileName,
                                            extension);

                var saveFileDlg = new SaveFileDialog
                {
                    Filter = filter,
                    FileName = fileName + "." + extension,
                };

                if (saveFileDlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var fs = new FileStream(saveFileDlg.FileName, FileMode.Create))
                        {
                            fs.Write(args.DocumentBytes, 0, args.DocumentBytes.Length);
                        }

                        //Optionally open the file
                        if (!string.IsNullOrEmpty(saveFileDlg.FileName))
                        {
                            System.Diagnostics.Process.Start(saveFileDlg.FileName);
                        }
                    }
                    catch (Exception)
                    {
                        //Handle exception
                    }
                }
            }
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
        //############################################### CLASE REPORT FORM #######
        internal partial class RptVwrFrm : Form
        {
            private Telerik.ReportViewer.WinForms.ReportViewer reportViewer;
            private System.ComponentModel.IContainer components = null;
            public RptVwrFrm(ReportViewer rv)
            {
                reportViewer = rv;
                InitializeComponent();
            }

            private void ReportViewerForm1_Load(object sender, EventArgs e)
            {
                reportViewer.RefreshReport();
            }
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            private void InitializeComponent()
            {

                try
                {
                    this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    this.ClientSize = new System.Drawing.Size(1350, 1077);
                    this.Controls.Add(this.reportViewer);
                    this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
                    this.Name = "ReportViewerForm";
                    this.Text = "Report Viewer Form";
                    this.Load += new System.EventHandler(this.ReportViewerForm1_Load);
                    this.ResumeLayout(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se he producido un error "+ ex);//TODO 
                }

            }
        }

    }
}

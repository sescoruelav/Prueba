using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Properties;
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class CrearIncidencia : Telerik.WinControls.UI.RadForm
    {
        public static List<string> lsFileNames = new List<string>();
        List<string> lsIncidencias = new List<string>(3) { Lenguaje.traduce("Tarea"),Lenguaje.traduce("Solicitud de servicio"),Lenguaje.traduce("Incidencia del servicio") };
        public CrearIncidencia()
        {
            InitializeComponent();
            rellenarComboBoxIncidenciasTipo();
        }
        public void rellenarComboBoxIncidenciasTipo()
        {
            try
            {
                comboBox1.DataSource = lsIncidencias;
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox2.SelectedIndex=0;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }
        public async Task CreateJiraIssueAsync()
        {
            string result = string.Empty;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://servicedesk.alfatecsistemas.es/jira/rest/api/2/issue"))
                    {
                        string key = "10314";
                        List<string> camposConfiguracion=getConfigOptions();
                        if (radTextBoxControl1.Text != string.Empty && radTextBoxControl2.Text!=string.Empty) { 
                            string json = "{" +
                                "\"fields\": " +
                                "{\"project\":{\"id\": \"10314\"}," +
                                "\"summary\": \""+radTextBoxControl2.Text +"\"," +
                                "\"description\":  \"" + radTextBoxControl1.Text + "\"," +
                                "\"priority\": {\"name\": \"" + comboBox2.Text + "\"}," +
                                //"\"components\": {\"id\": \"10913\"}," +
                                //"\"customfield_13000\": [\"Europastry (CAUSER-2511)\"]," +
                                //"\"customfield_13002\": [\"Madrid - Europastry (CAUSER-4009)\"]," +
                                "\"issuetype\": {\"name\": \"" +comboBox1.Text+"\"}}}";
                            request.Headers.TryAddWithoutValidation("Accept", "application/json");

                            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(camposConfiguracion[1]+":"+camposConfiguracion[2]));
                            request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                            var response = await httpClient.SendAsync(request);
                            var a = response.Content.ReadAsStringAsync();
                            var rawResponse = a.GetAwaiter().GetResult();
                            RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                        }
                        else
                        {
                            RadMessageBox.Show(Lenguaje.traduce("Rellena los campos"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void SendFileToServer(string issueKey,FileInfo fi)
        {
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            byte[] fileContents = File.ReadAllBytes(fi.FullName);

            //https://your-domain.atlassian.net/rest/api/2/issue/XYZ-315/attachments
            string urlPath = "https://servicedesk.alfatecsistemas.es/jira/rest/api/2/issue/" + issueKey + "/attachments";
            //Uri webService = new Uri(string.Format("{0}/{1}/attachments", urlPath, IssueKey));
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, urlPath);
            requestMessage.Headers.ExpectContinue = false;

            MultipartFormDataContent multiPartContent = new MultipartFormDataContent("----MyGreatBoundary");
            ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
            byteArrayContent.Headers.Add("Content-Type", "image/png");
            byteArrayContent.Headers.Add("X-Atlassian-Token", "no-check");
            multiPartContent.Add(byteArrayContent, "file", fi.Name);
            requestMessage.Content = multiPartContent;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("tolmeda" + ":" + "Madeira74&&")));
            httpClient.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
            try
            {
                Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                HttpResponseMessage httpResponse = httpRequest.Result;
                HttpStatusCode statusCode = httpResponse.StatusCode;
                HttpContent responseContent = httpResponse.Content;

                if (responseContent != null)
                {
                    Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
                    String stringContents = stringContentsTask.Result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void rumButton1_Click(object sender, EventArgs e)
        {
            try
            {
                _ = CreateJiraIssueAsync();
                //MessageBox.Show("",,);
                if (lsFileNames.Count > 0)
                {
                    for (int i = 0; i < lsFileNames.Count; i++)
                    {
                        FileInfo fi = new FileInfo(lsFileNames[i]);
                        SendFileToServer("287195", fi);
                    }
                }
                
            }
            catch (Exception ex )
            {

                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }
        private string crearAttachment()
        {
            try
            {
                return "";
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public List<string> getConfigOptions()
        {
            List<string> cs = new List<string>();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load("C:\\Rumbo\\hola.config");
                var Cliente = xml.SelectSingleNode("JiraConfig/Cliente"); 
                var usuario = xml.SelectSingleNode("JiraConfig/usuarioJira");
                var contraseña = xml.SelectSingleNode("JiraConfig/contrasenyaJira");
                if (Cliente.InnerText != string.Empty && usuario.InnerText != string.Empty && contraseña.InnerText != string.Empty)
                {
                    cs.Add(Cliente.InnerText);
                    cs.Add(usuario.InnerText);
                    cs.Add(contraseña.InnerText);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return cs;
        }
        private void rumButton2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                // image filters  
                open.Filter = "All Files|*.*";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(open.FileName);
                    ArchivoJira arch = new ArchivoJira(open.FileName,fi.Name,lsFileNames);
                    // display image in picture box  
                    
                    
                    // image file path  
                    lsFileNames.Add(open.FileName);
                    
                    panel1.Controls.Add(arch);
                    arch.Dock = DockStyle.Left;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void panel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            //lsFileNames.Remove();
            //cambiarList(lsFileNames, )
        }
        public List<string> GetLista()
        {
            return lsFileNames;
        }
        public void EditarLista(List<string> str)
        {
            lsFileNames = str;
        }
        
        
    }
}

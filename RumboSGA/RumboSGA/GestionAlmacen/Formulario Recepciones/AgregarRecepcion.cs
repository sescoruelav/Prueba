using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGA.RecepcionMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.FormularioRecepciones
{
    public partial class AgregarRecepcion : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Object[] jsonLineas;
        string idpro;
        public AgregarRecepcion(Object[] lineas,string idproveedor,string codigoProveedor, ArrayList pedidosPro)
        {
            InitializeComponent();
            ConfigurarIdiomaEtiquetas();
            transportista.DataSource =ConexionSQL.getDataTable("SELECT IDAGENCIA,NOMBRE FROM TBLAGENCIAS");
            transportista.DisplayMember = "NOMBRE";
            transportista.ValueMember = "IDAGENCIA";
            transportista.AutoCompleteMode = AutoCompleteMode.Append;

            muelle.DataSource = ConexionSQL.getDataTable("SELECT IDHUECO,DESCRIPCION FROM TBLHUECOS WHERE IDHUECOESTANTE='MU'");
            muelle.ValueMember = "IDHUECO";
            muelle.DisplayMember = "DESCRIPCION";
            muelle.AutoCompleteMode = AutoCompleteMode.Append;
            muelle.BestFitColumns();
            fecha.Value = DateTime.Now;
            transportista.BestFitColumns();
            jsonLineas = lineas;
            idpro = idproveedor;
            if (pedidosPro.Count > 1)
            {
                recepcion.Text = "REC-" + codigoProveedor +"-" +DateTime.Now.ToShortDateString();
            }
            else
            {
                recepcion.Text = "REC-" + pedidosPro[0].ToString()+"-" + DateTime.Now.ToShortDateString();
            }
            
            radButton1.Click += btnAceptar_Click;
            radButton2.Click += btnCerrar_Click;
            ValoresDefecto();
        }

        private void ValoresDefecto()
        {
            try
            {
                object valor =Business.getValorDefectoCampoTabla("TBLRECEPCIONESCAB", "IDAGENCIA");
                if (valor != null)
                {
                    transportista.SelectedValue = valor;
                }
                valor = Business.getValorDefectoCampoTabla("TBLRECEPCIONESCAB", "MUELLE");
                if (valor != null)
                {
                    muelle.SelectedValue = valor;
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        public AgregarRecepcion(string albaranText,string fechaText,string muelleText,string transportText,string matriculaText,string choferText,string obsText)
        {
            InitializeComponent();
            ConfigurarIdiomaEtiquetas();
            transportista.DataSource = ConexionSQL.getDataTable("SELECT IDAGENCIA,NOMBRE FROM TBLAGENCIAS");
            transportista.DisplayMember = "NOMBRE";
            transportista.ValueMember = "IDAGENCIA";
            transportista.AutoCompleteMode = AutoCompleteMode.Append;

            muelle.DataSource = ConexionSQL.getDataTable("SELECT IDHUECO,DESCRIPCION FROM TBLHUECOS WHERE IDHUECOESTANTE='MU'");
            muelle.ValueMember="IDHUECO";
            muelle.DisplayMember = "DESCRIPCION";
            muelle.AutoCompleteMode = AutoCompleteMode.Append;
            muelle.BestFitColumns();
            transportista.BestFitColumns();
            albaran.Text = albaranText;
            transportista.Text = transportText;
            matricula.Text = matriculaText;
            chofer.Text = choferText;
            observaciones.Text = obsText;

            muelle.Text = muelleText;
            fecha.Text = fechaText;

            radButton1.Click += btnAceptar_Click;

        }

        private void ConfigurarIdiomaEtiquetas()
        {
            this.Text = Lenguaje.traduce("Crear Recepción");
            lblRecepcion.Text = strings.Recepcion;
            lblAlbaran.Text = strings.AlbaranProveedor;
            lblFecha.Text = strings.FechaRecepcion;
            lblMuelle.Text = strings.Muelle;
            lblTransportista.Text = strings.Transportista;
            lblMatricula.Text = strings.Matricula;
            lblChofer.Text = strings.Chofer;
            lblObservaciones.Text = strings.Obs;
            //lblRemolque.Text = strings.Remolque;
        }
        private string formarJSON()
        {
            dynamic objDinamico = new ExpandoObject();

            try
            {
                int intMuelle, intIdprov;
                int.TryParse(muelle.SelectedValue.ToString(), out intMuelle);
                int.TryParse(idpro, out intIdprov);
                objDinamico.recepcion = recepcion.Text;
                objDinamico.muelle = intMuelle;
                objDinamico.albarantransportista = albaran.Text;
                objDinamico.idproveedor = intIdprov;
                objDinamico.matriculacamion = matricula.Text;
                objDinamico.idagencia = transportista.SelectedValue;
                objDinamico.fecharecepcion = fecha.Value.ToString();
                objDinamico.chofer = chofer.Text;
                objDinamico.observaciones = observaciones.Text;
                objDinamico.lineas = jsonLineas;
                objDinamico.error = "";
            }
            catch (Exception e)
            {
                log.Error("Mensaje:"+e.Message+"||StackTrace:"+e.StackTrace);
            }



            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio Crear Recepcion ProveedoresPedidosCabGridView" + User.IdUsuario);
                string json = formarJSON();               
                json = "[" + json + "]";
                log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " json:" + json);
                string resp = llamarWebService(json);
                if (String.IsNullOrEmpty(resp))
                {
                    RadMessageBox.Show(this,Lenguaje.traduce("No hay respuesta del web service"),"Error");
                    return;
                }
                string input = resp;
                var jss = new JavaScriptSerializer();
                bool lineasCorrectas = true;
                string errorLineas = string.Empty;

                dynamic d = jss.DeserializeObject(input);
                int i = 0;

                /*TODO: Se ha realizado este fix rápido(comprobar que d no sea nulo) para que la aplicación
                no dé una excepción no controlada al aceptar una recepción.
                Parece ser que la recepción funciona correctamente y se guardan las cosas en la BBDD,
                pero podría haber algún comportamiento inesperado.
                El problema parece provenir del WS o de la configuración del firewall.
                03/08: Se ha hablado con Javi y el problema parece solucionado, parece ser que era una incompatibilidad del JDK. Si vuelve a
                dar problemas, reportarlo.
                 */
                if (d != null)
                {
                    if (d[0]["error"] != string.Empty)
                    {
                        throw new Exception(Lenguaje.traduce(d[0]["error"]));
                        return;

                    }
                    foreach (var item in d[0]["lineas"])
                    {

                        var a = d[0]["lineas"][i]["error"];
                        if (a != string.Empty)
                        {
                            errorLineas = a;
                            lineasCorrectas = false;
                            log.Error("Error creando recepción: " + resp);
                            break;
                        }
                    }
                    Console.WriteLine(d[0]["error"]);
                    if (d[0]["error"] == string.Empty && lineasCorrectas)
                    {
                        this.DialogResult = DialogResult.OK;

                    }
                    else
                    {
                        this.DialogResult = DialogResult.Abort;
                        if (d[0]["error"] != string.Empty)
                        {
                            MessageBox.Show(Lenguaje.traduce(d[0]["error"]));

                        }
                        else
                        {
                            MessageBox.Show(Lenguaje.traduce(errorLineas));
                        }
                    }
                }
                this.Cursor = Cursors.Arrow;
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);


            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void btnCerrar_Click(object sender,EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }
        private string llamarWebService(string json)
        {
            log.Info(DateTime.Now.ToString("HH:mm:ss:ffff") + " Inicio webservicerecepcion.crearRecepcionCabYLin" + User.IdUsuario);
            WSRecepcionMotorClient webservicerecepcion = new WSRecepcionMotorClient();
            string respuesta = webservicerecepcion.crearRecepcionCabYLin(json);
            return respuesta;
             
        }
    }
}

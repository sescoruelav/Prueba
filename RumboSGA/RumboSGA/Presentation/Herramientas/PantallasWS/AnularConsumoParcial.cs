using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGA.ReservaMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class AnularConsumoParcial : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        RadForm form=new RadForm();
        public int idSalida;
        DataTable dtSalidas;
        private int tipoPresentacion = 0;
        private int idPresentacion = 0;


        public AnularConsumoParcial(int idSalida_)
        {
            InitializeComponent();
            this.idSalida = idSalida_;            
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.Text = Lenguaje.traduce("Datos Consumo");            
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            int tipoPres = Persistencia.getParametroInt("TIPOPRESENTACION");
            if (tipoPres < 0)
            {
                tipoPresentacion = 0;
            }
            else
            {
                tipoPresentacion = Convert.ToInt32(tipoPres);
            }

        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {

            try
            {
                String jsonWS = "";
                WSOrdenProduccionMotorClient wesm = new WSOrdenProduccionMotorClient();
                int cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, Convert.ToDouble(txtCantidad.Text));
                int idHueco = Convert.ToInt32(radComboBoxUbicacion.SelectedValue);
                jsonWS = formarJSONAnularConsumoParcial(idSalida, cantidadPalet,idHueco);
                if (jsonWS.Length > 0)
                {
                    jsonWS = "[" + jsonWS + "]";
                    wesm.anularConsumoParcial(jsonWS);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    form.Close();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Error confirmando datos "), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            
        }
        private void btnCancelar_Click(object sender,EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            form.Close();
        }

        

        private void AnularConsumoParcial_Load(object sender, EventArgs e)
        {
            try
            {
                dtSalidas = Business.GetDatosSalida(idSalida);
                if (dtSalidas.Rows.Count > 0)
                {
                    lblbTxtMatricula.Text = dtSalidas.Rows[0]["identrada"].ToString();
                    lblTxtSSCC.Text = dtSalidas.Rows[0]["SSCC"].ToString();
                    lblTxtCodArticulo.Text = dtSalidas.Rows[0]["referencia"].ToString();
                    lblTxtDescArticulo.Text = dtSalidas.Rows[0]["descripcion"].ToString();                    
                    txtCantidad.Text = dtSalidas.Rows[0]["cantidad"].ToString();                   
                    lblTxtLote.Text = dtSalidas.Rows[0]["lote"].ToString();
                    idPresentacion = Convert.ToInt32(dtSalidas.Rows[0]["idpresentacion"]);
                    int tipoPresArticulo = Convert.ToInt32(dtSalidas.Rows[0]["TIPOPRESENTACION"]);
                    if (tipoPresArticulo > 0)
                    {
                        tipoPresentacion = tipoPresArticulo;
                    }



                    if (tipoPresentacion == 0 || idPresentacion == 0)
                    {

                       
                        txtCantidad.Text = dtSalidas.Rows[0]["cantidad"].ToString();

                    }
                    else
                    {
                        object[] pres = Presentaciones.getTipoUnidadPresentacionVisualizacion(idPresentacion);
                        string idUnidadTipoPres = pres[1].ToString();
                        lblTxtTipoUnidad.Text = idUnidadTipoPres;
                        object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(dtSalidas.Rows[0]["cantidad"]));
                        txtCantidad.Text = presC[0].ToString();
                       
                    }
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                    
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void AnularConsumoParcial_Shown(object sender, EventArgs e)
        {
            radComboBoxUbicacion.SelectedValue = dtSalidas.Rows[0]["IDHUECO"];
        }
        private string formarJSONAnularConsumoParcial(int idSalida, int cantidad, int idhueco)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idsalida = idSalida;
            objDinamico.cantidad = cantidad;
            objDinamico.idhueco = idhueco;
            objDinamico.idoperario = User.IdOperario;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }
    }
}

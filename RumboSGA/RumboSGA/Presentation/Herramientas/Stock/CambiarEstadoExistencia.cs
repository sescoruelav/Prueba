using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class CambiarEstadoExistencia : Telerik.WinControls.UI.ShapedForm
    {
        List<string[]> campos;
        List<string[]> existencias;
        public string itemSeleccionado;
        public bool exito = false;
        bool activadoPermisoCambioEstado = Persistencia.getParametroBoolean("PERMISOCAMBIOSESTADO");
        public CambiarEstadoExistencia(List<string[]> celdas)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Cambiar estado de existencia");
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            lblCambiarEstado.Text = Lenguaje.traduce(strings.LabelCambiarEstado);
            campos = celdas;
           
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                    existencias = new List<string[]>();
                    List<string> errores = new List<string>();
                    string error = string.Empty;
                //Revisamos si tiene permiso para el cambio de estado
                foreach (var campo in campos)
                {
                    itemSeleccionado=radDropDownList1.SelectedValue.ToString();
                    object[] respuesta = User.PermEstado.tienePermiso(campo[1], itemSeleccionado);
                    if (respuesta != null)
                    {
                        bool tienePermiso = (bool)respuesta[0];
                        if (!tienePermiso)
                        {

                            errores.Add(Lenguaje.traduce(respuesta[1].ToString()));
                        }
                        else
                        {
                            existencias.Add(campo);
                        }
                    }
                    else
                    {

                    }
                }

                    Object[] arrayLineas = new Object[existencias.Count];
                    
                    int i = 0;
                    foreach (var existencia in existencias)
                    {
                        dynamic objDinamico = new ExpandoObject();
                        objDinamico.identrada = existencia[0];
                        objDinamico.idexistenciaestado = itemSeleccionado;
                        objDinamico.Error = "";                    
                        arrayLineas[i] = objDinamico;
                   
                        i++;
                    }
                if (arrayLineas.Length > 0)
                {
                    string json = JsonConvert.SerializeObject(arrayLineas);
                    WSEntradaMotorClient wse = new WSEntradaMotorClient();
                    String respuesta = wse.cambiarEstadoExistencias(json, User.IdOperario, 0);
                    List<dynamic> errorText = JsonConvert.DeserializeObject<List<dynamic>>(respuesta);

                    foreach (dynamic obj in errorText)
                    {

                        if (obj.error == string.Empty)
                        { }
                        else
                        {
                            //MessageBox.Show(errorText[i].error);
                            errores.Add(Convert.ToString(obj.error));
                        }
                    }
                }
                    if (errores.Count > 0)
                    {
                        String errorTxt = "";

                        foreach (string errorStr in errores)
                        {
                            errorTxt = errorTxt + System.Environment.NewLine + errorStr;
                        }
                        RadMessageBox.Show(errorTxt, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce(strings.AccionCompletada), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                exito = false;
            }
            this.Close();
            exito = true;
        }

       

        /*private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            itemSeleccionado = radDropDownList1.SelectedValue.ToString();
        }*/

        private void CambiarEstadoExistencia_Load(object sender, EventArgs e)
        {


           
           if (!activadoPermisoCambioEstado)
            {
                radDropDownList1.DataSource = DataAccess.getExistenciaEstado().DefaultView;
            }
            else
            {
                radDropDownList1.DataSource = DataAccess.getExistenciaEstadoPermitidos().DefaultView;
            }
           
            radDropDownList1.DisplayMember = "DESCRIPCION";
            radDropDownList1.ValueMember = "IDEXISTENCIAESTADO";
            radDropDownList1.AutoCompleteMode = AutoCompleteMode.Append;
            radDropDownList1.Refresh();
            //radDropDownList1.ReadOnly = true;
           
        }

    }
}

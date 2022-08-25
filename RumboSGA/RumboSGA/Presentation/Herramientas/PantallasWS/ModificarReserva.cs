using Rumbo.Core.Herramientas;
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class ModificarReserva : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        RadForm form=new RadForm();
        public int idReserva;
        DataTable dtReservas;
        WSReservaMotorClient wsReserva = new WSReservaMotorClient();

        public ModificarReserva(int idReserva_)
        {
            InitializeComponent();
            this.idReserva = idReserva_;
            //btnAceptar.Click += btnAceptar_Click;
            //btnCancelar.Click += btnCancelar_Click;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.Text = Lenguaje.traduce("Datos Reserva");            
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {

            try
            {
                decimal cantidad = Convert.ToDecimal(txtCantidad.Text);
                decimal cantidadP = Convert.ToDecimal(lblTxtCantidadPalet.Text);

                int idArticulo = Convert.ToInt32(dtReservas.Rows[0]["idarticulo"]);
                object[] presAlm = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidad);
                
                if (presAlm != null)
                {
                    cantidad = Convert.ToInt32(presAlm[0]);
                
                }
                object[] presAlm1 = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidadP);

                if (presAlm1 != null)
                {
                    cantidadP = Convert.ToInt32(presAlm1[0]);

                }
                wsReserva.modificarReserva(idReserva, Convert.ToInt32(cantidad), Convert.ToInt32(cantidadP), User.IdUsuario);
                this.DialogResult = DialogResult.OK;
                RadMessageBox.Show("Operación completada");
                this.Close();
                form.Close();
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            
        }
        private void btnCancelar_Click(object sender,EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            RadMessageBox.Show("Operación cancelada");
            this.Close();
            form.Close();
        }

        

        private void ModificarReserva_Load(object sender, EventArgs e)
        {
            try
            {
                dtReservas = Business.GetDatosReserva(idReserva);
                if (dtReservas.Rows.Count > 0)
                {
                    lblbTxtMatricula.Text = dtReservas.Rows[0]["identrada"].ToString();
                    lblTxtSSCC.Text = dtReservas.Rows[0]["SSCC"].ToString();
                    lblTxtCodArticulo.Text = dtReservas.Rows[0]["referencia"].ToString();
                    lblTxtDescArticulo.Text = dtReservas.Rows[0]["descripcion"].ToString();
                    /*
                    lblTxtCantidadPalet.Text = dtReservas.Rows[0]["cantidadpalet"].ToString();
                    txtCantidad.Text = dtReservas.Rows[0]["cantidad"].ToString();*/
                    int idArticulo = Convert.ToInt32(dtReservas.Rows[0]["idarticulo"]);
                    int cantidad = Convert.ToInt32(dtReservas.Rows[0]["cantidad"]);
                    int cantidadUnidad = Convert.ToInt32(dtReservas.Rows[0]["cantidadunidad"]);
                    int cantidadPalet = Convert.ToInt32(dtReservas.Rows[0]["cantidadpalet"]);
                    object[] presVis = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidad);
                    object[] presVis1 = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidadUnidad);
                    object[] presVis2 = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidadPalet);
                    if (presVis != null)
                    {
                        lblTxtCantidadPalet.Text = presVis2[0].ToString();
                        lblTipoUnidad.Text = presVis[1].ToString();
                        txtCantidad.Text = presVis[0].ToString();// dtExistencias.Rows[0]["cantidad"].ToString();
                       
                    }
                    else
                    {
                        lblTxtCantidadPalet.Text = dtReservas.Rows[0]["cantidad"].ToString();
                        txtCantidad.Text = dtReservas.Rows[0]["cantidad"].ToString();
                       
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

       
    }
}

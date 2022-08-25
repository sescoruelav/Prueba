using Rumbo.Core.Herramientas;
using RumboSGA.SalidaMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FrmSalidasRegularizacion : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataTable dtEntradas = new DataTable();

        private int idEntrada;
        private int cantidadExistencia;
        private int idPresentacion = 0;
        private int tipoPresentacion = 0;
        private int idArticulo;
        private bool hayPresentaciones = false;
        private bool hayQueLeerPeso = false;

        public FrmSalidasRegularizacion(int idEntrada_)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Salida Regularización");
            this.idEntrada = idEntrada_;
            string query = "SELECT EN.SSCC,EN.IDENTRADA,EX.IDARTICULO,EX.CANTIDADUNIDAD,EX.IDUNIDADTIPO,A.REFERENCIA,A.DESCRIPCION" +
                ",A.ATRIBUTO,A.REGISTRARPESO,EX.CANTIDAD,A.IDPRESENTACION,A.TIPOPRESENTACION,H.DESCRIPCION AS UBICACION "
                           + " FROM TBLENTRADAS EN"
                           + " INNER JOIN TBLEXISTENCIAS EX ON EX.IDENTRADA = EN.IDENTRADA"
                           + " INNER JOIN TBLHUECOS H ON EX.IDHUECO=H.IDHUECO "
                + " INNER JOIN TBLARTICULOS A ON A.IDARTICULO = EN.IDARTICULO WHERE EX.IDENTRADA=" + idEntrada;
            dtEntradas = ConexionSQL.getDataTable(query);
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            confirmarSalida();
        }

        private void confirmarSalida()
        {
            try
            {
                double cantidadBaja;

                if (rRButonQuedan.IsChecked)
                {
                    if (hayPresentaciones)
                    {
                        int cantidadIntroducida = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, Convert.ToDouble(rTxtUdsQuedan.Text));
                        cantidadBaja = cantidadExistencia - cantidadIntroducida;
                    }
                    else
                    {
                        cantidadBaja = cantidadExistencia - Convert.ToDouble(rTxtUdsQuedan.Text);
                    }
                }
                else
                {
                    if (hayPresentaciones)
                    {
                        int cantidadIntroducida = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, Convert.ToDouble(rTxtUnidadesBaja.Text));
                        cantidadBaja = cantidadIntroducida;
                    }
                    else
                    {
                        cantidadBaja = Convert.ToDouble(rTxtUnidadesBaja.Text);
                    }
                    if (cantidadBaja > cantidadExistencia)
                    {
                        RadMessageBox.Show(this, Lenguaje.traduce("La cantidad introducida es mayor que la cantidad que queda en el palet. \n"
                                            + "Vuelva a introducir la cantidad"), "Error");
                        return;
                    }
                }

                if (radComboBoxMotivo.SelectedValue == null)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Debe seleccionar un motivo"), "Error");
                    return;
                }
                string motivo = radComboBoxMotivo.SelectedValue.ToString();
                if (hayQueLeerPeso)
                {
                    double peso = double.Parse(radTextPeso.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    if (rRButonQuedan.IsChecked)
                    {
                        double pesoActual = double.Parse(lblPesoActual.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                        peso = pesoActual - peso;
                    }
                    regularizar(motivo, idEntrada, Convert.ToInt32(cantidadBaja), peso);
                }
                else
                {
                    regularizar(motivo, idEntrada, Convert.ToInt32(cantidadBaja));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        public void regularizar(string idMotivo, int idEntrada, int cantidad)
        {
            WSSalidaMotorClient salidaMotorClient = new WSSalidaMotorClient();
            try
            {
                log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'SalidaMotorClient.generarSalidaRegularizacionPaletCantidad' en Stock con parametros:IdUsuario(" + User.IdUsuario + ")" +
                    ",IdRecurso(0),IdEntrada(" + idEntrada + "),IdMotivo(" + idMotivo + "),comentario(),cantidad(" + cantidad + ")");
                salidaMotorClient.generarSalidaRegularizacionPaletCantidad(User.IdOperario, 0, idEntrada, idMotivo, radTxtComentario.Text, cantidad);
                log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada");
                this.Close();
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
            }
        }

        public void regularizar(string idMotivo, int idEntrada, int cantidad, double peso)
        {
            WSSalidaMotorClient salidaMotorClient = new WSSalidaMotorClient();
            try
            {
                log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'SalidaMotorClient.generarSalidaRegularizacionPaletCantidadPeso' en Stock con parametros:IdUsuario(" + User.IdUsuario + ")" +
                    ",IdRecurso(0),IdEntrada(" + idEntrada + "),IdMotivo(" + idMotivo + "),comentario(),cantidad(" + cantidad + ")");
                //  salidaMotorClient.generarSalidaRegularizacionPaletCantidadPeso(User.IdOperario, 0, idEntrada, idMotivo, radTxtComentario.Text, cantidad,peso);
                log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada");
                this.Close();
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
            }
        }

        private void FrmSalidasRegularizacion_Load(object sender, EventArgs e)
        {
            cargaDatosPantalla();
        }

        private void cargaDatosPantalla()
        {
            try
            {
                if (dtEntradas.Rows.Count > 0)
                {
                    DataRow row = dtEntradas.Rows[0];
                    radLabelMatricula.Text = row["IDENTRADA"].ToString() + " - " + row["SSCC"].ToString();
                    radLabelArticulo.Text = row["REFERENCIA"].ToString() + " " + row["DESCRIPCION"].ToString() + " " + row["ATRIBUTO"].ToString();
                    radLabelUbicacion.Text = row["UBICACION"].ToString();
                    cantidadExistencia = Convert.ToInt32(row["CANTIDAD"]);
                    radLabelCantidad.Text = cantidadExistencia.ToString();
                    idArticulo = Convert.ToInt32(row["IDARTICULO"]);
                    idPresentacion = Convert.ToInt32(row["IDPRESENTACION"]);
                    int tipoPres = Convert.ToInt32(row["TIPOPRESENTACION"]);

                    try
                    {
                        hayPresentaciones = dibujarPresentaciones(idArticulo, tipoPres, idPresentacion, Convert.ToInt32(row["CANTIDADUNIDAD"]));

                        if (hayPresentaciones)
                        {
                            object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, cantidadExistencia);
                            radLabelCantidad.Text = presC[0].ToString() + " " + presC[1].ToString();
                            rTxtCantUnidadQuedan.Visible = true;
                            radComboBoxUTQuedan.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        string registrarPeso = row["REGISTRARPESO"].ToString();
                        if (!string.IsNullOrEmpty(registrarPeso) && registrarPeso.Substring(0, 1).Equals("S"))
                        {
                            hayQueLeerPeso = true;
                            lblPeso.Visible = true;
                            radTextPeso.Visible = true;
                        }
                        else
                        {
                            hayQueLeerPeso = false;
                            lblPeso.Visible = false;
                            radTextPeso.Visible = false;
                        }
                        if (hayQueLeerPeso)
                        {
                            DataTable dtPeso = new DataTable();
                            String sql = "SELECT (e.peso - coalesce(sum(s.peso),0))  as pesoactual " +
                                " FROM dbo.TBLENTRADAS e left JOIN dbo.TBLSALIDAS s ON s.IDENTRADA = e.IDENTRADA " +
                                " WHERE e.IDENTRADA = " + row["IDENTRADA"] + "GROUP BY e.PESO";
                            dtPeso = ConexionSQL.getDataTable(sql);
                            if (dtPeso.Rows.Count > 0)
                            {
                                DataRow rowPeso = dtPeso.Rows[0];
                                lblPesoActual.Text = Math.Max(0, Decimal.Parse(rowPeso["PESOACTUAL"].ToString(), CultureInfo.CurrentCulture)).ToString("G29");
                            }
                        }
                        else
                        {
                            lblPesoActual.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxMotivo, DataAccess.GetIdDescripcionMotivosTipo("SR"), "IDMOTIVO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                rTxtUdsQuedan.Visible = true;
                rTxtUnidadesBaja.Visible = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private bool dibujarPresentaciones(int idArticulo, int tipoPres, int idPresentacion, int cantidadUnidad)
        {
            if (tipoPres > 0)
            {
                tipoPresentacion = tipoPres;
            }
            else
            {
                tipoPresentacion = Persistencia.getParametroInt("TIPOPRESENTACION");
            }
            if (tipoPresentacion == 0)
            {
                setPresentaciones(false);
                return false;
            }
            if (idPresentacion == 0)
            {
                setPresentaciones(false);
                return false;
            }
            object[] pres = Presentaciones.getTipoUnidadPresentacionVisualizacion(idPresentacion);
            Presentaciones.RellenarUnidadesTipoPresentaciones(idPresentacion, tipoPresentacion, ref radComboBoxUTBaja, pres[1].ToString());
            Presentaciones.RellenarUnidadesTipoPresentaciones(idPresentacion, tipoPresentacion, ref radComboBoxUTQuedan, pres[1].ToString());
            object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, cantidadUnidad);
            rTxtCantUnidadQuedan.Text = presC[0].ToString();
            rTxtCanUnidadbaja.Text = presC[0].ToString();
            return true;
        }

        private void setPresentaciones(bool v)
        {
            radComboBoxUTBaja.Visible = v;
            radComboBoxUTBaja.Visible = v;
            rTxtCantUnidadQuedan.Visible = v;
            rTxtCantUnidadQuedan.Visible = v;
        }

        private void rumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rRButonQuedan_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            rTxtUdsQuedan.Visible = true;
            rTxtUnidadesBaja.Visible = false;
            if (hayPresentaciones)
            {
                rTxtCantUnidadQuedan.Visible = true;
                radComboBoxUTQuedan.Visible = true;
                rTxtCanUnidadbaja.Visible = false;
                radComboBoxUTBaja.Visible = false;
            }
            else
            {
                rTxtCantUnidadQuedan.Visible = false;
                radComboBoxUTQuedan.Visible = false;
                rTxtCanUnidadbaja.Visible = false;
                radComboBoxUTBaja.Visible = false;
            }
            radTextPeso.Location = new Point(radTextPeso.Location.X, rRButonQuedan.Location.Y + 30);
            lblPeso.Location = new Point(lblPeso.Location.X, rRButonQuedan.Location.Y + 30);
        }

        private void rRButtonBaja_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            rTxtUnidadesBaja.Visible = true;
            rTxtUdsQuedan.Visible = false;
            if (hayPresentaciones)
            {
                rTxtCantUnidadQuedan.Visible = false;
                radComboBoxUTQuedan.Visible = false;
                rTxtCanUnidadbaja.Visible = true;
                radComboBoxUTBaja.Visible = true;
            }
            else
            {
                rTxtCantUnidadQuedan.Visible = false;
                radComboBoxUTQuedan.Visible = false;
                rTxtCanUnidadbaja.Visible = false;
                radComboBoxUTBaja.Visible = false;
            }
            radTextPeso.Location = new Point(radTextPeso.Location.X, rRButtonBaja.Location.Y + 30);
            lblPeso.Location = new Point(lblPeso.Location.X, rRButtonBaja.Location.Y + 30);
        }

        private void rTxtUdsQuedan_Leave(object sender, EventArgs e)
        {
            try
            {
                rTxtUdsQuedan.Text = rTxtUdsQuedan.Text.Replace('.', ',');
            }
            catch (Exception ex)
            {
            }
        }

        private void rTxtCantUnidadQuedan_Leave(object sender, EventArgs e)
        {
            try
            {
                rTxtCantUnidadQuedan.Text = rTxtCantUnidadQuedan.Text.Replace('.', ',');
            }
            catch (Exception ex)
            {
            }
        }

        private void rTxtUnidadesBaja_Leave(object sender, EventArgs e)
        {
            try
            {
                rTxtUnidadesBaja.Text = rTxtUnidadesBaja.Text.Replace('.', ',');
            }
            catch (Exception ex)
            {
            }
        }

        private void rTxtCanUnidadbaja_Leave(object sender, EventArgs e)
        {
            try
            {
                rTxtCanUnidadbaja.Text = rTxtCanUnidadbaja.Text.Replace('.', ',');
            }
            catch (Exception ex)
            {
            }
        }

        private void radLabel1_Click(object sender, EventArgs e)
        {
        }
    }
}
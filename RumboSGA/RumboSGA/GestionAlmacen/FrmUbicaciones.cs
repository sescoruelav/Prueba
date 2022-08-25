using RumboSGA.Herramientas;
using RumboSGA.Presentation.Herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Rumbo.Core.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGA.HuecosMotor;
using RumboSGA.Presentation;
using System.ServiceModel;
using System.Net;
using System.ServiceModel.Description;
using RumboSGA.Presentation.UserControls.Mantenimientos;

namespace RumboSGA.GestionAlmacen
{

    
    public partial class FrmUbicaciones : Telerik.WinControls.UI.RadForm
    {
        WSHuecosClient ws = null;
        private static String[] camposAlmacen = { "IDHUECOALMACEN", "DESCRIPCION" };
        private static string campoFormAlmacen = "TBLHUECOSALMACEN";
        private static string valueMemberAlmacen = "IDHUECOALMACEN";
        private static string displayMemberAlmacen = "DESCRIPCION";

        private static String[] camposEstado = { "IDHUECOESTADO", "DESCRIPCION" };
        private static string campoFormEstado = "TBLHUECOSESTADO";
        private static string valueMemberEstado = "IDHUECOESTADO";
        private static string displayMemberEstado = "DESCRIPCION";

        private static String[] camposTipoEstante = { "IDHUECOESTANTE", "DESCRIPCION" };
        private static string campoFormTipoEstante = "TBLHUECOSESTANTE";
        private static string valueMemberTipoEstante = "IDHUECOESTANTE";
        private static string displayMemberTipoEstante = "DESCRIPCION";

        private static String[] camposTipoHueco = { "IDHUECOTIPO", "DESCRIPCION" };
        private static string campoFormTipoHueco = "TBLHUECOSTIPO";
        private static string valueMemberTipoHueco = "IDHUECOTIPO";
        private static string displayMemberTipoHueco = "DESCRIPCION";

        private static String[] camposPicking = { "ID", "DESCRIPCION" };
        private static string campoFormPicking = "FNTABLASINO()";
        private static string valueMemberPicking = "ID";
        private static string displayMemberPicking = "DESCRIPCION";

        private static String[] camposBloque = { "TIPOBLOQUE", "DESCRIPCION" };
        private static string campoFormBloque = "TBLTIPOBLOQUE";
        private static string valueMemberBloque = "TIPOBLOQUE";
        private static string displayMemberBloque = "DESCRIPCION";

        


        public FrmUbicaciones()
        {

            InitializeComponent();
            ConfigurarBotones();
            rellenarDatosComboBox();
            radRibbonBarGroup4.Text = Lenguaje.traduce(radRibbonBarGroup4.Text);
            radRibbonBarGroup1.Text = Lenguaje.traduce(radRibbonBarGroup1.Text);
            radRibbonBarGroup7.Text = Lenguaje.traduce(radRibbonBarGroup7.Text);
            radGroupBloques.Text= Lenguaje.traduce(radGroupBloques.Text);
            comprobarWsHuecosConectado();
        }
        private void ConfigurarBotones()
        {
            btnFormAcera.Click += btnFormAcera_Event;
            btnFormUbicacion.Click += btnFormUbicacion_Event;
            btnVerMapa.Click += btnFormMapa_Event;
        }
        private void rellenarDatosComboBox()
        {
            DataTable tRec = DataAccess.getDataTableSQL(camposAlmacen, campoFormAlmacen, "");
            Utilidades.RellenarMultiColumnComboBox(ref this.radComboBoxAlmacen, tRec, valueMemberAlmacen, displayMemberAlmacen, ""/*idRecursoDefecto.ToString()*/, new String[] { "TODOS" });

            DataTable tTar = DataAccess.getDataTableSQL(camposEstado, campoFormEstado, "");
            Utilidades.RellenarMultiColumnComboBox(ref this.radComboBoxEstado, tTar, valueMemberEstado, displayMemberEstado, "", new String[] { "TODOS" });

            DataTable tMar = DataAccess.getDataTableSQL(camposTipoEstante, campoFormTipoEstante, "");
            Utilidades.RellenarMultiColumnComboBox(ref this.radComboBoxTipoEstante, tMar, valueMemberTipoEstante, displayMemberTipoEstante, "", new String[] { "TODOS" });

            DataTable tDec = DataAccess.getDataTableSQL(camposTipoHueco, campoFormTipoHueco, "");
            Utilidades.RellenarMultiColumnComboBox(ref this.radComboBoxTipoHueco, tDec, valueMemberTipoHueco, displayMemberTipoHueco, "", new String[] { "TODOS" });

            DataTable tDok = DataAccess.getDataTableSQL(camposPicking, campoFormPicking, "");
            Utilidades.RellenarMultiColumnComboBox(ref this.radComboBoxPicking, tDok, valueMemberPicking, displayMemberPicking, "", new String[] { "TODOS" });

            DataTable tRac = DataAccess.getDataTableSQL(camposBloque, campoFormBloque, "");
            Utilidades.RellenarMultiColumnComboBox(ref this.radComboBoxBloque, tRac, valueMemberBloque, displayMemberBloque, "", new String[] { "TODOS" });
        }
#region Eventos
        private void btnFormAcera_Event(object sender, EventArgs e)
        {
            GenerarMantenimientos generarMantenimientosAcera = new GenerarMantenimientos("Aceras", 20045, "Aceras");
            generarMantenimientosAcera.ShowDialog();
        }
        private void btnFormUbicacion_Event(object sender, EventArgs e)
        {
            VisorSQLRibbonUbicaciones generarMantenimientosUbicacion = new VisorSQLRibbonUbicaciones("Ubicacion","Ubicaciones");
            generarMantenimientosUbicacion.Show();
        }
        private void btnFormMapa_Event(object sender, EventArgs e)
        {
            FrmMapaAlmacen frmMapa = new FrmMapaAlmacen();
            frmMapa.ShowDialog();
        }
        
        
        private void btnGenerarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = Lenguaje.traduce("Las siguientes ubicaciones no se han creado por que ya existian: ");
                string[] lista ;
                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("generarUbicaciones")){
                        lista = ws.generarUbicaciones(Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerPicking(), obtenerIdAlmacen(), obtenerEstado(), obtenerTipoHueco(), obtenerTipoEstante(), Convert.ToInt32(tbCapacidad.Text), Convert.ToInt32(tbArticulosXPortal.Text), Properties.Settings.Default.usuario);
                        foreach (string str in lista)
                        {
                            if (str != null)
                            {
                                mensaje = mensaje + str + ", ";
                            }

                        }
                        if (lista[0] != null)
                        {
                            RadMessageBox.Show(mensaje, "", MessageBoxButtons.OK, RadMessageIcon.Info);
                        }
                        else
                        {
                            RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                        }
                    }
                    else {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);

                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    comprobarWsHuecosConectado();
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }

            //limpiarCampos();
        }

        private void btnImprimirEtiqueta_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ws = new WSHuecosClient()) 
                {
                    if (comprobarCampos("general"))
                    {
                        
                        ws.generarHuecosImpresion(User.NombreImpresora, Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    comprobarWsHuecosConectado();
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }

        private void btnImprimirEtiquetaDescripcion_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("generarHuecosImpresionMuelles"))
                    {
                        ws.generarHuecosImpresionMuelles(User.NombreImpresora, Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen(), obtenerTipoEstante());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    comprobarWsHuecosConectado();
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }
        private void btnGenerarClaveVoz_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("general"))
                    {
                        ws.generarClave(Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP")) {
                    comprobarWsHuecosConectado();
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }
        private void btnGenerarDigitoControlVoz_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("general"))
                    {
                        ws.generarDigControl(Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))

                {
                    comprobarWsHuecosConectado();
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }
        private void btnImprimirEtiquetaVoz_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("general"))
                    {
                        ws.generarHuecosImpresionVoz(User.NombreImpresora, Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }

        private void btnGenerarDescripcion_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("general"))
                    {
                        ws.generarDescripcion(Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                        
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }

        private void btnConsolidarPicking_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("general"))
                    {
                        ws.consolidarPicking(Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }
        private void btnAjustarBloques_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("generarBloque"))
                    {
                        ws.generarBloque(Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen(), Convert.ToInt32(tbPortalesBloque.Text), obtenerTipoBloque());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    comprobarWsHuecosConectado();
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
                
            }
        }
        private void btnEliminarBloques_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WSHuecosClient())
                {
                    if (comprobarCampos("general"))
                    {
                        ws.eliminarBloque(Convert.ToInt32(tbAceraDesde.Text), Convert.ToInt32(tbAceraHasta.Text), Convert.ToInt32(tbPortalDesde.Text), Convert.ToInt32(tbPortalHasta.Text), Convert.ToInt32(tbPisoDesde.Text), Convert.ToInt32(tbPisoHasta.Text), obtenerIdAlmacen());
                        RadMessageBox.Show(Lenguaje.traduce("Se ha generado correctamente"), "", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    else
                    {
                        RadMessageBox.Show(Lenguaje.traduce("Rellena los campos marcados"), "", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("SOAP"))
                {
                    comprobarWsHuecosConectado();
                    ExceptionManager.GestionarErrorWS(ex, ws.Endpoint);
                }
                else
                {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                }
            }
        }
        private void btnGenerarBloques_Click(object sender, EventArgs e)
        {
            if (radGroupBloques.Visible) { radGroupBloques.Visible = false; } else { radGroupBloques.Visible = true; }
        }

        private void FrmUbicaciones_Load(object sender, EventArgs e)
        {

        }

        private void tbAceraDesde_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            /*if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }*/
        }

        private void tbPortalDesde_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void tbPisoDesde_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void tbAceraHasta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void tbPortalHasta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void tbPisoHasta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void tbCapacidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void tbArticulosXPortal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private int obtenerIdAlmacen()
        {
            try
            {
                DataTable tRec = DataAccess.getDataTableSQL(camposAlmacen, campoFormAlmacen, "");
                int index = radComboBoxAlmacen.SelectedIndex;
                int idAlmacen = tRec.Rows[index].Field<int>("IDHUECOALMACEN");
                return idAlmacen;
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener id almacen", ex);
            }
        }
        private string obtenerEstado()
        {
            try
            {
                DataTable tRec = DataAccess.getDataTableSQL(camposEstado, campoFormEstado, "");
                int index = radComboBoxEstado.SelectedIndex;
                string idAlmacen = tRec.Rows[index].Field<string>("IDHUECOESTADO");
                return idAlmacen;
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener el estado", ex);
            }
            }
        private string obtenerTipoEstante()
        {
            try
            {
                DataTable tRec = DataAccess.getDataTableSQL(camposTipoEstante, campoFormTipoEstante, "");
                int index = radComboBoxTipoEstante.SelectedIndex;
                string idAlmacen = tRec.Rows[index].Field<string>("IDHUECOESTANTE");
                return idAlmacen;
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener Tipo estante", ex);
            }
        }
        private string obtenerTipoHueco()
        {
            try
            {
                DataTable tRec = DataAccess.getDataTableSQL(camposTipoHueco, campoFormTipoHueco, "");
                int index = radComboBoxTipoHueco.SelectedIndex;
                string idAlmacen = tRec.Rows[index].Field<string>("IDHUECOTIPO");
                return idAlmacen;
            }
            catch(Exception ex)
            {
                throw new Exception("No se ha podido obtener hueco tipo", ex);
            }
        }
        private string obtenerPicking()
        {
            try
            {
                DataTable tRec = DataAccess.getDataTableSQL(camposPicking, campoFormPicking, "");
                int index = radComboBoxPicking.SelectedIndex;
                string PickingSN = tRec.Rows[index].Field<string>("ID");
                return PickingSN;
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener el campo Picking", ex);
            }
        }
        private string obtenerTipoBloque()
        {
            try
            {
                DataTable tRec = DataAccess.getDataTableSQL(camposBloque, campoFormBloque, "");
                int index = radComboBoxBloque.SelectedIndex;
                string idBloque = tRec.Rows[index].Field<string>("TIPOBLOQUE");
                return idBloque;
            }
            catch(Exception ex)
            {
                throw new Exception("No se ha podido obtener Tipo Bloque",ex);
            }
            
        }

        private void limpiarCampos()
        {
            try
            {
                tbAceraDesde.Text = "";
                tbAceraHasta.Text = "";
                tbPortalDesde.Text = "";
                tbPortalHasta.Text = "";
                tbPisoDesde.Text = "";
                tbPisoHasta.Text = "";
                tbCapacidad.Text = "";
                tbArticulosXPortal.Text = "";
                tbPortalesBloque.Text = "";
                radComboBoxAlmacen.Text = "";
                radComboBoxEstado.Text = "";
                radComboBoxPicking.Text = "";
                radComboBoxTipoEstante.Text = "";
                radComboBoxTipoHueco.Text = "";
                radComboBoxBloque.Text = "";
            }
            catch (Exception ex)
            {
                throw new Exception("No se han vaciado los campos con exito",ex);
            }
        }
        private void limpiarCamposNull()
        {
            try
            {
                tbAceraDesde.NullText = "";
                tbAceraHasta.NullText = "";
                tbPortalDesde.NullText = "";
                tbPortalHasta.NullText = "";
                tbPisoDesde.NullText = "";
                tbPisoHasta.NullText = "";
                tbCapacidad.NullText = "";
                tbArticulosXPortal.NullText = "";
                tbPortalesBloque.NullText = "";
                radComboBoxAlmacen.NullText = "";
                radComboBoxEstado.NullText = "";
                radComboBoxPicking.NullText = "";
                radComboBoxTipoEstante.NullText = "";
                radComboBoxTipoHueco.NullText = "";
                radComboBoxBloque.NullText = "";
            }
            catch (Exception ex)
            {
                throw new Exception("No se han limpiado con exito",ex);
            }
        }
        private bool comprobarCampos(string tipo)
        {
            try
            {
                int contador = 0;
                limpiarCamposNull();
                switch (tipo)
                {
                    case "generarUbicaciones":
                        if (tbAceraDesde.Text == String.Empty) { tbAceraDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbAceraHasta.Text == String.Empty) { tbAceraHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalDesde.Text == String.Empty) { tbPortalDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalHasta.Text == String.Empty) { tbPortalHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPisoDesde.Text == String.Empty) { tbPisoDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPisoHasta.Text == String.Empty) { tbPisoHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxAlmacen.Text == String.Empty) { radComboBoxAlmacen.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxEstado.Text == String.Empty) { radComboBoxEstado.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxTipoEstante.Text == String.Empty) { radComboBoxTipoEstante.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxPicking.Text == String.Empty) { radComboBoxPicking.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxTipoHueco.Text == String.Empty) { radComboBoxTipoHueco.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbCapacidad.Text == String.Empty) { tbCapacidad.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbArticulosXPortal.Text == String.Empty) { tbArticulosXPortal.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (contador == 0) { return true; }
                        break;
                    case "generarHuecosImpresionMuelles":
                        if (tbAceraDesde.Text == String.Empty) { tbAceraDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbAceraHasta.Text == String.Empty) { tbAceraHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalDesde.Text == String.Empty) { tbPortalDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalHasta.Text == String.Empty) { tbPortalHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPisoDesde.Text == String.Empty) { tbPisoDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPisoHasta.Text == String.Empty) { tbPisoHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxAlmacen.Text == String.Empty) { radComboBoxAlmacen.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxTipoEstante.Text == String.Empty) { radComboBoxTipoEstante.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (contador == 0) { return true; }
                        break;
                    case "generarBloque":
                        if (tbAceraDesde.Text == String.Empty) { tbAceraDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbAceraHasta.Text == String.Empty) { tbAceraHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalDesde.Text == String.Empty) { tbPortalDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalHasta.Text == String.Empty) { tbPortalHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; ; }
                        if (tbPisoDesde.Text == String.Empty) { tbPisoDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPisoHasta.Text == String.Empty) { tbPisoHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalesBloque.Text == String.Empty) { tbPortalesBloque.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxAlmacen.Text == String.Empty) { radComboBoxAlmacen.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxBloque.Text == String.Empty) { radComboBoxBloque.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (contador == 0) { return true; }
                        break;
                    case "general":
                        if (tbAceraDesde.Text == String.Empty) { tbAceraDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbAceraHasta.Text == String.Empty) { tbAceraHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalDesde.Text == String.Empty) { tbPortalDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPortalHasta.Text == String.Empty) { tbPortalHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPisoDesde.Text == String.Empty) { tbPisoDesde.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (tbPisoHasta.Text == String.Empty) { tbPisoHasta.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (radComboBoxAlmacen.Text == String.Empty) { radComboBoxAlmacen.NullText = Lenguaje.traduce("Completa el campo"); contador++; }
                        if (contador == 0) { return true; }
                        break;

                    default:
                        break;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("No se han completado los campos correctamente",ex);
            }
        }

        private void comprobarWsHuecosConectado()
        {
            try
            {
                WSHuecosClient ws = new WSHuecosClient();
            }
            catch (Exception ex)
            {
                    ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
                    RadMessageBox.Show(Lenguaje.traduce("No tienes el web service WSHUECOS añadido "), "", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }


        #endregion Eventos

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            GenerarMantenimientos generarMantenimiento = new GenerarMantenimientos("Almacen", 20001, "Almacen");
            generarMantenimiento.ShowDialog();
        }

        private void btnFormUbicacion_Click(object sender, EventArgs e)
        {

        }
    }
}

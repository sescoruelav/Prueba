using Rumbo.Core.Herramientas.Herramientas;
using RumboSGA.InventarioMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class FrmValidarPrimeraPasada : Telerik.WinControls.UI.RadForm
    {
        private static String[] camposAlmacen = { "IDHUECOALMACEN", "DESCRIPCION" };
        private static string campoFormAlmacen = "TBLHUECOSALMACEN";
        private static string valueMemberAlmacen = "IDHUECOALMACEN";
        private static string displayMemberAlmacen = "DESCRIPCION";
        public FrmValidarPrimeraPasada()
        {
            try
            {
                InitializeComponent();
                rellenarDatosComboBox();
            }catch(Exception ex)
            {
                
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rellenarDatosComboBox()
        {
            try
            {
                DataTable tRec = DataAccess.getDataTableSQL(camposAlmacen, campoFormAlmacen, "");
                Utilidades.RellenarMultiColumnComboBox(ref this.radComboBoxAlmacen, tRec, valueMemberAlmacen, displayMemberAlmacen, "", new String[] { "TODOS" });
            }catch(Exception ex)
            {

            }
        }

        private void rBtnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                int idhuecoalmacen=-1;
                int aceradesde = -1;
                int acerahasta = -1;
                int portaldesde = -1;
                int portalhasta = -1;
                int pisodesde = -1;
                int pisohasta = -1;
                try
                {
                     idhuecoalmacen= int.Parse(radComboBoxAlmacen.SelectedValue.ToString());
                     aceradesde=int.Parse(tbAceraDesde.Text );
                     acerahasta=int.Parse(tbAceraHasta.Text);
                     portaldesde=int.Parse(tbPortalDesde.Text );
                     portalhasta=int.Parse(tbPortalHasta.Text);
                     pisodesde=int.Parse(tbPisoDesde.Text );
                     pisohasta=int.Parse(tbPisoHasta.Text );
                }catch(Exception ex)
                {
                    RadMessageBox.Show("Debe rellenar los campos con valores válidos", "Rellene campos", MessageBoxButtons.OK);
                    return;
                }
                
                DataTable dt= Business.getIncidenciasInventarioPrimeraPasada(idhuecoalmacen,  aceradesde,
             acerahasta,  portaldesde,  portalhasta,  pisodesde,
             pisohasta);
                Utilidades.TraducirDataTableColumnName(ref dt);
                rgvIncidencias.DataSource = dt;
                rgvIncidencias.BestFitColumns(BestFitColumnMode.AllCells);
                rgvIncidencias.Refresh();
                

            }
            catch(Exception ex)
            {

            }
        }

        private void rBtnValidar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            WSInventarioMotorClient imc = null;
            int idhuecoalmacen = -1;
            int aceradesde = -1;
            int acerahasta = -1;
            int portaldesde = -1;
            int portalhasta = -1;
            int pisodesde = -1;
            int pisohasta = -1;
            try
            {
                try
                {

                    imc = new WSInventarioMotorClient();

                }
                catch (Exception ex)
                {

                    ExceptionManager.GestionarError(ex, "Se ha producido un error al iniciar el servicio. Revise la configuración");
                    return;
                }

                try
                {
                    idhuecoalmacen = int.Parse(radComboBoxAlmacen.SelectedValue.ToString());
                    aceradesde = int.Parse(tbAceraDesde.Text);
                    acerahasta = int.Parse(tbAceraHasta.Text);
                    portaldesde = int.Parse(tbPortalDesde.Text);
                    portalhasta = int.Parse(tbPortalHasta.Text);
                    pisodesde = int.Parse(tbPisoDesde.Text);
                    pisohasta = int.Parse(tbPisoHasta.Text);
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("Debe rellenar los campos con valores válidos", "Rellene campos", MessageBoxButtons.OK);
                    return;
                }
                imc.inventariarAlmacen(idhuecoalmacen, aceradesde, acerahasta, portaldesde, portalhasta, pisodesde, pisohasta, DatosThread.getInstancia().getArrayDatos(), User.IdOperario, 0);
                RadMessageBox.Show("Se ha realizado validación de incidencias de inventario", "Limpieza", MessageBoxButtons.OK);

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, "Se ha producido un error limpiando incidencias");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}

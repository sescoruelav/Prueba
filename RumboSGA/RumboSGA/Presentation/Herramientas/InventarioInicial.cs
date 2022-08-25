using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.InventarioMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Herramientas
{
    public partial class InventarioInicial : Telerik.WinControls.UI.ShapedForm
    {
        public InventarioInicial()
        {
            InitializeComponent();
            this.btnSalir.Click += btnSalir_Event;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Shown += form_Shown;
            this.Show();
            this.topLabel.Text = Lenguaje.traduce(this.topLabel.Text);
            this.radLabel1.Text = Lenguaje.traduce(this.radLabel1.Text);
            this.Text = Lenguaje.traduce(this.Text);
            this.btnImprimir.Text = Lenguaje.traduce(this.btnImprimir.Text);
            this.btnSalir.Text = Lenguaje.traduce(this.btnSalir.Text);
            this.btnLimpiar.Text = Lenguaje.traduce(this.btnLimpiar.Text);
            this.btnValidar.Text = Lenguaje.traduce(this.btnValidar.Text);

        }
        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }
        public void btnSalir_Event(object sender,EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
          
            try
            {
                this.Cursor = Cursors.WaitCursor;
                WSInventarioMotorClient imc = null;
                try
                {

                    imc = new WSInventarioMotorClient();
                
                }
                catch (Exception ex)
                {

                    ExceptionManager.GestionarError(ex,"Se ha producido un error al iniciar el servicio. Revise la configuración");
                    return;
                }
            
                int.TryParse(txtNumEtiquetas.Text, out int numEtiq);
                imc.imprimirEtiquetasInventario(numEtiq, User.NombreImpresora);
                RadMessageBox.Show("Se ha lanzado a impresion " + numEtiq + " etiquetas", "Impresión", MessageBoxButtons.OK); 
               
            }            
            catch (Exception ex) {
                
                    ExceptionManager.GestionarError(ex, "Se ha producido un error al imprimir las etiquetas");


            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                WSInventarioMotorClient imc = null;
                
                    try
                    {

                        imc = new WSInventarioMotorClient();

                    }
                    catch (Exception ex)
                    {

                        ExceptionManager.GestionarError(ex, "Se ha producido un error al iniciar el servicio. Revise la configuración");
                        return;
                    }
                    imc.limpiarEtiquetasInventarioInicial();
                RadMessageBox.Show("Se ha realizado la limpieza  de incidencias de inventario", "Limpieza", MessageBoxButtons.OK);
                
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

        private void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                FrmValidarPrimeraPasada frm = new FrmValidarPrimeraPasada();
                frm.ShowDialog();
            }catch (Exception ex)
            {

            }
        }
    }
}

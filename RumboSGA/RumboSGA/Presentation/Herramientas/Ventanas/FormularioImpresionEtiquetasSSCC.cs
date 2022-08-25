using Rumbo.Core.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.LotesMotor;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.ProduccionMotor;
using RumboSGA.Properties;
using RumboSGA.RecepcionMotor;
using RumboSGA.VariablesMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FormularioImpresionEtiquetasSSCC : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int opcionImpresion = 0;
        private bool existeLote;
        private int idArticuloSeleccionado=-1;
        private DateTime? fechaCaducidadCalculada = null;
        #region Variables

        #endregion

        public FormularioImpresionEtiquetasSSCC()
        {
            
            InitializeComponent();
           
            radTextBoxControlNumEtiquetas.Text = "" + 1;            
            this.Text = Lenguaje.traduce("Impresión Etiquetas SSCC");
            this.Icon = Resources.bruju;
            this.radMCCMBPrefijo.Text = Lenguaje.traduce("PrefijoSSCC");
            rumrBtnAutogenerado.Select();
            
        }

        private void configurarComboArticulos()
        {
            Utilidades.RellenarMultiColumnComboBox(ref radComboBoxArticulo, DataAccess.GetIdReferenciaDescripcionAtributoArticulos(), "IDARTICULO", "DESCRIPCION", "", new String[] { "REFERENCIA", "DESCRIPCION", "ATRIBUTO" }, true);
        }

        #region Metodos


        #endregion

        #region MetodosPredefinidosYBotones
        private void FormularioImpresionEtiquetasSSCC_Load(object sender, EventArgs e)
        {
            configurarComboArticulos();
        }

        private void RumButtonImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                int numEtiquetas;
                int copias;
                int idArticulo;
                int cantidad;
                
                int.TryParse(radTextBoxControlNumEtiquetas.Text, out numEtiquetas);
                if (numEtiquetas > 100)
                {
                    if(RadMessageBox.Show(Lenguaje.traduce("Va a imprimir " + numEtiquetas + " etiquetas.¿Esta seguro?"), strings.Confirmar, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                int.TryParse(radTextBoxCopias.Text,out copias);
                int.TryParse(radTextBoxControlTotalUdsPicos.Text, out cantidad);
                if (radComboBoxArticulo.SelectedValue == null)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Debe seleccionar articulo"), "Error");
                }

                string prefijo = radMCCMBPrefijo.Text;
                if (opcionImpresion == 1)
                {
                    if (prefijo.Equals(Lenguaje.traduce("PrefijoSSCC")))
                    {
                        RadMessageBox.Show(this, Lenguaje.traduce("Debe seleccionar un prefijo"), "Error");
                    }
                }
                string fecha = radDateTimePickerFechaCaducidad.Value.ToString();
                idArticulo = Convert.ToInt32(radComboBoxArticulo.SelectedValue);
                WSEntradaMotorClient wsem = new WSEntradaMotorClient();
                wsem.imprimirEtiquetaSSCC(opcionImpresion, numEtiquetas, copias, idArticulo, radTextBoxControlLote.Text, fecha, cantidad, radTextBoxControlSSCCManual.Text, prefijo, User.NombreImpresora);
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a impresión "), Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadTextBoxControlLote_Leave(object sender, EventArgs e)
        {
            ComprobarExistenciaLotes();
        }
        private int ComprobarExistenciaLotes()
        {
            try
            {
                if (radComboBoxArticulo.SelectedValue == null)
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("Debe seleccionar articulo"), "Error");
                }
                
                rumLabelExisteLote.Visible = true;
                rumLabelExisteLote.Enabled = true;
                rumLabelExisteLote.Text = "Cargando";
                if (String.IsNullOrEmpty(radTextBoxControlLote.Text))
                {
                    MessageBox.Show(Lenguaje.traduce("El lote no puede estar vacio"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    existeLote = false;
                    radTextBoxControlLote.Focus();
                    return -1;
                }
                WSLotesClient wsLotes = new WSLotesClient();
                Object[] loteArticulo = wsLotes.getLoteArticulo(idArticuloSeleccionado, radTextBoxControlLote.Text);
                if (loteArticulo != null)
                {
                    rumLabelExisteLote.Text = "El lote existe";
                    object[] fechaCaducidad = (object[]) loteArticulo[4];
                    
                    radDateTimePickerFechaCaducidad.Value = (DateTime)fechaCaducidad[1]/*loteArticulo[4][1]*/;
                   existeLote = true;
                    return 1;
                }
                else
                {
                    rumLabelExisteLote.Text = "No existe el lote";
                    existeLote = false;
                    if (fechaCaducidadCalculada != null)
                    {
                        radDateTimePickerFechaCaducidad.Value = fechaCaducidadCalculada.Value;
                    }
                    else{
                        radDateTimePickerFechaCaducidad.Value = DateTime.Now;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                rumLabelExisteLote.Text = "Error consultando lote";
                ExceptionManager.GestionarError(ex, ex.Message);
                existeLote = false;
                return -1;
            }
        }
        #endregion

        private void rumrBtnAutogenerado_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            radTextBoxControlSSCCManual.Visible = false;
            radMCCMBPrefijo.Visible = false;
            opcionImpresion = 0;
        }

        private void rumrBtnEntradaManual_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            radMCCMBPrefijo.Visible = false;
            radTextBoxControlSSCCManual.Visible = true;
            opcionImpresion = 0;
        }

        private void rumrBtnSSCCGenerico_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {

            radMCCMBPrefijo.Visible = true;
            radTextBoxControlSSCCManual.Visible = false;
            opcionImpresion = 1;

        }

        private void radComboBoxArticulo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                idArticuloSeleccionado = Convert.ToInt32(radComboBoxArticulo.SelectedValue);
                cargaDatosArticulo();

            }
            catch (Exception ex)
            {

            }
        }
        private void cargaDatosArticulo()
        {
            try
            {
                if (idArticuloSeleccionado <= 0)
                {
                    verCampos(false);
                    return;
                }
                verCampos(true);
                DataRow valoresOrdenFila = DataAccess.GetAltaArticuloDefecto(idArticuloSeleccionado).Rows[0];
                if (valoresOrdenFila == null)
                {
                    MessageBox.Show(Lenguaje.traduce("No ha devuelto nada"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //Lote
                if (valoresOrdenFila["LOTEESTRICTO"].Equals("L") || valoresOrdenFila["LOTEESTRICTO"].Equals("S"))
                {
                    radTextBoxControlLote.Enabled = true;
                    radDateTimePickerFechaCaducidad.Enabled = true;
                    calcularFechaCaducidad(Convert.ToInt32(valoresOrdenFila["DIASCADUCIDAD"]));
                }
                else
                {
                    radTextBoxControlLote.Enabled = false;
                    radDateTimePickerFechaCaducidad.Enabled = false;
                }

                
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
                ExceptionManager.GestionarError(e);
            }
        }
        private void calcularFechaCaducidad(int diasCaducidad)
        {
            try
            {
                DateTime hoy = DateTime.Now;
                if (diasCaducidad > 0)
                {
                    fechaCaducidadCalculada = hoy.AddDays(diasCaducidad);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void verCampos(bool visible)
        {
          
            radTextBoxControlLote.Visible = visible;          
            rumLabelExisteLote.Visible = visible;
            rumLabelLote.Visible = visible;
            radDateTimePickerFechaCaducidad.Visible = visible;
            radTextBoxControlTotalUdsPicos.Visible = visible;            
            rumButtonAlta.Enabled = visible;
            rumLabelCantidadTotal.Visible = visible;
            rumLabelFechaCaducidad.Visible = visible;
            

        }
        private int CargarDatosCombos()
        {
            try
            {
                //Cargo combo Articulo
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxArticulo, DataAccess.GetIdReferenciaDescripcionAtributoArticulos(), "IDARTICULO", "DESCRIPCION", "", new String[] { "TODOS" },true);
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
                ExceptionManager.GestionarError(e);
            }

            return 0;
        }
    }
}

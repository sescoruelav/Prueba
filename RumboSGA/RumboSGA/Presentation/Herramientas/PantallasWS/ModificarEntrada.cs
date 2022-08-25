using Rumbo.Core.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGA.ReservaMotor;
using RumboSGA.VariablesMotor;
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
    public partial class ModificarEntrada : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        RadForm form=new RadForm();
        public int idEntrada;
        DataTable dtExistencias;
        WSEntradaMotorClient wsem = new WSEntradaMotorClient();
        private List<OAtributo> listaAtributos;

        public ModificarEntrada(int idEntrada_)
        {
            InitializeComponent();
            this.idEntrada = idEntrada_;
            //btnAceptar.Click += btnAceptar_Click;
            //btnCancelar.Click += btnCancelar_Click;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.Text = Lenguaje.traduce("Datos Existencia");            
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {

            try
            {
                Object[][] atributos = ComprobracionAtributos();
                if (atributos != null && (atributos[0][0] as String).Equals("Error"))
                {
                    MessageBox.Show(Lenguaje.traduce("Falta un atributo obligatorio"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal cantidad = Convert.ToDecimal(txtCantidad.Text);
                decimal cantidadUnidad = Convert.ToDecimal(txtCantidadUnidad.Text);
                int idArticulo = Convert.ToInt32(dtExistencias.Rows[0]["idarticulo"]);
                object[] presAlm = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidad);
                object[] presAlm1 = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidadUnidad);
                if (presAlm != null)
                {
                    cantidad =Convert.ToInt32( presAlm[0]);
                    cantidadUnidad = Convert.ToInt32(presAlm1[0]);
                }
                wsem.modificarEntrada(idEntrada, Convert.ToInt32(cantidad), Convert.ToInt32(cantidadUnidad),txtLote.Text,atributos);
                this.DialogResult = DialogResult.OK;
                this.Close();
                form.Close();
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            
        }
        private Object[][] ComprobracionAtributos()
        {
            if (listaAtributos == null || listaAtributos.Count < 1)
            {
                return null;
            }
            Object[][] objectAtributos = new Object[listaAtributos.Count][];

            for (int i = 0; i < tableLayoutPanelAtributos.RowCount; i++)
            {
                String nombre = (tableLayoutPanelAtributos.GetControlFromPosition(0, i) as RumLabel).Text.Replace(":", String.Empty);
                String valor = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox).Text;
                String nombreAtributo = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox).Tag.ToString();
                OAtributo atrib = listaAtributos.Find(atr => atr.Nombre.Equals(nombre));
                if (atrib.Obligatorio == 1 && valor.Equals(""))
                {
                    return new Object[1][] { new Object[] { "Error" } };
                }
                objectAtributos[i] = new Object[2] { nombreAtributo, valor };
            }

            return objectAtributos;
        }
        private void btnCancelar_Click(object sender,EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            form.Close();
        }

        

        private void ModificarEntrada_Load(object sender, EventArgs e)
        {
            try
            {
                dtExistencias = Business.GetDatosExistencia(idEntrada);
                if (dtExistencias.Rows.Count > 0)
                {
                    lblbTxtMatricula.Text = dtExistencias.Rows[0]["identrada"].ToString();
                    lblTxtSSCC.Text = dtExistencias.Rows[0]["SSCC"].ToString();
                    lblTxtCodArticulo.Text = dtExistencias.Rows[0]["referencia"].ToString();
                    lblTxtDescArticulo.Text = dtExistencias.Rows[0]["descripcion"].ToString();
                    int idArticulo = Convert.ToInt32(dtExistencias.Rows[0]["idarticulo"]);
                    int cantidad = Convert.ToInt32(dtExistencias.Rows[0]["cantidad"]);
                    int cantidadUnidad = Convert.ToInt32(dtExistencias.Rows[0]["cantidadunidad"]);
                    object[] presVis = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidad);
                    object[] presVis1 = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidadUnidad);
                    if (presVis != null)
                    {
                        LblTxtCantidadPalet.Text = presVis[0].ToString() +" " +presVis[1];
                        txtCantidad.Text =  presVis[0].ToString();// dtExistencias.Rows[0]["cantidad"].ToString();
                        txtCantidadUnidad.Text = presVis1[0].ToString();// dtExistencias.Rows[0]["cantidadunidad"].ToString();
                    }
                    else
                    {
                        LblTxtCantidadPalet.Text = dtExistencias.Rows[0]["cantidad"].ToString();
                        txtCantidad.Text = dtExistencias.Rows[0]["cantidad"].ToString();
                        txtCantidadUnidad.Text = dtExistencias.Rows[0]["cantidadunidad"].ToString();
                    }
                    txtLote.Text = dtExistencias.Rows[0]["lote"].ToString();
                }
                CargarDatosDefectoAtributos();
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private int CargarDatosDefectoAtributos()
        {
            try
            {
                /*WSVariablesClient wsv = new WSVariablesClient();
                object[] resultadoCompuesto = wsv.cargarDatos(new String[] { DataAccess.CARGA_INI_ATRIBUTOS_ENTRADAS });  */


                /*object resultado = UtilidadesWS.DescomponVariables(resultadoCompuesto, DataAccess.CARGA_INI_ATRIBUTOS_ENTRADAS);*/
                object resultado = UtilidadesWS.getListaAtributos();
                if (resultado == null)
                {
                    log.Info(Lenguaje.traduce("No hay atributos de entradas para cargar"));
                  
                    return 0;
                }
                this.listaAtributos = resultado as List<OAtributo>;

                tableLayoutPanelAtributos.Enabled = true;
                tableLayoutPanelAtributos.Visible = true;
                //tableLayoutPanel2.Controls.Clear();
                tableLayoutPanelAtributos.Controls.Clear();
                tableLayoutPanelAtributos.RowCount = listaAtributos.Count;
                tableLayoutPanelAtributos.ColumnCount = 2;
                for (int i = 0; i < listaAtributos.Count; i++)
                {
                    if (i == 0)
                    {
                        tableLayoutPanelAtributos.RowStyles[0].Height = 100 / listaAtributos.Count;
                    }
                    if (i > 0)
                    {
                        tableLayoutPanelAtributos.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / listaAtributos.Count));
                    }
                }
                if (listaAtributos.Count == 0)
                {
                    tableLayoutPanelAtributos.RowStyles[0].Height = 0;
                }
                


                for (int i = 0; i < listaAtributos.Count; i++)
                {

                    OAtributo atributo = listaAtributos[i];

                    RumLabel rumLabelAtributo = new RumLabel();
                    rumLabelAtributo.Name = "RadLabel" + atributo.Nombre;
                    rumLabelAtributo.Text = atributo.Nombre + ":";
                    rumLabelAtributo.Dock = DockStyle.Fill;
                    RadTextBox radTextBoxAtributo = new RadTextBox();
                    radTextBoxAtributo.Name = "RadTextBox" + atributo.Nombre;
                    radTextBoxAtributo.Tag = atributo.Atributo;
                    radTextBoxAtributo.Width = 180;
                    radTextBoxAtributo.Height = 30;
                    //radTextBoxAtributo.Dock = DockStyle.Fill;
                    radTextBoxAtributo.Text = dtExistencias.Rows[0][atributo.Atributo].ToString();

                    tableLayoutPanelAtributos.Controls.Add(rumLabelAtributo, 0, i);
                    tableLayoutPanelAtributos.Controls.Add(radTextBoxAtributo, 1, i);
                    
                    //tableLayoutPanelAtributos.RowStyles[0].Height = tableLayoutPanel2.RowStyles[1].Height + 40;






                }
                // tableLayoutPanel2.RowStyles[1].Height =30 *tableLayoutPanelAtributos.Height;
                this.Height = this.Size.Height + 100 * (listaAtributos.Count - 1);
                tableLayoutPanelAtributos.Height = 40 * tableLayoutPanelAtributos.RowCount;
               

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

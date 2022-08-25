using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Maestros;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.PedidoCliMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGA.RecepcionMotor;
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
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class FrmModificarLineasRecepcion : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();              
        private int recepcion;
        
        DataTable dtLineas;
        private int tipoPresentacion = 0;
        private int idPresentacion = 0;
        public FrmModificarLineasRecepcion(int recepcion_)
        {
            try
            {
                InitializeComponent();
                this.Text = Lenguaje.traduce("Modificar líneas recepción");
                this.recepcion = recepcion_;
                dtLineas = Business.GetRecepcionesJerarquicoLineas(recepcion.ToString());
                Utilidades.TraducirDataTableColumnName(ref dtLineas);
                
              
            } catch (Exception ex)
            {
                this.Close();
            }
        }

       
        private void LlenarGrid(int idRecepcion)
        {       
                rgvLineas.DataSource = dtLineas;
                OcultarCampos();
                rgvLineas.Refresh();
                AplicarPresentaciones();

        }

        private void OcultarCampos()
        {
            string json1 = DataAccess.LoadJson("RecepcionesJerarquicoLineas");
            JArray js = JArray.Parse(json1);
            string jsonEsquema = js.First()["Scheme"].ToString();
            List<TableScheme> esquema1 = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            for (int i = 0; i < esquema1.Count; i++)
            {
                if (esquema1[i].Etiqueta != string.Empty && esquema1[i].Etiqueta != null)
                {
                    if (!esquema1[i].EsVisible)
                    {
                        rgvLineas.Columns[esquema1[i].Etiqueta].IsVisible = false;
                       
                    }
                    if (rgvLineas.Columns[esquema1[i].Etiqueta].DataType == typeof(Decimal))
                    {
                        rgvLineas.Columns[esquema1[i].Etiqueta].FormatString = "{0:G29}";
                    } 
                }
            }
        }

        private void AplicarPresentaciones()
        {
            try
            {
                
                foreach (GridViewRowInfo row in rgvLineas.Rows)
                {

                    idPresentacion = Convert.ToInt32(dtLineas.Rows[0]["IDPRESENTACION"]);
                    int tipoPres = Persistencia.getParametroInt("TIPOPRESENTACION");
                    if (tipoPres < 0)
                    {
                        tipoPresentacion = 0;
                    }
                    else
                    {
                        tipoPresentacion = Convert.ToInt32(tipoPres);
                    }
                    int tipoPresArticulo = Convert.ToInt32(dtLineas.Rows[0]["TIPOPRESENTACION"]);
                    if (tipoPresArticulo >= 0)
                    {
                        tipoPresentacion = tipoPresArticulo;
                    }

                    if (tipoPresentacion > 0 && idPresentacion > 0)
                    {
                        int uds = Convert.ToInt32(row.Cells[Lenguaje.traduce("Cant Recepcion")].Value);
                        object[] presUds = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, uds);
                        row.Cells[Lenguaje.traduce("Cantidad Recepción")].Value = presUds[0].ToString();
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        void rgvLineas_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            GridSpinEditor editor = e.ActiveEditor as GridSpinEditor;
            if (editor != null)
            {
                RadSpinEditorElement element =(RadSpinEditorElement)editor.EditorElement;
                element.InterceptArrowKeys = false;
                element.KeyDown -= new KeyEventHandler(element_KeyDown);
                element.KeyDown += new KeyEventHandler(element_KeyDown);
            }
        }

        void element_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
            {
                rgvLineas.GridNavigator.SelectPreviousRow(1);
            }
            if (e.KeyData == Keys.Down)
            {
                rgvLineas.GridNavigator.SelectNextRow(1);
            }
        }
        private void SetPreferences()
        {
            try
            {
                rgvLineas.Dock = DockStyle.Fill;
                for (int i = 1; i < rgvLineas.Columns.Count; i++)
                {
                    rgvLineas.Columns[i].ReadOnly = true;
                }
                rgvLineas.Columns[Lenguaje.traduce("Cantidad Recepción")].ReadOnly = false;
                this.rgvLineas.MultiSelect = false;
                this.rgvLineas.MasterTemplate.EnableGrouping = true;
                this.rgvLineas.ShowGroupPanel = true;
                this.rgvLineas.MasterTemplate.AutoExpandGroups = true;
                this.rgvLineas.EnableHotTracking = true;
                this.rgvLineas.MasterTemplate.AllowAddNewRow = false;
                this.rgvLineas.MasterTemplate.AllowColumnResize = true;
                this.rgvLineas.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvLineas.AllowSearchRow = true;
                this.rgvLineas.EnablePaging = false;
                this.rgvLineas.TableElement.RowHeight = 40;
                this.rgvLineas.AllowRowResize = false;
                this.rgvLineas.AllowEditRow = true;
                this.rgvLineas.MasterView.TableSearchRow.SearchDelay = 2000;
                this.rgvLineas.CellEditorInitialized += rgvLineas_CellEditorInitialized;
                this.Dock = System.Windows.Forms.DockStyle.Fill;               
                //this.rgvLineas.EnterKeyMode = RadGridViewEnterKeyMode.EnterMovesToNextCell;
                this.rgvLineas.CellFormatting += rgvLineas_CellFormatting;
                this.rgvLineas.CellValidating += rgvLineas_CellValidating;

                rgvLineas.EnableFiltering = true;
                rgvLineas.MasterTemplate.EnableFiltering = true;
                rgvLineas.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rgvLineas_CellValidating(object sender, CellValidatingEventArgs e)
        {
            try
            {
                GridViewDataColumn column = e.Column as GridViewDataColumn;
                if (e.Row is GridViewDataRowInfo && column != null && column.Name == Lenguaje.traduce("Cantidad Recepción"))
                {
                    if (e.Value==null || string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        e.Cancel = true;
                        ((GridViewDataRowInfo)e.Row).ErrorText = "Validation error!";
                    }
                    else
                    {
                        int cantPedido = Convert.ToInt32(this.rgvLineas.Rows[e.Row.Index].Cells[Lenguaje.traduce("Cantidad Pedido")].Value);
                        int cantEntrada = Convert.ToInt32(this.rgvLineas.Rows[e.Row.Index].Cells[Lenguaje.traduce("Entrada Recepción")].Value);
                        int cantRecepcion = Convert.ToInt32(e.Value);

                        if (cantRecepcion < cantEntrada)
                        {
                            e.Cancel = true;
                            ((GridViewDataRowInfo)e.Row).ErrorText = "Validation error!";
                            throw new Exception("La cantidad no puede ser menor que la cantidad entrada");
                        }
                        
                        ((GridViewDataRowInfo)e.Row).ErrorText = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void rgvLineas_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.Name == Lenguaje.traduce("Cantidad Recepción"))
            {
                e.CellElement.ForeColor = Color.Red;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
            }
        }
        private string formarJSON()
        {
            dynamic objDinamico = new ExpandoObject();

            try
            {
               
                string jsonLineas = string.Empty;
                Object[] arrayLineas = new Object[rgvLineas.Rows.Count];
                int i = 0;
                foreach (GridViewRowInfo row in rgvLineas.Rows)
                {

                    int idPedidoPro = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Pedido")].Value);
                    int idPedidoProLin = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Pedido Pro Lin")].Value);
                    int cantidad = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, Convert.ToDouble(row.Cells[Lenguaje.traduce("Cantidad Recepción")].Value));
                    //int cantidad = Convert.ToInt32(row.Cells[Lenguaje.traduce("Cantidad Recepción")].Value);
                    dynamic linea = new ExpandoObject();
                    linea.idpedidopro = idPedidoPro;
                    linea.idpedidoprolin = idPedidoProLin;
                    linea.cantidadpterecibir = cantidad;
                    linea.albarantransportista = "";
                    arrayLineas[i] = linea;
                    i++;

                }

                jsonLineas = "[" + jsonLineas + "]";
                objDinamico.idrecepcion = recepcion;
                objDinamico.lineas = arrayLineas;
                objDinamico.error = "";
                       }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
            }



            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }
        private void rBtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string json = formarJSON();
                json = "[" + json + "]";
                log.Debug(json);
                string resp = llamarWebService(json);
                if (String.IsNullOrEmpty(resp))
                {
                    RadMessageBox.Show(this, Lenguaje.traduce("No hay respuesta del web service"), "Error");
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
                log.Debug("Respuesta WebService: " + input);
                if (d != null)
                {
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
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex,ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private string llamarWebService(string json)
        {

            WSRecepcionMotorClient webservicerecepcion = new WSRecepcionMotorClient();
            string respuesta = webservicerecepcion.modificarRecepcionCabYLin(json);
            return respuesta;

        }
        private void rBtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        




        

        private void FrmModificarLineasRecepcion_Load(object sender, EventArgs e)
        {
            try
            {


                LlenarGrid(recepcion);
                rgvLineas.BestFitColumns();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                rBtnCancelar.Click += rBtnCancelar_Click;
                rBtnGuardar.DoubleClick += rBtnGuardar_Click;
                SetPreferences();
                rgvLineas.Refresh();
                if (rgvLineas.Rows.Count > 0)
                {
                    rgvLineas.Rows[0].Cells[Lenguaje.traduce("Cantidad Recepción")].BeginEdit();



                }

            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex,ex.Message);
            }

        }
    }
}

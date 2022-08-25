using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using Rumbo.Core.Herramientas.Herramientas;
using RumboSGA.Maestros;
using RumboSGA.OlasMotor;
using RumboSGA.PedidoCliMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class FrmGenerarOrdenRecogidaManual : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
        DataTable dtLineasPedidos;
        List<int> pedidosSeleccionados;
        
        public FrmGenerarOrdenRecogidaManual(List<int> pPedidosSeleccionados)
        {
            try
            {
                InitializeComponent();
                pedidosSeleccionados = pPedidosSeleccionados;
                this.Text = Lenguaje.traduce("Crear Orden");
                AnyadirCheckSeleccion();
                ControlesLenguaje();
              



                checkBoxColumn.EditMode = EditMode.OnValueChange;

            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }

        }

        private void ControlesLenguaje()
        {
            chkRecursoManual.Text = Lenguaje.traduce("Recurso Manual");
        }

        private void AnyadirCheckSeleccion()
        {
            rgvLineas.Columns.Add(checkBoxColumn);
            checkBoxColumn.Name = "chkSeleccion";
            checkBoxColumn.EnableHeaderCheckBox = true;
            checkBoxColumn.HeaderText = "";
            checkBoxColumn.EditMode = EditMode.OnValueChange;
        }

        private void FrmGenerarOrdenRecogidaManual_Load(object sender, EventArgs e)
        {
            try
            {
                LlenarGrid();
              
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
               
                //Format_GridColumns(rgvExistencias);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }
        private void LlenarGrid()
        {
            dtLineasPedidos = Business.GetLineasPedidosCliPendientesRecogida(pedidosSeleccionados);
            Utilidades.TraducirDataTableColumnName(ref dtLineasPedidos);
            rgvLineas.DataSource = dtLineasPedidos;
            SetPreferences();
            OcultarCampos();
            
        }

        private void OcultarCampos()
        {
            foreach (GridViewColumn column in rgvLineas.Columns)
            {
                if (column.HeaderText.Contains("ID"))
                {
                    column.IsVisible = false;
                }
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
                rgvLineas.Columns[Lenguaje.traduce("Pendiente")].ReadOnly = false;
                rgvLineas.MultiSelect = true;
                this.rgvLineas.MasterTemplate.EnableGrouping = false;
                this.rgvLineas.ShowGroupPanel = false;
                this.rgvLineas.MasterTemplate.AutoExpandGroups = true;
                this.rgvLineas.EnableHotTracking = true;
                this.rgvLineas.MasterTemplate.AllowAddNewRow = false;
                this.rgvLineas.MasterTemplate.AllowColumnResize = true;
                this.rgvLineas.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvLineas.AllowSearchRow = true;
                this.rgvLineas.EnablePaging = false;
                this.rgvLineas.TableElement.RowHeight = 40;
                this.rgvLineas.AllowRowResize = false;
                this.rgvLineas.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvLineas.EnableFiltering = true;
                rgvLineas.MasterTemplate.EnableFiltering = true;
                FuncionesGenerales.setCheckBoxTrue(ref rgvLineas, checkBoxColumn.Name);
                this.rgvLineas.CellEditorInitialized += rgvLineas_CellEditorInitialized;
                this.Dock = System.Windows.Forms.DockStyle.Fill;               
                this.rgvLineas.CellFormatting += rgvLineas_CellFormatting;
                this.rgvLineas.CellValidating += rgvLineas_CellValidating;
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
                if (e.Row is GridViewDataRowInfo && column != null && column.Name == Lenguaje.traduce("Pendiente"))
                {
                    if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        e.Cancel = true;
                        ((GridViewDataRowInfo)e.Row).ErrorText = "Validation error!";
                    }
                    else
                    {
                        int cantPedido = Convert.ToInt32(this.rgvLineas.Rows[e.Row.Index].Cells[Lenguaje.traduce("Pedida")].Value);
                        int cantRecogida = Convert.ToInt32(this.rgvLineas.Rows[e.Row.Index].Cells[Lenguaje.traduce("Recogida")].Value);
                        int cantPte = Convert.ToInt32(e.Value);

                        if (cantPte> (cantPedido - cantRecogida))
                        {
                            e.Cancel = true;
                            ((GridViewDataRowInfo)e.Row).ErrorText = "Validation error!";
                            throw new Exception("La cantidad no puede ser mayor  que la cantidad pedida ");
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
        void rgvLineas_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            GridSpinEditor editor = e.ActiveEditor as GridSpinEditor;
            if (editor != null)
            {
                RadSpinEditorElement element = (RadSpinEditorElement)editor.EditorElement;
                element.InterceptArrowKeys = false;
                element.KeyDown -= new KeyEventHandler(element_KeyDown);
                element.KeyDown += new KeyEventHandler(element_KeyDown);
            }
        }

        private void rgvLineas_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.Name == Lenguaje.traduce("Pendiente"))
            {
                e.CellElement.ForeColor = Color.Red;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
            }
        }

        private void rBtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void rBtnCrearOrdenManual_Click(object sender, EventArgs e)
        {
            try
            {
                rBtnCrearOrdenManual.Image = global::RumboSGA.Properties.Resources.esperando;
                rBtnCrearOrdenManual.Enabled = false;
                rBtnCrearOrdenManual.Text = "Generando Orden Manual";

                string respuesta = await crearOrdenRecogidaManualAsync();

                List<int> ordenes = JsonConvert.DeserializeObject<List<int>>(respuesta);
                foreach (int idOrdenRecogida in ordenes)
                {
                    rBtnCrearOrdenManual.Text = "Reservando orden " + idOrdenRecogida;
                    string respuestaReserva = await reservarOrdenPreparacionAsync(idOrdenRecogida);

                }

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            finally
            {
                rBtnCrearOrdenManual.Text = "Crear y Lanzar";
                rBtnCrearOrdenManual.Image = global::RumboSGA.Properties.Resources.edit;
                rBtnCrearOrdenManual.Enabled = true;
                LlenarGrid();
            }
        }
        private async Task<string> crearOrdenRecogidaManualAsync()
        {
            String respuesta = "";
            String json;
            WSPedidoCliMotorClient pcmMotor = null;
            try
            {

                try
                {
                    pcmMotor = new WSPedidoCliMotorClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse al servicio.Revise la configuracion");
                    ExceptionManager.GestionarError(ex);
                    return "";
                }

                return await Task.Run(() =>
                {
                    string jsonLineas = string.Empty;
                    List<GridViewRowInfo> listaRecursiva = new List<GridViewRowInfo>();
                    ObtenerChildRowRecursivo(rgvLineas.ChildRows, listaRecursiva);

                    
                   List<object> arrayLineas = new List<object>();
                    int i = 0;                    
                    foreach (GridViewRowInfo row in listaRecursiva)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value) == true)
                        {
                            dynamic linea = new ExpandoObject();
                            linea.idpedidocli = Convert.ToInt32(row.Cells["" + Lenguaje.traduce("idpedidocli") + ""].Value);
                            linea.idpedidoclilin = Convert.ToInt32(row.Cells["" + Lenguaje.traduce("idpedidoclilin") + ""].Value);
                            linea.pendiente = Convert.ToInt32(row.Cells["" + Lenguaje.traduce("Pendiente") + ""].Value);                        
                            arrayLineas.Add( linea);
                            i++;                          
                        }
                    }

                    json = JsonConvert.SerializeObject(arrayLineas);
                    json = json.Replace("},{", "},\n{");
                    return pcmMotor.crearOrdenRecogidaManualAsync(json, DatosThread.getInstancia().getArrayDatos(), User.IdUsuario, User.IdOperario);



                });

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, pcmMotor.Endpoint);
                return "Error";
            }
            return "";

        }
        private void ObtenerChildRowRecursivo(GridViewChildRowCollection childRows, List<GridViewRowInfo> list)
        {
            foreach (var child in childRows)
            {
                if (child.GetType().Name == "GridViewGroupRowInfo")
                {
                    ObtenerChildRowRecursivo(child.ChildRows, list);
                }
                if (child.GetType().Name == "GridViewRowInfo" || child.GetType().Name == "GridViewHierarchyRowInfo" || child.GetType().Name == "GridViewDataRowInfo")
                {
                    list.Add(child);
                }
            }
        }
        private async Task<string> reservarOrdenPreparacionAsync(int idOrdenRecogida)
        {

            String respuesta = "";
            String json;
            WSPedidoCliMotorClient pedidoCliMotor = null;
            try
            {

                try
                {
                    pedidoCliMotor = new WSPedidoCliMotorClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectarse al servicio.Revise la configuracion");
                    ExceptionManager.GestionarError(ex);
                    return "";
                }

                return await Task.Run(() =>
                {
                    string jsonWS = string.Empty;
                    string result = "";
                    List<ReservarOrden> listaReservar = new List<ReservarOrden>();
                    ReservarOrden reserva = new ReservarOrden();
                    reserva.idordenrecogida = idOrdenRecogida;
                    reserva.recursoAutomatico = false;
                    reserva.idRecurso = 0;
                    reserva.idusuario = User.IdUsuario;
                    reserva.error = new List<String>();
                    listaReservar.Add(reserva);
                    if (listaReservar.Count > 0)
                    {
                        jsonWS = JsonConvert.SerializeObject(listaReservar);
                        return pedidoCliMotor.reservarOrdenRecogidaAsync(jsonWS);
                    }
                    return null;



                });

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, pedidoCliMotor.Endpoint);
                return "Error";
            }
            return "";

        }
    }
}

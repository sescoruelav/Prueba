using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Maestros;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
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
    public partial class FrmDevolucionesOrdenFab : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
        private WSOrdenProduccionMotorClient OrdenProduccion = new WSOrdenProduccionMotorClient();
        private string tb;
        private int idMaquina;
        private string nombreMaquina;
        private GridViewDecimalColumn necesidadOrdenesColumna = new GridViewDecimalColumn();
        private GridViewDecimalColumn devolucionColumna = new GridViewDecimalColumn();

        public FrmDevolucionesOrdenFab(int idMaquina_, string nombreMaquina_)
        {
            try
            {
                InitializeComponent();
                this.idMaquina = idMaquina_;
                this.nombreMaquina = nombreMaquina_;

                this.ThemeName = ThemeResolutionService.ApplicationThemeName;

                LlenarGrid(idMaquina);
                rbtnCancelar.Click += rBtnCancelar_Click;
                rBtnDevolver.DoubleClick += rBtnDevolver_Click;
                //gvDevoluciones.ValueChanged += gvDevoluciones_ValueChanged;
                gvDevoluciones.ViewCellFormatting += gvDevoluciones_ViewCellFormatting;
            }
            catch (Exception)
            {
                this.Close();
            }
        }

        private void FrmDevolucionesOrdenFab_Load(object sender, EventArgs e)
        {
            SetPreferences();
            gvDevoluciones.Refresh();
            ribbonTab1.Text = Lenguaje.traduce("Devolución  ") + Lenguaje.traduce("Línea Producción ") + nombreMaquina;
            this.Show();
        }

        private void gvDevoluciones_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            try
            {
                if (!(e.CellElement is GridHeaderCellElement)
                    && !(e.CellElement is GridCheckBoxCellElement)
                    && !(e.CellElement is GridFilterCellElement)
                    && e.Column.HeaderText == Lenguaje.traduce("Devolución"))
                {
                    e.CellElement.DrawFill = true;

                    e.CellElement.GradientStyle = GradientStyles.Solid;

                    e.CellElement.BackColor = Color.Yellow;
                }
                else
                {
                    e.CellElement.DrawFill = true;

                    e.CellElement.GradientStyle = GradientStyles.Solid;

                    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                }
                if (!(e.Column is GridViewRowHeaderColumn))
                {
                    if (!String.IsNullOrEmpty(e.Column.HeaderText))
                    {
                        GridViewDataColumn columna = gvDevoluciones.Columns[e.Column.HeaderText];
                        if (columna != null && columna.DataType != null)
                        {
                            if (columna.DataType == typeof(Decimal))
                            {
                                columna.FormatString = "{0:N2}";
                            }
                            if (columna.DataType == typeof(Single))
                            {
                                columna.FormatString = "{0:N2}";
                            }
                            if (columna.DataType == typeof(Double))
                            {
                                columna.FormatString = "{0:N2}";
                            }
                            if (columna.FieldName.Equals(Lenguaje.traduce("Cantidad")) && columna.DataType == typeof(Int32))
                            {
                                columna.FormatString = "{0:N2}";
                            }
                            if (columna.FieldName.Equals(Lenguaje.traduce("Uds en Curso")) && columna.DataType == typeof(Int32))
                            {
                                columna.FormatString = "{0:N2}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }

        private void LlenarGrid(int idMaquina)
        {
            try
            {
                DataTable dt = Business.getDatosDevolucionesProduccionMaquina(idMaquina);
                gvDevoluciones.DataSource = dt;
                AñadirColumnasExtra();
                AplicarPresentaciones();
                gvDevoluciones.BestFitColumns();
                gvDevoluciones.Columns[Lenguaje.traduce("ID Presentacion")].IsVisible = false;
                gvDevoluciones.Columns[Lenguaje.traduce("ID Articulo")].IsVisible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void AñadirColumnasExtra()
        {
            try
            {
                checkBoxColumn.Name = "checkColumn";
                checkBoxColumn.EnableHeaderCheckBox = true;
                checkBoxColumn.HeaderText = "";
                checkBoxColumn.EditMode = EditMode.OnValueChange;
                gvDevoluciones.Columns.Add(checkBoxColumn);
                gvDevoluciones.Columns.Move(checkBoxColumn.Index, 0);
                gvDevoluciones.Columns.Add(Lenguaje.traduce("Tipo Unidad"));
                devolucionColumna.FieldName = Lenguaje.traduce("Devolución");
                devolucionColumna.Name = Lenguaje.traduce("Devolución");
                devolucionColumna.HeaderText = Lenguaje.traduce("Devolución");
                devolucionColumna.FormatString = "{0:N2}";
                gvDevoluciones.Columns.Add(devolucionColumna);
                gvDevoluciones.Columns[Lenguaje.traduce("Devolución")].ReadOnly = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void AplicarPresentaciones()
        {
            try
            {
                foreach (GridViewRowInfo row in gvDevoluciones.Rows)
                {
                    try
                    {
                        int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                        int cantidad = Convert.ToInt32(row.Cells[Lenguaje.traduce("Cantidad")].Value);
                        object[] presPrev = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, cantidad);
                        row.Cells[Lenguaje.traduce("Cantidad")].Value = Decimal.Parse(presPrev[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                        row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presPrev[1].ToString();
                        int cantidadNecesaria = Convert.ToInt32(row.Cells[Lenguaje.traduce("Necesidad")].Value);
                        object[] presOt = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, cantidadNecesaria);
                        row.Cells[Lenguaje.traduce("Necesidad")].Value = Decimal.Parse(presOt[0].ToString());
                        row.Cells[Lenguaje.traduce("Necesidad")].Value = Decimal.Parse(presOt[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                        int enCurso = Convert.ToInt32(row.Cells[Lenguaje.traduce("Uds en curso")].Value);
                        object[] presCur = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, enCurso);
                        row.Cells[Lenguaje.traduce("Uds en curso")].Value = Decimal.Parse(presCur[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                        row.Cells[Lenguaje.traduce("Devolución")].Value = Decimal.Parse(presPrev[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                    }
                    catch (Exception ex)
                    {

                        ExceptionManager.GestionarErrorNuevo(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void SetPreferences()
        {
            try
            {
                gvDevoluciones.Dock = DockStyle.Fill;
                for (int i = 1; i < gvDevoluciones.Columns.Count; i++)
                {
                    gvDevoluciones.Columns[i].ReadOnly = true;
                }

                gvDevoluciones.Columns[Lenguaje.traduce("Devolución")].ReadOnly = false;

                gvDevoluciones.MultiSelect = true;
                this.gvDevoluciones.MasterTemplate.EnableGrouping = true;
                this.gvDevoluciones.ShowGroupPanel = true;
                this.gvDevoluciones.MasterTemplate.AutoExpandGroups = true;
                this.gvDevoluciones.EnableHotTracking = true;
                this.gvDevoluciones.MasterTemplate.AllowAddNewRow = false;
                this.gvDevoluciones.MasterTemplate.AllowColumnResize = true;
                this.gvDevoluciones.MasterTemplate.AllowMultiColumnSorting = true;
                this.gvDevoluciones.AllowSearchRow = true;
                this.gvDevoluciones.EnablePaging = false;
                this.gvDevoluciones.TableElement.RowHeight = 40;
                this.gvDevoluciones.AllowRowResize = false;
                this.gvDevoluciones.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                gvDevoluciones.EnableFiltering = true;
                gvDevoluciones.MasterTemplate.EnableFiltering = true;
                gvDevoluciones.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        //void gvDevoluciones_ValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (this.gvDevoluciones.ActiveEditor is RadCheckBoxEditor)
        //        {
        //            Debug.WriteLine(this.gvDevoluciones.CurrentCell.RowIndex);
        //            Debug.WriteLine(this.gvDevoluciones.ActiveEditor.Value);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.GestionarError(ex);
        //        log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
        //    }
        //}

        private void rBtnDevolver_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                log.Info("Pulsado botón Devolver Material web service " + DateTime.Now);
                string jsonWS = string.Empty;
                int idEntrada = 0;

                int uds;
                foreach (GridViewRowInfo row in gvDevoluciones.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        double cantidad = Convert.ToDouble(row.Cells[Lenguaje.traduce("Devolución")].Value);
                        int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                        uds = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidad);
                        idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Entrada")].Value);
                        jsonWS += formarJSON(idEntrada, uds) + ",";
                    }
                }
                if (jsonWS == string.Empty)
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener al menos una fila seleccionada."));
                }
                else
                {
                    jsonWS = jsonWS.Substring(0, jsonWS.Length - 1);
                    jsonWS = "[" + jsonWS + "]";
                    string respuest = OrdenProduccion.devolverArticulosOrden(jsonWS);
                    TratarRespuestaWS(respuest);
                    gvDevoluciones.DataSource = null;
                    gvDevoluciones.Columns.Clear();
                    LlenarGrid(idMaquina);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, OrdenProduccion.Endpoint);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
            this.Cursor = Cursors.Arrow;
        }

        private void TratarRespuestaWS(string respuest)
        {
            var jss = new JavaScriptSerializer();
            bool lineasCorrectas = true;
            string errorLineas = string.Empty;
            string input = respuest;
            dynamic d = jss.DeserializeObject(respuest);
            int i = 0;

            /*TODO: Se ha realizado este fix rápido(comprobar que d no sea nulo) para que la aplicación
            no dé una excepción no controlada al aceptar una recepción.
            Parece ser que la recepción funciona correctamente y se guardan las cosas en la BBDD,
            pero podría haber algún comportamiento inesperado.
            El problema parece provenir del WS o de la configuración del firewall.
            03/08: Se ha hablado con Javi y el problema parece solucionado, parece ser que era una incompatibilidad del JDK. Si vuelve a
            dar problemas, reportarlo.
             */
            if (d != null)
            {
                foreach (var item in d)
                {
                    var a = item["Error"];
                    if (a != string.Empty)
                    {
                        errorLineas = a;
                        lineasCorrectas = false;
                        log.Error("Error devolviendo material: " + respuest);
                        break;
                    }
                }

                if (lineasCorrectas)
                {
                    MessageBox.Show(strings.AccionCompletada, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.DialogResult = DialogResult.Abort;
                    if (errorLineas != string.Empty)
                    {
                        MessageBox.Show(Lenguaje.traduce(errorLineas));
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce("Se ha producido un error generando la devolución "));
                    }
                }
            }
        }

        private string formarJSON(int idEntrada, int uds)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idEntrada = idEntrada;
            objDinamico.uds = uds;
            objDinamico.Error = "";
            objDinamico.idOperario = User.IdOperario;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        private void rBtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void rBtnImprimirEtiqueta_Click(object sender, EventArgs e)
        {
            imprimirEtiquetas();
        }

        private void imprimirEtiquetas()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<int> idEntradas = new List<int>();
                log.Info("Pulsado botón imprimir etiqueta web service " + DateTime.Now);
                foreach (GridViewRowInfo row in gvDevoluciones.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        int idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Entrada")].Value);
                        idEntradas.Add(idEntrada);
                    }
                }

                OrdenProduccion.imprimirEtiqueta(idEntradas.ToArray(), User.NombreImpresora);
                MessageBox.Show(Lenguaje.traduce("Se han lanzado las etiquetas a la impresora "), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            this.Cursor = Cursors.Arrow;
        }

        //private void Format_GridColumns(RadGridView dgv_format){
        //    foreach (GridViewDataColumn dCol in dgv_format.Columns)

        //    {
        //        if (dCol.DataType ==typeof(Decimal))
        //        {
        //            /*if(dCol.FormatString.ToLower() =="{0}")
        //            {*/
        //                dCol.FormatString ="{0:N2}";  //                        "{0:N2}"
        //            /*}*/
        //        }
        //        if (dCol.DataType == typeof(Single))
        //        {
        //            /*if (dCol.FormatString.ToLower() == "{0}")
        //            {*/
        //                dCol.FormatString = "{0:N2}";  //                        "{0:N2}"
        //            /*}*/
        //        }

        //    }
        //}

        /*private void radGridView_1_CellFormatting(object sender,CellFormattingEventArgs e)
        {
            if(e.CellElement.ColumnIndex > 2)
            {
                e.CellElement.Text =String.Format("{0:N2}", ((GridDataCellElement)e.CellElement).Value);
            }
        }*/
    }
}
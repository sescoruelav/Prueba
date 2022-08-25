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
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class ConsumosOrdenFabMatricula : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
        private WSOrdenProduccionMotorClient OrdenProduccion = new WSOrdenProduccionMotorClient();
        private string tb;
        private int idPedidoFab;
        private static int MODO_EXISTENCIAS_DENTRO = 0;
        private static int MODO_EXISTENCIAS_FUERA = 1;
        private static int MODO_EXISTENCIAS_OTROS_ARTICULOS = 2;
        private int modo = MODO_EXISTENCIAS_DENTRO;

        public ConsumosOrdenFabMatricula(int idPedidoFab_)
        {
            try
            {
                InitializeComponent();
                this.idPedidoFab = idPedidoFab_;

                this.Show();
                consumosGridView.BestFitColumns();
                consumosGridView.ViewCellFormatting += consumosGridView_ViewCellFormatting;
                consumosGridView.CellValueNeeded += consumosGridView_CellValueNeeded;
                this.consumeButton.Text = Lenguaje.traduce("Consumir");
                this.cancelarButton.Text = Lenguaje.traduce("Salir");
                this.Text = Lenguaje.traduce("Consumos Matrícula");
                this.ribbonTab1.Text = Lenguaje.traduce("Opciones");

                checkBoxColumn.Name = "checkColumn";
                checkBoxColumn.EnableHeaderCheckBox = true;
                checkBoxColumn.HeaderText = "";
                checkBoxColumn.EditMode = EditMode.OnValueChange;
                consumosGridView.Columns.Add(checkBoxColumn);
                LlenarGrid();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                cancelarButton.Click += CancelarBtton_Click;
                consumeButton.DoubleClick += ConsumeButton_Click;
                consumosGridView.ValueChanged += ConsumosGrid_ValueChanged;
                SetPreferences();
                //Format_GridColumns(consumosGridView);
            }
            catch (Exception)
            {
                this.Close();
            }
        }

        private void consumosGridView_CellValueNeeded(object sender, GridViewCellValueEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Column.GetType() == typeof(Decimal))
                {
                    string valor = e.Value.ToString();
                    Decimal decimales = new Decimal();
                    decimales = decimal.Parse(valor);
                    e.Value = Decimal.Round(decimales, 5);
                }
            }
        }

        private void consumosGridView_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            try
            {
                if (!(e.CellElement is GridHeaderCellElement)
                    && !(e.CellElement is GridCheckBoxCellElement)
                    && !(e.CellElement is GridFilterCellElement)
                    && e.Column != null
                    && e.Column.HeaderText == Lenguaje.traduce("Cantidad"))
                {
                    e.CellElement.DrawFill = true;

                    e.CellElement.GradientStyle = GradientStyles.Solid;

                    e.CellElement.BackColor = Color.Yellow;
                    e.CellElement.Font = new Font(e.CellElement.Font, FontStyle.Bold);
                }
                else
                {
                    e.CellElement.DrawFill = true;

                    e.CellElement.GradientStyle = GradientStyles.Solid;

                    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }

        private void LlenarGrid()
        {
            switch (modo)
            {
                case 0:
                    tb = "SELECT DISTINCT EX.IDENTRADA as \"Matrícula\"" +
                         ",A.REFERENCIA as \"Código Artículo\"" +
                        ", A.DESCRIPCION as \"Descripción\"" +
                        ", A.ATRIBUTO as Atributo " +
                        ",CAST ( EX.CANTIDAD as decimal) as Cantidad" +
                        ",EX.IDUNIDADTIPO as \"Tipo Unidad\"" +
                        ",CAST ( EX.CANTIDADUNIDAD as decimal) as \"Cantidad Unidad\"" +
                        ",EX.IDEXISTENCIAESTADO as \"Estado Existencia\"" +
                        ",EX.PRIORIDAD as Prioridad " +
                        ",EX.IDEXISTENCIATIPO as \"Tipo Existencia\"" +
                        ",H.DESCRIPCION AS \"Ubicación\"" +
                        ",E.LOTE as Lote" +
                        ",L.FECHACADUCIDAD as \"Fecha Caducidad\"" +
                        ", EX.ATRIB1 as Atrib1" +
                        ", EX.ATRIB2 as Atrib2 " +
                        ", EX.ATRIB3 as Atrib3 " +
                        ", EX.ATRIB4 as Atrib4 " +
                        ", EX.ATRIB5 as Atrib5 " +
                        ", A.IDPRESENTACION as \"ID Presentacion\" " +
                        ",EX.IDARTICULO as \"ID Articulo\"" +
                        ",EX.IDHUECO  as \"ID Ubicacion\"" +
                    "FROM dbo.TBLMAQUINAS AS M  " +
                    "INNER JOIN dbo.TBLORDENFABRICACIONCAB AS OC ON M.IDMAQUINA = OC.IDMAQUINA " +
                    "INNER JOIN dbo.VHUECOSZONA AS HZ ON M.IDZONALOGICA = HZ.IDZONACAB " +
                    "INNER JOIN dbo.TBLEXISTENCIAS AS EX ON HZ.IDHUECO = EX.IDHUECO  " +
                    "INNER JOIN dbo.TBLHUECOS AS H ON H.IDHUECO = EX.IDHUECO  " +
                    "INNER JOIN dbo.TBLORDENFABRICACIONLIN AS OL ON EX.IDARTICULO = OL.IDARTICULO  " +
                    "INNER JOIN dbo.TBLARTICULOS AS A ON OL.IDARTICULO = A.IDARTICULO " +
                    "INNER JOIN dbo.TBLENTRADAS E ON E.IDENTRADA=EX.IDENTRADA  " +
                    "LEFT  JOIN dbo.TBLLOTES L ON E.IDARTICULO = L.IDARTICULO AND E.LOTE=L.LOTE " +
                    " WHERE oc.IDPEDIDOFAB=" + idPedidoFab;
                    break;

                case 1:
                    tb = "SELECT DISTINCT EX.IDENTRADA as \"Matrícula\"" +
                         ",A.REFERENCIA as \"Código Artículo\"" +
                         ", A.DESCRIPCION as \"Descripción\"" +
                         ", A.ATRIBUTO as Atributo " +
                         ",CAST ( EX.CANTIDAD as decimal) as Cantidad" +
                         ",EX.IDUNIDADTIPO as \"Tipo Unidad\"" +
                         ",CAST ( EX.CANTIDADUNIDAD as decimal) as \"Cantidad Unidad\"" +
                         ",EX.IDEXISTENCIAESTADO as \"Estado Existencia\"" +
                         ",EX.PRIORIDAD as Prioridad " +
                         ",EX.IDEXISTENCIATIPO as \"Tipo Existencia\"" +
                         ",H.DESCRIPCION AS \"Ubicación\"" +

                         ",E.LOTE as Lote" +
                         ",L.FECHACADUCIDAD as \"Fecha Caducidad\"" +
                         ", EX.ATRIB1 as Atrib1" +
                         ", EX.ATRIB2 as Atrib2 " +
                         ", EX.ATRIB3 as Atrib3 " +
                         ", EX.ATRIB4 as Atrib4 " +
                         ", EX.ATRIB5 as Atrib5 " +
                         ", A.IDPRESENTACION as \"ID Presentacion\" " +
                         ",EX.IDARTICULO as \"ID Articulo\"" +
                         ",EX.IDHUECO  as \"ID Ubicacion\"" +
                       " FROM dbo.TBLEXISTENCIAS AS EX "
                     + " INNER JOIN dbo.VHUECOSZONA AS HZ ON HZ.IDHUECO = EX.IDHUECO "
                     + " INNER JOIN dbo.TBLHUECOS AS H ON H.IDHUECO = EX.IDHUECO "
                     + " INNER JOIN dbo.TBLORDENFABRICACIONLIN AS OL ON EX.IDARTICULO = OL.IDARTICULO "
                     + " INNER JOIN dbo.TBLARTICULOS AS A ON OL.IDARTICULO = A.IDARTICULO "
                     + " INNER JOIN dbo.TBLENTRADAS E ON E.IDENTRADA=EX.IDENTRADA "
                     + " LEFT  JOIN dbo.TBLLOTES L ON E.IDARTICULO = L.IDARTICULO AND E.LOTE=L.LOTE "
                     + " INNER JOIN dbo.TBLORDENFABRICACIONCAB AS OC ON OL.IDPEDIDOFAB=OC.IDPEDIDOFAB "
                     + " WHERE ol.IDPEDIDOFAB=" + idPedidoFab
                     + " AND HZ.IDZONACAB NOT IN (SELECT M.IDZONALOGICA FROM TBLMAQUINAS M JOIN TBLORDENFABRICACIONCAB OC2 ON OC2.IDMAQUINA = M.IDMAQUINA WHERE M.IDMAQUINA=OC.IDMAQUINA AND OC2.IDPEDIDOFAB = " + idPedidoFab + ")";
                    break;

                case 2:
                    tb = "SELECT DISTINCT EX.IDENTRADA as \"Matrícula\"" +
                         ",A.REFERENCIA as \"Código Artículo\"" +
                        ", A.DESCRIPCION as \"Descripción\"" +
                        ", A.ATRIBUTO as Atributo " +
                        ",CAST ( EX.CANTIDAD as decimal) as Cantidad" +
                        ",EX.IDUNIDADTIPO as \"Tipo Unidad\"" +
                        ",CAST ( EX.CANTIDADUNIDAD as decimal) as \"Cantidad Unidad\"" +
                        ",EX.IDEXISTENCIAESTADO as \"Estado Existencia\"" +
                        ",EX.PRIORIDAD as Prioridad " +
                        ",EX.IDEXISTENCIATIPO as \"Tipo Existencia\"" +
                        ",H.DESCRIPCION AS \"Ubicación\"" +

                        ",E.LOTE as Lote" +
                        ",L.FECHACADUCIDAD as \"Fecha Caducidad\"" +
                        ", EX.ATRIB1 as Atrib1" +
                        ", EX.ATRIB2 as Atrib2 " +
                        ", EX.ATRIB3 as Atrib3 " +
                        ", EX.ATRIB4 as Atrib4 " +
                        ", EX.ATRIB5 as Atrib5 " +
                        ", A.IDPRESENTACION as \"ID Presentacion\" " +
                        ",EX.IDARTICULO as \"ID Articulo\"" +
                        ",EX.IDHUECO  as \"ID Ubicacion\""
                    + " FROM dbo.TBLEXISTENCIAS AS EX  "
                    + " INNER JOIN dbo.TBLARTICULOS a ON a.IDARTICULO = Ex.IDARTICULO "
                    + " INNER JOIN dbo.VHUECOSZONA AS HZ ON HZ.IDHUECO = EX.IDHUECO  "
                    + " INNER JOIN dbo.TBLHUECOS AS H ON H.IDHUECO = EX.IDHUECO  "
                    + " INNER JOIN dbo.TBLENTRADAS E ON E.IDENTRADA=EX.IDENTRADA  "
                    + " LEFT  JOIN dbo.TBLLOTES L ON E.IDARTICULO = L.IDARTICULO AND E.LOTE=L.LOTE "
                    + " WHERE HZ.IDZONACAB IN (SELECT M.IDZONALOGICA FROM dbo.TBLMAQUINAS M JOIN dbo.TBLORDENFABRICACIONCAB OC2 ON OC2.IDMAQUINA = M.IDMAQUINA WHERE M.IDMAQUINA=OC2.IDMAQUINA AND OC2.IDPEDIDOFAB=" + idPedidoFab
                    + " AND ex.IDARTICULO NOT IN (SELECT OL.IDARTICULO FROM dbo.TBLORDENFABRICACIONLIN OL where OL.IDPEDIDOFAB=" + idPedidoFab + "))";
                    break;

                default:
                    break;
            }

            DataTable dt = ConexionSQL.getDataTable(tb);
            if (dt != null)
            {
                Utilidades.TraducirDataTableColumnName(ref dt);

                consumosGridView.DataSource = dt;
                consumosGridView.BestFitColumns();
                AplicarPresentaciones();
            }
        }

        private void AplicarPresentaciones()
        {
            try
            {
                foreach (GridViewRowInfo row in consumosGridView.Rows)
                {
                    int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                    int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                    int cantidad = Convert.ToInt32(row.Cells[Lenguaje.traduce("Cantidad")].Value);
                    int cantidadunidad = Convert.ToInt32(row.Cells[Lenguaje.traduce("Cantidad Unidad")].Value);
                    object[] presCantidad = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidad);
                    row.Cells[Lenguaje.traduce("Cantidad")].Value = Decimal.Parse(presCantidad[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                    row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presCantidad[1].ToString();
                    object[] presCantUni = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, cantidadunidad);
                    row.Cells[Lenguaje.traduce("Cantidad Unidad")].Value = Decimal.Parse(presCantUni[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void SetPreferences()
        {
            try
            {
                consumosGridView.Dock = DockStyle.Fill;
                for (int i = 1; i < consumosGridView.Columns.Count; i++)
                {
                    consumosGridView.Columns[i].ReadOnly = true;
                    if (consumosGridView.Columns[i].Name.Contains("ID"))
                    {
                        consumosGridView.Columns[i].IsVisible = false;
                    }
                }

                consumosGridView.Columns[Lenguaje.traduce("Cantidad")].ReadOnly = false;

                consumosGridView.MultiSelect = true;
                this.consumosGridView.MasterTemplate.EnableGrouping = true;
                this.consumosGridView.ShowGroupPanel = true;
                this.consumosGridView.MasterTemplate.AutoExpandGroups = true;
                this.consumosGridView.EnableHotTracking = true;
                this.consumosGridView.MasterTemplate.AllowAddNewRow = false;
                this.consumosGridView.MasterTemplate.AllowColumnResize = true;
                this.consumosGridView.MasterTemplate.AllowMultiColumnSorting = true;
                this.consumosGridView.AllowSearchRow = true;
                this.consumosGridView.EnablePaging = false;
                this.consumosGridView.TableElement.RowHeight = 40;
                this.consumosGridView.AllowRowResize = false;
                //this.consumosGridView.Columns[Lenguaje.traduce("Cantidad")].FormatString = "{0:N2}";
                //this.consumosGridView.Columns[Lenguaje.traduce("Cantidad Unidad")].FormatString = "{0:N2}";
                this.consumosGridView.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                consumosGridView.EnableFiltering = true;
                consumosGridView.MasterTemplate.EnableFiltering = true;
                consumosGridView.BestFitColumns(BestFitColumnMode.AllCells);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ConsumosGrid_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.consumosGridView.ActiveEditor is RadCheckBoxEditor)
                {
                    Debug.WriteLine(this.consumosGridView.CurrentCell.RowIndex);
                    Debug.WriteLine(this.consumosGridView.ActiveEditor.Value);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ConsumeButton_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón consumir matricula web service " + DateTime.Now);
                string jsonWS = string.Empty;

                int uds;
                int idEntrada;
                foreach (GridViewRowInfo row in consumosGridView.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[EncontrarCheckBox(consumosGridView.Columns)].Value) == true)
                    {
                        decimal cantidad = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Cantidad")].Value);
                        int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                        int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                        object[] presAlm = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidad);
                        uds = Convert.ToInt32(presAlm[0]);
                        idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("Matrícula")].Value);
                        jsonWS += formarJSONConsumirArticulo(idPedidoFab, uds, idEntrada) + ",";
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
                    var respuest = OrdenProduccion.consumirArticulosSinTraza(jsonWS);
                    consumosGridView.DataSource = null;
                    LlenarGrid();

                    MessageBox.Show(strings.AccionCompletada, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, OrdenProduccion.Endpoint);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private string formarJSONConsumirArticulo(int idPedidoFab, int uds, int idEntrada)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idPedidoFab = idPedidoFab;
            objDinamico.uds = uds;
            objDinamico.idEntrada = idEntrada;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        private void CancelarBtton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Format_GridColumns(RadGridView dgv_format)
        {
            foreach (GridViewDataColumn dCol in dgv_format.Columns)

            {
                if (dCol.DataType == typeof(Decimal))
                {
                    dCol.FormatString = "{0:N2}";
                }
                if (dCol.DataType == typeof(Single))
                {
                    dCol.FormatString = "{0:N2}";
                }
            }
        }

        private void radGridView_1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnIndex > 2)
            {
                e.CellElement.Text = String.Format("{0:N2}", ((GridDataCellElement)e.CellElement).Value);
            }
        }

        private void rBtnItmExistenciasFueraZona_Click(object sender, EventArgs e)
        {
            modo = MODO_EXISTENCIAS_FUERA;
            LlenarGrid();
        }

        private void rBtnItmExistenciasZona_Click(object sender, EventArgs e)
        {
            modo = MODO_EXISTENCIAS_DENTRO;
            LlenarGrid();
        }

        private void rBtnItmOtrosArticulos_Click(object sender, EventArgs e)
        {
            modo = MODO_EXISTENCIAS_OTROS_ARTICULOS;
            LlenarGrid();
        }

        private int EncontrarCheckBox(GridViewColumnCollection columns)
        {
            foreach (var col in columns)
            {
                if (col.GetType().Name == "GridViewCheckBoxColumn")
                    return col.Index;
            }
            return 0;
        }
    }
}
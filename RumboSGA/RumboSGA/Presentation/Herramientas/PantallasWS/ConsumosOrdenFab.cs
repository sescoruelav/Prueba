using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using Rumbo.Core.Herramientas.Herramientas;
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
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class ConsumosOrdenFab : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
        private WSOrdenProduccionMotorClient OrdenProduccion = new WSOrdenProduccionMotorClient();
        private string tb;
        private string ordenes;
        private int idPedidoFabLin = -1;
        private GridViewTemplate jerarquia = new GridViewTemplate();
        private DataTable tablaGridTemplate;

        public ConsumosOrdenFab(string ordenes_)
        {
            try
            {
                InitializeComponent();
                this.ordenes = ordenes_;
                rgvConsumos.BestFitColumns();
                rgvConsumos.ViewCellFormatting += consumosGridView_ViewCellFormatting;

                this.consumeButton.Text = Lenguaje.traduce("Consumir");
                this.cancelarButton.Text = Lenguaje.traduce("Salir");
                this.Text = Lenguaje.traduce("Consumos");
                this.ribbonTab1.Text = Lenguaje.traduce("Opciones");

                checkBoxColumn.Name = "checkColumn";
                checkBoxColumn.EnableHeaderCheckBox = true;
                checkBoxColumn.HeaderText = "";
                checkBoxColumn.EditMode = EditMode.OnValueChange;
                rgvConsumos.Columns.Add(checkBoxColumn);
                jerarquia.AllowAddNewRow = false;
                jerarquia.AllowDragToGroup = false;
                jerarquia.AllowEditRow = false;
                jerarquia.AllowColumnChooser = true;
                jerarquia.AllowColumnReorder = true;
                jerarquia.EnableFiltering = true;
                LlenarGrid(ordenes);
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;

                cancelarButton.Click += CancelarBtton_Click;
                consumeButton.DoubleClick += ConsumeButton_Click;
                rgvConsumos.ValueChanged += ConsumosGrid_ValueChanged;
                SetPreferences();
                //Format_GridColumns(rgvConsumos);
                rgvConsumos.MasterTemplate.Templates.Add(jerarquia);
                jerarquia.BestFitColumns(BestFitColumnMode.AllCells);
                ElegirEstilo();
                addEliminarLayoutOpciones();
            }
            catch (Exception)
            {
                this.Close();
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
                    && e.Column.HeaderText == Lenguaje.traduce("Descontar"))
                {
                    e.CellElement.DrawFill = true;

                    e.CellElement.GradientStyle = GradientStyles.Solid;

                    e.CellElement.BackColor = Color.Yellow;
                    e.CellElement.Font = new Font(e.CellElement.Font, FontStyle.Bold);
                    ((GridViewDecimalColumn)e.Column).DecimalPlaces = 4;
                }
                else
                {
                    e.CellElement.DrawFill = true;

                    e.CellElement.GradientStyle = GradientStyles.Solid;

                    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                }
                if (!(e.CellElement is GridHeaderCellElement)
                    && !(e.CellElement is GridCheckBoxCellElement)
                    && !(e.CellElement is GridFilterCellElement)
                    && e.Column != null
                    && e.Column.HeaderText == Lenguaje.traduce("Matrícula Traza"))
                {
                    e.CellElement.Font = new Font(e.CellElement.Font, FontStyle.Bold);
                }
                else
                {
                }
                if (!(e.CellElement is GridHeaderCellElement)
                    && !(e.CellElement is GridCheckBoxCellElement)
                    && !(e.CellElement is GridFilterCellElement)
                    && e.Column != null
                    && e.Column.HeaderText == Lenguaje.traduce("Matrícula Descontar"))
                {
                    e.CellElement.Font = new Font(e.CellElement.Font, FontStyle.Bold);
                }
                else
                {
                }
                if (e.CellElement.Value != null)
                {
                    if (e.CellElement.Value.GetType() == typeof(Decimal))
                    {

                        decimal numero = (decimal)e.CellElement.Value;
                        e.CellElement.Value = Decimal.Round(numero, 4);
                    }
                }
                
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }

        private void LlenarGrid(string ordenes)
        {
            /* tb = "SELECT OFL.IDPEDIDOFAB as 'Num Orden', OFB.ORDEN as Orden, OFL.IDPEDIDOFABLIN as 'Num OrdenLin', A.IDARTICULO as 'Num Articulo', A.REFERENCIA as Referencia," +
                " A.DESCRIPCION as Articulo, A.IDPRESENTACION AS [ID Presentacion],OFL.UDSNECESARIAS AS Previsto, COALESCE(SC.CONSUMIDA, 0) as Consumida, COALESCE(EXP.PRODUCCION, 0) AS Produccion," +
                " CASE WHEN((OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0)) > 0) AND(COALESCE(EXP.PRODUCCION, 0) > (OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0))) " +
                "THEN(OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0)) WHEN((OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0)) <= 0) THEN 0 ELSE COALESCE(EXP.PRODUCCION, 0) END AS Descontar," +
                " COALESCE(EXP.PRODUCCION, 0) - (CASE WHEN((OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0)) > 0) AND(COALESCE(EXP.PRODUCCION, 0) > (OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0))) " +
                "THEN(OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0)) WHEN((OFL.UDSNECESARIAS - COALESCE(SC.CONSUMIDA, 0)) <= 0) THEN 0 ELSE COALESCE(EXP.PRODUCCION, 0) END) AS 'Queda Produccion'," +
                " 0 AS 'Num Entrada Traza',0 AS 'Matrícula Descontar','UN' AS 'Tipo Unidad', CANTIDAD AS 'Cantidad', CANTIDADUNIDAD AS 'Cantidad Unidad' " +
                "FROM TBLORDENFABRICACIONCAB OFB JOIN TBLORDENFABRICACIONLIN OFL ON OFB.IDPEDIDOFAB = OFL.IDPEDIDOFAB " +
                "JOIN TBLARTICULOS a ON a.IDARTICULO = ofl.IDARTICULO LEFT JOIN VSALIDASCONSUMO sc ON sc.IDARTICULO = a.IDARTICULO AND sc.IDPEDIDOFAB = OFL.IDPEDIDOFAB " +
                "AND sc.IDPEDIDOFABLIN = OFL.IDPEDIDOFABLIN LEFT JOIN VEXISTENCIASPRODUCCION exp ON exp.IDARTICULO = a.IDARTICULO AND exp.IDPEDIDOFAB = OFL.IDPEDIDOFAB " +
                "AND exp.IDPEDIDOFABLIN = OFL.IDPEDIDOFABLIN where ofb.IDPEDIDOFAB in(" + ordenes + ")";*/
            DataTable dt = Business.GetAcopiosConsumirLineas(ordenes);
            Utilidades.TraducirDataTableColumnName(ref dt);
            rgvConsumos.DataSource = dt;
            jerarquia.BestFitColumns(BestFitColumnMode.AllCells);
            JerarquicoExistencias();
            AplicarPresentaciones(rgvConsumos);
            //rgvConsumos.BestFitColumns(BestFitColumnMode.DisplayedCells);
        }
        
        private void JerarquicoExistencias()
        {
            try
            {
                /*String sql = "SELECT DISTINCT EX.IDENTRADA as 'Matrícula',EX.CANTIDAD as 'Cantidad',EX.IDUNIDADTIPO as 'Tipo Unidad',EX.CANTIDADUNIDAD as 'Cantidad Unidad',EX.IDPALETTIPO as 'ID Palet Tipo'" +
                    ",EX.IDEXISTENCIAESTADO as 'Estado Existencia',EX.ATRIB1,EX.ATRIB2,EX.ATRIB3,EX.ATRIB4,EX.ATRIB5," +
                    "EX.PRIORIDAD as 'Prioridad',EX.IDEXISTENCIATIPO as 'Tipo Existencia', cast(0 as float) as \"Consumo  Teórico\",cast(0 as float) AS \"Devolución\",H.DESCRIPCION AS Ubicación" +
                    ", A.REFERENCIA as 'Código Artículo', A.DESCRIPCION as 'Artículo', A.ATRIBUTO as 'Atributo',A.IDPRESENTACION as \"ID Presentacion\", E.LOTE as 'Lote'," +
                    "L.FECHACADUCIDAD as 'Fecha Caducidad',OL.IDPEDIDOFAB as 'Num Orden',OL.IDPEDIDOFABLIN as 'Num OrdenLin',EX.IDHUECO" +
                    " FROM dbo.TBLMAQUINAS AS M " +
                    " INNER JOIN dbo.TBLORDENFABRICACIONCAB AS OC ON M.IDMAQUINA = OC.IDMAQUINA " +
                    " INNER JOIN dbo. VHUECOSZONA AS HZ ON M.IDZONALOGICA = HZ.IDZONACAB  " +
                    " INNER JOIN dbo.TBLEXISTENCIAS AS EX ON HZ.IDHUECO = EX.IDHUECO  " +
                    " INNER JOIN dbo.TBLHUECOS AS H ON H.IDHUECO = EX.IDHUECO  " +
                    " INNER JOIN dbo.TBLORDENFABRICACIONLIN AS OL ON EX.IDARTICULO = OL.IDARTICULO  " +
                    " INNER JOIN dbo.TBLARTICULOS AS A ON OL.IDARTICULO = A.IDARTICULO " +
                     "INNER JOIN dbo.TBLENTRADAS E ON E.IDENTRADA=EX.IDENTRADA  " +
                    " LEFT  JOIN dbo.TBLLOTES L ON E.IDARTICULO = L.IDARTICULO AND E.LOTE=L.LOTE"
                    + " WHERE ol.IDPEDIDOFAB in(" + ordenes + ")";
                DataTable tablaGridTemplate = ConexionSQL.getDataTable(sql);*/
                tablaGridTemplate = Business.GetAcopiosConsumirLineasJerarquico(ordenes);

                Utilidades.TraducirDataTableColumnName(ref tablaGridTemplate);
                GridViewRelation relacion = new GridViewRelation(rgvConsumos.MasterTemplate);
                /* jerarquia.DataSource = tablaGridTemplate;
                 relacion.ChildTemplate = jerarquia;*/

                relacion.ParentColumnNames.Add(Lenguaje.traduce("Num Orden"));
                relacion.ChildColumnNames.Add(Lenguaje.traduce("Num Orden"));
                relacion.ParentColumnNames.Add(Lenguaje.traduce("Num OrdenLin"));
                relacion.ChildColumnNames.Add(Lenguaje.traduce("Num OrdenLin"));
                rgvConsumos.Relations.Clear();
                rgvConsumos.Relations.Add(relacion);

                jerarquia.DataSource = tablaGridTemplate;
                relacion.ChildTemplate = jerarquia;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void AplicarPresentaciones(RadGridView rg)
        {
            try
            {
                foreach (GridViewRowInfo row in rg.Rows)
                {
                    int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                    int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                    Decimal previsto = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Previsto")].Value.ToString());
                    Decimal consumida = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Consumida")].Value.ToString());
                    Decimal produccion = Convert.ToDecimal(row.Cells[Lenguaje.traduce("acopiado")].Value.ToString());
                    Decimal descontar = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Descontar")].Value.ToString());
                    Decimal queda = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Queda Producción")].Value.ToString());
                    object[] presPrev = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, previsto);
                    row.Cells[Lenguaje.traduce("Previsto")].Value = Decimal.Parse(presPrev[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                    row.Cells[Lenguaje.traduce("Tipo Unidad")].Value = presPrev[1].ToString();
                    object[] presCons = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, consumida);
                    row.Cells[Lenguaje.traduce("Consumida")].Value = Decimal.Parse(presCons[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                    object[] presProd = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, produccion);
                    row.Cells[Lenguaje.traduce("acopiado")].Value = Decimal.Parse(presProd[0].ToString(), CultureInfo.CurrentCulture).ToString("G29"); 
                    object[] presDesc = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, descontar);
                    row.Cells[Lenguaje.traduce("Descontar")].Value = Decimal.Parse(presDesc[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                    object[] presQue = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, queda);
                    row.Cells[Lenguaje.traduce("Queda Producción")].Value = Decimal.Parse(presQue[0].ToString(), CultureInfo.CurrentCulture).ToString("G29");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SetPreferences()
        {
            try
            {
                rgvConsumos.Dock = DockStyle.Fill;
                for (int i = 1; i < rgvConsumos.Columns.Count; i++)
                {
                    rgvConsumos.Columns[i].ReadOnly = true;
                }

                rgvConsumos.Columns[Lenguaje.traduce("Descontar")].ReadOnly = false;
                rgvConsumos.Columns[Lenguaje.traduce("Matrícula Traza")].ReadOnly = false;
                rgvConsumos.Columns[Lenguaje.traduce("Matrícula Descontar")].ReadOnly = false;
                rgvConsumos.MultiSelect = true;
                this.rgvConsumos.MasterTemplate.EnableGrouping = true;
                this.rgvConsumos.ShowGroupPanel = true;
                this.rgvConsumos.MasterTemplate.AutoExpandGroups = true;
                this.rgvConsumos.EnableHotTracking = true;
                this.rgvConsumos.MasterTemplate.AllowAddNewRow = false;
                this.rgvConsumos.MasterTemplate.AllowColumnResize = true;
                this.rgvConsumos.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvConsumos.AllowSearchRow = true;
                this.rgvConsumos.EnablePaging = false;
                this.rgvConsumos.TableElement.RowHeight = 40;
                this.rgvConsumos.AllowRowResize = false;
                //this.rgvConsumos.Columns[Lenguaje.traduce("Consumida")].FormatString = "{0:G29}";
                //this.rgvConsumos.Columns[Lenguaje.traduce("Producción")].FormatString = "{0:G29}";
                //this.rgvConsumos.Columns[Lenguaje.traduce("Consumida")].FormatString = "{0:G29}";
                //this.rgvConsumos.Columns[Lenguaje.traduce("Previsto")].FormatString = "{0:G29}";
                //this.rgvConsumos.Columns[Lenguaje.traduce("Queda Producción")].FormatString = "{0:G29}";
                //this.rgvConsumos.Columns[Lenguaje.traduce("Descontar")].FormatString = "{0:G29}";
                this.rgvConsumos.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvConsumos.EnableFiltering = true;
                rgvConsumos.MasterTemplate.EnableFiltering = true;
                rgvConsumos.BestFitColumns();
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
                if (sender is RadCheckBoxEditor)
                {
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
                log.Info("Pulsado botón consumir web service " + DateTime.Now);
                string jsonWS = string.Empty;
                int idPedidoFab = 0;
                int idPedidoFabLin;
                int uds;
                int idEntrada;
                int idEntradaDescontar;
                foreach (GridViewRowInfo row in rgvConsumos.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        decimal cantidad = Convert.ToDecimal(row.Cells[Lenguaje.traduce("Descontar")].Value);
                        int idPresentacion = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Presentacion")].Value);
                        int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                        object[] presAlm = Presentaciones.getCantidadPresentacionAlmacenamientoArticulo(idArticulo, cantidad);
                        idPedidoFab = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num Orden")].Value);
                        idPedidoFabLin = Convert.ToInt32(row.Cells[Lenguaje.traduce("Num OrdenLin")].Value);
                        idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("Matrícula Traza")].Value);
                        idEntradaDescontar = Convert.ToInt32(row.Cells[Lenguaje.traduce("Matrícula Descontar")].Value);
                        uds = Convert.ToInt32(presAlm[0]);
                        jsonWS += formarJSONConsumirArticulo(idPedidoFab, idPedidoFabLin, uds, idEntrada, idEntradaDescontar) + ",";
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
                    var respuest = OrdenProduccion.consumirArticulos(jsonWS, DatosThread.getInstancia().getArrayDatos());
                    rgvConsumos.DataSource = null;
                    LlenarGrid(ordenes);
                    rgvConsumos.MasterTemplate.Templates.Remove(jerarquia);
                    SetPreferences();
                    //Format_GridColumns(rgvConsumos);
                    rgvConsumos.MasterTemplate.Templates.Add(jerarquia);
                    ElegirEstilo();

                    MessageBox.Show(strings.AccionCompletada, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, OrdenProduccion.Endpoint);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private string formarJSONConsumirArticulo(int idPedidoFab, int idPedidoFabLin, int uds, int idEntrada, int idEntradaDescontar)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.idPedidoFab = idPedidoFab;
            objDinamico.idPedidoFabLin = idPedidoFabLin;
            objDinamico.uds = uds;
            objDinamico.idEntrada = idEntrada;
            objDinamico.idEntradaDescontar = idEntradaDescontar;
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
                    dCol.FormatString = "{0:G29}";
                }
                if (dCol.DataType == typeof(Single))
                {
                    dCol.FormatString = "{0:G29}";
                }
            }
        }

        /* private void radGridView_1_CellFormatting(object sender, CellFormattingEventArgs e)
         {
             if (e.CellElement.ColumnIndex > 2)
             {
                 e.CellElement.Text = String.Format("{0:N2}", ((GridDataCellElement)e.CellElement).Value);
             }
         }*/

        private void consumosGridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        private void addEliminarLayoutOpciones()
        {
            FuncionesGenerales.AddEliminarLayoutButton(ref rddBtnConfiguracion);
            if (this.rddBtnConfiguracion.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.rddBtnConfiguracion.Items["RadMenuItemEliminarLayout"].Click += eliminarLayout;
            }
        }

        private void eliminarLayout(object e, EventArgs a)
        {
            this.Name = "AcopiosConsumir";
            FuncionesGenerales.EliminarLayout(this.Name + "GridView.xml", rgvConsumos);
        }

        private bool IsExpandable(GridViewRowInfo rowInfo)
        {
            if (rowInfo.ChildRows != null && rowInfo.ChildRows.Count > 0)
            {
                return true;
            }

            return false;
        }

        private void rgvConsumos_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            GridGroupExpanderCellElement cell = e.CellElement as GridGroupExpanderCellElement;
            if (cell != null && e.CellElement.RowElement is GridDataRowElement)
            {
                if (!IsExpandable(cell.RowInfo))
                {
                    cell.Expander.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                }
                else
                {
                    cell.Expander.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                }
            }
        }

        private void rgvConsumos_ChildViewExpanding(object sender, ChildViewExpandingEventArgs e)
        {
            e.Cancel = !IsExpandable(e.ParentRow);
        }

        private void ItemColumnas_Click(object sender, EventArgs e)
        {
            try
            {
                this.rgvConsumos.ShowColumnChooser();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void LoadItem_Click(object sender, EventArgs e)
        {
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(2);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                LoadLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                LoadLayoutLocal();
            }
        }

        public void SaveItem_Click(object sender, EventArgs e)
        {
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                SaveLayoutGlobal();
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                SaveLayoutLocal();
            }
        }

        public void LoadLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;
            int pathGL = 0;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                this.Name = "AcopiosConsumir";
                string s = path + "\\" + this.Name + "GridView.xml";

                s.Replace(" ", "_");

                this.rgvConsumos.LoadLayout(s);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public void LoadLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal;
            int pathGL = 1;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                this.Name = "AcopiosConsumir";
                string s = path + "\\" + this.Name + "GridView.xml";

                s.Replace(" ", "_");

                this.rgvConsumos.LoadLayout(s);
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void SaveLayoutGlobal()
        {
            string path = Persistencia.DirectorioGlobal;
            int pathGL = 0;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                this.Name = "AcopiosConsumir";
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "GridView.xml";
                path.Replace(" ", "_");
                rgvConsumos.SaveLayout(path);
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void SaveLayoutLocal()
        {
            string path = Persistencia.DirectorioLocal;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                this.Name = "AcopiosConsumir";
                Directory.CreateDirectory(path);
                path += "\\" + this.Name + "GridView.xml";
                path.Replace(" ", "_");
                rgvConsumos.SaveLayout(path);
            }
            catch (DirectoryNotFoundException e)
            {
                log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                MessageBox.Show(Lenguaje.traduce(strings.NoEncuentroRuta) + ":" + path + "\n" + Lenguaje.traduce(strings.CambiarPath));
            }
        }

        public void ElegirEstilo()
        {
            string pathLocal = Persistencia.DirectorioLocal;
            string pathGlobal = Persistencia.DirectorioGlobal;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                pathLocal += "\\Español";
                pathGlobal += "\\Español";
            }
            else
            {
                pathLocal += "\\Ingles";
                pathGlobal += "\\Ingles";
            }

            this.Name = "AcopiosConsumir";
            string pathGridView = pathLocal + "\\" + this.Name + "GridView.xml";
            bool existsGridView = File.Exists(pathGridView);
            if (existsGridView)
            {
                LoadLayoutLocal();
            }
            else
            {
                this.Name = "AcopiosConsumir";
                pathGridView = pathGlobal + "\\" + this.Name + "GridView.xml";
                existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    LoadLayoutGlobal();
                }
                else
                {
                    rgvConsumos.BestFitColumns(BestFitColumnMode.DisplayedCells);
                }
            }
        }

        private void rgvConsumos_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
        }

        private void rgvConsumos_CellValidating(object sender, CellValidatingEventArgs e)
        {
            try
            {
                if (e.Row.Index >= 0)
                {
                    Decimal previstas = Convert.ToDecimal(e.Row.Cells["Previsto"].Value.ToString());
                    Decimal consumida = Convert.ToDecimal(e.Row.Cells["Consumida"].Value.ToString());
                    Decimal produccion = Convert.ToDecimal(e.Row.Cells["Producción"].Value.ToString());
                    Decimal devolver = Convert.ToDecimal(e.Row.Cells["Queda Producción"].Value.ToString());
                    if (e.Column != null)
                    {
                        string nombreColumna = e.Column.Name;
                        if (nombreColumna.Equals("Queda Producción"))
                        {
                            if (produccion < devolver)
                            {
                                e.Row.Cells["Queda Producción"].Value = 0;
                            }
                        }
                        if (nombreColumna.Equals("Descontar"))
                        {
                            Decimal descontar = Convert.ToDecimal(e.Value.ToString());
                            devolver = produccion - descontar;
                            if (devolver < 0)
                            {
                                e.Row.Cells["Descontar"].Value = produccion;
                                if (produccion == 0)
                                {
                                    MessageBox.Show("No hay uds en produccion");
                                }
                                else
                                {
                                    MessageBox.Show(
                                            "Solo se puede descontar como maximo "
                                                    + produccion + " uds");
                                }
                                devolver = 0;
                                e.Row.Cells["Queda Producción"].Value = 0;
                            }
                            else
                            {
                                e.Row.Cells["Queda Producción"].Value = devolver;
                            }
                        }
                        if (nombreColumna.Equals("Matrícula Descontar"))
                        {
                            int idEntradaDescontar = (int)e.Value;
                            if (idEntradaDescontar > 0)
                            {
                                GridViewRowInfo matriculaRow = isIdEntradaValida(e.Row, idEntradaDescontar);

                                if (matriculaRow == null)
                                {
                                    MessageBox.Show("La entrada " + idEntradaDescontar + " no es valida");
                                    e.Cancel = true;
                                    return;
                                }
                                Decimal descontar = Convert.ToDecimal(e.Value.ToString());
                                Decimal cantidad = Convert.ToDecimal(matriculaRow.Cells["Cantidad"].Value);
                                if (cantidad <= descontar)
                                {
                                    e.Row.Cells["Descontar"].Value = matriculaRow.Cells["Cantidad"].Value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private GridViewRowInfo isIdEntradaValida(GridViewRowInfo row, int idEntrada)
        {
            GridViewRowInfo fila = null;
            try
            {
                foreach (GridViewRowInfo chidrow in row.ChildRows)
                {
                    if (Convert.ToInt32(chidrow.Cells["Matrícula"].Value.ToString()) == idEntrada)
                    {
                        fila = chidrow;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return fila;
        }
    }
}
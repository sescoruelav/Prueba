using Rumbo.Core.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.SalidaMotor;
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
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class FrmImportacionExportacion : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataTable dtStockActual = null;
        private DataTable dtArticulos = null;
        private DataTable dtHuecos = null;
        private int colSSCC = -1;
        private int colRef = -1;
        private int colQty = -1;
        private int colId = -1;
        private int colUltima = -1;
        private int filaUltima = -1;
        private int colQtyStock = -1;
        private int colDiferencia = -1;
        private int colAccion = -1;
        private int colIdArticulo = -1;
        private int colDescArticulo = -1;
        private int colIdHueco = -1;
        private int colDescUbicacionERP = -1;
        private int colDescUbicacion = -1;
        private int colSSCCRumbo = -1;
        private int colLot = -1;
        private CellValueFormat formatoSSCC = new CellValueFormat("##########00000000");

        public FrmImportacionExportacion()
        {
            InitializeComponent();
            dtHuecos = Business.getHuecos();
        }

        private void rBtnComXSSCC_Click(object sender, EventArgs e)
        {
            try
            {
                Compare2Stock("SSCC");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private int ColComparacion(string tipoComp)
        {
            int columna = -1;
            switch (tipoComp.ToUpper())
            {
                case "ID":
                    columna = colId;
                    break;

                case "SSCC":
                    columna = colSSCC;
                    break;

                default:
                    columna = -1;
                    break;
            }
            return columna;
        }

        private void Compare2Stock(string tipoComp)
        {
            string tCantidad;
            string valorComp;
            string refer;
            try
            {
                CellRange usedCellRange = rsSheetStock.ActiveWorksheet.UsedCellRange;
                Worksheet hoja = rsSheetStock.ActiveWorksheet;
                CellValueFormat formatoNum = null;

                //buscar columna SSCC si no existe msg error y salir

                if (!EstructuraComp2Stock_OK(tipoComp))
                    return;
                //añadir las columnas de cálculo
                colUltima = usedCellRange.ToIndex.ColumnIndex;
                filaUltima = usedCellRange.ToIndex.RowIndex;
                AddNuevasColumnas(colUltima);
                formatoNum = new CellValueFormat("General");

                //crear data stock que es global para evitar demasiados movimientos y el acceso global no por valor.
                dtStockActual = Business.GetStockActual();
                //Para las altas de stock hay que buscar el idarticulo partiendo de la referencia
                dtArticulos = Business.GetArticuloDescripcion();
                //recorrer excel y ajustar
                this.Cursor = Cursors.WaitCursor;
                bool fila_llena = true;//dependiendo en como se ha creado la excel, podria recorrer filas vacias inutilmente, para evitarlo se mira que tengan valor y referencia
                for (int rowIndex = usedCellRange.FromIndex.RowIndex + 1; (rowIndex < usedCellRange.ToIndex.RowIndex) && fila_llena; rowIndex++)
                {
                    tCantidad = getCeldaString(rowIndex, colQty);
                    refer = getCeldaString(rowIndex, colRef);
                    valorComp = getCeldaSSCC(rowIndex, ColComparacion(tipoComp));
                    if (valorComp.Length == 0 && refer.Length == 0)// se considera fila excel vacia si no tiene ninguno de estos campos y se deja de recorrer la excel
                        fila_llena = false;
                    else
                        EvaluarTipo(valorComp, tCantidad, rowIndex, hoja, tipoComp);
                }

                //añadir resto
                AddNoEncontrado(hoja);
                //liberar recursos principales y salir
                dtStockActual.Dispose();
                dtArticulos.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private CellValueFormat formatoGeneral = new CellValueFormat("general");

        private int getCeldaInt(int fila, int col)
        {
            int valor;
            try
            {
                CellSelection selection = rsSheetStock.ActiveWorksheet.Cells[fila, col];
                ICellValue cellValue = selection.GetValue().Value;
                valor = Convert.ToInt32(cellValue.GetResultValueAsString(formatoGeneral));
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error convirtiendo a numero fila, columna ") + (fila + 1) + " , " + (col + 1) + "\n" + ex.Message);
                return -1;
            }
        }

        private string getCeldaString(int fila, int col)
        {
            string valor;
            try
            {
                CellSelection selection = rsSheetStock.ActiveWorksheet.Cells[fila, col];
                ICellValue cellValue = selection.GetValue().Value;
                valor = cellValue.GetResultValueAsString(formatoGeneral);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error en fila, columna ") + fila + " , " + col + "\n" + ex.Message);
                return null;
            }
        }

        private string getCeldaSSCC(int fila, int col)
        {
            string valor;
            try
            {
                CellSelection selection = rsSheetStock.ActiveWorksheet.Cells[fila, col];
                ICellValue cellValue = selection.GetValue().Value;
                valor = cellValue.GetResultValueAsString(formatoSSCC);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error en fila, columna ") + fila + " , " + col + "\n" + ex.Message);
                return null;
            }
        }

        private int getCeldaHueco(int fila, int col)
        {
            int valor;
            try
            {
                CellSelection selection = rsSheetStock.ActiveWorksheet.Cells[fila, col];
                ICellValue cellValue = selection.GetValue().Value;
                if (!cellValue.GetResultValueAsString(formatoGeneral).Equals(""))
                {
                    valor = Convert.ToInt32(cellValue.GetResultValueAsString(formatoGeneral));
                }
                else
                {
                    valor = -1;
                }
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Error en fila, columna ") + fila + " , " + col + "\n" + ex.Message);
                return -1;
            }
        }

        private void AddNuevasColumnas(int ultima)
        {
            ultima++;
            if (colQtyStock < 0)
                colQtyStock = ultima++;
            if (colDiferencia < 0)
                colDiferencia = ultima++;
            if (colAccion < 0)
                colAccion = ultima++;
            if (colSSCCRumbo < 0)
                colSSCCRumbo = ultima++;
            if (colId < 0)
                colId = ultima++;
            if (colRef < 0)
                colRef = ultima++;
            if (colIdArticulo < 0)
                colIdArticulo = ultima++;
            if (colDescArticulo < 0)
                colDescArticulo = ultima++;
            if (colIdHueco < 0)
                colIdHueco = ultima++;
            if (colDescUbicacion < 0)
                colDescUbicacion = ultima++;
            rsSheetStock.ActiveWorksheet.Cells[0, colQtyStock].SetValue("Qty stock.");
            rsSheetStock.ActiveWorksheet.Cells[0, colDiferencia].SetValue("Dif.");
            rsSheetStock.ActiveWorksheet.Cells[0, colAccion].SetValue("Acc.");
            rsSheetStock.ActiveWorksheet.Cells[0, colId].SetValue("Rumbo Id");
            rsSheetStock.ActiveWorksheet.Cells[0, colRef].SetValue("REFERENC.");
            rsSheetStock.ActiveWorksheet.Cells[0, colIdArticulo].SetValue("ART RUMBO ID.");
            rsSheetStock.ActiveWorksheet.Cells[0, colDescArticulo].SetValue("ART DESC.");
            rsSheetStock.ActiveWorksheet.Cells[0, colIdHueco].SetValue(Lenguaje.traduce("Ubicacion") + " ID");
            rsSheetStock.ActiveWorksheet.Cells[0, colDescUbicacion].SetValue(Lenguaje.traduce("Ubicacion"));
            rsSheetStock.ActiveWorksheet.Cells[0, colSSCCRumbo].SetValue("SSCC Rumbo");
            Worksheet hoja = rsSheetStock.ActiveWorksheet;
            hoja.Columns[colSSCCRumbo].SetFormat(formatoSSCC);
        }

        private bool EstructuraComp2Stock_OK(string tipoComp)
        {
            switch (tipoComp.ToUpper())
            {
                case "ID":
                case "SS":
                    return EstructuraComp2StockID_OK();

                case "SSCC":
                    return EstructuraComp2StockSSCC_OK();

                default:
                    return false;
            }
        }

        private bool EstructuraComp2StockSSCC_OK()
        {
            try
            {
                Worksheet hoja = rsSheetStock.ActiveWorksheet;
                CellRange usedCellRange = hoja.UsedCellRange;
                ResetCabecera();
                for (int columnIndex = usedCellRange.FromIndex.ColumnIndex; columnIndex <= usedCellRange.ToIndex.ColumnIndex; columnIndex++)
                {
                    ICellValue cellValue = hoja.Cells[0, columnIndex].GetValue().Value;
                    string nombre = cellValue.RawValue;
                    DetectaNombreColumna(columnIndex, nombre);
                }
                if (colSSCC < 0)
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha encontrado la columna SSCC"));
                    return false;
                }
                else if (colQty < 0)
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha encontrado la columna cantidad"));
                    return false;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                return false;
            }
        }

        private bool EstructuraComp2StockID_OK()
        {
            try
            {
                Worksheet hoja = rsSheetStock.ActiveWorksheet;
                CellRange usedCellRange = hoja.UsedCellRange;
                ResetCabecera();
                for (int columnIndex = usedCellRange.FromIndex.ColumnIndex; columnIndex <= usedCellRange.ToIndex.ColumnIndex; columnIndex++)
                {
                    ICellValue cellValue = hoja.Cells[0, columnIndex].GetValue().Value;
                    string nombre = cellValue.RawValue;
                    DetectaNombreColumna(columnIndex, nombre);
                }
                if (colId < 0)
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha encontrado la columna Rumbo Id"));
                    return false;
                }
                else if (colQty < 0)
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha encontrado la columna cant."));
                    return false;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                return false;
            }
        }

        private void DetectaNombreColumna(int col, string nombre)
        {
            try
            {
                switch (nombre.ToUpper())
                {
                    case "SSCC":
                    case "PALT_NBR":
                        colSSCC = col;
                        break;

                    case "ID":
                    case "IDEXISTENCIA":
                    case "RUMBO_ID":
                        colId = col;
                        break;

                    case "REF":
                    case "PRODNBR":
                        colRef = col;
                        break;

                    case "QTY":
                    case "CANT":
                    case "CANTIDAD":
                        colQty = col;
                        break;

                    case "LOTNUMB":
                    case "LOTE":
                        colLot = col;
                        break;

                    case "LOCATION ERP":
                    case "UBICACION ERP":
                        colDescUbicacionERP = col;
                        break;
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
            }
        }

        private void ResetCabecera()
        {
            colSSCC = -1;
            colRef = -1;
            colQty = -1;
            colId = -1;
            colUltima = -1;
            filaUltima = -1;
            colQtyStock = -1;
            colDiferencia = -1;
            colAccion = -1;
        }

        private void AddNoEncontrado(Worksheet ws)
        {
            try
            {
                CellRange usedCellRange = rsSheetStock.ActiveWorksheet.UsedCellRange;
                foreach (DataRow dr in dtStockActual.Rows)
                {
                    if (!(dr.RowState == DataRowState.Deleted))
                    {
                        filaUltima++;
                        ws.Cells[filaUltima, colQtyStock].SetValue(Convert.ToDouble(dr["cantidad"]));
                        ws.Cells[filaUltima, colDiferencia].SetValue(-1 * Convert.ToDouble(dr["cantidad"]));
                        ws.Cells[filaUltima, colSSCC].SetValue(Convert.ToString(dr["SSCC"]));
                        ws.Cells[filaUltima, colRef].SetValue(Convert.ToString(dr["referencia"]));
                        ws.Cells[filaUltima, colAccion].SetValue(Lenguaje.traduce("Consumido"));
                        ws.Cells[filaUltima, colId].SetValue(Convert.ToInt32(dr["idEntrada"]));
                        ws.Cells[filaUltima, colIdArticulo].SetValue(Convert.ToInt32(dr["idArticulo"]));
                        ws.Cells[filaUltima, colDescArticulo].SetValue(Convert.ToString(dr["descripcion"]));
                        ws.Cells[filaUltima, colIdHueco].SetValue(Convert.ToInt32(dr["idHueco"]));
                        ws.Cells[filaUltima, colDescUbicacion].SetValue(Convert.ToString(dr["Ubicacion"]));
                        ws.Cells[filaUltima, colSSCCRumbo].SetValue(Convert.ToString(dr["SSCC"]));
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
            }
        }

        private bool Evaluar(string sscc, string refer, string tValor, int fila, Worksheet ws)
        {
            try
            {
                double valor;
                try
                {
                    valor = Convert.ToDouble(tValor);
                }
                catch
                {
                    valor = 0;
                    ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("Cantidad stock erronea"));
                    return false;
                }
                DataRow[] drV = dtStockActual.Select("SSCC= '" + sscc + "'");
                int contador = 0;
                foreach (DataRow dr in drV)
                {
                    contador++;
                    double qty = Convert.ToDouble(dr["cantidad"]);
                    ws.Cells[fila, colId].SetValue(Convert.ToDouble(dr["IdEntrada"]));
                    ws.Cells[fila, colQtyStock].SetValue(qty);
                    ws.Cells[fila, colDiferencia].SetValue(qty - valor);
                    ws.Cells[fila, colIdArticulo].SetValue(Convert.ToInt32(dr["idArticulo"]));
                    ws.Cells[fila, colDescArticulo].SetValue(Convert.ToString(dr["descripcion"]));
                    ws.Cells[fila, colIdHueco].SetValue(Convert.ToInt32(dr["idHueco"]));
                    ws.Cells[fila, colDescUbicacion].SetValue(Convert.ToString(dr["Ubicacion"]));
                    ws.Cells[fila, colSSCCRumbo].SetValue(Convert.ToString(dr["SSCC"]));
                    if (contador != 1)
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("DUPLICADO"));
                    else if (qty == valor)
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("coincide"));
                    else
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("ajustar"));
                    dr.Delete();//lo borro del datatable temporal, asi los sscc que queden son discrepancias.
                }
                if (contador == 0)
                {
                    ws.Cells[fila, colQtyStock].SetValue(0);
                    ws.Cells[fila, colDiferencia].SetValue(valor);
                    ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("Alta"));
                    //TODO buscar idarticulo y idhueco
                }
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                return false;
            }
        }

        private bool EvaluarTipo(string valComp, string tValor, int fila, Worksheet ws, string tipoComp)
        {
            try
            {
                double valor;
                string refArt;
                CellSelection selection;
                ICellValue cellValue;
                CellValueFormat formatoGeneral = new CellValueFormat("General");
                try
                {
                    valor = Convert.ToDouble(tValor);
                }
                catch
                {
                    valor = 0;
                    ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("Cantidad stock erronea"));
                    return false;
                }
                //tomo la referencia para asegurarme de que coincide
                selection = rsSheetStock.ActiveWorksheet.Cells[fila, colRef];
                cellValue = selection.GetValue().Value;
                refArt = cellValue.GetResultValueAsString(formatoGeneral);

                DataRow[] drV = dtStockActual.Select(SelectCompare(tipoComp, valComp));
                int contador = 0;
                foreach (DataRow dr in drV)
                {
                    contador++;
                    double qty = Convert.ToDouble(dr["cantidad"]);
                    ws.Cells[fila, colId].SetValue(Convert.ToDouble(dr["IdEntrada"]));
                    ws.Cells[fila, colQtyStock].SetValue(qty);
                    ws.Cells[fila, colDiferencia].SetValue(qty - valor);
                    ws.Cells[fila, colIdArticulo].SetValue(Convert.ToInt32(dr["idArticulo"]));
                    ws.Cells[fila, colDescArticulo].SetValue(Convert.ToString(dr["descripcion"]));
                    ws.Cells[fila, colIdHueco].SetValue(Convert.ToInt32(dr["idHueco"]));
                    ws.Cells[fila, colDescUbicacion].SetValue(Convert.ToString(dr["Ubicacion"]));
                    ws.Cells[fila, colSSCCRumbo].SetValue(Convert.ToString(dr["SSCC"]));
                    if (contador != 1)
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("DUPLICADO"));
                    else if (qty == valor)
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("coincide"));
                    else
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("ajustar"));
                    if (!refArt.Equals(Convert.ToString(dr["Referencia"])))
                    {
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("Discrepancia en referencia articulo ") + Convert.ToString(dr["Referencia"]));
                    }
                    dr.Delete();//lo borro, asi los sscc que queden son discrepancias.
                }
                if (contador == 0)//si contador es 0 es que estaba en la excel importada y no estaba en la tabla existencias
                {                 //tenemos que buscar el idarticulo a partir de la referencia de la excel importada.
                    bool artEncontrado = false;
                    DataRow[] drArts = dtArticulos.Select("referencia= '" + refArt + "'");
                    foreach (DataRow drArt in drArts)
                    {
                        ws.Cells[fila, colIdArticulo].SetValue(Convert.ToInt32(drArt["idArticulo"]));
                        ws.Cells[fila, colDescArticulo].SetValue(Convert.ToString(drArt["descripcion"]));
                        artEncontrado = true;
                    }
                    ws.Cells[fila, colQtyStock].SetValue(0);
                    ws.Cells[fila, colDiferencia].SetValue(valor);
                    if (artEncontrado)
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("Alta"));
                    else
                        ws.Cells[fila, colAccion].SetValue(Lenguaje.traduce("Articulo no encontrado"));
                }
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                return false;
            }
        }

        private string SelectCompare(string tipo, string valComp)
        {
            switch (tipo.ToUpper())
            {
                case "SSCC":
                    return "SSCC= '" + valComp + "'";

                case "ID":
                    return "IDentrada= " + valComp;

                default:
                    return null;
            }
        }

        private void rBtnComXId_Click(object sender, EventArgs e)
        {
            Compare2Stock("ID");
        }

        private void rBtnCompStockStatus_Click(object sender, EventArgs e)
        {
            try
            {
                CellRange usedCellRange = rsSheetStock.ActiveWorksheet.UsedCellRange;
                Worksheet hoja = rsSheetStock.ActiveWorksheet;
                CellValueFormat formatoNum = new CellValueFormat("General");
                CellSelection selection;
                ICellValue cellValue;
                string tCantidad;
                string ValorComp;

                if (!EstructuraComp2Stock_OK("SS"))
                    return;

                FrmSeleccionarFecha frm = new FrmSeleccionarFecha();
                frm.ShowDialog();
                frm.TopMost = true;
                if (!frm.DialogResult.Equals(DialogResult.OK))
                {
                    MessageBox.Show(this, Lenguaje.traduce("Debe seleccionar una fecha"), Lenguaje.traduce("Seleccione fecha"), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                }

                DateTime dtss = frm.fechaSeleccionada;  // Hechonew DateTime(2020, 4, 29, 0, 0, 0, 717);

                colUltima = usedCellRange.ToIndex.ColumnIndex;
                filaUltima = usedCellRange.ToIndex.RowIndex;
                AddNuevasColumnas(colUltima);
                DataTable dtSS = Business.GetStockStatus(dtss);
                //recorrer excel y ajustar

                this.Cursor = Cursors.WaitCursor;
                for (int rowIndex = usedCellRange.FromIndex.RowIndex + 1; rowIndex < usedCellRange.ToIndex.RowIndex; rowIndex++)
                {
                    selection = hoja.Cells[rowIndex, colQty];
                    cellValue = selection.GetValue().Value;
                    tCantidad = cellValue.GetResultValueAsString(formatoNum);

                    selection = hoja.Cells[rowIndex, colId];
                    cellValue = selection.GetValue().Value;
                    ValorComp = cellValue.GetResultValueAsString(formatoNum);
                    EvaluarTipo(ValorComp, tCantidad, rowIndex, hoja, "ID");
                }

                //añadir resto no borrado
                AddNoEncontrado(hoja);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        #region EXPORTA_WS

        private void rbtnExportERP_Click(object sender, EventArgs e)
        {
            string accion;
            try
            {
                WSEntradaMotorClient wsp = new WSEntradaMotorClient();
                if (colAccion < 0)
                {
                    MessageBox.Show(Lenguaje.traduce("La hoja no tiene el formato esperado"));
                    return;
                }
                CellRange usedCellRange = rsSheetStock.ActiveWorksheet.UsedCellRange;
                Worksheet hoja = rsSheetStock.ActiveWorksheet;
                CellValueFormat formatoNum = new CellValueFormat("general");

                this.Cursor = Cursors.WaitCursor;
                for (int rowIndex = usedCellRange.FromIndex.RowIndex + 1; rowIndex < usedCellRange.ToIndex.RowIndex; rowIndex++)
                {
                    accion = getCeldaString(rowIndex, colAccion);
                    switch (accion)
                    {
                        case "consumed":
                        case "Consumido":
                            ExportaBaja(getCeldaInt(rowIndex, colId), getCeldaInt(rowIndex, colQtyStock));
                            break;

                        case "Alta":
                        case "add":
                        case "new":

                            ExportaAlta(getCeldaInt(rowIndex, colQty), getCeldaString(rowIndex, colLot), getCeldaInt(rowIndex, colIdArticulo), getCeldaSSCC(rowIndex, colSSCC), getCeldaHueco(rowIndex, colIdHueco), getCeldaString(rowIndex, colDescUbicacionERP));
                            break;

                        case "ajustar":

                            ExportaBaja(getCeldaInt(rowIndex, colId), getCeldaInt(rowIndex, colDiferencia));
                            //ExportaRegularizacion(getCeldaInt(rowIndex, colId), getCeldaInt(rowIndex, colQty));//o colDiferencia
                            break;

                        case "coincide":
                        case "duplicado":
                        default://no cal fer res
                            break;
                    }
                    // si podemos retornar el idExistencia lo actualizamosen la excel, tambien los mensajes de error en las filas.
                    // idExistencia = ExportarWS(sscc, refer, valor, accion);
                    // hoja.Cells[rowIndex, colId].SetValue(idExistencia);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                // TODO MONICA throw;???
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string ExportaBaja(int idEntrada, int qty)
        {
            try
            {
                WSSalidaMotorClient salidaMotorClient = new WSSalidaMotorClient();
                log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada a WebService 'SalidaMotorClient.generarSalidaRegularizacionPaletCantidad' en Stock con parametros:IdUsuario(" + User.IdUsuario + ")" +
                    ",IdRecurso(0),IdEntrada(" + idEntrada + "),IdMotivo('IM'),comentario(),cantidad(" + qty + ")");
                salidaMotorClient.generarSalidaRegularizacionPaletCantidad(User.IdOperario, 0, idEntrada, "IM", "", qty);
                log.Debug("Llamada a WebService del usuario " + User.IdUsuario + " completada");
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                return e.Message;
            }
            return "";
        }

        private string ExportaAlta(int qtyFinal, string lote, int idArticuloFinal, string sscc, int idHueco, string ubicacionERP)
        {
            try
            {
                int respuesta = -1;
                int tipoReg = 1; //no hay reserva
                int idHuecoFinal = idHueco;
                WSEntradaMotorClient wsp = new WSEntradaMotorClient();
                DataRow valoresOrdenFila = DataAccess.GetAltaArticuloDefecto(idArticuloFinal).Rows[0];
                if (valoresOrdenFila == null)
                {
                    MessageBox.Show(Lenguaje.traduce("No ha devuelto nada"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "No existe el articulo seleccionado";
                }
                if (idHueco < 0)
                {
                    if (!ubicacionERP.Equals(""))
                    {
                        DataRow[] drHuecos = dtHuecos.Select("UBICACIONALTERNATIVA= '" + ubicacionERP + "'");
                        if (drHuecos.Length > 0)
                        {
                            idHuecoFinal = Convert.ToInt32(drHuecos[0]["IDHUECO"]);
                        }
                    }
                }
                int qtyPalet = Convert.ToInt32(valoresOrdenFila["CANTIDAD"]);
                int qtyUnidad = Convert.ToInt32(valoresOrdenFila["CANTIDADUNIDAD"]);
                int numPalets = 1;
                int idOperario = User.IdOperario; //TODO  operarioActual;
                string idUnidadTipo = valoresOrdenFila["IDUNIDADTIPO"].ToString();
                string idPaletTipo = valoresOrdenFila["IDPALETTIPO"].ToString();
                string impresora = null;
                string estado = "OK";
                string motivo = "IM";//decidir motivo fijo
                bool conReserva = false;
                bool etiqueta = false;
                string nSerie = "";
                string comentario = "REGULARIZACION STOCK IMP/EXP"; //valorar alta importacion
                string ean = null;
                object[] atributos = null;

                /* respuesta = wsp.generarEntradaPaletsRegularizacion(tipoReg, qtyFinal, qtyUnidad, numPalets, idOperario, idUnidadTipo,
                                 idPaletTipo, lote, impresora, idHuecoFinal, idArticuloFinal, estado, motivo, conReserva, etiqueta, sscc, nSerie, comentario, ean, atributos,0);*/
                return null;  //si se tiene devolver el IdEntrada, si no msg error
            }
            catch (Exception ex)
            {
                return ex.Message;// si hay error pero podemos devolver int idExistencia cambiar a int y mostrar error aqui.
            }
        }

        #endregion EXPORTA_WS

        private void rBtnCargaStockStatus_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                /* DateTime dtss = new DateTime(2020, 4, 30, 0, 0, 0, 690);*/
                FrmSeleccionarFecha frm = new FrmSeleccionarFecha();
                frm.ShowDialog();
                frm.TopMost = true;
                if (!frm.DialogResult.Equals(DialogResult.OK))
                {
                    MessageBox.Show(this, Lenguaje.traduce("Debe seleccionar una fecha"), Lenguaje.traduce("Seleccione fecha"), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                }
                DateTime dtss = frm.fechaSeleccionada;
                DataTable dtSS = Business.GetStockStatus(dtss);
                bool continuar;
                continuar = PrimeraFilaSS(dtSS.Columns);
                if (!continuar)
                    return;
                CargarValores(dtSS);
                rsSheetStock.ActiveWorksheet.Name = "SS_" + dtss.ToString("YYMMddHH");
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool PrimeraFilaSS(DataColumnCollection dtSS)
        {
            try
            {
                //Validacion de que la hoja está limpia basado el que la celda (0,0) esté vacia
                string contenido00;
                Worksheet hoja = rsSheetStock.ActiveWorksheet;
                contenido00 = hoja.Cells[0, 0].GetValue().Value.RawValue;
                if (contenido00.Length != 0)
                {
                    MessageBox.Show(Lenguaje.traduce("Esta hoja no está vacia"));
                    //TODO Ofrecer machacar o crear hoja nueva
                    return false;
                }
                //Ponemos cabeceras de coumna
                Worksheet ws = rsSheetStock.ActiveWorksheet;
                int nCol = 0;
                foreach (DataColumn col in dtSS)
                {
                    ws.Cells[0, nCol++].SetValue(col.ColumnName);
                }
                return true;
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e);
                return false;
            }
        }

        private void CargarValores(DataTable dtSS)
        {
            Worksheet ws = rsSheetStock.ActiveWorksheet;
            int fila = 0;
            int nCol = 1;
            foreach (DataRow dr in dtSS.Rows)
            {
                fila++;
                for (nCol = 0; nCol < dtSS.Columns.Count; nCol++)
                {
                    ws.Cells[fila, nCol].SetValue(Convert.ToString(dr[nCol]));
                }
            }
        }
    }
}
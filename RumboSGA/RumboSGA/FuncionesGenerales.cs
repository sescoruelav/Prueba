using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;

namespace RumboSGA
{
    public class FuncionesGenerales
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum enumLineaOpciones
        {
            add,
            edit,
            delete
        };

        public static void AddEliminarLayoutButton(ref RumDropDownButtonElement rmenu)
        {
            RumMenuItem eliminarLayout = new RumMenuItem();
            eliminarLayout.Name = "RadMenuItemEliminarLayout";
            eliminarLayout.Text = Lenguaje.traduce("Eliminar Estilo");
            rmenu.Items.Add(eliminarLayout);
        }

        public static void RumDropDownAddManual(ref RumDropDownButtonElement rmenu, int rummantenimiento)
        {
            RumMenuItem manual = new RumMenuItem();
            manual.Name = "RadMenuItemManual";
            manual.Text = Lenguaje.traduce("Manual");
            manual.rumMantenimiento = rummantenimiento;
            manual.Click += Manual_Click;
            rmenu.Items.Add(manual);
        }

        private static void Manual_Click(object sender, EventArgs e)
        {
            RumMenuItem padre = (sender as RumMenuItem);
            int idMantenimiento = padre.rumMantenimiento;
            DataAccess.abrirManual(idMantenimiento);
        }

        public static void EliminarLayout(string nombreEstilo, RadGridView rgv)
        {
            string path = "";
            if (nombreEstilo == "VisorSQLRibbonArtículosInicio.xml")
            {
                MessageBox.Show(" Este estilo solo se puede modificar y no se puede borrar");
            }
            else
            {
                try
                {
                    log.Info("Pulsado botón eliminarLayout " + DateTime.Now);

                    path = "";

                    VentanaGuardarEstilo vge = new VentanaGuardarEstilo(3);
                    vge.ShowDialog();

                    if (VentanaGuardarEstilo.guardar == 0)
                    {
                        path = Persistencia.DirectorioGlobal;
                    }
                    else if (VentanaGuardarEstilo.guardar > 0)
                    {
                        path = Persistencia.DirectorioLocal;
                    }
                    else if (VentanaGuardarEstilo.guardar == -1)
                    {
                        return;
                    }

                    if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                    {
                        path += "\\Español";
                    }
                    else
                    {
                        path += "\\Ingles";
                    }

                    path += "\\" + nombreEstilo;
                    if (!File.Exists(path))
                    {
                        RadMessageBox.Show(Lenguaje.traduce("No existe el archivo " + nombreEstilo));
                        log.Debug("No se ha eliminado el estilo: " + path + " ya que no existe");
                        return;
                    }

                    DialogResult dr = RadMessageBox.Show("¿" + Lenguaje.traduce("Estas seguro de querer eliminar el archivo ") + nombreEstilo + "?", "Eliminar estilo", MessageBoxButtons.YesNo);
                    if (dr.Equals(DialogResult.Yes))
                    {
                        File.Delete(path);
                        log.Info("Se ha eliminado el archivo " + nombreEstilo);
                        RadMessageBox.Show(Lenguaje.traduce("Eliminación completada"), Lenguaje.traduce("Resultado"));
                        if (rgv != null)
                        {
                            rgv.Refresh();
                        }
                        return;
                    }
                    return;
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Error al eliminar el estilo"));
                    log.Error("Error eliminando el archivo " + path + "  " + ex.Message + " \n " + ex.StackTrace);
                }
            }
        }

        public static void AddGuardarYCargarLayoutEnRadMenu(ref RadDropDownButton rmenu)
        {
            RumMenuItem guardarLayout = new RumMenuItem();
            RumMenuItem cargarLayout = new RumMenuItem();
            guardarLayout.Name = "RadMenuItemGuardarLayout";
            cargarLayout.Name = "RadMenuItemCargarLayout";
            guardarLayout.Text = Lenguaje.traduce("Guardar Estilo");
            cargarLayout.Text = Lenguaje.traduce("Cargar Estilo");
            rmenu.Items.Add(guardarLayout);
            rmenu.Items.Add(cargarLayout);
        }

        public static void iniciarTimer(ref Timer timerRefrescar)
        {
            try
            {
                if (Persistencia.AlertaRefresco == false) return;
                timerRefrescar = new Timer();
                timerRefrescar.Interval = Persistencia.AlertaRefrescoTiempo;
                timerRefrescar.Enabled = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error creando el timer.");
            }
        }

        public static void guardarLayout(RadGridView rgv, string nombreForm)
        {
            guardarLayout(rgv, nombreForm, rgv.Name);
        }

        public static void guardarLayout(RadGridView rgv, string nombreForm, string nombreGrid)
        {
            log.Info("Pulsado botón guardarLayout " + DateTime.Now);
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(1);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                string path = Persistencia.DirectorioGlobal;
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                {
                    path += "\\Español";
                }
                else
                {
                    path += "\\Ingles";
                }

                Directory.CreateDirectory(path);
                path += "\\" + nombreForm + nombreGrid + "GridView.xml";
                path.Replace(" ", "_");
                rgv.SaveLayout(path);
            }
            if (VentanaGuardarEstilo.guardar == 1)
            {
                string path = Persistencia.DirectorioLocal;/*XmlReaderPropio.getLayoutPath(1);*/
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                {
                    path += "\\Español";
                }
                else
                {
                    path += "\\Ingles";
                }
                Directory.CreateDirectory(path);
                path += "\\" + nombreForm + nombreGrid + "GridView.xml";
                path.Replace(" ", "_");
                rgv.SaveLayout(path);
            }
        }

        public static void cargarLayout(RadGridView rgv, string nombreForm)
        {
            cargarLayout(rgv, nombreForm, rgv.Name);
        }

        public static void ElegirLayout(RadGridView rgv, string nombreForm)
        {
            ElegirLayout(rgv, nombreForm, rgv.Name);
        }

        public static void ElegirLayout(RadGridView rgv, string nombreForm, string nombreGrid)
        {
            try
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

                string pathGridView = pathLocal + "\\" + nombreForm + nombreGrid + "GridView.xml";
                bool existsGridView = File.Exists(pathGridView);
                if (existsGridView)
                {
                    rgv.LoadLayout(pathGridView);
                }
                else
                {
                    pathGridView = pathGlobal + "\\" + nombreForm + nombreGrid + "GridView.xml";
                    existsGridView = File.Exists(pathGridView);
                    if (existsGridView)
                    {
                        rgv.LoadLayout(pathGridView);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        public static void cargarLayout(RadGridView rgv, string nombreForm, string nombreGrid)
        {
            log.Info("Pulsado botón load " + DateTime.Now);
            VentanaGuardarEstilo vge = new VentanaGuardarEstilo(2);
            vge.ShowDialog();
            if (VentanaGuardarEstilo.guardar == 0)
            {
                string path = Persistencia.DirectorioGlobal;
                if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
                {
                    path += "\\Español";
                }
                else
                {
                    path += "\\Ingles";
                }

                string s = path + "\\" + nombreForm + nombreGrid + "GridView.xml";
                s.Replace(" ", "_");
                rgv.LoadLayout(s);
                rgv.TableElement.RowHeight = 40;
            }
            if (VentanaGuardarEstilo.guardar == 1)
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

                string s = path + "\\" + nombreForm + nombreGrid + "GridView.xml";

                s.Replace(" ", "_");
                rgv.LoadLayout(s);
                rgv.TableElement.RowHeight = 40;
            }
        }

        public static void exportarAExcelGenerico(RadGridView rgv)
        {
            try
            {
                log.Info("Se va a exportar a excel " + DateTime.Now);
                if (rgv == null)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Solo disponible en vista avanzada"), Lenguaje.traduce("Advertencia"));
                    return;
                }
                GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(rgv);
                for (int i = 0; i < rgv.Columns.Count; i++)
                {
                    if (rgv.Columns[i].DataType.Name.Equals("Int32"))
                    {
                        rgv.Columns[i].ExcelExportType = DisplayFormatType.None;
                    }
                    else if (rgv.Columns[i].DataType.Name.Equals("String"))
                    {
                        rgv.Columns[i].ExcelExportType = DisplayFormatType.Text;
                    }
                    else if (rgv.Columns[i].DataType.Name.Equals("DateTime"))
                    {
                        rgv.Columns[i].ExcelExportType = DisplayFormatType.ShortDateTime;
                    }
                    else
                    {
                        rgv.Columns[i].ExcelExportType = DisplayFormatType.Text;
                    }
                }
                spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
                spreadExporter.ExportVisualSettings = true;
                bool openExportFile = false;
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "";
                dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    log.Info("Se empieza a exportar " + DateTime.Now);
                    spreadExporter.RunExport(dialog.FileName, new SpreadStreamExportRenderer());
                    log.Info("Se termina de exportar " + DateTime.Now);
                    DialogResult dr = RadMessageBox.Show(Lenguaje.traduce(strings.ExportacionExito),
                    Lenguaje.traduce(strings.ExportarExcel), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        openExportFile = true;
                    }
                }
                if (openExportFile)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(dialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format(strings.ExportarError + "\nError message: {0}", ex.Message);
                        RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
                log.Info("Se termina de exportar a excel " + DateTime.Now);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al exportar Excel en VisorSQLRibbon");
            }
        }

        public static void setCheckBoxFalse(ref RadGridView rgv, String nombreCheckBox)
        {
            try
            {
                if (rgv.Columns[nombreCheckBox] == null)
                {
                    log.Debug("Checkbox del grid " + rgv.Name + " con supuesto nombre " + nombreCheckBox + " no encontrado");
                    return;
                }
                for (int i = 0; i < rgv.Rows.Count; i++)
                {
                    if (rgv.Rows[i].Cells[nombreCheckBox].Value == null)
                        rgv.Rows[i].Cells[nombreCheckBox].Value = "False";
                }
            }
            catch (Exception ex)
            {
                log.Error("Error con setColumnCheckBoxFalse(" + rgv.Name + " " + nombreCheckBox + ")");
                ExceptionManager.GestionarError(ex);
            }
        }

        public static void setCheckBoxTrue(ref RadGridView rgv, String nombreCheckBox)
        {
            try
            {
                if (rgv.Columns[nombreCheckBox] == null)
                {
                    log.Debug("Checkbox del grid " + rgv.Name + " con supuesto nombre " + nombreCheckBox + " no encontrado");
                    return;
                }
                for (int i = 0; i < rgv.Rows.Count; i++)
                {
                    if (rgv.Rows[i].Cells[nombreCheckBox].Value == null)
                        rgv.Rows[i].Cells[nombreCheckBox].Value = "True";
                }
            }
            catch (Exception ex)
            {
                log.Error("Error con setCheckBoxTrue(" + rgv.Name + " " + nombreCheckBox + ")");
                ExceptionManager.GestionarError(ex);
            }
        }

        public static DataTable descargarJsonDatos(String nombreJson, String wherePersonalizadoFiltroInicial)
        {
            return descargarJsonDatos(nombreJson, wherePersonalizadoFiltroInicial, "", "");
        }

        public static DataTable descargarJsonDatos(String nombreJson, String wherePersonalizadoFiltroInicial, String whereFiltroVirtual, String orderByPersonalizado)
        {
            DataTable dt = null;
            String sql = Utilidades.lecturaJsonSelect(nombreJson, wherePersonalizadoFiltroInicial, whereFiltroVirtual, orderByPersonalizado);

            if (!sql.Equals(""))
            {
                dt = ConexionSQL.getDataTable(sql);
            }

            return dt;
        }

        public static GridScheme descargarJsonEsquemaGridAdjunto(String nombreJson)
        {
            return Utilidades.sacarEsquemaGrid(nombreJson);
        }

        public static string descargarJsonNombreMantenimiento(String nombreJson)
        {
            return Utilidades.descargarJsonNombreMantenimiento(nombreJson);
        }

        public static List<TableScheme> CargarEsquema(string nombreJson)
        {
            try
            {
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);
                string jsonEsquemaSelect = js.First()["Scheme"].ToString();
                return JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaSelect);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        public static int CargarEsquemaYSacarCount(string nombreJson, string filtroInicialVolatil, ref List<TableScheme> lstEsquemaTabla)
        {
            try
            {
                DataTable dt = null;
                int resultado = 0;

                lstEsquemaTabla = CargarEsquema(nombreJson);

                String filtroInicial = "";

                if (filtroInicialVolatil != null && !string.IsNullOrEmpty(filtroInicialVolatil))
                {
                    filtroInicial = filtroInicialVolatil;
                }
                else
                {
                    filtroInicial = "";
                }

                String sql = Utilidades.lecturaJsonCount(nombreJson, filtroInicial);
                dt = ConexionSQL.getDataTable(sql);
                resultado = dt.Rows[0].Field<int>(0);
                return resultado;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
            return 0;
        }

        public static void DataTableLineaEdit(ref DataTable dt, dynamic rowAnterior, List<TableScheme> listTs, string nombreJson)
        {
            try
            {
                //Sacamos el filtro para poder sustituir la línea anterior por la nueva.
                JObject rowAnt = JsonConvert.DeserializeObject<JObject>(rowAnterior);

                string filtroDataTable = "";
                string filtroWhere = "";
                foreach (TableScheme ts in listTs)
                {
                    if (ts.EsPK)
                    {
                        filtroDataTable = filtroDataTable + ts.Etiqueta + " = " + rowAnt.GetValue(ts.Nombre).ToString() + ",";
                        filtroWhere = filtroWhere + ts.Nombre + " = " + rowAnt.GetValue(ts.Nombre).ToString() + ",";
                    }
                }
                filtroDataTable = filtroDataTable.Remove(filtroDataTable.Length - 1, 1);
                filtroWhere = filtroWhere.Remove(filtroWhere.Length - 1, 1);
                DataRow[] drModificadaAnteriorArray = dt.Select(filtroDataTable);
                if (drModificadaAnteriorArray.Length == 0)
                {
                    log.Error("Error en la modificación del DataTable, la select del PK no ha dado resultados.");
                    return;
                }
                DataRow drModificadaAnterior = drModificadaAnteriorArray[0];

                if (drModificadaAnteriorArray.Length > 1)
                {
                    log.Error("Error en la modificación del DataTable, la select del PK ha dado mas de un resultado");
                    return;
                }
                int index = dt.Rows.IndexOf(drModificadaAnterior);
                DataTable lineaCambiada = descargarJsonDatos(nombreJson, filtroWhere);
                foreach (TableScheme item in listTs)
                {
                    dt.Rows[index][item.Etiqueta] = lineaCambiada.Rows[0][item.Etiqueta];
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        public static void DataTableLineaAdd(ref DataTable dt, dynamic rowAnterior, List<TableScheme> listTs, string nombreJson, string filtroWhere)
        {
            try
            {
                //Sacamos el filtro para poder sustituir la línea anterior por la nueva.
                JObject rowAnt = JsonConvert.DeserializeObject<JObject>(rowAnterior);

                DataRow dtNuevo = dt.Rows.Add();
                int index = dt.Rows.IndexOf(dtNuevo);
                DataTable lineaCambiada = descargarJsonDatos(nombreJson, filtroWhere);

                foreach (TableScheme item in listTs)
                {
                    dt.Rows[index][item.Etiqueta] = lineaCambiada.Rows[0][item.Etiqueta];
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        public static void DataTableLineaDelete(ref DataTable dt, dynamic rowAnterior, List<TableScheme> listTs, string nombreJson)
        {
            try
            {
                //Sacamos el filtro para poder sustituir la línea anterior por la nueva.
                JObject rowAnt = JsonConvert.DeserializeObject<JObject>(rowAnterior);

                string filtroDataTable = "";
                foreach (TableScheme ts in listTs)
                {
                    if (ts.EsPK)
                    {
                        filtroDataTable = filtroDataTable + ts.Etiqueta + " = " + rowAnt.GetValue(ts.Nombre).ToString() + ",";
                    }
                }
                filtroDataTable = filtroDataTable.Remove(filtroDataTable.Length - 1, 1);
                DataRow[] drModificadaAnteriorArray = dt.Select(filtroDataTable);
                if (drModificadaAnteriorArray.Length == 0)
                {
                    log.Error("Error en la eliminación del DataTable, la select del PK no ha dado resultados.");
                    return;
                }
                DataRow drModificadaAnterior = drModificadaAnteriorArray[0];

                if (drModificadaAnteriorArray.Length > 1)
                {
                    log.Error("Error en la eliminación del DataTable, la select del PK ha dado mas de un resultado");
                    return;
                }
                int index = dt.Rows.IndexOf(drModificadaAnterior);
                dt.Rows.RemoveAt(index);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        //Acepta un JValue
        public static dynamic getValorDataTable(TableScheme ts, string valor)
        {
            try
            {
                dynamic d = null;
                if (ts.Tipo.Equals("DECIMAL"))
                {
                    d = valor.Replace('.', ',').Replace("'", "");
                    d = Decimal.Parse(d);
                }
                else if (ts.Tipo.Equals("INT"))
                {
                    d = valor.Replace('.', ',').Replace("'", "");
                    d = int.Parse(d);
                }
                else if (ts.Tipo.Contains("FLOAT"))
                {
                    d = valor.Replace('.', ',').Replace("'", "");
                    d = float.Parse(d);
                }
                else if (valor.Equals("NULL"))
                {
                    d = DBNull.Value;
                }
                else
                {
                    if (valor.StartsWith("'") && valor.EndsWith("'"))
                    {
                        d = valor;
                        d = (d as string).Remove(0, 1);
                        d = (d as string).Remove((d as string).Length - 1, 1);
                    }
                    else
                        d = valor;
                }

                return d;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        public static string ComponerJson(List<TableScheme> lista, dynamic selectedRow)
        {
            JObject RowJ = JsonConvert.DeserializeObject<JObject>(selectedRow);
            JObject resultado = new JObject();
            for (int i = 0; i < lista.Count; i++)
            {
                String valorCampo = RowJ.GetValue(lista[i].Nombre).ToString();

                if (lista[i].Control == "CMB")
                {
                    valorCampo = RowJ.GetValue(lista[i].CmbObject.CampoRelacionado).ToString();
                }
                NameValue par = new NameValue(lista[i], valorCampo);
                NameValue.SacarValorALinea(ref resultado, par);
            }
            string prueba = resultado.ToString();
            return resultado.ToString();
        }

        public static string ComponerJsonDelete(List<TableScheme> lista, dynamic selectedRow)
        {
            JObject RowJ = JsonConvert.DeserializeObject<JObject>(selectedRow);
            JObject resultado = new JObject();
            for (int i = 0; i < lista.Count; i++)
            {
                String valorCampo = RowJ.GetValue(lista[i].Nombre).ToString();
                NameValue par = new NameValue(lista[i], valorCampo);
                NameValue.SacarValorALineaOnDelete(ref resultado, par);
            }
            return resultado.ToString();
        }

        public static JObject ComponerRowDesdeNameValue(List<NameValue> lstNameValue, ref string error)
        {
            try
            {
                JObject resultado = new JObject();
                error = "";
                NameValue.SacarLinea(ref resultado, ref error, lstNameValue);
                return resultado;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        public static dynamic getRowGridView(Control rgvControl)
        {
            try
            {
                return getRowGridView(rgvControl, null, null, null, null, -1);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en getRowGridView() de FuncionesGenerales.cs");
            }

            return null;
        }

        public static dynamic getRowGridView(Control rgvControl, int rowIndex)
        {
            try
            {
                return getRowGridView(rgvControl, null, null, null, null, rowIndex);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en getRowGridView() de FuncionesGenerales.cs");
            }

            return null;
        }

        public static dynamic getRowGridView(Control rgvControl, List<TableScheme> tableScheme, DataTable dtFiltrada, DataTable dtString, DataTable dtOriginal)
        {
            try
            {
                return getRowGridView(rgvControl, tableScheme, dtFiltrada, dtString, dtOriginal, -1);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en getRowGridView() de FuncionesGenerales.cs");
            }

            return null;
        }

        //Porque tantas dataTable. El virtualGrid sobre los filtrados en ejecución de las DataTable hace que la manera optima de tratar
        //las columnas del virtualGrid es con todas las columnas a String. Esto es un problema cuando queremos sacar sus valores reales.
        //El dtFiltrado tiene los datos del VirtualGrid, el dtString tiene todos los datos pero en formato string
        //y el dtOriginal es el correcto con todos los datos
        public static dynamic getRowGridView(Control rgvControl, List<TableScheme> tableScheme, DataTable dtFiltrada, DataTable dtString, DataTable dtOriginal, int rowIndex)
        {
            int lineaSeleccionada = 0;
            try
            {
                if (rgvControl == null)
                {
                    log.Debug("Error en getRowGridView() de FuncionesGenerales.cs. el grid es Null");
                }
                if (rgvControl is RadGridView)
                {
                    RadGridView rgv = (rgvControl as RadGridView);
                    if (rgv.SelectedCells.Count == 0 && rgv.CurrentCell != null && rgv.CurrentCell.RowIndex < 0)
                        return null;
                    if (rgv.SelectedRows.Count < 1)
                    {
                        if (rgv.SelectedCells.Count < 1) return null;
                        else
                        {
                            lineaSeleccionada = rgv.Rows.IndexOf(rgv.SelectedCells[0].RowInfo);
                        }
                    }
                    else
                    {
                        lineaSeleccionada = rgv.Rows.IndexOf(rgv.SelectedRows[0]);
                    }
                    if (rowIndex != -1) lineaSeleccionada = rowIndex;

                    Dictionary<dynamic, dynamic> fila = new Dictionary<dynamic, dynamic>();
                    for (int i = 0; i < rgv.ColumnCount; i++)
                    {
                        if (tableScheme != null)
                        {
                            string nombre = rgv.Columns[i].Name;
                            foreach (TableScheme item in tableScheme)
                            {
                                if (item.Etiqueta.ToUpper().Equals(rgv.Columns[i].Name.ToUpper()))
                                {
                                    nombre = item.Nombre;
                                    break;
                                }
                            }
                            fila.Add(nombre, rgv.Rows[lineaSeleccionada].Cells[i].Value);
                        }
                        else
                        {
                            fila.Add(rgv.Columns[i].Name, rgv.Rows[lineaSeleccionada].Cells[i].Value);
                        }
                    }
                    string jsonLinea = JsonConvert.SerializeObject(fila);

                    return jsonLinea;
                }
                else if (rgvControl is RadVirtualGrid && dtFiltrada != null)
                {
                    //Según telerik, no es posible sacar una fila solo con la celda seleccionada, se necesita también
                    //acceso al datasource en este caso la datatable.
                    Dictionary<dynamic, dynamic> fila = new Dictionary<dynamic, dynamic>();
                    RadVirtualGrid rvg = (rgvControl as RadVirtualGrid);
                    if (rvg.CurrentCell == null) return null;
                    int indexDentroDelFiltrado = rvg.CurrentCell.RowIndex;

                    DataRow dataRow = dtFiltrada.Rows[indexDentroDelFiltrado];

                    string busqueda = "";
                    String valor = "";
                    for (int i = 0; i < dtString.Columns.Count; i++)
                    {
                        valor = dataRow.ItemArray[i].ToString();
                        if (!(dataRow.ItemArray[i] is String) && String.IsNullOrEmpty(valor))
                        {
                            valor = " IS NULL ";
                        }
                        else
                        {
                            string valor2 = "";
                            if (dataRow.ItemArray[i].ToString().Contains("'"))
                            {
                                valor2 = dataRow.ItemArray[i].ToString().Replace("'", "''");
                            }
                            else
                            {
                                valor2 = dataRow.ItemArray[i].ToString();
                            }
                            valor = " = '" + valor2 + "' ";
                        }
                        busqueda += " [" + dtString.Columns[i].ToString() + "] " + valor + " AND ";
                    }

                    busqueda = busqueda.Remove(busqueda.LastIndexOf("AND"), 3);

                    DataRow[] dtR = dtString.Select(busqueda);

                    if (dtR.Length != 1)
                    {
                        log.Error("Error filtrando la fila en getRowGridView Virtual grid. " +
                            "La cantidad de filas seleccionadas es: " + dtR.Length + " cuando debería ser 1");
                        return "";
                    }
                    DataRow dt = dtR[0];
                    int index = dtString.Rows.IndexOf(dt);//Indice de la línea en la dtOriginal
                    for (int i = 0; i < dtOriginal.Columns.Count; i++)
                    {
                        if (tableScheme != null)
                        {
                            bool continuar = true;
                            //    string nombre = dtOriginal.Rows[index][i].ToString();
                            string nombre = dtOriginal.Columns[i].ColumnName.ToUpper();
                            foreach (TableScheme item in tableScheme)
                            {
                                if (item.Etiqueta.ToUpper().Equals(dtOriginal.Columns[i].ColumnName.ToUpper()))
                                {
                                    if (item.IndexCmb)
                                    {
                                        foreach (var fil in fila)
                                        {
                                            if (fil.Key.ToString() == item.Nombre.ToString())
                                            {
                                                Dictionary<dynamic, dynamic> guardarFila = new Dictionary<dynamic, dynamic>();
                                                guardarFila.Add(fil.Key, dtOriginal.Rows[index][i]);
                                                fila.Remove(fil.Key);
                                                fila.Add(guardarFila.First().Key, guardarFila.First().Value);
                                                continuar = false;
                                                break;
                                            }
                                        }
                                    }

                                    nombre = item.Nombre;
                                    break;
                                }
                            }
                            if (continuar)
                            {
                                log.Debug(nombre);
                                fila.Add(nombre, dtOriginal.Rows[index][i]);
                            }
                        }
                        else
                        {
                            fila.Add(dtOriginal.Rows[index][i].ToString(), dtOriginal.Rows[index][i]);
                        }
                    }

                    string jsonLinea = JsonConvert.SerializeObject(fila);
                    return jsonLinea;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en getRowGridView() de FuncionesGenerales.cs");
            }

            return null;
        }

        public static AckResponse jsonUpdate(String nombreJson, dynamic rowNueva, dynamic selectedRow)
        {
            RumLog rl = RumLog.getRumLog(nombreJson, "Editar Mantenimiento");
            bool ok = Utilidades.jsonUpdateSQL(nombreJson, rowNueva, selectedRow, ref rl);
            String error = "";
            AckResponse ack = DataAccess.ComponerACKO(ok, error);

            if (ok)
            {
                rl.resultados = ack.Resultado;
            }
            else
            {
                rl.resultados = ack.Mensaje;
            }
            RumLog.EscribirRumLog(rl);
            return ack;
        }

        public static AckResponse jsonDelete(String nombreJson, dynamic selectedRow)
        {
            RumLog rl = RumLog.getRumLog(nombreJson, "Eliminar Mantenimiento");
            String sentencia = Utilidades.jsonDeleteSQL(nombreJson, selectedRow, ref rl);
            String error = "";
            bool ok = ConexionSQL.SQLClienteExec(sentencia, ref error);
            AckResponse ack = DataAccess.ComponerACKO(ok, error);

            if (ok)
            {
                rl.resultados = ack.Resultado;
            }
            else
            {
                rl.resultados = ack.Mensaje;
            }
            RumLog.EscribirRumLog(rl);
            return ack;
        }

        public static AckResponse jsonInsert(String nombreJson, dynamic selectedRow)
        {
            RumLog rl = RumLog.getRumLog(nombreJson, "Crear Mantenimiento");
            String error = "";
            AckResponse ack = Utilidades.jsonInsertSQL(nombreJson, selectedRow, ref rl);
            //AckResponse ack = DataAccess.ComponerACKO(ok, error);

            if (ack.Resultado == "ok")
            {
                rl.resultados = ack.Resultado;
            }
            else
            {
                rl.resultados = ack.Mensaje;
            }
            RumLog.EscribirRumLog(rl);
            return ack;
        }

        //funcion deshabilitar controles por permisos
        public static void DeshabilitarControlesPermisos(int idFormulario, RadRibbonBar radRibbonBar1)
        {
            try
            {
                if (User.Perm.tienePermisoEscritura(idFormulario))
                {
                    foreach (RibbonTab item in radRibbonBar1.CommandTabs)
                    {
                        foreach (RadRibbonBarGroup grupo in item.Items)
                        {
                            if (grupo.Name != "Vista" && grupo.Name != "Conf" && grupo.Name != "EtiqCant")
                            {
                                grupo.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    foreach (RibbonTab item in radRibbonBar1.CommandTabs)
                    {
                        foreach (RadRibbonBarGroup grupo in item.Items)
                        {
                            if (grupo.Name != "Vista" && grupo.Name != "Conf" && grupo.Name != "EtiqCant")
                            {
                                grupo.Enabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EstilosColor(GridViewRowInfo cell, Color col, string columna)
        {
            //Cambia a transparente el color de la letra
            // cell.Cells[columna].Style.ForeColor = Color.Transparent;

            //Estilos del relleno
            cell.Cells[columna].Style.CustomizeFill = true;
            cell.Cells[columna].Style.GradientStyle = GradientStyles.Solid;
            cell.Cells[columna].Style.BackColor = col;

            // Estilos de los bordes
            cell.Cells[columna].Style.CustomizeBorder = true;
            cell.Cells[columna].Style.BorderWidth = 1;
            cell.Cells[columna].Style.BorderColor = Color.Black;
        }
    }
}
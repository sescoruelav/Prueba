using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace RumboSGAManager
{
    public static class Utilidades
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Hashtable Merger(this Hashtable ht, Hashtable ht1)
        {
            var e = ht1.GetEnumerator();
            while (e.MoveNext())
            {
                if (!ht.ContainsKey(e.Key))
                    ht.Add(e.Key, e.Value);
            }
            return ht;
        }

        public static void TraducirDataTableColumnName(ref DataTable tabla)
        {
            //30/11/2020 - Metida función dentro de un try-catch para evitar crash si hay un tiempo de conexión agotado
            try
            {
                for (int i = 0; i < tabla.Columns.Count; i++)
                {
                    tabla.Columns[i].ColumnName = Lenguaje.traduce(tabla.Columns[i].ColumnName);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        public static void TraducirDataTableDatos(ref DataTable tabla)
        {
            try
            {
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    for (int y = 0; y < tabla.Columns.Count; y++)
                    {
                        if (tabla.Columns[y].DataType.Name.Equals("String"))
                        {
                            tabla.Rows[i][y] = Lenguaje.traduce(tabla.Rows[i][y].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        public static void ReadOnlyTrueExceptoCheckBoxTemplate(ref GridViewTemplate tmp)
        {
            if (tmp == null) return;
            tmp.AllowAddNewRow = false;
            if (tmp.Columns.Count > 0)
            {
                foreach (GridViewColumn item in tmp.Columns)
                {
                    if (item is GridViewCheckBoxColumn)
                    {
                        item.ReadOnly = false;
                    }
                    else
                    {
                        item.ReadOnly = true;
                    }
                }
            }
        }

        public static void ReadOnlyTrueExceptoCheckBoxTemplateValor(GridViewTemplate tmp)
        {
            if (tmp == null) return;
            tmp.AllowAddNewRow = false;
            if (tmp.Columns.Count > 0)
            {
                foreach (GridViewColumn item in tmp.Columns)
                {
                    if (item is GridViewCheckBoxColumn)
                    {
                        item.ReadOnly = false;
                    }
                    else
                    {
                        item.ReadOnly = true;
                    }
                }
            }
            return;
        }

        public static void ReadOnlyTrueExceptoCheckBoxGridView(ref RadGridView tmp)
        {
            if (tmp == null) return;
            tmp.AllowAddNewRow = false;
            for (int i = 0; i < tmp.Columns.Count; i++)
            {
                if (tmp.Columns[i] is GridViewCheckBoxColumn)
                {
                    tmp.Columns[i].ReadOnly = false;
                }
                else
                {
                    tmp.Columns[i].ReadOnly = true;
                }
            }
            for (int i = 0; i < tmp.Templates.Count; i++)
            {
                for (int j = 0; j < tmp.Templates[i].Columns.Count; j++)
                {
                    if (tmp.Templates[i].Columns[j] is GridViewCheckBoxColumn)
                    {
                        tmp.Templates[i].Columns[j].ReadOnly = false;
                    }
                    else
                    {
                        tmp.Templates[i].Columns[j].ReadOnly = true;
                    }
                }
            }
        }

        public static DataTable TraducirDataTableColumnNameDT(DataTable tabla)
        {
            TraducirDataTableColumnName(ref tabla);
            return tabla;
        }

        /*
         * Metodo para convertir el valor del filtro telerik a filtro de SQL
         *
         * La variable mirarEsquemaONombre se debé a que en algunos casos el filterExpresion
         * de gridView me da el NOMBRE del SCHEME o la ETIQUETA. 1 para etiqueta 2 para nombre
         *
         * accionARealizar sera 0 cuando sea un select, 1 cuando sea una update.
         */

        public static string ConvertirFiltroValorBbdd(string filterExpression, List<TableScheme> _lstEsquemaTabla, int mirarEsquemaONombre, int accionARealizar)
        {
            while (filterExpression != string.Empty && filterExpression.Contains("]"))
            {
                int primerCorchete = filterExpression.IndexOf("[");
                int segundoCorchete = filterExpression.IndexOf("]");
                int cantidadRecorte = 1 + (segundoCorchete - primerCorchete);

                string campoNombreOriginal = filterExpression.Substring(primerCorchete, cantidadRecorte);
                string campoNombreARemplazar = campoNombreOriginal;
                campoNombreARemplazar = campoNombreARemplazar.Replace("[", "");
                campoNombreARemplazar = campoNombreARemplazar.Replace("]", "");

                for (int i = 0; i < _lstEsquemaTabla.Count; i++)
                {
                    //Sustituye en caso de ser update, el nombre y etiqueta por la respectiva consulta SQL si es un campo relacionado
                    if (accionARealizar == 1)
                    {
                        if ((mirarEsquemaONombre == 1 && campoNombreARemplazar.Equals(_lstEsquemaTabla[i].Etiqueta))
                            || (mirarEsquemaONombre == 2 && campoNombreARemplazar.Equals(_lstEsquemaTabla[i].Nombre)))
                        {
                            if (_lstEsquemaTabla[i].CmbObject != null && _lstEsquemaTabla[i].CmbObject.TablaRelacionada != null)
                            {
                                //Hay que sustituir el campo por una select que apunte al campo de la tabla relacionada
                                String nuevaSelectCampo = " (SELECT " + _lstEsquemaTabla[i].CmbObject.CampoRelacionado + " FROM "
                                    + _lstEsquemaTabla[i].CmbObject.TablaRelacionada + " WHERE " + _lstEsquemaTabla[i].CmbObject.CampoMostrado;
                                //para comprobar donde cerrar el parentesis

                                int indice = filterExpression.IndexOf(campoNombreOriginal);
                                int indiceAndSiExiste = filterExpression.IndexOf(" AND ", indice);
                                int indiceOrSiExiste = filterExpression.IndexOf(" OR ", indice);

                                int indiceAPonerParentesis = 0;

                                if (indiceAndSiExiste > indiceOrSiExiste && indiceOrSiExiste > 0)
                                {
                                    indiceAPonerParentesis = indiceOrSiExiste;
                                }
                                else
                                {
                                    indiceAPonerParentesis = indiceAndSiExiste;
                                }
                                if (indiceAPonerParentesis > 0)
                                    filterExpression = filterExpression.Insert(indiceAPonerParentesis, " ) ");
                                else
                                    filterExpression = filterExpression.Insert(filterExpression.Length - 1, " ) ");

                                filterExpression = filterExpression.Replace(campoNombreOriginal, nuevaSelectCampo);
                                break;
                            }
                        }
                    }
                    if (mirarEsquemaONombre == 1 && campoNombreARemplazar.Equals(_lstEsquemaTabla[i].Etiqueta))
                    {
                        campoNombreARemplazar = _lstEsquemaTabla[i].Nombre;
                        filterExpression = filterExpression.Replace(campoNombreOriginal, campoNombreARemplazar);
                        break;
                    }
                    else if (mirarEsquemaONombre == 2 && campoNombreARemplazar.Equals(_lstEsquemaTabla[i].Nombre))
                    {
                        campoNombreARemplazar = _lstEsquemaTabla[i].Nombre;
                        filterExpression = filterExpression.Replace(campoNombreOriginal, campoNombreARemplazar);
                        break;
                    }
                }
            }
            while (filterExpression != string.Empty && filterExpression.Contains("#"))
            {
                int prAlmohadilla = filterExpression.IndexOf("#");
                int segAlmohadilla = filterExpression.IndexOf("#", prAlmohadilla + 1);
                int cantidadRecorte = 1 + (segAlmohadilla - prAlmohadilla);

                string fechaOriginal = filterExpression.Substring(prAlmohadilla, cantidadRecorte);
                string fechaEditable = fechaOriginal.Replace("#", "");
                DateTime fechaCorrecta;
                if (DateTime.TryParseExact(fechaEditable, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out fechaCorrecta))
                {
                    string cadenaCorrecta = " BETWEEN " + "'" + fechaCorrecta.ToString("MM/dd/yyyy") + "' AND '" + fechaCorrecta.AddDays(1).ToString("MM/dd/yyyy") + "'";
                    cadenaCorrecta.Replace("=", " ");
                    filterExpression = filterExpression.Replace(fechaOriginal, cadenaCorrecta);
                    filterExpression = filterExpression.Replace("=  BETWEEN", " BETWEEN");
                }
                else
                {
                }
            }
            return filterExpression;
        }

        /**
         * Metodo que rellena el radMultiColumn, valueMember, es el valor de la fila, display member es el campo que mostrara, por ejemplo id y descripcion
         * Los filtros son los distintos campos de la bbdd en forma de String, por ejemplo {"IDARTICULO","DESCRIPCION"}, para sacarlos todos poner NULL
         * Cuando no se quiera que se filtre por nada poner "0"
         */

        public static void RellenarMultiColumnComboBox(ref RadMultiColumnComboBox radMultiColumn, DataTable datosTabla, String valueMember, String displayMember, String valueMemberPorDefecto, String[] filtros)
        {
            RellenarMultiColumnComboBox(ref radMultiColumn, datosTabla, valueMember, displayMember, valueMemberPorDefecto, filtros, false);
        }

        public static void RellenarMultiColumnComboBox(ref RadMultiColumnComboBox radMultiColumn, DataTable datosTabla, String valueMember, String displayMember, String valueMemberPorDefecto, String[] filtros, bool campoVisible)
        {
            try
            {
                Utilidades.TraducirDataTableColumnName(ref datosTabla);
                radMultiColumn.DataSource = datosTabla;
                radMultiColumn.BindingContext = new BindingContext();
                radMultiColumn.DisplayMember = Lenguaje.traduce(displayMember);
                radMultiColumn.ValueMember = Lenguaje.traduce(valueMember);

                //Delay de la busqueda en 2 segundos
                if (datosTabla.Rows.Count > 3000)
                    radMultiColumn.AutoFilterDelay = 1000;
                else
                    radMultiColumn.AutoFilterDelay = 500;

                if (filtros == null)
                {
                    filtros = new String[] { "TODOS" };
                }
                //Si filtros vale 0 entonces se desactivan los filtros
                if (!filtros[0].Equals("0"))
                {
                    if (filtros != null && filtros.Length > 0)
                    {
                        if (filtros[0].Equals("TODOS"))
                        {
                            filtros = new String[datosTabla.Columns.Count];
                            for (int i = 0; i < datosTabla.Columns.Count; i++)
                            {
                                filtros[i] = datosTabla.Columns[i].ColumnName;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < filtros.Length; i++)
                            {
                                filtros[i] = Lenguaje.traduce(filtros[i]);
                            }
                        }
                        CompositeFilterDescriptor filtroComposite = null;
                        radMultiColumn.AutoFilter = true;
                        filtroComposite = new CompositeFilterDescriptor();
                        foreach (String filtroString in filtros)
                        {
                            if (IsNumeric(datosTabla.Columns[filtroString]))
                            //if (int.TryParse(datosTabla.Rows[0][filtroString].ToString(), out int resInt))
                            {
                                filtroComposite.FilterDescriptors.Add(new FilterDescriptor(filtroString, FilterOperator.IsEqualTo, 0));
                            }
                            else
                            {
                                filtroComposite.FilterDescriptors.Add(new FilterDescriptor(filtroString, FilterOperator.Contains, ""));
                            }
                        }
                        if (filtroComposite.FilterDescriptors.Count > 1)
                            filtroComposite.LogicalOperator = FilterLogicalOperator.Or;
                        radMultiColumn.EditorControl.FilterDescriptors.Add(filtroComposite);
                    }
                }
                if (campoVisible == false)
                {
                    radMultiColumn.Columns[0].IsVisible = false;
                }
                CargarValorComboBox(ref radMultiColumn, datosTabla, valueMemberPorDefecto, valueMember, displayMember);
            }
            catch (Exception ex)
            {
                log.Error("Error en RellenarMultiColumnComboBox:" + radMultiColumn.Name);
                ExceptionManager.GestionarError(ex);
                throw ex;
            }
            CalcularTamañoMultiComboBox(datosTabla, radMultiColumn); //Tomás || Funcion que obtiene el numero de caracteres de la tabla aumenta el tamaño de la columna automaticamente
            radMultiColumn.AutoSizeDropDownToBestFit = true;
            radMultiColumn.AutoSizeDropDownHeight = true;
            radMultiColumn.AutoSizeDropDownColumnMode = BestFitColumnMode.DisplayedCells;
            radMultiColumn.AutoCompleteMode = AutoCompleteMode.Suggest;
        }

        public static void CargarValorComboBox(ref RadMultiColumnComboBox radMultiColumn, DataTable datosTabla, String valueMemberPorDefecto, String valueMember, String displayMember)
        {
            try
            {
                if (valueMemberPorDefecto.Equals(""))
                {
                    radMultiColumn.Text = "";
                }
                else if (valueMember.Equals("-1"))
                {
                    DataRow[] resultados = datosTabla.Select("[" + Lenguaje.traduce(valueMember) + "] = '" + datosTabla.Rows[0][0].ToString().Replace("'", "''") + "'");
                    if (resultados.Length > 0)
                    {
                        radMultiColumn.Text = resultados[0][Lenguaje.traduce(displayMember)].ToString();
                    }
                    else
                    {
                        radMultiColumn.Text = "";
                    }
                }
                else
                {
                    try
                    {
                        DataRow[] resultados = datosTabla.Select("[" + Lenguaje.traduce(valueMember) + "] = '" + valueMemberPorDefecto.Replace("'", "''") + "'");
                        if (resultados.Length > 0)
                        {
                            radMultiColumn.Text = resultados[0][Lenguaje.traduce(displayMember)].ToString();
                            radMultiColumn.SelectedIndex = datosTabla.Rows.IndexOf(resultados[0]);
                        }
                        else
                        {
                            radMultiColumn.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.GestionarErrorNuevo(ex,
                            "Error al rellenar el RellenarMultiColumnComboBox con el valor:" + valueMemberPorDefecto.Replace("'", "''") +
                            " en la columna:" + valueMember);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error cargando valor en RadMultiColumn. " +
                    "ValueMember:" + valueMember + ",ValueMemberDefecto:" + valueMemberPorDefecto + ",DisplayMember:" + displayMember);
            }
        }

        //Tomás || Funcion que obtiene el numero de caracteres de la tabla y cambia el tamaño dependiendo de los caracteres que tiene para hacer el autosize
        public static void CalcularTamañoMultiComboBox(DataTable datos, RadMultiColumnComboBox comboBox)
        {
            var maxLength = Enumerable.Range(0, datos.Columns.Count).Select(col => datos.AsEnumerable().Select(row => row[col]).OfType<string>().Max(val => val?.Length)).ToList();

            for (int i = 0; i < datos.Columns.Count; i++)
            {
                if (maxLength[i] != null)
                {
                    comboBox.Columns[i].MinWidth = 10 * Convert.ToInt32(maxLength[i]);
                }
            }
        }

        //Metodo que añade a la DataTable una nueva columna con un checkbox marcado o descmarcado según el campo actual este en S o N.
        public static void RellenarConCBSN(ref DataTable dt, List<TableScheme> _lstEsquemaTabla)
        {
            for (int i = 0; i < _lstEsquemaTabla.Count; i++)
            {
                if (_lstEsquemaTabla[i].Control != null &&
                    _lstEsquemaTabla[i].Control.Equals("CBSN"))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.Equals(_lstEsquemaTabla[i].Nombre) ||
                            dt.Columns[j].ColumnName.Equals(_lstEsquemaTabla[i].Etiqueta))
                        {
                            String nombreCheckBox = "Check " + _lstEsquemaTabla[i].Etiqueta;
                            DataColumn dtc = new DataColumn(nombreCheckBox, typeof(Boolean));

                            dt.Columns.Add(dtc);
                            for (int x = 0; x < dt.Rows.Count; x++)
                            {
                                if (dt.Columns[nombreCheckBox] != null)
                                {
                                    if (dt.Rows[x][dt.Columns[j].ColumnName].ToString().Equals("S"))
                                    {
                                        dt.Rows[x][nombreCheckBox] = true;
                                    }
                                    else
                                    {
                                        dt.Rows[x][nombreCheckBox] = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool IsNumeric(this DataColumn col)
        {
            if (col == null)
                return false;
            // Make this const
            var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
        typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
        typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};
            return numericTypes.Contains(col.DataType);
        }

        public static void traduccionDropDown(ref RadDropDownList r)
        {
            if (r.Items.Count < 1) return;

            for (int i = 0; i < r.Items.Count; i++)
            {
                r.Items[i].Text = Lenguaje.traduce(r.Items[i].Text);
            }
        }

        public static void refrescarJerarquico(ref RadGridView gridViewActualizar, int jerarquicoARecargar)
        {
            try
            {
                if (jerarquicoARecargar == -1)
                {
                    for (int i = 0; i < gridViewActualizar.MasterTemplate.Templates.Count; i++)
                    {
                        gridViewActualizar.MasterTemplate.Templates[i].Refresh();
                    }
                    return;
                }
                gridViewActualizar.MasterTemplate.Templates[jerarquicoARecargar].Refresh();
            }
            catch (Exception ex)
            {
                log.Error("Error recargando el jerarquico. " + ex.Message);
            }
        }

        public static int buscarDondeEstaCheckBox(ref RadGridView rgv)
        {
            if (rgv == null) return -1;
            if (rgv.Columns.Count == 0) return -1;
            foreach (GridViewColumn item in rgv.Columns)
            {
                if (item is GridViewCheckBoxColumn)
                {
                    return item.Index;
                }
            }
            //Como la mayoría de las veces esta en la primera posición, pongo la -1 por si acaso.
            return -1;
        }

        public static string ponerCorchetesCampo(String campo)
        {
            String campoR = campo;
            if (campo.Contains(")") || (campo.StartsWith("'") && campo.EndsWith("'")))
            {
            }
            else if (campo.Contains("."))
            {
                campoR = campoR.Insert(campo.IndexOf(".") + 1, "[");
                int espacioEnBlancoPostPunto = campoR.IndexOf(" ", campo.IndexOf("."));
                if (espacioEnBlancoPostPunto != -1)
                {
                    campoR = campoR.Insert(espacioEnBlancoPostPunto, "]");
                }
                else
                {
                    campoR += "]";
                }
            }
            else
            {
                campoR = "[" + campo + "]";
            }
            return campoR;
        }

        private static string sacarAlias(String campo)
        {
            if (campo.Contains("."))
            {
                int indexAlias = campo.IndexOf('.');
                string alias = campo.Substring(0, indexAlias);
                return alias;
            }
            else
            {
                return "";
            }
        }

        public static string sacarCampo(String campo)
        {
            if (campo.Contains("."))
            {
                int indexAlias = campo.IndexOf('.');
                string alias = campo.Substring(indexAlias + 1, campo.Length - indexAlias - 1);
                /*      if (campo.Contains("( SELECT"))
                      {
                          string select = campo.Substring(0, indexAlias + -1);
                          alias = select + alias;
                      }*/
                /*    if (alias.Contains(')'))
                    {
                        alias = alias.Substring(0, alias.Length - 1);
                    }*/
                return alias;
            }
            else
            {
                return campo;
            }
        }

        public static string[] separarPorComas(String campo)
        {
            string[] words = campo.Split(',');

            return words;
        }

        public static GridScheme sacarEsquemaGrid(String nombreJson)
        {
            try
            {
                GridScheme gs = null;
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);
                if (js.First()["DataGridLin"] == null) return null;
                string jsonGridEsquema = js.First()["DataGridLin"].ToString();
                if (!string.IsNullOrEmpty(jsonGridEsquema))
                {
                    gs = JsonConvert.DeserializeObject<GridScheme>(jsonGridEsquema);
                }

                return gs;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        public static string descargarJsonNombreMantenimiento(String nombreJson)
        {
            try
            {
                String nombreMantenimiento = "";
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);
                if (js.First()["NombreMantenimiento"] == null) return "";
                nombreMantenimiento = js.First()["NombreMantenimiento"].ToString();

                return nombreMantenimiento;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return "";
        }

        public static string lecturaJsonSelect(String nombreJson, String wherePersonalizadoFiltroInicial, String whereFiltroVirtual, String orderByPersonalizado)
        {
            //Metodo no en uso, ideas generales para leer un Json comodamente.
            //Se quiere establecer con esto un recorrido para leer Jsons y descargar datos centralizado.

            try
            {
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);
                string jsonEsquemaSelect = js.First()["Scheme"].ToString();
                string jsonFrom = js.First()["FROM"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaSelect);
                String consulta = "SELECT ";

                int contadorJoins = 0;
                foreach (TableScheme item in lstEsquemaTabla)
                {
                    String itemNombre = ponerCorchetesCampo(item.Nombre);

                    if (item.CmbObject != null && item.CmbObject.CampoMostrado != null)
                    {
                        //Se tiene que sustituir por un Join o un OuterApply para mejorar renidmiento
                        ComboScheme c = item.CmbObject;
                        String aliasCombo = "J" + contadorJoins;

                        int lenght = itemNombre.IndexOf(']') - itemNombre.IndexOf('[');
                        String campoRelacionadoPadre = itemNombre.Substring(itemNombre.IndexOf('[') + 1, lenght - 1);

                        jsonFrom += " LEFT JOIN " + c.TablaRelacionada + " AS " + aliasCombo
                            + " ON " + c.AliasPadre + "." + campoRelacionadoPadre + " = " + aliasCombo + "." + c.CampoRelacionado + " ";

                        consulta += aliasCombo + ".[" + c.CampoMostrado + "] AS [" + item.Etiqueta + "]";

                        consulta += "," + aliasCombo + ".[" + c.CampoRelacionado + "] AS [" + c.CampoRelacionado + "]";

                        if (item.CmbObject.CampoMostradoAux != null)
                        {
                            string[] words = separarPorComas(item.CmbObject.CampoMostradoAux);

                            foreach (string word in words)
                            {
                                consulta += "," + aliasCombo + ".[" + word + "] AS [" + word + "]";
                            }
                        }

                        contadorJoins++;
                        consulta += ", ";
                        continue;
                    }

                    consulta += itemNombre + " AS [" + item.Etiqueta + "]";

                    consulta += ", ";
                }
                consulta = consulta.Remove(consulta.LastIndexOf(','), 1);

                consulta += " FROM " + jsonFrom + " ";

                string jsonWhere = "";
                if (String.IsNullOrEmpty(wherePersonalizadoFiltroInicial))
                {
                    if (js.First()["WHERE"] != null)
                    {
                        jsonWhere = js.First()["WHERE"].ToString();
                        if (jsonWhere != null && !jsonWhere.Equals(""))
                        {
                            consulta += " WHERE " + jsonWhere;
                        }
                    }
                }
                else
                {
                    jsonWhere = wherePersonalizadoFiltroInicial;
                    if (!jsonWhere.ToLower().Contains("where"))
                    {
                        consulta += " WHERE ";
                    }
                    consulta += jsonWhere;
                }

                if (!String.IsNullOrEmpty(whereFiltroVirtual))
                {
                    jsonWhere = whereFiltroVirtual;
                    if (!jsonWhere.ToLower().Contains("where") && !consulta.ToLower().Contains("where"))
                    {
                        consulta += " WHERE " + jsonWhere + " ";
                    }
                    consulta += " AND " + jsonWhere + " ";
                }

                if (js.First()["Order By"] != null && !String.IsNullOrEmpty(js.First()["Order By"].ToString()) && String.IsNullOrEmpty(orderByPersonalizado))
                {
                    String orderBy = " ORDER BY " + js.First()["Order By"].ToString() + " ";
                    consulta += orderBy;
                }
                else
                if (!String.IsNullOrEmpty(orderByPersonalizado))
                {
                    consulta += orderByPersonalizado;
                }

                if (js.First()["Group By"] != null && !String.IsNullOrEmpty(js.First()["Group By"].ToString()))
                {
                    consulta += " GROUP BY " + js.First()["Group By"].ToString();
                }

                return consulta;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce("Error cargando los datos"));
                ExceptionManager.GestionarErrorNuevo(ex, "Error en lecturaJsonSelect NombreJson:" + nombreJson
                    + " wherePersonalizadoFiltroInicial:" + Convert.ToString(wherePersonalizadoFiltroInicial));
                return "";
            }
        }

        public static string lecturaJsonCount(String nombreJson, String wherePersonalizadoFiltroInicial)
        {
            //Metodo para contar el total de un json.

            try
            {
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);
                string jsonEsquemaSelect = js.First()["Scheme"].ToString();
                string jsonFrom = js.First()["FROM"].ToString();

                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaSelect);
                String consulta = "SELECT COUNT(*) ";

                int contadorJoins = 0;
                foreach (TableScheme item in lstEsquemaTabla)
                {
                    String itemNombre = ponerCorchetesCampo(item.Nombre);

                    if (item.CmbObject != null && item.CmbObject.CampoMostrado != null)
                    {
                        //Se tiene que sustituir por un Join o un OuterApply para mejorar renidmiento
                        ComboScheme c = item.CmbObject;
                        String aliasCombo = "J" + contadorJoins;

                        int lenght = itemNombre.IndexOf(']') - itemNombre.IndexOf('[');
                        String campoRelacionadoPadre = itemNombre.Substring(itemNombre.IndexOf('[') + 1, lenght - 1);

                        jsonFrom += " LEFT JOIN " + c.TablaRelacionada + " AS " + aliasCombo
                            + " ON " + c.AliasPadre + "." + campoRelacionadoPadre + " = " + aliasCombo + "." + c.CampoRelacionado + " ";

                        contadorJoins++;
                        continue;
                    }
                }

                consulta += " FROM " + jsonFrom + " ";

                string jsonWhere = "";
                if (String.IsNullOrEmpty(wherePersonalizadoFiltroInicial))
                {
                    if (js.First()["WHERE"] != null)
                    {
                        jsonWhere = js.First()["WHERE"].ToString();
                        if (jsonWhere != null && !jsonWhere.Equals(""))
                        {
                            consulta += " WHERE " + jsonWhere;
                        }
                    }
                }
                else
                {
                    jsonWhere = wherePersonalizadoFiltroInicial;
                }

                return consulta;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(Lenguaje.traduce("Error cargando los datos"));
                ExceptionManager.GestionarErrorNuevo(ex, "Error en lecturaJsonCount NombreJson:" + nombreJson
                    + " wherePersonalizadoFiltroInicial:" + Convert.ToString(wherePersonalizadoFiltroInicial));
                return "";
            }
        }

        public static bool jsonUpdateSQL(String nombreJson, dynamic rowNueva, dynamic selectedRow, ref RumLog rl)
        {
            bool bolOk = false;
            bool contieneSelect = false;
            try
            {
                String sentencia = "UPDATE ";
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);

                string jsonEsquemaSelect = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaSelect);
                string aliasPrincipalJson = js.First()["AliasPrincipal"].ToString();
                string tablaPrincipalJson = js.First()["TablaPrincipal"].ToString();
                sentencia += tablaPrincipalJson + "  SET ";
                JObject lineaEditada = JsonConvert.DeserializeObject<JObject>(rowNueva);
                JObject lineaAnterior = JsonConvert.DeserializeObject<JObject>(selectedRow);
                String where = "";

                using (SqlConnection connection = new SqlConnection(ConexionSQL.getConnectionString()))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(null, connection);
                        command.Connection = connection;

                        foreach (TableScheme item in lstEsquemaTabla)
                        {
                            string converionMayusculas = item.Nombre.ToUpper();
                            // string selectDatos = "";
                            if (item.ExisteTabla)
                            {
                                continue;
                            }
                            if (item.IndexCmb)
                            {
                                continue;
                            }

                            /*  if (converionMayusculas.Contains("( SELECT"))
                              {
                                  string select = converionMayusculas.Substring(0, converionMayusculas.IndexOf('.') + -1);
                                  selectDatos = select + selectDatos;
                                  contieneSelect = true;
                              }*/

                            string quitarLetra = sacarCampo(converionMayusculas);
                            if (quitarLetra == "COSTETRANSPORTEESTIMADO")
                            {
                                log.Debug("prueba");
                            }
                            SqlParameter idParam = new SqlParameter("@" + quitarLetra, conversorTipoDatos(item.Tipo), 100);
                            string query = "";
                            quitarComillasFecha(idParam, lineaEditada, item);

                            string remplazarPunto = lineaEditada.GetValue(item.Nombre).ToString();

                            idParam = parseDatos(item, idParam, remplazarPunto);
                            /*  if (contieneSelect)
                              {
                                  query = selectDatos + " @" + quitarLetra + "), ";
                                  // command.Parameters.Add(idParam);
                              }
                              else
                              {
                                  if (!item.EsPK)
                                  {
                                      query = quitarLetra + " = @" + quitarLetra + ", ";
                                      command.Parameters.Add(idParam);
                                  }
                                  else
                                  {
                                      command.Parameters.Add(idParam);
                                  }
                              }*/
                            if (!item.EsPK)
                            {
                                String aliasDelCampo = sacarAlias(item.Nombre);
                                if (!String.IsNullOrEmpty(aliasPrincipalJson) && aliasDelCampo.Equals(aliasPrincipalJson))
                                {
                                    if (idParam != null)
                                    {
                                        query = quitarLetra + " = @" + quitarLetra + ", ";
                                        command.Parameters.Add(idParam);
                                    }
                                }
                            }
                            else
                            {
                                command.Parameters.Add(idParam);

                                /*   if (nombreJson == "EANProveedor" && quitarLetra == "IDARTICULO")
                                   {
                                       bool existeIdArticulo = true;
                                       if (command.Parameters.Count > 0)
                                       {
                                           foreach (SqlParameter parametro in command.Parameters)
                                           {
                                               if (parametro.ParameterName == "@IDARTICULO")
                                               {
                                                   existeIdArticulo = false;
                                               }
                                           }
                                       }
                                       if (existeIdArticulo)
                                       {
                                           command.Parameters.Add(idParam);
                                       }
                                   }
                                   else
                                   {
                                   }*/
                            }

                            /*if (nombreJson == "EANProveedor" && quitarLetra == "IDARTICULO")
                             {
                                 foreach (SqlParameter parametro in command.Parameters)
                                 {
                                     if (parametro.ParameterName == "@IDARTICULO")
                                     {
                                         parametro.Value = idParam.Value;
                                     }
                                 }
                             }
                            */
                            idParam.SqlDbType = conversorTipoDatos(item.Tipo);
                            idParam.Size = 100;
                            if (idParam.SqlDbType == SqlDbType.Decimal)
                            {
                                idParam.Precision = 18;
                                idParam.Scale = 6;
                            }
                            if (item.PuedeNull)
                            {
                                idParam.IsNullable = true;
                            }
                            query = query.ToUpper();

                            log.Info(quitarLetra + " - " + idParam.Value);
                            log.Info(" Tipo :  " + item.Tipo);

                            if (item.EsPK)
                            {
                                if (lineaEditada.GetValue(item.Nombre).ToString() != lineaAnterior.GetValue(item.Nombre).ToString())
                                {
                                    query = quitarLetra + " = @" + quitarLetra + ", ";

                                    SqlParameter idParamV = new SqlParameter("@" + quitarLetra + "V", conversorTipoDatos(item.Tipo), 100);

                                    idParamV.Value = lineaAnterior.GetValue(item.Nombre).ToString();
                                    string remplazarPuntoV = lineaAnterior.GetValue(item.Nombre).ToString();

                                    idParamV = parseDatos(item, idParamV, remplazarPuntoV);
                                    command.Parameters.Add(idParamV);

                                    where += quitarLetra + " = @" + quitarLetra + "V AND ";
                                }
                                else
                                {
                                    where += quitarLetra + " = @" + quitarLetra + " AND ";
                                }
                            }
                            sentencia += query.Replace("A.", "");
                            //  contieneSelect = false;
                        }

                        sentencia = sentencia.Remove(sentencia.LastIndexOf(','), 1);
                        where = where.Remove(where.LastIndexOf("AND"), 3);

                        if (String.IsNullOrEmpty(where))
                        {
                            log.Error("Error en la construcción de la sentencia UPDATE en el JSON " + nombreJson);
                            bolOk = false;
                            return bolOk;
                        }
                        sentencia = sentencia + " WHERE " + where;
                        command.CommandText = sentencia;
                        command.CommandType = CommandType.Text;
                        log.Error(command.CommandText);

                        command.Prepare();
                        command.ExecuteNonQuery();
                        bolOk = true;
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message);
                        bolOk = false;
                        ExceptionManager.GestionarErrorNuevo(e, "");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en jsonUpdateSQL NombreJson:" + nombreJson +
                    " selectedRow:" + Convert.ToString(rowNueva));
            }
            return bolOk;
        }

        public static string jsonDeleteSQL(String nombreJson, dynamic selectedRow, ref RumLog rl)
        {
            string logRumLog = "";
            try
            {
                String sentencia = "DELETE FROM ";
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);

                string jsonEsquemaSelect = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaSelect);
                string aliasPrincipalJson = js.First()["AliasPrincipal"].ToString();
                string tablaPrincipalJson = js.First()["TablaPrincipal"].ToString();
                sentencia += tablaPrincipalJson + " WHERE ";
                JObject lineaEditada = JsonConvert.DeserializeObject<JObject>(selectedRow);
                String where = "";
                foreach (TableScheme item in lstEsquemaTabla)
                {
                    if (item.ExisteTabla)
                    {
                        continue;
                    }
                    String nombreCampo = sacarCampo(item.Nombre);

                    String valorCampo = lineaEditada.GetValue(item.Nombre).ToString();

                    String aliasDelCampo = sacarAlias(item.Nombre);
                    if (!String.IsNullOrEmpty(aliasPrincipalJson) && aliasDelCampo.Equals(aliasPrincipalJson)
                        && item.EsPK)
                    {
                        valorCampo = lineaEditada.GetValue(item.Nombre).ToString();
                        if (item.Tipo.Equals("DATETIME") && !valorCampo.Equals("NULL"))
                        {
                            where += " (" + nombreCampo + " BETWEEN" + valorCampo +
                                " AND DATEADD(SS,1,CAST(" + valorCampo + " AS DATETIME))) AND ";
                        }
                        else if (valorCampo != null && (valorCampo.Equals("") || valorCampo.Equals("''") || valorCampo.Equals("NULL")))
                        {
                            valorCampo = "''";
                            where += " (" + nombreCampo + "=" + valorCampo + " OR " +
                                nombreCampo + " IS NULL) AND ";
                        }
                        else
                        {
                            where += " " + nombreCampo + "=" + valorCampo + " AND ";
                        }
                        logRumLog += " " + Lenguaje.traduce(nombreCampo) + "=" + valorCampo + " AND ";
                    }
                }

                where = where.Remove(where.LastIndexOf("AND"), 3);

                if (!log.Equals(""))
                {
                    logRumLog = logRumLog.Remove(logRumLog.LastIndexOf("AND"), 3);
                }

                if (String.IsNullOrEmpty(where))
                {
                    log.Error("Error en la construcción de la sentencia UPDATE en el JSON " + nombreJson);
                    return "";
                }

                if (tablaPrincipalJson == "TBLARTICULOS")
                {
                    String error = "";
                    ConexionSQL.SQLClienteExec("DELETE FROM TBLEANPROVEEDOR WHERE" + where, ref error);
                }

                sentencia = sentencia + " " + where;
                rl.parametros = logRumLog;
                return sentencia;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en jsonDeleteSQL NombreJson:" + nombreJson
                    + " selectedRow:" + Convert.ToString(selectedRow));
            }
            return "";
        }

        public static AckResponse jsonInsertSQL(String nombreJson, dynamic selectedRow, ref RumLog rl)
        {
            bool bolOk = false;
            string error = "";
            AckResponse ack;
            try
            {
                String sentencia = "INSERT INTO ";
                string json = DataAccess.LoadJson(nombreJson);
                JArray js = JArray.Parse(json);

                String columnas = " (";
                String valores = " VALUES (";

                string jsonEsquemaSelect = js.First()["Scheme"].ToString();
                List<TableScheme> lstEsquemaTabla = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquemaSelect);
                string aliasPrincipalJson = js.First()["AliasPrincipal"].ToString();
                string tablaPrincipalJson = js.First()["TablaPrincipal"].ToString();
                sentencia += tablaPrincipalJson;

                JObject lineaEditada = JsonConvert.DeserializeObject<JObject>(selectedRow);
                String mensaje = "";

                using (SqlConnection connection = new SqlConnection(ConexionSQL.getConnectionString()))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(null, connection);
                        command.Connection = connection;

                        foreach (TableScheme item in lstEsquemaTabla)
                        {
                            if (item.IndexCmb)
                            {
                                continue;
                            }

                            string converionMayusculas = item.Nombre.ToUpper();
                            string quitarLetra = sacarCampo(converionMayusculas);
                            SqlParameter idParam = new SqlParameter("@" + quitarLetra, conversorTipoDatos(item.Tipo), 100);

                            idParam.Value = lineaEditada.GetValue(item.Nombre).ToString();
                            quitarComillasFecha(idParam, lineaEditada, item);
                            string remplazarPunto = lineaEditada.GetValue(item.Nombre).ToString();

                            idParam = parseDatos(item, idParam, remplazarPunto);
                            // idParam.IsNullable = true;

                            idParam.SqlDbType = conversorTipoDatos(item.Tipo);
                            idParam.Size = 100;
                            if (idParam.SqlDbType == SqlDbType.Decimal)
                            {
                                idParam.Precision = 18;
                                idParam.Scale = 6;
                            }
                            if (item.PuedeNull)
                            {
                                idParam.IsNullable = true;
                            }
                            if (item.AutoIncrementado)
                            {
                                continue;
                            }
                            else
                            {
                                String aliasDelCampo = sacarAlias(item.Nombre);

                                if (!String.IsNullOrEmpty(aliasPrincipalJson) && aliasDelCampo.Equals(aliasPrincipalJson))
                                {
                                    if (idParam != null)
                                    {
                                        columnas += quitarLetra + ", ";
                                        valores += "@" + quitarLetra + ",";
                                        command.Parameters.Add(idParam);
                                    }
                                }
                            }
                            log.Info(quitarLetra + " - " + idParam.Value);
                            log.Info(" Tipo :  " + item.Tipo);
                        }

                        columnas = columnas.Remove(columnas.LastIndexOf(','), 1) + ")";
                        valores = valores.Remove(valores.LastIndexOf(','), 1) + ")";
                        sentencia = sentencia + columnas + valores;
                        if (!mensaje.Equals(""))
                            mensaje = mensaje.Remove(mensaje.LastIndexOf(","), 1);
                        rl.parametros = mensaje;

                        command.CommandText = sentencia;
                        command.CommandType = CommandType.Text;
                        log.Error(command.CommandText);

                        command.Prepare();
                        command.ExecuteNonQuery();
                        bolOk = true;
                    }
                    catch (Exception e)
                    {
                        log.Error(e.Message);
                        ExceptionManager.GestionarErrorNuevo(e);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en jsonInsertSQL NombreJson:" + nombreJson +
                    " selectedRow:" + Convert.ToString(selectedRow));
            }
            ack = DataAccess.ComponerACKO(bolOk, error);
            return ack;
        }

        private static void Permiso(RadButtonItem control, int id)
        {
            if (User.Perm.comprobarAcceso(id) == false)
            {
                control.Enabled = false;
            }
            else
            {
                if (User.Perm.tienePermisoEscritura(id) == true)
                {
                    control.Enabled = true;
                }
                else
                {
                    control.Enabled = false;
                }
            }
        }

        public static void ConversorMascaraTipo(DataTable datatable, GridViewRowInfo row, DataRow dataRow, int i)
        {
            try
            {
                switch (datatable.Columns[i].DataType.Name.ToString())
                {
                    case "Decimal":
                        row.Cells[datatable.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N5}", Decimal.Parse(dataRow[datatable.Columns[i].ColumnName].ToString()));
                        row.Cells[datatable.Columns[i].ColumnName].ColumnInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
                        break;

                    case "Single":
                        row.Cells[datatable.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N5}", Single.Parse(dataRow[datatable.Columns[i].ColumnName].ToString()));
                        row.Cells[datatable.Columns[i].ColumnName].ColumnInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
                        break;

                    case "Double":
                        row.Cells[datatable.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N5}", Double.Parse(dataRow[datatable.Columns[i].ColumnName].ToString()));
                        row.Cells[datatable.Columns[i].ColumnName].ColumnInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
                        break;

                    case "Int64":
                        row.Cells[datatable.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0:N5}", Int64.Parse(dataRow[datatable.Columns[i].ColumnName].ToString()));
                        row.Cells[datatable.Columns[i].ColumnName].ColumnInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
                        break;

                    case "Int32":
                        row.Cells[datatable.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0}", Int32.Parse(dataRow[datatable.Columns[i].ColumnName].ToString()));
                        row.Cells[datatable.Columns[i].ColumnName].ColumnInfo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
                        break;

                    default:
                        row.Cells[datatable.Columns[i].ColumnName].Value = String.Format(CultureInfo.InvariantCulture, "{0}", dataRow[datatable.Columns[i].ColumnName].ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Se ha producido un error");
            }
        }

        public static SqlDbType conversorTipoDatos(string tipo)
        {
            SqlDbType dbType = SqlDbType.VarChar;
            try
            {
                if (tipo.Contains("DECIMAL"))
                {
                    tipo = "DECIMAL";
                }

                switch (tipo)
                {
                    case "INT":
                        dbType = SqlDbType.Int;
                        break;

                    case "VARCHAR":
                        dbType = SqlDbType.VarChar;
                        break;

                    case "FLOAT":
                        dbType = SqlDbType.Float;
                        break;

                    case "CHAR":
                        dbType = SqlDbType.VarChar;
                        break;

                    case "DECIMAL":
                        dbType = SqlDbType.Decimal;
                        break;

                    case "DATE":
                        dbType = SqlDbType.Date;
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Se ha producido un error al abrir el JSON");
                log.Error("El error se debe a que falta un campo tipo en el JSON");
            }

            return dbType;
        }

        private static SqlParameter parseDatos(TableScheme item, SqlParameter idParam, string remplazarPunto)
        {
            if (idParam.Value != null)
            {
                if (!idParam.Value.Equals("NULL"))
                {
                    if (conversorTipoDatos(item.Tipo) == SqlDbType.Int)
                    {
                        if (!String.IsNullOrWhiteSpace(idParam.Value.ToString()))
                        {
                            idParam.SqlValue = Int32.Parse(idParam.Value.ToString());
                            idParam.Value = Int32.Parse(idParam.Value.ToString());
                        }
                    }
                    if (conversorTipoDatos(item.Tipo) == SqlDbType.Float)
                    {
                        idParam.Value = Double.Parse(remplazarPunto.Replace(".", ","));

                        idParam.SqlValue = Double.Parse(remplazarPunto.Replace(".", ","));
                    }
                    if (conversorTipoDatos(item.Tipo) == SqlDbType.VarChar)
                    {
                        if (idParam.Value.ToString().Substring(0, 1) == "'")
                        {
                            idParam.Value = idParam.Value.ToString().Remove(0, 1);
                        }
                        if (idParam.Value.ToString().Substring(idParam.Value.ToString().Length - 1, 1) == "'")
                        {
                            idParam.Value = idParam.Value.ToString().Remove(idParam.Value.ToString().Length - 1, 1);
                        }
                    }
                    if (conversorTipoDatos(item.Tipo) == SqlDbType.Decimal)
                    {
                        string comillas = idParam.Value.ToString().Replace("'", "");
                        if (!string.IsNullOrWhiteSpace(comillas))
                        {
                            idParam.Value = Decimal.Parse(comillas);
                            idParam.SqlValue = Decimal.Parse(comillas);
                        }
                        else
                        {
                            idParam.IsNullable = true;
                            idParam.Value = DBNull.Value;
                            idParam.SqlValue = DBNull.Value;
                        }
                    }
                }
                else
                {
                    idParam.IsNullable = true;
                    idParam.Value = DBNull.Value;
                }
            }
            return idParam;
        }

        public static string getQueryFromCommand(SqlCommand cmd)
        {
            string CommandTxt = cmd.CommandText;

            foreach (SqlParameter parms in cmd.Parameters)
            {
                string val = String.Empty;
                if (parms.DbType.Equals(DbType.String) || parms.DbType.Equals(DbType.DateTime))
                    val = "'" + Convert.ToString(parms.Value).Replace(@"\", @"\\").Replace("'", @"\'") + "'";
                if (parms.DbType.Equals(DbType.Int16) || parms.DbType.Equals(DbType.Int32) || parms.DbType.Equals(DbType.Int64) || parms.DbType.Equals(DbType.Decimal) || parms.DbType.Equals(DbType.Double))
                    val = Convert.ToString(parms.Value);
                string paramname = "@" + parms.ParameterName;
                CommandTxt = CommandTxt.Replace(paramname, val);
            }
            return (CommandTxt);
        }

        public static void quitarComillasFecha(SqlParameter idParam, JObject lineaEditada, TableScheme item)
        {
            if (idParam.SqlDbType == SqlDbType.Date)
            {
                string fech = lineaEditada.GetValue(item.Nombre).ToString();
                var fecha = Convert.ToDateTime(fech.Replace("'", ""));
                idParam.Value = fecha;
            }
            else
            {
                idParam.Value = lineaEditada.GetValue(item.Nombre).ToString();
            }
        }
    }
}
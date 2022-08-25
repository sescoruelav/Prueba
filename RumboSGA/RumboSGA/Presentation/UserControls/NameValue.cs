using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls
{
    public partial class NameValue : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables y Propiedades

        public TableLayoutPanel tableLayoutControl
        {
            get { return this.tableLayoutPanel1; }
        }

        public RadTextBox tb;
        protected DataGridView dt;
        protected RadMultiColumnComboBox combo;
        protected DateTimePicker dtPicker;
        protected RadTimePicker timePicker;
        protected TableScheme tableScheme;
        protected CheckBox rcb;
        protected string _valor;
        protected RadButtonTextBox colorBox;
        protected RadImageButtonElement rbe;
        protected DataTable dtCombo;
        public const string ErrorStringNameValue = "ERROR NAME VALUE GETVALUE";

        public string valorTextCombo;

        #endregion Variables y Propiedades

        #region Constructor

        public NameValue(TableScheme tableScheme, string valor)
        {
            try
            {
                InitializeComponent();
                this.tableScheme = tableScheme;
                this._valor = valor;
                if (string.IsNullOrEmpty(this.tableScheme.Etiqueta))
                {
                    this.radLabel1.Text = this.tableScheme.Nombre;
                }
                else
                {
                    this.radLabel1.Text = this.tableScheme.Etiqueta;
                }

                this.Name = this.tableScheme.Nombre + this.tableScheme.Etiqueta;

                switch (this.tableScheme.Control)//(_tipo)
                {
                    case "TB":
                        tb = new RadTextBox();

                        tb.Name = this.tableScheme.Nombre; //_titulo;

                        if (this.tableScheme.Ancho == 0)
                        {
                            tb.Dock = DockStyle.Fill;
                        }
                        else
                        {
                            tb.Width = (this.tableScheme.Ancho);//Ancho
                        }
                        if (this.tableScheme.Alto != 0)//(alto != 0)
                        {
                            string nombre = this.tableScheme.Nombre;

                            tb.Multiline = true;
                            tb.Height = this.tableScheme.Alto;//alto;

                            this.Height = this.tableScheme.Alto;
                            tb.MinimumSize = new System.Drawing.Size(0, this.tableScheme.Alto - 20);
                        }

                        SetValor(_valor);

                        if (this.tableScheme.NumeroColumnas != null && this.tableScheme.NumeroColumnas > 1)
                        {
                            multiplesColumnas(this, this.tableScheme);
                        }
                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));

                        tableLayoutPanel1.Controls.Add(tb, 1, 0);
                        break;

                    case "CBSN":
                        rcb = new CheckBox();
                        rcb.Name = this.tableScheme.Nombre; //_titulo;

                        SetValor(_valor);

                        if (this.tableScheme.NumeroColumnas != null && this.tableScheme.NumeroColumnas > 1)
                        {
                            multiplesColumnas(this, this.tableScheme);
                        }

                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                        tableLayoutPanel1.Controls.Add(rcb, 1, 0);
                        break;

                    case "CMB":
                        combo = new RadMultiColumnComboBox();
                        dtCombo = new DataTable();
                        string campoRelacionado = this.tableScheme.CmbObject.CampoRelacionado;
                        bool campoVisible = true;
                        if (this.tableScheme.CmbObject.EsVisible == false)
                        {
                            campoVisible = false;
                        }

                        if (this.tableScheme.CmbObject.CampoMostradoAux != null)
                        {
                            if (this.tableScheme.CmbObject.Filtro != null)
                                dtCombo = Business.GetDatosComboMulti(campoRelacionado, this.tableScheme.CmbObject.CampoMostrado + "," + this.tableScheme.CmbObject.CampoMostradoAux, this.tableScheme.CmbObject.TablaRelacionada, this.tableScheme.CmbObject.Filtro);
                            else
                                dtCombo = Business.GetDatosComboMulti(campoRelacionado, this.tableScheme.CmbObject.CampoMostrado + "," + this.tableScheme.CmbObject.CampoMostradoAux, this.tableScheme.CmbObject.TablaRelacionada);
                        }
                        else
                        {
                            if (this.tableScheme.CmbObject.Filtro != null)
                                dtCombo = Business.GetDatosComboMulti(campoRelacionado, this.tableScheme.CmbObject.CampoMostrado, this.tableScheme.CmbObject.TablaRelacionada, this.tableScheme.CmbObject.Filtro);
                            else
                                dtCombo = Business.GetDatosComboMulti(campoRelacionado, this.tableScheme.CmbObject.CampoMostrado, this.tableScheme.CmbObject.TablaRelacionada);
                        }

                        String selectedItem = "";

                        var test = "[" + this.tableScheme.CmbObject.CampoMostrado + "] = '" + _valor.Trim().Replace("'", "''") + "'";
                        string prueba = "[" + this.tableScheme.CmbObject.CampoRelacionado + "] = '" + _valor.Trim().Replace("'", "''") + "'";

                        DataRow[] resultados = null;
                        if (!_valor.Equals(String.Empty))
                        {
                            resultados = dtCombo.Select(prueba);
                        }

                        if (resultados != null && resultados.Length > 0 && !_valor.Equals(""))
                        {
                            selectedItem = resultados[0][campoRelacionado].ToString();
                        }
                        else
                        {
                            selectedItem = this.tableScheme.CmbObject.ValorDefecto;
                        }
                        valorTextCombo = selectedItem;
                        combo = new RadMultiColumnComboBox();
                        if (this.tableScheme.Ancho == 0)
                        {
                            combo.Dock = DockStyle.Fill;
                        }
                        else
                        {
                            combo.Width = this.tableScheme.Ancho;
                        }
                        //Filtrado y modo de autocompletado en los ComboBox con los datos únicos y relacionados, Hay 4 formas de realizar el autocompletado: Append, Suggest, SuggestAppend & None
                        Utilidades.RellenarMultiColumnComboBox(ref combo, dtCombo, campoRelacionado, this.tableScheme.CmbObject.CampoMostrado, selectedItem, new String[] { "TODOS" }, campoVisible);
                        //combo.AutoCompleteMode = AutoCompleteMode.None; //Jhon: Deshabilita la característica de finalización automática en los controles
                        //combo.AutoCompleteMode = AutoCompleteMode.Append; //Jhon: Anexa el resto de la cadena del candidato más probable a los caracteres existentes, y resalta los caracteres anexados.
                        combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend; //Jhon: Se aplica tanto a Suggest como a Append.
                        combo.AutoCompleteMode = AutoCompleteMode.Suggest; //Jhon: Muestra la lista desplegable auxiliar asociada al control de edición. Esta lista desplegable se rellena con una o más cadenas de finalización sugeridas.
                        combo.Name = this.tableScheme.Nombre;

                        if (this.tableScheme.NumeroColumnas != null && this.tableScheme.NumeroColumnas > 1)
                        {
                            multiplesColumnas(this, this.tableScheme);
                        }
                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                        tableLayoutPanel1.Controls.Add(combo, 1, 0);
                        break;

                    case "DATETIME":
                        dtPicker = new DateTimePicker();
                        dtPicker.Name = this.tableScheme.Nombre;
                        dtPicker.Format = DateTimePickerFormat.Custom;
                        string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        sysFormat += " HH:mm:ss";
                        dtPicker.CustomFormat = sysFormat;//"MM/dd/yyyy HH:mm:ss";
                        if (this.tableScheme.Ancho == 0)
                        {
                            dtPicker.Dock = DockStyle.Fill;
                        }
                        else
                        {
                            dtPicker.Width = this.tableScheme.Ancho;
                        }
                        SetValor(_valor);

                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                        tableLayoutPanel1.Controls.Add(dtPicker, 1, 0);
                        break;

                    case "DATE":
                        dtPicker = new DateTimePicker();
                        dtPicker.Name = this.tableScheme.Nombre;
                        dtPicker.Format = DateTimePickerFormat.Custom;
                        if (this.tableScheme.Ancho == 0)
                        {
                            dtPicker.Dock = DockStyle.Fill;
                        }
                        else
                        {
                            dtPicker.Width = this.tableScheme.Ancho;
                        }
                        SetValor(_valor);

                        if (this.tableScheme.NumeroColumnas > 1)
                        {
                            multiplesColumnas(this, this.tableScheme);
                        }

                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                        tableLayoutPanel1.Controls.Add(dtPicker, 1, 0);

                        break;

                    case "COL":
                        //Color
                        colorBox = new RadButtonTextBox();
                        colorBox.Name = this.tableScheme.Nombre;
                        if (this.tableScheme.Ancho == 0) colorBox.Dock = DockStyle.Fill;
                        else colorBox.Width = this.tableScheme.Ancho;
                        SetValor(_valor);

                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                        tableLayoutPanel1.Controls.Add(colorBox, 1, 0);
                        break;

                    case "TIME":
                        timePicker = new RadTimePicker();
                        timePicker.Name = this.tableScheme.Nombre;

                        SetValor(_valor);
                        if (this.tableScheme.Ancho == 0)
                            timePicker.Dock = DockStyle.Fill;
                        else
                            timePicker.Width = this.tableScheme.Ancho;

                        if (this.tableScheme.NumeroColumnas > 1)
                        {
                            multiplesColumnas(this, this.tableScheme);
                        }
                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(1, 0));
                        tableLayoutPanel1.Controls.Add(timePicker, 1, 0);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }

            #endregion Constructor
        }

        public void SetValor(String valor)
        {
            try
            {
                switch (tableScheme.Control)//(_tipo)
                {
                    case "TB":
                        //Hay veces que me está cargando en el textBox [''] en vez de vacío.

                        tb.Text = valor;

                        break;

                    case "CBSN":
                        if (valor.Equals("S"))
                        {
                            rcb.Checked = true;
                        }
                        else
                        {
                            rcb.Checked = false;
                        }
                        break;

                    case "CMB":
                        String selectedItem = "";

                        DataRow[] resultados = dtCombo.Select("[" + Lenguaje.traduce(tableScheme.CmbObject.CampoMostrado) + "] = '" + valor.Trim().Replace("'", "''") + "'");
                        if (resultados.Length > 0 && !valor.Equals(""))
                        {
                            selectedItem = resultados[0][Lenguaje.traduce(tableScheme.CmbObject.CampoRelacionado)].ToString();
                        }
                        else
                        {
                            selectedItem = tableScheme.CmbObject.ValorDefecto;
                        }
                        Utilidades.CargarValorComboBox(ref combo, dtCombo, selectedItem, combo.ValueMember, tableScheme.CmbObject.CampoMostrado);

                        break;

                    case "DATETIME": //Es el control de tipo fecha y hora
                        if (string.IsNullOrEmpty(valor))
                        {
                            dtPicker.Value = DateTime.Now;
                        }
                        else
                        {
                            try
                            {
                                //César: Se añade el tryparse por compatibilidad con el RumControl ya que el VirtualGrid
                                //saca la fecha con el cultureInfo del sistema.
                                DateTime Fecha = new DateTime();
                                if (!DateTime.TryParse(valor, out Fecha))
                                {
                                    string strFecha = valor.Split(' ')[0];
                                    string dia = strFecha.Split('/')[1];
                                    string mes = strFecha.Split('/')[0];
                                    string anyo = strFecha.Split('/')[2];

                                    string tiempo = valor.Split(' ')[1];
                                    string hora = tiempo.Split(':')[0];
                                    string minutos = tiempo.Split(':')[1];
                                    string segundos = tiempo.Split(':')[2];
                                    Fecha = new DateTime(int.Parse(anyo), int.Parse(mes), int.Parse(dia),
                                    int.Parse(hora), int.Parse(minutos), int.Parse(segundos));
                                }
                                dtPicker.Value = Fecha;
                            }
                            catch
                            {
                            }
                        }
                        break;

                    case "DATE": //Es el control de tipo fecha
                        if (string.IsNullOrEmpty(valor))
                        {
                            dtPicker.Value = DateTime.Now;
                        }
                        else
                        {
                            try
                            {
                                DateTime Fecha = new DateTime();
                                if (!DateTime.TryParse(valor, out Fecha))
                                {
                                    string strFecha = valor.Split(' ')[0];
                                    string dia = strFecha.Split('/')[1];
                                    string mes = strFecha.Split('/')[0];
                                    string anyo = strFecha.Split('/')[2];
                                    Fecha = new DateTime(int.Parse(anyo), int.Parse(mes), int.Parse(dia));
                                }
                                dtPicker.Value = Fecha;
                            }
                            catch
                            {
                            }
                        }
                        break;

                    case "TIME": //Es el control de tipo hora
                        if (string.IsNullOrEmpty(valor))
                            timePicker.Value = DateTime.Now;
                        else
                        {
                            try
                            {
                                string strHora = valor.Split(' ')[1];
                                string horas = strHora.Split(':')[0];
                                string minutos = strHora.Split(':')[1];
                                string segundos = strHora.Split(':')[2];

                                //dtPicker.Value = Convert.ToDateTime(valor);
                                DateTime Hora = new DateTime(1970, 1, 1, int.Parse(horas), int.Parse(minutos), int.Parse(segundos));
                                //DateTime.TryParse(valor, out Fecha);
                                timePicker.Value = Hora;
                            }
                            catch
                            {
                            }
                        }
                        break;

                    case "COL":
                        colorBox.Text = valor;
                        if (valor.Equals(""))
                        {
                            valor = "0";
                        }
                        colorBox.ForeColor = System.Drawing.Color.FromArgb(int.Parse(valor));
                        colorBox.BackColor = System.Drawing.Color.FromArgb(int.Parse(valor));
                        colorBox.AutoSize = false;

                        rbe = new RadImageButtonElement();
                        rbe.TextElement.CustomFont = "TelerikWebUI";
                        rbe.TextElement.CustomFontSize = 12;
                        rbe.TextElement.TextRenderingHint = TextRenderingHint.AntiAlias;
                        rbe.TextImageRelation = TextImageRelation.ImageBeforeText;
                        rbe.DisplayStyle = Telerik.WinControls.DisplayStyle.Image;
                        rbe.Image = Properties.Resources.colorButton;
                        rbe.Padding = new Padding(1, 1, 1, 1);
                        rbe.Click += (sender, args) =>
                        {
                            Telerik.WinControls.RadColorDialog dialog = new Telerik.WinControls.RadColorDialog();
                            ((RadForm)dialog.ColorDialogForm).ThemeName = "CAMBIO COLOR";
                            dialog.SelectedColor = System.Drawing.Color.FromArgb(int.Parse(valor));
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                valor = dialog.SelectedColor.ToArgb().ToString();
                                colorBox.ForeColor = System.Drawing.Color.FromArgb(int.Parse(valor));
                                colorBox.BackColor = System.Drawing.Color.FromArgb(int.Parse(valor));
                                colorBox.Text = valor;
                            }
                        };
                        colorBox.RightButtonItems.Clear();
                        colorBox.RightButtonItems.Add(rbe);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al cargar valor en NameValue");
            }
        }

        public void AutoCalculoArticulos()
        {
            try
            {
                int heightN = 0;
                double widthN = 0;

                this.radLabel1.AutoSize = true;
                this.radLabel1.LoadElementTree();
                if (this.tableLayoutPanel1.Controls[0].Height > this.tableLayoutPanel1.Controls[1].Height)
                {
                    heightN = this.tableLayoutPanel1.Controls[0].Height;
                }
                else
                {
                    heightN = this.tableLayoutPanel1.Controls[1].Height;
                }

                widthN = this.tableLayoutPanel1.Controls[0].Width + this.tableLayoutPanel1.Controls[1].Width;

                //Como despues la repartición es 40-60 para no dejar al label sin espacio le sumo un 20% para compensar
                widthN += widthN * 0.20;
                while (widthN * (tableLayoutPanel1.ColumnStyles[0].Width * 0.01) < this.tableLayoutPanel1.Controls[0].Width)
                {
                    widthN += 5;
                }
                while (widthN * (tableLayoutPanel1.ColumnStyles[1].Width * 0.01) < this.tableLayoutPanel1.Controls[1].Width)
                {
                    widthN += 5;
                }
                this.Width = Convert.ToInt32(Math.Round(widthN, MidpointRounding.AwayFromZero));
                this.Height = heightN;
                this.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            }
            catch (Exception ex)
            {
                log.Error("Error calculando automaticamente el tamaño del control (autoCalculoArticulos)" + ex.Message);
            }
        }

        public void Editable(bool editable)
        {
            tableLayoutPanel1.GetControlFromPosition(1, 0).Enabled = editable;
        }

        public System.Windows.Forms.TableLayoutPanel getTableLayoutPanel1()
        {
            return tableLayoutPanel1;
        }

        //Josep
        private void multiplesColumnas(NameValue par, TableScheme _tableScheme)
        {
            this.tableLayoutPanel1.AutoSize = false;
            //Se eliminan los estilos para poder añadir los nuevos estilos
            //con el tamaño que se les determina dependiendo de la cantidad de columnas que se requiere;
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(0);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(0);
            //Se divide el 40 porque es la proporción que tiene el estilo original y asi mantener las dimensiones
            float percentSize = (40 / _tableScheme.NumeroColumnas);
            //Se añaden los nuevos estilos
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, percentSize));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, (90 - percentSize)));
            tableLayoutControl.SetColumnSpan(this, 2);
            par.Width = (_tableScheme.Ancho * _tableScheme.NumeroColumnas);
            Width = (_tableScheme.Ancho * _tableScheme.NumeroColumnas);
        }

        public string getValor(ref string error)
        {
            string _valor = null;
            try
            {
                switch (tableScheme.Control)
                {
                    case "TB":
                        _valor = tb.Text;
                        break;

                    case "CMB":
                        if (combo.SelectedValue == null)
                        {
                            _valor = "";
                        }
                        else
                        {
                            _valor = combo.SelectedValue.ToString();
                        }

                        break;

                    case "DATE":
                    case "DATETIME":

                        _valor = dtPicker.Value.ToString("s");
                        if (DateTime.Compare(dtPicker.Value, new DateTime(1980, 1, 1)) == 0)
                        {
                            _valor = "NULL";
                        }
                        break;

                    case "TIME":
                        _valor = timePicker.Value.ToString();

                        break;

                    case "COL":
                        _valor = colorBox.Text.ToString();
                        break;

                    case "CBSN":
                        if (rcb.Checked)
                        {
                            _valor = "S";
                        }
                        else
                        {
                            _valor = "N";
                        }
                        break;

                    default:
                        _valor = tb.Text;
                        break;
                }

                if (tableScheme.PuedeNull && _valor.Equals(""))
                {
                    _valor = "NULL";
                }
                if (!tableScheme.PuedeVacio && _valor.Equals(""))
                {
                    error += Lenguaje.traduce("La columna " + tableScheme.Etiqueta + " no puede ser vacío\n");
                    return ErrorStringNameValue;
                }

                if (tableScheme.Tipo.Equals("INT"))
                {
                    _valor = _valor.Replace(",", ".");
                    if (!_valor.Equals("") && !_valor.Contains("NULL") && !int.TryParse(_valor, out int r))
                    {
                        error += Lenguaje.traduce("La columna: " + tableScheme.Etiqueta + " no es un numero primario\n");
                        return ErrorStringNameValue;
                    }
                }
                else if (tableScheme.Tipo.Contains("DECIMAL") || tableScheme.Tipo.Contains("FLOAT"))
                {
                    if (_valor.Contains("E"))
                    {
                        double valorlocal = Double.Parse(_valor);
                        decimal valordec = Convert.ToDecimal(valorlocal);
                        _valor = valordec.ToString();
                        //
                    }
                    else
                    {
                        _valor = _valor.Replace(",", ".");
                    }

                    if (!_valor.Equals("") && !_valor.Contains("NULL") && !Decimal.TryParse(_valor, out decimal r))
                    {
                        error += Lenguaje.traduce("La columna: " + tableScheme.Etiqueta + " no es un numero decimal.\n");
                        return ErrorStringNameValue;
                    }
                }
                else if (!_valor.Equals("NULL"))
                {
                    _valor = "'" + _valor + "'";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }

            return _valor;
        }

        public TableScheme getTableScheme()
        {
            return tableScheme;
        }

        public static void SacarLinea(ref JObject jsonObject, ref string error, List<NameValue> nameValueList)
        {
            try
            {
                foreach (NameValue item in nameValueList)
                {
                    string valor = item.getValor(ref error);
                    if (valor != null)
                        jsonObject.Add(item.tableScheme.Nombre, valor);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        public static void SacarValorALinea(ref JObject jsonObject, NameValue nv)
        {
            if (nv != null)
            {
                string _valor = null;

                switch (nv.tableScheme.Control)
                {
                    case "TB":
                        _valor = nv.tb.Text;
                        _valor = _valor.Replace("'", "''");
                        switch (nv.tableScheme.Tipo)
                        {
                            case "INT":
                                if (_valor.Equals(""))
                                {
                                    _valor = "NULL";
                                }
                                else if (_valor.Contains(","))
                                {
                                    _valor = _valor.Replace(",", ".");
                                }
                                break;

                            case "DECIMAL":
                            case "FLOAT":
                                if (String.IsNullOrEmpty(nv.tb.Text))
                                {
                                    _valor = "0";
                                }
                                if (_valor.Contains("E"))
                                {
                                    double valorlocal = Double.Parse(_valor);
                                    decimal valordec = Convert.ToDecimal(valorlocal);
                                    _valor = valordec.ToString();
                                    //
                                }
                                else
                                {
                                    _valor = _valor.Replace(",", ".");
                                }
                                break;

                            default:
                                if (false && _valor.Equals(""))
                                {
                                    _valor = "NULL";
                                }
                                else
                                {
                                    _valor = "'" + _valor + "'";
                                }
                                break;
                        }

                        break;

                    case "CMB":

                        if (nv.combo.SelectedItem != null)
                        {
                            if (nv.combo.Text.Equals("") || nv.combo.Text.Equals("NULL"))
                            {
                                if (nv.tableScheme.CmbObject.ValorDefecto.Equals(""))
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, "");
                                }
                                else if (nv.tableScheme.CmbObject.ValorDefecto.Equals("NULL"))
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, "NULL");
                                }
                                else
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, nv.tableScheme.CmbObject.ValorDefecto);
                                }
                            }
                            else
                            {
                                _valor = nv.combo.SelectedValue.ToString();
                                switch (nv.tableScheme.Tipo)
                                {
                                    case "INT":
                                        break;

                                    default:
                                        _valor = "'" + _valor + "'";
                                        break;
                                }
                                if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, _valor);
                                }
                            }
                        }
                        break;

                    case "DATE":
                    case "DATETIME":

                        _valor = nv.dtPicker.Value.ToString("s");
                        if (DateTime.Compare(nv.dtPicker.Value, new DateTime(1980, 1, 1)) == 0)
                        {
                            _valor = "NULL";
                        }
                        else
                        {
                            switch (nv.tableScheme.Tipo)
                            {
                                case "INT":
                                    break;

                                default:
                                    _valor = "'" + _valor + "'";
                                    break;
                            }
                        }
                        if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                        {
                            jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        }
                        break;

                    case "TIME":

                        _valor = nv.timePicker.Value.ToString();
                        switch (nv.tableScheme.Tipo)
                        {
                            case "INT":
                                break;

                            default:
                                _valor = "'" + _valor + "'";
                                break;
                        }
                        jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        break;

                    case "COL":

                        _valor = nv.colorBox.Text.ToString();
                        switch (nv.tableScheme.Tipo)
                        {
                            case "INT":
                                break;

                            default:
                                _valor = "'" + _valor + "'";
                                break;
                        }
                        if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                        {
                            jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        }
                        break;

                    case "CBSN":

                        if (nv.rcb.Checked)
                        {
                            _valor = "'S'";
                        }
                        else
                        {
                            _valor = "'N'";
                        }
                        if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                        {
                            jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        }
                        break;
                }
                if (!jsonObject.ContainsKey(nv.tableScheme.Nombre) && _valor != null)
                {
                    jsonObject.Add(nv.tableScheme.Nombre, _valor);
                }
            }
        }

        public static void SacarValorALineaOnDelete(ref JObject jsonObject, NameValue nv)
        {
            if (nv != null)
            {
                string _valor = null;

                switch (nv.tableScheme.Control)
                {
                    case "TB":
                        _valor = nv.tb.Text;
                        _valor = _valor.Replace("'", "''");
                        switch (nv.tableScheme.Tipo)
                        {
                            case "INT":
                                if (_valor.Equals(""))
                                {
                                    _valor = "NULL";
                                }
                                else if (_valor.Contains(","))
                                {
                                    _valor = _valor.Replace(",", ".");
                                }
                                break;

                            case "DECIMAL":
                            case "FLOAT":
                                if (String.IsNullOrEmpty(nv.tb.Text))
                                {
                                    _valor = "0";
                                }
                                if (_valor.Contains("E"))
                                {
                                    double valorlocal = Double.Parse(_valor);
                                    decimal valordec = Convert.ToDecimal(valorlocal);
                                    _valor = valordec.ToString();
                                    //
                                }
                                else
                                {
                                    _valor = _valor.Replace(",", ".");
                                }
                                break;

                            default:
                                if (false && _valor.Equals(""))
                                {
                                    _valor = "NULL";
                                }
                                else
                                {
                                    _valor = "'" + _valor + "'";
                                }
                                break;
                        }

                        break;

                    case "CMB":

                        if (nv.combo.SelectedItem != null)
                        {
                            if (nv.combo.Text.Equals("") || nv.combo.Text.Equals("NULL"))
                            {
                                if (nv.tableScheme.CmbObject.ValorDefecto.Equals(""))
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, "");
                                }
                                else if (nv.tableScheme.CmbObject.ValorDefecto.Equals("NULL"))
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, "NULL");
                                }
                                else
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, nv.tableScheme.CmbObject.ValorDefecto);
                                }
                            }
                            else
                            {
                                _valor = nv.combo.SelectedValue.ToString();
                                switch (nv.tableScheme.Tipo)
                                {
                                    case "INT":
                                        break;

                                    default:
                                        _valor = "'" + _valor + "'";
                                        break;
                                }
                                if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                                {
                                    jsonObject.Add(nv.tableScheme.Nombre, _valor);
                                }
                            }
                        }
                        break;

                    case "DATE":
                    case "DATETIME":

                        _valor = nv.dtPicker.Value.ToString("s");
                        if (DateTime.Compare(nv.dtPicker.Value, new DateTime(1980, 1, 1)) == 0)
                        {
                            _valor = "NULL";
                        }
                        else
                        {
                            switch (nv.tableScheme.Tipo)
                            {
                                case "INT":
                                    break;

                                default:
                                    _valor = "'" + _valor + "'";
                                    break;
                            }
                        }
                        if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                        {
                            jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        }
                        break;

                    case "TIME":

                        _valor = nv.timePicker.Value.ToString();
                        switch (nv.tableScheme.Tipo)
                        {
                            case "INT":
                                break;

                            default:
                                _valor = "'" + _valor + "'";
                                break;
                        }
                        jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        break;

                    case "COL":

                        _valor = nv.colorBox.Text.ToString();
                        switch (nv.tableScheme.Tipo)
                        {
                            case "INT":
                                break;

                            default:
                                _valor = "'" + _valor + "'";
                                break;
                        }
                        if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                        {
                            jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        }
                        break;

                    case "CBSN":

                        if (nv.rcb.Checked)
                        {
                            _valor = "'S'";
                        }
                        else
                        {
                            _valor = "'N'";
                        }
                        if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                        {
                            jsonObject.Add(nv.tableScheme.Nombre, _valor);
                        }
                        break;
                }
                if (!jsonObject.ContainsKey(nv.tableScheme.Nombre))
                {
                    jsonObject.Add(nv.tableScheme.Nombre, _valor);
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
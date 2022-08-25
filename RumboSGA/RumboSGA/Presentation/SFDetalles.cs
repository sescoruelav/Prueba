using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation
{
    public partial class SFDetalles : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables, Propiedades y Enumeradores

        protected BaseGridControl baseGridControl;
        protected TableLayoutPanel TablaPanelContenido;
        protected DataGridView dt;

        public enum modoForm
        {
            lectura,
            nuevo,
            edicion,
            clonar
        }

        public dynamic newRecord;
        public bool editado;
        public int filasGrid = 0;
        public bool ComprobarValores = false;

        /*public static modoForm ModoAperturaForm { get { return _modoAperturaForm; } set { _modoAperturaForm = value; } }
        private static modoForm _modoAperturaForm;*/

        public modoForm ModoAperturaForm
        { get { return _modoAperturaForm; } set { _modoAperturaForm = value; } }

        private modoForm _modoAperturaForm;

        //private List<TableScheme> _lstEsquemasForm;

        protected int _filasTotalesTabla = 0;

        private const float K_RATIO_AMPLIAR_VENTANA = 0.5F;

        //_lstEsquemas
        protected List<TableScheme> _lstEsquemas;

        protected List<NameValue> lstNameValues;

        //_values
        protected dynamic _values;

        //_modoApertura
        //modoForm ModoAperturaForm;
        //_esquemGrid
        protected GridScheme _esquemGrid;

        //_diccParamNuevo
        protected Hashtable _diccParamNuevo;

        //RadWaitingBar radWaitingBar1;
        protected Panel panWaiting = new Panel();

        protected RadWaitingBar radWait = new RadWaitingBar();
        public string registro;

        #endregion Variables, Propiedades y Enumeradores

        #region Constructor

        public SFDetalles(string nombre, List<TableScheme> _lstEsquemasParam, dynamic _valuesParam, modoForm _modoAperturaParam, GridScheme _esquemGridParam = null, Hashtable _diccParamNuevoParam = null)
        {
            InitializeComponent();
            this.dataDialog1.tituloForm.Text = Lenguaje.traduce(nombre);
            _lstEsquemas = _lstEsquemasParam;
            _values = _valuesParam;
            //_modoApertura = _modoAperturaParam;
            _esquemGrid = _esquemGridParam;
            _diccParamNuevo = _diccParamNuevoParam;
            ModoAperturaForm = _modoAperturaParam;
            this.WindowState = FormWindowState.Maximized;
            if (_values != null)
            {
                registro = JsonConvert.DeserializeObject(Convert.ToString(_values)).ToString(Formatting.None);
            }

            /*if (_esquemGrid != null)
            {
                float anchoNuevo = (this.Size.Width * K_RATIO_AMPLIAR_VENTANA) + this.Size.Width;
                float altoNuevo = (this.Size.Height * K_RATIO_AMPLIAR_VENTANA) + this.Size.Height;

                this.Size = new System.Drawing.Size(Convert.ToInt32(anchoNuevo), Convert.ToInt32(altoNuevo));
            }*/

            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;

            //Eventos
            radWait = this.radWaitingBar1;

            panWaiting.Dock = DockStyle.Fill;
            radWait.Dock = DockStyle.Bottom;
            this.panWaiting.Controls.Add(radWait);
            this.Controls.Add(panWaiting);
            panWaiting.BringToFront();

            myBackgroundWorker = new BackgroundWorker();
            myBackgroundWorker.WorkerReportsProgress = true;
            myBackgroundWorker.WorkerSupportsCancellation = true;
            myBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myBackgroundWorker1_RunWorkerCompleted);
            myBackgroundWorker.DoWork += new DoWorkEventHandler(myBackgroundWorker1_DoWork);
            //this.LanzarProcesoAsincrono();

            this.Load += SFDetalles_Load;
            this.FormClosed += SFDetalles_FormClosed;
        }

        private void LanzarProcesoAsincrono()
        {
            if (!myBackgroundWorker.IsBusy)
            {
                myBackgroundWorker.RunWorkerAsync();
                this.radWaitingBar1.StartWaiting();
                this.radWait.StartWaiting();
            }
        }

        private void myBackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker = sender as BackgroundWorker;
            CargarDatos();
        }

        private void myBackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backGroundWorkerTerminado();
            //this.tableLayoutPanel1_Detalle.Controls[0].AutoScroll = true;
        }

        private void backGroundWorkerTerminado()
        {
            this.radWaitingBar1.StopWaiting();
            this.radWaitingBar1.ResetWaiting();

            this.radWait.StopWaiting();
            this.radWait.ResetWaiting();

            panWaiting.Visible = false;

            this.tableLayoutPanel1_Detalle.AutoScroll = true;
        }

        private void SFDetalles_Load(object sender, EventArgs e)
        {
            this.LanzarProcesoAsincrono();
        }

        #endregion Constructor

        #region Eventos

        private void SFDetalles_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Al cerrar, debemos devolver, al formulario que ha llamado, un JSON con los datos modificados
            newRecord = ComponerJSON();

            if (ModoAperturaForm == modoForm.edicion)
            {
                editado = true;
            }
        }

        #endregion Eventos

        #region Funciones Auxiliares

        protected virtual void CargarDatos()
        {
            NameValue CRUDCntrl;
            lstNameValues = new List<NameValue>();

            int filas = 0;
            int columnas = 0;
            TamañoTabla(ref filas, ref columnas, _lstEsquemas);

            TablaPanelContenido = new TableLayoutPanel();
            TablaPanelContenido.ColumnCount = columnas;

            //int AnchoTotal = this.dataDialog1.PanelControles.Width;
            int AnchoTotal = this.Width;

            for (int i = 0; i < columnas; i++)
            {
                TablaPanelContenido.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, AnchoTotal / columnas));
            }

            int rowPosition = 0;
            int altoFilaDefecto = 30;
            int altoTotal = 0;
            bool bolNuevaFila = false;
            List<RowStyle> arrayFilas = new List<RowStyle>();
            TablaPanelContenido.RowCount += filas;
            if (filas > 0)
            {
                //rowPosition = tablaPanelControles.RowCount;
                bolNuevaFila = true;
                for (int a = 0; a < filas; a++)
                {
                    TablaPanelContenido.RowCount += 1;
                    float mayorAlto = GetMayorAltoFila(_lstEsquemas, a);
                    //tablaPanelControles.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

                    arrayFilas.Add(new RowStyle(SizeType.AutoSize, mayorAlto));
                }

                for (int i = 0; i < _lstEsquemas.Count; i++)
                {
                    if (_lstEsquemas[i].Fila > rowPosition)
                    {
                        bolNuevaFila = true;
                    }

                    if (bolNuevaFila)
                    {
                        for (int j = rowPosition; j < filas /*_lstEsquemas[i].Fila*/; j++)
                        {
                            int mayorAlto = GetMayorAltoFila(_lstEsquemas, rowPosition);
                            if (arrayFilas[rowPosition].Height != 0)
                            {
                                TablaPanelContenido.RowStyles.Add(arrayFilas[j]);

                                altoTotal += (int)arrayFilas[rowPosition].Height;
                                rowPosition += 1;
                            }
                            else
                            {
                                TablaPanelContenido.RowStyles.Add(new RowStyle(SizeType.Absolute, altoFilaDefecto));
                                altoTotal += altoFilaDefecto;
                                rowPosition += 1;
                            }

                            bolNuevaFila = false;
                        }
                    }

                    string valor = string.Empty;
                    string valorAux = string.Empty;

                    try
                    {
                        if (_values != null)
                        {
                            var z = JsonConvert.DeserializeObject(Convert.ToString(_values));
                            if (_lstEsquemas[i].Tipo == "DATETIME")
                            {
                                var pn = (DateTime)z[_lstEsquemas[i].Nombre];
                                string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                sysFormat += " HH:mm:ss";
                                if (pn != null) //César: causa un error throwable
                                    valor = pn.ToString(sysFormat, CultureInfo.CurrentCulture);
                            }
                            else
                            {
                                var pn = (String)z[_lstEsquemas[i].Nombre];

                                if (_lstEsquemas[i].Control == "CMB")
                                {
                                    pn = (String)z[_lstEsquemas[i].CmbObject.CampoRelacionado];
                                }

                                if (pn != null) //César: causa un error throwable
                                    valor = pn.ToString();
                            }
                        }
                    }
                    catch (System.NullReferenceException ex)
                    {
                    }
                    catch (Exception e)
                    {
                        ExceptionManager.GestionarError(e);
                        log.Error("Ha habido un error al deserializar el json en el campo " + _lstEsquemas[i].Nombre);
                    }

                    switch (ModoAperturaForm)
                    {
                        case modoForm.clonar:
                            if (_lstEsquemas[i].EsPK)
                            {
                                //valor = string.Empty;
                            }
                            if (_diccParamNuevo.Count > 0 && _diccParamNuevo.ContainsKey(_lstEsquemas[i].Nombre))
                            {
                                if (_diccParamNuevo[_lstEsquemas[i].Nombre].GetType() == typeof(Hashtable))
                                {
                                    Hashtable diccAux = (Hashtable)_diccParamNuevo[_lstEsquemas[i].Nombre];

                                    int _valorMayor = Business.GetMayorElemento(diccAux["CampoMostrado"].ToString(), diccAux["TablaRelacionada"].ToString(), diccAux["filtro"].ToString());

                                    if (!string.IsNullOrEmpty(diccAux["ValorDefecto"].ToString()))
                                    {
                                        _valorMayor += Convert.ToInt32(diccAux["ValorDefecto"].ToString());
                                    }

                                    valor = _valorMayor.ToString();
                                }
                                else
                                {
                                    valor = _diccParamNuevo[_lstEsquemas[i].Nombre].ToString();
                                }
                            }
                            break;

                        case modoForm.nuevo:

                            valor = string.Empty;

                            if (!string.IsNullOrEmpty(_lstEsquemas[i].ValorDefecto))
                            {
                                valor = _lstEsquemas[i].ValorDefecto;
                            }
                            if (_diccParamNuevo.Count > 0 && _diccParamNuevo.ContainsKey(_lstEsquemas[i].Nombre))
                            {
                                //valor = _diccParamNuevo[_lstEsquemas[i].Nombre].ToString();
                                if (_diccParamNuevo[_lstEsquemas[i].Nombre].GetType() == typeof(Hashtable))
                                {
                                    Hashtable diccAux = (Hashtable)_diccParamNuevo[_lstEsquemas[i].Nombre];

                                    int _valorMayor = Business.GetMayorElemento(diccAux["CampoMostrado"].ToString(), diccAux["TablaRelacionada"].ToString(), diccAux["filtro"].ToString());

                                    if (!string.IsNullOrEmpty(diccAux["ValorDefecto"].ToString()))
                                    {
                                        _valorMayor += Convert.ToInt32(diccAux["ValorDefecto"].ToString());
                                    }

                                    valor = _valorMayor.ToString();
                                }
                                else
                                {
                                    valor = _diccParamNuevo[_lstEsquemas[i].Nombre].ToString();
                                }
                            }
                            break;
                    }

                    CRUDCntrl = new NameValue(_lstEsquemas[i], valor);

                    if (_esquemGrid != null)
                    {
                        if (_esquemGrid.Filtro == _lstEsquemas[i].Nombre)
                        {
                            _esquemGrid.ValorFiltro = valor;
                        }
                        //César: _esquemGrid controla el GridAdjunto en parte
                        _esquemGrid.SetValorClave(_lstEsquemas[i].Nombre, valor);
                    }

                    TablaPanelContenido.Controls.Add(CRUDCntrl, _lstEsquemas[i].Columna, _lstEsquemas[i].Fila);
                    lstNameValues.Add(CRUDCntrl);
                }

                _filasTotalesTabla = TablaPanelContenido.RowCount; //Guardar número de filas para la devolución del JSON y para la Edición
                                                                   //tablaPanelControles.RowCount = tableLayoutPanel1.RowStyles.Count;

                TablaPanelContenido.Size = new System.Drawing.Size(this.Width - SystemInformation.VerticalScrollBarWidth - 30, /*this.ClientSize.Height*/ altoTotal);
                TablaPanelContenido.AutoSize = true;
                this.dataDialog1.PanelControles.AutoScroll = true;
                if (this.dataDialog1.PanelControles.AutoScrollMargin.Width < 5 ||
                    this.dataDialog1.PanelControles.AutoScrollMargin.Height < 5)
                {
                    this.dataDialog1.PanelControles.Invoke((MethodInvoker)delegate { this.dataDialog1.PanelControles.SetAutoScrollMargin(5, 5); });
                }

                if (this.dataDialog1.PanelControles.InvokeRequired)
                {
                    this.dataDialog1.PanelControles.Invoke((MethodInvoker)delegate
                    {
                        this.dataDialog1.PanelControles.Controls.Add(TablaPanelContenido);
                    });
                }
                else
                {
                    this.dataDialog1.PanelControles.Controls.Add(TablaPanelContenido);
                }

                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate { HabilitarDeshabilitarControles(ModoAperturaForm); });
                }
                else
                {
                    HabilitarDeshabilitarControles(ModoAperturaForm);
                }
            }

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { CargaDatosGridAdjunto(); });
            }
            else
            {
                CargaDatosGridAdjunto();
            }
            //Probar cambiar de estilo aquí
        }

        protected void CargaDatosGridAdjunto()
        {
            tableLayoutPanel1_Detalle.RowStyles[1] = new RowStyle(SizeType.Percent, 0F);

            if (_esquemGrid != null)
            {
                string filtro = "";
                Hashtable diccParamNuevo = new Hashtable();

                if (!string.IsNullOrEmpty(_esquemGrid.Filtro))
                {
                    if (_esquemGrid.FiltroEnJsonLinea != null)
                    {
                        //Así se hace con el RumControlGeneral
                        filtro = " " + _esquemGrid.FiltroEnJsonLinea + " = '" + _esquemGrid.ValorFiltro + "'";
                    }
                    else
                        filtro = " WHERE " + _esquemGrid.Filtro + " = '" + _esquemGrid.ValorFiltro + "'";
                }

                for (int i = 0; i < _esquemGrid.Claves.Count; i++)
                {
                    if (_esquemGrid.Claves[i].ValorTabla != null && _esquemGrid.Claves[i].ValorTabla.CampoMostrado != null && _esquemGrid.Claves[i].ValorTabla.TablaRelacionada != null)
                    {
                        Hashtable diccValorTabla = new Hashtable();
                        diccValorTabla.Add("CampoMostrado", _esquemGrid.Claves[i].ValorTabla.CampoMostrado);
                        diccValorTabla.Add("TablaRelacionada", _esquemGrid.Claves[i].ValorTabla.TablaRelacionada);
                        diccValorTabla.Add("filtro", filtro);
                        diccValorTabla.Add("ValorDefecto", _esquemGrid.Claves[i].ValorTabla.ValorDefecto);

                        diccParamNuevo.Add(_esquemGrid.Claves[i].Nombre, diccValorTabla);
                    }
                    else
                    {
                        diccParamNuevo.Add(_esquemGrid.Claves[i].Nombre, _esquemGrid.Claves[i].Valor);
                    }
                }

                switch (_esquemGrid.ArchivoJSON)
                {
                    case "ProveedoresPedidosLineas":
                        AttachGridControl<ProveedoresPedidosLineasControl>("ProveedoresPedidosLineas", ref baseGridControl, filtro, diccParamNuevo);
                        break;

                    case "ClientesPedidosAnteriorLineas":
                        AttachGridControl<ClientesPedidosLineasControl>("ClientesPedidosLineas", ref baseGridControl, filtro, diccParamNuevo);
                        break;

                    case "ZonaLogicaLin1":
                        AttachGridControl<ZonaLogicaLinControl>("ZonaLogicaLin", ref baseGridControl, filtro, diccParamNuevo);
                        break;

                    default:
                        AttachGridRumControl(_esquemGrid.ArchivoJSON, _esquemGrid.ArchivoJSON, filtro, diccParamNuevo);
                        break;
                }
            }
        }

        public JObject getLineaValoresValidos(ref string error)
        {
            try
            {
                JObject resultado = new JObject();
                error = "";
                NameValue.SacarLinea(ref resultado, ref error, lstNameValues);
                return resultado;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        public void AttachGridControl<T>(string name, ref BaseGridControl ctrl, string filtro, Hashtable diccParamNuevo) where T : BaseGridControl, new()
        {
            if (ctrl == null)
            {
                ctrl = new T();
                ctrl.FiltroExterno = filtro; //" WHERE [IDPEDIDOPRO] = 2973";
                ctrl.diccParamNuevo = diccParamNuevo;
                ctrl.name = name;
                ctrl.Dock = DockStyle.Fill;
                ctrl.Margin = new Padding(0, 0, 7, 7);
                ctrl.virtualGridControl.AllowFiltering = false;
            }

            tableLayoutPanel1_Detalle.Controls.Remove(tableLayoutPanel1_Detalle.GetControlFromPosition(0, 1));
            tableLayoutPanel1_Detalle.Controls.Add(ctrl, 0, 1);

            //---------------------------------------
            tableLayoutPanel1_Detalle.RowStyles[0] = new RowStyle(SizeType.Percent, 50F);
            tableLayoutPanel1_Detalle.RowStyles[1] = new RowStyle(SizeType.Percent, 50F);
            //---------------------------------------
        }

        public void AttachGridRumControl(string name, string nombreJson, string filtro, Hashtable diccParamNuevo)
        {
            try
            {
                RumControlGeneralLineas ctrl = new RumControlGeneralLineas(name, nombreJson, filtro, diccParamNuevo);
                //ctrl.diccParamNuevo = diccParamNuevo;
                ctrl.Dock = DockStyle.Fill;
                ctrl.Margin = new Padding(0, 0, 7, 7);
                ctrl.virtualGridControl.AllowFiltering = false;

                tableLayoutPanel1_Detalle.Controls.Remove(tableLayoutPanel1_Detalle.GetControlFromPosition(0, 1));
                tableLayoutPanel1_Detalle.Controls.Add(ctrl, 0, 1);

                //---------------------------------------
                tableLayoutPanel1_Detalle.RowStyles[0] = new RowStyle(SizeType.Percent, 50F);
                tableLayoutPanel1_Detalle.RowStyles[1] = new RowStyle(SizeType.Percent, 50F);
                //---------------------------------------
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error al desplegarGridAdjunto nombreJson:" + nombreJson + " filtro:" + filtro + "");
            }
        }

        public void HabilitarDeshabilitarControles(modoForm _modoAp)
        {
            try
            {
                ModoAperturaForm = _modoAp;

                //botones
                this.dataDialog1.BotonAceptarControl.Visible = false;
                this.dataDialog1.BotonEditarControl.Visible = false;
                this.dataDialog1.BotonCancelarControl.Visible = false;
                this.dataDialog1.BotonCancelarControl.Text = "Cancelar";

                switch (_modoAp)
                {
                    case modoForm.lectura:
                        this.dataDialog1.BotonCancelarControl.Text = "Salir";
                        this.dataDialog1.BotonEditarControl.Visible = true;
                        this.dataDialog1.BotonCancelarControl.Visible = true;
                        break;

                    case modoForm.edicion:
                    case modoForm.nuevo:
                    case modoForm.clonar:
                        this.dataDialog1.BotonAceptarControl.Visible = true;
                        this.dataDialog1.BotonCancelarControl.Visible = true;
                        break;
                }

                //controles dinámicos
                if (this.dataDialog1.PanelControles.Controls[0] != null)
                {
                    //TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0];
                    TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0].Controls[0];

                    //for (int i = 0; i < tpanel.RowCount; i++)
                    for (int i = 0; i < _filasTotalesTabla + 1; i++)
                    {
                        for (int j = 0; j < tpanel.ColumnCount; j++)
                        {
                            NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);

                            if (par != null)
                            {
                                TableScheme esquemaBuscado;
                                switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                                {
                                    case "RadTextBox":
                                        RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(tb.Name);
                                        tb.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;

                                    case "RadMultiColumnComboBox":
                                        RadMultiColumnComboBox combo = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(combo.Name);
                                        if (esquemaBuscado.CmbObject.ValorDefecto != null)
                                        {
                                            int defectoInt;
                                            if (!String.IsNullOrEmpty(par.valorTextCombo))
                                            {
                                                if (int.TryParse(par.valorTextCombo, out defectoInt))
                                                {
                                                    combo.SelectedValue = defectoInt;
                                                }
                                                else
                                                {
                                                    combo.SelectedValue = par.valorTextCombo;
                                                }
                                            }
                                            else if (int.TryParse(esquemaBuscado.CmbObject.ValorDefecto, out defectoInt))
                                            {
                                                if (defectoInt > 0)
                                                {
                                                    combo.SelectedValue = esquemaBuscado.CmbObject.ValorDefecto;
                                                }
                                            }
                                            else if (!String.IsNullOrEmpty(esquemaBuscado.CmbObject.ValorDefecto))
                                            {
                                                combo.SelectedValue = esquemaBuscado.CmbObject.ValorDefecto;
                                            }
                                            //combo.Text = esquemaBuscado.CmbObject.ValorDefecto;
                                        }
                                        combo.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;

                                    case "DateTimePicker":
                                        DateTimePicker dtPicker = (DateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(dtPicker.Name);
                                        dtPicker.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;

                                    case "RadTimePicker":
                                        RadTimePicker rtPicker = (RadTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(rtPicker.Name);
                                        rtPicker.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;

                                    case "RadButtonTextBox":
                                        RadButtonTextBox rdColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(rdColor.Name);
                                        rdColor.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;

                                    case "RadCheckBox":
                                        RadCheckBox rdCheckBox = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(rdCheckBox.Name);
                                        rdCheckBox.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        protected int GetMayorAltoFila(List<TableScheme> _lstEsquemas, int numFila)
        {
            int alto = 0;

            for (int i = 0; i < _lstEsquemas.Count; i++)
            {
                if (_lstEsquemas[i].Alto > alto && _lstEsquemas[i].Fila == numFila)
                {
                    alto = _lstEsquemas[i].Alto;
                }
            }

            return alto;
        }

        protected void TamañoTabla(ref int filas, ref int col, List<TableScheme> _lstEsquemas)
        {
            filas = 0;
            col = 0;

            for (int i = 0; i < _lstEsquemas.Count; i++)
            {
                if (_lstEsquemas[i].Fila > filas)
                {
                    filas = _lstEsquemas[i].Fila;
                }

                if (_lstEsquemas[i].Columna > col)
                {
                    col = _lstEsquemas[i].Columna;
                }
            }

            filas += 1;
            col += 1;
        }

        private TableScheme BuscarEsquemaControl(string strNombre)
        {
            TableScheme esquema = new TableScheme();

            for (int i = 0; i < _lstEsquemas.Count; i++)
            {
                if (_lstEsquemas[i].Nombre == strNombre)
                {
                    esquema = _lstEsquemas[i];
                    break;
                }
            }

            return esquema;
        }

        private bool HabilitarControl(modoForm _modoApertura, bool _esPK, bool _esModificable)
        {
            bool _controlHabilitado = false;

            switch (_modoApertura)
            {
                case modoForm.lectura:
                    break;

                case modoForm.edicion:
                case modoForm.nuevo:
                case modoForm.clonar:
                    if (_esModificable)
                    {
                        _controlHabilitado = true;
                    }
                    break;
            }

            return _controlHabilitado;
        }

        protected virtual string ComponerJSON()
        {
            var jsonObject = new JObject();

            if (this.dataDialog1.PanelControles.Controls[0] != null)
            {
                TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0].Controls[0];

                for (int i = 0; i < _filasTotalesTabla; i++)
                {
                    for (int j = 0; j < tpanel.ColumnCount; j++)
                    {
                        NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);
                        NameValue.SacarValorALinea(ref jsonObject, par);
                    }
                }
            }

            if (ModoAperturaForm == modoForm.clonar)
            {
                ComponerJSONGrid(ref jsonObject);
            }
            Debug.WriteLine(jsonObject.ToString());
            return jsonObject.ToString();
        }

        private string ComponerJSONGrid(ref JObject jsonObject)
        {
            string json = "";

            if (this.baseGridControl != null && this.baseGridControl.Controls[0] != null)
            {
                TableLayoutPanel tpanel = (TableLayoutPanel)this.baseGridControl.Controls[0];

                RadVirtualGrid grid = (RadVirtualGrid)((TableLayoutPanel)this.baseGridControl.Controls[0]).GetControlFromPosition(0, 1);
                if (grid != null)
                    filasGrid = grid.RowCount;
                else filasGrid = 0;
            }

            return json;
        }

        #endregion Funciones Auxiliares
    }
}
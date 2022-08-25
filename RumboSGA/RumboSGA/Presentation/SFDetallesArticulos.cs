using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGA.Presentation.Herramientas;
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
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation
{
    public partial class SFDetallesArticulos : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables, Propiedades y Enumeradores

        public ArticulosControl ArticulosGrid { get; private set; }
        private BaseGridControl baseGridControl;

        public enum modoForm
        {
            lectura,
            nuevo,
            edicion,
            clonar
        }

        public dynamic newRecord { get; set; }
        public bool editado;
        public int filasGrid = 0;

        /*public static modoForm ModoAperturaForm { get { return _modoAperturaForm; } set { _modoAperturaForm = value; } }
        private static modoForm _modoAperturaForm;*/

        public modoForm ModoAperturaForm
        { get { return _modoAperturaForm; } set { _modoAperturaForm = value; } }

        private modoForm _modoAperturaForm;

        //private List<TableScheme> _lstEsquemasForm;

        public int _filasTotalesTabla = 0;

        private const float K_RATIO_AMPLIAR_VENTANA = 0.5F;

        //_lstEsquemas
        private List<TableScheme> _lstEsquemas;

        //_values
        private dynamic _values;

        private string tabElegida;

        //_modoApertura
        //modoForm ModoAperturaForm;
        //_esquemGrid
        private GridScheme _esquemGrid;

        //_diccParamNuevo
        private Hashtable _diccParamNuevo;

        //RadWaitingBar radWaitingBar1;
        private Panel panWaiting = new Panel();

        private RadWaitingBar radWait = new RadWaitingBar();
        public string registro;
        private string valor;
        private List<ElementoValor> _listaCombo;

        public SFDetallesArticulos AgregarTabsGeneral { get; set; }
        public SFDetallesArticulos AgregarTabsUbicacion { get; set; }
        public SFDetallesArticulos AgregarTabsUbicacionUD { get; set; }
        public SFDetallesArticulos AgregarTabsSKU { get; set; }
        public SFDetallesArticulos AgregarTabsMedidas { get; set; }

        private TableLayoutPanel TablaPanel = new TableLayoutPanel();
        private NameValue CRUDCntrl;

        #endregion Variables, Propiedades y Enumeradores

        #region Constructor

        public SFDetallesArticulos(string nombre, string Tab, List<TableScheme> _lstEsquemasParam, dynamic _valuesParam, modoForm _modoAperturaParam, GridScheme _esquemGridParam = null, Hashtable _diccParamNuevoParam = null)
        {
            InitializeComponent();
            this.Name = nombre;
            this.Dock = DockStyle.Fill;
            //tableLayoutPanel1_Detalle.Dock = DockStyle.Fill;
            //this.dataDialog1.tituloForm.Text = nombre;
            _lstEsquemas = _lstEsquemasParam;
            _values = _valuesParam;
            tabElegida = Tab;
            //_modoApertura = _modoAperturaParam;
            _esquemGrid = _esquemGridParam;
            _diccParamNuevo = _diccParamNuevoParam;
            ModoAperturaForm = _modoAperturaParam;
            //this.WindowState = FormWindowState.Maximized;
            registro = JsonConvert.DeserializeObject(Convert.ToString(_values)).ToString(Formatting.None);

            if (_esquemGrid != null)
            {
                float anchoNuevo = (this.Size.Width * K_RATIO_AMPLIAR_VENTANA) + this.Size.Width;
                float altoNuevo = (this.Size.Height * K_RATIO_AMPLIAR_VENTANA) + this.Size.Height;

                this.Size = new System.Drawing.Size(Convert.ToInt32(anchoNuevo), Convert.ToInt32(altoNuevo));
            }

            //Eventos

            if (nombre == "SFDetallesGeneral") { }
            /*panWaiting.Dock = DockStyle.Fill;
            radWait.Dock = DockStyle.Bottom;
            this.panWaiting.Controls.Add(radWait);
            this.Controls.Add(panWaiting);
            panWaiting.BringToFront();

            myBackgroundWorker = new BackgroundWorker();
            myBackgroundWorker.WorkerReportsProgress = true;
            myBackgroundWorker.WorkerSupportsCancellation = true;
            myBackgroundWorker.DoWork += new DoWorkEventHandler(myBackgroundWorker1_DoWork);
            myBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myBackgroundWorker1_RunWorkerCompleted);*/
            //this.LanzarProcesoAsincrono();

            this.Load += SFDetalles_Load;
            // this.Shown += Form_Shown;
            this.FormClosed += SFDetalles_FormClosed;
        }

        private void LanzarProcesoAsincrono()
        {
            /*if (!myBackgroundWorker.IsBusy)
            {
                myBackgroundWorker.RunWorkerAsync();
                this.radWaitingBar1.StartWaiting();
                this.radWait.StartWaiting();
            }*/
        }

        public void editButton_Click(object sender, EventArgs e)
        {
            HabilitarDeshabilitarControles(modoForm.edicion);
        }

        private void myBackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker = sender as BackgroundWorker;
            //CargarDatos(_selectedRow);
        }

        private void myBackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.radWait.StopWaiting();
            this.radWait.ResetWaiting();

            panWaiting.Visible = false;
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
            //Cesar: Al cerrar la aplicación sin haber cerrado el formulario articulos da error ya que ArticulosGrid es null.
            try
            {
                ArticulosGrid.newRecord = ComponerJSON();
                if (ModoAperturaForm == modoForm.edicion)
                {
                    editado = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        public void CambiarModo()
        {
            HabilitarDeshabilitarControles(modoForm.edicion);
        }

        public void AceptarFormulario()
        {
            newRecord = ComponerJSON();

            if (ModoAperturaForm == modoForm.edicion)
            {
                editado = true;
            }
            HabilitarDeshabilitarControles(modoForm.lectura);
        }

        public void CancelarFormulario()
        {
            DialogResult = DialogResult.Cancel;

            HabilitarDeshabilitarControles(modoForm.lectura);
        }

        #endregion Eventos

        #region Funciones Auxiliares

        public void CargarDatos(dynamic _values, Size tamañoTabla)
        {
            int filas = 0;
            int columnas = 0;
            TamañoTabla(ref filas, ref columnas, _lstEsquemas);

            /*if(TablaPanel.Controls != null)
            {
                TablaPanel.Controls.Clear();
            }*/

            //TablaPanel.Dock = DockStyle.Fill;
            TablaPanel.ColumnCount = columnas + 1;
            this.TablaPanel.AutoScroll = true;
            //int AnchoTotal = this.dataDialog1.PanelControles.Width;

            for (int i = 0; i < columnas; i++)
            {
                //ColumnStyle columnStyle = new ColumnStyle(SizeType.Absolute, AnchoTotal / columnas);
                ColumnStyle columnStyle = new ColumnStyle(SizeType.AutoSize);
                TablaPanel.ColumnStyles.Add(columnStyle);
            }
            //añadimos al final un autosize vacío, este autosize se repartira el ultimo sitio que quede restante
            TablaPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            int rowPosition = 0;
            int altoFilaDefecto = 30;
            int altoTotal = 0;
            bool bolNuevaFila = false;
            List<RowStyle> arrayFilas = new List<RowStyle>();
            TablaPanel.RowCount += filas;
            if (filas > 0)
            {
                //rowPosition = tablaPanelControles.RowCount;
                bolNuevaFila = true;
                for (int a = 0; a < filas; a++)
                {
                    TablaPanel.RowCount += 1;
                    float mayorAlto = GetMayorAltoFila(_lstEsquemas, a);
                    //tablaPanelControles.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

                    arrayFilas.Add(new RowStyle(SizeType.Absolute, mayorAlto));
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
                                TablaPanel.RowStyles.Add(arrayFilas[j]);

                                altoTotal += (int)arrayFilas[rowPosition].Height;
                                rowPosition += 1;
                            }
                            else
                            {
                                TablaPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, altoFilaDefecto));
                                altoTotal += altoFilaDefecto;
                                rowPosition += 1;
                            }

                            bolNuevaFila = false;
                        }
                    }

                    string valor = string.Empty;

                    try
                    {
                        if (_values != null)
                        {
                            var z = JsonConvert.DeserializeObject(Convert.ToString(_values));
                            var pn = (string)z[_lstEsquemas[i].Nombre];
                            if (pn != null)
                                valor = pn.ToString();
                        }
                    }
                    catch (System.NullReferenceException ex)
                    {
                    }
                    catch (Exception e)
                    {
                        ExceptionManager.GestionarError(e);
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

                    //Compara la pestaña en la que estamos con la pestaña del json y decide si genera o no los campos correspondientes

                    /*Esta parte del código es muy importante(la que viene a continuación)
                     y debe ser modificada con mucho cuidado, ya que las 5 pestañas
                     deben de tener el ID al que pertenecen todas ellas.
                     El método les genera el campo ID de forma oculta a las 5 pestañas.
                     Si no se hiciera así y el programa no supiera el ID, al darle
                     al botón editar y guardar los cambios, modificaría
                     toda la base de datos por igual, rompiéndola completamente.
                     Si quieres modificar este método haz una copia de la base
                     de datos primero.
                     */
                    if (_lstEsquemas[i].Nombre == "idarticulo")
                    //genera el campo ID
                    {
                        CRUDCntrl = new NameValue(_lstEsquemas[i], valor);
                        CRUDCntrl.AutoCalculoArticulos();

                        if (_esquemGrid != null)
                        {
                            if (_esquemGrid.Filtro == _lstEsquemas[i].Nombre)
                            {
                                _esquemGrid.ValorFiltro = valor;
                            }

                            _esquemGrid.SetValorClave(_lstEsquemas[i].Nombre, valor);
                        }
                        //Lo añade con los demás campos y lo oculta
                        TablaPanel.Controls.Add(CRUDCntrl, _lstEsquemas[i].Columna, _lstEsquemas[i].Fila);
                        if (_lstEsquemas[i].Tab != "General")
                        {
                            CRUDCntrl.Visible = false;
                        }
                    }
                    else
                    {
                        if (tabElegida == _lstEsquemas[i].Tab)//añade según la pestaña que esté especificada en el JSON
                        {
                            CRUDCntrl = new NameValue(_lstEsquemas[i], valor);
                            CRUDCntrl.AutoCalculoArticulos();

                            if (_esquemGrid != null)
                            {
                                if (_esquemGrid.Filtro == _lstEsquemas[i].Nombre)
                                {
                                    _esquemGrid.ValorFiltro = valor;
                                }

                                _esquemGrid.SetValorClave(_lstEsquemas[i].Nombre, valor);
                            }

                            TablaPanel.Controls.Add(CRUDCntrl, _lstEsquemas[i].Columna, _lstEsquemas[i].Fila);
                        }
                    }
                }

                _filasTotalesTabla = TablaPanel.RowCount; //Guardar número de filas para la devolución del JSON y para la Edición
                                                          //tablaPanelControles.RowCount = tableLayoutPanel1.RowStyles.Count;

                //TablaPanel.Size = new System.Drawing.Size(this.Width - SystemInformation.VerticalScrollBarWidth - 30, /*this.ClientSize.Height*/ altoTotal);

                this.dataDialog1.PanelControles.AutoScroll = true;
                if (this.dataDialog1.PanelControles.AutoScrollMargin.Width < 5 ||
                    this.dataDialog1.PanelControles.AutoScrollMargin.Height < 5)
                {
                    //this.dataDialog1.PanelControles.Invoke((MethodInvoker)delegate { this.dataDialog1.PanelControles.SetAutoScrollMargin(5, 5); });
                }

                if (this.dataDialog1.PanelControles.InvokeRequired)
                {
                    this.dataDialog1.PanelControles.Invoke((MethodInvoker)delegate { this.dataDialog1.PanelControles.Controls.Add(TablaPanel); });
                    TablaPanel.Refresh();
                }
                else
                {
                    this.dataDialog1.PanelControles.Controls.Add(TablaPanel);
                    TablaPanel.Refresh();
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
            this.TablaPanel.Size = tamañoTabla;
            this.Size = tamañoTabla;
        }

        public void ActualizarDatos(dynamic _values)
        {
            for (int i = 0; i < _lstEsquemas.Count; i++)
            {
                valor = string.Empty;

                try
                {
                    if (_values != null)
                    {
                        var z = JsonConvert.DeserializeObject(Convert.ToString(_values));
                        var pn = (string)z[_lstEsquemas[i].Nombre];
                        if (pn != null)
                            valor = pn.ToString();
                    }
                }
                catch (System.NullReferenceException ex)
                {
                }
                if (_lstEsquemas[i].Nombre == "idarticulo")
                //genera el campo ID
                {
                    // CRUDCntrl.ActualizarCampos(_lstEsquemas[i], valor);

                    if (_esquemGrid != null)
                    {
                        if (_esquemGrid.Filtro == _lstEsquemas[i].Nombre)
                        {
                            _esquemGrid.ValorFiltro = valor;
                        }

                        _esquemGrid.SetValorClave(_lstEsquemas[i].Nombre, valor);
                    }
                    //Lo añade con los demás campos y lo oculta
                    ActualizarControles(_lstEsquemas[i].Nombre, _lstEsquemas[i]);

                    if (_lstEsquemas[i].Tab != "General")
                    {
                        CRUDCntrl.Visible = true;
                    }
                }
                else
                {
                    if (tabElegida == _lstEsquemas[i].Tab)//añade según la pestaña que esté especificada en el JSON
                    {
                        if (_esquemGrid != null)
                        {
                            if (_esquemGrid.Filtro == _lstEsquemas[i].Nombre)
                            {
                                _esquemGrid.ValorFiltro = valor;
                            }

                            _esquemGrid.SetValorClave(_lstEsquemas[i].Nombre, valor);
                        }

                        ActualizarControles(_lstEsquemas[i].Nombre, _lstEsquemas[i]);
                    }
                }
            }
        }

        //Decide qué controles habilitar o deshabilitar en función del modo en que abramos el formulario
        public void HabilitarDeshabilitarControles(modoForm _modoAp)
        {
            try
            {
                ModoAperturaForm = _modoAp;

                /*botones
                switch (_modoAp)
                {
                    case modoForm.lectura:

                        break;

                    case modoForm.edicion:
                    case modoForm.nuevo:
                    case modoForm.clonar:

                        break;
                }*/

                //controles dinámicos
                if (this.dataDialog1.PanelControles.Controls[0] != null)
                {
                    //TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0];

                    TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0].Controls[0];
                    tpanel.Dock = DockStyle.Fill;

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
                                        if (combo.SelectedValue == null)
                                        {
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
                                        };
                                        combo.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
                                        break;

                                    case "RadDateTimePicker":
                                        RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(dtPicker.Name);
                                        dtPicker.Enabled = HabilitarControl(_modoAp, esquemaBuscado.EsPK, esquemaBuscado.EsModificable);
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

        public void ActualizarControles(string nombreControl, TableScheme _tableScheme)
        {
            try
            {
                //controles dinámicos
                if (this.dataDialog1.PanelControles.Controls[0] != null)
                {
                    TableLayoutPanel tpanel = (TableLayoutPanel)this.dataDialog1.PanelControles.Controls[0].Controls[0];
                    tpanel.Dock = DockStyle.Fill;

                    //for (int i = 0; i < tpanel.RowCount; i++)
                    for (int i = 0; i < _filasTotalesTabla + 1; i++)
                    {
                        for (int j = 0; j < tpanel.ColumnCount; j++)
                        {
                            NameValue par = (NameValue)tpanel.GetControlFromPosition(j, i);

                            if (par != null)
                            {
                                TableScheme esquemaBuscado;

                                if (par.tableLayoutControl.GetControlFromPosition(1, 0) == null) continue;
                                if (!par.tableLayoutControl.GetControlFromPosition(1, 0).Name.Equals(nombreControl)) continue;

                                switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                                {
                                    case "RadTextBox":

                                        RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        if (nombreControl == tb.Name)
                                        {
                                            esquemaBuscado = BuscarEsquemaControl(tb.Name);
                                            tb.Text = valor;
                                            tpanel.Update();
                                        }
                                        break;

                                    case "RadMultiColumnComboBox":
                                        RadMultiColumnComboBox combo = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        if (nombreControl == combo.Name)
                                        {
                                            RadListDataItem selectedItem = null;

                                            if (selectedItem == null)
                                            {
                                                if (!string.IsNullOrEmpty(_tableScheme.CmbObject.ValorDefecto))
                                                {
                                                    combo.Text = valor;
                                                    //combo.SelectedValue = _tableScheme.CmbObject.CampoRelacionado;
                                                }
                                            }
                                            else
                                            {
                                                combo.SelectedItem = selectedItem;
                                            }
                                            esquemaBuscado = BuscarEsquemaControl(combo.Name);
                                        }
                                        break;

                                    case "RadDateTimePicker":
                                        RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(dtPicker.Name);

                                        break;

                                    case "RadButtonTextBox":
                                        RadButtonTextBox rdColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(rdColor.Name);
                                        break;

                                    case "RadCheckBox":
                                        RadCheckBox rCheckBox = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                        esquemaBuscado = BuscarEsquemaControl(rCheckBox.Name);
                                        if (valor.Equals("S"))
                                        {
                                            rCheckBox.Checked = true;
                                        }
                                        else
                                        {
                                            rCheckBox.Checked = false;
                                        }
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

        private int GetMayorAltoFila(List<TableScheme> _lstEsquemas, int numFila)
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

        private int GetMayorAnchoColumna(List<TableScheme> _lstEsquemas, int numColumna)
        {
            int ancho = 0;
            for (int i = 0; i < _lstEsquemas.Count; i++)
            {
                if (_lstEsquemas[i].Ancho > ancho && _lstEsquemas[i].Columna == numColumna && _lstEsquemas[i].Tab == this.tabElegida)
                {
                    ancho = _lstEsquemas[i].Ancho;
                }
            }

            return ancho;
        }

        private void TamañoTabla(ref int filas, ref int col, List<TableScheme> _lstEsquemas)
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

        public TableScheme BuscarEsquemaControl(string strNombre)
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

        public bool HabilitarControl(modoForm _modoApertura, bool _esPK, bool _esModificable)
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

        private string ComponerJSON()
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

                        if (par != null)
                        {
                            TableScheme esquemaBuscado;
                            string _valor;

                            switch (par.tableLayoutControl.GetControlFromPosition(1, 0).GetType().Name)
                            {
                                case "RadTextBox":
                                    RadTextBox tb = (RadTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = BuscarEsquemaControl(tb.Name);
                                    _valor = tb.Text;
                                    switch (esquemaBuscado.Tipo)
                                    {
                                        case "INT":
                                            break;

                                        default:
                                            _valor = "'" + _valor + "'";
                                            break;
                                    }
                                    jsonObject.Add(esquemaBuscado.Nombre, _valor);
                                    break;

                                case "RadMultiColumnComboBox":
                                    RadMultiColumnComboBox combo = (RadMultiColumnComboBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = BuscarEsquemaControl(combo.Name);
                                    if (combo.SelectedItem != null)
                                    {
                                        _valor = combo.SelectedValue.ToString();
                                        switch (esquemaBuscado.Tipo)
                                        {
                                            case "INT":
                                                break;

                                            default:
                                                _valor = "'" + _valor + "'";
                                                break;
                                        }
                                        jsonObject.Add(esquemaBuscado.Nombre, _valor);
                                    }
                                    break;

                                case "RadDateTimePicker":
                                    RadDateTimePicker dtPicker = (RadDateTimePicker)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = BuscarEsquemaControl(dtPicker.Name);
                                    _valor = dtPicker.Value.ToString();
                                    switch (esquemaBuscado.Tipo)
                                    {
                                        case "INT":
                                            break;

                                        default:
                                            _valor = "'" + _valor + "'";
                                            break;
                                    }
                                    jsonObject.Add(esquemaBuscado.Nombre, _valor);
                                    break;

                                case "RadButtonTextBox":
                                    RadButtonTextBox rLabelColor = (RadButtonTextBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = BuscarEsquemaControl(rLabelColor.Name);
                                    _valor = rLabelColor.Text.ToString();
                                    switch (esquemaBuscado.Tipo)
                                    {
                                        case "INT":
                                            break;

                                        default:
                                            _valor = "'" + _valor + "'";
                                            break;
                                    }
                                    jsonObject.Add(esquemaBuscado.Nombre, _valor);
                                    break;

                                case "RadCheckBox":
                                    RadCheckBox rCheckBox = (RadCheckBox)par.tableLayoutControl.GetControlFromPosition(1, 0);
                                    esquemaBuscado = BuscarEsquemaControl(rCheckBox.Name);
                                    if (rCheckBox.Checked)
                                    {
                                        _valor = "'S'";
                                    }
                                    else
                                    {
                                        _valor = "'N'";
                                    }
                                    jsonObject.Add(esquemaBuscado.Nombre, _valor);
                                    break;
                            }
                        }
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

                filasGrid = grid.RowCount;
            }

            return json;
        }

        #endregion Funciones Auxiliares
    }
}
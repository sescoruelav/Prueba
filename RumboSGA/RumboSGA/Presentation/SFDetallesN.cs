using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGAManager;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation
{
    public partial class SFDetallesN : Telerik.WinControls.UI.ShapedForm
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BackgroundWorker backgroundWorkerCarga;
        private List<TableScheme> lstEsquemaTabla;
        private List<NameValue> listaNV;
        private TableLayoutPanel tlpPrincipal;
        public dynamic newRecord;
        private dynamic selectedRow;
        private modoForm modoApertura;
        public bool editado = false;
        private int filas = 0;
        private int columnas = 0;
        private bool RumPageView;
        private RumPageView rumPageView;
        public bool ComprobarValores = false;

        public enum modoForm
        {
            lectura,
            nuevo,
            edicion,
            clonar
        }

        public SFDetallesN(string nombre, List<TableScheme> lstEsquemaTabla, dynamic selectedRow, modoForm modoApertura, bool RumPageView)
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.RumPageView = RumPageView;
            this.modoApertura = modoApertura;
            this.WindowState = FormWindowState.Maximized;
            this.lstEsquemaTabla = lstEsquemaTabla;
            this.selectedRow = selectedRow;
            this.panelPrincipal.AutoScroll = true;
            TamañoTabla(ref filas, ref columnas, lstEsquemaTabla);

            backgroundWorkerCarga = new BackgroundWorker();
            backgroundWorkerCarga.DoWork += WorkerCrearTabla_DoWork;
            backgroundWorkerCarga.RunWorkerCompleted += WorkerCrearTabla_RunWorkerCompleted;
            panelPrincipal.Visible = false;
        }

        private void LanzarProcesoAsincrono()
        {
            if (!backgroundWorkerCarga.IsBusy)
            {
                backgroundWorkerCarga.RunWorkerAsync();
            }
        }

        private void WorkerCrearTabla_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                log.Error("BackGroudWorker error: " + e.Error);
            }
            if (e.Result == null)
            {
                log.Error("El runworker a devuelto null");
                return;
            }
            List<Object> resultado = (e.Result as List<Object>);
            listaNV = (resultado[1] as List<NameValue>);
            if (RumPageView)
            {
                rumPageView = (resultado[0] as RumPageView);

                foreach (RadPageViewPage page in rumPageView.Pages)
                {
                    (page.Controls[0] as TableLayoutPanel).AutoSize = true;
                    (page.Controls[0] as TableLayoutPanel).Dock = (DockStyle.Top | DockStyle.Left);
                    page.PageLength = page.Controls[0].Size.Height + 10;//Aplico 10 mas para que no se corte ningún TB
                    page.IsContentVisible = true;
                }
                this.panelPrincipal.Controls.Add(rumPageView);
            }
            else
            {
                tlpPrincipal = (resultado[0] as TableLayoutPanel);
                tlpPrincipal.AutoScroll = true;
                this.panelPrincipal.Controls.Add(tlpPrincipal);
            }

            panelPrincipal.Visible = true;
        }

        private void WorkerCrearTabla_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Object> resultado = new List<Object>();
            TableLayoutPanel tlp = new TableLayoutPanel();
            List<NameValue> listaNVWorker = new List<NameValue>();
            tlp.AutoSize = true;
            tlp.AutoScroll = true;
            //tlp.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            tlp.ColumnCount = columnas + 1;

            if (RumPageView)
            {
                RumPageView rpv = new RumPageView("", lstEsquemaTabla, tableLayoutPanelPrincipal.Width, true, true);
                rpv.Dock = DockStyle.Fill;
                rpv.ViewMode = Telerik.WinControls.UI.PageViewMode.ExplorerBar;

                RadPageViewExplorerBarElement explorerBarElement = (rpv.ViewElement as RadPageViewExplorerBarElement);
                /*explorerBarElement.Text = "Yeah";
                explorerBarElement.ContentSizeMode = ExplorerBarContentSizeMode.FixedLength;
                explorerBarElement.TextParams.enabled = true;
                explorerBarElement.TextParams.backColor = Color.AliceBlue;
                explorerBarElement.TextParams.forceBackColor = true;*/

                rpv.LoadSelectedRow(selectedRow);
                resultado.Add(rpv);
                resultado.Add(rpv.listaNV);
            }
            else
            {
                for (int i = 0; i < columnas; i++)
                {
                    tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, Width / columnas));
                }
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 1));

                tlp.RowCount = filas;
                for (int i = 0; i < filas; i++)
                {
                    tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }

                JObject lineaSeleccionada = null;
                if (this.selectedRow != null)
                {
                    lineaSeleccionada = JsonConvert.DeserializeObject<JObject>(selectedRow);
                }
                bool editar = false;
                if (modoApertura == modoForm.nuevo)
                {
                    editar = true;
                }
                NameValue nv;
                foreach (TableScheme ts in lstEsquemaTabla)
                {
                    if (lineaSeleccionada != null)
                    {
                        nv = new NameValue(ts, lineaSeleccionada.GetValue(ts.Nombre).ToString());
                    }
                    else
                    {
                        nv = new NameValue(ts, "");
                    }
                    nv.Name = ts.Nombre;
                    nv.Editable(editar);
                    //nv.Dock = DockStyle.Fill
                    listaNVWorker.Add(nv);
                    tlp.Controls.Add(nv, ts.Columna, ts.Fila);
                }

                resultado.Add(tlp);
                resultado.Add(listaNVWorker);
            }
            e.Result = resultado;
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

        protected virtual string ComponerJSON()
        {
            var jsonObject = new JObject();

            if (this.listaNV != null)
            {
                for (int i = 0; i < listaNV.Count; i++)
                {
                    NameValue par = listaNV[i];
                    NameValue.SacarValorALinea(ref jsonObject, par);
                }
            }

            return jsonObject.ToString();
        }

        private void editableNameValues(bool editable)
        {
            foreach (NameValue nv in listaNV)
            {
                if (nv.getTableScheme().EsModificable && editable)
                {
                    nv.Editable(true);
                }
                else
                {
                    nv.Editable(false);
                }
            }
        }

        public JObject getLineaValoresValidos(ref string error)
        {
            try
            {
                JObject resultado = new JObject();
                error = "";
                NameValue.SacarLinea(ref resultado, ref error, listaNV);

                return resultado;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
            return null;
        }

        private void SFDetallesN_Load(object sender, EventArgs e)
        {
            LanzarProcesoAsincrono();
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            if (ComprobarValores)
            {
                string error = "";
                this.newRecord = getLineaValoresValidos(ref error).ToString();
                if (!error.Equals(""))
                {
                    RadMessageBox.Show(error);
                    return;
                }
            }
            DialogResult = DialogResult.OK;
            editado = true;
            Close();
        }

        private void SFDetallesN_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void RumButtonEditar_Click(object sender, EventArgs e)
        {
            rumButtonEditar.Visible = false;
            rumButtonAceptar.Visible = true;
            rumButtonCancelar.Visible = true;
            editableNameValues(true);
        }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGAManager;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    public partial class RumPageView : RadPageView
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TableScheme> lstEsquemaTabla;
        private int cantidadRegistrosFila = 4;
        public List<NameValue> listaNV = new List<NameValue>();
        private String nombreJson = "";
        private dynamic selectedRow;
        private bool mismoTamañoTabla;
        private List<RadPageViewPage> listaPagePrueba;

        public RumPageView(String nombreJson, List<TableScheme> lstEsquemaTabla, int Width, bool mismoTamañoTabla, bool detalles)
        {
            InitializeComponent();
            this.mismoTamañoTabla = mismoTamañoTabla;
            this.nombreJson = nombreJson;
            this.Dock = DockStyle.Fill;
            this.lstEsquemaTabla = lstEsquemaTabla;
            LoadTable(Width, detalles);
        }

        public RumPageView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void LoadTable(int Width, bool detalles)
        {
            //Columnas y filas: 2 posibilidades, mismo numero de columnas y filas para todos o distinto
            int columnas = 0;
            int filas = 0;
            listaPagePrueba = new List<RadPageViewPage>();

            if (this.lstEsquemaTabla == null || lstEsquemaTabla.Count == 0) return;
            List<String> listaPaginas = new List<String>();
            foreach (TableScheme ts in lstEsquemaTabla)
            {
                String nombrePagina = "";
                nombrePagina = ts.Tab;
                if (!listaPaginas.Contains(nombrePagina))
                {
                    listaPaginas.Add(nombrePagina);
                }

                if (mismoTamañoTabla)
                {
                    if (ts.Columna > columnas) columnas = ts.Columna;
                    if (ts.Fila > filas) filas = ts.Fila;
                }
            }
            columnas++;
            filas++;
            foreach (String pagina in listaPaginas)
            {
                if (!mismoTamañoTabla)
                {
                    columnas = 0;
                    filas = 0;
                }
                RadPageViewPage pv = new RadPageViewPage();
                pv.Padding = new Padding(0, 0, 5, 0);
                pv.Name = pagina;
                List<TableScheme> esquemasPagina = new List<TableScheme>();

                foreach (TableScheme ts in lstEsquemaTabla)
                {
                    if (ts.Tab.Equals(pagina))
                    {
                        esquemasPagina.Add(ts);
                        if (!mismoTamañoTabla)
                        {
                            if (ts.Columna > columnas) columnas = ts.Columna;
                            if (ts.Fila > filas) filas = ts.Fila;
                        }
                    }
                }
                if (!mismoTamañoTabla)
                {
                    columnas++;
                    filas++;
                }

                TableLayoutPanel tlp = new TableLayoutPanel();
                tlp.AutoSize = true;
                tlp.AutoScroll = true;
                tlp.Dock = DockStyle.Fill;

                tlp.ColumnCount = columnas;
                for (int i = 0; i < columnas; i++)
                {
                    tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, (Width) - 200 / columnas));
                }
                log.Info(columnas + " + " + tlp.ColumnStyles[0].Width.ToString());
                tlp.RowCount = filas;
                for (int i = 0; i < filas; i++)
                {
                    tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }

                foreach (TableScheme ts in esquemasPagina)
                {
                    if (ts.Tipo == "varchar")
                    {
                    }
                    NameValue nv = new NameValue(ts, "");
                    nv.Name = ts.Nombre;
                    listaNV.Add(nv);
                    tlp.Controls.Add(nv, ts.Columna, ts.Fila);
                }
                Editable(false);
                //pv.Text = Lenguaje.traduce(pagina);

                pv.Font = new Font(pv.Font, FontStyle.Bold);

                pv.Controls.Add(tlp);

                ((RadPageViewStripElement)this.ViewElement).StripButtons = StripViewButtons.None;
                if (!pagina.ToLower().Equals("oculto"))
                {
                    if (detalles)
                    {
                        listaPagePrueba.Add(pv);
                    }
                    else
                    {
                        this.Pages.Add(pv);
                    }
                }
            }
            if (detalles)
            {
                ModificarPaginasArticulos();
            }
        }

        private void ModificarPaginasArticulos()
        {
            RadPageViewPage[] listaPage = new RadPageViewPage[listaPagePrueba.Count];

            int contadorLista = listaPagePrueba.Count - 1;
            foreach (RadPageViewPage page in listaPagePrueba)
            {
                if (contadorLista >= 0)
                {
                    listaPage[contadorLista] = page;
                    contadorLista--;
                }
            }
            foreach (RadPageViewPage page in listaPage)
            {
                contadorLista++;
                this.Pages.Add(page);
            }
        }

        public void LoadSelectedRow(dynamic selectedRow)
        {
            Editable(false);
            this.selectedRow = selectedRow;
            JObject lineaSeleccionada = null;
            try
            {
                lineaSeleccionada = JsonConvert.DeserializeObject<JObject>(selectedRow);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en cargarVaores en selectedRow:" + selectedRow);
            }
            if (lineaSeleccionada == null) return;

            foreach (NameValue nv in listaNV)
            {
                try
                {
                    String valor = lineaSeleccionada.GetValue(nv.Name).ToString();
                    nv.SetValor(valor);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarErrorNuevo(ex, "Error en cargarVaores en NameValue: " + nv.Name);
                }
            }
        }

        public void Editable(bool editable)
        {
            foreach (NameValue nv in listaNV)
            {
                nv.Editable(editable);
            }
        }

        public AckResponse SaveChanges()
        {
            String error = "";
            AckResponse res = null;
            String lineaNueva = FuncionesGenerales.ComponerRowDesdeNameValue(this.listaNV, ref error).ToString();
            if (!error.Equals(""))
            {
                res = new AckResponse();
                RadMessageBox.Show(error);
                res.Mensaje = error;
                res.Resultado = "KO";
                return res;
            }
            res = FuncionesGenerales.jsonUpdate(nombreJson, lineaNueva, selectedRow);
            if (res.Resultado.Equals("OK"))
            {
                LoadSelectedRow(this.selectedRow);
            }
            else
            {
                RadMessageBox.Show(Lenguaje.traduce(res.Mensaje));
            }

            return res;
        }
    }
}
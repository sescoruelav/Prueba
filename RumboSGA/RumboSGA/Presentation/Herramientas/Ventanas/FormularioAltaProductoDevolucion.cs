using Rumbo.Core.Herramientas;
using Rumbo.Core.Herramientas.Herramientas;
using RumboSGA.DevolucionesMotor;
using RumboSGA.LotesMotor;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.Presentation.Herramientas.FormulariosComunes;
using RumboSGA.Presentation.Herramientas.PantallasWS;
using RumboSGA.Properties;
using RumboSGA.VariablesMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FormularioAltaProductoDevolucion : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        private int opcion, idArticulo;
        private int idDevolCli;
        private int idDevolCliLin;
        private List<OAtributo> listaAtributos;
        private bool existeLote;

        public const int AltaPalet = 1;
        public const int AltaPaletPicos = 2;
        public const int AltaPaletMultireferencia = 3;
        public const int AltaPaletCarroMovil = 4;
        public const int AltaPaletEntradasTotales = 5;
        public const int ENTRADA_PEDIDO_MOV = 3;
        public const int ENTRADA_TOTALES = 4;
        public const int ENTRADA_MULTIREFERENCIA = 5;
        public const int ENTRADA_INCLUIDO_EN_MULTI = 7;

        /// <summary>
        /// se utiliza en el coteo carro movil
        /// </summary>
        public const int ENTRADA_CARRO = 6;

        private WSVariablesClient wsv = new WSVariablesClient();
        private WSDevolucionesMotorClient wsp = new WSDevolucionesMotorClient();
        private int tipoPresentacion = 0;
        private int idPresentacion = 0;
        private bool mostrarSSCC = false;
        private bool capturarEAN = false;
        private bool capturarNSerie = false;
        private DateTime? fechaCaducidadCalculada = null;
        private bool conLineas = true;
        private int idArticuloSeleccionado;

        #endregion Variables

        public FormularioAltaProductoDevolucion(int opcion, int idDevolCli_, int idDevolCliLin_, string nombreOpcion, Image imagenIcono, bool conLineas_)
        {
            listaAtributos = null;
            existeLote = false;
            InitializeComponent();
            this.opcion = opcion;
            this.idDevolCli = idDevolCli_;
            this.idDevolCliLin = idDevolCliLin_;
            this.conLineas = conLineas_;
            radTextBoxControlNumPalets.Text = "" + 1;
            int tipoPres = Persistencia.getParametroInt("TIPOPRESENTACION");
            if (tipoPres < 0)
            {
                tipoPresentacion = 0;
            }
            else
            {
                tipoPresentacion = Convert.ToInt32(tipoPres);
            }
            mostrarSSCC = Convert.ToBoolean(Persistencia.getParametroInt("conSSCC"));
            if (Persistencia.getParametro("CAPTURAREAN") != null
                && !Persistencia.getParametro("CAPTURAREAN").Equals(""))
            {
                string valor = Persistencia.getParametro("CAPTURAREAN", 0);
                if (valor.Equals("S"))
                {
                    capturarEAN = true;
                }
            }
            if (!conLineas)
            {
                try
                {
                    CargarDatosCombos();
                    //CargarDatosDefectoAtributos();
                    verCampos(false);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                }
            }
            else
            {
                CargarConfiguracionVisual(opcion);
                CargarDatosDefectoAtributos();
                CargarDatosDefecto();
            }
            //this.Text = Lenguaje.traduce("Alta Producto");
            this.rumLabelImagen.Image = imagenIcono;
            this.rumLabelTipoAlta.Text = nombreOpcion;
            this.Icon = Resources.bruju;
        }

        #region Metodos

        private int CargarDatosCombos()
        {
            try
            {
                rumLabelArtículo.Visible = true;
                radComboBoxArticulo.Visible = true;
                //Cargo combo Articulo
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxArticulo, DataAccess.GetIdReferenciaDescripcionAtributoArticulos(), "IDARTICULO", "DESCRIPCION", "", new String[] { "REFERENCIA", "DESCRIPCION" }, true);
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
                ExceptionManager.GestionarError(e);
            }

            return 0;
        }

        private void verCampos(bool visible)
        {
            rumLabelNumPalets.Visible = visible;
            radTextBoxControlNumPalets.Visible = visible;
            rumLabelEmbalaje.Visible = visible;
            radComboBoxUnidadTipo.Visible = visible;
            rumLabelDE.Visible = visible;
            radTextBoxControlUdsCaja.Visible = visible;
            radTextBoxEmbalaje.Visible = visible;
            rumLabelComentario.Visible = visible;
            radTextBoxComentario.Visible = visible;
            radTextBoxControlLote.Visible = visible;
            rumLabelEAN.Visible = visible;
            radTextBoxEAN.Visible = visible;
            rumLabelEntradaPaletPadre.Visible = visible;
            radTextBoxControlEntradaPaletPadre.Visible = visible;
            rumLabelEstado.Visible = visible;
            radComboBoxEstado.Visible = visible;
            rumLabelExisteLote.Visible = visible;
            rumLabelLote.Visible = visible;
            rumLabelMotivo.Visible = visible;
            radComboBoxMotivo.Visible = visible;
            rumLabelNSerie.Visible = visible;
            radTextBoxNSerie.Visible = visible;

            rumLabelSSCC.Visible = visible;
            radTextBoxSSCC.Visible = visible;
            rumLabelTipoPalet.Visible = visible;
            radComboBoxTipoPalet.Visible = visible;
            rumLabelTotalUdsPicos.Visible = visible;
            radTextBoxControlTotalUdsPicos.Visible = visible;
            rumButtonAlta.Enabled = visible;
            rumButtonCrearEntradaPadre.Visible = visible;
            rumLabelCarro.Visible = visible;
            radComboBoxCarro.Visible = visible;
            rumLabelEtiquetas.Visible = visible;
            radTextBoxControlEtiquetas.Visible = visible;
        }

        private int CargarDatosDefecto()
        {
            try
            {
                List<TableScheme> listaResultado = new List<TableScheme>();
                DataRow valoresOrdenFila = null;
                if (conLineas)
                {
                    valoresOrdenFila = DataAccess.GetAltaArticuloDefectoDevolCli(idDevolCli, idDevolCliLin).Rows[0];
                }
                else
                {
                    valoresOrdenFila = DataAccess.GetAltaArticuloDefecto(idArticuloSeleccionado).Rows[0];
                }
                if (valoresOrdenFila == null)
                {
                    MessageBox.Show(Lenguaje.traduce("No ha devuelto nada"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                this.Text = Lenguaje.traduce("Alta Producto ") + valoresOrdenFila["REFERENCIA"].ToString() + " " + valoresOrdenFila["ATRIBUTO"].ToString() + " " + valoresOrdenFila["DESCRIPCION"].ToString();
                idPresentacion = Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]);
                int tipoPresArticulo = Convert.ToInt32(valoresOrdenFila["TIPOPRESENTACION"]);
                if (tipoPresArticulo > 0)
                {
                    tipoPresentacion = tipoPresArticulo;
                }
                this.idArticulo = int.Parse(valoresOrdenFila["IDARTICULO"].ToString());
                if (tipoPresentacion == 0 || idPresentacion == 0)
                {
                    radTextBoxControlUdsCaja.Text = String.Format("{0:N0}", valoresOrdenFila["CANTIDADUNIDADARTICULO"].ToString());
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, DataAccess.GetIdDescripcionUnidadesTipo(), "IDUNIDADTIPO", "DESCRIPCION", valoresOrdenFila["IDUNIDADTIPOARTICULO"].ToString(), null, true);
                    if (opcion == AltaPalet)
                    {
                        radTextBoxEmbalaje.Text = String.Format("{0:N0}", valoresOrdenFila["CANTIDADARTICULO"].ToString());
                    }
                }
                else if (tipoPresentacion == 1)
                {
                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPOARTICULO"].ToString());

                    if (opcion == AltaPalet)
                    {
                        object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(valoresOrdenFila["CANTIDADARTICULO"]));
                        radTextBoxEmbalaje.Text = String.Format("{0:N0}", presC[0]);
                    }
                }
                else if (tipoPresentacion == 2)
                {
                    object[] pres = Presentaciones.getTipoUnidadPresentacionVisualizacion(idPresentacion);
                    string idUnidadTipoPres = pres[1].ToString();

                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPOARTICULO"].ToString());
                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, idUnidadTipoPres);
                    object[] presCU = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(pres[0]));
                    radTextBoxControlUdsCaja.Text = String.Format("{0:N0}", presCU[0]);
                    if (opcion == AltaPalet)
                    {
                        object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(valoresOrdenFila["CANTIDADARTICULO"]));
                        radTextBoxEmbalaje.Text = String.Format("{0:N0}", presC[0]);
                    }
                }
                /*else
                {
                    object[] pres=Presentaciones.getTipoUnidadPresentacionVisualizacion(idPresentacion);
                    string idUnidadTipoPres = pres[1].ToString();
                    radTextBoxControlUdsCaja.Text = pres[0].ToString();
                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPO"].ToString());Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, idUnidadTipoPres);
                    if (opcion == AltaPalet)
                    {
                        object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(valoresOrdenFila["CANTIDAD"]));
                        radTextBoxEmbalaje.Text = presC[0].ToString();
                    }
                }*/

                //TipoPalet
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxTipoPalet, DataAccess.GetIdDescripcionTipoPalets(), "IDPALETTIPO", "DESCRIPCION", valoresOrdenFila["IDPALETTIPO"].ToString(), new String[] { "TODOS" }, true);

                //Lote
                if (valoresOrdenFila["LOTEESTRICTO"].Equals("L") || valoresOrdenFila["LOTEESTRICTO"].Equals("S"))
                {
                    radTextBoxControlLote.Enabled = true;
                    calcularFechaCaducidad(Convert.ToInt32(valoresOrdenFila["DIASCADUCIDAD"]));
                }

                //Estados Existencia
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxEstado, DataAccess.GetIdDescripcionExistenciaEstados(), "IDEXISTENCIAESTADO", "DESCRIPCION", valoresOrdenFila["ESTADOENTRADA"].ToString(), new String[] { "TODOS" }, true);
                //Motivos devolucion
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxMotivo, DataAccess.GetIdDescripcionMotivosTipo("ER"), "IDMOTIVO", "DESCRIPCION", "", new String[] { "TODOS" }, true);

                //Carro
                if (opcion == AltaPaletCarroMovil)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxCarro, DataAccess.GetIdDescripcionCarrosMovil(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                }
                if (opcion == AltaPaletEntradasTotales)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxCarro, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                }
                if (opcion == AltaPaletMultireferencia)
                {
                }

                string capturaNSerieStr = valoresOrdenFila["CAPTURANSERIE"].ToString();
                if (capturaNSerieStr.Substring(0, 1) == "1")
                {
                    capturarNSerie = true;
                }
                if (!capturarNSerie)
                {
                    radTextBoxNSerie.Visible = false;
                    rumLabelNSerie.Visible = false;
                }
                //CargarDatosDefectoAtributosClave(valoresOrdenFila);
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
                ExceptionManager.GestionarError(e);
            }

            return 0;
        }

        private void calcularFechaCaducidad(int diasCaducidad)
        {
            try
            {
                DateTime hoy = DateTime.Now;
                if (diasCaducidad > 0)
                {
                    fechaCaducidadCalculada = hoy.AddDays(diasCaducidad);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private Object[][] ComprobracionAtributos()
        {
            if (listaAtributos == null || listaAtributos.Count < 1)
            {
                return null;
            }
            Object[][] objectAtributos = new Object[listaAtributos.Count][];

            for (int i = 0; i < tableLayoutPanelAtributos.RowCount; i++)
            {
                String nombre = (tableLayoutPanelAtributos.GetControlFromPosition(0, i) as RumLabel).Text.Replace(":", String.Empty);
                String valor = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox).Text;
                String nombreAtributo = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox).Tag.ToString();
                OAtributo atrib = listaAtributos.Find(atr => atr.Nombre.Equals(nombre));
                if (atrib.Obligatorio == 1 && valor.Equals(""))
                {
                    return new Object[1][] { new Object[] { "Error" } };
                }
                objectAtributos[i] = new Object[2] { nombreAtributo, valor };
            }

            return objectAtributos;
        }

        private void RumButtonCrearEntradaPadre_Click(object sender, EventArgs e)
        {
            FormularioCrearPaletMultireferencia fcp = new FormularioCrearPaletMultireferencia(radComboBoxTipoPalet.SelectedValue.ToString());

            fcp.ShowDialog();
            if (fcp.resultado != null) radTextBoxControlEntradaPaletPadre.Text = fcp.resultado;
        }

        private int CargarDatosDefectoAtributos()
        {
            try
            {
                /*object[] resultadoCompuesto = wsv.cargarDatos(new String[] { DataAccess.CARGA_INI_ATRIBUTOS_ENTRADAS });
                object resultado = DataAccess.DescomponVariables(resultadoCompuesto, DataAccess.CARGA_INI_ATRIBUTOS_ENTRADAS);*/
                List<OAtributo> resultado = UtilidadesWS.getListaAtributos();
                if (resultado == null)
                {
                    log.Info(Lenguaje.traduce("No hay atributos de entradas para cargar"));
                    return 0;
                }
                this.listaAtributos = resultado as List<OAtributo>;

                tableLayoutPanelAtributos.Enabled = true;
                tableLayoutPanelAtributos.Visible = true;

                tableLayoutPanelAtributos.Controls.Clear();
                tableLayoutPanelAtributos.RowCount = listaAtributos.Count;
                tableLayoutPanelAtributos.ColumnCount = 2;
                for (int i = 0; i < listaAtributos.Count; i++)
                {
                    if (i == 0)
                    {
                        tableLayoutPanelAtributos.RowStyles[0].Height = 100 / listaAtributos.Count;
                    }
                    if (i > 0)
                    {
                        tableLayoutPanelAtributos.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / listaAtributos.Count));
                    }
                }
                if (listaAtributos.Count == 0)
                {
                    tableLayoutPanelAtributos.RowStyles[0].Height = 0;
                }

                for (int i = 0; i < listaAtributos.Count; i++)
                {
                    OAtributo atributo = listaAtributos[i];
                    RumLabel rumLabelAtributo = new RumLabel();
                    rumLabelAtributo.Name = "RadLabel" + atributo.Nombre;
                    rumLabelAtributo.Text = atributo.Nombre + ":";

                    RadTextBox radTextBoxAtributo = new RadTextBox();
                    radTextBoxAtributo.Name = "RadTextBox" + atributo.Nombre;
                    radTextBoxAtributo.Tag = atributo.Atributo;
                    radTextBoxAtributo.Width = 180;
                    radTextBoxAtributo.Height = 20;

                    tableLayoutPanelAtributos.Controls.Add(rumLabelAtributo, 0, i);
                    tableLayoutPanelAtributos.Controls.Add(radTextBoxAtributo, 1, i);
                }
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
                ExceptionManager.GestionarError(e);
            }
            return 0;
        }

        private int LlamarWebService(double totalUnidadPicos, double cantidadEmbalaje, double cantidadUnidad, int numPalets, int idOperario, string idUnidadTipo, string idPaletTipo, string lote,
            string impresora, int idHueco, int idArticuloFinal, string estado, string motivo, object[] atributos)
        {
            try
            {
                int respuesta = -1;
                double cantidadPalet = 0;
                double cantidadTotal = 0;
                switch (tipoPresentacion)
                {
                    case 0:
                        switch (opcion)
                        {
                            case AltaPalet:
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = numPalets * cantidadEmbalaje;
                                break;

                            case AltaPaletPicos:
                                cantidadTotal = totalUnidadPicos + cantidadEmbalaje;
                                cantidadPalet = totalUnidadPicos + cantidadEmbalaje;
                                break;

                            case AltaPaletMultireferencia:
                            case AltaPaletEntradasTotales:
                            case AltaPaletCarroMovil:
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = cantidadEmbalaje;
                                break;
                        }
                        break;

                    case 1:
                        switch (opcion)
                        {
                            case AltaPalet:
                                cantidadPalet = cantidadEmbalaje * cantidadUnidad;
                                cantidadTotal = numPalets * cantidadPalet;
                                break;

                            case AltaPaletPicos:
                                cantidadPalet = (cantidadEmbalaje * cantidadUnidad) + totalUnidadPicos;
                                cantidadTotal = cantidadPalet;
                                break;

                            case AltaPaletMultireferencia:
                            case AltaPaletCarroMovil:
                            case AltaPaletEntradasTotales:
                                cantidadPalet = (cantidadEmbalaje * cantidadUnidad);
                                cantidadTotal = cantidadPalet;
                                break;
                        }
                        break;

                    case 2:
                        switch (opcion)
                        {
                            case AltaPalet:
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = numPalets * cantidadEmbalaje;
                                cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadPalet);
                                cantidadTotal = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadTotal);
                                object[] pres = Presentaciones.getTipoUnidadPresentacionAlmacenamiento(idPresentacion);
                                idUnidadTipo = pres[1].ToString();
                                break;

                            case AltaPaletPicos:
                                cantidadEmbalaje = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadEmbalaje);
                                cantidadPalet = (cantidadEmbalaje + totalUnidadPicos) / cantidadUnidad;
                                cantidadTotal = (cantidadPalet * cantidadUnidad);
                                cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadPalet);
                                pres = Presentaciones.getTipoUnidadPresentacionAlmacenamiento(idPresentacion);
                                idUnidadTipo = pres[1].ToString();
                                break;

                            case AltaPaletMultireferencia:
                            case AltaPaletCarroMovil:
                            case AltaPaletEntradasTotales:
                                cantidadEmbalaje = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadEmbalaje);
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = cantidadPalet;
                                break;
                        }
                        break;
                }
                /*if (opcion == AltaPaletCarroMovil)
                {
                    cantidadPalet = cantidadEmbalaje;
                    idHueco = Convert.ToInt32(radComboBoxCarro.SelectedValue);
                }
                else
                {
                    if (tipoPresentacion == 2)
                    {
                        cantidadPalet =  cantidadEmbalaje;
                    }
                    else
                    {
                        cantidadPalet = cantidadUnidad * cantidadEmbalaje;
                    }
                }

                if (opcion == AltaPaletPicos) {
                    cantidadTotal = totalUnidadPicos;
                }
                else
                {
                    cantidadTotal = numPalets * cantidadPalet;
                }

                if (tipoPresentacion > 0)
                {
                    if (idPresentacion > 0)
                    {
                        cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadPalet);
                        cantidadTotal = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadTotal);
                        object[] pres = Presentaciones.getTipoUnidadPresentacionAlmacenamiento(idPresentacion);
                        idUnidadTipo = pres[1].ToString();
                    }
                }*/
                int QtyPalet = Convert.ToInt32(cantidadPalet);
                int QtyUnidad = Convert.ToInt32(cantidadUnidad);
                int QtyTotal = Convert.ToInt32(cantidadTotal);
                switch (opcion)
                {
                    case AltaPalet:
                    case AltaPaletPicos:
                        respuesta = wsp.generarEntrada(radTextBoxEAN.Text, idDevolCli, idDevolCliLin, idArticulo, radTextBoxSSCC.Text, ENTRADA_PEDIDO_MOV, QtyPalet, QtyUnidad, QtyTotal, User.IdOperario, idUnidadTipo, idPaletTipo, lote, idHueco, 0, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), 0, atributos, radTextBoxComentario.Text, radTextBoxNSerie.Text, DatosThread.getInstancia().getArrayDatos());
                        break;

                    case AltaPaletMultireferencia:

                        if (int.TryParse(radTextBoxControlEntradaPaletPadre.Text, out int entradaPadre) && entradaPadre > 0)
                        {
                            /*         respuesta = wsp.tratarTotal(radTextBoxEAN.Text, ENTRADA_INCLUIDO_EN_MULTI, "A", idRecepcion, idPedidoPro
                             , idPedidoProLin, idArticulo
                             , idHueco, QtyUnidad, QtyPalet, QtyTotal
                             , "" ,entradaPadre.ToString(), lote, idPaletTipo, radTextBoxSSCC.Text, false, atributos, radTextBoxNSerie.Text, idUnidadTipo, radComboBoxEstado.SelectedValue.ToString(), true,
                              User.IdOperario, 0, !rCheckBoxConReserva.Checked);*/
                            respuesta = wsp.generarEntrada(radTextBoxEAN.Text, idDevolCli, idDevolCliLin, idArticulo, radTextBoxSSCC.Text, ENTRADA_INCLUIDO_EN_MULTI, QtyPalet, QtyUnidad, QtyTotal, User.IdOperario, idUnidadTipo, idPaletTipo, lote, idHueco, entradaPadre, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), 0, atributos, radTextBoxComentario.Text, radTextBoxNSerie.Text, DatosThread.getInstancia().getArrayDatos());

                            MessageBox.Show("Creado! Código: " + respuesta, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(Lenguaje.traduce("Hay que introducir valores numericos en numero de etiquetas y entrada contenedor "), Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return -1;
                        }
                        break;

                    case AltaPaletCarroMovil:
                        /*   respuesta = wsp.tratarTotal(radTextBoxEAN.Text, ENTRADA_CARRO, "A", idRecepcion, idPedidoPro
                   , idPedidoProLin, idArticulo
                   , idHueco, QtyUnidad, QtyPalet, QtyTotal
                   , "", "", lote, idPaletTipo, radTextBoxSSCC.Text, false, atributos, radTextBoxNSerie.Text, idUnidadTipo, radComboBoxEstado.SelectedValue.ToString(), true,
                    User.IdOperario, 0, !rCheckBoxConReserva.Checked);*/
                        respuesta = wsp.generarEntrada(radTextBoxEAN.Text, idDevolCli, idDevolCliLin, idArticulo, radTextBoxSSCC.Text, ENTRADA_CARRO, QtyPalet, QtyUnidad, QtyTotal, User.IdOperario, idUnidadTipo, idPaletTipo, lote, idHueco, 0, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), 0, atributos, radTextBoxComentario.Text, radTextBoxNSerie.Text, DatosThread.getInstancia().getArrayDatos());
                        MessageBox.Show("Creado! Código: " + respuesta, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case AltaPaletEntradasTotales:
                        /* respuesta = wsp.tratarTotal(radTextBoxEAN.Text, ENTRADA_TOTALES, "A", idRecepcion, idPedidoPro
                 , idPedidoProLin, idArticulo
                 , idHueco, QtyUnidad, QtyPalet, QtyTotal
                 , "", "", lote, idPaletTipo, radTextBoxSSCC.Text, false, atributos, radTextBoxNSerie.Text, idUnidadTipo, radComboBoxEstado.SelectedValue.ToString(), true,
                  User.IdOperario, 0, !rCheckBoxConReserva.Checked);*/
                        respuesta = wsp.generarEntrada(radTextBoxEAN.Text, idDevolCli, idDevolCliLin, idArticulo, radTextBoxSSCC.Text, ENTRADA_TOTALES, QtyPalet, QtyUnidad, QtyTotal, User.IdOperario, idUnidadTipo, idPaletTipo, lote, idHueco, 0, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), 0, atributos, radTextBoxComentario.Text, radTextBoxNSerie.Text, DatosThread.getInstancia().getArrayDatos());
                        break;
                }
                MessageBox.Show(Lenguaje.traduce("Creado! Código: ") + respuesta, Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return respuesta;
            }
            catch (Exception e)
            {
                ExceptionManager.GestionarError(e, e.Message);
            }

            return 0;
        }

        private void CargarConfiguracionVisual(int opcion)
        {
            rumLabelNumPalets.Enabled = true;
            radTextBoxControlNumPalets.Enabled = true;

            rumLabelDE.Enabled = true;
            radTextBoxControlUdsCaja.Enabled = true;
            radComboBoxUnidadTipo.Enabled = true;

            rumLabelEmbalaje.Enabled = true;
            radTextBoxEmbalaje.Enabled = true;

            rumLabelLote.Enabled = true;
            radTextBoxControlLote.Enabled = false;

            rumLabelTipoPalet.Enabled = true;
            radComboBoxTipoPalet.Enabled = true;

            radTextBoxControlTotalUdsPicos.Enabled = true;
            rumLabelTotalUdsPicos.Enabled = true;

            radTextBoxControlEntradaPaletPadre.Enabled = true;
            rumLabelEntradaPaletPadre.Enabled = true;

            rumLabelEstado.Enabled = true;
            radComboBoxEstado.Enabled = true;

            rumLabelEtiquetas.Enabled = true;
            radTextBoxControlEtiquetas.Enabled = true;

            rumLabelCarro.Enabled = true;
            radComboBoxCarro.Enabled = true;
            if (!mostrarSSCC)
            {
                rumLabelSSCC.Visible = false;
                radTextBoxSSCC.Visible = false;
            }
            if (!capturarEAN)
            {
                rumLabelEAN.Visible = false;
                radTextBoxEAN.Visible = false;
            }

            switch (opcion)
            {
                case AltaPalet://Caso Entrada Mono
                    rumLabelEtiquetas.Enabled = false;
                    radTextBoxControlEtiquetas.Enabled = false;

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;

                    rumLabelCarro.Enabled = false;
                    radComboBoxCarro.Enabled = false;

                    break;

                case AltaPaletPicos://Caso Entrada Picos
                    rumLabelNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Text = "1";

                    rumLabelEtiquetas.Enabled = false;
                    radTextBoxControlEtiquetas.Enabled = false;

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    rumLabelCarro.Enabled = false;
                    radComboBoxCarro.Enabled = false;

                    break;

                case AltaPaletMultireferencia://Caso Entrada Multiproducto
                    rumLabelEtiquetas.Enabled = false;
                    radTextBoxControlEtiquetas.Enabled = false;

                    radTextBoxControlEntradaPaletPadre.Enabled = true;
                    rumLabelEntradaPaletPadre.Enabled = true;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;

                    rumLabelCarro.Enabled = false;
                    radComboBoxCarro.Enabled = false;

                    rumButtonCrearEntradaPadre.Enabled = true;
                    rumButtonCrearEntradaPadre.Visible = true;

                    break;

                case AltaPaletCarroMovil://Caso Entrada Carro Movil
                    rumLabelNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Text = "1";

                    rumLabelEtiquetas.Enabled = false;
                    radTextBoxControlEtiquetas.Enabled = false;

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;

                    break;

                case AltaPaletEntradasTotales://Caso Entradas Totales
                    radTextBoxControlNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Text = "1";

                    rumLabelEtiquetas.Enabled = false;
                    radTextBoxControlEtiquetas.Enabled = false;

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;

                    rumLabelTipoPalet.Enabled = false;
                    radComboBoxTipoPalet.Enabled = false;
                    rumLabelCarro.Text = strings.Ubicacion;
                    break;
            }
        }

        private int ComprobarExistenciaLotes()
        {
            try
            {
                rumLabelExisteLote.Visible = true;
                rumLabelExisteLote.Enabled = true;
                rumLabelExisteLote.Text = "Cargando";
                WSLotesClient wsLotes = new WSLotesClient();
                Object[] loteArticulo = wsLotes.getLoteArticulo(idArticulo, radTextBoxControlLote.Text);
                if (loteArticulo != null)
                {
                    rumLabelExisteLote.Text = "El lote existe";
                    existeLote = true;
                    return 1;
                }
                else
                {
                    rumLabelExisteLote.Text = "No existe el lote";
                    existeLote = false;
                    return 0;
                }
            }
            catch (Exception ex)
            {
                rumLabelExisteLote.Text = "Error consultando lote";
                ExceptionManager.GestionarError(ex);
                existeLote = false;
                return -1;
            }
        }

        #endregion Metodos

        #region MetodosPredefinidosYBotones

        private void FormularioAltaProducto_Load(object sender, EventArgs e)
        {
            if (conLineas)
            {
                CargarDatosDefectoAtributos();
            }
        }

        private void radComboBoxArticulo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                idArticuloSeleccionado = Convert.ToInt32(radComboBoxArticulo.SelectedValue);
                if (idArticuloSeleccionado <= 0)
                {
                    verCampos(false);
                    return;
                }
                verCampos(true);
                //CargarDatosDefectoAtributos();
                CargarConfiguracionVisual(opcion);
                CargarDatosDefecto();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                log.Info("Procediendo a realizar Alta Producto");
                String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
                bool errorBool = false;
                if (!int.TryParse(radTextBoxControlNumPalets.Text, out int numPalets))
                    errorBool = true;
                if (!double.TryParse(radTextBoxEmbalaje.Text, out double totalEmbalaje))
                    errorBool = true;
                if (!double.TryParse(radTextBoxControlUdsCaja.Text, out double udsCaja))
                    errorBool = true;
                if (!double.TryParse(radTextBoxControlTotalUdsPicos.Text, out double totalUdsPicos))
                    if (opcion == AltaPaletPicos)
                    {
                        if (radTextBoxControlTotalUdsPicos.Text.Equals(""))
                        {
                            totalUdsPicos = 0;
                        }
                        else
                        {
                            errorBool = true;
                        }
                    }
                    else
                    {
                        totalUdsPicos = totalEmbalaje;
                    }

                if (errorBool == true)
                {
                    MessageBox.Show(errorMensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Info("Comprobación de campos erronea.");
                    return;
                }

                int muelle = int.Parse(ConexionSQL.getDataTable("SELECT TOP 1 MUELLE FROM TBLDEVOLCLICAB WHERE IDDEVOLCLI = " + idDevolCli).Rows[0][0].ToString());
                if (opcion == AltaPaletCarroMovil || opcion == AltaPaletEntradasTotales)
                {
                    muelle = Convert.ToInt32(radComboBoxCarro.SelectedValue);
                }
                Object[][] atributos = ComprobracionAtributos();
                if (atributos != null && (atributos[0][0] as String).Equals("Error"))
                {
                    MessageBox.Show(Lenguaje.traduce("Falta un atributo obligatorio"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int resultado = 0;
                if (radTextBoxControlLote.Enabled)
                {
                    if (existeLote == true)
                    {
                        resultado = LlamarWebService(totalUdsPicos, totalEmbalaje, udsCaja, numPalets, User.IdOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                        radTextBoxControlLote.Text, User.NombreImpresora, muelle, idArticulo, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), atributos);
                    }
                    else
                    {
                        FormularioCrearLote formLote;
                        DateTime fechaCaducidad = new DateTime();

                        string fecha = null;
                        if (fechaCaducidadCalculada != null)
                        {
                            fecha = fechaCaducidadCalculada.ToString();
                        }
                        if (fecha == null)
                        {
                            formLote = new FormularioCrearLote(idArticulo, radTextBoxControlLote.Text, 0);
                        }
                        else
                        {
                            fechaCaducidad = DateTime.Parse(fecha);
                            formLote = new FormularioCrearLote(idArticulo, radTextBoxControlLote.Text, 0, fechaCaducidad);
                        }

                        formLote.ShowDialog();
                        Object[] resultadoLote = formLote.Resultado;
                        if (resultadoLote != null)
                        {
                            String loteCreado = (resultadoLote[2] as object[])[1].ToString();
                            resultado = LlamarWebService(totalUdsPicos, totalEmbalaje, udsCaja, numPalets, User.IdOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                            loteCreado, User.NombreImpresora, muelle, idArticulo, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), atributos);
                        }
                        else
                        {
                            MessageBox.Show(Lenguaje.traduce("Error creando el lote"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    resultado = LlamarWebService(totalUdsPicos, totalEmbalaje, udsCaja, numPalets, User.IdOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                    radTextBoxControlLote.Text, User.NombreImpresora, muelle, idArticulo, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), atributos);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            return;
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadTextBoxControlLote_Leave(object sender, EventArgs e)
        {
            ComprobarExistenciaLotes();
        }

        private void radComboBoxUnidadTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tipoPresentacion != 0)
            {
                if (radComboBoxUnidadTipo.SelectedValue != null)
                {
                    string idUnidadTipoPres = radComboBoxUnidadTipo.SelectedValue.ToString();
                    object[] pres = Presentaciones.getTipoUnidadPresentacion(idPresentacion, idUnidadTipoPres);
                    if (pres != null)
                    {
                        if (tipoPresentacion == 2)
                        {
                            object[] presCU = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(pres[0]));
                            radTextBoxControlUdsCaja.Text = presCU[0].ToString();
                        }
                        else
                        {
                            radTextBoxControlUdsCaja.Text = String.Format("{0:N0}", pres[0]);
                        }
                    }
                }
            }
        }

        private void CargarDatosDefectoAtributosClave(DataRow valoresOrdenFila)
        {
            try
            {
                if (listaAtributos == null || listaAtributos.Count < 1)
                {
                    return;
                }
                for (int i = 0; i < tableLayoutPanelAtributos.RowCount; i++)
                {
                    String nombre = (tableLayoutPanelAtributos.GetControlFromPosition(0, i) as RumLabel).Text.Replace(":", String.Empty);
                    RadTextBox text = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox);
                    String nombreAtributo = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox).Tag.ToString();
                    OAtributo atrib = listaAtributos.Find(atr => atr.Nombre.Equals(nombre));
                    if (atrib.AtribPers != null)
                    {
                        text.Text = valoresOrdenFila[atrib.AtribPers.CampoTabla].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion MetodosPredefinidosYBotones
    }
}
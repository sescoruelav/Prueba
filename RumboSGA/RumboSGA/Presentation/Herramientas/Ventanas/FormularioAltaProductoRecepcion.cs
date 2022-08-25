using Rumbo.Core.Herramientas;
using Rumbo.Core.Herramientas.Herramientas;
using RumboSGA.EntradaMotor;
using RumboSGA.LotesMotor;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGA.Presentation.Herramientas.FormulariosComunes;
using RumboSGA.Presentation.Herramientas.PantallasWS;
using RumboSGA.ProduccionMotor;
using RumboSGA.Properties;
using RumboSGA.RecepcionMotor;
using RumboSGA.VariablesMotor;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Constantes;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FormularioAltaProductoRecepcion : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        private int opcion, idArticulo;
        private int idRecepcion;
        private int idPedidoPro;
        private int idPedidoProLin;
        private List<OAtributo> listaAtributos;
        private bool existeLote;

        public const int AltaPalet = 1;
        public const int AltaPaletPicos = 2;
        public const int AltaPaletMultireferencia = 3;
        public const int AltaPaletCarroMovil = 4;
        public const int AltaPaletEntradasTotales = 5;
        public const int AltaCheps = 6;
        public const int ENTRADA_PEDIDO_MOV = 3;
        public const int ENTRADA_TOTALES = 4;
        public const int ENTRADA_MULTIREFERENCIA = 5;
        public const int ENTRADA_INCLUIDO_EN_MULTI = 7;

        /// <summary>
        /// se utiliza en el coteo carro movil
        /// </summary>
        public const int ENTRADA_CARRO = 6;

        private WSVariablesClient wsv = new WSVariablesClient();
        private WSRecepcionMotorClient wsp = new WSRecepcionMotorClient();
        private int tipoPresentacion = 0;
        private int idPresentacion = 0;
        private bool mostrarSSCC = false;
        private bool capturarEAN = false;
        private bool capturarNSerie = false;
        private DateTime? fechaCaducidadCalculada = null;
        private string loteEstricto;
        private bool registrarPeso = false;

        #endregion Variables

        public FormularioAltaProductoRecepcion(int opcion, int idRecepcion, int idPedidoPro, int idPedidoProLin, string nombreOpcion, Image imagenIcono)
        {
            listaAtributos = null;
            existeLote = false;
            InitializeComponent();
            this.opcion = opcion;
            this.idRecepcion = idRecepcion;
            this.idPedidoPro = idPedidoPro;
            this.idPedidoProLin = idPedidoProLin;
            CargarParametrizacion();
            CargarConfiguracionVisual(opcion);
            CargarDatosDefectoAtributos();
            CargarDatosDefecto();
            // this.Text = Lenguaje.traduce("Alta Producto");
            this.rumLabelImagen.Image = imagenIcono;
            this.rumLabelTipoAlta.Text = nombreOpcion;
            this.Icon = Resources.bruju;
            this.rCheckBoxConReserva.Text = Lenguaje.traduce(rCheckBoxConReserva.Text);
        }

        private void CargarParametrizacion()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        #region Metodos

        private int CargarDatosDefecto()
        {
            try
            {
                List<TableScheme> listaResultado = new List<TableScheme>();
                DataRow valoresOrdenFila = DataAccess.GetAltaArticuloDefectoPedidoPro(idPedidoPro, idPedidoProLin).Rows[0];
                if (valoresOrdenFila == null)
                {
                    MessageBox.Show(Lenguaje.traduce("No ha devuelto nada"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

                //Presentaciones
                idPresentacion = Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]);
                int tipoPresArticulo = Convert.ToInt32(valoresOrdenFila["TIPOPRESENTACION"]);
                if (tipoPresArticulo >= 0)
                {
                    tipoPresentacion = tipoPresArticulo;
                }
                this.idArticulo = int.Parse(valoresOrdenFila["IDARTICULO"].ToString());
                if (tipoPresentacion == 0 || idPresentacion == 0)
                {
                    radTextBoxControlUdsCaja.Text = String.Format("{0:N0}", valoresOrdenFila["CANTIDADUNIDADARTICULO"]);
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, DataAccess.GetIdDescripcionUnidadesTipo(), "IDUNIDADTIPO", "DESCRIPCION", valoresOrdenFila["IDUNIDADTIPOARTICULO"].ToString(), null, true);
                    if (opcion == AltaPalet)
                    {
                        radTextBoxEmbalaje.Text = String.Format("{0:N0}", valoresOrdenFila["CANTIDADARTICULO"]);
                    }
                }
                else if (tipoPresentacion == 1)
                {
                    if (String.IsNullOrEmpty(valoresOrdenFila["IDUNIDADTIPO"].ToString().Trim())) // Si la unidad tipo del pedido no viene definida pongo la del articulo
                    {
                        Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPOARTICULO"].ToString());
                    }
                    else
                    {
                        Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPO"].ToString());
                    }
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

                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPO"].ToString());
                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, idUnidadTipoPres);
                    object[] presCU = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(pres[0]));
                    radTextBoxControlUdsCaja.Text = String.Format("{0:N0}", presCU[0]);
                    if (opcion == AltaPalet)
                    {
                        object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(valoresOrdenFila["CANTIDADARTICULO"]));
                        radTextBoxEmbalaje.Text = String.Format("{0:N0}", presC[0]);
                    }
                }
                //TipoPalet
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxTipoPalet, DataAccess.GetIdDescripcionTipoPalets(), "IDPALETTIPO", "DESCRIPCION", valoresOrdenFila["IDPALETTIPO"].ToString(), new String[] { "TODOS" }, true);

                //Lote
                loteEstricto = valoresOrdenFila["LOTEESTRICTO"].ToString();
                if (loteEstricto.Equals(ConstArticulosConfLote.NO_CONFIRMAR_FECHA) || loteEstricto.Equals(ConstArticulosConfLote.SI))
                {
                    RadTextBoxControlLote.Enabled = true;
                    CalcularFechaCaducidad(Convert.ToInt32(valoresOrdenFila["DIASCADUCIDAD"]));
                }

                //Estados Existencia
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxEstado, DataAccess.GetIdDescripcionExistenciaEstados(), "IDEXISTENCIAESTADO", "DESCRIPCION", valoresOrdenFila["ESTADOENTRADA"].ToString(), new String[] { "TODOS" }, true);
                //Carro
                if (opcion == AltaPaletCarroMovil)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxCarro, DataAccess.GetIdDescripcionCarrosMovil(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                    rCheckBoxConReserva.Visible = false;
                }
                if (opcion == AltaPaletEntradasTotales)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxCarro, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                    rCheckBoxConReserva.Visible = false;
                }
                if (opcion == AltaPaletMultireferencia)
                {
                    rCheckBoxConReserva.Visible = false;
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
                string registrarPesoStr = valoresOrdenFila["REGISTRARPESO"].ToString();
                if (registrarPesoStr.Substring(0, 1) == "S")
                {
                    registrarPeso = true;
                }
                if (!registrarPeso)
                {
                    radTextBoxPeso.Visible = false;
                    lblPeso.Visible = false;
                }
                CargarDatosDefectoAtributosClave(valoresOrdenFila);

                this.Text = Lenguaje.traduce("Alta Producto " + valoresOrdenFila["DESCRIPCION"]);
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
                ExceptionManager.GestionarError(e);
            }

            return 0;
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

        private void CalcularFechaCaducidad(int diasCaducidad)
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
                    if (atributo.Clave)
                    {
                        radTextBoxAtributo.Enabled = false;

                        //rumLabelAtributo.Visible = false;
                    }

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
            string impresora, int idHueco, int idArticuloFinal, string estado, object[] atributos, double peso)
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
                                cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadPalet);
                                cantidadTotal = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadTotal);
                                object[] pres = Presentaciones.getTipoUnidadPresentacion(idPresentacion, idUnidadTipo);
                                idUnidadTipo = pres[1].ToString();
                                cantidadUnidad = double.Parse(pres[0].ToString());

                                break;

                            case AltaPaletPicos:
                                cantidadEmbalaje = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadEmbalaje);
                                cantidadPalet = (cantidadEmbalaje + totalUnidadPicos) / cantidadUnidad;
                                cantidadTotal = (cantidadPalet * cantidadUnidad);
                                cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadPalet);
                                cantidadUnidad = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadUnidad);
                                object[] pres1 = Presentaciones.getTipoUnidadPresentacion(idPresentacion, idUnidadTipo);
                                idUnidadTipo = pres1[1].ToString();
                                cantidadUnidad = double.Parse(pres1[0].ToString());

                                break;

                            case AltaPaletMultireferencia:
                            case AltaPaletCarroMovil:
                            case AltaPaletEntradasTotales:
                                cantidadEmbalaje = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadEmbalaje);
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = cantidadPalet;
                                object[] pres2 = Presentaciones.getTipoUnidadPresentacion(idPresentacion, idUnidadTipo);
                                idUnidadTipo = pres2[1].ToString();
                                cantidadUnidad = double.Parse(pres2[0].ToString());
                                break;
                        }
                        break;
                }

                if (opcion == AltaPaletCarroMovil)
                {
                    cantidadPalet = cantidadEmbalaje;
                }
                else
                {
                    if (tipoPresentacion == 0)
                    {
                        cantidadPalet = Math.Ceiling(cantidadEmbalaje / cantidadUnidad);
                    }
                    else
                    {
                        cantidadPalet = cantidadEmbalaje;
                    }
                }

                if (opcion == AltaPaletPicos)
                {
                    if (tipoPresentacion == 0)
                    {
                        cantidadTotal = totalUnidadPicos + cantidadEmbalaje;
                    }
                    else
                    {
                        cantidadPalet = cantidadEmbalaje * cantidadUnidad;
                        cantidadTotal = (cantidadPalet) + totalUnidadPicos;
                    }
                }
                else
                {
                    if (tipoPresentacion == 0)
                    {
                        cantidadTotal = numPalets * cantidadEmbalaje;
                    }
                    else if (tipoPresentacion == 1)
                    {
                        cantidadTotal = numPalets * cantidadPalet * cantidadUnidad;
                    }
                    else if (tipoPresentacion == 2)
                    {
                        cantidadTotal = numPalets * cantidadPalet;
                    }
                }

                if (tipoPresentacion == 2)
                {
                    cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadPalet);
                    cantidadTotal = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadTotal);
                    object[] pres = Presentaciones.getTipoUnidadPresentacionAlmacenamiento(idPresentacion);
                    idUnidadTipo = pres[1].ToString();
                }

                int QtyPalet = Convert.ToInt32(cantidadPalet);
                int QtyUnidad = Convert.ToInt32(cantidadUnidad);
                int QtyTotal = Convert.ToInt32(cantidadTotal);
                switch (opcion)
                {
                    case AltaPalet:
                    case AltaPaletPicos:
                        respuesta = wsp.tratarTotal(radTextBoxEAN.Text, ENTRADA_PEDIDO_MOV, "A", idRecepcion, idPedidoPro
                    , idPedidoProLin, idArticulo
                    , idHueco, QtyUnidad, QtyPalet, QtyTotal
                    , "", "", lote, idPaletTipo, radTextBoxSSCC.Text, false, atributos, radTextBoxNSerie.Text, idUnidadTipo, radComboBoxEstado.SelectedValue.ToString(), false, DatosThread.getInstancia().getArrayDatos(),
                     User.IdOperario, 0, !rCheckBoxConReserva.Checked, peso);

                        break;

                    case AltaPaletMultireferencia:

                        if (int.TryParse(radTextBoxControlEntradaPaletPadre.Text, out int entradaPadre) && entradaPadre > 0)
                        {
                            respuesta = wsp.tratarTotal(radTextBoxEAN.Text, ENTRADA_INCLUIDO_EN_MULTI, "A", idRecepcion, idPedidoPro
                    , idPedidoProLin, idArticulo
                    , idHueco, QtyUnidad, QtyPalet, QtyTotal
                    , "", entradaPadre.ToString(), lote, idPaletTipo, radTextBoxSSCC.Text, false, atributos, radTextBoxNSerie.Text, idUnidadTipo, radComboBoxEstado.SelectedValue.ToString(), false, DatosThread.getInstancia().getArrayDatos(),
                     User.IdOperario, 0, !rCheckBoxConReserva.Checked, peso);
                            MessageBox.Show("Creado! Código: " + respuesta, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(Lenguaje.traduce("Hay que introducir valores numericos en numero de etiquetas y entrada contenedor "), Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return -1;
                        }
                        break;

                    case AltaPaletCarroMovil:

                        break;

                    case AltaPaletEntradasTotales:
                        respuesta = wsp.tratarTotal(radTextBoxEAN.Text, ENTRADA_TOTALES, "A", idRecepcion, idPedidoPro
                , idPedidoProLin, idArticulo
                , idHueco, QtyUnidad, QtyPalet, QtyTotal
                , "", "", lote, idPaletTipo, radTextBoxSSCC.Text, false, atributos, radTextBoxNSerie.Text, idUnidadTipo, radComboBoxEstado.SelectedValue.ToString(), true, DatosThread.getInstancia().getArrayDatos(),
                 User.IdOperario, 0, !rCheckBoxConReserva.Checked, peso);
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
            RadTextBoxControlLote.Enabled = false;
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
                Object[] loteArticulo = wsLotes.getLoteArticulo(idArticulo, RadTextBoxControlLote.Text);
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
        }

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            try
            {
                int resultado = 0;
                double peso = 0;
                log.Info("Procediendo a realizar Alta Producto" + idArticulo);
                String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
                //Validamos inputs
                bool errorBool = false;
                if (!int.TryParse(radTextBoxControlNumPalets.Text, out int numPalets))
                    errorBool = true;
                if (!double.TryParse(radTextBoxEmbalaje.Text, out double totalEmbalaje))
                    errorBool = true;
                if (!double.TryParse(radTextBoxControlUdsCaja.Text, out double udsCaja))
                    errorBool = true;
                if (!double.TryParse(radTextBoxControlTotalUdsPicos.Text, out double totalUdsPicos))
                    if (RadTextBoxControlLote.Enabled && RadTextBoxControlLote.Text.Equals(""))
                    {
                        MessageBox.Show("Debe introducir el lote", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                if (radTextBoxPeso.Visible && radTextBoxPeso.Text.Equals(""))
                {
                    MessageBox.Show("Debe introducir el peso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (radTextBoxPeso.Visible && !double.TryParse(radTextBoxPeso.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out peso))
                    errorBool = true;
                if (opcion == AltaPaletPicos)
                {
                    if (radTextBoxControlTotalUdsPicos.Text.Equals(""))
                    {
                        totalUdsPicos = 0;
                    }
                    /*else
                    {
                        totalUdsPicos = totalEmbalaje;
                    }*/
                }
                else
                {
                    totalUdsPicos = totalEmbalaje;
                }
                if (errorBool)
                {
                    MessageBox.Show(errorMensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Info("Comprobación de campos erronea.");
                    return;
                }

                //Obtenemos muelle o carro o ubicacion totales
                int muelle = DataAccess.GetMuelleRecepcion(idRecepcion);//int.Parse(ConexionSQL.getDataTable("SELECT TOP 1 MUELLE FROM TBLRECEPCIONESCAB WHERE IDRECEPCION = " + idRecepcion).Rows[0][0].ToString());
                if (opcion == AltaPaletCarroMovil || opcion == AltaPaletEntradasTotales)
                {
                    muelle = Convert.ToInt32(radComboBoxCarro.SelectedValue);
                }
                //Extraemos valor de atributos
                Object[][] atributos = ComprobracionAtributos();
                if (atributos != null && (atributos[0][0] as String).Equals("Error"))
                {
                    MessageBox.Show(Lenguaje.traduce("Falta un atributo obligatorio"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (RadTextBoxControlLote.Enabled)
                {
                    if (existeLote == true)
                    {
                        resultado = LlamarWebService(totalUdsPicos, totalEmbalaje, udsCaja, numPalets, User.IdOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                        RadTextBoxControlLote.Text, User.NombreImpresora, muelle, idArticulo, radComboBoxEstado.SelectedValue.ToString(), atributos, peso);
                    }
                    else
                    {
                        Object[] resultadoLote = null;
                        if (loteEstricto.Equals(ConstArticulosConfLote.SI))
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
                                formLote = new FormularioCrearLote(idArticulo, RadTextBoxControlLote.Text, 0);
                            }
                            else
                            {
                                fechaCaducidad = DateTime.Parse(fecha);
                                formLote = new FormularioCrearLote(idArticulo, RadTextBoxControlLote.Text, 0, fechaCaducidad);
                            }

                            formLote.ShowDialog();
                            resultadoLote = formLote.Resultado;
                        }
                        else
                        {
                            if (loteEstricto.Equals(ConstArticulosConfLote.NO_CONFIRMAR_FECHA))
                            {
                                WSLotesClient LotesMotor = new WSLotesClient();
                                resultadoLote = LotesMotor.insertarLoteConArticulo(idArticulo, DateTime.Now, RadTextBoxControlLote.Text, 0);
                            }
                        }

                        if (resultadoLote != null)
                        {
                            String loteCreado = (resultadoLote[2] as object[])[1].ToString();
                            resultado = LlamarWebService(totalUdsPicos, totalEmbalaje, udsCaja, numPalets, User.IdOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                            loteCreado, User.NombreImpresora, muelle, idArticulo, radComboBoxEstado.SelectedValue.ToString(), atributos, peso);
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
                    RadTextBoxControlLote.Text, User.NombreImpresora, muelle, idArticulo, radComboBoxEstado.SelectedValue.ToString(), atributos, peso);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadComboBoxUnidadTipo_SelectedIndexChanged(object sender, EventArgs e)
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

        private void RadTextBoxControlLote_Leave(object sender, EventArgs e)
        {
            ComprobarExistenciaLotes();
        }

        #endregion MetodosPredefinidosYBotones
    }
}
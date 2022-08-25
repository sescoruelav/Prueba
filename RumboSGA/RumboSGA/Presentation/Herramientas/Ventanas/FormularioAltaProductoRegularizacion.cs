using Rumbo.Core.Herramientas;
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
    public partial class FormularioAltaProductoRegularizacion : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int resultado = -1;

        #region Variables

        private int opcion;
        private int idArticuloSeleccionado;

        //private DataTable dataTableUnidadesTipo, dataTableTipoPalet,
        //    dataTableOperarios, dataTableEstados, dataTableCarro;
        private List<OAtributo> listaAtributos;

        private bool existeLote;

        public const int AltaPalet = 1;
        public const int AltaPaletPicos = 2;
        public const int AltaPaletMultireferencia = 3;
        public const int AltaPaletCarroMovil = 4;
        public const int AltaPaletEntradasTotales = 5;
        private WSVariablesClient wsv = new WSVariablesClient();
        private WSEntradaMotorClient wsp = new WSEntradaMotorClient();
        private int tipoPresentacion = 0;
        private int idPresentacion = 0;
        private bool mostrarSSCC = false;
        private bool capturarEAN = false;
        private bool capturarNSerie = false;
        private DateTime? fechaCaducidadCalculada = null;
        private string loteEstricto;
        private bool registrarPeso = false;

        #endregion Variables

        public FormularioAltaProductoRegularizacion(int opcion, string nombreOpcion, Image imagenIcono)
        {
            listaAtributos = null;
            existeLote = false;
            InitializeComponent();
            this.opcion = opcion;
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
            //CargarConfiguracionVisual(opcion);
            this.Text = Lenguaje.traduce("Alta Producto Ajuste");
            this.rCheckBoxConReserva.Text = Lenguaje.traduce("Con Movimiento");
            this.rCheckBoxEtiqueta.Text = Lenguaje.traduce("Imprimir Etiqueta");
            this.rumLabelImagen.Image = imagenIcono;
            this.rumLabelTipoAlta.Text = nombreOpcion;
            this.Icon = Resources.bruju;
            try
            {
                CargarDatosCombos();
                CargarDatosDefectoAtributos();
                verCampos(false);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        #region Metodos

        //private int CargarPresentaciones()
        //{
        //    //Comprobar si se necesita aplicar presentaciones
        //    DataTable r = ConexionSQL.getDataTable("SELECT TIPOPRESENTACION FROM TBLARTICULOS WHERE IDARTICULO = "+idArticuloFinal);
        //    int tipoPresentacionArticulo = 0;
        //    int tipoPresentacion = 0 ;
        //    if (r != null && r.Rows.Count > 0)
        //    {
        //        tipoPresentacionArticulo = int.Parse(r.Rows[0][0].ToString());
        //    }

        //    if(tipoPresentacionArticulo>0 || tipoPresentacion > 0)
        //    {
        //        if (tipoPresentacionArticulo > 0)
        //        {
        //        }
        //    }

        //    return 0;
        //}
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

        private void cargaDatosArticulo()
        {
            try
            {
                if (idArticuloSeleccionado <= 0)
                {
                    verCampos(false);
                    return;
                }
                verCampos(true);
                //CargarDatosDefectoAtributos();
                CargarConfiguracionVisual(opcion);
                DataRow valoresOrdenFila = DataAccess.GetAltaArticuloDefecto(idArticuloSeleccionado).Rows[0];
                if (valoresOrdenFila == null)
                {
                    MessageBox.Show(Lenguaje.traduce("No ha devuelto nada"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                idPresentacion = Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]);
                int tipoPresArticulo = Convert.ToInt32(valoresOrdenFila["TIPOPRESENTACION"]);
                if (tipoPresArticulo > 0)
                {
                    tipoPresentacion = tipoPresArticulo;
                }

                if (tipoPresentacion == 0 || idPresentacion == 0)
                {
                    radTextBoxControlUdsCaja.Text = valoresOrdenFila["CANTIDADUNIDADARTICULO"].ToString();
                    if (radTextBoxControlUdsCaja.Text.Contains(".") || radTextBoxControlUdsCaja.Text.Contains(","))
                    {
                        radTextBoxControlUdsCaja.Text = "1";
                    }
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, DataAccess.GetIdDescripcionUnidadesTipo(), "IDUNIDADTIPO", "DESCRIPCION", valoresOrdenFila["IDUNIDADTIPOARTICULO"].ToString(), null, true);
                    if (opcion == AltaPalet)
                    {
                        radTextBoxEmbalaje.Text = valoresOrdenFila["CANTIDADUNIDADARTICULO"].ToString();
                    }
                }
                else
                {
                    object[] pres = Presentaciones.getTipoUnidadPresentacionVisualizacion(idPresentacion);
                    string idUnidadTipoPres = pres[1].ToString();
                    radTextBoxControlUdsCaja.Text = pres[0].ToString();
                    if (radTextBoxControlUdsCaja.Text.Contains(".") || radTextBoxControlUdsCaja.Text.Contains(","))
                    {
                        string[] tabla = pres[0].ToString().Split(',');
                        radTextBoxControlUdsCaja.Text = tabla[0];
                    }
                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPOARTICULO"].ToString()); Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, idUnidadTipoPres);
                    if (opcion == AltaPalet)
                    {
                        object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(valoresOrdenFila["CANTIDADARTICULO"]));
                        radTextBoxEmbalaje.Text = presC[0].ToString();
                    }
                }

                //Unidades Tipo Embalaje
                //Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, DataAccess.GetIdDescripcionUnidadesTipo(), "IDUNIDADTIPO", "DESCRIPCION", valoresOrdenFila["IDUNIDADTIPO"].ToString(), new String[] { "TODOS" });
                //Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPO"].ToString());
                //TipoPalet
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxTipoPalet, DataAccess.GetIdDescripcionTipoPalets(), "IDPALETTIPO", "DESCRIPCION", valoresOrdenFila["IDPALETTIPO"].ToString(), new String[] { "TODOS" }, true);
                loteEstricto = valoresOrdenFila["LOTEESTRICTO"].ToString();
                //Lote
                if (valoresOrdenFila["LOTEESTRICTO"].Equals("L") || valoresOrdenFila["LOTEESTRICTO"].Equals("S"))
                {
                    radTextBoxControlLote.Enabled = true;
                    calcularFechaCaducidad(Convert.ToInt32(valoresOrdenFila["DIASCADUCIDAD"]));
                }

                //Estados Existencia
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxEstado, DataAccess.GetIdDescripcionExistenciaEstados(), "IDEXISTENCIAESTADO", "DESCRIPCION", valoresOrdenFila["ESTADOENTRADA"].ToString(), new String[] { "TODOS" }, true);

                //Carro
                if (opcion == AltaPaletCarroMovil)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionCarrosMovil(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                    rumLabelUbicacion.Text = "Carro";
                }
                if (opcion == AltaPalet || opcion == AltaPaletPicos || opcion == AltaPaletMultireferencia)
                {
                    if (rCheckBoxConReserva.Checked)
                    {
                        Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionHuecoMuelle(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                        rumLabelUbicacion.Text = "Origen";
                    }
                    else
                    {
                        Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                        rumLabelUbicacion.Text = "Ubicacion";
                    }
                }
                if (opcion == 5)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                }

                //Operarios
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxMotivo, DataAccess.GetIdDescripcionMotivosTipo("ER"), "IDMOTIVO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
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
            }
            catch (Exception e)
            {
                log.Error("Mensaje:" + e.Message + "||StackTrace:" + e.StackTrace);
                ExceptionManager.GestionarError(e);
            }
        }

        private int CargarDatosCombos()
        {
            try
            {
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
                String nombreAtributo = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox).Tag.ToString();
                String valor = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox).Text;
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
            if (idArticuloSeleccionado <= 0)
            {
                return;
            }
            FormularioCrearPaletMultireferencia fcp = new FormularioCrearPaletMultireferencia(radComboBoxTipoPalet.SelectedValue.ToString());

            fcp.ShowDialog();
            if (fcp.resultado != null) radTextBoxControlEntradaPaletPadre.Text = fcp.resultado;
        }

        private int CargarDatosDefectoAtributos()
        {
            try
            {
                /* object[] resultadoCompuesto = wsv.cargarDatos(new String[] { DataAccess.CARGA_INI_ATRIBUTOS_ENTRADAS });
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
            string impresora, int muelle, int idArticuloFinal, string estado, String motivo, object[] atributos, double peso)
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
                /*if (opcion == AltaPaletCarroMovil)
                {
                    cantidadPalet = cantidadEmbalaje;
                }
                else
                {
                    if (tipoPresentacion == 2)
                    {
                        cantidadPalet = cantidadEmbalaje;
                    }
                    else
                    {
                        cantidadPalet = cantidadUnidad * cantidadEmbalaje;
                    }
                }

                double cantidadTotal = numPalets * cantidadPalet;
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
                int tipoReg;
                if (rCheckBoxConReserva.Checked)
                {
                    tipoReg = 2;
                }
                else
                {
                    tipoReg = 1;
                }
                switch (opcion)
                {
                    case AltaPalet:
                        /* respuesta = wsp.generarEntradaPaletsRegularizacion(tipoReg, QtyPalet, QtyUnidad, numPalets, idOperario, idUnidadTipo,
                             idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado, motivo, rCheckBoxConReserva.Checked, rCheckBoxEtiqueta.Checked, radTextBoxSSCC.Text, radTextBoxNSerie.Text, radTextBoxComentario.Text, radTextBoxEAN.Text, atributos,peso);*/
                        break;

                    case AltaPaletPicos:
                        /* respuesta = wsp.generarEntradaPaletsPicoRegularizacion(tipoReg, QtyPalet, QtyUnidad, QtyTotal, idOperario, idUnidadTipo,
                              idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado, motivo, rCheckBoxConReserva.Checked, rCheckBoxEtiqueta.Checked, radTextBoxSSCC.Text, radTextBoxNSerie.Text, radTextBoxComentario.Text, radTextBoxEAN.Text, atributos, peso);*/
                        break;

                    case AltaPaletMultireferencia:
                        if (int.TryParse(radTextBoxControlEntradaPaletPadre.Text, out int entradaPadre) && entradaPadre > 0)
                        {
                            /* respuesta = wsp.generarEntradaPaletsMultiRegularizacion(tipoReg, QtyPalet, QtyUnidad, QtyTotal, idOperario, idUnidadTipo,
                              idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado, motivo, rCheckBoxConReserva.Checked, rCheckBoxEtiqueta.Checked, radTextBoxSSCC.Text, radTextBoxNSerie.Text, entradaPadre, radTextBoxComentario.Text, radTextBoxEAN.Text, atributos,peso);*/
                        }
                        else
                        {
                            MessageBox.Show(Lenguaje.traduce("Hay que introducir valores numericos en entrada contenedor "), Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return -1;
                        }
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
            rumLabelEN.Enabled = true;
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

            rumLabelUbicacion.Enabled = true;
            radComboBoxUbicacion.Enabled = true;
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

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;

                    break;

                case AltaPaletPicos://Caso Entrada Picos
                    rumLabelNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Text = "1";

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    break;

                case AltaPaletMultireferencia://Caso Entrada Multiproducto

                    radTextBoxControlEntradaPaletPadre.Enabled = true;
                    rumLabelEntradaPaletPadre.Enabled = true;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;

                    rumButtonCrearEntradaPadre.Enabled = true;
                    rumButtonCrearEntradaPadre.Visible = true;
                    break;

                case AltaPaletCarroMovil://Caso Entrada Carro Movil
                    rumLabelNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Text = "1";

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;
                    break;

                case AltaPaletEntradasTotales://Caso Entradas Totales
                    radTextBoxControlNumPalets.Enabled = false;
                    radTextBoxControlNumPalets.Text = "1";

                    radTextBoxControlEntradaPaletPadre.Enabled = false;
                    rumLabelEntradaPaletPadre.Enabled = false;

                    rumLabelTotalUdsPicos.Enabled = false;
                    radTextBoxControlTotalUdsPicos.Enabled = false;

                    rumLabelTipoPalet.Enabled = false;
                    radComboBoxTipoPalet.Enabled = false;

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
                if (String.IsNullOrEmpty(radTextBoxControlLote.Text))
                {
                    MessageBox.Show(Lenguaje.traduce("El lote no puede estar vacio"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    existeLote = false;
                    radTextBoxControlLote.Focus();
                    return -1;
                }
                WSLotesClient wsLotes = new WSLotesClient();
                Object[] loteArticulo = wsLotes.getLoteArticulo(idArticuloSeleccionado, radTextBoxControlLote.Text);
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
                ExceptionManager.GestionarError(ex, ex.Message);
                existeLote = false;
                return -1;
            }
        }

        private void radComboBoxArticulo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radComboBoxArticulo.SelectedIndex >= 0)
                {
                    idArticuloSeleccionado = Convert.ToInt32(radComboBoxArticulo.SelectedValue);
                    cargaDatosArticulo();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }

        private void rCheckBoxConReserva_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (opcion == AltaPaletCarroMovil)
            {
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionCarrosMovil(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                rumLabelUbicacion.Text = "Carro";
            }
            if (opcion == AltaPalet || opcion == AltaPaletPicos)
            {
                if (rCheckBoxConReserva.Checked)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionHuecoMuelle(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                    rumLabelUbicacion.Text = "Origen";
                }
                else
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                    rumLabelUbicacion.Text = "Ubicacion";
                }
            }
            if (opcion == AltaPaletEntradasTotales)
            {
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUbicacion, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                rumLabelUbicacion.Text = "Ubicacion";
            }
        }

        #endregion Metodos

        #region MetodosPredefinidosYBotones

        private void FormularioAltaProductoRegularizacion_Load(object sender, EventArgs e)
        {
            /*try
            {
                CargarDatosCombos();
                //CargarDatosDefectoAtributos();
                verCampos(false);
            }catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }*/
        }

        private void verCampos(bool visible)
        {
            rumLabelNumPalets.Visible = visible;
            radTextBoxControlNumPalets.Visible = visible;
            rumLabelEmbalaje.Visible = visible;
            radComboBoxUnidadTipo.Visible = visible;
            rumLabelDE.Visible = visible;
            rumLabelEN.Visible = visible;
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
            lblPeso.Visible = visible;
            radTextBoxPeso.Visible = visible;

            rumLabelSSCC.Visible = visible;
            radTextBoxSSCC.Visible = visible;
            rumLabelTipoPalet.Visible = visible;
            radComboBoxTipoPalet.Visible = visible;
            rumLabelTotalUdsPicos.Visible = visible;
            radTextBoxControlTotalUdsPicos.Visible = visible;
            rumLabelUbicacion.Visible = visible;
            radComboBoxUbicacion.Visible = visible;
            rCheckBoxConReserva.Visible = visible;
            rCheckBoxEtiqueta.Visible = visible;
            rumButtonAlta.Enabled = visible;
            rumButtonCrearEntradaPadre.Visible = visible;
            tableLayoutPanelAtributos.Visible = visible;
            /*if (listaAtributos != null && listaAtributos.Count > 0)
            {
                 for (int i = 0; i < tableLayoutPanelAtributos.RowCount; i++)
                {
                    RumLabel label= (tableLayoutPanelAtributos.GetControlFromPosition(0, i) as RumLabel);
                    RadTextBox txt = (tableLayoutPanelAtributos.GetControlFromPosition(1, i) as RadTextBox);
                    label.Visible = visible;
                    txt.Visible = visible;
                }
            }*/
        }

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Procediendo a realizar Alta Producto");
                String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
                bool errorBool = false;
                double peso = 0;

                if (!int.TryParse(radTextBoxControlNumPalets.Text, out int numPalets))
                    errorBool = true;
                if (!double.TryParse(radTextBoxEmbalaje.Text, out double totalEmbalaje))
                    errorBool = true;
                if (!double.TryParse(radTextBoxControlUdsCaja.Text, out double udsCaja))
                    errorBool = true;
                if (!double.TryParse(radTextBoxControlTotalUdsPicos.Text, out double totalUdsPicos))
                    /*   if (!radTextBoxControlTotalUdsPicos.Text.Equals(""))
                   {
                      if (!double.TryParse(radTextBoxControlTotalUdsPicos.Text, out double totalUdsPicos))
                           errorBool = true;
                   }*/

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
                if (!int.TryParse(radComboBoxUbicacion.SelectedValue.ToString(), out int idHueco))
                    errorBool = true;

                if (errorBool == true)
                {
                    MessageBox.Show(errorMensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Info("Comprobación de campos erronea.");
                    return;
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
                    radTextBoxControlLote.Text, User.NombreImpresora, idHueco, idArticuloSeleccionado, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), atributos, peso);
                    }
                    else
                    {
                        Object[] resultadoLote = null;
                        FormularioCrearLote formLote;
                        DateTime fechaCaducidad = new DateTime();
                        if (loteEstricto.Equals(ConstArticulosConfLote.SI))
                        {
                            string fecha = null;
                            if (fechaCaducidadCalculada != null)
                            {
                                fecha = fechaCaducidadCalculada.ToString();
                            }
                            if (fecha == null)
                            {
                                formLote = new FormularioCrearLote(idArticuloSeleccionado, radTextBoxControlLote.Text, 0);
                            }
                            else
                            {
                                fechaCaducidad = DateTime.Parse(fecha);
                                formLote = new FormularioCrearLote(idArticuloSeleccionado, radTextBoxControlLote.Text, 0, fechaCaducidad);
                            }

                            formLote.ShowDialog();
                            resultadoLote = formLote.Resultado;
                        }
                        else
                        {
                            if (loteEstricto.Equals(ConstArticulosConfLote.NO_CONFIRMAR_FECHA))
                            {
                                WSLotesClient LotesMotor = new WSLotesClient();
                                resultadoLote = LotesMotor.insertarLoteConArticulo(idArticuloSeleccionado, DateTime.Now, radTextBoxControlLote.Text, 0);
                            }
                        }

                        if (resultadoLote != null)
                        {
                            String loteCreado = (resultadoLote[2] as object[])[1].ToString();
                            resultado = LlamarWebService(totalUdsPicos, totalEmbalaje, udsCaja, numPalets, User.IdOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                            loteCreado, User.NombreImpresora, idHueco, idArticuloSeleccionado, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), atributos, peso);
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
                    radTextBoxControlLote.Text, "", idHueco, idArticuloSeleccionado, radComboBoxEstado.SelectedValue.ToString(), radComboBoxMotivo.SelectedValue.ToString(), atributos, peso);
                }

                this.resultado = resultado;
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

        private void RadTextBoxControlLote_Leave(object sender, EventArgs e)
        {
            ComprobarExistenciaLotes();
        }

        private void radTextBoxEmbalaje_Leave(object sender, EventArgs e)
        {
            radTextBoxEmbalaje.Text = radTextBoxEmbalaje.Text.Replace('.', ',');
        }

        private void radTextBoxControlUdsCaja_Leave(object sender, EventArgs e)
        {
            radTextBoxControlUdsCaja.Text = radTextBoxControlUdsCaja.Text.Replace('.', ',');
        }

        private void radTextBoxControlTotalUdsPicos_Leave(object sender, EventArgs e)
        {
            radTextBoxControlTotalUdsPicos.Text = radTextBoxControlTotalUdsPicos.Text.Replace('.', ',');
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

        #endregion MetodosPredefinidosYBotones

        /*private void radComboBoxArticulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idArticuloSeleccionado = Convert.ToInt32(radComboBoxArticulo.SelectedValue);
                cargaDatosArticulo();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
            }
        }*/
    }
}
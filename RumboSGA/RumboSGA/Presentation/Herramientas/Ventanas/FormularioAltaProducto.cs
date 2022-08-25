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
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FormularioAltaProducto : Telerik.WinControls.UI.RadForm
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables
        private int opcion, idArticuloFinal, idPedidoFab;
        private String idOrden;      
        private List<OAtributo> listaAtributos;
        private bool existeLote;

        public const int AltaPalet = 1;
        public const int AltaPaletPicos = 2;
        public const int AltaPaletMultireferencia = 3;
        public const int AltaPaletCarroMovil = 4;
        public const int AltaPaletEntradasTotales = 5;
        private WSVariablesClient wsv = new WSVariablesClient();
        WSProduccionMotorClient wsp = new WSProduccionMotorClient();
        WSOrdenProduccionMotorClient wsop = new WSOrdenProduccionMotorClient();
        private int tipoPresentacion=0;        
        private int idPresentacion = 0;
        private DateTime? fechaCaducidadCalculada = null;
        private string loteEstricto;
        #endregion

        public FormularioAltaProducto(int opcion, string idOrden,string nombreOpcion, Image imagenIcono)
        {
            listaAtributos = null;
            existeLote = false;
            InitializeComponent();
            this.opcion = opcion;
            this.idOrden = idOrden;
            radTextBoxControlNumPalets.Text = "" + 1;
            int tipoPres = Persistencia.getParametroInt("TIPOPRESENTACION");            
            if (tipoPres<0)
            {
                tipoPresentacion = 0;
            }
            else
            {
                tipoPresentacion = Convert.ToInt32(tipoPres);
            }

            CargarConfiguracionVisual(opcion);
            CargarDatosDefectoAtributos();
            CargarDatosDefecto();
            this.Text = Lenguaje.traduce("Alta Producto");
            this.rumLabelImagen.Image = imagenIcono;
            this.rumLabelTipoAlta.Text = nombreOpcion;
            this.Icon = Resources.bruju;
        }

        #region Metodos     

        private int CargarDatosDefecto()
        {
            try
            {
                
                CompositeFilterDescriptor filtro = null;
                List<TableScheme> listaResultado = new List<TableScheme>();
                DataRow valoresOrdenFila = DataAccess.GetAltaProductoDefectoOrden(idOrden+"").Rows[0];
                if (valoresOrdenFila == null)
                {
                    MessageBox.Show(Lenguaje.traduce("No ha devuelto nada"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                idPresentacion = Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]);
                int tipoPresArticulo = Convert.ToInt32(valoresOrdenFila["TIPOPRESENTACION"]);
                if (tipoPresArticulo >= 0)
                {
                    tipoPresentacion = tipoPresArticulo;
                }

                this.idPedidoFab = int.Parse(valoresOrdenFila["IDPEDIDOFAB"].ToString());
                this.idArticuloFinal = int.Parse(valoresOrdenFila["IDARTICULO"].ToString());
                if (tipoPresentacion == 0 || idPresentacion==0)
                {
                    
                    radTextBoxControlUdsCaja.Text = valoresOrdenFila["CANTIDADUNIDAD"].ToString();
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, DataAccess.GetIdDescripcionUnidadesTipo(), "IDUNIDADTIPO", "DESCRIPCION", valoresOrdenFila["IDUNIDADTIPO"].ToString(), null, true);
                    if (opcion == AltaPalet)
                    {
                        radTextBoxEmbalaje.Text = valoresOrdenFila["CANTIDAD"].ToString();
                        radTextBoxEmbalajesFabricados.Text = valoresOrdenFila["CANTIDAD"].ToString();
                    }
                }
                else
                {
                    object[] pres=Presentaciones.getTipoUnidadPresentacionVisualizacion(idPresentacion);
                    string idUnidadTipoPres = pres[1].ToString();
                    radTextBoxControlUdsCaja.Text = pres[0].ToString();
                    Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, valoresOrdenFila["IDUNIDADTIPO"].ToString());Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo, idUnidadTipoPres);
                    if (opcion == AltaPalet)
                    {
                        object[] presC = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, Convert.ToInt32(valoresOrdenFila["CANTIDAD"]));
                        radTextBoxEmbalaje.Text = presC[0].ToString();
                        radTextBoxEmbalajesFabricados.Text = presC[0].ToString();
                    }
                }
                
                //Unidades Tipo Embalaje
                //Utilidades.RellenarMultiColumnComboBox(ref radComboBoxUnidadTipo, DataAccess.GetIdDescripcionUnidadesTipo(),"IDUNIDADTIPO","DESCRIPCION", valoresOrdenFila["IDUNIDADTIPO"].ToString(),new String[]{"TODOS"});
                //Presentaciones.RellenarUnidadesTipoPresentaciones(Convert.ToInt32(valoresOrdenFila["IDPRESENTACION"]), tipoPresentacion, ref radComboBoxUnidadTipo,  valoresOrdenFila["IDUNIDADTIPO"].ToString());
                //TipoPalet
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxTipoPalet, DataAccess.GetIdDescripcionTipoPalets(), "IDPALETTIPO", "DESCRIPCION", valoresOrdenFila["IDPALETTIPO"].ToString(), new String[] { "TODOS" }, true);

                //Lote
                loteEstricto = valoresOrdenFila["LOTEESTRICTO"].ToString();
                if (loteEstricto.Equals(ConstArticulosConfLote.NO_CONFIRMAR_FECHA) || loteEstricto.Equals(ConstArticulosConfLote.SI))
                {
                    RadTextBoxControlLote.Enabled = true;
                    CalcularFechaCaducidad(Convert.ToInt32(valoresOrdenFila["DIASCADUCIDAD"]));
                }

                //Maquina
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxMaquina, DataAccess.GetIdDescripcionMaquinas(), "IDMAQUINA", "DESCRIPCION", valoresOrdenFila["IDMAQUINA"].ToString(), new String[] { "TODOS" }, true);

                //Estados Existencia
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxEstado, DataAccess.GetIdDescripcionExistenciaEstados(), "IDEXISTENCIAESTADO", "DESCRIPCION", valoresOrdenFila["ESTADOENTRADA"].ToString(), new String[] { "TODOS" }, true);

                //Carro
                if (opcion == AltaPaletCarroMovil)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxCarro, DataAccess.GetIdDescripcionCarrosMovil(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                    
                }
                if (opcion == AltaPaletEntradasTotales)
                {
                    Utilidades.RellenarMultiColumnComboBox(ref radComboBoxCarro, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION", "", new String[] { "TODOS" }, true);
                }

                //Operarios
                Utilidades.RellenarMultiColumnComboBox(ref radComboBoxOperario, DataAccess.GetIdDescripcionOperarios(), "IDOPERARIO", "NOMBRE", User.IdOperario.ToString(), new String[] { "TODOS" }, true);
                RadTextBoxControlLote.Text = valoresOrdenFila["LOTE"].ToString();


                CargarDatosDefectoAtributosClave(valoresOrdenFila);
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
            if(listaAtributos==null || listaAtributos.Count < 1)
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
            FormularioCrearPaletMultireferencia fcp = new FormularioCrearPaletMultireferencia(radComboBoxTipoPalet.SelectedValue.ToString());

            fcp.ShowDialog();
            if(fcp.resultado!=null) radTextBoxControlEntradaPaletPadre.Text = fcp.resultado;

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

                tableLayoutPanelAtributos.RowCount = listaAtributos.Count;

                for (int i = 0; i < listaAtributos.Count; i++)
                {
                    OAtributo atributo = listaAtributos[i];
                    RumLabel rumLabelAtributo = new RumLabel();
                    rumLabelAtributo.Name = "RadLabel" + atributo.Nombre;
                    rumLabelAtributo.Text = atributo.Nombre + ":";

                    RadTextBox radTextBoxAtributo = new RadTextBox();
                    radTextBoxAtributo.Name = "RadTextBox" + atributo.Nombre;
                    radTextBoxAtributo.Tag =  atributo.Atributo;
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

        private void radTextBoxControlTotalUdsPicos_Leave(object sender, EventArgs e)
        {
            double cantidadEmbalaje = 0;
            int cantidadPicosText = 0;
            double cantidadPicos = 0;
            double cantidadTotal = 0;
            double cantidadUnidad=1;
            if (!String.IsNullOrEmpty(radTextBoxControlTotalUdsPicos.Text))
            {
                cantidadPicosText = int.Parse(radTextBoxControlTotalUdsPicos.Text);
            }
            if (!String.IsNullOrEmpty(radTextBoxEmbalaje.Text))
            {
                cantidadEmbalaje = double.Parse(radTextBoxEmbalaje.Text);
            }
            if (!String.IsNullOrEmpty(radTextBoxControlUdsCaja.Text))
            {
                cantidadUnidad = double.Parse(radTextBoxControlUdsCaja.Text);
            }
            switch (tipoPresentacion)
            {
                case 0:
                    cantidadTotal = cantidadPicos + cantidadEmbalaje;
                    break;
                case 1:
                    cantidadTotal = cantidadPicos + (cantidadEmbalaje * cantidadUnidad);
                    break;
                case 2:
                    int cantidadEmbalajeF = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadEmbalaje);
                    int cantidadTotalF = cantidadPicosText + cantidadEmbalajeF;
                    object[] pres = Presentaciones.getCantidadPresentacionVisualizacion(idPresentacion, cantidadTotalF);
                    cantidadTotal = Convert.ToDouble(pres[0]);
                    break;
                

                
            }
            
            radTextBoxEmbalajesFabricados.Text = cantidadTotal.ToString();


        }

        private int LlamarWebService(double totalUnidadPicos,double cantidadEmbalajeFabricado,double cantidadEmbalaje, double cantidadUnidad, int numPalets,int idOperario, string idUnidadTipo, string idPaletTipo,string lote,
            string impresora, int muelle,int idArticuloFinal, string estado,object[] atributos)
        {
            try
            {
               
                int respuesta = -1;
                double cantidadPalet = 0;
                double cantidadPaletFabricado = 0;
                double cantidadTotal = 0;
                double cantidadTotalFabricada = 0;

                switch (tipoPresentacion)
                {

                    case 0:
                        switch (opcion)
                        {
                            case AltaPalet:
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = numPalets * cantidadPalet;
                                cantidadPaletFabricado = cantidadEmbalajeFabricado;                               
                                cantidadTotalFabricada = numPalets * cantidadPaletFabricado;
                                break;                           
                            case AltaPaletMultireferencia:
                            case AltaPaletEntradasTotales:
                            case AltaPaletCarroMovil:
                            
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = cantidadPalet;
                                cantidadPaletFabricado = cantidadEmbalajeFabricado;                               
                                cantidadTotalFabricada = cantidadPaletFabricado;
                                break;
                            case AltaPaletPicos:
                                cantidadTotal = totalUnidadPicos + cantidadEmbalaje;
                                cantidadPalet = totalUnidadPicos + cantidadEmbalaje;
                                cantidadPaletFabricado = cantidadEmbalajeFabricado;
                                cantidadTotalFabricada = cantidadPaletFabricado;

                                break;

                        }
                        break;
                    case 1:
                        switch (opcion)
                        {
                            case AltaPalet:
                                cantidadPalet = cantidadEmbalaje * cantidadUnidad;
                                cantidadTotal = numPalets * cantidadPalet;
                                cantidadPaletFabricado = cantidadEmbalajeFabricado * cantidadUnidad;                               
                                cantidadTotalFabricada = numPalets * cantidadPaletFabricado;
                                break;
                            case AltaPaletPicos:
                                cantidadTotal = totalUnidadPicos + (cantidadEmbalaje * cantidadUnidad);
                                cantidadPalet = totalUnidadPicos + (cantidadEmbalaje * cantidadUnidad);
                                cantidadPaletFabricado = cantidadEmbalajeFabricado * cantidadUnidad;
                                cantidadTotalFabricada = cantidadPaletFabricado;
                                break;
                            case AltaPaletMultireferencia:
                            case AltaPaletCarroMovil:
                            case AltaPaletEntradasTotales:
                                cantidadPalet = cantidadEmbalaje * cantidadUnidad;
                                cantidadTotal = cantidadPalet;
                                cantidadPaletFabricado = cantidadEmbalajeFabricado * cantidadUnidad;                                
                                cantidadTotalFabricada = cantidadPaletFabricado;
                                break;
                        }
                        break;
                    case 2:
                        switch (opcion)
                        {
                            case AltaPalet:
                                cantidadPalet = cantidadEmbalaje;
                                cantidadTotal = numPalets * cantidadEmbalaje;
                                cantidadPaletFabricado = cantidadEmbalajeFabricado;
                                cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadPalet);
                                cantidadTotal = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadTotal);
                                cantidadPaletFabricado = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadPaletFabricado);
                                cantidadTotalFabricada =  numPalets * cantidadPaletFabricado ;
                                /* object[] pres = Presentaciones.getTipoUnidadPresentacionAlmacenamiento(idPresentacion);
                                 idUnidadTipo = pres[1].ToString();*/
                                object[] pres = Presentaciones.getTipoUnidadPresentacion(idPresentacion, idUnidadTipo);
                                idUnidadTipo = pres[1].ToString();
                                cantidadUnidad = double.Parse(pres[0].ToString());
                                break;
                            case AltaPaletPicos:
                                cantidadEmbalaje = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadEmbalaje);
                                cantidadPalet = (cantidadEmbalaje + totalUnidadPicos) / cantidadUnidad;
                                cantidadTotal = (cantidadPalet * cantidadUnidad);
                                cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, idUnidadTipo, cantidadPalet);
                                object[] pres1 = Presentaciones.getTipoUnidadPresentacion(idPresentacion, idUnidadTipo);
                                idUnidadTipo = pres1[1].ToString();
                                cantidadUnidad = double.Parse(pres1[0].ToString());
                                cantidadPaletFabricado = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadEmbalajeFabricado);
                                cantidadTotalFabricada = cantidadPaletFabricado;
                                break;
                            case AltaPaletMultireferencia:
                            case AltaPaletCarroMovil:
                            case AltaPaletEntradasTotales:
                                cantidadPaletFabricado = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadEmbalajeFabricado); ;
                                cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadEmbalaje);                               
                                cantidadTotal = cantidadPalet;
                                cantidadTotalFabricada = cantidadPaletFabricado;
                                object[] pres2 = Presentaciones.getTipoUnidadPresentacionAlmacenamiento(idPresentacion);
                                idUnidadTipo = pres2[1].ToString();
                                cantidadUnidad = double.Parse(pres2[0].ToString());
                                break;
                        }
                        break;

                }

               /* if (opcion == AltaPaletCarroMovil)
                {
                    cantidadPalet = cantidadEmbalaje;
                    cantidadPaletFabricado = cantidadEmbalajeFabricado;


                }
                else
                {
                    if (tipoPresentacion == 2)
                    {
                        cantidadPalet =  cantidadEmbalaje;
                        cantidadPaletFabricado = cantidadEmbalajeFabricado;
                    }
                    else
                    {
                        cantidadPalet = cantidadUnidad * cantidadEmbalaje;
                        cantidadPaletFabricado = cantidadUnidad * cantidadEmbalajeFabricado;
                    }
                }

                double cantidadTotal = numPalets * cantidadPalet;
                double cantidadTotalFabricada = numPalets * cantidadPaletFabricado;
                if (tipoPresentacion > 0)
                {
                    if (idPresentacion > 0)
                    {
                        cantidadPalet = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadPalet);
                        cantidadTotal = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadTotal);
                        cantidadTotalFabricada = Presentaciones.getCantidadPresentacionAlmacenamiento(idPresentacion, cantidadTotalFabricada);
                        object[] pres = Presentaciones.getTipoUnidadPresentacionAlmacenamiento(idPresentacion);
                        idUnidadTipo = pres[1].ToString();
                    }
                }*/
                int QtyPalet = Convert.ToInt32(cantidadPalet);
                int QtyUnidad = Convert.ToInt32(cantidadUnidad);
                int QtyTotal = Convert.ToInt32(cantidadTotal);
                int QtyTotalFabricada = Convert.ToInt32(cantidadTotalFabricada);
                switch (opcion)
                {
                    case AltaPalet:
                        respuesta = wsp.generarEntradaPaletsProduccion(idPedidoFab, QtyPalet, QtyUnidad, numPalets, idOperario, idUnidadTipo, idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado, atributos,QtyTotalFabricada, DatosThread.getInstancia().getArrayDatos());
                        break;
                    case AltaPaletPicos:
                        respuesta = wsp.generarEntradaPaletsPicoProduccion(idPedidoFab, QtyPalet, QtyUnidad, QtyTotal, idOperario, idUnidadTipo, idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado, atributos,QtyTotalFabricada, DatosThread.getInstancia().getArrayDatos());
                        break;
                    case AltaPaletMultireferencia:
                        
                        if (/*int.TryParse(radTextBoxControlEtiquetas.Text, out int nEtiquetas) &&*/ int.TryParse(radTextBoxControlEntradaPaletPadre.Text, out int entradaPadre) && entradaPadre>0)
                        {
                            respuesta = wsp.generarEntradaMultiproductoProduccion(idPedidoFab, QtyPalet, QtyUnidad, QtyTotal, idOperario, idUnidadTipo, idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado,entradaPadre,0, atributos, DatosThread.getInstancia().getArrayDatos());
                            MessageBox.Show(Lenguaje.traduce("Creado! Matricula: ") + respuesta, Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(Lenguaje.traduce("Hay que introducir valores numericos en numero de etiquetas y entrada contenedor "), Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return -1;
                        }
                        break;
                    case AltaPaletCarroMovil:
                            respuesta = wsp.generarEntradaCarroProduccion(idPedidoFab, QtyPalet, QtyUnidad, QtyTotal, idOperario, idUnidadTipo, idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado, atributos);
                            MessageBox.Show(Lenguaje.traduce("Creado! Matricula: ") + respuesta, Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case AltaPaletEntradasTotales:
                        respuesta = wsp.generarEntradaTotalesProduccion(idPedidoFab, QtyPalet, QtyUnidad, numPalets, idOperario, idUnidadTipo, idPaletTipo, lote, impresora, muelle, idArticuloFinal, estado, atributos,QtyTotalFabricada, DatosThread.getInstancia().getArrayDatos());
                        break;

                }
                MessageBox.Show(Lenguaje.traduce("Creado! Matricula: ") + respuesta, Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return respuesta;
            }catch(Exception e)
            {

                ExceptionManager.GestionarError(e, "Se ha generado un error en el alta de palet");
                
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

            rumLabelOperario.Enabled = true;
            radComboBoxOperario.Enabled = true;

            rumLabelLote.Enabled = true;
            RadTextBoxControlLote.Enabled = false;

            rumLabelTipoPalet.Enabled = true;
            radComboBoxTipoPalet.Enabled = true;

            radComboBoxMaquina.Enabled = true;
            rumLabelMaquina.Enabled = true;

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

                    rumLabelTotalUdsFabricadas.Text = "Total Uds Fabricadas";

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

                    rumLabelTotalUdsFabricadas.Visible = false;
                    radTextBoxEmbalajesFabricados.Visible = false;
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

                    rumLabelTotalUdsFabricadas.Visible = false;
                    radTextBoxEmbalajesFabricados.Visible = false;
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
            if (Persistencia.getParametroInt("CONSUMIRPORVISIBLEALTAPALET") == 0)
            {
                radTextBoxEmbalajesFabricados.Visible = false;
                rumLabelTotalUdsFabricadas.Visible = false;
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
                Object[] loteArticulo = wsLotes.getLoteArticulo(idArticuloFinal, RadTextBoxControlLote.Text);
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

        #endregion

        #region MetodosPredefinidosYBotones
        private void FormularioAltaProducto_Load(object sender, EventArgs e)
        {
            //CargarDatosDefectoAtributos();
        }

        private void RumButtonAlta_Click(object sender, EventArgs e)
        {
           
            log.Info("Procediendo a realizar Alta Producto");
            String errorMensaje = Lenguaje.traduce("Error en la comprobación de campos");
            bool errorBool = false;
            double totalEmbalajeFabricado;
           
            
           
            if (!int.TryParse(radTextBoxControlNumPalets.Text, out int numPalets))
                errorBool = true;
            if (!double.TryParse(radTextBoxEmbalaje.Text, out double totalEmbalaje))
                errorBool = true;
            if (!double.TryParse(radTextBoxControlUdsCaja.Text, out double udsCaja))
                errorBool = true;
            if (RadTextBoxControlLote.Enabled && RadTextBoxControlLote.Text.Equals(""))
            {
                MessageBox.Show("Debe introducir el lote", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            if (opcion == AltaPaletPicos ||opcion == AltaPalet || opcion == AltaPaletEntradasTotales)
            {
                if (!double.TryParse(radTextBoxEmbalajesFabricados.Text, out totalEmbalajeFabricado))

                    errorBool = true;
            }
            else
            {
                totalEmbalajeFabricado = totalEmbalaje;
            }
            
            
            if (!int.TryParse(radComboBoxOperario.SelectedValue.ToString(), out int idOperario))
                errorBool = true;

            if (errorBool == true)
            {
                MessageBox.Show(errorMensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Info("Comprobación de campos erronea.");
                return;
            }

            int muelle = DataAccess.GetMuelleMaquina(Convert.ToInt32(radComboBoxMaquina.SelectedValue));//int.Parse(ConexionSQL.getDataTable("SELECT TOP 1 MUELLE FROM TBLMAQUINAS WHERE IDMAQUINA = " + radComboBoxMaquina.SelectedValue.ToString()).Rows[0][0].ToString());
            if (opcion == AltaPaletCarroMovil || opcion == AltaPaletEntradasTotales)
            {
                muelle = Convert.ToInt32(radComboBoxCarro.SelectedValue);
            }
            Object[][] atributos = ComprobracionAtributos();
            if(atributos!=null && (atributos[0][0] as String).Equals("Error"))
            {
                MessageBox.Show(Lenguaje.traduce("Falta un atributo obligatorio"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int resultado = 0;
            if (RadTextBoxControlLote.Enabled)
            {
                if (existeLote == true)
                {
                    resultado = LlamarWebService(totalUdsPicos, totalEmbalajeFabricado, totalEmbalaje, udsCaja, numPalets, idOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                    RadTextBoxControlLote.Text,User.NombreImpresora, muelle, idArticuloFinal, radComboBoxEstado.SelectedValue.ToString(), atributos);
                }
                else
                {
                    Object[] resultadoLote = null;
                    FormularioCrearLote formLote;
                    //DateTime fechaCaducidad= new DateTime();
                    if (loteEstricto.Equals(ConstArticulosConfLote.SI))
                    {
                        DateTime fecha = wsop.getFechaCaducidadHeredada(idPedidoFab);
                        if (fecha == DateTime.MinValue)
                        {
                            if (fechaCaducidadCalculada == null)
                            {
                                formLote = new FormularioCrearLote(idArticuloFinal, RadTextBoxControlLote.Text, 0);
                            }
                            else
                            {

                                formLote = new FormularioCrearLote(idArticuloFinal, RadTextBoxControlLote.Text, 0, fechaCaducidadCalculada.Value);
                            }
                        }
                        else
                        {
                            //fechaCaducidad = DateTime.Parse(fecha);
                            formLote = new FormularioCrearLote(idArticuloFinal, RadTextBoxControlLote.Text, 0, fecha);
                        }

                        formLote.ShowDialog();
                        resultadoLote = formLote.Resultado;
                    }
                    else
                    {
                        if (loteEstricto.Equals(ConstArticulosConfLote.NO_CONFIRMAR_FECHA))
                        {
                            WSLotesClient LotesMotor = new WSLotesClient();
                            resultadoLote = LotesMotor.insertarLoteConArticulo(idArticuloFinal, DateTime.Now, RadTextBoxControlLote.Text, 0);
                        }
                    }
                   
                    if (resultadoLote != null)
                    {
                        String loteCreado = (resultadoLote[2] as object[])[1].ToString();
                        resultado = LlamarWebService(totalUdsPicos, totalEmbalajeFabricado, totalEmbalaje, udsCaja, numPalets, idOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                        loteCreado, User.NombreImpresora, muelle, idArticuloFinal, radComboBoxEstado.SelectedValue.ToString(), atributos);
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje.traduce("Error creando el lote"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                resultado = LlamarWebService(totalUdsPicos, totalEmbalajeFabricado, totalEmbalaje, udsCaja, numPalets, idOperario, radComboBoxUnidadTipo.SelectedValue.ToString(), radComboBoxTipoPalet.SelectedValue.ToString(),
                RadTextBoxControlLote.Text, User.NombreImpresora, muelle, idArticuloFinal, radComboBoxEstado.SelectedValue.ToString(), atributos);
            }
            


            this.Close();
          return;
        }

        private void RadTextBoxEmbalaje_TextChanged(object sender, EventArgs e)
        {
            if(this.opcion!= AltaPaletPicos)
            {
                radTextBoxEmbalajesFabricados.Text = radTextBoxEmbalaje.Text;
            }
        }

        private void RadTextBoxControlTotalUdsPicos_TextChanged(object sender, EventArgs e)
        {
            if (this.opcion == AltaPaletPicos)
            {
                radTextBoxEmbalajesFabricados.Text = radTextBoxControlTotalUdsPicos.Text;
            }
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

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadTextBoxControlLote_Leave(object sender, EventArgs e)
        {
            ComprobarExistenciaLotes();
        }
        #endregion
    }
}

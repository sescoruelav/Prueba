using Rumbo.Core.Herramientas;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
namespace RumboSGAManager.Model.Entities
{

    public static class XmlReaderPropio
    {
        private const int GLOBAL = 0;
        private const int LOCAL = 1;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum Valores { AcercaDe, Paginacion, LenguajePath, FiltroInicial, LayoutPath, ArbolLin, 
            LineasReservadas, LineasPreparadas, PctOFProduccion,pctArticulosPteAcopio, pctLineasPedProCab,
            pctLineasDevolCliCab, pctEntradas, pctEntradasDevolCli, pctEntradasPendientes, 
            pctEntradasPendientesDevolCli, pctCantidadReservada , pctCantidadServida,
            CantidadServida, CantidadReservada   }
        

        public static List<ColumnaModificable> getColumnasModStock()
        {
            List<ColumnaModificable> columnas = new List<ColumnaModificable>();
            string path = Persistencia.DirectorioBase + @"\Configs\stockGrid.xml";
            //infoDirectorio();
            try
            {
                XDocument X = XDocument.Load(path);
                foreach (var item in X.Element("SQL").Element("COLUMNASMOD").Nodes())
                {

                    XElement columna = (XElement)item;
                    ColumnaModificable col = new ColumnaModificable();
                    foreach (var columnaNode in columna.Nodes())
                    {
                        XElement elemento = (XElement)columnaNode;

                        if (elemento.Name.ToString().Equals("IDENTIFICADOR"))
                        {
                            col.NombreColumna = elemento.Value;
                        }
                        if (elemento.Name.ToString().Equals("NOMBREETIQUETA"))
                        {
                            col.NombreColumnaEtiqueta = Lenguaje.traduce(elemento.Value);
                        }
                        if (elemento.Name.ToString().Equals("TABLA"))
                        {
                            col.Tabla = elemento.Value;
                        }
                        //Si existe el tag lo sobreescribo
                        if (elemento.Name.ToString().Equals("NOMBREETIQUETA"))
                        {
                            col.NombreColumnaEtiqueta = Lenguaje.traduce(elemento.Value);
                        }
                        columnas.Add(col);
                    }
                }
                //return selectSQL;
            }
            catch (Exception e)
            {
                Debug.WriteLine(path);
                Debug.WriteLine(e.Message);
            }
            return columnas;
        }
        //Sobrecargamos la funcion con diferentes parametros
        public static string getTexto(Valores parametro)
        {
            return getTexto(parametro, String.Empty, 0);
        }
        public static string getTexto(Valores parametro, string sParametro)
        {
            return getTexto(parametro, sParametro, 0);
        }
        public static string getTexto(Valores parametro, int iParametro)
        {
            return getTexto(parametro, String.Empty, iParametro);
        }
        public static string getTexto(Valores parametro, string sParametro, int iParametro)
        {
            string path = string.Empty;
            string texto = string.Empty;
            XDocument xDoc = null;
            try
            {
                switch (parametro)
                {
                    case Valores.AcercaDe:
                        path = @"XML\acercaDe.xml";
                        xDoc = XDocument.Load(path);
                        texto = xDoc.Element("texto").Value;
                        break;
                    case Valores.LayoutPath:
                        if (iParametro == GLOBAL)
                            texto = Persistencia.DirectorioGlobal;
                        else
                            texto = Persistencia.DirectorioLocal;
                        break;
                    case Valores.ArbolLin:
                        path = @"XML\parametros.xml";
                        xDoc = XDocument.Load(path);
                        foreach (XElement item in xDoc.Descendants("root"))
                        {
                            foreach (XElement item2 in item.Nodes())
                            {
                                string text = item2.Value.ToString();
                                if (item2.Name == "arbolLin")
                                {
                                    texto = item2.Value;
                                }
                            }
                        }
                        break;
                    case Valores.FiltroInicial:
                        path = Persistencia.ConfigPath + @"\FiltroInicial.xml";
                        xDoc = XDocument.Load(path);
                        foreach (var item in xDoc.Descendants("filtroRecepcionesVistaPedido"))
                        {
                            foreach (var item2 in item.Descendants("filtro"))
                            {
                                string campo = item2.Element("campo").Value;
                                string operador = item2.Element("operadorComparacion").Value;
                                if (operador == "&lt;&gt;")
                                    operador = "<>";
                                string valor = item2.Element("valor").Value;
                                if (item2.HasAttributes)
                                {
                                    string operadorSQL = item2.Attribute("operadorSQL").Value;
                                    texto += operadorSQL + " " + campo + " " + operador + " " + valor + " ";
                                }
                                else
                                    texto += campo + " " + operador + " " + valor + " ";
                            }
                        }
                        break;
                    case Valores.LenguajePath:
                        path = @"XML\PathLayouts.xml";
                        xDoc = XDocument.Load(path);
                        texto = xDoc.Element("paths").Element("lenguaje").Value + System.IO.Path.DirectorySeparatorChar + sParametro + ".properties";
                        break;
                    default:
                        throw new Exception(sParametro.ToString() + " No implementado en retorno de texto");
                }
                return texto;//esto lo he puesto yo (Tomás) para probar que compile
                             
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("no se ha encontrado el fichero" + "\n" + path);
                return "";
            }
            catch (Exception e)
            {
                MessageBox.Show("se ha producido un error cargando el parametro XML" + "\n" + path);
                return "";
            }
        }

        [Obsolete("Mejor usa:getTexto(Valores.ArbolLin) ")]
        public static string GetArbolLin()
        {
            return getTexto(Valores.ArbolLin);
        }
        [Obsolete("Mejor usa:getTexto(Valores.LayoutPath) ")]
        public static string getLayoutPath(int tipo)
        {
            return getTexto(Valores.LayoutPath, tipo);
        }
        [Obsolete("Mejor usa:getTexto(Valores.FiltroInicial) ")]
        public static string getFiltroInicial()
        {
            return getTexto(Valores.FiltroInicial);
        }
       
        public static string getTextoAcercaDe()
        {
            return getTexto(Valores.AcercaDe);
        }

        [Obsolete("Mejor usa:getTexto(Valores.LenguajePath) ")]
        public static string getLenguajePath(String lenguaje)
        {
            return getTexto(Valores.LenguajePath, lenguaje);
        }
     
        /*
        /// /////////////////////////////////////////getInt
        */
        public static int getInt(Valores parametro,  int iParametro=-1)
        {
            string path = string.Empty;
            string sValor = string.Empty;
            int valor = -1;
            XDocument xDoc = null;
            path = Persistencia.ConfigPath + @"\ColumnasProgress.xml";
            string elemento = string.Empty;
            xDoc = XDocument.Load(path);
            try
            {
                switch (parametro)
                {
                    case Valores.LineasReservadas:
                        elemento = "IndiceColumnaLineasReservadas";
                        break;
                    case Valores.LineasPreparadas:
                        elemento = "IndiceColumnaLineasPreparadas";
                        break;
                    case Valores.PctOFProduccion:
                        elemento = "IndiceColumnaPctFabricacionOrdenes";
                        break;
                    case Valores.pctArticulosPteAcopio:
                        elemento = "IndiceColumnaPctPteAcopio";
                        break;
                    case Valores.pctLineasPedProCab:
                        elemento = "IndiceColumnaPorcentLineas";
                        break;
                    case Valores.pctLineasDevolCliCab:
                        elemento = "IndiceColumnaPorcentLineasDevolCli";
                        break;
                    case Valores.pctEntradas:
                        elemento = "IndiceColumnaLineasPorcentEntrada";
                        break;
                    case Valores.pctEntradasDevolCli:
                        elemento = "IndiceColumnaLineasPorcentEntradaDevolCli";
                        break;
                    case Valores.pctEntradasPendientes:
                        elemento = "IndiceColumnaLineaPorcentCantidadPteUbicar";
                        break;
                    case Valores.pctEntradasPendientesDevolCli:
                        elemento = "IndiceColumnaLineaPorcentCantidadPteUbicarDevolCli";
                        break;
                    case Valores.pctCantidadReservada:
                        elemento = "IndiceColumnaLineaPorcentCantidadPteUbicarDevolCli";
                        break;
                    case Valores.pctCantidadServida:
                        elemento = "IndiceColumnapctCantidadServida";
                        break;
                    case Valores.CantidadServida:
                        elemento = "IndiceColumnaCantidadServida";
                        break;
                    case Valores.CantidadReservada:
                        elemento = "IndiceColumnaCantidadReservada";
                        break;
                    default:
                        throw new Exception(parametro.ToString() + " No implementado en retorno entero");
                }
                sValor = xDoc.Element("Personalizacion").Element(elemento).Value;
                valor = Convert.ToInt32(sValor);
                return valor;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("no se ha encontrado el fichero" + "\n" + path);
                return -1;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("no se ha encontrado el elemento" + "\n" + elemento + "\n en el fichero \n"+ path);
                return -1;
            }
            catch (Exception e)
            {





                MessageBox.Show("se ha producido un error cargando el parametro XML" + "\n" + path+ "\n" + e.Message);
                return -1;
            }
        }

        public static int getLineasPreparadas(int pathGL)
        {
            return getInt(Valores.LineasPreparadas,pathGL);
        }
        public static int getLineasReservadas(int pathGL)
        {
            return getInt(Valores.LineasReservadas, pathGL);
        }
        public static int getPctFabricacionOrdenesProduccion(int pathGL)
        {
            return getInt(Valores.PctOFProduccion,pathGL);
        }
        public static int getPctArticulosPteAcopio(int pathGL)
        {
            return getInt(Valores.pctArticulosPteAcopio, pathGL);
        }
        public static int getPorcentLineasPedProCab(int pathGL)
        {
            return getInt(Valores.pctLineasPedProCab,pathGL);
        }
        public static int getPorcentLineasDevolCliCab()
        {
            return getInt(Valores.pctLineasDevolCliCab);
        }
        
        public static int getPorcentEntradas(int pathGL)
        {
            return getInt(Valores.pctEntradas,pathGL);
        }
        public static int getPorcentEntradasDevolCli()
        {
            return getInt(Valores.pctEntradasDevolCli);
        }
        
        public static int getPorcentEntradasPendientes(int pathGL)
        {
            return getInt(Valores.pctEntradasPendientes, pathGL);
        }
        public static int getPorcentEntradasPendientesDevolCli()
        {
            return getInt(Valores.pctEntradasPendientesDevolCli);
        }
      
        public static int getCantidadReservadaPorc(int pathGL)
        {
            return getInt(Valores.pctCantidadReservada, pathGL);
        }
        
        public static int getCantidadServidaPorc(int pathGL)
        {
            return getInt(Valores.pctCantidadServida, pathGL);
        }
        public static int getCantidadServida(int pathGL)
        {
            return getInt(Valores.CantidadServida, pathGL);
        }
        public static int getCantidadReservada(int pathGL)
        {
            return getInt(Valores.CantidadReservada,pathGL);
        }
        /*
        * set**************************
        */
        public static void SetInt(Valores parametro, int iParametro )
        {
            string elemento = string.Empty;
            string path = Persistencia.ConfigPath + @"\ColumnasProgress.xml";
            XDocument xDoc = XDocument.Load(path);
            try
            {
                switch (parametro)
                {
                    case Valores.pctCantidadServida:
                        elemento= "IndiceColumnapctCantidadServida";
                        break;
                    case Valores.CantidadServida:
                        elemento = "CantidadServida";
                        break;
                    case Valores.pctCantidadReservada:
                        elemento = "IndiceColumnapctCantidadReservada";
                        break;
                    case Valores.CantidadReservada:
                        elemento = "IndiceColumnaCantidadReservada";
                        break;
                    case Valores.pctEntradasPendientes:
                        elemento = "IndiceColumnaLineaPorcentCantidadPteUbicar";
                        break;
                    case Valores.pctEntradasPendientesDevolCli:
                        elemento = "IndiceColumnaLineaPorcentCantidadPteUbicarDevolCli";
                        break;
                    case Valores.LineasReservadas:
                        elemento = "IndiceColumnaLineasReservadas";
                        break;
                    case Valores.LineasPreparadas:
                        elemento = "IndiceColumnaLineasPreparadas";
                        break;
                    case Valores.PctOFProduccion:
                        elemento = "IndiceColumnaPctFabricacionOrdenes";
                        break;
                    case Valores.pctArticulosPteAcopio:
                        elemento = "IndiceColumnaPctPteAcopio";
                        break;
                    case Valores.pctLineasPedProCab:
                        elemento = "IndiceColumnaPorcentLineas";
                        break;
                    case Valores.pctLineasDevolCliCab:
                        elemento = "IndiceColumnaPorcentLineasDevolCli";
                        break;
                    case Valores.pctEntradas:
                        elemento = "IndiceColumnaLineasPorcentEntrada";
                        break;
                    case Valores.pctEntradasDevolCli:
                        elemento = "IndiceColumnaLineasPorcentEntradaDevolCli";
                        break;
                }
                xDoc.Element("Personalizacion").Element(elemento).SetValue(iParametro);
                xDoc.Save(path);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("no se ha encontrado el fichero" + "\n" + path);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        public static void setCantidadServidaPorc(int index,int pathGL)
        {
            SetInt(Valores.pctCantidadServida, index);
        }
       
        public static void setCantidadServida(int index, int pathGL)
        {
            SetInt(Valores.CantidadServida, index);
        }
        public static void setCantidadReservadaPorc(int index, int pathGL)
        {
            SetInt(Valores.pctCantidadReservada, index);
        }
        public static void setCantidadReservada(int index, int pathGL)
        {
            SetInt(Valores.CantidadReservada, index);
        }
        public static void setPorcentEntradasPendientes(int index, int pathGL)
        {
            SetInt(Valores.pctEntradasPendientes, index);
        }
        public static void setPorcentEntradasPendientesDevolCli(int index)
        {
            SetInt(Valores.pctEntradasPendientesDevolCli, index);
        }
        public static void setLineasReservadas(int index, int pathGL)
        {
            SetInt(Valores.LineasReservadas, index);
        }

        public static void setPaginacion(int size)
        {
            try
            {
                string path = @"XML\PersonalizacionUsuario.xml";
                XDocument X = XDocument.Load(path);
                X.Element("Personalizacion").Element("Paginacion").SetValue(size);
                X.Save(path);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        public static int getPaginacion()
        {
            string path = @"XML\PersonalizacionUsuario.xml";
            int size = 0;
            try
            {
                XDocument X = XDocument.Load(path);
                size = Convert.ToInt32(X.Element("Personalizacion").Element("Paginacion").Value);
            }
            catch (Exception e)
            {
                Debug.WriteLine(path);
                Debug.WriteLine(e.Message);
            }
            return size;
        }
        
        public static void setLineasPreparadas(int index, int pathGL)
        {
            SetInt(Valores.LineasPreparadas, index);
        }

        public static void setPctFabricacionOrdenesProduccion(int index, int pathGL)
        {
            SetInt(Valores.PctOFProduccion, index);
        }
        public static void setPctArticulosPteAcopio(int index, int pathGL)
        {
            SetInt(Valores.pctArticulosPteAcopio, index);
        }
        public static void setPorcentLineasPedProCab(int index, int pathGL)
        {
            SetInt(Valores.pctLineasPedProCab, index);
        }
        public static void setPorcentLineasDevolCliCab(int index)
        {
            SetInt(Valores.pctLineasDevolCliCab, index);
        }
        public static void setPorcentEntradas(int index, int pathGL)
        {
            SetInt(Valores.pctEntradas, index);
        }
        public static void setPorcentEntradasDevolCli(int index)
        {
            SetInt(Valores.pctEntradasDevolCli, index);
        }

        #region ColorPedidos
        public static Color getColorPedidos(string estado)
        {
            string path = @"XML\coloresPedidos.xml";
            Color colorcito = new Color();
            try
            {
                XDocument X = XDocument.Load(path);
                string color = "";
                color = X.Element("Colores").Element("Color" + estado).Value;
                colorcito = Color.FromArgb(Convert.ToInt32(color));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return colorcito;
        }
        public static void setColorPedidos(string color, string estado)
        {

            try
            {
                string path = @"XML\coloresPedidos.xml";
                XDocument X = XDocument.Load(path);
                X.Element("Colores").Element("Color" + estado).SetValue(color);
                X.Save(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string getControlTareas()
        {
            string selectSQL = string.Empty;
            string path = @"XML\controlTareas.xml";
            try
            {
                XDocument X = XDocument.Load(path);
                selectSQL = X.Element("SQL").Element("SELECT").Value;
            }
            catch (Exception e)
            {
                Debug.WriteLine(path);
                Debug.WriteLine(e.Message);
            }
            return selectSQL;
        }
#endregion
        #region ColorMaquina
        public static Color getColorMaquina(string estado)
        {
            try
            {
                string path = @"XML\coloresMaquina.xml";
                XDocument X = XDocument.Load(path);
                string color = "";
                color = X.Element("Colores").Element("Color"+estado).Value;
                Color colorcito = new Color();
                colorcito = Color.FromArgb(Convert.ToInt32(color));
                return colorcito;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Color.Black;
            }

        }
        public static void setColorMaquina(string color, string estado)
        {
            string path = @"XML\coloresMaquina.xml";
            XDocument X = XDocument.Load(path);
            X.Element("Colores").Element("Color"+estado).SetValue(color);
            X.Save(path);
        }
        #endregion

        #region ColorTipoTareas
        public static Color getColorTipoTareas(string estado)
        {
            string color = "";
            string path = @"XML\coloresTipoTareas.xml";
            try
            {
                XDocument X = XDocument.Load(path);
                color = X.Element("Colores").Element("Color"+estado).Value;
                Color colorcito = new Color();
                colorcito = Color.FromArgb(Convert.ToInt32(color));
                Color color2 = Color.FromArgb(255, colorcito);
                return color2;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "||" + e.StackTrace);
                return Color.Black;
            }
        }
        public static void setColorTipoTareas(string color, string estado)
        {
            try
            {
                string path = @"XML\coloresTipoTareas.xml";
                XDocument X = XDocument.Load(path);
                switch (estado)
                {
                    case "CARGA":
                        X.Element("Colores").Element("ColorCA").SetValue(color);
                        break;
                    case "CONTEO":
                        X.Element("Colores").Element("ColorCO").SetValue(color);
                        break;
                    case "PICKING ALM1":
                        X.Element("Colores").Element("ColorPI").SetValue(color);
                        break;
                    case "PICKING NAVE":
                        X.Element("Colores").Element("ColorPINA").SetValue(color);
                        break;
                    case "UBICACION PALET COMPLETO":
                        X.Element("Colores").Element("ColorUBI").SetValue(color);
                        break;
                    case "UBICACION PALET":
                        X.Element("Colores").Element("ColorPALET").SetValue(color);
                        break;
                    case "TAREA HACIA LA PLAYA":
                        X.Element("Colores").Element("ColorPLAYA").SetValue(color);
                        break;
                    case "UBICACION CARRO MOVIL":
                        X.Element("Colores").Element("ColorMOV").SetValue(color);
                        break;
                    case "TAREA DIRECTA A MUELLE":
                        X.Element("Colores").Element("ColorUP").SetValue(color);
                        break;
                    case "TAREA DE PLAYA A MUELLE":
                        X.Element("Colores").Element("ColorPLM").SetValue(color);
                        break;
                    case "ACOPIO":
                        X.Element("Colores").Element("ColorACOP").SetValue(color);
                        break;
                    case "ACOPIO ALM1":
                        X.Element("Colores").Element("ColorALM1").SetValue(color);
                        break;
                    case "REPOSICION":
                        X.Element("Colores").Element("ColorREP").SetValue(color);
                        break;
                    case "DESGLOSE DE CARRO MOVIL":
                        X.Element("Colores").Element("ColorDES").SetValue(color);
                        break;
                    case "ACOPIO ALM2":
                        X.Element("Colores").Element("ColorALM2").SetValue(color);
                        break;
                    case "PICKING CUT":
                        X.Element("Colores").Element("ColorCUT").SetValue(color);
                        break;
                    case "OLA PICKING":
                        X.Element("Colores").Element("ColorOLA").SetValue(color);
                        break;
                    case "UBICACION ACOPIO":
                        X.Element("Colores").Element("ColorUBIACOP").SetValue(color);
                        break;
                    case "PICKING MODULA":
                        X.Element("Colores").Element("ColorPIMOD").SetValue(color);
                        break;
                    case "OL PICKING MODULA":
                        X.Element("Colores").Element("ColorOLPIMOD").SetValue(color);
                        break;
                    case "UBICACION REPOSICIONES CARRO":
                        X.Element("Colores").Element("ColorUBIREPO").SetValue(color);
                        break;
                    case "PICKING ALM2":
                        X.Element("Colores").Element("ColorPI2").SetValue(color);
                        break;
                    case "ABONO PACKING LIST":
                        X.Element("Colores").Element("ColorLIST").SetValue(color);
                        break;
                    case "PICKING SAN JUAN":
                        X.Element("Colores").Element("ColorSJ").SetValue(color);
                        break;
                    case "PICKING VALLONGA":
                        X.Element("Colores").Element("ColorVLL").SetValue(color);
                        break;
                    case "ACOPIO SAN JUAN":
                        X.Element("Colores").Element("ColorACSJ").SetValue(color);
                        break;
                    case "UBICACION SAN JUAN":
                        X.Element("Colores").Element("ColorUBSJ").SetValue(color);
                        break;
                    case "ALTA MANUAL":
                        X.Element("Colores").Element("ColorALTA").SetValue(color);
                        break;
                    case "DESGLOSE MULTIREFERENCIA":
                        X.Element("Colores").Element("ColorDEMULTI").SetValue(color);
                        break;
                    case "PICKING":
                        X.Element("Colores").Element("ColorPICKING").SetValue(color);
                        break;
                    case "PICKING RESERVA":
                        X.Element("Colores").Element("ColorPIRES").SetValue(color);
                        break;
                    case "PICKING EXTERIOR":
                        X.Element("Colores").Element("ColorPIEX").SetValue(color);
                        break;
                    case "PICKING LARGOS":
                        X.Element("Colores").Element("ColorPILAR").SetValue(color);
                        break;
                    case "REPOSICION ESTANTERIA APILABLE":
                        X.Element("Colores").Element("ColorESTANPILA").SetValue(color);
                        break;
                    case "REPOSICION EXTERIOR":
                        X.Element("Colores").Element("ColorREPEXT").SetValue(color);
                        break;
                    case "REPOSICION RESERVA":
                        X.Element("Colores").Element("ColorREPRES").SetValue(color);
                        break;
                    case "UBICACION EXTERIOR":
                        X.Element("Colores").Element("ColorUBIEXT").SetValue(color);
                        break;
                    case "UBICACION PALET RESERVA":
                        X.Element("Colores").Element("ColorUPARES").SetValue(color);
                        break;
                    case "UBICACION PICKING":
                        X.Element("Colores").Element("ColorUBIPICKING").SetValue(color);
                        break;
                    case "UBICACION ZCALIENTE":
                        X.Element("Colores").Element("ColorUPZ").SetValue(color);
                        break;
                    case "PICKING 1-0":
                        X.Element("Colores").Element("ColorPI10").SetValue(color);
                        break;
                    case "UBICACION PLANTA 1-0":
                        X.Element("Colores").Element("ColorUPA10").SetValue(color);
                        break;
                    case "UBICACION PLANTA 1-1":
                        X.Element("Colores").Element("ColorUBIPA11").SetValue(color);
                        break;
                    case "RECEPCION ADMINISTRATIVA":
                        X.Element("Colores").Element("ColorRECADM").SetValue(color);
                        break;
                    case "PICKING TERMOS":
                        X.Element("Colores").Element("ColorPICTER").SetValue(color);
                        break;
                    case "REPOSICION TERMOS":
                        X.Element("Colores").Element("ColorREPTER").SetValue(color);
                        break;
                    case "REPOSICION UDS SUELTAS":
                        X.Element("Colores").Element("ColorUDS").SetValue(color);
                        break;
                    case "ACOPIO ACONDICIONAMIENTO":
                        X.Element("Colores").Element("ColorACONDI").SetValue(color);
                        break;
                    case "ACOPIO MP":
                        X.Element("Colores").Element("ColorACMP").SetValue(color);
                        break;
                    case "UBICACION ACONDICIONAMIENTO":
                        X.Element("Colores").Element("ColorUBIACOND").SetValue(color);
                        break;
                    case "UBICACION MATERIA PRIMA":
                        X.Element("Colores").Element("ColorMATPRIMA").SetValue(color);
                        break;
                    case "UBICACION PRODUCTO TERMINADO":
                        X.Element("Colores").Element("ColorUBIPRODTERM").SetValue(color);
                        break;
                }
                X.Save(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "||" + e.StackTrace);
            }
        }
        #endregion
        #region ColorEstadoTareas
        public static Color getColorTareas(string estado)
        {
            string color = "";
            string path = @"XML\coloresTareasEstado.xml";
            try
            {
                XDocument X = XDocument.Load(path);
                color = X.Element("Colores").Element("Color"+estado).Value;
                Color colorcito = new Color();
                colorcito = Color.FromArgb(Convert.ToInt32(color));
                return colorcito;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                return Color.Black;
            }

        }
        public static void setColorTareas(string color, string estado)
        {
            try
            {
                string path = @"XML\coloresTareasEstado.xml";
                XDocument X = XDocument.Load(path);
                X.Element("Colores").Element("Color"+estado).SetValue(color);
                X.Save(path);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        #endregion
        #region ColorEstadoOperarios
        public static Color getColorOperarios(string estado)
        {
            string color = "";
            string path = @"XML\coloresTareasEstado.xml";
            try
            {
                XDocument X = XDocument.Load(path);
                color = X.Element("Colores").Element("Color"+estado).Value;
                Color colorcito = new Color();
                colorcito = Color.FromArgb(Convert.ToInt32(color));
                return colorcito;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                return Color.Black;
            }
            
        }
        public static void setColorOperarios(string color, string estado)
        {
            try
            {
                string path = @"XML\coloresTareasEstado.xml";
                XDocument X = XDocument.Load(path);
                X.Element("Colores").Element("Color"+estado).SetValue(color);
                X.Save(path);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        #endregion
        
        public static List<DataTable> GetPackingTreeNodes()
        {
            List<DataTable> datos = new List<DataTable>();
            string path = @"XML\arbolPacking.xml";
            XDocument X = XDocument.Load(path);
            foreach (XElement item in X.Descendants("arbol"))
            {
                foreach (XElement item2 in item.Nodes())
                {
                    string text = item2.Value.ToString();
                    if (text!=string.Empty)
                    {
                        text = text.Replace("%", "<>");
                        datos.Add(ConexionSQL.getDataTable(text));
                        Debug.WriteLine(text);
                    }

                }

            }
            return datos;
        }
      
    }
}

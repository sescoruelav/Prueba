using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.GestionAlmacen;
using RumboSGA.Presentation.FormulariosComunes;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Telerik.Documents.SpreadsheetStreaming;
using Telerik.WinControls;
using Telerik.WinControls.Spreadsheet.UI;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Utilities;
using Telerik.Windows.Documents.Spreadsheet;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.WinForms.Controls.Spreadsheet.Worksheets;

namespace RumboSGA.Presentation.Herramientas
{
    public partial class FrmMapaAlmacen : Telerik.WinControls.UI.RadRibbonForm
    {
        const int portalMuyAlto = 50;//portal equivale a fila
        const int columnaMuyAlta = 50;
        const int maxEscalaColoresLlenado = 11;
        Boolean libroModificado = false;
        char[] charsToTrim = { '*', ' ', '\'' };
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string aceraT = Lenguaje.traduce ("ACERA"); //TODO traducir acera
        string plantaTraducido = Lenguaje.traduce("PLANTA");
        string alzadoTraducido = Lenguaje.traduce("ALZADO");
        string ocupacionTraducido = Lenguaje.traduce("OCUPACION");
        CellValueFormat formato = null;

        public FrmMapaAlmacen()
        {
            InitializeComponent();//TODO superlento 8 segundos
            radProgressBar1.Visible = false;
            this.Text = Lenguaje.traduce(this.Text);
            radRibbonBarGroup1.Text = (Lenguaje.traduce(radRibbonBarGroup1.Text));
        }

        private void btnNuevaPlantilla_Click(object sender, EventArgs e)
        {
            AnyadirNuevaPlantilla();
        }
        private void btnLlenado_Click(object sender, EventArgs e)
        {
            CalculoLlenadoPorUbicaciones();
        }
        private void btnReferenciaPicking_Click(object sender, EventArgs e)
        {
            CalculoReferenciaPicking();
        }
        private void btnInfoCelda_Click(object sender, EventArgs e)
        {
            CalculoInfoCelda();
        }
        private void btnFrecuenciaAcceso_Click(object sender, EventArgs e)
        {
            CalculoFrecuenciaAcceso();
        }

        private void btnZonas_Click(object sender, EventArgs e)
        {
            MostrarZona();
        }
        private void MostrarZona()
        {
            string ubicacion = "";
            
            int zonaActual = 1;
            try
            {
                FrmSeleccionZona frm = new FrmSeleccionZona();
                frm.ShowDialog();
                frm.TopMost = true;
                if (!frm.DialogResult.Equals(DialogResult.OK))
                {
                    return;
                }
                int[] zona = new int[3];
                PatternFill colorZona = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 0, 0, 255), System.Windows.Media.Color.FromArgb(0, 255, 0, 0));
                PatternFill colorBlanco = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 255, 255, 255), System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
                PatternFill[] color = new PatternFill[3];
                //Telerik.WinControls.Spreadsheet.UI.Color color = frm.color1;
                ThemableColor fcolor = ColorHelper.DrawingToThemableColor(frm.color1);
                color[0] = new PatternFill(PatternType.Solid, fcolor, fcolor);
                zona[0] = frm.zona1;
                fcolor = ColorHelper.DrawingToThemableColor(frm.color2);
                color[1] = new PatternFill(PatternType.Solid, fcolor, fcolor);
                zona[1] = frm.zona2;
                fcolor = ColorHelper.DrawingToThemableColor(frm.color3);
                color[2] = new PatternFill(PatternType.Solid, fcolor, fcolor);
                zona[2] = frm.zona3;
                

                Worksheet ws = duplicarHoja( Lenguaje.traduce("Zonas"));//creo una hoja moficada
                if (ws is null)
                    return;

                //Calcular el rango de celdas usadas.
                CellRange usedCellRange = ws.UsedCellRange;
                //las marco en blanco
                for( zonaActual=1; zonaActual<=3 ; zonaActual++)
                {
                    //Inicializo las consultas SQL
                    DataTable dtFrecuencia = Business.getFrecuenciaAcceso("Zona", zona[zonaActual-1]);
                    if (dtFrecuencia != null)
                    {
                        for (int rowIndex = usedCellRange.FromIndex.RowIndex; rowIndex <= usedCellRange.ToIndex.RowIndex; rowIndex++)
                        {
                            for (int columnIndex = usedCellRange.FromIndex.ColumnIndex; columnIndex <= usedCellRange.ToIndex.ColumnIndex; columnIndex++)
                            {
                                CellSelection selection = ws.Cells[rowIndex, columnIndex];
                                ICellValue cellValue = selection.GetValue().Value;
                                ubicacion = cellValue.GetResultValueAsString(formato);
                                if (ubicacion.StartsWith("W"))
                                {
                                    bool enZona = false;
                                    DataRow[] dr = dtFrecuencia.Select("UBICACION= '" + ubicacion + "'");
                                    foreach (DataRow row in dr)
                                    {
                                        enZona = true;
                                        if(zonaActual==1)
                                            ws.Cells[rowIndex, columnIndex].SetFill(color[0]);
                                        else
                                        {

                                            PatternFill p = (PatternFill) ws.Cells[rowIndex, columnIndex].GetFill().Value;
                                            PatternFill fill = SetfillMix(p, color[zonaActual - 1]);
                                            
                                            ws.Cells[rowIndex, columnIndex].SetFill(fill);
                                        }
                                    }
                                    if (!enZona && zonaActual == 1)
                                        ws.Cells[rowIndex, columnIndex].SetFill(colorBlanco);
                                }
                            }
                        }
                        dtFrecuencia.Dispose();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("ERROR mostrando zona" +"\n"+ex.Message));
            }
        }
        private PatternFill SetfillMix(PatternFill p1, PatternFill p2)
        {
            try
            {
                ThemableColor c1 = p1.BackgroundColor;
                ThemableColor c2 = p2.BackgroundColor;
                System.Windows.Media.Color co1 = c1.LocalValue;//getactualvalue
                System.Windows.Media.Color co2 = c2.LocalValue;
                if((co1.R!=0) || (co1.G != 0) || (co1.B != 0))
                {
                    int _r = Math.Min((co1.R/2 + co2.R/2), 255);
                    int _g = Math.Min((co1.G/2 + co2.G/2), 255);
                    int _b = Math.Min((co1.B/2 + co2.B/2), 255);

                    PatternFill colorZona = new PatternFill(PatternType.Solid, 
                        System.Windows.Media.Color.FromArgb(128, Convert.ToByte(_r), Convert.ToByte(_g), Convert.ToByte(_b)),
                        System.Windows.Media.Color.FromArgb(128, Convert.ToByte(_r), Convert.ToByte(_g), Convert.ToByte(_b)));
                    return colorZona;
                }
                else
                {
                    return p2;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("Se ha producido un error al poner color a la celda")+ "\n"+ex.Message);
            }
            return null;
        }
        private void AnyadirNuevaPlantilla()
        {
            try
            {
                Workbook libro = radSSheetHojaCalculo.Workbook;
                if ((libro.Worksheets.Count > 0) && libroModificado)
                {
                    GuardarLibro(libro);
                }
                /*NO FUNCIONA BorrarLibroActual(libro);*/
                cargaDatosAlmacen(libro);
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }

        }

        private void BorrarLibroActual(Workbook libro)
        {
            try
            {
                while (libro.Sheets.Count > 0)
                {
                    libro.Sheets.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void cargaDatosAlmacen(Workbook libro)
        {
            try
            {
                FrmSeleccionAlmacen frm = new FrmSeleccionAlmacen();
                frm.ShowDialog();
                frm.TopMost = true;
                if(!frm.DialogResult.Equals(  DialogResult.OK))
                {
                    return;
                }

                int almacen = frm.almacen;
                int celda = frm.celda;
                bool vertical=frm.orientacion.Equals("Vertical",StringComparison.OrdinalIgnoreCase);
                bool aceraCreciente=frm.acerasCrecientes.Equals("Números crecientes", StringComparison.OrdinalIgnoreCase);
                bool portalCreciente= frm.portalesCrecientes.Equals("Números crecientes", StringComparison.OrdinalIgnoreCase);
                string prefijoNombreHoja = frm.nombreHoja;
                int nivel = -1;
                int fila = 1;
                int columna = 1;
                int columnaAlzado = 1;
                CellSelection selection;
                int pisoActual=-1;
                int pisoAnterior = -1;
                int portalAnterior = -1;
                Boolean cambioAcera = true;

                PatternFill colorEstanteria = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 255, 0, 0), System.Windows.Media.Color.FromArgb(0, 255, 0, 0));
                PatternFill colorPasillo = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 0, 255, 0), System.Windows.Media.Color.FromArgb(0, 0, 255, 0));

                libroModificado = true;
                //creo hojas del libro
                Worksheet wsPlanta= libro.Worksheets.Add();
                if(libro.Worksheets.Contains(prefijoNombreHoja + plantaTraducido))
                {
                    MessageBox.Show(Lenguaje.traduce("El nombre de hoja ya existe:"+ prefijoNombreHoja + plantaTraducido));
                    return;
                }
                wsPlanta.Name=prefijoNombreHoja + plantaTraducido;
                Worksheet wsAlzado = libro.Worksheets.Add();
                wsAlzado.Name = prefijoNombreHoja + alzadoTraducido;
                libro.ActiveWorksheet = libro.Worksheets[wsPlanta.Name];

                string ordenAcera;
                if (aceraCreciente)
                    ordenAcera = "acera ASC";
                else
                    ordenAcera = "acera Desc";


                DataTable dAceras = Business.getAcerasAlmacen(almacen,ordenAcera );
                foreach (DataRow rowAcera in dAceras.Rows)
                {
                    CellRange rango = new CellRange(0, 0, portalMuyAlto, 0);
                    selection = wsPlanta.Cells[rango];
                    
                    selection.SetValue(Convert.ToString("-"));
                    selection.SetFill(colorPasillo);
                    int acera = Convert.ToInt32(rowAcera["acera"]);
                    columna = acera * 2 - acera % 2; // calculo la columna excel donde escribir
                    if (acera % 2 == 1) //ajusto las celdas de pasillo 
                    {   //en las impares la acera debe estar a la derecha, uso +2 por si hay aceras irregulares
                        if(vertical)
                            selection = wsPlanta.Cells[new CellRange(0,columna,portalMuyAlto,columna+2)];
                        else
                            selection = wsPlanta.Cells[new CellRange(columna, 0, columna + 2, portalMuyAlto)];
                    }
                    else
                    {   //en las pares la acera queda a la izquierda.
                        if(vertical)
                            selection = wsPlanta.Cells[new CellRange(0, columna, portalMuyAlto, columna )];
                        else
                            selection = wsPlanta.Cells[new CellRange(columna, 0, columna , portalMuyAlto)];

                    }
                    selection.SetValue(Convert.ToString("-"));
                    selection.SetFill(colorPasillo);
                    AjustarCeldaUbicacion(wsPlanta, 0, columna, vertical, aceraT + "-" + acera.ToString());
                    //relleno los portales 
                    DataTable dHuecosAcera = Business.getHuecosAlmacenAcera(almacen, acera,nivel,portalCreciente);
                    foreach(DataRow rowHueco in dHuecosAcera.Rows)
                    {
                        fila = CalculaFila(Convert.ToInt32(rowHueco["portal"]), portalCreciente);
                        pisoActual = Convert.ToInt32(rowHueco["piso"]);
                        if ((pisoActual != pisoAnterior)||cambioAcera)
                        {
                            cambioAcera = false;
                            columnaAlzado++;
                            if (pisoActual < pisoAnterior) {
                                AjustarCeldaUbicacion(wsAlzado, 0, columnaAlzado, vertical, ">" + pisoActual.ToString() + ">");
                            }
                            else{
                                AjustarCeldaUbicacion(wsAlzado, 0, columnaAlzado, vertical, "<" + pisoActual.ToString() + "<");
                            }
                            AjustarCeldaUbicacion(wsAlzado, 0, columnaAlzado, vertical, colorPasillo);
                            pisoAnterior = pisoActual;
                        }
                        if (pisoActual==1)//actualizo la hoja planta
                        {
                            portalAnterior = fila;
                            AjustarCeldaUbicacion(wsPlanta, fila, columna, vertical, Convert.ToString(rowHueco["descripcion"]).Trim(charsToTrim));
                            AjustarCeldaUbicacion(wsPlanta, fila, columna, vertical, colorEstanteria);
                            AjustarCeldaUbicacion(wsPlanta, fila, (columna - 1) + (columna % 2) * 2, vertical, Convert.ToString(rowHueco["portal"]));

                        }
                        AjustarCeldaUbicacion(wsAlzado, fila, columnaAlzado, vertical, colorEstanteria);
                        AjustarCeldaUbicacion(wsAlzado, fila, columnaAlzado, vertical, Convert.ToString(rowHueco["descripcion"]).Trim(charsToTrim));

                    }
                    columnaAlzado += 2;
                    cambioAcera = true;
                }
                libro.ActiveWorksheet = libro.Worksheets[wsAlzado.Name];
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("ERROR creando mapa de almacén:")+"\n" + ex.Message);
            }
        }
        int CalculaFila(int portal, Boolean portalCreciente)
        {
            if (portalCreciente)
                return portal;
            else
                return portalMuyAlto - portal;
        }
        private void AjustarCeldaUbicacion(Worksheet ws, int fila, int columna, Boolean vertical, String valor)
        {
            if (vertical)
                ws.Cells[fila, columna].SetValue(valor);
            else
                ws.Cells[columna, fila].SetValue(valor);
        }
        private void AjustarCeldaUbicacion(Worksheet ws, int fila, int columna, Boolean vertical, PatternFill fill)
        {
            if (vertical)
                ws.Cells[fila, columna].SetFill(fill);
            else
                ws.Cells[columna, fila].SetFill(fill);
        }
       
        private void CalculoFrecuenciaAcceso()
        {
            string ubicacion = "";
            int dias; 
            try
            {

                Worksheet ws = duplicarHoja("FrecuenciaAcceso");//creo una hoja moficada
                if (ws is null)
                    return;
                // pedir por pantalla los dias de historico.
                dias = PedirFechaDesde();
                if (dias == 0)
                    return;
                //Inicializo las consultas SQL
                DataTable dtFrecuencia = Business.getFrecuenciaAcceso("FrecuenciaAcceso", dias);
                if (dtFrecuencia is null)
                {
                    MessageBox.Show(Lenguaje.traduce("Error recuperando datos de frecuencias de acceso"));
                    return;
                }
                double maxFrecuencia=0;
                //int maxFrecuencia = Convert.ToInt32(dtFrecuencia.Compute("max([frecuencia])", string.Empty));
                //Calcular el rango de celdas usadas.
                CellRange usedCellRange = ws.UsedCellRange;
                //Como es un proceso lento muestro una barra de progreso
                ActivarProgressBar(0, 2* (usedCellRange.ToIndex.RowIndex - usedCellRange.FromIndex.RowIndex + 1) * (usedCellRange.ToIndex.ColumnIndex - usedCellRange.FromIndex.ColumnIndex + 1) + 1);
                for (int rowIndex = usedCellRange.FromIndex.RowIndex; rowIndex <= usedCellRange.ToIndex.RowIndex; rowIndex++)
                {
                    for (int columnIndex = usedCellRange.FromIndex.ColumnIndex; columnIndex <= usedCellRange.ToIndex.ColumnIndex; columnIndex++)
                    {
                        CellSelection selection = ws.Cells[rowIndex, columnIndex];
                        ICellValue cellValue = selection.GetValue().Value;
                        ubicacion = cellValue.GetResultValueAsString(formato);
                        if (ubicacion.StartsWith("W"))
                        {
                            double visitas=0;
                            DataRow[] dr = dtFrecuencia.Select("UBICACION= '" + ubicacion + "'");
                            foreach (DataRow row in dr)
                            {
                                visitas=Convert.ToDouble(row["frecuencia"]);
                            }
                            maxFrecuencia  = Math.Max(maxFrecuencia, visitas);
                            
                        }
                        ProgressBarInc();
                    }
                }
                //bucle para colorear
                for (int rowIndex = usedCellRange.FromIndex.RowIndex; rowIndex <= usedCellRange.ToIndex.RowIndex; rowIndex++)
                {
                    for (int columnIndex = usedCellRange.FromIndex.ColumnIndex; columnIndex <= usedCellRange.ToIndex.ColumnIndex; columnIndex++)
                    {
                        CellSelection selection = ws.Cells[rowIndex, columnIndex];
                        ICellValue cellValue = selection.GetValue().Value;
                        ubicacion = cellValue.GetResultValueAsString(formato);
                        if (ubicacion.StartsWith("W"))
                        {
                            double visitas = 0;
                            DataRow[] dr = dtFrecuencia.Select("UBICACION= '" + ubicacion + "'");
                            foreach (DataRow row in dr)
                            {
                                visitas = Convert.ToDouble(row["frecuencia"]);
                            }
                            ws.Cells[rowIndex, columnIndex].SetValue(Convert.ToString(visitas));
                            ws.Cells[rowIndex, columnIndex].SetFill(GetEscalaColor(ws, visitas / maxFrecuencia));
                        }
                        ProgressBarInc();
                    }
                }

                //cierro conexion y progressbar
                desactivarProgressBar();  
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR calculando llenado del mapa de almacén:\n" + ex.Message);
            }
        }
        
        int PedirFechaDesde()
        {
            try
            {
                FrmSeleccionarFecha frm = new FrmSeleccionarFecha();
                frm.ShowDialog();
                frm.TopMost = true;
                if (!frm.DialogResult.Equals(DialogResult.OK))
                {
                    return 0;
                }

                DateTime dtIni = frm.fechaSeleccionada;
                DateTime dtHoy = DateTime.Now;
                TimeSpan ts = dtHoy - dtIni;
                return ts.Days;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("ERROR en la fecha:" ) + "\n" + ex.Message);
            }
            return 0;
        }
        private void CalculoLlenadoPorUbicaciones()
        {
            //double[] patron=new double[maxEscalaColoresLlenado];
            //IFill[] escalaColores = new IFill[maxEscalaColoresLlenado];
            int existencias = 0;
            int cantidadExistencia = 0;
            int capacidadExistenciasUbicacion = 0;
            int cantidadPaletArticulo = 1;
            double ocupacion;
            CellValueFormat formatoPorcentaje= new CellValueFormat("0,00%");
            try
            {
                Worksheet ws = duplicarHoja("Ocupacion");//creo una hoja moficada
                if (ws is null)
                    return;
                //Inicializo las consultas SQL
                DataTable dtFrecuencia = Business.getFrecuenciaAcceso("LLENADOPORUBICACIONES", 465);
                if (dtFrecuencia is null)
                {
                    MessageBox.Show("Error recuperando datos de ubicaciones");
                    return;
                }
                formato = ws.Cells[5, 5].GetFormat().Value;//almaceno un formato para usarlo //TODO mejorar. 
                //CargarEscalaColorLlenado(wsLlenado ,patron, escalaColores);
                //Calcular el rango de celdas usadas.
                CellRange usedCellRange = ws.UsedCellRange;
                ActivarProgressBar(0, (usedCellRange.ToIndex.RowIndex - usedCellRange.FromIndex.RowIndex + 1) * (usedCellRange.ToIndex.ColumnIndex - usedCellRange.FromIndex.ColumnIndex + 1) + 1);
                for (int rowIndex = usedCellRange.FromIndex.RowIndex; rowIndex <= usedCellRange.ToIndex.RowIndex; rowIndex++)
                {
                    for (int columnIndex = usedCellRange.FromIndex.ColumnIndex; columnIndex <= usedCellRange.ToIndex.ColumnIndex; columnIndex++)
                    {
                        CellSelection selection = ws.Cells[rowIndex, columnIndex];
                        ICellValue cellValue = selection.GetValue().Value;
                        string ubicacion = cellValue.GetResultValueAsString(formato);
                        if (ubicacion.StartsWith("W"))
                        {
                            DataRow[] dr = dtFrecuencia.Select("UBICACION= '" + ubicacion + "'");
                            existencias = 0;
                            foreach (DataRow row in dr)
                            {
                                existencias++;
                                cantidadExistencia =Convert.ToInt32( row["cantidad"]);
                                capacidadExistenciasUbicacion = Convert.ToInt32(row["capacidad"]);
                                cantidadPaletArticulo = Convert.ToInt32(row["cantidadPalet"]);
                            }
                            if (existencias == 1)//si es monoubicacion la ocupacion es la del las unidades del palet si no de la capacidad existencias
                            {
                                if (cantidadPaletArticulo < cantidadExistencia || (cantidadPaletArticulo <= 0))
                                    cantidadPaletArticulo = cantidadExistencia;//la cantidad del palet aqui no puede ser menor que el contenido. 
                                ocupacion = cantidadExistencia / cantidadPaletArticulo;
                            }
                            else
                                if (existencias == 0)
                                    ocupacion = 0.0;
                                else
                                ocupacion = Convert.ToDouble (existencias) / capacidadExistenciasUbicacion;
                            ws.Cells[rowIndex, columnIndex].SetFill(GetEscalaColor(ws,ocupacion));
                            ws.Cells[rowIndex, columnIndex].SetValue(ocupacion);
                            ws.Cells[rowIndex, columnIndex].SetFormat(formatoPorcentaje);
                        }
                        ProgressBarInc();
                    }
                }
                radProgressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR calculando llenado del mapa de almacén:\n" + ex.Message);
            }
        }
        
        IFill GetEscalaColor(Worksheet ws, double valor)
        {
            ICellValue cellValue;
            double rango;
            try
            {
                int i = 0;
                do
                {
                    cellValue = ws.Cells[i, 0].GetValue().Value;
                    rango = Convert.ToDouble(cellValue.RawValue);
                    i++;
                }
                while (i < maxEscalaColoresLlenado && rango < valor);
                return ws.Cells[i-1, 0].GetFill().Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce(  "ERROR calculando el color del rango:" + "\n" + ex.Message));
            }
            return null;
        }
        private void CalculoReferenciaPicking()
        {
            try
            {
                Worksheet ws = duplicarHoja("Referencias Picking");//creo una hoja moficada
                if (ws is null)
                    return;
                //Inicializo las consultas SQL
                if (!Business.iniReaderDatosUbicacion("REFERENCIAPICKING"))
                {
                    MessageBox.Show(Lenguaje.traduce("Error recuperando datos de ubicaciones"));
                    return;
                }
                //Calcular el rango de celdas usadas.
                CellRange usedCellRange = ws.UsedCellRange;
                //Como es un proceso lento muestro una barra de progreso
                ActivarProgressBar(0, (usedCellRange.ToIndex.RowIndex - usedCellRange.FromIndex.RowIndex + 1) * (usedCellRange.ToIndex.ColumnIndex - usedCellRange.FromIndex.ColumnIndex + 1) + 1);
                for (int rowIndex = usedCellRange.FromIndex.RowIndex; rowIndex <= usedCellRange.ToIndex.RowIndex; rowIndex++)
                {
                    for (int columnIndex = usedCellRange.FromIndex.ColumnIndex; columnIndex <= usedCellRange.ToIndex.ColumnIndex; columnIndex++)
                    {
                        CellSelection selection = ws.Cells[rowIndex, columnIndex];
                        AjustarCeldaRefPicking(selection);
                        ProgressBarInc();
                    }
                }
                //cierro conexion y progressbar
                desactivarProgressBar();
                Business.closeDatosUbicacion();
                /*wsLlenado.Name = ocupacionTraducido;
                libro.ActiveWorksheet = libro.Worksheets[ocupacionTraducido];
                wsLlenado.Name = ocupacionTraducido;*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR calculando llenado del mapa de almacén:\n" + ex.Message);
            }
        }
        private void CalculoInfoCelda()
        {
            try
            {
                //Inicializo las consultas SQL REUTILIZO LA DE UBICACIONES
                if (!Business.iniReaderDatosUbicacion("LLENADOPORUBICACIONES"))
                {
                    MessageBox.Show(Lenguaje.traduce("Error recuperando datos de ubicaciones"));
                    return;
                }
                //Cojo la celda actual
                CellIndex indiceCelda=  radSSheetHojaCalculo.ActiveWorksheetEditor.Selection.ActiveCellIndex;
                MostrarInfoCelda(indiceCelda);
                //cierro conexion 
                Business.closeDatosUbicacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("ERROR calculando info ubicacion:")+"\n" + ex.Message);
            }
        }
        void ActivarProgressBar(int minimo, int maximo)
        {
            radProgressBar1.Minimum = minimo;
            radProgressBar1.Value1 = 0;
            radProgressBar1.Maximum = maximo;
            radProgressBar1.Visible = true;
            radProgressBar1.Refresh();
            Application.DoEvents();
        }
        void ProgressBarInc(int value, string text)
        {
            radProgressBar1.Value1 = value;
            radProgressBar1.Text = text;
            Application.DoEvents();
        }
        void desactivarProgressBar()
        {
            radProgressBar1.Visible = false;
            Application.DoEvents();
        }
        void ProgressBarInc()
        {
            radProgressBar1.Value1++;
            radProgressBar1.Text = Convert.ToString(radProgressBar1.Value1);
            //Application.DoEvents();//ojo se come 300 milis Pablo
        }

        private Worksheet duplicarHoja(string nombreHoja)
        {
            try
            {
                int i = 1;
                libroModificado = true; //por si quieren guardar
                Workbook libro = radSSheetHojaCalculo.Workbook;
                Worksheet wsNueva = libro.Worksheets.Add();//creo una hoja moficada
                wsNueva.CopyFrom(libro.ActiveWorksheet);//copiar pegar la hoja actual

                while (libro.Worksheets.Contains(nombreHoja))
                {
                    nombreHoja = nombreHoja + "_" + Convert.ToString(i++);
                    
                }

                wsNueva.Name = Lenguaje.traduce(nombreHoja);
                formato = wsNueva.Cells[5, 5].GetFormat().Value;//almaceno un formato para usarlo //TODO mejorar.
                Application.DoEvents();
                return wsNueva;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje.traduce("ERROR duplicando hoja de Cálculo:\n") + ex.Message);
                return null;
            }

        }

        private void CargarEscalaColorLlenado(Worksheet hoja, double[]escalaValores, IFill[]color )
        {
            try
            {
                for(int i=0; i<maxEscalaColoresLlenado; i++)
                {
                    ICellValue cellValue = hoja.Cells[i, 0].GetValue().Value;
                    //CellValueFormat format = hoja.Cells[i, 0].GetFormat().Value;
                    string s = cellValue.RawValue;
                    escalaValores[i]= Convert.ToDouble( s);
                    color[i] = hoja.Cells[i, 0].GetFill().Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR convirtiendo valores de la paleta de colores:\n" + ex.Message);
            }
        }
        private void MostrarInfoCelda(CellIndex IndiceCelda)
        {
            String ubicacion = null;
            double ocupacion;
            String infoCelda = "";
            try
            {
                ICellValue cellValue = radSSheetHojaCalculo.ActiveWorksheet.Cells[IndiceCelda].GetValue().Value;
                ubicacion = Convert.ToString(cellValue.RawValue);
                //ubicacion = Convert.ToString(wsLlenado.Cells[rowIndex, columnIndex].GetValue().Value);
                if (ubicacion.StartsWith("W"))
                {
                    //buscar datos de la ubicacion
                    SqlDataReader lectorDatos = Business.getReaderDatosUbicacion(ubicacion);
                    if (lectorDatos.HasRows)
                    {

                        Boolean primeraVez = true;
                        int existencias = 0;
                        int cantidadExistencia = 0;
                        int capacidadExistenciasUbicacion = 0;
                        int cantidadPaletArticulo = 1;
                        while (lectorDatos.Read())
                        {
                            if (primeraVez)
                            {
                                //TODO traducir tiras internas.
                                if (lectorDatos[0] is DBNull)
                                {
                                    existencias = 0;
                                    primeraVez = false;
                                    capacidadExistenciasUbicacion = lectorDatos.GetInt32(7);

                                    infoCelda = Lenguaje.traduce("Ubicación") + ":" + lectorDatos.GetString(6) + "\n";
                                    infoCelda = infoCelda + Lenguaje.traduce("Estado") + ":" + lectorDatos.GetString(9) + "\n";
                                    infoCelda = infoCelda + Lenguaje.traduce("Capacidad existencias") + ":" + Convert.ToString(capacidadExistenciasUbicacion) + "\n";
                                    infoCelda = infoCelda + Lenguaje.traduce("Capacidad artículos") + ":" + Convert.ToString(lectorDatos.GetInt32(8) + "\n");
                                    infoCelda = infoCelda + Lenguaje.traduce("Ubicación vacia");
                                }
                                else
                                {
                                    existencias++;
                                    primeraVez = false;
                                    cantidadExistencia = lectorDatos.GetInt32(0);
                                    capacidadExistenciasUbicacion = lectorDatos.GetInt32(7);
                                    cantidadPaletArticulo = lectorDatos.GetInt32(12);
                                    infoCelda = Lenguaje.traduce("Ubicación") + ":" + lectorDatos.GetString(6) + "\n";
                                    infoCelda = infoCelda + Lenguaje.traduce("Estado") + ":" + lectorDatos.GetString(9) + "\n";
                                    infoCelda = infoCelda + Lenguaje.traduce("Capacidad existencias") + ":" + Convert.ToString(capacidadExistenciasUbicacion) + "\n";
                                    infoCelda = infoCelda + Lenguaje.traduce("Capacidad artículos") + ":" + Convert.ToString(lectorDatos.GetInt32(8) + "\n");
                                }

                            }
                            if (!(lectorDatos[0] is DBNull))
                            {
                                infoCelda = infoCelda + "**" + Lenguaje.traduce("Articulo") + ":" + lectorDatos.GetString(10) + " " + lectorDatos.GetString(11) + "\n" +
                                Lenguaje.traduce("Matricula") + ": " + lectorDatos.GetInt32(14)+ "   SSCC:" + lectorDatos.GetString(15) +"\n"+
                                Lenguaje.traduce("Cantidad") + ":" + Convert.ToString(cantidadExistencia) +
                                Lenguaje.traduce(" en Palets de ") + Convert.ToString(cantidadPaletArticulo)+"\n"+
                                Lenguaje.traduce("LOTE") + ":" + lectorDatos.GetString(13);

                            }
                            if (existencias == 1)//si es monoubicacion la ocupacion es la del las unidades del palet si no de la capacidad existencias
                            {
                                if (cantidadPaletArticulo < cantidadExistencia || (cantidadPaletArticulo <= 0))
                                    cantidadPaletArticulo = cantidadExistencia;//la cantidad del palet aqui no puede ser menor que el contenido. 
                                ocupacion = cantidadExistencia / cantidadPaletArticulo;
                            }
                            else
                                ocupacion = existencias / capacidadExistenciasUbicacion;
                        }
                    }
                    else
                    {
                        infoCelda = Lenguaje.traduce("Ubicacion no encontrada" + ": " + ubicacion);
                    }
                    RumMessageBox message = new RumMessageBox(infoCelda);
                    message.Show();
                    //RadMessageBox.Show(infoCelda,"informacion de la celda",MessageBoxButtons.OK);
                    lectorDatos.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR mostrando informacion de celda  " + ubicacion + ":\n" + ex.Message);
            }


        }
        private void AjustarFrecuenciaAcceso(DataTable dt ,CellSelection celdaSeleccionada)
        {
            String ubicacion = null;
            double ocupacion;
            try
            {
                ICellValue cellValue = celdaSeleccionada.GetValue().Value;
                ubicacion = cellValue.GetResultValueAsString(formato);
                //ubicacion = Convert.ToString(wsLlenado.Cells[rowIndex, columnIndex].GetValue().Value);
                if (ubicacion.StartsWith("W"))
                {
                    //buscar datos de la ubicacion

                    SqlDataReader lectorDatos = Business.getReaderDatosUbicacion(ubicacion);
                    if (lectorDatos.HasRows)
                    {
                        String infoCelda = "";
                        while (lectorDatos.Read())
                        {
                                infoCelda = Convert.ToString(lectorDatos.GetInt32(1));
                        }
                        celdaSeleccionada.SetValue(infoCelda);
                    }
                    lectorDatos.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR ajustando celda del mapa de almacén en la celda " + ubicacion + ":\n" + ex.Message);
            }
        }

        private void AjustarCeldaUbicacion(CellSelection celdaSeleccionada, double[] escalaValores, IFill[] color)
        {
            String ubicacion = null;
            double ocupacion;
            try
            {
                ICellValue cellValue = celdaSeleccionada.GetValue().Value;
                ubicacion = cellValue.GetResultValueAsString(formato);
                //ubicacion = Convert.ToString(wsLlenado.Cells[rowIndex, columnIndex].GetValue().Value);
                if (ubicacion.StartsWith("W"))
                {
                    //buscar datos de la ubicacion
                    SqlDataReader lectorDatos = Business.getReaderDatosUbicacion(ubicacion);
                    if (lectorDatos.HasRows)
                    {
                        String infoCelda = "";
                        Boolean primeraVez = true;
                        int existencias = 0;
                        int cantidadExistencia = 0;
                        int capacidadExistenciasUbicacion = 0;
                        int cantidadPaletArticulo = 1;
                        while (lectorDatos.Read())
                        {
                            if (primeraVez)
                            {
                                //TODO traducir tiras internas.
                                existencias++;
                                primeraVez = false;
                                cantidadExistencia= lectorDatos.GetInt32(0);
                                capacidadExistenciasUbicacion = lectorDatos.GetInt32(7);
                                cantidadPaletArticulo= lectorDatos.GetInt32(12);
                                infoCelda = "UBICACION:" + lectorDatos.GetString(6);
                                infoCelda = $"{infoCelda} Estado:" + lectorDatos.GetString(9);
                                infoCelda = $"{infoCelda} \n Capacidad existencias:" + Convert.ToString(capacidadExistenciasUbicacion);
                                infoCelda = $"{infoCelda} \n Capacidad articulos:" + Convert.ToString(lectorDatos.GetInt32(8));
                            }
                            infoCelda = $"{infoCelda} \n** Articulo:" +lectorDatos.GetString(10) + " " + lectorDatos.GetString(11)+
                                "\n Cantidad " + Convert.ToString(cantidadExistencia)+ 
                                " en  Palets de " + Convert.ToString(cantidadPaletArticulo);
                            infoCelda = Lenguaje.traduce(infoCelda);
                        }
                        if (existencias == 1)//si es monoubicacion la ocupacion es la del las unidades del palet si no de la capacidad existencias
                        {
                            if (cantidadPaletArticulo < cantidadExistencia || (cantidadPaletArticulo <= 0))
                                cantidadPaletArticulo = cantidadExistencia;//la cantidad del palet aqui no puede ser menor que el contenido. 
                            ocupacion = cantidadExistencia / cantidadPaletArticulo;
                        }
                        else
                            ocupacion = existencias / capacidadExistenciasUbicacion;
                        int i = 0;
                        while ( i < maxEscalaColoresLlenado && escalaValores[i] < ocupacion  )
                        {
                            i++;
                        }
                        celdaSeleccionada.SetFill(color[i]);
                    }
                    lectorDatos.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR ajustando celda del mapa de almacén en la celda "+ ubicacion+ ":\n" + ex.Message);
            }
            

        }
        private void AjustarCeldaRefPicking(CellSelection celdaSeleccionada )
        {
            String ubicacion = null;
            try
            {
                ICellValue cellValue = celdaSeleccionada.GetValue().Value;
                ubicacion = cellValue.GetResultValueAsString(formato);
                if (ubicacion.StartsWith("W"))
                {
                    //buscar datos de la ubicacion
                    SqlDataReader lectorDatos = Business.getReaderDatosUbicacion(ubicacion);
                    if (lectorDatos.HasRows)
                    {
                        String infoCelda = "";
                        Boolean primeraVez = true;
                        while (lectorDatos.Read())
                        {
                            if (primeraVez)
                                primeraVez = false;
                            else
                                infoCelda = $"{infoCelda} \n--------------";

                            infoCelda = $"{infoCelda}" + lectorDatos.GetString(0) + " " +
                                lectorDatos.GetString(1);
                            infoCelda = Lenguaje.traduce(infoCelda);
                        }
                        celdaSeleccionada.SetValue(infoCelda);
                        PatternFill colorPicking = new PatternFill(PatternType.Solid, System.Windows.Media.Color.FromArgb(255, 0, 0, 0), System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
                        celdaSeleccionada.SetFill(colorPicking);
                    }
                    lectorDatos.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR ajustando celda del mapa de almacén en la celda " + ubicacion + ":\n" + ex.Message);
            }


        }

        private void CalculoLlenado()//OBSOLETO
        {
            double llenado;
            string ubicacion;

            try
            {
                libroModificado = true; //por si quieren guardar
                //cojo hoja alzado que tiene las ubicaciones y la duplico en otra hoja (worksheet) del libro(workbook) 
                Workbook libro = radSSheetHojaCalculo.Workbook;
                //creo una hoja moficada
                Worksheet wsLlenado = libro.Worksheets.Add();

                //copiar pegar la hoja actual
                wsLlenado.CopyFrom(libro.ActiveWorksheet);
                // VA wsLlenado.CopyFrom(libro.Sheets["Alzado"] as Worksheet);
                wsLlenado.Name = "Ocupación";

                //Cargar datos y sustituir
                ReplaceOptions options = new ReplaceOptions()
                {
                    //StartCell = new WorksheetCellIndex(wsLlenado, 0, 7),
                    //FindBy = FindBy.Columns,
                    //FindWithin = FindWithin.Workbook,
                    MatchCase = false,
                    MatchEntireCellContents = false,
                    FindIn = FindInContentType.Values,
                    SearchRanges = null,
                    FindWhat = "W1A01P001S0",
                    ReplaceWith = "0,5",
                };
                FindResult findResult;
                int ubicacionesEncontradas = 0;
                int ubicacionesCambiadas = 0;
                int ubicacionesTotales = 0;
                DataTable dtLlenadoMapa = Business.getLlenadoMapa();
                ubicacionesTotales = dtLlenadoMapa.Rows.Count;
                foreach (DataRow rowLlenado in dtLlenadoMapa.Rows)
                {
                    ubicacionesEncontradas++;
                    ubicacion = Convert.ToString(rowLlenado["ubicacion"]).Trim(charsToTrim);
                    llenado = Convert.ToDouble(rowLlenado["cantidad"]) / Convert.ToDouble(rowLlenado["cantidadPalet"]);
                    options.FindWhat = ubicacion;
                    options.ReplaceWith = Convert.ToString(llenado);
                    findResult = wsLlenado.Find(options);
                    //se podria optimizar con
                    //options.StartCell = findResult.FoundCell; 
                    if (wsLlenado.Replace(options))
                    {
                        ubicacionesCambiadas++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR calculando llenado del mapa de almacén:\n" + ex.Message);
            }
        }
        private void GuardarLibro(Workbook libro)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv.CsvFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv.CsvFormatProvider();
                saveFileDialog.Filter = /*"CSV (comma delimited) (.csv)|.csv|All Files (.)|."*/"Excel |*.xlsx|All Files (.)|.";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream output = saveFileDialog.OpenFile())
                    {
                        formatProvider.Export(libro, output);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Error:" + ex.Message);
            }

        }

        private void btnUbicacion_Click(object sender, EventArgs e)
        {
            FrmUbicaciones ubicaciones = new FrmUbicaciones();
            ubicaciones.Show();
        }
    }
}

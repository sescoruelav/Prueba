using Microsoft.CodeAnalysis.CSharp.Scripting;
using Rumbo.Core.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Timers;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Documents;
using RumboSGA.Controles;
using RumboSGAManager.Model;

namespace RumboSGA.Presentation.Herramientas
{
    internal class TorreControl
    {
        public TorreControl(ref DBTableLayoutPanel panel)
        {
            //panelCT.SuspendLayout();
            panelCT = panel;
            panelCT.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;// no se si me gusta.
            if (panelCT.Controls.Count != 0) //Evita que si vuelves a entrar a TorreControl estando TorreControl ya cargado vuelva a cargar los mismos controles dos veces
                panelCT.Controls.Clear();
            controles = CargarConfCtrlTwr();
            InstanciarCtrlTwr();
            inicializarRelojCtrlTwr();
            MostrarCtrlTwr(controles);

            //panelCT.ResumeLayout();
        }

        public struct SRegla
        {
            public string evaluar;
            public string nombreIco;
            public string color;
        };

        public struct SqlSeries
        {
            public string nombreSerie;
            public string sql;
            public DataTable oldDt;
        }

        public struct ControlTowerCtrl
        {
            public string nombre;
            public string tipoGrafica;
            public int tab;
            public int x;
            public int y;
            public int lonX;
            public int lonY;
            public List<SqlSeries> sqlSeries;
            public Control objGrafico;
            public ChartSeriesType tipoSerie;
            public bool activado;
            public List<SRegla> reglas;
        };

        private static System.Timers.Timer tik;
        private double tikMillis = 5 * 60 * 1000;
        private string path = Persistencia.DirectorioBase;
        private static TableLayoutPanel panelCT;
        private List<ControlTowerCtrl> controles;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void MostrarCtrlTwr()
        {
            MostrarCtrlTwr(controles);
        }

        private void MostrarCtrlTwr(List<ControlTowerCtrl> ctrls)
        {
            try
            {
                for (int ele = 0; ele < ctrls.Count; ele++)
                {
                    ControlTowerCtrl ctrl = (ControlTowerCtrl)ctrls[ele];
                    //panelCT.SetColumnSpan(ctrl.objGrafico, ctrl.lonY);
                    //panelCT.SetRowSpan(ctrl.objGrafico, ctrl.lonX);
                    ActualizaCtrl(ctrl, ele);
                }
                //panelCT.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("error mostrando control" + "\n" + e.Message));
            }
        }

        #region ActualizaControles

        private RadChartView ActualizaPie(ControlTowerCtrl ctrl, int ele)
        {
            //devuelve un null si no ha cambiado y un nuevo objeto grafico si ha cambiado
            bool haCambiado = false;
            DataTable newDt;
            try
            {   //comprobar si el grafico ha cambiado
                //TODO serie.oldDT se pierde al salir de la funcion, solucinar para mejorar el aspecto.
                for (int nSerie = 0; nSerie < ctrl.sqlSeries.Count; nSerie++)
                {
                    SqlSeries serie = (SqlSeries)ctrl.sqlSeries[nSerie];
                    newDt = ConexionSQL.getDataTable(serie.sql);
                    if (!ConexionSQL.DataTablesEquals(newDt, serie.oldDt))
                    {
                        serie.oldDt = newDt;
                        haCambiado = true;
                        ctrl.sqlSeries[0] = serie;
                        controles[ele] = ctrl;
                    }
                }
                if (!haCambiado)
                {
                    return null;
                }
                //hay que actualizar
                ctrl.objGrafico.Dispose();
                RadChartView pie = new RadChartView();
                ctrl.objGrafico = pie;
                pie.AreaType = ChartAreaType.Pie;
                pie.LegendTitle = ctrl.nombre;
                PieSeries pieSeries = new PieSeries();
                PieDataPoint pieDataPoint1;
                foreach (SqlSeries serie in ctrl.sqlSeries)
                {
                    DataTable dt = ConexionSQL.getDataTable(serie.sql);
                    foreach (DataRow row in dt.Rows)
                    {
                        pieDataPoint1 = new PieDataPoint();
                        pieDataPoint1.Label = row[0];
                        pieDataPoint1.LegendTitle = Convert.ToString(row[0]);
                        pieDataPoint1.OffsetFromCenter = 0D;
                        pieDataPoint1.RadiusAspectRatio = 1F;
                        pieDataPoint1.Value = Convert.ToDouble(row[1]);
                        pieSeries.DataPoints.Add(pieDataPoint1);
                    }
                }
                pie.Series.Add(pieSeries);
                pie.ShowGrid = false;
                pie.ShowLegend = false;
                pie.ShowTitle = true;
                pie.Size = new System.Drawing.Size(373, 181);
                pie.TabIndex = 0;
                pie.Title = ctrl.nombre;
                pie.Dock = DockStyle.Fill;

                pieSeries.ShowLabels = true;
                pieSeries.DrawLinesToLabels = true;
                pieSeries.SyncLinesToLabelsColor = true;
                pie.ShowSmartLabels = true;
                //controles[ele] = pie;//TODO Volver a poner

                ctrl.objGrafico = pie;
                controles[ele] = ctrl;
                panelCT.Controls.Add(pie, ctrl.x, ctrl.y);
                panelCT.SetColumnSpan(pie, ctrl.lonX);
                panelCT.SetRowSpan(pie, ctrl.lonY);
                ctrl.objGrafico.Visible = true;
                pie.Refresh();
                return pie;
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("error actualizando gráfica") + "\n" + ctrl.nombre + e.Message);
            }
            return null;
        }

        private RadChartView ActualizaBars(ref ControlTowerCtrl ctrl, int ele)
        {
            //devuelve un null si no ha cambiado y un nuevo objeto grafico si ha cambiado
            bool haCambiado = false;
            DataTable newDt;
            try
            {
                //comprobar si el grafico ha cambiado
                //TODO serie.oldDT se pierde al salir de la funcion, solucinar para mejorar el aspecto.
                for (int nSerie = 0; nSerie < ctrl.sqlSeries.Count && !haCambiado; nSerie++)
                {
                    SqlSeries serie = (SqlSeries)ctrl.sqlSeries[nSerie];
                    newDt = ConexionSQL.getDataTable(serie.sql);
                    if (!ConexionSQL.DataTablesEquals(newDt, serie.oldDt))
                    {
                        serie.oldDt = newDt;
                        haCambiado = true;
                    }
                }
                if (!haCambiado)
                {
                    return null;
                }
                //hay que actualizar
                ctrl.objGrafico.Dispose();
                CartesianArea cartesianArea = new CartesianArea();
                RadChartView gr = new RadChartView();
                gr.AreaDesign = cartesianArea;
                gr.Location = new System.Drawing.Point(443, 57);
                gr.Name = "radChartView1";
                gr.ShowGrid = false;
                gr.Size = new System.Drawing.Size(222, 296);
                gr.TabIndex = 1;

                for (int nSerie = 0; nSerie < ctrl.sqlSeries.Count; nSerie++)
                {
                    SqlSeries serie = (SqlSeries)ctrl.sqlSeries[nSerie];
                    newDt = ConexionSQL.getDataTable(serie.sql);
                    SqlSeries newSerie = new SqlSeries();
                    newSerie.sql = serie.sql;
                    newSerie.oldDt = newDt;
                    ctrl.sqlSeries[nSerie] = newSerie;

                    BarSeries barSeries = new BarSeries();
                    barSeries.DataSource = newDt;
                    barSeries.LegendTitle = serie.nombreSerie;
                    barSeries.ShowLabels = true;
                    barSeries.CategoryMember = newDt.Columns[0].ColumnName;
                    barSeries.ValueMember = newDt.Columns[1].ColumnName;
                    gr.Series.Add(barSeries);
                    barSeries.CombineMode = Telerik.Charting.ChartSeriesCombineMode.Stack;
                }
                controles[ele] = ctrl;
                gr.ShowGrid = false;
                gr.ShowLegend = true;
                gr.ShowTitle = true;
                gr.Title = ctrl.nombre;
                gr.Dock = DockStyle.Fill;

                gr.Axes[0].LabelFitMode = AxisLabelFitMode.MultiLine;
                gr.Axes[1].LabelFitMode = AxisLabelFitMode.MultiLine;

                ctrl.objGrafico = gr;
                controles[ele] = ctrl;
                panelCT.Controls.Add(gr, ctrl.x, ctrl.y);
                panelCT.SetColumnSpan(gr, ctrl.lonX);
                panelCT.SetRowSpan(gr, ctrl.lonY);
                ctrl.objGrafico.Visible = true;
                gr.Refresh();
                return gr;
            }
            catch (Exception e)
            {
                MessageBox.Show("error en datos grafica " + ctrl.nombre + "\n" + e.Message);
            }
            return null;
        }

        private RadChartView ActualizaStackedBars(ref ControlTowerCtrl ctrl, int ele)
        {
            //devuelve un null si no ha cambiado y un nuevo objeto grafico si ha cambiado
            bool haCambiado = false;
            DataTable newDt;
            try
            {
                //comprobar si el grafico ha cambiado
                //TODO serie.oldDT se pierde al salir de la funcion, solucinar para mejorar el aspecto.
                for (int nSerie = 0; nSerie < ctrl.sqlSeries.Count && !haCambiado; nSerie++)
                {
                    SqlSeries serie = (SqlSeries)ctrl.sqlSeries[nSerie];
                    newDt = ConexionSQL.getDataTable(serie.sql);
                    if (!ConexionSQL.DataTablesEquals(newDt, serie.oldDt))
                    {
                        serie.oldDt = newDt;
                        haCambiado = true;
                    }
                }
                if (!haCambiado)
                {
                    return null;
                }
                //hay que actualizar
                ctrl.objGrafico.Dispose();
                CartesianArea cartesianArea = new CartesianArea();
                RadChartView gr = new RadChartView();
                gr.AreaDesign = cartesianArea;
                gr.Location = new System.Drawing.Point(443, 57);
                gr.Name = "radChartView1";
                gr.ShowGrid = false;
                gr.Size = new System.Drawing.Size(222, 296);
                gr.TabIndex = 1;

                for (int nSerie = 0; nSerie < ctrl.sqlSeries.Count; nSerie++)
                {
                    SqlSeries serie = (SqlSeries)ctrl.sqlSeries[nSerie];
                    newDt = ConexionSQL.getDataTable(serie.sql);
                    SqlSeries newSerie = new SqlSeries();
                    newSerie.sql = serie.sql;
                    newSerie.oldDt = newDt;
                    ctrl.sqlSeries[nSerie] = newSerie;

                    BarSeries barSeries = new BarSeries();
                    barSeries.DataSource = newDt;
                    barSeries.LegendTitle = serie.nombreSerie;
                    barSeries.ShowLabels = true;
                    barSeries.CategoryMember = newDt.Columns[0].ColumnName;
                    barSeries.ValueMember = newDt.Columns[1].ColumnName;
                    gr.Series.Add(barSeries);
                    barSeries.CombineMode = Telerik.Charting.ChartSeriesCombineMode.Stack;
                }
                controles[ele] = ctrl;
                gr.ShowGrid = false;
                gr.ShowLegend = true;
                gr.ShowTitle = true;
                gr.Title = ctrl.nombre;
                gr.Dock = DockStyle.Fill;

                gr.Axes[0].LabelFitMode = AxisLabelFitMode.MultiLine;
                gr.Axes[1].LabelFitMode = AxisLabelFitMode.MultiLine;

                ctrl.objGrafico = gr;
                controles[ele] = ctrl;
                panelCT.Controls.Add(gr, ctrl.x, ctrl.y);
                panelCT.SetColumnSpan(gr, ctrl.lonX);
                panelCT.SetRowSpan(gr, ctrl.lonY);
                ctrl.objGrafico.Visible = true;
                gr.Refresh();
                return gr;
            }
            catch (Exception e)
            {
                MessageBox.Show("error en datos grafica " + ctrl.nombre + "\n" + e.Message);
            }
            return null;
        }

        private void ActualizaTabla(ControlTowerCtrl ctrl, int ele)
        {
            //devuelve un null si no ha cambiado y un nuevo objeto grafico si ha cambiado
            DataTable newDt;
            RumGridView gridWarn = null;
            SqlSeries s = ctrl.sqlSeries[0];
            try
            {
                //comprobar si el grafico ha cambiado
                newDt = Business.GetWarnings(s.sql, 0); //idusuario
                if (!ConexionSQL.DataTablesEquals(newDt, ctrl.sqlSeries[0].oldDt))
                {
                    //Hay que actualizar
                    gridWarn.Dispose();//lo quito para que no lo duplique
                    gridWarn = new RumGridView();
                    ctrl.objGrafico = gridWarn;

                    s.oldDt = newDt;
                    ctrl.sqlSeries[0] = s;
                    gridWarn = (RumGridView)ctrl.objGrafico;
                    gridWarn.DataSource = newDt;//idusuario
                    controles[ele] = ctrl;
                    panelCT.Controls.Add(gridWarn, ctrl.x, ctrl.y);
                    panelCT.SetColumnSpan(gridWarn, ctrl.lonX);
                    panelCT.SetRowSpan(gridWarn, ctrl.lonY);
                    ctrl.objGrafico.Visible = true;
                    gridWarn.Refresh();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("error actualizando tabla" + "\n" + e.Message));
            }
        }

        private void ActualizaTablaWarnings(ControlTowerCtrl ctrl, int ele)
        {
            //devuelve un null si no ha cambiado y un nuevo objeto grafico si ha cambiado
            DataTable newDt;
            RumGridView gridWarn = null;
            SqlSeries s = ctrl.sqlSeries[0];
            try
            {
                //comprobar si el grafico ha cambiado
                newDt = Business.GetWarnings(s.sql, 0); //idusuario
                if (!ConexionSQL.DataTablesEquals(newDt, ctrl.sqlSeries[0].oldDt))
                {
                    //Hay que actualizar
                    gridWarn.Dispose();//lo quito para que no lo duplique
                    gridWarn = new RumGridView();
                    ctrl.objGrafico = gridWarn;

                    s.oldDt = newDt;
                    ctrl.sqlSeries[0] = s;
                    gridWarn = (RumGridView)ctrl.objGrafico;
                    gridWarn.DataSource = newDt;//idusuario
                    controles[ele] = ctrl;
                    panelCT.Controls.Add(gridWarn, ctrl.x, ctrl.y);
                    panelCT.SetColumnSpan(gridWarn, ctrl.lonX);
                    panelCT.SetRowSpan(gridWarn, ctrl.lonY);
                    ctrl.objGrafico.Visible = true;
                    gridWarn.Refresh();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("error actualizando warnings" + "\n" + e.Message));
            }
        }

        private async Task ActualizaContadorAsync(ControlTowerCtrl ctrl, int ele)
        {
            TableLayoutPanel gr;
            RadTextBox txt;
            PictureBox ico;
            bool haCambiado = false;
            string valor = "-";
            try
            {
                gr = (TableLayoutPanel)ctrl.objGrafico;
                txt = (RadTextBox)gr.Controls["Numero"];
                ico = (PictureBox)gr.Controls["ico"];

                //buscar el valor
                SqlSeries serie0 = (SqlSeries)ctrl.sqlSeries[0];
                DataTable dt = ConexionSQL.getDataTable(serie0.sql);
                if (dt is null)
                {
                    txt.Text = "-";
                    return;
                }
                else
                {
                    foreach (DataRow rValor in dt.Rows)
                    {
                        valor = Convert.ToString(rValor[0]);
                        if (!txt.Text.Equals(valor))
                        {
                            txt.Text = valor;
                            txt.Refresh();
                            haCambiado = true;
                        }
                    }
                }
                if (!haCambiado)
                    return;
                //Actualizar colores e iconos
                bool seCumple = false;
                for (int nregla = 0; nregla < ctrl.reglas.Count && !seCumple; nregla++)
                {
                    SRegla sregla = (SRegla)ctrl.reglas[nregla];
                    string evaluar = sregla.evaluar.Replace("@valor", valor);
                    try
                    {
                        seCumple = await CSharpScript.EvaluateAsync<bool>(evaluar);
                        if (seCumple)
                        {
                            txt.ForeColor = BuscaColor(sregla.color);
                            if (sregla.nombreIco.Contains(".svg"))
                            {
                                //ico.SvgImage = RadSvgImage.FromFile(path + @"\icons\" + sregla.nombreIco);
                            }
                            else
                            {
                                if (File.Exists(Persistencia.DirectorioGlobal + @"\icons\" + sregla.nombreIco))
                                    ico.Image = Image.FromFile(Persistencia.DirectorioGlobal + @"\icons\" + sregla.nombreIco);
                                else
                                    log.Error("No se ha encontrado el icono: " + Persistencia.DirectorioGlobal + @"\icons\" + sregla.nombreIco);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message + "\n");//TODO mejorar tratamiento de errores
                    }
                }
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("error actualizando contador " + ctrl.nombre + "\n" + e.Message);
            }
        }

        private void ActualizaCtrl(ControlTowerCtrl ctrl, int ele)
        {
            try
            {
                switch (ctrl.tipoGrafica)
                {
                    case "contador":
#pragma warning disable CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                        ActualizaContadorAsync(ctrl, ele);
#pragma warning restore CS4014 // Como esta llamada no es 'awaited', la ejecución del método actual continuará antes de que se complete la llamada. Puede aplicar el operador 'await' al resultado de la llamada.
                        break;

                    case "pie":
                        ActualizaPie(ctrl, ele);
                        break;

                    case "stackedBars":
                        ActualizaStackedBars(ref ctrl, ele);
                        break;

                    case "Bars":
                        ActualizaBars(ref ctrl, ele);
                        break;

                    case "warnings":
                        ActualizaTablaWarnings(ctrl, ele);
                        break;

                    case "tabla":
                        ActualizaTabla(ctrl, ele);
                        break;

                    default:
                        ctrl.activado = false;
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("error asignando valores en torre de control " + "\n" + ctrl.nombre + "\n" + e.Message));
            }
        }

        #endregion ActualizaControles

        #region instanciaControles

        private void InstanciarCtrlTwr()//*********************************************
        {
            try
            {
                //panelCT.Visible = false;
                for (int ele = 0; ele < controles.Count; ele++)
                {
                    ControlTowerCtrl ctrl = (ControlTowerCtrl)controles[ele];
                    switch (ctrl.tipoGrafica.ToLower())
                    {
                        case "contador":
                            TableLayoutPanel obj = instanciarContador(ctrl);
                            ctrl.objGrafico = obj;
                            controles[ele] = ctrl;
                            panelCT.Controls.Add(obj, ctrl.x, ctrl.y);
                            ctrl.objGrafico.Visible = true;
                            break;

                        case "pie":
                            RadChartView pie = instanciarControlPie(ctrl);
                            ctrl.objGrafico = pie;
                            controles[ele] = ctrl;
                            panelCT.Controls.Add(pie, ctrl.x, ctrl.y);
                            panelCT.SetColumnSpan(pie, ctrl.lonX);
                            panelCT.SetRowSpan(pie, ctrl.lonY);
                            ctrl.objGrafico.Visible = true;
                            break;

                        case "grid":
                            break;

                        case "linearGauge":
                            break;

                        case "gauge":
                            break;

                        case "stackedbars":
                        case "bars":
                            RadChartView gr = InstanciarControlStackedBar(ctrl);
                            ctrl.objGrafico = gr;
                            controles[ele] = ctrl;
                            panelCT.Controls.Add(gr, ctrl.x, ctrl.y);
                            panelCT.SetColumnSpan(gr, ctrl.lonX);
                            panelCT.SetRowSpan(gr, ctrl.lonY);
                            ctrl.objGrafico.Visible = true;
                            break;

                        case "warnings":
                            RumGridView warn = instanciarWarning(ctrl);
                            ctrl.objGrafico = warn;
                            controles[ele] = ctrl;
                            panelCT.Controls.Add(warn, ctrl.x, ctrl.y);
                            panelCT.SetColumnSpan(warn, ctrl.lonX);
                            panelCT.SetRowSpan(warn, ctrl.lonY);
                            warn.Dock = DockStyle.Fill;
                            ctrl.objGrafico.Visible = true;
                            break;

                        case "tabla":
                            RumGridView tabla = instanciarTabla(ctrl);
                            ctrl.objGrafico = tabla;
                            controles[ele] = ctrl;
                            panelCT.Controls.Add(tabla, ctrl.x, ctrl.y);
                            panelCT.SetColumnSpan(tabla, ctrl.lonX);
                            panelCT.SetRowSpan(tabla, ctrl.lonY);
                            tabla.Dock = DockStyle.Fill;
                            ctrl.objGrafico.Visible = true;
                            break;

                        case "timer":
                            AjustaTimer(ctrl);
                            break;

                        default:
                            MessageBox.Show(Lenguaje.traduce("Tipo de gráfica desconocida") + ":" + ctrl.tipoGrafica + "\n en control: " + ctrl.nombre);
                            break;
                    }
                }
                //panelCT.Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Test" + e.Message);
            }
            finally
            {
                panelCT.Visible = true;
            }
        }

        private RumGridView instanciarTabla(ControlTowerCtrl ctrl)
        {
            DataTable newDt;
            try
            {
                RumGridView gridWarn = new RumGridView();
                gridWarn.Name = "tabla";
                gridWarn.ReadOnly = true;
                SqlSeries s = ctrl.sqlSeries[0];
                gridWarn.Dock = System.Windows.Forms.DockStyle.Fill;
                newDt = Business.GetWarnings(s.sql, 0);//idusuario
                gridWarn.DataSource = newDt;
                s.oldDt = newDt;
                ctrl.sqlSeries[0] = s;
                ctrl.objGrafico = gridWarn;
                gridWarn.Location = new System.Drawing.Point(ctrl.x, ctrl.y);
                gridWarn.BestFitColumns();
                gridWarn.Dock = DockStyle.Fill;
                gridWarn.Refresh();

                return gridWarn;
            }
            catch (Exception e)
            {
                MessageBox.Show("error Warnings \n" + e.Message);
            }
            return null;
        }

        private RumGridView instanciarWarning(ControlTowerCtrl ctrl)
        {
            DataTable newDt;
            try
            {
                RumGridView gridWarn = new RumGridView();
                gridWarn.Name = "Warnings";
                gridWarn.ReadOnly = true;
                SqlSeries s = ctrl.sqlSeries[0];
                gridWarn.Dock = System.Windows.Forms.DockStyle.Fill;
                newDt = Business.GetWarnings(s.sql, 0);//idusuario
                gridWarn.DataSource = newDt;
                s.oldDt = newDt;
                ctrl.sqlSeries[0] = s;
                ctrl.objGrafico = gridWarn;
                gridWarn.Location = new System.Drawing.Point(ctrl.x, ctrl.y);
                gridWarn.BestFitColumns();
                gridWarn.Dock = DockStyle.Fill;
                gridWarn.Refresh();

                return gridWarn;
            }
            catch (Exception e)
            {
                MessageBox.Show("error Warnings \n" + e.Message);
            }
            return null;
        }

        private TableLayoutPanel instanciarContador(ControlTowerCtrl ctrl)
        {
            //RadPanel pnl = new RadPanel();
            TableLayoutPanel pnl = new TableLayoutPanel();
            pnl.Dock = DockStyle.Fill;
            pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
            pnl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            pnl.RowStyles.Add(new RowStyle(SizeType.Percent, 66));
            pnl.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
            pnl.AccessibleDescription = ctrl.nombre;

            pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            pnl.Location = new System.Drawing.Point(3, 3);
            pnl.Name = ctrl.nombre;
            pnl.Size = new System.Drawing.Size(279, 197);
            pnl.TabIndex = 0;

            //
            // tTitulo
            //
            RadTextBox tTitulo = new RadTextBox();
            tTitulo.Location = new System.Drawing.Point(9, 158);
            tTitulo.Name = ctrl.nombre;
            tTitulo.Text = ctrl.nombre;
            //tTitulo.Size = new System.Drawing.Size(269, 27);
            tTitulo.TabIndex = 0;
            tTitulo.Dock = DockStyle.Fill;
            tTitulo.ReadOnly = true;
            tTitulo.AutoSize = false;
            tTitulo.TextBoxElement.Border.Visibility = ElementVisibility.Collapsed;
            pnl.SetColumnSpan(tTitulo, 2);

            pnl.Controls.Add(tTitulo, 0, 1);

            //
            // tNumero
            //
            RadTextBox tNumero = new RadTextBox();
            tNumero.Location = new System.Drawing.Point(167, 28);
            tNumero.Name = "Numero";
            tNumero.Text = "?";
            tNumero.Size = new System.Drawing.Size(100, 27);
            tNumero.TabIndex = 1;
            tNumero.Dock = DockStyle.Fill;
            tNumero.ReadOnly = true;
            tNumero.AutoSize = false;
            tNumero.Font = new System.Drawing.Font("Segoe UI", 25F);
            tNumero.TextBoxElement.Border.Visibility = ElementVisibility.Collapsed;
            //tNumero.RootElement.StretchHorizontally = true;
            //tNumero.RootElement.StretchVertically = true;
            pnl.Controls.Add(tNumero, 1, 0);
            //
            // icono
            //
            //RadButton ico = new RadButton();
            PictureBox ico = new PictureBox();
            ico.Location = new System.Drawing.Point(21, 13);
            ico.Name = "ico";
            ico.Size = new System.Drawing.Size(129, 129);
            ico.TabIndex = 2;
            ico.Text = "";

            //ico.DisplayStyle = DisplayStyle.Image;
            //ico.ImageAlignment = ContentAlignment.MiddleCenter;
            ico.SizeMode = PictureBoxSizeMode.Zoom;
            pnl.Controls.Add(ico, 0, 0);

            return pnl;
        }

        #endregion instanciaControles

        private RadChartView InstanciarControlStackedBar(ControlTowerCtrl ctrl)
        {
            try
            {
                CartesianArea cartesianArea = new CartesianArea();
                RadChartView radChartView = new RadChartView();

                radChartView.AreaDesign = cartesianArea;
                radChartView.Location = new System.Drawing.Point(443, 57);
                radChartView.Name = "radChartView1";
                radChartView.ShowGrid = false;
                radChartView.Size = new System.Drawing.Size(222, 296);
                radChartView.TabIndex = 1;
                //this.Controls.Add(radChartView9);
                return radChartView;
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("Error en gráfica de torre de control ") + ctrl.nombre + "\n" + e.Message);
            }
            return null;
        }

        private RadChartView instanciarControlPie(ControlTowerCtrl ctrl)
        {
            try
            {
                Telerik.WinControls.UI.RadChartView pie = new Telerik.WinControls.UI.RadChartView();
                Telerik.WinControls.UI.LinearAxis linearAxis1 = new Telerik.WinControls.UI.LinearAxis();
                Telerik.WinControls.UI.AreaSeries areaSeries1 = new Telerik.WinControls.UI.AreaSeries();
                Telerik.WinControls.UI.CartesianArea cartesianArea1 = new Telerik.WinControls.UI.CartesianArea();
                ctrl.objGrafico = pie;
                ctrl.tipoSerie = ChartSeriesType.Pie;
                pie.AreaDesign = cartesianArea1;
                //categoricalAxis1.IsPrimary = true;
                linearAxis1.AxisType = Telerik.Charting.AxisType.Second;
                linearAxis1.IsPrimary = true;
                linearAxis1.TickOrigin = null;
                //pie.Axes.AddRange(new Telerik.WinControls.UI.Axis[] { categoricalAxis1, linearAxis1 });

                pie.Dock = System.Windows.Forms.DockStyle.Fill;
                pie.Location = new System.Drawing.Point(ctrl.x, ctrl.y);
                pie.Name = ctrl.nombre;
                //areaSeries1.HorizontalAxis = categoricalAxis1;
                areaSeries1.VerticalAxis = linearAxis1;
                //this.chart1.Series.AddRange(new Telerik.WinControls.UI.ChartSeries[] {areaSeries1});
                pie.ShowGrid = false;
                pie.ShowLegend = true;
                pie.ShowTitle = true;
                pie.Size = new System.Drawing.Size(416, 107);
                pie.TabIndex = 2;
                return pie;
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("Error en gráfica de torre de control" + "\n" + ctrl.nombre + "\n" + e.Message));
            }
            return null;
        }

        private List<ControlTowerCtrl> CargarConfCtrlTwr()
        {
            try
            {
                //TODO V1:cargar esto desde un fichero JSON. Añadir en la cabecera el numero de filas y columnas del tablelayoutpal y los segundos del tik de reloj
                //TODO V2: Editar el Json cargandolo en un arbol, tabla o lo que sea y guardandolo despues.

                string origen = Persistencia.DirectorioBase + @"\configs\TorreControl.json";
                StreamReader read = new StreamReader(origen);
                string json = @read.ReadToEnd();
                List<ControlTowerCtrl> controles = JsonConvert.DeserializeObject<List<ControlTowerCtrl>>(json);

                return controles;
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("El fichero de configuración de la torre de control es incorrecto") + "\n" + e.Message);
            }

            return null;
        }

        private System.Drawing.Color BuscaColor(string color)
        {
            //TODO mover a una clase de utilidades general
            switch (color)
            {
                case "rojo":
                case "red":
                    return System.Drawing.Color.Red;

                case "verde":
                case "green":
                    return System.Drawing.Color.Green;

                case "naranja":
                case "orange":
                    return System.Drawing.Color.Orange;

                case "amarillo":
                case "yellow":
                    return System.Drawing.Color.Yellow;

                case "azul":
                case "blue":
                    return System.Drawing.Color.Blue;

                case "blanco":
                case "white":
                    return System.Drawing.Color.White;

                default:
                    return System.Drawing.Color.Black;
            }
        }

        public void inicializarRelojCtrlTwr()
        {
            tik = new System.Timers.Timer(tikMillis);

            tik.Elapsed += new ElapsedEventHandler(TikEvent);
            tik.AutoReset = true;
            tik.Enabled = true;
            log.Info("Se ha iniciado el reloj de la torre de control");

        }

        public void pararRelojCtrlTwr()
        {
            tik.Enabled = false;
            tik.Stop();
        }

        public delegate void ActualizaCallback();

        private void AjustaTimer(ControlTowerCtrl ctrl)
        {
            try
            {
                tikMillis = Convert.ToInt32(ctrl.sqlSeries[0].sql);
            }
            catch (Exception e)
            {
                MessageBox.Show(Lenguaje.traduce("Error en el tiempo de refresco dela torre de control:") + ctrl.sqlSeries[0].sql + "\n" + e.Message);
            }
        }

        private void TikEvent(Object source, ElapsedEventArgs e)
        {
            tik.Enabled = false;
            tik.Stop();
            //TODO actualizar panel solo si es visible, si no, no vale la pena.

            new Task(() => panelCT.BeginInvoke(new ActualizaCallback(MostrarCtrlTwr))).Start();
            tik.Start();
            tik.Enabled = true;
        }
    }
}
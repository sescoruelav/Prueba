using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using RumboSGAManager.Model.Entities;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;
using RumboSGAManager;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI.Export;
using RumboSGA.Properties;
using RumboSGA.Controles;

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class GridHistArticulos : Telerik.WinControls.UI.RadForm
    {
        private string ficheroExportacion;
        private string confirmacion = Lenguaje.traduce(strings.ExportacionExito);
        RadContextMenu contextMenu = new RadContextMenu();
        RumMenuItem exportarMenuItem = new RumMenuItem();
        public GridHistArticulos(string json)
        {
            InitializeComponent();
            contextMenu.Items.Add(exportarMenuItem);
            exportarMenuItem.Text = Lenguaje.traduce(strings.ExportarExcel);
            exportarMenuItem.Click += new EventHandler(rBtnExportarExcel_Click);
            radGridView1.ContextMenuOpening += gridView_ContextMenuOpeningEvent;
            radGridView1.EnableFiltering = true;
            this.Text = Lenguaje.traduce("GridHistArticulos");
            extraerInfoJson(json);
        }

        private void gridView_ContextMenuOpeningEvent(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = contextMenu.DropDown;
        }

        public void extraerInfoJson(string j)
        {
            radGridView1.ReadOnly = true;
           
            string[] columnas={"Id","Cantidad","Id Recurso","Fecha","Hora","Tipo","Movimiento","Saldo","Concepto","Stock Status","Id Entrada",
                                      "Hueco Origen","Hueco Destino","motivo","Id Operario","Nombre Operario","Lote","Informa ERP","Comentario","PedidoCompra","PedidoVenta","Fecha Caducidad"};

            for (int i=0;i<columnas.Length;i++)
            {
                GridViewTextBoxColumn col = new GridViewTextBoxColumn();
                col.FieldName = columnas[i];
                col.HeaderText = Lenguaje.traduce(columnas[i]);
                col.Name = columnas[i];
                radGridView1.Columns.Add(col);
            }
            HistArticulosScheme json = JsonConvert.DeserializeObject<HistArticulosScheme>(j);
            radGridView1.Rows.Add(json.PrimeraLinea.Id, json.PrimeraLinea.Cantidad, json.PrimeraLinea.IdRecurso, json.PrimeraLinea.Fecha,
                                  json.PrimeraLinea.Hora, json.PrimeraLinea.Tipo, Lenguaje.traduce(json.PrimeraLinea.Movimiento), json.PrimeraLinea.Saldo,
                                  Lenguaje.traduce(json.PrimeraLinea.Concepto), json.PrimeraLinea.StockStatus, json.PrimeraLinea.IdEntrada, json.PrimeraLinea.HuecoOrigen,
                                  json.PrimeraLinea.HuecoDestino, Lenguaje.traduce(json.PrimeraLinea.Motivo), json.PrimeraLinea.Idoperario, json.PrimeraLinea.NombreOperario,
                                  json.PrimeraLinea.Lote, json.PrimeraLinea.Informaerp, json.PrimeraLinea.Comentario,json.PrimeraLinea.PedidoCompra,json.PrimeraLinea.PedidoVenta,json.PrimeraLinea.FechaCaducidad);

            for (int i=0;i<json.LineasMedias.Length;i++)
            {
                radGridView1.Rows.Add(json.LineasMedias[i].Id, json.LineasMedias[i].Cantidad, json.LineasMedias[i].IdRecurso, json.LineasMedias[i].Fecha,
                                      json.LineasMedias[i].Hora, json.LineasMedias[i].Tipo, Lenguaje.traduce(json.LineasMedias[i].Movimiento), json.LineasMedias[i].Saldo,
                                      Lenguaje.traduce(json.LineasMedias[i].Concepto), json.LineasMedias[i].StockStatus, json.LineasMedias[i].IdEntrada, json.LineasMedias[i].HuecoOrigen,
                                      json.LineasMedias[i].HuecoDestino, Lenguaje.traduce(json.LineasMedias[i].Motivo), json.LineasMedias[i].Idoperario, json.LineasMedias[i].NombreOperario,
                                      json.LineasMedias[i].Lote, json.LineasMedias[i].Informaerp, json.LineasMedias[i].Comentario, json.LineasMedias[i].PedidoCompra, json.LineasMedias[i].PedidoVenta, json.LineasMedias[i].FechaCaducidad);
            }

            radGridView1.Rows.Add(json.LineaSaldo.Id, json.LineaSaldo.Cantidad, json.LineaSaldo.IdRecurso, json.LineaSaldo.Fecha,
                                  json.LineaSaldo.Hora, json.LineaSaldo.Tipo, Lenguaje.traduce(json.LineaSaldo.Movimiento), json.LineaSaldo.Saldo,
                                   Lenguaje.traduce(json.LineaSaldo.Concepto), json.LineaSaldo.StockStatus, json.LineaSaldo.IdEntrada, json.LineaSaldo.HuecoOrigen,
                                  json.LineaSaldo.HuecoDestino, Lenguaje.traduce(json.LineaSaldo.Motivo), json.LineaSaldo.Idoperario, json.LineaSaldo.NombreOperario,
                                  json.LineaSaldo.Lote, json.LineaSaldo.Informaerp, json.LineaSaldo.Comentario, json.LineaSaldo.PedidoCompra, json.LineaSaldo.PedidoVenta, json.LineaSaldo.FechaCaducidad);

            radGridView1.Rows.Add(json.LineaExistencia.Id, json.LineaExistencia.Cantidad, json.LineaSaldo.IdRecurso, json.LineaExistencia.Fecha,
                                  json.LineaExistencia.Hora, json.LineaExistencia.Tipo, Lenguaje.traduce(json.LineaExistencia.Movimiento), json.LineaExistencia.Saldo,
                                   Lenguaje.traduce(json.LineaExistencia.Concepto), json.LineaExistencia.StockStatus, json.LineaExistencia.IdEntrada, json.LineaExistencia.HuecoOrigen,
                                  json.LineaExistencia.HuecoDestino, Lenguaje.traduce(json.LineaExistencia.Motivo), json.LineaExistencia.Idoperario, json.LineaExistencia.NombreOperario,
                                  json.LineaExistencia.Lote, json.LineaExistencia.Informaerp, json.LineaExistencia.Comentario, json.LineaExistencia.PedidoCompra, json.LineaExistencia.PedidoVenta, json.LineaExistencia.FechaCaducidad);

            RumLabel labelAlertaStock = new RumLabel();
            labelAlertaStock.Text = traducirFilas(json.AlertaStockStatus);
            RumLabel labelFaltaStock = new RumLabel();
            labelFaltaStock.Text = traducirFilas(json.FaltasStockStatus);
            flowLayoutPanel1.Controls.Add(labelAlertaStock);
            flowLayoutPanel1.Controls.Add(labelFaltaStock);
            

            radGridView1.BestFitColumns();

        }

        public string traducirFilas(String textoTraducir)
        {
            string[] lineas = textoTraducir.Split(new[] { '\n' },StringSplitOptions.None);
            string res="";
            for (int i = 0; i < lineas.Length; i++)
            {
                res+= Lenguaje.traduce(lineas[i])+"\n";
            }
            return res;
        }

        private void rBtnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewSpreadStreamExport spreadExporter = new GridViewSpreadStreamExport(this.radGridView1);
                spreadExporter.SheetMaxRows = ExcelMaxRows._1048576;
                //spreadExporter.ExportFormat = SpreadExportFormat.Xlsx;
                spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;
                spreadExporter.PagingExportOption = PagingExportOption.AllPages;
                spreadExporter.AsyncExportProgressChanged += spreadExporter_AsyncExportProgressChanged;
                spreadExporter.AsyncExportCompleted += spreadExporter_AsyncExportCompleted;

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "";
                dialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                   
                    ficheroExportacion = dialog.FileName;
                    spreadExporter.RunExportAsync(ficheroExportacion, new SpreadStreamExportRenderer());
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                
            }
        }
        private void spreadExporter_AsyncExportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }
        private void spreadExporter_AsyncExportCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                bool openExportFile = false;
               
                DialogResult dr = RadMessageBox.Show(confirmacion,
                       Lenguaje.traduce(strings.ExportarExcel), MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    openExportFile = true;
                }
                if (openExportFile)
                {
                    try
                    {

                        System.Diagnostics.Process.Start(ficheroExportacion);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format(Lenguaje.traduce(strings.ExportarError) + "\nError message: {0}", ex.Message);
                        RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                    finally
                    {
                        ficheroExportacion= "";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);

            }
            finally
            {
                
            }
        }
    }
}

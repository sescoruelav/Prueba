using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Maestros;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.PedidoCliMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class FrmSeleccionarExistencia : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
        private WSOrdenProduccionMotorClient OrdenProduccion = new WSOrdenProduccionMotorClient();
        private string tb;
        private int idReserva;
        public int idEntrada;
        private DataTable dtReservas;

        public FrmSeleccionarExistencia(int idReserva_)
        {
            try
            {
                InitializeComponent();
                this.Text = Lenguaje.traduce("Seleccionar existencia");
                dtReservas = ConexionSQL.getDataTable("SELECT A.IDPRESENTACION,R.* FROM dbo.TBLARTICULOS A INNER JOIN dbo.TBLRESERVAS R ON R.IDARTICULO=A.IDARTICULO WHERE R.IDRESERVA=" + idReserva);
                this.idReserva = idReserva_;
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        private void LlenarGrid(int idReserva)
        {
            WSPedidoCliMotorClient ws = new WSPedidoCliMotorClient();
            String query = ws.getQueryExistenciasIntercambiables(idReserva);
            DataTable dt = ConexionSQL.getDataTable(query);
            Utilidades.TraducirDataTableColumnName(ref dt);

            rgvExistencias.DataSource = dt;
            rgvExistencias.BestFitColumns();
            AplicarPresentaciones();
        }

        private void AplicarPresentaciones()
        {
            try
            {
                foreach (GridViewRowInfo row in rgvExistencias.Rows)
                {

                    int idArticulo = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Articulo")].Value);
                    int uds = Convert.ToInt32(row.Cells[Lenguaje.traduce("Cantidad")].Value);                    
                    object[] presUds = Presentaciones.getCantidadPresentacionVisualizacionArticulo(idArticulo, uds);
                    
                    row.Cells[Lenguaje.traduce("Cantidad")].Value = presUds[0];
                    row.Cells[Lenguaje.traduce("ID Unidad Tipo")].Value = presUds[1].ToString();
                    rgvExistencias.Columns[Lenguaje.traduce("Unidad Tipo")].IsVisible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SetPreferences()
        {
            try
            {
                rgvExistencias.Dock = DockStyle.Fill;
                for (int i = 1; i < rgvExistencias.Columns.Count; i++)
                {
                    rgvExistencias.Columns[i].ReadOnly = true;
                }
                rgvExistencias.MultiSelect = false;
                this.rgvExistencias.MasterTemplate.EnableGrouping = true;
                this.rgvExistencias.ShowGroupPanel = true;
                this.rgvExistencias.MasterTemplate.AutoExpandGroups = true;
                this.rgvExistencias.EnableHotTracking = true;
                this.rgvExistencias.MasterTemplate.AllowAddNewRow = false;
                this.rgvExistencias.MasterTemplate.AllowColumnResize = true;
                this.rgvExistencias.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvExistencias.AllowSearchRow = true;
                this.rgvExistencias.EnablePaging = false;
                this.rgvExistencias.TableElement.RowHeight = 40;
                this.rgvExistencias.AllowRowResize = false;
                this.rgvExistencias.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvExistencias.EnableFiltering = true;
                rgvExistencias.MasterTemplate.EnableFiltering = true;
                rgvExistencias.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rBtnSeleccionar_Click(object sender, EventArgs e)
        {
            try
            {
                log.Info("Pulsado botón consumir web service " + DateTime.Now);
                foreach (GridViewRowInfo row in rgvExistencias.Rows)
                {
                    //check if its current child row
                    if (row.IsSelected)
                    {
                        idEntrada = Convert.ToInt32(row.Cells[Lenguaje.traduce("ID Entrada")].Value);
                        break;
                    }
                }
                if (idEntrada > 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener al menos una fila seleccionada."));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorWS(ex, OrdenProduccion.Endpoint);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rBtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Format_GridColumns(RadGridView dgv_format)
        {
            foreach (GridViewDataColumn dCol in dgv_format.Columns)

            {
                if (dCol.DataType == typeof(Decimal))
                {
                    /*if(dCol.FormatString.ToLower() =="{0}")
                    {*/
                    dCol.FormatString = "{0:N2}";  //                        "{0:N2}"
                    /*}*/
                }
                if (dCol.DataType == typeof(Single))
                {
                    /*if (dCol.FormatString.ToLower() == "{0}")
                    {*/
                    dCol.FormatString = "{0:N2}";  //                        "{0:N2}"
                    /*}*/
                }
            }
        }

        private void radGridView_1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnIndex > 2)
            {
                e.CellElement.Text = String.Format("{0:N2}", ((GridDataCellElement)e.CellElement).Value);
            }
        }

        private void FrmSeleccionarExistencia_Load(object sender, EventArgs e)
        {
            try
            {
                LlenarGrid(idReserva);
                rgvExistencias.BestFitColumns();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                rBtnCancelar.Click += rBtnCancelar_Click;
                rBtnSeleccionar.DoubleClick += rBtnSeleccionar_Click;
                SetPreferences();
                Format_GridColumns(rgvExistencias);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }
    }
}
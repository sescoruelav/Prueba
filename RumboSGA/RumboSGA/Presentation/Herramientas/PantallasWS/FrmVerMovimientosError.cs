using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Maestros;
using RumboSGA.OrdenProduccionMotor;
using RumboSGA.PedidoCliMotor;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGA.ReservaMotor;
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
    public partial class FrmVerMovimientosError : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();        
        string tb;        
        DataTable dtReservas;
        String queryUbicacion = "SELECT r.idreserva,r.identrada,r.idreservatipo, r.idarticulo,"
                + "a.referencia,a.descripcion,r.idhuecoorigen,ho.descripcion as Origen,"
                + " r.idhuecodestino,hd.descripcion as Destino ,r.idtarea "
                + " FROM TBLRESERVAS R join tblarticulos a on r.idarticulo=a.idarticulo "
                + " join tblhuecos ho on ho.idhueco=r.idhuecoorigen "
                + " join tblhuecos hd on hd.idhueco=r.idhuecodestino "
                + " where idreservaestado='ER'";
        String queryCarro = "SELECT MC.*, HC.DESCRIPCION AS CARRO, "
                + " HO.DESCRIPCION AS ORIGEN, HD.DESCRIPCION AS DESTINO "
                + " FROM TBLMOVIMIENTOSCARRO MC "
                + " INNER JOIN TBLHUECOS HO ON HO.IDHUECO=MC.IDHUECOORIGEN "
                + " INNER JOIN TBLHUECOS HD ON HD.IDHUECO=MC.IDHUECODESTINO "
                + " INNER JOIN TBLCARROMOVILCAB CM ON CM.IDCARRO=MC.IDCARRO "
                + " INNER JOIN TBLHUECOS HC ON CM.IDUBICACIONPRINCIPAL=HC.IDHUECO "
                + " where mc.IDMOVIMIENTOESTADO ='ER'";
        String queryBulto = "SELECT MB.*, CC.EXPEDICION, " + " CASE MB.NIVEL "
                + " WHEN 3 THEN (SELECT SSCC FROM TBLENTRADAS E WHERE E.IDENTRADA=MB.BULTO) "
                + " END AS SSCC, "
                + " HO.DESCRIPCION AS ORIGEN, HD.DESCRIPCION AS DESTINO "
                + " FROM TBLMOVIMIENTOSBULTO MB "
                + " INNER JOIN TBLHUECOS HO ON HO.IDHUECO=MB.IDHUECOORIGEN "
                + " INNER JOIN TBLHUECOS HD ON HD.IDHUECO=MB.IDHUECODESTINO "
                + " LEFT JOIN TBLCARGACAB CC ON CC.IDCARGA=MB.IDCARGA "
                + " where mb.IDMOVIMIENTOESTADO ='ER'";
        public FrmVerMovimientosError()
        {
            try
            {
                InitializeComponent();
                this.Text = Lenguaje.traduce("Movimientos");
                this.ribbonTabMovimientos.Text = Lenguaje.traduce("Ubicación");
                this.ribbonTabCarro.Text = Lenguaje.traduce("Carro");
                this.ribbonTabBulto.Text = Lenguaje.traduce("Bulto");
                this.ribbonTabMovimientos.Select();

                rellenarMovimientos();
                

            } catch (Exception ex)
            {
                this.Close();
            }
        }
        private void SetPreferences()
        {
            try
            {
                rgvMovimientos.Dock = DockStyle.Fill;
                for (int i = 1; i < rgvMovimientos.Columns.Count; i++)
                {
                    rgvMovimientos.Columns[i].ReadOnly = true;
                }                
                this.rgvMovimientos.MultiSelect = true;
                this.rgvMovimientos.MasterTemplate.EnableGrouping = true;
                this.rgvMovimientos.ShowGroupPanel = true;
                this.rgvMovimientos.AllowEditRow = false;
                this.rgvMovimientos.MasterTemplate.AutoExpandGroups = true;
                this.rgvMovimientos.EnableHotTracking = true;
                this.rgvMovimientos.MasterTemplate.AllowAddNewRow = false;
                this.rgvMovimientos.MasterTemplate.AllowColumnResize = true;
                this.rgvMovimientos.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvMovimientos.AllowSearchRow = true;
                this.rgvMovimientos.EnablePaging = false;
                this.rgvMovimientos.TableElement.RowHeight = 40;
                this.rgvMovimientos.AllowRowResize = false;                
                this.rgvMovimientos.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvMovimientos.EnableFiltering = true;
                rgvMovimientos.MasterTemplate.EnableFiltering = true;
                rgvMovimientos.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        

        private void rBtnRegenerarTareaMov_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> reservas = new List<int>();
                log.Info("Pulsado botón consumir web service " + DateTime.Now);
                foreach (GridViewRowInfo row in rgvMovimientos.Rows)
                {
                    //check if its current child row
                    if (row.IsSelected)
                    {
                        int idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDReserva")].Value);
                        reservas.Add(idReserva);
                    }
                }
                if (reservas.Count > 0)
                {
                    regenerarTareaMov(reservas);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener al menos una fila seleccionada."));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void regenerarTareaMov(List<int> reservas)
        {
            try
            {
                WSReservaMotorClient ws = new WSReservaMotorClient();
                object[] respuesta=ws.liberarMovimientosEnError(reservas.ToArray());
                if (respuesta.Length > 0)
                {
                    string error = "";
                    foreach (object resp in respuesta)
                    {
                        error = error + System.Environment.NewLine + Lenguaje.traduce(resp.ToString());

                    }
                    throw new Exception (error);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Se han liberado los movimientos seleccionados"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rellenarMovimientos();
                }

            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex,ex.Message);
                
                
            }
        }
        private void regenerarTareaCarro(List<int> movs)
        {
            try
            {
                WSReservaMotorClient ws = new WSReservaMotorClient();
                object[] respuesta=ws.liberarMovimientosCarroEnError(movs.ToArray());
                if (respuesta.Length > 0)
                {
                    string error = "";
                    foreach (object resp in respuesta)
                    {
                        error = error + System.Environment.NewLine + Lenguaje.traduce( resp.ToString());

                    }
                    throw new Exception(error);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Se han liberado los movimientos seleccionados"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rellenarCarro();
                }
               
               
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);

            }
        }
        private void regenerarTareaBulto(List<int> movs)
        {
            try
            {
                WSReservaMotorClient ws = new WSReservaMotorClient();
                object[] respuesta = ws.liberarMovimientosBultoEnError(movs.ToArray());
                if (respuesta.Length > 0)
                {
                    string error = "";
                    foreach (object resp in respuesta)
                    {
                        error = error +System.Environment.NewLine + Lenguaje.traduce(resp.ToString());

                    }
                    throw new Exception(error);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Se han liberado los movimientos seleccionados"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rellenarBultos();
                }
                
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);

            }
        }

        private void rBtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void Format_GridColumns(RadGridView dgv_format){
            foreach (GridViewDataColumn dCol in dgv_format.Columns)

            {
                
                if (dCol.DataType ==typeof(Decimal))
                {
                    /*if(dCol.FormatString.ToLower() =="{0}")
                    {*/
                        dCol.FormatString ="{0:N2}";  //                        "{0:N2}"                   
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




        private void radGridView_1_CellFormatting(object sender,CellFormattingEventArgs e)
        {


            if(e.CellElement.ColumnIndex > 2)
            {
                e.CellElement.Text =String.Format("{0:N2}", ((GridDataCellElement)e.CellElement).Value);

            }

        }

        private void FrmVerMovimientosError_Load(object sender, EventArgs e)
        {
            try
            {


               // LlenarGrid(idReserva);
                
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                rBtnCancelar.Click += rBtnCancelar_Click;
                rBtnRegenerarTareaReserva.DoubleClick += rBtnRegenerarTareaMov_Click;
                SetPreferences();
                Format_GridColumns(rgvMovimientos);
                this.ribbonTabMovimientos.IsSelected = true;
            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex,ex.Message);
            }

        }

       

        private void ribbonTabCarro_Click(object sender, EventArgs e)
        {
            try
            {
                rellenarCarro();
            }
            catch(Exception ex)
            {

            }
        }

        private void ribbonTabBulto_Click(object sender, EventArgs e)
        {
            try
            {
                rellenarBultos();
            }
            catch (Exception ex)
            {

            }
        }

        private void rBtnCancelarCarro_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void rBtnCancelarBulto_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void rBtnRegenerarTareaCarro_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> reservas = new List<int>();
                log.Info("Pulsado botón consumir web service " + DateTime.Now);
                foreach (GridViewRowInfo row in rgvMovimientos.Rows)
                {
                    //check if its current child row
                    if (row.IsSelected)
                    {
                        int idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDMOVIMIENTO")].Value);
                        reservas.Add(idReserva);
                    }
                }
                if (reservas.Count > 0)
                {
                    regenerarTareaCarro(reservas);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener al menos una fila seleccionada."));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void rBtnRegenerarTareaBulto_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> reservas = new List<int>();
                log.Info("Pulsado botón consumir web service " + DateTime.Now);
                foreach (GridViewRowInfo row in rgvMovimientos.Rows)
                {
                    //check if its current child row
                    if (row.IsSelected)
                    {
                        int idReserva = Convert.ToInt32(row.Cells[Lenguaje.traduce("IDMOVIMIENTO")].Value);
                        reservas.Add(idReserva);
                    }
                }
                if (reservas.Count > 0)
                {
                    regenerarTareaBulto(reservas);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("Es necesario tener al menos una fila seleccionada."));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void ribbonTabMovimientos_Click(object sender, EventArgs e)
        {
            rellenarMovimientos();
        }

        private void rellenarMovimientos()
        {
            try
            {
                dtReservas = ConexionSQL.getDataTable(queryUbicacion);
                Utilidades.TraducirDataTableColumnName(ref dtReservas);
                rgvMovimientos.DataSource = dtReservas;
                rgvMovimientos.Refresh();
                rgvMovimientos.BestFitColumns();
            }catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void rellenarCarro()
        {
            try
            {
                dtReservas = ConexionSQL.getDataTable(queryCarro);
                Utilidades.TraducirDataTableColumnName(ref dtReservas);
                rgvMovimientos.DataSource = dtReservas;
                rgvMovimientos.Refresh();
                rgvMovimientos.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void rellenarBultos()
        {
            try
            {
                dtReservas = ConexionSQL.getDataTable(queryBulto);
                Utilidades.TraducirDataTableColumnName(ref dtReservas);
                rgvMovimientos.DataSource = dtReservas;
                rgvMovimientos.Refresh();
                rgvMovimientos.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
    }
}

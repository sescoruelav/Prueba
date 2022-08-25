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
    public partial class FrmSeleccionarReposiciones : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();        
        string tb;        
        DataTable dtReservas;
        String query = "SELECT R.idreserva, r.idarticulo,a.referencia," +
                    "idhuecoorigen,ho.descripcion as ORIGEN,idhuecodestino,hd.descripcion as DESTINO,r.cantidad," +
                    "r.idrecurso,rec.descripcion as RECURSO,r.fecha as [FECHA CREACION]"
                + " FROM TBLRESERVAS R join tblarticulos a on r.idarticulo=a.idarticulo "
                + " join tblhuecos ho on ho.idhueco=r.idhuecoorigen "
                + " join tblhuecos hd on hd.idhueco=r.idhuecodestino "
                + " join tblrecursos rec on rec.idrecurso=r.idrecurso "
                + " where idreservatipo='RP' " +
                "AND IDENTRADA NOT IN  " +
                "(SELECT IDENTRADA FROM TBLRESERVAS R1 WHERE R1.IDRESERVATIPO='PI')";
        public FrmSeleccionarReposiciones()
        {
            try
            {
                InitializeComponent();
                this.Text = Lenguaje.traduce("Eliminar Reposiciones");
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
                rgvReposiciones.Dock = DockStyle.Fill;
                for (int i = 1; i < rgvReposiciones.Columns.Count; i++)
                {
                    rgvReposiciones.Columns[i].ReadOnly = true;
                }                
                this.rgvReposiciones.MultiSelect = true;
                this.rgvReposiciones.MasterTemplate.EnableGrouping = true;
                this.rgvReposiciones.ShowGroupPanel = true;
                this.rgvReposiciones.AllowEditRow = false;
                this.rgvReposiciones.MasterTemplate.AutoExpandGroups = true;
                this.rgvReposiciones.EnableHotTracking = true;
                this.rgvReposiciones.MasterTemplate.AllowAddNewRow = false;
                this.rgvReposiciones.MasterTemplate.AllowColumnResize = true;
                this.rgvReposiciones.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvReposiciones.AllowSearchRow = true;
                this.rgvReposiciones.EnablePaging = false;
                this.rgvReposiciones.TableElement.RowHeight = 40;
                this.rgvReposiciones.AllowRowResize = false;                
                this.rgvReposiciones.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvReposiciones.EnableFiltering = true;
                rgvReposiciones.MasterTemplate.EnableFiltering = true;
                rgvReposiciones.BestFitColumns();
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
                List<int> reservas = new List<int>();
                log.Info("Pulsado botón consumir web service " + DateTime.Now);
                foreach (GridViewRowInfo row in rgvReposiciones.Rows)
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
                    borrarReservasReposicion(reservas);
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
        private void rellenarMovimientos()
        {
            try
            {
                dtReservas = ConexionSQL.getDataTable(query);
                Utilidades.TraducirDataTableColumnName(ref dtReservas);
                rgvReposiciones.DataSource = dtReservas;
                rgvReposiciones.Refresh();
                rgvReposiciones.BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void borrarReservasReposicion(List<int> reservas)
        {
            try
            {
                WSReservaMotorClient ws = new WSReservaMotorClient();
                ws.borrarReposicionesSinReserva(reservas.ToArray(), User.IdOperario);
                MessageBox.Show(Lenguaje.traduce("Se han eliminado las reposiciones seleccionadas"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rellenarMovimientos();
            }
            catch(Exception ex)
            {
                throw ex;
                
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

        private void FrmSeleccionarExistencia_Load(object sender, EventArgs e)
        {
            try
            {


               // LlenarGrid(idReserva);
                
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                rBtnCancelar.Click += rBtnCancelar_Click;
                rBtnSeleccionar.DoubleClick += rBtnSeleccionar_Click;
                SetPreferences();
                Format_GridColumns(rgvReposiciones);
            }catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex,ex.Message);
            }

        }
    }
}

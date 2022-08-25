using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class FrmSeleccionFechaEntrega : Telerik.WinControls.UI.RadForm
    {

        DataTable dtFechas;
        private GridViewCheckBoxColumn chkColFechas = new GridViewCheckBoxColumn();
        public List<FechasEntrega> fechas = new List<FechasEntrega>();
        public FrmSeleccionFechaEntrega(string where)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Seleccionar fecha entrega");
            dtFechas = Business.getDatosFechasEntregaLineasPedidos(where);
            
        }

        private void FrmSeleccionFechaEntrega_Load(object sender, EventArgs e)
        {
            try
            {

                if (dtFechas.Rows.Count <= 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                LlenarGrid();
               // SetPreferences();
                anyadirColumnaCheck();
               
                rgvFechas.BestFitColumns();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;               
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, Lenguaje.traduce("Error cargando fechas de entrega ."), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ExceptionManager.GestionarError(ex);
            }
        }

        private void SetPreferences()
        {
            rgvFechas.BestFitColumns();
            for (int i = 1; i < rgvFechas.Columns.Count; i++)
            {
                rgvFechas.Columns[i].ReadOnly = true;
            }
            rgvFechas.MultiSelect = true;
            this.rgvFechas.MasterTemplate.EnableGrouping = false;
            this.rgvFechas.ShowGroupPanel = false;
            this.rgvFechas.MasterTemplate.AutoExpandGroups = true;
            this.rgvFechas.EnableHotTracking = true;
            this.rgvFechas.MasterTemplate.AllowAddNewRow = false;
            this.rgvFechas.MasterTemplate.AllowColumnResize = true;
            this.rgvFechas.MasterTemplate.AllowMultiColumnSorting = true;
            this.rgvFechas.AllowColumnReorder = false;
            this.rgvFechas.AllowSearchRow = false;
            this.rgvFechas.EnablePaging = false;
            this.rgvFechas.TableElement.RowHeight = 40;
            this.rgvFechas.AllowRowResize = false;
            this.rgvFechas.MasterView.TableSearchRow.SearchDelay = 2000;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            rgvFechas.EnableFiltering = false;
            rgvFechas.MasterTemplate.EnableFiltering = false;
        }

        private void anyadirColumnaCheck()
        {
            chkColFechas.Name = "checkColumn";
            chkColFechas.EnableHeaderCheckBox = true;
            chkColFechas.HeaderText = "";
            chkColFechas.EditMode = EditMode.OnValueChange;
            chkColFechas.CheckFilteredRows = false;
            if (rgvFechas.Columns[0] != chkColFechas)
            {
                rgvFechas.Columns.Add(chkColFechas);
                rgvFechas.Columns.Move(rgvFechas.Columns.Count - 1, 0);
            }
            rgvFechas.Columns[0].ReadOnly = false;
            UtilidadesGui.ReadOnlyTrueExceptoCheckBoxGridView(ref rgvFechas);
        }

        private void LlenarGrid()
        {
           
            Utilidades.TraducirDataTableColumnName(ref dtFechas);
            rgvFechas.DataSource = dtFechas;
            rgvFechas.Columns["idpedidocli"].IsVisible = false;
            rgvFechas.Refresh();
            rgvFechas.BestFitColumns();

          

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            try
            {
               
                foreach (GridViewRowInfo row in rgvFechas.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        int idPedidoCli=Convert.ToInt32(row.Cells[Lenguaje.traduce("idpedidocli")].Value);
                        if (row.Cells[Lenguaje.traduce("Fecha Entrega")].Value !=null)
                        {
                            DateTime fecha = Convert.ToDateTime(row.Cells[Lenguaje.traduce("Fecha Entrega")].Value);
                            FechasEntrega fechaEntrega = new FechasEntrega();
                            fechaEntrega.idpedidocli = idPedidoCli;
                            fechaEntrega.fechaentrega = fecha;
                            fechas.Add(fechaEntrega);
                        }
                        
                    }
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, Lenguaje.traduce("Error obteniendo fechas entrega ."), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ExceptionManager.GestionarError(ex);
            }


        }
        public class FechasEntrega
        {
            [JsonProperty]
            public int idpedidocli { get; set; }
            public DateTime fechaentrega { get; set; }
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }   
    }


}

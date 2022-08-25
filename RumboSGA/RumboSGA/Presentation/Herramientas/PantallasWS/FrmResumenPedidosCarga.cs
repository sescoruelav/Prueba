using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class FrmResumenPedidosCarga : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected List<TableScheme> _lstEsquemaTablaCargaPedidos = new List<TableScheme>();
        protected List<TableScheme> _lstEsquemaTablaCargaPedidosLineas = new List<TableScheme>(); 
        private int idCarga = -1;
        private bool cerrar = false;
        private bool soloFaltas = false;
        private ProgressBarColumn colpctCantidadReservada = new ProgressBarColumn(Lenguaje.traduce(strings.pctCantidadReservada));
        private ProgressBarColumn colpctCantidadServida = new ProgressBarColumn(Lenguaje.traduce(strings.pctCantidadServida));
        private ProgressBarColumn colLineasReservadas = new ProgressBarColumn(Lenguaje.traduce(strings.LineasReservadas));
        private ProgressBarColumn colLineasPreparadas = new ProgressBarColumn(Lenguaje.traduce(strings.LineasPreparadas));
        private DataTable pedidosTabla;
        private DataTable lineasPedido;
        GridViewTemplate tmpLineasPedido = new GridViewTemplate();

        public FrmResumenPedidosCarga(int idCarga_, bool soloFaltas_)
        {
            try
            {
                InitializeComponent();
                this.Text = Lenguaje.traduce("Confirmación cierre carga");
                this.idCarga = idCarga_;
                this.soloFaltas = soloFaltas;

               

            } catch (Exception ex)
            {
                this.Close();
            }
        }

       
        private void LlenarGrid(int idReserva)
        {
            pedidosTabla = Business.GetOrdenCargaPedidosResumen(_lstEsquemaTablaCargaPedidos, idCarga.ToString());
            rgvResumenCargaPedidos.DataSource = pedidosTabla;
            try
            {
                rgvResumenCargaPedidos.Columns.Remove(colLineasPreparadas.FieldName);
                rgvResumenCargaPedidos.Columns.Remove(colLineasReservadas.FieldName);
              
            }
            catch (Exception ex)
            {

            }
            rgvResumenCargaPedidos.Columns.Add(colLineasPreparadas);
            rgvResumenCargaPedidos.Columns.Add(colLineasReservadas);
            rgvResumenCargaPedidos.Columns.Move(colLineasReservadas.Index, 3);
            rgvResumenCargaPedidos.Columns.Move(colLineasPreparadas.Index,4);




        }

        private void RelacionGridLineas()
        {
            try
            {
                lineasPedido = Business.GetOrdenesCargaPedidosResumenLineasJerarquicoDatosGridView(idCarga.ToString());
               
                tmpLineasPedido.DataSource = lineasPedido;
                GridViewRelation relacion = new GridViewRelation(rgvResumenCargaPedidos.MasterTemplate);
                relacion.ChildTemplate = tmpLineasPedido;
                relacion.RelationName = "LineasPedido";
                relacion.ParentColumnNames.Add(Lenguaje.traduce("ID"));
                relacion.ChildColumnNames.Add(Lenguaje.traduce("ID"));
                rgvResumenCargaPedidos.Relations.Add(relacion);
                rgvResumenCargaPedidos.MasterTemplate.Templates.Add(tmpLineasPedido);              
                
                tmpLineasPedido.BestFitColumns(BestFitColumnMode.DisplayedCells);
                tmpLineasPedido.AllowAddNewRow = false;
                tmpLineasPedido.AllowEditRow = false;
                tmpLineasPedido.AllowDeleteRow = false;
                try
                {
                    tmpLineasPedido.Columns.Remove(colpctCantidadServida.FieldName);
                    tmpLineasPedido.Columns.Remove(colpctCantidadReservada.FieldName);

                }
                catch (Exception ex)
                {

                }
                tmpLineasPedido.Columns.Add(colpctCantidadServida);
                tmpLineasPedido.Columns.Add(colpctCantidadReservada);

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void OcultarColumnas()
        {
            try
            {
                rgvResumenCargaPedidos.Columns[Lenguaje.traduce("Num Pedido Cliente")].IsVisible = false;
                rgvResumenCargaPedidos.Columns[Lenguaje.traduce("Num Pedido Cliente")].VisibleInColumnChooser = true;
                rgvResumenCargaPedidos.Columns[Lenguaje.traduce("Num Pedido Cliente")].AllowHide = true;
                tmpLineasPedido.Columns[Lenguaje.traduce("Num Pedido Cliente")].IsVisible = false;
                tmpLineasPedido.Columns[Lenguaje.traduce("Num Pedido Cliente")].VisibleInColumnChooser = true;
                tmpLineasPedido.Columns[Lenguaje.traduce("Num Pedido Cliente")].AllowHide = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        private void SetPreferences()
        {
            try
            {
                rgvResumenCargaPedidos.Dock = DockStyle.Fill;
                for (int i = 1; i < rgvResumenCargaPedidos.Columns.Count; i++)
                {
                    rgvResumenCargaPedidos.Columns[i].ReadOnly = true;
                }                
                rgvResumenCargaPedidos.MultiSelect = false;
                this.rgvResumenCargaPedidos.MasterTemplate.EnableGrouping = true;
                this.rgvResumenCargaPedidos.ShowGroupPanel = true;
                this.rgvResumenCargaPedidos.MasterTemplate.AutoExpandGroups = true;
                this.rgvResumenCargaPedidos.EnableHotTracking = true;
                this.rgvResumenCargaPedidos.MasterTemplate.AllowAddNewRow = false;
                this.rgvResumenCargaPedidos.MasterTemplate.AllowColumnResize = true;
                this.rgvResumenCargaPedidos.MasterTemplate.AllowMultiColumnSorting = true;
                this.rgvResumenCargaPedidos.AllowSearchRow = true;               
                this.rgvResumenCargaPedidos.TableElement.RowHeight = 40;
                this.rgvResumenCargaPedidos.AllowRowResize = false;                
                this.rgvResumenCargaPedidos.MasterView.TableSearchRow.SearchDelay = 2000;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                rgvResumenCargaPedidos.EnableFiltering = true;
                rgvResumenCargaPedidos.MasterTemplate.EnableFiltering = true;
                rgvResumenCargaPedidos.BestFitColumns(BestFitColumnMode.DisplayedCells);


            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        

        private void rBtnConfirmar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        

        private void rBtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        





        private void FrmResumenPedidosCarga_Load(object sender, EventArgs e)
        {
            try
            {             
                              
                LlenarGrid(idCarga);
                RelacionGridLineas();
               
                rgvResumenCargaPedidos.BestFitColumns();
                this.ThemeName = ThemeResolutionService.ApplicationThemeName;
                SetPreferences();
                rBtnCancelar.Click += rBtnCancelar_Click;
                rBtnConfirmar.DoubleClick += rBtnConfirmar_Click;
                rgvResumenCargaPedidos.MasterTemplate.ExpandAll();

            }
            catch(Exception ex)
            {
                ExceptionManager.GestionarError(ex,ex.Message);
            }

        }
        #region ProgressBarColumn
        public class ProgressBarCellElement : GridDataCellElement
        {
            private RadProgressBarElement radProgressBarElement;
            public ProgressBarCellElement(GridViewColumn column, GridRowElement row) : base(column, row)
            {
                radProgressBarElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            }
            protected override void CreateChildElements()
            {
                try
                {
                    base.CreateChildElements();
                    radProgressBarElement = new RadProgressBarElement();
                    this.Children.Add(radProgressBarElement);
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
            protected override void SetContentCore(object value)
            {
                try
                {
                    if (this.Value != null && this.Value != DBNull.Value)
                    {
                        int valor = 0;
                        this.radProgressBarElement.Maximum = 100;
                        radProgressBarElement.Minimum = 0;
                        if (Convert.ToDecimal(this.Value) >= 100)
                        {
                            valor = 100;
                            radProgressBarElement.Value1 = valor;
                            this.radProgressBarElement.Text = valor + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                        else if (Convert.ToDecimal(this.Value) <= 0)
                        {
                            valor = 0;
                            radProgressBarElement.Value1 = valor;
                            this.radProgressBarElement.Text = valor + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                        else
                        {
                            this.radProgressBarElement.Value1 = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Value), 0));/* Convert.ToInt16(this.Value);*/
                            this.radProgressBarElement.Text = this.radProgressBarElement.Value1 + "%";
                            radProgressBarElement.ProgressOrientation = ProgressOrientation.Left;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                    log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
                }
            }
            protected override Type ThemeEffectiveType
            {
                get
                {
                    return typeof(GridDataCellElement);
                }
            }
            public override bool IsCompatible(GridViewColumn data, object context)
            {
                return data is ProgressBarColumn && context is GridDataRowElement;
            }
        }
        public class ProgressBarColumn : GridViewDataColumn
        {
            public ProgressBarColumn(string fieldName) : base(fieldName)
            {
            }
            public override Type GetCellType(GridViewRowInfo row)
            {
                if (row is GridViewDataRowInfo)
                {
                    return typeof(ProgressBarCellElement);
                }
                return base.GetCellType(row);
            }
        }


        #endregion

        
        }
}

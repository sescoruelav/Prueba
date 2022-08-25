using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;
using Newtonsoft.Json.Linq;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class TareasTipoControl : BaseGridControl
    {
        #region Variables y Propiedades

        List<dynamic> data = new List<dynamic>();
        RadColorDialog dialog = new RadColorDialog();

        public Action<List<dynamic>> Callback { get; private set; }
        public Action<int> CallbackCantidad { get; private set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GridViewDecimalColumn col = new GridViewDecimalColumn();
        #endregion

        #region Constructor

        public TareasTipoControl()
        {
            InitializeComponent();
            InicializarEventos();

            ObtencionTotalRegistros();

            ElegirGrid();
            //ElegirEstilo();
            RefreshData(0);
            if (GridView.Rows.Count != 0)
            {
                GridView.Rows[0].IsSelected = false;
            }
            GridView.ContextMenuOpening += radGridView1_ContextMenuOpening;
            ColorTipoTarea();
        }

        #endregion
        #region Color Tarea
        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
                if (this.GridView.CurrentRow is GridViewDataRowInfo)
                {
                    if (CultureInfo.CurrentUICulture.Name == "es-ES")
                    {
                        if (GridView.CurrentColumn.Name == "DESCRIPCION")
                        {
                            RadDropDownMenu menu = new RadDropDownMenu();
                            RadMenuItem menuItem = new RadMenuItem("Asignar nuevo color");
                            menuItem.Click += new EventHandler(menu_ItemClicked);
                            menu.Items.Add(menuItem);
                            e.ContextMenu = menu;
                        }
                    }
                    else
                    {
                        if (GridView.CurrentColumn.Name == "DESCRIPCION")
                        {
                            RadDropDownMenu menu = new RadDropDownMenu();
                            RadMenuItem menuItem = new RadMenuItem("New Colour");
                            menuItem.Click += new EventHandler(menu_ItemClicked);
                            menu.Items.Add(menuItem);
                            e.ContextMenu = menu;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void menu_ItemClicked(object sender, EventArgs e)
        {
            try
            {
                string estado = string.Empty;
                string color = SetCellBackground();
                GridView.CurrentCell.BackColor = dialog.SelectedColor;
                estado = GridView.CurrentRow.Cells["Descripcion"].Value.ToString();
                XmlReaderPropio.setColorTipoTareas(color, estado);
                Reload();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void Reload()
        {
            try
            {
                ObtencionTotalRegistros();
                ElegirGrid();
                ElegirEstilo();
                GridView.ContextMenuOpening += radGridView1_ContextMenuOpening;
                ColorTipoTarea();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private string SetCellBackground()
        {
            int row = (int)this.GridView.CurrentCell.RowInfo.Index;
            int column = (int)this.GridView.CurrentCell.ColumnInfo.Index;
            string valorColor = "";
            ((RadForm)dialog.ColorDialogForm).ThemeName = "CAMBIO COLOR";
            dialog.SelectedColor = this.GridView.Rows[row].Cells[column].Style.BackColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                GridView.Rows[row].Cells[column].Style.BackColor = dialog.SelectedColor;
                GridView.CurrentCell.BackColor = dialog.SelectedColor;
                valorColor = dialog.SelectedColor.ToArgb().ToString();
            }
            return valorColor;
        }
        private void ColorTipoTarea()
        {
            try
            {
                ConditionalFormattingObject obj1 = new ConditionalFormattingObject("Mi Condicion1", ConditionTypes.Equal, "CARGA", "", false);
                obj1.CellBackColor = XmlReaderPropio.getColorTipoTareas("CARGA");
                ConditionalFormattingObject obj2 = new ConditionalFormattingObject("Mi Condición2", ConditionTypes.Equal, "CONTEO", "", false);
                obj2.CellBackColor = XmlReaderPropio.getColorTipoTareas("CONTEO");
                ConditionalFormattingObject obj3 = new ConditionalFormattingObject("Mi Condición3", ConditionTypes.Equal, "PICKING ALM1", "", false);
                obj3.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING ALM1");
                ConditionalFormattingObject obj4 = new ConditionalFormattingObject("Mi Condición4", ConditionTypes.Equal, "UBICACION PALET COMPLETO", "", false);
                obj4.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION PALET COMPLETO");
                ConditionalFormattingObject obj5 = new ConditionalFormattingObject("Mi Condición5", ConditionTypes.Equal, "TAREA HACIA LA PLAYA", "", false);
                obj5.CellBackColor = XmlReaderPropio.getColorTipoTareas("TAREA HACIA LA PLAYA");
                ConditionalFormattingObject obj6 = new ConditionalFormattingObject("Mi Condición6", ConditionTypes.Equal, "UBICACION CARRO MOVIL", "", false);
                obj6.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION CARRO MOVIL");
                ConditionalFormattingObject obj7 = new ConditionalFormattingObject("Mi Condición7", ConditionTypes.Equal, "TAREA DIRECTA A MUELLE", "", false);
                obj7.CellBackColor = XmlReaderPropio.getColorTipoTareas("TAREA DIRECTA A MUELLE");
                ConditionalFormattingObject obj8 = new ConditionalFormattingObject("Mi Condición8", ConditionTypes.Equal, "TAREA DE PLAYA A MUELLE", "", false);
                obj8.CellBackColor = XmlReaderPropio.getColorTipoTareas("TAREA DE PLAYA A MUELLE");
                ConditionalFormattingObject obj9 = new ConditionalFormattingObject("Mi Condición9", ConditionTypes.Equal, "ACOPIO ALM1", "", false);
                obj9.CellBackColor = XmlReaderPropio.getColorTipoTareas("ACOPIO ALM1");
                ConditionalFormattingObject obj10 = new ConditionalFormattingObject("Mi Condición10", ConditionTypes.Equal, "REPOSICION", "", false);
                obj10.CellBackColor = XmlReaderPropio.getColorTipoTareas("REPOSICION");
                ConditionalFormattingObject obj11 = new ConditionalFormattingObject("Mi Condición11", ConditionTypes.Equal, "DESGLOSE DE CARRO MOVIL", "", false);
                obj11.CellBackColor = XmlReaderPropio.getColorTipoTareas("DESGLOSE DE CARRO MOVIL");
                ConditionalFormattingObject obj12 = new ConditionalFormattingObject("Mi Condición12", ConditionTypes.Equal, "ACOPIO ALM2", "", false);
                obj12.CellBackColor = XmlReaderPropio.getColorTipoTareas("ACOPIO ALM2");
                ConditionalFormattingObject obj13 = new ConditionalFormattingObject("Mi Condición13", ConditionTypes.Equal, "PICKING ALM2", "", false);
                obj13.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING ALM2");
                ConditionalFormattingObject obj15 = new ConditionalFormattingObject("Mi Condición15", ConditionTypes.Equal, "PICKING SAN JUAN", "", false);
                obj15.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING SAN JUAN");
                ConditionalFormattingObject obj16 = new ConditionalFormattingObject("Mi Condición16", ConditionTypes.Equal, "PICKING VALLONGA", "", false);
                obj16.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING VALLONGA");
                ConditionalFormattingObject obj17 = new ConditionalFormattingObject("Mi Condición17", ConditionTypes.Equal, "ACOPIO SAN JUAN", "", false);
                obj17.CellBackColor = XmlReaderPropio.getColorTipoTareas("ACOPIO SAN JUAN");
                ConditionalFormattingObject obj18 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION SAN JUAN", "", false);
                obj18.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION SAN JUAN");
                ConditionalFormattingObject obj19 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING NAVE", "", false);
                obj19.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING NAVE");
                ConditionalFormattingObject obj20 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION PALET", "", false);
                obj20.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION PALET");
                ConditionalFormattingObject obj21 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "ACOPIO", "", false);
                obj21.CellBackColor = XmlReaderPropio.getColorTipoTareas("ACOPIO");
                ConditionalFormattingObject obj22 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "OLA PICKING", "", false);
                obj22.CellBackColor = XmlReaderPropio.getColorTipoTareas("OLA PICKING");
                ConditionalFormattingObject obj23 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION ACOPIO", "", false);
                obj23.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION ACOPIO");
                ConditionalFormattingObject obj24 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING MODULA", "", false);
                obj24.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING MODULA");
                ConditionalFormattingObject obj25 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "OL PICKING MODULA", "", false);
                obj25.CellBackColor = XmlReaderPropio.getColorTipoTareas("OL PICKING MODULA");
                ConditionalFormattingObject obj26 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION REPOSICIONES CARRO", "", false);
                obj26.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION REPOSICIONES CARRO");
                ConditionalFormattingObject obj27 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "ALTA MANUAL", "", false);
                obj27.CellBackColor = XmlReaderPropio.getColorTipoTareas("ALTA MANUAL");
                ConditionalFormattingObject obj28 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "DESGLOSE MULTIREFERENCIA", "", false);
                obj28.CellBackColor = XmlReaderPropio.getColorTipoTareas("DESGLOSE MULTIREFERENCIA");
                ConditionalFormattingObject obj29 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING", "", false);
                obj29.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING");
                ConditionalFormattingObject obj30 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING RESERVA", "", false);
                obj30.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING RESERVA");
                ConditionalFormattingObject obj31 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "ABONO PACKING LIST", "", false);
                obj31.CellBackColor = XmlReaderPropio.getColorTipoTareas("ABONO PACKING LIST");
                ConditionalFormattingObject obj32 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING EXTERIOR", "", false);
                obj32.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING EXTERIOR");
                ConditionalFormattingObject obj33 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING LARGOS", "", false);
                obj33.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING LARGOS");
                ConditionalFormattingObject obj34 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "REPOSICION ESTANTERIA APILABLE", "", false);
                obj34.CellBackColor = XmlReaderPropio.getColorTipoTareas("REPOSICION ESTANTERIA APILABLE");
                ConditionalFormattingObject obj35 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "REPOSICION EXTERIOR", "", false);
                obj35.CellBackColor = XmlReaderPropio.getColorTipoTareas("REPOSICION EXTERIOR");
                ConditionalFormattingObject obj36 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "REPOSICION RESERVA", "", false);
                obj36.CellBackColor = XmlReaderPropio.getColorTipoTareas("REPOSICION RESERVA");
                ConditionalFormattingObject obj37 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION EXTERIOR", "", false);
                obj37.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION EXTERIOR");
                ConditionalFormattingObject obj38 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION PALET RESERVA", "", false);
                obj38.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION PALET RESERVA");
                ConditionalFormattingObject obj39 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION PICKING", "", false);
                obj39.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION PICKING");
                ConditionalFormattingObject obj40 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION ZCALIENTE", "", false);
                obj40.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION ZCALIENTE");
                ConditionalFormattingObject obj41 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING 1-0", "", false);
                obj41.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING 1-0");
                ConditionalFormattingObject obj42 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING 1-1", "", false);
                obj42.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING 1-1");
                ConditionalFormattingObject obj43 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION PLANTA 1-0", "", false);
                obj43.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION PLANTA 1-0");
                ConditionalFormattingObject obj44 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION PLANTA 1-1", "", false);
                obj44.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION PLANTA 1-1");
                ConditionalFormattingObject obj45 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "RECEPCION ADMINISTRATIVA", "", false);
                obj45.CellBackColor = XmlReaderPropio.getColorTipoTareas("RECEPCION ADMINISTRATIVA");
                ConditionalFormattingObject obj46 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "PICKING TERMOS", "", false);
                obj46.CellBackColor = XmlReaderPropio.getColorTipoTareas("PICKING TERMOS");
                ConditionalFormattingObject obj47 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "REPOSICION TERMOS", "", false);
                obj47.CellBackColor = XmlReaderPropio.getColorTipoTareas("REPOSICION TERMOS");
                ConditionalFormattingObject obj48 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "REPOSICION UDS SUELTAS", "", false);
                obj48.CellBackColor = XmlReaderPropio.getColorTipoTareas("REPOSICION UDS SUELTAS");
                ConditionalFormattingObject obj49 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "ACOPIO ACONDICIONAMIENTO", "", false);
                obj49.CellBackColor = XmlReaderPropio.getColorTipoTareas("ACOPIO ACONDICIONAMIENTO");
                ConditionalFormattingObject obj50 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "ACOPIO MP", "", false);
                obj50.CellBackColor = XmlReaderPropio.getColorTipoTareas("ACOPIO MP");
                ConditionalFormattingObject obj51 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION ACONDICIONAMIENTO", "", false);
                obj51.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION ACONDICIONAMIENTO");
                ConditionalFormattingObject obj52 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION MATERIA PRIMA", "", false);
                obj52.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION MATERIA PRIMA");
                ConditionalFormattingObject obj53 = new ConditionalFormattingObject("Mi Condición18", ConditionTypes.Equal, "UBICACION PRODUCTO TERMINADO", "", false);
                obj53.CellBackColor = XmlReaderPropio.getColorTipoTareas("UBICACION PRODUCTO TERMINADO");

                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj1);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj2);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj3);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj4);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj5);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj6);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj7);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj8);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj9);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj10);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj11);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj12);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj13);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj15);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj16);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj17);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj18);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj19);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj20);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj21);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj22);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj23);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj24);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj25);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj26);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj27);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj28);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj29);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj30);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj31);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj32);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj33);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj34);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj35);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj36);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj37);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj38);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj39);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj40);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj41);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj42);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj43);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj44);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj45);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj46);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj47);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj48);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj49);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj50);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj51);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj52);
                this.GridView.Columns["DESCRIPCION"].ConditionalFormattingObjectList.Add(obj53);

            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

        public override void ElegirEstilo()
        {
            base.ElegirEstilo();
            ColorTipoTarea();
        }
        #endregion

        #region Funciones Auxiliares

        //Eventos Asíncronos y Síncronos
        private void InicializarEventos()
        {
            //Indicamos que las llamadas, para la recogida de datos, serán Asíncronas            
            this.CallbackCantidad = new Action<int>(query =>
            {
                this.virtualGridControl.MasterViewInfo.IsWaiting = false;
                this.virtualGridControl.TableElement.SynchronizeRows();
            });

            this.Callback = new Action<List<dynamic>>(query =>
            {
                this.data = query;
                this.virtualGridControl.MasterViewInfo.IsWaiting = false;
                this.virtualGridControl.TableElement.SynchronizeRows();
                this.virtualGridControl.BestFitColumns();
            });

            //Declaración Eventos 
            this.virtualGridControl.SelectionChanged += GridControl_SelectionChanged;
            this.GridView.FilterChanged += gridViewFilterChanged_Event;
        }

        //Prepara el grid con la cantidad total de filas y columnas a pintar luego por el evento RadVirtualGrid_CellValueNeeded
        private void InicializarFilasColumnasGrid()
        {
            this.virtualGridControl.ColumnCount = _lstEsquemaTabla.Count;
            this.virtualGridControl.RowCount = CantidadRegistros + 1;
        }

        //Obtención de cantidad total de registros
        private void ObtencionTotalRegistros()
        {
            //Obtenemos, asíncronamente, la cantidad total de registros de Agencias, así como su esquema de datos, para poder preparar el Grid                        
            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            CantidadRegistros = Business.GetTareasTipoCantidad(ref _lstEsquemaTabla);
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + CantidadRegistros;

        }
        #endregion

        #region Eventos Propios               
        public override void btnCambiarVista_Click(object sender, EventArgs e)
        {


            if (TblLayout.GetControlFromPosition(0, 0) is RadGridView)
            {
                TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 0));
                TblLayout.Controls.Add(virtualGridControl, 0, 0);
                TblLayout.SetColumnSpan(virtualGridControl, 10);
                virtualGridControl.Dock = DockStyle.Fill;
                ElegirEstilo();
            }
            else if (TblLayout.GetControlFromPosition(0, 0) is RadVirtualGrid)
            {

                if (numRegistrosFiltrados > 100000)
                {
                    MessageBox.Show(Lenguaje.traduce(strings.ExcesoRegistros));
                }
                else
                {
                    if (numRegistrosFiltrados > 50000)
                    {
                        MessageBox.Show(Lenguaje.traduce("Aviso:" + strings.AvisoRegistros));
                    }
                    if (GridView.DataSource == null)
                    {
                        llenarGrid();
                    }
                    TblLayout.Controls.Remove(TblLayout.GetControlFromPosition(0, 0));
                    TblLayout.Controls.Add(GridView, 0, 0);
                    TblLayout.SetColumnSpan(GridView, 10);
                    GridView.Dock = DockStyle.Fill;
                    ElegirEstilo();

                }
            }

            else
            {
                MessageBox.Show(Lenguaje.traduce("Error inesperado"));
            }
        }
        //Selección de un registro del grid
        private void GridControl_SelectionChanged(object sender, EventArgs e)
        {
            _selectedRow = null;

            if (this.virtualGridControl.CurrentCell != null && this.virtualGridControl.CurrentCell.RowIndex >= 0)
            {

                if ((virtualGridControl.CurrentCell.RowIndex % virtualGridControl.PageSize) >= data.Count)
                {
                    return;
                }

                int index = this.virtualGridControl.CurrentCell.RowIndex - this.virtualGridControl.PageSize * this.virtualGridControl.PageIndex;
                var item = data[index];

                _selectedRow = item; //Fila seleccionada, en formato JSON
            }
        }

        #endregion

        #region Eventos Sobreescritos de la clase padre BaseGridControl 
        protected override void ElegirGrid()
        {
            if (CantidadRegistros > 10000)
            {
                ObtencionTotalRegistros();

                TblLayout.Controls.Add(virtualGridControl, 0, 0);

            }
            else
            {
                llenarGrid();
                TblLayout.Controls.Add(GridView, 0, 0);
                TblLayout.SetColumnSpan(GridView, 9);
                GridView.Dock = System.Windows.Forms.DockStyle.Fill;
                GridView.BestFitColumns();

            }
        }
        protected override void getGridViewRow()
        {
            for (int i = 0; i < data.Count; i++)
            {
                JObject objetoJson = JObject.Parse(data[i].ToString());

                string jsonID = (string)objetoJson[_lstEsquemaTabla[0].Nombre];
                if (this.GridView.CurrentRow.Cells[_lstEsquemaTabla[0].Nombre].Value.ToString() == jsonID)
                {
                    _selectedRow = data[i];
                    break;
                }
            }
        }
        public override void llenarGrid()
        {
            if (_lstEsquemaTabla.Count != 0)
            {
                try
                {
                    base.llenarGrid();
                    Business.GetTareasTipoCantidad(ref _lstEsquemaTabla);
                    GridView.DataSource = Business.GetTareasTipoDatosGridView(_lstEsquemaTabla);
                    foreach (var column in GridView.Columns)
                    {
                            column.Name = _lstEsquemaTabla[column.Index].Nombre;
                            column.FieldName = _lstEsquemaTabla[column.Index].Etiqueta;
                    }
                }
                catch (Exception e)
                {
                    log.Error("Mensaje:" + e.Message + "\n StackTrace:" + e.StackTrace);
                    MessageBox.Show(Lenguaje.traduce(strings.ErrorLlenarGrid));
                }
            }
        }
        //Este evento sólo se producirá la primera vez que cargue el número total de registros de BBDD
        protected override void TbCantidadRegistros_TextChanged(object sender, EventArgs e)
        {
            string cantidad = ((RadTextBox)sender).Text;
            if (!string.IsNullOrEmpty(cantidad) && (int.Parse(cantidad)) > 0)
            {
                CantidadRegistros = int.Parse(cantidad);
                //Cargamos la primera página del grid
                this.RefreshData(0);
            }
        }

        //Función que, asíncronamente, consulta los datos de Agencias de la página seleccionada en el Grid
        public override void RefreshData(int pageIndex)
        {
            //Actualizamos el número de filas totales que tendrá el grid con la variable CantidadRegistros heredada y que puede haber sido modificada al crear, clonar y eliminar registros
            InicializarFilasColumnasGrid();

            //Limpiamos datos
            data.Clear();

            int pagInicial = 0;
            int pagFinal = 0;
            pageIndex++;

            if (pageIndex == 1)
            {
                pagInicial = 1;
                pagFinal = _K_PAGINACION;
            }
            else
            {
                pagInicial = _K_PAGINACION * (pageIndex - 1) + 1;
                pagFinal = (_K_PAGINACION * pageIndex);
            }

            string sortExpression = "";
            string filterExpression = this.virtualGridControl.FilterDescriptors.Expression;
            foreach (var item in this.virtualGridControl.SortDescriptors)
            {
                sortExpression += "TAREASTIPO." + item.PropertyName;
                if (item.Direction.ToString() == "Ascending")
                {
                    sortExpression += " ASC,";
                }
                else
                {
                    sortExpression += " DESC,";
                }
            }
            if (sortExpression != string.Empty)
            {
                sortExpression = sortExpression.TrimEnd(',');

            }
            if (sortExpression == string.Empty)
            {
                sortExpression = "IDTAREATIPO ASC";
            }
            if (filterExpression != null&&filterExpression!=string.Empty)
            {
                filterExpression = " WHERE " + filterExpression;
                numRegistrosFiltrados = Business.GetTareasTipoRegistrosFiltrados(filterExpression);
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;

            }
            else
            {
                numRegistrosFiltrados = Business.GetTareasTipoRegistrosFiltrados("");
                lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + numRegistrosFiltrados;
            }


            this.virtualGridControl.MasterViewInfo.IsWaiting = true;
            ExecuteQueryAsync<List<dynamic>>(Task.Run(() => Business.GetTareasTipoDatos(sortExpression, filterExpression, pagInicial, pagFinal, _lstEsquemaTabla)), this.Callback);
            ElegirEstilo();

        }

        //Evento que pinta, celda a celda, los datos del Grid
        protected override void RadVirtualGrid_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
        {
            base.RadVirtualGrid_CellValueNeeded(sender, e);

            if (e.ColumnIndex < 0)
                return;
            if (e.RowIndex < 0)
            {
                e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
            }
                if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta == null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                }
                else if (e.RowIndex == RadVirtualGrid.HeaderRowIndex && _lstEsquemaTabla[e.ColumnIndex].Etiqueta != null)
                {
                    e.Value = this._lstEsquemaTabla[e.ColumnIndex].Etiqueta;
                    e.FieldName = this._lstEsquemaTabla[e.ColumnIndex].Nombre;
                }


            if (e.RowIndex >= 0 && data.Count > 0)
            {
                if ((e.RowIndex % virtualGridControl.PageSize) >= data.Count)
                {
                    return;
                }

                try
                {
                    int index = e.RowIndex - this.virtualGridControl.PageSize * this.virtualGridControl.PageIndex;

                    var z = Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(this.data[index]));

                    var pn = (string)z[this._lstEsquemaTabla[e.ColumnIndex].Nombre];

                    if (pn != null)
                    {
                        e.Value = pn.ToString();
                    }
                    else
                    {
                        e.Value = string.Empty;
                    }
                }
                catch (Exception exc)
                {
                    string ex = exc.Message;
                }
            }
        }
        private void gridViewFilterChanged_Event(object sender, EventArgs e)
        {
            if (GridView.FilterDescriptors.Expression != null && GridView.FilterDescriptors.Expression != string.Empty)
            {
            lblCantidad.Text = Lenguaje.traduce(strings.NProductos) + ":" + Business.GetTareasTipoRegistrosFiltrados(" WHERE " + GridView.FilterDescriptors.Expression).ToString();

            }
        }
        //Acción para crear una TareaTipo
        protected override AckResponse NewData(dynamic selectedRow)
        {
            return Business.AltaTareaTipo(_lstEsquemaTabla, selectedRow);
        }

        //Acción para editar una TareaTipo
        protected override AckResponse EditData(dynamic selectedRow)
        {
            return Business.EditarTareaTipo(_lstEsquemaTabla, selectedRow);
        }

        //Acción para eliminar una TareaTipo
        protected override AckResponse DeleteData(dynamic selectedRow)
        {
            return Business.EliminarTareaTipo(_lstEsquemaTabla, selectedRow);
        }

        #endregion
    }
}
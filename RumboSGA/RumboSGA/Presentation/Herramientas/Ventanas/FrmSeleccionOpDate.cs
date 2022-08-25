using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FrmSeleccionOpDate : Telerik.WinControls.UI.RadForm
    {
        public DateTime FechaDesde = new DateTime();
        public DateTime FechaHasta = new DateTime();
        public List<int> operarios=new List<int>();
        DataTable tRec = new DataTable();
        //public List<string> tipoMovimiento= new List<string>();
        /*private static String[] camposAlmacen = { "IDRESERVATIPO", "DESCRIPCION" };
        private static string campoFormAlmacen = "TBLRESERVASTIPO";
        private static string valueMemberAlmacen = "IDRESERVATIPO";
        private static string displayMemberAlmacen = "DESCRIPCION";*/
        private static String[] camposOperario = { "IDOPERARIO", "NOMBRE" };
        private static string campoOperario = "TBLOPERARIOS";
        private static string valueMemberOperario = "IDOPERARIO";
        private static string displayMemberOperario = "NOMBRE";
        RadComboBoxSelectionExtender extenderOperarios = new RadComboBoxSelectionExtender();
        RadComboBoxSelectionExtender extenderTipoMov = new RadComboBoxSelectionExtender();
        private RadContextMenu contextMenu;

        public FrmSeleccionOpDate(DateTime fechaDesde, DateTime fechaLimite)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce(this.Text);
            FechaDesde = fechaDesde;
            rumDateTimePickerDesde.Value = fechaDesde;
            rumDateTimePickerDesde.MaxDate = DateTime.Now;
            

            rumDateTimePickerHasta.MinDate = fechaDesde;
            rumDateTimePickerHasta.MaxDate = DateTime.Now;
            FechaHasta = fechaDesde.AddDays(1);
            rumDateTimePickerHasta.Value = FechaHasta;
            Utilidades.RellenarMultiColumnComboBox(ref radComboBoxOperarioDesde, DataAccess.GetIdDescripcionOperariosOrder(" ORDER BY IDOPERARIO"), "IDOPERARIO", "NOMBRE", "", new String[] { "TODOS" }, true);
            extenderOperarios.AssociatedRadMultiColumnComboBox = this.radComboBoxOperarioDesde;
            radComboBoxOperarioDesde.EditorControl.ContextMenuOpening += radComboBoxOperarioDesde_ContextMenuOpening;
            radComboBoxOperarioDesde.AutoSizeDropDownToBestFit= true;
            radComboBoxOperarioDesde.AutoSize = true;
            radComboBoxOperarioDesde.MaximumSize = new Size(339,60);
            this.radComboBoxOperarioDesde.EditorControl.MinimumSize = new System.Drawing.Size(240, 150);


            //seleccionarTodos();
            //extenderTipoMov.AssociatedRadMultiColumnComboBox = this.TipoMovComboBox;
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            //tipoMovimiento = TipoMovComboBox.SelectedValue.ToString();
            //int[] operarios = new int[extenderOperarios.Items.Count];
            for (int i = 0; i < extenderOperarios.Items.Count; i++)
            {
                DataTable trac = DataAccess.getDataTableSQL(camposOperario, campoOperario, " Nombre= '" + extenderOperarios.Items[i].Text.Trim() + "'");
                int idoperario = trac.Rows[0].Field<int>("IDOPERARIO");
                operarios.Add(idoperario);
            }
            
            DialogResult = DialogResult.OK;
            
            //operarios = Convert.ToInt32(radComboBoxOperarioDesde.SelectedValue);
            Close();
        }
        private void cargarSeleccionarTodos()
        {
            contextMenu = new RadContextMenu();
            RadMenuItem seleccionarTodos = new RadMenuItem("SeleccionarTodos");
            seleccionarTodos.Text = "Seleccionar Todos";
            seleccionarTodos.Click += seleccionarTodos_Click;
            contextMenu.Items.Add(seleccionarTodos);
            //seleccionarTodos.Click+= new EventHandltemplaer(extenderOperarios.selectall());
        }
        private void cargarDeseleccionarTodos()
        {
            contextMenu = new RadContextMenu();
            RadMenuItem DeseleccionarTodos = new RadMenuItem("DeseleccionarTodos");
            DeseleccionarTodos.Text = "Deseleccionar Todos";
            DeseleccionarTodos.Click += DeseleccionarTodos_Click;
            contextMenu.Items.Add(DeseleccionarTodos);
            //seleccionarTodos.Click+= new EventHandltemplaer(extenderOperarios.selectall());
        }
        private void DeseleccionarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRowInfo row in radComboBoxOperarioDesde.EditorControl.Rows)
                {

                    row.Tag = Boolean.FalseString;
                    //string value = row.Cells[1].Value.ToString().Trim();
                    radComboBoxOperarioDesde.Text = "";

                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex);
            }
            extenderOperarios.SyncCollection();
        }
        private void seleccionarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRowInfo row in radComboBoxOperarioDesde.EditorControl.Rows)
                {

                    row.Tag = Boolean.TrueString;
                    //string value = row.Cells[1].Value.ToString().Trim();
                    if (row.Tag != null && row.Tag.ToString() == Boolean.TrueString)
                    {
                        radComboBoxOperarioDesde.Text += row.Cells[1].Value + "; ";
                    }

                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex);
            }
            extenderOperarios.SyncCollection();
        }
        void radComboBoxOperarioDesde_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if (radComboBoxOperarioDesde.Text != "")
            {
                cargarDeseleccionarTodos();
            }
            else
            {
                cargarSeleccionarTodos();
            }
            e.ContextMenu = contextMenu.DropDown;
        }
        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void RumDateTimePickerDesde_ValueChanged(object sender, EventArgs e)
        {
            
            FechaDesde = rumDateTimePickerDesde.Value;
            rumDateTimePickerHasta.MinDate = FechaDesde.AddDays(1);
        }

        private void RumDateTimePickerHasta_ValueChanged(object sender, EventArgs e)
        {
            FechaHasta = rumDateTimePickerHasta.Value;
        }
    }
}

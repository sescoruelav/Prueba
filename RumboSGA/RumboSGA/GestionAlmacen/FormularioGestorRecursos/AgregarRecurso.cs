using Rumbo.Core.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen.FormularioGestorRecursos
{
    public partial class AgregarRecurso : Form
    {
        DataTable DtRecursos = new DataTable();
        string idtarea = "";
        public AgregarRecurso(string idTarea)
        {
            InitializeComponent();
            RellenarDataTableRecursos();
            idtarea = idTarea;
            this.Text = Lenguaje.traduce("Agregar Recurso");
        }
        public void RellenarDataTableRecursos()
        {
            try
            {
                /*GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
                checkBoxColumn.DataType = typeof(bool);
                checkBoxColumn.Name = "DiscontinuedColumn";
                checkBoxColumn.FieldName = "Discontinued";
                checkBoxColumn.HeaderText = "Discontinued?";
                checkBoxColumn.EditMode = EditMode.OnValueChange;
                radGridViewRecursos.MasterTemplate.Columns.Add(checkBoxColumn);*/
                string sql = "select distinct  r.IDRECURSO as '"+Lenguaje.traduce("Número de Recurso")+ "', r.DESCRIPCION as " + Lenguaje.traduce("Descripción") + ", r.ESTADO as " + Lenguaje.traduce("Estado") + " from TBLRECURSOS r ";
                DtRecursos = ConexionSQL.getDataTable(sql);
                radGridViewRecursos.DataSource = DtRecursos;
                configurarGridView(radGridViewRecursos);
                
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }
        public void configurarGridView(RadGridView gridView)
        {
            try
            {
                gridView.HorizontalScrollState = ScrollState.AutoHide;
                gridView.HorizontalScroll.Enabled = true;
                gridView.VerticalScrollState = ScrollState.AutoHide;
                gridView.VerticalScroll.Enabled = true;
                gridView.AllowAddNewRow = false;
                gridView.AllowDeleteRow = false;
                gridView.AllowDragToGroup = false;
                gridView.MultiSelect = true;
                gridView.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.FullRowSelect;
                gridView.AllowEditRow = false;
                for (int i = 0; i < DtRecursos.Columns.Count; i++)
                {
                    if (radGridViewRecursos.Columns[i].Name.Contains("ID"))
                    {
                        radGridViewRecursos.Columns[i].IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void radBtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radBtnAceptar_Click(object sender, EventArgs e)
        {
            string error = "";
            int contador = 0;
            try
            {
                if (radGridViewRecursos.SelectedRows.Count >= 1)
                {
                    for (int i = 0; i < radGridViewRecursos.SelectedRows.Count; i++)
                    {
                        string idRecurso= radGridViewRecursos.SelectedRows[i].Cells[0].Value.ToString();
                        string sql = "INSERT INTO TBLRECURSOSTAREA (  IDRECURSO, IDTAREATIPO,PRIORIDAD,DURSEGUNDOS,ACTIVA ) VALUES ( "+ idRecurso + ","+idtarea+",3,0,0 ) ";
                        bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                        if (data)
                        {
                            contador++;
                        }
                    }
                    if (error == "")
                    {
                        error = Lenguaje.traduce("Se han generado " + contador + " campos");
                    }
                    else{
                        if (contador == 1)
                        {
                            error = error + Lenguaje.traduce( ". Se ha generado " + contador + " campo no duplicado");
                        }
                        else
                        {
                            error = error + Lenguaje.traduce(". Se han generado " + contador + " campos no duplicados");
                        }
                    }
                    MessageBox.Show(error, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha seleccionado ningúna fila"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }

        private void radGridViewRecursos_ValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}

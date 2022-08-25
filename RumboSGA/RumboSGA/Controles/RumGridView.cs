using Newtonsoft.Json;
using RumboSGAManager;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using static RumboSGA.FuncionesGenerales;

namespace RumboSGA.Controles
{
    public class RumGridView : RadGridView
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RumGridView() : base()
        {
            EnablePaging = false;
            this.MasterTemplate.ShowHeaderCellButtons = true;
            this.MasterTemplate.ShowFilteringRow = false;
        }

        public override string ThemeClassName
        {
            get
            {
                return typeof(RadGridView).FullName;
            }
        }

        public void CambiarRegistroFila(List<TableScheme> lstEsquemaTabla, dynamic row, enumLineaOpciones opcion, string nombreJson)
        {
            try
            {
                //TODO: Mejorar este metodo para trasladarlo a otros grids. Problema actual, saber cual es el registro que se ha añadido en la sql
                if (!(DataSource is DataTable))
                {
                    log.Error("No se ha podido cambiar el registro fila porque no es DataTable el DataSource");
                }
                DataTable dt = (DataSource as DataTable);
                switch (opcion)
                {
                    case enumLineaOpciones.edit:
                        FuncionesGenerales.DataTableLineaEdit(ref dt, row, lstEsquemaTabla, nombreJson);
                        break;

                    case enumLineaOpciones.add:
                        FuncionesGenerales.DataTableLineaAdd(ref dt, row, lstEsquemaTabla, nombreJson, "");
                        break;

                    case enumLineaOpciones.delete:
                        FuncionesGenerales.DataTableLineaDelete(ref dt, row, lstEsquemaTabla, nombreJson);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en CambiarRegistroFila de RumGridView");
            }
        }

        public void limpiarFiltros()
        {
            try
            {
                if (IsInEditMode)
                {
                    EndEdit();
                }
                FilterDescriptors.Clear();
                GroupDescriptors.Clear();
                SortDescriptors.Clear();
                BestFitColumns();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en limpiarFiltros de RumGridView");
            }
        }

        public dynamic getSelectedRow(List<TableScheme> lstEsquemaTabla)
        {
            int lineaSeleccionada = 0;
            try
            {
                if (SelectedCells.Count == 0 && CurrentCell != null && CurrentCell.RowIndex < 0)
                    return null;
                if (SelectedRows.Count < 1)
                {
                    if (SelectedCells.Count < 1) return null;
                    else
                    {
                        lineaSeleccionada = Rows.IndexOf(SelectedCells[0].RowInfo);
                    }
                }
                else
                {
                    lineaSeleccionada = Rows.IndexOf(SelectedRows[0]);
                }

                Dictionary<dynamic, dynamic> fila = new Dictionary<dynamic, dynamic>();
                for (int i = 0; i < ColumnCount; i++)
                {
                    if (lstEsquemaTabla != null)
                    {
                        string nombre = Columns[i].Name;
                        foreach (TableScheme item in lstEsquemaTabla)
                        {
                            if (item.Etiqueta == Columns[i].Name)
                            {
                                nombre = item.Nombre;
                                break;
                            }
                        }
                        fila.Add(nombre, Rows[lineaSeleccionada].Cells[i].Value);
                    }
                    else
                    {
                        fila.Add(Columns[i].Name, Rows[lineaSeleccionada].Cells[i].Value);
                    }
                }
                string jsonLinea = JsonConvert.SerializeObject(fila);

                return jsonLinea;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, "Error en getSelectedRow de RumGridView");
            }
            return null;
        }

        public void AñadirColumnaRadioButton()
        {
            this.TableElement.BeginUpdate();

            RadioButtonColumn column = new RadioButtonColumn("Sel")
            {
                HeaderText = "Sel",
                Width = 170,
                ReadOnly = false
            };

            this.Columns.Add(column);
            this.Columns.Move(this.ColumnCount - 1, 0);
            this.TableElement.EndUpdate();
        }
    }
}
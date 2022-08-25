using Newtonsoft.Json;
using RumboSGA.ArticulosMotor;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Presentation.Herramientas.Stock;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using static RumboSGA.FuncionesGenerales;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class VisorSQLRibbonUbicaciones : VisorSQLRibbon
    {
        private RumRibbonBarGroup ribbonBarGroupUbicacion;
        private RadButtonElement buttonModificacionMasivaUbicacion;

        public VisorSQLRibbonUbicaciones(String name, String nombreJson) : base(name, nombreJson, true, false, "", "TBLHUECOS", "IDHUECO")
        {
            cargarVistas(Estados.VCHibrido);
            rumPageView = new RumPageView(nombreJson, lstEsquemaTabla, this.tablaPrincipal.Width, false, false);
            this.tablaPrincipal.Controls.Add(rumPageView, 0, 1);
            modoEdicionGrid(false);
        }

        protected override void cargarOpcionesRibbonBar()
        {
            base.cargarOpcionesRibbonBar();

            buttonModificacionMasivaUbicacion = new RadButtonElement();
            buttonModificacionMasivaUbicacion.Text = "Modificación Ubicaciones Masiva";
            buttonModificacionMasivaUbicacion.Image = Properties.Resources.EditList;
            buttonModificacionMasivaUbicacion.TextAlignment = ContentAlignment.BottomCenter;
            buttonModificacionMasivaUbicacion.ImageAlignment = ContentAlignment.MiddleCenter;
            buttonModificacionMasivaUbicacion.Click += ButtonModificacionMasivaUbicacion_Click;

            ribbonBarGroupUbicacion = new RumRibbonBarGroup();
            ribbonBarGroupUbicacion.Text = "Ubicación";
            rtAcciones.Items.Insert(0, ribbonBarGroupUbicacion);

            RumDropDownButtonElement rbCambiarVistasUbicacion = new RumDropDownButtonElement();
            rbCambiarVistasUbicacion.Text = "Vistas";
            rbCambiarVistasUbicacion.TextAlignment = ContentAlignment.BottomCenter;
            rbCambiarVistasUbicacion.ImageAlignment = ContentAlignment.MiddleCenter;
            rbCambiarVistasUbicacion.Image = Properties.Resources.down_chevron16;

            RumMenuItem vistaHibrida = new RumMenuItem();
            vistaHibrida.Text = "Vista Hibrida";
            vistaHibrida.Click += (sender, e) =>
            {
                cargarVistas(Estados.VCHibrido);
            };

            RumMenuItem vistaGrid = new RumMenuItem();
            vistaGrid.Text = "Vista Grid";
            vistaGrid.Click += (sender, e) =>
            {
                cargarVistas(Estados.VCTodoGrid);
            };

            RumMenuItem vistaDetalle = new RumMenuItem();
            vistaDetalle.Text = "Vista Detalle";
            vistaDetalle.Click += (sender, e) =>
            {
                cargarVistas(Estados.VCTodoDetalle);
            };
            rbCambiarVistasUbicacion.Items.Add(vistaHibrida);
            rbCambiarVistasUbicacion.Items.Add(vistaGrid);
            rbCambiarVistasUbicacion.Items.Add(vistaDetalle);
            grupoVer.Items.Add(rbCambiarVistasUbicacion);

            List<NameValue> listaNameValue = new List<NameValue>();
            lstEsquemaTabla = FuncionesGenerales.CargarEsquema(this.nombreJson);
            FuncionesGenerales.RumDropDownAddManual(ref rbConfiguracion, 20038);
        }

        private void modoEdicionGrid(bool activar)
        {
            try
            {
                if (activar)
                {
                    rumPageView.Editable(true);
                    ribbonBarGroupUbicacion.Items.Clear();
                    ribbonBarGroupUbicacion.Items.Add(buttonModificacionMasivaUbicacion);
                }
                else
                {
                    rumPageView.Editable(false);
                    ribbonBarGroupUbicacion.Items.Clear();
                    ribbonBarGroupUbicacion.Items.Add(buttonModificacionMasivaUbicacion);
                }
                foreach (RadItem item in rtAcciones.Items)
                {
                    if (activar)
                    {
                        if (item != ribbonBarGroupUbicacion)
                            item.Visibility = ElementVisibility.Hidden;
                    }
                    else
                    {
                        item.Visibility = ElementVisibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void ButtonModificacionMasivaUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("Modificación masiva abierta");
                List<TableScheme> lstEsquemaTablaMasivo = new List<TableScheme>();
                for (int i = 0; i < lstEsquemaTabla.Count; i++)
                {
                    if (lstEsquemaTabla[i].PuedeMasivo)
                    {
                        lstEsquemaTablaMasivo.Add(lstEsquemaTabla[i]);
                    }
                }
                if (lstEsquemaTablaMasivo.Count == 0)
                {
                    RadMessageBox.Show(Lenguaje.traduce("No hay campos marcados para la configuración masiva."));
                    log.Error("Problema en la modificación masiva, no hay campos en el Json con PuedeMasivo=true");
                    return;
                }
                SFConfiguradorUbicacion configuradorUbicacion = new SFConfiguradorUbicacion(name, lstEsquemaTablaMasivo, selectedRow, SFConfigurarArticulos.modoForm.edicion, esquemGrid, diccParamNuevo);
                configuradorUbicacion.ShowDialog();

                if (configuradorUbicacion.DialogResult != DialogResult.OK) return;
                Telerik.WinControls.Data.FilterDescriptorCollection filtrosAnteriores;

                int numeroUbic = 0;
                if (this.panel.Contains(gridView))
                {
                    filtrosAnteriores = gridView.FilterDescriptors;
                    numeroUbic = gridView.ChildRows.Count;
                }
                else if (this.panel.Contains(virtualGrid))
                {
                    filtrosAnteriores = virtualGrid.FilterDescriptors;
                    numeroUbic = dtFiltradoVirtual.Rows.Count;
                }
                else
                {
                    log.Error("Problema en la modificación masiva, no hay ni GridView ni VirtualGrid en pantalla");
                    return;
                }
                if (numeroUbic == 0)
                {
                    RadMessageBox.Show(Lenguaje.traduce("No hay Ubicaciones para modificar."));
                    log.Error("Problema en la modificación masiva, no hay cantidad");
                    return;
                }
                String mensaje = Lenguaje.traduce("Atención, estas apunto de hacer una modificación masiva de " + numeroUbic + " registros con los filtros " + filtrosAnteriores.Expression);
                if (RadMessageBox.Show(this, mensaje, Lenguaje.traduce("Confirmación"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    log.Debug("Modificación masiva cancelada");
                    RadMessageBox.Show(Lenguaje.traduce("Cancelando!"));
                    return;
                }
                string filtrosEnSQL = "";
                log.Debug("Se va actualizar Ubicaciones Masivas, filtros telerik: " + gridView.FilterDescriptors.Expression + "; Filtro where:" + filtrosEnSQL);

                if (this.panel.Contains(gridView))
                {
                    for (int i = 0; i < gridView.ChildRows.Count; i++)
                    {
                        filtrosEnSQL = " IDHUECO=" + gridView.ChildRows[i].Cells["ID ubicación"].Value;

                        AckResponse ack = Business.EditDataMasivo(configuradorUbicacion.newRecord, filtrosEnSQL, "TBLHUECOS");
                        //RadMessageBox.Show(ack.Mensaje);
                        if (ack != null && ack.Resultado == "OK")
                        {
                            log.Debug("Actualización realizada! ubicación: " + filtrosEnSQL);
                        }
                        else
                        {
                            log.Error("No se ha podido hacer el cambio masivo sobre la ubicación:" + filtrosEnSQL);
                        }
                    }
                }
                else if (this.panel.Contains(virtualGrid))
                {
                    for (int i = 0; i < dtFiltradoVirtual.Rows.Count; i++)
                    {
                        filtrosEnSQL = " IDHUECO=" + dtFiltradoVirtual.Rows[i]["ID ubicación"].ToString();
                        AckResponse ack = Business.EditDataMasivo(configuradorUbicacion.newRecord, filtrosEnSQL, "TBLHUECOS");
                        //RadMessageBox.Show(ack.Mensaje);
                        if (ack != null && ack.Resultado == "OK")
                        {
                            log.Debug("Actualización realizada! Ubicaciones: " + filtrosEnSQL);
                        }
                        else
                        {
                            RadMessageBox.Show(Lenguaje.traduce("Se ha añadido algún campo incorrecto, posiblemente una palabra en un campo que va un número"));
                            log.Error("No se ha podido hacer el cambio masivo sobre la ubicación:" + filtrosEnSQL);
                            break;
                        }
                    }
                }
                loadData();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        protected override void abrirSFDetallesEditar()
        {
            try
            {
                if (this.selectedRow == null)
                {
                    getGridViewRow();
                }
                if (selectedRow == null)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Selecciona una fila"));
                }
                // var dialog = new SFDetallesN(name, lstEsquemaTabla, selectedRow, SFDetallesN.modoForm.edicion, true);
                var dialog = new SFDetalle(name, lstEsquemaTabla, selectedRow, false, false, "TBLHUECOS");
                dialog.AutoSize = true;
                dialog.ComprobarValores = true;
                dialog.AutoSizeMode = AutoSizeMode.GrowOnly;
                dialog.Text = Lenguaje.traduce("Detalle");
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.editado == true)
                    {
                        AckResponse ack = EditData(dialog.newRecord);
                        RadMessageBox.Show(ack.Mensaje);
                        if (ack.Resultado == "OK")
                        {
                            loadData();
                            cargarLinea(enumLineaOpciones.edit, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        protected override void virtualGrid_CellClick(object sender, VirtualGridCellElementEventArgs e)
        {
            base.virtualGrid_CellClick(sender, e);
            seleccionarFilaRumPageView();
        }

        protected override void gridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            base.gridView_CellClick(sender, e);
            seleccionarFilaRumPageView();
        }

        private void seleccionarFilaRumPageView()
        {
            if (selectedRow == null) return;
            modoEdicionGrid(false);
            rumPageView.LoadSelectedRow(selectedRow);
        }
    }
}
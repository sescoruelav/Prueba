using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.Presentation.Herramientas.Stock;
using RumboSGA.ArticulosMotor;
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
using System.Globalization;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class VisorSQLRibbonArticulos : VisorSQLRibbon
    {
        private RadButtonElement buttonHistoricoArticulo;
        private RadButtonElement buttonModificacionMasivaArticulo;
        private RadButtonElement buttonEditarArticulo;
        private RadButtonElement buttonGuardarArticulo;
        private RadButtonElement buttonCancelarGuardado;
        private RumRibbonBarGroup ribbonBarGroupArticulo;
        private RadButtonElement eanArticulo;
        private string estilosInicio = "VisorSQLRibbonArtículosInicio.xml";

        public VisorSQLRibbonArticulos(String name, String nombreJson) : base(name, nombreJson, true, false, "", "TBLARTICULOS", "IDARTICULO")
        {
            cargarVistas(Estados.VCHibrido);
            rumPageView = new RumPageView(nombreJson, lstEsquemaTabla, this.tablaPrincipal.Width, false, false);
            this.tablaPrincipal.Controls.Add(rumPageView, 0, 1);
            modoEdicionGrid(false);
            LoadLayoutLocalInicio();
        }

        protected override void cargarOpcionesRibbonBar()
        {
            base.cargarOpcionesRibbonBar();

            buttonEditarArticulo = new RadButtonElement();
            buttonEditarArticulo.Text = "Editar Artículo";
            buttonEditarArticulo.Image = Properties.Resources.edit;
            buttonEditarArticulo.TextAlignment = ContentAlignment.BottomCenter;
            buttonEditarArticulo.ImageAlignment = ContentAlignment.MiddleCenter;
            buttonEditarArticulo.Click += EditarArticulo_Click;
            buttonEditarArticulo.Visibility = ElementVisibility.Hidden;

            buttonHistoricoArticulo = new RadButtonElement();
            buttonHistoricoArticulo.Text = "Histórico Artículo";
            buttonHistoricoArticulo.Image = Properties.Resources.EditList;
            buttonHistoricoArticulo.TextAlignment = ContentAlignment.BottomCenter;
            buttonHistoricoArticulo.ImageAlignment = ContentAlignment.MiddleCenter;
            buttonHistoricoArticulo.Click += ButtonHistoricoArticulo_Click;

            eanArticulo = new RadButtonElement();
            eanArticulo.Text = "Ean";
            eanArticulo.Image = Properties.Resources.codigobar;
            eanArticulo.TextAlignment = ContentAlignment.BottomCenter;
            eanArticulo.ImageAlignment = ContentAlignment.MiddleCenter;
            eanArticulo.Click += EanArticulo_Click;

            buttonModificacionMasivaArticulo = new RadButtonElement();
            buttonModificacionMasivaArticulo = new RumButtonElement();
            buttonModificacionMasivaArticulo.Text = "Modificación Artículos";
            buttonModificacionMasivaArticulo.Image = Properties.Resources.EditList;
            buttonModificacionMasivaArticulo.TextAlignment = ContentAlignment.BottomCenter;
            buttonModificacionMasivaArticulo.ImageAlignment = ContentAlignment.MiddleCenter;
            buttonModificacionMasivaArticulo.Click += ButtonModificacionMasivaArticulo_Click;

            buttonGuardarArticulo = new RadButtonElement();
            buttonGuardarArticulo.Text = "Guardar Artículo";
            buttonGuardarArticulo.Image = Properties.Resources.Save;
            buttonGuardarArticulo.TextAlignment = ContentAlignment.BottomCenter;
            buttonGuardarArticulo.ImageAlignment = ContentAlignment.MiddleCenter;
            buttonGuardarArticulo.Click += GuardarArticulo_Click;

            buttonCancelarGuardado = new RadButtonElement();
            buttonCancelarGuardado.Text = "Cancelar";
            buttonCancelarGuardado.Image = Properties.Resources.Cancel;
            buttonCancelarGuardado.TextAlignment = ContentAlignment.BottomCenter;
            buttonCancelarGuardado.ImageAlignment = ContentAlignment.MiddleCenter;
            buttonCancelarGuardado.Click += ButtonCancelarGuardado_Click;

            ribbonBarGroupArticulo = new RumRibbonBarGroup();
            ribbonBarGroupArticulo.Text = "Artículos";
            rtAcciones.Items.Insert(0, ribbonBarGroupArticulo);

            RumDropDownButtonElement rbCambiarVistasArticulos = new RumDropDownButtonElement();
            rbCambiarVistasArticulos.Text = "Vistas";
            rbCambiarVistasArticulos.TextAlignment = ContentAlignment.BottomCenter;
            rbCambiarVistasArticulos.ImageAlignment = ContentAlignment.MiddleCenter;
            rbCambiarVistasArticulos.Image = Properties.Resources.down_chevron16;

            RumMenuItem vistaHibrida = new RumMenuItem();
            vistaHibrida.Text = "Vista Hibrida";
            vistaHibrida.Click += (sender, e) =>
            {
                //loadData();
                cargarVistas(Estados.VCHibrido);
                rumPageView = new RumPageView(nombreJson, lstEsquemaTabla, this.tablaPrincipal.Width, false, false);
                this.tablaPrincipal.Controls.Add(rumPageView, 0, 1);
                this.tablaPrincipal.BringToFront();
            };

            RumMenuItem vistaGrid = new RumMenuItem();
            vistaGrid.Text = "Vista Grid";
            vistaGrid.Click += (sender, e) =>
            {
                cargarVistas(Estados.VCTodoGrid);
                rumPageView = new RumPageView(nombreJson, lstEsquemaTabla, this.tablaPrincipal.Width, false, false);
                this.tablaPrincipal.Controls.Add(rumPageView, 0, 1);
                //     loadData();
                // buttonEditarArticulo.Visibility = ElementVisibility.Hidden;
            };

            RumMenuItem vistaDetalle = new RumMenuItem();
            vistaDetalle.Text = "Vista Detalle";
            vistaDetalle.Click += (sender, e) =>
            {
                cargarVistas(Estados.VCTodoDetalle);
                //  buttonEditarArticulo.Visibility = ElementVisibility.Visible;
            };
            rbCambiarVistasArticulos.Items.Add(vistaHibrida);
            rbCambiarVistasArticulos.Items.Add(vistaGrid);
            rbCambiarVistasArticulos.Items.Add(vistaDetalle);
            grupoVer.Items.Add(rbCambiarVistasArticulos);

            List<NameValue> listaNameValue = new List<NameValue>();
            lstEsquemaTabla = FuncionesGenerales.CargarEsquema(this.nombreJson);
            FuncionesGenerales.RumDropDownAddManual(ref rbConfiguracion, 20004);
        }

        private void ButtonCancelarGuardado_Click(object sender, EventArgs e)
        {
            if (selectedRow == null) return;
            modoEdicionGrid(false);
            rumPageView.LoadSelectedRow(selectedRow);
        }

        private void ButtonModificacionMasivaArticulo_Click(object sender, EventArgs e)
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
                SFConfigurarArticulos ConfigurarArticulos = new SFConfigurarArticulos(name, lstEsquemaTablaMasivo, selectedRow, SFConfigurarArticulos.modoForm.edicion, esquemGrid, diccParamNuevo);
                ConfigurarArticulos.ShowDialog();

                if (ConfigurarArticulos.DialogResult != DialogResult.OK) return;
                Telerik.WinControls.Data.FilterDescriptorCollection filtrosAnteriores;

                int numeroArticulos = 0;
                if (this.panel.Contains(gridView))
                {
                    filtrosAnteriores = gridView.FilterDescriptors;
                    numeroArticulos = gridView.ChildRows.Count;
                }
                else if (this.panel.Contains(virtualGrid))
                {
                    filtrosAnteriores = virtualGrid.FilterDescriptors;
                    numeroArticulos = dtFiltradoVirtual.Rows.Count;
                }
                else
                {
                    log.Error("Problema en la modificación masiva, no hay ni GridView ni VirtualGrid en pantalla");
                    return;
                }
                if (numeroArticulos == 0)
                {
                    RadMessageBox.Show(Lenguaje.traduce("No hay artículos para modificar."));
                    log.Error("Problema en la modificación masiva, no hay cantidad");
                    return;
                }
                String mensaje = Lenguaje.traduce("Atención, estas apunto de hacer una modificación masiva de " + numeroArticulos + " registros con los filtros " + filtrosAnteriores.Expression);
                if (RadMessageBox.Show(this, mensaje, Lenguaje.traduce("Confirmación"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    log.Debug("Modificación masiva cancelada");
                    RadMessageBox.Show(Lenguaje.traduce("Cancelando!"));
                    return;
                }
                string filtrosEnSQL = "";
                log.Debug("Se va actualizar ConfigurarArticulos Masivos, filtros telerik: " + gridView.FilterDescriptors.Expression + "; Filtro where:" + filtrosEnSQL);

                if (this.panel.Contains(gridView))
                {
                    for (int i = 0; i < gridView.ChildRows.Count; i++)
                    {
                        filtrosEnSQL = " IDARTICULO=" + gridView.ChildRows[i].Cells["ID"].Value;

                        AckResponse ack = Business.EditDataMasivo(ConfigurarArticulos.newRecord, filtrosEnSQL, "TBLARTICULOS");
                        //RadMessageBox.Show(ack.Mensaje);
                        if (ack != null && ack.Resultado == "OK")
                        {
                            log.Debug("Actualización realizada! Artículo: " + filtrosEnSQL);
                        }
                        else
                        {
                            log.Error("No se ha podido hacer el cambio masivo sobre el artículo:" + filtrosEnSQL);
                        }
                    }
                }
                else if (this.panel.Contains(virtualGrid))
                {
                    for (int i = 0; i < dtFiltradoVirtual.Rows.Count; i++)
                    {
                        filtrosEnSQL = " IDARTICULO=" + dtFiltradoVirtual.Rows[i]["ID"].ToString();
                        AckResponse ack = Business.EditDataMasivo(ConfigurarArticulos.newRecord, filtrosEnSQL, "TBLARTICULOS");
                        //RadMessageBox.Show(ack.Mensaje);
                        if (ack != null && ack.Resultado == "OK")
                        {
                            log.Debug("Actualización realizada! Artículo: " + filtrosEnSQL);
                        }
                        else
                        {
                            RadMessageBox.Show(Lenguaje.traduce("Se ha añadido algún campo incorrecto, posiblemente una palabra en un campo que va un número"));
                            log.Error("No se ha podido hacer el cambio masivo sobre el artículo:" + filtrosEnSQL);
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

        private void ButtonHistoricoArticulo_Click(object sender, EventArgs e)
        {
            HistArticulosFecha formularioFecha = new HistArticulosFecha();
            DateTime fecha;
            int articulo;
            if (formularioFecha.ShowDialog() == DialogResult.OK)
            {
                fecha = formularioFecha.fecha;
                articulo = formularioFecha.idArticulo;
                List<GridViewRowInfo> rows = new List<GridViewRowInfo>();
                try
                {
                    string respJson = WebServiceHistorialArticulos(articulo, fecha);
                    GridHistArticulos gridHistArticulos = new GridHistArticulos(respJson);
                    gridHistArticulos.ShowDialog();
                }
                catch (Exception ex)
                {
                    ExceptionManager.GestionarError(ex);
                }
            }
        }

        private void GuardarArticulo_Click(object sender, EventArgs e)
        {
            AckResponse resultado = rumPageView.SaveChanges();
            if (resultado.Resultado.Equals("OK"))
            {
                //No haria falta reacargar todo. Simplemente modificar la fila seleccionada con lo mismo que se desee cambiar y actualizar el grid
                base.cargarLinea(enumLineaOpciones.edit, null);
                modoEdicionGrid(false);
                getGridViewRow();
                rumPageView.LoadSelectedRow(selectedRow);
                //loadData();
            }
            else
            {
                log.Error(resultado.Mensaje);
            }
        }

        private void EanArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRow == null)
                {
                    RadMessageBox.Show(Lenguaje.traduce("Selecciona una línea"));
                }
                else
                {
                    JObject json = JObject.Parse(selectedRow);
                    string idarticuloString = json["A.idarticulo"].ToString();

                    string whereSentence = "EAN.IDARTICULO = " + idarticuloString;
                    VisorSQLRibbon vsql = new VisorSQLRibbon("EAN", "EANProveedor", true, false, whereSentence);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private void EditarArticulo_Click(object sender, EventArgs e)
        {
            if (selectedRow == null)
            {
                RadMessageBox.Show(Lenguaje.traduce("Selecciona una línea"));
            }
            else
            {
                modoEdicionGrid(true);
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

        public void modoEdicionGrid(bool activar)
        {
            try
            {
                if (activar)
                {
                    rumPageView.Editable(true);
                    ribbonBarGroupArticulo.Items.Clear();
                    ribbonBarGroupArticulo.Items.Add(buttonGuardarArticulo);
                    ribbonBarGroupArticulo.Items.Add(buttonCancelarGuardado);
                }
                else
                {
                    rumPageView.Editable(false);
                    ribbonBarGroupArticulo.Items.Clear();
                    ribbonBarGroupArticulo.Items.Add(buttonEditarArticulo);
                    ribbonBarGroupArticulo.Items.Add(buttonHistoricoArticulo);
                    ribbonBarGroupArticulo.Items.Add(buttonModificacionMasivaArticulo);
                    ribbonBarGroupArticulo.Items.Add(eanArticulo);
                }
                foreach (RadItem item in rtAcciones.Items)
                {
                    if (activar)
                    {
                        if (item != ribbonBarGroupArticulo)
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

        public string WebServiceHistorialArticulos(int id, DateTime fecha)
        {
            WSArticulosClient wsArticulos = new WSArticulosClient();

            string fechaFormateada = fecha.ToString("dd/MM/yyyy");
            string j = formarJSONHistArticulos(id, fechaFormateada);
            log.Debug("Usuario " + User.IdUsuario + " ejecutando llamada al WebService 'WSArticulosClient.getDatosHistoricoArticulo' en Stock con parametros" + j);

            string json = wsArticulos.getDatosHistoricoArticulo(j);
            log.Debug("Terminada llamada al WebService.Respuesta:" + json);
            return json;
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
                else
                {
                    var dialog = new SFDetalle(name, lstEsquemaTabla, selectedRow, false, false, "TBLARTICULOS");
                    dialog.AutoSize = true;
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
                                //
                                cargarLinea(enumLineaOpciones.edit, null);
                                loadData();
                                QuitarFiltros();
                            }
                        }
                        //  modoEdicionGrid(true);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }

        private string formarJSONHistArticulos(int idArticulo, string fecha)
        {
            dynamic objDinamico = new ExpandoObject();
            objDinamico.id = idArticulo;
            objDinamico.fecha = fecha;
            string json = JsonConvert.SerializeObject(objDinamico);
            return json;
        }

        public void LoadLayoutLocalInicio()
        {
            string path = Persistencia.DirectorioLocal;
            int pathGL = 1;
            if (CultureInfo.CurrentUICulture.Name.Equals("es-ES"))
            {
                path += "\\Español";
            }
            else
            {
                path += "\\Ingles";
            }
            try
            {
                string s = path + "\\";
                s.Replace(" ", "_");
                if (panel.Controls.Contains(gridView))
                {
                    gridView.LoadLayout(s + estilosInicio);
                }
                else if (panel.Controls.Contains(virtualGrid))
                {
                    virtualGrid.LoadLayout(s + estilosInicio);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
    }
}
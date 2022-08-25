using Rumbo.Core.Herramientas;
using RumboSGA.Controles;
using RumboSGA.GestionAlmacen.FormularioGestorRecursos;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class FrmGestorRecursos : Telerik.WinControls.UI.RadRibbonForm
    {
        DataTable dtRecursos = new DataTable();
        DataTable dtTareasTipo = new DataTable();
        DataTable dtTiposDeTarea = new DataTable();
        DataTable dtTipoTareaPrincipal = new DataTable();
        DataTable dtTipoTareaRecurso = new DataTable();
        DataTable dtTipoTareaUbicacion = new DataTable();
        public FrmGestorRecursos()
        {
            InitializeComponent();
            CargarGridViewsRecursos();
            ConfigurarTextos();
        }
        public void CargarGridViewsRecursos()
        {
            try
            {
                cargarDatosRecursos();
                cargarDatosTareasTipo();
                cargarDatosTipoTarea();
                
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }


        }
        public void ConfigurarTextos()
        {
            radGridViewRecursos.BestFitColumns();
            RadGridViewTareaTipo.BestFitColumns();
            radGridViewZonaLog.BestFitColumns();
            RadGridViewTipoTareas.BestFitColumns();
            radGridViewTipoTareaPrincipal.BestFitColumns();
            RadGridViewRecursoTareaTipo.BestFitColumns();
            radGridViewRecursos.TitleText = Lenguaje.traduce("Recursos");
            radGridViewRecursos.GridViewElement.TitleLabelElement.Font = new Font("Consolas", 12, FontStyle.Bold);
            RadGridViewRecursoTareaTipo.TitleText = Lenguaje.traduce("Recursos asociados al tipo de tarea");
            RadGridViewRecursoTareaTipo.GridViewElement.TitleLabelElement.Font = new Font("Consolas", 12, FontStyle.Bold);
            radGridViewTipoTareaPrincipal.TitleText = Lenguaje.traduce("Tipo De Tarea");
            radGridViewTipoTareaPrincipal.GridViewElement.TitleLabelElement.Font = new Font("Consolas", 12, FontStyle.Bold);
            RadGridViewTipoTareas.TitleText = Lenguaje.traduce("Tareas Asignadas al recurso");
            RadGridViewTipoTareas.GridViewElement.TitleLabelElement.Font = new Font("Consolas", 12, FontStyle.Bold);
            radGridViewZonaLog.TitleText = Lenguaje.traduce("Zona Lógica");
            radGridViewZonaLog.GridViewElement.TitleLabelElement.Font = new Font("Consolas", 12, FontStyle.Bold);
            RadGridViewTareaTipo.TitleText = Lenguaje.traduce("Tipo de tarea");
            RadGridViewTareaTipo.GridViewElement.TitleLabelElement.Font = new Font("Consolas", 12, FontStyle.Bold);
            radBtnFormZonaLin.Text = Lenguaje.traduce("Formulario Zona");
            radBtnFormTipoTarea.Text = Lenguaje.traduce("Formulario Tipo de Tarea");
            radBtnFormRecursos.Text = Lenguaje.traduce("Formulario Recursos");
            radRibbonBarGroup2.Text = Lenguaje.traduce("Accesos Directos");
            radRibbonBarGroup3.Text = Lenguaje.traduce("Ver");
            radRibbonBarGroup1.Text = Lenguaje.traduce("Vistas");
            radbtnVistaTiposTarea.Text = Lenguaje.traduce("Vista Recursos");
            radBtnAgregarRecurso.Text = Lenguaje.traduce("Agregar Recurso");
            radBtnEliminarRecurso.Text = Lenguaje.traduce("Eliminar Recurso");
            btnCrearZona.Text = Lenguaje.traduce("Crear Línea de Zona");
            btnEditarZona.Text = Lenguaje.traduce("Editar Línea de Zona");
            btnEliminarZona.Text = Lenguaje.traduce("Eliminar Línea de Zona");
            rumButton1.Text = Lenguaje.traduce("Quitar Bloqueo");
            ribbonTab1.Text = Lenguaje.traduce("Herramientas");
            this.Text = Lenguaje.traduce("Formulario Gestión de Recursos");
            radRibbonBar1.StartButtonImage.Dispose();     
        }
        public void cargarGridViewsVistaTareasTipo()
        {
            try
            {
                cargarDatosTiposTareaPrincipal();
                cargarDatosTiposTareaRecurso();
                cargarDatosTiposTareaUbicacion();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
        public void CargarDataTable(RadGridView gridView, DataTable data, string sql)
        {

            try
            {
                data = ConexionSQL.getDataTable(sql);

                gridView.DataSource = data;
                if (gridView.Name == "RadGridViewTareaTipo")
                {
                    personalizarColor(RadGridViewTareaTipo, data);
                }
                configurarGridView(gridView, data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void cargarDatosRecursos()
        {
            try
            {
                string sqlDatosRecursos = "select IDRECURSO as '" + Lenguaje.traduce("Número de Recurso") + "', " + "r.DESCRIPCION as " + Lenguaje.traduce("Descripcion") + ", " + " ESTADO as  " + Lenguaje.traduce("Estado") + ", " + "r.NOMBREIMPRESORA as  '" + Lenguaje.traduce("Nombre Impresora") + "'," + " r.MODOPANT as  '" + Lenguaje.traduce("Modo de Pantalla") + "', " + "r.MAXTAREASRECURSO as  '" + Lenguaje.traduce("Máximo número de tareas") + "',"
                + " r.ULTIMOOPERARIO as '" + Lenguaje.traduce("Último operario") + "',OP.NOMBRE as '" + Lenguaje.traduce("Nombre Operario") + "',OP.APELLIDOS AS '" + Lenguaje.traduce("Apellidos Operario") + "' , " + "h1.descripcion as '" + Lenguaje.traduce("Ubicación Actual") + "', "
                + "h2.descripcion as '" + Lenguaje.traduce("Ubicación Principal") + "', " + "FECHALOGIN as '" + Lenguaje.traduce("Fecha Login") + "', " + "FECHALOGOUT as '" + Lenguaje.traduce("Fecha Logout") + "' from tblrecursos r left join tblzonalogcab z on z.idzonacab=r.IDZONALOG "
                + " LEFT JOIN TBLOPERARIOS OP ON OP.IDOPERARIO=R.ULTIMOOPERARIO "
                + " join tblhuecos h1 on r.idubicacionactual=h1.idhueco " + " join tblhuecos h2 on r.idubicacionprincipal=h2.idhueco ";
                CargarDataTable(radGridViewRecursos, dtRecursos, sqlDatosRecursos);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void cargarDatosTipoTarea()
        {
            try
            {
                //tengo que obtener id recurso en el sql de abajo
                if (radGridViewRecursos.CurrentRow.Cells[0].Value.ToString() != "" && radGridViewRecursos.CurrentRow.Cells[0].Value.ToString() != null)
                {
                    string valor = radGridViewRecursos.CurrentRow.Cells[0].Value.ToString();
                    string sql = "SELECT R.IDRECURSO as '" + Lenguaje.traduce("Número de Recurso") + "', R.PRIORIDAD as " + Lenguaje.traduce("Prioridad") + ", R.IDTAREATIPO,T.DESCRIPCION as " + Lenguaje.traduce("Descripción") + ""
                                + " FROM TBLRECURSOSTAREA R JOIN TBLTAREASTIPO T ON T.IDTAREATIPO=R.IDTAREATIPO where idrecurso=" + valor;


                    if (valor != null || valor != "")
                    {
                        CargarDataTable(RadGridViewTipoTareas, dtTiposDeTarea, sql);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void cargarDatosTareasTipo()
        {
            try
            {
                string sql = "Select d.TIPOTAREA as '"+Lenguaje.traduce("Tipo de Tarea")+"',d.DESCRIPCION as "+Lenguaje.traduce("Descripción")+",d.PRIORIDAD as "+ Lenguaje.traduce("Prioridad")+",z.DESCRIPCION as '"+ Lenguaje.traduce("Zona Lógica")+"',d.MAXNUMLINEAS as '"+Lenguaje.traduce("Máximo Número de Lineas")+"',d.TIPOMOVASOCIADO as '"+Lenguaje.traduce("Tipo de Movimiento Asociado")+"',d.IDTAREATIPO,d.DURSEGUNDOS as '"+Lenguaje.traduce("Duración en segundos")+"',d.COLORAMOSTRAR AS Color from TBLTAREASTIPO d join TBLZONALOGCAB z on d.ZONALOGACTUACION=z.IDZONACAB ";
                CargarDataTable(RadGridViewTareaTipo, dtTareasTipo, sql);
                
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
        public void cargarDatosTiposTareaRecurso()
        {
            try
            {
                if (radGridViewTipoTareaPrincipal.CurrentRow.Cells["IDTAREATIPO"].Value.ToString() != null || radGridViewTipoTareaPrincipal.CurrentRow.Cells["IDTAREATIPO"].Value.ToString() != "")
                {
                    String valor = radGridViewTipoTareaPrincipal.CurrentRow.Cells["IDTAREATIPO"].Value.ToString();
                    String sql = "select distinct  r.IDRECURSO as '"+Lenguaje.traduce("Número de Recurso")+"', r.DESCRIPCION as "+Lenguaje.traduce("Descripción")+", r.ESTADO as "+Lenguaje.traduce("Estado")+" from TBLRECURSOS r join TBLRECURSOSTAREA rt on rt.IDRECURSO=r.IDRECURSO  join TBLTAREASTIPO ti on ti.IDTAREATIPO=rt.IDTAREATIPO where rt.IDTAREATIPO= " + valor;
                    CargarDataTable(RadGridViewRecursoTareaTipo, dtTipoTareaRecurso, sql);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No hay valores seleccionados en el GridView Recursos"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
        public void cargarDatosTiposTareaPrincipal()
        {
            try
            {
                string sql = "SELECT  t.TIPOTAREA as '" + Lenguaje.traduce("Tipo de Tarea") + "', t.DESCRIPCION as " + Lenguaje.traduce("Descripción") + ",t.PRIORIDAD as " + Lenguaje.traduce("Prioridad") + ",r.DESCRIPCION as '" + Lenguaje.traduce("Zona Lógica") + "',t.MAXNUMLINEAS as '" + Lenguaje.traduce("Máximo Número de Lineas") + "',t.TIPOMOVASOCIADO as '" + Lenguaje.traduce("Tipo de Movimiento Asociado") + "',t.IDTAREATIPO,r.IDZONACAB FROM TBLTAREASTIPO t join TBLZONALOGCAB r on t.ZONALOGACTUACION = r.IDZONACAB";
                CargarDataTable(radGridViewTipoTareaPrincipal, dtTipoTareaPrincipal, sql);
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
        public void cargarDatosTiposTareaUbicacion()
        {
            try
            {
                string valor="";
                if (radGridViewTipoTareaPrincipal.CurrentRow != null)
                {
                     valor = radGridViewTipoTareaPrincipal.CurrentRow.Cells["IDZONACAB"].Value.ToString();
                }
                /*"SELECT IDZONACAB, IDZONALIN, AL.DESCRIPCION AS ALMACEN, ACERADESDE, ACERAHASTA, PORTALDESDE, PORTALHASTA, PISODESDE, PISOHASTA FROM @TBLZONALOGLIN L JOIN @TBLHUECOSALMACEN AL ON AL.IDHUECOALMACEN=L.IDHUECOALMACEN where idzonacab="
                            + idzonacab*/
                string sql = "SELECT  l.IDZONACAB, l.IDZONALIN, a.DESCRIPCION as " + Lenguaje.traduce("Zona") + ", l.ACERADESDE AS '" + Lenguaje.traduce("Acera Desde") + "', l.ACERAHASTA as '" + Lenguaje.traduce("Acera Hasta") + "', l.PORTALDESDE AS '" + Lenguaje.traduce("Portal Desde") + "', l.PORTALHASTA AS '" + Lenguaje.traduce("Portal Hasta") + "', l.PISODESDE AS '" + Lenguaje.traduce("Piso Desde") + "', l.PISOHASTA AS '" + Lenguaje.traduce("Piso Hasta") + "' from TBLZONALOGLIN l join TBLHUECOSALMACEN a on a.IDHUECOALMACEN=l.IDHUECOALMACEN where IDZONACAB=" + valor;
                if (valor != null && valor != "")
                {
                    CargarDataTable(radGridViewZonaLog, dtTipoTareaUbicacion, sql);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
        public void configurarGridView(RadGridView gridView, DataTable dt)
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
                gridView.EnableFiltering = true;
                gridView.AllowEditRow = false;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (gridView.Columns[i].Name.Contains("ID"))
                    {
                        gridView.Columns[i].IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void radGridView1_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            try
            {
                if (radGridViewRecursos.IsDisplayed)
                {
                    cargarDatosTipoTarea();
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, Lenguaje.traduce("No se ha actualizado correctamente el Grid"));
            }

        }

        private void rumButtonEliminarTareasTipo_Click(object sender, EventArgs e)
        {
            string error = Lenguaje.traduce("No se ha eliminado correctamente");
            try
            {


                if (RadGridViewTipoTareas.CurrentRow.Cells[0].Value.ToString() != null || RadGridViewTipoTareas.CurrentRow.Cells[0].Value.ToString() != "" || RadGridViewTipoTareas.CurrentRow.Cells[2].Value.ToString() != null || RadGridViewTipoTareas.CurrentRow.Cells[2].Value.ToString() != "")
                {
                    string idRecurso = RadGridViewTipoTareas.CurrentRow.Cells[0].Value.ToString();
                    string idTareatipo = RadGridViewTipoTareas.CurrentRow.Cells[2].Value.ToString();
                    string sql = "DELETE from TBLRECURSOSTAREA where IDRECURSO=" + idRecurso + " AND IDTAREATIPO=" + idTareatipo;
                    bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No hay ninguna fila seleccionada que eliminar"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, error);
            }
            finally
            {
                cargarDatosTipoTarea();
            }
        }

        private void rumbtnAgregarTareasTipo_Click(object sender, EventArgs e)
        {
            string error = Lenguaje.traduce("No se ha agregado correctamente");
            try
            {
                string idTareatipo = RadGridViewTareaTipo.CurrentRow.Cells[6].Value.ToString();
                string descripcion = RadGridViewTareaTipo.CurrentRow.Cells[1].Value.ToString(); ;
                string prioridad = RadGridViewTareaTipo.CurrentRow.Cells[2].Value.ToString(); ;
                string idRecurso = radGridViewRecursos.CurrentRow.Cells[0].Value.ToString();
                if (idRecurso != null || idRecurso != "" || idTareatipo != "null" || idTareatipo != "" || descripcion != "" || descripcion != null || prioridad != "" || prioridad != null)
                {
                    string sql = "INSERT INTO  TBLRECURSOSTAREA (IDRECURSO,PRIORIDAD,IDTAREATIPO,DURSEGUNDOS,ACTIVA) VALUES (" + idRecurso + "," + prioridad + "," + idTareatipo + "," + "0" + "," + "0" + ")";
                    bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, error);
            }
            finally
            {
                cargarDatosTipoTarea();
            }
        }

        private void radbtnVistaTiposTarea_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.Visible == true)
            {
                radbtnVistaTiposTarea.Text = Lenguaje.traduce("Vista Tipos de Tarea");
                cargarGridViewsVistaTareasTipo();
                tableLayoutPanel7.Visible = true;
                tableLayoutPanel1.Visible = false;
                tableLayoutPanel2.Visible = false;
                tableLayoutPanel5.Visible = true;
                tableLayoutPanel6.Visible = true;

            }
            else
            {

                radbtnVistaTiposTarea.Text = Lenguaje.traduce("Vista Recursos");
                cargarDatosRecursos();
                tableLayoutPanel7.Visible = false;
                tableLayoutPanel1.Visible = true;
                tableLayoutPanel2.Visible = true;
                tableLayoutPanel5.Visible = false;
                tableLayoutPanel6.Visible = false;
            }
        }

        private void RadGridViewRecursoTareaTipo_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            try
            {
                cargarDatosTiposTareaUbicacion();
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }

        private void radGridViewTipoTareaPrincipal_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            try
            {
                if (RadGridViewRecursoTareaTipo.Visible == true)
                {
                    cargarDatosTiposTareaRecurso();
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }

        private void radBtnFormZonaLin_Click(object sender, EventArgs e)
        {
            GenerarMantenimientos gen = new GenerarMantenimientos(Lenguaje.traduce("Zona Lógica Cab"), 20042, "ZonaLogicaCab");
            gen.ShowDialog();
        }

        private void radBtnFormTipoTarea_Click(object sender, EventArgs e)
        {
            GenerarMantenimientos gen = new GenerarMantenimientos(Lenguaje.traduce("Tipo de Tareas"), 20061, "TareasTipo");
            gen.ShowDialog();
        }

        private void radBtnFormRecursos_Click(object sender, EventArgs e)
        {
            //String filtro = "";
            //VisorSQLRibbon vsq = new VisorSQLRibbon("Recurso", "Recursos", true,"");
            //vsq.Show();
            GenerarMantenimientos gen = new GenerarMantenimientos("Recursos", 20030, "Recursos");
            gen.ShowDialog();
            cargarDatosRecursos();
        }

        private void rBtnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.Visible == true)
                {
                    CargarGridViewsRecursos();
                }
                else
                {
                    cargarGridViewsVistaTareasTipo();
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }

        private void rBtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.Visible == true)
                {
                    radGridViewRecursos.FilterDescriptors.Clear();
                    RadGridViewTareaTipo.FilterDescriptors.Clear();
                    RadGridViewTipoTareas.FilterDescriptors.Clear();
                }
                else
                {
                    RadGridViewRecursoTareaTipo.FilterDescriptors.Clear();
                    radGridViewZonaLog.FilterDescriptors.Clear();
                    radGridViewTipoTareaPrincipal.FilterDescriptors.Clear();
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }

        private void radBtnAgregarRecurso_Click(object sender, EventArgs e)
        {
            AgregarRecurso formAgregar = new AgregarRecurso(radGridViewTipoTareaPrincipal.CurrentRow.Cells[6].Value.ToString());
            formAgregar.ShowDialog();
            cargarDatosTiposTareaRecurso();
        }

        private void radBtnEliminarRecurso_Click(object sender, EventArgs e)
        {
            string error = Lenguaje.traduce("No se ha agregado correctamente");
            try
            {
                String idRecurso = RadGridViewRecursoTareaTipo.CurrentRow.Cells[0].Value.ToString();
                String idTareaTipo = radGridViewTipoTareaPrincipal.CurrentRow.Cells[6].Value.ToString();
                string sql = "DELETE FROM TBLRECURSOSTAREA  WHERE IDRECURSO=" + idRecurso + " AND IDTAREATIPO=" + idTareaTipo;
                bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                if (data == true)
                {
                    cargarDatosTiposTareaRecurso();
                    MessageBox.Show(Lenguaje.traduce("Se ha realizado correctamente"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha realizado correctamente"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }

        private void rumButton1_Click(object sender, EventArgs e)
        {
            string error = Lenguaje.traduce("No se ha agregado correctamente");
            try
            {
                string idRecurso = radGridViewRecursos.CurrentRow.Cells[0].Value.ToString();
                string sql = "UPDATE  TBLRECURSOS SET FECHALOGIN=NULL, FECHALOGOUT=NULL WHERE IDRECURSO=" + idRecurso;
                bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                if (data == true)
                {
                    cargarDatosRecursos();
                    MessageBox.Show(Lenguaje.traduce("se ha realizado correctamente"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha realizado correctamente"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }

        private void rumButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreImpresora = radGridViewRecursos.CurrentRow.Cells[3].Value.ToString();
                string sql = "select identrada from tblexistencias";
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
        private void personalizarColor(RadGridView gridView, DataTable data)
        {
            try
            {
                switch (gridView.Name)
                {
                    case "RadGridViewTareaTipo":
                        gridView.Columns[7].IsVisible = false;
                        foreach (DataRow dt in data.Rows)
                        {
                            var index = data.Columns.IndexOf("Color");
                            for (int x = 0; x <= data.Rows.Count - 1; x++)
                            {
                                //i obtiene los int del campo color
                                var i = data.Rows[x].Field<int>(index);
                                var myColor = Color.FromArgb(i);
                                var cell = gridView.Rows[x].Cells[index];

                                //Cambia a transparente el color de la letra
                                cell.Style.ForeColor = Color.Transparent;

                                //Estilos del relleno
                                cell.Style.CustomizeFill = true;
                                cell.Style.BackColor = myColor;

                                // Estilos de los bordes
                                cell.Style.CustomizeBorder = true;
                                cell.Style.BorderWidth = 1;
                                cell.Style.BorderColor = Color.Black;
                            }

                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void radGridViewZonaLog_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            
        }

        private void rumButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (radGridViewTipoTareaPrincipal.CurrentRow.Cells[7].Value.ToString() != null)
                {
                    AgregarZonaLin agregarZona = new AgregarZonaLin(radGridViewTipoTareaPrincipal.CurrentRow.Cells[7].Value.ToString());
                    agregarZona.ShowDialog();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha seleccionado ninguna fila de Tipo de Tarea"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cargarDatosTiposTareaUbicacion();
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
            
        }
        public string sacarString(RadGridView grid,int index)
        {
            try
            {
                string data = grid.CurrentRow.Cells[index].Value.ToString();
                return data;
            }
            catch (Exception e)
            {

                throw e;
            }
            
                
            
        }
        private void btnEditarZona_Click(object sender, EventArgs e)
        {
            try
            {
                if (sacarString(radGridViewZonaLog,0) != null)
                {
                    AgregarZonaLin agregarZona = new AgregarZonaLin(sacarString(radGridViewTipoTareaPrincipal,7), sacarString(radGridViewZonaLog,1), sacarString(radGridViewZonaLog,2), sacarString(radGridViewZonaLog,3), sacarString(radGridViewZonaLog,4), 
                        sacarString(radGridViewZonaLog,5), sacarString(radGridViewZonaLog,6), sacarString(radGridViewZonaLog,7), sacarString(radGridViewZonaLog,8));
                    agregarZona.ShowDialog();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha seleccionado ninguna fila del grid Zona Log"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cargarDatosTiposTareaUbicacion();
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }

        private void btnEliminarZona_Click(object sender, EventArgs e)
        {
            string error = "";
            try
            {
                if (sacarString(radGridViewZonaLog, 0) != null)
                {
                    string sql = "DELETE FROM TBLZONALOGLIN WHERE IDZONACAB="+sacarString(radGridViewZonaLog,0)+" AND IDZONALIN="+sacarString(radGridViewZonaLog,1);
                    bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce("No se ha seleccionado ninguna fila del grid Zona Log"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cargarDatosTiposTareaUbicacion();
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }

        private void rbtnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                exportarExcel export = new exportarExcel(radGridViewRecursos,RadGridViewTareaTipo,radGridViewZonaLog,RadGridViewTipoTareas,radGridViewTipoTareaPrincipal,RadGridViewRecursoTareaTipo);
                export.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }
    }
}

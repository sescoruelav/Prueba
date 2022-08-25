using RumboSGA.Controles;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class GenerarMantenimientos : Telerik.WinControls.UI.RadRibbonForm
    {
        #region Variables Generales
        BaseGridControl baseGridControl;
        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        #region Funciones Generales
        public GenerarMantenimientos(string nombreMantenimiento,int id,string nombreJSON)
        {
            InitializeComponent();
            
            attachRumControl(nombreMantenimiento,id,nombreJSON);
            InicializarEventosBotones();
            InitializeThemesDropDown();
            inicializarComboTamañofuente();
            InicializarComboPaginacion();
        }
        private void InicializarEventosBotones()
        {
            menuItemColumnas.Click += menuItemColumnas_Click;
            menuItemCargar.Click += menuItemCargar_Click;
            menuItemGuardar.Click += menuItemGuardar_Click;
        }
        private void attachRumControl(string name, int id, String nombreJson)
        {
            baseGridControl = new RumControlGeneral(nombreJson);
            baseGridControl.Dock = DockStyle.Fill;
            baseGridControl.Margin = new Padding(0, 0, 7, 7);
            baseGridControl.NombrarFormulario(baseGridControl.name);
            baseGridControl.name = name;
            baseGridControl.Name = name;
            baseGridControl.idFormulario = id;
            if (radRibbonBar1.CommandTabs.Count() == 0)
            {
                radRibbonBar1.CommandTabs.Add(mantenimientosTab);

            }
            radRibbonBar1.Expanded = true;
            radRibbonBar1.ExpandButton.Enabled = true;
            mantenimientosTab.Enabled = true;
            tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(0, 0));
            tableLayoutPanel1.Controls.Add(baseGridControl, 0, 0);

            BaseGridControl baseControl = tableLayoutPanel1.GetControlFromPosition(0, 0) as BaseGridControl;
            //lblCantidad.Text = baseControl.lblCantidad.Text;
            //baseControl.lblCantidad.TextChanged += RecogerCantidad;
            mantenimientosTab.IsSelected = true;
            if (baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                btnCambiarVista.Text = "Vista Rápida";
            }
            else
            {
                btnCambiarVista.Text = "Vista Avanzada";
            }
            DeshabilitarControlesPermisos(baseGridControl.idFormulario);
            this.ResumeLayout();
            /*
            //Eliminar layout button
            if (this.btnConf.Items["RadMenuItemEliminarLayout"] != null)
            {
                int indexBoton = this.btnConf.Items.IndexOf(this.btnConf.Items["RadMenuItemEliminarLayout"]);
                if (indexBoton != -1) this.btnConf.Items.RemoveAt(indexBoton);
            }
            if (baseControl != null && (baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView ||
                baseControl.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid))
            {
                FuncionesGenerales.AddEliminarLayoutButton(ref btnConf);
                if (this.btnConf.Items["RadMenuItemEliminarLayout"] != null)
                {
                    this.btnConf.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                    {
                        if (this.tableLayoutPanel1.GetControlFromPosition(0, 1) is RadGridView)
                        {
                            FuncionesGenerales.EliminarLayout(baseControl.Name + "GridView" + ConexionSQL.getNombreConexion() + ".xml", null);
                            baseControl.GridView.Refresh();
                        }
                        else
                        {
                            FuncionesGenerales.EliminarLayout(baseControl.Name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml", null);
                            baseControl.virtualGrid.Refresh();
                        }
                    });
                }
            }*/
        }
        private void DeshabilitarControlesPermisos(int idFormulario)
        {
            if (!User.Perm.tienePermisoEscritura(idFormulario))
            {
                foreach (RibbonTab item in radRibbonBar1.CommandTabs)
                {
                    foreach (RadRibbonBarGroup grupo in item.Items)
                    {
                        if (grupo.Name != "Vista" && grupo.Name != "Conf" && grupo.Name != "EtiqCant")
                        {
                            grupo.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (RibbonTab item in radRibbonBar1.CommandTabs)
                {
                    foreach (RadRibbonBarGroup grupo in item.Items)
                    {
                        if (grupo.Name != "Vista" && grupo.Name != "Conf" && grupo.Name != "EtiqCant")
                        {
                            grupo.Enabled = true;
                        }
                    }
                }
            }
        }
        #endregion
        #region Botones Eventos
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.newButton_Click(sender, e);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.deleteButton_Click(sender, e);
        }

        private void btnClonar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.cloneButton_Click(sender, e);
        }
        private void CambiarVista_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.btnCambiarVista_Click(sender, e);
            if (a.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
            {
                btnCambiarVista.Text = strings.CambiarVistaGridView;
                btnExportar.Enabled = true;
                menuItemColumnas.Enabled = true;
            }
            else
            {
                btnCambiarVista.Text = strings.CambiarVistaVirtual;
                btnExportar.Enabled = false;
                menuItemColumnas.Enabled = false;
            }
        }
        private void QuitarFiltros_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.btnQuitarFiltros_Click(sender, e);
        }
        private void Exportar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.btnExportacion_Click(sender, e);
        }
        private void BarraBusqueda_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            //a.OcultarBarraBusqueda(sender, e);
        }
        private void Refrescar_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.GetControlFromPosition(0, 0) is BaseGridControl)
            {
                BaseGridControl a = tableLayoutPanel1.GetControlFromPosition(0, 0) as BaseGridControl;
                if (a is RumControlGeneral)
                {
                    //El RumControl recarga los datos desde llenarGrid
                    //idependientemente de si es virtual o grid.
                    a.llenarGrid();
                }
                else if (a.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                {
                    a.llenarGrid();
                    a.ElegirEstilo();
                }
                else
                {
                    a.RefreshData(0);
                    a.ElegirEstilo();
                }
            }
        }
        private void menuItemGuardar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.SaveButton_Click(sender, e);
        }
        private void menuItemCargar_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.LoadButton_Click(sender, e);
        }
        private void menuItemColumnas_Click(object sender, EventArgs e)
        {
            BaseGridControl a = (BaseGridControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.btnColumnas_Click(sender, e);
        }
        private void inicializarComboTamañofuente()
        {
            for (int i = 8; i < 21; i++)
            {
                menuItemTamLetra.Items.Add(i.ToString());
            }
            menuItemTamLetra.ComboBoxElement.SelectedIndexChanged += comboTamañoFuente_Changed;
        }

        private void comboTamañoFuente_Changed(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is BaseGridControl)
                {
                    BaseGridControl temp = tableLayoutPanel1.GetControlFromPosition(0, 0) as BaseGridControl;
                    float tamaño = float.Parse(menuItemTamLetra.ComboBoxElement.SelectedItem.Text);
                    temp.virtualGridControl.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    temp.GridView.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        private void InicializarComboPaginacion()
        {
            if (XmlReaderPropio.getPaginacion() <= 10)
            {
                for (int i = 20; i < 60; i++)
                {
                    menuComboItem.Items.Add(i.ToString());
                }
            }
            else
            {
                for (int i = XmlReaderPropio.getPaginacion(); i < 60; i++)
                {
                    menuComboItem.Items.Add(i.ToString());
                }
            }
            menuComboItem.ComboBoxElement.SelectedIndexChanged += comboPaginacion_Changed;
        }

        private void comboPaginacion_Changed(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is BaseGridControl)
                {
                    BaseGridControl temp = tableLayoutPanel1.GetControlFromPosition(0, 0) as BaseGridControl;
                    int tamaño = int.Parse(menuComboItem.ComboBoxElement.SelectedItem.Text);
                    temp.virtualGridControl.PageSize = tamaño;
                    temp.GridView.PageSize = tamaño;
                    XmlReaderPropio.setPaginacion(tamaño);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }
        #endregion

        #region Temas
        private void InitializeThemesDropDown()
        {
            AddThemeItemToThemesDropDownList("Fluent", Resources.fluent);
            AddThemeItemToThemesDropDownList("FluentDark", Resources.fluent_dark);
            AddThemeItemToThemesDropDownList("Material", Resources.material);
            AddThemeItemToThemesDropDownList("MaterialPink", Resources.material_pink);
            AddThemeItemToThemesDropDownList("MaterialTeal", Resources.material_teal);
            AddThemeItemToThemesDropDownList("MaterialBlueGrey", Resources.material_blue_grey);
            AddThemeItemToThemesDropDownList("ControlDefault", Resources.control_default);
            AddThemeItemToThemesDropDownList("TelerikMetro", Resources.telerik_metro);
            AddThemeItemToThemesDropDownList("Windows8", Resources.windows8);

            loadedThemes.Add("ControlDefault", true);
            loadedThemes.Add("TelerikMetro", true);

            var assemblyName = "Telerik.WinControls.Themes.Windows8.dll";
            var strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), assemblyName);
            Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
            Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + "Windows8" + "Theme"));

            loadedThemes.Add("Windows8", true);
            ThemeResolutionService.ApplicationThemeName = "Windows8"; //Tema por Defecto de la Aplicación


        }

        private void temasItem_Click(object sender, EventArgs e)
        {
            var assemblyName = "Telerik.WinControls.Themes." + (sender as RadMenuItem).Text + ".dll";
            var themeName = (sender as RadMenuItem).Text;
            var strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), assemblyName);
            if (!System.IO.File.Exists(strTempAssmbPath)) // we are in the case of QSF as exe, so the Path is different
            {
                strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\Bin40");
                strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);

                if (!System.IO.File.Exists(strTempAssmbPath))
                {
                    strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\bin\\ReleaseTrial");
                    strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);
                }
            }

            if (!loadedThemes.ContainsKey(themeName))
            {

                Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
                Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + themeName + "Theme"));

                loadedThemes.Add(themeName, true);
            }

            ThemeResolutionService.ApplicationThemeName = themeName;
            if (ControlTraceMonitor.AnalyticsMonitor != null)
            {
                ControlTraceMonitor.AnalyticsMonitor.TrackAtomicFeature("ThemeChanged." + themeName);
            }
        }
        private void AddThemeItemToThemesDropDownList(string themeName, Image image)
        {
            RadMenuItem mainItem = menuItemTemas/*.Items[0] as RadMenuItem*/;
            RadMenuItem temasItem = new RadMenuItem();
            temasItem.Text = themeName;
            temasItem.Image = image;
            temasItem.Click += new EventHandler(temasItem_Click);
            mainItem.Items.Add(temasItem);
        }

        #endregion
    }

}

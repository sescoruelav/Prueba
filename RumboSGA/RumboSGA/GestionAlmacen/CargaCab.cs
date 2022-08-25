using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.UserControls.Mantenimientos;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class CargaCab : Telerik.WinControls.UI.RadRibbonForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CargaCabControl mantenimiento;
        public CargaCab()
        {
            InitializeComponent();
            //((Telerik.WinControls.UI.RadArrowButtonElement)(this.radRibbonBar1.GetChildAt(0).GetChildAt(4).GetChildAt(1).GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            CargaCabControl mantenimiento = new CargaCabControl();
            inicializarComboTamañofuente();
            InicializarComboPaginacion();
            tableLayoutPanel1.Controls.Add(mantenimiento,0,0);
            mantenimiento.Dock = DockStyle.Fill;
            ControlesSetText();
            InicializarEventosBotones();
            this.Text = Lenguaje.traduce("Cabecera carga");



            FuncionesGenerales.AddEliminarLayoutButton(ref btnConf);
            if (this.btnConf.Items["RadMenuItemEliminarLayout"] != null)
            {
                this.btnConf.Items["RadMenuItemEliminarLayout"].Click += new EventHandler((object e, EventArgs a) =>
                {

                    if (mantenimiento.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadVirtualGrid)
                    {
                        FuncionesGenerales.EliminarLayout(mantenimiento.name + "VirtualGrid" + ConexionSQL.getNombreConexion() + ".xml", mantenimiento.GridView);
                    }
                    else if (mantenimiento.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                    {
                        FuncionesGenerales.EliminarLayout(mantenimiento.name + "GridView" + ConexionSQL.getNombreConexion() + ".xml", mantenimiento.GridView);
                    }

                });
            }

        }

        private void InicializarEventosBotones()
        {
            menuItemColumnas.Click += menuItemColumnas_Click;
            menuItemCargar.Click += menuItemCargar_Click;
            menuItemGuardar.Click += menuItemGuardar_Click;
        }
        private void menuItemGuardar_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.SaveButton_Click(sender, e);
        }
        private void menuItemCargar_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.LoadButton_Click(sender, e);
        }
        private void menuItemColumnas_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.btnColumnas_Click(sender, e);
        }
        private void Nuevo_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.newButton_Click(sender, e);
        }
        private void Borrar_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.deleteButton_Click(sender, e);
        }
        private void Clonar_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.cloneButton_Click(sender, e);
        }

        private void CambiarVista_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
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
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.btnQuitarFiltros_Click(sender, e);
        }
        private void Refrescar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is CargaCabControl)
                {
                    CargaCabControl a = tableLayoutPanel1.GetControlFromPosition(0, 0) as CargaCabControl;
                    if (a.tableLayoutPanel1.GetControlFromPosition(0, 0) is RadGridView)
                    {
                        a.llenarGrid();
                    }
                    else
                    {
                        a.RefreshData(0);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error al actualizar el grid de CargaCab en Refrescar_Click");
                ExceptionManager.GestionarError(ex,"Error recargando");
            }
        }
        private void Exportar_Click(object sender, EventArgs e)
        {
            CargaCabControl a = (CargaCabControl)tableLayoutPanel1.GetControlFromPosition(0, 0);
            a.btnExportacion_Click(sender, e);
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
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is CargaCabControl)
                {
                    CargaCabControl temp = tableLayoutPanel1.GetControlFromPosition(1, 0) as CargaCabControl;
                    float tamaño = float.Parse(menuItemTamLetra.ComboBoxElement.SelectedItem.Text);
                    //temp.virtualGridControl.Font = new System.Drawing.Font("Microsoft Sans Serif", tamaño, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
                if (tableLayoutPanel1.GetControlFromPosition(0, 0) is CargaCabControl)
                {
                    CargaCabControl temp = tableLayoutPanel1.GetControlFromPosition(1, 0) as CargaCabControl;
                    int tamaño = int.Parse(menuComboItem.ComboBoxElement.SelectedItem.Text);
                    //temp.virtualGridControl.PageSize = tamaño;
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
        private void ControlesSetText()
        {
            menuItemColumnas.Text = strings.Columnas;
            menuItemCargar.Text = strings.Cargar;
            menuItemGuardar.Text = strings.Guardar;
            menuItemTemas.Text = strings.Temas;

            headerTamLetra.Text = "Tamaño Letra Tabla";
            btnNuevo.Text = strings.Nuevo;
            btnClonar.Text = strings.Clonar;
            btnBorrar.Text = strings.Borrar;
            modBarGroup.Text = strings.Modificaciones;
            vistaBarGroup.Text = strings.Ver;
            configBarGroup.Text = strings.Configuracion;
        }
    }
}

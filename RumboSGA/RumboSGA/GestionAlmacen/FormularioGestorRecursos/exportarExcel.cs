using Rumbo.Core.Herramientas;
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using static RumboSGA.FuncionesGenerales;

namespace RumboSGA.GestionAlmacen.FormularioGestorRecursos
{
    public partial class exportarExcel : Telerik.WinControls.UI.RadForm
    {
        RadGridView radRec,radTar,radZon,radTip,radPrin,radRecTar;

        private void rumButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rumButton1_Click(object sender, EventArgs e)
        {
            ComprobarSeleccionado();
        }

        List<string> grids = new List<string>(5) {Lenguaje.traduce("Recursos"), Lenguaje.traduce("Recursos asociados al tipo de tarea"), Lenguaje.traduce("Tipo de Tarea"), Lenguaje.traduce("Tareas asignadas al recurso"), Lenguaje.traduce("Zona Lógica") };
        public exportarExcel(RadGridView radRecursos, RadGridView radTareaTipo, RadGridView radZonaLog, RadGridView radTipoTareas, RadGridView radTipoTareasPrincipal, RadGridView radRecursoTareaTipo)
        {
            InitializeComponent();
            comboBox1.DataSource = grids;
            radRec = radRecursos;
            radTar = radTareaTipo;
            radZon = radZonaLog;
            radTip = radTipoTareas;
            radPrin=radTipoTareasPrincipal;
            radRecTar = radRecursoTareaTipo;
            radTitleBar1.Text = Lenguaje.traduce("Exportar Excel");
        }
        public void ComprobarSeleccionado()
        {
            try
            {
                switch (comboBox1.Text)
                {
                    case "Recursos":
                        ejecutarFuncion(radRec);
                        break;
                    case "Resources":
                        ejecutarFuncion(radRec);
                        break;
                    case "Recursos asociados al tipo de tarea":
                        ejecutarFuncion(radTip);
                        break;
                    case "Task Type Assigned Resources":
                        ejecutarFuncion(radTip);
                        break;
                    case "Tipo de Tarea":
                        ejecutarFuncion(radTar);
                        break;
                    case "TasK Type":
                        ejecutarFuncion(radTar);
                        break;
                    case "Tareas asignadas al recurso":
                        ejecutarFuncion(radRecTar);
                        break; 
                    case "Resource Assigned Tasks":
                        ejecutarFuncion(radRecTar);
                        break;
                    case "Zona Lógica":
                        ejecutarFuncion(radZon);
                        break;
                    case "Logical Zone":
                        ejecutarFuncion(radZon);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
        public void ejecutarFuncion(RadGridView rad)
        {
            try
            {
                FuncionesGenerales.exportarAExcelGenerico(rad);
                MessageBox.Show(Lenguaje.traduce("Se ha realizado correctamente"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
    }
}

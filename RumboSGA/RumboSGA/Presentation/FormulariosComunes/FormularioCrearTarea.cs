using Rumbo.Core.Herramientas;
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

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FormularioCrearTarea : Telerik.WinControls.UI.RadForm
    {
        private static String[] camposRecurso = { "IDRECURSO","DESCRIPCION" };
        private static string campoFormRecurso = "TBLRECURSOS";
        private static string valueMemberRecurso = "IDRECURSO";
        private static string displayMemberRecurso = "DESCRIPCION";

        private static String[] camposTareas = { "TIPOTAREA", "DESCRIPCION" };
        private static string campoFormTarea = "TBLTAREASTIPO";
        private static string valueMemberTarea = "TIPOTAREA";
        private static string displayMemberTarea = "DESCRIPCION";

        public FormularioCrearTarea(int idRecurso, string recursoTexto, bool recursoModificable)
        {
            InitializeComponent();
            this.Name = "FormularioCrearTarea";
            this.Text = Lenguaje.traduce("Nueva tarea");

            cargarMultiColumns(idRecurso, recursoModificable);
            

        }

        private void cargarMultiColumns(int idRecursoDefecto,bool recursoModificable)
        {
            this.radMultiColumnComboBoxRecurso.Enabled = recursoModificable;

            DataTable tRec = DataAccess.getDataTableSQL(camposRecurso, campoFormRecurso,"");
            Utilidades.RellenarMultiColumnComboBox(ref this.radMultiColumnComboBoxRecurso, tRec, valueMemberRecurso, displayMemberRecurso, idRecursoDefecto.ToString(), new String[] { "TODOS" }, true);

            DataTable tTar = DataAccess.getDataTableSQL(camposTareas, campoFormTarea,"");
            Utilidades.RellenarMultiColumnComboBox(ref this.radMultiColumnComboBoxTarea, tTar, valueMemberTarea, displayMemberTarea, "", new String[] { "TODOS" }, true);
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
        }

        /*
         * Metodo para crear nueva tarea, devuelve -1 si da error, y 1 si es correcto
         */
        public static int CrearNuevaTarea(int idRecurso, int idTareaTipo, int duracion, int activa)
        {
            return 0;
        }
    }
}

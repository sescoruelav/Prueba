using Rumbo.Core.Herramientas;
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
using Telerik.WinControls;

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FormularioCrearTareaTipo : Telerik.WinControls.UI.RadForm
    {
        private static String[] zonaLogCampos = { "IDZONACAB","DESCRIPCION"};
        private static String tablaLogCamp = "TBLZONALOGCAB";
        private static String valueMemberZonaLog = "IDZONACAB";
        private static String displayMemberZonaLog = "DESCRIPCION";

        public FormularioCrearTareaTipo()
        {
            InitializeComponent();
            CrearVisual();
            this.Text = Lenguaje.traduce("Crear nuevo tipo tarea");

        }


        private void CrearVisual()
        {
            DataTable dt = DataAccess.getDataTableSQL(zonaLogCampos,tablaLogCamp,"");
            Utilidades.RellenarMultiColumnComboBox(ref this.radMultiColumnComboBoxZonaLogAct,dt,valueMemberZonaLog,displayMemberZonaLog,"", new String[] { "TODOS" }, true);


        }

        private bool ComprobarCampos()
        {
            string mensajeError = "";
            if (string.IsNullOrEmpty(this.radTextBoxTareaTipo.Text))
            {
                mensajeError += Lenguaje.traduce("Tipo tarea no puede ser vacío") + "\n";
            }
            if (this.radTextBoxTareaTipo.Text.Length!=2)
            {
                mensajeError += Lenguaje.traduce("Tipo tarea tiene que tener 2 caracteres") + "\n";
            }

            if (string.IsNullOrEmpty(this.radTextBoxDescripcion.Text))
            {
                mensajeError += Lenguaje.traduce("Descripción no puede ser vacío") + "\n";
            }
            if (string.IsNullOrEmpty(this.radTextBoxTipoMovAsociado.Text))
            {
                mensajeError += Lenguaje.traduce("Tipo movimiento asociado no puede ser vacío") + "\n";
            }
            if (this.radTextBoxTipoMovAsociado.Text.Length != 2)
            {
                mensajeError += Lenguaje.traduce("Movimiento asociado tiene que tener 2 caracteres") + "\n";
            }
            if (this.radSpinEditorMaxNumLineas.Value < 1)
            {
                mensajeError += Lenguaje.traduce("Máximo número de líneas no puede ser menor que 0") + "\n";
            }
            if (this.radSpinEditorPrioridad.Value < 1)
            {
                mensajeError += Lenguaje.traduce("Prioridad no puede ser menor que 0") + "\n";
            }

            if (!this.radMultiColumnComboBoxZonaLogAct.SelectedValue.ToString().Equals("")&& int.Parse(this.radMultiColumnComboBoxZonaLogAct.SelectedValue.ToString())<0)
            {
                mensajeError += Lenguaje.traduce("Selecciona una zona lógica") + "\n";
            }
            if (mensajeError.Equals(""))
                return true;

             RadMessageBox.Show(this,mensajeError,"Error",MessageBoxButtons.OK);
            return false;

        }


        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            if (!ComprobarCampos()) return;
            AckResponse res = DataAccess.CrearTareaTipo(this.radTextBoxTareaTipo.Text, this.radTextBoxDescripcion.Text, int.Parse(radSpinEditorPrioridad.Value.ToString())
                ,int.Parse(this.radMultiColumnComboBoxZonaLogAct.SelectedValue.ToString()),int.Parse(radSpinEditorMaxNumLineas.Value.ToString()),
                radTextBoxTipoMovAsociado.Text,int.Parse(radSpinEditorDuracion.Value.ToString()));


            if (res == null) return;

            RadMessageBox.Show(this, res.Mensaje, res.Resultado, MessageBoxButtons.OK);

            if (res.Resultado.Equals("OK"))
            {
                this.Close();
            }

        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

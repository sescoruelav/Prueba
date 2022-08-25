using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas.Stock
{
    public partial class ElegirRecurso : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string idRecurso;
        public string descripcion;
        RadForm form=new RadForm();
        public ElegirRecurso()
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Asignar recurso");
            btnCancelar.Click += btnCancelar_Click;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            DataTable dt = ConexionSQL.getDataTable("Select IDRECURSO AS 'Numero recurso', DESCRIPCION AS 'Nombre recurso' from TBLRECURSOS");
            Utilidades.RellenarMultiColumnComboBox(ref comboBoxRecursos, dt, "Numero recurso", "Nombre recurso", "", null, true);


        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            idRecurso = comboBoxRecursos.SelectedValue.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
            form.Close();
        }
        private void btnCancelar_Click(object sender,EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            form.Close();
        }
    }
}

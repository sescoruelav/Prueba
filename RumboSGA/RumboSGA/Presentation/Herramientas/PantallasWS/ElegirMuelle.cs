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

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class ElegirMuelle : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int idMuelle;
        public string descripcion;
        RadForm form=new RadForm();
        public ElegirMuelle()
        {
            InitializeComponent();
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.Text = Lenguaje.traduce("Selecciona muelle");
            this.labelMuelle.Text = Lenguaje.traduce("Muelle");
            rcbMuelle.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            Utilidades.RellenarMultiColumnComboBox(ref rcbMuelle, DataAccess.GetIdDescripcionHuecoMuelle(), "IDHUECO", "DESCRIPCION","-1" , null, true);
            //if (CultureInfo.CurrentUICulture.Name == "es-ES")
            //{
            //    rddlMuelle.DataSource = ConexionSQL.getDataTable("Select IDRECURSO AS 'Num Recurso', DESCRIPCION AS Descripcion from TBLRECURSOS");
            //    rddlMuelle.DisplayMember = "Descripcion";
            //    form.Text = "Recursos";
            //    rddlMuelle.ValueMember = "Num Recurso";
            //}
            //else
            //{
            //    rddlMuelle.DataSource = ConexionSQL.getDataTable("Select IDRECURSO AS 'Resource Num', DESCRIPCION AS Description from TBLRECURSOS");
            //    form.Text = "Resources";
            //    rddlMuelle.DisplayMember = "Description";
            //    rddlMuelle.ValueMember = "IDRECURSO";
            //    labelMuelle.Text = "Resource";
            //    this.Text = "Resource Asign";
            //}

        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            descripcion = rcbMuelle.Text;
            if (String.IsNullOrEmpty(descripcion))
            {
                RadMessageBox.Show(Lenguaje.traduce("Debes seleccionar un muelle"));
                return;
            }
            idMuelle = Convert.ToInt32(rcbMuelle.SelectedValue);
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

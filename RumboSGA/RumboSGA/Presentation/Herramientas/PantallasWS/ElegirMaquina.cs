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
    public partial class ElegirMaquina : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string descripcion;
        public int idMaquina;
        RadForm form=new RadForm();
        public ElegirMaquina()
        {
            InitializeComponent();
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            maquinaList.AutoCompleteMode = AutoCompleteMode.Suggest;
            if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {
                maquinaList.DataSource = ConexionSQL.getDataTable("Select IDMAQUINA AS 'Num Maquina', DESCRIPCION AS Descripcion from TBLMAQUINAS");
                maquinaList.DisplayMember = "Descripcion";
                form.Text = "Maquinas";
                maquinaList.ValueMember = "Num Maquina";
            }
            else
            {
                maquinaList.DataSource = ConexionSQL.getDataTable("Select IDMAQUINA AS 'Num Machine', DESCRIPCION AS Description from TBLMAQUINAS");
                labelMaquina.Text = "Machine";
                maquinaList.DisplayMember = "Description";
                maquinaList.ValueMember = "Num Machine";
                this.Text = "Machine Asign";
                form.Text = "Machines";
            }
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            descripcion = maquinaList.Text;
            idMaquina = (int)maquinaList.SelectedValue;
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

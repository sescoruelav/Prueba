using RumboSGA.Properties;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using RumboSGAManager;
using Rumbo.Core.Herramientas;
using RumboSGA.SalidaMotor;
using RumboSGAManager.Model;

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class SeleccionRegularizacion : Telerik.WinControls.UI.RadForm
    {
        public string idMotivo;
        public string idOperario;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string comentario;
        public SeleccionRegularizacion()
        {
            InitializeComponent();
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;
            this.Text = Lenguaje.traduce("Regularizacion");
            this.Text = Lenguaje.traduce("Salida Regularización");
            //string query = "SELECT IDMOTIVO,DESCRIPCION FROM TBLMOTIVOSREG";
            //string query2 = "SELECT * FROM TBLOPERARIOS";
            //DataTable dtMotivos = ConexionSQL.getDataTable(query);
            //DataTable dt2 = ConexionSQL.getDataTable(query2);

            //radDropDownList1.DataSource = dtMotivos.DefaultView;
            //radDropDownList1.DisplayMember = "DESCRIPCION";
            //radDropDownList1.ValueMember = "IDMOTIVO";
            //comboMotivo.DataSource = dtMotivos.DefaultView;
            //comboMotivo.MultiColumnComboBoxElement.DropDownWidth = 300;
            //comboMotivo.MultiColumnComboBoxElement.BestFitColumns();
            //comboMotivo.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //comboMotivo.MultiColumnComboBoxElement.AutoSizeDropDownColumnMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            //comboMotivo.DisplayMember = "DESCRIPCION";
            //comboMotivo.ValueMember = "IDMOTIVO";
            //comboMotivo.ClearTextOnValidation = true;
            Utilidades.RellenarMultiColumnComboBox(ref radComboBoxMotivo, DataAccess.GetIdDescripcionMotivosTipo("SR"), "IDMOTIVO", "DESCRIPCION", "", new String[] { "TODOS" }, true);


            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            try
            {
                idMotivo = radComboBoxMotivo.SelectedValue.ToString();
                //idOperario = comboOperario.SelectedValue.ToString();
                comentario = radTextBoxControl1.Text;

                this.DialogResult = DialogResult.OK;
                
                this.Close();
            }
            catch (FormatException exc)
            {
                ExceptionManager.GestionarError(exc);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.PantallasWS
{
    public partial class LanzamientoPedidoWS : Telerik.WinControls.UI.ShapedForm
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string descripcionHueco;
        public int idHueco;
        public string descripcionRecurso;
        public int idRecurso;
        RadForm form = new RadForm();
        public LanzamientoPedidoWS()
        {

            InitializeComponent();
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            //muelleList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //recursoList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            labelMuelle.Text = Lenguaje.traduce(strings.Muelle);
            recursoLabel.Text = Lenguaje.traduce("Recurso");
            form.Text = strings.Seleccionar;
            string idMuelle = "";
            try
            {
                idMuelle = Persistencia.getParametroInt("MUELLEDEFECTOSALIDA").ToString();
               
            }
            catch (Exception ex)
            {

            }
            DataTable dtMuelle = ConexionSQL.getDataTable("Select IDHUECO AS 'Num Hueco', DESCRIPCION AS Descripcion from TBLHUECOS WHERE IDHUECOESTANTE = 'MU'");
            DataTable dtRecurso = ConexionSQL.getDataTable("Select IDRECURSO AS 'Num Recurso', DESCRIPCION AS Descripcion from TBLRECURSOS");

            Utilidades.RellenarMultiColumnComboBox(ref muelleList, dtMuelle, "Num Hueco", "DESCRIPCION",idMuelle, null, true);
            Utilidades.RellenarMultiColumnComboBox(ref recursoList, dtRecurso, "Num Recurso", "DESCRIPCION", "", null, true);
            
            /*
            if (CultureInfo.CurrentUICulture.Name == "es-ES")
            {
                muelleList.DataSource = ConexionSQL.getDataTable("Select IDHUECO AS 'Num Hueco', DESCRIPCION AS Descripcion from TBLHUECOS WHERE IDHUECOESTANTE = 'MU'");
                muelleList.ValueMember = "Num Hueco";
                recursoList.DataSource = ConexionSQL.getDataTable("Select IDRECURSO AS 'Num Recurso', DESCRIPCION AS Descripcion from TBLRECURSOS");
                recursoList.ValueMember = "Num Recurso";
            }
            else
            {
                muelleList.DataSource = ConexionSQL.getDataTable("Select idhueco AS 'Location ID', DESCRIPCION AS Description from TBLHUECOS WHERE IDHUECOESTANTE = 'MU'");
                muelleList.ValueMember = "Location ID";
                recursoList.DataSource = ConexionSQL.getDataTable("Select IDRECURSO AS 'Resource Num', DESCRIPCION AS Description from TBLRECURSOS");
                recursoList.ValueMember = "Resource Num";
            }
            muelleList.DisplayMember = Lenguaje.traduce(strings.Descripcion);
            recursoList.DisplayMember = Lenguaje.traduce(strings.Descripcion);*/
        }

        private void CargarValorDefectoMuelle()
        {
            try
            {
                object idMuelle = Persistencia.getParametroInt("MUELLEDEFECTOSALIDA");
                muelleList.SelectedItem = idMuelle;
            }catch(Exception ex)
            {

            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var muelleListSelectedItem = (GridViewDataRowInfo)muelleList.SelectedItem;
            var recursoListSelectedItem = (GridViewDataRowInfo)recursoList.SelectedItem;
            descripcionHueco = (string)muelleListSelectedItem.Cells[Lenguaje.traduce("Descripcion")].Value;
            idHueco = (int)muelleListSelectedItem.Cells[Lenguaje.traduce("Num Hueco")].Value;
            descripcionRecurso = (string)recursoListSelectedItem.Cells[Lenguaje.traduce("Descripcion")].Value;
            idRecurso = (int)recursoListSelectedItem.Cells[Lenguaje.traduce("Num Recurso")].Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
            form.Close();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            form.Close();
        }

        
    }
}
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas.Stock
{
    public partial class Reubicar : Telerik.WinControls.UI.ShapedForm
    {
        private List<string> entradas = new List<string>();
        public int idHueco;
        public bool conMovimiento;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Reubicar(List<string> entradas)
        {
            InitializeComponent();
            this.Name = Lenguaje.traduce(strings.Reubicar);
            this.Text = Lenguaje.traduce(strings.Reubicar);
            this.entradas = entradas;
            lblNuevaUb.Text = strings.NuevaUb;
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;
            checkMovimiento.Text = Lenguaje.traduce(strings.ConMovimiento);
            this.btnCancelar.Click += btnCancelar_Click;
            string query = formarQuery();
            log.Info("La consulta para la reubicacion es: " + query);
            this.radGridView1.DataSource = ConexionSQL.getDataTable(query);
            radMultiColumnComboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            foreach (GridViewDataColumn item in radGridView1.Columns)
            {
                item.HeaderText = Lenguaje.traduce(item.HeaderText);
            }
            this.radGridView1.BestFitColumns();

            DataTable dt = ConexionSQL.getDataTable("SELECT DESCRIPCION, IDHUECO FROM TBLHUECOS");
            String[] filtro = { "Description" };
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBox1, dt, "IDHUECO", "DESCRIPCION", "", null, true);
            radMultiColumnComboBox1.TextChanged += RadMultiColumnComboBox1_TextChanged1;
        }

        private void RadMultiColumnComboBox1_TextChanged1(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private string formarQuery()
        {
            string query = "SELECT AR.REFERENCIA AS ARTICULO,E.IDPALETTIPO AS 'TIPO PALET', AR.DESCRIPCION AS DESCRIPCION,H.DESCRIPCION AS UBICACION, E.FECHA AS 'FECHA ENTRADA' " +
            "FROM TBLEXISTENCIAS EX " +
            "LEFT JOIN TBLENTRADAS E ON E.IDENTRADA = EX.IDENTRADA " +
            "LEFT JOIN TBLPALETSMULTIREFERENCIA PM ON PM.IDENTRADAHIJA = EX.IDENTRADA " +
            "LEFT JOIN TBLENTRADAS EP ON PM.IDENTRADAPADRE = EP.IDENTRADA " +
            "JOIN TBLARTICULOS AR ON AR.IDARTICULO = EX.IDARTICULO " +
            "JOIN TBLHUECOS H ON H.IDHUECO = EX.IDHUECO  ";
            int cont = 0;
            foreach (var item in entradas)
            {
                int entrada = int.Parse(item);

                if (cont == 0)
                {
                    query += "WHERE EX.IDENTRADA=" + entrada;
                }
                else
                {
                    query += " OR EX.IDENTRADA=" + entrada;
                }
                cont++;
            }
            return query;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                idHueco = (int)radMultiColumnComboBox1.SelectedValue;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
        }

        private void checkMovimiento_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            conMovimiento = checkMovimiento.Checked;
        }

        private void RadMultiColumnComboBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
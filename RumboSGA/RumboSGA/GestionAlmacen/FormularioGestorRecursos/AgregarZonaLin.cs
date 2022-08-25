using Rumbo.Core.Herramientas;
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

namespace RumboSGA.GestionAlmacen.FormularioGestorRecursos
{
    public partial class AgregarZonaLin : Telerik.WinControls.UI.RadForm
    {
        private static String[] camposAlmacen = { "IDHUECOALMACEN", "DESCRIPCION" };
        private static string campoFormAlmacen = "TBLHUECOSALMACEN";
        private static string valueMemberAlmacen = "IDHUECOALMACEN";
        private static string displayMemberAlmacen = "DESCRIPCION";
        private string idLinea="";
        DataTable tRec;
        bool editar = false;
        public AgregarZonaLin(string id)
        {
            InitializeComponent();
            radTitleBar1.Text = Lenguaje.traduce("Agregar Línea de Zona");
            traducirNombres();
            radTextBoxIDZonaCab.Text = id;
            generarCamposComboBox();
        }
        public AgregarZonaLin(string id,string idLin,string almacen,string aceraDesde,string acerahasta,string portaldesde,string portalhasta,string pisoDesde, string pisoHasta)
        {
            InitializeComponent();
            radTitleBar1.Text = Lenguaje.traduce("Editar Línea de Zona");
            traducirNombres();
            generarCamposComboBox();
            radTextBoxIDZonaCab.Text = id;
            radTextBoxIDZonaLin.Text = idLin;
            radMultiColumnComboBoxAlmacen.Text = almacen;
            radTextBoxAceraDesde.Text = aceraDesde;
            radTextBoxAceraHasta.Text = acerahasta;
            radTextBoxPisoDesde.Text = pisoDesde;
            radTextBoxPisoHasta.Text = pisoHasta;
            radTextBoxPortalDesde.Text = portaldesde;
            radTextBoxPortalHasta.Text = portalhasta;
            idLinea = idLin;
            editar = true;
        }
        public void generarCamposComboBox()
        {
            try
            {
                tRec = DataAccess.getDataTableSQL(camposAlmacen, campoFormAlmacen, "");
                Utilidades.RellenarMultiColumnComboBox(ref this.radMultiColumnComboBoxAlmacen, tRec, valueMemberAlmacen, displayMemberAlmacen, ""/*idRecursoDefecto.ToString()*/, new String[] { "TODOS" });
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }
        public void traducirNombres()
        {
            
            lbIDZonaCab.Text = Lenguaje.traduce("ID Zona Cab");
            lbIDZonaLin.Text = Lenguaje.traduce("ID Zona Lin");
            lbAceraDesde.Text = Lenguaje.traduce("Acera Desde");
            lbAlmacen.Text = Lenguaje.traduce("Almacen");
            lbAceraHasta.Text = Lenguaje.traduce("Acera Hasta");
            lbPisoDesde.Text = Lenguaje.traduce("Piso Desde");
            lbPisoHasta.Text = Lenguaje.traduce("Piso Hasta");
            lbPortalDesde.Text = Lenguaje.traduce("Portal Desde");
            lbPortalHasta.Text = Lenguaje.traduce("Portal Hasta");
            rumButton1.Text = Lenguaje.traduce("Aceptar");
            rumButton2.Text = Lenguaje.traduce("Cancelar");
        }

        private void rumButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rumButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (editar)
                {
                    LanzarEditar();
                }
                else
                {
                    LanzarNuevaZonaLinea();
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex,ex.Message);
            }
        }
        private void LanzarEditar()
        {
            string error = "";
            try
            {
                int index = radMultiColumnComboBoxAlmacen.SelectedIndex;
                int idAlmacen = tRec.Rows[index].Field<int>("IDHUECOALMACEN");
                string sql = "UPDATE TBLZONALOGLIN SET IDZONACAB=" + radTextBoxIDZonaCab.Text + ", IDZONALIN=" + radTextBoxIDZonaLin.Text + ", IDHUECOALMACEN=" + idAlmacen + ",ACERADESDE=" + radTextBoxAceraDesde.Text +
                ",ACERAHASTA=" + radTextBoxAceraHasta.Text + ",PORTALDESDE=" + radTextBoxPortalDesde.Text + ",PORTALHASTA=" + radTextBoxPortalHasta.Text + ", PISODESDE=" + radTextBoxPisoDesde.Text + ", PISOHASTA=" + radTextBoxPisoHasta.Text + " WHERE IDZONACAB=" + radTextBoxIDZonaCab.Text + " AND IDZONALIN=" + idLinea;
                bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                if (data)
                {
                    MessageBox.Show(Lenguaje.traduce("Se ha realizado Correctamente"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce(error), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
            

        }
        private void LanzarNuevaZonaLinea()
        {
            string error = "";
            try
            {
                int index = radMultiColumnComboBoxAlmacen.SelectedIndex;
                int idAlmacen = tRec.Rows[index].Field<int>("IDHUECOALMACEN");
                string sql = "INSERT INTO TBLZONALOGLIN (IDZONACAB,IDZONALIN,IDHUECOALMACEN,ACERADESDE,ACERAHASTA,PORTALDESDE,PORTALHASTA,PISODESDE,PISOHASTA) VALUES " +
                    "("+radTextBoxIDZonaCab.Text+","+radTextBoxIDZonaLin.Text+","+ idAlmacen + ","+radTextBoxAceraDesde.Text+","+radTextBoxAceraHasta.Text+","+radTextBoxPortalDesde.Text+","
                    +radTextBoxPortalHasta.Text+","+radTextBoxPisoDesde.Text+","+radTextBoxPisoHasta.Text+")";
                bool data = ConexionSQL.SQLClienteExec(sql, ref error);
                if (data)
                {
                    MessageBox.Show(Lenguaje.traduce("Se ha realizado Correctamente"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Lenguaje.traduce(error), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
            
        }
    }
}

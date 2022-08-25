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
using RumboSGA.EmbalajesMotor;
using Rumbo.Core.Herramientas;
using RumboSGAManager.Model;

namespace RumboSGA.Presentation.FormularioRecepciones
{
    public partial class FrmAltaModEmbalaje : Telerik.WinControls.UI.RadForm
    {
        public int idRecepcion;
        private int idMovimiento;
        private DataRow movimiento;
        /**
         * 1 es alta
         * 2 es modificación
         * 
         */
        public FrmAltaModEmbalaje(int idRecepcion_,int idMovimiento_)
        {
            string provDefecto = "";
            string artDefecto = "";
            this.idMovimiento = idMovimiento_;

            InitializeComponent();//TODO revisar si hace falta
            if (idMovimiento <= 0)
            {
                this.Text = Lenguaje.traduce("Alta Embalaje");
            }
            else
            {
                this.Text = Lenguaje.traduce("Modificación Embalaje");
                movimiento = Business.getMovimientoEmbalaje(idMovimiento);
            }
                         
            this.idRecepcion = idRecepcion_;
            btnAceptar.Text = strings.Aceptar;
            btnCancelar.Text = strings.Cancelar;           
            string query1 = "select distinct p.idpedidopro,p.referencia,p.serie " +
                "from tblpedidosprocab p join tblrecepcioneslin l on l.idpedidopro=p.idpedidopro  " +
                "where l.idrecepcion=" + idRecepcion;

            if (movimiento != null)
            {
                provDefecto = movimiento["IDPEDIDOPRO"].ToString();
                artDefecto = movimiento["IDARTICULO"].ToString();
                rTxtCantidad.Text = movimiento["CANTIDAD"].ToString();
            }
            DataTable dtPedidos = ConexionSQL.getDataTable(query1);

            Utilidades.RellenarMultiColumnComboBox(ref rmccmbPedido, dtPedidos, "idpedidopro", "referencia", provDefecto, new String[] {"TODOS"}, true);
            

            string query2 = "select idarticulo,referencia,descripcion,atributo from tblarticulos where idembalaje='EM'";

            DataTable dtArticulos = ConexionSQL.getDataTable(query2);
            rmccmbArticulo.DataSource = dtArticulos.DefaultView;
            Utilidades.RellenarMultiColumnComboBox(ref rmccmbArticulo, dtArticulos, "idarticulo", "referencia", artDefecto, new String[] { "TODOS" }, true);

            if (idMovimiento_ != 0)
            {
                idMovimiento = idMovimiento_;
            }


            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }




        private void btnAceptar_Click(object sender,EventArgs e)
        {
            WSEmbalajeMotorClient cliente = new WSEmbalajeMotorClient();
            try
            {
                int idArticulo = -1;
                int idPedidoPro = -1;
                if (rmccmbArticulo.SelectedValue != null)
                {
                    idArticulo=Convert.ToInt32(rmccmbArticulo.SelectedValue);
                }
                if (rmccmbPedido.SelectedValue != null)
                {
                    idPedidoPro=Convert.ToInt32(rmccmbPedido.SelectedValue);
                }                
                int.TryParse(rTxtCantidad.Text, out int cantidad);


                if (idMovimiento <= 0)
                {
                    cliente.insertarEntradaEmbalaje(idArticulo, "EE", cantidad, idPedidoPro, idRecepcion, "", 0, 0);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    cliente.modificarEntradaEmbalaje(idMovimiento,idArticulo, "EE", cantidad, idPedidoPro, idRecepcion, "", 0, 0);

                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }
               
                
            }
            catch (Exception exc)
            {
                ExceptionManager.GestionarErrorWS(exc,cliente.Endpoint);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

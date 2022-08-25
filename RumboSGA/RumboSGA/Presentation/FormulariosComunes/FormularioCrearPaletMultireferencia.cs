using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGA.RecepcionMotor;
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
using Telerik.WinControls.Data;

namespace RumboSGA.Presentation.FormulariosComunes

{
    public partial class FormularioCrearPaletMultireferencia : Telerik.WinControls.UI.RadForm
    {
        public String resultado;
        private string idPaletTipo;
        public FormularioCrearPaletMultireferencia(String idPaletTipo_)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Crear palet contenedor");
            this.Icon = Resources.bruju;
            resultado = null;
            this.idPaletTipo = idPaletTipo_;
            
        }

        private void CargarComboBox(String idPaletTipo)
        {

            Utilidades.RellenarMultiColumnComboBox(ref radComboBoxTipoPalet, DataAccess.GetIdDescripcionTipoPalets(), "IDPALETTIPO", "DESCRIPCION", idPaletTipo, new String[] { "TODOS" },true);
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacion, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION","", new String[] { "TODOS" }, true);
            Utilidades.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxArticulos, DataAccess.GetIdReferenciaDescripcionAtributoGenericosArticulos(), "IDARTICULO", "DESCRIPCION", "", null, true);


        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            WSRecepcionMotorClient wsRecepcion = new WSRecepcionMotorClient();

            if(!int.TryParse(radMultiColumnComboBoxArticulos.SelectedValue.ToString(), out int idArticulo) &
                !int.TryParse(radMultiColumnComboBoxUbicacion.SelectedValue.ToString(),out int ubicacion))
            {
                MessageBox.Show(Lenguaje.traduce("Error los campos tienen que ser numericos"), Lenguaje.traduce("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            int idMulti = wsRecepcion.generarMultiproducto(idArticulo, ubicacion, 
                radComboBoxTipoPalet.SelectedValue.ToString(), User.IdOperario, 0);
            if (idMulti < 0)
            {
                MessageBox.Show(Lenguaje.traduce("Error creando el contenedor"), Lenguaje.traduce("Resultado"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                resultado = null;
            }
            else
            {
                resultado = idMulti.ToString();
            }
            this.Close();
        }

        private void FormularioCrearPaletMultireferencia_Load(object sender, EventArgs e)
        {
            CargarComboBox(idPaletTipo);
        }

       
    }
}

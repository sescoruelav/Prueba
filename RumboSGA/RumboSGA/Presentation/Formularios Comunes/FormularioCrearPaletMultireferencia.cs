using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGA.RecepcionMotor;
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

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FormularioCrearPaletMultireferencia : Telerik.WinControls.UI.RadForm
    {
        public String resultado;

        public FormularioCrearPaletMultireferencia(String idPaletTipo)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Crear palet contenedor");
            this.Icon = Resources.bruju;
            resultado = null;
            CargarComboBox(idPaletTipo);
        }

        private void CargarComboBox(String idPaletTipo)
        {
            //  FuncionesGenerales.RellenarMultiColumnComboBox(ref radComboBoxTipoPalet, DataAccess.GetIdDescripcionTipoPalets(), "IDPALETTIPO", "DESCRIPCION", idPaletTipo, new String[] { "TODOS" });
            //  FuncionesGenerales.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxUbicacion, DataAccess.GetIdDescripcionHueco(), "IDHUECO", "DESCRIPCION","", new String[] { "TODOS" });
            //  FuncionesGenerales.RellenarMultiColumnComboBox(ref radMultiColumnComboBoxArticulos, DataAccess.GetIdReferenciaDescripcionAtributoArticulos(), "IDARTICULO", "DESCRIPCION", "", new String[] { "TODOS" });
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            WSRecepcionMotorClient wsRecepcion = new WSRecepcionMotorClient();

            if (!int.TryParse(radComboBoxTipoPalet.ValueMember, out int articulo) &
                !int.TryParse(radComboBoxTipoPalet.ValueMember, out int ubicacion))
            {
                MessageBox.Show(Lenguaje.traduce("Error los campos tienen que ser numericos"), Lenguaje.traduce("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            int idMulti = wsRecepcion.generarMultiproducto(articulo, ubicacion,
                radComboBoxTipoPalet.ValueMember, User.IdOperario, 1);
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
    }
}
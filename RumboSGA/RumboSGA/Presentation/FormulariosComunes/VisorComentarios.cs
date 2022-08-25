using Rumbo.Core.Herramientas;
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class VisorComentarios : Telerik.WinControls.UI.RadForm
    {
        
        public VisorComentarios(DataTable tabla)
        {
            InitializeComponent();
            this.Text = Lenguaje.traduce("Visor Comentarios");
            this.ThemeName = ThemeResolutionService.ApplicationThemeName;
            Utilidades.TraducirDataTableColumnName(ref tabla);
            rumGridComentarios.DataSource = tabla;
            rumGridComentarios.BestFitColumns( BestFitColumnMode.AllCells);
            
        }

        private void rumGridComentarios_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string path = rumGridComentarios.CurrentRow.Cells[Lenguaje.traduce("Ruta Imagen")].Value.ToString();
                string nombre = rumGridComentarios.CurrentRow.Cells[Lenguaje.traduce("Nombre Imagen")].Value.ToString();

                if (!path.Equals("") && !nombre.Equals(""))
                {
                    string ruta = path + @"\" + nombre;
                    Image ima = Image.FromFile(ruta);
                    Imagen.Image = ima;
                }
                else
                {
                    Imagen.Image = global::RumboSGA.Properties.Resources.Delete;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(this, Lenguaje.traduce("Error cargando la imagen"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

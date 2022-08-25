using RumboSGA.Presentation.Herramientas.Ventanas;
using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RumboSGA.Controles
{
    public partial class ArchivoJira : UserControl
    {
        List<string> stringlist = new List<string>();
        string link;
        string txt;
        public ArchivoJira(string image, string text,List<string> vs)
        {
            InitializeComponent();
            Rellenarfoto(image, text);
            link = image;
            txt = text;
            stringlist = vs;
        }
        public void Rellenarfoto(string ima,string txt)
        {
            try
            {
                rumLabel1.Text = txt;
                radPictureBox1.Image = new Bitmap(ima);
            }
            catch (Exception ex)
            {
                rumLabel1.Text = txt;
                radPictureBox1.Image = new Bitmap(Image.FromFile(@"C:\Users\tolmeda\Desktop\descarga.png"));
            }
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //cambiarList(stringlist, txt);
            cambiarList();
            //this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        public void cambiarList()
        {
            try
            {
                CrearIncidencia a= (CrearIncidencia)this.ParentForm;
                stringlist= a.GetLista();
                int index = stringlist.IndexOf(link);
                stringlist.RemoveAt(index);
                a.EditarLista(stringlist);
            }
            catch (Exception ex)
            {

                ExceptionManager.GestionarErrorNuevo(ex, ex.Message);
            }
        }
    }
}

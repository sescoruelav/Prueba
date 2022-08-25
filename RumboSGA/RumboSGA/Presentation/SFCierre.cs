using Rumbo.Core.Herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RumboSGA.Presentation
{
    public partial class SFCierre : Form
    {
        public SFCierre()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;

        }


        private void SFCierre_Load(object sender, EventArgs e)
        {

            rumLabelTitulo.Text = Lenguaje.traduce("Estas seguro que quieres salir ");
            this.Text = Lenguaje.traduce("Salir");
            this.TopMost = true;
            this.AutoScroll = false;
            this.AutoScaleMode = AutoScaleMode.None;

        }


        private void RumButtonAceptar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RumButtonCancelar_Click(object sender, EventArgs e)
        {
            Close();
            
        }
      
    }
}

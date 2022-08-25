using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    public partial class RumMessageBox : Form
    {
        public RumMessageBox(string ubicacion)
        {
            InitializeComponent();
            configurarTextBox(ubicacion);
        }
        private void configurarTextBox(string ubi)
        {
            radTextBoxControl1.Multiline = true;
            radTextBoxControl1.AutoScroll = true;
            radTextBoxControl1.HorizontalScrollBarState = ScrollState.AlwaysHide;
            radTextBoxControl1.Text = ubi;
            Font font = new Font(radTextBoxControl1.Font.FontFamily, 10.0f);
            radTextBoxControl1.MaxLength = 5000;
            radTextBoxControl1.Font = font;

        }

        private void rumButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Pruebas
{
    public partial class pruebaBotones : Telerik.WinControls.UI.RadForm
    {
        public pruebaBotones()
        {
            InitializeComponent();
            radButton7.ButtonElement.ShowBorder = false;
        }
    }
}

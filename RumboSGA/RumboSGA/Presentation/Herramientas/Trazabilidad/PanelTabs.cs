using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas
{
    public partial class PanelTabs : Telerik.WinControls.UI.RadTabbedForm
    {
        Panel panel = new Panel() ;
        RadGridView grid=new RadGridView();
        public PanelTabs()
        {
            InitializeComponent();
            this.AllowAero = true;
        }
    }
}

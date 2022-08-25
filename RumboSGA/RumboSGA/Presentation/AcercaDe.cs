using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation
{
    public partial class AcercaDe : Telerik.WinControls.UI.RadForm
    {
        public AcercaDe()
        {
            InitializeComponent();
           radLabel1.Text= XmlReaderPropio.getTextoAcercaDe();
        }
    }
}

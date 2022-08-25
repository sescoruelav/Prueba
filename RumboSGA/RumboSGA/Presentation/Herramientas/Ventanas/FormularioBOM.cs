using RumboSGA.Presentation.UserControls.Mantenimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    public partial class FormularioBOM : Telerik.WinControls.UI.RadForm
    {
        public FormularioBOM()
        {
            InitializeComponent();
            BomControl bomControl = new BomControl();
            this.Controls.Add(bomControl);
            bomControl.Dock = DockStyle.Fill;
            bomControl.NombrarFormulario("BOM");
        }
    }
}

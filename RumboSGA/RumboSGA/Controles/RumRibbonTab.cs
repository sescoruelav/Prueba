using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;
using System.Diagnostics;

namespace RumboSGA.Controles
{
    public partial class RumRibbonTab:RibbonTab
    {
        public override string Text
        {
            get { return base.Text; }
            set
            {
                //if (DesignMode)
                //{
                //    base.Text = value;
                //    Lenguaje.DesingMode = DesignMode;
                //    return;
                //}
                String s = Lenguaje.traduce(value);
                base.Text = s;
            }

        }
    }
}

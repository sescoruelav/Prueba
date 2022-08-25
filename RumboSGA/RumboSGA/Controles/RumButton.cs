using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;
using System.Timers;
using System.Drawing;

namespace RumboSGA
{
    public partial class RumButton:RadButton
    {
        
        public override string Text
        {
            get { return base.Text; }
            set
            {
                String s = Lenguaje.traduce(value);
                base.Text = s;
            }

        }

    }
}

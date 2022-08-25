using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;

namespace RumboSGA.Controles
{
    public partial class RumMenuItem : RadMenuItem
    {
        public int rumMantenimiento;
        public override string Text
        {
            get { return base.Text; }
            set
            {
                string s = value;
                if (s != null && !s.Equals(""))
                {
                    s = Lenguaje.traduce(value);
                }
                
                base.Text = s;
            }

        }
    }
}

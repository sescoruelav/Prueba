using Rumbo.Core.Herramientas;
using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    public partial class RumLabelElement:RadLabelElement
    {

        public RumLabelElement()
        {
            this.BorderVisible = false;
            this.ShouldApplyTheme = false;
        }
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

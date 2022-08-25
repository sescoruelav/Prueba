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
   public partial class RumTextBoxColumn: GridViewTextBoxColumn
    {
        public override string HeaderText
        {
            get { return base.HeaderText; }
            set
            {
                String s = Lenguaje.traduce(value);
                base.HeaderText = s;
            }

        }
        public override string Name
        {
            get { return base.Name; }
            set
            {
                String s = Lenguaje.traduce(value);
                base.Name = s;
            }

        }
    }
}

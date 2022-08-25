using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    public partial class RumTreeViewElement: RadTreeViewElement
    {
        //Enable themeing for the element
        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(RadTreeViewElement);
            }
        }
       
    }
}

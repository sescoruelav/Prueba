using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    public class RadioButtonColumn : GridViewDataColumn
    {
        public RadioButtonColumn(string fieldName)
            : base(fieldName)
        {
        }

        public override Type GetCellType(GridViewRowInfo row)
        {
            if (row is GridViewDataRowInfo)
            {
                return typeof(RadioButtonCellElement);
            }
            return base.GetCellType(row);
        }
    }
}

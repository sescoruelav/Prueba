using RumboSGA.Controles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas
{
    class UtilidadesGui
    {
        public static void ReadOnlyTrueExceptoCheckBoxGridView(ref RumGridView tmp)
        {
            if (tmp == null) return;
            tmp.AllowAddNewRow = false;
            for (int i = 0; i < tmp.Columns.Count; i++)
            {
                if (tmp.Columns[i] is GridViewCheckBoxColumn)
                {
                    tmp.Columns[i].ReadOnly = false;
                }
                else
                {
                    tmp.Columns[i].ReadOnly = true;
                }
            }
            for (int i = 0; i < tmp.Templates.Count; i++)
            {
                for (int j = 0; j < tmp.Templates[i].Columns.Count; j++)
                {
                    if (tmp.Templates[i].Columns[j] is GridViewCheckBoxColumn)
                    {
                        tmp.Templates[i].Columns[j].ReadOnly = false;
                    }
                    else
                    {
                        tmp.Templates[i].Columns[j].ReadOnly = true;
                    }
                }
            }
        }
    }
}

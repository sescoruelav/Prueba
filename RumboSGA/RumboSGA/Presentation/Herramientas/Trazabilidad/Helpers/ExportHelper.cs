using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Helpers
{
    static class ExportHelper
    {

        static public void ExportarExcel(RadGridView gridview)
        {
            FuncionesGenerales.exportarAExcelGenerico(gridview);
        }
    }
}

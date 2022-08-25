using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    public partial class RumDateTimePicker : RadDateTimePicker
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        base.OnMouseDown(e);
                    }));
                }
                else
                {
                    base.OnMouseDown(e);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error al abrir el dateTimePicker de SFDetalles. \nMessage: " + ex.Message + " \nStackTrace:" + ex.StackTrace);
            }
        }

    }
}

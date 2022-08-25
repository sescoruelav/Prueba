using RumboSGAManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Rumbo.Core.Herramientas;
using System.Drawing;
using System.Timers;
using RumboSGAManager;
using Telerik.WinControls.Primitives;
using Telerik.WinControls;

namespace RumboSGA
{
    public partial class RumButtonElement:RadButtonElement
    {
        Timer tRefresco = null;
        bool clickActivo = false;
        public override string Text
        {
            get { return base.Text; }
            set
            {
                this.ShowBorder = false;
                String s = Lenguaje.traduce(value);
                base.Text = s;
                base.ToolTipText = s;
                this.MouseEnter += boton_MouseEnter;
                this.MouseLeave += boton_MouseDown;
                this.EnableRippleAnimation = true;
                this.RippleAnimationColor = Color.FromArgb(204, 255, 255);
                this.EnableFocusBorderAnimation = true;
            }
        }

        public void boton_MouseEnter(object sender, System.EventArgs e)
        {
            // Update the mouse event label to indicate the MouseEnter event occurred.
           this.BackColor = Color.FromArgb(204, 255, 255);
           this.ShowBorder = true;
           this.BorderElement.ForeColor = Color.FromArgb(102, 178, 255);
        }
        public void boton_MouseDown(object sender, System.EventArgs e)
        {
            // Update the mouse event label to indicate the MouseEnter event occurred.
            if (!clickActivo)
            {
                this.BackColor = Color.Transparent;
                this.ShowBorder = false;
            }
  
        }

        public void addRefrescoTimerColor()
        {
            try
            {
                if (this.tRefresco == null)
                {
                    tRefresco = new Timer();
                    tRefresco.Interval = Persistencia.TimerRefrescoCambioColor;
                    tRefresco.Elapsed += TRefresco_Elapsed;
                }
            }catch(Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }
        public void enableRefrescoTimerColor()
        {
            if (this.tRefresco != null && this.tRefresco.Enabled == false)
            {
                tRefresco.Enabled = true;
            }
        }
        public void disableRefrescoTimerColor()
        {
            if (this.tRefresco != null)
            {
                this.BackColor = Color.Transparent;
                this.tRefresco.Stop();
                this.tRefresco = null;
            }
        }

        private void TRefresco_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                tRefresco.Stop();
                if (Persistencia.colorFondoRefresco == null) return;
                if (this.BackColor == Persistencia.colorFondoRefresco)
                {
                    this.BackColor = Color.Transparent;
                }
                else
                {
                    this.BackColor = Persistencia.colorFondoRefresco;
                }
                if (tRefresco != null)
                {
                    tRefresco.Start();
                }
            }catch(Exception ex)
            {
                ExceptionManager.GestionarErrorNuevo(ex);
            }
        }
    }
}

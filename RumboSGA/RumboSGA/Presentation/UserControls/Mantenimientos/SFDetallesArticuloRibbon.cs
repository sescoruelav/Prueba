using RumboSGA.Controles;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class SFDetallesArticuloRibbon : SFDetalles
    {
        private RumPageView rpv;
        private String nombreJson;

        public SFDetallesArticuloRibbon(string nombreJson, string nombre, List<TableScheme> _lstEsquemasParam, dynamic _valuesParam, modoForm _modoAperturaParam, GridScheme _esquemGridParam = null, Hashtable _diccParamNuevoParam = null)
            : base(nombre, _lstEsquemasParam, (object)_valuesParam, _modoAperturaParam, _esquemGridParam, _diccParamNuevoParam)
        {
            this.nombreJson = nombreJson;
        }

        protected override void CargarDatos()
        {
            TablaPanelContenido = new TableLayoutPanel();
            rpv = new RumPageView(nombreJson, _lstEsquemas, this.dataDialog1.PanelControles.Width, false, false);
            TablaPanelContenido.Controls.Add(rpv);

            if (this.dataDialog1.PanelControles.InvokeRequired)
            {
                this.dataDialog1.PanelControles.Invoke((MethodInvoker)delegate
                {
                    this.dataDialog1.PanelControles.Controls.Add(TablaPanelContenido);
                });
            }
            else
            {
                this.dataDialog1.PanelControles.Controls.Add(TablaPanelContenido);
            }
        }
    }
}
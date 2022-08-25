using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class SeleccionarPlanificacion : Telerik.WinControls.UI.RadForm
    {
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        RadRadioButton stock = new RadRadioButton();
        RadRadioButton pedidosCliente = new RadRadioButton();
        RadRadioButton ordenesFabricacion = new RadRadioButton();
        RumButton aceptar = new RumButton();
        RumButton cancelar = new RumButton();
        RadGroupBox grupo = new RadGroupBox();

        public SeleccionarPlanificacion()
        {
            InitializeComponent();
            this.Shown += form_Shown;
            this.Show();
            this.Size = new Size(250,150);
            stock.Text = strings.PlanificacionStock;
            pedidosCliente.Text = strings.PedidosCliente;
            ordenesFabricacion.Text = strings.OrdenesFabricacion;
            aceptar.Text = strings.Aceptar;
            cancelar.Text = strings.Cancelar;
            grupo.Controls.Add(stock);
            grupo.Controls.Add(pedidosCliente);
            grupo.Controls.Add(ordenesFabricacion);
            stock.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            cancelar.Click += cancelar_Event;
            tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayout.Controls.Add(stock,0,0);
            tableLayout.Controls.Add(pedidosCliente,0,1);
            tableLayout.Controls.Add(ordenesFabricacion,0,2);
            tableLayout.Controls.Add(aceptar,0,3);
            tableLayout.Controls.Add(cancelar,1,3);
            this.Controls.Add(tableLayout);
        }

        private void cancelar_Event(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aceptar_Event(object sender, EventArgs e)
        {
            if (stock.IsChecked)
            {

            }
            else if(pedidosCliente.IsChecked)
            {

            }
            else
            {

            }
        }
        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }
    }
}

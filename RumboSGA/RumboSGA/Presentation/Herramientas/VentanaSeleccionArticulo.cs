using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas
{
    public partial class VentanaSeleccionArticulo : Telerik.WinControls.UI.ShapedForm
    {
        GroupBox panel = new GroupBox();
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        RadTextBox textBoxArticulo = new RadTextBox();
        RadDateTimePicker fecha = new RadDateTimePicker();
        RadLabel labelArticulo = new RadLabel();
        RadLabel labelFecha = new RadLabel();
        RadButton btnAceptar = new RadButton();
        RadButton btnCancelar = new RadButton();
        RadButton btnSeleccion = new RadButton();
        string query;
        public VentanaSeleccionArticulo(string queryRecibida)
        {
            InitializeComponent();
            this.Shown += form_Shown;
            this.Show();
            query = queryRecibida;
            this.Size = new Size(500,200);
            this.MaximumSize = new Size(500, 200);
            panel.Text = "Fecha";
            labelArticulo.Text = "Articulo";
            labelFecha.Text = "Fecha";
            btnAceptar.Text = "Aceptar";
            btnCancelar.Text = "Cancelar";
            btnSeleccion.Text = "...";
            panel.Dock = DockStyle.Fill;
            textBoxArticulo.Dock = DockStyle.Fill;
            tableLayoutPanel.Dock = DockStyle.Fill;
            fecha.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(textBoxArticulo,2,0);
            tableLayoutPanel.Controls.Add(fecha, 2, 1);
            tableLayoutPanel.Controls.Add(labelArticulo, 1, 0);
            tableLayoutPanel.Controls.Add(labelFecha, 1, 1);
            tableLayoutPanel.Controls.Add(btnAceptar,1,2);
            tableLayoutPanel.Controls.Add(btnCancelar,2,2);
            tableLayoutPanel.Controls.Add(btnSeleccion, 3, 0);

            btnSeleccion.Click += btnSeleccion_Event;
            btnCancelar.Click += btnCancelar_Event;
            btnAceptar.Click += btnAceptar_Event;
            this.Controls.Add(tableLayoutPanel);
        }
        private void btnSeleccion_Event(object sender, EventArgs e)
        {
            //string query = "SELECT idarticulo,referencia,descripcion,atributo FROM DEMO..TBLARTICULOS";
            VentanaGrid grid = new VentanaGrid(query);
            grid.GridViewBase.MouseDoubleClick += grid.OnDoubleClick_Cerrar;
            grid.ShowDialog();
            string[] text=new string[grid.GridViewBase.ColumnCount];
            for (int i = 0; i < grid.GridViewBase.ColumnCount; i++)
            {
                text[i]= grid.GridViewBase.CurrentRow.Cells[i].Value.ToString()+" ";
            }
            textBoxArticulo.Text = string.Concat(text);
        }
        private void btnCancelar_Event(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAceptar_Event(object sender,EventArgs e)
        {

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

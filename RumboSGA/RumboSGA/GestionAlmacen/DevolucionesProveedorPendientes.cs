using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.GestionAlmacen
{
    public partial class DevolucionesProveedorPendientes : Telerik.WinControls.UI.RadForm
    {
        GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
        public DevolucionesProveedorPendientes()
        {
            InitializeComponent();
            this.Name = NombresFormularios.DevolucionesProPendientes;
            this.Shown += form_Shown;
            this.Show();

            btnVerDevolucion.Click += salir_Event;
            btnSalir.Click += salir_Event;
            radGridView1.Columns.Add(checkBoxColumn);
            checkBoxColumn.EditMode = EditMode.OnValueChange;
            DataTable dt = ConexionSQL.getDataTable("SELECT * FROM TBLDEVOLPROCAB WHERE IDDEVOLPROESTADO not like 'PC'");
            Utilidades.TraducirDataTableColumnName(ref dt);
            radGridView1.DataSource = dt;
            radGridView1.BestFitColumns();
            radGridView1.ValueChanged += checkBoxValueChanged;
            btnVerDevolucion.Text = Lenguaje.traduce(btnVerDevolucion.Text);
            btnRefrescar.Text = Lenguaje.traduce(btnRefrescar.Text);
            btnSalir.Text = Lenguaje.traduce(btnSalir.Text);
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }
        private void ConfigurarGrid()
        {
            radGridView1.AllowAddNewRow = false;
            foreach (var row in radGridView1.Rows)
            {
                if ((bool)row.Cells[0].Value==true)
                {
                    Debug.WriteLine(row.Cells["IDDEVOLPRO"].Value);
                    Debug.WriteLine(row.Cells["REFERENCIA"].Value);
                    Debug.WriteLine(row.Cells["IDDEVOLPROESTADO"].Value);
                    Debug.WriteLine(row.Cells["IDDEVOLPRO"].Value);
                    Debug.WriteLine(row.Cells["IDDEVOLPRO"].Value);
                }
            }
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refrescar_Event(object sender,EventArgs e)
        {
            DataTable dt = Business.GetDevolucionesProveedor();
            Utilidades.TraducirDataTableColumnName(ref dt);
            radGridView1.DataSource = dt;
            radGridView1.BestFitColumns();
        }
        private void checkBoxValueChanged(object sender,EventArgs e)
        {
            try
            {
                if (this.radGridView1.ActiveEditor is RadCheckBoxEditor)
                {
                    Debug.WriteLine(this.radGridView1.CurrentCell.RowIndex);
                    Debug.WriteLine(this.radGridView1.ActiveEditor.Value);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex);
                //log.Error("Mensaje:" + ex.Message + "\n StackTrace:" + ex.StackTrace);
            }
        }

    }
}

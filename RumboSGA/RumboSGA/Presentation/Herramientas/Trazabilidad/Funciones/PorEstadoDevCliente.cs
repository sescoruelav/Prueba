using Rumbo.Core.Herramientas;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Herramientas.Trazabilidad.Funciones
{
    public partial class PorEstadoDevCliente : Telerik.WinControls.UI.RadForm
    {
        string path = Persistencia.DirectorioBase + @"\configs\Trazabilidad.xml";
        List<string> estados = new List<string>();
        RadGridView gridDevCliente;
        private string nombreFormulario;

        public PorEstadoDevCliente(RadGridView gridDevCliente, string nombreFormulario)
        {
            InitializeComponent();
            this.gridDevCliente = gridDevCliente;
            this.Text = Lenguaje.traduce(nombreFormulario);
            LlenarDiccionarioEstados();
            estadosList.DataSource = estados;
            this.ShowIcon = false;
            this.Shown += form_Shown;
            this.Show();
         
            
   
            btnAceptar.Click += aceptar_Event;
            btnSalir.Click += salir_Event;

        }

        private void LlenarDiccionarioEstados()
        {
            DataTable table = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(ConfigurarSQL.CargarConsulta(path, "LLENARDICCIONARIOESTADOSDEVCLIENTE")));
            for (int i = 0; i < table.Rows.Count; i++)
            {
                estados.Add(table.Rows[i][0].ToString());
            }
        }

        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }

        public void aceptar_Event(object sender, EventArgs e)
        {
            if (estadosList.SelectedItem!=null)
            {
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE DC.IDDEVOLCLIESTADO = '" + estadosList.SelectedItem.Text + "'";

            }
            else
            {
                DevolucionesCliente.q = ConfigurarSQL.CargarConsulta(path, "DEVOLUCIONESCLIENTE") + " WHERE DC.IDDEVOLCLIESTADO is null";

            }
            gridDevCliente.DataSource = Utilidades.TraducirDataTableColumnNameDT(ConexionSQL.getDataTable(DevolucionesCliente.q)).DefaultView;
            this.Close();
        }

        private void salir_Event(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

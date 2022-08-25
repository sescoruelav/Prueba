using Newtonsoft.Json;
using Rumbo.Core.Herramientas;
using RumboSGA.Properties;
using RumboSGA.RecursoMotor;
using RumboSGA.ReservaMotor;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
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

namespace RumboSGA.Herramientas
{
    public partial class Reposiciones : Telerik.WinControls.UI.ShapedForm
    {
        RadGridView grid = new RadGridView();
        WSRecursoMotorClient ws = null;
        WSReservaMotorClient ws2 = null;
        public Reposiciones()
        {
            InitializeComponent();
            
            this.WindowState = FormWindowState.Maximized;
            btnActualizar.Text = Lenguaje.traduce(strings.Actualizar);
            btnSoloConflictos.Text = Lenguaje.traduce(strings.SoloConflictos);
            btnGenerarReposicion.Text = Lenguaje.traduce(strings.GenerarRepos);
            btnAsignarRecurso.Text = Lenguaje.traduce(strings.AsignarRecurso);
            btnSalir.Text = Lenguaje.traduce(strings.Salir);
            grid.AllowAddNewRow = false;
            grid.ReadOnly = true;
            grid.Dock = DockStyle.Fill;
            grid.BestFitColumns();
            grid.EnableFiltering = true;
            grid.MasterTemplate.EnableFiltering = true;
            grid.EnablePaging = false;
            DataTable dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesActivadoZona);
            Utilidades.TraducirDataTableColumnName(ref dt);
            grid.DataSource = dt;
            grid.Dock = DockStyle.Fill;
            grid.BestFitColumns();
            ConditionalFormattingObject obj = new ConditionalFormattingObject(Lenguaje.traduce("Mi Condición"), ConditionTypes.Greater, "0", "", true);
            obj.RowForeColor = Color.Green;
            ConditionalFormattingObject obj2 = new ConditionalFormattingObject(Lenguaje.traduce("Mi Condición2"), ConditionTypes.Equal, "0", "", true);
            obj2.RowBackColor = Color.FromArgb(218, 213, 212);
            obj2.RowForeColor = Color.Red;
            

            tableLayoutPanel1.Controls.Add(grid,0,1);
            tableLayoutPanel1.SetColumnSpan(grid, 7);
            
            this.Shown += form_Shown;
            this.Show();

            this.grid.MasterTemplate.Columns["Total"].ConditionalFormattingObjectList.Add(obj);
            this.grid.MasterTemplate.Columns["Total"].ConditionalFormattingObjectList.Add(obj2);

        }
        private void form_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }


        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DataTable dt = ConexionSQL.getDataTable(SentenciasSQL.queryReposicionesActivadoZona);
            Utilidades.TraducirDataTableColumnName(ref dt);
            grid.DataSource = dt;
        }

        private void panelGrid_Paint(object sender, PaintEventArgs e)
        {
            
            
        }

        private void btnGenerarReposicion_Click(object sender, EventArgs e)
        {
            ws = new WSRecursoMotorClient();
            ws2 = new WSReservaMotorClient();
            try
            {

                string json = crearJson();
                if (ws.isActivadoReposicionPorZona())
                {
                                        
                    ws2.generarReservasZona(json);
                }
                else
                {
                    if (ws.isActivadoRellenarUbPicking())
                    {
                        ws2.generarReservasHPCapacidad(json);
                    }
                    else
                    {
                        ws2.generarReservas(1, json);
                    }
                }


               
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }

        private void btnSoloConflictos_Click(object sender, EventArgs e)
        {
            ws = new WSRecursoMotorClient();
            int[] idArticulos = new int[1];
            try
            {
                bool activadoPorZona = ws.isActivadoReposicionPorZona();
                DataTable dt = null;
                if (activadoPorZona)
                {
                     dt = ConexionSQL.getDataTable(SentenciasSQL.querySoloConflictosActivadoPorZona);
                }
                else
                {
                    dt = ConexionSQL.getDataTable(SentenciasSQL.querySoloConflictosNoActivadoPorZona);
                }
                Utilidades.TraducirDataTableColumnName(ref dt);
                grid.DataSource = dt;
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
        }


        private void btnAsignarRecurso_Click(object sender, EventArgs e)
        {

            ws = new WSRecursoMotorClient();
            int[] idArticulos = new int[1];
            try
            {

            String valor = grid.SelectedRows[0].Cells["idarticulo"].Value.ToString();
            int valor2 = int.Parse(valor);
            idArticulos[0] = valor2;

             ws.asignarRecursoReposicion(idArticulos);
            }
            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }


        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string crearJson()
        {
            string json = "[{\r\n \"idarticulo\":" + grid.SelectedRows[0].Cells[0].Value.ToString() +
                        ", \r\n\"referencia\":" + grid.SelectedRows[0].Cells[1].Value.ToString() +
                        "\r\n\"descripcion\":" + grid.SelectedRows[0].Cells[2].Value.ToString() +
                        "\r\n\"hpicking\":" + grid.SelectedRows[0].Cells[3].Value.ToString() +
                        "\r\n\"stockMinReab\":" + grid.SelectedRows[0].Cells[4].Value.ToString() +
                        "\r\n\"ExistenciasPI\":" + grid.SelectedRows[0].Cells[5].Value.ToString() +
                        "\r\n\"ExistenciasUB\":" + grid.SelectedRows[0].Cells[6].Value.ToString() +
                        "\r\n\"ReservasPI\":" + grid.SelectedRows[0].Cells[7].Value.ToString() +
                        "\r\n\"ReservasRP\":" + grid.SelectedRows[0].Cells[8].Value.ToString() +
                        "\r\n\"Total\":" + grid.SelectedRows[0].Cells[9].Value.ToString() +
                         "\r\n}]";
            return json;
        }
    }
}

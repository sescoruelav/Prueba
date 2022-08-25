using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace EstadoRecepciones
{
    public partial class clasificacionEntradasForm : Telerik.WinControls.UI.RadRibbonForm
    {
        protected System.Windows.Forms.Panel panel;
        public RadGridView gridViewLineas = new RadGridView();
        public RadGridView gridViewCabezera = new RadGridView();
        public Telerik.WinControls.UI.RadRibbonBar radRibbonBar;
        public RadRibbonBarGroup grupoBotonesAccion = new RadRibbonBarGroup();
        public RadRibbonBarGroup grupoBotonesEstilos = new RadRibbonBarGroup();
        public RadButtonElement btnCalculo = new RadButtonElement();
        public RadButtonElement btnConfirmar = new RadButtonElement();
        public RadButtonElement btnEditColumns = new RadButtonElement();
        public RadButtonElement btnguardarColumnas = new RadButtonElement();
        public RadButtonElement btnCargarColumnas = new RadButtonElement();
        protected RibbonTab rtAcciones = new RibbonTab();
        protected RibbonTab rtEstilos = new RibbonTab();
        private static string directorioLocal = @"C:\Rumbo\RumboEstilos";
        private static string nombreEstiloGridView = string.Empty;

        private Label lblNombre = new Label();

        public clasificacionEntradasForm()
        {
            InitializeComponent();
        }

        public clasificacionEntradasForm(string json)
        {
            try
            {
                InitializeComponent();
                this.radRibbonBar = new Telerik.WinControls.UI.RadRibbonBar();
                this.panel = new System.Windows.Forms.Panel();

                ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar)).BeginInit();
                //
                // radRibbonBar
                //
                this.radRibbonBar.ExitButton.Text = "Exit";
                this.radRibbonBar.LocalizationSettings.LayoutModeText = "Simplified Layout";
                this.radRibbonBar.Location = new System.Drawing.Point(0, 0);
                this.radRibbonBar.Name = "radRibbonBar";

                this.radRibbonBar.OptionsButton.Text = "Options";
                this.radRibbonBar.Size = new System.Drawing.Size(1378, 164);
                this.radRibbonBar.TabIndex = 3;
                ((Telerik.WinControls.UI.RadRibbonBarElement)(this.radRibbonBar.GetChildAt(0))).Text = "";
                ((Telerik.WinControls.UI.RadApplicationMenuButtonElement)(this.radRibbonBar.GetChildAt(0).GetChildAt(5))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                this.Controls.Add(this.radRibbonBar);
                this.radRibbonBar.Dock = DockStyle.Top;

                //
                //
                this.panel.Size = new System.Drawing.Size(750, 300);
                this.panel.TabIndex = 0;
                this.panel.Dock = DockStyle.Top;
                // this.panel.AutoSize = true;

                //this.radGridView.Size = new System.Drawing.Size(750, 250);
                this.gridViewLineas.Dock = DockStyle.Fill;
                this.gridViewLineas.AutoScroll = true;
                this.gridViewLineas.ReadOnly = true;
                this.gridViewLineas.EnableFiltering = true;
                this.gridViewLineas.MasterTemplate.EnableFiltering = true;
                this.gridViewLineas.BestFitColumns();
                this.gridViewLineas.AllowColumnReorder = true;
                this.gridViewLineas.AllowDragToGroup = true;

                this.Controls.Add(panel);
                this.Controls.Add(radRibbonBar);
                this.gridViewCabezera.Dock = DockStyle.Top;

                //Botones

                this.btnCalculo.Text = "Calculo Destinos";
                this.btnCalculo.BorderHighlightColor = Color.Black;

                this.btnConfirmar.Text = "Confirmar";
                this.btnConfirmar.BorderHighlightColor = Color.Black;

                this.btnEditColumns.Text = "Editar Estilo";
                this.btnEditColumns.Click += ItemColumnas_Click;

                this.btnguardarColumnas.Text = "Guardar Estilo";
                this.btnguardarColumnas.Click += SaveItem_Click;

                this.btnCargarColumnas.Text = "Cargar Estilo";
                this.btnCargarColumnas.Click += LoadItem_Click;

                //Tag con los botones accion
                grupoBotonesAccion.Items.Add(btnCalculo);
                grupoBotonesAccion.Items.Add(btnConfirmar);

                grupoBotonesEstilos.Items.Add(btnEditColumns);
                grupoBotonesEstilos.Items.Add(btnguardarColumnas);
                grupoBotonesEstilos.Items.Add(btnCargarColumnas);

                rtAcciones.Text = "Accion";
                rtAcciones.Items.Add(grupoBotonesAccion);
                rtAcciones.Items.Add(grupoBotonesEstilos);

                radRibbonBar.CommandTabs.Add(rtAcciones);
                this.panel.Controls.Add(gridViewLineas);

                // this.panel.Controls.Add(gridViewCabezera);

                ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar)).EndInit();

                ConversionJsonTabla(json);
                Utilidades.LoadLayout(directorioLocal, gridViewLineas, this.ProductName);
            }
            catch (Exception e)
            {
                MessageBox.Show("No se ha podido cargar bien la ventana");
            }
        }

        private void LoadItem_Click(object sender, EventArgs e)
        {
            Utilidades.LoadLayout(directorioLocal, gridViewLineas, this.ProductName);
        }

        private void SaveItem_Click(object sender, EventArgs e)
        {
            Utilidades.SaveLayout(directorioLocal, gridViewLineas, this.ProductName);
        }

        public void ConversionJsonTabla(string json)
        {
            try
            {
                DataTable tablaRecepciones = new DataTable();
                DataTable tablaLineas = new DataTable();

                string[] words = json.Split(']');
                List<string> tablas = new List<string>();

                foreach (string word in words)
                {
                    tablas.Add(word + "]");
                }
                tablaRecepciones = JsonConvert.DeserializeObject<DataTable>(tablas[0]);
                tablaLineas = JsonConvert.DeserializeObject<DataTable>(tablas[1]);

                foreach (DataColumn item in tablaRecepciones.Columns)
                {
                    bool repetido = false;

                    DataColumn column = new DataColumn();
                    column = item;
                    foreach (DataColumn columnaLineas in tablaLineas.Columns)
                    {
                        if (column.ColumnName == columnaLineas.ColumnName)
                        {
                            repetido = true;
                        }
                    }
                    if (!repetido)
                    {
                        tablaLineas.Columns.Add(column.ColumnName, typeof(String));
                    }
                }
                for (int filas = 0; filas < tablaLineas.Rows.Count; filas++)
                {
                    for (int i = 0; i < tablaLineas.Columns.Count; i++)
                    {
                        for (int x = 0; x < tablaRecepciones.Columns.Count; x++)
                        {
                            if (tablaLineas.Columns[i].ColumnName == tablaRecepciones.Columns[x].ColumnName)
                            {
                                tablaLineas.Rows[filas][tablaRecepciones.Columns[x].ColumnName] = tablaRecepciones.Rows[0][tablaRecepciones.Columns[x].ColumnName];
                            }
                        }
                    }
                }
                // gridViewCabezera.DataSource = tablaRecepciones;
                gridViewLineas.DataSource = tablaLineas;
            }
            catch (Exception e)
            {
                MessageBox.Show("No se ha podido cargar Los datos en las tablas");
            }
        }

        public void ItemColumnas_Click(object sender, EventArgs e)
        {
            //log.Info("Pulsado botón editar columnas " + DateTime.Now);
            /* (panel.Controls.Contains(gridView))
             {
             }*/

            gridViewLineas.ShowColumnChooser();
        }

        private void radGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
using Rumbo.Core.Herramientas;
using RumboSGA.Presentation.UserControls;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Herramientas.Stock
{
    public partial class CambiarAtributos : Telerik.WinControls.UI.ShapedForm
    {
        TableLayoutPanel tablaPanelControles = new TableLayoutPanel();
        public bool imprimirEtiq;
        public bool reservaRecurso;
        public Hashtable atributos = new Hashtable();
        public CambiarAtributos()
        {
            InitializeComponent();

            this.Text = Lenguaje.traduce("Cambiar Atributos");

            opcionesReservaGroup.Text = Lenguaje.traduce(strings.OpcionesEntrada);
            opcionesImpresionGroup.Text = Lenguaje.traduce(strings.OpcionesImpresion);
            entradaNormal.Text = strings.EntradaNormal;
            entradaReserva.Text = strings.ReservaRecurso;
            imprimirEtiqueta.Text = strings.ImprimirEtiq;
            noImprimirEtiqueta.Text = strings.NoImpr;
            entradaNormal.CheckStateChanged += opcionesReserva_Change;
            entradaReserva.CheckStateChanged += opcionesReserva_Change;
            imprimirEtiqueta.CheckStateChanged += opcionesImpresion_Change;
            noImprimirEtiqueta.CheckStateChanged += opcionesImpresion_Change;
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
            entradaNormal.IsChecked = true;
            imprimirEtiqueta.IsChecked = true;
            imprimirEtiq = false;
            reservaRecurso = false;
            ////Business.GetParametrosControles(ref _lstEsquemas);
            //this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent,10F));
            this.tablaPanelControles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tablaPanelControles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tablaPanelControles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tablaPanelControles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));

            //this.tablaPanelControles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));

            panelControles.Controls.Add(tablaPanelControles);
            tablaPanelControles.Dock = DockStyle.Fill;

            this.panelControles.Padding = new Padding(10, 10, 10, 10);

            GetControles();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowIcon = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
        }
        private int GetMayorAltoFila(List<TableScheme> _lstEsquemas, int numFila)
        {
            int alto = 0;

            for (int i = 0; i < _lstEsquemas.Count; i++)
            {
                if (_lstEsquemas[i].Alto > alto && _lstEsquemas[i].Fila == numFila)
                {
                    alto = _lstEsquemas[i].Alto;
                }
            }

            return alto;
        }
        private void GetControles()
        {
          DataTable tablaControles = DataAccess.getRumControles(GetIDFormulario());
            
            foreach (DataRow fila in tablaControles.Rows)
            {
                ElegirControl(fila);
            }
        }
        private void ElegirControl(DataRow fila)
        {
            RadCheckBox check = new RadCheckBox();
            string tipo = fila["TIPO"].ToString();
            int x = (int)fila["POSX"];
            int y = (int)fila["POSY"];
            string nombre= fila["NOMBRE"].ToString();
            check.Name =nombre;
            check.ToggleStateChanged += Check_Event;

            switch (tipo)
            {
                case "L":
                    RadLabel lbl = new RadLabel();
                    lbl.AutoSize = true;
                    lbl.Text = Lenguaje.traduce(fila["PREDETERMINADO"].ToString());
                    lbl.Name = "label_"+nombre;
                    tablaPanelControles.Controls.Add(lbl,x,y);
                    break;
                case "G":
                    RadMultiColumnComboBox combo = new RadMultiColumnComboBox();

                    string query = fila["PREDETERMINADO"].ToString().Replace("@", "");
                    DataTable dt = ConexionSQL.getDataTable(query);
                    Utilidades.TraducirDataTableColumnName(ref dt);
                    combo.DataSource = dt;

                    combo.Dock = DockStyle.Fill;
                    combo.AutoSize = true;
                    combo.MultiColumnComboBoxElement.BestFitColumns();
                    combo.MultiColumnComboBoxElement.AutoSize = true;
                    combo.Name= nombre;
                    combo.MultiColumnComboBoxElement.AutoCompleteMode = AutoCompleteMode.Append;
                    combo.Enabled=false;
                    tablaPanelControles.RowCount += 1;
                    tablaPanelControles.Controls.Add(combo,x,y);
                    tablaPanelControles.SetColumnSpan(combo,2);
                    tablaPanelControles.Controls.Add(check, 0, y);

                    break;
                case "T":
                    RadTextBox tBox = new RadTextBox();
                    tablaPanelControles.Controls.Add(tBox,x,y);
                    tBox.Dock = DockStyle.Fill;
                    tBox.Name = nombre;
                    tBox.Enabled = false;
                    tablaPanelControles.RowCount += 1;
                    tablaPanelControles.Controls.Add(check, 0, y);

                    break;
                default:
                    break;
            }
        }
        private int GetIDFormulario()
        {
            return 20060;
        }
        private void Check_Event(object sender,EventArgs e)
        {
            RadCheckBox a = (RadCheckBox)sender;
            var controls = tablaPanelControles.Controls;
            foreach (Control control in controls)
            {
                Debug.WriteLine(control.Name);

            }
            RadTextBox tBox =tablaPanelControles.Controls.Find(a.Name,true).FirstOrDefault() as RadTextBox;
            if (tBox!=null)
            {
                if (a.Checked)
                {
                    tBox.Enabled = true;
                }
                else
                {
                    tBox.Enabled = false;
                }
            }
            else
            {
                RadMultiColumnComboBox combo = tablaPanelControles.Controls.Find(a.Name,true).FirstOrDefault() as RadMultiColumnComboBox;
                if (combo!=null)
                {
                    if (a.Checked)
                    {
                        combo.Enabled = true;
                    }
                    else
                    {
                        combo.Enabled = false;
                    }
                }
            }

        }
        private void btnAceptar_Click(object sender,EventArgs e)
        {
            int cont=0;
            atributos.Clear();
            foreach (Control control in tablaPanelControles.Controls)
            {
                if (cont==tablaPanelControles.RowCount)
                {
                    break;
                   //MessageBox.Show(control.Name+" " + tablaPanelControles.Controls.Count);
                }
                RadCheckBox check = tablaPanelControles.GetControlFromPosition(0, cont) as RadCheckBox;
                if (control is RadCheckBox)
                {
                    Debug.WriteLine("check_"+control.Name);
                }
                else
                {
                    Debug.WriteLine(control.Name + "|||" + control.Text);

                }

                if (check.Checked)
                 {
                    if (!(control is RadLabel)&&!(control is RadCheckBox))
                    {
                        if (control is RadTextBox)
                        {
                            atributos.Add(control.Name, control.Text);
                        }
                        else
                        {
                            RadMultiColumnComboBox gridControl = control as RadMultiColumnComboBox;
                            gridControl.ValueMember = gridControl.Columns[0].Name;
                            atributos.Add(gridControl.Name,gridControl.SelectedValue);
                        }
                    }
                }
                //if (control is RadTextBox||control is RadMultiColumnComboBox)
                //{
                //    cont++;
                //}
                if (control is RadCheckBox)
                {
                    cont++;
                }

            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCancelar_Click(object sender,EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void opcionesImpresion_Change(object sender,EventArgs e)
        {
            if (imprimirEtiqueta.IsChecked)
            {
                imprimirEtiq = true;
            }
            else
            {
                imprimirEtiq = false;
            }
        }
        private void opcionesReserva_Change(object sender,EventArgs e)
        {
            if (entradaReserva.IsChecked)
            {
                reservaRecurso = true;
            }
            else
            {
                reservaRecurso = false;
            }
        }
    }
}

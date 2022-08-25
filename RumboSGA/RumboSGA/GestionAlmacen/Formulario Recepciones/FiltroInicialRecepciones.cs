using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumboSGA.Properties;
using RumboSGAManager;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.Formulario_Recepciones
{
    public partial class FiltroInicialRecepciones : Telerik.WinControls.UI.RadForm
    {
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        RadDropDownList cmb;
        string queryConstr = " WHERE ";
        List<TableScheme> esquema;
        string path = Persistencia.ConfigPath + @"\FiltroInicial.xml";

        public FiltroInicialRecepciones(string nombreJson)
        {
            InitializeComponent();
            string json = DataAccess.LoadJson(nombreJson);
            JArray js = JArray.Parse(json);
            string jsonEsquema = js.First()["Scheme"].ToString();
            esquema = JsonConvert.DeserializeObject<List<TableScheme>>(jsonEsquema);
            this.radScrollablePanel1.Controls.Add(tableLayoutPanel);
            this.radScrollablePanel1.VerticalScroll.Enabled = true;
            //tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Size = radScrollablePanel1.Size;
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowOnly;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel.ColumnCount = 3;
            btnAceptar.Text = strings.Aceptar;
            btnBorrar.Text = strings.Borrar;
            btnBorrar.Click += btnBorrar_Click;
            //extraerEsquemaJson("ProveedoresPedidosCab");
            cargarFiltros();


            //Añadir();
        }

        private RadDropDownList crearComboStrings()
        {

            RadDropDownList tb = new RadDropDownList();
            RadListDataItem dataItem = new RadListDataItem();
            RadListDataItem dataItem2 = new RadListDataItem();
            RadListDataItem dataItem3 = new RadListDataItem();

            RadListDataItem dataItem4 = new RadListDataItem();
            RadListDataItem dataItem5 = new RadListDataItem();
            RadListDataItem dataItem6 = new RadListDataItem();
            RadListDataItem dataItem7 = new RadListDataItem();


            dataItem.Text = "Like";
            dataItem2.Text = "=";
            dataItem3.Text = "<";
            dataItem4.Text = ">";
            dataItem5.Text = "<=";
            dataItem6.Text = ">=";
            dataItem7.Text = "<>";

            tb.Items.Add(dataItem);
            tb.Items.Add(dataItem2);
            tb.Items.Add(dataItem3);
            tb.Items.Add(dataItem4);
            tb.Items.Add(dataItem5);
            tb.Items.Add(dataItem6);
            tb.Items.Add(dataItem7);

            return tb;
        }
        private void crearComboInt()
        {

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            bool guardar = true;
            for (int i=0;i<=tableLayoutPanel.RowCount;i++)
            {
                string valor=string.Empty;
                if (tableLayoutPanel.GetControlFromPosition(0,i) is RadDropDownList)
                {
                    RadDropDownList temp = (RadDropDownList)tableLayoutPanel.GetControlFromPosition(0, i);
                    RadDropDownList temp2 = (RadDropDownList)tableLayoutPanel.GetControlFromPosition(1, i);
                    switch(tableLayoutPanel.GetControlFromPosition(2, i).GetType().ToString())
                    {
                        case "Telerik.WinControls.UI.RadTextBox":
                            RadTextBox tb = (RadTextBox)tableLayoutPanel.GetControlFromPosition(2,i);
                            valor = "'"+tb.Text+"'";
                            break;
                        case "Telerik.WinControls.UI.RadSpinEditor":
                            RadSpinEditor spin = (RadSpinEditor)tableLayoutPanel.GetControlFromPosition(2, i);
                            valor = spin.Value.ToString();
                            break;
                        case "Telerik.WinControls.UI.RadDateTimePicker":
                            RadDateTimePicker date = (RadDateTimePicker)tableLayoutPanel.GetControlFromPosition(2, i);
                            valor = date.Value.ToString();
                            break;

                    }
                    if (valor==string.Empty)
                    {
                        guardar = false;
                    }
                    else
                    {
                        queryConstr += temp.SelectedItem.Tag + " " + temp2.Text + " " + valor + " ";

                    }

                }
                else
                {
                    if (tableLayoutPanel.GetControlFromPosition(0, i) is RadLabel)
                    {
                        RadLabel lb = (RadLabel)tableLayoutPanel.GetControlFromPosition(0, i);
                        queryConstr += " " + lb.Text + " ";
                    }
                }

            }
            if (guardar)
            {
                guardarFiltro();
                //MessageBox.Show(queryConstr);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(strings.FiltroValorVacio);
            }

        }

        //private void btnAñadir_Click(object sender, EventArgs e)
        //{
        //    Añadir();

        //}
        private void Añadir()
        {
            //this.tableLayoutPanel.Size = new Size(this.tableLayoutPanel.Size.Width, this.tableLayoutPanel.Size.Height + 40);
            //lbl.Text = esquema[i].Etiqueta.ToString();
            //lbl.Tag = esquema[i].Nombre;
            int indice = tableLayoutPanel.RowCount;
            RadDropDownList campo = extraerEsquemaJson("ProveedoresPedidosCab");


            RadDropDownList operador = crearComboStrings();
           

            tableLayoutPanel.Controls.Add(campo,0, indice);
            tableLayoutPanel.Controls.Add(operador, 1, indice);
            campo.SelectedIndexChanged += DropDownList_ValueChanged;
            campo.SelectedIndex = 0;
            campo.Dock = DockStyle.Fill;
            operador.Dock = DockStyle.Fill;
            operador.SelectedIndex = 0;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowCount++;
        }
        private void AñadirXML(string campo,string operador,string valor)
        {
            //this.tableLayoutPanel.Size = new Size(this.tableLayoutPanel.Size.Width, this.tableLayoutPanel.Size.Height + 40);
            //lbl.Text = esquema[i].Etiqueta.ToString();
            //lbl.Tag = esquema[i].Nombre;
            int indice = tableLayoutPanel.RowCount;
            RadDropDownList campos = extraerEsquemaJson("ProveedoresPedidosCab");
            RadDropDownList operadores = crearComboStrings();
            //int indexCampo = campos.FindString(campo);

            for (int i = 0; i < campos.Items.Count; i++)
            {
                if (campo == campos.Items[i].Tag.ToString())
                {
                    campos.SelectedIndex = i;
                    break;
                }
            }



            foreach (TableScheme item in esquema)
            {
                if (campos.SelectedItem.Tag.ToString() == item.Nombre)
                {
                    switch (item.Tipo)
                    {
                        case "string":
                            RadTextBox tb = new RadTextBox();
                            tableLayoutPanel.Controls.Add(tb, 2, indice);
                            tb.Dock = DockStyle.Fill;
                            tb.Text = valor;
                            break;
                        case "int":
                            RadSpinEditor spin = new RadSpinEditor();
                            tableLayoutPanel.Controls.Add(spin, 2, indice);
                            spin.Dock = DockStyle.Fill;
                            int valNumerico;
                            int.TryParse(valor, out valNumerico);
                            break;
                        case "date":
                            RadDateTimePicker dateTimePicker = new RadDateTimePicker();
                            tableLayoutPanel.Controls.Add(dateTimePicker, 2, indice);
                            dateTimePicker.Dock = DockStyle.Fill;
                            DateTime parsedDate;
                            DateTime.TryParse(valor, out parsedDate);
                            dateTimePicker.Value = parsedDate;
                            break;
                        default:
                            break;
                    }
                }
            }


            tableLayoutPanel.Controls.Add(campos, 0, indice);
            tableLayoutPanel.Controls.Add(operadores, 1, indice);
            campos.Dock = DockStyle.Fill;
            operadores.Dock = DockStyle.Fill;


            campos.SelectedIndexChanged += DropDownList_ValueChanged;

            //campos.SelectedIndex = indexCampo;
            int indexOperadores = operadores.FindString(operador);
            operadores.SelectedIndex = indexOperadores;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowCount++;
        }

        private void btnBorrar_Click(object sender,EventArgs e)
        {
            if (tableLayoutPanel.RowCount==1)
            {
                //for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
                //{
                //    Control ctrl = tableLayoutPanel.GetControlFromPosition(i, tableLayoutPanel.RowCount - 1);
                //    tableLayoutPanel.Controls.Remove(ctrl);
                //}
                //tableLayoutPanel.RowCount--;
            }
            else
            {
                if (tableLayoutPanel.RowCount>1)
                {
                    for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
                    {
                        Control ctrl = tableLayoutPanel.GetControlFromPosition(i, tableLayoutPanel.RowCount-1);
                        tableLayoutPanel.Controls.Remove(ctrl);
                    }
                    Control lbl = tableLayoutPanel.GetControlFromPosition(0, tableLayoutPanel.RowCount-2);
                    tableLayoutPanel.Controls.Remove(lbl);
                    tableLayoutPanel.RowCount-=2;
                }
            }
        }

        private void btnAnd_Click(object sender, EventArgs e)
        {

            RadLabel lbl = new RadLabel();
            lbl.Text = "AND";
            tableLayoutPanel.Controls.Add(lbl, 0, tableLayoutPanel.RowCount);


            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowCount++;
            Añadir();
        }

        private void btnOr_Click(object sender, EventArgs e)
        {
            RadLabel lbl = new RadLabel();
            lbl.Text = "OR";
            tableLayoutPanel.Controls.Add(lbl, 0, tableLayoutPanel.RowCount);


            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowCount++;
            Añadir();

        }
        private void cargarFiltros()
        {
            string operadorSQL=string.Empty;
            XDocument X = XDocument.Load(path);
                foreach (var item in X.Descendants("filtroRecepcionesVistaPedido"))
                {
                if (item.Descendants("filtro").Count()==0)
                {
                    Añadir();
                }
                else
                { 
                    foreach (var filtro in item.Descendants("filtro"))
                    {
                        string campo = filtro.Element("campo").Value;
                        string operador = filtro.Element("operadorComparacion").Value;
                        string valor = filtro.Element("valor").Value;
                        valor = valor.Replace("'", string.Empty);

                        if (filtro.HasAttributes)
                        {
                            operadorSQL = filtro.Attribute("operadorSQL").Value;
                            RumLabel lbl = new RumLabel();
                            lbl.Text = operadorSQL;
                            tableLayoutPanel.Controls.Add(lbl, 0, tableLayoutPanel.RowCount);
                            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                            tableLayoutPanel.RowCount++;
                            AñadirXML(campo, operador, valor);

                        }
                        else
                        {
                            Debug.WriteLine(campo + " " + operador + " " + valor);
                            AñadirXML(campo, operador, valor);
                        }
                    }
            }
        }

        }
        private void guardarFiltro()
        {
            XDocument doc = XDocument.Load(path);
            XElement xmlTree = doc.Element("filtroRecepcionesVistaPedido");
            string ultimoOperador = string.Empty;
            string valor=string.Empty;
            foreach (var item in doc.Descendants("filtroRecepcionesVistaPedido"))
            {
                item.Descendants().Remove();
            }
            foreach (var item in doc.Descendants("filtroRecepcionesVistaPedido"))
            {
                for (int i = 0; i <= tableLayoutPanel.RowCount; i++)
                {
                    if (tableLayoutPanel.GetControlFromPosition(0, i) is RadDropDownList)
                    {
                        RadDropDownList temp = (RadDropDownList)tableLayoutPanel.GetControlFromPosition(0, i);
                        RadDropDownList temp2 = (RadDropDownList)tableLayoutPanel.GetControlFromPosition(1, i);
                        switch (tableLayoutPanel.GetControlFromPosition(2, i).GetType().ToString())
                        {
                            case "Telerik.WinControls.UI.RadTextBox":
                                RadTextBox tb = (RadTextBox)tableLayoutPanel.GetControlFromPosition(2, i);
                                valor ="'"+ tb.Text+"'";
                                break;
                            case "Telerik.WinControls.UI.RadSpinEditor":
                                RadSpinEditor spin = (RadSpinEditor)tableLayoutPanel.GetControlFromPosition(2, i);
                                valor = spin.Value.ToString();
                                break;
                            case "Telerik.WinControls.UI.RadDateTimePicker":
                                RadDateTimePicker date = (RadDateTimePicker)tableLayoutPanel.GetControlFromPosition(2, i);
                                valor = date.Value.ToString();
                                break;

                        }
                        XElement filtro = new XElement("filtro",
                        new XElement("campo", temp.SelectedItem.Tag),
                        new XElement("operadorComparacion", temp2.Text),
                        new XElement("valor", valor));
                        if (ultimoOperador!=string.Empty)
                        {
                            filtro.Add(new XAttribute("operadorSQL", ultimoOperador));

                        }
                        item.Add(filtro);

                    }
                    else
                    {
                        if (tableLayoutPanel.GetControlFromPosition(0, i) is RadLabel)
                        {
                            RadLabel lb = (RadLabel)tableLayoutPanel.GetControlFromPosition(0, i);
                            ultimoOperador = lb.Text;
                        }
                    }

                }

            }
            doc.Save(path);

        }
        private RadDropDownList extraerEsquemaJson(string nombreJson)
        {

            cmb = new RadDropDownList();
            for (int i = 0; i < esquema.Count; i++)
            {
                RadListDataItem dataItem = new RadListDataItem();
                dataItem.Tag = esquema[i].Nombre;

                    dataItem.Text = esquema[i].Etiqueta;
                cmb.Items.Add(dataItem);

            }
            return cmb;
        }
        private void crearControl(string nombre,int indice)
        {
            foreach (TableScheme item in esquema)
            {
                if (nombre == item.Nombre)
                {
                    switch (item.Tipo)
                    {
                        case "string":
                            RadTextBox tb = new RadTextBox();
                            tableLayoutPanel.Controls.Remove(tableLayoutPanel.GetControlFromPosition(2, indice));
                            tableLayoutPanel.Controls.Add(tb, 2, indice);
                            tb.Dock = DockStyle.Fill;
                            break;
                        case "int":
                            RadSpinEditor spin = new RadSpinEditor();
                            tableLayoutPanel.Controls.Remove(tableLayoutPanel.GetControlFromPosition(2, indice));
                            tableLayoutPanel.Controls.Add(spin, 2, indice);
                            spin.Dock = DockStyle.Fill;
                            break;
                        case "date":
                            RadDateTimePicker dateTimePicker = new RadDateTimePicker();
                            tableLayoutPanel.Controls.Remove(tableLayoutPanel.GetControlFromPosition(2, indice));
                            tableLayoutPanel.Controls.Add(dateTimePicker, 2, indice);
                            dateTimePicker.Dock = DockStyle.Fill;
                            dateTimePicker.Value = DateTime.Now;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void DropDownList_ValueChanged(object o, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            RadDropDownList temp = (RadDropDownList)o;
            int indice = tableLayoutPanel.GetRow(temp);
            crearControl(temp.SelectedItem.Tag.ToString(),indice);

        }
    }
}

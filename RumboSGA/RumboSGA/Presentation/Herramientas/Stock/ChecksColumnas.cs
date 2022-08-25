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

namespace RumboSGA.Presentation.Herramientas.Stock
{
    public partial class ChecksColumnas : Telerik.WinControls.UI.RadForm
    {
        private List<RadCheckBox> checks = new List<RadCheckBox>();
       public List<string> columnasVisibles;
        public ChecksColumnas(List<RadCheckBox> checks, ref List<string> columnasVis)
        {
            InitializeComponent();
            this.checks = checks;
            columnasVisibles = columnasVis;
            crearChecks(checks);
            
            
        }
        private void crearChecks(List<RadCheckBox> checks)
        {
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();
            this.tableLayoutPanel1.RowCount = checks.Count;
            int contRow = 0;
            int contCol = 0;
            foreach (RadCheckBox item in checks)
            {
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                this.tableLayoutPanel1.Controls.Add(item,0,contRow);
                contRow++;
                var visible = columnasVisibles.Find(x => x == item.Text);
                if (visible!=null)
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
                item.CheckStateChanged += ItemCheckedChanged;

            }
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.AutoScroll=true;
            this.AutoScroll = true;
        }
        private void ItemCheckedChanged(object sender,EventArgs e)
        {
            RadCheckBox cb = (RadCheckBox)sender;
            var item = columnasVisibles.Find(x=>x==cb.Text);
            if (item!=null&&item.Length!=0)
            {
                //if (cb.Checked)
                //{
                //    columnasVisibles.Add(cb.Text);
                //}
            columnasVisibles.Remove(cb.Text);

            }
            else
            {
                columnasVisibles.Add(cb.Text);

            }
            //foreach (var itemi in columnasVisibles)
            //{
            //    Debug.WriteLine(itemi);
            //}
        
        }
    }
}

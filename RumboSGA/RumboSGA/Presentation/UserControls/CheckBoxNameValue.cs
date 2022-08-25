using RumboSGA.Properties;
using RumboSGAManager.Model;
using RumboSGAManager.Model.DataContext;
using RumboSGAManager.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls
{
    public partial class CheckBoxNameValue : NameValue
    {
        public RadCheckBox checkBoxLineaEditada;

        public CheckBoxNameValue(TableScheme tableScheme, string valor) : base(tableScheme, valor)
        {
            añadirCheckBox();
        }

        private void añadirCheckBox()
        {
            tableLayoutPanel1.ColumnCount = tableLayoutPanel1.ColumnCount + 1;
            ColumnStyle cs = new ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F);
            checkBoxLineaEditada = new RadCheckBox();
            tableLayoutPanel1.ColumnStyles.Insert(0, cs);
            tableLayoutPanel1.Controls.Add(checkBoxLineaEditada, 0, 0);
        }
    }
}
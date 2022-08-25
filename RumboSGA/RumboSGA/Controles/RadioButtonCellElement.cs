using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Drawing;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.Enumerations;

namespace RumboSGA.Controles
{
    public class RadioButtonCellElement : GridDataCellElement
    {
        private RadRadioButtonElement radioButtonElement1;

        public RadioButtonCellElement(GridViewColumn column, GridRowElement row)
            : base(column, row)
        {
        }

        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(GridDataCellElement);
            }
        }

        protected override void CreateChildElements()
        {
            base.CreateChildElements();

            radioButtonElement1 = new RadRadioButtonElement();
            radioButtonElement1.Margin = new Padding(0, 2, 0, 0);
            radioButtonElement1.MinSize = new Size(50, 20);
            radioButtonElement1.Text = "";
            radioButtonElement1.MouseDown += new MouseEventHandler(radioButtonElement1_MouseDown);

            this.Children.Add(radioButtonElement1);
        }

        protected override void DisposeManagedResources()
        {
            radioButtonElement1.MouseDown -= new MouseEventHandler(radioButtonElement1_MouseDown);

            base.DisposeManagedResources();
        }

        protected override void SetContentCore(object value)
        {
            if (this.Value != null && this.Value != DBNull.Value)
            {
                int intValue = Convert.ToInt32(value);

                if (intValue == 1)
                {
                    radioButtonElement1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
                }
                else
                {
                    radioButtonElement1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
                }
            }
        }

        protected override SizeF ArrangeOverride(SizeF finalSize)
        {
            if (this.Children.Count == 1)
            {
                this.Children[0].Arrange(new RectangleF(
                    0,
                    (finalSize.Height / 2) - (this.Children[0].DesiredSize.Height / 2),
                    this.Children[0].DesiredSize.Width,
                    this.Children[0].DesiredSize.Height));
            }

            return finalSize;
        }

        public override bool IsCompatible(GridViewColumn data, object context)
        {
            return data is RadioButtonColumn && context is GridDataRowElement;
        }

        private void radioButtonElement1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Value = 1;

            foreach (GridViewRowInfo row in this.ViewTemplate.Rows)
            {
                if (row != this.RowInfo)
                {
                    row.Cells[this.ColumnInfo.Name].Value = 0;
                }
            }
        }
    }
}

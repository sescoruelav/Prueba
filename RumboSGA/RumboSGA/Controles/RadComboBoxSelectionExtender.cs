using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RumboSGA.Controles
{
    [ToolboxItem(false)]
public class RadComboBoxSelectionExtender : System.ComponentModel.Component
{
    private RadAutoCompleteBoxElement autoCompleteBoxElement;
    public System.Collections.Generic.Dictionary<string, GridViewRowInfo> rows = new System.Collections.Generic.Dictionary<string, GridViewRowInfo>();
    public RadMultiColumnComboBox associatedRadMultiColumnComboBox;
    private Size originalSize = Size.Empty;
        public RadAutoCompleteBoxElement AutoCompleteBoxElement
    {
        get
        {
            return this.autoCompleteBoxElement;
        }
    }
    /// <summary>
    /// Gets the tokenized items.
    /// </summary>
    public RadTokenizedTextItemCollection Items
    {
        get { return this.autoCompleteBoxElement.Items; }
    }
    public RadMultiColumnComboBox AssociatedRadMultiColumnComboBox
    {
        get
        {
            return this.associatedRadMultiColumnComboBox;
        }
        set
        {
            this.SetAssociatedRadMultiColumnComboBox(value);
        }
    }
    private void SetAssociatedRadMultiColumnComboBox(RadMultiColumnComboBox radMultiColumnComboBox)
    {
        if (radMultiColumnComboBox == null && this.associatedRadMultiColumnComboBox != null)
        {
            this.SetAssociatedRadMultiColumnComboBoxCoreToNull();
        }
        this.SetAssociatedRadMultiColumnComboBoxCore(radMultiColumnComboBox);
    }
    private void SetAssociatedRadMultiColumnComboBoxCoreToNull()
    {
        this.associatedRadMultiColumnComboBox.HandleCreated -= associatedRadMultiColumnComboBox_HandleCreated;
        this.associatedRadMultiColumnComboBox.DataBindingComplete -= associatedRadMultiColumnComboBox_DataBindingComplete;
        this.associatedRadMultiColumnComboBox.EditorControl.ViewCellFormatting -= EditorControl_ViewCellFormatting;
        this.associatedRadMultiColumnComboBox.DropDownClosing -= radMultiColumnCombobox1_DropDownClosing;
        this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.TextBoxElement.Visibility = ElementVisibility.Visible;
        this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.Children[2].Children.Remove(autoCompleteBoxElement);
        this.autoCompleteBoxElement.MaxSize = new Size(0, 0);
        this.autoCompleteBoxElement.AutoCompleteDataSource = null;
        this.associatedRadMultiColumnComboBox.AutoSize = false;
        this.associatedRadMultiColumnComboBox.Size = this.originalSize;
        foreach (GridViewRowInfo item in this.associatedRadMultiColumnComboBox.EditorControl.Rows)
        {
            item.Tag = null;
        }
        this.autoCompleteBoxElement.Items.CollectionChanged -= Items_CollectionChanged;
        this.autoCompleteBoxElement.CreateTextBlock -= autoCompleteBoxElement_CreateTextBlock;
        this.autoCompleteBoxElement.TokenValidating -= autoCompleteBoxElement_TokenValidating;
        this.autoCompleteBoxElement.Text = this.associatedRadMultiColumnComboBox.Text;
        this.autoCompleteBoxElement.KeyDown -= autoCompleteBoxElement_KeyDown;
        this.associatedRadMultiColumnComboBox = null;
        this.rows.Clear();
    }
    private void SetAssociatedRadMultiColumnComboBoxCore(RadMultiColumnComboBox radMultiColumnComboBox)
    {
        if (radMultiColumnComboBox == null)
        {
            return;
        }
        this.originalSize = radMultiColumnComboBox.Size;
        this.associatedRadMultiColumnComboBox = radMultiColumnComboBox;
        this.associatedRadMultiColumnComboBox.AutoSize = true;
        this.associatedRadMultiColumnComboBox.ThemeNameChanged += associatedRadMultiColumnComboBox_ThemeNameChanged;
        this.associatedRadMultiColumnComboBox.HandleCreated += associatedRadMultiColumnComboBox_HandleCreated;
        this.associatedRadMultiColumnComboBox.DataBindingComplete += associatedRadMultiColumnComboBox_DataBindingComplete;
        this.associatedRadMultiColumnComboBox.DropDownStyle = RadDropDownStyle.DropDownList;
        this.associatedRadMultiColumnComboBox.EditorControl.ViewCellFormatting += EditorControl_ViewCellFormatting;
        this.associatedRadMultiColumnComboBox.DropDownClosing += radMultiColumnCombobox1_DropDownClosing;
        this.autoCompleteBoxElement = new RadAutoCompleteBoxElement();
        this.autoCompleteBoxElement.MinSize = new Size(150, 24);
        this.autoCompleteBoxElement.Multiline = true;
        this.autoCompleteBoxElement.DrawBorder = false;
        this.autoCompleteBoxElement.KeyDown += autoCompleteBoxElement_KeyDown;
        this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.ArrowButton.ZIndex = 1;
        this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.TextBoxElement.Visibility = ElementVisibility.Hidden;
        this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.Children[2].Children.Add(this.autoCompleteBoxElement);
        this.autoCompleteBoxElement.MaxSize = new Size(this.associatedRadMultiColumnComboBox.Size.Width - this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.ArrowButton.Size.Width, 0);
        this.autoCompleteBoxElement.AutoCompleteDataSource = new System.Collections.Generic.List<string>(GetAutoCompleteItems());
        this.autoCompleteBoxElement.Margin = new System.Windows.Forms.Padding(0);
        this.autoCompleteBoxElement.Items.CollectionChanged += Items_CollectionChanged;
        this.autoCompleteBoxElement.CreateTextBlock += autoCompleteBoxElement_CreateTextBlock;
        this.autoCompleteBoxElement.TokenValidating += autoCompleteBoxElement_TokenValidating;
    }
    void associatedRadMultiColumnComboBox_ThemeNameChanged(object source, ThemeNameChangedEventArgs args)
    {
        this.autoCompleteBoxElement.MaxSize = new Size(this.associatedRadMultiColumnComboBox.Size.Width - this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.ArrowButton.Size.Width, 0);
    }

    void autoCompleteBoxElement_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (e.KeyCode == System.Windows.Forms.Keys.F4)
        {
            associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.ShowPopup();
        }
        if (e.KeyCode == System.Windows.Forms.Keys.Escape)
        {
            associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.ClosePopup(RadPopupCloseReason.CloseCalled);
        }
    }
    void associatedRadMultiColumnComboBox_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
    {
        autoCompleteBoxElement.AutoCompleteDataSource = new System.Collections.Generic.List<string>(GetAutoCompleteItems());
    }
    void associatedRadMultiColumnComboBox_HandleCreated(object sender, EventArgs e)
    {
        autoCompleteBoxElement.AutoCompleteDataSource = new System.Collections.Generic.List<string>(GetAutoCompleteItems());
    }
    private System.Collections.Generic.IEnumerable<string> GetAutoCompleteItems()
    {
        foreach (GridViewRowInfo row in this.associatedRadMultiColumnComboBox.EditorControl.Rows)
        {
            string value = row.Cells[this.associatedRadMultiColumnComboBox.DisplayMember].Value.ToString().Trim();
            if (!rows.ContainsKey(value))
            {
                this.rows.Add(value, row);
            }
            yield return (value);
        }
    }
    void radMultiColumnCombobox1_DropDownClosing(object sender, RadPopupClosingEventArgs args)
    {
        args.Cancel = (args.CloseReason == RadPopupCloseReason.Mouse &&
                     this.associatedRadMultiColumnComboBox.EditorControl.ElementTree.GetElementAtPoint<RadCheckBoxElement>(this.associatedRadMultiColumnComboBox.EditorControl.PointToClient(System.Windows.Forms.Cursor.Position)) != null);
        this.SyncCollection();
    }
    void EditorControl_ViewCellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
    {
        if (e.ColumnIndex != -1 || e.CellElement.RowIndex == -1)
        {
            return;
        }
        if (e.CellElement.Children.Count == 1)
        {
            RadCheckBoxElement checkBoxElement = new RadCheckBoxElement();
            checkBoxElement.Padding = new System.Windows.Forms.Padding(0, 3, 1, 0);
            checkBoxElement.Alignment = ContentAlignment.MiddleCenter;
            checkBoxElement.NotifyParentOnMouseInput = false;
            e.CellElement.Children.Add(checkBoxElement);
        }
        RadCheckBoxElement checkBox = e.CellElement.FindDescendant<RadCheckBoxElement>();
        checkBox.CheckStateChanged -= checkBox_CheckStateChanged;
        checkBox.IsChecked = e.Row.Tag != null && e.Row.Tag.ToString() == Boolean.TrueString;
        checkBox.CheckStateChanged += checkBox_CheckStateChanged;
    }
    public void SyncCollection()
    {
        this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.TextBoxElement.SuspendPropertyNotifications();
        this.associatedRadMultiColumnComboBox.Text = string.Empty;
        foreach (GridViewRowInfo item in this.associatedRadMultiColumnComboBox.EditorControl.Rows)
        {
            if (item.Tag != null && item.Tag.ToString() == Boolean.TrueString)
            {
                this.associatedRadMultiColumnComboBox.Text += item.Cells[this.associatedRadMultiColumnComboBox.DisplayMember].Value + "; ";
            }
        }
        this.associatedRadMultiColumnComboBox.MultiColumnComboBoxElement.TextBoxElement.ResumePropertyNotifications();
        autoCompleteBoxElement.Items.CollectionChanged -= Items_CollectionChanged;
        autoCompleteBoxElement.CreateTextBlock -= autoCompleteBoxElement_CreateTextBlock;
        autoCompleteBoxElement.TokenValidating -= autoCompleteBoxElement_TokenValidating;
        autoCompleteBoxElement.Text = this.associatedRadMultiColumnComboBox.Text;
        autoCompleteBoxElement.TokenValidating += autoCompleteBoxElement_TokenValidating;
        autoCompleteBoxElement.CreateTextBlock += autoCompleteBoxElement_CreateTextBlock;
        autoCompleteBoxElement.Items.CollectionChanged += Items_CollectionChanged;
    }
    void autoCompleteBoxElement_TokenValidating(object sender, TokenValidatingEventArgs e)
    {
        if (this.rows.ContainsKey(e.Text))
        {
            if (this.rows[e.Text].Tag + "" == Boolean.TrueString)
            {
                e.IsValidToken = false;
            }
        }
        else
        {
            e.IsValidToken = false;
        }
    }
    void autoCompleteBoxElement_CreateTextBlock(object sender, CreateTextBlockEventArgs e)
    {
        if (this.rows.ContainsKey(e.Text) && e.TextBlock is TokenizedTextBlockElement)
        {
            this.rows[e.Text].Tag = Boolean.TrueString;
            this.rows[e.Text].InvalidateRow();
        }
    }
    void Items_CollectionChanged(object sender, Telerik.WinControls.Data.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == Telerik.WinControls.Data.NotifyCollectionChangedAction.Remove)
        {
            foreach (RadTokenizedTextItem item in e.NewItems)
            {
                if (this.rows.ContainsKey(item.Text.Trim()))
                {
                    string itemText = item.Text.Trim();
                    this.rows[itemText].Tag = Boolean.FalseString;
                    this.rows[itemText].InvalidateRow();
                }
            }
        }
    }
    void checkBox_CheckStateChanged(object sender, EventArgs e)
    {
        RadCheckBoxElement checkBox = sender as RadCheckBoxElement;
        GridRowElement row = checkBox.FindAncestor<GridRowElement>();
        row.RowInfo.Tag = checkBox.IsChecked.ToString();
    }
}

}

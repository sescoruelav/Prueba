using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation
{
    public partial class MyMessageBox : Telerik.WinControls.UI.RadForm
    {
        static MyMessageBox newMessageBox;
        static string Button_id;

        public static string ShowBox(string txtMessage)
        {
            newMessageBox = new MyMessageBox();
            newMessageBox.lblMessage.Text = txtMessage;
            newMessageBox.ShowDialog();
            return Button_id;
        }

        public static string ShowBox(string txtMessage, string txtTitle)
        {
            newMessageBox = new MyMessageBox();
            newMessageBox.lblMessage.Text = txtMessage;
            newMessageBox.Text = txtTitle;
            newMessageBox.ShowDialog();
            return Button_id;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Button_id = "1";
            newMessageBox.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Button_id = "2";
            Clipboard.SetText(lblMessage.Text);
            //newMessageBox.Dispose();
        }
    }
}

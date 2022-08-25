using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RumboSGA
{
    public partial class ReportViewerForm1 : Form
    {
        public ReportViewerForm1()
        {
            InitializeComponent();
        }

        private void ReportViewerForm1_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }
    }
}

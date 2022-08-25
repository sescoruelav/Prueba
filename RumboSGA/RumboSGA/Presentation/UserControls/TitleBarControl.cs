using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RumboSGA.Presentation.UserControls
{
    public partial class TitleBarControl : UserControl
    {
        //private bool drag = false;
        //private Point startPoint = new Point(0, 0);
        #region Constructor

        public TitleBarControl()
        {
            InitializeComponent();

            //Inicialización de Propiedades
            radPanel1.ElementTree.EnableApplicationThemeName = false;
            radTitleBar1.ElementTree.EnableApplicationThemeName = false;
            radTitleBar1.TitleBarElement.BorderPrimitive.Visibility = ElementVisibility.Collapsed;
            radTitleBar1.ThemeName = "VisualStudio2012Dark";
            radPanel1.BackgroundImage = Resources.LogoTransparente;
            radPanel1.BackgroundImageLayout = ImageLayout.None;
            radPanel1.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;
            radPanel1.Text = "";
            radPanel1.BackColor = Color.FromArgb(52, 53, 54);
            radTitleBar1.BackColor = Color.FromArgb(52, 53, 54);
            //this.BackColor = Color.FromArgb(52, 53, 54);

            //this.MouseDown += new MouseEventHandler(Title_MouseDown);
            //this.MouseUp += new MouseEventHandler(Title_MouseUp);
            //this.MouseMove += new MouseEventHandler(Form1_MouseDown);
            //tableLayoutPanel1.MouseDown += new MouseEventHandler(Form1_MouseDown);
            //radPanel1.MouseDown += radPanel1_MouseDown;
        }

        //void Title_MouseUp(object sender, MouseEventArgs e)
        //{
        //    this.drag = false;
        //}

        //void Title_MouseDown(object sender, MouseEventArgs e)
        //{
        //    this.startPoint = e.Location;
        //    this.drag = true;
        //}

        //void Title_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (this.drag)
        //    { // if we should be dragging it, we need to figure out some movement
        //        Point p1 = new Point(e.X, e.Y);
        //        Point p2 = this.PointToScreen(p1);
        //        Point p3 = new Point(p2.X - this.startPoint.X,
        //                             p2.Y - this.startPoint.Y);
        //        this.Location = p3;
        //    }
        //}



        //public const int WM_NCLBUTTONDOWN = 0xA1;
        //public const int HT_CAPTION = 0x2;

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();

        //private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        ReleaseCapture();
        //        SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        //    }
        //}

        #endregion



        //private void radTitleBar1_Click(object sender, EventArgs e)
        //{

        //}
        //public const int WM_WINDOWPOSCHANGED = 0x47;
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_WINDOWPOSCHANGED)
        //    {
        //        this.Text = Control.MousePosition.ToString();
        //    }
        //    base.WndProc(ref m);
        //}




    }
}


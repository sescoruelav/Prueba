using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Analytics;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace RumboSGA.Presentation.UserControls
{
    public partial class TopControl : UserControl
    {
        #region Variables y Propiedades

        Dictionary<string, bool> loadedThemes = new Dictionary<string, bool>();

        public RadLabel ViewLabel { get { return this.viewLabel; } }

        #endregion

        #region Constructor

        public TopControl()
        {
            InitializeComponent();

            new FluentTheme();            
            new TelerikMetroTheme();
            new VisualStudio2012DarkTheme();

            InitializeThemesDropDown();
        }

        #endregion

        #region Funciones Auxiliares

        private void InitializeThemesDropDown()
        {            
            AddThemeItemToThemesDropDownList("Fluent", Resources.fluent);
            AddThemeItemToThemesDropDownList("FluentDark", Resources.fluent_dark);
            AddThemeItemToThemesDropDownList("Material", Resources.material);
            AddThemeItemToThemesDropDownList("MaterialPink", Resources.material_pink);
            AddThemeItemToThemesDropDownList("MaterialTeal", Resources.material_teal);
            AddThemeItemToThemesDropDownList("MaterialBlueGrey", Resources.material_blue_grey);
            AddThemeItemToThemesDropDownList("ControlDefault", Resources.control_default);
            AddThemeItemToThemesDropDownList("TelerikMetro", Resources.telerik_metro);
            AddThemeItemToThemesDropDownList("Windows8", Resources.windows8);

            loadedThemes.Add("ControlDefault", true);            
            loadedThemes.Add("TelerikMetro", true);
                        
            chooseThemeDropDown.Text = "TelerikMetro";            
            //ThemeResolutionService.ApplicationThemeName = "TelerikMetro"; //Tema por Defecto de la Aplicación

            chooseThemeDropDown.SelectedIndexChanged += ChooseThemeDropDown_SelectedIndexChanged;
            chooseThemeDropDown.DropDownStyle = RadDropDownStyle.DropDownList;


        }

        private void ChooseThemeDropDown_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            var assemblyName = "Telerik.WinControls.Themes." + chooseThemeDropDown.SelectedItem.Text + ".dll";
            var themeName = chooseThemeDropDown.SelectedItem.Text;
            var strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), assemblyName);
            if (!System.IO.File.Exists(strTempAssmbPath)) // we are in the case of QSF as exe, so the Path is different
            {
                strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\Bin40");
                strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);

                if (!System.IO.File.Exists(strTempAssmbPath))
                {
                    strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\bin\\ReleaseTrial");
                    strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, assemblyName);
                }
            }

            if (!loadedThemes.ContainsKey(themeName))
            {

                Assembly themeAssembly = Assembly.LoadFrom(strTempAssmbPath);
                Activator.CreateInstance(themeAssembly.GetType("Telerik.WinControls.Themes." + themeName + "Theme"));

                loadedThemes.Add(themeName, true);
            }

            ThemeResolutionService.ApplicationThemeName = themeName;
            if (ControlTraceMonitor.AnalyticsMonitor != null)
            {
                ControlTraceMonitor.AnalyticsMonitor.TrackAtomicFeature("ThemeChanged." + themeName);
            }
        }

        private void AddThemeItemToThemesDropDownList(string themeName, Image image)
        {
            RadListDataItem themeItem = new RadListDataItem();
            themeItem.Text = themeName;
            themeItem.Image = image;
            this.chooseThemeDropDown.Items.Add(themeItem);
        }

        #endregion
    }
}

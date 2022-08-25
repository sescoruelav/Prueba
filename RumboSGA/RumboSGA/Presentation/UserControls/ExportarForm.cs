using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Telerik.Data;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;

namespace RumboSGA.Presentation.UserControls.Mantenimientos
{
    public partial class ExportarForm : Telerik.WinControls.UI.RadForm
    {
        #region Variables
        private RadGroupBox radGroupSettings;
        private RadRadioButton radRadioButton1, radRadioButton2;
        private TableLayoutPanel tableLayoutPanel1;
        private RadGroupBox rgBox;
        private RadLabel radLabel1;
        private RadButton exportButton;
        private RadProgressBar radProgressBar1;
        private RadTextBox radTextBoxSheet;
        private RadCheckBox radCheckBoxExportVisual;
        private RadLabel radLblProgress;
        private RadGridView gridView;
        private string confirmacion= "Datos del grid exportados correctamente.Deseas abrir el archivo?";
        #endregion
        #region Constructor
        public ExportarForm(RadGridView grid)
        {
            InitializeComponent();
            //INICIALIZAR
            this.gridView = grid;
            tableLayoutPanel1 = new TableLayoutPanel();
            rgBox = new RadGroupBox();
            this.radLabel1 = new RadLabel();
            this.radProgressBar1 = new RadProgressBar();
            this.radTextBoxSheet = new RadTextBox();
            this.radCheckBoxExportVisual = new RadCheckBox();
            this.radLblProgress = new RadLabel();
            this.radGroupSettings = new RadGroupBox();this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.Size = new Size(200,500);
            this.MaximumSize = new System.Drawing.Size(200, 400);
            //  
            // entradaNormal 
            //  
            this.radRadioButton1 = new RadRadioButton();
            this.radRadioButton1.Enabled = false;
            this.radRadioButton1.Name = "entradaNormal";
            this.radRadioButton1.Size = new System.Drawing.Size(128, 33);
            this.radRadioButton1.TabIndex = 3;
            this.radRadioButton1.TabStop = true;
            this.radRadioButton1.Text = "Maximas filas soportadas\r\nen Excel 2007";
            this.radRadioButton1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            //  
            // entradaReserva 
            // 
            this.radRadioButton2 = new RadRadioButton();
            this.radRadioButton2.Enabled = false;
            this.radRadioButton2.Name = "entradaReserva";
            this.radRadioButton2.Size = new System.Drawing.Size(128, 33);
            this.radRadioButton2.TabIndex = 4;
            this.radRadioButton2.Text = "Maximas filas soportadas\r\npor versiones previas";
            // 
            // radCheckBoxExportVisual 
            //  
            this.radCheckBoxExportVisual.Enabled = false;
            this.radCheckBoxExportVisual.Name = "radCheckBoxExportVisual";
            this.radCheckBoxExportVisual.Size = new System.Drawing.Size(125, 18);
            this.radCheckBoxExportVisual.TabIndex = 2;
            this.radCheckBoxExportVisual.Text = "Exportar configuracion visual";
            this.radCheckBoxExportVisual.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            //
            //exportButton
            //
            exportButton = new RadButton();
            exportButton.Text = "Exportar";
            exportButton.TextAlignment = ContentAlignment.MiddleCenter;
            exportButton.Anchor = (AnchorStyles.Left|AnchorStyles.Right);
            //
            //opcionesExportarPanel
            //
            this.radGroupSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupSettings.Controls.Add(this.radTextBoxSheet);
            this.radGroupSettings.Controls.Add(this.radLabel1);
            this.radGroupSettings.Controls.Add(this.exportButton);
            this.radGroupSettings.Controls.Add(this.radRadioButton1);
            this.radGroupSettings.Controls.Add(this.radRadioButton2);
            this.radGroupSettings.Controls.Add(this.radProgressBar1);
            this.radGroupSettings.Controls.Add(this.radLblProgress);
            this.radGroupSettings.Controls.Add(this.radCheckBoxExportVisual);
            this.radGroupSettings.FooterText = "";
            this.radGroupSettings.HeaderText = "Export Settings";
            this.radGroupSettings.Name = "radGroupSettings";
            this.radGroupSettings.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            //
            this.radGroupSettings.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupSettings.Size = new System.Drawing.Size(185, 350);
            this.radGroupSettings.TabIndex = 8;
            this.radGroupSettings.Text = "Opciones Exportado";
            //
            //RadLabel
            //
 //           this.labelEntrada.Location = new System.Drawing.Point(9, 254);
            this.radLabel1.Name = "labelEntrada";
            this.radLabel1.Size = new System.Drawing.Size(68, 18);
            this.radLabel1.TabIndex = 7;
            this.radLabel1.Text = "Sheet name:";
            //
            //
            //RadProgressBar
 //           this.radProgressBar1.Location = new System.Drawing.Point(10, 323);
            this.radProgressBar1.Name = "radProgressBar1";
            this.radProgressBar1.SeparatorColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.radProgressBar1.SeparatorWidth = 4;
            this.radProgressBar1.Size = new System.Drawing.Size(190, 17);
            this.radProgressBar1.StepWidth = 13;
            this.radProgressBar1.TabIndex = 5;
            this.radProgressBar1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radProgressBar1.Visible = false;
            //
            //RadTextBoxSheet
            //
            //  
            // radTextBoxSheet 
            //  
           // this.radTextBoxSheet.Location = new System.Drawing.Point(10, 275);
            this.radTextBoxSheet.Name = "radTextBoxSheet";
            this.radTextBoxSheet.Size = new System.Drawing.Size(170, 20);
            this.radTextBoxSheet.TabIndex = 6;
            this.radTextBoxSheet.TabStop = false;
            //
            //Locations
            //
            radCheckBoxExportVisual.Location = new Point(10, 130);
            radRadioButton1.Location = new Point(10,150);
            radRadioButton2.Location = new Point(10, 190);
            //radComboBoxSummaries.Location = new Point(10, 227);
            radLabel1.Location = new Point(10, 250);
            radTextBoxSheet.Location = new Point(10,270);
            
            exportButton.Location = new Point(50,300);
            this.Controls.Add(radGroupSettings);
            //Handlers
            this.exportButton.Click += new System.EventHandler(this.buttonExport_Click);
        }

        #endregion 
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        private void exportProgress(object sender, ProgressEventArgs e)
        {
            // Call InvokeRequired to check if thread needs marshalling, to access properly the UI thread. 
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(
                    delegate {
                        if (e.ProgressValue<=100) 
                        {
                            radProgressBar1.Value1 = e.ProgressValue;
                        }
                        else
                        {
                            radProgressBar1.Value1 = 100;
                        }
                    }));
            }
            else
            {
                if (e.ProgressValue <=100)
                {
                    radProgressBar1.Value1 = e.ProgressValue;
                }
                else
                {
                    radProgressBar1.Value1 = 100;
                }

            }
        }
        private bool exportVisualSettings;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (saveFileDialog.FileName.Equals(String.Empty))
            {
                RadMessageBox.SetThemeName(this.gridView.ThemeName);
                RadMessageBox.Show("Introduce nombre de fichero");
                return;
            }

            string fileName = this.saveFileDialog.FileName;
            bool openExportFile = false;

            if (this.radCheckBoxExportVisual.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                this.exportVisualSettings = true;
            }
            else
            {
                this.exportVisualSettings = false;
            }
            RunExportToExcelML(fileName, ref openExportFile);


            if (openExportFile)
            {
                try
                {
                    System.Diagnostics.Process.Start(fileName);
                }
                catch (Exception ex)
                {
                    string message = String.Format("No se puede abrir el archivo en tu sistema\nError message: {0}", ex.Message);
                    RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
        private void RunExportToExcelML(string fileName, ref bool openExportFile)
        {
            ExportToExcelML excelExporter = new ExportToExcelML(this.gridView);

            if (this.radTextBoxSheet.Text != String.Empty)
            {
                excelExporter.SheetName = this.radTextBoxSheet.Text;

            }
            excelExporter.SummariesExportOption = SummariesOption.ExportAll;

            //set max sheet rows 
            if (this.radRadioButton1.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                excelExporter.SheetMaxRows = ExcelMaxRows._1048576;
            }
            else if (this.radRadioButton2.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                excelExporter.SheetMaxRows = ExcelMaxRows._65536;
            }

            //set exporting visual settings 
            excelExporter.ExportVisualSettings = this.exportVisualSettings;

            try
            {
                excelExporter.RunExport(fileName);

                RadMessageBox.SetThemeName(this.gridView.ThemeName);
                DialogResult dr = RadMessageBox.Show(confirmacion,
                    "Export to Excel", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    openExportFile = true;
                }
            }
            catch (IOException ex)
            {
                RadMessageBox.SetThemeName(this.gridView.ThemeName);
                RadMessageBox.Show(this, ex.Message, "I/O Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }
    }
}

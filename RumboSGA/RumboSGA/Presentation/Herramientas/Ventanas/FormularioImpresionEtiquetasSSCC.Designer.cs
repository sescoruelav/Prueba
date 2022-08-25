namespace RumboSGA.Presentation.Herramientas.Ventanas
{
    partial class FormularioImpresionEtiquetasSSCC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radTextBoxControlNumEtiquetas = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radTextBoxControlLote = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radPanelPrincipal = new Telerik.WinControls.UI.RadPanel();
            this.rumLabelExisteLote = new RumboSGA.RumLabel();
            this.radDateTimePickerFechaCaducidad = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radMCCMBPrefijo = new System.Windows.Forms.ComboBox();
            this.radTextBoxControlSSCCManual = new Telerik.WinControls.UI.RadTextBoxControl();
            this.rumrBtnSSCCGenerico = new RumboSGA.RumRadioButton();
            this.rumrBtnEntradaManual = new RumboSGA.RumRadioButton();
            this.rumrBtnAutogenerado = new RumboSGA.RumRadioButton();
            this.rumButtonCancelar = new RumboSGA.RumButton();
            this.rumButtonAlta = new RumboSGA.RumButton();
            this.rumLabelCantidadTotal = new RumboSGA.RumLabel();
            this.rumLabelFechaCaducidad = new RumboSGA.RumLabel();
            this.rumLabelTipoArticulo = new RumboSGA.RumLabel();
            this.rumLabelLote = new RumboSGA.RumLabel();
            this.rumLabelCopias = new RumboSGA.RumLabel();
            this.rumLabelNumEtiquetas = new RumboSGA.RumLabel();
            this.radComboBoxArticulo = new Telerik.WinControls.UI.RadMultiColumnComboBox();
            this.radTextBoxCopias = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radTextBoxControlTotalUdsPicos = new Telerik.WinControls.UI.RadTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlNumEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlLote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).BeginInit();
            this.radPanelPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelExisteLote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerFechaCaducidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlSSCCManual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumrBtnSSCCGenerico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumrBtnEntradaManual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumrBtnAutogenerado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelCantidadTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaCaducidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTipoArticulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelLote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelCopias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelNumEtiquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxArticulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxArticulo.EditorControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxArticulo.EditorControl.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxCopias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlTotalUdsPicos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxControlNumEtiquetas
            // 
            this.radTextBoxControlNumEtiquetas.Location = new System.Drawing.Point(178, 32);
            this.radTextBoxControlNumEtiquetas.Name = "radTextBoxControlNumEtiquetas";
            this.radTextBoxControlNumEtiquetas.Size = new System.Drawing.Size(119, 20);
            this.radTextBoxControlNumEtiquetas.TabIndex = 1;
            // 
            // radTextBoxControlLote
            // 
            this.radTextBoxControlLote.Location = new System.Drawing.Point(178, 111);
            this.radTextBoxControlLote.Name = "radTextBoxControlLote";
            this.radTextBoxControlLote.Size = new System.Drawing.Size(150, 20);
            this.radTextBoxControlLote.TabIndex = 5;
            this.radTextBoxControlLote.Leave += new System.EventHandler(this.RadTextBoxControlLote_Leave);
            // 
            // radPanelPrincipal
            // 
            this.radPanelPrincipal.Controls.Add(this.rumLabelExisteLote);
            this.radPanelPrincipal.Controls.Add(this.radDateTimePickerFechaCaducidad);
            this.radPanelPrincipal.Controls.Add(this.radGroupBox1);
            this.radPanelPrincipal.Controls.Add(this.rumButtonCancelar);
            this.radPanelPrincipal.Controls.Add(this.rumButtonAlta);
            this.radPanelPrincipal.Controls.Add(this.rumLabelCantidadTotal);
            this.radPanelPrincipal.Controls.Add(this.rumLabelFechaCaducidad);
            this.radPanelPrincipal.Controls.Add(this.rumLabelTipoArticulo);
            this.radPanelPrincipal.Controls.Add(this.rumLabelLote);
            this.radPanelPrincipal.Controls.Add(this.rumLabelCopias);
            this.radPanelPrincipal.Controls.Add(this.rumLabelNumEtiquetas);
            this.radPanelPrincipal.Controls.Add(this.radComboBoxArticulo);
            this.radPanelPrincipal.Controls.Add(this.radTextBoxCopias);
            this.radPanelPrincipal.Controls.Add(this.radTextBoxControlTotalUdsPicos);
            this.radPanelPrincipal.Controls.Add(this.radTextBoxControlNumEtiquetas);
            this.radPanelPrincipal.Controls.Add(this.radTextBoxControlLote);
            this.radPanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.radPanelPrincipal.Name = "radPanelPrincipal";
            this.radPanelPrincipal.Size = new System.Drawing.Size(546, 513);
            this.radPanelPrincipal.TabIndex = 12;
            this.radPanelPrincipal.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelPrincipal.GetChildAt(0))).BorderHighlightThickness = 1;
            // 
            // rumLabelExisteLote
            // 
            this.rumLabelExisteLote.AutoSize = false;
            this.rumLabelExisteLote.Enabled = false;
            this.rumLabelExisteLote.Location = new System.Drawing.Point(334, 114);
            this.rumLabelExisteLote.Name = "rumLabelExisteLote";
            this.rumLabelExisteLote.Size = new System.Drawing.Size(139, 18);
            this.rumLabelExisteLote.TabIndex = 52;
            this.rumLabelExisteLote.Text = "Existe o no existe el lote";
            // 
            // radDateTimePickerFechaCaducidad
            // 
            this.radDateTimePickerFechaCaducidad.Location = new System.Drawing.Point(178, 138);
            this.radDateTimePickerFechaCaducidad.Name = "radDateTimePickerFechaCaducidad";
            this.radDateTimePickerFechaCaducidad.Size = new System.Drawing.Size(193, 20);
            this.radDateTimePickerFechaCaducidad.TabIndex = 51;
            this.radDateTimePickerFechaCaducidad.TabStop = false;
            this.radDateTimePickerFechaCaducidad.Text = "lunes, 14 de septiembre de 2020";
            this.radDateTimePickerFechaCaducidad.Value = new System.DateTime(2020, 9, 14, 17, 6, 19, 995);
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.radMCCMBPrefijo);
            this.radGroupBox1.Controls.Add(this.radTextBoxControlSSCCManual);
            this.radGroupBox1.Controls.Add(this.rumrBtnSSCCGenerico);
            this.radGroupBox1.Controls.Add(this.rumrBtnEntradaManual);
            this.radGroupBox1.Controls.Add(this.rumrBtnAutogenerado);
            this.radGroupBox1.HeaderText = "";
            this.radGroupBox1.Location = new System.Drawing.Point(24, 211);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(492, 208);
            this.radGroupBox1.TabIndex = 50;
            // 
            // radMCCMBPrefijo
            // 
            this.radMCCMBPrefijo.FormattingEnabled = true;
            this.radMCCMBPrefijo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.radMCCMBPrefijo.Location = new System.Drawing.Point(200, 159);
            this.radMCCMBPrefijo.Name = "radMCCMBPrefijo";
            this.radMCCMBPrefijo.Size = new System.Drawing.Size(210, 21);
            this.radMCCMBPrefijo.TabIndex = 4;
            this.radMCCMBPrefijo.Text = "PrefijoSSCC";
            // 
            // radTextBoxControlSSCCManual
            // 
            this.radTextBoxControlSSCCManual.Location = new System.Drawing.Point(200, 90);
            this.radTextBoxControlSSCCManual.Name = "radTextBoxControlSSCCManual";
            this.radTextBoxControlSSCCManual.Size = new System.Drawing.Size(210, 20);
            this.radTextBoxControlSSCCManual.TabIndex = 3;
            // 
            // rumrBtnSSCCGenerico
            // 
            this.rumrBtnSSCCGenerico.Location = new System.Drawing.Point(54, 159);
            this.rumrBtnSSCCGenerico.Name = "rumrBtnSSCCGenerico";
            this.rumrBtnSSCCGenerico.Size = new System.Drawing.Size(94, 18);
            this.rumrBtnSSCCGenerico.TabIndex = 2;
            this.rumrBtnSSCCGenerico.Text = "SSCC Genérico";
            this.rumrBtnSSCCGenerico.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.rumrBtnSSCCGenerico_ToggleStateChanged);
            // 
            // rumrBtnEntradaManual
            // 
            this.rumrBtnEntradaManual.Location = new System.Drawing.Point(55, 90);
            this.rumrBtnEntradaManual.Name = "rumrBtnEntradaManual";
            this.rumrBtnEntradaManual.Size = new System.Drawing.Size(128, 18);
            this.rumrBtnEntradaManual.TabIndex = 1;
            this.rumrBtnEntradaManual.Text = "SSCC Entrada Manual";
            this.rumrBtnEntradaManual.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.rumrBtnEntradaManual_ToggleStateChanged);
            // 
            // rumrBtnAutogenerado
            // 
            this.rumrBtnAutogenerado.Location = new System.Drawing.Point(55, 21);
            this.rumrBtnAutogenerado.Name = "rumrBtnAutogenerado";
            this.rumrBtnAutogenerado.Size = new System.Drawing.Size(121, 18);
            this.rumrBtnAutogenerado.TabIndex = 0;
            this.rumrBtnAutogenerado.Text = "SSCC Autogenerado";
            this.rumrBtnAutogenerado.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.rumrBtnAutogenerado_ToggleStateChanged);
            // 
            // rumButtonCancelar
            // 
            this.rumButtonCancelar.Location = new System.Drawing.Point(406, 461);
            this.rumButtonCancelar.Name = "rumButtonCancelar";
            this.rumButtonCancelar.Size = new System.Drawing.Size(110, 24);
            this.rumButtonCancelar.TabIndex = 47;
            this.rumButtonCancelar.Text = "Cancelar";
            this.rumButtonCancelar.Click += new System.EventHandler(this.RumButtonCancelar_Click);
            // 
            // rumButtonAlta
            // 
            this.rumButtonAlta.Location = new System.Drawing.Point(290, 461);
            this.rumButtonAlta.Name = "rumButtonAlta";
            this.rumButtonAlta.Size = new System.Drawing.Size(110, 24);
            this.rumButtonAlta.TabIndex = 46;
            this.rumButtonAlta.Text = "Imprimir";
            this.rumButtonAlta.Click += new System.EventHandler(this.RumButtonImprimir_Click);
            // 
            // rumLabelCantidadTotal
            // 
            this.rumLabelCantidadTotal.AutoSize = false;
            this.rumLabelCantidadTotal.Location = new System.Drawing.Point(56, 165);
            this.rumLabelCantidadTotal.Name = "rumLabelCantidadTotal";
            this.rumLabelCantidadTotal.Size = new System.Drawing.Size(116, 18);
            this.rumLabelCantidadTotal.TabIndex = 32;
            this.rumLabelCantidadTotal.Text = "Cantidad Total";
            // 
            // rumLabelFechaCaducidad
            // 
            this.rumLabelFechaCaducidad.AutoSize = false;
            this.rumLabelFechaCaducidad.Location = new System.Drawing.Point(56, 138);
            this.rumLabelFechaCaducidad.Name = "rumLabelFechaCaducidad";
            this.rumLabelFechaCaducidad.Size = new System.Drawing.Size(116, 18);
            this.rumLabelFechaCaducidad.TabIndex = 31;
            this.rumLabelFechaCaducidad.Text = "Fecha Caducidad";
            // 
            // rumLabelTipoArticulo
            // 
            this.rumLabelTipoArticulo.AutoSize = false;
            this.rumLabelTipoArticulo.Location = new System.Drawing.Point(56, 85);
            this.rumLabelTipoArticulo.Name = "rumLabelTipoArticulo";
            this.rumLabelTipoArticulo.Size = new System.Drawing.Size(116, 18);
            this.rumLabelTipoArticulo.TabIndex = 30;
            this.rumLabelTipoArticulo.Text = "Articulo";
            // 
            // rumLabelLote
            // 
            this.rumLabelLote.AutoSize = false;
            this.rumLabelLote.Location = new System.Drawing.Point(56, 112);
            this.rumLabelLote.Name = "rumLabelLote";
            this.rumLabelLote.Size = new System.Drawing.Size(116, 18);
            this.rumLabelLote.TabIndex = 29;
            this.rumLabelLote.Text = "Lote";
            // 
            // rumLabelCopias
            // 
            this.rumLabelCopias.AutoSize = false;
            this.rumLabelCopias.Location = new System.Drawing.Point(56, 59);
            this.rumLabelCopias.Name = "rumLabelCopias";
            this.rumLabelCopias.Size = new System.Drawing.Size(116, 18);
            this.rumLabelCopias.TabIndex = 27;
            this.rumLabelCopias.Text = "Copias";
            // 
            // rumLabelNumEtiquetas
            // 
            this.rumLabelNumEtiquetas.AutoSize = false;
            this.rumLabelNumEtiquetas.Location = new System.Drawing.Point(56, 33);
            this.rumLabelNumEtiquetas.Name = "rumLabelNumEtiquetas";
            this.rumLabelNumEtiquetas.Size = new System.Drawing.Size(116, 18);
            this.rumLabelNumEtiquetas.TabIndex = 26;
            this.rumLabelNumEtiquetas.Text = "Etiquetas";
            // 
            // radComboBoxArticulo
            // 
            this.radComboBoxArticulo.AutoSizeDropDownHeight = true;
            this.radComboBoxArticulo.AutoSizeDropDownToBestFit = true;
            // 
            // radComboBoxArticulo.NestedRadGridView
            // 
            this.radComboBoxArticulo.EditorControl.BackColor = System.Drawing.SystemColors.Window;
            this.radComboBoxArticulo.EditorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radComboBoxArticulo.EditorControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radComboBoxArticulo.EditorControl.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radComboBoxArticulo.EditorControl.MasterTemplate.AllowAddNewRow = false;
            this.radComboBoxArticulo.EditorControl.MasterTemplate.AllowCellContextMenu = false;
            this.radComboBoxArticulo.EditorControl.MasterTemplate.AllowColumnChooser = false;
            this.radComboBoxArticulo.EditorControl.MasterTemplate.EnableGrouping = false;
            this.radComboBoxArticulo.EditorControl.MasterTemplate.ShowFilteringRow = false;
            this.radComboBoxArticulo.EditorControl.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radComboBoxArticulo.EditorControl.Name = "NestedRadGridView";
            this.radComboBoxArticulo.EditorControl.ReadOnly = true;
            this.radComboBoxArticulo.EditorControl.ShowGroupPanel = false;
            this.radComboBoxArticulo.EditorControl.Size = new System.Drawing.Size(240, 150);
            this.radComboBoxArticulo.EditorControl.TabIndex = 0;
            this.radComboBoxArticulo.Location = new System.Drawing.Point(178, 84);
            this.radComboBoxArticulo.Name = "radComboBoxArticulo";
            this.radComboBoxArticulo.Size = new System.Drawing.Size(338, 20);
            this.radComboBoxArticulo.TabIndex = 6;
            this.radComboBoxArticulo.TabStop = false;
            this.radComboBoxArticulo.SelectedValueChanged += new System.EventHandler(this.radComboBoxArticulo_SelectedValueChanged);
            // 
            // radTextBoxCopias
            // 
            this.radTextBoxCopias.Location = new System.Drawing.Point(178, 58);
            this.radTextBoxCopias.Name = "radTextBoxCopias";
            this.radTextBoxCopias.Size = new System.Drawing.Size(119, 20);
            this.radTextBoxCopias.TabIndex = 2;
            this.radTextBoxCopias.Text = "1";
            // 
            // radTextBoxControlTotalUdsPicos
            // 
            this.radTextBoxControlTotalUdsPicos.Location = new System.Drawing.Point(178, 164);
            this.radTextBoxControlTotalUdsPicos.Name = "radTextBoxControlTotalUdsPicos";
            this.radTextBoxControlTotalUdsPicos.Size = new System.Drawing.Size(119, 20);
            this.radTextBoxControlTotalUdsPicos.TabIndex = 8;
            // 
            // FormularioImpresionEtiquetasSSCC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(546, 513);
            this.Controls.Add(this.radPanelPrincipal);
            this.MaximumSize = new System.Drawing.Size(650, 543);
            this.MinimumSize = new System.Drawing.Size(500, 543);
            this.Name = "FormularioImpresionEtiquetasSSCC";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(650, 543);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impresión Etiquetas SSCC";
            this.Load += new System.EventHandler(this.FormularioImpresionEtiquetasSSCC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlNumEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlLote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelPrincipal)).EndInit();
            this.radPanelPrincipal.ResumeLayout(false);
            this.radPanelPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelExisteLote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerFechaCaducidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlSSCCManual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumrBtnSSCCGenerico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumrBtnEntradaManual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumrBtnAutogenerado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonCancelar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumButtonAlta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelCantidadTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelFechaCaducidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelTipoArticulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelLote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelCopias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumLabelNumEtiquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxArticulo.EditorControl.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxArticulo.EditorControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radComboBoxArticulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxCopias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlTotalUdsPicos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlNumEtiquetas;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlLote;
        private Telerik.WinControls.UI.RadPanel radPanelPrincipal;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlTotalUdsPicos;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxCopias;
        private Telerik.WinControls.UI.RadMultiColumnComboBox radComboBoxArticulo;
        private RumLabel rumLabelCantidadTotal;
        private RumLabel rumLabelFechaCaducidad;
        private RumLabel rumLabelTipoArticulo;
        private RumLabel rumLabelLote;
        private RumLabel rumLabelCopias;
        private RumLabel rumLabelNumEtiquetas;
        private RumButton rumButtonCancelar;
        private RumButton rumButtonAlta;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlSSCCManual;
        private RumRadioButton rumrBtnSSCCGenerico;
        private RumRadioButton rumrBtnEntradaManual;
        private RumRadioButton rumrBtnAutogenerado;
        private Telerik.WinControls.UI.RadDateTimePicker radDateTimePickerFechaCaducidad;
        private System.Windows.Forms.ComboBox radMCCMBPrefijo;
        private RumLabel rumLabelExisteLote;
    }
}

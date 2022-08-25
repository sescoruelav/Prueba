using RumboSGAManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RumboSGA.Presentation.FormulariosComunes
{
    public partial class FrmPopUpHelper : Telerik.WinControls.UI.ShapedForm
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Defino posiciones fijas
        //De momento para hacer pruebas rapidas y ver si funciona
        //TODO:Buscar algun tipo de container tipo wpf stackpanel para que apile controles automaticamente
        private Size _sizeLabels = new System.Drawing.Size(64, 19);
        private Size _sizeItems = new System.Drawing.Size(288, 24);
        private static int _verticalMargin = 25;

        private int _horizontalAlignmentLocation = 25;
        private int _verticalCurrentElement = 0; //Este parametro mantiene la posicion vertical del ultimo elemento añadido al contro

        //Contorles del formulario
        Controles.RumDateTimePicker rumDateTimePicker1;
        Telerik.WinControls.UI.RadDropDownList radDropDownList; //TODO: Para hacerlo multicolumna: Telerik.WinControls.UI.RadMultiColumnComboBox ;


        private TipoPopUp _tipoPopUp;
        enum TipoPopUp
        {
            ComboBox,
            DateTimePicker,
            ComboBoxDatePicker
        }


        //TODO: Cambiar estos objetos para que devuleva un diccionario con clae valor, que tengas todas las claves que se le pasen y devuelva valores seleccionados por el usuario
        //Objeto para enviar fecha seleccionada fuera del formulario
        public object PickedValue { get; set; }

        //Objeto para indice del combo seleccionado
        public object SelectedIndex { get; set; }



        public FrmPopUpHelper(string tittle, string labelText, DataTable dt, string valueMember, string displayMember)
        {
            log.Info(String.Format("Inicializo contructor FrmPopUpHelper con tipo comboBox:{0},{1},{2},{3}", tittle, labelText,valueMember, displayMember));

            InitializeComponent();

            this.Text = tittle;

            _tipoPopUp = TipoPopUp.ComboBox;

            //Creo el label
            CreateLabel(labelText);
            //Creo el combo
            CreateCombo(dt, valueMember, displayMember);
        }

        public FrmPopUpHelper(string tittle, string labelText)
        {
            log.Info(String.Format("Inicializo contructor FrmPopUpHelper con tipo datetimepicker:{0},{1}", tittle, labelText));

            InitializeComponent();
            _tipoPopUp = TipoPopUp.DateTimePicker;

            this.Text = tittle;

            //Creo el label
            CreateLabel(labelText);
            //Creo el datetimepicker
            CreateDatePicker();
        }

        public FrmPopUpHelper(string tittle, string labelCombo, DataTable dt, string valueMember, string displayMember, string labelPicker)
        {
            log.Info(String.Format("Inicializo contructor FrmPopUpHelper con tipo ComboBoxDatePicker:{0},{1},{2},{3},{4}", tittle, labelCombo, valueMember, displayMember, labelPicker));

            InitializeComponent();

            _tipoPopUp = TipoPopUp.ComboBoxDatePicker;

            this.Text = tittle;

            //Creo el label
            CreateLabel(labelCombo);
            //Creo el combo
            CreateCombo(dt, valueMember, displayMember);
            //Creo el label
            CreateLabel(labelPicker);
            //Creo el datetimepicker
            CreateDatePicker();
        }



        #region Botones de accion


        const string SELEECIONA_VALOR_VALIDO_CMB = "Debes seleccionar un valor de la lista en el desplegable";
        const string SELEECIONA_VALOR_VALIDO_FECHA = "Debes seleccionar un valor valido para la fecha";
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            log.Info("Se ha pulsado el boton aceptar");
            try
            {
                //Reviso que los campos seleccionados son correctos
                if (_tipoPopUp.Equals(TipoPopUp.ComboBox) || _tipoPopUp.Equals(TipoPopUp.ComboBoxDatePicker))
                {                    
                    if (radDropDownList.SelectedIndex == -1 )
                    {
                        log.Info("Valor seleccionado no valido para el combo");
                        MessageBox.Show(SELEECIONA_VALOR_VALIDO_CMB);
                        return;
                    }
                    
                }
                if (_tipoPopUp.Equals(TipoPopUp.DateTimePicker) || _tipoPopUp.Equals(TipoPopUp.ComboBoxDatePicker))
                {
                    if (rumDateTimePicker1.Value.Equals(DateTime.MinValue))
                    {
                        log.Info("Valor seleccionado no valido para la fecha");
                        MessageBox.Show(SELEECIONA_VALOR_VALIDO_FECHA);
                        return;
                    }

                }



                //Relleno lo parametros de salida de la funcion
                switch (_tipoPopUp)
                {
                    case TipoPopUp.ComboBox:
                        //Devuelvo el indice seleccionado. Despues con esto puedo saber la fila de la tabla
                        SelectedIndex = radDropDownList.SelectedIndex;
                        log.Info("SelectedIndex:" + SelectedIndex.ToString());
                        break;
                    case TipoPopUp.DateTimePicker:
                        PickedValue = rumDateTimePicker1.Value;
                        log.Info("SelectedPickedValue:" + PickedValue.ToString());
                        break;
                    case TipoPopUp.ComboBoxDatePicker:
                        SelectedIndex = radDropDownList.SelectedIndex;
                        PickedValue = rumDateTimePicker1.Value;
                        log.Info("SelectedIndex:" + SelectedIndex.ToString());
                        log.Info("SelectedPickedValue:" + PickedValue.ToString());
                        break;
                    default:
                        break;
                }

                this.DialogResult = DialogResult.OK;
            }

            catch (Exception ex)
            {
                ExceptionManager.GestionarError(ex, ex.Message);
            }
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            log.Info("Cancela");
            this.Close();
        }


        #endregion


        #region FUNCIONES AUXILIARES

        private void CreateLabel(string labelText)
        {
            log.Info("Creo Label: " + labelText);
            RumLabel rumLabelCombo = new RumLabel();
            _verticalCurrentElement += _verticalMargin;
            rumLabelCombo.Location = new Point(_horizontalAlignmentLocation, _verticalCurrentElement);
            rumLabelCombo.Size = _sizeItems;
            rumLabelCombo.Text = labelText;
            this.Controls.Add(rumLabelCombo);
        }

        private void CreateCombo(DataTable dt, string valueMember, string displayMember)
        {
            log.Info("Creo combo: " + valueMember + "  displayMember: " + displayMember);
            radDropDownList = new Telerik.WinControls.UI.RadDropDownList();
            radDropDownList.DropDownAnimationEnabled = true;
            _verticalCurrentElement += _verticalMargin;
            radDropDownList.Location = new System.Drawing.Point(_horizontalAlignmentLocation, _verticalCurrentElement);
            radDropDownList.Size = _sizeItems;
            radDropDownList.DataSource = dt;
            radDropDownList.ValueMember = valueMember;
            radDropDownList.DisplayMember = displayMember;
            radDropDownList.AutoCompleteMode = AutoCompleteMode.Append;
            this.Controls.Add(radDropDownList);
        }

        private void CreateDatePicker()
        {
            log.Info("Creo datePicker");
            rumDateTimePicker1 = new Controles.RumDateTimePicker();
            rumDateTimePicker1.CalendarSize = new System.Drawing.Size(290, 320);
            _verticalCurrentElement += _verticalMargin;
            rumDateTimePicker1.Location = new System.Drawing.Point(_horizontalAlignmentLocation, _verticalCurrentElement);
            rumDateTimePicker1.Size = _sizeItems;
            rumDateTimePicker1.TabStop = false;
            rumDateTimePicker1.Value = DateTime.Now;
            this.Controls.Add(rumDateTimePicker1);
        }

        #endregion




    }
}

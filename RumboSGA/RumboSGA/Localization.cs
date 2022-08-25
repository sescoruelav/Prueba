using RumboSGA.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace RumboSGA
{

    public class MiRadGridLocalization : RadGridLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadGridStringId.ConditionalFormattingPleaseSelectValidCellValue: return telerikStrings.SelecValorValido;
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValue: return telerikStrings.EstableceValorValido;
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValues: return telerikStrings.EstableceValoresValidos;
                case RadGridStringId.ConditionalFormattingPleaseSetValidExpression: return telerikStrings.EstableceExpresionValida;
                case RadGridStringId.ConditionalFormattingItem: return "Item";
                case RadGridStringId.ConditionalFormattingInvalidParameters: return telerikStrings.ParametrosInvalidos;
                case RadGridStringId.FilterFunctionBetween: return telerikStrings.Entre;
                case RadGridStringId.FilterFunctionContains: return telerikStrings.Contiene;
                case RadGridStringId.FilterFunctionDoesNotContain: return telerikStrings.NoContiene;
                case RadGridStringId.FilterFunctionEndsWith: return telerikStrings.AcabaEn;
                case RadGridStringId.FilterFunctionEqualTo: return telerikStrings.Igual;
                case RadGridStringId.FilterFunctionGreaterThan: return telerikStrings.MayorQue;
                case RadGridStringId.FilterFunctionGreaterThanOrEqualTo: return telerikStrings.MayorIgual;
                case RadGridStringId.FilterFunctionIsEmpty: return telerikStrings.EstaVacio;
                case RadGridStringId.FilterFunctionIsNull: return telerikStrings.EsNulo;
                case RadGridStringId.FilterFunctionLessThan: return telerikStrings.MenorQue;
                case RadGridStringId.FilterFunctionLessThanOrEqualTo: return telerikStrings.MenorIgual;
                case RadGridStringId.FilterFunctionNoFilter: return telerikStrings.SinFiltro;
                case RadGridStringId.FilterFunctionNotBetween: return telerikStrings.NoEntre;
                case RadGridStringId.FilterFunctionNotEqualTo: return telerikStrings.NoIgual;
                case RadGridStringId.FilterFunctionNotIsEmpty: return telerikStrings.NoVacio;
                case RadGridStringId.FilterFunctionNotIsNull: return telerikStrings.NoNulo;
                case RadGridStringId.FilterFunctionStartsWith: return telerikStrings.EmpiezaEn;
                case RadGridStringId.FilterFunctionCustom: return telerikStrings.Personalizado;
                case RadGridStringId.FilterOperatorBetween: return telerikStrings.Entre;
                case RadGridStringId.FilterOperatorContains: return telerikStrings.Contiene;
                case RadGridStringId.FilterOperatorDoesNotContain: return telerikStrings.NoContiene;
                case RadGridStringId.FilterOperatorEndsWith: return telerikStrings.AcabaEn;
                case RadGridStringId.FilterOperatorEqualTo: return telerikStrings.Igual;
                case RadGridStringId.FilterOperatorGreaterThan: return "GreaterThan";
                case RadGridStringId.FilterOperatorGreaterThanOrEqualTo: return "GreaterThanOrEquals";
                case RadGridStringId.FilterOperatorIsEmpty: return "IsEmpty";
                case RadGridStringId.FilterOperatorIsNull: return "IsNull";
                case RadGridStringId.FilterOperatorLessThan: return "LessThan";
                case RadGridStringId.FilterOperatorLessThanOrEqualTo: return "LessThanOrEquals";
                case RadGridStringId.FilterOperatorNoFilter: return "No filter";
                case RadGridStringId.FilterOperatorNotBetween: return "NotBetween";
                case RadGridStringId.FilterOperatorNotEqualTo: return "NotEquals";
                case RadGridStringId.FilterOperatorNotIsEmpty: return "NotEmpty";
                case RadGridStringId.FilterOperatorNotIsNull: return "NotNull";
                case RadGridStringId.FilterOperatorStartsWith: return "StartsWith";
                case RadGridStringId.FilterOperatorIsLike: return "Like";
                case RadGridStringId.FilterOperatorNotIsLike: return "NotLike";
                case RadGridStringId.FilterOperatorIsContainedIn: return "ContainedIn";
                case RadGridStringId.FilterOperatorNotIsContainedIn: return "NotContainedIn";
                case RadGridStringId.FilterOperatorCustom: return telerikStrings.Personalizado;
                case RadGridStringId.CustomFilterMenuItem: return telerikStrings.Personalizado;
                case RadGridStringId.CustomFilterDialogCaption: return "RadGridView Filter Dialog [{0}]";
                case RadGridStringId.CustomFilterDialogLabel: return telerikStrings.EnseñaFilasQue;
                case RadGridStringId.CustomFilterDialogRbAnd: return "And";
                case RadGridStringId.CustomFilterDialogRbOr: return "Or";
                case RadGridStringId.CustomFilterDialogBtnOk: return "OK";
                case RadGridStringId.CustomFilterDialogBtnCancel: return telerikStrings.Cancelar;
                case RadGridStringId.CustomFilterDialogCheckBoxNot: return "Not";
                case RadGridStringId.CustomFilterDialogTrue: return telerikStrings.Verdadero;
                case RadGridStringId.CustomFilterDialogFalse: return telerikStrings.Falso;
                case RadGridStringId.FilterMenuBlanks: return telerikStrings.Vacio;
                case RadGridStringId.FilterMenuAvailableFilters: return telerikStrings.FiltrosDisponibles;
                case RadGridStringId.FilterMenuSearchBoxText: return telerikStrings.Buscar;
                case RadGridStringId.FilterMenuClearFilters: return telerikStrings.LimpiarFiltro;
                case RadGridStringId.FilterMenuButtonOK: return "OK";
                case RadGridStringId.FilterMenuButtonCancel: return telerikStrings.Cancelar;
                case RadGridStringId.FilterMenuSelectionAll: return telerikStrings.Todos;
                case RadGridStringId.FilterMenuSelectionAllSearched: return telerikStrings.TodosResultadosBusqueda;
                case RadGridStringId.FilterMenuSelectionNull: return telerikStrings.Nulo;
                case RadGridStringId.FilterMenuSelectionNotNull: return telerikStrings.NoNulo;
                case RadGridStringId.FilterFunctionSelectedDates: return telerikStrings.FiltrarPorFechas;
                case RadGridStringId.FilterFunctionToday: return telerikStrings.Hoy;
                case RadGridStringId.FilterFunctionYesterday: return telerikStrings.Ayer;
                case RadGridStringId.FilterFunctionDuringLast7days: return telerikStrings.Ultimos7Dias;
                case RadGridStringId.FilterLogicalOperatorAnd: return "AND";
                case RadGridStringId.FilterLogicalOperatorOr: return "OR";
                case RadGridStringId.FilterCompositeNotOperator: return "NOT";
                case RadGridStringId.DeleteRowMenuItem: return telerikStrings.BorrarFila;
                case RadGridStringId.SortAscendingMenuItem: return telerikStrings.OrdenarAscendente;
                case RadGridStringId.SortDescendingMenuItem: return telerikStrings.OrdenarDescendente;
                case RadGridStringId.ClearSortingMenuItem: return telerikStrings.LimpiarOrdenacion;
                case RadGridStringId.ConditionalFormattingMenuItem: return telerikStrings.FormatoCondicional;
                case RadGridStringId.GroupByThisColumnMenuItem: return telerikStrings.AgruparPorEstaCoulmna;
                case RadGridStringId.UngroupThisColumn: return telerikStrings.DesagruparEstacolumna;
                case RadGridStringId.ColumnChooserMenuItem: return telerikStrings.SelectorColumnas;
                case RadGridStringId.HideMenuItem: return telerikStrings.OcultarColumna;
                case RadGridStringId.HideGroupMenuItem: return telerikStrings.OcultarGrupo;
                case RadGridStringId.UnpinMenuItem: return telerikStrings.DesfijarColumna;
                case RadGridStringId.UnpinRowMenuItem: return telerikStrings.DesfijarFila;
                case RadGridStringId.PinMenuItem: return telerikStrings.EstadoFijado;
                case RadGridStringId.PinAtLeftMenuItem: return telerikStrings.FijarIzquierda;
                case RadGridStringId.PinAtRightMenuItem: return telerikStrings.FijarDerecha;
                case RadGridStringId.PinAtBottomMenuItem: return telerikStrings.FijarAbajo;
                case RadGridStringId.PinAtTopMenuItem: return telerikStrings.FijarArriba;
                case RadGridStringId.BestFitMenuItem: return telerikStrings.MejorAjuste;
                case RadGridStringId.PasteMenuItem: return telerikStrings.Pegar;
                case RadGridStringId.EditMenuItem: return telerikStrings.Editar;
                case RadGridStringId.ClearValueMenuItem: return telerikStrings.LimpiarValor;
                case RadGridStringId.CopyMenuItem: return telerikStrings.Copiar;
                case RadGridStringId.CutMenuItem: return telerikStrings.Pegar;
                case RadGridStringId.AddNewRowString: return telerikStrings.ClickAñadirColumna;
                case RadGridStringId.ConditionalFormattingSortAlphabetically: return telerikStrings.OrdenarColumnasAlfabeticamente; ;
                case RadGridStringId.ConditionalFormattingCaption: return telerikStrings.ManagerReglasFormateo;
                case RadGridStringId.ConditionalFormattingLblColumn: return telerikStrings.FormatearSoloCeldas;
                case RadGridStringId.ConditionalFormattingLblName: return telerikStrings.NombreRegla;
                case RadGridStringId.ConditionalFormattingLblType: return telerikStrings.ValorCelda;
                case RadGridStringId.ConditionalFormattingLblValue1: return telerikStrings.Valor1;
                case RadGridStringId.ConditionalFormattingLblValue2: return telerikStrings.Valor2;
                case RadGridStringId.ConditionalFormattingGrpConditions: return "Rules";
                case RadGridStringId.ConditionalFormattingGrpProperties: return telerikStrings.PropiedadesRegla;
                case RadGridStringId.ConditionalFormattingChkApplyToRow: return telerikStrings.AplicarFormatoFilaEntera;
                case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows: return telerikStrings.AplicarFormatoSiFilaSeleccionada;
                case RadGridStringId.ConditionalFormattingBtnAdd: return telerikStrings.AñadirRegla;
                case RadGridStringId.ConditionalFormattingBtnRemove: return telerikStrings.Quitar;
                case RadGridStringId.ConditionalFormattingBtnOK: return "OK";
                case RadGridStringId.ConditionalFormattingBtnCancel: return telerikStrings.Cancelar;
                case RadGridStringId.ConditionalFormattingBtnApply: return telerikStrings.Aplicar;
                case RadGridStringId.ConditionalFormattingRuleAppliesOn: return telerikStrings.ReglaAplicaA;
                case RadGridStringId.ConditionalFormattingCondition: return telerikStrings.Condicion;
                case RadGridStringId.ConditionalFormattingExpression: return telerikStrings.Expresion;
                case RadGridStringId.ConditionalFormattingChooseOne: return telerikStrings.SeleccionaUno;
                case RadGridStringId.ConditionalFormattingEqualsTo: return telerikStrings.IgualValue1;
                case RadGridStringId.ConditionalFormattingIsNotEqualTo: return telerikStrings.NoIgualValue1;
                case RadGridStringId.ConditionalFormattingStartsWith: return telerikStrings.EmpiezaConValue1;
                case RadGridStringId.ConditionalFormattingEndsWith: return telerikStrings.AcabaConValue1;
                case RadGridStringId.ConditionalFormattingContains: return telerikStrings.ContieneValue1;
                case RadGridStringId.ConditionalFormattingDoesNotContain: return telerikStrings.NoContieneValue1;
                case RadGridStringId.ConditionalFormattingIsGreaterThan: return telerikStrings.MayorQueValue1;
                case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual: return telerikStrings.MayorIgualValue1;
                case RadGridStringId.ConditionalFormattingIsLessThan: return telerikStrings.MenorQueValue1;
                case RadGridStringId.ConditionalFormattingIsLessThanOrEqual: return telerikStrings.MenorIgualValue1;
                case RadGridStringId.ConditionalFormattingIsBetween: return telerikStrings.EntreValue1Value2;
                case RadGridStringId.ConditionalFormattingIsNotBetween: return telerikStrings.NoEntreValue1Value2;
                case RadGridStringId.ConditionalFormattingLblFormat: return telerikStrings.Formato;
                case RadGridStringId.ConditionalFormattingBtnExpression: return telerikStrings.EditorExpresiones;
                case RadGridStringId.ConditionalFormattingTextBoxExpression: return telerikStrings.Expresion;
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive: return telerikStrings.SensibleMayusculas;
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor: return telerikStrings.ColorFondoCelda;
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor: return telerikStrings.ColorTextoCelda;
                case RadGridStringId.ConditionalFormattingPropertyGridEnabled: return telerikStrings.Habilitado;
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor: return telerikStrings.ColorFondoFila;
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor: return telerikStrings.ColorTextoFila;
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment: return telerikStrings.AlineamientoTextoFila;
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment: return telerikStrings.AlineamientoTexto;
                case RadGridStringId.ConditionalFormattingPropertyGridCellFont: return telerikStrings.MiFuenteCelda;
                case RadGridStringId.ConditionalFormattingPropertyGridCellFontDescription: return telerikStrings.MiDescriptorFuente;
                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription: return telerikStrings.DescripcionCaseSensitive;
                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription: return telerikStrings.DescripcionCellBackColor;
                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription: return telerikStrings.DescripcionCellForeColor;
                case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription: return telerikStrings.DescripcionEnabled;
                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription: return telerikStrings.DescripcionRowBackColor;
                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription: return telerikStrings.DescripcionRowForeColor;
                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription: return telerikStrings.DescripcionRowTextAlignment;
                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription: return telerikStrings.DescripcionTextAlignment;
                case RadGridStringId.ColumnChooserFormCaption: return telerikStrings.SelectorColumnas;
                case RadGridStringId.ColumnChooserFormMessage: return telerikStrings.MensajeColumnChooser;
                case RadGridStringId.GroupingPanelDefaultMessage: return telerikStrings.MensajePanelAgrupacion;
                case RadGridStringId.GroupingPanelHeader: return telerikStrings.CabeceraAgrupaciones;
                case RadGridStringId.PagingPanelPagesLabel: return telerikStrings.PanelPaginacionEtiqueta;
                case RadGridStringId.PagingPanelOfPagesLabel: return telerikStrings.PanelPaginacionOfPagesEtiqueta;
                case RadGridStringId.NoDataText: return telerikStrings.SinDatosQueMostrar;
                case RadGridStringId.CompositeFilterFormErrorCaption: return telerikStrings.ErrorFiltro;
                case RadGridStringId.CompositeFilterFormInvalidFilter: return telerikStrings.FiltroCompuestoNoValido;
                case RadGridStringId.ExpressionMenuItem: return telerikStrings.Expresion;
                case RadGridStringId.ExpressionFormTitle: return telerikStrings.ConstructorExpresiones;
                case RadGridStringId.ExpressionFormFunctions: return telerikStrings.Funciones;
                case RadGridStringId.ExpressionFormFunctionsText: return telerikStrings.Texto;
                case RadGridStringId.ExpressionFormFunctionsAggregate: return telerikStrings.Agregar;
                case RadGridStringId.ExpressionFormFunctionsDateTime: return "Date-Time";
                case RadGridStringId.ExpressionFormFunctionsLogical: return telerikStrings.Logical;
                case RadGridStringId.ExpressionFormFunctionsMath: return telerikStrings.Math;
                case RadGridStringId.ExpressionFormFunctionsOther: return telerikStrings.Otro;
                case RadGridStringId.ExpressionFormOperators: return telerikStrings.Operadores;
                case RadGridStringId.ExpressionFormConstants: return telerikStrings.Constantes;
                case RadGridStringId.ExpressionFormFields: return telerikStrings.Campos;
                case RadGridStringId.ExpressionFormDescription: return telerikStrings.Descripcion;
                case RadGridStringId.ExpressionFormResultPreview: return telerikStrings.PrevisualizarResultado;
                case RadGridStringId.ExpressionFormTooltipPlus: return telerikStrings.Mas;
                case RadGridStringId.ExpressionFormTooltipMinus: return telerikStrings.Menos;
                case RadGridStringId.ExpressionFormTooltipMultiply: return telerikStrings.Multiplicar;
                case RadGridStringId.ExpressionFormTooltipDivide: return telerikStrings.Dividir;
                case RadGridStringId.ExpressionFormTooltipModulo: return telerikStrings.Modulo;
                case RadGridStringId.ExpressionFormTooltipEqual: return telerikStrings.Igual;
                case RadGridStringId.ExpressionFormTooltipNotEqual: return telerikStrings.NoIgual;
                case RadGridStringId.ExpressionFormTooltipLess: return telerikStrings.Menos;
                case RadGridStringId.ExpressionFormTooltipLessOrEqual: return telerikStrings.MenorOIgual;
                case RadGridStringId.ExpressionFormTooltipGreaterOrEqual: return telerikStrings.MayorOIgual;
                case RadGridStringId.ExpressionFormTooltipGreater: return telerikStrings.Mayor;
                case RadGridStringId.ExpressionFormTooltipAnd: return "Logical \"AND\"";
                case RadGridStringId.ExpressionFormTooltipOr: return "Logical \"OR\"";
                case RadGridStringId.ExpressionFormTooltipNot: return "Logical \"NOT\"";
                case RadGridStringId.ExpressionFormAndButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOrButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormNotButton: return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOKButton: return "OK";
                case RadGridStringId.ExpressionFormCancelButton: return telerikStrings.Cancelar;
                case RadGridStringId.SearchRowChooseColumns: return "SearchRowChooseColumns";
                case RadGridStringId.SearchRowSearchFromCurrentPosition: return "SearchRowSearchFromCurrentPosition";
                case RadGridStringId.SearchRowMenuItemMasterTemplate: return "SearchRowMenuItemMasterTemplate";
                case RadGridStringId.SearchRowMenuItemChildTemplates: return "SearchRowMenuItemChildTemplates";
                case RadGridStringId.SearchRowMenuItemAllColumns: return "SearchRowMenuItemAllColumns";
                case RadGridStringId.SearchRowTextBoxNullText: return "";
                case RadGridStringId.SearchRowResultsOfLabel: return "SearchRowResultsOfLabel";
                case RadGridStringId.SearchRowMatchCase: return "Match case";
            }
            return string.Empty;
        }
    }
    public class MiDataFilterLocalization : Telerik.WinControls.UI.DataFilterLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case DataFilterStringId.LogicalOperatorAnd:
                    return customFiltersStrings.Todo;
                case DataFilterStringId.LogicalOperatorOr:
                    return customFiltersStrings.Cualquiera;
                case DataFilterStringId.LogicalOperatorDescription:
                    return customFiltersStrings.EsCierto;

                case DataFilterStringId.FieldNullText:
                    return customFiltersStrings.SelecCampo;
                case DataFilterStringId.ValueNullText:
                    return customFiltersStrings.IntroduceValor;

                case DataFilterStringId.AddNewButtonText:
                    return customFiltersStrings.Añadir;
                case DataFilterStringId.AddNewButtonExpression:
                    return customFiltersStrings.Expresion;
                case DataFilterStringId.AddNewButtonGroup:
                    return customFiltersStrings.Agrupar;

                case DataFilterStringId.DialogTitle:
                    return customFiltersStrings.FiltroDatos;
                case DataFilterStringId.DialogOKButton:
                    return "OK";
                case DataFilterStringId.DialogCancelButton:
                    return customFiltersStrings.Cancelar;
                case DataFilterStringId.DialogApplyButton:
                    return customFiltersStrings.Aplicar;

                case DataFilterStringId.ErrorAddNodeDialogTitle:
                    return "RadDataFilter Error";
                case DataFilterStringId.ErrorAddNodeDialogText:
                    return "Cannot add entries to the control - missing property descriptors. \nDataSource is not set and/or DataFilterDescriptorItems are not added to the Descriptors collection of the control.";

                case DataFilterStringId.FilterFunctionBetween:
                    return customFiltersStrings.Entre;
                case DataFilterStringId.FilterFunctionContains:
                    return customFiltersStrings.Contiene;
                case DataFilterStringId.FilterFunctionDoesNotContain:
                    return customFiltersStrings.NoContiene;
                case DataFilterStringId.FilterFunctionEndsWith:
                    return customFiltersStrings.AcabaCon;
                case DataFilterStringId.FilterFunctionEqualTo:
                    return customFiltersStrings.Igual;
                case DataFilterStringId.FilterFunctionGreaterThan:
                    return customFiltersStrings.MayorQue;
                case DataFilterStringId.FilterFunctionGreaterThanOrEqualTo:
                    return customFiltersStrings.MayorIgual;
                case DataFilterStringId.FilterFunctionIsEmpty:
                    return customFiltersStrings.EstaVacio;
                case DataFilterStringId.FilterFunctionIsNull:
                    return customFiltersStrings.EsNulo;
                case DataFilterStringId.FilterFunctionLessThan:
                    return customFiltersStrings.MenorQue;
                case DataFilterStringId.FilterFunctionLessThanOrEqualTo:
                    return customFiltersStrings.MenorIgual;
                case DataFilterStringId.FilterFunctionNoFilter:
                    return customFiltersStrings.SinFiltro;
                case DataFilterStringId.FilterFunctionIsContainedIn:
                    return customFiltersStrings.EnLista;
                case DataFilterStringId.FilterFunctionIsNotContainedIn:
                    return customFiltersStrings.NoEnLista;
                case DataFilterStringId.FilterFunctionNotBetween:
                    return customFiltersStrings.NoEntre;
                case DataFilterStringId.FilterFunctionNotEqualTo:
                    return customFiltersStrings.NoIgual;
                case DataFilterStringId.FilterFunctionNotIsEmpty:
                    return customFiltersStrings.NoVacio;
                case DataFilterStringId.FilterFunctionNotIsNull:
                    return customFiltersStrings.NoNulo;
                case DataFilterStringId.FilterFunctionStartsWith:
                    return customFiltersStrings.EmpiezaCon;
                case DataFilterStringId.FilterFunctionCustom:
                    return customFiltersStrings.Personalizado;
            }
            return base.GetLocalizedString(id);
        }
    }

    public class MiRadVirtualGridLocalizationProvider : RadVirtualGridLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadVirtualGridStringId.NoDataText: return telerikStrings.SinDatosQueMostrar;
                case RadVirtualGridStringId.FilterFunctionBetween: return telerikStrings.Entre;
                case RadVirtualGridStringId.FilterFunctionContains: return telerikStrings.Contiene;
                case RadVirtualGridStringId.FilterFunctionDoesNotContain: return telerikStrings.NoContiene;
                case RadVirtualGridStringId.FilterFunctionEndsWith: return telerikStrings.AcabaEn;
                case RadVirtualGridStringId.FilterFunctionEqualTo: return telerikStrings.Igual;
                case RadVirtualGridStringId.FilterFunctionGreaterThan: return telerikStrings.MayorQue;
                case RadVirtualGridStringId.FilterFunctionGreaterThanOrEqualTo: return telerikStrings.MayorOIgual;
                case RadVirtualGridStringId.FilterFunctionIsEmpty: return telerikStrings.EstaVacio;
                case RadVirtualGridStringId.FilterFunctionIsNull: return telerikStrings.EsNulo;
                case RadVirtualGridStringId.FilterFunctionLessThan: return telerikStrings.MenorQue;
                case RadVirtualGridStringId.FilterFunctionLessThanOrEqualTo: return telerikStrings.MenorOIgual;
                case RadVirtualGridStringId.FilterFunctionNoFilter: return telerikStrings.SinFiltro;
                case RadVirtualGridStringId.FilterFunctionNotBetween: return telerikStrings.NoEntre;
                case RadVirtualGridStringId.FilterFunctionNotEqualTo: return telerikStrings.NoIgual;
                case RadVirtualGridStringId.FilterFunctionNotIsEmpty: return telerikStrings.NoVacio;
                case RadVirtualGridStringId.FilterFunctionNotIsNull: return telerikStrings.NoNulo;
                case RadVirtualGridStringId.FilterFunctionStartsWith: return telerikStrings.EmpiezaEn;
                case RadVirtualGridStringId.FilterFunctionCustom: return telerikStrings.Personalizado;
                case RadVirtualGridStringId.FilterOperatorNoFilter: return telerikStrings.SinFiltro;
                case RadVirtualGridStringId.FilterOperatorCustom: return telerikStrings.Personalizado;
                case RadVirtualGridStringId.FilterOperatorIsLike: return telerikStrings.Como;
                case RadVirtualGridStringId.FilterOperatorNotIsLike: return telerikStrings.NoComo;
                case RadVirtualGridStringId.FilterOperatorLessThan: return telerikStrings.MenorQue;
                case RadVirtualGridStringId.FilterOperatorLessThanOrEqualTo: return telerikStrings.MenorQue;
                case RadVirtualGridStringId.FilterOperatorEqualTo: return telerikStrings.Igual;
                case RadVirtualGridStringId.FilterOperatorNotEqualTo: return telerikStrings.NoIgual2;
                case RadVirtualGridStringId.FilterOperatorGreaterThanOrEqualTo: return telerikStrings.MayorOIgual;
                case RadVirtualGridStringId.FilterOperatorGreaterThan: return telerikStrings.MayorQue;
                case RadVirtualGridStringId.FilterOperatorStartsWith: return telerikStrings.EmpiezaEn;
                case RadVirtualGridStringId.FilterOperatorEndsWith: return telerikStrings.AcabaEn;
                case RadVirtualGridStringId.FilterOperatorContains: return telerikStrings.Contiene;
                case RadVirtualGridStringId.FilterOperatorDoesNotContain: return telerikStrings.NoContiene2;
                case RadVirtualGridStringId.FilterOperatorIsNull: return telerikStrings.EsNulo;
                case RadVirtualGridStringId.FilterOperatorNotIsNull: return telerikStrings.NoNulo2;
                case RadVirtualGridStringId.FilterOperatorIsContainedIn: return telerikStrings.ContenidoEn;
                case RadVirtualGridStringId.FilterOperatorNotIsContainedIn: return telerikStrings.NoContenidoEn;
                case RadVirtualGridStringId.AddNewRowString: return telerikStrings.ClickAñadirColumna;
                case RadVirtualGridStringId.PagingPanelPagesLabel: return telerikStrings.PanelPaginacionEtiqueta;
                case RadVirtualGridStringId.PagingPanelOfPagesLabel: return telerikStrings.PanelPaginacionOfPagesEtiqueta;
                case RadVirtualGridStringId.BestFitMenuItem: return telerikStrings.MejorAjuste;
                case RadVirtualGridStringId.ClearSortingMenuItem: return telerikStrings.LimpiarOrdenacion;
                case RadVirtualGridStringId.SortDescendingMenuItem: return telerikStrings.OrdenarDescendente;
                case RadVirtualGridStringId.SortAscendingMenuItem: return telerikStrings.OrdenarAscendente;
                case RadVirtualGridStringId.PinAtRightMenuItem: return telerikStrings.FijarDerecha;
                case RadVirtualGridStringId.PinAtLeftMenuItem: return telerikStrings.FijarIzquierda;
                case RadVirtualGridStringId.PinAtBottomMenuItem: return telerikStrings.FijarAbajo;
                case RadVirtualGridStringId.PinAtTopMenuItem: return telerikStrings.FijarArriba;
                case RadVirtualGridStringId.UnpinColumnMenuItem: return telerikStrings.DesfijarColumna;
                case RadVirtualGridStringId.UnpinRowMenuItem: return telerikStrings.DesfijarFila;
                case RadVirtualGridStringId.PinMenuItem: return telerikStrings.EstadoFijado;
                case RadVirtualGridStringId.DeleteRowMenuItem: return telerikStrings.BorrarFila;
                case RadVirtualGridStringId.ClearValueMenuItem: return telerikStrings.LimpiarValor;
                case RadVirtualGridStringId.EditMenuItem: return telerikStrings.Editar;
                case RadVirtualGridStringId.PasteMenuItem: return telerikStrings.Pegar;
                case RadVirtualGridStringId.CutMenuItem: return telerikStrings.Cortar;
                case RadVirtualGridStringId.CopyMenuItem: return telerikStrings.Copiar;
                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
}

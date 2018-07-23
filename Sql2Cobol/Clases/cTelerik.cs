using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;
using System.Windows.Forms;
using Telerik.WinControls.XmlSerialization;
using System.IO;
using System.Xml;
using System.Text;

namespace CTelerik
{
    public class Mensajes
    {
        public void Error(string mensaje, string titulo = "Error")
        {
            RadMessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, RadMessageIcon.Error);
        }

        public void Informacion(string mensaje, string titulo = "Información")
        {
            RadMessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, RadMessageIcon.Info);
        }

        public void Advertencia(string mensaje, string titulo = "Atención")
        {
            RadMessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, RadMessageIcon.Exclamation);
        }

        public bool Pregunta(string mensaje, string titulo = "Pregunta")
        {
            if (RadMessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                return true;
            else
                return false;
        }
    }

    public class RadCambiarIdiomaGrilla : RadGridLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadGridStringId.ConditionalFormattingPleaseSelectValidCellValue:
                    return "Seleccione un valor válido de celda";

                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValue:
                    return "Seleccione un valor válido de celda";

                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValues:
                    return "Seleccione un valor válido de celda";

                case RadGridStringId.ConditionalFormattingPleaseSetValidExpression:
                    return "Por favor seleccione una expresión válida";

                case RadGridStringId.ConditionalFormattingItem:
                    return "Item";

                case RadGridStringId.ConditionalFormattingInvalidParameters:
                    return "parametros inválidos";

                case RadGridStringId.FilterFunctionBetween:
                    return "Entre";

                case RadGridStringId.FilterFunctionContains:
                    return "Contiene";

                case RadGridStringId.FilterFunctionDoesNotContain:
                    return "No Contiene";

                case RadGridStringId.FilterFunctionEndsWith:
                    return "Finaliza con";

                case RadGridStringId.FilterFunctionEqualTo:
                    return "Igual a";

                case RadGridStringId.FilterFunctionGreaterThan:
                    return "Mayor a";

                case RadGridStringId.FilterFunctionGreaterThanOrEqualTo:
                    return "Mayor o igual a";

                case RadGridStringId.FilterFunctionIsEmpty:
                    return "Esta vacio";

                case RadGridStringId.FilterFunctionIsNull:
                    return "Es nulo";

                case RadGridStringId.FilterFunctionLessThan:
                    return "Menor a";

                case RadGridStringId.FilterFunctionLessThanOrEqualTo:
                    return "Menor ó igual a";

                case RadGridStringId.FilterFunctionNoFilter:
                    return "Sin Filtros";

                case RadGridStringId.FilterFunctionNotBetween:
                    return "Fuera";

                case RadGridStringId.FilterFunctionNotEqualTo:
                    return "Distinto a";

                case RadGridStringId.FilterFunctionNotIsEmpty:
                    return "No es vacio";

                case RadGridStringId.FilterFunctionNotIsNull:
                    return "No es nulo";

                case RadGridStringId.FilterFunctionStartsWith:
                    return "Empieza con";

                case RadGridStringId.FilterFunctionCustom:
                    return "Personalizado";

                case RadGridStringId.FilterOperatorBetween:
                    return "Entre";

                case RadGridStringId.FilterOperatorContains:
                    return "Contiene";

                case RadGridStringId.FilterOperatorDoesNotContain:
                    return "No Contiene";

                case RadGridStringId.FilterOperatorEndsWith:
                    return "Termina Con";

                case RadGridStringId.FilterOperatorEqualTo:
                    return "Igual";

                case RadGridStringId.FilterOperatorGreaterThan:
                    return "Mayor que";

                case RadGridStringId.FilterOperatorGreaterThanOrEqualTo:
                    return "Mayor Igual que";

                case RadGridStringId.FilterOperatorIsEmpty:
                    return "Esta Vacio";

                case RadGridStringId.FilterOperatorIsNull:
                    return "Es nullo";

                case RadGridStringId.FilterOperatorLessThan:
                    return "Menor que";

                case RadGridStringId.FilterOperatorLessThanOrEqualTo:
                    return "Menor Igual que";

                case RadGridStringId.FilterOperatorNoFilter:
                    return "Sin filtros";

                case RadGridStringId.FilterOperatorNotBetween:
                    return "Fuera";

                case RadGridStringId.FilterOperatorNotEqualTo:
                    return "Distinto";

                case RadGridStringId.FilterOperatorNotIsEmpty:
                    return "Lleno";

                case RadGridStringId.FilterOperatorNotIsNull:
                    return "No nulo";

                case RadGridStringId.FilterOperatorStartsWith:
                    return "Comienza con";

                case RadGridStringId.FilterOperatorIsLike:
                    return "como";

                case RadGridStringId.FilterOperatorNotIsLike:
                    return "Distinto";

                case RadGridStringId.FilterOperatorIsContainedIn:
                    return "Contenido en ";

                case RadGridStringId.FilterOperatorNotIsContainedIn:
                    return "No contenido en ";

                case RadGridStringId.FilterOperatorCustom:
                    return "Persolalizado";

                case RadGridStringId.CustomFilterMenuItem:
                    return "Persolalizado";

                case RadGridStringId.CustomFilterDialogCaption:
                    return "Dialogo [{0}]";

                case RadGridStringId.CustomFilterDialogLabel:
                    return "Mostrar filas donde:";

                case RadGridStringId.CustomFilterDialogRbAnd:
                    return "Y";

                case RadGridStringId.CustomFilterDialogRbOr:
                    return "O";

                case RadGridStringId.CustomFilterDialogBtnOk:
                    return "OK";

                case RadGridStringId.CustomFilterDialogBtnCancel:
                    return "Cancelar";

                case RadGridStringId.CustomFilterDialogCheckBoxNot:
                    return "No";

                case RadGridStringId.CustomFilterDialogTrue:
                    return "True";

                case RadGridStringId.CustomFilterDialogFalse:
                    return "False";

                case RadGridStringId.FilterMenuBlanks:
                    return "Vacio";

                case RadGridStringId.FilterMenuAvailableFilters:
                    return "Filtros Disponibles";

                case RadGridStringId.FilterMenuSearchBoxText:
                    return "Buscar...";

                case RadGridStringId.FilterMenuClearFilters:
                    return "Limpiar Filtros";

                case RadGridStringId.FilterMenuButtonOK:
                    return "OK";

                case RadGridStringId.FilterMenuButtonCancel:
                    return "Cancelar";

                case RadGridStringId.FilterMenuSelectionAll:
                    return "Todos";

                case RadGridStringId.FilterMenuSelectionAllSearched:
                    return "Buscar todos los resultados";

                case RadGridStringId.FilterMenuSelectionNull:
                    return "Nulo";

                case RadGridStringId.FilterMenuSelectionNotNull:
                    return "No Nulo";

                case RadGridStringId.FilterFunctionSelectedDates:
                    return "Filtro por una fecha especifica:";

                case RadGridStringId.FilterFunctionToday:
                    return "Hoy";

                case RadGridStringId.FilterFunctionYesterday:
                    return "Ayer";

                case RadGridStringId.FilterFunctionDuringLast7days:
                    return "Durante los ultimos 7 dias";

                case RadGridStringId.FilterLogicalOperatorAnd:
                    return "Y";

                case RadGridStringId.FilterLogicalOperatorOr:
                    return "O";

                case RadGridStringId.FilterCompositeNotOperator:
                    return "NO";

                case RadGridStringId.DeleteRowMenuItem:
                    return "Borrar fila";

                case RadGridStringId.SortAscendingMenuItem:
                    return "Ordenamiento ascendiente";

                case RadGridStringId.SortDescendingMenuItem:
                    return "Ordenamiento descencidente";

                case RadGridStringId.ClearSortingMenuItem:
                    return "Inicicializar el ordenamiento";

                case RadGridStringId.ConditionalFormattingMenuItem:
                    return "Formateo condicional";

                case RadGridStringId.GroupByThisColumnMenuItem:
                    return "Agrupar por esta columna";

                case RadGridStringId.UngroupThisColumn:
                    return "Desagrupar esta columna";

                case RadGridStringId.ColumnChooserMenuItem:
                    return "Elegir columna";

                case RadGridStringId.HideMenuItem:
                    return "Ocultar columna";

                case RadGridStringId.HideGroupMenuItem:
                    return "Ocultar grupo";

                case RadGridStringId.UnpinMenuItem:
                    return "Liberar columna";

                case RadGridStringId.UnpinRowMenuItem:
                    return "Liberar la fila";

                case RadGridStringId.PinMenuItem:
                    return "Liberar estado";

                case RadGridStringId.PinAtLeftMenuItem:
                    return "PIN a la izquierda";

                case RadGridStringId.PinAtRightMenuItem:
                    return "Pasador a la derecha";

                case RadGridStringId.PinAtBottomMenuItem:
                    return "Perno en la parte inferior";

                case RadGridStringId.PinAtTopMenuItem:
                    return "Perno en la parte superior";

                case RadGridStringId.BestFitMenuItem:
                    return "Mejor ajuste";

                case RadGridStringId.PasteMenuItem:
                    return "Pegar";

                case RadGridStringId.EditMenuItem:
                    return "Editar";

                case RadGridStringId.ClearValueMenuItem:
                    return "Limpiar Valor";

                case RadGridStringId.CopyMenuItem:
                    return "Copiar";

                case RadGridStringId.CutMenuItem:
                    return "Cortar";

                case RadGridStringId.AddNewRowString:
                    return "Haga clic aquí para agregar una nueva fila";

                case RadGridStringId.SearchRowResultsOfLabel:
                    return "de";

                case RadGridStringId.SearchRowMatchCase:
                    return "Match case";

                case RadGridStringId.ConditionalFormattingSortAlphabetically:
                    return "Ordenar columnas por orden alfabético";

                case RadGridStringId.ConditionalFormattingCaption:
                    return "Conditional Formatting Rules Manager";

                case RadGridStringId.ConditionalFormattingLblColumn:
                    return "Formato de celdas sólo con";

                case RadGridStringId.ConditionalFormattingLblName:
                    return "Nombre de la regla";

                case RadGridStringId.ConditionalFormattingLblType:
                    return "Valor de la celda";

                case RadGridStringId.ConditionalFormattingLblValue1:
                    return "Valor 1";

                case RadGridStringId.ConditionalFormattingLblValue2:
                    return "Valor 2";

                case RadGridStringId.ConditionalFormattingGrpConditions:
                    return "reglas.";

                case RadGridStringId.ConditionalFormattingGrpProperties:
                    return "Propiedades de la regla";

                case RadGridStringId.ConditionalFormattingChkApplyToRow:
                    return "Aplicar este formato a la fila completa";

                case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows:
                    return "Aplicar este formato si se selecciona la fila";

                case RadGridStringId.ConditionalFormattingBtnAdd:
                    return "Añade una nueva regla";

                case RadGridStringId.ConditionalFormattingBtnRemove:
                    return "Eliminar";

                case RadGridStringId.ConditionalFormattingBtnOK:
                    return "OK";

                case RadGridStringId.ConditionalFormattingBtnCancel:
                    return "Cancelar";

                case RadGridStringId.ConditionalFormattingBtnApply:
                    return "Aplicar";

                case RadGridStringId.ConditionalFormattingRuleAppliesOn:
                    return "Regla se aplica a";

                case RadGridStringId.ConditionalFormattingCondition:
                    return "Condición";

                case RadGridStringId.ConditionalFormattingExpression:
                    return "Expresión";

                case RadGridStringId.ConditionalFormattingChooseOne:
                    return "[Elegir una]";

                case RadGridStringId.ConditionalFormattingEqualsTo:
                    return "es igual a [Valor1]";

                case RadGridStringId.ConditionalFormattingIsNotEqualTo:
                    return "no es igual a [valor1]";

                case RadGridStringId.ConditionalFormattingStartsWith:
                    return "Se inicia con [Valor1]";

                case RadGridStringId.ConditionalFormattingEndsWith:
                    return "termina con [Valor1]";

                case RadGridStringId.ConditionalFormattingContains:
                    return "contiene [Valor1]";

                case RadGridStringId.ConditionalFormattingDoesNotContain:
                    return "no contiene [Valor1]";

                case RadGridStringId.ConditionalFormattingIsGreaterThan:
                    return "es mayor que [Valor1]";

                case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual:
                    return "es mayor o igual [Valor1]";

                case RadGridStringId.ConditionalFormattingIsLessThan:
                    return "es menor que [valor1]";

                case RadGridStringId.ConditionalFormattingIsLessThanOrEqual:
                    return "es menor o igual al [valor1]";

                case RadGridStringId.ConditionalFormattingIsBetween:
                    return "está entre [valor1] y [valor2]";

                case RadGridStringId.ConditionalFormattingIsNotBetween:
                    return "no es entre [valor1] y [valor1]";

                case RadGridStringId.ConditionalFormattingLblFormat:
                    return "Formato";

                case RadGridStringId.ConditionalFormattingBtnExpression:
                    return "Editor de expresiones";

                case RadGridStringId.ConditionalFormattingTextBoxExpression:
                    return "Expresión";

                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive:
                    return "Distingue mayúsculas y minúsculas";

                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor:
                    return "CellBackColor";

                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor:
                    return "CellForeColor";

                case RadGridStringId.ConditionalFormattingPropertyGridEnabled:
                    return "Habilitado";

                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor:
                    return "RowBackColor";

                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor:
                    return "RowForeColor";

                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment:
                    return "Alineación de texto de fila";

                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment:
                    return "Alineación del texto";

                case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription:
                    return "Determina si se realizará comparaciones entre mayúsculas y minúsculas cuando se evalúan los valores de cadena.";

                case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription:
                    return "Introduzca el color de fondo que se utilizará para la célula.";

                case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription:
                    return "Introduzca el color de primer plano que se utilizará para la célula.";

                case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription:
                    return "Determina si está habilitada la condición (puede ser evaluado y aplicado).";

                case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription:
                    return "Escriba el color de fondo para toda la fila.";

                case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription:
                    return "Escriba el color de primer plano para ser utilizado por toda la fila.";

                case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription:
                    return "Entrar en la alineación a utilizarse para los valores de las celdas, cuando vale para aplicar a la fila.";

                case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription:
                    return "Entrar en la alineación para los valores de las celdas.";

                case RadGridStringId.ColumnChooserFormCaption:
                    return "Selector de la columna";

                case RadGridStringId.ColumnChooserFormMessage:
                    return "Arrastre un encabezado de columna de la grilla aquí para quitar vista actual frommato.";

                case RadGridStringId.GroupingPanelDefaultMessage:
                    return "Arrastre una columna aqui para agrupar por esta.";

                case RadGridStringId.GroupingPanelHeader:
                    return "Agrupar por:";

                case RadGridStringId.PagingPanelPagesLabel:
                    return "Página";

                case RadGridStringId.PagingPanelOfPagesLabel:
                    return "de";

                case RadGridStringId.NoDataText:
                    return "No hay datos para mostrar";

                case RadGridStringId.CompositeFilterFormErrorCaption:
                    return "Error de filtro";

                case RadGridStringId.CompositeFilterFormInvalidFilter:
                    return "El descriptor de filtro compuesto no es válido.";

                case RadGridStringId.ExpressionMenuItem:
                    return "Expresión";

                case RadGridStringId.ExpressionFormTitle:
                    return "Generador de expresiones";

                case RadGridStringId.ExpressionFormFunctions:
                    return "Funciones";

                case RadGridStringId.ExpressionFormFunctionsText:
                    return "Texto";

                case RadGridStringId.ExpressionFormFunctionsAggregate:
                    return "Agregado";

                case RadGridStringId.ExpressionFormFunctionsDateTime:
                    return "Fecha y hora";

                case RadGridStringId.ExpressionFormFunctionsLogical:
                    return "Lógico";

                case RadGridStringId.ExpressionFormFunctionsMath:
                    return "Matemáticas";

                case RadGridStringId.ExpressionFormFunctionsOther:
                    return "Otros";

                case RadGridStringId.ExpressionFormOperators:
                    return "Operadores";

                case RadGridStringId.ExpressionFormConstants:
                    return "Constantes";

                case RadGridStringId.ExpressionFormFields:
                    return "Campos";

                case RadGridStringId.ExpressionFormDescription:
                    return "Descripción";

                case RadGridStringId.ExpressionFormResultPreview:
                    return "Vista previa del resultado";

                case RadGridStringId.ExpressionFormTooltipPlus:
                    return "Más";

                case RadGridStringId.ExpressionFormTooltipMinus:
                    return "Menos";

                case RadGridStringId.ExpressionFormTooltipMultiply:
                    return "Multiplicar";

                case RadGridStringId.ExpressionFormTooltipDivide:
                    return "Dividir";

                case RadGridStringId.ExpressionFormTooltipModulo:
                    return "Modulo";

                case RadGridStringId.ExpressionFormTooltipEqual:
                    return "Igual";

                case RadGridStringId.ExpressionFormTooltipNotEqual:
                    return "Distinto";

                case RadGridStringId.ExpressionFormTooltipLess:
                    return "Menos";

                case RadGridStringId.ExpressionFormTooltipLessOrEqual:
                    return "Menor o igual";

                case RadGridStringId.ExpressionFormTooltipGreaterOrEqual:
                    return "Mayor o igual";

                case RadGridStringId.ExpressionFormTooltipGreater:
                    return "Mayor";

                case RadGridStringId.ExpressionFormTooltipAnd:
                    return "Logical \"AND\"";

                case RadGridStringId.ExpressionFormTooltipOr:
                    return "Logical \"OR\"";

                case RadGridStringId.ExpressionFormTooltipNot:
                    return "Logical \"NOT\"";

                case RadGridStringId.ExpressionFormAndButton:
                    return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOrButton:
                    return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormNotButton:
                    return string.Empty; //if empty, default button image is used
                case RadGridStringId.ExpressionFormOKButton:
                    return "OK";

                case RadGridStringId.ExpressionFormCancelButton:
                    return "Cancelar";
            }
            return string.Empty;
        }
    }

    public class SpanishCommandBarLocalizationProvider : CommandBarLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case CommandBarStringId.CustomizeDialogChooseToolstripLabelText:
                    return "Elija un tooltip para organizar:";

                case CommandBarStringId.CustomizeDialogCloseButtonText:
                    return "Cerrar";

                case CommandBarStringId.CustomizeDialogItemsPageTitle:
                    return "Elementos";

                case CommandBarStringId.CustomizeDialogMoveDownButtonText:
                    return "Bajar";

                case CommandBarStringId.CustomizeDialogMoveUpButtonText:
                    return "Subir";

                case CommandBarStringId.CustomizeDialogResetButtonText:
                    return "Resetear";

                case CommandBarStringId.CustomizeDialogTitle:
                    return "Customizar";

                case CommandBarStringId.CustomizeDialogToolstripsPageTitle:
                    return "Toolstrips";

                case CommandBarStringId.OverflowMenuAddOrRemoveButtonsText:
                    return "Agregar o borrar botones";

                case CommandBarStringId.OverflowMenuCustomizeText:
                    return "Customizar...";

                case CommandBarStringId.ContextMenuCustomizeText:
                    return "Customizar...";

                default:
                    return base.GetLocalizedString(id);
            }
        }
    }
}
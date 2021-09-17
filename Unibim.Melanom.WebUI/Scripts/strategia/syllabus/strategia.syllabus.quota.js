$('#UnitxNo').live("change", function () {
    $('#Unit_Name').val($('#UnitxNo option:selected').text());

    if ($(this).val() == 0) {
        $('#div-program').hide();
    }
});

$('#Program_Department_No').live("change", function () {

    $('#Program_Department_Name').val($('#Program_Department_No option:selected').text());
    $('#Program_EducationType_Name').val($('#Program_No option:selected').text());

    if ($(this).val() > 0) {
        $('#div-program').show();
    }
    else {
        $('#div-program').hide();
    }
});

$('#Program_No').live("change", function () {
    if ($(this).val() > 0) {
        $('#Program_EducationType_Name').val($('#Program_No option:selected').text());
    }
});

$(document).ready(function () {

    CustomAutoComplete("/lesson/searchlesson", "LessonName");

    $('#Unit_Name').val($('#UnitxNo option:selected').text());
    $('#Program_Department_Name').val($('#Program_Department_No option:selected').text());
    $('#Program_EducationType_Name').val($('#Program_No option:selected').text());
});

function CustomAutoCompleted(uivalue, uilabel, spec1, spec2, spec3) {
    $("#LessonNo").val(uivalue);
}
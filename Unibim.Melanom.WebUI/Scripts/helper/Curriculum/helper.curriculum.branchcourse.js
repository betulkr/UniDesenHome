function CustomOnSuccess(result, status, xhr) {
    OnSuccess(result, status, xhr); //Required : Base OnSuccess Function --> ~/Scripts/helper.CIMS.js
    $("#course-list").find('tbody').html(result);
    iButtonEvents();
}

function iButtonEvents() {
    $('.on_off :checkbox, .on_off :radio').iButton({
        change: function ($input) {

            var yearNo, courseNo, branchTypeNo, programNo, branchProgramNo, isChecked;
            yearNo = $('#YearNo').val();
            courseNo = $input.val();
            branchTypeNo = $('#BranchTypeNo').val();
            programNo = $('#ProgramNo').val();
            branchProgramNo = $('#BranchProgramNo').val();
            isChecked = $input.is(":checked");

            $.ajax({
                url: '/Curriculum/AddOrRemoveBranchCourse',
                type: 'POST',
                data: { yearNo: yearNo, courseNo: courseNo, branchTypeNo: branchTypeNo, programNo: programNo, branchProgramNo: branchProgramNo, isChecked: isChecked },
                success: function (result) {
                    ExecuteResult(result);
                },
                error: function () {
                    Dialog(xhr + '-' + status + '-' + error);
                }
            });
        },
        labelOn: "",
        labelOff: "",
        enableDrag: false,
        allowRadioUncheck: true
    });
}

$('#YearNo').change(function () {
    var departmentEducationTypeNo, yearNo, curriculumDropDown;
    departmentEducationTypeNo = $('#ProgramNo').val();
    yearNo = $(this).val();
    curriculumDropDown = $('#CurriculumNo');

    ShowLoader();

    $.ajax({
        url: '/curriculum/searchcurriculum',
        data: { departmentEducationTypeNo: departmentEducationTypeNo, yearNo: yearNo },
        cache: false,
        type: 'GET',
        success: function (items) {
            curriculumDropDown.empty();
            curriculumDropDown.removeAttr("disabled");

            if (items != null) {
                $.each(items, function (index, item) {
                    curriculumDropDown.append($('<option/>', {
                        value: item.No,
                        text: item.Name
                    }));
                });
            }

            HideLoader();
        },
        error: function (xhr, status, error) {
            HideLoader();
            Dialog(xhr + '-' + status + '-' + error);
        }
    });
});

$(function () {
    $(".student-select").live("click", function () {
        ShowLoader();
        var srnumber, sno, sfirstname, slastname, sTrIdentityNumber, sCurriculumNo, next, number, isActive;
        srnumber = $(this).data("srnumber");
        sno = $(this).data("sno");
        sfirstname = $(this).data("sfirstname");
        slastname = $(this).data("slastname");
        sTrIdentityNumber = $(this).data("stridentitynumber");
        sCurriculumNo = $(this).data("scurriculumno");
        next = $(this).data("next");
        number = $(this).data("number");
        isActive = $(this).data("isactive");
        isMaster = $(this).data("ismaster");

        $.ajax({
            url: "/student/selectstudent",
            cache: false,
            data: { No: srnumber, StudentNo: sno, Firstname: sfirstname, Lastname: slastname, TRIdentityNumber: sTrIdentityNumber, CurriculumNo: sCurriculumNo, StudentNumber: number, IsActive: isActive },
            success: function (result) {

                $(".student-panel").find("#sName").html(result.Model.Firstname + ' ' + result.Model.Lastname);
                $(".student-panel").find("#sTrIdentityNo").html(result.Model.TRIdentityNumber);
                $(".student-panel").find("#sStudentNumber").html(result.Model.StudentNumber);
                $(".student-panel").find(".student-transcript").attr("data-srnumber", result.Model.No);
                $(".student-panel").find(".student-transcript").attr("data-ismaster", result.Model.IsMaster);
                $(".student-panel").find("img").attr('src', result.ImgSrc);

                var financialText = result.FinancialApprovalText;
                var exceededSemesterText = result.ExceededSemesterText;

                if (financialText != undefined && financialText != '' && financialText != null) {
                    $(".student-panel").find("#FinancialApproval").text(result.FinancialApprovalText);
                }
                else {
                    $(".student-panel").find("#FinancialApproval").text('');
                }

                if (exceededSemesterText != undefined && exceededSemesterText != '' && exceededSemesterText != null) {
                    $(".student-panel").find("#ExceededSemesterText").text(result.ExceededSemesterText);
                }
                else {
                    $(".student-panel").find("#ExceededSemesterText").text('');
                }

                if (next != undefined & next != '' & next != null) {
                    ShowLoader();
                    window.location.href = next;
                }

                $(".student-panel-selectStudent").hide();
                $(".student-panel").showRightPanel();
                setTimeout(function () { $(".student-panel").hideRightPanel(); }, 2000);
                HideLoader();
            },
            error: function (xhr, status, error) {
                Dialog(xhr + ' ' + status + ' ' + error);
                HideLoader();
            }
        });

        return false;
    });

    $(".student-panel .label").click(function () {
        var cssClass = $(".student-panel").attr("class");
        var personnelCssClass = $(".personnel-panel").attr("class");

        if (cssClass.indexOf("right-panel-show") < 0) {
            $(".student-panel").showRightPanel();
            $('.personnel-panel-selectPersonnel').hide();
            if (personnelCssClass.indexOf("right-panel-show")) {
                $(".personnel-panel").hideRightPanel();
            }
        }
        else {
            $(".student-panel").hideRightPanel();
            $('.personnel-panel-selectPersonnel').show();

        }
    });

    $(".student-transcript").live("click", function () {

        var srnumber = $(this).attr('data-srnumber');
        var ismaster = $(this).attr('data-ismaster');

        console.warn(srnumber);
        console.warn(ismaster);

        ShowTranscript(srnumber, ismaster);
    });

    $(".student-suspended").live("click", function () {
        var studentnumber = $(this).attr('data-studentnumber');
        $.ajax({
            data: { studentnumber: studentnumber },
            url: "/student/updatesuspendedstudent",
            type: "POST",
            beforeSend: function (xhr) {
                ShowLoader();
            },
            success: function (result) {
                HideLoader();
                Growl(result);
            }
        });
    });


    $.fn.showRightPanel = function () {
        $(this).addClass("right-panel-show").removeClass("right-panel-nothing").removeClass("right-panel-hide");
        $("#student-labelArrow").removeClass("icon-arrow-up-3").addClass("icon-arrow-down-3");
    };

    $.fn.hideRightPanel = function () {
        $(this).addClass("right-panel-hide").removeClass("right-panel-show");
        $("#student-labelArrow").removeClass("icon-arrow-down-3").addClass("icon-arrow-up-3");
    };

    $("#forget-student").live('click', function () {
        $(".student-panel-selectStudent").show();
    });
});
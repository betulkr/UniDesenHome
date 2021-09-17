

$(function () {
    $(".selectPersonnel").live("click", function () {
        ShowLoader();
        var userNo, firstname, lastname, identitynumber, academicpersonnelno, administrativepersonnelno, next, roleid;
        userNo= $(this).data("personnelno");
        firstname = $(this).data("firstname");
        lastname = $(this).data("lastname");
        identitynumber = $(this).data("identitynumber");
        academicpersonnelno = $(this).data("academicpersonnelno");
        administrativepersonnelno = $(this).data("administrativepersonnelno");
        next = $(this).data("next");
        roleid = $(this).data("roleid");
        //isActive = $(this).data("isactive");

        $.ajax({
            url: "/personnel/selectpersonnel",
            cache: false,
            data: { userNo: userNo, roleId: roleid, returnUrl: next },
            success: function (result) {

                $(".personnel-panel").find("#pName").html(result.Model.Firstname + ' ' + result.Model.lastname);
                $(".personnel-panel").find("#pTrIdentityNo").html(result.Model.IdentityNumber);
                $(".personnel-panel").find("#pType").html(result.Model.PersonnelType);
                
                //$(".student-panel").find("img").attr('src', result.ImgSrc);

                if (next != undefined & next != '' & next != null) {
                    ShowLoader();
                    window.location.href = next;
                }

                $(".personnel-panel-selectPersonnel").hide();
                $(".personnel-panel").showRightPanel();
                setTimeout(function () { $(".personnel-panel").hideRightPanel(); }, 2000);
                HideLoader();
            },
            error: function (xhr, status, error) {
                Dialog(xhr + ' ' + status + ' ' + error);
                HideLoader();
            }
        });

        //return false;
    });

    $(".personnel-panel .label").click(function () {
        var cssClass = $(".personnel-panel").attr("class");
        var studentCssClass = $(".student-panel").attr("class");

        if (cssClass.indexOf("right-panel-show") < 0) {
            $(".personnel-panel").showRightPanel();
            if (studentCssClass.indexOf("right-panel-show"))
            {
                $(".student-panel").hideRightPanel();
            }
        }
        else {
            $(".personnel-panel").hideRightPanel();

        }
    });

    $.fn.showRightPanel = function () {
        $(this).addClass("right-panel-show").removeClass("right-panel-nothing").removeClass("right-panel-hide");
        $("#student-labelArrow").removeClass("icon-arrow-up-3").addClass("icon-arrow-down-3");
    };

    $.fn.hideRightPanel = function () {
        $(this).addClass("right-panel-hide").removeClass("right-panel-show");
        $("#student-labelArrow").removeClass("icon-arrow-down-3").addClass("icon-arrow-up-3");
    };

    $("#forget-personnel").live('click', function () {
        $(".student-panel-selectPersonnel").show();
    });
});
$(".titleIcon input:checkbox").live("click", function () {
    var checkedStatus = this.checked;

    $(".checkAll tbody tr").each(function () {
        this.checked = checkedStatus;
        if (this.checked) {
            if ($(this).hasClass("message-unread")) {
                //as read...
                $("#li-read").removeClass("displayNone");
            }
            else if ($(this).hasClass("message-read")) {
                //as unread...
                $("#li-unread").removeClass("displayNone");
            }
            $("#li-delete").removeClass("displayNone");
        }
        else {
            $("#li-read").addClass("displayNone");
            $("#li-unread").addClass("displayNone");
            $("#li-delete").addClass("displayNone");
        }
    });
});

$('.checkAll tbody tr td:first-child input[type=checkbox]').live("change", function () {
    var $isRead = 0,
        $isUnRead = 0;

    $(".checkAll tbody tr td:first-child input[type=checkbox]").each(function () {
        if ($(this).closest("tr").hasClass("message-unread")) {
            if (this.checked) {
                $isUnRead++;
            }
        }
        else {
            if (this.checked) {
                $isRead++;
            }
        }
    });

    $("#li-read").addClass("displayNone");
    $("#li-unread").addClass("displayNone");
    $("#li-delete").addClass("displayNone");

    if ($isRead > 0) {
        $("#li-unread").removeClass("displayNone");
        $("#li-delete").removeClass("displayNone");
    }
    if ($isUnRead > 0) {
        $("#li-read").removeClass("displayNone");
        $("#li-delete").removeClass("displayNone");
    }
});

$(".tToolbar li[id*=li] a").live("click", function () {
    $href = $(this).attr("href");
    $form = $(this).closest("form");
    PostForm($href, $form);
    return false;
});

function PostForm(href, form) {
    $.ajax({
        url: href,
        type: 'POST',
        cache: false,
        data: form.serialize(),
        success: function (result) {
            ExecuteResult(result.InterfaceArgument);
            $("#messageList").find("tbody").html(result.Html);
            $(".titleIcon input:checkbox").closest('.checker > span').removeClass('checked');
            $(".checkAll tbody tr td:first-child input[type=checkbox]").uniform();

            $("#li-read").addClass("displayNone");
            $("#li-unread").addClass("displayNone");
            $("#li-delete").addClass("displayNone");
        },
        error: function (xhr, status, error) {
            Dialog(xhr + '-' + status + '-' + error);
        }
    });
}
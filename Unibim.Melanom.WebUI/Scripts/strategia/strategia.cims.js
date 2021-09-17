var $ajaxLoader, $dialog;

$(document).ready(function () {
    $.validator.methods["date"] = function (value, element) { return true; }
    $.validator.methods.number = function (value, element) {
        if (value.indexOf(".") >= 0) {
            return false;
        }
        return !isNaN(parseFloat(value));
    }
    $(".nav-stacked").find("li.active").closest("ul").closest("li.dropdown").addClass("active");
    $ajaxLoader = '<div class = "textC"><img src = "' + GLOBAL_VARIABLES.LoaderImagePath + '" /></div>';

    $dialog = $(".strategia-dialog").dialog({
        modal: true,
        bgiframe: true,
        autoOpen: false,
        resizable: false
    });
});

$(window).load(function () {
    HideLoader();
});

$(window).scroll(function () {
    var uzunluk = $(document).scrollTop();

    if (uzunluk > 300) {
        $("#top-link").fadeIn(500);
    }
    else {
        $("#top-link").fadeOut(500);
    }

    if (uzunluk > 525) {
        $("#top-lesson-link").fadeIn(500);
    }
    else {
        $("#top-lesson-link").fadeOut(500);
    }
});

$(window).resize(function () {
    var bodyHeight = $("body").height();
    if (bodyHeight <= (387 + $("#top").height())) {
        $("#login-wrap").css("top", "100px");
        $("#login-wrap").css("margin-top", "0");

        $(".errorWrapper").css("top", "100px");
        $(".errorWrapper").css("margin-top", "0");
    }
    else {
        $("#login-wrap").removeAttr("style");
        $(".errorWrapper").removeAttr("style");
    }
});

$(function () {
    $("#top-link").click(function () {
        $("html,body").stop().animate({ scrollTop: "0" }, 1000);
    });

    $(".report").live('click', function (result) {
        //window.open($(this).data("href"), "customWindow", "width=1060, height=560, top=50, left=50, resizable=no, scroll='no'");
        var href = $(this).data('href');
        $.ajax({
            type: "POST",
            url: href,
            data: {},
            beforeSend: function (xhr) {
                ShowLoader();
            }
        }).done(function (result) {
            HideLoader();
            ExecuteResult(result);
        })
        .fail(function () {
            alert("error");
        });

        return false;
    });

    $('.active-dept a').live('click', function () {
        $this = $(this);
        $srn = $this.data('srn');
        $href = $this.data('href');

        if ($this.closest('li').hasClass('selected-dept')) {
            return false;
        }
        else {

            $.ajax({
                type: "POST",
                url: $href,
                data: { srn: $srn, returnURL: window.location.href },
                beforeSend: function (xhr) {
                    ShowLoader();
                }
            }).done(function (result) {
                HideLoader();
                ExecuteResult(result);
            })
            .fail(function () {
                alert("error");
            });
            return false;
        }
    });
});



function DisableInteraction() {
    ShowLoader();
    $("input[type=submit], input[type=button]").each(function (index, data) {
        if ($(this).is(':disabled') == false) {
            $(this).attr("disabled", "disabled");
            $(this).addClass("autodisabled");
        }
    });
}

function EnableInteraction() {
    HideLoader();
    $(".autodisabled").each(function (index, data) {
        $(this).removeAttr("disabled");
        $(this).removeClass("autodisabled");
    });
}

function ExecuteResult(result) {
    if (result.Action == undefined) {
        Failure(ERROR_MESSAGES.UndefinedError);
        return false;
    }
    else if (result.Action == "Dialog") {
        Dialog(result.Message);
    }
    else if (result.Action == "Redirect") {
        window.location = result.Href;
    }
    else if (result.Action == "RedirectBlank") {
        var gen = screen.availWidth;
        var yuk = screen.availHeight;
        gen = gen - 20;
        yuk = yuk - 60;
        window.open(result.Href, "_blank", "menubar=0,toolbar=0,scrollbars=1,resizable=1,status=0,titlebar=0,top=0px,left=0px,width=" + gen + ",height=" + yuk + "");
    }
    else if (result.Action == "RedirectReport") {

        var $dialog_report;
        var _width = $(window).width() - 100;
        var _height = $(window).height() - 30;
        var queryString = "";
        var closeText = GLOBAL_VARIABLES.CloseButtonText;

        $dialog_report = $(".strategia-custom-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: true,
            title: GLOBAL_VARIABLES.ReportViewer,
            width: _width,
            height: _height,
            open: function (ev, ui) {
                $($dialog_report).html('');
                $($dialog_report).append($("<iframe style='width:100%;height:100%;' />").attr("src", "/Report/Index"));
                $dialog_report.css('overflow', 'hidden');
                $('.ui-dialog .ui-dialog-content').css('padding', '0');
            },
            close: function () {
                $($dialog_report).html("");
                $(".jGrowl-close").trigger("click")
            },
            buttons: {
                //Text : GLOBAL_VARIABLES.CloseButtonText,
                closeText: function () {
                    $(this).dialog("close");
                }
            },

        });
        $dialog_report.dialog("open");
        $(".ui-dialog-buttonset  button").text(GLOBAL_VARIABLES.CloseButtonText);
    }
    else if (result.Action == "Execute") {
        eval(result.EvalScript);
    }
    else if (result.Action == "Warning") {
        Warning(result.Message);
    }
    else if (result.Action == "Information") {
        Information(result.Message);
    }
    else if (result.Action == "Success") {
        Success(result.Message);
    }
    else if (result.Action == "SuccessSticky") {
        SuccessSticky(result.Message);
    }
    else if (result.Action == "Failure") {
        Failure(result.Message);
    }
    else if (result.Action == "GrowlSticky") {
        Sticky(result.Message);
    }
    else if (result.Action == "GrowlBasic") {
        Growl(result.Message);
    }
    else if (result.Action == "GrowlLongLive") {
        LongLive(result.Message);
    }
}

function BulkTableAction(url, action, table) {
    if (action != "" && $('input:checked', table).length > 0) {
        var values = "";

        $.each($('input:checked', table), function (index, item) {
            values += $(item).val() + ",";
        });

        alert(url + " -> " + action + " -> " + values);
    }
    return false;
}

function HideMessages() {
    $(".nNote").hide();
}

$.fn.AutoHide = function (cssName) {
    setTimeout(function () {
        $(cssName).hide();
    }, 9000);
};

function Dialog(text) {
    $("#dialog-modal-text").html(text);
    $("#dialog-modal").dialog("open");
    return false;
}

function Warning(message) {
    HideMessages();
    $(".nWarning").html("<p>" + message + "</p>");
    $(".nWarning").show();
    ScrollTopAnimate();
}

function Information(message) {
    HideMessages();
    $(".nInformation").html("<p>" + message + "</p>");
    $(".nInformation").show();
    ScrollTopAnimate();
}

function Success(message) {
    HideMessages();
    $(".nSuccess").html("<p>" + message + "</p>");
    $(".nSuccess").show();
    $(".nSuccess").AutoHide(".nSuccess");
    ScrollTopAnimate();
}

function SuccessSticky(message) {
    $(".nSuccess").html("<p>" + message + "</p>");
    $(".nSuccess").show();
    //$(".nSuccess").AutoHide(".nSuccess");
    ScrollTopAnimate();

}

function Failure(message) {
    HideMessages();
    $(".nFailure").html("<p>" + message + "</p>");
    $(".nFailure").show();
    $(".nFailure").AutoHide(".nFailure");
    ScrollTopAnimate();
}

function Sticky(message) {
    $.jGrowl(message, { sticky: true });
}

function StickyForTranscript(message) {
    $.jGrowl(message, { sticky: true });
    $('#jGrowl').attr("style", "margin : 100px!important;  width: 250px!important;");
    $('#jGrowl').draggable();
}

function Growl(message) {
    $.jGrowl(message);
}

function LongLive(message) {
    $.jGrowl(message, { life: 10000 });
}

function OnBegin(xhr, content) {

    HideMessages();
    ShowLoader();
}

function OnComplete(xhr, status) {

}

function OnSuccess(result, status, xhr) {
    if (result.Action != undefined) {
        ExecuteResult(result);
        if (result.Action != "Redirect") {
            HideLoader();
        }
    }
    else if (result.InterfaceArgument != undefined && result.InterfaceArgument.Action != undefined) {
        ExecuteResult(result.InterfaceArgument);
        if (result.InterfaceArgument != "Redirect") {
            HideLoader();
        }
    }
    else {
        HideLoader();
    }
}

function OnFailure(xhr, status, error) {
    Failure(ERROR_MESSAGES.UndefinedError);
    HideLoader();
    console.log(xhr);
}

function ShowLoader() {
    $('.page_spinner').show();
}

function HideLoader() {
    $('.page_spinner').hide();
}

function ScrollTopAnimate() {
    $("html,body").stop().animate({ scrollTop: 0 }, 1000);
}

function CustomAutoComplete(actionUrl, controlId) {
    $("#" + controlId).autocomplete({
        source: actionUrl,
        autoFill: true,
        selectFirst: true,
        cacheLength: 0,
        focus: function (event, ui) {
            $(this).val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            //$("#" + hiddenId).val(ui.item.value);
            //$("#Spec1").val(ui.item.spec1);
            //$("#Spec2").val(ui.item.spec2);
            //$("#Spec3").val(ui.item.spec3);
            $(this).val(ui.item.label);
            CustomAutoCompleted(ui.item.value, ui.item.label, ui.item.spec1, ui.item.spec2, ui.item.spec3, ui.item.spec4);
            return false;
        }
    });
}


$(document).ready(function () {
    var default_image_loader = '<div class = "textC"><img src = "' + GLOBAL_VARIABLES.LoaderImagePath + '" /></div>';
    var default_message_for_dialog = 'Bu işlemi onaylıyor musunuz?';

    //#region SearchBox AutoComplete
    $(".searchbox-autocomplete").autocomplete({
        source: '/home/searchbox',
        autoFill: true,
        selectFirst: true,
        cacheLength: 0,
        select: function (event, ui) {
            ShowLoader();
            window.location.href = ui.item.value;
        }
    });
    //#endregion

    $('.table-trigger').live("change", function () {
        var $id = this.value;
        var $dropdown = $(this);
        ShowLoader();
        $.ajax({
            url: $dropdown.data("action-url"),
            data: { no: $id },
            type: "POST",
            success: function (result) {
                if ($("#" + $dropdown.data("action-table")).hasClass('pre-registration')) {
                    $("#" + $dropdown.data("action-table")).html(result);
                }
                else {
                    $("#" + $dropdown.data("action-table")).find("tbody").html(result);
                }
                HideLoader();
            },
            error: function (xhr, status, error) {
                console.log(xhr);
                Dialog(xhr + '-' + status + '-' + error);
                HideLoader();
            }
        });
        return false;
    });

    //#region Cascading DropDownList
    $.fn.cascading = function () {
        $(this).live("change", function () {

            var $dropdown = $(this);
            var selectedValue = $dropdown.val();
            var actionUrl = $dropdown.attr("data-action-url");
            var actionLabel = $dropdown.attr("data-action-label");
            //var label = actionLabel != null ? actionLabel : "Seçiniz";
            var otherDropDown = $("#" + $dropdown.attr("data-action-dropdown"));
            //var chzn = $(otherDropDown.selector + "_chzn");

            console.log(selectedValue);

            if (selectedValue != null && selectedValue != '') {
                ShowLoader();
                $.getJSON(actionUrl, { val: selectedValue }, function (items) {

                    otherDropDown.empty();
                    otherDropDown.removeAttr("disabled");
                    //otherDropDown.closest("div").removeClass("disabled");

                    if (actionLabel != null && actionLabel != undefined) {
                        otherDropDown.append($('<option/>', {
                            value: "",
                            text: actionLabel
                        }));
                    }

                    if (items != null) {
                        $.each(items, function (index, item) {
                            otherDropDown.append($('<option/>', {
                                value: item.No,
                                text: item.Name
                            }));
                        });
                    }

                    HideLoader();

                    //otherDropDown.trigger("liszt:updated");
                    //$.uniform.update(otherDropDown);
                });
            }
            else {
                otherDropDown.empty();
                otherDropDown.attr("disabled", "disabled");
                //otherDropDown.closest("div").attr("disabled", "disabled");
                //otherDropDown.trigger("liszt:updated");
            }
        });
    };

    $('.cascading').cascading();
    //#endregion

    $('#changeuser-term').keyup(function () {
        if (event.keyCode == 13) {
            $("#search-input").click();
        }
    });

    $("#search-input").live("click", function () {
        var table = $("#changeuser-list");
        var term = $("#changeuser-term").val();

        table.empty().append(default_image_loader);
        $.ajax({
            url: '/account/changeuser',
            data: { term: term },
            cache: false,
            type: 'GET',
            success: function (result) {
                table.empty().append(result);
                //$("input:radio").uniform();
            },
            error: function (xhr, status, error) {
                Dialog(xhr + '-' + status + '-' + error);
            }
        });
    });

    $('.userNav a.change-user').live("click", function () {
        $('.top-userchange-content').fadeToggle(150);
        if ($('.topSearch').is(':visible')) {
            $('.topSearch').fadeOut('fast');
        }
        return false;
    });

    //$(document).bind('click', function (e) {
    //    var $clicked = $(e.target);

    //    if (!$clicked.hasClass("search"))
    //        $('.topSearch').fadeOut('fast');
    //});

    $("#add-favorites").click(function () {
        $.ajax({
            url: "/Home/AddFavorites",
            data: { menuNo: $('#menu-pathno').val() },
            cache: false,
            success: function (model) {
                if (model.Action != undefined) {
                    if (model.Action == 'Success') {
                        $("ul#favorites-menu li:last").after('' + model + '');
                    }
                    else {
                        ExecuteResult(model);
                    }
                }
                else if (model.InterfaceArgument != undefined && model.InterfaceArgument.Action != undefined) {
                    if (model.InterfaceArgument.Action == 'Success') {
                        $("ul#favorites-menu li:last").after('' + model.Model + '');
                    }
                    else {
                        ExecuteResult(model.InterfaceArgument);
                    }
                }
            },
            type: "POST"
        });
        return false;
    });

    $("#favorites-menu").sortable({
        revert: true,
        cancel: "#favorites-menu li a",
        placeholder: "sortable-placeholder",
        items: "li:not(.disabled)",
        forcePlaceholderSize: true,
        start: function (event, ui) {
            $(this).attr('data-prev', ui.item.data("itemid"));
            ui.placeholder.addClass("placeholder");
        },
        update: function (event, ui) {
            var newPosition = ui.item.index();
            var prevPosition = $(this).attr('data-prev');
            var form = $(this).sortable("serialize");

            $.ajax({
                url: "/Home/SaveFavoriteOrder",
                data: { ids: form, prevPosition: prevPosition },
                cache: false,
                type: "POST"
            });
        }
    });

    if ($("#countdown").length == 1) {
        var counter = parseInt(SESSION_COUNTER.SessionTime) * 60 * 1000;
        var time = counter / 1000;
        var min = parseInt(SESSION_COUNTER.SessionTime);
        var sec = 0;
        var pmin = "00";
        var psec = "00";
        var pad = "00";

        var interval = setInterval(function () {
            counter = counter - 1000;

            time = counter / 1000;
            sec = time % 60;
            min = (time - sec) / 60;

            pmin = pad.substring(0, pad.length - min.toString().length) + min.toString();
            psec = pad.substring(0, pad.length - sec.toString().length) + sec.toString();

            if (min == 1 && sec == 0) {
                Dialog(SESSION_COUNTER.AutoLogOffMessage);
            }

            if (counter <= 0) {
                clearInterval(interval);
                window.location = "/account/logout"
            }

            $("#countdown").html(pmin + ":" + psec);

        }, 1000);
    }
});

function DataTableAddNewRow(table, rowcontent) {

    var tags = $("td", rowcontent);
    var val, text, jsonData;
    var array = new Array();

    tags.each(function () {

        var tag = $(this);
        if (tag.attr("class") == "tableActs") {
            text = "<div class=\"tableActs\">" + tag.html() + "</div>";
        }
        else {
            text = tag.text();
        }
        if (!text) text = "-";
        array.push(text);
    });

    $(table).dataTable().fnAddData([array]);
    //$("select, .check, .check :checkbox, input:radio, input:file").uniform();
    //$('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, live: true });
}

function AjaxDataTable(table, content) {
    var _table = $(table);
    $(_table).dataTable({
        "bDestroy": true,
        "bJQueryUI": false,
        "bServerSide": true,
        "bProcessing": true,
        "sPaginationType": "full_numbers",
        "sAjaxSource": content,
        //"sDom": '<"tablePars"fl>t<"tableFooter"ip>'
        "sDom": 'R<"H"lfr>t<"F"ip<',
        "aoColumns":
        [
         { "mData": "UserName" },
         { "mData": "Firstname" },
         { "mData": "AcademicPersonelTask" },
         { "mData": "AcademicUnit" }
        ]


    });
}

function DataTableSource(table, rowcontent) {

    var _table = $(table);
    var arrayList = new Array();
    var i = 0;

    rowcontent = $(rowcontent).filter("tr");

    rowcontent.each(function () {
        var array = new Array();
        $('td', this).each(function () {
            var tag = $(this);
            if (tag.attr("class") == "tableActs") {
                text = "<div class=\"tableActs\">" + tag.html() + "</div>";
            }
            else {
                text = tag.text();
            }
            if (!text) text = "-";
            array.push(text);
        });
        arrayList[i] = array;
        i++;
    });

    if (_table.hasClass('initialised')) {
        _table.aaData = arrayList;
    }
    else {
        _table.dataTable({
            "aaData": arrayList,
            "bDestroy": true,
            "bJQueryUI": false,
            "bAutoWidth": false,
            "sPaginationType": "full_numbers",
            "sDom": '<"tablePars"fl>t<"tableFooter"ip>',
            "oLanguage": {
                "sProcessing": "Isleniyor...",
                "sLengthMenu": "Sayfada _MENU_ Kayit Goster",
                "sZeroRecords": "Herhangi Bir Kayit Bulunamadi.",
                "sInfo": "  _TOTAL_ Kayittan _START_ - _END_ Arasi Kayitlar",
                "sInfoEmpty": "Kayit Yok",
                "sInfoFiltered": "( _MAX_ Kayit Icerisinden Bulunan)",
                "sInfoPostFix": "",
                "sSearch": "Bul:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "Ilk",
                    "sPrevious": "Onceki",
                    "sNext": "Sonraki",
                    "sLast": "Son"
                }
            },
            "fnDrawCallback": function (oSettings) {
                $(".str-choice").uniform();
                $('.tableActs .on_off :checkbox, .tableActs .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false });
            }
        });
        //$("select, .check, .check :checkbox, input:radio, input:file").uniform();
        $('.tableActs .on_off :checkbox, .tableActs .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false });
    }
}

function DataTableOnLoad(table, action) {
    $.ajax({
        url: action,
        data: {},
        cache: false,
        type: 'GET',
        success: function (result) {
            DataTableSource($(table), result);
        },
        error: function (xhr, status, error) {
            Dialog(xhr + '-' + status + '-' + error);
        }
    });
    //$("select, .check, .check :checkbox, input:radio, input:file").uniform();
    //$('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false });
}

function CheckDefaultContactInformations() {

    $('#email-list').find('tbody').removeClass('nFailure');
    $('#address-list').find('tbody').removeClass('nFailure');
    $('#phone-list').find('tbody').removeClass('nFailure');

    if ($('#email-list tbody tr td').find('i.icon-checkmark').length == 0) {
        $('#email-list').find('tbody').addClass('nFailure');
        Growl("Sistemde en az bir tane varsayılan e-posta adresiniz olması gerekmektedir.");
        $("html,body").stop().animate({ scrollTop: $('#email-list').offset().top - 50 }, 1000);
        return false;
    }
    if ($('#address-list tbody tr td').find('i.icon-checkmark').length == 0) {
        $('#address-list').find('tbody').addClass('nFailure');
        Growl("Sistemde en az bir tane varsayılan adres bilginiz olması gerekmektedir.");
        $("html,body").stop().animate({ scrollTop: $('#address-list').offset().top - 50 }, 1000);
        return false;
    }
    if ($('#phone-list tbody tr td').find('i.icon-checkmark').length == 0) {
        $('#phone-list').find('tbody').addClass('nFailure');
        Growl("Sistemde en az bir tane varsayılan telefon bilginiz olması gerekmektedir.");
        $("html,body").stop().animate({ scrollTop: $('#phone-list').offset().top - 50 }, 1000);
        return false;
    }

    //if (i < 3) {
    //    if ($('#email-list').length > 0) {
    //        $("html,body").stop().animate({ scrollTop: $('#email-list').offset().top - 50 }, 1000);
    //        return false;
    //    }
    //}
    return true;
}

function ShowTranscript(srnumber, isMaster) {
    GetTrancript(srnumber, "/studentreport/getstudenttranscript", isMaster);
}

function ViewOwnTranscript(srnumber) {
    GetTrancript(srnumber, "/studentreport/viewowntranscript");
}

function ViewStudentTranscript(isMasterOrPostGraduate, srnumber) {
    //var $dialog = $('#' + id).closest('.ui-dialog')
    //$dialog.dialog('close');
    GetTrancript(isMasterOrPostGraduate,srnumber, "/studentreport/viewstudenttranscript");
    return false;
}

function GetTrancript(srnumber, url, isMasterOrPostGraduate) {
    $.ajax({
        data: { studentRegistrationNo: srnumber, isMasterOrPostGraduate: isMasterOrPostGraduate },
        url: url,
        type: "POST",
        beforeSend: function (xhr) {
            ShowLoader();
        },
        success: function (result) {
            HideLoader();
            ExecuteResult(result);

            if (result.StudentPayment == "Borçlu") {
                StickyForTranscript("BİLGİ : Öğrencinin mali onayı yoktur!");

            }

        }
    });
}
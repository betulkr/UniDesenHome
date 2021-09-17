// JQueryui Defaults - Dialog Enter KeyPress
$.extend($.ui.dialog.prototype.options, {
    create: function () {
        var $this = $(this);

        // focus first button and bind enter to it
        $this.parent().find('.ui-dialog-buttonpane button:first').focus();
        $this.keypress(function (e) {
            if (e.keyCode == 13) {
               // $this.parent().find('.ui-dialog-buttonpane button:first').click();
                return false;
            }
        });
    }
});

$(document).ready(function () {

    $("table").find('td.sort').each(function () {
        $($(this)).html($(this).html() + "<span></span>");
        $($(this)).addClass("header");
    });
});



function Post(control, table, searchKey, take, currentPage, sortKey) {
    ShowLoader();
    $.ajax({
        url: control.closest("form").attr("action"),
        data: { SortExpression: sortKey, SearchKey: searchKey, PageCount: take, PageIndex: currentPage, fc: JSON.stringify(control.closest("form").serializeObject()) },
        cache: false,
        type: 'POST',
        success: function (result) {
            ReloadData(table, result);
            HideLoader();
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
            HideLoader();
        }
    });
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function ReloadData(table, result) {
    $(table).find("tbody").html(result.Model);
    $(table).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
    $(table).find(".check, .check :checkbox, input:file").uniform();
    $("#pager_" + $(table).attr("id")).html(result.Pager);
}

function Response(result, table) {
    table = "#" + table;
    $(table).find("tbody").html(result.Model);

    $(table).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
    $(table).find(".check, .check :checkbox, input:file").uniform();
    $("#pager_" + $(table).attr("id")).html(result.Pager != null ? result.Pager : "");
    ExecuteResult(result.Execute);
}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?|&])" + key + "=.*?(&|#|$)", "i");
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    } else {
        var hash = '';
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        if (uri.indexOf('#') !== -1) {
            hash = uri.replace(/.*#/, '#');
            uri = uri.replace(/#.*/, '');
        }
        return uri + separator + key + "=" + value + hash;
    }

}

$('a.strajax-create').live("click", function (link) {
    ShowLoader();
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height, $tableId, $isNested, $dialog_create, $data_isCallback, $data_firstHref;
    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");
    $isNested = $link.attr("data-isnested");
    $data_isCallback = $link.attr("data-iscallbak");
    $data_firstHref = $link.attr("data-firsthref");
    $data_transaction = $link.attr("data-transaction");
    $data_transaction_action = $link.attr("data-transaction-action");
    $data_transaction_control = $link.attr("data-transaction-control");


    if ($link.attr("data-tableid")) {
        $tableId = $link.attr("data-tableid");
    }
    else {
        $tableId = $link.closest("table").attr("id");
    }

    if ($isNested == "true") {
        $dialog_create = $(".strategia-custom-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else if ($isNested == "true2x") {
        $dialog_create = $(".strategia-custom-dialog-2x").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else {
        $dialog_create = $(".strategia-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }

    // set windows content
    $dialog_create.html($ajaxLoader);
    $dialog_create.dialog({
        title: $title,
        width: $width,
        height: $height,
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
            $('.formError').remove();
        }
    });

    if ($link.attr("data-buttons") == undefined) {
        $dialog_create.dialog({
            buttons: [
                {
                    text: GLOBAL_VARIABLES.SaveButtonText,
                    click: function () {
                        var form = $('form', this);
                        $.validator.unobtrusive.parse(form);
                        $(form).makeValidationInline();

                        if ($data_transaction) {
                            if (form.valid()) {
                                ShowLoader();
                                $.ajax({
                                    url: $data_transaction_action,
                                    type: 'POST',
                                    cache: false,
                                    data: form.serialize(),
                                    async: false,
                                    success: function (result) {

                                        var tempCheck = false;
                                        if ($('#' + $data_transaction_control) != undefined) {
                                            tempCheck = $('#' + $data_transaction_control).is(':checked');
                                        }


                                        if (result.ResultValue && tempCheck) {

                                            var $icon_tran = '<span class="iconb" data-icon=""></span> ';
                                            $dialog_transaction = $("<div></div>").dialog({
                                                modal: true,
                                                bgiframe: true,
                                                autoOpen: false,
                                                resizable: false,
                                                close: function (event, ui) {
                                                    $(this).empty().dialog('destroy');
                                                }
                                            });

                                            $dialog_transaction.html('<P>' + $icon_tran + ' ' + result.Message.Message + '</P>');
                                            $dialog_transaction.dialog('option', 'buttons', {
                                                "Evet": function () {

                                                    ShowLoader();
                                                    $.ajax({
                                                        url: $href,
                                                        type: 'POST',
                                                        cache: false,
                                                        data: form.serialize(),
                                                        success: function (result) {
                                                            if (result.Action != undefined) {
                                                                HideLoader();
                                                                ExecuteResult(result);
                                                                return false;
                                                            }
                                                            $tableId = "#" + $tableId;
                                                            $($tableId).find("tbody").html(result.Model);
                                                            $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                                                            if ($($tableId).find(".check, .check :checkbox") == null || $($tableId).find(".check, .check :checkbox") == undefined) {
                                                                $($tableId).find(".check, .check :checkbox").uniform();
                                                            }
                                                            $($tableId).find(" input:file").uniform();
                                                            $("#pager_" + $($tableId).attr("id")).html(result.Pager != null ? result.Pager : "");

                                                            if ($isNested == undefined) {
                                                                ExecuteResult(result.Execute);
                                                            }

                                                            if ($data_isCallback != undefined && $data_isCallback == "true") {
                                                                CustomDialogCallback();
                                                            }
                                                            HideLoader();
                                                            $dialog_create.dialog("close");
                                                        },
                                                        error: function (xhr, status, error) {
                                                            HideLoader();
                                                            Dialog(xhr.status + ' (' + xhr.statusText + ')');
                                                        }
                                                    });
                                                    HideLoader();


                                                    $(this).dialog("close");
                                                },
                                                "Hayır": function () {
                                                    $(this).dialog("close");
                                                    return false;
                                                }
                                            });

                                            $dialog_transaction.dialog({ width: 300, height: 210, title: 'Onay!' });
                                            $dialog_transaction.dialog("open");

                                        } else {
                                            if (form.valid()) {
                                                ShowLoader();
                                                $.ajax({
                                                    url: $href,
                                                    type: 'POST',
                                                    cache: false,
                                                    data: form.serialize(),
                                                    success: function (result) {
                                                        if (result.Action != undefined) {
                                                            HideLoader();
                                                            ExecuteResult(result);
                                                            return false;
                                                        }
                                                        $tableId = "#" + $tableId;
                                                        $($tableId).find("tbody").html(result.Model);
                                                        $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                                                        if ($($tableId).find(".check, .check :checkbox") == null || $($tableId).find(".check, .check :checkbox") == undefined) {
                                                            $($tableId).find(".check, .check :checkbox").uniform();
                                                        }
                                                        $($tableId).find(" input:file").uniform();
                                                        $("#pager_" + $($tableId).attr("id")).html(result.Pager != null ? result.Pager : "");

                                                        if ($isNested == undefined) {
                                                            ExecuteResult(result.Execute);
                                                        }

                                                        if ($data_isCallback != undefined && $data_isCallback == "true") {
                                                            CustomDialogCallback();
                                                        }
                                                        HideLoader();
                                                        $dialog_create.dialog("close");
                                                    },
                                                    error: function (xhr, status, error) {
                                                        HideLoader();
                                                        Dialog(xhr.status + ' (' + xhr.statusText + ')');
                                                    }
                                                });
                                            }
                                        }
                                        HideLoader();
                                    }
                                });
                            }
                        } else {
                            if (form.valid()) {
                                ShowLoader();
                                $.ajax({
                                    url: $href,
                                    type: 'POST',
                                    cache: false,
                                    data: form.serialize(),
                                    success: function (result) {
                                        if (result.Action != undefined) {
                                            HideLoader();
                                            ExecuteResult(result);
                                            return false;
                                        }
                                        $tableId = "#" + $tableId;
                                        $($tableId).find("tbody").html(result.Model);
                                        $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                                        if ($($tableId).find(".check, .check :checkbox") == null || $($tableId).find(".check, .check :checkbox") == undefined) {
                                            $($tableId).find(".check, .check :checkbox").uniform();
                                        }
                                        $($tableId).find("input:radio, input:file").uniform();
                                        $("#pager_" + $($tableId).attr("id")).html(result.Pager != null ? result.Pager : "");

                                        if ($isNested == undefined) {
                                            ExecuteResult(result.Execute);
                                        }

                                        if ($data_isCallback != undefined && $data_isCallback == "true") {
                                            CustomDialogCallback();
                                        }
                                        HideLoader();
                                        $dialog_create.dialog("close");
                                    },
                                    error: function (xhr, status, error) {
                                        HideLoader();
                                        Dialog(xhr.status + ' (' + xhr.statusText + ')');
                                    }
                                });
                            }
                        }
                    }
                },
                {
                    text: GLOBAL_VARIABLES.CancelButtonText,
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]
        });
    }

    $.ajax({
        url: $href,
        data: { rootNo: $itemId }, //rootNo varsa, controller tarafına gönderilebilir.
        cache: false,
        type: 'GET',
        success: function (result) {
            if (result.Action != undefined) {
                ExecuteResult(result);
                return false;
            }
            $dialog_create.html(result);
            if ($(".ui-dialog-content").find(".check, .check :checkbox") == null || $(".ui-dialog-content").find(".check, .check :checkbox") == undefined) {
                $(".ui-dialog-content").find(".check, .check :checkbox").uniform();
            }
            $(".ui-dialog-content").find(" input:file").uniform();

            $(".ui-dialog-content").find(".datepicker").datepicker({ defaultDate: +7, showOtherMonths: true, autoSize: true, appendText: '', dateFormat: 'dd-mm-yy' });

            $(".ui-dialog-content").find('.onlyNums input').autotab_magic().autotab_filter('numeric');
            $(".ui-dialog-content").find('.onlyText input').autotab_magic().autotab_filter('text');
            $(".ui-dialog-content").find('.onlyAlpha input').autotab_magic().autotab_filter('alpha');
            $(".ui-dialog-content").find('.onlyRegex input').autotab_magic().autotab_filter({ format: 'custom', pattern: '[^0-9\.]' });
            $(".ui-dialog-content").find('.allUpper input').autotab_magic().autotab_filter({ format: 'alphanumeric', uppercase: true });

            $(".ui-dialog-content").find('.tipN').tipsy({ gravity: 'n', fade: true, html: true, live: true });
            $(".ui-dialog-content").find('.tipS').tipsy({ gravity: 's', fade: true, html: true, live: true });
            $(".ui-dialog-content").find('.tipW').tipsy({ gravity: 'w', fade: true, html: true, live: true });
            $(".ui-dialog-content").find('.tipE').tipsy({ gravity: 'e', fade: true, html: true, live: true });

            HideLoader();
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
            HideLoader();
        }
    });

    $dialog_create.dialog("open");
    return false;
});

function LoadTextboxFormat() {
    $('.onlyNums input').autotab_magic().autotab_filter('numeric');
    $('.onlyText input').autotab_magic().autotab_filter('text');
    $('.onlyAlpha input').autotab_magic().autotab_filter('alpha');
    $('.onlyRegex input').autotab_magic().autotab_filter({ format: 'custom', pattern: '[^0-9\.]' });
    $('.allUpper input').autotab_magic().autotab_filter({ format: 'alphanumeric', uppercase: true });
}

function LoadPhoto(id) {
    if (document.getElementById("fileUpload_"+id).files.length > 0) {
        var formData = new FormData();
        var file = document.getElementById("fileUpload_"+id).files[0];
        formData.append("ProfileImageFile", file);
        formData.append("id", id);
        $.ajax({
            type: "POST",
            url: '/Account/UpdatePhoto',
            
            data: formData,
            dataType: 'json',
            contentType: false,
            async: false,
            processData: false,
            success: function (data) {
                if (data != undefined) {
                   
                    d = new Date();
                    console.log(data);
                    $("#img_" + data).attr("src", "/files/user/images/" + data + ".jpg?" + d.getTime());
                    //ExecuteResult(data);

                }
             
            }
        });
    } else {
        $("#fileUpload").css("border", "1px solid red");
    }
}
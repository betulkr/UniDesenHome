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

$("table td.sort").live("click", function () {
    var $header, $table, $searchKey, $sortKey, $class, $key, $value, $take;

    $header = $(this);
    $table = $header.closest("table");
    $searchKey = $("#" + $table.attr("data-search-input"));
    $take = $("#" + $table.attr("data-take-input"));
    $sortKey = $header.attr("data-sort");
    $class = $header.attr("class");

    $($table).find('td.sort').attr("class", "sort header");

    switch ($class) {
        case "sort header":
        case "sort header headerSortUp":
            $header.attr("class", "sort header headerSortDown");
            $value = $sortKey + " asc";
            break;
        case "sort header headerSortDown":
            $header.attr("class", "sort header headerSortUp");
            $value = $sortKey + " desc";
            break;
    }

    Post($table, $table, $searchKey.val(), $take.val(), 0, $value);
});

$("table th.sort").live("click", function () {
    var $header, $table, $searchKey, $sortKey, $class, $key, $value, $take;

    $header = $(this);
    $table = $header.closest("table");
    $searchKey = $("#" + $table.attr("data-search-input"));
    $take = $("#" + $table.attr("data-take-input"));
    $sortKey = $header.attr("data-sort");
    $class = $header.attr("class");

    $($table).find('th.sort').attr("class", "sort sorting");

    switch ($class) {
        case "sort sorting":
        case "sort sorting_asc":
            $header.attr("class", "sort header sorting_desc");
            $value = $sortKey + " asc";
            break;
        case "sort header sorting_desc":
            $header.attr("class", "sort header sorting_asc");
            $value = $sortKey + " desc";
            break;

    }

    Post($table, $table, $searchKey.val(), $take.val(), 0, $value);
});

$('.dataTables_length select').live("change", function () {
    var $dropDown, $value, $take, $searchKey, $table;

    $dropDown = $(this);
    $take = $dropDown.attr("id");
    $table = $("table[data-take-input=" + $take + "]");
    //alert($table);
    $searchKey = $("#" + $table.attr("data-search-input"));
    Post($dropDown, $table, $searchKey.val(), $("#" + $take).val(), 0, "");
});

$('.tableFooter a').live("click", function (link) {
    link.preventDefault();

    var $table, $searchKey, $take, $sortKey, $link;
    $link = $(this);
    if ($link.context.className.indexOf("disabled") > -1 || $link.context.className.indexOf("active") > -1) {
        return false;
    }
    else {
        $table = $("#" + $(this).closest("div.tableFooter").attr("data-tableid"));
        $searchKey = $("#" + $($table).attr("data-search-input")).val();
        $take = $("#" + $($table).attr("data-take-input")).val();

        //Sorting key
        if ($($table).find("td.headerSortUp").attr("data-sort") != undefined) {
            $sortKey = $($table).find("td.headerSortUp").attr("data-sort") + " desc";
        }
        else if ($($table).find("td.headerSortDown").attr("data-sort") != undefined) {
            $sortKey = $($table).find("td.headerSortDown").attr("data-sort") + " asc";
        }

        var speed = 0;
        if ($($table).height() > 1000) {
            speed = 2000;
        }
        else {
            speed = 1000;
        }

        $("html,body").stop().animate({ scrollTop: $link.closest("div.widget").offset().top }, speed);
        Post($link, $table, $searchKey, $take, $link.attr("data-item_page"), $sortKey);

    }
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
        $dialog_create = $(".helper-custom-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else if ($isNested == "true2x") {
        $dialog_create = $(".helper-custom-dialog-2x").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else {
        $dialog_create = $(".helper-dialog").dialog({
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

$('a.strajax-edit').live("click", function (link) {
    ShowLoader();
    link.preventDefault();

    var $dialog_edit;

    var $link, $href, $itemId, $title, $width, $height, $isNested, $tableId, $data_isCallback, $isConfirm, $confirmMessage;
    var params = {}, queries, temp, i, l, name, value;

    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");
    $isNested = $link.attr("data-isnested");
    $data_isCallback = $link.attr("data-iscallback");
    $isConfirm = $link.attr("data-isconfirm");
    $confirmMessage = $link.attr("data-confirmmessage");
    $saveButtonMetod = $link.attr("data-savebuttonmethod");
    $typeid = $link.attr("data-lessontypeid");

    if ($isNested == "true") {
        $dialog_edit = $(".helper-custom-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else if ($isNested == "true2x") {
        $dialog_edit = $(".helper-custom-dialog-2x").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else {
        $dialog_edit = $(".helper-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }

    // set windows content
    $dialog_edit.html($ajaxLoader);
    $dialog_edit.dialog({
        title: $title,
        width: $width,
        height: $height,
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
            $('.formError').remove();

        },
        buttons: [
            {
                text: GLOBAL_VARIABLES.SaveButtonText,
                click: function () {


                    if ($saveButtonMetod != undefined && $saveButtonMetod != null && $saveButtonMetod == "true") {
                        var resultBool = ValMed();
                        if (!resultBool)
                            return false;
                    }

                    if (typeof (BeforeSaveAction) == "function") {
                        BeforeSaveAction();
                        //$(this).dialog("close");
                    }

                    var form = $('form', this);

                    if ($(form).hasClass("no-validate") == false) {
                        $.validator.unobtrusive.parse(form);
                        $(form).makeValidationInline();
                    }

                    if (form.valid() || $(form).hasClass("no-validate")) {

                        if ($isConfirm && $isConfirm == "true") {
                            var $message, $icon, $confirmDialog;
                            $message = ($confirmMessage == undefined || $confirmMessage == '') ? "No message!" : $confirmMessage;
                            $icon = '<span class="iconb" data-icon=""></span> ';

                            $confirmDialog = $("<div></div>").dialog({
                                modal: true,
                                bgiframe: true,
                                autoOpen: false,
                                resizable: false
                            });

                            // set windows content
                            $confirmDialog.html('<P>' + $icon + $message + '</P>');
                            $confirmDialog.dialog('option', 'buttons', {
                                "Evet": function () {
                                    ShowLoader();
                                    $.ajax({
                                        url: $href,
                                        type: 'POST',
                                        cache: false,
                                        data: form.serialize(),
                                        success: function (result) {

                                            if (result.Action != undefined && (result.Action == "Failure" || result.Action == "GrowlBasic")) {
                                                HideLoader();
                                                ExecuteResult(result);
                                                $confirmDialog.dialog("close");//Close the confirm dialog...
                                                return false;
                                            }

                                            $tableId = $link.closest("table").attr("id");
                                            $tableId = "#" + $tableId;
                                            $($tableId).find("tbody").html(result.Model);
                                            $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                                            $($tableId).find(" input:file").uniform();
                                            if ($($tableId).find(".check, .check :checkbox") == null || $($tableId).find(".check, .check :checkbox") == undefined) {
                                                $($tableId).find(".check, .check :checkbox").uniform();
                                            }


                                            $("#pager_" + $($tableId).attr("id")).html(result.Pager != null ? result.Pager : "");

                                            if ($isNested == undefined) {
                                                ExecuteResult(result.Execute);
                                            }

                                            if ($data_isCallback != undefined && $data_isCallback == "true") {
                                                CustomDialogCallback();
                                            }

                                            HideLoader();

                                            $confirmDialog.dialog("close");
                                            $dialog_edit.dialog("close");
                                        }
                                    });
                                },
                                "Hayır": function () {
                                    $(this).dialog("close");
                                }
                            });
                            $confirmDialog.dialog({ width: 400, height: 180, title: 'Onay' });
                            $confirmDialog.dialog("open");
                        }
                        else {
                            ShowLoader();
                            $.ajax({
                                url: $href,
                                type: 'POST',
                                cache: false,
                                data: form.serialize(),
                                success: function (result) {

                                    if (result.Action != undefined && (result.Action == "Failure" || result.Action == "GrowlBasic")) {
                                        HideLoader();
                                        ExecuteResult(result);
                                        return false;
                                    }

                                    $tableId = $link.closest("table").attr("id");
                                    if ($tableId == "undefined" || $tableId == undefined || $tableId == null) {
                                        $tableId = $link.closest("form").find('table').attr("id");
                                    }
                                    $tableId = "#" + $tableId;
                                    $($tableId).find("tbody").html(result.Model);
                                    $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                                    $($tableId).find(" input:file").uniform();

                                    if ($($tableId).find(".check, .check :checkbox") == null || $($tableId).find(".check, .check :checkbox") == undefined) {
                                        $($tableId).find(".check, .check :checkbox").uniform();
                                    }

                                    $("#pager_" + $($tableId).attr("id")).html(result.Pager != null ? result.Pager : "");

                                    if ($isNested == undefined) {
                                        ExecuteResult(result.Execute);
                                    }

                                    if ($data_isCallback != undefined && $data_isCallback == "true") {
                                        CustomDialogCallback();
                                    }

                                    if (result.ExtraProcess == "undefined" || result.ExtraProcess == undefined || result.ExtraProcess == null) {
                                        if (result.ExtraProcess == "HideToolBar") {
                                            $('.tToolbar').hide();
                                        }
                                    }


                                    HideLoader();
                                    $dialog_edit.dialog("close");
                                }
                            });
                        }

                        //if (typeof (AfterDialogClose) == "function") {
                        //    AfterDialogClose();
                        //}
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


    if ($typeid != undefined && $typeid != null) {
        $.ajax({
            url: $href,
            data: { no: $itemId, guid: $typeid },
            cache: false,
            type: 'GET',
            success: function (result) {
                if (result.Action != undefined) {
                    ExecuteResult(result);
                    return false;
                }

                $dialog_edit.dialog("open");
                $dialog_edit.html(result);
                $(".ui-dialog-content").find(" input:file").uniform();
                if ($(".ui-dialog-content").find(".check, .check :checkbox") == null || $(".ui-dialog-content").find(".check, .check :checkbox") == undefined) {
                    $(".ui-dialog-content").find(".check, .check :checkbox").uniform();
                }


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
    } else {
        $.ajax({
            url: $href,
            data: { no: $itemId },
            cache: false,
            type: 'GET',
            success: function (result) {
                if (result.Action != undefined) {
                    ExecuteResult(result);
                    return false;
                }

                $dialog_edit.dialog("open");
                $dialog_edit.html(result);
                $(".ui-dialog-content").find(" input:file").uniform();
                if ($(".ui-dialog-content").find(".check, .check :checkbox") == null || $(".ui-dialog-content").find(".check, .check :checkbox") == undefined) {
                    $(".ui-dialog-content").find(".check, .check :checkbox").uniform();
                }


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
    }

    HideLoader();

    return false;
});

$('a.strajax-delete').live("click", function (link) {
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height, $icon, $message, $tableId, $isNested, $dialog_delete;
    var params = {}, queries, temp, i, l, name, value;

    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");
    $rel = $link.attr("rel");
    $isNested = $link.attr("data-isnested");
    $tableId = "#" + $link.closest("table").attr("id");

    $message = ($rel == undefined || $rel == '') ? "No message!" : $rel;
    $icon = '<span class="iconb" data-icon=""></span> ';

    $dialog_delete = $("<div></div>").dialog({
        modal: true,
        bgiframe: true,
        autoOpen: false,
        resizable: false,
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
            $('.formError').remove();
        }
    });

    $dialog_delete.html('<P>' + $icon + $message + '</P>');
    $dialog_delete.dialog('option', 'buttons', [
        {
            text: GLOBAL_VARIABLES.YesButtonText,
            click: function () {
                ShowLoader();
                $.ajax({
                    url: $href,
                    data: { no: $itemId },
                    cache: false,
                    type: "POST",
                    success: function (result) {
                        if (result.Action != undefined) {
                            if (result.Action != "Redirect") {
                                HideLoader();
                            }
                            ExecuteResult(result);
                            return false;
                        }

                        $($tableId).find("tbody").html(result.Model);
                        $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                        $($tableId).find(" input:file").uniform();
                        if ($($tableId).find(".check, .check :checkbox") == null || $($tableId).find(".check, .check :checkbox") == undefined) {
                            $($tableId).find(".check, .check :checkbox").uniform();
                        }
                        $("#pager_" + $($tableId).attr("id")).html(result.Pager != null ? result.Pager : "");

                        if ($isNested == undefined) {
                            ExecuteResult(result.Execute);
                        }

                        HideLoader();
                    },
                    error: function (xhr, status, error) {
                        HideLoader();
                        Dialog(xhr.status + ' (' + xhr.statusText + ')');
                    }
                });

                $(this).dialog("close");
            }
        },
        {
            text: GLOBAL_VARIABLES.NoButtonText,
            click: function () {
                $(this).dialog("close");
            }
        }
    ]);
    $dialog_delete.dialog({ width: 320, height: 170, title: $title });


    $dialog_delete.dialog("open");
    return false;
});

$('a.strajax-view').live("click", function (link) {
    ShowLoader();
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height, $isNested, $dialog_view;

    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");
    $isNested = $link.attr("data-isnested");

    if ($isNested == "true") {
        $dialog_view = $(".helper-custom-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else if ($isNested == "true2x") {
        $dialog_view = $(".helper-custom-dialog-2x").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else {
        $dialog_view = $(".helper-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }

    $dialog_view.dialog({
        buttons: [
            {
                text: GLOBAL_VARIABLES.CloseButtonText,
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });

    // set windows content
    $dialog_view.html($ajaxLoader);
    $dialog_view.dialog({
        title: $title,
        width: $width,
        height: $height,
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
            $('.formError').remove();
        }
    });

    $.ajax({
        url: $href,
        data: { no: $itemId, rootNo: $itemId },
        cache: false,
        type: 'GET',
        success: function (result) {
            if (result.Action != undefined) {
                HideLoader();
                ExecuteResult(result);
                return false;
            }
            $dialog_view.dialog("open");
            $dialog_view.html(result);

            if ($(".ui-dialog-content").find(".check, .check :checkbox") == null || $(".ui-dialog-content").find(".check, .check :checkbox") == undefined) {
                $(".ui-dialog-content").find(".check, .check :checkbox").uniform();
            }

            //$(".ui-dialog-content").find(".check, .check :checkbox, input:radio, input:file").uniform();
            $(".ui-dialog-content").find(".datepicker").datepicker({ defaultDate: +7, showOtherMonths: true, autoSize: true, appendText: '', dateFormat: 'dd-mm-yy' });

            LoadTextboxFormat();
            HideLoader();
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
            HideLoader();
        }
    });

    return false;
});

$('a.strajax-details').live("click", function (link) {
    ShowLoader();
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height, $tableId, $dialog_detail;
    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");
    if ($link.attr("data-tableid")) {
        $tableId = $link.attr("data-tableid");
    }
    else {
        $tableId = $link.closest("table").attr("id");
    }

    if ($isNested == "true") {
        $dialog_detail = $(".helper-custom-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }
    else {
        $dialog_detail = $(".helper-dialog").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false
        });
    }

    // set windows content
    $dialog_detail.html($ajaxLoader);
    $dialog_detail.dialog({
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
            $('.formError').remove();
        },
        title: $title,
        width: $width,
        height: $height,
        buttons: [
            {
                text: GLOBAL_VARIABLES.SaveButtonText,
                click: function () {
                    var form = $('form', this);
                    $.validator.unobtrusive.parse(form);
                    $(form).makeValidationInline();
                    if (form.valid()) {
                        $.ajax({
                            url: $href,
                            type: 'POST',
                            cache: false,
                            data: form.serialize(),
                            success: function (result) {
                                $dialog_detail.html(result);
                            },
                            error: function (xhr, status, error) {
                                Dialog(xhr.status + ' (' + xhr.statusText + ')');
                            }
                        });
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

    $.ajax({
        url: $href,
        data: { no: $itemId },
        cache: false,
        type: 'GET',
        success: function (result) {
            if (result.Action != undefined) {
                ExecuteResult(result);
                return false;
            }
            $dialog_detail.html(result);
            $(".ui-dialog-content").find(".check, .check :checkbox,  input:file").uniform();
            $(".ui-dialog-content").find(".datepicker").datepicker({ defaultDate: +7, showOtherMonths: true, autoSize: true, appendText: '', dateFormat: 'dd-mm-yy' });

            HideLoader();
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
            HideLoader();
        }
    });

    $dialog_detail.dialog("open");
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
    if (document.getElementById("fileUpload_" + id).files.length > 0) {
        var formData = new FormData();
        var file = document.getElementById("fileUpload_" + id).files[0];
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
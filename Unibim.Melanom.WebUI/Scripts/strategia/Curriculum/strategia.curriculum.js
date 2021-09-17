$('a.custom-create').live("click", function (link) {
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height, $tableId;
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

    // set windows content
    $dialog.html($ajaxLoader);
    $dialog.dialog({
        title: $title,
        width: $width,
        height: $height,
        buttons: {
            Kaydet: function () {
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
                            $tableId = "#" + $tableId;
                            $($tableId).find("tbody").html(result.Model);
                            $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                            $($tableId).find(".check, .check :checkbox, input:radio, input:file").uniform();
                            $("div[class^='widget']").contentTabs();
                            ExecuteResult(result.Execute);

                            $dialog.dialog("close");
                        },
                        error: function (xhr, status, error) {
                            Dialog(xhr.status + ' (' + xhr.statusText + ')');
                        }
                    });
                }
            },
            İptal: function () {
                $(this).dialog("close");
            }
        }
    });

    $.ajax({
        url: $href,
        data: { rootNo: $itemId }, //rootNo varsa, controller tarafına gönderilebilir.
        cache: false,
        type: 'GET',
        success: function (result) {
            $dialog.html(result);
            $(".ui-dialog-content").find(".check, .check :checkbox, input:radio, input:file").uniform();
            $(".ui-dialog-content").find(".datepicker").datepicker({ defaultDate: +7, showOtherMonths: true, autoSize: true, appendText: '', dateFormat: 'dd-mm-yy' });
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
        }
    });

    $dialog.dialog("open");
    return false;
});

$('a.custom-edit').live("click", function (link) {
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height;
    var params = {}, queries, temp, i, l, name, value;

    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");

    // set windows content
    $dialog.html($ajaxLoader);
    $dialog.dialog({
        title: $title,
        width: $width,
        height: $height,
        buttons: {
            Kaydet: function () {
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

                            if (result.Action != 'Failure') {
                                $tableId = "#custom-widget";
                                $($tableId).html(result.Model);
                                $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                                $($tableId).find(".check, .check :checkbox, input:radio, input:file").uniform();
                                $("div[class^='widget']").contentTabs();
                                ExecuteResult(result.Execute);
                            }
                            else {
                                ExecuteResult(result);
                            }
                            $dialog.dialog("close");
                        },
                        error: function (xhr, status, error) {
                            console.log(xhr);
                        }
                    });
                }
            },
            İptal: function () {
                $(this).dialog("close");
            }
        }
    });

    $.ajax({
        url: $href,
        data: { no: $itemId },
        cache: false,
        type: 'GET',
        success: function (result) {
            $dialog.html(result);
            $(".ui-dialog-content").find(".check, .check :checkbox, input:radio, input:file").uniform();
            $(".ui-dialog-content").find(".datepicker").datepicker({ defaultDate: +7, showOtherMonths: true, autoSize: true, appendText: '', dateFormat: 'dd-mm-yy' });
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
        }
    });

    $dialog.dialog("open");
    return false;
});

$('a.custom-delete').live("click", function (link) {
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height, $icon, $message, $tableId;
    var params = {}, queries, temp, i, l, name, value;

    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");
    $rel = $link.attr("rel");
    $tableId = "#custom-widget";


    $message = ($rel == undefined || $rel == '') ? "No message!" : $rel;
    $icon = '<span class="iconb" data-icon=""></span> ';

    $dialog.html('<P>' + $icon + $message + '</P>');
    $dialog.dialog('option', 'buttons', {
        "Evet": function () {

            $.ajax({
                url: $href,
                data: { no: $itemId },
                cache: false,
                type: "POST",
                success: function (result) {
                    if (result.Action != "Failure") {
                        $tableId = "#" + $link.closest("table").attr("id");
                        $($tableId).find("tbody").html(result.Model);
                        $($tableId).find('.on_off :checkbox, .on_off :radio').iButton({ labelOn: "", labelOff: "", enableDrag: false, allowRadioUncheck: true });
                        $($tableId).find(".check, .check :checkbox, input:radio, input:file").uniform();
                        $("div[class^='widget']").contentTabs();
                        ExecuteResult(result.Execute);
                    }
                    else {
                        ExecuteResult(result);
                    }
                }
            });

            $(this).dialog("close");
        },
        "Hayır": function () {
            $(this).dialog("close");
        }
    });
    $dialog.dialog({ width: 300, height: 150, title: $title });


    $dialog.dialog("open");
    return false;
});
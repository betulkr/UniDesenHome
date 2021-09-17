$("a.help-popup").live("click", function (link) {
    link.preventDefault();

    var $link, $href, $title, $no;
    $link = $(this);
    $href = $link.attr("href");
    $title = $link.attr("original-title");
    $no = $("#menu-pathno").val();
    if (!$title) { $title = $link.attr("title"); }
    $dialog.html($ajaxLoader);
    $dialog.dialog('option', 'buttons', {});
    $dialog.dialog({ title: $title, width: 600, height: 400 });

    $.ajax({
        url: $href,
        data: { no: $no },
        cache: false,
        type: 'GET',
        success: function (result) {
            if (result.Action && result.Action == 'Failure') {
                ExecuteResult(result);
            }
            else {
                $dialog.html(result);
            }
        },
        error: function (xhr, status, error) {
            $dialog.html(GLOBAL_VARIABLES.HelpNoContent);
        }
    });

    $dialog.dialog("open");
    return false;
});

$("a.strajax-popup").live("click", function (link) {
    link.preventDefault();

    var $link, $href, $title, $no, $width, $height;
    $link = $(this);
    $href = $link.attr("href");
    $title = $link.attr("original-title");
    $no = $link.attr("data-itemid");
    $width = $link.attr("data-width");
    if (!$width) $width = 600;
    $height = $link.attr("data-height");
    if (!$height) $height = 400;
    if (!$title) { $title = $link.attr("title"); }

    $.ajax({
        url: $href,
        data: {},
        cache: false,
        type: 'GET',
        success: function (result) {
            $dialog.html(result);
            $("input:file").uniform();
        },
        error: function (xhr, status, error) {
            Dialog(xhr + '-' + status + '-' + error);
        }
    });

    $dialog.dialog('option', 'buttons', {});
    $dialog.dialog({ title: $title, width: $width, height: $height });
    $dialog.dialog("open");
    return false;
});

$("a.confirm").live("click", function (link) {
    link.preventDefault();

    var $href, $rel, $message, $icon;
    $href = $(this).attr("href");
    $rel = $(this).attr("rel");
    $message = ($rel == undefined || $rel == '') ? "No message!" : $rel;
    $icon = '<span class="iconb" data-icon=""></span> ';

    // set windows content
    $dialog.html('<P>' + $icon + $message + '</P>');
    $dialog.dialog('option', 'buttons', {
        "Evet": function () {
            window.location.href = $href;
        },
        "Hayır": function () {
            $(this).dialog("close");
        }
    });
    $dialog.dialog({ width: 300, height: 150 });
    $dialog.dialog("open");
});

$("a.strajax-confirm").live("click", function (link) {
    link.preventDefault();

    var $link, $itemId, $href, $rel, $message, $icon;
    $link = $(this);
    $itemId = $link.attr("data-itemid");
    $href = $link.attr("href");
    $rel = $link.attr("rel");
    $message = ($rel == undefined || $rel == '') ? "No message!" : $rel;
    $icon = '<span class="iconb" data-icon=""></span> ';

    // set windows content
    $dialog.html('<P>' + $icon + $message + '</P>');
    $dialog.dialog('option', 'buttons', {
        "Evet": function () {

            $.ajax({
                url: $href,
                data: { no: $itemId },
                cache: false,
                type: "POST",
                success: function (result) {
                    if (result.Action == "Success") {
                        $link.closest("tr").attr("class", "nWarning");
                        setTimeout(function () {
                            var row = $link.closest("tr");
                            row.remove();
                        }, 500);
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
    $dialog.dialog({ width: 300, height: 150 });
    $dialog.dialog("open");
});

// FORMS
$('input.confirm').live("click", function (input) {
    input.preventDefault();

    var $form, $rel, $message, $icon;
    $form = $(input.target).closest("form");
    $rel = $(this).attr("rel");
    $message = ($rel == undefined || $rel == '') ? "No message!" : $rel;
    $icon = '<span class="iconb" data-icon=""></span> ';

    $dialog.html('<P>' + $icon + $message + '</P>');
    $dialog.dialog('option', 'buttons', {
        "Evet": function () {
            $form.submit();
        },
        "Hayır": function () {
            $(this).dialog("close");
        }
    });
    $dialog.dialog({ width: 300, height: 150 });
    $dialog.dialog("open");
});

$(".delete-favorite").live("click", function (link) {
    link.preventDefault();
    $link = $(this);
    $no = $link.attr("data-user-menupathno");
    $rel = $link.attr("data-rel");
    $message = ($rel == undefined || $rel == '') ? "No message!" : $rel;
    $title = $link.attr("data-title");
    $icon = '<span class="iconb" data-icon=""></span> ';
    $href = $link.attr("data-href");
    $dialog.html('<P>' + $icon + $message + '</P>');

    $dialog.dialog('option', 'buttons', {
        "Evet": function () {
            $.ajax({
                url: $href,
                data: { no: $no },
                cache: false,
                type: "POST",
                success: function (result) {
                    if (result.Action == "Success") {
                        $("ul#favorites-menu #li-" + $no + "").remove();
                        ExecuteResult(result)
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
    $dialog.dialog({
        width: 300,
        height: 150,
        title: $title
    });
    $dialog.dialog("open");
});

$('.stra-custom-dialog-link').live('click', function () {
    var href, height, width, triggerId, dialog, title, link, itemId;

    link = $(this);
    href = link.data('href');
    height = link.data('height');
    width = link.data('width');
    title = link.data('title');
    triggerId = link.data('triggerid');
    itemId = link.data('itemid');

    dialog = $(".strategia-custom-dialog").dialog({
        modal: true,
        bgiframe: true,
        autoOpen: false,
        resizable: false,
        title: title,
        width: width,
        height: height,
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
        }
    });
    dialog.html($ajaxLoader);
    dialog.dialog({
        buttons: {
            Kaydet: function () {

                var form = $('form', this);
                $.validator.unobtrusive.parse(form);
                $(form).makeValidationInline();
                if (form.valid()) {
                    ShowLoader();
                    $.ajax({
                        url: href,
                        type: 'POST',
                        cache: false,
                        data: form.serialize(),
                        success: function (result) {
                            if (result.Action != undefined && (result.Action == "Failure" || result.Action == "GrowlBasic")) {
                                HideLoader();
                                ExecuteResult(result);
                                return false;
                            }

                            triggerId = "#" + triggerId;
                            $(triggerId).html(result.Model);

                            console.warn(triggerId);
                            console.warn(result.Model);

                            HideLoader();
                            dialog.dialog("close");
                        },
                        error: function (xhr, status, error) {
                            HideLoader();
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
        url: href,
        data: { no: itemId },
        cache: false,
        type: 'GET',
        success: function (result) {
            
            dialog.html(result);
            $('.onlyNums input').autotab_magic().autotab_filter('numeric');
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
        }
    });

    dialog.dialog("open");
    return false;
});

$('.stra-custom-dialog-link-delete').live("click", function (link) {
    link.preventDefault();



    var $link, $href, $itemId, $title, $width, $height, $icon, $message, $triggerId, $isNested, $dialog_delete;
    var params = {}, queries, temp, i, l, name, value;

    $link = $(this);
    $href = $link.data("href");
    $itemId = $link.data("itemid");
    $title = $link.data("title");
    $width = $link.data("width");
    $height = $link.data("height");
    $rel = $link.data("rel");
    $triggerId = "#" + $link.data("triggerid");


    $message = ($rel == undefined || $rel == '') ? "No message!" : $rel;
    $icon = '<span class="iconb" data-icon=""></span> ';

    $dialog_delete = $(".strategia-custom-dialog").dialog({
        modal: true,
        bgiframe: true,
        autoOpen: false,
        resizable: false,
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
        }
    });

    $dialog_delete.html('<P>' + $icon + $message + '</P>');
    $dialog_delete.dialog('option', 'buttons', {
        "Evet": function () {
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

                    $($triggerId).html(result.Model);

                    HideLoader();
                },
                error: function (xhr, status, error) {
                    HideLoader();
                    Dialog(xhr.status + ' (' + xhr.statusText + ')');
                }
            });

            $(this).dialog("close");
        },
        "Hayır": function () {
            $(this).dialog("close");
        }
    });
    $dialog_delete.dialog({ width: $width, height: $height, title: $title });

    $dialog_delete.dialog("open");
    return false;
});

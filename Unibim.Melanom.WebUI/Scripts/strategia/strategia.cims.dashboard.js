$(function () {
    $(".dragbox-content").each(function (index, data) {
        var actionurl = $(this).data("actionurl");
        var id = $(this).attr("id");

        $.ajax({
            url: '' + actionurl + '',
            data: {},
            type: 'POST',
            success: function (result) {
                $('#' + id).html(result);
            },
            error: function (xhr, status, error) {
                $('#' + id).html("<div class='m10 ml10 mr10'>" + ERROR_MESSAGES.ErrorWidgetLoad + "</div>");
            }
        });
    });

    $('.titleOpt a').click(function () {
        var div = $(this).data('div');
        $('#' + div).toggle();

        updateWidgetData();
    });

    $('.column').sortable({
        connectWith: '.column',
        handle: '.whead',
        cursor: 'move',
        placeholder: 'placeholder',
        forcePlaceholderSize: true,
        opacity: 0.4,
        start: function (event, ui) {
            if ($.browser.mozilla || $.browser.safari)
                $(ui.item).find('.dragbox-content').toggle();
        },
        stop: function (event, ui) {
            ui.item.css({ 'top': '0', 'left': '0' }); //Opera fix
            if (!$.browser.mozilla && !$.browser.safari)
                updateWidgetData();
        }
    })
    .disableSelection();
});

function updateWidgetData() {
    var items = [];
    $('.column').each(function () {
        var columnId = $(this).attr('id');
        $('.dragbox', this).each(function (i) {
            var collapsed = 0;
            if ($(this).css('display') == "none") {
                collapsed = 1;
            }

            var item = {
                No: $(this).attr('id').split('dragbox-content-')[1],
                TitleCode: '',
                ActionUrl: '',
                UserNo: GLOBAL_VARIABLES.UserNo,
                ColumnNumber: columnId.split('column-')[1],
                OrderNumber: i,
                IsCollapsed: collapsed == 0 ? false : true,
                IsActive: $(this).closest('.widget').css('display') == "none" ? false : true
            };

            items.push(item);
        });
    });

    $.ajax({
        url: "/Home/Index",
        data: { widgets: JSON.stringify(items) },
        cache: false,
        dataType: 'json',
        type: "POST",
        success: function (result) {

        },
        error: function (xhr, status, error) {
            Failure(ERROR_MESSAGES.UndefinedError);
        }
    });
}

$('.screen-options-wrap input[type=checkbox]').live("change", function () {
    var id = $(this).attr("id");
    var div = $(this).data("div");
    $('#' + div).toggle();
    updateWidgetData();
});
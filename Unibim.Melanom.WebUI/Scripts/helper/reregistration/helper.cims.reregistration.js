// JQueryui Defaults - Dialog Enter KeyPress
$.extend($.ui.dialog.prototype.options, {
    create: function () {
        var $this = $(this);

        // focus first button and bind enter to it
        $this.parent().find('.ui-dialog-buttonpane button:first').focus();
        $this.keypress(function (e) {
            if (e.keyCode == 13) {
                $this.parent().find('input[name="search"]').click();
                return false;
            }
        });
    }
});

$(document).ready(function () {
    ReadyCalendar();
    $('.disabled').live('click', function () {
        return false;
    });

    /*If Student*/
    $.ajax({
        url: '/reregistration/shareddata',
        data: {},
        cache: false,
        type: 'POST',
        success: function (result) {
            if (result != null && result != "" && result != undefined) {
                LoadCalendar(result);
                CustomSettings();

                //if (result.Action == undefined) {
                //    if (result.IsConfirmed) {
                //        $('#btnConfirmation').removeClass('disabled');
                //        $("#btnTranscript").addClass('disabled');
                //    }
                //    else {
                //        $('#btnConfirmation').addClass('disabled');
                //        $("#btnTranscript").addClass('disabled');
                //    }
                //    if (result.Message != undefined) {
                //        Failure(result.Message);
                //    }

                //}

            }
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
        }
    });
    /*If Student*/
});

//function CustomOnSuccess(result, status, xhr) {
//    //OnSuccess(result, status, xhr);

//    //if (result.Action != undefined) {
//    //    ExecuteResult(result);
//    //}
//    //else {
//    //    if (result.ShowPaymentPage == true) {
//    //        alert('Show Payment');
//    //    }
//    //}
//    alert('booo');
//}

$('.tToolbar .view-type a').click(function () {
    $('.tToolbar .view-type a').show();
    $(this).hide();

    var tableview = $('#sch-daytime');
    tableview.show();
    tableview.animate({
        left: parseInt(tableview.css('left'), 10) == 0 ? (-tableview.outerWidth()) - 2 : 0,
        opacity: "show"
    });
});

$('.fc-event-delete').live('click', function () {

    var syllabusNo = $(this).data('syllabusno');
    var href = $(this).data('href');

    var $dialog_delete = $("<div></div>").dialog({
        modal: true,
        bgiframe: true,
        autoOpen: false,
        resizable: false,
        close: function (event, ui) {
            $(this).empty().dialog('destroy');
            $('.formError').remove();
        }
    });

    var $message = 'Silmek istediğiniz derse ait not veya yoklama girişleri varsa, bu kayıtlar da silinecektir. Onaylıyor musunuz?';
    var $icon = '<span class="iconb" data-icon=""></span> ';
    $dialog_delete.html('<P>' + $icon + $message + '</P>');
    $dialog_delete.dialog('option', 'buttons', {
        "Evet": function () {
            ShowLoader();
            $.ajax({
                url: href,
                data: { syllabusNo: syllabusNo },
                cache: false,
                type: 'POST',
                success: function (result) {
                    LoadCalendar(result);
                    HideLoader();
                },
                error: function (xhr, status, error) {
                    Dialog(xhr.status + ' (' + xhr.statusText + ')');
                    ShowLoader();
                }
            });
            $(this).dialog("close");
        },
        "Hayır": function () {
            $(this).dialog("close");
        }
    });
    $dialog_delete.dialog({ width: 370, height: 160, title: 'Onay' });
    $dialog_delete.dialog("open");

    return false;
});

function ReadyCalendar() {
    $('#sch-daytime').fullCalendar({
        height: 999999,
        eventRender: function (event, element) {
            !event.isReadonly ? element.find('.fc-event-head').html(element.find('.fc-event-head').html() + '<span title="Sil" data-href="/reregistration/deletesyllabus" data-syllabusno="' + event.id + '" class="fc-event-delete">X</a>') : "",
            element.qtip({
                content: event.description,
                show: { solo: true },
                style: {
                    width: 250,
                    padding: 5,
                    color: 'black',
                    textAlign: 'left',
                    border: {
                        width: 1,
                        radius: 3
                    },
                    tip: 'topLeft',

                    classes: {
                        tooltip: 'ui-widget',
                        tip: 'ui-widget',
                        title: 'ui-widget-header',
                        content: 'ui-widget-content'
                    }
                }
            });
        },
        header: {
            left: '',
            center: 'title',
            right: ''
        },
        editable: false,
        defaultView: 'agendaWeek',
        allDaySlot: false,
        axisFormat: 'HH:mm',
        firstDay: 1,
        minTime: 8,
        maxTime: 25,
        year: 2007,
        month: 0,
        date: 1,
        dayNamesShort: GLOBAL_VARIABLES.DayNamesShort,
        columnFormat: {
            month: 'ddd',
            week: 'ddd',
            day: 'dddd M/d'
        },
        loading: function (bool) {
            if (bool) ShowLoader();
            else HideLoader();
        },
        timeFormat: 'HH:mm{ - HH:mm}',
        events: null
    });
}

function LoadCalendar(result) {
    if (result.Action != undefined) {
        ExecuteResult(result);
        HideLoader();
        return false;
    }
    events = {
        url: '/reregistration/calendardata',
        type: 'GET',
        data: {}
    }

    $('#sch-daytime').fullCalendar('removeEventSource', events);
    $('#sch-daytime').fullCalendar('addEventSource', events);
    $('#sch-daytime').fullCalendar('refetchEvents');

    $('#sch-tableview').html(result.TableModel);
    $('#student-information').html(result.StudentInformation);
    $('#depositedAmount').html(result.DepositedAmount);
    console.log(result.DepositedAmount);
    $('#selectedCredit').html(result.SelectedCredit);
    $('#programSyllabusFee').html(result.ProgramSyllabusFee);
    $('#accordion').html(result.ListSyllabus);

    $('#btnTranscript').removeClass('disabled');
    $('#btnConfirmation').removeClass('disabled');
    $('#btnSave').removeClass('disabled');
    $('#btnReject').removeClass('disabled');
}

$('#list-previous-syllabus li a').live('click', function () {
    var uniqueId, syllabusNo, previousStudentLessonNo, courseNo;
    uniqueId = $(this).data('uniqueid');
    href = $(this).data('href');
    syllabusNo = $(this).data('syllabusno');
    previousStudentLessonNo = $(this).data('prevstudentlessonno');
    courseNo = $(this).data('courseno');
    CreateSyllabus(href, uniqueId, syllabusNo, previousStudentLessonNo, courseNo);
    $(this).closest('.ui-dialog-content').dialog('close');
    return false;
});

$('.menu_body li ul li a').live('click', function () {
    ShowLoader();

    var link = $(this);
    var events, dialog_view;
    var href = $(this).data('href');
    var uniqueId = $(this).data('uniqueid');
    var syllabusNo = $(this).data('syllabusno');
    var previousStudentLessonNo = $(this).data('prevstudentlessonno');
    var courseNo = $(this).data('courseno');
    var isElective = $(this).closest('ul').prev('a').data('iselective');

    if (isElective == "True") {
        dialog_view = $("<div></div>").dialog({
            modal: true,
            bgiframe: true,
            autoOpen: false,
            resizable: false,
            title: 'Bu gruptan daha önce alınan dersler...',
            width: 400,
            height: 300
        });
        dialog_view.html($ajaxLoader);
        $.ajax({
            url: '/reregistration/listprevioussyllabus',
            data: { uniqueId: uniqueId, syllabusNo: syllabusNo, previousStudentLessonNo: previousStudentLessonNo, courseNo: courseNo },
            cache: false,
            type: 'GET',
            success: function (result) {
                dialog_view.html(result.Model);
            },
            error: function (xhr, status, error) {
                Dialog(xhr.status + ' (' + xhr.statusText + ')');
            }
        });
        dialog_view.dialog("open");
        HideLoader();
        return false;
    }

    CreateSyllabus(href, uniqueId, syllabusNo, previousStudentLessonNo, courseNo);
});

function CreateSyllabus(href, uniqueId, syllabusNo, previousStudentLessonNo, courseNo) {
    $.ajax({
        url: href,
        data: { uniqueId: uniqueId, syllabusNo: syllabusNo, previousStudentLessonNo: previousStudentLessonNo, courseNo: courseNo },
        cache: false,
        type: 'POST',
        success: function (result) {
            LoadCalendar(result);
        },
        error: function (xhr, status, error) {
            HideLoader();
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
        }
    });

    return false;
}

$('a.callback-dialog').live("click", function (link) {
    link.preventDefault();

    var $link, $href, $itemId, $title, $width, $height, $tableId, $isNested, $callback_dialog, $btnselect, $btncancel;
    $link = $(this);
    $href = $link.attr("href");
    $itemId = $link.attr("data-itemid");
    $title = $link.attr("data-title");
    $width = $link.attr("data-width");
    $height = $link.attr("data-height");
    $isNested = $link.attr("data-isnested");
    $btnselect = $link.attr("data-btnselect");
    $btncancel = $link.attr("data-btn2");

    if ($link.attr("data-tableid")) {
        $tableId = $link.attr("data-tableid");
    }
    else {
        $tableId = $link.closest("table").attr("id");
    }

    $callback_dialog = $(".helper-custom-dialog").dialog({
        modal: true,
        bgiframe: true,
        autoOpen: false,
        resizable: false
    });

    // set windows content
    $callback_dialog.html('');
    $callback_dialog.html($ajaxLoader);
    $callback_dialog.dialog({
        title: $title,
        width: $width,
        height: $height
    });

    $callback_dialog.dialog({
        buttons: {
            Seç: function () {
                var form = $('form', this);
                $.validator.unobtrusive.parse(form);
                $(form).makeValidationInline();
                if (form.valid()) {
                    ShowLoader();
                    $.ajax({
                        url: $href,
                        type: 'POST',
                        cache: false,
                        data: form.serialize(),
                        success: function (result) {

                            if (result.Action != undefined) {
                                ExecuteResult(result);
                                HideLoader();
                                LoadCalendar(result);
                                $callback_dialog.dialog("close");
                                return false;
                            }
                            else {
                                CallbackDialog(result);
                                HideLoader();
                                $callback_dialog.dialog("close");
                            }
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
        url: $href,
        data: { rootNo: $itemId },
        cache: false,
        type: 'GET',
        success: function (result) {
            $callback_dialog.html(result);
            $(".ui-dialog-content").find(".check, .check :checkbox, input:radio, input:file").uniform();
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
        },
        error: function (xhr, status, error) {
            Dialog(xhr.status + ' (' + xhr.statusText + ')');
        }
    });

    $callback_dialog.dialog("open");
    return false;
});

function JsonFromDateFullYear(date) {
    var value = new Date(parseInt(date.replace(/(^.*\()|([+-].*$)/g, '')));
    return value.getFullYear();
}

function CallbackDialog(result) {
    LoadCalendar(result);
    CustomSettings();
}

function CustomSettings() {
    $(".lightbox").fancybox({ 'padding': 2 });
    $('.acc div.menu_body:eq(0)').show();
    $('.acc .whead:eq(0)').show().css({ color: "#2B6893" });
    $(".acc .whead").click(function () {
        $(this).css({ color: "#2B6893" }).next("div.menu_body").slideToggle(200).siblings("div.menu_body").slideUp("slow");
        $(this).siblings().css({ color: "#404040" });
    });
}
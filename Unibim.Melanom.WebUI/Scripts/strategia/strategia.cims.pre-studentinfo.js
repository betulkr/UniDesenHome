$(document).ready(function () {
    if ($('#HighEducationStatus').val() == "1") {
        $('#university-informations').fadeIn();
    }

    $(".survey input").click(function () {
        $(this).is_subquestions(this);
    });

    $('#HighEducationStatus').live('change', function (e) {
        if ($(this).val() == "0") {
            $('#university-informations').fadeOut();
            $("#UndergraduatePeriodNumber").rules("remove", "required");
            $("#UndergraduatePeriodNumber").rules("remove", "number");
            return;
        }
        else {
            $('#university-informations').fadeIn();
            $("#UndergraduatePeriodNumber").rules("add", { required: true, number: true, messages: { required: "Bu alan zorunludur.", number: "Sadece rakam girmelisiniz." } });
        }
    });

    $('#PreparationStatus').live('change', function () {
        var sn = $(this).data("sn");
        var isExemptionExam = $(this).val();

        $.ajax({
            url: '/preregistration/listexams',
            data: { isYes: isExemptionExam, studentNo: sn },
            type: 'POST',
            success: function (result) {
                $('#exam-list').find('tbody').html(result);
                $('#exam-list').find('input:checkbox').uniform();
            },
            error: function (xhr, status, error) {
                alert('Error!');
            }
        });
    });

    //$('#exam-list input[type=checkbox]').change(function () {
    //    var en = $(this).data("en"); //examno
    //    var sn = $(this).data("sn"); //studentno
    //    var val = this.checked; //value

    //    $.ajax({
    //        url: "/preregistration/savestudenttoexam",
    //        data: { examNo: en, studentNo: sn, value: val },
    //        cache: false,
    //        dataType: 'json',
    //        type: "POST"
    //    });
    //});
});

$.fn.is_checkbox = function () { return this.is(':checkbox'); };
$.fn.is_radio = function () { return this.is(':radio'); };
$.fn.is_text = function () { return this.is(':text'); };
$.fn.is_submit = function () { return this.is(':submit'); };

function clear_inputs(crootno) {
    $('li[data-rootno="' + crootno + '"] ul').find('input[type="checkbox"]').attr('checked', false);
    $('li[data-rootno="' + crootno + '"] ul').find('input[type="checkbox"]').parent().removeClass('checked');
    $('li[data-rootno="' + crootno + '"] ul').find('input[type="radio"]').attr('checked', false);
    $('li[data-rootno="' + crootno + '"] ul').find('input[type="radio"]').parent().removeClass('checked');
    $('li[data-rootno="' + crootno + '"] ul').find('input[type="text"]').val('');
};

$.fn.is_subquestions = function (control) {
    var typeid, rootno, crootno, no, visible, name;

    name = $(this).attr('name');
    typeid = $('#' + control.id + '-sub li[data-rootno="' + control.id + '"]').data("typeid");
    rootno = $('#' + control.id + '-sub li[data-rootno="' + control.id + '"]').data("rootno");
    no = $('#' + control.id + '-sub li[data-rootno="' + control.id + '"]').data("no");
    visible = $('#' + control.id + '-sub li[data-rootno="' + control.id + '"]').data("visible");
    crootno = $(this).closest("li").data("rootno");

    if ($(control).is_radio()) {
        $('li[data-rootno="' + crootno + '"] ul').fadeOut();
        clear_inputs(crootno);
    }

    //$('li[data-rootno="' + name + '"]').closest("ul").removeClass('nFailure');

    if (no) {
        if ($(control).is(':checked')) {

            if (typeid == '4c9db42a-ad60-41a3-8d08-5de103cf32cf') {
                $('li[data-rootno="' + control.id + '"]').each(function (index, li) {
                    $("#" + $(this).data('no') + "-sub").fadeIn();
                });
            }

            $("#" + control.id + "-sub").fadeIn();
            $("#" + no + "-sub").fadeIn();
            $('li[data-no="' + no + '"]').closest("ul").removeClass('nFailure');
        }
        else {
            $('li[data-rootno="' + crootno + '"] ul').fadeOut();
            clear_inputs(crootno);
        }
    }
};

function SurveyOnSuccess(result, status, xhr) {

    $('.survey').find('.nFailure').removeClass('nFailure');

    if (result.validation) {
        $("html,body").stop().animate({ scrollTop: $('#q_' + result.validation[0].QuestionId + '-sub').offset().top - 50 }, 1000);

        var messages = 'Anket - Cevaplanması Zorunlu Alanlar: <hr />';
        messages += "<ul>";
        $.each(result.validation, function () {
            $('#q_' + this.QuestionId + '-sub').addClass("nFailure");
            messages += "<li>" + this.Message + '</li>';
        });
        messages += "</ul>";
        Growl(messages);
    }

    OnSuccess(result, status, xhr);
}
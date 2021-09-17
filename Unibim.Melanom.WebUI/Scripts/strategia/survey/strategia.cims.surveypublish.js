$(function () {
    $('#RoleNo').change(function () {
        var val = $(this).val();

        $.ajax({
            type: "GET",
            url: '/survey/getroleId',
            data: { val: val },
            beforeSend: function (xhr) {
                ShowLoader();
            }
        }).done(function (result) {
            HideLoader();
            var id = result;
            if (id == 'd36b7e39-8d78-40e9-a155-855393e1c581') { //Is Student?
                $('#unit-row').show();
            }
            else {
                $('#unit-row').hide();
            }
        })
        .fail(function () {
            alert("error");
        });
    });
});
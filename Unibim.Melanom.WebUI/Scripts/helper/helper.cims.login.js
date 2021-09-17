$("#login-wrap").attr("class", "loginWrapper flipped");
$(".logback").hide();

$(document).ready(function () {
    $("#announce-wrapper").draggable({ handle: "div.header", containment: "body" });

    $.ajax({
        url: '/Account/LoginAnnouncement',
        data: {},
        type: 'GET',
        cache: false,
        success: function (result) {
            if (result != null) {
                $('#announce').fadeIn();
                $('.announce-paperclip').fadeIn();
                $('#announce #announce-subject').html('<b>' + result.Subject + '</b>');
                $('#announce #announce-content').html(result.Content);
            }
        },
        error: function (xhr, status, error) {
            Dialog('Kullanıcı girişi duyuru kayıtları yüklenirken bir hata oluştu!');
        }
    });
});
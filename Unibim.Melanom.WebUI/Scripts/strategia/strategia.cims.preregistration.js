$(function () {
    $(".pre-registration-link").click(function (e) {
        $("#" + $(this).data("showdiv")).fadeIn();
        $("#" + $(this).data("hidediv")).fadeOut();

        $(".print-link").data("print", $(this).data("showdiv"));
        $(".download-link").data("print", $(this).data("showdiv"));

        //$("#" + $(this).data("hidediv")).hide("slide", { direction: "right" }, 0);
        //$("#" + $(this).data("showdiv")).delay(400).show("slide", { direction: "left" }, 0);

        $("#" + $(this).data("showlink")).fadeToggle();
        $(this).hide();
        e.preventDefault();
    });

    $(".print-link").click(function (e) {
        var div = "#" + $(this).data("print");
        $(div).printThis({
            debug: false,
            importCSS: true,
            printContainer: true,
            //loadCSS: "/content/themes/aquincum/aquincum.styles.css",
            pageTitle: $(document).find("title").text(),
            removeInline: false
        });
        e.preventDefault();
    });

    $(".download-link").click(function (e) {
        alert("Bir hata oluştu, lütfen daha sonra tekrar deneyiniz!");
        e.preventDefault();
    });
});

$.fn.showMemo = function (memoId, chartSection) {
    $(this).bind("plothover", function (event, pos, item) {
        if (!item) { return; }
        console.log(item.series.data)
        var html = [];
        var percent = parseFloat(item.series.percent).toFixed(2);

        html.push("<div style=\"border:none;background-color:",
             item.series.color,
             "\">",
             "<span style=\"color:white\">",
             item.series.label,
             " : ",
             item.series.data[0][1],
             " (", percent, "%)",
             "</span>",
             "</div>");
        $(chartSection + " " + memoId).show();
        $(chartSection + " " + memoId).html(html.join(''));
    });
};

function SetGraph(dataSet, chartSection) {

    var options = {
        series: {
            pie: {
                show: true,
                label: {
                    show: true,
                    radius: 180,
                    formatter: function (label, series) {
                        return '<div style="border:1px solid grey;font-size:8pt;text-align:center;padding:5px;color:white;">' +
                        label + ' : ' +
                        Math.round(series.percent) +
                        '%</div>';
                    },
                    background: {
                        opacity: 0.8,
                        //color: '#000'
                    }
                }
            }
        },
        legend: {
            show: true
        },
        grid: {
            hoverable: true
        }
    };

    $.plot($(chartSection + " .flotcontainer"), dataSet, options);
    $(chartSection + " .flotcontainer").showMemo(".flot-memo", chartSection);
}
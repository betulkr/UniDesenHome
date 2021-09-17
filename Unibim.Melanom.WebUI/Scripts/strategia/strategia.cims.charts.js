$.fn.quickStatsToggle = function (control) {
    $(control).toggle();
};

$.fn.showMemo = function (memoId) {
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
        $(memoId).show();
        $(memoId).html(html.join(''));
    });
};

$(document).ready(function () {
    // ***** IPTAL *****
    //$(".quickStats li a").live("click", function () {

    //    $("#system-graph").show();
    //    $(".flot-memo").html('');

    //    var cssName = $(this).attr("class");
    //    var userType = 1;
    //    switch (cssName) {
    //        case "blueImg":
    //            $("#system-graph .topDropArrow").css("left", "20%");
    //            userType = 1;
    //            break;
    //        case "redImg":
    //            $("#system-graph .topDropArrow").css("left", "50%");
    //            userType = 2;
    //            break;
    //        case "greenImg":
    //            $("#system-graph .topDropArrow").css("left", "80%");
    //            userType = 3;
    //            break;
    //    }
    //    getQuickStatsData(userType);
    //});

    //$("#system-graph-close").click(function () {
    //    $("#system-graph").hide();
    //});
});

function getQuickStatsData(userType) {
    $("#system-graph .flotcontainer").html("<div class='textC'><img src=" + GLOBAL_VARIABLES.LoaderImagePath + " /></div>");
    $.ajax({
        url: '/Statistic/UserStates',
        data: { userType: userType },
        cache: false,
        type: 'GET',
        success: function (result) {

            var data = result;

            var options = {
                series: {
                    pie: {
                        show: true,
                        label: {
                            show: true,
                            formatter: function (label, series) {
                                return '<div style="border:1px solid grey;font-size:8pt;text-align:center;padding:2px;color:white;">'/* + label + '<br/>' */ + Math.round(series.percent) + '%</div>';
                            },
                            background: { opacity: 0.8 }
                        }
                    }
                },
                legend: {
                    show: true,
                    noColumns: 1, // number of colums in legend table
                    labelFormatter: null, // fn: string -> string
                    labelBoxBorderColor: "#000", // border color for the little label boxes
                    container: null, // container (as jQuery object) to put legend in, null means default on top of graph
                    position: "ne", // position of default legend container within plot
                    margin: [5, 10], // distance from grid edge to default legend container within plot
                    backgroundColor: "#000", // null means auto-detect
                    backgroundOpacity: 0.2 // set to 0 to avoid background
                },
                grid: {
                    hoverable: true,
                    clickable: true
                }
            };

            $.plot($("#system-graph .flotcontainer"), data, options);
            $("#system-graph .flotcontainer").showMemo(".flot-memo");
        },
        error: function (xhr, status, error) {
            //Dialog(xhr + '-' + status + '-' + error);
            $("#system-graph .flotcontainer").html('<div style="color:#fff;" class="textC">' + ERROR_MESSAGES.UndefinedError + '</div>');
        }
    });
}
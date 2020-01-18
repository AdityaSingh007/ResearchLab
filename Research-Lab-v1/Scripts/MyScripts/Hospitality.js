var HospitalityService = function () {
    var hotelInfo,
        details,
        init = function () {
            $('#btnSouthern').click(function () {
                display(0);
            });
            $('#btnSandy').click(function () {
                display(2);
            });
            $('#btnEnchanted').click(function () {
                display(1);
            });
        },
        loadData = function () {
            $.getJSON("../data.json", function (result) {
                hotelInfo = result;
                console.log(hotelInfo);
                display(0);
            });
        },
        display = function (x) {
            $('#roomName').text(hotelInfo[x].name)
            $('#desc').text(hotelInfo[x].description)
            $('#photo').attr("src", "../Content/" + hotelInfo[x].photo)
            $('#weekday').text(hotelInfo[x].cost.weekday);
            $('#weekend').text(hotelInfo[x].cost.weekend);
            details = "";
            $.each(hotelInfo[x].details, function (index, ele) {

                details += "<P>" + hotelInfo[x].details[index] + "</P>";
            });
            $('#details').html(details);
        }
    return {
        Init: init,
        LoadData: loadData
    }
}();
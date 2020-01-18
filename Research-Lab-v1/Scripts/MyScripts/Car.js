var CarService = function () {
    var Fleet = [],
        $trs = [],
        PopulateFleetData = function () {
            return $.getJSON('../Fleet.json');
        },
        loadCarsPage = function () {
            PopulateFleetData().done(function (fleetData) {
                Fleet = fleetData;
                console.log(Fleet);
                $.each(fleetData, function (index, element) {
                    if (element.type === "car") {
                        var $tr = $("<tr>" + generateTd(element.license) + generateTd(element.make) + generateTd(element.model) + generateTd(element.miles) + "</tr>");
                        $trs.push($tr);
                    }

                })
                var $tbody = $("#carsTable tbody");
                $tbody.append($trs);
            })
        },
        generateTd = function td(data) {
            return "<td>" + data + "</td>";
        }
    return {
        LoadCarsPage: loadCarsPage
    }
}();
var DroneService = function () {
    var fleet = [],
        $trs = [],
        populateDroneFleet = function () {
            return $.getJSON('../Fleet.json');
        },
        loadDronesPage = function () {
            populateDroneFleet().done(function (fleetData) {
                Fleet = fleetData;
                console.log(Fleet);
                $.each(fleetData, function (index, element) {
                    if (element.type === "drone") {
                        var $tr = $("<tr>" + generateTd(element.license) + generateTd(element.base) + generateTd(element.model) + generateTd(element.airTimeHours) + "</tr>");
                        $trs.push($tr);
                    }

                })
                var $tbody = $("#DronesTable tbody");
                $tbody.append($trs);
            })
        },
        generateTd = function td(data) {
            return "<td>" + data + "</td>";
        }
    return {
        LoadDronesPage: loadDronesPage
    }
}();
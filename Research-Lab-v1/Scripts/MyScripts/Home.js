var FleetManagerService = function () {
    var init = function () {
        $('#btnCars').click(function () {
            $('#content').empty();
            $('#content').load('AppPages/Cars.html');
        })
        $('#btnDrones').click(function () {
            $('#content').empty();
            $('#content').load('AppPages/Drones.html');
        })
        $(window).on("load resize", function () {
            $(':image').css('height', window.innerHeight)
        })
    },
        activateRoutes = function (navTitle) {
            switch (navTitle) {
                case "Home":
                    handleRoutes('AppPages/Home.html')
                    break;
                case "Cars":
                    handleRoutes('AppPages/Cars.html')
                    break;
                case "Drones":
                    handleRoutes('AppPages/Drones.html')
                    break;
                case "Maps":
                    handleRoutes('AppPages/Maps.html')
                    break;
                default:
                    handleRoutes('AppPages/Home.html')

            }
        },
        handleRoutes = function (screenPath) {
            $('#content').empty();
            $('#content').load(screenPath);
        }
    return {
        Init: init,
        ActivateRoutes: activateRoutes
    }
}();
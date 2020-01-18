var loadDynamicContents = function () {
    var navs = [],
        callGenerateNavLinksApi = function () {
            var ApiUrlBase = 'http://localhost:60723/api/values/'
            return $.get(ApiUrlBase + 'getNavObjects' + '?isHtmlApp=' + false);
        },
        generateNavLinks = function () {
            callGenerateNavLinksApi().done(function (navLinksArray) {
                console.log(navLinksArray)
                $.each(navLinksArray, function (index, element) {
                    if (index === 0) {
                        var navlink = $('<li class="nav - item active"><a class="nav-link" href = "#">' + element.title + '<span class="sr-only">(current)</span></a></li>');
                        navlink.find('a').attr('href', element.src);
                        navs.push(navlink);
                    }
                    else {
                        var navlink = $('<li class="nav - item"><a class="nav-link" href = "#">' + element.title + '</a></li>');
                        navlink.find('a').attr('href', element.src);
                        navs.push(navlink);
                    }
                })
                $('#navbarNav').find('ul').append(navs);
            });
        },
        loadDefaultRoute = function () {
            $('#content').load('AppPages/Home.html');
        },
        init = function () {
            $('#btnCars').click(function () {
                window.location.href = "/Home/Cars";

            })
            $('#btnDrones').click(function () {
                window.location.href = "/Home/Drones";
            })
        }
    return {
        GenerateNavLinks: generateNavLinks,
        Init: init
    }
}();
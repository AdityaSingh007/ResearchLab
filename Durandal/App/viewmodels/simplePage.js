define(['plugins/router'], function (router) {
    var vm = {};

    vm.message = "!Hello, World";

    vm.navigate = function () {
        var param = "Passed param successfully";
        router.navigate('#simplePage1/' + encodeURIComponent(param));
    };

    return vm;
});
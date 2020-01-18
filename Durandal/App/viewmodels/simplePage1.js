define(['plugins/router'], function (router) {
    var vm = {};

    vm.message = "Hello SimplePage1";
    vm.param = "";

    vm.router = router.createChildRouter()
        .makeRelative({
            moduleId: 'viewmodels',
            route: 'simplePage1/:param1'
        }).map([
            { route: ['id(/:param2)', ''], moduleId: 'simplePageChild', title: 'simplePageChild', nav: true }
        ]).buildNavigationModel();

    vm.activate = function (passedParam) {
        vm.param = passedParam;
    }

    vm.attached = function () {
        //alert(vm.param);
    }
    return vm;
})
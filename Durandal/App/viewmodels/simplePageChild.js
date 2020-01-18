define([], function () {
    var vm = {};

    vm.message = "!Hello Child";
    vm.activate = function (name, id) {
        console.log(name + "" + id);
    };

    return vm;
})
var app = app || {};

app.vm = (function () {
    var people = app.data.getPeople(10);
    var selectedPerson = ko.observable();

    var vm = {
        people: people,
        selectedPerson: selectedPerson,
        selectPerson: selectPerson,
        removePerson: removePerson
    };

    return vm;

    function selectPerson(person) {
        selectedPerson(person);
        toastr.info('Selected ' + person.firstName() + ' ' + person.lastName());
    }

    function removePerson(person, event) {
        var msg = 'Removed ' + person.firstName() + ' ' + person.lastName();
        people.remove(person);
        if (person === selectedPerson()) {
            selectedPerson(undefined);
        }
        toastr.warning(msg);
        console.log(event);
    }

})();

$(function () {
    ko.applyBindings(app.vm);

    //**Below Jquery code is for without using ryan niemeyers knockout-delegated event plugin**//
    
    //select 
    //$('#people').on('click', 'tr button.btn-info', function () {
    //    var context = ko.contextFor(this);
    //    if (context) {
    //        context.$root.selectPerson(context.$data);
    //    }

    //})

    //delete
    //$('#people').on('click', 'tr button.btn-danger', function () {
    //    var context = ko.contextFor(this);
    //    if (context) {
    //        context.$root.removePerson(context.$data);
    //    }

    //})
});
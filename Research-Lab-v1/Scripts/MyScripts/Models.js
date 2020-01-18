var app = app || {};

app.Person = function (id, first, last, twitter) {
    var self = this;
    self.id = ko.observable(id);
    self.firstName = ko.observable(first);
    self.lastName = ko.observable(last);
    self.twitter = ko.observable(twitter);
};
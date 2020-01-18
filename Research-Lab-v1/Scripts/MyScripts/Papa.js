var Product = function () {
    this.id = ko.observable();
    this.price = ko.observable();
    this.model = ko.observable();
    this.decsription = ko.observable();
};

var mockdata = (function () {
    var getProducts = function () {
        var products = [];

        products.push(new Product()
            .id(1)
            .price(599.99)
            .model('Taylor 314Ce')
            .decsription('cool guitar'));

        products.push(new Product()
            .id(1)
            .price(599.99)
            .model('Taylor 314Ce')
            .decsription('cool guitar'));

        products.push(new Product()
            .id(1)
            .price(599.99)
            .model('Taylor 314Ce')
            .decsription('cool guitar'));
        return products;
    };

    var mockdata = {
        getProducts: getProducts
    };
    return mockdata;
})();

var vm = (function () {
    var data = mockdata.getProducts();
    var products = ko.observableArray(data);
    var displayGuitars = ko.observable(false);
    var movie = ko.observable();
    var data = ['Alabama', 'Louisana', 'Utah', 'California', 'Santa Monica'];
    var movies = ko.observableArray(data);

    var vm = {
        displayGuitars: displayGuitars,
        products: products,
        movie: movie,
        movies: movies
    };

    return vm;
})();

ko.applyBindings(vm);
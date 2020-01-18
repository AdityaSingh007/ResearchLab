﻿var my = my || {}; //my namespace
my.dataservice = (function (my) {
    "use strict";
    var getProducts = function () {
        return my.sampleData;
    };
    return {
        getProduct: getProducts
    };
})(my);

my.sampleData = (function (my) {
    "use strict";
    var data = {
        Products: [
            { "ModelId": 1, "SalePrice": 1649.01, "ListPrice": 2199.00, "Rating": 5, "Photo": "Taylor 314-CE Left-Handed Grand Auditorium Acoustic-Electric Guitar.png", "CategoryId": 1, "ItemNumber": "T314CE", "Description": "Taylor 314-CE Left-Handed Grand Auditorium Acoustic-Electric Guitar", "Reviews": [{ "ProductId": 4, "ReviewerName": "guitarhero", "Description": "This guitar has never let me down. I gig 3-6 nights a week and the 314CE just keeps on ticking. Change the battery and the strings regularly and she sounds like a dream everytime. If you want great sound, playability, dependability and just great playing fun get this guitar. You won\u0027t be sorry.", "ReviewDate": "\/Date(1302181440000)\/", "Id": 3 }, { "ProductId": 4, "ReviewerName": "billyjack", "Description": "Best guitar ever!", "ReviewDate": "\/Date(1302218040000)\/", "Id": 4 }], "Model": { "Name": "314ce", "Brand": "Taylor", "Id": 1 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 4 },
            {
                "ModelId": 3, "SalePrice": 4775.00, "ListPrice": 7199.00, "Rating": 5, "Photo": "T814ce_tobacco+sunburst.jpg", "CategoryId": 1, "ItemNumber": "T814CE", "Description": "Taylor 814-CE Acoustic Electric Guitar in Tobacco Sunburst",
                "Reviews": [],
                "Model": { "Name": "814ce", "Brand": "Taylor", "Id": 3 },
                "Category": { "Name": "Acoustic Guitars", "Id": 1 },
                "Id": 3
            },
            { "ModelId": 4, "SalePrice": 5899.00, "ListPrice": 7999.00, "Rating": 5, "Photo": "T914CE.jpg", "CategoryId": 1, "ItemNumber": "T914CE", "Description": "Taylor 814-CE Acoustic Electric Guitar", "Reviews": [], "Model": { "Name": "914ce", "Brand": "Taylor", "Id": 4 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 3 },
            { "ModelId": 5, "SalePrice": 5898.00, "ListPrice": 7998.00, "Rating": 5, "Photo": "T916ce_tobacco+sunburst.jpg", "CategoryId": 1, "ItemNumber": "T916CE", "Description": "Taylor 916-CE Acoustic Electric Guitar in Tobacco Sunburst", "Reviews": [], "Model": { "Name": "916ce", "Brand": "Taylor", "Id": 5 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 4 },
            { "ModelId": 6, "SalePrice": 8999.00, "ListPrice": 10599.00, "Rating": 5, "Photo": "TDMSM_front_print.jpg", "CategoryId": 1, "ItemNumber": "T814CE", "Description": "Taylor DMSM Acoustic Electric Guitar", "Reviews": [], "Model": { "Name": "DMSM", "Brand": "Taylor", "Id": 6 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 5 },
            { "ModelId": 8, "SalePrice": 4199.00, "ListPrice": 5199.00, "Rating": 5, "Photo": "Taylor Koa Series K66ce Grand Symphony 12-String Cutaway Acoustic Electric Guitar.png", "CategoryId": 1, "ItemNumber": "TK66CE", "Description": "Taylor Koa Series K66ce Grand Symphony 12-String Cutaway Acoustic Electric Guitar", "Reviews": [], "Model": { "Name": "K66e", "Brand": "Taylor", "Id": 8 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 11 },
            { "ModelId": 9, "SalePrice": 299.00, "ListPrice": 399.00, "Rating": 3, "Photo": "Taylor Baby Taylor Left-Handed Acoustic Guitar.png", "CategoryId": 1, "ItemNumber": "TBTL", "Description": "Taylor Baby Taylor Left-Handed Acoustic Guitar", "Reviews": [], "Model": { "Name": "Baby", "Brand": "Taylor", "Id": 9 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 12 },
            { "ModelId": 10, "SalePrice": 1999.00, "ListPrice": 2399.00, "Rating": 4, "Photo": "Taylor T5 Standard Acoustic-Electric Guitar with Spruce Top.png", "CategoryId": 1, "ItemNumber": "TT5E", "Description": "Taylor T5 Standard Acoustic-Electric Guitar with Spruce Top", "Reviews": [], "Model": { "Name": "T5", "Brand": "Taylor", "Id": 10 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 13 },
            { "ModelId": 11, "SalePrice": 149.99, "ListPrice": 169.99, "Rating": 4, "Photo": "El Dorado Vintage Hand-Tooled Leather Guitar Strap.png", "CategoryId": 4, "ItemNumber": "87123", "Description": "El Dorado Vintage Hand-Tooled Leather Guitar Strap", "Reviews": [], "Model": { "Name": "Vintage", "Brand": "El Dorado", "Id": 11 }, "Category": { "Name": "Straps", "Id": 4 }, "Id": 14 },
            { "ModelId": 12, "SalePrice": 16.99, "ListPrice": 19.99, "Rating": 3, "Photo": "Moody 2 half Inch Luxury Black Leather Guitar Strap with Tobacco Leather Back.png", "CategoryId": 4, "ItemNumber": "89120", "Description": "Moody 2 half Inch Luxury Black Leather Guitar Strap with Tobacco Leather Back", "Reviews": [], "Model": { "Name": "Luxury", "Brand": "Moody", "Id": 12 }, "Category": { "Name": "Straps", "Id": 4 }, "Id": 15 },
            { "ModelId": 13, "SalePrice": 150.00, "ListPrice": 180.00, "Rating": 2, "Photo": "LM Products Iron Cross Stud 2 Inch Guitar Strap.png", "CategoryId": 4, "ItemNumber": "12972", "Description": "LM Products Iron Cross Stud 2 Inch Guitar Strap", "Reviews": [], "Model": { "Name": "Iron Cross", "Brand": "LM", "Id": 13 }, "Category": { "Name": "Straps", "Id": 4 }, "Id": 17 },
            { "ModelId": 14, "SalePrice": 64.99, "ListPrice": 64.99, "Rating": 4, "Photo": "Jodi Head 3 Inch Denny Wide Art Deco Guitar Strap - Brown and Tan Sequin Sparkle.png", "CategoryId": 4, "ItemNumber": "98612", "Description": "Jodi Head 3\" Denny Wide Art Deco Guitar Strap - Brown and Tan Sequin Sparkle", "Reviews": [], "Model": { "Name": "Deco", "Brand": "Jodi", "Id": 14 }, "Category": { "Name": "Straps", "Id": 4 }, "Id": 18 },
            { "ModelId": 15, "SalePrice": 59.99, "ListPrice": 64.99, "Rating": 3, "Photo": "Levy\u0027s Leather Guitar Strap with Dog Tags.png", "CategoryId": 4, "ItemNumber": "71203", "Description": "Levy\u0027s Leather Guitar Strap with Dog Tags", "Reviews": [], "Model": { "Name": "Dog Tags", "Brand": "Levy\u0027s", "Id": 15 }, "Category": { "Name": "Straps", "Id": 4 }, "Id": 19 },
            { "ModelId": 16, "SalePrice": 14.99, "ListPrice": 19.99, "Rating": 5, "Photo": "Dunlop Trigger Classical Guitar Capo.png", "CategoryId": 3, "ItemNumber": "76123", "Description": "Dunlop Trigger Classical Guitar Capo", "Reviews": [], "Model": { "Name": "Trigger", "Brand": "Dunlop", "Id": 16 }, "Category": { "Name": "Capos", "Id": 3 }, "Id": 20 },
            { "ModelId": 17, "SalePrice": 16.99, "ListPrice": 17.99, "Rating": 4, "Photo": "Paige 6-String Guitar Capo.png", "CategoryId": 3, "ItemNumber": "36581", "Description": "Paige 6-String Guitar Capo", "Reviews": [], "Model": { "Name": "Basic Capo", "Brand": "Paige", "Id": 17 }, "Category": { "Name": "Capos", "Id": 3 }, "Id": 21 },
            { "ModelId": 18, "SalePrice": 24.99, "ListPrice": 25.99, "Rating": 4, "Photo": "Glider GL-1 Guitar Capo.png", "CategoryId": 3, "ItemNumber": "23421", "Description": "Glider GL-1 Guitar Capo", "Reviews": [], "Model": { "Name": "GL-1", "Brand": "Glider", "Id": 18 }, "Category": { "Name": "Capos", "Id": 3 }, "Id": 22 },
            { "ModelId": 19, "SalePrice": 14.99, "ListPrice": 18.99, "Rating": 1, "Photo": "Planet Waves NS Classical Guitar Capo.png", "CategoryId": 3, "ItemNumber": "25232", "Description": "Planet Waves NS Classical Guitar Capo", "Reviews": [], "Model": { "Name": "NS", "Brand": "Planet Waves", "Id": 19 }, "Category": { "Name": "Capos", "Id": 3 }, "Id": 23 },
            { "ModelId": 2, "SalePrice": 649.00, "ListPrice": 899.00, "Rating": 4, "Photo": "Taylor 314-CE Left-Handed Grand Auditorium Acoustic-Electric Guitar.png", "CategoryId": 1, "ItemNumber": "T110CE", "Description": "Taylor 114-CE Left-Handed Grand Auditorium Acoustic-Electric Guitar", "Reviews": [], "Model": { "Name": "110ce", "Brand": "Taylor", "Id": 2 }, "Category": { "Name": "Acoustic Guitars", "Id": 1 }, "Id": 25 }
        ]
    };
    return {
        data: data
    };
})(my);

app.data = (function () {
    var allPeople = ko.observableArray([
        new app.Person(1, 'John', 'Papa', '@john_papa'),
        new app.Person(2, 'Landon', 'Papa', '@landonpapa'),
        new app.Person(3, 'Ryan', 'Niemeyer', '@rpniemeyer'),
        new app.Person(4, 'Steve', 'Sanderson', '@stevensanderson'),
        new app.Person(5, 'Ward', 'Bell', '@wardbell'),
        new app.Person(6, 'Scott', 'Guthrie', '@scottgu')
    ]);
    var getPeople = function (count) {
        var people = [];
        for (var i = 1; i <= count; i++) {
            people.push(new app.Person(i, 'John' + i, 'Papa' + i, '@john_papa' + i));
        }
        return ko.observableArray(people);
    };
    return {
        allPeople: allPeople,
        getPeople: getPeople
    };
}());

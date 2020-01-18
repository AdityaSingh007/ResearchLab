(function ($) {
    $.log = function (value) {
        if (console)
            console.log(value);
    }
    $.log.group = function (value) {
        if (console && console.group)
            console.group(value);
    }
    $.log.groupEnd = function () {
        if (console && console.group)
            console.groupEnd();
    }
})(jQuery);	
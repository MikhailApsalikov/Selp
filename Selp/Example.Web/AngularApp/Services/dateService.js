(function() {
    "use strict";

    angular
        .module("APP")
        .service("dateService", [dateService]);

    function dateService() {
        return {
            toString: function(date) {
                return date.toLocaleDateString("ru-RU");
            },
            toDate: function(string) {
                var splitted = string.split(".");

                return new Date(splitted[2], +splitted[1] - 1, +splitted[0]);
            }
        };
    }
})();
(function() {
    "use strict";

    angular
        .module("APP")
        .controller("mainController", ["$scope", "$rootScope", "loginService", "$location", mainController]);

    function mainController($scope, $rootScope, loginService, $location) {
        activate();

        $scope.logout = function() {
            loginService.logout();
        };

        $scope.back = function() {
            $location.path($scope.data.backAction);
        };

        function activate() {
            $scope.isLoggedIn = loginService.isAuthenticated;
            $rootScope.$on("$stateChangeStart",
                function(event, next) {
                    if (!next || !next.mainControllerData) {
                        return;
                    }

                    $scope.data = next.mainControllerData;
                });
        }
    }
})();
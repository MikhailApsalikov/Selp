(function() {
    "use strict";

    angular
        .module("APP")
        .controller("policyListController", ["$scope", "policyService", policyListController]);

    function policyListController($scope, policyService) {
        $scope.refresh = function() {
            $scope.isLoaded = false;
            policyService.getPolicyList({
                    Page: $scope.page,
                    PageSize: $scope.pageSize
                })
                .then(function(data) {
                        $scope.policyList = data.data;
                        $scope.isLoaded = true;
                    },
                    function(data) {
                        $scope.isLoaded = true;
                    });
        };

        activate();

        $scope.$watch("page",
           function () {
               $scope.refresh();
           });

        $scope.$watch("pageSize",
            function () {
                $scope.page = 1;
                $scope.refresh();
            });

        function activate() {
            $scope.page = 1;
            $scope.pageSize = 25;
            $scope.refresh();
        }
    }
})();
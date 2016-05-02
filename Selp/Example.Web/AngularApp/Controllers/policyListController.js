(function() {
    "use strict";

    angular
        .module("APP")
        .controller("policyListController", ["$scope", "policyService", policyListController]);

    function policyListController($scope, policyService) {
        $scope.refresh = function() {
            policyService.getPolicyList({
                    Page: $scope.page,
                    PageSize: $scope.pageSize
                })
                .then(function(data) {
                        $scope.policyList = data.data;
                    },
                    function(data) {

                    });
        };

        activate();

        function activate() {
            $scope.page = 1;
            $scope.pageSize = 25;
            $scope.refresh();
        }
    }
})();
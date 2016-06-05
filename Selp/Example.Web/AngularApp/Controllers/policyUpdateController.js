(function () {
	'use strict';

	angular
      .module('APP')
      .controller('policyUpdateController', ['$scope', 'policyService', '$stateParams', policyCreateController]);

	function policyCreateController($scope, policyService, $stateParams) {
	    $scope.save = function() {
	        save();
	    };

	    $scope.changeStatus = function () {
	        switch ($scope.policy.Status) {
	            case "Проект":
	                $scope.policy.Status = "Действующий";
	                break;
	            case "Действующий":
	                $scope.policy.Status = "Аннулирован";
                    break;
	            default:
	                return;
	        }
	        save();
	    };

	    $scope.isProject = function() {
	        return $scope.policy.Status === "Проект";
	    };

	    $scope.isActual = function () {
	        return $scope.policy.Status === "Действующий";
	    };

	    $scope.isAnnulated = function() {
	        return $scope.policy.Status === "Аннулирован";
	    };

	    $scope.$watch("policy.StartDate", function (newValue) {
	        if (!newValue || !$scope.isProject()) {
	            return;
	        }
	        $scope.policy.ExpirationDate = addDays(newValue, 364);
	    });

	    $scope.$watch("policy.InsurancePremium", function (newValue) {
	        if (!newValue || !$scope.isProject()) {
	            return;
	        }
	        $scope.policy.InsuranceSum = newValue * 500;
	    });

	    activate();

	    function activate() {
	        policyService.getPolicyById($stateParams.id).then(function (policy) {
	            $scope.policy = policy;
	            $scope.title = "Полис №" + $scope.policy.Id;
	            $scope.isLoaded = true;
	            $scope.regions = [
	            {
	                Id: $scope.policy.RegionId,
	                Name: $scope.policy.Region
	            }];
	            loadRegions();
	        });
	    }

	    function addDays(date, days) {
	        var result = new Date(date);
	        result.setDate(result.getDate() + days);
	        return result;
	    }

	    function loadRegions() {
	        policyService.getRegions().then(function (loadRegions) {
	            $scope.regions = loadRegions;
	        });
	    }

	    function save() {
	        $scope.isLoaded = false;
            policyService.updatePolicy($scope.policy.Id, $scope.policy).then(function () {
                console.log(arguments);
                $scope.isLoaded = true;
            });
        };
	}
})();
(function () {
	'use strict';

	angular
      .module('APP')
      .controller('policyCreateController', ['$scope', 'policyService', policyCreateController]);

	function policyCreateController($scope, policyService) {
	    $scope.isProject = function() {
	        return true;
	    };

	    $scope.isActual = function () {
	        return false;
	    };
         
	    $scope.isAnnulated = function() {
	        return false;
	    };

	    $scope.generateNumber = function() {
	        policyService.generatePolicyNumber().then(function(data) {
	            $scope.policy.Serial = data.Serial;
	            $scope.policy.Number = data.Number;
	        });
	    };

	    $scope.$watch("policy.StartDate", function (newValue) {
            if (!newValue) {
                return;
            }
	        $scope.policy.ExpirationDate = addDays(newValue, 364);
	    });

	    $scope.$watch("policy.InsurancePremium", function (newValue) {
	        if (!newValue) {
	            return;
	        }
	        $scope.policy.InsuranceSum = newValue*500;
	    });

	    activate();

        function activate() {
            $scope.title = "Создание нового полиса";
            $scope.policy = policyService.createNew();
            $scope.isLoaded = true;
        }

        function addDays(date, days) {
            var result = new Date(date);
            result.setDate(result.getDate() + days);
            return result;
        }
	}
})();
(function () {
	'use strict';

	angular
      .module('APP')
      .controller('policyUpdateController', ['$scope', 'policyService', policyCreateController]);

	function policyCreateController($scope, policyService) {
	    $scope.isProject = function() {
	        return $scope.policy.PolicyStatus === "Проект";
	    };

	    $scope.isActual = function () {
	        return $scope.policy.PolicyStatus === "Действующий";
	    };

	    $scope.isAnnulated = function() {
	        return $scope.policy.PolicyStatus === "Аннулированный";
	    };

	    activate();

        function activate() {
            $scope.title = "Создание нового полиса";
            //$scope.policy = policyService.createNew();
			//$scope.isLoaded = true;
		}
	}
})();
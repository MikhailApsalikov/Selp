(function () {
	'use strict';

	angular
      .module('APP')
      .controller('policyUpdateController', ['$scope', 'policyService', '$stateParams', policyCreateController]);

	function policyCreateController($scope, policyService, $stateParams) {
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
	        policyService.getPolicyById($stateParams.id).then(function (policy) {
	            $scope.policy = policy;
	            $scope.title = "Полис №" + $scope.policy.Id;
	            $scope.isLoaded = true;
	        });
		}
	}
})();
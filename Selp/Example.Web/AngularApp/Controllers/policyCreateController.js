(function () {
	'use strict';

	angular
      .module('APP')
      .controller('policyCreateController', ['$scope', policyCreateController]);

	function policyCreateController($scope) {
		activate();

		function activate() {
			$scope.isLoaded = true;
			$scope.policy = {
				Id: 555,
				Serial: "XXY",
				Number: "9876543210",
				CreatedDate: "01.01.2000",
				StartDate: "01.01.2016",
				ExpirationDate: "01.01.2017",
				InsurancePremium: 1000,
				InsuranceSum: 500000,
				PolicyStatus: "Project",
				UserId: "admin",
				Region: {
					Id: "5",
					Name: "Какая-то область"
				}

		};
			/*setTimeout(function() {
				$scope.isLoaded = true;
				$scope.$apply();
			}, 1500);*/
		}
	}
})();
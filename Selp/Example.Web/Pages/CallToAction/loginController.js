(function () {
	'use strict';

	angular
      .module('APP')
      .controller('loginController', ['$scope', 'loginService', loginController]);

	function loginController($scope, loginService) {
		$scope.login = function() {
			loginService.login($scope.user.id, $scope.user.password);
		};

		activate();

		function activate() { }
	}
})();
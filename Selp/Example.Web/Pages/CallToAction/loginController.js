(function() {
	"use strict";

	angular
		.module("APP")
		.controller("loginController", ["$scope", "loginService", "$location", "$mdDialog", loginController]);

	function loginController($scope, loginService, $location, $mdDialog) {
		$scope.login = function() {
			$scope.error = null;
			loginService.login($scope.user.id, $scope.user.password)
				.then(function(data) {
						if (data.valid) {
							$mdDialog.hide();
							$location.path("/policyCreate");
							return;
						}

						$scope.error = data.error;
					},
					function(result) {
						if (result.status === 404) {
							$scope.error = "Неверный логин/пароль";
							return;
						}

						$scope.error = "Ошибка сервера авторизации, попробуйте зайти позднее.";
					});
		};

		activate();

		function activate() {}
	}
})();
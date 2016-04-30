(function() {
	"use strict";

	angular
		.module("APP")
		.controller("loginController", ["$scope", "loginService", "$location", "$mdDialog", loginController]);

	function loginController($scope, loginService, $location, $mdDialog) {
		$scope.login = function() {
			$scope.isInProgress = true;
			$scope.loginErrors = null;
			$scope.passwordErrors = null;
			loginService.login($scope.user.id, $scope.user.password)
				.then(function(data) {
						var errors;
						if (data.valid) {
							$mdDialog.hide();
							$location.path("/policyCreate");
							return;
						}

						errors = Enumerable.From(data.errors);

						$scope.loginErrors = errors.Where(function(x) {
								return x.FieldName === "Id";
							})
							.Select(function(x) {
								return x.Text;
							})
							.ToArray();
						$scope.passwordErrors = errors.Where(function(x) {
								return x.FieldName !== "Id";
							})
							.Select(function(x) {
								return x.Text;
							})
							.ToArray();
					},
					function(result) {
						if (result.status === 404) {
							$scope.passwordErrors = ["Неверный логин/пароль"];
							return;
						}

						$scope.passwordErrors = ["Ошибка сервера авторизации, попробуйте зайти позднее."];
					})
				.finally(function() {
					$scope.isInProgress = false;
				});
		};

		activate();

		function activate() {
			$scope.user = {};
		}
	}
})();
(function() {
	"use strict";

	angular
		.module("APP")
		.controller("signupController", ["$scope", "loginService", "$mdDialog", '$location', signupController]);

	function signupController($scope, loginService, $mdDialog, $location) {
		$scope.signup = function() {
			if (!$scope.arePasswordsTheSame()) {
				return;
			}
			$scope.isInProgress = true;

			$scope.loginErrors = null;
			$scope.passwordErrors = null;
			loginService.signup($scope.user.id, $scope.user.password)
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
					function() {
						$scope.passwordErrors = ["Ошибка сервера авторизации, попробуйте зайти позднее."];
					})
				.finally(function() {
					$scope.isInProgress = false;
				});
		};

		activate();

		$scope.arePasswordsTheSame = function() {
			return !$scope.user || $scope.user.password === $scope.user.passwordConfirmation;
		};

		function activate() {
			$scope.user = {};
			$scope.passwordConfirmationError = "Введенные пароли не совпадают!";
		}
	}
})();
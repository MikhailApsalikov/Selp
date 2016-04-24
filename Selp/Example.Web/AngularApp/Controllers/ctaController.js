(function () {
	'use strict';

	angular
      .module('APP')
      .controller('ctaController', ['$scope', '$mdDialog', ctaController]);

	function ctaController($scope, $mdDialog) {
		var vm = this;
		activate();

		vm.login = function () {

		};

		vm.signup = function () {

		};

		$scope.learnMore = function (ev) {
			$mdDialog.show(
				$mdDialog.alert()
				.parent(angular.element(document.querySelector('#popupContainer')))
				.clickOutsideToClose(true)
				.title('This is an alert title')
				.textContent('You can specify some description text in here.')
				.ariaLabel('Alert Dialog Demo')
				.ok('Got it!')
				.targetEvent(ev));
		};

		function activate() {
			$(document).ready(function () {
				$('.parallax').parallax();
			});
		}
	}
})();
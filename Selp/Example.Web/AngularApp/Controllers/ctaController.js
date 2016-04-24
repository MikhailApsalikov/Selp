(function () {
	'use strict';

	angular
      .module('APP')
      .controller('ctaController', ['$scope', '$mdDialog', ctaController]);

	function ctaController($scope, $mdDialog) {
		var vm = this;
		activate();

		$scope.login = function (ev) {
			$mdDialog.show({
				templateUrl: 'Pages/CallToAction/loginTemplate.html',
				parent: angular.element(document.body),
				targetEvent: ev,
				clickOutsideToClose: true,
				fullscreen: false
			});
		};

		$scope.signup = function (ev) {
			$mdDialog.show({
				templateUrl: 'Pages/CallToAction/signupTemplate.html',
				parent: angular.element(document.body),
				targetEvent: ev,
				clickOutsideToClose: true,
				fullscreen: false
			});
		};

		$scope.learnMore = function (ev) {
			$mdDialog.show(
				$mdDialog.alert()
				.parent(angular.element(document.querySelector('#popupContainer')))
				.clickOutsideToClose(true)
				.title('Контакты')
				.textContent('Вы можете найти нас по адресу г.Москва ул. Пушкина д.5 или позвонить по телефону +7 (123) 456-78-90')
				.ariaLabel('Alert Dialog Demo')
				.ok('ОК!')
				.targetEvent(ev));
		};

		function activate() {
			$(document).ready(function () {
				$('.parallax').parallax();
			});
		}
	}
})();
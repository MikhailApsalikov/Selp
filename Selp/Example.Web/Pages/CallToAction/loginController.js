(function () {
	'use strict';

	angular
      .module('APP')
      .controller('loginController', ['$scope', loginController]);

	function loginController($scope) {
		var vm = this;

		vm.title = '';

		activate();

		function activate() { }
	}
})();
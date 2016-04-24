(function () {
	'use strict';

	angular
      .module('APP')
      .controller('signupController', ['$scope', signupController]);

	function signupController($scope) {
		var vm = this;

		vm.title = '';

		activate();

		function activate() { }
	}
})();
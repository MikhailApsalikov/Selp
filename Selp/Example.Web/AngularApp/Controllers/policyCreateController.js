(function () {
	'use strict';

	angular
      .module('APP')
      .controller('policyCreateController', ['$scope', policyCreateController]);

	function policyCreateController($scope) {
		var vm = this;

		vm.title = '';

		activate();

		function activate() { }
	}
})();
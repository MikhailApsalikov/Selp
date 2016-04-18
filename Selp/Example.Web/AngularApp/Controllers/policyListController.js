(function () {
	'use strict';

	angular
      .module('APP')
      .controller('policyListController', ['$scope', policyListController]);

	function policyListController($scope) {
		var vm = this;

		vm.title = '';

		activate();

		function activate() { }
	}
})();
(function () {
	'use strict';

	angular
      .module('APP')
      .controller('ctaController', ['$scope', ctaController]);

	function ctaController($scope) {
		var vm = this;
		vm.title = '';

		activate();

		function activate() { }
	}
})();
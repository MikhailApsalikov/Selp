(function () {
    'use strict';

    angular
      .module('APP')
      .controller('mainController', ['$scope', 'loginService', mainController]);

    function mainController($scope, loginService) {
        var vm = this;
        $scope.isLoggedIn = loginService.isAuthenticated;

        vm.title = '';

        activate();

        function activate() { }
    }
})();
(function () {
	'use strict';
	angular.module("APP", ["ui.router", "ngMaterial"]).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
		$urlRouterProvider
			.when('', '/callToAction')
			.otherwise('/policyList');

		$stateProvider
			.state('callToAction', {
				url: '/callToAction',
				templateUrl: '/Pages/CallToAction/template.html',
				controller: 'ctaController'
			})
			.state('policyList', {
				url: '/policyList',
				templateUrl: '/Pages/PolicyList/template.html',
				controller: 'policyListController'
			});

			/*.state('tasks.new', {
				url: '/new',
				templateUrl: '/Home/NewTask',
				controller: 'newTaskController',
				data: {
					authorizedRoles: [USER_ROLES.admin, USER_ROLES.user]
				}
			})
			.state('tasks.id', {
				url: '/:id',
				templateUrl: '/Home/TheTask',
				controller: 'taskInstanceController',
				data: {
					authorizedRoles: [USER_ROLES.admin, USER_ROLES.user]
				}
			});*/
	}]);
}());
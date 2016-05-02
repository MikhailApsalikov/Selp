(function() {
	"use strict";
	angular.module("APP", ["ui.router", "ngMaterial"])
		.config([
			"$stateProvider", "$urlRouterProvider", function($stateProvider, $urlRouterProvider) {
				$urlRouterProvider
					.when("", "/callToAction")
					.otherwise("/callToAction");

				$stateProvider
					.state("callToAction",
					{
						url: "/callToAction",
						templateUrl: "/Pages/CallToAction/template.html",
						controller: "ctaController"
					})
					.state("policyList",
					{
						url: "/policyList",
						templateUrl: "/Pages/PolicyList/template.html",
						controller: "policyListController",
                        mainControllerData: {
                            title: "Список полисов",
                            isBackEnabled: false,
                            backAction: null
                        }
					})
					.state("policyCreate",
					{
						url: "/policyCreate",
						templateUrl: "/Pages/policyCreate/template.html",
						controller: "policyCreateController",
						mainControllerData: {
						    title: "Создание полиса",
						    isBackEnabled: true,
						    backAction: '/policyList'
						}
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
			}
		])
		.run(function($rootScope, loginService, $location) {
			$rootScope.$on("$stateChangeStart",
				function(event, next) {
					if (next.name === "callToAction") {
						return;
					}

					if (!loginService.isAuthenticated()) {
						$location.path("/callToAction");
					}
				});
		});;
}());
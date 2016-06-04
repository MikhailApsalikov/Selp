(function() {
	"use strict";
	angular.module("APP", ["ui.router", "ngMaterial"])
		.config([
			"$stateProvider", "$urlRouterProvider", "$mdDateLocaleProvider", function ($stateProvider, $urlRouterProvider, $mdDateLocaleProvider) {
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
						url: "/policy",
						templateUrl: "/Pages/policyCreate/template.html",
						controller: "policyCreateController",
						mainControllerData: {
						    title: "Создание полиса",
						    isBackEnabled: true,
						    backAction: '/policyList'
						}
					})
			        .state("policyUpdate",
					{
					    url: "/policy/:id",
					    templateUrl: "/Pages/policyCreate/template.html",
					    controller: "policyUpdateController",
					    mainControllerData: {
					        title: "Изменение полиса",
					        isBackEnabled: true,
					        backAction: '/policyList'
					    }
					});

	            $mdDateLocaleProvider.firstDayOfWeek = 1;
	            $mdDateLocaleProvider.months = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];
	            $mdDateLocaleProvider.shortMonths = ["Янв", "Фев", "Март", "Апр", "Май", "Июнь", "Июль", "Авг", "Сен", "Окт", "Нояб", "Дек"];
	            $mdDateLocaleProvider.days = ['Воскресенье', 'Понедельник', 'Вторник', 'Среда', 'Четверг','Пятница', 'Суббота'];
	            $mdDateLocaleProvider.shortDays = ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];

	            $mdDateLocaleProvider.parseDate = function (string) {
	                if (!string) {
	                    return null;
	                }
	                var splitted = string.split(".");

	                return new Date(splitted[2], +splitted[1] - 1, +splitted[0]);
	            };
	            $mdDateLocaleProvider.formatDate = function (date) {
	                if (!date) {
	                    return "";
	                }

	                return date.toLocaleDateString("ru-RU");
	            };
	            $mdDateLocaleProvider.msgCalendar = 'Календарь';
	            $mdDateLocaleProvider.msgOpenCalendar = 'Открыть календарь';
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
(function() {
	"use strict";

	angular
		.module("APP")
		.service("loginService", ["urlService", "$http", "$rootScope", loginService]);

	function loginService(urls, $http, $rootScope) {
		return {
			login: function (login, password) {
				return $http.post(urls.login,
				{
					Id: login,
					Password: password
				}).then(function (result) {
					if (result.data.valid) {
						window.localStorage['login'] = login;
					}
					
					return result.data;
				});
			},
			logout: function() {
				window.localStorage['login'] = null;
				$rootScope.$emit('$stateChangeStart');
			},
			isAuthenticated: function() {
				return !!window.localStorage['login'];
			},
			signup: function(login, password) {
				return $http.post(urls.signup,
				{
					Id: login,
					Password: password
				}).then(function (result) {
					if (result.data.valid) {
						window.localStorage['login'] = login;
					}

					return result.data;
				});
			}
		};
	}
})();
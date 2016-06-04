(function() {
	"use strict";

	angular
		.module("APP")
		.service("loginService", ["urlService", "$http", "$location", loginService]);

	function loginService(urls, $http, $location) {
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
			    window.localStorage.removeItem('login');
				$location.path('/callToAction');
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
			},
			getCurrentUserName: function () {
                return window.localStorage['login'];
            }
		};
	}
})();
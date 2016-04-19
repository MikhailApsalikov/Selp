(function() {
	"use strict";

	angular
		.module("APP")
		.service("loginService", ["urlService", loginService]);

	function loginService(urls) {


		return {
			login: function(login, password) {

			},
			logout: function() {

			},
			isAuthenticated: function() {

			},
			signup: function(login, password) {

			}
		};
	}
})();
(function() {
	"use strict";

	angular
		.module("APP")
		.service("loginService", ["urlService", loginService]);

	function loginService(urls) {


		return {
			login: function(login, password) {
				console.log(login, password, "Залогинилися");
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
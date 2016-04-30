(function () {
	"use strict";

	angular
		.module("APP")
		.service("urlService", [urlService]);

	function urlService() {
		var urls = {
			//uncommon
			login: "api/user/login",

			//controllers
			user: "api/user"
		};

		return urls;
	}
})();
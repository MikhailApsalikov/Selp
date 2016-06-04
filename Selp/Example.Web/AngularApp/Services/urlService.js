(function () {
	"use strict";

	angular
		.module("APP")
		.service("urlService", [urlService]);

	function urlService() {
		var urls = {
			//uncommon
			login: "api/user/login",
			signup: "api/user/signup",
			generatePolicyNumber: "api/policy/generateNumber",

			//controllers
			user: "api/user",
            policy: "api/policy"
		};

		return urls;
	}
})();
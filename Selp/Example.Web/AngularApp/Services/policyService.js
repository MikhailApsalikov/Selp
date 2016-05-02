(function() {
	"use strict";

	angular
		.module("APP")
		.service("policyService", ["urlService", "$http", policyService]);

	function policyService(urls, $http) {
		return {
		    getPolicyList: function (params) {
		        return $http.get(urls.policy, params);
			}
		};
	}
})();
(function() {
	"use strict";

	angular
		.module("APP")
		.service("policyService", ["urlService", "$http", policyService]);

	function policyService(urls, $http) {
	    var template = {
	        
	    };

	    return {
		    getPolicyList: function (params) {
		        return $http({
		            url: urls.policy, 
		            method: "GET",
		            params: params
		        });
		    },
            createEmptyPolicy: function() {
                return angular.copy(template);
            }
		};
	}
})();
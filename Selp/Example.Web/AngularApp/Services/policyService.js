(function() {
	"use strict";

	angular
		.module("APP")
		.service("policyService", ["urlService", "$http", 'loginService', 'dateService', policyService]);

	function policyService(urls, $http, loginService, dateService) {
	    var template = {
	        CreatedDate: dateService.toString(new Date()),
	        PolicyStatus: "Проект"
	    };

	    return {
		    getPolicyList: function (params) {
		        return $http({
		            url: urls.policy, 
		            method: "GET",
		            params: params
		        });
		    },
            getPolicyById: function(id) {
                return $http({
                    url: urls.policy,
                    method: "GET",
                    params: {
                        id: id
                    }
                }).then(function(data) {
                    data.data.StartDate = dateService.toDate(data.data.StartDate);
                    data.data.ExpirationDate = dateService.toDate(data.data.ExpirationDate);
                    return data.data;
                });
            },
		    createNew: function () {
		        template.UserId = loginService.getCurrentUserName();
                return angular.copy(template);
		    },
            generatePolicyNumber: function() {
                return $http.get(urls.generatePolicyNumber).then(function (result) {
				    return result.data;
				});
            },
            getRegions: function() {
                return $http({
                    url: urls.region,
                    method: "GET"
                }).then(function(result) {
                    return result.data;
                });
            }
		};
	}
})();
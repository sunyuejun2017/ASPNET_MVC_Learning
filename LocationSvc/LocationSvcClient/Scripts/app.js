/// <reference path="lib/angular.js" />

var locationApp = angular.module('locationApp', []);

var locationController = locationApp.controller("LocationController", function ($scope, $http) {
    $scope.getLocation = function () {
        $http.get("https://localhost:44304/api/location?cityName=dc").success(function (location) {
            $scope.city = location;
        });
    }
});
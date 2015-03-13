var garageApp = angular.module("GarageApp", [])

.controller("AppController", ['$scope',
    function ($scope) {
        $scope.Init = function () {
            
        };

        $scope.Messages;

        $scope.ShowMessage = function (msg) {
            $scope.Messages = [{ Message: msg }];
            $("#message").slideDown(400);
            $timeout(function () {
                $scope.Messages = {};
                $("#message").slideUp(300);
            }, 2000);
        };

        $scope.close = function (id) {
            $("#" + id).slideUp();
        };

        $scope.Init();
    }])

.controller("ParkingPlaceController", ['$scope', '$interval', 'parkingPlace', '$timeout', '$q',
    function ($scope, $interval, parkingPlace, $timeout, $q) {
        $scope.Init = function () {
            $scope.ParkingPlaces = $scope.getAllParkingPlaces();
            $interval(function () {
                $scope.getAllParkingPlaces();
            }, 7000);
        };
        $scope.ParkingPlaces;
        $scope.Messages;
        
        $scope.getAllParkingPlaces = function () {
            parkingPlace.GetAllParkingPlaces()
            .then(
            function (data) {
                //success
                $scope.ParkingPlaces = parkingPlace.ParkingPlaceList;
            },
            function (data, status, header) {
                //error
                $scope.ShowMessage(data);
            });
        };

        $scope.ShowMessage = function(msg) {
            $scope.Messages = [{Message : msg}];
            $("#message").slideDown(400);
            $timeout(function () {
                $scope.Messages = {};
                $("#message").slideUp(300);
            }, 2000);
        };

        $scope.close = function (id) {
            $("#" + id).slideUp();
        };

        $scope.Init();
    }]);

$(document).ready(



);
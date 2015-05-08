﻿garageApp

.controller('ParkingPlaceCtrl', ['$scope', 'parkingPlaces',
    function ($scope, parkingPlaces) {
        $scope.init = function () {
            $scope.getAllParkingPlaces();
        }

        $scope.getAllParkingPlaces = function () {
            parkingPlaces.GetAllParkingPlaces()
            .then(
                function (data) {
                    //success
                    $scope.ParkingPlaces = parkingPlaces.parkingPlaceList;
                },
                function (data) {
                    //error
                    $scope.ShowMessage(data, 2000);
                });
        }

        $scope.init();
    }])

.controller('ParkingDetailCtrl', ['$scope', 'parkingPlaces', '$routeParams', '$interval',
    function ($scope, parkingPlaces, $routeParams, $interval) {
        var stop;

        $scope.init = function () {
            parkingPlaces.GetParkingPlace($routeParams.id)
            .then(function (data) {
                $scope.ParkingPlaces = data;
                stop = $interval(function () {
                    $scope.getParkingPlace();
                }, 7000);
            });
        }

        $scope.ParkingPlaces = {};

        $scope.getParkingPlace = function () {
            parkingPlaces.GetParkingPlace($routeParams.id)
            .then(
                function (data) {
                    //success
                    $scope.ParkingPlaces = parkingPlaces.parkingPlaceList;
                },
                function (data) {
                    //error
                    $scope.ShowMessage(data, 2000);
                });
        }

        $scope.$on('$destroy', function () {
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
            }
        });

        $scope.init();
    }])

    .controller('AppController', ['$scope', 'geolocationService', '$http', '$interval', '$timeout',
    function ($scope, geolocationService, $http, $interval, $timeout) {
        var msgTimer;
        $scope.init = function () {
            $scope.getGeolocation();
        };

        $scope.Count = 0;
        $scope.Position;
        $scope.getGeolocation = function () {
            geolocationService.getGeolocation()
                .then(
                function (position) {
                    //success
                    console.log(position);
                    $scope.Count++;
                    $scope.Time = position.timestamp;
                    $scope.Position = "lat: " + position.coords.latitude + " long: " + position.coords.longitude;
                    return geolocationService.sendLocation(position);
                },
                function (data) {
                    //error
                    console.log(data);
                })
                .then(function (result) {
                    console.log(result);
                    $scope.Info = 'interval: ' + result.interval + ' isParked:' + result.isParked + ' checkSpeed:' + result.checkSpeed;
                    $scope.getNewLocation(result.interval);
                },
                function (data) {
                    //error
                    console.log(data);
                });
        };

        var stop;
        $scope.getNewLocation = function (interval) {
            console.log(interval);
            console.log(stop);
            if (angular.isDefined(stop)) {
                console.log("Waap");
                $timeout.cancel(stop);
                stop = undefined;
            }
            console.log("Woop");
            stop = $timeout(function () {
                console.log("Woop");
                $scope.getGeolocation();
            }, interval);
            console.log(stop);
        };

        $scope.Messages;

        $scope.ShowMessage = function (msg) {
            if (angular.isDefined(msgTimer)) {
                $scope.Messages = {};
                $("#message").hide();
                $timeout.cancel(msgTimer);
                msgTimer = undefined;
            }
            $scope.Messages = [{ Message: msg }];
            $("#message").slideDown(400);
            msgTimer = $timeout(function () {
                $scope.Messages = undefined;
                $("#message").slideUp(400);
            }, 2000);
        };

        $scope.close = function (id) {
            $("#" + id).slideUp();
        };

        $scope.init();
    }]);

$(document).ready(function () { });
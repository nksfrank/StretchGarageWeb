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
                    $scope.$emit('alert', [
                        { type: "danger", msg: data, }
                    ]);
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
                    $scope.$emit('alert', [
                        { type: "danger", msg: data, }
                    ]);
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

.controller('UnitCtrl', ['$scope', '$rootScope', 'settings', 'unitService', '$location', '$timeout',
    function ($scope, $rootScope, settings, unitService, $location, $timeout) {
        $scope.init = function () {
            if (settings.GetUser() !== undefined) {
                $scope.unit.Name = settings.GetUser();
                $scope.unit.Phonenumber = settings.GetNumber();
                $scope.toggleGpsText();
            }
        }

        $scope.unit = {};
        
        $scope.submit = function (isValid) {
            if (!isValid) return;
            var id = settings.GetId();
            if (!angular.isDefined(id)) {
                unitService.putUnit(settings.GetId(), $scope.unit, settings.GetType()).
                then(function () {
                    $scope.$emit('alert', [
                        { type: "success", msg: "Din profil har uppdaterats!", }
                    ]);
                    $timeout(function () {
                        $location.path("/");
                    }, 2000);
                });
            } else {
                unitService.createUnit($scope.unit)
                .then(function () {
                    $location.path("/");
                });
            }
        };

        $rootScope.$on('gpsChange', function (event, args) {
            $scope.toggleGpsText();
        });

        $scope.toggleGpsText = function() {
            if (settings.GetGps()) {
                $scope.gpsText = "Stoppa Gps";
            } else {
                $scope.gpsText = "Starta Gps";
            }
        }

        $scope.init();
    }])

.controller('AppController', ['$scope', '$rootScope', 'geolocationService', 'unitService', '$http', '$interval', '$timeout', 'settings', '$location',
    function ($scope, $rootScope, geolocationService, unitService, $http, $interval, $timeout, settings, $location) {
        $scope.init = function () {
            $scope.user = settings.GetUser();
            $scope.getNewLocation(0);
        };
        $scope.alerts = [];
        $scope.user;

        var SIZE = 3;
        $scope.lat = [];
        $scope.lng = [];
        $scope.spd = [];

        $scope.Count = 0;
        $scope.Position;
        $scope.getGeolocation = function () {
            geolocationService.getGeolocation()
                .then(
                function (position) {
                    //success
                    $scope.Count++;
                    $scope.Time = position.timestamp;

                    var lat = position.coords.latitude;
                    var lng = position.coords.longitude;
                    var spd = position.coords.speed;

                    if ($scope.lat.length >= SIZE) {
                        $scope.lat.splice(0, 1);
                    }
                    if ($scope.lng.length >= SIZE) {
                        $scope.lng.splice(0, 1);
                    }
                    if ($scope.spd.length >= SIZE) {
                        $scope.spd.splice(0, 1);
                    }

                    $scope.lat.push(lat);
                    $scope.lng.push(lng);

                    if (angular.isDefined(spd) || spd <= 10)
                        $scope.spd.push(-1);
                    else
                        $scope.spd.push(spd);

                    $scope.Position = "lat: " + lat + " long: " + lng;
                    return geolocationService.sendLocation($scope.lat, $scope.lng, $scope.spd);
                },
                function (data) {
                    //error
                    return $q.reject(data);
                })
                .then(function (result) {
                    $scope.Info = 'interval: ' + result.interval + ' isParked:' + result.isParked + ' checkSpeed:' + result.checkSpeed;
                    $scope.getNewLocation(result.interval);
                },
                function (data) {
                    //error
                });
        };

        var stop;
        $scope.getNewLocation = function (interval) {
            var id = settings.GetId();
            var gps = settings.GetGps();
            if (!angular.isDefined(id) || id === null || !gps)
                return;
            stop = $timeout(function () {
                $scope.getGeolocation();
            }, interval);
        };

        $scope.$on('alert', function (event, args) {
            $scope.alerts = $scope.alerts.concat(args);
            $timeout(function () {
                $scope.alerts = [];
            }, 2000);
        });

        $rootScope.$on('idChange', function (event, args) {
            $scope.getNewLocation(0);
        });

        $rootScope.$on('userChange', function (event, args) {
            $scope.user = args.user;
        });

        $scope.closeAlert = function(index) {
            $scope.alerts.splice(index, 1);
        };

        $scope.isReversible = function() {
            return $location.path() === "/";
        };

        $scope.toggleGps = function () {
            var gps = !settings.GetGps();
            settings.SetGps(gps);
            if (!gps) {
                $scope.stopGps();
            } else {
                $scope.startGps();
            }
        };

        $scope.stopGps = function () {
            if (angular.isDefined(stop)) {
                $timeout.cancel(stop);
                stop = undefined;
            }
        };

        $scope.startGps = function() {
            $scope.getNewLocation(0);
        };

        $scope.parkManually = function() {
            var location = $location.path().split("/");
            var index = location[location.length - 1];
            unitService.parkManually(index).
                then(function(result) {
                    $scope.alerts = [{ type: "success", msg: "Du har parkerats" }];
                    $timeout(function() {
                        $scope.alerts = [];
                    }, 2000);
                }, function(err) {
                    $scope.alerts = [{ type: "danger", msg: "Det gick inte att manuellt parkera bilen." }];
                    $timeout(function() {
                        $scope.alerts = [];
                    }, 2000);
                });
        };

        $scope.init();
    }]);
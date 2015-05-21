﻿garageApp
    .service('settings', function () {
        return {
            Id: function () {
                return window.localStorage.getItem("id");
            },
            User: function () {
                return window.localStorage.getItem("user");
            },
            Type: function () {
                return window.localStorage.getItem("type");
            },
            host: "http://localhost:3186/"
            //host: "http://stretchgarageweb.azurewebsites.net/"
        };
    })

    .service("geolocationService", ['$q', '$rootScope', '$http', 'settings',
        function geolocationService($q, $rootScope, $http, settings) {
            var geolocation = this;
            geolocation.getGeolocation = function () {
                var deferred = $q.defer();

                if (!navigator) {
                    $rootScope.$apply(function () {
                        deferred.reject(new Error("Geolocation is not supported"));
                    });
                } else {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        $rootScope.$apply(function () {
                            deferred.resolve(position);
                        });
                    }, function (error) {
                        $rootScope.$apply(function () {
                            deferred.reject(error);
                        });
                    }, { enableHighAccuracy: true });
                }

                return deferred.promise;
            }
            /*
            geolocation.sendLocation = function (position) {
                var lat = position.coords.latitude;
                var lng = position.coords.longitude;
                return $http.get(settings.host + 'api/CheckLocation/?id=' + settings.Id() + '&latitude=' + lat + '&longitude=' + lng)
                    .then(function (result) {
                        return result.data.content;
                    });
            }*/

            geolocation.sendLocation = function (lat, lng, spd) {
                var headers = "?id=" + settings.Id();
                for (var i = 0; i < lat.length; i++) {
                    headers += "&latitude[]=" + lat[i];
                    headers += "&longitude[]=" + lng[i];
                    headers += "&speed[]=" + spd[i];
                    //headers += "&speed[]=" + 11;
                }
                return $http.get(settings.host + 'api/CheckLocation/' + headers).
                then(function (result) {
                    return result.data.content;
                },
                function (err) {
                });
            };

            return geolocation;
        }])
    .service('parkingPlaces', ['$http', '$q', 'settings',
    function parkingPlaces($http, $q, settings) {
        var parkingPlace = this;
        parkingPlace.parkingPlaceList = {};

        parkingPlace.GetAllParkingPlaces = function () {
            var defer = $q.defer();
            $http({
                method: 'GET',
                url: settings.host + 'api/ParkingPlace/'
            })
            .success(function (data) {
                parkingPlace.parkingPlaceList = data;
                defer.resolve(data);
            })
            .error(function (err) {
                defer.reject(err);
            });

            return defer.promise;
        }

        parkingPlace.GetParkingPlace = function (id) {
            var defer = $q.defer();

            $http({
                method: 'GET',
                url: settings.host + 'api/ParkedCars/' + id
            })
            .success(function (data) {
                parkingPlace.parkingPlaceList = data;
                defer.resolve(data);
            })
            .error(function (err) {
                defer.reject(err);
            });

            return defer.promise;
        }

        return parkingPlace;
    }])

    .service("unitService", ['$http', 'settings', '$q',
    function unitService($http, settings, $q) {
        var unit = this;

        unit.createUnit = function (name) {
            var defer = $q.defer();

            $http({
                method: 'GET',
                url: settings.host + 'api/Unit/' + name + '/' + 0
            })
            .success(function (result) {
                console.log(result);
                if (!result.success) {
                    defer.reject(result.message);
                }
                else {
                    window.localStorage.setItem("id", result.content);
                    defer.resolve(result.content);
                }
            })
            .error(function (err) {
                defer.reject(err);
            });

            return defer.promise;
        }

        unit.putUnit = function (Id, Name, Type) {
            var defer = $q.defer();

            $http({
                method: 'PUT',
                data: {
                    id: Id,
                    name: Name,
                    type: Type,
                },
                url: settings.host + 'api/Unit/'
            }).
            success(function (result) {
                console.log(result);
                if (!result.success) {
                    defer.reject(result.message);
                } else {
                    window.localStorage.setItem("user", result.content.name);
                    window.localStorage.setItem("type", result.content.type);
                    defer.resolve(result.content);
                }
            }).
            error(function (err) {
                defer.reject(err);
            });

            return defer.promise;
        }

        return unit;
    }
    ]);

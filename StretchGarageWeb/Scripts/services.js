﻿garageApp
    .service('settings', ['$rootScope',
        function settings ($rootScope) {
            return {
                GetId: function () {
                    return window.localStorage.getItem("id");
                },
                SetId: function (id) {
                    window.localStorage.setItem("id", id);
                    $rootScope.$broadcast('idChange', { "id": id });
                },
                GetUser: function () {
                    return window.localStorage.getItem("user");
                },
                SetUser: function (user) {
                    window.localStorage.setItem("user", user);
                    $rootScope.$broadcast('userChange', { "user": user });
                },
                GetType: function () {
                    return window.localStorage.getItem("type");
                },
                SetType: function(type) {
                    window.localStorage.setItem("type", type);
                    $rootScope.$broadcast('typeChange', { "type": type });
                },
                host: "http://localhost:3186/"
                //host: "http://stretchgarageweb.azurewebsites.net/"
            };
        }
    ])

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
                return $http.get(settings.host + 'api/CheckLocation/?id=' + settings.GetId() + '&latitude=' + lat + '&longitude=' + lng)
                    .then(function (result) {
                        return result.data.content;
                    });
            }*/

            geolocation.sendLocation = function (lat, lng, spd) {
                var headers = "?id=" + settings.GetId();
                for (var i = 0; i < lat.length; i++) {
                    headers += "&latitude[]=" + lat[i];
                    headers += "&longitude[]=" + lng[i];
                    headers += "&speed[]=" + spd[i];
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
                if (!result.success) {
                    defer.reject(result.message);
                }
                else {
                    settings.SetId(result.content.id);
                    settings.SetUser(result.content.name);
                    settings.SetType(result.content.type);
                    defer.resolve(result.content);
                }
            })
            .error(function (err) {
                defer.reject(err);
            });

            return defer.promise;
        }

        unit.putUnit = function (_id, _name, _type) {
            var defer = $q.defer();

            $http({
                method: 'PUT',
                data: {
                    id: _id,
                    name: _name,
                    type: _type,
                },
                url: settings.host + 'api/Unit/'
            }).
            success(function (result) {
                console.log(result);
                if (!result.success) {
                    defer.reject(result.message);
                } else {
                    settings.SetUser(result.content.name);
                    settings.SetType(result.content.type);
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

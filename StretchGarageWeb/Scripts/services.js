garageApp

.service('parkingPlace',
    function parkingPlace($http, $q, $interval) {
        var stop;
        var parkingPlace = this;
        parkingPlace.ParkingPlaceList = {};

        parkingPlace.GetAllParkingPlaces = function () {
            var defer = $q.defer();
            $http({
                method: 'GET',
                url: '/api/ParkingPlace/'
            })
            .success(function (data) {
                parkingPlace.ParkingPlaceList = data;
                defer.resolve(data);
            })
            .error(function (err) {
                defer.reject(err);
            });

            return defer.promise;
        }

        parkingPlace.GetAllParkingPlacesInterval = function () {
            var defer = $q.defer();

            stop = $interval(function () {
                $http({
                    method: 'GET',
                    url: '/api/ParkingPlace/'
                })
                .success(function (data) {
                    spot.spotList = data;
                    defer.resolve(data);
                })
                .error(function (err) {
                    defer.reject(err);
                });

            }, 7000);
            return defer.promise;
        }

        parkingPlace.GetParkingPlace = function (id) {
            var defer = $q.defer();

            $http({
                method: 'GET',
                url: '/api/ParkingPlace/' + id
            })
            .success(function (data) {
                parkingPlace.ParkingPlaceList = data;
                defer.resolve(data);
            })
            .error(function (err) {
                defer.reject(err);
            });

            return defer.promise;
        }

        parkingPlace.GetParkingPlaceInterval = function (id) {
            if (angular.isDefined(stop)) return;

            var defer = $q.defer();
            stop = $interval(function () {
                $http({
                    method: 'GET',
                    url: '/api/ParkingPlace/' + id
                })
                .success(function (data) {
                    parkingPlace.ParkingPlaceList = data;
                    defer.resolve(data);
                })
                .error(function (err) {
                    defer.reject(err);
                });
            }, 7000);

            return defer.promise;
        }

        parkingPlace.cancel = function () {
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
            }
        };

        return parkingPlace;
    });
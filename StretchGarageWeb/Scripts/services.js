var garageApp = angular.module("GarageApp", [])

.service("parkingPlace", function spots($http, $q, $rootScope) {
    var parkingPlace = this;
    parkingPlaces.ParkingPlaceList = {};

    parkingPlace.GetAllParkingPlaces = function () {
        var defer = $q.defer();

        $http({
            method: 'GET',
            url: '/api/ParkingPlace/',
        })
        .success(function (data) {
            parkingPlaces.ParkingPlaceList = data;
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
            url: '/api/ParkingPlace/' + id,
        })
        .success(function (data) {
            parkingPlaces.ParkingPlaceList = data;
            defer.resolve(data);
        })
        .error(function (err) {
            defer.reject(err);
        });

        return defer.promise;
    }

    return parkingPlaces;
});
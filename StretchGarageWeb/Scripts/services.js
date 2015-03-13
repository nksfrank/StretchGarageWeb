garageApp

.service("parkingPlace", function parkingPlace($http, $q, $rootScope) {
    var parkingPlace = this;
    parkingPlace.ParkingPlaceList = {};

    parkingPlace.GetAllParkingPlaces = function () {
        var defer = $q.defer();

        $http({
            method: 'GET',
            url: '/api/ParkingPlace/',
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

    parkingPlace.GetParkingPlace = function (id) {
        var defer = $q.defer();

        $http({
            method: 'GET',
            url: '/api/ParkingPlace/' + id,
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

    return parkingPlace;
});
garageApp

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
                    $scope.showMessage("Woops, någonting gick fel!", 2000);
                });
        }

        $scope.init();
    }])

.controller('ParkingDetailCtrl', ['$scope', 'parkingPlaces', '$routeParams', '$interval',
    function ($scope, parkingPlaces, $routeParams, $interval) {
        var stop;
        $scope.init = function () {
            stop = $interval(function() {
                $scope.getParkingPlace();
            }, 7000);
        }

        $scope.ParkingPlaces = {
            "success": true,
            "message": "",
            "content": [
                { "isAvailable": true, "status": "Vaccant", "cssClass": "green" },
                { "isAvailable": true, "status": "Vaccant", "cssClass": "green" },
                { "isAvailable": true, "status": "Vaccant", "cssClass": "green" },
                { "isAvailable": true, "status": "Vaccant", "cssClass": "green" }
            ]
        };

        $scope.getParkingPlace = function () {
            parkingPlaces.GetParkingPlace($routeParams.id)
            .then(
                function(data) {
                    $scope.ParkingPlaces = parkingPlaces.parkingPlaceList;
                },
                function (data) {
                    //error
                    $scope.showMessage("Woops, någonting gick fel!", 2000);
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

.controller('AppController', ['$scope',
    function ($scope) {
        $scope.init = function () {
        }

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

        $scope.init();
    }]);

$(document).ready(
    $.get("api/CheckLocation", { lat: "55,60284900", long: "12,99749900" }).done(function (data) { console.log(data); })
);
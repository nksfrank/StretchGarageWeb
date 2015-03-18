garageApp

.controller('AppController', ['$scope',
    function ($scope) {
        $scope.init = function () {
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

        $scope.init();
    }])

.controller('ParkingPlaceCtrl', ['$scope', 'parkingPlace',
    function ($scope, parkingPlace) {
        $scope.init = function () {
            $scope.getAllParkingPlaces();
        }

        $scope.getAllParkingPlaces = function () {
            parkingPlace.cancel();
            parkingPlace.GetAllParkingPlaces()
            .then(
                function (data) {
                    //success
                    $scope.ParkingPlaces = parkingPlace.ParkingPlaceList;
                },
                function (data) {
                    //error
                    $scope.showMessage("Woops, någonting gick fel!", 2000);
                });
        }

        $scope.init();
    }])

.controller('ParkingDetailCtrl', ['$scope', 'parkingPlace', '$routeParams',
    function ($scope, parkingPlace, $routeParams) {
        $scope.init = function () {
            $scope.getAllParkingPlaces();
        }

        $scope.getAllParkingPlaces = function () {
            parkingPlace.cancel();
            parkingPlace.GetParkingPlaceInterval($routeParams.id)
            .then(
                function (data) {
                    //success
                    $scope.ParkingPlaces = parkingPlace.ParkingPlaceList;
                },
                function (data) {
                    //error
                    $scope.showMessage("Woops, någonting gick fel!", 2000);
                });
        }

        $scope.init();
    }]);

$(document).ready(



);
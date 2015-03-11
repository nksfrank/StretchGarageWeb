var garageApp = angular.module("GarageApp", [])

.controller("AppController", ['$scope', '$interval', '$http', '$timeout', '$q',
    function ($scope, $interval, $http, $timeout, $q) {
        $scope.ParkingId = 0;

        var fetchRequest = null;
        $interval(function () {
            if (fetchRequest) { fetchRequest.resolve(); }
            fetchRequest = $q.defer();

            $scope.ShowMessage("Uppdaterar platser från servern");

            $http({
                method: 'GET',
                url: '/api/ParkingSpot/',
                parameter: $scope.ParkingId,
            }).success(function(data) {    
                $scope.Spots = data;
            }).error(function() {
                $scope.ShowMessage("Error");
            });
        }, 7000);

        $scope.ShowMessage = function(msg) {
            $scope.Messages = msg;
            $("#message").slideDown(400);
            $timeout(function() {
                $scope.Messages = {};
                $("#message").slideUp(300);
            }, 2000);
        };

        $scope.close = function(id) {
            $("#" + id).slideUp();
        };

        $scope.Spots = [
            { IsAvailable: false, Status: "UPPTAGEN", CssClass: "red" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: false, Status: "UPPTAGEN", CssClass: "red" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
        ];
    }]);

$(document).ready(



);
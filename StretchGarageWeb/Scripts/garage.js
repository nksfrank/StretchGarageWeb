var garageApp = angular.module("GarageApp", [])

.controller("AppController", ['$scope', '$interval', '$http', '$timeout', '$q',
    function ($scope, $interval, $http, $timeout, $q) {

        var fetchRequest = null;
        $interval(function () {
            if (fetchRequest) { fetchRequest.resolve(); }
            fetchRequest = $q.defer();

            $scope.Messages = [{ Message: "Uppdaterar platser från servern" }, ];
            $("#message").slideDown("slow");
            $http({
                method: 'GET',
                url: '/api/ParkedCars/',
                params: { id : 1 }
            }).success(function (data) {
                $timeout(function () {
                    $scope.Messages = {};
                    $("#message").slideUp("slow");
                    //Move out of timer for live update
                    $scope.Spots = data;
                    console.log($scope.Spots);
                }, 2000);
            }).error(function () {
                $scope.Message = "Error";
                $("#message").slideDown("slow");
            });
        }, 7000);

        $scope.close = function (id) {
            $("#" + id).slideUp();
        };

        $scope.Messages;
        /*$scope.Spots = [
            { IsAvailable: false, Status: "UPPTAGEN", CssClass: "red" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: false, Status: "UPPTAGEN", CssClass: "red" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
            { IsAvailable: true, Status: "LEDIG", CssClass: "green" },
        ];*/
    }]);

$(document).ready(



);
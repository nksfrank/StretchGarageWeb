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
                url: 'http://nikz.se/projects/angular/garage/mockup.php',
            }).success(function (data) {
                $timeout(function () {
                    $scope.Messages = {};
                    $("#message").slideUp("slow");
                    //Move out of timer for live update
                    $scope.Spots = data;
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
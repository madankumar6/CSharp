'use strict';

var app = angular.module('Tracker', ['uiGmapgoogle-maps']);

app.controller('mapCtrl', function ($scope, $http, $interval) {
    $scope.Devices = [];

    $scope.CurrentDevice = null;
    $scope.SetCurrentDevice = function (currentDevice) {
        console.log(currentDevice);
        $scope.CurrentDevice = currentDevice;
        if ($scope.refresher == null) {
            $scope.GetDevicePosition($scope.CurrentDevice);
            $interval(function () {
                $scope.GetDevicePosition($scope.CurrentDevice);
            }, 3000);
        }
    };


    $scope.CurrentPosition = {
        latitude: null,
        longitude: null
    }

    $scope.map = {
    //    center: angular.copy($scope.CurrentPosition),
        zoom: 16
    };

    $scope.show = true;


    // Example
    //http://angular-ui.github.io/angular-google-maps/#!/api/marker
    $scope.marker = {
        id: 0,
        coords: $scope.CurrentPosition,
        options: { draggable: false },
        events: {
            dragend: function (marker, eventName, args) {
                var lat = marker.getPosition().lat();
                var lon = marker.getPosition().lng();
                $scope.marker.options = {
                    draggable: true,
                    labelContent: "lat: " + $scope.marker.coords.latitude + ' ' + 'lon: ' + $scope.marker.coords.longitude,
                    labelAnchor: "100 0",
                    labelClass: "marker-labels"
                };
            }
        }
    };


    $scope.GetDeviceList = function () {
        $http({
            method: 'GET',
            url: ApiUrl + "GetDeviceList"
        }).then(function successCallback(response) {
            $scope.Devices = response.data;
        }, function errorCallback(response) {
        });
    };

    $scope.GetDevicePosition = function (currentDevice) {
        $http({
            method: 'GET',
            url: ApiUrl + "GetCurrentPosition/?DeviceId=" + currentDevice
        }).then(function successCallback(response) {
            console.log(response);
            $scope.ChangeMarkerPosition(({
                latitude: response.data.Latitude,
                longitude: response.data.Longitude
            }));
            // this callback will be called asynchronously
            // when the response is available
        }, function errorCallback(response) {
            console.log("Error: ");
            console.log(response);
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    };

    $scope.GetDeviceList();

    $scope.refresher = null;

    $scope.ChangeMarkerPosition = function (CurrentPosition) {
        $scope.CurrentPosition.latitude = CurrentPosition.latitude;
        $scope.CurrentPosition.longitude = CurrentPosition.longitude;

        $scope.map.center = angular.copy(CurrentPosition);
    };
});
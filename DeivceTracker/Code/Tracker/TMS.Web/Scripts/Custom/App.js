var LiveTracking = function (elementId, isMultiTracking) {
    if (typeof (isMultiTracking) === "undefined" || isMultiTracking == null) {
        isMultiTracking = false;
    }

    this.IsMultiTracking = isMultiTracking;
    this.DevicesNMArkers = [];
    this.AddDevice = function (DeviceId) {
        var dMarker = new CMarker(this.map, { lat: 10.8922578, lng: 77.9549099 }, $("#MarkerImage-Car").prop('src'));
        this.DevicesNMArkers.push({ DeviceId: DeviceId, Marker: dMarker });
    };

    this.ShowHideDevice = function (DeviceId) {
        var deviceNMs = this.DevicesNMArkers.filter(function (ele) {
            if (ele.DeviceId == DeviceId) {
                return ele;
            }
        });
        if (deviceNMs.length > 0) {
            var curVisibility = deviceNMs[0].Marker.getVisible();
            if (!curVisibility) { // Whenever visibility is set to true, change center
                this.map.setCenter(deviceNMs[0].Marker.getPosition());
            }
            deviceNMs[0].Marker.setVisible(!(curVisibility));
        }
    };
    this.ShowDevice = function (DeviceId) {
        var deviceNMs = this.DevicesNMArkers.filter(function (ele) {
            if (ele.DeviceId == DeviceId) {
                return ele;
            }
        });
        if (deviceNMs.length > 0) {
            deviceNMs[0].Marker.setVisible(true);
            this.SetCurrentDevice(DeviceId);
            this.map.setCenter(deviceNMs[0].Marker.getPosition());
        }
    };

    this.ShowAllDevicesTogether = function () {
        var bounds = new google.maps.LatLngBounds();

        for (var i = 0; i < this.DevicesNMArkers.length; i++) {
            this.ShowDevice(this.DevicesNMArkers[i].DeviceId);
            bounds.extend(this.DevicesNMArkers[i].Marker.getPosition());
        }
        this.map.fitBounds(bounds);
    };

    this.HideDevice = function (DeviceId) {
        var deviceNMs = this.DevicesNMArkers.filter(function (ele) {
            if (ele.DeviceId == DeviceId) {
                return ele;
            }
        });
        if (deviceNMs.length > 0) {
            deviceNMs[0].Marker.setVisible(false);
        }
    };

    this.SetCenter = function (DeviceId) {
        this.centerChanged = false;
        var deviceNMs = this.DevicesNMArkers.filter(function (ele) {
            if (ele.DeviceId == DeviceId) {
                return ele;
            }
        });
        console.log("deviceNMs...");
        console.log(deviceNMs);

        if (deviceNMs.length > 0) {
            this.map.setCenter(deviceNMs[0].Marker.getPosition());
        }
    };

    this.SetMarkerValues = function (objectArray) {

        if (Array.isArray(objectArray)) {

            for (var i = 0; i < objectArray.length; i++) {

                if (objectArray[i].Status == 200 && objectArray[i].Data != null) {

                    var deviceNMs = this.DevicesNMArkers.filter(function (ele) {
                        if (ele.DeviceId == objectArray[i].DeviceId) {
                            return ele;
                        }
                    });

                    if (deviceNMs.length > 0) {

                        deviceNMs[0].Marker.setPosition(
                            {
                                lat: Number(objectArray[i].Data.Latitude),
                                lng: Number(objectArray[i].Data.Longitude)
                            });
                        deviceNMs[0].Marker.rotate(objectArray[i].Data.Direction);
                        deviceNMs[0].Marker.ProcessTrackingDisplay(objectArray[i].Data);

                        if (deviceNMs[0].DeviceId == this.currentDeviceId) {
                            if (!this.centerChanged && !this.IsMultiTracking) {
                                this.map.setCenter(deviceNMs[0].Marker.getPosition());
                            }
                        }
                    }

                } else {
                    //console.log(objectArray[i].Status);
                    //console.log(objectArray[i].ErrorMessage);
                }
            }
        }
    };


    // Ajax actions starts
    this.SyncDevices = function () {
        var devicesList = this.DevicesNMArkers.map(function (ele) { return ele.DeviceId });

        var self = this;

        $.ajax({
            type: "GET",
            url: ApiUrl,
            data: { DevicesList: JSON.stringify(devicesList) },
            success: function (result, status, xhr) {
                self.SetMarkerValues(result);
            },
            error: function () {
                console.log("Ajax error");
            }
        });
    };
    // Ajax actions ends




    this.ToggleCenterScreen = function (setTo) {
        if (setTo == true) {
            this.SetCenter(this.currentDeviceId);
        }
    };




    /* Tracking Start */
    this.PolyLine = null;
    this.PolyPath = null;
    this.TrackingDeviceId = "";
    this.TrackingHistoryOn = false;
    this.SetDeviceTracking = function (isOn) {
        console.log('SetDeviceTracking ...' + isOn);
        this.TrackingHistoryOn = isOn;
        if (isOn == true) {
            this.PolyLine = new google.maps.Polyline({
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 3
            });
            this.PolyLine.setMap(this.map);
            this.PolyPath = this.PolyLine.getPath();

        } else {
            if (this.PolyLine) {
                this.PolyLine.setMap(null);
                this.PolyPath = null;
            }
            this.PolyLine = null;
        }
    };

    this.ProcessTrackingDisplay = function (currentDeviceData) {
        if (this.TrackingHistoryOn == true) {
            if (this.TrackingDeviceId != currentDeviceData.DeviceId) {
                this.SetDeviceTracking(false);
                this.SetDeviceTracking(true);
                this.TrackingDeviceId = currentDeviceData.DeviceId;
            }

            if (this.PolyLine != null) {
                var newPoint = new google.maps.LatLng(Number(currentDeviceData.Latitude),
                    Number(currentDeviceData.Longitude));
                this.PolyPath.push(newPoint);
            }
        }
    };
    /* Tracking End */





    //Map controls
    this.map = null;

    this.zoomLevelChanged = false;
    this.centerChanged = false;

    this.centerPosition = null;
    this.currentMarker = null;

    this.markers = [];


    this.bind = function (elementId) {
        this.centerPosition = { lat: 10.8922578, lng: 77.9549099 };

        this.map = new google.maps.Map(document.getElementById(elementId), {
            center: this.centerPosition,
            scrollwheel: false,
            zoom: 7
        });

        // Bind timer
        var self = this;

        this.map.addListener('drag', function () {
            _LiveTracking.centerChanged = true;
            $(document).trigger("OnCenterChanged");
        });

    };

    this.updateCenterPosition = function () {
        if (!this.IsMultiTracking) {
            this.map.setCenter(this.centerPosition);
        }
    };

    this.updateCurrentCenterPosition = function (lat, lng) {
        this.centerPosition = { lat: Number(lat), lng: Number(lng) };

        if (!this.centerChanged) {
            this.map.setCenter(this.centerPosition);
        }

        if (!this.zoomLevelChanged) {
            this.map.setZoom(10);
            this.zoomLevelChanged = true;
        }
        this.currentMarker.setPosition(new google.maps.LatLng(Number(lat), Number(lng)));
    };

    this.currentDeviceId = null;

    this.SetCurrentDevice = function (DeviceId) {
        this.currentDeviceId = DeviceId;
        this.centerChanged = false;
    };

    this.GetLiveStatus = function () {
        if (this.currentDeviceId != null) {
            var self = this;
            $.ajax({
                type: "GET",
                url: ApiUrl,
                data: { DeviceId: self.currentDeviceId },
                success: function (result, status, xhr) {
                    self.GetLiveStatusCallBack(result);
                },
                error: function () {
                    console.log("Ajax error");
                }
            });
        }
    };

    this.GetLiveStatusCallBack = function (result) {
        this.updateCurrentCenterPosition(result.Latitude, result.Longitude);
    };

    this.bind(elementId);

    this.setZoom = function (zoomLevel) {
        this.map.setZoom(zoomLevel);
    };

    var self = this;
    setInterval(function () {
        self.SyncDevices();
    }, 10000);
};















var _LiveTracking = null;
$(document).ready(function () {
    setScreenSize();
    //_LiveTracking = new LiveTracking("google-live-map");
});

$(window).resize(function () {
    setScreenSize();
    //_LiveTracking.updateCenterPosition();
});

function setScreenSize() {
    if (window.innerWidth < 768) {
        $("#live-map-container").css("width", (window.innerWidth - 40 + 30) + "px");
    } else {
        $("#live-map-container").css("width", (window.innerWidth - 80 + 30) + "px");
    }
    $("#live-map-container").css("height", (window.innerHeight - 105) + "px");
}

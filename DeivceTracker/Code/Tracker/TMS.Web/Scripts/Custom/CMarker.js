var CMarker = function (map, position, markerIconSrc) {
    this.map = null;
    this.marker = null;


    this.canvasEle = null;
    this.canvasCtx = null;
    this.image = null;
    this.genIcon = null;

    this.PolyPath = null;
    this.PolyLine = null;

    this.create = function (map, position, markerIconSrc) {
        this.map = map;

        this.marker = new google.maps.Marker({
            map: this.map,
            position: position
        });

        this.canvasEle = document.createElement("canvas");
        this.canvasCtx = this.canvasEle.getContext("2d");

        this.canvasEle.width = 48;
        this.canvasEle.height = 48;

        var self = this;
        this.image = document.createElement("img");
        this.image.onload = function () {
            self.canvasCtx.drawImage(self.image, self.canvasEle.width / 2 - self.image.width / 2, self.canvasEle.height / 2 - self.image.width / 2);
        };

        this.image.src = markerIconSrc;
        this.drawRotated(0);

        this.genIcon = {
            //size: new google.maps.Size(220, 220),
            //scaledSize: new google.maps.Size(32, 32),
            origin: new google.maps.Point(0, 0),
            url: this.canvasEle.toDataURL(),
            anchor: new google.maps.Point(24, 24)
        };
        this.marker.setIcon(this.genIcon);
        this.marker.setVisible(false);

        this.PolyLine = new google.maps.Polyline({
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 3
        });
        this.PolyPath = this.PolyLine.getPath();

        this.PolyLine.setMap(this.map);
    };

    this.setPosition = function (position) {
        this.marker.setPosition(position);
    };

    this.getVisible = function () {
        return this.marker.getVisible();
    };

    this.setVisible = function (booleanVal) {
        this.marker.setVisible(booleanVal);
    };

    this.getPosition = function () {
        return this.marker.getPosition();
    };

    this.rotate = function (degrees) {
        if (degrees && $.isNumeric(degrees)) {
        } else {
            degrees = 0;
        }
        this.drawRotated(degrees);

        this.genIcon.url = this.canvasEle.toDataURL();
        this.marker.setIcon(this.genIcon);
    };

    this.drawRotated = function (degrees) {
        this.canvasCtx.clearRect(0, 0, this.canvasEle.width, this.canvasEle.height);
        this.canvasCtx.save();
        this.canvasCtx.translate(this.canvasEle.width / 2, this.canvasEle.height / 2);
        this.canvasCtx.rotate(degrees * Math.PI / 180);
        this.canvasCtx.drawImage(this.image, -this.image.width / 2, -this.image.width / 2);
        this.canvasCtx.restore();
    };

    this.ProcessTrackingDisplay = function (currentDeviceData) {
        // On create 
        var newPoint = new google.maps.LatLng(Number(currentDeviceData.Latitude),
                    Number(currentDeviceData.Longitude));
        this.PolyPath.push(newPoint);
    };

    this.create(map, position, markerIconSrc);
};

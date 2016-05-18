var map = null,
    geolocation = null,
    my_position = null,
    transfer = null,
    walking = null,
    driving = null;

var localPosition = '广州';

$(function () {
    init();
});

function init() {
    map = new AMap.Map('container', {
        resizeEnable: true,
        mapStyle: 'fresh',
        level: 17
    });

    AMap.service(["AMap.Transfer"], function () {
        var transOptions = {
            map: map,
            city: localPosition,
            policy: AMap.TransferPolicy.LEAST_TIME,
            panel: 'result'
        };

        transfer = new AMap.Transfer(transOptions);

        sendMsgToWebView("success", "map init");
    });
}

function goPosition(position) {
    transfer.search(new AMap.LngLat(my_position.lng, my_position.lat), new AMap.LngLat(position[0], position[1]),
        function (status, result) {
            if (status === "error") {
                sendMsgToWebView('error', result.toString());
            }
            if (status === "no_data") {
                sendMsgToWebView('error', '查询不到结果');
            }
        });
}

function navigateToPosition(now_lng, now_lat, to_lng, to_lat) {
    convertCoordinateToAMapBase(new AMap.LngLat(now_lng, now_lat))
        .done(function (lnglat) {
            transfer.search(lnglat,
                new AMap.LngLat(to_lng, to_lat), function (status, result) {
                    if (status === "error") {
                        sendMsgToWebView('error', result.toString());
                    } else if (status === "no_data") {
                        sendMsgToWebView('error', '查询不到结果');
                    } else {
                        $("#result").addClass("scroll-left");
                        $("#container").addClass("width-70");
                    }
                });
        });
    
}

function sendMsgToWebView(msg_type, content) {
    window.external.notify(msg_type + '|' + content);
}

function setLocation(lng, lat) {
    position = new AMap.LngLat(lng, lat);

    AMap.convertFrom(position,
        "gps", function (status, lnglat) {
            my_position = lnglat.locations[0];
            map.setCenter(my_position);
            new AMap.Marker({
                icon: "http://webapi.amap.com/theme/v1.3/markers/n/mark_b.png",
                position: my_position,
                map: map
            })
        });
}

function convertCoordinateToAMapBase(position) {
    var def = $.Deferred();
    var my_position = null;
    AMap.convertFrom(position,
        "gps", function (status, lnglat) {
            if (status === "complete") {
                my_position = lnglat.locations[0];
                def.resolve(my_position);
            } else {
                def.reject(status);
            }
        });
    return def.promise();
}
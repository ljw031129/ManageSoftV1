/**     第一部分，动画暂停、继续的实现     */

/**
* Marker移动控件
* param {Map} map    地图对象
* param {Marker} marker Marker对象
* param {Array} path   移动的路径，以坐标数组表示
*/
var MarkerMovingControl = function (map, marker) {
    this._map = map;
    this._marker = marker;
    //初始化path为空
    this._path = [];
    this._currentIndex = 0;
    this._speed = 100;
    this._pageSize = 100;
    this._pageNum = 1;
    this._isEnd = false;
    this._isShift = false;
    this._shiftDistance = 80;
    this._isStopPoint = false;
    this._stopPointDistance = 200;
    //停留点辅助计算便令
    this._stopTimes = 0;
    this._stopStart = false;

}

/**
 * 移动marker，会从当前位置开始向前移动
 */
MarkerMovingControl.prototype.move = function () {
    if (!this._listenToStepend) {
        this._listenToStepend = AMap.event.addListener(this, 'stepend', function () {
            this.step();
        }, this);
    }
    this.step();
};

/**
 * 停止移动marker，由于控件会记录当前位置，所以相当于暂停
 */
MarkerMovingControl.prototype.stop = function () {
    this._marker.stopMove();
};

/**
 * 重新开始，会把marker移动到路径的起点然后开始移动
 */
MarkerMovingControl.prototype.restart = function () {
    this._path = [];
    this._speed = 100;
    this._pageSize = 100;
    this._pageNum = 1;
    this._isEnd = false;
    this.stop();
    mapObj.clearMap();
    // this.stop();
};

/**
 * 向前移动一步
 */
MarkerMovingControl.prototype.step = function () {
    if (this._path.length == 1 && !this._isEnd) {
        this.updateNextPageNum();
        load();
    } else {
        if (!this._listenToMoveend) {
            //点标记执行moveTo动画结束时触发事件
            this._listenToMoveend = AMap.event.addListener(this._marker, 'moveend', function () {

                this._path.splice(0, 1);
                //触发指定事件。
                AMap.event.trigger(this, 'stepend');
            }, this);
        }
        if (this._path.length == 1) {
            var d = dialog({
                title: '提示',
                content: '轨迹播放完毕！'
            });
            d.show();
        } else {
            // new AMap.LngLat(lngX, latY)
            //this._path[0]
            //element["GpsPlog"], element["GpsPlat"]
            //设置中心点
            mapObj.setZoomAndCenter(14, new AMap.LngLat(this._path[0].GpsPlog, this._path[0].GpsPlat));
            //添加停留点

            var distance = getFlatternDistance(parseFloat(this._path[0].GpsPlog), parseFloat(this._path[0].GpsPlat), parseFloat(this._path[1].GpsPlog), parseFloat(this._path[1].GpsPlat));

            //处理停留点
            if (!this._isStopPoint && distance > this._stopPointDistance) {
                if (this._stopStart) {
                    this.addStopMarker(this._path[0], distance,this._stopTimes);
                    this._stopStart = false;
                    this._stopTimes = 0;
                } else {
                   // this.addMarker(this._path[1], distance);
                }
            } else {
                this._stopStart = true;
                this._stopTimes += timeDis(this._path[0].ReceiveTime, this._path[1].ReceiveTime);

            }
            this.addMarker(this._path[1], distance);
            //移动到下一点
            this._marker.moveTo(new AMap.LngLat(this._path[1].GpsPlog, this._path[1].GpsPlat), this._speed);
        }
    }
};
//添加停留点Mark
MarkerMovingControl.prototype.addStopMarker = function (path, distance, time) {
    var stopMarker = new AMap.Marker({
        icon: "../../Content/ICON/b.ico",
        offset: new AMap.Pixel(-20, -10), //相对于基点的位置
        zIndex: 90,
        // shadow:'',
        position: new AMap.LngLat(path.GpsPlog, path.GpsPlat)
    });
    //实例化信息窗体
    var infoWindow = new AMap.InfoWindow({
        isCustom: true,  //使用自定义窗体          
        offset: new AMap.Pixel(16, -45)//-113, -140
    });

    stopMarker.setMap(mapObj);  //在地图上添加点
    // stopMarker.setTitle('我是地图中心点哦~<br>sss'); //设置鼠标划过点标记显示的文字提示
    //stopMarker.setContent("<b>body</b>");
    AMap.event.addListener(stopMarker, 'click', function () { //鼠标点击marker弹出自定义的信息窗体
        var infoBody = "<p>" + path.ReceiveTime + "</p><p>" + path.GpsPos + "</p><p>" + distance + "</p><p>停留时间" + time + "</p>";

        infoWindow.setContent(createInfoWindow('定位信息&nbsp;&nbsp;<span style="font-size:11px;color:#F00;">' + path.GpsPlog + ',' + path.GpsPlat + '</span>', infoBody));
        infoWindow.open(mapObj, stopMarker.getPosition());
    });
};

//添加经过点Marker
MarkerMovingControl.prototype.addMarker = function (path, distance) {
    var stopMarker = new AMap.Marker({
        icon: "../../Content/ICON/a.ico",
        offset: new AMap.Pixel(-8, -10), //相对于基点的位置
        zIndex: 90,
        // shadow:'',
        position: new AMap.LngLat(path.GpsPlog, path.GpsPlat)
    });
    //实例化信息窗体
    var infoWindow = new AMap.InfoWindow({
        isCustom: true,  //使用自定义窗体          
        offset: new AMap.Pixel(16, -45)//-113, -140
    });

    stopMarker.setMap(mapObj);  //在地图上添加点
    // stopMarker.setTitle('我是地图中心点哦~<br>sss'); //设置鼠标划过点标记显示的文字提示
    //stopMarker.setContent("<b>body</b>");
    AMap.event.addListener(stopMarker, 'click', function () { //鼠标点击marker弹出自定义的信息窗体
        var infoBody = "<p>" + path.ReceiveTime + "</p><p>" + path.GpsPos + "</p><p>" + distance + "</p>";

        infoWindow.setContent(createInfoWindow('定位信息&nbsp;&nbsp;<span style="font-size:11px;color:#F00;">' + path.GpsPlog + ',' + path.GpsPlat + '</span>', infoBody));
        infoWindow.open(mapObj, stopMarker.getPosition());
    });
};

//添加开始点Marker
MarkerMovingControl.prototype.addStartMarker = function (path, distance) {
    var stopMarker = new AMap.Marker({
        icon: "../../Content/ICON/a.ico",
        offset: new AMap.Pixel(-8, -10), //相对于基点的位置
        zIndex: 90,
        // shadow:'',
        position: new AMap.LngLat(path.GpsPlog, path.GpsPlat)
    });
    //实例化信息窗体
    var infoWindow = new AMap.InfoWindow({
        isCustom: true,  //使用自定义窗体          
        offset: new AMap.Pixel(16, -45)//-113, -140
    });

    stopMarker.setMap(mapObj);  //在地图上添加点
    // stopMarker.setTitle('我是地图中心点哦~<br>sss'); //设置鼠标划过点标记显示的文字提示
    //stopMarker.setContent("<b>body</b>");
    AMap.event.addListener(stopMarker, 'click', function () { //鼠标点击marker弹出自定义的信息窗体
        var infoBody = "<p>" + path.ReceiveTime + "</p><p>" + path.GpsPos + "</p><p>" + distance + "</p>";

        infoWindow.setContent(createInfoWindow('定位信息&nbsp;&nbsp;<span style="font-size:11px;color:#F00;">' + path.GpsPlog + ',' + path.GpsPlat + '</span>', infoBody));
        infoWindow.open(mapObj, stopMarker.getPosition());
    });
};
//添加结束点Marker
MarkerMovingControl.prototype.addEndMarker = function (path, distance) {
    var stopMarker = new AMap.Marker({
        icon: "../../Content/ICON/a.ico",
        offset: new AMap.Pixel(-8, -10), //相对于基点的位置
        zIndex: 90,
        // shadow:'',
        position: new AMap.LngLat(path.GpsPlog, path.GpsPlat)
    });
    //实例化信息窗体
    var infoWindow = new AMap.InfoWindow({
        isCustom: true,  //使用自定义窗体          
        offset: new AMap.Pixel(16, -45)//-113, -140
    });

    stopMarker.setMap(mapObj);  //在地图上添加点
    // stopMarker.setTitle('我是地图中心点哦~<br>sss'); //设置鼠标划过点标记显示的文字提示
    //stopMarker.setContent("<b>body</b>");
    AMap.event.addListener(stopMarker, 'click', function () { //鼠标点击marker弹出自定义的信息窗体
        var infoBody = "<p>" + path.ReceiveTime + "</p><p>" + path.GpsPos + "</p><p>" + distance + "</p>";

        infoWindow.setContent(createInfoWindow('定位信息&nbsp;&nbsp;<span style="font-size:11px;color:#F00;">' + path.GpsPlog + ',' + path.GpsPlat + '</span>', infoBody));
        infoWindow.open(mapObj, stopMarker.getPosition());
    });
};
//构建自定义信息窗体
function createInfoWindow(title, content) {
    var info = document.createElement("div");
    info.className = "info";

    //可以通过下面的方式修改自定义窗体的宽高
    //info.style.width = "400px";

    // 定义顶部标题
    var top = document.createElement("div");
    top.className = "info-top";
    var titleD = document.createElement("div");
    titleD.innerHTML = title;
    var closeX = document.createElement("img");
    closeX.src = "http://webapi.amap.com/images/close2.gif";
    closeX.onclick = closeInfoWindow;

    top.appendChild(titleD);
    top.appendChild(closeX);
    info.appendChild(top);

    // 定义中部内容
    var middle = document.createElement("div");
    middle.className = "info-middle";
    middle.style.backgroundColor = 'white';
    middle.innerHTML = content;
    info.appendChild(middle);

    // 定义底部内容
    var bottom = document.createElement("div");
    bottom.className = "info-bottom";
    bottom.style.position = 'relative';
    bottom.style.top = '0px';
    bottom.style.margin = '0 auto';
    var sharp = document.createElement("img");
    sharp.src = "http://webapi.amap.com/images/sharp.png";
    bottom.appendChild(sharp);
    info.appendChild(bottom);
    return info;
}
//关闭信息窗体
function closeInfoWindow() {
    mapObj.clearInfoWindow();
}

//编辑折线
MarkerMovingControl.prototype.addLine = function (arr) {
    //定义折线对象
    var polyline = new AMap.Polyline({
        map: mapObj,
        path: arr,
        strokeColor: "#F00", //线颜色
        strokeOpacity: 0.4, //线透明度
        strokeWeight: 3, //线宽
        strokeStyle: "dashed", //线样式
        strokeDasharray: [10, 5] //补充线样式
    });
    this._map.setFitView();
    //构造折线编辑对象，并开启折线的编辑状态
}

//调整播放速度
MarkerMovingControl.prototype.setSpeed = function (value) {
    this._speed = value;
};
//更新path数据
MarkerMovingControl.prototype.updatePath = function (value) {
    //连接两个数组
    this._path = this._path.concat(value);
};
//更新pageNum
MarkerMovingControl.prototype.updateNextPageNum = function () {
    this._pageNum = this._pageNum + 1;
};
//得到当前pageNum
MarkerMovingControl.prototype.getPageNum = function () {
    return this._pageNum;
};
//得到当前pageSize
MarkerMovingControl.prototype.getPageSize = function () {
    return this._pageSize;
};
//得到当前path
MarkerMovingControl.prototype.getPath = function () {
    return this._path;
};
//设置IsEnd
MarkerMovingControl.prototype.setIsEnd = function (value) {
    this._isEnd = value;
};
//得到当前IsEnd
MarkerMovingControl.prototype.getIsEnd = function () {
    return this._isEnd;
};
//设置标记起始点mark
MarkerMovingControl.prototype.setMarkPosition = function (element) {
    this._marker.setPosition(new AMap.LngLat(element.GpsPlog, element.GpsPlat));
};
//设置标记停留点标记
MarkerMovingControl.prototype.seStopMarker = function (postion) {
    this._marker.setPosition(postion);
};


//公用方法

var EARTH_RADIUS = 6378137.0;    //单位M
var PI = Math.PI;
function getRad(d) {
    return d * PI / 180.0;
}
/**
* approx distance between two points on earth ellipsoid
* param {Object} lat1
* param {Object} lng1
* param {Object} lat2
* param {Object} lng2
*/
function getFlatternDistance(lat1, lng1, lat2, lng2) {
    var f = getRad((lat1 + lat2) / 2);
    var g = getRad((lat1 - lat2) / 2);
    var l = getRad((lng1 - lng2) / 2);

    var sg = Math.sin(g);
    var sl = Math.sin(l);
    var sf = Math.sin(f);

    var s, c, w, r, d, h1, h2;
    var a = EARTH_RADIUS;
    var fl = 1 / 298.257;

    sg = sg * sg;
    sl = sl * sl;
    sf = sf * sf;

    s = sg * (1 - sl) + (1 - sf) * sl;
    c = (1 - sg) * (1 - sl) + sf * sl;

    w = Math.atan(Math.sqrt(s / c));
    r = Math.sqrt(s * c) / w;
    d = 2 * w * a;
    h1 = (3 * r - 1) / 2 / c;
    h2 = (3 * r + 1) / 2 / s;

    return d * (1 + fl * (h1 * sf * (1 - sg) - h2 * (1 - sf) * sg));
}
//Json格式时间差计算
function timeDis(timeStart, timeEnd) {  
    var dateStart = parseInt(timeStart.replace(/\D/igm, ""));
    var dateEnd = parseInt(timeEnd.replace(/\D/igm, ""));
    return (dateEnd - dateStart) / 60000;
}
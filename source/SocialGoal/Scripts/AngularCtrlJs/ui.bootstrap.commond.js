'use strict';

var angularApp = angular.module('ui.bootstrap.commond', ['ui.bootstrap', 'ngAnimate', 'mgcrea.ngStrap']);
angularApp.controller('TabsCmdCtrl', function ($scope, $window, $http) {
    $scope.tabs = [
      { title: '有线智能终端v2.0', content: 'Dynamic content 1' },
      { title: '无线智能终端v1.0', content: 'Dynamic content 2', disabled: true },
      { title: '无线智能终端v2.0', content: 'Dynamic content 3' }
    ];

    //指令参数
    $scope.CommondParameters = { AddressType: "00", IpOrDomain: "123.57.41.22:9001", WorkStatue: "00", TootalWorkTime: "00", WorkModel: "00", Timing: "0", PostionCount: "1", WorkTime: "2",IntervalTime1:"0" };
    $scope.IMEI="";
    // timing
    $scope.TimingOptions = [
     { label: '不改变', value: "0" },
     { label: '1:00', value: "1" },
     { label: '2:00', value: "2" },
    { label: '3:00', value: "3" },
    { label: '4:00', value: "4" },
    { label: '5:00', value: "5" },
    { label: '6:00', value: "6" },
    { label: '7:00', value: "7" },
    { label: '8:00', value: "8" },
    { label: '9:00', value: "9" },
    { label: '10:00', value: "10" },
    { label: '11:00', value: "11" },
    { label: '12:00', value: "12" },
    { label: '13:00', value: "13" },
    { label: '14:00', value: "14" },
    { label: '15:00', value: "15" },
    { label: '16:00', value: "16" },
    { label: '17:00', value: "17" },
    { label: '18:00', value: "18" },
    { label: '19:00', value: "19" },
    { label: '20:00', value: "20" },
    { label: '21:00', value: "21" },
    { label: '22:00', value: "22" },
    { label: '23:00', value: "23" },
    { label: '00:00', value: "24" }
    ];
    $scope.loadIMEI = function (data) {
        alert(data);
    }
    //提交指令
    $scope.setParameterSubmit = function (IMEI) {
        alert(IMEI);
        if ($scope.CommondParameters) {
            setTimeout(function () {
                // Simple POST request example (passing data) :
              
                $http.post('/TerminalEquipmentCommand/InsertSendData', { MsgJson: JSON.stringify($scope.CommondParameters) }).
                  success(function (data, status, headers, config) {
                      alert("success");
                  }).
                  error(function (data, status, headers, config) {
                      alert("error");
                  });

            });
        }
    };

});


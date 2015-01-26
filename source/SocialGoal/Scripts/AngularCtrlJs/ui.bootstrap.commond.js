'use strict';
var angularApp = angular.module('ui.bootstrap.commond', ['ui.bootstrap', 'ngAnimate', 'mgcrea.ngStrap']);
angularApp.controller('TabsCmdCtrl', function ($scope, $window) {
    $scope.tabs = [
      { title: '有线智能终端v2.0', content: 'Dynamic content 1' },
      { title: '无线智能终端v1.0', content: 'Dynamic content 2', disabled: true },
      { title: '无线智能终端v2.0', content: 'Dynamic content 3' }
    ];

    $scope.alertMe = function () {
        setTimeout(function () {
            $window.alert('You\'ve selected the alert tab!');
        });
    };

   

});


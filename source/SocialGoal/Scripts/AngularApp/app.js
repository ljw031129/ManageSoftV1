//'use strict';

var angularApp = angular.module('angularjsFormBuilderApp', ['ngAnimate', 'mgcrea.ngStrap', 'ngRoute']);
angularApp.config(function ($modalProvider) {
    angular.extend($modalProvider.defaults, {
        animation: 'am-flip-x'
    });
});
angularApp.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: './Scripts/AngularApp/views/main.html',
            controller: 'MainCtrl'
        })
        .when('/forms/create', {
            templateUrl: './Scripts/AngularApp/views/create.html',
            controller: 'CreateCtrl'
        })
        .when('/forms/:id/view', {
            templateUrl: './Scripts/AngularApp/views/view.html',
            controller: 'ViewCtrl'
        })
        .otherwise({
            redirectTo: '/'
        });

}).run(['$rootScope', function () { }]);



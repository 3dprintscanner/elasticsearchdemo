'use strict';

/**
 * @ngdoc overview
 * @name elasticsearchtestappApp
 * @description
 * # elasticsearchtestappApp
 *
 * Main module of the application.
 */
var app = angular
  .module('elasticsearchtestappApp', [
    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ngRoute',
    'ngSanitize',
    'ngTouch',
    'elasticsearch'
  ])
  .config(function ($routeProvider) {
      $routeProvider
        .when('/', {
            templateUrl: 'views/main.html',
            controller: 'MainCtrl',
            controllerAs: 'main'
        })
        .when('/about', {
            templateUrl: 'views/about.html',
            controller: 'AboutCtrl',
            controllerAs: 'about'
        })
        .otherwise({
            redirectTo: '/'
        });
  });

app.service('client', function (esFactory) {
    return esFactory({
        host: 'localhost:9200'
    });
});

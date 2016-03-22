'use strict';

/**
 * @ngdoc function
 * @name elasticsearchtestappApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the elasticsearchtestappApp
 */
angular.module('elasticsearchtestappApp')
  .controller('MainCtrl', ['$scope', 'client', 'esFactory', function ($scope, client, esFactory) {


      client.cluster.state({
          metric: [
              'cluster_name'

          ]
      })
    .then(function (resp) {
        $scope.clusterState = resp;
        $scope.error = null;
        console.log('client created successfully');
    })
    .catch(function (err) {
        $scope.clusterState = null;
        $scope.error = err;
        // if the err is a NoConnections error, then the client was not able to
        // connect to elasticsearch. In that case, create a more detailed error
        // message
        if (err instanceof esFactory.errors.NoConnections) {
            $scope.error = new Error('Unable to connect to elasticsearch. ' +
              'Make sure that it is running and listening at http://localhost:9200');
        }
    });

      $scope.update = function () {
          console.log(client);

          // query the client with the fields


          client.search({
              index: 'stationsv5',
              body: {
                  query: {
                      match: {
                          nLCDESC: $scope.destination
                      }
                  }
              }
          },function(err, resp) {
              var result = resp.hits.hits;
              $scope.result = result;
          });
          
      };


  }]);

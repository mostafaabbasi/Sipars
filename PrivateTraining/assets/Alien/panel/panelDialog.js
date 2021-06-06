~function () {
    'use strict';

    angular.module("App").directive('panelDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/panelDialog.html?v=' + window.App.version,
            scope: {
                location: '=',
                serviceList: '=',
                service: '=',
            },
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    function controller($scope, $transclude, $element, $http) {
        $scope.model = {}

        $scope.data = {}

        $scope.$element = $element

        console.log('panelScope', $scope)

        $scope.getRule = function () {

            //rules =>
            //admin provider customer (...)


        }

        $scope.getService = function () {

        }

        $scope.provider = {}
        $scope.provider.disagreeService = function (buyServiceCode) {

            const service = $scope.getService(buyServiceCode)

            //no checking
            //delete from list
            //

            // set status to 2 DeActive = 2,
        }

    }


}();

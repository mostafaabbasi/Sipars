~function () {
    'use strict';

    angular.module('App').directive('locationDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/dialog/locationDialog.html?v='+ window.App.version,
            scope: {
                location: '=',
                serviceList: '=',
                service: '=',
            },
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });

    function controller($scope, $transclude, $element, $http) {



        $scope.model = {
            search: ''
        }

        $scope.data = {
            cityList: [],
        }


        $scope.changeSearch = function () {
            const search = $scope.model.search
            if (!search) return $scope.model.cityList = $scope.data.cityList

            return $scope.model.cityList = $scope.data.cityList.filter(c => c.name.includes(search))
        }

        $scope.getCities = function () {
            $http.get('/v1/base/cityList').success(function (response) {
                if (response.result == 'done') {
                    $scope.model.cityList = $scope.data.cityList = response.items
                }
            })
        }

        // $scope.getCities()
    }

}();
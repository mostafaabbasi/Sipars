~function () {
    'use strict';

    angular.module("App").directive('confirmDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/confirmDialog.html?v=' + window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    function controller($scope, $transclude, $element, $http) {
        $scope.model = {
            text: '',
            cancelCallback: function () {
            },
            okCallback: function () {

            }
        }

        $scope.data = {}

        $scope.$element = $element

        $scope.$root.confirmDialogShow = function (option, okCallback, cancelCallback) {
            $scope.option = option

            $scope.model.okCallback = okCallback
            $scope.model.cancelCallback = cancelCallback

            window.openDialog('confirmDialog')

        }

        $scope.onCancelClick = function () {
            $('#confirmDialog').modal('hide')

            if(!$scope.model.cancelCallback) return
            $scope.model.cancelCallback($scope.model.text)
        }

        $scope.onOkClick = function () {
            $('#confirmDialog').modal('hide')

            if(!$scope.model.okCallback) return
            $scope.model.okCallback($scope.model.text)
        }
    }


}();

~function () {
    'use strict';

    angular.module("App").directive('addPaymentDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/addPaymentDialog.html?v=' + window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });

    function controller($scope, $transclude, $element, $http) {
        $scope.model = {}

        $scope.data = {}

        $scope.$element = $element

        $scope.$root.addPaymentDialogShow = function (buy, data, rowData) {
            $scope.model.buy = buy
            $scope.data = data
            $scope.rowData = rowData

            window.openDialog('addPaymentDialog')

        }

        $scope.adminPayment = function () {

            if (!$scope.model.price || $scope.model.price < 1000) return

            waitingDialog.show("درحال دریافت اطلاعات...");

            const price = $scope.model.price
            $http.post('/v1/Payment/AdminPayment', {
                price: price,
                userId: $scope.data.userId,
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        response.message && NotifyCustom(response.message, 'success');
                        // $scope.rowData[6] += price
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.bankPayment = function () {
            if (!$scope.model.price || $scope.model.price < 1000) return

            waitingDialog.show("درحال دریافت اطلاعات...");

            $http.post('/v1/Payment/AddBankPayment', {
                price: $scope.model.price,
                bankCode: 1,
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        // response.message && NotifyCustom(response.message, 'danger');
                        window.location.assign(response.url)
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });

        }

    }


}();

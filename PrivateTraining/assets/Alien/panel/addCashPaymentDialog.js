~function () {
    'use strict';

    angular.module("App").directive('addCashPaymentDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/addCashPaymentDialog.html?v=' + window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });

    function controller($scope, $transclude, $element, $http) {
        $scope.model = {}

        $scope.data = {}

        $scope.$element = $element

        $scope.$root.addCashPaymentDialogShow = function (buy, data, $listScope) {
            $scope.buy = buy
            $scope.data = data
            $scope.$listScope = $listScope

            window.openDialog('addCashPaymentDialogShow')

        }

        $scope.addCashPayment = function () {

            if (!$scope.model.price || $scope.model.price < 1000) return

            waitingDialog.show("درحال دریافت اطلاعات...");

            const price = $scope.model.price
            $http.post('/v1/Payment/CustomerCash', {
                price: price,
                buyServiceId: $scope.buy.buyService.id,
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        response.message && NotifyCustom(response.message, 'success');

                        $scope.data.paymentList.push.apply($scope.data.paymentList, [response.payment])
                        $scope.$listScope.updateBuyServiceList()

                        $('#addCashPaymentDialogShow').modal('hide')
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

    }


}();

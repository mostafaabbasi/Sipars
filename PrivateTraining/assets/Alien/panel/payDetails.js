~function () {
    'use strict';

    angular.module("App").directive('payDetails', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/payDetails.html?v=' + window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    function controller($scope, $transclude, $element, $http) {
        $scope.model = {
            buy: null,
            workPriceList: []
        }

        $scope.$element = $element

        $scope.data = {}
        $scope.$root.payDetailsShow = function (buy, data, $listScope) {


            $scope.model.buy = buy
            $scope.data = data
            $scope.$listScope = $listScope
            $scope.paymentList = buy.paymentList
            $scope.costTimeList = buy.costTimeList
            $scope.paymentRefType = $listScope.paymentRefType
            $scope.percentOfShares = buy.property.percentOfShares

            window.openDialog('payDetails')

        }

        $scope.totalDept = function () {
            let dept = 0
            $scope.paymentList.forEach(p => {
                const isDept = $scope.paymentRefType.customerCash != p.refType
                const percent = isDept ? $scope.percentOfShares : 100 - $scope.percentOfShares
                dept += p.price * percent / 100 * (isDept ? -1 : 1)
            })

            return (dept < 0 ? 'بدهی' : 'طلب') + ' ' + $scope.$root.getCurrency(Math.abs(dept))
        }

        $scope.dept = function (payment) {
            const isDept = $scope.paymentRefType.customerCash != payment.refType
            const percent = isDept ? $scope.percentOfShares : 100 - $scope.percentOfShares
            return (isDept ? 'بدهی' : 'طلب') + ' ' + $scope.$root.getCurrency(payment.price * percent / 100)
        }

        $scope.totalWorkPrice = function () {
            let pay = 0
            $scope.costTimeList.forEach(p => pay += p.priceReceived)
            return pay
        }


        $scope.totalPay = function () {
            let pay = 0
            $scope.paymentList.forEach(p => pay += p.price)
            return pay
        }


        $scope.finalPay = function () {
            $('#payDetails').modal('hide')
            $scope.$root.payDialogShow($scope.model.buy, $scope.data, $scope.$listScope, true, false);
        }

        $scope.getPayText = function () {
            return $scope.$root.getCurrencyHezar(window.Math.abs($scope.getPay()))
        }

        $scope.getPayHeader = function () {
            if ($scope.data.userType == 'provider') {
                return $scope.getPay() > 0 ? "طلب مشتری:" : "بدهی مشتری:"
            }
            return $scope.getPay() > 0 ? "طلب شما:" : "بدهی شما:"
        }

        $scope.getProviderSharePayText = function () {
            return $scope.$root.getCurrencyHezar(window.Math.abs($scope.getProviderSharePay()))
        }

        $scope.getProviderSharePay = function () {
            if ($scope.data.userType == 'provider') {
                let dept = 0
                $scope.paymentList.forEach(p => {
                    const isDept = $scope.paymentRefType.customerCash != p.refType
                    const percent = isDept ? $scope.percentOfShares : 100 - $scope.percentOfShares
                    dept += p.price * percent / 100 * (isDept ? -1 : 1)
                })
                return dept
            }
            return 0
        }

        $scope.getPay = function () {
            let cost = 0
            $scope.model.buy.costTimeList.forEach(p => cost += p.priceReceived)
            let pay = 0
            $scope.model.buy.paymentList.forEach(p => pay += p.price)
            return pay - cost
        }

    }


}();

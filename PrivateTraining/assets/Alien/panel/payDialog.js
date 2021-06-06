~function () {
    'use strict';

    angular.module("App").directive('payDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/payDialog.html?v=' + window.App.version,
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

        $scope.$root.payDialogShow = function (buy, data, $listScope, finalPay, isDetail) {

            $scope.model.buy = buy
            $scope.data = data
            $scope.$listScope = $listScope
            $scope.finalPay = finalPay
            $scope.isDetail = isDetail

            const customerUser = data.userList.find(u => u.id == buy.buyService.serviceReceiverId)
            $scope.model.customerUser = customerUser

            $scope.model.workPriceList = JSON.parse(buy.buyService.workPriceListJson || []).map(w => {
                const wu = data.workUnitList.find(wu => wu.id == w.workUnitId)
                w.title = wu.title
                w.price = wu.price + (buy.percent / 100 * wu.price)
                return w
            })

            $scope.model.customPricePay = $scope.getPrice() - $scope.$root.user.credit
            if ($scope.model.customPricePay < 0) $scope.model.customPricePay = 0

            // $http.post('/v1/BuyService/getPrice', {
            //     "id": buy.buyService.id,
            // }).success(function (response) {
            //
            //     if (response.result == "done") {
            //         NotifyCustom(response.message, 'success');
            //
            //     } else {
            //         NotifyCustom(response.message, 'danger');
            //     }
            // }).finally(function () {
            //     waitingDialog.hide();
            // });

            $scope.refresh(true)

        }

        $scope.getMinPrice = function () {
            const payMinPercent = $scope.model.buy.property.payMinPercent || 0
            const payMin = $scope.model.buy.property.payMin || 0

            let totalPrice = $scope.getTotalPrice()
            if (angular.isString(totalPrice)) totalPrice = 0

            let pay = Math.min(totalPrice * (payMinPercent / 100), payMin)

            return pay
        }

        $scope.getColor = function () {
            return $scope.getPrice() > $scope.$root.user.credit ? "#d73d32" : "#53a93f"
        }

        $scope.refresh = function (showDialog) {
            if ($scope.isDetail) {
                return window.openDialog('payDialog')
            }

            waitingDialog.show('درحال بارگذاری اطلاعات حساب...');
            $http.get('/v1/Account/isLogin', {})
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.$root.user = response.user
                        window.localStorage.user = JSON.stringify(response.user)

                        showDialog && window.openDialog('payDialog')

                    } else {
                        $scope.$root.user = {}
                        window.localStorage.user = ''
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.getTotalPrice = function () {
            let price = 0
            let unknown = false
            $scope.model.workPriceList.forEach(workPrice => {
                price += +workPrice.price * workPrice.workCount
                if (workPrice.meetingUnknown) unknown = true
            })

            if (unknown) {
                return "نامشخص"
            }

            return price
        }

        $scope.pay = function () {
            if($scope.getPrice() == 0) return

            let confirm = window.confirm("برای پرداخت اطمینان دارید؟")
            if (!confirm) return

            waitingDialog.show("درحال دریافت اطلاعات...");

            if (!$scope.getPrice()) return NotifyCustom("مبلغ پرداخت غیرمجاز است!", 'danger');

            $http.post('/v1/Payment/ServicePayment', {
                price: $scope.getPrice(),
                buyServiceId: $scope.model.buy.buyService.id,
                refType: $scope.finalPay ? $scope.$listScope.paymentRefType.finalPay : $scope.$listScope.paymentRefType.minPricePay
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        // response.message && NotifyCustom(response.message, 'danger');
                        NotifyCustom("پرداخت با موفقیت انجام شد.", 'success');

                        //update main list
                        Object.assign($scope.model.buy.buyService, response.buyService)
                        $scope.data.paymentList.push(response.payment)
                        $scope.$listScope.updateBuyServiceList()

                        $scope.$root.user = response.user
                        window.localStorage.user = JSON.stringify(response.user)

                        $('#payDialog').modal('hide')

                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });

        }

        $scope.addPayment = function (price) {
            if (price < 1000) return

            waitingDialog.show("درحال دریافت اطلاعات...");

            $http.post('/v1/Payment/AddBankPayment', {
                price: price,
                bankCode: 1, //zarin
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        // response.message && NotifyCustom(response.message, 'danger');
                        window.open(response.url, "_blank")
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });

        }

        $scope.totalBuyServicePayed = function (buy) {
            buy = buy || $scope.model.buy

            let tempReceive = buy.buyService.payed

            buy.costTimeList.forEach(ct => {
                tempReceive += ct.priceReceived
            })

            return tempReceive
        }

        $scope.totalWorkPrice = function () {
            let pay = 0
            $scope.model.buy.costTimeList.forEach(p => pay += p.priceReceived)
            return pay
        }

        $scope.totalPay = function () {
            let pay = 0
            $scope.model.buy.paymentList.forEach(p => pay += p.price)
            return pay
        }


        $scope.getPrice = function () {
            return $scope.finalPay ? ($scope.totalWorkPrice() - $scope.totalPay()) : $scope.getMinPrice()
        }

    }


}();

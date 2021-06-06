~function () {
    'use strict';

    angular.module("App").directive('buyServiceTimeCostListDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/buyServiceTimeCostListDialog.html?v=' + window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    function controller($scope, $transclude, $element, $http) {
        $scope.model = {}

        $scope.data = {}

        $scope.$element = $element

        $scope.timePattern = /^([01]\d|2[0-3]):?([0-5]\d)$/

        $scope.$root.buyServiceTimeCostListDialogShow = function (buy, data, $listScope) {
            $scope.model.title = buy.serviceTitle + ' کد: ' + buy.code

            $scope.model.userType = data.userType


            $scope.model.buy = buy


            $scope.model.costTimeList = buy.costTimeList.map(ct => {

                let dateRegister = 'زمان ثبت: '
                dateRegister += ct.dateRegister + ' ' + ct.timeRegister

                let date = 'زمان ارائه: '
                date += ct.date + ' از ساعت ' + ct.fromTime + ' تا ' + ct.toTime

                let priceReceived = 'مبلغ دریافت شده: '
                priceReceived += $scope.$root.getCurrency(ct.priceReceived)

                let nextDate = 'زمان ارائه بعدی: '
                nextDate += ct.nextDate + ' ' + ct.nextFromTime + ' تا ' + ct.nextToTime

                return {
                    dateRegister,
                    date,
                    priceReceived,
                    next: ct.next,
                    nextDate,
                    costTime: ct
                }
            })

            window.openDialog('buyServiceTimeCostListDialog')

        }

        $scope.getStatusText = function (ct) {

            if (ct.costTime.status == 0) {
                return {
                    customer: "لطفا در خصوص زمان و مبلغ اعلام نظر نمایید",
                    provider: "در انتظار تایید",
                }
            }

            if (ct.costTime.status == 1) {
                return {
                    customer: "زمان و مبلغ تایید شد",
                    provider: "زمان و مبلغ تایید شد",
                }
            }

            if (ct.costTime.status == 2) {
                return {
                    customer: ct.costTime.description || '«دلیل اعتراض وارد نشده است»',
                    provider: ct.costTime.description || '«دلیل اعتراض وارد نشده است»',
                }
            }

        }

        $scope.acceptTimeCost = function (ct, accept) {

            if (accept) return $scope.changeStatus(ct, accept)

            $scope.$root.confirmDialogShow({
                header: 'ثبت اعتراض',
                title: 'دلیل اعتراض خود را ثبت نمایید',
                text: 'دلایل...',
                ok: 'ثبت',
                cancel: 'انصراف',
            }, function (text) {
                return $scope.changeStatus(ct, accept, text)
            })
        }

        $scope.changeStatus = function (ct, accept, description) {

            let m = accept ? 'تایید شد' : 'اعتراض ثبت گردید'
            waitingDialog.show(m);

            $http.post('/v1/BuyService/ChangeStatusTimeCost', {
                "id": ct.costTime.id,
                "status": accept ? 1 : 2,
                "description": description
            }).success(function (response) {

                if (response.result == "done") {
                    NotifyCustom(response.message, 'success');
                    ct.costTime.status = accept ? 1 : 2
                } else {
                    NotifyCustom(response.message, 'danger');
                }
            }).finally(function () {
                waitingDialog.hide();
            });

        }


    }


}();

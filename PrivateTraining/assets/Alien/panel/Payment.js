~function () {
    'use strict';

    angular.module("App").directive('buyServiceTimeCostDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/buyServiceTimeCostDialog.html?v=' + window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    window.enforceMinMax = function (el) {
        if (el.value != "") {
            if (parseInt(el.value) < parseInt(el.min)) {
                el.value = el.min;
            }
            if (parseInt(el.value) > parseInt(el.max)) {
                el.value = el.max;
            }
        }
    }

    function controller($scope, $transclude, $element, $http) {
        $scope.model = {}

        $scope.data = {}

        $scope.timePattern = /^([01]\d|2[0-3]):?([0-5]\d)$/

        $scope.$root.buyServiceTimeCostShow = function (buy, data, $listScope) {
            $scope.model.title = buy.serviceTitle + ' کد: ' + buy.code

            $scope.data.data = data

            $scope.model.buy = buy

            window.openDialog('buyServiceTimeCostDialog')

            $scope.setInitTime()
        }

        $scope.timePattern = /^([01]\d|2[0-3]):?([0-5]\d)$/
        $scope.getTime = function () {
            function getNum(n) {
                n = n + ''
                if (n.length > 1) return n.substr(0, 2)
                return '0' + n
            }

            const fromTime = getNum($scope.model.fromTimeHour) + ':' + getNum($scope.model.fromTimeMin)
            const toTime = getNum($scope.model.toTimeHour) + ':' + getNum($scope.model.toTimeMin)
            const nextToTime = getNum($scope.model.nextFromTimeHour) + ':' + getNum($scope.model.nextFromTimeMin)
            const nextFromTime = getNum($scope.model.nextToTimeHour) + ':' + getNum($scope.model.nextToTimeMin)

            return {
                fromTime: fromTime,
                toTime: toTime,
                nextToTime: nextToTime,
                nextFromTime: nextFromTime,
            }
        }

        $scope.checkTimes = function () {
            const times = $scope.getTime()
            const fromTime = times.fromTime.match($scope.timePattern)
            const toTime = times.toTime.match($scope.timePattern)
            const nextToTime = times.toTime.match($scope.timePattern)
            const nextFromTime = times.toTime.match($scope.timePattern)

            return {
                fromTime: !fromTime,
                toTime: !toTime,
                nextToTime: !nextToTime,
                nextFromTime: !nextFromTime,
            }
        }

        $scope.save = function () {

            const input = {
                "buyServiceId": $scope.model.buy.buyService.id,
                "date": $scope.model.date,
                "fromTime": $scope.model.fromTime,
                "toTime": $scope.model.toTime,
                "priceReceived": $scope.model.priceReceived,
                "notFinished": $scope.model.notFinished,
                "next": $scope.model.next,
            }

            if (input.next) {
                input["nextDate"] = $scope.model.nextDate
                input["nextFromTime"] = $scope.model.nextFromTime
                input["nextToTime"] = $scope.model.nextToTime
            }

            $http.post('/v1/BuyService/AddCostTime', input).success(function (response) {

                if (response.result == "done") {
                    NotifyCustom("خدمت ارائه شده ثبت شد.", 'success');

                    // response.buyServiceCostTime
                    $scope.model.buy = null

                    $scope.data.data.costTimeList.push.apply($scope.data.data.costTimeList, response.costTimeList)
                    $scope.updateBuyServiceList()

                    $('#buyServiceTimeCostDialog').modal('hide')
                } else {
                    NotifyCustom(response.message, 'danger');
                }
            }).finally(function () {
                waitingDialog.hide();
            });

        }

        let _inited = false
        $scope.setInitTime = function () {

            if (_inited) return

            _inited = true
            $scope.model.date = new Date().toLocaleDateString('fa-IR', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                formatMatcher: 'basic',
            })

            $scope.model.date = window.toEnglishDigits($scope.model.date)

            $scope.model.nextDate = new Date().toLocaleDateString('fa-IR', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                formatMatcher: 'basic',
            })
            $scope.model.nextDate = window.toEnglishDigits($scope.model.nextDate)

            const myDatePicker = $("#datePicker1").pDatepicker({
                inline: true,
                autoClose: false,
                navigator: {enabled: 1, scroll: {enabled: 0},},
                'dayPicker': {
                    autoClose: false,
                },
                'observer': false,
                'toolbox': {
                    'enabled': true,
                    calendarSwitch: {
                        enabled: false,
                    }
                },
                'onSelect': function onSelect(selectedDayUnix) {
                    $scope.model.date = new Date(selectedDayUnix).toLocaleDateString('fa-IR', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        formatMatcher: 'basic',
                    })

                    $scope.model.date = window.toEnglishDigits($scope.model.date)
                    $scope.$apply()
                    // $scope.model.date = window.toEnglishDigits(new persianDate(selectedDayUnix).format('YYYY/MM/DD'))
                },
            });

            const myDatePicker2 = $("#datePicker2").pDatepicker({
                inline: true,
                autoClose: false,
                navigator: {enabled: 1, scroll: {enabled: 0},},
                'dayPicker': {
                    autoClose: false,
                },
                'observer': false,
                'toolbox': {
                    'enabled': true,
                    calendarSwitch: {
                        enabled: false,
                    }
                },
                'onSelect': function onSelect(selectedDayUnix) {
                    $scope.model.nextDate = new Date(selectedDayUnix).toLocaleDateString('fa-IR', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        formatMatcher: 'basic',
                    })

                    $scope.model.nextDate = window.toEnglishDigits($scope.model.nextDate)
                    $scope.$apply()
                },
            });

            myDatePicker.setDate(new Date().getTime());
            myDatePicker2.setDate(new Date().getTime());

        }

    }


}();

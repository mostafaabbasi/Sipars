~function () {
    'use strict';

    angular.module('App').directive('serviceBuyListDirective', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/serviceBuyListDirective.html?v=' + window.App.version,
            scope: {
                serviceBuyStatus: '=',
            },
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });

    // public static class PaymentTypeEnum
    // {
    //     public static int bank = 1;
    //     public static int admin = 2;
    //     public static int buy = 3;
    //     public static int customerFinishPay = 4;
    //     public static int customerCash = 5;
    // }

    const paymentRefType = {
        bank: 1,
        admin: 2,
        minPricePay: 3,
        customerFinishPay: 4,
        customerCash: 5,
    }

    const BuyServiceStatus = {
        pending: 0,
        accept: 1,
        certain: 2,
        unfinished: 3,
        final: 4,
        checking: 5,
        unCertain: 6,
        rejected: 7,
        canceled: 8,
        deleted: 9,
        doing: 10,
        finish: 12,

    }

    function controller($scope, $transclude, $element, $http) {

        $scope.$element = $element

        window.$scopeBuyList = $scope
        //status
        // [Description("موافق ")]
        // Accept = 1,
        //
        //     [Description("قطعی ")]
        // certain = 2,
        //
        //     [Description("ناتمام ")]
        // Unfinished = 3,
        //
        //     [Description("اتمام ")]
        // final = 4,
        //
        //     [Description("در حال بررسی ")]
        // checking = 0,
        //
        //     [Description("غیر قطعی ")]
        // UnCertain = 6

        // [Description("مخالفت شده ")]
        // Rejected = 7,
        //
        //     [Description("مخالفت شده ")]
        // Canceled = 8,

        // buyServiceId: -1766711586
        // userCityId: 2
        // userCityTitle: "مشهد"
        // userAddress: "آدرس تست"
        // userMobile: "09393013397"
        // userDescription: ""
        // providerServiceLocationStatus: "none"
        // attachmentPath: ""
        // providerType: "providerSelectCustomer"
        // workPriceListJson: "[{"workUnitId":"4","workCount":-1}]"
        // date: null
        // time: null
        // dateTimeSyncByProvider: false
        // id: 10108
        // isEnable: true
        // serviceLocationId: 8875
        // serviceReceiverId: 1087
        // serviceProviderId: 1087
        // dateRegister: "1398/11/29"
        // dateAcceptStatus: null
        // timeAcceptStatus: null
        // dateCertainStatus: null
        // timeCertainStatus: null
        // whoChangeStatus: 1087
        // workUnitId: null
        // status: 0
        // calcPrice: 0
        // calcPriceReceived: 0
        // typeProblem: 0
        // reasonProblemByUserId: null
        // reasonProblem: null
        // dateProblem: null
        // timeProblem: null
        //

        $scope.model = {
            takeNumber: 6,
            enableLoadMore: false,
            loading: {
                list: false
            },
            buyServiceList: [],
        }

        $scope.data = {
            buyServiceList: [],
            workUnitList: [],
            userList: [],
            serviceList: [],
            serviceLevelList: [],
            servicePropertyList: [],
            costTimeList: [],
            paymentList: [],
            chatCountCustomer: {},
            chatCountProvider: {},

            userType: '', // provider, customer, admin
            totalServiceBuyCount: -1
        }


        $scope.BuyServiceStatus = BuyServiceStatus
        $scope.paymentRefType = paymentRefType


        $scope.getServiceBuyList = function (refresh) {

            const input = {
                takeNumber: $scope.model.takeNumber,
                serviceBuyStatus: -1, //all
                skipNumber: $scope.data.buyServiceList.length,
            }

            if (refresh) {
                input.skipNumber = 0
                input.takeNumber = $scope.data.buyServiceList.length

                $scope.model = {
                    takeNumber: 6,
                    enableLoadMore: false,
                    loading: {
                        list: false
                    },
                    buyServiceList: [],
                    lastServiceId: $scope.model.lastServiceId
                }

                $scope.data = {
                    buyServiceList: [],
                    workUnitList: [],
                    userList: [],
                    serviceList: $scope.data.serviceList,
                    serviceLevelList: [],
                    servicePropertyList: [],
                    costTimeList: [],
                    paymentList: [],
                    chatCountCustomer: {},
                    chatCountProvider: {},

                    userType: '', // provider, customer, admin
                    totalServiceBuyCount: -1
                }
            }

            waitingDialog.show('درحال دریافت اطلاعات...');
            $scope.model.loading.list = true
            $http.post('/v1/BuyService/GetList', input).success(function (response) {
                if (response.result == "done") {
                    $scope.data.buyServiceList.push.apply($scope.data.buyServiceList,
                        response.buyServiceList.filter(s => s.serviceId && s.serviceId != -1 && s.locationId && s.locationId != -1))
                    $scope.data.userList.push.apply($scope.data.userList, response.userList)
                    $scope.data.workUnitList.push.apply($scope.data.workUnitList, response.workUnitList)
                    $scope.data.serviceLevelList.push.apply($scope.data.serviceLevelList, response.serviceLevelList)

                    // response.servicePropertyList.forEach(s => s.payOnline = false)

                    $scope.data.servicePropertyList.push.apply($scope.data.servicePropertyList, response.servicePropertyList)
                    $scope.data.costTimeList.push.apply($scope.data.costTimeList, response.costTimeList)
                    $scope.data.paymentList.push.apply($scope.data.paymentList, response.paymentList)

                    angular.forEach(response.chatCountCustomer, (value, key) => {
                        $scope.data.chatCountCustomer[key] = value
                    })

                    angular.forEach(response.chatCountProvider, (value, key) => {
                        $scope.data.chatCountProvider[key] = value
                    })

                    $scope.data.totalServiceBuyCount = response.count
                    $scope.data.userType = response.userType

                    $scope.model.enableLoadMore = $scope.data.buyServiceList.length != $scope.data.totalServiceBuyCount

                    $scope.updateBuyServiceList()
                }


            }).finally(function () {
                $scope.model.loading.list = false
                waitingDialog.hide();
            });
        }

        $scope.loadMoreList = function () {
            $scope.getServiceBuyList()
        }

        function getText(value) {
            if (value === null || value === undefined) return ''
            return value
        }

        $scope.updateBuyServiceList = function () {

            $scope.model.buyServiceList = $scope.data.buyServiceList.map((buyService, index) => {

                const buyServiceReference = buyService
                buyService = angular.copy(buyService)
                const service = $scope.data.serviceList.find(s => s.id == buyService.serviceId)

                let dateTime = 'زمان شروع: ' + (buyService.date || '') + ' - ' + (buyService.time || '').substr(0, 5)
                    + ' ' + (buyService.dateTimeSyncByProvider ? 'با خدمتیار هماهنگ شود' : '')

                let dateTimeRegister = 'زمان ثبت: ' + (buyService.dateRegister || '') + ' - ' + (buyService.timeRegister || '').substr(0, 5)


                const customerUser = $scope.data.userList.find(u => u.id == buyService.serviceReceiverId)
                const providerUser = $scope.data.userList.find(u => u.id == buyService.serviceProviderId)

                let customerName = customerUser.name + ' ' + customerUser.family
                let providerName = providerUser.name + ' ' + providerUser.family
                let providerPictureUrl = providerUser.path + '/' + providerUser.picture

                let customerMobile = 'تلفن همراه: ' + getText(customerUser.mobile)
                let providerMobile = 'تلفن همراه: ' + getText(providerUser.mobile)
                let customerEmail = 'رایانامه: ' + getText(customerUser.email)
                let providerEmail = 'رایانامه: ' + getText(providerUser.email)

                let percent = 0
                let serviceLevelTitle = ''

                let serviceLevel = providerUser.serviceLevelList.find(sl => sl.serviceId == service.id)
                if (serviceLevel) {
                    serviceLevel = $scope.data.serviceLevelList.find(sl => sl.id == serviceLevel.serviceLevelListId)
                    if (serviceLevel) {
                        percent = serviceLevel.percent
                        serviceLevelTitle = serviceLevel.title
                    }
                }


                const workPriceList = JSON.parse(buyService.workPriceListJson || '[]')
                let price = ''
                workPriceList.forEach((workPrice, index) => {
                    if (index > 0) price += '\n'

                    price += 'تعرفه: '
                    const wId = workPrice.workUnitId
                    const workUnit = $scope.data.workUnitList.find(w => w.id == wId)
                    if (workUnit) {
                        price += workUnit.title

                        let pToman = workUnit.price
                        pToman = pToman + (percent / 100 * pToman)

                        price += " " + $scope.$root.getCurrency(pToman)

                        if (workPrice.workCount == -1) {
                            price += " (نامشخص) "
                        } else {
                            price += ' (' + workPrice.workCount + " واحد) "
                        }
                    }
                })

                let address = (buyService.userCityTitle || '') + ' - ' + (buyService.userAddress || '')

                let providerStatusText = ''
                let customerStatusText = ''
                if (buyService.status == 0) {
                    providerStatusText = 'برای انجام این سفارش توسط مشتری انتخاب شده اید سریعا نظرتان را اعلام کنید'
                    customerStatusText = 'درحال بررسی توسط خدمتیار'
                }
                if (buyService.status == 1) {
                    providerStatusText = 'بلافاصله با مشتری تماس بگیرید و قطعی یا غیرقطعی را مشخص نمایید'
                    customerStatusText = 'درحال بررسی توسط خدمتیار'
                }

                let property = $scope.data.servicePropertyList.find(sp => sp.id == service.id)
                let costTimeList = $scope.data.costTimeList.filter(ct => ct.buyServiceId == buyService.id)
                costTimeList.sort((ct1, ct2) => {
                    // const t1 = ct1.nextDate + ' ' + ct1.nextTime + ' ' + ct1.date + ' ' + ct1.time
                    // const t2 = ct2.nextDate + ' ' + ct2.nextTime + ' ' + ct2.date + ' ' + ct2.time

                    const t1 = ct1.date + ' ' + ct1.time
                    const t2 = ct2.date + ' ' + ct2.time

                    if (t1 > t2) return -1
                    if (t1 < t2) return 1
                    return 0
                })

                console.log(costTimeList)
                const firstCostTime = costTimeList[0]
                let nextTimeDate = ''
                if (firstCostTime && firstCostTime.next) {
                    nextTimeDate = 'ارائه جدید: ' + (firstCostTime.date || '') + ' - ' + (firstCostTime.fromTime || '').substr(0, 5)
                }

                const paymentList = $scope.data.paymentList.filter(p => p.refId == buyService.id)

                const showNewTag = $scope.model.lastServiceId && $scope.data.buyServiceList[0].id !== $scope.model.lastServiceId
                $scope.model.lastServiceId = $scope.data.buyServiceList[0].id

                let chatCount = 0
                if ($scope.data.userType == 'provider') {
                    chatCount = $scope.data.chatCountCustomer[buyService.id] - buyService.chatReadProvider
                }

                if ($scope.data.userType == 'customer') {
                    chatCount = $scope.data.chatCountProvider[buyService.id] - buyService.chatReadCustomer
                }

                return {
                    code: buyService.id,
                    buyService: buyServiceReference,
                    serviceTitle: $scope.$root.getServiceTitle(service),
                    dateTime: dateTime,
                    dateTimeRegister: dateTimeRegister,
                    price: price,
                    percent: percent,
                    customerName: customerName,
                    providerName: providerName,
                    providerPictureUrl: providerPictureUrl,
                    address: address,
                    providerStatusText: providerStatusText,
                    customerStatusText: customerStatusText,

                    serviceLevelTitle: serviceLevelTitle,

                    customerMobile: customerMobile,
                    customerEmail: customerEmail,

                    providerMobile,
                    providerEmail,

                    property,
                    costTimeList,

                    nextTimeDate,

                    paymentList,

                    showNewTag,

                    chatCount

                }
            })
        }

        $scope.init = function () {


            $http.get(window.App.Url.base + 'privateTraining/MenuList', {}).success(function (response) {
                if (response.result == "done") {
                    $scope.$root.model.menuList = $scope.data.serviceList = response.items
                    $scope.getServiceBuyList()
                }
            });
        }

        $scope.getStatusColor = function (buyService) {

            const errorColor = '#d73d32'
            const successColor = '#53a93f'
            const defaultColor = '#ffee10'

            switch (buyService.status) {

                case BuyServiceStatus.finish:
                case BuyServiceStatus.certain:
                case BuyServiceStatus.final:
                    return successColor

                case BuyServiceStatus.deleted:
                    return errorColor
            }

            return defaultColor
        }

        $scope.getStatusText = function (buyService) {

            let providerStatusText = ''
            let customerStatusText = ''
            if (buyService.status == 0) {
                providerStatusText = 'برای انجام این سفارش توسط مشتری انتخاب شده اید سریعا نظرتان را اعلام کنید'
                customerStatusText = 'درحال بررسی توسط خدمتیار'
            }
            if (buyService.status == 1) {
                providerStatusText = 'بلافاصله با مشتری تماس بگیرید و قطعی یا غیرقطعی را مشخص نمایید'
                customerStatusText = 'خدمتیار با سفارش شما موافقت کرده است و به زودی با شما تماس می گیرد'
            }

            if (buyService.status == 2) {
                providerStatusText = 'سفارش «قطعی» می باشد'

                const statusChangeList = JSON.parse(buyService.statusChangeJson || '[]')
                const lastStatus = statusChangeList.slice(-1)[0] || {
                    date: buyService.dateRegister,
                    time: buyService.timeRegister
                }

                customerStatusText = 'سفارش «قطعی» می باشد'
                providerStatusText = customerStatusText

                if (lastStatus) {
                    customerStatusText = 'سفارش شما در تاریخ '
                    customerStatusText += lastStatus.date
                    customerStatusText += ' و ساعت '
                    customerStatusText += lastStatus.time.substr(0, 5)
                    customerStatusText += " قطعی شده و نیاز به پرداخت آنلاین دارد. حداکثر تا یک ساعت بعد از این زمان باید پرداخت انجام شود در غیر این صورت سفارش لغو خواهد شد!"

                    providerStatusText = 'شما در تاریخ '
                    providerStatusText += lastStatus.date
                    providerStatusText += ' و ساعت '
                    providerStatusText += lastStatus.time.substr(0, 5)
                    providerStatusText += " سفارش را قطعی کرده اید. مشتری تا یک ساعت آینده باید پرداخت را انجام دهد در غیر این صورت سفارش لغو خواهد شد."
                }

            }

            if (buyService.status == 6) {
                providerStatusText = 'سفارش «غیرقطعی» می باشد'
                customerStatusText = 'با عرض پوزش خدمتیار مورد نظر آمادگی انجام سفارش شما را ندارد.'
            }

            if (buyService.status == 3) {
                providerStatusText = 'سفارش «درحال انجام» می باشد'
                customerStatusText = 'سفارش «درحال انجام» می باشد'
            }

            if (buyService.status == 4) {
                providerStatusText = 'سفارش «تمام شده» می باشد'
                customerStatusText = 'سفارش «تمام شده» می باشد'
            }

            //--

            if (buyService.status == 7) {
                providerStatusText = 'سفارش «مخالفت شده» می باشد'
                customerStatusText = 'خدمتیار انتخابی، سفارش را تایید نکرده است. می توانید خدمتیار ' +
                    'دیگری انتخاب کنید و یا جهت سهولت انتخاب را به سی پارس بسپارید'
            }

            if (buyService.status == BuyServiceStatus.canceled) {
                providerStatusText = 'سفارش «لغو شده» می باشد'
                customerStatusText = 'سفارش «لغو شده» می باشد'
            }

            if (buyService.status == BuyServiceStatus.doing) {
                providerStatusText = 'سفارش «درحال انجام» می باشد'
                customerStatusText = 'سفارش «درحال انجام» می باشد'
            }

            if (buyService.status == BuyServiceStatus.deleted) {
                providerStatusText = 'سفارش «حذف شده» می باشد'
                customerStatusText = 'سفارش «حذف شده» می باشد'
            }

            if (buyService.status == BuyServiceStatus.finish) {
                providerStatusText = 'خدمتیار گرامی ضمن تشکر از جناب عالی بابت انجام صحیح سفارش در صورتی که مشتری مستقیم با شما تماس بگیرد ضروری است با فعال سازی خدمت آن را ثبت نمایید.'
                customerStatusText = 'لطفا هزینه را کامل پرداخت نمایید.'

                const buy = $scope.model.buyServiceList.find(b => b.buyService === buyService)
                if (buy && $scope.isBuyPayed(buy)) {
                    customerStatusText = 'خدمت شما به پایان یافت. لطفا امتیاز دهید و نظرتان را اعلام کنید.\n قدردان انتخاب شما هستیم. چنانچه این سفارش را بار دیگر نیاز دارید تمدید سفارش را انتخاب نمایید.'
                }
            }

            return {providerStatusText, customerStatusText}
        }


        $scope.updateBuyService = function (buyService) {
            return setTimeout(() => {
                $scope.getServiceBuyList(true)
            })

            const old = $scope.data.buyServiceList.find(b => b.id == buyService.id)

            if (!old) return

            Object.assign(old, buyService)
            $scope.updateBuyServiceList()
        }

        $scope.removeBuyService = function (buyService) {
            let index = $scope.data.buyServiceList.indexOf(buyService)
            if (index == -1) return

            $scope.data.buyServiceList.splice(index, 1)

            index = $scope.model.buyServiceList.findIndex(buy => buy.buyService == buyService)

            if (index == -1) return
            $scope.model.buyServiceList.splice(index, 1)
        }

        $scope.changeBuyServiceStatus = function (buy, reject) {

            const buyService = buy.buyService
            const id = buyService.id
            let status = 0

            if (buyService.status == BuyServiceStatus.pending) {

                //only provider can do it
                status = reject ? BuyServiceStatus.rejected : BuyServiceStatus.accept
            } else if (buyService.status == BuyServiceStatus.accept) {

                //only provider can do it
                status = reject ? BuyServiceStatus.unCertain : BuyServiceStatus.certain

                if (reject) return $scope.provider.unCertainBuyService(buy)

                if (!reject) {

                    // no need to pay
                    if (!$scope.needMinPay(buy)) {

                        status = BuyServiceStatus.doing
                    } else {

                        $scope.checkMinPayment(buy)
                    }
                }

            } else if (buyService.status == BuyServiceStatus.certain) {

                //customer pay
                //
                if (!reject)
                    status = BuyServiceStatus.doing
            } else if (buyService.status == BuyServiceStatus.doing) {
                status = BuyServiceStatus.finish
            } else if (buyService.status == BuyServiceStatus.finish) {
                status = BuyServiceStatus.doing
            }


            waitingDialog.show('درحال انجام عملیات...');

            return $http.post('/v1/BuyService/ChangeStatus', {
                "id": id,
                "status": status
            }).success(function (response) {

                if (response.result == "done") {
                    NotifyCustom(response.message, 'success');
                    Object.assign(buy.buyService, response.buyService)

                    // if (status == 2) {
                    //     $scope.removeBuyService(buyService)
                    // }
                } else {
                    NotifyCustom(response.message, 'danger');
                }
            }).finally(function () {
                waitingDialog.hide();
            });
        }


        // $scope.changeBuyServiceStatus2 = function (buyService, reject) {
        //
        //     const id = buyService.id
        //     let status = 0
        //     if (buyService.status == 0) {
        //
        //         if (reject) {
        //             let con = confirm('برای لغو سفارش اطمینان دارید؟')
        //             if (!con) return
        //
        //             //delete it
        //             waitingDialog.show('درحال انجام عملیات...');
        //             $http.post('/PrivateTrain/ServiceReceiverServiceLocation/RefrenceServiceReceiverServiceLocations', {"Id": id}).success(function (response) {
        //
        //                 if (response.Resualt) {
        //                     NotifyCustom(response.Messages, 'success');
        //                     $scope.removeBuyService(buyService)
        //                 } else {
        //                     NotifyCustom(response.Messages, 'danger');
        //                 }
        //             }).finally(function () {
        //                 waitingDialog.hide();
        //             });
        //
        //             return
        //         }
        //
        //         status = 1
        //     } else {
        //         // if (buyService.status == 1) {
        //         //
        //         // }
        //
        //         status = reject ? 6 : 2
        //     }
        //
        //
        //     waitingDialog.show('درحال انجام عملیات...');
        //
        //     $http.post('/PrivateTrain/ServiceReceiverServiceLocation/ChangeStatusRequest', {
        //         "ServiceReceiverServiceLocationId": id,
        //         "Status": status
        //     }).success(function (response) {
        //
        //         if (response.Resualt) {
        //             NotifyCustom(response.Messages, 'success');
        //             buyService.status = status
        //
        //             if (status == 2) {
        //                 $scope.removeBuyService(buyService)
        //             }
        //         } else {
        //             NotifyCustom(response.Messages, 'danger');
        //         }
        //     }).finally(function () {
        //         waitingDialog.hide();
        //     });
        // }

        $scope.callUser = function (buyService) {

        }

        $scope.customer = {}
        $scope.customer.changeProvider = function (buyService) {

            const service = $scope.data.serviceList.find(s => s.id == buyService.serviceId)
            $scope.$root.siparsProviderDialogShow(service, buyService)
        }


        $scope.customer.cancelBuyService = function (buy) {


            let title = ''

            if (buy.status == BuyServiceStatus.doing && buy.property.payOnline) {
                title = 'توجه داشته باشید در صورت کنسل کردن سفارش هزینه کنسلی از شما کسر خواهد شد!!!'
            }

            title += ' دلیل لغو سفارش خود ' + buy.serviceTitle + ' را ثبت نمایید'

            $scope.$root.confirmDialogShow({
                header: 'لغو سفارش ',
                title: title,
                text: 'دلایل...',
                ok: 'ثبت',
                cancel: 'انصراف',
            }, function (text) {
                //do cancel
                waitingDialog.show('درحال لغو سفارش...');

                $http.post('/v1/BuyService/ChangeStatus', {
                    "id": buy.buyService.id,
                    "status": BuyServiceStatus.canceled,
                    "text": text,
                }).success(function (response) {

                    if (response.result == "done") {
                        NotifyCustom(response.message, 'success');
                        buy.buyService.status = BuyServiceStatus.canceled

                        $scope.$root.confirmDialogShow({
                            header: 'لغو انجام شد',
                            title: 'شما خدمت خود را لغو کرده اید در صورت تمایل می توانید آن را فعال کنید',
                            ok: 'خُب',
                        })
                    } else {
                        NotifyCustom(response.message, 'danger');
                    }
                }).finally(function () {
                    waitingDialog.hide();
                });

            })
        }

        $scope.customer.payBuyService = function (buy) {
            $scope.$root.payDialogShow(buy, $scope.data, $scope, buy.buyService.status == BuyServiceStatus.finish)
        }

        $scope.customer.tamdidBuyService = function (buy) {
            const service = $scope.data.serviceList.find(s => s.id == buy.buyService.serviceId)
            $scope.$root.siparsProviderDialogShow(service, buy.buyService)
        }

        $scope.customer.deleteBuyService = function (buy) {
            //let con = confirm('برای حذف سفارش اطمینان دارید؟')
            //if (!con) return

            //delete it
            waitingDialog.show('درحال حذف سفارش...');
            $http.post('/v1/BuyService/ChangeStatus', {
                "id": buy.buyService.id,
                "status": BuyServiceStatus.deleted
            }).success(function (response) {
                if (response.result == "done") {
                    NotifyCustom(response.message, 'success');
                    buy.buyService.status = BuyServiceStatus.deleted
                    $scope.removeBuyService(buy.buyService)

                } else {
                    NotifyCustom(response.message, 'danger');
                }

            }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.customer.activeAgainBuyService = function (buy) {
            const service = $scope.data.serviceList.find(s => s.id == buy.buyService.serviceId)
            $scope.$root.siparsProviderDialogShow(service, buy.buyService)
        }

        $scope.provider = {}
        $scope.provider.unCertainBuyService = function (buy) {

            $scope.$root.confirmDialogShow({
                header: 'لغو کردن سفارش ',
                title: 'توجه داشته باشید که لغو کردن خدمت برای شما امتیاز منفی دارد، لطفا دلیل لغو کردن سفارش ' + buy.serviceTitle + ' را ثبت نمایید',
                text: 'دلایل...',
                ok: 'ثبت',
                cancel: 'انصراف',
            }, function (text) {
                //do cancel
                waitingDialog.show('لغو کردن سفارش...');

                $http.post('/v1/BuyService/ChangeStatus', {
                    "id": buy.buyService.id,
                    "status": BuyServiceStatus.unCertain,
                    "text": text,
                }).success(function (response) {

                    if (response.result == "done") {
                        NotifyCustom(response.message, 'success');
                        buy.buyService.status = BuyServiceStatus.unCertain

                        $scope.$root.confirmDialogShow({
                            header: 'غیرقطعی شد',
                            title: 'شما خدمت خود را غیرقطعی کرده اید',
                            ok: 'خُب',
                        })
                    } else {
                        NotifyCustom(response.message, 'danger');
                    }
                }).finally(function () {
                    waitingDialog.hide();
                });

            })
        }

        $scope.provider.showAddCashPaymentDialog = function (buy) {
            $scope.$root.addCashPaymentDialogShow(buy, $scope.data, $scope);
        }

        $scope.provider.showTimeCostDialog = function (buy) {
            $scope.$root.buyServiceTimeCostShow(buy, $scope.data, $scope)
        }

        $scope.provider.activeAgainBuyService = function (buy) {
            let status = $scope.needMinPay(buy) ? BuyServiceStatus.certain : BuyServiceStatus.doing

            waitingDialog.show('درحال فعال سازی سفارش ...');
            $http.post('/v1/BuyService/ActiveAgainService', {
                "buyServiceId": buy.buyService.id,
                "status": status
            }).success(function (response) {
                if (response.result == "done") {
                    NotifyCustom(response.message, 'success');
                    $scope.getServiceBuyList(true)
                } else {
                    NotifyCustom(response.message, 'danger');
                }

            }).finally(function () {
                waitingDialog.hide();
            });

        }

        $scope.provider.showTimeListDialog = function (buy) {
            $scope.$root.buyServiceTimeCostListDialogShow(buy, $scope.data, $scope)
        }

        $scope.showContact = function (buyService) {
            if (buyService.status == BuyServiceStatus.finish) return false

            return buyService.status == BuyServiceStatus.accept ||
                buyService.status == BuyServiceStatus.certain ||
                buyService.status == BuyServiceStatus.doing
        }

        $scope.finishBuyService = function (buy) {
            $scope.changeBuyServiceStatus(buy)
        }

        $scope.openChatDialog = function (buy) {
            $scope.$root.chatDialogShow(buy, $scope.data, $scope)
        }

        $scope.checkMinPayment = function (buy) {
            $http.post('/v1/BuyService/CheckMinPayment', {
                "id": buy.buyService.id,
            })
        }

        $scope.totalBuyServicePayed = function (buy) {

            let tempReceive = buy.buyService.payed

            buy.costTimeList.forEach(ct => {
                tempReceive += ct.priceReceived
            })

            return tempReceive
        }

        $scope.getBuyTotalPrice = function (buy) {
            let price = 0
            let unknown = false

            const workPriceList = JSON.parse(buy.buyService.workPriceListJson || []).map(w => {
                const wu = $scope.data.workUnitList.find(wu => wu.id == w.workUnitId)
                w.title = wu.title
                w.price = wu.price + (buy.percent / 100 * wu.price)
                return w
            })


            workPriceList.forEach(workPrice => {
                price += +workPrice.price * workPrice.workCount
                if (workPrice.meetingUnknown) unknown = true
            })

            if (unknown) {
                return "نامشخص"
            }

            return price
        }

        $scope.isBuyPayed = function (buy) {
            return $scope.getBuyTotalPrice(buy) <= $scope.totalBuyServicePayed(buy)
        }

        $scope.payDetail = function (buy) {
            $scope.$root.payDetailsShow(buy, $scope.data, $scope, true, true)
        }


        $scope.needMinPay = function (buy) {
            return buy.property.payOnline && (buy.property.payMin > 0 || buy.property.payMinPercent > 0)
        }

        $scope.downloadAttach = function (buy) {
            function DownloadFile(url, name) {
                let a = document.createElement('a')
                a.href = url
                a.target = "blank"
                a.download = name || url.split('/').pop() || "file"
                document.body.appendChild(a)
                a.click()
                document.body.removeChild(a)
            }

            DownloadFile(buy.buyService.attachmentPath)
        }

        $scope.showDesc = function (buy) {
            $scope.$root.confirmDialogShow({
                header: " توضیحات " + buy.serviceTitle,
                title: buy.buyService.userDescription || 'توضیحاتی ثبت نشده است !',
                text: '',
                ok: '',
                cancel: '',
            })
        }


        $scope.init()


        if ('serviceWorker' in navigator && 'PushManager' in window) {
            navigator.serviceWorker.addEventListener('message', function (event) {
                const data = event.data.message
                if (data.type == 'chat') {

                    if ($scope.data.userType == 'provider') {
                        $scope.data.chatCountCustomer[data.buyServiceId] = $scope.data.chatCountCustomer[data.buyServiceId] + 1
                    }

                    if ($scope.data.userType == 'customer') {
                        $scope.data.chatCountProvider[data.buyServiceId] = $scope.data.chatCountProvider[data.buyServiceId] + 1
                    }

                    $scope.updateBuyServiceList()
                    $scope.$applyAsync()

                } else {

                    //refresh list
                    $scope.getServiceBuyList(true)
                }


            });
        }

    }


}();

~function () {
    'use strict';

    //%5W6vd0i
    window.app.directive('buyListDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/dialog/buyListDialog.html?v=' + window.App.version,
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
            buyList: []
        }

        $scope.data = {}


        const model = {
            "StateName": "خراسان رضوی",
            "CityName": "مشهد",
            "ServiceName": null,
            "LocationId": 26,
            "LocationName": "فرامرز- سجاد- فلسطین- کلاهدوز-احمدآباد- کوهسنگی- عدل خمینی",
            "ServicesProviderId": 0,
            "ServiceProviderInfoName": null,
            "ServiceProviderInfoFamily": null,
            "SelectServiceProviderForServices": [{
                "ServiceProviderId": 2399,
                "ServiceProviderName": "مهدی فضائل اسدزاده",
                "ServiceId": 7,
                "ServiceName": "علوم",
                "WorkUnitId": 3
            }],
            "ServiceProviderInfo": null,
            "ServiceReceiverInfo": null,
            "Mobile": "09393013397",
            "Name": "علی",
            "Family": "فرجادپزشک",
            "Email": "seyedali.farjad@gmail.com",
            "Sex": false,
            "HomePhone": null,
            "HomeAddress": null,
            "StateId": 1,
            "CityId": 2,
            "ServiceId": 0,
            "ServiceProviderId": 0,
            "HomeNumber": null,
            "UnitNumber": 0,
            "DateReceiver": null,
            "TimeReceiver": null
        }

        $scope.$root.addToBuyList = function (registerModel, provider, service, workUnitList, serviceLevelList, serviceLevelListId, buyService) {
            // $scope.data.provider, $scope.data.service, workUnitId
            // let price = workUnitList.find(w => w.WorkUnitId == workUnitId).PriceWorkUnit
            // if (serviceLevelList)
            //     price = price + (serviceLevelList.PercentServiceLevel / 100 * price)
            // selectedAddress
            // address: "مشهد"
            // cityId: "2"
            // id: 1
            // phone: "093"

            const workPriceList = []
            angular.forEach(registerModel.prices, function (val, key) {
                if (!val.select) return

                const workUnit = workUnitList.find(w => w.WorkUnitId == key)
                let price = workUnit.PriceWorkUnit
                if (serviceLevelList)
                    price = price + (serviceLevelList.PercentServiceLevel / 100 * price)

                workPriceList.push({
                    workUnitId: key,
                    meetingCount: val.meetingCount,
                    meetingUnknown: val.meetingUnknown,
                    select: val.select,
                    workUnit: workUnit,
                    price
                })
            })

            const buy = {
                provider: provider || {
                    "id": -1,
                    "name": "نامشخص",
                    "family": "",
                    "sex": false,
                    "picture": "",
                    "path": "",
                    "resume": "",
                    "level": 0,
                    "star": 100,
                    "serviceLevelListId": serviceLevelListId
                },
                service: service,
                workPriceList: workPriceList,
                user: $scope.$root.user,
                city: $scope.$root.model.city,
                state: $scope.$root.model.state,
                location: $scope.$root.model.location,
                serviceLevelList: serviceLevelList,
                date: registerModel.date,
                time: registerModel.time,
                address: registerModel.selectedAddress,
                dateTimeSyncByProvider: registerModel.dateTimeSyncByProvider,
                registerModel: registerModel,

                buyService
            }

            const buyItem = JSON.parse(JSON.stringify(buy))
            buyItem.registerModel.file = registerModel.file

            $scope.model.buyList.push(buyItem)
        }

        $scope.buyService = function (buy) {

            const selectedAddress = buy.registerModel.selectedAddress

            const input = {
                id: buy.buyService && buy.buyService.id,
                serviceId: buy.service.Id || buy.service.id,
                locationId: (buy.buyService && buy.buyService.locationId) || buy.location.Id || buy.location.id,
                serviceLevelListId: buy.provider.ServiceLevelListId || buy.provider.serviceLevelListId,

                providerSex: buy.registerModel.userSex == 'all' ? undefined : buy.registerModel.userSex == 'male',
                providerType: buy.registerModel.providerType,
                providerServiceLocationStatus: buy.registerModel.serviceLocationEmpty ? 'none' : buy.registerModel.serviceLocation ? 'provider' : 'customer',
                //workCount: buy.workUnit.meetingUnknown ? -1 : buy.workUnit.meetingCount,
                userDescription: buy.registerModel.description,
                userCityId: selectedAddress && selectedAddress.cityId || -1,
                userCityTitle: '',
                userAddress: selectedAddress && selectedAddress.address || '',
                userMobile: selectedAddress && selectedAddress.phone || '',
                serviceLocationId: buy.registerModel.serviceLocationId || -1,
                serviceProviderId: buy.provider.id,
                totalPrice: $scope.getTotalPrice(buy),
                dateTimeSyncByProvider: buy.dateTimeSyncByProvider,

                date: buy.date,
                time: buy.time,

                workPriceList: buy.workPriceList.map(workPrice => ({
                    workUnitId: +workPrice.workUnitId,
                    workCount: workPrice.meetingUnknown ? -1 : workPrice.meetingCount
                }))

            }

            const city = $scope.$root.cityList && $scope.$root.cityList.find(c => c.Id == input.userCityId) || {Name: 'نامشخص'}
            input.userCityTitle = city.Name || city.name

            const formData = new FormData();
            formData.append("input", JSON.stringify(input));
            formData.append("attachment", buy.registerModel.file);

            waitingDialog.show('درحال ثبت سفارش...');
            $http({
                url: '/v1/BuyService/BuyServiceSelectProvider',
                data: formData,
                method: 'POST',
                headers: {'Content-Type': undefined},
                //prevents serializing payload.  don't do it.
                transformRequest: angular.identity
            })
                .success(function (response) {
                    if (response.result == 'done') {

                        // $scope.model.buyList = $scope.model.buyList.filter(b => b !== buy)

                        //finish
                        // $scope.close()
                        NotifyCustom('درخواست شما با موفقیت ثبت شد.', 'success');
                        // window.location.assign('/Home/IndexPanel')

                        buyServiceResult(buy, true)
                    } else {
                        buyServiceResult(buy, false)
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });

            if (buy.buyService) {
                // window.$scopeBuyList.customer.deleteBuyService(buy.buyService)
            }

        }


        let buyServiceResultList = []

        function buyServiceResult(buyModel, success) {

            const message = "سفارش " +
                $scope.$root.getServiceTitle(buyModel.service) +
                " " + (buyModel.time || '') + ' - ' + (buyModel.date || '') + ' (' +
                buyModel.provider.name + ' ' + buyModel.provider.family + ')'


            buyServiceResultList.push({
                buyModel,
                success,
                message
            })

            if (buyServiceResultList.length == $scope.model.buyList.length) {
                //show message
                const successList = buyServiceResultList.filter(i => i.success)
                const notSuccessList = buyServiceResultList.filter(i => !i.success)

                let msg = ''

                if (successList.length > 0) {
                    msg += 'سفارشات زیر با موفقیت به ثبت رسیدند'
                    msg += '\n'
                    msg += successList.map(i => i.message).join('\n')
                    msg += '\n'
                }

                if (notSuccessList.length > 0) {
                    msg += ' متاسفانه در ثبت سفارشات زیر مشکلی رخ داده است'
                    msg += '\n'
                    msg += notSuccessList.map(i => i.message).join('\n')
                    msg += '\n'
                }

                window.alert(msg)

                if (successList.length > 0 && !window.location.pathname.includes("panel/service/list/1")) {
                    window.location.assign("panel/service/list/1")
                }
            }
        }

        $scope.submit = function () {

            buyServiceResultList = []

            $scope.model.buyList.forEach(buy => {
                $scope.buyService(buy)
            })
        }

        $scope.submit2 = function () {
            // $scope.buyService($scope.model.buyList[0])
            // return

            let length = $scope.model.buyList.length
            $scope.model.buyList.forEach(buy => {
                model.StateName = buy.state.Name
                model.StateId = buy.state.Id

                model.CityName = buy.city.Name
                model.CityId = buy.city.Id

                model.LocationName = buy.location.Name
                model.LocationId = buy.location.Id

                model.Name = buy.user.name
                model.Family = buy.user.family
                model.Sex = buy.user.sex
                model.Mobile = buy.user.mobile
                // model.Email = buy.user.email
                // model.Email = buy.user.email

                model.SelectServiceProviderForServices = [{
                    "ServiceProviderId": buy.provider.id,
                    "ServiceProviderName": buy.provider.name + ' ' + buy.provider.family,
                    "ServiceId": buy.service.Id,
                    "ServiceName": buy.service.Title,
                    "WorkUnitId": +buy.workPriceList[0].workUnitId
                    // "WorkUnitId": buy.workUnitId
                }]

                $http.post('/PrivateTraining/EditApproveServices', model).success(
                    function (response) {
                        length--

                        if (response.Result) {
                            $scope.model.buyList = $scope.model.buyList.filter(b => b !== buy)
                            // waitingDialog.hide();

                            if (length == 0) {
                                //finish
                                $scope.close()
                                NotifyCustom(response.Messages, 'success');
                                // NotifyCustom('درخواست شما با موفقیت ثبت شد.', 'success');
                                window.location.assign("/PrivateTrain/ServiceReceiverServiceLocation/ServicesServiceReceiver")
                            }

                        } else {
                            NotifyCustom(response.Messages, 'danger');
                            // waitingDialog.hide();
                        }


                    });
            })
        }

        $scope.getStarImageUrl = function (star) {
            if (star <= 20) return "/assets/img/smallrating-1.png"
            if (star > 20 && star <= 40) return "/assets/img/smallrating-2.png"
            if (star > 40 && star <= 60) return "/assets/img/smallrating-3.png"
            if (star > 60 && star <= 80) return "/assets/img/smallrating-4.png"
            if (star > 80) return "/assets/img/smallrating-5.png"
        }

        $scope.$root.showBuyListDialog = function (service, locationId, workUnitList, listServiceLevel) {
            $('#buyListDialog').modal('show')
        }

        $scope.close = function () {
            $("#buyListDialog").modal("hide");
        }

        $scope.goToAddService = function () {

        }

        $scope.removeBuy = function (buy) {
            const confirmDelete = confirm('از لیست حذف شود؟')
            if (!confirmDelete) return
            $scope.model.buyList = $scope.model.buyList.filter(b => b !== buy)
        }

        $scope.getTotalPrice = function (buy) {
            let price = 0
            let unknown = false
            buy.workPriceList.forEach(workPrice => {
                price += +workPrice.price * workPrice.meetingCount
                if (workPrice.meetingUnknown) unknown = true
            })

            if (unknown) {
                return "نامشخص"
            }

            return price
        }

    }


}();

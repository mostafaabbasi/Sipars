~function () {
    'use strict';

    window.app.directive('serviceRegisterDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/serviceRegisterDialog.html?v=' + window.App.version,
            scope: {
                location: '=',
                serviceList: '=',
                service: '=',
            },
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });

    function controller($scope, $transclude, $element, $http) {

        window.$scope = $scope
        $scope.model = {
            mode: 'provider',
            providerType: 'providerSelectCustomer', //providerSelectCustomer providerSelectSipars providerSelectProvider

            userSex: "all",
            serviceLevelId: '0',
            showLevelAndWorkUnits: false,

            formTitle: 'مشخصات خدمتیار',
            serviceLocation: true,//'provider' customer
            serviceLocationEmpty: true,//'provider' customer
            customerSex: true,//'male' female
            description: '',

            prices: {},
            if: {}

        }


        $scope.data = {}


        $scope.model.addressList = [{
            id: 1,
            phone: '09393013397',
            address: 'وکیل آباد-خیابان ابومسلم-پلاک 39- طبقه 1',
            cityId: '2',
            cityTitle: 'مشهد',
        }]

        $scope.initAddress = function () {
            $scope.model.addressList = JSON.parse($scope.$root.user.addressJson || "[]")
            $scope.model.phone = $scope.$root.user.mobile || ''

            if ($scope.$root.model.city) {
                $scope.model.currentCityId = $scope.$root.model.city.Id
                $scope.model.CityId = $scope.$root.model.city.Id
                $scope.model.cityDisable = true
            }
        }

        $scope.findCity = function (cityId) {
            return $scope.$root.cityList && $scope.$root.cityList.find(c => c.Id == cityId) || {Name: 'نامشخص'}
        }

        $scope.removeAddress = function (addressId) {
            const newList = $scope.model.addressList.filter(a => a.id != addressId)

            waitingDialog.show('درحال حذف آدرس...');
            $http.post('/v1/Account/UpdateUser', {"addressJson": newList})
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.model.addressList = newList

                        $scope.$root.user.addressJson = JSON.stringify(newList)
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });

        }

        //TODO
        //1- level to combo box

        $scope.$root.showRegisterDialog = function (service, locationId, workUnitList, listServiceLevel, provider, providerType, serviceLocationId, buyService) {

            $scope.model = {
                mode: 'provider',
                providerType: 'providerSelectCustomer', //providerSelectCustomer providerSelectSipars providerSelectProvider

                userSex: "all",
                serviceLevelId: '0',
                showLevelAndWorkUnits: false,

                formTitle: 'مشخصات خدمتیار',
                serviceLocation: true,//'provider' customer
                serviceLocationEmpty: true,//'provider' customer
                customerSex: true,//'male' female
                description: '',
                serviceLocationId: serviceLocationId,
                prices: {},
                if: {},



            }

            $scope.data.service = service
            $scope.data.locationId = locationId
            $scope.data.workUnitList = workUnitList
            $scope.data.listServiceLevel = listServiceLevel
            $scope.data.provider = provider

            $scope.data.buyService = buyService

            $scope.data.providerType = providerType
            $scope.model.providerType = providerType

            $scope.setDefaultPrice()
            $scope.setFormTitle()
            $scope.setIf()
            $scope.getUserDetail()

            //auto select
            $scope.model.mode = 'provider'
            const needProvider = [
                $scope.model.providerType != 'providerSelectCustomer' &&
                ($scope.model.if.askProviderSex ||
                    $scope.model.if.askProviderLevel),
                $scope.model.if.askLocation,
                $scope.model.if.askCustomerSex].find(f => f)

            if (!needProvider) {
                $scope.continue()
            } else {
                if (!$scope.model.if.askLocation) $scope.continue()
            }

            $scope.toggleDatePicker()
            $('#serviceRegister').modal('show')
        }

        let _getLogin = false
        let _needLoginPage = null

        //not used
        $scope.getAnotherUserDetail = function (userId) {
            waitingDialog.show('درحال بارگذاری اطلاعات حساب...');
            $http.get('/v1/Account/Detail/' + userId, {userId})
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.data.anotherUser = response.user
                    } else {
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.getUserDetail = function () {
            if (_getLogin) return
            // if ($scope.$root.user && $scope.$root.user.id) return

            waitingDialog.show('درحال بارگذاری اطلاعات حساب...');
            $http.get('/v1/Account/IsLogin', {})
                .success(function (response) {
                    if (response.result == 'done') {
                        _getLogin = true
                        $scope.$root.user = response.user
                        window.localStorage.user = JSON.stringify(response.user)
                    } else {
                        _needLoginPage = true
                        $scope.$root.user = {}
                        window.localStorage.user = ''
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        let isLoginOpened = false
        $scope.login = function () {
            isLoginOpened = true
            $scope.$root.showAccountDialog()
        }

        $scope.setIf = function () {
            const service = $scope.data.service
            const _if = {}

            _if.askProviderLevel = $scope.data.listServiceLevel.length > 0
            _if.askCustomerSex = service.askCustomerSex
            _if.askProviderSex = service.askProviderSex
            _if.askLocation = service.serviceLocationCustomer && service.serviceLocationProvider

            $scope.model.serviceLocationEmpty = !_if.askLocation

            // $scope.model.mode = 'main'
            // if (_if.askProviderLevel || _if.askProviderLevel || _if.askProviderLevel || _if.askProviderLevel) {
            //     $scope.model.mode = 'provider'
            // }


            _if.askPrice = $scope.data.workUnitList.length > 0 && $scope.model.providerType !== 'providerSelectProvider'
            _if.multiPrice = service.multiPrice
            _if.forceAttach = service.forceAttach
            _if.askTime = service.askTime

            if (_if.askTime) {
                $scope.setInitTime()
            }

            $scope.model.if = _if
        }

        $scope.if = {}

        $scope.if.askLocationAddress = function () {
            return $scope.data.service.serviceLocationCustomer && !$scope.data.service.serviceLocationProvider
                || (!$scope.model.serviceLocation)
        }


        $scope.priceSelect = function (workUnitId) {
            if ($scope.model.if.multiPrice) return
            Object.values($scope.model.prices).forEach(value => {
                value.select = false
            })

            $scope.model.prices[workUnitId].select = true

        }

        $scope.setServiceLevelList = function () {
            const serviceLevelListId = $scope.model.serviceLevelId
            if (!serviceLevelListId) return
            $scope.data.serviceLevelList = $scope.data.listServiceLevel.find(sl => sl.ServiceLevelListId == serviceLevelListId)

        }

        $scope.setDefaultPrice = function () {

            $scope.model.prices = {}

            if ($scope.model.providerType !== 'providerSelectProvider' && $scope.data.listServiceLevel[0]) {
                $scope.model.serviceLevelId = $scope.data.listServiceLevel[0].ServiceLevelListId
                $scope.setServiceLevelList()
            } else {
                $scope.model.serviceLevelId = -1
            }


            let serviceLevelListId = ''
            if ($scope.model.providerType == 'providerSelectCustomer') {
                serviceLevelListId = $scope.data.provider.serviceLevelListId
            } else {
                serviceLevelListId = $scope.model.serviceLevelId
            }

            // if (!serviceLevelListId) return
            $scope.data.serviceLevelList = $scope.data.listServiceLevel.find(sl => sl.ServiceLevelListId == serviceLevelListId)

            let set = true
            $scope.data.workUnitList.forEach(function (workUnit) {
                $scope.model.prices[workUnit.WorkUnitId] = {
                    meetingCount: 1,
                    meetingUnknown: false,
                    select: set,
                }

                set = false
            })
        }

        $scope.selectProviderSelectCustomer = function () {
            $scope.close()
            $scope.$root.showProviderList($scope.data.service, $scope.data.locationId, $scope.data.workUnitList, $scope.data.listServiceLevel)
        }

        $scope.getProviderImage = function (provider) {
            if (!provider.picture)
                return '/UserFiles/ProfilePicture/default-user.png'
            return 'http://sipars.ir' + provider.path + '/' + provider.picture
        }

        $scope.setFormTitle = function () {
            $scope.model.formTitle = {
                'provider': 'مشخصات خدمتیار',
                'main': 'انتخاب تعرفه',
            }[$scope.model.mode] || 'ثبت سفارش'
        }

        $scope.addAddress = function () {
            $scope.selectMode('address')
            const id = Math.max($scope.model.addressList.map(a => a.id)) + 1

            if (!$scope.model.phone || !$scope.model.addressDetail || !$scope.model.CityId) return

            const address = {
                id: id,
                phone: $scope.model.phone,
                address: $scope.model.addressDetail,
                cityId: $scope.model.CityId,
                // cityTitle: ($scope.$root.cityList.find(c => c.Id == $scope.model.CityId) || {Name: 'نامشخص'}).Name
            }

            const newList = [].concat($scope.model.addressList || []).concat([address])

            waitingDialog.show('درحال افزودن آدرس...');
            $http.post('/v1/Account/UpdateUser', {"addressJson": newList})
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.model.addressList = newList

                        $scope.model.phone = ''
                        $scope.model.addressDetail = ''
                        $scope.model.CityId = ''

                        $scope.$root.user.addressJson = JSON.stringify(newList)
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });


        }

        $scope.close = function () {
            $("#serviceRegister").modal("hide");
        }


        $scope.setServiceList = function (lastService) {
            // const lastService = $scope.model.selectedService.slice(-1)[0]
            if (lastService && lastService.id != -1) {
                const newList = $scope.data.serviceList.filter(service => service.level == lastService.level + 1 && service.parentId == lastService.id)
                if (newList.length !== 0) {
                    $scope.model.serviceList = newList
                } else {
                    return true
                }
            } else {
                $scope.model.serviceList = $scope.data.serviceList.filter(service => service.level == 1)
            }
        }


        $scope.setServiceTypes = function () {
            $scope.data.serviceList.forEach(service => {
                let type = 'parent'
                const newList = $scope.data.serviceList.filter(s => s.level == service.level + 1 && s.parentId == service.id)
                if (newList.length === 0) {
                    type = 'child'
                }
                $scope.model.serviceTypes[service.id] = type
            })
        }


        $scope.back = function () {
            if ($scope.model.mode == 'provider') {
                //nothing
            } else if ($scope.model.mode == 'main') {
                $scope.selectMode('provider')
            } else if ($scope.model.mode == 'address' || $scope.model.mode == 'addAddress') {
                $scope.model.if.askTime && $scope.selectMode('time')
                !$scope.model.if.askTime && $scope.selectMode('main')
            } else {
                $scope.selectMode('main')
            }
        }

        $scope.continue = function () {

            if ($scope.model.mode == 'provider') {
                $scope.selectMode('main')
            } else if ($scope.model.mode == 'main' && $scope.model.if.askTime) {
                $scope.selectMode('time')
            } else if (($scope.model.mode == 'main' || $scope.model.mode == 'time') && $scope.if.askLocationAddress()) {
                $scope.selectMode('address')
                $scope.initAddress()
            } else {
                //finish
                if ($scope.if.askLocationAddress() && !$scope.model.selectedAddress) {
                    return NotifyCustom('آدرسی باید انتخاب شود!', 'danger');
                }
                const workUnitId = Object.keys($scope.model.prices).find(key => $scope.model.prices[key].select)
                const sId = $scope.data.provider.ServiceLevelListId || $scope.data.provider.serviceLevelListId
                $scope.$root.addToBuyList($scope.model, $scope.data.provider, $scope.data.service, $scope.data.workUnitList, $scope.data.serviceLevelList, sId, $scope.data.buyService)
                $scope.close()
                $scope.$root.showBuyListDialog()
            }
        }

        $scope.selectMode = function (mode) {
            $scope.model.mode = mode
        }

        $scope.selectService = function (service) {
            if ($scope.setServiceList(service)) {
                //select
                $scope.buyService(service)
            } else {
                const index = $scope.model.selectedService.indexOf(service)
                if (index === -1) {
                    $scope.model.selectedService.push(service)
                } else {
                    $scope.model.selectedService.splice(index + 1)
                }
            }

        }

        $scope.enterService = function (enterService) {
            $scope.model.selectedService.splice(1)
            const sList = []

            const addParentToList = function (service) {
                if (!service) return
                if ($scope.model.serviceTypes[service.id] != 'child') {
                    sList.unshift(service)
                }
                addParentToList($scope.data.serviceList.find(s => s.id == service.parentId))
            }
            addParentToList(enterService)
            sList.forEach($scope.selectService)
        }

        $scope.onServiceClick = function (service) {
            $scope.selectService(service)
        }

        $scope.buyService = function (service) {
            const parentScope = $scope.getParentScope()
            parentScope.$$childTail.Approve.ServiceId = service.id
            parentScope.ShowServiceProvider(service.id)
        }

        $scope.getParentScope = function () {
            let scope = $scope
            while (!scope.ShowServiceProvider) {
                scope = scope.$parent
            }
            return scope
        }


        $scope.toggleDatePicker = function () {

            $scope.model.showDatePicker = !$scope.model.showDatePicker
            $("#datePicker").css('display', $scope.model.showDatePicker ? '' : 'none')
        }


        $scope.model.dateType = 'today'
        $scope.selectDate = function (type) {
            $scope.model.dateType = type
            if (type == 'tomorrow') {
                var today = new Date();
                var tomorrow = new Date();
                tomorrow.setDate(today.getDate() + 1);
                $scope.model.date = new persianDate(tomorrow.getTime()).format('YYYY/MM/DD')
            } else {
                $scope.model.date = new persianDate(new Date().getTime()).format('YYYY/MM/DD')
            }

            $scope.model.date = window.toEnglishDigits($scope.model.date)
        }

        let _inited = false
        let myDatePicker
        let myTimePicker
        $scope.setInitTime = function () {

            if (_inited) {
                $scope.model.date = new persianDate(new Date().getTime()).format('YYYY/MM/DD')
                $scope.model.date = window.toEnglishDigits($scope.model.date)
                myTimePicker.correctTime(new Date().getTime())
                return
            }

            _inited = true

            myDatePicker = $("#datePicker").pDatepicker({
                inline: true,
                autoClose: false,
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
                    $scope.model.date = new persianDate(selectedDayUnix).format('YYYY/MM/DD')
                    $scope.model.date = window.toEnglishDigits($scope.model.date)

                    $scope.toggleDatePicker()
                    $scope.$apply()
                    // $scope.model.date = window.toEnglishDigits(new persianDate(selectedDayUnix).format('YYYY/MM/DD'))
                },
            });

            myTimePicker = $("#timepicker").pDatepicker({
                inline: true,
                autoClose: true,
                'timePicker': {
                    'enabled': true,
                    'step': 1,
                    'hour': {
                        'enabled': true,
                        'step': null
                    },
                    'minute': {
                        'enabled': true,
                        'step': 15
                    },
                    'second': {
                        'enabled': false,
                        'step': null
                    },
                    'meridian': {
                        'enabled': false
                    }
                },
                'toolbox': {
                    'enabled': false,
                    submitButton: {
                        'enabled': false,
                    },
                },
                'onSelect': function onSelect(selectedDayUnix) {
                    $scope.model.time = new persianDate(selectedDayUnix).format('HH:mm')
                    $scope.model.time = window.toEnglishDigits($scope.model.time)
                    // const pd = myTimePicker.correctTime(selectedDayUnix)
                    // $scope.model.time = pd.format('HH:mm')
                    // console.log($scope.model.time)
                    $scope.$apply()
                },
                'onlyTimePicker': true,
            });


            myDatePicker.setDate(new Date().getTime());
            myTimePicker.setDate(new Date().getTime());

            let lastTime = ''
            myTimePicker.correctTime = function (selectedDayUnix) {
                const pd = new persianDate(selectedDayUnix)
                let minute = pd.minute()
                lastTime = lastTime || selectedDayUnix
                minute = minute - minute % 15 + (selectedDayUnix > lastTime ? 15 : 0)
                // if (minute >= 60) minute = 0
                pd.minute(minute)
                lastTime = pd.toDate().getTime()
                $scope.model.time = window.toEnglishDigits(pd.format('HH:mm:ss'))
                myTimePicker.setDate(pd);
                return pd
            }

            myTimePicker.correctTime(new Date().getTime())
        }


        //no need
        $scope.getLevelAndPrice = function () {
            const ServiceId = $scope.$root.Approve.service.id
            const LocationId = $scope.$root.Approve.LocationId
            $http.post('/PrivateTrain/ServiceProperties/LoadServiceWorkUnit',
                {"ServiceId": ServiceId, "LocationId": LocationId, ServiceLevelListId: 0})
                .success(function (response) {
                    if (response.Resualt) {
                        $scope.data.workUnitList = response.WorkUnits;
                        $scope.model.workUnitList = response.WorkUnits;

                        $scope.model.selectedUnit = $scope.model.workUnitList[0]

                    }
                });

            $http.post('/Account/ListServiceLevelPost/0', {"Id": ServiceId}).success(
                function (response) {
                    if (response.Result) {
                        $scope.data.listServiceLevel = response.Message;
                        $scope.model.listServiceLevel = response.Message;


                        // $scope.model.selectedLevel = $scope.model.listServiceLevel[0]
                    }
                });


        }

        $scope.getPrice = function (workUnit) {
            const percent = $scope.data.serviceLevelList && $scope.data.serviceLevelList.PercentServiceLevel || 0
            const priceWorkUnit = workUnit.PriceWorkUnit

            const price = priceWorkUnit + (percent / 100 * priceWorkUnit)

            return $scope.$root.getCurrency(price)
        }

        $scope.insertFile = function (input) {
            const file = input && input.files[0]
            if (!file) {
                return
            }

            $scope.model.fileUrl = URL.createObjectURL(file)
            $scope.model.file = file

            const fileType = file['type'];
            const validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
            $scope.model.fileIsImage = validImageTypes.includes(fileType);


            $scope.$applyAsync()
        }

        $scope.removeFile = function () {
            $scope.model.fileUrl = null
            $scope.model.file = null
        }


        $scope.checkUserIsLogin = function () {
            const needLogin = $scope.model.mode == 'main' && !$scope.$root.user.id

            if (isLoginOpened && _needLoginPage && $scope.$root.user.id) {
                _needLoginPage = false
                $scope.continue()
            }

            return needLogin
        }

    }


}();

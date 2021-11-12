~function () {
    'use strict';

    window.app.directive('serviceProviderSelectDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/dialog/serviceProviderSelectDialog.html?v=' + window.App.version,
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
            level: 1,
            serviceList: [],
            selectedService: [{id: -1, title: 'همه خدمات'}],
            serviceTypes: {},
            workUnitList: [],
            addressPicker: true,
            CityId: '2',
            mode: 'main',

            loading: {
                serviceProp: false,
                workUnit: false,
                serviceLevel: false,
                providerList: false,
                location: false,
            }
        }
        $scope.data = {
            serviceList: [],
            workUnitList: [],
            listServiceLevel: [],
            providerList: [],
        }

        $scope.model.tooltip = {
            "0": "درصورت داشتن کدتخفیف آن را وارد نمایید تا مبلغ مورد نظر از هزینه خدمت کسر شود.",
            "1": "انتخاب از خدمتیاران بر اساس مشخصات، رزومه، تخفیفات ",
            "2": "انتخاب شایسته ترین خدمتیار توسط سی پارس",
            "3": "انتخاب خدمتیار پس از دریافت قیمت های پیشنهادی",
        }

        $scope.$root.siparsProviderDialogShow = function (service, buyService) {
            waitingDialog.show("دریافت اطلاعات سرویس")
            $scope.model.loading.serviceProp = true
            $scope.model.loading.workUnit = true
            $scope.model.loading.serviceLevel = true
            $scope.model.loading.providerList = true

            $scope.model.loading.location = !$scope.$root.cityList || $scope.$root.cityList.length == 0

            //set location
            $scope.data.service = service

            if (buyService) {
                $scope.data.locationId = buyService.locationId
            } else {
                $scope.data.locationId = $scope.$root.Approve.LocationId
            }

            $scope.data.buyService = buyService

            $scope.getServiceProp(service.id)
            $scope.getServiceWorkUnit(service.id)
            $scope.getServiceLevel(service.id)
            $scope.getProviderList(service.id, $scope.data.locationId)

            if ($scope.model.loading.location) {
                $scope.getLocationList()
            }

            $('#serviceProviderSelectDialog').modal('show')

        }


        $scope.autoSelect = function () {

            const onlyOne = [$scope.model.service.providerSelectCustomer,
                $scope.model.service.providerSelectSipars,
                $scope.model.service.providerSelectProvider].filter(f => f).length === 1

            if (onlyOne) {
                $scope.model.service.providerSelectCustomer &&
                $scope.selectProviderSelectCustomer()

                $scope.model.service.providerSelectSipars &&
                $scope.selectProviderSelectSipars()

                // $scope.model.service.providerSelectProvider
                //todo
            }
        }

        //انتخاب توسط مشتری
        $scope.selectProviderSelectCustomer = function () {
            $scope.close()
            $scope.$root.showProviderList($scope.data.serviceProperty, $scope.data.locationId, $scope.data.workUnitList, $scope.data.listServiceLevel, $scope.data.providerList, $scope.model.ServiceLocationId, $scope.data.buyService)
        }
        //انتخاب توسط سی پارس
        $scope.selectProviderSelectSipars = function () {
            $scope.close()

            if ($scope.data.providerList.length === 0) {
                NotifyCustom('باعرض پوزش خدمتار برای سفارش شما وجود ندارد. اُمید که خدمتگذار خوبی برای سفارشات بعدی شما باشیم.', 'danger');
                return
            }


            $scope.$root.showRegisterDialog($scope.data.serviceProperty, $scope.data.locationId, $scope.data.workUnitList, $scope.data.listServiceLevel, null, 'providerSelectSipars', $scope.model.ServiceLocationId, $scope.data.buyService)
        }


        $scope.getServiceLevel = function () {
            $http.post('/Account/ListServiceLevelPost/0', {"Id": $scope.data.service.id}).success(
                function (response) {
                    if (response.Result) {
                        $scope.data.listServiceLevel = response.Message;
                        $scope.model.listServiceLevel = response.Message;
                    }
                }).finally(function () {
                $scope.model.loading.serviceLevel = false
                if (!Object.values($scope.model.loading).find(l => l)) {
                    $scope.autoSelect()
                    waitingDialog.hide();
                }
            })
        }

        $scope.getServiceWorkUnit = function () {
            $http.post('/PrivateTrain/ServiceProperties/LoadServiceWorkUnit',
                {"ServiceId": $scope.data.service.id, "LocationId": $scope.data.locationId, ServiceLevelListId: 0})
                .success(function (response) {
                    if (response.Resualt) {
                        $scope.data.workUnitList = response.WorkUnits;
                        $scope.model.workUnitList = response.WorkUnits;
                        $scope.model.ServiceLocationId = response.ServiceLocationId
                        $scope.$root.model.serviceLocationId = response.ServiceLocationId
                    }
                })
                .finally(function () {
                    $scope.model.loading.workUnit = false
                    if (!Object.values($scope.model.loading).find(l => l)) {
                        $scope.autoSelect()
                        waitingDialog.hide();
                    }
                })
        }

        $scope.getServiceProp = function (id) {
            $http.get('/v1/PrivateTraining/ServiceProperty/' + id, {id})
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.data.serviceProperty = response.serviceProperty
                        $scope.model.service = response.serviceProperty


                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                $scope.model.loading.serviceProp = false
                if (!Object.values($scope.model.loading).find(l => l)) {
                    $scope.autoSelect()
                    waitingDialog.hide();
                }

            });
        }

        $scope.getProviderList = function (serviceId, locationId) {
            $http.post('/v1/PrivateTraining/serviceProviderLocation', {
                serviceId,
                locationId
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.data.providerList = response.items
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                $scope.model.loading.providerList = false
                if (!Object.values($scope.model.loading).find(l => l)) {
                    $scope.autoSelect()
                    waitingDialog.hide();
                }

            });
        }

        $scope.getLocationList = function (serviceId, locationId) {
            $http.get('/v1/base/cityList',
                {"StateId": 1 /*khorasan razavi*/}).success(function (response) {
                if (response.result == 'done') {
                    $scope.$root.cityList = $scope.Cities = response.items.map(city => ({
                        IsEnable: true,
                        Id: city.id,
                        Name: city.name,
                        StateId: '1'
                    }));
                } else {
                }
            }).finally(function () {
                $scope.model.loading.location = false
                if (!Object.values($scope.model.loading).find(l => l)) {
                    $scope.autoSelect()
                    waitingDialog.hide();
                }
            });
        }


        // $scope.ListPrice = function () {
        //     $("#ShowPriceWorkUnit").modal("show");
        //     $scope.Priceworkunits = {};
        //     const ServiceId = $scope.$root.Approve.service.id
        //     $http.post('/PrivateTrain/ServiceProperties/LoadServiceWorkUnit',
        //         {"ServiceId":ServiceId, "LocationId": $scope.$root.Approve.LocationId}).success(
        //         function (response) {
        //             if (response.Resualt) {
        //                 $scope.Priceworkunits = response.WorkUnits;
        //                 //$scope.$apply();
        //             }
        //         });
        //
        //     var CountListServiceLevel = 0;
        //     $http.post('/Account/ListServiceLevelPost/0', {"Id": ServiceId}).success(
        //         function (response) {
        //             if (response.Result) {
        //                 $scope.ListServiceLevel = response.Message;
        //                 $scope.ServiceLevelListId = 0;
        //                 //$scope.$apply();
        //                 CountListServiceLevel = response.Message.length;
        //
        //                 if (CountListServiceLevel == 0 || CountListServiceLevel == undefined) {
        //                     $scope.RefreshUnit(0, 1, 1);
        //                 }
        //             }
        //         });
        //
        //     $("#ServiceLevelListId").val(0);
        //
        //     // $scop
        //     // e.ShowListServiceLevel(id);
        // }
        //
        // $scope.addAddress = function () {
        //     $scope.selectMode('address')
        //     $scope.model.addressList.push({
        //         id: 2,
        //         phone: $scope.model.phone,
        //         address: $scope.model.addressDetail,
        //         cityId: $scope.model.CityId,
        //         cityTitle: ($scope.$root.cityList.find(c => c.Id == $scope.model.CityId)||{Name: 'نامشخص'}).Name
        //     })
        // }

        $scope.close = function () {
            $("#serviceProviderSelectDialog").modal("hide");
        }


    }


}();

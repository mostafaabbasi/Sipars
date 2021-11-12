~function () {
    'use strict';

    window.app.directive('providerListDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/dialog/providerListDialog.html?v=' + window.App.version,
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
            providerList: [],
            userSex: "all",
            serviceLevelId: '0',
            showLevelAndWorkUnits: false
        }

        $scope.data = {
            serviceId: '',
            locationId: '',
            providerList: [],
        }

        //نمایش لیست خدمت یاران برای مشتری
        $scope.$root.showProviderList = function (service, locationId, workUnitList, listServiceLevel, providerList, serviceLocationId, buyService) {
            $scope.data.service = service
            $scope.data.serviceId = service.Id
            $scope.data.locationId = locationId
            $scope.data.workUnitList = workUnitList
            $scope.data.listServiceLevel = listServiceLevel

            $scope.data.buyService = buyService
            $scope.data.serviceLocationId = serviceLocationId

            $scope.model = {
                providerList: [],
                userSex: "all",
                serviceLevelId: '0',
                showLevelAndWorkUnits: false
            }

            if (providerList)
                $scope.data.providerList = providerList

            // $scope.getProviderList(service.Id, locationId)
            $scope.setProviderList()

            $('#providerListDialog').modal('show')

        }

        $scope.close = function () {
            $("#providerListDialog").modal("hide");
        }

        $scope.selectProvider = function (provider) {

            $scope.close()

            const buyService = $scope.data.buyService
            if (buyService && (buyService.status == 7 || buyService.status == 6)) {
                return $scope.changeProvider(provider)
            }

            $scope.$root.showRegisterDialog($scope.data.service, $scope.data.locationId, $scope.data.workUnitList, $scope.data.listServiceLevel, provider, 'providerSelectCustomer', $scope.data.serviceLocationId, buyService)
        }


        $scope.setProviderList = function () {
            const buyService = $scope.data.buyService

            $scope.model.providerList = $scope.data.providerList.filter(provider => {
                if ($scope.model.serviceLevelId != "0" && provider.serviceLevelListId != $scope.model.serviceLevelId) return false
                if ($scope.model.userSex != 'all' && provider.sex == ($scope.model.userSex != "female")) return false

                // if (buyService && buyService.serviceProviderId == provider.id) return false

                return true
            })
        }

        $scope.changeProvider = function (provider) {
            waitingDialog.show();
            $http.post('/v1/buyService/ChangeProvider', {
                buyServiceId: $scope.data.buyService.id,
                providerId: provider.id,
                serviceLocationId: $scope.data.serviceLocationId,
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        window.$scopeBuyList.updateBuyService(response.buyService)
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        // Family: "حسینی پناه"
        // Id: 2242
        // Level: 0
        // Name: "سیدجلیل"
        // Path: "/UserFiles/ProfilePicture"
        // Picture: "Picture_517113702.jpg"
        // Resume: "کارشناسی علوم تربیتی -اآموزگار رسمی آموزش و پرورش، 15 سال سابقه تدریس در دوره ابتدایی و5سال سابقه تدریس خصوصی"
        // ServiceLevelListId: 9
        // Sex: false
        // Star: 3069
        $scope.getProviderList = function (serviceId, locationId) {
            waitingDialog.show();
            $http.post('/v1/PrivateTraining/serviceProviderLocation', {
                serviceId,
                locationId
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.data.providerList = response.items
                        $scope.setProviderList()

                        $('#providerListDialog').modal('show')
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }


        $scope.getStarImageUrl = function (star) {
            if (star <= 20) return "/assets/img/smallrating-1.png"
            if (star > 20 && star <= 40) return "/assets/img/smallrating-2.png"
            if (star > 40 && star <= 60) return "/assets/img/smallrating-3.png"
            if (star > 60 && star <= 80) return "/assets/img/smallrating-4.png"
            if (star > 80) return "/assets/img/smallrating-5.png"
        }

    }


}();

~function () {
    'use strict';

    angular.module('App').directive('editServiceDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/dialog/editServiceDialog.html?v=' + window.App.version,
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
            checkBoxs: {},

        }

        $scope.data = {
            serviceList: [],
            workUnitList: []
        }

        const serviceProperty = {
            Id: 1,
            pricingSipars: 0,
            pricingProvider: 0,
            pricingShared: 0,

            providerSelectSipars: 0,
            providerSelectCustomer: 0,
            providerSelectProvider: 0, //best price

            serviceLocationCustomer: 0,
            serviceLocationProvider: 0,
            serviceLocationLess: 0,

            multiProviderSelect: 0,
            multiProviderOffer: 0,

            payOnline: 0,
            payMin: 0,
            payMinPercent: 0,

            askCustomerSex: 0,
            askProviderAddress: 0,
            askProviderSex: 0,
            askTime: 0,
            forceAttach: 0,
            multiPrice: 0,

            serviceUnitName: '',

            baseOff: 0,
            minOff: 0,

            serviceDescription: '',
            priceDescription: ''
        }

        waitingDialog.hide();

        $scope.model.service = Object.assign({}, serviceProperty)

        $scope.getChildProps = function () {
            const child = {}
            if ($scope.model.checkBoxs.price) {
                child.pricingSipars = $scope.model.service.pricingSipars
                child.pricingProvider = $scope.model.service.pricingProvider
                child.pricingShared = $scope.model.service.pricingShared
            }

            if ($scope.model.checkBoxs.providerChoose) {
                child.providerSelectSipars = $scope.model.service.providerSelectSipars
                child.providerSelectCustomer = $scope.model.service.providerSelectCustomer
                child.providerSelectProvider = $scope.model.service.providerSelectProvider
            }

            if ($scope.model.checkBoxs.serviceLocation) {
                child.serviceLocationCustomer = $scope.model.service.serviceLocationCustomer
                child.serviceLocationProvider = $scope.model.service.serviceLocationProvider
                child.serviceLocationLess = $scope.model.service.serviceLocationLess
            }

            if ($scope.model.checkBoxs.multiProvider) {
                child.multiProviderSelect = $scope.model.service.multiProviderSelect
                child.multiProviderOffer = $scope.model.service.multiProviderOffer
            }

            if ($scope.model.checkBoxs.pay) {
                child.payOnline = $scope.model.service.payOnline
                child.payMin = $scope.model.service.payMin
                child.payMinPercent = $scope.model.service.payMinPercent
            }

            if ($scope.model.checkBoxs.askCustomerSex) {
                child.askCustomerSex = $scope.model.service.askCustomerSex
            }

            if ($scope.model.checkBoxs.askProviderAddress) {
                child.askProviderAddress = $scope.model.service.askProviderAddress
            }

            if ($scope.model.checkBoxs.askProviderSex) {

                child.askProviderSex = $scope.model.service.askProviderSex
            }

            if ($scope.model.checkBoxs.askTime) {
                child.askTime = $scope.model.service.askTime
            }

            if ($scope.model.checkBoxs.forceAttach) {
                child.forceAttach = $scope.model.service.forceAttach
            }

            if ($scope.model.checkBoxs.multiPrice) {
                child.multiPrice = $scope.model.service.multiPrice
            }

            if ($scope.model.checkBoxs.serviceUnitName) {
                child.serviceUnitName = $scope.model.service.serviceUnitName
            }

            if ($scope.model.checkBoxs.baseOff) {
                child.baseOff = $scope.model.service.baseOff
                child.minOff = $scope.model.service.minOff
            }

            if ($scope.model.checkBoxs.serviceDescription) {
                child.serviceDescription = $scope.model.service.serviceDescription
            }

            if ($scope.model.checkBoxs.priceDescription) {
                child.priceDescription = $scope.model.service.priceDescription
            }

            return child
        }


        $scope.edit = function () {
            waitingDialog.show("درحال ویرایش سرویس...");
            const editServiceProperty = Object.assign({}, serviceProperty, $scope.model.service)
            $http.post('/PrivateTrain/ServiceProperties/EditServiceExtraProperties',
                {editService: editServiceProperty, childService: JSON.stringify($scope.getChildProps())})
                .success(function (response) {
                    if (response.result == 'done') {
                        NotifyCustom('ویرایش با موفقیت انجام شد', 'success');
                        $('#editServiceDialog').modal('hide')
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            })
        }

        $scope.model.deleteImage = function () {
            waitingDialog.show("درحال حذف تصویر...");

            $http.get('/v1/PrivateTraining/DeleteServiceImage/' + $scope.model.id, {id: $scope.model.id})
                .success(function (response) {
                    if (response.result == 'done') {
                        NotifyCustom('تصویر با موفقیت حذف شد.', 'success');
                        $scope.model.imageUrl = '/assets/New/img/LogoC.png'
                        $scope.model.service.image = ""
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.$root.showEditServiceDialog = function (id, name) {
            waitingDialog.show('دریافت اطلاعات خدمت ' + name);

            $scope.model.title = name
            $scope.model.id = id
            $scope.model.imageUrl = ''
            $scope.model.file = null

            $http.get('/v1/PrivateTraining/ServiceProperty/' + id, {id})
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.data.serviceProperty = serviceProperty
                        $('#editServiceDialog').modal('show')
                        Object.assign($scope.model.service, response.serviceProperty)

                        $scope.model.tagTitle = $scope.model.service.tagTitle
                        $scope.model.showTag = $scope.model.service.showTag
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.model.insertFile = function (input) {

            const file = input && input.files[0]
            if (!file) {
                return
            }

            $scope.model.imageUrl = URL.createObjectURL(file)
            $scope.model.file = file
            $scope.$applyAsync()
        }


        $scope.model.uploadImage = function (setChild) {

            if (!$scope.model.file) return NotifyCustom('فایل یافت نشد!', 'danger');

            var fd = new FormData();
            fd.append('image', $scope.model.file);
            fd.append('id', $scope.model.id);
            fd.append('setForChildren', setChild ? "1" : "0");

            waitingDialog.show('درحال آپلود تصویر...');
            $http.post('/v1/PrivateTraining/SetServiceImage', fd, {
                headers: {
                    'Content-Type': undefined
                },
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        NotifyCustom('تصویر با موفقیت آپلود شد.', 'success');
                        $scope.model.service.image = response.image

                        $scope.model.imageUrl = ''
                        $scope.model.file = null
                    }
                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.model.editTag = function () {
            const model = {
                id: $scope.model.id,
                tagTitle: $scope.model.tagTitle,
                showTag: $scope.model.showTag,
            }

            waitingDialog.show('ویرایش برچسب سرویس...');
            $http.post('/v1/PrivateTraining/EditServiceTag', model)
                .success(function (response) {
                    if (response.result == 'done') {
                        NotifyCustom('ویرایش با موفقیت انجام شد', 'success');
                        $scope.model.service.tagTitle = $scope.model.tagTitle
                        $scope.model.service.showTag = $scope.model.showTag
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }
    }

}();
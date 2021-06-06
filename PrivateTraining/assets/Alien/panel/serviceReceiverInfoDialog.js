~function () {
    'use strict';

    angular.module("App").directive('serviceReceiverInfoDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/serviceReceiverInfoDialog.html?v=' + window.App.version,
            scope: {
            },
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    //http://api.sipars.ir/PrivateTrain/ServiceReceiverRequest/AddServiceReceiverRequest
    function controller($scope, $transclude, $element, $http) {
        $scope.model = {}

        $scope.data = {}

        $scope.$root.serviceReceiverInfoDialogShow = function (receiverId, status, buyServiceId) {
            waitingDialog.show("دریافت اطلاعات مشتری...")

            $http.post('/Account/LoadEditServiceReciever', { "serviceReceiverId": receiverId }).success(function (response) {
                if (response.Result) {
                    if (status != 0)
                        $scope.model.showContact = true;
                    else
                        $scope.model.showContact = false;

                    $scope.model.user = response.TempUser;
                    $scope.model.receiverId = receiverId;
                    $scope.model.buyServiceId = buyServiceId;
                }
            });

            window.openDialog('serviceReceiverInfoDialog')

        }
    }


}();

~function () {
    'use strict';

    window.app.directive('accountDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/dialog/accountDialog.html?v='+ window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    function controller($scope, $transclude, $element, $http) {

        $scope.model = {
            mode: 'login',
            submitTitle: 'ورود',
            username: '',
            password: '',
            activationCode: '',
            user: {}
        }

        $scope.data = {}


        $scope.$root.showAccountDialog = function () {
            $('#accountDialog').modal('show')

        }

        $scope.close = function () {
            $("#accountDialog").modal("hide");
        }

        $scope.completeSubmit = function (username) {
            const password = $scope.model.password

            if (password.length < 6) {
                return NotifyCustom("طول پسورد حداقل باید 6 باشد", 'danger');
            }
            if (password != $scope.model.password2) {
                return NotifyCustom("رمز های عبور با یکدیگر مطابقت ندارند!", 'danger');
            }


            waitingDialog.show("درحال تکمیل و ورود به حساب... لطفا شکیبا باشید.");
            const user = $scope.model.user
            const body = {
                username,
                password: password,
                name: user.name,
                family: user.family,
                sex: !!user.sex,
                email: user.email,
                activationCode: $scope.model.activationCode
            }
            $http.post('/v1/Account/CompleteCustomerAccount', body)
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.$root.user = response.user
                        window.localStorage.user = JSON.stringify(response.user)
                        //response.userId
                        $scope.close()
                    }
                    response.message && NotifyCustom(response.message, 'danger');
                })
                .finally(function () {
                    waitingDialog.hide();
                });
        }


        $scope.registerSubmit = function (username) {
            waitingDialog.show("درحال ساخت حساب... لطفا شکیبا باشید.");
            $http.post('/v1/Account/RegisterCustomer', {username})
                .success(function (response) {
                    if (response.result == 'needActive') {
                        //response.userId
                        NotifyCustom('کُد فعال سازی برای شما ارسال شده است. آن را برای فعال کردن حساب وارد نمایید.', 'success');
                        $scope.activeCodeMode()
                    }
                    response.message && NotifyCustom(response.message, 'danger');
                })
                .finally(function () {
                    waitingDialog.hide();
                });
        }

        $scope.activeSubmit = function (username) {
            waitingDialog.show("درحال فعال سازی حساب... لطفا شکیبا باشید.");
            $http.post('/v1/Account/ActiveCodeCustomer', {username, activationCode: $scope.model.activationCode})
                .success(function (response) {
                    if (response.result == 'done') {
                        //response.userId
                        NotifyCustom('حساب شما فعال شد. لطفاً رمز ورود برای حساب وارد کنید و اطلاعات حساب خود را کامل نمایید', 'success');

                        $scope.completeMode()
                    }
                    response.message && NotifyCustom(response.message, 'danger');
                })
                .finally(function () {
                    waitingDialog.hide();
                });
        }

        $scope.loginSubmit = function (username, password) {
            if (password.length < 6) {
                return NotifyCustom("طول پسورد حداقل باید 6 باشد", 'danger');
            }

            waitingDialog.show("درحال ورود به حساب... لطفا شکیبا باشید.");
            $http.post('/v1/Account/Login', {username, password})
                .success(function (response) {
                    if (response.result == 'done') {

                        window.App.handleUserLogin($scope.$root, response.user)
                        window.App.serviceWorker()

                        $scope.close()
                    }
                    response.message && NotifyCustom(response.message, 'danger');
                })
                .finally(function () {
                    waitingDialog.hide();
                });
        }


        $scope.forgotPasswordSubmit = function (username) {
            waitingDialog.show("درحال بررسی حساب... لطفا شکیبا باشید.");
            $http.post('/v1/Account/ForgotPassword', {username, mobile: username})
                .success(function (response) {
                    if (response.result == 'done') {
                        //response.userId
                        NotifyCustom('رمز ورود جدید به موبایل شما ارسال شد.', 'success');
                        $scope.loginMode()
                    }
                    response.message && NotifyCustom(response.message, 'danger');
                })
                .finally(function () {
                    waitingDialog.hide();
                });
        }


        $scope.submit = function () {
            const username = $scope.model.username
            const password = $scope.model.password

            switch ($scope.model.mode) {
                case 'register':
                    $scope.registerSubmit(username)
                    break
                case 'active':
                    $scope.activeSubmit(username)
                    break
                case 'complete':
                    $scope.completeSubmit(username)
                    break
                case 'forgotPassword':
                    $scope.forgotPasswordSubmit(username)
                    break

                case 'login':
                    $scope.loginSubmit(username, password)
                    break

            }

        }


        $scope.completeMode = function () {
            $scope.model.mode = 'complete'
            $scope.model.submitTitle = 'تکمیل اطلاعات حساب'
        }
        $scope.activeCodeMode = function () {
            $scope.model.mode = 'active'
            $scope.model.submitTitle = 'فعال سازی حساب'
        }

        $scope.loginMode = function () {
            $scope.model.mode = 'login'
            $scope.model.submitTitle = 'ورود'
        }


        $scope.registerMode = function () {
            $scope.model.mode = 'register'
            $scope.model.submitTitle = 'ساخت حساب'

        }

        $scope.forgotPasswordMode = function () {
            $scope.model.mode = 'forgotPassword'
            $scope.model.submitTitle = 'فراموشی رمز ورود'

        }

    }


}();

~function () {
    'use strict';

    angular.module("App").directive('chatDialog', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/panel/chatDialog.html?v=' + window.App.version,
            scope: {},
            controller: ['$scope', '$transclude', '$element', '$http', controller],

        }
    });


    function controller($scope, $transclude, $element, $http) {
        $scope.model = {
            description: ''
        }

        $scope.data = {chatList: []}

        $scope.$element = $element

        const chat = {
            id: 1,
            buyServiceId: 1,
            text: `سلام چتوری
خوبی؟
من امروز هستم`,
            type: 1,
            senderId: 1,
            receiverId: 2,

            date: '1398/12/07',
            time: '12:20:20',

        }


        $scope.$root.chatDialogShow = function (buy, data, $listScope) {

            $scope.model.title = buy.serviceTitle + ' کد: ' + buy.code
            $scope.data.buy = buy
            $scope.data.data = data
            $scope.$listScope = $listScope

            $scope.model.description = ''

            const customerUser = data.userList.find(u => u.id == buy.buyService.serviceReceiverId)
            const providerUser = data.userList.find(u => u.id == buy.buyService.serviceProviderId)

            $scope.model.senderId = -1

            if (data.userType == 'customer') {
                $scope.model.senderId = customerUser.id
                $scope.model.receiverId = providerUser.id
                $scope.model.name = buy.providerName
            } else {
                $scope.model.senderId = providerUser.id
                $scope.model.receiverId = customerUser.id
                $scope.model.name = buy.customerName
            }

            $scope.getChats()

            window.openDialog('chatDialog')
            // $('chatDialog').modal('show')
        }

        const initChatList = function () {

            $scope.model.chatList = $scope.data.chatList.map((chat, index) => {
                let senderType = $scope.model.senderId == chat.senderId ? 'sender' : 'receiver'

                let time = ''
                let date = ''
                const beforeChat = $scope.data.chatList[index - 1]

                if (beforeChat) {
                    if (beforeChat.date != chat.date) date = chat.date
                    if (beforeChat.time.substr(0, 5) != chat.time.substr(0, 5))
                        time = chat.time.substr(0, 5)
                }

                if (index == 0) {
                    date = chat.date
                }

                if (index == $scope.data.chatList.length - 1) {
                    time = chat.time.substr(0, 5)
                }

                return {
                    chatItem: chat,
                    sender: senderType == 'sender',
                    time: time,
                    date: date,
                }
            })

            requestAnimationFrame(() => {
                window.document.querySelector('#chatDialog .modal-body').scrollTop = 1000000
            })
        }

        $scope.getChats = function () {
            const buyServiceId = $scope.data.buy.buyService.id

            waitingDialog.show("درحال دریافت چت ها...");
            $http.post('/v1/PrivateTraining/GetChatList', {
                buyServiceId: buyServiceId,
                userType: $scope.data.data.userType
            })
                .success(function (response) {
                    if (response.result == 'done') {
                        $scope.data.chatList = response.chatList

                        const readCount = response.count

                        if ($scope.data.data.userType == 'customer') {
                            $scope.data.buy.buyService.chatReadCustomer = readCount
                            $scope.$listScope.updateBuyServiceList()
                        }

                        if ($scope.data.data.userType == 'provider') {
                            $scope.data.buy.buyService.chatReadProvider = readCount
                            $scope.$listScope.updateBuyServiceList()
                        }

                        initChatList()
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });
        }

        $scope.addChat = function (chatMsg) {
            if (!chatMsg) return

            waitingDialog.show("درحال دریافت اطلاعات...");

            let date = new Date().toLocaleDateString('fa-IR', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                formatMatcher: 'basic',
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit',
            })
            date = date.trim().replace("‏ ", '')

            let time = date.split('،')[1]
            time = window.toEnglishDigits(time)
            date = date.split('،')[0]
            date = window.toEnglishDigits(date)

            // $scope.model.receiverId == $scope.model.user.ServiceProviderId ? ''
            let type = 1


            const buyServiceId = $scope.data.buy.buyService.id

            const chat = {
                date,
                time,
                type,
                text: chatMsg.trim(),
                buyServiceId,
                senderId: $scope.model.senderId,
                receiverId: $scope.model.receiverId,

            }

            $http.post('/v1/PrivateTraining/AddChat', chat)
                .success(function (response) {
                    if (response.result == 'done') {
                        // response.message && NotifyCustom(response.message, 'danger');
                        $scope.data.chatList.push(response.chat)
                        initChatList()

                        // $scope.$listScope.updateBuyServiceList()
                    }

                    response.message && NotifyCustom(response.message, 'danger');
                }).finally(function () {
                waitingDialog.hide();
            });

            $scope.model.description = ''
        }

        if ('serviceWorker' in navigator && 'PushManager' in window) {
            navigator.serviceWorker.addEventListener('message', function (event) {
                const data = event.data.message

                if (window.isOpenDialog('chatDialog') && data.type == 'chat') {
                    const buyServiceId = $scope.data.buy.buyService.id
                    if (data.buyServiceId == buyServiceId) {
                        $scope.getChats()
                    }
                }
            });
        }


    }


}();

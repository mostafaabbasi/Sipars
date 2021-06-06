window.App = {}
window.App.Url = {
    base: window.location.origin + '/v1/'
}

//must-revalidate, max-age=36000

window.App.version = '.17'

window.App.init = ($rootScope) => {
    window.$rootScope = $rootScope
    window.rootScope = $rootScope
    $rootScope._window = window
    waitingDialog.create()

    window.App.initUser($rootScope).finally(window.App.serviceWorker)

    $rootScope.inUrl = function (url) {
        return window.location.pathname.toLowerCase().includes(url)
    }

    $rootScope.ngIfDialog = function ($scope) {
        return $scope.$element[0].querySelector('.modal').classList.contains('in')
    }

    setTimeout(function () {
        requestAnimationFrame(function () {
            const menuBar = document.querySelectorAll('.menuBar')[1]
            if (menuBar) {
                menuBar.firstElementChild.classList.remove('active')
                menuBar.firstElementChild.nextElementSibling.classList.add('active')
            }
        })
    })

    $rootScope.user = JSON.parse(window.localStorage.user || '{}')
    $rootScope.isLogin = function () {
        return $rootScope.user.id
    }

    $rootScope.model = {}
    $rootScope.setLocations = function (Approve, stateList, cityList, locationList) {
        Approve = Approve || $rootScope.Approve
        stateList = stateList || []
        cityList = cityList || []
        locationList = locationList || []

        if (!Approve) return
        $rootScope.model.city = cityList.find(city => city.Id == Approve.CityId)
        $rootScope.model.state = stateList.find(state => state.Id == Approve.StateId)
        $rootScope.model.location = locationList.find(location => location.Id == Approve.LocationId)
    }

    $rootScope.getServiceTitle = function (service) {
        if (!service) return ''

        let title = service.title || service.Title
        if (!$rootScope.model.menuList) return title
        let parent = $rootScope.model.menuList.find(s => s.id == (service.parentId || service.ParentId))
        if (parent) {
            title += ' ' + parent.title
        } else return title
        parent = $rootScope.model.menuList.find(s => s.id == parent.parentId)
        if (parent) title += ' ' + parent.title
        return title
    }

    $rootScope.closeThisDialog = ($event) => {
        let parent = $event.target.parentElement
        while (!parent.classList.contains('modal')) {
            parent = parent.parentElement
        }

        $(parent).modal("hide");
    }

    $rootScope.closeDialog = (id) => {
        $("#" + id).modal("hide");
    }

    $rootScope.getServiceImage = (image) => {
        if (!image) return '/assets/New/img/LogoC.png'
        return "/UserFiles/serviceImages/" + image
    }

    $rootScope.location = {
        showDialog: function () {
            $('#locationDialog').modal('show')
        }
    }

    $rootScope.getCurrencyHezar = function (currency, forceRayegan) {
        if (currency < 1000 || currency % 1000 !== 0) return $rootScope.getCurrency(currency, forceRayegan)
        return $rootScope.getCurrency(currency / 1000, forceRayegan).replace('تومان', 'هزار تومان')
    }

    $rootScope.getCurrency = function getCurrency(currency, forceRayegan) {
        if (currency === undefined || isNaN(currency)) {
            return 'نا مشخص';
        }
        if (!currency) {
            if (!forceRayegan) return '0 تومان'
            return 'رایگان';
        }

        // let dot = currency.toString().split('.')[1] || ''
        let dot = ''
        currency = currency.toString().split('.')[0]

        return currency.toString().replace(/./g, function (c, i, a) {
            return i && c !== "." && ((a.length - i) % 3 === 0) ? ',' + c : c;
        }) + dot + ' تومان';
    }

    $rootScope.searchDebounce = {updateOn: "default blur", debounce: {default: 750, blur: 0}}
    $rootScope.inputDebounce = {updateOn: "default blur", debounce: {default: 350, blur: 0}}

    window.toEnglishDigits = function (str) {
        let id = {'۰': '0', '۱': '1', '۲': '2', '۳': '3', '۴': '4', '۵': '5', '۶': '6', '۷': '7', '۸': '8', '۹': '9'};
        return str.replace(/[^0-9.]/g, function (w) {
            return id[w] || w;
        });
    };

    window.toPersianDigits = function (str) {
        let id = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
        return str.replace(/[0-9]/g, function (w) {
            return id[+w]
        });
    };

}

window._historyState = []
window._dialogHistory = []
window.isOpenDialog = function (id) {
    if (!id.startsWith('#')) id = '#' + id
    return document.querySelector(id + '.in')
}

window.openDialog = function (id) {

    const name = id
    if (!id.startsWith('#')) id = '#' + id
    $(id).modal('show')

    $(id).off('hidden.bs.modal')

    $(id).on('hidden.bs.modal', function () {
        // do something…
        if (window.history.state && window.history.state.dialog == name) {
            window.history.back()
        }

    });

    window._historyState.push({dialog: name})

    window.history.pushState({dialog: name}, null, '?' + name)
}

window.onpopstate = function (e) {
    const state = window._historyState.pop()
    if (state && state.dialog) {
        document.querySelectorAll('.modal.in').forEach(node => {
            if (node.id && node.id !== 'loading') {
                $(node).modal('hide')
            }
        })

    }
};

window.App.handleUserLogin = function ($rootScope, user) {
    window.App.isLogin = true
    window.App.user = user

    if ($rootScope) {
        $rootScope.user = user
    }

    window.localStorage.user = JSON.stringify(user)
}

window.App.initUser = function ($rootScope) {

    return fetch('/v1/Account/isLogin').then(response => {
        if (response.status == 200) {
            return response.json()
        }
    }).then(result => {
        if (result.result == 'done' && result.user) {
            window.App.handleUserLogin($rootScope, result.user)
            return result.user
        }
        return result
    })
}

const applicationServerPublicKey = 'BNkHnP2i4eiINK_Rje0dsMZX5wOd3WT51hziIyA-lmtf8vnmuJadPgcIYWlVB9G1cG5OMKa0hjZnS6dftPLQ1iE';
const applicationServerPrivateKey = 'R_BFNAJDIASDUwijewjeiou34u3u9eifisdfjsidofu84ur89';

//notification

function urlB64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');

    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

window.App.serviceWorker = function () {

    if ('serviceWorker' in navigator && 'PushManager' in window) {
        console.log('Service Worker and Push is supported');
        console.log(Notification.permission)

        if (Notification.permission === 'denied') {
            return
        }

        navigator.serviceWorker.register('/assets/Alien/sw.js')
            .then(function (swReg) {
                console.log('Service Worker is registered', swReg);

                if (!window.App.isLogin) return

                //only login

                swReg.pushManager.getSubscription()
                    .then(function (subscription) {
                        let isSubscribed = !(subscription === null);

                        const subscriptionJson = window.localStorage['subscription']
                        const subscriptionId = window.localStorage['subscriptionId']


                        if (isSubscribed && subscriptionJson && subscriptionId && window.App.user.id == subscriptionId && window.App.user.subscription) {
                            console.log('User IS subscribed.');
                            //its ok

                        } else {
                            console.log('User is NOT subscribed.');
                            // const confirm = window.confirm("تمایل به دریافت اطلاع رسانی (نوتیفیکیشن) در مرورگر خود دارید؟")
                            //
                            // if (!confirm) return

                            const applicationServerKey = urlB64ToUint8Array(applicationServerPublicKey);
                            swReg.pushManager.subscribe({
                                userVisibleOnly: true,
                                applicationServerKey: applicationServerKey
                            })
                                .then(function (subscription) {
                                    console.log('User is subscribed.');

                                    fetch('/v1/Account/subscribe', {
                                        method: 'POST',
                                        headers: {
                                            'Accept': 'application/json',
                                            'Content-Type': 'application/json'
                                        },
                                        body: JSON.stringify({
                                            userId: window.App.user.id,
                                            subscription: JSON.stringify(subscription),
                                            oldUserId: window.App.user.id == subscriptionId ? undefined : subscriptionId
                                        })
                                    }).then(response => {
                                        if (response.status == 200) {
                                            return response.json()
                                        }
                                    }).then(result => {
                                        if (result.result == 'done') {
                                            window.localStorage['subscription'] = JSON.stringify(subscription)
                                            window.localStorage['subscriptionId'] = window.App.user.id
                                        }
                                    })


                                })
                                .catch(function (err) {
                                    console.log('Failed to subscribe the user: ', err);
                                });

                        }

                    });

            })
            .catch(function (error) {
                console.error('Service Worker Error', error);
            });
    } else {
        console.warn('Push messaging is not supported');
    }
}

window.App.serviceWorker()

//{"endpoint":"https://fcm.googleapis.com/fcm/send/fBeuPN5P9zk:APA91bGgHRS8BcOJSDXsWvWxdEpkeGcIzyNQSq3RT7O5-4KHnfrUL1eqNMCS7YaLcxyoYho_Uf5Ud7-flTxN3ZgaEslq1HAYL1sra9ctq1O3uftnGUCqvDVR1Qf6e8FzppQWmUlCSynM","expirationTime":null,"keys":{"p256dh":"BH5-bx5yl19JthEVnmkM1AtQWT6EhgJ-StLlGrdtXBgtoN5iLj2d7Oiwy5ujaZFFFVrHes8F6KsGp4zVV172XtU","auth":"VuIMGcC_1obKfEmYmIufYA"}}

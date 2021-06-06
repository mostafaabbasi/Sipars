~function () {
    'use strict';

    window.app.directive('serviceCard', function () {
        return {
            restrict: 'E',
            // transclude: true,
            // priority: 700,
            templateUrl: '/assets/Alien/serviceCardTemplate.html?v='+ window.App.version,
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
            searchServiceList: [],
            tags: {},
        }

        $scope.data = {
            serviceList: [],
            searchList: [],
        }

        $scope.getTagKeys = function () {
            return Object.keys($scope.model.tags)
        }

        $scope.setTagList = function () {
            const list = $scope.data.serviceList
            const tags = {}
            list.forEach(s => {
                if (s.showTag && s.tagTitle) {
                    tags[s.tagTitle] = tags[s.tagTitle] || []
                    tags[s.tagTitle].push(s)
                }
            })

            $scope.model.tags = tags
        }

        $scope.changeSearch = function () {

            const list = $scope.data.serviceList

            if ($scope.data.searchList.length == 0) {

                const tempSearchList = []

                const add = function (service, title) {
                    let callAdd = false
                    list.filter(s => s.parentId == service.id).forEach(s => {
                        callAdd = 1
                        add(s, title + " " + s.title)
                    })

                    if (!callAdd) {
                        tempSearchList.push({
                            id: service.id,
                            title: title + " " + service.title,
                        })
                    }
                }

                list.filter(s => s.parentId == 0).forEach(s => {
                    add(s, s.title)
                })
                $scope.data.searchList = tempSearchList
            }

            const text = $scope.model.search
            const textList = text.split(' ')
            const searchList = $scope.data.searchList
            const result = searchList.filter(s => textList.find(t => s.title.includes(t)))
            $scope.model.searchServiceList = list.filter(s => result.find(r => r.id == s.id)).slice(0, 25)
            $scope.model.searchServiceTitles = {}
            $scope.model.searchServiceList.forEach(s => {
                const parent = list.find(service => service.id == s.parentId) || {}
                const parent2 = list.find(service => service.id == parent.parentId) || {}
                $scope.model.searchServiceTitles[s.id] = parent2.title + ' ' + parent.title + ' ' + s.title
            })
            console.log(result)
        }

        $http.get(window.App.Url.base + 'privateTraining/MenuList', {}).success(function (response) {
            if (response.result == "done") {
                $scope.$root.model.menuList = $scope.data.serviceList = response.items
                $scope.setTagList()
                $scope.setServiceTypes()
                $scope.setServiceList()
            }
        });

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


        $scope.showIcon = function () {
            // return 1
            return $scope.model.selectedService.length <= 3
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

        $scope.canBack = function () {
            return $scope.model.selectedService.length > 1
        }

        $scope.back = function () {
            const backService = $scope.model.selectedService.slice(-2)[0]
            return backService && $scope.onServiceClick(backService)
        }

        $scope.onServiceClick = function (service) {
            $scope.selectService(service)
        }

        $scope.buyService = function (service) {
            // const parentScope = $scope.getParentScope()
            // parentScope.$$childTail.Approve.ServiceId = service.id
            // parentScope.ShowServiceProvider(service.id)
            $scope.$root.Approve.service = service

            $scope.$root.siparsProviderDialogShow(service)
            // $scope.$root.showRegisterDialog(service)
        }

        $scope.getParentScope = function () {
            let scope = $scope
            while (!scope.ShowServiceProvider) {
                scope = scope.$parent
            }
            return scope
        }

        //----------

        $scope.getServiceDescription = function () {
            const lastService = $scope.model.selectedService.slice(-1)[0]
            return lastService && lastService.serviceDescription
        }
    }


}();
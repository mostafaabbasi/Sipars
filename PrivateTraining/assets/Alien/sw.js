/*
*
*  Push Notifications codelab
*  Copyright 2015 Google Inc. All rights reserved.
*
*  Licensed under the Apache License, Version 2.0 (the "License");
*  you may not use this file except in compliance with the License.
*  You may obtain a copy of the License at
*
*      https://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License
*
*/

/* eslint-env browser, serviceworker, es6 */

'use strict';

self.addEventListener('push', function(event) {
    console.log('[Service Worker] Push Received.');
    const data = JSON.parse(event.data.text() || '{}')
    console.log(`[Service Worker] Push had this data: `, data);

    const title = data.title;
    const options = {
        body: data.body,
        icon: '/assets/New/img/LogoC.png',
        data
    };

    const examplePage = '/panel/service/list/1';
    const urlToOpen = new URL(examplePage, self.location.origin).href;

    const promiseChain = clients.matchAll({
        type: 'window',
        includeUncontrolled: true
    }).then((windowClients) => {
        let matchingClient = null;

        for (let i = 0; i < windowClients.length; i++) {
            const windowClient = windowClients[i];
            if (windowClient.url.includes(urlToOpen)) {
                matchingClient = windowClient;

                windowClient.postMessage({
                    message: data,
                    time: new Date().toString()
                });

            }
        }
    });

    event.waitUntil(self.registration.showNotification(title, options));
});

self.addEventListener('notificationclick', function(event) {
    const clickedNotification = event.notification;
    clickedNotification.close();

    // Do something as the result of the notification click
    const examplePage = '/panel/service/list/1';
    const urlToOpen = new URL(examplePage, self.location.origin).href;

    const promiseChain = clients.matchAll({
        type: 'window',
        includeUncontrolled: true
    }).then((windowClients) => {
        let matchingClient = null;

        for (let i = 0; i < windowClients.length; i++) {
            const windowClient = windowClients[i];
            if (windowClient.url.includes(urlToOpen)) {
                matchingClient = windowClient;
                break;
            }
        }

        if (matchingClient) {
            return matchingClient.focus();
        } else {
            return clients.openWindow(urlToOpen);
        }
    });

    event.waitUntil(promiseChain);
});

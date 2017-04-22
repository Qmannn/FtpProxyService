((): void => {
    'use strict';
    angular.module('app', ['app.core',
        'app.routes',
        'app.services',
        'app.navbar',
        'app.pageControllers'
    ]);
})(); 
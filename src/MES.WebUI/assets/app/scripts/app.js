requirejs.config({
    //By default load any module IDs from js/lib
    baseUrl: '/assets/app/scripts/vendor',
    //except, if the module ID starts with "app",
    //load it from the js/app directory. paths
    //config is relative to the baseUrl, and
    //never includes a ".js" extension since
    //the paths config could be for a directory.
    paths: {
        app: '../app',
        lib: '../shared',
        bootstrap: '../../../../Scripts/bootstrap.bundle',        
        jquery: '../../../../Scripts/jquery-3.3.1',
        bootbox: 'bootbox',
        modal: '../shared/modal',
        'Culture.NotifyLocals.locals': '../shared/notifyLocals',
        lodash: 'lodash',
        select2: 'select2/js/select2',
        //moment: 'moment.js/moment-with-locales',
        'bootstrap-datetime': 'tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4',
        'jquery-ui': 'jquery-ui', 
        notify: '../shared/bootstrap-notify'
    },
    packages: [{
        name: 'moment',
        // This location is relative to baseUrl. Choose bower_components
        // or node_modules, depending on how moment was installed.
        location: 'moment.js',
        main: 'moment'
    }],
    shim: {        
        'bootstrap': {
            'deps': ['jquery']
        },
        'bootbox': {
            'deps': ['jquery', 'bootstrap']
        },
        'bootstrap-datetime': {
            'deps': ['jquery', 'moment']
        },
        'notify': {
            'deps': ['jquery', 'lodash', 'bootstrap']
        },
    }
});

// Start the main app logic.
requirejs([], function () {
        //jQuery, canvas and the app/sub module are all
        //loaded and can be used here now.
        // modal.init();
});
define(['jquery', 'bootbox', 'lodash','jquery-ui'], function ($, bootbox, _) {

    function initModalOnEvent(callback, options) {
        // callback (optional)
        // options.parentToWatch (required)
        // options.siblingTrigger (requried)
        options = options || {};

        // shorten references into memory
        var parentToWatch = options.parentToWatch && options.parentToWatch.length > 0 ? $(options.parentToWatch) : null,
            triggerEvent = options.triggerEvent || 'click',
            siblingTrigger = options.siblingTrigger;

        if (parentToWatch != null || parentToWatch != undefined)
            parentToWatch.on(triggerEvent, siblingTrigger, _.partial(handleCustomEventForOpeningModal, options, callback));

        return true;
    }

    function handleCustomEventForOpeningModal(options, callback, event, args) {
        event.preventDefault();
        args = args || {};

        var $modalMainContent = baseInit(options);

        if (callback) {
            callback($modalMainContent.data('modalInstance'), options, args);
        }
    }

    function baseInit(options) {
        // options.title (optional)
        // options.isCloseButtonVisible (optional)

        // shorten references into memory
        var title = options.title || '',
            uniqueModalName = getUniqueModalName(),
            isCloseButtonVisible = showCloseButton(options.isCloseButtonVisible),
            size = options.size || 'medium';

        // initialize notification container inside modal
        // notify.init({ container: '#modalNotifyContainer' });

        // initialize dialog
        var $modalWrapper = bootbox.dialog({
            title: title,
            message: '<div id="' + uniqueModalName + '">loading...</div>',
            className: 'clearfix',
            closeButton: isCloseButtonVisible,
            buttons: options.buttons,
            size: size
        });

        var $modalMainContent = $('#' + uniqueModalName);

        // save data related to the object for future reference
        $modalMainContent.data('modalInstance', {
            wrapper: $modalWrapper,
            element: $modalMainContent,
            hide: _.partial(hideModal, $modalWrapper)
        });

        return $modalMainContent;
    }

    function hideModal($modalWrapper) {
        // $modalWrapper (required)

        $modalWrapper.modal('hide');
    }

    function initStandardModal(modalInstance, options, args) {
        // options.getId (required if no options.getData)
        // options.getData (required if no options.getId)
        // options.getDataUsingContentType (required if no options.getId and content type need to be json)
        // options.getUrlData (optional)
        // options.baseUrl (required)
        // options.modalErrorMessage (optional)

        var ajaxOptions = {
            url: options.baseUrl,
            type: 'GET',
            cache: false,
            success: _.partial(onSuccessContent, modalInstance, options),
            error: _.partial(onErrorContent, modalInstance, options.modalErrorMessage)
        };

        // getId functionality is temporarily retained so that pages which are currently using this method won't break. This will be deleted when getData is implemented in all the screens.
        if (options.getId) {
            ajaxOptions.url += options.getId(event);
        }

        if (options.getData) {
            ajaxOptions.type = 'POST';
            ajaxOptions.data = options.getData(event);
        }

        if (options.getDataUsingContentType) {
            ajaxOptions.type = 'POST';
            ajaxOptions.contentType = 'application/json; charset=utf-8';
            ajaxOptions.data = options.getDataUsingContentType(event);
        }
        if (options.getUrlData) {
            ajaxOptions.data = options.getUrlData(event, args);

            // set cache options
            if (options.cacheAjaxCall === true) {
                ajaxOptions.cache = true;
            } else {
                ajaxOptions.data.cacheBuster = new Date().getTime() / 1000;
            }
        }

        $.ajax(ajaxOptions);
    }

    function onErrorContent(modalInstance, errorMessage, xhr, textStatus, errorThrown) {
        modalInstance.element.text(errorMessage + errorThrown);
    }

    function onSuccessContent(modalInstance, options, content) {
        var onContentSuccess = options.onContentSuccess || $.noop,
            formOptions = options.formOpts || {},
            onFormSuccess = formOptions.successCallback || $.noop;

        $('.modal-body').addClass('clearfix');

        makeModalDraggable();

        // insert content to modal
        modalInstance.element.html(content);

        // run OnContentSuccess
        onContentSuccess(modalInstance);

        // init form inside modal
        // formInitr.init();

        // on form success close modal
        formOptions.successCallback = function (data) {
            onFormSuccess(data);
            modalInstance.hide();
        }

        // fs.listenForSubmit(modalInstance.element.find('form'), formOptions);
    }

    function confirm(title, message, callback) {
        var uniqueModalName = getUniqueModalName();
        //notify.init({
        //    container: '#modalNotifyContainer'
        //});
        bootbox.dialog({
            title: title,
            message: message,
            className: uniqueModalName,
            buttons: {
                success: {
                    label: locals.values.yesBtnText,
                    callback: function () {
                        callback.call(this, true);
                    }
                },
                cancel: {
                    label: locals.values.noBtnText,
                    callback: function () {
                        callback.call(this, false);
                    }
                }
            },
        });

        makeModalDraggable();
    }

    function getUniqueModalName() {
        return 'modal-box-' + ($.guid++);
    }

    function showCloseButton(isCloseButtonVisible) {
        return typeof isCloseButtonVisible !== 'undefined' ? isCloseButtonVisible : true;
    }

    function makeModalDraggable() {
        $('.modal-content').draggable({
            handle: '.modal-header',
            cursor: 'move'
        });
    }

    return {
        init: _.partial(initModalOnEvent, initStandardModal),
        initModalOnEvent: initModalOnEvent,
        baseInit: baseInit,
        hideModal: hideModal,
        confirm: confirm
    };
});

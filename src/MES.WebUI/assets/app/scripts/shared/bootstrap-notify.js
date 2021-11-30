define(['jquery', 'lodash', 'Culture.NotifyLocals.locals', 'bootstrap'], function ($, _, locals) {

    var $container;

    var defaults = {
        container: '#notify-container',
        fadeOutDelay: 3500
    };

    function init(opts) {
        defaults = _.assign(defaults, opts);
        $container = $(defaults.container);
    }

    function render(notification) {

        $container = $(defaults.container);

        if ($container.length) {
            notification.message = notification.message.replace(/\n/g, "<br />");
            var alert = $(templates.notifyTemplate(notification));

            if (notification.shouldFloat) {
                $container.addClass('notify-container');
            } else {
                $container.removeClass('notify-container');
            }
            var containerParentId = $container.parent().attr('id');

            if (containerParentId != undefined && containerParentId.indexOf('modal-box') != -1)
                $container.attr('style', 'position: relative');

            if (notification.sticky) {
                alert.addClass('alert-sticky');
            }

            applyFade(notification, alert);

            $container.append(alert);
        }
    }

    function success(notification) {
        render(_.assign({
            title: locals.values.successTitle,
            message: '',
            cssClass: 'alert-success',
            icon: 'fa-check-circle-o',
            shouldFade: true,
            sticky: true,
            shouldFloat: true
        }, notification));
    }

    function warn(notification) {
        render(_.assign({
            title: locals.values.warnTitle,
            message: '',
            cssClass: 'alert-warning',
            icon: 'fa-exclamation-triangle',
            sticky: true,
            shouldFloat: true
        }, notification));
    }

    function warnAfterSuccess(notification) {
        render(_.assign({
            title: locals.values.warnTitle,
            message: '',
            cssClass: 'alert-warning after-success',
            icon: 'fa-exclamation-triangle',
            sticky: true,
            shouldFloat: true
        }, notification));
    }

    function error(notification) {
        render(_.assign({
            title: locals.values.errorTitle,
            message: '',
            cssClass: 'alert-danger',
            icon: 'fa-exclamation-circle',
            sticky: true,
            shouldFloat: true
        }, notification));
    }

    function info(notification) {
        render(_.assign({
            title: locals.values.infoTitle,
            message: '',
            cssClass: 'alert-info',
            icon: 'fa-info-circle',
            sticky: true,
            shouldFloat: true
        }, notification));
    }

    function validationSummary(notification) {
        $container = $(defaults.container);

        function createSuccessContainer(template) {
            var successContainer = '';
            if (notification.success && notification.success.length) {
                var successOptions = {
                    title: locals.values.successTitle,
                    messages: notification.success,
                    cssClass: 'alert-success',
                    icon: 'fa-check-circle-o',
                    shouldFade: true,
                    sticky: false,
                    shouldFloat: false
                };

                successContainer = $(template(successOptions));
                if (notification.isClient)
                    successContainer.addClass('client');
                applyFade(successOptions, successContainer);
            }
            return successContainer;
        }

        function createErrorContainer(template) {
            var errorContainer = '';
            if (notification.errors && notification.errors.length) {
                var errorOptions = {
                    title: locals.values.errorSummaryTitle,
                    messages: notification.errors,
                    cssClass: 'alert-danger',
                    icon: 'fa-exclamation-circle',
                    sticky: false,
                    shouldFloat: false
                };

                errorContainer = $(template(errorOptions));
                if (notification.isClient)
                    errorContainer.addClass('client');
                applyFade(errorOptions, errorContainer);
            }
            return errorContainer;
        }

        function buildValidation(notification) {
            if (notification.success) {
                $container
                    .append(createSuccessContainer(templates.baseModalTemplate));
            }

            if (notification.isClient)
            $container
                .append(createErrorContainer(templates.baseModalTemplate));
            else
                $container
                    .empty()
                .append(createErrorContainer(templates.baseModalTemplate));

            if (notification.warnings && notification.warnings.length) {
                $container.append(createWarningsContainer(templates.baseModalTemplate));
            }

            if (notification.warningsAfterSuccess) {
                $container.append(createWarningsAfterSuccessContainer(templates.warningAfterSuccessSummaryTemplate));
            }
        }

        function removeSaveAnyway(e) {
            var $form = $(e.target).closest('form');
            $('.save-anyway-wrapper').remove();

            $form.find("input:not('.keepReadonly'), textarea").removeAttr('readonly');
            $form.find('.select2-container').select2('readonly', false);
            $form.find('.btn').show();
            buildValidation(e.data);
            var btnNoError = $form.find(".save-anyway-no-error");
            if (btnNoError.length > 0) {
                $form.find(".save-anyway-no-error").trigger('click');
            }
        }

        function createWarningsContainer(template) {
            var warningContainer = '';
            if (notification.warnings && notification.warnings.length) {
                var warningOptions = {
                    title: locals.values.warningSummaryTitle,
                    preamble: locals.values.preamble,
                    messages: notification.warnings,
                    summary: locals.values.summary,
                    cssClass: 'alert-warning',
                    icon: 'fa-exclamation-triangle',
                    sticky: false,
                    shouldFloat: false
                };

                warningContainer = $(template(warningOptions));
                applyFade(warningOptions, warningContainer);
            }
            return warningContainer;
        }

        function createWarningsAfterSuccessContainer(template) {
            var warningContainer = '';
            if (notification.warningsAfterSuccess.elements && notification.warningsAfterSuccess.elements.length) {
                var warningOptions = {
                    title: locals.values.warningSummaryTitle,
                    preamble: notification.warningsAfterSuccess.preamble,
                    messages: notification.warningsAfterSuccess.elements,
                    summary: locals.values.summary,
                    cssClass: 'alert-warning after-success',
                    icon: 'fa-exclamation-triangle',
                    sticky: false,
                    shouldFloat: false,
                    shouldFade: false
                };

                warningContainer = $(template(warningOptions));
                applyFade(warningOptions, warningContainer);
            }
            return warningContainer;
        }

        function buildSaveAnyway() {
            var $form = $container.parent().find('form');

            $form.find('input, textarea').attr('readonly', 'readonly');
            $form.find('.select2-container').select2('readonly', true);
            $form.find('.btn').hide();

            var warningInfo = $(templates.warningAccpetanceTemplate({
                warningCommentLbl: locals.values.saveAnywayWarningCommentLabel,
                saveTxt: locals.values.saveAnywaySubmitButtonText,
                backTxt: locals.values.saveAnywayBackButtonText
            }));

            $container.append(createWarningsContainer(templates.warningSummaryTemplate));

            var $warningWrapper = $('<div>')
                .addClass('save-anyway-wrapper')
                .addClass('col-md-12')
                .append(warningInfo);

            $warningWrapper.find('.save-anyway-back')
                .click(notification, removeSaveAnyway);

            $form.find('.save-anyway-wrapper').remove();
            $form.append($warningWrapper);

            // set flag to notify back-end that no warning was specified
            var handleWarningCommentChange = function (event) {                
                $form.find('[name=checkWarningComment]').val(!event.target.value.trim());
            };
            $form.find('[name=warningComment]').on('change', handleWarningCommentChange).trigger('change');
            $form.find('[name=ignoreWarnings]').val(true);
        }
            
        function buildWarningsAfterSuccess() {
            $container.append(createWarningsAfterSuccessContainer(templates.warningAfterSuccessSummaryTemplate));
        }

        if ($container.length) {
            if ((notification.success && notification.success.length) || (notification.errors && notification.errors.length) || notification.useSimpleWarningSummary) {
                buildValidation(notification);
            }
            else if (notification.warningsAfterSuccess && notification.warningsAfterSuccess.elements && notification.warningsAfterSuccess.elements.length) {
                buildWarningsAfterSuccess();
            }
            else {
                buildSaveAnyway();
            }
        }
    }

    function closeAll() {
        $container = $(defaults.container);

        if ($container) {
            $container.find('button.close').click();
        }
    }

    function close() {
        $container = $(defaults.container);
        if ($container) {
            $container.find('.client').find('button.close').click();
        }
    }

    function reload() {
        $container = $(defaults.container);
        $container.empty();
    }

    function stopClickPropogationOnCheckbox() {
        $('input[type=checkbox]').click(function () {
            if (event.stopPropagation) {    // standard
                event.stopPropagation();
            } else {    // IE6-8
                event.cancelBubble = true;
            }
        });
    }

    function applyFade(notification, alert) {
        if (notification.shouldFade) {
            setTimeout(function () {
                alert.fadeOut({
                    complete: function () {
                        $(this).remove();
                    }
                });
            }, notification.fadeOutDelay || defaults.fadeOutDelay);
        }
    }

    var templates = function () {
        return {
            notifyTemplate: _.template(
                '<div class="alert alert-dismissible <%= cssClass %>">'
                   + '<button type="button" class="close" data-dismiss="alert">×</button>'
                   + '<h4 class="alert-icon">'
                   + '<span class="fa <%= icon %>"></span>'
                   + '</h4>'
                   + '<h4><%= title %></h4>'
                   + '<p><%= message %></p>'
               + '</div>'),
            baseModalTemplate: _.template(
                '<div class="alert alert-dismissible <%= cssClass %>">'
                    + '<button type="button" class="close" data-dismiss="alert">×</button>'
                    + '<h4 class="alert-icon">'
                    + '<span class="fa <%= icon %>"></span>'
                    + '</h4>'
                    + '<h4><%= title %></h4>'
                    + '<p><ul><% messages.forEach(function(message) { %><li><%= message.ErrorMessage || message %></li><% }); %></ul></p>'
                + '</div>'),
            warningSummaryTemplate: _.template(
                '<div class="alert alert-dismissible <%= cssClass %> save-anyway-wrapper">'
                    + '<button type="button" class="close" data-dismiss="alert">×</button>'
                    + '<h4 class="alert-icon"  >'
                        + '<span class="fa <%= icon %>"></span>'
                    + '</h4>'
                    + '<h4><%= title %></h4>'
                    + '<p><%= preamble %></p>'
                    + '<ul><% messages.forEach(function(message) { %><li><%= message.ErrorMessage %></li><% }); %></ul>'
                    + '<p><%= summary %></p>'
                + '</div>'),
            warningAfterSuccessSummaryTemplate: _.template(
                '<div class="alert alert-dismissible <%= cssClass %>">'
                    + '<button type="button" class="close" data-dismiss="alert">×</button>'
                    + '<h4 class="alert-icon"  >'
                        + '<span class="fa <%= icon %>"></span>'
                    + '</h4>'
                    + '<h4><%= title %></h4>'
                    + '<p><%= preamble %></p>'
                    + '<ul><% messages.forEach(function(message) { %><li><%= message.errorMessage %></li><% }); %></ul>'
                + '</div>'),
            warningAccpetanceTemplate: _.template(
                '<div class="form-group required">'
                    + '<label class="control-label"><%= warningCommentLbl %></label>'
                    + '<textarea id="warningComment" name="warningComment" class="form-control"></textarea>'
                    + '<input type="hidden" name="ignoreWarnings" />'
                    + '<input type="hidden" name="checkWarningComment" />'
                + '</div>'
                + '<div class="text-center">'
                    + '<button type="button" class="btn btn-default save-anyway-back"><%= backTxt %></button> '
                    + '<button type="submit" class="btn btn-primary save-anyway-submit"><%= saveTxt %></button>'
                + '</div>')
        };
    }();

    return {
        stopClickPropogationOnCheckbox: stopClickPropogationOnCheckbox,
        success: success,
        error: error,
        warn: warn,
        warnAfterSuccess:warnAfterSuccess,
        info: info,
        validationSummary: validationSummary,
        closeAll: closeAll,
        close:close,
        reload: reload,
        init: init
    };
});

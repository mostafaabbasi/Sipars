function NotifyCustom(message, theme) {

    if (!message) return
    toastr.options.extendedTimeOut = 0; //1000;
    toastr.options.timeOut = 5000;
    toastr.options.closeButton = true;

    if (theme == 'success') {
        toastr.options.positionClass = 'toast-bottom-left';
        toastr.options.iconClass = 'fa-check toast-' + theme;
    } else if (theme == 'danger') {
        toastr.options.positionClass = 'toast-top-full-width';
        toastr.options.iconClass = 'fa-times toast-' + theme;
    }


    toastr['custom'](message);
}


var waitingDialog = waitingDialog || (function ($) {
    'use strict';

    // Creating modal dialog's DOM
    var $dialog = $(
        '<div id="loading" class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="display: flex">' +
        '<div class="modal-dialog modal-m" style="max-width: 425px;width: 94% !important; margin: auto">' +
        '<div class="modal-content">' +
        '<div class="modal-header"><h5 style="margin:0;"></h5></div>' +
        '<div class="modal-body">' +
        '<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
        '</div>' +
        '</div></div></div>');

    return {
        /**
         * Opens our dialog
         * @param message Custom message
         * @param options Custom options:
         *                  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
         *                  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
         */
        show: function (message, options) {
            if (!this.created) {
                this.create(message, options)
            }

            $dialog.find('h5').text(message);
            $dialog.show()
            $('.modal-backdrop.fade.in').show()
        },

        created: false,
        create: function (message, options) {
            if (this.created) return

            this.created = true
            // Assigning defaults
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'Loading';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // This callback runs after the dialog was hidden
            }, options);

            // Configuring dialog
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h5').text(message);
            // Adding callbacks
            if (typeof settings.onHide === 'function') {
                $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($dialog);
                });
            }
            // Opening dialog
            $dialog.modal();
            setTimeout(() => {
                requestAnimationFrame(() => {
                    waitingDialog.hide()
                })
            })
        },
        /**
         * Closes dialog
         */
        hide: function () {
            $dialog.hide()
            $('.modal-backdrop.fade.in').hide()
        }
    };

})(jQuery);
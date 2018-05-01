(function ($) {
    $.validator.addMethod('requiredif', function (value, element, params) {
        /*var inAppPurchase = $('#InAppPurchase').is(':checked');

        if (inAppPurchase) {
            return true;
        }

        return false;*/
        
         
        var isChecked = $(param).length>0;

        if (isChecked) {
            return false;
        }

        return true;
    }, '');

    //$.validator.unobtrusive.adapters.add('requiredif', ['param'], function (options) {
    //    options.rules["requiredif"] = '#' + options.params.param;
    //    options.messages['requiredif'] = options.message;
    //});
    $.validator.unobtrusive.adapters.add("requiredif", ["param"], function (options) {
        console.log(options.params);
        options.rules["requiredif"] = "#" + options.params.param; // mvc html helpers
        options.messages["requiredif"] = options.message;

        
    });
})(jQuery);
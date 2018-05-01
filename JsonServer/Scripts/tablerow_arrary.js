(function ($) {
    $.extend({
        toDictionary: function (query) {
            var parms = {};
            var items = query.split("&"); // split
            for (var i = 0; i < items.length; i++) {
                var values = items[i].split("=");
                var key = decodeURIComponent(values.shift());
                var value = values.join("=")
                parms[key] = decodeURIComponent(value);
            }
            return (parms);
        }
    })
})(jQuery);


(function ($) {
    $.fn.serializeFormJSON = function () {
        var o = [];
        $(this).find('tr').each(function () {
            var elements = $(this).find('input, textarea, select')
            console.log($(this));
            if (elements.size() > 0) {
                var serialized = $(this).find('input, textarea, select').serialize();
                var item = $.toDictionary(serialized);
                o.push(item);
            }
        });
        return o;
    };
})(jQuery);
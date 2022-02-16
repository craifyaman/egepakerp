var Global = function () {

    var init = function () {

        $('.selectpicker').selectpicker();

        $('[data-toggle="tooltip"]').tooltip();

        $(document).on("change", ".globalSwitch", function (event) {
            event.preventDefault();
            if ($(this).is(":checked")) {
                $(this).val(true);
            } else {
                $(this).val(false);
            }
        });


    };

    var cardTemplate = function (str) {
        var html = '<div class="card card-custom example example-compact">';
        html += '<div class="card-body">';
        html += str;
        html += '</div>';
        html += '</div>';

        return html;
    }

    return {
        // public functions
        init: function () {
            init();
        },
        cardTemplate: function (str) {
            return cardTemplate(str);
        }
    };
}();

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

        $(".defaultSelect2").select2();

        /*
        // input group and left alignment setup
        $('.dateRangePicker').daterangepicker({
            buttonClasses: ' btn',
            applyClass: 'btn-primary',
            cancelClass: 'btn-secondary',
            locale: {
                "direction": "ltr",
                "format": "DD/M M/YYYY",
                "separator": " - ",
                "applyLabel": "Uygula",
                "cancelLabel": "İptal",
                "fromLabel": "Da",
                "toLabel": "A",
                "customRangeLabel": "Personalizzata",
                "daysOfWeek": [
                    "pzt",
                    "Sl",
                    "Çrş",
                    "Prş",
                    "Cm",
                    "Cmt",
                    "Pz"
                ],
                "monthNames": [
                    "Ocak",
                    "Şubat",
                    "Mart",
                    "Nisan",
                    "Mayıs",
                    "Haziran",
                    "Temmuz",
                    "Ağustos",
                    "Eylül",
                    "Ekim",
                    "Kasım",
                    "Aralık"
                ],
                "firstDay": 0
            },
        }, function (start, end, label) {
            //$('.dateRangePicker').val(start.format('DD-MM-YYYY') + ' / ' + end.format('DD-MM-YYYY'));
        }

        );
        */

    };

    var cardTemplate = function (str) {
        var html = '<div class="card card-custom example example-compact">';
        html += '<div class="card-body">';
        html += str;
        html += '</div>';
        html += '</div>';

        return html;
    }

    var parseFloatFix = function (inputId, tofix) {
        debugger;
        var sonuc = parseFloat($(inputId).val().replace(",", "."));
        return sonuc.toFixed(tofix);

    }

    var dateRange = function () {
        // input group and left alignment setup
        $('.dateRangePicker').daterangepicker({
            buttonClasses: ' btn',
            applyClass: 'btn-primary',
            cancelClass: 'btn-secondary',
            locale: {
                "direction": "ltr",
                "format": "DD/MM/YYYY",
                "separator": " - ",
                "applyLabel": "Uygula",
                "cancelLabel": "İptal",
                "fromLabel": "Da",
                "toLabel": "A",
                "customRangeLabel": "Personalizzata",
                "daysOfWeek": [
                    "pzt",
                    "Sl",
                    "Çrş",
                    "Prş",
                    "Cm",
                    "Cmt",
                    "Pz"
                ],
                "monthNames": [
                    "Ocak",
                    "Şubat",
                    "Mart",
                    "Nisan",
                    "Mayıs",
                    "Haziran",
                    "Temmuz",
                    "Ağustos",
                    "Eylül",
                    "Ekim",
                    "Kasım",
                    "Aralık"
                ],
                "firstDay": 0
            },
        }, function (start, end, label) {
            $('.dateRangePicker .form-control').val(start.format('DD-MM-YYYY') + ' / ' + end.format('DD-MM-YYYY'));
        });
    }

    var responseTemplate = function (response) {
        debugger;
        console.log(response);
        response.Success == true ? toastr.success(response.Description) : toastr.error(response.Description);

    }

    var bootBoxHideAll = function (r) {
        if (r.responseJSON.Success) {
            setTimeout(function () {
                bootbox.hideAll();
                location.reload();
            }, 2000)

        } else {
            console.log(r.responseJSON);
            toastr.error(r.responseJSON.Description);
        }
    }

    return {
        // public functions
        init: function () {
            init();
        },
        cardTemplate: function (str) {
            return cardTemplate(str);
        },
        parseFloatFix: function (inputId, tofix) {
            return parseFloatFix(inputId, tofix);
        },
        dateRange: function () {
            dateRange();
        },
        ResponseTemplate: function (response) {
            responseTemplate(response);
        },
        BootBoxHideAll: function (r) {
            bootBoxHideAll(r);
        }
    };
}();

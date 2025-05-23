﻿var HammaddeHareket = function () {

    function Kaydet(formId, submitUrl) {

        var validation = ValidateForm.IsValid(formId, ValidationFields.HammaddeHareketFormFields())

        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#" + formId).serializeJSON();

                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;


                Post(submitUrl,
                    { form: form },
                    function (response) {
                        if (response.Success) {
                            toastr.success(response.Description);
                        } else {
                            toastr.error(response.Description);
                        }
                    },
                    function (x, y, z) {
                        //Error
                    },
                    function () {
                        //BeforeSend
                    },
                    function (r) {
                        //Complete
                        if (r.responseJSON.Success) {
                            setTimeout(function () {
                                bootbox.hideAll();
                                $('#kt_datatable').KTDatatable('reload');
                            }, 3000)

                        } else {
                            bootbox.hideAll();
                        }

                    },
                    "json");
            } else {
                return false;
            }
        });
    }

    function KdvTutarHesapla() {
        debugger;

        var birimFiyat = Global.parseFloatFix("#BirimFiyat",2);
        var KdvOran = Global.parseFloatFix("#KdvOrani",2);
        var Kur = Global.parseFloatFix("#Kur",2);
        var Miktar = Global.parseFloatFix("#Miktar",2);

        var KdvTutar = (birimFiyat * (KdvOran / 100)) * (Kur * Miktar);
        var ToplamTutar = (birimFiyat + KdvTutar) * Miktar * Kur;

        $("#ToplamTutar").val(ToplamTutar.toFixed(2));
        $("#KdvTutari").val(KdvTutar.toFixed(2));

    }
    var handleEvent = function () {
        $(document).on("click", "[event='hammaddeHareketFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            var formId = $(this).attr("formId");
            var formUrl = $(this).attr("formUrl");
            var submitUrl = $(this).attr("submitUrl");
            Post(formUrl,
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: title,
                        message: Global.cardTemplate(response),
                        size: 'large',
                        buttons: {
                            cancel: {
                                label: "Kapat",
                                className: 'btn-danger',
                                callback: function () { }
                            },
                            ok: {
                                label: "Kaydet",
                                className: 'btn-info',
                                callback: function () {
                                    Kaydet(formId, submitUrl);
                                    return false;
                                }
                            }
                        }
                    });
                },
                function (x, y, z) {
                    //Error
                },
                function () {
                    //BeforeSend
                },
                function () {
                    Global.init();
                },
                "html");
        });

        $(document).on("change", ".Kur", function (evet) {
            event.preventDefault();
            $("#Kur").val($(this).val());
            KdvTutarHesapla();
        });

        $(document).on("change", ".KdvHesap", function (evet) {
            event.preventDefault();
            KdvTutarHesapla();
        });
    }

    var DtInit = function (domId, url, columns, params) {

        var datatable = $('#' + domId).KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        //url: HOST_URL + '/api/datatables/demos/default.php',
                        url: url,
                        params: params
                    },
                },
                pageSize: 10, // display 20 records per page
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },

            //layout definition
            layout: {
                scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
                footer: false, // display/hide footer
            },

            //column sorting
            sortable: true,

            //enable pagination
            pagination: true,

            //columns definition
            columns: columns

        });

        $('.form-control').on('change', function (e) {
            datatable.search($(this).val().toLowerCase(), $(this).attr("id"));
        });
    }

    return {

        EventInit: function () {
            handleEvent();
        },

        DtInit: function (domId, url, columns, params) {
            DtInit(domId, url, columns, params);
        }
    };
}();
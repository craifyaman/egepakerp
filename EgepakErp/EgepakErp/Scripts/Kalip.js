var Kalip = function () {

    function Kaydet(formId, submitUrl) {

        var validation = ValidateForm.IsValid(formId, ValidationFields.KalipFormFields())

        validation.validate().then(function (status) {
            if (status == 'Valid') {
                debugger;
                var form = $("#" + formId).serializeJSON();
                $("#" + formId+' input[type=checkbox]').map(function () {
                    if (!this.checked) {
                        fields.push({
                            name: this.name, value: "off"
                        });
                    }
                });
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


    var handleEvent = function () {

        $(document).on("click", "[event='kalipFormPopup']", function (e) {
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


        $(document).on("click", "[event='KalipSil']", function (e) {
            e.preventDefault();
            debugger;
            var KalipId = $(this).attr("KalipId");
            bootbox.confirm({
                title: "Kalıp Silinsin mi?",
                message: Global.cardTemplate("Kalıbı silmek istediğine emin misin, bu işlemin geri dönüşü yok"),
                buttons: {
                    cancel: {
                        label: '<i class="fa fa-times"></i> İptal'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> Sil'
                    }
                },
                callback: function (result) {
                    if (result == true) {
                        Post("/kalip/sil",
                            { KalipId: KalipId },
                            function (response) {
                                if (response.Success == true) {
                                    toastr.success("kalıp silindi");
                                }
                                else {
                                    toastr.error(response.Description);
                                }
                            },
                            function (x, y, z) {
                                toastr.error("kalıp silinemedi");
                            },
                            function () {
                                //BeforeSend
                            },
                            function () {
                                Global.init();
                                setTimeout(function () {
                                    bootbox.hideAll();
                                    $('#kt_datatable').KTDatatable('reload');
                                }, 3000)
                            },
                            "json");
                    }
                }
            });
        });


        $(document).on("click", "[event='KalipDetay']", function (e) {
            e.preventDefault();
            debugger;
            var KalipId = $(this).attr("Id");
            Post("/kalip/KalipDetay",
                { KalipId: KalipId },
                function (response) {
                    bootbox.dialog({
                        title: "Kalıp Detay",
                        message: Global.cardTemplate(response),
                        size: 'large',
                        buttons: {
                            cancel: {
                                label: "Kapat",
                                className: 'btn-danger',
                                callback: function () { }
                            }
                        }
                    });
                },
                function (x, y, z) {
                    toastr.error("Bir Hata Oluştu");
                },
                function () {
                    //BeforeSend
                },
                function () {
                    Global.init();
                },
                "html");
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
                pageSize: 20, // display 20 records per page
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },

            //layout definition
            layout: {
                theme: 'default',
                class: 'datatable-brand',
                height: 600,
                scroll: true, // enable/disable datatable scroll both horizontal and vertical when needed.
                footer: false, // display/hide footer
                customScrollbar: true,
            },

            //column sorting
            sortable: true,

            //enable pagination
            pagination: true,

            //columns definition
            columns: columns,

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


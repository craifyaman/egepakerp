var Kisi = function () {
    function Kaydet() {
        var validation = ValidateForm.IsValid("kisiForm",ValidationFields.KisiFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var kisi = $("#kisiForm").serializeJSON();

                var keys = Object.keys(kisi);
                var include = keys.slice(1, keys.length);
                kisi.Include = include;
               
                
                
                Post('/Kisi/Kaydet',
                    { kisi: kisi },
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
    function DurumGuncelle(id) {
        Post('/Kisi/DurumGuncelle',
            { id: id },
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
            function () {
                //Complete
                setTimeout(function () { $('#kt_datatable').KTDatatable('reload'); }, 2000)
            },
            "json");
    }

    var handleEvent = function () {

        $(document).on("click", "[event='kisiFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            Post('/Kisi/Form',
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
                                    Kaydet();
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
        $(document).on("click", "[event='kisiDurum']", function (evet) {
            event.preventDefault();
            var id = $(this).attr("id");

            bootbox.dialog({
                title: 'Kişi Durum Güncelle',
                message: Global.cardTemplate('İşleme Devam Etmek İstiyor musunuz ?'),
                size: 'large',
                buttons: {
                    cancel: {
                        label: "Vazgeç",
                        className: 'btn-danger',
                        callback: function () { }
                    },
                    ok: {
                        label: "Devam",
                        className: 'btn-info',
                        callback: function () {
                            DurumGuncelle(id);
                        }
                    }
                }
            });
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

        DtInit: function (domId, url, columns,params) {
            DtInit(domId, url, columns,params);
        }
    };
}();

 
var Gorusme = function () {

   

    function GorusmeKaydet() {

        var validation =  ValidateForm.IsValid("gorusmeForm",ValidationFields.GorusmeFormFields())
         
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var frm = $("#gorusmeForm").serializeJSON();

                Post('/Gorusme/Kaydet',
                    { frm: frm },
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

        $(document).on("click", "[event='gorusmeFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            Post('/Gorusme/Form',
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
                                    GorusmeKaydet();
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
                    SelectPickerRemote.init();
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


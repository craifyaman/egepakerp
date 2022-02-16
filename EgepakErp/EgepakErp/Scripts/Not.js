var Not = function () {

   

    function NotKaydet() {

        validation =  ValidateForm.IsValid("notForm",ValidationFields.NotFormFields())
         
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var frm = $("#notForm").serializeJSON();

                Post('/Not/Kaydet',
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

        $(document).on("click", "[event='notFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            Post('/Not/Form',
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
                                    NotKaydet();
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
                },
                "html");
        });

    }

    var DtInit = function (domId, url, columns, params) {

        var datatable = $('#' + domId).KTDatatable({
            
            data: {
                type: 'remote',
                source: {
                    read: {
                       
                        url: url,
                        params: params
                    },
                },
                pageSize: 20,  
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },
            layout: {
                scroll: false,  
                footer: false,  
            },
            sortable: true,
            pagination: true,
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


var Tanim = function () {
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    $.fn.hasAttr = function (name) {
        return this.attr(name) !== undefined;
    };

     
    function BransKaydet() {

        validation = FormValidation.formValidation(
            KTUtil.getById('BransForm'),
            {
                fields: {
                    Adi: {
                        validators: {
                            notEmpty: { message: 'Bos Birakilamaz' }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
            }
        );
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var brans = $("#BransForm").serializeObject();

                Post('/Tanim/BransKaydet',
                    { brans: brans },
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
                        console.log("r", r);
                        var data = r.responseJSON.Data;

                        $('#BransId').append($('<option>', {
                            value: data.BransId,
                            text: data.Adi,
                            selected: 'selected'
                        }));

                        $('#BransId').selectpicker('refresh');
                    },
                    "json");
            } else {

            }
        });
    }
    function CariGrupKaydet() {

        validation = FormValidation.formValidation(
            KTUtil.getById('CariGrupForm'),
            {
                fields: {
                    Adi: {
                        validators: {
                            notEmpty: { message: 'Bos Birakilamaz' }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
            }
        );
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var frm = $("#CariGrupForm").serializeObject();

                Post('/Tanim/CariGrupKaydet',
                    { cariGrup: frm },
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
                        console.log("r", r);
                        var data = r.responseJSON.Data;

                        $('#CariGrupId').append($('<option>', {
                            value: data.CariGrupId,
                            text: data.Adi,
                            selected: 'selected'
                        }));

                        $('#CariGrupId').selectpicker('refresh');
                    },
                    "json");
            } else {

            }
        });
    }

    function GorusmeTipKaydet() {

        validation = FormValidation.formValidation(
            KTUtil.getById('GorusmeTipForm'),
            {
                fields: {
                    Adi: {
                        validators: {
                            notEmpty: { message: 'Bos Birakilamaz' }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
            }
        );
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var frm = $("#GorusmeTipForm").serializeObject();

                Post('/Tanim/GorusmeTipKaydet',
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
                        console.log("r", r);
                        var data = r.responseJSON.Data;

                        $('#GorusmeTipId').append($('<option>', {
                            value: data.GorusmeTipId,
                            text: data.Adi,
                            selected: 'selected'
                        }));

                        $('#GorusmeTipId').selectpicker('refresh');
                    },
                    "json");
            } else {

            }
        });
    }
   

    var handleEvent = function () {

        $(document).on("click", "[event='bransFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            Post('/Tanim/BransForm',
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: title,
                        message: response,
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
                                    BransKaydet();
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
        $(document).on("click", "[event='gorusmeTipFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            Post('/Tanim/GorusmeTipForm',
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: title,
                        message: response,
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
                                    GorusmeTipKaydet();
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
        $(document).on("click", ".cariGrupFormPopup", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            Post('/Tanim/CariGrupForm',
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: title,
                        message: response,
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
                                    CariGrupKaydet();
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

 
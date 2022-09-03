var UretimEmir = function () {
    
    function Kaydet() {
        debugger;
        var validation = ValidateForm.IsValid("UretimEmirForm", ValidationFields.UretimEmirFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#UretimEmirForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;               
                console.log(form);
                Post("/UretimEmir/kaydet",
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
                            }, 2000)
                            
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

        $(document).on("click", "[event='UretimEmirFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");            
            Post("/UretimEmir/form",
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: "UretimEmir form",
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
                    KTBootstrapTimepicker.init();
                },
                "html");
        });

        
        $(document).on("change", "#SiparisId", function (event) {
            event.preventDefault();
            var siparisId = $(this).val();            
            Post("/uretimemir/SiparisKalipBySiparis",
                { siparisId: siparisId },
                function (response) {
                    $("#SiparisKalipList").empty().html(response);
                    $("#SiparisKalipList").show();

                },
                function (x, y, z) {
                    //Error
                },
                function () {
                    //BeforeSend
                },
                function () {
                },
                "html");
        });

    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        }
    };
}();

 
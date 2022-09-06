var Aksiyon = function () {

    function Kaydet(box) {
        debugger;
        var validation = ValidateForm.IsValid("AksiyonForm", ValidationFields.AksiyonFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#AksiyonForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/Aksiyon/kaydet",
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
                                box.modal('hide');
                            }, 2000);
                        } else {
                            //bootbox.modal('hide');
                        }

                    },
                    "json");
            } else {
                return false;
            }
        });
    }

    function AksiyonForm(id,uretimEmirId) {
        Post("/Aksiyon/form",
            { id: id, UretimEmirId: uretimEmirId },
            function (response) {
                var box = bootbox.dialog({
                    title: "Aksiyon form",
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
                                Kaydet($(this));
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
                KTBootstrapDatetimepicker.init();
            },
            "html");
    }

    var handleEvent = function () {

        //$(document).on("click", "[event='AksiyonFormPopup']", function (e) {
        //    debugger;
        //    e.preventDefault();
        //    var id = $(this).attr("id");
        //    var uretimEmirId = $(this).attr("UretimEmirId");
        //    AksiyonForm(id, uretimEmirId);
        //});

    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        },
        AksiyonForm: function (id, uretimEmirId) {
            AksiyonForm(id, uretimEmirId);
        }
    };
}();


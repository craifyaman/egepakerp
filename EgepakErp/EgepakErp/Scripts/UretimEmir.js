var UretimEmir = function () {

    function Kaydet() {
        debugger;
        var validation = ValidateForm.IsValid("UretimEmirForm", ValidationFields.UretimEmirFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#UretimEmirForm").serializeJSON(
                    {
                        customTypes: {
                            CustomSwitch: (str, el) => {
                                if (str == "on") {
                                    return "true";
                                }
                                return "false";
                            },
                        }
                    });
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
                                location.reload();
                            }, 2000)

                        } else {
                            toastr.error("Bir hata oluştu");
                        }

                    },
                    "json");
            } else {
                return false;
            }
        });
    }

    function UretimEmirForm(id) {
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
                KTBootstrapDatetimepicker.init();
                KTSelect2.init();
            },
            "html");
    }

    var handleEvent = function () {

        $(document).on("click", "[event='UretimEmirFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            UretimEmirForm(id);
        });

       


        $(document).on("change", "#SiparisId", function (event) {
            debugger;
            event.preventDefault();
            var siparisId = $(this).val();
            var adet = $("#SiparisId option:selected").attr("Adet");
            $("#SiparisAdet").val(adet);
            Post("/uretimemir/SiparisKalipBySiparis",
                { siparisId: siparisId },
                function (response) {
                    $("#SiparisKalipList").empty().html(response);

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
        },
        UretimEmirForm: function (id) {
            UretimEmirForm(id);
        }
    };
}();


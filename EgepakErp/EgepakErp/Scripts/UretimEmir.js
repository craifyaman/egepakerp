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
                //form.UretimEmirDurumList = $("#UretimEmirDurumId").val().toString();
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
                            console.log(r.responseJSON);
                            toastr.error(r.responseJSON.Description);
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
                $("#UretimEmirDurumId").select2();
            },
            "html");
    }


    function UretimEmirAksiyonKaydet() {
        debugger;
        var validation = ValidateForm.IsValid("UretimEmirAksiyonForm", ValidationFields.UretimEmirAksiyonFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#UretimEmirAksiyonForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);

                Post("/UretimEmir/UretimEmirAksiyonKaydet",
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
                            console.log(r.responseJSON);
                            toastr.error(r.responseJSON.Description);
                        }

                    },
                    "json");
            } else {
                return false;
            }
        });
    }

    function UretimEmirAksiyonForm(UretimEmirId, UretimEmirAksiyonTypeId) {
        Post("/UretimEmir/UretimEmirAksiyonForm",
            { UretimEmirId: UretimEmirId, UretimEmirAksiyonTypeId: UretimEmirAksiyonTypeId },
            function (response) {
                bootbox.dialog({
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
                                UretimEmirAksiyonKaydet();
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
                $("#KisiId").select2();
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

        $(document).on("click", "[event='UretimEmirAksiyonFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var UretimEmirId = $(this).attr("UretimEmirId");
            var UretimEmirAksiyonTypeId = $(this).attr("UretimEmirAksiyonTypeId");
            UretimEmirAksiyonForm(UretimEmirId, UretimEmirAksiyonTypeId);
        });

        $(document).on("click", "[event='UretimAksiyonFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var UretimAksiyonId = $(this).attr("UretimAksiyonId");
            UretimAksiyon.Form(UretimAksiyonId);
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
        },
        UretimEmirAksiyonForm: function (UretimEmirId, UretimEmirAksiyonTypeId) {
            UretimEmirAksiyonForm(UretimEmirId, UretimEmirAksiyonTypeId);
        }
    };
}();


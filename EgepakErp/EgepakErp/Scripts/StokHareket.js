var StokHareket = function () {

    function Kaydet() {
        debugger;
        var validation = ValidateForm.IsValid("StokHareketForm", ValidationFields.StokHareketFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#StokHareketForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/StokHareket/kaydet",
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
                            }, 2000);
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

    function StokHareketForm(id) {
        Post("/StokHareket/form",
            { id: id },
            function (response) {
                var box = bootbox.dialog({
                    title: "StokHareket form",
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
            },
            "html");
    }

    function Sil(id) {
        Post("/StokHareket/Sil",
            { id: id },
            function (response) {
                Global.ResponseTemplate(response, true);
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
            "json");
    }


    var handleEvent = function () {
        debugger;
        $(document).on("click", "[event='StokHareketFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            StokHareketForm(id);
        });

        $(document).on("click", "[event='StokHareketSil']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            bootbox.dialog({
                title: "Kayıt Sil",
                message: Global.cardTemplate("Kayıt Silinecek Onaylıyor Musunuz?"),
                size: 'large',
                buttons: {
                    cancel: {
                        label: "İptal",
                        className: 'btn-info',
                        callback: function () { }
                    },
                    ok: {
                        label: "Sil",
                        className: 'btn-danger',
                        callback: function () {
                            Sil(id);
                            return false;
                        }
                    }
                }
            });
        });

    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        },
        StokHareketForm: function (id) {
            StokHareketForm(id);
        }
    };
}();


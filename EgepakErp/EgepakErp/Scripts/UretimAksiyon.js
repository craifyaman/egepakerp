var UretimAksiyon = function () {

    function Kaydet(box) {
        debugger;
        var validation = ValidateForm.IsValid("UretimAksiyonForm", ValidationFields.UretimAksiyonFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#UretimAksiyonForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/UretimAksiyon/kaydet",
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
                                box.modal('hide')
                                $('#kt_datatable').KTDatatable('reload');
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

    function Form(id, UretimEmirId, MakineId) {
        debugger;
        Post("/UretimAksiyon/form",
            { id: id, UretimEmirId: UretimEmirId, MakineId: MakineId },
            function (response) {
                var box = bootbox.dialog({
                    title: "Üretim Aksiyon form",
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

    var handleEvent = function () {

        $(document).on("click", "[event='UretimAksiyonFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            var UretimEmirId = $(this).attr("UretimEmirId");
            Form(id, UretimEmirId);
        });
    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        },
        Form: function (id, UretimEmirId, MakineId) {
            Form(id, UretimEmirId, MakineId);
        }
    };
}();


var StokGirisHareket = function () {

    function Kaydet() {
        debugger;
        var validation = ValidateForm.IsValid("StokGirisHareketForm", ValidationFields.StokGirisHareketFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#StokGirisHareketForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/StokGirisHareket/kaydet",
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

    function StokGirisHareketForm(id,stokHareketId) {
        debugger;
        Post("/StokGirisHareket/form",
            { id: id, stokHareketId: stokHareketId  },
            function (response) {
                var box = bootbox.dialog({
                    title: "Stok Giris Hareket form",
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

    function StokGirisListeGetir(stokHareketId) {
        debugger;
        //stok hareket id'sine göre listeleme
        Post("/StokGirisHareket/GetListById",
            { stokHareketId: stokHareketId },
            function (response) {
                var box = bootbox.dialog({
                    title: "Hareket Listesi",
                    message: Global.cardTemplate(response),
                    size: 'large',
                    buttons: {
                        cancel: {
                            label: "Kapat",
                            className: 'btn-danger',
                            callback: function () { }
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
        debugger;
        $(document).on("click", "[event='StokGirisHareketFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            var stokHareketId = $(this).attr("stokHareketId");
            StokGirisHareketForm(id, stokHareketId);
        });

        $(document).on("click", "[event='GirisHareketListe']", function (e) {
            debugger;
            e.preventDefault();
            var stokHareketId = $(this).attr("stokHareketId");
            StokGirisListeGetir(stokHareketId);
        });

        
    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        },
        StokGirisHareketForm: function (id) {
            StokGirisHareketForm(id, StokGirisHareketId);
        }
    };
}();


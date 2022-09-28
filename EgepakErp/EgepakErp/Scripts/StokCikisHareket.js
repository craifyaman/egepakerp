var StokCikisHareket = function () {

    function Kaydet() {
        debugger;
        var validation = ValidateForm.IsValid("StokCikisHareketForm", ValidationFields.StokCikisHareketFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#StokCikisHareketForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/StokCikisHareket/kaydet",
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

    function StokCikisHareketForm(id,stokHareketId,cariId) {
        debugger;
        Post("/StokCikisHareket/form",
            { id: id, stokHareketId: stokHareketId, cariId: cariId },
            function (response) {
                var box = bootbox.dialog({
                    title: "StokCikisHareket form",
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
                $('#CariId').select2({
                    placeholder: 'Cari Seciniz'
                });
            },
            "html");
    }

    function StokCikisListeGetir(stokHareketId) {
        debugger;
        //stok hareket id'sine göre listeleme
        Post("/StokCikisHareket/GetListById",
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
                        },
                        //ok: {
                        //    label: "Kaydet",
                        //    className: 'btn-info',
                        //    callback: function () {
                        //        Kaydet($(this));
                        //        return false;
                        //    }
                        //}
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
                $('#CariId').select2({
                    placeholder: 'Cari Seciniz'
                });
            },
            "html");
    }

    


    var handleEvent = function () {
        debugger;
        $(document).on("click", "[event='StokCikisHareketFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            var stokHareketId = $(this).attr("stokHareketId");
            var cariId = $(this).attr("CariId");
            StokCikisHareketForm(id, stokHareketId, cariId);
        });

        $(document).on("click", "[event='CikisHareketListe']", function (e) {
            debugger;
            e.preventDefault();
            var stokHareketId = $(this).attr("stokHareketId");
            StokCikisListeGetir(stokHareketId);
        });

        
    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        },
        StokCikisHareketForm: function (id) {
            StokCikisHareketForm(id, stokCikisHareketId);
        }
    };
}();


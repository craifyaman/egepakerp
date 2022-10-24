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

    function CikisHareketKaydet() {
        debugger;
        var validation = ValidateForm.IsValid("CikisHareketForm", ValidationFields.StokCikisHareketFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#CikisHareketForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/StokHareket/CikisHareketKaydet",
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

    function CikisHareketForm(id) {
        Post("/StokHareket/CikisHareketForm",
            { id: id },
            function (response) {
                bootbox.dialog({
                    title: "Çıkış Hareket Form",
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
                                CikisHareketKaydet();
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
    
    function CikisHareketSil(id) {
        Post("/StokHareket/CikisHareketSil",
            { id: id },
            function (response) {
                Global.ResponseTemplate(response, true,true);
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


    function GirisHareketKaydet() {
        debugger;
        var validation = ValidateForm.IsValid("GirisHareketForm", ValidationFields.StokCikisHareketFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#GirisHareketForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/StokHareket/GirisHareketKaydet",
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

    function GirisHareketForm(id) {
        debugger;
        Post("/StokHareket/GirisHareketForm",
            { id: id },
            function (response) {
                bootbox.dialog({
                    title: "Giriş Hareket Form",
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
                                GirisHareketKaydet();
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

    function GirisHareketSil(id) {
        Post("/StokHareket/GirisHareketSil",
            { id: id },
            function (response) {
                Global.ResponseTemplate(response, true, true);
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

        
        $(document).on("click", "[event='cikisHareketFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("StokCikisHareketId");
            CikisHareketForm(id);
        });

        $(document).on("click", "[event='CikisHareketSil']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("StokCikisHareketId");
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
                            CikisHareketSil(id);
                            return false;
                        }
                    }
                }
            });
        });


        $(document).on("click", "[event='girisHareketFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("StokGirisHareketId");
            GirisHareketForm(id);
        });

        $(document).on("click", "[event='girisHareketSil']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("StokGirisHareketId");
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
                            GirisHareketSil(id);
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


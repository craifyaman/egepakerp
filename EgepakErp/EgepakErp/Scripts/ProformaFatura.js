var ProformaFatura = function () {

    function Kaydet() {

        var ProformaFaturaUrunDto = function () {
            this.Aciklama;//string
            this.Adet;//int
            this.BirimFiyat;//decimal
        }

        var dtoList = [];
        debugger;
        var validation = ValidateForm.IsValid("ProformaFaturaForm", ValidationFields.ProformaFaturaFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#ProformaFaturaForm").serializeJSON();

                //repeater verilerini nesneye çevir
                var _repeaterVal = $("#kt_repeater_1").repeaterVal();
                var obj = _repeaterVal;
                var arr = Object.keys(obj).map(function (key) { return obj[key]; });
                $.each(arr[0], function (i, v) {
                    var dto = new ProformaFaturaUrunDto();
                    dto.Aciklama = v.Aciklama;
                    dto.Adet = v.Adet;
                    dto.BirimFiyat = v.BirimFiyat;
                    dtoList.push(dto);
                });

                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                form.ProformaUrun = dtoList;
                console.log(form);

                Post("/ProformaFatura/kaydet",
                    { form: form },
                    function (response) {
                        if (response.Success == true) {
                            toastr.success(response.Description);
                        }

                        if (response.Success == false) {
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

    function ProformaFaturaForm(id) {
        debugger;
        Post("/ProformaFatura/form",
            { id: id  },
            function (response) {
                var box = bootbox.dialog({
                    title: "Proforma Fatura Formu",
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
                KTBootstrapDatetimepicker.ProformaFatura();
                KTSelect2.ProformaFatura();
                KTFormRepeater.init();
            },
            "html");
    }


    var handleEvent = function () {
        $(document).on("click", "[event='ProformaFaturaFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            ProformaFaturaForm(id);
        });
    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        },
        ProformaFaturaForm: function (id) {
            ProformaFaturaForm(id);
        }
    };
}();


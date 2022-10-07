var TeklifForm = function () {

    function Kaydet() {

        var TeklifFormUrunDto = function () {
            this.Kod;//string
            this.UrunAdi;//string
            this.Teklif;//decimal
        }

        var dtoList = [];
        debugger;
        var validation = ValidateForm.IsValid("TeklifFormForm", ValidationFields.TeklifFormFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#TeklifFormForm").serializeJSON();

                //repeater verilerini nesneye çevir
                var _repeaterVal = $("#kt_repeater_1").repeaterVal();
                var obj = _repeaterVal;
                var arr = Object.keys(obj).map(function (key) { return obj[key]; });
                $.each(arr[0], function (i, v) {
                    var dto = new TeklifFormUrunDto();
                    dto.Kod = v.Kod;
                    dto.UrunAdi = v.UrunAdi;
                    dto.Teklif = v.Teklif;
                    dtoList.push(dto);
                });
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                form.SiparisTeklifFormUrun = dtoList;


                console.log(form);
                Post("/TeklifForm/kaydet",
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

    function TeklifFormForm(id) {
        debugger;
        Post("/TeklifForm/form",
            { id: id  },
            function (response) {
                var box = bootbox.dialog({
                    title: "Teklif Formu",
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
                KTBootstrapDatetimepicker.TeklifForm();
                KTSelect2.TeklifForm();
                KTFormRepeater.init();
            },
            "html");
    }


    var handleEvent = function () {
        $(document).on("click", "[event='TeklifFormFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");
            TeklifFormForm(id);
        });
    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        },
        TeklifFormForm: function (id) {
            TeklifFormForm(id);
        }
    };
}();


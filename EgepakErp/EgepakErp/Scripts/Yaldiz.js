var Yaldiz = function () {
    
    function Kaydet() {
        debugger;
        var validation = ValidateForm.IsValid("YaldizForm", ValidationFields.YaldizFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#YaldizForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;               
                console.log(form);
                Post("/Yaldiz/kaydet",
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
                            }, 2000)
                            
                        } else {
                            console.log(r.responseJSON);
                            toastr.error(r.responseJSON.Description);
                            bootbox.hideAll();
                        }

                    },
                    "json");
            } else {
                return false;
            }
        });
    }
    

    var handleEvent = function () {

        $(document).on("click", "[event='YaldizFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");            
            Post("/Yaldiz/form",
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: "Yaldız form",
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
                    KTSelect2.Yaldiz();
                },
                "html");
        });

        $(document).on("change", ".UploadFile2", function (event) {
            event.preventDefault();
            debugger;
            var inputId = $(this).attr("id");
            var TargetDirectory = "SiparisPdf/CariId-" + $("#CariId").val() + "/Yaldiz/";
            var TargetInputId = $(this).attr("TargetInputId");
            UploadImage(inputId, "/File/DosyaKaydet", TargetDirectory, TargetInputId);
        });
    }

    return {

        EventInit: function () {
            handleEvent();
        },
        Kaydet: function () {
            Kaydet();
        }
    };
}();

 
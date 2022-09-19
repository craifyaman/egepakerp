var BoyaKaplama = function () {
  
    function Kaydet() {
        debugger;
        var validation = ValidateForm.IsValid("BoyaKaplamaForm", ValidationFields.BoyaKaplamaFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#BoyaKaplamaForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;               
                console.log(form);
                Post("/BoyaKaplama/kaydet",
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

        $(document).on("click", "[event='BoyaKaplamaFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var id = $(this).attr("id");            
            Post("/BoyaKaplama/form",
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: "BoyaKaplama form",
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
        });

    }

    return {

        EventInit: function () {
            handleEvent();
        }
    };
}();

 
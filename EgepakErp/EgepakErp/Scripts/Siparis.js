var Siparis = function () {

    function KalipKaydet(formId, submitUrl) {

        var validation = ValidateForm.IsValid(formId, ValidationFields.KalipFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#" + formId).serializeJSON();

                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;


                Post(submitUrl,
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
                                UrunKaliplariGetir();
                            }, 3000)

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

    function UrunKaliplari() {
        var form = $("#KalipIdler").serializeObject();
        var keys = Object.keys(form);
        var include = keys.slice(1, form.length);
        form.Include = include;

        console.log("form", form);
        $.ajax({
            type: "GET",
            url: "/siparis/Form",
            data: form,
            success: function (html) {
                console.log(html);
            },
            error: function () {
                toastr.error("bir hata oluştu");
            }
        })
    }

    function UrunKaliplariGetir() {
        var urunId = $("#UrunId").val();
        var excludes = ",";
        if ($("#exclude").val() != undefined) {
            excludes = $("#exclude").val().slice(0, -1);
        }
        Post("/siparis/UrunKaliplari",
            { urunid: urunId, exclude: excludes },
            function (response) {
                $("#UrunKaliplari").empty().html(response);
            },
            function (x, y, z) {
                //Error
            },
            function () {
                //BeforeSend
            },
            function () {
                maliyetTablosuGetir();
            },
            "html");
    }

    function maliyetHesapla() {

        var liste = [];

        var Maliyet = function () {
            this.KalipId;
            this.Tutar;
        }
        $("input.birimFiyat").each(function (index, value) {
            var input = value;
            if ($(input).prop('disabled') == false) {
                var maliyet = new Maliyet();
                maliyet.kalipId = input.getAttribute("kalipId");
                maliyet.Tutar = input.value;
                liste.push(maliyet);
            }   
        });
        
        Post("/siparis/MaliyetHesap",
            { liste: liste },
            function (response) {
                $("tr.sum").each(function (index, value) {
                    $(this).remove();
                });
                $("#MaliyetTablo").append(response);
                toastr.success("Maliyet hesplandı.");
            },
            function (x, y, z) {
                toastr.error("bir hata oluştu");
            },
            function () {
                //BeforeSend
            },
            function () {

            },
            "html");
    }

    function maliyetTablosuGetir() {


        var idList = $("#kalipIdList").val().split(",");
        var excludes = $("#exclude").val().slice(0, -1).split(",");
        var FixedIdList = [];
        for (var i = 0; i < idList.length; i++) {
            if (checkValue(idList[i], excludes) == false) {
                FixedIdList.push(idList[i])
            }
        }
        Post("/siparis/maliyetForm",
            { idList: FixedIdList },
            function (response) {
                $("#maliyetTablosu").empty().html(response);
                maliyetHesapla();
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
    }

    function checkValue(value, arr) {
        var status = false;
        for (var i = 0; i < arr.length; i++) {
            var val = arr[i];
            if (val == value) {
                status = true;
                break;
            }
        }
        return status;
    }

    var handleEvent = function () {

        $(document).on("click", "[event='kalipFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            var formId = $(this).attr("formId");
            var formUrl = $(this).attr("formUrl");
            var submitUrl = $(this).attr("submitUrl");

            Post(formUrl,
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: title,
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
                                    KalipKaydet(formId, submitUrl);
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

        $(document).on("click", "[event='maliyetTablosuGetir']", function (event) {
            event.preventDefault();
            maliyetTablosuGetir();

        });

        $(document).on("click", "[event='KalipListesindenCikar']", function (event) {
            event.preventDefault();
            var _this = $(this);
            var tr = _this.parents("tr");
            var id = tr.attr("KalipId");
            $("#exclude").val($("#exclude").val() + id + ",");
            tr.remove();
            setTimeout(function () {
                maliyetTablosuGetir();
            }, 1000)
        });

        $(document).on("change", "#UrunId", function (event) {
            event.preventDefault();
            UrunKaliplariGetir();
        });

        $(document).on("change", ".birimFiyat", function () {
            maliyetHesapla();
        })
    }

    var DtInit = function (domId, url, columns, params) {

        var datatable = $('#' + domId).KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        //url: HOST_URL + '/api/datatables/demos/default.php',
                        url: url,
                        params: params
                    },
                },
                pageSize: 20, // display 20 records per page
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
            },

            //layout definition
            layout: {
                scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
                footer: false, // display/hide footer
            },

            //column sorting
            sortable: true,

            //enable pagination
            pagination: true,

            //columns definition
            columns: columns

        });

        $('.form-control').on('change', function (e) {
            datatable.search($(this).val().toLowerCase(), $(this).attr("id"));
        });
    }


    return {

        EventInit: function () {
            handleEvent();
        },

        DtInit: function (domId, url, columns, params) {
            DtInit(domId, url, columns, params);
        },
        MaliyetHesapla: function () {
            maliyetHesapla();
        }

    };
}();


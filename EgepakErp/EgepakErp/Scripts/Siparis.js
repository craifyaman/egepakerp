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

    function FaturadanCikar(id, tagname) {
        debugger
        var tdInputs = $("td[" + tagname + "-cikar='" + id + "'] input");
        var SelectBoxes = $("td[" + tagname + "-cikar='" + id + "'] select");
        var btn = $("span[" + tagname + "-cikar='" + id + "']");

        if (btn.hasClass("btn-danger")) {
            tdInputs.prop("disabled", true);
            SelectBoxes.prop("disabled", true);
            btn.removeClass("btn-danger").addClass("btn-success");
            btn.html("+");
        }
        else {
            btn.removeClass("btn-success").addClass("btn-danger");
            tdInputs.prop("disabled", false);
            SelectBoxes.prop("disabled", false);
            btn.html("-");
        }
        maliyetHesapla();
    }

    function UrunKaliplariGetir() {
        debugger;
        var urunId = $("#UrunId").val();
        var excludes = ",";
        var includes = ",";

        if ($("#include").val() != undefined) {
            includes = $("#include").val().slice(0, -1);
            includes = includes.split(",");
        }
        if ($("#exclude").val() != undefined) {
            excludes = $("#exclude").val().slice(0, -1);
        }
        Post("/siparis/UrunKaliplari",
            { urunid: urunId, exclude: excludes, includes: includes },
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
        debugger
        var liste = [];
        var Maliyet = function () {
            this.KalipId;
            this.Tutar;
            this.Status;
        }
        var teklifTutar = 0;
        var array = $(".birimFiyat").sort();
        array.each(function (index, value) {
            var input = value;
            var maliyet = new Maliyet();
            maliyet.kalipId = input.getAttribute("kalipId");
            maliyet.Tutar = input.value;

            if ($(input).prop('disabled') == false) {
                maliyet.Status = true;
                teklifTutar += parseFloat($(input).val().replace(",", "."));
            } else {
                maliyet.Status = false;
            }
            liste.push(maliyet);
        });

        Post("/siparis/MaliyetHesap",
            { liste: liste },
            function (response) {
                $("tr.sum").each(function (index, value) {
                    $(this).remove();
                });
                $("#MaliyetTablo").append(response);

                $("#TeklifTutari").html(teklifTutar.toFixed(2));

                var malzemeMaliyet = ToplamMalzemeMaliyet();
                $("#ToplamMalzemeMaliyet").html(malzemeMaliyet);

                var urunMaliyet = ToplamUretimMaliyet();
                $("#ToplamUretimMaliyet").html(urunMaliyet);

                ToplamMaliyetHesapla(teklifTutar.toFixed(2));
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
        debugger;
        var idList = $("#kalipIdList").val().split(",");
        var urunId = $("#UrunId").val();
        var excludes = $("#exclude").val().slice(0, -1).split(",");
        var FixedIdList = [];
        for (var i = 0; i < idList.length; i++) {
            if (checkValue(idList[i], excludes) == false) {
                FixedIdList.push(idList[i])
            }
        }

        Post("/siparis/maliyetForm",
            { idList: FixedIdList, urunId: urunId },
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
                DisMalzemelerSifirla();
            },
            "html");
    }

    function DisMalzemelerSifirla() {
        var arr = $("td");
        $.each(arr, function (index, val) {
            if ($(val).attr("HazirMalzemeMi") == "True") {
                $(val).find(".birimFiyat").val("0");
            }
        });
        maliyetHesapla();
    }

    function InputBulEkle(MaliyetType, KalipId, Value) {
        var Input = $("td[" + MaliyetType + "-cikar='" + KalipId + "'] input");
        Input.val(Value);
    }


    function maliyetDetayGetir(maliyetType, kalipId, PosetParametre, urunId) {
        Post("/siparis/MaliyetDetay",
            { MaliyetType: maliyetType, KalipId: kalipId, PosetParametre: PosetParametre, urunId: urunId },
            function (response) {
                bootbox.dialog({
                    title: maliyetType.toUpperCase() + " DETAY",
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
                                var value = $(".Fiyat").val();
                                InputBulEkle(maliyetType, kalipId, value);
                                debugger;
                                if ($(".parentDiv").attr("uruntype") == "koli") {
                                    var posetKatsayi = $("#posetParametre").val();
                                    var targetId = $("#posetParametre").attr("targetInputId");
                                    $("#" + targetId).val(posetKatsayi);
                                }
                                maliyetHesapla();
                                bootbox.hideAll();
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

    function ToplamMalzemeMaliyet() {
        var toplam = 0;
        var array = $("input.birimFiyat[HesplamaType=malzeme]").sort();
        array.each(function (index, value) {
            var input = value;
            if ($(input).prop('disabled') == false) {
                toplam += parseFloat($(input).val().replace(",", "."));
            }
        });
        console.log("toplam malzeme maliyeti : " + toplam);
        return toplam.toFixed(2);
    }

    function ToplamUretimMaliyet() {
        var toplam = 0;
        var array = $("input.birimFiyat[HesplamaType=uretim]").sort();
        array.each(function (index, value) {
            var input = value;
            if ($(input).prop('disabled') == false) {
                toplam += parseFloat($(input).val().replace(",", "."));
            }
        });
        console.log("toplam uretim maliyeti : " + toplam);
        return toplam.toFixed(2);
    }

    function ToplamMaliyetHesapla(tutar) {
        debugger;
        console.log(tutar);
        $.ajax({
            type: "GET",
            url: "/siparis/UsdKurHesapla?tutar=" + tutar,
            dataType: "json",
            success: function (response) {
                if (response.Success == true) {
                    var sonuc = response.Data;
                    console.log("usd kur hesaplama sonuç :" + sonuc);
                    $("#usdKurSonuc").html(sonuc.toFixed(2));
                } else {
                    toastr.error(response.Description);
                }
            },
            error: function () {
                toastr.error("bir hata oluştu");
            }
        });

        $.ajax({
            type: "GET",
            url: "/siparis/EurKurHesapla?tutar=" + tutar,
            dataType: "json",
            success: function (response) {
                if (response.Success == true) {
                    var sonuc = response.Data;
                    console.log("euro kur hesaplama sonuç :" + sonuc);
                    $("#eurKurSonuc").html(sonuc.toFixed(2));
                } else {
                    toastr.error(response.Description);
                }
            },
            error: function () {
                toastr.error("bir hata oluştu");
            }
        });

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
            var includeList = $("#include").val().slice(0, -1).split(",");
            debugger;
            $.each(includeList, function (i, v) {
                if (id == v) {
                    includeList = $.grep(includeList, function (n) {
                        return n != v;
                    });
                }
            });
            console.log(includeList);
            $("#include").val(includeList.toString());
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

        $(document).on("click", "[event='MaliyetDetay']", function (event) {
            event.preventDefault();
            debugger;
            var MaliyetType = $(this).attr("maliyetType");
            var KalipId = $(this).attr("kalipId");
            var PosetParametre = $("#PosetParametre_" + KalipId).val();
            var urunId = $("#UrunId").val();
            maliyetDetayGetir(MaliyetType, KalipId, PosetParametre, urunId);
        });

        $(document).on("click", "[event='FaturadanCikar']", function (event) {
            event.preventDefault();
            var KalipId = $(this).attr("KalipId");
            var cikarType = $(this).attr("cikar-type");
            FaturadanCikar(KalipId, cikarType);
        });

        $(document).on("change", "#FiyatOrtalamaSelect", function (event) {
            event.preventDefault();
            var target = $("#BirimFiyat");
            var birimFiyat = parseFloat($(this).val().replace(",", "."));
            target.val(birimFiyat);
            $(document.getElementById("BirimFiyat")).trigger("change");
        });


        $(document).on("change", "#KoliBirimFiyat", function (event) {
            event.preventDefault();
            debugger;
            var target = $("#BirimFiyat");
            var birimFiyat = parseFloat($(this).val().replace(",", "."));
            var posetParametre = $("#KoliBirimFiyat option:selected").attr("posetParametre");
            var Katsayi = $("#KoliBirimFiyat option:selected").attr("Katsayi");

            $("#posetParametre").val(posetParametre);
            $("#KoliKatsayi").val(Katsayi);

            target.val(birimFiyat);
            $(document.getElementById("BirimFiyat")).trigger("change");
        });

        $(document).on("change", "#BantSelect", function (event) {
            event.preventDefault();            
            $(document.getElementById("BirimFiyat")).trigger("change");
        });
        
        $(document).on("change", "#PosetBirimFiyat", function (event) {
            event.preventDefault();
            var target = $("#BirimFiyat");
            var birimFiyat = parseFloat($(this).val().replace(",", "."));
            target.val(birimFiyat);
            $(document.getElementById("BirimFiyat")).trigger("change");
        });


        $(document).on("change", ".hesaplama", function (event) {
            event.preventDefault();
            debugger
            var Parent = $(this).parents(".parentDiv");
            var type = Parent.attr("UrunType");

            if (type == "hammadde") {
                HesapFunctionHammadde();
            }
            if (type == "tozBoya") {
                HesapFunctionTozBoya();
            }
            if (type == "koli") {
                HesapFunctionKoli();
            }
            if (type == "poset") {
                HesapFunctionPoset();
            }
            if (type == "yaldiz") {
                HesapFunctionYaldiz();
            }
            if (type == "enjeksiyon") {
                HesapFunctionEnjeksiyon();
            }
            if (type == "sicakbaski") {
                HesapFunctionSicakBaski();
            }
            if (type == "montaj") {
                HesapFunctionMontaj();
            }

        });

        $(document).on("change", "#yaldizSelect", function (event) {
            event.preventDefault();
            debugger;
            var target = $("#BirimFiyat");
            var birimFiyat = parseFloat($(this).val().replace(",", "."));
            target.val(birimFiyat);
            $(document.getElementById("BirimFiyat")).trigger("change");
        });

        $(document).on("change", "#SiparisKalipSelect", function (event) {
            event.preventDefault();
            debugger;
            var kalipId = $(this).val();
            $("#include").val($("#include").val() + kalipId + ",");
            UrunKaliplariGetir();
        });


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
        }

    };
}();


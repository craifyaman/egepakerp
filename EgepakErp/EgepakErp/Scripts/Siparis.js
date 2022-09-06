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

    function OzelSecim() {
        debugger;
        var urunCins = $("#UrunKisaltma").val().toLowerCase();

        if (urunCins == "ms") {
            $("#fircaDiv").show();
        }
        if (urunCins == "rj") {
            $("#asansorDiv").show();
            $("#rujTupDiv").show();
            $("#helezonDiv").show();
        }
    }

    // Read a page's GET URL variables and return them as an associative array.
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        console.log(vars)
        return vars;
    }

    function UrunKaliplariGetir() {
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

        debugger;
        if (window.location.href.search("siparisId") != -1) {
            var siparisId = getUrlVars()["siparisId"];

            $.ajax({
                type: "GET",
                url: "/siparis/UrunKaliplari?siparisId=" + siparisId,
                dataType: "html",
                success: function (response) {
                    $("#UrunKaliplari").empty().html(response);
                },
                error: function () {

                },
                complete: function () {
                    maliyetTablosuGetir();
                    OzelSecim();
                }
            })
        }

        else {
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
                    OzelSecim();
                },
                "html");
        }


    }

    function maliyetHesapla() {

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

                teklifTutar = teklifTutar * 1.25 * 1.4;
                $("#TeklifTutari").html(teklifTutar.toFixed(3).replace(".", ","));
                $("#ToplamMaliyet").val(teklifTutar.toFixed(3).replace(".", ","));

                var malzemeMaliyet = ToplamMalzemeMaliyet();
                $("#ToplamMalzemeMaliyet").html(malzemeMaliyet.replace(".", ","));

                var urunMaliyet = ToplamUretimMaliyet();
                $("#ToplamUretimMaliyet").val(urunMaliyet.replace(".", ","));

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

        var idList = $("#kalipIdList").val().split(",");
        var urunId = $("#UrunId").val();
        var excludes = $("#exclude").val().slice(0, -1).split(",");
        var FixedIdList = [];
        for (var i = 0; i < idList.length; i++) {
            if (checkValue(idList[i], excludes) == false) {
                FixedIdList.push(idList[i])
            }
        }
        debugger;
        if (window.location.href.search("siparisId") != -1) {
            var siparisId = getUrlVars()["siparisId"];

            $.ajax({
                type: "GET",
                url: "/siparis/MaliyetFormSiparis?siparisId=" + siparisId,
                dataType: "html",
                success: function (response) {
                    $("#maliyetTablosu").empty().html(response);
                    maliyetHesapla();
                },
                error: function () {

                },
                complete: function () {
                    DisMalzemelerSifirla();
                }
            })
        }
        else {
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

    function InputBulEkle(MaliyetType, KalipId, Value, YaldizId = null, boyaKod = null) {
        debugger;
        var Input = $("td[" + MaliyetType + "-cikar='" + KalipId + "'] input");
        Input.val(Value);

        if (YaldizId != null) {
            Input.attr("YaldizId", YaldizId)
            Input.addClass("changed");
        }

        if (boyaKod != null) {
            Input.attr("boyaKod", boyaKod)
            Input.addClass("changed");
        }
    }

    function CheckString(str) {
        if (typeof str === "string") {
            return true;
        } else {
            return false;
        }
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
                                debugger;
                                var value = $(".Fiyat").val();
                                var YaldizId = null;
                                var boyaKod = null;

                                if ($(".Fiyat").attr("YaldizId") !== null && $(".Fiyat").attr("YaldizId") !== undefined) {
                                    YaldizId = $(".Fiyat").attr("YaldizId");
                                }

                                if ($(".Fiyat").attr("boyaKod") !== null && $(".Fiyat").attr("boyaKod") !== undefined) {
                                    boyaKod = $(".Fiyat").attr("boyaKod");
                                }

                                InputBulEkle(maliyetType, kalipId, value, YaldizId, boyaKod);

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
        $("input.MamulMalzemeMaliyet").val("0");

        array.each(function (index, value) {
            var input = value;
            if ($(input).prop('disabled') == false) {
                var kalipId = $(input).attr("kalipId");
                var mamulInput = $("input.MamulMalzemeMaliyet[kalipId='" + kalipId + "']");
                var f1 = 0;
                var f2 = 0;
                try {
                    f1 = mamulInput.val().replace(",", ".");
                }
                catch (err) {
                    f1 = mamulInput.val();
                }
                try {
                    f2 = $(input).val().replace(",", ".");
                }
                catch (err) {
                    f2 = $(input).val();
                }
                fiyatTofixed = parseFloat(f1) + parseFloat(f2);
                mamulInput.val(fiyatTofixed.toFixed(3).replace(".", ","))
                toplam += parseFloat(f2);
            }
        });
        console.log("toplam malzeme maliyeti : " + toplam);
        return toplam.toFixed(3);
    }

    function ToplamUretimMaliyet() {

        var toplam = 0;
        var array = $("input.birimFiyat[HesplamaType=uretim]").sort();
        $("input.MamulUretimMaliyet").val("0");
        array.each(function (index, value) {
            var input = value;
            if ($(input).prop('disabled') == false) {
                var kalipId = $(input).attr("kalipId");
                var mamulInput = $("input.MamulUretimMaliyet[kalipId='" + kalipId + "']");

                var f1 = 0;
                var f2 = 0;
                try {
                    f1 = mamulInput.val().replace(",", ".");
                }
                catch (err) {
                    f1 = mamulInput.val();
                }
                try {
                    f2 = $(input).val().replace(",", ".");
                }
                catch (err) {
                    f2 = $(input).val();
                }

                fiyatTofixed = parseFloat(f1) + parseFloat(f2);
                mamulInput.val(fiyatTofixed.toFixed(3).replace(".", ","))
                toplam += parseFloat(f2);
            }
        });
        console.log("toplam uretim maliyeti : " + toplam);
        return toplam.toFixed(3);
    }

    function ToplamMaliyetHesapla(tutar) {

        console.log(tutar);
        $.ajax({
            type: "GET",
            url: "/siparis/UsdKurHesapla?tutar=" + tutar,
            dataType: "json",
            success: function (response) {
                if (response.Success == true) {
                    var sonuc = response.Data;
                    console.log("usd kur hesaplama sonuç :" + sonuc);
                    $("#usdKurSonuc").html(sonuc.toFixed(3).replace(".", ","));
                    $("#ToplamMaliyetUsd").val(sonuc.toFixed(3).replace(".", ","));
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
                    $("#eurKurSonuc").html(sonuc.toFixed(3).replace(".", ","));
                    $("#ToplamMaliyetEur").val(sonuc.toFixed(3).replace(".", ","));
                } else {
                    toastr.error(response.Description);
                }
            },
            error: function () {
                toastr.error("bir hata oluştu");
            }
        });

    }

    function SiparisKaydet(SiparisId) {
        debugger;
        var liste = [];

        var SiparisKalipDto = function () {
            this.Maliyet;//decimal
            this.KalipKod;//string
            this.MaliyetType;//string
            this.isEnable;//bool
            this.YaldizId;//string 
            this.BoyaKodId;//int 
        }

        var SiparisDto = function () {
            this.SiparisId;//int
            this.CariId;//int
            this.UrunId;//int
            this.SiparisKalip;//object list
            this.Aciklama;//string
            this.TerminTarihi;//string
            this.SiparisAdet;//string
            this.SiparisDurumId;//int
            this.Include;//string
        }

        var array = $(".birimFiyat").sort();

        array.each(function (index, value) {
            var input = value;
            var dto = new SiparisKalipDto();
            dto.KalipKod = input.getAttribute("KalipKod");
            dto.Maliyet = input.value;
            dto.MaliyetType = input.getAttribute("MaliyetType");

            var YaldizId = input.getAttribute("YaldizId");

            if (YaldizId !== undefined && YaldizId != null) {
                dto.YaldizId = YaldizId;
            } else {
                dto.YaldizId = null;
            }

            var BoyaKod = input.getAttribute("boyaKod");
            if (BoyaKod !== undefined && BoyaKod != null) {
                dto.BoyaKodId = BoyaKod;
            } else {
                dto.BoyaKodId = null;
            }

            if ($(input).prop('disabled') == false) {
                dto.isEnable = true;
            } else {
                dto.isEnable = false;
            }

            liste.push(dto);
        });
        debugger;

        var siparisDto = new SiparisDto();
        siparisDto.SiparisId = SiparisId;
        siparisDto.CariId = $("#SiparisCariId").val();
        siparisDto.UrunId = $("#UrunId").val();
        siparisDto.SiparisKalip = liste;
        siparisDto.ToplamMaliyet = $("#ToplamMaliyet").val();
        siparisDto.ToplamMaliyetUsd = $("#ToplamMaliyetUsd").val();
        siparisDto.ToplamMaliyetEur = $("#ToplamMaliyetEur").val();
        siparisDto.Aciklama = $("#Aciklama").val();
        siparisDto.TerminTarihi = $("#TerminTarihi").val();
        siparisDto.SiparisAdet = $("#SiparisAdet").val();
        siparisDto.SiparisDurumId = $("#SiparisDurumId").val();

        var keys = Object.keys(siparisDto);
        var include = keys.slice(1, keys.length);
        siparisDto.Include = include;

        Post("/siparis/kaydet",
            { siparis: siparisDto },
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
            function () {
            },
            "json");
    }

    function SiparisKalipGuncelle(siparisKalipId, maliyet) {

        Post("/siparis/SiparisKalipGuncelle",
            { siparisKalipId: siparisKalipId, maliyet: maliyet },
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
            function () {
            },
            "json");
    }

    function TopluSiparisKalipGuncelle(toplam, toplamUsd, toplamEur, siparisId) {
        debugger;
        var liste = [];

        var Degisen = function () {
            this.SiparisKalipId;//int
            this.Maliyet;//decimal
            this.isEnable;//bool
            this.YaldizId;//string
            this.BoyaKodId;//int
        }

        var array = $(".changed").sort();
        array.each(function (index, value) {
            var input = value;
            var degisen = new Degisen();
            degisen.SiparisKalipId = input.getAttribute("SiparisKalipId");
            degisen.Maliyet = input.value;
            degisen.YaldizId = null;
            degisen.BoyaKodId = null;
            if ($(input).prop('disabled') == false) {
                degisen.isEnable = true;
            } else {
                degisen.Status = false;
            }
            var yaldiz = $(input).attr("YaldizId");
            var boyaKod = $(input).attr("boyaKod");
            if (yaldiz != null && yaldiz !== undefined) {
                degisen.YaldizId = yaldiz;
            }
            if (boyaKod != null && boyaKod !== undefined) {
                degisen.BoyaKodId = boyaKod;
            }

            liste.push(degisen);
        });

        console.log(liste);

        Post("/siparis/TopluSiparisKalipGuncelle",
            { liste: liste, toplam: toplam, toplamUsd: toplamUsd, toplamEur: toplamEur, siparisId: siparisId },
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
            function () {
            },
            "json");
        debugger;
        var siparis = $("#sipForm").serializeJSON();

        Post("/siparis/guncelle",
            { siparis: siparis },
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
            function () {
            },
            "json");
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
            debugger;
            $(".OzelSecim").hide();
            UrunKaliplariGetir();
        });

        $(document).on("change", ".birimFiyat", function () {
            maliyetHesapla();
        })

        $(document).on("click", "[event='MaliyetDetay']", function (event) {
            event.preventDefault();

            var MaliyetType = $(this).attr("maliyetType");
            var KalipId = $(this).attr("kalipId");
            var PosetParametre = $("#PosetParametre_" + KalipId).val();
            var urunId = $("#UrunId").val();
            debugger;
            if (window.location.href.search("siparisId") != -1) {
                urunId = getUrlVars()["urunId"];
            }
            maliyetDetayGetir(MaliyetType, KalipId, PosetParametre, urunId);
        });

        $(document).on("click", "[event='FaturadanCikar']", function (event) {
            event.preventDefault();
            debugger;
            var KalipId = $(this).attr("KalipId");
            var cikarType = $(this).attr("cikar-type");
            var siparisKalipId = $(this).attr("TargetSiparisKalipId");
            if (siparisKalipId !== undefined) {
                var inp = $(".birimFiyat[SiparisKalipId=" + siparisKalipId + "]");
                $(inp).trigger("change");
                //$(document.getElementById("BirimFiyat")).trigger("change");
            }
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
            if (type == "baskiMakina") {
                HesapFunctionBaskiMakina();
            }


        });

        $(document).on("change", "#yaldizSelect", function (event) {
            event.preventDefault();

            var target = $("#BirimFiyat");
            var birimFiyat = parseFloat($(this).val().replace(",", "."));
            target.val(birimFiyat);
            $(document.getElementById("BirimFiyat")).trigger("change");
        });

        $(document).on("change", "#SiparisKalipSelect", function (event) {
            event.preventDefault();
            var kalipId = $(this).val();
            $("#include").val($("#include").val() + kalipId + ",");
            UrunKaliplariGetir();
        });

        $(document).on("change", ".OzelSecimSelect", function (event) {
            event.preventDefault();
            debugger;
            var kalipId = $(this).val();
            $("#include").val($("#include").val() + kalipId + ",");
            UrunKaliplariGetir();
        });

        $(document).on("click", "[event='siparisKaydet']", function (event) {
            event.preventDefault();
            debugger;
            var siparisId = $(this).attr("siparisId");

            if (siparisId != 0) {
                debugger;
                var toplam = $("#ToplamMaliyet").val();
                var toplamUsd = $("#ToplamMaliyetUsd").val();
                var toplamEur = $("#ToplamMaliyetEur").val();
                var siparisId = siparisId;
                TopluSiparisKalipGuncelle(toplam, toplamUsd, toplamEur, siparisId);
            }
            else {
                SiparisKaydet(siparisId);
            }


        });

        $(document).on("change", ".siparisFiyat", function (event) {
            event.preventDefault();
            debugger;
            $(this).addClass("changed");
        });

        $(document).on("click", ".siparisGuncelle", function (event) {
            event.preventDefault();
            debugger;
            var toplam = $("#ToplamMaliyet").val();
            var toplamUsd = $("#ToplamMaliyetUsd").val();
            var toplamEur = $("#ToplamMaliyetEur").val();
            var siparisId = getUrlVars()["siparisId"];
            TopluSiparisKalipGuncelle(toplam, toplamUsd, toplamEur, siparisId);
        });

        $(document).on("change", ".UploadFile", function (event) {
            event.preventDefault();
            debugger;
            var inputId = $(this).attr("id");
            var TargetDirectory = "SiparisPdf/CariId-" + $("#SiparisCariId").val()+"/";
            var TargetInputId = $(this).attr("TargetInputId");
            UploadImage(inputId, "/File/DosyaKaydet", TargetDirectory, TargetInputId);
        });


        $(document).on("change", "#CariId", function (event) {
            event.preventDefault();
            debugger;
            var id = $(this).val();
            $("#SiparisCariId").val(id);

        });

        $(document).on("change", "#boyaKodSelect", function (event) {
            event.preventDefault();
            debugger;
            var boyaKod = $(this).val();
            $(".Fiyat").attr("boyaKod", boyaKod);

        });

        $(document).on("change", "#YaldizId", function (event) {
            event.preventDefault();
            debugger;
            var YaldizId = $(this).val();
            $(".Fiyat").attr("YaldizId", YaldizId);

        });

        $(document).on("click", "#YaldizEkle", function (event) {
            event.preventDefault();
            debugger;
            var id = 0;
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
                                    Yaldiz.Kaydet();
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
        

        $(document).on("click", ".webviewer", function (event) {
            event.preventDefault();
            debugger;
            var url = $(this).attr("pdfurl");
            bootbox.dialog({
                title: "pdf okuyucu",
                message: `<div id='viewer'></div>`,
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
                            bootbox.hideAll();
                            return false;
                        }
                    }
                }
            });

            

            WebViewer({
                path: 'WebViewer/lib', // path to the PDF.js Express'lib' folder on your server
                licenseKey: 'Insert commercial license key here after purchase',
                initialDoc: url,
                // initialDoc: '/path/to/my/file.pdf',  // You can also use documents on your server
            }, document.getElementById('viewer'))
                .then(instance => {
                    instance.UI.setLanguage('tr');
                    const docViewer = instance.docViewer;
                    const annotManager = instance.annotManager;
                    // call methods from instance, docViewer and annotManager as needed

                    // you can also access major namespaces from the instance as follows:
                    // const Tools = instance.Tools;
                    // const Annotations = instance.Annotations;

                    docViewer.on('documentLoaded', () => {
                        // call methods relating to the loaded document
                    });
                });

        });

    }


    return {

        EventInit: function () {
            handleEvent();
        },
        GetUrlVars: function () {
            getUrlVars();
        }

    };
}();


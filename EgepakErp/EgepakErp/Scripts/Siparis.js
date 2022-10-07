var Siparis = function () {

    var _UsdKur = 0;
    var _EurKur = 0;

    function Kaydet() {

        var validation = ValidateForm.IsValid("SiparisKisitliForm", ValidationFields.SiparisFormFields())
        validation.validate().then(function (status) {
            if (status == 'Valid') {
                var form = $("#SiparisKisitliForm").serializeJSON();
                var keys = Object.keys(form);
                var include = keys.slice(1, keys.length);
                form.Include = include;
                console.log(form);
                Post("/Siparis/KisitliKaydet",
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

    function SiparisUretimAcKapat(siparisId) {
        Post("/siparis/ackapat",
            { siparisId: siparisId },
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

                }

            },
            "json");
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

    function KurGetir(kisaltma) {
        //USD,EUR
        $.ajax({
            type: "GET",
            url: "/siparis/KurGetir?kisaltma=" + kisaltma,
            dataType: "json",
            success: function (response) {
                if (response.Success == true) {
                    var sonuc = response.Data;
                    console.log(kisaltma + " kur :" + sonuc);
                    $("#" + kisaltma + "Kur").val(sonuc);
                    if (kisaltma.toLowerCase() == "usd") {
                        _UsdKur = sonuc;
                    }
                    if (kisaltma.toLowerCase() == "eur") {
                        _EurKur = sonuc;
                    }
                } else {
                    toastr.error(response.Description);
                }
            },
            error: function () {
                toastr.error("Kur çekilemedi oluştu");
            }
        });
    }

    function SatisFiyatHesapla() {
        debugger;
        var array = $(".NakitSatisFiyat").sort();
        var teklifTutar = 0;
        var nakitKatsayi = $("#NakitSatisKatsayi").val().replace(",", ".");
        var Vadelikatsayi = $("#VadeliSatisKatsayi").val().replace(",", ".");

        var UsdKur = $("#UsdKur").val();
        var EurKur = $("#EurKur").val();

        array.each(function (index, value) {

            //nakit satış fiyatı
            var input = value;
            var KalipId = $(input).attr("KalipId");

            var orjinalFiyat = $(".GenelGiderSatisFiyat[KalipId=" + KalipId + "]").val().replace(",", ".");
            var nakitSatisFiyat = parseFloat(orjinalFiyat * nakitKatsayi);

            var targetNakitSatisInput = $(".NakitSatisFiyat[KalipId=" + KalipId + "]");
            targetNakitSatisInput.val(nakitSatisFiyat.toFixed(3).replace(".", ","));

            //vadeli fiyat             
            var targetInputTl = $(".VadeliSatisFiyatTl[KalipId=" + KalipId + "]");
            var targetInputUsd = $(".VadeliSatisFiyatUsd[KalipId=" + KalipId + "]");
            var targetInputEur = $(".VadeliSatisFiyatEur[KalipId=" + KalipId + "]");

            var sonuc = nakitSatisFiyat * Vadelikatsayi;
            targetInputTl.val(sonuc.toFixed(3).replace(".", ","));
            teklifTutar += sonuc;

            sonuc = nakitSatisFiyat * Vadelikatsayi / UsdKur;
            targetInputUsd.val(sonuc.toFixed(3).replace(".", ","));

            sonuc = nakitSatisFiyat * Vadelikatsayi / EurKur;
            targetInputEur.val(sonuc.toFixed(3).replace(".", ","));


        });

        //Teklif Tutar Yazdırma
        var TeklifTutarTl = $("#TeklifTutarTl");
        TeklifTutarTl.val(teklifTutar.toFixed(3).replace(".", ","));

        var TeklifTutarUsd = $("#TeklifTutarUsd");
        TeklifTutarUsd.val((teklifTutar / UsdKur).toFixed(3).replace(".", ","));

        var TeklifTutarEur = $("#TeklifTutarEur");
        TeklifTutarEur.val((teklifTutar / EurKur).toFixed(3).replace(".", ","));

        console.log("teklif tutarı : " + teklifTutar);
    }
    //Teklif tutar tablosunu çalıştır
    function VadeliFiyatHesapla() {

        var array = $(".VadeliSatisFiyatTl").sort();
        var teklifTutar = 0;
        //var UsdKur = $("#UsdKur").val();
        //var EurKur = $("#EurKur").val();

        array.each(function (index, value) {
            //nakit satış fiyatı
            var input = value;
            var fiyat = parseFloat($(input).val().replace(",", "."));
            teklifTutar += fiyat;
        });
        //teklif tutar tl karşılığı
        $("#TeklifTutarTl").val(teklifTutar.toFixed(3).replace(".", ","));

        //teklif tutar dolar karşılığı
        var sonuc = teklifTutar / _UsdKur;
        $("#TeklifTutarUsd").val(sonuc.toFixed(3).replace(".", ","));

        //teklif tutar euro karşılığı
        sonuc = teklifTutar / _EurKur;
        $("#TeklifTutarEur").val(sonuc.toFixed(3).replace(".", ","));
    }

    function maliyetHesapla() {
        debugger;
        var liste = [];
        var Maliyet = function () {
            this.KalipId;
            this.Tutar;
            this.Status;
        }

        var array = $(".birimFiyat").sort();
        array.each(function (index, value) {
            var input = value;
            var maliyet = new Maliyet();
            maliyet.kalipId = input.getAttribute("kalipId");
            maliyet.Tutar = input.value;

            if ($(input).prop('disabled') == false) {
                maliyet.Status = true;;
            } else {
                maliyet.Status = false;
            }
            liste.push(maliyet);
        });

        var nakitKatsayi = $("#NakitKatsayiTemp").val();
        var vadeliKatsayi = $("#VadeliKatsayiTemp").val();


        Post("/siparis/MaliyetHesap",
            { liste: liste, nakitKatsayi: nakitKatsayi, vadeliKatsayi: vadeliKatsayi },
            function (response) {

                $("tr.sum").each(function (index, value) {
                    $(this).remove();
                });

                $("#MaliyetTablo").append(response);

                var malzemeMaliyet = ToplamMalzemeMaliyet();
                $("#ToplamMalzemeMaliyet").html(malzemeMaliyet.replace(".", ","));

                var urunMaliyet = ToplamUretimMaliyet();
                $("#ToplamUretimMaliyet").val(urunMaliyet.replace(".", ","));

                SatisFiyatHesapla();
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

        if (window.location.href.search("siparisId") != -1) {
            var siparisId = getUrlVars()["siparisId"];

            $.ajax({
                type: "GET",
                url: "/siparis/MaliyetFormSiparis?siparisId=" + siparisId,
                dataType: "html",
                success: function (response) {
                    $("#maliyetTablosu").empty().html(response);
                    DisMalzemelerSifirla();
                    maliyetHesapla();
                },
                error: function () {

                },
                complete: function () {

                }
            })
        }
        else {
            Post("/siparis/maliyetForm",
                { idList: FixedIdList, urunId: urunId },
                function (response) {
                    $("#maliyetTablosu").empty().html(response);
                    DisMalzemelerSifirla();
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


    }

    function DisMalzemelerSifirla() {
        var arr = $("td");
        $.each(arr, function (index, val) {
            if ($(val).attr("HazirMalzemeMi") == "True") {
                //$(val).find(".birimFiyat").val("0");
            }
        });
        ///maliyetHesapla();
    }

    function InputBulEkle(MaliyetType, KalipId, Value, YaldizId = null, boyaKod = null, formul, aciklama = null, spreyBoyaKod = null, metalizeId = null) {

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

        if (metalizeId != null) {
            Input.attr("metalizeId", metalizeId)
            Input.addClass("changed");
        }

        if (spreyBoyaKod != null) {
            Input.attr("spreyBoyaKod", spreyBoyaKod)
            Input.addClass("changed");
        }

        if (formul != null) {
            Input.attr("formul", formul)
            Input.addClass("changed");
        }

        if (aciklama != null) {
            Input.attr("aciklama", aciklama)
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

                                var value = $(".Fiyat").val();
                                var YaldizId = null;
                                var boyaKod = null;
                                var spreyBoyaKod = null;
                                var formul = null;
                                var aciklama = null;
                                var metalizeId = null;

                                if ($(".Fiyat").attr("YaldizId") !== null && $(".Fiyat").attr("YaldizId") !== undefined) {
                                    YaldizId = $(".Fiyat").attr("YaldizId");
                                }

                                if ($(".Fiyat").attr("boyaKod") !== null && $(".Fiyat").attr("boyaKod") !== undefined) {
                                    boyaKod = $(".Fiyat").attr("boyaKod");
                                }
                                if ($(".Fiyat").attr("metalizeId") !== null && $(".Fiyat").attr("metalizeId") !== undefined) {
                                    metalizeId = $(".Fiyat").attr("metalizeId");
                                }

                                if ($(".Fiyat").attr("spreyBoyaKod") !== null && $(".Fiyat").attr("spreyBoyaKod") !== undefined) {
                                    spreyBoyaKod = $(".Fiyat").attr("spreyBoyaKod");
                                }

                                if ($(".Fiyat").attr("formul") !== null && $(".Fiyat").attr("formul") !== undefined) {
                                    formul = $("#formul").val();
                                }

                                if ($("#SiparisKalipAciklama").val() !== null && $("#SiparisKalipAciklama").val() !== undefined) {
                                    aciklama = $("#SiparisKalipAciklama").val();
                                }

                                InputBulEkle(maliyetType, kalipId, value, YaldizId, boyaKod, formul, aciklama, spreyBoyaKod, metalizeId);

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
                KTSelect2.BoyaKod();
                $('#yaldizKodSelect').select2({
                    language: "tr",
                    placeholder: 'Yaldiz secin'
                });
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

    function SiparisKaydet(SiparisId) {
        debugger;
        var liste = [];

        var SiparisKalipDto = function () {
            this.Maliyet;//decimal
            this.KalipKod;//string
            this.KalipAdi;//string
            this.MaliyetType;//string
            this.isEnable;//bool
            this.YaldizKodList;//list<int>
            this.TozBoyaKodList;//string
            this.SpreyBoyaKodId;//int 
            this.Aciklama;//string
            this.Formul;//string
            this.MetalizeKodId;//int
            this.EnjeksiyonRenk;//string
        }

        var SiparisDto = function () {
            this.SiparisId;      //int
            this.CariId;         //int
            this.UrunId;         //int
            this.SiparisKalip;   //object list
            this.TeklifFiyat;    //decimal
            this.TeklifFiyatUsd; //decimal
            this.TeklifFiyatEur; //decimal
            this.NakitKatsayi;   //decimal
            this.VadeliKatsayi;  //decimal
            this.Aciklama;       //string
            this.TerminTarihi;   //string
            this.SiparisAdet;    //string
            this.SiparisDurumId; //int
            this.Include;        //string
        }

        var array = $(".birimFiyat").sort();

        array.each(function (index, value) {
            var input = value;
            var dto = new SiparisKalipDto();
            dto.KalipKod = input.getAttribute("KalipKod");
            dto.KalipAdi = input.getAttribute("KalipAdi");
            dto.Maliyet = input.value;
            dto.MaliyetType = input.getAttribute("MaliyetType");
            dto.EnjeksiyonRenk = $("#enj_renk_" + $(input).attr("KalipId")).val();
            var YaldizId = input.getAttribute("YaldizId");

            if (YaldizId !== undefined && YaldizId != null) {
                dto.YaldizKodList = YaldizId;
            } else {
                dto.YaldizKodList = null;
            }

            var BoyaKod = input.getAttribute("boyaKod");
            if (BoyaKod !== undefined && BoyaKod != null) {
                dto.TozBoyaKodList = BoyaKod;
            } else {
                dto.TozBoyaKodList = null;
            }

            var metalizeId = input.getAttribute("metalizeId");
            if (metalizeId !== undefined && metalizeId != null) {
                dto.MetalizeKodId = metalizeId;
            } else {
                dto.MetalizeKodId = null;
            }


            var SpreyBoyaKod = input.getAttribute("spreyBoyaKod");
            if (SpreyBoyaKod !== undefined && SpreyBoyaKod != null) {
                dto.SpreyBoyaKodId = SpreyBoyaKod;
            } else {
                dto.SpreyBoyaKodId = null;
            }

            var Aciklama = input.getAttribute("Aciklama");
            if (Aciklama !== undefined && Aciklama != null) {
                dto.Aciklama = Aciklama;
            } else {
                dto.Aciklama = null;
            }

            var Formul = input.getAttribute("Formul");
            if (Formul !== undefined && Formul != null) {
                dto.Formul = Formul;
            } else {
                dto.Formul = null;
            }

            if ($(input).prop('disabled') == false) {
                dto.isEnable = true;
            } else {
                dto.isEnable = false;
            }

            liste.push(dto);
        });


        var siparisDto = new SiparisDto();
        siparisDto.SiparisId = SiparisId;
        siparisDto.CariId = $("#SiparisCariId").val();
        siparisDto.UrunId = $("#UrunId").val();
        siparisDto.SiparisKalip = liste;
        siparisDto.TeklifFiyat = $("#TeklifTutarTl").val();
        siparisDto.TeklifFiyatUsd = $("#TeklifTutarUsd").val();
        siparisDto.TeklifFiyatEur = $("#TeklifTutarEur").val();
        siparisDto.NakitKatsayi = $("#NakitSatisKatsayi").val();
        siparisDto.VadeliKatsayi = $("#VadeliSatisKatsayi").val();
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

    function TopluSiparisKalipGuncelle(toplam, toplamUsd, toplamEur, siparisId, nakitKatsayi, vadeliKatsayi) {

        var liste = [];

        var Degisen = function () {
            this.SiparisKalipId;//int
            this.Maliyet;//decimal
            this.isEnable;//bool
            this.YaldizId;//string
            this.TozBoyaKodList;//int
            this.SpreyBoyaKodId;//int
            this.Aciklama;//string
            this.Formul;//string
            this.MetalizeKodId;//string
        }

        var array = $(".changed").sort();
        array.each(function (index, value) {
            var input = value;
            var degisen = new Degisen();
            degisen.SiparisKalipId = input.getAttribute("SiparisKalipId");
            degisen.Maliyet = input.value;
            degisen.YaldizKodList = null;
            degisen.TozBoyaKodList = null;
            degisen.SpreyBoyaKodId = null;
            degisen.Aciklama = null;
            degisen.Formul = null;

            if ($(input).prop('disabled') == false) {
                degisen.isEnable = true;
            } else {
                degisen.Status = false;
            }
            var yaldiz = $(input).attr("YaldizId");
            var boyaKod = $(input).attr("boyaKod");
            var spreyBoyaKod = $(input).attr("spreyBoyaKod");
            var aciklama = $(input).attr("aciklama");
            var formul = $(input).attr("formul");
            var metalizeId = $(input).attr("metalizeId");

            if (yaldiz != null && yaldiz !== undefined) {
                degisen.YaldizKodList = yaldiz;
            }
            if (boyaKod != null && boyaKod !== undefined) {
                degisen.TozBoyaKodList = boyaKod;
            }
            if (metalizeId != null && metalizeId !== undefined) {
                degisen.MetalizeKodId = metalizeId;
            }
            if (spreyBoyaKod != null && spreyBoyaKod !== undefined) {
                degisen.SpreyBoyaKodId = spreyBoyaKod;
            }

            if (aciklama != null && aciklama !== undefined) {
                degisen.Aciklama = aciklama;
            }
            if (formul != null && formul !== undefined) {
                degisen.Formul = formul;
            }

            liste.push(degisen);
        });

        console.log(liste);

        var sendObj = {
            liste: liste,
            toplam: toplam,
            toplamUsd: toplamUsd,
            toplamEur: toplamEur,
            siparisId: siparisId,
            nakitKatsayi: nakitKatsayi,
            vadeliKatsayi: vadeliKatsayi
        }
        Post("/siparis/TopluSiparisKalipGuncelle",
            sendObj,
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
        $(document).on("click", "[event='SiparisKisitliFormPopup']", function (e) {

            e.preventDefault();
            var id = $(this).attr("id");
            Post("/siparis/form",
                { id: id },
                function (response) {
                    bootbox.dialog({
                        title: "Sipariş form",
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
                    KTSelect2.Siparis();
                    KTSummernoteDemo.init();
                },
                "html");
        });
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

            $(".OzelSecim").hide();
            UrunKaliplariGetir();
        });

        $(document).on("change", ".birimFiyat", function () {
            maliyetHesapla();
        });

        $(document).on("click", "[event='MaliyetDetay']", function (event) {
            event.preventDefault();

            var MaliyetType = $(this).attr("maliyetType");
            var KalipId = $(this).attr("kalipId");
            var PosetParametre = $("#PosetParametre_" + KalipId).val();
            var urunId = $("#UrunId").val();

            if (window.location.href.search("siparisId") != -1) {
                urunId = getUrlVars()["urunId"];
            }
            maliyetDetayGetir(MaliyetType, KalipId, PosetParametre, urunId);
        });

        $(document).on("click", "[event='FaturadanCikar']", function (event) {
            event.preventDefault();

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

            var kalipId = $(this).val();
            $("#include").val($("#include").val() + kalipId + ",");
            UrunKaliplariGetir();
        });

        $(document).on("click", "[event='siparisKaydet']", function (event) {
            event.preventDefault();
            debugger;
            var siparisId = $(this).attr("siparisId");

            if (siparisId != 0) {

                var toplam = $("#TeklifTutarTl").val();
                var toplamUsd = $("#TeklifTutarUsd").val();
                var toplamEur = $("#TeklifTutarEur").val();
                var siparisId = siparisId;
                var nakitKatsayi = $("#NakitSatisKatsayi").val();
                var vadeliKatsayi = $("#VadeliSatisKatsayi").val();
                TopluSiparisKalipGuncelle(toplam, toplamUsd, toplamEur, siparisId, nakitKatsayi, vadeliKatsayi);
            }
            else {
                SiparisKaydet(siparisId);
            }


        });

        $(document).on("change", ".siparisFiyat", function (event) {
            event.preventDefault();
            $(this).addClass("changed");
        });

        $(document).on("change", "#NakitSatisKatsayi", function (event) {
            event.preventDefault();
            var katsayi = $(this).val();
            $("#NakitKatsayiTemp").val(katsayi);
            SatisFiyatHesapla();
        });

        $(document).on("change", "#VadeliSatisKatsayi", function (event) {
            event.preventDefault();
            debugger;
            var katsayi = $(this).val();
            $("#VadeliKatsayiTemp").val(katsayi);
            SatisFiyatHesapla();
        });

        $(document).on("change", ".GenelGiderSatisFiyat", function (event) {
            event.preventDefault();
            SatisFiyatHesapla();
        });

        //Teklif Tutarının tl karşılığı değişirse dolar ve euro karşılığını güncellle
        $(document).on("change", "#TeklifTutarTl", function (event) {
            event.preventDefault();

            var fiyat = $(this).val().replace(",", ".");

            var UsdSonuc = fiyat / _UsdKur;
            var EurSonuc = fiyat / _EurKur;

            $("#TeklifTutarUsd").val(UsdSonuc.toFixed(3).replace(".", ","));
            $("#TeklifTutarEur").val(EurSonuc.toFixed(3).replace(".", ","));

        });

        //Teklif Tutarının dolar karşılığı değişirse tl ve euro karşılığını güncellle
        $(document).on("change", "#TeklifTutarUsd", function (event) {
            event.preventDefault();

            var fiyat = $(this).val().replace(",", ".");

            var TlSonuc = fiyat * _UsdKur;
            var EurSonuc = TlSonuc / _EurKur;

            $("#TeklifTutarTl").val(TlSonuc.toFixed(3).replace(".", ","));
            $("#TeklifTutarEur").val(EurSonuc.toFixed(3).replace(".", ","));

        });

        //Teklif Tutarının euro karşılığı değişirse tl ve dolar karşılığını güncellle
        $(document).on("change", "#TeklifTutarEur", function (event) {
            event.preventDefault();

            var fiyat = $(this).val().replace(",", ".");

            var TlSonuc = fiyat * _EurKur;
            var UsdSonuc = TlSonuc / _UsdKur;

            $("#TeklifTutarTl").val(TlSonuc.toFixed(3).replace(".", ","));
            $("#TeklifTutarUsd").val(UsdSonuc.toFixed(3).replace(".", ","));
        });


        //sipariş formu kalıp nakit satış alanında değişiklik olursa
        $(document).on("change", ".NakitSatisFiyat", function (event) {
            event.preventDefault();


            var kalipId = $(this).attr("KalipId");
            //var UsdKur = $("#UsdKur").val();
            //var EurKur = $("#EurKur").val();

            var value = parseFloat($(this).val().replace(",", "."));

            //Vadeli satış katsayısı
            var katsayi = $("#VadeliSatisKatsayi").val().replace(",", ".");

            //orjinal değer
            var sonuc = value * katsayi;
            $(".VadeliSatisFiyatTl[KalipId=" + kalipId + "]").val(sonuc.toFixed(3));

            //dolar karşılığı
            sonuc = value * katsayi / _UsdKur;
            $(".VadeliSatisFiyatUsd[KalipId=" + kalipId + "]").val(sonuc.toFixed(3));

            //euro karşılığı
            sonuc = value * katsayi / _EurKur;
            $(".VadeliSatisFiyatEur[KalipId=" + kalipId + "]").val(sonuc.toFixed(3));

            //Teklif tutar tablosunu çalıştır
            VadeliFiyatHesapla();
        });

        //sipariş formu kalıp vadeli satış alanında değişiklik olursa
        $(document).on("change", ".VadeliSatisFiyatTl", function (event) {
            event.preventDefault();

            VadeliFiyatHesapla();
            //var eskiTutar = $(this).attr("EskiTutar").replace(",", ".");
            //var kalipId = $(this).attr("KalipId");
            //var UsdKur = $("#UsdKur").val(); 
            //var EurKur = $("#EurKur").val(); 

            //var value = parseFloat($(this).val().replace(",", "."));
            //var sonuc = value / UsdKur;
            //$(".VadeliSatisFiyatUsd[KalipId=" + kalipId + "]").val(sonuc);

            //sonuc = value / EurKur;
            //$(".VadeliSatisFiyatEur[KalipId=" + kalipId + "]").val(sonuc);

            ////teklif tutar güncelle
            //var teklifTutarInput = $("#TeklifTutarTl");
            //var teklifTutar = $("#TeklifTutarTl").val().replace(",", ".");
            //teklifTutar -= eskiTutar;
            //teklifTutar += value;
            //teklifTutarInput.val(teklifTutar);
            //$(this).attr("EskiTutar", value);
        });




        $(document).on("click", ".siparisGuncelle", function (event) {
            event.preventDefault();

            var toplam = $("#TeklifTutarTl").val();
            var toplamUsd = $("#TeklifTutarUsd").val();
            var toplamEur = $("#TeklifTutarEur").val();
            var siparisId = getUrlVars()["siparisId"];
            var nakitKatsayi = $("#NakitSatisKatsayi").val();
            var vadeliKatsayi = $("#VadeliSatisKatsayi").val();
            TopluSiparisKalipGuncelle(toplam, toplamUsd, toplamEur, siparisId, nakitKatsayi, vadeliKatsayi);
        });

        $(document).on("change", ".UploadFile", function (event) {
            event.preventDefault();

            var inputId = $(this).attr("id");
            var TargetDirectory = "SiparisPdf/CariId-" + $("#SiparisCariId").val() + "/";
            var TargetInputId = $(this).attr("TargetInputId");
            UploadImage(inputId, "/File/DosyaKaydet", TargetDirectory, TargetInputId);
        });


        $(document).on("change", "#CariId", function (event) {
            event.preventDefault();

            var id = $(this).val();
            $("#SiparisCariId").val(id);

        });

        $(document).on("change", "#boyaKodSelect", function (event) {
            event.preventDefault();
            var boyaKod = $(this).val();
            $(".Fiyat").attr("boyaKod", boyaKod);

        });

        $(document).on("change", "#yaldizKodSelect", function (event) {
            event.preventDefault();
            var yaldizList = $(this).val();
            $(".Fiyat").attr("yaldizId", yaldizList);

        });

        $(document).on("change", "#boyaKaplamaKodSelect", function (event) {
            event.preventDefault();
            var metalizeId = $(this).val();
            $(".Fiyat").attr("metalizeId", metalizeId);

        });

        $(document).on("change", "#spreyBoyaKodSelect", function (event) {
            event.preventDefault();

            var boyaKod = $(this).val();
            $(".Fiyat").attr("spreyBoyaKod", boyaKod);

        });

        $(document).on("change", "#YaldizId", function (event) {
            event.preventDefault();

            var YaldizId = $(this).val();
            $(".Fiyat").attr("YaldizId", YaldizId);

        });


        $(document).on("click", "[event='UretimeAcKapat']", function (event) {
            event.preventDefault();

            var siparisId = $(this).attr("SiparisId");
            SiparisUretimAcKapat(siparisId);
        });


        $(document).on("click", "#YaldizEkle", function (event) {
            event.preventDefault();

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

        $(document).ready(function () {
            $('#CariId').select2({
                placeholder: 'Cari Seciniz'
            });
        });
    }


    return {

        EventInit: function () {
            handleEvent();
        },
        GetUrlVars: function () {
            getUrlVars();
        },
        UsdKur: function () {
            return _UsdKur;
        },
        EurKur: function () {
            return _EurKur;
        },
        KurGetir: function (doviz) {
            KurGetir(doviz);
        }
    };
}();


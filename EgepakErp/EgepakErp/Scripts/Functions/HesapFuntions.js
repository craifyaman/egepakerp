function HesapFunctionHammadde() {
    debugger;
    var kalipAgirlik = parseFloat($("#kalipAgirlik").val().replace(",", "."));
    var BirimFiyatInput = $("#BirimFiyat");
    var birimFiyat = BirimFiyatInput.val().replace(",", ".");
    var fireOran = parseFloat($("#fireOran").val().replace(",", "."));
    fireOran = (100 + fireOran) / 100;
    var sonuc = ((birimFiyat * fireOran) / 1000) * kalipAgirlik;
    var target = $(".Fiyat");
    BirimFiyatInput.val(birimFiyat)
    target.val(sonuc.toFixed(3));
}

function HesapFunctionTozBoya() {
    debugger;
    var kalipAgirlik = parseFloat($("#KalipAgirlik").val().replace(",", "."));
    //var boyaMiktar = $("#BoyaMiktar").val().replace(",", ".");
    var birimFiyat = $("#BirimFiyat").val().replace(",", ".");
    var sabit = 12 / 1000;
    var sonuc = sabit * kalipAgirlik * (birimFiyat / 1000);
    var target = $(".Fiyat");
    target.val(sonuc);
}


function HesapFunctionKoli() {
    debugger;
    var BirimFiyat = parseFloat($("#BirimFiyat").val().replace(",", "."));
    //var KalipAgirlik = $("#KalipAgirlik").val().replace(",", ".");
    var Katsayi = parseFloat($("#KoliKatsayi").val().replace(",", "."));
    var Kapasite = $("#Kapasite").val();
    var BanFiyat = parseFloat($("#BantSelect").val().replace(",", "."));
    var sonuc = (BanFiyat * Katsayi + BirimFiyat) / Kapasite;
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(4));
}

function HesapFunctionPoset() {
    debugger;
    var BirimFiyat = parseFloat($("#BirimFiyat").val().replace(",", "."));
    var PosetKatsayi = parseFloat($("#PosetKatsayi").val().replace(",", "."));
    var Kapasite = $("#Kapasite").val();
    var sonuc = (BirimFiyat * PosetKatsayi) / Kapasite;
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(4));
}

function HesapFunctionYaldiz() {
    debugger;
    var katSayi = $("#KatSayi").val().replace(",", ".");
    var birimFiyat = $("#BirimFiyat").val().replace(",", ".");
    var birim = $("#yaldizSelect option:selected").attr("birim");
    var Bolum = 10000;
    if (birim.toLowerCase() == "rulo") {
        Bolum = 744200;
    }
    var sonuc = (birimFiyat * katSayi) / Bolum;
    var target = $(".Fiyat");
    $("#yaldizSabit").html(Bolum);
    target.val(sonuc.toFixed(4));
}

function HesapFunctionEnjeksiyon() {
    var SaatMaliyet = $("#SaatMaliyet").val().replace(",", ".");
    var UretimZamani = $("#UretimZamani").val().replace(",", ".");
    var GozSayisi = $("#GozSayisi").val().replace(",", ".");

    var sonuc = SaatMaliyet / ((3600 / UretimZamani) * GozSayisi);
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(2));
}

function HesapFunctionSicakBaski() {
    debugger
    var SaatAdet = $("#SaatAdet").val().replace(",", ".");
    var SaatMaliyet = $("#SaatMaliyet").val().replace(",", ".");
    var sonuc = SaatMaliyet / SaatAdet;
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(2));
}

function HesapFunctionMontaj() {
    debugger
    var SaatAdet = $("#SaatAdet").val().replace(",", ".");
    var SaatMaliyet = $("#SaatMaliyet").val().replace(",", ".");
    var sonuc = SaatMaliyet / SaatAdet;
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(3));
}

function HesapFunctionBaskiMakina() {
    debugger
    var SaatAdet = $("#SaatAdet").val().replace(",", ".");
    var SaatMaliyet = $("#SaatMaliyet").val().replace(",", ".");
    var sonuc = SaatMaliyet / SaatAdet;
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(2));
}

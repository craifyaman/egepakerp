function HesapFunctionHammadde() {
    var kalipAgirlik = parseFloat($("#kalipAgirlik").val().replace(",", "."));
    var BirimFiyatInput = $("#BirimFiyat");
    var birimFiyat = BirimFiyatInput.val().replace(",", ".");
    var sonuc = (birimFiyat / 1000) * kalipAgirlik;
    var target = $(".Fiyat");

    BirimFiyatInput.val(birimFiyat)
    target.val(sonuc.toFixed(2));
}

function HesapFunctionTozBoya() {
    var kalipAgirlik = parseFloat($("#KalipAgirlik").val().replace(",", "."));
    var boyaMiktar = $("#BoyaMiktar").val().replace(",", ".");
    var birimFiyat = $("#BirimFiyat").val().replace(",", ".");
    var sonuc = kalipAgirlik * boyaMiktar * birimFiyat;
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(2));
}


function HesapFunctionKoli() {
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
    var katSayi = $("#KatSayi").val().replace(",", ".");
    var birimFiyat = $("#BirimFiyat").val().replace(",", ".");
    var sonuc = (birimFiyat * katSayi) / 10000;
    var target = $(".Fiyat");
    target.val(sonuc.toFixed(4));
}

function HesapFunctionEnjeksiyon() {
    var SaatMaliyet = $("#SaatMaliyet").val().replace(",", ".");
    var UretimZamani = $("#UretimZamani").val().replace(",", ".");
    var GozSayisi = $("#GozSayisi").val().replace(",", ".");
    var sonuc = ( (3600 / UretimZamani) * GozSayisi ) / (SaatMaliyet);
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
    target.val(sonuc.toFixed(2));
}
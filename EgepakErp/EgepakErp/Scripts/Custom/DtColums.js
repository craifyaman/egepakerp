﻿var DtColums = function () {

    function cariColumns() {
        var clm = [
            {
                field: 'CariId',
                title: 'ID',
                width: 50
            },
            {
                field: 'Unvan',
                title: 'Unvan'
            },

            {
                field: 'BirincilKisi',
                title: 'Birincil Kişi',
                sortable: false,
            },
            {
                field: 'BirincilKisiEposta',
                title: 'Birincil Kişi Eposta',
                sortable: false,
                width: 200
            },
            {
                field: 'Ilce.Adi',
                title: 'İlçe',
                template: function (row) {
                    return row.Ilce
                }
            },
            {
                field: 'Il.Adi',
                title: 'İl',
                template: function (row) {
                    return row.Il
                }
            },
            {
                field: 'Aktif',
                title: 'Durum',
                template: function (row) {
                    var cls = row.Durum == "Aktif" ? "success" : "danger";
                    return '<span class="label font-weight-bold label-lg label-light-' + cls + ' label-inline">' + row.Durum + '</span>'
                }
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var cls = row.Durum == "Aktif" ? "danger" : "success";
                    var durum = row.Durum == "Aktif" ? "Pasif" : "Aktif";
                    return '\
	                       <a class="btn btn-icon btn-info" event="cariFormPopup" formTitle="'+ row.Unvan + ' Düzenle" href="#" id="' + row.CariId + '" title="Hızlı Düzenle" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-edit" ></i>\
                           </a>\
                             <a class="btn btn-icon btn-primary" href="/cari/detay/'+ row.CariId + '" title="Cari Detay Sayfası" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-search" ></i>\
                           </a>\
                            <a class="btn btn-icon btn-'+ cls + '" event="durum" href="#" id="' + row.CariId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-user" ></i>\
                           </a>\
	                    ';
                },
            }]

        console.log("clm", clm);
        return clm;
    }

    function kisiColumns() {
        var columns = [
            {
                field: 'KisiId',
                title: 'ID',
                width: 50
            },
            {
                field: 'Brans.Adi',
                title: 'Branş',
                template: function (row) {
                    return row.Brans
                }
            },
            {
                field: 'AdSoyad',
                title: 'Ad Soyad'
            },
            {
                field: 'Cari.Unvan',
                title: 'Cari',
                template: function (row) {
                    return row.CariUnvan
                }
            },
            {
                field: 'Gorev',
                title: 'Görev'
            },
            {
                field: 'Eposta',
                title: 'Eposta',
                width: 200
            },
            {
                field: 'Eposta2',
                title: 'Eposta 2',
                width: 200
            },
            {
                field: 'Telefon',
                title: 'Telefon',
                width: 200
            },
            {
                field: 'Telefon2',
                title: 'Telefon 2',
                width: 200
            },
            {
                field: 'Aktif',
                title: 'Aktif',
                template: function (row) {
                    var cls = row.Aktif == "Aktif" ? "success" : "danger";
                    return '<span class="label font-weight-bold label-lg label-light-' + cls + ' label-inline">' + row.Aktif + '</span>'
                }
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var cls = row.Aktif == "Aktif" ? "danger" : "success";
                    var durum = row.Aktif == "Aktif" ? "Pasif" : "Aktif";
                    var str = '<a class="btn btn-icon btn-info mr-1" event="kisiFormPopup" formTitle="' + row.AdSoyad + ' Düzenle" href="#" id="' + row.KisiId + '" title="Hızlı Düzenle" data-toggle="tooltip" data-placement="top"><i class="flaticon-edit" ></i> </a>'
                    //str += ' <a class="btn btn-icon btn-primary mr-1" href = "/kisi/detay/' + row.KisiId + '" title = "Kişi Detay" data - toggle="tooltip" data - placement="top" > <i class="flaticon-search" ></i></a>'
                    str += '<a class="btn btn-icon mr-1 btn-' + cls + '" event="kisiDurum" href="#" id="' + row.KisiId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;
                },
            }];
        return columns;
    }

    function gorusmeColumns() {
        var columns = [
            {
                field: 'GorusmeId',
                title: 'ID',
                width: 50
            },
            {
                field: 'Kisi.AdSoyad',
                title: 'Kisi',
                template: function (row) {
                    return row.Kisi
                }
            },
            {
                field: 'Cari.Unvan',
                title: 'Cari',
                template: function (row) {
                    return row.Cari
                }
            },
            {
                field: 'GorusmeTipId',
                title: 'Görüşme Tipi',
                template: function (row) {
                    return row.GorusmeTip
                }
            },
            {
                field: 'Konu',
                title: 'Konu'
            },
            {
                field: 'Personel.Adi',
                title: 'Personel',
                template: function (row) {
                    return row.Personel
                }
            },
            {
                field: 'GorusmeTarihi',
                title: 'Tarih',
                type: 'date',
                format: 'YYYY/MM/DD'
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {

                    var str = '<a class="btn btn-icon btn-info mr-1" event="gorusmeFormPopup" formTitle="' + row.Konu + ' Düzenle" href="#" id="' + row.GorusmeId + '" title="Hızlı Düzenle" data-toggle="tooltip" data-placement="top"><i class="flaticon-edit" ></i> </a>'
                    //str += ' <a class="btn btn-icon btn-primary mr-1" href = "/kisi/detay/' + row.KisiId + '" title = "Kişi Detay" data - toggle="tooltip" data - placement="top" > <i class="flaticon-search" ></i></a>'
                    //str += '<a class="btn btn-icon mr-1 btn-' + cls + '" event="kisiDurum" href="#" id="' + row.KisiId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;
                },
            }];
        return columns;
    }

    function notColumns() {
        var columns = [
            {
                field: 'NotId',
                title: 'ID',
                width: 50
            },
            {
                field: 'Aciklama',
                title: 'Açıklama'
            },
            {
                field: 'Cari.Unvan',
                title: 'Cari',
                template: function (row) {
                    return row.Cari
                }
            },
            {
                field: 'Personel.Adi',
                title: 'Personel',
                template: function (row) {
                    return row.Personel
                }
            },
            {
                field: 'KayitTarihi',
                title: 'Tarih',
                type: 'date',
                format: 'YYYY/MM/DD'
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {

                    var str = '<a class="btn btn-icon btn-info mr-1" event="notFormPopup" formTitle="' + row.Aciklama + ' Düzenle" href="#" id="' + row.NotId + '" title="Hızlı Düzenle" data-toggle="tooltip" data-placement="top"><i class="flaticon-edit" ></i> </a>'
                    //str += ' <a class="btn btn-icon btn-primary mr-1" href = "/kisi/detay/' + row.KisiId + '" title = "Kişi Detay" data - toggle="tooltip" data - placement="top" > <i class="flaticon-search" ></i></a>'
                    //str += '<a class="btn btn-icon mr-1 btn-' + cls + '" event="kisiDurum" href="#" id="' + row.KisiId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;
                },
            }];
        return columns;
    }

    function hatirlaticiColumns() {
        var clm = [
            {
                field: 'HatirlaticiId',
                title: 'ID',
                width: 50
            },
            {
                field: 'Personel',
                title: 'Personel'
            },

            {
                field: 'HatirlatmaTarihi',
                title: 'Hatirlatma Tarihi'
            },
            {
                field: 'Durum',
                title: 'Durum',
                template: function (row) {
                    var cls = row.Durum == "Açık" ? "success" : "danger";
                    return '<span class="label font-weight-bold label-lg label-light-' + cls + ' label-inline">' + row.Durum + '</span>'
                }
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var cls = row.Durum == "Aktif" ? "danger" : "success";
                    var durum = row.Durum == "Aktif" ? "Pasif" : "Aktif";

                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'formTitle = "Hatırlatıcı Düzenle"';
                    str += 'formId = "hatirlaticiForm"';
                    str += 'formUrl = "/Hatirlatici/Form"';
                    str += 'submitUrl = "/Hatirlatici/Kaydet"';
                    str += 'event="hatirlaticiFormPopup" href = "#" id = "' + row.HatirlaticiId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    //str += ' <a class="btn btn-icon btn-primary mr-1" href = "/kisi/detay/' + row.KisiId + '" title = "Kişi Detay" data - toggle="tooltip" data - placement="top" > <i class="flaticon-search" ></i></a>'
                    //str += '<a class="btn btn-icon mr-1 btn-' + cls + '" event="kisiDurum" href="#" id="' + row.KisiId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;
                },
            }]

        console.log("clm", clm);
        return clm;
    }

    return {
        // public functions
        CariColumns: function () {
            return cariColumns();
        },
        KisiColoumns: function () {
            return kisiColumns();
        },
        GorusmeColumns: function () {
            return gorusmeColumns();
        },
        NotColumns: function () {
            return notColumns();
        },
        HatirlaticiColumns: function () {
            return hatirlaticiColumns();
        },
        GetColoums: function (name) {
            if (name == "cari") {
                return cariColumns();

            } else if (name == "kisi") {
                return kisiColumns();
            }
            else if (name == "gorusme") {
                return gorusmeColumns();
            }
            else if (name == "not") {
                return notColumns();
            }
            else if (name == "hatirlatici") {
                return hatirlaticiColumns();
            }
        }
    };
}();

var DtColums = function () {

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

    function hammaddeColumns() {
        var clm = [
            {
                field: 'Adi',
                title: 'Adı'
            },
            {
                field: 'Birim',
                title: 'Birimi',
                sortable: false,

            },
            {
                field: 'Aciklamasi',
                title: 'Açıklama',
                sortable: false,
                width: 200
            },
            {
                field: 'Durum',
                title: 'Durum'
            },
            //{
            //    field: 'Kaliplar',
            //    title: 'Kalıplar'
            //},
            //{
            //    field: 'KalipKodu',
            //    title: 'Kalıp Kodları'
            //},
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var cls = row.Durum == true ? "warning" : "success";
                    var durum = row.Durum == true ? "Pasif" : "Aktif";
                    var str = '<a class="btn btn-icon btn-info mr-1" event="hammaddeFormPopup" formTitle="' + row.Adi + ' Düzenle" href="#" id="' + row.HammaddeCinsiId + '" title="Hızlı Düzenle" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-edit" ></i>\
                           </a>'

                    str += '<a class="btn btn-icon mr-1 btn-' + cls + '" event="durum" href="#" id="' + row.HammaddeCinsiId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-user" ></i>\
                           </a>';
                    str += '<a class="btn btn-icon btn-danger" event="HammaddeCinsiSil" id="' + row.HammaddeCinsiId + '" href="#" title="Sil" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-cancel" ></i>\
                           </a>';
                    return str;
                },
            }]

        console.log("clm", clm);
        return clm;
    }

    function hammaddeHareketColumns() {
        var clm = [
            {
                field: 'HammaddeHareketId',
                title: '#',
                width: 50
            },
            {
                field: 'FaturaNo',
                title: 'Fatura No'
            },
            {
                field: 'UrunAdi',
                title: 'Ürün'
            },
            {
                field: 'KayitTarihi',
                title: 'Tarih',
                type: 'date',
                format: 'YYYY/MM/DD'
            },
            {
                field: 'HammaddeCinsi.Adi',
                title: 'Sektör',
                template: function (row) {
                    return row.HammaddeCinsi
                }
            },
            {
                field: 'BirimFiyat',
                title: 'Birim Fiyat'
            },
            {
                field: 'ToplamTutar',
                title: 'Toplam Tutar'
            },
            {
                field: 'Doviz.Adi',
                title: 'Para Birimi',
                template: function (row) {
                    return row.Doviz
                }
            },
            {
                field: 'Miktar',
                title: 'Miktar'
            },
            {
                field: 'DolarKuru',
                title: 'DolarKuru'
            },
            {
                field: 'EuroKuru',
                title: 'EuroKuru'
            },
            {
                field: 'HammaddeBirimi.Birimi',
                title: 'HammaddeBirimi',
                template: function (row) {
                    return row.HammaddeBirimi
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
                    var str = '<a href="#" id="' + row.HammaddeHareketId + '" class="btn btn-success font-weight-bolder font-size-sm " event = "hammaddeHareketFormPopup" formTitle = "Düzenle" formId = "hammaddeHareketForm" formUrl = "/HammaddeHareket/Form" submitUrl = "/HammaddeHareket/Kaydet" aria - haspopup="true" aria - expanded="false" > <i class="flaticon-edit"></i> </a >';
                    //str += ' <a class="btn btn-icon btn-primary mr-1" href = "/kisi/detay/' + row.KisiId + '" title = "Kişi Detay" data - toggle="tooltip" data - placement="top" > <i class="flaticon-search" ></i></a>'
                    //str += '<a class="btn btn-icon mr-1 btn-' + cls + '" event="kisiDurum" href="#" id="' + row.KisiId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;
                },
            }

        ];
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
    function urunColumns() {
        var columns = [
            {
                field: 'UrunId',
                title: 'ID',
                width: 50
            },
            {
                field: 'UrunCinsi.Kisaltmasi',
                title: 'Ürün Kodu',
                template: function (row) {
                    return row.UrunKodu
                }

            },
            {
                field: 'Kalip',
                title: 'Kalıp',
                ortable: false,
                template: function (row) {
                    console.log(row);
                    return row.Kalip
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

                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'formTitle = "' + row.UrunKodu + ' Ürün Düzenle"';
                    str += 'formId = "urunForm"';
                    str += 'formUrl = "/Urun/Form"';
                    str += 'submitUrl = "/Urun/Kaydet"';
                    str += 'event="urunFormPopup" href = "#" id = "' + row.UrunId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    str += ' <a class="btn btn-icon btn-danger mr-1" UrunId="' + row.UrunId + '" event="UrunAktifPasif" title = "Ürün sil" data-toggle="tooltip" data-placement="top" > <i class="flaticon-cancel" ></i></a>'
                    //str += '<a class="btn btn-icon mr-1 btn-' + cls + '" event="kisiDurum" href="#" id="' + row.KisiId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;

                    return str;
                },
            }];
        return columns;
    }

    function kalipColumns() {
        var columns = [
            {
                field: 'KalipId',
                title: 'ID',
                width: 50
            },
            {
                field: 'KalipKodu',
                title: 'Kalıp Kodu'

            },
            {
                field: 'Urun',
                title: 'Ürün',
                sortable: false,
                template: function (row) {
                    console.log(row.Urun);
                    var list = row.Urun.toString().split(",");
                    var html = "";
                    $.each(list, function (i, v) {
                        html += "<br/>" + v;
                        if (i == 2) {
                            html += "...";
                            return false;
                        }
                    })
                    return html;
                }
            },
            {
                field: 'Adi',
                title: 'Adı',
                template: function (row) {
                    return row.Adi
                }

            },
            {
                field: 'Birim',
                title: 'Birim'
            },

            {
                field: 'UretimTeminSekli.Adi',
                title: 'Üretim Şekli',
                template: function (row) {
                    return row.UretimTeminSekli
                }

            },
            {
                field: 'ParcaAgirlik',
                title: 'Agırlık(gr)',
                template: function (row) {

                    return row.Agirlik
                }
            },
            {
                field: 'KalipGozSayisi',
                title: 'Göz Sayısı',
                template: function (row) {
                    return row.GozSayisi
                }
            },
            {
                field: 'UretimZamani',
                title: 'Çevrim Süresi(sn)',
                template: function (row) {
                    return row.UretimZamani
                }
            },
            {
                field: 'Hammadde',
                title: 'Hammadde',
                sortable: false,
                template: function (row) {
                    return row.Hammadde
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

                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'formTitle = "' + row.KalipKodu + ' ' + row.Adi + ' Ürün Düzenle"';
                    str += 'formId = "kalipForm"';
                    str += 'formUrl = "/Kalip/Form"';
                    str += 'submitUrl = "/Kalip/Kaydet"';
                    str += 'event="kalipFormPopup" href = "#" id = "' + row.KalipId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    str += ' <a class="btn btn-icon btn-danger mr-1" href = "#" title = "Kalıp Sil" data - toggle="tooltip" data - placement="top" event="KalipSil" KalipId="' + row.KalipId + '"> <i class="flaticon-cancel" ></i></a>'
                    str += '<a class="btn btn-icon mr-1 btn-info" event="KalipDetay" href="#" id="' + row.KalipId + '" title="Kalıp Detay" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;
                },
            }];
        return columns;
    }

    function fiyatColumns() {
        var columns = [
            {
                field: 'FiyatId',
                title: '#',
                width: 50
            },
            {
                field: 'Aciklama',
                title: 'Açıklama'
            },
            {
                field: 'Kod',
                title: 'Kod'
            },
            {
                field: 'Tutar',
                title: 'Tutar'
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {

                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'formTitle = "' + row.FiyatId + ' ' + row.FiyatId + ' Ürün Düzenle"';
                    str += 'formId = "kalipForm"';
                    str += 'formUrl = "/Kalip/Form"';
                    str += 'submitUrl = "/Kalip/Kaydet"';
                    str += 'event="FiyatFormPopup" href = "#" id = "' + row.FiyatId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    str += ' <a class="btn btn-icon btn-danger mr-1" href = "#" title = "Kalıp Sil" data - toggle="tooltip" data - placement="top" event="KalipSil" KalipId="' + row.FiyatId + '"> <i class="flaticon-cancel" ></i></a>'
                    str += '<a class="btn btn-icon mr-1 btn-info" event="KalipDetay" href="#" id="' + row.FiyatId + '" title="Kalıp Detay" data-toggle="tooltip" data-placement="top"><i class="flaticon-user" ></i></a> ';

                    return str;

                    return str;
                },
            }];
        return columns;
    }

    function uretimSabitColumns() {
        var clm = [
            {
                field: 'UretimSabitlerId',
                title: '#'
            },
            {
                field: 'Aciklama',
                title: 'Açıklama'
            },
            {
                field: 'Maliyet',
                title: 'Maliyet'
            },
            {
                field: 'Birim',
                title: 'Birim'
            },
            {
                field: 'Kod',
                title: 'Kod'
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = '<a class="btn btn-icon btn-info mr-1" event="UretimSabitFormPopup"  href="#" id="' + row.UretimSabitlerId + '" title="Hızlı Düzenle" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-edit" ></i>\
                           </a>';

                    return str;
                },
            }]

        return clm;
    }

    function siparisColumns() {
        var clm = [
            {
                field: 'SiparisId',
                title: '#'
            },
            {
                field: 'SiparisAd',
                title: 'Sipariş'
            },
            {
                field: 'Urun',
                title: 'Urun',
                width: 100,
            },
            {
                field: 'Cari',
                title: 'Cari',
                sortable:false
            },
            {
                field: 'Durum',
                title: 'Durum'
            },
          
            //{
            //    field: 'UrunId',
            //    title: 'Ürün Id'
            //},
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    /* 
                     <a class="dropdown-item" href="/siparis/SiparisDetayPdf?siparisId=${row.SiparisId}" target="_blank">
                             <i class="far fa-file-pdf mr-3"></i>  Pdf Dökümü
                            </a>
                     */
                    var str = `
                    <div class="dropdown dropdown-inline mr-4" >
                        <button type="button" class="btn btn-light-primary btn-icon btn-sm" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="ki ki-bold-more-hor"></i>
                        </button>
                        <div class="dropdown-menu">
                           <a class="dropdown-item CPointer" href="/siparis/siparisformu?siparisId=${row.SiparisId}&urunId=${row.UrunId}" target="_blank">
                              <i class="flaticon-edit mr-3" ></i>  Düzenle
                            </a>

                           <a class="dropdown-item CPointer" event="SiparisKisitliFormPopup" id="${row.SiparisId}">
                             <i class="fa-solid fa-truck-fast mr-3"></i>  Hızlı Düzenle
                           </a>

                            <a class="dropdown-item CPointer" href="/siparis/SiparisUretimDetayPdf?siparisId=${row.SiparisId}" target="_blank">
                             <i class="far fa-file-pdf mr-3"></i>  Üretim Pdf Dökümü
                            </a>

                        </div>
                   </div >

`;

                    return str;


                },
            }
        ];
        return clm;
    }

    function boyaKodColumns() {
        var columns = [
            {
                field: 'BoyaKodId',
                title: '#',
                width: 50
            },
            {
                field: 'Aciklama',
                title: 'Açıklama'
            },
            {
                field: 'Kod',
                title: 'Kod'
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {

                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'event="BoyaKodFormPopup" href = "#" id = "' + row.BoyaKodId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >';

                    return str;
                },
            }];
        return columns;
    }
    function boyaKaplamaColumns() {
        var columns = [
            {
                field: 'BoyaKaplamaId',
                title: '#',
                width: 50
            },
            {
                field: 'Aciklama',
                title: 'Açıklama'
            },
            {
                field: 'Kod',
                title: 'Kod'
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {

                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'event="BoyaKaplamaFormPopup" href = "#" id = "' + row.BoyaKaplamaId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >';
                    return str;
                },
            }];
        return columns;
    }
    function yaldizColumns() {
        var columns = [
            {
                field: 'YaldizId',
                title: '#',
                width: 50
            },
            {
                field: 'Aciklama',
                title: 'Açıklama'
            },
            {
                field: 'Cari',
                title: 'Cari'
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {

                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'event="YaldizFormPopup" href = "#" id = "' + row.YaldizId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >';

                    return str;
                },
            }];
        return columns;
    }

    function uretimEmirColumns() {
        var columns = [
            {
                field: 'UretimEmirId',
                title: '#',
                width: 50
            },
            {
                field: 'Siparis',
                title: 'Siparis',
            },
            {
                field: 'Urun',
                title: 'Urun',
            },
            {
                field: 'Parca',
                title: 'Parca',
            },
            {
                field: 'Baslangic',
                title: 'Baslangic',
            },
            {
                field: 'SiparisAdet',
                title: 'Sipariş Adet',
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    
                    var str = `
                    <div class="dropdown dropdown-inline mr-4" >
                        <button type="button" class="btn btn-light-primary btn-icon btn-sm" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="ki ki-bold-more-hor"></i>
                        </button>
                        <div class="dropdown-menu">
                           <a class="dropdown-item CPointer" event="UretimEmirFormPopup" id="${row.UretimEmirId}">
                              <i class="flaticon-edit mr-3" ></i> Hızlı Düzenle
                            </a>

                           <a class="dropdown-item CPointer" event="UretimEmirSil" id="${row.UretimEmirId}">
                             <i class="fa-solid fa-cancel mr-3"></i>  Sil
                           </a>

                        </div>
                   </div >

`;

                    return str;
                },
            }];
        return columns;
    }

    function makineColumns() {
        var columns = [
            {
                field: 'MakineId',
                title: '#',
                width: 50
            },
            {
                field: 'MakineAd',
                title: 'Adı',
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'event="MakineFormPopup" href = "#" id = "' + row.MakineId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    return str;
                },
            }];
        return columns;
    }

    function aksiyonColumns() {
        var columns = [
            {
                field: 'Id',
                title: '#',
                width: 50
            },
            {
                field: 'Aciklama',
                title: 'Açıklama',
            },
            {
                field: 'AksiyonType',
                title: 'AksiyonType',
            },
            {
                field: 'Baslangic',
                title: 'Başlangıç',
            },
            {
                field: 'Bitis',
                title: 'Bitiş',
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'event="UretimEmirFormPopup" href = "#" id = "' + row.UretimEmirId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    return str;
                },
            }];
        return columns;
    }
    function stokHareketColumns() {
        var columns = [
            //{
            //    field: 'Type',
            //    title: 'Tip',
            //},
            {
                field: 'SiparisKod',
                title: 'Siparis Kod',
            },
            {
                field: 'Cari',
                title: 'Cari',
            },
            {
                field: 'KalipKodList',
                title: 'Kalıplar',
            },
            {
                field: 'Yer',
                title: 'Yer',
            },
            {
                field: 'Adet',
                title: 'Depoya Giren',

            },
            {
                field: 'Kalan',
                title: 'Stok',
                width: 50
            },
            //{
            //    field: 'Yaldiz',
            //    title: 'Yaldiz'
            //},
            //{
            //    field: 'BoyaKod',
            //    title: 'BoyaKod'
            //},
            //{
            //    field: 'İşlem',
            //    title: 'İşlem',
            //    sortable: false,
            //    width: 180,
            //    overflow: 'visible',
            //    autoHide: false,
            //    template: function (row) {
            //        var str = '<a class="btn btn-icon btn-info mr-1"';
            //        str += 'event="StokHareketFormPopup" href = "#" id = "' + row.Id + '" title = "Hızlı Düzenle" data-toggle="tooltip" data-placement="top" > <i class="flaticon-edit" ></i> </a >'

            //        str += '<a class="btn btn-icon btn-success mr-1" event="StokGirisHareketFormPopup" href = "#" stokHareketId = "' + row.Id + '" title = "Giriş Hareketi Ekle" data-toggle="tooltip" data-placement="top" > <i class="fa-solid fa-truck-fast"></i></a >'


            //        str += '<a class="btn btn-icon btn-warning mr-1" event="StokCikisHareketFormPopup" href = "#" CariId="' + row.CariId + '" stokHareketId = "' + row.Id + '" title = "Çıkış Hareketi Ekle" data-toggle="tooltip" data-placement="top" > <i class="fas fa-truck-loading"></i></a >'


            //        str += '<a class="btn btn-icon btn-info mr-1" event="CikisHareketListe" href = "#" stokHareketId = "' + row.Id + '" title = "Çıkış Hareket Listesi" data-toggle="tooltip" data-placement="top" > <i class="fa-solid fa-list"></i></a >'

            //        return str;
            //    },
            //}

            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = `
                    <div class="dropdown dropdown-inline mr-4" >
                        <button type="button" class="btn btn-light-primary btn-icon btn-sm" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="ki ki-bold-more-hor"></i>
                        </button>
                        <div class="dropdown-menu">
                           <a class="dropdown-item" event="StokHareketFormPopup" href = "#" id="${row.Id}">
                              <i class="flaticon-edit mr-3" ></i>  Hızlı düzenle
                            </a >
                           
                           <a class="dropdown-item" event="StokGirisHareketFormPopup" href = "#" stokHareketId = "${row.Id}">
                             <i class="fa-solid fa-truck-fast mr-3"></i>  Giriş Hareketi Ekle
                           </a>
             
                            <a class="dropdown-item" event="StokCikisHareketFormPopup" href = "#" CariId="${row.CariId}" stokHareketId = ${row.Id}>
                             <i class="fas fa-truck-loading mr-3"></i>  Çıkış Hareketi Ekle
                            </a>
               
                            <a class="dropdown-item" event="CikisHareketListe" href = "#" stokHareketId = "${row.Id}">
                              <i class="fa-solid fa-list mr-3"></i> Çıkış Hareketleri
                            </a >

                            <a class="dropdown-item" event="GirisHareketListe" href = "#" stokHareketId = "${row.Id}">
                              <i class="fa-solid fa-list mr-3"></i> Giriş Hareketleri
                            </a >
                        </div>
                   </div >

`;

                    return str;
                },
            }


        ];
        return columns;
    }
    function stokCikisHareketColumns() {
        var columns = [
            {
                field: 'Id',
                title: '#',
            },
            {
                field: 'Adet',
                title: 'Adet',
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'event="StokCikisHareketFormPopup" href = "#" id = "' + row.Id + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    return str;
                },
            }
        ];
        return columns;
    }

    function stokGirisHareketColumns() {
        var columns = [
            {
                field: 'Id',
                title: '#',
            },
            {
                field: 'Adet',
                title: 'Adet',
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = '<a class="btn btn-icon btn-info mr-1"';
                    str += 'event="StokGirisHareketFormPopup" href = "#" id = "' + row.Id + '" stokhareketId = "' + row.StokHareketId + '" title = "Hızlı Düzenle" data - toggle="tooltip" data - placement="top" > <i class="flaticon-edit" ></i> </a >'
                    return str;
                },
            }
        ];
        return columns;
    }

    function teklifFormColumns() {
        var columns = [
            {
                field: 'Id',
                title: '#',
            },
            {
                field: 'Cari',
                title: 'Cari',
            },

            {
                field: 'Alan',
                title: 'Alan',
            },

            {
                field: 'AlanEposta',
                title: 'Alan Eposta',
            },

            {
                field: 'AlanBilgi',
                title: 'Alan Bilgi',
            },
            {
                field: 'KayitTarih',
                title: 'KayitTarih',
            },
            {
                field: 'TeslimTarihi',
                title: 'TeslimTarihi',
            },
            {
                field: 'PersonelAd',
                title: 'Personel',
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = `
                    <div class="dropdown dropdown-inline mr-4" >
                        <button type="button" class="btn btn-light-primary btn-icon btn-sm" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="ki ki-bold-more-hor"></i>
                        </button>

                        <div class="dropdown-menu">
                           <a class="dropdown-item" event="TeklifFormFormPopup" href = "#" id="${row.Id}">
                              <i class="flaticon-edit mr-3" ></i>  Hızlı düzenle
                            </a >
                    
                            <a class="dropdown-item" target="_blank" href="teklifform/pdf?formId=${row.Id}&lang=tr" href = "#" id="${row.Id}">
                              <i class="flaticon-edit mr-3" ></i>  Teklif Formu (Türkçe)
                            </a >
    
                            <a class="dropdown-item" target="_blank" href="teklifform/pdf?formId=${row.Id}" href = "#" id="${row.Id}">
                              <i class="flaticon-edit mr-3" ></i>  Teklif Formu (İngilizce)
                            </a >


                        </div>

                   </div >

`;
                    return str;
                },
            }
        ];
        return columns;
    }

    function proformaFaturaFormColumns() {
        var columns = [
            {
                field: 'Id',
                title: '#',
            },
            {
                field: 'Cari',
                title: 'Cari',
            },

            {
                field: 'Firma',
                title: 'Firma',
            },

            {
                field: 'Yetkili',
                title: 'Yetkili',
            },

            {
                field: 'AlanEposta',
                title: 'E-Posta',
            },
            {
                field: 'Tarih',
                title: 'Tarih',
            },
            {
                field: 'TeslimTarihi',
                title: 'TeslimTarihi',
            },
            {
                field: 'İşlem',
                title: 'İşlem',
                sortable: false,
                width: 130,
                overflow: 'visible',
                autoHide: false,
                template: function (row) {
                    var str = `
                    <div class="dropdown dropdown-inline mr-4" >
                        <button type="button" class="btn btn-light-primary btn-icon btn-sm" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="ki ki-bold-more-hor"></i>
                        </button>

                        <div class="dropdown-menu">

                           <a class="dropdown-item" event="ProformaFaturaFormPopup" href = "#" id="${row.Id}">
                              <i class="flaticon-edit mr-3" ></i>  Hızlı düzenle
                            </a >
                    
                            <a class="dropdown-item" target="_blank" href="proformafatura/pdf?faturaId=${row.Id}&lang=tr" href = "#" id="${row.Id}">
                              <i class="flaticon-edit mr-3" ></i>  Proforma Fatura (Türkçe)
                            </a >
    
                            <a class="dropdown-item" target="_blank" href="proformafatura/pdf?faturaId=${row.Id}" href = "#" id="${row.Id}">
                              <i class="flaticon-edit mr-3" ></i>  Proforma Fatura (İngilizce)
                            </a >


                        </div>

                   </div >

`;
                    return str;
                },
            }
        ];
        return columns;
    }

    function enjeksiyonColumns() {
        var columns = [
            {
                field: 'Id',
                title: '#',
            },
            {
                field: 'Kisi',
                title: 'Kisi',
                sortable:false
            },
            {
                field: 'Bolum',
                title: 'Bölüm',
                sortable: false
            },            
            {
                field: 'Parca',
                title: 'Parca',
            },
            {
                field: 'BitenAdet',
                title: 'Adet',
            },
            {
                field: 'KayitTarih',
                title: 'Tarih',
                width:'100px'
            },
            
        ];
        return columns;
    }

    function sevkiyatColumns() {
        var columns = [
            {
                field: 'Id',
                title: '#',
            },
            {
                field: 'Cari',
                title: 'Cari',
                sortable: false
            },
            {
                field: 'Adet',
                title: 'Adet'
            },
            {
                field: 'Aciklama',
                title: 'Açıklama',
            },
            {
                field: 'CikisTarih',
                title: 'Çıkış Tarihi',
                width:'100px'
            },

        ];
        return columns;
    }

    return {
        // public functions
        CariColumns: function () {
            return cariColumns();
        },
        HammaddeColumns: function () {
            return hammaddeColumns();
        },
        HammaddeHareketColumns: function () {
            return hammaddeHareketColumns();
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
        UrunColumns: function () {
            return urunColumns();
        },
        KalipColumns: function () {
            return kalipColumns();
        },
        FiyatColumns: function () {
            return fiyatColumns();
        },
        UretimSabitColumns: function () {
            return uretimSabitColumns();
        },
        SiparisColumns: function () {
            return siparisColumns();
        },
        BoyaKodColumns: function () {
            return boyaKodColumns();
        },
        BoyaKaplamaColumns: function () {
            return boyaKaplamaColumns();
        },
        YaldizColumns: function () {
            return yaldizColumns();
        },
        UretimEmirColumns: function () {
            return uretimEmirColumns();
        },
        MakineColumns: function () {
            return makineColumns();
        },
        AksiyonColumns: function () {
            return aksiyonColumns();
        },
        StokHareketColumns: function () {
            return stokHareketColumns();
        },
        StokCikisHareketColumns: function () {
            return stokCikisHareketColumns();
        },
        StokGirisHareketColumns: function () {
            return stokGirisHareketColumns();
        },
        TeklifFormColumns: function () {
            return teklifFormColumns();
        },
        ProformaFormColumns: function () {
            return proformaFaturaFormColumns();
        },
        EnjeksiyonColumns: function () {
            return enjeksiyonColumns();
        },       
        SevkiyatColumns: function () {
            return sevkiyatColumns();
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
            else if (name == "urun") {
                return urunColumns();
            }
            else if (name == "kalip") {
                return kalipColumns();
            }
            else if (name == "hammaddehareket") {
                return hammaddeHareketColumns();
            }
            else if (name == "uretimsabit") {
                return uretimSabitColumns();
            }
            else if (name == "siparis") {
                return siparisColumns();
            }
            else if (name == "boyakod") {
                return boyaKodColumns();
            }
            else if (name == "yaldiz") {
                return yaldizColumns();
            }
            else if (name == "uretimemir") {
                return uretimEmirColumns();
            }
            else if (name == "makine") {
                return makineColumns();
            }
            else if (name == "aksiyon") {
                return aksiyonColumns();
            }
            else if (name == "boyakaplama") {
                return boyaKaplamaColumns();
            }



        }
    };
}();


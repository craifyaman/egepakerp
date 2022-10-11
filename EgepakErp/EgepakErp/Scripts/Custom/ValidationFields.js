var ValidationFields = function () {

    function cariFormFields() {
        var fields = {
            Unvan: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Eposta: {
                validators: {
                    emailAddress: { message: 'Gecerli Bir Eposta Adresi Girin', },
                }
            },
            WebSitesi: {
                validators: {
                    uri: { message: 'Gecerli Bir url giriniz', },
                }
            }
        };
        return fields;
    }

    function gorusmeFormFields() {
        var fields = {
            KisiId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            GorusmeTipId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Konu: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' },
                }
            },
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' },
                }
            }
        };
        return fields;
    }
    function notFormFields() {
        var fields = {
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }
    function kisiFormFields() {
        var fields = {
            AdSoyad: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            CariId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            BransId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Eposta: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' },
                    emailAddress: { message: 'Gecerli Bir Eposta Adresi Girin', },
                }
            },
            Eposta2: {
                validators: {
                    emailAddress: { message: 'Gecerli Bir Eposta Adresi Girin', },
                }
            },
            Telefon: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' },
                    stringLength: {
                        min: 10,
                        max: 10,
                        message: 'Başında 0 olmadan 10 haneli telefon numarası giriniz'
                    },
                    integer: {
                        message: 'Sadece Rakam Giriniz',
                    },
                }
            },
        };
        return fields;
    }

    function hatirlaticiFormFields() {
        var fields = {
            PersonelId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            HatirlatmaTarihi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' },
                }
            }
        };
        return fields;
    }

    function hammaddeFormFields() {
        var fields = {
            Adi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Kisaltmasi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Aciklamasi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function hammaddeHareketFormFields() {
        var fields = {
            UrunAdi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            HammaddeGirisTarihi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            FaturaNo: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            TedarikciId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            HammaddeCinsiId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            BirimFiyat: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            DovizId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Miktar: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            HammaddeBirimiId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }


    function urunFormFields() {
        var fields = {
            UrunCinsiId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            UrunNo: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function kalipFormFields() {
        var fields = {
            KalipNo: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Adi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            //ParcaAgirlik: {
            //    validators: {
            //        integer: {
            //            message: 'Rakam Giriniz',
            //            thousandsSeparator: '',
            //            decimalSeparator: ',',
            //        },
            //    }
            //},
            KalipGozSayisi: {
                validators: {
                    integer: {
                        message: 'Rakam Giriniz'
                    },
                }
            },
            UretimZamani: {
                validators: {
                    integer: {
                        message: 'Rakam Giriniz'
                    },
                }
            }
        };
        return fields;
    }

    function pompaFiyatFormFields() {
        var fields = {
            Adi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            PompaKod: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Tutar: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function uretimSabitFormFields() {
        var fields = {
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Maliyet: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Birim: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Kod: {
                validators: {
                    notEmpty: { message: 'Kod' }
                }
            }
        };
        return fields;
    }
    function boyaKodFormFields() {
        var fields = {
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Kod: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function boyaKaplamaFormFields() {
        var fields = {
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Kod: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            BoyaKaplamaTypeId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function yaldizFormFields() {
        var fields = {
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function uretimEmirFormFields() {
        var fields = {
            UretimEmirDurumId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Baslangic: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Bitis: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            SiparisAdet: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            UretilenAdet: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            SiparisKalipId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            

        };
        return fields;
    }

    function aksiyonFormFields() {

        var fields = {
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            //AksiyonTypeId: {
            //    validators: {
            //        notEmpty: { message: 'Bos Birakilamaz' }
            //    }
            //},
            Baslangic: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Bitis: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }

        };
        return fields;
    }

    function stokHareketFormFields() {

        var fields = {
            Adet: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function stokCikisHareketFormFields() {

        var fields = {
            Adet: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function stokGirisHareketFormFields() {

        var fields = {
            Adet: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }


    function makineFormFields() {
        var fields = {
            MakineAdi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function siparisFormFields() {
        var fields = {
            TeklifFiyati: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
        };
        return fields;
    }

    function teklifFormFormFields() {
        var fields = {
            CariId: {
                validators: {
                    notEmpty: { message: 'Cari Seçin' }
                }
            }
        };
        return fields;
    }

    function proformaFaturaFormFields() {
        var fields = {
            CariId: {
                validators: {
                    notEmpty: { message: 'Cari Seçin' }
                }
            }
        };
        return fields;
    }

    function uretimAksiyonFormFields() {
        var fields = {
            UretilenAdet: {
                validators: {
                    notEmpty: { message: 'Üretilen adet girin' }
                }
            },
            MakineId: {
                validators: {
                    notEmpty: { message: 'Makine seçin' }
                }
            },
            
        };
        return fields;
    }

    function uretimEmirAksiyonFormFields() {
        var fields = {
            BitenAdet: {
                validators: {
                    notEmpty: { message: 'Boş olamaz' }
                }
            },

        };
        return fields;
    }
    

    return {
        // public functions
        GorusmeFormFields: function () {
            return gorusmeFormFields();
        },
        NotFormFields: function () {
            return notFormFields();
        },
        KisiFormFields: function () {
            return kisiFormFields();
        },
        CariFormFields: function () {
            return cariFormFields();
        },
        HatirlaticiFormFields: function () {
            return hatirlaticiFormFields();
        },
        UrunFormFields: function () {
            return urunFormFields();
        },
        KalipFormFields: function () {
            return kalipFormFields();
        },
        HammaddeFormFields: function () {
            return hammaddeFormFields();
        },
        HammaddeHareketFormFields: function () {
            return hammaddeHareketFormFields();
        },
        PompaFiyatFormFields: function () {
            return pompaFiyatFormFields();
        },
        UretimSabitFormFields: function () {
            return uretimSabitFormFields();
        },
        BoyaKodFormFields: function () {
            return boyaKodFormFields();
        },
        BoyaKaplamaFormFields: function () {
            return boyaKaplamaFormFields();
        },
        YaldizFormFields: function () {
            return yaldizFormFields();
        },
        UretimEmirFormFields: function () {
            return uretimEmirFormFields();
        },
        MakineFormFields: function () {
            return makineFormFields();
        },
        AksiyonFormFields: function () {
            return aksiyonFormFields();
        },
        StokHareketFormFields: function () {
            return stokHareketFormFields();
        },
        StokCikisHareketFormFields: function () {
            return stokCikisHareketFormFields();
        },
        StokGirisHareketFormFields: function () {
            return stokGirisHareketFormFields();
        },
        SiparisFormFields: function () {
            return siparisFormFields();
        },
        TeklifFormFormFields: function () {
            return teklifFormFormFields();
        },
        ProformaFaturaFormFields: function () {
            return proformaFaturaFormFields();
        },
        UretimAksiyonFormFields: function () {
            return uretimAksiyonFormFields();
        },
        UretimEmirAksiyonFormFields: function () {
            return uretimEmirAksiyonFormFields();
        }
        

    };
}();


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
            KalipOzellik: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Adi: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            UretimTeminSekliId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            ParcaAgirlik: {
                validators: {
                    integer: {
                        message: 'Rakam Giriniz',
                        thousandsSeparator: '',
                        decimalSeparator: ',',
                    },
                }
            },
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

    function hammaddeHareketFormFields() {
        var fields = {
            
            HammaddeCinsiId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            TedarikciId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            DovizId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            MarkaId: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            KdvOranı: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            ToplamTutar: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            KdvTutari: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            },
            Aciklama: {
                validators: {
                    notEmpty: { message: 'Bos Birakilamaz' }
                }
            }
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
        }

        
    };
}();


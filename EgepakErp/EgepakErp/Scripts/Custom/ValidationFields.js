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
                        min:10, 
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
        }
    };
}();


var Personel = function () {
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    $.fn.hasAttr = function (name) {
        return this.attr(name) !== undefined;
    };
    var handleList = function () {

        var datatable = $('#kt_datatable').KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        //url: HOST_URL + '/api/datatables/demos/default.php',
                        url: '/personel/liste',
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
            columns: [
                {
                    field: 'PersonelId',
                    title: 'ID',
                    width: 50
                },
                {
                    field: 'Kod',
                    title: 'Kod'
                },

                {
                    field: 'Adi',
                    title: 'Ad Soyad'
                },
                {
                    field: 'Departman.Adi',
                    title: 'Departman'
                },
                {
                    field: 'PersonelTip.Adi',
                    title: 'Personel Tip'
                },
                {
                    field: 'Durum',
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
	                       <a class="btn btn-icon btn-info" href="/personel/detay/'+ row.PersonelId + '" title="Düzenle" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-edit" ></i>\
                           </a>\
                            <a class="btn btn-icon btn-'+ cls + ' durum" href="#" id="' + row.PersonelId + '" title="' + durum + ' Yap" data-toggle="tooltip" data-placement="top">\
                                <i class="flaticon-user" ></i>\
                           </a>\
	                    ';
                    },
                }],

        });

        $('.form-control').on('change', function () {
            datatable.search($(this).val().toLowerCase(), $(this).attr("id"));
        });

        $(document).on("click", ".durum", function (evet) {
            event.preventDefault();
            var id = $(this).attr("id");

            bootbox.dialog({
                title: 'Personel Durum Güncelle',
                message: 'İşleme Devam Etmek İstiyor musunuz?',
                size: 'large',
                buttons: {
                    cancel: {
                        label: "Vazgeç",
                        className: 'btn-danger',
                        callback: function () { }
                    },
                    ok: {
                        label: "Devam",
                        className: 'btn-info',
                        callback: function () {
                            DurumGuncelle(id);
                        }
                    }
                }
            });
        });

        function DurumGuncelle(id) {
            Post('/Personel/DurumGuncelle',
                { id: id },
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
                    //Complete
                    setTimeout(function () { $('#kt_datatable').KTDatatable('reload'); }, 3000)
                },
                "json");
        }


    };

    var handleForm = function () {
        var validation;
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/

        $('#personekEkle').on('click', function (e) {
            e.preventDefault();
            Post('/Personel/Form',
                {},
                function (response) {
                    bootbox.dialog({
                        title: 'Personel Formu',
                        message: response,
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
                    //Complete
                },
                "html");
        });

        function Kaydet() {
            validation = FormValidation.formValidation(
                KTUtil.getById('form'),
                {
                    fields: {
                        Adi: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }
                            }
                        },
                        DepartmanId: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }

                            }
                        },
                        PersonelTipId: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }

                            }
                        },
                        Eposta: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' },
                                emailAddress: { message: 'Gecerli Bir Eposta Adresi Girin', },
                            }
                        }
                    },
                    plugins: {
                        trigger: new FormValidation.plugins.Trigger(),
                        submitButton: new FormValidation.plugins.SubmitButton(),
                        //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                        bootstrap: new FormValidation.plugins.Bootstrap()
                    }
                }
            );
            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    var personel = $("#form").serializeObject();

                    Post('/Personel/Kaydet',
                        { personel: personel },
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
                            //Complete
                            setTimeout(function () { $('#kt_datatable').KTDatatable('reload'); }, 3000)
                        },
                        "json");
                } else {

                }
            });
        }

    }

    var detayForm = function () {

        $(".navi-link").click(function (event) {

            event.preventDefault();
            $("#formArea").empty();
            $(".navi-link").removeClass("active");

            $(this).addClass("active");
            $("#cardTitle").html($(this).attr("cardLTitle"));
            $("#cardDescription").html($(this).attr("cardDescription"));

            $(".btnKaydet").attr("id", $(this).attr("btnId"));

            var form = $(this).attr("form");
            var id = $("#PersonelId").val();

            Post('/Personel/' + form,
                { id: id },
                function (response) {
                    $("#formArea").empty().html(response);
                },
                function (x, y, z) {
                    //Error
                },
                function () {
                    //BeforeSend
                },
                function () {
                    //Complete
                },
                "html");

        });

        var validation;


        $('#personekEkle').on('click', function (e) {
            e.preventDefault();
            Post('/Personel/Form',
                {},
                function (response) {
                    bootbox.dialog({
                        title: 'Personel Formu',
                        message: response,
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
                    //Complete
                },
                "html");
        });


        $(document).on("click", "#btnPersonelKaydet", function (e) {
            e.preventDefault();

            var form = $(".formSerialize");
            var formId = $(form).attr("id");

            validation = FormValidation.formValidation(
                KTUtil.getById(formId),
                {
                    fields: {
                        Adi: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }
                            }
                        },
                        Unvan: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }
                            }
                        },
                        Telefon: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }
                            }
                        },
                        DepartmanId: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }

                            }
                        },
                        PersonelTipId: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' }

                            }
                        },
                        Eposta: {
                            validators: {
                                notEmpty: { message: 'Bos Birakilamaz' },
                                emailAddress: { message: 'Gecerli Bir Eposta Adresi Girin', },
                            }
                        }
                    },
                    plugins: {
                        trigger: new FormValidation.plugins.Trigger(),
                        submitButton: new FormValidation.plugins.SubmitButton(),
                        //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                        bootstrap: new FormValidation.plugins.Bootstrap()
                    }
                }
            );

            validation.validate().then(function (status) {
                if (status == 'Valid') {

                    var personel = $(form).serializeObject();

                    Post('/Personel/Kaydet',
                        { personel: personel },
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
                            //Complete
                        },
                        "json");
                } else {

                }
            });

        });


        $(document).on("click", "#btnParolaKaydet", function (e) {
            e.preventDefault();


            var form = document.getElementById('parolaForm');
            const fv = FormValidation.formValidation(form, {
                fields: {
                    Parola: {
                        validators: {
                            notEmpty: {
                                message: 'Boş Bırakılamaz',
                            },
                        },
                    },
                    YeniParola: {
                        validators: {
                            notEmpty: {
                                message: 'Boş Bırakılamaz',
                            },
                        },
                    },
                    YeniParolaTekrar: {
                        validators: {
                            identical: {
                                compare: function () {
                                    return form.querySelector('[name="YeniParola"]').value;
                                },
                                message: 'Girdiğiniz değerler aynı olmalıdır',
                            },
                        },
                    },
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    icon: new FormValidation.plugins.Icon({
                        valid: 'fa fa-check',
                        invalid: 'fa fa-times',
                        validating: 'fa fa-refresh',
                    }),
                },
            });

            fv.validate().then(function (status) {
                if (status == 'Valid') {

                    var personel = $(form).serializeObject();
                    Post('/Personel/ParolaGuncelle',
                        { personel: personel },
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
                            //Complete
                        },
                        "json");
                } else {

                }
            });
            // Revalidate the confirmation password when changing the password



            form.querySelector('[name="YeniParola"]').addEventListener('input', function () {
                fv.revalidateField('YeniParolaTekrar');
            });

        });


        $(document).on('click', "#btnYetkiKaydet", function () {

            var personelId = parseInt($("#PersonelId").val());
            var id = document.getElementsByClassName("kid");
            var ids = [];

            $.each(id, function (index, dom) {
                if ($(dom).is(":checked")) {
                    ids.push($(dom).val());
                }
            });


            Post('/Personel/YetkiKaydet',
                { id: personelId, ids: ids },
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
                function (response) {
                    //Complete
                },
                "json");


        });



    }

    return {
        // public functions
        ListInit: function () {
            handleList();
        },
        FormInit: function () {
            handleForm();
        },
        DetayInit: function () {
            detayForm();
        }
    };
}();

jQuery(document).ready(function () {
    Personel.ListInit();
    Personel.FormInit();
});
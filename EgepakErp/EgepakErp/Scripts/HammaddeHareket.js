var HammaddeHareket = function () {

    function Kaydet() {
        //var validation = ValidateForm.IsValid("hammaddeHareketForm", ValidationFields.HammaddeHareketFormFields());
        //validation.validate().then(function (status) {
        //    if (status == 'Valid') {

              

        //    } else {
        //        return false;
        //    }
        //});

        var hammaddeHareket = $("#hammaddeHareketForm").serializeObject();

        var keys = Object.keys(hammaddeHareket);
        var include = keys.slice(1, hammaddeHareket.length);
        hammaddeHareket.Include = include;

        console.log("hammaddeHareket", hammaddeHareket);

        Post('/HammaddeHareket/Kaydet',
            { hammaddeHareket: hammaddeHareket },
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
                bootbox.hideAll();
                setTimeout(function () { $('#kt_datatable').KTDatatable('reload'); }, 3000)
            },
            "json");
    }

    

    function DurumGuncelle(id) {
        Post('/HammaddeHareket/DurumGuncelle',
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
                setTimeout(function () { $('#kt_datatable').KTDatatable('reload'); }, 2000)
            },
            "json");
    }

    function GetForm(formUrl, id, appendto) {
        Post(formUrl,
            { id: id },
            function (response) {
                $("#" + appendto).empty().html(response);
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
    }

    function GetFilterForm(formUrl, appendto) {
        Post(formUrl,
            {},
            function (response) {

                $("#" + appendto).empty().html(response);
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
    }

    var handleEvent = function () {

        $(document).on("click", "[event='hammaddeHareketKaydet']", function (evet) {
            event.preventDefault();
            Kaydet();
        });

        $(document).on("click", "[event='durum']", function (evet) {
            event.preventDefault();
            var id = $(this).attr("id");

            bootbox.dialog({
                title: 'Personel Durum Güncelle',
                message: Global.cardTemplate('İşleme Devam Etmek İstiyor musunuz?'),
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

        $(document).on("click", "[event='hammaddeHareketFormPopup']", function (e) {
            e.preventDefault();
            var title = $(this).attr("formTitle");
            var id = $(this).attr("id");
            Post('/HammaddeHareket/Form',
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
                },
                "html");
        });
       


    

        $(".navi-link").click(function (event) {
            event.preventDefault();

            $(".navi-link").removeClass("active");
            $(this).addClass("active");

            $("#cardTitle").html($(this).attr("cardLTitle"));
            $("#cardDescription").html($(this).attr("cardDescription"));

            var formUrl = $(this).attr("form");
            var displayType = $(this).attr("displayType");
            var id = $(this).attr("id");

            $(".eventButton").attr("event", $(this).attr("triggerEvent"));
            $(".eventButton").text($(this).attr("triggerText"));

            if (displayType == "form") {

                $("#kt_datatable").empty();
                $("#filterForm").empty();

                GetForm(formUrl, id, "formArea");

            } else if (displayType == "list") {

                $("#formArea").empty();

                var coloumns = $(this).attr("coloumns");
                var filterForm = $(this).attr("filterForm");
                if (filterForm !== "") {
                    GetFilterForm(filterForm, "filterForm");
                }

                $('#kt_datatable').KTDatatable('destroy');

                var params = { hammaddeHareketId: id };
                DtInit("kt_datatable", formUrl, DtColums.GetColoums(coloumns), params)
            }

        });

    }

    var DtInit = function (domId, url, columns, params) {

        var datatable = $('#' + domId).KTDatatable({
            // datasource definition
            data: {
                type: 'remote',
                source: {
                    read: {
                        //url: HOST_URL + '/api/datatables/demos/default.php',
                        url: url,
                        params: params
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
            columns: columns

        });

        $(document).on("change", ".form-control", function (e) {
            e.preventDefault();
            datatable.search($(this).val().toLowerCase(), $(this).attr("id"));
        });


    }

    return {

        EventInit: function () {
            handleEvent();
        },

        DtInit: function (domId, url, columns) {
            DtInit(domId, url, columns);
        }
    };
}();


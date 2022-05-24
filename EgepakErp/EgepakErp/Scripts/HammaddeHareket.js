var HammaddeHareket = function () {


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

                var params = { HammaddeId: id };
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


var Dt = function () {

    var datatable;
    var sayac = 0;
    var DtInit = function (domId, url, columns, params,rowSelectionForm) {

        datatable = $('#' + domId).KTDatatable({
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
                scroll: true, // enable/disable datatable scroll both horizontal and vertical when needed.
                customScrollbar: true,
                footer: false, // display/hide footer
                height:600
            },

            //column sorting
            sortable: true,

            //enable pagination
            pagination: true,

            //columns definition
            columns: columns,

            //extensions : {
            //    // boolean or object (extension options)
            //    checkbox: false,
            //}

        });

        datatable.on(
            'datatable-on-check datatable-on-uncheck',
            function (e) {

                var checkedNodes = datatable.rows('.datatable-row-active').nodes();
                var count = checkedNodes.length;
                var idList = [];
                var ids = datatable.rows('.datatable-row-active').
                    nodes().
                    find('.checkbox > [type="checkbox"]').
                    map(function (i, chk) {
                        idList.push($(chk).val());
                        return $(chk).val();
                    });

                console.log(ids);

                console.log("checkedNodes", checkedNodes);
                console.log("ids", ids);
                console.log("idList", idList);

                 
                if (count > 0) {
                    Form.GetFilterFormWithParams(rowSelectionForm, { ids: idList} ,"kt_datatable_group_action_form");
                } else {
                    $('#kt_datatable_group_action_form').html('');
                }
            });

        $(document).on("change", ".filterFormControl", function (e) {
            sayac++;
            e.preventDefault();
            var val = $(this).val();
            var id = $(this).attr("id");
            console.log("val", val);
            console.log("id", id);
            console.log("sayac", sayac);
            if (id !=undefined) {
                datatable.search(val, id);
            }
           
        });
    }
    var DtDestroybyId = function (domId) {
        console.log("DtDestroybyId", domId);
        $('#' + domId).KTDatatable('destroy');
    }
    var DtDestroy = function () {
        console.log("DtDestroy",$(datatable));
        $(datatable).KTDatatable('destroy');
    }
    var triggerSearch = function (val, id) {
        datatable.search(val, id);
    }

    return {

        DtInit: function (domId, url, columns, params, rowSelectionForm) {
            DtInit(domId, url, columns, params, rowSelectionForm);
        },
        DtDestroybyId: function (domId) {
            DtDestroybyId(domId);
        },
        DtDestroy: function () {
            DtDestroy();
        },
        DataTable: function () {
            return datatable;
        },
        TriggerSearch: function (val, id) {
            return triggerSearch(val, id)
        }
    };
}();
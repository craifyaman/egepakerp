var SelectPickerRemote = function () {

    function formatRepo(repo) {

        console.log("formatRepo", repo);
        if (repo.loading) return "Getiriliyor";
        var markup = "<div class='select2-result-repository clearfix'>" +
            "<div class='select2-result-repository__meta'>" +
            "<div class='select2-result-repository__title'>" + repo.text + "</div>";
        if (repo.cari) {
            markup += "<div class='select2-result-repository__description'>Bağlı Olduğu Cari: " + repo.cari + "</div>";
        }
        return markup;
    }

    function formatRepoSelection(repo) {
        //return repo.full_name || repo.text;
        return repo.text;
    }

    var init = function () {

        $.each($(".select2"), function (index, item) {

            var url = $(item).attr("url");
            var placeHolder = $(item).attr("placeHolder");

            $(item).select2({
                placeholder: placeHolder,
                allowClear: true,
                ajax: {
                    url: url,
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            q: params.term // search term
                            //page: params.page
                        };
                    },
                    processResults: function (data, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used
                        //params.page = params.page || 1;
                        params.page = 1;

                        return {
                            results: data.items
                            //pagination: {
                            //    more: (params.page * 30) < data.total_count
                            //}
                        };
                    },
                    cache: true
                },
                escapeMarkup: function (markup) {
                    return markup;
                }, // let our custom formatter work
                minimumInputLength: 3,
                templateResult: formatRepo, // omitted for brevity, see the source of this page
                templateSelection: formatRepoSelection // omitted for brevity, see the source of this page
            });

            
        })

    };

    return {
        // public functions
        init: function () {
            init();
        }
    };
}();

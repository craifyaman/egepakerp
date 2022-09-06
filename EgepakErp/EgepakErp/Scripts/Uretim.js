var Uretim = function () {

    function TimeLineOlusturEnjeksiyon(UretimEmirleri) {

        var options = {
            stack: true,
            maxHeight: 640,
            horizontalScroll: true,
            verticalScroll: true,
            zoomKey: "ctrlKey",
            orientation: {
                axis: "both",
                item: "top",
            },
        };

        var groups = new vis.DataSet();
        var items = new vis.DataSet();

        var count = UretimEmirleri.length;

        var makineler = [];

        $.each(UretimEmirleri, function (i, v) {
            var id = v.MakineId;
            makineler.push(id);
        });

        makineler = Array.from(new Set(makineler));

        console.log("id array :" + makineler);
        for (var i = 0; i < makineler.length; i++) {
            groups.add({
                id: i,
                content: "Makina " + makineler[i],
                order: i,
            });
        }

        for (var i = 0; i < count; i++) {
            var start = UretimEmirleri[i].Baslangic;
            var end = UretimEmirleri[i].Bitis;
            var classList = UretimEmirleri[i].ClassList;
            const content = `<a class="text-center text-dark uretimemir " event="uretimEmirDetay" UretimEmirId="` + UretimEmirleri[i].UretimEmirId + `">` + UretimEmirleri[i].KalipAd + `</a>`;
            const _content = document.createElement("div");
            _content.innerHTML = content;

            items.add({
                id: UretimEmirleri[i].UretimEmirId,
                group: $.inArray(UretimEmirleri[i].MakineId, makineler),
                start: start,
                end: end,
                type: "range",
                title: UretimEmirleri[i].Durum,
                content: _content,
                className: classList,

            });
        }

        // create a Timeline
        var container = document.getElementById("kt_docs_vistimeline_group");
        var timeline = new vis.Timeline(container, items, groups, options);
        //timeline = new vis.Timeline(container, null, options);
        timeline.setGroups(groups);
        timeline.setItems(items);


        function debounce(func, wait = 100) {
            let timeout;
            return function (...args) {
                clearTimeout(timeout);
                timeout = setTimeout(() => {
                    func.apply(this, args);
                }, wait);
            };
        }

        let groupFocus = (e) => {
            let vGroups = timeline.getVisibleGroups();
            let vItems = vGroups.reduce((res, groupId) => {
                let group = timeline.itemSet.groups[groupId];
                if (group.items) {
                    res = res.concat(Object.keys(group.items));
                }
                return res;
            }, []);
            timeline.focus(vItems);
        };
        timeline.on("scroll", debounce(groupFocus, 200));
    }

    function TimeLineOlustur(liste, divId) {
        debugger;
        var options = {
            stack: true,
            maxHeight: 640,
            horizontalScroll: true,
            verticalScroll: true,
            zoomKey: "ctrlKey",
            orientation: {
                axis: "both",
                item: "top",
            },
        };

        var groups = new vis.DataSet();
        var items = new vis.DataSet();

        var count = liste.length;

        var siparisler = [];
        var fixListe = [];

        $.each(liste, function (i, v) {
            var id = v.SiparisId;
            fixListe.push(id);
        });

        $.each(liste, function (i, v) {
            var id = v.SiparisId;
            siparisler.push(v);
        });

        fixListe = Array.from(new Set(fixListe));
        //siparisler = Array.from(new Set(siparisler));

        console.log("sipariş array :" + siparisler);

        for (var i = 0; i < fixListe.length; i++) {
            groups.add({
                id: i,
                content: siparisler[i].SiparisAdi,
                order: i,
            });
        }

        for (var i = 0; i < count; i++) {
            var start = liste[i].Baslangic;
            var end = liste[i].Bitis;
            var classList = liste[i].ClassList;
            const content = `<a class="text-center text-dark uretimemir " event="uretimEmirDetay" UretimEmirId="` + liste[i].UretimEmirId + `">` + liste[i].KalipAd + `</a>`;
            const _content = document.createElement("div");
            _content.innerHTML = content;

            items.add({
                id: liste[i].UretimEmirId,
                group: $.inArray(liste[i].SiparisId, fixListe),
                start: start,
                end: end,
                type: "range",
                title: liste[i].Durum,
                content: _content,

            });
        }

        // create a Timeline
        var container = document.getElementById(divId);
        var timeline = new vis.Timeline(container, items, groups, options);
        //timeline = new vis.Timeline(container, null, options);
        timeline.setGroups(groups);
        timeline.setItems(items);


        function debounce(func, wait = 100) {
            let timeout;
            return function (...args) {
                clearTimeout(timeout);
                timeout = setTimeout(() => {
                    func.apply(this, args);
                }, wait);
            };
        }

        let groupFocus = (e) => {
            let vGroups = timeline.getVisibleGroups();
            let vItems = vGroups.reduce((res, groupId) => {
                let group = timeline.itemSet.groups[groupId];
                if (group.items) {
                    res = res.concat(Object.keys(group.items));
                }
                return res;
            }, []);
            timeline.focus(vItems);
        };
        timeline.on("scroll", debounce(groupFocus, 200));
    }

    function PostToTimeLine(url, divId) {
        Post(url,
            {},
            function (response) {
                var liste = response.Data;
                TimeLineOlustur(liste, divId);
            },
            function (x, y, z) {
                //Error
            },
            function () {
                //BeforeSend
            },
            function () {

            },
            "json");
    }

    var handleEvent = function () {
        debugger;

        Post("/UretimEmir/GetAll?type=enjeksiyon",
            {},
            function (response) {
                debugger;
                var UretimEmirleri = response.Data;
                TimeLineOlusturEnjeksiyon(UretimEmirleri);
            },
            function (x, y, z) {
                //Error
            },
            function () {
                //BeforeSend
            },
            function () {

            },
            "json");

        PostToTimeLine("/UretimEmir/GetAll?type=sicakbaski", "kt_docs_vistimeline_group_SicakBaski");
        PostToTimeLine("/UretimEmir/GetAll?type=sprey", "kt_docs_vistimeline_group_Sprey");
        PostToTimeLine("/UretimEmir/GetAll?type=montaj", "kt_docs_vistimeline_group_Montaj");
        PostToTimeLine("/UretimEmir/GetAll?type=metalize", "kt_docs_vistimeline_group_Metalize");
        


        $(document).on("change", "#SiparisId", function (event) {
            event.preventDefault();
            var siparisId = $(this).val();
            Post("/uretimemir/SiparisKalipBySiparis",
                { siparisId: siparisId },
                function (response) {
                    $("#SiparisKalipList").empty().html(response);
                    $("#SiparisKalipList").show();

                },
                function (x, y, z) {
                    //Error
                },
                function () {
                    //BeforeSend
                },
                function () {
                },
                "html");
        });

        $(document).on("click", "[event='UretimEmirKaydet']", function (event) {
            event.preventDefault();
            UretimEmir.Kaydet();
        });

        $(document).on("click", "[event='uretimEmirDetay']", function (event) {
            event.preventDefault();
            debugger;
            var id = $(this).attr("uretimemirid");
            UretimEmir.UretimEmirForm(id);
        });

        $(document).on("click", "[event='UretimEmirEkle']", function (event) {
            event.preventDefault();
            debugger;
            UretimEmir.UretimEmirForm(0);
        });

        $(document).on("click", "[event='AksiyonEkle']", function (e) {
            debugger;
            e.preventDefault();
            var id = 0;
            var uretimEmirId = $(this).attr("UretimEmirId");
            Aksiyon.AksiyonForm(id, uretimEmirId);
        });
    }



    return {

        EventInit: function () {
            handleEvent();
        }
    };
}();


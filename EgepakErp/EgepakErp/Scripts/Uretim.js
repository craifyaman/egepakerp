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

    function DepoyaAktarCoklu() {
        debugger;
        var SiparisKalipDepoDto = function () {
            this.siparisKalipId;//int
            this.SiparisId;//int
            this.UretimEmirId;//int
            this.Adet;//int
            this.Yer;//string
        }

        var SiparisKalipDepoDtoList = [];

        var array = $(".MontajliKalip").sort();

        array.each(function (index, value) {
            var input = value;
            var dto = new SiparisKalipDepoDto();
            dto.SiparisKalipId = $(input).attr("SiparisKalipId");
            dto.UretimEmirId = $(input).attr("UretimEmirId");
            dto.SiparisId = $(input).attr("SiparisId");
            dto.Adet = $(input).attr("Adet");
            dto.Yer = $(input).attr("Yer");

            if ($(input).prop('checked') == true) {
                SiparisKalipDepoDtoList.push(dto);
            }
        });

        console.log(SiparisKalipDepoDtoList);

        Post("stokhareket/DepoyaAktarCoklu",
            { liste: SiparisKalipDepoDtoList },
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
                location.reload();
            },
            "json");
    }

    function DepoyaAktarTekli(SiparisKalipId, SiparisId, Adet, Yer, UretimEmirId) {
        debugger;
        Post("stokhareket/DepoyaAktarTekli",
            { SiparisKalipId: SiparisKalipId, SiparisId: SiparisId, Adet: Adet, Yer: Yer, UretimEmirId: UretimEmirId },
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
                location.reload();
            },
            "json");
    }

    var handleEvent = function () {

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
/*
        PostToTimeLine("/UretimEmir/GetAll?type=sicakbaski", "kt_docs_vistimeline_group_SicakBaski");
        PostToTimeLine("/UretimEmir/GetAll?type=sprey", "kt_docs_vistimeline_group_Sprey");
        PostToTimeLine("/UretimEmir/GetAll?type=montaj", "kt_docs_vistimeline_group_Montaj");
        PostToTimeLine("/UretimEmir/GetAll?type=metalize", "kt_docs_vistimeline_group_Metalize");

*/

        $(document).on("change", "#SiparisId", function (event) {
            debugger;
            event.preventDefault();
            var siparisId = $(this).val();
            var adet = $("#SiparisId option:selected").attr("Adet");
            $("#SiparisAdet").val(adet);
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

       

        $(document).on("click", "[event='MontajliKalipDepoEkleme']", function (e) {
            debugger;
            e.preventDefault();
            DepoyaAktarCoklu();
        });

        $(document).on("click", ".webviewer", function (e) {
            debugger;
            e.preventDefault();
            var url = $(this).attr("pdfurl");
            CustomWebViewer.View(url);
        });

        $(document).on("change", "#UretimEmirDurumId", function (e) {
            debugger;
            var id = $(this).val();
            if (id == 7) {
                $("#evMontajDiv").show();
            }
            else {
                $("#evMontajDiv").hide();
            }

        });

        $(document).on("click", "[event='UretimAksiyonFormPopup']", function (e) {
            debugger;
            e.preventDefault();
            var UretimEmirId = $(this).attr("UretimEmirId"); 
            var id = $(this).attr("id");
            var MakineId = $("#MakineId").val();
            UretimAksiyon.Form(id, UretimEmirId, MakineId);
        });

        $(document).on("click", "[event='AksiyonEkle']", function (e) {
            debugger;
            e.preventDefault();
            var UretimEmirId = $(this).attr("UretimEmirId");
            var AksiyonType = $(this).attr("AksiyonType");
            var UretimEmirDurumId = $(this).attr("UretimEmirDurumId");
            var id = $(this).attr("id");
            Aksiyon.AksiyonForm(id, UretimEmirId, AksiyonType, UretimEmirDurumId);
        });

        //$(document).on("click", "[event='AksiyonEkle']", function (e) {
        //    debugger;
        //    e.preventDefault();
        //    var id = 0;
        //    var uretimEmirId = $(this).attr("UretimEmirId");
        //    Aksiyon.AksiyonForm(id, uretimEmirId);
        //});
        
    }

    var handleEvent2 = function () {

        $(document).on("change", ".SiparisAdetInput", function (e) {
            e.preventDefault();
            debugger;
            var siparisKalipId = $(this).attr("SiparisKalipId");
            var targetInput = $(".MontajliKalip[SiparisKalipId='" + siparisKalipId + "']");
            targetInput.attr("Adet", $(this).val());
        });

        $(document).on("change", ".YerInput", function (e) {
            e.preventDefault();
            debugger;
            var siparisKalipId = $(this).attr("SiparisKalipId");
            var targetInput = $(".MontajliKalip[SiparisKalipId='" + siparisKalipId + "']");
            targetInput.attr("Yer", $(this).val());
        });


        $(document).on("click", "[event='MontajliKalipDepoEkleme']", function (e) {
            debugger;
            e.preventDefault();
            if ($(this).attr("disabled") != "disabled") {
                $(this).attr("disabled", "disabled");
                DepoyaAktarCoklu();
            }
        });

        $(document).on("click", "[event='DepoAktarTekli']", function (e) {
            debugger;
            e.preventDefault();
            if ($(this).attr("disabled") != "disabled") {

                $(this).attr("disabled", "disabled");
                var SiparisKalipId = $(this).attr("SiparisKalipId");
                var UretimEmirId = $(this).attr("UretimEmirId");
                var SiparisId = $(this).attr("SiparisId");
                var Adet = $(".SiparisAdetInput[SiparisKalipId='" + SiparisKalipId + "']").val();
                var Yer = $(".YerInput[SiparisKalipId='" + SiparisKalipId + "']").val();
                DepoyaAktarTekli(SiparisKalipId, SiparisId, Adet, Yer, UretimEmirId);
            }
            else {
                toastr.warning("ürün zaten depoya eklendi");
            }
        });


    }

    return {

        EventInit: function () {
            handleEvent();
        },
        EventInit2: function () {
            handleEvent2();
        }
    };
}();


var Uretim = function () {

    function UretimEmirIslem(UretimEmirleri) {

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
            const content = `<a class="text-center text-dark uretimemir" event="uretimEmirDetay" UretimEmirId="` + UretimEmirleri[i].UretimEmirId + `">` + UretimEmirleri[i].KalipAd + `</a>`; 
            const _content = document.createElement("div");
            _content.innerHTML = content;

            items.add({
                id: UretimEmirleri[i].UretimEmirId,
                group: $.inArray(UretimEmirleri[i].MakineId, makineler),
                start: start,
                end: end,
                type: "range",
                content: _content,
                
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
        // Enabling the next line leads to a continuous since calling focus might scroll vertically even if it shouldn't
        // this.timeline.on("scrollSide", debounce(groupFocus, 200))

        // Handle button click
        //const button = document.getElementById('kt_docs_vistimeline_group_button');
        //button.addEventListener('click', e => {
        //    e.preventDefault();

        //    var a = timeline.getVisibleGroups();
        //    document.getElementById("visibleGroupsContainer").innerHTML = "";
        //    document.getElementById("visibleGroupsContainer").innerHTML += a;
        //});
    }

    function GetUrunEmirleri() {
        var liste;
        Post("/UretimEmir/GetAll",
            {},
            function (response) {
                liste = response.Data;
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

        return liste;
    }



    //function UretimEmirForm() {
    //    Post("/UretimEmir/Form",
    //        {},
    //        function (response) {
    //            console.log(response);
    //            $("#UretimEmirDiv").html(response);
    //        },
    //        function (x, y, z) {
    //            //Error
    //            toastr.error("form yüklenemedi")
    //        },
    //        function () {
    //            //BeforeSend
    //        },
    //        function () {
    //        },
    //        "html");
    //}


    var handleEvent = function () {

        Post("/UretimEmir/GetAll",
            {},
            function (response) {
                var UretimEmirleri = response.Data;
                UretimEmirIslem(UretimEmirleri);
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

    }



    return {

        EventInit: function () {
            handleEvent();
        }
    };
}();


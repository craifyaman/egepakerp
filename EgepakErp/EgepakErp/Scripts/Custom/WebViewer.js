var CustomWebViewer = function () {

    function View(url) {
        debugger;
        var box = bootbox.dialog({
            title: "pdf okuyucu",
            message: `<div id='viewer'></div>`,
            size: 'large',
            buttons: {
                cancel: {
                    label: "Kapat",
                    className: 'btn-danger',
                    callback: function () {
                        box.modal('hide');
                    }
                }
            }
        });


        WebViewer({
            path: 'WebViewer/lib', // path to the PDF.js Express'lib' folder on your server
            licenseKey: 'Insert commercial license key here after purchase',
            initialDoc: url,
            // initialDoc: '/path/to/my/file.pdf',  // You can also use documents on your server
        }, document.getElementById('viewer'))
            .then(instance => {
                instance.UI.setLanguage('tr');
                const docViewer = instance.docViewer;
                const annotManager = instance.annotManager;
                // call methods from instance, docViewer and annotManager as needed

                // you can also access major namespaces from the instance as follows:
                // const Tools = instance.Tools;
                // const Annotations = instance.Annotations;
                docViewer.on('documentLoaded', () => {
                    // call methods relating to the loaded document
                });
            });
    }
    return {
        // public functions
        View: function (url) {
            return View(url);
        }
    };
}();


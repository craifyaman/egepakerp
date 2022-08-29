
function UploadImage(inputId, Url, TargetDirectory,TargetInputId) {
    debugger;
    // Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var fileUpload = $("#" + inputId).get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object  
        fileData.append('file', 'file');

        $.ajax({
            url: Url,
            type: "POST",
            dataType:"json",
            headers: { 'TargetDirectory': TargetDirectory },
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: function (res) {
                if (res.Success) {
                    $("#" + TargetInputId).val(res.Data);
                    toastr.success(res.Description)
                } else {
                    toastr.error(res.Description)
                }
                
            },
            error: function () {
                toastr.error("dosya alınamadı.")
            }
        });
    }
}

function UploadImageCustom(inputId, Url, TargetDirectory,fnSuccess,fnError) {
    debugger;
    // Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var fileUpload = $("#" + inputId).get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object  
        fileData.append('file', 'file');

        $.ajax({
            url: Url,
            type: "POST",
            dataType: "json",
            headers: { 'TargetDirectory': TargetDirectory },
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: fnSuccess,
            error: fnError
        });
    }
}
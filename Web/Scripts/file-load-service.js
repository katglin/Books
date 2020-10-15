$(document).ready(function () {
    // upload and save book title
    $(".book-title-input").change(function () {
        var row = $(this).parent().parent().parent()[0];
        var img = $(row).find("img");
        var bookId = $(row).find("td")[0].innerText;

        var formData = new FormData();
        var totalFiles = this.files.length;
        if (totalFiles > 0) {
            var file = this.files[0];
            if (file.size > 512000) { // 500 KB
                alert('Sorry, only files up to 500 KB are allowed');
                return;
            }
            formData.append("BookTitleInput", file);
            formData.append("BookId", bookId);
        }
        $.ajax({
            type: "POST",
            url: '/Book/UploadImageAsync/' + bookId,
            data: formData,
            dataType: "text",
            contentType: false,
            processData: false,
            success: function (url) {
                if (url && img) {
                    img.attr('src', url);
                }
            },
            error: function () {
                alert('File was not saved')
            }
        });
    });

    // upload and save book attachment
    $(".book-attachment-input").change(function () {
        var row = $(this).parent().parent()[0];
        var bookId = $(row).find("td")[0].innerText;
        var attachments = $(row).find(".attachments-list");
        var fileName = this.files[0].name;

        var formData = new FormData();
        var totalFiles = this.files.length;
        if (totalFiles > 0) {
            var file = this.files[0];
            if (file.size > 512000) { // 500 KB
                alert('Sorry, only files up to 500 KB are allowed');
                return;
            }
            formData.append("BookAttachmentInput", file);
            formData.append("BookId", bookId);
        }
        $.ajax({
            type: "POST",
            url: '/Attachment/UploadAttachmentAsync/' + bookId,
            data: formData,
            dataType: "text",
            contentType: false,
            processData: false,
            success: function (res) {
                if (res) {
                    var data = JSON.parse(res);
                    attachments.append(
                        '<a data-attachment="' + data.FileS3Key+'" class="attachment-file attachment-info" href="' + data.FileUrl + '">' + fileName + '</a>'+
                        '<span data-attachment="' + data.FileS3Key+'" class="glyphicon glyphicon-remove remove-attachment attachment-info"></span></br>'
                    );
                    var files = $(row).find(".attachment-file");
                    if (files.length > 2) {
                        var uploadBtn = $(row).find(".book-attachment-input");
                        $(uploadBtn).remove();
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('File was not saved')
            }
        });
    });

    // download attachment by click on its name
    $(document).on('click', '.attachment-file', function (e) {
        e.preventDefault();
        e.stopPropagation();
        var link = $(this)[0];

        fetch(link.href).then(function (res) {
            return res.blob().then((b) => {
                var a = document.createElement("a");
                a.href = URL.createObjectURL(b);
                a.setAttribute("download", link.innerText);
                a.click();
            });
        });
    })

    // remove attachment by click on remove icon
    $(document).on('click', '.remove-attachment', function (e) {
        var fileKey = $(this)[0].attributes["data-attachment"].value;
        var currentIcon = this;
        $.ajax({
            type: "POST",
            url: "/Attachment/DeleteAttachmentAsync",
            data: { fileKey: fileKey },
            success: function () {
                var row = $(currentIcon).parent().parent()[0];
                var attachments = $(row).find(".attachments-list");
                var links = $(attachments).find(".attachment-info");
                links.each(function (i) {
                    if ($(this)[0].attributes["data-attachment"].value == fileKey) {
                        $(this).remove();
                    }
                });
            },
            error: function (data) {
                alert('Sorry, failed to delete attachment. Try again later');
            }
        });
    })
});